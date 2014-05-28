Imports CKG.Portal.PageElements

Partial Public Class VIPMessage
    Inherits System.Web.UI.Page
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ucHeader.Visible = False
        If Not Session("App_AdminMessage") Is Nothing Then
            Dim Table As DataTable
            Table = CType(Session("App_AdminMessage"), DataTable)
            Repeater1.DataSource = Table
            Repeater1.DataBind()
        End If

    End Sub

End Class