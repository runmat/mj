
Namespace Kernel
    Public Class AppTypeList
        Inherits DataTable
#Region " Constructor "
        Public Sub New(ByVal strConnectionString As String)
            Me.New(New SqlClient.SqlConnection(strConnectionString))
        End Sub
        Public Sub New(ByVal cn As SqlClient.SqlConnection)
            Try
                If cn.State = ConnectionState.Closed Then
                    cn.Open()
                End If
                '§§§ JVE 26.01.2006 Neue Tabelle "AplicatinType" abgreifen...
                'Dim daAppType As New SqlClient.SqlDataAdapter("SELECT DISTINCT AppType FROM Application", cn)
                Dim daAppType As New SqlClient.SqlDataAdapter("SELECT AppType FROM ApplicationType", cn)
                '----------------------------------------------------------------------------------------------
                daAppType.Fill(Me)
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End Sub
#End Region

    End Class
End Namespace