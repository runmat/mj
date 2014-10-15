Public Partial Class Start
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        If Not Request.QueryString("ReturnUrl") Is Nothing Then
            FormsAuthentication.RedirectFromLoginPage("Test", False)
            Response.Redirect(Request.QueryString("ReturnUrl").ToString())
        End If

    End Sub

End Class
