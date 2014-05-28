Option Explicit On
Option Strict On

Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Admin
Imports CKG.Base.Kernel.Security.Crypto
Imports CKG.Base.Kernel.Security

Partial Public Class Abrufgruende
    Inherits System.Web.UI.Page

    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private Connection As New SqlClient.SqlConnection()

#Region "Properties"

    Private Property Refferer() As String
        Get
            If Not Session.Item(Me.Request.Url.LocalPath & "Refferer") Is Nothing Then
                Return Session.Item(Me.Request.Url.LocalPath & "Refferer").ToString()
            Else : Return Nothing
            End If
        End Get
        Set(ByVal value As String)
            Session.Item(Me.Request.Url.LocalPath & "Refferer") = value
        End Set
    End Property
#End Region



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Hier Benutzercode zur Seiteninitialisierung einfügen

        m_User = GetUser(Me)
        m_App = New Base.Kernel.Security.App(m_User) 'erzeugt ein App_objekt 
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        lblError.text = ""

        Connection.ConnectionString = ConfigurationManager.AppSettings("Connectionstring")

        If Not IsPostBack Then
            If Refferer Is Nothing Then
                If Not Me.Request.UrlReferrer Is Nothing Then
                    Refferer = Me.Request.UrlReferrer.ToString
                Else
                    Refferer = ""
                End If
            End If

            GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text

            FillCustomer(Connection)

            If Not ddlFilterCustomer.Enabled Then
                fillGrids()
            End If

        End If

    End Sub

    Private Sub FillCustomer(ByVal cn As SqlClient.SqlConnection)
        Dim dtCustomers As Kernel.CustomerList
        dtCustomers = New Kernel.CustomerList(m_User.Customer.AccountingArea, cn)

        dtCustomers.AddAllNone(True, True)
        With ddlFilterCustomer
            Dim dv As DataView = dtCustomers.DefaultView
            dv.Sort = "Customername"
            .DataSource = dv
            .DataTextField = "Customername"
            .DataValueField = "CustomerID"
            .DataBind()
            If m_User.HighestAdminLevel = AdminLevel.Master Or m_User.HighestAdminLevel = AdminLevel.FirstLevel Then
                .Items.FindByValue("0").Selected = True
            Else
                .Enabled = False
                .Items.FindByValue(m_User.Customer.CustomerId.ToString).Selected = True
            End If
        End With
    End Sub

    Private Sub responseBack()
        If Refferer = "" Then
            Dim strLinkPrefix As String = "/" & ConfigurationManager.AppSettings("WebAppPath") & "/"
            Response.Redirect(strLinkPrefix & "Start/Selection.aspx")
        Else
            Response.Redirect(Refferer)
        End If
    End Sub

    Private Sub lb_zurueck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_zurueck.Click
        responseBack()
    End Sub

    Private Sub ddlFilterCustomer_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFilterCustomer.SelectedIndexChanged
        fillGrids()
    End Sub


    Private Sub fillGrids()

        Dim dtTemp As DataTable = New Kernel.AbrufgruendeDT(Connection, ddlFilterCustomer.SelectedValue, "0", False, "temp")

        If Not dtTemp.Rows.Count = 0 Then
            gvTemporaer.DataSource = dtTemp
            gvTemporaer.DataBind()
        Else
            gvTemporaer.DataSource = Nothing
            gvTemporaer.DataBind()
        End If


        Dim dtEndg As DataTable = New Kernel.AbrufgruendeDT(Connection, ddlFilterCustomer.SelectedValue, "0", False, "endg")

        If Not dtEndg.Rows.Count = 0 Then
            gvEndgueltig.DataSource = dtEndg
            gvEndgueltig.DataBind()
        Else
            gvEndgueltig.DataSource = Nothing
            gvEndgueltig.DataBind()
        End If



    End Sub

    Protected Sub lbEintragen_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbEintragen.Click

        If Page.IsValid Then

            InsertIntoAbrufgrund()
            clearInput()
            fillGrids()
        End If

    End Sub

    Private Sub InsertIntoAbrufgrund()
        Try
            Dim cmd As New SqlClient.SqlCommand
            Connection.Open()
            cmd.Connection = Connection
            cmd.CommandType = CommandType.Text
            Dim SqlQuery As String
          
            SqlQuery = "INSERT INTO CustomerAbrufgruende" & _
             " (CustomerID, GroupID, AbrufTyp, WebBezeichnung, SapWert, MitZusatzText, Zusatzbemerkung, Eingeschraenkt) " & _
             "VALUES (" & ddlFilterCustomer.SelectedValue & ",0, '" & rblTyp.SelectedValue & "', '" & txtWebBezeichnung.Text & _
             "', '" & txtSapWert.Text & "'," & CInt(chkZusatzeingabe.Checked) & ",'" & txtZusatzbemerkung.Text & _
             "'," & ddlEingeschraenkt.SelectedValue & ")"
            cmd.CommandText = SqlQuery
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        Finally
            Connection.Close()
        End Try
    End Sub

    Private Sub DeleteFromAbrufgrund(ByVal SapWert As String, ByVal typ As String)
        Try
            Dim cmd As New SqlClient.SqlCommand
            Connection.Open()
            cmd.Connection = Connection
            cmd.CommandType = CommandType.Text
            Dim SqlQuery As String

            SqlQuery = "Delete From CustomerAbrufgruende " & _
                            "WHERE SapWert=" & SapWert & " AND AbrufTyp='" & typ & "' AND CustomerID=" & ddlFilterCustomer.SelectedValue & " ;"
            cmd.CommandText = SqlQuery
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        Finally
            Connection.Close()
        End Try
    End Sub


    Private Sub clearInput()
        txtSapWert.Text = ""
        txtWebBezeichnung.Text = ""
        txtZusatzbemerkung.Text = ""
        chkZusatzeingabe.Checked = False
    End Sub



    Private Function plausi() As Boolean
        If Not txtSapWert.Text.Replace(" ", "").Length = 0 AndAlso Not txtWebBezeichnung.Text.Replace(" ", "").Length = 0 Andalso Not rblTyp.SelectedIndex = -1 Then
            Return True
        End If
    End Function

   
    Private Sub gvEndgueltig_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvEndgueltig.RowCommand
        If e.CommandName = "loesch" Then
            DeleteFromAbrufgrund(e.CommandArgument.ToString, "endg")
            fillGrids()
        End If
    End Sub


    Private Sub gvTemporaer_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTemporaer.RowCommand
        If e.CommandName = "loesch" Then
            DeleteFromAbrufgrund(e.CommandArgument.ToString, "temp")
            fillGrids()
        End If
    End Sub
End Class

' ************************************************
' $History: Abrufgruende.aspx.vb $
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 7.06.10    Time: 11:22
' Updated in $/CKAG/admin
' ITA: 3824(HBr)
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 16.11.09   Time: 15:46
' Updated in $/CKAG/admin
' ITA: 3298
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 25.05.09   Time: 14:37
' Updated in $/CKAG/admin
' ITA 2839 nachbesserung
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 25.05.09   Time: 14:30
' Updated in $/CKAG/admin
' ITA 2839 testfertig
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 22.05.09   Time: 16:02
' Updated in $/CKAG/admin
' ITA 2839 unfertig
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 20.05.09   Time: 17:25
' Updated in $/CKAG/admin
' ITA 2839 unfertig
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 19.05.09   Time: 16:50
' Created in $/CKAG/admin
' ITA 2839 torso
' 
' ************************************************