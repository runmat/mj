Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Common

Public Class SixtLease_11
    REM § Status-Report, Kunde: Sixt, BAPI: Z_M_Briefeohnefzg2 (Fill),
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
        FILL(m_strAppID, m_strSessionID, m_strBriefnummer, m_datEingangsdatumVon, m_datEingangsdatumBis, m_strFahrgestellnummer, m_strHaendlerID)
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal strBriefnummer As String, ByVal datEingangsdatumVon As DateTime, ByVal datEingangsdatumBis As DateTime, ByVal strFahrgestellnummer As String, ByVal strHaendlerID As String)

    End Sub

    Public Sub FillHistory(ByVal strAppID As String, ByVal strSessionID As String, ByVal strAmtlKennzeichen As String, ByVal strFahrgestellnummer As String, ByVal strBriefnummer As String, ByVal strOrdernummer As String)
        m_strClassAndMethod = "Sixt_B02.FillHistory"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            'Dim intID As Int32 = -1

            Try
                'intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Brieflebenslauf_Arval", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxyNoPage("Z_M_Brieflebenslauf_Arval", m_objApp, m_objUser)

                'myProxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
                'myProxy.setImportParameter("I_VKORG", "1510")
                'myProxy.setImportParameter("I_ZZKENN", UCase(strAmtlKennzeichen))
                'myProxy.setImportParameter("I_ZZFAHRG", UCase(strFahrgestellnummer))
                'myProxy.setImportParameter("I_ZZBRIEF", UCase(strBriefnummer))
                'myProxy.setImportParameter("I_ZZREF1", UCase(strOrdernummer))

                'myProxy.callBapiNoPage(m_strAppID, m_strSessionID)

                'If intID > -1 Then
                '    m_objlogApp.WriteEndDataAccessSAP(intID, True)
                'End If

                S.AP.InitExecute("Z_M_Brieflebenslauf_Arval", "I_KUNNR, I_VKORG, I_ZZKENN, I_ZZFAHRG, I_ZZBRIEF, I_ZZREF1",
                                 Right("0000000000" & m_objUser.KUNNR, 10), "1510", UCase(strAmtlKennzeichen), UCase(strFahrgestellnummer), UCase(strBriefnummer), UCase(strOrdernummer))

                'm_tblHistory = myProxy.getExportTable("GT_WEB")
                'm_tableResult = myProxy.getExportTable("GT_QMMA")

                m_tblHistory = S.AP.GetExportTable("GT_WEB")
                m_tableResult = S.AP.GetExportTable("GT_QMMA")

                Dim row As DataRow

                For Each row In m_tableResult.Rows
                    If Not row("ZZUEBER") Is Nothing Then
                        If IsDate(row("ZZUEBER")) Then
                            row("ZZUEBER") = CDate(row("ZZUEBER")).ToShortDateString
                        End If
                    End If
                    If Not TypeOf row("PSTER") Is System.DBNull Then
                        If IsDate(row("PSTER")) Then
                            row("PSTER") = CDate(row("PSTER")).ToShortDateString
                        End If
                    End If
                    If Not TypeOf row("AEDAT") Is System.DBNull Then
                        If IsDate(row("AEDAT")) Then
                            row("AEDAT") = CDate(row("AEDAT")).ToShortDateString
                        End If
                    End If
                    If Not TypeOf row("AEZEIT") Is System.DBNull Then
                        If row("AEZEIT").ToString <> String.Empty Then
                            row("AEZEIT") = Left(row("AEZEIT").ToString, 2) & ":" & row("AEZEIT").ToString.Substring(2, 2)
                        End If
                    End If
                Next

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ", ZZBRIEF=" & UCase(strBriefnummer) & ", ZFAHRG=" & UCase(strFahrgestellnummer) & ", ZZREF1=" & UCase(strOrdernummer) & ", ZZKENN=" & UCase(strAmtlKennzeichen), m_tblHistory, False)
            Catch ex As Exception
                m_intStatus = -2222
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_DATA"
                        m_strMessage = "Keine Ergebnisse für die angegebenen Kriterien."
                    Case "NO_WEB"
                        m_strMessage = "Keine Web-Tabelle erstellt."
                    Case Else
                        m_strMessage = ex.Message
                End Select
                'If intID > -1 Then
                '    m_objlogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                'End If
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", ZZBRIEF=" & UCase(strBriefnummer) & ", ZFAHRG=" & UCase(strFahrgestellnummer) & ", ZZREF1=" & UCase(strOrdernummer) & ", ZZKENN=" & UCase(strAmtlKennzeichen) & ", " & Replace(m_strMessage, "<br>", " "), m_tblHistory, False)
            Finally
                'If intID > -1 Then
                '    m_objlogApp.WriteStandardDataAccessSAP(intID)
                'End If

                m_blnGestartet = False
            End Try
        End If
    End Sub
#End Region
End Class

' ************************************************
' $History: SixtLease_11.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 17:27
' Created in $/CKAG/Applications/appsixtl/Lib
' 
' *****************  Version 8  *****************
' User: Uha          Date: 3.07.07    Time: 9:34
' Updated in $/CKG/Applications/AppSIXTL/AppSIXTLWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 7  *****************
' User: Uha          Date: 8.03.07    Time: 13:42
' Updated in $/CKG/Applications/AppSIXTL/AppSIXTLWeb/Lib
' 
' ************************************************
