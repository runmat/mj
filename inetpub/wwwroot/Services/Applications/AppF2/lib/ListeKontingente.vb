Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports CKG.Base.Common

Public Class ListeKontingente

    Inherits DatenimportBase

#Region " Declarations"

    Private m_Filename As String
    Private m_Page As System.Web.UI.Page

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
        m_strClassAndMethod = "ListeKontingente.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Try
            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_HAENDLERKONTINGENTE", m_objApp, m_objUser, page)

            myProxy.setImportParameter("KUNNR_AG", Right("0000000000" & m_objUser.KUNNR, 10))
            myProxy.setImportParameter("KKBER", "0001")
            myProxy.setImportParameter("KLIMK", "99999")

            myProxy.callBapi()

            Dim tblTemp As DataTable = myProxy.getExportTable("GT_WEB")


            For Each dr As DataRow In tblTemp.Rows

                If dr("KUNNR_ZF").ToString.Length > 4 Then
                    dr("KUNNR_ZF") = Right(dr("KUNNR_ZF").ToString, 5)
                End If

            Next

            tblTemp.AcceptChanges()


            CreateOutPut(tblTemp, m_strAppID)

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
        Catch ex As Exception

            m_intStatus = -9999
            m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten."

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        End Try
    End Sub


#End Region




End Class

' ************************************************
' $History: ListeKontingente.vb $
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 17.03.10   Time: 8:49
' Updated in $/CKAG2/Applications/AppF2/lib
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 10.03.10   Time: 17:10
' Updated in $/CKAG2/Applications/AppF2/lib
' ITA: 2918
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.03.10    Time: 13:10
' Created in $/CKAG2/Applications/AppF2/lib
' ITA: 3542
' 