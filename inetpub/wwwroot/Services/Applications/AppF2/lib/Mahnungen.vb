
Imports CKG.Base.Business
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Common

Public Class Mahnungen
    Inherits BankBase

    Private Const BAPI_zu_mahnende_fzge As String = "Z_M_ZU_MAHNENDE_FAHRZEUGE_STD"


    Sub New(ByVal user As User, ByVal app As App, ByVal appId As String, ByVal sessionID As String)
        MyBase.New(user, app, appId, sessionID, "")

        KUNNR = user.KUNNR
    End Sub

    Public Sub LoadData(ByRef page As Page, ByVal haendler_ex As String)
        m_intStatus = 0

        Dim mi = System.Reflection.MethodInfo.GetCurrentMethod
        m_strClassAndMethod = mi.DeclaringType.Name & "." & mi.Name

        Try
            Dim proxy = DynSapProxy.getProxy(BAPI_zu_mahnende_fzge, m_objApp, m_objUser, page)

            proxy.setImportParameter("I_AG", KUNNR)
            proxy.setImportParameter("I_HAENDLER_EX", haendler_ex)

            proxy.callBapi()

            Dim result = proxy.getExportTable("GT_MAHN")

            result.Columns.Add("AUGRU_TEXT", GetType(String))

            Dim abrufgruende = New Abrufgruende(m_objUser).Result
            If Not abrufgruende Is Nothing AndAlso abrufgruende.Rows.Count > 0 Then
                Dim augru2Text = abrufgruende.Rows.Cast(Of DataRow).ToDictionary(Function(r) CStr(r("SapWert")), Function(r) CStr(r("WebBezeichnung")))
                For Each r In result.Rows
                    If Not DBNull.Value.Equals(r("AUGRU")) Then
                        Dim txt = String.Empty
                        If augru2Text.TryGetValue(CStr(r("AUGRU")), txt) Then
                            r("AUGRU_TEXT") = txt
                        End If
                    End If
                Next
            End If

            result.AcceptChanges()

            m_tblResult = result

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult)
        Catch ex As Exception
            m_tblResult = Nothing
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_HAENDLER"
                    m_intStatus = -2502
                    m_strMessage = "Händler nicht vorhanden."
                Case "NO_CREDITSTEUER"
                    m_intStatus = -11
                    m_strMessage = "AG nicht in der TAB ZDADCREDITSTEUER."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
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
