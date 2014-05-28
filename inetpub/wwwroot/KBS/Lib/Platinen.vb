
Public Class Platinen
    Inherits ErrorHandlingClass

    Private mBestellungen As DataTable
    Private mLieferanten As DataTable
    Private mArtikel As DataTable
    Private mReiter As DataTable
    Private mOffBestellungen As DataTable
    Private mstrBedienerNr As String
    Private mstrKostStelle As String
    Private mstrGeliefert As String
    Private mstrLieferscheinnr As String
    Private mLieferdatum As Date
    Private strSelReiter As String
    Private strSelLief As Integer
    Private bvorhandeneBest As Boolean = False
    Private mNummer As String
    Private mCommited As Boolean
    Private mstrSendToKost As String = ""
    Private mstrKostText As String = ""

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
            KostStelle = mstrKostStelle
        End Get
        Set(ByVal value As String)
            mstrKostStelle = value
        End Set
    End Property

    Public Property Nummer() As String
        Get
            Nummer = mNummer
        End Get
        Set(ByVal value As String)
            mNummer = value
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

    Public Property SelLief() As Integer
        Get
            Return strSelLief
        End Get
        Set(ByVal Value As Integer)
            strSelLief = Value
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

    Public Property Lieferdatum() As Date
        Get
            Return mLieferdatum
        End Get
        Set(value As Date)
            mLieferdatum = value
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

#End Region

    Public Sub New()

    End Sub

    Public Sub getLieferantenERP(ByVal Kost As String, ByVal bMaster As Boolean, Optional ByVal Customername As String = "")

        ClearErrorState()

        Try
            SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)
            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()

            dt.Rows.Add(New Object() {"I_KOSTL", False, Kost, 10})
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

    Public Sub getOffeneBestellungERP(ByVal sLifnr As String)

        ClearErrorState()

        Try
            SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)
            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()
            'befüllen der Importparameter
            If mstrSendToKost <> "" Then
                dt.Rows.Add(New Object() {"I_LGORT", False, mstrSendToKost, 4})
            Else
                dt.Rows.Add(New Object() {"I_LGORT", False, mstrKostStelle, 4})
            End If
            dt.Rows.Add(New Object() {"I_LIFNR", False, sLifnr, 10})
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

    Public Sub sendOrderToSAPERP(ByVal sLifnr As String, ByVal bMaster As Boolean)

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
            If mstrSendToKost <> "" Then
                dt.Rows.Add(New Object() {"I_KOSTL", False, mstrSendToKost, 4})
            Else
                dt.Rows.Add(New Object() {"I_KOSTL", False, mstrKostStelle, 4})
            End If
            dt.Rows.Add(New Object() {"I_LIFNR", False, sLifnr, 10})
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
            If Date.Compare(mLieferdatum, Now) >= 0 Then
                dt.Rows.Add(New Object() {"I_EEIND", False, mLieferdatum, 8})
            Else
                dt.Rows.Add(New Object() {"I_EEIND", False, "00000000", 8})
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

    Public Sub getArtikelReiterERP(ByVal sLifnr As String)

        ClearErrorState()

        Try
            SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)
            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()
            'befüllen der Importparameter
            If mstrSendToKost <> "" Then
                dt.Rows.Add(New Object() {"I_KOSTL", False, mstrSendToKost, 4})
            Else
                dt.Rows.Add(New Object() {"I_KOSTL", False, mstrKostStelle, 4})
            End If
            dt.Rows.Add(New Object() {"I_LIFNR", False, sLifnr, 10})
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

            dt.Rows.Add(New Object() {"I_KOSTL_SEND", False, Right("0000000000" & NeuKost, 10), 10})
            dt.Rows.Add(New Object() {"I_KOSTL_RECEIVE", False, Right("0000000000" & NeuKost, 10), 10})
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

End Class
