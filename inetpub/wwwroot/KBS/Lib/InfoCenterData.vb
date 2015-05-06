Option Explicit On
Option Strict On

Imports System
Imports System.Collections.Generic

Public Class InfoCenterData

#Region "Declarations"
    Private m_tblCustomer As New DataTable
    Private m_tblDocumentTypes As New DataTable
    Private m_tblDocuments As New DataTable
    Private m_tblDocumentRights As New DataTable
    Private m_uploadfile As String
#End Region

#Region "Properties"
    Public Property Customer() As DataTable
        Get
            Return m_tblCustomer
        End Get
        Set(ByVal Value As DataTable)
            m_tblCustomer = Value
        End Set
    End Property

    Public Property DocumentTypes() As DataTable
        Get
            Return m_tblDocumentTypes
        End Get
        Set(ByVal Value As DataTable)
            m_tblDocumentTypes = Value
        End Set
    End Property

    Public Property Documents() As DataTable
        Get
            Return m_tblDocuments
        End Get
        Set(ByVal Value As DataTable)
            m_tblDocuments = Value
        End Set
    End Property

    Public Property DocumentRights() As DataTable
        Get
            Return m_tblDocumentRights
        End Get
        Set(ByVal Value As DataTable)
            m_tblDocumentRights = Value
        End Set
    End Property

    Public Property UploadFile As String
        Get
            Return m_uploadfile
        End Get
        Set(value As String)
            m_uploadfile = value
        End Set
    End Property

#End Region

#Region "Methods"

    Public Sub GetCustomerList()
        Using cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
            With cn
                .Open()

                m_tblCustomer.Clear()

                Dim daDocTypes As SqlClient.SqlDataAdapter
                daDocTypes = New SqlClient.SqlDataAdapter("SELECT CustomerID, Customername, KUNNR FROM Customer", cn)

                daDocTypes.Fill(m_tblCustomer)

                .Close()
            End With
        End Using
    End Sub

    Public Sub GetDocumentTypes()
        Using cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
            With cn
                .Open()

                m_tblDocumentTypes.Clear()

                Dim daDocTypes As SqlClient.SqlDataAdapter
                daDocTypes = New SqlClient.SqlDataAdapter("SELECT * FROM DocumentType", cn)

                daDocTypes.Fill(m_tblDocumentTypes)

                .Close()
            End With
        End Using
    End Sub

    ''' <summary>
    ''' Gibt alle Dokumente bzw. alle Dokumente für einen Kunden zurück
    ''' </summary>
    ''' <param name="customerId"></param>
    ''' <remarks></remarks>
    Public Sub GetDocuments(Optional ByVal customerId As Integer = -1)
        Using cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
            With cn
                .Open()

                m_tblDocuments.Clear()

                Dim daDocs As SqlClient.SqlDataAdapter

                If customerId = -1 Then
                    daDocs = New SqlClient.SqlDataAdapter("SELECT * FROM vwDocumentWithType", cn)
                Else
                    daDocs = New SqlClient.SqlDataAdapter("SELECT * FROM vwDocumentCustomer WHERE customerId = @customerId", cn)
                    daDocs.SelectCommand.Parameters.AddWithValue("@customerId", customerId)
                End If

                daDocs.Fill(m_tblDocuments)

                .Close()
            End With
        End Using
    End Sub

    Public Sub GetDocumentRights(ByVal documentId As Integer)
        Using cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
            With cn
                .Open()

                m_tblDocumentRights.Clear()

                Dim daDocs As SqlClient.SqlDataAdapter
                daDocs = New SqlClient.SqlDataAdapter("SELECT * FROM documentRights WHERE documentId = @documentId", cn)

                daDocs.SelectCommand.Parameters.AddWithValue("@documentId", documentId)
                daDocs.Fill(m_tblDocumentRights)

                .Close()
            End With
        End Using
    End Sub

    Public Sub SaveDocumentType(ByVal docTypeId As Integer, ByVal docType As String)
        Dim tablerow As DataRow
        Dim rows As DataRow()

        Using cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
            With cn
                .Open()

                Dim cmd As SqlClient.SqlCommand = New SqlClient.SqlCommand()
                cmd.Connection = cn
                cmd.CommandType = CommandType.Text

                If docTypeId = -1 Then
                    cmd.CommandText = "INSERT INTO DocumentType (docTypeName) VALUES (@docTypeName);SELECT SCOPE_IDENTITY() AS 'Identity'"
                    cmd.Parameters.AddWithValue("@docTypeName", docType)
                    Dim newId As Integer = CInt(cmd.ExecuteScalar)
                    tablerow = m_tblDocumentTypes.NewRow
                    tablerow("documentTypeId") = newId
                    tablerow("docTypeName") = docType
                    m_tblDocumentTypes.Rows.Add(tablerow)
                Else
                    cmd.CommandText = "UPDATE DocumentType SET docTypeName = @docTypeName WHERE documentTypeId = @documentTypeId;"
                    cmd.Parameters.AddWithValue("@docTypeName", docType)
                    cmd.Parameters.AddWithValue("@documentTypeId", docTypeId)
                    cmd.ExecuteNonQuery()
                    tablerow = m_tblDocumentTypes.Select("documentTypeId=" & docTypeId)(0)
                    tablerow("docTypeName") = docType
                    rows = m_tblDocuments.Select("docTypeId=" & docTypeId)
                    For Each dr As DataRow In rows
                        dr("docTypeName") = docType
                    Next
                End If

                m_tblDocumentTypes.AcceptChanges()
                m_tblDocuments.AcceptChanges()

                .Close()
            End With
        End Using

    End Sub

    Public Sub SaveDocument(ByVal documentId As Integer, ByVal docTypeId As Integer, Optional ByVal fileName As String = "", Optional ByVal fileType As String = "", Optional ByVal fileLastEdited As DateTime = Nothing, Optional ByVal fileSize As Long = 0)
        Dim tablerow As DataRow

        Dim jetzt As DateTime = DateTime.Now

        Using cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
            With cn
                .Open()

                Dim cmd As SqlClient.SqlCommand = New SqlClient.SqlCommand()
                cmd.Connection = cn
                cmd.CommandType = CommandType.Text

                If documentId = -1 Then
                    'prüfen, ob die Datei schon existiert, also nur überschrieben werden soll, oder wirklich DB-seitig neu angelegt werden muss
                    cmd.CommandText = "SELECT documentId FROM Document WHERE fileName = @fileName AND fileType = @fileType;"
                    cmd.Parameters.AddWithValue("@fileName", fileName)
                    cmd.Parameters.AddWithValue("@fileType", fileType)
                    Dim gefundeneId As Object = cmd.ExecuteScalar()

                    If gefundeneId IsNot Nothing Then
                        cmd.CommandText = "UPDATE Document SET uploaded = @uploaded WHERE documentId = @documentId;"
                        cmd.Parameters.Clear()
                        cmd.Parameters.AddWithValue("@uploaded", jetzt)
                        cmd.Parameters.AddWithValue("@documentId", CInt(gefundeneId))
                        cmd.ExecuteNonQuery()
                        tablerow = m_tblDocuments.Select("documentId='" & CInt(gefundeneId) & "'")(0)
                        tablerow("lastEdited") = jetzt
                    Else
                        cmd.CommandText = "INSERT INTO Document (docTypeId,fileName,fileType,lastEdited,fileSize,uploaded) VALUES (@docTypeId,@fileName,@fileType,@lastEdited,@fileSize,@uploaded);SELECT SCOPE_IDENTITY() AS 'Identity'"
                        cmd.Parameters.Clear()
                        cmd.Parameters.AddWithValue("@docTypeId", docTypeId)
                        cmd.Parameters.AddWithValue("@fileName", fileName)
                        cmd.Parameters.AddWithValue("@fileType", fileType)
                        cmd.Parameters.AddWithValue("@lastEdited", fileLastEdited)
                        cmd.Parameters.AddWithValue("@fileSize", fileSize)
                        cmd.Parameters.AddWithValue("@uploaded", jetzt)
                        Dim newId As Integer = CInt(cmd.ExecuteScalar)
                        tablerow = m_tblDocuments.NewRow
                        tablerow("documentId") = newId
                        tablerow("docTypeId") = docTypeId
                        tablerow("docTypeName") = m_tblDocumentTypes.Select("documentTypeId=" & docTypeId)(0)("docTypeName")
                        tablerow("fileName") = fileName
                        tablerow("fileType") = fileType
                        tablerow("lastEdited") = fileLastEdited
                        m_tblDocuments.Rows.Add(tablerow)
                    End If
                Else
                    'aktuell nur Dokumententyp als veränderbarer Parameter vorgesehen
                    cmd.CommandText = "UPDATE Document SET docTypeId = @docTypeId WHERE documentId = @documentId;"
                    cmd.Parameters.AddWithValue("@docTypeId", docTypeId)
                    cmd.Parameters.AddWithValue("@documentId", documentId)
                    cmd.ExecuteNonQuery()
                    tablerow = m_tblDocuments.Select("documentId='" & documentId & "'")(0)
                    tablerow("docTypeId") = docTypeId
                    tablerow("docTypeName") = m_tblDocumentTypes.Select("documentTypeId=" & docTypeId)(0)("docTypeName")
                End If

                m_tblDocuments.AcceptChanges()

                .Close()
            End With
        End Using

    End Sub

    Public Sub SaveDocumentRights(ByVal documentId As Integer, ByVal customerList As List(Of Integer))
        Dim tablerow As DataRow

        If customerList IsNot Nothing AndAlso customerList.Count > 0 Then
            Using cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
                With cn
                    .Open()

                    Dim cmd As SqlClient.SqlCommand = New SqlClient.SqlCommand()
                    cmd.Connection = cn
                    cmd.CommandType = CommandType.Text

                    cmd.CommandText = "INSERT INTO DocumentRights (documentId,customerId) VALUES (@documentId,@customerId);"

                    For Each kunde As Integer In customerList
                        cmd.Parameters.Clear()
                        cmd.Parameters.AddWithValue("@documentId", documentId)
                        cmd.Parameters.AddWithValue("@customerId", kunde)
                        cmd.ExecuteNonQuery()
                        tablerow = m_tblDocumentRights.NewRow
                        tablerow("documentId") = documentId
                        tablerow("customerId") = kunde
                        m_tblDocumentRights.Rows.Add(tablerow)
                    Next

                    m_tblDocumentRights.AcceptChanges()

                    .Close()
                End With
            End Using
        End If

    End Sub

    Public Function DeleteDocumentType(ByVal docTypeId As Integer) As Boolean
        Dim tablerow As DataRow

        Using cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
            With cn
                .Open()

                Dim cmd As SqlClient.SqlCommand = New SqlClient.SqlCommand()
                cmd.Connection = cn
                cmd.CommandType = CommandType.Text

                'prüfen, ob DocType noch verwendet wird -> dann nicht löschen
                cmd.CommandText = "SELECT COUNT(*) FROM Document WHERE DocTypeId=" & docTypeId & " ;"
                If CInt(cmd.ExecuteScalar()) > 0 Then
                    Return False
                End If

                cmd.CommandText = "DELETE FROM DocumentType WHERE DocumentTypeId=" & docTypeId & " ;"
                cmd.Parameters.Clear()
                cmd.ExecuteNonQuery()
                tablerow = m_tblDocumentTypes.Select("documentTypeId='" & docTypeId & "'")(0)
                tablerow.Delete()
                m_tblDocumentTypes.AcceptChanges()

                .Close()
            End With
        End Using

        Return True

    End Function

    Public Sub DeleteDocument(ByVal documentId As Integer)
        Dim tablerow As DataRow
        Dim rows As DataRow()

        Using cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
            With cn
                .Open()

                Dim cmd As SqlClient.SqlCommand = New SqlClient.SqlCommand()
                cmd.Connection = cn
                cmd.CommandType = CommandType.Text

                'ggf. vorhandene Verknüpfungen entfernen
                cmd.CommandText = "DELETE FROM DocumentRights WHERE DocumentId=" & documentId & " ;"
                cmd.ExecuteNonQuery()
                If m_tblDocumentRights IsNot Nothing AndAlso m_tblDocumentRights.Rows.Count > 0 Then
                    rows = m_tblDocumentRights.Select("documentId='" & documentId & "'")
                    For Each dr As DataRow In rows
                        dr.Delete()
                    Next
                    m_tblDocumentRights.AcceptChanges()
                End If

                cmd.CommandText = "DELETE FROM Document WHERE DocumentId=" & documentId & " ;"
                cmd.Parameters.Clear()
                cmd.ExecuteNonQuery()
                tablerow = m_tblDocuments.Select("documentId='" & documentId & "'")(0)
                tablerow.Delete()
                m_tblDocuments.AcceptChanges()

                .Close()
            End With
        End Using

    End Sub

    Public Sub DeleteDocumentRights(ByVal documentId As Integer, ByVal customerList As List(Of Integer))
        Dim tablerow As DataRow

        If customerList IsNot Nothing AndAlso customerList.Count > 0 Then
            Using cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
                With cn
                    .Open()

                    Dim cmd As SqlClient.SqlCommand = New SqlClient.SqlCommand()
                    cmd.Connection = cn
                    cmd.CommandType = CommandType.Text

                    cmd.CommandText = "DELETE FROM DocumentRights WHERE documentId = @documentId AND customerId = @customerId;"

                    For Each kunde As Integer In customerList
                        cmd.Parameters.Clear()
                        cmd.Parameters.AddWithValue("@documentId", documentId)
                        cmd.Parameters.AddWithValue("@customerId", kunde)
                        cmd.ExecuteNonQuery()
                        tablerow = m_tblDocumentRights.Select("documentId='" & documentId & "' AND customerId='" & kunde & "'")(0)
                        tablerow.Delete()
                    Next

                    m_tblDocumentRights.AcceptChanges()

                    .Close()
                End With
            End Using
        End If

    End Sub

#End Region

End Class

