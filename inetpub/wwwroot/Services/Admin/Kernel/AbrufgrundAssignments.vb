Imports System.Web.UI.WebControls

Namespace Kernel
    Public Class AbrufgrundAssignments

        REM § Dient der Speicherung bzw. Löschung von Abrufgrundzuordnungenzuordnungen
        REM § bzw. deren Abhängigkeiten beim Speichern von Kunden/Gruppen.

#Region " Membervariables "

        Private m_intCustomerID As Integer
        Private m_intGroupID As Integer

#End Region

#Region " Constructor "

        Public Sub New(ByVal intCustomerID As Integer, ByVal intGroupID As Integer)
            m_intCustomerID = intCustomerID
            m_intGroupID = intGroupID
        End Sub

#End Region

#Region " Functions "

        Public Sub Save(ByVal dvAbrufgruendeAssigned As DataView, ByVal lstAbrufgruendeAssigned As ListItemCollection, ByVal abruftyp As String, ByVal cn As SqlClient.SqlConnection)
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            Dim cmd As New SqlClient.SqlCommand()
            cmd.Connection = cn

            If Not dvAbrufgruendeAssigned Is Nothing Then
                'ermitteln, welche NICHT MEHR zugeordnet
                Dim strToDelete() As String = Nothing
                Dim intDel As Integer = 0
                For Each drv As DataRowView In dvAbrufgruendeAssigned
                    If lstAbrufgruendeAssigned.FindByValue(drv("SapWert").ToString) Is Nothing Then
                        ReDim Preserve strToDelete(intDel)
                        strToDelete(intDel) = drv("SapWert").ToString
                        intDel += 1
                    End If
                Next
                If Not strToDelete Is Nothing Then
                    Dim strDelete As String = String.Format("DELETE FROM CustomerAbrufgruende WHERE CustomerID = {0} AND GroupID = {1} AND AbrufTyp = '{2}' AND SapWert IN ({3}) ", _
                                                            m_intCustomerID, m_intGroupID, abruftyp, String.Join(", ", strToDelete))
                    cmd.CommandText = strDelete
                    cmd.ExecuteNonQuery()
                End If
            End If

            Dim strToAdd() As String = Nothing
            Dim intAdd As Integer = 0
            If dvAbrufgruendeAssigned Is Nothing Or dvAbrufgruendeAssigned.Table Is Nothing Then
                For Each li As ListItem In lstAbrufgruendeAssigned
                    ReDim Preserve strToAdd(intAdd)
                    strToAdd(intAdd) = li.Value.ToString
                    intAdd += 1
                Next
            Else
                dvAbrufgruendeAssigned.Sort = "SapWert"
                For Each li As ListItem In lstAbrufgruendeAssigned
                    If dvAbrufgruendeAssigned.Find(li.Value) = -1 Then
                        ReDim Preserve strToAdd(intAdd)
                        strToAdd(intAdd) = li.Value.ToString
                        intAdd += 1
                    End If
                Next
            End If
            If Not strToAdd Is Nothing Then
                Dim strInsert As String = String.Format("INSERT INTO CustomerAbrufgruende (CustomerID, GroupID, AbrufTyp, WebBezeichnung, " & _
                                                        " SapWert, MitZusatzText, Zusatzbemerkung, VersandadressArt, Eingeschraenkt) " & _
                                                        " SELECT CustomerID, {0}, AbrufTyp, WebBezeichnung, " & _
                                                        " SapWert, MitZusatzText, Zusatzbemerkung, VersandadressArt, Eingeschraenkt " & _
                                                        " FROM CustomerAbrufgruende WHERE CustomerID = {1} AND GroupID = 0 AND AbrufTyp = '{2}' AND SapWert IN ({3}) ", _
                                                        m_intGroupID, m_intCustomerID, abruftyp, String.Join(", ", strToAdd))
                cmd.CommandText = strInsert
                cmd.ExecuteNonQuery()
            End If
        End Sub

#End Region

    End Class
End Namespace