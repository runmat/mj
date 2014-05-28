
Partial Public Class Login
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        If KBS_BASE.login(Me) Then
            Response.Redirect("Selection.aspx")
        Else
            lblError.Text = "Keine Berechtigung - bitte wenden Sie sich an Ihre Filialbetreuung <br>IP: " & Request.UserHostAddress
        End If
    End Sub

End Class