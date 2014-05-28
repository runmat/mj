Option Strict On
Option Explicit On

Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common

Partial Public Class Menue
    Inherits System.Web.UI.UserControl

    Private m_User As Security.User
    Private m_App As Security.App
    Protected WithEvents tbAnwendungen As Global.System.Web.UI.HtmlControls.HtmlTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.Page.Session.Count = 0 Then
            m_User = GetUser(Me.Page)
            m_App = New Security.App(m_User)

            Dim conn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
            conn.Open()
            Try

                Dim table As DataTable
                Dim command As New SqlClient.SqlCommand()
                Dim blnReturn As Boolean = True

                command.CommandText = "SELECT AppType,DisplayName,ButtonPath FROM ApplicationType ORDER BY Rank"


                Dim da As New SqlClient.SqlDataAdapter(command)
                command.Connection = conn

                table = New DataTable()
                da.Fill(table)

                MenuePunkte.DataSource = table
                MenuePunkte.DataBind()

                For Each tmpItem As RepeaterItem In MenuePunkte.Items
                    Dim tmpRepeater As Repeater = CType(tmpItem.FindControl("appLinks"), Repeater)

                    Dim dvAppLinks As DataView = New DataView(m_User.Applications)
                    dvAppLinks.RowFilter = "AppType='" & table.Rows(tmpItem.ItemIndex)("AppType").ToString & "' AND AppInMenu=1"


                    If Not dvAppLinks.Count = 0 Then
                        tmpRepeater.DataSource = dvAppLinks
                        tmpRepeater.DataBind()
                    Else
                        tmpItem.FindControl("menuePunkteContainer").Visible = False
                    End If
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
            Return (strAppUrl).Replace("AppVFS", "AppGenerali")
        Else
            Return (strAppUrl & "?AppID=" & strAppID & paramlist).Replace("AppVFS", "AppGenerali")
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