Namespace Kernel
    Public Class HierarchyList

        Inherits DataTable

#Region " Constructor "
        Public Sub New(ByVal strConnectionString As String, Optional ByVal blnAll As Boolean = False, Optional ByVal blnNone As Boolean = False)
            Me.New(New SqlClient.SqlConnection(strConnectionString), blnAll, blnNone)
        End Sub
        Public Sub New(ByVal cn As SqlClient.SqlConnection, Optional ByVal blnAll As Boolean = False, Optional ByVal blnNone As Boolean = False)
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            Dim daHierarchys As New SqlClient.SqlDataAdapter( _
                        "SELECT " & _
                        "[ID], " & _
                        "[Level] " & _
                        "FROM [Hierarchy] ORDER BY [ID]", cn)
            daHierarchys.Fill(Me)
            AddAllNone(blnAll, blnNone)
        End Sub
#End Region

#Region " Functions "
        Public Sub AddAllNone(ByVal blnAll As Boolean, ByVal blnNone As Boolean)
            Dim dr As DataRow
            If blnAll Then
                dr = NewRow()
                dr("ID") = 0
                dr("Level") = " - alle - "
                Rows.Add(dr)
            End If
            If blnNone Then
                dr = NewRow()
                dr("ID") = -1
                dr("Level") = " - keine - "
                Rows.Add(dr)
            End If
        End Sub
#End Region

    End Class
End Namespace