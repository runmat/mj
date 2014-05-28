
Public Class Retoure
    Private mRetoure As DataTable
    Private mLieferanten As DataTable
    Private mArtikel As DataTable
    Private mGrund As DataTable
    Private mMyKasse As Kasse
    Private WithEvents mTimer As New Timers.Timer
    Private mSAPMessage As String
    Private mLieferscheinnummer As String
    Private mKostenstelle As String
    Private mVerkaeufer As String
    Private mE_SUBRC As Integer
    Private mE_MESSAGE As String
    Private mE_BSTNR As String
    Private mE_BELNR As String
    Private mCommited As Boolean = False
    Private mSelLief As Integer
    Private mSelArtikel As Integer
    Private mRetoureindex As Int32 = 0
    Private mIsADM As Boolean = False
    Private mFilepath As String
    Private mstrSendToKost As String = ""
    Private mstrKostText As String = ""
    Private mstrKostStelle As String
    Dim SAPExc As SAPExecutor.SAPExecutor

    Public Property Filepath() As String
        Get
            Return mFilepath
        End Get
        Set(ByVal value As String)
            mFilepath = value
        End Set
    End Property

    Public ReadOnly Property E_BSTNR() As String
        Get
            Return mE_BSTNR
        End Get
    End Property

    Public ReadOnly Property E_BELNR() As String
        Get
            Return mE_BELNR
        End Get
    End Property

    Public ReadOnly Property Retouregruende() As DataTable
        Get
            Return mGrund
        End Get
    End Property

    Public Property IsADM() As Boolean
        Get
            Return mIsADM
        End Get
        Set(ByVal value As Boolean)
            mIsADM = value
        End Set
    End Property

    Public Property Retoureindex() As Integer
        Get
            Return mRetoureindex
        End Get
        Set(ByVal value As Integer)
            mRetoureindex = value
        End Set
    End Property

    Public ReadOnly Property Artikel() As DataTable
        Get
            Return mArtikel
        End Get
    End Property

    Public ReadOnly Property Lieferanten() As DataTable
        Get
            Return mLieferanten
        End Get
    End Property

    Public Property SelArtikel() As Integer
        Get
            Return mSelArtikel
        End Get
        Set(ByVal value As Integer)
            mSelArtikel = value
        End Set
    End Property

    Public Property SelLief() As Integer
        Get
            Return mSelLief
        End Get
        Set(ByVal value As Integer)
            mSelLief = value
        End Set
    End Property

    Public ReadOnly Property Commited() As Boolean
        Get
            Return mCommited
        End Get
    End Property

    Public Property Verkaeufer() As String
        Get
            Return mVerkaeufer
        End Get
        Set(ByVal value As String)
            mVerkaeufer = value
        End Set
    End Property

    Public Property Kostenstelle() As String
        Get
            Return mKostenstelle
        End Get
        Set(ByVal value As String)
            mKostenstelle = value
        End Set
    End Property

    Public Property Lieferscheinnummer() As String
        Get
            Return mLieferscheinnummer
        End Get
        Set(ByVal value As String)
            mLieferscheinnummer = value
        End Set
    End Property

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

    Public Property KostStelle() As String
        Get
            KostStelle = mstrKostStelle
        End Get
        Set(ByVal value As String)
            mstrKostStelle = value
        End Set
    End Property

    Public ReadOnly Property Retouren() As DataTable
        Get
            If mRetoure Is Nothing Then
                mRetoure = New DataTable

                mRetoure.Columns.Add("ArtIdx", Type.GetType("System.Int32"))
                mRetoure.Columns("ArtIdx").Unique = True
                mRetoure.Columns.Add("ARTLIF", String.Empty.GetType)
                mRetoure.Columns.Add("Artikelbezeichnung", String.Empty.GetType)
                mRetoure.Columns.Add("Menge", Type.GetType("System.Int32"))
                mRetoure.Columns.Add("Retouregrund", String.Empty.GetType)
                mRetoure.Columns.Add("GRUND", String.Empty.GetType)

                'Primärschlüsselspalte definieren
                Dim strPKeys(0) As DataColumn
                strPKeys(0) = mRetoure.Columns("ArtIdx")
                mRetoure.PrimaryKey = strPKeys

                mRetoureindex = 0
            Else
                If Not mRetoure.GetChanges() Is Nothing Then 'bitte niemals manuell acceptchanges aufrufen
                    mTimer.Start()
                End If
            End If
            Return mRetoure
        End Get
    End Property

    Public Sub New(ByVal mKasse As Kasse)
        mMyKasse = mKasse
        Retouren.AcceptChanges()
        mTimer.Interval = 1200000 '20 Minuten
        mTimer.AutoReset = False
    End Sub

    Public Sub endRetoure()
        mRetoure.Clear()
        Finalize()
    End Sub

    Public Function getLieferantenERP(Optional ByVal Customername As String = "") As DataTable

        SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)
        E_MESSAGE = ""
        E_SUBRC = 0

        Try
            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()

            dt.Rows.Add(New Object() {"I_KOSTL", False, mKostenstelle, 10})
            dt.Rows.Add(New Object() {"I_SUPER_USER", False, "X", 1})
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
                E_SUBRC = SAPExc.E_SUBRC
                E_MESSAGE = SAPExc.E_MESSAGE
            End If
            Dim retRows As DataRow = dt.Select("Fieldname='GT_PLATSTAMM'")(0)
            If Not retRows Is Nothing Then
                mLieferanten = DirectCast(retRows("Data"), DataTable)
            End If

        Catch ex As Exception
        Finally

        End Try
        Return mLieferanten
    End Function

    Public Sub getArtikelReiterERP(ByVal sLifnr As String)

        SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)
        E_MESSAGE = ""
        E_SUBRC = 0

        Try
            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()
            'befüllen der Importparameter
            dt.Rows.Add(New Object() {"I_KOSTL", False, mKostenstelle, 4})

            dt.Rows.Add(New Object() {"I_LIFNR", False, sLifnr, 10})
            dt.Rows.Add(New Object() {"I_RUECKS", False, "X", 1})
            dt.Rows.Add(New Object() {"GT_PLATART", True})

            SAPExc.ExecuteERP("Z_FIL_EFA_PLATARTIKEL", dt)

            If (SAPExc.ErrorOccured) Then
                E_SUBRC = SAPExc.E_SUBRC
                E_MESSAGE = SAPExc.E_MESSAGE
            End If
            Dim retRows = dt.Select("Fieldname='GT_PLATART'")(0)
            If Not retRows Is Nothing Then
                mArtikel = DirectCast(retRows("Data"), DataTable)
                mArtikel.Columns.Add("Menge", Type.GetType("System.Int32"))
                mArtikel.Columns.Add("Beschreibung", Type.GetType("System.String"))
            End If

        Catch ex As Exception
        Finally

        End Try

    End Sub

    Public Function getRetoureGruendeERP() As DataTable 'ByVal sLifnr As String

        SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)
        E_MESSAGE = ""
        E_SUBRC = 0

        Try
            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()

            dt.Rows.Add(New Object() {"GT_GRUND", True})

            SAPExc.ExecuteERP("Z_FIL_EFA_RUECK_GRUND", dt)

            If (SAPExc.ErrorOccured) Then
                E_SUBRC = SAPExc.E_SUBRC
                E_MESSAGE = SAPExc.E_MESSAGE
            End If
            Dim retRows As DataRow = dt.Select("Fieldname='GT_GRUND'")(0)
            If Not retRows Is Nothing Then
                mGrund = DirectCast(retRows("Data"), DataTable)
            End If

        Catch ex As Exception
        Finally

        End Try
        Return mGrund

    End Function

    Public Sub sendRetoureToSAPERP(ByVal mLieferant As String, ByVal bMaster As Boolean)

        Dim tblSAP As New DataTable()
        tblSAP.Columns.Add("ARTLIF", String.Empty.GetType)
        tblSAP.Columns.Add("MENGE", String.Empty.GetType)
        tblSAP.Columns.Add("GRUND", String.Empty.GetType)

        Dim tmpSAPRow As DataRow
        'mit select filter, da sonst auch deletet rows mitgenommen werden, die dann auf einen fehler laufen JJU20090511
        For Each tmprow As DataRow In Retouren.Select("", "", DataViewRowState.CurrentRows)
            tmpSAPRow = tblSAP.NewRow
            tmpSAPRow("ARTLIF") = tmprow("ARTLIF").ToString
            tmpSAPRow("MENGE") = tmprow("Menge").ToString
            tmpSAPRow("GRUND") = tmprow("GRUND").ToString

            tblSAP.Rows.Add(tmpSAPRow)
        Next

        SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)
        E_MESSAGE = ""
        E_SUBRC = 0

        Try

            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()

            'befüllen der Importparameter
            If bMaster = True Then
                dt.Rows.Add(New Object() {"I_KOSTL", False, mstrSendToKost, 4})
            Else
                dt.Rows.Add(New Object() {"I_KOSTL", False, mstrKostStelle, 4})
            End If
            dt.Rows.Add(New Object() {"I_LIFNR", False, mLieferant, 10})
            dt.Rows.Add(New Object() {"I_VERKAEUFER", False, Verkaeufer, 4})
            dt.Rows.Add(New Object() {"I_LIEFERSNR", False, Right("00000000000000000000" & Lieferscheinnummer, 20), 20})
            dt.Rows.Add(New Object() {"GT_RUECK", False, tblSAP})
            dt.Rows.Add(New Object() {"E_BSTNR", True})
            dt.Rows.Add(New Object() {"E_BELNR", True})

            SAPExc.ExecuteERP("Z_FIL_EFA_RUECKGABE", dt)

            If (SAPExc.ErrorOccured) Then
                E_SUBRC = SAPExc.E_SUBRC
                E_MESSAGE = SAPExc.E_MESSAGE
                If (E_SUBRC <> 0) Then
                    Throw New Exception(E_SUBRC & ": " & E_MESSAGE)
                End If
            Else
                Dim retRows As DataRow = dt.Select("Fieldname='E_BELNR'")(0)
                If Not retRows Is Nothing Then
                    mE_BELNR = retRows("Data").ToString.TrimStart(" ")
                End If
                retRows = dt.Select("Fieldname='E_BSTNR'")(0)
                If Not retRows Is Nothing Then
                    mE_BSTNR = retRows("Data").ToString.TrimStart(" ")
                End If
            End If
            mCommited = True

        Catch ex As Exception
            If (mE_SUBRC = "0") Then
                mE_SUBRC = -11
            End If
            mE_MESSAGE = ex.Message
            mCommited = False
        Finally

        End Try
    End Sub

    Public Function KostStelleNameERP(ByVal NeuKost As String) As String
        SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)
        E_MESSAGE = ""
        E_SUBRC = 0
        Dim mKostStellName As String = String.Empty

        Try

            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()

            dt.Rows.Add(New Object() {"I_KOSTL_SEND", False, Right("0000000000" & KostStelle, 10), 10})
            dt.Rows.Add(New Object() {"I_KOSTL_RECEIVE", False, Right("0000000000" & KostStelle, 10), 10})
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
                    mKostStellName = retRows("Data").ToString
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
        Return mKostStellName
    End Function

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

    Public Sub Clear()
        mRetoure.Rows.Clear()
        mRetoureindex = 0
        mLieferscheinnummer = Nothing
        mE_BELNR = Nothing
        mE_BSTNR = Nothing
        mFilepath = Nothing
        mSAPMessage = Nothing
        mE_MESSAGE = Nothing
        mE_SUBRC = Nothing
    End Sub

End Class
