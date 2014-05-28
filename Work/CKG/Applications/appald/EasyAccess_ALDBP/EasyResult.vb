Imports System.Collections

Public Class EasyResult

    Private numOfHits As Long
    Private numOfFields As Long
    Private hitLst As Data.DataTable
    Private hitTableHeader As ArrayList
    'Private hitTableSearch As String
    'Private resultFields As ArrayList

    Public Sub New()
        hitLst = New Data.DataTable()
        hitTableHeader = New ArrayList()
        'hitTableSearch = New ArrayList()
    End Sub

    Public Property hits() As String
        Get
            Return numOfHits
        End Get
        Set(ByVal Value As String)
            numOfHits = Value
        End Set
    End Property

    Public Property fields() As String
        Get
            Return numOfFields
        End Get
        Set(ByVal Value As String)
            numOfFields = Value
        End Set
    End Property

    Public Property hitList() As Data.DataTable
        Get
            Return hitLst
        End Get
        Set(ByVal Value As Data.DataTable)
            hitLst = Value
        End Set
    End Property

    Public Property hitTblHeader() As ArrayList
        Get
            Return hitTableHeader
        End Get
        Set(ByVal Value As ArrayList)
            hitTableHeader = Value
        End Set
    End Property

    'Public Property hitTblSearch() As String
    '    Get
    '        Return hitTableSearch
    '    End Get
    '    Set(ByVal Value As String)
    '        hitTableSearch = Value
    '    End Set
    'End Property

    'Public Property hitTableColumns() As ArrayList
    '    Get
    '        Return resultFields
    '    End Get
    '    Set(ByVal Value As ArrayList)
    '        resultFields = Value
    '    End Set
    'End Property

    Public Function getHitTable() As Data.DataTable
        Return hitLst
    End Function

    Public Sub clear()
        hitLst = New Data.DataTable()
    End Sub

    Public Sub addHitTableHeader(ByVal header As String)

        Dim index As Integer
        Dim i As Integer
        Dim j As Integer
        Dim pos As Integer
        Dim field As EasyResultField

        'header = hitTableSearch
        header = header.Replace("^", "").ToUpper

        For i = 0 To hitTableHeader.Count - 1     'Position der Ergebnisspalten bestimmen
            index = 0
            field = CType(hitTableHeader(i), EasyResultField)
            pos = header.IndexOf(field.Name)
            For j = 0 To pos
                If header.Chars(j) = "," Then
                    index += 1
                End If
            Next
            field.Index = CType(index + 1, String)
        Next

        With hitLst.Columns
            .Add("DOC_ID", System.Type.GetType("System.String"))
            .Add("DOC_VERSION", System.Type.GetType("System.String"))
            .Add("Bilder", System.Type.GetType("System.String"))
            .Add("Link", System.Type.GetType("System.String"))
            '壯克VE 12.10.2005 <>
            .Add("Vorschau", System.Type.GetType("System.String"))
            .Add("VorschauLinks", System.Type.GetType("System.String"))
            '壯克VE 12.10.2005 <>
        End With

        For j = 0 To hitTableHeader.Count - 1
            hitLst.Columns.Add(CType(hitTableHeader(j), EasyResultField).Name, System.Type.GetType("System.String"))
        Next
    End Sub

    Public Sub addHitTableRow(ByVal row As String, ByVal doc_id As String, ByVal doc_version As String)
        Dim nRow As Data.DataRow
        Dim index As Integer
        Dim field As EasyResultField
        Dim value As String
        Dim found As Boolean
        Dim i As Integer
        Dim j As Integer

        row = row.Replace("^", "")
        row = row & ","
        nRow = hitLst.NewRow()
        nRow("DOC_ID") = doc_id
        nRow("DOC_VERSION") = doc_version
        nRow("Bilder") = Nothing
        '壯克VE 12.10.2005 <>
        nRow("Vorschau") = Nothing
        nRow("VorschauLinks") = Nothing
        '壯克VE 12.10.2005 <>
        found = False
        For j = 0 To hitTableHeader.Count - 1
            field = CType(hitTableHeader(j), EasyResultField)
            index = 0
            found = False
            For i = 0 To row.Length - 1
                If row.Chars(i) = "," Then
                    index += 1
                End If
                If found = False Then
                    If (index = field.Index - 1) Then
                        If index = 0 Then
                            value = row.Substring(0, row.Length - i - 1)
                        Else
                            value = row.Substring(i + 1, row.Length - i - 1)
                        End If
                        value = value.Substring(0, value.IndexOf(","))
                        nRow(field.Name.ToUpper) = value
                        found = True
                    End If
                End If

            Next
        Next
        hitLst.Rows.Add(nRow)
        hitLst.AcceptChanges()
    End Sub
End Class

' ************************************************
' $History: EasyResult.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 29.04.09   Time: 14:58
' Updated in $/CKAG/EasyAccess_ALDBP/EasyAccess_ALDBP
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 17.04.08   Time: 15:21
' Created in $/CKAG/EasyAccess_ALDBP/EasyAccess_ALDBP
' 
' *****************  Version 3  *****************
' User: Uha          Date: 5.03.07    Time: 16:59
' Updated in $/CKG/Applications/AppALD/EasyAccess_ALDBP
' 
' ************************************************
