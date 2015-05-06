Imports System.Globalization
Imports KBSBase

Public Class KVP
    Inherits ErrorHandlingClass

#Region "Declarations"

    Private mAktuelleKVPId As String
    Private mKostenstelle As String
    Private mBenutzer As String
    Private mBenutzername As String
    Private Const mAbteilung As String = "FIL"
    Private Const mFunktion As String = "Filialmitarbeiter"
    Private mStandort As String
    Private mVorgesetzter As String
    Private mKurzbeschreibung As String
    Private mSituationText As String
    Private mVeraenderungText As String
    Private mVorteilText As String
    Private mStatus As Integer = 0

    Private mBewertungsfrist As String
    Private mBewertungPositiv As Boolean
    Private mBewertungNegativ As Boolean

    Private mZuBewertendeKVPs As Integer = 0
    Private mGeparkteKVPId As String

    Private mVorschlagsliste As DataTable

#End Region

#Region "Properties"

    Public ReadOnly Property AktuelleKVPId() As String
        Get
            Return mAktuelleKVPId
        End Get
    End Property

    Public ReadOnly Property Kostenstelle() As String
        Get
            Return mKostenstelle
        End Get
    End Property

    Public ReadOnly Property Benutzer() As String
        Get
            Return mBenutzer
        End Get
    End Property

    Public ReadOnly Property Benutzername() As String
        Get
            Return mBenutzername
        End Get
    End Property

    Public ReadOnly Property Abteilung() As String
        Get
            Return mAbteilung
        End Get
    End Property

    Public ReadOnly Property Funktion() As String
        Get
            Return mFunktion
        End Get
    End Property

    Public ReadOnly Property Standort() As String
        Get
            Return mStandort
        End Get
    End Property

    Public ReadOnly Property Vorgesetzter() As String
        Get
            Return mVorgesetzter
        End Get
    End Property

    Public Property Kurzbeschreibung() As String
        Get
            Return mKurzbeschreibung
        End Get
        Set(value As String)
            mKurzbeschreibung = value
        End Set
    End Property

    Public Property SituationText() As String
        Get
            Return mSituationText
        End Get
        Set(value As String)
            mSituationText = value
        End Set
    End Property

    Public Property VeraenderungText() As String
        Get
            Return mVeraenderungText
        End Get
        Set(value As String)
            mVeraenderungText = value
        End Set
    End Property

    Public Property VorteilText() As String
        Get
            Return mVorteilText
        End Get
        Set(value As String)
            mVorteilText = value
        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get
            Return mStatus
        End Get
    End Property

    Public ReadOnly Property Bewertungsfrist() As String
        Get
            Return mBewertungsfrist
        End Get
    End Property

    Public Property BewertungPositiv() As Boolean
        Get
            Return mBewertungPositiv
        End Get
        Set(value As Boolean)
            mBewertungPositiv = value
        End Set
    End Property

    Public Property BewertungNegativ() As Boolean
        Get
            Return mBewertungNegativ
        End Get
        Set(value As Boolean)
            mBewertungNegativ = value
        End Set
    End Property

    Public ReadOnly Property ZuBewertendeKVPs() As Integer
        Get
            Return mZuBewertendeKVPs
        End Get
    End Property

    Public ReadOnly Property GeparkteKVPId() As String
        Get
            Return mGeparkteKVPId
        End Get
    End Property

    Public ReadOnly Property Vorschlagsliste() As DataTable
        Get
            Return mVorschlagsliste
        End Get
    End Property

#End Region

    Public Sub KVPLogin(ByVal kst As String, ByVal user As String)
        ClearErrorState()

        mKostenstelle = kst
        mBenutzer = user

        Try
            S.AP.Init("Z_KVP_ANMELDUNG")

            S.AP.SetImportParameter("I_KOSTL", mKostenstelle)
            S.AP.SetImportParameter("I_UNAME", mBenutzer)
            S.AP.SetImportParameter("I_ABTEILUNG", mAbteilung)

            S.AP.Execute()

            If S.AP.ResultCode = 0 Then
                mBenutzername = S.AP.GetExportParameter("E_NAME")
                mStandort = S.AP.GetExportParameter("E_STANDORT")
                mVorgesetzter = S.AP.GetExportParameter("E_VORGESETZTER")
                mZuBewertendeKVPs = S.AP.GetExportParameter("E_ANZ_KVP_BW")
                mGeparkteKVPId = S.AP.GetExportParameter("E_KVPID")
            Else
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

    Public Sub LoadKVP(ByVal kvpId As String, Optional ByVal preserveBewertungsfrist As Boolean = False)
        ClearErrorState()
        ClearKVP(preserveBewertungsfrist)

        mAktuelleKVPId = kvpId

        Try
            S.AP.Init("Z_KVP_READ", "I_KVPID", mAktuelleKVPId)

            S.AP.Execute()

            If S.AP.ResultCode = 0 Then
                Dim SapTable As DataTable = S.AP.GetExportTable("ES_VORGANG")
                If SapTable.Rows.Count > 0 Then
                    mKurzbeschreibung = SapTable.Rows(0)("KTEXT").ToString()
                    mStatus = CInt(SapTable.Rows(0)("STATUS"))
                End If

                mSituationText = S.AP.GetExportParameter("E_LTEXT_WIE")
                mVeraenderungText = S.AP.GetExportParameter("E_LTEXT_WAS")
                mVorteilText = S.AP.GetExportParameter("E_LTEXT_WEM")
            Else
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

    Public Sub SaveKVP(Optional ByVal nurParken As Boolean = False)
        ClearErrorState()

        Try
            S.AP.Init("Z_KVP_SAVE")

            S.AP.SetImportParameter("I_LTEXT_WIE", mSituationText)
            S.AP.SetImportParameter("I_LTEXT_WAS", mVeraenderungText)
            S.AP.SetImportParameter("I_LTEXT_WEM", mVorteilText)

            Dim SapTable As DataTable = S.AP.GetImportTable("IS_VORGANG")
            Dim newRow As DataRow = SapTable.NewRow()
            newRow("KVPID") = mAktuelleKVPId
            newRow("KOSTL") = mKostenstelle
            newRow("KTEXT") = mKurzbeschreibung
            newRow("UNAME") = mBenutzer
            newRow("NAME") = mBenutzername
            newRow("ABTEILUNG") = mAbteilung
            newRow("STANDORT") = mStandort
            newRow("FUNKTION") = mFunktion
            newRow("VORGESETZTER") = mVorgesetzter
            If nurParken Then
                newRow("STATUS") = 1
            Else
                newRow("STATUS") = 2
            End If
            SapTable.Rows.Add(newRow)

            S.AP.Execute()

            If S.AP.ResultCode = 0 Then
                mAktuelleKVPId = S.AP.GetExportParameter("E_KVPID")
                If Not nurParken Then
                    ClearKVP()
                End If
            Else
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

    Public Sub DeleteKVP()
        ClearErrorState()

        Try
            S.AP.Init("Z_KVP_DELETE", "I_KVPID", mAktuelleKVPId)

            S.AP.Execute()

            If S.AP.ResultCode = 0 Then
                ClearKVP()
            Else
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

    Private Sub ClearKVP(Optional ByVal preserveBewertungsfrist As Boolean = False)
        mAktuelleKVPId = ""
        mKurzbeschreibung = ""
        mSituationText = ""
        mVeraenderungText = ""
        mVorteilText = ""
        mStatus = 0
        mBewertungPositiv = False
        mBewertungNegativ = False
        If Not preserveBewertungsfrist Then
            mBewertungsfrist = ""
        End If
    End Sub

    Public Sub LoadBewertungen()
        ClearErrorState()

        'Wenn von vornherein bekannt ist, dass es keine Bewertungen gibt, das Bapi gar nicht erst aufrufen
        If ZuBewertendeKVPs = 0 Then
            If mVorschlagsliste IsNot Nothing AndAlso mVorschlagsliste.Rows.Count > 0 Then
                mVorschlagsliste.Rows.Clear()
            End If
            Exit Sub
        End If

        Try
            S.AP.Init("Z_KVP_GET_BEWERT", "I_UNAME, I_ABTEILUNG", mBenutzer, mAbteilung)

            S.AP.Execute()

            If S.AP.ResultCode = 0 Then
                mVorschlagsliste = S.AP.GetExportTable("ET_BW_VORGANG")
                mVorschlagsliste.Columns.Add("RESTTAGE", GetType(Integer))
                For Each dRow As DataRow In mVorschlagsliste.Rows
                    Dim fristende As DateTime
                    If DateTime.TryParseExact(dRow("BW_FRIST").ToString(), "dd.MM.yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, fristende) Then
                        dRow("RESTTAGE") = CInt((fristende - DateTime.Now).TotalDays)
                    End If
                Next
            Else
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

    Public Sub SaveBewertung()
        ClearErrorState()

        Try
            Dim bewertung As String = ""
            If mBewertungPositiv Then
                bewertung = "P"
            ElseIf mBewertungNegativ Then
                bewertung = "N"
            End If

            S.AP.Init("Z_KVP_SAVE_BEWERT")

            S.AP.SetImportParameter("I_KVPID", mAktuelleKVPId)
            S.AP.SetImportParameter("I_UNAME", mBenutzer)
            S.AP.SetImportParameter("I_BEWERT", bewertung)

            S.AP.Execute()

            If S.AP.ResultCode = 0 Then
                mZuBewertendeKVPs -= 1
                ClearKVP()
            Else
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

    Public Sub SelectKVPForBewertung(ByVal kvpId As String)
        ClearKVP()
        mAktuelleKVPId = kvpId
        Dim rows As DataRow() = mVorschlagsliste.Select("KVPID='" & kvpId & "'")
        If rows.Length > 0 Then
            mBewertungsfrist = rows(0)("BW_FRIST").ToString()
        End If
    End Sub

End Class

