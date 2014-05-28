Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports System.Data.SqlClient

Public Class Change_01
    REM § Lese-/Schreibfunktion, Kunde: SIXT, 
    REM § New - BAPI: Z_M_Meldungen_Pdi,
    REM § Show - BAPI: Z_M_Meldungen_Fahrzeuge,
    REM § ShowDatenABE - BAPI: Z_M_Abezufzg,
    REM § SelectCars - BAPI: ,
    REM § Change - BAPI: Z_M_Einzelzulassung, Z_M_Zulassungssperre, Z_M_Pdiwechsel.

    Inherits Base.Business.BankBase  'FDD_Bank_Base

#Region " Declarations"
    Private m_intErrorCount As Int32
    Private m_objABE_Daten As Base.Business.ABE2FHZG
    Private m_dsPDI_Data As DataSet
    Private m_dsPDI_Data_Selected As DataSet
    Private m_tblPDIs As DataTable
    Private m_tblAllPDIs As DataTable
    Private m_tblModelle As DataTable
    Private m_tblFahrzeuge As DataTable
    Private m_tblFahrzeugeText As DataTable
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
    Private returnList As ArrayList
    Private m_versicherer As Admin.VersichererList
    Private m_halter As Admin.HalterList
    Private m_tblVerwendung As DataTable
    Private gridMain As DataGrid
    Private m_strPDISuche As String
#End Region
    'Dim returnTable As New SAPProxy_SIXT_01.BAPIRET2Table() 'Nur Temporär.
    'Dim returnValue As String = "Hallo"
#Region " Properties"
    Public Property PPDISuche() As String
        Get
            Return m_strPDISuche
        End Get
        Set(ByVal Value As String)
            m_strPDISuche = Value
        End Set
    End Property

    Public ReadOnly Property PTexte() As DataTable
        Get
            Return m_tblFahrzeugeText
        End Get
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

    Public ReadOnly Property PHalter() As DataTable
        Get
            Return m_halter
        End Get
    End Property

    Public ReadOnly Property PVersicherer() As DataTable
        Get
            Return m_versicherer
        End Get
    End Property

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
        getVersicherer()
        getHalter()
        'getVerwendung(strAppID, strSessionID)
    End Sub

    Public Sub getVersicherer()
        Dim dtVersicherer As Admin.VersichererList
        Dim cn As New SqlClient.SqlConnection(m_objUser.App.Connectionstring)

        m_intStatus = 0

        Try
            cn.Open()
            m_versicherer = New Admin.VersichererList("%", "%", cn, CInt(m_objUser.KUNNR))
        Catch ex As Exception
            m_intStatus = -1
        Finally
            cn.Close()
        End Try

    End Sub

    Public Sub getHalter()
        Dim dtHalter As Admin.HalterList
        Dim cn As New SqlClient.SqlConnection(m_objUser.App.Connectionstring)

        m_intStatus = 0

        Try
            cn.Open()
            m_halter = New Admin.HalterList("%", "%", CInt(m_objUser.KUNNR), cn)

        Catch ex As Exception
            m_intStatus = -1
        Finally
            cn.Close()
        End Try

    End Sub


    Public Sub getVerwendung(ByVal strAppID As String, ByVal strSessionID As String)
        'Dim intID As Int32
        'm_strClassAndMethod = "Change_01.getVerwendung"
        'm_strAppID = strAppID
        'm_strSessionID = strSessionID
        'If Not m_blnGestartet Then
        '    m_blnGestartet = True

        '    MakeDestination()
        '    Dim objSAP As New SAPProxy_STRAUB.SAPProxy_STRAUB()
        '    objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
        '    objSAP.Connection.Open()


        '    If m_objLogApp Is Nothing Then
        '        m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
        '    End If

        '    intID = -1

        '    Try
        '        Dim SAPTableReturnHersteller As New SAPProxy_STRAUB.ZDAD_M_EC_AVM_HERSTELLERTable()
        '        Dim SAPTableReturnVerwendung As New SAPProxy_STRAUB.ZDAD_M_EC_AVM_VERWENDUNGTable()
        '        Dim SAPTableReturnModell As New SAPProxy_STRAUB.ZMODELIDTable()

        '        'SAP-Logeintrag (initialisieren)
        '        intID = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_EC_AVM_HERST_VWZWECK", strAppID, strSessionID)
        '        'BAPI-Aufruf
        '        'objSAP.Z_M_Ec_Avm_Herst_Vwzweck_Modid(SAPTableReturnHersteller, SAPTableReturnModell, SAPTableReturnVerwendung)
        '        'objSAP.Z_M_Ec_Avm_Herst_Vwzweck_Modid(SAPTableReturnHersteller, SAPTableReturnModell, SAPTableReturnVerwendung)
        '        objSAP.CommitWork()
        '        'SAP-Logeintrag (füllen)
        '        If intID > -1 Then
        '            m_objlogApp.WriteEndDataAccessSAP(intID, True)
        '        End If

        '        'Tabellen formatieren
        '        'tblHersteller = SAPTableReturnHersteller.ToADODataTable
        '        m_tblVerwendung = SAPTableReturnVerwendung.ToADODataTable
        '        'tblModell = SAPTableReturnModell.ToADODataTable

        '        'Report-Logeintrag (ok)
        '        WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblVerwendung)
        '    Catch ex As Exception
        '        Select Case ex.Message
        '            Case "NO_DATA"
        '                m_intStatus = -1234
        '                m_strMessage = "Fehler: Keine Daten gefunden!"
        '            Case Else
        '                m_intStatus = -9999
        '                m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        '        End Select
        '        'tblHersteller = Nothing
        '        m_tblVerwendung = Nothing
        '        'Report-Logeintrag (Fehler)
        '        If intID > -1 Then
        '            m_objlogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
        '        End If
        '        WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult)
        '    Finally
        '        If intID > -1 Then
        '            m_objlogApp.WriteStandardDataAccessSAP(intID)
        '        End If

        '        objSAP.Connection.Close()
        '        objSAP.Dispose()

        '        m_blnGestartet = False
        '    End Try
        'End If
    End Sub

    Public Sub setDisposition(ByVal strAppID As String, ByVal strSessionID As String)
        Dim intID As Int32
        Dim rowsZulassung As DataRow()
        Dim row As DataRow
        Dim strZuldatum As String
        Dim strKBANR As String
        Dim strKunnr As String
        Dim strHalter As String
        Dim strVersicherer As String
        Dim strPDI As String
        Dim strFahrgestell As String
        Dim strSonder As String
        'Rückgabeparameter (Zulassung)
        Dim strEquipment As String
        Dim strDatBem As String
        Dim strQmNr As String
        Dim intResult As Integer
        Dim strVBELN As String

        m_strClassAndMethod = "Change_01.setDisposition"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            MakeDestination()
            Dim objSAP As New SAPProxy_Base.SAPProxy_Base()
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If

            intID = -1

            Try
                Dim tblSAP As New SAPProxy_Base.TLINETable() '.ZDAD_M_WEB_MESSAGE_DATEN_SIXTTable() ' .ZDAD_M_EC_AVM_HERSTELLERTable()
                Dim tblRowSAP As SAPProxy_Base.TLINE
                Dim intCount As Integer

                'Tabellenzeile identifizieren
                rowsZulassung = Result.Select("SelectedEinzel=True AND Art='CAR'")

                'Alle FAhrzeuge durchgehen.....
                For intCount = 0 To rowsZulassung.Length - 1
                    'Inputarameter füllen ......
                    strKunnr = Right("0000000000" & m_objUser.KUNNR, 10)
                    strPDI = CStr(rowsZulassung(intCount)("DADPDI"))
                    strDatBem = HelpProcedures.MakeDateSAP(CStr(rowsZulassung(intCount)("DatumBemerkung")))
                    strQmNr = CStr(rowsZulassung(intCount)("QMNUM"))

                    'Bemerkungstexte
                    tblRowSAP = New SAPProxy_Base.TLINE()
                    tblRowSAP.Tdline = CStr(rowsZulassung(intCount)("Bemerkung"))
                    tblSAP.Clear()
                    tblSAP.Add(tblRowSAP)

                    Try
                        'BAPI-Aufruf
                        intID = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Pdiwechsel", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)

                        objSAP.Z_M_Pdiwechsel("X", "", "", strDatBem, strPDI, strKunnr, strQmNr, tblSAP)
                        objSAP.CommitWork()

                        If intID > -1 Then
                            m_objlogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                        End If
                        m_strMessage = "Vorgang OK."
                        rowsZulassung(intCount)("Status") = m_strMessage
                    Catch ex As Exception
                        'Fehler...
                        Select Case ex.Message
                            Case "ERR_INV_KUNNR"
                                m_intStatus = -1234
                                m_strMessage = "Fehler! Ungültige Kundennummer."
                            Case "ERR_INV_QMNUM"
                                m_intStatus = -1235
                                m_strMessage = "Fehler! Ungültige Meldungsnummer."
                            Case "ERR_NOT_SAVED"
                                m_intStatus = -1236
                                m_strMessage = "Fehler! Daten nicht gespeichert."
                            Case "ERR_INV_CARPORT"
                                m_intStatus = -1236
                                m_strMessage = "Fehler! Ungültiger Carport."
                            Case "SCHON_ZUGELASSEN"
                                m_intStatus = -1236
                                m_strMessage = "Fehler! Fahrzeug bereits zugelassen."
                            Case "VERSCHOBEN"
                                m_intStatus = -1236
                                m_strMessage = "Fehler! Fahrzeug verschoben."
                            Case "GESPERRT"
                                m_intStatus = -1236
                                m_strMessage = "Fehler! Fahrzeug gesperrt."
                            Case Else
                                m_intStatus = -9999
                                m_strMessage = "Fehler! Unbekannter Fehler."
                        End Select
                        rowsZulassung(intCount)("Status") = m_strMessage
                        WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult)
                    End Try

                Next
            Catch ex As Exception
                m_intStatus = -9999
                m_strMessage = "Unbekannter Fehler."

                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult)
            Finally
                objSAP.Connection.Close()
                objSAP.Dispose()

                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Sub setZulassung(ByVal strAppID As String, ByVal strSessionID As String)
        Dim intID As Int32
        Dim rowsZulassung As DataRow()
        Dim row As DataRow
        Dim strZuldatum As String
        Dim strKBANR As String
        Dim strKunnr As String
        Dim strHalter As String
        Dim strVersicherer As String
        Dim strPDI As String
        Dim strFahrgestell As String
        Dim strSonder As String
        'Rückgabeparameter (Zulassung)
        Dim strEquipment As String
        Dim strKennzeichen As String
        Dim strQmNr As String
        Dim intResult As Integer
        Dim strVBELN As String

        m_strClassAndMethod = "Change_01.setZulassung"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            MakeDestination()
            Dim objSAP As New SAPProxy_STRAUB.SAPProxy_STRAUB()
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If

            intID = -1

            Try
                Dim tblReturnSAP As New SAPProxy_STRAUB.ZDAD_M_WEB_MESSAGE_DATEN_SIXTTable() ' .ZDAD_M_EC_AVM_HERSTELLERTable()
                Dim intCount As Integer

                'Tabellenzeile identifizieren
                rowsZulassung = Result.Select("SelectedEinzel=True AND Art='CAR'")

                'Alle FAhrzeuge durchgehen.....
                For intCount = 0 To rowsZulassung.Length - 1
                    'Inputarameter füllen ......
                    strZuldatum = CStr(rowsZulassung(intCount)("SelectedDate"))
                    strKBANR = CStr(PHalter.Rows(0)("KBANR"))                                   'Achtung! Hier ist z.Z. nur ein Eintrag in der SQL-Tabelle vorgesehen
                    strHalter = Right("0000000000" & CStr(PHalter.Rows(0)("SAP-Nr")), 10)
                    strKunnr = Right("0000000000" & m_objUser.KUNNR, 10)

                    row = PVersicherer.Select("VersichererID=" & CStr(rowsZulassung(intCount)("SelectedVersicherung")))(0)
                    strVersicherer = Right("0000000000" & CStr(row("SAP-Nr")), 10)

                    strPDI = CStr(rowsZulassung(intCount)("DADPDI"))
                    strFahrgestell = CStr(rowsZulassung(intCount)("ZZFAHRG"))
                    strSonder = String.Empty

                    Try
                        'BAPI-Aufruf
                        intID = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Einzelzulassung", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)

                        objSAP.Z_M_Einzelzulassung(strZuldatum, strKBANR, strKunnr, "", strHalter, strVersicherer, strPDI, strFahrgestell, "", strSonder, strEquipment, strKennzeichen, strQmNr, intResult, strVBELN, tblReturnSAP)
                        objSAP.CommitWork()

                        If intID > -1 Then
                            m_objlogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                        End If

                        If intResult = 0 Then
                            'Ok...
                            rowsZulassung(intCount)("Status") = "Vorgang OK.<br>" & strKennzeichen
                        Else
                            'Fehler...
                            Dim tblRetunrnADO As DataTable
                            Dim strMessage As String

                            tblRetunrnADO = tblReturnSAP.ToADODataTable
                            strMessage = "Fehler bei der Zulassung."

                            If Not (tblRetunrnADO Is Nothing) AndAlso (tblRetunrnADO.Rows.Count > 0) Then
                                strMessage &= ":<br>" & CStr(tblRetunrnADO.Rows(0)("Message"))
                            End If

                            rowsZulassung(intCount)("Status") = strMessage

                        End If
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
            Catch ex As Exception
                m_intStatus = -9999
                m_strMessage = "Unbekannter Fehler."

                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult)
            Finally
                objSAP.Connection.Close()
                objSAP.Dispose()

                m_blnGestartet = False
            End Try
        End If
    End Sub


    Public Sub getCars()
        'Liest alle Fahrzeuge un gruppiert sie nach PDI und Modell.
        m_strClassAndMethod = "Change_01.getCars()"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            m_intErrorCount = 0
            m_intLastID = -1

            'Create start data
            Dim objSAP As New SAPProxy_STRAUB.SAPProxy_STRAUB()

            Dim tblFahrzeugeSAP As New SAPProxy_STRAUB.ZDAD_M_WEB_REPORTING_SIXTTable()
            Dim tblFahrzeugMeldungenSAP As New SAPProxy_STRAUB.ZDAD_M_WEB_MELDUNGSTEXTE_SIXTTable()

            MakeDestination()
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1

            Try
                m_intStatus = 0
                m_strMessage = ""
                m_intFahrzeugeGesamtZulassungsf = 0
                m_intFahrzeugeGesamtGesperrt = 0

                m_intIDsap = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Meldungen_Pdi1", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)
                objSAP.Z_M_Meldungen_Pdi1(Right("0000000000" & m_objUser.KUNNR, 10), "1", "1510", m_strPDISuche, tblFahrzeugMeldungenSAP, tblFahrzeugeSAP)
                objSAP.CommitWork()
                If m_intIDsap > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, True)
                End If

                Dim tblFahrzeuge As New DataTable()
                Dim tblTempAll As New DataTable()

                tblFahrzeuge = tblFahrzeugeSAP.ToADODataTable
                m_tblFahrzeugeText = tblFahrzeugMeldungenSAP.ToADODataTable     'Texte

                With tblFahrzeuge.Columns
                    .Add("Art", GetType(System.String))   'Gruppierungstyp: PDI, odell, Fahrzeug
                    .Add("Anzahl", GetType(System.Int32))
                    .Add("RowID", GetType(System.String))   'ID der Zeile
                    .Add("DatumBemerkung", GetType(System.String))
                    .Add("Bemerkung", GetType(System.String))

                    .Add("Zusatzdaten", GetType(System.String))           'Eingetragene Fahrzeuganzahl
                    .Add("SelectedDate", GetType(System.String))            'Eingetragenes Zulassungsdatum
                    .Add("SelectedVerwendung", GetType(System.String))        'Eingetragener Verwendungszweck
                    .Add("SelectedVersicherung", GetType(System.String))     'Eingetragener Versicherung
                    .Add("SelectedVersicherungText", GetType(System.String))     'Eingetragener Versicherung
                    .Add("SelectedAnzahl", GetType(System.Int16))           'Eingetragene Fahrzeuganzahl
                    .Add("PDIAnzahl", GetType(System.String))
                    .Add("MODAnzahl", GetType(System.String))
                    .Add("SelectedAlle", GetType(System.Int16))             'Werte übernehmen?
                    .Add("SelectedEinzel", GetType(System.Boolean))           'Eingetragene Fahrzeuganzahl
                    .Add("Status", GetType(System.String))             'Werte übernehmen?
                    .Add("Equipment", GetType(System.String))             'Werte übernehmen?
                End With

                Dim rowFahrzeug As DataRow
                Dim rowFahrzeuge As DataRow()
                Dim intPDI As Integer
                Dim rowPDIs As DataRow()
                Dim rowPDI As DataRow
                Dim rowModels As DataRow()
                Dim rowModel As DataRow
                Dim newRow As DataRow
                Dim newRowModel As DataRow
                Dim newRowCar As DataRow
                Dim anzahl As Int32
                Dim rowAutos As DataRow()
                Dim rowAuto As DataRow
                Dim rowBemerkung As DataRow()
                Dim strHelp As String

                Dim intID_PDI As Integer
                Dim intID_MODEL As Integer
                Dim intID_CAR As Integer = 0

                Dim colorTextArray() As String = {"weiss", "gelb", "orange", "rot", "violett", "blau", "grün", "grau", "braun", "schwarz"}
                Dim colorValueArray() As String = {"FFFFFF", "FFFF00", "FF8040", "FF0000", "FF00FF", "0000FF", "00FF00", "808080", "804000", "000000"}

                Dim btnDetail As LinkButton

                m_tblResult = tblFahrzeuge.Clone()

                intID_PDI = 0
                For Each rowFahrzeug In tblFahrzeuge.Rows
                    rowFahrzeuge = m_tblResult.Select("KUNPDI='" & CType(rowFahrzeug("KUNPDI"), String) & "'")

                    If (rowFahrzeuge.Length = 0) Then  'Kein Eintrag gefunden
                        '2. Kopfzeile (PDI)
                        newRow = m_tblResult.NewRow()

                        intID_PDI += 1
                        newRow("RowID") = "P" & Right("00000" & CStr(intID_PDI), 5)         'ID vergeben
                        'strHelp = Right("00000" & CStr(intID_PDI), 5)

                        newRow("KUNPDI") = CType(rowFahrzeug("KUNPDI"), String)
                        newRow("ZZBEZEI") = String.Empty
                        newRow("ZZMODELL") = String.Empty
                        newRow("ZZFAHRG") = String.Empty
                        newRow("ZZDAT_EIN") = String.Empty
                        newRow("ZZFARBE") = String.Empty
                        newRow("Art") = "PDI"

                        'newRow("DetailButton") = strHelp
                        newRow("Anzahl") = 1

                        newRow("SelectedDate") = String.Empty                       'Eingetragenes Zulassungsdatum
                        newRow("SelectedVerwendung") = String.Empty                 'Eingetragener Verwendungszweck
                        newRow("SelectedVersicherung") = String.Empty               'Eingetragener Versicherung
                        newRow("SelectedAnzahl") = 0                                'Eingentragene Fahrzeuganzahl
                        newRow("PDIAnzahl") = CType(rowFahrzeug("KUNPDI"), String)
                        newRow("MODAnzahl") = String.Empty
                        newRow("SelectedAlle") = False
                        newRow("SelectedEinzel") = False
                        newRow("Status") = String.Empty

                        m_tblResult.Rows.Add(newRow)
                        m_tblResult.AcceptChanges()

                        '2. Kopfzeile (Modell). Alle Modelle mit der gefundenen PDI suchen
                        'Alle Zeilen mit der gefundenen PDI holen...
                        rowPDIs = tblFahrzeuge.Select("KUNPDI='" & CType(newRow("KUNPDI"), String) & "'")

                        intID_MODEL = 0
                        For Each rowPDI In rowPDIs
                            'Alle Modelle suchen...
                            rowModels = m_tblResult.Select("KUNPDI='" & CType(rowPDI("KUNPDI"), String) & "' AND ZZMODELL='" & CType(rowPDI("ZZMODELL"), String) & "'")
                            If rowModels.Length = 0 Then    'Noch kein Model gefunden
                                newRowModel = m_tblResult.NewRow()

                                intID_MODEL += 1
                                newRowModel("RowID") = "M" & Right("00000" & CStr(intID_PDI), 5) & "." & Right("00000" & CStr(intID_MODEL), 5)             'ID vergeben
                                'strHelp = Right("00000" & CStr(intID_PDI), 5) & "." & Right("00000" & CStr(intID_MODEL), 5)              'ID vergeben

                                newRowModel("KUNPDI") = CType(rowPDI("KUNPDI"), String)
                                newRowModel("ZZBEZEI") = CType(rowPDI("ZZBEZEI"), String)
                                newRowModel("ZZMODELL") = CType(rowPDI("ZZMODELL"), String)
                                newRowModel("ZZFAHRG") = String.Empty
                                newRowModel("ZZDAT_EIN") = String.Empty
                                newRowModel("ZZFARBE") = String.Empty
                                newRowModel("Art") = "MOD"

                                'newRowModel("DetailButton") = strHelp

                                newRowModel("SelectedDate") = String.Empty                      'Eingetragenes Zulassungsdatum
                                newRowModel("SelectedVerwendung") = String.Empty                'Eingetragener Verwendungszweck
                                newRowModel("SelectedVersicherung") = String.Empty              'Eingetragener Versicherung
                                newRowModel("SelectedAnzahl") = 0                               'Eingentragene Fahrzeuganzahl
                                newRowModel("SelectedAlle") = False
                                newRowModel("PDIAnzahl") = String.Empty
                                newRowModel("MODAnzahl") = CType(rowPDI("ZZMODELL"), String)     'Ausgabe
                                newRowModel("SelectedEinzel") = False
                                newRowModel("Status") = String.Empty

                                m_tblResult.Rows.Add(newRowModel)

                                '3. Die Fahrzeuge, die zum PDI und Modell passen
                                rowAutos = tblFahrzeuge.Select("KUNPDI='" & CType(newRowModel("KUNPDI"), String) & "' AND ZZMODELL='" & CType(newRowModel("ZZMODELL"), String) & "'")

                                intID_CAR = 0
                                For Each rowAuto In rowAutos
                                    'Nur Fahrzeuge mit gefüllter ModelID berücksichtigen...
                                    'If ((Not (TypeOf rowAuto("ZZMODELL") Is System.DBNull)) OrElse (CStr(rowAuto("ZZMODELL")) <> String.Empty)) Then
                                    newRowCar = m_tblResult.NewRow()

                                    intID_CAR += 1
                                    newRowCar("RowID") = "C" & Right("00000" & CStr(intID_PDI), 5) & "." & Right("00000" & CStr(intID_MODEL), 5) & "." & Right("00000" & CStr(intID_CAR), 5)                'ID vergeben

                                    newRowCar("KUNPDI") = CType(rowAuto("KUNPDI"), String)
                                    newRowCar("DADPDI") = CType(rowAuto("DADPDI"), String)
                                    newRowCar("ZZBEZEI") = CType(rowAuto("ZZBEZEI"), String)
                                    newRowCar("ZZMODELL") = CType(rowAuto("ZZMODELL"), String)
                                    newRowCar("ZZFAHRG") = CType(rowAuto("ZZFAHRG"), String)
                                    newRowCar("ZZDAT_EIN") = HelpProcedures.MakeDateStandard(CType(rowAuto("ZZDAT_EIN"), String)).ToShortDateString
                                    newRowCar("QMNUM") = CType(rowAuto("QMNUM"), String)
                                    newRowCar("ZZDATBEM") = Left(CStr(HelpProcedures.MakeDateStandard(CType(rowAuto("ZZDATBEM"), String))), 10)
                                    If CStr(newRowCar("ZZDATBEM")) = "01.01.1900" Then
                                        newRowCar("ZZDATBEM") = String.Empty
                                    End If
                                    newRowCar("ZZFARBE") = "<a style=""color:#" & colorValueArray(CInt(rowAuto("ZZFARBE"))) & """>&#127;&#127;&#127</a>&nbsp;-&nbsp;" & colorTextArray(CInt(rowAuto("ZZFARBE")))

                                    newRowCar("Zusatzdaten") = CType(rowAuto("ZZAUSF"), String) & "-" & CType(rowAuto("ZZSIPP3"), String) & "-" & CType(rowAuto("ZZANTR"), String) & "-" & CType(rowAuto("ZZNAVI"), String)

                                    newRowCar("Art") = "CAR"
                                    newRowCar("Anzahl") = 1

                                    newRowCar("SelectedDate") = String.Empty                        'Eingetragenes Zulassungsdatum
                                    newRowCar("SelectedVerwendung") = String.Empty                  'Eingetragener Verwendungszweck
                                    newRowCar("SelectedVersicherung") = String.Empty                'Eingetragener Versicherung
                                    newRowCar("SelectedAnzahl") = 0                                 'Eingentragene Fahrzeuganzahl
                                    newRowCar("SelectedAlle") = False
                                    newRowCar("PDIAnzahl") = String.Empty
                                    newRowCar("MODAnzahl") = String.Empty
                                    newRowCar("SelectedEinzel") = False
                                    newRowCar("Status") = String.Empty
                                    newRowCar("DatumBemerkung") = CStr(newRowCar("ZZDATBEM"))
                                    newRowCar("Equipment") = CStr(rowAuto("EQUNR"))
                                    'Bemerkungen füllen
                                    rowBemerkung = PTexte.Select("QMNUM='" & CStr(newRowCar("QMNUM")) & "'")
                                    If (rowBemerkung.Length = 1) Then
                                        newRowCar("Bemerkung") = CStr(rowBemerkung(0)("TDLINE"))
                                    Else
                                        newRowCar("Bemerkung") = String.Empty
                                    End If
                                    m_tblResult.Rows.Add(newRowCar)
                                    m_tblResult.AcceptChanges()
                                    'End If
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
                m_intStatus = 0
                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblPDIs)

            Catch ex As Exception
                Select Case ex.Message
                    Case "NO_DATA"
                        m_intStatus = -1234
                        m_strMessage = "Es wurden keine Daten gefunden."
                    Case "NO_WEB"
                        m_intStatus = -1234
                        m_strMessage = "Keine Web-Tabelle erstellt."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message
                End Select
                If m_intIDsap > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, False, m_strMessage)
                End If
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", ZZCARPORT=" & m_strPDINummerSuche & " , " & Replace(m_strMessage, "<br>", " "), Nothing)
            Finally
                If m_intIDsap > -1 Then
                    m_objlogApp.WriteStandardDataAccessSAP(m_intIDsap)
                End If

                objSAP.Connection.Close()
                objSAP.Dispose()

                m_blnGestartet = False
            End Try

        End If
    End Sub


    Public Overrides Sub Show()
    End Sub
    Public Overrides Sub change()
    End Sub
#End Region
End Class

' ************************************************
' $History: Change_01.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 17:38
' Created in $/CKAG/Applications/AppSTRAUB/Lib
' 
' *****************  Version 23  *****************
' User: Uha          Date: 3.07.07    Time: 9:51
' Updated in $/CKG/Applications/AppSTRAUB/AppSTRAUBWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 22  *****************
' User: Uha          Date: 18.06.07   Time: 16:14
' Updated in $/CKG/Applications/AppSTRAUB/AppSTRAUBWeb/Lib
' Parameterliste von Z_M_Pdiwechsel laut Änderung in SAPProxy_Base
' angepasst
' 
' *****************  Version 21  *****************
' User: Uha          Date: 8.03.07    Time: 14:18
' Updated in $/CKG/Applications/AppSTRAUB/AppSTRAUBWeb/Lib
' 
' ************************************************
