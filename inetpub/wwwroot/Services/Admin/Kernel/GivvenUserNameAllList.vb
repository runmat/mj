Namespace Kernel
    Public Class GivvenUserNameAllList
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
            Dim daGivvenUserNames As New SqlClient.SqlDataAdapter("SELECT UserName FROM WebUser WHERE (LEN(LTRIM(RTRIM(UserName))) > 0) ", cn)
            daGivvenUserNames.Fill(Me)
        End Sub
        Public Sub New(ByVal strGivvenUserNameFilter As String, ByVal strConnectionString As String)
            Me.New(strGivvenUserNameFilter, New SqlClient.SqlConnection(strConnectionString))
        End Sub
        Public Sub New(ByVal strGivvenUserNameFilter As String, ByVal cn As SqlClient.SqlConnection)
            If strGivvenUserNameFilter = String.Empty Then strGivvenUserNameFilter = "%"
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            Dim strSQL As String
            strSQL = "SELECT UserName FROM WebUser WHERE (LEN(LTRIM(RTRIM(UserName))) > 0) " & _
                     "AND (UserName LIKE @GivvenUserNameName)"
            Dim daGivvenUserNames As New SqlClient.SqlDataAdapter(strSQL, cn)
            With daGivvenUserNames.SelectCommand.Parameters
                .AddWithValue("@GivvenUserNameName", Replace(strGivvenUserNameFilter, "*", "%"))
            End With
            daGivvenUserNames.Fill(Me)
        End Sub

#End Region

#Region " Functions "

#End Region
    End Class
End Namespace