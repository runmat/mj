Namespace Kernel
    ''' <summary>
    ''' Auflistungsobjekt für Gruppen
    ''' </summary>
    ''' <remarks></remarks>
    Public Class GroupList
        REM § Auflistungsobjekt für Gruppen zur Verwendung in den Administrationsseiten

        Inherits DataTable

#Region " Constructor "
        Public Sub New(ByVal intCustomerID As Integer, ByVal strConnectionString As String, ByVal intAccountingArea As Integer, Optional ByVal blnAll As Boolean = False, Optional ByVal blnNone As Boolean = False)
            Me.New(intCustomerID, New SqlClient.SqlConnection(strConnectionString), intAccountingArea, blnAll, blnNone)
        End Sub
        Public Sub New(ByVal intCustomerID As Integer, ByVal cn As SqlClient.SqlConnection, ByVal intAccountingArea As Integer, Optional ByVal blnAll As Boolean = False, Optional ByVal blnNone As Boolean = False)
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            Dim daGroups As SqlClient.SqlDataAdapter
            If intAccountingArea > -1 Then
                daGroups = New SqlClient.SqlDataAdapter("SELECT * " & _
                                                             "FROM vwWebGroupAccountingArea " & _
                                                             "WHERE (CustomerID = @CustomerID) " & _
                                                             "AND (AccountingArea = @AccountingArea)", cn)
                daGroups.SelectCommand.Parameters.AddWithValue("@AccountingArea", intAccountingArea)
            Else
                daGroups = New SqlClient.SqlDataAdapter("SELECT * " & _
                                                             "FROM WebGroup " & _
                                                             "WHERE (CustomerID = @CustomerID)", cn)
            End If
            daGroups.SelectCommand.Parameters.AddWithValue("@CustomerID", intCustomerID)
            daGroups.Fill(Me)
            AddAllNone(blnAll, blnNone)
        End Sub
        Public Sub New(ByVal strGroupFilter As String, ByVal intCustomerID As Integer, ByVal strConnectionString As String, ByVal intAccountingArea As Integer)
            Me.New(strGroupFilter, intCustomerID, New SqlClient.SqlConnection(strConnectionString), intAccountingArea)
        End Sub
        Public Sub New(ByVal strGroupFilter As String, ByVal intCustomerID As Integer, ByVal cn As SqlClient.SqlConnection, ByVal intAccountingArea As Integer)
            If strGroupFilter = String.Empty Then strGroupFilter = "%"
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            Dim strSQL As String
            If intCustomerID > 0 Then
                strSQL = "SELECT * " & _
                         "FROM WebGroup " & _
                         "WHERE (CustomerID = @CustomerID) " & _
                           "AND (GroupName LIKE @GroupName)"
            ElseIf intCustomerID = 0 Then
                strSQL = "SELECT * " & _
                         "FROM WebGroup " & _
                         "WHERE (GroupName LIKE @GroupName)"
            Else
                strSQL = "SELECT * " & _
                         "FROM WebGroup " & _
                         "WHERE (CustomerID IS NULL) " & _
                           "AND (GroupName LIKE @GroupName)"
            End If
            If intAccountingArea > -1 Then
                strSQL = Replace(strSQL, "WebGroup", "vwWebGroupAccountingArea") & " AND (AccountingArea = @AccountingArea)"
            End If
            Dim daGroups As New SqlClient.SqlDataAdapter(strSQL, cn)
            With daGroups.SelectCommand.Parameters
                .AddWithValue("@CustomerID", intCustomerID)
                .AddWithValue("@GroupName", Replace(strGroupFilter, "*", "%"))
                If intAccountingArea > -1 Then
                    .AddWithValue("@AccountingArea", intAccountingArea)
                End If
            End With
            daGroups.Fill(Me)
        End Sub

#End Region

#Region " Functions "
        Public Sub AddAllNone(ByVal blnAll As Boolean, ByVal blnNone As Boolean)
            Dim dr As DataRow
            If blnAll Then
                dr = NewRow()
                dr("GroupID") = 0
                dr("GroupName") = " - alle - "
                dr("IsCustomerGroup") = 0
                Rows.Add(dr)
            End If
            If blnNone Then
                dr = NewRow()
                dr("GroupID") = -1
                dr("GroupName") = " - keine - "
                dr("IsCustomerGroup") = 0
                Rows.Add(dr)
            End If
        End Sub
#End Region
    End Class
End Namespace

' ************************************************
' $History: GroupList.vb $
' 
' *****************  Version 3  *****************
' User: Hartmannu    Date: 9.09.08    Time: 13:42
' Updated in $/CKAG/admin/Kernel
' ITA 2152 und 2158
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