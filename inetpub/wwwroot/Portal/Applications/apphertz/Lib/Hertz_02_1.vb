Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel

Public Class Hertz_02_1
    REM § Status-Report, Kunde: Hertz, BAPI: Z_M_Brieflebenslauf_Tuete,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.ReportBase

#Region " Declarations"
    Private m_strFahrgestellnummer As String
#End Region

#Region " Properties"

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Overrides Sub Fill()
        'FillMe(m_strAppID, m_strSessionID, "")
    End Sub

    Public Overloads Sub FillMe(ByVal strAppID As String, ByVal strSessionID As String, ByVal strFahrgestellnummer As String)
        m_strClassAndMethod = "Hertz_02_1.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_HERTZ.SAPProxy_HERTZ() ' SAPProxy_SIXT_01.SAPProxy_SIXT_01()
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            Dim intID As Int32 = -1
            Dim strKUNNR As String = Right("0000000000" & m_objUser.KUNNR, 10)
            Dim strZZFAHRG As String = strFahrgestellnummer

            Try
                Dim SAPTable As New SAPProxy_HERTZ.ZDAD_M_WEB_TUETENDATEN_SIXTTable() ' SAPProxy_SIXT_01.ZDAD_M_WEB_TUETENDATEN_SIXTTable()

                intID = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Brieflebenslauf_Tuetehertz", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)
                Dim strVKORG As String = "1510"
                objSAP.Z_M_Brieflebenslauf_Tuetehertz(strKUNNR, strVKORG, strZZFAHRG, SAPTable)
                objSAP.CommitWork()
                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, True)
                End If
                m_tblResult = SAPTable.ToADODataTable
                WriteLogEntry(True, "KUNNR=" & strKUNNR & ", ZZFAHRG=" & strZZFAHRG, m_tblResult, False)
            Catch ex As Exception
                m_intStatus = -5555
                Select Case ex.Message
                    Case "NO_TUETE"
                        m_strMessage = "Es konnte keine Tüte gefunden werden."
                    Case "NO_HERTZ"
                        m_strMessage = "Dieser BAPI soll nur Hertz genutzt werden."
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "KUNNR=" & strKUNNR & ", ZZFAHRG=" & strZZFAHRG & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
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
' $History: Hertz_02_1.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 14:01
' Created in $/CKAG/Applications/apphertz/Lib
' 
' *****************  Version 7  *****************
' User: Uha          Date: 2.07.07    Time: 17:55
' Updated in $/CKG/Applications/AppHERTZ/AppHERTZWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 6  *****************
' User: Uha          Date: 22.05.07   Time: 13:07
' Updated in $/CKG/Applications/AppHERTZ/AppHERTZWeb/Lib
' Nacharbeiten + Bereinigungen
' 
' ************************************************
