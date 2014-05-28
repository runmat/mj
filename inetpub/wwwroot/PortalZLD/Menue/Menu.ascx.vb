Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports Telerik.Web.UI

Public Class Menu
    Inherits System.Web.UI.UserControl

    Private m_User As Security.User
    Private m_App As Security.App

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("objUser") Is Nothing Then

            m_User = CType(Session("objUser"), Security.User)

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

                Dim AppTable As DataTable = m_User.Applications.Copy

                Dim dRow As DataRow
                For Each dRow In AppTable.Rows
                    dRow.Item(5) = GetUrlString(dRow.Item(5).ToString, dRow.Item(1).ToString)
                Next

                For Each NewRow As DataRow In AppTable.Select("AppType='Admin' AND AppInMenu=1")

                    Dim newItme As New RadMenuItem(NewRow("AppFriendlyName").ToString, NewRow("AppUrl").ToString)
                    RadMenu1.Items(0).Items.Add(newItme)
                Next

                If AppTable.Select("AppType='Admin' AND AppInMenu=1").Length > 0 Then
                    Dim newSeperator = New RadMenuItem()
                    newSeperator.IsSeparator = True
                    RadMenu1.Items(0).Items.Add(newSeperator)
                End If




                For Each NewRow As DataRow In AppTable.Select("AppType='Change' AND AppInMenu=1")
                    Dim newItme As New RadMenuItem(NewRow("AppFriendlyName").ToString, NewRow("AppUrl").ToString)
                    RadMenu1.Items(0).Items.Add(newItme)
                Next
                If AppTable.Select("AppType='Change' AND AppInMenu=1").Length > 0 Then
                    Dim newSeperator = New RadMenuItem()
                    newSeperator.IsSeparator = True
                    RadMenu1.Items(0).Items.Add(newSeperator)
                End If



                For Each NewRow As DataRow In AppTable.Select("AppType='Report' AND AppInMenu=1")
                    Dim newItme As New RadMenuItem(NewRow("AppFriendlyName").ToString, NewRow("AppUrl").ToString)
                    newItme.Target = Nothing
                    RadMenu1.Items(0).Items.Add(newItme)
                Next

                If AppTable.Select("AppType='Report' AND AppInMenu=1").Length > 0 Then
                    Dim newSeperator = New RadMenuItem()
                    newSeperator.IsSeparator = True
                    RadMenu1.Items(0).Items.Add(newSeperator)
                End If


                For Each NewRow As DataRow In AppTable.Select("AppType='Helpdesk' AND AppInMenu=1")
                    Dim newItme As New RadMenuItem(NewRow("AppFriendlyName").ToString, NewRow("AppUrl").ToString)
                    RadMenu1.Items(0).Items.Add(newItme)
                Next



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