Public Class VehicleRegs_EndgVers
    Inherits CollectionBase

    Public Sub Add(ByVal VehicleReg As VehicleRegistrationEndgVers)
        List.Add(VehicleReg)
    End Sub

    Default Public Overridable ReadOnly Property Item(ByVal index As Integer) As VehicleRegistrationEndgVers
        Get
            Return CType(List(index), VehicleRegistrationEndgVers)
        End Get
    End Property
End Class
