Option Explicit On
Option Strict On

Imports CKG.Base.Kernel.Security
Imports CKG.Base.Common
'Imports Microsoft.Data.SAPClient


Public Class Bestand
    Private tblPLangruppenzähler As New DataTable
    Private tblArbeitsplan As New DataTable
    Private tblKopfdaten As New DataTable
    Private tblPartner As New DataTable
    Private tblArbeitsplanDatumswerte As New DataTable
    Private tblGesamt As New DataTable
    Private tblRueckmeldung As New DataTable
    Private tblLeistungsart As New DataTable
    Private mArbeitsplatzUser As String
    Private mGridSort As String
    Private mGridPageIndex As Int32
    Private mGridPageSize As Integer
    Private mOffTeil As Boolean

    Private mFin As String
    Private mLvNummer As String
    Private mKennzeichen As String
    Private mMitarbeiternummer As String
    Private mHaltername As String
    Private mStrasse As String
    Private mPlzOrt As String
    Private mTelefon As String
    Private mMail As String
    Private mHinAb As String
    Private mHinAn As String
    Private mRueckAb As String
    Private mRueckAn As String
    Private mDienstCount As String
    Private mHinDifferenz As String
    Private mRueckDifferenz As String

    Private mGewählterPlangruppenzähler As String
    Private mArtMit As String
    Private mArtOhne As String
    Private mOp As String
    Private mMitDatum As String

    Public Property MitDatum() As String
        Get
            Return mMitDatum
        End Get
        Set(ByVal value As String)
            mMitDatum = value
        End Set
    End Property

    Public Property Op() As String
        Get
            Return mOp
        End Get
        Set(ByVal value As String)
            mOp = value
        End Set
    End Property

    Public Property ArtOhne() As String
        Get
            Return mArtOhne
        End Get
        Set(ByVal value As String)
            mArtOhne = value
        End Set
    End Property

    Public Property GewählterPlangruppenzähler() As String
        Get
            Return mGewählterPlangruppenzähler
        End Get
        Set(ByVal value As String)
            mGewählterPlangruppenzähler = value
        End Set
    End Property

    Public Property ArtMit() As String
        Get
            Return mArtMit
        End Get
        Set(ByVal value As String)
            mArtMit = value
        End Set
    End Property

    Public Property Fin() As String
        Get
            Return mFin
        End Get
        Set(ByVal value As String)
            mFin = value
        End Set
    End Property

    Public Property LvNummer() As String
        Get
            Return mLvNummer
        End Get
        Set(ByVal value As String)
            mLvNummer = value
        End Set
    End Property

    Public Property Kennzeichen() As String
        Get
            Return mKennzeichen
        End Get
        Set(ByVal value As String)
            mKennzeichen = value
        End Set
    End Property

    Public Property Mitarbeiternr() As String
        Get
            Return mMitarbeiternummer
        End Get
        Set(ByVal value As String)
            mMitarbeiternummer = value
        End Set
    End Property

    Public Property Haltername() As String
        Get
            Return mHaltername
        End Get
        Set(ByVal value As String)
            mHaltername = value
        End Set
    End Property

    Public Property Strasse() As String
        Get
            Return mStrasse
        End Get
        Set(ByVal value As String)
            mStrasse = value
        End Set
    End Property


    Public Property PlzOrt() As String
        Get
            Return mPlzOrt
        End Get
        Set(ByVal value As String)
            mPlzOrt = value
        End Set
    End Property


    Public Property Telefon() As String
        Get
            Return mTelefon
        End Get
        Set(ByVal value As String)
            mTelefon = value
        End Set
    End Property

    Public Property Mail() As String
        Get
            Return mMail
        End Get
        Set(ByVal value As String)
            mMail = value
        End Set
    End Property

    Public Property HinAb() As String
        Get
            Return mHinAb
        End Get
        Set(ByVal value As String)
            mHinAb = value
        End Set
    End Property

    Public Property HinAn() As String
        Get
            Return mHinAn
        End Get
        Set(ByVal value As String)
            mHinAn = value
        End Set
    End Property

    Public Property RueckAb() As String
        Get
            Return mRueckAb
        End Get
        Set(ByVal value As String)
            mRueckAb = value
        End Set
    End Property

    Public Property RueckAn() As String
        Get
            Return mRueckAn
        End Get
        Set(ByVal value As String)
            mRueckAn = value
        End Set
    End Property

    Public Property DienstCount() As String
        Get
            Return mDienstCount
        End Get
        Set(ByVal value As String)
            mDienstCount = value
        End Set
    End Property

    Public Property HinDifferenz() As String
        Get
            Return mHinDifferenz
        End Get
        Set(ByVal value As String)
            mHinDifferenz = value
        End Set
    End Property

    Public Property RueckDifferenz() As String
        Get
            Return mRueckDifferenz
        End Get
        Set(ByVal value As String)
            mRueckDifferenz = value
        End Set
    End Property

    Public ReadOnly Property Plangruppenzähler() As DataTable
        Get
            Return tblPLangruppenzähler
        End Get
    End Property

    Public ReadOnly Property Arbeitsplan() As DataTable
        Get
            Return tblArbeitsplan
        End Get
    End Property

    Public ReadOnly Property Leistungsart() As DataTable
        Get
            Return tblLeistungsart
        End Get
    End Property

    Public ReadOnly Property Kopfdaten() As DataTable
        Get
            Return tblKopfdaten
        End Get
    End Property

    Public ReadOnly Property Partner() As DataTable
        Get
            Return tblPartner
        End Get
    End Property

    Public ReadOnly Property Gesamt() As DataTable
        Get
            Return tblGesamt
        End Get
    End Property

    Public ReadOnly Property ArbeitsplanDatumswerte() As DataTable
        Get
            Return tblArbeitsplanDatumswerte
        End Get
    End Property

    Public ReadOnly Property Rueckmeldung() As DataTable
        Get
            Return tblRueckmeldung
        End Get
    End Property

    Public Property ArbeitsplatzUser() As String
        Get
            Return mArbeitsplatzUser
        End Get
        Set(ByVal value As String)
            mArbeitsplatzUser = value
        End Set
    End Property

    Public Property GridSort() As String
        Get
            Return mGridSort
        End Get
        Set(ByVal value As String)
            mGridSort = value
        End Set
    End Property

    Public Property GridPageIndex() As Int32
        Get
            Return mGridPageIndex
        End Get
        Set(ByVal value As Int32)
            mGridPageIndex = value
        End Set
    End Property

    Public Property GridPageSize() As Integer
        Get
            Return mGridPageSize
        End Get
        Set(ByVal value As Int32)
            mGridPageSize = value
        End Set
    End Property

    Public Property OffTeil() As Boolean
        Get
            Return mOffTeil
        End Get
        Set(ByVal value As Boolean)
            mOffTeil = value
        End Set
    End Property

    Public Property tblAbweichendeUrsachen As DataTable
    Public Property Kundenstatus As String

    Public Sub GetPlangruppenzähler(ByRef objUser As User, ByRef objApp As App, ByVal page As Page)

        Dim kunnr As String = Right("0000000000" & objUser.Customer.KUNNR, 10)

        Dim myProxy = DynSapProxy.getProxy("Z_M_IMP_AUFTRDAT_007", objApp, objUser, page)
        myProxy.setImportParameter("I_KUNNR", kunnr)
        myProxy.setImportParameter("I_KENNUNG", "PLANGRUPPENZÄHLER")
        myProxy.callBapi()

        tblPLangruppenzähler = myProxy.getExportTable("GT_WEB")
    End Sub

    Public Sub GetLeistungsart(ByRef objUser As User, ByRef objApp As App, ByVal page As Page)

        Dim kunnr As String = Right("0000000000" & objUser.Customer.KUNNR, 10)

        Dim myProxy = DynSapProxy.getProxy("Z_DPM_COIH_READ_AUFTRAG_001", objApp, objUser, page)

        'Importparameter
        Dim importtable = myProxy.getImportTable("GS_IN")

        Dim dr = importtable.NewRow
        dr("KUNNR_AG") = kunnr
        dr("NUR_AP") = "X"
        dr.SetField("VAGRP", "4")
        dr.SetField("STATU", "3")

        If Not String.IsNullOrEmpty(GewählterPlangruppenzähler) Then
            dr.SetField("PLNAL", GewählterPlangruppenzähler.PadLeft(2, "0"c))
        End If
        importtable.Rows.Add(dr)
        importtable.AcceptChanges()

        myProxy.callBapi()

        tblLeistungsart = myProxy.getExportTable("GT_AP")

        'mit arbeitsplatzeinschränkung
        mArbeitsplatzUser = objUser.Groups.ItemByID(objUser.GroupID).GroupName

        If tblLeistungsart.Select("ARBPL='" & mArbeitsplatzUser & "'").Length > 0 Then
            'wenn der arbeitsplatz enthalten ist, nur deren anzeigen, sonst alle
            tblLeistungsart.DefaultView.RowFilter = "ARBPL='" & objUser.Groups.ItemByID(objUser.GroupID).GroupName & "'"
        Else
            tblLeistungsart.DefaultView.RowFilter = ""
        End If

        'Dim ERR_AP As String = myProxy.getExportParameter("ERR_AP")
        'Dim ERR_AUF As String = myProxy.getExportParameter("ERR_AUF")
        'Dim ERR_RUECK As String = myProxy.getExportParameter("ERR_RUECK")
    End Sub


    Public Function GetKMStand(ByVal fahrgestellnummer As String, ByRef objUser As User, ByRef objApp As App,
                               ByVal page As Page) As DataTable

        Dim kunnr As String = Right("0000000000" & objUser.Customer.KUNNR, 10)

        Dim myProxy = DynSapProxy.getProxy("Z_DPM_COIH_READ_KM_STAND_001", objApp, objUser, page)

        'Importparameter
        Dim importtable = myProxy.getImportTable("GT_WEB")
        Dim dr = importtable.NewRow
        dr("KUNNR_AG") = kunnr
        dr("CHASSIS_NUM") = fahrgestellnummer
        importtable.Rows.Add(dr)
        importtable.AcceptChanges()

        myProxy.callBapi()

        Return myProxy.getExportTable("GT_WEB")
    End Function

    Public Sub GetAuswertung(ByRef objUser As User,
                             ByRef objApp As App,
                             ByVal page As Page)

        Dim kunnr = Right("0000000000" & objUser.Customer.KUNNR, 10)

        Dim tmpRowRueckmeldung As DataRow
        'Dim MatchMitarbeiter As String = ""

        Dim myProxy = DynSapProxy.getProxy("Z_DPM_COIH_READ_AUFTRAG_001", objApp, objUser, page)

        Try


            'Importparameter
            Dim importtable = myProxy.getImportTable("GS_IN")
            Dim dr = importtable.NewRow
            dr("KUNNR_AG") = kunnr
            dr("AUTYP") = "30"
            dr("LARNT_I") = mArtMit
            dr("LARNT_E") = mArtOhne
            dr("V_OP") = mOp
            dr("SORTL_ZH") = mMitarbeiternummer
            If mHaltername.Length > 0 Then
                dr("NAME1_ZH") = "*" + mHaltername + "*"
            End If

            If mMitDatum.Length > 0 Then
                dr("IEDD") = mMitDatum
            End If

            dr("ANLZU") = Kundenstatus

            dr.SetField("VAGRP", "4")
            dr.SetField("STATU", "3")

            If Not String.IsNullOrEmpty(GewählterPlangruppenzähler) Then
                dr.SetField("PLNAL", GewählterPlangruppenzähler.PadLeft(2, "0"c))
            End If

            importtable.Rows.Add(dr)
            importtable.AcceptChanges()

            If mFin.Length + mLvNummer.Length > 0 Then
                Dim identsTable = myProxy.getImportTable("GT_IN_IDENTS")
                Dim drIdents = identsTable.NewRow
                drIdents("CHASSIS_NUM") = mFin
                drIdents("LIZNR") = mLvNummer
                identsTable.Rows.Add(drIdents)
                identsTable.AcceptChanges()
            End If

            myProxy.callBapi()

            tblArbeitsplan = myProxy.getExportTable("GT_AP")
            tblKopfdaten = myProxy.getExportTable("GT_AUF_KOPF")
            tblPartner = myProxy.getExportTable("GT_AUF_PARTNER")
            tblArbeitsplanDatumswerte = myProxy.getExportTable("GT_AUF_RUECK")
            tblAbweichendeUrsachen = myProxy.getExportTable("GT_ABW_URS")

            If tblKopfdaten.Rows.Count > 0 Then

                'tbl für Rückmeldedialog
                tblRueckmeldung = New DataTable
                tblRueckmeldung.Columns.Add("Fahrgestellnummer", GetType(String))
                tblRueckmeldung.Columns.Add("Halter", GetType(String))
                tblRueckmeldung.Columns.Add("Leistungsart", GetType(String))
                tblRueckmeldung.Columns.Add("Postleitzahl", GetType(String))
                tblRueckmeldung.Columns.Add("Strasse", GetType(String))
                tblRueckmeldung.Columns.Add("Ort", GetType(String))
                tblRueckmeldung.Columns.Add("Telefon", GetType(String))
                tblRueckmeldung.Columns.Add("Email", GetType(String))
                tblRueckmeldung.Columns.Add("Endrueckmeldung", GetType(String))
                tblRueckmeldung.Columns.Add("Rueckmeldetext", GetType(String))
                tblRueckmeldung.Columns.Add("Auftragsnummer", GetType(String))
                tblRueckmeldung.Columns.Add("Arbeitsplatz", GetType(String))
                tblRueckmeldung.Columns.Add("Teilrückmeldung", GetType(String))
                tblRueckmeldung.Columns.Add("Prognosedatum", GetType(String))

                tblGesamt = New DataTable

                tblGesamt.Columns.Add("ANLZU", GetType(String))
                tblGesamt.Columns.Add("Auftragsnummer", GetType(String))
                tblGesamt.Columns.Add("Vertragsnummer", GetType(String))
                tblGesamt.Columns.Add("Fahrgestellnummer", GetType(String))
                tblGesamt.Columns.Add("Kennzeichen", GetType(String))
                tblGesamt.Columns.Add("Mitarbeiternummer", GetType(String))
                tblGesamt.Columns.Add("Haltername", GetType(String))
                tblGesamt.Columns.Add("Status", GetType(String))
                tblGesamt.Columns.Add("Strasse", GetType(String))
                tblGesamt.Columns.Add("PLZ", GetType(String))
                tblGesamt.Columns.Add("Ort", GetType(String))
                tblGesamt.Columns.Add("Telefon", GetType(String))
                tblGesamt.Columns.Add("Email", GetType(String))

                'KM-Stände
                tblGesamt.Columns.Add("KM_Hin_Abfahrt", GetType(String))
                tblGesamt.Columns.Add("KM_Hin_Ankunft", GetType(String))
                tblGesamt.Columns.Add("KM_Hin_Differenz", GetType(String))

                tblGesamt.Columns.Add("KM_Rueck_Abfahrt", GetType(String))
                tblGesamt.Columns.Add("KM_Rueck_Ankunft", GetType(String))
                tblGesamt.Columns.Add("KM_Rueck_Differenz", GetType(String))

                'Spalten aus Arbeitsplantabelle hinzufügen

                mArbeitsplatzUser = objUser.Groups.ItemByID(objUser.GroupID).GroupName
                If tblLeistungsart.Select("ARBPL='" & mArbeitsplatzUser & "'").Length > 0 Then
                    'wenn der arbeitsplatz enthalten ist, nur deren anzeigen, sonst alle
                    For Each dRow As DataRow In tblArbeitsplan.Select("ARBPL='" & mArbeitsplatzUser & "'")
                        With tblGesamt
                            .Columns.Add(dRow("LTXA1").ToString, GetType(String))
                        End With
                    Next
                Else
                    mArbeitsplatzUser = "showAll"
                    For Each dRow As DataRow In tblArbeitsplan.Rows
                        With tblGesamt
                            .Columns.Add(dRow("LTXA1").ToString, GetType(String))
                        End With
                    Next
                End If

                tblGesamt.AcceptChanges()

                For Each kopfRow As DataRow In tblKopfdaten.Rows

                    'MatchMitarbeiter = ""

                    Dim gesamtrow As DataRow = tblGesamt.NewRow
                    tmpRowRueckmeldung = tblRueckmeldung.NewRow

                    gesamtrow("ANLZU") = kopfRow("ANLZU")
                    gesamtrow("Auftragsnummer") = kopfRow("AUFNR")
                    gesamtrow("Vertragsnummer") = kopfRow("DEVICEID")
                    gesamtrow("Fahrgestellnummer") = kopfRow("KTEXT").ToString
                    gesamtrow("Kennzeichen") = kopfRow("LICENSE_NUM").ToString
                    gesamtrow("KM_Hin_Abfahrt") = kopfRow("ZABKM_H").ToString
                    gesamtrow("KM_Hin_Ankunft") = kopfRow("ZANKM_H").ToString

                    If kopfRow("ZABKM_H").ToString.Length > 0 AndAlso kopfRow("ZANKM_H").ToString.Length > 0 Then
                        gesamtrow("KM_Hin_Differenz") = (CInt(kopfRow("ZANKM_H")) - CInt(kopfRow("ZABKM_H"))).ToString
                    End If

                    gesamtrow("KM_Rueck_Abfahrt") = kopfRow("ZABKM_R").ToString
                    gesamtrow("KM_Rueck_Ankunft") = kopfRow("ZANKM_R").ToString

                    If kopfRow("ZABKM_R").ToString.Length > 0 AndAlso kopfRow("ZANKM_R").ToString.Length > 0 Then
                        gesamtrow("KM_Rueck_Differenz") = (CInt(kopfRow("ZANKM_R")) - CInt(kopfRow("ZABKM_R"))).ToString
                    End If

                    tmpRowRueckmeldung("Fahrgestellnummer") = kopfRow("KTEXT")

                    If String.IsNullOrEmpty(kopfRow("KUNNR_ZH").ToString) = False Then
                        Dim halterRow As DataRow() = tblPartner.Select("KUNNR=" & kopfRow("KUNNR_ZH").ToString)

                        'Partner(Halter) zuordnen
                        If halterRow.Length > 0 Then
                            'MatchMitarbeiter = halterRow(0)("SORTL").ToString

                            gesamtrow("Mitarbeiternummer") = halterRow(0)("SORTL")
                            gesamtrow("Haltername") = halterRow(0)("NAME1")
                            gesamtrow("Strasse") = halterRow(0)("STREET")
                            gesamtrow("PLZ") = halterRow(0)("POST_CODE1")
                            gesamtrow("Ort") = halterRow(0)("CITY1")
                            gesamtrow("Telefon") = halterRow(0)("TEL_NUMBER")
                            gesamtrow("Email") = halterRow(0)("SMTP_ADDR")

                            tmpRowRueckmeldung("Halter") = halterRow(0)("NAME1")
                            tmpRowRueckmeldung("Strasse") = halterRow(0)("STREET")
                            tmpRowRueckmeldung("Postleitzahl") = halterRow(0)("POST_CODE1")
                            tmpRowRueckmeldung("Ort") = halterRow(0)("CITY1")
                            tmpRowRueckmeldung("Telefon") = halterRow(0)("TEL_NUMBER")
                            tmpRowRueckmeldung("Email") = halterRow(0)("SMTP_ADDR")
                        End If
                    End If

                    If kopfRow("ABGESCHLOSSEN").ToString.ToUpper = "X" Then
                        gesamtrow("Haltername") = gesamtrow("Haltername").ToString & "*X*"
                    End If

                    Dim strSelectionArbeitsplatz As String = ""
                    If Not mArbeitsplatzUser = "showAll" Then
                        strSelectionArbeitsplatz = "AND ARBPL='" & mArbeitsplatzUser & "'"
                    End If

                    'Arbeitspläne zuordnen
                    Dim arbeitsplanDatumRow = tblArbeitsplanDatumswerte.Select("AUFNR=" & kopfRow("AUFNR").ToString)

                    If arbeitsplanDatumRow.Length > 0 Then
                        Dim countErledigt As Integer = 0
                        Dim gesamtAnzahl As Integer = 0

                        For i As Integer = 0 To arbeitsplanDatumRow.Length - 1
                            gesamtAnzahl = Arbeitsplan.Rows.Count

                            If arbeitsplanDatumRow(i)("AUERU").ToString.ToUpper = "X" Then
                                countErledigt += 1
                            End If

                            Dim tempRow =
                                    tblArbeitsplan.Select(
                                        "LARNT= '" & arbeitsplanDatumRow(i)("LEARR").ToString & "' " &
                                        strSelectionArbeitsplatz)

                            If tempRow.Length > 0 Then
                                If arbeitsplanDatumRow(i)("IEDD").ToString.Length > 10 Then

                                    gesamtrow(tempRow(0)("LTXA1").ToString) =
                                        Left(arbeitsplanDatumRow(i)("IEDD").ToString, 10)

                                    If Not arbeitsplanDatumRow(i)("AUERU").ToString.ToUpper = "X" Then
                                        If arbeitsplanDatumRow(i)("PEDD").ToString.Length > 10 Then
                                            gesamtrow(tempRow(0)("LTXA1").ToString) =
                                                Left(arbeitsplanDatumRow(i)("PEDD").ToString, 10).ToString & " *PD*"
                                            'Prognosedatum
                                            tmpRowRueckmeldung("Prognosedatum") =
                                                Left(arbeitsplanDatumRow(i)("PEDD").ToString, 10).ToString
                                        Else
                                            gesamtrow(tempRow(0)("LTXA1").ToString) =
                                                gesamtrow(tempRow(0)("LTXA1").ToString).ToString & " *TR*"
                                        End If

                                        tmpRowRueckmeldung("Teilrückmeldung") = "X"
                                    End If
                                Else
                                    gesamtrow(tempRow(0)("LTXA1").ToString) = arbeitsplanDatumRow(i)("IEDD").ToString
                                End If
                            End If
                        Next

                        gesamtrow("Status") = countErledigt & "/" & gesamtAnzahl
                    End If

                    ''Soll nach Mitarbeiternummer gefiltert werden?
                    'If mMitarbeiternummer.Length > 0 Then
                    '    If mMitarbeiternummer = MatchMitarbeiter Then
                    '        tblGesamt.Rows.Add(Gesamtrow)
                    '        tblRueckmeldung.Rows.Add(tmpRowRueckmeldung)
                    '    End If

                    'Else
                    tblGesamt.Rows.Add(gesamtrow)
                    tblRueckmeldung.Rows.Add(tmpRowRueckmeldung)
                    'End If
                Next

                tblGesamt.AcceptChanges()
                tblRueckmeldung.AcceptChanges()
            Else
                tblGesamt.Rows.Clear()
            End If

        Catch ex As Exception

        Finally
            myProxy = Nothing
        End Try
    End Sub


    Public Function SaveData(ByRef objUser As User,
                             ByRef objApp As App,
                             ByVal page As Page,
                             ByVal plangruppenzaehler As String,
                             ByVal vin As String,
                             ByVal Leistungsart As String,
                             ByVal endrueckmeldung As String,
                             ByVal rueckmeldetext As String,
                             ByVal leistungsartText As String,
                             ByVal prognoseEnd As String) As String

        Dim returnString As String = ""

        Try
            Dim kunnr As String = Right("0000000000" & objUser.Customer.KUNNR, 10)

            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_COIH_SAVE_RUECK_001", objApp, objUser, page)

            'Importparameter
            Dim importtable = myProxy.getImportTable("GT_IN_RUECK")
            Dim dr = importtable.NewRow
            dr("KUNNR_AG") = kunnr
            dr("AUART") = "ZS02"
            dr("PLNAL") = plangruppenzaehler
            dr("CHASSIS_NUM") = vin
            dr("LARNT") = Leistungsart
            dr("IEDD") = Date.Today.ToShortDateString
            dr("LTXA1") = rueckmeldetext
            dr("AUERU") = endrueckmeldung
            If IsDate(prognoseEnd) Then dr("PEDD") = prognoseEnd
            importtable.Rows.Add(dr)
            importtable.AcceptChanges()

            myProxy.callBapi()

            'Rückgabetabelle
            Dim errFlag As String = myProxy.getExportParameter("FLAG_ERROR")

            If Trim(errFlag).Length = 0 Then
                tblGesamt.Select("Fahrgestellnummer='" & vin & "'")(0)(leistungsartText) =
                    Date.Today.ToShortDateString & IIf(endrueckmeldung = "", " *TR*", "").ToString
                tblGesamt.AcceptChanges()
            Else
                returnString = "Fehler beim Sichern der Daten."
            End If
        Catch ex As Exception
            returnString = "Fehler beim Sichern der Daten."
        End Try

        Return returnString
    End Function


    Public Sub SaveStatus(ByRef objUser As User,
                          ByRef objApp As App,
                          ByVal page As Page, status As String,
                          ByVal aufnr As String)

        Try

            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_COIH_CHANGE_AUFTRAG_002", objApp, objUser, page)

            'Importparameter
            Dim importtable = myProxy.getImportTable("GS_IN")
            Dim dr = importtable.NewRow
            dr("AUFNR") = aufnr
            dr("ANLZU") = status
            importtable.Rows.Add(dr)
            importtable.AcceptChanges()

            myProxy.callBapi()

        Catch ex As Exception

        End Try
    End Sub
End Class


' ************************************************
' $History: Bestand.vb $
' 
' *****************  Version 30  *****************
' User: Fassbenders  Date: 25.05.11   Time: 11:06
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing/lib
' 
' *****************  Version 29  *****************
' User: Fassbenders  Date: 4.08.10    Time: 13:42
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing/lib
' 
' *****************  Version 28  *****************
' User: Rudolpho     Date: 28.07.10   Time: 15:05
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing/lib
' 
' *****************  Version 27  *****************
' User: Rudolpho     Date: 28.07.10   Time: 9:23
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing/lib
' 
' *****************  Version 26  *****************
' User: Rudolpho     Date: 23.07.10   Time: 16:20
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing/lib
' 
' *****************  Version 25  *****************
' User: Rudolpho     Date: 23.07.10   Time: 11:16
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing/lib
' 
' *****************  Version 24  *****************
' User: Rudolpho     Date: 23.07.10   Time: 10:53
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing/lib
' 
' *****************  Version 23  *****************
' User: Fassbenders  Date: 15.07.10   Time: 14:48
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing/lib
' 
' *****************  Version 22  *****************
' User: Fassbenders  Date: 14.07.10   Time: 16:24
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing/lib
' 
' *****************  Version 21  *****************
' User: Fassbenders  Date: 13.07.10   Time: 13:54
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing/lib
' 
' *****************  Version 20  *****************
' User: Fassbenders  Date: 12.07.10   Time: 8:47
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing/lib
' 
' *****************  Version 19  *****************
' User: Fassbenders  Date: 9.07.10    Time: 11:23
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing/lib
' 
' *****************  Version 18  *****************
' User: Fassbenders  Date: 8.07.10    Time: 13:21
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing/lib
' 
' *****************  Version 17  *****************
' User: Fassbenders  Date: 8.07.10    Time: 10:13
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing/lib
' 
' *****************  Version 16  *****************
' User: Fassbenders  Date: 5.07.10    Time: 17:06
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing/lib
' 
' *****************  Version 15  *****************
' User: Fassbenders  Date: 16.06.10   Time: 14:03
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing/lib
' 
' *****************  Version 14  *****************
' User: Fassbenders  Date: 15.06.10   Time: 12:59
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing/lib
' ITA: 3829
' 
' *****************  Version 13  *****************
' User: Fassbenders  Date: 2.06.10    Time: 10:36
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing/lib
' ITA: 3754
' 
' *****************  Version 12  *****************
' User: Fassbenders  Date: 1.06.10    Time: 14:35
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing/lib
' 
' *****************  Version 11  *****************
' User: Fassbenders  Date: 31.05.10   Time: 17:34
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing/lib
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 20.05.10   Time: 13:35
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing/lib
' ITA 3738 testfertig
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 19.05.10   Time: 23:11
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing/lib
' ITA 3738
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 18.05.10   Time: 20:37
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing/lib
' ITA 3754 unfertig
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 18.05.10   Time: 17:58
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing/lib
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 17.05.10   Time: 21:08
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing/lib
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 17.05.10   Time: 12:00
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing/lib
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 14.05.10   Time: 16:59
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing/lib
'
' ************************************************