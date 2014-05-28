Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Change49_1
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
    Private m_change As fin_17

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
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
    Protected WithEvents cmdConfirm As System.Web.UI.WebControls.LinkButton
    Protected WithEvents txtVersanddatum As System.Web.UI.WebControls.Label
    Protected WithEvents txtFaxnummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtMailadresse As System.Web.UI.WebControls.TextBox
    Protected WithEvents cbxZusatz As System.Web.UI.WebControls.CheckBox
    Protected WithEvents rowFaxnummer As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents rowMailadresse As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents rowZusatz As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents cbxCOCBescheinigungVorhanden As System.Web.UI.WebControls.CheckBox
    Protected WithEvents lnk As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lbl_Briefnummer As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Mandant As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Ausblenden Eingabefelder
        rowFaxnummer.Visible = False
        rowMailadresse.Visible = False
        rowZusatz.Visible = False

        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)
        lnk.NavigateUrl = "Change49.aspx?AppID=" & CStr(Request.QueryString("AppID"))

        lblHead.Text = m_User.Applications.Select("AppID = '" & CStr(Request.QueryString("AppID")) & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text
        m_App = New Base.Kernel.Security.App(m_User)

        If Not Session("m_change") Is Nothing Then
            m_change = CType(Session("m_change"), fin_17)
            If Not IsPostBack Then
                txtFahrgestellnummer.Text = m_change.Fahrgestellnummer
                txtVertragsnummer.Text = m_change.Kontonummer
                txtKennzeichen.Text = m_change.Kennzeichen
                txtBriefNr.Text = m_change.BriefNr
                txtLabel.Text = m_change.Label
                txtModell.Text = m_change.Modell
                cbxCOCBescheinigungVorhanden.Checked = m_change.COCBescheinigungVorhanden
                If m_change.Versand > CDate("01.01.1900") Then
                    txtVersanddatum.Text = m_change.Versand.ToShortDateString
                Else
                    txtVersanddatum.Text = ""
                End If
            End If
        Else
            Response.Redirect("../Start/Selection.aspx")
        End If
      
    End Sub

    Private Sub cmdConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdConfirm.Click
        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        logApp.CollectDetails("Vertragsnummer", CType(txtVertragsnummer.Text, Object), True)
        logApp.CollectDetails("Fahrgestellnummer", CType(txtFahrgestellnummer.Text, Object))
        logApp.CollectDetails("Kennzeichen", CType(txtKennzeichen.Text, Object))
        logApp.CollectDetails("Briefnummer", CType(txtBriefNr.Text, Object))
        logApp.CollectDetails("Label", CType(txtLabel.Text, Object))
        logApp.CollectDetails("Modell", CType(txtModell.Text, Object))
        logApp.CollectDetails("Versanddatum", CType(txtVersanddatum.Text, Object))
        logApp.CollectDetails("CoC vorhanden", CType(cbxCOCBescheinigungVorhanden.Checked, Object))
        logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)

        If m_change.Versand.Year < 1950 Then
            m_change.Faxnummer = txtFaxnummer.Text
            m_change.Mailadresse = txtMailadresse.Text
            m_change.Zusatz = cbxZusatz.Checked
            m_change.Change()

            If m_change.Message.Length > 0 Then
                lblError.Text = m_change.Message
                logApp.UpdateEntry("ERR", Session("AppID").ToString, "Fehler beim Anforderm der Briefkopie für Vertragsnummer " & txtVertragsnummer.Text & " (" & lblError.Text & ").", logApp.InputDetails)
                'logApp.WriteEntry("ERR", m_User.UserName, Session.SessionID, CInt(Request.QueryString("AppID")), m_User.Applications.Select("AppID = '" & CStr(Request.QueryString("AppID")) & "'")(0)("AppFriendlyName").ToString, txtFahrgestellnummer.Text, "Fehler beim Anforderm der Briefkopie für Vertragsnummer " & txtVertragsnummer.Text & " (" & lblError.Text & ").", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0, logApp.InputDetails)
                lblError.Visible = True
            Else
                logApp.UpdateEntry("APP", Session("AppID").ToString, "Briefkopie angefordert für Vertragsnummer " & txtVertragsnummer.Text, logApp.InputDetails)
                'logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Request.QueryString("AppID")), m_User.Applications.Select("AppID = '" & CStr(Request.QueryString("AppID")) & "'")(0)("AppFriendlyName").ToString, txtFahrgestellnummer.Text, "Briefkopie angefordert für Vertragsnummer " & txtVertragsnummer.Text, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0, logApp.InputDetails)
                lblError.Text = "Dokument wird innerhalb von 2 Stunden im optischen Archiv zur Verfügung gestellt"
                lblError.Visible = True
                txtFaxnummer.Enabled = False
                txtMailadresse.Enabled = False
                cbxZusatz.Enabled = False
                cmdConfirm.Visible = False
            End If

            logApp.WriteStandardDataAccessSAP(m_change.IDSAP)
        Else
            lblError.Text = "Das Dokument kann zur Zeit nicht zur Verfügung gestellt werden, da der Brief versand wurde."
            logApp.UpdateEntry("ERR", Session("AppID").ToString, "Fehler beim Anforderm der Briefkopie für Vertragsnummer " & txtVertragsnummer.Text & " (" & lblError.Text & ").", logApp.InputDetails)
            'logApp.WriteEntry("ERR", m_User.UserName, Session.SessionID, CInt(Request.QueryString("AppID")), m_User.Applications.Select("AppID = '" & CStr(Request.QueryString("AppID")) & "'")(0)("AppFriendlyName").ToString, txtFahrgestellnummer.Text, "Fehler beim Anforderm der Briefkopie für Vertragsnummer " & txtVertragsnummer.Text & " (" & lblError.Text & ").", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0, logApp.InputDetails)
            lblError.Visible = True
            txtFaxnummer.Enabled = False
            txtMailadresse.Enabled = False
            cbxZusatz.Enabled = False
            cmdConfirm.Visible = False
        End If


    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Change49_1.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Dittbernerc  Date: 19.06.09   Time: 15:49
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA - 2918 .Net Connector Umstellung
' 
' Bapis:
' Z_M_Brief_Ohne_Daten
' Z_M_Daten_Einz_Report_001
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 5.06.09    Time: 15:58
' Updated in $/CKAG/Components/ComCommon/Finance
' mögliche try catches entfernt
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:32
' Created in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 10:48
' Created in $/CKAG/portal/Components/ComCommon/Finance
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 18.02.08   Time: 13:47
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' akf änderungen
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 4.02.08    Time: 16:56
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA: 1677
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 1.02.08    Time: 14:33
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' AKF Änderungen
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 10.01.08   Time: 12:50
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1482 Torso
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 10.01.08   Time: 10:36
' Created in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' 
' ************************************************
