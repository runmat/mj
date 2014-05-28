Namespace Business
    Public Class Versicherer
        REM § Enthält Daten eines einzelnen Versicheres.

#Region " Membervariables "
        Private m_strConnectionstring As String
        Private m_intVersichererID As Integer
        Private m_strSAPNr As String
        Private m_strName1 As String
        Private m_intCustomerID As Integer  '§§§ JVE 19.10.2006
#End Region

#Region " Constructor "
        Public Sub New(ByVal intVersichererID As Integer)
            m_intVersichererID = intVersichererID
        End Sub
        Public Sub New(ByVal intVersichererID As Integer, _
                       ByVal strSAPNr As String, _
                       ByVal strName1 As String)
            m_intVersichererID = intVersichererID
            m_strSAPNr = strSAPNr
            m_strName1 = strName1
        End Sub
        Public Sub New(ByVal intVersichererID As Integer, ByVal _user As Base.Kernel.Security.User)
            Me.New(intVersichererID, _user.App.Connectionstring)
        End Sub
        Public Sub New(ByVal intVersichererID As Integer, ByVal strConnectionString As String)
            Me.New(intVersichererID, New SqlClient.SqlConnection(strConnectionString))
        End Sub
        Public Sub New(ByVal intVersichererID As Integer, ByVal cn As SqlClient.SqlConnection)
            m_intVersichererID = intVersichererID
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            GetData(cn)
        End Sub
#End Region

#Region " Properties "
        Public ReadOnly Property VersichererId() As Integer
            Get
                Return m_intVersichererID
            End Get
        End Property

        Public ReadOnly Property SAPNr() As String
            Get
                Return m_strSAPNr
            End Get
        End Property

        Public ReadOnly Property Name1() As String
            Get
                Return m_strName1
            End Get
        End Property
#End Region

#Region " Functions "
        Private Sub GetData(ByVal cn As SqlClient.SqlConnection)
            Dim cmdGet As New SqlClient.SqlCommand("SELECT * FROM Versicherer WHERE VersichererID=@VersichererID", cn)
            cmdGet.Parameters.AddWithValue("@VersichererID", VersichererId)
            Dim dr As SqlClient.SqlDataReader = cmdGet.ExecuteReader
            Try
                While dr.Read
                    m_intVersichererID = CInt(dr("VersichererID"))
                    m_strSAPNr = CStr(dr("SAP-Nr"))
                    m_strName1 = CStr(dr("Name1"))
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
            m_strConnectionstring = strConnectionString
            Dim cn As New SqlClient.SqlConnection(m_strConnectionstring)
            Try
                cn.Open()
                Delete(cn)
            Catch ex As Exception
                Throw New Exception("Fehler beim Löschen des Versicherers!", ex)
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End Sub
        Public Sub Delete(ByVal cn As SqlClient.SqlConnection)
            Try
                Dim strDeleteSQL As String = "DELETE " & _
                                              "FROM Versicherer " & _
                                              "WHERE VersichererID=@VersichererID"

                Dim cmd As New SqlClient.SqlCommand()
                cmd.Connection = cn
                cmd.Parameters.AddWithValue("@VersichererID", m_intVersichererID)

                cmd.CommandText = strDeleteSQL
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                Throw New Exception("Fehler beim Löschen des Versicherers!", ex)
            End Try
        End Sub

        Public Sub Save(ByVal strConnectionString As String, ByVal intCustomerID As Integer)
            m_strConnectionstring = strConnectionString
            Dim cn As New SqlClient.SqlConnection(m_strConnectionstring)
            Try
                cn.Open()
                Save(cn, intCustomerID)
            Catch ex As Exception
                Throw New Exception("Fehler beim Speichern des Versicherers!", ex)
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End Sub

        Public Sub Save(ByVal cn As SqlClient.SqlConnection, ByVal CustomerID As Integer) '§§§ JVE 19.10.2006: CustomerID eingefügt!!
            Try
                Dim strInsert As String = "INSERT INTO Versicherer([SAP-Nr], " & _
                                                      "Name1,CustomerID) " & _
                                          "VALUES(@SAPNr, " & _
                                                 "@Name1,@CustomerID); " & _
                                          "SELECT SCOPE_IDENTITY()"

                Dim strUpdate As String = "UPDATE Versicherer " & _
                                          "SET [SAP-Nr]=@SAPNr, " & _
                                              "Name1=@Name1,CustomerID=@CustomerID " & _
                                           "WHERE VersichererID=@VersichererID"

                Dim cmd As New SqlClient.SqlCommand()
                cmd.Connection = cn

                'Speichern
                If m_intVersichererID = -1 Then
                    cmd.CommandText = strInsert
                Else
                    cmd.CommandText = strUpdate
                    cmd.Parameters.AddWithValue("@VersichererID", m_intVersichererID)
                End If
                With cmd.Parameters
                    .AddWithValue("@SAPNr", m_strSAPNr)
                    .AddWithValue("@Name1", m_strName1)
                    .AddWithValue("@CustomerID", CustomerID)        '§§§ JVE 19.10.2006
                End With

                If m_intVersichererID = -1 Then
                    m_intVersichererID = CInt(cmd.ExecuteScalar)
                Else
                    cmd.ExecuteNonQuery()
                End If
            Catch ex As Exception
                Throw New Exception("Fehler beim Speichern des Versicherers!", ex)
            End Try
        End Sub

#End Region

    End Class
End Namespace

' ************************************************
' $History: Versicherer.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 11.04.08   Time: 11:57
' Updated in $/CKAG/Base/Business
' Migration OR
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 10.04.08   Time: 17:37
' Updated in $/CKAG/Base/Business
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 3.04.08    Time: 16:42
' Created in $/CKAG/Base/Business
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 6.12.07    Time: 12:47
' Updated in $/CKG/Base/Base/Business
' ITA:1440
' 
' *****************  Version 4  *****************
' User: Uha          Date: 1.03.07    Time: 16:32
' Updated in $/CKG/Base/Base/Business
' 
' ************************************************