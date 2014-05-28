Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security
Imports CKG.Services

Partial Public Class AktuellePflegeUebersicht
    Inherits System.Web.UI.Page

    Private m_User As User
    Private m_App As App
    Private mSeite As String
    Private mDatum As Date
    Private mObjAktuelles As Aktuelles

    Protected WithEvents GridNavigation1 As GridNavigation

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        AdminAuth(Me, m_User, AdminLevel.Organization)
        m_App = New App(m_User)
        GetAppIDFromQueryString(Me)


        GridNavigation1.setGridElment(grvUebersicht)

        lblError.Text = ""

        If Not IsPostBack Then
            FillCustomer()
        End If

    End Sub

    Private Sub grvUebersicht_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grvUebersicht.PageIndexChanging
        FillGridUebersicht(e.NewPageIndex)
    End Sub

    Private Sub GridNavigation1_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation1.PagerChanged
        FillGridUebersicht(PageIndex)

    End Sub

    Private Sub GridNavigation1_PageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillGridUebersicht(0)
    End Sub


    Sub grvUebersicht_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles grvUebersicht.RowCommand
        If e.CommandName = "Select" Then
            If Not Session("AppAktuelles") Is Nothing Then
                mObjAktuelles = Session("AppAktuelles")
                mObjAktuelles.Id = e.CommandArgument
                If mObjAktuelles.CustomerID = 0 Then
                    mObjAktuelles.CustomerID = mObjAktuelles.tblUebersicht.Select("ID=" & mObjAktuelles.Id)(0)("CustomerID")
                End If
                Response.Redirect("AktuellesPflege.aspx?AppID=" & Session("AppID").ToString)
            End If
        ElseIf e.CommandName = "Delete" Then
            Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            cn.Open()

            Dim da As New SqlClient.SqlCommand("DELETE FROM CustomerNews" & _
                                               "   WHERE ID = " & e.CommandArgument, cn)

            da.ExecuteNonQuery()

            FillGridUebersicht(grvUebersicht.PageIndex, "")
        End If
    End Sub

    Sub gridview_deletecommand(ByVal sender As Object, ByVal e As GridViewDeletedEventArgs) Handles grvUebersicht.RowDeleted

    End Sub

    Protected Sub btnSuche_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSuche.Click
        FillGridUebersicht(1)
    End Sub

    Protected Sub btnNew_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNew.Click
        mObjAktuelles = New Aktuelles()
        mObjAktuelles.clsError = ""
        mObjAktuelles.IsNew = True
        mObjAktuelles.CustomerID = ddlFilterCustomer.SelectedValue
        Session("AppAktuelles") = mObjAktuelles

        Response.Redirect("AktuellesPflege.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Protected Sub ddlFilterCustomer_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlFilterCustomer.SelectedIndexChanged
        FillGridUebersicht(grvUebersicht.PageIndex, "")
    End Sub
#End Region

#Region "Methods"
    Private Sub FillCustomer()
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()
        Try
            Dim dtCustomers As Kernel.CustomerList
            dtCustomers = New Kernel.CustomerList(m_User.Customer.AccountingArea, cn, True, False)

            Dim dv As DataView = dtCustomers.DefaultView
            dv.Sort = "Customername"
            Session.Add("AppCustomerListView", dv)

            With ddlFilterCustomer
                .DataSource = dv
                .DataTextField = "Customername"
                .DataValueField = "CustomerID"
                .DataBind()
            End With
        Catch ex As Exception
            lblError.Text = "Fehler beim Laden der Kunden"
        Finally
            cn.Close()
        End Try
    End Sub

    

    Private Sub FillGridUebersicht(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Dim intID As Integer
        intID = ddlFilterCustomer.SelectedValue
        mObjAktuelles = New Aktuelles()
        mObjAktuelles.clsError = ""
        mObjAktuelles.GetCustomerNews(intID, cn)
        If mObjAktuelles.clsError = "" Then
            If mObjAktuelles.tblUebersicht.Rows.Count > 0 Then
                With grvUebersicht
                    .PageIndex = intPageIndex
                    .DataSource = mObjAktuelles.tblUebersicht
                    .DataBind()
                End With
                Result.Visible = True
                Session("AppAktuelles") = mObjAktuelles
            Else
                lblError.Text = "Keine Daten für diese Selektion."
            End If
        Else
            lblError.Text = mObjAktuelles.clsError
        End If
    End Sub
#End Region

    Protected Sub grvUebersicht_RowDeleting(sender As Object, e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles grvUebersicht.RowDeleting

    End Sub
End Class