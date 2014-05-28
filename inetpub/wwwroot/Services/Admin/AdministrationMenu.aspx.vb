Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common

Partial Public Class AdministrationMenu
    Inherits System.Web.UI.Page
    Private m_User As User
    Private m_App As App

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        If m_User.PasswordExpired Then
            Response.Redirect("ChangePassword.aspx?pwdexp=true")
        End If

        Try
            Dim AdminCount As Integer
            m_App = New App(m_User)

            lblError.Text = ""
            lblMessage.Text = ""

            If Not IsPostBack Then
                Dim _AdminLevel As AdminLevel = m_User.HighestAdminLevel

                If _AdminLevel = AdminLevel.None Then
                    Exit Try
                End If
                trUserManagement.Visible = True
                AdminCount += 1
                If _AdminLevel = AdminLevel.FirstLevel Then
                    Exit Try
                End If

                If _AdminLevel >= AdminLevel.Customer Then
                    trGroupManagement.Visible = True
                    trUserManagement.Visible = True
                    trContactManagement.Visible = False
                    AdminCount += 1
                End If


                If _AdminLevel = AdminLevel.Organization Then
                    trContactManagement.Visible = False
                End If


                If _AdminLevel >= AdminLevel.Organization Then

                    If Not m_User.Customer.ShowOrganization Then
                        trOrganizationManagement.Visible = False
                    Else
                        trOrganizationManagement.Visible = True
                    End If
                End If


                If _AdminLevel = AdminLevel.Master Then
                    trCustomerManagement.Visible = True
                    trArchivManagement.Visible = True
                    trContactManagement.Visible = True
                End If

                If _AdminLevel = AdminLevel.Master And m_User.Customer.AccountingArea = -1 Then
                    'nur wenn adminlevel = Master und ist in übergeordneter Firma, also buchungskreis -1 JJU2008.10.02
                    trAppManagement.Visible = True
                End If
            End If
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "AdminMenu", "PageLoad", lblError.Text)
            lblError.Text = "Fehler bei der Ermittlung der Menüpunkte (" & ex.Message & ")"
        End Try
    End Sub

End Class