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
        Dim field As EasyResultField

        'header = hitTableSearch
        header = header.Replace("^", "").ToUpper

        For i = 0 To hitTableHeader.Count - 1     'Position der Ergebnisspalten bestimmen
            index = 0
            field = CType(hitTableHeader(i), EasyResultField)
            Dim sArray() As String
            sArray = header.Split(",")
            If sArray.Length > 0 Then
                For j = 0 To sArray.Length - 1
                    If field.Name.ToUpper = sArray(j).ToUpper Then
                        field.Index = CType(j + 1, String)
                        Exit For
                    End If

                Next
            End If

        Next

        With hitLst.Columns
            .Add("DOC_Location", System.Type.GetType("System.String"))
            .Add("DOC_Archive", System.Type.GetType("System.String"))
            .Add("DOC_ID", System.Type.GetType("System.String"))
            .Add("DOC_VERSION", System.Type.GetType("System.String"))
            .Add("Bilder", System.Type.GetType("System.String"))
            .Add("Link", System.Type.GetType("System.String"))
            '§§§JVE 12.10.2005 <>
            '.Add("Vorschau", System.Type.GetType("System.String"))
            '.Add("VorschauLinks", System.Type.GetType("System.String"))
            '§§§JVE 12.10.2005 <>
        End With

        For j = 0 To hitTableHeader.Count - 1
            hitLst.Columns.Add(CType(hitTableHeader(j), EasyResultField).Name, System.Type.GetType("System.String"))
        Next
    End Sub

    Public Sub addHitTableRow(ByVal row As String, ByVal strLocFound As String, ByVal strArcFound As String, ByVal doc_id As String, ByVal doc_version As String)
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
        nRow("DOC_Location") = strLocFound
        nRow("DOC_Archive") = strArcFound
        nRow("DOC_ID") = doc_id
        nRow("DOC_VERSION") = doc_version
        nRow("Bilder") = Nothing
        '§§§JVE 12.10.2005 <>
        'nRow("Vorschau") = Nothing
        'nRow("VorschauLinks") = Nothing
        '§§§JVE 12.10.2005 <>
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
                        '§§§ JVE 23.10.2006 TODO: Wenn nur 1 Spalte in der Trefferliste, nicht kommaseparierten String erwarten.
                        If value.Contains(",") Then
                            value = value.Substring(0, value.IndexOf(","))
                        End If

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
' *****************  Version 4  *****************
' User: Fassbenders  Date: 20.04.11   Time: 14:01
' Updated in $/CKAG/EasyAccess
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 26.10.09   Time: 16:07
' Updated in $/CKAG/EasyAccess
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 16.04.08   Time: 11:53
' Updated in $/CKAG/EasyAccess
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:09
' Created in $/CKAG/EasyAccess
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:03
' Created in $/CKAG/Components/ComArchive/Work/CKG/EasyAccess
' 
' *****************  Version 6  *****************
' User: Uha          Date: 1.03.07    Time: 16:55
' Updated in $/CKG/EasyAccess
' 
' ************************************************