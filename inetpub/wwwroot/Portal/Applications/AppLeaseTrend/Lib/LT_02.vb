Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel

Public Class LT_02
    REM § Status-Report, Kunde: LeaseTrend, BAPI: Z_M_Versendete_Equipments,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits DatenimportBase ' FFD_Bank_Datenimport

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Overrides Sub Fill()
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        m_strClassAndMethod = "LT_02.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Try

            S.AP.InitExecute("Z_M_Fehlende_Coc_Leasetr", "I_KUNNR, I_KONZS_ZK", Right("0000000000" & m_objUser.KUNNR, 10), m_objUser.Reference)

            Dim tblTemp2 As DataTable = S.AP.GetExportTable("GT_WEB")

            CreateOutPut(tblTemp2, strAppID)
            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
        Catch ex As Exception
            m_intStatus = -5555
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_strMessage = "Keine Daten gefunden."
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten."
            End Select
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
        End Try
    End Sub

End Class

' ************************************************
' $History: LT_02.vb $
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 30.06.09   Time: 16:47
' Updated in $/CKAG/Applications/AppLeaseTrend/Lib
' ITA 2918 Z_M_Fehlende_Coc_Leasetr
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
