Option Strict On
Option Explicit On

Imports System.Collections.ObjectModel
Imports System.ComponentModel

<Serializable()>
    Public Class Fahrzeug
    Implements INotifyPropertyChanged

    <Serializable()>
    Private Class FahrtenCollection
        Inherits Collection(Of Fahrt)
        Private ReadOnly owner As Fahrzeug

        Public Sub New(owner As Fahrzeug)
            Me.owner = owner
        End Sub

        Protected Overrides Sub ClearItems()
            For Each Item As Fahrt In Me.Items
                Item.SetFahrzeug(Nothing)
            Next

            MyBase.ClearItems()
            Me.owner.RaisePropertyChanged("Zusatzfahrten")
        End Sub

        Protected Overrides Sub InsertItem(index As Integer, item As Fahrt)
            If item Is Nothing Then
                Throw New ArgumentNullException()
            End If

            If index < Me.Count Then
                Me(index).SetFahrzeug(Nothing)
            End If

            If item.Fahrzeug IsNot Nothing Then
                item.Fahrzeug.Zusatzfahrten.Remove(item)
            End If

            item.SetFahrzeug(Me.owner)

            MyBase.InsertItem(index, item)
            Me.owner.RaisePropertyChanged("Zusatzfahrten")
        End Sub

        Protected Overrides Sub RemoveItem(index As Integer)
            Me(index).SetFahrzeug(Nothing)
            MyBase.RemoveItem(index)
            Me.owner.RaisePropertyChanged("Zusatzfahrten")
        End Sub

        Protected Overrides Sub SetItem(index As Integer, item As Fahrt)
            If item Is Nothing Then
                Throw New ArgumentNullException()
            End If

            Me(index).SetFahrzeug(Nothing)

            If item.Fahrzeug IsNot Nothing Then
                item.Fahrzeug.Zusatzfahrten.Remove(item)
            End If

            item.SetFahrzeug(Me.owner)

            MyBase.SetItem(index, item)
            Me.owner.RaisePropertyChanged("Zusatzfahrten")
        End Sub
    End Class

    <Serializable()>
    Private Class ProtokollCollection
        Inherits Collection(Of Protokoll)
        Private ReadOnly owner As Fahrzeug

        Public Sub New(owner As Fahrzeug)
            Me.owner = owner
        End Sub

        Protected Overrides Sub ClearItems()
            For Each Item As Protokoll In Me.Items
                Item.SetFahrzeug(Nothing)
            Next

            MyBase.ClearItems()
            Me.owner.RaisePropertyChanged("Protokolle")
        End Sub

        Protected Overrides Sub InsertItem(index As Integer, item As Protokoll)
            If item Is Nothing Then
                Throw New ArgumentNullException()
            End If

            If index < Me.Count Then
                Me(index).SetFahrzeug(Nothing)
            End If

            If item.Fahrzeug IsNot Nothing Then
                item.Fahrzeug.Protokolle.Remove(item)
            End If

            item.SetFahrzeug(Me.owner)

            MyBase.InsertItem(index, item)
            Me.owner.RaisePropertyChanged("Protokolle")
        End Sub

        Protected Overrides Sub RemoveItem(index As Integer)
            Me(index).SetFahrzeug(Nothing)
            MyBase.RemoveItem(index)
            Me.owner.RaisePropertyChanged("Protokolle")
        End Sub

        Protected Overrides Sub SetItem(index As Integer, item As Protokoll)
            If item Is Nothing Then
                Throw New ArgumentNullException()
            End If

            Me(index).SetFahrzeug(Nothing)

            If item.Fahrzeug IsNot Nothing Then
                item.Fahrzeug.Protokolle.Remove(item)
            End If

            item.SetFahrzeug(Me.owner)

            MyBase.SetItem(index, item)
            Me.owner.RaisePropertyChanged("Protokolle")
        End Sub
    End Class

    Private ReadOnly _protokolle As New ProtokollCollection(Me)
    Private ReadOnly _zusatzfahrten As New FahrtenCollection(Me)
    Private _abholfahrt As Fahrt
    Private _zielfahrt As Fahrt
    Private _halterfahrt As Fahrt

    Public Property Fahrgestellnummer As String
    Public Property Zugelassen As String
    Public Property Kennzeichen As String
    Public Property Beauftragt As String
    Public Property Typ As String
    Public Property Bereifung As String
    Public Property Referenznummer As String
    Public Property Klasse As String
    Public Property Wert As String

    Public Property Abholfahrt As Fahrt
        Get
            Return Me._abholfahrt
        End Get
        Set(value As Fahrt)
            If Me._abholfahrt IsNot Nothing Then
                Me._abholfahrt.SetFahrzeug(Nothing)
            End If

            If value IsNot Nothing Then
                If value.Fahrzeug IsNot Nothing Then
                    value.Fahrzeug.Abholfahrt = Nothing
                End If

                value.SetFahrzeug(Me)
            End If

            Me._abholfahrt = value
            Me.RaisePropertyChanged("Abholfahrt")
        End Set
    End Property

    Public Property Zielfahrt As Fahrt
        Get
            Return Me._zielfahrt
        End Get
        Set(value As Fahrt)
            If Me._zielfahrt IsNot Nothing Then
                Me._zielfahrt.SetFahrzeug(Nothing)
            End If

            If value IsNot Nothing Then
                If value.Fahrzeug IsNot Nothing Then
                    value.Fahrzeug.Zielfahrt = Nothing
                End If

                value.SetFahrzeug(Me)
            End If

            Me._zielfahrt = value
            Me.RaisePropertyChanged("Zielfahrt")
        End Set
    End Property


    Public Property Halterfahrt As Fahrt
        Get
            Return Me._halterfahrt
        End Get
        Set(value As Fahrt)
            If Me._halterfahrt IsNot Nothing Then
                Me._halterfahrt.SetFahrzeug(Nothing)
            End If

            If value IsNot Nothing Then
                If value.Fahrzeug IsNot Nothing Then
                    value.Fahrzeug.Halterfahrt = Nothing
                End If

                value.SetFahrzeug(Me)
            End If

            Me._halterfahrt = value
            Me.RaisePropertyChanged("Halterfahrt")
        End Set
    End Property

    Public ReadOnly Property Zusatzfahrten As Collection(Of Fahrt)
        Get
            Return Me._zusatzfahrten
        End Get
    End Property

    Public ReadOnly Property Protokolle As Collection(Of Protokoll)
        Get
            Return Me._protokolle
        End Get
    End Property

    Private Sub RaisePropertyChanged(propertyName As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    End Sub

    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged
End Class