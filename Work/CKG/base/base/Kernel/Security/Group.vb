
Namespace Kernel.Security
    <Serializable()> Public Class Group
        REM § Haltung und Verwaltung von Daten EINER administrativen Gruppe

#Region " Membervariables "
        Private m_strConnectionstring As String
        Private m_intGroupID As Integer
        Private m_strGroupName As String
        Private m_strDocuPath As String
        Private m_intCustomerID As Integer
        Private m_blnIsNew As Boolean = False
        Private m_blnDelete As Boolean = False
        Private m_intAuthorizationright As Int32
        Private m_blnIsCustomerGroup As Boolean
        Private m_strStartMethod As String
        Private m_strMessage As String
        Private m_intMaxReadMessageCount As Int32
#End Region

#Region " Constructor "
        Public Sub New(ByVal intGroupID As Integer, ByVal intCustomerID As Integer)
            Me.new(intGroupID, "", intCustomerID, "", 0, False, True, "", "", 3)
        End Sub
        Public Sub New(ByVal intGroupID As Integer, _
                       ByVal strGroupName As String, _
                       ByVal intCustomerID As Integer, _
                       ByVal strDocuPath As String, _
                       ByVal intAuthorizationright As Int32, _
                       ByVal blnIsCustomerGroup As Boolean, _
                       ByVal blnNew As Boolean, _
                       ByVal strStartMethod As String, _
                       ByVal strMessage As String, _
                       ByVal intMaxReadMessageCount As Int32)
            m_blnIsNew = blnNew
            m_intGroupID = intGroupID
            m_strGroupName = strGroupName
            m_strDocuPath = strDocuPath
            m_intCustomerID = intCustomerID
            m_intAuthorizationright = intAuthorizationright
            m_blnIsCustomerGroup = blnIsCustomerGroup
            m_strStartMethod = strStartMethod
            m_strMessage = strMessage
            m_intMaxReadMessageCount = intMaxReadMessageCount
        End Sub
        Public Sub New(ByVal intGroupID As Integer, ByVal strConnectionString As String)
            Me.New(intGroupID, New SqlClient.SqlConnection(strConnectionString))
        End Sub
        Public Sub New(ByVal intGroupID As Integer, ByVal cn As SqlClient.SqlConnection)
            m_intGroupID = intGroupID
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            GetGroup(cn)
            cn.Close()
        End Sub
#End Region

#Region " Properties "
        Public ReadOnly Property MaxReadMessageCount() As Int32
            Get
                Return m_intMaxReadMessageCount
            End Get
        End Property

        Public Property Message() As String
            Get
                Return m_strMessage
            End Get
            Set(ByVal Value As String)
                m_strMessage = Value
            End Set
        End Property

        Public ReadOnly Property Authorizationright() As Int32
            Get
                Return m_intAuthorizationright
            End Get
        End Property

        Public ReadOnly Property IsCustomerGroup() As Boolean
            Get
                Return m_blnIsCustomerGroup
            End Get
        End Property

        Public ReadOnly Property DocuPath() As String
            Get
                Return m_strDocuPath
            End Get
        End Property

        Public ReadOnly Property GroupId() As Integer
            Get
                Return m_intGroupID
            End Get
        End Property

        Public ReadOnly Property GroupName() As String
            Get
                Return m_strGroupName
            End Get
        End Property

        Public ReadOnly Property CustomerId() As Integer
            Get
                Return m_intCustomerID
            End Get
        End Property

        Public ReadOnly Property IsNew() As Boolean
            Get
                Return m_blnIsNew
            End Get
        End Property

        Public ReadOnly Property IsDeleted() As Boolean
            Get
                Return m_blnDelete
            End Get
        End Property

        Public ReadOnly Property StartMethod() As String
            Get
                Return m_strStartMethod
            End Get
        End Property
#End Region

#Region " Functions "
        Private Sub GetGroup(ByVal cn As SqlClient.SqlConnection)
            Try
                Dim cmdGetCustomer As New SqlClient.SqlCommand("SELECT * FROM WebGroup WHERE GroupID=@GroupID", cn)
                cmdGetCustomer.Parameters.AddWithValue("@GroupID", m_intGroupID)
                Dim dr As SqlClient.SqlDataReader
                dr = cmdGetCustomer.ExecuteReader
                While dr.Read
                    m_strGroupName = dr("GroupName").ToString
                    m_strDocuPath = dr("DocuPath").ToString
                    'm_blnGroupAdmin = CBool(dr("GroupAdmin"))
                    m_intCustomerID = CInt(dr("CustomerID"))
                    m_intMaxReadMessageCount = CInt(dr("MaxReadMessageCount"))
                    m_intAuthorizationright = CInt(dr("Authorizationright"))
                    m_blnIsCustomerGroup = CBool(dr("IsCustomerGroup"))
                    If Not dr("StartMethod") Is System.DBNull.Value Then
                        m_strStartMethod = CStr(dr("StartMethod"))
                    End If
                    m_strMessage = ""
                    If Not dr("Message") Is System.DBNull.Value Then
                        m_strMessage = CStr(dr("Message"))
                    End If
                End While
                dr.Close()
                cn.Close()
            Catch ex As Exception
                Throw ex
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End Sub

        Public Sub MarkDeleted()
            m_blnDelete = True
        End Sub

        Public Sub Delete(ByVal strConnectionString As String)
            m_strConnectionstring = strConnectionString
            Dim cn As New SqlClient.SqlConnection(m_strConnectionstring)
            Try
                cn.Open()
                Delete(cn)
            Catch ex As Exception
                Throw New Exception("Fehler beim Löschen der Gruppe!", ex)
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End Sub
        Public Sub Delete(ByVal cn As SqlClient.SqlConnection)
            Try
                Dim strDeleteApps As String = "DELETE " & _
                                              "FROM Rights " & _
                                              "WHERE GroupID=@GroupID"

                Dim strDeleteArchive As String = "DELETE " & _
                                              "FROM WebGroupArchives " & _
                                              "WHERE GroupID=@GroupID"

                Dim strDeleteEmployees As String = "DELETE " & _
                                               "FROM WebGroupEmployee " & _
                                               "WHERE GroupID=@GroupID"

                Dim strDeleteGroup As String = "DELETE " & _
                                               "FROM WebGroup " & _
                                               "WHERE GroupID=@GroupID"




                Dim cmd As New SqlClient.SqlCommand()
                cmd.Connection = cn
                cmd.Parameters.AddWithValue("@GroupId", m_intGroupID)

                'Application-Verknuepfungen loeschen
                cmd.CommandText = strDeleteApps
                cmd.ExecuteNonQuery()

                'Archiv-Verknuepfungen loeschen
                cmd.CommandText = strDeleteArchive
                cmd.ExecuteNonQuery()

                'Mitarbeiter-Verknuepfungen loeschen
                cmd.CommandText = strDeleteEmployees
                cmd.ExecuteNonQuery()

                'Group loeschen
                cmd.CommandText = strDeleteGroup
                cmd.ExecuteNonQuery()

            Catch ex As Exception
                Throw New Exception("Fehler beim Löschen der Gruppe!", ex)
            End Try
        End Sub

        Public Sub Save(ByVal strConnectionString As String)
            m_strConnectionstring = strConnectionString
            Dim cn As New SqlClient.SqlConnection(m_strConnectionstring)
            Try
                cn.Open()
                Save(cn)
            Catch ex As Exception
                Throw New Exception("Fehler beim Speichern der Gruppe!", ex)
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try

        End Sub
        Public Sub Save(ByVal cn As SqlClient.SqlConnection)
            Try
                Dim strInsert As String = "INSERT INTO WebGroup(GroupName, " & _
                                                               "CustomerID, " & _
                                                               "DocuPath, " & _
                                                               "Authorizationright, " & _
                                                               "IsCustomerGroup, " & _
                                                               "StartMethod, " & _
                                                               "Message, " & _
                                                               "MaxReadMessageCount) " & _
                                                 "VALUES(@GroupName, " & _
                                                        "@CustomerID, " & _
                                                        "@DocuPath, " & _
                                                        "@Authorizationright, " & _
                                                        "@IsCustomerGroup, " & _
                                                        "@StartMethod, " & _
                                                        "@Message, " & _
                                                        "@MaxReadMessageCount); " & _
                                                 "SELECT SCOPE_IDENTITY()"

                Dim strUpdate As String = "UPDATE WebGroup " & _
                                          "SET GroupName=@GroupName, " & _
                                               "CustomerID=@CustomerID, " & _
                                               "DocuPath=@DocuPath, " & _
                                               "Authorizationright=@Authorizationright, " & _
                                               "IsCustomerGroup=@IsCustomerGroup, " & _
                                               "StartMethod=@StartMethod, " & _
                                               "Message=@Message, " & _
                                               "MaxReadMessageCount=@MaxReadMessageCount " & _
                                          "WHERE GroupID=@GroupID"

                Dim cmd As New SqlClient.SqlCommand()
                cmd.Connection = cn

                'Speichern
                If m_blnIsNew Then
                    cmd.CommandText = strInsert
                Else
                    cmd.CommandText = strUpdate
                    cmd.Parameters.AddWithValue("@GroupId", m_intGroupID)
                End If
                With cmd.Parameters
                    .AddWithValue("@GroupName", m_strGroupName)
                    .AddWithValue("@CustomerID", m_intCustomerID)
                    .AddWithValue("@DocuPath", m_strDocuPath)
                    .AddWithValue("@Authorizationright", m_intAuthorizationright)
                    .AddWithValue("@IsCustomerGroup", m_blnIsCustomerGroup)
                    .AddWithValue("@StartMethod", m_strStartMethod)
                    .AddWithValue("@Message", m_strMessage)
                    .AddWithValue("@MaxReadMessageCount", m_intMaxReadMessageCount)
                End With


                If m_blnIsNew Then
                    'Wenn Group neu ist dann ID ermitteln, damit bei nachfolgendem Fehler und erneutem Speichern Datensatz nicht doppelt angelegt wird.
                    m_intGroupID = CInt(cmd.ExecuteScalar)
                Else
                    cmd.ExecuteNonQuery()
                End If
            Catch ex As Exception
                Throw New Exception("Fehler beim Speichern der Gruppe!", ex)
            End Try
        End Sub

        Public Function HasUser(ByVal strConnectionString As String) As Boolean
            Dim cn As New SqlClient.SqlConnection(strConnectionString)
            Return HasUser(cn)
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Function
        Public Function HasUser(ByVal cn As SqlClient.SqlConnection) As Boolean
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            Dim cmd As New SqlClient.SqlCommand("SELECT COUNT(UserID) FROM WebMember WHERE GroupID=@GroupID", cn)
            cmd.Parameters.AddWithValue("@GroupID", m_intGroupID)
            If CInt(cmd.ExecuteScalar) > 0 Then
                Return True
            End If
            Return False
        End Function

#End Region

    End Class
End Namespace

' ************************************************
' $History: Group.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 11.02.09   Time: 13:51
' Updated in $/CKAG/Base/Kernel/Security
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
' *****************  Version 4  *****************
' User: Rudolpho     Date: 6.12.07    Time: 12:47
' Updated in $/CKG/Base/Base/Kernel/Security
' ITA:1440
' 
' *****************  Version 3  *****************
' User: Uha          Date: 1.03.07    Time: 16:32
' Updated in $/CKG/Base/Base/Kernel/Security
' 
' ************************************************