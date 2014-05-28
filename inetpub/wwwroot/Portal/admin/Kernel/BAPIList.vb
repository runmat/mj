Namespace Kernel
    Public Class BAPIList
        REM § Objekt zum Auflisten von Kunden für die Administration

        Inherits DataTable

#Region " Constructor "
        Public Sub New(ByVal strConnectionString As String, Optional ByVal blnNone As Boolean = False)
            Me.New(New SqlClient.SqlConnection(strConnectionString), blnNone)
        End Sub
        Public Sub New(ByVal cn As SqlClient.SqlConnection, Optional ByVal blnNone As Boolean = False)
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            Dim daBAPIs As New SqlClient.SqlDataAdapter("SELECT ID, BAPI " & _
                                                            "FROM BAPI", cn)
            daBAPIs.Fill(Me)
            AddAllNone(blnNone)
        End Sub
        Public Sub New(ByVal strFilterBAPIName As String, ByVal strConnectionString As String)
            Me.New(strFilterBAPIName, New SqlClient.SqlConnection(strConnectionString))
        End Sub
        Public Sub New(ByVal strFilterBAPIName As String, ByVal cn As SqlClient.SqlConnection)
            If strFilterBAPIName = String.Empty Then strFilterBAPIName = "%"
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            Dim daBAPIs As New SqlClient.SqlDataAdapter("SELECT * " & _
                                                            "FROM BAPI " & _
                                                            "WHERE BAPI LIKE @BAPI", cn)
            daBAPIs.SelectCommand.Parameters.AddWithValue("@BAPI", Replace(strFilterBAPIName, "*", "%"))
            daBAPIs.Fill(Me)
        End Sub
#End Region

#Region " Functions "
        Public Sub AddAllNone(ByVal blnNone As Boolean)
            Dim dr As DataRow
            If blnNone Then
                dr = NewRow()
                dr("ID") = 0
                dr("BAPI") = " - keine Auswahl - "
                Rows.Add(dr)
            End If
        End Sub
#End Region

    End Class
End Namespace

' ************************************************
' $History: BAPIList.vb $
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