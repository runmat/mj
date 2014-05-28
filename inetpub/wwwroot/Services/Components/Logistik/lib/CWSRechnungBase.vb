Option Strict On
Option Explicit On

Public MustInherit Class CWSRechnungBase
    Inherits TranslatedUserControl
    Implements ICollapsibleWizardStep

    Protected MustOverride ReadOnly Property AddressControl As PartnerAddress
    Protected MustOverride ReadOnly Property ValidationGroup As String
    Protected MustOverride ReadOnly Property RowFilter As String

    Public Custom Event WizardStepError As EventHandler(Of ErrorEventArgs) Implements ICollapsibleWizardStep.WizardStepError
        AddHandler(value As EventHandler(Of ErrorEventArgs))

        End AddHandler

        RemoveHandler(value As EventHandler(Of ErrorEventArgs))

        End RemoveHandler

        RaiseEvent(sender As Object, e As ErrorEventArgs)

        End RaiseEvent
    End Event

    Protected ReadOnly Property TransferPage As ITransferPage
        Get
            Return DirectCast(Me.Page, ITransferPage)
        End Get
    End Property

    Protected Overrides Sub OnLoad(e As System.EventArgs)
        MyBase.OnLoad(e)

        If (Not Me.IsPostBack) Then
            Dim dv As DataView = Me.TransferPage.Transfer.Partner.DefaultView
            dv.RowFilter = Me.RowFilter
            Me.AddressControl.AddressListSource = dv
            Me.AddressControl.DisplayAddress = Me.TransferPage.Transfer.Partner.Select(Me.RowFilter & " AND DEFPA = 'X'").FirstOrDefault()
            Me.AddressControl.DataBind()
        End If
    End Sub

    Protected Sub OnSelectedAddressChanged(sender As Object, e As EventArgs)
        Me.AddressControl.DisplayAddress = Me.TransferPage.Transfer.Partner.Select(Me.RowFilter & " AND KUNNR = '" & Me.AddressControl.SelectedAddress & "'").FirstOrDefault()
        Me.AddressControl.DataBind()
    End Sub

    Public MustOverride Sub Save() Implements ICollapsibleWizardStep.Save

    Public Sub Validate() Implements ICollapsibleWizardStep.Validate
        Me.Page.Validate(Me.ValidationGroup)
    End Sub
End Class