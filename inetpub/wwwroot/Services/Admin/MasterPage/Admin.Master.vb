Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Services

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
        Dim strDocuPath As String = ""
        Dim strTitle As String
        Dim bc As HttpBrowserCapabilities
        bc = Request.Browser

        m_User = Session("objUser")
        If m_User Is Nothing Then

            If Not IsPostBack Then
                tdHandbuch.Visible = False
                lnkHauptmenue.Text = "Anmeldung"
                tdChangePasword.Visible = False
                'tdLogout.Visible = False
                lblUserName.Visible = False
                imgLogo.Visible = False
                PlaceHeader.Visible = True
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
            lblUserName.Text = "Benutzer: " & m_User.UserName
            lnkLogout.Visible = True
            lnkChangePassword.Visible = True

            If m_User.Customer.ShowsTeamViewer = True Then
                sidebarRight.Visible = True
            Else
                sidebarRight.Visible = False
            End If

            Dim strCSSLink As String = ""
            Dim strCustomerCss As String = m_User.Customer.CustomerStyle.CssPath
            With bc
                If .Type = "IE6" Then

                    strCSSLink = "<link href=""/Services/Styles/defaultIE6.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"
                    Select Case m_User.CustomerName
                        Case "Firma 1"
                            strCSSLink &= ("<link href=""../Customize/Admin/adminIE6.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />")
                            imgLogo.ImageUrl = "../../Images/kroschke.jpg"
                        Case "DAD"
                            strCSSLink &= ("<link href=""/Services/Styles/dadIE6.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />")
                        Case Else
                            If strCustomerCss.Contains(".css") Then
                                Dim strCSS() As String = strCustomerCss.Split(".css")
                                If strCSS.Length = 2 Then
                                    strCSSLink &= "<link href=""" & strCSS(0) & "IE6.css" & """ media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"
                                End If
                            End If
                    End Select
                Else
                    strCSSLink = "<link href=""/Services/Styles/default.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"

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
                            If strCustomerCss.Contains(".css") Then
                                strCSSLink &= "<link href=""" & strCustomerCss & """ media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"
                            End If
                    End Select
                End If
            End With
            Me.Head1.Controls.Add(New LiteralControl(strCSSLink))

            If HttpContext.Current.Request.UserAgent IsNot Nothing AndAlso (HttpContext.Current.Request.UserAgent.ToLower().Contains("msie 10") OrElse HttpContext.Current.Request.UserAgent.ToLower().Contains("rv:11.0")) Then
                Me.Head1.Controls.AddAt(0, New LiteralControl("<META content=""IE=9,chrome=1"" http-equiv=""X-UA-Compatible"">"))
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

            If m_User.GroupID > 0 Then
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

        End If

        Dim ServicesHeader As Services = New Services()
        ServicesHeader.FillHeaderMitKundenlogo(imgLogo, m_User, JavaScriptBlock, imgLogoDIV)


        lblCopyright.Text = lblCopyright.Text.Replace("year", DateTime.Now.Year.ToString)
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