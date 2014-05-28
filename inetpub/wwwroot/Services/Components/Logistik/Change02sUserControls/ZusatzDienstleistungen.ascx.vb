Option Strict On
Option Explicit On

Public NotInheritable Class NeedsServicesEventArgs
    Inherits EventArgs

    Private ReadOnly _fahrt As Fahrt

    Public ReadOnly Property Fahrt As Fahrt
        Get
            Return Me._fahrt
        End Get
    End Property

    Public Property Dienstleistungen As IEnumerable(Of Dienstleistung)

    Public Sub New(fahrt As Fahrt)
        Me._fahrt = fahrt
    End Sub
End Class

<ValidationProperty("IsDirty")>
Public Class ZusatzDienstleistungen
    Inherits TranslatedUserControl

    Private Shared ReadOnly NeedsServicesEvent As New Object()

    Public Custom Event NeedsServices As EventHandler(Of NeedsServicesEventArgs)
        AddHandler(value As EventHandler(Of NeedsServicesEventArgs))
            Me.Events.AddHandler(ZusatzDienstleistungen.NeedsServicesEvent, value)
        End AddHandler

        RemoveHandler(value As EventHandler(Of NeedsServicesEventArgs))
            Me.Events.RemoveHandler(ZusatzDienstleistungen.NeedsServicesEvent, value)
        End RemoveHandler

        RaiseEvent(sender As Object, e As NeedsServicesEventArgs)
            Dim eh As EventHandler(Of NeedsServicesEventArgs) = DirectCast(Me.Events(ZusatzDienstleistungen.NeedsServicesEvent), EventHandler(Of NeedsServicesEventArgs))

            If eh IsNot Nothing Then
                eh(sender, e)
            End If
        End RaiseEvent
    End Event

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
            Return Me.Services.ValidationGroup
        End Get
        Set(value As String)
            Me.Services.ValidationGroup = value
        End Set
    End Property

    Public ReadOnly Property IsDirty As Boolean
        Get
            Dim services As New HashSet(Of String)(Me.Services.Dienstleistungen)

            Return Me.gvFahrten.SelectedIndex <> -1 AndAlso _
                Not services.SetEquals(From dienstleistung In Me.Fahrten(Me.gvFahrten.SelectedIndex).Dienstleistungen _
                                           Select dienstleistung.Nummer)
        End Get
    End Property

    Public Sub SetFahrten(fahrten As IEnumerable(Of Fahrt))
        Me.ViewState("Fahrten") = New List(Of Fahrt)(fahrten)
        Me.FillGrid()
    End Sub

    Protected Overrides Sub OnLoad(e As System.EventArgs)
        MyBase.OnLoad(e)

        Me.FillGrid()
    End Sub

    Private Sub FillGrid()
        Me.gvFahrten.DataSource = Me.Fahrten
        Me.gvFahrten.DataBind()

        If Me.gvFahrten.SelectedIndex <> -1 Then
            Dim fahrt As Fahrt = Me.Fahrten(Me.gvFahrten.SelectedIndex)
            Dim ne As New NeedsServicesEventArgs(fahrt)

            RaiseEvent NeedsServices(Me, ne)

            Me.Services.InitSelectbox(ne.Dienstleistungen)
        End If
    End Sub

    Protected Sub OnSelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs)
        e.Cancel = Me.IsDirty
    End Sub

    Protected Sub OnSelectedIndexChanged(sender As Object, e As EventArgs)
        If Me.gvFahrten.SelectedIndex = -1 Then
            Me.Reset()
        Else
            Dim fahrt As Fahrt = Me.Fahrten(Me.gvFahrten.SelectedIndex)
            Me.Services.Dienstleistungen = From dienstleistung In fahrt.Dienstleistungen _
                                           Select dienstleistung.Nummer
            Dim ne As New NeedsServicesEventArgs(fahrt)

            RaiseEvent NeedsServices(Me, ne)

            Me.Services.InitSelectbox(ne.Dienstleistungen)
            Me.Services.Bemerkung = fahrt.Bemerkung
            Me.ServicesPanel.Enabled = True
        End If
    End Sub

    Protected Sub OnSave(sender As Object, e As EventArgs)
        Me.Page.Validate(Me.ValidationGroup)

        If Me.Page.IsValid Then
            Dim fahrt As Fahrt = Me.Fahrten(Me.gvFahrten.SelectedIndex)
            Dim ne As New NeedsServicesEventArgs(fahrt)

            RaiseEvent NeedsServices(Me, ne)

            fahrt.Dienstleistungen.Clear()
            Dim services As New HashSet(Of String)(Me.Services.Dienstleistungen)
            Dim dienstleistungen As IEnumerable(Of Dienstleistung) = From d In ne.Dienstleistungen
                                                                     Where services.Contains(d.Nummer)
                                                                     Select d

            For Each dienstleistung As Dienstleistung In dienstleistungen
                fahrt.Dienstleistungen.Add(dienstleistung)
            Next

            fahrt.Bemerkung = Me.Services.Bemerkung
            Me.Reset()
        End If
    End Sub

    Protected Sub OnCancel(sender As Object, e As EventArgs)
        Me.Reset()
    End Sub

    Private Sub Reset()
        Me.gvFahrten.SelectedIndex = -1
        Me.Services.InitSelectbox(Nothing)
        Me.Services.Bemerkung = String.Empty
        Me.ServicesPanel.Enabled = False
    End Sub
End Class