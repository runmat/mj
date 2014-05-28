Namespace Common
    ''' <summary>
    ''' Stellt eine SAP-Fehlermeldung über das SUBRC-System dar
    ''' </summary>
    Public Class SapError
        Implements ISapError

        Private strErrorCode As String
        Private strErrorMessage As String
        Private bErrorOccured As Boolean = False

        ReadOnly Property ErrorCode As String Implements ISapError.ErrorCode
            Get
                Return strErrorCode
            End Get
        End Property

        ReadOnly Property ErrorMessage As String Implements ISapError.ErrorMessage
            Get
                Return strErrorMessage
            End Get
        End Property

        ReadOnly Property ErrorOccured As Boolean Implements ISapError.ErrorOccured
            Get
                Return bErrorOccured
            End Get
        End Property

        Public Sub ClearError() Implements ISapError.ClearError
            bErrorOccured = False
        End Sub

        Public Sub RaiseError(errorcode As String, message As String) Implements ISapError.RaiseError
            bErrorOccured = True

            strErrorCode = errorcode
            strErrorMessage = message
        End Sub

        Public Function GetFormatedErrorMessage() As String
            Return ErrorMessage + " (Fehlercode " + ErrorCode + ")"
        End Function
    End Class

End Namespace
