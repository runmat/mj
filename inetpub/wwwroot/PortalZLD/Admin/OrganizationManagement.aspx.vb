Imports CKG.Base.Kernel.Admin
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common

Partial Public Class OrganizationManagement
    Inherits System.Web.UI.Page

#Region " Membervariables "

    Private m_User As User
    Private m_App As App
    Protected WithEvents GridNavigation1 As Global.CKG.PortalZLD.GridNavigation

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)

        lblHead.Text = "Organisationsverwaltung"
        AdminAuth(Me, m_User, AdminLevel.Customer)
        GridNavigation1.setGridElment(dgSearchResult)
        Try
            m_App = New App(m_User)

            If Not IsPostBack Then
                lblError.Text = ""

                FillForm()
            End If
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "OrganizationManagement", "Page_Load", ex.ToString)

            lblError.Text = ex.ToString
            lblError.Visible = True
        End Try
    End Sub

#Region " Data and Function "

    Private Sub FillForm()
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()
        FillCustomer(cn)
        trEditUser.Visible = False
        trSearchResult.Visible = False

        If m_User.HighestAdminLevel = AdminLevel.Master Then
            ddlFilterCustomer.Visible = True
            txtFilterOrganizationName.Visible = True
            lblCustomer.Visible = False
            'wenn SuperUser und übergeordnete Firma
            If m_User.Customer.AccountingArea = -1 Then
                lnkAppManagement.Visible = True
            End If

        Else
            lnkArchivManagement.Visible = False
            lnkCustomerManagement.Visible = False
            lnkAppManagement.Visible = False
            If m_User.IsCustomerAdmin Then
                txtFilterOrganizationName.Visible = True
            Else
                lnkCustomerManagement.Visible = False
                OrganizationAdminMode()
            End If
        End If
        lblCustomer.Text = m_User.Customer.CustomerName
        lblOrganizationName.Visible = Not txtFilterOrganizationName.Visible
    End Sub

    Private Sub FillCustomer(ByVal cn As SqlClient.SqlConnection)
        Dim dtCustomers As Kernel.CustomerList
        dtCustomers = New Kernel.CustomerList(m_User.Customer.AccountingArea, cn, True, False)

        Dim dv As DataView = dtCustomers.DefaultView
        dv.Sort = "Customername"
        'm_context.Cache.Insert("myCustomerListView", dv, Nothing, DateTime.Now.AddMinutes(20), TimeSpan.Zero)
        Session("myCustomerListView") = dv
        With ddlFilterCustomer
            .DataSource = dv
            .DataTextField = "Customername"
            .DataValueField = "CustomerID"
            .DataBind()
            .Items.FindByValue(m_User.Customer.CustomerId.ToString).Selected = True
        End With
    End Sub

    Private Sub FillDataGrid()
        Dim strSort As String = "OrganizationID"
        If Not ViewState("ResultSort") Is Nothing Then
            strSort = ViewState("ResultSort").ToString
        End If
        FillDataGrid(strSort)
    End Sub

    Private Sub FillDataGrid(ByVal strSort As String)
        trSearchResult.Visible = True
        Dim dvOrganization As DataView

        'If Not m_context.Cache("myOrganizationListView") Is Nothing Then
        '    dvOrganization = CType(m_context.Cache("myOrganizationListView"), DataView)
        If Not Session("myOrganizationListView") Is Nothing Then
            dvOrganization = CType(Session("myOrganizationListView"), DataView)
        Else
            Dim dtOrganization As OrganizationList
            Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            cn.Open()

            Dim intTemp As Integer
            If m_User.HighestAdminLevel = AdminLevel.Master Then
                intTemp = CInt(ddlFilterCustomer.SelectedItem.Value)
            Else
                intTemp = m_User.Customer.CustomerId
            End If

            dtOrganization = New OrganizationList(txtFilterOrganizationName.Text, _
                                                intTemp, _
                                                cn, _
                                                m_User.Customer.AccountingArea)
            dvOrganization = dtOrganization.DefaultView
            'm_context.Cache.Insert("myOrganizationListView", dvOrganization, Nothing, DateTime.Now.AddMinutes(20), TimeSpan.Zero)
            Session("myOrganizationListView") = dvOrganization
        End If
        dvOrganization.Sort = strSort
        With dgSearchResult
            .DataSource = dvOrganization
            .DataBind()
        End With
    End Sub

    Private Function FillEdit(ByVal intOrganizationId As Integer) As Boolean
        SearchMode(False)
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()
        Dim _Organization As New Organization(intOrganizationId, cn)
        txtOrganizationID.Text = _Organization.OrganizationId.ToString
        txtOrganizationName.Text = _Organization.OrganizationName
        txtOrganizationReference.Text = _Organization.OrganizationReference
        cbxAllOrganizations.Checked = _Organization.AllOrganizations
        txtLogoPath.Text = _Organization.LogoPath
        txtCssPath.Text = _Organization.CssPath
        If Not _Organization.OrganizationContact Is Nothing Then
            txtCName.Text = _Organization.OrganizationContact.Name
            txtCAddress.Text = _Organization.OrganizationContact.Address
            txtCMailDisplay.Text = _Organization.OrganizationContact.MailDisplay
            txtCMail.Text = _Organization.OrganizationContact.Mail
            txtCWebDisplay.Text = _Organization.OrganizationContact.WebDisplay
            txtCWeb.Text = _Organization.OrganizationContact.Web
        End If
        Dim dvCustomer As New DataView
        'If Not m_context.Cache("myCustomerListView") Is Nothing Then
        '    dvCustomer = CType(m_context.Cache("myCustomerListView"), DataView)
        If Not Session("myCustomerListView") Is Nothing Then
            dvCustomer = CType(Session("myCustomerListView"), DataView)
        Else
            Dim dtCustomers As Kernel.CustomerList
            dtCustomers = New Kernel.CustomerList(m_User.Customer.AccountingArea, cn, True, False)

            dvCustomer.Sort = "Customername"
            dvCustomer = dtCustomers.DefaultView
            'm_context.Cache.Insert("myCustomerListView", dvCustomer, Nothing, DateTime.Now.AddMinutes(20), TimeSpan.Zero)
            Session("myCustomerListView") = dvCustomer
        End If
        txtCustomerID.Text = _Organization.CustomerId.ToString
        If _Organization.CustomerId > 0 Then
            dvCustomer.Sort = "CustomerID"
            txtCustomer.Text = dvCustomer(dvCustomer.Find(_Organization.CustomerId)).Item("CustomerName").ToString
        End If
        Return True
    End Function

    Private Sub ClearEdit()
        txtOrganizationID.Text = "-1"
        txtOrganizationName.Text = ""
        txtOrganizationReference.Text = "999"
        txtCName.Text = ""
        txtCAddress.Text = ""
        txtCMailDisplay.Text = ""
        txtCMail.Text = ""
        txtCWebDisplay.Text = ""
        txtCWeb.Text = ""
        txtLogoPath.Text = ""
        txtCssPath.Text = ""
        Dim intCustomerID As Integer = CInt(ddlFilterCustomer.SelectedItem.Value)
        If intCustomerID > 0 Then
            txtCustomerID.Text = intCustomerID.ToString
            txtCustomer.Text = ddlFilterCustomer.SelectedItem.Text
        Else
            txtCustomerID.Text = m_User.Customer.CustomerId.ToString
            txtCustomer.Text = m_User.Customer.CustomerName
        End If
        lbtnSave.Visible = True
        lbtnDelete.Visible = False
        LockEdit(False)
    End Sub

    Private Sub LockEdit(ByVal blnLock As Boolean)
        Dim strBackColor As String = "White"
        If blnLock Then
            strBackColor = "LightGray"
        End If
        txtOrganizationID.Enabled = Not blnLock
        txtOrganizationID.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtOrganizationName.Enabled = Not blnLock
        txtOrganizationName.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtOrganizationReference.Enabled = Not blnLock
        txtOrganizationReference.BackColor = System.Drawing.Color.FromName(strBackColor)
        cbxAllOrganizations.Enabled = Not blnLock
        txtCName.Enabled = Not blnLock
        txtCName.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtCAddress.Enabled = Not blnLock
        txtCAddress.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtCMailDisplay.Enabled = Not blnLock
        txtCMailDisplay.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtCMail.Enabled = Not blnLock
        txtCMail.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtCWebDisplay.Enabled = Not blnLock
        txtCWebDisplay.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtCWeb.Enabled = Not blnLock
        txtCWeb.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtLogoPath.Enabled = Not blnLock
        txtLogoPath.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtCssPath.Enabled = Not blnLock
        txtCssPath.BackColor = System.Drawing.Color.FromName(strBackColor)
    End Sub

    Private Sub OrganizationAdminMode()
        SearchMode(False)
        If Not m_User.Organization Is Nothing Then
            If m_User.Organization.OrganizationAdmin Then
                EditEditMode(m_User.Organization.OrganizationId)
            End If
        End If
    End Sub

    Private Sub EditEditMode(ByVal intOrganizationId As Integer)
        If Not FillEdit(intOrganizationId) Then
            LockEdit(True)
            lbtnSave.Enabled = False
        Else
            lbtnSave.Enabled = True
        End If
        lbtnCancel.Text = "Verwerfen&nbsp;&#187;"
    End Sub

    Private Sub EditDeleteMode(ByVal intOrganizationId As Integer)
        If Not FillEdit(intOrganizationId) Then
            lbtnDelete.Enabled = False
        Else
            lblMessage.Text = "Möchten Sie die Organisation wirklich löschen?"
            lbtnDelete.Enabled = True
        End If
        LockEdit(True)
        lbtnCancel.Text = "Abbrechen&nbsp;&#187;"
        lbtnSave.Visible = False
        lbtnDelete.Visible = True
    End Sub

    Private Sub SearchMode(Optional ByVal blnSearchMode As Boolean = True)
        trEditUser.Visible = Not blnSearchMode
        tableSearch.Visible = blnSearchMode
        trSearchResult.Visible = blnSearchMode
        lbtnSave.Visible = Not blnSearchMode
        lbtnCancel.Visible = Not blnSearchMode
        lbtnNew.Visible = blnSearchMode
        DivSearch1.Visible = blnSearchMode
        QueryFooter.Visible = blnSearchMode
        Result.Visible = blnSearchMode
        Input.Visible = Not blnSearchMode
        trSearchSpacer.Visible = blnSearchMode
    End Sub

    Private Sub Search(Optional ByVal blnRefillDataGrid As Boolean = False, Optional ByVal blnResetSelectedIndex As Boolean = False, Optional ByVal blnResetPageIndex As Boolean = False, Optional ByVal blnClearCache As Boolean = False)
        ClearEdit()
        If blnClearCache Then
            'm_context.Cache.Remove("myOrganizationListView")
            Session.Remove("myOrganizationListView")
        End If
        If blnResetSelectedIndex Then dgSearchResult.SelectedIndex = -1
        If blnResetPageIndex Then dgSearchResult.PageIndex = 0
        If m_User.HighestAdminLevel > AdminLevel.Organization Then
            SearchMode()
            If blnRefillDataGrid Then FillDataGrid()
        Else
            OrganizationAdminMode()
        End If
    End Sub

#End Region

#Region " Events "

    Private Sub lbtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnCancel.Click
        Search(, True)
    End Sub

    Private Sub lbtnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnNew.Click
        Dim intCustomer As Integer = CInt(ddlFilterCustomer.SelectedItem.Value)

        If intCustomer < 1 Then
            lblError.Text = "Wählen Sie bitte zunächst eine Firma aus!"
        Else
            SearchMode(False)
            ClearEdit()
            Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            cn.Open()
        End If
    End Sub

    Private Sub lbtnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnSave.Click
        Try
            If txtOrganizationReference.Text = String.Empty And Not cbxAllOrganizations.Checked Then
                Throw New Exception("Das Feld Organisationsreferenz darf nicht leer sein, wenn ""Zeige ALLE Organisationen"" nicht gesetzt ist!")
            End If
            Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            cn.Open()
            Dim intOrganizationId As Integer = CInt(txtOrganizationID.Text)
            Dim blnNew As Boolean = True
            If Not (intOrganizationId = -1) Then
                blnNew = False
            End If

            Dim _Organization As New Organization(intOrganizationId, txtOrganizationName.Text, _
                                                CInt(txtCustomerID.Text), txtOrganizationReference.Text, _
                                                cbxAllOrganizations.Checked, False, _
                                                txtLogoPath.Text, txtCssPath.Text, _
                                                txtCName.Text, txtCAddress.Text, _
                                                txtCMailDisplay.Text, txtCMail.Text, _
                                                txtCWebDisplay.Text, txtCWeb.Text, _
                                                blnNew)
            _Organization.Save(cn)

            Search(True, True, True, True)
            lblMessage.Text = "Die Änderungen wurden gespeichert."
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "OrganizationManagement", "lbtnSave_Click", ex.ToString)

            lblError.Text = ex.Message
            If Not ex.InnerException Is Nothing Then
                lblError.Text &= ": " & ex.InnerException.Message
            End If
        End Try
    End Sub

    Private Sub lbtnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnDelete.Click
        Try
            Dim _Organization As New Organization(CInt(txtOrganizationID.Text), txtOrganizationName.Text, _
                CInt(ddlFilterCustomer.SelectedItem.Value), txtOrganizationReference.Text, _
                cbxAllOrganizations.Checked, False, _
                txtLogoPath.Text, txtCssPath.Text, _
                txtCName.Text, txtCAddress.Text, _
                txtCMailDisplay.Text, txtCMail.Text, _
                txtCWebDisplay.Text, txtCWeb.Text, _
                False)
            Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            cn.Open()

            If Not _Organization.HasUser(cn) Then
                _Organization.Delete(cn)

                Search(True, True, True, True)
                lblMessage.Text = "Die Organisation wurde gelöscht."
            Else
                lblMessage.Text = "Die Organisation kann nicht gelöscht werden, da ihr noch Benutzer zugeordnet sind."
            End If
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "OrganizationManagement", "lbtnDelete_Click", ex.ToString)

            lblError.Text = ex.Message
            If Not ex.InnerException Is Nothing Then
                lblError.Text &= ": " & ex.InnerException.Message
            End If
        End Try
    End Sub

#End Region

    Private Sub btnSuche_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSuche.Click
        Search(True, True, True, True)
    End Sub

    Private Sub dgSearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgSearchResult.RowCommand
        Dim index As Integer
        Dim row As GridViewRow
        Dim CtrlLabel As Label


        If e.CommandName = "Edit" Then
            index = Convert.ToInt32(e.CommandArgument)
            row = dgSearchResult.Rows(index)
            CtrlLabel = row.Cells(0).FindControl("lblOrgaID")
            EditEditMode(CInt(CtrlLabel.Text))
            dgSearchResult.SelectedIndex = row.RowIndex
        ElseIf e.CommandName = "Del" Then
            index = Convert.ToInt32(e.CommandArgument)
            row = dgSearchResult.Rows(index)
            CtrlLabel = row.Cells(0).FindControl("lblOrgaID")
            EditDeleteMode(CInt(CtrlLabel.Text))
            dgSearchResult.SelectedIndex = row.RowIndex
        End If
    End Sub

    Private Sub dgSearchResult_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles dgSearchResult.RowEditing

    End Sub

    Private Sub GridNavigation1_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation1.PagerChanged
        dgSearchResult.PageIndex = PageIndex
        FillDataGrid()
    End Sub

    Private Sub GridNavigation1_PageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillDataGrid()
    End Sub

    Private Sub dgSearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgSearchResult.Sorting
        Dim strSort As String = e.SortExpression
        If Not ViewState("ResultSort") Is Nothing AndAlso ViewState("ResultSort").ToString = strSort Then
            strSort &= " DESC"
        End If
        ViewState("ResultSort") = strSort
        FillDataGrid(strSort)
    End Sub

    Protected Sub btnEmpty_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnEmpty.Click
        Search(True, True, True, True)
    End Sub

End Class