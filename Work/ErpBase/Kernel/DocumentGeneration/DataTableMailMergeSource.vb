Option Explicit On 
Option Strict On

Namespace Kernel.DocumentGeneration

    Public Class DataTableMailMergeSource
        Implements Aspose.Words.Reporting.IMailMergeDataSource

        Private _dt As DataTable
        Private _onlyFirstRow As Boolean

        Private _index As Integer = -1

        Public Sub New(ByVal dt As DataTable, ByVal onlyFirstRow As Boolean)
            Me._dt = dt
            Me._onlyFirstRow = onlyFirstRow
        End Sub

        Public Function GetValue(ByVal fieldName As String, ByRef fieldValue As Object) As Boolean Implements Aspose.Words.Reporting.IMailMergeDataSource.GetValue
            If Me._dt.Columns.Contains(fieldName) Then
                fieldValue = Me._dt.Rows(Me._index)(fieldName)
                Return True
            Else
                fieldValue = Nothing
                Return False
            End If

        End Function

        Public Function MoveNext() As Boolean Implements Aspose.Words.Reporting.IMailMergeDataSource.MoveNext
            If (Me._onlyFirstRow AndAlso Me._index > 0) OrElse Me._dt.Rows.Count <= _index + 1 Then
                Return False
            Else
                _index += 1
                Return True
            End If
        End Function

        Public ReadOnly Property TableName() As String Implements Aspose.Words.Reporting.IMailMergeDataSource.TableName
            Get
                Return Me._dt.TableName
            End Get
        End Property

    End Class

End Namespace

' ************************************************
' $History: DataTableMailMergeSource.vb $
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
