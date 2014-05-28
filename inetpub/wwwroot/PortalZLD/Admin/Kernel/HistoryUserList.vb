Namespace Kernel
    Public Class HistoryUserList
        REM § Liste von Benutzern zur Verwendung auf Admin-Seiten

        Inherits DataTable

#Region " Constructor "
        Public Sub New(ByVal strUserFilter As String, ByVal strCustomerName As String, ByVal strGroupName As String, ByVal strOrganizationName As String, ByVal blnTestUser As Boolean, ByVal strConnectionString As String)
            Me.New(strUserFilter, strCustomerName, strGroupName, strOrganizationName, blnTestUser, New SqlClient.SqlConnection(strConnectionString))
        End Sub
        Public Sub New(ByVal strUserFilter As String, ByVal strCustomerName As String, ByVal strGroupName As String, ByVal strOrganizationName As String, ByVal intTestUsers As Integer, ByVal cn As SqlClient.SqlConnection)
            If strUserFilter = String.Empty Then strUserFilter = "%"
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            Dim daUser As New SqlClient.SqlDataAdapter("SELECT DISTINCT " & _
                                                        "Username, " & _
                                                        "Reference, " & _
                                                        "TestUser, " & _
                                                        "CustomerName, " & _
                                                        "CustomerAdmin, " & _
                                                        "GroupName, " & _
                                                        "OrganizationName, " & _
                                                        "AccountIsLockedOut, " & _
                                                        "FailedLogins, " & _
                                                        "PwdNeverExpires, " & _
                                                        "LastPwdChange, " & _
                                                        "Created, " & _
                                                        "Deleted, " & _
                                                        "DeleteDate, " & _
                                                        "LastChanged, " & _
                                                        "LastChange, " & _
                                                        "LastChangedBy, " & _
                                                        "UserHistoryID, " & _
                                                        "Password " & _
                                                        "FROM WebUserHistory " & _
                                                        "WHERE UserName LIKE @UserName ", cn)
            daUser.SelectCommand.Parameters.AddWithValue("@UserName", Replace(strUserFilter, "*", "%"))

            If strGroupName <> " - alle - " Then
                daUser.SelectCommand.CommandText &= "AND GroupName = @GroupName "
                daUser.SelectCommand.Parameters.AddWithValue("@GroupName", strGroupName)
            End If

            If strOrganizationName <> " - alle - " Then
                daUser.SelectCommand.CommandText &= "AND OrganizationName = @OrganizationName "
                daUser.SelectCommand.Parameters.AddWithValue("@OrganizationName", strOrganizationName)
            End If

            If strCustomerName <> " - alle - " Then
                daUser.SelectCommand.CommandText &= "AND CustomerName = @CustomerName "
                daUser.SelectCommand.Parameters.AddWithValue("@CustomerName", strCustomerName)
            End If

            If intTestUsers = 1 Then
                daUser.SelectCommand.CommandText &= "AND TestUser = @TestUser "
                daUser.SelectCommand.Parameters.AddWithValue("@TestUser", True)
            ElseIf intTestUsers = 2 Then
                daUser.SelectCommand.CommandText &= "AND TestUser = @TestUser "
                daUser.SelectCommand.Parameters.AddWithValue("@TestUser", False)
            End If

            daUser.Fill(Me)
        End Sub
#End Region

    End Class
End Namespace

' ************************************************
' $History: HistoryUserList.vb $
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
' *****************  Version 5  *****************
' User: Uha          Date: 1.03.07    Time: 16:49
' Updated in $/CKG/Admin/AdminWeb/Kernel
' 
' ************************************************