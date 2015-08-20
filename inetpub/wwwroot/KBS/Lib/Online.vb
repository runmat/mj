Imports System.IO
Imports KBSBase
Imports SmartSoft.PdfLibrary

Public Class Online
    Inherits ErrorHandlingClass

#Region "Declarations"

    Private mKostenstelle As String
    Private mAuftraege As DataTable
    Private mFormulare As DataTable

#End Region

#Region "Properties"

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

    Public Sub New(ByVal kst As String)
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
                Return PdfMerger.MergeFiles(filesByte)
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try

        Return Nothing
    End Function

    Public Sub SendAuftraege()
        ClearErrorState()

        Try
            Dim selRows As DataRow() = mAuftraege.Select("Auswahl = True")

            If selRows.Length = 0 Then
                RaiseError("9999", "Keine Aufträge selektiert!")
                Exit Sub
            End If

            S.AP.Init("Z_FIL_PRAEG_IMPORT_ERLKZ")

            Dim impTable As DataTable = S.AP.GetImportTable("GT_IMP")

            For Each dRow As DataRow In selRows
                If dRow("POSNR").ToString() = "1" Then
                    Dim impRow As DataRow = impTable.NewRow()
                    impRow("PRAEG_ID") = dRow("PRAEG_ID").ToString()
                    impTable.Rows.Add(impRow)
                End If
            Next

            S.AP.Execute()

            If S.AP.ResultCode = 0 Then
                For Each dRow As DataRow In selRows
                    mAuftraege.Rows.Remove(dRow)
                Next
            Else
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

End Class

