Public NotInheritable Class ErrorEventArgs
    Inherits EventArgs
    Private ReadOnly _errorMessage As String

    Public Sub New(errorMessage As String)
        Me._errorMessage = errorMessage
    End Sub

    Public ReadOnly Property ErrorMessage As String
        Get
            Return Me._errorMessage
        End Get
    End Property
End Class

Public Interface ICollapsibleWizardStep
    Sub Validate()
    Sub Save()

    Event WizardStepError As EventHandler(Of ErrorEventArgs)
End Interface