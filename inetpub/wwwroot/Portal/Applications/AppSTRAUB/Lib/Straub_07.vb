Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Straub_07
    REM § Status-Report, Kunde: Sixt, BAPI: Z_M_Abm_Abgemeldete_Kfz,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase ' FFD_Bank_Datenimport

#Region " Declarations"
    Private m_datAbmeldedatumVon As DateTime
    Private m_datAbmeldedatumBis As DateTime
    Private m_strFahrgestellnummer As String
    Private m_strKennzeichen As String
#End Region

#Region " Properties"

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Overrides Sub Fill()
        FILL(m_strAppID, m_strSessionID, m_datAbmeldedatumVon, m_datAbmeldedatumBis, m_strKennzeichen, m_strFahrgestellnummer)
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal datAbmeldedatumVon As DateTime, ByVal datAbmeldedatumBis As DateTime, ByVal strKennzeichen As String, ByVal strFahrgestellnummer As String)
        m_strClassAndMethod = "Straub_07.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True
            '§§§JVE 20.09.2005 <begin>
            'BAPI Z_M_Abm_Abgemeldete_Kfz wurde um 2 Inputparameter erweitert, für ARVAL.
            'Da es ein allgemeines BAPI ist (für HERTZ, ARVAL, ANC, SIXT) wurde es in den allg. Proxy "SAPProxy_Base" verschoben
            'Muß dann auch (s.u.) in den Kundenprojekten angepasst werden.
            'Anschließend müssen die Proxys der einzelnen Kunden um dieses BAPI "bereinigt" werden!

            Dim objSAP As New SAPProxy_Base.SAPProxy_Base() ' SAPProxy_SIXT.SAPProxy_SIXT()
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            Dim intID As Int32 = -1

            Try
                Dim SAPTable As New SAPProxy_Base.ZDAD_M_WEB_REPORTING_SIXTTable() ' .ZDAD_M_WEB_REPORTING_SIXTTable()
                Dim strDatTempVon As String
                If IsDate(datAbmeldedatumVon) Then
                    strDatTempVon = HelpProcedures.MakeDateSAP(datAbmeldedatumVon)
                    If strDatTempVon = "10101" Then
                        strDatTempVon = "|"
                    End If
                Else
                    strDatTempVon = "|"
                End If
                Dim strDatTempBis As String
                If IsDate(datAbmeldedatumBis) Then
                    strDatTempBis = HelpProcedures.MakeDateSAP(datAbmeldedatumBis)
                    If strDatTempBis = "10101" Then
                        strDatTempBis = "|"
                    End If
                Else
                    strDatTempBis = "|"
                End If

                Dim strAnzahl As String
                intID = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Abm_Abgemeldete_Kfz", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)
                objSAP.Z_M_Abm_Abgemeldete_Kfz(strDatTempVon, strDatTempBis, Right("0000000000" & m_objUser.KUNNR, 10), "", "", "", strFahrgestellnummer, strKennzeichen, strAnzahl, SAPTable)
                '§§§JVE 20.09.2005 <end>
                objSAP.CommitWork()
                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, True)
                End If

                Dim tblTemp2 As DataTable = SAPTable.ToADODataTable

                CreateOutPut(tblTemp2, strAppID)
                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ", TIDNR=, ZFAHRG=" & strFahrgestellnummer & ", ZZKENN=" & strKennzeichen & ", ABMDATAB=" & datAbmeldedatumVon.ToShortDateString & ", ABMDATBI=" & datAbmeldedatumBis.ToShortDateString, m_tblResult, False)
            Catch ex As Exception
                m_intStatus = -9999
                m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", TIDNR=, ZFAHRG=" & strFahrgestellnummer & ", ZZKENN=" & strKennzeichen & ", ABMDATAB=" & datAbmeldedatumVon.ToShortDateString & ", ABMDATBI=" & datAbmeldedatumBis.ToShortDateString & ", " & ex.Message, m_tblResult, False)
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
#End Region
End Class

' ************************************************
' $History: Straub_07.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 17:38
' Created in $/CKAG/Applications/AppSTRAUB/Lib
' 
' *****************  Version 4  *****************
' User: Uha          Date: 3.07.07    Time: 9:51
' Updated in $/CKG/Applications/AppSTRAUB/AppSTRAUBWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 3  *****************
' User: Uha          Date: 8.03.07    Time: 14:18
' Updated in $/CKG/Applications/AppSTRAUB/AppSTRAUBWeb/Lib
' 
' ************************************************
