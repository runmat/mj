Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common
Imports System.IO
Imports System.Linq
Imports System.Collections.Generic
'Imports GeneralTools.Models
'Imports GeneralTools.Services
Imports DocumentTools.Services


<CLSCompliant(False)> Partial Class _Report02
    Inherits System.Web.UI.Page

    Protected WithEvents ucStyles As Styles
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblScript As System.Web.UI.WebControls.Label
    Protected WithEvents cbxArc As System.Web.UI.WebControls.CheckBox
    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User


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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)

        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text

        Try
            m_App = New Base.Kernel.Security.App(m_User)

            If Not IsPostBack Then
                FillAuftragsListe()
            Else
                lblMsg.Text = String.Empty
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Const AuftragsListeKey = "AuftragsListe"
    Private m_auftragsListe As List(Of ReviewAuftrag)

    Private ReadOnly Property AuftragsListe As List(Of ReviewAuftrag)
        Get
            If m_auftragsListe Is Nothing Then
                Dim sessionObj As Object = Session(AuftragsListeKey)
                If Not sessionObj Is Nothing AndAlso GetType(List(Of ReviewAuftrag)).Equals(sessionObj.GetType()) Then
                    m_auftragsListe = CType(sessionObj, List(Of ReviewAuftrag))
                Else
                    Dim folder = New DirectoryInfo(ConfigurationManager.AppSettings("UploadPathLocal"))
                    Dim backupFolder = New DirectoryInfo(ConfigurationManager.AppSettings("UploadPathLocalBackup"))
                    Dim serverFolder = ConfigurationManager.AppSettings("UploadPath")

                    Dim files = ReviewFile.FindUploadedFiles(folder.FullName, backupFolder.FullName, serverFolder, True)
                    Dim auftraege = ReviewAuftrag.GroupFiles(files)

                    Dim ueberf = New Ueberfuehrung(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                    ueberf.ReadAuftragsdaten(auftraege)
                    m_auftragsListe = auftraege
                    Session(AuftragsListeKey) = m_auftragsListe
                End If
            End If
            Return m_auftragsListe
        End Get
    End Property

    Private Sub FillAuftragsListe()
        m_auftragsListe = Nothing
        Session.Remove(AuftragsListeKey)

        lbxAuftrag.DataSource = AuftragsListe
        lbxAuftrag.DataBind()

        FillGridServer()
    End Sub

    Private Sub FillGridServer()
        Dim auftrag = GetSelectedAuftrag()

        gridServer.DataSource = Nothing

        If Not auftrag Is Nothing AndAlso auftrag.Files.Count > 0 Then
            gridServer.DataSource = auftrag.Files
        End If

        gridServer.DataBind()

        UpdateSelection()
        FillMoveToList(auftrag)

        btnFinish.Enabled = Not gridServer.DataSource Is Nothing
        btnBack.Enabled = Not gridServer.DataSource Is Nothing
        btnConfirm.Enabled = Not gridServer.DataSource Is Nothing
    End Sub

    Private Sub FillMoveToList(ByVal selectedAuftrag As ReviewAuftrag)
        If AuftragsListe Is Nothing OrElse selectedAuftrag Is Nothing Then
            moveToList.DataSource = Nothing
        Else
            Dim otherAuftraege = AuftragsListe.Where(Function(a) Not a.Equals(selectedAuftrag)).ToList()
            moveToList.DataSource = IIf(otherAuftraege.Count > 0, otherAuftraege, Nothing)
        End If
        moveToList.DataBind()
        moveLabel.Enabled = Not moveToList.DataSource Is Nothing
        moveToList.Enabled = Not moveToList.DataSource Is Nothing
        moveButton.Enabled = Not moveToList.DataSource Is Nothing
    End Sub

    Private Function GetSelectedAuftrag() As ReviewAuftrag
        Return AuftragsListe.ElementAtOrDefault(lbxAuftrag.SelectedIndex)
    End Function

    Protected Sub AuftragSelected(ByVal sender As Object, ByVal e As EventArgs)
        SetFinished(False)
        FillGridServer()
    End Sub

    Protected Sub FileSelected(ByVal sender As Object, ByVal e As EventArgs)
        Dim auftrag = GetSelectedAuftrag()
        If auftrag Is Nothing Then Return

        Dim chk = DirectCast(sender, CheckBox)
        Dim row = DirectCast(chk.Parent.Parent, GridViewRow)

        Dim filename = gridServer.DataKeys(row.DataItemIndex).Value.ToString()
        Dim file = auftrag.Files.FirstOrDefault(Function(f) f.Filename = filename)

        If Not file Is Nothing Then
            file.Selected = chk.Checked
        End If

        UpdateSelection()
    End Sub

    Protected Sub MoveClick(ByVal sender As Object, ByVal e As EventArgs)
        Dim auftrag = GetSelectedAuftrag()
        If auftrag Is Nothing Then Return

        Dim destAuftrag = AuftragsListe.FirstOrDefault(Function(a) a.ToString().Equals(moveToList.SelectedValue.ToString()))
        If destAuftrag Is Nothing Then Return

        Dim moveFiles = auftrag.Files.Where(Function(f) f.Selected).ToList()
        Dim folder = New DirectoryInfo(ConfigurationManager.AppSettings("UploadPathLocal"))
        Dim backupFolder = New DirectoryInfo(ConfigurationManager.AppSettings("UploadpathLocalBackup"))
        For Each file In moveFiles
            Try
                file.MoveTo(folder.FullName, file.Extension, backupFolder.FullName, destAuftrag)
            Catch ex As Exception
                lblError.Text = ex.ToString
                lblError.Visible = True
                Return
            End Try
        Next
        If moveFiles.Count = 1 Then
            lblMsg.Text = "Ein Bild nach " & destAuftrag.ToString() & " verschoben."
        Else
            lblMsg.Text = moveFiles.Count & " Bilder nach " & destAuftrag.ToString() & " verschoben."
        End If

        FillAuftragsListe()
        lbxAuftrag.SelectedIndex = AuftragsListe.FindIndex(Function(a) a.AuftragsNummer = auftrag.AuftragsNummer AndAlso a.FahrerID = auftrag.FahrerID AndAlso a.Fahrt = auftrag.Fahrt)
        FillGridServer()
    End Sub

    Private Sub UpdateSelection()
        Dim auftrag = GetSelectedAuftrag()
        If auftrag Is Nothing Then
            movePanel.Visible = False
            Return
        End If

        Dim selected = auftrag.Files.Where(Function(f) f.Selected).Count()
        If selected = 0 Then
            moveLabel.Text = "Keine Bilder gewählt"
            movePanel.Visible = False
        Else
            If selected = 1 Then
                moveLabel.Text = "1 Bild nach"
            Else
                moveLabel.Text = selected & " Bilder nach"
            End If
            movePanel.Visible = True
        End If
    End Sub

    Protected Sub GridServerRowDeleting(ByVal source As Object, ByVal e As GridViewDeleteEventArgs)
        Dim auftrag = GetSelectedAuftrag()
        If auftrag Is Nothing Then Return

        Dim file = auftrag.Files.ElementAtOrDefault(e.RowIndex)
        If file Is Nothing Then Return

        auftrag.Files.Remove(file)
        file.Delete()

        If auftrag.Files.Count = 0 Then
            FillAuftragsListe()
        End If

        FillGridServer()
    End Sub

    Protected Sub FinishClick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        SetFinished(True)
    End Sub

    Protected Sub ConfirmClick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim auftrag = GetSelectedAuftrag()
        If auftrag Is Nothing Then Return

        Dim targetArchiv = Path.Combine(ConfigurationManager.AppSettings("UploadPathSambaArchive"), Path.Combine(auftrag.Kundennummer.Value.ToString("0000000000"), auftrag.AuftragsNummer.ToString("0000000000")))
        If Not Directory.Exists(targetArchiv) Then
            Try
                Directory.CreateDirectory(targetArchiv)
            Catch ex As Exception
                lblError.Text = "Fehler beim Erstellen des Verzeichnisses<br/>" & ex.ToString
                Return
            End Try

        End If

        Dim fileList = New List(Of String)

        For Each file In auftrag.Files
            Try
                file.Archive(targetArchiv)
                fileList.Add(Path.Combine(targetArchiv, file.Filename))
            Catch ex As Exception
                lblError.Text = "Fehler beim Archivieren der Bilder<br/>" & ex.ToString
                Return
            End Try
        Next

        ' PDF herstellen
        Dim pdfFileName = "Bilder_" + auftrag.AuftragsNummer.ToString("0000000000") + "_" + auftrag.Fahrt.ToString("0000") + ".pdf"
        Try
            PdfDocumentFactory.CreatePdfFromImages(fileList, Path.Combine(targetArchiv, pdfFileName))
        Catch ex As Exception
            lblError.Text = "Fehler beim Erstellen des PDF-Dokuments <br/>" & ex.ToString
            Return
        End Try

        Try
            Dim ueberf = New Ueberfuehrung(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
            ueberf.AuftragAbschließen(auftrag)

            If Not String.IsNullOrEmpty(ueberf.Message) OrElse ueberf.Status <> 0 Then
                Throw New ApplicationException("Fehler beim Abschließen des Auftrags<br />" & ueberf.Message)
            End If
        Catch aex As ApplicationException
            lblError.Text = aex.Message
            Return
        Catch ex As Exception
            lblError.Text = "Fehler beim Abschließen des Auftrags<br/>" & ex.ToString
            Return
        End Try

        FillAuftragsListe()
        FillGridServer()
        SetFinished(False)
    End Sub

    Protected Sub BackClick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        SetFinished(False)
    End Sub

    Private Sub SetFinished(ByVal finished As Boolean)
        gridServer.Columns(2).Visible = Not finished
        gridServer.Columns(1).Visible = Not finished
        movePanel.Enabled = Not finished


        If (finished) Then
            lblPageTitle.Text = ": AUFTRAGSBESTÄTIGUNG"
        Else
            Dim selected = GetSelectedAuftrag()
            If Not selected Is Nothing Then
                lblPageTitle.Text = ": " & selected.ToString()
            Else
                lblPageTitle.Text = ": Aufträge bearbeiten"
            End If
        End If

        If finished Then lblError.Text = String.Empty

        btnFinish.Visible = Not finished
        btnConfirm.Visible = finished
        btnBack.Visible = finished
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class
