Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Change09Edit
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
    Private m_objCSCBrief As CSC_Briefkopien

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
        lnk.NavigateUrl = "Change09.aspx?AppID=" & CStr(Request.QueryString("AppID"))
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & CStr(Request.QueryString("AppID")) & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If Not Session("m_objCSCBrief") Is Nothing Then
                m_objCSCBrief = CType(Session("m_objCSCBrief"), CSC_Briefkopien)
                If Not IsPostBack Then
                    txtFahrgestellnummer.Text = m_objCSCBrief.Fahrgestellnummer
                    txtVertragsnummer.Text = m_objCSCBrief.Kontonummer
                    txtKennzeichen.Text = m_objCSCBrief.Kennzeichen
                    txtBriefNr.Text = m_objCSCBrief.BriefNr
                    txtLabel.Text = m_objCSCBrief.Label
                    txtModell.Text = m_objCSCBrief.Modell
                    cbxCOCBescheinigungVorhanden.Checked = m_objCSCBrief.COCBescheinigungVorhanden
                    'If m_objCSCBrief.Versand > CDate("01.01.0001") Then
                    txtVersanddatum.Text = m_objCSCBrief.Versand
                    'Else
                    '    txtVersanddatum.Text = ""
                    'End If
                End If
            Else
                Response.Redirect("../Start/Selection.aspx")
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdConfirm.Click
        Try
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

            If m_objCSCBrief.Versand = "" Then
                m_objCSCBrief.Faxnummer = txtFaxnummer.Text
                m_objCSCBrief.Mailadresse = txtMailadresse.Text
                m_objCSCBrief.Zusatz = cbxZusatz.Checked
                m_objCSCBrief.Change(Session("AppID").ToString, Session.SessionID.ToString, Me)

                If m_objCSCBrief.Message.Length > 0 Then
                    lblError.Text = m_objCSCBrief.Message
                    logApp.UpdateEntry("ERR", Session("AppID").ToString, "Fehler beim Anforderm der Briefkopie für Vertragsnummer " & txtVertragsnummer.Text & " (" & lblError.Text & ").", logApp.InputDetails)
                    'logApp.WriteEntry("ERR", m_User.UserName, Session.SessionID, CInt(Request.QueryString("AppID")), m_User.Applications.Select("AppID = '" & CStr(Request.QueryString("AppID")) & "'")(0)("AppFriendlyName").ToString, txtFahrgestellnummer.Text, "Fehler beim Anforderm der Briefkopie für Vertragsnummer " & txtVertragsnummer.Text & " (" & lblError.Text & ").", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0, logApp.InputDetails)
                    lblError.Visible = True
                Else
                    logApp.UpdateEntry("APP", Session("AppID").ToString, "Briefkopie angefordert für Vertragsnummer " & txtVertragsnummer.Text, logApp.InputDetails)
                    'logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Request.QueryString("AppID")), m_User.Applications.Select("AppID = '" & CStr(Request.QueryString("AppID")) & "'")(0)("AppFriendlyName").ToString, txtFahrgestellnummer.Text, "Briefkopie angefordert für Vertragsnummer " & txtVertragsnummer.Text, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0, logApp.InputDetails)
                    lblError.Text = "Die Briekkopie wird im Archiv zur Verfügung gestellt."
                    lblError.Visible = True
                    txtFaxnummer.Enabled = False
                    txtMailadresse.Enabled = False
                    cbxZusatz.Enabled = False
                    cmdConfirm.Visible = False
                End If

                logApp.WriteStandardDataAccessSAP(m_objCSCBrief.IDSAP)
            Else
                lblError.Text = "Die Briekkopie kann zur Zeit nicht zur Verfügung gestellt werden, da der Brief versand wurde."
                logApp.UpdateEntry("ERR", Session("AppID").ToString, "Fehler beim Anforderm der Briefkopie für Vertragsnummer " & txtVertragsnummer.Text & " (" & lblError.Text & ").", logApp.InputDetails)
                'logApp.WriteEntry("ERR", m_User.UserName, Session.SessionID, CInt(Request.QueryString("AppID")), m_User.Applications.Select("AppID = '" & CStr(Request.QueryString("AppID")) & "'")(0)("AppFriendlyName").ToString, txtFahrgestellnummer.Text, "Fehler beim Anforderm der Briefkopie für Vertragsnummer " & txtVertragsnummer.Text & " (" & lblError.Text & ").", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0, logApp.InputDetails)
                lblError.Visible = True
                txtFaxnummer.Enabled = False
                txtMailadresse.Enabled = False
                cbxZusatz.Enabled = False
                cmdConfirm.Visible = False
            End If

        Catch ex As Exception
            lblError.Text = ex.Message
            lblError.Visible = True
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
' $History: Change09Edit.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 18.03.10   Time: 12:51
' Updated in $/CKAG/Applications/appcsc/Forms
' Bugfix DynProxyUmstellung
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
