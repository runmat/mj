Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common

Partial Public Class Services
    Inherits System.Web.UI.MasterPage
    Private m_User As Security.User

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Dim strLogoPath As String = ""
        Dim strLogoPath2 As String = ""
        Dim strDocuPath As String = ""
        Dim strTitle As String
        Dim bc As HttpBrowserCapabilities
        bc = Request.Browser

        'Aktuelles Jahr ins Copyright setzen.
        lblCopyright.Text = lblCopyright.Text.Replace("year", DateTime.Now.Year.ToString)

        m_User = Session("objUser")
        lnkLogout.NavigateUrl = "/Services/(S(" & Session.SessionID & "))/Start/Logout.aspx"
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

                        strCSSLink = "<link href=""/Services/Styles/defaultIE6.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"
                        Select Case m_User.CustomerName
                            Case "Firma 1"
                                strCSSLink &= ("<link href=""../Customize/Admin/adminIE6.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />")
                                imgLogo.ImageUrl = "../../Images/kroschke.jpg"
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
                        Dim strtemp As String = Server.MapPath("~/Services/Styles/default.css")
                        strCSSLink = "<link href=""/Services/Styles/default.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"

                        If Not m_User Is Nothing Then

                            Select Case m_User.CustomerName
                                Case "Volksfürsorge"
                                    strCSSLink &= "<link href=""/Services/Customize/Wuerttenbergische/wuerttenb.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"
                                Case "AKF Bank Retail"
                                    strCSSLink &= "<link href=""/Services//Akf_Retail/AKFRetail.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"
                                Case "Arval"
                                    strCSSLink &= "<link href=""/Services/Customize/Arval/Arval.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"
                                Case "Porsche"
                                    strCSSLink &= "<link href=""/Services/Customize/porsche/porsche.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"
                                Case "Firma 1"
                                    strCSSLink &= ("<link href=""../Customize/Admin/admin.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />")
                                    imgLogo.ImageUrl = "../../Images/kroschke.jpg"
                                Case "DAD"
                                    strCSSLink &= ("<link href=""/Services/Styles/dad.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />")
                                Case Else
                                    strCSSLink &= "<link href=""" & m_User.Customer.CustomerStyle.CssPath & """ media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"
                                    If m_User.Customer.AccountingArea = 1010 Then
                                        imgLogo.ImageUrl = m_User.Customer.LogoPath2
                                    End If
                            End Select
                        Else
                            strCSSLink &= "<link href=""../Customize/AppKroschke/Kroschke.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"
                        End If
                    End If
                End With
                Me.Head1.Controls.Add(New LiteralControl(strCSSLink))

                If HttpContext.Current.Request.UserAgent IsNot Nothing AndAlso HttpContext.Current.Request.UserAgent.ToLower().Contains("msie 10") Then
                    Me.Head1.Controls.AddAt(0, New LiteralControl("<META content=""IE=9,chrome=1"" http-equiv=""X-UA-Compatible"">"))
                End If
            End If
        Else
            imgLogo.Visible = True
            PlaceHeader.Visible = False
            lnkHauptmenue.NavigateUrl = "/Services/(S(" & Session.SessionID & "))/Start/Selection.aspx"
            lnkChangePassword.NavigateUrl = "/Services/(S(" & Session.SessionID & "))/Start/ChangePassword.aspx"
            lnkLogout.NavigateUrl = "/Services/(S(" & Session.SessionID & "))/Start/Logout.aspx"
            lnkContact.NavigateUrl = "/Services/(S(" & Session.SessionID & "))/Info/ContactPage.aspx"
            lnkResponsible.NavigateUrl = "/Services/(S(" & Session.SessionID & "))/Info/ResponsiblePage.aspx"
            lnkImpressum.NavigateUrl = "/Services/(S(" & Session.SessionID & "))/Info/Impressum.aspx"

            PlaceHeader.Visible = False

            lnkChangePassword.Visible = True
            lblBenutzer.Text = "Benutzer: "
            lblUserName.Text = m_User.UserName
            lnkLogout.Visible = True
            lnkChangePassword.Visible = True

            If m_User.Customer.ShowsTeamViewer Then
                sidebarRight.Visible = True
            Else
                sidebarRight.Visible = False
            End If

            Dim strCSSLink As String = ""

            With bc
                If .Type = "IE6" Then
                    strCSSLink = "<link href=""/Services/Styles/defaultIE6.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"

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
                    Dim strtemp As String = Server.MapPath("~/Services/Styles/default.css")
                    strCSSLink = "<link href=""/Services/Styles/default.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"
                    Select Case m_User.CustomerName
                        Case "Firma 1"
                            strCSSLink &= ("<link href=""/Services/Customize/Admin/admin.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />")
                            imgLogo.ImageUrl = "/Services/Images/kroschke.jpg"
                        Case "DAD"
                            strCSSLink &= ("<link href=""/Services/Styles/dad.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />")

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
                Me.Head1.Controls.AddAt(0, New LiteralControl("<META content=""IE=9,chrome=1"" http-equiv=""X-UA-Compatible"">"))
            End If

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
                    tdHandbuch.Visible = True
                    lnkHandbuch.NavigateUrl = strDocuPath
                End If

                If strLogoPath = String.Empty Then
                    strLogoPath = m_User.Customer.CustomerStyle.LogoPath
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

        FillHeaderMitKundenlogo(imgLogo, m_User, JavaScriptBlock, imgLogoDIV)

    End Sub

    Public Sub FillHeaderMitKundenlogo(ByRef f_imgLogo As Image, ByVal f_User As Security.User, ByRef f_JavaScriptBlock As Label, ByRef f_imgLogoDIV As HtmlGenericControl)
        Try

            If f_User.Customer.LogoPath <> "" Then
                f_imgLogoDIV.Style.Add("background-image", "url('" & f_User.Customer.LogoPath & "')")
                f_imgLogoDIV.Style.Add("background-repeat", "no-repeat")
                f_imgLogoDIV.Style.Add("background-position", "20px")
            End If

            If f_User.Customer.LogoPath2 <> "" And (InStr(f_User.Customer.LogoPath2, ".jpg") > 0 Or InStr(f_User.Customer.LogoPath2, ".jepg") > 0 Or InStr(f_User.Customer.LogoPath2, ".png") > 0) Then
                f_imgLogo.ImageUrl = f_User.Customer.LogoPath2
            End If

        Catch
        End Try
        Try
            If f_User.Customer.HeaderBackgroundPath <> "" Then
                f_JavaScriptBlock.Text = "<script type=""text/javascript"">$(""div#header"").css(""background-image"",""url('" & f_User.Customer.HeaderBackgroundPath.Replace("~", "/Services").Replace("../", "/Services/") & "')"");</script>"
                JavaScriptBlock.Text = "<script type=""text/javascript"">$(""div#header"").css(""background-image"",""url('" & f_User.Customer.HeaderBackgroundPath.Replace("~", "/Services") & "')"");</script>"
            End If
        Catch
        End Try
    End Sub



End Class