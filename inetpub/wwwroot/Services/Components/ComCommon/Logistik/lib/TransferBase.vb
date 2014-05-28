Module TransferBase


    Private mTimeRange As DataTable
    Public ReadOnly Property TimeRange() As DataTable
        Get

            mTimeRange = New DataTable

            mTimeRange.Columns.Add("ID", GetType(System.String))
            mTimeRange.Columns.Add("Range", GetType(System.String))

            Dim nRow As DataRow

            nRow = mTimeRange.NewRow
            nRow(0) = "0-0"
            nRow(1) = "keine Wunschzeit"
            mTimeRange.Rows.Add(nRow)

            nRow = mTimeRange.NewRow
            nRow(0) = "080000-120000"
            nRow(1) = "Vormittags"
            mTimeRange.Rows.Add(nRow)

            nRow = mTimeRange.NewRow
            nRow(0) = "120000-180000"
            nRow(1) = "Nachmittags"
            mTimeRange.Rows.Add(nRow)

            For i = 80000 To 180000 Step 20000

                nRow = mTimeRange.NewRow

                nRow(0) = i.ToString.PadLeft(6, "0"c) & "-" & (i + 20000).ToString.PadLeft(6, "0"c)
                nRow(1) = "von " & _
                          Left(i.ToString.PadLeft(6, "0"c), 2) & ":00" & _
                          " bis " & Left((i + 20000).ToString.PadLeft(6, "0"c), 2) & ":00" & _
                          " Uhr"

                mTimeRange.Rows.Add(nRow)

            Next


            Return mTimeRange
        End Get

    End Property


End Module
