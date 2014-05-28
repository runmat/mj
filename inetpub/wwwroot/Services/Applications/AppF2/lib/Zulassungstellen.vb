Imports CKG.Base.Business
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Common

Public Class Zulassungstellen
    Inherits BankBase

    Private Const BAPI_Zulassungstellen As String = "Z_Get_Zulst_By_Plz"

    Sub New(ByVal user As User, ByVal app As App, ByVal appId As String, ByVal sessionID As String)
        MyBase.New(user, app, appId, sessionID, "")

        KUNNR = user.KUNNR
    End Sub

    Private lastPlz As String = String.Empty
    Private lastOrt As String = String.Empty

    Public Sub LoadData(ByRef page As Page, Optional ByVal plz As String = "", Optional ByVal ort As String = "")
        Dim mi = System.Reflection.MethodInfo.GetCurrentMethod

        If lastPlz = plz AndAlso lastOrt = ort AndAlso _
            Not Result Is Nothing AndAlso Result.Rows.Count > 0 Then Exit Sub

        m_intStatus = 0
        m_strClassAndMethod = mi.DeclaringType.Name & "." & mi.Name

        Try
            Dim proxy = DynSapProxy.getProxy(BAPI_Zulassungstellen, m_objApp, m_objUser, page)

            proxy.setImportParameter("I_PLZ", plz)
            proxy.setImportParameter("I_ORT", ort)

            proxy.callBapi()

            Dim r = proxy.getExportTable("T_ZULST")
            Dim display = New DataColumn("DISPLAY", GetType(String))
            display.DefaultValue = String.Empty
            r.Columns.Add(display)

            'Dim counter = 0
            For Each row In r.Rows
                If row("ORT01").ToString <> String.Empty And row("LIFNR").ToString <> String.Empty Then
                    Dim intTemp As Integer = InStr(row("ORT01").ToString, "ZLS")
                    Dim intTemp2 As Integer = InStr(row("ORT01").ToString, "geschl")
                    If Not (intTemp > 0 AndAlso intTemp2 > intTemp) Then
                        row("DISPLAY") = row("PSTLZ").ToString & " - " & row("ORT01").ToString & " - " & row("STRAS").ToString
                        'newRow("PSTL2") = CType(counter, String) 'Lfd. Nr. (für spätere Suche!)
                        'counter += 1
                    End If
                End If
            Next
            r.AcceptChanges()

            For Each row In r.Select("DISPLAY = ''")
                row.Delete()
            Next
            r.AcceptChanges()

            m_tblResult = r

            lastOrt = ort
            lastPlz = plz
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
