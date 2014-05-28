Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel

Public Class LT_03
    REM � Status-Report, Kunde: Arval, BAPI: Z_M_Briefeohnefzg2 (Fill),
    REM � BAPI: Z_M_Brieflebenslauf (FillHistory),
    REM � Ausgabetabelle per Zuordnung in Web-DB (Fill), Direkte R�ckgabe (FillHistory).

    Inherits DatenimportBase ' FFD_Bank_Datenimport

#Region " Declarations"

    Private m_tblHistory As DataTable

#End Region

#Region " Properties"

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

    Public Sub FillHistory(ByVal strAppID As String, ByVal strSessionID As String, ByVal strAmtlKennzeichen As String, ByVal strFahrgestellnummer As String, ByVal strBriefnummer As String, ByVal strOrdernummer As String, ByVal page As Page)
        m_strClassAndMethod = "LT_03.FillHistory"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Try
            S.AP.Init("Z_M_Brieflebenslauf_Lt")

            S.AP.SetImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
            S.AP.SetImportParameter("I_VKORG", "1510")
            S.AP.SetImportParameter("I_CHASSIS_NUM", UCase(strFahrgestellnummer))
            S.AP.SetImportParameter("I_LICENSE_NUM", UCase(strAmtlKennzeichen))
            S.AP.SetImportParameter("I_TIDNR", UCase(strBriefnummer))
            S.AP.SetImportParameter("I_LIZNR", UCase(strOrdernummer))
            S.AP.SetImportParameter("I_KONZS_ZK", m_objUser.Reference)

            S.AP.Execute()

            m_tblHistory = S.AP.GetExportTable("GT_WEB")
            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ", ZZBRIEF=" & UCase(strBriefnummer) & ", ZFAHRG=" & UCase(strFahrgestellnummer) & ", ZZREF1=" & UCase(strOrdernummer) & ", ZZKENN=" & UCase(strAmtlKennzeichen), m_tblHistory, False)
        Catch ex As Exception
            m_intStatus = -2222
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_strMessage = "Keine Daten gefunden."
                Case "NO_WEB"
                    m_strMessage = "Keine Web-Tabelle erstellt."
                Case Else
                    m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
            End Select
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", ZZBRIEF=" & UCase(strBriefnummer) & ", ZFAHRG=" & UCase(strFahrgestellnummer) & ", ZZREF1=" & UCase(strOrdernummer) & ", ZZKENN=" & UCase(strAmtlKennzeichen) & ", " & Replace(m_strMessage, "<br>", " "), m_tblHistory, False)
        End Try
    End Sub

#End Region

End Class

' ************************************************
' $History: LT_03.vb $
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 30.06.09   Time: 14:58
' Updated in $/CKAG/Applications/AppLeaseTrend/Lib
' ITA 2918  Z_M_FEHLENDE_VERTRAGSNUMMERN, Z_M_BRIEFLEBENSLAUF_LT,
' Z_M_BRIEFERSTEINGANG_LEASETR, Z_M_BRIEFANFORDERUNG_ALLG,
' Z_M_UNANGEFORDERT_L, Z_M_KUNDENDATEN_LT, Z_M_VERSENDETE_ZB2_ENDG_LT,
' Z_M_VERSENDETE_ZB2_TEMP_LT, Z_M_BRIEF_TEMP_VERS_MAHN_002,
' Z_M_DATEN_OHNE_ZB2_LT
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 15:07
' Created in $/CKAG/Applications/AppLeaseTrend/Lib
' 
' *****************  Version 5  *****************
' User: Uha          Date: 2.07.07    Time: 18:14
' Updated in $/CKG/Applications/AppLeaseTrend/AppLeaseTrendWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 4  *****************
' User: Uha          Date: 8.03.07    Time: 10:46
' Updated in $/CKG/Applications/AppLeaseTrend/AppLeaseTrendWeb/Lib
' 
' ************************************************
