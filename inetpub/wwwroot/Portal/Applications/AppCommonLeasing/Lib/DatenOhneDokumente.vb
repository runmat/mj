Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
'Imports Microsoft.Data.SAPClient
Imports CKG.Base.Common

Public Class DatenOhneDokumente
    Inherits Base.Business.DatenimportBase


    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Overrides Sub Fill()

    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "DatenOhneDokumente.Fill"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID As Int32 = -1

            Dim strKUNNR As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)

            Try

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_DATEN_OHNE_DOKUMENTE", m_objApp, m_objUser, page)

                'myProxy.setImportParameter("I_KUNNR_AG", strKUNNR)


                'myProxy.callBapi()

                S.AP.InitExecute("Z_M_DATEN_OHNE_DOKUMENTE", "I_KUNNR_AG", strKUNNR)

                ResultTable = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")


                ResultTable.DefaultView.RowFilter = "SUCLTXT = 'aktiv'"

                ResultTable = ResultTable.DefaultView.ToTable


                CreateOutPut(ResultTable, strAppID)

            Catch ex As Exception
                Select Case ex.Message
                    Case "NO_DATA"
                        m_strMessage = "Keine Daten gefunden."
                        m_intStatus = -1111
                    Case Else
                        m_strMessage = ex.Message
                        m_intStatus = -9999
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If

    End Sub
End Class

' ************************************************
' $History: DatenOhneDokumente.vb $
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 26.05.11   Time: 15:35
' Updated in $/CKAG/Applications/AppCommonLeasing/Lib
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 26.05.11   Time: 13:44
' Updated in $/CKAG/Applications/AppCommonLeasing/Lib
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 11.11.08   Time: 10:14
' Created in $/CKAG/Applications/AppCommonLeasing/Lib
' ITA 2370,2367 (Nachbesserungen)
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 11.11.08   Time: 8:52
' Created in $/CKAG/Applications/AppCommonLeasing/Lib
' ITA 2372 + 2367 (Nachbesserung)
' 
' ************************************************
