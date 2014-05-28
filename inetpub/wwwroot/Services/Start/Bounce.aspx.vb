Public Partial Class Bounce
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            Dim blnDatabaseEntries As Boolean = (ConfigurationManager.AppSettings("DatabaseEntries") = "ON")

            Dim strURL As String = Me.Request.QueryString("ReturnURL").ToString
            If strURL = String.Empty Then
                If Not Me.Request.UrlReferrer Is Nothing Then
                    strURL = Me.Request.UrlReferrer.AbsoluteUri
                    If blnDatabaseEntries Then
                        WriteLog("Request.UrlReferrer.AbsoluteUri: " & Me.Request.UrlReferrer.AbsoluteUri & ", Request.UrlReferrer.AbsolutePath: " & Me.Request.UrlReferrer.AbsolutePath)
                    End If
                End If
            Else
                If blnDatabaseEntries Then
                    WriteLog("Request.QueryString(""ReturnURL""): " & Me.Request.QueryString("ReturnURL").ToString)
                End If
            End If
            If InStr(UCase(strURL), "?") > 0 Then
                strURL = Left(strURL, InStr(UCase(strURL), "?") - 1)
            End If
            If Not strURL = String.Empty Then
                'Response.Redirect(strURL & "?Logon=open", True)
                Response.Redirect(strURL)
                'Response.Redirect("http://www.kroschke.de")
            Else
                WriteError()
            End If
        Catch ex As Exception
            WriteError()
        End Try
    End Sub

    Private Sub WriteLog(ByVal strMessage As String)
        Dim conn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))

        Try
            Dim cmdWriteLog As New SqlClient.SqlCommand("INSERT INTO LogBounce (Message,UserHostAddress) VALUES (@Message,@UserHostAddress)", conn)

            conn.Open()

            cmdWriteLog.Parameters.AddWithValue("@Message", strMessage)
            cmdWriteLog.Parameters.AddWithValue("@UserHostAddress", Me.Request.UserHostAddress)
            cmdWriteLog.ExecuteNonQuery()
            cmdWriteLog.Dispose()
        Catch ex As Exception
        Finally
            If conn.State <> ConnectionState.Closed Then
                conn.Close()
            End If
        End Try
    End Sub

    Private Sub WriteError()
        Response.Write("<b>Unable to redirect!</b>")
        Response.Write("<BR>")
        Response.Write("ReturnURL: ")
        Response.Write(Me.Request.QueryString("ReturnURL"))
        Response.Write("<BR>")
        Response.Write("UrlReferrer: ")
        Response.Write(Me.Request.UrlReferrer)
        Response.Write("<BR>")
        Response.Write("RawUrl: ")
        Response.Write(Me.Request.RawUrl)
        Response.Write("<BR>")
        Response.Write("Url.AbsoluteUri: ")
        Response.Write(Me.Request.Url.AbsoluteUri)
        Response.Write("<BR>")
    End Sub
End Class