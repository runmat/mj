Public Class Brieffreigaben
    Inherits CollectionBase

    Public Sub Add(ByVal Freigabe As Brieffreigabe)
        List.Add(Freigabe)
    End Sub

    Default Public Overridable ReadOnly Property Item(ByVal index As Integer) As Brieffreigabe
        Get
            Return CType(Me.List(index), Brieffreigabe)
        End Get
    End Property
End Class
