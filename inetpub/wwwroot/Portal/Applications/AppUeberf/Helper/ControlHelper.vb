Imports System.Text.RegularExpressions

Namespace Helper

    Public Class ControlHelper

        Public Shared Function CheckMandatoryField(ByVal ctrl As TextBox, ByVal error_msg As String, ByVal appender As System.Text.StringBuilder) As Boolean
            If ctrl.Text.Trim = String.Empty Then
                appender.Append(error_msg + "<br>")
                Return False
            Else
                Return True
            End If
        End Function

        Public Shared Function CheckPostcode(ByVal ctrl As TextBox, ByVal appender As System.Text.StringBuilder) As Boolean

            'Prüfen, ob PLZ fünfstellige Zahl
            If Not Regex.IsMatch(ctrl.Text, "^\d{5}$") Then
                appender.Append("Bitte geben Sie eine 5-stellige Postleitzahl an." + "<br>")
                Return False
            Else
                Return True
            End If
        End Function


    End Class

End Namespace