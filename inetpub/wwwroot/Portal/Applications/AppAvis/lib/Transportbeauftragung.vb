Imports CKG.Base.Kernel

Public Class Transportbeauftragung
    Inherits CKG.Base.Common.SapError

#Region "Declarations"

#Region "Objects"
    Protected m_objLogApp As Base.Kernel.Logging.Trace
    Protected m_objApp As Base.Kernel.Security.App
    Protected m_objUser As Base.Kernel.Security.User
#End Region

#Region "Tables"

    ''' <summary>
    ''' Tabelle Fahrzeugbestand aktuell in SAP
    ''' </summary>
    Public tblSAPBestand As DataTable

    ''' <summary>
    ''' Tabelle Fahrzeuge warten auf Emailversand in SAP
    ''' </summary>
    Public tblSAPWaitingForEmail As DataTable

    ''' <summary>
    ''' Tabelle Summe warten auf Emailversand
    ''' </summary>
    Public tblSAPWaitingForEmailSumme As DataTable

    ''' <summary>
    ''' Tabelle Fahrzeugbestand der beauftragt werden soll
    ''' </summary>
    Public tblSAPBeauftragt As DataTable

    ''' <summary>
    ''' Tabelle Auslatungszahlen der Carports
    ''' </summary>
    Private tblSAPAuslastung As DataTable

    ''' <summary>
    ''' Kopftabelle für Beauftragung
    ''' </summary>
    Public tblWEBHead As DataTable

    ''' <summary>
    ''' Satztabelle für Beauftragung
    ''' </summary>
    Public tblWEBItem As DataTable

    ''' <summary>
    ''' Tabelle geändertes Anschreiben
    ''' </summary>
    Public tblWEBAnschreiben As DataTable

    ''' <summary>
    ''' Tabelle mit aufbereiter Übersicht
    ''' </summary>
    Public tblStatistik As DataTable

    ''' <summary>
    ''' Tabelle Stationsdaten zum aktuellen Stationcode
    ''' </summary>
    Public tblStation As DataTable

    ''' <summary>
    ''' Tabelle der Spedituere zum aktuellen Kunden
    ''' </summary> 
    Public tblSpediteure As DataTable

    ''' <summary>
    ''' Tabelle mit der aktuellen Auslastung der relevanten Stationen
    ''' </summary>
    Public tblAuslastung As DataTable

    ''' <summary>
    ''' Arbeitskopie der aktuellen Auslastungstabelle
    ''' </summary>
    Public tblAuslastungWork As DataTable

#End Region

#Region "Lists"
    ''' <summary>
    ''' Liste der Carports
    ''' </summary>
    Public lstCarports As New List(Of String)

    ''' <summary>
    ''' Liste der Hersteller
    ''' </summary>
    ''' <remarks></remarks>
    Public lstHersteller As New List(Of String)

    ''' <summary>
    ''' Liste der Vermietgruppe
    ''' </summary>
    Public lstVermietgruppe As New List(Of String)

    ''' <summary>
    ''' Liste der Kraftstoffe
    ''' </summary>
    Public lstKraftstoffe As New List(Of String)

    ''' <summary>
    ''' Liste der zu Versendenden Aufträge
    ''' </summary>
    Public lstAuftragMail As New List(Of String)
#End Region

#Region "Fields"
    Private m_KunNr As String = ""
    Private m_strClassAndMethod As String = "Transportbeauftragung"
    Private m_strAppID As String = ""
    Private m_strSessionID As String = ""

    ' aktuell gesetzte Filter und Selektionen
    Public strFilterCarport As String = ""
    Public strFilterHersteller As String = ""
    Public strFilterVermietergruppe As String = ""
    Public strFilterKraftstoffart As String = ""
    Public strAuftragsnummer As String = ""
    Public strSpediteur As String = ""

    Public DatumZulassungVon As Date? = Nothing
    Public DatumZulassungBis As Date? = Nothing
    Public DatumFreisetzungVon As Date? = Nothing
    Public DatumFreisetzungBis As Date? = Nothing

    Private DatumBeauftragung As String = ""
    Private UhrzeitBeauftragung As String = ""
    
    Public strDispo As String = "Dispo"
    Public strBeauftragt As String = "Beauftragt"
#End Region

#End Region
    
    Public Sub New(ByRef App As Base.Kernel.Security.App, ByRef User As Base.Kernel.Security.User)
        m_objApp = App
        m_objUser = User
        m_objLogApp = New Logging.Trace(App.Connectionstring, True)

        m_KunNr = m_objUser.KUNNR

        ' Statistiktabelle generieren
        tblStatistik = New DataTable
        tblStatistik.Columns.Add("Carport")
        tblStatistik.Columns.Add("NichtBeauftragt")
        tblStatistik.Columns.Add("Disponiert")
        tblStatistik.Columns.Add("Beauftragt")
        tblStatistik.AcceptChanges()

        FillSpediteure()
    End Sub

#Region "Fill-Methoden"

    ''' <summary>
    ''' Liefert den SAP-Fahrzeugbestand abhängig vom mitgegebenen Aktionsmodus
    ''' </summary>
    ''' <param name="actioncode">
    '''   <para>A - Bestand für Beauftragung,</para><br/>
    '''   <para>C - Bestand für Änderung/ Stornierung</para><br/>
    ''' <para>R - Bestand für Mailversand</para>
    ''' </param>
    Private Function GetBestandFromSap(ByVal actioncode As String) As DataTable
        ClearError()

        Try
            Select Case actioncode
                Case "A"
                    S.AP.Init("Z_DPM_AVIS_TRANSPBEAUF_001", "I_KUNNR_AG,I_ART,I_ZULDAT_VON,I_ZULDAT_BIS,I_FREI_VON,I_FREI_BIS",
                                     m_KunNr, actioncode, DatumZulassungVon, DatumZulassungBis, DatumFreisetzungVon, DatumFreisetzungBis)
                Case "C", "R"
                    S.AP.Init("Z_DPM_AVIS_TRANSPBEAUF_001", "I_KUNNR_AG,I_ART,I_AUFNR",
                                     m_KunNr, actioncode, strAuftragsnummer)
            End Select

            tblWEBHead = S.AP.GetImportTable("GT_IN_HEAD")
            tblWEBItem = S.AP.GetImportTable("GT_IN_ITEM")
            
            S.AP.Execute()

            If CInt(S.AP.GetExportParameter("E_SUBRC")) <> 0 Then
                RaiseError(CStr(S.AP.GetExportParameter("E_SUBRC")), CStr(S.AP.GetExportParameter("E_MESSAGE")))
            End If

            Dim tblSap = S.AP.GetExportTable("GT_OUT")
            tblSAPAuslastung = S.AP.GetExportTable("GT_OUT_SUMMEN")

            ' # Neue Spalten und Formatierungen ergänzen
            tblSap.Columns.Add("RowID", GetType(Integer)).DefaultValue = -1
            tblSap.Columns.Add("ZuBeauftragen", GetType(Boolean)).DefaultValue = False
            tblSap.Columns.Add("StatusT", GetType(String))

            For i = 0 To tblSap.Rows.Count - 1
                tblSap.Rows(i)("RowID") = i
                tblSap.Rows(i)("ZuBeauftragen") = False

                If CStr(tblSap.Rows(i)("UHRZEIT")) <> "" Then
                    ' Harte Formatierung, da Format-Funktion den String nicht erkennt
                    Dim temp = CStr(tblSap.Rows(i)("UHRZEIT"))
                    temp = temp.Insert(2, ":")
                    temp = temp.Insert(5, ":")
                    tblSap.Rows(i)("UHRZEIT") = temp
                End If

                TranslateStatus(tblSap.Rows(i), "StatusT")
            Next

            tblSap.AcceptChanges()
            ' #

            Return tblSap
        Catch ex As Exception
            RaiseError("W999", ex.Message)
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' Liefert den aktuellen Bestand ungefiltert zu den aktuellen Datumswerten
    ''' </summary>
    ''' <param name="AppID"></param>
    ''' <param name="SessionID"></param>
    Public Sub FillBestand(ByVal AppID As String, ByVal SessionID As String)

        m_strClassAndMethod = "Transportbeauftragung.FillBestand"
        m_strAppID = AppID
        m_strSessionID = SessionID

        strAuftragsnummer = ""

        ' Auslastungsstatistik
        tblSAPBestand = GetBestandFromSap("A")
        FillAuslastung()

        ' Bearbeitungsliste erstellen
        CreateListsFromBestand(tblSAPBestand)
    End Sub

    ''' <summary>
    ''' Liefert den aktuellen Bestand gefiltert auf Einträge ohne Email-Versanddatum zu den aktuellen Datumswerten
    ''' </summary>
    ''' <param name="AppID"></param>
    ''' <param name="SessionID"></param>
    Public Sub FillBestandWaitingForEmail(Optional ByVal AppID As String = "", Optional ByVal SessionID As String = "")

        m_strClassAndMethod = "Transportbeauftragung.FillBestandWaitingForEmail"
        m_strAppID = AppID
        m_strSessionID = SessionID

        tblSAPWaitingForEmail = GetBestandFromSap("R")
        BuildtblSAPWaitingForEmailSumme()
    End Sub

    ''' <summary>
    ''' Baut die Summentabelle für Aufträge ohne Emailversand auf
    ''' </summary>
    Private Sub BuildtblSAPWaitingForEmailSumme()
        tblSAPWaitingForEmailSumme = New DataTable()
        With tblSAPWaitingForEmailSumme
            .Columns.Add("AUF_NEUW_TRANSP", Type.GetType("System.String"))
            .Columns.Add("PDISTANDORT", Type.GetType("System.String"))
            .Columns.Add("SPEDITEUR", Type.GetType("System.String"))
            .Columns.Add("ANLDAT", Type.GetType("System.DateTime"))
            .Columns.Add("DATMAIL", Type.GetType("System.DateTime"))
            .Columns.Add("WEBUSER", Type.GetType("System.String"))
            .Columns.Add("EX_KUNNR", Type.GetType("System.String"))
            .Columns.Add("BEAUFDAT", Type.GetType("System.DateTime"))
            .Columns.Add("Summe", Type.GetType("System.Int32"))
        End With

        For Each row As DataRow In tblSAPWaitingForEmail.Rows

            Dim found = False

            For i = 0 To tblSAPWaitingForEmailSumme.Rows.Count - 1
                Dim row2 As DataRow = tblSAPWaitingForEmailSumme.Rows(i)

                If CStr(row2("AUF_NEUW_TRANSP")) = CStr(row("AUF_NEUW_TRANSP")) Then
                    found = True
                    row2("Summe") = CInt(row2("Summe")) + 1
                    Exit For
                End If
            Next

            If Not found Then
                Dim nRow = tblSAPWaitingForEmailSumme.NewRow()

                nRow("AUF_NEUW_TRANSP") = row("AUF_NEUW_TRANSP")
                nRow("PDISTANDORT") = row("PDISTANDORT")
                nRow("SPEDITEUR") = row("SPEDITEUR")
                nRow("ANLDAT") = row("ANLDAT")
                nRow("DATMAIL") = row("DATMAIL")
                nRow("WEBUSER") = row("WEBUSER")
                nRow("EX_KUNNR") = row("EX_KUNNR")
                nRow("BEAUFDAT") = row("BEAUFDAT")
                nRow("Summe") = 1

                tblSAPWaitingForEmailSumme.Rows.Add(nRow)

            End If

        Next

    End Sub

    ''' <summary>
    ''' Liefert den aktuellen Bestand der geändert oder storniert werden kann ungefiltert zu den aktuellen Datumswerten
    ''' </summary>
    ''' <param name="AppID"></param>
    ''' <param name="SessionID"></param>
    Public Function FillBestandStornoAenderung(ByVal AppID As String, ByVal SessionID As String) As DataTable

        m_strClassAndMethod = "Transportbeauftragung.FillBestandStornoAenderung"
        m_strAppID = AppID
        m_strSessionID = SessionID

        ' Auslastungsstatistik
        tblSAPBestand = GetBestandFromSap("C")
        FillAuslastung()

        ' Bearbeitungsliste erstellen
        'CreateListsFromBestand(tblSAPBestand)

        Return tblSAPBestand
    End Function

    ''' <summary>
    ''' Liefert eine Liste der beauftragten Fahrzeuge zur Auftragsnummer
    ''' </summary>
    ''' <param name="AppID"></param>
    ''' <param name="SessionID"></param>
    ''' <returns>Liste der beauftragten Fahrzeuge</returns>
    Public Function FillBeauftragung(ByVal AppID As String, ByVal SessionID As String) As DataView

        Try
            FillBestand(AppID, SessionID)
            Dim TempDV As DataView = tblSAPBestand.DefaultView
            If strAuftragsnummer = String.Empty Then
                TempDV.RowFilter = "AUF_NEUW_TRANSP is NULL"
            Else
                TempDV.RowFilter = "AUF_NEUW_TRANSP='" & strAuftragsnummer & "'"
            End If

            tblSAPBeauftragt = TempDV.Table
            Return TempDV
        Catch ex As Exception
            RaiseError("W999", ex.Message)
        End Try

        Return Nothing
    End Function

    ''' <summary>
    ''' Befüllt die Kopftabelle für eine neue Beauftragung
    ''' </summary>
    ''' <param name="stationscode">Stationscode</param>
    ''' <param name="name1">NAME1 Länge 35 Zeichen</param>
    ''' <param name="name2">NAME2 Länge 35 Zeichen</param>
    ''' <param name="straße">Straße Länge 35 Zeichen</param>
    ''' <param name="hausnr">Hausnummer Länge 10 Zeichen</param>
    ''' <param name="plz">Postleitzahl Länge 10 Zeichen</param>
    ''' <param name="ort">Ort Länge 40 Zeichen</param>
    ''' <param name="freitext">FREITEXT Länge 255 Zeichen</param>
    ''' <param name="beaufdat">Beauftragungsdatum</param>
    ''' <param name="uhrzeit">Uhrzeit Länge 6 Zeichen</param>
    ''' <param name="webuser">Name des aktuellen Webusers Länge 40 Zeichen</param>
    ''' <param name="emailwebuser">E-Mail-Adresse des Webusers Länge 241 Zeichen</param>
    Sub FilltblWebHead(ByVal stationscode As String, ByVal name1 As String, ByVal name2 As String, ByVal straße As String, ByVal hausnr As String,
                       ByVal plz As String, ByVal ort As String, ByVal freitext As String, ByVal beaufdat As Date?, ByVal uhrzeit As Date?,
                       ByVal webuser As String, ByVal emailwebuser As String)

        Dim row = tblWEBHead.NewRow()

        row("EX_KUNNR") = stationscode
        row("NAME1") = name1
        row("NAME2") = name2
        row("STRAS") = straße
        row("HAUSNR") = hausnr
        row("PLZ") = plz
        row("ORT") = ort
        row("FREITEXT") = freitext
        row("CARPORT") = strFilterCarport
        row("WEBUSER") = webuser
        row("EMAIL_WEBUSER") = emailwebuser

        tblWEBHead.Rows.Add(row)
        tblWEBHead.AcceptChanges()

        ' Datumswerte für die Items setzen
        If beaufdat.HasValue AndAlso beaufdat.Value.Year > 1900 Then
            DatumBeauftragung = beaufdat.Value.ToShortDateString()
        Else
            DatumBeauftragung = ""
        End If

        If Not uhrzeit Is Nothing Then
            UhrzeitBeauftragung = Right("00" & CDate(uhrzeit).Hour.ToString, 2) &
                Right("00" & CDate(uhrzeit).Minute.ToString, 2) &
                Right("00" & CDate(uhrzeit).Second.ToString, 2)
        Else
            UhrzeitBeauftragung = ""
        End If

    End Sub

    ''' <summary>
    ''' Baut die Statistiktabelle der Carportbeauftragungen aus 
    ''' der Liste der Carport und der tblSAPBestand auf.
    ''' </summary>
    Public Sub FillStatistik()
        ClearError()

        Try
            tblStatistik.Rows.Clear()

            For Each strCarport In lstCarports
                Dim newRow = tblStatistik.NewRow()

                newRow("Carport") = strCarport

                If tblSAPBestand IsNot Nothing Then
                    ' Carport, nicht disponiert, keine Mail
                    Dim iNichtBeauftragt = tblSAPBestand.Select("Carport='" & strCarport & "' AND BEAUFDAT IS NULL AND DATMAIL IS NULL").GetLength(0)
                    ' Carport, disponiert, keine Mail
                    Dim iDispo = tblSAPBestand.Select("Carport='" & strCarport & "' AND BEAUFDAT IS NOT NULL AND DATMAIL IS NULL").GetLength(0)
                    ' Carport, disponiert, Mail
                    Dim iBeauf = tblSAPBestand.Select("Carport='" & strCarport & "' AND BEAUFDAT IS NOT NULL AND DATMAIL IS NOT NULL").GetLength(0)

                    newRow("NichtBeauftragt") = iNichtBeauftragt
                    newRow("Disponiert") = iDispo
                    newRow("Beauftragt") = iBeauf
                End If

                tblStatistik.Rows.Add(newRow)
                tblStatistik.AcceptChanges()
            Next
        Catch ex As Exception
            RaiseError("W999", ex.Message)
        End Try

    End Sub

    ''' <summary>
    ''' Füllt die Auslastungstabelle anhand der SAP Summen
    ''' </summary>
    Public Sub FillAuslastung()
        ClearError()

        Try
            tblAuslastung = New DataTable
            tblAuslastung.PrimaryKey = {tblAuslastung.Columns.Add("STATION", GetType(String))}

            For Each row As DataRow In tblSAPAuslastung.Rows

                Dim sStation = CStr(row("EX_KUNNR"))
                Dim sDatum = ""
                If Not TypeOf row("BEAUFDAT") Is DBNull Then
                    sDatum = CStr(row("BEAUFDAT"))
                    If Not tblAuslastung.Columns.Contains(sDatum + " " + strBeauftragt) Then
                        tblAuslastung.Columns.Add(sDatum + " " + strDispo, GetType(Integer))
                        tblAuslastung.Columns.Add(sDatum + " " + strBeauftragt, GetType(Integer))

                        For Each rowAusl As DataRow In tblAuslastung.Rows
                            rowAusl(sDatum + " " + strDispo) = 0
                            rowAusl(sDatum + " " + strBeauftragt) = 0
                        Next

                        tblAuslastung.AcceptChanges()
                    End If
                End If

                If Not tblAuslastung.Rows.Contains(sStation) Then
                    Dim nRow = tblAuslastung.NewRow()

                    For Each columnAusl As DataColumn In tblAuslastung.Columns
                        nRow(columnAusl.ColumnName) = 0
                    Next

                    nRow("STATION") = sStation
                    tblAuslastung.Rows.Add(nRow)
                    tblAuslastung.AcceptChanges()
                End If

                Dim rowFind As DataRow = tblAuslastung.Rows.Find(sStation)

                If sDatum <> "" Then
                    rowFind(sDatum + " " + strDispo) = CInt(row("ANZAHL_DISPO"))
                    rowFind(sDatum + " " + strBeauftragt) = CInt(row("ANZAHL"))
                End If

            Next

            tblAuslastung.AcceptChanges()

            ColumnReorder(tblAuslastung)

            ResetTblAuslastungWork()
        Catch ex As Exception
            RaiseError("W999", ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Holt die Daten zu einem bestimmten Stationscode
    ''' </summary>
    Public Sub FillStationsDaten(ByVal strSationsCode As String)
        ClearError()
        Try
            S.AP.InitExecute("Z_DPM_UF_STATION_LESEN", "I_AG,I_STATION", m_KunNr, strSationsCode)

            tblStation = S.AP.GetExportTable("ES_STATION")

            If CInt(S.AP.GetExportParameter("E_SUBRC")) <> 0 Then
                RaiseError(CStr(S.AP.GetExportParameter("E_SUBRC")), CStr(S.AP.GetExportParameter("E_MESSAGE")))
            End If

        Catch ex As Exception
            RaiseError("W999", ex.Message)
        End Try
    End Sub

    Public Sub FillSpediteure()
        ClearError()

        Try
            S.AP.InitExecute("Z_DPM_READ_TRANSPORT_SPEDITEUR", "I_KUNNR", m_KunNr.PadLeft(10, "0"c))

            tblSpediteure = S.AP.GetExportTable("GT_OUT")

        Catch ex As Exception
            RaiseError("W999", ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Liefert alle Daten für den Übersichtsreport zum aktuellen Kunden
    ''' </summary>
    ''' <param name="AppID"></param>
    ''' <param name="SessionID"></param>
    Public Sub FillReportBeauftragung(ByVal AppID As String, ByVal SessionID As String, ByVal Carport As String, ByVal Distrikt As String)

        ClearError()

        m_strClassAndMethod = "Transportbeauftragung.FillReportBeauftragung"
        m_strAppID = AppID
        m_strSessionID = SessionID

        Try
            S.AP.InitExecute("Z_DPM_AVIS_READ_TRANBEAUF_001", "I_KUNNR_AG,I_CARPORT,I_DISTRIKT,I_ZULDAT_VON,I_ZULDAT_BIS",
                             Right("0000000000" & m_KunNr, 10), Carport, Distrikt, DatumZulassungVon, DatumZulassungBis)

            tblSAPBestand = S.AP.GetExportTable("GT_OUT")

            If CInt(S.AP.GetExportParameter("E_SUBRC")) <> 0 Then
                RaiseError(CStr(S.AP.GetExportParameter("E_SUBRC")), CStr(S.AP.GetExportParameter("E_MESSAGE")))
            End If

            For i = 0 To tblSAPBestand.Rows.Count - 1
                If CStr(tblSAPBestand.Rows(i)("UHRZEIT")) <> "" Then
                    ' Harte Formatierung, da Format-Funktion den String nicht erkennt
                    Dim temp = CStr(tblSAPBestand.Rows(i)("UHRZEIT"))
                    temp = temp.Insert(2, ":")
                    temp = temp.Insert(5, ":")
                    tblSAPBestand.Rows(i)("UHRZEIT") = temp
                End If
            Next

            tblSAPBestand.AcceptChanges()

        Catch ex As Exception
            RaiseError("W999", ex.Message)
        End Try
    End Sub

#End Region

#Region "Datenmanipulation"

    ''' <summary>
    ''' Baut Filterlisten und Auslastungstabelle auf.
    ''' </summary>
    ''' <param name="tblBestand">tblSAPBestand</param>
    ''' <param name="refreshCarport">Carportliste aktualisieren? Ja/Nein default:Ja</param>
    Private Sub CreateListsFromBestand(ByRef tblBestand As DataTable, Optional ByVal refreshCarport As Boolean = True)
        If tblBestand IsNot Nothing Then
            If refreshCarport Then
                lstCarports.Clear()
            End If
            lstHersteller.Clear()
            lstKraftstoffe.Clear()
            lstVermietgruppe.Clear()

            For Each row As DataRow In tblBestand.Rows
                ' Listen
                If refreshCarport AndAlso Not lstCarports.Contains(CStr(row("Carport"))) Then
                    lstCarports.Add(CStr(row("Carport")))
                End If

                If Not lstHersteller.Contains(CStr(row("HERST_NUMMER"))) Then
                    lstHersteller.Add(CStr(row("HERST_NUMMER")))
                End If

                If Not lstKraftstoffe.Contains(CStr(row("KRAFTSTOFF"))) Then
                    lstKraftstoffe.Add(CStr(row("KRAFTSTOFF")))
                End If

                If Not lstVermietgruppe.Contains(CStr(row("VERMIET_GRP"))) Then
                    lstVermietgruppe.Add(CStr(row("VERMIET_GRP")))
                End If
            Next

            ' Sortierung
            lstCarports.Sort(New IntStringComparer())
            lstHersteller.Sort(Function(str1, str2) String.Compare(str1, str2, StringComparison.Ordinal))
            lstKraftstoffe.Sort(Function(str1, str2) String.Compare(str1, str2, StringComparison.Ordinal))
            lstVermietgruppe.Sort(Function(str1, str2) String.Compare(str1, str2, StringComparison.Ordinal))

        End If
    End Sub

    ''' <summary>
    ''' aktualisiert die Filterlisten
    ''' </summary>
    Public Sub RefreshLists()
        If tblSAPBestand IsNot Nothing Then
            CreateListsFromBestand(GetFilterBestand().ToTable, False)
        Else
            CreateListsFromBestand(tblSAPBestand, False)
        End If
    End Sub

    ''' <summary>
    ''' Liefert den Bestand als gefiltert DataView. Es wird mit den aktuellen Filtern der Klasse und nach leerem Beauftragunsdatum gefiltert.
    ''' </summary>
    ''' <returns>Liefert den gefilterten Bestand</returns>
    Public Function GetFilterBestand() As DataView
        Dim DV As DataView = tblSAPBestand.DefaultView

        Dim strFilterExpression = "DATMAIL IS NULL" 'Nur unbeauftragte Daten

        If strFilterCarport <> "" Then
            If strFilterExpression <> "" Then
                strFilterExpression &= " AND "
            End If
            strFilterExpression &= "CARPORT='" & strFilterCarport & "'"
        End If

        If strFilterHersteller <> "" Then
            If strFilterExpression <> "" Then
                strFilterExpression &= " AND "
            End If
            strFilterExpression &= "HERST_NUMMER='" & strFilterHersteller & "'"
        End If

        If strFilterVermietergruppe <> "" Then
            If strFilterExpression <> "" Then
                strFilterExpression &= " AND "
            End If
            strFilterExpression &= "VERMIET_GRP='" & strFilterVermietergruppe & "'"
        End If

        If strFilterKraftstoffart <> "" Then
            If strFilterExpression <> "" Then
                strFilterExpression &= " AND "
            End If
            strFilterExpression &= "KRAFTSTOFF='" & strFilterKraftstoffart & "'"
        End If

        DV.RowFilter = strFilterExpression

        Return DV
    End Function

    ''' <summary>
    ''' Liefert den Bestand ohne Mailversand als gefilterte DataView. Es wird mit der Auftragsnummer gefiltert.
    ''' </summary>
    ''' <returns>Liefert den gefilterten Bestand</returns>
    Public Function GetFilterBestandMail() As DataView
        Dim dv As DataView = tblSAPWaitingForEmail.DefaultView
        dv.RowFilter = "AUF_NEUW_TRANSP = " + strAuftragsnummer
        Return dv
    End Function
    
    ''' <summary>
    ''' Setzt den Stand der Arbeitstabelle auf den Ursprungszustand zurück
    ''' </summary>
    Public Sub ResetTblAuslastungWork()
        tblAuslastungWork = tblAuslastung.Copy()
    End Sub

    ''' <summary>
    ''' Ordnet die letzte Spalte in das aktuelle Spaltenschema ein
    ''' </summary>
    ''' <param name="tblAuslast">Auslastungstabelle die geordnet werden soll</param>
    Public Shared Sub ColumnReorder(ByRef tblAuslast As DataTable)
        If tblAuslast IsNot Nothing And tblAuslast.Columns.Count > 0 Then
            Dim arColumns(tblAuslast.Columns.Count - 1) As DataColumn
            tblAuslast.Columns.CopyTo(arColumns, 0)

            Array.Sort(arColumns, 1, arColumns.GetLength(0) - 1, New DataColumnComparer())

            For i = 0 To arColumns.GetLength(0) - 1
                tblAuslast.Columns(arColumns(i).ColumnName).SetOrdinal(i)
            Next
        End If
        
    End Sub

#End Region


#Region "Daten senden"

    ''' <summary>
    ''' Startet einen SAP-Aufruf für die Fahrzeugdaten
    ''' </summary>
    ''' <param name="actioncode">Steuerwert für SAP Abfrage</param>
    Private Function SendFahrzeuge(ByVal actioncode As String) As DataTable
        ClearError()

        Try
            S.AP.Init("Z_DPM_AVIS_TRANSPBEAUF_001", "I_KUNNR_AG,I_ART,I_AUFNR", m_KunNr, actioncode, strAuftragsnummer)

            Dim tblHead = S.AP.GetImportTable("GT_IN_HEAD")
            Dim tblItem = S.AP.GetImportTable("GT_IN_ITEM")

            If actioncode = "M" Then
                ' Kopfdaten werden nicht gesendet
                Create_tblWEBItemMail(tblItem)
            Else
                tblHead.Merge(tblWEBHead)
                Create_tblWEBItem(tblItem)
            End If

            S.AP.Execute()

            If CInt(S.AP.GetExportParameter("E_SUBRC")) <> 0 Then
                RaiseError(CStr(S.AP.GetExportParameter("E_SUBRC")), CStr(S.AP.GetExportParameter("E_MESSAGE")))
            End If

            Dim tblSap = S.AP.GetExportTable("GT_OUT")

            tblSAPAuslastung = S.AP.GetExportTable("GT_OUT_SUMMEN")

            ' # Neue Spalten und Formatierungen ergänzen
            tblSap.Columns.Add("RowID", GetType(Integer)).DefaultValue = -1
            tblSap.Columns.Add("ZuBeauftragen", GetType(Boolean)).DefaultValue = False
            tblSap.Columns.Add("StatusT", GetType(String))

            For i = 0 To tblSap.Rows.Count - 1
                tblSap.Rows(i)("RowID") = i
                tblSap.Rows(i)("ZuBeauftragen") = False

                If CStr(tblSap.Rows(i)("UHRZEIT")) <> "" Then
                    ' Harte Formatierung, da Format-Funktion den String nicht erkennt
                    Dim temp = CStr(tblSap.Rows(i)("UHRZEIT"))
                    temp = temp.Insert(2, ":")
                    temp = temp.Insert(5, ":")
                    tblSap.Rows(i)("UHRZEIT") = temp
                End If

                TranslateStatus(tblSap.Rows(i), "StatusT")
            Next

            tblSap.AcceptChanges()
            ' #

            Return tblSap
        Catch ex As Exception
            RaiseError("W999", ex.Message)
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Sendet die aktuellen Daten der Head und Item-Tabellen an SAP im Modus "Beauftragen"
    ''' </summary>
    Public Sub FahrzeugeBeauftragen()
        ' Auslastungsstatistik
        tblSAPBestand = SendFahrzeuge("B")
        FillAuslastung()
        
        ' Bearbeitungsliste erstellen
        CreateListsFromBestand(tblSAPBestand)
    End Sub

    ''' <summary>
    ''' Sendet die aktuellen Daten der Item-Tabelle an SAP im Modus "Email versenden"
    ''' </summary>
    Public Sub FahrzeugeMailVersenden()

        For Each s As String In lstAuftragMail
            strAuftragsnummer = s
            SendFahrzeuge("M")
        Next
        
        ' Tabellen refreshen
        FillBestandWaitingForEmail()
    End Sub

    ''' <summary>
    ''' Sendet die aktuellen Daten der Head und Item-Tabellen an SAP im Modus "Stornieren"
    ''' </summary>
    Public Sub FahrzeugeStornieren()
        ' Auslastungsstatistik
        tblSAPBestand = SendFahrzeuge("S")
        FillAuslastung()

        ' Bearbeitungsliste erstellen
        CreateListsFromBestand(tblSAPBestand)
    End Sub

    ''' <summary>
    ''' Sendet die aktuellen Daten der Head und Item-Tabellen an SAP im Modus "Ändern"
    ''' </summary>
    Public Sub FahrzeugeAendern()
        ' Auslastungsstatistik
        tblSAPBestand = SendFahrzeuge("D")
        FillAuslastung()

        ' Bearbeitungsliste erstellen
        CreateListsFromBestand(tblSAPBestand)
    End Sub

    ''' <summary>
    ''' Baut aus der aktuell gefilterten Ansicht des Bestands die WebImport Tabelle der Positionen auf
    ''' </summary>
    Private Sub Create_tblWEBItem(ByRef PosTable As DataTable)
        Dim Rows = tblSAPBestand.Select("ZuBeauftragen=true") 'AND BEAUFDAT is Null

        For Each row As DataRow In Rows
            Dim newRow = PosTable.NewRow()
            For Each col As DataColumn In PosTable.Columns
                If col.ColumnName = "BEAUFDAT" Then
                    newRow(col.ColumnName) = DatumBeauftragung
                ElseIf col.ColumnName = "UHRZEIT" Then
                    newRow(col.ColumnName) = UhrzeitBeauftragung
                ElseIf col.ColumnName = "SPEDITEUR" Then
                    newRow(col.ColumnName) = strSpediteur
                Else
                    newRow(col.ColumnName) = row(col.ColumnName)
                End If
            Next
            PosTable.Rows.Add(newRow)
        Next

        PosTable.AcceptChanges()
        tblWEBItem = PosTable
    End Sub

    ''' <summary>
    ''' Baut aus der aktuell gefilterten Ansicht des Bestands die WebImport Tabelle der Positionen auf
    ''' </summary>
    Private Sub Create_tblWEBItemMail(ByRef PosTable As DataTable)
        
        Try
            For Each auftragsnummer As String In lstAuftragMail
                Dim rows = tblSAPWaitingForEmail.Select("AUF_NEUW_TRANSP = " + auftragsnummer)

                For Each row As DataRow In rows
                    Dim newRow = PosTable.NewRow()
                    For Each col As DataColumn In PosTable.Columns
                        newRow(col.ColumnName) = row(col.ColumnName)
                    Next
                    PosTable.Rows.Add(newRow)
                Next

                PosTable.AcceptChanges()
                tblWEBItem = PosTable
            Next
        Catch ex As Exception
            Dim strError = "Fehler beim Aufbau der Tabelle WEBItemMail"

#If DEBUG Then
            strError &= ex.Message
#End If
            RaiseError("9999", strError)
        End Try

    End Sub

#End Region


#Region "Helper"

    ''' <summary>
    ''' Setzt die Filterwerte für die Tabellen zurück
    ''' </summary>
    Public Sub ClearFilter()
        strFilterCarport = ""
        strFilterHersteller = ""
        strFilterVermietergruppe = ""
        strFilterKraftstoffart = ""
    End Sub

    ''' <summary>
    ''' Übersetzt die Werte des Status Feld
    ''' </summary>
    ''' <param name="row">Die zu übersetzende Zeile</param>
    ''' <param name="targetcolumn">Ausgabespalte für das Ergebnis</param>
    Private Sub TranslateStatus(row As DataRow, targetcolumn As String)
        Dim value = CStr(row("STATUS"))

        Select Case value
            Case "01"
                row(targetcolumn) = "Auftrag erteilt"
            Case "02"
                row(targetcolumn) = "Auftrag geändert"
            Case "03"
                row(targetcolumn) = "Auftrag storniert"
            Case "04"
                row(targetcolumn) = "Auftrag erledigt"
            Case "05"
                row(targetcolumn) = "Auftrag verzögert"
            Case "06"
                row(targetcolumn) = "Selbstabholer"
            Case "07"
                row(targetcolumn) = "Sonstige Destination"
        End Select
    End Sub

    ''' <summary>
    ''' Vergleicher-Klasse für Strings, die Zahlen enthalten
    ''' </summary>
    Private Class IntStringComparer
        Implements IComparer(Of String)

        Public Function Compare(ByVal one As String, ByVal other As String) As Integer Implements IComparer(Of String).Compare
            Try
                If one Is Nothing And other Is Nothing Then
                    Return 0
                ElseIf one Is Nothing And other IsNot Nothing Then
                    Return -1
                ElseIf one IsNot Nothing And other Is Nothing Then
                    Return 1
                Else
                    If one.Length > other.Length Then
                        Return 1
                    ElseIf one.Length < other.Length Then
                        Return -1
                    Else
                        If CInt(one) < CInt(other) Then
                            Return -1
                        ElseIf CInt(one) > CInt(other) Then
                            Return 1
                        Else
                            Return 0
                        End If
                    End If
                End If
            Catch ex As Exception
                Return 0
            End Try
        End Function

    End Class

    ''' <summary>
    ''' Vergleicher-Klasse für DataColumns
    ''' </summary>
    Private Class DataColumnComparer
        Implements IComparer(Of DataColumn)

        Public Function Compare(ByVal one As DataColumn, ByVal other As DataColumn) As Integer Implements IComparer(Of DataColumn).Compare
            Try
                If one Is Nothing And other Is Nothing Then
                    Return 0
                ElseIf one Is Nothing And other IsNot Nothing Then
                    Return -1
                ElseIf one IsNot Nothing And other Is Nothing Then
                    Return 1
                Else
                    'Beauftragt-Spalten sollen jeweils hinter den Dispo-Spalten stehen
                    Dim col1TempName As String = one.ColumnName.Replace("Beauftragt", "ZBeauftragt")
                    Dim col2TempName As String = other.ColumnName.Replace("Beauftragt", "ZBeauftragt")
                    Return String.Compare(col1TempName, col2TempName)
                End If
            Catch ex As Exception
                Return 0
            End Try
        End Function

    End Class

#End Region

End Class




