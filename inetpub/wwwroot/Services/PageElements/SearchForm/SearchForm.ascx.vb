Imports System
Imports System.ComponentModel


Public Class SearchForm
    Inherits System.Web.UI.UserControl


    Private ReadOnly Property TbFahrgestell() As TextBox
        Get
            Return txtFahrgestellnummer
        End Get
    End Property

    Private ReadOnly Property TbBrief() As TextBox
        Get
            Return txtBrief
        End Get
    End Property

    Private ReadOnly Property TbKennzeichen() As TextBox
        Get
            Return txtAmtlKennzeichen
        End Get
    End Property

    Private ReadOnly Property TbReferenz1() As TextBox
        Get
            Return txtReferenz1
        End Get
    End Property

    Private ReadOnly Property TbReferenz2() As TextBox
        Get
            Return txtReferenz2
        End Get
    End Property


    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never)>
    Public ReadOnly Property FahrgestellText() As String
        Get
            Return GetTextWithAutoStars(TbFahrgestell)
        End Get
    End Property

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never)>
    Public ReadOnly Property BriefText() As String
        Get
            Return GetTextWithAutoStars(TbBrief)
        End Get
    End Property

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never)>
    Public ReadOnly Property KennzeichenText() As String
        Get
            Return GetTextWithAutoStars(TbKennzeichen)
        End Get
    End Property

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never)>
    Public ReadOnly Property Referenz1Text() As String
        Get
            Return GetTextWithAutoStars(TbReferenz1)
        End Get
    End Property

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never)>
    Public ReadOnly Property Referenz2Text() As String
        Get
            Return GetTextWithAutoStars(TbReferenz2)
        End Get
    End Property

    Public Function IsEmpty() As Boolean
        Return Not (TextBoxVisibleAndNotEmpty(TbFahrgestell) _
                    Or TextBoxVisibleAndNotEmpty(TbBrief) _
                    Or TextBoxVisibleAndNotEmpty(TbKennzeichen) _
                    Or TextBoxVisibleAndNotEmpty(TbReferenz1) _
                    Or TextBoxVisibleAndNotEmpty(TbReferenz2))
    End Function

    Private Function TextBoxVisibleAndNotEmpty(tb As TextBox) As Boolean
        Return tb.Visible And tb.Text <> String.Empty
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim t As String

        t = GetTextWithAutoStars(TbKennzeichen)
        t = GetTextWithAutoStars(TbBrief)
        t = GetTextWithAutoStars(TbFahrgestell)
        t = GetTextWithAutoStars(TbReferenz1)
        t = GetTextWithAutoStars(TbReferenz2)

    End Sub

    Public Function GetTextWithAutoStars(tb As TextBox) As String

        Dim sMaxLen As String
        Dim hiddenControl As HtmlInputHidden

        Dim text As String
        Dim maxLen As Integer

        text = tb.Text

        hiddenControl = FindControlRecursive(tb.Parent, tb.ID.Substring(3) + "Hidden")
        If (hiddenControl Is Nothing) Then Return text
        sMaxLen = hiddenControl.Value

        If (String.IsNullOrEmpty(tb.Text) Or Not Int32.TryParse(sMaxLen, maxLen)) Then
            Return ""
        End If

        If (text.Length < maxLen) Then
            If (text(0) <> "*") Then text = "*" + text
            If (text(text.Length - 1) <> "*") Then text = text + "*"
        End If

        Return text

    End Function

    Private Function FindControlRecursive(
        ByVal rootControl As Control, ByVal controlID As String) As Control

        If rootControl.ID = controlID Then
            Return rootControl
        End If

        For Each controlToSearch As Control In rootControl.Controls
            Dim controlToReturn As Control =
                FindControlRecursive(controlToSearch, controlID)
            If controlToReturn IsNot Nothing Then
                Return controlToReturn
            End If
        Next
        Return Nothing
    End Function

End Class