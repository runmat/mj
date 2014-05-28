Option Strict On
Option Explicit On

Imports System.Collections.ObjectModel

<Serializable()>
    Public Class Fahrt
    Implements Runtime.Serialization.IDeserializationCallback
    Implements IEquatable(Of Fahrt)

    <Serializable()>
    Private Class DienstleistungenCollection
        Inherits Collection(Of Dienstleistung)
        Private ReadOnly owner As Fahrt

        Public Sub New(owner As Fahrt)
            Me.owner = owner
        End Sub

        Protected Overrides Sub ClearItems()
            For Each Item As Dienstleistung In Me.Items
                Item.SetFahrt(Nothing)
            Next

            MyBase.ClearItems()
        End Sub

        Protected Overrides Sub InsertItem(index As Integer, item As Dienstleistung)
            If item Is Nothing Then
                Throw New ArgumentNullException()
            End If

            If index < Me.Count Then
                Me(index).SetFahrt(Nothing)
            End If

            If item.Fahrt IsNot Nothing Then
                item.Fahrt.Dienstleistungen.Remove(item)
            End If

            item.SetFahrt(Me.owner)

            MyBase.InsertItem(index, item)
        End Sub

        Protected Overrides Sub RemoveItem(index As Integer)
            Me(index).SetFahrt(Nothing)
            MyBase.RemoveItem(index)
        End Sub

        Protected Overrides Sub SetItem(index As Integer, item As Dienstleistung)
            If item Is Nothing Then
                Throw New ArgumentNullException()
            End If

            Me(index).SetFahrt(Nothing)

            If item.Fahrt IsNot Nothing Then
                item.Fahrt.Dienstleistungen.Remove(item)
            End If

            item.SetFahrt(Me.owner)

            MyBase.SetItem(index, item)
        End Sub

    End Class

    <NonSerialized()>
    Private _fahrzeug As Fahrzeug
    Private _adresse As Adresse
    Private ReadOnly _dienstleistungen As New DienstleistungenCollection(Me)

    Friend Sub SetFahrzeug(fahrzeug As Fahrzeug)
        Me._fahrzeug = fahrzeug
    End Sub

    Public ReadOnly Property Fahrzeug As Fahrzeug
        Get
            Return Me._fahrzeug
        End Get
    End Property

    Public Property Transporttyp As String
    Public Property TransporttypCode As String
    Public Property Datum As DateTime?
    Public Property ZeitVon As String
    Public Property ZeitBis As String
    Public Property Bemerkung As String

    Public Property Adresse As Adresse
        Get
            Return Me._adresse
        End Get
        Set(value As Adresse)
            If Me._adresse IsNot Nothing Then
                Me._adresse.SetFahrt(Nothing)
            End If

            If value IsNot Nothing Then
                If value.Fahrt IsNot Nothing Then
                    value.Fahrt.Adresse = Nothing
                End If

                value.SetFahrt(Me)
            End If

            Me._adresse = value
        End Set
    End Property

    Public Overrides Function Equals(obj As Object) As Boolean
        Dim other As Fahrt = TryCast(obj, Fahrt)
        Return other IsNot Nothing AndAlso Me.Equals(other)
    End Function

    Public Overloads Function Equals(other As Fahrt) As Boolean Implements System.IEquatable(Of Fahrt).Equals
        Return other IsNot Nothing AndAlso
            String.Equals(Me.TransporttypCode, other.TransporttypCode, StringComparison.Ordinal) AndAlso
            Nullable.Equals(Me.Datum, other.Datum) AndAlso
            String.Equals(Me.ZeitVon, other.ZeitVon, StringComparison.Ordinal) AndAlso
            String.Equals(Me.ZeitBis, other.ZeitBis, StringComparison.Ordinal) AndAlso
            String.Equals(Me.Bemerkung, other.Bemerkung, StringComparison.Ordinal) AndAlso
            Adresse.Equals(Me.Adresse, other.Adresse)
    End Function

    Public Overloads Shared Function Equals(left As Fahrt, right As Fahrt) As Boolean
        Return If(left Is Nothing, right Is Nothing, left.Equals(right))
    End Function

    Public ReadOnly Property Dienstleistungen As Collection(Of Dienstleistung)
        Get
            Return Me._dienstleistungen
        End Get
    End Property

    Public Sub OnDeserialization(sender As Object) Implements System.Runtime.Serialization.IDeserializationCallback.OnDeserialization
        If Me._adresse IsNot Nothing Then
            Me._adresse.SetFahrt(Me)
        End If
    End Sub
End Class