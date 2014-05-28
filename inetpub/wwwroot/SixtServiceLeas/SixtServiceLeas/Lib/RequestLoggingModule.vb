Imports System.IO

Public Class RequestLoggingModule
    Implements IHttpModule

    Private Shared LockObject As New Object()

    Public Sub Dispose() Implements System.Web.IHttpModule.Dispose

    End Sub

    Public Sub Init(context As System.Web.HttpApplication) Implements System.Web.IHttpModule.Init
        AddHandler context.BeginRequest, AddressOf Me.App_BeginRequest
        AddHandler context.EndRequest, AddressOf Me.App_EndRequest
    End Sub

    Public Sub App_BeginRequest(ByVal source As Object, ByVal e As EventArgs)
        Dim app As HttpApplication = CType(source, HttpApplication)
        Dim context As HttpContext = app.Context

        If context.Request.ContentLength = 0 Then Return

        Dim request As HttpRequest = context.Request
        Dim response As HttpResponse = context.Response

        Dim filter As New ResponseFilterStream(response.Filter)
        response.Filter = filter

        Dim path As String = System.Configuration.ConfigurationManager.AppSettings("RequestLogFilePath")

        LogStream(String.Format(path, DateTime.Now), request.InputStream, String.Format("-----Request at {0:yyyy-MM-dd_HH-mm-ss}", DateTime.Now))
    End Sub

    Public Sub App_EndRequest(ByVal source As Object, ByVal e As EventArgs)
        Dim app As HttpApplication = CType(source, HttpApplication)
        Dim context As HttpContext = app.Context

        If context.Request.ContentLength = 0 Then Return

        Dim filter As ResponseFilterStream = CType(context.Response.Filter, ResponseFilterStream)

        Dim path As String = System.Configuration.ConfigurationManager.AppSettings("ResponseLogFilePath")

        LogString(String.Format(path, DateTime.Now), filter.ReadStream(), String.Format("-----Response at {0:yyyy-MM-dd_HH-mm-ss}", DateTime.Now))
    End Sub

    Private Sub LogStream(ByVal path As String, ByVal stream As Stream, ByVal headline As String)
        Dim bytes = New Byte(stream.Length - 1) {}
        stream.Read(bytes, 0, bytes.Length)
        stream.Position = 0
        Dim responseData As String = Encoding.UTF8.GetString(bytes)
        LogString(path, responseData, headline)
    End Sub

    Private Sub LogString(ByVal path As String, ByVal data As String, ByVal headline As String)
        If Not String.IsNullOrEmpty(data) Then

            If Not System.IO.Path.IsPathRooted(path) Then
                path = HttpContext.Current.Server.MapPath(path)
            End If

            path = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(path), System.IO.Path.GetFileNameWithoutExtension(path)) &
                "-" & System.Diagnostics.Process.GetCurrentProcess().Id &
                System.IO.Path.GetExtension(path)

            Try
                SyncLock RequestLoggingModule.LockObject
                    Using streamWriter As New System.IO.StreamWriter(path, True, System.Text.Encoding.UTF8)
                        streamWriter.WriteLine(headline)
                        streamWriter.WriteLine(data)
                    End Using
                End SyncLock
            Catch ex As Exception
                EventLog.WriteEntry("SixtServiceLeas", ex.Message, EventLogEntryType.Error)
            End Try
        End If
    End Sub

End Class
