Option Strict On
Option Explicit On

Partial Public Class KBSMenue
    Inherits UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        If IsPostBack = False Then

            Dim objKasse As Kasse = CType(Page.Session("mKasse"), Kasse)

            Dim Applications As DataView = New DataView(objKasse.Applications)

            'Nur "Superadmin"-User dürfen Anwendungen mit Autorisierungslevel >0 sehen/nutzen
            If Not objKasse.Master Then
                Applications.RowFilter = "ISNULL(AuthorizationLevel, 0) = 0"
            End If

            GridChange.DataSource = Applications
            GridChange.DataBind()
        End If
    End Sub

    Private Sub GridChange_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles GridChange.RowCommand
        If e.CommandName = "goTo" Then
            Dim zielUrl As String = e.CommandArgument.ToString()

            If zielUrl.StartsWith("http") Then
                OpenExternalLink(zielUrl)
            Else
                Response.Redirect("/KBS/Forms/" & zielUrl)
            End If
        End If
    End Sub

    Private Sub OpenExternalLink(ByVal zielUrl As String)
        If (Not Page.ClientScript.IsStartupScriptRegistered("OpenExternalLink")) Then

            Dim sb As StringBuilder = New StringBuilder()
            sb.Append("<script type=""text/javascript"">")
            sb.Append("window.open(""" & zielUrl & """, ""_blank"", ""left=0,top=0,resizable=YES,scrollbars=YES"");" & vbCrLf)
            sb.Append("</script>")
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "OpenExternalLink", sb.ToString())

        End If
    End Sub
End Class
