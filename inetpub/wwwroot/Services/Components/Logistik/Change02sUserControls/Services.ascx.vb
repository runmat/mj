Option Strict On
Option Explicit On

Imports Telerik.Web.UI

<ValidationProperty("ValidationProperty")> _
Public Class Services
    Inherits UserControl

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)

        ScriptManager.RegisterClientScriptBlock(Me, GetType(Services), "Services_DoubleClick", "function doubleClick(s, e) { " +
                                                "var item = e.get_item(); " +
                                                "s.transferToDestination(item);} ", True)
    End Sub

    Public ReadOnly Property ValidationProperty As String
        Get
            Return String.Join(",", Dienstleistungen.ToArray())
        End Get
    End Property

    Public Property Dienstleistungen As IEnumerable(Of String)
        Get
            Return selectedServices.Items.Select(Function(i) i.Value).ToList
        End Get
        Set(value As IEnumerable(Of String))
            selectedValues.Value = String.Join(",", value.ToArray)
            TrySyncSelection()
        End Set
    End Property

    Public Property Bemerkung As String
        Get
            Return txtAbBemerkung.Text
        End Get
        Set(value As String)
            txtAbBemerkung.Text = value
        End Set
    End Property

    Public Property ValidationGroup As String

    Public ReadOnly Property MustInitSelectbox As Boolean
        Get
            Return _items Is Nothing AndAlso availableServices.Items.Count = 0 AndAlso selectedServices.Items.Count = 0
        End Get
    End Property

    Public Sub InitSelectbox(services As IEnumerable)
        _items = New List(Of RadListBoxItem)
        For Each service As Object In services
            Dim text As String = CStr(DataBinder.Eval(service, "Text"))
            Dim value As String = CStr(DataBinder.Eval(service, "Nummer"))
            _items.Add(New RadListBoxItem(text, value))
        Next
        TrySyncSelection()
    End Sub

    Private Sub TrySyncSelection()
        If Not _items Is Nothing Then
            selectedServices.Items.Clear()
            availableServices.Items.Clear()

            Dim selected = selectedValues.Value.Split(","c)
            For Each item In _items
                If Not selected.Contains(item.Value) Then
                    availableServices.Items.Add(item)
                Else
                    selectedServices.Items.Add(item)
                End If
            Next
        End If
    End Sub

    Private _items As List(Of RadListBoxItem)

    Protected Sub ServicesDropped(sender As Object, e As RadListBoxDroppedEventArgs)
        selectedValues.Value = ValidationProperty
        ResetPreselection()
    End Sub

    Protected Sub ServicesTransferred(ByVal sender As Object, ByVal e As RadListBoxTransferredEventArgs)
        selectedValues.Value = ValidationProperty
        ResetPreselection()
    End Sub

    Public Sub ResetPreselection()
        availableServices.Items.ToList().ForEach(Sub(i) i.Selected = False)
    End Sub

    Public Sub Preselect(ByVal value As String)
        Dim item = availableServices.Items.FirstOrDefault(Function(i) i.Value = value)
        If Not item Is Nothing Then item.Selected = True
    End Sub

End Class