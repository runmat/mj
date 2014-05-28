Namespace Kernel
    Public Class ApplicationBAPIList
        REM § Liest Übersetzungen von SAP-Tabellen-Spalten zu Reporttabellenspalten pro Anwendung

        Inherits DataTable

#Region " Constructor "
        Public Sub New(ByVal intAppID As Integer, ByVal strConnectionString As String)
            Me.New(intAppID, New SqlClient.SqlConnection(strConnectionString))
        End Sub
        Public Sub New(ByVal intAppID As Integer, ByVal cn As SqlClient.SqlConnection)
            Try
                If cn.State = ConnectionState.Closed Then
                    cn.Open()
                End If
                Dim daApp As New SqlClient.SqlDataAdapter("SELECT * " & _
                                                          "FROM vwApplicationBAPI " & _
                                                          "WHERE ApplicationID=@AppID", cn)
                daApp.SelectCommand.Parameters.AddWithValue("@AppID", intAppID)
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
' $History: ApplicationBAPIList.vb $
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
' *****************  Version 4  *****************
' User: Rudolpho     Date: 6.12.07    Time: 14:36
' Updated in $/CKG/Admin/AdminWeb/Kernel
' ITA: 1440
' 
' *****************  Version 3  *****************
' User: Uha          Date: 1.03.07    Time: 16:49
' Updated in $/CKG/Admin/AdminWeb/Kernel
' 
' ************************************************