Namespace Kernel.Admin
    Public Class ApplicationList
        REM § Liste der im jeweiligen Kontext gültigen Anwendungen

        Inherits DataTable

#Region " Membervariables "
        Private m_cn As SqlClient.SqlConnection
        Private m_intCustomerID As Integer
        Private m_intGroupID As Integer
        Private m_blnTableFilled As Boolean = False
        Private m_intHighestAuthorizationlevel As Int32
#End Region

#Region " Properties"
        Public ReadOnly Property HighestAuthorizationlevel() As Int32
            Get
                Return m_intHighestAuthorizationlevel
            End Get
        End Property
#End Region

#Region " Constructor "
        Public Sub New(ByVal intCustomerID As Integer, ByVal strConnectionString As String)
            Me.New(intCustomerID, New SqlClient.SqlConnection(strConnectionString))
        End Sub
        Public Sub New(ByVal intCustomerID As Integer, ByVal cn As SqlClient.SqlConnection)
            Try
                If cn.State = ConnectionState.Closed Then
                    cn.Open()
                End If
                m_cn = cn
                m_intCustomerID = intCustomerID
                m_intGroupID = -2 'Indikator dafuer, dass nicht nach Gruppe gesucht wird.
                m_intHighestAuthorizationlevel = 0 '(noch) keine Anwendung gefunden, die Autorisierung erfordert
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End Sub
        Public Sub New(ByVal intGroupID As Integer, ByVal intCustomerID As Integer, ByVal strConnectionString As String)
            Me.New(intGroupID, intCustomerID, New SqlClient.SqlConnection(strConnectionString))
        End Sub
        Public Sub New(ByVal intGroupID As Integer, ByVal intCustomerID As Integer, ByVal cn As SqlClient.SqlConnection)
            Try
                If cn.State = ConnectionState.Closed Then
                    cn.Open()
                End If
                m_cn = cn
                m_intCustomerID = intCustomerID
                m_intGroupID = intGroupID
                m_intHighestAuthorizationlevel = 0 '(noch) keine Anwendung gefunden, die Autorisierung erfordert
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End Sub
        Public Sub New(ByVal strAppNameFilter As String, ByVal strConnectionString As String, ByVal strAppFriendlyNameFilter As String)
            Me.New(strAppNameFilter, New SqlClient.SqlConnection(strConnectionString), strAppFriendlyNameFilter)
        End Sub

        Public Sub New(ByVal strAppNameFilter As String, ByVal strConnectionString As String)
            Me.New(strAppNameFilter, New SqlClient.SqlConnection(strConnectionString), String.Empty)
        End Sub

        Public Sub New(ByVal strAppNameFilter As String, ByVal cn As SqlClient.SqlConnection, ByVal strAppFriendlyNameFilter As String)
            Try
                If strAppNameFilter = String.Empty Then strAppNameFilter = "%"
                If strAppFriendlyNameFilter = String.Empty Then strAppFriendlyNameFilter = "%"
                If cn.State = ConnectionState.Closed Then
                    cn.Open()
                End If
                Dim daApp As New SqlClient.SqlDataAdapter("SELECT Application.AppID, " & _
                                                                 "Application.AppName, " & _
                                                                 "Application.AppFriendlyName, " & _
                                                                 "Application.AppType, " & _
                                                                 "Application.AppURL, " & _
                                                                 "Application.AppInMenu, " & _
                                                                 "Application.AppComment, " & _
                                                                 "Application.AppParent, " & _
                                                                 "Application_1.AppName AS AppParentName, " & _
                                                                 "Application.AuthorizationLevel, " & _
                                                                 "Application.BatchAuthorization, " & _
                                                                 "Application.LogDuration, " & _
                                                                 "Application.AppRank " & _
                                                          "FROM Application LEFT OUTER JOIN Application Application_1 " & _
                                                            "ON Application.AppParent = Application_1.AppID " & _
                                                            "WHERE Application.AppName LIKE @AppName" & _
                                                            " AND Application.AppFriendlyName LIKE @AppFriendlyName", cn)
                daApp.SelectCommand.Parameters.AddWithValue("@AppName", Replace(strAppNameFilter, "*", "%"))
                daApp.SelectCommand.Parameters.AddWithValue("@AppFriendlyName", Replace(strAppFriendlyNameFilter, "*", "%"))
                daApp.Fill(Me)
                CheckAuthorizationlevel()
                m_blnTableFilled = True
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End Sub
        Public Sub New(ByVal strConnectionString As String)
            Me.New(New SqlClient.SqlConnection(strConnectionString))
        End Sub
        Public Sub New(ByVal cn As SqlClient.SqlConnection)
            Try
                If cn.State = ConnectionState.Closed Then
                    cn.Open()
                End If
                Dim daApp As New SqlClient.SqlDataAdapter("SELECT * " & _
                                                          "FROM Application " & _
                                                          "WHERE AppParent=0", cn)
                daApp.Fill(Me)
                CheckAuthorizationlevel()
                Dim dr As DataRow
                dr = NewRow()
                dr("AppID") = 0
                dr("AppName") = " - keiner - "
                dr("AuthorizationLevel") = 0
                Rows.Add(dr)
                m_blnTableFilled = True
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End Sub
#End Region

#Region " Functions "
        Public Sub GetAssignedCustomerAdmin()
            If Not m_blnTableFilled Then
                If m_intGroupID = -1 OrElse m_intCustomerID = -1 Then
                    'Customer/Gruppe ist neu: Keine zugewiesen.
                    m_blnTableFilled = True
                    Exit Sub
                End If

                Dim daGroups As New SqlClient.SqlDataAdapter()
                daGroups.SelectCommand = New SqlClient.SqlCommand()
                daGroups.SelectCommand.Connection = m_cn

                Dim strSQL As String
                If m_intGroupID = -2 Then
                    'Es wird nur nach Customer gesucht, nicht nach Gruppe
                    strSQL = "SELECT AppId, AppURL, AppName, AppFriendlyName, AppInMenu, AppRank, AppComment, AppType, AuthorizationLevel " & _
                                              "FROM vwCustomerAppAssigned " & _
                                              "WHERE (CustomerID = @CustomerID) " 
                Else
                    strSQL = "SELECT AppId, AppURL, AppName, AppFriendlyName, AppInMenu, AppRank, AppComment, AppType, AuthorizationLevel, LevelAppToGroup, WithAuthorization, NewLevel " & _
                             "FROM vwGroupAppAssigned " & _
                             "WHERE (CustomerID = @CustomerID) " & _
                               "AND (GroupID = @GroupID) " 
                    daGroups.SelectCommand.Parameters.AddWithValue("@GroupID", m_intGroupID)
                End If
                daGroups.SelectCommand.CommandText = strSQL
                daGroups.SelectCommand.Parameters.AddWithValue("@CustomerID", m_intCustomerID)
                daGroups.Fill(Me)
                CheckAuthorizationlevel()
                m_blnTableFilled = True
            End If
        End Sub

        Public Sub GetAssigned()
            If Not m_blnTableFilled Then
                If m_intGroupID = -1 OrElse m_intCustomerID = -1 Then
                    'Customer/Gruppe ist neu: Keine zugewiesen.
                    m_blnTableFilled = True
                    Exit Sub
                End If

                Dim daGroups As New SqlClient.SqlDataAdapter()
                daGroups.SelectCommand = New SqlClient.SqlCommand()
                daGroups.SelectCommand.Connection = m_cn

                Dim strSQL As String
                If m_intGroupID = -2 Then
                    'Es wird nur nach Customer gesucht, nicht nach Gruppe
                    strSQL = "SELECT AppId, AppURL, AppName, AppFriendlyName, AppInMenu, AppRank, AppComment, AppType, AuthorizationLevel " & _
                                              "FROM vwCustomerAppAssigned " & _
                                              "WHERE (CustomerID = @CustomerID) " & _
                                                "AND (AppParent=0)"
                Else
                    strSQL = "SELECT AppId, AppURL, AppName, AppFriendlyName, AppInMenu, AppRank, AppComment, AppType, AuthorizationLevel, LevelAppToGroup, WithAuthorization, NewLevel " & _
                             "FROM vwGroupAppAssigned " & _
                             "WHERE (CustomerID = @CustomerID) " & _
                               "AND (GroupID = @GroupID) " & _
                               "AND (AppParent=0)"
                    daGroups.SelectCommand.Parameters.AddWithValue("@GroupID", m_intGroupID)
                End If
                daGroups.SelectCommand.CommandText = strSQL
                daGroups.SelectCommand.Parameters.AddWithValue("@CustomerID", m_intCustomerID)
                daGroups.Fill(Me)
                CheckAuthorizationlevel()
                m_blnTableFilled = True
            End If
        End Sub

        Public Sub GetUnassigned()
            If Not m_blnTableFilled Then
                Dim daGroups As New SqlClient.SqlDataAdapter()
                daGroups.SelectCommand = New SqlClient.SqlCommand()
                daGroups.SelectCommand.Connection = m_cn

                Dim strSQL As String
                If m_intGroupID = -1 Then
                    'Gruppe ist neu: Zeige alle die Customer hat.
                    strSQL = "SELECT AppId, AppURL, AppName, AppFriendlyName, AppType " & _
                             "FROM vwCustomerAppAssigned " & _
                             "WHERE (CustomerID = @CustomerID) " & _
                               "AND (AppParent=0)"
                ElseIf m_intGroupID = -2 Then
                    'Es wird nur nach Customer gesucht, nicht nach Gruppe
                    If m_intCustomerID = -1 Then
                        strSQL = "SELECT AppId, AppURL, AppName, AppFriendlyName, AppType " & _
                                 "FROM Application " & _
                                 "WHERE (AppParent=0)"
                    Else
                        strSQL = "SELECT AppId, AppURL, AppName, AppFriendlyName, AppType " & _
                                 "FROM vwCustomerAppUnAssigned " & _
                                 "WHERE (CustomerID = @CustomerID) " & _
                                   "AND (AppParent=0)"
                    End If
                Else
                    strSQL = "SELECT AppId, AppURL, AppName, AppFriendlyName, AppType " & _
                             "FROM vwGroupAppUnAssigned " & _
                             "WHERE (CustomerID = @CustomerID) " & _
                               "AND (GroupID = @GroupID) " & _
                               "AND (AppParent=0)"
                    daGroups.SelectCommand.Parameters.AddWithValue("@GroupID", m_intGroupID)
                End If
                daGroups.SelectCommand.CommandText = strSQL
                daGroups.SelectCommand.Parameters.AddWithValue("@CustomerID", m_intCustomerID)
                daGroups.Fill(Me)
                m_blnTableFilled = True
            End If
        End Sub

        Public Sub AddAllNone(ByVal blnAll As Boolean, ByVal blnNone As Boolean)
            Dim dr As DataRow
            If blnAll Then
                dr = NewRow()
                dr("AppID") = 0
                dr("AppName") = " - alle - "
                dr("AuthorizationLevel") = 0
                Rows.Add(dr)
            End If
            If blnNone Then
                dr = NewRow()
                dr("AppID") = -1
                dr("AppName") = " - keine - "
                dr("AuthorizationLevel") = 0
                Rows.Add(dr)
            End If
        End Sub

        Private Sub CheckAuthorizationlevel()
            Dim rowTemp As DataRow
            If Me.Rows.Count > 0 Then
                For Each rowTemp In Me.Rows
                    If CInt(rowTemp("AuthorizationLevel")) > m_intHighestAuthorizationlevel Then
                        m_intHighestAuthorizationlevel = CInt(rowTemp("AuthorizationLevel"))
                    End If
                Next
            End If
        End Sub
#End Region
    End Class
End Namespace

' ************************************************
' $History: ApplicationList.vb $
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 25.03.11   Time: 10:37
' Updated in $/CKAG/Base/Kernel/Admin
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 25.02.11   Time: 16:32
' Updated in $/CKAG/Base/Kernel/Admin
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 25.08.10   Time: 17:57
' Updated in $/CKAG/Base/Kernel/Admin
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 24.08.10   Time: 11:52
' Updated in $/CKAG/Base/Kernel/Admin
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 10.04.08   Time: 17:37
' Updated in $/CKAG/Base/Kernel/Admin
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 3.04.08    Time: 16:42
' Created in $/CKAG/Base/Kernel/Admin
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 6.12.07    Time: 12:47
' Updated in $/CKG/Base/Base/Kernel/Admin
' ITA:1440
' 
' *****************  Version 6  *****************
' User: Fassbenders  Date: 27.11.07   Time: 17:36
' Updated in $/CKG/Base/Base/Kernel/Admin
' 
' *****************  Version 5  *****************
' User: Uha          Date: 20.06.07   Time: 14:32
' Updated in $/CKG/Base/Base/Kernel/Admin
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 4  *****************
' User: Uha          Date: 1.03.07    Time: 16:32
' Updated in $/CKG/Base/Base/Kernel/Admin
' 
' ************************************************