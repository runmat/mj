Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Common

Public Class Report_38
    Inherits Base.Business.DatenimportBase

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Overrides Sub Fill()

    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page, _
                              ByVal strFahrgestellnummer As String)
        m_strClassAndMethod = "Report_38.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim strZZFAHRG As String = strFahrgestellnummer

            Try
                S.AP.InitExecute("Z_M_BRIEFLEBENSLAUF_TUETE", "I_KUNNR, I_VKORG, I_ZZFAHRG",
                                 Right("0000000000" & m_objUser.KUNNR, 10), "1510", strZZFAHRG)

                m_tblResult = S.AP.GetExportTable("GT_WEB")

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ", ZZFAHRG=" & strZZFAHRG, m_tblResult, False)

            Catch ex As Exception
                m_intStatus = -5555
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
                End Select
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", ZZFAHRG=" & strZZFAHRG & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)

            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub
#End Region
End Class
