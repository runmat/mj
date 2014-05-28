Namespace Kernel
    Public Class AccountingAreaList
        REM § Liest Buchungskreise aus Steuertabelle

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
                Dim daApp As New SqlClient.SqlDataAdapter("SELECT Area, Description FROM AccountingArea", cn)
                daApp.Fill(Me)
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End Sub
#End Region

    End Class
End Namespace

' ************************************************
' $History: AccountingAreaList.vb $
' 
' *****************  Version 1  *****************
' User: Hartmannu    Date: 9.09.08    Time: 13:42
' Created in $/CKAG/admin/Kernel
' ITA 2152 und 2158
' 
' ************************************************