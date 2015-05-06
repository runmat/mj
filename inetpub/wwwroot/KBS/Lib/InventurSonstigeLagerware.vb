Imports KBSBase

Public Class InventurSonstigeLagerware
    Inherits ErrorHAndlingClass

#Region "Declarations"

    Protected mFiliale As String
    Protected mArtikelliste As DataTable

#End Region

#Region "Properties"

    Public Property Filiale() As String
        Get
            Return mFiliale
        End Get
        Set(ByVal Value As String)
            mFiliale = Value
        End Set
    End Property

    Public Property Artikelliste() As DataTable
        Get
            Return mArtikelliste
        End Get
        Set(ByVal Value As DataTable)
            mArtikelliste = Value
        End Set
    End Property

#End Region

#Region "Functions"

    Public Sub New(ByVal filiale As String)
        mFiliale = filiale
    End Sub

    Public Sub FillArtikelliste()
        InitArtikelliste()
        FillArtikellisteSap()
    End Sub

    Private Sub InitArtikelliste()
        Dim dRow As DataRow

        mArtikelliste = New DataTable()
        mArtikelliste.Columns.Add("Position", GetType(Int32))
        mArtikelliste.Columns.Add("Artikel", GetType(System.String))
        mArtikelliste.Columns.Add("Menge", GetType(System.String))
        mArtikelliste.Columns.Add("Vorbelegt", GetType(System.Boolean))
        mArtikelliste.Columns.Add("Filiale", GetType(System.String))

        'Positionen initial füllen
        For i As Integer = 1 To 50
            dRow = mArtikelliste.NewRow()
            dRow("Position") = i
            dRow("Artikel") = ""
            dRow("Menge") = "0"
            dRow("Vorbelegt") = False
            dRow("Filiale") = mFiliale
            mArtikelliste.Rows.Add(dRow)
        Next
    End Sub

    Private Sub FillArtikellisteSap()
        Dim dRow As DataRow
        Dim posNr As Integer
        Dim foundRows As DataRow()

        ClearErrorState()

        Try
            S.AP.Init("Z_CK_WAWI_MAT_001")

            S.AP.Execute()

            If S.AP.ResultCode <> 0 Then
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

            Dim sapTable As DataTable = S.AP.GetExportTable("GT_WEB")

            For Each sRow As DataRow In sapTable.Rows
                If Integer.TryParse(sRow("POSNR").ToString(), posNr) Then
                    foundRows = mArtikelliste.Select("Position=" & posNr)
                    If foundRows.Count > 0 Then
                        dRow = foundRows(0)
                        dRow("Artikel") = sRow("ARKTXT").ToString()
                        dRow("Vorbelegt") = True
                    End If
                End If
            Next

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

#End Region

End Class
