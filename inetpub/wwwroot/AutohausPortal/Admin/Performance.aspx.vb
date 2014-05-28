Imports CKG.Base.Kernel.Admin
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common
Imports CKG

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

            Dim objTrace As Base.Kernel.Logging.Trace
            objTrace = New Base.Kernel.Logging.Trace(m_User.App.Connectionstring, m_User.App.SaveLogAccessSAP)
            If objTrace.PerformanceData_All Then
                DataGrid1.DataSource = objTrace.StandardLog
                DataGrid1.DataBind()
                GridNavigation1.setGridElment(DataGrid1)
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