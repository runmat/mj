Namespace Controls

    Public Class ScrollHereControl
        Inherits System.Web.UI.HtmlControls.HtmlGenericControl

        Public Sub New()
            MyBase.New("A")
            MyBase.ID = "ScrollHere"
            MyBase.Attributes.Add("name", "#ScrollHere")
            MyBase.EnableViewState = False
        End Sub


        Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)

            MyBase.Render(writer)
            writer.WriteLine("<script language=""JavaScript"">")
            writer.WriteLine("<!--")
            writer.WriteLine("  location.href='#ScrollHere';")
            writer.WriteLine("-->")
            writer.WriteLine("</script>")

        End Sub

    End Class

End Namespace