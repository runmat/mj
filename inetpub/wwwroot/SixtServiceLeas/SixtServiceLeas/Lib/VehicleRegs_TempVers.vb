Public Class VehicleRegs_TempVers

    Inherits CollectionBase

    Public Sub Add(ByVal VehicleReg As VehicleRegistrationTempVers)
        List.Add(VehicleReg)
    End Sub

    Default Public Overridable ReadOnly Property Item(ByVal index As Integer) As VehicleRegistrationTempVers
        Get
            Return CType(List(index), VehicleRegistrationTempVers)
        End Get
    End Property

End Class
