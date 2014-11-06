Imports System.Collections

Public Class EasyResult

    Private numOfHits As Long
    Private numOfFields As Long
    Private hitLst As Data.DataTable
    Private hitTableHeader As List(Of EasyResultField)

    Public Sub New()
        hitLst = New Data.DataTable()
        hitTableHeader = New List(Of EasyResultField)
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

    Public Property hitTblHeader() As List(Of EasyResultField)
        Get
            Return hitTableHeader
        End Get
        Set(ByVal Value As List(Of EasyResultField))
            hitTableHeader = Value
        End Set
    End Property

    Public Function getHitTable() As Data.DataTable
        Return hitLst
    End Function

    Public Sub clear()
        hitLst = New Data.DataTable()
    End Sub

    Public Sub addHitTableHeader(ByVal header As String)

        header = header.Replace("^", "").ToUpper

        'Position der Ergebnisspalten bestimmen
        For Each field As EasyResultField In hitTableHeader
            Dim sArray() As String
            sArray = header.Split(",")
            If sArray.Length > 0 Then
                For i As Integer = 0 To sArray.Length - 1
                    If field.Name.ToUpper = sArray(i).ToUpper Then
                        field.Index = CType(i + 1, String)
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
        End With

        For Each field As EasyResultField In hitTableHeader
            hitLst.Columns.Add(field.Name, GetType(String))
        Next

    End Sub

    Public Sub addHitTableRow(ByVal row As String, ByVal strLocFound As String, ByVal strArcFound As String, ByVal doc_id As String, ByVal doc_version As String)
        Dim nRow As Data.DataRow
        Dim index As Integer
        Dim value As String
        Dim found As Boolean
        Dim i As Integer

        row = row.Replace("^", "")
        row = row & ","
        nRow = hitLst.NewRow()
        nRow("DOC_Location") = strLocFound
        nRow("DOC_Archive") = strArcFound
        nRow("DOC_ID") = doc_id
        nRow("DOC_VERSION") = doc_version
        nRow("Bilder") = Nothing

        For Each field As EasyResultField In hitTableHeader
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

