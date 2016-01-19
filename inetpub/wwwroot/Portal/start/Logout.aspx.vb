Imports CKG.Base.Kernel.Common.Common

Namespace Start
    Public Class Logout
        Inherits System.Web.UI.Page
        Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable
        Protected WithEvents Table2 As System.Web.UI.HtmlControls.HtmlTable
        Protected WithEvents HyperLink1 As System.Web.UI.WebControls.HyperLink
        Protected WithEvents Literal1 As System.Web.UI.WebControls.Literal

#Region " Web Form Designer Generated Code "

        'This call is required by the Web Form Designer.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()
        End Sub

#End Region

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            System.Web.Security.FormsAuthentication.SignOut()

            On Error Resume Next
            Session("logoutMode") = "OK"

            Dim m_User As CKG.Base.Kernel.Security.User
            m_User = GetUser(Me)

            Dim intPause As Integer = 0
            Table1.Visible = False
            Table2.Visible = False

            'Prüfe zugreifende IP
            Dim intRestrictedCustomerId As Integer = CheckRestrictedIP()
            If intRestrictedCustomerId > -1 Then
                'Aha, Benutzer greift von IP zu, die Beschränkungen unterliegt

                'Ermittele Standard-Benutzer
                Dim strIpStandardUser As String = GiveIpStandardUser(intRestrictedCustomerId)
                If m_User.UserName.ToUpper = strIpStandardUser.ToUpper Then
                    Table2.Visible = True
                    intPause = -1
                End If
            End If

            If Not String.IsNullOrEmpty(m_User.Customer.LogoutLink) Then
                Session.Abandon()
                Response.Redirect(m_User.Customer.LogoutLink, True)
            End If

            If intPause > -1 Then
                If (Request.QueryString("DoubleLogin") Is Nothing) OrElse (Not Request.QueryString("DoubleLogin").ToString = "True") Then
                    m_User.SetLoggedOn(m_User.UserName, False)
                Else
                    intPause = 4000
                    Table1.Visible = True
                End If

                Session.Abandon()
                System.Web.Security.FormsAuthentication.RedirectFromLoginPage(m_User.UserID.ToString, False)
                Response.Redirect("/services", True)

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
    End Class
End Namespace

' ************************************************
' $History: Logout.aspx.vb $
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 28.04.09   Time: 13:45
' Updated in $/CKAG/portal/Start
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 27.04.09   Time: 11:14
' Updated in $/CKAG/portal/Start
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 11.04.08   Time: 11:57
' Updated in $/CKAG/portal/Start
' Migration OR
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 10.04.08   Time: 17:37
' Updated in $/CKAG/portal/Start
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 3.04.08    Time: 17:21
' Created in $/CKAG/portal/start
' 
' *****************  Version 12  *****************
' User: Uha          Date: 22.01.08   Time: 11:40
' Updated in $/CKG/Portal/Start
' 'ITA 1644: Logout für Standarduser (mit hinterlegter IP) führt auf
' Logout-Seite ohne Weiterleitung
' 
' *****************  Version 11  *****************
' User: Uha          Date: 22.01.08   Time: 10:35
' Updated in $/CKG/Portal/Start
' 
' *****************  Version 10  *****************
' User: Uha          Date: 7.08.07    Time: 16:49
' Updated in $/CKG/Portal/Start
' 
' *****************  Version 9  *****************
' User: Uha          Date: 31.05.07   Time: 11:41
' Updated in $/CKG/Portal/Start
' ITA 1077 - Login bei bereits aktiver Anmeldung ermöglichen
' 
' *****************  Version 8  *****************
' User: Uha          Date: 15.03.07   Time: 18:13
' Updated in $/CKG/Portal/Start
' Fehler: Logon-Flag wird bei Abmeldung wieder zurückgesetzt
' 
' *****************  Version 7  *****************
' User: Uha          Date: 14.03.07   Time: 12:51
' Updated in $/CKG/Portal/Start
' Interne Reihenfolge geändert
' 
' *****************  Version 6  *****************
' User: Uha          Date: 14.03.07   Time: 12:42
' Updated in $/CKG/Portal/Start
' Code aus Global - Session_End übernommen
' 
' *****************  Version 5  *****************
' User: Uha          Date: 12.03.07   Time: 10:14
' Updated in $/CKG/Portal/Start
' 
' *****************  Version 4  *****************
' User: Uha          Date: 5.03.07    Time: 17:17
' Updated in $/CKG/Portal/Start
' im Imagepfad alte Start-Anwendung durch "Portal" ersetzt
' 
' *****************  Version 3  *****************
' User: Uha          Date: 1.03.07    Time: 16:38
' Updated in $/CKG/Portal/Start
' 
' ************************************************