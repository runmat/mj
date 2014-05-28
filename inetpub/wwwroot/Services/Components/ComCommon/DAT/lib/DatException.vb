Option Strict On
Option Explicit On

Namespace SilverDAT
    <Serializable()> _
    Friend Class DatException
        Inherits ApplicationException

        Private ReadOnly _errorCode As Integer

        Public Sub New(message As String, innerException As Exception, errorCode As Integer)
            MyBase.New(message, innerException)

            Me._errorCode = errorCode
        End Sub

        Public ReadOnly Property ErrorCode As Integer
            Get
                Return Me._errorCode
            End Get
        End Property

        Friend Shared Function FromFault(fault As ServiceModel.FaultException) As DatException
            Dim reason = fault.Reason.GetMatchingTranslation().Text
            Dim match = Regex.Match(reason, "\<statusCodeId\>(?<StatusCode>\d+)\</statusCodeId\>.*\<statusMessage\>(?<StatusMessage>.*)\</statusMessage\>")

            If match.Success Then
                Return New DatException(match.Groups("StatusMessage").Value, fault, Convert.ToInt32(match.Groups("StatusCode").Value))
            Else
                Return Nothing
            End If
        End Function
    End Class
End Namespace