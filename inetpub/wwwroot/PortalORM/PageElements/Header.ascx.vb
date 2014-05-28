Imports CKG.Base.Kernel
Imports System.Text

Namespace PageElements
    Public MustInherit Class Header
        Inherits System.Web.UI.UserControl
        Protected WithEvents litSetBackground As System.Web.UI.WebControls.Literal
        Protected WithEvents lnkHauptmenue As System.Web.UI.WebControls.HyperLink
        Protected WithEvents lnkChangePassword As System.Web.UI.WebControls.HyperLink
        Protected WithEvents lnkLogout As System.Web.UI.WebControls.HyperLink
        Protected WithEvents lnkHandbuch As System.Web.UI.WebControls.HyperLink
        Protected WithEvents lnkContact As System.Web.UI.WebControls.HyperLink
        Protected WithEvents lnkResponsible As System.Web.UI.WebControls.HyperLink
        Protected WithEvents lnkImpressum As System.Web.UI.WebControls.HyperLink
        Protected WithEvents lnkQuickSupport As System.Web.UI.WebControls.HyperLink
        Protected WithEvents tdHandbuch As System.Web.UI.HtmlControls.HtmlTableCell
        Protected WithEvents tdResponsible As System.Web.UI.HtmlControls.HtmlTableCell
        Protected WithEvents tdChangePasword As System.Web.UI.HtmlControls.HtmlTableCell
        Protected WithEvents tdMessage As System.Web.UI.HtmlControls.HtmlTableCell
        Protected WithEvents imgCustomerLogo As System.Web.UI.WebControls.Image
        Protected WithEvents imgDADLogo As System.Web.UI.HtmlControls.HtmlImage
        Protected WithEvents lblUserName As System.Web.UI.WebControls.Label
        Protected WithEvents tdHauptmenue As System.Web.UI.HtmlControls.HtmlTableCell
        Protected WithEvents tdQuickSupport As System.Web.UI.HtmlControls.HtmlTableCell
        Protected WithEvents tdLogout As System.Web.UI.HtmlControls.HtmlTableCell
        Protected WithEvents imgMessage As System.Web.UI.HtmlControls.HtmlImage
        Protected PostBackStr As String

#Region " Vom Web Form Designer generierter Code "

        'Dieser Aufruf ist für den Web Form-Designer erforderlich.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: Diese Methode ist für den Web Form-Designer erforderlich
            'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
            InitializeComponent()
        End Sub


#End Region

        Private m_User As Security.User

        Public Sub InitUser(ByVal _User As Security.User)
            m_User = _User
        End Sub

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            ' Hier Benutzercode zur Seiteninitialisierung einfügen
            Dim strLogoPath As String = ""
            Dim strLogoPath2 As String = ""
            Dim strDocuPath As String = ""

            imgDADLogo.Alt = Me.Page.User.Identity.Name
            Me.tdHandbuch.Visible = False
            PostBackStr = Page.ClientScript.GetPostBackEventReference(Me, "MyCustomArgument")
            If Page.IsPostBack Then
                Dim eventArg As String = Request("__EVENTARGUMENT")
                If eventArg = "Logout" Then
                    Response.Redirect("/PortalORM/(S(" & Session.SessionID & "))/Start/Logout.aspx")
                End If
            End If
            If Not m_User Is Nothing Then
                With Me
                    If Not IsPostBack Then
                        lnkHauptmenue.NavigateUrl = "/PortalORM/(S(" & Session.SessionID & "))/Start/Selection.aspx"
                        lnkChangePassword.NavigateUrl = "/PortalORM/(S(" & Session.SessionID & "))/Start/ChangePassword.aspx"
                        lnkLogout.NavigateUrl = "/PortalORM/(S(" & Session.SessionID & "))/Start/Logout.aspx"
                        lnkContact.NavigateUrl = "/PortalORM/(S(" & Session.SessionID & "))/Info/ContactPage.aspx"
                        lnkResponsible.NavigateUrl = "/PortalORM/(S(" & Session.SessionID & "))/Info/ResponsiblePage.aspx"
                        lnkImpressum.NavigateUrl = "/PortalORM/(S(" & Session.SessionID & "))/Info/Impressum.aspx"
                        tdQuickSupport.Visible = True
                    End If

                    If m_User.Customer.ShowsTeamViewer Then
                        tdQuickSupport.Visible = True
                    Else
                        tdQuickSupport.Visible = False
                    End If

                    If (m_User.Customer.LogoPath2 Is Nothing) OrElse (m_User.Customer.LogoPath2 = String.Empty) Then
                        strLogoPath2 = ""
                        imgDADLogo.Src = ""

                    Else
                        strLogoPath2 = m_User.Customer.LogoPath2
                        imgDADLogo.Src = strLogoPath2
                    End If

                    '------------------------------------------------------

                    If m_User.GroupID > 0 Then
                        strLogoPath = m_User.Organization.LogoPath
                        strDocuPath = m_User.Groups.ItemByID(m_User.GroupID).DocuPath

                        Dim result As New DataTable()

                        Dim cn As SqlClient.SqlConnection
                        cn = New SqlClient.SqlConnection(m_User.App.Connectionstring)
                        cn.Open()

                        Dim daApp As New SqlClient.SqlDataAdapter("SELECT *, Name2 + ',' + Name1 + ' (' + [position] + ')' AS EmployeeName" & _
                                                                  " FROM Contact INNER JOIN ContactGroups ON Contact.id = ContactGroups.ContactID " & _
                                                                  " WHERE     (ContactGroups.CustomerID = @CustomerID) AND (ContactGroups.GroupID = @GroupID)", cn)


                        daApp.SelectCommand.Parameters.AddWithValue("@CustomerID", m_User.Customer.CustomerId)
                        daApp.SelectCommand.Parameters.AddWithValue("@GroupID", m_User.GroupID)
                        daApp.Fill(result)
                        cn.Close()

                        If result.DefaultView.Count > 0 Then
                            tdResponsible.Visible = True
                        Else
                            tdResponsible.Visible = False
                        End If
                    End If
                    If strDocuPath = String.Empty Then
                        strDocuPath = m_User.Customer.DocuPath
                    End If
                    If Not strDocuPath = String.Empty Then
                        .tdHandbuch.Visible = True
                        .lnkHandbuch.NavigateUrl = strDocuPath
                    End If

                    If strLogoPath = String.Empty Then
                        strLogoPath = m_User.Customer.CustomerStyle.LogoPath
                    End If
                    If Not strLogoPath = String.Empty Then
                        .imgCustomerLogo.Visible = True
                        .imgCustomerLogo.ImageUrl = strLogoPath
                    End If
                    .lnkChangePassword.Visible = True
                    lnkLogout.Visible = True
                    .imgDADLogo.Alt &= vbCrLf & m_User.UserID
                    .lblUserName.Text = "Benutzer: " & m_User.UserName

                    .litSetBackground.Visible = False
                    If m_User.IsTestUser Then
                        .litSetBackground.Visible = True
                        .litSetBackground.Text = "		<script language=""JavaScript"">" & vbCrLf & _
                                                 "<!-- //" & vbCrLf & _
                                                 " window.document.getElementsByTagName(""body"")[0].background = ""/PortalORM/Images/TestUser.JPG"";" & vbCrLf & _
                                                 "//-->" & vbCrLf & _
                                                 "		</script>"
                    Else
                        If ConfigurationManager.AppSettings("ShowProductiveBackground") = "ON" Then
                            .litSetBackground.Visible = True
                            .litSetBackground.Text = "		<script language=""JavaScript"">" & vbCrLf & _
                                                     "<!-- //" & vbCrLf & _
                                                     " window.document.getElementsByTagName(""body"")[0].background = ""/PortalORM/Images/ProdUser.JPG"";" & vbCrLf & _
                                                     "//-->" & vbCrLf & _
                                                     "		</script>"
                        End If
                    End If
                    displayMessages()
                End With
            Else
                imgDADLogo.Src = "/PortalORM/Images/empty.gif"
                imgCustomerLogo.Visible = True
                imgCustomerLogo.ImageUrl = "/PortalORM/Images/Armaturenbrett.jpg"
                tdChangePasword.Visible = False
                tdLogout.Visible = False
                lnkHauptmenue.Text = "Anmeldung"
                lnkHauptmenue.NavigateUrl = "/PortalORM/Start/Login.aspx"
                tdQuickSupport.Visible = False

            End If
        End Sub

        Public Sub HideLinks()
            With Me
                Me.tdChangePasword.Visible = False
                Me.tdHandbuch.Visible = False
                Me.tdHauptmenue.Visible = False
            End With
        End Sub

        Public Sub LockLinks()
            With Me
                .lnkChangePassword.Enabled = False
                .lnkHandbuch.Enabled = False
                .lnkHauptmenue.Enabled = False
                .lnkImpressum.Enabled = False
                .lnkContact.Enabled = False
                .lnkResponsible.Enabled = False
            End With
        End Sub

        Private Sub displayMessages()
            'Nachrichten generieren  

            Dim table As DataTable
            Dim row As System.Data.DataRow
            Dim command As New SqlClient.SqlCommand()

            Dim datTime As DateTime = CDate("01.01.1900 " & Now.ToShortTimeString)  '01.01.1900 !!!

            command.CommandText = "SELECT id,(convert(varchar,creationDate,104) + ' ' + convert(varchar,creationdate,108) + ' - ' + titleText) as titleText,messageText,enableLogin,onlyTEST,onlyPROD,activeTimeFrom FROM LoginMessage" & _
                    " WHERE" & _
                    " datediff(minute,DATEADD(minute,15,getdate()),convert(varchar,activeDateFrom,104)+' '+convert(varchar,activeTimeFrom,108)) <=0" & _
                    " and" & _
                    " datediff(minute,getdate(),convert(varchar,activeDateTo,104)+' '+convert(varchar,activeTimeTo,108)) >=0" & _
                    " and active <> 0 and messagecolor = 0" & _
                    " ORDER BY id DESC"

            Try
                Dim conn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
                Dim da As New SqlClient.SqlDataAdapter(command)
                command.Connection = conn
                conn.Open()
                table = New DataTable()
                da.Fill(table)
                conn.Close()
                conn.Dispose()
                da.Dispose()

                Dim text As String
                Dim htext As String

                For Each row In table.Rows
                    text = CType(row.Item("titleText"), String)     '--- Überschrift formatieren                
                    text = text.Replace("{c=", "{font color=")
                    text = text.Replace("{/c}", "{/font}")
                    text = text.Replace("{", "<")
                    text = text.Replace("}", ">")
                    row.Item("titleText") = text

                    text = CType(row.Item("messageText"), String)   '--- Nachricht formatieren
                    If text.IndexOf("{h}") > 0 Then
                        htext = text.Substring(text.IndexOf("{h}") + 3, text.IndexOf("{/h}") - text.IndexOf("{h}") - 3)
                        text = text.Replace("{h}", "<a href=""")
                        text = text.Replace("{/h}", """ target = ""_BLANK"">" & htext & "</a>")
                    End If
                    text = text.Replace("{c=", "{font color=")
                    text = text.Replace("{/c}", "{/font}")
                    text = text.Replace("{", "<")
                    text = text.Replace("}", ">")
                    row.Item("messageText") = text

                    table.AcceptChanges()
                    Dim prooftime As DateTime = Nothing

                    If CType(row.Item("onlyPROD"), Integer) = 0 Then

                        If IsDate(row.Item("activeTimeFrom")) Then
                            Dim tempTime As Date = "01.01.1900"
                            tempTime = DateAdd(DateInterval.Hour, Now.Hour, tempTime)
                            tempTime = DateAdd(DateInterval.Minute, Now.Minute, tempTime)
                            tempTime = DateAdd(DateInterval.Second, Now.Second, tempTime)
                            Dim secForImage As Long = DateDiff(DateInterval.Second, tempTime, CDate(row.Item("activeTimeFrom")))
                            Dim secondsTo As Long = 60 - Now.Second
                            tdMessage.Visible = True

                            Dim cs As ClientScriptManager = Page.ClientScript
                            Dim csname1 As String = "FunctionScript"
                            Dim cstype As Type = Me.GetType()
                            If Not (m_User.HighestAdminLevel = Security.AdminLevel.Master) Then
                                If (Not cs.IsStartupScriptRegistered(cstype, csname1)) Then
                                    Dim Dmin As Long
                                    Dmin = DateDiff(DateInterval.Minute, tempTime, CDate(row.Item("activeTimeFrom")))

                                    Dim cstext1 As String = " cd('" & Dmin & "','" & secondsTo & "' );"
                                    cs.RegisterStartupScript(cstype, csname1, cstext1, True)

                                End If
                            End If


                            If secForImage <= 300 Then
                                imgMessage.Src = "../Images/Achtung2.gif"
                            End If
                        End If
                    End If
                Next
                Session("App_AdminMessage") = table
            Catch ex As Exception

            End Try

        End Sub


    End Class
End Namespace

' ************************************************
' $History: Header.ascx.vb $
' 
' *****************  Version 8  *****************
' User: Dittbernerc  Date: 9.05.11    Time: 13:39
' Updated in $/CKAG/PortalORM/PageElements
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 22.07.09   Time: 12:57
' Updated in $/CKAG/PortalORM/PageElements
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 21.07.09   Time: 14:33
' Updated in $/CKAG/PortalORM/PageElements
' ITA:3008
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 2.06.09    Time: 14:19
' Updated in $/CKAG/PortalORM/PageElements
' ITA: 2804
' 
' *****************  Version 4  *****************
' User: Hartmannu    Date: 9.09.08    Time: 13:42
' Updated in $/CKAG/PortalORM/PageElements
' ITA 2152 und 2158
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 11.04.08   Time: 11:57
' Updated in $/CKAG/PortalORM/PageElements
' Migration OR
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 4.04.08    Time: 16:05
' Updated in $/CKAG/PortalORM/PageElements
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 3.04.08    Time: 17:17
' Created in $/CKAG/PortalORM/PageElements
' 
' *****************  Version 16  *****************
' User: Uha          Date: 4.09.07    Time: 9:59
' Updated in $/CKG/PortalORM/PageElements
' ITA 1280: Bugfixing
' 
' *****************  Version 15  *****************
' User: Uha          Date: 31.05.07   Time: 13:43
' Updated in $/CKG/PortalORM/PageElements
' ITA 0844 Architektur
' ITA 1077 Doppeltes Login
' 
' *****************  Version 14  *****************
' User: Uha          Date: 2.05.07    Time: 10:55
' Updated in $/CKG/PortalORM/PageElements
' Pfadangaben in den Links (absolut und ohne Session-ID) führten zum
' Neuaufbau der Session und damit zu unerwarteten Resultaten z.B. beim
' Logging und beim Abmelden. => Session in Pfade eingesetzt
' 
' *****************  Version 13  *****************
' User: Uha          Date: 13.03.07   Time: 14:56
' Updated in $/CKG/PortalORM/PageElements
' Zusätzliches Hintergrundbild für produktive Benutzer
' 
' *****************  Version 12  *****************
' User: Uha          Date: 12.03.07   Time: 10:13
' Updated in $/CKG/PortalORM/PageElements
' 
' *****************  Version 11  *****************
' User: Uha          Date: 12.03.07   Time: 8:58
' Updated in $/CKG/PortalORM/PageElements
' im Imagepfad alte Start-Anwendung durch "Portal" ersetzt
' 
' *****************  Version 10  *****************
' User: Uha          Date: 1.03.07    Time: 16:38
' Updated in $/CKG/PortalORM/PageElements
' 
' ************************************************