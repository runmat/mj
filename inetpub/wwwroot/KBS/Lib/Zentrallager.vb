
Public Class Zentrallager
    Private mE_SUBRC As Integer
    Private mE_MESSAGE As String
    Private mBestellungen As DataTable
    Private mArtikel As DataTable
    Private mstrKostStelle As String
    Private mstrSendToKost As String = ""
    Private mstrFreitext As String
    Private mFreitextSend As Boolean
    Private mstrKostText As String = ""
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

    Public ReadOnly Property Artikel() As DataTable
        Get
            Return mArtikel
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

    Public Property Freitext() As String
        Get
            Return mstrFreitext
        End Get
        Set(ByVal value As String)
            mstrFreitext = value
        End Set
    End Property

    Public Property FreitextSend() As Boolean
        Get
            Return mFreitextSend
        End Get
        Set(ByVal value As Boolean)
            mFreitextSend = value
        End Set
    End Property

    Public Property SendToKost() As String
        Get
            Return mstrSendToKost
        End Get
        Set(ByVal value As String)
            mstrSendToKost = value
        End Set
    End Property

    Public Property KostText() As String
        Get
            Return mstrKostText
        End Get
        Set(ByVal value As String)
            mstrKostText = value
        End Set
    End Property

    Public Sub New()
        If mBestellungen Is Nothing Then
            mBestellungen = New DataTable
            mBestellungen.Columns.Add("ARTLIF", String.Empty.GetType)
            mBestellungen.Columns.Add("ARTBEZ", String.Empty.GetType)
            mBestellungen.Columns.Add("Menge", Type.GetType("System.Int32"))
            mBestellungen.Columns.Add("VMEINS", String.Empty.GetType)
            mBestellungen.Columns.Add("FREITEXT", String.Empty.GetType)
        End If

    End Sub

    Public Sub ChangeERP(ByVal bMaster As Boolean, Optional ByVal OnlyFreitext As String = "")

        E_MESSAGE = ""
        E_SUBRC = 0
        mFreitextSend = False
        Dim tblSAP As New DataTable()
        tblSAP.Columns.Add("ARTLIF", String.Empty.GetType)
        tblSAP.Columns.Add("MENGE", String.Empty.GetType)
        tblSAP.Columns.Add("FREITEXT", String.Empty.GetType)

        Dim tmpSAPRow As DataRow
        If OnlyFreitext = "" Then
            If Bestellungen.Rows.Count = 0 Then
                E_MESSAGE = "Ausgewählte Artikel konnten nicht gefunden werden!"
                Exit Sub
            End If
            'mit select filter, da sonst auch deletet rows mitgenommen werden, die dann auf einen fehler laufen JJU20090511
            For Each tmprow As DataRow In Bestellungen.Rows
                tmpSAPRow = tblSAP.NewRow
                tmpSAPRow("ARTLIF") = tmprow("ARTLIF").ToString
                tmpSAPRow("MENGE") = tmprow("Menge").ToString
                tmpSAPRow("FREITEXT") = tmprow("FREITEXT").ToString
                tblSAP.Rows.Add(tmpSAPRow)
            Next

        Else
            tmpSAPRow = tblSAP.NewRow
            tmpSAPRow("ARTLIF") = ""
            tmpSAPRow("MENGE") = ""
            tmpSAPRow("FREITEXT") = mstrFreitext
            tblSAP.Rows.Add(tmpSAPRow)
            mFreitextSend = True
        End If

        SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)

        Try

            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()
            If bMaster = True Then
                dt.Rows.Add(New Object() {"I_KOSTL", False, Right("0000000000" & mstrSendToKost, 10), 10})
            Else
                dt.Rows.Add(New Object() {"I_KOSTL", False, Right("0000000000" & mstrKostStelle, 10), 10})
            End If

            dt.Rows.Add(New Object() {"GT_PO_ZENT", False, tblSAP})

            SAPExc.ExecuteERP("Z_FIL_EFA_PO_ZENTRALE", dt)

            If (SAPExc.ErrorOccured) Then
                E_SUBRC = SAPExc.E_SUBRC
                E_MESSAGE = SAPExc.E_MESSAGE
            End If

        Catch ex As Exception
            E_MESSAGE = ex.Message
        Finally

        End Try
    End Sub

    Public Sub ShowERP()
        SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)
        E_MESSAGE = ""
        E_SUBRC = 0

        Try

            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()

            dt.Rows.Add(New Object() {"I_KOSTL", False, Right("0000000000" & mstrKostStelle, 10), 10})
            dt.Rows.Add(New Object() {"GT_ARTZENT", True})

            SAPExc.ExecuteERP("Z_FIL_EFA_READ_ARTZENTRALE", dt)

            If (SAPExc.ErrorOccured) Then
                E_SUBRC = SAPExc.E_SUBRC
                E_MESSAGE = SAPExc.E_MESSAGE
            Else
                Dim retRows As DataRow = dt.Select("Fieldname='GT_ARTZENT'")(0)
                If Not retRows Is Nothing Then
                    mArtikel = DirectCast(retRows("Data"), DataTable)
                End If
            End If

        Catch ex As Exception

        Finally

        End Try

    End Sub

    Public Sub CheckKostStelleERP(ByVal NeuKost As String)
        SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)
        E_MESSAGE = ""
        E_SUBRC = 0

        Try

            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()

            dt.Rows.Add(New Object() {"I_KOSTL_SEND", False, Right("0000000000" & NeuKost, 10), 10})
            dt.Rows.Add(New Object() {"I_KOSTL_RECEIVE", False, Right("0000000000" & NeuKost, 10), 10})
            dt.Rows.Add(New Object() {"E_KOSTL", True})
            dt.Rows.Add(New Object() {"E_KTEXT", True})
            dt.Rows.Add(New Object() {"E_LTEXT", True})

            SAPExc.ExecuteERP("Z_FIL_EFA_GET_KOSTL", dt)

            If (SAPExc.ErrorOccured) Then
                E_SUBRC = SAPExc.E_SUBRC
                E_MESSAGE = SAPExc.E_MESSAGE
            Else
                Dim retRows As DataRow = dt.Select("Fieldname='E_KTEXT'")(0)
                If Not retRows Is Nothing Then
                    mstrKostText = retRows("Data").ToString
                    mstrSendToKost = NeuKost
                End If
            End If

            Select Case E_SUBRC
                Case 102
                    E_MESSAGE = "KST " & NeuKost & " ist nicht zulässig! Bitte einen Lieferscheinverkauf eingeben."
                Case 104
                    E_MESSAGE = "KST nicht zulässig! Bitte richtige KST eingeben."

            End Select

        Catch ex As Exception

        Finally

        End Try

    End Sub

    Public Sub insertIntoBestellungen(ByVal Artikelnr As String, ByVal Menge As Integer, ByVal Artikelbezeichnung As String, ByVal VerpackEinheit As String)
        Dim tmpRow As DataRow = mBestellungen.NewRow
        tmpRow.Item("ARTLIF") = Artikelnr
        tmpRow.Item("ARTBEZ") = Artikelbezeichnung
        tmpRow("Menge") = Menge
        tmpRow("VMEINS") = VerpackEinheit
        mBestellungen.Rows.Add(tmpRow)
    End Sub

End Class
