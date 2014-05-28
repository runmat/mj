Option Strict On
Option Explicit On

<Serializable()>
    Public Class Adresse
    Implements IEquatable(Of Adresse)

    <NonSerialized()>
    Private _fahrt As Fahrt

    Friend Sub SetFahrt(fahrt As Fahrt)
        Me._fahrt = fahrt
    End Sub

    Public ReadOnly Property Fahrt As Fahrt
        Get
            Return Me._fahrt
        End Get
    End Property

    Public Property DebitorNr As String
    Public Property Name As String
    Public Property Ansprechpartner As String
    Public Property Straße As String
    Public Property Postleitzahl As String
    Public Property Ort As String
    Public Property Land As String
    Public Property Telefon As String
    Public Property EMail As String
    Public Property Carport As String

    Public Overrides Function Equals(obj As Object) As Boolean
        Dim other As Adresse = TryCast(obj, Adresse)
        Return other IsNot Nothing AndAlso Me.Equals(other)
    End Function

    Public Overloads Function Equals(other As Adresse) As Boolean Implements System.IEquatable(Of Adresse).Equals
        Return other IsNot Nothing AndAlso
            String.Equals(Me.Name, other.Name, StringComparison.Ordinal) AndAlso
            String.Equals(Me.Ansprechpartner, other.Ansprechpartner, StringComparison.Ordinal) AndAlso
            String.Equals(Me.Straße, other.Straße, StringComparison.Ordinal) AndAlso
            String.Equals(Me.Postleitzahl, other.Postleitzahl, StringComparison.Ordinal) AndAlso
            String.Equals(Me.Ort, other.Ort, StringComparison.Ordinal) AndAlso
            String.Equals(Me.Land, other.Land, StringComparison.Ordinal) AndAlso
            String.Equals(Me.Telefon, other.Telefon, StringComparison.Ordinal)
    End Function

    Public Overloads Shared Function Equals(left As Adresse, right As Adresse) As Boolean
        Return If(left Is Nothing, right Is Nothing, left.Equals(right))
    End Function
End Class