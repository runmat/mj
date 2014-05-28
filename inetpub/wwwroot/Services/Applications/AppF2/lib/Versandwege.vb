Imports CKG.Base.Business
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Common

Public Class Versandwege
    Inherits BankBase

    Private Const BAPI_Verswege As String = "Z_DPM_READ_VERSWEGE_01"

    Sub New(ByVal user As User, ByVal app As App, ByVal appId As String, ByVal sessionID As String)
        MyBase.New(user, app, appId, sessionID, "")

        KUNNR = user.KUNNR
    End Sub

    Public Sub LoadData(ByRef page As Page)
        Dim mi = System.Reflection.MethodInfo.GetCurrentMethod

        If Not Result Is Nothing AndAlso Result.Rows.Count > 0 Then Exit Sub

        m_intStatus = 0
        m_strClassAndMethod = mi.DeclaringType.Name & "." & mi.Name

        Try

            Dim proxy = DynSapProxy.getProxy(BAPI_Verswege, m_objApp, m_objUser, page)

            proxy.setImportParameter("I_KUNNR_AG", Right("0000000000" & KUNNR, 10))

            If Not String.IsNullOrEmpty(m_objUser.Reference) Then proxy.setImportParameter("I_KUNNR_ZF", m_objUser.Reference)

            proxy.callBapi()

            Dim r = proxy.getExportTable("GT_WEB")

            m_tblResult = r
        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)

                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
            End Select

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & m_strMessage.Replace("<br>", " "), m_tblResult)
        End Try
    End Sub

    Public Overrides Sub Change()
        Throw New NotImplementedException
    End Sub

    Public Overrides Sub Show()
        Throw New NotImplementedException
    End Sub
End Class
