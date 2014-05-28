Imports CKG.Base.Business
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Common

Public Class Haendlerbestand
    Inherits BankBase

    Private Const BAPI_bestand_lesen As String = "Z_M_BESTAND_LESEN_STD"

    Sub New(ByVal user As User, ByVal app As App, ByVal appId As String, ByVal sessionID As String)
        MyBase.New(user, app, appId, sessionID, "")

        KUNNR = user.KUNNR
    End Sub

    Public Sub LoadData(ByRef page As Page, ByVal haendler_ex As String)
        m_intStatus = 0

        Dim mi = System.Reflection.MethodInfo.GetCurrentMethod
        m_strClassAndMethod = mi.DeclaringType.Name & "." & mi.Name

        Try
            Dim proxy = DynSapProxy.getProxy(BAPI_bestand_lesen, m_objApp, m_objUser, page)

            proxy.setImportParameter("I_AG", KUNNR)
            proxy.setImportParameter("I_HAENDLER_EX", haendler_ex)
            
            proxy.callBapi()

            Dim result = proxy.getExportTable("GT_WEB")

            If result.Rows.Count = 0 Then
                m_intStatus = -10
                m_strMessage = "Kein Suchergebnis."
            End If

            result.Columns.Add("ZZHUBRAUM_SORT", GetType(Integer))
            result.Columns.Add("ZZNENNLEISTUNG_SORT", GetType(Integer))

            For Each r In result.Rows
                If Not DBNull.Value.Equals(r("DATAB")) Then
                    r("DATAB") = Left(r("DATAB").ToString, 10)
                End If
                If Not DBNull.Value.Equals(r("ZZHUBRAUM")) Then
                    Dim s = CStr(r("ZZHUBRAUM"))
                    Dim i As Integer
                    If Integer.TryParse(s, i) Then r("ZZHUBRAUM_SORT") = i
                End If
                If Not DBNull.Value.Equals(r("ZZNENNLEISTUNG")) Then
                    Dim s = CStr(r("ZZNENNLEISTUNG"))
                    Dim i As Integer
                    If Integer.TryParse(s, i) Then r("ZZNENNLEISTUNG_SORT") = i
                End If
            Next
            result.AcceptChanges()

            m_tblResult = result

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult)
        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_HAENDLER"
                    m_intStatus = -2502
                    m_strMessage = "Händler nicht vorhanden."
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
