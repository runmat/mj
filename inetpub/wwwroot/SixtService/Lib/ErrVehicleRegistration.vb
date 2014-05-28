Public Class ErrVehicleRegistration
    Inherits CollectionBase

    Public Sub Add(ByVal ErrMessage As String)
        List.Add(ErrMessage)
    End Sub

    Default Public Overridable ReadOnly Property Item(ByVal index As Integer) As String
        Get
            Return CType(Me.List(index), String)
        End Get
    End Property
End Class
