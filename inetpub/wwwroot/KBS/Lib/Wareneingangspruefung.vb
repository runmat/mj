
Public Class Wareneingangspruefung
    Private mMyKasse As Kasse
    Private mErwarteteLieferungen As DataTable
    Private mBestellPostitionen As DataTable
    Private mSAPStatus As Integer
    Private mSAPMessage As String
    Private mBestellnummer As String
    Private mLieferantAnzeige As String
    Private mLieferantSelection As String = ""
    Private mBestellnummerSelection As String = ""
    Private mIstUmlagerung As String = ""
    Private mstrBedienerNr As String = ""
    Private mCurrentApplikationPage As Page
    Private mE_SUBRC As Integer
    Private mE_MESSAGE As String = ""
    Private mUmlNummer As String = ""
    Dim SAPExc As SAPExecutor.SAPExecutor

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

    Public Sub New(ByRef Kasse As Kasse)
        mMyKasse = Kasse
        getErwarteteLieferungenFromSAPERP()
    End Sub

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

    Public Property E_SUBRC() As String
        Get
            Return mE_SUBRC
        End Get
        Set(ByVal Value As String)
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

    Public Sub endWareneingangspruefung()
        Finalize()
    End Sub

    Public Shared Function MakeDateStandard(ByVal strInput As String) As Date
        REM § Formt String-Input im SAP-Format in Standard-Date um. Gibt "01.01.1900" zurück, wenn Umwandlung nicht möglich ist.
        Dim strTemp As String = Right(strInput, 2) & "." & Mid(strInput, 5, 2) & "." & Left(strInput, 4)
        If IsDate(strTemp) Then
            Return CDate(strTemp)
        Else
            Return CDate("01.01.1900")
        End If
    End Function
    
    Public Sub getLieferungsPositionenFromSAPERP(ByVal bestnr As String)

        mBestellnummer = bestnr
        SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)
        E_MESSAGE = ""
        E_SUBRC = 0

        Try
            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()

            dt.Rows.Add(New Object() {"I_EBELN", False, bestnr, 10})
            dt.Rows.Add(New Object() {"GT_WEB", True})

            SAPExc.ExecuteERP("Z_FIL_READ_OFF_BEST_POS_001", dt)

            If (SAPExc.ErrorOccured) Then
                If IsNumeric(SAPExc.E_SUBRC) Then mSAPStatus = SAPExc.E_SUBRC
                E_MESSAGE = SAPExc.E_MESSAGE
            End If
            Dim SapTable As DataTable
            Dim retRows As DataRow = dt.Select("Fieldname='GT_WEB'")(0)
            If Not retRows Is Nothing Then
                SapTable = DirectCast(retRows("Data"), DataTable)
                Dim tmprow As DataRow
                If SapTable.Rows.Count > 0 Then
                    For Each row As DataRow In SapTable.Rows

                        tmprow = Bestellpositionen.NewRow
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
                    mSAPMessage = "Keine Positionen vorhanden"
                    mSAPStatus = -1
                End If

            End If

        Catch ex As Exception
            mSAPStatus = -11
            mSAPMessage = "Techn. Fehlermeldung: " & ex.Message
        Finally

        End Try

    End Sub

    Public Sub getUmlPositionenFromSAPERP(ByVal bestnr As String)

        mBestellnummer = bestnr
        SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)
        E_MESSAGE = ""
        E_SUBRC = 0

        Try
            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()

            dt.Rows.Add(New Object() {"I_BELNR", False, bestnr, 10})
            dt.Rows.Add(New Object() {"GT_OFF_UML_POS", True})

            SAPExc.ExecuteERP("Z_FIL_EFA_UML_OFF_POS", dt)

            If (SAPExc.ErrorOccured) Then
                If IsNumeric(SAPExc.E_SUBRC) Then mSAPStatus = SAPExc.E_SUBRC
                E_MESSAGE = SAPExc.E_MESSAGE
            End If
            Dim SapTable As DataTable
            Dim retRows As DataRow = dt.Select("Fieldname='GT_OFF_UML_POS'")(0)
            If Not retRows Is Nothing Then
                SapTable = DirectCast(retRows("Data"), DataTable)
                Dim tmprow As DataRow
                If SapTable.Rows.Count > 0 Then
                    For Each row As DataRow In SapTable.Rows
                        tmprow = Bestellpositionen.NewRow
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
                    mSAPMessage = "Keine Positionen vorhanden"
                    mSAPStatus = -1
                End If

            End If
        Catch ex As Exception
            mSAPStatus = -11
            mSAPMessage = "Techn. Fehlermeldung: " & ex.Message
        Finally

        End Try

    End Sub

    Public Sub getErwarteteLieferungenFromSAPERP()
        mSAPStatus = 0
        mSAPMessage = ""
        SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)
        Try
            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()

            dt.Rows.Add(New Object() {"I_LGORT", False, mMyKasse.Lagerort, 4})
            dt.Rows.Add(New Object() {"I_LIFNR", False, Bestellnummer, 10})
            dt.Rows.Add(New Object() {"GT_WEB", True})
            dt.Rows.Add(New Object() {"GT_OFF_UML", True})
            dt.Rows.Add(New Object() {"GT_OFF_BANF", True})

            SAPExc.ExecuteERP("Z_FIL_READ_OFF_BEST_001", dt)

            If (SAPExc.ErrorOccured) Then
                If IsNumeric(SAPExc.E_SUBRC) Then mSAPStatus = SAPExc.E_SUBRC
                mSAPMessage = SAPExc.E_MESSAGE
            End If
            Dim SapTableWEB As DataTable = Nothing
            Dim SapTableUML As DataTable = Nothing
            Dim retRowsWEB As DataRow = dt.Select("Fieldname='GT_WEB'")(0)
            Dim retRowsUML As DataRow = dt.Select("Fieldname='GT_OFF_UML'")(0)

            If Not retRowsWEB Is Nothing Then SapTableWEB = DirectCast(retRowsWEB("Data"), DataTable)
            If Not retRowsUML Is Nothing Then SapTableUML = DirectCast(retRowsUML("Data"), DataTable)
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
            ElseIf SapTableUML.Rows.Count = 0 Then
                mSAPMessage = "Keine Bestellungen vorhanden"
                mSAPStatus = -1
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
            ElseIf SapTableWEB.Rows.Count = 0 Then
                mSAPMessage = "Keine Bestellungen vorhanden"
                mSAPStatus = -1
            End If
        Catch ex As Exception
            Select Case KBS_BASE.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    mSAPStatus = -1
                    mSAPMessage = "Keine Bestellungen vorhanden"
                Case Else
                    mSAPStatus = -11
                    mSAPMessage = "Techn. Fehlermeldung: " & ex.Message
            End Select
        End Try
    End Sub

    Public Sub sendOrderCheckToSAPERP(ByVal Lieferscheinnummer As String, ByVal Belegdatum As Date)

        mSAPStatus = 0
        mSAPMessage = ""
        E_MESSAGE = ""
        E_SUBRC = 0

        Dim tblSAP As New DataTable()
        tblSAP.Columns.Add("EBELP", String.Empty.GetType)
        tblSAP.Columns.Add("MATNR", String.Empty.GetType)
        tblSAP.Columns.Add("ERFMG", String.Empty.GetType)
        tblSAP.Columns.Add("ERFME", String.Empty.GetType)
        tblSAP.Columns.Add("EAN11", String.Empty.GetType)
        tblSAP.Columns.Add("ELIKZ", String.Empty.GetType)

        Dim tmpSAPRow As DataRow
        Dim ueberspringe As Boolean = False
        For Each tmprow As DataRow In Bestellpositionen.Rows
            tmpSAPRow = tblSAP.NewRow
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
        SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)
        Try
            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()

            dt.Rows.Add(New Object() {"I_LGORT", False, mMyKasse.Lagerort, 4})
            dt.Rows.Add(New Object() {"I_EBELN", False, mBestellnummer, 10})
            dt.Rows.Add(New Object() {"I_LFSNR", False, Lieferscheinnummer, 16})
            dt.Rows.Add(New Object() {"I_BLDAT", False, Belegdatum.ToShortDateString, 8})
            dt.Rows.Add(New Object() {"GT_WEB", False, tblSAP})

            SAPExc.ExecuteERP("Z_FIL_WE_ZUR_BEST_POS_001", dt)

            If (SAPExc.ErrorOccured) Then
                If IsNumeric(SAPExc.E_SUBRC) Then mSAPStatus = SAPExc.E_SUBRC
                mSAPMessage = SAPExc.E_MESSAGE
            End If

        Catch ex As Exception
            mSAPStatus = -11
            mSAPMessage = "Techn. Fehlermeldung: " & ex.Message
        Finally
        End Try

    End Sub

    Public Sub sendUmlToSAPERP(ByVal Belegdatum As Date)
        mSAPStatus = 0
        mSAPMessage = ""

        Dim tblSAP As New DataTable()
        tblSAP.Columns.Add("BELNR", String.Empty.GetType)
        tblSAP.Columns.Add("POSNR", String.Empty.GetType)
        tblSAP.Columns.Add("MATNR", String.Empty.GetType)
        tblSAP.Columns.Add("MENGE", String.Empty.GetType)
        tblSAP.Columns.Add("MAKTX", String.Empty.GetType)
        tblSAP.Columns.Add("BUDAT", String.Empty.GetType)
        tblSAP.Columns.Add("EAN11", String.Empty.GetType)
        tblSAP.Columns.Add("TEXT", String.Empty.GetType)
        tblSAP.Columns.Add("LTEXT_NR", String.Empty.GetType)
        tblSAP.Columns.Add("KENNZFORM", String.Empty.GetType)

        Dim tmpSAPRow As DataRow
        Dim ueberspringe As Boolean = False
        For Each tmprow As DataRow In Bestellpositionen.Rows
            tmpSAPRow = tblSAP.NewRow
            tmpSAPRow("BELNR") = mUmlNummer
            tmpSAPRow("POSNR") = tmprow("Bestellposition").ToString
            tmpSAPRow("MATNR") = tmprow("Materialnummer").ToString
            tmpSAPRow("MAKTX") = tmprow("Artikelbezeichnung").ToString
            tmpSAPRow("BUDAT") = Year(Belegdatum) & Right("0" & Month(Belegdatum), 2) & Right("0" & Day(Belegdatum), 2)
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

        SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)
        Try
            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()

            dt.Rows.Add(New Object() {"I_BELNR", False, mUmlNummer, 10})
            dt.Rows.Add(New Object() {"I_KOSTL", False, mMyKasse.Lagerort, 4})
            dt.Rows.Add(New Object() {"I_BUDAT", False, Belegdatum.ToShortDateString, 8})
            dt.Rows.Add(New Object() {"GT_OFF_UML_POS", False, tblSAP})

            SAPExc.ExecuteERP("Z_FIL_EFA_UML_STEP2", dt)

            If (SAPExc.ErrorOccured) Then
                If IsNumeric(SAPExc.E_SUBRC) Then mSAPStatus = SAPExc.E_SUBRC
                mSAPMessage = SAPExc.E_MESSAGE
            End If

        Catch ex As Exception
            mSAPStatus = -11
            mSAPMessage = "Techn. Fehlermeldung: " & ex.Message
        Finally
        End Try


    End Sub

End Class


' ************************************************
' $History: Wareneingangspruefung.vb $
' 
' *****************  Version 21  *****************
' User: Rudolpho     Date: 30.03.11   Time: 10:17
' Updated in $/CKAG2/KBS/Lib
' 
' *****************  Version 20  *****************
' User: Rudolpho     Date: 28.03.11   Time: 14:02
' Updated in $/CKAG2/KBS/Lib
' 
' *****************  Version 19  *****************
' User: Rudolpho     Date: 16.09.10   Time: 17:48
' Updated in $/CKAG2/KBS/Lib
' 
' *****************  Version 18  *****************
' User: Rudolpho     Date: 11.08.10   Time: 10:46
' Updated in $/CKAG2/KBS/Lib
' 
' *****************  Version 17  *****************
' User: Rudolpho     Date: 10.08.10   Time: 13:01
' Updated in $/CKAG2/KBS/Lib
' 
' *****************  Version 16  *****************
' User: Rudolpho     Date: 5.08.10    Time: 11:21
' Updated in $/CKAG2/KBS/Lib
' 
' *****************  Version 15  *****************
' User: Rudolpho     Date: 9.03.10    Time: 17:44
' Updated in $/CKAG2/KBS/Lib
' 
' *****************  Version 14  *****************
' User: Rudolpho     Date: 19.02.10   Time: 8:59
' Updated in $/CKAG2/KBS/Lib
' 
' *****************  Version 13  *****************
' User: Rudolpho     Date: 18.02.10   Time: 9:18
' Updated in $/CKAG2/KBS/Lib
' 
' *****************  Version 12  *****************
' User: Jungj        Date: 17.06.09   Time: 10:43
' Updated in $/CKAG2/KBS/Lib
' Nachbesserung 
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 3.06.09    Time: 16:53
' Updated in $/CKAG2/KBS/Lib
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 3.06.09    Time: 15:43
' Updated in $/CKAG2/KBS/Lib
' nachbesserungen
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 6.05.09    Time: 14:16
' Updated in $/CKAG2/KBS/Lib
' testrun entfernt wareneingang
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 5.05.09    Time: 12:58
' Updated in $/CKAG2/KBS/Lib
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 5.05.09    Time: 12:37
' Updated in $/CKAG2/KBS/Lib
' ITA 2838 testfertig
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 5.05.09    Time: 10:12
' Updated in $/CKAG2/KBS/Lib
' ITA 2838 kommentare 
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 4.05.09    Time: 17:35
' Updated in $/CKAG2/KBS/Lib
' ITA 2838
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 4.05.09    Time: 11:44
' Updated in $/CKAG2/KBS/Lib
' ITA 2838 unfertig
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 30.04.09   Time: 16:56
' Updated in $/CKAG2/KBS/Lib
' ITA 2838 unfertig
'
' ************************************************