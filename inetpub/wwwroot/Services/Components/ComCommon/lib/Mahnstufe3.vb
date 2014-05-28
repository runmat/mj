Option Explicit On
Option Strict On

Imports CKG.Base.Business
Imports CKG.Base.Common

Public Class Mahnstufe3
    Inherits Base.Business.DatenimportBase

#Region " Declarations"
#End Region

#Region " Properties"

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Overrides Sub Fill()
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        m_strClassAndMethod = "Mahnstufe3.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        Try
            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Brief_Temp_Vers_Mahn_002", m_objApp, m_objUser, page)

            myProxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
            'myProxy.setImportParameter("I_KONZS_ZK", m_objUser.Reference)

            myProxy.callBapi()

            Dim tblTemp2 As DataTable = myProxy.getExportTable("GT_WEB")

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

#End Region
End Class
