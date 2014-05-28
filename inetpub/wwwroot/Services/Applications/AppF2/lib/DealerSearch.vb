Imports CKG.Base.Business
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Common

Public Class DealerSearch
    Inherits BankBase

    Private Const BAPI_addressdaten As String = "Z_M_ADRESSDATEN_STD"


    Sub New(ByVal user As User, ByVal app As App, ByVal appId As String, ByVal sessionID As String)
        MyBase.New(user, app, appId, sessionID, "")

        KUNNR = user.KUNNR
    End Sub

    Public Sub LoadData(ByRef page As Page, Optional ByVal haendler_ex As String = Nothing)
        m_intStatus = 0

        Dim mi = System.Reflection.MethodInfo.GetCurrentMethod
        m_strClassAndMethod = mi.DeclaringType.Name & "." & mi.Name


        Try
            Dim proxy = DynSapProxy.getProxy(BAPI_addressdaten, m_objApp, m_objUser, page)

            proxy.setImportParameter("I_AG", KUNNR)
            proxy.setImportParameter("I_MAX", "2000")
            If Not String.IsNullOrEmpty(haendler_ex) Then
                haendler_ex = "*" & haendler_ex.Trim(" "c, "*"c, "%"c)
                proxy.setImportParameter("I_HAENDLER_EX", haendler_ex)
            End If

            proxy.callBapi()

            Dim result = proxy.getExportTable("GT_ADRS")

            If result.Rows.Count = 0 Then
                m_intStatus = -10
                m_strMessage = "Kein Suchergebnis."
            End If

            result.Columns.Add("DISPLAY", System.Type.GetType("System.String"))
            result.Rows.Cast(Of DataRow).ToList.ForEach(Sub(r)
                                                            r("DISPLAY") = String.Format("{0} {1} - {2}, {3}", r("NAME1"), r("NAME2"), r("STRAS"), r("ORT01"))
                                                        End Sub)
            result.AcceptChanges()

            m_tblResult = result

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult)
        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_CREDITSTEUER"
                    m_intStatus = -11
                    m_strMessage = "AG nicht in der TAB ZDADCREDITSTEUER."
                Case "NO_DATA"
                    m_intStatus = -12
                    m_strMessage = "Keine Eingabedaten vorhanden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
            End Select

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & m_strMessage.Replace("<br>", " "), m_tblResult)
        End Try
    End Sub

    Public Function FilterResult(ByVal haendlerNr As String, ByVal name As String, ByVal name2 As String, ByVal plz As String, ByVal ort As String, ByRef page As Page) As DataView
        m_intStatus = 0

        Dim mi = System.Reflection.MethodInfo.GetCurrentMethod
        m_strClassAndMethod = mi.DeclaringType.Name & "." & mi.Name

        If Me.Result Is Nothing Then
            LoadData(page)
            If Me.Result Is Nothing Then Return Nothing
        End If

        Dim filters = New List(Of String)()
        filters.Add(GetFilter("HAENDLER_EX", haendlerNr))
        filters.Add(GetFilter("NAME1", name))
        filters.Add(GetFilter("NAME2", name2))
        filters.Add(GetFilter("PSTLZ", plz))
        filters.Add(GetFilter("ORT01", ort))

        Return New DataView(Result, String.Join(" AND ", filters.Where(Function(f) Not f Is Nothing).ToArray), "DISPLAY", DataViewRowState.CurrentRows)

        'Try
        '    Dim proxy = DynSapProxy.getProxy(BAPI_addressdaten, m_objApp, m_objUser, Page)

        '    proxy.setImportParameter("I_AG", KUNNR)
        '    proxy.setImportParameter("I_HAENDLER_EX", haendlerNr.Trim()) '("*"c))
        '    proxy.setImportParameter("I_NAME", name.Trim()) '("*"c))
        '    proxy.setImportParameter("I_ORT", ort.Trim()) '("*"c))
        '    proxy.setImportParameter("I_PSTLZ", plz.Trim()) '("*"c))
        '    proxy.setImportParameter("I_MAX", "2000")

        '    proxy.callBapi()

        '    Dim result = proxy.getExportTable("GT_ADRS")

        '    If result.Rows.Count = 0 Then
        '        m_strMessage = "Kein Suchergebnis."
        '    ElseIf Not String.IsNullOrEmpty(name2) Then
        '        Dim sw = name2.StartsWith("*")
        '        Dim ew = name2.EndsWith("*")
        '        Dim filter = name2.Trim("*"c)

        '        Dim tmpResult = result.Clone
        '        Dim rows = result.Rows.Cast(Of DataRow).Where(Function(r)
        '                                                          Dim val = r("NAME2").ToString
        '                                                          Return _
        '                                                              (sw AndAlso val.StartsWith(filter, StringComparison.InvariantCultureIgnoreCase)) OrElse _
        '                                                              (ew AndAlso val.EndsWith(filter, StringComparison.InvariantCultureIgnoreCase)) OrElse _
        '                                                              (val.Equals(filter))
        '                                                      End Function).ToList
        '        rows.ForEach(Sub(r)
        '                         tmpResult.ImportRow(r)
        '                     End Sub)
        '        result = tmpResult
        '    End If

        '    result.Columns.Add("DISPLAY", System.Type.GetType("System.String"))
        '    result.Rows.Cast(Of DataRow).ToList.ForEach(Sub(r)
        '                                                    r("DISPLAY") = String.Format("{0} {1} - {2}, {3}", r("NAME1"), r("NAME2"), r("STRAS"), r("ORT01"))
        '                                                End Sub)
        '    result.AcceptChanges()

        '    m_tblResult = result

        '    WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult)
        'Catch ex As Exception
        '    Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
        '        Case "NO_CREDITSTEUER"
        '            m_intStatus = -11
        '            m_strMessage = "AG nicht in der TAB ZDADCREDITSTEUER."
        '        Case "NO_DATA"
        '            m_intStatus = -12
        '            m_strMessage = "Keine Eingabedaten vorhanden."
        '        Case Else
        '            m_intStatus = -9999
        '            m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
        '    End Select

        '    WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & m_strMessage.Replace("<br>", " "), m_tblResult)
        'End Try
    End Function

    Private Function GetFilter(column As String, value As String, Optional useLike As Boolean = True) As String
        value = value.Trim

        If String.IsNullOrEmpty(value) Then Return Nothing

        If Not useLike Then Return String.Format("{0} = '{1}'", column, value.Replace("*"c, " "c))

        If value.StartsWith("*") Then value = "%" & value.Substring(1)
        If value.EndsWith("*") Then value = value.Substring(0, value.Length - 1) & "%"

        Return String.Format("{0} LIKE '{1}'", column, value.Replace("*"c, " "c))
    End Function


    Public Overrides Sub Show()
        Throw New NotImplementedException
    End Sub

    Public Overrides Sub Change()
        Throw New NotImplementedException
    End Sub

End Class
