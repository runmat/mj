
''' <summary>
''' Senden von Dateien an den Client
''' </summary>
''' <remarks></remarks>
Partial Public Class Downloadfile2
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Session("OnlinePdfBytes") IsNot Nothing Then
            Dim pdfBytes As Byte() = CType(Session("OnlinePdfBytes"), Byte())

            Dim filename As String = "AuftraegeOnline.pdf"
            If Session("OnlinePdfName") IsNot Nothing Then
                filename = Session("OnlinePdfName").ToString()
            End If

            Response.Clear()
            Response.ClearContent()
            Response.ClearHeaders()
            Response.ContentType = "application/pdf"
            Response.AddHeader("Content-Disposition", "inline; filename=" + filename)
            Response.AddHeader("Expires", "0")
            Response.AddHeader("Pragma", "cache")
            Response.AddHeader("Cache-Control", "private")
            Response.BinaryWrite(pdfBytes)
            If (Response.IsClientConnected) Then
                Response.Flush()
            End If
            Response.End()
            Session("OnlinePdfBytes") = Nothing
        End If
    End Sub

End Class