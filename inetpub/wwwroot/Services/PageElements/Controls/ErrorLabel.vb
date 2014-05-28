Imports System.Web.UI.WebControls


Public Class ErrorLabel
    Inherits Label

    Public Overrides Property Text() As String
        Get
            Return MyBase.Text
        End Get
        Set(ByVal value As String)
            MyBase.Text = value
            Visible = Not String.IsNullOrEmpty(Text)
        End Set
    End Property

    Protected Overrides ReadOnly Property TagKey() As HtmlTextWriterTag
        Get
            Return HtmlTextWriterTag.Div
        End Get
    End Property

    Public Sub New()
        'Style.Add("padding-top", "30px")
        'Style.Add("background-color", "yellow")

        CssClass = "ErrorLabel"

        Visible = Not String.IsNullOrEmpty(Text)
    End Sub

End Class

