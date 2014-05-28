Public Class CWSHalter
    Inherits TranslatedUserControl
    Implements ICollapsibleWizardStep

    Protected ReadOnly Property TransferPage As ITransferPage
        Get
            Return DirectCast(Page, ITransferPage)
        End Get
    End Property

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)

        If HalterAddress.MustInitDropdowns Then
            HalterAddress.InitDropdowns(TransferPage.Transfer.Laender.DefaultView, TimeRange.DefaultView, TransferPage.Transfer.Transporttyp.DefaultView)
        End If
    End Sub

    Public Sub Save() Implements ICollapsibleWizardStep.Save
        TransferPage.Dal.HalterAdresse = HalterAddress.Adresse
    End Sub

    Public Sub Validate() Implements ICollapsibleWizardStep.Validate
        Page.Validate("CWSHalter")
    End Sub

    Public Event WizardStepError(sender As Object, e As ErrorEventArgs) Implements ICollapsibleWizardStep.WizardStepError
End Class