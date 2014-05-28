Public Class Report05_Formular
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("App_Filepath") <> Nothing Then
            Dim sPfad As String = Session("App_Filepath").ToString()
            Response.ContentType = "Application/pdf"
            ' Get the physical path to the file.
            Dim FilePath As String = sPfad
            ' Write the file directly to the HTTP output stream.
            Response.WriteFile(FilePath)
            Response.End()
        End If
    End Sub

End Class