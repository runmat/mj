Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
'Imports CKG.Base.Kernel.Common.Common
'Imports CKG.Portal.PageElements
'Imports System.Data.SqlClient
'Imports CKG.Base.Common
Imports System.Collections.Generic


Public Class change_01
    REM § Lese-/Schreibfunktion, Kunde: SIXT, 
    REM § New - BAPI: Z_M_Meldungen_Pdi,
    REM § Show - BAPI: Z_M_Meldungen_Fahrzeuge,
    REM § ShowDatenABE - BAPI: Z_M_Abezufzg,
    REM § SelectCars - BAPI: ,
    REM § Change - BAPI: Z_Massenzulassung, Z_M_Zulassungssperre, Z_M_Pdiwechsel.

    Inherits Base.Business.BankBase   'FDD_Bank_Base

#Region " Declarations"
    Private m_intErrorCount As Int32
    Private m_objABE_Daten As Base.Business.ABE2FHZG
    Private m_tblAllPDIs As DataTable
    Private m_tblAllMODs As DataTable
    Private m_tblFahrzeuge As DataTable
    Private m_strPDINummer As String
    Private m_strPDINummerSuche As String
    Private m_strModellCode As String
    Private m_strFahrgestellnummer As String
    Private m_strTask As String
    Private m_intFahrzeugeGesamtZulassungsf As Int32
    Private m_intFahrzeugeGesamtGesperrt As Int32

    Private m_tblKennzeichenserie As DataTable
    Private m_tblVerwendung As DataTable
    Private m_tblFahrzeugeText As DataTable
    Private gridMain As DataGrid
    Private m_strPDISuche As String
    Private m_phase As Char

    Private m_selectedPDI As String
    Private m_selectedMOD As String

    Private m_blnSelectedPDI As Boolean
    Private m_blnSelectedMOD As Boolean
    Private m_Sperre As Char

    Public lstPDI As New List(Of String)
    Public lstHersteller As New List(Of String)
    Public lstModellID As New List(Of String)
    Public lstSIPPCode As New List(Of String)
    Public lstReifenart As New List(Of String)
    Public lstFarbe As New List(Of String)
    Public lstNavi As New List(Of String)
    Public lstAhk As New List(Of String)

    Public FilterPdi As String = ""
    Public FilterHersteller As String = ""
    Public FilterModelId As String = ""
    Public FilterSipp As String = ""
    Public FilterReifenart As String = ""
    Public FilterFarbe As String = ""
    Public FilterNavi As String = ""
    Public FilterAhk As String = ""
    Public FilterAuftragsnrVon As String = ""
    Public FilterAuftragsnrBis As String = ""
    Public FilterFahrgnr As String = ""
    Public FilterSperre As Char = " "c

    Private colorTextArray() As String = {"weiss", "gelb", "orange", "rot", "violett", "blau", "grün", "grau", "braun", "schwarz"}
    Private colorValueArray() As String = {"FFFFFF", "FFFF00", "FF8040", "FF0000", "FF00FF", "0000FF", "00FF00", "808080", "804000", "000000"}

#End Region

#Region " Properties"

    Public ReadOnly Property PTexte() As DataTable
        Get
            Return m_tblFahrzeugeText
        End Get
    End Property

    Public Property PSperre() As Char
        Get
            Return m_Sperre
        End Get
        Set(ByVal value As Char)
            m_Sperre = value
        End Set
    End Property

    Public Property PPhase() As Char
        Get
            Return m_phase
        End Get
        Set(ByVal value As Char)
            m_phase = value
        End Set
    End Property

    Public Property PSelectionPdi() As Boolean
        Get
            Return m_blnSelectedPDI
        End Get
        Set(ByVal value As Boolean)
            m_blnSelectedPDI = value
        End Set
    End Property

    Public Property PSelectionMod() As Boolean
        Get
            Return m_blnSelectedMOD
        End Get
        Set(ByVal value As Boolean)
            m_blnSelectedMOD = value
        End Set
    End Property

    Public Property PSelectedMod() As String
        Get
            Return m_selectedMOD
        End Get
        Set(ByVal value As String)
            m_selectedMOD = value
        End Set
    End Property

    Public Property PSelectedPdi() As String
        Get
            Return m_selectedPDI
        End Get
        Set(ByVal value As String)
            m_selectedPDI = value
        End Set
    End Property

    Public Property PpdiSuche() As String
        Get
            Return m_strPDISuche
        End Get
        Set(ByVal value As String)
            m_strPDISuche = value
        End Set
    End Property

    Public Property PGrid() As DataGrid
        Get
            Return gridMain
        End Get
        Set(ByVal value As DataGrid)
            gridMain = value
        End Set
    End Property

    Public ReadOnly Property PVerwendung() As DataTable
        Get
            Return m_tblVerwendung
        End Get
    End Property

    Public ReadOnly Property PKennzeichenSerie() As DataTable
        Get
            Return m_tblKennzeichenserie
        End Get
    End Property

    Public ReadOnly Property ErrorCount() As Int32
        Get
            Return m_intErrorCount
        End Get
    End Property

    Public ReadOnly Property FahrzeugeGesamtZulassungsf() As Int32
        Get
            Return m_intFahrzeugeGesamtZulassungsf
        End Get
    End Property

    Public ReadOnly Property FahrzeugeGesamtGesperrt() As Int32
        Get
            Return m_intFahrzeugeGesamtGesperrt
        End Get
    End Property

    Public Property AbeDaten() As Base.Business.ABE2FHZG
        Get
            Return m_objABE_Daten
        End Get
        Set(ByVal value As Base.Business.ABE2FHZG)
            m_objABE_Daten = value
        End Set
    End Property

    Public ReadOnly Property AllPdIs() As DataTable
        Get
            Return m_tblAllPDIs
        End Get
    End Property

    Public ReadOnly Property AllMoDs() As DataTable
        Get
            Return m_tblAllMODs
        End Get
    End Property

    Public Property Task() As String
        Get
            Return m_strTask
        End Get
        Set(ByVal value As String)
            m_strTask = value
        End Set
    End Property

    Public Property PdiNummer() As String
        Get
            Return m_strPDINummer
        End Get
        Set(ByVal value As String)
            m_strPDINummer = value
        End Set
    End Property

    Public Property PdiNummerSuche() As String
        Get
            Return m_strPDINummerSuche
        End Get
        Set(ByVal value As String)
            m_strPDINummerSuche = value
        End Set
    End Property

    Public Property Fahrgestellnummer() As String
        Get
            Return m_strFahrgestellnummer
        End Get
        Set(ByVal value As String)
            m_strFahrgestellnummer = value
        End Set
    End Property

    Public Property ModellCode() As String
        Get
            Return m_strModellCode
        End Get
        Set(ByVal value As String)
            m_strModellCode = value
        End Set
    End Property

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)

        GetVerwendung(strAppID, strSessionID)

        If Not m_blnErrorOccured Then
            GetKennzeichenserie(strAppID, strSessionID)
        End If
    End Sub

    Public Sub GetKennzeichenserie(ByVal strAppId As String, ByVal strSessionId As String)
        ClearError()

        Dim intId As Int32
        Dim row As DataRow
        Dim intRow As Integer

        m_strClassAndMethod = "Change_01.getKennzeichenserie"
        m_strAppID = strAppId
        m_strSessionID = strSessionId

        Try
            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Ec_Avm_Kennz_Serie", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

            'myProxy.callBapi()

            S.AP.InitExecute("Z_M_EC_AVM_KENNZ_SERIE", "I_KUNNR", m_strKUNNR)

            m_tblKennzeichenserie = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")

            m_tblKennzeichenserie.DefaultView.Sort = "SONDERSERIE ASC" ' =HelpProcedures.DataTableAlphabeticSort(m_tblKennzeichenserie, "SONDERSERIE", 0)
            m_tblKennzeichenserie = m_tblKennzeichenserie.DefaultView.ToTable()

            'SAPReturnTable.SortBy("SONDERSERIE", "ASC") 'Standardserie vorbelegt (erster Eintrag)

            ' m_tblKennzeichenserie = SAPReturnTable.ToADODataTable
            With m_tblKennzeichenserie.Columns
                .Add("ID", GetType(System.Int16))
                .Add("Serie", GetType(System.String))
            End With

            intRow = 0
            For Each row In m_tblKennzeichenserie.Rows
                row("ID") = intRow
                row("Serie") = CStr(row("ORTKENNZ")) & "-" & CStr(row("MINLETTER"))
                If CStr(row("SONDERSERIE")) <> String.Empty Then
                    row("Serie") = CStr(row("Serie")) & " (" & CStr(row("SONDERSERIE")) & ")"
                End If
                intRow += 1
            Next
            m_tblKennzeichenserie.AcceptChanges()
            'Report-Logeintrag (ok)
            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblVerwendung)
        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_intStatus = -1234
                    m_strMessage = "Fehler: Keine Daten gefunden!"
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
            End Select
            'tblHersteller = Nothing
            m_tblKennzeichenserie = Nothing
            'Report-Logeintrag (Fehler)
            If intId > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intId, False, m_strMessage)
            End If
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult)
        End Try
    End Sub

    Public Sub GetVerwendung(ByVal strAppId As String, ByVal strSessionId As String)
        ClearError()

        m_strClassAndMethod = "Change_01.getVerwendung"
        m_strAppID = strAppId
        m_strSessionID = strSessionId


        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Ec_Avm_Herst_Vwzweck_Modid", m_objApp, m_objUser, page)
            S.AP.InitExecute("Z_M_EC_AVM_HERST_VWZWECK_MODID")
            m_tblVerwendung = S.AP.GetExportTable("GT_VERW")

            'myProxy.callBapi()
            'Tabellen formatieren
            'tblHersteller = SAPTableReturnHersteller.ToADODataTable
            'm_tblVerwendung = myProxy.getExportTable("GT_VERW")
            'tblModell = SAPTableReturnModell.ToADODataTable

            'Report-Logeintrag (ok)
            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblVerwendung)
        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_intStatus = -1234
                    m_strMessage = "Fehler: Keine Daten gefunden!"
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
            End Select
            'tblHersteller = Nothing
            m_tblVerwendung = Nothing

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult)

        End Try
    End Sub

    Public Function CreateZulassungCountTable(ByVal datum As Date) As DataTable
        Dim dtAnzahlZulassungen As New DataTable()

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Ec_Avm_Anz_Beauftr_Zul", m_objApp, m_objUser, Page)

            'myProxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
            'myProxy.setImportParameter("I_ZULDAT", datum.ToShortDateString)

            'myProxy.callBapi()

            S.AP.InitExecute("Z_M_EC_AVM_ANZ_BEAUFTR_ZUL", "I_KUNNR,I_ZULDAT", m_strKUNNR, datum) '.ToShortDateString

            dtAnzahlZulassungen = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")

        Catch ex As Exception
            'do nothing
        End Try

        Return dtAnzahlZulassungen
    End Function

    Public Function SetResultRowClear() As DataTable
        Dim row As DataRow
        Dim intIndex As Integer
        Dim blnSelectedEinzel As Boolean
        Dim tblExcel As New DataTable()

        'Nicht ausgewählte Zeilen löschen
        Try
            For intIndex = m_tblResult.Rows.Count - 1 To 0 Step -1
                row = m_tblResult.Rows(intIndex)

                blnSelectedEinzel = CBool(row("SelectedEinzel"))
                If blnSelectedEinzel = False Then
                    m_tblResult.Rows.Remove(row)
                End If
            Next
            m_tblResult.AcceptChanges()

            'Tabelle für Excel-Export neu aufbauen...
            With tblExcel.Columns
                .Add("PDI", GetType(System.String))
                .Add("Modell", GetType(System.String))
                .Add("Eingangsdatum", GetType(System.String))
                .Add("Fahrgestellnummer", GetType(System.String))
                .Add("Farbcode", GetType(System.String))
                .Add("Zulassungsdatum", GetType(System.String))
                .Add("Kennzeichenserie", GetType(System.String))
                .Add("Status", GetType(System.String))
            End With
            'Werte füllen
            Dim rowNew As DataRow
            For Each row In m_tblResult.Rows
                rowNew = tblExcel.NewRow
                rowNew("PDI") = CStr(row("KUNPDI"))
                rowNew("Modell") = CStr(row("ZZBEZEI"))
                rowNew("Eingangsdatum") = CStr(row("ZZDAT_EIN"))
                rowNew("Fahrgestellnummer") = CStr(row("ZZFAHRG"))
                rowNew("Farbcode") = CStr(row("ZZFARBE"))
                rowNew("Zulassungsdatum") = CStr(row("SelectedDate"))
                rowNew("Kennzeichenserie") = CStr(row("SelectedKennzeichenserieText"))
                rowNew("Status") = CStr(row("Status"))
                tblExcel.Rows.Add(rowNew)
            Next
            tblExcel.AcceptChanges()

        Catch ex As Exception
            m_tblResult = Nothing
            tblExcel = Nothing
        End Try

        Return tblExcel
    End Function

    Public Sub SetZulassung(ByVal strAppId As String, ByVal strSessionId As String)
        'Zulassen
        Dim rowsZulassung As DataRow()
        Dim strZuldatum As String
        Dim strKbanr As String
        Dim strKunnr As String
        Dim strHalter As String
        Dim strVersicherer As String
        Dim strPdi As String
        Dim strFahrgestell As String
        Dim strSonder As String


        m_strClassAndMethod = "Change_01.setZulassung"
        m_strAppID = strAppId
        m_strSessionID = strSessionId

        FahrzeugeSperren(Result, m_strAppID, SessionID)
        Try
            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_Massenzulassung", m_objApp, m_objUser, page)


            Dim zulSpeicherTable As DataTable = S.AP.GetImportTableWithInit("Z_Massenzulassung.INTERNTAB") 'myProxy.getImportTable("INTERNTAB")
            Dim zulSpeicher As DataRow
            Dim zulOutPutTable As DataTable
            Dim zulOutPut As DataRow

            Dim intCount As Integer

            'Tabellenzeilen identifizieren
            rowsZulassung = Result.Select("SelectedEinzel=True AND Art='CAR'")

            'Alle FAhrzeuge durchgehen.....
            For intCount = 0 To rowsZulassung.Length - 1
                'Inputarameter füllen ......
                strZuldatum = Format(CDate(rowsZulassung(intCount)("SelectedDate")), "dd.MM.yyyy")
                strKunnr = m_strKUNNR
                strHalter = Right("0000000000" & "100010607", 10)
                'Versichererwechsel ab 01.04.2008
                If CDate(strZuldatum) < CDate("01.04.2008") Then
                    strVersicherer = Right("0000000000106", 10)
                Else
                    strVersicherer = Right("0000000001917", 10)
                End If

                strKbanr = "0200000"
                strPdi = CStr(rowsZulassung(intCount)("DADPDI"))
                strFahrgestell = CStr(rowsZulassung(intCount)("ZZFAHRG"))
                strSonder = CStr(rowsZulassung(intCount)("SelectedSonderserie"))
                Try
                    zulSpeicher = zulSpeicherTable.NewRow
                    With zulSpeicher
                        .Item("I_Kunnr_Ag") = m_objUser.KUNNR.PadLeft(10, "0"c)
                        .Item("I_Zzfahrg") = strFahrgestell
                        .Item("I_Edatu") = strZuldatum
                        .Item("I_Kunnr_Zv") = strVersicherer
                        .Item("I_Zzkennz") = String.Empty
                        .Item("I_Kunnr_Zh") = strHalter
                        .Item("I_Kunnr_Za") = String.Empty
                        .Item("I_Zzsonder") = strSonder
                        .Item("I_Kbanr") = strKbanr
                        .Item("I_Zzcarport") = strPdi
                    End With
                    zulSpeicherTable.Rows.Add(zulSpeicher)
                    rowsZulassung(intCount)("Status") = "Vorgang OK: Daten wurden gesendet."
                Catch ex As Exception
                    'Fehler...
                    Select Case ex.Message
                        Case "SCHON_ZUGELASSEN"
                            m_intStatus = -1234
                            m_strMessage = "Fehler! Fahrzeug bereits zugelassen."
                        Case "GESPERRT"
                            m_intStatus = -1235
                            m_strMessage = "Fehler! Fahrzeug gesperrt"
                        Case "VERSCHOBEN"
                            m_intStatus = -1236
                            m_strMessage = "Fehler! Fahrzeug verschoben."
                        Case Else
                            m_intStatus = -9999
                            m_strMessage = "Fehler! Unbekannter Fehler."
                    End Select
                    rowsZulassung(intCount)("Status") = m_strMessage
                    WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult)
                End Try

            Next

            If m_strTask = "Zulassen" AndAlso zulSpeicherTable.Rows.Count > 0 Then
                Try
                    Dim anzahl As String = ""
                    Dim sReturn As String = ""

                    'myProxy.callBapi()
                    S.AP.Execute()

                    sReturn = S.AP.GetExportParameter("RETURN") 'myProxy.getExportParameter("RETURN")
                    anzahl = S.AP.GetExportParameter("ANZAHL") 'myProxy.getExportParameter("ANZAHL")
                    zulOutPutTable = S.AP.GetExportTable("OUTPUT") 'myProxy.getExportTable("OUTPUT")


                    If sReturn = "OK" Then
                        m_intStatus = 0
                        Result.AcceptChanges()
                        For Each zulOutPut In zulOutPutTable.Rows
                            Result.Select("Fahrgestellnummer='" & zulOutPut.Item("Id").ToString & "'")(0)("Belegnummer") = zulOutPut.Item("Message")
                            If zulOutPut.Item("Message").ToString.Length > 0 Then
                                m_intErrorCount += 1
                            End If
                        Next
                        Result.AcceptChanges()
                        m_strMessage = String.Format("Es wurden {0} Fahrzeuge zugelassen.", anzahl.Trim)
                    Else
                        m_intStatus = -9999

                        For intCount = 0 To rowsZulassung.Length - 1
                            m_intErrorCount += 1
                            rowsZulassung(intCount)("Status") = "Fehler beim Speichern."
                        Next
                        m_strMessage = "Es ist ein Fehler aufgetreten.<br>Daten wurden nicht übermittelt."
                    End If

                Catch ex As Exception
                    m_intStatus = -9999

                    m_strMessage = String.Concat("Es ist ein Fehler aufgetreten.<br>", HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                End Try
            End If

        Catch ex As Exception
            m_intStatus = -9999
            m_strMessage = "Unbekannter Fehler."
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult)
        End Try
    End Sub

    Public Sub SetSperre(ByVal strAppId As String, ByVal strSessionId As String)
        'Sperren / Entsperren
        Dim rowsSperre As DataRow()
        Dim strKunnr As String
        Dim strBemerkungDatum As String
        Dim strPDI As String
        Dim strFahrgestell As String
        Dim strEquipment As String = ""
        Dim strQmNr As String = ""
        Dim intResult As Integer

        m_strClassAndMethod = "Change_01.setSperre"
        m_strAppID = strAppId
        m_strSessionID = strSessionId
        Try
            Dim intCount As Integer

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Ec_Avm_Zulassungssperre", m_objApp, m_objUser, page)

            'Dim tblBemerkungenSAP As DataTable = myProxy.getImportTable("GT_TEXTE")
            'Dim tblRowBemerkungenSAP As DataRow

            'Tabellenzeile identifizieren
            rowsSperre = Result.Select("SelectedEinzel=True AND Art='CAR'")

            'Alle FAhrzeuge durchgehen.....
            For intCount = 0 To rowsSperre.Length - 1
                'myProxy.getImportTable("GT_TEXTE")
                Try

                    Dim tblRowBemerkungenSap As DataRow

                    'Inputarameter füllen ......
                    strKunnr = m_strKUNNR
                    strPDI = CStr(rowsSperre(intCount)("DADPDI"))
                    strFahrgestell = CStr(rowsSperre(intCount)("ZZFAHRG"))
                    strBemerkungDatum = Now.ToShortDateString

                    S.AP.Init("Z_M_Ec_Avm_Zulassungssperre", "I_KUNNR,I_ZZFAHRG,I_ZZAKTSPERRE,I_ZZCARPORT,I_ZZDATBEM", strKunnr, strFahrgestell, m_Sperre, strPDI, strBemerkungDatum)

                    Dim tblBemerkungenSap As DataTable = S.AP.GetImportTable("GT_TEXTE")

                    'Bemerkungstext(e) als Tabelle
                    tblRowBemerkungenSap = tblBemerkungenSap.NewRow
                    tblRowBemerkungenSap("Tdline") = CStr(rowsSperre(intCount)("Bemerkung"))
                    tblBemerkungenSap.Rows.Add(tblRowBemerkungenSap)


                    'myProxy.setImportParameter("I_KUNNR", strKunnr)
                    'myProxy.setImportParameter("I_ZZFAHRG", strFahrgestell)
                    'myProxy.setImportParameter("I_ZZAKTSPERRE", m_Sperre)
                    'myProxy.setImportParameter("I_ZZCARPORT", strPDI)
                    'myProxy.setImportParameter("I_ZZDATBEM", strBemerkungDatum)

                    'myProxy.callBapi()


                    'intResult = CInt(myProxy.getExportParameter("E_SUBRC"))
                    'strEquipment = myProxy.getExportParameter("E_EQUNR")
                    'strQmNr = myProxy.getExportParameter("E_QMNUM")

                    S.AP.Execute()

                    intResult = CInt(S.AP.GetExportParameter("E_SUBRC"))
                    strEquipment = S.AP.GetExportParameter("E_EQUNR")
                    strQmNr = S.AP.GetExportParameter("E_QMNUM")

                    If intResult = 0 Then
                        rowsSperre(intCount)("Status") = "Vorgang OK."
                    End If
                Catch ex As Exception
                    Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                        Case "SCHON_ZUGELASSEN"
                            m_intStatus = -1234
                            m_strMessage = "Fehler! Fahrzeug bereits zugelassen."
                        Case "GESPERRT"
                            m_intStatus = -1235
                            m_strMessage = "Fehler! Fahrzeug gesperrt"
                        Case "VERSCHOBEN"
                            m_intStatus = -1236
                            m_strMessage = "Fehler! Fahrzeug verschoben."
                        Case Else
                            m_intStatus = -9999
                            m_strMessage = "Fehler! Unbekannter Fehler."
                    End Select
                    rowsSperre(intCount)("Status") = m_strMessage
                    WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult)
                End Try

            Next
        Catch ex As Exception
            m_intStatus = -9999
            m_strMessage = "Unbekannter Fehler."
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult)
        End Try
    End Sub

    Public Sub SetVerschieb(ByVal strAppId As String, ByVal strSessionId As String)
        'Verschieben
        Dim intId As Int32
        Dim rowsVerschieb As DataRow()
        Dim strKunnr As String
        Dim strBemerkungDatum As String
        Dim strPDI As String
        Dim strPDINeu As String
        Dim strQmNr As String
        Dim intResult As Integer

        m_strClassAndMethod = "Change_01.setVerschieb"
        m_strAppID = strAppId
        m_strSessionID = strSessionId

        Try
            Dim intCount As Integer

            'Tabellenzeile identifizieren
            rowsVerschieb = Result.Select("SelectedEinzel=True AND Art='CAR'")

            'Alle FAhrzeuge durchgehen.....
            For intCount = 0 To rowsVerschieb.Length - 1
                Try
                    'Inputarameter füllen ......
                    strKunnr = m_strKUNNR
                    strPDI = CStr(rowsVerschieb(intCount)("DADPDI"))
                    strPDINeu = CStr(rowsVerschieb(intCount)("SelectedZielPDI"))
                    strBemerkungDatum = Now.ToShortDateString
                    strQmNr = CStr(rowsVerschieb(intCount)("QMNUM"))

                    'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Ec_Avm_Pdiwechsel", m_objApp, m_objUser, page)
                    S.AP.InitExecute("Z_M_Ec_Avm_Pdiwechsel", "ZZKUNNR,ZZQMNUM,ZZCARPORT,I_ZZCARPORT,I_ZZDATBEM", strKunnr, strQmNr, strPDINeu, strPDI, strBemerkungDatum)

                    'myProxy.setImportParameter("ZZKUNNR", strKunnr)
                    'myProxy.setImportParameter("ZZQMNUM", strQmNr)
                    'myProxy.setImportParameter("ZZCARPORT", strPDINeu)
                    'myProxy.setImportParameter("I_ZZCARPORT", strPDI)
                    'myProxy.setImportParameter("I_ZZDATBEM", strBemerkungDatum)

                    'myProxy.callBapi()

                    If intResult = 0 Then
                        'Ok...
                        rowsVerschieb(intCount)("Status") = "Vorgang OK."
                    End If

                Catch ex As Exception
                    'Fehler...
                    Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                        Case "ERR_INV_KUNNR"
                            m_intStatus = -1211
                            m_strMessage = "Fehler! Ungültige Kundennummer."
                        Case "ERR_INV_QMNUM"
                            m_intStatus = -1212
                            m_strMessage = "Fehler! Ungültige Meldungsnummer."
                        Case "ERR_NOT_SAVED"
                            m_intStatus = -1213
                            m_strMessage = "Fehler! Datensatz konnte nicht gespeichert werden."
                        Case "ERR_INV_CARPORT"
                            m_intStatus = -1214
                            m_strMessage = "Fehler! Ungültiger PDI."
                        Case "SCHON_ZUGELASSEN"
                            m_intStatus = -1234
                            m_strMessage = "Fehler! Fahrzeug bereits zugelassen."
                        Case "GESPERRT"
                            m_intStatus = -1235
                            m_strMessage = "Fehler! Fahrzeug gesperrt"
                        Case "VERSCHOBEN"
                            m_intStatus = -1236
                            m_strMessage = "Fehler! Fahrzeug verschoben."
                        Case Else
                            m_intStatus = -9999
                            m_strMessage = "Fehler! Unbekannter Fehler."
                    End Select
                    rowsVerschieb(intCount)("Status") = m_strMessage
                    WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult)
                End Try

            Next
        Catch ex As Exception
            m_intStatus = -9999
            m_strMessage = "Unbekannter Fehler."

            If intId > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intId, False, m_strMessage)
            End If
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult)
        End Try
    End Sub

    Public Sub GetCars(ByVal strAppId As String, ByVal strSessionId As String)
        'Holt alle Fahrzeuge, die zulassungsbereit sind.
        Dim row As DataRow
        Dim rowBemerkung As DataRow()

        m_strClassAndMethod = "Change_01.getCars()"

        m_intErrorCount = 0

        ClearError()
        ClearFilters()

        Try

            m_intStatus = 0
            m_strMessage = ""
            m_intFahrzeugeGesamtZulassungsf = 0
            m_intFahrzeugeGesamtGesperrt = 0

            S.AP.InitExecute("Z_M_EC_AVM_MELDUNGEN_PDI1", "I_VKORG,I_KUNNR,I_ZZCARPORT,I_PHASE", "1510", m_strKUNNR, m_strPDISuche, m_phase)

            m_tblFahrzeuge = S.AP.GetExportTable("GT_WEB")
            m_tblFahrzeugeText = S.AP.GetExportTable("GT_TXT")

            With m_tblFahrzeuge.Columns
                .Add("Art", GetType(System.String))   'Gruppierungstyp: PDI, Modell, Fahrzeug
                .Add("Anzahl", GetType(System.Int32))
                .Add("RowID", GetType(System.String))   'ID der Zeile
                .Add("DetailButton", GetType(System.String))
                .Add("SelectedDate", GetType(System.String))            'Eingetragenes Zulassungsdatum
                .Add("SelectedKennzeichenserie", GetType(System.String))     'Eingetragener Versicherung
                .Add("SelectedKennzeichenserieText", GetType(System.String))     'Eingetragener Versicherung
                .Add("SelectedSonderserie", GetType(System.String))     'Eingetragener Versicherung
                .Add("SelectedEinzel", GetType(System.Boolean))           'Eingetragene Fahrzeuganzahl
                .Add("SelectedAnzahl", GetType(System.Int16))           'Eingetragene Fahrzeuganzahl
                .Add("PDIAnzahl", GetType(System.String))
                .Add("MODAnzahl", GetType(System.String))
                .Add("SelectedAlle", GetType(System.Int16))             'Werte übernehmen?
                .Add("Status", GetType(System.String))             'Werte übernehmen?
                .Add("Bemerkung", GetType(System.String))
                .Add("SelectedZielPDI", GetType(System.String))
                .Add("SelectedZielPDIText", GetType(System.String))
                .Add("SIPPCODE", GetType(System.String))        'SIPP-Code
                .Add("ZUSATZDATEN", GetType(System.String))
                .Add("Eingangsdatum", GetType(System.DateTime))
                .Add("PDI", GetType(System.String))
                .Add("Farbname", GetType(System.String))            'Farbbezeichnung zum Farbcode
                .Add("Farbe", GetType(System.String))           'HTML-Code zur Farbdarstellung
            End With

            m_tblFahrzeuge.AcceptChanges()

            For Each row In m_tblFahrzeuge.Rows
                row("RowID") = CStr(row("EQUNR"))
                row("ZZDAT_EIN") = row("ZZDAT_EIN")

                Try
                    If CStr(row("ZZFARBE")) <> String.Empty Then
                        row("Farbname") = colorTextArray(CInt(row("ZZFARBE")))
                        row("Farbe") = "<a style=""color:#" & colorValueArray(CInt(row("ZZFARBE"))) & """>&#149;&#149;&#149</a>&nbsp;" & colorTextArray(CInt(row("ZZFARBE")))
                    Else
                        row("Farbname") = "k.A."
                        row("Farbe") = "k.A."
                    End If
                Catch ex As Exception
                    row("Farbname") = "k.A."
                    row("Farbe") = "k.A."
                End Try

                row("Art") = "CAR"
                row("Anzahl") = 1

                row("SelectedDate") = String.Empty                        'Eingetragenes Zulassungsdatum
                row("SelectedKennzeichenserie") = String.Empty                'Eingetragener Versicherung
                row("SelectedKennzeichenserieText") = String.Empty                'Eingetragener Versicherung
                row("SelectedSonderserie") = String.Empty               'Eingetragener Versicherung
                row("SelectedEinzel") = False                                 'Eingentragene Fahrzeuganzahl
                row("SelectedAlle") = 0
                row("PDIAnzahl") = String.Empty
                row("MODAnzahl") = String.Empty
                row("SelectedAnzahl") = 0
                row("Status") = String.Empty
                row("Bemerkung") = String.Empty
                row("SelectedZielPDI") = String.Empty
                row("SelectedZielPDIText") = String.Empty
                row("ZUSATZDATEN") = String.Empty

                'Eingangsdatum
                Try
                    row("Eingangsdatum") = CDate(row("ZZDAT_EIN"))
                Catch ex As Exception
                    'row("Eingangsdatum") = String.Empty
                End Try

                'Zusätzliche Daten----------------------
                row("SIPPCODE") = CType(row("ZZSIPP1"), String) & CType(row("ZZSIPP2"), String) & CType(row("ZZSIPP3"), String) & CType(row("ZZSIPP4"), String)

                If CType(row("ZZNAVI"), String) <> String.Empty Then
                    row("ZUSATZDATEN") = CType(row("ZUSATZDATEN"), String) & CType(row("ZZNAVI"), String)
                End If
                If CType(row("ZZREIFEN"), String) <> String.Empty Then
                    row("ZUSATZDATEN") = CType(row("ZUSATZDATEN"), String) & "," & CType(row("ZZREIFEN"), String)
                End If

                'Bemerkungen füllen
                If Not (row("QMNUM") Is Nothing) Then
                    rowBemerkung = PTexte.Select("QMNUM='" & CStr(row("QMNUM")) & "'")
                    If (rowBemerkung.Length = 1) Then
                        row("Bemerkung") = CStr(rowBemerkung(0)("TDLINE"))
                    Else
                        row("Bemerkung") = String.Empty
                    End If
                Else
                    row("Bemerkung") = String.Empty
                End If

                row("PDI") = CStr(row("KUNPDI")) & " - " & CStr(row("DADPDI_NAME1"))

                m_tblFahrzeuge.AcceptChanges()
            Next

            m_tblResult = m_tblFahrzeuge

            m_intStatus = 0
            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblFahrzeuge)

        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    'm_intStatus = -1234
                    'm_strMessage = "Es wurden keine Daten gefunden."
                    RaiseError("-1234", "Es wurden keine Daten gefunden.")
                Case "NO_WEB"
                    'm_intStatus = -1234
                    'm_strMessage = "Keine Web-Tabelle erstellt."
                    RaiseError("-1234", "Keine Web-Tabelle erstellt.")
                Case Else
                    'm_intStatus = -9999
                    'm_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    RaiseError("-9999", HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
            End Select

            If m_intIDSAP > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
            End If

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", ZZCARPORT=" & m_strPDINummerSuche & " , " & Replace(m_strMessage, "<br>", " "), Nothing)
        End Try
    End Sub

    Private Sub ClearFilters()
        FilterPdi = ""
        FilterHersteller = ""
        FilterModelId = ""
        FilterFarbe = ""
        FilterReifenart = ""
        FilterSipp = ""
        FilterNavi = ""
        FilterAhk = ""
        FilterAuftragsnrVon = ""
        FilterAuftragsnrBis = ""
        FilterFahrgnr = ""
        FilterSperre = " "c
    End Sub

    Public Sub GetPdIs(ByRef strStatus As String)
        Dim row As DataRow
        Dim rowPdIs As DataRow()
        Dim rowNew As DataRow

        strStatus = String.Empty

        Try
            m_tblAllPDIs = New DataTable()
            With m_tblAllPDIs.Columns
                .Add("KUNPDI", GetType(System.String))
                .Add("PDIAnzahl", GetType(System.String))
                .Add("PDIName", GetType(System.String))
            End With

            For Each row In m_tblResult.Rows 'm_tblFahrzeuge.Rows
                rowPdIs = m_tblAllPDIs.Select("KUNPDI='" & CType(row("KUNPDI"), String) & "'")
                If rowPdIs.Length = 0 Then  'PDI noch nicht gefunden
                    rowNew = m_tblAllPDIs.NewRow
                    rowNew("PDIAnzahl") = row("KUNPDI")
                    rowNew("KUNPDI") = row("KUNPDI")
                    rowNew("PDIName") = row("DADPDI_NAME1")
                    m_tblAllPDIs.Rows.Add(rowNew)
                    m_tblAllPDIs.AcceptChanges()
                End If
            Next
        Catch ex As Exception
            strStatus = "Fehler bei der Ermittlung der PDIs."
        End Try
    End Sub

    Public Sub GetMoDs(ByRef strStatus As String)
        Dim row As DataRow
        Dim rowPdi As DataRow
        Dim rowPdIs As DataRow()
        Dim rowMoDs As DataRow()
        Dim rowNew As DataRow

        strStatus = String.Empty

        If m_tblAllPDIs Is Nothing Then
            GetPdIs(strStatus)
        End If

        'Dim lngTimeStart As Date = Now

        Try
            If strStatus = String.Empty Then

                m_tblAllMODs = New DataTable()
                With m_tblAllMODs.Columns
                    .Add("ZZMODELL", GetType(System.String))
                    .Add("MODAnzahl", GetType(System.String))
                    .Add("KUNPDI", GetType(System.String))
                    .Add("MODName", GetType(System.String))
                End With

                For Each row In m_tblAllPDIs.Rows
                    'rowPdIs = m_tblFahrzeuge.Select("KUNPDI='" & CType(row("KUNPDI"), String) & "'")
                    rowPdIs = m_tblResult.Select("KUNPDI='" & CType(row("KUNPDI"), String) & "'")

                    For Each rowPdi In rowPdIs      'PDIs durchgehen
                        rowMoDs = m_tblAllMODs.Select("KUNPDI='" & CType(rowPdi("KUNPDI"), String) & "' AND ZZMODELL='" & CType(rowPdi("ZZMODELL"), String) & "'")
                        If rowMoDs.Length = 0 Then          'Modell noch nicht gefunden
                            rowNew = m_tblAllMODs.NewRow
                            rowNew("MODAnzahl") = rowPdi("ZZMODELL")
                            rowNew("ZZMODELL") = rowPdi("ZZMODELL")
                            rowNew("KUNPDI") = rowPdi("KUNPDI")
                            rowNew("MODName") = rowPdi("ZZBEZEI")
                            m_tblAllMODs.Rows.Add(rowNew)
                            m_tblAllMODs.AcceptChanges()
                        End If
                    Next
                Next
            End If
        Catch ex As Exception
            strStatus = "Fehler bei der Ermittlung der Modelle."
        End Try

        'Dim lngTimeEnd As Date = Now

    End Sub

    Public Function GetSummen(ByVal tblZulassungen As DataTable, ByRef status As String) As DataTable
        Dim tblReturn As New DataTable()
        Dim rowsZulassungen As DataRow()
        Dim row As DataRow
        Dim rows As DataRow()
        Dim rowNew As DataRow
        Dim intGesamt As Integer

        status = String.Empty

        Try
            tblReturn.Columns.Add("Modell", GetType(System.String))
            tblReturn.Columns.Add("ModellAnzeige", GetType(System.String))
            tblReturn.Columns.Add("Anzahl", GetType(System.Int16))
            tblReturn.AcceptChanges()

            rowsZulassungen = tblZulassungen.Select("SelectedEinzel=True AND Art='CAR'")

            For Each row In rowsZulassungen
                rows = tblReturn.Select("Modell='" & CStr(row("ZZMODELL")) & "'")
                If rows.Length = 0 Then     'Noch nicht gefunden, einfügen
                    rowNew = tblReturn.NewRow
                    rowNew("Modell") = CStr(row("ZZMODELL"))
                    rowNew("ModellAnzeige") = CStr(row("ZZMODELL")) & " / " & CStr(row("ZZBEZEI"))
                    rowNew("Anzahl") = 1
                    tblReturn.Rows.Add(rowNew)
                    tblReturn.AcceptChanges()
                Else
                    'gefunden, Zähler hochsetzen...
                    rows(0)("Anzahl") = CInt(rows(0)("Anzahl")) + 1
                    tblReturn.AcceptChanges()
                End If
            Next

            intGesamt = 0

            For Each row In tblReturn.Rows
                row("ModellAnzeige") = Right("000" & CStr(row("Anzahl")), 3) & " x " & CStr(row("ModellAnzeige"))
                intGesamt += CInt(row("Anzahl"))
            Next

            'Gesamtsumme:
            rowNew = tblReturn.NewRow()
            rowNew("ModellAnzeige") = "------"
            rowNew("Anzahl") = 0
            tblReturn.Rows.Add(rowNew)
            tblReturn.AcceptChanges()
            tblReturn.AcceptChanges()

            rowNew = tblReturn.NewRow()
            rowNew("ModellAnzeige") = Right("000" & CStr(intGesamt), 3) & " Fahrzeuge gesamt."
            rowNew("Anzahl") = 1
            tblReturn.Rows.Add(rowNew)
            tblReturn.AcceptChanges()
            tblReturn.AcceptChanges()

        Catch ex As Exception
            status = "Fehler bei der Ermittlung der Summen."
        End Try

        Return tblReturn
    End Function

    Public Overrides Sub Show()
    End Sub

    Public Overrides Sub change()
    End Sub

    Public Sub FahrzeugeSperren(ByVal tblFahrzeuge As DataTable, ByVal strAppId As String, ByVal strSessionId As String)
        Dim rows As DataRow()
        Dim i As Integer

        Try
            m_intStatus = 0
            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Warenkorb_Sperre_001", m_objApp, m_objUser, page)

            'Dim tblFahrzeugeSAP As DataTable = myProxy.getImportTable("GT_IN")
            Dim tblFahrzeugeSap As DataTable = S.AP.GetImportTableWithInit("Z_M_Warenkorb_Sperre_001.GT_IN")

            Dim tblFahrzeugeSapRow As DataRow


            'Fahrzeuge übernehmen
            rows = tblFahrzeuge.Select("SelectedEinzel=True AND Art='CAR'")

            For i = 0 To rows.Length - 1
                tblFahrzeugeSapRow = tblFahrzeugeSap.NewRow
                tblFahrzeugeSapRow("Equnr") = CType(rows(i)("EQUNR"), String)
                tblFahrzeugeSap.Rows.Add(tblFahrzeugeSapRow)
            Next

            'SAP-Aufruf
            'myProxy.callBapi()
            S.AP.Execute()

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, tblFahrzeuge)
        Catch ex As Exception
            m_intStatus = -9999
            m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & m_strMessage, tblFahrzeuge)
        End Try
    End Sub

#End Region

#Region "Neue Selektion"

    ''' <summary>
    ''' Baut Filterlisten und Auslastungstabelle auf.
    ''' </summary>
    ''' <param name="tblBestand">tblSAPBestand</param>
    Private Sub CreateListsFromBestand(ByRef tblBestand As DataTable)
        If tblBestand IsNot Nothing Then

            lstPDI.Clear()
            lstHersteller.Clear()
            lstModellID.Clear()
            lstSIPPCode.Clear()
            lstReifenart.Clear()
            lstFarbe.Clear()
            lstNavi.Clear()
            lstAhk.Clear()

            For Each row As DataRow In tblBestand.Rows
                ' Listen
                If Not lstPDI.Contains(CStr(row("PDI"))) Then
                    lstPDI.Add(CStr(row("PDI")))
                End If

                If Not lstHersteller.Contains(CStr(row("HERST_T"))) Then
                    lstHersteller.Add(CStr(row("HERST_T")))
                End If

                If Not lstModellID.Contains(CStr(row("ZZMODELL"))) Then
                    lstModellID.Add(CStr(row("ZZMODELL")))
                End If

                Dim strSippCode As String = CStr(row("ZZSIPP1")) & CStr(row("ZZSIPP2")) & CStr(row("ZZSIPP3")) & CStr(row("ZZSIPP4"))
                If Not lstSIPPCode.Contains(strSippCode) Then
                    lstSIPPCode.Add(strSippCode)
                End If

                If Not lstReifenart.Contains(CStr(row("ZZREIFEN"))) Then
                    lstReifenart.Add(CStr(row("ZZREIFEN")))
                End If

                If Not lstFarbe.Contains(CStr(row("Farbname"))) Then
                    lstFarbe.Add(CStr(row("Farbname")))
                End If

                Dim hatNavi As String = ""
                If row("ZNAVI").ToString() = "X" Then
                    hatNavi = "ja"
                Else
                    hatNavi = "nein"
                End If
                If Not lstNavi.Contains(hatNavi) Then
                    lstNavi.Add(hatNavi)
                End If

                Dim hatAhk As String = ""
                If row("ZAHK").ToString() = "X" Then
                    hatAhk = "ja"
                Else
                    hatAhk = "nein"
                End If
                If Not lstAhk.Contains(hatAhk) Then
                    lstAhk.Add(hatAhk)
                End If
            Next

            lstPDI.Sort(Function(str1, str2) str1.CompareTo(str2))
            lstHersteller.Sort(Function(str1, str2) str1.CompareTo(str2))
            lstModellID.Sort(Function(str1, str2) str1.CompareTo(str2))
            lstSIPPCode.Sort(Function(str1, str2) str1.CompareTo(str2))
            lstReifenart.Sort(Function(str1, str2) str1.CompareTo(str2))
            lstFarbe.Sort(Function(str1, str2) str1.CompareTo(str2))
            lstNavi.Sort(Function(str1, str2) str1.CompareTo(str2))
            lstAhk.Sort(Function(str1, str2) str1.CompareTo(str2))

            ' Leere Einträge zum löschen der Filter einfügen
            If Not lstPDI.Contains("") Then
                lstPDI.Insert(0, "")
            End If

            If Not lstHersteller.Contains("") Then
                lstHersteller.Insert(0, "")
            End If

            If Not lstModellID.Contains("") Then
                lstModellID.Insert(0, "")
            End If

            If Not lstSIPPCode.Contains("") Then
                lstSIPPCode.Insert(0, "")
            End If

            If Not lstReifenart.Contains("") Then
                lstReifenart.Insert(0, "")
            End If

            If Not lstFarbe.Contains("") Then
                lstFarbe.Insert(0, "")
            End If

            If Not lstNavi.Contains("") Then
                lstNavi.Insert(0, "")
            End If

            If Not lstAhk.Contains("") Then
                lstAhk.Insert(0, "")
            End If

        End If
    End Sub

    ''' <summary>
    ''' Erzeugt die Listen mit Filterwerten für die vorhandenen Fahrzeuge
    ''' </summary>
    Sub FillFilterLists(Optional ByVal pdi As String = "", Optional ByVal hersteller As String = "", Optional modelId As String = "",
                        Optional ByVal sipp As String = "", Optional ByVal reifenart As String = "", Optional farbe As String = "",
                        Optional ByVal navi As String = "", Optional ByVal ahk As String = "", Optional auftragsnrvon As String = "",
                        Optional auftragsnrbis As String = "", Optional fahrgnr As String = "", Optional gesperrte As Char = " "c)

        ' Filterwerte puffern, damit sie in den DropDowns wieder selektiert werden können
        FilterPdi = pdi
        FilterHersteller = hersteller
        FilterModelId = modelId
        FilterSipp = sipp
        FilterReifenart = reifenart
        FilterFarbe = farbe
        FilterNavi = navi
        FilterAhk = ahk
        FilterAuftragsnrVon = auftragsnrvon
        FilterAuftragsnrBis = auftragsnrbis
        FilterFahrgnr = fahrgnr
        FilterSperre = gesperrte

        Dim strFilter As String = ""

        If Not String.IsNullOrEmpty(FilterPdi) Then
            If strFilter.Length > 0 Then
                strFilter &= " AND "
            End If
            strFilter &= "PDI='" & FilterPdi & "'"
        End If

        If Not String.IsNullOrEmpty(FilterHersteller) Then
            If strFilter.Length > 0 Then
                strFilter &= " AND "
            End If
            strFilter &= "HERST_T='" & FilterHersteller & "'"
        End If

        If Not String.IsNullOrEmpty(FilterModelId) Then
            If strFilter.Length > 0 Then
                strFilter &= " AND "
            End If
            strFilter &= "ZZMODELL='" & FilterModelId & "'"
        End If

        If Not String.IsNullOrEmpty(FilterSipp) Then
            If strFilter.Length > 0 Then
                strFilter &= " AND "
            End If
            strFilter &= "SIPPCODE='" & FilterSipp & "'"
        End If

        If Not String.IsNullOrEmpty(FilterReifenart) Then
            If strFilter.Length > 0 Then
                strFilter &= " AND "
            End If
            strFilter &= "ZZREIFEN='" & FilterReifenart & "'"
        End If

        If Not String.IsNullOrEmpty(FilterFarbe) Then
            Dim farbcode As Integer = Array.IndexOf(colorTextArray, FilterFarbe)
            If farbcode >= 0 Then
                If strFilter.Length > 0 Then
                    strFilter &= " AND "
                End If
                strFilter &= "ZZFARBE='" & farbcode.ToString() & "'"
            ElseIf FilterFarbe = "k.A." Then
                If strFilter.Length > 0 Then
                    strFilter &= " AND "
                End If
                strFilter &= "ZZFARBE='-'"
            End If
        End If

        If Not String.IsNullOrEmpty(FilterNavi) Then
            Dim mitNavi As String = IIf(FilterNavi = "ja", "X", " ").ToString()
            If strFilter.Length > 0 Then
                strFilter &= " AND "
            End If
            strFilter &= "ZNAVI='" & mitNavi & "'"
        End If

        If Not String.IsNullOrEmpty(FilterAhk) Then
            Dim mitAhk As String = IIf(FilterAhk = "ja", "X", " ").ToString()
            If strFilter.Length > 0 Then
                strFilter &= " AND "
            End If
            strFilter &= "ZAHK='" & mitAhk & "'"
        End If

        If Not String.IsNullOrEmpty(auftragsnrvon) Then
            If strFilter.Length > 0 Then
                strFilter &= " AND "
            End If
            strFilter &= "[ZZREF1] >= '" & auftragsnrvon & "'"
        End If

        If Not String.IsNullOrEmpty(auftragsnrbis) Then
            If strFilter.Length > 0 Then
                strFilter &= " AND "
            End If
            strFilter &= "[ZZREF1] <= '" & auftragsnrbis & "'"
        End If

        If Not String.IsNullOrEmpty(fahrgnr) Then
            If strFilter.Length > 0 Then
                strFilter &= " AND "
            End If
            strFilter &= "ZZFAHRG = '" & fahrgnr & "'"
        End If

        If Not String.IsNullOrEmpty(FilterSperre) Then
            If strFilter.Length > 0 Then
                strFilter &= " AND "
            End If
            strFilter &= "ZZAKTSPERRE='" & FilterSperre & "'"
        End If

        Dim dv As DataView = m_tblFahrzeuge.DefaultView
        dv.RowFilter = strFilter
        m_tblResult = dv.ToTable()

        CreateListsFromBestand(m_tblResult)
    End Sub

#End Region


End Class

' ************************************************
' $History: change_01.vb $
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 1.12.09    Time: 16:16
' Updated in $/CKAG/Applications/appec/Lib
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 21.08.09   Time: 12:20
' Updated in $/CKAG/Applications/appec/Lib
' ITA: 2918
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 8.07.09    Time: 11:40
' Updated in $/CKAG/Applications/appec/Lib
' Helpprocedures DataTableAlphabeticSort hinzugefügt
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 30.06.09   Time: 8:44
' Updated in $/CKAG/Applications/appec/Lib
' ITA 2948
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 29.06.09   Time: 9:23
' Updated in $/CKAG/Applications/appec/Lib
' ITA 2918 Z_M_Fehlend_Unfall_001, Z_M_Schlue_Set_Mahnsp_001
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 25.06.09   Time: 16:04
' Updated in $/CKAG/Applications/appec/Lib
' ITA 2918 Z_M_Herstellergroup, Z_M_EC_AVM_BATCH_update,
' Z_M_EC_AVM_HERST_VWZWECK_MODID, Z_M_EC_AVM_BATCH_INSERT,
' Z_M_EC_AVM_BATCH_ANSICHT
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 25.06.09   Time: 14:20
' Updated in $/CKAG/Applications/appec/Lib
' ITA 2918 Z_M_WARENKORB_SPERRE, Z_MASSENZULASSUNG,
' Z_M_EC_AVM_KENNZ_SERIE, Z_M_EC_AVM_PDIWECHSEL,
' Z_M_EC_AVM_ZULASSUNGSSPERRE
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 17.06.09   Time: 8:47
' Updated in $/CKAG/Applications/appec/Lib
' Warnungen entfernt!
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 24.07.08   Time: 14:52
' Updated in $/CKAG/Applications/appec/Lib
' SAP connection Closed verbessert/hinzugefügt
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 15:49
' Created in $/CKAG/Applications/appec/Lib
' 
' *****************  Version 27  *****************
' User: Fassbenders  Date: 25.03.08   Time: 17:33
' Updated in $/CKG/Applications/AppEC/AppECWeb/Lib
' 
' *****************  Version 26  *****************
' User: Jungj        Date: 28.01.08   Time: 15:09
' Updated in $/CKG/Applications/AppEC/AppECWeb/Lib
' Funktioniert nicht! ITA 1655
' 
' *****************  Version 25  *****************
' User: Jungj        Date: 22.10.07   Time: 18:09
' Updated in $/CKG/Applications/AppEC/AppECWeb/Lib
' 
' *****************  Version 24  *****************
' User: Jungj        Date: 22.10.07   Time: 13:34
' Updated in $/CKG/Applications/AppEC/AppECWeb/Lib
' 
' *****************  Version 23  *****************
' User: Uha          Date: 2.07.07    Time: 17:23
' Updated in $/CKG/Applications/AppEC/AppECWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 22  *****************
' User: Uha          Date: 7.03.07    Time: 11:02
' Updated in $/CKG/Applications/AppEC/AppECWeb/Lib
' 
' ************************************************

