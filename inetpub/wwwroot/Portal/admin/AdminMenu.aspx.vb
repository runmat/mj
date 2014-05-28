
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class AdminMenu
    Inherits System.Web.UI.Page

#Region " Vom Web Form Designer generierter Code "
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
    Protected WithEvents lnkCustomerManagement As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkAppManagement As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkUserManagement As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lnkOrganizationManagement As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkGroupManagement As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkArchivManagement As System.Web.UI.WebControls.HyperLink
    Protected WithEvents ucHeader As Header
    'Dieser Aufruf ist für den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: Diese Methode ist für den Web Form-Designer erforderlich
        'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
        InitializeComponent()
    End Sub

#End Region

    Private m_User As User
    Private m_App As App
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        ucStyles.TitleText = "Administrationsmenü"
        If m_User.PasswordExpired Then
            Response.Redirect("ChangePassword.aspx?pwdexp=true")
        End If

        Try
            m_App = New App(m_User)

            If Not IsPostBack Then
                lblError.Text = ""
                Dim _AdminLevel As AdminLevel = m_User.HighestAdminLevel

                If _AdminLevel = AdminLevel.None Then
                    Exit Try
                End If

                lnkUserManagement.Visible = True

                If _AdminLevel = AdminLevel.FirstLevel Then
                    Exit Try
                End If

                If _AdminLevel >= AdminLevel.Organization Then
                    If Not m_User.Customer.ShowOrganization Then
                        lnkOrganizationManagement.Visible = False
                    Else
                        lnkOrganizationManagement.Visible = True
                    End If
                End If

                If _AdminLevel >= AdminLevel.Customer Then
                    lnkGroupManagement.Visible = True
                End If

                If _AdminLevel = AdminLevel.Master Then
                    lnkCustomerManagement.Visible = True
                    lnkArchivManagement.Visible = True
                End If

                If _AdminLevel = AdminLevel.Master And m_User.Customer.AccountingArea = -1 Then
                    'nur wenn adminlevel = Master und ist in übergeordneter Firma, also buchungskreis -1 JJU2008.10.02
                    lnkAppManagement.Visible = True
                End If
            End If
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "AdminMenu", "PageLoad", lblError.Text)
            lblError.Text = "Fehler bei der Ermittlung der Menüpunkte (" & ex.Message & ")"
        End Try
    End Sub

End Class

' ************************************************
' $History: AdminMenu.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 6.10.08    Time: 9:19
' Updated in $/CKAG/admin
' ITA 2295 fertig
' 
' *****************  Version 2  *****************
' User: Hartmannu    Date: 9.09.08    Time: 13:42
' Updated in $/CKAG/admin
' ITA 2152 und 2158
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 14:47
' Created in $/CKAG/admin
' 
' *****************  Version 6  *****************
' User: Uha          Date: 30.08.07   Time: 12:36
' Updated in $/CKG/Admin/AdminWeb
' ITA 1280: Paßwortversand im Web auf Benutzerwunsch
' 
' *****************  Version 5  *****************
' User: Uha          Date: 27.08.07   Time: 17:13
' Updated in $/CKG/Admin/AdminWeb
' ITA 1269: Archivverwaltung/-zuordnung jetzt auch per Web
' 
' *****************  Version 4  *****************
' User: Uha          Date: 13.03.07   Time: 10:53
' Updated in $/CKG/Admin/AdminWeb
' History-Eintrag vorbereitet
' 
' ************************************************
