Namespace Kernel
    Public Class AppConfigurationList
        REM § Liest Anwendungseinstellungen pro AppID

        Inherits DataTable

#Region " Constructor "

        Public Sub New(ByVal intAppID As Integer, ByVal intCustomerID As Integer, ByVal intGroupID As Integer, ByVal strConnectionString As String)

            Using cn As New SqlClient.SqlConnection(strConnectionString)
                cn.Open()

                Dim daFields As New SqlClient.SqlDataAdapter( _
                                    "SELECT * " & _
                                    "FROM ApplicationConfig " & _
                                    "WHERE (AppID = @AppID) " & _
                                    "AND (CustomerID IN (1, @CustomerID)) " & _
                                    "AND (GroupID IN (0, @GroupID)) " & _
                                    "ORDER BY " & _
                                    "ConfigKey, " & _
                                    "CustomerID, " & _
                                    "GroupID " _
                                    , cn)
                daFields.SelectCommand.Parameters.AddWithValue("@AppID", intAppID)
                daFields.SelectCommand.Parameters.AddWithValue("@CustomerID", intCustomerID)
                daFields.SelectCommand.Parameters.AddWithValue("@GroupID", intGroupID)
                daFields.Fill(Me)

                Columns.Add("Standard", Type.GetType("System.String"))

                If Rows.Count > 0 Then
                    Dim i As Integer
                    Dim strName As String = "X"

                    For i = Rows.Count - 1 To 0 Step -1
                        Dim row As DataRow = Rows(i)

                        If strName = CStr(row("ConfigKey")) Then
                            Rows.RemoveAt(i)
                        Else
                            strName = CStr(row("ConfigKey"))
                            If Not ((CInt(row("CustomerID")) = intCustomerID) And (CInt(row("GroupID")) = intGroupID)) Then
                                row("ConfigID") = -1
                            End If
                        End If
                    Next
                End If

                cn.Close()
            End Using

        End Sub

#End Region

    End Class
End Namespace
