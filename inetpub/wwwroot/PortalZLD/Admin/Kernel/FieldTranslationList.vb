Namespace Kernel
    Public Class FieldTranslationList
        REM § Liest Übersetzungen von Formularfeldern pro ASPX-Seite (Anwendungs-URL)

        Inherits DataTable

#Region " Constructor "
        'liest die Feldübersetzung anhand des Ausgewählten Kunden um Diese dann im Grid anzeigen zu können. JJ 2007.11.12
        Public Sub New(ByVal strAppURL As String, ByVal intCustomerID As Integer, ByVal intLanguageID As Integer, ByVal strConnectionString As String)
            Me.New(strAppURL, intCustomerID, intLanguageID, New SqlClient.SqlConnection(strConnectionString))
        End Sub
        Public Sub New(ByVal strAppURL As String, ByVal intCustomerID As Integer, ByVal intLanguageID As Integer, ByVal cn As SqlClient.SqlConnection, Optional ByVal intGroupID As Integer = 0)
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            Dim daFields As New SqlClient.SqlDataAdapter( _
                                    "SELECT TOP 100 PERCENT " & _
                                    "[ApplicationFieldID], " & _
                                    "[FieldType], " & _
                                    "[FieldName], " & _
                                    "[FieldType] + [FieldName] AS [Field], " & _
                                    "[CustomerID], " & _
                                    "[GroupID], " & _
                                    "[LanguageID], " & _
                                    "[Visibility], " & _
                                    "[IsInputField], " & _
                                    "ISNULL([Content], '') AS [Content] " & _
                                    "FROM ApplicationField " & _
                                    "WHERE ([AppURL] = @AppURL) " & _
                                    "AND ([CustomerID] IN (1, @CustomerID)) " & _
                                    "AND ([GroupID] IN (0, @GroupID)) " & _
                                    "AND ([LanguageID] IN (1, @LanguageID)) " & _
                                    "ORDER BY " & _
                                    "[FieldType], " & _
                                    "[FieldName], " & _
                                    "[CustomerID], " & _
                                    "[GroupID], " & _
                                    "[LanguageID] " _
                                    , cn)
            daFields.SelectCommand.Parameters.AddWithValue("@AppURL", strAppURL)
            daFields.SelectCommand.Parameters.AddWithValue("@CustomerID", intCustomerID)
            daFields.SelectCommand.Parameters.AddWithValue("@GroupID", intGroupID)
            daFields.SelectCommand.Parameters.AddWithValue("@LanguageID", intLanguageID)
            daFields.Fill(Me)

            Me.Columns.Add("Standard", System.Type.GetType("System.String"))

            If Not Me Is Nothing AndAlso Me.Rows.Count > 0 Then
                Dim i As Integer
                Dim strType As String = "X"
                Dim strName As String = "X"

                For i = Me.Rows.Count - 1 To 0 Step -1
                    If strType = CStr(Me.Rows(i)("FieldType")) And strName = CStr(Me.Rows(i)("FieldName")) Then
                        Me.Rows.RemoveAt(i)

                    ElseIf CBool(Me.Rows(i)("IsInputField") = True) Then
                        strType = CStr(Me.Rows(i)("FieldType"))
                        strName = CStr(Me.Rows(i)("FieldName"))
                        Me.Rows.RemoveAt(i)
                    Else
                        strType = CStr(Me.Rows(i)("FieldType"))
                        strName = CStr(Me.Rows(i)("FieldName"))
                        If Not ((CInt(Me.Rows(i)("CustomerID")) = intCustomerID) And (CInt(Me.Rows(i)("GroupID")) = intGroupID) And (CInt(Me.Rows(i)("GroupID")) = intGroupID)) Then
                            Me.Rows(i)("ApplicationFieldID") = -1
                            Me.Rows(i)("Standard") = CStr(Me.Rows(i)("Content"))
                            Me.Rows(i)("Content") = ""
                        Else
                            Me.Rows(i)("Standard") = ""
                        End If
                    End If
                Next
            End If
        End Sub
        Public Sub New(ByVal strAppURL As String, ByVal intCustomerID As Integer, ByVal intLanguageID As Integer, ByVal cn As SqlClient.SqlConnection, ByVal Master As String, Optional ByVal intGroupID As Integer = 0)
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            Dim daFields As New SqlClient.SqlDataAdapter( _
                                    "SELECT TOP 100 PERCENT " & _
                                    "[ApplicationFieldID], " & _
                                    "[FieldType], " & _
                                    "[FieldName], " & _
                                    "[FieldType] + [FieldName] AS [Field], " & _
                                    "[CustomerID], " & _
                                    "[GroupID], " & _
                                    "[LanguageID], " & _
                                    "[Visibility], " & _
                                    "[IsInputField], " & _
                                    "ISNULL([Content], '') AS [Content] " & _
                                    "FROM ApplicationField " & _
                                    "WHERE ([AppURL] = @AppURL) " & _
                                    "AND ([CustomerID] IN (1, @CustomerID)) " & _
                                    "AND ([GroupID] IN (0, @GroupID)) " & _
                                    "AND ([LanguageID] IN (1, @LanguageID)) " & _
                                    "ORDER BY " & _
                                    "[FieldType], " & _
                                    "[FieldName], " & _
                                    "[CustomerID], " & _
                                    "[GroupID], " & _
                                    "[LanguageID] " _
                                    , cn)
            daFields.SelectCommand.Parameters.AddWithValue("@AppURL", strAppURL)
            daFields.SelectCommand.Parameters.AddWithValue("@CustomerID", intCustomerID)
            daFields.SelectCommand.Parameters.AddWithValue("@GroupID", intGroupID)
            daFields.SelectCommand.Parameters.AddWithValue("@LanguageID", intLanguageID)
            daFields.Fill(Me)

            Me.Columns.Add("Standard", System.Type.GetType("System.String"))

            If Not Me Is Nothing AndAlso Me.Rows.Count > 0 Then
                Dim i As Integer
                Dim strType As String = "X"
                Dim strName As String = "X"

                For i = Me.Rows.Count - 1 To 0 Step -1
                    If strType = CStr(Me.Rows(i)("FieldType")) And strName = CStr(Me.Rows(i)("FieldName")) Then
                        Me.Rows.RemoveAt(i)
                    Else
                        strType = CStr(Me.Rows(i)("FieldType"))
                        strName = CStr(Me.Rows(i)("FieldName"))
                        If Not ((CInt(Me.Rows(i)("CustomerID")) = intCustomerID) And (CInt(Me.Rows(i)("GroupID")) = intGroupID) And (CInt(Me.Rows(i)("GroupID")) = intGroupID)) Then
                            Me.Rows(i)("ApplicationFieldID") = -1
                            Me.Rows(i)("Standard") = CStr(Me.Rows(i)("Content"))
                            Me.Rows(i)("Content") = ""
                        Else
                            Me.Rows(i)("Standard") = ""
                        End If
                    End If
                Next
            End If
        End Sub

        'was soll das ? denke das dient zum füllen der Datatable in diesem Konkreten Objekt, doch wo wird das Aufgerufen/Verwendet? JJ 2007.11.12
        Public Sub New(ByVal strAppURL As String, ByVal strConnectionString As String)
            Me.New(strAppURL, New SqlClient.SqlConnection(strConnectionString))
        End Sub


        Public Sub New(ByVal strAppURL As String, ByVal cn As SqlClient.SqlConnection)
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            Dim daFields As New SqlClient.SqlDataAdapter("SELECT " & _
                                    "[AppURL], " & _
                                    "[ApplicationFieldID], " & _
                                    "[FieldType] + [FieldName] AS [Field], " & _
                                    "[FieldType], " & _
                                    "[FieldName], " & _
                                    "[CustomerID], " & _
                                    "[LanguageID], " & _
                                    "[Visibility], " & _
                                    "[Content] " & _
                                    "FROM ApplicationField " & _
                                    "WHERE ([AppURL] = @AppURL)", cn)
            daFields.SelectCommand.Parameters.AddWithValue("@AppURL", strAppURL)
            daFields.Fill(Me)
        End Sub
#End Region

    End Class
End Namespace
