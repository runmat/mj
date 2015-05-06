Imports KBSBase

Public Class Versicherungen
    Inherits ErrorHandlingClass

    Private mBestellungen As DataTable
    Private mVersicherungen As DataTable
    Private mtblErgebnis As DataTable
    Private mstrKostStelle As String

    Public Property Bestellungen() As DataTable
        Get
            Bestellungen = mBestellungen
        End Get
        Set(ByVal value As DataTable)
            mBestellungen = value
        End Set
    End Property

    Public ReadOnly Property Versicherungen() As DataTable
        Get
            Return mVersicherungen
        End Get
    End Property

    Public Property KostStelle() As String
        Get
            KostStelle = mstrKostStelle
        End Get
        Set(ByVal value As String)
            mstrKostStelle = value
        End Set
    End Property

    Public ReadOnly Property tblErgebnis() As DataTable
        Get
            Return mtblErgebnis
        End Get
    End Property

    Public Sub New()
        If mBestellungen Is Nothing Then
            mBestellungen = New DataTable
            mBestellungen.Columns.Add("KEY", Type.GetType("System.Int32"))
            mBestellungen.Columns.Add("MATNR", String.Empty.GetType)
            mBestellungen.Columns.Add("MAKTX", String.Empty.GetType)
            mBestellungen.Columns.Add("MENGE", Type.GetType("System.Int32"))
            mBestellungen.Columns.Add("VKP", Type.GetType("System.Double"))
        End If
    End Sub

    Public Sub insertIntoBestellungen(ByVal Artikelnr As String, ByVal Menge As Integer, ByVal Artikelbezeichnung As String, ByVal Preis As Double)
        Dim NewKey As Integer = 1
        For Each dRow As DataRow In mBestellungen.Rows
            dRow("KEY") = NewKey
            NewKey += 1
        Next

        Dim tmpRow As DataRow = mBestellungen.NewRow
        tmpRow.Item("KEY") = NewKey
        tmpRow.Item("MATNR") = Artikelnr
        tmpRow.Item("MAKTX") = Artikelbezeichnung
        tmpRow.Item("VKP") = Preis
        tmpRow.Item("MENGE") = Menge
        mBestellungen.Rows.Add(tmpRow)
    End Sub

    Public Sub ShowERP()
        ClearErrorState()

        Try
            S.AP.Init("Z_FIL_EFA_VERSICHERUNGSTAMM")

            S.AP.Execute()

            If S.AP.ResultCode <> 0 Then
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

            mVersicherungen = S.AP.GetExportTable("GT_VERSSTAMM")

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

    Public Sub ChangeERP()
        ClearErrorState()

        Try
            If Bestellungen.Rows.Count = 0 Then
                RaiseError("9999", "Ausgewählte Versicherungen konnten nicht gefunden werden!")
                Exit Sub
            End If

            S.AP.Init("Z_FIL_EFA_PO_VERSICHERUNG", "I_KOSTL", mstrKostStelle)

            Dim tblSAP As DataTable = S.AP.GetImportTable("GT_BEST")
            'mit select filter, da sonst auch deleted rows mitgenommen werden, die dann auf einen fehler laufen JJU20090511
            For Each tmprow As DataRow In Bestellungen.Rows
                Dim tmpSAPRow As DataRow = tblSAP.NewRow
                tmpSAPRow("MATNR") = tmprow("MATNR").ToString
                tmpSAPRow("MENGE") = tmprow("MENGE").ToString
                tmpSAPRow("VKP") = tmprow("VKP").ToString
                tblSAP.Rows.Add(tmpSAPRow)
            Next

            S.AP.Execute()

            If S.AP.ResultCode <> 0 Then
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

            mtblErgebnis = S.AP.GetExportTable("GT_BEST")

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

End Class
