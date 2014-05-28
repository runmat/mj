
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Performance
    Inherits System.Web.UI.Page
    Private m_User As User
    Private m_App As App

    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles

#Region " Declarations"
    Protected WithEvents cmdCreate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
#End Region

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
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        ucStyles.TitleText = "Allgemeine Leistungsangaben - Übersicht"
        AdminAuth(Me, m_User, AdminLevel.Organization)
        Session("PerformanceCounterID") = Nothing

        Try
            m_App = New App(m_User)

            Dim objTrace As Base.Kernel.Logging.Trace
            objTrace = New Base.Kernel.Logging.Trace(m_User.App.Connectionstring, m_User.App.SaveLogAccessSAP)
            If objTrace.PerformanceData_All Then
                DataGrid1.DataSource = objTrace.StandardLog
                DataGrid1.DataBind()
            End If

            If Not IsPostBack Then
                lblError.Text = ""
            End If
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "Performance", "Page_Load", ex.ToString)

            lblError.Text = ex.ToString
            lblError.Visible = True
        End Try
    End Sub
End Class

' ************************************************
' $History: Performance.aspx.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 14:47
' Created in $/CKAG/admin
' 
' *****************  Version 5  *****************
' User: Uha          Date: 13.03.07   Time: 10:53
' Updated in $/CKG/Admin/AdminWeb
' History-Eintrag vorbereitet
' 
' ************************************************
