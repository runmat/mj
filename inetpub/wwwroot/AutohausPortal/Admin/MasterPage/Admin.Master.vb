Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common

Partial Public Class Admin
    Inherits System.Web.UI.MasterPage
    Private m_User As Security.User
    Private m_strTitleText As String

    Public Property TitleText() As String
        Get
            Return m_strTitleText
        End Get
        Set(ByVal Value As String)
            m_strTitleText = Value
        End Set
    End Property

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Dim strLogoPath As String = ""
        Dim strLogoPath2 As String = ""
        Dim strDocuPath As String = ""
        Dim strTitle As String
        Dim bc As HttpBrowserCapabilities
        bc = Request.Browser
        'm_User = GetUser(Page)
        ''imgDADLogo.Alt = Me.Page.User.Identity.Name
        'Me.
        lblCopyright.Text = lblCopyright.Text.Replace("year", DateTime.Now.Year.ToString())
        m_User = Session("objUser")



        If m_User Is Nothing Then
            '    With Me
            '        'UH: 02.05.2007
            '        'Fehler: Bei jeder Rückkekr ins Hauptmenü wird eine neue Session erzeugt.
            '        'Lösungsansatz: SessionID in URL mitgeben
            If Not IsPostBack Then
                tdHandbuch.Visible = False
                lnkHauptmenue.Text = "Anmeldung"
                tdChangePasword.Visible = False
                'tdLogout.Visible = False
                lblUserName.Visible = False
                imgLogo.Visible = False
                PlaceHeader.Visible = True
                '        End If

                '    End With
            Else

                '    imgDADLogo.Src = "/Portal/Images/empty.gif"
                '    imgCustomerLogo.Visible = True
                '    imgCustomerLogo.ImageUrl = "/Portal/Images/Armaturenbrett.jpg"

            End If
        Else
            imgLogo.Visible = True
            PlaceHeader.Visible = False
            lnkHauptmenue.NavigateUrl = "/AutohausPortal/(S(" & Session.SessionID & "))/Start/Selection.aspx"
            lnkChangePassword.NavigateUrl = "/AutohausPortal/(S(" & Session.SessionID & "))/Start/ChangePassword.aspx"
            lnkLogout.NavigateUrl = "/AutohausPortal/(S(" & Session.SessionID & "))/Start/Logout.aspx"
            lnkContact.NavigateUrl = "/AutohausPortal/(S(" & Session.SessionID & "))/Info/Kontakt.aspx"
            lnkResponsible.NavigateUrl = "/AutohausPortal/(S(" & Session.SessionID & "))/Info/ResponsiblePage.aspx"
            lnkImpressum.NavigateUrl = "/AutohausPortal/(S(" & Session.SessionID & "))/Info/Impressum.aspx"

            PlaceHeader.Visible = False

            lnkChangePassword.Visible = True
            lblUserName.Text = "Benutzer: " & m_User.UserName
            lnkLogout.Visible = True
            lnkChangePassword.Visible = True

            Dim strCSSLink As String = ""
            With bc
                If .Type = "IE6" Then

                    strCSSLink = "<link href=""/AutohausPortal/Admin/Styles/defaultIE6.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"
                    Select Case m_User.CustomerName
                        Case "Firma 1"
                            strCSSLink &= ("<link href=""/AutohausPortal/Admin/Styles/adminIE6.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />")
                            imgLogo.ImageUrl = "/AutohausPortal/Admin/Images/kroschke.jpg"
                        Case "DAD"
                            strCSSLink &= ("<link href=""/AutohausPortal/Admin/Styles/dadIE6.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />")
                        Case Else
                            strCSSLink &= "<link href=""" & m_User.Customer.CustomerStyle.CssPath & """ media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"
                            Dim strCSS() As String
                            Dim strCSSPath As String = m_User.Customer.CustomerStyle.CssPath
                            If strCSSPath.Contains(".css") Then
                                strCSS = strCSSPath.Split(".css")
                                If strCSS.Length = 2 Then
                                    strCSSPath = strCSS(0) & "IE6.css"
                                    strCSSLink &= "<link href=""" & strCSSPath & """ media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"
                                End If
                            End If
                    End Select
                Else
                    Dim strtemp As String = Server.MapPath("~/PortalZLD/Styles/default.css")
                    strCSSLink = "<link href=""/AutohausPortal/Admin/Styles/default.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"

                    Select Case m_User.CustomerName
                        Case "Firma 1"
                            strCSSLink &= ("<link href=""/AutohausPortal/Admin/Styles/admin.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />")
                            imgLogo.ImageUrl = "/AutohausPortal/Admin/Images/kroschke.jpg"
                        Case Else
                            strCSSLink &= "<link href=""" & m_User.Customer.CustomerStyle.CssPath & """ media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"
                    End Select
                End If
            End With
            Me.Head1.Controls.Add(New LiteralControl(strCSSLink))

            If HttpContext.Current.Request.UserAgent IsNot Nothing AndAlso HttpContext.Current.Request.UserAgent.ToLower().Contains("msie 10") Then
                Me.Head1.Controls.Add(New LiteralControl("<META content=""IE=9,chrome=1"" http-equiv=""X-UA-Compatible"">"))
            End If

            If Me.Page.User.Identity.IsAuthenticated Then
                Select Case Me.Page.Title
                    Case "Startseite"
                        tdHauptmenue.Attributes.Add("class", "active")
                        tdChangePasword.Attributes.Add("class", "")
                        'tdLogout.Attributes.Add("class", "")
                        tdContact.Attributes.Add("class", "")
                        tdImpressum.Attributes.Add("class", "")
                        tdResponsible.Attributes.Add("class", "")

                    Case "Änderung Passwort"
                        tdHauptmenue.Attributes.Add("class", "")
                        tdChangePasword.Attributes.Add("class", "active")
                        'tdLogout.Attributes.Add("class", "")
                        tdContact.Attributes.Add("class", "")
                        tdImpressum.Attributes.Add("class", "")
                        tdResponsible.Attributes.Add("class", "")
                    Case "Kontaktseite"
                        tdHauptmenue.Attributes.Add("class", "")
                        tdChangePasword.Attributes.Add("class", "")
                        ' tdLogout.Attributes.Add("class", "")
                        tdContact.Attributes.Add("class", "active")
                        tdImpressum.Attributes.Add("class", "")
                        tdResponsible.Attributes.Add("class", "")
                    Case "Impressum"
                        tdHauptmenue.Attributes.Add("class", "")
                        tdChangePasword.Attributes.Add("class", "")
                        'tdLogout.Attributes.Add("class", "")
                        tdContact.Attributes.Add("class", "")
                        tdImpressum.Attributes.Add("class", "active")
                        tdResponsible.Attributes.Add("class", "")
                    Case "Ansprechpartner"
                        tdHauptmenue.Attributes.Add("class", "")
                        tdChangePasword.Attributes.Add("class", "")
                        'tdLogout.Attributes.Add("class", "")
                        tdContact.Attributes.Add("class", "")
                        tdImpressum.Attributes.Add("class", "")
                        tdResponsible.Attributes.Add("class", "active")
                    Case Else
                        tdHauptmenue.Attributes.Add("class", "")
                        tdChangePasword.Attributes.Add("class", "")
                        'tdLogout.Attributes.Add("class", "")
                        tdContact.Attributes.Add("class", "")
                        tdImpressum.Attributes.Add("class", "")
                        tdResponsible.Attributes.Add("class", "")
                End Select

                If Me.Page.User.Identity.IsAuthenticated Then
                    If Me.Page.Title = "Startseite" Then
                        strTitle = m_User.Customer.CustomerName & " - " & "Startseite"
                        Me.Page.Title = strTitle

                    ElseIf Session("AppID") Is Nothing Then
                        Me.Page.Title = m_User.Customer.CustomerName

                    Else
                        strTitle = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
                        Me.Page.Title = m_User.Customer.CustomerName & " - " & strTitle

                    End If

                End If

            End If
            '§§§ JVE 18.09.2006: Rechtes Logo auch parametrisieren.
            'If (m_User.Customer.LogoPath2 Is Nothing) OrElse (m_User.Customer.LogoPath2 = String.Empty) Then
            '    strLogoPath2 = ""
            '    imgDADLogo.Src = ""
            'Else
            '    strLogoPath2 = m_User.Customer.LogoPath2
            '    imgDADLogo.Src = strLogoPath2
            'End If

            '------------------------------------------------------

            If m_User.GroupID > 0 Then
                strLogoPath = m_User.Organization.LogoPath
                strDocuPath = m_User.Groups.ItemByID(m_User.GroupID).DocuPath

                Dim cn As SqlClient.SqlConnection
                cn = New SqlClient.SqlConnection(m_User.App.Connectionstring)
                Dim _EmployeeAssigned As New CKG.Base.Kernel.Admin.EmployeeList(m_User.GroupID, m_User.Customer.AccountingArea, cn)
                _EmployeeAssigned.GetAssigned()
                If _EmployeeAssigned.DefaultView.Count > 0 Then
                    _EmployeeAssigned.DefaultView.Sort = "EmployeeName"
                    tdResponsible.Visible = True
                Else
                    tdResponsible.Visible = False
                End If
            End If
            If strDocuPath = String.Empty Then
                strDocuPath = m_User.Customer.DocuPath
            End If
            If Not strDocuPath = String.Empty Then
                tdHandbuch.Visible = True
                lnkHandbuch.NavigateUrl = strDocuPath
            End If

            If strLogoPath = String.Empty Then
                strLogoPath = m_User.Customer.CustomerStyle.LogoPath
            End If
            'If Not strLogoPath = String.Empty Then
            '    .imgCustomerLogo.Visible = True
            '    .imgCustomerLogo.ImageUrl = strLogoPath
            'End If

            '.imgDADLogo.Alt &= vbCrLf & m_User.UserID


            'litSetBackground.Visible = False
            'If m_User.IsTestUser Then
            '    litSetBackground.Visible = True
            '    litSetBackground.Text = "		<script language=""JavaScript"">" & vbCrLf & _
            '                             "<!-- //" & vbCrLf & _
            '                             " window.document.getElementsByTagName(""body"")[0].background = ""/Portal/Images/TestUser.JPG"";" & vbCrLf & _
            '                             "//-->" & vbCrLf & _
            '                             "		</script>"
            'Else
            '    If ConfigurationManager.AppSettings("ShowProductiveBackground") = "ON" Then
            '        litSetBackground.Visible = True
            '        litSetBackground.Text = "		<script language=""JavaScript"">" & vbCrLf & _
            '                                 "<!-- //" & vbCrLf & _
            '                                 " window.document.getElementsByTagName(""body"")[0].background = ""/Portal/Images/ProdUser.JPG"";" & vbCrLf & _
            '                                 "//-->" & vbCrLf & _
            '                                 "		</script>"
            '    End If
            'End If

            End If

    End Sub
    Public Sub HideLinks()

        tdChangePasword.Visible = False
        tdHandbuch.Visible = False
        'Me.tdLogout.Visible = False
        tdHauptmenue.Visible = False

    End Sub
    Public Sub LockLinks()

        lnkChangePassword.Enabled = False
        lnkHandbuch.Enabled = False
        lnkHauptmenue.Enabled = False
        lnkImpressum.Enabled = False
        lnkContact.Enabled = False
        lnkResponsible.Enabled = False

    End Sub

End Class