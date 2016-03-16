Imports System.Web.UI.WebControls

Namespace Kernel
    Public Class AppAssignments

        REM § Dient der Speicherung bzw. Löschung von Anwendungszuordnungen
        REM § bzw. deren Abhängigkeiten beim Speichern von Kunden/Gruppen.

#Region " Membervariables "
        Private m_intParameterID As Integer
        Private m_strParameterFeld As String
        Private m_strTabellenName As String
        Private m_atType As AssignmentType
#End Region

#Region " Constructor "
        Public Sub New(ByVal intParameterID As Integer, ByVal atType As AssignmentType)
            m_intParameterID = intParameterID
            m_atType = atType
            If m_atType = AssignmentType.Customer Then
                m_strParameterFeld = "CustomerID"
                m_strTabellenName = "CustomerRights"
            Else
                m_strParameterFeld = "GroupID"
                m_strTabellenName = "Rights"
            End If
        End Sub
#End Region

#Region " Functions "

        Public Sub Save(ByVal dvAppOriginallyAssigned As DataView, ByVal lstAppNowAssigned As List(Of String), ByVal cn As SqlClient.SqlConnection)
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            Dim cmd As New SqlClient.SqlCommand()
            cmd.Connection = cn

            If Not dvAppOriginallyAssigned Is Nothing Then
                'ermitteln, welche NICHT MEHR zugeordnet
                Dim strToDelete() As String = Nothing
                Dim intDel As Integer = 0
                Dim drv As DataRowView
                For Each drv In dvAppOriginallyAssigned
                    If Not lstAppNowAssigned.Contains(drv("AppId").ToString) Then
                        ReDim Preserve strToDelete(intDel)
                        strToDelete(intDel) = drv("AppId").ToString
                        intDel += 1
                    End If
                Next
                If Not strToDelete Is Nothing Then
                    Dim strDelete As String = String.Format("DELETE " & _
                                                            "FROM {2} " & _
                                                            "WHERE {3}={1} " & _
                                                              "AND AppID IN " & _
                                                            "(SELECT AppID " & _
                                                             "FROM Application " & _
                                                             "WHERE (AppID IN ({0})) " & _
                                                                "OR (AppParent IN ({0})))", _
                                                            String.Join(", ", strToDelete), _
                                                            m_intParameterID, _
                                                            m_strTabellenName, _
                                                            m_strParameterFeld)
                    cmd.CommandText = strDelete
                    cmd.ExecuteNonQuery()
                End If

                'Wenn AssignmentType Customer ist, dann muessen auch die Anwendungen in den Gruppen,
                'die zu dem Customer gehoeren geloescht werden.
                '***********************************************************************************
                If m_atType = AssignmentType.Customer Then
                    If Not strToDelete Is Nothing Then
                        Dim strDelete As String = String.Format("DELETE " & _
                                                                "FROM Rights " & _
                                                                "WHERE GroupID IN " & _
                                                                "(SELECT GroupID " & _
                                                                 "FROM WebGroup " & _
                                                                 "WHERE CustomerID={1}) " & _
                                                                  "AND AppID IN " & _
                                                                "(SELECT AppID " & _
                                                                 "FROM Application " & _
                                                                 "WHERE (AppID IN ({0})) " & _
                                                                    "OR (AppParent IN ({0})))", _
                                                                String.Join(", ", strToDelete), _
                                                                m_intParameterID)
                        cmd.CommandText = strDelete
                        cmd.ExecuteNonQuery()
                    End If
                End If
                '***********************************************************************************
            End If

            Dim strToAdd() As String = Nothing
            Dim intAdd As Integer = 0
            If dvAppOriginallyAssigned Is Nothing OrElse dvAppOriginallyAssigned.Table Is Nothing Then
                For Each li As String In lstAppNowAssigned
                    ReDim Preserve strToAdd(intAdd)
                    strToAdd(intAdd) = li
                    intAdd += 1
                Next
            Else
                dvAppOriginallyAssigned.Sort = "AppID"
                For Each li As String In lstAppNowAssigned
                    If dvAppOriginallyAssigned.Find(li) = -1 Then
                        ReDim Preserve strToAdd(intAdd)
                        strToAdd(intAdd) = li
                        intAdd += 1
                    End If
                Next
            End If
            If Not strToAdd Is Nothing Then
                Dim strInsert As String = String.Format("INSERT INTO {2} ({3}, AppID) " & _
                                                        "SELECT {1}, AppID " & _
                                                        "FROM Application " & _
                                                        "WHERE (AppID IN ({0})) " & _
                                                           "OR (AppParent IN ({0}))", _
                                                        String.Join(", ", strToAdd), _
                                                        m_intParameterID, _
                                                        m_strTabellenName, _
                                                        m_strParameterFeld)
                cmd.CommandText = strInsert
                cmd.ExecuteNonQuery()
            End If
        End Sub
#End Region

    End Class

    Public Enum AssignmentType
        Customer = 1
        Group = 0
    End Enum
End Namespace