Option Explicit On 
Option Strict On
Imports System.Configuration
Namespace DocumentGeneration

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

    End Class

End Namespace

' ************************************************
' $History: AbstractDocumentFactory.vb $
' 
' *****************  Version 1  *****************
' User: Rudolpho     Date: 24.02.10   Time: 17:59
' Created in $/CKAG2/KBS/DocumentGeneration
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
