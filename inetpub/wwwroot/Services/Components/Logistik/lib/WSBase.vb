
Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Common


Public MustInherit Class WSBase
    Inherits TranslatedUserControl
    Implements IWizardStep

    Private Shared ReadOnly EventCompleted As New Object()
    Private Shared ReadOnly EventNavigateBack As New Object()

    Protected cwcSteps As CKG.Components.Controls.CollapsibleWizardControl
    Protected upValidation As UpdatePanel
    Protected lblErrorStamm As Label

    Public Custom Event Completed As EventHandler Implements IWizardStep.Completed
        AddHandler(value As EventHandler)
            Me.Events.AddHandler(WSBase.EventCompleted, value)
        End AddHandler
        RemoveHandler(value As EventHandler)
            Me.Events.RemoveHandler(WSBase.EventCompleted, value)
        End RemoveHandler
        RaiseEvent(sender As Object, e As EventArgs)
            Dim eh As EventHandler = DirectCast(Me.Events(WSBase.EventCompleted), EventHandler)

            If eh IsNot Nothing Then
                eh(sender, e)
            End If
        End RaiseEvent
    End Event

    Public Custom Event NavigateBack As EventHandler Implements IWizardStep.NavigateBack
        AddHandler(value As EventHandler)
            Me.Events.AddHandler(WSBase.EventNavigateBack, value)
        End AddHandler
        RemoveHandler(value As EventHandler)
            Me.Events.RemoveHandler(WSBase.EventNavigateBack, value)
        End RemoveHandler
        RaiseEvent(sender As Object, e As EventArgs)
            Dim eh As EventHandler = DirectCast(Me.Events(WSBase.EventNavigateBack), EventHandler)

            If eh IsNot Nothing Then
                eh(sender, e)
            End If
        End RaiseEvent
    End Event

    Protected Overrides Sub OnInit(e As System.EventArgs)
        MyBase.OnInit(e)

        Dim [step] As ICollapsibleWizardStep = TryCast(Me.cwcSteps.Content, ICollapsibleWizardStep)
        If [step] IsNot Nothing Then
            AddHandler [step].WizardStepError, AddressOf OnWizardStepError
        End If
    End Sub

    Public Function Save() As Boolean
        ' check for invalid steps.
        For i As Integer = 0 To Me.cwcSteps.StepCount - 1
            Dim cwStep As CKG.Components.Controls.CollapsibleWizardStep = Me.cwcSteps.Steps(i)
            Dim [step] As ICollapsibleWizardStep = TryCast(cwStep.Content, ICollapsibleWizardStep)

            If cwStep.Enabled AndAlso ([step] IsNot Nothing) Then
                [step].Validate()

                If Not Page.IsValid Then
                    Me.cwcSteps.SelectedIndex = i
                    Return False
                Else
                    ' store data in session.
                    [step].Save()
                End If
            End If
        Next

        Return True
    End Function

    Protected Overridable Sub OnWizardCompleted(sender As Object, e As EventArgs)
        If Not Save() Then Return

        Me.OnCompleted(EventArgs.Empty)
    End Sub

    Protected Overridable Sub OnCompleted(e As EventArgs)
        RaiseEvent Completed(Me, e)
    End Sub


    Public ReadOnly Property Transfer As Transfer
        Get
            Return DirectCast(Me.Session("Transfer"), Transfer)
        End Get
    End Property


    Protected Overridable Sub OnNextClick(sender As Object, e As EventArgs)
        Dim [step] As ICollapsibleWizardStep = TryCast(Me.cwcSteps.Content, ICollapsibleWizardStep)

        Dim e2 = e

        If [step] IsNot Nothing Then
            [step].Validate()

            If Page.IsValid Then
                If (Me.cwcSteps.SelectedIndex = 3) Then
                    Dim ct = Transfer

                    'ct.CheckFahrzeugStandort(Page)



                    Me.cwcSteps.NavigateForward(True)
                Else
                    Me.cwcSteps.NavigateForward(True)
                End If



            End If
        End If
    End Sub

    
    Protected Overridable Sub OnPreviousClick(sender As Object, e As EventArgs)
        If Me.cwcSteps.SelectedIndex > 0 AndAlso Not _
            Me.cwcSteps.NavigateBackward(True) Then
            RaiseEvent NavigateBack(Me, EventArgs.Empty)
        End If
    End Sub

    Protected Overrides Sub OnPreRender(e As System.EventArgs)
        Me.upValidation.Update()
        MyBase.OnPreRender(e)
    End Sub

    Protected Overridable Sub OnWizardStepError(sender As Object, e As ErrorEventArgs)
        Me.lblErrorStamm.Text = e.ErrorMessage
        Me.lblErrorStamm.Visible = True
    End Sub


End Class