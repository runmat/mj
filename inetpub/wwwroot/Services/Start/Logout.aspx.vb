Imports CKG.Base.Kernel.Common.Common

Partial Public Class Logout
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        System.Web.Security.FormsAuthentication.SignOut()

        On Error Resume Next
        Session("logoutMode") = "OK"


        If (TryUrlRemoteUserLogoutAndRedirect()) Then Exit Sub


        Dim m_User As CKG.Base.Kernel.Security.User
        m_User = GetUser(Me)

        Dim intPause As Integer = 0
        Table1.Visible = False
        Table2.Visible = False
        FormDiv.Visible = False

        'Prüfe zugreifende IP
        Dim intRestrictedCustomerId As Integer = CheckRestrictedIP()
        If intRestrictedCustomerId > -1 Then
            'Aha, Benutzer greift von IP zu, die Beschränkungen unterliegt

            'Ermittele Standard-Benutzer
            Dim strIpStandardUser As String = GiveIpStandardUser(intRestrictedCustomerId)
            If m_User.UserName.ToUpper = strIpStandardUser.ToUpper Then
                Table2.Visible = True
                FormDiv.Visible = True
                intPause = -1
            End If
        End If

        If intPause > -1 Then
            m_User.SetLoggedOn(m_User.UserName, False)

            Session.Abandon()
            System.Web.Security.FormsAuthentication.RedirectFromLoginPage(m_User.UserID.ToString, False)
            Response.Redirect("Bounce.aspx?ReturnUrl=Login.aspx?Logon=open")

        End If

        'Das hier ruft die Ereignisbehandlung in der Global.aspx.vb auf:
        Session.Abandon()
    End Sub

    Private Function GiveIpStandardUser(ByVal intCust As Integer) As String
        'Ermittele IpStandardUser des Kunden
        Dim result As Object
        Dim conn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Dim command As SqlClient.SqlCommand
        Dim strReturn As String = ""

        Try
            conn.Open()

            command = New SqlClient.SqlCommand("SELECT IpStandardUser FROM Customer" & _
                    " WHERE" & _
                    " CustomerID = " & intCust.ToString, _
                    conn)

            result = command.ExecuteScalar
            If Not result Is Nothing Then
                strReturn = CStr(result)
            End If
        Catch ex As Exception
            '?
        Finally
            conn.Close()
            conn.Dispose()
        End Try
        Return strReturn
    End Function

    Private Function CheckRestrictedIP() As Integer
        'Prüfe, ob IP in DB existent

        Dim result As Object
        Dim conn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Dim command As SqlClient.SqlCommand
        Dim intReturn As Integer = -1

        Try
            conn.Open()

            command = New SqlClient.SqlCommand("SELECT CustomerID FROM IpAddresses" & _
                    " WHERE" & _
                    " IpAddress = '" & Request.UserHostAddress & "'", _
                    conn)

            result = command.ExecuteScalar
            If Not result Is Nothing AndAlso IsNumeric(result) Then
                intReturn = CInt(result)
            End If
        Catch ex As Exception
            '?
        Finally
            conn.Close()
            conn.Dispose()
        End Try
        Return intReturn
    End Function

    Function TryUrlRemoteUserLogoutAndRedirect() As Boolean
        Dim urlRemoteLogin_LogoutUrl As String = Session("UrlRemoteLogin_LogoutUrl")
        If (Not String.IsNullOrEmpty(urlRemoteLogin_LogoutUrl)) Then
            urlRemoteLogin_LogoutUrl = urlRemoteLogin_LogoutUrl.ToLower
            If (Not urlRemoteLogin_LogoutUrl.StartsWith("http")) Then urlRemoteLogin_LogoutUrl = "http://" & urlRemoteLogin_LogoutUrl

            Response.Redirect(urlRemoteLogin_LogoutUrl)
            Return True
        End If

        Return False
    End Function

End Class
