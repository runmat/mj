Imports KBSBase

Public Class VersicherungsStatistiken
    Inherits ErrorHandlingClass

    Private lstFilter As List(Of VersicherungsArtikel)

#Region "Strukturen"

    Public Class VersicherungsArtikel
        Private mMaterialkurztext As String
        Private mMaterialnummer As String

        Public Property Materialkurztext As String
            Get
                Return mMaterialkurztext
            End Get
            Set(value As String)
                mMaterialkurztext = value
            End Set
        End Property

        Public Property Materialnummer As String
            Get
                Return mMaterialnummer
            End Get
            Set(value As String)
                mMaterialnummer = value
            End Set
        End Property

        Public Sub New(ByVal kurztext As String, ByVal MatNr As String)
            mMaterialkurztext = kurztext
            mMaterialnummer = MatNr
        End Sub

    End Class

#End Region

#Region "Properties"

    ''' <summary>
    ''' Liste mit Filterwerten
    ''' </summary>
    ''' <value>Filterliste</value>
    ''' <returns>Filterliste</returns>
    ReadOnly Property FilterListe As List(Of VersicherungsArtikel)
        Get
            Return lstFilter
        End Get
    End Property

#End Region

    ''' <summary>
    ''' Liefert die Versicherungen zu den gewählten Filtern
    ''' </summary>
    ''' <param name="VkBur">Verkaufsbüro</param>
    ''' <param name="VkOrg">Verkaufsorganisation</param>
    ''' <param name="DatumVon">untere Datumsgrenze</param>
    ''' <param name="DatumBis">obere Datumsgrenze</param>
    ''' <param name="VkBlArt">Belegart</param>
    ''' <param name="MatNr">Materialnummer</param>
    ''' <param name="EVB">EVB-Nummer</param>
    ''' <returns>Tabelle der Versicherungen</returns>
    Public Function GetVersicherungen(ByVal VkBur As String, ByVal VkOrg As String, Optional ByVal DatumVon As String = "",
                                      Optional ByVal DatumBis As String = "", Optional ByVal VkBlArt As String = "0", Optional ByVal MatNr As String = "",
                                      Optional ByVal EVB As String = "") As DataTable
        ClearErrorState()

        Dim dtVersicherung As DataTable = Nothing

        Try
            S.AP.Init("Z_F_CK_KASSE_STAT_INSURRANCE")

            S.AP.SetImportParameter("I_VKORG", VkOrg)
            S.AP.SetImportParameter("I_VKBUR", VkBur)
            S.AP.SetImportParameter("I_DATUM_VON", DatumVon)
            S.AP.SetImportParameter("I_DATUM_BIS", DatumBis)
            S.AP.SetImportParameter("I_VKBLART", VkBlArt)
            S.AP.SetImportParameter("I_MATNR", MatNr)
            S.AP.SetImportParameter("I_EVB", EVB)

            S.AP.Execute()

            If S.AP.ResultCode = 0 Then
                ' Versicherungen auslesen           
                dtVersicherung = S.AP.GetExportTable("GT_WEB")
                CreateFilterlist(dtVersicherung)
            Else
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try

        Return dtVersicherung
    End Function

    ''' <summary>
    ''' Liste mit Filterwerten erzeugen
    ''' </summary>
    Private Sub CreateFilterlist(ByRef tblResult As DataTable)
        lstFilter = New List(Of VersicherungsArtikel)

        For Each row In tblResult.Rows
            Dim Found = False

            For Each item As VersicherungsArtikel In lstFilter
                If row("MATNR") = item.Materialnummer Then
                    Found = True
                    Exit For
                End If
            Next

            If Not Found Then
                lstFilter.Add(New VersicherungsArtikel(row("MAKTX"), row("MATNR")))
            End If
        Next

        lstFilter.Sort(AddressOf CompareArtikelByMATNR)
    End Sub

    Private Shared Function CompareArtikelByMATNR(ByVal Artikel1 As VersicherungsArtikel, ByVal Artikel2 As VersicherungsArtikel) As Integer

        If Artikel1 Is Nothing Then
            If Artikel2 Is Nothing Then
                ' Beide Artikel sind gleich
                Return 0
            Else
                ' Artikel2 ist größer
                Return -1
            End If
        Else
            If Artikel2 Is Nothing Then
                ' Artikel1 ist größer
                Return 1
            Else
                'Kein Artikel ist Nothing, dann vergleichen
                If CInt(Artikel1.Materialnummer) < CInt(Artikel2.Materialnummer) Then
                    Return -1
                ElseIf CInt(Artikel1.Materialnummer) > CInt(Artikel2.Materialnummer) Then
                    Return 1
                Else
                    Return 0
                End If
            End If
        End If

    End Function

End Class
