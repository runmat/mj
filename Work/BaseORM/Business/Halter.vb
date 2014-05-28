
Namespace Business
    Public Class Halter
        REM § Enthält Daten eines einzelnen Halters.

#Region " Membervariables "
        Private m_strConnectionstring As String
        Private m_intHalterID As Integer
        Private m_strSAPNr As String
        Private m_strName1 As String
        Private m_strName2 As String
        Private m_strStrasseHNr As String
        Private m_strOrt As String
        Private m_strKBANR As String
        Private m_intCustomerID As Integer
#End Region

#Region " Constructor "
        Public Sub New(ByVal intHalterID As Integer)
            m_intHalterID = intHalterID
        End Sub
        Public Sub New(ByVal intHalterID As Integer, _
                       ByVal strSAPNr As String, _
                       ByVal strName1 As String, _
                       ByVal strName2 As String, _
                       ByVal strStrasseHNr As String, _
                       ByVal strOrt As String, _
                       ByVal strKBANR As String, ByVal intCustomerID As Integer)
            m_intHalterID = intHalterID
            m_strSAPNr = strSAPNr
            m_strName1 = strName1
            m_strName2 = strName2
            m_strStrasseHNr = strStrasseHNr
            m_strOrt = strOrt
            m_strKBANR = strKBANR
            m_intCustomerID = intCustomerID
        End Sub
        Public Sub New(ByVal intHalterID As Integer, ByVal _user As Kernel.Security.User)
            Me.New(intHalterID, _user.App.Connectionstring)
        End Sub
        Public Sub New(ByVal intHalterID As Integer, ByVal strConnectionString As String)
            Me.New(intHalterID, New SqlClient.SqlConnection(strConnectionString))
        End Sub
        Public Sub New(ByVal intHalterID As Integer, ByVal cn As SqlClient.SqlConnection)
            m_intHalterID = intHalterID
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            GetData(cn)
        End Sub
#End Region

#Region " Properties "
        Public ReadOnly Property HalterId() As Integer
            Get
                Return m_intHalterID
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

        Public ReadOnly Property Name2() As String
            Get
                Return m_strName2
            End Get
        End Property

        Public ReadOnly Property StrasseHNr() As String
            Get
                Return m_strStrasseHNr
            End Get
        End Property

        Public ReadOnly Property Ort() As String
            Get
                Return m_strOrt
            End Get
        End Property

        Public ReadOnly Property KBANR() As String
            Get
                Return m_strKBANR
            End Get
        End Property
#End Region

#Region " Functions "
        Private Sub GetData(ByVal cn As SqlClient.SqlConnection)
            Dim cmdGet As New SqlClient.SqlCommand("SELECT * FROM Halter WHERE HalterID=@HalterID", cn)
            cmdGet.Parameters.AddWithValue("@HalterID", HalterId)
            Dim dr As SqlClient.SqlDataReader = cmdGet.ExecuteReader
            Try
                While dr.Read
                    m_intHalterID = CInt(dr("HalterID"))
                    m_strSAPNr = CStr(dr("SAP-Nr"))
                    m_strName1 = CStr(dr("Name1"))
                    m_strName2 = CStr(dr("Name2"))
                    m_strStrasseHNr = CStr(dr("StrasseHNr"))
                    m_strOrt = CStr(dr("Ort"))
                    m_strKBANR = CStr(dr("KBANR"))
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
                Throw New Exception("Fehler beim Löschen des Halters!", ex)
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End Sub
        Public Sub Delete(ByVal cn As SqlClient.SqlConnection)
            Try
                Dim strDeleteSQL As String = "DELETE " & _
                                              "FROM Halter " & _
                                              "WHERE HalterID=@HalterID"

                Dim cmd As New SqlClient.SqlCommand()
                cmd.Connection = cn
                cmd.Parameters.AddWithValue("@HalterID", m_intHalterID)

                cmd.CommandText = strDeleteSQL
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                Throw New Exception("Fehler beim Löschen des Halters!", ex)
            End Try
        End Sub

        Public Sub Save(ByVal strConnectionString As String)
            m_strConnectionstring = strConnectionString
            Dim cn As New SqlClient.SqlConnection(m_strConnectionstring)
            Try
                cn.Open()
                Save(cn)
            Catch ex As Exception
                Throw New Exception("Fehler beim Speichern des Halters!", ex)
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End Sub
        Public Sub Save(ByVal cn As SqlClient.SqlConnection)
            Try
                Dim strInsert As String = "INSERT INTO Halter([SAP-Nr], " & _
                                                      "Name1, " & _
                                                      "Name2, " & _
                                                      "StrasseHNr, " & _
                                                      "Ort, " & _
                                                      "KBANR, CustomerID) " & _
                                          "VALUES(@SAPNr, " & _
                                                 "@Name1, " & _
                                                 "@Name2, " & _
                                                 "@StrasseHNr, " & _
                                                 "@Ort, " & _
                                                 "@KBANR,@CustomerID); " & _
                                          "SELECT SCOPE_IDENTITY()"

                Dim strUpdate As String = "UPDATE Halter " & _
                                          "SET [SAP-Nr]=@SAPNr, " & _
                                              "Name1=@Name1, " & _
                                              "Name2=@Name2, " & _
                                              "StrasseHNr=@StrasseHNr, " & _
                                              "Ort=@Ort, " & _
                                              "KBANR=@KBANR,CustomerID=@CustomerID " & _
                                           "WHERE HalterID=@HalterID"

                Dim cmd As New SqlClient.SqlCommand()
                cmd.Connection = cn

                'Speichern
                If m_intHalterID = -1 Then
                    cmd.CommandText = strInsert
                Else
                    cmd.CommandText = strUpdate
                    cmd.Parameters.AddWithValue("@HalterID", m_intHalterID)
                End If
                With cmd.Parameters
                    .AddWithValue("@SAPNr", m_strSAPNr)
                    .AddWithValue("@Name1", m_strName1)
                    .AddWithValue("@Name2", m_strName2)
                    .AddWithValue("@StrasseHNr", m_strStrasseHNr)
                    .AddWithValue("@Ort", m_strOrt)
                    .AddWithValue("@KBANR", m_strKBANR)
                    .AddWithValue("@CustomerID", m_intCustomerID)
                End With

                If m_intHalterID = -1 Then
                    m_intHalterID = CInt(cmd.ExecuteScalar())
                Else
                    cmd.ExecuteNonQuery()
                End If
            Catch ex As Exception
                Throw New Exception("Fehler beim Speichern des Halters!", ex)
            End Try
        End Sub

#End Region

    End Class
End Namespace

' ************************************************
' $History: Halter.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 11.04.08   Time: 11:57
' Updated in $/CKAG/Base/Business
' Migration OR
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 3.04.08    Time: 16:42
' Created in $/CKAG/Base/Business
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 6.12.07    Time: 12:47
' Updated in $/CKG/Base/Base/Business
' ITA:1440
' 
' *****************  Version 5  *****************
' User: Uha          Date: 22.05.07   Time: 9:31
' Updated in $/CKG/Base/Base/Business
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 4  *****************
' User: Uha          Date: 1.03.07    Time: 16:32
' Updated in $/CKG/Base/Base/Business
' 
' ************************************************