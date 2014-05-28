
Public Class KVPSelection
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

    End Sub

    Protected Sub lb_zurueck_Click(sender As Object, e As EventArgs) Handles lb_zurueck.Click
        Session("ObjKVP") = Nothing
        Response.Redirect("../Selection.aspx")
    End Sub

End Class