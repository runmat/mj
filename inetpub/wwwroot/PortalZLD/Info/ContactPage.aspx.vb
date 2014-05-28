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
            lblSuccess.Visible = False

        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Protected Sub cmdSend_Click(sender As Object, e As EventArgs) Handles cmdSend.Click
        Dim bError As Boolean = False
        lblError.Text = ""
        txtName.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf")
        txtBetreff.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf")
        txtTel.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf")
        txtDate.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf")


        If txtName.Text.Length = 0 Then
            txtName.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B")
            bError = True
        End If
        If txtBetreff.Text.Length = 0 Then
            txtBetreff.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B")
            bError = True
        End If
        If txtTel.Text.Length = 0 Then
            txtTel.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B")
            bError = True
        End If
        If txtDate.Text.Length = 0 Then
            txtDate.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B")
            bError = True
        End If

        If bError = False Then
            Dim str As String = ""
            str &= vbCrLf
            str &= "Anrede         : " & ddlAnrede.SelectedItem.Text & vbCrLf
            str &= "Name           : " & txtName.Text & vbCrLf
            str &= "Telefon        : " & txtTel.Text & vbCrLf
            str &= "Betreff        : " & txtBetreff.Text & vbCrLf
            str &= "Rückruftermin  : " & txtDate.Text & " " & ddlTime.SelectedItem.Text & vbCrLf
            If SendMail(str) Then
                ddlAnrede.SelectedIndex = -1
                txtName.Text = String.Empty
                txtTel.Text = String.Empty
                txtBetreff.Text = String.Empty
                txtDate.Text = String.Empty
                ddlTime.SelectedIndex = -1
                lblSuccess.Visible = True
            End If
        Else
            lblError.Text = "Füllen Sie bitte alle markierten Felder!"
        End If
    End Sub
    Private Function SendMail(ByVal message As String) As Boolean
        Dim bSend = True

        Try
            Dim Mail As System.Net.Mail.MailMessage
            Dim smtpMailSender As New System.Net.Mail.MailAddress(ConfigurationManager.AppSettings("SmtpMailSender"))
            Dim smtpMailServer As String = ""
            Dim MailAdresses As String = ""


            'Mail = New System.Net.Mail.MailMessage(smtpMailSender, "Web-Administrator@dad.de", "Helpdeskanfrage der Login-Seite", message)
            Mail = New System.Net.Mail.MailMessage()
            Mail.Body = message
            Mail.From = smtpMailSender
            If m_User.IsTestUser = False Then
                Mail.To.Add("support@kroschke.de")
            Else
                'Mail.To.Add("oliver.rudolph@kroschke.de")
                Mail.To.Add("nadia.grzanna@kroschke.de")
            End If


            Mail.Subject = "Benutzer bittet um Rückruf (ZLD-Portal)"

            Mail.IsBodyHtml = False
            smtpMailServer = ConfigurationManager.AppSettings("SmtpMailServer")
            Dim client As New System.Net.Mail.SmtpClient(smtpMailServer)
            client.Send(Mail)
        Catch ex As Exception
            bSend = False
            lblError.Text = "Fehler beim Versenden der E-Mail."
        End Try

        Return bSend
    End Function
End Class