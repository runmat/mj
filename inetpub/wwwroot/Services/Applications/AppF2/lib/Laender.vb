Imports CKG.Base.Common
Imports CKG.Base.Business
Imports CKG.Base.Kernel.Security

Public Class Laender
    Inherits BankBase

    Private Const BAPI_Land_Plz As String = "Z_M_Land_Plz_001"

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
            Dim proxy = DynSapProxy.getProxy(BAPI_Land_Plz, m_objApp, m_objUser, page)

            proxy.callBapi()

            Dim r = proxy.getExportTable("GT_WEB")

            r.Columns.Add("Beschreibung", System.Type.GetType("System.String"))
            r.Columns.Add("FullDesc", System.Type.GetType("System.String"))
            Dim rowTemp As DataRow
            For Each rowTemp In r.Rows
                If CInt(rowTemp("LNPLZ")) > 0 Then
                    rowTemp("Beschreibung") = CStr(rowTemp("Landx")) & " (" & CStr(CInt(rowTemp("LNPLZ"))) & ")"
                Else
                    rowTemp("Beschreibung") = CStr(rowTemp("Landx"))
                End If
                rowTemp("FullDesc") = CStr(rowTemp("Land1")) & " " & CStr(rowTemp("Beschreibung"))
            Next

            r.AcceptChanges()

            m_tblResult = r
        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "ERR_INV_PLZ"
                    m_strMessage = "Ungültige Postleitzahl."
                    m_intStatus = -1118
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
