Option Explicit On
Option Strict On
Imports CKG
Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Business
Imports CKG.Base.Common

<Serializable()> Public Class F1_Bank_2
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
            m_strHaendler = Value
            m_tblAuftraege = m_tblAuftraegeAlle.Copy

            Dim i As Int32
            For i = m_tblAuftraege.Rows.Count - 1 To 0 Step -1      'Zeilen, die nicht zu dem gewählten Händler gehören, löschen
                If Not m_tblAuftraege.Rows(i)("HAENDLER_EX").ToString = m_strHaendler Then
                    m_tblAuftraege.Rows(i).Delete()
                End If
            Next
            m_tblAuftraege.AcceptChanges()
            Dim rowTemp As DataRow
            'Es kann immer nur eine Kontingentart z.Z. angezeigt werden!!!
            m_tblAuftraege.Columns.Add("KontingentCode", System.Type.GetType("System.String"))  'Hier werden die Kontingentarten je nach vorheriger Auswahl (bei mehreren Arten) rausgefiltert. (Nicht ausgewählte gelöscht)
            m_tblAuftraege.Columns.Add("ZZKKBERAnzeige", System.Type.GetType("System.String"))  'für die Anzeige des Langtextes JJU2009.03.13
            For i = m_tblAuftraege.Rows.Count - 1 To 0 Step -1
                rowTemp = m_tblAuftraege.Rows(i)
                If Not m_blnZeigeAlle Then
                    'HEZ
                    If m_blnZeigeStandard And (rowTemp("ZZKKBER").ToString = "0004" Or rowTemp("ZZKKBER").ToString = "0005") Then
                        m_tblAuftraege.Rows(i).Delete()
                        GoTo DerNaechsteBitte
                    End If
                    If m_blnZeigeFlottengeschaeft And (rowTemp("ZZKKBER").ToString <> "0004") Then
                        m_tblAuftraege.Rows(i).Delete()
                        GoTo DerNaechsteBitte
                    End If
                    If m_blnZeigeHEZ And (rowTemp("ZZKKBER").ToString <> "0005") Then
                        m_tblAuftraege.Rows(i).Delete()
                        GoTo DerNaechsteBitte
                    End If
                End If
                rowTemp.Item("HAENDLER_EX") = rowTemp.Item("HAENDLER_EX").ToString
                rowTemp("KontingentCode") = rowTemp("ZZKKBER").ToString
                Select Case rowTemp("ZZKKBER").ToString
                    Case "0001"
                        rowTemp("ZZKKBERAnzeige") = "Standard temporär"
                    Case "0002"
                        rowTemp("ZZKKBERAnzeige") = "Standard endgültig"
                    Case "0003"
                        rowTemp("ZZKKBERAnzeige") = "Retail"
                    Case "0004"
                        rowTemp("ZZKKBERAnzeige") = "Erweitertes Zahlungsziel (Delayed Payment) endgültig"
                    Case "0005"
                        rowTemp("ZZKKBERAnzeige") = "Händlereigene Zulassung"
                    Case "0006"
                        rowTemp("ZZKKBERAnzeige") = "KF/KL"
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

    Public Overrides Sub Show()

    End Sub

    Public Overloads Sub Show(ByVal strAppID As String, ByVal strSessionID As String)
        '----------------------------------------------------------------------
        ' Methode: GiveCars
        ' Autor: JJU
        ' Beschreibung: gibt die gesperrten Anforderungen zurück
        ' Erstellt am: 04.03.2009
        ' ITA: 2661
        '----------------------------------------------------------------------

        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Auftragsdaten_STD", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_AG", Right("0000000000" & m_objUser.KUNNR, 10))
            'myProxy.setImportParameter("I_HAENDLER_EX", Haendler)

            'myProxy.callBapi()

            S.AP.InitExecute("Z_M_Auftragsdaten_STD", "I_AG,I_HAENDLER_EX", m_strKUNNR, Haendler)

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

            Dim tblAuftragsUebersichtSAP As DataTable = S.AP.GetExportTable("GT_ANZ") 'myProxy.getExportTable("GT_ANZ")
            Dim rowAuftragsUebersichtSAP As DataRow
            Dim rowTemp As DataRow

            For i = 0 To tblAuftragsUebersichtSAP.Rows.Count - 1
                rowAuftragsUebersichtSAP = tblAuftragsUebersichtSAP.Rows(i)
                If IsNumeric(rowAuftragsUebersichtSAP("Zaehler_01")) And IsNumeric(rowAuftragsUebersichtSAP("Zaehler_02")) And IsNumeric(rowAuftragsUebersichtSAP("Zaehler_04")) And IsNumeric(rowAuftragsUebersichtSAP("Zaehler_05")) Then
                    If CInt(rowAuftragsUebersichtSAP("Zaehler_01")) + CInt(rowAuftragsUebersichtSAP("Zaehler_02")) + CInt(rowAuftragsUebersichtSAP("Zaehler_04")) + CInt(rowAuftragsUebersichtSAP("Zaehler_05")) > 0 Then
                        rowTemp = m_tblAuftragsUebersicht.NewRow
                        rowTemp("Händlernummer") = rowAuftragsUebersichtSAP("HAENDLER_EX").ToString
                        rowTemp("Händlername") = rowAuftragsUebersichtSAP("Name1_Zf")
                        rowTemp("Anzahl Standard endgültig") = CInt(rowAuftragsUebersichtSAP("Zaehler_02"))
                        rowTemp("Anzahl Standard temporär") = CInt(rowAuftragsUebersichtSAP("Zaehler_01"))
                        rowTemp("Anzahl Flottengeschäft") = CInt(rowAuftragsUebersichtSAP("Zaehler_04"))
                        rowTemp("Anzahl HEZ") = CInt(rowAuftragsUebersichtSAP("Zaehler_05"))
                        rowTemp("Anzahl KF/KL") = CInt(rowAuftragsUebersichtSAP("Zaehler_06"))
                        m_tblAuftragsUebersicht.Rows.Add(rowTemp)
                    End If
                End If
            Next

            m_tblRaw = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")
            m_tblAuftraegeAlle = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")
            m_tblAuftraegeAlle.Columns.Add("InAutorisierung", System.Type.GetType("System.Boolean"))
            m_tblAuftraegeAlle.Columns.Add("Initiator", System.Type.GetType("System.String"))
            m_tblAuftraegeAlle.Columns.Add("Abrufgrund", System.Type.GetType("System.String"))

            For Each rowTemp In m_tblAuftraegeAlle.Rows
                Dim intAutID As Int32 = 0
                Dim strInitiator As String = ""

                m_objApp.CheckForPendingAuthorization(CInt(m_strAppID), rowTemp("HAENDLER_EX").ToString, rowTemp("ZZFAHRG").ToString, m_objUser.IsTestUser, strInitiator, intAutID)
                rowTemp("Initiator") = strInitiator
                If intAutID > 0 Then
                    rowTemp("InAutorisierung") = True
                Else
                    rowTemp("InAutorisierung") = False
                End If

                rowTemp("VBELN") = Right(rowTemp("VBELN").ToString, 10).TrimStart("0"c)
                rowTemp("ZZANFDT") = FormatDateTime(CDate(rowTemp("ZZANFDT").ToString), DateFormat.ShortDate)

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

                rowTemp("Abrufgrund") = getAbrufgrund(rowTemp("AUGRU").ToString)
            Next
            Dim tmpRows As DataRow()

            tmpRows = m_tblRaw.Select("ZZKKBER = '0003'")
            If tmpRows.Length > 0 Then
                Dim i2 As Integer

                For i2 = 0 To tmpRows.Length - 1
                    Dim iLoeschFlag As Integer = 0
                    tmpRows(i2).BeginEdit()
                    If tmpRows(i2).Item("ZZKKBER").ToString = "0003" Then
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

    Public Overrides Sub Change()

    End Sub

    Public Overloads Sub Change(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
        '----------------------------------------------------------------------
        ' Methode: Change
        ' Autor: JJU
        ' Beschreibung: gibt einen gesperrten auftrag frei oder storniert ihn
        ' Erstellt am: 11.03.2009
        ' ITA: 2675
        '----------------------------------------------------------------------

        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Auftragsdaten_Freigabe_STD", m_objApp, m_objUser, page)
            S.AP.Init("Z_M_Auftragsdaten_Freigabe_STD")


            If m_strAuftragsNummer.Trim("0"c).Length = 0 Or (Not m_strAuftragsNummer.Length = 10) Then
                m_intStatus = -1301
                m_strMessage = "Keine gültige Auftragsnummer übergeben."
            Else
                Dim rowFahrzeug() As DataRow = m_tblRaw.Select("VBELN = '" & m_strAuftragsNummer & "'")
                Dim rowAuftrag() As DataRow = m_tblAuftraege.Select("VBELN = '" & m_strAuftragsNummer.TrimStart("0"c) & "'")

                'myProxy.setImportParameter("I_AG", Right("0000000000" & m_objUser.KUNNR, 10))
                'myProxy.setImportParameter("I_HAENDLER_EX", rowFahrzeug(0)("HAENDLER_EX").ToString)
                'myProxy.setImportParameter("I_EQUNR", rowFahrzeug(0)("EQUNR").ToString)
                'myProxy.setImportParameter("I_VBELN", m_strAuftragsNummer)
                'myProxy.setImportParameter("I_ERNAM", Right(m_objUser.UserName, 12))

                S.AP.SetImportParameter("I_AG", m_strKUNNR)
                S.AP.SetImportParameter("I_HAENDLER_EX", rowFahrzeug(0)("HAENDLER_EX").ToString)
                S.AP.SetImportParameter("I_EQUNR", rowFahrzeug(0)("EQUNR").ToString)
                S.AP.SetImportParameter("I_VBELN", m_strAuftragsNummer)
                S.AP.SetImportParameter("I_ERNAM", Right(m_objUser.UserName, 12))

                Dim strStorno As String = " "
                If m_blnStorno Then
                    strStorno = "X"
                End If

                'myProxy.setImportParameter("I_STORNO", strStorno)
                'myProxy.setImportParameter("I_TEXT50", m_strKunde)

                S.AP.SetImportParameter("I_STORNO", strStorno)
                S.AP.SetImportParameter("I_TEXT50", m_strKunde)

                If IsDate(m_strFaelligkeit) Then
                    'myProxy.setImportParameter("I_ZZFAEDT", m_strFaelligkeit)
                    S.AP.SetImportParameter("I_ZZFAEDT", m_strFaelligkeit)
                End If
            End If

            'myProxy.callBapi()
            S.AP.Execute()

        Catch ex As Exception
            m_intStatus = -9999
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
                Case "ZDADCREDITLIMIT_SPERRE"
                    m_intStatus = -3407
                    m_strMessage = "Händler ist gesperrt"
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Fehler: " & ex.Message
            End Select
        End Try
    End Sub

    Private Function getAbrufgrund(ByVal kuerzel As String) As String
        Dim cn As New SqlClient.SqlConnection
        Dim cmdAg As SqlClient.SqlCommand
        Dim dsAg As DataSet
        Dim adAg As SqlClient.SqlDataAdapter
        Dim dr As SqlClient.SqlDataReader
        Dim sReturn As String = ""
        Try

            cn.ConnectionString = ConfigurationManager.AppSettings("Connectionstring")
            cn.Open()
            dsAg = New DataSet()
            adAg = New SqlClient.SqlDataAdapter()
            cmdAg = New SqlClient.SqlCommand("SELECT " & _
                                            "[WebBezeichnung]" & _
                                            "FROM CustomerAbrufgruende " & _
                                            "WHERE " & _
                                            "CustomerID =' " & m_objUser.Customer.CustomerId.ToString & "' AND " & _
                                            "SapWert='" & kuerzel & "'", cn)
            cmdAg.CommandType = CommandType.Text
            dr = cmdAg.ExecuteReader()

            If dr.Read() = True Then
                If dr.IsDBNull(0) Then
                    sReturn = String.Empty
                Else
                    sReturn = CStr(dr.Item("WebBezeichnung"))
                End If
            End If
        Catch ex As Exception

            Throw ex

        Finally
            cn.Close()
        End Try
        Return sReturn
    End Function

    Public Sub checkAutorisierungsEintraegeForFreigabeGesperrterAuftraege(ByRef KompletterAutorisierungsBestand As DataTable, ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)

        m_strClassAndMethod = "F1_Bank_2.checkAutorisierungsEintraegeForFreigabeGesperrterAuftraege"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            'Übergabetabelle definieren
            Dim sapTable As DataTable = S.AP.GetImportTableWithInit("Z_M_AUTORISIERTE_AUFTRAEGE.GT_WEB")
            'Dim dataColumns(1) As DataColumn
            'dataColumns(0) = New DataColumn("KUNNR", System.Type.GetType("System.String"))
            'dataColumns(1) = New DataColumn("CHASSIS_NUM", System.Type.GetType("System.String"))
            'sapTable.Columns.AddRange(dataColumns)
            'sapTable.AcceptChanges()


            'tabelle mit allen Einträgen aus der Autorisierung befüllen die Change02 sind
            'Change02 = Freigabe gesperrter Aufträge

            Dim tmpRows() As DataRow

            tmpRows = KompletterAutorisierungsBestand.Select("AppName='Change02'")
            If tmpRows.Length = 0 Then
                m_blnGestartet = False
                Exit Sub
            Else
                Dim tmpItemArray(1) As Object
                For Each tmpRow As DataRow In tmpRows
                    tmpItemArray(0) = m_strKUNNR
                    tmpItemArray(1) = tmpRow.Item("ProcessReference")
                    sapTable.Rows.Add(tmpItemArray)
                Next
                sapTable.AcceptChanges()
            End If

            Try

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_AUTORISIERTE_AUFTRAEGE", m_objApp, m_objUser, page)

                'myProxy.callBapi()

                sapTable = S.AP.GetExportTableWithExecute("GT_WEB") 'myProxy.getExportTable("GT_WEB")

                If sapTable.Rows.Count = 0 Then
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
                m_strMessage = ex.Message

                WriteLogEntry(False, "Fehler in F1_Bank_2, BAPI=Z_M_AUTORISIERTE_AUFTRAEGE", sapTable)
            Finally

            End Try
        End If

    End Sub

    '§§§JVE 07.09.05 <end>
#End Region

    Public Sub FixSerializedSettings(newInstanceUsingCurrentSettings As F1_Bank_2)
        m_BizTalkSapConnectionString = newInstanceUsingCurrentSettings.m_BizTalkSapConnectionString
        m_objApp = newInstanceUsingCurrentSettings.m_objApp
        m_objLogApp = newInstanceUsingCurrentSettings.m_objLogApp
        m_objUser = newInstanceUsingCurrentSettings.m_objUser

    End Sub

End Class
' ************************************************
' $History: F1_Bank_2.vb $
' 
' *****************  Version 13  *****************
' User: Rudolpho     Date: 24.02.10   Time: 14:41
' Updated in $/CKAG/Applications/AppF1/lib
' ITA: 3223
' 
' *****************  Version 12  *****************
' User: Jungj        Date: 20.04.09   Time: 9:15
' Updated in $/CKAG/Applications/AppF1/lib
' bugfix freigabe
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 9.04.09    Time: 14:45
' Updated in $/CKAG/Applications/AppF1/lib
' Nachbesserungen dokumentenversand
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 3.04.09    Time: 9:05
' Updated in $/CKAG/Applications/AppF1/lib
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 30.03.09   Time: 9:30
' Updated in $/CKAG/Applications/AppF1/lib
' ITA 2675 nachbesserungen
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 25.03.09   Time: 10:21
' Updated in $/CKAG/Applications/AppF1/lib
' ITA 2741, 2670
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 16.03.09   Time: 17:51
' Updated in $/CKAG/Applications/AppF1/lib
' ITA 2675
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 16.03.09   Time: 10:26
' Updated in $/CKAG/Applications/AppF1/lib
' ita 2685
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 13.03.09   Time: 14:18
' Updated in $/CKAG/Applications/AppF1/lib
' ITA 2675 unfertig
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 13.03.09   Time: 9:18
' Updated in $/CKAG/Applications/AppF1/lib
' ITA 2675 unfertig
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 12.03.09   Time: 17:27
' Updated in $/CKAG/Applications/AppF1/lib
' ITA 2675 unfertig
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 10.03.09   Time: 10:48
' Updated in $/CKAG/Applications/AppF1/lib
' ITA 2675 unfertig
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 9.03.09    Time: 17:49
' Created in $/CKAG/Applications/AppF1/lib
' ITA 2675 unfertig
'
' ************************************************
