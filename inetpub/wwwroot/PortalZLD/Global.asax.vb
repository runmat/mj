Imports System.Web
Imports System.Configuration


Public Class Global_asax
    Inherits System.Web.HttpApplication
    Private m_lngCurrentDate As Long

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)

        ZLDBaseMvc.MVC.CryptAndSaveAppSettings()

        ' Wird beim Starten der Anwendung ausgelöst
        Dim conn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Try

            'Caching unterbinden, damit definitiv immer die neuesten Programm-/Skriptversionen geladen werden
            HttpContext.Current.Response.Cache.SetAllowResponseInBrowserHistory(False)
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache)
            HttpContext.Current.Response.Cache.SetNoStore()
            HttpContext.Current.Response.Cache.SetExpires(DateTime.Now)
            HttpContext.Current.Response.Cache.SetValidUntilExpires(True)

            Dim cmdUnlockUser As New SqlClient.SqlCommand("update LogWebAccess set endTime=@endTime, hostname='Restart' where endTime is null", conn)
            Dim command As New SqlClient.SqlCommand("update WebUser set LoggedOn=0, LastChangedBy='Restart'", conn)

            conn.Open()

            cmdUnlockUser.Parameters.AddWithValue("@endTime", Now)
            cmdUnlockUser.ExecuteNonQuery()
            cmdUnlockUser.Dispose()

            command.ExecuteNonQuery()
            command.Dispose()

        Catch ex As Exception
            'Fehlerbehandlung erfolgt in laufender Session
            'Hier wird nur die Ausgabe von "Serverfehler..." verhindert
        Finally
            conn.Close()
            conn.Dispose()
        End Try
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Wird beim Starten der Sitzung ausgelöst
        Dim conn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        If (Not (m_lngCurrentDate = CLng(Format(Now, "yyyyMMdd")))) Then
            m_lngCurrentDate = CLng(Format(Now, "yyyyMMdd"))
            Try

                Dim cmdlockUser As New SqlClient.SqlCommand("UPDATE WebUser SET AccountIsLockedOut=1, LastChangedBy='Inaktivität' WHERE AccountIsLockedOut=0 AND UserID IN (SELECT UserID From vwLastUserAccess2Lock)", conn)

                conn.Open()

                cmdlockUser.ExecuteNonQuery()
                cmdlockUser.Dispose()
            Catch ex As Exception
                'Fehlerbehandlung erfolgt in laufender Session
                'Hier wird nur die Ausgabe von "Serverfehler..." verhindert
            Finally
                If conn.State <> ConnectionState.Closed Then
                    conn.Close()
                End If
                conn.Dispose()
            End Try
        End If
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)

    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Wird beim Versuch der Benutzerauthentifizierung ausgelöst
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)

        ' Wird ausgelöst, wenn ein Fehler auftritt.
        Dim LastError As Exception = Server.GetLastError()

        Dim logService As GeneralTools.Services.LogService = New GeneralTools.Services.LogService("/PortalZLD", String.Empty)
        logService.LogElmahError(LastError, Nothing)

    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        On Error Resume Next

        Dim m_User As CKG.Base.Kernel.Security.User
        m_User = CType(Session("objUser"), CKG.Base.Kernel.Security.User)
        m_User.SetLoggedOn(m_User.UserName, False, "")

        'If Not HttpContext.Current Is Nothing Then
        '    HttpContext.Current.Cache.Remove("myAppListView")
        '    HttpContext.Current.Cache.Remove("myAppParentView")
        '    HttpContext.Current.Cache.Remove("myColListView")
        '    HttpContext.Current.Cache.Remove("myCustomerListView")
        '    HttpContext.Current.Cache.Remove("myCustomerAppAssigned")
        '    HttpContext.Current.Cache.Remove("myGroupListView")
        '    HttpContext.Current.Cache.Remove("myOrganizationListView")
        '    HttpContext.Current.Cache.Remove("myAppAssigned")
        '    HttpContext.Current.Cache.Remove("myHalterListView")
        '    HttpContext.Current.Cache.Remove("myUserListView")
        '    HttpContext.Current.Cache.Remove("myVersichererListView")
        '    HttpContext.Current.Cache.Remove("myZulassungsVorbelegungListView")
        '    HttpContext.Current.Cache.Remove("objChange01_objFDDBank")
        '    HttpContext.Current.Cache.Remove("objReport05_objFDDBank")
        '    HttpContext.Current.Cache.Remove("objReport05_objFDDBank2")
        '    HttpContext.Current.Cache.Remove("m_objTrace")
        'End If

        m_User = Nothing

        'Session.RemoveAll()

        'GC.Collect()
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Wird beim Beenden der Anwendung ausgelöst
    End Sub

End Class