Namespace Kernel
    Public Class HistoryOrganizationList
        REM § Auflistungsobjekt für Organisationen zur Verwendung in den Administrationsseiten

        Inherits DataTable

#Region " Constructor "
        Public Sub New(ByVal strCustomerName As String, ByVal strConnectionString As String, Optional ByVal blnAll As Boolean = False, Optional ByVal blnNone As Boolean = False)
            Me.New(strCustomerName, New SqlClient.SqlConnection(strConnectionString), blnAll, blnNone)
        End Sub
        Public Sub New(ByVal strCustomerName As String, ByVal cn As SqlClient.SqlConnection, Optional ByVal blnAll As Boolean = False, Optional ByVal blnNone As Boolean = False)
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            Dim daOrganizations As New SqlClient.SqlDataAdapter("SELECT TOP 100 PERCENT OrganizationName " & _
                                                          "FROM dbo.WebUserHistory " & _
                                                          "GROUP BY OrganizationName, Customername " & _
                                                          "HAVING (CustomerName = @CustomerName) " & _
                                                          "ORDER BY OrganizationName", cn)
            daOrganizations.SelectCommand.Parameters.AddWithValue("@CustomerName", strCustomerName)
            daOrganizations.Fill(Me)
            AddAllNone(blnAll, blnNone)
        End Sub

        Public Sub New(ByVal strOrganizationFilter As String, ByVal strCustomerName As String, ByVal strConnectionString As String)
            Me.New(strOrganizationFilter, strCustomerName, New SqlClient.SqlConnection(strConnectionString))
        End Sub
        Public Sub New(ByVal strOrganizationFilter As String, ByVal strCustomerName As String, ByVal cn As SqlClient.SqlConnection)
            If strOrganizationFilter = String.Empty Then strOrganizationFilter = "%"
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            Dim strSQL As String
            If strCustomerName.Length > 0 Then
                strSQL = "SELECT TOP 100 PERCENT OrganizationName " & _
                         "FROM dbo.WebUserHistory " & _
                         "GROUP BY OrganizationName, Customername " & _
                         "HAVING (Customername = @Customername) AND (OrganizationName LIKE @OrganizationName) " & _
                         "ORDER BY OrganizationName"
            ElseIf strCustomerName.Length = 0 Then
                strSQL = "SELECT TOP 100 PERCENT OrganizationName " & _
                         "FROM dbo.WebUserHistory " & _
                         "GROUP BY OrganizationName, Customername " & _
                         "HAVING (OrganizationName LIKE @OrganizationName) " & _
                         "ORDER BY OrganizationName"
            Else
                strSQL = "SELECT TOP 100 PERCENT OrganizationName " & _
                         "FROM dbo.WebUserHistory " & _
                         "GROUP BY OrganizationName, Customername " & _
                         "HAVING (Customername IS NULL) AND (OrganizationName LIKE @OrganizationName) " & _
                         "ORDER BY OrganizationName"
            End If
            Dim daOrganizations As New SqlClient.SqlDataAdapter(strSQL, cn)
            With daOrganizations.SelectCommand.Parameters
                .AddWithValue("@CustomerName", strCustomerName)
                .AddWithValue("@OrganizationName", Replace(strOrganizationFilter, "*", "%"))
            End With
            daOrganizations.Fill(Me)
        End Sub

#End Region

#Region " Functions "
        Public Sub AddAllNone(ByVal blnAll As Boolean, ByVal blnNone As Boolean)
            Dim dr As DataRow
            If blnAll Then
                dr = NewRow()
                dr("OrganizationName") = " - alle - "
                Rows.Add(dr)
            End If
            If blnNone Then
                dr = NewRow()
                dr("OrganizationName") = " - keine - "
                Rows.Add(dr)
            End If
        End Sub
#End Region
    End Class
End Namespace

' ************************************************
' $History: HistoryOrganizationList.vb $
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