Imports System.Web.SessionState

Public Class Global_asax
    Inherits System.Web.HttpApplication

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Wird beim Starten der Anwendung ausgelöst
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Wird beim Starten der Sitzung ausgelöst
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Wird zu Beginn jeder Anforderung ausgelöst
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Wird beim Versuch der Benutzerauthentifizierung ausgelöst
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Wird ausgelöst, wenn ein Fehler auftritt.
        Dim LastError As Exception = Server.GetLastError()
        Dim ErrMessage As String = LastError.ToString()

        Dim LogName As String = "SixtServiceLeas"
        Dim Message As String = "Url " & Request.Path & " Warning: " & ErrMessage

        ' Create Event Log if It Doesn't Exist
        If EventLog.SourceExists(LogName) Then

            Dim Log As New EventLog()
            Log.Source = LogName
            Log.WriteEntry(Message, EventLogEntryType.Warning)

        End If


    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Wird beim Beenden der Sitzung ausgelöst
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Wird beim Beenden der Anwendung ausgelöst
    End Sub

End Class