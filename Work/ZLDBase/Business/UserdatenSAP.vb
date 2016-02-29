Option Explicit On 
Option Infer On
Option Strict On

Imports System
Imports CKG.Base.Common
Imports CKG.Base.Kernel.Common

Namespace Business

    '''Stellt Zugriffsmethoden für das Auslesen von User-Hilfsdaten aus SAP bereit
    Public Class UserdatenSAP
        Inherits DatenimportBase

        Public Sub New(ByRef objUser As Kernel.Security.User, ByVal objApp As Kernel.Security.App)
            MyBase.New(objUser, objApp, "")
        End Sub

        Public Function getKostenstellenLZLD(ByVal strAppID As String, ByVal strSessionID As String) As DataTable
            Dim tempTable As New DataTable()
            m_strClassAndMethod = "UserdatenSAP.getKostenstellenLZLD"
            m_strAppID = strAppID
            m_strSessionID = strSessionID

            If Not m_blnGestartet Then
                m_blnGestartet = True

                Try
                    Dim proxy = DynSapProxy.getProxy("Z_ZLD_FILIALEN_ZUM_LZLD", m_objApp, m_objUser, PageHelper.GetCurrentPage())

                    proxy.setImportParameter("I_BUKRS", m_objUser.Reference.Substring(0, 4))
                    proxy.setImportParameter("I_LZLD_G", m_objUser.GebietLZLD)
                    proxy.setImportParameter("I_MIT_VERTR", "X")

                    proxy.callBapi()

                    Dim gt_web = proxy.getExportTable("GT_ZLD")
                    gt_web.DefaultView.Sort = "VKBUR"

                    tempTable = gt_web.DefaultView.ToTable()

                Catch ex As Exception
                    m_intStatus = -9999
                    Select Case ex.Message
                        Case "NO_DATA"
                            m_strMessage = "Keine Eingabedaten vorhanden."
                        Case Else
                            m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                    End Select
                Finally
                    m_blnGestartet = False
                End Try
            End If

            Return tempTable
        End Function

    End Class

End Namespace
