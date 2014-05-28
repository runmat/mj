Public Class VehicleDocuments
    Inherits CollectionBase

    Public Sub Add(ByVal VehicleDoc As VehicleDocument)
        List.Add(VehicleDoc)
    End Sub

    Default Public Overridable ReadOnly Property Item(ByVal index As Integer) As VehicleDocument
        Get
            Return CType(Me.List(index), VehicleDocument)
        End Get
    End Property
End Class
