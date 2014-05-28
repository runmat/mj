Public Class UpdateDBProxys
    Inherits DataTable

#Region " Properties"
    Dim cn As SqlClient.SqlConnection
#End Region

#Region " Constructor "
    Public Sub New(ByVal strConnectionString As String, Optional ByVal withUpdateRun As Boolean = False)
        cn = New SqlClient.SqlConnection(strConnectionString)
        If withUpdateRun Then
            refill2()
        Else
            refill()
        End If

    End Sub

    Public Sub New()

    End Sub

    Public Sub refill()
        Try
            Me.Clear()
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            Dim daApp As New SqlClient.SqlDataAdapter("SELECT *" & _
                                                        "FROM BapiUpdate " & _
                                                      "Where BapiName <> 'WebUpdateRun'", cn)
            daApp.Fill(Me)
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Sub


    Public Sub refill2()
        Try
            Me.Clear()
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            Dim daApp As New SqlClient.SqlDataAdapter("SELECT *" & _
                                                        "FROM BapiUpdate", cn)

            daApp.Fill(Me)
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Sub

#End Region
End Class
