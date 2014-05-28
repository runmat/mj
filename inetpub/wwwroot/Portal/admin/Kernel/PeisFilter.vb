Public Class PeisFilter
    Inherits DataTable

#Region " Properties"
    Dim cn As SqlClient.SqlConnection
#End Region

#Region " Constructor "
    Public Sub New(ByVal strConnectionString As String)
        cn = New SqlClient.SqlConnection(strConnectionString)
        refill()
    End Sub

    Public Sub New()

    End Sub
#End Region

    Public Sub refill()
        Try
            Me.Clear()
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            Dim daApp As New SqlClient.SqlDataAdapter("SELECT *" & _
                                                        "FROM PeisFilter ", cn)
            daApp.Fill(Me)

            For Each tmpRow As DataRow In Me.Rows
                tmpRow("KeyWords") = tmpRow("KeyWords").ToString.Replace(",", "<br>")
            Next

            CKG.Base.Business.HelpProcedures.killAllDBNullValuesInDataTable(Me)


        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Sub
End Class

' ************************************************
' $History: PeisFilter.vb $
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 16.07.09   Time: 8:53
' Updated in $/CKAG/admin/Kernel
' nachbesserungen
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 15.07.09   Time: 15:05
' Updated in $/CKAG/admin/Kernel
' Peis administration testfertig
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 15.07.09   Time: 13:33
' Updated in $/CKAG/admin/Kernel
' Peis Administration
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 15.07.09   Time: 10:41
' Created in $/CKAG/admin/Kernel
' Torso
' 
' ************************************************