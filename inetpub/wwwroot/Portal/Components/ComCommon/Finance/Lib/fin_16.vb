Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports CKG.Base.Common

Public Class fin_16

    Inherits CKG.Base.Business.DatenimportBase

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        Try
            m_intStatus = 0
            m_strMessage = ""

            S.AP.InitExecute("Z_M_Klaerfaelle_001", "I_AG", Right("0000000000" & m_objUser.KUNNR, 10))

            Dim tmpTable As DataTable = S.AP.GetExportTable("GT_WEB")
            MyBase.CreateOutPut(tmpTable, strAppID)

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
        Catch ex As Exception

            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_intStatus = -1111
                    m_strMessage = "Keine Fahrzeuge gefunden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
            End Select
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        End Try
    End Sub

End Class

' ************************************************
' $History: fin_16.vb $
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 23.06.09   Time: 9:26
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA 2918, Z_M_Klaerfaelle_001
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:32
' Created in $/CKAG/Components/ComCommon/Finance/Lib
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 10:48
' Created in $/CKAG/portal/Components/ComCommon/Finance/Lib
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 6.03.08    Time: 15:39
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' Eigener Finance Proxy Fertigstellung
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 10.01.08   Time: 14:21
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ITA: 1504
' 
' *****************  Version 1  *****************
' User: Rudolpho     Date: 10.01.08   Time: 10:23
' Created in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ITA: 1504
' 
' ************************************************

