Option Explicit On
Option Strict On

Imports CKG.Base.Kernel.Common

Public MustInherit Class CWSKfzBase
    Inherits TranslatedUserControl
    Implements ICollapsibleWizardStep

    Private Shared ReadOnly EventWizardStepError As New Object

    Public Custom Event WizardStepError As EventHandler(Of ErrorEventArgs) Implements ICollapsibleWizardStep.WizardStepError
        AddHandler(value As EventHandler(Of ErrorEventArgs))
            Me.Events.AddHandler(CWSKfzBase.EventWizardStepError, value)
        End AddHandler
        RemoveHandler(value As EventHandler(Of ErrorEventArgs))
            Me.Events.RemoveHandler(CWSKfzBase.EventWizardStepError, value)
        End RemoveHandler
        RaiseEvent(sender As Object, e As ErrorEventArgs)
            Dim eh As EventHandler(Of ErrorEventArgs) = DirectCast(Me.Events(CWSKfzBase.EventWizardStepError), EventHandler(Of ErrorEventArgs))

            If eh IsNot Nothing Then
                eh(sender, e)
            End If
        End RaiseEvent
    End Event

    Protected ReadOnly Property TransferPage As ITransferPage
        Get
            Return DirectCast(Me.Page, ITransferPage)
        End Get
    End Property

    Protected MustOverride ReadOnly Property FahrzeugNummer As String
    Protected MustOverride ReadOnly Property ValidationGroup As String
    Protected MustOverride ReadOnly Property KfzControl As KfzStammdaten

    Public Overridable Sub Save() Implements ICollapsibleWizardStep.Save
        Dim dal As TransferDal = Me.TransferPage.Dal
        Dim kfz As Fahrzeug = Me.KfzControl.Fahrzeug

        If String.IsNullOrEmpty(kfz.Fahrgestellnummer) Then
            kfz = Nothing
        End If

        If Me.FahrzeugNummer.Equals("1", StringComparison.Ordinal) Then
            dal.Fzg1 = kfz
        Else
            dal.Fzg2 = kfz
        End If
    End Sub

    Public Overridable Sub Validate() Implements ICollapsibleWizardStep.Validate
        Me.Page.Validate(Me.ValidationGroup)
    End Sub

    Protected Overridable Sub OnFillData(sender As Object, e As EventArgs)
        Dim kfz As Fahrzeug = Me.KfzControl.Fahrzeug

        If kfz.Fahrgestellnummer.Length + kfz.Kennzeichen.Length + kfz.Referenznummer.Length > 0 Then
            Dim ct As Transfer = Me.TransferPage.Transfer

            ct.SuchFahrzeugestellnummer = kfz.Fahrgestellnummer
            ct.Suchkennzeichen = kfz.Kennzeichen
            ct.SuchReferenz = kfz.Referenznummer
            ct.FillFahrzeuge(Me.TransferPage.CSKUser, Me.Page, Me.TransferPage.GetKundennummer())

            If ct.Status = 0 Then
                If ct.SuchFahrzeuge.Rows.Count > 0 Then
                    Dim row = ct.SuchFahrzeuge.Rows(0)

                    kfz.Fahrgestellnummer = row("CHASSIS_NUM").ToString
                    kfz.Kennzeichen = row("LICENSE_NUM").ToString
                    kfz.Referenznummer = row("LIZNR").ToString

                    Dim iGewicht As Integer

                    If IsNumeric(row("ZZZULGESGEW").ToString) Then
                        iGewicht = CInt(row("ZZZULGESGEW"))
                        Select Case (iGewicht <= 7500)
                            Case True
                                kfz.Klasse = "PKW"
                            Case Else
                                kfz.Klasse = "LKW"
                        End Select
                    End If

                    If row("ZZHERST_TEXT").ToString.Length + row("ZZHANDELSNAME").ToString.Length > 25 Then
                        kfz.Typ = row("ZZHANDELSNAME").ToString
                    Else
                        kfz.Typ = row("ZZHERST_TEXT").ToString + " " + row("ZZHANDELSNAME").ToString
                    End If

                    Me.KfzControl.Fahrzeug = kfz
                End If
            Else
                Dim err As New ErrorEventArgs(ct.Message)
                RaiseEvent WizardStepError(Me, err)
            End If
        Else
            Dim err As New ErrorEventArgs("Bitte geben Sie Suchkriterien an.")
            RaiseEvent WizardStepError(Me, err)
        End If
    End Sub

End Class