Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Change50
    Inherits System.Web.UI.Page

    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label

    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.LinkButton
    Protected WithEvents rblSearch As System.Web.UI.WebControls.RadioButtonList

    Dim m_change As fin_18


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

        m_User = GetUser(Me) ' füllen Form.Session("objUser"), rückgabe eines UserObjekte
        FormAuth(Me, m_User)
        m_App = New Base.Kernel.Security.App(m_User) 'erzeugt ein App_objekt 
        ucHeader.InitUser(m_User)
        GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text

        If Page.IsPostBack = False Then
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            m_change = New fin_18(m_User, m_App, CStr(Session("AppID")), Session.SessionID, strFileName)
            Session.Add("objChange", m_change)
        End If

    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        If m_change Is Nothing Then
            m_change = CType(Session("objChange"), fin_18)
        End If

        'leer Darf man als Value nicht setzten, ist dann "Abweichung nach Wiedereingang"
        If rblSearch.SelectedItem.Value.Length > 3 Then
            m_change.SuchKennzeichen = String.Empty
        Else
            m_change.SuchKennzeichen = rblSearch.SelectedItem.Value
        End If

        Response.Redirect("Change50_1.aspx?AppID=" & Page.Session("AppID"))
    End Sub
End Class
' ************************************************
' $History: Change50.aspx.vb $
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
' *****************  Version 9  *****************
' User: Jungj        Date: 11.02.08   Time: 9:16
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' History hinzugefügt
' 
' ************************************************
