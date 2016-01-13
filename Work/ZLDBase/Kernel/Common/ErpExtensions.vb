Imports System.Globalization
Imports System.Runtime.CompilerServices
Imports ERPConnect

Module ErpExtensions

    <Extension()> _
    Public Function ToADOTableLocaleDe(rfct As RFCTable) As DataTable
        Dim tmpTable As DataTable = rfct.ToADOTable()
        tmpTable.Locale = New CultureInfo("de-DE")
        Return tmpTable
    End Function

End Module
