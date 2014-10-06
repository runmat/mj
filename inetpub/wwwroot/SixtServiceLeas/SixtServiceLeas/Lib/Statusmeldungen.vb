Public Class Statusmeldungen
    Inherits CollectionBase

    Public Sub Add(ByVal VehicleReg As Statusmeldung)
        List.Add(VehicleReg)
    End Sub

    Default Public Overridable ReadOnly Property Item(ByVal index As Integer) As Statusmeldung
        Get
            Return CType(List(index), Statusmeldung)
        End Get
    End Property

End Class
