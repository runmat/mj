Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports Microsoft.Data.SAPClient
Imports CKG.Base.Business

<Serializable()> Public Class FFE_Bank_2
    REM § Lese-/Schreibfunktion, Kunde: FFD,
    REM § Show - BAPI: Z_M_AUFTRAGSDATEN,
    REM § Change - BAPI: Zz_Sd_Order_Credit_Release.

    Inherits Base.Business.BankBase

#Region " Declarations"
    Private m_strHaendler As String
    Private m_tblAuftragsUebersicht As DataTable
    Private m_tblAuftraegeAlle As DataTable
    Private m_tblAuftraege As DataTable
    Private m_strAuftragsNummer As String
    Private m_tblRaw As DataTable
    Private m_blnZeigeFlottengeschaeft As Boolean
    Private m_blnZeigeHEZ As Boolean            'HEZ
    Private m_blnZeigeStandard As Boolean       'HEZ
    'Private m_strZulassungsart As String        'HEZ    §§§JVE 14.07.2005
    Private m_blnZeigeAlle As Boolean
    Private m_blnZeigeKFKL As Boolean
    Private m_blnStorno As Boolean
    Private m_strKunde As String
    Private m_strFaelligkeit As String
    Private m_strBetrag As String
    Private m_strBriefnr As String
    Private m_strFahrgestellnr As String
    Private m_strUserRef As String

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

    Public Property UserRef() As String
        Get
            Return m_strUserRef
        End Get
        Set(ByVal Value As String)
            m_strUserRef = Value
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
    Public Property Fahrgestellnr() As String
        Get
            Return m_strFahrgestellnr
        End Get
        Set(ByVal Value As String)
            m_strFahrgestellnr = Value
        End Set
    End Property

    Public Property Briefnr() As String
        Get
            Return m_strBriefnr
        End Get
        Set(ByVal Value As String)
            m_strBriefnr = Value
        End Set
    End Property

    Public Property Betrag() As String
        Get
            Return m_strBetrag
        End Get
        Set(ByVal Value As String)
            m_strBetrag = Value
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
    Public Property Storno() As Boolean
        Get
            Return m_blnStorno
        End Get
        Set(ByVal Value As Boolean)
            m_blnStorno = Value
        End Set
    End Property

    Public ReadOnly Property AuftraegeAlle() As DataTable
        Get
            Return m_tblAuftraegeAlle
        End Get
    End Property

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

    Public Property Haendler() As String
        Get
            Return m_strHaendler
        End Get
        Set(ByVal Value As String)
            m_strHaendler = Right(Value, 6).TrimStart("0"c)
            m_tblAuftraege = m_tblAuftraegeAlle.Copy

            Dim i As Int32
            For i = m_tblAuftraege.Rows.Count - 1 To 0 Step -1      'Zeilen, die nicht zu dem gewählten Händler gehören, löschen
                If Not Right(m_tblAuftraege.Rows(i)("KUNNR").ToString, 5).TrimStart("0"c) = m_strHaendler Then
                    m_tblAuftraege.Rows(i).Delete()
                End If
            Next
            m_tblAuftraege.AcceptChanges()
            Dim rowTemp As DataRow
            'Es kann immer nur eine Kontingentart z.Z. angezeigt werden!!!
            m_tblAuftraege.Columns.Add("KontingentCode", System.Type.GetType("System.String"))  'Hier werden die Kontingentarten je nach vorheriger Auswahl (bei mehreren Arten) rausgefiltert. (Nicht ausgewählte gelöscht)
            For i = m_tblAuftraege.Rows.Count - 1 To 0 Step -1
                rowTemp = m_tblAuftraege.Rows(i)
                If Not m_blnZeigeAlle Then
                    'HEZ
                    If m_blnZeigeStandard And (rowTemp("BSTZD").ToString = "0004" Or rowTemp("BSTZD").ToString = "0005") Then
                        m_tblAuftraege.Rows(i).Delete()
                        GoTo DerNaechsteBitte
                    End If
                    If m_blnZeigeFlottengeschaeft And (rowTemp("BSTZD").ToString <> "0004") Then
                        m_tblAuftraege.Rows(i).Delete()
                        GoTo DerNaechsteBitte
                    End If
                    If m_blnZeigeHEZ And (rowTemp("BSTZD").ToString <> "0005") Then
                        m_tblAuftraege.Rows(i).Delete()
                        GoTo DerNaechsteBitte
                    End If
                End If
                rowTemp.Item("KUNNR") = Right(rowTemp.Item("KUNNR").ToString, 5).TrimStart("0"c)
                rowTemp("KontingentCode") = rowTemp("BSTZD").ToString
                Select Case rowTemp("BSTZD").ToString
                    Case "0001"
                        rowTemp("BSTZD") = "Standard temporär"
                    Case "0002"
                        rowTemp("BSTZD") = "Standard endgültig"
                    Case "0003"
                        rowTemp("BSTZD") = "Retail"
                    Case "0004"
                        rowTemp("BSTZD") = "Erweitertes Zahlungsziel (Delayed Payment) endgültig"
                    Case "0005"
                        rowTemp("BSTZD") = "Händlereigene Zulassung" 'TODOHEZ
                    Case "0006"
                        rowTemp("BSTZD") = "KF/KL"
                End Select
DerNaechsteBitte:
            Next

            m_tblAuftraege.Columns.Add("Fälligkeitsdatum", System.Type.GetType("System.DateTime"))
        End Set
    End Property

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

    Public Property ZeigeKFKL() As Boolean
        Get
            Return m_blnZeigeKFKL
        End Get
        Set(ByVal Value As Boolean)
            m_blnZeigeKFKL = Value
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

    Public Sub Show_Retail()
        m_strClassAndMethod = "FDD_Bank_2.Show_Retail"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_FFE.SAPProxy_FFE()

            Dim tblAuftragsUebersichtSAP As New SAPProxy_FFE.ZDAD_M_WEB_AUFTRAEGE_ANZ_VORTable()
            Dim rowAuftragsUebersichtSAP As New SAPProxy_FFE.ZDAD_M_WEB_AUFTRAEGE_ANZ_VOR()
            Dim tblAuftraegeSAP As New SAPProxy_FFE.ZDAD_M_WEB_AUFTRAEGETable()
            Dim sapDatum As New SAPProxy_FFE.DATUM_RANGETable()


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

                m_intIDSAP = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Auftragsdaten", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)
                objSAP.Z_M_Auftragsdaten_FCE("0003", m_strFiliale, m_strCustomer, "", "1510", tblAuftragsUebersichtSAP, tblAuftraegeSAP, sapDatum)         ' TODOHEZ
                objSAP.CommitWork()
                If m_intIDSAP > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                End If

                m_tblRaw = tblAuftraegeSAP.ToADODataTable
                m_tblAuftraegeAlle = CreateOutPut(m_tblRaw, m_strAppID)
                m_tblAuftragsUebersicht = New DataTable()

                m_tblAuftragsUebersicht.Columns.Add("Händlernummer", System.Type.GetType("System.String"))
                m_tblAuftragsUebersicht.Columns.Add("Händlername", System.Type.GetType("System.String"))
                m_tblAuftragsUebersicht.Columns.Add("Anzahl Retail", System.Type.GetType("System.Int32"))



                For i = 0 To tblAuftragsUebersichtSAP.Count - 1
                    rowAuftragsUebersichtSAP = tblAuftragsUebersichtSAP.Item(i)
                    If IsNumeric(rowAuftragsUebersichtSAP.Zaehler_03) Then
                        If CInt(rowAuftragsUebersichtSAP.Zaehler_03) > 0 Then
                            rowTemp = m_tblAuftragsUebersicht.NewRow
                            rowTemp("Händlernummer") = Right(rowAuftragsUebersichtSAP.Kunnr, 6).TrimStart("0"c)
                            rowTemp("Händlername") = rowAuftragsUebersichtSAP.Name1_Zf
                            rowTemp("Anzahl Retail") = CInt(rowAuftragsUebersichtSAP.Zaehler_03)
                            m_tblAuftragsUebersicht.Rows.Add(rowTemp)
                        End If
                    End If
                Next

                'm_tblAuftraegeAlle.Columns.Add("Anfragenummer", System.Type.GetType("System.String"))
                m_tblAuftraegeAlle.Columns.Add("InAutorisierung", System.Type.GetType("System.Boolean"))
                m_tblAuftraegeAlle.Columns.Add("Initiator", System.Type.GetType("System.String"))

                For i = 0 To m_tblAuftraegeAlle.Columns.Count - 1
                    Dim s As String = m_tblAuftraegeAlle.Columns.Item(i).Caption()
                Next
                For Each rowTemp In m_tblAuftraegeAlle.Rows
                    Dim intAutID As Int32 = 0
                    Dim strInitiator As String = ""
                    m_objApp.CheckForPendingAuthorization(CInt(m_strAppID), Right(rowTemp("KUNNR").ToString, 5), rowTemp("ZZFAHRG").ToString, m_objUser.IsTestUser, strInitiator, intAutID)
                    rowTemp("Initiator") = strInitiator
                    rowTemp("VBELN") = Right(rowTemp("VBELN").ToString, 10).TrimStart("0"c)
                    rowTemp("ZZANFDT") = FormatDateTime(MakeDateStandard(rowTemp("ZZANFDT").ToString), DateFormat.ShortDate)

                    If intAutID > 0 Then
                        rowTemp("InAutorisierung") = True
                    Else
                        rowTemp("InAutorisierung") = False
                    End If
                    If rowTemp("BSTZD").ToString = "0003" Then
                        rowTemp("BSTZD") = "Retail"
                    End If
                Next

                If m_strHaendler.Length > 0 Then
                    Haendler = m_strHaendler
                End If

                WriteLogEntry(True, "KONZS=" & m_strCustomer.TrimStart("0"c) & ", KNRZE=" & m_strFiliale & ", KUNNR=, KKBER=", m_tblAuftragsUebersicht)
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
                If m_intIDsap > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, False, m_strMessage)
                End If
                WriteLogEntry(False, "KONZS=" & m_strCustomer.TrimStart("0"c) & ", KNRZE=" & m_strFiliale & ", KUNNR=, KKBER=" & " , " & Replace(m_strMessage, "<br>", " "), m_tblAuftragsUebersicht)

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
    Public Overrides Sub Show()
        m_strClassAndMethod = "FFE_Bank_2.Show"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_FFE.SAPProxy_FFE()

            Dim tblAuftragsUebersichtSAP As New SAPProxy_FFE.ZDAD_M_WEB_AUFTRAEGE_ANZ_VORTable()
            Dim rowAuftragsUebersichtSAP As New SAPProxy_FFE.ZDAD_M_WEB_AUFTRAEGE_ANZ_VOR()
            Dim tblAuftraegeSAP As New SAPProxy_FFE.ZDAD_M_WEB_AUFTRAEGETable()
            Dim sapDatum As New SAPProxy_FFE.DATUM_RANGETable()


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
                Dim strKKBER As String = String.Empty

                m_intIDSAP = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Auftragsdaten", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)
                objSAP.Z_M_Auftragsdaten_FCE(strKKBER, m_strFiliale, m_strCustomer, "", "1510", tblAuftragsUebersichtSAP, tblAuftraegeSAP, sapDatum)         ' TODOHEZ
                objSAP.CommitWork()
                If m_intIDSAP > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                End If

                m_tblAuftragsUebersicht = New DataTable()

                m_tblAuftragsUebersicht.Columns.Add("Händlernummer", System.Type.GetType("System.String"))
                m_tblAuftragsUebersicht.Columns.Add("Händlername", System.Type.GetType("System.String"))
                m_tblAuftragsUebersicht.Columns.Add("Anzahl Standard endgültig", System.Type.GetType("System.Int32"))
                m_tblAuftragsUebersicht.Columns.Add("Anzahl Standard temporär", System.Type.GetType("System.Int32"))
                m_tblAuftragsUebersicht.Columns.Add("Anzahl Flottengeschäft", System.Type.GetType("System.Int32"))
                'HEZ
                m_tblAuftragsUebersicht.Columns.Add("Anzahl HEZ", System.Type.GetType("System.Int32"))
                '
                m_tblAuftragsUebersicht.Columns.Add("Anzahl KF/KL", System.Type.GetType("System.Int32"))

                For i = 0 To tblAuftragsUebersichtSAP.Count - 1
                    rowAuftragsUebersichtSAP = tblAuftragsUebersichtSAP.Item(i)
                    If IsNumeric(rowAuftragsUebersichtSAP.Zaehler_01) And IsNumeric(rowAuftragsUebersichtSAP.Zaehler_02) And IsNumeric(rowAuftragsUebersichtSAP.Zaehler_04) And IsNumeric(rowAuftragsUebersichtSAP.Zaehler_05) Then
                        If CInt(rowAuftragsUebersichtSAP.Zaehler_01) + CInt(rowAuftragsUebersichtSAP.Zaehler_02) + CInt(rowAuftragsUebersichtSAP.Zaehler_04) + CInt(rowAuftragsUebersichtSAP.Zaehler_05) > 0 Then
                            rowTemp = m_tblAuftragsUebersicht.NewRow
                            rowTemp("Händlernummer") = Right(rowAuftragsUebersichtSAP.Kunnr, 6).TrimStart("0"c)
                            rowTemp("Händlername") = rowAuftragsUebersichtSAP.Name1_Zf
                            rowTemp("Anzahl Standard endgültig") = CInt(rowAuftragsUebersichtSAP.Zaehler_02)
                            rowTemp("Anzahl Standard temporär") = CInt(rowAuftragsUebersichtSAP.Zaehler_01)
                            rowTemp("Anzahl Flottengeschäft") = CInt(rowAuftragsUebersichtSAP.Zaehler_04)
                            rowTemp("Anzahl HEZ") = CInt(rowAuftragsUebersichtSAP.Zaehler_05)
                            rowTemp("Anzahl KF/KL") = CInt(rowAuftragsUebersichtSAP.Zaehler_06)
                            m_tblAuftragsUebersicht.Rows.Add(rowTemp)
                        End If
                    End If
                Next

                m_tblRaw = tblAuftraegeSAP.ToADODataTable
                m_tblAuftraegeAlle = CreateOutPut(m_tblRaw, m_strAppID)
                m_tblAuftraegeAlle.Columns.Add("InAutorisierung", System.Type.GetType("System.Boolean"))
                m_tblAuftraegeAlle.Columns.Add("Initiator", System.Type.GetType("System.String"))


                For Each rowTemp In m_tblAuftraegeAlle.Rows
                    Dim intAutID As Int32 = 0
                    Dim strInitiator As String = ""
                    m_objApp.CheckForPendingAuthorization(CInt(m_strAppID), Right(rowTemp("KUNNR").ToString, 5), rowTemp("ZZFAHRG").ToString, m_objUser.IsTestUser, strInitiator, intAutID)
                    rowTemp("Initiator") = strInitiator
                    If intAutID > 0 Then
                        rowTemp("InAutorisierung") = True
                    Else
                        rowTemp("InAutorisierung") = False
                    End If

                    rowTemp("VBELN") = Right(rowTemp("VBELN").ToString, 10).TrimStart("0"c)
                    rowTemp("ZZANFDT") = FormatDateTime(MakeDateStandard(rowTemp("ZZANFDT").ToString), DateFormat.ShortDate)

                    '§§§JVE 14.07.2005 <begin> Zulassungsarten für HEZ füllen...

                    If rowTemp("KVGR3").ToString = "2" Then
                        rowTemp("KVGR3") = "S"
                    End If
                    If rowTemp("KVGR3").ToString = "3" Then
                        rowTemp("KVGR3") = "N"
                    End If
                    If rowTemp("KVGR3").ToString = "4" Then
                        rowTemp("KVGR3") = "V"
                    End If
                    '§§§JVE 14.07.2005 <end>
                Next
                Dim tmpRows As DataRow()

                tmpRows = m_tblRaw.Select("Bstzd = '0003'")
                If tmpRows.Length > 0 Then
                    Dim i2 As Integer

                    For i2 = 0 To tmpRows.Length - 1
                        Dim iLoeschFlag As Integer = 0
                        tmpRows(i2).BeginEdit()
                        If tmpRows(i2).Item("Bstzd").ToString = "0003" Then
                            iLoeschFlag = 1
                        End If
                        If iLoeschFlag = 1 Then
                            tmpRows(i2).Delete()
                        End If
                        tmpRows(i2).EndEdit()
                        m_tblRaw.AcceptChanges()
                    Next i2
                End If

                If m_strHaendler.Length > 0 Then
                    Haendler = m_strHaendler
                End If

                WriteLogEntry(True, "KONZS=" & m_strCustomer.TrimStart("0"c) & ", KNRZE=" & m_strFiliale & ", KUNNR=, KKBER=", m_tblAuftragsUebersicht)
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
                If m_intIDsap > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, False, m_strMessage)
                End If
                WriteLogEntry(False, "KONZS=" & m_strCustomer.TrimStart("0"c) & ", KNRZE=" & m_strFiliale & ", KUNNR=, KKBER=" & " , " & Replace(m_strMessage, "<br>", " "), m_tblAuftragsUebersicht)
            End Try
            If m_intIDsap > -1 Then
                m_objlogApp.WriteStandardDataAccessSAP(m_intIDsap)
            End If

            objSAP.Connection.Close()
            objSAP.Dispose()

            m_blnGestartet = False
        End If
    End Sub
    Public Sub Fill()
        m_strClassAndMethod = "FDD_Bank_2.Show"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_FFE.SAPProxy_FFE()

            Dim tblAuftragsUebersichtSAP As New SAPProxy_FFE.ZDAD_M_WEB_AUFTRAEGE_ANZ_VORTable()
            Dim rowAuftragsUebersichtSAP As New SAPProxy_FFE.ZDAD_M_WEB_AUFTRAEGE_ANZ_VOR()
            Dim tblAuftraegeSAP As New SAPProxy_FFE.ZDAD_M_WEB_AUFTRAEGETable()
            Dim sapDatum As New SAPProxy_FFE.DATUM_RANGETable()


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
                Dim strKKBER As String = String.Empty

                m_intIDSAP = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Auftragsdaten", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)
                objSAP.Z_M_Auftragsdaten_Fce(strKKBER, m_strFiliale, m_strCustomer, "", "1510", tblAuftragsUebersichtSAP, tblAuftraegeSAP, sapDatum)         ' TODOHEZ

                objSAP.CommitWork()
                If m_intIDSAP > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                End If

                m_tblAuftragsUebersicht = New DataTable()

                m_tblAuftragsUebersicht.Columns.Add("Händlernummer", System.Type.GetType("System.String"))
                m_tblAuftragsUebersicht.Columns.Add("Händlername", System.Type.GetType("System.String"))
                m_tblAuftragsUebersicht.Columns.Add("Anzahl Standard endgültig", System.Type.GetType("System.Int32"))
                m_tblAuftragsUebersicht.Columns.Add("Anzahl Standard temporär", System.Type.GetType("System.Int32"))
                m_tblAuftragsUebersicht.Columns.Add("Anzahl Flottengeschäft", System.Type.GetType("System.Int32"))
                'HEZ
                m_tblAuftragsUebersicht.Columns.Add("Anzahl HEZ", System.Type.GetType("System.Int32"))

                For i = 0 To tblAuftragsUebersichtSAP.Count - 1
                    rowAuftragsUebersichtSAP = tblAuftragsUebersichtSAP.Item(i)
                    If IsNumeric(rowAuftragsUebersichtSAP.Zaehler_01) And IsNumeric(rowAuftragsUebersichtSAP.Zaehler_02) And IsNumeric(rowAuftragsUebersichtSAP.Zaehler_04) And IsNumeric(rowAuftragsUebersichtSAP.Zaehler_05) Then
                        If CInt(rowAuftragsUebersichtSAP.Zaehler_01) + CInt(rowAuftragsUebersichtSAP.Zaehler_02) + CInt(rowAuftragsUebersichtSAP.Zaehler_04) + CInt(rowAuftragsUebersichtSAP.Zaehler_05) > 0 Then
                            rowTemp = m_tblAuftragsUebersicht.NewRow
                            rowTemp("Händlernummer") = Right(rowAuftragsUebersichtSAP.Kunnr, 6).TrimStart("0"c)
                            rowTemp("Händlername") = rowAuftragsUebersichtSAP.Name1_Zf
                            rowTemp("Anzahl Standard endgültig") = CInt(rowAuftragsUebersichtSAP.Zaehler_02)
                            rowTemp("Anzahl Standard temporär") = CInt(rowAuftragsUebersichtSAP.Zaehler_01)
                            rowTemp("Anzahl Flottengeschäft") = CInt(rowAuftragsUebersichtSAP.Zaehler_04)
                            rowTemp("Anzahl HEZ") = CInt(rowAuftragsUebersichtSAP.Zaehler_05)
                            m_tblAuftragsUebersicht.Rows.Add(rowTemp)
                        End If
                    End If
                Next

                m_tblRaw = tblAuftraegeSAP.ToADODataTable
                m_tblAuftraegeAlle = CreateOutPut(m_tblRaw, m_strAppID)
                'm_tblAuftraegeAlle = m_tblRaw
                If m_strHaendler.Length > 0 Then
                    Haendler = m_strHaendler
                End If


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
                If m_intIDsap > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, False, m_strMessage)
                End If
                WriteLogEntry(False, "KONZS=" & m_strCustomer.TrimStart("0"c) & ", KNRZE=" & m_strFiliale & ", KUNNR=, KKBER=" & " , " & Replace(m_strMessage, "<br>", " "), m_tblAuftragsUebersicht)
            End Try
            If m_intIDsap > -1 Then
                m_objlogApp.WriteStandardDataAccessSAP(m_intIDsap)
            End If

            objSAP.Connection.Close()
            objSAP.Dispose()

            m_blnGestartet = False
        End If
    End Sub
    Public Overrides Sub Change()
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_FFE.SAPProxy_FFE()

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

                If m_strAuftragsNummer.Trim("0"c).Length = 0 Or (Not m_strAuftragsNummer.Length = 10) Then
                    m_intStatus = -1301
                    m_strMessage = "Keine gültige Auftragsnummer übergeben."
                Else
                    Dim rowFahrzeug() As DataRow = m_tblRaw.Select("VBELN = '" & m_strAuftragsNummer & "'")
                    Dim rowAuftrag() As DataRow = m_tblAuftraege.Select("VBELN = '" & m_strAuftragsNummer.TrimStart("0"c) & "'")
                    Dim strLeer As String = String.Empty
                    m_intIDSAP = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Auftragsdaten_Freigabe_Web", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                    Dim strBSTZD As String = ""
                    If rowFahrzeug(0)("BSTZD").ToString = "Retail" Then
                        strBSTZD = "0003"
                    Else
                        strBSTZD = rowFahrzeug(0)("BSTZD").ToString
                    End If

                    Dim strEQUNR As String = rowFahrzeug(0)("EQUNR").ToString
                    Dim strERNAM As String = m_strInternetUser
                    Dim strKNZRE As String = rowFahrzeug(0)("KNRZE").ToString
                    Dim strKUNNR As String = rowFahrzeug(0)("KUNNR").ToString
                    Dim strKONZS As String = m_strCustomer
                    Dim strMATNR As String = strLeer
                    Dim strStorno As String
                    Dim strText50 As String = m_strKunde
                    Dim strVBELN As String = m_strAuftragsNummer
                    Dim strYESNO As String = " "
                    Dim strZZFAEDT As String = ""
                    Dim intBetrag As Decimal = 0
                    Dim strFreitext As String = rowFahrzeug(0)("ZZTXT2").ToString
                    If rowAuftrag.Length > 0 AndAlso strBSTZD = "0003" Then ' nur Betrag bei Retail
                        m_strBetrag = rowAuftrag(0)("ANSWT").ToString
                    End If
                    If IsDate(m_strFaelligkeit) Then
                        strZZFAEDT = Format(CDate(m_strFaelligkeit), "yyyyMMdd")
                    End If
                    If m_blnStorno Then
                        strStorno = "X"
                    Else
                        strStorno = " "
                    End If
                    If IsNumeric(m_strBetrag) = True Then
                        intBetrag = CDec(m_strBetrag)
                    End If
                    If Not ZeigeHEZ Then
                        objSAP.Z_M_Auftragsdaten_Freigabe_Fce(intBetrag, strBSTZD, strEQUNR, strERNAM, "", strKNZRE, strKONZS, strKUNNR, strMATNR, strStorno, strText50, strFreitext, strVBELN, strZZFAEDT)      '01.06.2005 HEZ
                    Else
                        objSAP.Z_M_Auftragsdaten_Freigabe_Fce(intBetrag, strBSTZD, strEQUNR, strERNAM, "X", strKNZRE, strKONZS, strKUNNR, strMATNR, strStorno, strText50, "", strVBELN, strZZFAEDT) ''01.06.2005 HEZ
                    End If

                    objSAP.CommitWork()

                    If m_intIDSAP > -1 Then
                        m_objlogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                    End If
                End If
            Catch ex As Exception
                Select Case ex.Message
                    Case "NO_DATA"
                        m_intStatus = -3401
                        m_strMessage = "Keine Eingabedaten vorhanden"
                    Case "NO_UPDATE"
                        m_intStatus = -3402
                        m_strMessage = "Kein EQUI-UPDATE"
                    Case "NO_ZDADVERSAND"
                        m_intStatus = -3403
                        m_strMessage = "Kein ZDADVERSAND-INSERT"
                    Case "NO_UPDATE_SALESDOCUMENT"
                        m_intStatus = -3404
                        m_strMessage = "Keine Auftragsänderung"
                    Case "ZCREDITCONTROL_SPERRE"
                        m_intStatus = -3405
                        m_strMessage = "ZCREDITCONTROL vom DAD gesperrt"
                    Case "NO_FREIGABE"
                        m_intStatus = -3406
                        m_strMessage = "Brief bereits freigegeben (Kontingentänderung oder Zahlungseingang)"
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message
                End Select
                If m_intIDSAP > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
                End If
            Finally
                objSAP.Connection.Close()
                objSAP.Dispose()
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Sub GiveCarSingle(ByVal linzr As String)
        '§§§JVE 07.09.05 <begin>
        'Sucht ein Fahrzeug aus der Menge der Angeforderten Fahrzeuge anhand der Vertragsnummer raus
        m_strClassAndMethod = "FDD_Haendler.GiveCarsSingle"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_FFE.SAPProxy_FFE()
            Dim tblFahrzeugeSAP As New SAPProxy_FFE.ZDAD_FAHRZEUGDATEN_FCETable() '  .ZDAD_M_WEB_EQUIDATENTable()

            m_tblAuftraege = New DataTable()
            With m_tblAuftraege.Columns

                .Add("PARNR", System.Type.GetType("System.String"))
                .Add("LIZNR", System.Type.GetType("System.String"))
                .Add("TIDNR", System.Type.GetType("System.String"))
                .Add("CHASSIS_NUM", System.Type.GetType("System.String"))
                .Add("ZZREFERENZ1", System.Type.GetType("System.String"))
                .Add("ZZKKBER", System.Type.GetType("System.String"))
                .Add("ZZTMPDT", System.Type.GetType("System.String"))
                .Add("ZZFAEDT", System.Type.GetType("System.String"))
                .Add("ZZBEZAHLT", System.Type.GetType("System.String"))
                .Add("ZZRWERT", System.Type.GetType("System.String"))
            End With
            MakeDestination()
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1
            Dim Chassi As String = ""
            Dim ZZREF As String = ""
            Dim Liznr As String = ""
            Try
                m_intStatus = 0
                m_strMessage = ""

                'If CheckCustomerData() Then
                m_intIDsap = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Kreditkontrollb_Aendern", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                objSAP.Z_M_Fahrzeugdaten_Fce(m_strKUNNR, m_strFahrgestellnr, linzr, m_strUserRef, m_strBriefnr, tblFahrzeugeSAP)
                objSAP.CommitWork()

                If m_intIDsap > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, True)
                End If

                Dim tblTemp As DataTable
                tblTemp = tblFahrzeugeSAP.ToADODataTable

                Dim newRow As DataRow
                Dim tblRow As DataRow

                newRow = m_tblAuftraege.NewRow()
                For Each tblRow In tblTemp.Rows
                    newRow("PARNR") = tblRow("PARNR")
                    newRow("LIZNR") = tblRow("LIZNR")
                    Liznr = tblRow("LIZNR").ToString
                    newRow("TIDNR") = tblRow("TIDNR")
                    newRow("CHASSIS_NUM") = tblRow("CHASSIS_NUM")
                    Chassi = tblRow("CHASSIS_NUM").ToString
                    newRow("ZZREFERENZ1") = tblRow("ZZREFERENZ1")
                    ZZREF = tblRow("ZZREFERENZ1").ToString
                    newRow("ZZKKBER") = tblRow("ZZKKBER")
                    newRow("ZZTMPDT") = MakeDateStandard(tblRow("ZZTMPDT").ToString).ToShortDateString
                    If CType(newRow("ZZTMPDT"), String) = "01.01.1900" Then
                        newRow("ZZTMPDT") = String.Empty
                    End If

                    newRow("ZZFAEDT") = tblRow("ZZFAEDT")
                    newRow("ZZBEZAHLT") = tblRow("ZZBEZAHLT")
                    newRow("ZZRWERT") = tblRow("ZZRWERT")
                    m_tblAuftraege.Rows.Add(newRow)
                Next


                m_tblAuftraege.AcceptChanges()

                WriteLogEntry(True, "CHASSIS_NUM=" & Chassi & ", KONZS=" & m_strKUNNR.TrimStart("0"c) & ", KNRZE=" & m_strFiliale & ", KUNNR=" & Right(m_strCustomer, 5) & ", LIZNR=" & Liznr & ", ZZREFERENZ1=" & ZZREF, m_tblAuftraege)
                'End If
            Catch ex As Exception
                Select Case ex.Message
                    Case "NO_LIZNT"
                        m_intStatus = -2500
                        m_strMessage = "Die Vertragsnummer muß 11-stellig sein."
                    Case "NO_DATA"
                        m_intStatus = -2501
                        m_strMessage = "Es wurden keine Daten gefunden."
                    Case "NO_VERSAND"
                        m_intStatus = -2502
                        m_strMessage = "Brief nicht versendet."
                    Case "NO_OFFEN"
                        m_intStatus = -2503
                        m_strMessage = "Fahrzeug bereits bezahlt."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = "Unbekannter Fehler!" 'ex.Message
                End Select
                If m_intIDsap > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, False, m_strMessage)
                End If

                WriteLogEntry(False, "CHASSIS_NUM=" & Chassi & ", KONZS=" & m_strKUNNR.TrimStart("0"c) & ", KNRZE=" & m_strFiliale & ", KUNNR=" & Right(m_strCustomer, 5) & ", LIZNR=" & Liznr & ", ZZREFERENZ1=" & ZZREF & " , " & Replace(m_strMessage, "<br>", " "), m_tblAuftraege)
            Finally
                If m_intIDsap > -1 Then
                    m_objlogApp.WriteStandardDataAccessSAP(m_intIDsap)
                End If

                objSAP.Connection.Close()
                objSAP.Dispose()

                m_blnGestartet = False
            End Try
        End If
        '§§§JVE 07.09.05 <end>
    End Sub

    Public Sub SetStatus(ByVal liznr As String, ByVal haendlernr As String, ByVal kkber As String, ByVal fdat As String)
        'Schreibt die Änderungen an einem Fahrzeugbrief in SAP 
        'liznr: Vertragsnummer
        'kkber: Neue Kontingentart
        'fdat : Faelligkeitsdatum (nur bei DP)
        '§§§JVE 07.09.05 <begin>

        m_strClassAndMethod = "FDD_Bank_2.SetStatus"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_FFE.SAPProxy_FFE()

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

                'If CheckCustomerData() Then
                m_intIDsap = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Equipment_Aktualisieren", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                If fdat <> String.Empty Then
                    If IsDate(fdat) Then
                        fdat = MakeDateSAP(fdat)
                    End If
                End If

                objSAP.Z_M_Equi_Change_Kkber_Fce(m_strKUNNR, Right("0000000000" & "60" & haendlernr, 10), liznr, fdat, kkber)
                objSAP.CommitWork()

                If m_intIDsap > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, True)
                End If

                WriteLogEntry(True, "LIZNR=" & liznr & ", KONZS=" & m_strKUNNR.TrimStart("0"c) & ", KNRZE=" & m_strFiliale & ", KUNNR=" & Right(m_strCustomer, 5), m_tblAuftraege)
                'End If
            Catch ex As Exception
                Select Case ex.Message
                    Case "NO_FORD"
                        m_intStatus = -2500
                        m_strMessage = "Fehler: Falsche Kundennummer."
                    Case "NO_DATA"
                        m_intStatus = -2501
                        m_strMessage = "Fehler: Es wurden keine Daten gefunden."
                    Case "NO_CREDITCONTROL"
                        m_intStatus = -2501
                        m_strMessage = "Fehler: Händler nicht gefunden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = "Unbekannter Fehler!" 'ex.Message
                End Select
                If m_intIDsap > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, False, m_strMessage)
                End If

                WriteLogEntry(False, "LIZNR=" & liznr & ", KONZS=" & m_strKUNNR.TrimStart("0"c) & ", KNRZE=" & m_strFiliale & ", KUNNR=" & Right(m_strCustomer, 5), m_tblAuftraege)
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


    Public Sub checkAutorisierungsEintraegeForFreigabeGesperrterAuftraege(ByRef KompletterAutorisierungsBestand As DataTable)

        m_strClassAndMethod = "FFE_Bank_2.checkAutorisierungsEintraegeForFreigabeGesperrterAuftraege"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            'übergabetabelle definieren
            Dim sapTable As New DataTable
            Dim dataColumns(1) As DataColumn
            dataColumns(0) = New DataColumn("KUNNR", System.Type.GetType("System.String"))
            dataColumns(1) = New DataColumn("CHASSIS_NUM", System.Type.GetType("System.String"))
            sapTable.Columns.AddRange(dataColumns)
            sapTable.AcceptChanges()


            'tabelle mit allen Einträgen aus der Autorisierung befüllen die Change02 sind
            'Change02 =freigabe gesperrter aufträge

            Dim tmpRows() As DataRow

            tmpRows = KompletterAutorisierungsBestand.Select("AppName='Change02'")
            If tmpRows.Length = 0 Then
                m_blnGestartet = False
                Exit Sub
            Else
                'Dim tmpRowX As DataRow
                Dim tmpItemArray(1) As Object
                For Each tmpRow As DataRow In tmpRows
                    tmpItemArray(0) = m_strKUNNR
                    tmpItemArray(1) = tmpRow.Item("ProcessReference")
                    sapTable.Rows.Add(tmpItemArray)
                Next
                sapTable.AcceptChanges()
            End If

            Dim con As New SAPConnection(m_BizTalkSapConnectionString)
            con.Open()
            Try
                Dim cmd As New SAPCommand()
                cmd.Connection = con

                Dim strCom As String

                strCom = "EXEC Z_M_AUTORISIERTE_AUFTRAEGE @GT_WEB=@pI_GT_WEB,"
                strCom = strCom & "@GT_WEB=@pO_GT_WEB OUTPUT OPTION 'disabledatavalidation'"

                cmd.CommandText = strCom

                Dim pI_GT_WEB As New SAPParameter("@pI_GT_WEB", sapTable)
                Dim pO_GT_WEB As New SAPParameter("@pO_GT_WEB", ParameterDirection.Output)

                'tabelle Hinzufügen
                cmd.Parameters.Add(pI_GT_WEB)
                cmd.Parameters.Add(pO_GT_WEB)


                If m_objLogApp Is Nothing Then
                    m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                End If
                m_intIDSAP = -1

                m_intStatus = 0
                m_strMessage = ""

                cmd.ExecuteNonQuery()

                sapTable = DirectCast(pO_GT_WEB.Value, DataTable)

                If sapTable.Rows.Count = 0 Then
                    con.Close()
                    m_blnGestartet = False
                    Exit Sub
                End If

                Dim tmpRowX As DataRow
                For Each tmpRow As DataRow In sapTable.Rows
                    tmpRowX = KompletterAutorisierungsBestand.Select("ProcessReference='" & CStr(tmpRow.Item(1)) & "'")(0)
                    DeleteAuthorizationEntry(m_objApp.Connectionstring, CInt(tmpRowX.Item("AuthorizationID")))
                    tmpRowX.Delete()
                Next
                KompletterAutorisierungsBestand.AcceptChanges()
                WriteLogEntry(True, "alle mittlerweile Freigegebenen Aufträge die sich noch in der Autorisierungstabelle befanden, wurden gelöscht", sapTable)
            Catch ex As Exception

                m_intStatus = -9999
                m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)

                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
                End If

                WriteLogEntry(False, "Fehler in FFE_BANK_2, BAPI=Z_M_AUTORISIERTE_AUFTRAEGE", sapTable)
            Finally
                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteStandardDataAccessSAP(m_intIDSAP)
                End If
                con.Close()
                m_blnGestartet = False
            End Try
        End If

    End Sub

    '§§§JVE 07.09.05 <end>
#End Region
End Class
' ************************************************
' $History: FFE_Bank_2.vb $
' 
' *****************  Version 13  *****************
' User: Rudolpho     Date: 30.04.09   Time: 15:42
' Updated in $/CKAG/Applications/AppFFE/lib
' ITA 2837
' 
' *****************  Version 12  *****************
' User: Jungj        Date: 28.07.08   Time: 9:55
' Updated in $/CKAG/Applications/AppFFE/lib
' ITA 2057 fertig
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 24.07.08   Time: 11:59
' Updated in $/CKAG/Applications/AppFFE/lib
' Finally Block hinzugefügt
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 11.07.08   Time: 9:24
' Updated in $/CKAG/Applications/AppFFE/lib
' ITA 2069
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 7.07.08    Time: 10:40
' Updated in $/CKAG/Applications/AppFFE/lib
' SSV Historien hinzugefügt
' 
' ************************************************
