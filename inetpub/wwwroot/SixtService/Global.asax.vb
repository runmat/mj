Imports System.Web
Imports System.Web.SessionState
Imports System.IO
Public Class [Global]
    Inherits System.Web.HttpApplication

#Region " Vom Component Designer generierter Code "

    Public Sub New()
        MyBase.New()

        ' Dieser Aufruf ist für den Komponenten-Designer erforderlich.
        InitializeComponent()

        ' Initialisierungen nach dem Aufruf InitializeComponent() hinzufügen

    End Sub

    ' Für Komponenten-Designer erforderlich
    Private components As System.ComponentModel.IContainer

    'HINWEIS: Die folgende Prozedur ist für den Komponenten-Designer erforderlich
    'Sie kann mit dem Komponenten-Designer modifiziert werden.
    'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        components = New System.ComponentModel.Container()
    End Sub

#End Region

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Wird ausgelöst, wenn die Anwendung gestartet wird.
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Wird ausgelöst, wenn die Sitzung gestartet wird.
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Wird am Anfang jeder Anforderung ausgelöst.
        'Request.SaveAs(("C:\Inetpub\wwwroot\Portal\Temp\" & "Test" & Format(DateTime.Now, "ddMMyyyyHHmmss") & ".xml"), False)

    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Wird ausgelöst, wenn versucht wird, die Verwendung zu authentifizieren.
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Wird ausgelöst, wenn ein Fehler auftritt.
        Dim LastError As Exception = Server.GetLastError()
        Dim ErrMessage As String = LastError.ToString()

        Dim LogName As String = "SixtService"
        Dim Message As String = "Url " & Request.Path & " Warning: " & ErrMessage

        ' Create Event Log if It Doesn't Exist
        If EventLog.SourceExists(LogName) Then

            Dim Log As New EventLog()
            Log.Source = LogName
            Log.WriteEntry(Message, EventLogEntryType.Warning)

        End If


    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Wird ausgelöst, wenn die Sitzung beendet wird.
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Wird ausgelöst, wenn die Anwendung beendet wird.
    End Sub

End Class
