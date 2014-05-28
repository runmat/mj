Option Explicit On
Option Strict On

Imports CKG.Base.Common

Public Class Auswertung

    Private mDatumVon As String
    Private mDatumBis As String
    Private mAnzahl As String
    Private mLeistArt As DataTable
    Private tblArbeitsplan As New DataTable
    Private tblAuswertung As New DataTable
    Private tblGesamt As New DataTable

    Public Property DatumVon() As String
        Get
            Return mDatumVon
        End Get
        Set(ByVal value As System.String)
            mDatumVon = value
        End Set
    End Property

    Public Property DatumBis() As String
        Get
            Return mDatumBis
        End Get
        Set(ByVal value As System.String)
            mDatumBis = value
        End Set

    End Property
    Public Property LeistArt() As DataTable
        Get
            Return mLeistArt
        End Get
        Set(ByVal value As DataTable)
            mLeistArt = value
        End Set
    End Property
    Public ReadOnly Property Auswertung() As DataTable
        Get
            Return tblAuswertung
        End Get
    End Property
    Public ReadOnly Property Arbeitsplan() As DataTable
        Get
            Return tblArbeitsplan
        End Get
    End Property
    Public Property Gesamt() As DataTable
        Get
            Return tblGesamt
        End Get
        Set(ByVal value As DataTable)
            tblGesamt = value
        End Set
    End Property
    Public Property Anzahl() As String
        Get
            Return mAnzahl
        End Get
        Set(ByVal value As System.String)
            mAnzahl = value
        End Set

    End Property


    Public Sub GetData(ByRef objUser As Base.Kernel.Security.User, _
                              ByRef objApp As Base.Kernel.Security.App, _
                              ByVal page As Page)

        Dim intID As Int32 = -1

        Dim KUNNR As String = Right("0000000000" & objUser.Customer.KUNNR, 10)

        'Dim tmpRowRueckmeldung As DataRow
        'Dim MatchMitarbeiter As String = ""

        Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_COIH_READ_AUFTRAG_002", objApp, objUser, page)


        Try


            'Importparameter
            Dim Importtable As DataTable = myProxy.getImportTable("GS_IN")

            Dim dr As DataRow = Importtable.NewRow

            dr("KUNNR_AG") = KUNNR
            dr("AUTYP") = "30"
            If Not mDatumVon Is Nothing Then
                dr("IEDD_VON") = mDatumVon
            End If
            If Not mDatumBis Is Nothing Then
                dr("IEDD_BIS") = mDatumBis
            End If

            Dim tmpTable As DataTable

            If Not mLeistArt Is Nothing Then
                tmpTable = myProxy.getImportTable("GT_IN_LARNT")
                Dim NewRow As DataRow


                For Each LeistArtRow As DataRow In mLeistArt.Rows
                    NewRow = tmpTable.NewRow
                    NewRow("LARNT") = LeistArtRow("LARNT").ToString
                    tmpTable.Rows.Add(NewRow)
                Next


            End If

            Importtable.Rows.Add(dr)

            Importtable.AcceptChanges()


            myProxy.callBapi()

            tblArbeitsplan = myProxy.getExportTable("GT_AP")
            tblAuswertung = myProxy.getExportTable("GT_AUSWERTUNG")
            tblAuswertung.Columns.Add("LTXA1", GetType(System.String))

            mAnzahl = myProxy.getExportParameter("E_ANZ_AUF")
            If IsNumeric(mAnzahl) Then
                mAnzahl = CInt(mAnzahl).ToString
            End If

            If Not mLeistArt Is Nothing Then
                Dim NewAPRows As New DataTable
                NewAPRows = tblArbeitsplan.Clone
                For i As Integer = tblArbeitsplan.Rows.Count - 1 To 0 Step -1
                    Dim SelAPRows() As DataRow
                    Dim tmpBool As Boolean = True
                    For Each LeistArtRow As DataRow In mLeistArt.Rows
                        SelAPRows = tblArbeitsplan.Select("LARNT = '" & LeistArtRow("LARNT").ToString & "'")
                        If LeistArtRow("LARNT").ToString = tblArbeitsplan.Rows(i)("LARNT").ToString Then

                            Dim NewRow As DataRow = NewAPRows.NewRow
                            For i2 As Integer = 0 To tblArbeitsplan.Columns.Count - 1
                                NewRow(i2) = tblArbeitsplan.Rows(i)(i2).ToString
                            Next
                            NewAPRows.Rows.Add(NewRow)
                        End If

                    Next

                Next
                Arbeitsplan.Rows.Clear()

                For iCount As Integer = NewAPRows.Rows.Count - 1 To 0 Step -1
                    Dim NewAPRow As DataRow = Arbeitsplan.NewRow
                    For i2 As Integer = 0 To NewAPRows.Columns.Count - 1
                        NewAPRow(i2) = NewAPRows.Rows(iCount)(i2).ToString
                    Next
                    Arbeitsplan.Rows.Add(NewAPRow)
                Next

            End If

            Dim iLeistartCount As Integer = 0
            If tblAuswertung.Rows.Count > 0 Then

                For i = 0 To tblArbeitsplan.Rows.Count - 1
                    Dim drows() As DataRow = tblAuswertung.Select("LARNT = '" & tblArbeitsplan.Rows(i)("LARNT").ToString & "'")
                    For Each row As DataRow In drows
                        row("LTXA1") = tblArbeitsplan.Rows(i)("LTXA1").ToString
                    Next
                Next i

                tblGesamt = New DataTable
                tblGesamt.Columns.Add("Monat/Jahr", GetType(System.String))
                tblGesamt.Columns.Add("TG", GetType(System.String))
                For Each row As DataRow In tblArbeitsplan.Rows

                    Dim strLeistart As String = row("LTXA1").ToString
                    tblAuswertung.Select()
                    iLeistartCount += 1

                    If Not tblGesamt.Columns.Contains(strLeistart) Then
                        tblGesamt.Columns.Add(strLeistart, GetType(System.String))
                        tblGesamt.Columns.Add("Erl. in Proz." & iLeistartCount, GetType(System.String))
                    End If
                Next

            Else

                Throw New Exception("NO DATA")

            End If

            Dim tblAnzahl As DataTable
            tblAnzahl = Gesamt.Clone

            Dim drowsAusw() As DataRow = tblAuswertung.Select("LARNT = '" & tblArbeitsplan.Rows(0)("LARNT").ToString & "'" & _
                                                            " AND AUERU='X'")

            For Each row As DataRow In drowsAusw
                Dim NewRow As DataRow = tblAnzahl.NewRow
                NewRow("Monat/Jahr") = row("GMONAT").ToString & "/" & row("GJAHR").ToString
                NewRow("TG") = row("AUERU").ToString
                tblAnzahl.Rows.Add(NewRow)
            Next





            For iCount As Integer = 0 To tblAnzahl.Rows.Count - 1
                Dim MonatJahr As String = tblAnzahl.Rows(iCount)("Monat/Jahr").ToString
                Dim MonatArr() As String = Split(tblAnzahl.Rows(iCount)("Monat/Jahr").ToString, "/")
                Dim ArbeitsplanDetailRow() As DataRow
                ' For Each Planrow As DataRow In tblArbeitsplan.Rows
                ArbeitsplanDetailRow = tblAuswertung.Select(" GJAHR='" & MonatArr(1) & "'" & _
                                                            " AND GMONAT='" & MonatArr(0) & "'" & _
                                                            " AND AUERU='X'")
                '" AND AUERU='" & tblAnzahl.Rows(iCount)("TG").ToString & "'")

                If ArbeitsplanDetailRow.Length > 0 Then
                    Dim NewRow As DataRow = Gesamt.NewRow
                    For i2 As Integer = 0 To ArbeitsplanDetailRow.Length - 1
                        Dim RowAP() As DataRow = Arbeitsplan.Select("LARNT = '" & ArbeitsplanDetailRow(i2)("LARNT").ToString & "'")
                        NewRow(RowAP(0)("LTXA1").ToString) = ArbeitsplanDetailRow(i2)("ANZ_RUECKM").ToString
                        NewRow("Erl. in Proz." & i2 + 1) = ArbeitsplanDetailRow(i2)("PROZ_RUECKM").ToString
                    Next
                    NewRow("Monat/Jahr") = MonatJahr
                    NewRow("TG") = tblAnzahl.Rows(iCount)("TG").ToString
                    Gesamt.Rows.Add(NewRow)

                End If

                'Next
            Next





            'Next

        Catch ex As Exception

        Finally
            myProxy = Nothing
        End Try

    End Sub

End Class
