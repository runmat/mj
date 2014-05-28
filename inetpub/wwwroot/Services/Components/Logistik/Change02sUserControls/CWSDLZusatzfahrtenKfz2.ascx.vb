Option Strict On
Option Explicit On

Public Class CWSDLZusatzfahrtenKfz2
    Inherits TranslatedUserControl
    Implements ICollapsibleWizardStep

    Private _fzg As Fahrzeug

    Protected ReadOnly Property TransferPage As ITransferPage
        Get
            Return DirectCast(Me.Page, ITransferPage)
        End Get
    End Property

    Protected Overrides Sub OnInit(e As System.EventArgs)
        MyBase.OnInit(e)

        AddHandler Me.TransferPage.Dal.PropertyChanged, AddressOf Me.OnDalPropertyChanged
        Me.AddFzgPropertyChanged(Me.TransferPage.Dal.Fzg2)
    End Sub

    Protected Overrides Sub OnLoad(e As System.EventArgs)
        MyBase.OnLoad(e)

        If Me.TransferPage.Dal.Fzg2 IsNot Nothing Then
            Dim fzg As Fahrzeug = Me.TransferPage.Dal.Fzg2

            If fzg IsNot Nothing Then
                Me.ZusatzDienstleistungen.SetFahrten(fzg.Zusatzfahrten)
            End If
        End If
    End Sub

    Protected Overrides Sub OnUnload(e As System.EventArgs)
        Me.RemoveFzgPropertyChanged()
        RemoveHandler Me.TransferPage.Dal.PropertyChanged, AddressOf Me.OnDalPropertyChanged

        MyBase.OnUnload(e)
    End Sub

    Private Sub OnDalPropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs)
        If e.PropertyName.Equals("Fzg2", StringComparison.Ordinal) Then
            Me.RemoveFzgPropertyChanged()
            Me.AddFzgPropertyChanged(Me.TransferPage.Dal.Fzg2)
        End If
    End Sub

    Private Sub AddFzgPropertyChanged(newFzg As Fahrzeug)
        Me._fzg = newFzg

        If Me._fzg IsNot Nothing Then
            AddHandler Me._fzg.PropertyChanged, AddressOf Me.OnFzgPropertyChanged
        End If
    End Sub

    Private Sub RemoveFzgPropertyChanged()
        If Me._fzg IsNot Nothing Then
            RemoveHandler Me._fzg.PropertyChanged, AddressOf Me.OnFzgPropertyChanged
            Me._fzg = Nothing
        End If
    End Sub

    Private Sub OnFzgPropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs)
        If e.PropertyName.Equals("Zusatzfahrten", StringComparison.Ordinal) Then
            Dim fzg As Fahrzeug = Me.TransferPage.Dal.Fzg2
            Me.ZusatzDienstleistungen.SetFahrten(fzg.Zusatzfahrten)
        End If
    End Sub

    Protected Sub OnNeedsServices(sender As Object, e As NeedsServicesEventArgs)
        e.Dienstleistungen = Me.TransferPage.Dal.GetDienstleistungen(e.Fahrt.TransporttypCode)
    End Sub

    Public Sub Save() Implements ICollapsibleWizardStep.Save

    End Sub

    Public Sub Validate() Implements ICollapsibleWizardStep.Validate

    End Sub

    Public Event WizardStepError(sender As Object, e As ErrorEventArgs) Implements ICollapsibleWizardStep.WizardStepError
End Class