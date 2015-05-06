Imports TimeRegistration
Imports KBS.KBS_BASE

Public Class PrintZeitübersicht
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        FormAuth(Me)

        If Not Session("TimeReg") Is Nothing Then
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
                Response.AddHeader("Content-Disposition", "inline; filename=" + "0003_2013.pdf")
                Response.AddHeader("Expires", "0")
                Response.AddHeader("Pragma", "cache")
                Response.AddHeader("Cache-Control", "private")
                Response.BinaryWrite(bytePDF)
                Response.Flush()
                Response.Close()
                Response.End()

            Catch ex As Exception

            End Try
        Else
            Response.Redirect("ÜbersichtZeiten.aspx")
        End If

    End Sub

End Class