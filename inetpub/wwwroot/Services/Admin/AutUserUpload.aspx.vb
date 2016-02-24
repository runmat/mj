Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common
Imports System.Data.OleDb
Imports System.Text.RegularExpressions
Imports System.Drawing
Imports CKG.Base.Kernel.Admin
Imports WebTools.Services

Partial Public Class AutUserUpload
    Inherits Page

#Region "Declarations"

    Private m_User As User
    Private m_App As App
    Protected WithEvents GridNavigation1 As CKG.Services.GridNavigation
    Private Const MINANZAHLEXCELSPALTEN As Integer = 14
    Private Const PFLICHTFELDLEER As String = "Dieses Pflichtfeld hat keinen Eintrag."
    Private _rechtWarenkorbNurEigene As Boolean
    Private _rechtDarfNichtAbsenden As Boolean

    Protected ReadOnly Property RechtWarenkorbNurEigene As Boolean
        Get
            Return _rechtWarenkorbNurEigene
        End Get
    End Property

    Protected ReadOnly Property RechtDarfNichtAbsenden As Boolean
        Get
            Return _rechtDarfNichtAbsenden
        End Get
    End Property

    Private ReadOnly Property AnzahlExcelspalten As Integer
        Get
            Return (MINANZAHLEXCELSPALTEN + IIf(_rechtWarenkorbNurEigene, 1, 0) + IIf(_rechtDarfNichtAbsenden, 1, 0))
        End Get
    End Property

    Private Enum UploadExcelColumns
        Title = 0
        Firstname = 1
        LastName = 2
        Username = 3
        Reference = 4
        Reference2 = 5
        Reference3 = 6
        Store = 7
        TestUser = 8
        Groupname = 9
        Organization = 10
        EMailAdress = 11
        Telephone = 12
        ValidFrom = 13
    End Enum

#End Region

#Region "Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        lblHead.Text = "Automatisierte Benutzeranlage"
        AdminAuth(Me, m_User, AdminLevel.Organization)
        GridNavigation1.setGridElment(grvAusgabe)

        Try
            m_App = New App(m_User)

            lblError.Text = ""
            lblMessage.Text = ""

            If Session("RechtWarenkorbNurEigene") IsNot Nothing Then
                _rechtWarenkorbNurEigene = CBool(Session("RechtWarenkorbNurEigene"))
            End If
            If Session("RechtDarfNichtAbsenden") IsNot Nothing Then
                _rechtDarfNichtAbsenden = CBool(Session("RechtDarfNichtAbsenden"))
            End If

            If Not IsPostBack Then
                FillCustomer()

                If Session("Checked") = Nothing Then
                    Session.Add("Checked", False)
                End If

            End If
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "AutUserUpload", "Page_Load", ex.ToString)
            lblError.Text = ex.ToString
        End Try
    End Sub

    Protected Sub rblUpload_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rblUpload.SelectedIndexChanged

        lblMessage.Visible = False
        grvAusgabe.Visible = False

        Session("UploadTable") = Nothing

        If rblUpload.SelectedValue = "Upload" Then
            Image1.Visible = True
            lblDateiauswahl.Visible = True
            upFile.Visible = True
            lbtnLaden.Visible = False

            lbtnFreigeben.Text = Replace(lbtnFreigeben.Text, "Freigeben", "Suchen")
            lbtnFreigeben.Visible = False
            lbtnUpload.Visible = True

            If ddlFilterCustomer.SelectedValue <> 0 AndAlso CheckCustomerRemoteLoginAllowed() Then
                trRemoteLoginKey.Visible = True
            Else
                trRemoteLoginKey.Visible = False
            End If
            trBenutzerFreigeben.Visible = True
            trKeineMailsSenden.Visible = True
        Else
            Image1.Visible = False
            lblDateiauswahl.Visible = False
            upFile.Visible = False
            lbtnUpload.Visible = False
            lbtnFreigeben.Visible = True
            trRemoteLoginKey.Visible = False
            trBenutzerFreigeben.Visible = False
            trKeineMailsSenden.Visible = False
        End If
    End Sub

    Protected Sub lbtnUpload_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnUpload.Click

        If ddlFilterCustomer.SelectedValue = 0 Then

            lblError.Text = "Bitte wählen Sie eine Firma aus."
            Exit Sub

        End If

        'Prüfe Fehlerbedingung
        If (Not upFile.PostedFile Is Nothing) AndAlso (Not (upFile.PostedFile.FileName = String.Empty)) Then
            lblExcelfile.Text = upFile.PostedFile.FileName
            If Right(upFile.PostedFile.FileName.ToUpper, 4) <> ".XLS" Then
                lblError.Text = "Es können nur Dateien im .XLS - Format verarbeitet werden."
                Exit Sub
            End If
        Else
            lblError.Text = "Keine Datei ausgewählt"
            Exit Sub
        End If

        'Lade Datei
        Upload(upFile.PostedFile)

        If lblError.Text = String.Empty Then
            ddlFilterCustomer.Enabled = False
            Result.Visible = True
            Image1.Visible = False
            lblDateiauswahl.Visible = False
            upFile.Visible = False
        End If
    End Sub

    Protected Sub lnkCreateExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel.Click
        Try
            Dim TempTable As DataTable = CType(Session("UploadTable"), DataTable).Clone
            Dim AusgabeTable As DataTable = CType(Session("UploadTable"), DataTable).Copy

            Dim Found As Boolean

            For Each col2 As DataColumn In TempTable.Columns

                Found = False

                For Each FieldControl As DataControlField In grvAusgabe.Columns

                    If col2.ColumnName.ToUpper = FieldControl.SortExpression.ToUpper Then
                        Found = True

                        AusgabeTable.Columns(col2.ColumnName).ColumnName = FieldControl.HeaderText
                        If FieldControl.Visible = False Then Found = False

                        Exit For

                    End If

                Next

                If Not Found Then
                    AusgabeTable.Columns.Remove(AusgabeTable.Columns(col2.ColumnName))

                End If

                AusgabeTable.AcceptChanges()
            Next

            If AusgabeTable.Columns.Count > 0 Then
                Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
                Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
                excelFactory.CreateDocumentAndSendAsResponse(strFileName, AusgabeTable, Page)
            Else
                Err.Raise(-1, , "Fehler beim Erstellen der Excel-Datei.")
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub grvAusgabe_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs) Handles grvAusgabe.Sorting
        CheckGrid()
        FillGrid(grvAusgabe.PageIndex, e.SortExpression)
    End Sub

    Private Sub GridNavigation1_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation1.PagerChanged
        CheckGrid()
        FillGrid(PageIndex, String.Empty)
    End Sub

    Private Sub GridNavigation1_PageSizeChanged() Handles GridNavigation1.PageSizeChanged
        CheckGrid()
        FillGrid(0, String.Empty)
    End Sub

    Protected Sub lbtnLaden_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnLaden.Click
        grvAusgabe.Enabled = True
        lbtnLaden.Visible = False
        lbtnSave.Visible = False
        lbtnPruefen.Visible = True
    End Sub

    Protected Sub lbtnBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnBack.Click
        Session("Checked") = Nothing
        Response.Redirect("../Start/Selection.aspx")
    End Sub

    Protected Sub lbtnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnSave.Click

        Dim TempTable As DataTable = CType(Session("UploadTable"), DataTable)

        Dim blnTestUser As Boolean
        Dim _User As User

        lblError.Text = ""

        For Each TempRow As DataRow In TempTable.Rows
            'Wert "ja" unabhängig von Groß-/Kleinschreibung erfassen
            blnTestUser = False
            If TempRow(UploadExcelColumns.TestUser) IsNot Nothing Then
                If TempRow(UploadExcelColumns.TestUser).ToString().ToUpper() = "JA" Then
                    blnTestUser = True
                End If
            End If

            _User = New User(-1, _
                              TempRow(UploadExcelColumns.Username), _
                              TempRow(UploadExcelColumns.Reference), _
                              TempRow(UploadExcelColumns.Reference2), _
                              TempRow(UploadExcelColumns.Reference3), _
                              (RechtWarenkorbNurEigene AndAlso TempRow(14).ToString.ToUpper() = "JA"), _
                              blnTestUser, _
                              CInt(ddlFilterCustomer.SelectedItem.Value), _
                              False, _
                              False, _
                              Not chkBenutzerFreigeben.Checked, _
                              False, _
                              False, False, _
                              m_User.App.Connectionstring, _
                              0, _
                              m_User.UserName, _
                              chkBenutzerFreigeben.Checked, _
                              TempRow(UploadExcelColumns.Firstname), _
                              TempRow(UploadExcelColumns.LastName), _
                              TempRow(UploadExcelColumns.Title), _
                              TempRow(UploadExcelColumns.Store), _
                              False, _
                              IIf(TempRow(UploadExcelColumns.ValidFrom) Is DBNull.Value, String.Empty, TempRow(UploadExcelColumns.ValidFrom)), _
                              "")

            _User.Email = TempRow(UploadExcelColumns.EMailAdress).ToString()
            _User.Telephone = TempRow(UploadExcelColumns.Telephone).ToString()
            If chkRemoteLoginKey.Checked Then
                _User.UrlRemoteLoginKey = HttpUtility.UrlEncode(Guid.NewGuid().ToString)
            End If

            Dim intGroupID As Integer = GetGroupID(TempRow(UploadExcelColumns.Groupname))

            If intGroupID > 0 Then
                'Gruppe ausgewählt
                If Not _User.Groups.IsInGroups(intGroupID) Then
                    'gewaehlte Gruppe ist neu
                    'vorhandene Gruppen loeschen
                    '(da nur eine Gruppe je User erlaubt)
                    If Not _User.Groups.Count = 0 Then
                        Dim gr As Security.Group
                        For Each gr In _User.Groups
                            gr.MarkDeleted()
                        Next
                    End If
                    'neue Gruppe hinzufuegen
                    _User.Groups.Add(New Security.Group(intGroupID, CInt(ddlFilterCustomer.SelectedItem.Value)))
                End If
            Else
                lblError.Text = "Bitte geben Sie für den Mitarbeiter eine Gruppe an!"
                Exit Sub
            End If

            Dim intOrganizationID As Integer = GetOrganizationID(TempRow(UploadExcelColumns.Organization))

            If _User.Save() Then
                _User.SetLastLogin(Now)
                _User.Organization.ReAssignUserToOrganization(m_User.UserName, -1, _User.UserID, intOrganizationID, False, m_User.App.Connectionstring)

                'Linkschlüssel generieren
                Dim confirmationToken As String = UserSecurityService.GenerateToken(_User.UserName)
                _User.UpdateWebUserPasswordChangeRequestKey(confirmationToken)

                'Erstellt einen Eintrag in der Tabelle für den Freigabe-Workflow
                InsertIntoWebUserUpload(_User.UserID, HttpUtility.UrlEncode(confirmationToken), chkKeineMailsSenden.Checked, _User.ValidFrom)

                'kundenindividuelle Einstellungen speichern
                If RechtDarfNichtAbsenden Then
                    RightList.SaveRightPerUser(_User.UserName, "AUFTRAEGE_NICHT_ABSENDEN", TempRow(IIf(RechtWarenkorbNurEigene, 15, 14)).ToString().ToUpper() = "JA", m_User.UserName)
                End If
            Else
                lblError.Text = _User.ErrorMessage
            End If

            _User = Nothing

        Next

        If lblError.Text = "" Then
            lblError.Text = "Die Benutzer wurden erfolgreich angelegt"
        End If

        lbtnLaden.Visible = False
        lbtnSave.Visible = False
        Result.Visible = False
    End Sub

    Protected Sub ddlFilterCustomer_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlFilterCustomer.SelectedIndexChanged
        If ddlFilterCustomer.SelectedValue = 0 Then

            lblError.Text = "Bitte wählen Sie eine Firma für den Upload aus."
            Exit Sub
        Else

            If rblUpload.SelectedValue = "Upload" Then
                Image1.Visible = True
                lblDateiauswahl.Visible = True
                upFile.Visible = True

                trBenutzerFreigeben.Visible = True
                If ddlFilterCustomer.SelectedValue <> 0 AndAlso CheckCustomerRemoteLoginAllowed() Then
                    trRemoteLoginKey.Visible = True
                Else
                    trRemoteLoginKey.Visible = False
                End If
                trKeineMailsSenden.Visible = True

                Dim intCustomerID As Integer = CInt(ddlFilterCustomer.SelectedValue)
                Dim _customer As New Customer(intCustomerID, m_User.App.Connectionstring)

                ' Referenzfelder
                If Not String.IsNullOrEmpty(_customer.ReferenceType1) Then
                    lblRef1.Text = _customer.ReferenceType1Name
                Else
                    lblRef1.Text = ""
                End If
                If Not String.IsNullOrEmpty(_customer.ReferenceType2) Then
                    lblRef2.Text = _customer.ReferenceType2Name
                Else
                    lblRef2.Text = ""
                End If
                If Not String.IsNullOrEmpty(_customer.ReferenceType3) Then
                    lblRef3.Text = _customer.ReferenceType3Name
                Else
                    lblRef3.Text = ""
                End If

                'kundenspezifische Einstellungen
                _rechtWarenkorbNurEigene = (_customer.ReferenceType4 = "AH_WK_NUR_EIGENE")
                Dim rightsOfCustomer = RightList.ShowRightsPerCustomer(_customer.CustomerId)
                _rechtDarfNichtAbsenden = rightsOfCustomer.Contains("AUFTRAEGE_NICHT_ABSENDEN")
                ApplyCustomerSpecificSettings()
            Else
                trBenutzerFreigeben.Visible = False
                trRemoteLoginKey.Visible = False
                trKeineMailsSenden.Visible = False
            End If

        End If

    End Sub

    Protected Sub lbtnPruefen_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnPruefen.Click
        'Erste Prüfung
        If CType(Session("Checked"), Boolean) = False Then
            SetInitialErrorInDataTable()
            Session("Checked") = True
        End If

        CheckGrid()

        Dim TempTable As DataTable = CType(Session("UploadTable"), DataTable)

        Dim ErrCount As Integer = TempTable.Select("Error = " & True).GetLength(0)

        If ErrCount > 0 Then
            lblError.Text = "Es wurden " & ErrCount & " Zeilen mit Fehlern gefunden. Bitte korrigieren Sie diese."
            Result.Visible = True
        Else
            grvAusgabe.Enabled = False
            lbtnPruefen.Visible = False
            lbtnLaden.Visible = True
            lbtnSave.Visible = True
        End If

    End Sub

    Protected Sub lbtnFreigeben_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnFreigeben.Click

        If ddlFilterCustomer.SelectedValue = 0 Then

            lblError.Text = "Bitte wählen Sie eine Firma aus."
            Exit Sub

        End If

        If lbtnFreigeben.Text.Contains("Freigeben") = True Then
            FreigabeWebUser()
            lblMessage.Text = "Die Benutzer wurden freigegeben."
            grvAusgabe.Visible = False
            lblMessage.Visible = False
            lbtnFreigeben.Text = Replace(lbtnFreigeben.Text, "Freigeben", "Suchen")
        Else
            LoadFreigaben()
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

#End Region

#Region "Methods"

    '----------------------------------------------------------------------
    ' Methode:      FillGrid
    ' Autor:        SFa
    ' Beschreibung: Füllt das gridview grvAusgabe aus der Bestandstabelle.
    '               Je nach Übergabeparameter findet eine Sortierung oder
    '               Seitenanzeige statt
    ' Erstellt am:  02.04.2009
    ' ITA:          2709
    '----------------------------------------------------------------------
    Private Sub FillGrid(ByVal PageIndex As Int32, Optional ByVal Sort As String = "")

        Dim Direction As String = String.Empty

        If IsNothing(Session("UploadTable")) = False Then

            Dim UploadTable As DataTable = CType(Session("UploadTable"), DataTable)

            If UploadTable.Rows.Count > 0 Then

                grvAusgabe.Visible = True

                If Sort.Trim(" "c).Length > 0 Then
                    PageIndex = 0
                    Sort = Sort.Trim(" "c)
                    If (ViewState("Sort") Is Nothing) OrElse (ViewState("Sort").ToString = Sort) Then
                        If ViewState("Direction") Is Nothing Then
                            Direction = "desc"
                        Else
                            Direction = ViewState("Direction").ToString
                        End If
                    Else
                        Direction = "desc"
                    End If

                    If Direction = "asc" Then
                        Direction = "desc"
                    Else
                        Direction = "asc"
                    End If

                    ViewState("Sort") = Sort
                    ViewState("Direction") = Direction
                Else
                    If Not ViewState("Sort") Is Nothing Then
                        Sort = ViewState("Sort").ToString
                        If ViewState("Direction") Is Nothing Then
                            Direction = "asc"
                            ViewState("Direction") = Direction
                        Else
                            Direction = ViewState("Direction").ToString
                        End If
                    End If
                End If

                If Not Sort.Length = 0 Then
                    UploadTable.DefaultView.Sort = Sort & " " & Direction
                End If

                grvAusgabe.PageIndex = PageIndex

                grvAusgabe.DataSource = UploadTable.DefaultView
                grvAusgabe.DataBind()

                grvAusgabe.Visible = True

                lblMessage.Text = "Es wurden " & UploadTable.Rows.Count.ToString & " Datensätze gefunden."
                lblMessage.Visible = True

                If Not Session("Checked") = Nothing Then
                    CheckGrid()
                End If

            Else
                lblMessage.Visible = True
                lblMessage.Text = "Es wurden keine Daten gefunden."

            End If
        Else
            lblMessage.Text = "Es wurden keine Daten gefunden."
            lblMessage.Visible = True
        End If

    End Sub

    Private Sub FillCustomer()

        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()

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

            .Items.RemoveAt(1)

            .Items.FindByValue("0").Text = "- Auswahl -"

            .Items.FindByValue("0").Selected = True

        End With

        cn.Close()
        cn.Dispose()

    End Sub

    Private Sub Upload(ByVal uFile As HttpPostedFile)
        Try

            Dim filepath As String = ConfigurationManager.AppSettings("UploadpathLocal")
            Dim filename As String
            Dim info As IO.FileInfo
            Dim TempTable As New DataTable
            filename = m_User.UserName & "_" & Format(Now, "yyyyMMddhhmmss") & ".xls"

            lblError.Text = String.Empty

            If Not (uFile Is Nothing) Then
                uFile.SaveAs(ConfigurationManager.AppSettings("UploadpathLocal") & filename)
                uFile = Nothing
                info = New IO.FileInfo(filepath & filename)
                If Not (info.Exists) Then
                    lblError.Text = "Fehler beim Speichern."
                    Exit Sub
                End If
                'Datei gespeichert -> Auswertung
                TempTable = getDataTableFromExcel(filepath, filename)

                If TempTable.Columns.Count = AnzahlExcelspalten Then
                    If TempTable.Rows.Count < 1 Then
                        Err.Raise(-1, , "Die Exceldatei enthält keine Daten.")
                    End If
                Else
                    Err.Raise(-1, , "Die Exceldatei hat nicht die korrekte Anzahl an Spalten.")
                End If

            End If

            If CheckCustomerMaxUser(TempTable.Rows.Count) = True Then Exit Sub

            CheckTableData(TempTable)

        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Private Function getDataTableFromExcel(ByVal filepath As String, ByVal filename As String) As DataTable
        '----------------------------------------------------------------------
        ' Methode: GetDataTable
        ' Autor: JJU 
        ' Beschreibung: extrahiert die Daten aus dem ersten Exceltabellen-Blatt in eine Datatable
        ' Erstellt am: 2008.09.22
        ' ITA: 1844
        '----------------------------------------------------------------------

        Dim wBook As New Aspose.Cells.Workbook

        wBook.Open(filepath & filename, Aspose.Cells.FileFormatType.Excel2000)

        Dim wSheet As Aspose.Cells.Worksheet = wBook.Worksheets(0)

        Dim colCount As Integer = wSheet.Cells.Columns.Count
        'x wird hinzugefügt, da über OLEBD nur 8 Zeilen ausgelesen werden,
        'um den Datentyp zu bestimmen. Das führt zu Problemen.
        For i As Integer = 0 To (colCount - 1)
            wSheet.Cells(1, i).PutValue("x" & wSheet.Cells(1, i).Value)
        Next

        wBook.Save(filepath & filename, Aspose.Cells.FileFormatType.Excel2000)

        wBook = Nothing

        Dim strConn As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data " & _
        "Source=" & filepath & filename & ";Extended Properties=""Excel 8.0;HDR=Yes;IMEX=1"""
        Dim conn As New OleDbConnection(strConn)

        Dim da As New OleDbDataAdapter("Select * From [Tabelle1$]", conn)

        Dim dt As New DataTable

        da.Fill(dt)

        'x wieder entfernen 
        Dim Row As DataRow = dt.Rows(0)

        For i As Integer = 0 To (colCount - 1)
            Row(i) = Mid(Row(i), 2)
        Next

        dt.AcceptChanges()

        Return dt

    End Function

    Private Sub CheckTableData(ByVal TempTable As DataTable)

        'ggf. Dummy-Spalten ergänzen, damit das DataBinding funktioniert
        If TempTable.Columns.Count = 14 Then
            TempTable.Columns.Add("WarenkorbNurEigene")
        End If
        If TempTable.Columns.Count = 15 Then
            TempTable.Columns.Add("DarfNichtAbsenden")
        End If

        TempTable.Columns.Add("ID", Type.GetType("System.Int32"))
        TempTable.AcceptChanges()

        Dim e As Integer = 0
        Dim blnShowPrefixField As Boolean = False

        For Each TempRow As DataRow In TempTable.Rows

            TempRow("ID") = e

            If TempRow(UploadExcelColumns.Username) Is DBNull.Value Then
                'lblError.Text = "Abbruch: Benutzername fehlt. Überprüfen Sie die Exceldatei."
                'Exit Sub
                blnShowPrefixField = True
            End If

            ' Leerzeichen entfernen
            If TempRow(UploadExcelColumns.Username) IsNot DBNull.Value Then
                TempRow(UploadExcelColumns.Username) = TempRow(UploadExcelColumns.Username).ToString().Replace(" ", "")
            End If
            If TempRow(UploadExcelColumns.Title) IsNot DBNull.Value Then
                TempRow(UploadExcelColumns.Title) = TempRow(UploadExcelColumns.Title).ToString().Replace(" ", "")
            End If
            If TempRow(UploadExcelColumns.Firstname) IsNot DBNull.Value Then
                TempRow(UploadExcelColumns.Firstname) = TempRow(UploadExcelColumns.Firstname).ToString().Replace(" ", "")
            End If
            If TempRow(UploadExcelColumns.LastName) IsNot DBNull.Value Then
                TempRow(UploadExcelColumns.LastName) = TempRow(UploadExcelColumns.LastName).ToString().Replace(" ", "")
            End If
            If TempRow(UploadExcelColumns.EMailAdress) IsNot DBNull.Value Then
                TempRow(UploadExcelColumns.EMailAdress) = TempRow(UploadExcelColumns.EMailAdress).ToString().Replace(" ", "")
            End If

            e += 1
        Next

        trUsernamePrefix.Visible = blnShowPrefixField

        Dim EnumArray() As String = [Enum].GetNames(GetType(UploadExcelColumns))

        For i As Integer = 0 To 13
            TempTable.Columns(i).ColumnName = EnumArray(i)
        Next

        If RechtWarenkorbNurEigene Then
            TempTable.Columns(14).ColumnName = "WarenkorbNurEigene"
            If RechtDarfNichtAbsenden Then
                TempTable.Columns(15).ColumnName = "DarfNichtAbsenden"
            End If
        ElseIf RechtDarfNichtAbsenden Then
            TempTable.Columns(14).ColumnName = "DarfNichtAbsenden"
        End If

        Dim Column As New DataColumn("Error", Type.GetType("System.Boolean"))
        Column.DefaultValue = False

        TempTable.Columns.Add(Column)

        TempTable.AcceptChanges()

        Session.Add("UploadTable", TempTable)

        lbtnUpload.Visible = False
        lbtnPruefen.Visible = True
        lblMessage.Text = "Bitte überprüfen Sie, ob die Daten in den richtigen Spalten stehen."
        FillGrid(0)

    End Sub

    Private Sub LoadFreigaben()

        Dim UserUploadTable As New DataTable
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()

        Dim daUser As SqlClient.SqlDataAdapter
        daUser = New SqlClient.SqlDataAdapter("SELECT * " & _
                                                            "FROM vwWebUserUpload " & _
                                                            "WHERE Approved = 0 and " & _
                                                            "CustomerID=@CustomerID and " & _
                                                            "CreatedBy<>@User", cn)

        daUser.SelectCommand.Parameters.AddWithValue("@CustomerID", ddlFilterCustomer.SelectedValue)
        daUser.SelectCommand.Parameters.AddWithValue("@User", m_User.UserName)
        daUser.Fill(UserUploadTable)

        cn.Close()
        cn.Dispose()

        Session("UploadTable") = Nothing

        If UserUploadTable.Rows.Count > 0 Then

            Session.Add("UploadTable", UserUploadTable)

            lbtnFreigeben.Text = Replace(lbtnFreigeben.Text, "Suchen", "Freigeben")

            grvAusgabe.Enabled = False

        End If

        FillGrid(0)

    End Sub

    Private Sub InsertIntoWebUserUpload(ByVal UserID As Integer, ByVal LinkKey As String, ByVal MailversandUnterdruecken As Boolean, ByVal gueltigAb As String)
        Dim cmdInsert As SqlClient.SqlCommand

        Using cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            cn.Open()

            If MailversandUnterdruecken Then
                cmdInsert = New SqlClient.SqlCommand("INSERT INTO WebUserUpload(UserID,RightUserLink,Gueltigkeitsdatum,MailSend) Values(@UserID,@RightUserLink,@Gueltigkeitsdatum,@MailSend)", cn)
            Else
                cmdInsert = New SqlClient.SqlCommand("INSERT INTO WebUserUpload(UserID,RightUserLink,Gueltigkeitsdatum) Values(@UserID,@RightUserLink,@Gueltigkeitsdatum)", cn)
            End If

            With cmdInsert.Parameters
                .AddWithValue("@UserID", UserID)
                .AddWithValue("@RightUserLink", LinkKey)
                .AddWithValue("@Gueltigkeitsdatum", gueltigAb)
                If MailversandUnterdruecken Then
                    .AddWithValue("@MailSend", 1)
                End If
            End With
            cmdInsert.ExecuteNonQuery()

            cn.Close()
        End Using

    End Sub

    Private Sub FreigabeWebUser()
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()

        Dim UpdateTable As DataTable = CType(Session("UploadTable"), DataTable)
        Dim UpdateRow As DataRow

        Dim cmdUpdate As New SqlClient.SqlCommand("Update WebUser set Approved = 1 where UserID=@UserID", cn)

        Dim SqlParam As New SqlClient.SqlParameter("@UserID", SqlDbType.Int)

        cmdUpdate.Parameters.Add(SqlParam)

        For Each UpdateRow In UpdateTable.Rows

            cmdUpdate.Parameters("@UserID").Value = CInt(UpdateRow("ID"))
            cmdUpdate.ExecuteNonQuery()
        Next

        cn.Close()
        cn.Dispose()

    End Sub

    Private Sub CheckGrid()

        Dim GridRow As GridViewRow
        Dim FoundError As Boolean
        Dim txt As TextBox
        Dim ToolMessage As String
        Dim chk As CheckBox
        Dim TempTable As DataTable = CType(Session("UploadTable"), DataTable)
        Dim RowSet As DataRow()
        Dim GridControl As Control
        Dim letzteUsernummer As Integer = 0

        'Für das gewählte Username-Präfix die nächste lfd. Nummer ermitteln und damit anschließend automatische Usernamen vergeben 
        If Not String.IsNullOrEmpty(txtUsernamePrefix.Text) Then
            letzteUsernummer = GetMaxUsernameNumber(txtUsernamePrefix.Text.Trim())
        End If

        For Each GridRow In grvAusgabe.Rows

            If CType(Session("Checked"), Boolean) = True Then

                FoundError = False

                'Pflichtfelder überprüfen

                'Title
                txt = GridRow.FindControl("txtAnrede")

                If Trim(txt.Text).Length = 0 Then
                    txt.BorderColor = Color.Red
                    txt.ToolTip = PFLICHTFELDLEER
                    FoundError = True
                Else
                    txt.BorderColor = Color.Empty
                    txt.ToolTip = String.Empty

                    If txt.Text <> "Herr" AndAlso txt.Text <> "Frau" Then
                        FoundError = True
                        txt.BorderColor = Color.Red
                        txt.ToolTip = "Bitte tragen Sie Herr oder Frau ein."
                    End If
                End If

                'Vorname
                txt = GridRow.FindControl("txtVorname")
                If Trim(txt.Text).Length = 0 Then
                    txt.BorderColor = Color.Red
                    txt.ToolTip = PFLICHTFELDLEER
                    FoundError = True
                Else
                    txt.BorderColor = Color.Empty
                    txt.ToolTip = String.Empty
                End If

                'Nachname
                txt = GridRow.FindControl("txtNachname")
                If Trim(txt.Text).Length = 0 Then
                    txt.BorderColor = Color.Red
                    txt.ToolTip = PFLICHTFELDLEER
                    FoundError = True
                Else
                    txt.BorderColor = Color.Empty
                    txt.ToolTip = String.Empty
                End If

                'Username
                txt = GridRow.FindControl("txtUsername")
                'Ggf. im 1. Schritt den Usernamen automatisch setzen
                If Trim(txt.Text).Length = 0 AndAlso Not String.IsNullOrEmpty(txtUsernamePrefix.Text) Then
                    letzteUsernummer += 1
                    txt.Text = txtUsernamePrefix.Text.Trim() & "_" & letzteUsernummer.ToString("0000")
                End If
                If Trim(txt.Text).Length = 0 Then
                    txt.BorderColor = Color.Red
                    txt.ToolTip = PFLICHTFELDLEER
                    FoundError = True
                Else
                    txt.BorderColor = Color.Empty
                    txt.ToolTip = String.Empty

                    ToolMessage = ForbiddenUserName(txt.Text)

                    If ToolMessage <> String.Empty Then
                        txt.ToolTip = ToolMessage
                        txt.BorderColor = Color.Red
                        FoundError = True
                    Else

                        ToolMessage = UserExists(txt.Text)

                        If ToolMessage <> String.Empty Then
                            txt.ToolTip = ToolMessage
                            txt.BorderColor = Color.Red
                            FoundError = True
                        End If

                    End If

            End If

                'Testzugang
                txt = GridRow.FindControl("txtTestzugang")
                If Trim(txt.Text).Length > 0 AndAlso txt.Text.ToUpper <> "JA" Then
                    txt.BorderColor = Color.Red
                    txt.ToolTip = "Bitte tragen Sie hier Ja ein oder lassen Sie das Feld leer."
                    FoundError = True
                Else
                    txt.BorderColor = Color.Empty
                    txt.ToolTip = String.Empty
                End If

                'Gruppe
                txt = GridRow.FindControl("txtGruppe")
                If Trim(txt.Text).Length = 0 Then
                    txt.BorderColor = Color.Red
                    txt.ToolTip = PFLICHTFELDLEER
                    FoundError = True
                Else

                    txt.BorderColor = Color.Empty
                    txt.ToolTip = String.Empty

                    ToolMessage = GroupExists(txt.Text)

                    If ToolMessage <> String.Empty Then
                        txt.ToolTip = ToolMessage
                        txt.BorderColor = Color.Red
                        FoundError = True
                    End If
                End If

                'Organisation
                txt = GridRow.FindControl("txtOrganization")
                If Trim(txt.Text).Length = 0 Then
                    txt.BorderColor = Color.Red
                    txt.ToolTip = PFLICHTFELDLEER
                    FoundError = True
                Else
                    txt.BorderColor = Color.Empty
                    txt.ToolTip = String.Empty

                    ToolMessage = OrganizationExists(txt.Text)

                    If ToolMessage <> String.Empty Then
                        txt.ToolTip = ToolMessage
                        txt.BorderColor = Color.Red
                        FoundError = True
                    End If
                End If

                'Mailadresse
                txt = GridRow.FindControl("txtMail")
                If Trim(txt.Text).Length = 0 Then
                    txt.BorderColor = Color.Red
                    txt.ToolTip = PFLICHTFELDLEER
                    FoundError = True
                Else
                    txt.BorderColor = Color.Empty
                    txt.ToolTip = String.Empty

                    If EmailAddressCheck(txt.Text) = False Then
                        txt.BorderColor = Color.Red
                        txt.ToolTip = "Bitte geben Sie eine gültige E-Mailadresse ein."
                        FoundError = True
                    End If
                End If

                'Gültigkeitsdatum
                txt = GridRow.FindControl("txtGueltigkeitsdatum")
                If Trim(txt.Text).Length > 0 Then
                    If IsDate(txt.Text) = False OrElse (txt.Text < Date.Today) Then
                        txt.BorderColor = Color.Red
                        txt.ToolTip = "Bitte geben Sie eine gültiges Datum(tt.mm.jjjj) ein, welches in der Zukunft liegt."
                        FoundError = True
                    Else
                        txt.BorderColor = Color.Empty
                        txt.ToolTip = String.Empty
                    End If
                Else
                    txt.BorderColor = Color.Empty
                    txt.ToolTip = String.Empty
                End If

                If FoundError = True Then
                    chk = GridRow.FindControl("chkError")
                    chk.Checked = True
                End If

            End If

            'Update in der Tabelle
            RowSet = TempTable.Select("ID = " & GridRow.Cells(17).Text)

            RowSet(0).BeginEdit()

            For i As Integer = 0 To 15
                For Each GridControl In GridRow.Cells(i).Controls

                    If TypeOf GridControl Is TextBox Then
                        txt = CType(GridControl, TextBox)
                        If i <> UploadExcelColumns.ValidFrom Then
                            RowSet(0)(i) = txt.Text
                        Else
                            If txt.Text = String.Empty Then
                                RowSet(0)(i) = DBNull.Value
                            Else
                                RowSet(0)(i) = txt.Text
                            End If

                        End If

                    End If

                Next
            Next

            If CType(Session("Checked"), Boolean) = True Then
                RowSet(0)("Error") = FoundError
            End If

            TempTable.AcceptChanges()

            Session("UploadTable") = TempTable

        Next

    End Sub

    Private Sub SetInitialErrorInDataTable()

        Dim TempTable As DataTable = CType(Session("UploadTable"), DataTable)
        Dim Row As DataRow
        Dim FoundError As Boolean
        Dim ErrorMessage As String

        'Pflichtfelder überprüfen

        For Each Row In TempTable.Rows

            FoundError = False

            'Title
            If Row(UploadExcelColumns.Title) Is DBNull.Value Then
                FoundError = True
            ElseIf Trim(Row(UploadExcelColumns.Title)).Length = 0 Then
                FoundError = True
            Else
                If Row(UploadExcelColumns.Title) <> "Herr" AndAlso Row(UploadExcelColumns.Title) <> "Frau" Then
                    FoundError = True
                End If
            End If

            'Vorname
            If Row(UploadExcelColumns.Firstname) Is DBNull.Value Then
                FoundError = True
            ElseIf Trim(Row(UploadExcelColumns.Firstname)).Length = 0 Then
                FoundError = True
            End If

            'Nachname
            If Row(UploadExcelColumns.LastName) Is DBNull.Value Then
                FoundError = True
            ElseIf Trim(Row(UploadExcelColumns.LastName)).Length = 0 Then
                FoundError = True
            End If

            'Username
            If Row(UploadExcelColumns.Username) Is DBNull.Value Then
                FoundError = True
            ElseIf Trim(Row(UploadExcelColumns.Username)).Length = 0 Then
                FoundError = True
            Else

                ErrorMessage = ForbiddenUserName(Row(UploadExcelColumns.Username))

                If ErrorMessage <> String.Empty Then
                    FoundError = True
                Else

                    ErrorMessage = UserExists(Row(UploadExcelColumns.Username))

                    If ErrorMessage <> String.Empty Then
                        FoundError = True
                    End If

                End If

            End If

            'Testzugang
            If Not Row(UploadExcelColumns.TestUser) Is DBNull.Value Then

                If Trim(Row(UploadExcelColumns.TestUser)).Length > 0 AndAlso Row(UploadExcelColumns.TestUser).ToUpper <> "JA" Then
                    FoundError = True
                End If
            End If

            'Gruppe
            If Row(UploadExcelColumns.Groupname) Is DBNull.Value Then
                FoundError = True
            ElseIf Trim(Row(UploadExcelColumns.Groupname)).Length = 0 Then
                FoundError = True
            Else

                ErrorMessage = GroupExists(Row(UploadExcelColumns.Groupname))

                If ErrorMessage <> String.Empty Then
                    FoundError = True
                End If
            End If

            'Organisation
            If Row(UploadExcelColumns.Organization) Is DBNull.Value Then
                FoundError = True
            ElseIf Trim(Row(UploadExcelColumns.Organization)).Length = 0 Then
                FoundError = True
            Else

                ErrorMessage = OrganizationExists(Row(UploadExcelColumns.Organization))

                If ErrorMessage <> String.Empty Then
                    FoundError = True
                End If
            End If

            'Mailadresse
            If Row(UploadExcelColumns.EMailAdress) Is DBNull.Value Then
                FoundError = True
            ElseIf Trim(Row(UploadExcelColumns.EMailAdress)).Length = 0 Then
                FoundError = True
            Else
                If EmailAddressCheck(Row(UploadExcelColumns.EMailAdress)) = False Then
                    FoundError = True
                End If
            End If

            'Gültigkeitsdatum
            If Not Row(UploadExcelColumns.ValidFrom) Is DBNull.Value Then
                If Trim(Row(UploadExcelColumns.ValidFrom)).Length > 0 Then
                    If IsDate(Row(UploadExcelColumns.ValidFrom)) = False Then
                        FoundError = True
                    End If
                End If
            End If

            If FoundError = True Then
                Row("Error") = CInt(True)
            End If

        Next

        TempTable.AcceptChanges()

        Session("UploadTable") = TempTable

    End Sub

    Private Function ForbiddenUserName(ByVal Username As String) As String

        Dim intLoop As Integer
        Dim dvForbiddenUserName As DataView
        Dim dtForbiddenUserNameAll As Kernel.ForbiddenUserNameAllList

        Dim ToolTipText As String = String.Empty

        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()

        dtForbiddenUserNameAll = New Kernel.ForbiddenUserNameAllList(cn)
        dvForbiddenUserName = dtForbiddenUserNameAll.DefaultView
        For intLoop = 0 To dvForbiddenUserName.Count - 1
            If InStr(UCase(Username), UCase(CStr(dvForbiddenUserName(intLoop)("UserName")))) > 0 Then
                ToolTipText = "Bitte wählen Sie einen anderen Namen für den neuen Benutzer!"
                ToolTipText &= " <br>(Der Name oder ein Teil davon ist eine gesperrte Zeichenfolge.)"
                Exit For
            End If
        Next

        cn.Close()
        cn.Dispose()

        Return ToolTipText

    End Function

    Private Function UserExists(ByVal Username As String) As String

        Dim ToolTipText As String = String.Empty

        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()

        Dim cmdCheckUserExits As New SqlClient.SqlCommand("SELECT COUNT(UserID) FROM WebUser WHERE Username=@Username", cn)
        cmdCheckUserExits.Parameters.AddWithValue("@Username", Username)
        If cmdCheckUserExits.ExecuteScalar.ToString <> "0" Then
            ToolTipText = "Es existiert bereits ein Benutzer mit dieser Kennung (Login-Name) im System! Bitte wählen sie eine andere Kennung!"

        End If

        cn.Close()
        cn.Dispose()

        Return ToolTipText

    End Function

    ''' <summary>
    ''' Ermittelt die höchste vergebene fortlaufende Usernummer für das gewählte Präfix aus der DB 
    ''' (Namensschema "PRAEFIX_XXXX", wobei XXXX = 4st. lfd. Nr.)
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetMaxUsernameNumber(ByVal praefix As String) As Integer

        Dim erg As Integer = 0

        Using cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            cn.Open()

            Dim cmdCheckMaxUsernameNumber As New SqlClient.SqlCommand("SELECT MAX(Username) FROM WebUser WHERE Username LIKE @Username", cn)
            cmdCheckMaxUsernameNumber.Parameters.AddWithValue("@Username", praefix & "[_]%")
            Dim sqlerg As Object = cmdCheckMaxUsernameNumber.ExecuteScalar()
            If sqlerg IsNot DBNull.Value AndAlso Not String.IsNullOrEmpty(cmdCheckMaxUsernameNumber.ExecuteScalar().ToString()) Then
                Integer.TryParse(cmdCheckMaxUsernameNumber.ExecuteScalar().ToString().Replace(praefix & "_", ""), erg)
            End If

            cn.Close()
        End Using

        Return erg

    End Function

    Private Function GroupExists(ByVal Groupname As String) As String

        Dim ToolTipText As String = String.Empty

        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()

        Dim cmdCheckGroupExists As New SqlClient.SqlCommand("SELECT COUNT(GroupID) FROM WebGroup WHERE Groupname=@Groupname and CustomerID =@CustomerID", cn)
        cmdCheckGroupExists.Parameters.AddWithValue("@Groupname", Groupname)
        cmdCheckGroupExists.Parameters.AddWithValue("@CustomerID", ddlFilterCustomer.SelectedValue)
        If cmdCheckGroupExists.ExecuteScalar.ToString = "0" Then
            ToolTipText = "Die Gruppe <<" & Groupname & ">> ist unbekannt."
        End If

        cn.Close()
        cn.Dispose()

        Return ToolTipText

    End Function

    Private Function GetGroupID(ByVal Groupname As String) As Integer

        Dim TempTable As New DataTable

        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()

        Dim cmdCheckGroupExits As SqlClient.SqlDataAdapter
        cmdCheckGroupExits = New SqlClient.SqlDataAdapter("SELECT GroupID FROM WebGroup WHERE Groupname=@Groupname and CustomerID =@CustomerID", cn)
        cmdCheckGroupExits.SelectCommand.Parameters.AddWithValue("@Groupname", Groupname)
        cmdCheckGroupExits.SelectCommand.Parameters.AddWithValue("@CustomerID", ddlFilterCustomer.SelectedValue)

        cmdCheckGroupExits.Fill(TempTable)

        cn.Close()
        cn.Dispose()

        Return CInt(TempTable.Rows(0)(0))

    End Function

    Private Function OrganizationExists(ByVal OrgName As String) As String

        Dim ToolTipText As String = String.Empty

        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()

        Dim cmdCheckOrgExits As New SqlClient.SqlCommand("SELECT COUNT(OrganizationID) FROM Organization WHERE Organizationname=@Organizationname and CustomerID =@CustomerID", cn)
        cmdCheckOrgExits.Parameters.AddWithValue("@Organizationname", OrgName)
        cmdCheckOrgExits.Parameters.AddWithValue("@CustomerID", ddlFilterCustomer.SelectedValue)
        If cmdCheckOrgExits.ExecuteScalar.ToString = "0" Then
            ToolTipText = "Die Organisation <<" & OrgName & ">> ist unbekannt."
        End If

        cn.Close()
        cn.Dispose()

        Return ToolTipText

    End Function

    Private Function GetOrganizationID(ByVal OrgName As String) As Integer

        Dim TempTable As New DataTable

        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()

        Dim cmdCheckOrgExits As SqlClient.SqlDataAdapter
        cmdCheckOrgExits = New SqlClient.SqlDataAdapter("SELECT OrganizationID FROM Organization WHERE Organizationname=@Organizationname and CustomerID =@CustomerID", cn)
        cmdCheckOrgExits.SelectCommand.Parameters.AddWithValue("@Organizationname", OrgName)
        cmdCheckOrgExits.SelectCommand.Parameters.AddWithValue("@CustomerID", ddlFilterCustomer.SelectedValue)

        cmdCheckOrgExits.Fill(TempTable)

        cn.Close()
        cn.Dispose()

        Return CInt(TempTable.Rows(0)(0))

    End Function

    Function EmailAddressCheck(ByVal emailAddress As String) As Boolean
        Dim pattern As String = "^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"
        Dim emailAddressMatch As Match = Regex.Match(emailAddress, pattern)
        If emailAddressMatch.Success Then
            EmailAddressCheck = True
        Else
            EmailAddressCheck = False
        End If
    End Function

    Private Function CheckCustomerMaxUser(ByVal AnzahlUserInUploadTable As Integer) As Boolean

        Dim intCustomerID As Integer = CInt(ddlFilterCustomer.SelectedValue)
        Dim _customer As New Customer(intCustomerID, m_User.App.Connectionstring)

        Dim UsersInDb As Integer

        lblError.Text = String.Empty

        Using cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)

            cn.Open()

            Using cmd As SqlClient.SqlCommand = cn.CreateCommand()

                cmd.CommandType = CommandType.Text

                cmd.CommandText = "SELECT COUNT(*) FROM WebUser WHERE CustomerID = @CustomerID"

                cmd.Parameters.AddWithValue("@CustomerID", intCustomerID)

                UsersInDb = CInt(cmd.ExecuteScalar())

            End Using

            cn.Close()

        End Using

        If (UsersInDb + AnzahlUserInUploadTable) >= _customer.MaxUser Then
            lblError.Text = "Max. Useranzahl zum Kunden(" & _customer.MaxUser & ") wird überschritten." & vbCrLf & _
                            "Bereits angelegt: " & UsersInDb & ". Anzahl User im Upload: " & AnzahlUserInUploadTable & "."
            Return True
        Else
            Return False
        End If

    End Function

    Private Function CheckCustomerRemoteLoginAllowed() As Boolean
        Dim erg As Boolean = False

        Using cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            cn.Open()

            Dim cmdRemoteLoginAllowed As New SqlClient.SqlCommand("SELECT AllowURLRemoteLogin FROM dbo.Customer WHERE (dbo.Customer.CustomerID = @CustomerID)", cn)
            cmdRemoteLoginAllowed.Parameters.AddWithValue("@CustomerID", ddlFilterCustomer.SelectedValue)
            Dim sqlerg As Object = cmdRemoteLoginAllowed.ExecuteScalar()
            If sqlerg IsNot DBNull.Value AndAlso CBool(sqlerg) Then
                erg = True
            End If

            cn.Close()
        End Using

        Return erg

    End Function

    Private Sub ApplyCustomerSpecificSettings()

        lblWarenkorbNurEigene.Visible = RechtWarenkorbNurEigene
        lblWarenkorbNurEigeneZeile1.Visible = RechtWarenkorbNurEigene
        lblWarenkorbNurEigeneZeile2.Visible = RechtWarenkorbNurEigene

        lblDarfNichtAbsenden.Visible = RechtDarfNichtAbsenden
        lblDarfNichtAbsendenZeile1.Visible = RechtDarfNichtAbsenden
        lblDarfNichtAbsendenZeile2.Visible = RechtDarfNichtAbsenden

        grvAusgabe.Columns(14).Visible = RechtWarenkorbNurEigene
        grvAusgabe.Columns(15).Visible = RechtDarfNichtAbsenden

        Session("RechtWarenkorbNurEigene") = RechtWarenkorbNurEigene
        Session("RechtDarfNichtAbsenden") = RechtDarfNichtAbsenden

    End Sub

#End Region

End Class
