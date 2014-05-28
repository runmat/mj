
Imports CKG.Base.Kernel.Admin
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class GroupManagement
    Inherits System.Web.UI.Page

    Protected WithEvents ucStyles As Styles

#Region " Vom Web Form Designer generierter Code "
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblCustomer As System.Web.UI.WebControls.Label
    Protected WithEvents ddlFilterCustomer As System.Web.UI.WebControls.DropDownList
    Protected WithEvents dgSearchResult As System.Web.UI.WebControls.DataGrid
    Protected WithEvents txtGroupName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtGroupID As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
    Protected WithEvents trSearch As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trSearchResult As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trEditUser As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lstAppUnAssigned As System.Web.UI.WebControls.ListBox
    Protected WithEvents lstAppAssigned As System.Web.UI.WebControls.ListBox
    Protected WithEvents btnAssign As System.Web.UI.WebControls.Button
    Protected WithEvents btnUnAssign As System.Web.UI.WebControls.Button
    Protected WithEvents txtFilterGroupName As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblGroupName As System.Web.UI.WebControls.Label
    Protected WithEvents trSearchSpacer As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trApp As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lnkAppManagement As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lbtnNew As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbtnSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbtnCancel As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbtnDelete As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkUserManagement As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkCustomerManagement As System.Web.UI.WebControls.HyperLink
    Protected WithEvents trCustomer As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents txtCustomer As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCustomerID As System.Web.UI.WebControls.TextBox
    Protected WithEvents Form1 As System.Web.UI.HtmlControls.HtmlForm
    Protected WithEvents lnkOrganizationManagement As System.Web.UI.WebControls.HyperLink
    Protected WithEvents trAuthorization As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents ddlAuthorizationright As System.Web.UI.WebControls.DropDownList
    Protected WithEvents trCustomergroup As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents cbxIsCustomerGroup As System.Web.UI.WebControls.CheckBox
    Protected WithEvents txtDocuPath As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtStartMethod As System.Web.UI.WebControls.TextBox
    Protected WithEvents trStartMethod As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents txtMessage As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtMessageOld As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtMaxReadMessageCount As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnSuche As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkArchivManagement As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lstArchivUnAssigned As System.Web.UI.WebControls.ListBox
    Protected WithEvents btnAssignArchiv As System.Web.UI.WebControls.Button
    Protected WithEvents btnUnAssignArchiv As System.Web.UI.WebControls.Button
    Protected WithEvents lstArchivAssigned As System.Web.UI.WebControls.ListBox
    'Protected WithEvents lstEmployeeUnAssigned As System.Web.UI.WebControls.ListBox
    'Protected WithEvents btnAssignEmployee As System.Web.UI.WebControls.Button
    'Protected WithEvents btnUnAssignEmployee As System.Web.UI.WebControls.Button
    'Protected WithEvents lstEmployeeAssigned As System.Web.UI.WebControls.ListBox
    Protected WithEvents xCoordHolder As Global.System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents yCoordHolder As Global.System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents ucHeader As Header
    Protected WithEvents tblAbrufgruendeTemp As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents lbxAbrufgruendeTempUnAssign As ListBox
    Protected WithEvents btnAssignAbrufgrundTemp As Button
    Protected WithEvents btnUnAssignAbrufgrundTemp As Button
    Protected WithEvents lbxAbrufgruendeTempAssign As ListBox

    Protected WithEvents tblAbrufgruendeEndg As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents lbxAbrufgruendeEndgUnAssign As ListBox
    Protected WithEvents btnAssignAbrufgrundEndg As Button
    Protected WithEvents btnUnAssignAbrufgrundEndg As Button
    Protected WithEvents lbxAbrufgruendeEndgAssign As ListBox
    Protected WithEvents gvAutLevel As System.Web.UI.WebControls.GridView
    Protected WithEvents tblAutorisierung As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents cbxTeamViewer As System.Web.UI.WebControls.CheckBox
    Protected WithEvents cbxIsServiceGroup As System.Web.UI.WebControls.CheckBox
    'Dieser Aufruf ist für den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: Diese Methode ist für den Web Form-Designer erforderlich
        'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
        InitializeComponent()
    End Sub

#End Region

#Region " Membervariables "
    Private m_User As User
    Private m_App As App
    Private m_context As HttpContext = HttpContext.Current
#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Hier Benutzercode zur Seiteninitialisierung einfügen
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        ucStyles.TitleText = "Gruppenverwaltung"
        AdminAuth(Me, m_User, AdminLevel.Customer)

        Try
            m_App = New App(m_User)

            If Not IsPostBack Then
                lblError.Text = ""

                'Fülle ddlAuthorizationright mit festen Vorgaben
                Dim strAuthorRights(4) As String
                strAuthorRights(0) = "0 - keine"
                strAuthorRights(1) = "0..1 - wenig"
                strAuthorRights(2) = "0..2 - mittel"
                strAuthorRights(3) = "0..3 - viel"
                Dim i As Int32
                For i = 0 To 3
                    Dim listitem As New ListItem()
                    listitem.Value = CStr(i)
                    listitem.Text = strAuthorRights(i)
                    ddlAuthorizationright.Items.Add(listitem)
                Next

                FillForm()
            End If
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "GroupManagement", "Page_Load", ex.ToString)
            lblError.Text = ex.ToString
            lblError.Visible = True
        End Try
    End Sub

#Region " Data and Function "
    Private Sub FillForm()
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try
            cn.Open()
            FillCustomer(cn)
            trEditUser.Visible = False
            trSearchResult.Visible = False

            If m_User.HighestAdminLevel = AdminLevel.Master Then
                'wenn SuperUser und übergeordnete Firma
                If m_User.Customer.AccountingArea = -1 Then
                    lnkAppManagement.Visible = True
                End If
                lblCustomer.Visible = False
                ddlFilterCustomer.Visible = True
                txtFilterGroupName.Visible = True
                tblAutorisierung.Visible = True
            Else
                trStartMethod.Visible = False
                lnkCustomerManagement.Visible = False
                lnkArchivManagement.Visible = False
                trCustomer.Visible = False 'Customer-Auswahl im Edit-bereich ausblenden
                lnkAppManagement.Visible = False
                tblAutorisierung.Visible = False
                If m_User.IsCustomerAdmin Then
                    txtFilterGroupName.Visible = True
                Else
                    GroupAdminMode()
                End If
            End If
            lblCustomer.Text = m_User.Customer.CustomerName
            lblGroupName.Visible = Not txtFilterGroupName.Visible
            '##################################################################
            If Not m_User.Customer.ShowOrganization Then        '23.05.2005 JVE
                lnkOrganizationManagement.Visible = False
            End If
            '#################################################################
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Sub

    Private Sub FillCustomer(ByVal cn As SqlClient.SqlConnection)
        Dim dtCustomers As Kernel.CustomerList
        dtCustomers = New Kernel.CustomerList(m_User.Customer.AccountingArea, cn, True, False)

        Dim dv As DataView = dtCustomers.DefaultView
        dv.Sort = "Customername"
        ' m_context.Cache.Insert("myCustomerListView", dv, Nothing, DateTime.Now.AddMinutes(20), TimeSpan.Zero)
        Session.Add("myCustomerListView", dv)

        With ddlFilterCustomer
            .DataSource = dv
            .DataTextField = "Customername"
            .DataValueField = "CustomerID"
            .DataBind()
            .Items.FindByValue(m_User.Customer.CustomerId.ToString).Selected = True
        End With
    End Sub

    Private Sub FillDataGrid()
        Dim strSort As String = "GroupID"
        If Not ViewState("ResultSort") Is Nothing Then
            strSort = ViewState("ResultSort").ToString
        End If
        FillDataGrid(strSort)
    End Sub
    Private Sub FillDataGrid(ByVal strSort As String)
        trSearchResult.Visible = True
        Dim dvGroup As DataView

        If Not Session("myGroupListView") Is Nothing Then
            dvGroup = CType(Session("myGroupListView"), DataView)
        Else
            Dim dtGroup As Kernel.GroupList
            Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            Try
                cn.Open()

                Dim intTemp As Integer
                If m_User.HighestAdminLevel = AdminLevel.Master Then
                    intTemp = CInt(ddlFilterCustomer.SelectedItem.Value)
                Else
                    intTemp = m_User.Customer.CustomerId
                End If

                dtGroup = New Kernel.GroupList(txtFilterGroupName.Text, _
                                                    intTemp, _
                                                    cn, _
                                                    m_User.Customer.AccountingArea)
                dvGroup = dtGroup.DefaultView
                Session.Add("myGroupListView", dvGroup)
                ' m_context.Cache.Insert("myGroupListView", dvGroup, Nothing, DateTime.Now.AddMinutes(20), TimeSpan.Zero)
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End If
        dvGroup.Sort = strSort
        If dvGroup.Count > dgSearchResult.PageSize Then
            dgSearchResult.PagerStyle.Visible = True
        Else
            dgSearchResult.PagerStyle.Visible = False
        End If

        With dgSearchResult
            .DataSource = dvGroup
            .DataBind()
        End With
    End Sub

    Private Function FillEdit(ByVal intGroupId As Integer) As Boolean
        Dim cn As SqlClient.SqlConnection
        cn = New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try
            SearchMode(False)
            cn.Open()
            Dim _Group As New Group(intGroupId, cn)
            txtGroupID.Text = _Group.GroupId.ToString
            txtGroupName.Text = _Group.GroupName
            If Not ddlAuthorizationright.SelectedItem Is Nothing Then
                ddlAuthorizationright.SelectedItem.Selected = False
            End If
            Dim _li As ListItem = ddlAuthorizationright.Items.FindByValue(_Group.Authorizationright.ToString)
            If Not _li Is Nothing Then
                _li.Selected = True
            End If
            cbxIsCustomerGroup.Checked = _Group.IsCustomerGroup
            txtDocuPath.Text = _Group.DocuPath
            Dim dvCustomer As New DataView
            If Not Session("myCustomerListView") Is Nothing Then
                dvCustomer = CType(Session("myCustomerListView"), DataView)
            Else
                Dim dtCustomers As Kernel.CustomerList
                dtCustomers = New Kernel.CustomerList(m_User.Customer.AccountingArea, cn, True, False)

                dvCustomer.Sort = "Customername"
                dvCustomer = dtCustomers.DefaultView
                Session.Add("myCustomerListView", dvCustomer)
            End If
            txtCustomerID.Text = _Group.CustomerId.ToString
            If _Group.CustomerId > 0 Then
                dvCustomer.Sort = "CustomerID"
                txtCustomer.Text = dvCustomer(dvCustomer.Find(_Group.CustomerId)).Item("CustomerName").ToString
            End If
            'prüfung ob der Kunde Abrufgründe hat, wenn nicht abrufgründe ausblenden, wenn
            'ja ob die Gruppe schon -keine Auswahl- hat, wenn nicht hinzufügen
            checkCustomerAbrufGruende(cn, _Group.GroupId, _Group.CustomerId)

            FillAssigned(_Group.GroupId, _Group.CustomerId, cn)
            FillUnAssigned(_Group.GroupId, _Group.CustomerId, cn)
            txtStartMethod.Text = _Group.StartMethod
            txtMessage.Text = _Group.Message
            txtMessageOld.Text = _Group.Message
            txtMaxReadMessageCount.Text = _Group.MaxReadMessageCount.ToString
            cbxTeamViewer.Checked = _Group.ShowsTeamViewer
            cbxIsServiceGroup.Checked = _Group.IsServiceGroup
            Return True
        Catch ex As Exception
            lblError.Text = "Fehler: " & ex.Message
            m_App.WriteErrorText(1, m_User.UserName, "GroupManagement", "FillEdit", ex.ToString)
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Function

    Private Sub checkCustomerAbrufGruende(ByVal cn As SqlClient.SqlConnection, ByVal groupID As String, ByVal CustomerID As String)
        Try
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If

            Dim cmd As New SqlClient.SqlCommand
            cmd.Connection = cn
            cmd.CommandType = CommandType.Text

            Dim tmpDT As New DataTable
            Dim daApp As New SqlClient.SqlDataAdapter("SELECT *" & _
                                                          "FROM CustomerAbrufgruende WHERE CustomerID=" & CustomerID & " AND GroupID=" & groupID & " ", cn)
            daApp.Fill(tmpDT)

            If tmpDT.Rows.Count > 1 Then
                'hier muss geprüft werden ob SapWert 000 ( -Keine Auswahl-) vorhanden ist.
                tblAbrufgruendeEndg.Visible = True
                tblAbrufgruendeTemp.Visible = True

                If Not tmpDT.Select("SapWert=000").Length = 2 Then

                    If tmpDT.Select("SapWert=000 And AbrufTyp='temp'").Length = 0 Then
                        insertKeineAuswahlAbrufgrund(cn, groupID, CustomerID, "temp")
                    End If

                    If tmpDT.Select("SapWert=000 And AbrufTyp='endg'").Length = 0 Then
                        insertKeineAuswahlAbrufgrund(cn, groupID, CustomerID, "endg")
                    End If
                End If

            Else  'wenn keine vorhanden , dann abrufgründe ausblenden
                tblAbrufgruendeEndg.Visible = False
                tblAbrufgruendeTemp.Visible = False
            End If


        Catch ex As Exception
            Throw ex
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Sub

    Private Sub insertKeineAuswahlAbrufgrund(ByVal cn As SqlClient.SqlConnection, ByVal groupID As String, ByVal CustomerID As String, ByVal abruftyp As String)
        Try
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If

            Dim cmd As New SqlClient.SqlCommand
            cmd.Connection = cn
            cmd.CommandType = CommandType.Text
            Dim SqlQuery As String

            SqlQuery = "INSERT INTO CustomerAbrufgruende" & _
             " (CustomerID, GroupID, AbrufTyp, WebBezeichnung, SapWert, MitZusatzText, Zusatzbemerkung) " & _
             "VALUES (" & CustomerID & ", " & groupID & ", '" & abruftyp & "', '- keine Auswahl -', '000',0,'')"
            cmd.CommandText = SqlQuery
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Sub


    Private Sub FillAbrufgruendeAssignedTemp(ByVal groupID As String, ByVal cusotmerID As String, ByVal cn As SqlClient.SqlConnection)
        Dim dtAbrufgruende As Kernel.AbrufgruendeDT
        dtAbrufgruende = New Kernel.AbrufgruendeDT(cn, cusotmerID, groupID, False, "temp")

        With lbxAbrufgruendeTempAssign
            Dim dv As DataView = dtAbrufgruende.DefaultView
            dv.Sort = "WebBezeichnung"
            .DataSource = dv
            .DataTextField = "WebBezeichnung"
            .DataValueField = "GrundID"
            .DataBind()
        End With
    End Sub

    Private Sub FillAbrufgruendeUnAssignedTemp(ByVal groupID As String, ByVal cusotmerID As String, ByVal cn As SqlClient.SqlConnection)
        Dim dtAbrufgruende As Kernel.AbrufgruendeDT
        dtAbrufgruende = New Kernel.AbrufgruendeDT(cn, cusotmerID, groupID, True, "temp")

        With lbxAbrufgruendeTempUnAssign
            Dim dv As DataView = dtAbrufgruende.DefaultView
            dv.Sort = "WebBezeichnung"
            .DataSource = dv
            .DataTextField = "WebBezeichnung"
            .DataValueField = "GrundID"
            .DataBind()
            if dtAbrufgruende.DefaultView.Count > 0 then
                tblAbrufgruendeTemp.Visible=True
            end if
        End With
    End Sub

    Private Sub FillAbrufgruendeAssignedEndg(ByVal groupID As String, ByVal cusotmerID As String, ByVal cn As SqlClient.SqlConnection)
        Dim dtAbrufgruende As Kernel.AbrufgruendeDT
        dtAbrufgruende = New Kernel.AbrufgruendeDT(cn, cusotmerID, groupID, False, "Endg")

        With lbxAbrufgruendeEndgAssign
            Dim dv As DataView = dtAbrufgruende.DefaultView
            dv.Sort = "WebBezeichnung"
            .DataSource = dv
            .DataTextField = "WebBezeichnung"
            .DataValueField = "GrundID"
            .DataBind()
        End With
    End Sub

    Private Sub FillAbrufgruendeUnAssignedEndg(ByVal groupID As String, ByVal cusotmerID As String, ByVal cn As SqlClient.SqlConnection)
        Dim dtAbrufgruende As Kernel.AbrufgruendeDT
        dtAbrufgruende = New Kernel.AbrufgruendeDT(cn, cusotmerID, groupID, True, "Endg")

        With lbxAbrufgruendeEndgUnAssign
            Dim dv As DataView = dtAbrufgruende.DefaultView
            dv.Sort = "WebBezeichnung"
            .DataSource = dv
            .DataTextField = "WebBezeichnung"
            .DataValueField = "GrundID"
            .DataBind()
            if dtAbrufgruende.DefaultView.Count > 0 then
                tblAbrufgruendeEndg.Visible=True
            end if


        End With
    End Sub


    Private Function GetAppAssignedView(ByVal intGroupID As Integer, ByVal intCustomerID As Integer, ByVal cn As SqlClient.SqlConnection) As DataView
        'Als Extra-Funktion ausgelagert, da beim Speichern evtl.
        'benoetigt, wenn Cache leer ist (in jenem Fall soll die
        'List kein DataBind ausfuehren).
        Dim _AppAssigned As New ApplicationList(intGroupID, intCustomerID, cn)
        _AppAssigned.GetAssigned()
        _AppAssigned.DefaultView.Sort = "AppFriendlyName"
        'Cache wird befuellt, um spaeter beim Speichern darauf
        'zu zugreifen.
        Session.Add("myAppAssigned", _AppAssigned.DefaultView)
        Return _AppAssigned.DefaultView
    End Function

    Private Function GetArchivAssignedView(ByVal intGroupID As Integer, ByVal intCustomerID As Integer, ByVal cn As SqlClient.SqlConnection) As DataView
        'Als Extra-Funktion ausgelagert, da beim Speichern evtl.
        'benoetigt, wenn Cache leer ist (in jenem Fall soll die
        'List kein DataBind ausfuehren).
        Dim _ArchivAssigned As New ArchivList(intGroupID, intCustomerID, cn)
        _ArchivAssigned.GetAssigned()
        _ArchivAssigned.DefaultView.Sort = "EasyArchivName"
        'Cache wird befuellt, um spaeter beim Speichern darauf
        'zu zugreifen.
        Session.Add("myArchivAssigned", _ArchivAssigned.DefaultView)
        Return _ArchivAssigned.DefaultView
    End Function

    'Private Function GetEmployeeAssignedView(ByVal intGroupID As Integer, ByVal intAccountingArea As Integer, ByVal cn As SqlClient.SqlConnection) As DataView
    '    'Als Extra-Funktion ausgelagert, da beim Speichern evtl.
    '    'benoetigt, wenn Cache leer ist (in jenem Fall soll die
    '    'List kein DataBind ausfuehren).
    '    Dim _EmployeeAssigned As New EmployeeList(intGroupID, intAccountingArea, cn)
    '    _EmployeeAssigned.GetAssigned()
    '    _EmployeeAssigned.DefaultView.Sort = "EmployeeName"
    '    'Cache wird befuellt, um spaeter beim Speichern darauf
    '    'zu zugreifen.
    '    Session.Add("myEmployeeAssigned", _EmployeeAssigned.DefaultView)
    '    Return _EmployeeAssigned.DefaultView
    'End Function

    Private Sub FillAssigned(ByVal intGroupID As Integer, ByVal intCustomerID As Integer, ByVal cn As SqlClient.SqlConnection)
        Dim dvAppAssigned As DataView = GetAppAssignedView(intGroupID, intCustomerID, cn)
        lstAppAssigned.DataSource = dvAppAssigned
        lstAppAssigned.DataTextField = "AppFriendlyName"
        lstAppAssigned.DataValueField = "AppID"
        lstAppAssigned.DataBind()

        gvAutLevel.DataSource = dvAppAssigned
        gvAutLevel.DataBind()


        Dim dvArchivAssigned As DataView = GetArchivAssignedView(intGroupID, intCustomerID, cn)
        lstArchivAssigned.DataSource = dvArchivAssigned
        lstArchivAssigned.DataTextField = "EasyArchivName"
        lstArchivAssigned.DataValueField = "ArchivID"
        lstArchivAssigned.DataBind()

        'Dim dvEmployeeAssigned As DataView = GetEmployeeAssignedView(intGroupID, m_User.Customer.AccountingArea, cn)
        'lstEmployeeAssigned.DataSource = dvEmployeeAssigned
        'lstEmployeeAssigned.DataTextField = "EmployeeName"
        'lstEmployeeAssigned.DataValueField = "UserID"
        'lstEmployeeAssigned.DataBind()

        FillAbrufgruendeUnAssignedTemp(intGroupID.ToString, intCustomerID.ToString, cn)
        FillAbrufgruendeAssignedTemp(intGroupID.ToString, intCustomerID.ToString, cn)

        FillAbrufgruendeUnAssignedEndg(intGroupID.ToString, intCustomerID.ToString, cn)
        FillAbrufgruendeAssignedEndg(intGroupID.ToString, intCustomerID.ToString, cn)


    End Sub

    Private Sub FillUnAssigned(ByVal intGroupID As Integer, ByVal intCustomerID As Integer, ByVal cn As SqlClient.SqlConnection)
        Dim _AppUnAssigned As New ApplicationList(intGroupID, intCustomerID, cn)
        _AppUnAssigned.GetUnassigned()
        _AppUnAssigned.DefaultView.Sort = "AppFriendlyName"
        lstAppUnAssigned.DataSource = _AppUnAssigned.DefaultView
        lstAppUnAssigned.DataTextField = "AppFriendlyName"
        lstAppUnAssigned.DataValueField = "AppID"
        lstAppUnAssigned.DataBind()

        Dim _ArchivUnAssigned As New ArchivList(intGroupID, intCustomerID, cn)
        _ArchivUnAssigned.GetUnassigned()
        _ArchivUnAssigned.DefaultView.Sort = "EasyArchivName"
        lstArchivUnAssigned.DataSource = _ArchivUnAssigned.DefaultView
        lstArchivUnAssigned.DataTextField = "EasyArchivName"
        lstArchivUnAssigned.DataValueField = "ArchivID"
        lstArchivUnAssigned.DataBind()

        'Dim _EmployeeUnAssigned As New EmployeeList(intGroupID, m_User.Customer.AccountingArea, cn)
        '_EmployeeUnAssigned.GetUnassigned()
        '_EmployeeUnAssigned.DefaultView.Sort = "EmployeeName"
        'lstEmployeeUnAssigned.DataSource = _EmployeeUnAssigned.DefaultView
        'lstEmployeeUnAssigned.DataTextField = "EmployeeName"
        'lstEmployeeUnAssigned.DataValueField = "UserID"
        'lstEmployeeUnAssigned.DataBind()
    End Sub

    Private Sub ClearEdit()
        txtGroupID.Text = "-1"
        txtGroupName.Text = ""
        txtDocuPath.Text = ""
        txtStartMethod.Text = ""
        txtMessage.Text = ""
        txtMessageOld.Text = ""
        txtMaxReadMessageCount.Text = "3"
        Dim intCustomerID As Integer = CInt(ddlFilterCustomer.SelectedItem.Value)
        If intCustomerID > 0 Then
            txtCustomerID.Text = intCustomerID.ToString
            txtCustomer.Text = ddlFilterCustomer.SelectedItem.Text
        Else
            txtCustomerID.Text = m_User.Customer.CustomerId.ToString
            txtCustomer.Text = m_User.Customer.CustomerName
        End If
        If Not ddlAuthorizationright.SelectedItem Is Nothing Then
            ddlAuthorizationright.SelectedItem.Selected = False
        End If
        ddlAuthorizationright.Items(0).Selected = True
        cbxIsCustomerGroup.Checked = False
        lstAppAssigned.Items.Clear()
        lstAppUnAssigned.Items.Clear()
        lbtnSave.Visible = True
        lbtnDelete.Visible = False
        LockEdit(False)
        cbxTeamViewer.Checked = New Customer(intCustomerID).ShowsTeamViewer
        cbxIsServiceGroup.Checked = False
    End Sub

    Private Sub LockEdit(ByVal blnLock As Boolean)
        Dim strBackColor As String = "White"
        If blnLock Then
            strBackColor = "LightGray"
        End If
        txtGroupID.Enabled = Not blnLock
        txtGroupID.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtGroupName.Enabled = Not blnLock
        txtGroupName.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtMessage.Enabled = Not blnLock
        txtMessage.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtMaxReadMessageCount.Enabled = Not blnLock
        txtMaxReadMessageCount.BackColor = System.Drawing.Color.FromName(strBackColor)
        ddlAuthorizationright.Enabled = Not blnLock
        ddlAuthorizationright.BackColor = System.Drawing.Color.FromName(strBackColor)
        cbxIsCustomerGroup.Enabled = Not blnLock
        txtDocuPath.Enabled = Not blnLock
        txtDocuPath.BackColor = System.Drawing.Color.FromName(strBackColor)
        lstAppAssigned.Enabled = Not blnLock
        lstAppAssigned.BackColor = System.Drawing.Color.FromName(strBackColor)
        lstAppUnAssigned.Enabled = Not blnLock
        lstAppUnAssigned.BackColor = System.Drawing.Color.FromName(strBackColor)
        btnAssign.Enabled = Not blnLock
        btnUnAssign.Enabled = Not blnLock
        cbxTeamViewer.Enabled = Not blnLock
        cbxIsServiceGroup.Enabled = Not blnLock
    End Sub

    Private Sub GroupAdminMode()
        SearchMode(False)
        trApp.Visible = False

        If m_User.Groups.Count > 0 Then
            If m_User.Groups(0).IsCustomerGroup Then
                EditEditMode(m_User.Groups(0).GroupId)
            End If
        End If
    End Sub

    Private Sub EditEditMode(ByVal intGroupId As Integer)
        If Not FillEdit(intGroupId) Then
            LockEdit(True)
            lbtnSave.Enabled = False
        Else
            lbtnSave.Enabled = True
        End If
        lbtnCancel.Text = "Verwerfen"
    End Sub

    Private Sub EditDeleteMode(ByVal intGroupId As Integer)
        If Not FillEdit(intGroupId) Then
            lbtnDelete.Enabled = False
        Else
            lblMessage.Text = "Möchten Sie die Gruppe wirklich löschen?"
            lbtnDelete.Enabled = True
        End If
        LockEdit(True)
        lbtnCancel.Text = "Abbrechen"
        lbtnSave.Visible = False
        lbtnDelete.Visible = True
    End Sub

    Private Sub SearchMode(Optional ByVal blnSearchMode As Boolean = True)
        trEditUser.Visible = Not blnSearchMode
        trSearch.Visible = blnSearchMode
        trSearchSpacer.Visible = blnSearchMode
        trSearchResult.Visible = blnSearchMode
        lbtnSave.Visible = Not blnSearchMode
        lbtnCancel.Visible = Not blnSearchMode
        lbtnNew.Visible = blnSearchMode
    End Sub

    Private Sub Search(Optional ByVal blnRefillDataGrid As Boolean = False, Optional ByVal blnResetSelectedIndex As Boolean = False, Optional ByVal blnResetPageIndex As Boolean = False, Optional ByVal blnClearCache As Boolean = False)
        ClearEdit()
        If blnClearCache Then
            Session("myGroupListView") = Nothing
        End If
        If blnResetSelectedIndex Then dgSearchResult.SelectedIndex = -1
        If blnResetPageIndex Then dgSearchResult.CurrentPageIndex = 0
        If m_User.HighestAdminLevel > AdminLevel.Organization Then
            SearchMode()
            If blnRefillDataGrid Then FillDataGrid()
        Else
            GroupAdminMode()
        End If
    End Sub

    Private Sub Log(ByVal strIdentification As String, ByVal strDescription As String, ByVal tblParameters As DataTable, Optional ByVal strCategory As String = "APP")
        Dim logApp As New Base.Kernel.Logging.Trace(m_User.App.Connectionstring, m_User.App.SaveLogAccessSAP, m_User.App.LogLevel)

        ' strCategory
        Dim strUserName As String = m_User.UserName ' strUserName
        Dim strSessionID As String = Session.SessionID ' strSessionID
        Dim intSource As Integer = CInt(Request.QueryString("AppID")) ' intSource 
        Dim strTask As String = "Admin - Gruppenverwaltung" ' strTask
        ' strIdentification
        ' strDescription
        Dim strCustomerName As String = m_User.CustomerName ' strCustomername
        Dim blnIsTestUser As Boolean = m_User.IsTestUser ' blnIsTestUser
        Dim intSeverity As Integer = 0 ' intSeverity 

        logApp.WriteEntry(strCategory, strUserName, strSessionID, intSource, strTask, strIdentification, strDescription, strCustomerName, m_User.Customer.CustomerId, blnIsTestUser, intSeverity, tblParameters)
    End Sub

    Private Function SetOldLogParameters(ByVal intGroupId As Int32, ByVal tblPar As DataTable) As DataTable
        Dim cn As SqlClient.SqlConnection
        cn = New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try

            cn.Open()
            Dim _Group As New Group(intGroupId, cn)

            If tblPar Is Nothing Then
                tblPar = CreateLogTableStructure()
            End If
            With tblPar
                .Rows.Add(.NewRow)
                .Rows(.Rows.Count - 1)("Status") = "Alt"
                .Rows(.Rows.Count - 1)("Gruppenname") = _Group.GroupName
                .Rows(.Rows.Count - 1)("Aut.- Recht") = _Group.Authorizationright.ToString
                .Rows(.Rows.Count - 1)("Kunden- Gruppe") = _Group.IsCustomerGroup
                Dim dvCustomer As New DataView
                If Not Session("myCustomerListView") Is Nothing Then
                    dvCustomer = CType(Session("myCustomerListView"), DataView)
                Else
                    Dim dtCustomers As Kernel.CustomerList
                    dtCustomers = New Kernel.CustomerList(m_User.Customer.AccountingArea, cn, True, False)

                    dvCustomer.Sort = "Customername"
                    dvCustomer = dtCustomers.DefaultView
                    Session.Add("myCustomerListView", dvCustomer)
                End If
                txtCustomerID.Text = _Group.CustomerId.ToString
                If _Group.CustomerId > 0 Then
                    dvCustomer.Sort = "CustomerID"
                    .Rows(.Rows.Count - 1)("Firma") = dvCustomer(dvCustomer.Find(_Group.CustomerId)).Item("CustomerName").ToString
                End If

                Dim dvAppAssigned As DataView = GetAppAssignedView(intGroupId, _Group.CustomerId, cn)
                Dim strAnwendungen As String = ""
                Dim j As Int32
                For j = 0 To dvAppAssigned.Count - 1
                    strAnwendungen &= dvAppAssigned(j)("AppFriendlyName").ToString & vbNewLine
                Next
                .Rows(.Rows.Count - 1)("Anwendungen") = strAnwendungen
                .Rows(.Rows.Count - 1)("Handbuch") = _Group.DocuPath
                .Rows(.Rows.Count - 1)("Startmethode") = _Group.StartMethod
                .Rows(.Rows.Count - 1)("Message") = _Group.Message
                .Rows(.Rows.Count - 1)("MaxReadMessageCount") = _Group.MaxReadMessageCount
            End With
            Return tblPar
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "GroupManagement", "SetOldLogParameters", ex.ToString)

            Dim dt As New DataTable()
            dt.Columns.Add("Fehler beim Erstellen der Log-Parameter", System.Type.GetType("System.String"))
            dt.Rows.Add(dt.NewRow)
            Dim str As String = ex.Message
            If Not ex.InnerException Is Nothing Then
                str &= ": " & ex.InnerException.Message
            End If
            dt.Rows(0)("Fehler beim Erstellen der Log-Parameter") = str
            Return dt
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Function

    Private Function SetNewLogParameters(ByVal tblPar As DataTable) As DataTable
        Try
            If tblPar Is Nothing Then
                tblPar = CreateLogTableStructure()
            End If
            With tblPar
                .Rows.Add(.NewRow)
                .Rows(.Rows.Count - 1)("Status") = "Neu"
                .Rows(.Rows.Count - 1)("Gruppenname") = txtGroupName.Text
                .Rows(.Rows.Count - 1)("Aut.- Recht") = ddlAuthorizationright.SelectedItem.Text
                .Rows(.Rows.Count - 1)("Kunden- Gruppe") = cbxIsCustomerGroup.Checked
                .Rows(.Rows.Count - 1)("Firma") = txtCustomer.Text
                Dim _li As ListItem
                Dim strAnwendungen As String = ""
                For Each _li In lstAppAssigned.Items
                    strAnwendungen &= _li.Text & vbNewLine
                Next
                .Rows(.Rows.Count - 1)("Anwendungen") = strAnwendungen
                .Rows(.Rows.Count - 1)("Handbuch") = txtDocuPath.Text
                .Rows(.Rows.Count - 1)("Startmethode") = txtStartMethod.Text
                .Rows(.Rows.Count - 1)("Message") = txtMessage.Text
                .Rows(.Rows.Count - 1)("MaxReadMessageCount") = CInt(txtMaxReadMessageCount.Text)
            End With
            Return tblPar
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "GroupManagement", "SetNewLogParameters", ex.ToString)

            Dim dt As New DataTable()
            dt.Columns.Add("Fehler beim Erstellen der Log-Parameter", System.Type.GetType("System.String"))
            dt.Rows.Add(dt.NewRow)
            Dim str As String = ex.Message
            If Not ex.InnerException Is Nothing Then
                str &= ": " & ex.InnerException.Message
            End If
            dt.Rows(0)("Fehler beim Erstellen der Log-Parameter") = str
            Return dt
        End Try
    End Function

    Private Function CreateLogTableStructure() As DataTable
        Dim tblPar As New DataTable()
        With tblPar
            .Columns.Add("Status", System.Type.GetType("System.String"))
            .Columns.Add("Gruppenname", System.Type.GetType("System.String"))
            .Columns.Add("Aut.- Recht", System.Type.GetType("System.String"))
            .Columns.Add("Kunden- Gruppe", System.Type.GetType("System.Boolean"))
            .Columns.Add("Firma", System.Type.GetType("System.String"))
            .Columns.Add("Anwendungen", System.Type.GetType("System.String"))
            .Columns.Add("Handbuch", System.Type.GetType("System.String"))
            .Columns.Add("Startmethode", System.Type.GetType("System.String"))
            .Columns.Add("Message", System.Type.GetType("System.String"))
            .Columns.Add("MaxReadMessageCount", System.Type.GetType("System.Int32"))
        End With
        Return tblPar
    End Function
#End Region

#Region " Events "
    'Private Sub btnSuche_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSuche.Click
    '    Search(True, True, True, True)
    'End Sub

    Private Sub dgSearchResult_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgSearchResult.SortCommand
        Dim strSort As String = e.SortExpression
        If Not ViewState("ResultSort") Is Nothing AndAlso ViewState("ResultSort").ToString = strSort Then
            strSort &= " DESC"
        End If
        ViewState("ResultSort") = strSort
        FillDataGrid(strSort)
    End Sub

    Private Sub dgSearchResult_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgSearchResult.ItemCommand
        If e.CommandName = "Edit" Then
            EditEditMode(CInt(e.Item.Cells(0).Text))
            dgSearchResult.SelectedIndex = e.Item.ItemIndex
        ElseIf e.CommandName = "Delete" Then
            EditDeleteMode(CInt(e.Item.Cells(0).Text))
            dgSearchResult.SelectedIndex = e.Item.ItemIndex
        End If
    End Sub

    Private Sub dgSearchResult_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgSearchResult.PageIndexChanged
        dgSearchResult.CurrentPageIndex = e.NewPageIndex
        FillDataGrid()
    End Sub

    Private Sub lbtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnCancel.Click
        Search(, True)
    End Sub

    Private Sub lbtnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnNew.Click
        Dim intCustomer As Integer = CInt(ddlFilterCustomer.SelectedItem.Value)
        Session("myAppAssigned") = Nothing
        txtGroupID.Text = -1

        If intCustomer < 1 Then
            lblError.Text = "Wählen Sie bitte zunächst eine Firma aus!"
        Else
            SearchMode(False)
            ClearEdit()
            Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            Try
                cn.Open()
                FillUnAssigned(CInt(txtGroupID.Text), CInt(ddlFilterCustomer.SelectedItem.Value), cn)
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End If
    End Sub

    Private Sub lbtnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnSave.Click
        Dim tblLogParameter As DataTable
        Dim cn As SqlClient.SqlConnection

        If Not IsNumeric(txtMaxReadMessageCount.Text) Then
            lblMessage.Text = "Bitte geben Sie einen Zahlenwert für die Häufigkeit ein."
            Exit Sub
        End If
        cn = New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try

            cn.Open()
            Dim intGroupId As Integer = CInt(txtGroupID.Text)
            Dim strLogMsg As String = "Gruppe anlegen"
            If Not (intGroupId = -1) Then
                strLogMsg = "Gruppe ändern"
                tblLogParameter = New DataTable
                tblLogParameter = SetOldLogParameters(intGroupId, tblLogParameter)
            End If

            Dim blnNew As Boolean = False
            If CInt(txtGroupID.Text) < 1 Then blnNew = True
            Dim _group As New Group(intGroupId, _
                                                txtGroupName.Text, _
                                                CInt(txtCustomerID.Text), _
                                                txtDocuPath.Text, _
                                                CInt(ddlAuthorizationright.SelectedItem.Value.ToString), _
                                                cbxIsCustomerGroup.Checked, _
                                                blnNew, _
                                                txtStartMethod.Text, _
                                                txtMessage.Text.Trim.TrimEnd, _
                                                CInt(txtMaxReadMessageCount.Text), _
                                                cbxTeamViewer.Checked, _
                                                cbxIsServiceGroup.Checked)
            _group.Save(cn)
            If (Not (txtMessage.Text = txtMessageOld.Text)) OrElse (Not blnNew) Then
                'User Count zurücksetzen
                Dim cmdUpdateUser As SqlClient.SqlCommand
                Dim cmdGetUser As New SqlClient.SqlCommand("SELECT UserID FROM WebMember WHERE GroupID=@GroupID", cn)
                cmdGetUser.Parameters.AddWithValue("@GroupID", intGroupId)
                Dim dt As New DataTable()
                Dim da As New SqlClient.SqlDataAdapter()
                da.SelectCommand = cmdGetUser
                da.Fill(dt)
                Dim dr As DataRow
                For Each dr In dt.Rows
                    cmdUpdateUser = New SqlClient.SqlCommand("UPDATE WebUser SET ReadMessageCount=@ReadMessageCount WHERE UserID=@UserID", cn)
                    Dim intReadMessageCount As Int32 = 0
                    cmdUpdateUser.Parameters.AddWithValue("@ReadMessageCount", intReadMessageCount)
                    cmdUpdateUser.Parameters.AddWithValue("@UserID", CInt(dr("UserID")))
                    cmdUpdateUser.ExecuteNonQuery()
                Next
            End If

            'Anwendungen zuordnen
            Dim dvAppAssigned As New DataView
            If blnNew Then
                intGroupId = _group.GroupId
                txtGroupID.Text = intGroupId.ToString
            Else
                If Not Session("myAppAssigned") Is Nothing Then
                    dvAppAssigned = CType(Session("myAppAssigned"), DataView)
                Else
                    dvAppAssigned = GetAppAssignedView(intGroupId, _group.CustomerId, cn)
                End If
            End If
            Dim _assignment As New Kernel.AppAssignments(intGroupId, Kernel.AssignmentType.Group)
            _assignment.Save(dvAppAssigned, lstAppAssigned.Items, cn)

          

            'Archive zuordnen
            Dim dvArchivAssigned As New DataView
            If blnNew Then
                intGroupId = _group.GroupId
                txtGroupID.Text = intGroupId.ToString
            Else
                If Not Session("myArchivAssigned") Is Nothing Then
                    dvArchivAssigned = CType(Session("myArchivAssigned"), DataView)
                Else
                    dvArchivAssigned = GetArchivAssignedView(intGroupId, _group.CustomerId, cn)
                End If
            End If
            Dim _archivassignment As New Kernel.ArchivAssignments(intGroupId, Kernel.AssignmentType.Group)
            _archivassignment.Save(dvArchivAssigned, lstArchivAssigned.Items, cn)


            'Abrufgründe Temp zuordnen
            saveAbrufGruendeTemp(intGroupId, _group.CustomerId, cn)

            'Abrufgründe Temp zuordnen
            saveAbrufGruendeEndg(intGroupId, _group.CustomerId, cn)


            saveLevelAppToGroup(intGroupId, cn)


            'Employees zuordnen
            'Dim dvEmployeeAssigned As New DataView
            'If blnNew Then
            '    intGroupId = _group.GroupId
            '    txtGroupID.Text = intGroupId.ToString
            'Else
            '    If Not Session("myEmployeeAssigned") Is Nothing Then
            '        dvEmployeeAssigned = CType(Session("myEmployeeAssigned"), DataView)
            '    Else
            '        dvEmployeeAssigned = GetEmployeeAssignedView(intGroupId, m_User.Customer.AccountingArea, cn)
            '    End If
            'End If
            'Dim _Employeeassignment As New Kernel.EmployeeAssignments(intGroupId)
            '_Employeeassignment.Save(dvEmployeeAssigned, lstEmployeeAssigned.Items, cn)

            tblLogParameter = New DataTable
            tblLogParameter = SetNewLogParameters(tblLogParameter)
            Log(_group.GroupId.ToString, strLogMsg, tblLogParameter)

            Search(True, True, , True)
            lblMessage.Text = "Die Änderungen wurden gespeichert."
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "GroupManagement", "lbtnSave_Click", ex.ToString)

            lblError.Text = ex.Message
            If Not ex.InnerException Is Nothing Then
                lblError.Text &= ": " & ex.InnerException.Message
            End If
            tblLogParameter = New DataTable
            Log(txtGroupID.Text, lblError.Text, tblLogParameter, "ERR")
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Sub

    Private Sub saveAbrufGruendeTemp(ByVal groupID As String, ByVal cusotmerID As String, ByVal cn As SqlClient.SqlConnection)
        'es müssen hier nur die zugewiesenen beachtet werden.
        'ursprüngliche liste holen
        Dim alDelete As New ArrayList
        Dim alInsert As New ArrayList
        Dim dtAbrufgruende As Kernel.AbrufgruendeDT
        dtAbrufgruende = New Kernel.AbrufgruendeDT(cn, cusotmerID, groupID, False, "temp")
        dtAbrufgruende.Columns.Add("checked", String.Empty.GetType)

        For Each item As ListItem In lbxAbrufgruendeTempAssign.Items
            If dtAbrufgruende.Select("GrundID='" & item.Value & "'").Length > 0 Then
                'abrufgrund war bereits vorhanden und ist checked
                dtAbrufgruende.Select("GrundID='" & item.Value & "'")(0)("checked") = "X"
            Else
                'abrufgrunde war noch nicht vorhanden, muss inserted werden
                insertAbrufgrundCopyOf(groupID, item.Value, cn)
            End If
        Next

        For Each row As DataRow In dtAbrufgruende.Rows
            If row("checked") Is DBNull.Value Then
                'alle die nicht checked sind, entfernen
                deleteAbrufgrund(row("GrundID").ToString, cn)
            End If
        Next

    End Sub

    Private Sub deleteAbrufgrund(ByVal abrufGrundID As String, ByVal cn As SqlClient.SqlConnection)
        If cn.State = ConnectionState.Closed Then
            cn.Open()
        End If
        Dim cmd As New SqlClient.SqlCommand()
        cmd.Connection = cn
        Try
            Dim sqlQuery As String
            sqlQuery = "Delete From CustomerAbrufgruende " & _
              "WHERE GrundID=" & abrufGrundID & ";"
            cmd.CommandText = sqlQuery
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Sub
    Private Sub saveLevelAppToGroup(ByVal groupID As String, ByVal cn As SqlClient.SqlConnection)
        If cn.State = ConnectionState.Closed Then
            cn.Open()
        End If
        Dim cmd As New SqlClient.SqlCommand()
        cmd.Connection = cn

        Try
            For Each row As GridViewRow In gvAutLevel.Rows
                Dim sAppID As String
                Dim iLevel As Integer
                Dim iWithAut As Integer = 0
                Dim sqlQuery As String
                sAppID = CType(row.Cells(0).FindControl("lblAppID"), Label).Text
                If CType(row.Cells(2).FindControl("chkWithAut"), CheckBox).Checked = True Then
                    iWithAut = 1
                End If
                If CType(row.Cells(3).FindControl("rbLevel0"), RadioButton).Checked = True Then
                    iLevel = 0
                ElseIf CType(row.Cells(4).FindControl("rbLevel1"), RadioButton).Checked = True Then
                    iLevel = 1
                ElseIf CType(row.Cells(5).FindControl("rbLevel2"), RadioButton).Checked = True Then
                    iLevel = 2
                ElseIf CType(row.Cells(6).FindControl("rbLevel3"), RadioButton).Checked = True Then
                    iLevel = 3
                ElseIf CType(row.Cells(7).FindControl("rbLevel4"), RadioButton).Checked = True Then
                    iLevel = 4
                ElseIf CType(row.Cells(8).FindControl("rbLevel5"), RadioButton).Checked = True Then
                    iLevel = 5
                ElseIf CType(row.Cells(9).FindControl("rbLevel6"), RadioButton).Checked = True Then
                    iLevel = 6
                End If


                sqlQuery = "Update dbo.rights Set AuthorizationLevel = " & iLevel & ", WithAuthorization = " & iWithAut & _
                  " WHERE AppID=" & sAppID & " And GroupID= " & groupID
                cmd.CommandText = sqlQuery
                cmd.ExecuteNonQuery()
            Next

        Catch ex As Exception
            Throw ex
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Sub

    Private Sub saveAbrufGruendeEndg(ByVal groupID As String, ByVal cusotmerID As String, ByVal cn As SqlClient.SqlConnection)
        'es müssen hier nur die zugewiesenen beachtet werden.
        'ursprüngliche liste holen
        Dim alDelete As New ArrayList
        Dim alInsert As New ArrayList
        Dim dtAbrufgruende As Kernel.AbrufgruendeDT
        dtAbrufgruende = New Kernel.AbrufgruendeDT(cn, cusotmerID, groupID, False, "Endg")
        dtAbrufgruende.Columns.Add("checked", String.Empty.GetType)

        For Each item As ListItem In lbxAbrufgruendeEndgAssign.Items
            If dtAbrufgruende.Select("GrundID='" & item.Value & "'").Length > 0 Then
                'abrufgrund war bereits vorhanden und ist checked
                dtAbrufgruende.Select("GrundID='" & item.Value & "'")(0)("checked") = "X"
            Else
                'abrufgrunde war noch nicht vorhanden, muss inserted werden
                insertAbrufgrundCopyOf(groupID, item.Value, cn)
            End If
        Next

        For Each row As DataRow In dtAbrufgruende.Rows
            If row("checked") Is DBNull.Value Then
                'alle die nicht checked sind, entfernen
                deleteAbrufgrund(row("GrundID").ToString, cn)
            End If
        Next

    End Sub

    Private Sub insertAbrufgrundCopyOf(ByVal groupID As String, ByVal abrufGrundID As String, ByVal cn As SqlClient.SqlConnection)
        If cn.State = ConnectionState.Closed Then
            cn.Open()
        End If
        Dim cmd As New SqlClient.SqlCommand()
        cmd.Connection = cn
        Try
            Dim tmpDT As New DataTable
            Dim sqlQuery As String
            Dim daApp As New SqlClient.SqlDataAdapter("SELECT *" & _
                                                          "FROM CustomerAbrufgruende WHERE GrundID=" & abrufGrundID & "", cn)
            daApp.Fill(tmpDT)


            sqlQuery = "INSERT INTO CustomerAbrufgruende" & _
             " (CustomerID, GroupID, AbrufTyp, WebBezeichnung, SapWert, MitZusatzText, Zusatzbemerkung) " & _
             "VALUES (" & tmpDT.Rows(0)("CustomerID").ToString & "," & groupID & ", '" & tmpDT.Rows(0)("AbrufTyp").ToString & "', '" & tmpDT.Rows(0)("WebBezeichnung").ToString & "', '" & tmpDT.Rows(0)("SapWert").ToString & "'," & CInt(CBool(tmpDT.Rows(0)("MitZusatzText"))) & ",'" & tmpDT.Rows(0)("Zusatzbemerkung").ToString & "')"
            cmd.CommandText = sqlQuery
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try

    End Sub

    Private Sub lbtnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnDelete.Click
        Dim tblLogParameter As DataTable
        Dim cn As SqlClient.SqlConnection
        cn = New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try
            Dim _Group As New Group(CInt(txtGroupID.Text), CInt(ddlFilterCustomer.SelectedItem.Value))

            cn.Open()
            tblLogParameter = New DataTable
            tblLogParameter = SetOldLogParameters(_Group.GroupId, tblLogParameter)
            If Not _Group.HasUser(cn) Then
                _Group.Delete(cn)
                Log(_Group.GroupId.ToString, "Gruppe löschen", tblLogParameter)

                Search(True, True, True, True)
                lblMessage.Text = "Die Gruppe wurde gelöscht."
            Else
                lblMessage.Text = "Die Gruppe kann nicht gelöscht werden, da ihr noch Benutzer zugeordnet sind."
            End If
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "GroupManagement", "lbtnDelete_Click", ex.ToString)

            lblError.Text = ex.Message
            If Not ex.InnerException Is Nothing Then
                lblError.Text &= ": " & ex.InnerException.Message
            End If
            tblLogParameter = New DataTable
            Log(txtGroupID.Text, lblError.Text, tblLogParameter, "ERR")
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Sub

    Private Sub btnAssign_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAssign.Click
        Dim _item As ListItem
        Dim _coll As New ListItemCollection()

        For Each _item In lstAppUnAssigned.Items
            If _item.Selected = True Then
                _item.Selected = False
                _coll.Add(_item)
            End If
        Next

        For Each _item In _coll
            lstAppAssigned.Items.Add(_item)
            lstAppUnAssigned.Items.Remove(_item)
        Next
    End Sub

    Private Sub btnUnAssign_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUnAssign.Click
        Dim _item As ListItem
        Dim _coll As New ListItemCollection()

        For Each _item In lstAppAssigned.Items
            If _item.Selected = True Then
                _item.Selected = False
                _coll.Add(_item)
            End If
        Next

        For Each _item In _coll
            lstAppUnAssigned.Items.Add(_item)
            lstAppAssigned.Items.Remove(_item)
        Next
    End Sub
#End Region

    Private Sub btnSuche_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSuche.Click
        Search(True, True, True, True)
    End Sub

    Private Sub btnAssignArchiv_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAssignArchiv.Click
        Dim _item As ListItem
        Dim _coll As New ListItemCollection()

        For Each _item In lstArchivUnAssigned.Items
            If _item.Selected = True Then
                _item.Selected = False
                _coll.Add(_item)
            End If
        Next

        For Each _item In _coll
            lstArchivAssigned.Items.Add(_item)
            lstArchivUnAssigned.Items.Remove(_item)
        Next
    End Sub

    Private Sub btnUnAssignArchiv_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUnAssignArchiv.Click
        Dim _item As ListItem
        Dim _coll As New ListItemCollection()

        For Each _item In lstArchivAssigned.Items
            If _item.Selected = True Then
                _item.Selected = False
                _coll.Add(_item)
            End If
        Next

        For Each _item In _coll
            lstArchivUnAssigned.Items.Add(_item)
            lstArchivAssigned.Items.Remove(_item)
        Next
    End Sub

    'Protected Sub btnAssignEmployee_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAssignEmployee.Click
    '    Dim _item As ListItem
    '    Dim _coll As New ListItemCollection()

    '    For Each _item In lstEmployeeUnAssigned.Items
    '        If _item.Selected = True Then
    '            _item.Selected = False
    '            _coll.Add(_item)
    '        End If
    '    Next

    '    For Each _item In _coll
    '        lstEmployeeAssigned.Items.Add(_item)
    '        lstEmployeeUnAssigned.Items.Remove(_item)
    '    Next
    'End Sub

    'Protected Sub btnUnAssignEmployee_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUnAssignEmployee.Click
    '    Dim _item As ListItem
    '    Dim _coll As New ListItemCollection()

    '    For Each _item In lstEmployeeAssigned.Items
    '        If _item.Selected = True Then
    '            _item.Selected = False
    '            _coll.Add(_item)
    '        End If
    '    Next

    '    For Each _item In _coll
    '        lstEmployeeUnAssigned.Items.Add(_item)
    '        lstEmployeeAssigned.Items.Remove(_item)
    '    Next
    'End Sub

    Protected Sub btnAssignAbrufgrundTemp_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAssignAbrufgrundTemp.Click
        Dim _item As ListItem
        Dim _coll As New ListItemCollection()

        For Each _item In lbxAbrufgruendeTempUnAssign.Items
            If _item.Selected = True Then
                _item.Selected = False
                _coll.Add(_item)
            End If
        Next

        For Each _item In _coll
            lbxAbrufgruendeTempAssign.Items.Add(_item)
            lbxAbrufgruendeTempUnAssign.Items.Remove(_item)
        Next
    End Sub

    Protected Sub btnUnAssignAbrufgrundTemp_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUnAssignAbrufgrundTemp.Click
        Dim _item As ListItem
        Dim _coll As New ListItemCollection()

        For Each _item In lbxAbrufgruendeTempAssign.Items
            If _item.Selected = True Then
                _item.Selected = False
                _coll.Add(_item)
            End If
        Next

        For Each _item In _coll
            lbxAbrufgruendeTempUnAssign.Items.Add(_item)
            lbxAbrufgruendeTempAssign.Items.Remove(_item)
        Next
    End Sub


    Protected Sub btnAssignAbrufgrundEndg_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAssignAbrufgrundEndg.Click
        Dim _item As ListItem
        Dim _coll As New ListItemCollection()

        For Each _item In lbxAbrufgruendeEndgUnAssign.Items
            If _item.Selected = True Then
                _item.Selected = False
                _coll.Add(_item)
            End If
        Next

        For Each _item In _coll
            lbxAbrufgruendeEndgAssign.Items.Add(_item)
            lbxAbrufgruendeEndgUnAssign.Items.Remove(_item)
        Next
    End Sub

    Protected Sub btnUnAssignAbrufgrundEndg_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUnAssignAbrufgrundEndg.Click
        Dim _item As ListItem
        Dim _coll As New ListItemCollection()

        For Each _item In lbxAbrufgruendeEndgAssign.Items
            If _item.Selected = True Then
                _item.Selected = False
                _coll.Add(_item)
            End If
        Next

        For Each _item In _coll
            lbxAbrufgruendeEndgUnAssign.Items.Add(_item)
            lbxAbrufgruendeEndgAssign.Items.Remove(_item)
        Next
    End Sub

End Class

' ************************************************
' $History: GroupManagement.aspx.vb $
' 
' *****************  Version 14  *****************
' User: Dittbernerc  Date: 9.05.11    Time: 16:52
' Updated in $/CKAG/admin
' 
' *****************  Version 13  *****************
' User: Dittbernerc  Date: 9.05.11    Time: 13:39
' Updated in $/CKAG/admin
' 
' *****************  Version 12  *****************
' User: Rudolpho     Date: 26.08.10   Time: 14:25
' Updated in $/CKAG/admin
' 
' *****************  Version 11  *****************
' User: Rudolpho     Date: 26.08.10   Time: 8:51
' Updated in $/CKAG/admin
' 
' *****************  Version 10  *****************
' User: Fassbenders  Date: 1.04.10    Time: 11:38
' Updated in $/CKAG/admin
' ITA: 3460
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 26.05.09   Time: 14:04
' Updated in $/CKAG/admin
' nachbesserung ita 2839
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 25.05.09   Time: 14:30
' Updated in $/CKAG/admin
' ITA 2839 testfertig
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 22.05.09   Time: 16:02
' Updated in $/CKAG/admin
' ITA 2839 unfertig
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 6.10.08    Time: 9:19
' Updated in $/CKAG/admin
' ITA 2295 fertig
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 24.09.08   Time: 13:40
' Updated in $/CKAG/admin
' ITA: 2273
' 
' *****************  Version 4  *****************
' User: Hartmannu    Date: 9.09.08    Time: 13:42
' Updated in $/CKAG/admin
' ITA 2152 und 2158
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 13.06.08   Time: 15:04
' Updated in $/CKAG/admin
' Speicherung der Views aus Cache genommen & in Session geschrieben
' Freitag der 13.
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 11.04.08   Time: 15:47
' Updated in $/CKAG/admin
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 14:47
' Created in $/CKAG/admin
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 6.12.07    Time: 14:36
' Updated in $/CKG/Admin/AdminWeb
' ITA: 1440
' 
' *****************  Version 9  *****************
' User: Uha          Date: 30.08.07   Time: 12:36
' Updated in $/CKG/Admin/AdminWeb
' ITA 1280: Paßwortversand im Web auf Benutzerwunsch
' 
' *****************  Version 8  *****************
' User: Uha          Date: 27.08.07   Time: 17:13
' Updated in $/CKG/Admin/AdminWeb
' ITA 1269: Archivverwaltung/-zuordnung jetzt auch per Web
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 1.08.07    Time: 10:14
' Updated in $/CKG/Admin/AdminWeb
' Fehlerbehandlung GroupManagement Funktion FillEdit eingefügt
' 
' *****************  Version 6  *****************
' User: Uha          Date: 13.03.07   Time: 10:53
' Updated in $/CKG/Admin/AdminWeb
' History-Eintrag vorbereitet
' 
' ************************************************
