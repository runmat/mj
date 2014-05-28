
Namespace Kernel
    Public Class AppTechTypeList
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

                Dim daAppType As New SqlClient.SqlDataAdapter("SELECT AppTechType FROM AppTechTypes", cn)
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