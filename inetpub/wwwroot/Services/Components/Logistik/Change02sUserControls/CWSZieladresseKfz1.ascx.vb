Option Strict On
Option Explicit On

Public Class CWSZieladresseKfz1
    Inherits TranslatedUserControl
    Implements ICollapsibleWizardStep

    Protected ReadOnly Property TransferPage As ITransferPage
        Get
            Return DirectCast(Page, ITransferPage)
        End Get
    End Property

    Protected Overrides Sub OnInit(e As EventArgs)
        MyBase.OnInit(e)

        AddHandler TransferPage.Dal.PropertyChanged, AddressOf OnDalPropertyChanged
    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)

        If TransportAddress.MustInitDropdowns Then
            TransportAddress.InitDropdowns(TransferPage.Transfer.Laender.DefaultView, TimeRange.DefaultView, TransferPage.Transfer.Transporttyp.DefaultView)
        End If

        If Not IsPostBack AndAlso Not TransferPage.Dal.Fzg1 Is Nothing AndAlso Not TransferPage.Dal.Fzg1.Zielfahrt Is Nothing Then
            TransportAddress.Fahrt = TransferPage.Dal.Fzg1.Zielfahrt
        End If
    End Sub

    Protected Overrides Sub OnUnload(e As EventArgs)
        RemoveHandler TransferPage.Dal.PropertyChanged, AddressOf OnDalPropertyChanged

        MyBase.OnUnload(e)
    End Sub

    Private Sub OnDalPropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs)
        If e.PropertyName.Equals("Fzg1", StringComparison.Ordinal) Then
            TransportAddress.ShowVehicleDetails(TransferPage.Dal.Fzg1)
        End If
    End Sub

    Protected Sub OnValidatePostcode(sender As Object, e As ValidatePostcodeEventArgs)
        Dim ct As Transfer = TransferPage.Transfer
        Dim lnPlz As Integer = CInt(ct.Laender.Select("Land1='" & e.Country & "'")(0)("Lnplz"))

        If lnPlz > 0 Then
            If Not lnPlz = e.Postcode.Length Then
                e.IsValid = False
            End If
        End If
    End Sub

    Public Sub Save() Implements ICollapsibleWizardStep.Save
        Dim fahrt As Fahrt = TransportAddress.Fahrt

        For Each dienstleistung As Dienstleistung In TransferPage.Dal.GetStandarddienstleistungen(fahrt.TransporttypCode)
            fahrt.Dienstleistungen.Add(dienstleistung)
        Next

        TransferPage.Dal.Fzg1.Zielfahrt = fahrt
    End Sub

    Public Sub Validate() Implements ICollapsibleWizardStep.Validate
        Page.Validate("CWSZieladresseKfz1")
    End Sub

    Public Event WizardStepError(sender As Object, e As ErrorEventArgs) Implements ICollapsibleWizardStep.WizardStepError
End Class