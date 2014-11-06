Public Class VehicleRegs_Status
    Inherits CollectionBase

    Public Sub Add(ByVal VehicleReg As VehicleRegistrationStatus)
        List.Add(VehicleReg)
    End Sub

    Default Public Overridable ReadOnly Property Item(ByVal index As Integer) As VehicleRegistrationStatus
        Get
            Return CType(List(index), VehicleRegistrationStatus)
        End Get
    End Property

End Class
