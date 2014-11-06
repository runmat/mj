
Imports CKG.Base.Kernel.Admin
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Services.PageElements
Imports CKG.Base.Kernel
Imports CKG
Imports Telerik.Web.UI

Public Class LogViewer
    Inherits System.Web.UI.Page

#Region " Vom Web Form Designer generierter Code "

    'Dieser Aufruf ist für den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: Diese Methode ist für den Web Form-Designer erforderlich
        'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
        InitializeComponent()
    End Sub

#End Region

    Private m_User As User
    Private m_App As App

    Private m_blnShowDetails() As Boolean
    Private m_objTrace As Base.Kernel.Logging.Trace

    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents lblDownloadTip As System.Web.UI.WebControls.Label
    Protected WithEvents lnkExcel As System.Web.UI.WebControls.HyperLink

#Region "Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        AdminAuth(Me, m_User, AdminLevel.Organization)

        GetAppIDFromQueryString(Me)

        Try
            m_App = New App(m_User)

            lblError.Text = ""

            If Not IsPostBack Then
                FillForm()
                TblLog.Visible = False
                trGruppe.Visible = False
                trOrganisation.Visible = False
            Else
                If Session("m_objTrace") IsNot Nothing Then
                    m_objTrace = CType(Session("m_objTrace"), Base.Kernel.Logging.Trace)
                End If
            End If

        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "LogViewer", "Page_Load", ex.ToString)

            lblError.Text = ex.ToString
        End Try
    End Sub

    Private Sub rgResult_SortCommand(ByVal source As Object, ByVal e As Telerik.Web.UI.GridSortCommandEventArgs) Handles rgResult.SortCommand
        ShrinkGrid()
        FillGrid(rgResult.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub rgResult_PageIndexChanged(ByVal source As Object, ByVal e As Telerik.Web.UI.GridPageChangedEventArgs) Handles rgResult.PageIndexChanged
        ShrinkGrid()
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub rgResult_ItemCommand(ByVal source As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles rgResult.ItemCommand
        If e.CommandSource.ToString = "System.Web.UI.WebControls.ImageButton" Then
            Dim item As DataGridItem
            Dim cell As TableCell
            Dim checkbox As CheckBox
            Dim control As Control

            For Each item In rgResult.Items
                cell = item.Cells(0)
                For Each control In cell.Controls
                    If TypeOf control Is CheckBox Then
                        checkbox = CType(control, CheckBox)
                        If checkbox.Checked Then
                            m_blnShowDetails(item.ItemIndex) = checkbox.Checked
                        End If
                    End If
                Next
            Next

            m_blnShowDetails(e.Item.ItemIndex) = Not m_blnShowDetails(e.Item.ItemIndex)
            FillGrid(rgResult.CurrentPageIndex)
        End If
    End Sub

    Private Sub btnSuche_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSuche.Click

        If Not Me.txtFilterUserName.Visible Then
            ClearEdit()
            txtFilterUserName.Text = "*"
            If TblLog.Visible Then
                CreateList()
            Else
                TblLog.Visible = False
            End If
        Else
            Session.Remove("myUserListView")
            rgSearchResult.CurrentPageIndex = 0
            FillDataGrid()
            TblLog.Visible = False
        End If
    End Sub

    Private Sub rgSearchResult_SortCommand(ByVal source As Object, ByVal e As Telerik.Web.UI.GridSortCommandEventArgs) Handles rgSearchResult.SortCommand
        Dim strSort As String = e.SortExpression
        If Not ViewState("ResultSort") Is Nothing AndAlso ViewState("ResultSort").ToString = strSort Then
            strSort &= " DESC"
        End If
        ViewState("ResultSort") = strSort
        FillDataGrid(strSort)
    End Sub

    Private Sub rgSearchResult_ItemCommand(ByVal source As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles rgSearchResult.ItemCommand
        If e.CommandName = "Edit" Then
            FillEdit(e.CommandArgument, CType(e.CommandSource, LinkButton).Text)
            'FillEdit(e.Item.Cells(0).Text, CType(e.Item.Cells(1).Controls(0), LinkButton).Text)
            rgSearchResult.Items(e.Item.ItemIndex).Selected = True
        End If
    End Sub

    Private Sub rgSearchResult_PageIndexChanged(ByVal source As Object, ByVal e As Telerik.Web.UI.GridPageChangedEventArgs) Handles rgSearchResult.PageIndexChanged
        Me.rgSearchResult.CurrentPageIndex = e.NewPageIndex
        FillDataGrid()
    End Sub

    Private Sub lbcreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbCreate.Click
        CreateList()    
    End Sub

    Private Sub ddlFilterCustomer_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlFilterCustomer.SelectedIndexChanged
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()
        FillGroups(CInt(ddlFilterCustomer.SelectedItem.Value), cn) 'DropDowns fuer Group fuellen
        FillOrganizations(CInt(ddlFilterCustomer.SelectedItem.Value), cn)
        FillAction(cn)

        ' gewählten Benutzer deselektieren
        If Not Me.txtFilterUserName.Visible Then
            ClearEdit()
            txtFilterUserName.Text = "*"
            Result.Visible = False
        End If

    End Sub

#End Region

#Region "Methods"

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        rgSearchResult.Visible = False

        If m_objTrace.StandardLog.Rows.Count = 0 Then
            TblLog.Visible = False
            lblError.Text = "Keine Daten zur Anzeige gefunden."
            Result.Visible = False
        Else
            Result.Visible = True
            TblLog.Visible = True

            ' Angezeigte Einträge Filtern nach Messagetype
            Dim tmpDataView As New DataView()
            Dim strFilter As String = ""
            Dim FilterArgs As String() = ddlMessagetype.SelectedValue.Split(","c)

            If FilterArgs(0) <> String.Empty Then
                strFilter += "Category='" & FilterArgs(0) & "'"
                If FilterArgs.Length > 1 Then
                    For i = 1 To FilterArgs.Length - 1
                        strFilter += " OR Category='" + FilterArgs(i) & "'"
                    Next
                End If
            End If

            tmpDataView = m_objTrace.StandardLog.DefaultView
            tmpDataView.RowFilter = strFilter

            Dim Keys(1) As DataColumn
            Keys(0) = tmpDataView.Table.Columns(0)
            tmpDataView.Table.PrimaryKey = Keys

            Label2.Text = "Datenanzeige: Datensätze ermittelt: " & m_objTrace.StandardLog.Rows.Count & ", Angezeigt: " & tmpDataView.Count
            If ddlMessagetype.SelectedIndex <> 0 Then
                Label2.Text += ", ACHTUNG: Filter aktiv!"
            End If

            Dim intTempPageIndex As Int32 = intPageIndex
            Dim strTempSort As String = ""
            Dim strDirection As String = ""

            If strSort.Trim(" "c).Length > 0 Then
                intTempPageIndex = 0
                strTempSort = strSort.Trim(" "c)
                If (ViewState("Sort") Is Nothing) OrElse (ViewState("Sort").ToString = strTempSort) Then
                    If ViewState("Direction") Is Nothing Then
                        strDirection = "desc"
                    Else
                        strDirection = ViewState("Direction").ToString
                    End If
                Else
                    strDirection = "desc"
                End If

                If strDirection = "asc" Then
                    strDirection = "desc"
                Else
                    strDirection = "asc"
                End If

                ViewState("Sort") = strTempSort
                ViewState("Direction") = strDirection
            Else
                If Not ViewState("Sort") Is Nothing Then
                    strTempSort = ViewState("Sort").ToString
                    If ViewState("Direction") Is Nothing Then
                        strDirection = "asc"
                        ViewState("Direction") = strDirection
                    Else
                        strDirection = ViewState("Direction").ToString
                    End If
                End If
            End If

            If Not strTempSort.Length = 0 Then
                tmpDataView.Sort = strTempSort & " " & strDirection
            End If

            rgResult.CurrentPageIndex = intTempPageIndex

            rgResult.DataSource = tmpDataView
            rgResult.DataBind()

        End If
    End Sub

    Private Sub ShrinkGrid()
        'Dim item As DataGridItem
        Dim item As GridDataItem
        Dim cell As TableCell
        Dim checkbox As CheckBox
        Dim control As Control

        'For Each item In DataGrid1.Items
        For Each item In rgResult.Items
            cell = item.Cells(0)
            For Each control In cell.Controls
                If TypeOf control Is CheckBox Then
                    checkbox = CType(control, CheckBox)
                    checkbox.Checked = False
                End If
            Next
        Next
    End Sub

    Private Sub FillForm()
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()
        FillCustomer(cn) 'DropDowns fuer Customer fuellen
        FillGroups(CInt(ddlFilterCustomer.SelectedItem.Value), cn) 'DropDowns fuer Group fuellen
        FillOrganizations(CInt(ddlFilterCustomer.SelectedItem.Value), cn)
        FillAction(cn)
        FillMessagetype()

        If m_User.HighestAdminLevel = AdminLevel.Master Then
            'Wenn DAD-SuperUser:
            lblCustomer.Visible = False 'Label mit Customer-Namen ausblenden
            ddlFilterCustomer.Visible = True 'DropDown zur Customer-Auswahl einblenden
        Else
            'Wenn nicht DAD-Super-User:
            lblCustomer.Text = m_User.Customer.CustomerName 'Customer des angemeldeten Benutzers
            rgSearchResult.Columns(5).Visible = False 'Spalte "Test-Zugang" ausblenden
            If Not m_User.IsCustomerAdmin Then
                'Wenn nicht Customer-Admin:
                rgSearchResult.Columns(4).Visible = False 'Spalte "Customer-Admin" ausblenden
                If m_User.Groups.Count > 0 Then lblGroup.Text = m_User.Groups(0).GroupName 'Gruppe des angemeldeten Benutzers
                lblGroup.Visible = True 'Label mit Gruppen-Namen einblenden
                ddlFilterGroup.Visible = False 'DropDown zur Gruppenauswahl ausblenden
            End If
        End If

        Result.Visible = False 'Suchergebnis ausblenden

        rdpVonDatum.SelectedDate = Today
        rdpBisDatum.SelectedDate = Today
    End Sub

    Private Sub FillGroups(ByVal intCustomerID As Integer, ByVal cn As SqlClient.SqlConnection)
        Dim dtGroups As New Kernel.GroupList(intCustomerID, cn, m_User.Customer.AccountingArea)
        FillGroup(ddlFilterGroup, True, dtGroups)
    End Sub

    Private Sub FillGroup(ByVal ddl As DropDownList, ByVal blnAllNone As Boolean, ByVal dtgroups As Kernel.GroupList)
        If blnAllNone Then dtgroups.AddAllNone(True, False)
        With ddl
            .Items.Clear()
            Dim dv As DataView = dtgroups.DefaultView
            dv.Sort = "GroupName"
            .DataSource = dv
            .DataTextField = "GroupName"
            .DataValueField = "GroupID"
            .DataBind()
            If blnAllNone Then .Items.FindByValue("0").Selected = True
        End With
    End Sub

    Private Sub FillOrganizations(ByVal intCustomerID As Integer, ByVal cn As SqlClient.SqlConnection)
        Dim dtOrganizations As New OrganizationList(intCustomerID, cn, m_User.Customer.AccountingArea)
        FillOrganization(ddlFilterOrganization, True, dtOrganizations)
    End Sub

    Private Sub FillOrganization(ByVal ddl As DropDownList, ByVal blnAllNone As Boolean, ByVal dtOrganizations As OrganizationList)
        If blnAllNone Then dtOrganizations.AddAllNone(True, False)
        With ddl
            .Items.Clear()
            Dim dv As DataView = dtOrganizations.DefaultView
            dv.Sort = "OrganizationName"
            .DataSource = dv
            .DataTextField = "OrganizationName"
            .DataValueField = "OrganizationID"
            .DataBind()

            If blnAllNone Then .Items.FindByValue("0").Selected = True
        End With
    End Sub

    Private Sub FillCustomer(ByVal cn As SqlClient.SqlConnection)
        Dim dtCustomers As Kernel.CustomerList
        dtCustomers = New Kernel.CustomerList(m_User.Customer.AccountingArea, cn)

        dtCustomers.AddAllNone(True, False)
        With ddlFilterCustomer
            Dim dv As DataView = dtCustomers.DefaultView
            dv.Sort = "Customername"
            .DataSource = dv
            .DataTextField = "Customername"
            .DataValueField = "CustomerID"
            .DataBind()
            .Items.FindByValue(m_User.Customer.CustomerId.ToString).Selected = True
        End With
    End Sub

    Private Sub FillMessagetype()

        Dim item1 As New ListItem("-- Alle --", "")
        Dim item2 As New ListItem("Kontrollnachrichten", "APP,INF")
        Dim item3 As New ListItem("Debuginfos", "DBG,ERR,WRN")
        Dim item4 As New ListItem("Fehler", "ERR,WRN")

        ddlMessagetype.Items.Add(item1)
        ddlMessagetype.Items.Add(item2)
        ddlMessagetype.Items.Add(item3)
        ddlMessagetype.Items.Add(item4)

        ddlMessagetype.DataBind()

    End Sub

    Private Sub FillDataGrid()
        Dim strSort As String = "UserID"
        If Not ViewState("ResultSort") Is Nothing Then
            strSort = ViewState("ResultSort").ToString
        End If
        FillDataGrid(strSort)

    End Sub

    Private Sub FillDataGrid(ByVal strSort As String)
        Result.Visible = True
        rgSearchResult.Visible = True

        Dim dvUser As DataView

        If Not Session("myUserListView") Is Nothing Then
            dvUser = CType(Session("myUserListView"), DataView)
        Else
            Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            cn.Open()

            Dim dtUser As New Kernel.UserList(txtFilterUserName.Text, _
                                                   CInt(ddlFilterCustomer.SelectedItem.Value), _
                                                   CInt(ddlFilterGroup.SelectedItem.Value), _
                                                   CInt(ddlFilterOrganization.SelectedItem.Value), _
                                                   cn, _
                                                   False, _
                                                   -1, _
                                                   m_User.Customer.AccountingArea)
            dvUser = dtUser.DefaultView
            Session("myUserListView") = dvUser
        End If

        dvUser.Sort = strSort

        If dvUser.Count > rgSearchResult.PageSize Then
            rgSearchResult.PagerStyle.Visible = True
        Else
            rgSearchResult.PagerStyle.Visible = False
        End If

        With rgSearchResult
            .DataSource = dvUser
            .DataBind()
        End With

    End Sub

    Private Sub FillAction(ByVal cn As SqlClient.SqlConnection)
        Dim dt As ApplicationList = Nothing
        Select Case m_User.HighestAdminLevel
            Case AdminLevel.Master
                If Not ddlFilterCustomer.SelectedIndex = 0 OrElse Not ddlFilterCustomer.SelectedIndex = 1 Then
                    dt = New ApplicationList(CInt(ddlFilterCustomer.SelectedValue), cn)
                    dt.GetAssigned()
                Else
                    dt = New ApplicationList(cn)
                End If
            Case AdminLevel.FirstLevel
                If Not ddlFilterCustomer.SelectedIndex = 0 OrElse Not ddlFilterCustomer.SelectedIndex = 1 Then
                    dt = New ApplicationList(CInt(ddlFilterCustomer.SelectedValue), cn)
                    dt.GetAssigned()
                Else
                    dt = New ApplicationList(cn)
                End If
            Case AdminLevel.Customer
                dt = New ApplicationList(m_User.Customer.CustomerId, cn)
                dt.GetAssigned()
            Case AdminLevel.Organization
                dt = New ApplicationList(m_User.Groups(0).GroupId, m_User.Customer.CustomerId, cn)
                dt.GetAssigned()
        End Select

        If dt.Rows.Count = 0 Then
            ddlAction.Items.Clear()
            lbCreate.Enabled = False
            Exit Sub
        Else
            lbCreate.Enabled = True
        End If

        Dim str(7) As String
        If m_User.HighestAdminLevel = AdminLevel.Master Then
            str(0) = "Admin - Anwendungsverwaltung"
        Else
            str(0) = "-"
        End If
        If m_User.HighestAdminLevel = AdminLevel.Master Then
            str(1) = "Admin - Spaltenübersetzungen"
        Else
            str(1) = "-"
        End If
        If m_User.HighestAdminLevel >= AdminLevel.Customer Then
            str(2) = "Admin - Kundenverwaltung"
        Else
            str(2) = "-"
        End If
        If m_User.HighestAdminLevel >= AdminLevel.Organization Then
            str(3) = "Admin - Gruppenverwaltung"
            str(4) = "Admin - Benutzerverwaltung"
            str(5) = "Admin - Organisationsverwaltung"
        Else
            str(3) = "-"
            str(4) = "-"
            str(5) = "-"
        End If
        str(6) = ""
        str(7) = "Admin - Kennwortänderung"

        Dim strI As String
        Dim dr As DataRow
        For Each strI In str
            If strI <> "-" Then
                dr = dt.NewRow
                dr("AppFriendlyName") = strI
                If strI = "" Then
                    dr("AppType") = "None"
                Else
                    dr("AppType") = "Admin"
                End If
                dt.Rows.Add(dr)
            End If
        Next

        dt.DefaultView.Sort = "AppFriendlyName"
        dt.DefaultView.RowFilter = "AppType='Change'"
        With ddlAction
            .DataTextField = "AppFriendlyName"
            .DataValueField = "AppFriendlyName"
            .DataSource = dt.DefaultView
            .DataBind()
        End With
    End Sub

    Private Sub SearchMode(Optional ByVal blnSearchMode As Boolean = True)
        With Me
            .lblUserName.Visible = Not blnSearchMode
            .txtFilterUserName.Visible = blnSearchMode
            If blnSearchMode Then
                .btnSuche.Text = ">> Benutzer suchen"
            Else
                .btnSuche.Text = ">> Deselektieren"
                .Result.Visible = blnSearchMode
            End If
        End With
    End Sub

    Private Function FillEdit(ByVal strId As String, ByVal strName As String) As Boolean
        With Me
            .txtUserID.Text = strId
            .lblUserName.Text = strName
        End With
        SearchMode(False)
    End Function

    Private Sub ClearEdit()
        With Me
            .txtUserID.Text = "-1"
            .lblUserName.Text = ""
        End With
        SearchMode(True)
    End Sub

    Protected Sub rgResult_DetailTableDataBind(sender As Object, e As Telerik.Web.UI.GridDetailTableDataBindEventArgs) Handles rgResult.DetailTableDataBind
        ' Sucht anhand des Schlüsselwertes der Spalte, die passenden Detailwerte und weißt diese der Detailtable als Datasource zu
        If e.DetailTableView.DataSourceID = "" Then
            Dim dataItem As GridDataItem = e.DetailTableView.ParentItem
            Dim ID As String = dataItem.GetDataKeyValue("ID").ToString()
            Dim dt As DataTable = m_objTrace.LogDetails(CInt(ID))
            If Not dt Is Nothing Then
                If dt.Rows.Count > 0 Then
                    e.DetailTableView.DataSource = dt
                End If
            Else
                e.Canceled = True
                e.DetailTableView.Visible = False
            End If
        End If

    End Sub

    Private Sub CreateList()
        lblError.Text = ""
        If Not IsDate(rdpVonDatum.SelectedDate) Then
            If Not IsStandardDate(rdpVonDatum.SelectedDate.ToString()) Then
                If Not IsSAPDate(rdpVonDatum.SelectedDate.ToString()) Then
                    lblError.Text = "Geben Sie bitte ein gültiges Datum ein!<br>"
                End If
            End If
        End If
        If Not IsDate(rdpBisDatum.SelectedDate) Then
            If Not IsStandardDate(rdpBisDatum.SelectedDate.ToString()) Then
                If Not IsSAPDate(rdpBisDatum.SelectedDate.ToString()) Then
                    lblError.Text = "Geben Sie bitte ein gültiges Datum ein!<br>"
                End If
            End If
        End If
        Dim datAb As Date = rdpVonDatum.SelectedDate
        Dim datBis As Date = rdpBisDatum.SelectedDate
        If datAb > datBis Then
            lblError.Text = "Das Enddatum darf nicht vor dem Anfangsdatum liegen!<br>"
        End If
        If lblError.Text.Length > 0 Then
            Exit Sub
        End If

        m_objTrace = New Base.Kernel.Logging.Trace(m_User.App.Connectionstring, m_User.App.SaveLogAccessSAP)
        Dim blnFill As Boolean = False
        If Me.txtUserID.Text = "-1" Then
            If Me.ddlFilterGroup.SelectedItem.Value = "0" Then
                If Me.ddlFilterCustomer.SelectedItem.Value = "0" Then
                    m_objTrace.AllData(Me.ddlAction.SelectedItem.Value, Me.rdpVonDatum.SelectedDate.ToString(), Me.rdpBisDatum.SelectedDate.ToString())
                    blnFill = True
                Else
                    m_objTrace.CustomerData(Me.ddlFilterCustomer.SelectedItem.Text, Me.ddlAction.SelectedItem.Value, Me.rdpVonDatum.SelectedDate.ToString(), Me.rdpBisDatum.SelectedDate.ToString())
                    blnFill = True
                End If
            Else
                m_objTrace.GroupData(Me.ddlFilterCustomer.SelectedItem.Text, Me.ddlFilterGroup.SelectedItem.Value, Me.ddlAction.SelectedItem.Value, Me.rdpVonDatum.SelectedDate.ToString(), Me.rdpBisDatum.SelectedDate.ToString())
                blnFill = True
            End If
        Else
            If Me.ddlAction.SelectedItem.Value = String.Empty Then
                m_objTrace.UserData(Me.lblUserName.Text, Me.rdpVonDatum.SelectedDate.ToString(), Me.rdpBisDatum.SelectedDate.ToString())
                blnFill = True
            Else
                m_objTrace.UserData(Me.lblUserName.Text, Me.ddlAction.SelectedItem.Value, Me.rdpVonDatum.SelectedDate.ToString(), Me.rdpBisDatum.SelectedDate.ToString())
                blnFill = True
            End If
        End If
        If blnFill Then
            Me.Result.Visible = True
            FillGrid(0)
            Session("m_objTrace") = m_objTrace
        Else
            Me.Result.Visible = False
            Me.lblError.Text = "Diese Selektion ist nicht auswertbar."
        End If
    End Sub

#End Region

End Class

