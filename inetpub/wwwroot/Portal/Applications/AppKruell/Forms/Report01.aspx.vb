Option Explicit On 
Option Strict On


Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business

Public Class Report01
    Inherits System.Web.UI.Page
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lb_Suche As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents trCreate As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents txt_Fahrgestellnummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_Ordernummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Treffer As System.Web.UI.WebControls.Label
    Protected WithEvents lb_GeheZu As System.Web.UI.WebControls.LinkButton
    Protected WithEvents Tr1 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents rblAuswahl As System.Web.UI.WebControls.RadioButtonList
    Dim m_report As kruell_03
    Dim strErrorText As String
    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App


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
        Try
            m_User = GetUser(Me) ' füllen Form.Session("objUser"), rückgabe eines UserObjekte 
            m_App = New Base.Kernel.Security.App(m_User) 'erzeugt ein App_objekt 

            FormAuth(Me, m_User)
            ucHeader.InitUser(m_User)
            GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text

            lbl_Treffer.Text = "0"
            If Not IsPostBack Then
                m_report = New kruell_03(m_User, m_App)
                m_report.SessionID = Me.Session.SessionID
                m_report.AppID = CStr(Session("AppID"))
                Session.Add("objReport", m_report)
            End If
        Catch ex As Exception
            lblError.Text = "Fehler beim laden der Seite: " & ex.Message.ToString
        End Try
    End Sub


    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub lb_Suche_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_Suche.Click
        If m_report Is Nothing Then
            m_report = CType(Session("objReport"), kruell_03)
        End If

        'Selektionsfelder leeren im report, wenn öfters gesucht wird, bleibend die alten werte bestehen
        m_report.ohneEingangKMC = ""
        m_report.mitEingangKMC = ""
        m_report.mitStoerungsmeldung = ""
        m_report.ordernummer = ""
        m_report.fahrgestellnummer = ""


        'ordernummer/fahrgestellnummer steht über rb selektion
        If txt_Fahrgestellnummer.Text.Trim Is String.Empty AndAlso txt_Ordernummer.Text.Trim Is String.Empty Then
            Select Case rblAuswahl.SelectedIndex
                Case 0
                    m_report.ohneEingangKMC = "X"
                Case 1
                    m_report.mitEingangKMC = "X"
                Case 2
                    m_report.mitStoerungsmeldung = "X"
            End Select
        Else
            m_report.selOrdernummer = txt_Ordernummer.Text
            m_report.selFahrgestellnummer = txt_Fahrgestellnummer.Text
        End If
        Try
            m_report.Fill(Session("AppID").ToString, Session.SessionID.ToString, Me)
            lbl_Treffer.Text = CStr(m_report.Result.Rows.Count)
        Catch
            lblError.Text = m_report.Message
            Exit Sub
        End Try
    End Sub

    Private Sub lb_GeheZu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_GeheZu.Click
        Response.Redirect("Report01_1.aspx?AppID=" & Session("AppID").ToString)
    End Sub
End Class
' ************************************************
' $History: Report01.aspx.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 14:37
' Created in $/CKAG/Applications/AppKruell/Forms
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 23.01.08   Time: 14:02
' Updated in $/CKG/Applications/AppKruell/AppKruellWeb/Forms
' testfertig ITA 1580
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 15.01.08   Time: 15:15
' Updated in $/CKG/Applications/AppKruell/AppKruellWeb/Forms
' in bearbeitung
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 15.01.08   Time: 11:46
' Updated in $/CKG/Applications/AppKruell/AppKruellWeb/Forms
' Script erzeugen?
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 15.01.08   Time: 10:46
' Updated in $/CKG/Applications/AppKruell/AppKruellWeb/Forms
' ITA 1580, in bearbeitung
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 14.01.08   Time: 17:11
' Updated in $/CKG/Applications/AppKruell/AppKruellWeb/Forms
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 9.01.08    Time: 14:29
' Updated in $/CKG/Applications/AppKruell/AppKruellWeb/Forms
' ITA 1580 Torso
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 9.01.08    Time: 13:57
' Created in $/CKG/Applications/AppKruell/AppKruellWeb/Forms
' ITA 1580 Report01 hinzugefügt, SS History Bodys hinzugefügt
' ************************************************

