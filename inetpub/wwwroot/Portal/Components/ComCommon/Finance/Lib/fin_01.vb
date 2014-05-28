Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports CKG.Base.Common

Public Class fin_01
    REM § Status-Report, Kunde: Übergreifend, BAPI: Z_M_Daten_Ohne_Brief,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase ' FFD_Bank_Datenimport

#Region " Declarations"


    Private dtSAPHersteller As DataTable
    Private m_strFilename2 As String
    Private mPage As System.Web.UI.Page


#End Region

#Region " Properties"
    Public ReadOnly Property FileName() As String
        Get
            Return m_strFilename2
        End Get
    End Property

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
        m_strFilename2 = strFilename
    End Sub

    'Public Overloads Overrides Sub Fill()
    '    Fill(m_strAppID, m_strSessionID, mPage)
    'End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "fin_01.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Try
            S.AP.InitExecute("Z_M_DATEN_OHNE_BRIEF", "I_AG", Right("0000000000" & m_objUser.KUNNR, 10))

            Dim tblTemp As DataTable = S.AP.GetExportTable("GT_WEB")
            tblTemp.Columns.Add("Finanzierungsart", System.Type.GetType("System.String"))

            Dim rowTemp As DataRow
            For Each rowTemp In tblTemp.Rows
                If Not TypeOf rowTemp("Zzfinart") Is System.DBNull Then
                    Select Case CStr(rowTemp("Zzfinart"))
                        Case "30"
                            rowTemp("Finanzierungsart") = "LZT"
                        Case "31"
                            rowTemp("Finanzierungsart") = "HEV"
                        Case "32"
                            rowTemp("Finanzierungsart") = "WHS"
                    End Select
                End If
            Next
            CreateOutPut(tblTemp, m_strAppID)

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NOT_FOUND"
                    m_intStatus = -1111
                    m_strMessage = "Keine Ergebnisse zu den Kriterien gefunden."
                Case "NO_DATA"
                    m_intStatus = -12
                    m_strMessage = "Keine Dokumente gefunden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        End Try
    End Sub

    Public Sub DelDocuments(ByVal strAppID As String, ByVal strSessionID As String, ByVal strTidNR As String)
        m_strClassAndMethod = "fin_01.DelDocuments"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Try
            S.AP.InitExecute("Z_M_DATEN_OHNE_BRIEF_DEL", "I_AG, I_TIDNR",
                             Right("0000000000" & m_objUser.KUNNR, 10), strTidNR)

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NOT_DEL"
                    m_intStatus = -1111
                    m_strMessage = "Eintrag konnte nicht gelöscht werden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        End Try
    End Sub

#End Region

End Class

' ************************************************
' $History: fin_01.vb $
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 9.03.10    Time: 10:34
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA: 2918
' 
' *****************  Version 3  *****************
' User: Dittbernerc  Date: 17.06.09   Time: 15:56
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' .Net Connector Umstellung
' 
' Bapis:
' 
' Z_M_Daten_Ohne_Brief
' Z_M_Daten_Ohne_Brief_Del
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 20.05.08   Time: 11:30
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA 1925
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:32
' Created in $/CKAG/Components/ComCommon/Finance/Lib
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 10:48
' Created in $/CKAG/portal/Components/ComCommon/Finance/Lib
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 26.03.08   Time: 16:06
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ITA: 1800
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 6.03.08    Time: 15:39
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' Eigener Finance Proxy Fertigstellung
' 
' *****************  Version 3  *****************
' User: Uha          Date: 11.12.07   Time: 16:31
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ITA 1468/1500 Testversion
' 
' *****************  Version 2  *****************
' User: Uha          Date: 11.12.07   Time: 15:47
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ITA 1470 bzw. 1473/1497 ASPX-Seite und Lib hinzugefügt
' 
' *****************  Version 1  *****************
' User: Uha          Date: 11.12.07   Time: 13:56
' Created in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ITA 1468/1500 ASPX-Seite und Lib hinzugefügt
' 
' ************************************************
