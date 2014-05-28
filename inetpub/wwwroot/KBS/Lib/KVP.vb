Imports System.Globalization

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

    Private SAPExc As SAPExecutor.SAPExecutor

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

        SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)

        Try
            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()

            dt.Rows.Add(New Object() {"I_KOSTL", False, mKostenstelle, 10})
            dt.Rows.Add(New Object() {"I_UNAME", False, mBenutzer, 50})
            dt.Rows.Add(New Object() {"I_ABTEILUNG", False, mAbteilung, 3})
            dt.Rows.Add(New Object() {"E_NAME", True})
            dt.Rows.Add(New Object() {"E_STANDORT", True})
            dt.Rows.Add(New Object() {"E_VORGESETZTER", True})
            dt.Rows.Add(New Object() {"E_ANZ_KVP_BW", True})
            dt.Rows.Add(New Object() {"E_KVPID", True})

            SAPExc.ExecuteERP("Z_KVP_ANMELDUNG", dt)

            If (SAPExc.ErrorOccured) Then
                RaiseError(SAPExc.E_SUBRC, SAPExc.E_MESSAGE)
            Else
                mBenutzername = dt.Select("Fieldname='E_NAME'")(0)("Data").ToString()
                mStandort = dt.Select("Fieldname='E_STANDORT'")(0)("Data").ToString()
                mVorgesetzter = dt.Select("Fieldname='E_VORGESETZTER'")(0)("Data").ToString()
                mZuBewertendeKVPs = dt.Select("Fieldname='E_ANZ_KVP_BW'")(0)("Data").ToString()
                mGeparkteKVPId = dt.Select("Fieldname='E_KVPID'")(0)("Data").ToString()
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

    Public Sub LoadKVP(ByVal kvpId As String, Optional ByVal preserveBewertungsfrist As Boolean = False)
        ClearErrorState()
        ClearKVP(preserveBewertungsfrist)

        mAktuelleKVPId = kvpId

        SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)

        Try
            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()

            dt.Rows.Add(New Object() {"I_KVPID", False, mAktuelleKVPId, 10})
            dt.Rows.Add(New Object() {"ES_VORGANG", True})
            dt.Rows.Add(New Object() {"E_LTEXT_WIE", True})
            dt.Rows.Add(New Object() {"E_LTEXT_WAS", True})
            dt.Rows.Add(New Object() {"E_LTEXT_WEM", True})

            SAPExc.ExecuteERP("Z_KVP_READ", dt)

            If (SAPExc.ErrorOccured) Then
                RaiseError(SAPExc.E_SUBRC, SAPExc.E_MESSAGE)
            Else

                Dim retRows As DataRow = dt.Select("Fieldname='ES_VORGANG'")(0)

                If Not retRows Is Nothing Then
                    Dim SapTable As DataTable = DirectCast(retRows("Data"), DataTable)

                    If SapTable.Rows.Count > 0 Then
                        mKurzbeschreibung = SapTable.Rows(0)("KTEXT").ToString()
                        mStatus = CInt(SapTable.Rows(0)("STATUS"))
                    End If
                End If

                mSituationText = dt.Select("Fieldname='E_LTEXT_WIE'")(0)("Data").ToString()
                mVeraenderungText = dt.Select("Fieldname='E_LTEXT_WAS'")(0)("Data").ToString()
                mVorteilText = dt.Select("Fieldname='E_LTEXT_WEM'")(0)("Data").ToString()
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

    Public Sub SaveKVP(Optional ByVal nurParken As Boolean = False)
        ClearErrorState()

        SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)

        Try
            Dim SapTable As DataTable = InitSapTable()
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

            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()

            dt.Rows.Add(New Object() {"IS_VORGANG", False, SapTable})
            dt.Rows.Add(New Object() {"I_LTEXT_WIE", False, mSituationText, 400})
            dt.Rows.Add(New Object() {"I_LTEXT_WAS", False, mVeraenderungText, 400})
            dt.Rows.Add(New Object() {"I_LTEXT_WEM", False, mVorteilText, 400})
            dt.Rows.Add(New Object() {"E_KVPID", True})

            SAPExc.ExecuteERP("Z_KVP_SAVE", dt)

            If (SAPExc.ErrorOccured) Then
                RaiseError(SAPExc.E_SUBRC, SAPExc.E_MESSAGE)
            Else
                mAktuelleKVPId = dt.Select("Fieldname='E_KVPID'")(0)("Data").ToString()
                If Not nurParken Then
                    ClearKVP()
                End If
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

    Public Sub DeleteKVP()
        ClearErrorState()

        SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)

        Try
            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()

            dt.Rows.Add(New Object() {"I_KVPID", False, mAktuelleKVPId, 10})

            SAPExc.ExecuteERP("Z_KVP_DELETE", dt)

            If (SAPExc.ErrorOccured) Then
                RaiseError(SAPExc.E_SUBRC, SAPExc.E_MESSAGE)
            Else
                ClearKVP()
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

        SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)

        Try
            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()

            dt.Rows.Add(New Object() {"I_UNAME", False, mBenutzer, 50})
            dt.Rows.Add(New Object() {"I_ABTEILUNG", False, mAbteilung, 3})
            dt.Rows.Add(New Object() {"ET_BW_VORGANG", True})

            SAPExc.ExecuteERP("Z_KVP_GET_BEWERT", dt)

            If (SAPExc.ErrorOccured) Then
                RaiseError(SAPExc.E_SUBRC, SAPExc.E_MESSAGE)
            Else

                Dim retRows As DataRow = dt.Select("Fieldname='ET_BW_VORGANG'")(0)

                If Not retRows Is Nothing Then
                    mVorschlagsliste = DirectCast(retRows("Data"), DataTable)
                    mVorschlagsliste.Columns.Add("RESTTAGE", GetType(Integer))
                    For Each dRow As DataRow In mVorschlagsliste.Rows
                        Dim fristende As DateTime
                        If DateTime.TryParseExact(dRow("BW_FRIST").ToString(), "dd.MM.yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, fristende) Then
                            dRow("RESTTAGE") = CInt((fristende - DateTime.Now).TotalDays)
                        End If
                    Next
                End If
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

    Public Sub SaveBewertung()
        ClearErrorState()

        SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)

        Try
            Dim bewertung As String = ""
            If mBewertungPositiv Then
                bewertung = "P"
            ElseIf mBewertungNegativ Then
                bewertung = "N"
            End If

            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()

            dt.Rows.Add(New Object() {"I_KVPID", False, mAktuelleKVPId, 10})
            dt.Rows.Add(New Object() {"I_UNAME", False, mBenutzer, 50})
            dt.Rows.Add(New Object() {"I_BEWERT", False, bewertung, 1})

            SAPExc.ExecuteERP("Z_KVP_SAVE_BEWERT", dt)

            If (SAPExc.ErrorOccured) Then
                RaiseError(SAPExc.E_SUBRC, SAPExc.E_MESSAGE)
            Else
                mZuBewertendeKVPs -= 1
                ClearKVP()
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

    Private Function InitSapTable() As DataTable
        Dim tmpTable As New DataTable

        tmpTable.Columns.Add("KVPID", GetType(String))
        tmpTable.Columns.Add("KOSTL", GetType(String))
        tmpTable.Columns.Add("KTEXT", GetType(String))
        tmpTable.Columns.Add("UNAME", GetType(String))
        tmpTable.Columns.Add("NAME", GetType(String))
        tmpTable.Columns.Add("ABTEILUNG", GetType(String))
        tmpTable.Columns.Add("STANDORT", GetType(String))
        tmpTable.Columns.Add("FUNKTION", GetType(String))
        tmpTable.Columns.Add("VORGESETZTER", GetType(String))
        tmpTable.Columns.Add("STATUS", GetType(Integer))

        Return tmpTable
    End Function

End Class

