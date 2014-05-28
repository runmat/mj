Imports System.Configuration

Namespace Kernel.Security
    <Serializable()> Public Class HistoryUser
        REM § Enthält Daten eines einzelnen Benutzers.

#Region " Membervariables "
        Private m_strConnectionstring As String

        Private m_blnAccountIsLockedOut As Boolean
        Private m_blnCustomerAdmin As Boolean = False
        Private m_intFailedLogins As Integer
        Private m_intUserHistoryID As Int32 = -1
        Private m_blnPwdNeverExpires As Boolean = False
        Private m_blnTestUser As Boolean = False
        Private m_dtmLastPwdChange As DateTime
        Private m_strCustomerName As String
        Private m_strGroupName As String
        Private m_strOrganizationName As String
        Private m_strReference As String
        Private m_strUsername As String
        Private m_strPassword As String
        Private m_dtmCreated As DateTime
        Private m_blnDeleted As Boolean = False
        Private m_dtmDeleteDate As DateTime
        Private m_dtmLastChanged As DateTime
        Private m_strLastChange As String
        Private m_strLastChangedBy As String

        Private m_strErrorMessage As String
#End Region

#Region " Constructor "
        Public Sub New(ByVal intUserHistoryID As Integer, ByVal strConnectionString As String)
            Me.New(intUserHistoryID, New SqlClient.SqlConnection(strConnectionString))
        End Sub
        Public Sub New(ByVal intUserHistoryID As Integer, ByVal cn As SqlClient.SqlConnection)
            m_strConnectionstring = cn.ConnectionString
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            GetUserDataFromId(intUserHistoryID, cn)
        End Sub
#End Region

#Region " Properties "

        Public ReadOnly Property Password() As String
            Get
                Return m_strPassword
            End Get
        End Property
        Public ReadOnly Property Created() As DateTime
            Get
                Return m_dtmCreated
            End Get
        End Property
        Public ReadOnly Property Deleted() As Boolean
            Get
                Return m_blnDeleted
            End Get
        End Property
        Public ReadOnly Property DeleteDate() As DateTime
            Get
                Return m_dtmDeleteDate
            End Get
        End Property
        Public ReadOnly Property LastChanged() As DateTime
            Get
                Return m_dtmLastChanged
            End Get
        End Property
        Public ReadOnly Property LastChange() As String
            Get
                Return m_strLastChange
            End Get
        End Property
        Public ReadOnly Property LastChangedBy() As String
            Get
                Return m_strLastChangedBy
            End Get
        End Property

        Public ReadOnly Property OrganizationName() As String
            Get
                Return m_strOrganizationName
            End Get
        End Property

        Public ReadOnly Property CustomerName() As String 'legacy
            Get
                Return m_strCustomerName
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

        Public ReadOnly Property CustomerAdmin() As Boolean
            Get
                Return m_blnCustomerAdmin
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

        Public ReadOnly Property Reference() As String
            Get
                Return m_strReference
            End Get
        End Property

        Public ReadOnly Property ErrorMessage() As String
            Get
                Return m_strErrorMessage
            End Get
        End Property

        Public ReadOnly Property GroupName() As String
            Get
                Return m_strGroupName
            End Get
        End Property
#End Region

#Region " Functions "
        Private Function GetUserDataFromName(ByVal strUserName As String, ByVal cn As SqlClient.SqlConnection) As Boolean
            Dim blnReturn As Boolean

            Try
                'Eingabe-Parameter ueberpruefen
                If strUserName = String.Empty Then
                    Throw New System.Exception("Kein gültiger Benutzername!")
                End If

                'Prerequisits
                m_strErrorMessage = ""

                Dim cmdUser As SqlClient.SqlCommand
                cmdUser = New SqlClient.SqlCommand("SELECT * FROM WebUserHistory " & _
                                                   "WHERE Username = @Username", cn)
                cmdUser.Parameters.AddWithValue("@Username", strUserName)
                blnReturn = GetUserData(cmdUser, cn)

            Catch ex As Exception
                blnReturn = False
                m_strErrorMessage = ex.Message
            End Try
            Return blnReturn
        End Function

        Private Function GetUserDataFromId(ByVal intUserHistoryID As Integer, ByVal cn As SqlClient.SqlConnection) As Boolean
            Dim blnReturn As Boolean

            Try
                'Eingabe-Parameter ueberpruefen
                If intUserHistoryID = 0 Then
                    Throw New System.Exception("Keine gültige Benutzer-ID!")
                End If

                'Prerequisits
                m_strErrorMessage = ""

                Dim cmdUser As SqlClient.SqlCommand
                cmdUser = New SqlClient.SqlCommand("SELECT * FROM WebUserHistory " & _
                                                   "WHERE UserHistoryID = @UserHistoryID", cn)
                cmdUser.Parameters.AddWithValue("@UserHistoryID", intUserHistoryID)

                blnReturn = GetUserData(cmdUser, cn)

            Catch ex As Exception
                blnReturn = False
                m_strErrorMessage = ex.Message
            End Try

            Return blnReturn
        End Function

        Private Function GetUserData(ByVal cmdUser As SqlClient.SqlCommand, ByVal cn As SqlClient.SqlConnection) As Boolean
            Dim blnReturn As Boolean

            Try
                'User-Informationen holen
                Dim daUser As New SqlClient.SqlDataAdapter(cmdUser)
                Dim dtUser As New DataTable()
                daUser.Fill(dtUser)

                Dim drUser As DataRow
                For Each drUser In dtUser.Rows
                    m_blnAccountIsLockedOut = CBool(drUser("AccountIsLockedOut"))
                    m_intUserHistoryID = CType(drUser("UserHistoryID").ToString, System.Int32)
                    m_strUsername = drUser("Username").ToString
                    m_strPassword = drUser("Password").ToString
                    m_blnCustomerAdmin = CBool(drUser("CustomerAdmin"))
                    m_blnTestUser = CBool(drUser("TestUser"))
                    m_dtmLastPwdChange = CDate(drUser("LastPwdChange"))
                    m_blnPwdNeverExpires = CBool(drUser("PwdNeverExpires"))
                    m_intFailedLogins = CInt(drUser("FailedLogins"))
                    m_strReference = ""
                    If Not TypeOf drUser("Reference") Is System.DBNull Then
                        m_strReference = CStr(drUser("Reference"))
                    End If
                    m_strCustomerName = ""
                    If Not TypeOf drUser("CustomerName") Is System.DBNull Then
                        m_strCustomerName = CStr(drUser("CustomerName"))
                    End If
                    m_strOrganizationName = ""
                    If Not TypeOf drUser("OrganizationName") Is System.DBNull Then
                        m_strOrganizationName = CStr(drUser("OrganizationName"))
                    End If
                    m_strGroupName = ""
                    If Not TypeOf drUser("GroupName") Is System.DBNull Then
                        m_strGroupName = CStr(drUser("GroupName"))
                    End If
                    If Not TypeOf drUser("DeleteDate") Is System.DBNull Then
                        m_dtmDeleteDate = CDate(drUser("DeleteDate"))
                    End If
                    m_dtmCreated = CDate(drUser("Created"))
                    m_blnDeleted = CBool(drUser("Deleted"))
                    m_dtmLastChanged = CDate(drUser("LastChanged"))
                    m_strLastChange = CStr(drUser("LastChange"))
                    m_strLastChangedBy = CStr(drUser("LastChangedBy"))
                Next

                If m_intUserHistoryID = -1 Then
                    Return False
                End If

                blnReturn = True
            Catch ex As Exception
                blnReturn = False
                m_strErrorMessage = ex.Message
            End Try

            Return blnReturn
        End Function

#End Region
    End Class
End Namespace

' ************************************************
' $History: HistoryUser.vb $
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
' *****************  Version 7  *****************
' User: Rudolpho     Date: 6.12.07    Time: 17:10
' Updated in $/CKG/Base/Base/Kernel/Security
' ITA 1440
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 6.12.07    Time: 12:47
' Updated in $/CKG/Base/Base/Kernel/Security
' ITA:1440
' 
' *****************  Version 5  *****************
' User: Uha          Date: 1.03.07    Time: 16:32
' Updated in $/CKG/Base/Base/Kernel/Security
' 
' ************************************************