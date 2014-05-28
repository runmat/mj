
''' <summary>
''' Senden von Dateien an den Client
''' </summary>
''' <remarks></remarks>
Partial Public Class Downloadfile
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Session("App_Filepath") IsNot Nothing Then
            Dim sPfad As String = Session("App_Filepath").ToString()

            If Session("App_ContentDisposition") IsNot Nothing AndAlso Not String.IsNullOrEmpty(Session("App_ContentDisposition").ToString()) Then
                Dim strContentDisposition As String = Session("App_ContentDisposition").ToString()
                If sPfad.Contains("\") Then
                    Response.AddHeader("Content-Disposition", strContentDisposition & "; filename=" & sPfad.Substring(sPfad.LastIndexOf("\"c) + 1, sPfad.Length - (sPfad.LastIndexOf("\"c) + 1)))
                Else
                    Response.AddHeader("Content-Disposition", strContentDisposition & "; filename=" & sPfad)
                End If
            End If

            Response.ContentType = Session("App_ContentType").ToString()
            'Get the physical path to the file.
            Dim FilePath As String = sPfad
            'Write the file directly to the HTTP output stream.
            Response.WriteFile(FilePath)
            Response.End()
        End If
    End Sub

End Class