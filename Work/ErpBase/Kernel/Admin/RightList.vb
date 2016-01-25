Imports System.Configuration
Imports System.Data.SqlClient
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security

Namespace Kernel.Admin

    Public Class RightList
        Inherits DataTable

#Region " Membervariables "
        Private m_cn As SqlClient.SqlConnection
        Private m_intCustomerID As Integer
        Private m_intGroupID As Integer
        Private m_blnTableFilled As Boolean = False
        Private m_intHighestAuthorizationlevel As Int32



#End Region

#Region " Properties"
        Public ReadOnly Property HighestAuthorizationlevel() As Int32
            Get
                Return m_intHighestAuthorizationlevel
            End Get
        End Property
#End Region

#Region " Constructor "

        Public Sub New(ByVal intCustomerId As Integer, ByVal cn As SqlClient.SqlConnection)

            m_cn = cn
            m_intCustomerID = intCustomerId
            m_intGroupID = -2 'Indikator dafuer, dass nicht nach Gruppe gesucht wird.
            m_intHighestAuthorizationlevel = 0 '(noch) keine Anwendung gefunden, die Autorisierung erfordert

        End Sub

#End Region

#Region " Functions "

        Public Sub GetAllPossibleRightsforThisCustomer()

            Dim strSql As String = ""
            Dim daRights As New SqlClient.SqlDataAdapter()

            daRights.SelectCommand = New SqlClient.SqlCommand()
            daRights.SelectCommand.Connection = m_cn

            If m_intGroupID = -2 Then

                strSql = " SELECT CustomerID, CategorySettingsCustomer.CategoryID AS CategoryID, HasSettings, "
                strSql += "Description, EditUserName, EditDate"
                strSql += " FROM CategorySettingsCustomer LEFT OUTER JOIN "
                strSql += " CategorySettingsMetadata ON CategorySettingsCustomer.CategoryID = CategorySettingsMetadata.CategoryID "
                strSql += "WHERE (CustomerID = @CustomerID) "

            End If

            daRights.SelectCommand.CommandText = strSql
            daRights.SelectCommand.Parameters.AddWithValue("@CustomerID", m_intCustomerID)
            daRights.Fill(Me)

        End Sub

        Public Shared Sub UpdateSingleRightPerCustomer(ByVal customerId As Integer, ByVal categoryId As String, _
                                                       ByVal isChecked As Boolean, ByVal strUsernameBearbeiter As String)

            Dim cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
            Dim cmd As New SqlClient.SqlCommand
            Dim sSQl As String = ""

            sSQl = "UPDATE CategorySettingsCustomer SET "
            sSQl += "HasSettings = '" & isChecked & "', "
            sSQl += "EditUserName = '" & strUsernameBearbeiter & "', "
            sSQl += "EditDate = '" & Date.Now() & "'"
            sSQl += " WHERE "
            sSQl += "CustomerID = " & customerId
            sSQl += " AND "
            sSQl += "CategoryID = '" & categoryId & "'"

            Try
                cn.Open()
                cmd.Connection = cn
                cmd.CommandType = CommandType.Text
                cmd.CommandText = sSQl
                cmd.ExecuteNonQuery()

            Catch ex As Exception
                Throw New Exception("Update der CustomerRights fehlgeschlagen")
            Finally
                cn.Close()
            End Try

        End Sub

        Public Shared Function InsertOrDeleteRightForAllUsersOfThisCustomer(ByVal customerId As Integer, ByVal categoryId As String, _
                                                     ByVal isChecked As Boolean, ByVal strUsernameBearbeiter As String)

            Dim cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
            Dim sSql As String

            sSql = "SELECT UserID, Username FROM WebUser WHERE customerID = " & customerId


            Using cn
                Dim command As SqlCommand = New SqlCommand(sSql, cn)
                cn.Open()

                Dim reader As SqlDataReader = command.ExecuteReader()

                If reader.HasRows Then
                    Do While reader.Read()
                        InsertOrDeleteRightForSingleUser(customerId, categoryId, isChecked, strUsernameBearbeiter, reader.GetString(1))

                    Loop
                Else
                    Console.WriteLine("No rows found.")
                End If

                reader.Close()
            End Using

            Return Nothing



        End Function

        Private Shared Sub InsertOrDeleteRightForSingleUser(ByVal customerId As Integer, ByVal categoryId As String, _
                                                     ByVal isChecked As Boolean, ByVal strUsernameBearbeiter As String, ByVal strUserId As String)


            Dim cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
            Dim cmd As New SqlClient.SqlCommand
            Dim sSQl As String = ""


            If isChecked = True Then

                sSQl += "IF NOT EXISTS("
                sSql += "SELECT CategoryID FROM [CategorySettingsWebUser] "
                sSql += "WHERE "
                sSql += "UserName LIKE '" & strUserId & "' "
                sSql += "AND "
                sSql += "CategoryID LIKE '" & categoryId & "'"
                sSql += ")"
                sSql += "INSERT INTO [CategorySettingsWebUser] "
                sSql += "(UserName, CategoryID, SettingsValue, EditUserName, EditDate)"
                sSql += "VALUES "
                sSQl += "('" & strUserId & "', '" & categoryId & "', 'false', '" & strUsernameBearbeiter & "', CURRENT_TIMESTAMP)  "

            Else

                sSql += "DELETE FROM [CategorySettingsWebUser] "
                sSql += "WHERE "
                sSql += "UserName LIKE '" & strUserId & "' "
                sSql += "AND "
                sSQl += "CategoryID LIKE '" & categoryId & "'"

            End If

            Try
                cn.Open()
                cmd.Connection = cn
                cmd.CommandType = CommandType.Text
                cmd.CommandText = sSQl

                cmd.ExecuteNonQuery()

            Catch ex As Exception
                Throw New Exception("Problem eim Inserten / Deleten der Rechte für den User " & strUserId)
            Finally
                cn.Close()
            End Try

        End Sub

        Public Shared Function ShowRightsPerUser(ByVal userName As String) As ArrayList

            Dim cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
            Dim sSql As String
            Dim strUserName As String

            strUserName = userName

            sSql = ""
            sSql += "SELECT CategorySettingsWebUser.UserName, CategorySettingsWebUser.CategoryID, CategorySettingsMetadata.SettingsType, "
            sSql += "CategorySettingsWebUser.SettingsValue, CategorySettingsMetadata.Description, CategorySettingsWebUser.EditUserName, "
            sSql += "dbo.CategorySettingsWebUser.EditDate "
            sSql += "FROM CategorySettingsMetadata INNER JOIN "
            sSql += "CategorySettingsWebUser ON CategorySettingsMetadata.CategoryID = CategorySettingsWebUser.CategoryID "
            sSql += "WHERE UserName LIKE '" & strUserName & "'"

            Dim values As ArrayList = New ArrayList()

            Using cn
                Dim command As SqlCommand = New SqlCommand(sSql, cn)
                cn.Open()

                Dim reader As SqlDataReader = command.ExecuteReader()

                If reader.HasRows Then
                    Do While reader.Read()
                        values.Add(New UserRightInformation(reader.GetString(0),
                                                            reader.GetString(1),
                                                            reader.GetString(2),
                                                            reader.GetString(3),
                                                            reader.GetString(4)
                                                            )
                                    )
                    Loop
                Else
                    System.Diagnostics.Debug.WriteLine("No rows found.")
                End If

                reader.Close()

            End Using

            Return values


        End Function

        Shared Sub UpdateRightPerUser(strUserName As String, categoryId As String, strUserRightValue As String, strRightFieldtype As String)

            Dim cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
            Dim cmd As New SqlClient.SqlCommand
            Dim sSQl As String = ""
            Dim strTeilSql As String = ""

            If strRightFieldtype = "txtfield" Then
                strTeilSql += "SettingsValue = '" & strUserRightValue & "' "
            ElseIf (strRightFieldtype = "chkbox") Then
                strTeilSql += "SettingsValue = '" & strUserRightValue & "'"
            End If

            sSQl += "UPDATE CategorySettingsWebUser "
            sSQl += "SET "
            sSQl += strTeilSQL
            sSQl += " WHERE "
            sSQl += "UserName = '" & strUserName & "'"
            sSQl += " AND "
            sSQl += "CategoryID = '" & categoryId & "'"

            Try
                cn.Open()
                cmd.Connection = cn
                cmd.CommandType = CommandType.Text
                cmd.CommandText = sSQl
                cmd.ExecuteNonQuery()

            Catch ex As Exception
                Throw New Exception("Update der CustomerRights fehlgeschlagen")
            Finally
                cn.Close()
            End Try


        End Sub


#End Region
    End Class

    Public Class UserRightInformation

        Dim m_username As String
        Dim m_categoryId As String
        Dim m_settingstype As String
        Dim m_settingsvalue As String
        Dim m_description As String

        Public Sub New(username As String, categoryId As String, settingstype As String, settingsvalue As String, description As String)
            MyBase.New()
            m_username = username
            m_categoryId = categoryId
            m_settingstype = settingstype
            m_settingsvalue = settingsvalue
            m_description = description
        End Sub

        Property UserName As String
            Get
                Return m_username
            End Get

            Set(ByVal value As String)
                m_username = value
            End Set

        End Property

        Property CategoryId As String
            Get
                Return m_categoryId
            End Get
            Set(ByVal value As String)
                m_categoryId = value
            End Set
        End Property

        Property SettingsType As String
            Get
                Return m_settingstype
            End Get
            Set(ByVal value As String)
                m_settingstype = value
            End Set
        End Property

        ReadOnly Property IsCheckBoxVisible As String
            Get
                Return m_settingstype = "chkbox"
            End Get
        End Property

        ReadOnly Property IsTextBoxVisible As String
            Get
                Return m_settingstype = "txtfield"
            End Get
        End Property
        Property SettingsValue As String
            Get
                Return m_settingsvalue
            End Get
            Set(ByVal value As String)
                m_settingsvalue = value
            End Set
        End Property

        Property Description As String
            Get
                Return m_description
            End Get
            Set(ByVal value As String)
                m_description = value
            End Set
        End Property


    End Class

End Namespace
