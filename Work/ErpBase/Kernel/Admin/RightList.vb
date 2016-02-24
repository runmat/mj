Imports System.Collections.Generic
Imports System.Configuration
Imports System.Data.SqlClient

Namespace Kernel.Admin

    Public Class RightList
        Inherits DataTable

#Region " Membervariables "

        Private m_cn As SqlConnection
        Private m_intCustomerID As Integer

#End Region

#Region " Properties"

#End Region

#Region " Constructor "

        Public Sub New(ByVal intCustomerId As Integer, ByVal cn As SqlConnection)
            m_cn = cn
            m_intCustomerID = intCustomerId
        End Sub

#End Region

#Region " Functions "

        Public Sub GetAllPossibleRightsforThisCustomer()

            Using daRights As New SqlDataAdapter()

                daRights.SelectCommand = New SqlCommand()
                daRights.SelectCommand.Connection = m_cn

                daRights.SelectCommand.CommandText = "SELECT m.*, c.CustomerID AS CustomerID, ISNULL(c.HasSettings, 0) AS HasSettings, c.EditUserName, c.EditDate " & _
                    "FROM CategorySettingsMetadata m LEFT OUTER JOIN CategorySettingsCustomer c ON m.CategoryID = c.CategoryID AND c.CustomerID = @CustomerID"

                daRights.SelectCommand.Parameters.AddWithValue("@CustomerID", m_intCustomerID)

                daRights.Fill(Me)

            End Using

        End Sub

        Public Shared Function ShowRightsPerCustomer(ByVal customerId As Integer) As List(Of String)

            Dim values As New List(Of String)

            Using cn As New SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))

                cn.Open()

                Using cmd As SqlCommand = cn.CreateCommand()

                    cmd.CommandType = CommandType.Text

                    cmd.CommandText = "SELECT CategoryID FROM CategorySettingsCustomer WHERE CustomerID = @CustomerID AND HasSettings = @HasSettings"

                    cmd.Parameters.AddWithValue("@CustomerID", customerId)
                    cmd.Parameters.AddWithValue("@HasSettings", True)

                    Using reader As SqlDataReader = cmd.ExecuteReader()

                        Do While reader.Read()
                            values.Add(reader.GetString(0))
                        Loop

                        reader.Close()

                    End Using

                End Using

                cn.Close()

            End Using

            Return values

        End Function

        Public Shared Sub SaveRightPerCustomer(ByVal customerId As Integer, ByVal categoryId As String, ByVal isChecked As Boolean, ByVal strUsernameBearbeiter As String)

            Using cn As New SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))

                cn.Open()

                Using cmd As SqlCommand = cn.CreateCommand()

                    cmd.CommandType = CommandType.Text

                    cmd.CommandText = "SELECT COUNT(*) FROM CategorySettingsCustomer WHERE CustomerID = @CustomerID AND CategoryID = @CategoryID"

                    cmd.Parameters.AddWithValue("@CustomerID", customerId)
                    cmd.Parameters.AddWithValue("@CategoryID", categoryId)

                    Dim rowCount As Integer = CInt(cmd.ExecuteScalar())

                    cmd.Parameters.Clear()

                    If rowCount > 0 Then
                        cmd.CommandText = "UPDATE CategorySettingsCustomer SET HasSettings = @HasSettings, EditUserName = @EditUserName, EditDate = @EditDate " & _
                        "WHERE CustomerID = @CustomerID AND CategoryID = @CategoryID"
                    Else
                        cmd.CommandText = "INSERT INTO CategorySettingsCustomer (HasSettings, EditUserName, EditDate, CustomerID, CategoryID) " & _
                            "VALUES (@HasSettings, @EditUserName, @EditDate, @CustomerID, @CategoryID)"
                    End If

                    cmd.Parameters.AddWithValue("@HasSettings", isChecked)
                    cmd.Parameters.AddWithValue("@EditUserName", strUsernameBearbeiter)
                    cmd.Parameters.AddWithValue("@EditDate", DateTime.Now)
                    cmd.Parameters.AddWithValue("@CustomerID", customerId)
                    cmd.Parameters.AddWithValue("@CategoryID", categoryId)

                    cmd.ExecuteNonQuery()

                End Using

                cn.Close()

            End Using

        End Sub

        Public Shared Sub DeleteRightSettingForAllUsersOfThisCustomer(ByVal customerId As Integer, ByVal categoryId As String)

            Using cn As New SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))

                cn.Open()

                Using cmd As SqlCommand = cn.CreateCommand()

                    cmd.CommandType = CommandType.Text

                    cmd.CommandText = "DELETE FROM CategorySettingsWebUser WHERE Username IN (SELECT Username FROM WebUser WHERE CustomerID = @CustomerID) AND CategoryID = @CategoryID"

                    cmd.Parameters.AddWithValue("@CustomerID", customerId)
                    cmd.Parameters.AddWithValue("@CategoryID", categoryId)

                    cmd.ExecuteNonQuery()

                End Using

                cn.Close()

            End Using

        End Sub

        Public Shared Function ShowRightsPerUser(ByVal userName As String, ByVal customerId As Integer) As ArrayList

            Dim values As New ArrayList()

            Using cn As New SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))

                cn.Open()

                Using cmd As SqlCommand = cn.CreateCommand()

                    cmd.CommandType = CommandType.Text

                    cmd.CommandText = "SELECT m.*, ISNULL(u.UserName, '') AS UserName, ISNULL(u.SettingsValue, '') AS SettingsValue " & _
                        "FROM CategorySettingsMetadata m INNER JOIN CategorySettingsCustomer c ON m.CategoryID = c.CategoryID " & _
                        "LEFT OUTER JOIN CategorySettingsWebUser u ON m.CategoryID = u.CategoryID AND u.UserName = @UserName WHERE c.CustomerID = @CustomerID AND c.HasSettings = @HasSettings"

                    cmd.Parameters.AddWithValue("@CustomerID", customerId)
                    cmd.Parameters.AddWithValue("@HasSettings", True)
                    cmd.Parameters.AddWithValue("@UserName", userName)

                    Using reader As SqlDataReader = cmd.ExecuteReader()

                        Do While reader.Read()
                            values.Add(New UserRightInformation(reader.GetString(reader.GetOrdinal("UserName")),
                                                                reader.GetString(reader.GetOrdinal("CategoryID")),
                                                                reader.GetString(reader.GetOrdinal("SettingsType")),
                                                                reader.GetString(reader.GetOrdinal("SettingsValue")),
                                                                reader.GetString(reader.GetOrdinal("Description"))))
                        Loop

                        reader.Close()

                    End Using

                End Using

                cn.Close()

            End Using

            Return values

        End Function

        Shared Sub SaveRightPerUser(ByVal strUserName As String, ByVal categoryId As String, ByVal strUserRightValue As String, ByVal strUsernameBearbeiter As String)

            Using cn As New SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))

                cn.Open()

                Using cmd As SqlCommand = cn.CreateCommand()

                    cmd.CommandType = CommandType.Text

                    cmd.CommandText = "SELECT COUNT(*) FROM CategorySettingsWebUser WHERE UserName = @UserName AND CategoryID = @CategoryID"

                    cmd.Parameters.AddWithValue("@UserName", strUserName)
                    cmd.Parameters.AddWithValue("@CategoryID", categoryId)

                    Dim rowCount As Integer = CInt(cmd.ExecuteScalar())

                    cmd.Parameters.Clear()

                    If rowCount > 0 Then
                        cmd.CommandText = "UPDATE CategorySettingsWebUser SET SettingsValue = @SettingsValue, EditUserName = @EditUserName, EditDate = @EditDate " & _
                        "WHERE UserName = @UserName AND CategoryID = @CategoryID"
                    Else
                        cmd.CommandText = "INSERT INTO CategorySettingsWebUser (SettingsValue, EditUserName, EditDate, UserName, CategoryID) " & _
                            "VALUES (@SettingsValue, @EditUserName, @EditDate, @UserName, @CategoryID)"

                    End If

                    cmd.Parameters.AddWithValue("@SettingsValue", strUserRightValue)
                    cmd.Parameters.AddWithValue("@EditUserName", strUsernameBearbeiter)
                    cmd.Parameters.AddWithValue("@EditDate", DateTime.Now)
                    cmd.Parameters.AddWithValue("@UserName", strUserName)
                    cmd.Parameters.AddWithValue("@CategoryID", categoryId)

                    cmd.ExecuteNonQuery()

                End Using

                cn.Close()

            End Using

        End Sub

#End Region
    End Class

    Public Class UserRightInformation

        Public Property UserName As String

        Public Property CategoryId As String

        Public Property SettingsType As String

        ReadOnly Property IsCheckBoxVisible As Boolean
            Get
                Return SettingsType = "chkbox"
            End Get
        End Property

        ReadOnly Property IsTextBoxVisible As Boolean
            Get
                Return SettingsType = "txtfield"
            End Get
        End Property

        Public Property SettingsValue As String

        Public Property Description As String

        Public Sub New(username As String, categoryId As String, settingstype As String, settingsvalue As String, description As String)
            MyBase.New()
            Me.UserName = username
            Me.CategoryId = categoryId
            Me.SettingsType = settingstype
            Me.SettingsValue = settingsvalue
            Me.Description = description
        End Sub

    End Class

End Namespace
