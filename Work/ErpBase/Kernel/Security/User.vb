Imports System.Configuration
Imports System.Web
Imports GeneralTools.Models
Imports GeneralTools.Services
Imports WebTools.Services


Namespace Kernel.Security
    <Serializable()> Public Class User
        REM § Enthält Daten eines einzelnen Benutzers.

        Public Enum PasswordMailMode
            Neu
            Zuruecksetzen
        End Enum

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
        Private m_strReference2 As String
        Private m_strReference3 As String
        Private m_blnReference4 As Boolean
        Private m_blnLoggedOn As Boolean
        Private m_blnMatrixfilled As Boolean
        Private m_blnPasswordExpired As Boolean = False
        Private m_blnInitialPassword As Boolean = False
        Private m_groups As Groups
        Private m_intGroupID As Int32
        <NonSerialized()> Private m_blnFirstLevelAdmin As Boolean

        Private m_intReadMessageCount As Int32

        Private m_tblApplications As New DataTable()
        Private m_strErrorMessage As String
        Private m_app As App
        Private m_customer As Customer
        Private m_organization As Organization
        Private m_dtmLastLogin As DateTime
        Private m_mail As String = ""  'Emailadresse
        Private m_telephone As String = ""
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
        Private m_ValidTo As String
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
                       ByVal strReference2 As String, _
                       ByVal strReference3 As String, _
                       ByVal blnReference4 As Boolean, _
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
                       ByVal strValidTo As String)

            m_blnDoubleLoginTry = False
            m_intUserId = intUserId
            m_strUsername = strUserName
            m_strReference = strReference
            m_strReference2 = strReference2
            m_strReference3 = strReference3
            m_blnReference4 = blnReference4
            m_blnTestUser = blnTestUser
            m_blnIsCustomerAdmin = blnCustomerAdmin
            m_blnPwdNeverExpires = blnPwdNeverExpires
            m_blnAccountIsLockedOut = blnAccountIsLockedOut
            m_blnFirstLevelAdmin = blnFirstLevelAdmin
            m_blnLoggedOn = blnLoggedOn
            m_intReadMessageCount = intReadMessageCount
            m_strConnectionstring = strConnectionString
            'Customer
            m_customer = New Customer(intCustomerId, m_strConnectionstring)
            'Gruppe
            m_groups = New Groups(Me, m_strConnectionstring)
            'Organization
            m_organization = New Organization(-1, m_strConnectionstring, intUserId)
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
            m_ValidTo = strValidTo
        End Sub

        Public Sub New( _
                       ByVal intUserId As Integer, _
                       ByVal strUserName As String, _
                       ByVal strReference As String, _
                       ByVal strReference2 As String, _
                       ByVal strReference3 As String, _
                       ByVal blnReference4 As Boolean, _
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
                       ByVal strValidTo As String)

            m_blnDoubleLoginTry = False
            m_intUserId = intUserId
            m_strUsername = strUserName
            m_strReference = strReference
            m_strReference2 = strReference2
            m_strReference3 = strReference3
            m_blnReference4 = blnReference4
            m_blnTestUser = blnTestUser
            m_blnIsCustomerAdmin = blnCustomerAdmin
            m_blnPwdNeverExpires = blnPwdNeverExpires
            m_blnAccountIsLockedOut = blnAccountIsLockedOut
            m_blnFirstLevelAdmin = blnFirstLevelAdmin
            m_blnLoggedOn = blnLoggedOn
            m_intReadMessageCount = intReadMessageCount
            m_strConnectionstring = strConnectionString
            'Customer
            m_customer = New Customer(intCustomerId, m_strConnectionstring)
            'Gruppe
            m_groups = New Groups(Me, m_strConnectionstring)
            'Organization
            m_organization = New Organization(-1, m_strConnectionstring, intUserId)
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
            m_ValidTo = strValidTo
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

        Public Property CurrentLogAccessASPXID() As Integer
            Get
                Return m_intCurrentLogAccessASPXID
            End Get
            Set(ByVal Value As Integer)
                m_intCurrentLogAccessASPXID = Value
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

        Public Property ReadMessageCount() As Int32
            Get
                Return m_intReadMessageCount
            End Get
            Set(ByVal Value As Int32)
                m_intReadMessageCount = Value
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

        Public ReadOnly Property Reference2() As String
            Get
                Return m_strReference2
            End Get
        End Property

        Public ReadOnly Property Reference3() As String
            Get
                Return m_strReference3
            End Get
        End Property

        Public ReadOnly Property Reference4() As Boolean
            Get
                Return m_blnReference4
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

        Public Property ValidTo() As String
            Get
                Return m_ValidTo
            End Get
            Set(value As String)
                m_ValidTo = value
            End Set
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

        Public Sub SetLoggedOn(ByVal strUserName As String, ByVal LogonValue As Boolean)
            Dim cn As SqlClient.SqlConnection = New SqlClient.SqlConnection(m_strConnectionstring)
            Try
                cn.Open()
                Dim cmdSetLogins As New SqlClient.SqlCommand("UPDATE WebUser " & _
                                                             "SET LoggedOn=@LoggedOn " & _
                                                             "WHERE Username=@Username", cn)
                cmdSetLogins.Parameters.AddWithValue("@Username", strUserName)
                cmdSetLogins.Parameters.AddWithValue("@LoggedOn", LogonValue)
                cmdSetLogins.ExecuteNonQuery()
                m_blnLoggedOn = LogonValue
            Catch ex As Exception
                Throw New Exception("Login-Status des Benutzers konnte nicht geändert werden.", ex)
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

        Private Sub UnlockAccount(ByVal strUserName As String, ByVal cn As SqlClient.SqlConnection, ByVal strChangeUser As String)
            Try
                If cn.State = ConnectionState.Closed Then
                    cn.Open()
                End If
                Dim cmdSetLogins As New SqlClient.SqlCommand("UPDATE WebUser " & _
                                                             "SET FailedLogins=@FailedLogins, " & _
                                                                 "AccountIsLockedOut=@AccountIsLockedOut, " & _
                                                                 "LastChangedBy=@ChangeUser " & _
                                                             "WHERE Username=@Username", cn)
                cmdSetLogins.Parameters.AddWithValue("@FailedLogins", 0)
                cmdSetLogins.Parameters.AddWithValue("@AccountIsLockedOut", False)
                cmdSetLogins.Parameters.AddWithValue("@ChangeUser", strChangeUser)
                cmdSetLogins.Parameters.AddWithValue("@Username", strUserName)
                cmdSetLogins.ExecuteNonQuery()
                cmdSetLogins = New SqlClient.SqlCommand("UPDATE WebUserHistory " & _
                                                             "SET FailedLogins=@FailedLogins, " & _
                                                                 "AccountIsLockedOut=@AccountIsLockedOut," & _
                                                                 "LastChanged=Getdate()," & _
                                                                 "LastChange='Benutzer entsperrt'," & _
                                                                 "LastChangedBy=@ChangeUser " & _
                                                             "WHERE UserHistoryID=@UserHistoryID", cn)
                cmdSetLogins.Parameters.AddWithValue("@FailedLogins", 0)
                cmdSetLogins.Parameters.AddWithValue("@AccountIsLockedOut", False)
                cmdSetLogins.Parameters.AddWithValue("@ChangeUser", strChangeUser)
                cmdSetLogins.Parameters.AddWithValue("@UserHistoryID", m_intUserHistoryID)
                cmdSetLogins.ExecuteNonQuery()
            Catch ex As Exception
                Throw New Exception("Konto konnte nicht entsperrt werden!", ex)
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

        Public Function Login(ByVal strUsername As String, ByVal strSessionID As String, ByVal strUrl As String) As Boolean
            Dim blnReturn As Boolean
            Dim cn As New SqlClient.SqlConnection(m_strConnectionstring)

            Try

                'Eingabe-Parameter ueberpruefen
                If Not (Len(strUsername) > 0) Then
                    Throw New Exception("Keine gültigen Anmeldedaten eingegeben!")
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
                                                    "Reference2, " & _
                                                    "Reference3, " & _
                                                    "Reference4, " & _
                                                    "LoggedOn, " & _
                                                    "LastLogin, " & _
                                                    "ReadMessageCount, " & _
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
                                                    "ValidTo, " & _
                                                    "UrlRemoteLoginKey " & _
                                                    "FROM vwWebUser " & _
                                                    "WHERE Username = @Username ", cn)
                With cmdUser.Parameters
                    .AddWithValue("@Username", strUsername)
                End With
                blnReturn = GetUserData(cmdUser, cn)

                If Not blnReturn Then
                    Throw New Exception(m_strErrorMessage)
                End If

                'Anmeldestatus pruefen
                If m_intUserId = -1 Then
                    If GetUserDataFromName(strUsername, cn) Then
                        ' FailedLogin-Zähler nur dann verwenden, wenn Account nicht bereits gesperrt
                        If m_intFailedLogins < m_customer.CustomerLoginRules.LockedAfterNLogins AndAlso Not m_blnAccountIsLockedOut Then
                            SetFailedLogins(strUsername, m_intFailedLogins + 1, cn, strUsername)
                            m_intFailedLogins += 1
                            If m_mail.Length > 0 And m_customer.ForcePasswordQuestion And m_intQuestionID > -1 Then
                                Throw New Exception("4174")
                            Else
                                Throw New Exception("")
                            End If
                        Else
                            If m_blnAccountIsLockedOut Then
                                Dim sLockedBy As String = CStr(GetHistoryInfos(Me))
                                If sLockedBy.ToLower = UserName.ToLower Then
                                    m_strAccountIsLockedBy = "User"
                                ElseIf sLockedBy.ToLower = "[admin-regelprozess]" Then
                                    m_strAccountIsLockedBy = "Regelprozess"
                                Else
                                    m_strAccountIsLockedBy = "Admin"
                                End If
                            Else
                                LockOutAccount(strUsername, cn, strUsername)
                                m_blnAccountIsLockedOut = True
                                m_strAccountIsLockedBy = "Now"
                            End If
                            Throw New Exception("Das Passwort wurde mehrfach falsch eingegeben. Das Benutzerkonto wurde gesperrt. Bitte setzen Sie sich mit Ihrem Administrator in Verbindung!")
                        End If
                    Else
                        Throw New Exception("")
                    End If
                Else
                    GetEmail(m_intUserId, blnReturn)
                End If

                'Auf Freigabe des Accounts prüfen
                If Not m_approved Then
                    Throw New Exception("Das Benutzerkonto ist noch nicht freigegeben. Bitte setzen Sie Sich mit Ihrem Administrator in Verbindung!")
                End If

                'Auf Sperrung des Accounts pruefen
                If m_blnAccountIsLockedOut Then
                    Dim sLockedBy As String = CStr(GetHistoryInfos(Me))
                    If sLockedBy.ToLower = UserName.ToLower Then
                        m_strAccountIsLockedBy = "User"
                    ElseIf sLockedBy.ToLower = "[admin-regelprozess]" Then
                        m_strAccountIsLockedBy = "Regelprozess"
                    Else
                        m_strAccountIsLockedBy = "Admin"
                    End If
                    Throw New Exception("Das Benutzerkonto ist gesperrt. Bitte setzen Sie sich mit Ihrem Administrator in Verbindung!")
                End If

                'Gibt es eine Datumsbeschränkung
                If Not m_ValidFrom Is DBNull.Value Then
                    If m_ValidFrom.Length > 0 AndAlso IsDate(m_ValidFrom) = True Then
                        If CDate(m_ValidFrom) >= Date.Now Then
                            Throw New Exception("Das Benutzerkonto steht Ihnen ab " & CDate(m_ValidFrom).ToShortDateString & " zur Verfügung.")
                        End If
                    End If
                End If

                'Ggf. prüfen, ob User den richtigen Portal-Link benutzt
                If m_customer.ForceSpecifiedLoginLink Then
                    Dim specifiedLoginLink As String = m_customer.LoginLink.ToLower().Replace("http://", "").Replace("https://", "")
                    If specifiedLoginLink.Contains("/"c) Then
                        specifiedLoginLink = specifiedLoginLink.Split("/"c)(0)
                    End If

                    Dim currentLoginLink As String = strUrl.ToLower().Replace("http://", "").Replace("https://", "")
                    If currentLoginLink.Contains("/"c) Then
                        currentLoginLink = currentLoginLink.Split("/"c)(0)
                    End If

                    If Not String.IsNullOrEmpty(currentLoginLink) AndAlso Not currentLoginLink.StartsWith("localhost") AndAlso specifiedLoginLink <> currentLoginLink Then
                        Throw New Exception("Bitte melden Sie sich künftig über die Adresse https://" & specifiedLoginLink & " im Portal an.<br/>Sie erreichen die Seite auch über die DAD Homepage www.dad.de => Kunden-Login")
                    End If
                End If

                'Logonstatus des Accounts pruefen und setzen
                If Not m_customer.AllowMultipleLogin Then
                    If m_blnLoggedOn Then
                        m_blnDoubleLoginTry = True
                        'Throw New Exception("Der Benutzer ist bereits angemeldet. Mehrfache Anmeldungen sind nicht gestattet.")
                    Else
                        SetLoggedOn(strUsername, True)
                    End If
                Else
                    SetLoggedOn(strUsername, True)
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
                              ByVal strSessionID As String, ByVal strUrl As String, Optional ByVal blnPasswdlink As Boolean = False) As Boolean
            Dim blnReturn As Boolean
            Dim cn As New SqlClient.SqlConnection(m_strConnectionstring)

            Try

                'Eingabe-Parameter ueberpruefen
                If Not (Len(strUsername) > 0 And Len(strPassword) > 0) Then
                    Throw New Exception("Keine gültigen Anmeldedaten eingegeben!")
                End If
                'Passwort mit Leerzeichen
                If strPassword.Length <> strPassword.Trim.Length Then
                    Throw New Exception("Überprüfen Sie Ihr Passwort! Leerzeichen nicht erlaubt!")
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
                                                    "Reference2, " & _
                                                    "Reference3, " & _
                                                    "Reference4, " & _
                                                    "LoggedOn, " & _
                                                    "LastLogin, " & _
                                                    "ReadMessageCount, " & _
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
                                                    "ValidTo, " & _
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
                '    Throw New Exception(m_strErrorMessage)
                'End If

                'Anmeldestatus pruefen
                If m_intUserId = -1 Then
                    If GetUserDataFromName(strUsername, cn, blnPasswdlink) Then
                        ' FailedLogin-Zähler nur dann verwenden, wenn Account nicht bereits gesperrt
                        If m_intFailedLogins < m_customer.CustomerLoginRules.LockedAfterNLogins AndAlso Not m_blnAccountIsLockedOut Then
                            SetFailedLogins(strUsername, m_intFailedLogins + 1, cn, strUsername)
                            m_intFailedLogins += 1
                            If m_mail.Length > 0 And m_customer.ForcePasswordQuestion And m_intQuestionID > -1 Then
                                Throw New Exception("4174")
                            Else
                                Throw New Exception("9999")
                            End If
                        Else
                            If m_blnAccountIsLockedOut Then
                                Dim sLockedBy As String = CStr(GetHistoryInfos(Me))
                                If sLockedBy.ToLower = UserName.ToLower Then
                                    m_strAccountIsLockedBy = "User"
                                ElseIf sLockedBy.ToLower = "[admin-regelprozess]" Then
                                    m_strAccountIsLockedBy = "Regelprozess"
                                Else
                                    m_strAccountIsLockedBy = "Admin"
                                End If
                            Else
                                LockOutAccount(strUsername, cn, strUsername)
                                m_blnAccountIsLockedOut = True
                                m_strAccountIsLockedBy = "Now"
                            End If
                            Throw New Exception("Das Passwort wurde mehrfach falsch eingegeben. Das Benutzerkonto wurde gesperrt. Bitte setzen Sie sich mit Ihrem Administrator in Verbindung!")

                        End If
                    Else
                        Throw New Exception("")
                    End If
                Else
                    GetEmail(m_intUserId, blnReturn)
                End If

                'Auf Freigabe des Accounts prüfen
                If Not m_approved Then
                    Throw New Exception("Das Benutzerkonto ist noch nicht freigegeben. Bitte setzen Sie Sich mit Ihrem Administrator in Verbindung!")
                End If

                'Auf Sperrung des Accounts pruefen
                If m_blnAccountIsLockedOut Then
                    Dim sLockedBy As String = CStr(GetHistoryInfos(Me))
                    If sLockedBy.ToLower = UserName.ToLower Then
                        m_strAccountIsLockedBy = "User"
                    ElseIf sLockedBy.ToLower = "[admin-regelprozess]" Then
                        m_strAccountIsLockedBy = "Regelprozess"
                    Else
                        m_strAccountIsLockedBy = "Admin"
                    End If
                    Throw New Exception("Das Benutzerkonto ist gesperrt. Bitte setzen Sie sich mit Ihrem Administrator in Verbindung!")
                End If

                'Gibt es eine Datumsbeschränkung
                If Not m_ValidFrom Is DBNull.Value Then
                    If m_ValidFrom.Length > 0 AndAlso IsDate(m_ValidFrom) = True Then
                        If CDate(m_ValidFrom) >= Date.Now Then
                            Throw New Exception("Das Benutzerkonto steht Ihnen erst ab " & CDate(m_ValidFrom).ToShortDateString & " zur Verfügung.")
                        End If
                    End If
                End If

                'Ggf. prüfen, ob User den richtigen Portal-Link benutzt
                If m_customer.ForceSpecifiedLoginLink Then
                    Dim specifiedLoginLink As String = m_customer.LoginLink.ToLower().Replace("http://", "").Replace("https://", "")
                    If specifiedLoginLink.Contains("/"c) Then
                        specifiedLoginLink = specifiedLoginLink.Split("/"c)(0)
                    End If

                    Dim currentLoginLink As String = strUrl.ToLower().Replace("http://", "").Replace("https://", "")
                    If currentLoginLink.Contains("/"c) Then
                        currentLoginLink = currentLoginLink.Split("/"c)(0)
                    End If

                    If Not String.IsNullOrEmpty(currentLoginLink) AndAlso Not currentLoginLink.StartsWith("localhost") AndAlso specifiedLoginLink <> currentLoginLink Then
                        Throw New Exception("Bitte melden Sie sich künftig über die Adresse https://" & specifiedLoginLink & " im Portal an.<br/>Sie erreichen die Seite auch über die DAD Homepage www.dad.de => Kunden-Login")
                    End If
                End If

                'Logonstatus des Accounts pruefen und setzen
                If Not m_customer.AllowMultipleLogin Then
                    If m_blnLoggedOn Then
                        m_blnDoubleLoginTry = True
                        'Throw New Exception("Der Benutzer ist bereits angemeldet. Mehrfache Anmeldungen sind nicht gestattet.")
                    Else
                        SetLoggedOn(strUsername, True)
                    End If
                Else
                    SetLoggedOn(strUsername, True)
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

        Private Function GetHistoryInfos(ByVal objUser As Kernel.Security.User) As String

            Dim cn As New SqlClient.SqlConnection(m_strConnectionstring)
            Try
                cn.Open()
                If cn.State = ConnectionState.Closed Then
                    cn.Open()
                End If
                Dim cmd As SqlClient.SqlCommand = New SqlClient.SqlCommand("SELECT LastChangedBy FROM AdminHistory_User " & _
                    "WHERE ID = (SELECT MAX(ID) FROM AdminHistory_User WHERE Username = @Username AND Action = 'Benutzer gesperrt')", cn)

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

                    If Not SendPasswordResetMail(m_strErrorMessage, PasswordMailMode.Zuruecksetzen) Then
                        Throw New Exception(m_strErrorMessage)
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
                        Throw New Exception("Die Daten wurden mehrfach falsch eingegeben. Das Benutzerkonto wurde gesperrt. Bitte setzen Sie sich mit Ihrem Administrator in Verbindung!")
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
                    Throw New Exception("Kein gültiger Benutzername!")
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
                                                    "Reference2, " & _
                                                    "Reference3, " & _
                                                    "Reference4, " & _
                                                    "LoggedOn, " & _
                                                    "LastLogin, " & _
                                                    "ReadMessageCount, " & _
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
                                                    "ValidTo, " & _
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
                    Throw New Exception("Keine gültige Benutzer-ID!")
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
                                                    "Reference2, " & _
                                                    "Reference3, " & _
                                                    "Reference4, " & _
                                                    "LoggedOn, " & _
                                                    "LastLogin, " & _
                                                    "ReadMessageCount, " & _
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
                                                    "ValidTo, " & _
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
                        m_strReference2 = ""
                        If Not TypeOf drUser("Reference2") Is System.DBNull Then
                            m_strReference2 = CStr(drUser("Reference2"))
                        End If
                        m_strReference3 = ""
                        If Not TypeOf drUser("Reference3") Is System.DBNull Then
                            m_strReference3 = CStr(drUser("Reference3"))
                        End If
                        m_blnReference4 = False
                        If Not TypeOf drUser("Reference4") Is System.DBNull Then
                            m_blnReference4 = CBool(drUser("Reference4"))
                        End If
                        m_blnLoggedOn = CBool(drUser("LoggedOn"))
                        If Not drUser("LastLogin") Is System.DBNull.Value Then
                            m_dtmLastLogin = CDate(drUser("LastLogin"))
                        End If
                        m_intReadMessageCount = CInt(drUser("ReadMessageCount"))
                        m_approved = CBool(drUser("Approved"))
                        m_firstname = drUser("FirstName").ToString
                        m_lastname = drUser("LastName").ToString
                        m_title = drUser("Title").ToString
                        m_store = drUser("Store").ToString
                        m_ValidFrom = drUser("ValidFrom").ToString
                        If Not String.IsNullOrEmpty(m_ValidFrom) Then m_ValidFrom = m_ValidFrom.Replace(" 00:00:00", "")
                        m_ValidTo = drUser("ValidTo").ToString
                        If Not String.IsNullOrEmpty(m_ValidTo) Then m_ValidTo = m_ValidTo.Replace(" 00:00:00", "")
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
                    End If

                    'App instanziieren
                    m_app = New App(Me)

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
                                                          "WHERE UserID = @UserID", cn)
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

        Public Function SendPasswordResetMail(ByRef errMsg As String, ByVal modus As PasswordMailMode) As Boolean

            If Not m_customer.CustomerUsernameRules.DontSendEmail Then
                If String.IsNullOrEmpty(m_mail) Then
                    errMsg = "Keine Mailadresse angegeben. Link konnte nicht versendet werden."
                    Return False
                End If

                Dim confirmationToken As String = UserSecurityService.GenerateToken(UserName)
                UpdateWebUserPasswordChangeRequestKey(confirmationToken)

                Dim portalLink As String = LoadLoginLinks(m_customer.LoginLinkID)
                If portalLink = String.Empty Then
                    Throw New Exception("Kein Login-Link konfiguriert!")
                End If
                'Pwd-Änderung + Anmeldung erfolgt immer über das ServicesMvc-Login
                portalLink = portalLink.ToLower()
                portalLink = portalLink.Replace("/start/login.aspx", "/")
                portalLink = portalLink.Replace("/portal/", "/servicesmvc/")
                portalLink = portalLink.Replace("/services/", "/servicesmvc/")

                Dim controller As String = "Login"
                Dim action As String = "ChangePassword"
                Dim confirmationUrl As String = String.Format("{0}{1}/{2}?confirmation={3}", portalLink, controller, action, HttpUtility.UrlEncode(confirmationToken))

                Dim verb As String = IIf(modus = PasswordMailMode.Neu, "Generieren", "Zurücksetzen").ToString()

                Dim smtpMailServer As String = ConfigurationManager.AppSettings("SmtpMailServer")
                Dim smtpMailSender As String = ConfigurationManager.AppSettings("SmtpMailSender")
                Dim subject As String = String.Format("Link zum {0} Ihres Passworts", verb)
                Dim userSalutation As String = String.Format("{0} {1}", Title, LastName)
                Dim companyName As String = GeneralConfiguration.GetConfigValue("Global", "AppOwnerFullName")
                Dim textBuilder As New Text.StringBuilder()
                With textBuilder
                    .AppendLine(String.Format("Guten Tag {0},", userSalutation))
                    .Append(Environment.NewLine)
                    .Append(Environment.NewLine)
                    .AppendLine(String.Format("bitte nutzen Sie diesen Link zum {0} Ihres Passworts:", verb))
                    .Append(Environment.NewLine)
                    .AppendLine(confirmationUrl)
                    .Append(Environment.NewLine)
                    .Append(Environment.NewLine)
                    .AppendLine("Mit freundlichen Grüßen")
                    .AppendLine(companyName)
                End With

                Dim client As New System.Net.Mail.SmtpClient(smtpMailServer)
                client.Send(smtpMailSender, m_mail, subject, textBuilder.ToString)
            End If

            Return True

        End Function

        Public Function SendUsernameMail(ByRef errorMsg As String, ByVal Reapproved As Boolean) As Boolean
            Try
                If Not m_customer.CustomerUsernameRules.DontSendEmail Then
                    If (m_mail <> String.Empty) Then
                        Dim smtpMailServer As String = ConfigurationManager.AppSettings("SmtpMailServer")
                        Dim smtpMailSender As String = ConfigurationManager.AppSettings("SmtpMailSender")
                        Dim subject As String = "Ihr persönlicher Benutzername"
                        Dim companyName As String = GeneralConfiguration.GetConfigValue("Global", "AppOwnerFullName")
                        Dim textBuilder As New Text.StringBuilder()

                        With textBuilder
                            .AppendLine("Guten Tag,")
                            .Append(Environment.NewLine)
                            .AppendLine("Ihr neuer persönlicher Benutzername für den Internetbereich der Kroschke Gruppe und dem Deutschen Auto Dienst lautet:")
                            .Append(Environment.NewLine)
                            .AppendLine(m_strUsername.Trim())
                            .AppendLine("(Bitte achten Sie auf die korrekte Groß- und Kleinschreibung)")
                            .Append(Environment.NewLine)
                            .AppendLine("Mit diesem Benutzernamen sowie Ihrem persönlichen Passwort können Sie das Login vornehmen.")
                            .Append(Environment.NewLine)
                            .AppendLine("Hinweis!: Den Link zum Generieren Ihres persönlichen Passworts erhalten Sie in einer separaten E-mail.")
                            .Append(Environment.NewLine)
                            .AppendLine("Mit freundlichen Grüßen")
                            .AppendLine(companyName)

                        End With

                        Dim client As New System.Net.Mail.SmtpClient(smtpMailServer)
                        client.Send(smtpMailSender, m_mail, subject, textBuilder.ToString)

                        Return True
                    Else
                        errorMsg = "Keine Mailadresse angegeben. Der Benutzername konnte nicht versendet werden."
                        Return False
                    End If
                End If
            Catch ex As Exception
                errorMsg = String.Format("Beim Mailversand ist ein technisches Problem aufgetreten. Die Email wurde möglicherweise nicht verschickt. (Wortlaut der Fehlermeldung: {0})", ex.Message)
                Return False
            End Try
        End Function

        Public Function SendUsernameChangedMail(ByRef errorMsg As String) As Boolean
            Try
                If Not m_customer.CustomerUsernameRules.DontSendEmail Then
                    If (m_mail <> String.Empty) Then
                        Dim smtpMailServer As String = ConfigurationManager.AppSettings("SmtpMailServer")
                        Dim smtpMailSender As String = ConfigurationManager.AppSettings("SmtpMailSender")
                        Dim subject As String = "Ihr Benutzername wurde geändert"
                        Dim companyName As String = GeneralConfiguration.GetConfigValue("Global", "AppOwnerFullName")
                        Dim textBuilder As New Text.StringBuilder()
                        With textBuilder
                            .AppendLine("Guten Tag,")
                            .Append(Environment.NewLine)
                            .AppendLine("Ihr persönlicher Benutzername für den Internetbereich der Kroschke Gruppe und dem Deutschen Auto Dienst hat sich geändert.")
                            .AppendLine("Ihr neuer Benutzername lautet:")
                            .Append(Environment.NewLine)
                            .AppendLine(m_strUsername.Trim())
                            .AppendLine("(Bitte achten Sie auf die korrekte Groß- und Kleinschreibung)")
                            .Append(Environment.NewLine)
                            .AppendLine("Mit diesem Benutzernamen sowie Ihrem persönlichen Passwort können Sie das Login vornehmen.")
                            .Append(Environment.NewLine)
                            .AppendLine("Hinweis!: Den Link zum Generieren Ihres persönlichen Passworts erhalten Sie in einer separaten E-mail.")
                            .Append(Environment.NewLine)
                            .AppendLine("Mit freundlichen Grüßen")
                            .AppendLine(companyName)
                        End With

                        Dim client As New System.Net.Mail.SmtpClient(smtpMailServer)
                        client.Send(smtpMailSender, m_mail, subject, textBuilder.ToString)

                        Return True
                    Else
                        errorMsg = "Keine Mailadresse angegeben. Der Benutzername konnte nicht versendet werden."
                        Return False
                    End If
                End If
            Catch ex As Exception
                errorMsg = String.Format("Beim Mailversand ist ein technisches Problem aufgetreten. Die Email wurde möglicherweise nicht verschickt. (Wortlaut der Fehlermeldung: {0})", ex.Message)
                Return False
            End Try
        End Function

        Public Function SendUsernameMail(ByRef errorMsg As String) As Boolean
            Try
                If Not m_customer.CustomerUsernameRules.DontSendEmail Then

                    If String.IsNullOrEmpty(m_mail) Then
                        errorMsg = "Keine Mailadresse angegeben. Link konnte nicht versendet werden."
                        Return False
                    End If

                    Dim confirmationToken As String = UserSecurityService.GenerateToken(UserName)
                    UpdateWebUserPasswordChangeRequestKey(confirmationToken)

                    Dim portalLinkOrig As String = LoadLoginLinks(m_customer.LoginLinkID)
                    If portalLinkOrig = String.Empty Then
                        Throw New Exception("Kein Login-Link konfiguriert!")
                    End If
                    'Pwd-Änderung + Anmeldung erfolgt immer über das ServicesMvc-Login
                    Dim portalLink As String = portalLinkOrig.ToLower()
                    portalLink = portalLink.Replace("/start/login.aspx", "/")
                    portalLink = portalLink.Replace("/portal/", "/servicesmvc/")
                    portalLink = portalLink.Replace("/services/", "/servicesmvc/")

                    Dim controller As String = "Login"
                    Dim action As String = "ChangePassword"
                    Dim confirmationUrl As String = String.Format("{0}{1}/{2}?confirmation={3}", portalLink, controller, action, HttpUtility.UrlEncode(confirmationToken))

                    Dim smtpMailServer As String = ConfigurationManager.AppSettings("SmtpMailServer")
                    Dim smtpMailSender As String = ConfigurationManager.AppSettings("SmtpMailSender")
                    Dim subject As String = "Ihr persönlicher Benutzername"
                    Dim userSalutation As String = String.Format("{0} {1}", Title, LastName)
                    Dim companyName As String = GeneralConfiguration.GetConfigValue("Global", "AppOwnerFullName")
                    Dim textBuilder As New Text.StringBuilder()
                    With textBuilder
                        .AppendLine(String.Format("Guten Tag {0},", userSalutation))
                        .Append(Environment.NewLine)
                        .Append(Environment.NewLine)
                        .AppendLine("mit dieser Email übersenden wir Ihnen die Zugangsdaten zum Internetbereich der Kroschke Gruppe und dem Deutschen Auto Dienst GmbH. ")
                        .Append(Environment.NewLine)
                        .AppendLine("Bitte speichern Sie sich folgenden Link in Ihrem Internetbrowser ab:")
                        .Append(Environment.NewLine)
                        .AppendLine(portalLinkOrig)
                        .Append(Environment.NewLine)
                        .AppendLine("Ihr Benutzername lautet:")
                        .AppendLine(m_strUsername.Trim)
                        .Append(Environment.NewLine)
                        .AppendFormat("Sofern Sie der korrekte Empfänger ({0} {1}) dieser Mail sind, klicken Sie bitte auf den folgenden Link, um Ihre Benutzerdaten zu bestätigen und ein neues Passwort zu generieren.", FirstName, LastName)
                        .Append(Environment.NewLine)
                        .AppendLine(confirmationUrl)
                        .Append(Environment.NewLine)
                        .Append(Environment.NewLine)
                        .AppendLine("Mit freundlichen Grüßen")
                        .AppendLine(companyName)
                    End With

                    Dim client As New System.Net.Mail.SmtpClient(smtpMailServer)
                    client.Send(smtpMailSender, m_mail, subject, textBuilder.ToString)

                    Return True
                End If
            Catch ex As Exception
                errorMsg = String.Format("Beim Mailversand ist ein technisches Problem aufgetreten. Die Email wurde möglicherweise nicht verschickt. (Wortlaut der Fehlermeldung: {0})", ex.Message)
                Return False
            End Try
        End Function

        Public Sub UpdateWebUserPasswordChangeRequestKey(key As String)
            Dim cn As New SqlClient.SqlConnection(m_app.Connectionstring)
            cn.Open()

            Dim cmdUpdate As New SqlClient.SqlCommand("UPDATE WebUser SET PasswordChangeRequestKey=@key where UserID=" & m_intUserId, cn)

            With cmdUpdate.Parameters
                .AddWithValue("@UserID", UserID)
                .AddWithValue("@key", key)
            End With
            cmdUpdate.ExecuteNonQuery()

            cn.Close()
            cn.Dispose()

        End Sub

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

        Public Function SendUserUnlockMail(ByRef errorMsg As String, currentUser As User) As Boolean
            Try
                If Not m_customer.CustomerUsernameRules.DontSendEmail Then
                    If (m_mail <> String.Empty) Then
                        Dim smtpMailServer As String = ConfigurationManager.AppSettings("SmtpMailServer")
                        Dim smtpMailSender As String = ConfigurationManager.AppSettings("SmtpMailSender")
                        Dim subject As String = "Ihr Benutzerkonto wurde entsperrt"
                        Dim textBuilder As New Text.StringBuilder()
                        Dim PortalLink As String = LoadLoginLinks(m_customer.LoginLinkID)

                        If PortalLink = String.Empty Then
                            PortalLink = "https://sgw.kroschke.de/Services/"
                        End If

                        ' Bsp.: "https://sgw.kroschke.de/Services[/]Start/Login.aspx" --> Das richtige Trennzeichen [] suchen, um den aktuellen Server auszulesen
                        Dim iSplitindex As Integer = 0
                        For i As Integer = 0 To 3
                            iSplitindex = PortalLink.IndexOf("/", iSplitindex)
                            iSplitindex += 1
                        Next

                        Dim sServer As String = PortalLink.Remove(iSplitindex - 1)

                        With textBuilder
                            .AppendFormat("Sehr geehrte(r) {0} {1} {2},", m_title, m_firstname, m_lastname)
                            .Append(Environment.NewLine)
                            .Append(Environment.NewLine)
                            .AppendLine("Ihr Benutzerkonto für Internetbereich der Kroschke Gruppe und dem Deutschen Auto Dienst GmbH wurde entsperrt.")
                            .Append(Environment.NewLine)
                            .AppendLine("Sie können sich unter folgendem Link,")
                            .Append(Environment.NewLine)
                            .AppendLine(PortalLink)
                            .Append(Environment.NewLine)
                            .AppendFormat("mit Ihrem Benutzernamen {0} und Ihrem persönlichen Passwort anmelden.", m_strUsername)
                            .Append(Environment.NewLine)
                            .Append(Environment.NewLine)
                            .Append(Environment.NewLine)
                            .AppendLine("Haben Sie Fragen oder benötigen Sie ein neues Passwort, melden Sie sich telefonisch bei Ihrem Web-Administrator.")
                            .Append(Environment.NewLine)
                            .AppendLine("Mit freundlichen Grüßen")
                            .Append(Environment.NewLine)
                            .AppendFormat("Web-Administration, {0} {1}", currentUser.FirstName, currentUser.LastName)

                        End With

                        Dim client As New System.Net.Mail.SmtpClient(smtpMailServer)
                        client.Send(smtpMailSender, m_mail, subject, textBuilder.ToString)

                        Return True
                    Else
                        errorMsg = "Keine Mailadresse angegeben. Der Entsperr-Email konnte nicht versendet werden."
                        Return False
                    End If
                End If
            Catch ex As Exception
                errorMsg = String.Format("Beim Mailversand ist ein technisches Problem aufgetreten. Die Email wurde möglicherweise nicht verschickt. (Wortlaut der Fehlermeldung: {0})", ex.Message)
                Return False
            End Try
        End Function

        Private Function LoadLoginLinks(LinkID As Integer) As String

            Dim TempTable As New DataTable
            Dim cn As New SqlClient.SqlConnection(m_strConnectionstring)
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
                                                 "Reference2, " & _
                                                 "Reference3, " & _
                                                 "Reference4, " & _
                                                 "LoggedOn, " & _
                                                 "LastLogin, " & _
                                                 "ReadMessageCount, " & _
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
                                                 "ValidTo, " & _
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
                                    "@Reference2, " & _
                                    "@Reference3, " & _
                                    "@Reference4, " & _
                                    "@LoggedOn, " & _
                                    "GetDate(), " & _
                                    "@ReadMessageCount, " & _
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
                                    "@ValidTo, " & _
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
                                                 "ValidTo, " & _
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
                                    "@ValidTo, " & _
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
                        .AddWithValue("@ValidTo", m_ValidTo)
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
                                 "Reference2=@Reference2, " & _
                                 "Reference3=@Reference3, " & _
                                 "Reference4=@Reference4, " & _
                                 "LoggedOn=@LoggedOn, " & _
                                 "ReadMessageCount=@ReadMessageCount, " & _
                                 "Approved=@Approved, " & _
                                 "FirstName=@FirstName, " & _
                                 "LastName=@LastName, " & _
                                 "Title=@Title, " & _
                                 "Store=@Store, " & _
                                 "LastChangedBy=@ChangeUser, " & _
                                 "Matrix=@Matrix, " & _
                                 "ValidFrom=@ValidFrom, " & _
                                 "ValidTo=@ValidTo, " & _
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
                                 "ValidTo=@ValidTo, " & _
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
                        .AddWithValue("@ValidTo", IIf(m_ValidTo = String.Empty, DBNull.Value, m_ValidTo))
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
                    .AddWithValue("@Reference2", m_strReference2)
                    .AddWithValue("@Reference3", m_strReference3)
                    .AddWithValue("@Reference4", m_blnReference4)
                    .AddWithValue("@LoggedOn", m_blnLoggedOn)
                    .AddWithValue("@ReadMessageCount", m_intReadMessageCount)
                    .AddWithValue("@Approved", m_approved)
                    .AddWithValue("@FirstName", m_firstname)
                    .AddWithValue("@LastName", m_lastname)
                    .AddWithValue("@Title", m_title)
                    .AddWithValue("@Store", m_store)
                    .AddWithValue("@Matrix", m_blnMatrixfilled)
                    .AddWithValue("@ChangeUser", m_strCreatedBy)
                    .AddWithValue("@ValidFrom", IIf(m_ValidFrom = String.Empty, DBNull.Value, m_ValidFrom))
                    .AddWithValue("@ValidTo", IIf(m_ValidTo = String.Empty, DBNull.Value, m_ValidTo))
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
                If (Not m_groups(0).Message.Length = 0) And (m_intReadMessageCount <= m_groups(0).MaxReadMessageCount) Then
                    m_intReadMessageCount += 1
                Else
                    m_groups(0).Message = ""
                End If
                Dim cmd As New SqlClient.SqlCommand("UPDATE WebUser SET LastLogin=@LastLogin, ReadMessageCount=@ReadMessageCount WHERE UserID=@UserID", cn)
                With cmd.Parameters
                    .AddWithValue("@UserID", m_intUserId)
                    .AddWithValue("@LastLogin", dtmLastLogin.ToString)
                    .AddWithValue("@ReadMessageCount", m_intReadMessageCount)
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

        Public Sub UnlockAccount()
            Try
                Using connection As New SqlClient.SqlConnection(m_strConnectionstring)
                    connection.Open()

                    UnlockAccount(m_strUsername, connection, "System")

                    connection.Close()
                End Using

            Catch ex As Exception

            End Try

        End Sub

        Public Function GetStringUserReferenceValueByReferenceType(ByVal referenceType As ReferenzfeldtypString) As String
            If Customer.ReferenceType1 = referenceType.ToString() Then
                Return Reference
            End If

            If Customer.ReferenceType2 = referenceType.ToString() Then
                Return Reference2
            End If

            If Customer.ReferenceType3 = referenceType.ToString() Then
                Return Reference3
            End If

            Return ""

        End Function

        Public Function GetBoolUserReferenceValueByReferenceType(ByVal referenceType As ReferenzfeldtypBool) As Boolean
            If Customer.ReferenceType4 = referenceType.ToString() Then
                Return Reference4
            End If

            Return False

        End Function

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
