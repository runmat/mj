Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG

Public Class Porsche_051
    REM § Lese-/Schreibfunktion, Kunde: FFD,
    REM § Show - BAPI: Z_M_AUFTRAGSDATEN,
    REM § Change - BAPI: Zz_Sd_Order_Credit_Release.

    Inherits Base.Business.BankBase

#Region " Declarations"
    Private m_strHaendler As String
    Private m_tblAuftragsUebersicht As DataTable
    'Private m_tblAuftraegeAlle As DataTable
    Private m_tblAuftraege As DataTable
    Private m_strAuftragsNummer As String
    Private m_tblRaw As DataTable
    Private m_blnZeigeFlottengeschaeft As Boolean
    Private m_blnZeigeHEZ As Boolean            'HEZ
    Private m_blnZeigeStandard As Boolean       'HEZ
    'Private m_strZulassungsart As String        'HEZ    §§§JVE 14.07.2005
    Private m_blnZeigeAlle As Boolean
    Private m_strStorno As String
    Private m_strKunde As String
    Private m_strFaelligkeit As String
    Private m_strEquipment As String
#End Region

#Region " Properties"
    Public Property Kunde() As String
        Get
            Return m_strKunde
        End Get
        Set(ByVal Value As String)
            m_strKunde = Value
        End Set
    End Property

    Public Property Faelligkeit() As String
        Get
            Return m_strFaelligkeit
        End Get
        Set(ByVal Value As String)
            m_strFaelligkeit = Value
        End Set
    End Property
    '§§§JVE 14.07.2005 <begin>
    'Public Property Zulassungsart() As String
    '    Get
    '        Return m_strZulassungsart
    '    End Get
    '    Set(ByVal Value As String)
    '        m_strZulassungsart = Value
    '    End Set
    'End Property
    '§§§JVE 14.07.2005 <end>
    Public Property Storno() As String
        Get
            Return m_strStorno
        End Get
        Set(ByVal Value As String)
            m_strStorno = Value
        End Set
    End Property

    'Public ReadOnly Property AuftraegeAlle() As DataTable
    '    Get
    '        Return m_tblAuftraegeAlle
    '    End Get
    'End Property

    Public ReadOnly Property AuftragsUebersicht() As DataTable
        Get
            Return m_tblAuftragsUebersicht
        End Get
    End Property

    Public ReadOnly Property Auftraege() As DataTable
        Get
            Return m_tblAuftraege
        End Get
    End Property

    Public Property AuftragsNummer() As String
        Get
            Return m_strAuftragsNummer
        End Get
        Set(ByVal Value As String)
            m_strAuftragsNummer = Right("0000000000" & Value, 10)
        End Set
    End Property

    Public Property EquipmentNummer() As String
        Get
            Return m_strEquipment
        End Get
        Set(ByVal Value As String)
            m_strEquipment = Value
        End Set
    End Property

    'Public Property Haendler() As String
    '        Get
    '            Return m_strHaendler
    '        End Get
    '        Set(ByVal Value As String)
    '            m_strHaendler = Right(Value, 6).TrimStart("0"c)
    '            'm_tblAuftraege = m_tblAuftraegeAlle.Copy

    '            Dim i As Int32
    '            For i = m_tblAuftraege.Rows.Count - 1 To 0 Step -1      'Zeilen, die nicht zu dem gewählten Händler gehören, löschen
    '                If Not Right(m_tblAuftraege.Rows(i)("Händler").ToString, 5).TrimStart("0"c) = m_strHaendler Then
    '                    m_tblAuftraege.Rows(i).Delete()
    '                End If
    '            Next

    '            Dim rowTemp As DataRow
    '            'Es kann immer nur eine Kontingentart z.Z. angezeigt werden!!!
    '            m_tblAuftraege.Columns.Add("KontingentCode", System.Type.GetType("System.String"))  'Hier werden die Kontingentarten je nach vorheriger Auswahl (bei mehreren Arten) rausgefiltert. (Nicht ausgewählte gelöscht)
    '            For i = m_tblAuftraege.Rows.Count - 1 To 0 Step -1
    '                rowTemp = m_tblAuftraege.Rows(i)
    '                If Not m_blnZeigeAlle Then
    '                    'If m_blnZeigeFlottengeschaeft And (rowTemp("Kontingentart").ToString = "0001" Or rowTemp("Kontingentart").ToString = "0002") Then
    '                    '    m_tblAuftraege.Rows(i).Delete()
    '                    '    GoTo DerNaechsteBitte
    '                    'End If
    '                    'If (Not m_blnZeigeFlottengeschaeft) And rowTemp("Kontingentart").ToString = "0004" Then
    '                    '    m_tblAuftraege.Rows(i).Delete()
    '                    '    GoTo DerNaechsteBitte
    '                    'End If

    '                    'HEZ
    '                    If m_blnZeigeStandard And (rowTemp("Kontingentart").ToString = "0004" Or rowTemp("Kontingentart").ToString = "0005") Then
    '                        m_tblAuftraege.Rows(i).Delete()
    '                        GoTo DerNaechsteBitte
    '                    End If
    '                    If m_blnZeigeFlottengeschaeft And (rowTemp("Kontingentart").ToString <> "0004") Then
    '                        m_tblAuftraege.Rows(i).Delete()
    '                        GoTo DerNaechsteBitte
    '                    End If
    '                    If m_blnZeigeHEZ And (rowTemp("Kontingentart").ToString <> "0005") Then
    '                        m_tblAuftraege.Rows(i).Delete()
    '                        GoTo DerNaechsteBitte
    '                    End If
    '                End If
    '                rowTemp.Item("Händler") = Right(rowTemp.Item("Händler").ToString, 5).TrimStart("0"c)
    '                rowTemp("KontingentCode") = rowTemp("Kontingentart").ToString
    '                Select Case rowTemp("Kontingentart").ToString
    '                    Case "0001"
    '                        rowTemp("Kontingentart") = "Standard temporär"
    '                    Case "0002"
    '                        rowTemp("Kontingentart") = "Standard endgültig"
    '                    Case "0004"
    '                        rowTemp("Kontingentart") = "Erweitertes Zahlungsziel (Delayed Payment) endgültig"
    '                    Case "0005"
    '                        rowTemp("Kontingentart") = "Händlereigene Zulassung" 'TODOHEZ
    '                End Select
    'DerNaechsteBitte:
    '            Next

    '            m_tblAuftraege.Columns.Add("Fälligkeitsdatum", System.Type.GetType("System.DateTime"))
    '        End Set
    'End Property

    Public Property ZeigeAlle() As Boolean
        Get
            Return m_blnZeigeAlle
        End Get
        Set(ByVal Value As Boolean)
            m_blnZeigeAlle = Value
        End Set
    End Property

    Public Property ZeigeFlottengeschaeft() As Boolean
        Get
            Return m_blnZeigeFlottengeschaeft
        End Get
        Set(ByVal Value As Boolean)
            m_blnZeigeFlottengeschaeft = Value
        End Set
    End Property

    Public Property ZeigeHEZ() As Boolean
        Get
            Return m_blnZeigeHEZ
        End Get
        Set(ByVal Value As Boolean)
            m_blnZeigeHEZ = Value
        End Set
    End Property

    Public Property ZeigeStandard() As Boolean
        Get
            Return m_blnZeigeStandard
        End Get
        Set(ByVal Value As Boolean)
            m_blnZeigeStandard = Value
        End Set
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
        m_blnZeigeFlottengeschaeft = False
        m_blnZeigeHEZ = False   'HEZ
        m_blnZeigeAlle = False
        m_blnZeigeStandard = False 'HEZ
        m_strHaendler = ""
    End Sub

    Public Overrides Sub Show()
        m_strClassAndMethod = "FDD_Bank_2.Show"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_PORSCHE.SAPProxy_PORSCHE() ' SAPProxy_FFD.SAPProxy_FFD()

            'Dim rowAuftraegeSAP As New SAPProxy_PORSCHE.ZDAD_M_WEB_GESP_AUFTRAEGE() ' SAPProxy_FFD.ZDAD_M_WEB_AUFTRAEGE_ANZ_VOR()
            Dim tblAuftraegeSAP As New SAPProxy_PORSCHE.ZDAD_M_WEB_GESP_AUFTRAEGETable() ' .ZDAD_M_WEB_AUFTRAEGETable()
            'Dim sapDatum As New SAPProxy_FFD.DATUM_RANGETable()


            MakeDestination()
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1
            Dim i As Int32
            Dim rowTemp As DataRow

            Try
                m_intStatus = 0
                m_strMessage = ""
                Dim strKKBER As String

                m_intIDSAP = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Gesperrt_Auftrag_Porsche", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)
                objSAP.Z_M_Gesperrt_Auftrag_Porsche(Right("0000000000" & m_objUser.KUNNR, 10), m_strFiliale, "Change04_5.aspx", tblAuftraegeSAP)
                objSAP.CommitWork()
                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                End If

                m_tblAuftraege = tblAuftraegeSAP.ToADODataTable

                Dim row As DataRow
                Dim rowFound As DataRow()
                Dim rowNewHaendler As DataRow
                Dim rowNewKontingent As DataRow

                m_tblAuftraege.Columns.Add("KontingentartText", System.Type.GetType("System.String"))
                m_tblAuftraege.Columns.Add("Status", System.Type.GetType("System.String"))


                For Each row In m_tblAuftraege.Rows
                    row("Status") = String.Empty
                    row("VBELN") = CStr(row("VBELN")).TrimStart("0"c)

                    If CInt(row("ABCKZ")) = 1 Then
                        row("KontingentartText") = "Standard temporär"
                    End If
                    If CInt(row("ABCKZ")) = 2 Then
                        row("KontingentartText") = "Standard endgültig"
                    End If
                    If Not (TypeOf (row("ZZANFDT")) Is System.DBNull) Then
                        row("ZZANFDT") = Left(CStr(MakeDateStandard(CStr(row("ZZANFDT")))), 10)
                    End If


                    If Not (TypeOf (row("ERZET")) Is System.DBNull) Then
                        row("ERZET") = CStr(row("ERZET")).Insert(2, ":").Insert(5, ":")
                        row("ERZET") = System.Convert.ToDateTime(row("ERZET")).ToLongTimeString
                    End If


                Next

                m_tblAuftragsUebersicht = New DataTable()

                With m_tblAuftragsUebersicht.Columns
                    .Add("Händlernummer", System.Type.GetType("System.String"))
                    .Add("HändlernummerText", System.Type.GetType("System.String"))
                    .Add("Händleradresse", System.Type.GetType("System.String"))
                    .Add("StandardTemp", System.Type.GetType("System.Int32"))
                    .Add("StandardEndg", System.Type.GetType("System.Int32"))
                    .Add("Kontingentart", System.Type.GetType("System.Int32"))
                    '.Add("Anzahl", System.Type.GetType("System.Int32"))             'Anzahl
                End With

                For Each row In m_tblAuftraege.Rows
                    rowFound = m_tblAuftragsUebersicht.Select("Händlernummer='" & CStr(row("KUNNR_ZF")) & "'")
                    If (rowFound.Length = 0) Then       'Händler noch nicht gefunden, einfügen
                        'Zähler hochsetzen, sichtbar machen
                        rowNewHaendler = m_tblAuftragsUebersicht.NewRow
                        rowNewHaendler("Händlernummer") = CStr(row("KUNNR_ZF"))
                        rowNewHaendler("HändlernummerText") = Right(CStr(row("KUNNR_ZF")), 5)
                        rowNewHaendler("Händleradresse") = CStr(row("NAME1")) & "," & CStr(row("ORT01"))

                        If CInt(row("ABCKZ")) = 1 Then
                            rowNewHaendler("StandardTemp") = 1
                            rowNewHaendler("StandardEndg") = 0
                            rowNewHaendler("Kontingentart") = 1
                        End If
                        If CInt(row("ABCKZ")) = 2 Then
                            rowNewHaendler("StandardEndg") = 1
                            rowNewHaendler("StandardTemp") = 0
                            rowNewHaendler("Kontingentart") = 2
                        End If
                        m_tblAuftragsUebersicht.Rows.Add(rowNewHaendler)
                        m_tblAuftragsUebersicht.AcceptChanges()
                    Else
                        If CInt(row("ABCKZ")) = 1 Then
                            rowFound(0)("StandardTemp") = CInt(rowFound(0)("StandardTemp")) + 1
                            rowFound(0)("Kontingentart") = 1
                        End If
                        If CInt(row("ABCKZ")) = 2 Then
                            rowFound(0)("StandardEndg") = CInt(rowFound(0)("StandardEndg")) + 1
                            rowFound(0)("Kontingentart") = 2
                        End If
                    End If
                Next

                WriteLogEntry(True, "KONZS=" & m_objUser.KUNNR & ", KNRZE=" & m_strFiliale & ", KUNNR=, KKBER=", m_tblAuftragsUebersicht)
            Catch ex As Exception
                Select Case ex.Message
                    Case "NO_WEB"
                        m_intStatus = -1401
                        m_strMessage = "Keine Web-Tabelle erstellt."
                    Case "NO_DATA"
                        m_intStatus = -1402
                        m_strMessage = "Keine Daten gefunden."
                    Case "NO_HAENDLER"
                        m_intStatus = -1403
                        m_strMessage = "Händler nicht vorhanden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message
                End Select
                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
                End If
                WriteLogEntry(False, "KONZS=" & m_objUser.KUNNR & ", KNRZE=" & m_strFiliale & ", KUNNR=, KKBER=" & " , " & Replace(m_strMessage, "<br>", " "), m_tblAuftragsUebersicht)

            Finally
                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteStandardDataAccessSAP(m_intIDSAP)
                End If

                objSAP.Connection.Close()
                objSAP.Dispose()

                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Overrides Sub Change()
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_PORSCHE.SAPProxy_PORSCHE()

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

                m_intIDSAP = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Freigeben_Auftrag_Porsche", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)
                objSAP.Z_M_Freigeben_Auftrag_Porsche(m_strEquipment, m_objUser.UserName, m_strKunde, Right("0000000000" & m_objUser.KUNNR, 10), Right("000000000060" & m_strFiliale, 10), "", m_strStorno, m_strAuftragsNummer, "Change05_2.aspx")
                objSAP.CommitWork()

                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                End If
            Catch ex As Exception
                Select Case ex.Message
                    Case "NO_ZDADCREDITLIMIT"
                        m_intStatus = -3401
                        m_strMessage = "Keine Kreditlimit-Information vorhanden."
                    Case "NO_ZDADVERSAND"
                        m_intStatus = -3403
                        m_strMessage = "Insert - Fehler."
                    Case "NO_AUFTRAG"
                        m_intStatus = -3406
                        m_strMessage = "Kein passender Auftrag selektiert."
                        '§§§ JVE 26.07.2006: Weiter Ausnahmen für Stornofunktionalität...
                    Case "NO_GESPERRT"
                        m_intStatus = -3606
                        m_strMessage = "Dieser Auftrag ist nicht gesperrt."
                    Case "NO_EQUI"
                        m_intStatus = -3706
                        m_strMessage = "Fehler bei der Equipment-Änderung."
                    Case "NO_AUFTRAG_CHANGE"
                        m_intStatus = -3806
                        m_strMessage = "Auftragsänderung fehlgeschlagen."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message
                End Select
                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
                End If
            Finally
                objSAP.Connection.Close()
                objSAP.Dispose()
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Sub GiveCarSingle(ByVal linzr As String)
        ''§§§JVE 07.09.05 <begin>
        ''Sucht ein Fahrzeug aus der Menge der Angeforderten Fahrzeuge anhand der Vertragsnummer raus
        'm_strClassAndMethod = "FDD_Haendler.GiveCarsSingle"
        'If Not m_blnGestartet Then
        '    m_blnGestartet = True

        '    Dim objSAP As New SAPProxy_FFD2.SAPProxy_FFD2()
        '    Dim tblFahrzeugeSAP As New SAPProxy_FFD2.ZDAD_M_WEB_KKBER() '  .ZDAD_M_WEB_EQUIDATENTable()

        '    m_tblAuftraege = New DataTable()
        '    With m_tblAuftraege.Columns

        '        .Add("PARNR", System.Type.GetType("System.String"))
        '        .Add("LIZNR", System.Type.GetType("System.String"))
        '        .Add("CHASSIS_NUM", System.Type.GetType("System.String"))
        '        .Add("ZZREFERENZ1", System.Type.GetType("System.String"))
        '        .Add("ZZKKBER", System.Type.GetType("System.String"))
        '        .Add("ZZTMPDT", System.Type.GetType("System.String"))
        '        .Add("ZZFAEDT", System.Type.GetType("System.String"))
        '        .Add("ZZBEZAHLT", System.Type.GetType("System.String"))
        '        .Add("ZZRWERT", System.Type.GetType("System.String"))
        '    End With
        '    MakeDestination()
        '    objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
        '    objSAP.Connection.Open()

        '    If m_objLogApp Is Nothing Then
        '        m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
        '    End If
        '    m_intIDSAP = -1

        '    Try
        '        m_intStatus = 0
        '        m_strMessage = ""

        '        'If CheckCustomerData() Then
        '        m_intIDsap = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Unangefordert", m_strAppID, m_strSessionID)

        '        objSAP.Z_M_Kreditkontrollb_Aendern(m_strKUNNR, linzr, tblFahrzeugeSAP)
        '        objSAP.CommitWork()

        '        If m_intIDsap > -1 Then
        '            m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, True)
        '        End If

        '        Dim newRow As DataRow
        '        newRow = m_tblAuftraege.NewRow()

        '        newRow("PARNR") = tblFahrzeugeSAP.Parnr
        '        newRow("LIZNR") = tblFahrzeugeSAP.Liznr
        '        newRow("CHASSIS_NUM") = tblFahrzeugeSAP.Chassis_Num
        '        newRow("ZZREFERENZ1") = tblFahrzeugeSAP.Zzreferenz1
        '        newRow("ZZKKBER") = tblFahrzeugeSAP.Zzkkber
        '        newRow("ZZTMPDT") = MakeDateStandard(tblFahrzeugeSAP.Zztmpdt).ToShortDateString
        '        If CType(newRow("ZZTMPDT"), String) = "01.01.1900" Then
        '            newRow("ZZTMPDT") = String.Empty
        '        End If

        '        newRow("ZZFAEDT") = tblFahrzeugeSAP.Zzfaedt
        '        newRow("ZZBEZAHLT") = tblFahrzeugeSAP.Zzbezahlt
        '        newRow("ZZRWERT") = tblFahrzeugeSAP.Zzrwert

        '        m_tblAuftraege.Rows.Add(newRow)
        '        m_tblAuftraege.AcceptChanges()

        '        WriteLogEntry(True, "CHASSIS_NUM=" & tblFahrzeugeSAP.Chassis_Num & ", KONZS=" & m_strKUNNR.TrimStart("0"c) & ", KNRZE=" & m_strFiliale & ", KUNNR=" & Right(m_strCustomer, 5) & ", LIZNR=" & tblFahrzeugeSAP.Liznr & ", ZZREFERENZ1=" & tblFahrzeugeSAP.Zzreferenz1, m_tblAuftraege)
        '        'End If
        '    Catch ex As Exception
        '        Select Case ex.Message
        '            Case "NO_LIZNT"
        '                m_intStatus = -2500
        '                m_strMessage = "Die Vertragsnummer muß 11-stellig sein."
        '            Case "NO_DATA"
        '                m_intStatus = -2501
        '                m_strMessage = "Es wurden keine Daten gefunden."
        '            Case "NO_VERSAND"
        '                m_intStatus = -2502
        '                m_strMessage = "Brief nicht versendet."
        '            Case "NO_OFFEN"
        '                m_intStatus = -2503
        '                m_strMessage = "Fahrzeug bereits bezahlt."
        '            Case Else
        '                m_intStatus = -9999
        '                m_strMessage = "Unbekannter Fehler!" 'ex.Message
        '        End Select
        '        If m_intIDsap > -1 Then
        '            m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, False, m_strMessage)
        '        End If

        '        WriteLogEntry(False, "CHASSIS_NUM=" & tblFahrzeugeSAP.Chassis_Num & ", KONZS=" & m_strKUNNR.TrimStart("0"c) & ", KNRZE=" & m_strFiliale & ", KUNNR=" & Right(m_strCustomer, 5) & ", LIZNR=" & tblFahrzeugeSAP.Liznr & ", ZZREFERENZ1=" & tblFahrzeugeSAP.Zzreferenz1 & " , " & Replace(m_strMessage, "<br>", " "), m_tblAuftraege)
        '    Finally
        '        If m_intIDsap > -1 Then
        '            m_objlogApp.WriteStandardDataAccessSAP(m_intIDsap)
        '        End If

        '        objSAP.Connection.Close()
        '        objSAP.Dispose()

        '        m_blnGestartet = False
        '    End Try
        'End If
        '§§§JVE 07.09.05 <end>
    End Sub

    Public Sub SetStatus(ByVal liznr As String, ByVal haendlernr As String, ByVal kkber As String, ByVal fdat As String)
        'Schreibt die Änderungen an einem Fahrzeugbrief in SAP 
        'liznr: Vertragsnummer
        'kkber: Neue Kontingentart
        'fdat : Faelligkeitsdatum (nur bei DP)
        '§§§JVE 07.09.05 <begin>

        'm_strClassAndMethod = "FDD_Bank_2.SetStatus"
        'If Not m_blnGestartet Then
        '    m_blnGestartet = True

        '    Dim objSAP As New SAPProxy_FFD2.SAPProxy_FFD2()

        '    MakeDestination()
        '    objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
        '    objSAP.Connection.Open()

        '    If m_objLogApp Is Nothing Then
        '        m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
        '    End If
        '    m_intIDSAP = -1

        '    Try
        '        m_intStatus = 0
        '        m_strMessage = ""

        '        'If CheckCustomerData() Then
        '        m_intIDsap = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Unangefordert", m_strAppID, m_strSessionID)

        '        If fdat <> String.Empty Then
        '            If IsDate(fdat) Then
        '                fdat = MakeDateSAP(fdat)
        '            End If
        '        End If

        '        objSAP.Z_M_Equipment_Aktualisieren(m_strKUNNR, Right("0000000000" & "60" & haendlernr, 10), liznr, fdat, kkber)
        '        objSAP.CommitWork()

        '        If m_intIDsap > -1 Then
        '            m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, True)
        '        End If

        '        WriteLogEntry(True, "LIZNR=" & liznr & ", KONZS=" & m_strKUNNR.TrimStart("0"c) & ", KNRZE=" & m_strFiliale & ", KUNNR=" & Right(m_strCustomer, 5), m_tblAuftraege)
        '        'End If
        '    Catch ex As Exception
        '        Select Case ex.Message
        '            Case "NO_FORD"
        '                m_intStatus = -2500
        '                m_strMessage = "Fehler: Falsche Kundennummer."
        '            Case "NO_DATA"
        '                m_intStatus = -2501
        '                m_strMessage = "Fehler: Es wurden keine Daten gefunden."
        '            Case "NO_CREDITCONTROL"
        '                m_intStatus = -2501
        '                m_strMessage = "Fehler: Händler nicht gefunden."
        '            Case Else
        '                m_intStatus = -9999
        '                m_strMessage = "Unbekannter Fehler!" 'ex.Message
        '        End Select
        '        If m_intIDsap > -1 Then
        '            m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, False, m_strMessage)
        '        End If

        '        WriteLogEntry(False, "LIZNR=" & liznr & ", KONZS=" & m_strKUNNR.TrimStart("0"c) & ", KNRZE=" & m_strFiliale & ", KUNNR=" & Right(m_strCustomer, 5), m_tblAuftraege)
        '    Finally
        '        If m_intIDsap > -1 Then
        '            m_objlogApp.WriteStandardDataAccessSAP(m_intIDsap)
        '        End If

        '        objSAP.Connection.Close()
        '        objSAP.Dispose()

        '        m_blnGestartet = False
        '    End Try
        'End If
    End Sub
    '§§§JVE 07.09.05 <end>
#End Region
End Class

' ************************************************
' $History: Porsche_051.vb $
' 
' *****************  Version 1  *****************
' User: Rudolpho     Date: 7.04.09    Time: 15:20
' Created in $/CKAG2/Applications/AppPorsche/lib
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 24.07.08   Time: 14:18
' Updated in $/CKAG/Applications/AppPorsche/Lib
' Finally block hinzugefügt
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 11:28
' Created in $/CKAG/Applications/AppPorsche/Lib
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 14.02.08   Time: 11:43
' Updated in $/CKG/Applications/AppPorsche/AppPorscheWeb/Lib
' ita 1654 fertig
' 
' *****************  Version 7  *****************
' User: Uha          Date: 2.07.07    Time: 18:24
' Updated in $/CKG/Applications/AppPorsche/AppPorscheWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 6  *****************
' User: Uha          Date: 5.03.07    Time: 12:50
' Updated in $/CKG/Applications/AppPorsche/AppPorscheWeb/Lib
' 
' ************************************************