Option Explicit On
Option Strict On

Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Admin
Imports CKG.Base.Kernel.Security.Crypto
Imports CKG.Base.Kernel.Security

Partial Public Class Abrufgruende
    Inherits System.Web.UI.Page
    Private m_App As App
    Private m_User As User
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
        m_User = GetUser(Me)
        AdminAuth(Me, m_User, AdminLevel.Master)
        m_App = New App(m_User)
        GetAppIDFromQueryString(Me)
        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

        Connection.ConnectionString = ConfigurationManager.AppSettings("Connectionstring")

        If Not IsPostBack Then
            If Refferer Is Nothing Then
                If Not Me.Request.UrlReferrer Is Nothing Then
                    Refferer = Me.Request.UrlReferrer.ToString
                Else
                    Refferer = ""
                End If
            End If

            FillCustomer(Connection)

            If Not ddlFilterCustomer.Enabled Then
                fillGrids()
            End If

        End If

    End Sub

    Private Sub ddlFilterCustomer_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFilterCustomer.SelectedIndexChanged
        fillGrids()
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

    Private Sub fillGrids()

        Dim dtTemp As DataTable = New Kernel.AbrufgruendeDT(Connection, ddlFilterCustomer.SelectedValue, "0", False, "temp")

        If Not dtTemp.Rows.Count = 0 Then
            gvTemporaer.DataSource = dtTemp
            gvTemporaer.DataBind()
            Result.Visible = True
            tab1.Visible = False
            lbEintragen.Visible = False
            Queryfooter.Visible = False
        Else
            gvTemporaer.DataSource = Nothing
            gvTemporaer.DataBind()
        End If


        Dim dtEndg As DataTable = New Kernel.AbrufgruendeDT(Connection, ddlFilterCustomer.SelectedValue, "0", False, "endg")

        If Not dtEndg.Rows.Count = 0 Then
            gvEndgueltig.DataSource = dtEndg
            gvEndgueltig.DataBind()
            Result.Visible = True
            tab1.Visible = False
        Else
            gvEndgueltig.DataSource = Nothing
            gvEndgueltig.DataBind()
        End If



    End Sub
    Private Function plausi() As Boolean
        If Not txtSapWert.Text.Replace(" ", "").Length = 0 AndAlso Not txtWebBezeichnung.Text.Replace(" ", "").Length = 0 AndAlso Not rblTyp.SelectedIndex = -1 Then
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

    Private Sub InsertIntoAbrufgrund()
        Try
            Dim cmd As New SqlClient.SqlCommand
            Connection.Open()
            cmd.Connection = Connection
            cmd.CommandType = CommandType.Text
            Dim SqlQuery As String

            SqlQuery = "INSERT INTO CustomerAbrufgruende" & _
             " (CustomerID, GroupID, AbrufTyp, WebBezeichnung, SapWert, MitZusatzText, Zusatzbemerkung) " & _
             "VALUES (" & ddlFilterCustomer.SelectedValue & ",0, '" & rblTyp.SelectedValue & "', '" & txtWebBezeichnung.Text & "', '" & txtSapWert.Text & "'," & CInt(chkZusatzeingabe.Checked) & ",'" & txtZusatzbemerkung.Text & "')"
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

    Protected Sub lbEintragen_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbEintragen.Click

        If Page.IsValid Then

            InsertIntoAbrufgrund()
            clearInput()
            fillGrids()
        End If

    End Sub

    Private Sub clearInput()
        txtSapWert.Text = ""
        txtWebBezeichnung.Text = ""
        txtZusatzbemerkung.Text = ""
        chkZusatzeingabe.Checked = False
    End Sub

    Protected Sub NewSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles NewSearch.Click
        tab1.Visible = Not tab1.Visible
        lbEintragen.Visible = Not lbEintragen.Visible
        Queryfooter.Visible = Not Queryfooter.Visible
    End Sub
End Class