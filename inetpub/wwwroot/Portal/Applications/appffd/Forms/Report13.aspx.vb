Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business

Public Class Report13
    Inherits System.Web.UI.Page

#Region " Vom Web Form Designer generierter Code "

    'Dieser Aufruf ist f�r den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: Diese Methode ist f�r den Web Form-Designer erforderlich
        'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
        InitializeComponent()
    End Sub

#End Region

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App

    Protected WithEvents Hyperlink1 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkLogout As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents suche1 As Suche
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        NoDealer(Me, m_User)
        suche1.NurFilialSuche = True
        suche1.AllowEmptySearch = True
        suche1.RedirectUrl = "Report13_2.aspx"
        suche1.User = m_User

        GetAppIDFromQueryString(Me)

        suche1.NurFilialSuche = True
        suche1.RedirectUrl = "Report13_2.aspx?AppID=" & Session("AppID").ToString
        suche1.User = m_User
        Session("SelectedDealer") = Nothing
        Try
            suche1.Headline = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            m_App = New Base.Kernel.Security.App(m_User)
            ucStyles.TitleText = suche1.Headline

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
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
' $History: Report13.aspx.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:23
' Created in $/CKAG/Applications/appffd/Forms
' 
' *****************  Version 7  *****************
' User: Uha          Date: 2.07.07    Time: 12:52
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingef�hrt
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 15.06.07   Time: 8:12
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' 
' *****************  Version 5  *****************
' User: Uha          Date: 3.05.07    Time: 18:05
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' �nderungen aus StartApplication vom 02.05.2007 Mittags �bernommen
' 
' ************************************************
