Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Public Class ALDA_07
    REM § BAPI: Z_M_Fahrzeugbriefhistorie (FillHistory),
    REM § Direkte Rückgabe (FillHistory).

    Inherits Base.Business.DatenimportBase ' FFD_Bank_Datenimport

#Region " Declarations"
    Private m_strBriefnummer As String
    Private m_datEingangsdatumVon As DateTime
    Private m_datEingangsdatumBis As DateTime
    Private m_strFahrgestellnummer As String
    Private m_strHaendlerID As String
    Private m_tblHistory As DataTable
    Private m_tblResultModelle As DataTable
    Private m_intResultCount As Integer
#End Region

#Region " Properties"
    Public ReadOnly Property ResultCount() As Integer
        Get
            Return m_intResultCount
        End Get
    End Property

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
        FILL(m_strAppID, m_strSessionID, m_strBriefnummer, m_datEingangsdatumVon, m_datEingangsdatumBis, m_strFahrgestellnummer, m_strHaendlerID)
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal strBriefnummer As String, ByVal datEingangsdatumVon As DateTime, ByVal datEingangsdatumBis As DateTime, ByVal strFahrgestellnummer As String, ByVal strHaendlerID As String)
    End Sub

    Public Sub FillHistory(ByVal strAppID As String, ByVal strSessionID As String, ByVal strAmtlKennzeichen As String, ByVal strFahrgestellnummer As String, ByVal strBriefnummer As String, ByVal strOrdernummer As String)
        m_strClassAndMethod = "ALDA_07.FillHistory"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_ALDA.SAPProxy_ALDA()
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            Dim intID As Int32 = -1

            Try
                Dim SAPTable As New SAPProxy_ALDA.ZDAD_M_WEB_BRIEFDATEN_ALDBPTable()
                Dim SAPTableFleet As New SAPProxy_ALDA.ZFLEETTable()

                intID = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Fahrzeugbriefhistorie", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)
                '20.02.2006 TODO
                objSAP.Z_M_Fahrzeugbriefhistorie(Right("0000000000" & m_objUser.KUNNR, 10), UCase(strOrdernummer), UCase(strFahrgestellnummer), UCase(strAmtlKennzeichen), "", m_intResultCount, SAPTableFleet, SAPTable)
                objSAP.CommitWork()
                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, True)
                End If

                m_tblHistory = SAPTable.ToADODataTable

                If m_intResultCount > 1 Then
                    m_tblResult = SAPTableFleet.ToADODataTable
                    m_tblResult.Columns.Add("DISPLAY")
                    If m_tblResult.Rows.Count > 0 Then
                        Dim row As DataRow
                        For Each row In m_tblResult.Rows
                            Dim strTemp As String = CStr(row("ZZFAHRG")) & ", " & CStr(row("TIDNR")) & ", " & CStr(row("ZZKENN"))
                            row("DISPLAY") = strTemp
                        Next
                    End If
                End If
                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ", ZZBRIEF=" & UCase(strBriefnummer) & ", ZFAHRG=" & UCase(strFahrgestellnummer) & ", ZZREF1=" & UCase(strOrdernummer) & ", ZZKENN=" & UCase(strAmtlKennzeichen) & ", Count=" & m_intResultCount.ToString, m_tblHistory, False)
            Catch ex As Exception
                m_intResultCount = 0
                m_intStatus = -2222
                Select Case ex.Message
                    Case "NO_DATA"
                        m_strMessage = "Keine Daten gefunden."
                    Case "NO_WEB"
                        m_strMessage = "Keine Web-Tabelle erstellt."
                    Case Else
                        m_strMessage = ex.Message
                End Select
                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", ZZBRIEF=" & UCase(strBriefnummer) & ", ZFAHRG=" & UCase(strFahrgestellnummer) & ", ZZREF1=" & UCase(strOrdernummer) & ", ZZKENN=" & UCase(strAmtlKennzeichen) & ", " & Replace(m_strMessage, "<br>", " "), m_tblHistory, False)
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
' $History: ALDA_07.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 17.04.08   Time: 15:29
' Created in $/CKAG/Applications/appalda/Lib
' 
' *****************  Version 5  *****************
' User: Uha          Date: 2.07.07    Time: 15:43
' Updated in $/CKG/Applications/AppALDA/AppALDAWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 4  *****************
' User: Uha          Date: 6.03.07    Time: 13:21
' Updated in $/CKG/Applications/AppALDA/AppALDAWeb/Lib
' 
' ************************************************
