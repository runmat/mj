Option Explicit On
Option Strict On
Imports System.Configuration
Imports System.Web

Namespace Kernel.DocumentGeneration

    Public Enum Dateitypen
        pdf
        xls
        xlsx
        doc
        docx
        jpg
        jpeg
        gif
    End Enum

    Public MustInherit Class AbstractDocumentFactory

        Shared Sub New()

            Dim strLicencePath As String = ConfigurationManager.AppSettings("LicensePath")

            Dim licenseCells As Aspose.Cells.License = New Aspose.Cells.License()
            licenseCells.SetLicense(strLicencePath & "Aspose.Total.lic")

            Dim licenseWords As Aspose.Words.License = New Aspose.Words.License()
            licenseWords.SetLicense(strLicencePath & "Aspose.Total.lic")

            Dim licensePDF As Aspose.Pdf.License = New Aspose.Pdf.License()
            licensePDF.SetLicense(strLicencePath & "Aspose.Total.lic")

        End Sub

        Protected Sub TransmitFileToClient(ByVal seite As System.Web.UI.Page, ByVal dateipfad As String, ByVal dateityp As Dateitypen)
            TransmitFileToClient(seite.Response, dateipfad, dateityp)
        End Sub

        Protected Sub TransmitFileToClient(ByVal seitenresponse As Web.HttpResponse, ByVal dateipfad As String, ByVal dateityp As Dateitypen)
            If dateipfad IsNot Nothing Then
                seitenresponse.Clear()
                seitenresponse.ClearContent()
                seitenresponse.ClearHeaders()

                Select Case dateityp
                    Case Dateitypen.pdf
                        seitenresponse.ContentType = "Application/pdf"
                    Case Dateitypen.xls, Dateitypen.xlsx
                        seitenresponse.ContentType = "Application/vnd.ms-excel"
                    Case Dateitypen.doc, Dateitypen.docx
                        seitenresponse.ContentType = "Application/msword"
                    Case Dateitypen.jpg, Dateitypen.jpeg
                        seitenresponse.ContentType = "image/jpeg"
                    Case Dateitypen.gif
                        seitenresponse.ContentType = "image/gif"
                End Select

                'Write the file directly to the HTTP output stream.
                Dim fname As String = Right(dateipfad, dateipfad.Length - dateipfad.LastIndexOf("\") - 1)
                Dim MyFileStream As New IO.FileStream(dateipfad, IO.FileMode.Open)
                Dim FileSize As Long = MyFileStream.Length

                Dim dateiStream(CInt(FileSize)) As Byte
                MyFileStream.Read(dateiStream, 0, CInt(FileSize))
                MyFileStream.Close()

                seitenresponse.AddHeader("Accept-Header", FileSize.ToString())
                'seitenresponse.AddHeader("Content-Disposition", "inline; filename=" + fname)
                seitenresponse.AddHeader("Content-Disposition", "attachment")
                seitenresponse.AddHeader("Expires", "0")
                seitenresponse.AddHeader("Pragma", "cache")
                seitenresponse.AddHeader("Cache-Control", "private")
                seitenresponse.BinaryWrite(dateiStream)
                If seitenresponse.IsClientConnected Then
                    seitenresponse.Flush()
                End If
                'Response.Close()
                seitenresponse.End()
                HttpContext.Current.ApplicationInstance.CompleteRequest()
            End If
        End Sub

    End Class

End Namespace

' ************************************************
' $History: AbstractDocumentFactory.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 11.04.08   Time: 11:57
' Updated in $/CKAG/Base/Kernel/DocumentGeneration
' Migration OR
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 3.04.08    Time: 16:42
' Created in $/CKAG/Base/Kernel/DocumentGeneration
' 
' *****************  Version 2  *****************
' User: Uha          Date: 31.05.07   Time: 14:07
' Updated in $/CKG/Base/Base/Kernel/DocumentGeneration
' Probleme mit eingebetteten Dateien unter vbc.exe (=> absolute Pfade)
' 
' *****************  Version 1  *****************
' User: Uha          Date: 15.05.07   Time: 11:04
' Created in $/CKG/Base/Base/Kernel/DocumentGeneration
' Änderungen aus StartApplication vom 11.05.2007 (Aspose-Tool,
' DataTableHelper.vb, Customer.vb)
' 
' ************************************************
