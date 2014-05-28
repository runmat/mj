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

' ************************************************
' $History: ScrollHereControl.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:00
' Created in $/CKAG/Applications/appdcl/Controls
' 
' *****************  Version 1  *****************
' User: Uha          Date: 15.05.07   Time: 15:58
' Created in $/CKG/Applications/AppDCL/AppDCLWeb/Controls
' Änderungen aus StartApplication vom 11.05.2007
' 
' ************************************************
