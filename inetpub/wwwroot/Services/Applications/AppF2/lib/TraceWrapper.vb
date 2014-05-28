Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common
Imports CKG.Base.Kernel.Logging
Imports System.Data.SqlClient

Public Class TraceWrapper
    'Private trace As Trace
    Private app As App
    Private user As User
    Private sessionID As String
    Private lastInsertedAt As Nullable(Of DateTime)
    Private lastRestrictedToSession As Boolean
    Private lastResult As DataTable
    Private lastError As String

    Public Sub New(app As App, user As User, sessionID As String)
        'trace = New Trace(app.Connectionstring, False)
        Me.app = app
        Me.user = user
        Me.sessionID = sessionID
    End Sub

    Public Sub Load(Optional ByVal restrictToSession As Boolean = True, Optional ByVal insertedAt As Nullable(Of DateTime) = Nothing)
        If Not lastResult Is Nothing AndAlso _
            lastRestrictedToSession = restrictToSession AndAlso _
            lastInsertedAt.Equals(insertedAt) Then
            Return
        End If

        lastError = String.Empty
        Try
            Dim p = New List(Of String)
            p.Add(String.Format("( UserName = '{0}' )", user.UserName))
            If restrictToSession Then p.Add(String.Format("( SessionID = '{0}' )", sessionID))
            If insertedAt.HasValue Then p.Add(String.Format("( Inserted between convert(DateTime, '{0:d}', 104) and convert(DateTime, '{1:d}', 104) )", insertedAt.Value, insertedAt.Value.AddDays(1)))
            p.Add("( not Identification = 'Report' )")


            Dim cnn = New SqlConnection(app.Connectionstring)
            Dim cmd = cnn.CreateCommand()
            cmd.CommandText = "select * from LogStandard where " + String.Join(" and ", p.ToArray)
            cmd.CommandType = CommandType.Text

            cnn.Open()
            Dim reader = cmd.ExecuteReader()
            Dim result = New DataTable()
            result.Load(reader)
            cnn.Close()

            lastResult = result
            lastRestrictedToSession = restrictToSession
            lastInsertedAt = insertedAt
        Catch ex As Exception
            lastError = ex.ToString
        End Try
    End Sub


    Public ReadOnly Property Result As DataTable
        Get
            Return lastResult
        End Get
    End Property

    Public ReadOnly Property ErrorMessage As String
        Get
            Return lastError
        End Get
    End Property
End Class
