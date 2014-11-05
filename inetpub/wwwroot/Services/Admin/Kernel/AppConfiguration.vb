Namespace Kernel
    Public Class AppConfiguration
        REM § Enthält Einstellungen zu einer AppID

#Region " Membervariables "

        Private m_intConfigID As Integer
        Private m_intAppID As Integer
        Private m_strConfigType As String
        Private m_strConfigKey As String
        Private m_intCustomerID As Integer
        Private m_intGroupID As Integer
        Private m_strConfigValue As String
        Private m_strDescription As String

#End Region

#Region " Constructor "

        Public Sub New(ByVal intConfigID As Integer, ByVal intAppID As Integer, ByVal strConfigType As String, ByVal strConfigKey As String, ByVal strConfigValue As String, ByVal strDescription As String, ByVal intCustomerID As Integer, ByVal intGroupID As Integer)
            m_intConfigID = intConfigID
            m_intAppID = intAppID
            m_strConfigType = strConfigType
            m_strConfigKey = strConfigKey
            m_intCustomerID = intCustomerID
            m_strConfigValue = strConfigValue
            m_strDescription = strDescription
            m_intGroupID = intGroupID
        End Sub

        Public Sub New(ByVal intAppID As Integer, ByVal strConfigType As String, ByVal strConfigKey As String, ByVal intCustomerID As Integer, ByVal intGroupID As Integer, ByVal cn As SqlClient.SqlConnection)
            m_intAppID = intAppID
            m_strConfigType = strConfigType
            m_strConfigKey = strConfigKey
            m_intCustomerID = intCustomerID
            m_intGroupID = intGroupID
            GetData(cn)
        End Sub

        Public Sub New(ByVal intConfigID As Integer, ByVal cn As SqlClient.SqlConnection)
            GetData(intConfigID, cn)
        End Sub

#End Region

#Region " Properties "

        Public ReadOnly Property ConfigID As Integer
            Get
                Return m_intConfigID
            End Get
        End Property

        Public ReadOnly Property AppID As Integer
            Get
                Return m_intAppID
            End Get
        End Property

        Public ReadOnly Property ConfigType As String
            Get
                Return m_strConfigType
            End Get
        End Property

        Public ReadOnly Property ConfigKey As String
            Get
                Return m_strConfigKey
            End Get
        End Property

        Public ReadOnly Property CustomerID As Integer
            Get
                Return m_intCustomerID
            End Get
        End Property

        Public ReadOnly Property GroupID As Integer
            Get
                Return m_intGroupID
            End Get
        End Property

        Public ReadOnly Property ConfigValue As String
            Get
                Return m_strConfigValue
            End Get
        End Property

        Public ReadOnly Property Description As String
            Get
                Return m_strDescription
            End Get
        End Property

#End Region

#Region " Functions "

        Private Sub GetData(ByVal cn As SqlClient.SqlConnection)
            Try
                Dim cmdGetAppConfigurations As New SqlClient.SqlCommand("SELECT * " & _
                        "FROM ApplicationConfig " & _
                        "WHERE (AppID = @AppID) " & _
                        "AND (ConfigType = @ConfigType) " & _
                        "AND (ConfigKey = @ConfigKey) " & _
                        "AND (CustomerID = @CustomerID) " & _
                        "AND (GroupID = @GroupID) ", cn)
                cmdGetAppConfigurations.Parameters.AddWithValue("@AppID", m_intAppID)
                cmdGetAppConfigurations.Parameters.AddWithValue("@ConfigType", m_strConfigType)
                cmdGetAppConfigurations.Parameters.AddWithValue("@ConfigKey", m_strConfigKey)
                cmdGetAppConfigurations.Parameters.AddWithValue("@CustomerID", m_intCustomerID)
                cmdGetAppConfigurations.Parameters.AddWithValue("@GroupID", m_intGroupID)

                Using dr As SqlClient.SqlDataReader = cmdGetAppConfigurations.ExecuteReader
                    While dr.Read()
                        m_intConfigID = CInt(dr("ConfigID"))
                        m_strConfigValue = dr("ConfigValue").ToString()
                        m_strDescription = dr("Description").ToString()
                    End While
                    dr.Close()
                End Using

            Catch ex As Exception
                Throw New Exception("Fehler beim Laden der Anwendungseinstellung!", ex)
            End Try
        End Sub

        Private Sub GetData(ByVal intConfigID As Integer, ByVal cn As SqlClient.SqlConnection)
            Try
                Dim cmdGetAppConfigurations As New SqlClient.SqlCommand("SELECT * " & _
                        "FROM ApplicationConfig " & _
                        "WHERE (ConfigID = @ConfigID) ", cn)
                cmdGetAppConfigurations.Parameters.AddWithValue("@ConfigID", intConfigID)

                Using dr As SqlClient.SqlDataReader = cmdGetAppConfigurations.ExecuteReader
                    While dr.Read()
                        m_intConfigID = intConfigID
                        m_intCustomerID = CInt(dr("CustomerID"))
                        m_intGroupID = CInt(dr("GroupID"))
                        m_strConfigType = dr("ConfigType").ToString()
                        m_strConfigKey = dr("ConfigKey").ToString()
                        m_strConfigValue = dr("ConfigValue").ToString()
                        m_strDescription = dr("Description").ToString()
                    End While
                    dr.Close()
                End Using

            Catch ex As Exception
                Throw New Exception("Fehler beim Laden der Anwendungseinstellung!", ex)
            End Try
        End Sub

        Public Sub Delete(ByVal cn As SqlClient.SqlConnection)
            Try
                Dim strDelete As String = "DELETE " & _
                                          "FROM ApplicationConfig " & _
                                          "WHERE ConfigID=@ConfigID "

                Dim cmd As New SqlClient.SqlCommand()
                cmd.Connection = cn
                cmd.Parameters.AddWithValue("@ConfigID", m_intConfigID)
                cmd.CommandText = strDelete
                cmd.ExecuteNonQuery()

            Catch ex As Exception
                Throw New Exception("Fehler beim Löschen der Anwendungseinstellung!", ex)
            End Try
        End Sub

        Public Sub Save(ByVal cn As SqlClient.SqlConnection)
            Try
                Dim strInsert As String = "INSERT INTO ApplicationConfig (AppID, " & _
                                                      "CustomerID, " & _
                                                      "GroupID, " & _
                                                      "ConfigType, " & _
                                                      "ConfigKey, " & _
                                                      "ConfigValue, " & _
                                                      "Description)" & _
                                          "VALUES (@AppID, " & _
                                                 "@CustomerID, " & _
                                                 "@GroupID, " & _
                                                 "@ConfigType, " & _
                                                 "@ConfigKey, " & _
                                                 "@ConfigValue, " & _
                                                 "@Description)"

                Dim strUpdate As String = "UPDATE ApplicationConfig " & _
                                          "SET AppID=@AppID, " & _
                                              "CustomerID=@CustomerID, " & _
                                              "GroupID=@GroupID, " & _
                                              "ConfigType=@ConfigType, " & _
                                              "ConfigKey=@ConfigKey, " & _
                                              "ConfigValue=@ConfigValue, " & _
                                              "Description=@Description " & _
                                          "WHERE ConfigID=@ConfigID "

                Dim cmd As New SqlClient.SqlCommand()
                cmd.Connection = cn

                If m_intConfigID = -1 Then
                    cmd.CommandText = strInsert
                Else
                    cmd.CommandText = strUpdate
                    cmd.Parameters.AddWithValue("@ConfigID", m_intConfigID)
                End If
                With cmd.Parameters
                    .AddWithValue("@AppID", m_intAppID)
                    .AddWithValue("@CustomerID", m_intCustomerID)
                    .AddWithValue("@GroupID", m_intGroupID)
                    .AddWithValue("@ConfigType", m_strConfigType)
                    .AddWithValue("@ConfigKey", m_strConfigKey)
                    .AddWithValue("@ConfigValue", m_strConfigValue)
                    .AddWithValue("@Description", m_strDescription)
                End With

                cmd.ExecuteNonQuery()

                If m_intConfigID = -1 Then
                    cmd.CommandText = "SELECT ConfigID FROM ApplicationConfig " & _
                                        "WHERE AppID=@AppID " & _
                                        "AND CustomerID=@CustomerID " & _
                                        "AND GroupID=@GroupID " & _
                                        "AND ConfigType=@ConfigType " & _
                                        "AND ConfigKey=@ConfigKey "

                    With cmd.Parameters
                        .Clear()
                        .AddWithValue("@AppID", m_intAppID)
                        .AddWithValue("@CustomerID", m_intCustomerID)
                        .AddWithValue("@GroupID", m_intGroupID)
                        .AddWithValue("@ConfigType", m_strConfigType)
                        .AddWithValue("@ConfigKey", m_strConfigKey)
                    End With

                    m_intConfigID = CInt(cmd.ExecuteScalar)
                End If
            Catch ex As Exception
                Throw New Exception("Fehler beim Speichern der Anwendungseinstellung!", ex)
            End Try
        End Sub

#End Region

    End Class
End Namespace