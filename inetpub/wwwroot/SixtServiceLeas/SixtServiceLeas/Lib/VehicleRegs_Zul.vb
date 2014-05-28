Public Class VehicleRegs_Zul
    Inherits CollectionBase

    Public Sub Add(ByVal VehicleReg As VehicleRegistrationZul)
        List.Add(VehicleReg)
    End Sub

    Default Public Overridable ReadOnly Property Item(ByVal index As Integer) As VehicleRegistrationZul
        Get
            Return CType(Me.List(index), VehicleRegistrationZul)
        End Get
    End Property

End Class
