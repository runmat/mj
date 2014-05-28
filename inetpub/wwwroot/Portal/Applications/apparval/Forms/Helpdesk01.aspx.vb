Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Helpdesk01
    Inherits System.Web.UI.Page

    Protected WithEvents ucStyles As Styles
    Protected WithEvents ucHeader As Header

    Private m_App As Base.Kernel.Security.App
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents Image1 As System.Web.UI.WebControls.Image
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents Image2 As System.Web.UI.WebControls.Image
    Protected WithEvents Image3 As System.Web.UI.WebControls.Image
    Private m_User As Base.Kernel.Security.User

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
        ' Hier Benutzercode zur Seiteninitialisierung einfügen

        GetAppIDFromQueryString(Me)

        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text

        Try
            m_App = New Base.Kernel.Security.App(m_User)

            If Not IsPostBack Then
                Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
                logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")),
                                 m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString,
                                 m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
                logApp.UpdateEntry("APP", Session("AppID").ToString, "Report: Optisches Archiv")
            Else
                lblError.Text = String.Empty
            End If
        Catch ex As Exception
            lblError.Text = "Fehler beim Laden der Seite."
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
' $History: Helpdesk01.aspx.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 16:18
' Created in $/CKAG/Applications/apparval/Forms
' 
' *****************  Version 6  *****************
' User: Uha          Date: 2.07.07    Time: 10:05
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 5  *****************
' User: Uha          Date: 13.03.07   Time: 16:55
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Forms
' Image-Pfade korrigiert
' 
' *****************  Version 4  *****************
' User: Uha          Date: 6.03.07    Time: 15:30
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Forms
' GetAppIDFromQueryString(Me) hinzugefügt
' 
' *****************  Version 3  *****************
' User: Uha          Date: 6.03.07    Time: 14:48
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Forms
' 
' ************************************************
