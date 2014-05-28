Imports System.Web.UI.WebControls

Namespace Kernel
    Public Class ArchivAssignments

        REM § Dient der Speicherung bzw. Löschung von Archivzuordnungen
        REM § bzw. deren Abhängigkeiten beim Speichern von Kunden/Gruppen.

#Region " Membervariables "
        Private m_intParameterID As Integer
        Private m_strParameterFeld As String
        Private m_strTabellenName As String
        Private m_atType As ArchiveAssignmentType
#End Region

#Region " Constructor "
        Public Sub New(ByVal intParameterID As Integer, ByVal atType As ArchiveAssignmentType)
            m_intParameterID = intParameterID
            m_atType = atType
            If m_atType = AssignmentType.Customer Then
                m_strParameterFeld = "CustomerID"
                m_strTabellenName = "ArchiveRights"
            Else
                m_strParameterFeld = "GroupID"
                m_strTabellenName = "WebGroupArchives"
            End If
        End Sub
#End Region

#Region " Functions "
        Public Sub Save(ByVal dvArchivAssigned As DataView, ByVal lstArchivAssigned As ListItemCollection, ByVal strConnectionString As String)
            Save(dvArchivAssigned, lstArchivAssigned, New SqlClient.SqlConnection(strConnectionString))
        End Sub
        Public Sub Save(ByVal dvArchivAssigned As DataView, ByVal lstArchivAssigned As ListItemCollection, ByVal cn As SqlClient.SqlConnection)
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            Dim cmd As New SqlClient.SqlCommand()
            cmd.Connection = cn

            If Not dvArchivAssigned Is Nothing Then
                'ermitteln, welche NICHT MEHR zugeordnet
                Dim strToDelete() As String = Nothing
                Dim intDel As Integer = 0
                Dim drv As DataRowView
                For Each drv In dvArchivAssigned
                    If lstArchivAssigned.FindByValue(drv("ArchivId").ToString) Is Nothing Then
                        ReDim Preserve strToDelete(intDel)
                        strToDelete(intDel) = drv("ArchivId").ToString
                        intDel += 1
                    End If
                Next
                If Not strToDelete Is Nothing Then
                    Dim strDelete As String = String.Format("DELETE " & _
                                                            "FROM {2} " & _
                                                            "WHERE {3}={1} " & _
                                                              "AND ArchivID IN " & _
                                                            "(SELECT ArchivID " & _
                                                             "FROM Archiv " & _
                                                             "WHERE (ArchivID IN ({0})) )", _
                                                            String.Join(", ", strToDelete), _
                                                            m_intParameterID, _
                                                            m_strTabellenName, _
                                                            m_strParameterFeld)
                    cmd.CommandText = strDelete
                    cmd.ExecuteNonQuery()
                End If

                'Wenn AssignmentType Customer ist, dann muessen auch die Archive in den Gruppen,
                'die zu dem Customer gehoeren geloescht werden.
                '***********************************************************************************
                If m_atType = AssignmentType.Customer Then
                    If Not strToDelete Is Nothing Then
                        Dim strDelete As String = String.Format("DELETE " & _
                                                                "FROM WebGroupArchives " & _
                                                                "WHERE GroupID IN " & _
                                                                "(SELECT GroupID " & _
                                                                 "FROM WebGroup " & _
                                                                 "WHERE CustomerID={1}) " & _
                                                                  "AND ArchivID IN " & _
                                                                "(SELECT ArchivID " & _
                                                                 "FROM Archiv " & _
                                                                 "WHERE (ArchivID IN ({0})) )", _
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
            Dim li As ListItem
            If dvArchivAssigned Is Nothing Or dvArchivAssigned.Table Is Nothing Then
                For Each li In lstArchivAssigned
                    ReDim Preserve strToAdd(intAdd)
                    strToAdd(intAdd) = li.Value.ToString
                    intAdd += 1
                Next
            Else
                dvArchivAssigned.Sort = "ArchivID"
                For Each li In lstArchivAssigned
                    If dvArchivAssigned.Find(li.Value) = -1 Then
                        ReDim Preserve strToAdd(intAdd)
                        strToAdd(intAdd) = li.Value.ToString
                        intAdd += 1
                    End If
                Next
            End If
            If Not strToAdd Is Nothing Then
                Dim strInsert As String = String.Format("INSERT INTO {2} ({3}, ArchivID) " & _
                                                        "SELECT {1}, ArchivID " & _
                                                        "FROM Archiv " & _
                                                        "WHERE (ArchivID IN ({0})) ", _
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

    Public Enum ArchiveAssignmentType
        Customer = 1
        Group = 0
    End Enum
End Namespace