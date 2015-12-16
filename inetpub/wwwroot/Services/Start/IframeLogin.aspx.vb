Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base
Imports CKG.Base.Business
Partial Public Class IframeLogin
    Inherits System.Web.UI.Page
    Private m_User As New Base.Kernel.Security.User()
    Private m_App As Base.Kernel.Security.App

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Me.Session("objUser") Is Nothing Then
            m_User = CType(Session("objUser"), Base.Kernel.Security.User)
            If Not (m_User.LoggedOn And m_User.DoubleLoginTry) Then
                If Me.User.Identity.IsAuthenticated = False Then
                    Response.Redirect(BouncePage(Me), True)


                End If
            End If
        End If

        txtUsername.Focus()
        Page.Title = "Anmeldung"
        If Not IsPostBack Then

        End If

    End Sub
    Private Sub cmdLogin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdLogin.Click
        Try
            If Not Me.Session("objUser") Is Nothing AndAlso _
                Me.User.Identity.IsAuthenticated = False Then
                '---JVE: User nicht mehr in der Session gespeichert bzw. nicht Authentifiziert---
                Response.Redirect(BouncePage(Me), True)
                Exit Sub
            End If

            If m_User.Login(txtUsername.Text, txtPassword.Text, Session.SessionID.ToString, Request.Url.AbsoluteUri, False) Then
                '    If m_User.Login(txtUsername.Text, Session.SessionID.ToString) Then

                Session("UrlRemoteLogin_LogoutUrl") = m_User.Customer.LogoutLink
                m_User.SetLastLogin(Now)
                Session("objUser") = m_User
                If m_User.DoubleLoginTry Then
                    Me.StandardLogin.Visible = False
                Else
                    'System.Web.Security.FormsAuthentication.RedirectFromLoginPage(m_User.UserID.ToString, False)

                    'zur späteren Benutzung (iframe)
                    FormsAuthentication.SetAuthCookie(m_User.UserID.ToString, False)
                    Response.Write("<script language='javascript'>")
                    Response.Write("window.open('Selection.aspx' ,'','width=1000, height=725,toobar=yes,addressbar=yes,menubar=yes,scrollbars=yes,resizable=yes');")
                    Response.Write("window.location.href ='IframeLogin.aspx';")
                    Response.Write("<" + "/" + "script" + ">")

                End If
            Else
                '############################################################
                'Error-Property bei User-Objekt einfügen und hier darstellen
                If Len(m_User.ErrorMessage) > 0 Then
                    If m_User.ErrorMessage = "4174" Then
                        'Benutzer existiert und die Voraussetzungen zur Passwortanforderung
                        'per geheimer Frage sind gegeben
                        Session("objUser") = m_User
                        lblError.Text = "Fehler bei der Anmeldung"

                    ElseIf m_User.ErrorMessage = "9999" Then
                        lblError.Text = "Fehler bei der Anmeldung"
                    Else
                        If m_User.AccountIsLockedOut AndAlso m_User.AccountIsLockedBy = "User" Then ' Gleich weiter zur Ensperrung!
                            If m_User.Email.Length > 0 And m_User.Customer.ForcePasswordQuestion And m_User.QuestionID > -1 Then
                                System.Web.Security.FormsAuthentication.RedirectFromLoginPage(m_User.UserID.ToString, False)
                            Else
                                lblError.Text = "Fehler bei der Anmeldung (" & m_User.ErrorMessage & ")"
                            End If
                        ElseIf m_User.AccountIsLockedOut AndAlso m_User.AccountIsLockedBy = "Now" Then ' gerade gesperrt? Sperrung anzeigen!
                            lblError.Text = "Fehler bei der Anmeldung (" & m_User.ErrorMessage & ")"
                        Else
                            lblError.Text = "Fehler bei der Anmeldung (" & m_User.ErrorMessage & ")"

                        End If

                    End If
                Else
                    lblError.Text = "Fehler bei der Anmeldung"
                End If
            End If
        Catch ex As Exception
            m_App = New Base.Kernel.Security.App(m_User)
            m_App.WriteErrorText(1, txtUsername.Text, "Login", "cmdLogin_Click", ex.ToString)

            lblError.Text = "Fehler bei der Anmeldung (" & ex.Message & ")"
        End Try
    End Sub

    Private Sub btnEmpty_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnEmpty.Click
        cmdLogin_Click(sender, e)
    End Sub
End Class