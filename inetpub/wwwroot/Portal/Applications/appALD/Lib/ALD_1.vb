Option Explicit On 
Option Strict On

Imports System
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization
Imports System.Configuration
Imports CKG.Base.Kernel
Imports CKG.Base.Business

<Serializable()> Public Class ALD_1
    REM § Lese-/Schreibfunktion, Kunde: ALD, 
    REM § Show - BAPI: Z_M_ZULF_FZGE_ALD,
    REM § Change - BAPI: ?.

    Inherits BankBase ' FDD_Bank_Base

#Region " Declarations"
    Private m_strHaendlernummer As String = String.Empty
    Private m_strHalterNummer As String = String.Empty
    Private m_strStandortNummer As String = String.Empty
    Private m_strZielFirma As String = String.Empty
    Private m_strZielFirma2 As String = String.Empty
    Private m_strZielStrasse As String = String.Empty
    Private m_strZielHNr As String = String.Empty
    Private m_strZielPLZ As String = String.Empty
    Private m_strZielOrt As String = String.Empty
    Private m_strZielLand As String = String.Empty

    Private m_strAuf As String = String.Empty
    Private m_strBetreff As String = String.Empty

    Private m_blnALDZulassung As Boolean

    Private m_strSucheFahrgestellNr As String = String.Empty
    Private m_strSucheKennzeichen As String = String.Empty
    Private m_strSucheLeasingvertragsNr As String = String.Empty
    Private m_kbanr As String = String.Empty
    Private m_zulkz As String = String.Empty
    Private m_Fahrzeuge As Int32
    Private m_tableGrund As DataTable
    Private m_versandadr_ZE As String = String.Empty
    Private m_versandadr_ZS As String = String.Empty
    Private m_versandadrtext As String = String.Empty
    Private m_versicherung As String = String.Empty
    Private m_material As String = String.Empty
    Private m_schein As String = String.Empty
    Private m_abckz As String = String.Empty
    Private m_equ As String = String.Empty
    Private m_kennz As String = String.Empty
    Private m_tidnr As String = String.Empty
    Private m_liznr As String = String.Empty
    Private m_versgrund As String = String.Empty
    Private m_versgrundText As String = String.Empty
    Private strAuftragsstatus As String = String.Empty
    Private strAuftragsnummer As String = String.Empty
    Private dataArray As ArrayList
    Private rowToChange As DataRow

    Private m_tblAdressen As DataTable
#End Region

#Region " Properties"
    Public Property Auf() As String
        Get
            Return m_strAuf
        End Get
        Set(ByVal Value As String)
            m_strAuf = Value
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

    Public Property ALDZulassung() As Boolean
        Get
            Return m_blnALDZulassung
        End Get
        Set(ByVal Value As Boolean)
            m_blnALDZulassung = Value
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
        m_blnALDZulassung = False
        'If objUser.Reference = String.Empty Then
        '    m_blnALDZulassung = True
        'End If
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
            With m_tblresult.Columns
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
                '.Add("FUFlag", System.Type.GetType("System.String")) 'Für Flag, ob Zulassungsunterlagen vollständig
                .Add("FUnterlagen", System.Type.GetType("System.String")) 'Für Statusmeldung Zulassungsunterlagen vollständig
                .Add("Zulstelle", System.Type.GetType("System.String"))
                .Add("Versandadresse", System.Type.GetType("System.String"))    'Für Abweichende Versandadresse
            End With
        End If
    End Sub

    Public Sub getZulStelle(ByVal plz As String, ByVal ort As String, ByRef status As String)
        Dim objSAP As New SAPProxy_Base.SAPProxy_Base()
        Dim objPLZ As New SAPProxy_Base.ZPOSTLEITZAHLENTable()
        Dim objZul As New SAPProxy_Base.ZULSTTable()
        Dim table As New DataTable()

        Try
            'MakeDestination()
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If

            objSAP.Z_Get_Zulst_By_Plz(ort, plz, objPLZ, objZul)
            objSAP.CommitWork()

            table = objZul.ToADODataTable
            If (table.Rows.Count > 1) Then
                'Mehr als ein Eintrag gefunden! Bei ALD möglich?? 'TODO
                status = "PLZ nicht eindeutig. Mehrere Treffer gefunden."
            End If

            If m_intIDsap > -1 Then
                m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, True)
            End If

            m_kbanr = table.Rows(0)("KBANR").ToString
            m_zulkz = table.Rows(0)("ZKFZKZ").ToString
        Catch ex As Exception
            Select Case ex.Message
                Case "ERR_INV_PLZ"
                    m_strMessage = "Ungültige Postleitzahl."
                    m_intStatus = -1118
                Case Else
                    m_strMessage = "Unbekannter Fehler."
                    m_intStatus = -9999
            End Select
            status = m_strMessage
        Finally
            If m_intIDsap > -1 Then
                m_objLogApp.LogStandardIdentity = m_intStandardLogID
                m_objlogApp.WriteStandardDataAccessSAP(m_intIDsap)
            End If

            objSAP.Connection.Close()
            objSAP.Dispose()
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
        Dim tableGrund As New DataTable()
        Dim tableFahrzeuge As New DataTable()
        Dim tableHide As New DataTable()
        Dim status As String = ""
        Dim row As DataRow
        Dim rowResult As DataRow()

        readAllAuthorizationSets(tableHide, status) 'Daten, die zur Autorisierung gespeichert wurden nicht anzeigen!

        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_ALD.SAPProxy_ALD()
            Dim tblFahrzeuge As New SAPProxy_ALD.ZDAD_M_WEB_ALDTable()
            '§§§ JVE 17.01.2006 Tabellennam hat sich geändert. Wieso???
            'Dim tblFahrzeuge As New SAPProxy_ALD.ZDAD_M_WEB_EQUIDATENTable()
            Dim tblGrund As New SAPProxy_ALD.ZVERSAND_GRUNDTable()
            '----------------------------------------------------------------
            MakeDestination()
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1

            Dim intID As Int32 = -1

            Try

                intID = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Unangefordert_Aldbp", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                objSAP.Z_M_Unangefordert_Aldbp(m_strSucheFahrgestellNr, "0000000000", m_strSucheKennzeichen, m_strSucheLeasingvertragsNr, tblGrund, tblFahrzeuge)
                ' Right("0000000000" & m_objUser.KUNNR, 10)
                objSAP.CommitWork()
                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, True)
                End If
                m_tableGrund = New DataTable()
                m_tableGrund = tblGrund.ToADODataTable
                m_tblResult = tblFahrzeuge.ToADODataTable
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
                    'Abmeldedatum formatieren
                    Try
                        row("EXPIRY_DATE") = Left(CStr(MakeDateStandard(CStr(row("EXPIRY_DATE")).TrimStart("0"c))), 10)

                        If CStr(row("EXPIRY_DATE")) = "01.01.1900" Then
                            row("EXPIRY_DATE") = "XX.XX.XXXX"
                        End If

                        If CStr(row("EXPIRY_DATE")) = String.Empty Then
                            row("EXPIRY_DATE") = "XX.XX.XXXX"
                        End If

                    Catch ex As Exception
                        row("EXPIRY_DATE") = "XX.XX.XXXX"
                    End Try
                Next

                If (m_tblResult Is Nothing) OrElse (m_tblResult.Rows.Count = 0) Then
                    m_intStatus = -3331
                    m_strMessage = "Zu den gewählten Kriterien wurden keine Ergebnisse gefunden."
                End If

                WriteLogEntry(True, "FahrgestellNr=" & m_strSucheFahrgestellNr & ", LVNr.=" & m_strSucheLeasingvertragsNr & ", KfzKz.=" & m_strSucheKennzeichen & ", KUNNR=" & m_objUser.KUNNR, m_tblResult)

            Catch ex As Exception
                Select Case ex.Message
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
                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "FahrgestellNr=" & m_strSucheFahrgestellNr & ", LVNr.=" & m_strSucheLeasingvertragsNr & ", KfzKz.=" & m_strSucheKennzeichen & ", KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult)
            Finally
                If intID > -1 Then
                    m_objlogApp.WriteStandardDataAccessSAP(intID)
                End If

                objSAP.Connection.Close()
                objSAP.Dispose()

                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Sub getZulassungsdienste(ByRef tblSTVA As DataTable, ByRef status As String)

        Dim objSAP As New SAPProxy_Base.SAPProxy_Base() ' SAPProxy_FFD_HEZ.SAPProxy_FFD_HEZ()
        Dim objPLZ As New SAPProxy_Base.ZPOSTLEITZAHLENTable()
        Dim objZul As New SAPProxy_Base.ZULSTTable()

        Try
            'MakeDestination()
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If

            objSAP.Z_Get_Zulst_By_Plz("", "", objPLZ, objZul)
            objSAP.CommitWork()

            tblSTVA = objZul.ToADODataTable
            'tblOrte = objPLZ.ToADODataTable

            If m_intIDsap > -1 Then
                m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, True)
            End If

        Catch ex As Exception
            Select Case ex.Message
                Case "ERR_INV_PLZ"
                    m_strMessage = "Ungültige Postleitzahl."
                    m_intStatus = -1118
                Case Else
                    m_strMessage = "Unbekannter Fehler."
                    m_intStatus = -9999
            End Select
            status = m_strMessage
        Finally
            If m_intIDsap > -1 Then
                m_objLogApp.LogStandardIdentity = m_intStandardLogID
                m_objlogApp.WriteStandardDataAccessSAP(m_intIDsap)
            End If

            objSAP.Connection.Close()
            objSAP.Dispose()
        End Try
    End Sub

    Public Sub AnfordernSAP()
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_ALD.SAPProxy_ALD()

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

                m_intIDsap = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Briefanforderung_Aldbp", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                m_strZielLand = RemoveSingleSpace(m_strZielLand)
                If m_strZielLand = "" Then
                    m_strZielLand = "DE"
                End If

                Dim strDaten As String = "abckz=" & m_abckz & _
                    ", strSucheFahrgestellNr=" & m_strSucheFahrgestellNr & _
                    ", equ=" & m_equ & _
                    ", strZielHNr=" & m_strZielHNr & _
                    ", KUNNR=" & KUNNR & _
                    ", versandadr_ZE=" & m_versandadr_ZE & _
                    ", versandadr_ZS=" & m_versandadr_ZS & _
                    ", strZielLand=" & m_strZielLand & _
                    ", kennz=" & m_kennz & _
                    ", liznr=" & m_liznr & _
                    ", material=" & m_material & _
                    ", strZielFirma=" & m_strZielFirma & _
                    ", strZielFirma2=" & m_strZielFirma2 & _
                    ", strZielOrt=" & m_strZielOrt & _
                    ", strZielPLZ=" & m_strZielPLZ & _
                    ", strZielStrasse=" & m_strZielStrasse & _
                    ", tidnr=" & m_tidnr & _
                    ", versgrund=" & m_versgrund & _
                    ", strAuf=" & m_strAuf & _
                    ", strBetreff=" & m_strBetreff

                objSAP.Z_M_Briefanforderung_Aldbp(m_abckz, RemoveSingleSpace(m_strSucheFahrgestellNr), RemoveSingleSpace(m_equ), m_objUser.UserName, RemoveSingleSpace(m_strZielHNr), KUNNR, RemoveSingleSpace(m_versandadr_ZE), RemoveSingleSpace(m_strHaendlernummer), "", RemoveSingleSpace(m_versandadr_ZS), RemoveSingleSpace(m_strZielLand), RemoveSingleSpace(m_kennz), RemoveSingleSpace(m_liznr), Right("000000000000000000" & m_material, 18), RemoveSingleSpace(m_strZielFirma), RemoveSingleSpace(m_strZielFirma2), RemoveSingleSpace(m_strZielOrt), RemoveSingleSpace(m_strZielPLZ), RemoveSingleSpace(m_strZielStrasse), RemoveSingleSpace(m_tidnr), RemoveSingleSpace(m_strBetreff), "", RemoveSingleSpace(m_strAuf), RemoveSingleSpace(m_versgrund), strAuftragsstatus, strAuftragsnummer)
                objSAP.CommitWork()

                If m_intIDsap > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, True)
                End If
                strAuftragsnummer = strAuftragsnummer.TrimStart("0"c)
                strAuftragsstatus = "Vorgang OK"

                If (Not m_abckz = "3") And (strAuftragsnummer.Length = 0) Then
                    m_intStatus = -2100
                    m_strMessage = "Ihre Anforderung konnte im System nicht erstellt werden."
                    strAuftragsstatus = "Keine Auftragsnummer erzeugt."
                    If m_intIDsap > -1 Then
                        m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, False, Left(m_strMessage & ", " & strDaten, 500))
                    End If
                Else
                    If m_intIDsap > -1 Then
                        m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, True, Left(strDaten, 500))
                    End If
                End If
            Catch ex As Exception
                m_strMessage = "Ihre Anforderung konnte im System nicht erstellt werden."
                Select Case ex.Message
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
                If m_intIDsap > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, False, m_strMessage)
                End If
            Finally
                If m_intIDsap > -1 Then
                    m_objLogApp.LogStandardIdentity = m_intStandardLogID
                    m_objlogApp.WriteStandardDataAccessSAP(m_intIDsap)
                End If

                objSAP.Connection.Close()
                objSAP.Dispose()

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
            '.Add("id")
            '.Add("Erstellt")
            '.Add("Benutzer")
            '.Add("Equipment")
            '.Add("Fahrgestellnr")
            '.Add("Versandadresse")
            '.Add("VersandadresseName1")
            '.Add("VersandadresseName2")
            '.Add("VersandadresseStr")
            '.Add("VersandadresseNr")
            '.Add("VersandadressePlz")
            '.Add("VersandadresseOrt")
            '.Add("Lvnr")
            '.Add("Haendlernummer")
            '.Add("Versandart")
            '.Add("Kennzeichen")
            '.Add("TIDNr")
            '.Add("LIZNr")
            '.Add("Materialnummer")
            '.Add("VersandartShow")
            '.Add("Status")

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
            .Add("m_blnALDZulassung")
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
            row("m_blnALDZulassung") = ""
        Else
            row("m_blnALDZulassung") = objData(12)
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
        Dim cn As SqlClient.SqlConnection
        Dim cmdOutPut As SqlClient.SqlCommand
        Dim objData As ArrayList
        Dim ms As New MemoryStream()
        Dim drAppData As SqlClient.SqlDataReader
        Dim intI As Int32 = 4

        addResultTableHeader()
        cn = New SqlClient.SqlConnection
        Try
            cn.ConnectionString = ConfigurationManager.AppSettings("Connectionstring")
            cn.Open()

            cmdOutPut = New SqlClient.SqlCommand()
            cmdOutPut.Connection = cn
            cmdOutPut.CommandText = "SELECT * FROM vwAuthorizationALD WHERE UserID = " & m_objUser.UserID.ToString & " AND NOT username like '" & m_objUser.UserName & "'"
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
        Dim connection As SqlClient.SqlConnection
        Dim para As SqlClient.SqlParameter

        Dim command As New SqlClient.SqlCommand()
        Dim sqlInsert As String
        Dim b As Byte()
        connection = New SqlClient.SqlConnection()
        Try

            connection.ConnectionString = ConfigurationManager.AppSettings("Connectionstring")
            sqlInsert = "INSERT INTO AuthorizationALD (username,equipment, data) VALUES (@user,@equi,@data)"

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
            .Add(m_blnALDZulassung)                 '12
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
        Dim connection As SqlClient.SqlConnection
        Dim command As New SqlClient.SqlCommand()
        Dim sqlInsert As String
        connection = New SqlClient.SqlConnection()
        Try
            connection.ConnectionString = ConfigurationManager.AppSettings("Connectionstring")

            sqlInsert = "DELETE FROM AuthorizationALD WHERE id = @id"

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
        Dim connection As SqlClient.SqlConnection
        Dim command As New SqlClient.SqlCommand()
        Dim adapter As New SqlClient.SqlDataAdapter()
        Dim sqlInsert As String
        connection = New SqlClient.SqlConnection
        Try
            connection.ConnectionString = ConfigurationManager.AppSettings("Connectionstring")
            sqlInsert = "SELECT * FROM AuthorizationALD"

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

    'Public Sub Anfordern(ByVal art As String)
    '    If (art = "TEMP") Then
    '        AnfordernSAP()
    '    Else
    '        anfordernSQL()
    '    End If
    'End Sub

    Public Function LeseAdressenSAP(ByVal strParentNode As String, Optional ByVal adressart As String = "A", Optional ByVal nodelevel As String = "99") As Int32
        Dim objSAP As New SAPProxy_Base.SAPProxy_Base()

        Try
            Dim tblTemp As DataTable
            m_tblAdressen = New DataTable()
            m_tblAdressen.Columns.Add("ADDRESSNUMBER", System.Type.GetType("System.String"))
            m_tblAdressen.Columns.Add("DISPLAY_ADDRESS", System.Type.GetType("System.String"))

            m_tblAdressen.Columns.Add("POSTL_CODE", System.Type.GetType("System.String"))
            m_tblAdressen.Columns.Add("STREET", System.Type.GetType("System.String"))
            m_tblAdressen.Columns.Add("COUNTRYISO", System.Type.GetType("System.String"))
            m_tblAdressen.Columns.Add("CITY", System.Type.GetType("System.String"))
            m_tblAdressen.Columns.Add("NAME", System.Type.GetType("System.String"))
            m_tblAdressen.Columns.Add("NAME_2", System.Type.GetType("System.String"))

            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)

            Dim SAPTable As New SAPProxy_Base.BAPIKNA1_KNVHTable()
            Dim SAPReturnTable As New SAPProxy_Base.BAPIRET2()
            Dim SAPSalesTable As New SAPProxy_Base.BAPI_SDVTBERTable()
            Dim SAPCustomerDetail As New SAPProxy_Base.BAPICUSTOMER_KNA1()
            Dim SAPCustomerAdress As New SAPProxy_Base.BAPICUSTOMER_04()
            Dim SAPCustomerCompanyDetail As New SAPProxy_Base.BAPICUSTOMER_05()
            Dim SAPBapiRet1 As New SAPProxy_Base.BAPIRET1()
            Dim SAPCustomerBankDetail As New SAPProxy_Base.BAPICUSTOMER_02Table()

            objSAP.Bapi_Customer_Get_Children(adressart, Right("0000000000" & strParentNode, 10), nodelevel, Format(Now, "yyyyMMdd"), SAPReturnTable, SAPTable, SAPSalesTable)

            tblTemp = SAPTable.ToADODataTable

            Dim SAPTableRow As DataRow

            Dim newDealerDetailRow As DataRow

            For Each SAPTableRow In tblTemp.Rows
                'Der Händler soll sich nicht selbst zur Auswahl bekommen!!!
                'If SAPTableRow("NODE_LEVEL").ToString.TrimStart("0"c) = "1" Then
                'Die Detaildaten zu den Händlern in die Tabelle m_tblHaendler schreiben
                objSAP.Bapi_Customer_Getdetail2("", SAPTableRow("CUSTOMER").ToString, SAPCustomerAdress, SAPCustomerCompanyDetail, SAPCustomerDetail, SAPBapiRet1, SAPCustomerBankDetail)

                If SAPBapiRet1.Type.Trim(" "c) = "" Or SAPBapiRet1.Type = "S" Or SAPBapiRet1.Type = "I" Then
                    'If (Not SAPCustomerDetail.Groupkey = m_objUser.Reference) Or (SAPCustomerDetail.Groupkey.Length = 0 And m_objUser.Reference.Length = 0) Then
                    newDealerDetailRow = m_tblAdressen.NewRow

                    Dim strTemp As String = SAPCustomerAdress.Name
                    If SAPCustomerAdress.Name_2.Length > 0 Then
                        strTemp &= ", " & SAPCustomerAdress.Name_2
                    End If
                    If SAPCustomerAdress.Name_3.Length > 0 Then
                        strTemp &= ", " & SAPCustomerAdress.Name_3
                    End If
                    If SAPCustomerAdress.Name_4.Length > 0 Then
                        strTemp &= ", " & SAPCustomerAdress.Name_4
                    End If

                    newDealerDetailRow("DISPLAY_ADDRESS") = strTemp & ", " & SAPCustomerAdress.Countryiso & " - " & SAPCustomerAdress.Postl_Code & " " & SAPCustomerAdress.City & ", " & SAPCustomerAdress.Street
                    newDealerDetailRow("ADDRESSNUMBER") = SAPCustomerDetail.Customer
                    If SAPCustomerAdress.Postl_Code.Length > 0 Then
                        newDealerDetailRow("POSTL_CODE") = SAPCustomerAdress.Postl_Code
                    End If
                    If SAPCustomerAdress.Street.Length > 0 Then
                        newDealerDetailRow("STREET") = SAPCustomerAdress.Street
                    End If
                    If SAPCustomerAdress.Countryiso.Length > 0 Then
                        newDealerDetailRow("COUNTRYISO") = SAPCustomerAdress.Countryiso
                    End If
                    If SAPCustomerAdress.City.Length > 0 Then
                        newDealerDetailRow("CITY") = SAPCustomerAdress.City
                    End If
                    If SAPCustomerAdress.Name.Length > 0 Then
                        newDealerDetailRow("NAME") = SAPCustomerAdress.Name
                    End If
                    If SAPCustomerAdress.Name_2.Length > 0 Then
                        newDealerDetailRow("NAME_2") = SAPCustomerAdress.Name_2
                    End If

                    m_tblAdressen.Rows.Add(newDealerDetailRow)
                    'End If
                End If
                'End If
            Next

            Return m_tblAdressen.Rows.Count
        Catch ex As Exception
            Throw ex
        Finally
            objSAP.Connection.Close()
            objSAP.Dispose()
        End Try
    End Function

    Public Function GiveResultStructure() As DataTable
        Dim tblTemp As DataTable
        '§§§ JVE 17.01.2006 Tabellenname hat sich geändert. Wieso???
        'Dim tblTempSAP As New SAPProxy_ALD.ZDAD_M_WEB_EQUIDATENTable()
        Dim tblTempSAP As New SAPProxy_ALD.ZDAD_M_WEB_ALDTable()
        '--------------------------------------------------------------
        tblTemp = tblTempSAP.ToADODataTable
        tblTemp.Columns.Add("STATUS", System.Type.GetType("System.String"))
        tblTemp.Columns.Add("SWITCH", System.Type.GetType("System.Boolean"))
        Return tblTemp
    End Function
#End Region
End Class

' ************************************************
' $History: ALD_1.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 29.04.09   Time: 14:58
' Updated in $/CKAG/Applications/appALD/Lib
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 17.04.08   Time: 15:15
' Created in $/CKAG/Applications/appald/Lib
' 
' *****************  Version 23  *****************
' User: Rudolpho     Date: 6.12.07    Time: 13:15
' Updated in $/CKG/Applications/AppALD/AppALDWeb/Lib
' ITA: 1440
' 
' *****************  Version 22  *****************
' User: Uha          Date: 2.07.07    Time: 15:39
' Updated in $/CKG/Applications/AppALD/AppALDWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 21  *****************
' User: Uha          Date: 5.03.07    Time: 16:59
' Updated in $/CKG/Applications/AppALD/AppALDWeb/Lib
' 
' ************************************************
