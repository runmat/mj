Imports KBSBase

Public Class Zentrallager
    Inherits ErrorHandlingClass

    Private mBestellungen As DataTable
    Private mArtikel As DataTable
    Private mstrKostStelle As String
    Private mstrSendToKost As String = ""
    Private mstrFreitext As String
    Private mFreitextSend As Boolean
    Private mstrKostText As String = ""
    Private mLetzteBestellungen As DataTable

#Region "Properties"

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

    Public Property LetzteBestellungen() As DataTable
        Get
            LetzteBestellungen = mLetzteBestellungen
        End Get
        Set(ByVal value As DataTable)
            mLetzteBestellungen = value
        End Set
    End Property

#End Region

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
        ClearErrorState()

        mFreitextSend = False

        Try
            S.AP.Init("Z_FIL_EFA_PO_ZENTRALE", "I_KOSTL", IIf(bMaster, mstrSendToKost.PadLeft(10, "0"c), mstrKostStelle.PadLeft(10, "0"c)))

            Dim tblSAP As DataTable = S.AP.GetImportTable("GT_PO_ZENT")
            Dim tmpSAPRow As DataRow
            If String.IsNullOrEmpty(OnlyFreitext) Then
                If Bestellungen.Rows.Count = 0 Then
                    RaiseError("9999", "Ausgewählte Artikel konnten nicht gefunden werden!")
                    Exit Sub
                End If
                'mit select filter, da sonst auch deleted rows mitgenommen werden, die dann auf einen fehler laufen JJU20090511
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

            S.AP.Execute()

            If S.AP.ResultCode <> 0 Then
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

    Public Sub ShowERP()
        ClearErrorState()

        Try
            S.AP.Init("Z_FIL_EFA_READ_ARTZENTRALE", "I_KOSTL", mstrKostStelle.PadLeft(10, "0"c))

            S.AP.Execute()

            If S.AP.ResultCode = 0 Then
                mArtikel = S.AP.GetExportTable("GT_ARTZENT")
            Else
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

    Public Sub CheckKostStelleERP(ByVal NeuKost As String)
        ClearErrorState()

        Try
            S.AP.Init("Z_FIL_EFA_GET_KOSTL", "I_KOSTL_SEND, I_KOSTL_RECEIVE", NeuKost.PadLeft(10, "0"c), NeuKost.PadLeft(10, "0"c))

            S.AP.Execute()

            If S.AP.ResultCode = 0 Then
                mstrKostText = S.AP.GetExportParameter("E_KTEXT")
                mstrSendToKost = NeuKost
            Else
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)

                Select Case ErrorCode
                    Case "102"
                        RaiseError("102", "KST " & NeuKost & " ist nicht zulässig! Bitte einen Lieferscheinverkauf eingeben.")
                    Case "104"
                        RaiseError("104", "KST nicht zulässig! Bitte richtige KST eingeben.")
                End Select
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
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

    Public Sub FillLetzteBestellungen(ByVal kst As String)
        ClearErrorState()

        Try
            If mLetzteBestellungen IsNot Nothing Then
                mLetzteBestellungen.Clear()
            End If

            S.AP.Init("Z_FIL_EFA_PO_ZENTRALE_LISTE", "I_KOSTL", kst.PadLeft(10, "0"c))

            S.AP.Execute()

            If S.AP.ResultCode = 0 Or S.AP.ResultCode = 141 Then
                mLetzteBestellungen = S.AP.GetExportTable("GT_LISTE")
                mLetzteBestellungen.Columns.Add("Bestelldatum", GetType(DateTime))
                For Each row As DataRow In mLetzteBestellungen.Rows
                    Dim tmpDate As DateTime
                    If Not String.IsNullOrEmpty(row("BEDAT").ToString()) AndAlso DateTime.TryParse(row("BEDAT").ToString(), tmpDate) Then
                        row("Bestelldatum") = tmpDate
                    End If
                Next
            Else
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

End Class
