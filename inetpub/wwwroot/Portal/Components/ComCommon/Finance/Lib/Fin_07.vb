Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Public Class Fin_07
    REM § Mahn-Report, Kunde: Banken, BAPI: Z_M_Faellige_Fahrzdok,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase

#Region " Declarations"

#End Region

#Region " Properties"

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal strZZKKBER As String)
        m_strClassAndMethod = "Fin_07.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Dim strKUNNR As String = m_objUser.Reference ' Right("0000000000" & m_objUser.KUNNR, 10)
        Dim strKNRZE As String = m_strFiliale
        Dim strKONZS As String = "0000324562"

        Try
            S.AP.InitExecute("Z_M_Faellige_Fahrzdok", "I_AG, I_HAENDLER, I_KKBER, I_VKORG",
                             m_objUser.KUNNR, m_objUser.Reference, strZZKKBER, "1510")

            Dim tblTemp2 As DataTable = S.AP.GetExportTable("GT_WEB")
            tblTemp2.Columns("ZZKKBER").MaxLength = 25

            Dim rowTemp As DataRow
            For Each rowTemp In tblTemp2.Rows
                Select Case CStr(rowTemp("ZZKKBER"))
                    Case "0001"
                        rowTemp("ZZKKBER") = "Standard temporär"
                    Case "0002"
                        rowTemp("ZZKKBER") = "Standard endgültig"
                End Select
            Next
            tblTemp2.Columns.Add("Adresse", System.Type.GetType("System.String"))
            For Each rowTemp In tblTemp2.Rows
                rowTemp("Adresse") = rowTemp("Name1").ToString & ", " & rowTemp("Post_Code1").ToString & " " & rowTemp("City1").ToString & ", " & rowTemp("Street").ToString & " " & rowTemp("House_NUM1").ToString
            Next

            CreateOutPut(tblTemp2, strAppID)
            WriteLogEntry(True, "KNRZE=" & strKNRZE & ", KONZS=" & strKONZS & ", KUNNR=" & strKUNNR, m_tblResult, False)
        Catch ex As Exception
            m_intStatus = -9999

            Dim errormessage As String = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message).Replace(" Execution failed", "")

            If errormessage.Contains("NO_DATA") Then
                m_strMessage = "Keine Eingabedaten vorhanden."
                m_intStatus = -1
            ElseIf errormessage.Contains("NO_WEB") Then
                m_strMessage = "Keine Web-Tabelle erstellt."
            ElseIf errormessage.Contains("NO_HAENDLER") Then
                m_strMessage = "Händler nicht vorhanden."
            ElseIf errormessage.Contains("HAENDLER_NOT_FOUND") Then
                m_strMessage = "Händler nicht gefunden."
            Else
                m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
            End If
            
            WriteLogEntry(False, "KNRZE=" & strKNRZE & ", KONZS=" & strKONZS & ", KUNNR=" & strKUNNR & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        End Try
    End Sub

#End Region

End Class

' ************************************************
' $History: Fin_07.vb $
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 22.06.09   Time: 16:08
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' 
' *****************  Version 2  *****************
' User: Dittbernerc  Date: 18.06.09   Time: 15:39
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA 2918 - abschaltung .net connector
' 
' BAPIS:
' 
' Z_M_Haendlerbestand
' Z_M_Faellige_Fahrzdok
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:32
' Created in $/CKAG/Components/ComCommon/Finance/Lib
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 10:48
' Created in $/CKAG/portal/Components/ComCommon/Finance/Lib
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 6.03.08    Time: 15:39
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' Eigener Finance Proxy Fertigstellung
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 1.02.08    Time: 11:35
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' AKF Änderungen
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 22.01.08   Time: 10:44
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' Doku
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 4.01.08    Time: 16:01
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' Bugfix RTFS
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 14.12.07   Time: 11:02
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ITA 1466/1499 Fällige Vorgänge
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 13.12.07   Time: 14:50
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ITA 1466 Kompilierfähig
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 13.12.07   Time: 13:36
' Created in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' Fällige Vorgänge Händler
' 
' ************************************************
