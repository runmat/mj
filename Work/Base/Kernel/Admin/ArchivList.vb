Namespace Kernel.Admin
    Public Class ArchivList
        REM § Liste der im jeweiligen Kontext gültigen Anwendungen

        Inherits DataTable

#Region " Membervariables "
        Private m_cn As SqlClient.SqlConnection
        Private m_intCustomerID As Integer
        Private m_intGroupID As Integer
        Private m_blnTableFilled As Boolean = False
#End Region

#Region " Properties"

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
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End Sub
        Public Sub New(ByVal strAppNameFilter As String, ByVal strConnectionString As String)
            Me.New(strAppNameFilter, New SqlClient.SqlConnection(strConnectionString))
        End Sub
        Public Sub New(ByVal strEasyArchivNameFilter As String, ByVal cn As SqlClient.SqlConnection)
            Try
                If strEasyArchivNameFilter = String.Empty Then strEasyArchivNameFilter = "%"
                If cn.State = ConnectionState.Closed Then
                    cn.Open()
                End If
                Dim daApp As New SqlClient.SqlDataAdapter("SELECT    ArchivID, " & _
                                                                    "EasyLagerortName, " & _
                                                                    "EasyArchivName, " & _
                                                                    "EasyQueryIndex, " & _
                                                                    "EasyQueryIndexName, " & _
                                                                    "EasyTitleName, " & _
                                                                    "DefaultQuery, " & _
                                                                    "Archivetype, " & _
                                                                    "SortOrder " & _
                                                            "FROM Archiv " & _
                                                            "WHERE EasyArchivName LIKE @EasyArchivName", cn)
                daApp.SelectCommand.Parameters.AddWithValue("@EasyArchivName", Replace(strEasyArchivNameFilter, "*", "%"))
                daApp.Fill(Me)
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
                Dim daApp As New SqlClient.SqlDataAdapter("SELECT    ArchivID, " & _
                                                                    "EasyLagerortName, " & _
                                                                    "EasyArchivName, " & _
                                                                    "EasyQueryIndex, " & _
                                                                    "EasyQueryIndexName, " & _
                                                                    "EasyTitleName, " & _
                                                                    "DefaultQuery, " & _
                                                                    "Archivetype, " & _
                                                                    "SortOrder " & _
                                                            "FROM Archiv", cn)
                daApp.Fill(Me)
                Dim dr As DataRow
                dr = NewRow()
                dr("ArchivID") = 0
                dr("EasyArchivName") = " - keines - "
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
                    strSQL = "SELECT ArchivID, EasyLagerortName, EasyArchivName, EasyQueryIndex, EasyQueryIndexName, EasyTitleName, DefaultQuery, Archivetype, SortOrder " & _
                                              "FROM vwCustomerArchivAssigned " & _
                                              "WHERE (CustomerID = @CustomerID) "
                Else
                    strSQL = "SELECT ArchivID, EasyLagerortName, EasyArchivName, EasyQueryIndex, EasyQueryIndexName, EasyTitleName, DefaultQuery, Archivetype, SortOrder " & _
                             "FROM vwGroupArchivAssigned " & _
                             "WHERE (CustomerID = @CustomerID) " & _
                               "AND (GroupID = @GroupID) "
                    daGroups.SelectCommand.Parameters.AddWithValue("@GroupID", m_intGroupID)
                End If
                daGroups.SelectCommand.CommandText = strSQL
                daGroups.SelectCommand.Parameters.AddWithValue("@CustomerID", m_intCustomerID)
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
                    'Gruppe ist neu: Zeige alle die Customer hat.
                    strSQL = "SELECT ArchivID, EasyLagerortName, EasyArchivName, EasyQueryIndex, EasyQueryIndexName, EasyTitleName, DefaultQuery, Archivetype, SortOrder " & _
                             "FROM vwCustomerArchivAssigned " & _
                             "WHERE (CustomerID = @CustomerID) "
                ElseIf m_intGroupID = -2 Then
                    'Es wird nur nach Customer gesucht, nicht nach Gruppe
                    If m_intCustomerID = -1 Then
                        strSQL = "SELECT ArchivID, EasyLagerortName, EasyArchivName, EasyQueryIndex, EasyQueryIndexName, EasyTitleName, DefaultQuery, Archivetype, SortOrder " & _
                                 "FROM Archiv "
                    Else
                        strSQL = "SELECT ArchivID, EasyLagerortName, EasyArchivName, EasyQueryIndex, EasyQueryIndexName, EasyTitleName, DefaultQuery, Archivetype, SortOrder " & _
                                 "FROM vwCustomerArchivUnAssigned " & _
                                 "WHERE (CustomerID = @CustomerID) "
                    End If
                Else
                    strSQL = "SELECT ArchivID, EasyLagerortName, EasyArchivName, EasyQueryIndex, EasyQueryIndexName, EasyTitleName, DefaultQuery, Archivetype, SortOrder " & _
                             "FROM vwGroupArchivUnAssigned " & _
                             "WHERE (CustomerID = @CustomerID) " & _
                               "AND (GroupID = @GroupID) "
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
                dr("ArchivID") = 0
                dr("EasyArchivName") = " - alle - "
                Rows.Add(dr)
            End If
            If blnNone Then
                dr = NewRow()
                dr("ArchivID") = -1
                dr("EasyArchivName") = " - keine - "
                Rows.Add(dr)
            End If
        End Sub
#End Region
    End Class
End Namespace

' ************************************************
' $History: ArchivList.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 11.04.08   Time: 11:57
' Updated in $/CKAG/Base/Kernel/Admin
' Migration OR
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 3.04.08    Time: 16:42
' Created in $/CKAG/Base/Kernel/Admin
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 6.12.07    Time: 12:47
' Updated in $/CKG/Base/Base/Kernel/Admin
' ITA:1440
' 
' *****************  Version 1  *****************
' User: Uha          Date: 27.08.07   Time: 17:13
' Created in $/CKG/Base/Base/Kernel/Admin
' ITA 1269: Archivverwaltung/-zuordnung jetzt auch per Web
' 
' ************************************************