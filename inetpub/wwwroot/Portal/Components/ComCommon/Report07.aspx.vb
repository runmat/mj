Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Report07
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

    Private m_context As HttpContext = HttpContext.Current
    Private m_App As Security.App
    Private m_User As Security.User

    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents StandardLog1 As StandardLog

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        m_App = New Security.App(m_User)

        StandardLog1.ConnectionString = m_App.Connectionstring
        StandardLog1.SessionID = Session.SessionID
        StandardLog1.User = m_User.UserName
        StandardLog1.Header = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = StandardLog1.Header
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Report07.aspx.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:32
' Created in $/CKAG/Components/ComCommon
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 10:47
' Created in $/CKAG/portal/Components/ComCommon
' 
' *****************  Version 7  *****************
' User: Uha          Date: 12.07.07   Time: 9:28
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 6  *****************
' User: Uha          Date: 22.05.07   Time: 9:45
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 5  *****************
' User: Uha          Date: 5.03.07    Time: 14:57
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' 
' ************************************************
