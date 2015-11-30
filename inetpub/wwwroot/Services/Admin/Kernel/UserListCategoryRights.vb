Option Explicit On
Option Strict On

Namespace Kernel
    Public Class UserListCategoryRights
        REM § Liste von Benutzern zur Verwendung auf Admin-Seiten

        Inherits DataTable

#Region " Constructor "
        Public Sub New(ByVal strUserFilter As String, ByVal intCustomerID As Integer, ByVal intGroupID As Integer, ByVal intOrganizationID As Integer, ByVal cn As SqlClient.SqlConnection, ByVal Employee As Boolean, ByVal Hierarchy As Integer, ByVal AccountingArea As Integer, strCategoryName As String, nurMitRechten As Boolean, Optional ByVal blnLoggedOn As Boolean = False)
            Me.New(strUserFilter, intCustomerID, intGroupID, intOrganizationID, False, String.Empty, cn, String.Empty, Employee, Hierarchy, AccountingArea, strCategoryName, nurMitRechten, blnLoggedOn)
        End Sub

        Public Sub New(ByVal strUserFilter As String, ByVal intCustomerID As Integer, ByVal intGroupID As Integer, ByVal intOrganizationID As Integer, ByVal notApproved As Boolean, ByVal notCreatedBy As String, ByVal cn As SqlClient.SqlConnection, ByVal strKundenReferenz As String, ByVal blnEmployee As Boolean, ByVal intHierarchy As Integer, ByVal intAccountingArea As Integer, strCategoryName As String, nurMitRechten As Boolean, Optional ByVal blnLoggedOn As Boolean = False, Optional ByVal onlyDisabledUser As Boolean = False, _
                       Optional ByVal lastLogin As Date = Nothing)
            If strUserFilter = String.Empty Then strUserFilter = "%"
            'If strKundenReferenz = String.Empty Then strKundenReferenz = "%"
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            '<§§§JVE 07.11.2005: Select um Customer-ID erweitert...>
            '20130121 MCa: Select um SelfAdministration, SelfAdm erweitert
            Dim daUser As New SqlClient.SqlDataAdapter("SELECT DISTINCT UserID, " & _
                                                               "Title, " & _
                                                               "FirstName, " & _
                                                               "LastName, " & _
                                                               "CustomerID, " & _
                                                               "CustomerName, " & _
                                                               "ISNULL(SelfAdministration, 0) SelfAdministration, " & _
                                                               "AccountingArea, " & _
                                                               "m.UserName, " & _
                                                               "GroupName, " & _
                                                               "OrganizationName, " & _
                                                               "Reference, " & _
                                                               "CustomerAdmin, " & _
                                                               "TestUser, " & _
                                                               "LastPwdChange, " & _
                                                               "LastLogin, " & _
                                                               "PwdNeverExpires, " & _
                                                               "FailedLogins, " & _
                                                               "AccountIsLockedOut, " & _
                                                               "LoggedOn, " & _
                                                               "URLRemoteLoginKey, " & _
                                                               "CreatedBy, " & _
                                                               "c.HasRights HasCategoryRights, " & _
                                                               "c.CreateUserName CategoryRightsCreateUser, " & _
                                                               "c.CreateDate CategoryRightsCreateDate " & _
                                                               "FROM vwWebUserWebMember m " & _
                                                               "inner join WebUserCategoryRights c on m.UserName = c.UserName and c.CategoryName = '" & strCategoryName & "' " & _
                                                       "WHERE m.UserName LIKE @UserName ", cn)
            '" AND Reference LIKE @Reference "

            daUser.SelectCommand.Parameters.AddWithValue("@UserName", Replace(strUserFilter, "*", "%"))
            'daUser.SelectCommand.Parameters.AddWithValue("@Reference", Replace(strKundenReferenz, "*", "%"))

            If nurMitRechten Then
                daUser.SelectCommand.CommandText &= " AND c.HasRights = 1 "
            End If

            If Not strKundenReferenz = String.Empty And Not strKundenReferenz = "*" Then
                daUser.SelectCommand.CommandText &= " AND Reference LIKE @Reference "
                daUser.SelectCommand.Parameters.AddWithValue("@Reference", Replace(strKundenReferenz, "*", "%"))
            End If

            If intGroupID > 0 Then
                daUser.SelectCommand.CommandText &= "AND GroupID = @GroupID "
                daUser.SelectCommand.Parameters.AddWithValue("@GroupID", intGroupID)
                'ElseIf intGroupID < 0 Then
                '    daUser.SelectCommand.CommandText &= "AND GroupID IS NULL "
            End If

            If intOrganizationID > 0 Then
                daUser.SelectCommand.CommandText &= "AND OrganizationID = @OrganizationID "
                daUser.SelectCommand.Parameters.AddWithValue("@OrganizationID", intOrganizationID)
                'ElseIf intOrganizationID < 0 Then
                '    daUser.SelectCommand.CommandText &= "AND OrganizationID IS NULL "
            End If

            If intCustomerID > 0 Then
                daUser.SelectCommand.CommandText &= "AND CustomerId = @CustomerId "
                daUser.SelectCommand.Parameters.AddWithValue("@CustomerID", intCustomerID)
                'ElseIf intCustomerID < 0 Then
                '    daUser.SelectCommand.CommandText &= "AND CustomerId IS NULL "
            End If

            If blnEmployee Then
                daUser.SelectCommand.CommandText &= "AND employee = 1 "

                If intHierarchy > 0 Then
                    daUser.SelectCommand.CommandText &= "AND employeeHierarchy = @employeeHierarchy "
                    daUser.SelectCommand.Parameters.AddWithValue("@employeeHierarchy", intHierarchy)
                End If
            End If

            If blnLoggedOn Then
                daUser.SelectCommand.CommandText &= "AND LoggedOn = 1 "
            End If

            If intAccountingArea > 0 Then
                daUser.SelectCommand.CommandText &= "AND AccountingArea = @AccountingArea "
                daUser.SelectCommand.Parameters.AddWithValue("@AccountingArea", intAccountingArea)
            End If

            If notApproved Then
                daUser.SelectCommand.CommandText &= "AND Approved = 0 "
            End If



            If onlyDisabledUser Then
                daUser.SelectCommand.CommandText &= "AND AccountIsLockedOut = 1 "
            End If

            If Not lastLogin = Nothing Then
                daUser.SelectCommand.CommandText &= "AND LastLogin <= @LastLogin"
                daUser.SelectCommand.Parameters.AddWithValue("@LastLogin", lastLogin)
            End If




            daUser.Fill(Me)
        End Sub



        Public Sub New(ByVal strUserFilter As String, ByVal strFilterFirstName As String, ByVal strFilterLastName As String, ByVal strFilterMail As String,
                       ByVal intCustomerID As Integer, ByVal intGroupID As Integer, ByVal intOrganizationID As Integer, ByVal notApproved As Boolean,
                       ByVal notCreatedBy As String, ByVal cn As SqlClient.SqlConnection, ByVal strKundenReferenz As String, ByVal blnEmployee As Boolean,
                       ByVal intHierarchy As Integer, ByVal intAccountingArea As Integer, Optional ByVal blnLoggedOn As Boolean = False,
                       Optional ByVal onlyDisabledUser As Boolean = False, Optional ByVal lastLogin As Date = Nothing)
            If strUserFilter = String.Empty Then strUserFilter = "%"
            'If strFilterFirstName = String.Empty Then strFilterFirstName = "%"
            'If strFilterLastName = String.Empty Then strFilterLastName = "%"
            'If strFilterMail = String.Empty Then strFilterMail = "%"
            'If strKundenReferenz = String.Empty Then strKundenReferenz = "%"

            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            '<§§§JVE 07.11.2005: Select um Customer-ID erweitert...>
            '20130121 MCa: Select um SelfAdministration, SelfAdm erweitert
            Dim daUser As New SqlClient.SqlDataAdapter("SELECT DISTINCT UserID, " & _
                                                                "FirstName, " & _
                                                                "LastName, " & _
                                                                "mail, " & _
                                                                "telephone, " & _
                                                                "telephone2, " & _
                                                               "CustomerID, " & _
                                                               "CustomerName, " & _
                                                               "ISNULL(SelfAdministration, 0) SelfAdministration, " & _
                                                               "AccountingArea, " & _
                                                               "UserName, " & _
                                                               "GroupName, " & _
                                                               "OrganizationName, " & _
                                                               "Reference, " & _
                                                               "CustomerAdmin, " & _
                                                               "TestUser, " & _
                                                               "LastPwdChange, " & _
                                                                "LastLogin, " & _
                                                               "PwdNeverExpires, " & _
                                                               "FailedLogins, " & _
                                                               "AccountIsLockedOut, " & _
                                                               "LoggedOn, " & _
                                                               "URLRemoteLoginKey, " & _
                                                               "CreatedBy " & _
                                                               "FROM vwWebUserWebMember " & _
                                                       "WHERE UserName LIKE @UserName ", cn)
            '" AND FirstName LIKE @FirstName " & _
            '" AND LastName LIKE @LastName " & _
            '" AND mail LIKE @Mail " & _
            '" AND Reference LIKE @Reference "


            daUser.SelectCommand.Parameters.AddWithValue("@UserName", Replace(strUserFilter, "*", "%"))
            'daUser.SelectCommand.Parameters.AddWithValue("@FirstName", Replace(strFilterFirstName, "*", "%"))
            'daUser.SelectCommand.Parameters.AddWithValue("@LastName", Replace(strFilterLastName, "*", "%"))
            'daUser.SelectCommand.Parameters.AddWithValue("@Mail", Replace(strFilterMail, "*", "%"))
            'daUser.SelectCommand.Parameters.AddWithValue("@Reference", Replace(strKundenReferenz, "*", "%"))

            If Not strFilterFirstName = String.Empty And Not strFilterFirstName = "*" Then
                daUser.SelectCommand.CommandText &= " AND FirstName LIKE @FirstName "
                daUser.SelectCommand.Parameters.AddWithValue("@FirstName", Replace(strFilterFirstName, "*", "%"))
            End If

            If Not strFilterLastName = String.Empty And Not strFilterLastName = "*" Then
                daUser.SelectCommand.CommandText &= " AND LastName LIKE @LastName "
                daUser.SelectCommand.Parameters.AddWithValue("@LastName", Replace(strFilterLastName, "*", "%"))
            End If

            If Not strFilterMail = String.Empty And Not strFilterMail = "*" Then
                daUser.SelectCommand.CommandText &= " AND mail LIKE @Mail "
                daUser.SelectCommand.Parameters.AddWithValue("@Mail", Replace(strFilterMail, "*", "%"))
            End If

            If Not strKundenReferenz = String.Empty And Not strKundenReferenz = "*" Then
                daUser.SelectCommand.CommandText &= " AND Reference LIKE @Reference "
                daUser.SelectCommand.Parameters.AddWithValue("@Reference", Replace(strKundenReferenz, "*", "%"))
            End If



            If intGroupID > 0 Then
                daUser.SelectCommand.CommandText &= "AND GroupID = @GroupID "
                daUser.SelectCommand.Parameters.AddWithValue("@GroupID", intGroupID)
            ElseIf intGroupID < 0 Then
                daUser.SelectCommand.CommandText &= "AND GroupID IS NULL "
            End If

            If intOrganizationID > 0 Then
                daUser.SelectCommand.CommandText &= "AND OrganizationID = @OrganizationID "
                daUser.SelectCommand.Parameters.AddWithValue("@OrganizationID", intOrganizationID)
            ElseIf intOrganizationID < 0 Then
                daUser.SelectCommand.CommandText &= "AND OrganizationID IS NULL "
            End If

            If intCustomerID > 0 Then
                daUser.SelectCommand.CommandText &= "AND CustomerId = @CustomerId "
                daUser.SelectCommand.Parameters.AddWithValue("@CustomerID", intCustomerID)
            ElseIf intCustomerID < 0 Then
                daUser.SelectCommand.CommandText &= "AND CustomerId IS NULL "
            End If

            If blnEmployee Then
                daUser.SelectCommand.CommandText &= "AND employee = 1 "

                If intHierarchy > 0 Then
                    daUser.SelectCommand.CommandText &= "AND employeeHierarchy = @employeeHierarchy "
                    daUser.SelectCommand.Parameters.AddWithValue("@employeeHierarchy", intHierarchy)
                End If
            End If

            If blnLoggedOn Then
                daUser.SelectCommand.CommandText &= "AND LoggedOn = 1 "
            End If

            If intAccountingArea > 0 Then
                daUser.SelectCommand.CommandText &= "AND AccountingArea = @AccountingArea "
                daUser.SelectCommand.Parameters.AddWithValue("@AccountingArea", intAccountingArea)
            End If

            If notApproved Then
                daUser.SelectCommand.CommandText &= "AND Approved = 0 "
            End If

            If onlyDisabledUser Then
                daUser.SelectCommand.CommandText &= "AND AccountIsLockedOut = 1 "
            End If

            If Not lastLogin = Nothing Then
                daUser.SelectCommand.CommandText &= "AND LastLogin <= @LastLogin"
                daUser.SelectCommand.Parameters.AddWithValue("@LastLogin", lastLogin)
            End If




            daUser.Fill(Me)
        End Sub

#End Region

    End Class
End Namespace
