Partial Public Class Report01_2
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Session("App_Filepath") Is Nothing Then
            Dim sPfad As String = Session("App_Filepath").ToString
            Response.ContentType = Session("App_ContentType").ToString
            Dim sName As String = Right(sPfad, sPfad.Length - sPfad.LastIndexOf("\") - 1)
            Response.AppendHeader("content-disposition", _
                "attachment; filename=" & sName)
            'Get the physical path to the file.
            Dim FilePath As String = sPfad
            'Write the file directly to the HTTP output stream.
            Response.WriteFile(FilePath)
            Response.End()
        End If
    End Sub

End Class