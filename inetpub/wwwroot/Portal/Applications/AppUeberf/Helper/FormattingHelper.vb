Namespace Helper

    Public Class FormattingHelper

        '----------
        'Liefert die Kundennummer zehnstellig mit führenden Nullen
        '----------
        Public Shared Function FormatKundennummer(ByVal kunnr As String) As String
            Return Right("0000000000" & kunnr, 10)
        End Function

    End Class

End Namespace