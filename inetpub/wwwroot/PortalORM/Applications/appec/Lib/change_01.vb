Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports System.Data.SqlClient
Imports CKG.Base.Common


Public Class change_01
    REM § Lese-/Schreibfunktion, Kunde: SIXT, 
    REM § New - BAPI: Z_M_Meldungen_Pdi,
    REM § Show - BAPI: Z_M_Meldungen_Fahrzeuge,
    REM § ShowDatenABE - BAPI: Z_M_Abezufzg,
    REM § SelectCars - BAPI: ,
    REM § Change - BAPI: Z_Massenzulassung, Z_M_Zulassungssperre, Z_M_Pdiwechsel.

    Inherits Base.Business.BankBase  'FDD_Bank_Base

#Region " Declarations"
    Private m_intErrorCount As Int32
    Private m_objABE_Daten As Base.Business.ABE2FHZG
    Private m_dsPDI_Data As DataSet
    Private m_dsPDI_Data_Selected As DataSet
    Private m_tblPDIs As DataTable
    Private m_tblAllPDIs As DataTable
    Private m_tblAllMODs As DataTable
    Private m_tblModelle As DataTable
    Private m_tblFahrzeuge As DataTable
    Private m_tblSaveCars As DataTable
    Private m_strPDINummer As String
    Private m_strPDINummerSuche As String
    Private m_strModellCode As String
    Private m_strFahrgestellnummer As String
    Private m_intLastID As Int32
    Private m_strTask As String
    Private m_intSelectedCars As Int32
    Private m_blnShowBelegnummer As Boolean
    Private m_intFahrzeugeGesamtZulassungsf As Int32
    Private m_intFahrzeugeGesamtGesperrt As Int32
    Private m_tblErledigt As DataTable
    Private data As ArrayList
    Private m_pdiListe As DataTable
    Private returnList As ArrayList
    'Private m_versicherer As VersichererList
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

    Private Const strTaskZulassen As String = "Zulassen"
    Private Const strTaskSperren As String = "Sperren"
    Private Const strTaskEntsperren As String = "Entsperren"
    Private Const strTaskVerschieben As String = "Verschieben"


#End Region
    'Dim returnTable As New SAPProxy_SIXT_01.BAPIRET2Table() 'Nur Temporär.
    'Dim returnValue As String = "Hallo"
#Region " Properties"
    Public Property PDIListe() As DataTable
        Get
            Return m_pdiListe
        End Get
        Set(ByVal Value As DataTable)
            m_pdiListe = Value
        End Set
    End Property

    Public ReadOnly Property PTexte() As DataTable
        Get
            Return m_tblFahrzeugeText
        End Get
    End Property

    Public Property PSperre() As Char
        Get
            Return m_Sperre
        End Get
        Set(ByVal Value As Char)
            m_Sperre = Value
        End Set
    End Property

    Public Property PPhase() As Char
        Get
            Return m_phase
        End Get
        Set(ByVal Value As Char)
            m_phase = Value
        End Set
    End Property

    Public Property PSelectionPDI() As Boolean
        Get
            Return m_blnSelectedPDI
        End Get
        Set(ByVal Value As Boolean)
            m_blnSelectedPDI = Value
        End Set
    End Property

    Public Property PSelectionMOD() As Boolean
        Get
            Return m_blnSelectedMOD
        End Get
        Set(ByVal Value As Boolean)
            m_blnSelectedMOD = Value
        End Set
    End Property

    Public Property PSelectedMOD() As String
        Get
            Return m_selectedMOD
        End Get
        Set(ByVal Value As String)
            m_selectedMOD = Value
        End Set
    End Property


    Public Property PSelectedPDI() As String
        Get
            Return m_selectedPDI
        End Get
        Set(ByVal Value As String)
            m_selectedPDI = Value
        End Set
    End Property

    Public Property PPDISuche() As String
        Get
            Return m_strPDISuche
        End Get
        Set(ByVal Value As String)
            m_strPDISuche = Value
        End Set
    End Property


    Public Property PGrid() As DataGrid
        Get
            Return gridMain
        End Get
        Set(ByVal Value As DataGrid)
            gridMain = Value
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

    'Public ReadOnly Property PVersicherer() As DataTable
    '    Get
    '        Return m_versicherer
    '    End Get
    'End Property

    Public ReadOnly Property ErrorCount() As Int32
        Get
            Return m_intErrorCount
        End Get
    End Property

    Public ReadOnly Property Erledigt() As DataTable
        Get
            Return m_tblErledigt
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

    Public Property ABE_Daten() As Base.Business.ABE2FHZG
        Get
            Return m_objABE_Daten
        End Get
        Set(ByVal Value As Base.Business.ABE2FHZG)
            m_objABE_Daten = Value
        End Set
    End Property

    Public ReadOnly Property AllPDIs() As DataTable
        Get
            Return m_tblAllPDIs
        End Get
    End Property

    Public ReadOnly Property AllMODs() As DataTable
        Get
            Return m_tblAllMODs
        End Get
    End Property

    Public ReadOnly Property PDI_Data_Selected() As DataSet
        Get
            Return m_dsPDI_Data_Selected
        End Get
    End Property

    Public ReadOnly Property ShowBelegnummer() As Boolean
        Get
            Return m_blnShowBelegnummer
        End Get
    End Property

    Public ReadOnly Property PDI_Data() As DataSet
        Get
            Return m_dsPDI_Data
        End Get
    End Property

    Public Property Task() As String
        Get
            Return m_strTask
        End Get
        Set(ByVal Value As String)
            m_strTask = Value
        End Set
    End Property

    Public Property PDINummer() As String
        Get
            Return m_strPDINummer
        End Get
        Set(ByVal Value As String)
            m_strPDINummer = Value
        End Set
    End Property

    Public Property PDINummerSuche() As String
        Get
            Return m_strPDINummerSuche
        End Get
        Set(ByVal Value As String)
            m_strPDINummerSuche = Value
        End Set
    End Property

    Public Property Fahrgestellnummer() As String
        Get
            Return m_strFahrgestellnummer
        End Get
        Set(ByVal Value As String)
            m_strFahrgestellnummer = Value
        End Set
    End Property

    Public Property ModellCode() As String
        Get
            Return m_strModellCode
        End Get
        Set(ByVal Value As String)
            m_strModellCode = Value
        End Set
    End Property

    Public ReadOnly Property SelectedCars() As Int32
        Get
            Return m_intSelectedCars
        End Get
    End Property

    Public ReadOnly Property confirmList() As ArrayList
        Get
            Return returnList
        End Get
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
        getVerwendung(strAppID, strSessionID, New Page)
        getKennzeichenserie(strAppID, strSessionID, New Page)
    End Sub

    Public Sub getKennzeichenserie(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        Dim intID As Int32
        Dim row As DataRow
        Dim intRow As Integer

        m_strClassAndMethod = "Change_01.getKennzeichenserie"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Try
            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Ec_Avm_Kennz_Serie", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

            'myProxy.callBapi()

            S.AP.InitExecute("Z_M_Ec_Avm_Kennz_Serie", "I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

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
            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
            End If
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult)
        End Try
    End Sub

    Public Sub getVerwendung(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        m_strClassAndMethod = "Change_01.getVerwendung"
        m_strAppID = strAppID
        m_strSessionID = strSessionID


        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Ec_Avm_Herst_Vwzweck_Modid", m_objApp, m_objUser, page)
            S.AP.InitExecute("Z_M_Ec_Avm_Herst_Vwzweck_Modid")
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

    Public Function createZulassungCountTable(ByVal datum As Date, ByVal page As Page) As DataTable
        Dim dtAnzahlZulassungen As New DataTable()
        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Ec_Avm_Anz_Beauftr_Zul", m_objApp, m_objUser, Page)

            'myProxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
            'myProxy.setImportParameter("I_ZULDAT", datum.ToShortDateString)

            'myProxy.callBapi()

            S.AP.InitExecute("Z_M_Ec_Avm_Anz_Beauftr_Zul", "I_KUNNR,I_ZULDAT", Right("0000000000" & m_objUser.KUNNR, 10), datum.ToShortDateString)

            dtAnzahlZulassungen = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")

        Catch ex As Exception
            'do nothing
        End Try
        Return dtAnzahlZulassungen
    End Function


    Public Function setResultRowClear() As DataTable
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

    Public Sub setZulassung(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        'Zulassen
        Dim rowsZulassung As DataRow()
        Dim strZuldatum As String
        Dim strKBANR As String
        Dim strKunnr As String
        Dim strHalter As String
        Dim strVersicherer As String
        Dim strPDI As String
        Dim strFahrgestell As String
        Dim strSonder As String


        m_strClassAndMethod = "Change_01.setZulassung"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
      
        FahrzeugeSperren(Result, m_strAppID, SessionID, page)
        Try
            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_Massenzulassung", m_objApp, m_objUser, page)


            Dim zulSpeicherTable As DataTable = S.AP.GetImportTableWithInit("Z_Massenzulassung.INTERNTAB", "") 'myProxy.getImportTable("INTERNTAB")
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
                strKunnr = Right("0000000000" & m_objUser.KUNNR, 10)
                strHalter = Right("0000000000" & "100010607", 10)
                'Versichererwechsel ab 01.04.2008
                If CDate(strZuldatum) < CDate("01.04.2008") Then
                    strVersicherer = Right("0000000000106", 10)
                Else
                    strVersicherer = Right("0000000001917", 10)
                End If

                strKBANR = "0200000"
                strPDI = CStr(rowsZulassung(intCount)("DADPDI"))
                strFahrgestell = CStr(rowsZulassung(intCount)("ZZFAHRG"))
                strSonder = CStr(rowsZulassung(intCount)("SelectedSonderserie"))
                Try
                    zulSpeicher = zulSpeicherTable.NewRow
                    With zulSpeicher
                        .Item("I_Kunnr_Ag") = m_strCustomer
                        .Item("I_Zzfahrg") = strFahrgestell
                        .Item("I_Edatu") = strZuldatum
                        .Item("I_Kunnr_Zv") = strVersicherer
                        .Item("I_Zzkennz") = String.Empty
                        .Item("I_Kunnr_Zh") = strHalter
                        .Item("I_Kunnr_Za") = String.Empty
                        .Item("I_Zzsonder") = strSonder
                        .Item("I_Kbanr") = strKBANR
                        .Item("I_Zzcarport") = strPDI
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

    Public Sub setSperre(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
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
        m_strAppID = strAppID
        m_strSessionID = strSessionID
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

                    Dim tblRowBemerkungenSAP As DataRow

                    'Inputarameter füllen ......
                    strKunnr = Right("0000000000" & m_objUser.KUNNR, 10)
                    strPDI = CStr(rowsSperre(intCount)("DADPDI"))
                    strFahrgestell = CStr(rowsSperre(intCount)("ZZFAHRG"))
                    strBemerkungDatum = Now.ToShortDateString

                    S.AP.Init("Z_M_Ec_Avm_Zulassungssperre", "I_KUNNR,I_ZZFAHRG,I_ZZAKTSPERRE,I_ZZCARPORT,I_ZZDATBEM", strKunnr, strFahrgestell, m_Sperre, strPDI, strBemerkungDatum)

                    Dim tblBemerkungenSAP As DataTable = S.AP.GetImportTable("GT_TEXTE")

                    'Bemerkungstext(e) als Tabelle
                    tblRowBemerkungenSAP = tblBemerkungenSAP.NewRow
                    tblRowBemerkungenSAP("Tdline") = CStr(rowsSperre(intCount)("Bemerkung"))
                    tblBemerkungenSAP.Rows.Add(tblRowBemerkungenSAP)


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

    Public Sub setVerschieb(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        'Verschieben
        Dim intID As Int32
        Dim rowsVerschieb As DataRow()
        Dim strKunnr As String
        Dim strBemerkungDatum As String
        Dim strPDI As String
        Dim strPDINeu As String
        Dim strQmNr As String
        Dim intResult As Integer

        m_strClassAndMethod = "Change_01.setVerschieb"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Try
            Dim intCount As Integer

            'Tabellenzeile identifizieren
            rowsVerschieb = Result.Select("SelectedEinzel=True AND Art='CAR'")

            'Alle FAhrzeuge durchgehen.....
            For intCount = 0 To rowsVerschieb.Length - 1
                Try
                    'Inputarameter füllen ......
                    strKunnr = Right("0000000000" & m_objUser.KUNNR, 10)
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

            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
            End If
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult)
        End Try
    End Sub

    Private Sub getCars2()
        Dim rowFahrzeug As DataRow
        Dim rowFahrzeuge As DataRow()
        Dim rowPDIs As DataRow()
        Dim rowPDI As DataRow
        Dim rowModels As DataRow()
        Dim newRow As DataRow
        Dim newRowModel As DataRow
        Dim newRowCar As DataRow
        Dim anzahl As Int32
        Dim rowAutos As DataRow()
        Dim rowAuto As DataRow
        Dim strHelp As String
        Dim rowBemerkung As DataRow()
        Dim intID_PDI As Integer
        Dim intID_MODEL As Integer
        Dim intID_CAR As Integer = 0

        Dim colorTextArray() As String = {"weiss", "gelb", "orange", "rot", "violett", "blau", "grün", "grau", "braun", "schwarz"}
        Dim colorValueArray() As String = {"FFFFFF", "FFFF00", "FF8040", "FF0000", "FF00FF", "0000FF", "00FF00", "808080", "804000", "000000"}


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
        End With

        m_tblResult = m_tblFahrzeuge.Clone()

        intID_PDI = 0

        For Each rowFahrzeug In m_tblFahrzeuge.Rows

            rowFahrzeuge = m_tblResult.Select("KUNPDI='" & CType(rowFahrzeug("KUNPDI"), String) & "'")

            If (rowFahrzeuge.Length = 0) Then  'Kein Eintrag gefunden
                '##############################1. Kopfzeile (PDI)

                newRow = m_tblResult.NewRow()

                intID_PDI += 1
                newRow("RowID") = "P" & Right("00000" & CStr(intID_PDI), 5)         'ID vergeben
                strHelp = Right("00000" & CStr(intID_PDI), 5)

                newRow("KUNPDI") = CType(rowFahrzeug("KUNPDI"), String)
                newRow("Art") = "PDI"

                newRow("DetailButton") = strHelp
                newRow("Anzahl") = 1
                newRow("SelectedAlle") = 0

                newRow("SelectedEinzel") = False                               'Eingentragene Fahrzeuganzahl
                newRow("SelectedAnzahl") = 0
                newRow("PDIAnzahl") = CType(rowFahrzeug("KUNPDI"), String)  'Darstellung in ListBox (View)

                m_tblResult.Rows.Add(newRow)

                '############################2. Kopfzeile (Modell). Alle Modelle mit der gefundenen PDI suchen
                'Alle Zeilen mit der gefundenen PDI holen...
                rowPDIs = m_tblFahrzeuge.Select("KUNPDI='" & CType(newRow("KUNPDI"), String) & "'")

                intID_MODEL = 0
                For Each rowPDI In rowPDIs

                    'Alle Modelle suchen...
                    rowModels = m_tblResult.Select("KUNPDI='" & CType(rowPDI("KUNPDI"), String) & "' AND ZZMODELL='" & CType(rowPDI("ZZMODELL"), String) & "'")
                    If rowModels.Length = 0 Then    'Noch kein Model gefunden
                        newRowModel = m_tblResult.NewRow()

                        intID_MODEL += 1
                        newRowModel("RowID") = "M" & Right("00000" & CStr(intID_PDI), 5) & "." & Right("00000" & CStr(intID_MODEL), 5)             'ID vergeben
                        strHelp = Right("00000" & CStr(intID_PDI), 5) & "." & Right("00000" & CStr(intID_MODEL), 5)              'ID vergeben

                        newRowModel("KUNPDI") = CType(rowPDI("KUNPDI"), String)
                        newRowModel("ZZMODELL") = CType(rowPDI("ZZMODELL"), String)
                        newRowModel("ZZFAHRG") = String.Empty
                        newRowModel("ZZDAT_EIN") = String.Empty
                        newRowModel("ZZFARBE") = String.Empty
                        newRowModel("Art") = "MOD"

                        newRowModel("DetailButton") = strHelp

                        newRowModel("SelectedDate") = String.Empty                      'Eingetragenes Zulassungsdatum
                        newRowModel("SelectedKennzeichenserie") = String.Empty              'Eingetragener Versicherung
                        newRowModel("SelectedKennzeichenserieText") = String.Empty              'Eingetragener Versicherung
                        newRowModel("SelectedSonderserie") = String.Empty               'Eingetragener Versicherung
                        newRowModel("SelectedEinzel") = False                              'Eingentragene Fahrzeuganzahl
                        newRowModel("SelectedAlle") = 0
                        newRowModel("PDIAnzahl") = String.Empty
                        newRowModel("MODAnzahl") = CType(rowPDI("ZZMODELL"), String)    'Darstellung in Listbox (View)
                        newRowModel("SelectedAnzahl") = 0
                        newRowModel("Status") = String.Empty
                        newRowModel("Bemerkung") = String.Empty

                        'Zusätzliche Daten----------------------
                        newRowModel("SIPPCODE") = String.Empty
                        newRowModel("HERST_K") = String.Empty
                        newRowModel("ZZBEZEI") = CType(rowPDI("ZZBEZEI"), String)
                        newRowModel("ZZANTR") = String.Empty
                        newRowModel("ZZNAVI") = String.Empty
                        newRowModel("ZZREIFEN") = String.Empty

                        newRowModel("ZZAKTSPERRE") = String.Empty
                        newRowModel("ZZVERWENDUNG") = String.Empty
                        '---------------------------------------

                        m_tblResult.Rows.Add(newRowModel)

                        '############################3. Die Fahrzeuge, die zum PDI und Modell passen
                        rowAutos = m_tblFahrzeuge.Select("KUNPDI='" & CType(newRowModel("KUNPDI"), String) & "' AND ZZMODELL='" & CType(newRowModel("ZZMODELL"), String) & "'")

                        intID_CAR = 0
                        For Each rowAuto In rowAutos
                            newRowCar = m_tblResult.NewRow()

                            intID_CAR += 1
                            newRowCar("RowID") = "C" & Right("00000" & CStr(intID_PDI), 5) & "." & Right("00000" & CStr(intID_MODEL), 5) & "." & Right("00000" & CStr(intID_CAR), 5)                'ID vergeben

                            newRowCar("KUNPDI") = CType(rowAuto("KUNPDI"), String)
                            newRowCar("DADPDI") = CType(rowAuto("DADPDI"), String)
                            newRowCar("ZZMODELL") = CType(rowAuto("ZZMODELL"), String)
                            newRowCar("ZZFAHRG") = CType(rowAuto("ZZFAHRG"), String)
                            newRowCar("ZZDAT_EIN") = MakeDateStandard(CType(rowAuto("ZZDAT_EIN"), String)).ToShortDateString

                            If CStr(rowAuto("ZZFARBE")) <> String.Empty Then
                                newRowCar("ZZFARBE") = "<a style=""color:#" & colorValueArray(CInt(rowAuto("ZZFARBE"))) & """>&#149;&#149;&#149</a>&nbsp;" & colorTextArray(CInt(rowAuto("ZZFARBE")))
                            Else
                                newRowCar("ZZFARBE") = "k.A."
                            End If

                            newRowCar("Art") = "CAR"
                            newRowCar("Anzahl") = 1

                            newRowCar("SelectedDate") = String.Empty                        'Eingetragenes Zulassungsdatum
                            newRowCar("SelectedKennzeichenserie") = String.Empty                'Eingetragener Versicherung
                            newRowCar("SelectedKennzeichenserieText") = String.Empty                'Eingetragener Versicherung
                            newRowCar("SelectedSonderserie") = String.Empty               'Eingetragener Versicherung
                            newRowCar("SelectedEinzel") = False                                 'Eingentragene Fahrzeuganzahl
                            newRowCar("SelectedAlle") = 0
                            newRowCar("PDIAnzahl") = String.Empty
                            newRowCar("MODAnzahl") = String.Empty
                            newRowCar("SelectedAnzahl") = 0
                            newRowCar("Status") = String.Empty
                            newRowCar("Bemerkung") = String.Empty
                            newRowCar("QMNUM") = CType(rowAuto("QMNUM"), String)
                            newRowCar("SelectedZielPDI") = String.Empty
                            newRowCar("SelectedZielPDIText") = String.Empty

                            'Zusätzliche Daten----------------------
                            newRowCar("SIPPCODE") = CType(rowPDI("ZZSIPP1"), String) & CType(rowPDI("ZZSIPP2"), String) & CType(rowPDI("ZZSIPP3"), String) & CType(rowPDI("ZZSIPP4"), String)
                            newRowCar("HERST_K") = CType(rowPDI("HERST_K"), String)
                            newRowCar("ZZBEZEI") = CType(rowPDI("ZZBEZEI"), String)
                            newRowCar("ZZANTR") = CType(rowPDI("ZZANTR"), String)
                            newRowCar("ZZNAVI") = CType(rowPDI("ZZNAVI"), String)
                            newRowCar("ZZREIFEN") = CType(rowPDI("ZZREIFEN"), String)

                            If CType(rowPDI("ZZNAVI"), String) <> String.Empty Then
                                newRowCar("ZUSATZDATEN") = CType(newRowCar("ZUSATZDATEN"), String) & CType(newRowCar("ZZNAVI"), String)
                            End If
                            If CType(rowPDI("ZZREIFEN"), String) <> String.Empty Then
                                newRowCar("ZUSATZDATEN") = CType(newRowCar("ZUSATZDATEN"), String) & "," & CType(rowPDI("ZZREIFEN"), String)
                            End If
                            newRowCar("ZZAKTSPERRE") = CType(rowPDI("ZZAKTSPERRE"), String)
                            newRowCar("ZZVERWENDUNG") = CType(rowPDI("ZZVERWENDUNG"), String)

                            'Bemerkungen füllen
                            If Not (newRowCar("QMNUM") Is Nothing) Then
                                rowBemerkung = PTexte.Select("QMNUM='" & CStr(newRowCar("QMNUM")) & "'")
                                If (rowBemerkung.Length = 1) Then
                                    newRowCar("Bemerkung") = CStr(rowBemerkung(0)("TDLINE"))
                                Else
                                    newRowCar("Bemerkung") = String.Empty
                                End If
                            Else
                                newRowCar("Bemerkung") = String.Empty
                            End If
                            '---------------------------------------
                            m_tblResult.Rows.Add(newRowCar)
                            m_tblResult.AcceptChanges()
                        Next
                        newRowModel("Anzahl") = rowAutos.Length 'Anzahl Fahrzeuge pro PDI und Model 
                        m_tblResult.AcceptChanges()
                    End If
                Next
            Else
                anzahl = CType(rowFahrzeuge(0)("Anzahl"), Int32) + 1
                rowFahrzeuge(0)("Anzahl") = anzahl
                m_tblResult.AcceptChanges()
            End If
        Next
    End Sub

    Public Sub getCars(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        'Holt alle Fahrzeuge, die zulassungsbereit sind.
        Dim row As DataRow
        Dim rowBemerkung As DataRow()

        Dim colorTextArray() As String = {"weiss", "gelb", "orange", "rot", "violett", "blau", "grün", "grau", "braun", "schwarz"}
        Dim colorValueArray() As String = {"FFFFFF", "FFFF00", "FF8040", "FF0000", "FF00FF", "0000FF", "00FF00", "808080", "804000", "000000"}

        m_strClassAndMethod = "Change_01.getCars()"

        m_intErrorCount = 0
        m_intLastID = -1
        Try

            m_intStatus = 0
            m_strMessage = ""
            m_intFahrzeugeGesamtZulassungsf = 0
            m_intFahrzeugeGesamtGesperrt = 0


            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Ec_Avm_Meldungen_Pdi1", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_VKORG", "1510")
            'myProxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
            'myProxy.setImportParameter("I_ZZCARPORT", m_strPDISuche)
            'myProxy.setImportParameter("I_PHASE", m_phase)

            'myProxy.callBapi()

            'm_tblFahrzeuge = myProxy.getExportTable("GT_WEB")
            'm_tblFahrzeugeText = myProxy.getExportTable("GT_TXT")

            S.AP.InitExecute("Z_M_Ec_Avm_Meldungen_Pdi1", "I_VKORG,I_KUNNR,I_ZZCARPORT,I_PHASE", "1510", Right("0000000000" & m_objUser.KUNNR, 10), m_strPDISuche, m_phase)

            m_tblFahrzeuge = S.AP.GetExportTable("GT_WEB")
            m_tblFahrzeugeText = S.AP.GetExportTable("GT_TXT")

            Dim intRows As Integer

            For intRows = m_tblFahrzeuge.Rows.Count - 1 To 0 Step -1
                'Filtern...
                Select Case m_strTask
                    '§§§ JVE 28.08.2006
                    Case strTaskEntsperren
                        'Bei Entsperren werden nur Fahrzeuge angezeigt, bei denen das Sperrkennzeichen gesetzt ist
                        If CStr(m_tblFahrzeuge.Rows(intRows)("ZZAKTSPERRE")) <> "X" Then
                            m_tblFahrzeuge.Rows.Remove(m_tblFahrzeuge.Rows(intRows))
                        End If
                    Case Else
                        'Bei Zulassen,Sperren,Verschieben werden nur Fahrzeuge angezeigt, bei denen das Sperrkennzeichen nicht gesetzt ist
                        If CStr(m_tblFahrzeuge.Rows(intRows)("ZZAKTSPERRE")) <> "" Then
                            m_tblFahrzeuge.Rows.Remove(m_tblFahrzeuge.Rows(intRows))
                        End If
                End Select
            Next
            m_tblFahrzeuge.AcceptChanges()

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
            End With

            m_tblFahrzeuge.Columns("ZZFARBE").MaxLength = 250
            m_tblFahrzeuge.AcceptChanges()

            For Each row In m_tblFahrzeuge.Rows
                row("RowID") = CStr(row("EQUNR"))
                row("ZZDAT_EIN") = row("ZZDAT_EIN")

                Try
                    If CStr(row("ZZFARBE")) <> String.Empty Then
                        row("ZZFARBE") = "<a style=""color:#" & colorValueArray(CInt(row("ZZFARBE"))) & """>&#149;&#149;&#149</a>&nbsp;" & colorTextArray(CInt(row("ZZFARBE")))
                    Else
                        row("ZZFARBE") = "k.A."
                    End If
                Catch ex As Exception
                    row("ZZFARBE") = "k.A."
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
                    row("Eingangsdatum") = String.Empty
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

                m_tblFahrzeuge.AcceptChanges()
            Next
            m_tblResult = m_tblFahrzeuge

            m_intStatus = 0
            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblPDIs)

        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_intStatus = -1234
                    m_strMessage = "Es wurden keine Daten gefunden."
                Case "NO_WEB"
                    m_intStatus = -1234
                    m_strMessage = "Keine Web-Tabelle erstellt."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
            End Select
            If m_intIDSAP > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
            End If
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", ZZCARPORT=" & m_strPDINummerSuche & " , " & Replace(m_strMessage, "<br>", " "), Nothing)

        End Try
    End Sub

    Public Sub getPDIs(ByRef strStatus As String)
        Dim row As DataRow
        Dim rowPDIs As DataRow()
        Dim rowNew As DataRow

        strStatus = String.Empty

        Try
            m_tblAllPDIs = New DataTable()
            With m_tblAllPDIs.Columns
                .Add("KUNPDI", GetType(System.String))
                .Add("PDIAnzahl", GetType(System.String))
                .Add("PDIName", GetType(System.String))
            End With

            For Each row In m_tblFahrzeuge.Rows
                rowPDIs = m_tblAllPDIs.Select("KUNPDI='" & CType(row("KUNPDI"), String) & "'")
                If rowPDIs.Length = 0 Then  'PDI noch nicht gefunden
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

    Public Sub getMODs(ByRef strStatus As String)
        Dim row As DataRow
        Dim rowPdi As DataRow
        Dim rowPDIs As DataRow()
        Dim rowMODs As DataRow()
        Dim rowNew As DataRow

        strStatus = String.Empty

        If m_tblAllPDIs Is Nothing Then
            getPDIs(strStatus)
        End If

        Dim lngTimeStart As DateTime
        Dim lngTimeEnd As DateTime

        lngTimeStart = Now

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
                    rowPDIs = m_tblFahrzeuge.Select("KUNPDI='" & CType(row("KUNPDI"), String) & "'")

                    For Each rowPdi In rowPDIs      'PDIs durchgehen
                        rowMODs = m_tblAllMODs.Select("KUNPDI='" & CType(rowPdi("KUNPDI"), String) & "' AND ZZMODELL='" & CType(rowPdi("ZZMODELL"), String) & "'")
                        If rowMODs.Length = 0 Then          'Modell noch nicht gefunden
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

        lngTimeEnd = Now

    End Sub

    Public Function getSummen(ByVal tblZulassungen As DataTable, ByRef status As String) As DataTable
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
                rows = tblReturn.Select("Modell='" & CStr(row("ZZBEZEI")) & "'")
                If rows.Length = 0 Then     'Noch nicht gefunden, einfügen
                    rowNew = tblReturn.NewRow
                    rowNew("Modell") = CStr(row("ZZBEZEI"))
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


    Public Sub FahrzeugeSperren(ByVal tblFahrzeuge As DataTable, ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        Dim rows As DataRow()
        Dim i As Integer
      
        Try
            m_intStatus = 0
            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Warenkorb_Sperre_001", m_objApp, m_objUser, page)

            'Dim tblFahrzeugeSAP As DataTable = myProxy.getImportTable("GT_IN")
            Dim tblFahrzeugeSAP As DataTable = S.AP.GetImportTableWithInit("Z_M_Warenkorb_Sperre_001", "GT_IN")

            Dim tblFahrzeugeSAPRow As DataRow


            'Fahrzeuge übernehmen
            rows = tblFahrzeuge.Select("SelectedEinzel=True AND Art='CAR'")

            For i = 0 To rows.Length - 1
                tblFahrzeugeSAPRow = tblFahrzeugeSAP.NewRow
                tblFahrzeugeSAPRow("Equnr") = CType(rows(i)("EQUNR"), String)
                tblFahrzeugeSAP.Rows.Add(tblFahrzeugeSAPRow)
            Next

            'SAP-Aufruf
            'myProxy.callBapi()
            S.AP.Execute()

            WriteLogEntry(True, "KUNNR=" & m_strCustomer.TrimStart("0"c), tblFahrzeuge)
        Catch ex As Exception
            m_intStatus = -9999
            m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
            WriteLogEntry(False, "KUNNR=" & m_strCustomer.TrimStart("0"c) & m_strMessage, tblFahrzeuge)
        End Try
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

