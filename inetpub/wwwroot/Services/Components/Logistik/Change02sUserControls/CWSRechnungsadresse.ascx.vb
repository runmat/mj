Option Explicit On
Option Strict On

Public Class CWSRechnungsadresse
    Inherits CWSRechnungBase

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)

        If Not IsPostBack AndAlso Not String.IsNullOrEmpty(TransferPage.Transfer.RE) Then
            paRechnungsadresse.SelectedAddress = TransferPage.Transfer.RE
            paRechnungsadresse.DisplayAddress = TransferPage.Transfer.Partner.Select(RowFilter & " AND KUNNR = '" & TransferPage.Transfer.RE & "'").FirstOrDefault()
            paRechnungsadresse.DataBind()
        End If
    End Sub

    Protected Overrides ReadOnly Property AddressControl As PartnerAddress
        Get
            Return paRechnungsadresse
        End Get
    End Property

    Protected Overrides ReadOnly Property RowFilter As String
        Get
            Return "PARVW = 'RE'"
        End Get
    End Property

    Protected Overrides ReadOnly Property ValidationGroup As String
        Get
            Return "CWSRechnungsadresse"
        End Get
    End Property

    Public Overrides Sub Save()
        TransferPage.Transfer.RE = paRechnungsadresse.SelectedAddress
    End Sub
End Class