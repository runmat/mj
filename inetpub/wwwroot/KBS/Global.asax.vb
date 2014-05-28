
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
        ' Wird bei einem Fehler ausgelöst
        Dim LastError As Exception = Server.GetLastError()

        Dim logService As GeneralTools.Services.LogService = New GeneralTools.Services.LogService("/KBS", String.Empty)
        logService.LogElmahError(LastError, Nothing)
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Wird beim Beenden der Sitzung ausgelöst
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Wird beim Beenden der Anwendung ausgelöst
    End Sub

End Class