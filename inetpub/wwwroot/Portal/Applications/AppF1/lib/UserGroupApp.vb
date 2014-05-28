Public Class UserGroupApp

    Public Function GetUserByGroup(ByVal CustomerID As Integer, ByVal GroupID As Integer, _
                                 ByVal cn As SqlClient.SqlConnection) As DataTable


        If cn.State = ConnectionState.Closed Then
            cn.Open()
        End If

        Dim daUser As New SqlClient.SqlDataAdapter("SELECT DISTINCT " & _
                                                           "UserName, " & _
                                                           "GroupName, " & _
                                                           "GroupID, " & _
                                                           "FirstName, " & _
                                                           "LastName, " & _
                                                           "LastLogin, " & _
                                                           "LastPwdChange, " & _
                                                           "AccountIsLockedOut " & _
                                                           "FROM vwWebUserWebMember " & _
                                                   "WHERE CustomerID = @CustomerID and " & _
                                                   "GroupID = @GroupID", cn)


        daUser.SelectCommand.Parameters.AddWithValue("@CustomerID", CustomerID)
        daUser.SelectCommand.Parameters.AddWithValue("@GroupID", GroupID)

        Dim TempTable As New DataTable

        daUser.Fill(TempTable)

        Return TempTable

    End Function

    Public Function GetUserByAppID(ByVal CustomerID As Integer, ByVal AppID As Integer, _
                                 ByVal cn As SqlClient.SqlConnection) As DataTable


        If cn.State = ConnectionState.Closed Then
            cn.Open()
        End If

        Dim SQL As String

        SQL = " SELECT "
        SQL = SQL & "   dbo.WebUser.Username,"
        SQL = SQL & "   dbo.WebUser.FirstName,"
        SQL = SQL & "   dbo.WebUser.LastName,"
        SQL = SQL & "   dbo.WebGroup.GroupName,"
        SQL = SQL & "   dbo.Rights.GroupID,"
        SQL = SQL & "   dbo.WebUser.LastLogin,"
        SQL = SQL & "   dbo.WebUser.LastPwdChange,"
        SQL = SQL & "   dbo.WebUser.AccountIsLockedOut"
        SQL = SQL & " FROM "
        SQL = SQL & "   dbo.WebUser INNER JOIN "
        SQL = SQL & "   dbo.WebMember ON dbo.WebUser.UserID = dbo.WebMember.UserID INNER JOIN "
        SQL = SQL & "   dbo.WebGroup ON dbo.WebMember.GroupID = dbo.WebGroup.GroupID INNER JOIN "
        SQL = SQL & "   dbo.Rights ON dbo.WebGroup.GroupID = dbo.Rights.GroupID INNER JOIN "
        SQL = SQL & "   dbo.Application ON dbo.Rights.AppID = dbo.Application.AppID"
        SQL = SQL & " WHERE"
        SQL = SQL & "   WebGroup.CustomerID = @CustomerID and"
        SQL = SQL & "   Application.AppID = @AppID"
        SQL = SQL & " order by dbo.WebGroup.GroupName"


        Dim daUser As New SqlClient.SqlDataAdapter(SQL, cn)


        daUser.SelectCommand.Parameters.AddWithValue("@CustomerID", CustomerID)
        daUser.SelectCommand.Parameters.AddWithValue("@AppID", AppID)

        Dim TempTable As New DataTable

        daUser.Fill(TempTable)

        Return TempTable

    End Function

    Public Function GetUserByCustomerID(ByVal CustomerID As Integer, _
                                 ByVal cn As SqlClient.SqlConnection) As DataTable


        If cn.State = ConnectionState.Closed Then
            cn.Open()
        End If
        Dim daUser As New SqlClient.SqlDataAdapter("SELECT DISTINCT " & _
                                                           "UserName, " & _
                                                           "GroupName, " & _
                                                           "GroupID, " & _
                                                           "FirstName, " & _
                                                           "LastName, " & _
                                                           "LastLogin, " & _
                                                           "LastPwdChange, " & _
                                                           "AccountIsLockedOut " & _
                                                           "FROM vwWebUserWebMember " & _
                                                   "WHERE CustomerID = @CustomerID ", cn)


        daUser.SelectCommand.Parameters.AddWithValue("@CustomerID", CustomerID)

        Dim TempTable As New DataTable

        daUser.Fill(TempTable)

        Return TempTable

    End Function

    Public Function GetGroups(ByVal CustomerID As Integer, _
                              ByVal cn As SqlClient.SqlConnection) As DataTable

        If cn.State = ConnectionState.Closed Then
            cn.Open()
        End If

        Dim SQL As String

        SQL = " SELECT "
        SQL = SQL & "   WebGroup.GroupID,"
        SQL = SQL & "   WebGroup.GroupName"
        SQL = SQL & " FROM "
        SQL = SQL & "   WebGroup INNER JOIN "
        SQL = SQL & "   Customer ON WebGroup.CustomerID = Customer.CustomerID"
        SQL = SQL & " WHERE"
        SQL = SQL & "   Customer.CustomerID = @CustomerID"
        SQL = SQL & "   ORDER BY WebGroup.GroupName"


        Dim daUser As New SqlClient.SqlDataAdapter(SQL, cn)


        daUser.SelectCommand.Parameters.AddWithValue("@CustomerID", CustomerID)

        Dim TempTable As New DataTable

        daUser.Fill(TempTable)


        Return TempTable

    End Function

    Public Function GetApplications(ByVal CustomerID As Integer, _
                          ByVal cn As SqlClient.SqlConnection) As DataTable

        If cn.State = ConnectionState.Closed Then
            cn.Open()
        End If

        Dim SQL As String

        SQL = " SELECT DISTINCT"
        SQL = SQL & "   AppID,"
        SQL = SQL & "   AppFriendlyName"
        SQL = SQL & " FROM vwCustomerAppAssigned"
        SQL = SQL & " WHERE"
        SQL = SQL & "   CustomerID = @CustomerID AND AppParent=0 Order By AppFriendlyName"

        Dim daUser As New SqlClient.SqlDataAdapter(SQL, cn)
        daUser.SelectCommand.Parameters.AddWithValue("@CustomerID", CustomerID)

        Dim TempTable As New DataTable
        daUser.Fill(TempTable)

        Return TempTable

    End Function

    Public Function GetApplicationsPerGroup(ByVal GroupID As Integer, _
                      ByVal cn As SqlClient.SqlConnection) As DataTable

        If cn.State = ConnectionState.Closed Then
            cn.Open()
        End If

        Dim SQL As String

        SQL = " SELECT DISTINCT"
        SQL = SQL & "   AppID,"
        SQL = SQL & "   AppFriendlyName"
        SQL = SQL & " FROM vwGroupAppAssigned"
        SQL = SQL & " WHERE"
        SQL = SQL & "   GroupID = @GroupID AND AppParent=0 Order By AppFriendlyName"

        Dim daUser As New SqlClient.SqlDataAdapter(SQL, cn)
        daUser.SelectCommand.Parameters.AddWithValue("@GroupID", GroupID)

        Dim TempTable As New DataTable
        daUser.Fill(TempTable)

        Return TempTable

    End Function

End Class
