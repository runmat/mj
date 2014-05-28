Option Explicit On 
Option Strict On

Imports System
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Configuration
Imports CKG.Base.Business
Imports CKG.Base.Common
Imports CKG.Base.Kernel

<Serializable()> Public Class FMS_1
    REM § Lese-/Schreibfunktion, Kunde: FMS, 
    REM § Show - BAPI: Z_M_ZULF_FZGE_ALD,
    REM § Change - BAPI: ?.

    Inherits BankBase ' FDD_Bank_Base


#Region " Declarations"

    Private configurationAppSettings As System.Configuration.AppSettingsReader

    Private m_strHaendlernummer As String
    Private m_strHalterNummer As String
    Private m_strStandortNummer As String
    Private m_strZielFirma As String
    Private m_strZielFirma2 As String
    Private m_strZielStrasse As String
    Private m_strZielHNr As String
    Private m_strZielPLZ As String
    Private m_strZielOrt As String
    Private m_strZielLand As String

    Private m_strAuf As String
    Private m_strBetreff As String

    Private m_blnFMSZulassung As Boolean

    Private m_strSucheFahrgestellNr As String
    Private m_strSucheKennzeichen As String
    Private m_strSucheLeasingvertragsNr As String
    Private m_kbanr As String
    Private m_zulkz As String
    Private m_Fahrzeuge As Int32
    Private m_tableGrund As DataTable
    Private m_versandadr_ZE As String
    Private m_versandadr_ZS As String
    Private m_versandadrtext As String
    Private m_versicherung As String
    Private m_material As String
    Private m_schein As String
    Private m_abckz As String
    Private m_equ As String
    Private m_kennz As String
    Private m_tidnr As String
    Private m_liznr As String
    Private m_versgrund As String
    Private m_versgrundText As String
    Private strAuftragsstatus As String
    Private strAuftragsnummer As String
    Private dataArray As ArrayList
    Private rowToChange As DataRow
    Private m_IsCustomer As Boolean
    Private m_strErrorMessage As String


    Private m_tblAdressen As DataTable

    <NonSerialized()> Private m_strAdressNummer As String
#End Region

#Region " Properties"
    Public ReadOnly Property AdressNummer() As String
        Get
            Return m_strAdressNummer
        End Get
    End Property

    Public Property Auf() As String
        Get
            Return m_strAuf
        End Get
        Set(ByVal Value As String)
            m_strAuf = Value
        End Set
    End Property

    Public Property IsBooCustomerGroup() As Boolean
        Get
            Return m_IsCustomer
        End Get
        Set(ByVal Value As Boolean)
            m_IsCustomer = Value
        End Set
    End Property


    Public Property Betreff() As String
        Get
            Return m_strBetreff
        End Get
        Set(ByVal Value As String)
            m_strBetreff = Value
        End Set
    End Property

    Public ReadOnly Property Adressen() As DataTable
        Get
            Return m_tblAdressen
        End Get
    End Property

    Public Property Versicherung() As String
        Get
            Return m_versicherung
        End Get
        Set(ByVal Value As String)
            m_versicherung = Value
        End Set
    End Property

    Public Property VersandGrundText() As String
        Get
            Return m_versgrundText
        End Get
        Set(ByVal Value As String)
            m_versgrundText = Value
        End Set
    End Property

    Public Property rowChange() As DataRow
        Get
            Return rowToChange
        End Get
        Set(ByVal Value As DataRow)
            rowToChange = Value
        End Set
    End Property

    Public Property objData() As ArrayList
        Get
            Return dataArray
        End Get
        Set(ByVal Value As ArrayList)
            dataArray = Value
        End Set
    End Property

    Public Property Auftragsstatus() As String
        Get
            Return strAuftragsstatus
        End Get
        Set(ByVal Value As String)
            strAuftragsstatus = Value
        End Set
    End Property

    Public Property Auftragsnummer() As String
        Get
            Return strAuftragsnummer
        End Get
        Set(ByVal Value As String)
            strAuftragsnummer = Value
        End Set
    End Property

    Public Property VersandGrund() As String
        Get
            Return m_versgrund
        End Get
        Set(ByVal Value As String)
            m_versgrund = Value
        End Set
    End Property

    Public Property LizenzNr() As String
        Get
            Return m_liznr
        End Get
        Set(ByVal Value As String)
            m_liznr = Value
        End Set
    End Property

    Public Property TIDNr() As String
        Get
            Return m_tidnr
        End Get
        Set(ByVal Value As String)
            m_tidnr = Value
        End Set
    End Property

    Public Property Kennzeichen() As String
        Get
            Return m_kennz
        End Get
        Set(ByVal Value As String)
            m_kennz = Value
        End Set
    End Property

    Public Property Equimpent() As String
        Get
            Return m_equ
        End Get
        Set(ByVal Value As String)
            m_equ = Value
        End Set
    End Property

    Public Property Versandart() As String
        Get
            Return m_abckz
        End Get
        Set(ByVal Value As String)
            m_abckz = Value
        End Set
    End Property

    Public Property ScheinSchildernummer() As String
        Get
            Return m_schein
        End Get
        Set(ByVal Value As String)
            m_schein = Value
        End Set
    End Property

    Public Property Materialnummer() As String
        Get
            Return m_material
        End Get
        Set(ByVal Value As String)
            m_material = Value
        End Set
    End Property

    Public Property VersandAdresse_ZE() As String
        Get
            Return m_versandadr_ZE
        End Get
        Set(ByVal Value As String)
            m_versandadr_ZE = Value
        End Set
    End Property

    Public Property VersandAdresse_ZS() As String
        Get
            Return m_versandadr_ZS
        End Get
        Set(ByVal Value As String)
            m_versandadr_ZS = Value
        End Set
    End Property

    Public Property VersandAdresseText() As String
        Get
            Return m_versandadrtext
        End Get
        Set(ByVal Value As String)
            m_versandadrtext = Value
        End Set
    End Property

    Public Property Fahrzeuge() As DataTable
        Get
            Return m_tblResult
        End Get
        Set(ByVal Value As DataTable)
            m_tblResult = Value
        End Set
    End Property
    Public ReadOnly Property GrundTabelle() As DataTable
        Get
            Return m_tableGrund
        End Get
    End Property

    Public Property SucheKennzeichen() As String
        Get
            Return m_strSucheKennzeichen
        End Get
        Set(ByVal Value As String)
            m_strSucheKennzeichen = Value
        End Set
    End Property

    Public Property SucheLeasingvertragsNr() As String
        Get
            Return m_strSucheLeasingvertragsNr
        End Get
        Set(ByVal Value As String)
            m_strSucheLeasingvertragsNr = Value
        End Set
    End Property

    Public Property SucheFahrgestellNr() As String
        Get
            Return m_strSucheFahrgestellNr
        End Get
        Set(ByVal Value As String)
            m_strSucheFahrgestellNr = Value
        End Set
    End Property

    Public Property FMSZulassung() As Boolean
        Get
            Return m_blnFMSZulassung
        End Get
        Set(ByVal Value As Boolean)
            m_blnFMSZulassung = Value
        End Set
    End Property

    Public Property ZielLand() As String
        Get
            Return m_strZielLand
        End Get
        Set(ByVal Value As String)
            m_strZielLand = Value
        End Set
    End Property

    Public Property ZielOrt() As String
        Get
            Return m_strZielOrt
        End Get
        Set(ByVal Value As String)
            m_strZielOrt = Value
        End Set
    End Property

    Public Property ZielPLZ() As String
        Get
            Return m_strZielPLZ
        End Get
        Set(ByVal Value As String)
            m_strZielPLZ = Value
        End Set
    End Property

    Public Property ZielHNr() As String
        Get
            Return m_strZielHNr
        End Get
        Set(ByVal Value As String)
            m_strZielHNr = Value
        End Set
    End Property

    Public Property ZielStrasse() As String
        Get
            Return m_strZielStrasse
        End Get
        Set(ByVal Value As String)
            m_strZielStrasse = Value
        End Set
    End Property

    Public Property ZielFirma2() As String
        Get
            Return m_strZielFirma2
        End Get
        Set(ByVal Value As String)
            m_strZielFirma2 = Value
        End Set
    End Property

    Public Property ZielFirma() As String
        Get
            Return m_strZielFirma
        End Get
        Set(ByVal Value As String)
            m_strZielFirma = Value
        End Set
    End Property

    Public Property Haendlernummer() As String
        Get
            Return m_strHaendlernummer
        End Get
        Set(ByVal Value As String)
            m_strHaendlernummer = Value
        End Set
    End Property

    Public Property HalterNummer() As String
        Get
            Return m_strHalterNummer
        End Get
        Set(ByVal Value As String)
            m_strHalterNummer = Value
        End Set
    End Property

    Public Property StandortNummer() As String
        Get
            Return m_strStandortNummer
        End Get
        Set(ByVal Value As String)
            m_strStandortNummer = Value
        End Set
    End Property
#End Region

#Region " Methods"

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
        m_blnFMSZulassung = False

    End Sub

    Public Overrides Sub Show()

    End Sub

    Public Overrides Sub Change()

    End Sub

    Public Function ErfasseBrief(ByVal strLeasingNummer As String, ByVal strFhgstNummer As String, ByVal strPZ As String, ByVal strHaendlerNummer As String) As Boolean
        Dim rowNew As DataRow
        Dim blnReturn As Boolean = False
        Dim blnTest As Boolean = False

        'Test, ob schon in Kollection
        If m_tblResult Is Nothing Then
            'Noch leer, d.h. auf keinen Fall doppelt
            blnTest = True
        Else
            Dim rowsTemp As DataRow() = m_tblResult.Select("FhgstNummer='" & strFhgstNummer & "'")
            If rowsTemp.GetLength(0) = 0 Then
                'Nicht gefunden, d.h. auch o.k.
                blnTest = True
            End If
        End If

        If blnTest Then
            '&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
            ' Hier SAP-Aufruf
            '&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
            If strLeasingNummer = "4711" Then
                ' Mißerfolg -> Fehlermeldung
                m_strMessage = "Fehler bei der Datenerfassung (" & "Bla" & ")"
            Else
                ' Erfolg -> Übernahme in Kollektion
                NewResultTable()
                rowNew = m_tblResult.NewRow
                rowNew("Ausgewaehlt") = True
                rowNew("Kennzeichenserie") = False
                rowNew("Vorreserviert") = False
                rowNew("HaendlerNummer") = strHaendlerNummer
                rowNew("LeasingNummer") = strLeasingNummer
                rowNew("FhgstNummer") = strFhgstNummer
                rowNew("DatumZulassung") = SuggestionDay()
                m_tblResult.Rows.Add(rowNew)
                blnReturn = True
            End If
        Else
            m_strMessage = "Fehler: Doppelerfassung"
        End If

        Return blnReturn
    End Function

    Private Function SuggestionDay() As DateTime
        Dim datTemp As DateTime = Now
        Dim intAddDays As Int32 = 0
        Do While datTemp.DayOfWeek = DayOfWeek.Saturday Or datTemp.DayOfWeek = DayOfWeek.Sunday Or intAddDays < 3
            datTemp = datTemp.AddDays(1)
            If Not (datTemp.DayOfWeek = DayOfWeek.Saturday Or datTemp.DayOfWeek = DayOfWeek.Sunday) Then
                intAddDays += 1
            End If
        Loop
        Return datTemp
    End Function

    Public Sub ClearResultTable()
        m_tblResult = Nothing
    End Sub

    Private Sub NewResultTable()
        If m_tblResult Is Nothing Then
            m_tblResult = New DataTable()
            With m_tblResult.Columns
                .Add("Ausgewaehlt", System.Type.GetType("System.Boolean"))
                .Add("HaendlerNummer", System.Type.GetType("System.String"))
                .Add("EquipmentNummer", System.Type.GetType("System.String"))
                .Add("LeasingNummer", System.Type.GetType("System.String"))
                .Add("FhgstNummer", System.Type.GetType("System.String"))
                .Add("DatumBriefeingang", System.Type.GetType("System.DateTime"))
                .Add("DatumZulassung", System.Type.GetType("System.DateTime"))
                .Add("Wuk01_Buchstaben", System.Type.GetType("System.String"))
                .Add("Wuk01_Ziffern", System.Type.GetType("System.String"))
                .Add("Wuk02_Buchstaben", System.Type.GetType("System.String"))
                .Add("Wuk02_Ziffern", System.Type.GetType("System.String"))
                .Add("Wuk03_Buchstaben", System.Type.GetType("System.String"))
                .Add("Wuk03_Ziffern", System.Type.GetType("System.String"))
                .Add("Kennzeichenserie", System.Type.GetType("System.Boolean"))
                .Add("Vorreserviert", System.Type.GetType("System.Boolean"))
                .Add("Reservierungsdaten", System.Type.GetType("System.String"))
                .Add("Ergebnis", System.Type.GetType("System.String"))
                .Add("Halter", System.Type.GetType("System.String")) 'Name+PLZ+Ort als Anzeige im WEB
                .Add("HalterKunnr", System.Type.GetType("System.String"))
                .Add("HalterName1", System.Type.GetType("System.String"))
                .Add("HalterName2", System.Type.GetType("System.String"))
                .Add("HalterStr", System.Type.GetType("System.String"))
                .Add("HalterNr", System.Type.GetType("System.String"))
                .Add("HalterPlz", System.Type.GetType("System.String"))
                .Add("HalterOrt", System.Type.GetType("System.String"))

                .Add("Standort", System.Type.GetType("System.String")) ''Name+PLZ+Ort als Anzeige im WEB
                .Add("StandortKunnr", System.Type.GetType("System.String"))
                .Add("StandortName1", System.Type.GetType("System.String"))
                .Add("StandortName2", System.Type.GetType("System.String"))
                .Add("StandortStr", System.Type.GetType("System.String"))
                .Add("StandortNr", System.Type.GetType("System.String"))
                .Add("StandortPlz", System.Type.GetType("System.String"))
                .Add("StandortOrt", System.Type.GetType("System.String"))

                .Add("Haendler", System.Type.GetType("System.String"))
                .Add("HaendlerKunnr", System.Type.GetType("System.String"))
                .Add("HaendlerName1", System.Type.GetType("System.String"))
                .Add("HaendlerName2", System.Type.GetType("System.String"))
                .Add("HaendlerStr", System.Type.GetType("System.String"))
                .Add("HaendlerNr", System.Type.GetType("System.String"))
                .Add("HaendlerPlz", System.Type.GetType("System.String"))
                .Add("HaendlerOrt", System.Type.GetType("System.String"))

                .Add("Wunschkennzeichen", System.Type.GetType("System.String"))
                .Add("Status", System.Type.GetType("System.String")) 'Für Statusmeldung nach Speichern
                .Add("FUnterlagen", System.Type.GetType("System.String")) 'Für Statusmeldung Zulassungsunterlagen vollständig
                .Add("Zulstelle", System.Type.GetType("System.String"))
                .Add("Versandadresse", System.Type.GetType("System.String"))    'Für Abweichende Versandadresse
            End With
        End If
    End Sub

    Public Sub getZulStelle(ByRef page As Web.UI.Page, ByVal plz As String, ByVal ort As String, ByRef status As String)

        Try
            S.AP.Init("Z_Get_Zulst_By_Plz")

            S.AP.SetImportParameter("I_PLZ", plz)
            S.AP.SetImportParameter("I_ORT", ort)

            S.AP.Execute()

            Dim table As DataTable = S.AP.GetExportTable("T_ZULST")

            If (table.Rows.Count > 1) Then
                'Mehr als ein Eintrag gefunden! Darf nicht sein!
                status = "PLZ nicht eindeutig. Mehrere Treffer gefunden."
            End If

            m_kbanr = table.Rows(0)("KBANR").ToString
            m_zulkz = table.Rows(0)("ZKFZKZ").ToString

        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message).Replace("Execution failed", "").Trim()

                Case "ERR_INV_PLZ"
                    m_strMessage = "Ungültige Postleitzahl."
                    m_intStatus = -1118
                Case Else
                    m_strMessage = "Unbekannter Fehler."
                    m_intStatus = -9999
            End Select
            status = m_strMessage
            m_kbanr = ""
            m_zulkz = ""
        End Try
    End Sub

    Public Function CheckAgainstAuthorizationTable(ByVal strEQUIPMENT As String) As Boolean
        Dim tableHide As New DataTable()
        Dim status As String = ""
        Dim rowResult As DataRow()
        Dim blnReturn As Boolean = False

        Try
            readAllAuthorizationSets(tableHide, status) 'Daten, die zur Autorisierung gespeichert wurden nicht anzeigen!

            rowResult = tableHide.Select("EQUIPMENT = '" & strEQUIPMENT & "'")
            If Not (rowResult.Length = 0) Then
                blnReturn = True
            End If
        Catch ex As Exception

        End Try
        Return blnReturn
    End Function

    Public Sub GiveCars()
        Dim tableHide As New DataTable()
        Dim status As String = ""
        Dim row As DataRow
        Dim rowResult As DataRow()
        Dim strRefBWFuhrpark As String

        configurationAppSettings = New System.Configuration.AppSettingsReader()

        readAllAuthorizationSets(tableHide, status) 'Daten, die zur Autorisierung gespeichert wurden nicht anzeigen!

        If Not m_blnGestartet Then
            m_blnGestartet = True

            m_intIDSAP = -1

            Try
                If m_objUser.Groups(0).IsCustomerGroup = True Then
                    IsBooCustomerGroup = True

                    strRefBWFuhrpark = CType(configurationAppSettings.GetValue("RefBWFuhrpark", GetType(System.String)), String)
                    'BAPI für Fuhrparkbestand

                    S.AP.Init("Z_M_Unangefordert_Bwfuhr")

                    S.AP.SetImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
                    S.AP.SetImportParameter("I_KUNNR_ZF", Right("0000000000" & strRefBWFuhrpark, 10))
                    S.AP.SetImportParameter("I_LICENSE_NUM", m_strSucheKennzeichen)
                    S.AP.SetImportParameter("I_LIZNR", m_strSucheLeasingvertragsNr)
                    S.AP.SetImportParameter("I_CHASSIS_NUM", m_strSucheFahrgestellNr)

                Else
                    'Original
                    S.AP.Init("Z_M_Unangefordert_Aldbp")

                    S.AP.SetImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
                    S.AP.SetImportParameter("I_LICENSE_NUM", m_strSucheKennzeichen)
                    S.AP.SetImportParameter("I_LIZNR", m_strSucheLeasingvertragsNr)
                    S.AP.SetImportParameter("I_CHASSIS_NUM", m_strSucheFahrgestellNr)

                End If

                S.AP.Execute()

                m_tableGrund = S.AP.GetExportTable("GT_GRU")
                m_tblResult = S.AP.GetExportTable("GT_WEB")
                m_tblResult.Columns.Add("STATUS", System.Type.GetType("System.String"))
                m_tblResult.Columns.Add("SWITCH", System.Type.GetType("System.Boolean"))
                m_intStatus = 0

                'Zur Autorisierung gespeicherte Daten entfernen!
                If tableHide.Rows.Count > 0 Then
                    For Each row In tableHide.Rows
                        rowResult = m_tblResult.Select("EQUNR = '" & row("EQUIPMENT").ToString & "'")
                        If Not (rowResult.Length = 0) Then
                            m_tblResult.Rows.Remove(rowResult(0))
                        End If
                    Next
                End If

                For Each row In m_tblResult.Rows
                    row("STATUS") = String.Empty
                    row("SWITCH") = False
                Next

                If (m_tblResult Is Nothing) OrElse (m_tblResult.Rows.Count = 0) Then
                    m_intStatus = -3331
                    m_strMessage = "Zu den gewählten Kriterien wurden keine Ergebnisse gefunden."
                End If

            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message).Replace("Execution failed", "").Trim()
                    Case "NO_DATA"
                        m_intStatus = -3331
                        m_strMessage = "Zu den gewählten Kriterien wurden keine Ergebnisse gefunden."
                    Case "NO_HAENDLER"
                        m_intStatus = -3332
                        m_strMessage = "Keine oder falsche Haendlernummer."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Sub getZulassungsdienste(ByVal page As System.Web.UI.Page, ByRef tblSTVA As DataTable, ByRef status As String)

        Try
            S.AP.InitExecute("Z_Get_Zulst_By_Plz")

            tblSTVA = S.AP.GetExportTable("T_ZULST")

        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message).Replace("Execution failed", "").Trim()
                Case "ERR_INV_PLZ"
                    m_strMessage = "Ungültige Postleitzahl."
                    m_intStatus = -1118
                Case Else
                    m_strMessage = "Unbekannter Fehler."
                    m_intStatus = -9999
            End Select
            status = m_strMessage

        End Try
    End Sub

    Public Sub AnfordernSAP()
        If Not m_blnGestartet Then
            m_blnGestartet = True

            m_intIDSAP = -1

            Try
                m_intStatus = 0
                m_strMessage = ""

                m_strZielLand = RemoveSingleSpace(m_strZielLand)
                If m_strZielLand = "" Then
                    m_strZielLand = "DE"
                End If

                S.AP.Init("Z_M_Briefanforderung_Fms")

                S.AP.SetImportParameter("I_KUNNR_AG", KUNNR)
                S.AP.SetImportParameter("I_KUNNR_ZF", RemoveSingleSpace(m_strHaendlernummer))
                S.AP.SetImportParameter("I_KUNNR_ZH", "")
                S.AP.SetImportParameter("I_KUNNR_ZE", RemoveSingleSpace(m_versandadr_ZE))
                S.AP.SetImportParameter("I_KUNNR_ZS", RemoveSingleSpace(m_versandadr_ZS))
                S.AP.SetImportParameter("I_ABCKZ", m_abckz)
                S.AP.SetImportParameter("I_EQUNR", RemoveSingleSpace(m_equ))
                S.AP.SetImportParameter("I_ERNAM", m_objUser.UserName)
                S.AP.SetImportParameter("I_CHASSIS_NUM", RemoveSingleSpace(m_strSucheFahrgestellNr))
                S.AP.SetImportParameter("I_LICENSE_NUM", RemoveSingleSpace(m_kennz))
                S.AP.SetImportParameter("I_TIDNR", RemoveSingleSpace(m_tidnr))
                S.AP.SetImportParameter("I_LIZNR", RemoveSingleSpace(m_liznr))
                S.AP.SetImportParameter("I_MATNR", Right("000000000000000000" & m_material, 18))
                S.AP.SetImportParameter("I_ZZVGRUND", RemoveSingleSpace(m_versgrund))
                S.AP.SetImportParameter("I_NAME1", RemoveSingleSpace(m_strZielFirma))
                S.AP.SetImportParameter("I_NAME2", RemoveSingleSpace(m_strZielFirma2))
                S.AP.SetImportParameter("I_PSTLZ", RemoveSingleSpace(m_strZielPLZ))
                S.AP.SetImportParameter("I_ORT01", RemoveSingleSpace(m_strZielOrt))
                S.AP.SetImportParameter("I_STR01", RemoveSingleSpace(m_strZielStrasse))
                S.AP.SetImportParameter("I_HOUSE", RemoveSingleSpace(m_strZielHNr))
                S.AP.SetImportParameter("I_LAND1", RemoveSingleSpace(m_strZielLand))
                S.AP.SetImportParameter("I_ZZBETREFF", RemoveSingleSpace(m_strBetreff))
                S.AP.SetImportParameter("I_ZZNAME_ZH", RemoveSingleSpace(m_strAuf))

                S.AP.Execute()

                strAuftragsnummer = S.AP.GetExportParameter("E_VBELN").TrimStart("0"c)
                strAuftragsstatus = S.AP.GetExportParameter("E_FEHLTXT")

                If strAuftragsstatus.Length = 0 Then
                    strAuftragsstatus = "Vorgang OK"
                End If

                If (Not m_abckz = "3") And (strAuftragsnummer.Length = 0) Then
                    m_intStatus = -2100
                    m_strMessage = "Ihre Anforderung konnte im System nicht erstellt werden."
                End If
            Catch ex As Exception
                m_strMessage = "Ihre Anforderung konnte im System nicht erstellt werden."
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message).Replace("Execution failed", "").Trim()
                    Case "ZCREDITCONTROL_ENTRY_LOCKED"
                        strAuftragsstatus = "System ausgelastet. Bitte clicken Sie erneut auf ""Absenden""."
                        m_intStatus = -1111
                    Case "NO_UPDATE_EQUI"
                        strAuftragsstatus = "Fehler bei der Datenspeicherung (EQUI-UPDATE)"
                        m_intStatus = -1112
                    Case "NO_AUFTRAG"
                        strAuftragsstatus = "Kein Auftrag angelegt"
                        m_intStatus = -1113
                    Case "NO_ZDADVERSAND"
                        strAuftragsstatus = "Keine Einträge für die Versanddatei erstellt"
                        m_intStatus = -1114
                    Case "NO_MODIFY_ILOA"
                        strAuftragsstatus = "ILOA-MODIFY-Fehler"
                        m_intStatus = -1115
                    Case "NO_BRIEFANFORDERUNG"
                        strAuftragsstatus = "Brief bereits angefordert"
                        m_intStatus = -1116
                    Case "NO_EQUZ"
                        strAuftragsstatus = "Kein Brief vorhanden (EQUZ)"
                        m_intStatus = -1117
                    Case "NO_ILOA"
                        strAuftragsstatus = "Kein Brief vorhanden (ILOA)"
                        m_intStatus = -1118
                    Case "NO_INSERT_ADRESSE"
                        strAuftragsstatus = "Adresse kann nicht verwendet werden."
                        m_intStatus = -1119
                    Case Else
                        strAuftragsstatus = ex.Message
                        m_intStatus = -9999
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Private Function RemoveSingleSpace(ByVal strIn As String) As String
        Dim strReturn As String = ""
        Try
            Dim strOut As String = strIn.Trim(" "c)
            If strOut = "&nbsp;" Then
                strOut = ""
            End If
            strReturn = strOut
        Catch
        End Try
        Return strReturn
    End Function

    Private Sub addResultTableHeader()
        If (m_tblResult Is Nothing) Then
            m_tblResult = New DataTable()
        End If
        With m_tblResult.Columns

            .Add("id")
            .Add("Erstellt")
            .Add("Benutzer")
            .Add("m_strHaendlernummer")
            .Add("m_strHalterNummer")
            .Add("m_strStandortNummer")
            .Add("m_strZielFirma")
            .Add("m_strZielFirma2")
            .Add("m_strZielStrasse")
            .Add("m_strZielHNr")
            .Add("m_strZielPLZ")
            .Add("m_strZielOrt")
            .Add("m_strZielLand")
            .Add("m_strAuf")
            .Add("m_strBetreff")
            .Add("m_blnFMSZulassung")
            .Add("m_strSucheFahrgestellNr")
            .Add("m_strSucheKennzeichen")
            .Add("m_strSucheLeasingvertragsNr")
            .Add("m_kbanr")
            .Add("m_zulkz")
            .Add("m_Fahrzeuge")
            .Add("m_versandadr_ZE")
            .Add("m_versandadr_ZS")
            .Add("m_versandadrtext")
            .Add("m_versicherung")
            .Add("m_material")
            .Add("m_schein")
            .Add("m_abckz")
            .Add("m_equ")
            .Add("m_kennz")
            .Add("m_tidnr")
            .Add("m_liznr")
            .Add("m_versgrund")
            .Add("m_versgrundText")
            .Add("VersandartShow")
            .Add("Status")
        End With

    End Sub

    Private Sub addResultTableRow(ByVal id As String, ByVal tstamp As String, ByVal user As String, ByVal objData As ArrayList)
        Dim row As DataRow

        row = m_tblResult.NewRow
        row("id") = id
        row("Erstellt") = tstamp
        row("Benutzer") = user
        If objData(0) Is Nothing Then
            row("m_strHaendlernummer") = ""
        Else
            row("m_strHaendlernummer") = objData(0)
        End If
        If objData(1) Is Nothing Then
            row("m_strHalterNummer") = ""
        Else
            row("m_strHalterNummer") = objData(1)
        End If
        If objData(2) Is Nothing Then
            row("m_strStandortNummer") = ""
        Else
            row("m_strStandortNummer") = objData(2)
        End If
        If objData(3) Is Nothing Then
            row("m_strZielFirma") = ""
        Else
            row("m_strZielFirma") = objData(3)
        End If
        If objData(4) Is Nothing Then
            row("m_strZielFirma2") = ""
        Else
            row("m_strZielFirma2") = objData(4)
        End If
        If objData(5) Is Nothing Then
            row("m_strZielStrasse") = ""
        Else
            row("m_strZielStrasse") = objData(5)
        End If
        If objData(6) Is Nothing Then
            row("m_strZielHNr") = ""
        Else
            row("m_strZielHNr") = objData(6)
        End If
        If objData(7) Is Nothing Then
            row("m_strZielPLZ") = ""
        Else
            row("m_strZielPLZ") = objData(7)
        End If
        If objData(8) Is Nothing Then
            row("m_strZielOrt") = ""
        Else
            row("m_strZielOrt") = objData(8)
        End If
        If objData(9) Is Nothing Then
            row("m_strZielLand") = ""
        Else
            row("m_strZielLand") = objData(9)
        End If
        If objData(10) Is Nothing Then
            row("m_strAuf") = ""
        Else
            row("m_strAuf") = objData(10)
        End If



        If objData(11) Is Nothing Then
            row("m_strBetreff") = ""
        Else
            row("m_strBetreff") = objData(11)
        End If
        If objData(12) Is Nothing Then
            row("m_blnFMSZulassung") = ""
        Else
            row("m_blnFMSZulassung") = objData(12)
        End If
        If objData(13) Is Nothing Then
            row("m_strSucheFahrgestellNr") = ""
        Else
            row("m_strSucheFahrgestellNr") = objData(13)
        End If
        If objData(14) Is Nothing Then
            row("m_strSucheKennzeichen") = ""
        Else
            row("m_strSucheKennzeichen") = objData(14)
        End If
        If objData(15) Is Nothing Then
            row("m_strSucheLeasingvertragsNr") = ""
        Else
            row("m_strSucheLeasingvertragsNr") = objData(15)
        End If
        If objData(16) Is Nothing Then
            row("m_kbanr") = ""
        Else
            row("m_kbanr") = objData(16)
        End If
        If objData(17) Is Nothing Then
            row("m_zulkz") = ""
        Else
            row("m_zulkz") = objData(17)
        End If
        If objData(18) Is Nothing Then
            row("m_Fahrzeuge") = ""
        Else
            row("m_Fahrzeuge") = objData(18)
        End If
        If objData(19) Is Nothing Then
            row("m_versandadr_ZE") = ""
        Else
            row("m_versandadr_ZE") = objData(19)
        End If
        If objData(20) Is Nothing Then
            row("m_versandadr_ZS") = ""
        Else
            row("m_versandadr_ZS") = objData(20)
        End If
        If objData(21) Is Nothing Then
            row("m_versandadrtext") = ""
        Else
            row("m_versandadrtext") = objData(21)
        End If
        If objData(22) Is Nothing Then
            row("m_versicherung") = ""
        Else
            row("m_versicherung") = objData(22)
        End If
        If objData(23) Is Nothing Then
            row("m_material") = "1391"
        Else
            row("m_material") = objData(23)
        End If
        Select Case CStr(row("m_material"))
            Case "1391"
                row("VersandartShow") = "innerhalb von 24 bis 48 h"
            Case "1385"
                row("VersandartShow") = "vor 9:00 Uhr"
            Case "1389"
                row("VersandartShow") = "vor 10:00 Uhr"
            Case "1390"
                row("VersandartShow") = "vor 12:00 Uhr"
        End Select
        If objData(24) Is Nothing Then
            row("m_schein") = ""
        Else
            row("m_schein") = objData(25)
        End If
        If objData(25) Is Nothing Then
            row("m_abckz") = ""
        Else
            row("m_abckz") = objData(25)
        End If
        If objData(26) Is Nothing Then
            row("m_equ") = ""
        Else
            row("m_equ") = objData(26)
        End If
        If objData(27) Is Nothing Then
            row("m_kennz") = ""
        Else
            row("m_kennz") = objData(27)
        End If
        If objData(28) Is Nothing Then
            row("m_tidnr") = ""
        Else
            row("m_tidnr") = objData(28)
        End If
        If objData(29) Is Nothing Then
            row("m_liznr") = ""
        Else
            row("m_liznr") = objData(29)
        End If
        If objData(30) Is Nothing Then
            row("m_versgrund") = ""
        Else
            row("m_versgrund") = objData(30)
        End If
        If objData(31) Is Nothing Then
            row("m_versgrundText") = ""
        Else
            row("m_versgrundText") = objData(31)
        End If
        m_tblResult.Rows.Add(row)
        m_tblResult.AcceptChanges()
    End Sub

    Public Sub getAutorizationData(ByRef status As String)
        Dim formatter As System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
        Dim cn As New SqlClient.SqlConnection
        Dim cmdOutPut As SqlClient.SqlCommand
        Dim objData As ArrayList
        Dim ms As New MemoryStream()
        Dim drAppData As SqlClient.SqlDataReader
        Dim intI As Int32 = 4

        addResultTableHeader()

        Try
            cn.ConnectionString = ConfigurationManager.AppSettings("Connectionstring")
            cn.Open()

            cmdOutPut = New SqlClient.SqlCommand()
            cmdOutPut.Connection = cn
            cmdOutPut.CommandText = "SELECT * FROM vwAuthorizationFMS WHERE UserID = " & m_objUser.UserID.ToString & " AND NOT username like '" & m_objUser.UserName & "'"
            drAppData = cmdOutPut.ExecuteReader

            While drAppData.Read()
                If Not drAppData Is Nothing Then
                    If Not TypeOf drAppData(intI) Is System.DBNull Then
                        ' 1. Daten als bytearray aus der DB lesen:
                        Dim bytData(CInt(drAppData.GetBytes(intI, 0, Nothing, 0, Integer.MaxValue - 1) - 1)) As Byte
                        drAppData.GetBytes(intI, 0, bytData, 0, bytData.Length)
                        ' Dataset über einen Memory Stream aus dem ByteArray erzeugen:
                        Dim stmAppData As MemoryStream
                        stmAppData = New MemoryStream(bytData)
                        ms = stmAppData
                        formatter = New BinaryFormatter()
                        objData = New ArrayList()
                        objData = DirectCast(formatter.Deserialize(ms), ArrayList)
                        addResultTableRow(CType(drAppData("id"), String), CType(drAppData("tstamp"), String), CType(drAppData("username"), String), objData)
                    Else
                        ms = Nothing
                    End If
                Else
                    ms = Nothing
                End If
            End While
        Catch ex As Exception
            Throw ex
        Finally
            cn.Close()
            cn.Dispose()
        End Try
    End Sub

    Private Sub writedb(ByVal username As String, ByVal equipment As String, ByVal objectData As MemoryStream, ByRef status As String)
        Dim connection As New SqlClient.SqlConnection
        Dim para As SqlClient.SqlParameter

        Dim command As New SqlClient.SqlCommand()
        Dim sqlInsert As String
        Dim b As Byte()

        Try
            connection.ConnectionString = ConfigurationManager.AppSettings("Connectionstring")

            sqlInsert = "INSERT INTO AuthorizationFMS (username,equipment, data) VALUES (@user,@equi,@data)"

            With command
                .Connection = connection
                .CommandType = CommandType.Text
                .CommandText = sqlInsert
                .Parameters.Clear()
            End With

            b = objectData.ToArray
            para = New SqlClient.SqlParameter("@data", SqlDbType.Image, b.Length, ParameterDirection.Input, False, 0, 0, Nothing, DataRowVersion.Current, b)
            With command.Parameters
                .AddWithValue("@user", username)
                .AddWithValue("@equi", equipment)
                .Add(para)
            End With
            connection.Open()
            command.ExecuteNonQuery()
            status = String.Empty
        Catch ex As Exception
            status = ex.Message
        Finally
            connection.Close()
            connection.Dispose()
        End Try
    End Sub

    Private Sub anfordernSQL()
        Dim formatter As System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
        Dim ms As MemoryStream
        dataArray = New ArrayList()

        With dataArray
            .Add(m_strHaendlernummer)               ' 0
            .Add(m_strHalterNummer)                 ' 1
            .Add(m_strStandortNummer)               ' 2
            .Add(m_strZielFirma)                    ' 3
            .Add(m_strZielFirma2)                   ' 4
            .Add(m_strZielStrasse)                  ' 5
            .Add(m_strZielHNr)                      ' 6
            .Add(m_strZielPLZ)                      ' 7
            .Add(m_strZielOrt)                      ' 8
            .Add(m_strZielLand)                     ' 9
            .Add(m_strAuf)                          '10
            .Add(m_strBetreff)                      '11
            .Add(m_blnFMSZulassung)                 '12
            .Add(m_strSucheFahrgestellNr)           '13
            .Add(m_strSucheKennzeichen)             '14
            .Add(m_strSucheLeasingvertragsNr)       '15
            .Add(m_kbanr)                           '16
            .Add(m_zulkz)                           '17
            .Add(m_Fahrzeuge)                       '18
            .Add(m_versandadr_ZE)                   '19
            .Add(m_versandadr_ZS)                   '20
            .Add(m_versandadrtext)                  '21
            .Add(m_versicherung)                    '22
            .Add(m_material)                        '23
            .Add(m_schein)                          '24
            .Add(m_abckz)                           '25
            .Add(m_equ)                             '26
            .Add(m_kennz)                           '27
            .Add(m_tidnr)                           '28
            .Add(m_liznr)                           '29
            .Add(m_versgrund)                       '30
            .Add(m_versgrundText)                   '31
        End With

        ms = New MemoryStream()
        formatter = New BinaryFormatter()
        formatter.Serialize(ms, dataArray)

        writedb(m_objUser.UserName, m_equ, ms, strAuftragsstatus)
        If (strAuftragsstatus = String.Empty) Then
            strAuftragsstatus = "Auftrag zur Freigabe gespeichert."
            strAuftragsnummer = "OK"
        Else
            strAuftragsnummer = ""
        End If
    End Sub

    Public Sub clearDB(ByVal id As Int32, ByRef status As String)
        Dim connection As New SqlClient.SqlConnection
        Dim command As New SqlClient.SqlCommand()
        Dim sqlInsert As String

        Try
            connection.ConnectionString = ConfigurationManager.AppSettings("Connectionstring")

            sqlInsert = "DELETE FROM AuthorizationFMS WHERE id = @id"

            With command
                .Connection = connection
                .CommandType = CommandType.Text
                .CommandText = sqlInsert
                .Parameters.Clear()
            End With

            With command.Parameters
                .AddWithValue("@id", id)
            End With
            connection.Open()
            command.ExecuteScalar()
            status = String.Empty
        Catch ex As Exception
            status = ex.Message
        Finally
            connection.Close()
            connection.Dispose()
        End Try
    End Sub

    Public Sub readAllAuthorizationSets(ByRef resultTable As DataTable, ByRef status As String)
        Dim connection As New SqlClient.SqlConnection
        Dim command As New SqlClient.SqlCommand()
        Dim adapter As New SqlClient.SqlDataAdapter()
        Dim sqlInsert As String

        Try
            connection.ConnectionString = ConfigurationManager.AppSettings("Connectionstring")
            sqlInsert = "SELECT * FROM AuthorizationFMS"

            With command
                .Connection = connection
                .CommandType = CommandType.Text
                .CommandText = sqlInsert
                .Parameters.Clear()
            End With

            connection.Open()
            adapter.SelectCommand = command
            adapter.Fill(resultTable)

            status = String.Empty
        Catch ex As Exception
            status = ex.Message
        Finally
            connection.Close()
            connection.Dispose()
        End Try
    End Sub

    Public Sub Anfordern(ByVal admin As Boolean)
        If admin Then
            AnfordernSAP()
        Else
            anfordernSQL()
        End If
    End Sub

    Public Function LeseAdressenSAP(ByRef page As Web.UI.Page, ByVal strParentNode As String, ByVal strName As String, ByVal strPLZ As String, Optional ByVal adressart As String = "A", Optional ByVal nodelevel As String = "99") As Int32

        Try

            m_tblAdressen = New DataTable()
            m_tblAdressen.Columns.Add("ADDRESSNUMBER", System.Type.GetType("System.String"))
            m_tblAdressen.Columns.Add("DISPLAY_ADDRESS", System.Type.GetType("System.String"))

            m_tblAdressen.Columns.Add("POSTL_CODE", System.Type.GetType("System.String"))
            m_tblAdressen.Columns.Add("STREET", System.Type.GetType("System.String"))
            m_tblAdressen.Columns.Add("COUNTRYISO", System.Type.GetType("System.String"))
            m_tblAdressen.Columns.Add("CITY", System.Type.GetType("System.String"))
            m_tblAdressen.Columns.Add("NAME", System.Type.GetType("System.String"))
            m_tblAdressen.Columns.Add("NAME_2", System.Type.GetType("System.String"))

            S.AP.Init("Z_M_Customer_Get_Children_Fms")

            S.AP.SetImportParameter("VALID_ON", Today.ToShortDateString)
            S.AP.SetImportParameter("NODE_LEVEL", nodelevel)
            S.AP.SetImportParameter("CUSTOMERNO", Right("0000000000" & strParentNode, 10))
            S.AP.SetImportParameter("I_NAME1", strName)
            S.AP.SetImportParameter("I_PSTLZ", strPLZ)
            S.AP.SetImportParameter("CUSTHITYP", adressart)

            S.AP.Execute()

            Dim tblTemp As DataTable = S.AP.GetExportTable("NODE_LIST")

            Dim SAPTableRow As DataRow

            Dim newDealerDetailRow As DataRow

            For Each SAPTableRow In tblTemp.Rows

                S.AP.InitExecute("Bapi_Customer_Getdetail2", "CUSTOMERNO", SAPTableRow("CUSTOMER").ToString)

                Dim SAPBapiRet1 As DataTable = S.AP.GetExportTable("RETURN")
                Dim SAPCustomerAdress As DataTable = S.AP.GetExportTable("CUSTOMERADDRESS")
                Dim SAPCustomerDetail As DataTable = S.AP.GetExportTable("CUSTOMERGENERALDETAIL")

                If SAPBapiRet1.Rows(0)("TYPE").ToString.Trim(" "c) = "" Or SAPBapiRet1.Rows(0)("TYPE").ToString = "S" Or SAPBapiRet1.Rows(0)("TYPE").ToString = "I" Then

                    newDealerDetailRow = m_tblAdressen.NewRow

                    Dim strTemp As String = SAPCustomerAdress.Rows(0)("Name").ToString
                    If SAPCustomerAdress.Rows(0)("Name_2").ToString.Length > 0 Then
                        strTemp &= ", " & SAPCustomerAdress.Rows(0)("Name_2").ToString
                    End If
                    If SAPCustomerAdress.Rows(0)("Name_3").ToString.Length > 0 Then
                        strTemp &= ", " & SAPCustomerAdress.Rows(0)("Name_3").ToString
                    End If
                    If SAPCustomerAdress.Rows(0)("Name_4").ToString.Length > 0 Then
                        strTemp &= ", " & SAPCustomerAdress.Rows(0)("Name_4").ToString
                    End If

                    newDealerDetailRow("DISPLAY_ADDRESS") = strTemp & ", " & SAPCustomerAdress.Rows(0)("Countryiso").ToString & " - " & SAPCustomerAdress.Rows(0)("Postl_Code").ToString & " " & SAPCustomerAdress.Rows(0)("City").ToString & ", " & SAPCustomerAdress.Rows(0)("Street").ToString
                    newDealerDetailRow("ADDRESSNUMBER") = SAPCustomerDetail.Rows(0)("Customer").ToString
                    If SAPCustomerAdress.Rows(0)("Postl_Code").ToString.Length > 0 Then
                        newDealerDetailRow("POSTL_CODE") = SAPCustomerAdress.Rows(0)("Postl_Code").ToString
                    End If
                    If SAPCustomerAdress.Rows(0)("Street").ToString.Length > 0 Then
                        newDealerDetailRow("STREET") = SAPCustomerAdress.Rows(0)("Street").ToString
                    End If
                    If SAPCustomerAdress.Rows(0)("Countryiso").ToString.Length > 0 Then
                        newDealerDetailRow("COUNTRYISO") = SAPCustomerAdress.Rows(0)("Countryiso").ToString
                    End If
                    If SAPCustomerAdress.Rows(0)("CITY").ToString.Length > 0 Then
                        newDealerDetailRow("CITY") = SAPCustomerAdress.Rows(0)("CITY").ToString
                    End If
                    If SAPCustomerAdress.Rows(0)("Name").ToString.Length > 0 Then
                        newDealerDetailRow("NAME") = SAPCustomerAdress.Rows(0)("Name").ToString
                    End If
                    If SAPCustomerAdress.Rows(0)("Name_2").ToString.Length > 0 Then
                        newDealerDetailRow("NAME_2") = SAPCustomerAdress.Rows(0)("Name_2").ToString
                    End If

                    m_tblAdressen.Rows.Add(newDealerDetailRow)

                End If

            Next

            Return m_tblAdressen.Rows.Count

        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case Else
                    m_strErrorMessage = "Es ist ein Fehler aufgetreten: " & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    m_intStatus = -3
            End Select
        End Try

    End Function

    Public Function GiveResultStructure() As DataTable

        S.AP.Init("Z_M_UNANGEFORDERT_BWFUHR")

        Dim tblTemp As DataTable = S.AP.GetImportTable("GT_WEB")

        tblTemp.Columns.Add("STATUS", System.Type.GetType("System.String"))
        tblTemp.Columns.Add("SWITCH", System.Type.GetType("System.Boolean"))

        Return tblTemp


    End Function

#End Region

End Class

' ************************************************
' $History: FMS_1.vb $
' 
' *****************  Version 6  *****************
' User: Fassbenders  Date: 11.03.10   Time: 14:17
' Updated in $/CKAG/Applications/appfms/Lib
' ITA: 2918
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 5.03.10    Time: 10:02
' Updated in $/CKAG/Applications/appfms/Lib
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 4.03.10    Time: 14:24
' Updated in $/CKAG/Applications/appfms/Lib
' ITA: 2918
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 4.03.10    Time: 10:03
' Updated in $/CKAG/Applications/appfms/Lib
' ITA: 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 30.04.09   Time: 15:58
' Updated in $/CKAG/Applications/appfms/Lib
' ITA 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:44
' Created in $/CKAG/Applications/appfms/Lib
' 
' *****************  Version 14  *****************
' User: Rudolpho     Date: 6.12.07    Time: 13:57
' Updated in $/CKG/Applications/AppFMS/AppFMSWeb/Lib
' ITA 1440
' 
' *****************  Version 13  *****************
' User: Uha          Date: 26.09.07   Time: 18:05
' Updated in $/CKG/Applications/AppFMS/AppFMSWeb/Lib
' BAPI Z_M_LAND_PLZ_001 hinzugefügt und BAPI Z_M_BRIEFANFORDERUNG_FMS
' erweitert.
' 
' *****************  Version 12  *****************
' User: Uha          Date: 2.07.07    Time: 17:52
' Updated in $/CKG/Applications/AppFMS/AppFMSWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 11  *****************
' User: Uha          Date: 7.03.07    Time: 13:11
' Updated in $/CKG/Applications/AppFMS/AppFMSWeb/Lib
' 
' ******************************************
