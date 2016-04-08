Imports CKG.Base.Kernel

Partial Public Class Design
    Inherits MasterPage
    Private m_User As Security.User
    Private m_strTitleText As String

    Public Property TitleText() As String
        Get
            Return m_strTitleText
        End Get
        Set(ByVal value As String)
            m_strTitleText = Value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

    End Sub

    Public Sub HideLinks()
        With Me
            .tdChangePasword.Visible = False
            .tdHandbuch.Visible = False
            .tdHauptmenue.Visible = False
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

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        Dim strDocuPath As String = ""
        Dim strTitle As String
        Dim strCSSLink As String = ""
        Dim bc As HttpBrowserCapabilities
        bc = Request.Browser

        lblCopyright.Text = lblCopyright.Text.Replace("year", DateTime.Now.Year.ToString())
        lblHeaderHint.Text = ConfigurationManager.AppSettings("PortalHeaderHint")
        m_User = Session("objUser")
        If m_User Is Nothing Then

            If Not IsPostBack Then
                tdHandbuch.Visible = False
                lnkHauptmenue.Text = "Anmeldung"
                tdChangePasword.Visible = False
                tdResponsible.Visible = False
                lnkLogout.Visible = False
                lblUserName.Visible = False
                lblBenutzer.Visible = False
                PlaceHeader.Visible = True
                tdImpressum.Visible = True
            End If

            With bc
                If .Type = "IE6" Then
                    strCSSLink &= "<link href=""/PortalZLD/Styles/defaultIE6.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"
                    strCSSLink &= "<link href=""/PortalZLD/Customize/AppKroschke/KroschkeIE6.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"
                Else
                    strCSSLink &= "<link href=""/PortalZLD/Styles/default.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"
                    strCSSLink &= "<link href=""/PortalZLD/Customize/AppKroschke/Kroschke.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"
                End If
            End With
            Head1.Controls.Add(New LiteralControl(strCSSLink))

            If HttpContext.Current.Request.UserAgent IsNot Nothing AndAlso HttpContext.Current.Request.UserAgent.ToLower().Contains("msie 10") Then
                Me.Head1.Controls.Add(New LiteralControl("<META content=""IE=9,chrome=1"" http-equiv=""X-UA-Compatible"">"))
            End If
        Else

            With bc
                If .Type = "IE6" Then
                    strCSSLink &= "<link href=""/PortalZLD/Styles/defaultIE6.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"
                    strCSSLink &= "<link href=""/PortalZLD/Customize/AppKroschke/KroschkeIE6.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"

                Else
                    strCSSLink &= "<link href=""/PortalZLD/Styles/default.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"
                    strCSSLink &= "<link href=""/PortalZLD/Customize/AppKroschke/Kroschke.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"

                End If
            End With
            ' 
            Head1.Controls.Add(New LiteralControl(strCSSLink))

            If HttpContext.Current.Request.UserAgent IsNot Nothing AndAlso HttpContext.Current.Request.UserAgent.ToLower().Contains("msie 10") Then
                Me.Head1.Controls.Add(New LiteralControl("<META content=""IE=9,chrome=1"" http-equiv=""X-UA-Compatible"">"))
            End If

            PlaceHeader.Visible = False
            lnkHauptmenue.NavigateUrl = "/PortalZLD/(S(" & Session.SessionID & "))/Start/Selection.aspx"
            lnkChangePassword.NavigateUrl = "/PortalZLD/(S(" & Session.SessionID & "))/Start/ChangePassword.aspx"
            lnkLogout.NavigateUrl = "/PortalZLD/(S(" & Session.SessionID & "))/Start/Logout.aspx"
            lnkContact.NavigateUrl = "/PortalZLD/(S(" & Session.SessionID & "))/Info/ContactPage.aspx"
            lnkResponsible.NavigateUrl = "/PortalZLD/(S(" & Session.SessionID & "))/Info/ResponsiblePage.aspx"
            lnkImpressum.NavigateUrl = "/PortalZLD/(S(" & Session.SessionID & "))/Info/Impressum.aspx"

            lnkChangePassword.Visible = True
            lblBenutzer.Text = "Benutzer: "
            If m_User.IsLeiterZLD Then
                lblUserName.Text = m_User.UserName & " (Kst.: " & m_User.Kostenstelle & ")"
            Else
                lblUserName.Text = m_User.UserName
            End If
            lnkLogout.Visible = True
            lnkChangePassword.Visible = True

            If Page.User.Identity.IsAuthenticated Then
                If Page.Title = "Startseite" Then
                    strTitle = m_User.Customer.CustomerName & " - " & "Startseite"
                    Page.Title = strTitle

                ElseIf Session("AppID") Is Nothing OrElse Session("AppID").ToString = "0" Then
                    Page.Title = m_User.Customer.CustomerName

                Else
                    strTitle = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
                    Page.Title = m_User.Customer.CustomerName & " - " & strTitle

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
        Select Case Page.Title
            Case "Startseite"
                tdHauptmenue.Attributes.Add("class", "active")
                tdChangePasword.Attributes.Add("class", "")
                tdContact.Attributes.Add("class", "")
                tdImpressum.Attributes.Add("class", "")
                tdResponsible.Attributes.Add("class", "")
            Case "Anmeldung"
                tdHauptmenue.Attributes.Add("class", "active")
                tdChangePasword.Attributes.Add("class", "")
                tdContact.Attributes.Add("class", "")
                tdImpressum.Attributes.Add("class", "")
                tdResponsible.Attributes.Add("class", "")
            Case "Passwort ändern"
                tdHauptmenue.Attributes.Add("class", "")
                tdChangePasword.Attributes.Add("class", "active")
                tdContact.Attributes.Add("class", "")
                tdImpressum.Attributes.Add("class", "")
                tdResponsible.Attributes.Add("class", "")
            Case "Kontaktseite"
                tdHauptmenue.Attributes.Add("class", "")
                tdChangePasword.Attributes.Add("class", "")
                tdContact.Attributes.Add("class", "active")
                tdImpressum.Attributes.Add("class", "")
                tdResponsible.Attributes.Add("class", "")
            Case "Impressum"
                tdHauptmenue.Attributes.Add("class", "")
                tdChangePasword.Attributes.Add("class", "")
                tdContact.Attributes.Add("class", "")
                tdImpressum.Attributes.Add("class", "active")
                tdResponsible.Attributes.Add("class", "")
            Case "Ansprechpartner"
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