Imports System.Configuration
Imports System.Web.Mail

Namespace Kernel.Security
    <Serializable()> Public Class User
        REM § Enthält Daten eines einzelnen Benutzers.

#Region " Membervariables "
        Private m_strConnectionstring As String

        Private m_intUserId As Int32 = -1
        Private m_intUserHistoryID As Int32 = -1
        Private m_strUsername As String
        Private m_blnIsCustomerAdmin As Boolean = False
        Private m_blnTestUser As Boolean = False
        Private m_dtmLastPwdChange As DateTime
        Private m_blnPwdNeverExpires As Boolean = False
        Private m_intFailedLogins As Integer
        Private m_blnAccountIsLockedOut As Boolean
        Private m_strAccountIsLockedBy As String
        Private m_strReference As String
        Private m_blnLoggedOn As Boolean
        Private m_blnMatrixfilled As Boolean
        Private m_blnPasswordExpired As Boolean = False
        Private m_blnInitialPassword As Boolean = False
        Private m_groups As Groups
        Private m_intGroupID As Int32
        <NonSerialized()> Private m_blnFirstLevelAdmin As Boolean

        Private m_intReadMessageCount As Int32
        Private m_intReadCustomerMessageCount As Int32

        Private m_tblApplications As New DataTable()
        Private m_strErrorMessage As String
        Private m_app As App
        Private m_customer As Customer
        Private m_organization As Organization
        Private m_blnOrganizationAdmin As Boolean
        Private m_dtmLastLogin As DateTime
        Private m_mail As String = ""  'Emailadresse
        Private m_telephone As String = ""
        <NonSerialized()> Private m_strSessionID As String
        <NonSerialized()> Private m_strCreatedBy As String
        <NonSerialized()> Private m_blnDoubleLoginTry As Boolean
        <NonSerialized()> Private m_intCurrentLogAccessASPXID As Integer
        <NonSerialized()> Private m_intQuestionID As Integer
        <NonSerialized()> Private m_strAnswerText As String
        Private m_approved As Boolean
        Private m_firstname As String
        Private m_lastname As String
        Private m_title As String
        Private m_store As String
        Private m_ValidFrom As String
        Private m_strUrlRemoteLoginKey As String

        <NonSerialized()> Private m_blnEmployee As Boolean = False
        <NonSerialized()> Private m_blnPicture As Boolean = False
        <NonSerialized()> Private m_intHierarchyID As Integer = 1
        <NonSerialized()> Private m_strDepartment As String = ""
        <NonSerialized()> Private m_strPosition As String = ""
        <NonSerialized()> Private m_strPhoneEmployee As String = ""
        <NonSerialized()> Private m_strFax As String = ""
#End Region

#Region " Constructor "
        Public Sub New()
            m_blnDoubleLoginTry = False
            m_strConnectionstring = ConfigurationManager.AppSettings("Connectionstring")
            m_intGroupID = 0
        End Sub
        Public Sub New(ByVal intUserId As Integer, ByVal strConnectionString As String)
            Me.New(intUserId, New SqlClient.SqlConnection(strConnectionString))
        End Sub
        Public Sub New(ByVal intUserId As Integer, ByVal cn As SqlClient.SqlConnection)
            m_blnDoubleLoginTry = False
            m_strConnectionstring = cn.ConnectionString
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            GetUserDataFromId(intUserId, cn)
        End Sub
        Public Sub New(ByVal intUserId As Integer)
            m_blnDoubleLoginTry = False
            m_strConnectionstring = ConfigurationManager.AppSettings("Connectionstring")
            Dim cn As New SqlClient.SqlConnection(m_strConnectionstring)
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            GetUserDataFromId(intUserId, cn)
        End Sub
        Public Sub New( _
                       ByVal intUserId As Integer, _
                       ByVal strUserName As String, _
                       ByVal strReference As String, _
                       ByVal blnTestUser As Boolean, _
                       ByVal intCustomerId As Integer, _
                       ByVal blnCustomerAdmin As Boolean, _
                       ByVal blnPwdNeverExpires As Boolean, _
                       ByVal blnAccountIsLockedOut As Boolean, _
                       ByVal blnFirstLevelAdmin As Boolean, _
                       ByVal blnLoggedOn As Boolean, _
                       ByVal blnOrganizationAdmin As Boolean, _
                       ByVal strConnectionString As String, _
                       ByVal intReadMessageCount As Int32, _
                       ByVal strCreatedBy As String, _
                       ByVal blnApproved As Boolean, _
                       ByVal strFirstName As String, _
                       ByVal strLastName As String, _
                       ByVal strTitle As String, _
                       ByVal strStore As String, _
                       ByVal blnMatrixfilled As Boolean, _
                       ByVal strValidFrom As String, _
                       ByVal intReadCustomerMessageCount As Int32)

            m_blnDoubleLoginTry = False
            m_intUserId = intUserId
            m_strUsername = strUserName
            m_strReference = strReference
            m_blnTestUser = blnTestUser
            m_blnIsCustomerAdmin = blnCustomerAdmin
            m_blnPwdNeverExpires = blnPwdNeverExpires
            m_blnAccountIsLockedOut = blnAccountIsLockedOut
            m_blnFirstLevelAdmin = blnFirstLevelAdmin
            m_blnLoggedOn = blnLoggedOn
            m_intReadMessageCount = intReadMessageCount
            m_intReadCustomerMessageCount = intReadCustomerMessageCount
            m_strConnectionstring = strConnectionString
            'Customer
            m_customer = New Customer(intCustomerId, m_strConnectionstring)
            'Gruppe
            m_groups = New Groups(Me, m_strConnectionstring)
            'Organization
            m_organization = New Organization(-1, m_strConnectionstring, intUserId)
            m_blnOrganizationAdmin = blnOrganizationAdmin
            'Angelegt von
            m_strCreatedBy = strCreatedBy
            '
            m_approved = blnApproved
            m_firstname = strFirstName
            m_lastname = strLastName
            m_title = strTitle
            m_store = strStore
            m_blnMatrixfilled = blnMatrixfilled
            m_ValidFrom = strValidFrom
        End Sub

        Public Sub New( _
                       ByVal intUserId As Integer, _
                       ByVal strUserName As String, _
                       ByVal strReference As String, _
                       ByVal blnTestUser As Boolean, _
                       ByVal intCustomerId As Integer, _
                       ByVal blnCustomerAdmin As Boolean, _
                       ByVal blnPwdNeverExpires As Boolean, _
                       ByVal blnAccountIsLockedOut As Boolean, _
                       ByVal blnFirstLevelAdmin As Boolean, _
                       ByVal blnLoggedOn As Boolean, _
                       ByVal blnOrganizationAdmin As Boolean, _
                       ByVal strConnectionString As String, _
                       ByVal intReadMessageCount As Int32, _
                       ByVal strCreatedBy As String, _
                       ByVal blnApproved As Boolean, _
                       ByVal strStore As String, _
                       ByVal blnMatrixfilled As Boolean, _
                       ByVal strValidFrom As String, _
                       ByVal intReadCustomerMessageCount As Int32)

            m_blnDoubleLoginTry = False
            m_intUserId = intUserId
            m_strUsername = strUserName
            m_strReference = strReference
            m_blnTestUser = blnTestUser
            m_blnIsCustomerAdmin = blnCustomerAdmin
            m_blnPwdNeverExpires = blnPwdNeverExpires
            m_blnAccountIsLockedOut = blnAccountIsLockedOut
            m_blnFirstLevelAdmin = blnFirstLevelAdmin
            m_blnLoggedOn = blnLoggedOn
            m_intReadMessageCount = intReadMessageCount
            m_intReadCustomerMessageCount = intReadCustomerMessageCount
            m_strConnectionstring = strConnectionString
            'Customer
            m_customer = New Customer(intCustomerId, m_strConnectionstring)
            'Gruppe
            m_groups = New Groups(Me, m_strConnectionstring)
            'Organization
            m_organization = New Organization(-1, m_strConnectionstring, intUserId)
            m_blnOrganizationAdmin = blnOrganizationAdmin
            'Angelegt von
            m_strCreatedBy = strCreatedBy
            '
            m_approved = blnApproved
            m_firstname = ""
            m_lastname = ""
            m_title = ""
            m_store = strStore
            m_blnMatrixfilled = blnMatrixfilled
            m_ValidFrom = strValidFrom
        End Sub
#End Region

#Region " Properties "
        '---------- Employee Properties - START -----------------
        Public Property Employee() As Boolean
            Get
                Return m_blnEmployee
            End Get
            Set(ByVal value As Boolean)
                m_blnEmployee = value
            End Set
        End Property

        Public Property Picture() As Boolean
            Get
                Return m_blnPicture
            End Get
            Set(ByVal value As Boolean)
                m_blnPicture = value
            End Set
        End Property

        Public Property HierarchyID() As Integer
            Get
                Return m_intHierarchyID
            End Get
            Set(ByVal value As Integer)
                m_intHierarchyID = value
            End Set
        End Property

        Public Property Department() As String
            Get
                Return m_strDepartment
            End Get
            Set(ByVal value As String)
                m_strDepartment = value
            End Set
        End Property

        Public Property Position() As String
            Get
                Return m_strPosition
            End Get
            Set(ByVal value As String)
                m_strPosition = value
            End Set
        End Property

        Public Property PhoneEmployee() As String
            Get
                Return m_strPhoneEmployee
            End Get
            Set(ByVal value As String)
                m_strPhoneEmployee = value
            End Set
        End Property

        Public Property Fax() As String
            Get
                Return m_strFax
            End Get
            Set(ByVal value As String)
                m_strFax = value
            End Set
        End Property
        '---------- Employee Properties - END -----------------

        Public Property FirstLevelAdmin() As Boolean
            Get
                Return m_blnFirstLevelAdmin
            End Get
            Set(ByVal value As Boolean)
                m_blnFirstLevelAdmin = value
            End Set
        End Property

        Public Property QuestionID() As Integer
            Get
                Return m_intQuestionID
            End Get
            Set(ByVal Value As Integer)
                m_intQuestionID = Value
            End Set
        End Property

        Public Property DoubleLoginTry() As Boolean
            Get
                Return m_blnDoubleLoginTry
            End Get
            Set(ByVal Value As Boolean)
                m_blnDoubleLoginTry = Value
            End Set
        End Property

        Public Property AnswerText() As String
            Get
                Return m_strAnswerText
            End Get
            Set(ByVal Value As String)
                m_strAnswerText = Value
            End Set
        End Property

        Public Property SessionID() As String
            Get
                Return m_strSessionID
            End Get
            Set(ByVal Value As String)
                m_strSessionID = Value
            End Set
        End Property

        Public Property ReadMessageCount() As Int32
            Get
                Return m_intReadMessageCount
            End Get
            Set(ByVal Value As Int32)
                m_intReadMessageCount = Value
            End Set
        End Property

        Public Property ReadCustomerMessageCount() As Int32
            Get
                Return m_intReadCustomerMessageCount
            End Get
            Set(ByVal Value As Int32)
                m_intReadCustomerMessageCount = Value
            End Set
        End Property

        Public ReadOnly Property Organization() As Organization
            Get
                Return m_organization
            End Get
        End Property

        Public ReadOnly Property Customer() As Customer
            Get
                Return m_customer
            End Get
        End Property

        Public ReadOnly Property CustomerName() As String 'legacy
            Get
                If m_customer Is Nothing Then Return ""
                Return m_customer.CustomerName
            End Get
        End Property

        Public ReadOnly Property KUNNR() As String 'legacy
            Get
                If m_customer Is Nothing Then Return ""
                Return m_customer.KUNNR
            End Get
        End Property

        Public ReadOnly Property UserID() As Int32
            Get
                Return m_intUserId
            End Get
        End Property

        Public ReadOnly Property UserHistoryID() As Int32
            Get
                Return m_intUserHistoryID
            End Get
        End Property

        Public ReadOnly Property UserName() As String
            Get
                Return m_strUsername
            End Get
        End Property

        Public ReadOnly Property IsCustomerAdmin() As Boolean
            Get
                Return m_blnIsCustomerAdmin
            End Get
        End Property

        Public ReadOnly Property IsTestUser() As Boolean
            Get
                Return m_blnTestUser
            End Get
        End Property

        Public ReadOnly Property LastPasswordChange() As DateTime
            Get
                Return m_dtmLastPwdChange
            End Get
        End Property

        Public ReadOnly Property PasswordNeverExpires() As Boolean
            Get
                Return m_blnPwdNeverExpires
            End Get
        End Property

        Public ReadOnly Property FailedLogins() As Integer
            Get
                Return m_intFailedLogins
            End Get
        End Property

        Public ReadOnly Property AccountIsLockedOut() As Boolean
            Get

                Return m_blnAccountIsLockedOut
            End Get
        End Property
        Public ReadOnly Property AccountIsLockedBy() As String
            Get

                Return m_strAccountIsLockedBy
            End Get
        End Property
        Public ReadOnly Property Matrixfilled() As Boolean
            Get
                Return m_blnMatrixfilled
            End Get
        End Property

        Public ReadOnly Property Reference() As String
            Get
                Return m_strReference
            End Get
        End Property

        Public ReadOnly Property LoggedOn() As Boolean
            Get
                Return m_blnLoggedOn
            End Get
        End Property

        Public ReadOnly Property PasswordExpired() As Boolean
            Get
                Return m_blnPasswordExpired
            End Get
        End Property

        Public ReadOnly Property InitialPassword() As Boolean
            Get
                Return m_blnInitialPassword
            End Get
        End Property

        Public ReadOnly Property Groups() As Groups
            Get
                Return m_groups
            End Get
        End Property

        Public ReadOnly Property Applications() As DataTable
            Get
                Return m_tblApplications
            End Get
        End Property

        Public ReadOnly Property ErrorMessage() As String
            Get
                Return m_strErrorMessage
            End Get
        End Property

        Public ReadOnly Property App() As App
            Get
                Return m_app
            End Get
        End Property

        Public ReadOnly Property GroupID() As Int32
            Get
                Return m_intGroupID
            End Get
        End Property

        Public Property Email() As String
            Get
                Return m_mail
            End Get
            Set(ByVal Value As String)
                m_mail = Value
            End Set
        End Property

        Public ReadOnly Property HighestAdminLevel() As AdminLevel
            Get
                If m_customer.IsMaster And m_blnIsCustomerAdmin Then Return AdminLevel.Master
                If m_blnFirstLevelAdmin Then Return AdminLevel.FirstLevel
                If m_blnIsCustomerAdmin Then Return AdminLevel.Customer
                If m_organization.OrganizationAdmin Then Return AdminLevel.Organization
                Return AdminLevel.None
            End Get
        End Property

        Public ReadOnly Property LastLogin() As DateTime
            Get
                Return m_dtmLastLogin
            End Get
        End Property

        Public ReadOnly Property Approved() As Boolean
            Get
                Return m_approved
            End Get
        End Property

        Public ReadOnly Property FirstName() As String
            Get
                Return m_firstname
            End Get
        End Property

        Public ReadOnly Property LastName() As String
            Get
                Return m_lastname
            End Get
        End Property

        Public ReadOnly Property Title() As String
            Get
                Return m_title
            End Get
        End Property

        Public ReadOnly Property Store() As String
            Get
                Return m_store
            End Get
        End Property

        Public ReadOnly Property ValidFrom() As String
            Get
                Return m_ValidFrom
            End Get
        End Property

        Public Property Telephone() As String
            Get
                Return m_telephone
            End Get
            Set(ByVal Value As String)
                m_telephone = Value
            End Set
        End Property

        Public ReadOnly Property CreatedBy() As String
            Get
                Return m_strCreatedBy
            End Get
        End Property

        Public Property UrlRemoteLoginKey() As String
            Get
                Return m_strUrlRemoteLoginKey
            End Get
            Set(ByVal value As String)
                m_strUrlRemoteLoginKey = value
            End Set
        End Property

        Public ReadOnly Property IsLeiterZLD() As Boolean
            Get
                If m_groups.Contains("LeiterZLD") Then
                    Return True
                End If
                Return False
            End Get
        End Property

        Public ReadOnly Property GebietLZLD() As String
            Get
                If Not String.IsNullOrEmpty(m_store) AndAlso m_store.StartsWith("LZLD") AndAlso m_store.Length > 4 Then
                    Return m_store.Substring(4).Trim.PadLeft(3, "0"c)
                Else
                    Return ""
                End If
            End Get
        End Property

        Public ReadOnly Property Buchungskreis() As String
            Get
                If Not String.IsNullOrEmpty(m_strReference) AndAlso m_strReference.Length > 4 Then
                    Return m_strReference.Substring(0, 4)
                Else
                    Return ""
                End If
            End Get
        End Property

        Public ReadOnly Property Kostenstelle() As String
            Get
                If Not String.IsNullOrEmpty(m_strReference) AndAlso m_strReference.Length > 4 Then
                    Return m_strReference.Substring(4).Trim().PadLeft(3, "0"c)
                Else
                    Return ""
                End If
            End Get
        End Property
#End Region

#Region " Functions "
        Public Function GetTranslations(ByVal sBrowserLanguage As String, ByVal sAppURL As String) As DataTable
            Dim cn As New SqlClient.SqlConnection(m_strConnectionstring)
            Dim tblReturn As New DataTable()
            'hier wird die dt für die Übersetzung für jede ASPX Seite erstellt und in einer Session die den Namen der AppUrl Trägt abgelegt, JJ 2007.11.12
            Try
                cn.Open()

                Dim cmdTranslations As SqlClient.SqlCommand = New SqlClient.SqlCommand( _
                        "SELECT     TOP 100 PERCENT dbo.ApplicationField.FieldType, dbo.ApplicationField.FieldName, dbo.ApplicationField.FieldType + '_' + dbo.ApplicationField.FieldName AS [ControlID], dbo.ApplicationField.Visibility, RTRIM(IsNull(dbo.ApplicationField.Content,'')) AS Content, " & _
                        "                      dbo.ApplicationField.CustomerID, dbo.ApplicationField.LanguageID, dbo.ApplicationField.ToolTip " & _
                        "FROM         dbo.ApplicationField INNER JOIN " & _
                        "                      dbo.[Language] ON dbo.ApplicationField.LanguageID = dbo.[Language].LanguageID " & _
                        "WHERE     (dbo.ApplicationField.AppURL = @AppURL) AND (dbo.ApplicationField.CustomerID IN (1, @CustomerID)) AND " & _
                        "                      (dbo.[Language].BrowserLanguage IN ('de', @BrowserLanguage)) " & _
                        "ORDER BY dbo.ApplicationField.FieldType, dbo.ApplicationField.FieldName, dbo.ApplicationField.CustomerID, dbo.ApplicationField.LanguageID ", cn)
                cmdTranslations.Parameters.AddWithValue("@AppURL", sAppURL)
                cmdTranslations.Parameters.AddWithValue("@BrowserLanguage", sBrowserLanguage)
                cmdTranslations.Parameters.AddWithValue("@CustomerID", m_customer.CustomerId)
                Dim adTranslations As New SqlClient.SqlDataAdapter(cmdTranslations)
                adTranslations.Fill(tblReturn)

                If Not tblReturn Is Nothing AndAlso tblReturn.Rows.Count > 0 Then
                    Dim i As Integer
                    Dim strType As String = "X"
                    Dim strName As String = "X"

                    For i = tblReturn.Rows.Count - 1 To 0 Step -1
                        If strType = CStr(tblReturn.Rows(i)("FieldType")) And strName = CStr(tblReturn.Rows(i)("FieldName")) Then
                            tblReturn.Rows.RemoveAt(i)
                        Else
                            strType = CStr(tblReturn.Rows(i)("FieldType"))
                            strName = CStr(tblReturn.Rows(i)("FieldName"))
                        End If
                    Next
                End If
            Catch ex As Exception
                Throw ex
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try

            Return tblReturn
        End Function
        Public Function GetTranslations(ByVal sBrowserLanguage As String, ByVal sAppURL As String, ByVal ZLD As String) As DataTable
            Dim cn As New SqlClient.SqlConnection(m_strConnectionstring)
            Dim tblReturn As New DataTable()
            'hier wird die dt für die Übersetzung für jede ASPX Seite erstellt und in einer Session die den Namen der AppUrl Trägt abgelegt, JJ 2007.11.12
            Try
                cn.Open()

                Dim cmdTranslations As SqlClient.SqlCommand = New SqlClient.SqlCommand( _
                        "SELECT     TOP 100 PERCENT dbo.ApplicationField.FieldType, dbo.ApplicationField.FieldName, dbo.ApplicationField.FieldType + '_' + dbo.ApplicationField.FieldName AS [ControlID], dbo.ApplicationField.Visibility, RTRIM(IsNull(dbo.ApplicationField.Content,'')) AS Content, " & _
                        "                      dbo.ApplicationField.CustomerID, dbo.ApplicationField.LanguageID, dbo.ApplicationField.ToolTip " & _
                        "FROM         dbo.ApplicationField INNER JOIN " & _
                        "                      dbo.[Language] ON dbo.ApplicationField.LanguageID = dbo.[Language].LanguageID " & _
                        "WHERE     (dbo.ApplicationField.AppURL = @AppURL) AND (dbo.ApplicationField.CustomerID IN (1, @CustomerID)) AND " & _
                        "                      (dbo.[Language].BrowserLanguage IN ('de', @BrowserLanguage)) " & _
                        "AND                        (dbo.ApplicationField.GroupID IN (0, @GroupID)) " & _
                        "ORDER BY dbo.ApplicationField.FieldType, dbo.ApplicationField.FieldName, dbo.ApplicationField.CustomerID, dbo.ApplicationField.LanguageID, dbo.ApplicationField.GroupID ", cn)
                cmdTranslations.Parameters.AddWithValue("@AppURL", sAppURL)
                cmdTranslations.Parameters.AddWithValue("@BrowserLanguage", sBrowserLanguage)
                cmdTranslations.Parameters.AddWithValue("@CustomerID", m_customer.CustomerId)
                cmdTranslations.Parameters.AddWithValue("@GroupID", GroupID)
                Dim adTranslations As New SqlClient.SqlDataAdapter(cmdTranslations)
                adTranslations.Fill(tblReturn)

                If Not tblReturn Is Nothing AndAlso tblReturn.Rows.Count > 0 Then
                    Dim i As Integer
                    Dim strType As String = "X"
                    Dim strName As String = "X"

                    For i = tblReturn.Rows.Count - 1 To 0 Step -1
                        If strType = CStr(tblReturn.Rows(i)("FieldType")) And strName = CStr(tblReturn.Rows(i)("FieldName")) Then
                            tblReturn.Rows.RemoveAt(i)
                        Else
                            strType = CStr(tblReturn.Rows(i)("FieldType"))
                            strName = CStr(tblReturn.Rows(i)("FieldName"))
                        End If
                    Next
                End If
            Catch ex As Exception
                Throw ex
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try

            Return tblReturn
        End Function
        Private Function GetFailedLogins(ByVal strUserName As String, ByVal cn As SqlClient.SqlConnection) As Integer
            Try
                Dim cmdGetLogins As New SqlClient.SqlCommand("SELECT FailedLogins " & _
                                                             "FROM WebUser " & _
                                                             "WHERE Username=@Username", cn)
                cmdGetLogins.Parameters.AddWithValue("@Username", strUserName)
                Return CInt(cmdGetLogins.ExecuteScalar)
            Catch ex As Exception
                Throw New Exception("Konnte die Anzahl der fehlgeschlagenen Anmeldungen nicht aus der Datenbank lesen!", ex)
            End Try
        End Function

        Public Sub SetEmployeePicture(ByVal blnPicture As Boolean, ByVal strChangeUser As String)
            Dim cn As New SqlClient.SqlConnection(m_strConnectionstring)
            cn.Open()

            Dim cmdSetLogins As New SqlClient.SqlCommand("UPDATE WebUserInfo " & _
                                                         "SET picture=@picture, " & _
                                                         "LastChangedBy=@LastChangedBy " & _
                                                         "WHERE id_user=@id_user", cn)
            cmdSetLogins.Parameters.AddWithValue("@id_user", m_intUserId)
            cmdSetLogins.Parameters.AddWithValue("@picture", blnPicture)
            cmdSetLogins.Parameters.AddWithValue("@LastChangedBy", strChangeUser)
            cmdSetLogins.ExecuteNonQuery()

            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Sub

        Private Sub SetFailedLogins(ByVal strUserName As String, ByVal Number As Integer, ByVal cn As SqlClient.SqlConnection, ByVal strChangeUser As String)
            Try
                Dim blnCloseOnEnd As Boolean = False
                If cn.State = ConnectionState.Closed Then
                    cn.Open()
                    blnCloseOnEnd = True
                End If
                Dim cmdSetLogins As New SqlClient.SqlCommand("UPDATE WebUser " & _
                                                             "SET FailedLogins=@FailedLogins, " & _
                                                                 "LastChangedBy=@ChangeUser, " & _
                                                                 "AccountIsLockedOut=0" & _
                                                             "WHERE Username=@Username ", cn)
                cmdSetLogins.Parameters.AddWithValue("@Username", strUserName)
                cmdSetLogins.Parameters.AddWithValue("@FailedLogins", Number)
                cmdSetLogins.Parameters.AddWithValue("@ChangeUser", strChangeUser)

                cmdSetLogins.ExecuteNonQuery()


                If blnCloseOnEnd Then
                    cn.Close()
                End If
            Catch ex As Exception
                Throw New Exception(String.Format("Konnte die Anzahl der Fehlgeschlagenen Anmeldungen nicht auf {0} setzen!", Number), ex)
            End Try
        End Sub

        Public Sub SetLoggedOn(ByVal strUserName As String, ByVal LogonValue As Boolean, ByVal strSessionID As String)
            Dim cn As SqlClient.SqlConnection = New SqlClient.SqlConnection(m_strConnectionstring)
            Try
                cn.Open()
                Dim cmdSetLogins As New SqlClient.SqlCommand("UPDATE WebUser " & _
                                                             "SET LoggedOn=@LoggedOn " & _
                                                             ",SessionID=@SessionID " & _
                                                             "WHERE Username=@Username", cn)
                cmdSetLogins.Parameters.AddWithValue("@Username", strUserName)
                cmdSetLogins.Parameters.AddWithValue("@LoggedOn", LogonValue)
                cmdSetLogins.Parameters.AddWithValue("@SessionID", strSessionID)
                cmdSetLogins.ExecuteNonQuery()
                m_blnLoggedOn = LogonValue
                m_strSessionID = strSessionID
            Catch ex As Exception
                Throw New Exception("Login-Status des Benutzers konnte nicht geändert werden.", ex)
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End Sub

        Public Sub GetCurrentSessionID(ByVal strUserName As String)
            Dim cn As SqlClient.SqlConnection = New SqlClient.SqlConnection(m_strConnectionstring)
            Try
                cn.Open()
                Dim cmdGetCurrentSessionID As New SqlClient.SqlCommand("SELECT SessionID FROM WebUser " & _
                                                             "WHERE Username=@Username", cn)
                cmdGetCurrentSessionID.Parameters.AddWithValue("@Username", strUserName)
                m_strSessionID = cmdGetCurrentSessionID.ExecuteScalar.ToString
                m_blnLoggedOn = True
            Catch ex As Exception
                Throw New Exception("Session-ID des Benutzers konnte nicht ermittelt werden.", ex)
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End Sub

        Private Sub LockOutAccount(ByVal strUserName As String, ByVal cn As SqlClient.SqlConnection, ByVal strChangeUser As String)
            Try
                If cn.State = ConnectionState.Closed Then
                    cn.Open()
                End If
                Dim cmdSetLogins As New SqlClient.SqlCommand("UPDATE WebUser " & _
                                                             "SET FailedLogins=@FailedLogins, " & _
                                                                 "AccountIsLockedOut=@AccountIsLockedOut, " & _
                                                                 "LastChangedBy=@ChangeUser " & _
                                                             "WHERE Username=@Username", cn)
                cmdSetLogins.Parameters.AddWithValue("@Username", strUserName)
                cmdSetLogins.Parameters.AddWithValue("@FailedLogins", 3)
                cmdSetLogins.Parameters.AddWithValue("@AccountIsLockedOut", True)
                cmdSetLogins.Parameters.AddWithValue("@ChangeUser", strChangeUser)
                cmdSetLogins.ExecuteNonQuery()
                cmdSetLogins = New SqlClient.SqlCommand("UPDATE WebUserHistory " & _
                                                             "SET FailedLogins=@FailedLogins, " & _
                                                                 "AccountIsLockedOut=@AccountIsLockedOut," & _
                                                                 "LastChanged=Getdate()," & _
                                                                 "LastChange='Benutzer gesperrt'," & _
                                                                 "LastChangedBy=@ChangeUser " & _
                                                             "WHERE UserHistoryID=@UserHistoryID", cn)
                cmdSetLogins.Parameters.AddWithValue("@UserHistoryID", m_intUserHistoryID)
                cmdSetLogins.Parameters.AddWithValue("@ChangeUser", strChangeUser)
                cmdSetLogins.Parameters.AddWithValue("@FailedLogins", 3)
                cmdSetLogins.Parameters.AddWithValue("@AccountIsLockedOut", True)
                cmdSetLogins.ExecuteNonQuery()
            Catch ex As Exception
                Throw New Exception("Konto konnte nicht gesperrt werden!", ex)
            End Try
        End Sub

        Public Sub SaveQuestion(ByVal inQuestionID As Integer, ByVal strQuestionText As String)
            Dim cn As New SqlClient.SqlConnection(m_strConnectionstring)

            Try
                Dim strTemp As String = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(strQuestionText, "sha1")
                cn.Open()

                Dim cmdWork As New SqlClient.SqlCommand("DELETE FROM PasswordQuestionAnswers " & _
                                                          "WHERE UserID=@UserID", cn)
                With cmdWork.Parameters
                    .AddWithValue("@UserID", m_intUserId)
                End With
                cmdWork.ExecuteNonQuery()

                cmdWork = New SqlClient.SqlCommand("INSERT INTO PasswordQuestionAnswers " & _
                                                         "VALUES (@UserID,@QuestionID,@AnswerText)", cn)
                With cmdWork.Parameters
                    .AddWithValue("@UserID", m_intUserId)
                    .AddWithValue("@QuestionID", inQuestionID)
                    .AddWithValue("@AnswerText", strTemp)
                End With
                cmdWork.ExecuteNonQuery()

                m_intQuestionID = inQuestionID
                m_strAnswerText = strTemp
            Catch ex As Exception
                Throw New Exception("Fehler beim Ändern des Passworts: " & ex.Message, ex)
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
                cn.Dispose()
            End Try

        End Sub

        Public Function GetQuestionText() As String
            Dim strReturn As String = ""
            Dim cn As New SqlClient.SqlConnection(m_strConnectionstring)

            Try

                cn.Open()

                Dim cmdGetQuestionText As SqlClient.SqlCommand
                cmdGetQuestionText = New SqlClient.SqlCommand("SELECT PasswordQuestionList.QuestionText " & _
                                                                "FROM WebUser INNER JOIN " & _
                                                                "PasswordQuestionAnswers ON WebUser.UserID = PasswordQuestionAnswers.UserID INNER JOIN " & _
                                                                "PasswordQuestionList ON PasswordQuestionAnswers.QuestionID = PasswordQuestionList.QuestionID " & _
                                                                "WHERE (WebUser.Username = @Username)", cn)
                cmdGetQuestionText.Parameters.AddWithValue("@Username", m_strUsername)

                strReturn = cmdGetQuestionText.ExecuteScalar
            Catch ex As Exception
                m_strErrorMessage = ex.Message
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
                cn.Dispose()
            End Try
            Return strReturn
        End Function

        Public Function GetQuestions() As DataTable
            Dim tblReturn As New DataTable()
            Dim cn As New SqlClient.SqlConnection(m_strConnectionstring)

            Try

                cn = New SqlClient.SqlConnection(m_strConnectionstring)
                cn.Open()

                Dim cmdGetQuestions As SqlClient.SqlCommand
                cmdGetQuestions = New SqlClient.SqlCommand("SELECT QuestionID, QuestionText FROM PasswordQuestionList", cn)
                Dim da As New SqlClient.SqlDataAdapter(cmdGetQuestions)
                da.Fill(tblReturn)

                Dim dr As DataRow
                dr = tblReturn.NewRow()
                dr("QuestionID") = -1
                dr("QuestionText") = " - keine Auswahl - "
                tblReturn.Rows.Add(dr)

            Catch ex As Exception
                m_strErrorMessage = ex.Message
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
                cn.Dispose()
            End Try
            Return tblReturn
        End Function

        Public Function Login(ByVal strUsername As String, ByVal strSessionID As String) As Boolean
            Dim blnReturn As Boolean
            Dim cn As New SqlClient.SqlConnection(m_strConnectionstring)

            Try

                'Eingabe-Parameter ueberpruefen
                If Not (Len(strUsername) > 0) Then
                    Throw New System.Exception("Keine gültigen Anmeldedaten eingegeben!")
                End If

                'Prerequisits
                m_strErrorMessage = ""
                cn.Open()

                Dim cmdUser As SqlClient.SqlCommand
                cmdUser = New SqlClient.SqlCommand("SELECT " & _
                                                    "UserID, " & _
                                                    "Username, " & _
                                                    "Password, " & _
                                                    "CustomerID, " & _
                                                    "CustomerAdmin, " & _
                                                    "TestUser, " & _
                                                    "LastPwdChange, " & _
                                                    "PwdNeverExpires, " & _
                                                    "FailedLogins, " & _
                                                    "AccountIsLockedOut, " & _
                                                    "FirstLevelAdmin, " & _
                                                    "Reference, " & _
                                                    "LoggedOn, " & _
                                                    "LastLogin, " & _
                                                    "ReadMessageCount, " & _
                                                    "ReadCustomerMessageCount, " & _
                                                    "UserHistoryID, " & _
                                                    "CreatedBy, " & _
                                                    "CreateDate, " & _
                                                    "MasterCreate, " & _
                                                    "Approved, " & _
                                                    "FirstName, " & _
                                                    "LastName, " & _
                                                    "Title, " & _
                                                    "Store, " & _
                                                    "Matrix, " & _
                                                    "SessionID, " & _
                                                    "QuestionID, " & _
                                                    "AnswerText, " & _
                                                    "ValidFrom, " & _
                                                    "UrlRemoteLoginKey " & _
                                                    "FROM vwWebUser " & _
                                                    "WHERE Username = @Username ", cn)
                With cmdUser.Parameters
                    .AddWithValue("@Username", strUsername)
                End With
                blnReturn = GetUserData(cmdUser, cn)

                If Not blnReturn Then
                    Throw New System.Exception(m_strErrorMessage)
                End If

                'Anmeldestatus pruefen
                If m_intUserId = -1 Then
                    If GetUserDataFromName(strUsername, cn) Then
                        If m_intFailedLogins < m_customer.CustomerLoginRules.LockedAfterNLogins Then
                            SetFailedLogins(strUsername, m_intFailedLogins + 1, cn, strUsername)
                            m_intFailedLogins += 1
                            If m_mail.Length > 0 And m_customer.ForcePasswordQuestion And m_intQuestionID > -1 Then
                                Throw New System.Exception("4174")
                            Else
                                Throw New System.Exception("")
                            End If
                        Else
                            If m_blnAccountIsLockedOut Then
                                Dim sLockedBy As String = CStr(GetHistoryInfos(Me))
                                If sLockedBy.ToLower <> UserName.ToLower AndAlso sLockedBy <> "" Then
                                    m_strAccountIsLockedBy = "Admin"
                                ElseIf sLockedBy = UserName Then
                                    m_strAccountIsLockedBy = "User"
                                End If
                            Else
                                LockOutAccount(strUsername, cn, strUsername)
                                m_blnAccountIsLockedOut = True
                                m_strAccountIsLockedBy = "Now"
                            End If
                            Throw New System.Exception("Das Passwort wurde mehrfach falsch eingegeben. Das Benutzerkonto wurde gesperrt. Bitte setzen Sie sich mit Ihrem Administrator in Verbindung!")
                        End If
                    Else
                        Throw New System.Exception("")
                    End If
                Else
                    GetEmail(m_intUserId, blnReturn)
                End If

                'Auf Freigabe des Accounts prüfen
                If Not m_approved Then
                    Throw New System.Exception("Das Benutzerkonto ist noch nicht freigegeben. Bitte setzen Sie Sich mit Ihrem Administrator in Verbindung!")
                End If

                'Auf Sperrung des Accounts pruefen
                If m_blnAccountIsLockedOut Then
                    Dim sLockedBy As String = CStr(GetHistoryInfos(Me))
                    If sLockedBy.ToLower <> UserName.ToLower AndAlso sLockedBy <> "" Then
                        m_strAccountIsLockedBy = "Admin"
                    ElseIf sLockedBy = UserName Then
                        m_strAccountIsLockedBy = "User"
                    End If
                    Throw New System.Exception("Das Benutzerkonto ist gesperrt. Bitte setzen Sie sich mit Ihrem Administrator in Verbindung!")
                End If

                'Gibt es eine Datumsbeschränkung
                If Not m_ValidFrom Is DBNull.Value Then
                    If m_ValidFrom.Length > 0 AndAlso IsDate(m_ValidFrom) = True Then
                        If CDate(m_ValidFrom) >= Date.Now Then
                            Throw New System.Exception("Das Benutzerkonto steht Ihnen ab " & CDate(m_ValidFrom).ToShortDateString & " zur Verfügung.")
                        End If
                    End If
                End If


                'Logonstatus des Accounts pruefen und setzen
                If Not m_customer.AllowMultipleLogin Then
                    If m_blnLoggedOn Then
                        m_blnDoubleLoginTry = True
                        'Throw New System.Exception("Der Benutzer ist bereits angemeldet. Mehrfache Anmeldungen sind nicht gestattet.")
                    Else
                        SetLoggedOn(strUsername, True, strSessionID)
                    End If
                Else
                    SetLoggedOn(strUsername, True, strSessionID)
                End If
                '

                m_blnInitialPassword = GetPasswordHistory(cn)

                'Nachbereitung
                SetFailedLogins(m_strUsername, 0, cn, m_strUsername)
                m_intFailedLogins = 0
            Catch ex As Exception
                blnReturn = False
                m_strErrorMessage = ex.Message
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
                cn.Dispose()
            End Try
            Return blnReturn
        End Function

        Public Function Login(ByVal strUsername As String, ByVal strPassword As String, _
                              ByVal strSessionID As String, Optional ByVal blnPasswdlink As Boolean = False) As Boolean
            Dim blnReturn As Boolean
            Dim cn As New SqlClient.SqlConnection(m_strConnectionstring)

            Try

                'Eingabe-Parameter ueberpruefen
                If Not (Len(strUsername) > 0 And Len(strPassword) > 0) Then
                    Throw New System.Exception("Keine gültigen Anmeldedaten eingegeben!")
                End If
                'Passwort mit Leerzeichen
                If strPassword.Length <> strPassword.Trim.Length Then
                    Throw New System.Exception("Überprüfen Sie Ihr Passwort! Leerzeichen nicht erlaubt!")
                End If

                'Prerequisits
                m_strErrorMessage = ""
                cn.Open()

                Dim cmdUser As SqlClient.SqlCommand
                cmdUser = New SqlClient.SqlCommand("SELECT " & _
                                                    "UserID, " & _
                                                    "Username, " & _
                                                    "Password, " & _
                                                    "CustomerID, " & _
                                                    "CustomerAdmin, " & _
                                                    "TestUser, " & _
                                                    "LastPwdChange, " & _
                                                    "PwdNeverExpires, " & _
                                                    "FailedLogins, " & _
                                                    "AccountIsLockedOut, " & _
                                                    "FirstLevelAdmin, " & _
                                                    "Reference, " & _
                                                    "LoggedOn, " & _
                                                    "LastLogin, " & _
                                                    "ReadMessageCount, " & _
                                                    "ReadCustomerMessageCount, " & _
                                                    "UserHistoryID, " & _
                                                    "CreatedBy, " & _
                                                    "CreateDate, " & _
                                                    "MasterCreate, " & _
                                                    "Approved, " & _
                                                    "FirstName, " & _
                                                    "LastName, " & _
                                                    "Title, " & _
                                                    "Store, " & _
                                                    "Matrix, " & _
                                                    "SessionID, " & _
                                                    "QuestionID, " & _
                                                    "AnswerText, " & _
                                                    "ValidFrom, " & _
                                                    "UrlRemoteLoginKey " & _
                                                    "FROM vwWebUser " & _
                                                    "WHERE Username = @Username " & _
                                                    "AND Password = @Password", cn)
                With cmdUser.Parameters
                    .AddWithValue("@Username", strUsername)
                    .AddWithValue("@Password", System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(strPassword, "sha1"))
                End With
                blnReturn = GetUserData(cmdUser, cn, blnPasswdlink)

                'Sonderregelung fuer alte Klartextpassworte in der Datenbank
                If m_intUserId = -1 Then
                    With cmdUser.Parameters
                        .Clear()
                        .AddWithValue("@Username", strUsername)
                        .AddWithValue("@Password", strPassword)
                    End With
                    blnReturn = GetUserData(cmdUser, cn, blnPasswdlink)
                End If

                '-- ORU 20080205: von UHa einfügt für Testzwecke??? und vergessen zu entfernen
                'If Not blnReturn Then 
                '    Throw New System.Exception(m_strErrorMessage)
                'End If

                'Anmeldestatus pruefen
                If m_intUserId = -1 Then
                    If GetUserDataFromName(strUsername, cn, blnPasswdlink) Then
                        If m_intFailedLogins < m_customer.CustomerLoginRules.LockedAfterNLogins Then
                            SetFailedLogins(strUsername, m_intFailedLogins + 1, cn, strUsername)
                            m_intFailedLogins += 1
                            If m_mail.Length > 0 And m_customer.ForcePasswordQuestion And m_intQuestionID > -1 Then
                                Throw New System.Exception("4174")
                            Else
                                Throw New System.Exception("9999")
                            End If
                        Else

                            If m_blnAccountIsLockedOut Then
                                Dim sLockedBy As String = CStr(GetHistoryInfos(Me))
                                If sLockedBy.ToLower <> UserName.ToLower AndAlso sLockedBy <> "" Then
                                    m_strAccountIsLockedBy = "Admin"
                                ElseIf sLockedBy = UserName Then
                                    m_strAccountIsLockedBy = "User"
                                End If
                            Else
                                LockOutAccount(strUsername, cn, strUsername)
                                m_blnAccountIsLockedOut = True
                                m_strAccountIsLockedBy = "Now"
                            End If
                            Throw New System.Exception("Das Passwort wurde mehrfach falsch eingegeben. Das Benutzerkonto wurde gesperrt. Bitte setzen Sie sich mit Ihrem Administrator in Verbindung!")

                        End If
                    Else
                        Throw New System.Exception("")
                    End If
                Else
                    GetEmail(m_intUserId, blnReturn)
                End If

                'Auf Freigabe des Accounts prüfen
                If Not m_approved Then
                    Throw New System.Exception("Das Benutzerkonto ist noch nicht freigegeben. Bitte setzen Sie Sich mit Ihrem Administrator in Verbindung!")
                End If

                'Auf Sperrung des Accounts pruefen
                If m_blnAccountIsLockedOut Then
                    Dim sLockedBy As String = CStr(GetHistoryInfos(Me))
                    If sLockedBy.ToLower <> UserName.ToLower AndAlso sLockedBy <> "" Then
                        m_strAccountIsLockedBy = "Admin"
                    ElseIf sLockedBy = UserName Then
                        m_strAccountIsLockedBy = "User"
                    End If
                    Throw New System.Exception("Das Benutzerkonto ist gesperrt. Bitte setzen Sie sich mit Ihrem Administrator in Verbindung!")
                End If

                'Gibt es eine Datumsbeschränkung
                If Not m_ValidFrom Is DBNull.Value Then
                    If m_ValidFrom.Length > 0 AndAlso IsDate(m_ValidFrom) = True Then
                        If CDate(m_ValidFrom) >= Date.Now Then
                            Throw New System.Exception("Das Benutzerkonto steht Ihnen erst ab " & CDate(m_ValidFrom).ToShortDateString & " zur Verfügung.")
                        End If
                    End If
                End If


                'Logonstatus des Accounts pruefen und setzen
                If Not m_customer.AllowMultipleLogin Then
                    If m_blnLoggedOn Then
                        m_blnDoubleLoginTry = True
                        'Throw New System.Exception("Der Benutzer ist bereits angemeldet. Mehrfache Anmeldungen sind nicht gestattet.")
                    Else
                        SetLoggedOn(strUsername, True, strSessionID)
                    End If
                Else
                    SetLoggedOn(strUsername, True, strSessionID)
                End If
                '

                m_blnInitialPassword = GetPasswordHistory(cn)

                'Nachbereitung
                SetFailedLogins(m_strUsername, 0, cn, m_strUsername)
                m_intFailedLogins = 0
            Catch ex As Exception
                blnReturn = False
                m_strErrorMessage = ex.Message
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
                cn.Dispose()
            End Try
            Return blnReturn
        End Function
        '-------
        'Sperrung durch
        '-------
        Private Function GetHistoryInfos(ByVal objUser As Kernel.Security.User) As String

            Dim cn As New SqlClient.SqlConnection(m_strConnectionstring)
            Try
                cn.Open()
                If cn.State = ConnectionState.Closed Then
                    cn.Open()
                End If
                Dim cmd As SqlClient.SqlCommand = New SqlClient.SqlCommand("SELECT LastChangedBy,Max(ID) as ID  " & _
                                                          "FROM AdminHistory_User  " & _
                                                          "WHERE Username = @Username And " & _
                                                          "Action='Benutzer gesperrt' Group By LastChangedBy ORDER BY ID DESC", cn)

                cmd.Parameters.AddWithValue("@Username", objUser.UserName)
                Dim sUser As String = CStr(cmd.ExecuteScalar)

                If Not sUser Is Nothing Then
                    Return sUser
                Else
                    Return ""
                End If
            Catch ex As Exception
                Throw
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End Function
        Public Function RequestNewPassword(ByVal strAnswer As String) As Integer
            Dim intReturn As Integer = -9999
            Dim cn As New SqlClient.SqlConnection(m_strConnectionstring)
            m_strErrorMessage = ""

            Try

                cn.Open()

                Dim cmdCheckAnswer As SqlClient.SqlCommand
                cmdCheckAnswer = New SqlClient.SqlCommand("SELECT COUNT(UserID) AS Hits " & _
                                                            "FROM PasswordQuestionAnswers WHERE " & _
                                                            "(QuestionID = @QuestionID) AND " & _
                                                            "(UserID = @UserID) AND " & _
                                                            "(AnswerText = @AnswerText)" _
                                                            , cn)

                cmdCheckAnswer.Parameters.AddWithValue("@QuestionID", m_intQuestionID)
                cmdCheckAnswer.Parameters.AddWithValue("@UserID", m_intUserId)
                cmdCheckAnswer.Parameters.AddWithValue("@AnswerText", System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(strAnswer, "sha1"))

                Dim intTemp As Integer = CInt(cmdCheckAnswer.ExecuteScalar)

                If intTemp = 1 Then
                    'Antwort war richtig

                    'Passwort erzeugen und mailen!
                    Dim strTemp As String = m_customer.CustomerPasswordRules.CreateNewPasswort(m_strErrorMessage)
                    If m_strErrorMessage.Length = 0 Then
                        If Not ChangePassword("", strTemp, strTemp, m_strUsername, True) Then
                            Throw New System.Exception(m_strErrorMessage)
                        Else
                            If Not SendPasswordMail(strTemp, m_strErrorMessage, True) Then
                                Throw New System.Exception(m_strErrorMessage)
                            End If
                        End If
                    Else
                        Throw New System.Exception(m_strErrorMessage)
                    End If

                    'Fehlerhafte Logins zurücksetzen
                    SetFailedLogins(m_strUsername, 0, cn, m_strUsername)

                    intReturn = 0
                Else
                    'Die Anwort war falsch. => Reagieren!
                    SetFailedLogins(m_strUsername, m_intFailedLogins + 1, cn, m_strUsername)
                    m_intFailedLogins += 1
                    intTemp = m_customer.CustomerLoginRules.LockedAfterNLogins - m_intFailedLogins
                    If intTemp <= 0 Then
                        LockOutAccount(m_strUsername, cn, m_strUsername)
                        Throw New System.Exception("Die Daten wurden mehrfach falsch eingegeben. Das Benutzerkonto wurde gesperrt. Bitte setzen Sie sich mit Ihrem Administrator in Verbindung!")
                    Else
                        intReturn = intTemp
                    End If
                End If
            Catch ex As Exception
                m_strErrorMessage = ex.Message
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
                cn.Dispose()
            End Try

            Return intReturn
        End Function

        Private Function GetUserDataFromName(ByVal strUserName As String, ByVal cn As SqlClient.SqlConnection, Optional ByVal blnPasswdlink As Boolean = False, Optional ByVal blnFromLogin As Boolean = True) As Boolean
            Dim blnReturn As Boolean

            Try
                'Eingabe-Parameter ueberpruefen
                If strUserName = String.Empty Then
                    Throw New System.Exception("Kein gültiger Benutzername!")
                End If

                'Prerequisits
                m_strErrorMessage = ""

                Dim cmdUser As SqlClient.SqlCommand
                cmdUser = New SqlClient.SqlCommand("SELECT " & _
                                                    "UserID, " & _
                                                    "Username, " & _
                                                    "Password, " & _
                                                    "CustomerID, " & _
                                                    "CustomerAdmin, " & _
                                                    "TestUser, " & _
                                                    "LastPwdChange, " & _
                                                    "PwdNeverExpires, " & _
                                                    "FailedLogins, " & _
                                                    "AccountIsLockedOut, " & _
                                                    "FirstLevelAdmin, " & _
                                                    "Reference, " & _
                                                    "LoggedOn, " & _
                                                    "LastLogin, " & _
                                                    "ReadMessageCount, " & _
                                                    "ReadCustomerMessageCount, " & _
                                                    "UserHistoryID, " & _
                                                    "CreatedBy, " & _
                                                    "CreateDate, " & _
                                                    "MasterCreate, " & _
                                                    "Approved, " & _
                                                    "FirstName, " & _
                                                    "LastName, " & _
                                                    "Title, " & _
                                                    "Store, " & _
                                                    "Matrix, " & _
                                                    "SessionID, " & _
                                                    "QuestionID, " & _
                                                    "AnswerText, " & _
                                                    "ValidFrom, " & _
                                                    "UrlRemoteLoginKey " & _
                                                    "FROM vwWebUser " & _
                                                    "WHERE Username = @Username", cn)
                cmdUser.Parameters.AddWithValue("@Username", strUserName)
                blnReturn = GetUserData(cmdUser, cn, blnPasswdlink, False)

            Catch ex As Exception
                blnReturn = False
                m_strErrorMessage = ex.Message
            End Try

            GetEmail(m_intUserId, blnReturn)
            Return blnReturn
        End Function

        Private Function GetUserDataFromId(ByVal intUserId As Integer, ByVal cn As SqlClient.SqlConnection, Optional ByVal blnPasswdlink As Boolean = False) As Boolean
            Dim blnReturn As Boolean

            Try
                'Eingabe-Parameter ueberpruefen
                If intUserId = 0 Then
                    Throw New System.Exception("Keine gültige Benutzer-ID!")
                End If

                'Prerequisits
                m_strErrorMessage = ""

                Dim cmdUser As SqlClient.SqlCommand
                cmdUser = New SqlClient.SqlCommand("SELECT " & _
                                                    "UserID, " & _
                                                    "Username, " & _
                                                    "Password, " & _
                                                    "CustomerID, " & _
                                                    "CustomerAdmin, " & _
                                                    "TestUser, " & _
                                                    "LastPwdChange, " & _
                                                    "PwdNeverExpires, " & _
                                                    "FailedLogins, " & _
                                                    "AccountIsLockedOut, " & _
                                                    "FirstLevelAdmin, " & _
                                                    "Reference, " & _
                                                    "LoggedOn, " & _
                                                    "LastLogin, " & _
                                                    "ReadMessageCount, " & _
                                                    "ReadCustomerMessageCount, " & _
                                                    "UserHistoryID, " & _
                                                    "CreatedBy, " & _
                                                    "CreateDate, " & _
                                                    "MasterCreate, " & _
                                                    "Approved, " & _
                                                    "FirstName, " & _
                                                    "LastName, " & _
                                                    "Title, " & _
                                                    "Store, " & _
                                                    "Matrix, " & _
                                                    "SessionID, " & _
                                                    "QuestionID, " & _
                                                    "AnswerText, " & _
                                                    "ValidFrom, " & _
                                                    "UrlRemoteLoginKey " & _
                                                    "FROM vwWebUser " & _
                                                    "WHERE UserId = @UserId", cn)
                cmdUser.Parameters.AddWithValue("@UserId", intUserId)

                blnReturn = GetUserData(cmdUser, cn, blnPasswdlink)

            Catch ex As Exception
                blnReturn = False
                m_strErrorMessage = ex.Message
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try

            GetEmail(intUserId, blnReturn)

            Return blnReturn
        End Function

        Public Sub GetEmail(ByVal intUserId As Integer, ByRef bReturn As Boolean)
            Dim conn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
            Dim cmdUserMail As New SqlClient.SqlCommand()

            If bReturn = True Then
                cmdUserMail.CommandText = "SELECT " & _
                                                "[mail], " & _
                                                "[employee], " & _
                                                "[picture], " & _
                                                "[employeeHierarchy], " & _
                                                "[department], " & _
                                                "[position], " & _
                                                "[telephone], " & _
                                                "[fax], " & _
                                                "[Telephone2] " & _
                                                "FROM WebUserInfo " & _
                                                "WHERE id_User=@UserId"
                cmdUserMail.Connection = conn
                cmdUserMail.Parameters.AddWithValue("@UserId", intUserId)

                Try
                    conn.Open()

                    Dim daUser As New SqlClient.SqlDataAdapter(cmdUserMail)
                    Dim dtUser As New DataTable()

                    daUser.Fill(dtUser)
                    If Not dtUser.Rows Is Nothing AndAlso dtUser.Rows.Count = 1 Then
                        m_mail = ""
                        If Not TypeOf dtUser.Rows(0)("mail") Is System.DBNull Then
                            m_mail = CStr(dtUser.Rows(0)("mail"))
                        End If
                        m_blnEmployee = CBool(dtUser.Rows(0)("employee"))
                        m_blnPicture = CBool(dtUser.Rows(0)("picture"))
                        m_intHierarchyID = CInt(dtUser.Rows(0)("employeeHierarchy"))
                        m_strDepartment = ""
                        If Not TypeOf dtUser.Rows(0)("department") Is System.DBNull Then
                            m_strDepartment = CStr(dtUser.Rows(0)("department"))
                        End If
                        m_strPosition = ""
                        If Not TypeOf dtUser.Rows(0)("position") Is System.DBNull Then
                            m_strPosition = CStr(dtUser.Rows(0)("position"))
                        End If
                        m_strPhoneEmployee = ""
                        If Not TypeOf dtUser.Rows(0)("telephone") Is System.DBNull Then
                            m_strPhoneEmployee = CStr(dtUser.Rows(0)("telephone"))
                        End If
                        m_strFax = ""
                        If Not TypeOf dtUser.Rows(0)("fax") Is System.DBNull Then
                            m_strFax = CStr(dtUser.Rows(0)("fax"))
                        End If
                        m_telephone = ""
                        If Not TypeOf dtUser.Rows(0)("Telephone2") Is System.DBNull Then
                            m_telephone = CStr(dtUser.Rows(0)("Telephone2"))
                        End If
                    End If
                Catch ex As Exception
                    bReturn = False
                Finally
                    If conn.State <> ConnectionState.Closed Then
                        conn.Close()
                    End If
                    conn.Dispose()
                End Try
            End If
        End Sub

        Private Function GetUserData(ByVal cmdUser As SqlClient.SqlCommand, _
                                     ByVal cn As SqlClient.SqlConnection, Optional ByVal blnPasswdlink As Boolean = False, Optional ByVal blnFromLogin As Boolean = True) As Boolean
            Dim blnReturn As Boolean

            Try
                'User-Informationen holen
                Dim daUser As New SqlClient.SqlDataAdapter(cmdUser)
                Dim dtUser As New DataTable()
                Dim intCustID As Int32
                daUser.Fill(dtUser)
                If dtUser.Rows.Count > 0 Then
                    Dim drUser As DataRow
                    For Each drUser In dtUser.Rows
                        intCustID = CInt(drUser("CustomerID"))
                        m_intUserId = CType(drUser("UserID").ToString, System.Int32)
                        m_intUserHistoryID = -2
                        If Not TypeOf drUser("UserHistoryID") Is System.DBNull Then
                            m_intUserHistoryID = CType(drUser("UserHistoryID").ToString, System.Int32)
                        End If
                        m_customer = New Customer(intCustID, cn)
                        m_organization = New Organization(-1, cn, m_intUserId)
                        m_strUsername = drUser("Username").ToString
                        m_blnIsCustomerAdmin = CBool(drUser("CustomerAdmin"))
                        m_blnTestUser = CBool(drUser("TestUser"))
                        m_dtmLastPwdChange = CDate(drUser("LastPwdChange"))
                        m_blnPwdNeverExpires = CBool(drUser("PwdNeverExpires"))
                        m_intFailedLogins = CInt(drUser("FailedLogins"))
                        m_blnAccountIsLockedOut = CBool(drUser("AccountIsLockedOut"))
                        m_blnFirstLevelAdmin = CBool(drUser("FirstLevelAdmin"))
                        m_blnMatrixfilled = CBool(drUser("Matrix"))
                        m_strReference = ""
                        If Not TypeOf drUser("Reference") Is System.DBNull Then
                            m_strReference = CStr(drUser("Reference"))
                        End If
                        m_blnLoggedOn = CBool(drUser("LoggedOn"))
                        If Not drUser("LastLogin") Is System.DBNull.Value Then
                            m_dtmLastLogin = CDate(drUser("LastLogin"))
                        End If
                        m_intReadMessageCount = CInt(drUser("ReadMessageCount"))
                        m_intReadCustomerMessageCount = CInt(drUser("ReadCustomerMessageCount"))
                        m_approved = CBool(drUser("Approved"))
                        m_firstname = drUser("FirstName").ToString
                        m_lastname = drUser("LastName").ToString
                        m_title = drUser("Title").ToString
                        m_store = drUser("Store").ToString
                        m_ValidFrom = drUser("ValidFrom").ToString
                        m_strUrlRemoteLoginKey = drUser("UrlRemoteLoginKey").ToString
                        m_strCreatedBy = drUser("CreatedBy").ToString()
                        m_intQuestionID = -1
                        If Not drUser("QuestionID") Is System.DBNull.Value Then
                            m_intQuestionID = CInt(drUser("QuestionID"))
                        End If
                        m_strAnswerText = ""
                        If Not drUser("AnswerText") Is System.DBNull.Value Then
                            m_strAnswerText = CStr(drUser("AnswerText"))
                        End If
                    Next
                Else
                    m_intUserId = -1
                End If


                If m_intUserId = -1 Then
                    Return False
                Else
                    If Not blnPasswdlink Or blnFromLogin = True Then
                        'Gruppen holen
                        GetGroups(cn)

                        'Applications holen
                        GetApplications(cn, intCustID)

                        'App instanzieren
                        m_app = New App(Me)
                    End If
                    'Auf abgelaufenes Passwort pruefen
                    If (Not m_blnPwdNeverExpires) AndAlso (m_dtmLastPwdChange < Now.Subtract(System.TimeSpan.FromDays(m_customer.CustomerLoginRules.NewPasswordAfterNDays))) Then
                        m_blnPasswordExpired = True
                    Else
                        m_blnPasswordExpired = False
                    End If
                End If

                blnReturn = True
            Catch ex As Exception
                blnReturn = False
                m_strErrorMessage = ex.Message
            End Try

            Return blnReturn
        End Function
        Private Function GetGroups(ByVal cn As SqlClient.SqlConnection) As Boolean
            Dim blnReturn As Boolean

            Try
                m_groups = New Groups(Me, cn)

                'GroupID ermitteln
                If cn.State = ConnectionState.Closed Then
                    cn.Open()
                End If
                Dim cmdGroupID As SqlClient.SqlCommand = New SqlClient.SqlCommand("SELECT GroupID " & _
                                                          "FROM vwGroupWebUser  " & _
                                                          "WHERE UserID = @UserID", cn)
                cmdGroupID.Parameters.AddWithValue("@UserID", m_intUserId)
                m_intGroupID = CInt(cmdGroupID.ExecuteScalar)

                blnReturn = True
            Catch ex As Exception
                blnReturn = False
                m_strErrorMessage = ex.Message
            End Try

            Return blnReturn
        End Function

        Private Function GetApplications(ByVal cn As SqlClient.SqlConnection, ByVal intCustomerID As Int32) As Boolean
            Dim blnReturn As Boolean

            Try
                'Applikaions-Informationen holen - User
                m_tblApplications.Rows.Clear()
                Dim cmdApplication As SqlClient.SqlCommand = New SqlClient.SqlCommand("SELECT * " & _
                                                          "FROM vwApplicationWebUser " & _
                                                          "WHERE UserID = @UserID order by AppRank", cn)
                cmdApplication.Parameters.AddWithValue("@UserID", m_intUserId)
                Dim adApplication As New SqlClient.SqlDataAdapter(cmdApplication)

                adApplication.Fill(m_tblApplications)

                blnReturn = True
            Catch ex As Exception
                blnReturn = False
                m_strErrorMessage = ex.Message
            End Try

            Return blnReturn
        End Function

        Private Function VerifyPassword(ByVal strPassword As String, ByVal cn As SqlClient.SqlConnection) As Boolean
            Try
                Dim cmdGetLogins As New SqlClient.SqlCommand("SELECT Password " & _
                                                             "FROM WebUser " & _
                                                             "WHERE UserID=@UserID", cn)
                cmdGetLogins.Parameters.AddWithValue("@UserID", m_intUserId)
                Dim strPasswordFromDB As String = CStr(cmdGetLogins.ExecuteScalar)
                If System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(strPassword, "sha1") _
                   = strPasswordFromDB Then Return True
                If strPassword = strPasswordFromDB Then Return True
                Return False
            Catch ex As Exception
                Throw New Exception("Fehler bei der Passwortüberprüfung!", ex)
            End Try
        End Function

        Private Function CheckPasswordHistory(ByVal strPassword As String, ByVal cn As SqlClient.SqlConnection) As Boolean
            Dim cmdCheckPassword As New SqlClient.SqlCommand("SELECT Count(DateOfChange) FROM WebUserPwdHistory WHERE UserID=@UserID AND Password=@Password", cn)
            With cmdCheckPassword.Parameters
                .AddWithValue("@UserID", m_intUserId)
                .AddWithValue("@Password", System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(strPassword, "sha1"))
            End With
            If CInt(cmdCheckPassword.ExecuteScalar) > 0 Then
                Return False
            Else
                Return True
            End If
        End Function

        Private Sub SavePasswordHistory(ByVal strPassword As String, ByVal LastPswdChange As Date, ByVal cn As SqlClient.SqlConnection, Optional ByVal blnInitialPswd As Boolean = False)
            Try
                Dim intMaxHistEntries As Integer = m_customer.CustomerPasswordRules.PasswordHistoryEntries
                Dim cmdCountHist As New SqlClient.SqlCommand("SELECT COUNT(Password) FROM WebUserPwdHistory WHERE UserID=@UserID", cn)
                cmdCountHist.Parameters.AddWithValue("@UserID", m_intUserId)
                Dim intCountHist As Integer = CInt(cmdCountHist.ExecuteScalar)
                If intCountHist >= intMaxHistEntries AndAlso intCountHist <> 0 Then
                    Dim cmdDeleteHist As New SqlClient.SqlCommand("DELETE FROM WebUserPwdHistory WHERE ID=(SELECT TOP 1 ID FROM WebUserPwdHistory WHERE UserID=@UserID ORDER BY DateOfChange ASC)", cn)
                    cmdDeleteHist.Parameters.AddWithValue("@UserID", m_intUserId)
                    Do
                        cmdDeleteHist.ExecuteNonQuery()
                        intCountHist -= 1
                    Loop Until intCountHist < intMaxHistEntries OrElse intCountHist < 1
                End If

                If intMaxHistEntries > 0 Then
                    Dim cmdSaveHist As New SqlClient.SqlCommand("INSERT INTO WebUserPwdHistory(UserID, Password, DateOfChange, InitialPwd) Values(@UserID, @Password, @DateOfChange, @InitialPwd)", cn)
                    With cmdSaveHist.Parameters
                        .AddWithValue("@UserID", m_intUserId)
                        .AddWithValue("@Password", System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(strPassword, "sha1"))
                        .AddWithValue("@DateOfChange", Now)
                        .AddWithValue("@InitialPwd", blnInitialPswd)
                    End With
                    cmdSaveHist.ExecuteNonQuery()
                End If
            Catch ex As Exception
                Throw New Exception("Fehler beim Schreiben der Passworthistorie!", ex)
            End Try
        End Sub

        Public Function ChangePasswordNew(ByVal strOldPwd As String, ByVal strNewPwd As String, ByVal strNewPwdConfirm As String, _
                                        ByVal strChangeUser As String, Optional ByVal blnAdmin As Boolean = False, Optional ByVal blnInitialPswd As Boolean = False) As Boolean
            Dim cn As New SqlClient.SqlConnection(m_strConnectionstring)
            cn.Open()
            If blnAdmin OrElse VerifyPassword(strOldPwd, cn) Then
                If blnAdmin OrElse (Not strOldPwd = String.Empty) Then
                    If Not strNewPwd = String.Empty Then
                        If Not strOldPwd = strNewPwd Then
                            If strNewPwd = strNewPwdConfirm Then
                                If m_customer.CustomerPasswordRules.PasswordIsValid(strNewPwd) Then
                                    If CheckPasswordHistory(strNewPwd, cn) Then
                                        Try
                                            Dim cmdUpdate As New SqlClient.SqlCommand("UPDATE WebUser " & _
                                                                                      "SET Password=@Password, LastPwdChange=@LastPwdChange, LastLogin=@LastLogin, LastChangedBy=@ChangeUser " & _
                                                                                      "WHERE UserID=@UserID", cn)
                                            Dim tmpLastPwdChange As Date
                                            With cmdUpdate.Parameters
                                                .AddWithValue("@UserID", m_intUserId)
                                                .AddWithValue("@Password", System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(strNewPwd, "sha1"))
                                                If blnAdmin Then
                                                    .AddWithValue("@LastPwdChange", CDate("03.02.1963"))
                                                    tmpLastPwdChange = CDate("03.02.1963")
                                                Else
                                                    tmpLastPwdChange = Now
                                                    .AddWithValue("@LastPwdChange", tmpLastPwdChange)
                                                End If
                                                .AddWithValue("@LastLogin", tmpLastPwdChange)
                                                .AddWithValue("@ChangeUser", strChangeUser)
                                            End With
                                            cmdUpdate.ExecuteNonQuery()

                                            cmdUpdate = New SqlClient.SqlCommand("UPDATE WebUserHistory " & _
                                                                                     "SET Password=@Password, LastPwdChange=@LastPwdChange,LastChanged=Getdate(),LastChange='Passwort geändert',LastChangedBy=@ChangeUser " & _
                                                                                     "WHERE (UserHistoryID=@UserHistoryID)", cn)
                                            With cmdUpdate.Parameters
                                                .AddWithValue("@UserHistoryID", m_intUserHistoryID)
                                                .AddWithValue("@LastPwdChange", tmpLastPwdChange)
                                                .AddWithValue("@ChangeUser", strChangeUser)
                                                .AddWithValue("@Password", System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(strNewPwd, "sha1"))
                                            End With
                                            cmdUpdate.ExecuteNonQuery()
                                            SavePasswordHistory(strNewPwd, tmpLastPwdChange, cn, blnInitialPswd)
                                            Return True
                                        Catch ex As Exception
                                            Throw New Exception("Fehler beim Ändern des Passworts: " & ex.Message, ex)
                                        Finally
                                            If cn.State <> ConnectionState.Closed Then
                                                cn.Close()
                                            End If
                                        End Try
                                    Else
                                        m_strErrorMessage = String.Format("Dieses Passwort kann nicht wiederverwendet werden. Die letzten {0} Kennwörter eines Benutzers sind für eine nochmalige Verwendung gesperrt.", m_customer.CustomerPasswordRules.PasswordHistoryEntries)
                                    End If
                                Else
                                    m_strErrorMessage = m_customer.CustomerPasswordRules.ErrorMessage
                                End If
                            Else
                                m_strErrorMessage = "Das neue Passwort und die Passwortbestätigung müssen einander entsprechen."
                            End If
                        Else
                            m_strErrorMessage = "Das alte und das neue Passwort dürfen nicht gleich sein."
                        End If
                    Else
                        m_strErrorMessage = "Leere Zeichenfolge ist nicht erlaubt!"
                    End If
                Else
                    m_strErrorMessage = "Sie müssen Ihr altes Passwort angeben!"
                End If
            ElseIf strOldPwd.Length <> strOldPwd.Trim.Length Then
                m_strErrorMessage = "Überprüfen Sie Ihr altes Passwort! Leerzeichen nicht erlaubt!"
            Else
                m_strErrorMessage = ""
            End If
            Return False
        End Function
        Public Function ChangePassword(ByVal strOldPwd As String, ByVal strNewPwd As String, ByVal strNewPwdConfirm As String, _
                                ByVal strChangeUser As String, Optional ByVal blnAdmin As Boolean = False) As Boolean
            Dim cn As New SqlClient.SqlConnection(m_strConnectionstring)
            cn.Open()
            If blnAdmin OrElse VerifyPassword(strOldPwd, cn) Then
                If blnAdmin OrElse (Not strOldPwd = String.Empty) Then
                    If Not strNewPwd = String.Empty Then
                        If Not strOldPwd = strNewPwd Then
                            If strNewPwd = strNewPwdConfirm Then
                                If m_customer.CustomerPasswordRules.PasswordIsValid(strNewPwd) Then
                                    If CheckPasswordHistory(strNewPwd, cn) Then
                                        Try
                                            Dim cmdUpdate As New SqlClient.SqlCommand("UPDATE WebUser " & _
                                                                                      "SET Password=@Password, LastPwdChange=@LastPwdChange, LastLogin=@LastLogin, LastChangedBy=@ChangeUser " & _
                                                                                      "WHERE UserID=@UserID", cn)
                                            Dim tmpLastPwdChange As Date
                                            With cmdUpdate.Parameters
                                                .AddWithValue("@UserID", m_intUserId)
                                                .AddWithValue("@Password", System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(strNewPwd, "sha1"))

                                                If blnAdmin Then
                                                    .AddWithValue("@LastPwdChange", CDate("03.02.1963"))
                                                    tmpLastPwdChange = CDate("03.02.1963")
                                                Else
                                                    tmpLastPwdChange = Now
                                                    .AddWithValue("@LastPwdChange", tmpLastPwdChange)

                                                End If
                                                .AddWithValue("@LastLogin", tmpLastPwdChange)
                                                .AddWithValue("@ChangeUser", strChangeUser)
                                            End With
                                            cmdUpdate.ExecuteNonQuery()

                                            cmdUpdate = New SqlClient.SqlCommand("UPDATE WebUserHistory " & _
                                                                                     "SET Password=@Password, LastPwdChange=@LastPwdChange,LastChanged=Getdate(),LastChange='Passwort geändert',LastChangedBy=@ChangeUser " & _
                                                                                     "WHERE (UserHistoryID=@UserHistoryID)", cn)
                                            With cmdUpdate.Parameters
                                                .AddWithValue("@UserHistoryID", m_intUserHistoryID)
                                                .AddWithValue("@ChangeUser", strChangeUser)
                                                .AddWithValue("@Password", System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(strNewPwd, "sha1"))
                                                .AddWithValue("@LastPwdChange", tmpLastPwdChange)

                                            End With
                                            cmdUpdate.ExecuteNonQuery()
                                            SavePasswordHistory(strNewPwd, tmpLastPwdChange, cn)
                                            m_dtmLastPwdChange = tmpLastPwdChange
                                            If (Not m_blnPwdNeverExpires) AndAlso (m_dtmLastPwdChange < Now.Subtract(System.TimeSpan.FromDays(m_customer.CustomerLoginRules.NewPasswordAfterNDays))) Then
                                                m_blnPasswordExpired = True
                                            Else
                                                m_blnPasswordExpired = False
                                            End If
                                            Return True
                                        Catch ex As Exception
                                            Throw New Exception("Fehler beim Ändern des Passworts: " & ex.Message, ex)
                                        Finally

                                            If cn.State <> ConnectionState.Closed Then
                                                cn.Close()
                                            End If
                                        End Try
                                    Else
                                        m_strErrorMessage = String.Format("Dieses Passwort kann nicht wiederverwendet werden. Die letzten {0} Passwörter eines Benutzers sind für eine nochmalige Verwendung gesperrt.", m_customer.CustomerPasswordRules.PasswordHistoryEntries)
                                    End If
                                Else
                                    m_strErrorMessage = m_customer.CustomerPasswordRules.ErrorMessage
                                End If
                            Else
                                m_strErrorMessage = "Das neue Passwort und die Passwortbestätigung müssen einander entsprechen."
                            End If
                        Else
                            m_strErrorMessage = "Das alte und das neue Passwort dürfen nicht gleich sein."
                        End If
                    Else
                        m_strErrorMessage = "Leere Zeichenfolge ist nicht erlaubt!"
                    End If
                Else
                    m_strErrorMessage = "Sie müssen Ihr altes Passwort angeben!"
                End If
            ElseIf strOldPwd.Length <> strOldPwd.Trim.Length Then
                m_strErrorMessage = "Überprüfen Sie Ihr altes Passwort! Leerzeichen nicht erlaubt!"
            Else
                m_strErrorMessage = ""
            End If
            Return False
        End Function

        Public Function ChangePasswordFirstLogin(ByVal strNewPwd As String, ByVal strNewPwdConfirm As String, _
                        ByVal strChangeUser As String) As Boolean
            Dim cn As New SqlClient.SqlConnection(m_strConnectionstring)
            cn.Open()
            If Not strNewPwd = String.Empty Then
                If strNewPwd = strNewPwdConfirm Then
                    If m_customer.CustomerPasswordRules.PasswordIsValid(strNewPwd) Then
                        If CheckPasswordHistory(strNewPwd, cn) Then
                            Try
                                Dim cmdUpdate As New SqlClient.SqlCommand("UPDATE WebUser " & _
                                                                          "SET Password=@Password, LastPwdChange=@LastPwdChange, LastLogin=@LastLogin, LastChangedBy=@ChangeUser " & _
                                                                          "WHERE UserID=@UserID", cn)
                                Dim tmpLastPwdChange As Date
                                With cmdUpdate.Parameters
                                    .AddWithValue("@UserID", m_intUserId)
                                    .AddWithValue("@Password", System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(strNewPwd, "sha1"))
                                    tmpLastPwdChange = Now
                                    .AddWithValue("@LastPwdChange", tmpLastPwdChange)
                                    .AddWithValue("@LastLogin", tmpLastPwdChange)
                                    .AddWithValue("@ChangeUser", strChangeUser)
                                End With
                                cmdUpdate.ExecuteNonQuery()

                                cmdUpdate = New SqlClient.SqlCommand("UPDATE WebUserHistory " & _
                                                                         "SET Password=@Password, LastPwdChange=@LastPwdChange,LastChanged=Getdate(),LastChange='Passwort geändert',LastChangedBy=@ChangeUser " & _
                                                                         "WHERE (UserHistoryID=@UserHistoryID)", cn)
                                With cmdUpdate.Parameters
                                    .AddWithValue("@UserHistoryID", m_intUserHistoryID)
                                    .AddWithValue("@ChangeUser", strChangeUser)
                                    .AddWithValue("@Password", System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(strNewPwd, "sha1"))
                                    .AddWithValue("@LastPwdChange", tmpLastPwdChange)

                                End With
                                cmdUpdate.ExecuteNonQuery()
                                SavePasswordHistory(strNewPwd, tmpLastPwdChange, cn)
                                m_dtmLastPwdChange = tmpLastPwdChange
                                If (Not m_blnPwdNeverExpires) AndAlso (m_dtmLastPwdChange < Now.Subtract(System.TimeSpan.FromDays(m_customer.CustomerLoginRules.NewPasswordAfterNDays))) Then
                                    m_blnPasswordExpired = True
                                Else
                                    m_blnPasswordExpired = False
                                End If
                                Return True
                            Catch ex As Exception
                                Throw New Exception("Fehler beim Ändern des Passworts: " & ex.Message, ex)
                            Finally

                                If cn.State <> ConnectionState.Closed Then
                                    cn.Close()
                                End If
                            End Try
                        Else
                            m_strErrorMessage = String.Format("Dieses Passwort kann nicht wiederverwendet werden. Die letzten {0} Passwörter eines Benutzers sind für eine nochmalige Verwendung gesperrt.", m_customer.CustomerPasswordRules.PasswordHistoryEntries)
                        End If
                    Else
                        m_strErrorMessage = m_customer.CustomerPasswordRules.ErrorMessage
                    End If
                Else
                    m_strErrorMessage = "Das neue Passwort und die Passwortbestätigung müssen einander entsprechen."
                End If
            Else
                m_strErrorMessage = "Leere Zeichenfolge ist nicht erlaubt!"
            End If
            Return False
        End Function

        Public Function SendPasswordMail(ByVal password As String, ByRef errorMessage As String, Optional ByVal blnSendPasswordAnyway As Boolean = False) As Boolean
            Try
                If (Not m_customer.CustomerPasswordRules.DontSendEmail) Or blnSendPasswordAnyway Then
                    If (m_mail <> String.Empty) Then
                        Dim smtpMailServer As String = ConfigurationManager.AppSettings("SmtpMailServer")
                        Dim smtpMailSender As String = ConfigurationManager.AppSettings("SmtpMailSender")
                        Dim subject As String = "Ihr persönliches Passwort"
                        Dim textBuilder As New System.Text.StringBuilder()
                        With textBuilder
                            .Append("Guten Tag,")
                            .Append(ControlChars.CrLf)
                            .Append(ControlChars.CrLf)
                            .Append("Ihr persönliches Passwort für den Internetbereich der Kroschke Gruppe und dem Deutschen Auto Dienst lautet:")
                            .Append(ControlChars.CrLf)
                            .Append(ControlChars.CrLf)
                            .AppendFormat("{0}{1}(Bitte achten Sie auf die korrekte Groß- und Kleinschreibung)", password, ControlChars.CrLf)
                            .Append(ControlChars.CrLf)
                            .Append(ControlChars.CrLf)
                            .Append("Mit diesem Passwort sowie Ihrem Usernamen können Sie nun das Login vornehmen.")
                            .Append(ControlChars.CrLf)
                            .Append("Wir müssen Sie bitten, Ihr Passwort nach der ersten Anmeldung aus Sicherheitsgründen zu ändern.")
                            .Append(ControlChars.CrLf)
                            .Append("Bitte bewahren Sie Ihren Usernamen sowie Ihr Online-Passwort an einem sicheren Platz auf und geben Sie Ihre Zugangsdaten nicht an Dritte weiter.")
                            .Append(ControlChars.CrLf)
                            .Append(ControlChars.CrLf)
                            .Append("Mit freundlichen Grüßen")
                            .Append(ControlChars.CrLf)
                            .Append("Christoph Kroschke GmbH /")
                            .Append(ControlChars.CrLf)
                            .Append("Deutscher Auto Dienst GmbH")
                        End With

                        Dim client As New System.Net.Mail.SmtpClient(smtpMailServer)
                        client.Send(smtpMailSender, m_mail, subject, textBuilder.ToString)

                        Return True
                    Else
                        errorMessage = "Keine Mailadresse angegeben. Neues Passwort konnte nicht versendet werden."
                        Return False
                    End If
                End If
            Catch ex As Exception
                errorMessage = String.Format("Beim Mailversand ist ein technisches Problem aufgetreten. Die Email wurde möglicherweise nicht verschickt. (Wortlaut der Fehlermeldung: {0})", ex.Message)
                Return False
            End Try
        End Function

        Public Function SendUsernameMail(ByRef errorMessage As String, ByVal Reapproved As Boolean, Optional ByVal blnSendUsernameAnyway As Boolean = False) As Boolean
            Try
                If (Not m_customer.CustomerUsernameRules.DontSendEmail) Or blnSendUsernameAnyway Then
                    If (m_mail <> String.Empty) Then
                        Dim smtpMailServer As String = ConfigurationManager.AppSettings("SmtpMailServer")
                        Dim smtpMailSender As String = ConfigurationManager.AppSettings("SmtpMailSender")
                        Dim subject As String = "Ihr persönlicher Username"
                        Dim textBuilder As New System.Text.StringBuilder()

                        With textBuilder
                            .Append("Guten Tag,")
                            .Append(ControlChars.CrLf)
                            .Append(ControlChars.CrLf)
                            .Append("Ihr neuer persönlicher Username für den Internetbereich der Kroschke Gruppe und dem Deutschen Auto Dienst lautet:")
                            .Append(ControlChars.CrLf)
                            .Append(ControlChars.CrLf)
                            .Append(m_strUsername & " (Bitte achten Sie auf die korrekte Groß- und Kleinschreibung)")
                            .Append(ControlChars.CrLf)
                            .Append(ControlChars.CrLf)
                            .Append("Mit diesem Username sowie Ihrem persönlichen Passwort können Sie das Login vornehmen.")
                            .Append(ControlChars.CrLf)
                            .Append(ControlChars.CrLf)
                            .Append("Hinweis!: Das persönliche Passwort erhalten Sie in einer separaten E-mail.")
                            .Append(ControlChars.CrLf)
                            .Append(ControlChars.CrLf)
                            .Append("Mit freundlichen Grüßen")
                            .Append(ControlChars.CrLf)
                            .Append("Christoph Kroschke GmbH /")
                            .Append(ControlChars.CrLf)
                            .Append("Deutscher Auto Dienst GmbH")

                        End With

                        Dim client As New System.Net.Mail.SmtpClient(smtpMailServer)
                        client.Send(smtpMailSender, m_mail, subject, textBuilder.ToString)

                        Return True
                    Else
                        errorMessage = "Keine Mailadresse angegeben. Der Username konnte nicht versendet werden."
                        Return False
                    End If
                End If
            Catch ex As Exception
                errorMessage = String.Format("Beim Mailversand ist ein technisches Problem aufgetreten. Die Email wurde möglicherweise nicht verschickt. (Wortlaut der Fehlermeldung: {0})", ex.Message)
                Return False
            End Try
        End Function

        Public Function SendUsernameChangedMail(ByRef errorMessage As String, Optional ByVal blnSendUsernameAnyway As Boolean = False) As Boolean
            Try
                If (Not m_customer.CustomerUsernameRules.DontSendEmail) Or blnSendUsernameAnyway Then
                    If (m_mail <> String.Empty) Then
                        Dim smtpMailServer As String = ConfigurationManager.AppSettings("SmtpMailServer")
                        Dim smtpMailSender As String = ConfigurationManager.AppSettings("SmtpMailSender")
                        Dim subject As String = "Ihr Username wurde geändert"
                        Dim textBuilder As New System.Text.StringBuilder()
                        With textBuilder
                            .Append("Guten Tag,")
                            .Append(ControlChars.CrLf)
                            .Append(ControlChars.CrLf)
                            .Append("Ihr neuer persönlicher Username für den Internetbereich der Kroschke Gruppe und dem Deutschen Auto Dienst hat sich geändert.")
                            .Append(ControlChars.CrLf)
                            .Append("Ihr neuer Username lautet:")
                            .Append(ControlChars.CrLf)
                            .Append(ControlChars.CrLf)
                            .Append(m_strUsername & " (Bitte achten Sie auf die korrekte Groß- und Kleinschreibung)")
                            .Append(ControlChars.CrLf)
                            .Append(ControlChars.CrLf)
                            .Append("Mit diesem Username sowie Ihrem persönlichen Passwort können Sie das Login vornehmen.")
                            .Append(ControlChars.CrLf)
                            .Append(ControlChars.CrLf)
                            .Append("Hinweis!: Das persönliche Passwort erhalten Sie in einer separaten E-mail.")
                            .Append(ControlChars.CrLf)
                            .Append(ControlChars.CrLf)
                            .Append("Mit freundlichen Grüßen")
                            .Append(ControlChars.CrLf)
                            .Append("Christoph Kroschke GmbH /")
                            .Append(ControlChars.CrLf)
                            .Append("Deutscher Auto Dienst GmbH")
                        End With

                        Dim client As New System.Net.Mail.SmtpClient(smtpMailServer)
                        client.Send(smtpMailSender, m_mail, subject, textBuilder.ToString)

                        Return True
                    Else
                        errorMessage = "Keine Mailadresse angegeben. Der Username konnte nicht versendet werden."
                        Return False
                    End If
                End If
            Catch ex As Exception
                errorMessage = String.Format("Beim Mailversand ist ein technisches Problem aufgetreten. Die Email wurde möglicherweise nicht verschickt. (Wortlaut der Fehlermeldung: {0})", ex.Message)
                Return False
            End Try
        End Function

        Public Function SendUsernameMail(ByRef errorMessage As String, ByVal PortalLinkID As Integer, ByVal Goodlink As String, ByVal BadLink As String, ByVal currentUser As User, Optional ByVal blnSendUsernameAnyway As Boolean = False) As Boolean
            Try
                If (Not m_customer.CustomerUsernameRules.DontSendEmail) Or blnSendUsernameAnyway Then
                    If (m_mail <> String.Empty) Then
                        Dim smtpMailServer As String = ConfigurationManager.AppSettings("SmtpMailServer")
                        Dim smtpMailSender As String = ConfigurationManager.AppSettings("SmtpMailSender")
                        Dim subject As String = "Ihr persönlicher Username"
                        Dim textBuilder As New System.Text.StringBuilder()

                        Dim Portallink As String = LoadLoginLinks(PortalLinkID)

                        If Portallink = String.Empty Then
                            Portallink = "https://kundenportal.kroschke.de/AutohausPortal/Start/Login.aspx"
                        End If

                        ' Bsp.: "https://sgw.kroschke.de[/]Services/Start/Login.aspx" --> Das richtige Trennzeichen [] suchen, um den aktuellen Server auszulesen
                        Dim iSplitindex As Integer = 0
                        For i As Integer = 0 To 2
                            iSplitindex = Portallink.IndexOf("/", iSplitindex)
                            iSplitindex += 1
                        Next

                        Dim sServer As String = Portallink.Remove(iSplitindex - 1)
                        Dim sValidationLink As String = sServer & "/Validation/Forms/Confirmation.aspx?key="


                        With textBuilder
                            .AppendFormat("Sehr geehrte(r) {0} {1} {2},", m_title, m_firstname, m_lastname)
                            .Append(ControlChars.CrLf)
                            .Append(ControlChars.CrLf)
                            .Append("mit dieser Email übersenden wir Ihnen die Zugangsdaten zum Internetbereich der Kroschke Gruppe und dem Deutschen Auto Dienst GmbH. ")
                            .Append(ControlChars.CrLf)
                            .Append(ControlChars.CrLf)
                            .Append("Bitte speichern Sie sich folgenden Link in Ihrem Internetbrowser ab:")
                            .Append(ControlChars.CrLf)
                            .Append(ControlChars.CrLf)
                            .Append(Portallink)
                            .Append(ControlChars.CrLf)
                            .Append(ControlChars.CrLf)
                            .Append("Ihr Benutzername lautet: " & m_strUsername)
                            .Append(ControlChars.CrLf)
                            .Append(ControlChars.CrLf)
                            .AppendFormat("Sofern Sie der korrekte Empfänger ({0} {1}) dieser Mail sind, klicken Sie bitte auf den folgenden Link, um Ihre Benutzerdaten zu bestätigen und das Portal nutzen zu können. Nach der Bestätigung erhalten Sie das Passwort per Mail.", m_firstname, m_lastname)
                            .Append(ControlChars.CrLf)
                            .Append(sValidationLink & Goodlink)
                            .Append(ControlChars.CrLf)
                            .Append(ControlChars.CrLf)
                            .Append("Sofern Sie nicht der korrekte Empfänger dieser Mail sind, klicken Sie bitte auf den folgenden Link:")
                            .Append(ControlChars.CrLf)
                            .Append(sValidationLink & BadLink)
                            .Append(ControlChars.CrLf)
                            .Append("oder melden Sie sich telefonisch bei Ihrem Web-Administrator.")
                            .Append(ControlChars.CrLf)
                            .Append(ControlChars.CrLf)
                            .Append(ControlChars.CrLf)
                            .Append("Für Rückfragen zur Anmeldung stehen wir Ihnen jederzeit gern zur Verfügung.")
                            .Append(ControlChars.CrLf)
                            .Append(ControlChars.CrLf)
                            .Append("Mit freundlichen Grüßen")
                            .Append(ControlChars.CrLf)
                            .Append(ControlChars.CrLf)
                            .AppendFormat("Web-Administration, {0} {1}", currentUser.FirstName, currentUser.LastName)

                        End With

                        Dim client As New System.Net.Mail.SmtpClient(smtpMailServer)
                        client.Send(smtpMailSender, m_mail, subject, textBuilder.ToString)

                        Return True
                    Else
                        errorMessage = "Keine Mailadresse angegeben. Der Username konnte nicht versendet werden."
                        Return False
                    End If
                End If
            Catch ex As Exception
                errorMessage = String.Format("Beim Mailversand ist ein technisches Problem aufgetreten. Die Email wurde möglicherweise nicht verschickt. (Wortlaut der Fehlermeldung: {0})", ex.Message)
                Return False
            End Try
        End Function

        Public Sub UpdateWebUserUploadMailSend(ByVal isSend As Boolean)
            Dim cn As New SqlClient.SqlConnection(m_app.Connectionstring)
            cn.Open()

            Dim cmdUpdate As New SqlClient.SqlCommand("UPDATE WebUserUpload SET MailSend=@MailSend where UserID=" & m_intUserId, cn)

            With cmdUpdate.Parameters
                .AddWithValue("@UserID", UserID)
                .AddWithValue("@MailSend", CInt(isSend))
            End With
            cmdUpdate.ExecuteNonQuery()

            cn.Close()
            cn.Dispose()

        End Sub

        ''' <summary>
        ''' Prüft ob der Benutzer den kompletten Validationsprozess durchlaufen hat.
        ''' <Return>
        '''     
        ''' </Return>
        ''' </summary>


        Public Function CheckValidationConfirmed() As Boolean
            Dim blnConfirmed As Boolean = False

            Try
                Using connection As New SqlClient.SqlConnection(m_app.Connectionstring)
                    Dim command1 As New SqlClient.SqlCommand("SELECT UserID FROM WebUser where UserID=" & m_intUserId, connection)
                    Dim command2 As New SqlClient.SqlCommand("SELECT Confirmed FROM WebUserUpload where UserID=" & m_intUserId, connection)
                    connection.Open()

                    Dim reader1 As SqlClient.SqlDataReader = command1.ExecuteReader()

                    If reader1.HasRows Then
                        Dim reader2 As SqlClient.SqlDataReader = command2.ExecuteReader()
                        While reader2.Read()
                            blnConfirmed = reader2.GetBoolean(0)
                            Exit While
                        End While

                        reader2.Close()
                    End If

                    reader1.Close()

                    connection.Close()
                    connection.Dispose()
                End Using


            Catch ex As Exception

            End Try

            Return blnConfirmed
        End Function

        ''' <summary>
        ''' Prüft ob ein Benutzer den kompletten Validationsprozess durchlaufen hat.
        ''' <Return>
        '''     
        ''' </Return>
        ''' </summary>

        Public Shared Function CheckValidationConfirmed(ByVal UserID As String, ByVal Connectionstring As String) As Boolean
            Dim blnConfirmed As Boolean = False

            Try
                Using connection As New SqlClient.SqlConnection(Connectionstring)
                    Dim command1 As New SqlClient.SqlCommand("SELECT UserID FROM WebUser where UserID=" & UserID, connection)
                    Dim command2 As New SqlClient.SqlCommand("SELECT Confirmed FROM WebUserUpload where UserID=" & UserID, connection)
                    connection.Open()

                    Dim reader1 As SqlClient.SqlDataReader = command1.ExecuteReader()

                    If reader1.HasRows Then
                        Dim reader2 As SqlClient.SqlDataReader = command2.ExecuteReader()
                        While reader2.Read()
                            blnConfirmed = reader2.GetBoolean(0)
                            Exit While
                        End While

                        reader2.Close()
                    End If

                    reader1.Close()

                    connection.Close()
                    connection.Dispose()
                End Using

            Catch ex As Exception

            End Try

            Return blnConfirmed
        End Function

        ''' <summary>
        ''' Prüft ob eine EMail mit Benutzerdaten für den Validationsprozess versendet wurde.
        ''' <Return>
        '''     
        ''' </Return>
        ''' </summary>

        Public Function CheckValidationMailsend() As Boolean
            Dim blnConfirmed As Boolean = False

            Try
                Using connection As New SqlClient.SqlConnection(m_app.Connectionstring)
                    Dim command1 As New SqlClient.SqlCommand("SELECT UserID FROM WebUser where UserID=" & m_intUserId, connection)
                    Dim command2 As New SqlClient.SqlCommand("SELECT Mailsend FROM WebUserUpload where UserID=" & m_intUserId, connection)
                    connection.Open()

                    Dim reader1 As SqlClient.SqlDataReader = command1.ExecuteReader()

                    If reader1.HasRows Then
                        Dim reader2 As SqlClient.SqlDataReader = command2.ExecuteReader()
                        While reader2.Read()
                            blnConfirmed = reader2.GetBoolean(0)
                            Exit While
                        End While

                        reader2.Close()
                    End If

                    reader1.Close()

                    connection.Close()
                    connection.Dispose()
                End Using

            Catch ex As Exception

            End Try

            Return blnConfirmed
        End Function

        ''' <summary>
        ''' Prüft ob eine EMail mit Benutzerdaten für den Validationsprozess versendet wurde.
        ''' <Return>
        '''     
        ''' </Return>
        ''' </summary>

        Public Shared Function CheckValidationMailsend(ByVal UserID As String, ByVal Connectionstring As String) As Boolean
            Dim blnConfirmed As Boolean = False

            Try
                Using connection As New SqlClient.SqlConnection(Connectionstring)
                    Dim command1 As New SqlClient.SqlCommand("SELECT UserID FROM WebUser where UserID=" & UserID, connection)
                    Dim command2 As New SqlClient.SqlCommand("SELECT Mailsend FROM WebUserUpload where UserID=" & UserID, connection)
                    connection.Open()

                    Dim reader1 As SqlClient.SqlDataReader = command1.ExecuteReader()

                    If reader1.HasRows Then
                        Dim reader2 As SqlClient.SqlDataReader = command2.ExecuteReader()
                        While reader2.Read()
                            blnConfirmed = reader2.GetBoolean(0)
                            Exit While
                        End While

                        reader2.Close()
                    End If

                    reader1.Close()

                    connection.Close()
                    connection.Dispose()
                End Using

            Catch ex As Exception

            End Try

            Return blnConfirmed
        End Function

        Public Function SendUserUnlockMail(ByRef errorMessage As String, currentUser As User, Optional ByVal blnSendUsernameAnyway As Boolean = False) As Boolean
            Try
                If (Not m_customer.CustomerUsernameRules.DontSendEmail) Or blnSendUsernameAnyway Then
                    If (m_mail <> String.Empty) Then
                        Dim smtpMailServer As String = ConfigurationManager.AppSettings("SmtpMailServer")
                        Dim smtpMailSender As String = ConfigurationManager.AppSettings("SmtpMailSender")
                        Dim subject As String = "Ihr Benutzerkonto wurde entsperrt"
                        Dim textBuilder As New System.Text.StringBuilder()
                        Dim PortalLink As String = LoadLoginLinks(m_customer.LoginLinkID)

                        If PortalLink = String.Empty Then
                            PortalLink = "https://kundenportal.kroschke.de/AutohausPortal/"
                        End If

                        ' Bsp.: "https://sgw.kroschke.de/Services[/]Start/Login.aspx" --> Das richtige Trennzeichen [] suchen, um den aktuellen Server auszulesen
                        Dim iSplitindex As Integer = 0
                        For i As Integer = 0 To 3
                            iSplitindex = PortalLink.IndexOf("/", iSplitindex)
                            iSplitindex += 1
                        Next

                        Dim sServer As String = PortalLink.Remove(iSplitindex - 1)

                        With textBuilder
                            .Append("Sehr geehrte(r) " & m_title & " " & m_firstname & " " & m_lastname & ",")
                            .Append(ControlChars.CrLf)
                            .Append(ControlChars.CrLf)
                            .Append("Ihr Benutzerkonto für Internetbereich der Kroschke Gruppe und dem Deutschen Auto Dienst GmbH wurde entsperrt.")
                            .Append(ControlChars.CrLf)
                            .Append(ControlChars.CrLf)
                            .Append("Sie können sich unter folgendem Link,")
                            .Append(ControlChars.CrLf)
                            .Append(ControlChars.CrLf)
                            .Append(PortalLink)
                            .Append(ControlChars.CrLf)
                            .Append(ControlChars.CrLf)
                            .Append("mit Ihrem Benutzernamen " & m_strUsername & " und Ihrem persönlichen Passwort anmelden.")
                            .Append(ControlChars.CrLf)
                            .Append(ControlChars.CrLf)
                            .Append(ControlChars.CrLf)
                            .Append("Haben Sie Fragen oder benötigen Sie ein neues Passwort, melden Sie sich telefonisch bei Ihrem Web-Administrator.")
                            .Append(ControlChars.CrLf)
                            .Append(ControlChars.CrLf)
                            .Append("Mit freundlichen Grüßen")
                            .Append(ControlChars.CrLf)
                            .Append(ControlChars.CrLf)
                            .AppendFormat("Web-Administration, {0} {1}", currentUser.FirstName, currentUser.LastName)

                        End With

                        Dim client As New System.Net.Mail.SmtpClient(smtpMailServer)
                        client.Send(smtpMailSender, m_mail, subject, textBuilder.ToString)

                        Return True
                    Else
                        errorMessage = "Keine Mailadresse angegeben. Der Entsperr-Email konnte nicht versendet werden."
                        Return False
                    End If
                End If
            Catch ex As Exception
                errorMessage = String.Format("Beim Mailversand ist ein technisches Problem aufgetreten. Die Email wurde möglicherweise nicht verschickt. (Wortlaut der Fehlermeldung: {0})", ex.Message)
                Return False
            End Try
        End Function

        Private Function LoadLoginLinks(LinkID As Integer) As String

            Dim TempTable As New DataTable
            Dim cn As New SqlClient.SqlConnection(m_app.Connectionstring)
            cn.Open()

            Dim daLoginLink As SqlClient.SqlDataAdapter
            daLoginLink = New SqlClient.SqlDataAdapter("SELECT * FROM WebUserUploadLoginLink where ID=" & LinkID, cn)

            daLoginLink.Fill(TempTable)

            cn.Close()
            cn.Dispose()

            Dim strLink As String = ""

            If TempTable.Rows.Count > 0 Then
                strLink = TempTable.Rows(0)("Text").ToString()
            End If

            Return strLink
        End Function

        Public Function Delete(ByVal intUserId As Integer, ByVal strConnectionString As String, ByVal strChangeUser As String) As Boolean
            Dim cn As New SqlClient.SqlConnection(strConnectionString)
            Try

                cn.Open()
                Dim cmdDeleteWebGroupEmployee As New SqlClient.SqlCommand("DELETE FROM WebGroupEmployee WHERE UserID = @UserID", cn)
                cmdDeleteWebGroupEmployee.Parameters.AddWithValue("@UserId", intUserId)
                cmdDeleteWebGroupEmployee.ExecuteNonQuery()

                Dim cmdDeleteOrgMemberships As New SqlClient.SqlCommand("DELETE FROM OrganizationMember WHERE UserID = @UserID", cn)
                cmdDeleteOrgMemberships.Parameters.AddWithValue("@UserId", intUserId)
                cmdDeleteOrgMemberships.ExecuteNonQuery()
                Dim cmdDeleteMemberships As New SqlClient.SqlCommand("DELETE FROM WebMember WHERE UserId = @UserID", cn)
                cmdDeleteMemberships.Parameters.AddWithValue("@UserId", intUserId)
                cmdDeleteMemberships.ExecuteNonQuery()
                Dim cmdDeletePasswordHistory As New SqlClient.SqlCommand("DELETE FROM WebUserPwdHistory WHERE UserId = @UserID", cn)
                cmdDeletePasswordHistory.Parameters.AddWithValue("@UserId", intUserId)
                cmdDeletePasswordHistory.ExecuteNonQuery()

                Dim cmdHistoryDeleteEntry As New SqlClient.SqlCommand("UPDATE WebUserHistory SET Deleted=1,DeleteDate=Getdate(),LastChanged=Getdate(),LastChange='Benutzer gelöscht',LastChangedBy=@ChangeUser WHERE UserHistoryID IN (SELECT UserHistoryID FROM WebUser WHERE UserID=@UserID)", cn)
                cmdHistoryDeleteEntry.Parameters.AddWithValue("@UserId", intUserId)
                cmdHistoryDeleteEntry.Parameters.AddWithValue("@ChangeUser", strChangeUser)
                cmdHistoryDeleteEntry.ExecuteNonQuery()

                Dim cmdDeleteUser As New SqlClient.SqlCommand("DELETE FROM WebUser WHERE UserId = @UserID", cn)
                cmdDeleteUser.Parameters.AddWithValue("@UserID", intUserId)
                If cmdDeleteUser.ExecuteNonQuery > 0 Then Return True
            Catch ex As Exception
                Throw New Exception("Fehler beim Löschen des Benutzers!", ex)
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
            Return False
        End Function

        Public Function SaveUserInfo(ByVal strChangeUser As String) As Boolean
            Dim cn As New SqlClient.SqlConnection(m_strConnectionstring)
            Dim strUserSQL As String
            Dim cmdUser As New SqlClient.SqlCommand()
            Dim id As Int32

            Try
                cn.Open()
                'Prüfen, ob Eintrag schon vorhanden...
                strUserSQL = "SELECT count(id) FROM WebUserInfo WHERE id_User=@id_User"

                cmdUser.CommandText = strUserSQL
                cmdUser.Connection = cn

                cmdUser.Parameters.AddWithValue("@id_User", m_intUserId)

                id = cmdUser.ExecuteScalar()

                If id = 0 Then 'Insert
                    strUserSQL = "INSERT INTO WebUserInfo(" & _
                                                "[id_user]," & _
                                                "[mail], " & _
                                                "[employee], " & _
                                                "[picture], " & _
                                                "[employeeHierarchy], " & _
                                                "[department], " & _
                                                "[position], " & _
                                                "[LastChangedBy], " & _
                                                "[telephone], " & _
                                                "[fax], " & _
                                                "[Telephone2] " & _
                                        ") VALUES(" & _
                                                "@id_User, " & _
                                                "@mail, " & _
                                                "@employee, " & _
                                                "@picture, " & _
                                                "@employeeHierarchy, " & _
                                                "@department, " & _
                                                "@position, " & _
                                                "@LastChangedBy, " & _
                                                "@telephone, " & _
                                                "@fax, " & _
                                                "@Telephone2 " & _
                                        ")"
                Else
                    strUserSQL = "UPDATE WebUserInfo SET " & _
                                                "[mail]=@mail, " & _
                                                "[employee]=@employee, " & _
                                                "[picture]=@picture, " & _
                                                "[employeeHierarchy]=@employeeHierarchy, " & _
                                                "[department]=@department, " & _
                                                "[position]=@position, " & _
                                                "[LastChangedBy]=@LastChangedBy, " & _
                                                "[telephone]=@telephone, " & _
                                                "[fax]=@fax, " & _
                                                "[Telephone2]=@Telephone2 " & _
                                            "WHERE [id_User]=@id_User"
                End If

                cmdUser.CommandText = strUserSQL

                If m_mail Is Nothing Then m_mail = String.Empty
                With cmdUser.Parameters
                    .AddWithValue("@mail", m_mail)
                    .AddWithValue("@employee", m_blnEmployee)
                    .AddWithValue("@picture", m_blnPicture)
                    .AddWithValue("@employeeHierarchy", m_intHierarchyID)
                    .AddWithValue("@department", m_strDepartment)
                    .AddWithValue("@position", m_strPosition)
                    .AddWithValue("@LastChangedBy", strChangeUser)
                    .AddWithValue("@telephone", m_strPhoneEmployee)
                    .AddWithValue("@fax", m_strFax)
                    .AddWithValue("@Telephone2", m_telephone)
                End With
                cmdUser.ExecuteNonQuery()
            Catch ex As Exception
                Throw New Exception("Fehler beim Speichern der Benutzerdaten!", ex)
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End Function

        Public Function Save() As Boolean
            Dim saveMail As Boolean
            Dim cn As New SqlClient.SqlConnection(m_strConnectionstring)
            Try

                cn.Open()

                Dim strUserSQL As String
                Dim strUserHistorySQL As String
                Dim intTemp As Integer = m_intUserId
                If m_intUserId = -1 Then
                    strUserSQL = "INSERT INTO WebUser(Username, " & _
                                                 "Password, " & _
                                                 "CustomerID, " & _
                                                 "CustomerAdmin, " & _
                                                 "TestUser, " & _
                                                 "PwdNeverExpires, " & _
                                                 "FailedLogins, " & _
                                                 "AccountIsLockedOut, " & _
                                                 "FirstLevelAdmin, " & _
                                                 "Reference, " & _
                                                 "LoggedOn, " & _
                                                 "LastLogin, " & _
                                                 "ReadMessageCount, " & _
                                                 "ReadCustomerMessageCount, " & _
                                                 "UserHistoryID," & _
                                                 "CreatedBy," & _
                                                 "Approved," & _
                                                 "FirstName," & _
                                                 "LastName," & _
                                                 "Title," & _
                                                 "Store," & _
                                                 "Matrix," & _
                                                 "LastChangedBy," & _
                                                 "ValidFrom, " & _
                                                 "UrlRemoteLoginKey) " & _
                             "VALUES(@Username, " & _
                                    "'', " & _
                                    "@CustomerID, " & _
                                    "@CustomerAdmin, " & _
                                    "@TestUser, " & _
                                    "@PwdNeverExpires, " & _
                                    "@FailedLogins, " & _
                                    "@AccountIsLockedOut, " & _
                                    "@FirstLevelAdmin, " & _
                                    "@Reference, " & _
                                    "@LoggedOn, " & _
                                    "GetDate(), " & _
                                    "@ReadMessageCount, " & _
                                    "@ReadCustomerMessageCount, " & _
                                    "@UserHistoryID, " & _
                                    "@CreatedBy, " & _
                                    "@Approved, " & _
                                    "@FirstName, " & _
                                    "@LastName, " & _
                                    "@Title, " & _
                                    "@Store, " & _
                                    "@Matrix, " & _
                                    "@ChangeUser, " & _
                                    "@ValidFrom, " & _
                                    "@UrlRemoteLoginKey); " & _
                         "SELECT SCOPE_IDENTITY()"
                    Dim cmdCheckUserExits As New SqlClient.SqlCommand("SELECT COUNT(UserID) FROM WebUser WHERE Username=@Username", cn)
                    cmdCheckUserExits.Parameters.AddWithValue("@Username", m_strUsername)
                    If cmdCheckUserExits.ExecuteScalar.ToString <> "0" Then
                        m_strErrorMessage = "Es existiert bereits ein Benutzer mit dieser Kennung (Login-Name) im System! Bitte wählen sie eine andere Kennung!"
                        Return False
                    End If
                    Dim cmdCheckMaxUser As New SqlClient.SqlCommand("SELECT COUNT(UserID) FROM WebUser WHERE CustomerID=@CustomerID", cn)
                    cmdCheckMaxUser.Parameters.AddWithValue("@CustomerID", m_customer.CustomerId)
                    If CInt(cmdCheckMaxUser.ExecuteScalar) + 1 > m_customer.MaxUser Then
                        m_strErrorMessage = String.Format("Der Benutzer kann nicht hinzugefügt werden, da sonst die maximale Anzahl von {0} Benutzern überschritten wird.", CStr(m_customer.MaxUser))
                        Return False
                    End If

                    strUserHistorySQL = "INSERT INTO WebUserHistory(Username, " & _
                                                 "Password, " & _
                                                 "CustomerName, " & _
                                                 "CustomerAdmin, " & _
                                                 "TestUser, " & _
                                                 "PwdNeverExpires, " & _
                                                 "FailedLogins, " & _
                                                 "AccountIsLockedOut, " & _
                                                 "FirstLevelAdmin, " & _
                                                 "Reference, " & _
                                                 "LastChange, " & _
                                                 "LastChangedBy, " & _
                                                 "Approved, " & _
                                                 "FirstName, " & _
                                                 "LastName, " & _
                                                 "Title, " & _
                                                 "Store, " & _
                                                 "Matrix, " & _
                                                 "ValidFrom, " & _
                                                 "UrlRemoteLoginKey) " & _
                             "VALUES(@Username, " & _
                                    "'', " & _
                                    "@CustomerName, " & _
                                    "@CustomerAdmin, " & _
                                    "@TestUser, " & _
                                    "@PwdNeverExpires, " & _
                                    "@FailedLogins, " & _
                                    "@AccountIsLockedOut, " & _
                                    "@FirstLevelAdmin, " & _
                                    "@Reference, " & _
                                    "'Benutzer angelegt', " & _
                                    "@ChangeUser, " & _
                                    "@Approved, " & _
                                    "@FirstName, " & _
                                    "@LastName, " & _
                                    "@Title, " & _
                                    "@Store, " & _
                                    "@Matrix, " & _
                                    "@ValidFrom, " & _
                                    "@UrlRemoteLoginKey); " & _
                           "SELECT SCOPE_IDENTITY()"

                    Dim cmdUserHistory As New SqlClient.SqlCommand(strUserHistorySQL, cn)
                    With cmdUserHistory.Parameters
                        .AddWithValue("@Username", m_strUsername)
                        .AddWithValue("@CustomerName", m_customer.CustomerName)
                        .AddWithValue("@CustomerAdmin", m_blnIsCustomerAdmin)
                        .AddWithValue("@TestUser", m_blnTestUser)
                        .AddWithValue("@PwdNeverExpires", m_blnPwdNeverExpires)
                        .AddWithValue("@FailedLogins", m_intFailedLogins)
                        .AddWithValue("@AccountIsLockedOut", m_blnAccountIsLockedOut)
                        .AddWithValue("@FirstLevelAdmin", m_blnFirstLevelAdmin)
                        .AddWithValue("@Reference", m_strReference)
                        .AddWithValue("@ChangeUser", m_strCreatedBy)
                        .AddWithValue("@Approved", m_approved)
                        .AddWithValue("@FirstName", m_firstname)
                        .AddWithValue("@LastName", m_lastname)
                        .AddWithValue("@Title", m_title)
                        .AddWithValue("@Store", m_store)
                        .AddWithValue("@Matrix", m_blnMatrixfilled)
                        .AddWithValue("@ValidFrom", m_ValidFrom)
                        .AddWithValue("@UrlRemoteLoginKey", CStr(IIf(m_strUrlRemoteLoginKey Is Nothing, "", m_strUrlRemoteLoginKey)))
                    End With

                    m_intUserHistoryID = CInt(cmdUserHistory.ExecuteScalar)


                Else
                    strUserSQL = "UPDATE WebUser " & _
                             "SET Username=@Username, " & _
                                 "CustomerID=@CustomerID, " & _
                                 "CustomerAdmin=@CustomerAdmin, " & _
                                 "TestUser=@TestUser, " & _
                                 "PwdNeverExpires=@PwdNeverExpires, " & _
                                 "FailedLogins=@FailedLogins, " & _
                                 "AccountIsLockedOut=@AccountIsLockedOut, " & _
                                 "FirstLevelAdmin=@FirstLevelAdmin, " & _
                                 "Reference=@Reference, " & _
                                 "LoggedOn=@LoggedOn, " & _
                                 "ReadMessageCount=@ReadMessageCount, " & _
                                 "ReadCustomerMessageCount=@ReadCustomerMessageCount, " & _
                                 "Approved=@Approved, " & _
                                 "FirstName=@FirstName, " & _
                                 "LastName=@LastName, " & _
                                 "Title=@Title, " & _
                                 "Store=@Store, " & _
                                 "LastChangedBy=@ChangeUser, " & _
                                 "Matrix=@Matrix, " & _
                                 "ValidFrom=@ValidFrom, " & _
                                 "UrlRemoteLoginKey=@UrlRemoteLoginKey " & _
                             "WHERE UserID=@UserID"

                    strUserHistorySQL = "UPDATE WebUserHistory " & _
                             "SET Username=@Username, " & _
                                 "CustomerName=@CustomerName, " & _
                                 "CustomerAdmin=@CustomerAdmin, " & _
                                 "TestUser=@TestUser, " & _
                                 "PwdNeverExpires=@PwdNeverExpires, " & _
                                 "FailedLogins=@FailedLogins, " & _
                                 "AccountIsLockedOut=@AccountIsLockedOut, " & _
                                 "FirstLevelAdmin=@FirstLevelAdmin, " & _
                                 "Reference=@Reference, " & _
                                 "LastChanged=Getdate(), " & _
                                 "LastChange='Benutzer aktualisiert', " & _
                                 "LastChangedBy=@ChangeUser, " & _
                                 "Approved=@Approved, " & _
                                 "FirstName=@FirstName, " & _
                                 "LastName=@LastName, " & _
                                 "Title=@Title, " & _
                                 "Store=@Store, " & _
                                 "Matrix=@Matrix, " & _
                                 "ValidFrom=@ValidFrom, " & _
                                 "UrlRemoteLoginKey=@UrlRemoteLoginKey " & _
                             "WHERE UserHistoryID=@UserHistoryID"

                    Dim cmdUserHistory As New SqlClient.SqlCommand(strUserHistorySQL, cn)
                    With cmdUserHistory.Parameters
                        .AddWithValue("@Username", m_strUsername)
                        .AddWithValue("@CustomerName", m_customer.CustomerName)
                        .AddWithValue("@CustomerAdmin", m_blnIsCustomerAdmin)
                        .AddWithValue("@TestUser", m_blnTestUser)
                        .AddWithValue("@PwdNeverExpires", m_blnPwdNeverExpires)
                        .AddWithValue("@FailedLogins", m_intFailedLogins)
                        .AddWithValue("@AccountIsLockedOut", m_blnAccountIsLockedOut)
                        .AddWithValue("@FirstLevelAdmin", m_blnFirstLevelAdmin)
                        .AddWithValue("@Reference", m_strReference)
                        .AddWithValue("@ChangeUser", m_strCreatedBy)
                        .AddWithValue("@UserHistoryID", m_intUserHistoryID)
                        .AddWithValue("@Approved", m_approved)
                        .AddWithValue("@FirstName", m_firstname)
                        .AddWithValue("@LastName", m_lastname)
                        .AddWithValue("@Title", m_title)
                        .AddWithValue("@Store", m_store)
                        .AddWithValue("@Matrix", m_blnMatrixfilled)
                        .AddWithValue("@ValidFrom", IIf(m_ValidFrom = String.Empty, DBNull.Value, m_ValidFrom))
                        .AddWithValue("@UrlRemoteLoginKey", CStr(IIf(m_strUrlRemoteLoginKey Is Nothing, "", m_strUrlRemoteLoginKey)))
                    End With

                    cmdUserHistory.ExecuteNonQuery()
                End If

                Dim cmdUser As New SqlClient.SqlCommand(strUserSQL, cn)
                With cmdUser.Parameters
                    .AddWithValue("@Username", m_strUsername)
                    .AddWithValue("@CustomerID", m_customer.CustomerId)
                    .AddWithValue("@CustomerAdmin", m_blnIsCustomerAdmin)
                    .AddWithValue("@TestUser", m_blnTestUser)
                    .AddWithValue("@PwdNeverExpires", m_blnPwdNeverExpires)
                    .AddWithValue("@FailedLogins", m_intFailedLogins)
                    .AddWithValue("@AccountIsLockedOut", m_blnAccountIsLockedOut)
                    .AddWithValue("@FirstLevelAdmin", m_blnFirstLevelAdmin)
                    .AddWithValue("@UserID", m_intUserId)
                    .AddWithValue("@UserHistoryID", m_intUserHistoryID)
                    If m_intUserId = -1 Then
                        .AddWithValue("@CreatedBy", m_strCreatedBy)
                    End If
                    .AddWithValue("@Reference", m_strReference)
                    .AddWithValue("@LoggedOn", m_blnLoggedOn)
                    .AddWithValue("@ReadMessageCount", m_intReadMessageCount)
                    .AddWithValue("@ReadCustomerMessageCount", m_intReadCustomerMessageCount)
                    .AddWithValue("@Approved", m_approved)
                    .AddWithValue("@FirstName", m_firstname)
                    .AddWithValue("@LastName", m_lastname)
                    .AddWithValue("@Title", m_title)
                    .AddWithValue("@Store", m_store)
                    .AddWithValue("@Matrix", m_blnMatrixfilled)
                    .AddWithValue("@ChangeUser", m_strCreatedBy)
                    .AddWithValue("@ValidFrom", IIf(m_ValidFrom = String.Empty, DBNull.Value, m_ValidFrom))
                    .AddWithValue("@UrlRemoteLoginKey", CStr(IIf(m_strUrlRemoteLoginKey Is Nothing, "", m_strUrlRemoteLoginKey)))
                End With

                If m_intUserId = -1 Then
                    'Wenn User neu ist dann ID ermitteln, damit bei nachfolgendem Fehler und erneutem Speichern Datensatz nicht doppelt angelegt wird.
                    m_intUserId = CInt(cmdUser.ExecuteScalar)
                    saveMail = True
                Else
                    saveMail = False
                    cmdUser.ExecuteNonQuery()
                End If
                SaveUserInfo(m_strCreatedBy)
                m_groups.Save(cn, m_strCreatedBy)
                If GetUserDataFromName(m_strUsername, cn) Then
                    Dim cmdUserHistory As SqlClient.SqlCommand
                    If intTemp = -1 Then
                        strUserHistorySQL = "UPDATE WebUserHistory SET " & _
                                                     "GroupName=@GroupName " & _
                                 "WHERE UserHistoryID=@UserHistoryID"

                        cmdUserHistory = New SqlClient.SqlCommand(strUserHistorySQL, cn)
                        With cmdUserHistory.Parameters
                            .AddWithValue("@GroupName", m_groups(0).GroupName)
                            .AddWithValue("@UserHistoryID", m_intUserHistoryID)
                        End With

                        cmdUserHistory.ExecuteNonQuery()
                    Else
                        Dim histUser As New HistoryUser(m_intUserHistoryID, cn)
                        If Not histUser.GroupName = m_groups(0).GroupName Then
                            cmdUserHistory = New SqlClient.SqlCommand("UPDATE WebUserHistory SET " & _
                                                                                "GroupName=@GroupName, " & _
                                                                                "LastChanged=@LastChanged, " & _
                                                                                "LastChange=@LastChange, " & _
                                                                                "LastChangedBy=@LastChangedBy " & _
                                                                                "WHERE UserHistoryID=@UserHistoryID", cn)
                            With cmdUserHistory.Parameters
                                .Clear()
                                .AddWithValue("@UserHistoryID", m_intUserHistoryID)
                                .AddWithValue("@GroupName", m_groups(0).GroupName)
                                .AddWithValue("@LastChanged", Now)
                                .AddWithValue("@LastChange", "Gruppe geändert")
                                .AddWithValue("@LastChangedBy", m_strCreatedBy)
                            End With

                            cmdUserHistory.ExecuteNonQuery()
                        End If
                    End If
                    Return True
                Else
                    Return False
                End If
            Catch ex As Exception
                Throw New Exception("Fehler beim Speichern der Benutzerdaten!", ex)
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try

        End Function

        Public Function IsSuperiorTo(ByVal _User As User) As Boolean
            If Me.HighestAdminLevel > _User.HighestAdminLevel Then
                Return True

            Else
                Return False
            End If
        End Function

        Public Sub SetLastLogin(ByVal dtmLastLogin As DateTime)
            SetLastLogin(dtmLastLogin, m_strConnectionstring)
        End Sub
        Public Sub SetLastLogin(ByVal dtmLastLogin As DateTime, ByVal strConnectionString As String)
            If strConnectionString = "" Then
                Throw New Exception("Fehler beim Setzen des Datums des letzen Logins: Kein ConnectionString vorhanden!")
                Exit Sub
            End If
            SetLastLogin(dtmLastLogin, New SqlClient.SqlConnection(strConnectionString))
        End Sub
        Public Sub SetLastLogin(ByVal dtmLastLogin As DateTime, ByVal cn As SqlClient.SqlConnection)
            Try
                If cn.State = ConnectionState.Closed Then
                    cn.Open()
                End If
                If (Not String.IsNullOrEmpty(m_groups(0).Message)) And (m_intReadMessageCount <= m_groups(0).MaxReadMessageCount) Then
                    m_intReadMessageCount += 1
                Else
                    m_groups(0).Message = ""
                    'wenn keine Gruppen-Meldung (mehr) -> prüfen, ob Kunden-Meldung
                    If (Not String.IsNullOrEmpty(m_customer.Message)) And (m_intReadCustomerMessageCount <= m_customer.MaxReadMessageCount) Then
                        m_intReadCustomerMessageCount += 1
                    Else
                        m_customer.Message = ""
                    End If
                End If

                Dim cmd As New SqlClient.SqlCommand("UPDATE WebUser SET LastLogin=@LastLogin, ReadMessageCount=@ReadMessageCount, ReadCustomerMessageCount=@ReadCustomerMessageCount WHERE UserID=@UserID", cn)
                With cmd.Parameters
                    .AddWithValue("@UserID", m_intUserId)
                    .AddWithValue("@LastLogin", dtmLastLogin.ToString)
                    .AddWithValue("@ReadMessageCount", m_intReadMessageCount)
                    .AddWithValue("@ReadCustomerMessageCount", m_intReadCustomerMessageCount)
                End With
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                Throw New Exception("Fehler beim Setzen des Datums des letzen Logins: " & ex.Message, ex)
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End Sub

        Public Function Approve(ByVal approvedBy As String) As Boolean
            m_strCreatedBy = approvedBy
            m_approved = True
            Save()
        End Function

        Private Function GetPasswordHistory(ByVal cn As SqlClient.SqlConnection) As Boolean
            Try

                Dim blnInitialPwd As Boolean = False
                Dim cmdHist As New SqlClient.SqlCommand("SELECT TOP 1 InitialPwd FROM WebUserPwdHistory WHERE UserID=@UserID ORDER BY DateOfChange DESC", cn)
                cmdHist.Parameters.AddWithValue("@UserID", m_intUserId)
                Return CBool(cmdHist.ExecuteScalar)


            Catch ex As Exception
                Throw New Exception("Fehler beim Schreiben der Passworthistorie!", ex)
            End Try
        End Function

        '''Dient der Übernahme der Kostenstellenauswahl bei der LZLD-Anmeldung
        Public Sub ChangeUserReference(ByVal newReference As String)
            m_strReference = newReference
        End Sub

#End Region


    End Class

    Public Enum AdminLevel
        Master = 4
        FirstLevel = 3
        Customer = 2
        Organization = 1
        None = 0
    End Enum
End Namespace

' ************************************************
' $History: User.vb $
' 
' *****************  Version 25  *****************
' User: Rudolpho     Date: 22.03.11   Time: 14:50
' Updated in $/CKAG/Base/Kernel/Security
' 
' *****************  Version 24  *****************
' User: Rudolpho     Date: 9.09.10    Time: 11:57
' Updated in $/CKAG/Base/Kernel/Security
' 
' *****************  Version 23  *****************
' User: Rudolpho     Date: 8.09.10    Time: 12:30
' Updated in $/CKAG/Base/Kernel/Security
' 
' *****************  Version 22  *****************
' User: Rudolpho     Date: 7.09.10    Time: 16:07
' Updated in $/CKAG/Base/Kernel/Security
' 
' *****************  Version 21  *****************
' User: Rudolpho     Date: 7.09.10    Time: 14:17
' Updated in $/CKAG/Base/Kernel/Security
' 
' *****************  Version 20  *****************
' User: Rudolpho     Date: 7.09.10    Time: 10:57
' Updated in $/CKAG/Base/Kernel/Security
' 
' *****************  Version 19  *****************
' User: Rudolpho     Date: 6.09.10    Time: 17:23
' Updated in $/CKAG/Base/Kernel/Security
' 
' *****************  Version 18  *****************
' User: Rudolpho     Date: 19.07.10   Time: 17:54
' Updated in $/CKAG/Base/Kernel/Security
' 
' *****************  Version 17  *****************
' User: Rudolpho     Date: 28.06.10   Time: 9:50
' Updated in $/CKAG/Base/Kernel/Security
' 
' *****************  Version 16  *****************
' User: Rudolpho     Date: 16.06.10   Time: 15:33
' Updated in $/CKAG/Base/Kernel/Security
' 
' *****************  Version 15  *****************
' User: Rudolpho     Date: 8.06.10    Time: 10:49
' Updated in $/CKAG/Base/Kernel/Security
' ITA: 3825
' 
' *****************  Version 14  *****************
' User: Rudolpho     Date: 7.06.10    Time: 11:22
' Updated in $/CKAG/Base/Kernel/Security
' ITA: 3824(HBr)
' 
' *****************  Version 13  *****************
' User: Fassbenders  Date: 31.03.10   Time: 16:58
' Updated in $/CKAG/Base/Kernel/Security
' 
' *****************  Version 12  *****************
' User: Rudolpho     Date: 7.10.09    Time: 14:52
' Updated in $/CKAG/Base/Kernel/Security
' 
' *****************  Version 11  *****************
' User: Rudolpho     Date: 1.09.09    Time: 13:19
' Updated in $/CKAG/Base/Kernel/Security
' 
' *****************  Version 10  *****************
' User: Fassbenders  Date: 17.04.09   Time: 16:59
' Updated in $/CKAG/Base/Kernel/Security
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 1.12.08    Time: 16:12
' Updated in $/CKAG/Base/Kernel/Security
' Bugfix: Doppelte Anmeldung nach Paßwortfrage
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 27.11.08   Time: 13:01
' Updated in $/CKAG/Base/Kernel/Security
' Bugfix: Doppelte Anmeldung nach Paßwortfrage
' 
' *****************  Version 7  *****************
' User: Hartmannu    Date: 11.09.08   Time: 17:47
' Updated in $/CKAG/Base/Kernel/Security
' 
' *****************  Version 6  *****************
' User: Hartmannu    Date: 10.09.08   Time: 17:28
' Updated in $/CKAG/Base/Kernel/Security
' ITA 2027 - Anzeige der erweiterten Benutzerhistorie
' 
' *****************  Version 5  *****************
' User: Hartmannu    Date: 10.09.08   Time: 9:31
' Updated in $/CKAG/Base/Kernel/Security
' ITA 2027 (Erweiterung Historie)
' 
' *****************  Version 4  *****************
' User: Hartmannu    Date: 10.09.08   Time: 9:07
' Updated in $/CKAG/Base/Kernel/Security
' 
' *****************  Version 3  *****************
' User: Hartmannu    Date: 9.09.08    Time: 13:42
' Updated in $/CKAG/Base/Kernel/Security
' ITA 2152 und 2158
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 11.04.08   Time: 11:57
' Updated in $/CKAG/Base/Kernel/Security
' Migration OR
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 3.04.08    Time: 16:42
' Created in $/CKAG/Base/Kernel/Security
' 
' *****************  Version 32  *****************
' User: Rudolpho     Date: 5.02.08    Time: 13:46
' Updated in $/CKG/Base/Base/Kernel/Security
' ORU 20080205: von UHa einfügte Abfrage für Testzwecke???  entfernt
' 
' *****************  Version 31  *****************
' User: Uha          Date: 21.01.08   Time: 18:09
' Updated in $/CKG/Base/Base/Kernel/Security
' ITA 1644: Ermöglicht Login nur mit IP und festgelegtem Benutzer
' 
' *****************  Version 30  *****************
' User: Uha          Date: 8.01.08    Time: 11:44
' Updated in $/CKG/Base/Base/Kernel/Security
' ITA 1508: Aufruf von SetLoggedOn auch für Benutzer, deren Firmen
' Mehrfachlogin erlauben
' 
' *****************  Version 29  *****************
' User: Rudolpho     Date: 6.12.07    Time: 12:47
' Updated in $/CKG/Base/Base/Kernel/Security
' ITA:1440
' 
' *****************  Version 28  *****************
' User: Jungj        Date: 12.11.07   Time: 14:56
' Updated in $/CKG/Base/Base/Kernel/Security
' 
' *****************  Version 27  *****************
' User: Rudolpho     Date: 2.11.07    Time: 15:16
' Updated in $/CKG/Base/Base/Kernel/Security
' Loginfehler(Sprung auf leere Loginseite) nach Falschanmeldung behoben!
' 
' *****************  Version 26  *****************
' User: Uha          Date: 27.09.07   Time: 10:27
' Updated in $/CKG/Base/Base/Kernel/Security
' Controlnamen für Feldübersetzungen geändert
' 
' *****************  Version 25  *****************
' User: Fassbenders  Date: 20.09.07   Time: 15:22
' Updated in $/CKG/Base/Base/Kernel/Security
' 
' *****************  Version 24  *****************
' User: Uha          Date: 19.09.07   Time: 13:19
' Updated in $/CKG/Base/Base/Kernel/Security
'  ITA 1261: Testfähige Version
' 
' *****************  Version 23  *****************
' User: Uha          Date: 10.09.07   Time: 13:23
' Updated in $/CKG/Base/Base/Kernel/Security
' ITA 1280: Bugfix III; ITA 1263: Testversion Translation
' 
' *****************  Version 22  *****************
' User: Uha          Date: 30.08.07   Time: 18:44
' Updated in $/CKG/Base/Base/Kernel/Security
' ITA 1280: Bugfix II
' 
' *****************  Version 21  *****************
' User: Uha          Date: 30.08.07   Time: 15:17
' Updated in $/CKG/Base/Base/Kernel/Security
' ITA 1280: Bugfix
' 
' *****************  Version 20  *****************
' User: Uha          Date: 30.08.07   Time: 12:36
' Updated in $/CKG/Base/Base/Kernel/Security
' ITA 1280: Paßwortversand im Web auf Benutzerwunsch
' 
' *****************  Version 19  *****************
' User: Uha          Date: 7.08.07    Time: 14:23
' Updated in $/CKG/Base/Base/Kernel/Security
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 18  *****************
' User: Uha          Date: 2.07.07    Time: 15:39
' Updated in $/CKG/Base/Base/Kernel/Security
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 17  *****************
' User: Uha          Date: 31.05.07   Time: 11:41
' Updated in $/CKG/Base/Base/Kernel/Security
' ITA 1077 - Login bei bereits aktiver Anmeldung ermöglichen
' 
' *****************  Version 16  *****************
' User: Uha          Date: 15.05.07   Time: 15:29
' Updated in $/CKG/Base/Base/Kernel/Security
' Änderungen aus StartApplication vom 11.05.2007
' 
' *****************  Version 15  *****************
' User: Uha          Date: 3.05.07    Time: 12:35
' Updated in $/CKG/Base/Base/Kernel/Security
' 
' ************************************************
