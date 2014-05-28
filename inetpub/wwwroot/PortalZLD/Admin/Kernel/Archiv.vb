Imports CKG.Base.Kernel.Security

Namespace Kernel
    Public Class Archiv
        REM § Enthält die Daten einer Anwendung
        REM § (siehe Tabelle Archiv in SQL Server DB)

#Region " Membervariables "
        Private m_strConnectionstring As String
        Private m_intArchivID As Integer
        Private m_strEasyLagerortName As String
        Private m_strEasyArchivName As String
        Private m_intEasyQueryIndex As Integer
        Private m_strEasyQueryIndexName As String
        Private m_strEasyTitleName As String
        Private m_strDefaultQuery As String
        Private m_strArchivetype As String
        Private m_intSortOrder As Integer
#End Region

#Region " Constructor "
        Public Sub New(ByVal intArchivID As Integer)
            m_intArchivID = intArchivID
        End Sub
        Public Sub New(ByVal intArchivID As Integer, _
                       ByVal strEasyLagerortName As String, _
                       ByVal strEasyArchivName As String, _
                       ByVal intEasyQueryIndex As Integer, _
                       ByVal strEasyQueryIndexName As String, _
                       ByVal strEasyTitleName As String, _
                       ByVal strDefaultQuery As String, _
                       ByVal strArchivetype As String, _
                       ByVal intSortOrder As Integer)
            m_intArchivID = intArchivID
            m_strEasyLagerortName = strEasyLagerortName
            m_strEasyArchivName = strEasyArchivName
            m_intEasyQueryIndex = intEasyQueryIndex
            m_strEasyQueryIndexName = strEasyQueryIndexName
            m_strEasyTitleName = strEasyTitleName
            m_strDefaultQuery = strDefaultQuery
            m_strArchivetype = strArchivetype
            m_intSortOrder = intSortOrder
        End Sub
        Public Sub New(ByVal intArchivID As Integer, ByVal _user As User)
            Me.New(intArchivID, _user.App.Connectionstring)
        End Sub
        Public Sub New(ByVal intArchivID As Integer, ByVal strConnectionString As String)
            Me.New(intArchivID, New SqlClient.SqlConnection(strConnectionString))
        End Sub
        Public Sub New(ByVal intArchivID As Integer, ByVal cn As SqlClient.SqlConnection)
            m_intArchivID = intArchivID
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            GetArchiv(cn)
            cn.Open()
        End Sub
#End Region

#Region " Properties "
        Public ReadOnly Property SortOrder() As Integer
            Get
                Return m_intSortOrder
            End Get
        End Property

        Public ReadOnly Property ArchivId() As Integer
            Get
                Return m_intArchivID
            End Get
        End Property

        Public ReadOnly Property EasyLagerortName() As String
            Get
                Return m_strEasyLagerortName
            End Get
        End Property

        Public ReadOnly Property EasyArchivName() As String
            Get
                Return m_strEasyArchivName
            End Get
        End Property

        Public ReadOnly Property EasyQueryIndexName() As String
            Get
                Return m_strEasyQueryIndexName
            End Get
        End Property

        Public ReadOnly Property EasyTitleName() As String
            Get
                Return m_strEasyTitleName
            End Get
        End Property

        Public ReadOnly Property DefaultQuery() As String
            Get
                Return m_strDefaultQuery
            End Get
        End Property

        Public ReadOnly Property Archivetype() As String
            Get
                Return m_strArchivetype
            End Get
        End Property

        Public ReadOnly Property EasyQueryIndex() As Integer
            Get
                Return m_intEasyQueryIndex
            End Get
        End Property
#End Region

#Region " Functions "
        Private Sub GetArchiv(ByVal cn As SqlClient.SqlConnection)

            Dim cmdGetCustomer As New SqlClient.SqlCommand("SELECT * FROM Archiv WHERE ArchivID=@ArchivID", cn)
            cmdGetCustomer.Parameters.AddWithValue("@ArchivID", m_intArchivID)
            Dim dr As SqlClient.SqlDataReader = cmdGetCustomer.ExecuteReader
            Try

                While dr.Read
                    m_strEasyLagerortName = dr("EasyLagerortName").ToString
                    m_strEasyArchivName = dr("EasyArchivName").ToString
                    m_strEasyQueryIndexName = dr("EasyQueryIndexName").ToString
                    m_strEasyTitleName = dr("EasyTitleName").ToString
                    m_strDefaultQuery = dr("DefaultQuery").ToString
                    m_intEasyQueryIndex = CInt(dr("EasyQueryIndex").ToString)
                    m_strArchivetype = dr("Archivetype").ToString
                    m_intSortOrder = CInt(dr("SortOrder").ToString)
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
                Throw New Exception("Fehler beim Löschen der Archivlikation!", ex)
            End Try
        End Sub
        Public Sub Delete(ByVal cn As SqlClient.SqlConnection)
            Try
                Dim strDeleteGroupArchivs As String = "DELETE " & _
                                                   "FROM WebGroupArchives " & _
                                                   "WHERE ArchivID=@ArchivID"

                Dim strDeleteCustomerArchivs As String = "DELETE " & _
                                                      "FROM ArchiveRights " & _
                                                      "WHERE ArchivID=@ArchivID"

                Dim strDeleteArchivs As String = "DELETE " & _
                                              "FROM Archiv " & _
                                              "WHERE ArchivID=@ArchivID"

                Dim cmd As New SqlClient.SqlCommand()
                cmd.Connection = cn
                cmd.Parameters.AddWithValue("@ArchivID", m_intArchivID)

                'Group-Archiv-Verknuepfungen loeschen
                cmd.CommandText = strDeleteGroupArchivs
                cmd.ExecuteNonQuery()

                'Customer-Archiv-Verknuepfungen loeschen
                cmd.CommandText = strDeleteCustomerArchivs
                cmd.ExecuteNonQuery()

                'Archiv loeschen
                cmd.CommandText = strDeleteArchivs
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                Throw New Exception("Fehler beim Löschen des Archivs!", ex)
            End Try
        End Sub

        Public Sub Save(ByVal strConnectionString As String, Optional ByRef typ As Boolean = True)
            Dim cn As SqlClient.SqlConnection
            m_strConnectionstring = strConnectionString
            cn = New SqlClient.SqlConnection(m_strConnectionstring)
            Try

                cn.Open()
                Save(cn, typ)
            Catch ex As Exception
                Throw New Exception("Fehler beim Speichern des Archivs!", ex)
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
                'Normales insert (Haupttabelle 'Archiv')
                Dim strInsert As String = "INSERT INTO Archiv(EasyLagerortName, " & _
                                                      "EasyArchivName, " & _
                                                      "EasyQueryIndex, " & _
                                                      "EasyQueryIndexName, " & _
                                                      "EasyTitleName, " & _
                                                      "DefaultQuery, " & _
                                                      "Archivetype, " & _
                                                      "SortOrder) " & _
                                          "VALUES(@EasyLagerortName, " & _
                                                 "@EasyArchivName, " & _
                                                 "@EasyQueryIndex, " & _
                                                 "@EasyQueryIndexName, " & _
                                                 "@EasyTitleName, " & _
                                                 "@DefaultQuery, " & _
                                                 "@Archivetype, " & _
                                                 "@SortOrder); " & _
                                          "SELECT SCOPE_IDENTITY()"

                Dim strUpdate As String = "UPDATE Archiv " & _
                                                      "SET EasyLagerortName=@EasyLagerortName, " & _
                                                          "EasyArchivName=@EasyArchivName, " & _
                                                          "EasyQueryIndex=@EasyQueryIndex, " & _
                                                          "EasyQueryIndexName=@EasyQueryIndexName, " & _
                                                          "EasyTitleName=@EasyTitleName, " & _
                                                          "DefaultQuery=@DefaultQuery, " & _
                                                          "Archivetype=@Archivetype, " & _
                                                          "SortOrder=@SortOrder " & _
                                                      "WHERE ArchivID=@ArchivID"

                Dim cmd As New SqlClient.SqlCommand()
                cmd.Connection = cn

                'Speichern
                If m_intArchivID = -1 Then
                    cmd.CommandText = strInsert
                    typ = True  '!
                Else
                    cmd.CommandText = strUpdate
                    cmd.Parameters.AddWithValue("@ArchivID", m_intArchivID)
                    typ = False '!
                End If
                With cmd.Parameters
                    .AddWithValue("@EasyLagerortName", m_strEasyLagerortName)
                    .AddWithValue("@EasyArchivName", m_strEasyArchivName)
                    .AddWithValue("@EasyQueryIndex", m_intEasyQueryIndex)
                    .AddWithValue("@EasyQueryIndexName", m_strEasyQueryIndexName)
                    .AddWithValue("@EasyTitleName", m_strEasyTitleName)
                    .AddWithValue("@DefaultQuery", m_strDefaultQuery)
                    .AddWithValue("@Archivetype", m_strArchivetype)
                    .AddWithValue("@SortOrder", m_intSortOrder)
                End With

                If m_intArchivID = -1 Then
                    'Wenn Archiv neu ist dann ID ermitteln, damit bei nachfolgendem Fehler und erneutem Speichern Datensatz nicht doppelt angelegt wird.
                    m_intArchivID = CInt(cmd.ExecuteScalar)
                Else
                    cmd.ExecuteNonQuery()
                End If
            Catch ex As Exception
                Throw New Exception("Fehler beim Speichern des Archivs!", ex)
            End Try
        End Sub
#End Region

    End Class
End Namespace