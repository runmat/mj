
Public Class Bestellung
    Private mBestellungen As DataTable
    Private mMyKasse As Kasse
    Private mSAPStatus As Integer
    Private mSAPMessage As String
    Private mBestellnummer As String
    Private mE_SUBRC As Integer
    Private mE_MESSAGE As String
    Dim SAPExc As SAPExecutor.SAPExecutor

    Public Property E_SUBRC() As Integer
        Get
            Return mE_SUBRC
        End Get
        Set(ByVal Value As Integer)
            mE_SUBRC = Value
        End Set
    End Property

    Public Property E_MESSAGE() As String
        Get
            Return mE_MESSAGE
        End Get
        Set(ByVal Value As String)
            mE_MESSAGE = Value
        End Set
    End Property

    Public ReadOnly Property Bestellungen() As DataTable
        Get
            If mBestellungen Is Nothing Then
                mBestellungen = createArtikelTabelle()
            End If
            Return mBestellungen
        End Get
    End Property

    Public ReadOnly Property SAPStatus() As Integer
        Get
            Return mSAPStatus
        End Get
    End Property

    Public ReadOnly Property SAPStatusText() As String
        Get
            Return mSAPMessage
        End Get
    End Property

    Public ReadOnly Property Bestellnummer() As String
        Get
            Return mBestellnummer
        End Get
    End Property

    Public Sub New(ByRef Kasse As Kasse)
        mMyKasse = Kasse
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
            If E_MESSAGE.Length > 0 Then
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
            If E_MESSAGE.Length > 0 Then
                Return False
            Else
                Return True
            End If
        Else
            Return False
        End If
    End Function

    Private Function getEANDatenSAPERP(ByVal EAN As String) As DataTable

        SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)
        E_MESSAGE = ""
        E_SUBRC = 0

        Try
            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()

            dt.Rows.Add(New Object() {"I_EAN11", False, EAN.TrimStart("0"c), 18})
            dt.Rows.Add(New Object() {"I_KOSTL", False, mMyKasse.Lagerort, 10})
            dt.Rows.Add(New Object() {"ES_MAT_INFO", True})

            SAPExc.ExecuteERP("Z_FIL_READ_MATNR_PO", dt)

            If (SAPExc.ErrorOccured) Then
                E_SUBRC = SAPExc.E_SUBRC
                E_MESSAGE = SAPExc.E_MESSAGE
            Else
                Dim retRows As DataRow = dt.Select("Fieldname='ES_MAT_INFO'")(0)
                If Not retRows Is Nothing Then
                    Return DirectCast(retRows("Data"), DataTable)
                End If
            End If

        Catch ex As Exception

        End Try

        Return Nothing

    End Function

    Private Function getEANInfoSAPERP(ByVal EAN As String, ByVal bIgnoreError104 As Boolean, ByRef Materialnummer As String, ByRef Artikelbezeichnung As String) As Boolean

        SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)
        E_MESSAGE = ""
        E_SUBRC = 0

        Try
            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()

            dt.Rows.Add(New Object() {"I_EAN11", False, EAN.TrimStart("0"c), 18})
            dt.Rows.Add(New Object() {"I_KOSTL", False, mMyKasse.Lagerort, 10})
            dt.Rows.Add(New Object() {"I_PRODH", False, "", 18})
            dt.Rows.Add(New Object() {"E_MAKTX", True})
            dt.Rows.Add(New Object() {"E_MATNR", True})

            SAPExc.ExecuteERP("Z_FIL_READ_MATNR_001", dt)

            'Artikel mit Returncode 104 muss trotzdem umgelagert werden können!
            If SAPExc.ErrorOccured And Not (bIgnoreError104 And CInt(SAPExc.E_SUBRC) = 104) Then
                E_SUBRC = SAPExc.E_SUBRC
                E_MESSAGE = SAPExc.E_MESSAGE
            Else
                Dim retRows As DataRow = dt.Select("Fieldname='E_MATNR'")(0)
                If Not retRows Is Nothing Then
                    Materialnummer = retRows("Data").ToString()
                End If
                retRows = dt.Select("Fieldname='E_MAKTX'")(0)
                If Not retRows Is Nothing Then
                    Artikelbezeichnung = retRows("Data").ToString()
                End If
            End If
            If E_SUBRC = 103 Then
                E_MESSAGE = "Der Artikel ist nicht mehr bestellbar!"
            End If
            If Not Materialnummer = "" Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            Return False
        Finally

        End Try

    End Function

    Public Sub sendOrderToSAPERP()

        SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)

        mSAPStatus = 0
        mBestellnummer = ""
        mSAPMessage = ""

        Dim tblSAP As DataTable = createArtikelTabelle()

        'mit select filter, da sonst auch deletet rows mitgenommen werden, die dann auf einen fehler laufen JJU20090511
        For Each tmprow As DataRow In Bestellungen.Select("", "", DataViewRowState.CurrentRows)
            Dim newRow As DataRow = tblSAP.NewRow()
            newRow.ItemArray = tmprow.ItemArray
            tblSAP.Rows.Add(newRow)
        Next

        Try
            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()

            dt.Rows.Add(New Object() {"GT_WEB", False, tblSAP})
            dt.Rows.Add(New Object() {"GT_FEHL", True})
            dt.Rows.Add(New Object() {"E_BANFN", True})

            SAPExc.ExecuteERP("Z_FIL_PO_CREATE_001", dt)

            If (SAPExc.ErrorOccured) Then
                E_SUBRC = SAPExc.E_SUBRC
                E_MESSAGE = SAPExc.E_MESSAGE
            Else
                Dim retRows As DataRow = dt.Select("Fieldname='E_BANFN'")(0)
                If Not retRows Is Nothing Then
                    mBestellnummer = retRows("Data").ToString()
                End If
            End If

            Bestellungen.Rows.Clear()

        Catch ex As Exception
            mSAPStatus = -11
            mSAPMessage = ex.Message
        End Try

    End Sub

    Private Function createArtikelTabelle() As DataTable
        Dim tbl As New DataTable()

        tbl.Columns.Add("EKORG", GetType(String))  'Einkaufsorganisation
        tbl.Columns.Add("KOSTL", GetType(String))  'Kostenstelle
        tbl.Columns.Add("WERKS", GetType(String))  'Werk
        tbl.Columns.Add("LIFNR", GetType(String))  'Kontonummer Lieferant
        tbl.Columns.Add("MATNR", GetType(String))  'Materialnummer
        tbl.Columns.Add("BSTMG", GetType(Integer))  'Bestellmenge
        tbl.Columns.Add("EAN11", GetType(String))  'EAN
        tbl.Columns.Add("LGORT", GetType(String))  'Lagerort
        tbl.Columns.Add("MAKTX", GetType(String))  'Materialbezeichnung
        tbl.Columns.Add("NAME1", GetType(String))  'Lieferantenname
        tbl.Columns.Add("IDNLF", GetType(String))  'Materialnummer (Lieferant)
        tbl.Columns.Add("RELIF", GetType(String))  'Regellieferant
        tbl.Columns.Add("ESOKZ", GetType(String))  'Einkaufsinfosatz-Typ
        tbl.Columns.Add("KBETR", GetType(Double))  'Konditionsbetrag
        tbl.Columns.Add("KONWA", GetType(String))  'Konditionseinheit
        tbl.Columns.Add("KPEIN", GetType(Integer))   'Konditionspreiseinheit
        tbl.Columns.Add("BPRME", GetType(String))  'Bestellpreismengeneinheit
        tbl.Columns.Add("MEINS", GetType(String))  'Basismengeneinheit
        tbl.Columns.Add("UMRECH", GetType(Integer))  'Umrechnung Bestellmenge -> Lagermengeneinheit
        tbl.Columns.Add("MINBM", GetType(Integer))  'Mindestbestellmenge
        tbl.Columns.Add("MINBW", GetType(Double))  'Mindestbestellwert

        Return tbl
    End Function

End Class


' ************************************************
' $History: Bestellung.vb $
' 
' *****************  Version 18  *****************
' User: Rudolpho     Date: 1.11.10    Time: 16:16
' Updated in $/CKAG2/KBS/Lib
' 
' *****************  Version 17  *****************
' User: Rudolpho     Date: 24.09.10   Time: 16:58
' Updated in $/CKAG2/KBS/Lib
' 
' *****************  Version 16  *****************
' User: Rudolpho     Date: 12.03.10   Time: 16:40
' Updated in $/CKAG2/KBS/Lib
' 
' *****************  Version 15  *****************
' User: Rudolpho     Date: 12.03.10   Time: 13:36
' Updated in $/CKAG2/KBS/Lib
' 
' *****************  Version 14  *****************
' User: Rudolpho     Date: 4.03.10    Time: 9:43
' Updated in $/CKAG2/KBS/Lib
' 
' *****************  Version 13  *****************
' User: Jungj        Date: 16.07.09   Time: 14:45
' Updated in $/CKAG2/KBS/Lib
' nachbesserung fehlerausgabe
' 
' *****************  Version 12  *****************
' User: Jungj        Date: 13.05.09   Time: 17:56
' Updated in $/CKAG2/KBS/Lib
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 12.05.09   Time: 16:17
' Updated in $/CKAG2/KBS/Lib
' ITA 2808
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 11.05.09   Time: 10:35
' Updated in $/CKAG2/KBS/Lib
' nderungen Nach test ITA 2808
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 5.05.09    Time: 14:43
' Updated in $/CKAG2/KBS/Lib
' ITA Nachbesserungen Bestellung
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 5.05.09    Time: 12:44
' Updated in $/CKAG2/KBS/Lib
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 5.05.09    Time: 10:12
' Updated in $/CKAG2/KBS/Lib
' ITA 2838 kommentare 
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 30.04.09   Time: 13:39
' Updated in $/CKAG2/KBS/Lib
' ITA 2838 unfertig
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 29.04.09   Time: 9:31
' Updated in $/CKAG2/KBS/Lib
' ITA 2808 testfertig
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 28.04.09   Time: 17:56
' Updated in $/CKAG2/KBS/Lib
' ITA 2808
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 27.04.09   Time: 17:24
' Updated in $/CKAG2/KBS/Lib
' ITA 2808
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 24.04.09   Time: 15:48
' Updated in $/CKAG2/KBS/Lib
' ITA 2808
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 23.04.09   Time: 17:50
' Created in $/CKAG2/KBS/Lib
' ITA 2808
' 
'
' ************************************************