Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Report11
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
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents cmdCreate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents txtAmtlKennzeichen As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtFahrgestellnummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBriefnummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtOrdernummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header

    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)


        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text
        m_App = New Base.Kernel.Security.App(m_User)

        'DoSubmit()
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        Session("ShowLink") = "True"
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        Dim m_Report As New Report_011(m_User, m_App, "")

        Dim strBriefnummer As String
        Dim strFahrgestellnummer As String
        Dim strAmtlKennzeichen As String
        Dim strOrdernummer As String

        If txtOrdernummer.Text.Length = 0 Then
            strOrdernummer = ""
        Else
            strOrdernummer = txtOrdernummer.Text
        End If
        If txtBriefnummer.Text.Length = 0 Then
            strBriefnummer = ""
        Else
            strBriefnummer = txtBriefnummer.Text
        End If
        If txtFahrgestellnummer.Text.Length = 0 Then
            strFahrgestellnummer = ""
        Else
            strFahrgestellnummer = txtFahrgestellnummer.Text
        End If
        If txtAmtlKennzeichen.Text.Length = 0 Then
            strAmtlKennzeichen = ""
        Else
            strAmtlKennzeichen = txtAmtlKennzeichen.Text
        End If

        If txtBriefnummer.Text.Length + txtFahrgestellnummer.Text.Length + txtAmtlKennzeichen.Text.Length + txtOrdernummer.Text.Length = 0 Then
            lblError.Text = "Bitte geben Sie ein Suchkriterium ein."
        Else
            m_Report.FillHistory(Me.Page, Session("AppID").ToString, Session.SessionID.ToString, strAmtlKennzeichen, strFahrgestellnummer, strBriefnummer, strOrdernummer)

            Session("ResultTable") = m_Report.History

        End If

        If Not m_Report.Status = 0 Then
            lblError.Text = m_Report.Message
        Else
            If (m_Report.History Is Nothing) OrElse (m_Report.History.Rows.Count = 0) Then
                lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
            Else
                Response.Redirect("Report11_2.aspx?AppID=" & Session("AppID").ToString)
            End If
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
' $History: Report11.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 9.03.10    Time: 10:27
' Updated in $/CKAG/Components/ComCommon
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 5.06.09    Time: 15:58
' Updated in $/CKAG/Components/ComCommon
' mögliche try catches entfernt
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 10.04.08   Time: 17:37
' Updated in $/CKAG/Components/ComCommon
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:32
' Created in $/CKAG/Components/ComCommon
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 10:47
' Created in $/CKAG/portal/Components/ComCommon
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 5.10.07    Time: 10:46
' Created in $/CKG/Components/ComCommon/ComCommonWeb
' 

