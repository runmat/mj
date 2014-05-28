Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports CKG.Base.Common

Public Class ec_01
    REM § Status-Report, Kunde: ALD, BAPI: Z_V_Ueberf_Auftr_Kund_Port,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase ' FFD_Bank_Datenimport

#Region " Declarations"
    Private tblHersteller As DataTable
    Private tblVerwendung As DataTable
    Private tblModell As DataTable
    Private tblUnitnummern As DataTable

    Private strModelId As String
    Private strModellBezeichnung As String
    Private strSippCode As String
    Private strHersteller As String
    Private strHerstellerBezeichnung As String
    Private lngBatchId As Long
    Private strDatumEinsteuerung As String
    Private intAnzahlKfz As Integer
    Private strUnitnrVon As String
    Private strUnitnrBis As String
    Private intLaufzeit As Integer
    Private blnLaufzeitbindung As Boolean
    Private strBemerkungen As String
    Private strAuftragsnrVon As String
    Private strAuftragsnrBis As String
    Private strVerwendungszweck As String
    Private strVerwendungszweckBezeichnung As String
    Private blnFahrzeuggruppe As Boolean
    Private blnWinterbereifung As Boolean
    Private blnAnhaengerkupplung As Boolean
    Private blnSecurityFleet As Boolean
    Private blnNavigationssystem As Boolean
    Private intSelectedRow As Integer
    Private strSelectionId As String
    Private blnLeasing As Boolean
    Private strKennzeichenSerie As String
#End Region

#Region " Properties"

    ReadOnly Property HerstellerAuswahl() As DataTable
        Get
            Return tblHersteller
        End Get
    End Property

    ReadOnly Property VerwendungszweckAuswahl() As DataTable
        Get
            Return tblVerwendung
        End Get
    End Property

    ReadOnly Property ModellAuswahl() As DataTable
        Get
            Return tblModell
        End Get
    End Property

    Property Unitnummern() As DataTable
        Get
            Return tblUnitnummern
        End Get
        Set(value As DataTable)
            tblUnitnummern = value
        End Set
    End Property

    Property ModelID() As String
        Get
            Return strModelId
        End Get
        Set(ByVal Value As String)
            strModelId = Value
        End Set
    End Property

    Property ModellBezeichnung() As String
        Get
            Return strModellBezeichnung
        End Get
        Set(ByVal Value As String)
            strModellBezeichnung = Value
        End Set
    End Property

    Property KennzeichenSerie() As String
        Get
            Return strKennzeichenSerie
        End Get
        Set(ByVal Value As String)
            strKennzeichenSerie = Value
        End Set
    End Property

    Property SippCode() As String
        Get
            Return strSippCode
        End Get
        Set(ByVal Value As String)
            strSippCode = Value
        End Set
    End Property

    Property Hersteller() As String
        Get
            Return strHersteller
        End Get
        Set(ByVal Value As String)
            strHersteller = Value
        End Set
    End Property

    Property HerstellerBezeichnung() As String
        Get
            Return strHerstellerBezeichnung
        End Get
        Set(ByVal Value As String)
            strHerstellerBezeichnung = Value
        End Set
    End Property

    Property BarchId() As Long
        Get
            Return lngBatchId
        End Get
        Set(ByVal Value As Long)
            lngBatchId = Value
        End Set
    End Property

    Property DatumEinsteuerung() As String
        Get
            Return strDatumEinsteuerung
        End Get
        Set(ByVal Value As String)
            strDatumEinsteuerung = Value
        End Set
    End Property

    Property AnzahlFahrzeuge() As Integer
        Get
            Return intAnzahlKfz
        End Get
        Set(ByVal Value As Integer)
            intAnzahlKfz = Value
        End Set
    End Property

    Property UnitNrVon() As String
        Get
            Return strUnitnrVon
        End Get
        Set(ByVal Value As String)
            strUnitnrVon = Value
        End Set
    End Property

    Property UnitNrBis() As String
        Get
            Return strUnitnrBis
        End Get
        Set(ByVal Value As String)
            strUnitnrBis = Value
        End Set
    End Property

    Property Laufzeit() As Integer
        Get
            Return intLaufzeit
        End Get
        Set(ByVal Value As Integer)
            intLaufzeit = Value
        End Set
    End Property

    Property LaufzeitBindung() As Boolean
        Get
            Return blnLaufzeitbindung
        End Get
        Set(ByVal Value As Boolean)
            blnLaufzeitbindung = Value
        End Set
    End Property

    Property Leasing() As Boolean
        Get
            Return blnLeasing
        End Get
        Set(ByVal Value As Boolean)
            blnLeasing = Value
        End Set
    End Property


    Property Bemerkungen() As String
        Get
            Return strBemerkungen
        End Get
        Set(ByVal Value As String)
            strBemerkungen = Value
        End Set
    End Property

    Property AuftragsNrVon() As String
        Get
            Return strAuftragsnrVon
        End Get
        Set(ByVal Value As String)
            strAuftragsnrVon = Value
        End Set
    End Property

    Property AuftragsNrBis() As String
        Get
            Return strAuftragsnrBis
        End Get
        Set(ByVal Value As String)
            strAuftragsnrBis = Value
        End Set
    End Property

    Property Verwendungszweck() As String
        Get
            Return strVerwendungszweck
        End Get
        Set(ByVal Value As String)
            strVerwendungszweck = Value
        End Set
    End Property

    Property VerwendungszweckBezeichnung() As String
        Get
            Return strVerwendungszweckBezeichnung
        End Get
        Set(ByVal Value As String)
            strVerwendungszweckBezeichnung = Value
        End Set
    End Property

    Property Fahrzeuggruppe() As Boolean
        Get
            Return blnFahrzeuggruppe
        End Get
        Set(ByVal Value As Boolean)
            blnFahrzeuggruppe = Value
        End Set
    End Property

    Property WinterBereifung() As Boolean
        Get
            Return blnWinterbereifung
        End Get
        Set(ByVal Value As Boolean)
            blnWinterbereifung = Value
        End Set
    End Property

    Property Anhaengerkupplung() As Boolean
        Get
            Return blnAnhaengerkupplung
        End Get
        Set(ByVal Value As Boolean)
            blnAnhaengerkupplung = Value
        End Set
    End Property

    Property SecurFleet() As Boolean
        Get
            Return blnSecurityFleet
        End Get
        Set(ByVal Value As Boolean)
            blnSecurityFleet = Value
        End Set
    End Property

    Property Navi() As Boolean
        Get
            Return blnNavigationssystem
        End Get
        Set(ByVal Value As Boolean)
            blnNavigationssystem = Value
        End Set
    End Property

    Property Selection() As Integer
        Get
            Return intSelectedRow
        End Get
        Set(ByVal Value As Integer)
            intSelectedRow = Value
        End Set
    End Property

    Property SelectionId() As String
        Get
            Return strSelectionId
        End Get
        Set(ByVal Value As String)
            strSelectionId = Value
        End Set
    End Property

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
        createResultTable()
        intSelectedRow = -1
    End Sub

    Public Sub saveData(ByVal strAppID As String, ByVal strSessionID As String, ByVal row As DataRow, ByVal page As Page)
        m_strClassAndMethod = "ec_01.saveData"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0

        Try

            ' Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Ec_Avm_Batch_Insert", m_objApp, m_objUser, Page)
            S.AP.Init("Z_M_Ec_Avm_Batch_Insert")

            'Dim SAPTable As DataTable = myProxy.getImportTable("ZBATCH_IN")
            Dim SAPTable As DataTable = S.AP.GetImportTable("ZBATCH_IN")
            Dim SAPTableRow As DataRow

            SAPTableRow = SAPTable.NewRow

            With SAPTableRow
                .Item("Zbatch_Id") = row("BatchId").ToString
                .Item("Zmodel_Id") = row("ModelId").ToString
                .Item("Zsipp_Code") = row("SippCode").ToString
                .Item("Zmake") = row("HerstellerBezeichnung").ToString
                .Item("Zmod_Descr") = row("ModellBezeichnung").ToString
                .Item("Zpurch_Mth") = row("DatumEinsteuerung").ToString
                .Item("Zanzahl") = row("AnzahlFahrzeuge").ToString
                .Item("Zunit_Nr_Von") = row("UnitNrVon").ToString
                .Item("Zunit_Nr_Bis") = row("UnitNrBis").ToString

                If KennzeichenSerie.IndexOf("(") > -1 Then
                    .Item("Zsonderserie") = KennzeichenSerie.Substring(KennzeichenSerie.IndexOf("(") + 1, 1)
                Else
                    .Item("Zsonderserie") = ""
                End If

                '§§§ JVE 19.09.2006 Korrektur!
                If (CType(row("Fahrzeuggruppe"), Boolean) = True) Then
                    '.Zfzg_Group = "LKW"
                    .Item("Zfzg_Group") = "PKW"
                Else
                    '.Zfzg_Group = "PKW"
                    .Item("Zfzg_Group") = "LKW"
                End If
                '------------------------------

                .Item("Zlaufzeit") = row("Laufzeit").ToString

                If (CType(row("LaufzeitBindung"), Boolean) = True) Then
                    .Item("Zlzbindung") = "X"
                Else
                    .Item("Zlzbindung") = ""
                End If

                .Item("Zaufnr_Von") = row("AuftragsNrVon").ToString
                .Item("Zaufnr_Bis") = row("AuftragsNrBis").ToString

                If (CType(row("Winterbereifung"), Boolean) = True) Then
                    .Item("Zms_Reifen") = "X"
                Else
                    .Item("Zms_Reifen") = ""
                End If




                If (CType(row("SecurFleet"), Boolean) = True) Then
                    .Item("Zsecu_Fleet") = "X"
                Else
                    .Item("Zsecu_Fleet") = ""
                End If

                If (CType(row("Leasing"), Boolean) = True) Then
                    .Item("Zleasing") = "X"
                Else
                    .Item("Zleasing") = ""
                End If

                If (CType(row("NavigationsSystem"), Boolean) = True) Then
                    .Item("Znavi") = "X"
                Else
                    .Item("Znavi") = ""
                End If

                If (CType(row("Anhaengerkupplung"), Boolean) = True) Then
                    .Item("ZAHK") = "X"
                Else
                    .Item("ZAHK") = ""
                End If


                .Item("Zverwendung") = row("Verwendungszweck").ToString

                .Item("Zbemerkung") = row("Bemerkungen").ToString
                .Item("Zernam") = Left(m_objUser.UserName, 11)
            End With
            SAPTable.Rows.Add(SAPTableRow)

            If tblUnitnummern IsNot Nothing Then
                Dim unitNrTable As DataTable = S.AP.GetImportTable("GT_IN") 'myProxy.getImportTable("GT_IN")
                Dim unitNrTableRow As DataRow
                For Each zeile As DataRow In tblUnitnummern.Rows
                    unitNrTableRow = unitNrTable.NewRow
                    unitNrTableRow.Item("ZUNIT_NR") = zeile("ZUNIT_NR").ToString
                    unitNrTable.Rows.Add(unitNrTableRow)
                Next
            End If

            'myProxy.callBapi()
            S.AP.Execute()

            'Report-Logeintrag (ok)
            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "FALS_INTERVALL"
                    m_intStatus = -1234
                    m_strMessage = "Ungültiges Intervall."
                Case "NO_INSERT"
                    m_intStatus = -1235
                    m_strMessage = "Fehler beim Einfügen in die Tabelle."
                Case "EXISTENCE_BATCH"
                    m_intStatus = -1236
                    m_strMessage = "Batch bereits vorhanden."
                Case "EXISTENCE_UNITNR"
                    m_intStatus = -1237
                    m_strMessage = "Unit-Nr. bereits vorhanden."
                Case "NO_BATCH_ID"
                    m_intStatus = -1238
                    m_strMessage = "Batch-Id fehlt."
                Case "NO_MODEL_ID"
                    m_intStatus = -1239
                    m_strMessage = "Model-Id fehlt."
                Case "NO_ZSIPP_CODE"
                    m_intStatus = -1244
                    m_strMessage = "SIPP-Code fehlt."
                Case "NO_MAKE"
                    m_intStatus = -1254
                    m_strMessage = "Hersteller fehlt."
                Case "NO_MOD_DESCR"
                    m_intStatus = -1264
                    m_strMessage = "Modellbeziechnung fehlt."
                Case "NO_PURCH_MTH"
                    m_intStatus = -1274
                    m_strMessage = "Einsteuerungsmonat fehlt."
                Case "NO_ANZAHL"
                    m_intStatus = -1284
                    m_strMessage = "Fahrzeuganzahl fehlt."
                Case "ERR_ANZ_FZG"
                    m_intStatus = -1285
                    m_strMessage = "Ungültige Fahrzeuganzahl."
                Case "NO_FZG_GROUP"
                    m_intStatus = -1294
                    m_strMessage = "Fahrzeuggruppe fehlt."
                Case "NO_LAUFZEIT"
                    m_intStatus = -1434
                    m_strMessage = "Laufzeit passt nicht zur Laufzeitbindung."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
            End Select

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        End Try

    End Sub

    Public Sub getData(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        m_strClassAndMethod = "ec_01.getData"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Try
            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Ec_Avm_Herst_Vwzweck_Modid", m_objApp, m_objUser, page)
            S.AP.InitExecute("Z_M_Ec_Avm_Herst_Vwzweck_Modid")
            Dim row As DataRow

            'myProxy.callBapi()

            'Tabellen formatieren
            'tblHersteller = myProxy.getExportTable("GT_HERST")
            'tblVerwendung = myProxy.getExportTable("GT_VERW")
            'tblModell = myProxy.getExportTable("GT_MODELID")

            tblHersteller = S.AP.GetExportTable("GT_HERST")
            tblVerwendung = S.AP.GetExportTable("GT_VERW")
            tblModell = S.AP.GetExportTable("GT_MODELID")

            'Sipp-Code zusammensetzen
            tblModell.Columns.Add("Sipp", GetType(System.String))

            For Each row In tblModell.Rows
                row("Sipp") = CStr(row("SIPP1")) & CStr(row("SIPP2")) & CStr(row("SIPP3")) & CStr(row("SIPP4"))
            Next
            tblModell.AcceptChanges()

            'Report-Logeintrag (ok)
            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_intStatus = -1234
                    m_strMessage = "Fehler: Keine Daten gefunden!"
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
            End Select
            tblHersteller = Nothing
            tblVerwendung = Nothing

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        End Try
    End Sub

    Public Sub getRowData(ByRef status As String)
        Dim row As DataRow()

        status = String.Empty
        row = ResultTable.Select("RowId='" & strSelectionId & "'")     'Zeile suchen....
        If (row.Length = 0) Then
            status = "Datensatz konnte nicht geladen werden."
        Else
            strModelId = CType(row(0)("ModelId"), String)
            strModellBezeichnung = CType(row(0)("ModellBezeichnung"), String)
            strSippCode = CType(row(0)("SippCode"), String)
            strHersteller = CType(row(0)("Hersteller"), String)
            strHerstellerBezeichnung = CType(row(0)("HerstellerBezeichnung"), String)
            lngBatchId = CType(row(0)("BatchId"), Long)
            strDatumEinsteuerung = CType(row(0)("DatumEinsteuerung"), String)
            intAnzahlKfz = CType(row(0)("AnzahlFahrzeuge"), Integer)
            strUnitnrVon = CType(row(0)("UnitNrVon"), String)
            strUnitnrBis = CType(row(0)("UnitNrBis"), String)
            intLaufzeit = CType(row(0)("Laufzeit"), Integer)
            blnLaufzeitbindung = CType(row(0)("LaufzeitBindung"), Boolean)
            strBemerkungen = CType(row(0)("Bemerkungen"), String)
            strAuftragsnrVon = CType(row(0)("AuftragsNrVon"), String)
            strAuftragsnrBis = CType(row(0)("AuftragsNrBis"), String)
            strVerwendungszweck = CType(row(0)("Verwendungszweck"), String)
            strVerwendungszweckBezeichnung = CType(row(0)("VerwendungszweckBezeichnung"), String)
            blnFahrzeuggruppe = CType(row(0)("Fahrzeuggruppe"), Boolean)
            blnWinterbereifung = CType(row(0)("Winterbereifung"), Boolean)
            blnSecurityFleet = CType(row(0)("SecurFleet"), Boolean)
            blnLeasing = CType(row(0)("Leasing"), Boolean)

            KennzeichenSerie = CType(row(0)("Kennzeichenserie"), String)
            blnNavigationssystem = CType(row(0)("NavigationsSystem"), Boolean)
            blnAnhaengerkupplung = CType(row(0)("Anhaengerkupplung"), Boolean)

            Dim strUnitnummern As String = CType(row(0)("Unitnummern"), String)
            If Not String.IsNullOrEmpty(strUnitnummern) Then
                tblUnitnummern.Clear()
                Dim teile As String() = strUnitnummern.Split(","c)
                Dim unitnrRow As DataRow
                For Each unitnr As String In teile
                    unitnrRow = tblUnitnummern.NewRow
                    unitnrRow.Item("ZUNIT_NR") = unitnr.Trim
                    tblUnitnummern.Rows.Add(unitnrRow)
                Next
                tblUnitnummern.AcceptChanges()
            End If
        End If
    End Sub

    Public Sub addNewRow()
        Dim row As DataRow

        row = ResultTable.NewRow()

        row("RowId") = m_objUser.UserID & "." & Now.Year & Right("00" & Now.Month, 2) & Right("00" & Now.Hour, 2) & Right("00" & Now.Minute, 2) & Right("00" & Now.Second, 2)
        row("ModelId") = strModelId
        row("ModellBezeichnung") = strModellBezeichnung
        row("SippCode") = strSippCode
        row("Hersteller") = strHersteller
        row("HerstellerBezeichnung") = strHerstellerBezeichnung
        row("BatchId") = lngBatchId
        row("DatumEinsteuerung") = strDatumEinsteuerung
        row("AnzahlFahrzeuge") = intAnzahlKfz
        row("UnitNrVon") = strUnitnrVon
        row("UnitNrBis") = strUnitnrBis
        row("Laufzeit") = intLaufzeit
        row("LaufzeitBindung") = blnLaufzeitbindung
        row("Bemerkungen") = strBemerkungen
        row("AuftragsNrVon") = strAuftragsnrVon
        row("AuftragsNrBis") = strAuftragsnrBis
        row("Verwendungszweck") = strVerwendungszweck
        row("VerwendungszweckBezeichnung") = strVerwendungszweckBezeichnung
        row("Fahrzeuggruppe") = blnFahrzeuggruppe
        row("Winterbereifung") = blnWinterbereifung
        row("Leasing") = blnLeasing
        row("SecurFleet") = blnSecurityFleet
        row("Selection") = intSelectedRow
        row("Kennzeichenserie") = KennzeichenSerie
        row("NavigationsSystem") = blnNavigationssystem
        row("Anhaengerkupplung") = blnAnhaengerkupplung

        Dim strUnitnummern As String = ""
        If tblUnitnummern IsNot Nothing AndAlso tblUnitnummern.Rows.Count > 0 Then
            For Each zeile As DataRow In tblUnitnummern.Rows
                strUnitnummern &= zeile.Item("ZUNIT_NR").ToString & ","
            Next
            strUnitnummern.Trim(","c)
        End If
        row("Unitnummern") = strUnitnummern

        ResultTable.Rows.Add(row)

    End Sub

    Public Sub updateExistingRow(ByRef status As String)
        Dim row As DataRow()

        status = String.Empty

        Try
            row = ResultTable.Select("RowId='" & strSelectionId & "'")     'Zeile suchen....

            If (row.Length = 0) Then
                status = "Datensatz konnte nicht ermittelt werden."
            Else
                row(0).BeginEdit()

                row(0)("ModelId") = strModelId
                row(0)("ModellBezeichnung") = strModellBezeichnung
                row(0)("SippCode") = strSippCode
                row(0)("Hersteller") = strHersteller
                row(0)("HerstellerBezeichnung") = strHerstellerBezeichnung
                row(0)("BatchId") = lngBatchId
                row(0)("DatumEinsteuerung") = strDatumEinsteuerung
                row(0)("AnzahlFahrzeuge") = intAnzahlKfz
                row(0)("UnitNrVon") = strUnitnrVon
                row(0)("UnitNrBis") = strUnitnrBis
                row(0)("Laufzeit") = intLaufzeit
                row(0)("LaufzeitBindung") = blnLaufzeitbindung
                row(0)("Bemerkungen") = strBemerkungen
                row(0)("AuftragsNrVon") = strAuftragsnrVon
                row(0)("AuftragsNrBis") = strAuftragsnrBis
                row(0)("Verwendungszweck") = strVerwendungszweck
                row(0)("VerwendungszweckBezeichnung") = strVerwendungszweckBezeichnung
                row(0)("Fahrzeuggruppe") = blnFahrzeuggruppe
                row(0)("Winterbereifung") = blnWinterbereifung
                row(0)("Leasing") = blnLeasing
                row(0)("SecurFleet") = blnSecurityFleet
                row(0)("Selection") = intSelectedRow
                row(0)("Kennzeichenserie") = KennzeichenSerie
                row(0)("NavigationsSystem") = blnNavigationssystem
                row(0)("Anhaengerkupplung") = blnNavigationssystem

                Dim strUnitnummern As String = ""
                If tblUnitnummern IsNot Nothing AndAlso tblUnitnummern.Rows.Count > 0 Then
                    For Each zeile As DataRow In tblUnitnummern.Rows
                        strUnitnummern &= zeile.Item("ZUNIT_NR").ToString & ","
                    Next
                    strUnitnummern.Trim(","c)
                End If
                row(0)("Unitnummern") = strUnitnummern

                row(0).EndEdit()

                ResultTable.AcceptChanges()
            End If
        Catch ex As Exception
            status = "Datensatz konnte nicht aktualisiert werden."
        End Try
    End Sub

    ''' <summary>
    ''' Erzeugt die ErgebnisTabelle
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub createResultTable()
        ResultTable = New DataTable()

        With ResultTable.Columns
            .Add("RowId", System.Type.GetType("System.String"))

            .Add("ModelId", System.Type.GetType("System.String"))
            .Add("ModellBezeichnung", System.Type.GetType("System.String"))
            .Add("SippCode", System.Type.GetType("System.String"))
            .Add("Hersteller", System.Type.GetType("System.String"))
            .Add("HerstellerBezeichnung", System.Type.GetType("System.String"))
            .Add("BatchId", System.Type.GetType("System.Int32"))
            .Add("DatumEinsteuerung", System.Type.GetType("System.String"))
            .Add("AnzahlFahrzeuge", System.Type.GetType("System.Int32"))
            .Add("UnitNrVon", System.Type.GetType("System.String"))
            .Add("UnitNrBis", System.Type.GetType("System.String"))
            .Add("Unitnummern", System.Type.GetType("System.String"))
            .Add("Laufzeit", System.Type.GetType("System.Int32"))
            .Add("LaufzeitBindung", System.Type.GetType("System.Boolean"))
            .Add("Bemerkungen", System.Type.GetType("System.String"))
            .Add("AuftragsNrVon", System.Type.GetType("System.String"))
            .Add("AuftragsNrBis", System.Type.GetType("System.String"))
            .Add("Verwendungszweck", System.Type.GetType("System.String"))
            .Add("VerwendungszweckBezeichnung", System.Type.GetType("System.String"))
            .Add("Fahrzeuggruppe", System.Type.GetType("System.Boolean"))
            .Add("Winterbereifung", System.Type.GetType("System.Boolean"))
            .Add("SecurFleet", System.Type.GetType("System.Boolean"))
            .Add("Selection", System.Type.GetType("System.Int32"))
            .Add("Status", System.Type.GetType("System.String"))
            .Add("Leasing", System.Type.GetType("System.Boolean"))
            .Add("Kennzeichenserie", System.Type.GetType("System.String"))
            .Add("NavigationsSystem", System.Type.GetType("System.Boolean"))
            .Add("Anhaengerkupplung", System.Type.GetType("System.Boolean"))
        End With
    End Sub

#End Region
End Class

' ************************************************
' $History: ec_01.vb $
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 11.04.11   Time: 14:49
' Updated in $/CKAG/Applications/appec/Lib
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 4.02.10    Time: 16:13
' Updated in $/CKAG/Applications/appec/Lib
' BUGFIX KGa ZERNAM max 11 Stellen
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 25.06.09   Time: 16:04
' Updated in $/CKAG/Applications/appec/Lib
' ITA 2918 Z_M_Herstellergroup, Z_M_EC_AVM_BATCH_update,
' Z_M_EC_AVM_HERST_VWZWECK_MODID, Z_M_EC_AVM_BATCH_INSERT,
' Z_M_EC_AVM_BATCH_ANSICHT
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 16.04.08   Time: 11:54
' Updated in $/CKAG/Applications/appec/Lib
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 7.04.08    Time: 13:26
' Updated in $/CKG/Applications/AppEC/AppECWeb/Lib
' ITA 1818
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 29.01.08   Time: 9:50
' Updated in $/CKG/Applications/AppEC/AppECWeb/Lib
' ITA 1655 Done
' 
' *****************  Version 9  *****************
' User: Uha          Date: 2.07.07    Time: 17:23
' Updated in $/CKG/Applications/AppEC/AppECWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 8  *****************
' User: Uha          Date: 7.03.07    Time: 11:02
' Updated in $/CKG/Applications/AppEC/AppECWeb/Lib
' 
' ************************************************
