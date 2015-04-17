Imports KBSBase

Public Class Umlagerung
    Inherits ErrorHandlingClass

    Private mUmlagerung As DataTable
    Private mArtikel As DataTable
    Private mListeGroesse As DataTable
    Private mstrKostStelle As String = ""
    Private mstrKostStelleNeu As String = ""
    Private mstrKostText As String = ""
    Private mstrBelegNr As String = ""
    Private mstrBelegNrParken As String = ""
    Private mstrFilePath As String = ""
    Private mstrMandant As String = ""

#Region "Properties"

    Public ReadOnly Property Umlagerung() As DataTable
        Get
            Return mUmlagerung
        End Get
    End Property

    Public ReadOnly Property Artikel() As DataTable
        Get
            Return mArtikel
        End Get
    End Property

    Public ReadOnly Property ListeGroesse() As DataTable
        Get
            Return mListeGroesse
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

    Public Property BelegNr() As String
        Get
            Return mstrBelegNr
        End Get
        Set(ByVal Value As String)
            mstrBelegNr = Value
        End Set
    End Property

    Public ReadOnly Property BelegNrParken() As String
        Get
            Return mstrBelegNrParken
        End Get
    End Property

    Public Property FilePath() As String
        Get
            Return mstrFilePath
        End Get
        Set(ByVal Value As String)
            mstrFilePath = Value
        End Set
    End Property

    Public Property Mandant() As String
        Get
            Return mstrMandant
        End Get
        Set(ByVal Value As String)
            mstrMandant = Value
        End Set
    End Property

#End Region

    Public Sub New()
        If mUmlagerung Is Nothing Then
            mUmlagerung = New DataTable

            mUmlagerung.Columns.Add("POSNR", String.Empty.GetType)
            mUmlagerung.Columns.Add("MATNR", String.Empty.GetType)
            mUmlagerung.Columns.Add("MAKTX", String.Empty.GetType)
            mUmlagerung.Columns.Add("EAN11", String.Empty.GetType)
            mUmlagerung.Columns.Add("Menge", Type.GetType("System.Int32"))
            mUmlagerung.Columns.Add("LTEXT_NR", String.Empty.GetType)
            mUmlagerung.Columns.Add("LTEXT", String.Empty.GetType)
            mUmlagerung.Columns.Add("KENNZFORM", String.Empty.GetType)
        End If

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
                tmpRow.Item("MAKTX") = Artikelbezeichnung
                tmpRow.Item("EAN11") = EAN
                tmpRow("Menge") = Menge
                tmpRow("LTEXT_NR") = LTextNR
                tmpRow.Item("LTEXT") = ""
                tmpRow.Item("KENNZFORM") = Kennzform
            Else
                tmpRow = mUmlagerung.NewRow()
                tmpRow.Item("MATNR") = Artikelnr
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

    ''' <summary>
    ''' Artikelliste laden
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ShowERP()
        ClearErrorState()

        Try
            S.AP.Init("Z_FIL_EFA_UML_MAT", "I_KOSTL", mstrKostStelle)

            S.AP.Execute()

            If S.AP.ResultCode = 0 Then
                mArtikel = S.AP.GetExportTable("GT_MAT")
            Else
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Umlagerung speichern
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ChangeERP()
        ClearErrorState()

        If Umlagerung.Rows.Count = 0 Then
            RaiseError("9999", "Ausgewählte Artikel konnten nicht gefunden werden!")
            Exit Sub
        End If

        Dim LSTS As New LongStringToSap

        Try
            S.AP.Init("Z_FIL_EFA_UML_STEP1")
            Dim tblSAP As DataTable = S.AP.GetImportTable("GT_MAT")

            For Each tmprow As DataRow In Umlagerung.Rows
                Dim tmpSAPRow As DataRow = tblSAP.NewRow
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

            'erneutes Init erforderlich nach Aufruf der Langtext-Bapis
            S.AP.Init("Z_FIL_EFA_UML_STEP1")

            S.AP.SetImportParameter("I_KOSTL_IN", mstrKostStelleNeu)
            S.AP.SetImportParameter("I_KOSTL_OUT", mstrKostStelle)
            S.AP.SetImportParameter("I_BUDAT", DateTime.Today)

            If Not String.IsNullOrEmpty(mstrBelegNrParken) Then
                S.AP.SetImportParameter("I_BELNR", mstrBelegNrParken.PadLeft(10, "0"c))
            End If

            S.AP.Execute()

            If S.AP.ResultCode = 0 Then
                mstrBelegNrParken = ""

                Dim BelegTable As DataTable = S.AP.GetExportTable("GT_BELNR")
                If BelegTable.Rows.Count > 0 Then
                    For i = 0 To BelegTable.Rows.Count - 1
                        mstrBelegNr += BelegTable.Rows(i)("BELNR").ToString & vbCrLf
                    Next
                End If

                If String.IsNullOrEmpty(mstrBelegNr) Then
                    RaiseError("9999", "Es wurde keine Belegnummer erzeugt!")
                End If
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
            S.AP.Init("Z_FIL_EFA_GET_KOSTL", "I_KOSTL_SEND, I_KOSTL_RECEIVE", KostStelle.PadLeft(10, "0"c), NeuKost.PadLeft(10, "0"c))

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

    Public Function GetListeAusparkenERP() As DataTable
        ClearErrorState()

        Dim tblListeAusparken As New DataTable()

        Try
            S.AP.Init("Z_FIL_EFA_UML_PARK_LISTE", "I_KOSTL_IN", mstrKostStelle)

            S.AP.Execute()

            If S.AP.ResultCode <> 0 Then
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

            tblListeAusparken = S.AP.GetExportTable("GT_PARK_LISTE")

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try

        Return tblListeAusparken
    End Function

    Public Sub ParkenERP()
        ClearErrorState()

        Try
            If Umlagerung.Rows.Count = 0 Then
                RaiseError("0001", "Keine Datensätze für Umlagerung vorhanden!")
                Exit Sub
            End If

            Dim LSTS As New LongStringToSap

            If String.IsNullOrEmpty(mstrBelegNrParken) Then
                'Insert
                S.AP.Init("Z_FIL_EFA_UML_PARK_INS")
                Dim tblTemp As DataTable = S.AP.GetImportTable("GT_MAT")

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

                'erneutes Init erforderlich nach Aufruf der Langtext-Bapis
                S.AP.Init("Z_FIL_EFA_UML_PARK_INS", "I_KOSTL_IN, I_KOSTL_OUT", mstrKostStelleNeu, mstrKostStelle)
            Else
                'Update
                S.AP.Init("Z_FIL_EFA_UML_PARK_UPD")
                Dim tblKopf As DataTable = S.AP.GetImportTable("IS_UMLK_PARK")

                Dim kRow As DataRow = tblKopf.NewRow()
                kRow("MANDT") = mstrMandant
                kRow("BELNR") = mstrBelegNrParken.PadLeft(10, "0"c)
                kRow("LGORT_SEND") = mstrKostStelle
                kRow("LGORT_EMPF") = mstrKostStelleNeu
                tblKopf.Rows.Add(kRow)

                Dim tblPos As DataTable = S.AP.GetImportTable("GT_UMLP_PARK")

                For Each row As DataRow In Umlagerung.Rows
                    Dim tRow As DataRow = tblPos.NewRow()
                    tRow("MANDT") = mstrMandant
                    tRow("BELNR") = mstrBelegNrParken.PadLeft(10, "0"c)
                    tRow("POSNR") = row("POSNR")
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

                    tblPos.Rows.Add(tRow)
                Next

                'erneutes Init erforderlich nach Aufruf der Langtext-Bapis
                S.AP.Init("Z_FIL_EFA_UML_PARK_UPD")
            End If

            S.AP.Execute()

            If S.AP.ResultCode = 0 Then
                mUmlagerung.Rows.Clear()
                mstrKostStelleNeu = ""
                mstrBelegNrParken = ""
            Else
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

    Public Function AusparkenERP(ByVal BelNr As String) As DataTable
        ClearErrorState()

        Try
            SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)
            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()

            dt.Rows.Add(New Object() {"I_BELNR", False, BelNr.PadLeft(10, "0"c), 10})
            dt.Rows.Add(New Object() {"ES_UMLK_PARK", True})
            dt.Rows.Add(New Object() {"GT_UMLP_PARK", True})

            SAPExc.ExecuteERP("Z_FIL_EFA_UML_PARK_READ", dt)

            If (SAPExc.ErrorOccured) Then
                RaiseError(SAPExc.E_SUBRC, SAPExc.E_MESSAGE)
            Else
                Dim retRows As DataRow = dt.Select("Fieldname='ES_UMLK_PARK'")(0)
                Dim tblTemp As DataTable
                If Not retRows Is Nothing Then
                    tblTemp = DirectCast(retRows("Data"), DataTable)
                    If tblTemp.Rows.Count > 0 Then
                        mstrMandant = tblTemp.Rows(0)("MANDT")
                        mstrBelegNrParken = tblTemp.Rows(0)("BELNR")
                        mstrKostStelleNeu = tblTemp.Rows(0)("LGORT_EMPF")
                    End If
                End If

                mUmlagerung.Rows.Clear()

                Dim tblTemp2 As New DataTable
                retRows = dt.Select("Fieldname='GT_UMLP_PARK'")(0)
                If Not retRows Is Nothing Then
                    tblTemp2 = DirectCast(retRows("Data"), DataTable)
                End If
                Dim mLSTS As New LongStringToSap()

                For Each row As DataRow In tblTemp2.Rows
                    Dim trow As DataRow = mUmlagerung.NewRow
                    trow("POSNR") = row("POSNR")
                    trow("MATNR") = row("MATNR")
                    trow("MENGE") = CInt(row("MENGE"))
                    trow("EAN11") = row("EAN11")
                    trow("LTEXT_NR") = row("LTEXT_NR")
                    trow("KENNZFORM") = row("KENNZFORM")
                    Dim rowArt As DataRow = mArtikel.Select("MATNR='" & CStr(trow("MATNR") & "'"))(0)

                    trow("MAKTX") = rowArt("MAKTX")
                    Dim strTemp As String = ""
                    If Not TypeOf (trow("LTEXT_NR")) Is DBNull Then
                        strTemp = CStr(trow("LTEXT_NR"))
                    End If
                    trow("LTEXT") = mLSTS.ReadStringERP(strTemp)

                    mUmlagerung.Rows.Add(trow)
                Next
            End If
        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try

        Return mUmlagerung

    End Function

    Public Sub GeparktLoeschenERP(ByVal BelNr As String, ByVal TextDel As String)
        ClearErrorState()

        Try
            If Not String.IsNullOrEmpty(mstrBelegNrParken) AndAlso mstrBelegNrParken.PadLeft(10, "0"c) = BelNr.PadLeft(10, "0"c) Then
                mstrBelegNrParken = ""
            End If

            SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)
            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()

            dt.Rows.Add(New Object() {"I_BELNR", False, BelNr.PadLeft(10, "0"c), 10})
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

            dt.Rows.Add(New Object() {"I_WERKS", False, vkorg, 4})
            dt.Rows.Add(New Object() {"I_LGORT", False, vkbur, 4})
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
