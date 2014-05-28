Public Class InventurSonstigeLagerware

#Region "Declarations"

    Protected mE_SUBRC As Integer
    Protected mE_MESSAGE As String
    Protected mFiliale As String
    Protected mArtikelliste As DataTable
    Dim SAPExc As SAPExecutor.SAPExecutor

#End Region

#Region "Properties"

    Public Property E_SUBRC() As Integer
        Get
            Return mE_SUBRC
        End Get
        Set(ByVal Value As Integer)
            mE_SUBRC = Value
        End Set
    End Property

    Public Property E_MESSAGE() As String
        Get
            Return mE_MESSAGE
        End Get
        Set(ByVal Value As String)
            mE_MESSAGE = Value
        End Set
    End Property

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

        SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)
        E_MESSAGE = ""
        E_SUBRC = 0
        Try
            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()
            'Importparameter (aktuell nicht relevant)
            dt.Rows.Add(New Object() {"I_POSNR", False, ""})
            dt.Rows.Add(New Object() {"I_ARKTXT", False, ""})
            'Exportparameter
            dt.Rows.Add(New Object() {"GT_WEB", True})

            SAPExc.ExecuteERP("Z_CK_WAWI_MAT_001", dt)

            If (SAPExc.ErrorOccured) Then
                E_SUBRC = SAPExc.E_SUBRC
                E_MESSAGE = SAPExc.E_MESSAGE
            End If

            Dim retRows As DataRow = dt.Select("Fieldname='GT_WEB'")(0)
            If retRows IsNot Nothing Then
                Dim sapTable As DataTable = DirectCast(retRows("Data"), DataTable)

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
            End If

        Catch ex As Exception
            mE_SUBRC = -9999
            mE_MESSAGE = "Es ist ein Fehler aufgetreten: " & ex.Message
        End Try

    End Sub

#End Region

End Class
