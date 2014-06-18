Public Partial Class Start
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Request.QueryString("ReturnUrl") Is Nothing Then
            FormsAuthentication.RedirectFromLoginPage("Test", False)
            Response.Redirect(Request.QueryString("ReturnUrl").ToString())
        End If

    End Sub

End Class
' ************************************************
' $History: Start.aspx.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.10.09    Time: 8:42
' Created in $/Validation/Validation/Forms
' ITA: 3132
' 