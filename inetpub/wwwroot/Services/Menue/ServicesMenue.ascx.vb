Option Strict On
Option Explicit On

Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports ErpBaseMvc
Imports CKG.Base.Kernel.Security

Partial Public Class ServicesMenue
    Inherits System.Web.UI.UserControl
    Private m_User As Security.User
    Private m_App As Security.App

    Public MenuChangeSource As DataView
    Public MenuAdminSource As DataView
    Public MenuHelpDeskSource As DataView
    Public MenuReportSource As DataView




    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Session("objUser") Is Nothing Then

            'm_User = CType(Session("objUser"), Security.User)
            m_User = ErpBaseMvc.MVC.GetSessionUserObject()

            ' --- 20.5.2014 NB
            ' --- PageVisitLog verhindern für bestimmte Kunden
            Dim customersWithoutPageVisitLog As String
            customersWithoutPageVisitLog = ConfigurationManager.AppSettings("SkipPageVisitLogForCustomerId")

            If String.IsNullOrEmpty(customersWithoutPageVisitLog) = False Then

                Dim listOfCustomersWithoutPageVisitLog() As String
                listOfCustomersWithoutPageVisitLog = customersWithoutPageVisitLog.Split(CChar(","))

                ' Ich gehe davon aus dass wenn customersWithoutPageVisitLog nicht leer ist dass darin eine Liste von Komma separierten Werten drin ist
                If listOfCustomersWithoutPageVisitLog.Contains(m_User.Customer.CustomerId.ToString()) Then
                    ' Script für die Erstellung von Log Nachrichten prueft diesen Wert ab
                    SkipPageVisitLog.Value = "true"
                Else
                    SkipPageVisitLog.Value = "false"
                End If
            End If

            ' --- 20.06.2013, MJE
            ' --- MVC Integration:
            ' ---
            'm_User = GetUser(Me.Page)
            m_App = New Security.App(m_User)

            Dim conn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
            conn.Open()
            Try

                Dim table As DataTable
                Dim command As New SqlClient.SqlCommand()
                Dim blnReturn As Boolean = True

                command.CommandText = "SELECT AppType,DisplayName FROM ApplicationType ORDER BY Rank"


                Dim da As New SqlClient.SqlDataAdapter(command)
                command.Connection = conn

                table = New DataTable()
                da.Fill(table)

                Dim appTable As DataTable = m_User.Applications.Copy


                ' --- 20.06.2013, MJE
                ' --- MVC Integration:
                ' ---
                'MVC.MvcPrepareDataRowsUrl(appTable, m_User.UserName)
                For Each dRow As DataRow In appTable.Rows
                    If dRow("AppURL").ToString().ToLower().StartsWith("mvc/") Then
                        dRow("AppURL") = ErpBaseMvc.MVC.MvcPrepareUrl(dRow("AppURL").ToString(), dRow("AppID").ToString(), m_User.UserName)
                    Else
                        dRow("AppURL") = GetUrlString(dRow("AppURL").ToString(), dRow("AppID").ToString())
                    End If
                    dRow("AppURL") = dRow("AppURL").ToString() & "&cp=" & GetUserContextParams(dRow("AppID").ToString())
                Next


                Dim dvAppLinks As DataView = New DataView(appTable)
                dvAppLinks.RowFilter = "AppType='Report' AND AppInMenu=1"
                MenuReportSource = New DataView(appTable)
                MenuReportSource.RowFilter = "AppType='Report' AND AppInMenu=1"

                dvAppLinks.RowFilter = "AppType='Change' AND AppInMenu=1"
                MenuChangeSource = New DataView(appTable)
                MenuChangeSource.RowFilter = "AppType='Change' AND AppInMenu=1"

                dvAppLinks.RowFilter = "AppType='Admin' AND AppInMenu=1"
                MenuAdminSource = New DataView(appTable)
                MenuAdminSource.RowFilter = "AppType='Admin' AND AppInMenu=1"

                dvAppLinks.RowFilter = "AppType='Helpdesk' AND AppInMenu=1"
                MenuHelpDeskSource = New DataView(appTable)
                MenuHelpDeskSource.RowFilter = "AppType='Helpdesk' AND AppInMenu=1"


                If m_User.PasswordExpired And Not m_User.InitialPassword = True Then
                    Try
                        ibtnInfo.Enabled = False
                    Catch DoNothing As Exception
                    End Try
                ElseIf m_User.InitialPassword = True Then
                    Try
                        ibtnInfo.Enabled = False
                    Catch DoNothing As Exception
                    End Try
                Else
                    ibtnInfo.Enabled = True
                End If

                If (Not Request.QueryString("pwdreq") Is Nothing) _
                    AndAlso (Request.QueryString("pwdreq") = "true") Then
                    Try
                        ibtnInfo.Enabled = False
                    Catch DoNothing As Exception
                    End Try
                End If

                'GridChange.

            Catch ex As Exception

            Finally
                conn.Close()
            End Try
        End If
    End Sub

    Function GetUserContextParams(appID As String) As String
        Dim userObject As User = ErpBaseMvc.MVC.GetSessionUserObject()
        If (userObject Is Nothing Or userObject.Customer Is Nothing) Then
            Return ""
        End If

        Return String.Format("{0}_{1}_{2}_{3}_{4}", appID, userObject.UserID, userObject.Customer.CustomerId, userObject.Customer.KUNNR, Log.PortalType)
    End Function

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