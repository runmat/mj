Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common

Partial Public Class _Report02s
    Inherits System.Web.UI.Page
    Private m_App As Security.App
    Private m_User As Security.User

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            m_App = New Base.Kernel.Security.App(m_User)

            DoSubmit()
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub
    Private Sub DoSubmit()
        Dim sql As String
        Dim table As DataTable
        Dim command As New SqlClient.SqlCommand()
        Dim blnReturn As Boolean = True
        Dim intAppID As Int32



        sql = " SELECT "
        sql = sql & "   Application_1.AppID"
        sql = sql & " FROM "
        sql = sql & "   dbo.Application INNER JOIN"
        sql = sql & "   dbo.Application Application_1 ON dbo.Application.AppID = Application_1.AppParent"
        sql = sql & " WHERE "
        sql = sql & "   (dbo.Application.AppURL = '../Components/ComArchiv/_Report02s.aspx') AND"
        sql = sql & "   (Application_1.AppURL = '../Components/ComArchiv/Report00s.aspx')"



        command.CommandText = sql

        Dim conn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Dim da As New SqlClient.SqlDataAdapter(command)
        command.Connection = conn
        conn.Open()
        table = New DataTable()
        da.Fill(table)

        If table.Rows.Count > 0 Then


            intAppID = table.Rows(0).Item("AppID")


            table = Nothing

        End If


        sql = " SELECT     EasyLagerortName"
        sql = sql & " FROM         dbo.Archiv"
        sql = sql & " GROUP BY EasyLagerortName"
        sql = sql & " HAVING      (EasyLagerortName <> N'FORD')"
        sql = sql & " Union"
        sql = sql & " SELECT     EasyLagerortName + ' ' + substring(EasyArchivname,1,3) as EasyLagerortName"
        sql = sql & " FROM         dbo.Archiv"
        sql = sql & " GROUP BY EasyLagerortName,EasyArchivname"
        sql = sql & " HAVING      (EasyLagerortName = 'FORD')"


        command.CommandText = sql
        table = New DataTable()
        da.Fill(table)


        Try
            If table.Rows.Count > 0 Then
                Dim tmpRow As DataRow
                For Each tmpRow In table.Rows
                    Dim trApp As HtmlTableRow
                    Dim tdApp2 As HtmlTableCell
                    Dim litApp As Literal

                    trApp = New HtmlTableRow()
                    trApp.Attributes.Add("class", "formquery")
                    litApp = New Literal()
                    litApp.Text = "&nbsp;"
                    tdApp2 = New HtmlTableCell()
                    tdApp2.Attributes.Add("class", "firstLeft active")
                    tdApp2.Attributes.Add("nowrap", "nowrap")
                    tdApp2.Width = "100%"
                    Dim blnAlternate As Boolean = False
                    litApp = New Literal()
                    litApp.Text = "<table cellspacing=""0"" border=""0"" style=""width:100%;border-collapse:collapse;"">" & vbCrLf

                    Dim appUrl As String
                    Dim appSymbol As String

                    appUrl = "../../Components/ComArchiv/Report00s.aspx?AppID=" & CStr(intAppID) & "&ELN=" & CStr(tmpRow("EasyLagerortName"))

                    '--------------------------------------
                    appSymbol = "../../Images/arrowgrey.gif"
                    litApp.Text &= "<td><img style=""width: 16px; height: 16px"" src=""" & appSymbol & """ border=""0"" /></td>" & vbCrLf
                    litApp.Text &= "<td nowrap=""nowrap"">&nbsp;<a class=""LinksVerwaltung"" href=""" & appUrl & """ target=""_self"">" & CStr(tmpRow("EasyLagerortName")) & "</a>&nbsp;</td>" & vbCrLf

                    '---------------------------------------------
                    litApp.Text &= "<td style=""width:100%;"">&nbsp;&nbsp;<span></span></td>" & vbCrLf

                    litApp.Text &= "</tr>" & vbCrLf

                    litApp.Text &= "</table>" & vbCrLf
                    tdApp2.Controls.Add(litApp)
                    trApp.Cells.Add(tdApp2)
                    tbAnwendungen.Rows.Add(trApp)
                Next
            End If
            conn.Close()
            conn.Dispose()
            da.Dispose()

        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "Selection", "Page_Load", ex.ToString)

            lblError.Text = "Fehler bei der Ermittlung der Menüpunkte (" & ex.Message & ")"
            lblError.Visible = True
        End Try
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnback.Click
        Response.Redirect("/Services/start/Selection.aspx?AppID=" & Session("AppID").ToString)
    End Sub
End Class