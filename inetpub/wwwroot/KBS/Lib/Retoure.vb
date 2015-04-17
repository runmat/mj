Imports KBSBase

Public Class Retoure
    Inherits ErrorHandlingClass

    Private mRetoure As DataTable
    Private mLieferanten As DataTable
    Private mArtikel As DataTable
    Private mGrund As DataTable
    Private WithEvents mTimer As New Timers.Timer
    Private mLieferscheinnummer As String
    Private mKostenstelle As String
    Private mVerkaeufer As String
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

#Region "Properties"

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

#End Region

    Public Sub New()
        Retouren.AcceptChanges()
        mTimer.Interval = 1200000 '20 Minuten
        mTimer.AutoReset = False
    End Sub

    Public Sub endRetoure()
        mRetoure.Clear()
        Finalize()
    End Sub

    Public Function getLieferantenERP(Optional ByVal Customername As String = "") As DataTable
        ClearErrorState()

        Try
            S.AP.Init("Z_FIL_EFA_PLATSTAMM")

            S.AP.SetImportParameter("I_KOSTL", mKostenstelle)
            S.AP.SetImportParameter("I_SUPER_USER", "X")

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

        Return mLieferanten
    End Function

    Public Sub getArtikelReiterERP(ByVal sLifnr As String)
        ClearErrorState()

        Try
            S.AP.Init("Z_FIL_EFA_PLATARTIKEL")

            S.AP.SetImportParameter("I_KOSTL", mKostenstelle)
            S.AP.SetImportParameter("I_LIFNR", sLifnr)
            S.AP.SetImportParameter("I_RUECKS", "X")

            S.AP.Execute()

            If S.AP.ResultCode <> 0 Then
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

            mArtikel = S.AP.GetExportTable("GT_PLATART")
            mArtikel.Columns.Add("Menge", Type.GetType("System.Int32"))
            mArtikel.Columns.Add("Beschreibung", Type.GetType("System.String"))

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

    Public Function getRetoureGruendeERP() As DataTable
        ClearErrorState()

        Try
            S.AP.Init("Z_FIL_EFA_RUECK_GRUND")

            S.AP.Execute()

            If S.AP.ResultCode <> 0 Then
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

            mGrund = S.AP.GetExportTable("GT_GRUND")

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try

        Return mGrund
    End Function

    Public Sub sendRetoureToSAPERP(ByVal mLieferant As String, ByVal bMaster As Boolean)
        ClearErrorState()

        Try
            S.AP.Init("Z_FIL_EFA_RUECKGABE")

            S.AP.SetImportParameter("I_KOSTL", IIf(bMaster, mstrSendToKost, mstrKostStelle))
            S.AP.SetImportParameter("I_LIFNR", mLieferant)
            S.AP.SetImportParameter("I_VERKAEUFER", Verkaeufer)
            S.AP.SetImportParameter("I_LIEFERSNR", Lieferscheinnummer.PadLeft(20, "0"c))

            Dim tblSAP As DataTable = S.AP.GetImportTable("GT_RUECK")
            'mit select filter, da sonst auch deleted rows mitgenommen werden, die dann auf einen fehler laufen JJU20090511
            For Each tmprow As DataRow In Retouren.Select("", "", DataViewRowState.CurrentRows)
                Dim tmpSAPRow As DataRow = tblSAP.NewRow
                tmpSAPRow("ARTLIF") = tmprow("ARTLIF").ToString
                tmpSAPRow("MENGE") = tmprow("Menge").ToString
                tmpSAPRow("GRUND") = tmprow("GRUND").ToString
                tblSAP.Rows.Add(tmpSAPRow)
            Next

            S.AP.Execute()

            If S.AP.ResultCode = 0 Then
                mE_BELNR = S.AP.GetExportParameter("E_BELNR").TrimStart(" ")
                mE_BSTNR = S.AP.GetExportParameter("E_BSTNR").TrimStart(" ")
                mCommited = True
            Else
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
                mCommited = False
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
            mCommited = False
        End Try
    End Sub

    Public Function KostStelleNameERP(ByVal NeuKost As String) As String
        ClearErrorState()

        Dim mKostStellName As String = ""

        Try
            S.AP.Init("Z_FIL_EFA_GET_KOSTL", "I_KOSTL_SEND, I_KOSTL_RECEIVE", KostStelle.PadLeft(10, "0"c), KostStelle.PadLeft(10, "0"c))

            S.AP.Execute()

            If S.AP.ResultCode = 0 Then
                mKostStellName = S.AP.GetExportParameter("E_KTEXT")
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

        Return mKostStellName
    End Function

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

    Public Sub Clear()
        mRetoure.Rows.Clear()
        mRetoureindex = 0
        mLieferscheinnummer = Nothing
        mE_BELNR = Nothing
        mE_BSTNR = Nothing
        mFilepath = Nothing
    End Sub

End Class
