
Public Class Versicherungen
    Private mE_SUBRC As Integer
    Private mE_MESSAGE As String
    Private mBestellungen As DataTable
    Private mVersicherungen As DataTable
    Private mtblErgebnis As DataTable
    Private mstrKostStelle As String
    Dim SAPExc As SAPExecutor.SAPExecutor

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

    Public Sub insertIntoBestellungen(ByVal Artikelnr As String, ByVal Menge As Integer, _
                                       ByVal Artikelbezeichnung As String, ByVal Preis As Double)


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

        SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)
        E_MESSAGE = ""
        E_SUBRC = 0

        Try
            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()

            dt.Rows.Add(New Object() {"GT_VERSSTAMM", True})

            SAPExc.ExecuteERP("Z_FIL_EFA_VERSICHERUNGSTAMM", dt)

            If (SAPExc.ErrorOccured) Then
                E_SUBRC = SAPExc.E_SUBRC
                E_MESSAGE = SAPExc.E_MESSAGE
            End If
            Dim retRows As DataRow = dt.Select("Fieldname='GT_VERSSTAMM'")(0)
            If Not retRows Is Nothing Then
                mVersicherungen = DirectCast(retRows("Data"), DataTable)
            End If

        Catch ex As Exception
        Finally

        End Try

    End Sub

    Public Sub ChangeERP()

        E_MESSAGE = ""
        E_SUBRC = 0

        Dim tblSAP As New DataTable()
        tblSAP.Columns.Add("MATNR", String.Empty.GetType)
        tblSAP.Columns.Add("MENGE", String.Empty.GetType)
        tblSAP.Columns.Add("VKP", String.Empty.GetType)
        tblSAP.Columns.Add("BSTNR", String.Empty.GetType)

        Dim tmpSAPRow As DataRow

        If Bestellungen.Rows.Count = 0 Then
            E_MESSAGE = "Ausgewählte Versicherungen konnten nicht gefunden werden!"
            Exit Sub
        End If
        'mit select filter, da sonst auch deletet rows mitgenommen werden, die dann auf einen fehler laufen JJU20090511
        For Each tmprow As DataRow In Bestellungen.Rows
            tmpSAPRow = tblSAP.NewRow
            tmpSAPRow("MATNR") = tmprow("MATNR").ToString
            tmpSAPRow("MENGE") = tmprow("MENGE").ToString
            tmpSAPRow("VKP") = tmprow("VKP").ToString
            tblSAP.Rows.Add(tmpSAPRow)
        Next

        SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)

        Try
            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()
            dt.Rows.Add(New Object() {"I_KOSTL", False, mstrKostStelle, 10})
            dt.Rows.Add(New Object() {"GT_BEST", False, tblSAP})
            dt.Rows.Add(New Object() {"GT_BEST", True})

            SAPExc.ExecuteERP("Z_FIL_EFA_PO_VERSICHERUNG", dt)

            If (SAPExc.ErrorOccured) Then
                E_SUBRC = SAPExc.E_SUBRC
                E_MESSAGE = SAPExc.E_MESSAGE
            End If
            Dim retRows As DataRow = dt.Select("Fieldname='GT_BEST' AND OutputField = 1 ")(0)
            If Not retRows Is Nothing Then
                mtblErgebnis = DirectCast(retRows("Data"), DataTable)
            End If

        Catch ex As Exception
        Finally

        End Try
    End Sub

End Class
