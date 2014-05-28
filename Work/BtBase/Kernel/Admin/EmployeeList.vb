Namespace Kernel.Admin
    Public Class EmployeeList
        REM § Liste der im jeweiligen Kontext gültigen Anwendungen

        Inherits DataTable

#Region " Membervariables "
        Private m_cn As SqlClient.SqlConnection
        Private m_intAccountingArea As Integer
        Private m_intGroupID As Integer
        Private m_blnTableFilled As Boolean = False
#End Region

#Region " Properties"

#End Region

#Region " Constructor "
        Public Sub New(ByVal intAccountingArea As Integer, ByVal strConnectionString As String)
            Me.New(intAccountingArea, New SqlClient.SqlConnection(strConnectionString))
        End Sub
        Public Sub New(ByVal intAccountingArea As Integer, ByVal cn As SqlClient.SqlConnection)
            Try
                If cn.State = ConnectionState.Closed Then
                    cn.Open()
                End If
                m_cn = cn
                m_intAccountingArea = intAccountingArea
                m_intGroupID = -2 'Indikator dafuer, dass nicht nach Gruppe gesucht wird.
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End Sub
        Public Sub New(ByVal intGroupID As Integer, ByVal intAccountingArea As Integer, ByVal strConnectionString As String)
            Me.New(intGroupID, intAccountingArea, New SqlClient.SqlConnection(strConnectionString))
        End Sub
        Public Sub New(ByVal intGroupID As Integer, ByVal intAccountingArea As Integer, ByVal cn As SqlClient.SqlConnection)
            Try
                If cn.State = ConnectionState.Closed Then
                    cn.Open()
                End If
                m_cn = cn
                m_intAccountingArea = intAccountingArea
                m_intGroupID = intGroupID
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End Sub
        Public Sub New(ByVal strUsername As String, ByVal strConnectionString As String, ByVal intAccountingArea As Integer)
            Me.New(strUsername, New SqlClient.SqlConnection(strConnectionString), intAccountingArea)
        End Sub
        Public Sub New(ByVal strUsername As String, ByVal cn As SqlClient.SqlConnection, ByVal intAccountingArea As Integer)
            Try
                If strUsername = String.Empty Then strUsername = "%"
                If cn.State = ConnectionState.Closed Then
                    cn.Open()
                End If
                Dim daApp As New SqlClient.SqlDataAdapter("SELECT " & _
                                                                    "UserID, " & _
                                                                    "AccountingArea, " & _
                                                                    "Username, " & _
                                                                    "FirstName, " & _
                                                                    "LastName, " & _
                                                                    "LastName + ',' + FirstName + ' (' + [position] + ')' AS EmployeeName, " & _
                                                                    "Title, " & _
                                                                    "mail, " & _
                                                                    "picture, " & _
                                                                    "employeeHierarchy, " & _
                                                                    "department, " & _
                                                                    "[position], " & _
                                                                    "telephone, " & _
                                                                    "fax " & _
                                                            "FROM vwEmployee " & _
                                                            "WHERE Username LIKE @Username " & _
                                                            "AND AccountingArea = @AccountingArea", cn)
                daApp.SelectCommand.Parameters.AddWithValue("@Username", Replace(strUsername, "*", "%"))
                daApp.SelectCommand.Parameters.AddWithValue("@AccountingArea", intAccountingArea)
                daApp.Fill(Me)
                m_blnTableFilled = True
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End Sub
        Public Sub New(ByVal strConnectionString As String, ByVal intAccountingArea As Integer)
            Me.New(New SqlClient.SqlConnection(strConnectionString), intAccountingArea)
        End Sub
        Public Sub New(ByVal cn As SqlClient.SqlConnection, ByVal intAccountingArea As Integer)
            Try
                If cn.State = ConnectionState.Closed Then
                    cn.Open()
                End If
                Dim daApp As New SqlClient.SqlDataAdapter("SELECT " & _
                                                                    "UserID, " & _
                                                                    "AccountingArea, " & _
                                                                    "Username, " & _
                                                                    "FirstName, " & _
                                                                    "LastName, " & _
                                                                    "LastName + ',' + FirstName + ' (' + [position] + ')' AS EmployeeName, " & _
                                                                    "Title, " & _
                                                                    "mail, " & _
                                                                    "picture, " & _
                                                                    "employeeHierarchy, " & _
                                                                    "department, " & _
                                                                    "[position], " & _
                                                                    "telephone, " & _
                                                                    "fax " & _
                                                            "FROM vwEmployee " & _
                                                            "WHERE AccountingArea = @AccountingArea", cn)
                daApp.SelectCommand.Parameters.AddWithValue("@AccountingArea", intAccountingArea)
                daApp.Fill(Me)
                Dim dr As DataRow
                dr = NewRow()
                dr("EmployeeID") = 0
                dr("EasyEmployeeName") = " - keines - "
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
        Public Sub GetAssigned()
            If Not m_blnTableFilled Then
                If m_intGroupID = -1 Then
                    'AccountingArea/Gruppe ist neu: Keine zugewiesen.
                    m_blnTableFilled = True
                    Exit Sub
                End If

                Dim daGroups As New SqlClient.SqlDataAdapter()
                daGroups.SelectCommand = New SqlClient.SqlCommand()
                daGroups.SelectCommand.Connection = m_cn

                Dim strSQL As String
                If m_intGroupID = -2 Then
                    'Es wird nur nach AccountingArea gesucht, nicht nach Gruppe
                    If m_intAccountingArea = -1 Then
                        strSQL = "SELECT " & _
                                        "UserID, " & _
                                        "AccountingArea, " & _
                                        "Username, " & _
                                        "FirstName, " & _
                                        "LastName, " & _
                                        "LastName + ',' + FirstName + ' (' + [position] + ')' AS EmployeeName, " & _
                                        "Title, " & _
                                        "mail, " & _
                                        "picture, " & _
                                        "employeeHierarchy, " & _
                                        "department, " & _
                                        "[position], " & _
                                        "telephone, " & _
                                        "fax " & _
                                "FROM vwEmployee "
                    Else
                        strSQL = "SELECT " & _
                                        "UserID, " & _
                                        "AccountingArea, " & _
                                        "Username, " & _
                                        "FirstName, " & _
                                        "LastName, " & _
                                        "LastName + ',' + FirstName + ' (' + [position] + ')' AS EmployeeName, " & _
                                        "Title, " & _
                                        "mail, " & _
                                        "picture, " & _
                                        "employeeHierarchy, " & _
                                        "department, " & _
                                        "[position], " & _
                                        "telephone, " & _
                                        "fax " & _
                                "FROM vwEmployee " & _
                                "WHERE AccountingArea = @AccountingArea"
                    End If
                Else
                    If m_intAccountingArea = -1 Then
                        strSQL = "SELECT " & _
                                        "UserID, " & _
                                        "AccountingArea, " & _
                                        "Username, " & _
                                        "FirstName, " & _
                                        "LastName, " & _
                                        "EmployeeName, " & _
                                        "Title, " & _
                                        "mail, " & _
                                        "picture, " & _
                                        "employeeHierarchy, " & _
                                        "department, " & _
                                        "[position], " & _
                                        "telephone, " & _
                                        "fax, " & _
                                        "PictureName " & _
                                "FROM vwGroupEmployeeAssigned " & _
                                 "WHERE (GroupID = @GroupID) "
                    Else
                        strSQL = "SELECT " & _
                                        "UserID, " & _
                                        "AccountingArea, " & _
                                        "Username, " & _
                                        "FirstName, " & _
                                        "LastName, " & _
                                        "EmployeeName, " & _
                                        "Title, " & _
                                        "mail, " & _
                                        "picture, " & _
                                        "employeeHierarchy, " & _
                                        "department, " & _
                                        "[position], " & _
                                        "telephone, " & _
                                        "fax, " & _
                                        "PictureName " & _
                                "FROM vwGroupEmployeeAssigned " & _
                                 "WHERE (AccountingArea = @AccountingArea) " & _
                                   "AND (GroupID = @GroupID) "
                    End If
                    daGroups.SelectCommand.Parameters.AddWithValue("@GroupID", m_intGroupID)
                End If
                daGroups.SelectCommand.CommandText = strSQL
                daGroups.SelectCommand.Parameters.AddWithValue("@AccountingArea", m_intAccountingArea)
                daGroups.Fill(Me)
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
                    'Gruppe ist neu: Zeige alle die AccountingArea hat.
                    strSQL = "SELECT " & _
                                    "UserID, " & _
                                    "AccountingArea, " & _
                                    "Username, " & _
                                    "FirstName, " & _
                                    "LastName, " & _
                                    "LastName + ',' + FirstName + ' (' + [position] + ')' AS EmployeeName, " & _
                                    "Title, " & _
                                    "mail, " & _
                                    "picture, " & _
                                    "employeeHierarchy, " & _
                                    "department, " & _
                                    "[position], " & _
                                    "telephone, " & _
                                    "fax " & _
                            "FROM vwEmployee " & _
                            "WHERE AccountingArea = @AccountingArea"
                ElseIf m_intGroupID = -2 Then
                    'Es wird nur nach AccountingArea gesucht, nicht nach Gruppe
                    If m_intAccountingArea = -1 Then
                        strSQL = "SELECT " & _
                                        "UserID, " & _
                                        "AccountingArea, " & _
                                        "Username, " & _
                                        "FirstName, " & _
                                        "LastName + ',' + FirstName + ' (' + [position] + ')' AS EmployeeName, " & _
                                        "LastName, " & _
                                        "Title, " & _
                                        "mail, " & _
                                        "picture, " & _
                                        "employeeHierarchy, " & _
                                        "department, " & _
                                        "[position], " & _
                                        "telephone, " & _
                                        "fax " & _
                                "FROM vwEmployee "
                    Else
                        If m_intAccountingArea = -1 Then
                            strSQL = "SELECT " & _
                                            "UserID, " & _
                                            "AccountingArea, " & _
                                            "Username, " & _
                                            "FirstName, " & _
                                            "LastName, " & _
                                            "LastName + ',' + FirstName + ' (' + [position] + ')' AS EmployeeName, " & _
                                            "Title, " & _
                                            "mail, " & _
                                            "picture, " & _
                                            "employeeHierarchy, " & _
                                            "department, " & _
                                            "[position], " & _
                                            "telephone, " & _
                                            "fax " & _
                                    "FROM vwEmployee "
                        Else
                            strSQL = "SELECT " & _
                                            "UserID, " & _
                                            "AccountingArea, " & _
                                            "Username, " & _
                                            "FirstName, " & _
                                            "LastName, " & _
                                            "LastName + ',' + FirstName + ' (' + [position] + ')' AS EmployeeName, " & _
                                            "Title, " & _
                                            "mail, " & _
                                            "picture, " & _
                                            "employeeHierarchy, " & _
                                            "department, " & _
                                            "[position], " & _
                                            "telephone, " & _
                                            "fax " & _
                                    "FROM vwEmployee " & _
                                    "WHERE AccountingArea = @AccountingArea"
                        End If
                    End If
                Else
                    If m_intAccountingArea = -1 Then
                        strSQL = "SELECT " & _
                                            "UserID, " & _
                                            "AccountingArea, " & _
                                            "Username, " & _
                                            "FirstName, " & _
                                            "LastName, " & _
                                            "LastName + ',' + FirstName + ' (' + [position] + ')' AS EmployeeName, " & _
                                            "Title, " & _
                                            "mail, " & _
                                            "picture, " & _
                                            "employeeHierarchy, " & _
                                            "department, " & _
                                            "[position], " & _
                                            "telephone, " & _
                                            "fax " & _
                                    "FROM vwGroupEmployeeUnAssigned " & _
                                 "WHERE (GroupID = @GroupID) "
                    Else
                        strSQL = "SELECT " & _
                                            "UserID, " & _
                                            "AccountingArea, " & _
                                            "Username, " & _
                                            "FirstName, " & _
                                            "LastName, " & _
                                            "LastName + ',' + FirstName + ' (' + [position] + ')' AS EmployeeName, " & _
                                            "Title, " & _
                                            "mail, " & _
                                            "picture, " & _
                                            "employeeHierarchy, " & _
                                            "department, " & _
                                            "[position], " & _
                                            "telephone, " & _
                                            "fax " & _
                                    "FROM vwGroupEmployeeUnAssigned " & _
                                 "WHERE (AccountingArea = @AccountingArea) " & _
                                   "AND (GroupID = @GroupID) "
                    End If
                    daGroups.SelectCommand.Parameters.AddWithValue("@GroupID", m_intGroupID)
                End If
                daGroups.SelectCommand.CommandText = strSQL
                daGroups.SelectCommand.Parameters.AddWithValue("@AccountingArea", m_intAccountingArea)
                daGroups.Fill(Me)
                m_blnTableFilled = True
            End If
        End Sub

        Public Sub AddAllNone(ByVal blnAll As Boolean, ByVal blnNone As Boolean)
            Dim dr As DataRow
            If blnAll Then
                dr = NewRow()
                dr("EmployeeID") = 0
                dr("EasyEmployeeName") = " - alle - "
                Rows.Add(dr)
            End If
            If blnNone Then
                dr = NewRow()
                dr("EmployeeID") = -1
                dr("EasyEmployeeName") = " - keine - "
                Rows.Add(dr)
            End If
        End Sub
#End Region
    End Class
End Namespace

' ************************************************
' $History: EmployeeList.vb $
' 
' *****************  Version 2  *****************
' User: Hartmannu    Date: 9.09.08    Time: 15:21
' Updated in $/CKAG/Base/Kernel/Admin
' 
' *****************  Version 1  *****************
' User: Hartmannu    Date: 9.09.08    Time: 13:42
' Created in $/CKAG/Base/Kernel/Admin
' ITA 2152 und 2158
' 

' ************************************************