Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Business.HelpProcedures

Partial Public Class _Error
    Inherits System.Web.UI.Page

#Region " Membervariables "
    Private m_User As Security.User
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Session("objUser") Is Nothing Then
                m_User = CType(Session("objUser"), Security.User)
            End If
            m_User = GetUser(Me)
            lblHead.Text = "Fehler"

            Dim strID As String = ""

            If Request.QueryString("ID") IsNot Nothing Then
                strID = CStr(Request.QueryString("ID"))
            End If

            Select Case strID
                Case "404"
                    lblErrorMessage.Text = "Die angeforderte Seite wurde nicht gefunden! (HTTP-Error 404)"
                Case "403"
                    lblErrorMessage.Text = "Sie haben keinen Zugriff auf die angeforderte Seite oder das Verzeichnis! (HTTP-Error 403)"
                Case "500"
                    lblErrorMessage.Text = "Ein interner Server-Fehler ist aufgetreten! (HTTP-Error 500)"
                Case Else
                    lblErrorMessage.Text = "Ein Fehler ist aufgetreten!" & "<br /><br />"
                    Dim ex As Exception = Server.GetLastError
                    Dim exIn As Exception = ex
                    While Not exIn Is Nothing
                        lblErrorMessage.Text = exIn.Message & "<br /><br />"
                        exIn = exIn.InnerException
                    End While
                    Server.ClearError()
            End Select

            If Not m_User Is Nothing AndAlso Not m_User.Customer Is Nothing AndAlso Not m_User.Groups.Count = 0 Then
                Dim _contact As Security.Contact = m_User.Customer.CustomerContact.CombineWith(m_User.Organization.OrganizationContact)
                pnlLinks.Controls.Clear()

                With _contact
                    lblCName.Text = .Name
                    lblCAddress.Text = TranslateHTML(.Address, TranslationDirection.ReadHTML)
                End With

                With pnlLinks.Controls
                    .Add(_contact.GetMailHyperLink)
                    .Add(New LiteralControl("<br />"))
                    .Add(_contact.GetWebHyperLink)
                End With
            End If

        Catch ex As Exception

        End Try
    End Sub

End Class