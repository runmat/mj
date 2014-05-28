Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common
Imports System.Data.OleDb
Imports System.Data

Public Class Change80
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

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private objHaendler As MDR_03

    Protected WithEvents Hyperlink1 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkLogout As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents upFile As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents lblExcelfile As System.Web.UI.WebControls.Label
    Protected WithEvents cmdContinue As System.Web.UI.WebControls.LinkButton
    Protected WithEvents tblUploadSelection As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents lblDateiname As System.Web.UI.WebControls.Label
    Protected WithEvents lblVerarbeitung_Datum As System.Web.UI.WebControls.Label
    Protected WithEvents tblProtocoll As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents lblVerarbeitung_Zeit As System.Web.UI.WebControls.Label
    Protected WithEvents lblGespeichert As System.Web.UI.WebControls.Label
    Protected WithEvents lblBereits_Angelegt As System.Web.UI.WebControls.Label
    Protected WithEvents lblUnbekannt_Abgewiesen As System.Web.UI.WebControls.Label
    Protected WithEvents lblKundeUnbekannt As System.Web.UI.WebControls.Label
    Protected WithEvents lblGelesen As System.Web.UI.WebControls.Label

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If (Session("objHaendler") Is Nothing) OrElse (Not IsPostBack) Then
                objHaendler = New MDR_03(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
            Else
                objHaendler = CType(Session("objHaendler"), MDR_03)
            End If

            Session("objHaendler") = objHaendler
            If Not IsPostBack Then
                tblUploadSelection.Visible = True
                tblProtocoll.Visible = False
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdContinue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdContinue.Click
        'Prüfe Fehlerbedingung
        If (Not upFile.PostedFile Is Nothing) AndAlso (Not (upFile.PostedFile.FileName = String.Empty)) Then
            lblExcelfile.Text = upFile.PostedFile.FileName
            If Right(upFile.PostedFile.FileName.ToUpper, 4) <> ".CSV" Then
                lblError.Text = "Es können nur Dateien im .CSV - Format verarbeitet werden."
                Exit Sub
            End If
        Else
            lblError.Text = "Keine Datei ausgewählt"
            Exit Sub
        End If

        'Lade Datei
        upload(upFile.PostedFile)
    End Sub

    Private Sub upload(ByVal uFile As System.Web.HttpPostedFile)
        Try
            Dim filepath As String = ConfigurationManager.AppSettings("UploadpathLocal")
            Dim filename As String
            Dim info As System.IO.FileInfo
            Dim streamreader As System.IO.StreamReader
            Dim strLine As String
            Dim rowData As DataRow

            'Dateiname: User_yyyyMMddhhmmss.csv
            'filename = m_User.UserName & "_" & Format(Now, "yyyyMMddhhmmss") & ".clv

            'Dateiname: Originalname
            filename = uFile.FileName
            Dim intSlashPosition As Integer = InStrRev(filename, "\")
            filename = Right(filename, Len(filename) - intSlashPosition)

            If Not (uFile Is Nothing) Then

                lblDateiname.Text = filename
                cmdContinue.Enabled = False
                tblUploadSelection.Visible = False
                tblProtocoll.Visible = True

                uFile.SaveAs(ConfigurationManager.AppSettings("UploadpathLocal") & filename)
                info = New System.IO.FileInfo(filepath & filename)
                If Not (info.Exists) Then
                    lblError.Text = "Fehler beim Speichern."
                    Exit Sub
                End If

                'Datei gespeichert -> Auswertung
                streamreader = New System.IO.StreamReader(filepath & filename)

                strLine = streamreader.ReadLine

                Do While Not strLine Is Nothing
                    rowData = objHaendler.FileInput.NewRow
                    rowData("I_Filetable_Data") = Left(strLine, 1024)
                    objHaendler.FileInput.Rows.Add(rowData)

                    strLine = streamreader.ReadLine
                Loop

                streamreader.Close()


                Dim i As Integer = 0

                Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
                logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
                Session.Add("logObj", logApp)


                If Not objHaendler.FileInput.Rows Is Nothing AndAlso objHaendler.FileInput.Rows.Count > 0 Then
                    objHaendler.Dateiname = filename
                    objHaendler.Change(Session("AppID").ToString, Session.SessionID.ToString, Me)

                    If Not objHaendler.Status = 0 Then
                        lblError.Text = objHaendler.Message
                        lblError.Visible = True
                    Else
                        lblBereits_Angelegt.Text = objHaendler.Bereits_Angelegt
                        lblKundeUnbekannt.Text = objHaendler.KundeUnbekannt
                        lblGelesen.Text = objHaendler.Gelesen
                        lblGespeichert.Text = objHaendler.Gespeichert
                        lblUnbekannt_Abgewiesen.Text = objHaendler.Unbekannt_Abgewiesen
                        lblVerarbeitung_Datum.Text = objHaendler.Verarbeitung_Datum.ToShortDateString
                        lblVerarbeitung_Zeit.Text = objHaendler.Verarbeitung_Zeit.ToShortTimeString
                    End If
                Else
                    lblError.Text = "Datei enthielt keine verwendbaren Daten."
                End If

                Session("objHaendler") = Nothing
            End If
        Catch ex As Exception
            lblError.Text = ex.Message
        Finally
        End Try
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Change80.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 26.06.09   Time: 10:33
' Updated in $/CKAG/Applications/appffd/Forms
' ITA: 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 30.04.09   Time: 14:43
' Updated in $/CKAG/Applications/appffd/Forms
' ITA: 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:23
' Created in $/CKAG/Applications/appffd/Forms
' 
' *****************  Version 2  *****************
' User: Uha          Date: 29.08.07   Time: 10:05
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' ITA 1224: Neue Ergebnisspalte "Kunde_Unbekannt" hinzugefügt
' 
' *****************  Version 1  *****************
' User: Uha          Date: 15.08.07   Time: 16:18
' Created in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' ITA 1224: "Hinterlegung ALM-Daten" (Change80) hinzugefügt
' 
' ************************************************
