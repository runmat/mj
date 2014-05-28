Imports System.Data
Imports System.Configuration
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports System.Collections
Imports System.Collections.Generic


Namespace CKG_Adapter
    'Public Class GroupedDropDownListAdapter
    Public NotInheritable Class GroupedDropDownListAdapter
        Inherits System.Web.UI.WebControls.DropDownList
        'Inherits System.Web.UI.WebControls.Adapters.WebControlAdapter
        Protected Overrides Sub RenderContents(writer As HtmlTextWriter)
            'Dim list As DropDownList = TryCast(Me.Control, DropDownList)
            Dim list As DropDownList = Me
            Dim currentOptionGroup As String
            Dim renderedOptionGroups As New List(Of String)()

            For Each item As ListItem In list.Items
                If item.Attributes("OptionGroup") Is Nothing Then
                    RenderListItem(item, writer)
                Else
                    currentOptionGroup = item.Attributes("OptionGroup")

                    If renderedOptionGroups.Contains(currentOptionGroup) Then
                        RenderListItem(item, writer)
                    Else
                        If renderedOptionGroups.Count > 0 Then
                            RenderOptionGroupEndTag(writer)
                        End If

                        RenderOptionGroupBeginTag(currentOptionGroup, writer)
                        renderedOptionGroups.Add(currentOptionGroup)

                        RenderListItem(item, writer)
                    End If
                End If
            Next

            If renderedOptionGroups.Count > 0 Then
                RenderOptionGroupEndTag(writer)
            End If
        End Sub

        Private Sub RenderOptionGroupBeginTag(name As String, writer As HtmlTextWriter)
            writer.WriteBeginTag("optgroup")
            writer.WriteAttribute("label", name)
            writer.Write(HtmlTextWriter.TagRightChar)
            writer.WriteLine()
        End Sub

        Private Sub RenderOptionGroupEndTag(writer As HtmlTextWriter)
            writer.WriteEndTag("optgroup")
            writer.WriteLine()
        End Sub

        Private Sub RenderListItem(item As ListItem, writer As HtmlTextWriter)
            writer.WriteBeginTag("option")
            writer.WriteAttribute("value", item.Value, True)

            If item.Selected Then
                writer.WriteAttribute("selected", "selected", False)
            End If

            For Each key As String In item.Attributes.Keys
                writer.WriteAttribute(key, item.Attributes(key))
            Next

            writer.Write(HtmlTextWriter.TagRightChar)
            HttpUtility.HtmlEncode(item.Text, writer)
            writer.WriteEndTag("option")
            writer.WriteLine()
        End Sub
    End Class
End Namespace