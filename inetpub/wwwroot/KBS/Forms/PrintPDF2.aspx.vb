
Imports System.IO

Partial Public Class PrintPDF2
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Session("App_FilepathUtsch") Is Nothing Then
                Dim sPfad As String = Session("App_FilepathUtsch").ToString
                Try
                    Response.Clear()
                    Response.ClearContent()
                    Response.ClearHeaders()
                    Response.ContentType = Session("App_ContentType").ToString
                Catch ex As Exception
                    lblError.Text = "App_ContentType Catch EX"
                End Try

                Try
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
                    Response.Flush()
                    Response.Close()
                    Response.End()
                Catch ex As Exception
                    lblError.Text = "FilePath Catch EX: " + sPfad
                End Try
                'Get the physical path to the file.
            End If
        Catch ex As Exception
            lblError.Text = "Oberste Catch EX"
        End Try

    End Sub

End Class