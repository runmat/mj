Option Strict On
Option Explicit On

Imports CKG.Base.Kernel

Partial Public Class ServicesMenue
    Inherits System.Web.UI.UserControl

    Private m_User As Security.User
    'Private m_App As Security.App

    Public MenuChangeSource As DataView
    Public MenuAdminSource As DataView
    Public MenuHelpDeskSource As DataView
    Public MenuReportSource As DataView
    Public MenuKVPSource As DataView

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Session("objUser") Is Nothing Then

            'm_User = CType(Session("objUser"), Security.User)
            m_User = ZLDBaseMvc.MVC.GetSessionUserObject()

            ' --- 28.08.2013, CDI
            ' --- MVC Integration:
            ' ---
            'm_User = GetUser(Me.Page)
            'm_App = New Security.App(m_User)

            Dim conn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
            conn.Open()
            Try

                Dim table As DataTable
                Dim command As New SqlClient.SqlCommand()
                'Dim blnReturn As Boolean = True

                command.CommandText = "SELECT AppType,DisplayName FROM ApplicationType ORDER BY Rank"


                Dim da As New SqlClient.SqlDataAdapter(command)
                command.Connection = conn

                table = New DataTable()
                da.Fill(table)

                Dim appTable As DataTable = m_User.Applications.Copy


                ' --- 28.08.2013, CDI
                ' --- MVC Integration:
                ' ---
                'MVC.MvcPrepareDataRowsUrl(appTable, m_User.UserName)
                For Each dRow As DataRow In appTable.Rows
                    If dRow("AppURL").ToString().ToLower().StartsWith("mvc/") Then
                        dRow("AppURL") = ZLDBaseMvc.MVC.MvcPrepareUrl(dRow("AppURL").ToString(), dRow("AppID").ToString(), m_User.UserName)
                    Else
                        dRow("AppURL") = GetUrlString(dRow("AppURL").ToString(), dRow("AppID").ToString())
                    End If

                    ' PageVisit soll auf dem Server geloggt werden und nicht auf via JS
                    ' Umschreiben als Aufruf an Log.aspx der dann einen Redirect zu dieser Adresse erstellt
                    Dim url As String = dRow("AppURL").ToString()

                    ' Url encoden für die Verwendung als Query Params
                    url = HttpUtility.UrlEncode(url)
                    url = Convert.ToBase64String(Encoding.UTF8.GetBytes(url.ToCharArray()))

                    ' Jetzt besteht die neue url aus: appid, original url unverändert übernehmen
                    dRow("AppURL") = String.Concat("../Start/Log.aspx?", "APP-ID=", dRow("AppID"), "&url=", url)

                Next

                MenuReportSource = New DataView(appTable)
                MenuReportSource.RowFilter = "AppType='Report' AND AppInMenu=1"

                MenuChangeSource = New DataView(appTable)
                MenuChangeSource.RowFilter = "AppType='Change' AND AppInMenu=1"

                MenuAdminSource = New DataView(appTable)
                MenuAdminSource.RowFilter = "AppType='Admin' AND AppInMenu=1"

                MenuKVPSource = New DataView(appTable)
                MenuKVPSource.RowFilter = "AppType='KVP' AND AppInMenu=1"

                MenuHelpDeskSource = New DataView(appTable)
                MenuHelpDeskSource.RowFilter = "AppType='Helpdesk' AND AppInMenu=1"

            Catch ex As Exception

            Finally
                conn.Close()
            End Try
        End If
    End Sub

    Public Function GetUrlString(ByVal strAppUrl As String, ByVal strAppID As String) As String
        Dim paramlist As String = ""
        Dim getParamList As Boolean

        getParamList = getAppParameters(strAppID, paramlist)
        If Left(strAppUrl, 4) = "http" Then
            strAppUrl = (strAppUrl).Replace("../Applications/AppGenerali/forms/", "")
            Return strAppUrl
            ' Return (strAppUrl).Replace("AppVFS", "AppGenerali")

        Else
            strAppUrl = (strAppUrl & "?AppID=" & strAppID & paramlist).Replace("AppVFS", "AppGenerali")
            '   strAppUrl = (strAppUrl).Replace("../Applications/AppGenerali/forms/", "")
            Return strAppUrl
        End If
        
    End Function

    Public Function getAppParameters(ByVal strAppID As String, ByRef paramlist As String) As Boolean
        Dim conn As New SqlClient.SqlConnection()
        Dim command As New SqlClient.SqlCommand()
        Dim adapter As New SqlClient.SqlDataAdapter()
        Dim result As New DataTable()

        command.CommandType = CommandType.Text
        command.CommandText = "SELECT * FROM ApplicationParamlist WHERE id_app = " & strAppID
        conn.ConnectionString = ConfigurationManager.AppSettings("Connectionstring")
        command.Connection = conn
        Try

            conn.Open()
            adapter.SelectCommand = command
            adapter.Fill(result)
            paramlist = String.Empty
            If Not (result.Rows.Count = 0) Then
                paramlist = result.Rows(0)("paramlist").ToString
            End If
            Return True
        Catch ex As Exception
            paramlist = String.Empty
            Return False
        Finally
            conn.Close()
            conn.Dispose()
        End Try
    End Function

End Class