Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common

Public Class Performance
    Inherits System.Web.UI.Page
    Private m_User As User
    Private m_App As App

#Region " Declarations"
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
        AdminAuth(Me, m_User, AdminLevel.Organization)
        Session("PerformanceCounterID") = Nothing

        Try
            m_App = New App(m_User)

            If Not IsPostBack Then
                lblError.Text = "Keine Daten vorhanden. Dieses Logging ist veraltet."
            End If
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "Performance", "Page_Load", ex.ToString)

            lblError.Text = ex.ToString
            lblError.Visible = True
        End Try
    End Sub

End Class