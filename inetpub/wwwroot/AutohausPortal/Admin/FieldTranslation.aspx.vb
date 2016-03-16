Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common

Partial Public Class FieldTranslation
    Inherits Page

#Region "Properties"

    Private Property Refferer() As String
        Get
            Return ViewState.Item("refferer").ToString
        End Get
        Set(ByVal value As String)
            ViewState.Item("refferer") = value
        End Set
    End Property

#End Region

#Region " Membervariables "

    Private m_User As User
    Private m_App As App
    Private m_context As HttpContext = HttpContext.Current
    Protected WithEvents GridNavigation1 As Global.Admin.GridNavigation

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        ' Hier Benutzercode zur Seiteninitialisierung einfügen
        m_User = GetUser(Me)
        lblHead.Text = "Spaltenübersetzungen für"
        FormAuth(Me, m_User)
        GridNavigation1.setGridElment(dgSearchResult)
        Dim cn As New SqlClient.SqlConnection()



        Try
            m_App = New App(m_User)

            If Not IsPostBack Then
                'wenn erstmaliger Seitenaufruf, Füllen der Kunden und Namens DropDownBoxen sowie das "Cleanen" der EditFelder, warum auch immer weil beim Initialaufruf ja dort noch nichts enthalten sein kann. JJ2007.11.12
                lblError.Text = ""
                trStandard.Visible = False

                If Not Request.UrlReferrer Is Nothing Then
                    Refferer = Request.UrlReferrer.ToString
                Else
                    Refferer = "Selection.aspx"
                End If

                If m_User.Customer.AccountingArea = -1 Then
                    'Admin übergeordneter Firma, link einblenden, neubutton einblenden
                    lnkAppManagement.Visible = True
                    lbtnNew.Enabled = True

                End If

                lblAppURL.Text = CStr(Request.QueryString("AppURL"))



                If lblAppURL.Text = String.Empty Then
                    lbtnNew.Visible = False
                Else
                    If m_User.IsCustomerAdmin Then

                    End If
                    cn = New SqlClient.SqlConnection(m_User.App.Connectionstring)
                    If Not cn.State = ConnectionState.Open Then
                        cn.Open()
                    End If
                    Dim _CustomerList As Kernel.CustomerList
                    _CustomerList = New Kernel.CustomerList(m_User.Customer.AccountingArea, cn)

                    Dim vwCustomer As DataView = _CustomerList.DefaultView
                    vwCustomer.Sort = "Customername"
                    ddlCustomer.DataSource = vwCustomer
                    ddlCustomer.DataValueField = "CustomerID"
                    ddlCustomer.DataTextField = "Customername"
                    ddlCustomer.DataBind()
                    ddlCustomer.ClearSelection()

                    Dim li As ListItem = ddlCustomer.Items.FindByValue(m_User.Customer.CustomerId.ToString)
                    If Not li Is Nothing Then
                        li.Selected = True
                    End If

                    Dim dtGroup As Kernel.GroupList
                    Dim dvGroup As DataView
                    Dim iCustomerID As Integer

                    If m_User.FirstLevelAdmin Then
                        iCustomerID = CInt(m_User.Customer.CustomerId)
                    ElseIf m_User.HighestAdminLevel >= 2 Then
                        iCustomerID = CInt(ddlCustomer.SelectedItem.Value)
                    End If

                    dtGroup = New Kernel.GroupList(iCustomerID, _
                                    cn, _
                                    m_User.Customer.AccountingArea, True)
                    dvGroup = dtGroup.DefaultView
                    Session.Add("myGroupListView", dvGroup)

                    dvGroup = CType(Session("myGroupListView"), DataView)



                    dvGroup.Sort = "GroupName"
                    ddlGroup.DataSource = dvGroup
                    ddlGroup.DataValueField = "GroupID"
                    ddlGroup.DataTextField = "GroupName"
                    ddlGroup.DataBind()

                    ddlGroup.ClearSelection()

                    If m_User.FirstLevelAdmin Then
                        li = ddlGroup.Items.FindByText(m_User.Reference.ToString)
                    ElseIf m_User.HighestAdminLevel >= 2 Then
                        li = ddlGroup.Items.FindByValue("0")
                    End If
                    If Not li Is Nothing Then
                        li.Selected = True
                    End If



                    Dim _LanguageList As New Kernel.LanguageList(cn)
                    Dim vwLanguage As DataView = _LanguageList.DefaultView
                    ddlLanguage.DataSource = vwLanguage
                    ddlLanguage.DataValueField = "LanguageID"
                    ddlLanguage.DataTextField = "Language"
                    ddlLanguage.DataBind()
                    ddlLanguage.ClearSelection()
                    ddlLanguage.Items.FindByValue("1").Selected = True

                    FillForm() 'Stellt den Seitenaufbau entsprechend der eines Initalaufrufes dar, JJ 2007.11.12

                    If m_User.FirstLevelAdmin Then
                        ddlCustomer.Enabled = False
                        ddlGroup.Enabled = False
                        lnkCustomerManagement.Visible = False
                        lnkGroupManagement.Visible = False
                        lnkAppManagement.Visible = False
                        lnkArchivManagement.Visible = False
                        lnkOrganizationManagement.Visible = False
                        lnkUserManagement.Visible = False
                    ElseIf m_User.HighestAdminLevel = AdminLevel.Customer Then
                        ddlCustomer.Enabled = False
                        ddlGroup.Enabled = True
                        lnkAppManagement.Visible = False
                        lnkArchivManagement.Visible = False
                    ElseIf m_User.HighestAdminLevel = AdminLevel.Master Then
                        trEingabeSpalte.Visible = True
                    End If
                End If
            End If
        Catch ex As Exception
            lblError.Text = ex.ToString
            lblError.Visible = True
            m_App.WriteErrorText(1, m_User.UserName, "FieldTranslation", "PageLoad", lblError.Text)
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Sub

#Region " Data and Function "

    Private Sub EnableRadioButtons(ByVal blnValue As Boolean)
        rbLabel.Enabled = blnValue
        rbLinkButton.Enabled = blnValue
        rbRadioButton.Enabled = blnValue
        rbTableRow.Enabled = blnValue
        rbGridColumn.Enabled = blnValue
        rbTextBox.Enabled = blnValue
    End Sub

    Private Sub FillForm()

        Tablex1.Visible = False 'Tabelle für das Editieren von Feldübersetzungen ausblenden JJ2007.11.12
        Result.Visible = False 'Datagrid/Tabelle für die Feldübersetzungen ausblenden JJ2007.11.12
        Search(False, True, True, True, True) 'Setzt gewisse Kriterien wie Maske aussehen soll, Einblenden von Editierung, FeldübersetzungGrid, Auswahl innerhalb der DropDownListen, nicht vollständig nachvollziehbar wann welche Parameter zu setzen sind. JJ2007.11.12
    End Sub

    Private Sub FillDataGrid()
        Dim strSort As String = "Field"
        If Not ViewState("ResultSort") Is Nothing Then 'wenn eine Sortierung bereits Feststeht, beibehlaten bei Neubefüllung JJ2007.11.12
            strSort = ViewState("ResultSort").ToString
        End If
        FillDataGrid(strSort)
    End Sub

    Private Sub FillDataGrid(ByVal strSort As String)
        Result.Visible = True 'tabellenZeile die DataGrid beinhaltet auf anzeigen JJ2007.11.12
        Dim dvFieldTranslation As DataView

        If Not m_context.Cache("myColListView") Is Nothing Then
            dvFieldTranslation = CType(m_context.Cache("myColListView"), DataView) 'wenn Cach oBjekt myColListView vorhanden Grid aus dieser Quelle Füllen JJ2007.11.12
        Else
            Dim dtFieldTranslation As Kernel.FieldTranslationList
            Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            Try
                cn.Open()
                If m_User.HighestAdminLevel = AdminLevel.Master Then
                    dtFieldTranslation = New Kernel.FieldTranslationList(lblAppURL.Text, CInt(ddlCustomer.SelectedItem.Value), CInt(ddlLanguage.SelectedItem.Value), cn, "X", CInt(ddlGroup.SelectedItem.Value))
                Else
                    dtFieldTranslation = New Kernel.FieldTranslationList(lblAppURL.Text, CInt(ddlCustomer.SelectedItem.Value), CInt(ddlLanguage.SelectedItem.Value), cn, CInt(ddlGroup.SelectedItem.Value))
                End If

                dvFieldTranslation = dtFieldTranslation.DefaultView

                Session("myColListView") = dvFieldTranslation
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End If
        dvFieldTranslation.Sort = strSort
        If dvFieldTranslation.Count = 0 Then
            lblError.Text = "Übersetzung existiert noch nicht."
        Else


            With dgSearchResult
                .DataSource = dvFieldTranslation
                .DataBind()
            End With
            If Not m_User.Customer.AccountingArea = -1 Then
                If ddlCustomer.SelectedValue <> "1" Then
                    'alle Ändern Buttons deaktivieren wenn es kein SuperAdmin und es Firma1, also standardübersetzung ist
                    For Each tmpItem As GridViewRow In dgSearchResult.Rows
                        '    CType(tmpItem.FindControl("lbAendern"), LinkButton).Visible = False
                        '    CType(tmpItem.FindControl("lbAnlegen"), LinkButton).Visible = True
                        CType(tmpItem.FindControl("ibtnDel"), ImageButton).Visible = False
                        CType(tmpItem.FindControl("DelEnabled"), Image).Visible = True
                    Next
                End If
            End If

        End If
    End Sub

    Private Sub SetInput(ByVal FTrans As Kernel.FieldTranslation)
        'Die Eingabemaske der Feldübersetzung in bezug des Ausgewählten Feldes füllen, dieses geschieht über das übergeben Fieldtranslation Objekt JJ 2007.11.12
        lblFieldID.Text = FTrans.ApplicationFieldID.ToString
        lblField.Text = FTrans.Field
        txtFieldName.Text = FTrans.FieldName
        rbLabel.Checked = False
        rbTableRow.Checked = False
        rbLinkButton.Checked = False
        rbRadioButton.Checked = False
        rbGridColumn.Checked = False
        rbTextBox.Checked = False

        'immer auf Visible False setzten, bei Case TXT wird die Option eingeblendet JJ 2007.11.12
        lbl_TextTooltip.Visible = False
        txt_Tooltip.Visible = False
        'wenn Visible False, auch gleichzeitig Inhalt löschen, da sonst Inhalt wenn vorher eine txt bearbeitet wurde, wahrscheinlich auch in die Datenbank von einem nicht, txt Feld geschrieben wird. JJ2007.11.12
        txt_Tooltip.Text = String.Empty

        Select Case UCase(FTrans.FieldType)
            Case "LBL"
                rbLabel.Checked = True
            Case "TR"
                rbTableRow.Checked = True
            Case "LB"
                rbLinkButton.Checked = True
            Case "RB"
                rbRadioButton.Checked = True
            Case "COL"
                rbGridColumn.Checked = True
            Case "TXT"
                rbTextBox.Checked = True
                lbl_TextTooltip.Visible = True
                txt_Tooltip.Visible = True

        End Select
        ddlCustomer.ClearSelection()
        Dim Li As ListItem
        Li = ddlCustomer.Items.FindByValue(FTrans.CustomerID.ToString)
        If Not Li Is Nothing Then
            Li.Selected = True
        End If
        ddlLanguage.ClearSelection()
        ddlLanguage.Items.FindByValue(FTrans.LanguageID.ToString).Selected = True
        cbxVisible.Checked = FTrans.Visibility
        chkEingabe.Checked = FTrans.EingebeFeld
        txtContent.Text = FTrans.Content
        txt_Tooltip.Text = FTrans.ToolTip
    End Sub

    Private Function FillEdit(ByVal strAppURL As String, ByVal strFieldType As String, ByVal strFieldName As String,
                              ByVal intCustomerID As Integer, ByVal intLanguageID As Integer, ByVal intGroupID As Integer) As Boolean
        SearchMode(False)
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()
        Dim _FieldTrans As Kernel.FieldTranslation
        lblError.Text = ""

        Try
            _FieldTrans = New Kernel.FieldTranslation(strAppURL, strFieldType, strFieldName, intCustomerID, intLanguageID, cn, intGroupID)
            SetInput(_FieldTrans)
        Catch ex As Exception
            Dim intTempCustomerID As Integer = CInt(ddlCustomer.SelectedItem.Value)
            Dim intTempLanguageID As Integer = CInt(ddlLanguage.SelectedItem.Value)
            Dim intTempGroupID As Integer = CInt(ddlGroup.SelectedItem.Value)
            FillEdit(CInt(lblFieldID.Text))
            lblError.Text = "Übersetzung existiert noch nicht."
            txtContent.Text = ""
            lblFieldID.Text = "-1"
            ddlCustomer.ClearSelection()
            ddlCustomer.Items.FindByValue(intTempCustomerID.ToString).Selected = True
            ddlGroup.ClearSelection()
            ddlGroup.Items.FindByValue(intTempGroupID.ToString).Selected = True
            ddlLanguage.ClearSelection()
            ddlLanguage.Items.FindByValue(intTempLanguageID.ToString).Selected = True
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
        Return True
    End Function

    Private Function FillEdit(ByVal intFieldId As Integer) As Boolean
        SearchMode(False)
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()
        Dim _FieldTrans As Kernel.FieldTranslation
        lblError.Text = ""

        Try
            'holt sich die Aktuellen Daten aus DB beim Editieren JJ2007.11.12
            _FieldTrans = New Kernel.FieldTranslation(intFieldId, cn)
            'füllt die Felder der Editierungsmaske JJ 2007.11.12
            SetInput(_FieldTrans)
        Catch ex As Exception
            Dim intTempCustomerID As Integer = CInt(ddlCustomer.SelectedItem.Value)
            Dim intTempLanguageID As Integer = CInt(ddlLanguage.SelectedItem.Value)
            Dim intTempGroupID As Integer = CInt(ddlGroup.SelectedItem.Value)
            FillEdit(CInt(lblFieldIDSave.Text))
            lblError.Text = "Übersetzung existiert noch nicht."
            txtContent.Text = ""
            lblFieldID.Text = "-1"
            ddlCustomer.ClearSelection()
            ddlCustomer.Items.FindByValue(intTempCustomerID.ToString).Selected = True
            ddlGroup.ClearSelection()
            ddlGroup.Items.FindByValue(intTempGroupID.ToString).Selected = True
            ddlLanguage.ClearSelection()
            ddlLanguage.Items.FindByValue(intTempLanguageID.ToString).Selected = True
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try

        Return True
    End Function

    Private Sub ClearEdit(ByVal blnClearDdlSelection As Boolean)
        'löscht alle eingaben der Feldübersetzung, bzw Setzt es auf den Standardtwert der Radiobuttonsauswahl zurück. JJ2007.11.12
        lblFieldID.Text = "-1"
        lblFieldIDSave.Text = "-1"
        txtContent.Text = ""
        txtFieldName.Text = ""
        lblField.Text = ""
        rbLabel.Checked = True
        rbLinkButton.Checked = False
        rbRadioButton.Checked = False
        rbTableRow.Checked = False
        cbxVisible.Visible = True
        chkEingabe.Checked = False
        'Buttons
        lbtnSave.Visible = True
        lbtnDelete.Visible = False

        If blnClearDdlSelection Then
            ddlCustomer.ClearSelection()
            ddlCustomer.Items.FindByValue(m_User.Customer.CustomerId.ToString).Selected = True
            Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            cn.Open()
            Dim dtGroups As New Kernel.GroupList(m_User.Customer.CustomerId, cn, m_User.Customer.AccountingArea)
            ddlGroup.Items.Clear()
            Dim dv As DataView = dtGroups.DefaultView
            dv.Sort = "GroupName"
            ddlGroup.DataSource = dv
            ddlGroup.DataTextField = "GroupName"
            ddlGroup.DataValueField = "GroupID"
            ddlGroup.DataBind()
            If ddlCustomer.SelectedItem.Value = "1" Then

                Dim liGroup As New ListItem("- keine Auswahl -", "0")
                ddlGroup.Items.Add(liGroup)
                liGroup.Selected = True

            End If
            ddlLanguage.ClearSelection()
            ddlLanguage.Items.FindByValue("1").Selected = True
        End If

        LockEdit(False)

    End Sub

    Private Sub LockEdit(ByVal blnLock As Boolean)
        'sperrt Eingabefelder laut dem Übergabeparameter und setzt deren Farbe dementsprechend JJ 2007.11.12
        Dim strBackColor As String = "White"
        If blnLock Then
            strBackColor = "LightGray"
        End If
        txtContent.Enabled = Not blnLock
        txtContent.BackColor = Drawing.Color.FromName(strBackColor)
        txtFieldName.Enabled = Not blnLock
        txtFieldName.BackColor = Drawing.Color.FromName(strBackColor)
        rbLabel.Enabled = Not blnLock
        rbLabel.BackColor = Drawing.Color.FromName(strBackColor)
        rbLinkButton.Enabled = Not blnLock
        rbLinkButton.BackColor = Drawing.Color.FromName(strBackColor)
        rbRadioButton.Enabled = Not blnLock
        rbRadioButton.BackColor = Drawing.Color.FromName(strBackColor)
        rbTableRow.Enabled = Not blnLock
        rbTableRow.BackColor = Drawing.Color.FromName(strBackColor)
        cbxVisible.Enabled = Not blnLock
        cbxVisible.BackColor = Drawing.Color.FromName(strBackColor)
        ddlCustomer.Enabled = Not blnLock
        ddlCustomer.BackColor = Drawing.Color.FromName(strBackColor)
        ddlLanguage.Enabled = Not blnLock
        ddlLanguage.BackColor = Drawing.Color.FromName(strBackColor)
        rbTextBox.Enabled = Not blnLock
        rbTextBox.BackColor = Drawing.Color.FromName(strBackColor)
    End Sub

    Private Sub EditCreateMode(ByVal strFieldType As String, ByVal strFieldName As String)
        Dim strCustomerID As String = ddlCustomer.SelectedItem.Value
        Dim strLanguageID As String = ddlLanguage.SelectedItem.Value
        Dim strGroupID As String = ddlGroup.SelectedItem.Value
        If Not FillEdit(lblAppURL.Text, strFieldType, strFieldName, 1, 1, 0) Then
            LockEdit(True)
            lbtnSave.Enabled = False
        Else
            lblFieldID.Text = -1
            ddlCustomer.ClearSelection()
            ddlCustomer.Items.FindByValue(strCustomerID).Selected = True
            ddlLanguage.ClearSelection()
            ddlLanguage.Items.FindByValue(strLanguageID).Selected = True
            ddlGroup.ClearSelection()
            ddlGroup.Items.FindByValue(strGroupID).Selected = True
            lbtnSave.Enabled = True
        End If
        lbtnCancel.Text = "Verwerfen"
    End Sub

    Private Sub EditEditMode(ByVal intFieldId As Integer)
        If Not FillEdit(intFieldId) Then
            LockEdit(True)
            lbtnSave.Enabled = False
        Else
            lbtnSave.Enabled = True
        End If
        lbtnCancel.Text = "Verwerfen"
    End Sub

    Private Sub EditDeleteMode(ByVal intFieldId As Integer)
        If Not FillEdit(intFieldId) Then
            lbtnDelete.Enabled = False
        Else
            lblMessage.Text = "Möchten Sie die Spalenübersetzung wirklich löschen?"
            lbtnDelete.Enabled = True
        End If
        LockEdit(True)
        lbtnCancel.Text = "Abbrechen"
        lbtnSave.Visible = False
        lbtnDelete.Visible = True
    End Sub

    Private Sub SearchMode(Optional ByVal blnSearchMode As Boolean = True)
        'setzt diverse Eingabemasken, dropdownlisten, Gridausgaben je nach übergabeparameter und anderen Kriterien auf Enable bzw Visible, in welchem Zusammenhang ist unklar JJ2007.11.12
        Tablex1.Visible = Not blnSearchMode 'Tablex1 Ist die Eingabemaske für Feldübersetzungen JJ2007.11.12
        ddlCustomer.Enabled = Not Tablex1.Visible 'ddlCustomer.Enabled wenn nicht Eingabemaske Visible=false  JJ2007.11.12
        ddlLanguage.Enabled = Not Tablex1.Visible ' ddlLanguage.Enabled wenn nicht Eingabemaske Visible=False JJ2007.11.12
        Result.Visible = blnSearchMode 'trSearchResult= Tabelle mit Resultaten der aktuellen Feldübersetzung (Grid)JJ2007.11.12
        lbtnSave.Visible = Not blnSearchMode 'Setzten der Sichbarkeit von den Linkbuttons JJ2007.11.12
        lbtnCancel.Visible = Not blnSearchMode 'Setzten der Sichbarkeit von den Linkbuttons JJ2007.11.12
        lbtnNew.Visible = blnSearchMode 'Setzten der Sichbarkeit von den Linkbuttons JJ2007.11.12
        If Not m_User.HighestAdminLevel = AdminLevel.Master Then
            lbtnNew.Visible = False
        End If
    End Sub

    Private Sub Search(ByVal blnClearDdlSelection As Boolean, Optional ByVal blnRefillDataGrid As Boolean = False, Optional ByVal blnResetSelectedIndex As Boolean = False, Optional ByVal blnResetPageIndex As Boolean = False, Optional ByVal blnClearCache As Boolean = False)
        'bei Initalload sind Parameter False,True,True,True
        ClearEdit(blnClearDdlSelection) 'setzt editfelder Initialwerte z.B. ausgewählt ist Label, übersetzungsfelder Leer, der Übergabeparameter dient dazu ob die Dropdownlisten auch auf Initial gestellt werden sollen(Firma1) JJ2007.11.12 
        If blnClearCache Then 'löscht ein Cache Objekt mit dem key "myColListView", weiß noch nicht für was das gut ist, JJ2007.11.12
            ' m_context.Cache.Remove("myColListView")
            Session.Remove("myColListView")
        End If
        If blnResetSelectedIndex Then dgSearchResult.SelectedIndex = -1 'wenn True keine Auswahl des SelectedIndex des datagrids JJ2007.11.12
        If blnResetPageIndex Then dgSearchResult.PageIndex = 0 'wenn True keine erster PageIndex des Grids JJ2007.11.12
        SearchMode() 'mit Optionalen Übergabeparameter, wenn keine kein Übergabeparameter = True JJ2007.11.12
        If blnRefillDataGrid Then FillDataGrid() 'wenn Parameter blnRefillDataGrid =true DataGrid neu Füllen
        ProofRights()
    End Sub

    Private Sub ProofRights()
        ddlCustomer.Enabled = False
        ddlGroup.Enabled = False
        If m_User.HighestAdminLevel = AdminLevel.Master Then
            ddlCustomer.Enabled = True
            ddlGroup.Enabled = True
        ElseIf m_User.HighestAdminLevel = AdminLevel.Customer Then
            ddlGroup.Enabled = True
        End If
    End Sub

#End Region

#Region " Events "

    Private Sub lbtnCancel_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles lbtnCancel.Click
        Search(False, , True)
    End Sub

    Private Sub lbtnNew_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles lbtnNew.Click
        SearchMode(False)
        ClearEdit(True)
        'Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        'cn.Open()
    End Sub

    Private Sub lbtnSave_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles lbtnSave.Click
        Dim cn As SqlClient.SqlConnection
        cn = New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try

            cn.Open()
            Dim intApplicationFieldID As Integer = CInt(lblFieldID.Text)

            Dim strFieldType As String = ""
            If rbLabel.Checked Then
                strFieldType = "lbl"
            ElseIf rbTableRow.Checked Then
                strFieldType = "tr"
            ElseIf rbLinkButton.Checked Then
                strFieldType = "lb"
            ElseIf rbRadioButton.Checked Then
                strFieldType = "rb"
            ElseIf rbGridColumn.Checked Then
                strFieldType = "col"
            ElseIf rbTextBox.Checked Then
                strFieldType = "txt"
            End If

            Dim _FieldTranslation As New Kernel.FieldTranslation(intApplicationFieldID, _
                                                strFieldType, _
                                                txtFieldName.Text, _
                                                lblAppURL.Text, _
                                                CInt(ddlCustomer.SelectedItem.Value), _
                                                CInt(ddlLanguage.SelectedItem.Value), _
                                                cbxVisible.Checked, _
                                                txtContent.Text, _
                                                txt_Tooltip.Text, _
                                                chkEingabe.Checked, _
                                                 CInt(ddlGroup.SelectedItem.Value))
            _FieldTranslation.Save(cn, "")
            Search(False, True, True, , True)
            lblMessage.Text = "Die Änderungen wurden gespeichert."
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "FieldTranslation", "lbtnSave_Click", ex.ToString)

            lblError.Text = ex.Message
            If Not ex.InnerException Is Nothing Then
                lblError.Text &= ": " & ex.InnerException.Message
            End If
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Sub

    Private Sub lbtnDelete_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles lbtnDelete.Click
        Dim cn As SqlClient.SqlConnection
        cn = New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try
            Dim _FieldTranslation As New Kernel.FieldTranslation(CInt(lblFieldID.Text), m_User)
            cn.Open()
            _FieldTranslation.Delete(cn)
            Search(False, True, True, True, True)
            lblMessage.Text = "Die Spaltenübersetzung wurde gelöscht."
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "FieldTranslation", "lbtnDelete_Click", ex.ToString)

            lblError.Text = ex.Message
            If Not ex.InnerException Is Nothing Then
                lblError.Text &= ": " & ex.InnerException.Message
            End If
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Sub

    Private Sub responseBack()
        If String.IsNullOrEmpty(Refferer) Then
            Dim strLinkPrefix As String = "/" & ConfigurationManager.AppSettings("WebAppPath") & "/"
            Response.Redirect(strLinkPrefix & "Start/Selection.aspx")
        Else
            Response.Redirect(Refferer)
        End If
    End Sub

    Private Sub lnkBack_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles lnkBack.Click
        responseBack()
    End Sub

    Private Sub ddlCustomer_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlCustomer.SelectedIndexChanged
        Dim intCustomerID As Integer = CInt(ddlCustomer.SelectedItem.Value)
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()
        If intCustomerID > 0 Then
            ddlCustomer.SelectedItem.Selected = False
            ddlCustomer.Items.FindByValue(intCustomerID.ToString).Selected = True
            Dim dtGroups As New Kernel.GroupList(intCustomerID, cn, m_User.Customer.AccountingArea, True)
            ddlGroup.Items.Clear()
            Dim dv As DataView = dtGroups.DefaultView
            dv.Sort = "GroupName"
            ddlGroup.DataSource = dv
            ddlGroup.DataTextField = "GroupName"
            ddlGroup.DataValueField = "GroupID"
            ddlGroup.DataBind()
        End If
        FillForm()
    End Sub

    Private Sub ddlLanguage_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlLanguage.SelectedIndexChanged
        FillForm()
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.PreRender
        If Tablex1.Visible Then
            lnkBack.Visible = False
            'Wenn Es können nur Übersetzungen in StandardFirma geändert werden, ist Kunde nicht StandardFirma ist keine Änderung der FeldTypen möglich. JJ 2007.11.12
            If ddlCustomer.SelectedItem.Value = "1" And ddlLanguage.SelectedItem.Value = "1" Then
                EnableRadioButtons(True)
            Else
                EnableRadioButtons(False)
            End If
        Else
            lnkBack.Visible = True
            If ddlCustomer.SelectedItem.Value = "1" And ddlLanguage.SelectedItem.Value = "1" Then
                Dim item As GridViewRow
                Dim cell As TableCell
                Dim control As Control
                Dim lnkButton As LinkButton
                For Each item In dgSearchResult.Rows
                    cell = item.Cells(item.Cells.Count - 1) 'letztes ItemCell (button Löschen) im Grid JJ2007.11.12
                    For Each control In cell.Controls
                        If TypeOf control Is LinkButton Then
                            lnkButton = CType(control, LinkButton)
                            lnkButton.Enabled = False 'alle disabeln StandardFirma,  bei Kunden dürfen Felder gelöscht werden, Wenn in der Standardfirma einmal eine Übersetzung vorhanden ist darf diese nicht mehr gelöscht werden  JJ2007.11.12
                        End If
                    Next
                Next
            End If
        End If
    End Sub

#End Region

    Private Sub rbTextBox_checkedChanged(ByVal sender As System.Object, ByVal e As EventArgs) Handles rbTextBox.CheckedChanged, rbLinkButton.CheckedChanged, rbGridColumn.CheckedChanged, rbRadioButton.CheckedChanged, rbTableRow.CheckedChanged

        If sender Is rbTextBox Then
            'wenn bei einer neuanlegung von Feldübersetzungen auf Textbox geklickt wird, Tooltip EingabeFeld einblenden JJ2007.11.13
            lbl_TextTooltip.Visible = True
            txt_Tooltip.Visible = True
        Else
            'wird wieder ein anderes eingabefeld gewählt, dann ausblenden
            lbl_TextTooltip.Visible = False
            txt_Tooltip.Visible = False
            'wenn Visible False, auch gleichzeitig Inhalt löschen, da sonst Inhalt wenn vorher eine txt bearbeitet wurde, wahrscheinlich auch in die Datenbank von einem nicht, txt Feld geschrieben wird. JJ2007.11.12
            txt_Tooltip.Text = String.Empty
        End If
    End Sub

    Private Sub GridNavigation1_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation1.PagerChanged
        dgSearchResult.PageIndex = PageIndex
        FillDataGrid()
    End Sub

    Private Sub GridNavigation1_PageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillDataGrid()
    End Sub

    Private Sub dgSearchResult_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles dgSearchResult.RowCommand
        Dim CtrlLabel As Label
        Dim CtrlFieldType As Label
        Dim CtrlFieldName As Label
        Dim index As Integer
        Dim row As GridViewRow

        Select Case e.CommandName
            Case "Create"
                index = Convert.ToInt32(e.CommandArgument)
                row = dgSearchResult.Rows(index)
                CtrlLabel = row.Cells(0).FindControl("lblAppID")
                lblFieldIDSave.Text = CtrlLabel.Text
                CtrlFieldType = row.Cells(1).FindControl("lblFieldType")
                CtrlFieldName = row.Cells(2).FindControl("lblFieldName")
                EditCreateMode(CtrlFieldType.Text, CtrlFieldName.Text)
                dgSearchResult.SelectedIndex = row.RowIndex
            Case "Edit"
                index = Convert.ToInt32(e.CommandArgument)
                row = dgSearchResult.Rows(index)
                CtrlLabel = row.Cells(0).FindControl("lblAppID")
                EditEditMode(CInt(CtrlLabel.Text))
                dgSearchResult.SelectedIndex = row.RowIndex
            Case "Del"
                index = Convert.ToInt32(e.CommandArgument)
                row = dgSearchResult.Rows(index)
                CtrlLabel = row.Cells(0).FindControl("lblAppID")
                EditDeleteMode(CInt(CtrlLabel.Text))
                dgSearchResult.SelectedIndex = row.RowIndex
        End Select
    End Sub

    Private Sub dgSearchResult_RowEditing(ByVal sender As Object, ByVal e As GridViewEditEventArgs) Handles dgSearchResult.RowEditing

    End Sub

    Private Sub dgSearchResult_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs) Handles dgSearchResult.Sorting
        Dim strSort As String = e.SortExpression
        If Not ViewState("ResultSort") Is Nothing AndAlso ViewState("ResultSort").ToString = strSort Then
            strSort &= " DESC"
        End If
        ViewState("ResultSort") = strSort

    End Sub

    Protected Sub ddlGroup_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlGroup.SelectedIndexChanged
        FillForm()
    End Sub
End Class