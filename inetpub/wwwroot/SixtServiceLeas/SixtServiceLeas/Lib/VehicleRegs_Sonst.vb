Public Class VehicleRegs_Sonst
    Inherits CollectionBase

    Public Sub Add(ByVal VehicleReg As VehicleRegistrationSonst)
        List.Add(VehicleReg)
    End Sub

    Default Public Overridable ReadOnly Property Item(ByVal index As Integer) As VehicleRegistrationSonst
        Get
            Return CType(List(index), VehicleRegistrationSonst)
        End Get
    End Property
End Class
