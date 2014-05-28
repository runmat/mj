Imports CKG.Base.Kernel.Security

Namespace Kernel
    Public Class Application
        REM § Enthält die Daten einer Anwendung
        REM § (siehe Tabelle Application in SQL Server DB)

#Region " Membervariables "
        Private m_strConnectionstring As String
        Private m_intAppID As Integer
        Private m_strAppName As String
        Private m_strAppFriendlyName As String
        Private m_strAppType As String
        Private m_strAppURL As String
        Private m_strAppParam As String
        Private m_blnAppInMenu As Boolean
        Private m_strAppComment As String
        Private m_intAppParent As Integer
        Private m_intAppRank As Integer
        Private m_intAuthorizationlevel As Int32
        Private m_blnBatchAuthorization As Boolean
        Private m_AppSchwellwert As String
        Private m_MaxLevel As Integer
        Private m_MaxLevelsPerGroup As Integer
        Private m_AppTechType As String
        Private m_AppDescription As String
#End Region

#Region " Constructor "
        Public Sub New(ByVal intAppID As Integer)
            m_intAppID = intAppID
        End Sub

        Public Sub New(ByVal intAppID As Integer, _
                       ByVal strAppName As String, _
                       ByVal strAppFriendlyName As String, _
                       ByVal strAppType As String, _
                       ByVal strAppURL As String, _
                       ByVal blnAppInMenu As Boolean, _
                       ByVal strAppComment As String, _
                       ByVal intAppParent As Integer, _
                       ByVal intAppRank As Integer, _
                       ByVal intAuthorizationlevel As Integer, _
                       ByVal blnBatchAuthorization As Boolean, _
                       ByVal strAppTechType As String, _
                       ByVal strAppDescription As String, _
                       Optional ByVal intMaxLevel As Integer = 0, _
                       Optional ByVal intMaxLevelsPerGroup As Integer = 0)
            m_intAppID = intAppID
            m_strAppName = strAppName
            m_strAppFriendlyName = strAppFriendlyName
            m_strAppType = strAppType
            m_strAppURL = strAppURL
            m_blnAppInMenu = blnAppInMenu
            m_strAppComment = strAppComment
            m_intAppParent = intAppParent
            m_intAppRank = intAppRank
            m_intAuthorizationlevel = intAuthorizationlevel
            m_blnBatchAuthorization = blnBatchAuthorization
            m_AppTechType = strAppTechType
            m_AppDescription = strAppDescription
            m_AppSchwellwert = AppSchwellwert
            m_MaxLevel = intMaxLevel
            m_MaxLevelsPerGroup = intMaxLevelsPerGroup
        End Sub

        Public Sub New(ByVal intAppID As Integer, ByVal _user As User)
            Me.New(intAppID, _user.App.Connectionstring)
        End Sub

        Public Sub New(ByVal intAppID As Integer, ByVal strConnectionString As String)
            Me.New(intAppID, New SqlClient.SqlConnection(strConnectionString))
        End Sub

        Public Sub New(ByVal intAppID As Integer, ByVal cn As SqlClient.SqlConnection)
            Try
                m_intAppID = intAppID
                If cn.State = ConnectionState.Closed Then
                    cn.Open()
                End If
                GetApp(cn)
                cn.Open()
                GetAppParam(cn)
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End Sub
#End Region

#Region " Properties "
        Public ReadOnly Property BatchAuthorization() As Boolean
            Get
                Return m_blnBatchAuthorization
            End Get
        End Property

        Public ReadOnly Property Authorizationlevel() As Int32
            Get
                Return m_intAuthorizationlevel
            End Get
        End Property

        Public ReadOnly Property AppId() As Integer
            Get
                Return m_intAppID
            End Get
        End Property

        Public ReadOnly Property AppName() As String
            Get
                Return m_strAppName
            End Get
        End Property

        Public ReadOnly Property AppParam() As String
            Get
                Return m_strAppParam
            End Get
        End Property

        Public ReadOnly Property AppFriendlyName() As String
            Get
                Return m_strAppFriendlyName
            End Get
        End Property

        Public ReadOnly Property AppType() As String
            Get
                Return m_strAppType
            End Get
        End Property

        Public ReadOnly Property AppURL() As String
            Get
                Return m_strAppURL
            End Get
        End Property

        Public ReadOnly Property AppInMenu() As Boolean
            Get
                Return m_blnAppInMenu
            End Get
        End Property

        Public ReadOnly Property AppComment() As String
            Get
                Return m_strAppComment
            End Get
        End Property

        Public ReadOnly Property AppParent() As Integer
            Get
                Return m_intAppParent
            End Get
        End Property

        Public ReadOnly Property AppRank() As Integer
            Get
                Return m_intAppRank
            End Get
        End Property

        Public ReadOnly Property AppSchwellwert() As String
            Get
                Return m_AppSchwellwert
            End Get
        End Property

        Public ReadOnly Property MaxLevel As Integer
            Get
                Return m_MaxLevel
            End Get
        End Property

        Public ReadOnly Property MaxLevelsPerGroup As Integer
            Get
                Return m_MaxLevelsPerGroup
            End Get
        End Property

        Public ReadOnly Property AppTechType() As String
            Get
                Return m_AppTechType
            End Get
        End Property

        Public ReadOnly Property AppDescription() As String
            Get
                Return m_AppDescription
            End Get
        End Property
#End Region

#Region " Functions "
        Private Sub GetApp(ByVal cn As SqlClient.SqlConnection)

            Dim cmdGetCustomer As New SqlClient.SqlCommand("SELECT * FROM Application WHERE AppID=@AppID", cn)
            cmdGetCustomer.Parameters.AddWithValue("@AppID", m_intAppID)
            Dim dr As SqlClient.SqlDataReader = cmdGetCustomer.ExecuteReader
            Try
                While dr.Read
                    m_strAppName = dr("AppName").ToString
                    m_strAppFriendlyName = dr("AppFriendlyName").ToString
                    m_strAppType = dr("AppType").ToString
                    m_strAppURL = dr("AppURL").ToString
                    m_blnAppInMenu = CBool(dr("AppInMenu"))
                    m_strAppComment = dr("AppComment").ToString
                    m_intAppParent = CInt(dr("AppParent").ToString)
                    m_intAppRank = CInt(dr("AppRank").ToString)
                    m_intAuthorizationlevel = CInt(dr("Authorizationlevel").ToString)
                    m_blnBatchAuthorization = CBool(dr("BatchAuthorization"))
                    m_AppSchwellwert = dr("AppSchwellwert").ToString
                    m_MaxLevel = CInt(dr("MaxLevel"))
                    m_MaxLevelsPerGroup = CInt(dr("MaxLevelsPerGroup"))
                    m_AppTechType = dr("AppTechType").ToString()
                    m_AppDescription = dr("AppDescription").ToString()
                End While
                dr.Close()
                cn.Close()
            Catch ex As Exception
                dr.Close()
                cn.Close()
                Throw ex
            End Try
        End Sub

        Private Sub GetAppParam(ByVal cn As SqlClient.SqlConnection)

            Dim cmdGetCustomer As New SqlClient.SqlCommand("SELECT * FROM ApplicationParamlist WHERE id_App=@AppID", cn)
            cmdGetCustomer.Parameters.AddWithValue("@AppID", m_intAppID)
            Dim dr As SqlClient.SqlDataReader = cmdGetCustomer.ExecuteReader

            Try

                While dr.Read
                    m_strAppParam = dr("paramlist").ToString
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
            Dim cn As SqlClient.SqlConnection
            m_strConnectionstring = strConnectionString
            cn = New SqlClient.SqlConnection(m_strConnectionstring)
            Try
                cn.Open()
                Delete(cn)
            Catch ex As Exception
                Throw New Exception("Fehler beim Löschen der Applikation!", ex)

            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End Sub

        Public Sub Delete(ByVal cn As SqlClient.SqlConnection)
            Try
                If cn.State = ConnectionState.Closed Then
                    cn.Open()
                End If
                Dim strDeleteGroupApps As String = "DELETE " & _
                                                   "FROM Rights " & _
                                                   "WHERE AppID=@AppID"

                Dim strDeleteCustomerApps As String = "DELETE " & _
                                                      "FROM CustomerRights " & _
                                                      "WHERE AppID=@AppID"

                Dim strDeleteApps As String = "DELETE " & _
                                              "FROM Application " & _
                                              "WHERE AppID=@AppID"

                Dim cmd As New SqlClient.SqlCommand()

                cmd.Connection = cn
                cmd.Parameters.AddWithValue("@AppID", m_intAppID)

                'Group-Application-Verknuepfungen loeschen
                cmd.CommandText = strDeleteGroupApps
                cmd.ExecuteNonQuery()

                'Customer-Application-Verknuepfungen loeschen
                cmd.CommandText = strDeleteCustomerApps
                cmd.ExecuteNonQuery()

                'Application loeschen
                cmd.CommandText = strDeleteApps
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                Throw New Exception("Fehler beim Löschen der Applikation!", ex)
            Finally
                If cn.State = ConnectionState.Open Then
                    cn.Close()
                End If
            End Try
        End Sub

        Public Sub SaveParams(ByVal cn As SqlClient.SqlConnection, ByVal param As String, ByVal typ As Boolean)
            'SQL-Anweisungen für Parameterliste
            Dim strInsertParam As String = "INSERT INTO ApplicationParamlist (id_App,paramlist) VALUES (@AppID,@paramlist)"
            Dim strUpdateParam As String = "UPDATE ApplicationParamlist SET paramlist = @paramlist WHERE id_app = @AppID"
            Dim cmd As New SqlClient.SqlCommand()

            cmd.Connection = cn
            If typ = True Then
                cmd.CommandText = strInsertParam
            Else
                cmd.CommandText = strUpdateParam
            End If
            cmd.Parameters.AddWithValue("@AppID", m_intAppID)
            cmd.Parameters.AddWithValue("@paramlist", param)
            Try
                cn.Open()
                cmd.ExecuteNonQuery()
            Catch ex As Exception
            Finally
                cn.Close()
            End Try

        End Sub

        Public Sub ReAssign(ByVal cn As SqlClient.SqlConnection, ByVal intAppID As Integer, ByVal intParentAppID As Integer)
            If intParentAppID > 0 Then
                Try
                    If cn.State = ConnectionState.Closed Then
                        cn.Open()
                    End If

                    Dim cmd As SqlClient.SqlCommand
                    Dim da As SqlClient.SqlDataAdapter
                    Dim dt As DataTable
                    Dim dr As DataRow

                    'Lösche alte Zuordnungen zu Gruppen
                    cmd = New SqlClient.SqlCommand()
                    cmd.Connection = cn
                    cmd.CommandText = "DELETE FROM Rights WHERE AppID=@AppID"
                    cmd.Parameters.AddWithValue("@AppID", intAppID)
                    cmd.ExecuteNonQuery()

                    'Lösche alte Zuordnungen zu Kunden
                    cmd = New SqlClient.SqlCommand()
                    cmd.Connection = cn
                    cmd.CommandText = "DELETE FROM CustomerRights WHERE AppID=@AppID"
                    cmd.Parameters.AddWithValue("@AppID", intAppID)
                    cmd.ExecuteNonQuery()

                    'Ermittele Kunden, denen Parent zugeordnet ist
                    da = New SqlClient.SqlDataAdapter("SELECT CustomerID FROM CustomerRights WHERE AppID=@AppID", cn)
                    da.SelectCommand.Parameters.AddWithValue("@AppID", intParentAppID)
                    dt = New DataTable()
                    da.Fill(dt)

                    'Speichere Zuordnung zu den Kunden
                    If Not dt Is Nothing Then
                        For Each dr In dt.Rows
                            cmd = New SqlClient.SqlCommand()
                            cmd.Connection = cn
                            cmd.CommandText = "INSERT INTO CustomerRights VALUES (@CustomerID,@AppID)"
                            cmd.Parameters.AddWithValue("@CustomerID", CInt(dr("CustomerID")))
                            cmd.Parameters.AddWithValue("@AppID", intAppID)
                            cmd.ExecuteNonQuery()
                        Next
                    End If

                    'Ermittele Gruppen, denen Parent zugeordnet ist
                    da = New SqlClient.SqlDataAdapter("SELECT GroupID FROM Rights WHERE AppID=@AppID", cn)
                    da.SelectCommand.Parameters.AddWithValue("@AppID", intParentAppID)
                    dt = New DataTable()
                    da.Fill(dt)

                    'Speichere Zuordnung zu den Gruppen
                    If Not dt Is Nothing Then
                        For Each dr In dt.Rows
                            cmd = New SqlClient.SqlCommand()
                            cmd.Connection = cn
                            cmd.CommandText = "INSERT INTO Rights(GroupID,AppID) VALUES (@GroupID,@AppID)"
                            cmd.Parameters.AddWithValue("@GroupID", CInt(dr("GroupID")))
                            cmd.Parameters.AddWithValue("@AppID", intAppID)
                            cmd.ExecuteNonQuery()
                        Next
                    End If

                Catch ex As Exception
                    Throw New Exception("Fehler beim Speichern der Child-Zuordnungen!", ex)
                End Try
            End If
        End Sub

        Public Sub Save(ByVal strConnectionString As String, Optional ByRef typ As Boolean = True)
            Dim cn As SqlClient.SqlConnection
            m_strConnectionstring = strConnectionString
            cn = New SqlClient.SqlConnection(m_strConnectionstring)
            Try

                cn.Open()
                Save(cn, typ)
            Catch ex As Exception
                Throw New Exception("Fehler beim Speichern der Applikation!", ex)
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                    cn.Dispose()
                End If
            End Try
        End Sub

        Public Sub Save(ByVal cn As SqlClient.SqlConnection, Optional ByRef typ As Boolean = True)
            'typ: true = INSERT, false = UPDATE. Wird für die Speicherung der Parameterliste benötigt.
            Try
                'Normales insert (Haupttabelle 'Application')
                Dim strInsert As String = "INSERT INTO Application(AppName, " & _
                                                      "AppFriendlyName, " & _
                                                      "AppType, " & _
                                                      "AppURL, " & _
                                                      "AppInMenu, " & _
                                                      "AppComment, " & _
                                                      "AppParent, " & _
                                                      "AppRank, " & _
                                                      "Authorizationlevel, " & _
                                                      "BatchAuthorization, " & _
                                                      "AppSchwellwert, " & _
                                                      "MaxLevel, " & _
                                                      "MaxLevelsPerGroup, " & _
                                                      "AppTechType) " & _
                                          "VALUES(@AppName, " & _
                                                 "@AppFriendlyName, " & _
                                                 "@AppType, " & _
                                                 "@AppURL, " & _
                                                 "@AppInMenu, " & _
                                                 "@AppComment, " & _
                                                 "@AppParent, " & _
                                                 "@AppRank, " & _
                                                 "@Authorizationlevel, " & _
                                                 "@BatchAuthorization, " & _
                                                 "@Schwellwert, " & _
                                                 "@MaxLevel, " & _
                                                 "@MaxLevelsPerGroup, " & _
                                                 "@AppTechType); " & _
                                          "SELECT SCOPE_IDENTITY()"

                Dim strUpdate As String = "UPDATE Application " & _
                                                      "SET AppName=@AppName, " & _
                                                          "AppFriendlyName=@AppFriendlyName, " & _
                                                          "AppType=@AppType, " & _
                                                          "AppURL=@AppURL, " & _
                                                          "AppInMenu=@AppInMenu, " & _
                                                          "AppComment=@AppComment, " & _
                                                          "AppParent=@AppParent, " & _
                                                          "AppRank=@AppRank, " & _
                                                          "Authorizationlevel=@Authorizationlevel, " & _
                                                          "BatchAuthorization=@BatchAuthorization, " & _
                                                          "AppSchwellwert=@Schwellwert, " & _
                                                          "MaxLevel=@MaxLevel, " & _
                                                          "MaxLevelsPerGroup=@MaxLevelsPerGroup, " & _
                                                          "AppTechType=@AppTechType, " & _
                                                          "AppDescription=@AppDescription " & _
                                                      "WHERE AppID=@AppID"

                Dim cmd As New SqlClient.SqlCommand()
                cmd.Connection = cn

                'Speichern
                If m_intAppID = -1 Then
                    cmd.CommandText = strInsert
                    typ = True  '!
                Else
                    cmd.CommandText = strUpdate
                    cmd.Parameters.AddWithValue("@AppID", m_intAppID)
                    typ = False '!
                End If
                Dim intAutTemp As Int32 = m_intAuthorizationlevel
                If m_intAppParent > 0 Then
                    intAutTemp = 0
                End If
                With cmd.Parameters
                    .AddWithValue("@AppName", m_strAppName)
                    .AddWithValue("@AppFriendlyName", m_strAppFriendlyName)
                    .AddWithValue("@AppType", m_strAppType)
                    .AddWithValue("@AppURL", m_strAppURL)
                    .AddWithValue("@AppInMenu", m_blnAppInMenu)
                    .AddWithValue("@AppComment", m_strAppComment)
                    .AddWithValue("@AppParent", m_intAppParent)
                    .AddWithValue("@AppRank", m_intAppRank)
                    .AddWithValue("@Authorizationlevel", intAutTemp)
                    .AddWithValue("@BatchAuthorization", m_blnBatchAuthorization)
                    .AddWithValue("@Schwellwert", IIf(m_AppSchwellwert = String.Empty, DBNull.Value, m_AppSchwellwert))
                    .AddWithValue("@MaxLevel", m_MaxLevel)
                    .AddWithValue("@MaxLevelsPerGroup", m_MaxLevelsPerGroup)
                    .AddWithValue("@AppTechType", m_AppTechType)
                    .AddWithValue("@AppDescription", m_AppDescription)
                End With

                If m_intAppID = -1 Then
                    'Wenn App neu ist dann ID ermitteln, damit bei nachfolgendem Fehler und erneutem Speichern Datensatz nicht doppelt angelegt wird.
                    m_intAppID = CInt(cmd.ExecuteScalar)
                Else
                    cmd.ExecuteNonQuery()
                End If
            Catch ex As Exception
                Throw New Exception("Fehler beim Speichern der Applikation!", ex)
            End Try
        End Sub

        Public Function HasChildren(ByVal strConnectionString As String) As Boolean
            Dim cn As New SqlClient.SqlConnection(strConnectionString)
            Return HasChildren(cn)
        End Function

        Public Function HasChildren(ByVal cn As SqlClient.SqlConnection) As Boolean
            Try
                If cn.State = ConnectionState.Closed Then
                    cn.Open()
                End If
                Dim cmd As New SqlClient.SqlCommand("SELECT COUNT(AppID) FROM Application WHERE AppParent=@AppParent", cn)
                cmd.Parameters.AddWithValue("@AppParent", m_intAppID)
                If CInt(cmd.ExecuteScalar) > 0 Then
                    Return True
                End If
                Return False
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End Function
#End Region

    End Class
End Namespace