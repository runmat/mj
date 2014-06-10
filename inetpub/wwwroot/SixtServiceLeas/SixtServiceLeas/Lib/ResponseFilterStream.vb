Imports System.IO

Public Class ResponseFilterStream
    Inherits Stream
    Private ReadOnly InnerStream As Stream
    Private ReadOnly CopyStream As MemoryStream

    Public Sub New(inner As Stream)
        InnerStream = inner
        CopyStream = New MemoryStream()
    End Sub

    Public Function ReadStream() As String
        SyncLock InnerStream
            If CopyStream.Length <= 0L OrElse Not CopyStream.CanRead OrElse Not CopyStream.CanSeek Then
                Return String.Empty
            End If
            Dim pos As Long = CopyStream.Position
            CopyStream.Position = 0L
            Try
                Return New StreamReader(CopyStream).ReadToEnd()
            Finally
                Try
                    CopyStream.Position = pos
                Catch
                End Try
            End Try
        End SyncLock
    End Function

    Public Overrides ReadOnly Property CanRead() As Boolean
        Get
            Return InnerStream.CanRead
        End Get
    End Property

    Public Overrides ReadOnly Property CanSeek() As Boolean
        Get
            Return InnerStream.CanSeek
        End Get
    End Property

    Public Overrides ReadOnly Property CanWrite() As Boolean
        Get
            Return InnerStream.CanWrite
        End Get
    End Property

    Public Overrides Sub Flush()
        InnerStream.Flush()
    End Sub

    Public Overrides ReadOnly Property Length() As Long
        Get
            Return InnerStream.Length
        End Get
    End Property

    Public Overrides Property Position() As Long
        Get
            Return InnerStream.Position
        End Get
        Set(value As Long)
            CopyStream.Position = InlineAssignHelper(InnerStream.Position, value)
        End Set
    End Property

    Public Overrides Function Read(buffer As Byte(), offset As Integer, count As Integer) As Integer
        Return InnerStream.Read(buffer, offset, count)
    End Function

    Public Overrides Function Seek(offset As Long, origin As SeekOrigin) As Long
        CopyStream.Seek(offset, origin)
        Return InnerStream.Seek(offset, origin)
    End Function

    Public Overrides Sub SetLength(value As Long)
        CopyStream.SetLength(value)
        InnerStream.SetLength(value)
    End Sub

    Public Overrides Sub Write(buffer As Byte(), offset As Integer, count As Integer)
        CopyStream.Write(buffer, offset, count)
        InnerStream.Write(buffer, offset, count)
    End Sub

    Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
        target = value
        Return value
    End Function

End Class
