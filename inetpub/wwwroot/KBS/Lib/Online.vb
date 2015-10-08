Imports System.IO
Imports DocumentTools.Services
Imports KBSBase

Public Class Online
    Inherits ErrorHandlingClass

#Region "Declarations"

    Private mWerk As String
    Private mKostenstelle As String
    Private mAuftraege As DataTable
    Private mFormulare As DataTable

#End Region

#Region "Properties"

    Public ReadOnly Property Werk() As String
        Get
            Return mWerk
        End Get
    End Property

    Public ReadOnly Property Kostenstelle() As String
        Get
            Return mKostenstelle
        End Get
    End Property

    Public ReadOnly Property Auftraege() As DataTable
        Get
            Return mAuftraege
        End Get
    End Property

    Public ReadOnly Property Formulare() As DataTable
        Get
            Return mFormulare
        End Get
    End Property

#End Region

    Public Sub New(ByVal werk As String, ByVal kst As String)
        mWerk = werk
        mKostenstelle = kst
    End Sub

    Public Sub LoadAuftraege()
        ClearErrorState()

        Try
            S.AP.Init("Z_FIL_PRAEG_EXPORT_LISTE", "I_VKBUR, I_DATUM", mKostenstelle, DateTime.Today)

            S.AP.Execute()

            If S.AP.ResultCode = 0 Then
                mAuftraege = S.AP.GetExportTable("GT_PRAEG")
                mFormulare = S.AP.GetExportTable("GT_FILES")

                mAuftraege.Columns.Add("HasDocuments", GetType(Boolean))
                mAuftraege.Columns.Add("Auswahl", GetType(Boolean))

                For Each dRow As DataRow In mAuftraege.Rows
                    Dim docRows As DataRow() = mFormulare.Select("PRAEG_ID = '" & dRow("PRAEG_ID").ToString() & "'")
                    dRow("HasDocuments") = (docRows.Length > 0)
                    dRow("Auswahl") = False
                    dRow("POSNR") = dRow("POSNR").ToString().TrimStart("0"c)
                    dRow("MATNR") = dRow("MATNR").ToString().TrimStart("0"c)

                    Dim strZeit As String = dRow("ERZEIT").ToString()
                    If strZeit.Length = 6 Then
                        dRow("ERZEIT") = String.Format("{0}:{1}:{2}", strZeit.Substring(0, 2), strZeit.Substring(2, 2), strZeit.Substring(4, 2))
                    End If
                Next
            Else
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

    Public Sub SetAuswahlAuftrag(ByVal praegId As String, ByVal auswahl As Boolean)
        Dim dRows As DataRow() = mAuftraege.Select("PRAEG_ID = '" & praegId & "'")
        For Each dRow As DataRow In dRows
            dRow("Auswahl") = auswahl
        Next
    End Sub

    Public Sub SetAuswahlAlle()
        For Each dRow As DataRow In mAuftraege.Rows
            dRow("Auswahl") = True
        Next
    End Sub

    Public Function GetMergedPdf(Optional ByVal praegId As String = Nothing) As Byte()
        ClearErrorState()

        Try
            Dim docRows As DataRow()

            If praegId IsNot Nothing Then
                docRows = mFormulare.Select("PRAEG_ID = '" & praegId & "'")
            Else
                docRows = mFormulare.Select()
            End If

            If docRows.Length > 0 Then
                Dim filesByte As New List(Of Byte())
                Dim sPath As String = ConfigurationManager.AppSettings("DownloadPathOnline")

                For Each docRow As DataRow In docRows
                    Dim fullPath As String = sPath & docRow("FILENAME").ToString().TrimStart("/"c).Replace("/"c, "\"c)

                    If File.Exists(fullPath) Then
                        filesByte.Add(File.ReadAllBytes(fullPath))
                    End If
                Next

                'Mergen der einzelnen PDF´s in ein großes PDF
                Return PdfDocumentFactory.MergePdfDocuments(filesByte)
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try

        Return Nothing
    End Function

    Public Function SendAuftraege() As Byte()
        ClearErrorState()

        Try
            Dim selRows As DataRow() = mAuftraege.Select("Auswahl = True AND PosNr = '10'")

            If selRows.Length = 0 Then
                RaiseError("9999", "Keine Aufträge selektiert!")
                Return Nothing
            End If

            Dim filesByte As New List(Of Byte())
            Dim errors As New List(Of String)

            For Each selRow As DataRow In selRows

                S.AP.Init("Z_FIL_PRAEG_IMPORT_ERLKZ", "I_PRAEG_ID, I_WERKS", selRow("PRAEG_ID").ToString(), mWerk)

                S.AP.Execute()

                If S.AP.ResultCode = 0 Then
                    Dim htmlString As String = S.AP.GetExportParameter("E_HTML")
                    htmlString = Encoding.Default.GetString(Convert.FromBase64String(htmlString))

                    Dim gifString As String = S.AP.GetExportParameter("E_GIF")

                    Dim imgPattern As String = "<IMG SRC=""[^""]*?"""
                    Dim imgReplace As String = String.Format("<IMG SRC=""data:image/gif;base64,{0}""", gifString)

                    htmlString = Regex.Replace(htmlString, imgPattern, imgReplace)

                    Dim delRows As DataRow() = mAuftraege.Select("PRAEG_ID = '" & selRow("PRAEG_ID").ToString() & "'")
                    For Each delRow As DataRow In delRows
                        mAuftraege.Rows.Remove(delRow)
                    Next

                    filesByte.Add(PdfDocumentFactory.ConvertHtmlToPdf(htmlString, 1.5F))

                Else
                    errors.Add(String.Format("{0}: {1}", selRow("PRAEG_ID").ToString(), S.AP.ResultMessage))
                End If

            Next

            If errors.Count > 0 Then
                RaiseError("9999", String.Join(", ", errors.ToArray()))

                If filesByte.Count = 0 Then Return Nothing
            End If

            'Mergen der einzelnen PDF´s in ein großes PDF
            Return PdfDocumentFactory.MergePdfDocuments(filesByte)

        Catch ex As Exception
            RaiseError("9999", ex.Message)
            Return Nothing
        End Try
    End Function

End Class
