Public Class UpdateDBProxys
    Inherits DataTable

#Region " Properties"
    Dim cn As SqlClient.SqlConnection
    Dim mTestSap As Boolean

    Public ReadOnly Property TestSap() As Boolean
        Get
            Return mTestSap
        End Get
    End Property
#End Region

#Region " Constructor "
    Public Sub New(ByVal strConnectionString As String, ByVal testSap As Boolean, Optional ByVal withUpdateRun As Boolean = False)
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
            Dim daApp As New SqlClient.SqlDataAdapter("SELECT * FROM BapiUpdate WHERE BapiName <> 'WebUpdateRun' AND ISNULL(TestSap,0)=@TestSap", cn)
            daApp.SelectCommand.Parameters.AddWithValue("@TestSap", mTestSap)
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

' ************************************************
' $History: UpdateDBProxys.vb $
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 15.12.08   Time: 17:37
' Created in $/CKAG/admin/Kernel
' Dyn Proxy integriert
' 
' ************************************************