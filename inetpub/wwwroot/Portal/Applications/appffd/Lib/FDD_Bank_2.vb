Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports CKG.Base.Common

<Serializable()> Public Class FDD_Bank_2
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
    Private m_blnStorno As Boolean
    Private m_strKunde As String
    Private m_strFaelligkeit As String
    Private m_strBetrag As String
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

    Public Property Betrag() As String
        Get
            Return m_strBetrag
        End Get
        Set(ByVal Value As String)
            m_strBetrag = Value
        End Set
    End Property

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
                If Not Right(m_tblAuftraege.Rows(i)("Händler").ToString, 5).TrimStart("0"c) = m_strHaendler Then
                    m_tblAuftraege.Rows(i).Delete()
                End If
            Next

            Dim rowTemp As DataRow
            'Es kann immer nur eine Kontingentart z.Z. angezeigt werden!!!
            m_tblAuftraege.Columns.Add("KontingentCode", System.Type.GetType("System.String"))  'Hier werden die Kontingentarten je nach vorheriger Auswahl (bei mehreren Arten) rausgefiltert. (Nicht ausgewählte gelöscht)
            For i = m_tblAuftraege.Rows.Count - 1 To 0 Step -1
                rowTemp = m_tblAuftraege.Rows(i)
                If Not m_blnZeigeAlle Then
                    'HEZ
                    If m_blnZeigeStandard And (rowTemp("Kontingentart").ToString = "0004" Or rowTemp("Kontingentart").ToString = "0005") Then
                        m_tblAuftraege.Rows(i).Delete()
                        GoTo DerNaechsteBitte
                    End If
                    If m_blnZeigeFlottengeschaeft And (rowTemp("Kontingentart").ToString <> "0004") Then
                        m_tblAuftraege.Rows(i).Delete()
                        GoTo DerNaechsteBitte
                    End If
                    If m_blnZeigeHEZ And (rowTemp("Kontingentart").ToString <> "0005") Then
                        m_tblAuftraege.Rows(i).Delete()
                        GoTo DerNaechsteBitte
                    End If
                End If
                rowTemp.Item("Händler") = Right(rowTemp.Item("Händler").ToString, 5).TrimStart("0"c)
                rowTemp("KontingentCode") = rowTemp("Kontingentart").ToString
                Select Case rowTemp("Kontingentart").ToString
                    Case "0001"
                        rowTemp("Kontingentart") = "Standard temporär"
                    Case "0002"
                        rowTemp("Kontingentart") = "Standard endgültig"
                    Case "0003"
                        rowTemp("Kontingentart") = "Retail"
                    Case "0004"
                        rowTemp("Kontingentart") = "Erweitertes Zahlungsziel (Delayed Payment) endgültig"
                    Case "0005"
                        rowTemp("Kontingentart") = "Händlereigene Zulassung" 'TODOHEZ
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

    End Sub

    Public Overrides Sub Change()

    End Sub

    Public Overloads Sub Show_Retail(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)

        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0

        Try

            m_strMessage = ""
            Dim strKKBER As String = ""

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Auftragsdaten", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_KUNNR", "")
            'myProxy.setImportParameter("I_KNRZE", m_strFiliale)
            'myProxy.setImportParameter("I_KONZS", m_strCustomer)
            'myProxy.setImportParameter("I_KKBER", "0003")
            'myProxy.setImportParameter("I_VKORG", "1510")

            'myProxy.callBapi()

            S.AP.InitExecute("Z_M_Auftragsdaten", "I_KUNNR,I_KNRZE,I_KONZS,I_KKBER,I_VKORG", "", m_strFiliale, m_strCustomer, "0003", "1510")

            m_tblAuftragsUebersicht = New DataTable()

            m_tblAuftragsUebersicht.Columns.Add("Händlernummer", System.Type.GetType("System.String"))
            m_tblAuftragsUebersicht.Columns.Add("Händlername", System.Type.GetType("System.String"))
            m_tblAuftragsUebersicht.Columns.Add("Anzahl Retail", System.Type.GetType("System.Int32"))

            Dim tblAuftragsUebersichtSAP As DataTable = S.AP.GetExportTable("GT_ANZ") 'myProxy.getExportTable("GT_ANZ")
            Dim rowAuftragsUebersichtSAP As DataRow
            Dim rowTemp As DataRow
            Dim i As Integer

            For i = 0 To tblAuftragsUebersichtSAP.Rows.Count - 1
                rowAuftragsUebersichtSAP = tblAuftragsUebersichtSAP.Rows(i)
                If IsNumeric(rowAuftragsUebersichtSAP("Zaehler_03")) Then
                    If CInt(rowAuftragsUebersichtSAP("Zaehler_03")) > 0 Then
                        rowTemp = m_tblAuftragsUebersicht.NewRow
                        rowTemp("Händlernummer") = Right(rowAuftragsUebersichtSAP("Kunnr").ToString, 6).TrimStart("0"c)
                        rowTemp("Händlername") = rowAuftragsUebersichtSAP("Name1_Zf")
                        rowTemp("Anzahl Retail") = CInt(rowAuftragsUebersichtSAP("Zaehler_03"))
                        m_tblAuftragsUebersicht.Rows.Add(rowTemp)
                    End If
                End If
            Next

            m_tblRaw = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")
            m_tblAuftraegeAlle = CreateOutPut(m_tblRaw, m_strAppID)
            m_tblAuftraegeAlle.Columns.Add("InAutorisierung", System.Type.GetType("System.Boolean"))
            m_tblAuftraegeAlle.Columns.Add("Initiator", System.Type.GetType("System.String"))

            For i = 0 To m_tblAuftraegeAlle.Columns.Count - 1
                Dim s As String = m_tblAuftraegeAlle.Columns.Item(i).Caption()
            Next

            For Each rowTemp In m_tblAuftraegeAlle.Rows
                Dim intAutID As Int32 = 0
                Dim strInitiator As String = ""
                m_objApp.CheckForPendingAuthorization(CInt(m_strAppID), Right(rowTemp("Händler").ToString, 5), rowTemp("Fahrgestellnummer").ToString, m_objUser.IsTestUser, strInitiator, intAutID)
                rowTemp("Initiator") = strInitiator
                If intAutID > 0 Then
                    rowTemp("InAutorisierung") = True
                Else
                    rowTemp("InAutorisierung") = False
                End If
                If rowTemp("Kontingentart").ToString = "0003" Then
                    rowTemp("Kontingentart") = "Retail"
                End If
            Next

            If m_strHaendler.Length > 0 Then
                Haendler = m_strHaendler
            End If

            WriteLogEntry(True, "KONZS=" & m_strCustomer.TrimStart("0"c) & ", KNRZE=" & m_strFiliale & ", KUNNR=, KKBER=", m_tblAuftragsUebersicht)
        Catch ex As Exception
            Select Case ex.Message.Replace("Execution failed", "").Trim()
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
                    WriteLogEntry(False, "KONZS=" & m_strCustomer.TrimStart("0"c) & ", KNRZE=" & m_strFiliale & ", KUNNR=, KKBER=" & " , " & Replace(m_strMessage, "<br>", " "), m_tblAuftragsUebersicht)
            End Select
        End Try

    End Sub

    Public Overloads Sub Show(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)

        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0

        Try

            Dim strKKBER As String = ""

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Auftragsdaten", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_KUNNR", "")
            'myProxy.setImportParameter("I_KNRZE", m_strFiliale)
            'myProxy.setImportParameter("I_KONZS", m_strCustomer)
            'myProxy.setImportParameter("I_KKBER", strKKBER)
            'myProxy.setImportParameter("I_VKORG", "1510")

            'myProxy.callBapi()

            S.AP.InitExecute("Z_M_Auftragsdaten", "I_KUNNR,I_KNRZE,I_KONZS,I_KKBER,I_VKORG", "", m_strFiliale, m_strCustomer, strKKBER, "1510")

            m_tblAuftragsUebersicht = New DataTable()

            m_tblAuftragsUebersicht.Columns.Add("Händlernummer", System.Type.GetType("System.String"))
            m_tblAuftragsUebersicht.Columns.Add("Händlername", System.Type.GetType("System.String"))
            m_tblAuftragsUebersicht.Columns.Add("Anzahl Standard endgültig", System.Type.GetType("System.Int32"))
            m_tblAuftragsUebersicht.Columns.Add("Anzahl Standard temporär", System.Type.GetType("System.Int32"))
            m_tblAuftragsUebersicht.Columns.Add("Anzahl Flottengeschäft", System.Type.GetType("System.Int32"))
            m_tblAuftragsUebersicht.Columns.Add("Anzahl HEZ", System.Type.GetType("System.Int32"))

            Dim tblAuftragsUebersichtSAP As DataTable = S.AP.GetExportTable("GT_ANZ") 'myProxy.getExportTable("GT_ANZ")
            Dim rowAuftragsUebersichtSAP As DataRow
            Dim rowTemp As DataRow
            Dim i As Integer

            For i = 0 To tblAuftragsUebersichtSAP.Rows.Count - 1
                rowAuftragsUebersichtSAP = tblAuftragsUebersichtSAP.Rows(i)
                If IsNumeric(rowAuftragsUebersichtSAP("Zaehler_01")) And IsNumeric(rowAuftragsUebersichtSAP("Zaehler_02")) And IsNumeric(rowAuftragsUebersichtSAP("Zaehler_04")) And IsNumeric(rowAuftragsUebersichtSAP("Zaehler_05")) Then
                    If CInt(rowAuftragsUebersichtSAP("Zaehler_01")) + CInt(rowAuftragsUebersichtSAP("Zaehler_02")) + CInt(rowAuftragsUebersichtSAP("Zaehler_04")) + CInt(rowAuftragsUebersichtSAP("Zaehler_05")) > 0 Then
                        rowTemp = m_tblAuftragsUebersicht.NewRow
                        rowTemp("Händlernummer") = Right(rowAuftragsUebersichtSAP("Kunnr").ToString, 6).TrimStart("0"c)
                        rowTemp("Händlername") = rowAuftragsUebersichtSAP("Name1_Zf")
                        rowTemp("Anzahl Standard endgültig") = CInt(rowAuftragsUebersichtSAP("Zaehler_02"))
                        rowTemp("Anzahl Standard temporär") = CInt(rowAuftragsUebersichtSAP("Zaehler_01"))
                        rowTemp("Anzahl Flottengeschäft") = CInt(rowAuftragsUebersichtSAP("Zaehler_04"))
                        rowTemp("Anzahl HEZ") = CInt(rowAuftragsUebersichtSAP("Zaehler_05"))
                        m_tblAuftragsUebersicht.Rows.Add(rowTemp)
                    End If
                End If
            Next

            m_tblRaw = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")
            m_tblAuftraegeAlle = CreateOutPut(m_tblRaw, m_strAppID)
            m_tblAuftraegeAlle.Columns.Add("InAutorisierung", System.Type.GetType("System.Boolean"))
            m_tblAuftraegeAlle.Columns.Add("Initiator", System.Type.GetType("System.String"))


            For Each rowTemp In m_tblAuftraegeAlle.Rows
                Dim intAutID As Int32 = 0
                Dim strInitiator As String = ""
                m_objApp.CheckForPendingAuthorization(CInt(m_strAppID), Right(rowTemp("Händler").ToString, 5), rowTemp("Fahrgestellnummer").ToString, m_objUser.IsTestUser, strInitiator, intAutID)
                rowTemp("Initiator") = strInitiator
                If intAutID > 0 Then
                    rowTemp("InAutorisierung") = True
                Else
                    rowTemp("InAutorisierung") = False
                End If

                If rowTemp("ART").ToString = "2" Then
                    rowTemp("ART") = "S"
                End If

                If rowTemp("ART").ToString = "3" Then
                    rowTemp("ART") = "N"
                End If

                If rowTemp("ART").ToString = "4" Then
                    rowTemp("ART") = "V"
                End If
            Next

            Dim tmpRows As DataRow() = m_tblRaw.Select("Bstzd = '0003'")

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
            Select Case ex.Message.Replace("Execution failed", "").Trim()
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
                    WriteLogEntry(False, "KONZS=" & m_strCustomer.TrimStart("0"c) & ", KNRZE=" & m_strFiliale & ", KUNNR=, KKBER=" & " , " & Replace(m_strMessage, "<br>", " "), m_tblAuftragsUebersicht)
            End Select
        End Try

    End Sub

    Public Sub Fill(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
      
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0

        If Not m_blnGestartet Then
            m_blnGestartet = True

            m_intIDSAP = -1
            Dim i As Int32

            Try
                Dim strKKBER As String = ""

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Auftragsdaten", m_objApp, m_objUser, page)

                'myProxy.setImportParameter("I_KUNNR", "")
                'myProxy.setImportParameter("I_KNRZE", m_strFiliale)
                'myProxy.setImportParameter("I_KONZS", m_strCustomer)
                'myProxy.setImportParameter("I_KKBER", strKKBER)
                'myProxy.setImportParameter("I_VKORG", "1510")

                'myProxy.callBapi()

                S.AP.InitExecute("Z_M_Auftragsdaten", "I_KUNNR,I_KNRZE,I_KONZS,I_KKBER,I_VKORG", "", m_strFiliale, m_strCustomer, strKKBER, "1510")

                m_tblAuftragsUebersicht = New DataTable()

                m_tblAuftragsUebersicht.Columns.Add("Händlernummer", System.Type.GetType("System.String"))
                m_tblAuftragsUebersicht.Columns.Add("Händlername", System.Type.GetType("System.String"))
                m_tblAuftragsUebersicht.Columns.Add("Anzahl Standard endgültig", System.Type.GetType("System.Int32"))
                m_tblAuftragsUebersicht.Columns.Add("Anzahl Standard temporär", System.Type.GetType("System.Int32"))
                m_tblAuftragsUebersicht.Columns.Add("Anzahl Flottengeschäft", System.Type.GetType("System.Int32"))
                m_tblAuftragsUebersicht.Columns.Add("Anzahl HEZ", System.Type.GetType("System.Int32"))

                Dim tblAuftragsUebersichtSAP As DataTable = S.AP.GetExportTable("GT_ANZ") 'myProxy.getExportTable("GT_ANZ")
                Dim rowAuftragsUebersichtSAP As DataRow
                Dim rowTemp As DataRow


                For i = 0 To tblAuftragsUebersichtSAP.Rows.Count - 1
                    rowAuftragsUebersichtSAP = tblAuftragsUebersichtSAP.Rows(i)
                    If IsNumeric(rowAuftragsUebersichtSAP("Zaehler_01")) And IsNumeric(rowAuftragsUebersichtSAP("Zaehler_02")) And IsNumeric(rowAuftragsUebersichtSAP("Zaehler_04")) And IsNumeric(rowAuftragsUebersichtSAP("Zaehler_05")) Then
                        If CInt(rowAuftragsUebersichtSAP("Zaehler_01")) + CInt(rowAuftragsUebersichtSAP("Zaehler_02")) + CInt(rowAuftragsUebersichtSAP("Zaehler_04")) + CInt(rowAuftragsUebersichtSAP("Zaehler_05")) > 0 Then
                            rowTemp = m_tblAuftragsUebersicht.NewRow
                            rowTemp("Händlernummer") = Right(rowAuftragsUebersichtSAP("Kunnr").ToString, 6).TrimStart("0"c)
                            rowTemp("Händlername") = rowAuftragsUebersichtSAP("Name1_Zf")
                            rowTemp("Anzahl Standard endgültig") = CInt(rowAuftragsUebersichtSAP("Zaehler_02"))
                            rowTemp("Anzahl Standard temporär") = CInt(rowAuftragsUebersichtSAP("Zaehler_01"))
                            rowTemp("Anzahl Flottengeschäft") = CInt(rowAuftragsUebersichtSAP("Zaehler_04"))
                            rowTemp("Anzahl HEZ") = CInt(rowAuftragsUebersichtSAP("Zaehler_05"))
                            m_tblAuftragsUebersicht.Rows.Add(rowTemp)
                        End If
                    End If
                Next

                m_tblRaw = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")
                m_tblAuftraegeAlle = CreateOutPut(m_tblRaw, m_strAppID)

                If m_strHaendler.Length > 0 Then
                    Haendler = m_strHaendler
                End If

            Catch ex As Exception
                Select Case ex.Message.Replace("Execution failed", "").Trim()
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
                WriteLogEntry(False, "KONZS=" & m_strCustomer.TrimStart("0"c) & ", KNRZE=" & m_strFiliale & ", KUNNR=, KKBER=" & " , " & Replace(m_strMessage, "<br>", " "), m_tblAuftragsUebersicht)
            End Try
        End If
    End Sub

    Public Overloads Sub Change(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
      
        m_intIDSAP = -1

        Try
            m_intStatus = 0
            m_strMessage = ""
            'Dim myProxy = DynSapProxy.getProxy("Z_M_Auftragsdaten_Freigabe_Web", m_objApp, m_objUser, page)

            If m_strAuftragsNummer.Trim("0"c).Length = 0 Or (Not m_strAuftragsNummer.Length = 10) Then
                m_intStatus = -1301
                m_strMessage = "Keine gültige Auftragsnummer übergeben."
            Else
                Dim rowFahrzeug() = m_tblRaw.Select("VBELN = '" & m_strAuftragsNummer & "'")
                Dim rowAuftrag() = m_tblAuftraege.Select("Auftragsnummer = '" & m_strAuftragsNummer.TrimStart("0"c) & "'")
                Dim strBSTZD = rowFahrzeug(0)("BSTZD").ToString
                Dim strEQUNR = rowFahrzeug(0)("EQUNR").ToString
                Dim strERNAM = Left(m_strInternetUser, 12)
                Dim strKNZRE = rowFahrzeug(0)("KNRZE").ToString
                Dim strKUNNR = rowFahrzeug(0)("KUNNR").ToString
                Dim strKONZS = m_strCustomer
                Dim strMATNR = ""
                Dim strStorno As String
                Dim strText50 = m_strKunde
                Dim strVBELN = m_strAuftragsNummer
                Dim strYESNO = " "
                Dim strZZFAEDT = ""
                Dim intBetrag As Decimal = 0
                Dim strFreitext = rowFahrzeug(0)("ZZTXT2").ToString

                If rowAuftrag.Length > 0 AndAlso strBSTZD = "0003" Then ' nur Betrag bei Retail
                    m_strBetrag = rowAuftrag(0)("Betrag").ToString
                ElseIf rowAuftrag.Length = 0 Then
                    Throw New Exception("NO_UPDATE_SALESDOCUMENT")
                End If

                strStorno = If(m_blnStorno, "X", "")

                If IsNumeric(m_strBetrag) = True Then
                    intBetrag = CDec(m_strBetrag)
                End If

                'myProxy.setImportParameter("I_KUNNR", strKUNNR)
                'myProxy.setImportParameter("I_KNRZE", strKNZRE)
                'myProxy.setImportParameter("I_KONZS", strKONZS)
                'myProxy.setImportParameter("I_EQUNR", strEQUNR)
                'myProxy.setImportParameter("I_BSTZD", strBSTZD)
                'myProxy.setImportParameter("I_MATNR", strMATNR)
                'myProxy.setImportParameter("I_VBELN", strVBELN)
                'myProxy.setImportParameter("I_ERNAM", strERNAM)
                'myProxy.setImportParameter("I_YESNO", strYESNO)
                'myProxy.setImportParameter("I_STORNO", strStorno)
                'myProxy.setImportParameter("I_TEXT50", strText50)

                Dim ImportString As String = "I_KUNNR,I_KNRZE,I_KONZS,I_EQUNR,I_BSTZD,I_MATNR,I_VBELN,I_ERNAM,I_YESNO,I_STORNO,I_TEXT50"
                Dim ImportValues As Object() = {strKUNNR, strKNZRE, strKONZS, strEQUNR, strBSTZD, strMATNR, strVBELN, strERNAM, strYESNO, strStorno, strText50}

                S.AP.Init("Z_M_Auftragsdaten_Freigabe_Web", ImportString, ImportValues)

                ''autorisierungsdaten
                'myProxy.setImportParameter("I_USER_AUTOR", Left(CType(page.Session("objUser"), Base.Kernel.Security.User).UserName, 30))
                'myProxy.setImportParameter("I_DATUM_AUTOR", Now.ToShortDateString)
                'myProxy.setImportParameter("I_UZEIT_AUTOR", FormatDateTime(Now, DateFormat.LongTime).Replace(":", ""))

                S.AP.SetImportParameter("I_USER_AUTOR", Left(CType(page.Session("objUser"), Base.Kernel.Security.User).UserName, 30))
                S.AP.SetImportParameter("I_DATUM_AUTOR", Now.ToShortDateString)
                S.AP.SetImportParameter("I_UZEIT_AUTOR", FormatDateTime(Now, DateFormat.LongTime).Replace(":", ""))

                If IsDate(m_strFaelligkeit) Then
                    strZZFAEDT = m_strFaelligkeit
                    'myProxy.setImportParameter("I_ZZFAEDT", strZZFAEDT)
                    S.AP.SetImportParameter("I_ZZFAEDT", strZZFAEDT)
                End If

                If Not ZeigeHEZ Then
                    'myProxy.setImportParameter("I_TXT_KOPF_POS", strFreitext)
                    'myProxy.setImportParameter("I_HEZKZ", "")
                    S.AP.SetImportParameter("I_TXT_KOPF_POS", strFreitext)
                    S.AP.SetImportParameter("I_HEZKZ", "")
                Else
                    'myProxy.setImportParameter("I_TXT_KOPF_POS", "")
                    'myProxy.setImportParameter("I_HEZKZ", "X")
                    S.AP.SetImportParameter("I_TXT_KOPF_POS", "")
                    S.AP.SetImportParameter("I_HEZKZ", "X")
                End If
                'myProxy.setImportParameter("I_ANSWT", Replace(CStr(intBetrag), ",", "."))
                S.AP.SetImportParameter("I_ANSWT", CStr(intBetrag))
                'myProxy.callBapi()

                S.AP.Execute()

            End If
        Catch ex As Exception
            Select Case ex.Message.Replace("Execution failed", "").Trim()
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
                Case "NO_TEXTAENDERUNG"
                    m_intStatus = -3407
                    m_strMessage = "Fehler bei der Textänderung"
                Case "NO_STORNO"
                    m_intStatus = -3408
                    m_strMessage = "Auftrag konnte nicht freigegeben werden, die Anforderung wurde bereits storniert"
                Case Else
                    m_intStatus = -9999
                    m_strMessage = ex.Message
            End Select
        End Try
    End Sub


    Public Overloads Sub GiveCarSingle(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page, ByVal Linznr As String)

        'Sucht ein Fahrzeug aus der Menge der Angeforderten Fahrzeuge anhand der Vertragsnummer raus
        m_strClassAndMethod = "FDD_Haendler.GiveCarsSingle"

        m_intStatus = 0
        m_strMessage = ""

        'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Kreditkontrollb_Aendern", m_objApp, m_objUser, page)

        Try
            m_intStatus = 0
            m_strMessage = ""

            'myProxy.setImportParameter("I_KUNNR", m_strKUNNR)
            'myProxy.setImportParameter("I_LIZNR", Linznr)

            'myProxy.callBapi()

            S.AP.InitExecute("Z_M_Kreditkontrollb_Aendern", "I_KUNNR,I_LIZNR", m_strKUNNR, Linznr)

            m_tblAuftraege = New DataTable
            m_tblAuftraege = S.AP.GetExportTable("ES_WEB") 'myProxy.getExportTable("ES_WEB")

            WriteLogEntry(True, "CHASSIS_NUM=" & m_tblAuftraege.Rows(0).Item("Chassis_Num").ToString & ", KONZS=" & m_strKUNNR.TrimStart("0"c) & ", KNRZE=" & m_strFiliale & ", KUNNR=" & Right(m_strCustomer, 5) & ", LIZNR=" & Linznr & ", ZZREFERENZ1=" & m_tblAuftraege.Rows(0).Item("Zzreferenz1").ToString, m_tblAuftraege)

        Catch ex As Exception
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NO_LIZNR"
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
                    m_strMessage = "Unbekannter Fehler!"
            End Select
            If m_intIDSAP > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
            End If

            WriteLogEntry(False, " LIZNR=" & Linznr, m_tblAuftraege)

        End Try

    End Sub

    Public Overloads Sub SetStatus(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page, _
                                    ByVal liznr As String, ByVal haendlernr As String, ByVal kkber As String, ByVal fdat As String)

        m_strClassAndMethod = "FDD_Bank_2.SetStatus"

        'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Equipment_Aktualisieren", m_objApp, m_objUser, page)

        m_intIDSAP = -1

        Try
            m_intStatus = 0
            m_strMessage = ""

            'myProxy.setImportParameter("I_KUNNR", m_strKUNNR)
            'myProxy.setImportParameter("I_KUNNR_ZF", Right("0000000000" & "60" & haendlernr, 10))
            'myProxy.setImportParameter("I_LIZNR", liznr)
            'myProxy.setImportParameter("I_ZZKKBER", kkber)

            S.AP.Init("Z_M_Equipment_Aktualisieren", "I_KUNNR,I_KUNNR_ZF,I_LIZNR,I_ZZKKBER", m_strKUNNR, Right("0000000000" & "60" & haendlernr, 10), liznr, kkber)

            If IsDate(fdat) Then
                'myProxy.setImportParameter("I_ZZFAEDT", fdat)
                S.AP.SetImportParameter("I_ZZFAEDT", fdat)
            End If

            'myProxy.callBapi()
            S.AP.Execute()

            WriteLogEntry(True, "LIZNR=" & liznr & ", KONZS=" & m_strKUNNR.TrimStart("0"c) & ", KNRZE=" & m_strFiliale & ", KUNNR=" & Right(m_strCustomer, 5), m_tblAuftraege)
        Catch ex As Exception
            Select Case ex.Message.Replace("Execution failed", "").Trim()
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
                    m_strMessage = "Unbekannter Fehler!"
            End Select
            If m_intIDSAP > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
            End If

            WriteLogEntry(False, "LIZNR=" & liznr & ", KONZS=" & m_strKUNNR.TrimStart("0"c) & ", KNRZE=" & m_strFiliale & ", KUNNR=" & Right(m_strCustomer, 5), m_tblAuftraege)
        End Try

    End Sub

#End Region

End Class

' ************************************************
' $History: FDD_Bank_2.vb $
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 10.08.10   Time: 15:14
' Updated in $/CKAG/Applications/appffd/Lib
' ITA 3591 fertig
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 13.07.09   Time: 17:44
' Updated in $/CKAG/Applications/appffd/Lib
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 10.07.09   Time: 9:58
' Updated in $/CKAG/Applications/appffd/Lib
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 8.07.09    Time: 15:46
' Updated in $/CKAG/Applications/appffd/Lib
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 15.06.09   Time: 17:08
' Updated in $/CKAG/Applications/appffd/Lib
' ITA 2918
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 12.06.09   Time: 15:23
' Updated in $/CKAG/Applications/appffd/Lib
' ITA 2918
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 7.05.09    Time: 17:23
' Updated in $/CKAG/Applications/appffd/Lib
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 30.04.09   Time: 14:43
' Updated in $/CKAG/Applications/appffd/Lib
' ITA: 2837
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 7.11.08    Time: 14:59
' Updated in $/CKAG/Applications/appffd/Lib
' Finally block hinzugefügt
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:23
' Created in $/CKAG/Applications/appffd/Lib
' 
' *****************  Version 14  *****************
' User: Rudolpho     Date: 23.11.07   Time: 15:55
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' ITA: 1372 OR
' 
' *****************  Version 13  *****************
' User: Uha          Date: 2.07.07    Time: 17:40
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 12  *****************
' User: Rudolpho     Date: 14.06.07   Time: 14:55
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' 
' *****************  Version 11  *****************
' User: Rudolpho     Date: 14.06.07   Time: 14:43
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' Abgleich Portal - Startapplication 14.06.2007
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 13.06.07   Time: 17:03
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' Abgleich Portal - Startapplication 13.06.2005
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 8.06.07    Time: 15:36
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' Abgleich Beyond Compare
' 
' *****************  Version 8  *****************
' User: Fassbenders  Date: 8.06.07    Time: 11:26
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' 
' *****************  Version 7  *****************
' User: Uha          Date: 3.05.07    Time: 18:05
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' Änderungen aus StartApplication vom 02.05.2007 Mittags übernommen
' 
' ************************************************
