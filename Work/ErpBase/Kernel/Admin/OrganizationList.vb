Namespace Kernel.Admin
    Public Class OrganizationList
        REM § Auflistungsobjekt für Organisationen zur Verwendung in den Administrationsseiten

        Inherits DataTable

#Region " Constructor "
        Public Sub New(ByVal intCustomerID As Integer, ByVal strConnectionString As String, ByVal intAccountingArea As Integer, Optional ByVal blnAll As Boolean = False, Optional ByVal blnNone As Boolean = False)
            Me.New(intCustomerID, New SqlClient.SqlConnection(strConnectionString), intAccountingArea, blnAll, blnNone)
        End Sub
        Public Sub New(ByVal intCustomerID As Integer, ByVal cn As SqlClient.SqlConnection, ByVal intAccountingArea As Integer, Optional ByVal blnAll As Boolean = False, Optional ByVal blnNone As Boolean = False)
            Try
                If cn.State = ConnectionState.Closed Then
                    cn.Open()
                End If
                Dim daOrganizations As SqlClient.SqlDataAdapter
                If intAccountingArea > -1 Then
                    daOrganizations = New SqlClient.SqlDataAdapter("SELECT * " & _
                                                                 "FROM vwOrganization " & _
                                                                 "WHERE (CustomerID = @CustomerID) " & _
                                                                 "AND (AccountingArea = @AccountingArea)", cn)
                    daOrganizations.SelectCommand.Parameters.AddWithValue("@AccountingArea", intAccountingArea)
                Else
                    daOrganizations = New SqlClient.SqlDataAdapter("SELECT * " & _
                                                                 "FROM vwOrganization " & _
                                                                 "WHERE (CustomerID = @CustomerID)", cn)
                End If
                daOrganizations.SelectCommand.Parameters.AddWithValue("@CustomerID", intCustomerID)
                daOrganizations.Fill(Me)
                AddAllNone(blnAll, blnNone)
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End Sub
        Public Sub New(ByVal strOrganizationFilter As String, ByVal intCustomerID As Integer, ByVal strConnectionString As String, ByVal intAccountingArea As Integer)
            Me.New(strOrganizationFilter, intCustomerID, New SqlClient.SqlConnection(strConnectionString), intAccountingArea)
        End Sub
        Public Sub New(ByVal strOrganizationFilter As String, ByVal intCustomerID As Integer, ByVal cn As SqlClient.SqlConnection, ByVal intAccountingArea As Integer)
            Try
                If strOrganizationFilter = String.Empty Then strOrganizationFilter = "%"
                If cn.State = ConnectionState.Closed Then
                    cn.Open()
                End If
                Dim strSQL As String
                If intCustomerID > 0 Then
                    strSQL = "SELECT * " & _
                             "FROM vwOrganization " & _
                             "WHERE (CustomerID = @CustomerID) " & _
                               "AND (OrganizationName LIKE @OrganizationName)"
                ElseIf intCustomerID = 0 Then
                    strSQL = "SELECT * " & _
                             "FROM vwOrganization " & _
                             "WHERE (OrganizationName LIKE @OrganizationName)"
                Else
                    strSQL = "SELECT * " & _
                             "FROM vwOrganization " & _
                             "WHERE (CustomerID IS NULL) " & _
                               "AND (OrganizationName LIKE @OrganizationName)"
                End If
                If intAccountingArea > -1 Then
                    strSQL &= " AND (AccountingArea = @AccountingArea)"
                End If
                Dim daOrganizations As New SqlClient.SqlDataAdapter(strSQL, cn)
                With daOrganizations.SelectCommand.Parameters
                    .AddWithValue("@CustomerID", intCustomerID)
                    .AddWithValue("@OrganizationName", Replace(strOrganizationFilter, "*", "%"))
                    If intAccountingArea > -1 Then
                        .AddWithValue("@AccountingArea", intAccountingArea)
                    End If
                End With
                daOrganizations.Fill(Me)
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End Sub

#End Region

#Region " Functions "
        Public Sub AddAllNone(ByVal blnAll As Boolean, ByVal blnNone As Boolean)
            Dim dr As DataRow
            If blnAll Then
                dr = NewRow()
                dr("OrganizationID") = 0
                dr("OrganizationName") = " - alle - "
                Rows.Add(dr)
            End If
            If blnNone Then
                dr = NewRow()
                dr("OrganizationID") = -1
                dr("OrganizationName") = " - keine - "
                Rows.Add(dr)
            End If
        End Sub
#End Region
    End Class
End Namespace

' ************************************************
' $History: OrganizationList.vb $
' 
' *****************  Version 3  *****************
' User: Hartmannu    Date: 9.09.08    Time: 13:42
' Updated in $/CKAG/Base/Kernel/Admin
' ITA 2152 und 2158
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 11.04.08   Time: 11:57
' Updated in $/CKAG/Base/Kernel/Admin
' Migration OR
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 3.04.08    Time: 16:42
' Created in $/CKAG/Base/Kernel/Admin
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 6.12.07    Time: 12:47
' Updated in $/CKG/Base/Base/Kernel/Admin
' ITA:1440
' 
' *****************  Version 3  *****************
' User: Uha          Date: 1.03.07    Time: 16:32
' Updated in $/CKG/Base/Base/Kernel/Admin
' 
' ************************************************