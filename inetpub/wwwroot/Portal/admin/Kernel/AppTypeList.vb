Namespace Kernel
    Public Class AppTypeList
        REM § Ermittelt alle AppTypes (Anwendungstypen: Admin, Change, Report) aus Tabelle Application in SQL DB

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

' ************************************************
' $History: AppTypeList.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 14:47
' Created in $/CKAG/admin/Kernel
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 6.12.07    Time: 14:36
' Updated in $/CKG/Admin/AdminWeb/Kernel
' ITA: 1440
' 
' *****************  Version 4  *****************
' User: Uha          Date: 1.03.07    Time: 16:49
' Updated in $/CKG/Admin/AdminWeb/Kernel
' 
' ************************************************