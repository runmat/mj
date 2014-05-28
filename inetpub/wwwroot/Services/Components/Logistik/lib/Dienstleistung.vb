Option Strict On
Option Explicit On

<Serializable()>
    Public Class Dienstleistung
    Implements IEquatable(Of Dienstleistung)

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

    Public Property Nummer As String
    Public Property Text As String
    Public Property MatNummer As String

    Public Overrides Function Equals(obj As Object) As Boolean
        Dim other As Dienstleistung = TryCast(obj, Dienstleistung)
        Return other IsNot Nothing AndAlso Me.Equals(other)
    End Function

    Public Overloads Function Equals(other As Dienstleistung) As Boolean Implements System.IEquatable(Of Dienstleistung).Equals
        Return other IsNot Nothing AndAlso
            String.Equals(Me.Nummer, other.Nummer, StringComparison.Ordinal) AndAlso
            Nullable.Equals(Me.Text, other.Text) AndAlso
            String.Equals(Me.MatNummer, other.MatNummer, StringComparison.Ordinal)
    End Function

    Public Overloads Shared Function Equals(left As Dienstleistung, right As Dienstleistung) As Boolean
        Return If(left Is Nothing, right Is Nothing, left.Equals(right))
    End Function
End Class