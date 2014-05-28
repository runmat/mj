Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common

Partial Public Class Genarali
    Inherits System.Web.UI.MasterPage
    Private m_User As Security.User
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    'Public Sub HideLinks()
    '    With Me
    '        Me.tdChangePasword.Visible = False
    '        Me.tdHandbuch.Visible = False
    '        'Me.tdLogout.Visible = False
    '        Me.tdHauptmenue.Visible = False
    '    End With
    'End Sub
    'Public Sub LockLinks()
    '    With Me
    '        .lnkChangePassword.Enabled = False
    '        .lnkHandbuch.Enabled = False
    '        .lnkHauptmenue.Enabled = False
    '        .lnkImpressum.Enabled = False
    '        .lnkContact.Enabled = False
    '        .lnkResponsible.Enabled = False
    '    End With
    'End Sub

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

            Dim strCSSLink As String = ""
            With bc
                If .Type = "IE6" Then

                    strCSSLink = "<link href=""../../../Styles/defaultIE6.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"
                    strCSSLink &= "<link href=""../../../Customize/Wuerttenbergische/wuerttenbIE6.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"
                Else

                    strCSSLink = "<link href=""../../../Styles/default.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"
                    strCSSLink &= "<link href=""../../../Customize/Wuerttenbergische/wuerttenb.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"
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
End Class