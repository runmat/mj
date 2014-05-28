Namespace Kernel
    Public Class HistoryCustomerList
        REM § Objekt zum Auflisten von Kunden für die Administration

        Inherits DataTable

#Region " Constructor "
        Public Sub New(ByVal strConnectionString As String, Optional ByVal blnAll As Boolean = False, Optional ByVal blnNone As Boolean = False)
            Me.New(New SqlClient.SqlConnection(strConnectionString), blnAll, blnNone)
        End Sub
        Public Sub New(ByVal cn As SqlClient.SqlConnection, Optional ByVal blnAll As Boolean = False, Optional ByVal blnNone As Boolean = False)
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            Dim daCustomers As New SqlClient.SqlDataAdapter("SELECT TOP 100 PERCENT Customername FROM dbo.WebUserHistory GROUP BY Customername ORDER BY Customername", cn)
            daCustomers.Fill(Me)
            AddAllNone(blnAll, blnNone)
        End Sub
        Public Sub New(ByVal strFilterCustomerName As String, ByVal strConnectionString As String)
            Me.New(strFilterCustomerName, New SqlClient.SqlConnection(strConnectionString))
        End Sub
        Public Sub New(ByVal strFilterCustomerName As String, ByVal cn As SqlClient.SqlConnection)
            If strFilterCustomerName = String.Empty Then strFilterCustomerName = "%"
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            Dim daCustomers As New SqlClient.SqlDataAdapter("SELECT TOP 100 PERCENT Customername FROM dbo.WebUserHistory GROUP BY Customername ORDER BY Customername " & _
                                                            "WHERE CustomerName LIKE @CustomerName", cn)
            daCustomers.SelectCommand.Parameters.AddWithValue("@CustomerName", Replace(strFilterCustomerName, "*", "%"))
            daCustomers.Fill(Me)
        End Sub
#End Region

#Region " Functions "
        Public Sub AddAllNone(ByVal blnAll As Boolean, ByVal blnNone As Boolean)
            Dim dr As DataRow
            If blnAll Then
                dr = NewRow()
                dr("Customername") = " - alle - "
                Rows.Add(dr)
            End If
            If blnNone Then
                dr = NewRow()
                dr("Customername") = " - keine - "
                Rows.Add(dr)
            End If
        End Sub
#End Region

    End Class
End Namespace

' ************************************************
' $History: HistoryCustomerList.vb $
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