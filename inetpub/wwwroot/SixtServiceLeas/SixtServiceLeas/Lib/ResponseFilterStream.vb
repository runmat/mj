Imports System.IO

Public Class ResponseFilterStream
    Inherits Stream
    Private ReadOnly InnerStream As Stream
    Private ReadOnly CopyStream As MemoryStream
    Public Sub New(inner As Stream)
        Me.InnerStream = inner
        Me.CopyStream = New MemoryStream()
    End Sub
    Public Function ReadStream() As String
        SyncLock Me.InnerStream
            If Me.CopyStream.Length <= 0L OrElse Not Me.CopyStream.CanRead OrElse Not Me.CopyStream.CanSeek Then
                Return [String].Empty
            End If
            Dim pos As Long = Me.CopyStream.Position
            Me.CopyStream.Position = 0L
            Try
                Return New StreamReader(Me.CopyStream).ReadToEnd()
            Finally
                Try
                    Me.CopyStream.Position = pos
                Catch
                End Try
            End Try
        End SyncLock
    End Function
    Public Overrides ReadOnly Property CanRead() As Boolean
        Get
            Return Me.InnerStream.CanRead
        End Get
    End Property
    Public Overrides ReadOnly Property CanSeek() As Boolean
        Get
            Return Me.InnerStream.CanSeek
        End Get
    End Property
    Public Overrides ReadOnly Property CanWrite() As Boolean
        Get
            Return Me.InnerStream.CanWrite
        End Get
    End Property
    Public Overrides Sub Flush()
        Me.InnerStream.Flush()
    End Sub
    Public Overrides ReadOnly Property Length() As Long
        Get
            Return Me.InnerStream.Length
        End Get
    End Property
    Public Overrides Property Position() As Long
        Get
            Return Me.InnerStream.Position
        End Get
        Set(value As Long)
            Me.CopyStream.Position = InlineAssignHelper(Me.InnerStream.Position, value)
        End Set
    End Property
    Public Overrides Function Read(buffer As Byte(), offset As Integer, count As Integer) As Integer
        Return Me.InnerStream.Read(buffer, offset, count)
    End Function
    Public Overrides Function Seek(offset As Long, origin As SeekOrigin) As Long
        Me.CopyStream.Seek(offset, origin)
        Return Me.InnerStream.Seek(offset, origin)
    End Function
    Public Overrides Sub SetLength(value As Long)
        Me.CopyStream.SetLength(value)
        Me.InnerStream.SetLength(value)
    End Sub
    Public Overrides Sub Write(buffer As Byte(), offset As Integer, count As Integer)
        Me.CopyStream.Write(buffer, offset, count)
        Me.InnerStream.Write(buffer, offset, count)
    End Sub
    Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
        target = value
        Return value
    End Function
End Class
