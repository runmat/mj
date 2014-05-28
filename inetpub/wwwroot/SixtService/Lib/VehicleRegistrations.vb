Public Class VehicleRegistrations
    Inherits CollectionBase

    Public Sub Add(ByVal VehicleReg As VehicleRegistration)
        List.Add(VehicleReg)
    End Sub

    Default Public Overridable ReadOnly Property Item(ByVal index As Integer) As VehicleRegistration
        Get
            Return CType(Me.List(index), VehicleRegistration)
        End Get
    End Property

End Class
