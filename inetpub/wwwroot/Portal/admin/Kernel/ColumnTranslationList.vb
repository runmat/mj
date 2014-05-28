Namespace Kernel
    Public Class ColumnTranslationList
        REM § Liest Übersetzungen von SAP-Tabellen-Spalten zu Reporttabellenspalten pro Anwendung

        Inherits DataTable

#Region " Constructor "
        Public Sub New(ByVal intAppID As Integer, ByVal strConnectionString As String)
            Me.New(intAppID, New SqlClient.SqlConnection(strConnectionString))
        End Sub
        Public Sub New(ByVal intAppID As Integer, ByVal cn As SqlClient.SqlConnection)
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            Dim daApp As New SqlClient.SqlDataAdapter("SELECT * " & _
                                                      "FROM ColumnTranslation " & _
                                                      "WHERE AppID=@AppID", cn)
            daApp.SelectCommand.Parameters.AddWithValue("@AppID", intAppID)
            daApp.Fill(Me)
        End Sub
#End Region

    End Class
End Namespace

' ************************************************
' $History: ColumnTranslationList.vb $
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