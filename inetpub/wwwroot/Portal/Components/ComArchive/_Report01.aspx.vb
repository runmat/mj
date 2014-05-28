Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common
Imports CKG.EasyAccess

<CLSCompliant(False)> Public Class _Report01
    Inherits System.Web.UI.Page

    Protected WithEvents ucStyles As Styles
    'Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents lblScript As System.Web.UI.WebControls.Label
    Protected WithEvents btnBearb As System.Web.UI.WebControls.LinkButton

    'Private zulDaten As Zulassung
    Private m_App As Security.App
    Private m_User As Security.User
    Private showCheckbox As Boolean
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents Table10 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tdSearch As System.Web.UI.HtmlControls.HtmlTableCell
    'Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Private strDocument As String
    Protected WithEvents txtLAenge As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtErstellDatum As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAenderDatum As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtTitel As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtFelderGesamt As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtFelderText As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtFelderBild As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtFelderBlob As System.Web.UI.WebControls.TextBox
    Protected WithEvents Form1 As System.Web.UI.HtmlControls.HtmlForm
    Protected WithEvents lnkFahrzeugsuche As System.Web.UI.WebControls.HyperLink

    Private appl As String

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
        FormAuthNoReferrer(Me, m_User)

        strDocument = Request.QueryString("I")
        If (strDocument Is Nothing) OrElse (strDocument = String.Empty) Then
            lblError.Text = "Kein Dokument gefunden!"
            Exit Sub
        End If

        'lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        'ucStyles.TitleText = lblHead.Text

        Try
            m_App = New Security.App(m_User)
            'Literal1.Text = String.Empty

            If Not IsPostBack Then
                'Dim logApp As New Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
                'logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
                'logApp.UpdateEntry("APP", Session("AppID").ToString, "Report: Optisches Archiv")
                loadForm(strDocument)
            Else
                'searchfields = CType(Session("EasySearchFields"), ArrayList)
                'addSearchFields(searchfields)
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub loadForm(ByVal strDocument As String)
        Dim easy As EasyAccess.EasyAccess
        'Dim archives As EasyAccess.EasyArchive = easy.getArchives()
        Dim ids As String()
        Dim strArcLoc As String
        Dim strArcNam As String
        Dim strDocID As String
        Dim strDocVer As String
        'Rückgabeparameter
        Dim lngLaenge As Long
        Dim strErstellDat As String = ""
        Dim strAenderDat As String = ""
        Dim strTitel As String = ""
        Dim intFelderGes As Integer
        Dim intTextFelder As Integer
        Dim intBildFelder As Integer
        Dim intBlobFelder As Integer
        Dim status As String = ""

        easy = CType(Session("EasyAccess"), EasyAccess.EasyAccess)

        Try
            ids = strDocument.Split("."c)
            strArcLoc = ids(0)
            strArcNam = ids(1)
            strDocID = ids(2)
            strDocVer = ids(3)

            easy.getDocumentInfo(strArcLoc, strArcNam, strDocID, strDocVer, lngLaenge, strErstellDat, strAenderDat, strTitel, intFelderGes, intTextFelder, intBildFelder, intBlobFelder, status)

            If (status <> String.Empty) Then
                lblError.Text = "Fehler beim Laden des Dokumentes."
            Else
                txtAenderDatum.Text = strAenderDat
                txtErstellDatum.Text = strErstellDat
                txtFelderBild.Text = intBildFelder.ToString
                txtFelderBlob.Text = intBlobFelder.ToString
                txtFelderGesamt.Text = intFelderGes.ToString
                txtFelderText.Text = intTextFelder.ToString
                txtLAenge.Text = lngLaenge.ToString
                txtTitel.Text = strTitel
            End If

        Catch ex As Exception
            lblError.Text = "Fehler beim Laden der Daten."
            Exit Sub
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
' $History: _Report01.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 4.05.09    Time: 11:22
' Updated in $/CKAG/Components/ComArchive
' ITA:2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:29
' Created in $/CKAG/Components/ComArchive
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:03
' Created in $/CKAG/Components/ComArchive/inetpub/wwwroot/Portal/Components/ComArchive
' 
' *****************  Version 5  *****************
' User: Uha          Date: 7.08.07    Time: 14:23
' Updated in $/CKG/Components/ComArchive/ComArchiveWeb
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 4  *****************
' User: Uha          Date: 5.03.07    Time: 17:21
' Updated in $/CKG/Components/ComArchive/ComArchiveWeb
' 
' ************************************************
