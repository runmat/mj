Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Public Class fin_04
    REM § Status-Report, Kunde: Übergreifend, BAPI: Z_M_Brief_Ohne_Daten,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase

#Region " Declarations"


    'Private dtSAPHersteller As DataTable
    Private m_strFilename2 As String
    'Private mPage As Web.UI.Page


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

    'Public Overrides Sub Fill()
    '    Fill(m_strAppID, m_strSessionID)
    'End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "fin_04.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Try
            S.AP.InitExecute("Z_M_Brief_Ohne_Daten", "I_AG", Right("0000000000" & m_objUser.KUNNR, 10))

            Dim SAPTable As DataTable = S.AP.GetExportTable("GT_WEB")
            CreateOutPut(SAPTable, m_strAppID)

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
        Catch ex As Exception
            Dim errormessage As String = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)

            If errormessage.Contains("NOT_FOUND") Then
                m_intStatus = -1111
                m_strMessage = "Keine Ergebnisse zu den Kriterien gefunden."
            ElseIf errormessage.Contains("NO_DATA") Then
                m_intStatus = -12
                m_strMessage = "Keine Dokumente gefunden."
            Else
                m_intStatus = -9999
                m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End If
            
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        End Try
    End Sub

    Public Sub Change(ByRef SAPTable As DataTable, ByRef Status As String, ByVal strAppID As String, ByVal strSessionID As String)
        Dim sZB As String = ""
        Try
            Dim dRow As DataRow
            Dim dRows() As DataRow

            dRows = SAPTable.Select("Zuordnen=True")
            For Each dRow In dRows

                Dim sHnummer As String
                Dim sLizNr As String
                Dim sFinart As String
                Dim sLabel As String

                sZB = dRow("Nummer ZB2").ToString
                sHnummer = dRow("Haendlernummer").ToString
                sLizNr = dRow("Lizenznummer").ToString
                sFinart = dRow("Finanzierungsart").ToString
                sLabel = dRow("Label").ToString

                S.AP.Init("Z_M_Daten_Anlage")
                S.AP.SetImportParameter("I_AG", Right("0000000000" & m_objUser.KUNNR, 10))
                S.AP.SetImportParameter("I_HAENDLER", Left(sHnummer, 10))
                S.AP.SetImportParameter("I_TIDNR", sZB)
                S.AP.SetImportParameter("I_LIZNR", sLizNr)
                S.AP.SetImportParameter("I_UNAME", Left(m_objUser.UserName, 12))
                S.AP.SetImportParameter("I_FINART", sFinart)
                S.AP.SetImportParameter("I_LABEL", Left(sLabel, 3))
                S.AP.Execute()

                dRow("Status") = "Vorgang OK"
                SAPTable.AcceptChanges()
            Next
            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
        Catch ex As Exception
            Dim errormessage As String = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)

            If errormessage.Contains("HAENDLER_NOT_FOUND") Then
                m_intStatus = -1111
                m_strMessage = "Händler nicht gefunden.(" & sZB & ")"
            ElseIf errormessage.Contains("NO_EQUI") Then
                m_intStatus = -12
                m_strMessage = "Fehler beim Speichern der Daten(" & sZB & ").<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
            ElseIf errormessage.Contains("ERROR_EQUI_UPDATE") Then
                m_intStatus = -2222
                m_strMessage = "Fehler beim Speichern der Daten(" & sZB & ").<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
            Else
                m_intStatus = -9999
                m_strMessage = "Fehler beim Speichern der Daten(" & sZB & ").<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
            End If
            
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
            Status = m_strMessage
        End Try
    End Sub

#End Region

End Class

' ************************************************
' $History: fin_04.vb $
' 
' *****************  Version 8  *****************
' User: Fassbenders  Date: 9.03.10    Time: 10:34
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA: 2918
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 23.06.09   Time: 10:37
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA 2918 Z_M_Daten_Anlage
' 
' *****************  Version 6  *****************
' User: Dittbernerc  Date: 19.06.09   Time: 15:49
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA - 2918 .Net Connector Umstellung
' 
' Bapis:
' Z_M_Brief_Ohne_Daten
' Z_M_Daten_Einz_Report_001
' 
' *****************  Version 5  *****************
' User: Dittbernerc  Date: 19.06.09   Time: 14:29
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA 2918 - .Net Connector Umstellung
' 
' Bapis:
' Z_M_Abweich_abrufgrund
' Z_M_Save_ZABWVERGRUND
' 
' *****************  Version 4  *****************
' User: Dittbernerc  Date: 19.06.09   Time: 11:25
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA 2918 - Abschaltung .Net Connector
' 
' Bapis:
' 
' Z_M_Brief_Ohne_Daten
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 11.04.08   Time: 13:43
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' Migration
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 10.04.08   Time: 17:37
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:32
' Created in $/CKAG/Components/ComCommon/Finance/Lib
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 10:48
' Created in $/CKAG/portal/Components/ComCommon/Finance/Lib
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 6.03.08    Time: 15:39
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' Eigener Finance Proxy Fertigstellung
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 26.02.08   Time: 17:24
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ITA: 1733
' 
' *****************  Version 2  *****************
' User: Uha          Date: 12.12.07   Time: 10:20
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' Kosmetik im Bereich Finance
' 
' *****************  Version 1  *****************
' User: Uha          Date: 12.12.07   Time: 10:08
' Created in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ITA 1469/1501 (Dokumente ohne Daten) in Testversion hinzugefügt
' 
' ************************************************
