Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Public Class SixtLease_04
    REM § Status-Report, Kunde: Arval, BAPI: Z_M_Briefeohnefzg2 (Fill),
    REM § BAPI: Z_M_Brieflebenslauf (FillHistory),
    REM § Ausgabetabelle per Zuordnung in Web-DB (Fill), Direkte Rückgabe (FillHistory).

    Inherits Base.Business.DatenimportBase ' FFD_Bank_Datenimport

#Region " Declarations"
    Private m_strBriefnummer As String
    Private m_datEingangsdatumVon As DateTime
    Private m_datEingangsdatumBis As DateTime
    Private m_strFahrgestellnummer As String
    Private m_strHaendlerID As String
    Private m_tblHistory As DataTable
    Private m_tblResultModelle As DataTable
#End Region

#Region " Properties"
    Public ReadOnly Property ResultModelle() As DataTable
        Get
            Return m_tblResultModelle
        End Get
    End Property

    Public ReadOnly Property History() As DataTable
        Get
            Return m_tblHistory
        End Get
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Overrides Sub Fill()
        'Fill(m_strAppID, m_strSessionID, m_strBriefnummer, m_datEingangsdatumVon, m_datEingangsdatumBis, m_strFahrgestellnummer, m_strHaendlerID)
    End Sub

    'Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal strBriefnummer As String, ByVal datEingangsdatumVon As DateTime, ByVal datEingangsdatumBis As DateTime, ByVal strFahrgestellnummer As String, ByVal strHaendlerID As String)
    '    m_strClassAndMethod = "Arval_07.FILL"
    '    m_strAppID = strAppID
    '    m_strSessionID = strSessionID
    '    If Not m_blnGestartet Then
    '        m_blnGestartet = True

    '        Dim objSAP As New SAPProxy_SIXTL.SAPProxy_SIXTL()            'SAPProxy_SIXT_01.SAPProxy_SIXT_01()
    '        objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
    '        objSAP.Connection.Open()

    '        Dim intID As Int32 = -1

    '        Try
    '            Dim SAPTable As New SAPProxy_SIXTL.ZDAD_M_WEB_REPORTING_SIXTTable() ' SAPProxy_SIXT_01.ZDAD_M_WEB_REPORTING_SIXTTable()
    '            Dim strDatTempVon As String
    '            If IsDate(datEingangsdatumVon) Then
    '                strDatTempVon = MakeDateSAP(datEingangsdatumVon)
    '                If strDatTempVon = "10101" Then
    '                    strDatTempVon = "|"
    '                End If
    '            Else
    '                strDatTempVon = "|"
    '            End If
    '            Dim strDatTempBis As String
    '            If IsDate(datEingangsdatumBis) Then
    '                strDatTempBis = MakeDateSAP(datEingangsdatumBis)
    '                If strDatTempBis = "10101" Then
    '                    strDatTempBis = "|"
    '                End If
    '            Else
    '                strDatTempBis = "|"
    '            End If

    '            intID = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Briefeohnefzg2", strAppID, strSessionID)
    '            objSAP.Z_M_Briefeohnefzg2(strBriefnummer, strDatTempBis, strDatTempVon, strFahrgestellnummer, "|", Right("0000000000" & m_objUser.KUNNR, 10), SAPTable)
    '            objSAP.CommitWork()
    '            If intID > -1 Then
    '                m_objlogApp.WriteEndDataAccessSAP(intID, True)
    '            End If

    '            Dim tblTemp2 As DataTable = SAPTable.ToADODataTable

    '            tblTemp2.Columns.Add("Modell", System.Type.GetType("System.String"))
    '            tblTemp2.Columns.Add("VM", System.Type.GetType("System.String"))
    '            Dim i As Int32
    '            For i = 0 To tblTemp2.Rows.Count - 1
    '                Dim objZulassungsVorbelegung As ZulassungsVorbelegung
    '                objZulassungsVorbelegung = New ZulassungsVorbelegung(CStr(tblTemp2.Rows(i)("ZZFAHRG")), Now, m_objApp.Connectionstring)
    '                tblTemp2.Rows(i).AcceptChanges()
    '                tblTemp2.Rows(i)("Modell") = objZulassungsVorbelegung.Modell
    '                If objZulassungsVorbelegung.CheckVMBoolean(CStr(tblTemp2.Rows(i)("ZZREF1")), CStr(tblTemp2.Rows(i)("FLEET_VIN"))) Then
    '                    tblTemp2.Rows(i)("VM") = "Ja"
    '                Else
    '                    tblTemp2.Rows(i)("VM") = "Nein"
    '                End If
    '                tblTemp2.Rows(i).AcceptChanges()
    '            Next

    '            CreateOutPut(tblTemp2, strAppID)

    '            m_tblResultModelle = New DataTable()
    '            m_tblResultModelle.Columns.Add("Modell", System.Type.GetType("System.String"))
    '            m_tblResultModelle.Columns.Add("Fahrzeuge", System.Type.GetType("System.Int32"))
    '            m_tblResultModelle.Columns.Add("AppID", System.Type.GetType("System.String"))

    '            Dim vwTemp As DataView = tblTemp2.DefaultView

    '            vwTemp.Sort = "Modell"
    '            Dim strModellTest As String = "Startwert"
    '            Dim intFahrzeuge As Int32 = 0
    '            For i = 0 To vwTemp.Count - 1
    '                If Not CStr(vwTemp.Item(i)("Modell")) = strModellTest Then
    '                    If intFahrzeuge > 0 Then
    '                        Dim rowTemp As DataRow = m_tblResultModelle.NewRow
    '                        rowTemp("Modell") = strModellTest
    '                        rowTemp("Fahrzeuge") = intFahrzeuge
    '                        rowTemp("AppID") = strAppID
    '                        m_tblResultModelle.Rows.Add(rowTemp)
    '                    End If
    '                    strModellTest = CStr(vwTemp.Item(i)("Modell"))
    '                    intFahrzeuge = 1
    '                Else
    '                    intFahrzeuge += 1
    '                End If
    '            Next
    '            If intFahrzeuge > 0 Then
    '                Dim rowTemp As DataRow = m_tblResultModelle.NewRow
    '                rowTemp("Modell") = strModellTest
    '                rowTemp("Fahrzeuge") = intFahrzeuge
    '                m_tblResultModelle.Rows.Add(rowTemp)
    '            End If
    '            WriteLogEntry(True, "ZZERDAT_VON=" & datEingangsdatumVon.ToShortDateString & ", ZZERDAT_BIS=" & datEingangsdatumBis.ToShortDateString & ", KUNNR=" & m_objUser.KUNNR & ", ZZHAENDLER=, ZFAHRG=" & strFahrgestellnummer, m_tblResult, False)
    '        Catch ex As Exception
    '            m_intStatus = -3333
    '            Select Case ex.Message
    '                Case "ERR_NO_KRIT"
    '                    m_strMessage = "Keine Suchkriterien eingegeben."
    '                Case "ERR_INV_ERDAT"
    '                    m_strMessage = "Geben Sie bitte ein gültiges Eingangsdatum ein!"
    '                Case "ERR_INV_BRIEFNR"
    '                    m_strMessage = "Ungültige Brief-Nr."
    '                Case "ERR_INV_KUNNR"
    '                    m_strMessage = "Ungültige Kundennummer."
    '                Case "ERR_INV_FAHRG"
    '                    m_strMessage = "Ungültige Fahrgestellnummer."
    '                Case "ERR_INV_HAENDLER"
    '                    m_strMessage = "Ungültiger Händler."
    '                Case "ERR_NO_DATA"
    '                    m_strMessage = "Keine Daten gefunden."
    '                Case Else
    '                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
    '            End Select
    '            If intID > -1 Then
    '                m_objlogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
    '            End If
    '            WriteLogEntry(False, "ZZERDAT_VON=" & datEingangsdatumVon.ToShortDateString & ", ZZERDAT_BIS=" & datEingangsdatumBis.ToShortDateString & ", KUNNR=" & m_objUser.KUNNR & ", ZZHAENDLER=, ZFAHRG=" & strFahrgestellnummer & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
    '        Finally
    '            If intID > -1 Then
    '                m_objlogApp.WriteStandardDataAccessSAP(intID)
    '            End If

    '            objSAP.Connection.Close()
    '            objSAP.Dispose()

    '            m_blnGestartet = False
    '        End Try
    '    End If
    'End Sub

    'Public Sub FillHistory(ByVal strAppID As String, ByVal strSessionID As String, ByVal strAmtlKennzeichen As String, ByVal strFahrgestellnummer As String, ByVal strBriefnummer As String, ByVal strOrdernummer As String)
    '    m_strClassAndMethod = "Arval_07.FillHistory"
    '    m_strAppID = strAppID
    '    m_strSessionID = strSessionID
    '    If Not m_blnGestartet Then
    '        m_blnGestartet = True

    '        Dim objSAP As New SAPProxy_ARVAL.SAPProxy_ARVAL() ' SAPProxy_SIXT_01.SAPProxy_SIXT_01()
    '        objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
    '        objSAP.Connection.Open()

    '        Dim intID As Int32 = -1

    '        Try
    '            Dim SAPTable As New SAPProxy_ARVAL.ZDAD_M_WEB_BRIEFDATEN_SIXTTable()

    '            intID = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Brieflebenslauf", strAppID, strSessionID)
    '            '§§§ JVE 19.12.2005 <begin>
    '            'BAPI wurde umbenannt
    '            'objSAP.Z_M_Brieflebenslauf(Right("0000000000" & m_objUser.KUNNR, 10), "1510", UCase(strBriefnummer), UCase(strFahrgestellnummer), UCase(strAmtlKennzeichen), UCase(strOrdernummer), SAPTable)
    '            objSAP.Z_M_Brieflebenslauf_Arval(Right("0000000000" & m_objUser.KUNNR, 10), "1510", UCase(strBriefnummer), UCase(strFahrgestellnummer), UCase(strAmtlKennzeichen), UCase(strOrdernummer), SAPTable)

    '            '§§§ JVE 19.12.2005 <end>
    '            objSAP.CommitWork()
    '            If intID > -1 Then
    '                m_objlogApp.WriteEndDataAccessSAP(intID, True)
    '            End If

    '            m_tblHistory = SAPTable.ToADODataTable
    '            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ", ZZBRIEF=" & UCase(strBriefnummer) & ", ZFAHRG=" & UCase(strFahrgestellnummer) & ", ZZREF1=" & UCase(strOrdernummer) & ", ZZKENN=" & UCase(strAmtlKennzeichen), m_tblHistory, False)
    '        Catch ex As Exception
    '            m_intStatus = -2222
    '            Select Case ex.Message
    '                Case "NO_DATA"
    '                    m_strMessage = "Keine Daten gefunden."
    '                Case "NO_WEB"
    '                    m_strMessage = "Keine Web-Tabelle erstellt."
    '                Case Else
    '                    m_strMessage = ex.Message
    '            End Select
    '            If intID > -1 Then
    '                m_objlogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
    '            End If
    '            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", ZZBRIEF=" & UCase(strBriefnummer) & ", ZFAHRG=" & UCase(strFahrgestellnummer) & ", ZZREF1=" & UCase(strOrdernummer) & ", ZZKENN=" & UCase(strAmtlKennzeichen) & ", " & Replace(m_strMessage, "<br>", " "), m_tblHistory, False)
    '        Finally
    '            If intID > -1 Then
    '                m_objlogApp.WriteStandardDataAccessSAP(intID)
    '            End If

    '            objSAP.Connection.Close()
    '            objSAP.Dispose()

    '            m_blnGestartet = False
    '        End Try
    '    End If
    'End Sub
#End Region
End Class

' ************************************************
' $History: SixtLease_04.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 17:27
' Created in $/CKAG/Applications/appsixtl/Lib
' 
' *****************  Version 4  *****************
' User: Uha          Date: 8.03.07    Time: 13:42
' Updated in $/CKG/Applications/AppSIXTL/AppSIXTLWeb/Lib
' 
' ************************************************
