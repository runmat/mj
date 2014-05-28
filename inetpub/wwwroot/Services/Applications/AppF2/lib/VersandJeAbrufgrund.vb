Imports CKG.Base.Business
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Common

Public Class VersandJeAbrufgrund
    Inherits BankBase

    Private Const BAPI_versand_je_abrufgrund As String = "Z_M_VERSAND_JE_ABRUFGRUND_STD"

    Sub New(ByVal user As User, ByVal app As App, ByVal appId As String, ByVal sessionID As String)
        MyBase.New(user, app, appId, sessionID, "")

        KUNNR = user.KUNNR
    End Sub

    Public Sub LoadData(ByRef page As Page, ByVal haendler_ex As String, ByVal ab As DateTime, ByVal bis As DateTime, ByVal versandGrund As String, ByVal bezahlt As String)
        m_intStatus = 0

        Dim mi = System.Reflection.MethodInfo.GetCurrentMethod
        m_strClassAndMethod = mi.DeclaringType.Name & "." & mi.Name

        Try
            Dim proxy = DynSapProxy.getProxy(BAPI_versand_je_abrufgrund, m_objApp, m_objUser, page)

            proxy.setImportParameter("I_AG", KUNNR)
            proxy.setImportParameter("I_HAENDLER_EX", haendler_ex)
            proxy.setImportParameter("I_DAT_VON", ab)
            proxy.setImportParameter("I_DAT_BIS", bis)
            proxy.setImportParameter("I_VSGRUND", versandGrund)
            proxy.setImportParameter("I_BEZAHLT", bezahlt)

            proxy.callBapi()

            Dim result = proxy.getExportTable("GT_WEB")

            m_tblResult = result

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult)
        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                   Case "NO_DATA"
                    m_intStatus = -1402
                    m_strMessage = "Keine Daten vorhanden."
                Case Else
                    m_intStatus = -5555
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
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
