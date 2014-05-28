Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports CKG.Base.Common

Public Class ec_200
    Inherits Base.Business.DatenimportBase


    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, _
                          ByVal datAbmeldedatumVon As String, _
                          ByVal datAbmeldedatumBis As String, _
                          ByVal page As System.Web.UI.Page)
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Try
                m_intStatus = 0
                m_strMessage = ""

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Abm_Abgemeldete_Kfz", m_objApp, m_objUser, page)

                'myProxy.setImportParameter("KUNNR", m_objUser.KUNNR.PadLeft(10, "0"c))
                'myProxy.setImportParameter("PICKDATAB", datAbmeldedatumVon)
                'myProxy.setImportParameter("PICKDATBI", datAbmeldedatumBis)


                'myProxy.callBapi()

                S.AP.InitExecute("Z_M_Abm_Abgemeldete_Kfz", "KUNNR,PICKDATAB,PICKDATBI", m_objUser.KUNNR.PadLeft(10, "0"c), datAbmeldedatumVon, datAbmeldedatumBis)

                'CreateOutPut(myProxy.getExportTable("AUSGABE"), strAppID)
                CreateOutPut(S.AP.GetExportTable("AUSGABE"), strAppID)

            Catch ex As Exception
                m_intStatus = -9999
                m_strMessage = ex.Message

            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub
End Class
