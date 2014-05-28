Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports CKG.Base.Common

Public Class DatenOhneZB2
    Inherits DatenimportBase

#Region " Declarations"

    Private dtSAPHersteller As DataTable
    Private m_Filename As String
    Private m_Page As System.Web.UI.Page

#End Region

#Region " Properties"
    Public ReadOnly Property FileName() As String
        Get
            Return m_Filename
        End Get
    End Property

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As CKG.Base.Kernel.Security.User, ByVal objApp As CKG.Base.Kernel.Security.App, ByVal Filename As String)
        MyBase.New(objUser, objApp, Filename)
        m_Filename = Filename
    End Sub

    Public Overloads Overrides Sub Fill()
        Fill(m_strAppID, m_strSessionID, m_Page)

    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        m_strClassAndMethod = "DatenOhneZB2.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Try
            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_DATEN_OHNE_BRIEF", m_objApp, m_objUser, page)

            myProxy.setImportParameter("I_AG", Right("0000000000" & m_objUser.KUNNR, 10))

            myProxy.callBapi()

            Dim tblTemp As DataTable = myProxy.getExportTable("GT_WEB")

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

    Public Sub DelDocuments(ByVal strAppID As String, ByVal strSessionID As String, ByVal strTidNR As String, ByVal page As Page)
        m_strClassAndMethod = "DatenOhneZB2.DelDocuments"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Try
            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_DATEN_OHNE_BRIEF_DEL", m_objApp, m_objUser, page)

            myProxy.setImportParameter("I_AG", Right("0000000000" & m_objUser.KUNNR, 10))
            myProxy.setImportParameter("I_TidNR", strTidNR)

            myProxy.callBapi()

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
' $History: DatenOhneZB2.vb $
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 6.04.10    Time: 13:42
' Updated in $/CKAG2/Applications/AppF2/lib
' ITA: 3634
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 10.03.10   Time: 17:10
' Updated in $/CKAG2/Applications/AppF2/lib
' ITA: 2918
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 7.08.09    Time: 13:15
' Updated in $/CKAG2/Applications/AppF2/lib
' 