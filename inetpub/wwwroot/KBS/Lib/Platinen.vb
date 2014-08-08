
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

    Dim SAPExc As SAPExecutor.SAPExecutor

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

    Public Property BestellnummerParken() As String
        Get
            Return mBestellnummerParken
        End Get
        Set(ByVal value As String)
            mBestellnummerParken = value
        End Set
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

#End Region

    Public Sub New()

    End Sub

    Public Sub getLieferantenERP(ByVal bMaster As Boolean, Optional ByVal Customername As String = "")

        ClearErrorState()

        Try
            SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)
            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()

            dt.Rows.Add(New Object() {"I_KOSTL", False, mstrKostStelle, 10})
            If bMaster = True Then
                dt.Rows.Add(New Object() {"I_SUPER_USER", False, "X", 1})
            Else
                dt.Rows.Add(New Object() {"I_SUPER_USER", False, "", 1})
            End If
            Dim IsFil As String = ""
            Dim IsZLD As String = ""

            If Customername.Contains("ZLD") Then
                IsZLD = "X"
            ElseIf Customername.Contains("Kroschke") Then
                IsFil = "X"
            End If
            dt.Rows.Add(New Object() {"I_FIL", False, IsFil, 1})
            dt.Rows.Add(New Object() {"I_ZLD", False, IsZLD, 1})
            dt.Rows.Add(New Object() {"GT_PLATSTAMM", True})

            SAPExc.ExecuteERP("Z_FIL_EFA_PLATSTAMM", dt)

            If (SAPExc.ErrorOccured) Then
                RaiseError(SAPExc.E_SUBRC, SAPExc.E_MESSAGE)
            End If

            Dim retRows As DataRow = dt.Select("Fieldname='GT_PLATSTAMM'")(0)
            If Not retRows Is Nothing Then
                mLieferanten = DirectCast(retRows("Data"), DataTable)
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try

    End Sub

    Public Sub getOffeneBestellungERP()

        ClearErrorState()

        Try
            SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)
            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()
            'befüllen der Importparameter
            dt.Rows.Add(New Object() {"I_LGORT", False, mstrKostStelle, 4})
            dt.Rows.Add(New Object() {"I_LIFNR", False, mstrLieferantennr.PadLeft(10, "0"c), 10})
            dt.Rows.Add(New Object() {"GT_WEB", True})

            SAPExc.ExecuteERP("Z_FIL_READ_OFF_BEST_001", dt)

            If (SAPExc.ErrorOccured) Then
                RaiseError(SAPExc.E_SUBRC, SAPExc.E_MESSAGE)
            End If
            Dim retRows As DataRow = dt.Select("Fieldname='GT_WEB'")(0)
            If Not retRows Is Nothing Then
                mOffBestellungen = DirectCast(retRows("Data"), DataTable)
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

    Public Sub sendOrderToSAPERP()

        ClearErrorState()

        Dim tblSAP As New DataTable()
        tblSAP.Columns.Add("ARTLIF", String.Empty.GetType)
        tblSAP.Columns.Add("MENGE", String.Empty.GetType)
        tblSAP.Columns.Add("ZUSINFO_TXT", String.Empty.GetType)
        tblSAP.Columns.Add("PREIS", String.Empty.GetType)
        tblSAP.Columns.Add("LTEXT_NR", String.Empty.GetType)

        Dim tmpSAPRow As DataRow
        Dim selRows() As DataRow
        selRows = Artikel.Select("Menge > 0")
        If selRows.Length = 0 Then
            RaiseError("0", "Ausgewählte Artikel konnten nicht gefunden werden!")
            Exit Sub
        End If
        'mit select filter, da sonst auch deletet rows mitgenommen werden, die dann auf einen fehler laufen JJU20090511
        For Each tmprow As DataRow In selRows
            tmpSAPRow = tblSAP.NewRow
            tmpSAPRow("ARTLIF") = tmprow("ARTLIF").ToString
            tmpSAPRow("MENGE") = tmprow("Menge").ToString
            tmpSAPRow("ZUSINFO_TXT") = tmprow("Beschreibung").ToString
            tblSAP.Rows.Add(tmpSAPRow)
        Next

        Try
            SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)
            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()
            'befüllen der Importparameter
            dt.Rows.Add(New Object() {"I_KOSTL", False, mstrKostStelle, 4})
            dt.Rows.Add(New Object() {"I_LIFNR", False, mstrLieferantennr.PadLeft(10, "0"c), 10})
            dt.Rows.Add(New Object() {"I_VERKAEUFER", False, mstrBedienerNr, 4})
            If mstrGeliefert = "X" Then
                dt.Rows.Add(New Object() {"I_LIEF_KZ", False, mstrGeliefert, 1})
            Else
                dt.Rows.Add(New Object() {"I_LIEF_KZ", False, "", 1})
            End If
            If mstrLieferscheinnr.Length > 0 Then
                dt.Rows.Add(New Object() {"I_LIEF_NR", False, mstrLieferscheinnr.ToString(), 20})
            Else
                dt.Rows.Add(New Object() {"I_LIEF_NR", False, "", 20})
            End If
            If mLieferdatum.HasValue Then
                dt.Rows.Add(New Object() {"I_EEIND", False, mLieferdatum.Value, 8})
            End If

            dt.Rows.Add(New Object() {"GT_POS", False, tblSAP})

            SAPExc.ExecuteERP("Z_FIL_EFA_PO_CREATE", dt)
            Commited = True

            If (SAPExc.ErrorOccured) Then
                RaiseError(SAPExc.E_SUBRC, SAPExc.E_MESSAGE)
                Commited = False
            End If
        Catch ex As Exception
            RaiseError("0", ex.Message)
        End Try

    End Sub

    Public Sub getArtikelReiterERP()

        ClearErrorState()

        Try
            SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)
            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()
            'befüllen der Importparameter
            dt.Rows.Add(New Object() {"I_KOSTL", False, mstrKostStelle, 4})
            dt.Rows.Add(New Object() {"I_LIFNR", False, mstrLieferantennr.PadLeft(10, "0"c), 10})
            dt.Rows.Add(New Object() {"I_RUECKS", False, "*", 1})

            dt.Rows.Add(New Object() {"GT_PLATART", True})
            dt.Rows.Add(New Object() {"GT_PLATRTR", True})

            SAPExc.ExecuteERP("Z_FIL_EFA_PLATARTIKEL", dt)

            If (SAPExc.ErrorOccured) Then
                RaiseError(SAPExc.E_SUBRC, SAPExc.E_MESSAGE)
            End If

            Dim retRows As DataRow = dt.Select("Fieldname='GT_PLATRTR'")(0)
            If Not retRows Is Nothing Then
                mReiter = DirectCast(retRows("Data"), DataTable)
                For Each dRow As DataRow In mReiter.Rows
                    dRow("REITER") = dRow("REITER").ToString().TrimStart("0")
                Next
            End If
            retRows = dt.Select("Fieldname='GT_PLATART'")(0)
            If Not retRows Is Nothing Then
                mArtikel = DirectCast(retRows("Data"), DataTable)
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
            End If

        Catch ex As Exception
            RaiseError("0", ex.Message)
        End Try

    End Sub

    Public Sub CheckKostStelleERP(ByVal NeuKost As String)

        ClearErrorState()

        Try
            SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)
            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()

            dt.Rows.Add(New Object() {"I_KOSTL_SEND", False, NeuKost.PadLeft(10, "0"c), 10})
            dt.Rows.Add(New Object() {"I_KOSTL_RECEIVE", False, NeuKost.PadLeft(10, "0"c), 10})
            dt.Rows.Add(New Object() {"E_KOSTL", True})
            dt.Rows.Add(New Object() {"E_KTEXT", True})
            dt.Rows.Add(New Object() {"E_LTEXT", True})

            SAPExc.ExecuteERP("Z_FIL_EFA_GET_KOSTL", dt)

            If (SAPExc.ErrorOccured) Then
                Select Case SAPExc.E_SUBRC
                    Case "102"
                        RaiseError(SAPExc.E_SUBRC, "KST " & NeuKost & " ist nicht zulässig! Bitte einen Lieferscheinverkauf eingeben.")
                    Case "104"
                        RaiseError(SAPExc.E_SUBRC, "KST nicht zulässig! Bitte richtige KST eingeben.")
                    Case Else
                        RaiseError(SAPExc.E_SUBRC, SAPExc.E_MESSAGE)
                End Select

            Else
                Dim retRows As DataRow = dt.Select("Fieldname='E_KTEXT'")(0)
                If Not retRows Is Nothing Then
                    mstrKostText = retRows("Data").ToString
                End If
            End If

        Catch ex As Exception
            RaiseError("0", ex.Message)
        End Try

    End Sub

    Public Sub getLetzteBestellungenERP()

        ClearErrorState()

        Try
            SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)
            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()
            'befüllen der Importparameter
            dt.Rows.Add(New Object() {"I_KOSTL", False, mstrKostStelle.PadLeft(10, "0"c), 10})
            dt.Rows.Add(New Object() {"I_LIFNR", False, mstrLieferantennr.PadLeft(10, "0"c), 10})

            dt.Rows.Add(New Object() {"GT_PO_K", True})
            dt.Rows.Add(New Object() {"GT_PO_P", True})

            SAPExc.ExecuteERP("Z_FIL_EFA_PO_LISTE", dt)

            If (SAPExc.ErrorOccured) Then
                RaiseError(SAPExc.E_SUBRC, SAPExc.E_MESSAGE)
            End If

            Dim retRowsKopf As DataRow = dt.Select("Fieldname='GT_PO_K'")(0)
            If Not retRowsKopf Is Nothing Then
                mLetzteBestellungenKopf = DirectCast(retRowsKopf("Data"), DataTable)
            End If
            Dim retRowsPos As DataRow = dt.Select("Fieldname='GT_PO_P'")(0)
            If Not retRowsPos Is Nothing Then
                mLetzteBestellungenPos = DirectCast(retRowsPos("Data"), DataTable)
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

    Public Function GetListeAusparkenERP() As DataTable
        ClearErrorState()
        Dim tblListeAusparken As New DataTable()

        Try
            SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)

            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()

            dt.Rows.Add(New Object() {"I_KOSTL", False, mstrKostStelle.PadLeft(10, "0"c), 10})
            dt.Rows.Add(New Object() {"GT_LISTE", True})

            SAPExc.ExecuteERP("Z_FIL_EFA_PO_PARK_LISTE", dt)

            If (SAPExc.ErrorOccured) Then
                Select Case SAPExc.E_SUBRC
                    Case "101"
                        'E_MESSAGE = ""
                    Case Else
                        RaiseError(SAPExc.E_SUBRC, SAPExc.E_MESSAGE)
                End Select
            End If

            Dim retRows As DataRow = dt.Select("Fieldname='GT_LISTE'")(0)

            If Not retRows Is Nothing Then
                tblListeAusparken = DirectCast(retRows("Data"), DataTable)
                tblListeAusparken.Columns.Add("Lieferant")
                For Each row As DataRow In tblListeAusparken.Rows
                    Dim liefRows() As DataRow = Lieferanten.Select("LIFNR = '" & row("LIFNR") & "'")
                    If liefRows.Length > 0 Then
                        row("Lieferant") = liefRows(0)("NAME1")
                    End If
                Next
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try

        Return tblListeAusparken
    End Function

    Public Sub ParkenERP(ByVal lieferant As String)
        ClearErrorState()

        Try
            SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)

            Dim selRows() As DataRow = Artikel.Select("Menge > 0")

            If selRows.Length > 0 Then
                Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()

                If String.IsNullOrEmpty(mBestellnummerParken) Then

                    'Insert
                    dt.Rows.Add(New Object() {"I_KOSTL", False, mstrKostStelle.PadLeft(10, "0"c), 10})
                    dt.Rows.Add(New Object() {"I_LIFNR", False, lieferant.PadLeft(10, "0"c), 10})
                    dt.Rows.Add(New Object() {"I_LIEF_KZ", False, Geliefert, 1})
                    dt.Rows.Add(New Object() {"I_LIEF_NR", False, Lieferscheinnummer, 20})
                    If mLieferdatum.HasValue Then
                        dt.Rows.Add(New Object() {"I_EEIND", False, Lieferdatum.Value, 8})
                    End If

                    Dim tblTemp As New DataTable
                    tblTemp.Columns.Add("ARTLIF")
                    tblTemp.Columns.Add("MENGE")
                    tblTemp.Columns.Add("ZUSINFO_TXT")
                    tblTemp.Columns.Add("PREIS")
                    tblTemp.Columns.Add("LTEXT_NR")

                    For Each row As DataRow In selRows
                        Dim tRow As DataRow = tblTemp.NewRow()
                        tRow("ARTLIF") = row("ARTLIF")
                        tRow("MENGE") = row("MENGE")
                        tRow("ZUSINFO_TXT") = row("Beschreibung")
                        tblTemp.Rows.Add(tRow)
                    Next
                    dt.Rows.Add(New Object() {"GT_POS", False, tblTemp})
                    dt.Rows.Add(New Object() {"E_BSTNR", True})

                    SAPExc.ExecuteERP("Z_FIL_EFA_PO_PARK_INS", dt)
                Else

                    'Update
                    Dim tblKopf As New DataTable
                    tblKopf.Columns.Add("BSTNR")
                    tblKopf.Columns.Add("LIFNR")
                    tblKopf.Columns.Add("KOSTL")
                    tblKopf.Columns.Add("LIEFERSNR")
                    tblKopf.Columns.Add("LIEF_KZ")
                    tblKopf.Columns.Add("EEIND")

                    Dim kRow As DataRow = tblKopf.NewRow()
                    kRow("BSTNR") = mBestellnummerParken.PadLeft(10, "0"c)
                    kRow("LIFNR") = lieferant.PadLeft(10, "0"c)
                    kRow("KOSTL") = mstrKostStelle.PadLeft(10, "0"c)
                    kRow("LIEFERSNR") = Lieferscheinnummer
                    kRow("LIEF_KZ") = Geliefert
                    If mLieferdatum.HasValue Then
                        kRow("EEIND") = Lieferdatum.Value
                    Else
                        kRow("EEIND") = "00000000"
                    End If
                    tblKopf.Rows.Add(kRow)

                    dt.Rows.Add(New Object() {"IS_KOPF", False, tblKopf})

                    Dim tblTemp As New DataTable
                    tblTemp.Columns.Add("BSTNR")
                    tblTemp.Columns.Add("ARTLIF")
                    tblTemp.Columns.Add("MENGE")
                    tblTemp.Columns.Add("ZUSINFO_TXT")
                    tblTemp.Columns.Add("PREIS")

                    For Each row As DataRow In selRows
                        Dim tRow As DataRow = tblTemp.NewRow()
                        tRow("BSTNR") = mBestellnummerParken.PadLeft(10, "0"c)
                        tRow("ARTLIF") = row("ARTLIF")
                        tRow("MENGE") = row("MENGE")
                        tRow("ZUSINFO_TXT") = row("Beschreibung")
                        tblTemp.Rows.Add(tRow)
                    Next
                    dt.Rows.Add(New Object() {"GT_POS", False, tblTemp})

                    SAPExc.ExecuteERP("Z_FIL_EFA_PO_PARK_UPD", dt)
                End If

                If (SAPExc.ErrorOccured) Then
                    RaiseError(SAPExc.E_SUBRC, SAPExc.E_MESSAGE)
                Else
                    For Each row As DataRow In selRows
                        row("MENGE") = DBNull.Value
                        row("Beschreibung") = DBNull.Value
                    Next
                    Artikel.AcceptChanges()
                    mstrGeliefert = ""
                    mstrLieferscheinnr = ""
                    mLieferdatum = Nothing
                    mBestellnummerParken = ""
                End If

            Else
                RaiseError("0001", "Keine Datensätze für Bestellung vorhanden!")
            End If
        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try

    End Sub

    Public Function AusparkenERP(ByVal BstNr As String) As DataTable
        ClearErrorState()

        Try
            SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)
            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()

            dt.Rows.Add(New Object() {"I_BSTNR", False, BstNr.PadLeft(10, "0"c), 10})
            dt.Rows.Add(New Object() {"I_KOSTL", False, mstrKostStelle.PadLeft(10, "0"c), 10})
            dt.Rows.Add(New Object() {"E_KOPF", True})
            dt.Rows.Add(New Object() {"GT_POS", True})
            SAPExc.ExecuteERP("Z_FIL_EFA_PO_PARK_READ", dt)

            If (SAPExc.ErrorOccured) Then
                RaiseError(SAPExc.E_SUBRC, SAPExc.E_MESSAGE)
            Else
                Dim retRows As DataRow = dt.Select("Fieldname='E_KOPF'")(0)
                Dim tblTemp As DataTable
                If Not retRows Is Nothing Then
                    tblTemp = DirectCast(retRows("Data"), DataTable)
                    If tblTemp.Rows.Count > 0 Then
                        mBestellnummerParken = tblTemp.Rows(0)("BSTNR")
                        mstrLieferantennr = tblTemp.Rows(0)("LIFNR")
                        mstrGeliefert = tblTemp.Rows(0)("LIEF_KZ")
                        mstrLieferscheinnr = tblTemp.Rows(0)("LIEFERSNR")
                        Dim tmpDate As DateTime
                        If tblTemp.Rows(0)("EEIND") IsNot Nothing AndAlso Not IsDBNull(tblTemp.Rows(0)("EEIND")) AndAlso DateTime.TryParse(tblTemp.Rows(0)("EEIND"), tmpDate) Then
                            mLieferdatum = tmpDate
                        Else
                            mLieferdatum = Nothing
                        End If
                    End If
                End If

                Dim selRows() As DataRow = Artikel.Select("Menge > 0")
                For Each row As DataRow In selRows
                    row("MENGE") = DBNull.Value
                Next

                Dim tblTemp2 As New DataTable
                retRows = dt.Select("Fieldname='GT_POS'")(0)
                If Not retRows Is Nothing Then
                    tblTemp2 = DirectCast(retRows("Data"), DataTable)
                End If

                For Each row As DataRow In tblTemp2.Rows
                    Dim posRows() As DataRow = Artikel.Select("ARTLIF = '" & row("ARTLIF").ToString() & "'")
                    If posRows.Length > 0 Then
                        posRows(0)("MENGE") = row("MENGE")
                        posRows(0)("Beschreibung") = row("ZUSINFO_TXT")
                    End If
                Next
            End If
        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try

        Return mArtikel

    End Function

    Public Sub GeparktLoeschenERP(ByVal BstNr As String, ByVal TextDel As String)
        ClearErrorState()

        Try
            SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)
            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()

            dt.Rows.Add(New Object() {"I_BSTNR", False, BstNr.PadLeft(10, "0"c), 10})
            dt.Rows.Add(New Object() {"I_TEXTDEL", False, TextDel, 1})

            SAPExc.ExecuteERP("Z_FIL_EFA_PO_PARK_DEL", dt)
            If (SAPExc.ErrorOccured) Then
                RaiseError(SAPExc.E_SUBRC, SAPExc.E_MESSAGE)
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

End Class
