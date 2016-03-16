Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common

Public Class Performance2
    Inherits System.Web.UI.Page

    Private m_User As User
    Private m_App As App

#Region " Declarations"
    Protected WithEvents cmdCreate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblMin As System.Web.UI.WebControls.Label
    Protected WithEvents lblMax As System.Web.UI.WebControls.Label
    Protected WithEvents lblCategoryName As System.Web.UI.WebControls.Label
    Protected WithEvents lblCounterName As System.Web.UI.WebControls.Label
    Protected WithEvents lblInstanceName As System.Web.UI.WebControls.Label
    Protected WithEvents lblValue As System.Web.UI.WebControls.Label
    Protected WithEvents lblCounterUnit As System.Web.UI.WebControls.Label
    Protected WithEvents Repeater1 As System.Web.UI.WebControls.Repeater
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    Protected WithEvents Label6 As System.Web.UI.WebControls.Label
    Protected WithEvents Label7 As System.Web.UI.WebControls.Label
    Protected WithEvents Label8 As System.Web.UI.WebControls.Label
    Protected WithEvents Label9 As System.Web.UI.WebControls.Label
    Protected WithEvents Label10 As System.Web.UI.WebControls.Label
    Protected WithEvents Label11 As System.Web.UI.WebControls.Label
    Protected WithEvents Label12 As System.Web.UI.WebControls.Label
    Protected WithEvents Label13 As System.Web.UI.WebControls.Label
    Protected WithEvents Label14 As System.Web.UI.WebControls.Label
    Protected WithEvents Label15 As System.Web.UI.WebControls.Label
    Protected WithEvents Label16 As System.Web.UI.WebControls.Label
    Protected WithEvents Label17 As System.Web.UI.WebControls.Label
    Protected WithEvents Label18 As System.Web.UI.WebControls.Label
    Protected WithEvents Label19 As System.Web.UI.WebControls.Label
    Protected WithEvents Label20 As System.Web.UI.WebControls.Label
    Protected WithEvents Label21 As System.Web.UI.WebControls.Label
    Protected WithEvents Label22 As System.Web.UI.WebControls.Label
    Protected WithEvents Label23 As System.Web.UI.WebControls.Label
    Protected WithEvents Label24 As System.Web.UI.WebControls.Label
    Protected WithEvents Repeater2 As System.Web.UI.WebControls.Repeater
    Protected WithEvents Repeater3 As System.Web.UI.WebControls.Repeater
    Protected WithEvents Repeater4 As System.Web.UI.WebControls.Repeater
    Protected WithEvents Repeater5 As System.Web.UI.WebControls.Repeater
    Protected WithEvents Repeater6 As System.Web.UI.WebControls.Repeater
    Protected WithEvents Repeater7 As System.Web.UI.WebControls.Repeater
    Protected WithEvents Repeater8 As System.Web.UI.WebControls.Repeater
    Protected WithEvents Repeater9 As System.Web.UI.WebControls.Repeater
    Protected WithEvents Repeater10 As System.Web.UI.WebControls.Repeater
    Protected WithEvents Repeater11 As System.Web.UI.WebControls.Repeater
    Protected WithEvents Repeater12 As System.Web.UI.WebControls.Repeater
    Protected WithEvents Repeater13 As System.Web.UI.WebControls.Repeater
    Protected WithEvents Repeater14 As System.Web.UI.WebControls.Repeater
    Protected WithEvents Repeater15 As System.Web.UI.WebControls.Repeater
    Protected WithEvents Repeater16 As System.Web.UI.WebControls.Repeater
    Protected WithEvents Repeater17 As System.Web.UI.WebControls.Repeater
    Protected WithEvents Repeater18 As System.Web.UI.WebControls.Repeater
    Protected WithEvents Repeater19 As System.Web.UI.WebControls.Repeater
    Protected WithEvents Repeater20 As System.Web.UI.WebControls.Repeater
    Protected WithEvents Repeater21 As System.Web.UI.WebControls.Repeater
    Protected WithEvents Repeater22 As System.Web.UI.WebControls.Repeater
    Protected WithEvents Repeater23 As System.Web.UI.WebControls.Repeater
    Protected WithEvents Repeater24 As System.Web.UI.WebControls.Repeater
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label

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

        Try
            m_App = New App(m_User)

            If Not IsPostBack Then
                lblError.Text = ""
            End If

            If Request.QueryString("Return") Is Nothing Then
                FillData()
            End If
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "Performance2", "Page_Load", ex.ToString)

            lblError.Text = ex.ToString
            lblError.Visible = True
        End Try
    End Sub

    Private Sub FillData()
        lblError.Text = "Keine Daten vorhanden. Dieses Logging ist veraltet."
        lblError.Visible = True
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        Try
            Response.Redirect("Performance.aspx?Return=True")
        Catch
        End Try
    End Sub

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        FillData()
    End Sub

    Protected Sub lb_zurueck_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdcreate.Click
        Response.Redirect("Performance.aspx?Return=True")
    End Sub

End Class