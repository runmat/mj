Imports CKG.Base.Kernel.Security

Namespace Kernel
    Public Class FieldTranslation
        REM § Enthält Übersetzungen einer SAP-Tabellen-Spalte zu einer Reporttabellenspalte
        REM § (Übersetzung von Namen und z.T. Datentyp)

#Region " Membervariables "
        Private m_strConnectionstring As String
        Private m_intApplicationFieldID As Integer
        Private m_strFieldType As String
        Private m_strFieldName As String
        Private m_strField As String
        Private m_strAppURL As String
        Private m_intCustomerID As Integer
        Private m_intLanguageID As Integer
        Private m_blnVisibility As Boolean
        Private m_strContent As String
        Private m_strToolTip As String
        Private m_intGroupID As Integer
        Private m_blnEingebeFeld As Boolean
#End Region

#Region " Constructor "
        Public Sub New(ByVal intApplicationFieldID As Integer, _
                        ByVal strFieldType As String, _
                        ByVal strFieldName As String, _
                        ByVal strAppURL As String, _
                        ByVal intCustomerID As Integer, _
                        ByVal intLanguageID As Integer, _
                        ByVal blnVisibility As Boolean, _
                        ByVal strContent As String, _
                        ByVal strToolTip As String, _
                        ByVal blnEingabefeld As String, _
                        Optional ByVal intGroupID As Integer = 0)

            m_intApplicationFieldID = intApplicationFieldID
            m_strFieldType = strFieldType
            m_strFieldName = strFieldName
            m_strAppURL = strAppURL
            m_intCustomerID = intCustomerID
            m_intLanguageID = intLanguageID
            m_blnVisibility = blnVisibility
            m_strContent = strContent
            m_strToolTip = strToolTip
            m_blnEingebeFeld = blnEingabefeld
            m_intGroupID = intGroupID
        End Sub
        Public Sub New(ByVal intApplicationFieldID As Integer, ByVal _user As User)
            Me.New(intApplicationFieldID, _user.App.Connectionstring)
        End Sub
        Public Sub New(ByVal strAppURL As String, ByVal strFieldType As String, ByVal strFieldName As String, ByVal intCustomerID As Integer, ByVal intLanguageID As Integer, ByVal _user As User)
            Me.New(strAppURL, strFieldType, strFieldName, intCustomerID, intLanguageID, _user.App.Connectionstring)
        End Sub
        Public Sub New(ByVal intApplicationFieldID As Integer, ByVal strConnectionString As String)
            Me.New(intApplicationFieldID, New SqlClient.SqlConnection(strConnectionString))
        End Sub
        Public Sub New(ByVal strAppURL As String, ByVal strFieldType As String, ByVal strFieldName As String, ByVal intCustomerID As Integer, ByVal intLanguageID As Integer, ByVal strConnectionString As String)
            Me.New(strAppURL, strFieldType, strFieldName, intCustomerID, intLanguageID, New SqlClient.SqlConnection(strConnectionString))
        End Sub
        Public Sub New(ByVal intApplicationFieldID As Integer, ByVal cn As SqlClient.SqlConnection)
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            GetParameters(intApplicationFieldID, cn)
        End Sub
        Public Sub New(ByVal strAppURL As String, ByVal strFieldType As String, ByVal strFieldName As String, ByVal intCustomerID As Integer, ByVal intLanguageID As Integer, ByVal cn As SqlClient.SqlConnection, Optional ByVal intGroupID As Integer = 0)
            m_strAppURL = strAppURL
            m_strFieldType = strFieldType
            m_strFieldName = strFieldName
            m_intCustomerID = intCustomerID
            m_intLanguageID = intLanguageID
            m_intGroupID = intGroupID
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            GetParameters(cn)
        End Sub
#End Region

#Region " Properties "

        Public ReadOnly Property ToolTip() As String
            Get
                Return m_strToolTip
            End Get
        End Property

        Public ReadOnly Property ApplicationFieldID() As Integer
            Get
                Return m_intApplicationFieldID
            End Get
        End Property

        Public ReadOnly Property FieldType() As String
            Get
                Return m_strFieldType
            End Get
        End Property

        Public ReadOnly Property Field() As String
            Get
                Return m_strField
            End Get
        End Property

        Public ReadOnly Property FieldName() As String
            Get
                Return m_strFieldName
            End Get
        End Property

        Public ReadOnly Property AppURL() As String
            Get
                Return m_strAppURL
            End Get
        End Property

        Public ReadOnly Property CustomerID() As Integer
            Get
                Return m_intCustomerID
            End Get
        End Property

        Public ReadOnly Property LanguageID() As Integer
            Get
                Return m_intLanguageID
            End Get
        End Property

        Public ReadOnly Property Visibility() As Boolean
            Get
                Return m_blnVisibility
            End Get
        End Property

        Public ReadOnly Property Content() As String
            Get
                Return m_strContent
            End Get
        End Property

        Public ReadOnly Property EingebeFeld() As Boolean
            Get
                Return m_blnEingebeFeld
            End Get
        End Property
#End Region

#Region " Functions "
        Private Sub GetParameters(ByVal intApplicationFieldID As Integer, ByVal cn As SqlClient.SqlConnection)
            m_intApplicationFieldID = intApplicationFieldID
            Dim cmdGetCustomer As New SqlClient.SqlCommand("SELECT " & _
                                "[AppURL], " & _
                                "[FieldType] + [FieldName] AS [Field], " & _
                                "[FieldType], " & _
                                "[FieldName], " & _
                                "[CustomerID], " & _
                                "[LanguageID], " & _
                                "[Visibility], " & _
                                "[Content], " & _
                                "[IsInputField], " & _
                                "[ToolTip] " & _
                                "FROM ApplicationField " & _
                                "WHERE ([ApplicationFieldID] = @ApplicationFieldID) " _
                                , cn)
            cmdGetCustomer.Parameters.AddWithValue("@ApplicationFieldID", m_intApplicationFieldID)
            Dim dr As SqlClient.SqlDataReader = cmdGetCustomer.ExecuteReader
            Try
                While dr.Read
                    m_strAppURL = dr("AppURL").ToString
                    m_strField = dr("Field").ToString
                    m_strFieldType = dr("FieldType").ToString
                    m_strFieldName = dr("FieldName").ToString
                    m_intCustomerID = CInt(dr("CustomerID"))
                    m_intLanguageID = CInt(dr("LanguageID"))
                    m_blnVisibility = CBool(dr("Visibility"))
                    m_strContent = dr("Content").ToString
                    m_strToolTip = dr("ToolTip").ToString
                    m_blnEingebeFeld = CBool(dr("IsInputField"))
                End While
                dr.Close()
                cn.Close()
            Catch ex As Exception
                dr.Close()
                cn.Close()
                Throw ex
            End Try
        End Sub

        Private Sub GetParameters(ByVal cn As SqlClient.SqlConnection)

            Dim cmdGetCustomer As New SqlClient.SqlCommand("SELECT " & _
                    "[ApplicationFieldID], " & _
                    "[FieldType] + [FieldName] AS [Field], " & _
                    "[Visibility], " & _
                    "[Content], " & _
                    "[ToolTip]," & _
                    "[IsInputField]" & _
                    "FROM ApplicationField " & _
                    "WHERE ([AppURL] = @AppURL) " & _
                    "AND ([FieldType] = @FieldType) " & _
                    "AND ([FieldName] = @FieldName) " & _
                    "AND ([CustomerID] = @CustomerID) " & _
                    "AND ([GroupID] = @GroupID) " & _
                    "AND ([LanguageID] = @LanguageID) ", cn)
            cmdGetCustomer.Parameters.AddWithValue("@AppURL", m_strAppURL)
            cmdGetCustomer.Parameters.AddWithValue("@FieldType", m_strFieldType)
            cmdGetCustomer.Parameters.AddWithValue("@FieldName", m_strFieldName)
            cmdGetCustomer.Parameters.AddWithValue("@CustomerID", m_intCustomerID)
            cmdGetCustomer.Parameters.AddWithValue("@GroupID", m_intGroupID)
            cmdGetCustomer.Parameters.AddWithValue("@LanguageID", m_intLanguageID)
            Dim dr As SqlClient.SqlDataReader = cmdGetCustomer.ExecuteReader

            Try

                While dr.Read
                    m_intApplicationFieldID = CInt(dr("ApplicationFieldID"))
                    m_strField = dr("Field").ToString
                    m_blnVisibility = CBool(dr("Visibility"))
                    m_strContent = dr("Content").ToString
                    m_strToolTip = dr("ToolTip").ToString
                    m_blnEingebeFeld = CBool(dr("IsInputField"))
                End While
                dr.Close()
                cn.Close()
            Catch ex As Exception
                dr.Close()
                cn.Close()
                Throw ex
            End Try
        End Sub



        Public Sub Delete(ByVal strConnectionString As String)
            Try
                m_strConnectionstring = strConnectionString
                Dim cn As New SqlClient.SqlConnection(m_strConnectionstring)
                cn.Open()
                Delete(cn)
            Catch ex As Exception
                Throw New Exception("Fehler beim Löschen der Feldübersetzung!", ex)
            End Try
        End Sub
        Public Sub Delete(ByVal cn As SqlClient.SqlConnection)
            Try
                Dim strDelete As String = "DELETE " & _
                                          "FROM ApplicationField " & _
                                          "WHERE ApplicationFieldID=@ApplicationFieldID "

                Dim cmd As New SqlClient.SqlCommand()
                cmd.Connection = cn
                cmd.Parameters.AddWithValue("@ApplicationFieldID", m_intApplicationFieldID)
                cmd.CommandText = strDelete
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                Throw New Exception("Fehler beim Löschen der Feldübersetzung!", ex)
            End Try
        End Sub

        Public Sub Save(ByVal strConnectionString As String)
            Try
                m_strConnectionstring = strConnectionString
                Dim cn As New SqlClient.SqlConnection(m_strConnectionstring)
                cn.Open()
                Save(cn)
            Catch ex As Exception
                Throw New Exception("Fehler beim Speichern der Feldübersetzung!", ex)
            End Try
        End Sub



        Public Sub Save(ByVal cn As SqlClient.SqlConnection)
            Try
                Dim strInsert As String = "INSERT INTO ApplicationField ([AppURL], " & _
                                                      "[FieldType], " & _
                                                      "[FieldName], " & _
                                                      "[CustomerID], " & _
                                                      "[LanguageID], " & _
                                                      "[Visibility], " & _
                                                      "[Content], " & _
                                                      "[ToolTip])" & _
                                                      "[GroupID])" & _
                                                      "[IsInputField])" & _
                                          "VALUES (@AppURL, " & _
                                                 "@FieldType, " & _
                                                 "@FieldName, " & _
                                                 "@CustomerID, " & _
                                                 "@LanguageID, " & _
                                                 "@Visibility, " & _
                                                 "@Content, " & _
                                                 "@ToolTip , " & _
                                                 "@GroupID , " & _
                                                 "@IsInputField)"

                Dim strUpdate As String = "UPDATE ApplicationField " & _
                                          "SET [AppURL]=@AppURL, " & _
                                              "[FieldType]=@FieldType, " & _
                                              "[FieldName]=@FieldName, " & _
                                              "[CustomerID]=@CustomerID, " & _
                                              "[LanguageID]=@LanguageID, " & _
                                              "[Visibility]=@Visibility, " & _
                                              "[Content]=@Content, " & _
                                              "[ToolTip]=@Tooltip " & _
                                          "WHERE ApplicationFieldID=@ApplicationFieldID "

                Dim cmd As New SqlClient.SqlCommand()
                cmd.Connection = cn

                'Speichern
                If m_intApplicationFieldID = -1 Then
                    cmd.CommandText = strInsert
                Else
                    cmd.CommandText = strUpdate
                    cmd.Parameters.AddWithValue("@ApplicationFieldID", m_intApplicationFieldID)
                End If
                With cmd.Parameters
                    .AddWithValue("@AppURL", m_strAppURL)
                    .AddWithValue("@FieldType", m_strFieldType)
                    .AddWithValue("@FieldName", m_strFieldName)
                    .AddWithValue("@CustomerID", m_intCustomerID)
                    .AddWithValue("@LanguageID", m_intLanguageID)
                    .AddWithValue("@Visibility", m_blnVisibility)
                    .AddWithValue("@Content", m_strContent)
                    .AddWithValue("@ToolTip", m_strToolTip)
                End With

                cmd.ExecuteNonQuery()

                If m_intApplicationFieldID = -1 Then
                    With cmd.Parameters
                        .Clear()
                        .AddWithValue("@AppURL", m_strAppURL)
                        .AddWithValue("@FieldType", m_strFieldType)
                        .AddWithValue("@FieldName", m_strFieldName)
                        .AddWithValue("@CustomerID", m_intCustomerID)
                        .AddWithValue("@LanguageID", m_intLanguageID)
                    End With

                    cmd.CommandText = "SELECT [ApplicationFieldID] FROM ApplicationField " & _
                                        "WHERE [AppURL]=@AppURL " & _
                                        "AND [FieldType]=@FieldType " & _
                                        "AND [FieldName]=@FieldName " & _
                                        "AND [CustomerID]=@CustomerID " & _
                                        "AND [LanguageID]=@LanguageID "

                    m_intApplicationFieldID = CInt(cmd.ExecuteScalar)
                End If
            Catch ex As Exception
                Throw New Exception("Fehler beim Speichern der Feldübersetzung!", ex)
            End Try
        End Sub

        Public Sub Save(ByVal cn As SqlClient.SqlConnection, ByVal ZLD As String)
            Try
                Dim strInsert As String = "INSERT INTO ApplicationField ([AppURL], " & _
                                                      "[FieldType], " & _
                                                      "[FieldName], " & _
                                                      "[CustomerID], " & _
                                                      "[LanguageID], " & _
                                                      "[Visibility], " & _
                                                      "[Content], " & _
                                                      "[ToolTip], " & _
                                                      "[GroupID]," & _
                                                      "[IsInputField])" & _
                                          "VALUES (@AppURL, " & _
                                                 "@FieldType, " & _
                                                 "@FieldName, " & _
                                                 "@CustomerID, " & _
                                                 "@LanguageID, " & _
                                                 "@Visibility, " & _
                                                 "@Content, " & _
                                                 "@ToolTip, " & _
                                                 "@GroupID, " & _
                                                 "@IsInputField )"

                Dim strUpdate As String = "UPDATE ApplicationField " & _
                                          "SET [AppURL]=@AppURL, " & _
                                              "[FieldType]=@FieldType, " & _
                                              "[FieldName]=@FieldName, " & _
                                              "[CustomerID]=@CustomerID, " & _
                                              "[LanguageID]=@LanguageID, " & _
                                              "[Visibility]=@Visibility, " & _
                                              "[Content]=@Content, " & _
                                              "[ToolTip]=@Tooltip, " & _
                                              "[GroupID]=@GroupID, " & _
                                              "[IsInputField]=@IsInputField " & _
                                          "WHERE ApplicationFieldID=@ApplicationFieldID "

                Dim cmd As New SqlClient.SqlCommand()
                cmd.Connection = cn

                'Speichern
                If m_intApplicationFieldID = -1 Then
                    cmd.CommandText = strInsert
                Else
                    cmd.CommandText = strUpdate
                    cmd.Parameters.AddWithValue("@ApplicationFieldID", m_intApplicationFieldID)
                End If
                With cmd.Parameters
                    .AddWithValue("@AppURL", m_strAppURL)
                    .AddWithValue("@FieldType", m_strFieldType)
                    .AddWithValue("@FieldName", m_strFieldName)
                    .AddWithValue("@CustomerID", m_intCustomerID)
                    .AddWithValue("@LanguageID", m_intLanguageID)
                    .AddWithValue("@Visibility", m_blnVisibility)
                    .AddWithValue("@Content", m_strContent)
                    .AddWithValue("@ToolTip", m_strToolTip)
                    .AddWithValue("@GroupID", m_intGroupID)
                    .AddWithValue("@IsInputField", m_blnEingebeFeld)

                End With

                cmd.ExecuteNonQuery()

                If m_intApplicationFieldID = -1 Then
                    With cmd.Parameters
                        .Clear()
                        .AddWithValue("@AppURL", m_strAppURL)
                        .AddWithValue("@FieldType", m_strFieldType)
                        .AddWithValue("@FieldName", m_strFieldName)
                        .AddWithValue("@CustomerID", m_intCustomerID)
                        .AddWithValue("@LanguageID", m_intLanguageID)
                        .AddWithValue("@GroupID", m_intGroupID)
                    End With

                    cmd.CommandText = "SELECT [ApplicationFieldID] FROM ApplicationField " & _
                                        "WHERE [AppURL]=@AppURL " & _
                                        "AND [FieldType]=@FieldType " & _
                                        "AND [FieldName]=@FieldName " & _
                                        "AND [CustomerID]=@CustomerID " & _
                                        "AND [GroupID]=@GroupID " & _
                                        "AND [LanguageID]=@LanguageID "

                    m_intApplicationFieldID = CInt(cmd.ExecuteScalar)
                End If
            Catch ex As Exception
                Throw New Exception("Fehler beim Speichern der Feldübersetzung!", ex)
            End Try
        End Sub
#End Region

    End Class
End Namespace