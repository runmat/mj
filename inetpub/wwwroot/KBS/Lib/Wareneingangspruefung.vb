Imports KBSBase

Public Class Wareneingangspruefung
    Inherits ErrorHandlingClass

    Private mMyKasse As Kasse
    Private mErwarteteLieferungen As DataTable
    Private mBestellPostitionen As DataTable
    Private mBestellnummer As String
    Private mLieferantAnzeige As String
    Private mLieferantSelection As String = ""
    Private mBestellnummerSelection As String = ""
    Private mIstUmlagerung As String = ""
    Private mstrBedienerNr As String = ""
    Private mCurrentApplikationPage As Page
    Private mUmlNummer As String = ""

#Region "Properties"

    Public ReadOnly Property ErwarteteLieferungen() As DataTable
        Get
            If mErwarteteLieferungen Is Nothing Then
                mErwarteteLieferungen = New DataTable
                mErwarteteLieferungen.Columns.Add("Bestellnummer", String.Empty.GetType)
                mErwarteteLieferungen.Columns.Add("LieferantName", String.Empty.GetType)
                mErwarteteLieferungen.Columns.Add("AnzeigeText", String.Empty.GetType)
                mErwarteteLieferungen.Columns.Add("IstUmlagerung", String.Empty.GetType)
            End If
            Return mErwarteteLieferungen
        End Get
    End Property

    Public ReadOnly Property Bestellpositionen() As DataTable
        Get
            If mBestellPostitionen Is Nothing Then
                mBestellPostitionen = New DataTable
                mBestellPostitionen.Columns.Add("Bestellposition", Type.GetType("System.Int32"))
                mBestellPostitionen.Columns.Add("Materialnummer", String.Empty.GetType)
                mBestellPostitionen.Columns.Add("Artikelbezeichnung", String.Empty.GetType)
                mBestellPostitionen.Columns.Add("MaterialnummerLieferant", String.Empty.GetType)
                mBestellPostitionen.Columns.Add("BestellteMenge", Type.GetType("System.Int32"))
                mBestellPostitionen.Columns.Add("Mengeneinheit", String.Empty.GetType)
                mBestellPostitionen.Columns.Add("Buchungsdatum", String.Empty.GetType)
                mBestellPostitionen.Columns.Add("EAN", String.Empty.GetType)
                mBestellPostitionen.Columns.Add("PositionLieferMenge", Type.GetType("System.Int32"))
                mBestellPostitionen.Columns.Add("PositionAbgeschlossen", String.Empty.GetType)
                mBestellPostitionen.Columns.Add("PositionVollstaendig", String.Empty.GetType)
                mBestellPostitionen.Columns.Add("LTEXT_NR", String.Empty.GetType)
                mBestellPostitionen.Columns.Add("TEXT", String.Empty.GetType)
                mBestellPostitionen.Columns.Add("KennzForm", String.Empty.GetType)
            End If
            Return mBestellPostitionen
        End Get
    End Property

    Public Property currentApplikationPage() As Page
        Get
            Return mCurrentApplikationPage
        End Get
        Set(ByVal value As Page)
            mCurrentApplikationPage = value
        End Set
    End Property

    Public ReadOnly Property SelectionString() As String
        Get
            Return "LieferantName LIKE '" & mLieferantSelection.Replace("*", "%") & "' AND Bestellnummer LIKE '" & mBestellnummerSelection.Replace("*", "%") & "'"
        End Get
    End Property

    Public WriteOnly Property LieferantSelection() As String
        Set(ByVal value As String)
            mLieferantSelection = value
        End Set
    End Property

    Public WriteOnly Property BestellnummerSelection() As String
        Set(ByVal value As String)
            mBestellnummerSelection = value
        End Set
    End Property

    Private Property Bestellnummer() As String
        Get
            Return mBestellnummer
        End Get
        Set(ByVal value As String)
            mBestellnummer = value
        End Set
    End Property

    Public Property LiefantAnzeige() As String
        Get
            Return mLieferantAnzeige
        End Get
        Set(ByVal value As String)
            mLieferantAnzeige = value
        End Set
    End Property

    Public Property IstUmlagerung() As String
        Get
            Return mIstUmlagerung
        End Get
        Set(ByVal value As String)
            mIstUmlagerung = value
        End Set
    End Property

    Public Property BedienerNr() As String
        Get
            BedienerNr = mstrBedienerNr
        End Get
        Set(ByVal value As String)
            mstrBedienerNr = value
        End Set
    End Property

    Public Property UmlNummer() As String
        Get
            Return mUmlNummer
        End Get
        Set(ByVal value As String)
            mUmlNummer = value
        End Set
    End Property

#End Region

    Public Sub New(ByRef Kasse As Kasse)
        mMyKasse = Kasse
        getErwarteteLieferungenFromSAPERP()
    End Sub

    Public Sub endWareneingangspruefung()
        Finalize()
    End Sub

    Public Sub getLieferungsPositionenFromSAPERP(ByVal bestnr As String)
        ClearErrorState()

        mBestellnummer = bestnr

        Try
            S.AP.Init("Z_FIL_READ_OFF_BEST_POS_001", "I_EBELN", bestnr)

            S.AP.Execute()

            If S.AP.ResultCode <> 0 Then
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

            Dim SapTable As DataTable = S.AP.GetExportTable("GT_WEB")
            If SapTable.Rows.Count > 0 Then
                For Each row As DataRow In SapTable.Rows
                    Dim tmprow As DataRow = Bestellpositionen.NewRow
                    tmprow("Bestellposition") = CInt(row("EBELP"))
                    tmprow("Materialnummer") = row("MATNR").ToString
                    tmprow("Artikelbezeichnung") = row("TXZ01").ToString
                    tmprow("MaterialnummerLieferant") = row("IDNLF").ToString
                    tmprow("BestellteMenge") = CInt(row("BSTMG"))
                    tmprow("Mengeneinheit") = row("MEINS").ToString
                    tmprow("EAN") = row("EAN11").ToString
                    tmprow("PositionLieferMenge") = DBNull.Value
                    tmprow("PositionAbgeschlossen") = ""
                    tmprow("PositionVollstaendig") = ""
                    tmprow("LTEXT_NR") = ""
                    tmprow("TEXT") = ""
                    tmprow("KennzForm") = ""
                    Bestellpositionen.Rows.Add(tmprow)
                Next
            Else
                RaiseError("-1", "Keine Positionen vorhanden")
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

    Public Sub getUmlPositionenFromSAPERP(ByVal bestnr As String)
        ClearErrorState()

        mBestellnummer = bestnr

        Try
            S.AP.Init("Z_FIL_EFA_UML_OFF_POS", "I_BELNR", bestnr)

            S.AP.Execute()

            If S.AP.ResultCode <> 0 Then
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

            Dim SapTable As DataTable = S.AP.GetExportTable("GT_OFF_UML_POS")
            If SapTable.Rows.Count > 0 Then
                For Each row As DataRow In SapTable.Rows
                    Dim tmprow As DataRow = Bestellpositionen.NewRow
                    tmprow("Bestellposition") = CInt(row("POSNR"))
                    tmprow("Materialnummer") = row("MATNR").ToString
                    tmprow("Artikelbezeichnung") = row("MAKTX").ToString
                    tmprow("MaterialnummerLieferant") = row("MATNR").ToString
                    tmprow("BestellteMenge") = CInt(row("MENGE"))
                    tmprow("Buchungsdatum") = row("BUDAT").ToString
                    tmprow("Mengeneinheit") = ""
                    tmprow("EAN") = row("EAN11").ToString
                    tmprow("PositionLieferMenge") = DBNull.Value
                    tmprow("PositionAbgeschlossen") = ""
                    tmprow("PositionVollstaendig") = ""
                    tmprow("LTEXT_NR") = row("LTEXT_NR").ToString
                    tmprow("TEXT") = row("TEXT").ToString
                    tmprow("Kennzform") = row("Kennzform").ToString
                    Bestellpositionen.Rows.Add(tmprow)
                Next
            Else
                RaiseError("-1", "Keine Positionen vorhanden")
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

    Public Sub getErwarteteLieferungenFromSAPERP()
        ClearErrorState()

        Try
            S.AP.Init("Z_FIL_READ_OFF_BEST_001", "I_LGORT, I_LIFNR", mMyKasse.Lagerort, Bestellnummer)

            S.AP.Execute()

            If S.AP.ResultCode <> 0 Then
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

            Dim SapTableWEB As DataTable = S.AP.GetExportTable("GT_WEB")
            Dim SapTableUML As DataTable = S.AP.GetExportTable("GT_OFF_UML")

            ErwarteteLieferungen.Clear()
            Dim tmprow As DataRow

            If SapTableWEB.Rows.Count > 0 Then
                For Each row As DataRow In SapTableWEB.Rows
                    tmprow = ErwarteteLieferungen.NewRow
                    tmprow("Bestellnummer") = row("EBELN").ToString
                    tmprow("LieferantName") = row("NAME1").ToString & " " & row("NAME2").ToString
                    tmprow("AnzeigeText") = row("EBELN").ToString & ", " & row("NAME1").ToString & " " & row("NAME2").ToString & " " & row("ORT01").ToString & ", " & row("BEDAT").ToString
                    tmprow("IstUmlagerung") = ""
                    ErwarteteLieferungen.Rows.Add(tmprow)
                Next
            End If
            If SapTableUML.Rows.Count > 0 Then
                For Each row As DataRow In SapTableUML.Rows
                    tmprow = ErwarteteLieferungen.NewRow
                    tmprow("Bestellnummer") = row("BELNR").ToString
                    tmprow("LieferantName") = row("KTEXT").ToString
                    tmprow("AnzeigeText") = row("BELNR").ToString & ", " & row("KTEXT").ToString & ", " & row("BUDAT").ToString
                    tmprow("IstUmlagerung") = "X"
                    ErwarteteLieferungen.Rows.Add(tmprow)
                Next
            End If

            If ErwarteteLieferungen.Rows.Count = 0 Then
                RaiseError("-1", "Keine Positionen vorhanden")
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

    Public Sub sendOrderCheckToSAPERP(ByVal Lieferscheinnummer As String, ByVal Belegdatum As Date)
        ClearErrorState()

        Try
            S.AP.Init("Z_FIL_WE_ZUR_BEST_POS_001")

            S.AP.SetImportParameter("I_LGORT", mMyKasse.Lagerort)
            S.AP.SetImportParameter("I_EBELN", mBestellnummer)
            S.AP.SetImportParameter("I_LFSNR", Lieferscheinnummer)
            S.AP.SetImportParameter("I_BLDAT", Belegdatum.ToShortDateString())

            Dim tblSAP As DataTable = S.AP.GetImportTable("GT_WEB")
            Dim ueberspringe As Boolean = False
            For Each tmprow As DataRow In Bestellpositionen.Rows
                Dim tmpSAPRow As DataRow = tblSAP.NewRow
                tmpSAPRow("EBELP") = tmprow("Bestellposition").ToString
                tmpSAPRow("MATNR") = tmprow("Materialnummer").ToString
                tmpSAPRow("ERFME") = tmprow("Mengeneinheit").ToString
                tmpSAPRow("EAN11") = tmprow("EAN").ToString
                If tmprow("PositionVollstaendig").ToString = "X" Then
                    If tmprow("PositionLieferMenge").ToString.Length > 0 Then
                        tmpSAPRow("ERFMG") = tmprow("PositionLieferMenge").ToString
                    Else
                        tmpSAPRow("ERFMG") = tmprow("BestellteMenge").ToString
                    End If

                    tmpSAPRow("ELIKZ") = "X"
                Else 'wenn lieferung nicht vollständig, dann lieferpositionsmenge / LieferungsAbschluss eintragen
                    If tmprow("PositionAbgeschlossen").ToString = "J" Then
                        tmpSAPRow("ERFMG") = tmprow("PositionLieferMenge").ToString
                        tmpSAPRow("ELIKZ") = "X"
                    Else
                        tmpSAPRow("ELIKZ") = ""
                        If CInt(tmprow("PositionLieferMenge")) > 0 Then
                            tmpSAPRow("ERFMG") = tmprow("PositionLieferMenge").ToString
                        Else
                            ueberspringe = True
                        End If
                    End If
                End If

                If Not ueberspringe Then
                    tblSAP.Rows.Add(tmpSAPRow)
                End If
                ueberspringe = False
            Next

            S.AP.Execute()

            If S.AP.ResultCode <> 0 Then
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

    Public Sub sendUmlToSAPERP(ByVal Belegdatum As Date)
        ClearErrorState()

        Try
            S.AP.Init("Z_FIL_EFA_UML_STEP2")

            S.AP.SetImportParameter("I_BELNR", mUmlNummer)
            S.AP.SetImportParameter("I_KOSTL", mMyKasse.Lagerort)
            S.AP.SetImportParameter("I_BUDAT", Belegdatum.ToShortDateString())

            Dim tblSAP As DataTable = S.AP.GetImportTable("GT_OFF_UML_POS")
            Dim ueberspringe As Boolean = False
            For Each tmprow As DataRow In Bestellpositionen.Rows
                Dim tmpSAPRow As DataRow = tblSAP.NewRow
                tmpSAPRow("BELNR") = mUmlNummer
                tmpSAPRow("POSNR") = tmprow("Bestellposition").ToString
                tmpSAPRow("MATNR") = tmprow("Materialnummer").ToString
                tmpSAPRow("MAKTX") = tmprow("Artikelbezeichnung").ToString
                tmpSAPRow("BUDAT") = Belegdatum.ToShortDateString()
                If tmprow("PositionVollstaendig").ToString = "X" Then
                    tmpSAPRow("MENGE") = tmprow("BestellteMenge").ToString
                Else 'wenn lieferung nicht vollständig, dann lieferpositionsmenge / LieferungsAbschluss eintragen
                    If tmprow("PositionAbgeschlossen").ToString = "J" Then
                        tmpSAPRow("MENGE") = tmprow("PositionLieferMenge").ToString
                    Else
                        If CInt(tmprow("PositionLieferMenge")) > 0 Then
                            tmpSAPRow("MENGE") = tmprow("PositionLieferMenge").ToString
                        Else
                            ueberspringe = True
                        End If
                    End If
                End If
                tmpSAPRow("TEXT") = tmprow("Materialnummer").ToString
                tmpSAPRow("TEXT") = tmprow("TEXT").ToString
                tmpSAPRow("LTEXT_NR") = tmprow("LTEXT_NR").ToString
                tmpSAPRow("EAN11") = tmprow("EAN").ToString
                tmpSAPRow("KENNZFORM") = tmprow("KENNZFORM").ToString

                If Not ueberspringe Then
                    tblSAP.Rows.Add(tmpSAPRow)
                End If
                ueberspringe = False
            Next

            S.AP.Execute()

            If S.AP.ResultCode <> 0 Then
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

End Class
