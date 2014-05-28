Option Explicit On
Option Strict On

Public Class CWSZusatzfahrtenKfz2
    Inherits TranslatedUserControl
    Implements ICollapsibleWizardStep

    Protected ReadOnly Property TransferPage As ITransferPage
        Get
            Return DirectCast(Me.Page, ITransferPage)
        End Get
    End Property

    Protected Overrides Sub OnInit(e As System.EventArgs)
        MyBase.OnInit(e)

        AddHandler Me.TransferPage.Dal.PropertyChanged, AddressOf Me.OnDalPropertyChanged
    End Sub

    Protected Overrides Sub OnLoad(e As System.EventArgs)
        MyBase.OnLoad(e)

        If Me.Zusatzfahrten.MustInitDropdowns Then
            Me.Zusatzfahrten.InitDropdowns(Me.TransferPage.Transfer.Laender.DefaultView, TimeRange.DefaultView, Me.TransferPage.Transfer.Transporttyp.DefaultView)
        End If
    End Sub

    Protected Overrides Sub OnUnload(e As System.EventArgs)
        RemoveHandler Me.TransferPage.Dal.PropertyChanged, AddressOf Me.OnDalPropertyChanged

        MyBase.OnUnload(e)
    End Sub

    Private Sub OnDalPropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs)
        If e.PropertyName.Equals("Fzg2", StringComparison.Ordinal) Then
            Me.Zusatzfahrten.ShowVehicleDetails(Me.TransferPage.Dal.Fzg2)
        End If
    End Sub

    Protected Sub OnServerValidate(sender As Object, e As ServerValidateEventArgs)
        Dim isDirty As Boolean = CBool(e.Value)
        e.IsValid = Not isDirty
    End Sub

    Public Sub Save() Implements ICollapsibleWizardStep.Save
        Me.TransferPage.Dal.Fzg2.Zusatzfahrten.Clear()

        For Each fahrt As Fahrt In Me.Zusatzfahrten.Fahrten
            For Each dienstleistung As Dienstleistung In Me.TransferPage.Dal.GetStandarddienstleistungen(fahrt.TransporttypCode)
                fahrt.Dienstleistungen.Add(dienstleistung)
            Next

            Me.TransferPage.Dal.Fzg2.Zusatzfahrten.Add(fahrt)
        Next
    End Sub

    Public Sub Validate() Implements ICollapsibleWizardStep.Validate
        Me.Page.Validate("CWSZusatzfahrtenKfz2Extra")
    End Sub

    Public Event WizardStepError(sender As Object, e As ErrorEventArgs) Implements ICollapsibleWizardStep.WizardStepError
End Class