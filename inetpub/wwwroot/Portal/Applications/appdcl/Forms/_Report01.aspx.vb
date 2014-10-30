Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security
Imports CuteWebUI
Imports System.Collections.Generic
Imports System.IO

<CLSCompliant(False)> Partial Class _Report01
    Inherits System.Web.UI.Page

    Private m_App As App
    Private m_User As User

    Protected WithEvents ucHeader As CKG.Portal.PageElements.Header
    Protected WithEvents ucStyles As CKG.Portal.PageElements.Styles

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

    Protected Overrides Sub OnLoad(e As System.EventArgs)
        MyBase.OnLoad(e)

        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text

        Try
            m_App = New Base.Kernel.Security.App(m_User)

            If Not IsPostBack Then
                loadForm(getFahrerID())
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Protected Overrides Sub OnPreRender(e As System.EventArgs)
        MyBase.OnPreRender(e)

        Dim auftragSelected = lbxAuftrag.SelectedIndex <> -1
        Uploader1.Visible = auftragSelected
        lbDeleteAll.Visible = auftragSelected

        SetEndASPXAccess(Me)
    End Sub

    Protected Overrides Sub OnUnload(e As System.EventArgs)
        MyBase.OnUnload(e)

        SetEndASPXAccess(Me)
    End Sub

    Private m_uploadedFiles As Dictionary(Of String, UploadedFile)

    Private ReadOnly Property UploadedFiles As Dictionary(Of String, UploadedFile)
        Get
            If m_uploadedFiles Is Nothing Then
                Dim sessionObj As Object = Session("UploadedFiles")
                If Not sessionObj Is Nothing AndAlso TypeOf sessionObj Is Dictionary(Of String, UploadedFile) Then
                    m_uploadedFiles = DirectCast(sessionObj, Dictionary(Of String, UploadedFile))
                Else
                    m_uploadedFiles = New Dictionary(Of String, UploadedFile)
                    Session("UploadedFiles") = m_uploadedFiles
                End If
            End If
            Return m_uploadedFiles
        End Get
    End Property

    Private Sub loadForm(ByVal fahrerID As Integer)
        Dim table As DataTable

        lblFahrernr.Text = fahrerID
        Dim status = String.Empty
        Dim uebf = New Ueberfuehrung(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
        Try
            table = uebf.leseFahrerDatenSAP(Session("AppID").ToString, Session.SessionID.ToString, status, fahrerID.ToString())
        Catch ex As Exception

            lblError.Text = "Keine Aufträge gefunden."
            Return
        End Try

        If (status <> String.Empty) Then
            lblError.Text = status  'Fehler
        Else
            If table.Rows.Count > 0 Then
                Dim count = 0
                'Ok, Liste mit den Tourdaten füllen....
                For Each row As DataRow In table.Rows
                    Dim strItem = row("Daten").ToString
                    lbxAuftrag.Items.Add(New ListItem(strItem, count))
                    count += 1
                Next
            Else
                lblError.Text = "Keine Tourdaten gefunden."
            End If
        End If
    End Sub

    Private Sub FillGridServer(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "", Optional ByVal direction As String = "")
        Dim table As DataTable

        table = CType(Session("Serverfiles"), DataTable)

        If table Is Nothing Then
            gridServer.DataSource = Nothing
            gridServer.DataBind()
            lblNoData.Text = String.Empty
            gridServer.Visible = False
            Return
        End If

        If table.Rows.Count = 0 Then
            gridServer.Visible = False
            lblNoData.Visible = True
            lblNoData.Text = "Keine Daten zur Anzeige gefunden."
        Else
            gridServer.Visible = True
            lblNoData.Visible = False

            Dim tmpDataView As DataView = table.DefaultView

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

            If (direction.Length > 0) Then
                tmpDataView.Sort = strTempSort & " " & direction
            End If

            gridServer.DataSource = tmpDataView
            gridServer.DataBind()

            If (Not Session("ShowOtherString") Is Nothing) AndAlso CStr(Session("ShowOtherString")).Length > 0 Then
                lblNoData.Text = CStr(Session("ShowOtherString"))
            Else
                lblNoData.Text = "Es wurden " & tmpDataView.Count.ToString & " Dokument(e) gefunden."
            End If
            If (Not Session("BackLink") Is Nothing) AndAlso CStr(Session("BackLink")) = "HistoryBack" Then
                lnkKreditlimit.Text = "Zurück"
                lnkKreditlimit.NavigateUrl = "javascript:history.back()"
            End If
            lblNoData.Visible = True
        End If
    End Sub

    Private Sub gridServer_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles gridServer.ItemCommand
        Dim filename As String
        Dim file As FileInfo
        Dim filenameBackup As String
        Dim fileBackup As FileInfo

        Dim table As DataTable
        Dim row As DataRow
        Dim status As String

        table = CType(Session("Serverfiles"), DataTable)
        status = String.Empty

        If e.CommandName = "Delete" Then
            filename = ConfigurationManager.AppSettings("UploadpathLocal") & e.Item.Cells(4).Text
            file = New FileInfo(filename)

            filenameBackup = ConfigurationManager.AppSettings("UploadpathLocalBackup") & e.Item.Cells(4).Text
            fileBackup = New FileInfo(filenameBackup)

            Try
                If file.Exists Then
                    file.Delete()

                    If fileBackup.Exists Then
                        fileBackup.Delete()
                    End If

                    loadData()
                Else
                    status = "Datei nicht gefunden."
                End If
            Catch ex As Exception
                status = "Fehler beim Löschen der Datei."
            Finally
                If (status <> String.Empty) Then
                    row = table.Select("Filename = '" & e.Item.Cells(4).Text & "'")(0)
                    row("Status") = status
                    table.AcceptChanges()
                    Session("Serverfiles") = table
                End If
                FillGridServer(0)
            End Try
        End If
    End Sub

    Private Sub gridServer_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles gridServer.SortCommand
        FillGridServer(gridServer.CurrentPageIndex, e.SortExpression)
    End Sub

    Public Function getAuftragsNr() As Integer
        Dim auftragsnr As String

        If lbxAuftrag.SelectedIndex >= 0 Then
            auftragsnr = lbxAuftrag.Items.Item(lbxAuftrag.SelectedIndex).Text
            auftragsnr = Left(auftragsnr, auftragsnr.IndexOf("."))
        Else
            auftragsnr = String.Empty
        End If

        Return Integer.Parse(auftragsnr)
    End Function

    Public Function getTourNr() As Integer
        Dim tournr As String

        If lbxAuftrag.SelectedIndex >= 0 Then
            tournr = lbxAuftrag.Items.Item(lbxAuftrag.SelectedIndex).Text
            tournr = Right(tournr, tournr.Length - tournr.IndexOf(".") - 1)
            tournr = Left(tournr, 1)
        Else
            tournr = String.Empty
        End If

        Return Integer.Parse(tournr)
    End Function

    Private Function getFahrerID() As Integer
        Dim fahrerId = String.Empty
        If Not String.IsNullOrEmpty(m_User.Reference) Then
            fahrerId = m_User.Reference
        ElseIf Not Request.QueryString.Item("FID") Is Nothing Then
            fahrerId = Request.QueryString.Item("FID").ToString
        Else
            Throw New ApplicationException("Keine Fahrernummer übergeben!")
        End If

        Return Integer.Parse(fahrerId)
    End Function

    Private Sub loadData()
        Dim table As DataTable
        Dim uebf As Ueberfuehrung

        uebf = New Ueberfuehrung(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
        table = uebf.readDataFromServer(getAuftragsNr(), getTourNr(), True)

        If Not (table Is Nothing) AndAlso (table.Rows.Count > 0) Then
            Session("Serverfiles") = table
        Else
            Session("Serverfiles") = Nothing
        End If
    End Sub

    Protected Sub lbDeleteAll_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim status As String = ""
        For Each GridRow As DataGridItem In gridServer.Items

            Dim filename = ConfigurationManager.AppSettings("UploadpathLocal") & GridRow.Cells(4).Text
            Dim file = New FileInfo(filename)

            Dim filenameBackup = ConfigurationManager.AppSettings("UploadpathLocalBackup") & GridRow.Cells(4).Text
            Dim fileBackup = New FileInfo(filenameBackup)
            Try
                If file.Exists Then
                    file.Delete()
                Else
                    status = "Datei nicht gefunden."
                End If

                If fileBackup.Exists Then
                    fileBackup.Delete()
                Else
                    status = "Datei nicht gefunden."
                End If
            Catch ex As Exception
                status = "Fehler beim Löschen der Dateien."
            End Try
        Next

        lblError.Text = status
        loadData()
        FillGridServer(0)
        lbDeleteAll.Visible = False
    End Sub

    Protected Sub lbxAuftrag_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles lbxAuftrag.SelectedIndexChanged
        Dim auftragsNr As Integer
        Try
            auftragsNr = getAuftragsNr()

            loadData()
            FillGridServer(0)
            lblError.Text = String.Empty
            lblInfo1.Text = lbxAuftrag.SelectedItem.Text

            Dim table As New DataTable
            If Not Session("Serverfiles") Is Nothing Then table = CType(Session("Serverfiles"), DataTable)
            If table.Rows.Count > 0 Then
                lbDeleteAll.Visible = True
                divHelp.Visible = False
            Else
                lbDeleteAll.Visible = False
            End If
        Catch ex As Exception
            lblError.Text = "Kein Auftrag ausgewählt."
        End Try
    End Sub

    Protected Sub ibtnHelp_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnHelp.Click
        gridServer.Visible = False
        divHelp.Visible = True
    End Sub

    Protected Sub lbtnClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnClose.Click
        gridServer.Visible = True
        divHelp.Visible = False
    End Sub

    Protected Sub UploaderAttachmentAdded(sender As Object, e As AttachmentItemEventArgs)
        Dim item = e.Item
        If UploadedFiles.ContainsKey(item.FileName) Then
            item.Remove()
            item.Delete()

            lblError.Text += "Datei """ & item.FileName & """ ist bereits in der Liste vorhanden.<br />"
            Return
        Else
            UploadedFiles(item.FileName) = UploadedFile.FromAttachmentItem(item)
        End If
    End Sub

    Protected Sub UploaderAttachmentRemove(ByVal sender As Object, ByVal e As AttachmentItemEventArgs)
        Dim item = e.Item
        If UploadedFiles.ContainsKey(item.FileName) Then
            UploadedFiles.Remove(item.FileName)
        End If
    End Sub

    Protected Sub UploaderCompleted(sender As Object, e As UploaderEventArgs())
        If (lbxAuftrag.SelectedItem Is Nothing) Then
            lblError.Text = "Kein Auftrag ausgewählt."
            Return
        End If

        If (UploadedFiles.Count = 0) Then
            lblError.Text = "Keine Datei(en) ausgewählt."
            Return
        End If

        Dim fahrerID = 0
        Try
            fahrerID = getFahrerID()
        Catch aex As ApplicationException
            lblError.Text = aex.Message
            Return
        End Try

        Dim auftrag = getAuftragsNr()
        Dim fahrt = getTourNr()

        Dim destination = New DirectoryInfo(ConfigurationManager.AppSettings("UploadPathLocal"))
        If Not destination.Exists Then
            destination.Create()
        End If

        Dim destinationBackup = New DirectoryInfo(ConfigurationManager.AppSettings("UploadPathLocalBackup"))
        If Not destinationBackup.Exists Then
            destinationBackup.Create()
        End If

        Dim fileNames = New List(Of String)(UploadedFiles.Keys)

        For Each filename In fileNames
            Dim f = UploadedFiles(filename)

            Try
                Dim mvcFile = Uploader1.GetUploadedFile(f.Id)
                Dim fileDestination = ReviewFilename.NextFilename(destination.FullName, ".JPG", auftrag, fahrerID, fahrt)
                ' Nur den Ordnernamen im Pfad austauschen, damit Bild und Backup den selben Dateinamen haben
                Dim fileBackupDestination = fileDestination.Replace(destination.FullName, destinationBackup.FullName)

                mvcFile.CopyTo(fileDestination)
                mvcFile.CopyTo(fileBackupDestination)

                If Not File.Exists(fileDestination) Then
                    Throw New ApplicationException("Fehler beim Speichern.")
                Else
                    f.Status = "OK"

                    For i = 0 To Uploader1.Items.Count - 1
                        Dim item = Uploader1.Items(i)
                        If (item.FileName = filename) Then
                            item.Remove()
                            item.Delete()
                            Exit For
                        End If
                    Next

                    mvcFile.Delete()

                    UploadedFiles.Remove(filename)
                End If
            Catch ex As Exception
                lblError.Text = "Fehler beim Speichern."
                If Not Uploader1 Is Nothing Then
                    Uploader1.DeleteAllAttachments()
                End If
                UploadedFiles.Clear()
                Return
            End Try
        Next

        loadData()
        FillGridServer(0)
    End Sub

End Class
