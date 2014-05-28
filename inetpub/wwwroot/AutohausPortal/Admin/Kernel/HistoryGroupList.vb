Namespace Kernel
    Public Class HistoryGroupList
        REM § Auflistungsobjekt für Gruppen zur Verwendung in den Administrationsseiten

        Inherits DataTable

#Region " Constructor "
        Public Sub New(ByVal strCustomerName As String, ByVal strConnectionString As String, Optional ByVal blnAll As Boolean = False, Optional ByVal blnNone As Boolean = False)
            Me.New(strCustomerName, New SqlClient.SqlConnection(strConnectionString), blnAll, blnNone)
        End Sub
        Public Sub New(ByVal strCustomerName As String, ByVal cn As SqlClient.SqlConnection, Optional ByVal blnAll As Boolean = False, Optional ByVal blnNone As Boolean = False)
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If

            Dim daGroups As New SqlClient.SqlDataAdapter("SELECT TOP 100 PERCENT GroupName " & _
                                                         "FROM dbo.WebUserHistory " & _
                                                         "GROUP BY GroupName, Customername " & _
                                                         "HAVING (Customername = @Customername) ORDER BY GroupName", cn)
            daGroups.SelectCommand.Parameters.AddWithValue("@Customername", strCustomerName)
            daGroups.Fill(Me)
            AddAllNone(blnAll, blnNone)
        End Sub

        Public Sub New(ByVal strGroupFilter As String, ByVal strCustomerName As String, ByVal strConnectionString As String)
            Me.New(strGroupFilter, strCustomerName, New SqlClient.SqlConnection(strConnectionString))
        End Sub
        Public Sub New(ByVal strGroupFilter As String, ByVal strCustomerName As String, ByVal cn As SqlClient.SqlConnection)
            If strGroupFilter = String.Empty Then strGroupFilter = "%"
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            Dim strSQL As String
            If strCustomerName.Length > 0 Then
                strSQL = "SELECT TOP 100 PERCENT GroupName " & _
                         "FROM  dbo.WebUserHistory " & _
                         "GROUP BY GroupName, Customername " & _
                         "HAVING (CustomerName = @CustomerName) AND (GroupName LIKE @GroupName) ORDER BY GroupName"
            ElseIf strCustomerName.Length = 0 Then
                strSQL = "SELECT TOP 100 PERCENT GroupName " & _
                         "FROM  dbo.WebUserHistory " & _
                         "GROUP BY GroupName, Customername " & _
                         "HAVING (GroupName LIKE @GroupName) ORDER BY GroupName"
            Else
                strSQL = "SELECT TOP 100 PERCENT GroupName " & _
                         "FROM  dbo.WebUserHistory " & _
                         "GROUP BY GroupName, Customername " & _
                         "HAVING (CustomerName IS NULL) AND (GroupName LIKE @GroupName) ORDER BY GroupName"
            End If
            Dim daGroups As New SqlClient.SqlDataAdapter(strSQL, cn)
            With daGroups.SelectCommand.Parameters
                .AddWithValue("@CustomerName", strCustomerName)
                .AddWithValue("@GroupName", Replace(strGroupFilter, "*", "%"))
            End With
            daGroups.Fill(Me)
        End Sub

#End Region

#Region " Functions "
        Public Sub AddAllNone(ByVal blnAll As Boolean, ByVal blnNone As Boolean)
            Dim dr As DataRow
            If blnAll Then
                dr = NewRow()
                dr("GroupName") = " - alle - "
                Rows.Add(dr)
            End If
            If blnNone Then
                dr = NewRow()
                dr("GroupName") = " - keine - "
                Rows.Add(dr)
            End If
        End Sub
#End Region
    End Class
End Namespace

' ************************************************
' $History: HistoryGroupList.vb $
' 
' *****************  Version 1  *****************
' User: Rudolpho     Date: 11.03.11   Time: 15:00
' Created in $/CKPortalZLD/PortalZLD/Admin/Kernel
' 
' *****************  Version 1  *****************
' User: Dittbernerc  Date: 5.11.09    Time: 15:02
' Created in $/CKAG2/Admin/Kernel
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 11.04.08   Time: 15:47
' Updated in $/CKAG/admin/Kernel
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 14:47
' Created in $/CKAG/admin/Kernel
' 
' *****************  Version 3  *****************
' User: Uha          Date: 1.03.07    Time: 16:49
' Updated in $/CKG/Admin/AdminWeb/Kernel
' 
' ************************************************