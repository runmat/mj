Imports System.Globalization
Imports KBSBase

Public Class Platinen
    Inherits ErrorHandlingClass

    Private mBestellungen As DataTable
    Private mLieferanten As DataTable
    Private mArtikel As DataTable
    Private mReiter As DataTable
    Private mOffBestellungen As DataTable
    Private mstrBedienerNr As String
    Private mstrKostStelle As String
    Private mstrLieferantennr As String
    Private mstrGeliefert As String
    Private mstrLieferscheinnr As String
    Private mLieferdatum As Date?
    Private strSelReiter As String
    Private bvorhandeneBest As Boolean = False
    Private mBestellnummerParken As String = ""
    Private mCommited As Boolean
    Private mstrKostText As String = ""
    Private mLetzteBestellungenKopf As DataTable
    Private mLetzteBestellungenPos As DataTable
    Private mAusgeparkteBestellungPos As DataTable

#Region "Properties"

    Public ReadOnly Property Bestellungen() As DataTable
        Get
            Return mBestellungen
        End Get
    End Property

    Public ReadOnly Property Lieferanten() As DataTable
        Get
            Return mLieferanten
        End Get
    End Property

    Public ReadOnly Property Artikel() As DataTable
        Get
            Return mArtikel
        End Get
    End Property

    Public ReadOnly Property Reiter() As DataTable
        Get
            Return mReiter
        End Get
    End Property

    Public ReadOnly Property OffBestellungen() As DataTable
        Get
            Return mOffBestellungen
        End Get
    End Property

    Public Property KostStelle() As String
        Get
            Return mstrKostStelle
        End Get
        Set(ByVal value As String)
            mstrKostStelle = value
        End Set
    End Property

    Public Property Lieferantennr() As String
        Get
            Return mstrLieferantennr
        End Get
        Set(ByVal value As String)
            mstrLieferantennr = value
        End Set
    End Property

    Public ReadOnly Property BestellnummerParken() As String
        Get
            Return mBestellnummerParken
        End Get
    End Property

    Public Property SelReiter() As String
        Get
            Return strSelReiter
        End Get
        Set(ByVal Value As String)
            strSelReiter = Value
        End Set
    End Property

    Public Property vorhandeneBest() As Boolean
        Get
            Return bvorhandeneBest
        End Get
        Set(ByVal value As Boolean)
            bvorhandeneBest = value
        End Set
    End Property

    Public Property BedienerNr() As String
        Get
            Return mstrBedienerNr
        End Get
        Set(ByVal value As String)
            mstrBedienerNr = value
        End Set
    End Property

    Public Property Commited() As Boolean
        Get
            Return mCommited
        End Get
        Set(ByVal value As Boolean)
            mCommited = value
        End Set
    End Property

    Public Property Geliefert() As String
        Get
            Return mstrGeliefert
        End Get
        Set(value As String)
            mstrGeliefert = value
        End Set
    End Property

    Public Property Lieferscheinnummer() As String
        Get
            Return mstrLieferscheinnr
        End Get
        Set(value As String)
            mstrLieferscheinnr = value
        End Set
    End Property

    Public Property Lieferdatum() As DateTime?
        Get
            Return mLieferdatum
        End Get
        Set(value As DateTime?)
            mLieferdatum = value
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

    Public ReadOnly Property LetzteBestellungenKopf() As DataTable
        Get
            Return mLetzteBestellungenKopf
        End Get
    End Property

    Public ReadOnly Property LetzteBestellungenPos() As DataTable
        Get
            Return mLetzteBestellungenPos
        End Get
    End Property

    Public ReadOnly Property AusgeparkteBestellungPos() As DataTable
        Get
            Return mAusgeparkteBestellungPos
        End Get
    End Property

#End Region

    Public Sub getLieferantenERP(ByVal bMaster As Boolean, Optional ByVal Customername As String = "")
        ClearErrorState()

        Try
            S.AP.Init("Z_FIL_EFA_PLATSTAMM", "I_KOSTL", mstrKostStelle)

            If bMaster Then
                S.AP.SetImportParameter("I_SUPER_USER", "X")
            End If

            If Customername.Contains("ZLD") Then
                S.AP.SetImportParameter("I_ZLD", "X")
            ElseIf Customername.Contains("Kroschke") Then
                S.AP.SetImportParameter("I_FIL", "X")
            End If

            S.AP.Execute()

            If S.AP.ResultCode <> 0 Then
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

            mLieferanten = S.AP.GetExportTable("GT_PLATSTAMM")

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

    Public Sub getOffeneBestellungERP()
        ClearErrorState()

        Try
            S.AP.Init("Z_FIL_READ_OFF_BEST_001", "I_LGORT", mstrKostStelle)

            If Not String.IsNullOrEmpty(mstrLieferantennr) Then
                S.AP.SetImportParameter("I_LIFNR", mstrLieferantennr.PadLeft(10, "0"c))
            End If

            S.AP.Execute()

            If S.AP.ResultCode <> 0 Then
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

            mOffBestellungen = S.AP.GetExportTable("GT_WEB")

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

    Public Sub sendOrderToSAPERP()
        ClearErrorState()

        Try
            Dim selRows() As DataRow = Artikel.Select("Menge > 0")
            If selRows.Length = 0 Then
                RaiseError("0", "Ausgewählte Artikel konnten nicht gefunden werden!")
                Exit Sub
            End If

            S.AP.Init("Z_FIL_EFA_PO_CREATE")

            S.AP.SetImportParameter("I_KOSTL", mstrKostStelle)
            S.AP.SetImportParameter("I_LIFNR", mstrLieferantennr.PadLeft(10, "0"c))
            S.AP.SetImportParameter("I_VERKAEUFER", mstrBedienerNr)

            If mstrGeliefert = "X" Then
                S.AP.SetImportParameter("I_LIEF_KZ", "X")
            End If

            If Not String.IsNullOrEmpty(mstrLieferscheinnr) Then
                S.AP.SetImportParameter("I_LIEF_NR", mstrLieferscheinnr)
            End If

            If mLieferdatum.HasValue Then
                S.AP.SetImportParameter("I_EEIND", mLieferdatum.Value)
            End If

            If Not String.IsNullOrEmpty(mBestellnummerParken) Then
                S.AP.SetImportParameter("I_BSTNR_PARK", mBestellnummerParken.PadLeft(10, "0"c))
            End If

            Dim tblSAP As DataTable = S.AP.GetImportTable("GT_POS")
            'mit select filter, da sonst auch deleted rows mitgenommen werden, die dann auf einen fehler laufen JJU20090511
            For Each tmprow As DataRow In selRows
                Dim tmpSAPRow As DataRow = tblSAP.NewRow
                tmpSAPRow("ARTLIF") = tmprow("ARTLIF").ToString
                tmpSAPRow("MENGE") = tmprow("Menge").ToString
                tmpSAPRow("ZUSINFO_TXT") = tmprow("Beschreibung").ToString
                tblSAP.Rows.Add(tmpSAPRow)
            Next

            S.AP.Execute()

            If S.AP.ResultCode = 0 Then
                mBestellnummerParken = ""
                Commited = True
            Else
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
                Commited = False
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

    Public Sub getArtikelReiterERP()
        ClearErrorState()

        Try
            S.AP.Init("Z_FIL_EFA_PLATARTIKEL")

            S.AP.SetImportParameter("I_KOSTL", mstrKostStelle)
            S.AP.SetImportParameter("I_FIL", "X")
            S.AP.SetImportParameter("I_LIFNR", mstrLieferantennr.PadLeft(10, "0"c))
            S.AP.SetImportParameter("I_RUECKS", "*")

            S.AP.Execute()

            If S.AP.ResultCode <> 0 Then
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

            mReiter = S.AP.GetExportTable("GT_PLATRTR")
            For Each dRow As DataRow In mReiter.Rows
                dRow("REITER") = dRow("REITER").ToString().TrimStart("0")
            Next

            mArtikel = S.AP.GetExportTable("GT_PLATART")
            mArtikel.Columns.Add("Menge", Type.GetType("System.Int32"))
            mArtikel.Columns.Add("SortPos", Type.GetType("System.Int32"))
            mArtikel.Columns.Add("Beschreibung", Type.GetType("System.String"))
            mArtikel.Columns.Add("ImageUrl", Type.GetType("System.String"))
            For Each dRow As DataRow In mArtikel.Rows
                If dRow("TOPSELLER") Is DBNull.Value Then
                    dRow("TOPSELLER") = String.Empty
                End If
                dRow("SortPos") = CInt(dRow("POS"))
                If dRow("ZUSINFO") Is DBNull.Value Then
                    dRow("ZUSINFO") = String.Empty
                End If
                dRow("ImageUrl") = "/kbs/Images/" & dRow("ARTLIF").ToString & ".jpg"
                dRow("REITER") = dRow("REITER").ToString().TrimStart("0")
            Next

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

    Public Sub getLetzteBestellungenERP()
        ClearErrorState()

        Try
            S.AP.Init("Z_FIL_EFA_PO_LISTE", "I_KOSTL, I_LIFNR", mstrKostStelle.PadLeft(10, "0"c), mstrLieferantennr.PadLeft(10, "0"c))

            S.AP.Execute()

            If S.AP.ResultCode <> 0 Then
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

            mLetzteBestellungenKopf = S.AP.GetExportTable("GT_PO_K")
            mLetzteBestellungenKopf.Columns.Add("Bestelldatum", GetType(DateTime))
            mLetzteBestellungenKopf.Columns.Add("Lieferdatum", GetType(DateTime))
            Dim tmpDate As DateTime
            For Each row As DataRow In mLetzteBestellungenKopf.Rows
                If Not String.IsNullOrEmpty(row("BEDAT").ToString()) AndAlso DateTime.TryParse(row("BEDAT").ToString(), tmpDate) Then
                    row("Bestelldatum") = tmpDate
                End If
                If Not String.IsNullOrEmpty(row("EEIND").ToString()) AndAlso DateTime.TryParse(row("EEIND").ToString(), tmpDate) Then
                    row("Lieferdatum") = tmpDate
                End If
            Next

            mLetzteBestellungenPos = S.AP.GetExportTable("GT_PO_P")

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

    Public Function GetListeAusparkenERP() As DataTable
        ClearErrorState()

        Dim tblListeAusparken As New DataTable()

        Try
            S.AP.Init("Z_FIL_EFA_PO_PARK_LISTE", "I_KOSTL", mstrKostStelle.PadLeft(10, "0"c))

            S.AP.Execute()

            If S.AP.ResultCode <> 0 AndAlso S.AP.ResultCode <> 101 Then
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

            tblListeAusparken = S.AP.GetExportTable("GT_LISTE")
            tblListeAusparken.Columns.Add("Lieferant")
            For Each row As DataRow In tblListeAusparken.Rows
                Dim liefRows() As DataRow = Lieferanten.Select("LIFNR = '" & row("LIFNR") & "'")
                If liefRows.Length > 0 Then
                    row("Lieferant") = liefRows(0)("NAME1")
                End If
            Next

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try

        Return tblListeAusparken
    End Function

    Public Sub ParkenERP(ByVal lieferant As String)
        ClearErrorState()

        Try
            Dim selRows() As DataRow = Artikel.Select("Menge > 0")

            If selRows.Length = 0 Then
                RaiseError("0001", "Keine Datensätze für Bestellung vorhanden!")
                Exit Sub
            End If

            If String.IsNullOrEmpty(mBestellnummerParken) Then
                'Insert
                S.AP.Init("Z_FIL_EFA_PO_PARK_INS")

                S.AP.SetImportParameter("I_KOSTL", mstrKostStelle.PadLeft(10, "0"c))
                S.AP.SetImportParameter("I_LIFNR", lieferant.PadLeft(10, "0"c))
                S.AP.SetImportParameter("I_LIEF_KZ", Geliefert)
                S.AP.SetImportParameter("I_LIEF_NR", Lieferscheinnummer)

                If mLieferdatum.HasValue Then
                    S.AP.SetImportParameter("I_EEIND", Lieferdatum.Value)
                End If

                Dim tblPos As DataTable = S.AP.GetImportTable("GT_POS")
                For Each row As DataRow In selRows
                    Dim tRow As DataRow = tblPos.NewRow()
                    tRow("ARTLIF") = row("ARTLIF")
                    tRow("MENGE") = row("MENGE")
                    tRow("ZUSINFO_TXT") = row("Beschreibung")
                    tblPos.Rows.Add(tRow)
                Next
            Else
                'Update
                S.AP.Init("Z_FIL_EFA_PO_PARK_UPD")

                Dim tblKopf As DataTable = S.AP.GetImportTable("IS_KOPF")
                Dim kRow As DataRow = tblKopf.NewRow()
                kRow("BSTNR") = mBestellnummerParken.PadLeft(10, "0"c)
                kRow("LIFNR") = lieferant.PadLeft(10, "0"c)
                kRow("KOSTL") = mstrKostStelle.PadLeft(10, "0"c)
                kRow("LIEFERSNR") = Lieferscheinnummer
                kRow("LIEF_KZ") = Geliefert
                If mLieferdatum.HasValue Then
                    kRow("EEIND") = Lieferdatum.Value.ToShortDateString()
                End If
                tblKopf.Rows.Add(kRow)

                Dim tblPos As DataTable = S.AP.GetImportTable("GT_POS")
                For Each row As DataRow In selRows
                    Dim tRow As DataRow = tblPos.NewRow()
                    tRow("BSTNR") = mBestellnummerParken.PadLeft(10, "0"c)
                    tRow("ARTLIF") = row("ARTLIF")
                    tRow("MENGE") = row("MENGE")
                    tRow("ZUSINFO_TXT") = row("Beschreibung")
                    tblPos.Rows.Add(tRow)
                Next
            End If

            S.AP.Execute()

            If S.AP.ResultCode = 0 Then
                For Each row As DataRow In selRows
                    row("MENGE") = DBNull.Value
                    row("Beschreibung") = DBNull.Value
                Next
                Artikel.AcceptChanges()
                mstrGeliefert = ""
                mstrLieferscheinnr = ""
                mLieferdatum = Nothing
                mBestellnummerParken = ""
            Else
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

    Public Function AusparkenERP(ByVal BstNr As String) As DataTable
        ClearErrorState()

        Try
            S.AP.Init("Z_FIL_EFA_PO_PARK_READ", "I_BSTNR, I_KOSTL", BstNr.PadLeft(10, "0"c), mstrKostStelle.PadLeft(10, "0"c))

            S.AP.Execute()

            If S.AP.ResultCode = 0 Then
                Dim tblTemp As DataTable = S.AP.GetExportTable("E_KOPF")
                If tblTemp.Rows.Count > 0 Then
                    mBestellnummerParken = tblTemp.Rows(0)("BSTNR")
                    mstrLieferantennr = tblTemp.Rows(0)("LIFNR")
                    mstrGeliefert = tblTemp.Rows(0)("LIEF_KZ")
                    mstrLieferscheinnr = tblTemp.Rows(0)("LIEFERSNR")
                    Dim tmpDate As DateTime
                    If Not IsDBNull(tblTemp.Rows(0)("EEIND")) AndAlso Not String.IsNullOrEmpty(tblTemp.Rows(0)("EEIND").ToString()) AndAlso DateTime.TryParseExact(tblTemp.Rows(0)("EEIND"), "yyyyMMdd", CultureInfo.CurrentCulture, DateTimeStyles.None, tmpDate) Then
                        mLieferdatum = tmpDate
                    Else
                        mLieferdatum = Nothing
                    End If
                End If

                Dim selRows() As DataRow = Artikel.Select("Menge > 0")
                For Each row As DataRow In selRows
                    row("MENGE") = DBNull.Value
                Next

                mAusgeparkteBestellungPos = S.AP.GetExportTable("GT_POS")
            Else
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try

        Return mArtikel
    End Function

    Public Sub GeparktePositionenUebernehmen()
        For Each row As DataRow In mAusgeparkteBestellungPos.Rows
            Dim posRows() As DataRow = Artikel.Select("ARTLIF = '" & row("ARTLIF").ToString() & "'")
            If posRows.Length > 0 Then
                posRows(0)("MENGE") = row("MENGE")
                posRows(0)("Beschreibung") = row("ZUSINFO_TXT")
            End If
        Next
    End Sub

    Public Sub GeparktLoeschenERP(ByVal BstNr As String, ByVal TextDel As String)
        ClearErrorState()

        Try
            If Not String.IsNullOrEmpty(mBestellnummerParken) AndAlso mBestellnummerParken.PadLeft(10, "0"c) = BstNr.PadLeft(10, "0"c) Then
                mBestellnummerParken = ""
            End If

            S.AP.Init("Z_FIL_EFA_PO_PARK_DEL", "I_BSTNR, I_TEXTDEL", BstNr.PadLeft(10, "0"c), TextDel)

            S.AP.Execute()

            If S.AP.ResultCode <> 0 Then
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

End Class
