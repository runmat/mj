Namespace Kernel
    <Serializable()> Public Class ForbiddenUserName
        REM § Haltung und Verwaltung von Daten EINES verbotenen Benutzernamens

#Region " Membervariables "
        Private m_strConnectionstring As String
        Private m_intID As Integer
        Private m_strUserName As String
        Private m_strUpdateUser As String
        Private m_blnIsNew As Boolean = False
        Private m_blnDelete As Boolean = False
#End Region

#Region " Constructor "
        Public Sub New(ByVal intID As Integer, _
                       ByVal strUserName As String, _
                       ByVal strUpdateUser As String, _
                      ByVal blnNew As Boolean)
            m_blnIsNew = blnNew
            m_intID = intID
            m_strUserName = strUserName
            m_strUpdateUser = strUpdateUser
        End Sub
        Public Sub New(ByVal intID As Integer, ByVal cn As SqlClient.SqlConnection)
            Dim blnCloseOnEnd As Boolean = False
            m_intID = intID
            If cn.State = ConnectionState.Closed Then
                cn.Open()
                blnCloseOnEnd = True
            End If
            GetForbiddenUserName(cn)
            If blnCloseOnEnd Then
                cn.Close()
            End If
        End Sub
#End Region

#Region " Properties "
        Public ReadOnly Property Id() As Integer
            Get
                Return m_intID
            End Get
        End Property

        Public ReadOnly Property UserName() As String
            Get
                Return m_strUserName
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

        Public ReadOnly Property UpdateUser() As String
            Get
                Return m_strUpdateUser
            End Get
        End Property
#End Region

#Region " Functions "
        Private Sub GetForbiddenUserName(ByVal cn As SqlClient.SqlConnection)
            Try
                Dim cmdGetForbiddenUserName As New SqlClient.SqlCommand()
                cmdGetForbiddenUserName.Connection = cn
                cmdGetForbiddenUserName.CommandText = "SELECT * FROM ForbiddenUserNames WHERE [ID]=@ID"
                cmdGetForbiddenUserName.Parameters.AddWithValue("@ID", m_intID)
                Dim dr As SqlClient.SqlDataReader
                dr = cmdGetForbiddenUserName.ExecuteReader
                While dr.Read
                    m_strUserName = dr("UserName").ToString
                    m_strUpdateUser = dr("UpdateUser").ToString
                    m_intID = CInt(dr("ID"))
                End While
                dr.Close()
            Catch ex As Exception
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
                Throw New Exception("Fehler beim Löschen eines Eintrages!", ex)
            End Try
        End Sub
        Public Sub Delete(ByVal cn As SqlClient.SqlConnection)
            Try
                Dim strDeleteForbiddenUserName As String = "DELETE " & _
                                               "FROM ForbiddenUserNames " & _
                                               "WHERE [ID]=@ID"

                Dim cmd As New SqlClient.SqlCommand()
                cmd.Connection = cn
                cmd.Parameters.AddWithValue("@Id", m_intID)

                'ForbiddenUserName loeschen
                cmd.CommandText = strDeleteForbiddenUserName
                cmd.ExecuteNonQuery()

            Catch ex As Exception
                Throw New Exception("Fehler beim Löschen eines Eintrages!", ex)
            End Try
        End Sub

        Public Sub Save(ByVal strConnectionString As String)
            Try
                m_strConnectionstring = strConnectionString
                Dim cn As New SqlClient.SqlConnection(m_strConnectionstring)
                cn.Open()
                Save(cn)
            Catch ex As Exception
                Throw New Exception("Fehler beim Speichern eines Eintrages!", ex)
            End Try
        End Sub
        Public Sub Save(ByVal cn As SqlClient.SqlConnection)
            Try
                Dim strInsert As String = "INSERT INTO ForbiddenUserNames (UserName, " & _
                                                               "UpdateDate, " & _
                                                               "UpdateUser) " & _
                                                 "VALUES(@UserName, " & _
                                                        "GetDate(), " & _
                                                        "@UpdateUser); " & _
                                                 "SELECT IDENT_CURRENT('ForbiddenUserNames')"

                Dim strUpdate As String = "UPDATE ForbiddenUserNames " & _
                                          "SET UserName=@UserName, " & _
                                               "UpdateDate=GetDate(), " & _
                                               "UpdateUser=@UpdateUser " & _
                                          "WHERE [ID]=@ID"

                Dim cmd As New SqlClient.SqlCommand()
                cmd.Connection = cn

                'Speichern
                If m_blnIsNew Then
                    cmd.CommandText = strInsert
                Else
                    cmd.CommandText = strUpdate
                    cmd.Parameters.AddWithValue("@Id", m_intID)
                End If
                With cmd.Parameters
                    .AddWithValue("@UserName", UCase(m_strUserName))
                    .AddWithValue("@UpdateUser", m_strUpdateUser)
                End With


                If m_blnIsNew Then
                    'Wenn ForbiddenUserName neu ist dann ID ermitteln, damit bei nachfolgendem Fehler und erneutem Speichern Datensatz nicht doppelt angelegt wird.
                    m_intID = CInt(cmd.ExecuteScalar)
                Else
                    cmd.ExecuteNonQuery()
                End If
            Catch ex As Exception
                Throw New Exception("Fehler beim Speichern eines Eintrages!", ex)
            End Try
        End Sub
#End Region

    End Class
End Namespace

' ************************************************
' $History: ForbiddenUserName.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 11.04.08   Time: 15:47
' Updated in $/CKAG/admin/Kernel
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 14:47
' Created in $/CKAG/admin/Kernel
' 
' *****************  Version 3  *****************
' User: Uha          Date: 1.03.07    Time: 16:49
' Updated in $/CKG/Admin/AdminWeb/Kernel
' 
' ************************************************