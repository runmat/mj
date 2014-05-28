Namespace Kernel
    Public Class ForbiddenUserNameAllList
        REM § Auflistungsobjekt für verbotene Benutzernamen zur Verwendung in den Administrationsseiten

        Inherits DataTable

#Region " Constructor "
        Public Sub New(ByVal strConnectionString As String)
            Me.New(New SqlClient.SqlConnection(strConnectionString))
        End Sub
        Public Sub New(ByVal cn As SqlClient.SqlConnection)
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            Dim daForbiddenUserNames As New SqlClient.SqlDataAdapter("SELECT UserName FROM vwForbiddenUser WHERE (LEN(LTRIM(RTRIM(UserName))) > 0) ", cn)
            daForbiddenUserNames.Fill(Me)
        End Sub
        Public Sub New(ByVal strForbiddenUserNameFilter As String, ByVal strConnectionString As String)
            Me.New(strForbiddenUserNameFilter, New SqlClient.SqlConnection(strConnectionString))
        End Sub
        Public Sub New(ByVal strForbiddenUserNameFilter As String, ByVal cn As SqlClient.SqlConnection)
            If strForbiddenUserNameFilter = String.Empty Then strForbiddenUserNameFilter = "%"
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            Dim strSQL As String
            strSQL = "SELECT UserName FROM vwForbiddenUser WHERE (LEN(LTRIM(RTRIM(UserName))) > 0) " & _
                     "AND (UserName LIKE @ForbiddenUserNameName)"
            Dim daForbiddenUserNames As New SqlClient.SqlDataAdapter(strSQL, cn)
            With daForbiddenUserNames.SelectCommand.Parameters
                .AddWithValue("@ForbiddenUserNameName", Replace(strForbiddenUserNameFilter, "*", "%"))
            End With
            daForbiddenUserNames.Fill(Me)
        End Sub

#End Region

#Region " Functions "

#End Region
    End Class
End Namespace

' ************************************************
' $History: ForbiddenUserNameAllList.vb $
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
' *****************  Version 3  *****************
' User: Uha          Date: 1.03.07    Time: 16:49
' Updated in $/CKG/Admin/AdminWeb/Kernel
' 
' ************************************************