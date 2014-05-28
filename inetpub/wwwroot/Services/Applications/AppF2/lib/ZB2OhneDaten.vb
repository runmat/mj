Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports CKG.Base.Common
Public Class ZB2OhneDaten
    Inherits DatenimportBase

#Region "Declarations"
    Private m_Page As System.Web.UI.Page
#End Region


#Region " Methods"
    Public Sub New(ByRef objUser As CKG.Base.Kernel.Security.User, ByVal objApp As CKG.Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, "")

    End Sub

    Public Overloads Overrides Sub Fill()
        Fill(m_strAppID, m_strSessionID, m_Page)

    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        m_strClassAndMethod = "ZB2OhneDaten.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID


        'Dim TestAKFKunnnr As String = "302660"

        Dim intID As Int32 = -1

        Try

            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Brief_Ohne_Daten", m_objApp, m_objUser, Page)

            myProxy.setImportParameter("I_AG", Right("0000000000" & m_objUser.KUNNR, 10))
            'myProxy.setImportParameter("I_AG", Right("0000000000" & TestAKFKunnnr, 10))


            myProxy.callBapi()

            Dim SAPTable As DataTable = myProxy.getExportTable("GT_WEB")
            CreateOutPut(SAPTable, m_strAppID)

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
        Finally
            If intID > -1 Then
                m_objLogApp.WriteStandardDataAccessSAP(intID)
            End If
        End Try
    End Sub

    Public Sub Change(ByRef SAPTable As DataTable, ByRef Status As String, ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
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

                sZB = dRow("Briefnummer").ToString
                sHnummer = dRow("Haendlernummer").ToString
                sLizNr = dRow("Lizenznr").ToString
                sFinart = dRow("Finart").ToString
                sLabel = dRow("Label").ToString

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Daten_Anlage", m_objApp, m_objUser, page)

                'Dim TestAKFKunnnr As String = "302660"

                myProxy.setImportParameter("I_AG", Right("0000000000" & m_objUser.KUNNR, 10))
                'myProxy.setImportParameter("I_AG", Right("0000000000" & TestAKFKunnnr, 10))
                myProxy.setImportParameter("I_HAENDLER", Left(sHnummer, 10))
                myProxy.setImportParameter("I_TIDNR", sZB)
                myProxy.setImportParameter("I_LIZNR", sLizNr)
                myProxy.setImportParameter("I_UNAME", Left(m_objUser.UserName, 12))
                myProxy.setImportParameter("I_FINART", sFinart)
                myProxy.setImportParameter("I_LABEL", Left(sLabel, 3))

                myProxy.callBapi()

                dRow("Status") = "Vorgang OK"
                SAPTable.AcceptChanges()
            Next
            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "HAENDLER_NOT_FOUND"
                    m_intStatus = -1111
                    m_strMessage = "Händler nicht gefunden.(" & sZB & ")"
                Case "NO_EQUI"
                    m_intStatus = -12
                    m_strMessage = "Fehler beim Speichern der Daten(" & sZB & ").<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
                Case "ERROR_EQUI_UPDATE"
                    m_intStatus = -2222
                    m_strMessage = "Fehler beim Speichern der Daten(" & sZB & ").<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Fehler beim Speichern der Daten(" & sZB & ").<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
            End Select
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
            Status = m_strMessage
        End Try
    End Sub





#End Region


End Class
' ************************************************
' $History: ZB2OhneDaten.vb $
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 10.03.10   Time: 17:10
' Updated in $/CKAG2/Applications/AppF2/lib
' ITA: 2918
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 11.08.09   Time: 14:37
' Updated in $/CKAG2/Applications/AppF2/lib
' ITA: 3069
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 10.08.09   Time: 8:23
' Updated in $/CKAG2/Applications/AppF2/lib
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 7.08.09    Time: 16:25
' Updated in $/CKAG2/Applications/AppF2/lib
' ITA: 3069
' 