Imports CKG.Base.Kernel

Partial Public Class Services
    Inherits MasterPage
    Private m_User As Security.User

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        Dim strDocuPath As String = ""
        Dim strTitle As String
        Dim bc As HttpBrowserCapabilities
        bc = Request.Browser
        lblCopyright.Text = lblCopyright.Text.Replace("year", DateTime.Now.Year.ToString())
        lblHeaderHint.Text = ConfigurationManager.AppSettings("PortalHeaderHint")
        m_User = Session("objUser")
        lnkLogout.NavigateUrl = "/PortalZLD/(S(" & Session.SessionID & "))/Start/Logout.aspx"
        If m_User Is Nothing Then
            If Not IsPostBack Then
                tdHandbuch.Visible = False
                tdResponsible.Visible = False
                lnkHauptmenue.Text = "Anmeldung"
                tdChangePasword.Visible = False
                lblUserName.Visible = False
                imgLogo.Visible = False
                PlaceHeader.Visible = True
                Menue1.Visible = False
                Dim strCSSLink As String = ""
                With bc
                    If .Type = "IE6" Then

                        strCSSLink = "<link href=""/PortalZLD/Styles/defaultIE6.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"
                        Select Case m_User.CustomerName
                            Case "Firma 1"
                                strCSSLink &= ("<link href=""/PortalZLD/Customize/Admin/adminIE6.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />")
                                imgLogo.ImageUrl = "/PortalZLD/Images/kroschke_inside_logo.png"
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
                                If m_User.Customer.AccountingArea = 1010 Then
                                    imgLogo.ImageUrl = m_User.Customer.LogoPath2
                                End If
                        End Select
                    Else
                        strCSSLink = "<link href=""/PortalZLD/Styles/default.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"

                        If Not m_User Is Nothing Then

                            Select Case m_User.CustomerName
                                Case "Firma 1"
                                    strCSSLink &= ("<link href=""/PortalZLD/Customize/Admin/admin.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />")
                                    imgLogo.ImageUrl = "/PortalZLD/Images/kroschke_inside_logo.png"
                                Case Else
                                    strCSSLink &= "<link href=""" & m_User.Customer.CustomerStyle.CssPath & """ media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"
                                    If m_User.Customer.AccountingArea = 1010 Then
                                        imgLogo.ImageUrl = m_User.Customer.LogoPath2
                                    End If
                            End Select
                        Else
                            strCSSLink &= "<link href=""/PortalZLD/Customize/AppKroschke/Kroschke.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"
                        End If
                    End If
                End With
                Me.Head1.Controls.Add(New LiteralControl(strCSSLink))

                If HttpContext.Current.Request.UserAgent IsNot Nothing AndAlso HttpContext.Current.Request.UserAgent.ToLower().Contains("msie 10") Then
                    Me.Head1.Controls.Add(New LiteralControl("<META content=""IE=9,chrome=1"" http-equiv=""X-UA-Compatible"">"))
                End If

            End If
        Else
            imgLogo.Visible = True
            PlaceHeader.Visible = False
            lnkHauptmenue.NavigateUrl = "/PortalZLD/(S(" & Session.SessionID & "))/Start/Selection.aspx"
            lnkChangePassword.NavigateUrl = "/PortalZLD/(S(" & Session.SessionID & "))/Start/ChangePassword.aspx"
            lnkLogout.NavigateUrl = "/PortalZLD/(S(" & Session.SessionID & "))/Start/Logout.aspx"
            lnkContact.NavigateUrl = "/PortalZLD/(S(" & Session.SessionID & "))/Info/ContactPage.aspx"
            lnkResponsible.NavigateUrl = "/PortalZLD/(S(" & Session.SessionID & "))/Info/ResponsiblePage.aspx"
            lnkImpressum.NavigateUrl = "/PortalZLD/(S(" & Session.SessionID & "))/Info/Impressum.aspx"

            PlaceHeader.Visible = False

            lnkChangePassword.Visible = True
            lblBenutzer.Text = "Benutzer: "
            If m_User.IsLeiterZLD Then
                lblUserName.Text = m_User.UserName & " (Kst.: " & m_User.Kostenstelle & ")"
            Else
                lblUserName.Text = m_User.UserName
            End If
            lnkLogout.Visible = True
            lnkChangePassword.Visible = True

            Dim strCSSLink As String = ""

            With bc
                If .Type = "IE6" Then
                    strCSSLink = "<link href=""/PortalZLD/Styles/defaultIE6.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"

                    Dim strCSS() As String
                    Dim strCSSPath As String = m_User.Customer.CustomerStyle.CssPath
                    If strCSSPath.Contains(".css") Then
                        strCSS = strCSSPath.Split(".css")
                        If strCSS.Length = 2 Then
                            strCSSPath = strCSS(0) & "IE6.css"
                            strCSSLink &= "<link href=""" & strCSSPath & """ media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"
                        End If
                    End If
                    If m_User.Customer.AccountingArea = 1010 Then
                        imgLogo.ImageUrl = m_User.Customer.LogoPath2
                    End If
                Else
                    strCSSLink = "<link href=""/PortalZLD/Styles/default.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"
                    Select Case m_User.CustomerName
                        Case "Firma 1"
                            strCSSLink &= ("<link href=""/PortalZLD/Customize/Admin/admin.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />")
                            imgLogo.ImageUrl = "/PortalZLD/Images/kroschke_inside_logo.png"
                        Case "DAD"
                            strCSSLink &= ("<link href=""/PortalZLD/Styles/dad.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />")
                        Case Else
                            strCSSLink &= "<link href=""" & m_User.Customer.CustomerStyle.CssPath & """ media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"

                            If m_User.Customer.AccountingArea = 1010 Then
                                imgLogo.ImageUrl = m_User.Customer.LogoPath2
                            End If

                    End Select
                End If
            End With
            Me.Head1.Controls.Add(New LiteralControl(strCSSLink))

            If HttpContext.Current.Request.UserAgent IsNot Nothing AndAlso HttpContext.Current.Request.UserAgent.ToLower().Contains("msie 10") Then
                Me.Head1.Controls.Add(New LiteralControl("<META content=""IE=9,chrome=1"" http-equiv=""X-UA-Compatible"">"))
            End If

            If Me.Page.User.Identity.IsAuthenticated Then
                If Me.Page.Title = "Startseite" Then
                    strTitle = m_User.Customer.CustomerName & " - " & "Startseite"
                    Me.Page.Title = strTitle

                ElseIf Session("AppID") Is Nothing OrElse Session("AppID").ToString = "0" Then
                    Me.Page.Title = m_User.Customer.CustomerName

                Else
                    strTitle = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
                    Me.Page.Title = m_User.Customer.CustomerName & " - " & strTitle

                End If

            End If

            If m_User.GroupID > 0 Then
                strDocuPath = m_User.Groups.ItemByID(m_User.GroupID).DocuPath

                Dim cn As SqlClient.SqlConnection
                cn = New SqlClient.SqlConnection(m_User.App.Connectionstring)
                Dim _EmployeeAssigned As New Admin.EmployeeList(m_User.GroupID, m_User.Customer.AccountingArea, cn)
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

        End If
        Select Case Me.Request.FilePath
            Case "/Services/Start/Selection.aspx"
                tdHauptmenue.Attributes.Add("class", "active")
                tdChangePasword.Attributes.Add("class", "")
                tdContact.Attributes.Add("class", "")
                tdImpressum.Attributes.Add("class", "")
                tdResponsible.Attributes.Add("class", "")
            Case "/Services/Start/ChangePassword.aspx"
                tdHauptmenue.Attributes.Add("class", "")
                tdChangePasword.Attributes.Add("class", "active")
                tdContact.Attributes.Add("class", "")
                tdImpressum.Attributes.Add("class", "")
                tdResponsible.Attributes.Add("class", "")
            Case "/Services/Info/ContactPage.aspx"
                tdHauptmenue.Attributes.Add("class", "")
                tdChangePasword.Attributes.Add("class", "")
                tdContact.Attributes.Add("class", "active")
                tdImpressum.Attributes.Add("class", "")
                tdResponsible.Attributes.Add("class", "")
            Case "/Services/Info/Impressum.aspx"
                tdHauptmenue.Attributes.Add("class", "")
                tdChangePasword.Attributes.Add("class", "")
                tdContact.Attributes.Add("class", "")
                tdImpressum.Attributes.Add("class", "active")
                tdResponsible.Attributes.Add("class", "")
            Case "/Services/Info/ResponsiblePage.aspx"
                tdHauptmenue.Attributes.Add("class", "")
                tdChangePasword.Attributes.Add("class", "")
                tdContact.Attributes.Add("class", "")
                tdImpressum.Attributes.Add("class", "")
                tdResponsible.Attributes.Add("class", "active")
            Case Else
                tdHauptmenue.Attributes.Add("class", "")
                tdChangePasword.Attributes.Add("class", "")
                tdContact.Attributes.Add("class", "")
                tdImpressum.Attributes.Add("class", "")
                tdResponsible.Attributes.Add("class", "")
        End Select
    End Sub

End Class