Option Strict On
Option Explicit On

<ValidationProperty("IsDirty")>
    Public Class Zusatzfahrten
    Inherits TranslatedUserControl

    Public ReadOnly Property Fahrten As IList(Of Fahrt)
        Get
            Dim ret As IList(Of Fahrt) = DirectCast(Me.ViewState("Fahrten"), IList(Of Fahrt))

            If ret Is Nothing Then
                ret = New List(Of Fahrt)()
                Me.ViewState("Fahrten") = ret
            End If

            Return ret
        End Get
    End Property

    Public Property ValidationGroup As String
        Get
            Return Me.TransportAddress.ValidationGroup
        End Get
        Set(value As String)
            Me.TransportAddress.ValidationGroup = value
        End Set
    End Property

    Public ReadOnly Property IsDirty As Boolean
        Get
            Dim fahrt As Fahrt = Me.TransportAddress.Fahrt

            Return fahrt IsNot Nothing AndAlso
                (Me.gvFahrten.SelectedIndex = -1 OrElse
                 Not fahrt.Equals(Me.Fahrten(Me.gvFahrten.SelectedIndex)))
        End Get
    End Property

    Public ReadOnly Property MustInitDropdowns As Boolean
        Get
            Return Me.TransportAddress.MustInitDropdowns
        End Get
    End Property

    Protected Overrides Sub OnLoad(e As System.EventArgs)
        MyBase.OnLoad(e)

        Me.FillGrid()
    End Sub

    Public Sub InitDropdowns(countries As IEnumerable, times As IEnumerable, Optional transportTypes As IEnumerable = Nothing)
        Me.TransportAddress.InitDropdowns(countries, times, transportTypes)
    End Sub

    Public Sub ShowVehicleDetails(kfz As Fahrzeug)
        Me.TransportAddress.ShowVehicleDetails(kfz)
    End Sub

    Private Sub FillGrid()
        Me.gvFahrten.DataSource = Me.Fahrten
        Me.gvFahrten.DataBind()
    End Sub

    Protected Sub OnFahrtenDataBound(sender As Object, e As EventArgs)
        If Me.gvFahrten.Rows.Count > 0 Then
            Dim wctl As WebControl = DirectCast(Me.gvFahrten.Rows(0).FindControl("lbUp"), WebControl)
            wctl.Style.Add(HtmlTextWriterStyle.Visibility, "hidden")
            wctl = DirectCast(Me.gvFahrten.Rows(Me.gvFahrten.Rows.Count - 1).FindControl("lbDown"), WebControl)
            wctl.Style.Add(HtmlTextWriterStyle.Visibility, "hidden")
        End If
    End Sub

    Protected Sub OnSelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs)
        e.Cancel = Me.IsDirty
    End Sub

    Protected Sub OnSelectedIndexChanged(sender As Object, e As EventArgs)
        If Me.gvFahrten.SelectedIndex = -1 Then
            Me.TransportAddress.Fahrt = Nothing
        Else
            Me.TransportAddress.Fahrt = Me.Fahrten(Me.gvFahrten.SelectedIndex)
        End If
    End Sub

    Protected Sub OnRowDeleting(sender As Object, e As GridViewDeleteEventArgs)
        If Me.gvFahrten.SelectedIndex = e.RowIndex Then
            Me.gvFahrten.SelectedIndex = -1
            Me.TransportAddress.Fahrt = Nothing
        End If

        Me.Fahrten.RemoveAt(e.RowIndex)
        Me.FillGrid()
    End Sub

    Protected Sub OnRowCommand(sender As Object, e As GridViewCommandEventArgs)
        Select Case e.CommandName
            Case "Up"
                Dim index As Integer = CInt(e.CommandArgument)
                Dim temp As Fahrt = Me.Fahrten(index - 1)
                Me.Fahrten(index - 1) = Me.Fahrten(index)
                Me.Fahrten(index) = temp
                Me.FillGrid()
            Case "Down"
                Dim index As Integer = CInt(e.CommandArgument)
                Dim temp As Fahrt = Me.Fahrten(index + 1)
                Me.Fahrten(index + 1) = Me.Fahrten(index)
                Me.Fahrten(index) = temp
                Me.FillGrid()
        End Select
    End Sub

    Protected Sub OnValidateDate(sender As Object, e As ValidateDateEventArgs)
        Dim tmpDate As DateTime
        e.IsValid = DateTime.TryParse(e.Datum, tmpDate)
    End Sub

    Protected Sub OnSave(sender As Object, e As EventArgs)
        Me.Page.Validate(Me.ValidationGroup)

        If Me.Page.IsValid Then
            Dim fahrt As Fahrt = Me.TransportAddress.Fahrt
            If Me.gvFahrten.SelectedIndex = -1 Then
                Me.Fahrten.Add(fahrt)
            Else
                Me.Fahrten(Me.gvFahrten.SelectedIndex) = fahrt
            End If

            Me.FillGrid()

            Me.gvFahrten.SelectedIndex = -1
            Me.TransportAddress.Fahrt = Nothing
        End If
    End Sub

    Protected Sub OnCancel(sender As Object, e As EventArgs)
        Me.gvFahrten.SelectedIndex = -1
        Me.TransportAddress.Fahrt = Nothing
    End Sub

End Class