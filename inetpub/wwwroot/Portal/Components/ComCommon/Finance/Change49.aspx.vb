Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Change49
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
    Private m_change As fin_17
    Private AppID As String

    Protected WithEvents Hyperlink1 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkLogout As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents txtVertragsnummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtKennzeichen As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbl_Kennzeichen As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Kontonummer As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Fahrgestellnummer As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents txtFahrgestellnummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtzb2Nummer As System.Web.UI.WebControls.TextBox


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        lblHead.Text = m_User.Applications.Select("AppID = '" & CStr(Request.QueryString("AppID")) & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text
        m_App = New Base.Kernel.Security.App(m_User)

        If IsPostBack = False Then
            If Not Request.QueryString("FIN") Is Nothing AndAlso Not Request.QueryString("FIN") Is String.Empty Then
                txtFahrgestellnummer.Text = Request.QueryString("FIN")
                DoSubmit()
            End If
        End If

    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        DoSubmit()
    End Sub

    'Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    '    DoSubmit()
    'End Sub

    Private Sub DoSubmit()
        lblError.Text = ""
        lblError.Visible = False
        AppID = Request.QueryString("AppID").ToString
        
        m_change = New fin_17(m_User, m_App, txtKennzeichen.Text, txtVertragsnummer.Text, txtFahrgestellnummer.Text.Trim, AppID, Session.SessionID.ToString, "")
        m_change.ZB2Nummer = txtzb2Nummer.Text
        If TypeOf Session("InaktiveVertraege") Is String Then
            m_change.Customer = CStr(Session("InaktiveVertraege"))
        Else
            m_change.Customer = m_User.KUNNR
        End If

        m_change.Show()

        If m_change.Fahrgestellnummer = String.Empty Then
            lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
            lblError.Visible = True
            If m_change.Message.Length > 0 Then
                lblError.Text &= vbCrLf + m_change.Message
                lblError.Visible = True
            End If
        Else
            Session.Add("m_change", m_change)
            Response.Redirect("Change49_1.aspx?AppID=" & CStr(Request.QueryString("AppID")))
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
' $History: Change49.aspx.vb $
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
' *****************  Version 4  *****************
' User: Jungj        Date: 12.02.08   Time: 13:23
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' akf änderungen
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
