Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Change09
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
    Private m_objCSCBrief As CSC_Briefkopien
    Private AppID As String

    Protected WithEvents Hyperlink1 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkLogout As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents txtVertragsnummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtKennzeichen As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.LinkButton
    Protected WithEvents txtFahrgestellnummer As System.Web.UI.WebControls.TextBox

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & CStr(Request.QueryString("AppID")) & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        DoSubmit()
    End Sub

    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        lblError.Text = ""
        lblError.Visible = False
        AppID = Request.QueryString("AppID").ToString

        Try
            'm_objCSCBrief = New FDD_Bank.CSC_Briefkopien(m_User, m_App, txtKennzeichen.Text, txtVertragsnummer.Text, txtFahrgestellnummer.Text, Session("AppID").ToString, Session.SessionID.ToString, "")
            m_objCSCBrief = New CSC_Briefkopien(m_User, m_App, txtKennzeichen.Text, txtVertragsnummer.Text, txtFahrgestellnummer.Text, AppID, Session.SessionID.ToString, "")
            If TypeOf Session("InaktiveVertraege") Is String Then
                m_objCSCBrief.Customer = CStr(Session("InaktiveVertraege"))
            Else
                m_objCSCBrief.Customer = m_User.KUNNR
            End If

            m_objCSCBrief.Show(Session("AppID").ToString, Session.SessionID.ToString, Me)

            If m_objCSCBrief.Fahrgestellnummer = String.Empty Then
                lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                lblError.Visible = True
                If m_objCSCBrief.Message.Length > 0 Then
                    lblError.Text &= vbCrLf + m_objCSCBrief.Message
                    lblError.Visible = True
                End If
            Else
                Session.Add("m_objCSCBrief", m_objCSCBrief)
                Response.Redirect("Change09Edit.aspx?AppID=" & CStr(Request.QueryString("AppID")))

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
' $History: Change09.aspx.vb $
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
