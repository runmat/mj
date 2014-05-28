Imports System.IO
Partial Public Class Printpdf
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("App_Filepath") Is Nothing Then
            Dim sPfad As String = Session("App_Filepath").ToString
            Response.Clear()
            Response.ClearContent()
            Response.ClearHeaders()
            Response.ContentType = Session("App_ContentType").ToString
            Session("App_Filepath") = Nothing
            Session("App_ContentType") = Nothing
            'Get the physical path to the file.
            Dim FilePath As String = sPfad
            'Write the file directly to the HTTP output stream.
            ' Response.AddHeader("content-disposition", "attachment;filename=" + "kroschke.pdf")
            Dim fname As String = Right(sPfad, sPfad.Length - sPfad.LastIndexOf("\") - 1)
            Dim MyFileStream = New FileStream(FilePath, FileMode.Open)
            Dim FileSize As Long = MyFileStream.Length

            Dim pdfStream(MyFileStream.Length) As Byte
            MyFileStream.Read(pdfStream, 0, CInt(FileSize))
            MyFileStream.Close()

            Response.AddHeader("Accept-Header", FileSize.ToString())
            Response.AddHeader("Content-Disposition", "inline; filename=" + fname)
            Response.AddHeader("Expires", "0")
            Response.AddHeader("Pragma", "cache")
            Response.AddHeader("Cache-Control", "private")
            Response.BinaryWrite(pdfStream)
            If Response.IsClientConnected Then
                Response.Flush()
            End If
            'Response.Close()
            Response.End()

        End If
    End Sub

End Class