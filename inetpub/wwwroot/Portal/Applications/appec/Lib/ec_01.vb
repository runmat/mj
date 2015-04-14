Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Public Class ec_01
    REM § Status-Report, Kunde: ALD, BAPI: Z_V_Ueberf_Auftr_Kund_Port,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase ' FFD_Bank_Datenimport

#Region " Declarations"

    Private tblHersteller As DataTable
    Private tblVerwendung As DataTable
    Private tblModell As DataTable
    Private tblUnitnummern As DataTable
    Private tblBatche As DataTable

    Private strModelId As String
    Private strModellBezeichnung As String
    Private strSippCode As String
    Private strHersteller As String
    Private strHerstellerBezeichnung As String
    Private strBatchId As String
    Private strDatumEinsteuerung As String
    Private strAnzahlKfz As String
    Private strUnitnrVon As String
    Private strUnitnrBis As String
    Private strLaufzeit As String
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
    Private blnLeasing As Boolean
    Private strKennzeichenSerie As String

    Private strFilterBatchIdVon As String
    Private strFilterBatchIdBis As String
    Private strFilterUnitnrVon As String
    Private strFilterUnitnrBis As String
    Private strFilterModelIdVon As String
    Private strFilterModelIdBis As String
    Private strFilterEinsteuerungVon As String
    Private strFilterEinsteuerungBis As String
    Private strFilterErfasser As String
    Private strFilterAnlagedatumVon As String
    Private strFilterAnlagedatumBis As String

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

    ReadOnly Property Batche() As DataTable
        Get
            Return tblBatche
        End Get
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

    Property BatchId() As String
        Get
            Return strBatchId
        End Get
        Set(ByVal Value As String)
            strBatchId = Value
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

    Property AnzahlFahrzeuge() As String
        Get
            Return strAnzahlKfz
        End Get
        Set(ByVal Value As String)
            strAnzahlKfz = Value
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

    Property Laufzeit() As String
        Get
            Return strLaufzeit
        End Get
        Set(ByVal Value As String)
            strLaufzeit = Value
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

    Public Property FilterBatchIdVon() As String
        Get
            Return strFilterBatchIdVon
        End Get
        Set(value As String)
            strFilterBatchIdVon = value
        End Set
    End Property

    Public Property FilterBatchIdBis() As String
        Get
            Return strFilterBatchIdBis
        End Get
        Set(value As String)
            strFilterBatchIdBis = value
        End Set
    End Property

    Public Property FilterUnitnrVon() As String
        Get
            Return strFilterUnitnrVon
        End Get
        Set(value As String)
            strFilterUnitnrVon = value
        End Set
    End Property

    Public Property FilterUnitnrBis() As String
        Get
            Return strFilterUnitnrBis
        End Get
        Set(value As String)
            strFilterUnitnrBis = value
        End Set
    End Property

    Public Property FilterModelIdVon() As String
        Get
            Return strFilterModelIdVon
        End Get
        Set(value As String)
            strFilterModelIdVon = value
        End Set
    End Property

    Public Property FilterModelIdBis() As String
        Get
            Return strFilterModelIdBis
        End Get
        Set(value As String)
            strFilterModelIdBis = value
        End Set
    End Property

    Public Property FilterEinsteuerungVon() As String
        Get
            Return strFilterEinsteuerungVon
        End Get
        Set(value As String)
            strFilterEinsteuerungVon = value
        End Set
    End Property

    Public Property FilterEinsteuerungBis() As String
        Get
            Return strFilterEinsteuerungBis
        End Get
        Set(value As String)
            strFilterEinsteuerungBis = value
        End Set
    End Property

    Public Property FilterErfasser() As String
        Get
            Return strFilterErfasser
        End Get
        Set(value As String)
            strFilterErfasser = value
        End Set
    End Property

    Public Property FilterAnlagedatumVon() As String
        Get
            Return strFilterAnlagedatumVon
        End Get
        Set(value As String)
            strFilterAnlagedatumVon = value
        End Set
    End Property

    Public Property FilterAnlagedatumBis() As String
        Get
            Return strFilterAnlagedatumBis
        End Get
        Set(value As String)
            strFilterAnlagedatumBis = value
        End Set
    End Property

#End Region

#Region " Methods"

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Sub loadData(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "ec_01.loadData"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0

        Try
            S.AP.Init("Z_M_EC_AVM_BATCH_SELECT")

            If Not String.IsNullOrEmpty(strFilterBatchIdVon) OrElse Not String.IsNullOrEmpty(strFilterBatchIdBis) _
                OrElse Not String.IsNullOrEmpty(strFilterUnitnrVon) OrElse Not String.IsNullOrEmpty(strFilterUnitnrBis) _
                OrElse Not String.IsNullOrEmpty(strFilterModelIdVon) OrElse Not String.IsNullOrEmpty(strFilterModelIdBis) _
                OrElse Not String.IsNullOrEmpty(strFilterEinsteuerungVon) OrElse Not String.IsNullOrEmpty(strFilterEinsteuerungBis) _
                OrElse Not String.IsNullOrEmpty(strFilterAnlagedatumVon) OrElse Not String.IsNullOrEmpty(strFilterAnlagedatumBis) _
                OrElse Not String.IsNullOrEmpty(strFilterErfasser) Then

                Dim SAPTable As DataTable = S.AP.GetImportTable("GT_IN")

                Dim SAPTableRow As DataRow = SAPTable.NewRow()

                With SAPTableRow
                    If Not String.IsNullOrEmpty(strFilterBatchIdVon) Then .Item("ZBATCH_ID_VON") = strFilterBatchIdVon
                    If Not String.IsNullOrEmpty(strFilterBatchIdBis) Then .Item("ZBATCH_ID_BIS") = strFilterBatchIdBis
                    If Not String.IsNullOrEmpty(strFilterUnitnrVon) Then .Item("ZUNIT_NR_VON") = strFilterUnitnrVon
                    If Not String.IsNullOrEmpty(strFilterUnitnrBis) Then .Item("ZUNIT_NR_BIS") = strFilterUnitnrBis
                    If Not String.IsNullOrEmpty(strFilterModelIdVon) Then .Item("ZMODEL_ID_VON") = strFilterModelIdVon
                    If Not String.IsNullOrEmpty(strFilterModelIdBis) Then .Item("ZMODEL_ID_BIS") = strFilterModelIdBis
                    If Not String.IsNullOrEmpty(strFilterEinsteuerungVon) Then .Item("ZPURCH_MTH_VON") = strFilterEinsteuerungVon
                    If Not String.IsNullOrEmpty(strFilterEinsteuerungBis) Then .Item("ZPURCH_MTH_BIS") = strFilterEinsteuerungBis
                    If Not String.IsNullOrEmpty(strFilterAnlagedatumVon) Then .Item("ERDAT_VON") = strFilterAnlagedatumVon
                    If Not String.IsNullOrEmpty(strFilterAnlagedatumBis) Then .Item("ERDAT_BIS") = strFilterAnlagedatumBis
                    If Not String.IsNullOrEmpty(strFilterErfasser) Then .Item("ZERNAM") = strFilterErfasser
                End With

                SAPTable.Rows.Add(SAPTableRow)

            End If

            tblBatche = S.AP.GetExportTableWithExecute("GT_OUT")

            For Each row As DataRow In tblBatche.Rows
                row("ZANZAHL") = row("ZANZAHL").ToString().TrimStart("0"c)
                row("ZAUFNR_VON") = row("ZAUFNR_VON").ToString().TrimStart("0"c)
                row("ZAUFNR_BIS") = row("ZAUFNR_BIS").ToString().TrimStart("0"c)
            Next

        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_intStatus = -1234
                    m_strMessage = "Fehler: Keine Daten gefunden!"
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
            End Select
        End Try
    End Sub

    Public Sub saveData(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "ec_01.saveData"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0

        Try
            S.AP.Init("Z_M_EC_AVM_BATCH_INSERT", "WEB_USER", Left(m_objUser.UserName, 12))

            Dim SAPTable As DataTable = S.AP.GetImportTable("ZBATCH_IN")

            Dim SAPTableRow As DataRow = SAPTable.NewRow()

            With SAPTableRow
                .Item("Zbatch_Id") = strBatchId
                .Item("Zmodel_Id") = strModelId
                .Item("Zsipp_Code") = strSippCode
                .Item("Zmake") = strHerstellerBezeichnung
                .Item("Zmod_Descr") = strModellBezeichnung
                .Item("Zpurch_Mth") = strDatumEinsteuerung
                .Item("Zanzahl") = strAnzahlKfz
                .Item("Zunit_Nr_Von") = strUnitnrVon
                .Item("Zunit_Nr_Bis") = strUnitnrBis

                If KennzeichenSerie.Contains("(") Then
                    .Item("Zsonderserie") = KennzeichenSerie.Substring(KennzeichenSerie.IndexOf("(") + 1, 1)
                Else
                    .Item("Zsonderserie") = ""
                End If

                .Item("Zfzg_Group") = IIf(blnFahrzeuggruppe, "PKW", "LKW")
                .Item("Zlaufzeit") = strLaufzeit
                .Item("Zlzbindung") = IIf(blnLaufzeitbindung, "X", "")
                .Item("Zaufnr_Von") = strAuftragsnrVon
                .Item("Zaufnr_Bis") = strAuftragsnrBis
                .Item("Zms_Reifen") = IIf(blnWinterbereifung, "X", "")
                .Item("Zsecu_Fleet") = IIf(blnSecurityFleet, "X", "")
                .Item("Zleasing") = IIf(blnLeasing, "X", "")
                .Item("Znavi") = IIf(blnNavigationssystem, "X", "")
                .Item("ZAHK") = IIf(blnAnhaengerkupplung, "X", "")
                .Item("Zverwendung") = strVerwendungszweck
                .Item("Zbemerkung") = strBemerkungen
            End With

            SAPTable.Rows.Add(SAPTableRow)

            If tblUnitnummern IsNot Nothing Then
                Dim unitNrTable As DataTable = S.AP.GetImportTable("GT_IN")
                Dim unitNrTableRow As DataRow
                For Each zeile As DataRow In tblUnitnummern.Rows
                    unitNrTableRow = unitNrTable.NewRow()
                    unitNrTableRow.Item("ZUNIT_NR") = zeile("ZUNIT_NR").ToString()
                    unitNrTable.Rows.Add(unitNrTableRow)
                Next
            End If

            S.AP.Execute()

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
        End Try

    End Sub

    Public Sub getStammdaten(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "ec_01.getStammdaten"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0

        Try
            S.AP.InitExecute("Z_M_EC_AVM_HERST_VWZWECK_MODID")

            tblHersteller = S.AP.GetExportTable("GT_HERST")
            tblVerwendung = S.AP.GetExportTable("GT_VERW")
            tblModell = S.AP.GetExportTable("GT_MODELID")

            'Sipp-Code zusammensetzen
            tblModell.Columns.Add("Sipp", GetType(System.String))

            For Each row As DataRow In tblModell.Rows
                row("Sipp") = CStr(row("SIPP1")) & CStr(row("SIPP2")) & CStr(row("SIPP3")) & CStr(row("SIPP4"))
            Next
            tblModell.AcceptChanges()

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
        End Try
    End Sub

#End Region

End Class

