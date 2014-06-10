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

        Try
            EventLog.WriteEntry("SixtServiceLeas", "Url " & Request.Path & " Warning: " & ErrMessage, EventLogEntryType.Warning)
        Catch
            'Fehlgeschlagener Eventlog-Eintrag darf nicht zum Abbruch der Anwendung führen
        End Try

    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Wird beim Beenden der Sitzung ausgelöst
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Wird beim Beenden der Anwendung ausgelöst
    End Sub

End Class