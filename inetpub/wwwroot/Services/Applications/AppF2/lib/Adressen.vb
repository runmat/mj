Imports CKG.Base.Business
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Common

Public Class Adressen
    Inherits BankBase

    Private Const BAPI_CustomerChildren As String = "Bapi_Customer_Get_Children"
    Private Const BAPI_CustomerDetails As String = "Bapi_Customer_Getdetail2"

    Sub New(ByVal user As User, ByVal app As App, ByVal appId As String, ByVal sessionID As String)
        MyBase.New(user, app, appId, sessionID, "")

        KUNNR = user.KUNNR
    End Sub

    Private lastParentNode As String
    Private lastAdressArt As String
    Private lastNodeLevel As String

    Public Sub LoadData(ByRef page As Page, ByVal parentNode As String, Optional ByVal adressArt As String = "A", Optional ByVal nodeLevel As String = "99") 'HEZ: "B" oder "C"
        Dim mi = System.Reflection.MethodInfo.GetCurrentMethod

        If lastParentNode = parentNode AndAlso lastAdressArt = adressArt AndAlso lastNodeLevel = nodeLevel AndAlso _
            Not Result Is Nothing AndAlso Result.Rows.Count > 0 Then Exit Sub

        m_intStatus = 0
        m_strClassAndMethod = mi.DeclaringType.Name & "." & mi.Name

        Try
            Dim proxy = DynSapProxy.getProxy(BAPI_CustomerChildren, m_objApp, m_objUser, page)

            'proxy.setImportParameter("VALID_ON", Today.ToShortDateString)
            proxy.setImportParameter("NODE_LEVEL", nodeLevel)
            proxy.setImportParameter("CUSTOMERNO", parentNode.PadLeft(10, "0"c))
            proxy.setImportParameter("CUSTHITYP", adressArt)

            proxy.callBapi()

            Dim nodes = proxy.getExportTable("NODE_LIST")

            Dim r = New DataTable()
            r.Columns.Add("ADDRESSNUMBER", GetType(String))
            r.Columns.Add("DISPLAY_ADDRESS", GetType(String))
            r.Columns.Add("POSTL_CODE", GetType(String))
            r.Columns.Add("STREET", GetType(String))
            r.Columns.Add("COUNTRYISO", GetType(String))
            r.Columns.Add("CITY", GetType(String))
            r.Columns.Add("NAME", GetType(String))
            r.Columns.Add("NAME_2", GetType(String))

            For Each node In nodes.Rows
                If CStr(node("NODE_LEVEL")).TrimStart("0"c) <> "1" Then Continue For

                proxy = DynSapProxy.getProxy(BAPI_CustomerDetails, m_objApp, m_objUser, page)
                proxy.setImportParameter("CUSTOMERNO", CStr(node("CUSTOMER")))
                proxy.callBapi()

                Dim ret = proxy.getExportTable("RETURN")
                Dim retType = CStr(ret.Rows.Cast(Of DataRow).First()("TYPE"))

                If Not String.IsNullOrEmpty(retType) AndAlso retType <> "S" AndAlso retType <> "I" Then Continue For

                Dim custDetail = proxy.getExportTable("CUSTOMERGENERALDETAIL")
                Dim groupKey = CStr(custDetail.Rows.Cast(Of DataRow).First()("Groupkey"))
                If groupKey <> m_objUser.Reference Or (String.IsNullOrEmpty(groupKey) AndAlso String.IsNullOrEmpty(m_objUser.Reference)) Then

                    'If (Not SAPReturnCustomerDetail(0)("Groupkey").ToString = m_objUser.Reference) Or (SAPReturnCustomerDetail(0)("Groupkey").ToString.Length = 0 And m_objUser.Reference.Length = 0) Then

                    Dim custAddress = proxy.getExportTable("CUSTOMERADDRESS")
                    Dim addressRow = custAddress.Rows.Cast(Of DataRow).First()

                    Dim newRow = r.NewRow

                    Dim strTemp As String = addressRow("Name").ToString
                    If addressRow("Name_2").ToString.Length > 0 Then
                        strTemp &= ", " & addressRow("Name_2").ToString
                    End If
                    If addressRow("Name_3").ToString.Length > 0 Then
                        strTemp &= ", " & addressRow("Name_3").ToString
                    End If
                    If addressRow("Name_4").ToString.Length > 0 Then
                        strTemp &= ", " & addressRow("Name_4").ToString
                    End If

                    newRow("DISPLAY_ADDRESS") = strTemp & ", " & addressRow("Countryiso").ToString & " - " & addressRow("Postl_Code").ToString & " " & addressRow("City").ToString & ", " & addressRow("Street").ToString
                    newRow("ADDRESSNUMBER") = addressRow("Customer").ToString
                    If addressRow("Postl_Code").ToString.Length > 0 Then
                        newRow("POSTL_CODE") = addressRow("Postl_Code").ToString
                    End If
                    If addressRow("Street").ToString.Length > 0 Then
                        newRow("STREET") = addressRow("Street").ToString
                    End If
                    If addressRow("Countryiso").ToString.Length > 0 Then
                        newRow("COUNTRYISO") = addressRow("Countryiso").ToString
                    End If
                    If addressRow("City").ToString.Length > 0 Then
                        newRow("CITY") = addressRow("City").ToString
                    End If
                    If addressRow("Name").ToString.Length > 0 Then
                        newRow("NAME") = addressRow("Name").ToString
                    End If
                    If addressRow("Name_2").ToString.Length > 0 Then
                        newRow("NAME_2") = addressRow("Name_2").ToString
                    End If
                    r.Rows.Add(newRow)
                End If
            Next

            r.AcceptChanges()

            m_tblResult = r

            lastAdressArt = adressArt
            lastNodeLevel = nodeLevel
            lastParentNode = parentNode
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
