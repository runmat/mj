Imports TimeRegistration

Public Class PDFZeitnachweise
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            ' Header bereinigen
            Response.Clear()
            Response.ClearContent()
            Response.ClearHeaders()
            Response.ContentType = "application/pdf"

            Dim PDFPrint As PDFPrintObj = Session("PDFPrintObj")
            Dim objZeitnachweis As New Zeitnachweis()
            Dim bytePDF As Byte() = objZeitnachweis.GetPdf(PDFPrint)

            ' Header 
            Response.AddHeader("Content-Disposition", "inline; filename=" + "0003_0213.pdf")
            Response.AddHeader("Expires", "0")
            Response.AddHeader("Pragma", "cache")
            Response.AddHeader("Cache-Control", "private")
            Response.BinaryWrite(bytePDF)
            Response.Flush()
            Response.Close()
            Response.End()

        Catch ex As Exception
            lblError.Text = ex.Message
        End Try

    End Sub

End Class