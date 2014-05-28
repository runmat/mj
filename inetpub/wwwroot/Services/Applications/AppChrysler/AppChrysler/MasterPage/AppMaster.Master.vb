Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common

Partial Public Class AppMaster
    Inherits System.Web.UI.MasterPage
    Private m_User As Security.User
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Dim strLogoPath As String = ""
        Dim strLogoPath2 As String = ""
        Dim strDocuPath As String = ""
        Dim strTitle As String
        Dim bc As HttpBrowserCapabilities
        bc = Request.Browser

        'Aktuelles Jahr ins Copyright setzen.
        lblCopyright.Text = lblCopyright.Text.Replace("year", DateTime.Now.Year.ToString)

        lnkLogout.NavigateUrl = "/Services/(S(" & Session.SessionID & "))/Start/Logout.aspx"
        m_User = CType(Session("objUser"), Security.User)
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
            lblBenutzer.Text = "Benutzer: "
            lblUserName.Text = m_User.UserName
            lnkLogout.Visible = True
            lnkChangePassword.Visible = True

            Dim strCSSLink As String = ""
            With bc
                If .Type = "IE6" Then

                    strCSSLink = "<link href=""/Services/Styles/defaultIE6.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"
                    Select Case m_User.CustomerName
                        Case "Volksfürsorge"
                            strCSSLink &= "<link href=""Services/Customize/Wuerttenbergische/wuerttembIE6.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"
                        Case "AKF Bank Retail"
                            strCSSLink &= "<link href=""/Services/Customize/Akf_Retail/AKFRetailIE6.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"
                        Case "Arval"
                            strCSSLink &= "<link href=""/Services/Customize/Arval/ArvalIE6.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"
                        Case "Porsche"
                            strCSSLink &= "<link href=""/Services/Customize/porsche/porscheE6.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"
                        Case "Volvo Auto Bank GmbH"
                            strCSSLink &= "<link href=""/Services/Customize/Volvo/volvoIE6.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"
                        Case "Chrysler"
                            strCSSLink &= "<link href=""/Services/Customize/Chrysler/ChryslerIE6.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"

                        Case Else

                    End Select

                Else
                    Dim strtemp As String = Server.MapPath("~/Services/Styles/default.css")
                    strCSSLink = "<link href=""/Services/Styles/default.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"
                    Select Case m_User.CustomerName
                        Case "Volksfürsorge"
                            strCSSLink &= "<link href=""/Services/Customize/Wuerttenbergische/wuerttenb.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"
                        Case "AKF Bank Retail"
                            strCSSLink &= "<link href=""/Services/Customize/Akf_Retail/AKFRetail.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"
                        Case "Arval"
                            strCSSLink &= "<link href=""/Services/Customize/Arval/Arval.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"
                        Case "Porsche"
                            strCSSLink &= "<link href=""/Services/Customize/porsche/porsche.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"
                        Case Else
                            strCSSLink &= "<link href=""" & m_User.Customer.CustomerStyle.CssPath & """ media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"
                    End Select
                End If
            End With
            Me.Head1.Controls.Add(New LiteralControl(strCSSLink))

            If Me.Page.User.Identity.IsAuthenticated Then
                Select Case Me.Page.Title
                    Case "Startseite"
                        tdHauptmenue.Attributes.Add("class", "active")
                        tdChangePasword.Attributes.Add("class", "")
                        'tdLogout.Attributes.Add("class", "")
                        tdContact.Attributes.Add("class", "")
                        tdImpressum.Attributes.Add("class", "")
                        tdResponsible.Attributes.Add("class", "")

                    Case "Passwort ändern"
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
                strLogoPath = m_User.Organization.LogoPath
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

            If strLogoPath = String.Empty Then
                strLogoPath = m_User.Customer.CustomerStyle.LogoPath
            End If

        End If
    End Sub
End Class