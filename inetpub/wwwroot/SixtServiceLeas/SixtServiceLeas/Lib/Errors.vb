Public Class Errors
    Inherits CollectionBase

    Public Sub Add(ByVal VehicleError As _Error)
        List.Add(VehicleError)
    End Sub

    Default Public Overridable ReadOnly Property Item(ByVal index As Integer) As _Error
        Get
            Return CType(Me.List(index), _Error)
        End Get
    End Property
End Class
