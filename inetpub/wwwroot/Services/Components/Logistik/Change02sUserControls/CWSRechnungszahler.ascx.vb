Option Explicit On
Option Strict On

Public Class CWSRechnungszahler
    Inherits CWSRechnungBase

    Protected Overrides ReadOnly Property AddressControl As PartnerAddress
        Get
            Return Me.paRechnungszahler
        End Get
    End Property

    Protected Overrides ReadOnly Property RowFilter As String
        Get
            Return "PARVW = 'RG'"
        End Get
    End Property

    Protected Overrides ReadOnly Property ValidationGroup As String
        Get
            Return "CWSRechnungszahler"
        End Get
    End Property

    Public Overrides Sub Save()
        Me.TransferPage.Transfer.RG = Me.paRechnungszahler.SelectedAddress
    End Sub
End Class