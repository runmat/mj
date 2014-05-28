﻿Imports TimeRegistration
Imports KBS.KBS_BASE

Public Class PrintZeitübersicht
    Inherits Page

    Private mObjKasse As Kasse

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        FormAuth(Me)

        If Not Session("mKasse") Is Nothing Then
            mObjKasse = Session("mKasse")
        End If

        If Not Session("TimeReg") Is Nothing Then
            Try
                ' Header bereinigen
                Response.Clear()
                Response.ClearContent()
                Response.ClearHeaders()
                Response.ContentType = "application/pdf"

                ' SAP Zugriff vorbereiten
                Dim PDFPrint As PDFPrintObj = Session("PDFPrintObj")
                Dim SapEx As SAPExecutor.SAPExecutor = New SAPExecutor.SAPExecutor(PDFPrint.SAPConnectionString)

                Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()

                ' Tabellenschema {Feldname, ParameterDirection(0=Input,1=Output), (optional) Feldinhalt als Object, Feldlänge (0=unbestimmt)}
                'Import-Parameter
                dt.Rows.Add(New Object() {"BD_NR", False, PDFPrint.Kartennummer, 4})
                dt.Rows.Add(New Object() {"VDATE", False, PDFPrint.VDate})
                dt.Rows.Add(New Object() {"BDATE", False, PDFPrint.BDate})
                dt.Rows.Add(New Object() {"MODUS", False, "M"})
                dt.Rows.Add(New Object() {"AUSGABE", False, ""})
                dt.Rows.Add(New Object() {"E_PDF", True})

                SapEx.ExecuteERP("Z_HR_ZE_GET_POSTINGS_AS_PDF", dt)

                ' Byte() aus SAP holen
                Dim retRow As DataRow = dt.Select("Fieldname='E_PDF'")(0)
                Dim bytePDF As Byte() = retRow("DATA")

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