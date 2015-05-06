Imports KBSBase

Public Class Bestellung
    Inherits ErrorHandlingClass

    Private mBestellungen As DataTable
    Private mMyKasse As Kasse
    Private mBestellnummer As String

    Public ReadOnly Property Bestellungen() As DataTable
        Get
            Return mBestellungen
        End Get
    End Property

    Public ReadOnly Property Bestellnummer() As String
        Get
            Return mBestellnummer
        End Get
    End Property

    Public Sub New(ByRef Kasse As Kasse)
        mMyKasse = Kasse
        S.AP.Init("Z_FIL_PO_CREATE_001")
        mBestellungen = S.AP.GetImportTable("GT_WEB")
        checkForSavedOrders()
        Bestellungen.AcceptChanges()
    End Sub

    Public Sub CheckForSavedBestellungen()
        Bestellungen.Rows.Clear()
        checkForSavedOrders()
        Bestellungen.AcceptChanges()
    End Sub

    Public Sub SaveToSQLDB()
        mBestellungen.AcceptChanges()
        DeleteBestellungFromSQLDB()
        InsertIntoBestellungsSQLDB()
    End Sub

    Private Sub checkForSavedOrders()
        '----------------------------------------------------------------------
        'Methode:      checkForSavedOrders
        'Autor:         Julian Jung
        'Beschreibung:  prüft die SQL DB auf vorhandene Bestellungen
        'Erstellt am:   05.05.2009
        '----------------------------------------------------------------------

        Dim cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))

        Try
            cn.Open()
            Dim cmd As New SqlClient.SqlCommand("SELECT * FROM KBS_EFA_BESTELLUNGEN WHERE LGORT ='" & mMyKasse.Lagerort & "'", cn)
            Dim myDataReader As SqlClient.SqlDataReader = cmd.ExecuteReader
            Dim tmpRow As DataRow
            While myDataReader.Read
                tmpRow = Bestellungen.NewRow

                tmpRow("EKORG") = myDataReader.GetString(myDataReader.GetOrdinal("EKORG")) 'Einkaufsorganisation
                tmpRow("KOSTL") = myDataReader.GetString(myDataReader.GetOrdinal("KOSTL")) 'Kostenstelle
                tmpRow("WERKS") = myDataReader.GetString(myDataReader.GetOrdinal("WERKS")) 'Werk
                tmpRow("LIFNR") = myDataReader.GetString(myDataReader.GetOrdinal("LIFNR")) 'Kontonummer Lieferant
                tmpRow("MATNR") = myDataReader.GetString(myDataReader.GetOrdinal("MATNR")) 'Materialnummer
                tmpRow("BSTMG") = CInt(myDataReader.GetDecimal(myDataReader.GetOrdinal("BSTMG"))) 'Bestellmenge
                tmpRow("EAN11") = myDataReader.GetString(myDataReader.GetOrdinal("EAN11")) 'EAN
                tmpRow("LGORT") = myDataReader.GetString(myDataReader.GetOrdinal("LGORT")) 'Lagerort
                tmpRow("MAKTX") = myDataReader.GetString(myDataReader.GetOrdinal("MAKTX")) 'Materialbezeichnung
                tmpRow("NAME1") = myDataReader.GetString(myDataReader.GetOrdinal("NAME1")) 'Lieferantenname
                tmpRow("IDNLF") = myDataReader.GetString(myDataReader.GetOrdinal("IDNLF")) 'Materialnummer (Lieferant)
                tmpRow("RELIF") = myDataReader.GetString(myDataReader.GetOrdinal("RELIF")) 'Regellieferant
                tmpRow("ESOKZ") = myDataReader.GetString(myDataReader.GetOrdinal("ESOKZ")) 'Einkaufsinfosatz-Typ
                tmpRow("KBETR") = CDbl(myDataReader.GetDecimal(myDataReader.GetOrdinal("KBETR"))) 'Konditionsbetrag
                tmpRow("KONWA") = myDataReader.GetString(myDataReader.GetOrdinal("KONWA")) 'Konditionseinheit
                tmpRow("KPEIN") = myDataReader.GetInt32(myDataReader.GetOrdinal("KPEIN"))  'Konditionspreiseinheit
                tmpRow("BPRME") = myDataReader.GetString(myDataReader.GetOrdinal("BPRME")) 'Bestellpreismengeneinheit
                tmpRow("MEINS") = myDataReader.GetString(myDataReader.GetOrdinal("MEINS")) 'Basismengeneinheit
                tmpRow("UMRECH") = myDataReader.GetInt32(myDataReader.GetOrdinal("UMRECH")) 'Umrechnungsfaktor Bestellmenge <-> Lagermengeneinheit 
                tmpRow("MINBM") = CInt(myDataReader.GetDecimal(myDataReader.GetOrdinal("MINBM"))) 'Mindestbestellmenge
                tmpRow("MINBW") = CDbl(myDataReader.GetDecimal(myDataReader.GetOrdinal("MINBW"))) 'Mindestbestellwert

                Bestellungen.Rows.Add(tmpRow)
            End While
        Catch ex As Exception
            Throw
        Finally
            cn.Close()
        End Try
    End Sub

    Private Sub InsertIntoBestellungsSQLDB()
        '----------------------------------------------------------------------
        'Methode:       InsertIntoBestellungsDB
        'Autor:         Julian Jung
        'Beschreibung:  schreibt die Bestellung die SQL DB
        'Erstellt am:   27.04.2009
        '----------------------------------------------------------------------

        Dim cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Dim cmd As New SqlClient.SqlCommand

        Try
            cn.Open()
            cmd.Connection = cn
            cmd.CommandType = CommandType.Text
            Dim SqlQuery As String
            SqlQuery = "INSERT INTO KBS_EFA_BESTELLUNGEN (EKORG, KOSTL, WERKS, LIFNR, MATNR, BSTMG, EAN11, LGORT, MAKTX, NAME1, IDNLF, RELIF, ESOKZ, KBETR, KONWA, KPEIN, BPRME, MEINS, UMRECH, MINBM, MINBW) " & _
                "VALUES (@EKORG, @KOSTL, @WERKS, @LIFNR, @MATNR, @BSTMG, @EAN11, @LGORT, @MAKTX, @NAME1, @IDNLF, @RELIF, @ESOKZ, @KBETR, @KONWA, @KPEIN, @BPRME, @MEINS, @UMRECH, @MINBM, @MINBW);"
            With cmd
                .Parameters.Add("@EKORG", SqlDbType.NVarChar)
                .Parameters.Add("@KOSTL", SqlDbType.NVarChar)
                .Parameters.Add("@WERKS", SqlDbType.NVarChar)
                .Parameters.Add("@LIFNR", SqlDbType.NVarChar)
                .Parameters.Add("@MATNR", SqlDbType.NVarChar)
                .Parameters.Add("@BSTMG", SqlDbType.Decimal)
                .Parameters.Add("@EAN11", SqlDbType.NVarChar)
                .Parameters.Add("@LGORT", SqlDbType.NVarChar)
                .Parameters.Add("@MAKTX", SqlDbType.NVarChar)
                .Parameters.Add("@NAME1", SqlDbType.NVarChar)
                .Parameters.Add("@IDNLF", SqlDbType.NVarChar)
                .Parameters.Add("@RELIF", SqlDbType.NVarChar)
                .Parameters.Add("@ESOKZ", SqlDbType.NVarChar)
                .Parameters.Add("@KBETR", SqlDbType.Decimal)
                .Parameters.Add("@KONWA", SqlDbType.NVarChar)
                .Parameters.Add("@KPEIN", SqlDbType.Int)
                .Parameters.Add("@BPRME", SqlDbType.NVarChar)
                .Parameters.Add("@MEINS", SqlDbType.NVarChar)
                .Parameters.Add("@UMRECH", SqlDbType.Int)
                .Parameters.Add("@MINBM", SqlDbType.Decimal)
                .Parameters.Add("@MINBW", SqlDbType.Decimal)
            End With
            cmd.CommandText = SqlQuery
            For Each tmpRow As DataRow In Bestellungen.Rows
                With cmd
                    .Parameters("@EKORG").Value = tmpRow("EKORG")
                    .Parameters("@KOSTL").Value = tmpRow("KOSTL")
                    .Parameters("@WERKS").Value = tmpRow("WERKS")
                    .Parameters("@LIFNR").Value = tmpRow("LIFNR")
                    .Parameters("@MATNR").Value = tmpRow("MATNR")
                    .Parameters("@BSTMG").Value = CDec(tmpRow("BSTMG"))
                    .Parameters("@EAN11").Value = tmpRow("EAN11")
                    .Parameters("@LGORT").Value = mMyKasse.Lagerort
                    .Parameters("@MAKTX").Value = tmpRow("MAKTX")
                    .Parameters("@NAME1").Value = tmpRow("NAME1")
                    .Parameters("@IDNLF").Value = tmpRow("IDNLF")
                    .Parameters("@RELIF").Value = tmpRow("RELIF")
                    .Parameters("@ESOKZ").Value = tmpRow("ESOKZ")
                    .Parameters("@KBETR").Value = CDec(tmpRow("KBETR"))
                    .Parameters("@KONWA").Value = tmpRow("KONWA")
                    .Parameters("@KPEIN").Value = tmpRow("KPEIN")
                    .Parameters("@BPRME").Value = tmpRow("BPRME")
                    .Parameters("@MEINS").Value = tmpRow("MEINS")
                    .Parameters("@UMRECH").Value = tmpRow("UMRECH")
                    .Parameters("@MINBM").Value = CDec(tmpRow("MINBM"))
                    .Parameters("@MINBW").Value = CDec(tmpRow("MINBW"))
                End With
                cmd.ExecuteNonQuery()
            Next
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            cn.Close()
        End Try
    End Sub

    Private Sub DeleteBestellungFromSQLDB()
        '----------------------------------------------------------------------
        'Methode:      DeleteBestellungsFrom
        'Autor:         Julian Jung
        'Beschreibung:  löscht alle bestellungsdaten einer kasse aus der sqldb
        'Erstellt am:   27.04.2009
        '----------------------------------------------------------------------

        Dim cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Dim cmd As New SqlClient.SqlCommand

        Try
            cn.Open()
            cmd.Connection = cn
            cmd.CommandType = CommandType.Text
            Dim SqlQuery As String
            SqlQuery = "Delete FROM KBS_EFA_BESTELLUNGEN WHERE LGORT=@LGORT;"
            cmd.Parameters.AddWithValue("@LGORT", mMyKasse.Lagerort)
            cmd.CommandText = SqlQuery
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            cn.Close()
        End Try
    End Sub

    Public Function addArtikel(ByVal EAN As String, ByVal menge As Integer) As Boolean

        Dim tmpTable As DataTable = getEANDatenSAPERP(EAN)
        If tmpTable IsNot Nothing AndAlso tmpTable.Rows.Count > 0 Then
            If ErrorOccured Then
                Return False
            Else
                Dim newRow As DataRow = mBestellungen.NewRow()
                newRow.ItemArray = tmpTable(0).ItemArray
                newRow("BSTMG") = menge
                mBestellungen.Rows.Add(newRow)
                Return True
            End If
        Else
            Return False
        End If

    End Function

    Public Sub endOrder()
        mBestellungen.Clear()
        DeleteBestellungFromSQLDB()
        Finalize()
    End Sub

    Public Function getArtikelInfo(ByVal EAN As String, ByVal bIgnoreError104 As Boolean, ByRef Materialnummer As String, ByRef Artikelbezeichnung As String) As Boolean

        If getEANInfoSAPERP(EAN, bIgnoreError104, Materialnummer, Artikelbezeichnung) Then
            Return Not ErrorOccured
        Else
            Return False
        End If
    End Function

    Private Function getEANDatenSAPERP(ByVal EAN As String) As DataTable
        ClearErrorState()

        Try
            S.AP.Init("Z_FIL_READ_MATNR_PO", "I_EAN11, I_KOSTL", EAN.TrimStart("0"c), mMyKasse.Lagerort)

            S.AP.Execute()

            If S.AP.ResultCode = 0 Then
                Return S.AP.GetExportTable("ES_MAT_INFO")
            Else
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try

        Return Nothing
    End Function

    Private Function getEANInfoSAPERP(ByVal EAN As String, ByVal bIgnoreError104 As Boolean, ByRef Materialnummer As String, ByRef Artikelbezeichnung As String) As Boolean
        ClearErrorState()

        Try
            S.AP.Init("Z_FIL_READ_MATNR_001", "I_EAN11, I_KOSTL", EAN.TrimStart("0"c), mMyKasse.Lagerort)

            S.AP.Execute()

            'Artikel mit Returncode 104 muss trotzdem umgelagert werden können!
            If S.AP.ResultCode = 0 Or (bIgnoreError104 And S.AP.ResultCode = 104) Then
                Materialnummer = S.AP.GetExportParameter("E_MATNR")
                Artikelbezeichnung = S.AP.GetExportParameter("E_MAKTX")
            Else
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

            If ErrorCode = "103" Then
                RaiseError("103", "Der Artikel ist nicht mehr bestellbar!")
            End If

            Return (Not String.IsNullOrEmpty(Materialnummer))

        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Sub sendOrderToSAPERP()
        ClearErrorState()

        mBestellnummer = ""

        Try
            S.AP.Init("Z_FIL_PO_CREATE_001")

            Dim tblSAP As DataTable = S.AP.GetImportTable("GT_WEB")

            'mit select filter, da sonst auch deleted rows mitgenommen werden, die dann auf einen fehler laufen JJU20090511
            For Each tmprow As DataRow In Bestellungen.Select("", "", DataViewRowState.CurrentRows)
                Dim newRow As DataRow = tblSAP.NewRow()
                newRow.ItemArray = tmprow.ItemArray
                tblSAP.Rows.Add(newRow)
            Next

            S.AP.Execute()

            If S.AP.ResultCode = 0 Then
                mBestellnummer = S.AP.GetExportParameter("E_BANFN")
            Else
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

            Bestellungen.Rows.Clear()

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub
End Class
