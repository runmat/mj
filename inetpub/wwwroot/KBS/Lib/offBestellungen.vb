Imports KBSBase

Public Class offBestellungen
    Inherits ErrorHandlingClass

    Private mstrKostStelle As String
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
    Private mBANF As String = ""

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
                mBestellPostitionen.Columns.Add("EAN", String.Empty.GetType)
                mBestellPostitionen.Columns.Add("PositionLieferMenge", Type.GetType("System.Int32"))
                mBestellPostitionen.Columns.Add("PositionAbgeschlossen", String.Empty.GetType)
                mBestellPostitionen.Columns.Add("PositionVollstaendig", String.Empty.GetType)
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

    Public Property KostStelle() As String
        Get
            KostStelle = mstrKostStelle
        End Get
        Set(ByVal value As String)
            mstrKostStelle = value
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

    Public Property BANF() As String
        Get
            Return mBANF
        End Get
        Set(ByVal value As String)
            mBANF = value
        End Set
    End Property

#End Region

    Public Sub getLieferungsPositionenFromSAPERP(ByVal bestnr As String)
        ClearErrorState()

        Try
            Bestellpositionen.Clear()

            mBestellnummer = bestnr

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
                    Bestellpositionen.Rows.Add(tmprow)
                Next
            Else
                RaiseError("-1", "Keine Positionen vorhanden.")
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

    Public Sub getUmlPositionenFromSAPERP(ByVal bestnr As String)
        ClearErrorState()

        Try
            mBestellPostitionen.Clear()

            mBestellnummer = bestnr

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
                    tmprow("Mengeneinheit") = ""
                    tmprow("EAN") = ""
                    tmprow("PositionLieferMenge") = DBNull.Value
                    tmprow("PositionAbgeschlossen") = ""
                    tmprow("PositionVollstaendig") = ""
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
            S.AP.Init("Z_FIL_READ_OFF_BEST_001", "I_LGORT", mstrKostStelle)

            S.AP.Execute()

            If S.AP.ResultCode <> 0 Then
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

            Dim SapTableWEB As DataTable = S.AP.GetExportTable("GT_WEB")
            Dim SapTableUML As DataTable = S.AP.GetExportTable("GT_OFF_UML")
            Dim SapTableBANF As DataTable = S.AP.GetExportTable("GT_OFF_BANF")

            Dim tmprow As DataRow
            If SapTableWEB.Rows.Count > 0 Then
                ErwarteteLieferungen.Clear()
                For Each row As DataRow In SapTableWEB.Rows
                    tmprow = ErwarteteLieferungen.NewRow
                    tmprow("Bestellnummer") = row("EBELN").ToString
                    tmprow("LieferantName") = row("NAME1").ToString & " " & row("NAME2").ToString
                    tmprow("AnzeigeText") = row("EBELN").ToString & ", " & row("NAME1").ToString & " " & row("NAME2").ToString & " " & row("ORT01").ToString & ", " & row("BEDAT").ToString
                    tmprow("IstUmlagerung") = ""
                    ErwarteteLieferungen.Rows.Add(tmprow)
                Next
            ElseIf SapTableBANF.Rows.Count = 0 Then
                RaiseError("-1", "Keine Bestellungen vorhanden")
            End If

            If SapTableBANF.Rows.Count > 0 Then
                For Each row As DataRow In SapTableBANF.Rows
                    tmprow = ErwarteteLieferungen.NewRow
                    tmprow("Bestellnummer") = row("BANFN").ToString
                    tmprow("LieferantName") = ""
                    tmprow("AnzeigeText") = row("BANFN").ToString & ", " & row("BADAT").ToString
                    tmprow("IstUmlagerung") = "BANF"
                    ErwarteteLieferungen.Rows.Add(tmprow)
                Next
            ElseIf SapTableWEB.Rows.Count = 0 Then
                RaiseError("-1", "Keine Bestellungen vorhanden")
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

    Public Sub getBANFPositionenFromSAPERP()
        ClearErrorState()

        Try
            S.AP.Init("Z_FIL_EFA_BANF_OFF_POS", "I_BANFN", BANF)

            S.AP.Execute()

            If S.AP.ResultCode <> 0 Then
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

            Dim SapTableBANF As DataTable = S.AP.GetExportTable("GT_BANF")
            If SapTableBANF.Rows.Count > 0 Then
                Dim tmprow As DataRow
                For Each row As DataRow In SapTableBANF.Rows

                    tmprow = Bestellpositionen.NewRow
                    tmprow("Bestellposition") = CInt(row("BNFPO"))
                    tmprow("Materialnummer") = row("MATNR").ToString
                    tmprow("Artikelbezeichnung") = row("TXZ01").ToString
                    tmprow("MaterialnummerLieferant") = ""
                    tmprow("BestellteMenge") = CInt(row("MENGE"))
                    tmprow("Mengeneinheit") = row("MEINS").ToString
                    tmprow("EAN") = row("EAN11")
                    tmprow("PositionLieferMenge") = DBNull.Value
                    tmprow("PositionAbgeschlossen") = ""
                    tmprow("PositionVollstaendig") = ""

                    Bestellpositionen.Rows.Add(tmprow)
                Next
            Else
                RaiseError("-1", "Keine Positionen vorhanden")
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

End Class
