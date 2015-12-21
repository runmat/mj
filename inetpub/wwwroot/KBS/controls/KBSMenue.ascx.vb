Option Strict On
Option Explicit On

Imports GeneralTools.Models

Partial Public Class KBSMenue
    Inherits UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        If IsPostBack = False Then

            Dim objKasse As Kasse = CType(Page.Session("mKasse"), Kasse)

            Dim Applications As New DataView(objKasse.Applications)

            'Nur "Superadmin"-User dürfen Anwendungen mit Autorisierungslevel >0 sehen/nutzen
            If Not objKasse.Master Then
                Applications.RowFilter = "ISNULL(AuthorizationLevel, 0) = 0"
            End If

            repeater1.DataSource = Applications
            repeater1.DataBind()
        End If
    End Sub

    Protected Sub repeater1_OnItemDataBound(sender As Object, e As RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim appType As String = CType(e.Item.DataItem, DataRowView)("AppType").ToString()

            Dim existingGroups() As String = dataGroups.Value.NotNullOrEmpty().Split(";"c)
            If Array.IndexOf(existingGroups, appType) = -1 Then
                dataGroups.Value &= appType + ";"
            End If

            'zum javascriptseitigen Aufbereiten der Gruppen
            CType(e.Item.FindControl("tableItem"), HtmlTable).Attributes.Add("data-group", appType)
        End If
    End Sub

    Protected Sub lbApplication_OnClick(sender As Object, e As EventArgs)
        Dim zielUrl As String = CType(sender, LinkButton).CommandArgument

        If zielUrl.StartsWith("http") Then
            OpenExternalLink(zielUrl)
        Else
            Response.Redirect("/KBS/Forms/" & zielUrl)
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
