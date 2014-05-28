Option Strict Off


''' -----------------------------------------------------------------------------
''' Project	 : Common
''' Class	 : Common.Helper.QueryDataHelper
''' 
''' -----------------------------------------------------------------------------
''' <summary>
''' Bietet Möglichkeit DataTables zu Gruppieren und Aggregation vorzunehmen
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[x1tbardenhag]	21.04.2005	Created
''' </history>
''' -----------------------------------------------------------------------------
Public Class QueryDataHelper

    Private Class FieldInfo
        Public RelationName As String
        Public FieldName As String      ' source table field name
        Public FieldAlias As String     ' destination table field name
        Public Aggregate As String
    End Class

    'Member
    '------------------
    Private ds As DataSet

    Private xm_FieldInfo As ArrayList, xm_FieldList As String
    Private xm_GroupByFieldInfo As ArrayList, xm_GroupByFieldList As String

    'Hinzugefügt
    Private m_trennzeichen As String
    Private Const TRENNZEICHEN As String = ";"

    'Konstruktoren
    '------------------
    Public Sub New(ByVal dataSet As DataSet, Optional ByVal trennzeichen As String = TRENNZEICHEN)
        ds = dataSet
        m_trennzeichen = trennzeichen
    End Sub

    Public Sub New(Optional ByVal trennzeichen As String = TRENNZEICHEN)
        Me.new(Nothing, trennzeichen)
    End Sub


    Private Sub ParseGroupByFieldList_(ByVal FieldList As String, Optional ByVal AllowRelation As Boolean = False)
        '
        ' Parses FieldList into FieldInfo objects and then adds them to the m_FieldInfo private member
        '
        ' FieldList syntax: [relationname.]fieldname[ alias],...
        '
        If xm_GroupByFieldList = FieldList Then Exit Sub
        xm_GroupByFieldInfo = New ArrayList()
        xm_GroupByFieldList = FieldList
        Dim Field As FieldInfo, FieldParts() As String, Fields() As String = FieldList.Split(",")
        Dim I As Integer
        For I = 0 To Fields.Length - 1
            Field = New FieldInfo()
            '
            ' Parse FieldAlias
            '
            FieldParts = Fields(I).Trim().Split(", ")
            Select Case FieldParts.Length
                Case 1
                    ' To be set at the end of the loop
                Case 2
                    Field.FieldAlias = FieldParts(1)
                Case Else
                    Throw New ArgumentException("Too many spaces in field definition: '" & Fields(I) & "'.")
            End Select
            '
            ' Parse FieldName and RelationName
            '
            FieldParts = FieldParts(0).Split(".")
            Select Case FieldParts.Length
                Case 1
                    Field.FieldName = FieldParts(0)
                Case 2
                    If Not AllowRelation Then _
                        Throw New ArgumentException("Relation specifiers not allowed in field list: '" & Fields(I) & "'.")
                    Field.RelationName = FieldParts(0).Trim()
                    Field.FieldName = FieldParts(1).Trim()
                Case Else
                    Throw New ArgumentException("Invalid field definition: '" & Fields(I) & "'.")
            End Select
            If Field.FieldAlias = "" Then Field.FieldAlias = Field.FieldName
            xm_GroupByFieldInfo.Add(Field)
        Next
    End Sub


    Private Sub ParseFieldList_(ByVal FieldList As String)
        '
        ' Parses FieldList into FieldInfo objects and then adds them to the GroupByFieldInfo private member
        '
        ' FieldList syntax: fieldname[ alias]|operatorname(fieldname)[ alias],...
        '
        ' Supported Operators: count,sum,max,min,first,last
        '
        If xm_FieldList = FieldList Then Exit Sub

        xm_FieldInfo = New ArrayList()
        Dim Field As FieldInfo, FieldParts() As String, Fields() As String = FieldList.Split(",")
        Dim I As Integer
        For I = 0 To Fields.Length - 1
            Field = New FieldInfo()
            '
            ' Parse FieldAlias
            '
            FieldParts = Fields(I).Trim().Split(", ")
            Select Case FieldParts.Length
                Case 1
                    ' To be set at the end of the loop
                Case 2
                    Field.FieldAlias = FieldParts(1)
                Case Else
                    Throw New ArgumentException("Too many spaces in field definition: '" & Fields(I) & "'.")
            End Select
            '
            ' Parse FieldName and Aggregate
            '
            FieldParts = FieldParts(0).Split("(")
            Select Case FieldParts.Length
                Case 1
                    Field.FieldName = FieldParts(0)
                Case 2
                    Field.Aggregate = FieldParts(0).Trim().ToLower() ' You will do a case-sensitive comparison later.
                    Field.FieldName = FieldParts(1).Trim(" "c, ")"c)
                Case Else
                    Throw New ArgumentException("Invalid field definition: '" & Fields(I) & "'.")
            End Select
            If Field.FieldAlias = "" Then
                If Field.Aggregate = "" Then
                    Field.FieldAlias = Field.FieldName
                Else
                    Field.FieldAlias = Field.Aggregate & "Of" & Field.FieldName
                End If
            End If
            xm_FieldInfo.Add(Field)
        Next
        xm_FieldList = FieldList
    End Sub


    Private Function CreateGroupByTable(ByVal TableName As String, _
                                       ByVal SourceTable As DataTable, _
                                       ByVal FieldList As String) As DataTable
        '
        ' Creates a table based on aggregates of fields of another table
        '
        ' RowFilter affects rows before the GroupBy operation. No HAVING-type support
        ' although this can be emulated by later filtering of the resultant table.
        '
        ' FieldList syntax: fieldname[ alias]|aggregatefunction(fieldname)[ alias], ...
        '
        If FieldList = "" Then
            Throw New ArgumentException("You must specify at least one field in the field list.")
            ' Return CreateTable(TableName, SourceTable)
        Else
            Dim dt As New DataTable(TableName)
            ParseFieldList_(FieldList)

            Dim Field As FieldInfo, dc As DataColumn
            For Each Field In xm_FieldInfo
                dc = SourceTable.Columns(Field.FieldName)
                If Field.Aggregate = "" Then
                    dt.Columns.Add(Field.FieldAlias, dc.DataType, dc.Expression)
                ElseIf Field.Aggregate = "list" Then
                    dt.Columns.Add(Field.FieldAlias, Type.GetType("System.String"))
                ElseIf Field.Aggregate = "count" Then
                    dt.Columns.Add(Field.FieldAlias, Type.GetType("System.Decimal"))
                Else
                    dt.Columns.Add(Field.FieldAlias, dc.DataType)
                End If
            Next
            If Not ds Is Nothing Then ds.Tables.Add(dt)
            Return dt
        End If
    End Function


    Private Sub InsertGroupByInto(ByVal DestTable As DataTable, _
                             ByVal SourceTable As DataTable, _
                             ByVal FieldList As String, _
                             Optional ByVal RowFilter As String = "", _
                             Optional ByVal GroupBy As String = "")
        '
        ' Copies the selected rows and columns from SourceTable and inserts them into DestTable
        ' FieldList has same format as CreateGroupByTable
        '
        ParseFieldList_(FieldList)  ' parse field list
        ParseGroupByFieldList_(GroupBy)           ' parse field names to Group By into an arraylist
        Dim Field As FieldInfo
        Dim Rows() As DataRow = SourceTable.Select(RowFilter, FieldInfoArrayToNameList(xm_GroupByFieldInfo))
        Dim SourceRow As DataRow
        Dim LastSourceRow As DataRow
        Dim SameRow As Boolean
        Dim I As Integer
        Dim DestRow As DataRow, RowCount As Integer
        '
        ' Initialize Grand total row
        '
        DestRow = DestTable.NewRow()
        '
        ' Process source table rows
        '
        For Each SourceRow In Rows
            '
            ' Determine whether we've hit a control break
            '
            SameRow = False
            If Not (LastSourceRow Is Nothing) Then
                SameRow = True
                For I = 0 To xm_GroupByFieldInfo.Count - 1 ' fields to Group By
                    Field = xm_GroupByFieldInfo(I)
                    If ColumnEqual(LastSourceRow(Field.FieldName), SourceRow(Field.FieldName)) = False Then
                        SameRow = False
                        Exit For
                    End If
                Next I
                '
                ' Add previous totals to the destination table
                '
                If Not SameRow Then
                    DestTable.Rows.Add(DestRow)
                End If
            End If
            '
            ' create new destination rows
            '
            If Not SameRow Then
                DestRow = DestTable.NewRow()
                RowCount = 0
            End If

            RowCount += 1
            For Each Field In xm_FieldInfo
                Select Case Field.Aggregate  ' this test is case-sensitive - made lower-case by Build_GroupByFiledInfo
                    Case ""    ' implicit Last
                        DestRow(Field.FieldAlias) = SourceRow(Field.FieldName)
                    Case "last"
                        DestRow(Field.FieldAlias) = SourceRow(Field.FieldName)
                    Case "first"
                        If RowCount = 1 Then DestRow(Field.FieldAlias) = SourceRow(Field.FieldName)
                    Case "count"
                        DestRow(Field.FieldAlias) = RowCount
                    Case "sum"
                        DestRow(Field.FieldAlias) = Add(DestRow(Field.FieldAlias), SourceRow(Field.FieldName))
                        'Hinzugefügt -----
                    Case "list"
                        DestRow(Field.FieldAlias) = List(DestRow(Field.FieldAlias), SourceRow(Field.FieldName))
                        'end -------------
                    Case "max"
                        DestRow(Field.FieldAlias) = Max(DestRow(Field.FieldAlias), SourceRow(Field.FieldName))
                    Case "min"
                        If RowCount = 1 Then
                            DestRow(Field.FieldAlias) = SourceRow(Field.FieldName)  ' so we get by initial NULL
                        Else
                            DestRow(Field.FieldAlias) = Min(DestRow(Field.FieldAlias), SourceRow(Field.FieldName))
                        End If
                End Select
            Next
            LastSourceRow = SourceRow
        Next
        If Rows.Length > 0 Then
            '
            ' Add row
            '
            DestTable.Rows.Add(DestRow)
        End If
    End Sub

    Private Function LocateFieldInfoByName(ByVal FieldList As ArrayList, ByVal Name As String) As FieldInfo
        '
        ' Looks up a FieldInfo record based on FieldName
        '
        Dim Field As FieldInfo
        For Each Field In FieldList
            If Field.FieldName = Name Then Return Field
        Next
    End Function

    Private Function ColumnEqual(ByVal A As Object, ByVal B As Object) As Boolean
        '
        ' Compares two values to determine if they are equal. Also compares DBNULL.Value.
        '
        ' NOTE: If your DataTable contains object fields, you must extend this
        ' function to handle them in a meaningful way if you intend to group on them.
        '
        If A Is DBNull.Value And B Is DBNull.Value Then Return True ' Both are DBNull.Value.
        If A Is DBNull.Value Or B Is DBNull.Value Then Return False ' Only one is DbNull.Value.
        Return A = B                                                ' Value type standard comparison
    End Function

    Private Function Min(ByVal A As Object, ByVal B As Object) As Object
        '
        ' Returns MIN of two values. DBNull is less than all others.
        '
        If A Is DBNull.Value Or B Is DBNull.Value Then Return DBNull.Value
        If A < B Then Return A Else Return B
    End Function

    Private Function Max(ByVal A As Object, ByVal B As Object) As Object
        '
        ' Returns Max of two values. DBNull is less than all others.
        '
        If A Is DBNull.Value Then Return B
        If B Is DBNull.Value Then Return A
        If A > B Then Return A Else Return B
    End Function

    Private Function Add(ByVal A As Object, ByVal B As Object) As Object
        '
        ' Adds two values. If one is DBNull, returns the other.
        '
        If A Is DBNull.Value Then Return B
        If B Is DBNull.Value Then Return A
        Return A + B
    End Function


    Private Function List(ByVal A As Object, ByVal B As Object) As Object
        '
        ' Hinzugefügt
        ' Konkateniert die Felder
        '
        If A Is DBNull.Value Then Return B.ToString()
        If B Is DBNull.Value Then Return A.ToString()
        Return A.ToString() + m_trennzeichen + B.ToString()
    End Function


    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Selektiert, gruppiert und aggregiert ein DataTable und
    ''' gibt neuen DataTable mit Ergebnissen zurück.
    ''' Funktioniert ähnlich wie ein SQL-Statement.
    ''' </summary>
    ''' <param name="TableName">Namen des neuen DataTables</param>
    ''' <param name="SourceTable">Das zu bearbeitende DataTable</param>
    ''' <param name="FieldList">Feldlist der auszugebenden Spalten </param>
    ''' <param name="RowFilter"></param>
    ''' <param name="GroupBy">Felder nach den gruppiert werden soll.</param>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[x1tbardenhag]	21.04.2005	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Function SelectGroupByInto(ByVal TableName As String, _
                                  ByVal SourceTable As DataTable, _
                                  ByVal FieldList As String, _
                                  Optional ByVal RowFilter As String = "", _
                                  Optional ByVal GroupBy As String = "") As DataTable
        '
        ' Selects data from one DataTable to another and performs various aggregate functions
        ' along the way. See InsertGroupByInto and ParseGroupByFieldList for supported aggregate functions.
        '
        Dim dt As DataTable = CreateGroupByTable(TableName, SourceTable, FieldList)
        InsertGroupByInto(dt, SourceTable, FieldList, RowFilter, GroupBy)
        Return dt
    End Function

    Private Function FieldInfoArrayToNameList(ByVal arr As ArrayList) As String
        Dim a As Object
        Dim res As String = String.Empty
        For Each a In arr
            If res = String.Empty Then
                res += CType(a, FieldInfo).FieldName
            Else
                res += ", " + CType(a, FieldInfo).FieldName
            End If
        Next
        Return res
    End Function

End Class

' ************************************************
' $History: QueryDataHelper.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 29.04.08   Time: 12:27
' Updated in $/CKAG/Applications/appvfs/Lib
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 17:43
' Created in $/CKAG/Applications/appvfs/Lib
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 18.07.07   Time: 16:20
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Lib
' ITA: 1140
' 
' *****************  Version 1  *****************
' User: Uha          Date: 3.05.07    Time: 19:07
' Created in $/CKG/Applications/AppVFS/AppVFSWeb/Lib
' Änderungen aus StartApplication vom 02.05.2007 Mittags übernommen
' 
' ************************************************
