Namespace Kernel
Public Class CustomerList
    REM § Objekt zum Auflisten von Kunden für die Administration

    Inherits DataTable

#Region " Constructor "
        Public Sub New(ByVal intAccountingArea As Integer, ByVal strConnectionString As String, Optional ByVal blnAll As Boolean = False, Optional ByVal blnNone As Boolean = False)
            Me.New(intAccountingArea, New SqlClient.SqlConnection(strConnectionString), blnAll, blnNone)
        End Sub
        Public Sub New(ByVal intAccountingArea As Integer, ByVal cn As SqlClient.SqlConnection, Optional ByVal blnAll As Boolean = False, Optional ByVal blnNone As Boolean = False)
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            Dim daCustomers As SqlClient.SqlDataAdapter
            If intAccountingArea > -1 Then
                daCustomers = New SqlClient.SqlDataAdapter("SELECT * " & _
                                                                "FROM vwCustomer " & _
                                                                "WHERE AccountingArea = @AccountingArea", cn)
                daCustomers.SelectCommand.Parameters.AddWithValue("@AccountingArea", intAccountingArea)
            Else
                daCustomers = New SqlClient.SqlDataAdapter("SELECT * " & _
                                                                "FROM vwCustomer ", cn)
            End If
            daCustomers.Fill(Me)
            AddAllNone(blnAll, blnNone)
        End Sub
        Public Sub New(ByVal strFilterCustomerName As String, ByVal intAccountingArea As Integer, ByVal strConnectionString As String)
            Me.New(strFilterCustomerName, intAccountingArea, New SqlClient.SqlConnection(strConnectionString))
        End Sub
        Public Sub New(ByVal strFilterCustomerName As String, ByVal intAccountingArea As Integer, ByVal cn As SqlClient.SqlConnection)
            If strFilterCustomerName = String.Empty Then strFilterCustomerName = "%"
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            Dim daCustomers As SqlClient.SqlDataAdapter
            If intAccountingArea > -1 Then
                daCustomers = New SqlClient.SqlDataAdapter("SELECT * " & _
                                                                "FROM vwCustomer " & _
                                                                "WHERE CustomerName LIKE @CustomerName " & _
                                                                "AND AccountingArea = @AccountingArea", cn)
                daCustomers.SelectCommand.Parameters.AddWithValue("@AccountingArea", intAccountingArea)
            Else
                daCustomers = New SqlClient.SqlDataAdapter("SELECT * " & _
                                                                "FROM vwCustomer " & _
                                                                "WHERE CustomerName LIKE @CustomerName", cn)
            End If
            daCustomers.SelectCommand.Parameters.AddWithValue("@CustomerName", Replace(strFilterCustomerName, "*", "%"))
            daCustomers.Fill(Me)
        End Sub
#End Region

#Region " Functions "
    Public Sub AddAllNone(ByVal blnAll As Boolean, ByVal blnNone As Boolean)
        Dim dr As DataRow
        If blnAll Then
            dr = NewRow()
            dr("CustomerID") = 0
            dr("Customername") = " - alle - "
            Rows.Add(dr)
        End If
        If blnNone Then
            dr = NewRow()
            dr("CustomerID") = -1
            dr("Customername") = " - keine - "
            Rows.Add(dr)
        End If
    End Sub
#End Region

End Class
End Namespace

' ************************************************
' $History: CustomerList.vb $
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
