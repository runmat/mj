Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements


Public Class Hertz_02
    REM § Status-Report, Kunde: Hertz, BAPI: Z_M_Briefeohnefzg2 (Fill),
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

    Public Sub FillHistory(ByVal strAppID As String, ByVal strSessionID As String, ByVal strAmtlKennzeichen As String, ByVal strFahrgestellnummer As String, ByVal strBriefnummer As String, ByVal strOrdernummer As String)
        m_strClassAndMethod = "Hertz_02.FillHistory"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_HERTZ.SAPProxy_HERTZ()
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            Dim intID As Int32 = -1

            Try
                Dim SAPTable As New SAPProxy_HERTZ.ZDAD_M_WEB_BRIEFDATEN_SIXTTable()

                intID = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Brieflebenslauf_Hertz", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)
                objSAP.Z_M_Brieflebenslauf_Hertz(Right("0000000000" & m_objUser.KUNNR, 10), "1510", UCase(strBriefnummer), UCase(strFahrgestellnummer), UCase(strAmtlKennzeichen), UCase(strOrdernummer), SAPTable)
                objSAP.CommitWork()
                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, True)
                End If

                m_tblHistory = SAPTable.ToADODataTable
                WriteLogEntry(True, "ZZBRIEF=" & UCase(strBriefnummer) & ", ZZFAHRG=" & UCase(strFahrgestellnummer) & ", ZZKENN=" & UCase(strAmtlKennzeichen) & ", ZZREF1= " & UCase(strOrdernummer) & ", KUNNR=" & m_objUser.KUNNR, m_tblHistory, False)
            Catch ex As Exception
                m_intStatus = -2222
                Select Case ex.Message
                    Case "NO_DATA"
                        m_strMessage = "Keine Eingabedaten vorhanden."
                    Case "NO_WEB"
                        m_strMessage = "Keine Web-Tabelle erstellt."
                    Case Else
                        m_strMessage = ex.Message
                End Select
                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "ZZBRIEF=" & UCase(strBriefnummer) & ", ZZFAHRG=" & UCase(strFahrgestellnummer) & ", ZZKENN=" & UCase(strAmtlKennzeichen) & ", ZZREF1= " & UCase(strOrdernummer) & ", KUNNR=" & m_objUser.KUNNR & ", " & Replace(m_strMessage, "<br>", " "), m_tblHistory, False)
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
' $History: Hertz_02.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 14:01
' Created in $/CKAG/Applications/apphertz/Lib
' 
' *****************  Version 5  *****************
' User: Uha          Date: 2.07.07    Time: 17:55
' Updated in $/CKG/Applications/AppHERTZ/AppHERTZWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 4  *****************
' User: Uha          Date: 22.05.07   Time: 13:07
' Updated in $/CKG/Applications/AppHERTZ/AppHERTZWeb/Lib
' Nacharbeiten + Bereinigungen
' 
' ************************************************
