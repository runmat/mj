
Public Class Umlagerung
    Inherits ErrorHandlingClass

    Private mUmlagerung As DataTable
    Private mArtikel As DataTable
    Private mListeAusparken As DataTable
    Private mListeGroesse As DataTable
    Private mstrKostStelle As String = ""
    Private mstrKostStelleNeu As String = ""
    Private mstrKostText As String = ""
    Private mstrBelegNR As String = ""
    Private mstrFilePath As String = ""
    Private mstrMandant As String = ""
    Private mstrParkDate As String = ""

    Dim SAPExc As SAPExecutor.SAPExecutor

#Region "Properties"

    Public ReadOnly Property ListeAusparken() As DataTable
        Get
            Return mListeAusparken
        End Get
    End Property

    Public ReadOnly Property ListeGroesse() As DataTable
        Get
            Return mListeGroesse
        End Get
    End Property

    Public Property Umlagerung() As DataTable
        Get
            Return mUmlagerung
        End Get
        Set(ByVal value As DataTable)
            mUmlagerung = value
        End Set
    End Property

    Public ReadOnly Property Artikel() As DataTable
        Get
            Return mArtikel
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

    Public Property KostStelleNeu() As String
        Get
            Return mstrKostStelleNeu
        End Get
        Set(ByVal value As String)
            mstrKostStelleNeu = value
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

    Public Property BelegNR() As String
        Get
            Return mstrBelegNR
        End Get
        Set(ByVal Value As String)
            mstrBelegNR = Value
        End Set
    End Property

    Public Property FilePath() As String
        Get
            Return mstrFilePath
        End Get
        Set(ByVal Value As String)
            mstrFilePath = Value
        End Set
    End Property

    Public Property ParkDate() As String
        Get
            Return mstrParkDate
        End Get
        Set(ByVal value As String)
            mstrParkDate = value
        End Set
    End Property

#End Region

    Public Sub New()
        If mUmlagerung Is Nothing Then
            mUmlagerung = New DataTable

            mUmlagerung.Columns.Add("MATNR", String.Empty.GetType)
            mUmlagerung.Columns.Add("UMLGO", String.Empty.GetType)
            mUmlagerung.Columns.Add("MAKTX", String.Empty.GetType)
            mUmlagerung.Columns.Add("EAN11", String.Empty.GetType)
            mUmlagerung.Columns.Add("Menge", Type.GetType("System.Int32"))
            mUmlagerung.Columns.Add("LTEXT_NR", String.Empty.GetType)
            mUmlagerung.Columns.Add("LTEXT", String.Empty.GetType)
            mUmlagerung.Columns.Add("KENNZFORM", String.Empty.GetType)
        End If

    End Sub

    Public Sub endUmlagerung()
        Finalize()
    End Sub

    Public Sub insertIntoBestellungen(ByVal Artikelnr As String, ByVal Menge As Integer, ByVal Artikelbezeichnung As String, ByVal EAN As String,
                                         Optional ByVal LTextNR As String = "", Optional ByVal Kennzform As String = "")
        ClearErrorState()

        Try
            Dim Rows As DataRow()
            If Kennzform = "" Then
                Rows = mUmlagerung.Select("MATNR='" & Artikelnr & "'")
            Else
                Rows = mUmlagerung.Select("MATNR='" & Artikelnr & "' AND Kennzform = '" & Kennzform & "'")
            End If

            Dim tmpRow As DataRow

            If Rows.GetLength(0) > 0 Then
                tmpRow = Rows(0)
                tmpRow.Item("UMLGO") = mstrKostStelleNeu
                tmpRow.Item("MAKTX") = Artikelbezeichnung
                tmpRow.Item("EAN11") = EAN
                tmpRow("Menge") = Menge
                tmpRow("LTEXT_NR") = LTextNR
                tmpRow.Item("LTEXT") = ""
                tmpRow.Item("KENNZFORM") = Kennzform
            Else
                tmpRow = mUmlagerung.NewRow()
                tmpRow.Item("MATNR") = Artikelnr
                tmpRow.Item("UMLGO") = mstrKostStelleNeu
                tmpRow.Item("MAKTX") = Artikelbezeichnung
                tmpRow.Item("EAN11") = EAN
                tmpRow("Menge") = Menge
                tmpRow("LTEXT_NR") = LTextNR
                tmpRow.Item("LTEXT") = ""
                tmpRow.Item("KENNZFORM") = Kennzform
                mUmlagerung.Rows.Add(tmpRow)
            End If
        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try

    End Sub

    Public Sub updateBestellungInfotext(ByVal Artikelnr As String, ByVal LText As String, Optional ByVal Kennzform As String = "")
        ClearErrorState()

        Try
            Dim Rows As DataRow()
            If Kennzform = "" Then
                Rows = mUmlagerung.Select("MATNR='" & Artikelnr & "'")
            Else
                Rows = mUmlagerung.Select("MATNR='" & Artikelnr & "' AND Kennzform = '" & Kennzform & "'")
            End If

            Dim tmpRow As DataRow

            If Rows.GetLength(0) > 0 Then
                tmpRow = Rows(0)
                tmpRow.Item("LTEXT") = LText
            End If
        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try

    End Sub

    Public Sub ShowERP()
        ClearErrorState()

        SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)

        Try

            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()

            dt.Rows.Add(New Object() {"I_KOSTL", False, mstrKostStelle, 10})
            dt.Rows.Add(New Object() {"GT_MAT", True})

            SAPExc.ExecuteERP("Z_FIL_EFA_UML_MAT", dt)

            If (SAPExc.ErrorOccured) Then
                RaiseError(SAPExc.E_SUBRC, SAPExc.E_MESSAGE)
            Else
                mArtikel = DirectCast(dt.Rows(1)("Data"), DataTable)

            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

    Public Sub ChangeERP(Optional sErdat As String = "")
        ClearErrorState()

        Dim tblSAP As New DataTable()
        tblSAP.Columns.Add("MATNR", String.Empty.GetType)
        tblSAP.Columns.Add("MENGE", String.Empty.GetType)
        tblSAP.Columns.Add("EAN11", String.Empty.GetType)
        tblSAP.Columns.Add("LTEXT_NR", String.Empty.GetType)
        tblSAP.Columns.Add("KENNZFORM", String.Empty.GetType)

        Dim tmpSAPRow As DataRow

        If Umlagerung.Rows.Count = 0 Then
            RaiseError("", "Ausgewählte Artikel konnten nicht gefunden werden!")
            Exit Sub
        End If

        Dim LSTS As New LongStringToSap

        For Each tmprow As DataRow In Umlagerung.Rows
            tmpSAPRow = tblSAP.NewRow
            tmpSAPRow("MATNR") = tmprow("MATNR").ToString
            tmpSAPRow("MENGE") = tmprow("MENGE").ToString
            tmpSAPRow("EAN11") = tmprow("EAN11").ToString
            If tmprow("LTEXT_NR").ToString = "" Then
                If tmprow("LTEXT").ToString <> "" Then
                    tmpSAPRow("LTEXT_NR") = LSTS.InsertStringERP(tmprow("LTEXT").ToString, "UMLT")
                End If
            Else
                LSTS.UpdateStringERP(tmprow("LTEXT").ToString, tmprow("LTEXT_NR").ToString)
                tmpSAPRow("LTEXT_NR") = tmprow("LTEXT_NR").ToString
            End If

            tmpSAPRow("KENNZFORM") = tmprow("KENNZFORM").ToString
            tblSAP.Rows.Add(tmpSAPRow)
        Next


        Dim Budat As DateTime

        Try
            SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)

            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()

            dt.Rows.Add(New Object() {"I_KOSTL_IN", False, mstrKostStelleNeu, 4})
            dt.Rows.Add(New Object() {"GT_MAT", False, tblSAP})
            dt.Rows.Add(New Object() {"I_KOSTL_OUT", False, mstrKostStelle, 4})
            dt.Rows.Add(New Object() {"I_BELNR", False, mstrBelegNR, 10})
            dt.Rows.Add(New Object() {"GT_BELNR", True})
            If IsDate(sErdat) Then
                DateTime.TryParse(sErdat, Budat)
                dt.Rows.Add(New Object() {"I_BUDAT", False, Budat, 8})
            End If

            SAPExc.ExecuteERP("Z_FIL_EFA_UML_STEP1", dt)

            If (SAPExc.ErrorOccured) Then
                RaiseError(SAPExc.E_SUBRC, SAPExc.E_MESSAGE)
            Else
                Dim BelegTable As DataTable
                Dim retRows As DataRow = dt.Select("Fieldname='GT_BELNR'")(0)
                If Not retRows Is Nothing Then
                    BelegTable = DirectCast(retRows("Data"), DataTable)
                    If BelegTable.Rows.Count > 0 Then
                        For i = 0 To BelegTable.Rows.Count - 1
                            mstrBelegNR += BelegTable.Rows(i)("BELNR").ToString & vbCrLf
                        Next
                    End If
                End If

                If mstrBelegNR = "" Then
                    RaiseError("", "Es wurde keine Belegnummer erzeugt!")
                End If
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try

    End Sub

    Public Sub CheckKostStelleERP(ByVal NeuKost As String)
        ClearErrorState()

        Try
            SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)

            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()

            dt.Rows.Add(New Object() {"I_KOSTL_SEND", False, Right("0000000000" & KostStelle, 10), 10})
            dt.Rows.Add(New Object() {"I_KOSTL_RECEIVE", False, Right("0000000000" & NeuKost, 10), 10})
            dt.Rows.Add(New Object() {"E_KOSTL", True})
            dt.Rows.Add(New Object() {"E_KTEXT", True})
            dt.Rows.Add(New Object() {"E_LTEXT", True})

            SAPExc.ExecuteERP("Z_FIL_EFA_GET_KOSTL", dt)

            If (SAPExc.ErrorOccured) Then
                RaiseError(SAPExc.E_SUBRC, SAPExc.E_MESSAGE)
            Else
                Dim retRows As DataRow = dt.Select("Fieldname='E_KTEXT'")(0)
                If Not retRows Is Nothing Then
                    mstrKostText = retRows("Data").ToString
                End If
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        Finally
            If ErrorOccured Then
                Select Case ErrorCode
                    Case "102"
                        RaiseError("102", "KST " & NeuKost & " ist nicht zulässig! Bitte einen Lieferscheinverkauf eingeben.")
                    Case "104"
                        RaiseError("104", "KST nicht zulässig! Bitte richtige KST eingeben.")
                End Select
            End If
        End Try

    End Sub

    Public Function GetListeAusparkenERP() As DataTable
        ClearErrorState()

        Try
            SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)

            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()
            mListeAusparken = New DataTable
            dt.Rows.Add(New Object() {"I_KOSTL_IN", False, Right("0000" & mstrKostStelle, 4), 4})
            dt.Rows.Add(New Object() {"GT_PARK_LISTE", True})

            SAPExc.ExecuteERP("Z_FIL_EFA_UML_PARK_LISTE", dt)

            If (SAPExc.ErrorOccured) Then
                Select Case SAPExc.E_SUBRC
                    Case "101"
                        'E_MESSAGE = ""
                    Case Else
                        RaiseError(SAPExc.E_SUBRC, SAPExc.E_MESSAGE)
                End Select
            End If

            Dim retRows As DataRow = dt.Select("Fieldname='GT_PARK_LISTE'")(0)

            If Not retRows Is Nothing Then
                mListeAusparken = DirectCast(retRows("Data"), DataTable)
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try

        Return mListeAusparken
    End Function

    Public Sub ParkenERP()
        ClearErrorState()

        Try
            SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)

            If Umlagerung.Rows.Count > 0 Then
                mstrKostStelleNeu = mUmlagerung.Rows(0)("UMLGO")

                Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()

                dt.Rows.Add(New Object() {"I_KOSTL_IN", False, Right("0000" & mstrKostStelleNeu, 4), 4})
                dt.Rows.Add(New Object() {"I_KOSTL_OUT", False, Right("0000" & mstrKostStelle, 4), 4})
                Dim tblTemp As New DataTable

                tblTemp.Columns.Add("MATNR")
                tblTemp.Columns.Add("MENGE")
                tblTemp.Columns.Add("EAN11")
                tblTemp.Columns.Add("LTEXT_NR")
                tblTemp.Columns.Add("KENNZFORM")

                Dim LSTS As New LongStringToSap

                For Each row As DataRow In Umlagerung.Rows
                    Dim tRow As DataRow = tblTemp.NewRow()
                    tRow("MATNR") = row("MATNR")
                    tRow("MENGE") = row("MENGE")
                    tRow("EAN11") = row("EAN11")
                    tRow("KENNZFORM") = row("KENNZFORM")
                    'Steuerung der Langtexte
                    Dim tmpText As String = ""
                    If Not IsDBNull(row("LTEXT")) Then
                        tmpText = CStr(row("LTEXT"))
                    End If
                    If tmpText <> "" And row("LTEXT_NR").ToString.Length = 0 Then
                        tRow("LTEXT_NR") = LSTS.InsertStringERP(tmpText, "UMLT")
                    ElseIf tRow("LTEXT_NR").ToString.Length = 0 Then
                        LSTS.UpdateStringERP(tmpText, row("LTEXT_NR").ToString)
                        tRow("LTEXT_NR") = row("LTEXT_NR").ToString
                    End If

                    tblTemp.Rows.Add(tRow)
                Next
                dt.Rows.Add(New Object() {"GT_MAT", False, tblTemp})
                dt.Rows.Add(New Object() {"E_BELNR", True})

                SAPExc.ExecuteERP("Z_FIL_EFA_UML_PARK_INS", dt)

                If (SAPExc.ErrorOccured) Then
                    RaiseError(SAPExc.E_SUBRC, SAPExc.E_MESSAGE)
                Else
                    Dim retRows As DataRow = dt.Select("Fieldname='E_BELNR'")(0)

                    If Not retRows Is Nothing Then
                        mstrBelegNR = retRows("Data").ToString()
                    End If

                    mUmlagerung.Rows.Clear()
                    mstrKostStelleNeu = ""
                End If

            Else
                RaiseError("0001", "Keine Datensätze für Umlagerung vorhanden!")
            End If
        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try

    End Sub

    Public Function AusparkenERP(ByVal BelNr As String, Optional ByVal onlyread As Boolean = False) As DataTable
        ClearErrorState()

        mstrBelegNR = BelNr
        mstrParkDate = ""
        Dim tblOnlyRead As New DataTable

        Try
            SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)
            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()

            dt.Rows.Add(New Object() {"I_BELNR", False, Right("0000000000" & mstrBelegNR, 10), 10})
            dt.Rows.Add(New Object() {"ES_UMLK_PARK", True})
            dt.Rows.Add(New Object() {"GT_UMLP_PARK", True})
            SAPExc.ExecuteERP("Z_FIL_EFA_UML_PARK_READ", dt)

            If (SAPExc.ErrorOccured) Then
                RaiseError(SAPExc.E_SUBRC, SAPExc.E_MESSAGE)
            Else
                Dim retRows As DataRow = dt.Select("Fieldname='ES_UMLK_PARK'")(0)
                Dim tblTemp As New DataTable
                If Not retRows Is Nothing Then
                    tblTemp = DirectCast(retRows("Data"), DataTable)
                    If tblTemp.Rows.Count > 0 And onlyread = False Then
                        mstrMandant = tblTemp.Rows(0)("MANDT")
                        mstrBelegNR = tblTemp.Rows(0)("BELNR")
                        mstrKostStelleNeu = tblTemp.Rows(0)("LGORT_EMPF")
                    End If
                End If

                If onlyread Then
                    Dim mstrKStNeuTemp As String = ""
                    If tblTemp.Rows.Count > 0 Then
                        mstrKStNeuTemp = tblTemp.Rows(0)("LGORT_EMPF")
                        Dim strTemp As String = CStr(tblTemp.Rows(0)("ERDAT"))
                        mstrParkDate = Right(strTemp, 2) & "." & Mid(strTemp, 5, 2) & "." & Left(strTemp, 4)
                    End If

                    tblOnlyRead.Columns.Add("MATNR", String.Empty.GetType)
                    tblOnlyRead.Columns.Add("UMLGO", String.Empty.GetType)
                    tblOnlyRead.Columns.Add("MAKTX", String.Empty.GetType)
                    tblOnlyRead.Columns.Add("EAN11", String.Empty.GetType)
                    tblOnlyRead.Columns.Add("Menge", System.Type.GetType("System.Int32"))
                    tblOnlyRead.Columns.Add("LTEXT_NR", String.Empty.GetType)
                    tblOnlyRead.Columns.Add("LTEXT", String.Empty.GetType)
                    tblOnlyRead.Columns.Add("KENNZFORM", String.Empty.GetType)

                    Dim tblTemp2 As New DataTable
                    retRows = dt.Select("Fieldname='GT_UMLP_PARK'")(0)
                    If Not retRows Is Nothing Then
                        tblTemp2 = DirectCast(retRows("Data"), DataTable)
                    End If

                    Dim mLSTS As New LongStringToSap()

                    For Each row As DataRow In tblTemp2.Rows
                        Dim trow As DataRow = tblOnlyRead.NewRow
                        trow("MATNR") = row("MATNR")
                        trow("MENGE") = CInt(row("MENGE"))
                        trow("EAN11") = row("EAN11")
                        trow("LTEXT_NR") = row("LTEXT_NR")
                        trow("KENNZFORM") = row("KENNZFORM")
                        trow("UMLGO") = mstrKStNeuTemp
                        Dim rowArt As DataRow = mArtikel.Select("MATNR='" & CStr(trow("MATNR")) & "'")(0)
                        trow("MAKTX") = rowArt("MAKTX")
                        Dim strTemp As String = ""
                        If Not TypeOf (trow("LTEXT_NR")) Is DBNull Then
                            strTemp = CStr(trow("LTEXT_NR"))
                        End If
                        trow("LTEXT") = mLSTS.ReadStringERP(strTemp)

                        tblOnlyRead.Rows.Add(trow)
                    Next
                Else
                    mUmlagerung.Rows.Clear()

                    Dim tblTemp2 As New DataTable
                    retRows = dt.Select("Fieldname='GT_UMLP_PARK'")(0)
                    If Not retRows Is Nothing Then
                        tblTemp2 = DirectCast(retRows("Data"), DataTable)
                    End If
                    Dim mLSTS As New LongStringToSap()

                    For Each row As DataRow In tblTemp2.Rows
                        Dim trow As DataRow = mUmlagerung.NewRow
                        trow("MATNR") = row("MATNR")
                        trow("MENGE") = CInt(row("MENGE"))
                        trow("EAN11") = row("EAN11")
                        trow("LTEXT_NR") = row("LTEXT_NR")
                        trow("KENNZFORM") = row("KENNZFORM")
                        trow("UMLGO") = mstrKostStelleNeu
                        Dim rowArt As DataRow = mArtikel.Select("MATNR='" & CStr(trow("MATNR") & "'"))(0)

                        trow("MAKTX") = rowArt("MAKTX")
                        Dim strTemp As String = ""
                        If Not TypeOf (trow("LTEXT_NR")) Is DBNull Then
                            strTemp = CStr(trow("LTEXT_NR"))
                        End If
                        trow("LTEXT") = mLSTS.ReadStringERP(strTemp)

                        mUmlagerung.Rows.Add(trow)
                    Next
                    GeparktLoeschenERP(mstrBelegNR, "")
                    mstrBelegNR = ""
                End If
            End If
        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try

        If onlyread Then
            Return tblOnlyRead
        Else
            Return mUmlagerung
        End If

    End Function

    Public Sub GeparktLoeschenERP(ByVal BelNr As String, ByVal TextDel As String)
        ClearErrorState()

        Try
            SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)
            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()

            dt.Rows.Add(New Object() {"I_BELNR", False, Right("0000000000" & BelNr, 10), 10})
            dt.Rows.Add(New Object() {"I_TEXTDEL", False, TextDel, 1})

            SAPExc.ExecuteERP("Z_FIL_EFA_UML_PARK_DEL", dt)
            If (SAPExc.ErrorOccured) Then
                RaiseError(SAPExc.E_SUBRC, SAPExc.E_MESSAGE)
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

    Public Sub GetKennzFormERP(ByVal Matnr As String)
        ClearErrorState()

        Try
            SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)
            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()

            dt.Rows.Add(New Object() {"I_MATNR", False, Matnr, 18})
            dt.Rows.Add(New Object() {"GT_MAT", True})

            SAPExc.ExecuteERP("Z_FIL_EFA_UML_MAT_GROESSE", dt)
            If (SAPExc.ErrorOccured) Then
                RaiseError(SAPExc.E_SUBRC, SAPExc.E_MESSAGE)
            End If
            Dim retRows As DataRow = dt.Select("Fieldname='GT_MAT'")(0)
            If Not retRows Is Nothing Then
                mListeGroesse = DirectCast(retRows("Data"), DataTable)
            End If

        Catch ex As Exception
           RaiseError("9999", ex.Message)
        End Try

    End Sub

    Public Function GetAdresseLagerort(ByVal vkorg As String, ByVal vkbur As String) As DataTable
        Dim tblAdresse As New DataTable()

        ClearErrorState()

        Try
            SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)
            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()

            dt.Rows.Add(New Object() {"I_WERKS", False, Right("0000" & vkorg, 4), 4})
            dt.Rows.Add(New Object() {"I_LGORT", False, Right("0000" & vkbur, 4), 4})
            dt.Rows.Add(New Object() {"E_ADRC", True})

            SAPExc.ExecuteERP("Z_FIL_EFA_GET_LGORT_ADRESSE", dt)
            If (SAPExc.ErrorOccured) Then
                RaiseError(SAPExc.E_SUBRC, SAPExc.E_MESSAGE)
            End If
            Dim retRows As DataRow = dt.Select("Fieldname='E_ADRC'")(0)
            If Not retRows Is Nothing Then
                tblAdresse = DirectCast(retRows("Data"), DataTable)
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try

        Return tblAdresse
    End Function

End Class
