Imports CKG.Base.Kernel

Partial Public Class ContactPage
    Inherits System.Web.UI.Page

    Private m_User As Security.User
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Session("objUser") Is Nothing Then
                m_User = CType(Session("objUser"), Security.User)
            End If
            'InitHeader.InitUser(m_User)
            lblHead.Text = "Kontaktseite"
            Page.Title = "Kontaktseite"
            If Not m_User Is Nothing AndAlso Not m_User.Customer Is Nothing AndAlso Not m_User.Groups.Count = 0 Then
                Dim _contact As Security.Contact = m_User.Customer.CustomerContact.CombineWith(m_User.Organization.OrganizationContact)
                pnlLinks.Controls.Clear()

                With _contact
                    lblCName.Text = .Name
                    lblCAddress.Text = .Address
                    lblCAddress.Text = Replace(lblCAddress.Text, "}", ">")
                    lblCAddress.Text = Replace(lblCAddress.Text, "{", "<")
                    lblOrt.Visible = False
                End With

                With pnlLinks.Controls
                    .Add(_contact.GetMailHyperLink)
                    .Add(New LiteralControl("<br />"))
                    .Add(_contact.GetWebHyperLink)
                End With

                pnl2ndAddress.Controls.Clear()
            Else
                pnl2ndAddress.Visible = True
            End If

        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

End Class