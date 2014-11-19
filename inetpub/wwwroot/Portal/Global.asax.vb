Imports System.Web
Imports System.Web.SessionState
Imports System.Diagnostics
Imports System.Configuration
Imports GeneralTools.Log.Services

Public Class [Global]
    Inherits System.Web.HttpApplication

    Private m_lngCurrentDate As Long

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
        Dim conn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Try
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
        ' Wird am Anfang jeder Anforderung ausgelöst.
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Wird ausgelöst, wenn versucht wird, die Verwendung zu authentifizieren.
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Wird ausgelöst, wenn ein Fehler auftritt.
        Dim LastError As Exception = Server.GetLastError()

        Dim logService As GeneralTools.Services.LogService = New GeneralTools.Services.LogService(String.Empty, String.Empty)
        logService.LogElmahError(LastError, Nothing)
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        On Error Resume Next

        Dim m_User As CKG.Base.Kernel.Security.User
        m_User = CType(Session("objUser"), CKG.Base.Kernel.Security.User)
        m_User.SetLoggedOn(m_User.UserName, False)


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

        If Not Session("FreeDBFromAccess") Is Nothing Then
            ClearSessionFromDB(Session.SessionID, m_User)
        End If


        Dim log As CKG.Base.Kernel.Logging.LogWebAccess
        Dim mode As Boolean
        mode = CType(Session("logoutMode"), String) Is Nothing

        log = CType(Session("log"), CKG.Base.Kernel.Logging.LogWebAccess)
        log.updateEndTime(mode)
        log.Dispose()
        m_User = Nothing

        Session.RemoveAll()

        'GC.Collect()
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Wird ausgelöst, wenn die Anwendung beendet wird.
    End Sub

    Private Sub ClearSessionFromDB(ByVal SessionID As String, ByVal User As CKG.Base.Kernel.Security.User)
        Dim m_App As CKG.Base.Kernel.Security.App = User.App
        Dim str As String = m_App.Connectionstring
        Dim con As New SqlClient.SqlConnection()
        con.ConnectionString = str
        Try
            con.Open()
            Dim com As String = "UPDATE [ArchivUse] SET InUse = 0 WHERE SessionID = '" & SessionID & "'"
            Dim SQC As New SqlClient.SqlCommand(com, con)
            SQC.ExecuteNonQuery()
        Catch ex As Exception

        Finally
            con.Close()
        End Try
    End Sub

End Class

' ************************************************
' $History: Global.asax.vb $
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 5.06.09    Time: 13:05
' Updated in $/CKAG/portal
' Applikation error aufgeräumt
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 5.06.09    Time: 9:27
' Updated in $/CKAG/portal
' Peis
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 4.06.09    Time: 15:26
' Updated in $/CKAG/portal
' ita 2694 nachbesserungen
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 5.05.09    Time: 10:10
' Updated in $/CKAG/portal
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 28.04.09   Time: 13:45
' Updated in $/CKAG/portal
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 15.09.08   Time: 8:34
' Updated in $/CKAG/portal
' Fehler durch Trigger übersprungen.
' 
' *****************  Version 3  *****************
' User: Hartmannu    Date: 10.09.08   Time: 9:31
' Updated in $/CKAG/portal
' ITA 2027 (Erweiterung Historie)
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 11.04.08   Time: 11:57
' Updated in $/CKAG/portal
' Migration OR
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 3.04.08    Time: 17:16
' Created in $/CKAG/portal
' 
' *****************  Version 11  *****************
' User: Uha          Date: 7.08.07    Time: 16:49
' Updated in $/CKG/Portal
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 2.08.07    Time: 13:26
' Updated in $/CKG/Portal
' Fehler im Eventlog von Error auf Warning umgestellt.
' 
' *****************  Version 9  *****************
' User: Uha          Date: 21.06.07   Time: 18:45
' Updated in $/CKG/Portal
' Bugfixing VFS 2
' 
' *****************  Version 8  *****************
' User: Uha          Date: 31.05.07   Time: 11:41
' Updated in $/CKG/Portal
' ITA 1077 - Login bei bereits aktiver Anmeldung ermöglichen
' 
' *****************  Version 7  *****************
' User: Uha          Date: 15.05.07   Time: 15:29
' Updated in $/CKG/Portal
' Änderungen aus StartApplication vom 11.05.2007
' 
' *****************  Version 6  *****************
' User: Uha          Date: 1.03.07    Time: 16:38
' Updated in $/CKG/Portal
' 
' ************************************************