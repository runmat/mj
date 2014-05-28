Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common
Imports System.Data.OleDb
Imports System.Text.RegularExpressions
Public Class AutUserUpload
    Inherits System.Web.UI.Page

#Region "Declarations"
    Private m_User As User
    Private m_App As App
    Private Const ANZAHLEXCELSPALTEN As Integer = 11
    Private Const PFLICHTFELDLEER As String = "Dieses Pflichtfeld hat keinen Eintrag."

    Private Enum UploadExcelColumns
        Title = 0
        Firstname = 1
        LastName = 2
        Username = 3
        Reference = 4
        Store = 5
        TestUser = 6
        Groupname = 7
        Organization = 8
        EMailAdress = 9
        ValidFrom = 10
        ID = 11
    End Enum

#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        ucStyles.TitleText = "Automatisierte Benutzeranlage"
        AdminAuth(Me, m_User, AdminLevel.Organization)
        Try
            m_App = New App(m_User)

            If Not IsPostBack Then
                lblError.Text = ""
                FillCustomer()

                LoadLoginLinks()

                ddlPageSize.Items.Add("10")
                ddlPageSize.Items.Add("20")
                ddlPageSize.Items.Add("50")
                ddlPageSize.Items.Add("100")
                ddlPageSize.Items.Add("200")
                ddlPageSize.Items.Add("500")
                ddlPageSize.Items.Add("1000")
                ddlPageSize.SelectedIndex = 2

                'lbtnUpload.Visible = False
                'TableUpload.Visible = False
                tdExcel.Visible = False

                If Session("Checked") = Nothing Then
                    Session.Add("Checked", False)
                End If

            End If
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "AutUserUpload", "Page_Load", ex.ToString)

            lblError.Text = ex.ToString
            lblError.Visible = True
        End Try
    End Sub

    Protected Sub rblUpload_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rblUpload.SelectedIndexChanged

        lblNoData.Visible = False
        grvAusgabe.Visible = False

        Session("UploadTable") = Nothing

        If rblUpload.SelectedValue = "Upload" Then
            TableUpload.Visible = True
            lbtnLaden.Visible = False

            lbtnFreigeben.Text = Replace(lbtnFreigeben.Text, "Freigeben", "Suchen")
            lbtnFreigeben.Visible = False
            lbtnUpload.Visible = True

            ddlLoginPage.Visible = True
            lblLoginLink.Visible = True

        Else
            TableUpload.Visible = False
            lbtnUpload.Visible = False
            lbtnFreigeben.Visible = True

            ddlLoginPage.Visible = False
            lblLoginLink.Visible = False

        End If
    End Sub

    Protected Sub lbtnUpload_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnUpload.Click

        If ddlFilterCustomer.SelectedValue = 0 Then

            lblError.Text = "Bitte wählen Sie eine Firma aus."
            Exit Sub
        Else
            If CustomerNameInputOptional(ddlFilterCustomer.SelectedValue) = False Then
                lblError.Text = "Namensangaben zum Kunden nicht als Pflicht definiert. <br> Bitte im Kunden ändern."
                Exit Sub
            End If

            If ddlLoginPage.SelectedValue = 0 Then
                lblError.Text = "Bitte wählen Sie einen Link zur Startseite aus."
                Exit Sub
            End If


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
            ddlLoginPage.Enabled = False
            TableUpload.Visible = False
        End If
    End Sub

    Protected Sub lnkCreateExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel.Click
        Try
            Dim control As New Control
            Dim tblTranslations As New DataTable()
            Dim AppURL As String
            Dim TempTable As DataTable = CType(Session("UploadTable"), DataTable).Clone
            Dim AusgabeTable As DataTable = CType(Session("UploadTable"), DataTable).Copy
            Dim col2 As DataColumn

            AppURL = Replace(Me.Request.Url.LocalPath, "/Portal", "..")
            tblTranslations = CType(Me.Session(AppURL), DataTable)


            Dim FieldControl As DataControlField
            Dim Found As Boolean = False


            For Each col2 In TempTable.Columns

                Found = False


                For Each FieldControl In grvAusgabe.Columns

                    If col2.ColumnName.ToUpper = FieldControl.SortExpression.ToUpper Then
                        Found = True

                        AusgabeTable.Columns(col2.ColumnName).ColumnName = FieldControl.HeaderText
                        If FieldControl.Visible = False Then Found = False

                        Exit For

                    End If

                Next

                If Found = False Then
                    AusgabeTable.Columns.Remove(AusgabeTable.Columns(col2.ColumnName))

                End If

                AusgabeTable.AcceptChanges()
            Next

            If AusgabeTable.Columns.Count > 0 Then
                Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
                Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
                excelFactory.CreateDocumentAndSendAsResponse(strFileName, AusgabeTable, Me.Page)
            Else
                Err.Raise(-1, , "Fehler beim Erstellen der Excel-Datei.")
            End If


        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub grvAusgabe_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grvAusgabe.PageIndexChanging
        CheckGrid()
        FillGrid(e.NewPageIndex, String.Empty)
    End Sub

    Private Sub grvAusgabe_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grvAusgabe.Sorting
        CheckGrid()
        FillGrid(grvAusgabe.PageIndex, e.SortExpression)
    End Sub


    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        grvAusgabe.PageSize = CInt(ddlPageSize.SelectedItem.Value)
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
        Dim TempRow As DataRow


        Dim _User As User
        Dim _customer As New Customer(CInt(ddlFilterCustomer.SelectedItem.Value), m_User.App.Connectionstring)

        For Each TempRow In TempTable.Rows

            _User = New User(-1, _
                              TempRow(UploadExcelColumns.Username), _
                              TempRow(UploadExcelColumns.Reference), _
                              IIf(TempRow(UploadExcelColumns.TestUser) = "Ja", True, False), _
                              CInt(ddlFilterCustomer.SelectedItem.Value), _
                              False, _
                              False, _
                              True, _
                              False, _
                              False, False, _
                              m_User.App.Connectionstring, _
                              0, _
                              m_User.UserName, _
                              False, _
                              TempRow(UploadExcelColumns.Firstname), _
                              TempRow(UploadExcelColumns.LastName), _
                              TempRow(UploadExcelColumns.Title), _
                              TempRow(UploadExcelColumns.Store), _
                              False, _
                              IIf(TempRow(UploadExcelColumns.ValidFrom) Is System.DBNull.Value, String.Empty, TempRow(UploadExcelColumns.ValidFrom)))


            _User.Email = TempRow(UploadExcelColumns.EMailAdress)


            Dim strLogMsg As String = "User anlegen"

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

            Dim blnSuccess As Boolean = False
            Dim pword As String = ""
            Dim pwordconfirm As String = ""
            Dim LinkKey As String = ""


            If _User.Save() Then
                blnSuccess = True

                pword = _customer.CustomerPasswordRules.CreateNewPasswort(lblError.Text)
                blnSuccess = _User.ChangePassword("", pword, pword, m_User.UserName, True)


                _User.SetLastLogin(Now)
                _User.Organization.ReAssignUserToOrganization(m_User.UserName, -1, _User.UserID, intOrganizationID, False, m_User.App.Connectionstring)

                'Linkschlüssel generieren
                LinkKey = _customer.CustomerPasswordRules.CreateNewPasswort(lblError.Text)
                InsertIntoWebUserUpload(_User.UserID, pword, _User.UserName, LinkKey)

            Else
                lblError.Text = _User.ErrorMessage
            End If

            _User = Nothing

        Next

        lbtnLaden.Visible = False
        lbtnSave.Visible = False

    End Sub

    Protected Sub ddlFilterCustomer_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlFilterCustomer.SelectedIndexChanged
        If ddlFilterCustomer.SelectedValue = 0 Then

            lblError.Text = "Bitte wählen Sie eine Firma für den Upload aus."
            Exit Sub
        Else

            If rblUpload.SelectedValue = "Upload" Then
                TableUpload.Visible = True
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
            lblError.Visible = True
            lblError.Text = "Es wurden " & ErrCount & " Zeilen mit Fehlern gefunden. Bitte korrigieren Sie diese."
        Else
            grvAusgabe.Enabled = False
            lblError.Visible = False
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
            lblNoData.Visible = False
            tdExcel.Visible = False
            lbtnFreigeben.Text = Replace(lbtnFreigeben.Text, "Freigeben", "Suchen")
        Else
            LoadFreigaben()
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
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


        Dim UploadTable As New DataTable
        Dim Direction As String = String.Empty

        If IsNothing(Session("UploadTable")) = False Then

            UploadTable = CType(Session("UploadTable"), DataTable)

            If UploadTable.Rows.Count > 0 Then


                grvAusgabe.Visible = True
                ddlPageSize.Visible = True

                tdExcel.Visible = True

                Dim TempPageIndex As Int32 = PageIndex

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
                lnkCreateExcel.Visible = True

                lblNoData.Text = "Es wurden " & UploadTable.Rows.Count.ToString & " Datensätze gefunden."
                lblNoData.Visible = True

                If Not Session("Checked") = Nothing Then
                    CheckGrid()
                End If

            Else
                lblNoData.Visible = True
                lblNoData.Text = "Es wurden keine Daten gefunden."

            End If
        Else
            lblNoData.Text = "Es wurden keine Daten gefunden."
            lblNoData.Visible = True
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

    Private Sub Upload(ByVal uFile As System.Web.HttpPostedFile)
        Try

            Dim filepath As String = ConfigurationManager.AppSettings("UploadpathLocal")
            Dim filename As String
            Dim info As System.IO.FileInfo
            Dim TempTable As New DataTable
            filename = m_User.UserName & "_" & Format(Now, "yyyyMMddhhmmss") & ".xls"

            lblError.Text = String.Empty

            If Not (uFile Is Nothing) Then
                uFile.SaveAs(ConfigurationManager.AppSettings("UploadpathLocal") & filename)
                uFile = Nothing
                info = New System.IO.FileInfo(filepath & filename)
                If Not (info.Exists) Then
                    lblError.Text = "Fehler beim Speichern."
                    Exit Sub
                End If
                'Datei gespeichert -> Auswertung
                TempTable = getDataTableFromExcel(filepath, filename)

                If TempTable.Columns.Count = ANZAHLEXCELSPALTEN Then
                    If TempTable.Rows.Count < 1 Then
                        Err.Raise(-1, , "Die Exceldatei enthält keine Daten.")
                    End If
                Else
                    Err.Raise(-1, , "Die Exceldatei hat nicht die korrekt Anzahl an Spalten.")

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

        'x wird hinzugefügt, da über OLEBD nur 8 Zeilen ausgelesen werden,
        'um den Datentyp zu bestimmen. Das führt zu Problemen.
        For i As Integer = 0 To 10
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

        For i As Integer = 0 To 10
            Row(i) = Mid(Row(i), 2)
        Next

        dt.AcceptChanges()


        Return dt


    End Function

    Private Sub CheckTableData(ByVal TempTable As DataTable)

        Dim Column As New DataColumn

        Dim TempRow As DataRow

        Column.DataType = System.Type.GetType("System.Int32")
        Column.ColumnName = "ID"

        TempTable.Columns.Add(Column)
        TempTable.AcceptChanges()

        Dim e As Integer = 0

        For Each TempRow In TempTable.Rows

            TempRow("ID") = e


            If TempRow(UploadExcelColumns.Username) Is DBNull.Value Then
                lblError.Text = "Abbruch: Benutzername fehlt. Überprüfen Sie die Exceldatei."
                Exit Sub
            End If

            e += 1
        Next


        Dim EnumArray() As String = [Enum].GetNames(GetType(UploadExcelColumns))

        Dim i As Integer = 0

        Column = New DataColumn

        For Each Column In TempTable.Columns
            Column.ColumnName = EnumArray(i)
            i += 1
        Next


        Column = New DataColumn

        Column.DataType = System.Type.GetType("System.Boolean")
        Column.ColumnName = "Error"
        Column.DefaultValue = False

        TempTable.Columns.Add(Column)


        TempTable.AcceptChanges()


        Session.Add("UploadTable", TempTable)

        lbtnUpload.Visible = False
        lbtnPruefen.Visible = True
        lblMessage.Text = "Bitte überprüfen Sie, ob die Daten in den richtigen Spalten stehen."
        FillGrid(0)


    End Sub

    Private Sub WriteIntoUploadTable(ByVal TempRow As DataRow, ByVal cn As SqlClient.SqlConnection)

        Dim SQL As String

        SQL = "INSERT INTO WebUserUpload(CustomerID, " & _
                             "Username, " & _
                             "Title," & _
                             "FirstName," & _
                             "LastName," & _
                             "Reference, " & _
                             "Store," & _
                             "TestUser, " & _
                             "GroupName, " & _
                             "Organization, " & _
                             "EMailAdress, " & _
                             "ValidFrom, " & _
                             "CreatedBy) " & _
                     "VALUES(@CustomerID, " & _
                            "@Username, " & _
                            "@Title, " & _
                            "@FirstName, " & _
                            "@LastName, " & _
                            "@Reference, " & _
                            "@Store, " & _
                            "@TestUser, " & _
                            "@GroupName, " & _
                            "@Organization, " & _
                            "@EMailAdress, " & _
                            "@ValidFrom, " & _
                            "@CreatedBy) "


        Dim cmdUser As New SqlClient.SqlCommand(SQL, cn)
        With cmdUser.Parameters
            .AddWithValue("@CustomerID", ddlFilterCustomer.SelectedValue)
            .AddWithValue("@Username", TempRow(UploadExcelColumns.Username))
            .AddWithValue("@Title", TempRow(UploadExcelColumns.Title))
            .AddWithValue("@FirstName", TempRow(UploadExcelColumns.Firstname))
            .AddWithValue("@LastName", TempRow(UploadExcelColumns.LastName))
            .AddWithValue("@Reference", TempRow(UploadExcelColumns.Reference))
            .AddWithValue("@Store", TempRow(UploadExcelColumns.Store))
            .AddWithValue("@TestUser", TempRow(UploadExcelColumns.TestUser))
            .AddWithValue("@GroupName", TempRow(UploadExcelColumns.Groupname))
            .AddWithValue("@Organization", TempRow(UploadExcelColumns.Organization))
            .AddWithValue("@EMailAdress", TempRow(UploadExcelColumns.EMailAdress))
            .AddWithValue("@ValidFrom", TempRow(UploadExcelColumns.ValidFrom))
            .AddWithValue("@CreatedBy", User.Identity.Name)

        End With


        cmdUser.ExecuteNonQuery()


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

    Private Sub LoadLoginLinks()

        Dim TempTable As New DataTable
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()

        Dim daLoginLink As SqlClient.SqlDataAdapter
        daLoginLink = New SqlClient.SqlDataAdapter("SELECT * FROM WebUserUploadLoginLink", cn)

        daLoginLink.Fill(TempTable)

        cn.Close()
        cn.Dispose()

        Dim dItem As New ListItem

        dItem.Text = " --Auswahl-- "
        dItem.Value = 0

        ddlLoginPage.Items.Add(dItem)

        For Each row As DataRow In TempTable.Rows

            dItem = New ListItem

            dItem.Text = row("Text").ToString
            dItem.Value = row("ID")

            ddlLoginPage.Items.Add(dItem)

        Next


    End Sub

    Private Sub InsertIntoWebUserUpload(ByVal UserID As Integer, ByRef PWord As String, ByVal Username As String, ByVal LinkKey As String)
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()

        Dim cmdInsert As New SqlClient.SqlCommand("INSERT INTO WebUserUpload(UserID,Password,RightUserLink,WrongUserLink,LoginLinkID) Values(@UserID,@Password,@RightUserLink,@WrongUserLink,@LoginLinkID)", cn)
        Dim RightUser As String
        Dim WrongUser As String

        Dim Crypto As New Crypt


        RightUser = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Username & LinkKey & "Right", "sha1")
        WrongUser = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Username & LinkKey & "Wrong", "sha1")

        PWord = Crypto.psEncrypt(PWord)

        With cmdInsert.Parameters
            .AddWithValue("@UserID", UserID)
            .AddWithValue("@Password", PWord)
            .AddWithValue("@RightUserLink", RightUser)
            .AddWithValue("@WrongUserLink", WrongUser)
            .AddWithValue("@LoginLinkID", ddlLoginPage.SelectedValue)
        End With
        cmdInsert.ExecuteNonQuery()

        cn.Close()
        cn.Dispose()

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
            RowSet = TempTable.Select("ID = " & GridRow.Cells(12).Text)

            RowSet(0).BeginEdit()

            For i As Integer = 0 To 10
                For Each GridControl In GridRow.Cells(i).Controls

                    If TypeOf GridControl Is TextBox Then
                        txt = CType(GridControl, TextBox)
                        If i <> UploadExcelColumns.ValidFrom Then
                            RowSet(0)(i) = txt.Text
                        Else
                            If txt.Text = String.Empty Then
                                RowSet(0)(i) = System.DBNull.Value
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

    Private Function GroupExists(ByVal Groupname As String) As String

        Dim ToolTipText As String = String.Empty

        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()

        Dim cmdCheckGroupExits As New SqlClient.SqlCommand("SELECT COUNT(GroupID) FROM WebGroup WHERE Groupname=@Groupname and CustomerID =@CustomerID", cn)
        cmdCheckGroupExits.Parameters.AddWithValue("@Groupname", Groupname)
        cmdCheckGroupExits.Parameters.AddWithValue("@CustomerID", ddlFilterCustomer.SelectedValue)
        If cmdCheckGroupExits.ExecuteScalar.ToString = "0" Then
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




    Private Function CustomerNameInputOptional(ByVal ID As String) As Boolean

        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()

        Dim cmdCustomer As New SqlClient.SqlCommand("SELECT Count(CustomerID) FROM Customer WHERE CustomerID=@ID and NameInputOptional = 1", cn)
        cmdCustomer.Parameters.AddWithValue("@ID", ID)
        If cmdCustomer.ExecuteScalar.ToString = "0" Then
            Return True
        Else
            Return False
        End If

        cn.Close()
        cn.Dispose()

    End Function


    Private Function CheckCustomerMaxUser(ByVal AnzahlUserInUploadTable As Integer) As Boolean

        Dim TempTable As New DataTable
        Dim SQL As String
        Dim HaveUsers As Integer

        lblError.Text = String.Empty


        SQL = " SELECT "
        SQL = SQL & "   COUNT(dbo.WebUser.UserID) AS AnzahlUser,"
        SQL = SQL & "   dbo.Customer.MaxUser"
        SQL = SQL & " FROM "
        SQL = SQL & "   dbo.WebUser INNER JOIN dbo.Customer ON dbo.WebUser.CustomerID = dbo.Customer.CustomerID"
        SQL = SQL & " WHERE "
        SQL = SQL & "   (dbo.WebUser.CustomerID = @CustomerID)"
        SQL = SQL & " GROUP BY dbo.Customer.MaxUser"

        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()

        Dim cmdMaxUser As SqlClient.SqlDataAdapter
        cmdMaxUser = New SqlClient.SqlDataAdapter(SQL, cn)
        cmdMaxUser.SelectCommand.Parameters.AddWithValue("@CustomerID", ddlFilterCustomer.SelectedValue)


        cmdMaxUser.Fill(TempTable)

        'Es wurden noch keine User angelegt
        If TempTable.Rows.Count = 0 Then
            HaveUsers = 0

            SQL = " SELECT "
            SQL = SQL & "   dbo.Customer.MaxUser"
            SQL = SQL & " FROM "
            SQL = SQL & "   dbo.Customer "
            SQL = SQL & " WHERE "
            SQL = SQL & "   (dbo.Customer.CustomerID = @CustomerID)"

            cmdMaxUser = New SqlClient.SqlDataAdapter(SQL, cn)
            cmdMaxUser.SelectCommand.Parameters.AddWithValue("@CustomerID", ddlFilterCustomer.SelectedValue)

            cmdMaxUser.Fill(TempTable)

        Else
            HaveUsers = CInt(TempTable.Rows(0)("AnzahlUser"))
            

        End If

        cn.Close()
        cn.Dispose()

        If (HaveUsers + AnzahlUserInUploadTable) >= CInt(TempTable.Rows(0)("MaxUser")) Then
            lblError.Text = "Max. Useranzahl zum Kunden(" & CInt(TempTable.Rows(0)("MaxUser")) & ") wird überschritten." & vbCrLf & _
                            "Bereits angelegt: " & HaveUsers & ". Anzahl User im Upload: " & AnzahlUserInUploadTable & "."
            Return True
        Else
            Return False
        End If



    End Function

#End Region

End Class

' ************************************************
' $History: AutUserUpload.aspx.vb $
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 7.04.10    Time: 11:11
' Updated in $/CKAG/admin
' ITA: 3610
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 1.07.09    Time: 11:42
' Updated in $/CKAG/admin
' ITA 2963 fertig
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 21.04.09   Time: 11:29
' Updated in $/CKAG/admin
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 17.04.09   Time: 16:59
' Updated in $/CKAG/admin
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 1.04.09    Time: 14:37
' Created in $/CKAG/admin
' 