Option Explicit On 
Option Strict On

Namespace DocumentGeneration

    Public Class DataTableMailMergeSource
        Implements Aspose.Words.Reporting.IMailMergeDataSource

        Private _dt As DataTable
        Private _onlyFirstRow As Boolean

        Private _index As Integer = -1

        Public Sub New(ByVal dt As DataTable, ByVal onlyFirstRow As Boolean)
            _dt = dt
            _onlyFirstRow = onlyFirstRow
        End Sub

        Public Function GetValue(ByVal fieldName As String, ByRef fieldValue As Object) As Boolean Implements Aspose.Words.Reporting.IMailMergeDataSource.GetValue
            If _dt.Columns.Contains(fieldName) Then
                fieldValue = _dt.Rows(_index)(fieldName)
                Return True
            Else
                fieldValue = Nothing
                Return False
            End If

        End Function

        Public Function MoveNext() As Boolean Implements Aspose.Words.Reporting.IMailMergeDataSource.MoveNext
            If (_onlyFirstRow AndAlso _index > 0) OrElse _dt.Rows.Count <= _index + 1 Then
                Return False
            Else
                _index += 1
                Return True
            End If
        End Function

        Public ReadOnly Property TableName() As String Implements Aspose.Words.Reporting.IMailMergeDataSource.TableName
            Get
                Return _dt.TableName
            End Get
        End Property

    End Class

End Namespace

' ************************************************
' $History: DataTableMailMergeSource.vb $
' 
' *****************  Version 1  *****************
' User: Rudolpho     Date: 24.02.10   Time: 17:59
' Created in $/CKAG2/KBS/DocumentGeneration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 3.04.08    Time: 16:42
' Created in $/CKAG/Base/Kernel/DocumentGeneration
' 
' *****************  Version 1  *****************
' User: Uha          Date: 15.05.07   Time: 11:04
' Created in $/CKG/Base/Base/Kernel/DocumentGeneration
' Änderungen aus StartApplication vom 11.05.2007 (Aspose-Tool,
' DataTableHelper.vb, Customer.vb)
' 
' ************************************************
