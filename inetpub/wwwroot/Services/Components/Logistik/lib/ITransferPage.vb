Imports CKG.Base.Kernel.Security

Public Interface ITransferPage
    ReadOnly Property CSKUser As User
    ReadOnly Property CSKApp As App

    ReadOnly Property Transfer As Transfer
    ReadOnly Property Dal As TransferDal


    Function GetKundennummer() As String
End Interface