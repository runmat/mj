Imports System.Web.UI.WebControls

Namespace Kernel
    Public Class EmployeeAssignments

        REM § Dient der Speicherung bzw. Löschung von Employeezuordnungen
        REM § bzw. deren Abhängigkeiten beim Speichern von Kunden/Gruppen.

#Region " Membervariables "
        Private m_intParameterID As Integer
        Private m_strParameterFeld As String
        Private m_strTabellenName As String
#End Region

#Region " Constructor "
        Public Sub New(ByVal intParameterID As Integer)
            m_intParameterID = intParameterID
            m_strParameterFeld = "GroupID"
            m_strTabellenName = "WebGroupEmployee"
        End Sub
#End Region

#Region " Functions "
        Public Sub Save(ByVal dvEmployeeAssigned As DataView, ByVal lstEmployeeAssigned As ListItemCollection, ByVal strConnectionString As String)
            Save(dvEmployeeAssigned, lstEmployeeAssigned, New SqlClient.SqlConnection(strConnectionString))
        End Sub
        Public Sub Save(ByVal dvEmployeeAssigned As DataView, ByVal lstEmployeeAssigned As ListItemCollection, ByVal cn As SqlClient.SqlConnection)
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            Dim cmd As New SqlClient.SqlCommand()
            cmd.Connection = cn

            If Not dvEmployeeAssigned Is Nothing Then
                'ermitteln, welche NICHT MEHR zugeordnet
                Dim strToDelete() As String = Nothing
                Dim intDel As Integer = 0
                Dim drv As DataRowView
                For Each drv In dvEmployeeAssigned
                    If lstEmployeeAssigned.FindByValue(drv("UserID").ToString) Is Nothing Then
                        ReDim Preserve strToDelete(intDel)
                        strToDelete(intDel) = drv("UserID").ToString
                        intDel += 1
                    End If
                Next
                If Not strToDelete Is Nothing Then
                    Dim strDelete As String = String.Format("DELETE " & _
                                                            "FROM {2} " & _
                                                            "WHERE {3}={1} " & _
                                                              "AND UserID IN " & _
                                                            "(SELECT UserID " & _
                                                             "FROM vwEmployee " & _
                                                             "WHERE (UserID IN ({0})) )", _
                                                            String.Join(", ", strToDelete), _
                                                            m_intParameterID, _
                                                            m_strTabellenName, _
                                                            m_strParameterFeld)
                    cmd.CommandText = strDelete
                    cmd.ExecuteNonQuery()
                End If
            End If

            Dim strToAdd() As String = Nothing
            Dim intAdd As Integer = 0
            Dim li As ListItem
            If dvEmployeeAssigned Is Nothing Or dvEmployeeAssigned.Table Is Nothing Then
                For Each li In lstEmployeeAssigned
                    ReDim Preserve strToAdd(intAdd)
                    strToAdd(intAdd) = li.Value.ToString
                    intAdd += 1
                Next
            Else
                dvEmployeeAssigned.Sort = "UserID"
                For Each li In lstEmployeeAssigned
                    If dvEmployeeAssigned.Find(li.Value) = -1 Then
                        ReDim Preserve strToAdd(intAdd)
                        strToAdd(intAdd) = li.Value.ToString
                        intAdd += 1
                    End If
                Next
            End If
            If Not strToAdd Is Nothing Then
                Dim strInsert As String = String.Format("INSERT INTO {2} ({3}, UserID) " & _
                                                        "SELECT {1}, UserID " & _
                                                        "FROM vwEmployee " & _
                                                        "WHERE (UserID IN ({0})) ", _
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

End Namespace