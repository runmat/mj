Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Change10Edit
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
    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private m_intLineCount As Int32
    Private m_objCSCEinzel As CSC_Einzelvorgaenge

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents Hyperlink2 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents ShowScript As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents txtFahrgestellnummer As System.Web.UI.WebControls.Label
    Protected WithEvents txtVertragsnummer As System.Web.UI.WebControls.Label
    Protected WithEvents txtKennzeichen As System.Web.UI.WebControls.Label
    Protected WithEvents txtBriefNr As System.Web.UI.WebControls.Label
    Protected WithEvents txtLabel As System.Web.UI.WebControls.Label
    Protected WithEvents txtModell As System.Web.UI.WebControls.Label
    Protected WithEvents txtErsteingang As System.Web.UI.WebControls.Label
    Protected WithEvents txtVersand As System.Web.UI.WebControls.Label
    Protected WithEvents txtWiedereingang As System.Web.UI.WebControls.Label
    Protected WithEvents txtNochmaligerVersand As System.Web.UI.WebControls.Label
    Protected WithEvents cbxGesperrt As System.Web.UI.WebControls.CheckBox
    Protected WithEvents cmdConfirm As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdCancel As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cbxCOCBescheinigungVorhanden As System.Web.UI.WebControls.CheckBox
    Protected WithEvents lnk As System.Web.UI.WebControls.HyperLink
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)
        lnk.NavigateUrl = "Change10.aspx?AppID=" & CStr(Request.QueryString("AppID"))
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & CStr(Request.QueryString("AppID")) & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If Not Session("m_objCSCEinzel") Is Nothing Then
                m_objCSCEinzel = CType(Session("m_objCSCEinzel"), CSC_Einzelvorgaenge)
                If Not IsPostBack Then
                    txtFahrgestellnummer.Text = m_objCSCEinzel.Fahrgestellnummer
                    txtVertragsnummer.Text = m_objCSCEinzel.Kontonummer
                    txtKennzeichen.Text = m_objCSCEinzel.Kennzeichen
                    txtBriefNr.Text = m_objCSCEinzel.BriefNr
                    txtLabel.Text = m_objCSCEinzel.Label
                    txtModell.Text = m_objCSCEinzel.Modell
                    If m_objCSCEinzel.Ersteingang > CDate("01.01.1900") Then txtErsteingang.Text = m_objCSCEinzel.Ersteingang.ToShortDateString
                    If m_objCSCEinzel.Versand > CDate("01.01.1900") Then txtVersand.Text = m_objCSCEinzel.Versand.ToShortDateString
                    If m_objCSCEinzel.Wiedereingang1 > CDate("01.01.1900") Then txtWiedereingang.Text = m_objCSCEinzel.Wiedereingang1.ToShortDateString
                    If m_objCSCEinzel.Wiedereingang2 > CDate("01.01.1900") Then txtWiedereingang.Text &= "&nbsp;&nbsp;&nbsp;" & m_objCSCEinzel.Wiedereingang2.ToShortDateString
                    If m_objCSCEinzel.Wiedereingang3 > CDate("01.01.1900") Then txtWiedereingang.Text &= "&nbsp;&nbsp;&nbsp;" & m_objCSCEinzel.Wiedereingang3.ToShortDateString
                    If m_objCSCEinzel.NochmaligerVersand1 > CDate("01.01.1900") Then txtNochmaligerVersand.Text = m_objCSCEinzel.NochmaligerVersand1.ToShortDateString
                    If m_objCSCEinzel.NochmaligerVersand2 > CDate("01.01.1900") Then txtNochmaligerVersand.Text &= "&nbsp;&nbsp;&nbsp;" & m_objCSCEinzel.NochmaligerVersand2.ToShortDateString
                    If m_objCSCEinzel.NochmaligerVersand3 > CDate("01.01.1900") Then txtNochmaligerVersand.Text &= "&nbsp;&nbsp;&nbsp;" & m_objCSCEinzel.NochmaligerVersand3.ToShortDateString
                    cbxGesperrt.Checked = m_objCSCEinzel.Gesperrt
                    cbxCOCBescheinigungVorhanden.Checked = m_objCSCEinzel.COCBescheinigungVorhanden
                End If
            Else
                Response.Redirect("../Start/Selection.aspx")
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        cbxGesperrt.Enabled = False
        cmdSave.Visible = False
        cmdConfirm.Visible = True
        cmdCancel.Visible = True
    End Sub

    Private Sub cmdConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdConfirm.Click
        Try

            Dim strKontonummerAlt As String = txtVertragsnummer.Text
            Dim blnGesperrtAlt As Boolean = m_objCSCEinzel.Gesperrt

            Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
            logApp.CollectDetails("Kennzeichen", CType(txtKennzeichen.Text, Object), True)
            logApp.CollectDetails("Vertragsnummer", CType(txtVertragsnummer.Text, Object))
            logApp.CollectDetails("Fahrgestellnummer", CType(txtFahrgestellnummer.Text, Object))
            logApp.CollectDetails("Briefnummer", CType(txtBriefNr.Text, Object))
            logApp.CollectDetails("Label", CType(txtLabel.Text, Object))
            logApp.CollectDetails("Modell", CType(txtModell.Text, Object))
            logApp.CollectDetails("Ersteingang", CType(txtErsteingang.Text, Object))
            logApp.CollectDetails("Versand", CType(txtVersand.Text, Object))
            logApp.CollectDetails("Wiedereingang", CType(Replace(txtWiedereingang.Text, "&nbsp;&nbsp;&nbsp;", "/"), Object))
            logApp.CollectDetails("Nochmaliger Versand", CType(Replace(txtNochmaligerVersand.Text, "&nbsp;&nbsp;&nbsp;", "/"), Object))
            logApp.CollectDetails("Gesperrt", CType(cbxGesperrt.Checked, Object))

            m_objCSCEinzel.Kontonummer = ""
            m_objCSCEinzel.Gesperrt = cbxGesperrt.Checked
            m_objCSCEinzel.Change(Session("AppID").ToString, Session.SessionID, Me, strKontonummerAlt)

            Dim strKontonummerNeu As String = m_objCSCEinzel.Kontonummer
            Dim blnGesperrtNeu As Boolean = m_objCSCEinzel.Gesperrt
            Dim strAusgabe As String
            If (Not strKontonummerAlt = strKontonummerNeu) AndAlso strKontonummerNeu.Length > 0 Then
                strAusgabe = "Kontonummer geändert: " & strKontonummerAlt & " (alt) -> " & strKontonummerNeu & " (neu)"
            Else
                strAusgabe = "Kontonummer: " & strKontonummerAlt
            End If
            If blnGesperrtAlt = blnGesperrtNeu Then
                If blnGesperrtAlt Then
                    strAusgabe &= ", Gesperrt"
                Else
                    strAusgabe &= ", Nicht gesperrt"
                End If
            Else
                strAusgabe &= ", Sperre geändert: "
                If blnGesperrtAlt Then
                    strAusgabe &= " Gesperrt (alt) -> "
                Else
                    strAusgabe &= " Nicht gesperrt (alt) -> "
                End If
                If blnGesperrtNeu Then
                    strAusgabe &= " Gesperrt (neu)"
                Else
                    strAusgabe &= " Nicht gesperrt (neu)"
                End If
            End If

            If m_objCSCEinzel.Message.Length > 0 Then
                logApp.WriteEntry("ERR", m_User.UserName, Session.SessionID, CInt(Request.QueryString("AppID")), m_User.Applications.Select("AppID = '" & CStr(Request.QueryString("AppID")) & "'")(0)("AppFriendlyName").ToString, txtFahrgestellnummer.Text, "Fehler: " & m_objCSCEinzel.Message & ",Vorgang: " & strAusgabe, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0, logApp.InputDetails)
                lblError.Text = m_objCSCEinzel.Message
                lblError.Visible = True
            Else
                logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Request.QueryString("AppID")), m_User.Applications.Select("AppID = '" & CStr(Request.QueryString("AppID")) & "'")(0)("AppFriendlyName").ToString, txtFahrgestellnummer.Text, strAusgabe, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0, logApp.InputDetails)
                lblError.Text = "Die Änderungen wurden übernommen."
                lblError.Visible = True
                txtVertragsnummer.Enabled = False
                cbxGesperrt.Enabled = False
                cmdSave.Visible = False
                cmdConfirm.Visible = False
                cmdCancel.Visible = False
            End If

            logApp.WriteStandardDataAccessSAP(m_objCSCEinzel.IDSAP)
        Catch ex As Exception
            lblError.Text = ex.Message
            lblError.Visible = True
        End Try
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        txtVertragsnummer.Enabled = True
        cbxGesperrt.Enabled = True
        cmdSave.Visible = True
        cmdConfirm.Visible = False
        cmdCancel.Visible = False
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Change10Edit.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 31.08.10   Time: 16:43
' Updated in $/CKAG/Applications/appcsc/Forms
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 11.03.10   Time: 13:34
' Updated in $/CKAG/Applications/appcsc/Forms
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 11:37
' Created in $/CKAG/Applications/appcsc/Forms
' 
' *****************  Version 5  *****************
' User: Uha          Date: 2.07.07    Time: 10:42
' Updated in $/CKG/Applications/AppCSC/AppCSCWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 4  *****************
' User: Uha          Date: 7.03.07    Time: 9:28
' Updated in $/CKG/Applications/AppCSC/AppCSCWeb/Forms
' 
' ************************************************
