Namespace Kernel
    Public Class GivvenGroupAllList
        REM § Auflistungsobjekt für verbotene Benutzernamen zur Verwendung in den Administrationsseiten

        Inherits DataTable

#Region " Constructor "
        Public Sub New(ByVal strConnectionString As String)
            Me.New(New SqlClient.SqlConnection(strConnectionString))
        End Sub
        Public Sub New(ByVal cn As SqlClient.SqlConnection)
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            Dim daGivvenGroups As New SqlClient.SqlDataAdapter("SELECT GroupName FROM WebGroup WHERE (LEN(LTRIM(RTRIM(GroupName))) > 0) ", cn)
            daGivvenGroups.Fill(Me)
        End Sub
        Public Sub New(ByVal strGivvenGroupFilter As String, ByVal strConnectionString As String)
            Me.New(strGivvenGroupFilter, New SqlClient.SqlConnection(strConnectionString))
        End Sub
        Public Sub New(ByVal strGivvenGroupFilter As String, ByVal cn As SqlClient.SqlConnection)
            If strGivvenGroupFilter = String.Empty Then strGivvenGroupFilter = "%"
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            Dim strSQL As String
            strSQL = "SELECT GroupName FROM WebGroup WHERE (LEN(LTRIM(RTRIM(GroupName))) > 0) " & _
                     "AND (GroupName LIKE @GivvenGroupName)"
            Dim daGivvenGroups As New SqlClient.SqlDataAdapter(strSQL, cn)
            With daGivvenGroups.SelectCommand.Parameters
                .AddWithValue("@GivvenGroupName", Replace(strGivvenGroupFilter, "*", "%"))
            End With
            daGivvenGroups.Fill(Me)
        End Sub
        Public Sub New(ByVal cn As SqlClient.SqlConnection, ByVal CustomerID As String)

            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            Dim strSQL As String
            strSQL = "SELECT GroupName FROM WebGroup WHERE (LEN(LTRIM(RTRIM(GroupName))) > 0) " & _
                     "AND (CustomerID = @CustomerID)"
            Dim daGivvenGroups As New SqlClient.SqlDataAdapter(strSQL, cn)
            With daGivvenGroups.SelectCommand.Parameters
                .AddWithValue("@CustomerID", CustomerID)
            End With
            daGivvenGroups.Fill(Me)
        End Sub
#End Region

#Region " Functions "

#End Region
    End Class
End Namespace