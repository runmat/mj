Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common
Imports Admin.FieldTranslation

Partial Public Class FieldTranslation
    Inherits System.Web.UI.Page

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
    Protected WithEvents GridNavigation1 As Global.CKG.Services.GridNavigation
    Private m_context As HttpContext = HttpContext.Current
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Hier Benutzercode zur Seiteninitialisierung einfügen
        m_User = GetUser(Me)
        lblHead.Text = "Spaltenübersetzungen für"
        AdminAuth(Me, m_User, AdminLevel.Master)
        GridNavigation1.setGridElment(dgSearchResult)
        Dim cn As New SqlClient.SqlConnection()



        Try
            m_App = New App(m_User)

            lblError.Text = ""
            lblMessage.Text = ""

            If Not IsPostBack Then
                'wenn erstmaliger Seitenaufruf, Füllen der Kunden und Namens DropDownBoxen sowie das "Cleanen" der EditFelder, warum auch immer weil beim Initialaufruf ja dort noch nichts enthalten sein kann. JJ2007.11.12
                trStandard.Visible = False

                If Not Me.Request.UrlReferrer Is Nothing Then
                    Refferer = Me.Request.UrlReferrer.ToString
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
                    'so nun ist es so das ein PM, der eigentlich keine berechtigung hat die Firma1 zu 
                    'administrieren, gerne die standardwerte sehen möchte, manuell dann firma 1 hinzufügen wenn nicht 
                    'vorhanden JJU2008.10.8
                    Dim li As ListItem = ddlCustomer.Items.FindByValue("1")
                    If Not li Is Nothing Then
                        li.Selected = True
                    Else
                        'firma 1 nicht vorhanden
                        Dim tmpItem As New ListItem("Firma1", "1")
                        tmpItem.Selected = True
                        ddlCustomer.Items.Add(tmpItem)

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
                End If
            End If
        Catch ex As Exception
            lblError.Text = ex.ToString
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

        trEditUser.Visible = False 'Tabelle für das Editieren von Feldübersetzungen ausblenden JJ2007.11.12
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

                dtFieldTranslation = New Kernel.FieldTranslationList(lblAppURL.Text, CInt(ddlCustomer.SelectedItem.Value), CInt(ddlLanguage.SelectedItem.Value), cn)
                dvFieldTranslation = dtFieldTranslation.DefaultView
                'm_context.Cache.Insert("myColListView", dvFieldTranslation, Nothing, DateTime.Now.AddMinutes(20), TimeSpan.Zero) 'catch OBjekt myColListView erzeugen, und mit DefaulDataview füllen? JJ2007.11.12
                Session("myColListView") = dvFieldTranslation
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End If
        dvFieldTranslation.Sort = strSort

        With dgSearchResult
            .DataSource = dvFieldTranslation
            .DataBind()
        End With
        If Not m_User.Customer.AccountingArea = -1 Then
            If ddlCustomer.SelectedValue = "1" Then
                'alle Ändern Buttons deaktivieren wenn es kein SuperAdmin und es Firma1, also standardübersetzung ist
                dgSearchResult.Columns(7).Visible = False
            End If
        End If
        For Each tmpItem As GridViewRow In dgSearchResult.Rows
            tmpItem.Cells(6).Visible = CType(tmpItem.FindControl("lblAppID"), Label).Text = "-1"
            tmpItem.Cells(7).Visible = Not CType(tmpItem.FindControl("lblAppID"), Label).Text = "-1"
        Next
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
            Case Else


        End Select
        ddlCustomer.ClearSelection()
        ddlCustomer.Items.FindByValue(FTrans.CustomerID.ToString).Selected = True
        ddlLanguage.ClearSelection()
        ddlLanguage.Items.FindByValue(FTrans.LanguageID.ToString).Selected = True
        cbxVisible.Checked = FTrans.Visibility
        txtContent.Text = FTrans.Content
        txt_Tooltip.Text = FTrans.ToolTip
    End Sub


    Private Function FillEdit(ByVal strAppURL As String, ByVal strFieldType As String, ByVal strFieldName As String, ByVal intCustomerID As Integer, ByVal intLanguageID As Integer) As Boolean
        SearchMode(False)
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()
        Dim _FieldTrans As Kernel.FieldTranslation
        lblError.Text = ""

        Try
            _FieldTrans = New Kernel.FieldTranslation(strAppURL, strFieldType, strFieldName, intCustomerID, intLanguageID, cn)
            SetInput(_FieldTrans)
        Catch ex As Exception
            Dim intTempCustomerID As Integer = CInt(ddlCustomer.SelectedItem.Value)
            Dim intTempLanguageID As Integer = CInt(ddlLanguage.SelectedItem.Value)
            FillEdit(CInt(lblFieldID.Text))
            lblError.Text = "Übersetzung existiert noch nicht."
            txtContent.Text = ""
            lblFieldID.Text = "-1"
            ddlCustomer.ClearSelection()
            ddlCustomer.Items.FindByValue(intTempCustomerID.ToString).Selected = True
            ddlLanguage.ClearSelection()
            ddlLanguage.Items.FindByValue(intTempLanguageID.ToString).Selected = True
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
        Return True
    End Function

    Private Function FillEdit(ByVal intFieldId As Integer, Optional ByVal blnAvoidExceptionLoop As Boolean = False) As Boolean
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
            'ggf. Stack-Overflow durch ExceptionLoop verhindern
            If Not blnAvoidExceptionLoop Then
                FillEdit(CInt(lblFieldIDSave.Text), True)
            End If
            lblError.Text = "Übersetzung existiert noch nicht."
            txtContent.Text = ""
            lblFieldID.Text = "-1"
            ddlCustomer.ClearSelection()
            ddlCustomer.Items.FindByValue(intTempCustomerID.ToString).Selected = True
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
        'Buttons
        lbtnSave.Visible = True
        lbtnDelete.Visible = False

        If blnClearDdlSelection Then
            ddlCustomer.ClearSelection()
            ddlCustomer.Items.FindByValue("1").Selected = True
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
        txtContent.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtFieldName.Enabled = Not blnLock
        txtFieldName.BackColor = System.Drawing.Color.FromName(strBackColor)
        rbLabel.Enabled = Not blnLock
        rbLabel.BackColor = System.Drawing.Color.FromName(strBackColor)
        rbLinkButton.Enabled = Not blnLock
        rbLinkButton.BackColor = System.Drawing.Color.FromName(strBackColor)
        rbRadioButton.Enabled = Not blnLock
        rbRadioButton.BackColor = System.Drawing.Color.FromName(strBackColor)
        rbTableRow.Enabled = Not blnLock
        rbTableRow.BackColor = System.Drawing.Color.FromName(strBackColor)
        cbxVisible.Enabled = Not blnLock
        cbxVisible.BackColor = System.Drawing.Color.FromName(strBackColor)
        ddlCustomer.Enabled = Not blnLock
        ddlCustomer.BackColor = System.Drawing.Color.FromName(strBackColor)
        ddlLanguage.Enabled = Not blnLock
        ddlLanguage.BackColor = System.Drawing.Color.FromName(strBackColor)
        rbTextBox.Enabled = Not blnLock
        rbTextBox.BackColor = System.Drawing.Color.FromName(strBackColor)
    End Sub

    Private Sub EditCreateMode(ByVal strFieldType As String, ByVal strFieldName As String)
        Dim strCustomerID As String = ddlCustomer.SelectedItem.Value
        Dim strLanguageID As String = ddlLanguage.SelectedItem.Value
        If Not FillEdit(lblAppURL.Text, strFieldType, strFieldName, 1, 1) Then
            LockEdit(True)
            lbtnSave.Enabled = False
        Else
            lblFieldID.Text = -1
            ddlCustomer.ClearSelection()
            ddlCustomer.Items.FindByValue(strCustomerID).Selected = True
            ddlLanguage.ClearSelection()
            ddlLanguage.Items.FindByValue(strLanguageID).Selected = True
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
            lblMessage.Text = "Möchten Sie die Spaltenübersetzung wirklich löschen?"
            lbtnDelete.Enabled = True
        End If
        LockEdit(True)
        lbtnCancel.Text = "Abbrechen"
        lbtnSave.Visible = False
        lbtnDelete.Visible = True
    End Sub

    Private Sub SearchMode(Optional ByVal blnSearchMode As Boolean = True)
        'setzt diverse Eingabemasken, dropdownlisten, Gridausgaben je nach übergabeparameter und anderen Kriterien auf Enable bzw Visible, in welchem Zusammenhang ist unklar JJ2007.11.12
        trEditUser.Visible = Not blnSearchMode 'trEditUser Ist die Eingabemaske für Feldübersetzungen JJ2007.11.12
        ddlCustomer.Enabled = Not trEditUser.Visible 'ddlCustomer.Enabled wenn nicht Eingabemaske Visible=false  JJ2007.11.12
        ddlLanguage.Enabled = Not trEditUser.Visible ' ddlLanguage.Enabled wenn nicht Eingabemaske Visible=False JJ2007.11.12
        Result.Visible = blnSearchMode 'trSearchResult= Tabelle mit Resultaten der aktuellen Feldübersetzung (Grid)JJ2007.11.12
        lbtnSave.Visible = Not blnSearchMode 'Setzten der Sichbarkeit von den Linkbuttons JJ2007.11.12
        lbtnCancel.Visible = Not blnSearchMode 'Setzten der Sichbarkeit von den Linkbuttons JJ2007.11.12
        lbtnNew.Visible = blnSearchMode 'Setzten der Sichbarkeit von den Linkbuttons JJ2007.11.12
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
    End Sub

    Private Sub Log(ByVal strIdentification As String, ByVal strDescription As String, ByVal tblParameters As DataTable, Optional ByVal strCategory As String = "APP")
        Dim logApp As New CKG.Base.Kernel.Logging.Trace(m_User.App.Connectionstring, m_User.App.SaveLogAccessSAP, m_User.App.LogLevel)

        ' strCategory
        Dim strUserName As String = m_User.UserName ' strUserName
        Dim strSessionID As String = Session.SessionID ' strSessionID
        Dim intSource As Integer = 0 ' intSource 
        'Dim intSource As Integer = CInt(Request.QueryString("AppID")) ' intSource 
        Dim strTask As String = "Admin - Spaltenübersetzungen" ' strTask
        ' strIdentification
        ' strDescription
        Dim strCustomerName As String = m_User.CustomerName ' strCustomername
        Dim blnIsTestUser As Boolean = m_User.IsTestUser ' blnIsTestUser
        Dim intSeverity As Integer = 0 ' intSeverity 

        logApp.WriteEntry(strCategory, strUserName, strSessionID, intSource, strTask, strIdentification, strDescription, strCustomerName, m_User.Customer.CustomerId, blnIsTestUser, intSeverity, tblParameters)
    End Sub

    Private Function SetOldLogParameters(ByVal intAppId As Int32, ByVal tblPar As DataTable) As DataTable
        'Fehler in der logik der Funktion, JJ  2007.11.09
        Dim cn As SqlClient.SqlConnection
        cn = New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try

            cn.Open()
            Dim _FieldTrans As New Kernel.FieldTranslation(intAppId, cn)

            If tblPar Is Nothing Then
                tblPar = CreateLogTableStructure()
            End If
            With tblPar
                .Rows.Add(.NewRow)
                .Rows(.Rows.Count - 1)("Status") = "Alt"
                .Rows(.Rows.Count - 1)("ApplicationFieldID") = _FieldTrans.ApplicationFieldID
                .Rows(.Rows.Count - 1)("AppURL") = _FieldTrans.AppURL
                .Rows(.Rows.Count - 1)("FieldType") = _FieldTrans.FieldType
                .Rows(.Rows.Count - 1)("FieldName") = _FieldTrans.FieldName
                .Rows(.Rows.Count - 1)("CustomerID") = _FieldTrans.CustomerID
                .Rows(.Rows.Count - 1)("LanguageID") = _FieldTrans.LanguageID
                .Rows(.Rows.Count - 1)("Visibility") = _FieldTrans.Visibility
                .Rows(.Rows.Count - 1)("Content") = _FieldTrans.Content
                .Rows(.Rows.Count - 1)("ToolTip") = _FieldTrans.ToolTip
            End With
            Return tblPar
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "FieldTranslation", "SetOldLogParameters", ex.ToString)

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
        'Fehler in der logik der Function, JJ  2007.11.09
        Try
            If tblPar Is Nothing Then
                tblPar = CreateLogTableStructure()
            End If
            With tblPar
                .Rows.Add(.NewRow)
                .Rows(.Rows.Count - 1)("Status") = "Neu"
                .Rows(.Rows.Count - 1)("ApplicationFieldID") = CInt(lblFieldID.Text)
                .Rows(.Rows.Count - 1)("AppURL") = lblAppURL.Text
                If rbLabel.Checked Then
                    .Rows(.Rows.Count - 1)("FieldType") = "lbl"
                ElseIf rbTableRow.Checked Then
                    .Rows(.Rows.Count - 1)("FieldType") = "tr"
                ElseIf rbLinkButton.Checked Then
                    .Rows(.Rows.Count - 1)("FieldType") = "lb"
                ElseIf rbRadioButton.Checked Then
                    .Rows(.Rows.Count - 1)("FieldType") = "rb"
                ElseIf rbGridColumn.Checked Then
                    .Rows(.Rows.Count - 1)("FieldType") = "col"
                ElseIf rbTextBox.Checked Then
                    .Rows(.Rows.Count - 1)("FieldType") = "txt"
                End If
                .Rows(.Rows.Count - 1)("FieldName") = txtFieldName.Text
                .Rows(.Rows.Count - 1)("CustomerID") = CInt(ddlCustomer.SelectedItem.Value)
                .Rows(.Rows.Count - 1)("LanguageID") = CInt(ddlLanguage.SelectedItem.Value)
                .Rows(.Rows.Count - 1)("Visibility") = cbxVisible.Checked
                .Rows(.Rows.Count - 1)("Content") = txtContent.Text
                .Rows(.Rows.Count - 1)("ToolTip") = txtContent.Text
            End With
            Return tblPar
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "FieldTranslation", "SetNewLogParameters", ex.ToString)

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
            .Columns.Add("ApplicationFieldID", System.Type.GetType("System.Integer"))
            .Columns.Add("AppURL", System.Type.GetType("System.String"))
            .Columns.Add("FieldType", System.Type.GetType("System.String"))
            .Columns.Add("FieldName", System.Type.GetType("System.String"))
            .Columns.Add("CustomerID", System.Type.GetType("System.Integer"))
            .Columns.Add("LanguageID", System.Type.GetType("System.Integer"))
            .Columns.Add("Visibility", System.Type.GetType("System.Boolean"))
            .Columns.Add("Content", System.Type.GetType("System.String"))
            .Columns.Add("ToolTip", System.Type.GetType("System.String"))
        End With
        Return tblPar
    End Function
#End Region

#Region " Events "


    Private Sub lbtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnCancel.Click

        Search(False, , True)
    End Sub

    Private Sub lbtnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnNew.Click

        SearchMode(False)
        ClearEdit(True)
        'Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        'cn.Open()
    End Sub

    Private Sub lbtnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnSave.Click
        Dim tblLogParameter As DataTable
        Dim cn As SqlClient.SqlConnection
        cn = New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try

            cn.Open()
            Dim intApplicationFieldID As Integer = CInt(lblFieldID.Text)
            Dim strLogMsg As String = "Spaltenübersetzungen anlegen"
            If Not (intApplicationFieldID = -1) Then
                strLogMsg = "Spaltenübersetzungen ändern"
                tblLogParameter = New DataTable
                tblLogParameter = SetOldLogParameters(intApplicationFieldID, tblLogParameter)
            End If

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
                                                txt_Tooltip.Text)
            _FieldTranslation.Save(cn)
            tblLogParameter = New DataTable
            tblLogParameter = SetNewLogParameters(tblLogParameter)
            Log(_FieldTranslation.ApplicationFieldID.ToString, strLogMsg, tblLogParameter)
            Search(False, True, True, , True)
            lblMessage.Text = "Die Änderungen wurden gespeichert."
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "FieldTranslation", "lbtnSave_Click", ex.ToString)

            lblError.Text = ex.Message
            If Not ex.InnerException Is Nothing Then
                lblError.Text &= ": " & ex.InnerException.Message
            End If
            tblLogParameter = New DataTable
            Log(lblFieldID.Text, lblError.Text, tblLogParameter, "ERR")
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

            Dim _FieldTranslation As New Kernel.FieldTranslation(CInt(lblFieldID.Text), m_User)
            cn.Open()
            tblLogParameter = New DataTable
            tblLogParameter = SetOldLogParameters(CInt(lblFieldID.Text), tblLogParameter)
            _FieldTranslation.Delete(cn)
            Log(_FieldTranslation.ApplicationFieldID.ToString, "Spaltenübersetzungen löschen", tblLogParameter)
            Search(False, True, True, True, True)
            lblMessage.Text = "Die Spaltenübersetzung wurde gelöscht."
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "FieldTranslation", "lbtnDelete_Click", ex.ToString)

            lblError.Text = ex.Message
            If Not ex.InnerException Is Nothing Then
                lblError.Text &= ": " & ex.InnerException.Message
            End If
            tblLogParameter = New DataTable
            Log(lblFieldID.Text, lblError.Text, tblLogParameter, "ERR")
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Sub

    Private Sub lnkBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkBack.Click
        Response.Redirect(Refferer)
    End Sub

    Private Sub ddlCustomer_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCustomer.SelectedIndexChanged
        FillForm()
    End Sub

    Private Sub ddlLanguage_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlLanguage.SelectedIndexChanged
        FillForm()
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        If trEditUser.Visible Then
            lnkBack.Visible = False
            'Wenn Es können nur Übersetzungen in StandardFirma geändert werden, ist Kunde nicht StandardFirma ist keine Änderung der FeldTypen möglich. JJ 2007.11.12
            If Not ddlCustomer.SelectedItem Is Nothing AndAlso ddlCustomer.SelectedItem.Value = "1" AndAlso Not ddlLanguage.SelectedItem Is Nothing AndAlso ddlLanguage.SelectedItem.Value = "1" Then
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


    Private Sub rbTextBox_checkedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbTextBox.CheckedChanged, rbLinkButton.CheckedChanged, rbGridColumn.CheckedChanged, rbRadioButton.CheckedChanged, rbTableRow.CheckedChanged

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

    Private Sub dgSearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgSearchResult.RowCommand
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

    Private Sub dgSearchResult_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles dgSearchResult.RowEditing

    End Sub

    Private Sub dgSearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgSearchResult.Sorting
        Dim strSort As String = e.SortExpression
        If Not ViewState("ResultSort") Is Nothing AndAlso ViewState("ResultSort").ToString = strSort Then
            strSort &= " DESC"
        End If
        ViewState("ResultSort") = strSort
        FillDataGrid(strSort)
    End Sub
End Class