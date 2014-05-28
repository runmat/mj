
Public Class VersicherungsStatistiken
    Inherits ErrorHandlingClass

    Private SapExc As SAPExecutor.SAPExecutor
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

    Public Sub New()
        SapExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)
    End Sub

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

        Dim dtVersicherung As DataTable = Nothing

        ' FehlerStatus zurücksetzen
        ClearErrorState()

        ' SAPKomunikationstabelle holen
        Dim dtValues As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()

        'Import-Parameter
        dtValues.Rows.Add(New Object() {"I_VKORG", False, VkOrg})
        dtValues.Rows.Add(New Object() {"I_VKBUR", False, VkBur})
        dtValues.Rows.Add(New Object() {"I_DATUM_VON", False, DatumVon})
        dtValues.Rows.Add(New Object() {"I_DATUM_BIS", False, DatumBis})
        dtValues.Rows.Add(New Object() {"I_VKBLART", False, VkBlArt})
        dtValues.Rows.Add(New Object() {"I_MATNR", False, MatNr})
        dtValues.Rows.Add(New Object() {"I_EVB", False, EVB})

        'Export-Parameter
        dtValues.Rows.Add(New Object() {"E_SUBRC", True})
        dtValues.Rows.Add(New Object() {"E_MESSAGE", True})

        dtValues.Rows.Add(New Object() {"GT_WEB", True})

        SapExc.ExecuteERP("Z_F_CK_KASSE_STAT_INSURRANCE", dtValues)

        If SapExc.ErrorOccured Then
            If SapExc.E_SUBRC = "101" Then
                RaiseError(SapExc.E_SUBRC, "Keine Daten zu den Suchkriterien vorhanden.")
            Else
                RaiseError(SapExc.E_SUBRC, SapExc.E_MESSAGE)
            End If
        Else
            ' Versicherungen auslesen           
            dtVersicherung = CType(dtValues.Rows(9)(2), DataTable)
            CreateFilterlist(dtVersicherung)
        End If

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
