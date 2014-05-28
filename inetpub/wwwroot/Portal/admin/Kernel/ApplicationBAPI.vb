Imports CKG.Base.Kernel.Security

Namespace Kernel
    Public Class ApplicationBAPI
        REM § Enthält Übersetzungen einer SAP-Tabellen-Spalte zu einer Reporttabellenspalte
        REM § (Übersetzung von Namen und z.T. Datentyp)

#Region " Membervariables "
        Private m_strConnectionstring As String
        Private m_intAppID As Integer
        Private m_intBAPI As Integer
#End Region

#Region " Constructor "
        Public Sub New(ByVal intAppID As Integer, ByVal intBAPI As Integer)
            m_intAppID = intAppID
            m_intBAPI = intBAPI
        End Sub
        Public Sub New(ByVal intAppID As Integer, ByVal intBAPI As Integer, ByVal _user As User)
            Me.New(intAppID, intBAPI, _user.App.Connectionstring)
        End Sub
        Public Sub New(ByVal intAppID As Integer, ByVal intBAPI As Integer, ByVal strConnectionString As String)
            Me.New(intAppID, intBAPI, New SqlClient.SqlConnection(strConnectionString))
        End Sub
        Public Sub New(ByVal intAppID As Integer, ByVal intBAPI As Integer, ByVal cn As SqlClient.SqlConnection)
            Try
                m_intAppID = intAppID
                m_intBAPI = intBAPI
                If cn.State = ConnectionState.Closed Then
                    cn.Open()
                End If
                GetCol(cn)
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                    cn.Dispose()
                End If
            End Try
        End Sub
#End Region

#Region " Properties "
        Public ReadOnly Property AppId() As Integer
            Get
                Return m_intAppID
            End Get
        End Property

        Public ReadOnly Property BAPI() As Integer
            Get
                Return m_intBAPI
            End Get
        End Property
#End Region

#Region " Functions "
        Private Function GetCol(ByVal cn As SqlClient.SqlConnection) As Boolean
            Dim blnReturn As Boolean = False
            Dim cmdGetCustomer As New SqlClient.SqlCommand("SELECT * " & _
                                                            "FROM ApplicationBAPI " & _
                                                            "WHERE ApplicationID=@ApplicationID " & _
                                                            "AND BapiID=@BapiID", cn)
            cmdGetCustomer.Parameters.AddWithValue("@ApplicationID", m_intAppID)
            cmdGetCustomer.Parameters.AddWithValue("@BapiID", m_intBAPI)
            Dim dr As SqlClient.SqlDataReader = cmdGetCustomer.ExecuteReader
            Try

                While dr.Read
                    m_intAppID = CInt(dr("ApplicationID"))
                    m_intBAPI = CInt(dr("BapiID").ToString)
                    blnReturn = True
                End While
            Catch ex As Exception
                Throw ex
            Finally
                dr.Close()
                'cn.Close()
            End Try

            Return blnReturn
        End Function

        Public Sub Delete(ByVal strConnectionString As String)
            Dim cn As SqlClient.SqlConnection
            m_strConnectionstring = strConnectionString
            cn = New SqlClient.SqlConnection(m_strConnectionstring)
            Try
                cn.Open()
                Delete(cn)
            Catch ex As Exception
                Throw New Exception("Fehler beim Löschen der BAPI-Zuordnung!", ex)
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End Sub
        Public Sub Delete(ByVal cn As SqlClient.SqlConnection)
            Try
                Dim strDelete As String = "DELETE " & _
                                          "FROM ApplicationBAPI " & _
                                          "WHERE ApplicationID=@ApplicationID " & _
                                            "AND BapiID=@BapiID"

                Dim cmd As New SqlClient.SqlCommand()
                cmd.Connection = cn
                cmd.Parameters.AddWithValue("@ApplicationID", m_intAppID)
                cmd.Parameters.AddWithValue("@BapiID", m_intBAPI)
                cmd.CommandText = strDelete
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                Throw New Exception("Fehler beim Löschen der BAPI-Zuordnung!", ex)
            End Try
        End Sub

        Public Sub Save(ByVal strConnectionString As String)
            Dim cn As SqlClient.SqlConnection
            m_strConnectionstring = strConnectionString
            cn = New SqlClient.SqlConnection(m_strConnectionstring)

            Try
                cn.Open()
                Save(cn)
            Catch ex As Exception
                Throw New Exception("Fehler beim Speichern der BAPI-Zuordnung!", ex)
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End Sub
        Public Sub Save(ByVal cn As SqlClient.SqlConnection)
            Try
                If Not GetCol(cn) Then
                    Dim strInsert As String = "INSERT INTO ApplicationBAPI(ApplicationID, " & _
                                                          "Alignment) " & _
                                              "VALUES(@ApplicationID, " & _
                                                     "@Alignment)"

                    Dim cmd As New SqlClient.SqlCommand()
                    cmd.Connection = cn

                    'Speichern
                    With cmd.Parameters
                        .AddWithValue("@ApplicationID", m_intAppID)
                        .AddWithValue("@BapiID", m_intBAPI)
                    End With

                    cmd.ExecuteNonQuery()
                End If

            Catch ex As Exception
                Throw New Exception("Fehler beim Speichern der BAPI-Zuordnung!", ex)
            End Try
        End Sub
#End Region

    End Class
End Namespace

' ************************************************
' $History: ApplicationBAPI.vb $
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
' *****************  Version 5  *****************
' User: Rudolpho     Date: 6.12.07    Time: 14:36
' Updated in $/CKG/Admin/AdminWeb/Kernel
' ITA: 1440
' 
' *****************  Version 4  *****************
' User: Uha          Date: 1.03.07    Time: 16:49
' Updated in $/CKG/Admin/AdminWeb/Kernel
' 
' ************************************************