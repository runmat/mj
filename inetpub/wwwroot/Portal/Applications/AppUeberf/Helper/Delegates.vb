Namespace Helper

    Public Class Delegates

        Public Delegate Sub ProcessException(ByVal ex As Exception)
        Public Delegate Sub ProcessAddresses(ByVal sender As Object, ByVal dt As DataSets.AddressDataSet.ADDRESSEDataTable)

    End Class

End Namespace