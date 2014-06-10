Imports System.IO

Public Class RequestLoggingModule
    Implements IHttpModule

    Public Sub Dispose() Implements IHttpModule.Dispose

    End Sub

    Public Sub Init(context As HttpApplication) Implements IHttpModule.Init
        AddHandler context.BeginRequest, AddressOf App_BeginRequest
        AddHandler context.EndRequest, AddressOf App_EndRequest
    End Sub

    Public Sub App_BeginRequest(ByVal source As Object, ByVal e As EventArgs)
        Dim app As HttpApplication = CType(source, HttpApplication)
        Dim context As HttpContext = app.Context

        If context.Request.ContentLength = 0 Then Return

        Dim request As HttpRequest = context.Request
        Dim response As HttpResponse = context.Response

        Dim filter As New ResponseFilterStream(response.Filter)
        response.Filter = filter

        Dim logService As GeneralTools.Services.LogService = New GeneralTools.Services.LogService(String.Empty, String.Empty)
        logService.LogWebServiceTraffic("Request", GetString(request.InputStream), ConfigurationManager.AppSettings("LogTableName"))
    End Sub

    Public Sub App_EndRequest(ByVal source As Object, ByVal e As EventArgs)
        Dim app As HttpApplication = CType(source, HttpApplication)
        Dim context As HttpContext = app.Context

        If context.Request.ContentLength = 0 Then Return

        Dim filter As ResponseFilterStream = CType(context.Response.Filter, ResponseFilterStream)

        Dim logService As GeneralTools.Services.LogService = New GeneralTools.Services.LogService(String.Empty, String.Empty)
        logService.LogWebServiceTraffic("Response", filter.ReadStream(), ConfigurationManager.AppSettings("LogTableName"))
    End Sub

    Private Function GetString(ByVal stream As Stream) As String
        Dim bytes = New Byte(stream.Length - 1) {}
        stream.Read(bytes, 0, bytes.Length)
        stream.Position = 0
        Return Encoding.UTF8.GetString(bytes)
    End Function

End Class
