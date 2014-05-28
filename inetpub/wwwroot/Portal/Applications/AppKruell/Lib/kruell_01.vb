Option Explicit On
Option Strict On

Imports System
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization
Imports System.Configuration
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports Microsoft.Data.SAPClient
Public Class kruell_01
    Inherits Base.Business.DatenimportBase
    Private strfahrgestellnummer As String
    Private strLeasinggeber As String
    Private strordernummer As String
    Private strDatumVon As String
    Private strDatumBis As String
    Private dtSapFehlerTable As DataTable
    Private dtSapDatenTable As DataTable
    Private boolDataChanged As Boolean = False
    Private alDisabledFieldIDs As New ArrayList()
    Private alBearbeitungsControlsWithSort As New ArrayList()
    Private dtSAPVorschlagswerte As DataTable
    Private strSAPResultMeldung As String = ""
    Private dtEinbau As New DataTable()
    Private alHtmlFormulare As New ArrayList()
    Private SAPearlyPrintTable As New DataTable
    Dim strOrderNR As String ' für Anzeige der Ordernummer bei Exception


#Region " Properties "

    Public Property Einbau() As DataTable
        Get
            Return dtEinbau
        End Get
        Set(ByVal value As DataTable)
            dtEinbau = value
        End Set
    End Property



    Public ReadOnly Property SAPResultat() As String
        Get
            Return strSAPResultMeldung
        End Get

    End Property

    Public ReadOnly Property SAPgetSAPRowForEarlyPrinting(ByVal Ordernummer As String) As DataRow
        Get
            Dim tmpRow As DataRow
            'Dim tmpSAPRow As SAPProxy_Kruell.ZDAD_AUFTR_IMP_001
            For Each tmpRow In SAPearlyPrintTable.Rows
                If CStr(tmpRow("ORDER_NR")) = Ordernummer Then
                    Return tmpRow
                End If
            Next
        End Get
    End Property
    Public Property dataChanged() As Boolean
        Get
            Return boolDataChanged
        End Get
        Set(ByVal Value As Boolean)
            boolDataChanged = Value
        End Set
    End Property
    Public Property Leasinggeber() As String
        Get
            Return strLeasinggeber
        End Get
        Set(ByVal value As String)
            strLeasinggeber = value
        End Set
    End Property


    Public Property DatumVon() As String
        Get
            Return strDatumVon
        End Get
        Set(ByVal value As String)
            strDatumVon = value
        End Set
    End Property

    Public Property DatenTabelle() As DataTable
        Get
            Return dtSapDatenTable
        End Get
        Set(ByVal value As DataTable)
            dtSapDatenTable = value
        End Set
    End Property
    Public ReadOnly Property Vorschlagswerte() As DataTable
        Get
            Return dtSAPVorschlagswerte
        End Get
    End Property

    Public Property FehlerTabelle() As DataTable
        Get
            Return dtSapFehlerTable
        End Get
        Set(ByVal value As DataTable)
            dtSapFehlerTable = value
        End Set
    End Property



    Public Property DatumBis() As String
        Get
            Return strDatumBis
        End Get
        Set(ByVal value As String)
            strDatumBis = value
        End Set
    End Property

    Public Property fahrgestellnummer() As String
        Get
            Return strfahrgestellnummer
        End Get
        Set(ByVal value As String)
            strfahrgestellnummer = value
        End Set
    End Property

    Public Property Ordernummer() As String
        Get
            Return strordernummer
        End Get
        Set(ByVal value As String)
            If Not value Is Nothing Then
                strordernummer = value.Trim(" "c).ToUpper
            End If

        End Set
    End Property


    Public ReadOnly Property htmlFormulare() As ArrayList
        Get
            Return alHtmlFormulare
        End Get
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
        dtEinbau.Columns.Add("Item", strFilename.GetType)
        dtEinbau.Columns.Add("Beschreibung", strFilename.GetType)
    End Sub

    Public Overloads Overrides Sub Fill()
        Fill(m_strAppID, m_strSessionID)
        'System.Diagnostics.Debug.Write("" & m_tblResult.Rows.Count)
        'System.Diagnostics.Debug.Write("" & ResultTable.Rows.Count)

    End Sub


    Private Function createTable() As DataTable

        Dim sapTable As New DataTable

        Dim dataColumns(53) As DataColumn

        dataColumns(0) = New DataColumn("KUNNR_LN", System.Type.GetType("System.String"))
        dataColumns(1) = New DataColumn("NAME_LN", System.Type.GetType("System.String"))
        dataColumns(2) = New DataColumn("STR_HNR_LN", System.Type.GetType("System.String"))
        dataColumns(3) = New DataColumn("PLZ_LN", System.Type.GetType("System.String"))
        dataColumns(4) = New DataColumn("STADT_LN", System.Type.GetType("System.String"))

        dataColumns(5) = New DataColumn("AP_NAME1_LN", System.Type.GetType("System.String"))
        dataColumns(6) = New DataColumn("AP_NAME2_LN", System.Type.GetType("System.String"))
        dataColumns(7) = New DataColumn("TEL_LN", System.Type.GetType("System.String"))
        dataColumns(8) = New DataColumn("FAX_LN", System.Type.GetType("System.String"))
        dataColumns(9) = New DataColumn("HANDY_LN", System.Type.GetType("System.String"))

        dataColumns(10) = New DataColumn("SONST_LN", System.Type.GetType("System.String"))
        dataColumns(11) = New DataColumn("ORDER_NR", System.Type.GetType("System.String"))
        dataColumns(12) = New DataColumn("CAR_TYPE", System.Type.GetType("System.String"))
        dataColumns(13) = New DataColumn("CHASSIS_NUM", System.Type.GetType("System.String"))
        dataColumns(14) = New DataColumn("CAR_BEM", System.Type.GetType("System.String"))

        dataColumns(15) = New DataColumn("CAR_BESTNR", System.Type.GetType("System.String"))
        dataColumns(16) = New DataColumn("CAR_LICENSE_NUM", System.Type.GetType("System.String"))
        dataColumns(17) = New DataColumn("CAR_RETURN", System.Type.GetType("System.String"))
        dataColumns(18) = New DataColumn("LIC_NUM_RETURN", System.Type.GetType("System.String"))

        'dataColumns(19) = New DataColumn("ZZSPERR_DAD", System.Type.GetType("System.String"))
        'dataColumns(20) = New DataColumn("ZZINSOLVENZ", System.Type.GetType("System.String"))
        'dataColumns(21) = New DataColumn("TEXT50", System.Type.GetType("System.String"))
        'dataColumns(22) = New DataColumn("TEXT200", System.Type.GetType("System.String"))
        'dataColumns(23) = New DataColumn("REPLA_DATE", System.Type.GetType("System.String"))

        dataColumns(19) = New DataColumn("CAR_RET_BESCH", System.Type.GetType("System.String"))
        dataColumns(20) = New DataColumn("CAR_RET_ADR", System.Type.GetType("System.String"))
        dataColumns(21) = New DataColumn("ZUL_DIENST", System.Type.GetType("System.String"))
        dataColumns(22) = New DataColumn("CAR_TANK", System.Type.GetType("System.String"))
        dataColumns(23) = New DataColumn("VERS", System.Type.GetType("System.String"))

        dataColumns(24) = New DataColumn("AUSSTATT", System.Type.GetType("System.String"))
        dataColumns(25) = New DataColumn("SONST_OPT", System.Type.GetType("System.String"))
        dataColumns(26) = New DataColumn("GEW_VERBR_DAT", System.Type.GetType("System.String"))
        dataColumns(27) = New DataColumn("FA_AUFBER", System.Type.GetType("System.String"))

        dataColumns(28) = New DataColumn("AUFBER_ART", System.Type.GetType("System.String"))
        dataColumns(29) = New DataColumn("FA_ZUSEINBAUT", System.Type.GetType("System.String"))
        dataColumns(30) = New DataColumn("AUFBER_POS", System.Type.GetType("System.String"))
        dataColumns(31) = New DataColumn("SONDEINB_POS", System.Type.GetType("System.String"))


        dataColumns(32) = New DataColumn("ZUL_ART", System.Type.GetType("System.String"))
        dataColumns(33) = New DataColumn("ABW_HALTER", System.Type.GetType("System.String"))
        dataColumns(34) = New DataColumn("W_KENNZ_RES", System.Type.GetType("System.String"))
        dataColumns(35) = New DataColumn("W_KENNZ", System.Type.GetType("System.String"))


        dataColumns(36) = New DataColumn("ABW_LIEF_ADR", System.Type.GetType("System.String"))
        dataColumns(37) = New DataColumn("FA_UEBERF", System.Type.GetType("System.String"))
        dataColumns(38) = New DataColumn("BEM_SUZ", System.Type.GetType("System.String"))
        dataColumns(39) = New DataColumn("BEM_SUZ2", System.Type.GetType("System.String"))
        dataColumns(40) = New DataColumn("NAME_LG", System.Type.GetType("System.String"))



        dataColumns(41) = New DataColumn("KUNNR_LG", System.Type.GetType("System.String"))
        dataColumns(42) = New DataColumn("DAT_ERF_AUFTR", System.Type.GetType("System.String"))
        dataColumns(43) = New DataColumn("DAT_FREIG", System.Type.GetType("System.String"))
        dataColumns(44) = New DataColumn("WEB_USER_FREIG", System.Type.GetType("System.String"))



        dataColumns(45) = New DataColumn("BEM_RUECKFZG", System.Type.GetType("System.String"))
        dataColumns(46) = New DataColumn("ANGABEN_LG", System.Type.GetType("System.String"))
        dataColumns(47) = New DataColumn("VORHOL", System.Type.GetType("System.String"))
        dataColumns(48) = New DataColumn("ABGEARB", System.Type.GetType("System.String"))


        dataColumns(49) = New DataColumn("STANDORT", System.Type.GetType("System.String"))
        dataColumns(50) = New DataColumn("EINBAUFIRMA", System.Type.GetType("System.String"))

        dataColumns(51) = New DataColumn("FIRMA_WINTER", System.Type.GetType("System.String"))
        dataColumns(52) = New DataColumn("FA_WINTERRAD", System.Type.GetType("System.String"))
        dataColumns(53) = New DataColumn("WINTERRAD_POS", System.Type.GetType("System.String"))



        sapTable.Columns.AddRange(dataColumns)
        sapTable.AcceptChanges()
        Return sapTable
    End Function
    Public Sub FillControlElementsOrSAPTable(ByVal RowID As Int32, ByRef alControls As ArrayList, ByVal changeData As Boolean)
        'füllt Formular mit den werten der Angeforderten Row aus der SAPDatenTabelle  oder füllt die SAPDatenTabelle mit werten aus dem FormularJJ2007.11.14
        Dim tmpControl As Control
        Dim tmpTextbox As New TextBox()
        Dim tmpCheckBox As New CheckBox()
        Dim tmpRow As DataRow
        Dim orderNr As String = ""
        Dim aStrSplittedID() As String
        Dim ex As Exception


        'wenn noch keine Datentabelle Vorhanden ist, jetzt eine erstellen JJ2007.11.15
        If DatenTabelle Is Nothing Then
            createEmptySAPDataTable(createTable())
        End If
        'wenn noch keine Vorschlagswerte vorhanden sind, jetzt füllen JJ2007.11.27
        If Vorschlagswerte Is Nothing Then
            fillVorschlagswerte(Me.AppID, Me.SessionID)
        End If



        If RowID >= 0 Then
            tmpRow = DatenTabelle.Rows.Find(RowID)
            'ordernummer laut RowID FindenJJ2007.11.19
            orderNr = CStr(tmpRow.Item("Order_NR"))

        Else
            tmpRow = DatenTabelle.NewRow()

            'wenn eine neue Zeile erstellt wird, müssen alle Columns einen Leeren String wert erhalten
            'außer die die RowID Column da sie der autogenerierte Primärschlüssel ist, hinzugefügte Columns : "RowID","Status" daher length-3
            Dim i As Int32
            For i = 0 To tmpRow.ItemArray.Length - 3
                tmpRow.Item(i) = String.Empty
            Next i

        End If

        If changeData = True Then

            'wenn Abgearbeitet gekennzeichnet keine Änderungen vornehmen JJU2008.05.21
            If Not CStr(tmpRow.Item("ABGEARB")) = "X" Then
                For Each tmpControl In alControls

                    aStrSplittedID = tmpControl.ID.Split(CChar("_"))
                    'aufbau einer Contorl ID ( typ_behandlung_NameßZusammensetzungsnummer)
                    'Behandlungskürzel D=Direkt, S=Sonstige, B=Bearbeiten
                    'Endung ßZusammensetzungsnummer kommt nur bei Bearbeitungstyp vor
                    'JJ2007.11.21
                    Select Case aStrSplittedID(1)

                        Case "D"
                            fillDataRowByControl(tmpControl, tmpRow)
                        Case "B"
                            fillRowByBearbeitetenControls(tmpControl, tmpRow)
                        Case "S"
                            fillRowbySonstigeControls(tmpControl, tmpRow)
                        Case Else
                            ex = New Exception("Control IDs in der Eingabemaske sind nicht den Konventionen entsprechend: " & tmpControl.ID)
                            Throw ex
                    End Select
                Next
                'alle Controls die eine Nummerierung zur Sortierung in eine RowColum hatten werden jetzt in die ensprechende Reihenfolge gebracht und in die passende RowColum geschrieben(txt_B_NAMEß1, txt_B_NAMEß2,txt_B_NAMEß3) JJ2007.11.22 
                sortControlValuesAndWriteItIntoRowColumn(tmpRow)


                If Not RowID >= 0 Then
                    DatenTabelle.Rows.Add(tmpRow)
                End If
            End If
        Else 'zweig wenn daten aus der DT in die das Formular gefüllt werden

            'Disabeln von Controls kann nur stattfinden wenn Daten die aus dem SAP kamen bearbeitet werden,


            If Not orderNr = "" Then
                'füllen der arraylist die bestimmt welche Felder deaktiviert werden müssen anhand der Ordernummer, muss nur beim Anzeigen der Daten ausgeführt werden
                fillDisableFieldIds(orderNr)
            End If

            For Each tmpControl In alControls

                aStrSplittedID = tmpControl.ID.Split(CChar("_"))
                'aufbau einer Contorl ID ( typ_behandlung_NameßZusammensetzungsnummer)
                'Behandlungskürzel D=Direkt, S=Sonstige, B=Bearbeiten
                'Endung ßZusammensetzungsnummer kommt nur bei Bearbeitungstyp vor
                'JJ2007.11.21
                Select Case aStrSplittedID(1)

                    Case "D"
                        fillControlByDataRow(tmpControl, tmpRow)
                    Case "B"
                        fillControlsByBearbeitetenRows(tmpControl, tmpRow)
                    Case "S"
                        fillSonstigeControlsByrow(tmpControl, tmpRow)
                    Case Else
                        ex = New Exception("Control IDs in der Eingabemaske sind nicht den Konventionen entsprechend: " & tmpControl.ID)
                        Throw ex
                End Select

            Next
        End If

        setStatus()

    End Sub

    Private Sub fillRowbySonstigeControls(ByRef tmpControl As Control, ByRef tmpRow As DataRow)

        Dim tmpDDL As New DropDownList()

        If tmpControl.GetType.Equals(tmpDDL.GetType) Then
            tmpDDL = CType(tmpControl, DropDownList)

            If tmpControl.ID.Equals("DDL_S_ZULASSUNGSDIENST") Then
                tmpRow("ZUL_DIENST") = tmpDDL.SelectedItem.Value
            End If

            If tmpControl.ID.Equals("DDL_S_LIEFERUNGDURCH") Then
                tmpRow("FA_UEBERF") = tmpDDL.SelectedItem.Value
            End If

            If tmpControl.ID.Equals("DDL_S_STANDORT") Then
                tmpRow("STANDORT") = tmpDDL.SelectedItem.Value
            End If

            If tmpControl.ID.Equals("DDL_S_EINBAUFIRMA") Then
                tmpRow("EINBAUFIRMA") = tmpDDL.SelectedItem.Value
            End If

            If tmpControl.ID.Equals("DDL_S_FIRMA_WINTER") Then
                tmpRow("FIRMA_WINTER") = tmpDDL.SelectedItem.Value
            End If


        End If

        If tmpControl.ID = "DTG_S_SONDEINB_POS" Then
            'das EinbauDataGrid
            Dim tmpStringItem As String = ""
            Dim tmpStringBeschreibung As String = ""
            Dim tmpDTRow As DataRow
            'die DataTable für das DataGrid
            tmpDTRow = Einbau.NewRow()

            For Each tmpDTRow In Einbau.Rows
                tmpStringItem = tmpStringItem & CStr(tmpDTRow.Item("Item")) & "|"
                tmpStringBeschreibung = tmpStringBeschreibung & CStr(tmpDTRow.Item("Beschreibung")) & "|"
            Next
            If Not tmpStringBeschreibung = "" AndAlso Not tmpStringItem = "" AndAlso Not tmpStringBeschreibung = String.Empty AndAlso Not tmpStringItem = String.Empty Then
                'letzten Delimeter Entfernen
                tmpStringItem = tmpStringItem.Remove(tmpStringItem.Length - 1, 1)
                tmpStringBeschreibung = tmpStringBeschreibung.Remove(tmpStringBeschreibung.Length - 1, 1)
                'in Row einfügen
                tmpRow("AUFBER_POS") = tmpStringBeschreibung
                tmpRow("SONDEINB_POS") = tmpStringItem
            Else 'es wurden Alle einträge Entfernt oder es sind keine getätigt worden
                tmpRow("AUFBER_POS") = ""
                tmpRow("SONDEINB_POS") = ""
            End If

        End If


        'scheisse hier da sap inkonsitent, einmal checkboxen mit 0 und 1 und einmal x und blank, daher keine direkte befüllung möglich JJU2008.05.2008
        If tmpControl.ID = "cb_S_VORHOL" Then
            Dim tmpCb As CheckBox = CType(tmpControl, CheckBox)
            If tmpCb.Checked = True Then
                tmpRow("VORHOL") = "X"
            Else
                tmpRow("VORHOL") = ""
            End If
        End If


        If tmpControl.ID = "txt_S_BEM_SUZ" Then
            Dim tmpTxt As TextBox = CType(tmpControl, TextBox)
            If tmpTxt.Text.Length > 255 Then
                tmpRow("BEM_SUZ") = tmpTxt.Text.Substring(0, 255)
                tmpRow("BEM_SUZ2") = tmpTxt.Text.Substring(255, tmpTxt.Text.Length - 256)
            Else
                tmpRow("BEM_SUZ") = tmpTxt.Text
            End If
        End If

    End Sub


    Private Sub fillSonstigeControlsByrow(ByRef tmpControl As Control, ByRef tmpRow As DataRow)

        'controls die die Vorschlagwerte Enthalten
        If tmpControl.ID = "LBX_S_AUFBEREITUNGSART" OrElse tmpControl.ID = "LBX_S_AUSSTATTUNG" OrElse tmpControl.ID = "LBX_S_EINBAU" OrElse tmpControl.ID = "DDL_S_ZULASSUNGSDIENST" OrElse tmpControl.ID = "DDL_S_LIEFERUNGDURCH" OrElse tmpControl.ID = "DDL_S_STANDORT" OrElse tmpControl.ID = "DDL_S_EINBAUFIRMA" OrElse tmpControl.ID = "LBX_S_WINTERRAD_POS" OrElse tmpControl.ID = "DDL_S_FIRMA_WINTER" Then

            Dim tmpRows2() As DataRow
            Dim tmpRow2 As DataRow

            tmpRow2 = Vorschlagswerte.NewRow


            Dim tmpControl2 As ListControl
            'abfrage nach ControlIDName
            tmpRows2 = Vorschlagswerte.Select("KENNUNG='" & tmpControl.ID.Substring(tmpControl.ID.IndexOf("_", tmpControl.ID.IndexOf("_") + 1) + 1) & "'")
            'tmpRows = Vorschlagswerte.Select("Select * WHERE KENNUNG='" & tmpControl.ID.Substring(tmpControl.ID.IndexOf("_", tmpControl.ID.IndexOf("_") + 1) + 1) & "'")
            'füllen der Vorbelegten Controls mit werten aus der Tabelle Vorschlagswerte
            If tmpRows2.Length > 0 Then

                'weil nur DDLs und LBXs
                tmpControl2 = CType(tmpControl, ListControl)
                If tmpControl2.Items.Count > 0 Then
                    tmpControl2.Items.Clear()
                End If

                For Each tmpRow2 In tmpRows2
                    Dim item As New ListItem(CStr(tmpRow2("Pos_Text")), CStr(tmpRow2("POS_KURZTEXT")))
                    tmpControl2.Items.Add(item)
                Next
            End If

            'Passendes Selektionsitem für DDLs mit vergleich auf SAPDatenTabelle
            'wenn kein Passendes Item in der Liste gefunden wird, den Wert aus SAP in Liste einfügen und auswählen
            Dim bItemFound As Boolean = False

            If tmpControl.ID = "DDL_S_ZULASSUNGSDIENST" Then
                Dim item As ListItem
                For Each item In tmpControl2.Items

                    If item.Value.Equals(tmpRow("ZUL_DIENST")) Then
                        item.Selected = True
                        bItemFound = True
                        Exit For
                    End If
                Next

                If bItemFound = False And Not CStr(tmpRow("ZUL_DIENST")) Is String.Empty Then
                    item = New ListItem(CStr(tmpRow("ZUL_DIENST")), CStr(tmpRow("ZUL_DIENST")))
                    item.Selected = True
                    tmpControl2.Items.Add(item)
                End If

            End If

            'hinzufügen von -manuelle Eingabe- bei den Standortvorschlägen, und Selected Item wiederherstellen
            If tmpControl.ID = "DDL_S_STANDORT" Then
                Dim item As New ListItem("-manuelle Eingabe-", "")
                tmpControl2.Items.Insert(0, item)

                For Each item In tmpControl2.Items
                    If item.Value.Equals(tmpRow("STANDORT")) Then
                        item.Selected = True
                        Exit For
                    End If
                Next
            End If


            'hinzufügen von -manuelle Eingabe- bei den Standortvorschlägen, und Selected Item wiederherstellen
            If tmpControl.ID = "DDL_S_FIRMA_WINTER" Then
                Dim item As New ListItem("-manuelle Eingabe-", "")
                tmpControl2.Items.Insert(0, item)

                For Each item In tmpControl2.Items
                    If item.Value.Equals(tmpRow("FIRMA_WINTER")) Then
                        item.Selected = True
                        Exit For
                    End If
                Next



            End If


            'hinzufügen von -manuelle Eingabe- bei den Standortvorschlägen,  und Selected Item wiederherstellen
            If tmpControl.ID = "DDL_S_EINBAUFIRMA" Then
                Dim item As New ListItem("-manuelle Eingabe-", "")
                tmpControl2.Items.Insert(0, item)
                For Each item In tmpControl2.Items
                    If item.Value.Equals(tmpRow("EINBAUFIRMA")) Then
                        item.Selected = True
                        Exit For
                    End If
                Next

            End If


            If tmpControl.ID = "DDL_S_LIEFERUNGDURCH" Then
                Dim item As ListItem
                For Each item In tmpControl2.Items
                    If item.Value.Equals(tmpRow("FA_UEBERF")) Then
                        item.Selected = True
                        bItemFound = True
                        Exit For
                    End If
                Next

                If bItemFound = False And Not CStr(tmpRow("FA_UEBERF")) = String.Empty Then
                    item = New ListItem(CStr(tmpRow("FA_UEBERF")), CStr(tmpRow("FA_UEBERF")))
                    item.Selected = True
                    tmpControl2.Items.Add(item)
                End If

            End If

        End If


        If tmpControl.ID = "DTG_S_SONDEINB_POS" Then
            Dim tmpdtg As New DataGrid()

            tmpdtg = CType(tmpControl, DataGrid)
            tmpdtg.DataSource = Einbau
            Dim aStrSplitted1() As String
            Dim aStrSplitted2() As String
            Dim bla(1) As Object
            Dim i As Int32
            aStrSplitted1 = CStr(tmpRow("SONDEINB_POS")).Split(CChar("|"))
            aStrSplitted2 = CStr(tmpRow("AUFBER_POS")).Split(CChar("|"))

            'vorherige Daten in DataTable entfernen
            Einbau.Clear()
            'hier wird nur auf die Sondereinbau_Pos geprüft, eine Position muss es immer geben aber nicht immer ein Beschreibung (Aufber_Pos) JJ2007.11.26
            If aStrSplitted1.Length > 0 AndAlso aStrSplitted1(0) = "" = False AndAlso aStrSplitted1(0) = " " = False AndAlso aStrSplitted1(0) = String.Empty = False Then

                'die string arrays auf gleiche Länge anpassen, dies kann nur vorkommen wenn daten per schnittstellen kommen, per manuelle Auftragsbearbeitung werden auch wenn 
                'keine AUFBER_POS zur SONDEINB_POS angegebene wird das Trennzeichen | erstellt, somit werden die arrays gleichgroß definiert
                If aStrSplitted1.Length > aStrSplitted2.Length Then
                    'preserve bewirkt das speichern der elemente in dem array bei redim
                    ReDim Preserve aStrSplitted2(aStrSplitted1.Length - 1)
                End If
                For i = 0 To aStrSplitted1.Length - 1 'geht nur wenn beide Arrays gleich lang sind
                    'prüfen ob fälschlicher weise ein Trennzeichen | über die schnittstelle bei einem Item mitgegeben wurde
                    If Not aStrSplitted1(i).Trim = String.Empty Then
                        bla(0) = aStrSplitted1(i)
                        bla(1) = findVorschlagswertTextByShortText("EINBAU", aStrSplitted2(i))
                        Einbau.Rows.Add(bla)
                    End If
                Next

                Einbau.AcceptChanges()

                tmpdtg.DataBind()
            End If
        End If

        'scheisse hier da sap inkonsitent, einmal checkboxen mit 0 und 1 und einmal x und blank, daher keine direkte befüllung möglich JJU2008.05.2008
        If tmpControl.ID = "cb_S_VORHOL" Then
            Dim tmpCb As CheckBox = CType(tmpControl, CheckBox)
            If CStr(tmpRow(tmpControl.ID.Substring(tmpControl.ID.IndexOf("_", tmpControl.ID.IndexOf("_") + 1) + 1))).Equals("X") Then
                tmpCb.Checked = True
            Else
                tmpCb.Checked = False
            End If
        End If

        If tmpControl.ID = "txt_S_BEM_SUZ" Then
            Dim tmpTxt As TextBox = CType(tmpControl, TextBox)
            tmpTxt.Text = tmpRow("BEM_SUZ").ToString & tmpRow("BEM_SUZ2").ToString
        End If




    End Sub

    Private Sub fillDataRowByControl(ByRef tmpControl As Control, ByRef tmpRow As DataRow)

        Dim tmpTextbox As New TextBox()
        Dim tmpCheckBox As New CheckBox()



        'Jedes Die Row wird nach der ID jedes Controls selektiert abzüglich des ControlTyp Präfixes (zb txt_D_)JJ2007.11.15
        If tmpControl.GetType.Equals(tmpTextbox.GetType) Then
            tmpTextbox = CType(tmpControl, TextBox)
            If tmpTextbox.Enabled = True Then
                tmpRow(tmpTextbox.ID.Substring(tmpTextbox.ID.IndexOf("_", tmpTextbox.ID.IndexOf("_") + 1) + 1)) = tmpTextbox.Text
            End If

        ElseIf tmpControl.GetType.Equals(tmpCheckBox.GetType) Then
            tmpCheckBox = CType(tmpControl, CheckBox)
            If tmpCheckBox.Enabled = True Then
                If tmpCheckBox.Checked Then
                    tmpRow(tmpCheckBox.ID.Substring(tmpCheckBox.ID.IndexOf("_", tmpCheckBox.ID.IndexOf("_") + 1) + 1)) = "1"
                Else
                    tmpRow(tmpCheckBox.ID.Substring(tmpCheckBox.ID.IndexOf("_", tmpCheckBox.ID.IndexOf("_") + 1) + 1)) = "0"
                End If
            End If
        End If

    End Sub


    Private Sub fillControlByDataRow(ByRef tmpControl As Control, ByRef tmpRow As DataRow)
        Dim tmpTextbox As New TextBox()
        Dim tmpCheckBox As New CheckBox()


        System.Diagnostics.Debug.WriteLine(tmpControl.ID)

        If Not tmpControl.ID.Equals("txt_License_service2") Then 'nur solange Schnittstellenbeschreibung noch nicht feststeht, weil Feld nicht im SAP vorhanden JJ2007.11.15

            If tmpControl.GetType.Equals(tmpTextbox.GetType) Then
                tmpTextbox = CType(tmpControl, TextBox)

                'jetzt noch nicht relevant JJ2007.11.21
                'Prüfen ob Control bearbeitet werden darf oder nicht
                'If alDisabledFieldIDs.Contains(tmpTextbox.ID) Then
                '    tmpTextbox.Enabled = False
                '    tmpTextbox.ToolTip = CStr(alDisabledFieldIDs.Item(alDisabledFieldIDs.IndexOf(tmpTextbox.ID) + 1))
                'End If

                'ID nach 2. _ abschneiden so das die ID gleich ist mit dem ColumnName der ROW JJ2007.11.21
                tmpTextbox.Text = CStr(tmpRow(tmpTextbox.ID.Substring(tmpTextbox.ID.IndexOf("_", tmpTextbox.ID.IndexOf("_") + 1) + 1)))

            ElseIf tmpControl.GetType.Equals(tmpCheckBox.GetType) Then
                tmpCheckBox = CType(tmpControl, CheckBox)

                'jetzt noch nicht relevant JJ2007.11.21
                'Prüfen ob Control bearbeitet werden darf oder nicht
                'If alDisabledFieldIDs.Contains(tmpTextbox.ID) Then
                '    tmpCheckBox.Enabled = False
                '    tmpCheckBox.ToolTip = CStr(alDisabledFieldIDs.Item(alDisabledFieldIDs.IndexOf(tmpCheckBox.ID) + 1))
                'End If

                'ID nach 2. _ abschneiden so das die ID gleich ist mit dem ColumnName der ROW JJ2007.11.21
                If Not CStr(tmpRow(tmpCheckBox.ID.Substring(tmpCheckBox.ID.IndexOf("_", tmpCheckBox.ID.IndexOf("_") + 1) + 1))).Equals("1") Then
                    tmpCheckBox.Checked = False
                Else
                    tmpCheckBox.Checked = True
                End If
            End If
        End If

    End Sub

    Private Sub fillControlsByBearbeitetenRows(ByRef tmpControl As Control, ByRef tmpRow As DataRow)
        Dim aStrSplittedRowValue() As String
        Dim tmpStr As String
        Dim tmpLBX As New ListBox()
        Dim tmpTxt As New TextBox()


        'es gibt nur die möglichkeit das eine Listbox.id keine nummerierung enthält, da die felder der Row als items in der lbx verwendet werden. jj2007.11.22
        If tmpControl.GetType.Equals(tmpLBX.GetType) = True AndAlso tmpControl.ID.LastIndexOf("ß") = -1 Then
            tmpLBX = CType(tmpControl, ListBox)

            aStrSplittedRowValue = CStr(tmpRow(tmpLBX.ID.Substring(tmpLBX.ID.IndexOf("_", tmpLBX.ID.IndexOf("_") + 1) + 1))).Split(CChar("|"))

            If aStrSplittedRowValue.Length > 0 Then
                For Each tmpStr In aStrSplittedRowValue
                    If tmpStr Is String.Empty = False AndAlso tmpStr.Equals("") = False AndAlso tmpStr.Equals(" ") = False Then
                        'auch die items in listen müssen umgewandelt werden in den anzeigetext von vorschlagswerten, leider sind die kennungen nicht gleich dem feldnamen im SAP, daher hier unterscheidungen treffen JJU2008.07.16
                        Dim kennnung As String = tmpLBX.ID.Substring(tmpLBX.ID.IndexOf("_", tmpLBX.ID.IndexOf("_") + 1) + 1)
                        Select Case kennnung
                            Case "AUFBER_ART"
                                kennnung = "AUFBEREITUNGSART"
                            Case "AUSSTATT"
                                kennnung = "AUSSTATTUNG"
                        End Select
                        Dim tmpItem As New ListItem(findVorschlagswertTextByShortText(kennnung, tmpStr), tmpStr)
                        tmpLBX.Items.Add(tmpItem)
                    End If
                Next
            End If
        End If


        'es gibt nur die möglichkeit das eine textbix.id eine nummerierung enthält, da alle Angaben in der row auf verschiedene textboxen in einer Bestimmten Reihenfolge aufgeteilt werden jj2007.11.22
        If tmpControl.GetType.Equals(tmpTxt.GetType) = True AndAlso Not tmpControl.ID.LastIndexOf("ß") = -1 Then
            tmpTxt = CType(tmpControl, TextBox)
            'holt sich aus dem aus dem ID-String des Controls (z.B. TXT_B_NAMEß1) den Teil Name herraus, sucht mit dem in der Row die passende Column, Splittet dessen Inhalt dann mit dem Seperator "|" JJ2007.11.22 
            aStrSplittedRowValue = CStr(tmpRow(tmpTxt.ID.Substring(tmpTxt.ID.IndexOf("_", tmpTxt.ID.IndexOf("_") + 1) + 1, (tmpTxt.ID.Length - ((tmpTxt.ID.Length - tmpTxt.ID.LastIndexOf(CChar("ß"))) + tmpTxt.ID.IndexOf("_", tmpTxt.ID.IndexOf("_") + 1) + 1))))).Split(CChar("|"))

            If aStrSplittedRowValue.Length > 1 Then
                'ordnet den string aus einer Bestimmten Postition aus dem Array, die sich auf den Parameter der Contorl.id bezieht, dem Control zu, dabei ist zu beachten, dass das Array 0 basierend ist, die Control Nummerierung nicht JJ2007.11.22
                tmpTxt.Text = aStrSplittedRowValue(CInt(tmpTxt.ID.Substring(tmpTxt.ID.LastIndexOf(CChar("ß")) + 1, (tmpTxt.ID.Length - tmpTxt.ID.LastIndexOf(CChar("ß")) - 1))) - 1)
            End If
        End If
    End Sub


    Private Sub fillRowByBearbeitetenControls(ByRef tmpControl As Control, ByRef tmpRow As DataRow)
        Dim tmpStr As String = String.Empty
        Dim tmpLBX As New ListBox()
        Dim tmpTxt As New TextBox()


        'es gibt nur die möglichkeit das eine Listbox.id keine nummerierung enthält, da die felder der Row als items in der lbx verwendet werden. jj2007.11.22
        If tmpControl.GetType.Equals(tmpLBX.GetType) = True AndAlso tmpControl.ID.LastIndexOf("ß") = -1 Then
            tmpLBX = CType(tmpControl, ListBox)
            Dim item As New ListItem()


            If tmpLBX.Items.Count > 0 Then
                For Each item In tmpLBX.Items
                    tmpStr = tmpStr & item.Value.ToString & "|"
                Next
                'letzten delimeter entfernen
                tmpStr = tmpStr.Remove(tmpStr.Length - 1, 1)
                tmpRow(tmpLBX.ID.Substring(tmpLBX.ID.IndexOf("_", tmpLBX.ID.IndexOf("_") + 1) + 1)) = tmpStr
            Else 'wenn alles herausgelöscht wurde muss natürlich auch die Row geleert werden!
                tmpRow(tmpLBX.ID.Substring(tmpLBX.ID.IndexOf("_", tmpLBX.ID.IndexOf("_") + 1) + 1)) = ""
            End If
        End If

        'es gibt nur die möglichkeit das eine textbix.id eine nummerierung enthält, da alle Angaben in der row auf verschiedene textboxen in einer Bestimmten Reihenfolge aufgeteilt werden, bzw alle Control-Inhalte in einer Column wieder zusammengeführt werden in der Richtigen Reihenfolge jj2007.11.22
        If tmpControl.GetType.Equals(tmpTxt.GetType) = True AndAlso Not tmpControl.ID.LastIndexOf("ß") = -1 Then
            alBearbeitungsControlsWithSort.Add(tmpControl)

            ''holt sich aus dem aus dem ID-String des Controls (z.B. TXT_B_NAMEß1) den Teil Name herraus, sucht mit dem in der Row die passende Column, Splittet dessen Inhalt dann mit dem Seperator "|" JJ2007.11.22 
            'aStrSplittedRowValue = CStr(tmpRow(tmpTxt.ID.Substring(tmpTxt.ID.IndexOf("_", tmpTxt.ID.IndexOf("_") + 1) + 1, (tmpTxt.ID.Length - ((tmpTxt.ID.Length - tmpTxt.ID.LastIndexOf(CChar("ß"))) + tmpTxt.ID.IndexOf("_", tmpTxt.ID.IndexOf("_") + 1) + 1))))).Split(CChar("|"))

            'If aStrSplittedRowValue.Length > 1 Then
            '    'ordnet den string aus einer Bestimmten Postition aus dem Array, die sich auf den Parameter der Contorl.id bezieht, dem Control zu, dabei ist zu beachten, dass das Array 0 basierend ist, die Control Nummerierung nicht JJ2007.11.22
            '    tmpTxt.Text = aStrSplittedRowValue(CInt(tmpTxt.ID.Substring(tmpTxt.ID.LastIndexOf(CChar("ß")) + 1, tmpTxt.ID.Length)) - 1)
            'End If
        End If
    End Sub


    Private Sub sortControlValuesAndWriteItIntoRowColumn(ByRef tmpRow As DataRow)
        Dim tmpcontrol As Control

        If alBearbeitungsControlsWithSort.Count > 0 Then
            tmpcontrol = CType(alBearbeitungsControlsWithSort(0), Control)
            getAllColumnControls(tmpcontrol.ID.Substring(tmpcontrol.ID.IndexOf("_", tmpcontrol.ID.IndexOf("_") + 1) + 1, (tmpcontrol.ID.Length - ((tmpcontrol.ID.Length - tmpcontrol.ID.LastIndexOf(CChar("ß"))) + tmpcontrol.ID.IndexOf("_", tmpcontrol.ID.IndexOf("_") + 1) + 1))), tmpRow)
        End If


    End Sub

    Private Sub getAllColumnControls(ByVal ColumnName As String, ByRef tmpRow As DataRow)
        Dim tmpcontrol As Control
        Dim alControlOfOneColumn As New ArrayList()
        For Each tmpcontrol In alBearbeitungsControlsWithSort

            If Not tmpcontrol.ID.IndexOf(ColumnName) = -1 Then
                'hinzufügen zur temporären AL die nur controls beinhaltet die in eine rowColumn gehören
                alControlOfOneColumn.Add(tmpcontrol)
            End If
        Next

        For Each tmpcontrol In alControlOfOneColumn
            'entfernen aus Allgemeinen Arrayliste mit Controls die Nummeriert sind und bearbeitet werden müssen
            alBearbeitungsControlsWithSort.Remove(tmpcontrol)
        Next
        fillRowColumnWithNumberedControls(tmpRow, alControlOfOneColumn, ColumnName)
        'eine Art Rekursion bis diese Methode nicht mehr aufgerufen wird von der  "sortControlValuesAndWriteItIntoRowColumn"
        sortControlValuesAndWriteItIntoRowColumn(tmpRow)
    End Sub

    Private Sub fillRowColumnWithNumberedControls(ByRef tmpRow As DataRow, ByVal alControlOfOneColumn As ArrayList, ByVal columnName As String)
        Dim tmpcontrol As New Control()
        Dim strColumnValue As String = String.Empty
        Dim tmpLBX As New ListBox()
        Dim tmpTxt As New TextBox()
        Dim al As New ArrayList()
        Dim ex As Exception
        Dim intPositionOfColumValue As Int32

        Dim aStrColumnValue(alControlOfOneColumn.Count - 1) As String

        For Each tmpcontrol In alControlOfOneColumn

            If Not tmpcontrol.ID.LastIndexOf("ß") = -1 Then
                If tmpcontrol.GetType.Equals(tmpTxt.GetType) = True Then
                    tmpTxt = CType(tmpcontrol, TextBox)
                    intPositionOfColumValue = CInt(tmpTxt.ID.Substring(tmpTxt.ID.LastIndexOf("ß") + 1, (tmpTxt.ID.Length - tmpTxt.ID.LastIndexOf("ß") - 1)))
                    If Not intPositionOfColumValue > aStrColumnValue.Length Then
                        'hinzufügen des ControlInhalts an die Position in ein StringArray die in der ID angegeben ist, Arraylist ist 0 basierend JJ2007.11.22
                        aStrColumnValue(intPositionOfColumValue - 1) = tmpTxt.Text
                    Else
                        ex = New Exception("Fehler, ControlNummer ist Größer als Array für den ColumnString: " & tmpTxt.ID)
                        Throw ex
                    End If
                End If
            Else
                ex = New Exception("Fehler, in methode fillRowColumnWithNumberedControls, in der ArrayListe befand sich ein nicht nummeriertes Controls: " & tmpcontrol.ID)
                Throw ex
            End If
        Next

        Dim tmpStr As String

        'den String aus dem StringArray zusammenfügen mit Delimeter | und in die passende Column in der Row Schreiben
        For Each tmpStr In aStrColumnValue
            strColumnValue = strColumnValue & tmpStr & "|"
        Next
        'letzten delimeter entfernen
        strColumnValue = strColumnValue.Remove(strColumnValue.Length - 1, 1)

        tmpRow(columnName) = strColumnValue
    End Sub


    Private Sub createEmptySAPDataTable(ByVal sapRueckgabetabelle As DataTable)
        'war mal vor biztalkumstellung JJU2008.07.03
        'Dim tmpDatenTabelle As New SAPProxy_Kruell.ZDAD_AUFTR_IMP_001Table()
        'DatenTabelle = tmpDatenTabelle.ToADODataTable()
        'addRowIDAndRowStatusColumnToDataTable(DatenTabelle)

        'brauch eine Leere Tabelle mit der struktur der SAP rückgabetabelle
        DatenTabelle = sapRueckgabetabelle.Clone


        addRowIDAndRowStatusColumnToDataTable()
    End Sub

    Public Sub setStatus(Optional ByVal versendet As Integer = 0)
        'setzt den status der Row in die das Feld("Status")
        Dim row As DataRow
        row = DatenTabelle.NewRow()

        For Each row In DatenTabelle.Rows

            If versendet = 0 Then


                'wenn RowStatus ist "gelöscht" keinen Neuen Status setzten, dieser Status ist nur Gefakt um die Anzeige im Grid zu erhalten, dieser status wird direkt gesetzt wenn auf lösch button geklickt wurde
                If Not row("Status") Is "gelöscht" Then
                    'wenn der auftrag abgearbeitet ist bleibt der status auch dabei, die row ist leider modified durch das setzen des statuses beim laden JJU2008.05.21




                    If Not CStr(row("ABGEARB")) = "X" Then
                        If row.RowState = DataRowState.Added Then
                            row("Status") = "neuer Auftrag"
                            boolDataChanged = True
                        ElseIf row.RowState = DataRowState.Modified Then
                            boolDataChanged = True
                            row("Status") = "Auftrag bearbeitet"
                        End If

                    End If


                    'kommt eigentlich nur bei der inital füllung vor, wobei danach ein acceptChange ausgeführt wird, muss aber so gehandelt werden
                    'weil wenn der status der Row Unchanged ist, weil er ansonsten changed ist wenn die Column "status" überschrieben wird
                    If row.RowState = DataRowState.Unchanged And Not row("Status") Is "Auftrag unbearbeitet" Then

                        If CStr(row("ABGEARB")) = "X" Then 'auftrag als abgearbeitet gekennzeichnet und dafür status angeben
                            row("Status") = "Auftrag abgearbeitet"
                        Else
                            row("Status") = "Auftrag unbearbeitet"
                        End If
                    End If

                Else
                    boolDataChanged = True
                End If
            Else
                row("Status") = "Vorgang abgeschlossen \ keine Änderung"
            End If

        Next

    End Sub

    Private Sub addRowIDAndRowStatusColumnToDataTable()
        'fügt eine Spalte "RowID" der Datatable als AutoIncrement Primärschlüssel hinzu JJ2007.11.16
        Dim primaryKeyColumns(1) As DataColumn
        Dim bla As Int32
        Dim RowStatusColum As New DataColumn("Status", String.Empty.GetType)

        Dim RowIDColumn As New DataColumn("ROWID", bla.GetType)
        primaryKeyColumns(0) = RowIDColumn
        DatenTabelle.Columns.Add(RowIDColumn)
        DatenTabelle.Columns("ROWID").AutoIncrementSeed = 0
        DatenTabelle.Columns("ROWID").AutoIncrementStep = 1
        DatenTabelle.Columns("ROWID").AutoIncrement = True
        DatenTabelle.AcceptChanges()
        DatenTabelle.PrimaryKey = primaryKeyColumns
        DatenTabelle.Columns.Add(RowStatusColum)
        DatenTabelle.AcceptChanges()

    End Sub

    Private Sub fillDisableFieldIds(ByVal orderNr As String)
        'wenn eine neue Zeile hinzugefügt wurde und diese dann Bearbeitet wurde, gibt es keine SAP Fehler Tabelle
        'füllt eine Arrayliste mit den DisabledFieldIDs und Verhinderungsgrundtext im wechsel( ID,Grund,ID,Grund...)
        alDisabledFieldIDs.Clear()

        If Not dtSapFehlerTable Is Nothing AndAlso dtSapFehlerTable.Rows.Count > 0 Then
            Dim tmpDataRows() As DataRow
            Dim tmpDataRow As DataRow
            Dim strResultID As String
            Dim strResultText As String


            tmpDataRows = dtSapFehlerTable.Select("* Where ORDER_NR=" & orderNr)
            If Not tmpDataRows Is Nothing And tmpDataRows.Length > 0 Then
                For Each tmpDataRow In tmpDataRows
                    strResultID = CStr(tmpDataRow.Item("FELDNAME"))
                    strResultText = Str(tmpDataRow.Item("FEHLER"))
                    alDisabledFieldIDs.Add(strResultID)
                    alDisabledFieldIDs.Add(strResultText)
                Next
            End If
        End If

    End Sub

    Public Overloads Sub Fill(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "kruell_01.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True
        End If

       
        Dim intID As Int32 = -1

        Dim tmpDataTable As New DataTable

        Dim con As New SAPConnection(m_BizTalkSapConnectionString)
        con.Open()

        Try
            Dim cmd As New SAPCommand()
            cmd.Connection = con

            Dim strCom As String



            strCom = "EXEC Z_M_IMP_AUFTRDAT_002 @I_KUNNR=@pI_KUNNR,@I_DAT_ERF_VON=@pI_DAT_ERF_VON,"
            strCom = strCom & "@I_DAT_ERF_BIS=@pI_DAT_ERF_BIS, @I_NAME_LG=@pI_NAME_LG, @I_ORDER_NR=@pI_ORDER_NR, @I_CHASSIS_NUM=@pI_CHASSIS_NUM,"
            strCom = strCom & "@GT_AUFTR1=@pI_GT_AUFTR1, @GT_FEHL=@pI_GT_FEHL, @GT_AUFTR1=@pO_GT_AUFTR1  OUTPUT ,@GT_FEHL=@pO_GT_FEHL OUTPUT OPTION 'disabledatavalidation'"

            cmd.CommandText = strCom

            Dim pI_KUNNR As New SAPParameter("@pI_KUNNR", ParameterDirection.Input)
            Dim pI_DAT_ERF_VON As New SAPParameter("@pI_DAT_ERF_VON", ParameterDirection.Input)
            Dim pI_DAT_ERF_BIS As New SAPParameter("@pI_DAT_ERF_BIS", ParameterDirection.Input)
            Dim pI_NAME_LG As New SAPParameter("@pI_NAME_LG", ParameterDirection.Input)
            Dim pI_ORDER_NR As New SAPParameter("@pI_ORDER_NR", ParameterDirection.Input)
            Dim pI_CHASSIS_NUM As New SAPParameter("@pI_CHASSIS_NUM", ParameterDirection.Input)

            Dim pI_GT_AUFTR1 As New SAPParameter("@pI_GT_AUFTR1", tmpDataTable)

            Dim pO_GT_AUFTR1 As New SAPParameter("@pO_GT_AUFTR1", ParameterDirection.Output)

            Dim pI_GT_FEHL As New SAPParameter("@pI_GT_FEHL", tmpDataTable)
            Dim pO_GT_FEHL As New SAPParameter("@pO_GT_FEHL", ParameterDirection.Output)

            'Importparameter hinzufügen
            cmd.Parameters.Add(pI_KUNNR)
            cmd.Parameters.Add(pI_DAT_ERF_VON)
            cmd.Parameters.Add(pI_DAT_ERF_BIS)
            cmd.Parameters.Add(pI_NAME_LG)
            cmd.Parameters.Add(pI_ORDER_NR)
            cmd.Parameters.Add(pI_CHASSIS_NUM)

            'tabelle Hinzufügen
            cmd.Parameters.Add(pI_GT_AUFTR1)
            cmd.Parameters.Add(pO_GT_AUFTR1)
            cmd.Parameters.Add(pI_GT_FEHL)
            cmd.Parameters.Add(pO_GT_FEHL)


            pI_DAT_ERF_BIS.Value = "00000000"
            If Not strDatumBis = "" AndAlso Not strDatumBis Is String.Empty Then
                pI_DAT_ERF_BIS.Value = MakeDateSAP(strDatumBis)
            End If

            pI_DAT_ERF_VON.Value = "00000000"
            If Not strDatumVon = "" AndAlso Not strDatumVon Is String.Empty Then
                pI_DAT_ERF_VON.Value = MakeDateSAP(strDatumVon)
            End If


            If strLeasinggeber Is Nothing Then
                strLeasinggeber = String.Empty
            End If
            If strordernummer Is Nothing Then
                strordernummer = String.Empty
            End If
            If strfahrgestellnummer Is Nothing Then
                strfahrgestellnummer = String.Empty
            End If

            pI_NAME_LG.Value = strLeasinggeber
            pI_ORDER_NR.Value = strordernummer
            pI_CHASSIS_NUM.Value = strfahrgestellnummer


            pI_KUNNR.Value = Right("00000000000" & m_objUser.KUNNR, 10)


            m_intStatus = 0
            m_strMessage = ""

            intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_IMP_AUFTRDAT_002", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)
            cmd.ExecuteNonQuery()
            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, True)
            End If



            dtSapFehlerTable = DirectCast(pO_GT_FEHL.Value, DataTable)

            SAPearlyPrintTable = DirectCast(pO_GT_AUFTR1.Value, DataTable)
            killAllDBNullValuesInDataTable(SAPearlyPrintTable)

            'nachträglich kann einer Tabelle die mit daten gefüllt ist schlecht eine Column mit autoIncrement werten hinzugefügt werden, daher müssen die einzelnen Rows in die modifizierte Datentabelle kopiert werden JJ2007.11.19


            createEmptySAPDataTable(DirectCast(pO_GT_AUFTR1.Value, DataTable))
            Dim bla() As Object
            Dim tmpDT As DataTable
            Dim tmprow As DataRow
            tmpDT = DirectCast(pO_GT_AUFTR1.Value, DataTable)
            For Each tmprow In tmpDT.Rows

                'Datum formatieren in Zeile
                If Not tmprow.Item("Dat_Erf_Auftr") Is DBNull.Value Then
                    tmprow.Item("Dat_Erf_Auftr") = MakeDateStandard(CStr(tmprow.Item("Dat_Erf_Auftr"))).ToShortDateString
                End If

                If Not tmprow.Item("DAT_FREIG") Is DBNull.Value Then
                    tmprow.Item("DAT_FREIG") = MakeDateStandard(CStr(tmprow.Item("DAT_FREIG"))).ToShortDateString
                End If
                bla = tmprow.ItemArray
                DatenTabelle.Rows.Add(bla)
            Next

            DatenTabelle.AcceptChanges()

            'biztalkadapter gibt immer DBnull statt leere strings zurück, das ist nervig!!!! JJU2008.07.03
            killAllDBNullValuesInDataTable(DatenTabelle)


            setStatus() 'status Colums füllen
            DatenTabelle.AcceptChanges() 'row state wieder auf unchanged setzen




        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_intStatus = -1111
                    m_strMessage = "Keine Fahrzeuge gefunden."
                Case "NOT_FOUND"
                    m_intStatus = -1111
                    m_strMessage = "Keine Fahrzeuge gefunden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select
            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
            End If
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        Finally
            If intID > -1 Then
                m_objLogApp.WriteStandardDataAccessSAP(intID)
            End If
            con.Close()
            m_blnGestartet = False
        End Try
    End Sub


    Private Sub killAllDBNullValuesInDataTable(ByRef datentabelle As DataTable)

        For Each tmpRow As DataRow In datentabelle.Rows
            For i As Int32 = 0 To tmpRow.ItemArray.Length - 1
                If tmpRow(i) Is DBNull.Value Then
                    tmpRow(i) = String.Empty
                End If
            Next
        Next
        datentabelle.AcceptChanges()

    End Sub

    Public Sub SendDataToSAP(ByVal strAppID As String, ByVal strSessionID As String)

        m_strClassAndMethod = "kruell_01.SendDataToSAP"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True
        End If

        'ka seitdem auf biztalk umgestellt wurde, ist das vll ein wenig schwachsinnig, aber was sollst das datumsformat muss eh geändert werden JJU2008.7.3
        'SAPdatenTable.FromADODataTable(DatenTabelle)
        'Dim SAPdatenTable As New SAPProxy_Kruell.ZDAD_AUFTR_IMP_001Table()
        Dim SAPdatenTable As New DataTable

        Dim con As New SAPConnection(m_BizTalkSapConnectionString)
        con.Open()


        Dim intID As Int32 = -1

        Try

            'mit lokaler DT weiterarbeiten, weil wenn fehler auftritt sonst ROWID/Status Column entfernt ist. JJ2007.11.16
            Dim dtX As New DataTable()
            dtX = DatenTabelle.Clone
            dtX = DatenTabelle.Copy

            'Status der Aufträge  setzen, kann erst nach clonen geschehen sonst kann nicht mehr festgestellt werden was der User geändert hat JJ2007.11.19
            'DatenTabelle.AcceptChanges()
            'setStatus()'fraglich, wahrscheinlich wird der vorgang komplett abgebrochen mit einem Statusbericht über die ereignisse, so das nicht im gleichen Grid weiter gearbeitet wird.
            'hier wird die Dt für die Druckausgabe modifiziert
            modifyDatentabelleForPrint()

            'sapResultmeldung löschen
            strSAPResultMeldung = ""



            'finden Aller zeilen, die neu hinzugefügt wurden und wieder geschlöscht worden sind, diese müsse nicht ins SAP übertragen werden. JJ2007.11.19
            Dim hasTitel As Boolean = False
            Dim drs() As DataRow
            Dim dr As DataRow
            drs = dtX.Select("Status='gelöscht'")
            If Not drs Is Nothing Then
                For Each dr In drs
                    If dr.RowState = DataRowState.Added Then
                        'wenn Rowneuhinzugefügt wurde und wieder gelöscht wurde, einfach nicht ins sap senden
                        dtX.Rows.Remove(dr)
                    Else
                        'wenn noch keine löschüberschrift gesetzt ist, jetzt eine setzen
                        If hasTitel = False Then
                            'LöschÜberschrift hinzufügen
                            strSAPResultMeldung = strSAPResultMeldung & "<p align=""center""><FONT size=""2""><b><u>Auftragslöschungen</u></b></font></p>"
                            hasTitel = True
                        End If
                        'löschen laut Order_nr in sap
                        loeschDatainSap(CStr(dr("Order_NR")))

                        'row aus Datentabelle entfernen die dann als Druckansicht dient
                        DatenTabelle.Rows.Find(dr("RowID")).Delete()
                        'row Entfernen das Sie nicht wieder ins sap geschrieben wird
                        dtX.Rows.Remove(dr)


                    End If
                Next
            End If

            'leerzeile Nach auftragslöschungen
            strSAPResultMeldung = strSAPResultMeldung & "<br>"


            'vorbereiten der Datatable für senden ins SAP( Tabelle muss gleich sein mit übergabestruktur von SAP)  JJ2007.11.19
            dtX.PrimaryKey = Nothing
            dtX.AcceptChanges()
            dtX.Columns.Remove("ROWID") 'column wieder entfernen
            dtX.Columns.Remove("Status") 'column wieder entfernen
            dtX.AcceptChanges()


            'muss seit biztalk sein, da ich ja keine struktur habe und sie mir so machen muss. JJU2008.07.03
            SAPdatenTable = dtX.Clone()
            SAPdatenTable.AcceptChanges()


           
            Dim adoColumn As New DataColumn()
            'Dim sapRow As SAPProxy_Kruell.ZDAD_AUFTR_IMP_001
            Dim NewRow As DataRow
            Dim adorow As DataRow
            adorow = DatenTabelle.NewRow

            'Tabelle Zum Senden  wird gefülltJJ2007.11.16
            For Each adorow In dtX.Rows
                NewRow = SAPdatenTable.NewRow
                'sapRow = New SAPProxy_Kruell.ZDAD_AUFTR_IMP_001()
                For Each adoColumn In dtX.Columns
                    If Not adorow(adoColumn.ColumnName).GetType Is DBNull.Value.GetType Then 'wenn Feld in Eingabemaske nicht vorhanden aber dennoch in der Struktur der Datatable aus SAP, somit=DBNull und darf nicht hinzugefügt werden JJ2007.11.16
                        If (adoColumn.ColumnName).ToUpper = "DAT_ERF_AUFTR" OrElse (adoColumn.ColumnName).ToUpper = "DAT_FREIG" Then  'datum in SAP Format umschreiben
                            If Not CStr(adorow(adoColumn.ColumnName)) = "" OrElse Not CStr(adorow(adoColumn.ColumnName)) = String.Empty Then
                                If CStr(adorow(adoColumn.ColumnName)) = "01.01.1900" Then 'wenn makeDateStandard aufgerufen wurde und Datum aus SAP 00000000, wird dieses Datum eingetragen, muss jetzt wieder rückgängig gemacht werden, JJ2007.11.16
                                    NewRow(adoColumn.ColumnName) = "00000000"
                                Else
                                    NewRow(adoColumn.ColumnName) = MakeDateSAP(CStr(adorow(adoColumn.ColumnName)))
                                End If

                            End If
                        Else
                            NewRow(adoColumn.ColumnName) = CStr(adorow(adoColumn.ColumnName))
                        End If

                    End If
                Next
                SAPdatenTable.Rows.Add(NewRow)
            Next
            SAPdatenTable.AcceptChanges()

            'Dim SAPFehlerTable As DataTable
            Dim strSAPDoingKennzeichen As String 'ergebniss der Anlage/Änderung im SAP JJ2007.11.16
            Dim strSAPFreigabeKennzeichen As String 'Kennzeichen ob Auftrag Freigegeben ist

            Dim tmpSendRow As DataRow
            Dim tmpDataTable As New DataTable
            Dim SapSendTableX As New DataTable
            SapSendTableX = SAPdatenTable.Clone()
            killAllDBNullValuesInDataTable(SAPdatenTable)

            For Each tmpSendRow In SAPdatenTable.Rows
                SapSendTableX.Clear()
                Dim cmd As New SAPCommand()
                cmd.Connection = con

                Dim strCom As String

                strCom = "EXEC Z_M_Imp_Auftrdat_001 @I_KUNNR=@pI_KUNNR,@I_DATEN=@pI_DATEN,"
                strCom = strCom & "@E_AKT=@pE_AKT Output, @E_FREI=@pE_FREI Output,"
                strCom = strCom & "@GT_FEHL=@pI_GT_FEHL, @GT_FEHL=@pO_GT_FEHL OUTPUT OPTION 'disabledatavalidation'"

                cmd.CommandText = strCom

                'importparameter
                Dim pI_KUNNR As New SAPParameter("@pI_KUNNR", ParameterDirection.Input)
                'Dim pI_DATEN As New SAPParameter("@pI_DATEN", tmpSendRow)
                Dim pI_DATEN As New SAPParameter("@pI_DATEN", SapSendTableX)


                'exportparameter
                Dim pE_AKT As New SAPParameter("@pE_AKT", ParameterDirection.Output)
                Dim pE_FREI As New SAPParameter("@pE_FREI", ParameterDirection.Output)

                'tabellen
                Dim pI_GT_FEHL As New SAPParameter("@pI_GT_FEHL", tmpDataTable)
                Dim pO_GT_FEHL As New SAPParameter("@pO_GT_FEHL", ParameterDirection.Output)

                'Importparameter hinzufügen
                cmd.Parameters.Add(pI_KUNNR)
                cmd.Parameters.Add(pI_DATEN)

                'exportparameter hinzufügen
                cmd.Parameters.Add(pE_AKT)
                cmd.Parameters.Add(pE_FREI)

                'tabellen hinzufügen
                cmd.Parameters.Add(pI_GT_FEHL)
                cmd.Parameters.Add(pO_GT_FEHL)


                pI_KUNNR.Value = Right("00000000000" & m_objUser.KUNNR, 10)
                'pI_DATEN.Value = tmpSendRow

                'versuche hier verzweifelt nur eine Row in eine tabelle die der SAP struktur gerecht wird einzufügen JJU2008.07.03
                Dim tmpXXXRow As DataRow = SapSendTableX.NewRow
                tmpXXXRow.ItemArray = tmpSendRow.ItemArray
                SapSendTableX.Rows.Add(tmpXXXRow)
                SapSendTableX.AcceptChanges()


                strOrderNR = CStr(tmpSendRow("Order_Nr"))

                'objSAP.Z_M_Imp_Auftrdat_001(sapRow, Right("0000000000" & m_objUser.KUNNR, 10), strSAPDoingKennzeichen, strSAPFreigabeKennzeichen, SAPFehlerTable)

                intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Imp_Auftrdat_001", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)
                cmd.ExecuteNonQuery()
                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, True)
                End If

                'export parameter/tabellen auswerten 
                If pE_AKT.Value Is DBNull.Value Then
                    pE_AKT.Value = String.Empty
                End If

                If pE_FREI.Value Is DBNull.Value Then
                    pE_FREI.Value = String.Empty
                End If

                strSAPDoingKennzeichen = CStr(pE_AKT.Value)
                strSAPFreigabeKennzeichen = CStr(pE_FREI.Value)


                If strSAPFreigabeKennzeichen = "X" Then
                    strSAPResultMeldung = strSAPResultMeldung & "<p align=""center""><FONT color=""GREEN"" size=""2""><b>Auftrag freigegeben:&nbsp; OrderNr:&nbsp; " & CStr(tmpSendRow("Order_Nr")) & "</font></b></p>"
                End If

                Select Case (strSAPDoingKennzeichen)
                    Case "A"
                        createAenderungsFormular(tmpSendRow, "<FONT Color=""Orange""><b>Vorgang abgeschlossen \ neuer Auftrag</b></font>")
                    Case "N"
                        'alle zeilen löschen die keine Änderung hatten außer die Sie sind freigegeben worden
                        If Not strSAPFreigabeKennzeichen = "X" Then
                            Dim tmprowX() As DataRow
                            tmprowX = DatenTabelle.Select("ORDER_NR='" & CStr(tmpSendRow("Order_Nr")) & "'")
                            DatenTabelle.Rows.Remove(tmprowX(0))
                        Else
                            'wenn sonst kein Anzeigetext zu Neuanlage oder Änderung, dann Freigabetext
                            createAenderungsFormular(tmpSendRow, "<FONT Color=""Green""><b>Vorgang abgeschlossen \ Auftrag freigegeben</b></font>")
                        End If

                    Case "C"
                        createAenderungsFormular(tmpSendRow, "<FONT color=""#0066cc""><b>Vorgang abgeschlossen \ Änderung</b></font>")
                End Select

                pE_AKT.Value = ""
                strSAPDoingKennzeichen = ""


                'Fehlermeldungen Sap Auswerten
                Dim tmpdt As New DataTable()
                Dim tmpRow As DataRow
                tmpdt = DirectCast(pO_GT_FEHL.Value, DataTable)
                killAllDBNullValuesInDataTable(tmpdt)
                tmpRow = tmpdt.NewRow

                If Not tmpdt.Rows.Count = 0 Then
                    strSAPResultMeldung = strSAPResultMeldung & "<p align=""center""><FONT color=""RED"" size=""2""><b><u>Fehler: &nbsp; OrderNr:&nbsp; " & CStr(tmpSendRow("Order_Nr")) & " </u></b></font></p>"

                    For Each tmpRow In tmpdt.Rows
                        strSAPResultMeldung = strSAPResultMeldung & "<p align=""center""><b>Fehler:&nbsp;" & CStr(tmpRow.Item(3)) & "&nbsp;&nbsp;&nbsp;Feld:&nbsp;" & CStr(tmpRow.Item(2)) & "</b></p>"
                    Next

                    strSAPResultMeldung = strSAPResultMeldung & "<br>"
                    'sapFehlertabelle leeren, da es kann, das bei bestimmten szenarien, Z.B. senden fehlerhaften Auftrag + folgesendung Auftrag neu und Freigabe neuer Auftrag (fehlerfrei), die Fehlertabelle die Fehler des ersten Auftrages behält. JJ2007.11.30
                    'erübrigt sich vll durch biztalk, da parameter bei jedem durchlauf neu sind
                    'SAPFehlerTable.Clear()
                End If
            Next


            boolDataChanged = False

        

            'ka bisher wars noch nie benötigt worden, JJU2008.06.03
            '--------------------------------------------------------------------
            'dtSapFehlerTable = SAPFehlerTable.ToADODataTable
            'adorow = dtSapFehlerTable.NewRow
            'For Each adorow In dtSapFehlerTable.Rows
            'Next
            '--------------------------------------------------------------------
        Catch ex As Exception
            Select Case ex.Message
                Case "NOT_FOUND"
                    m_intStatus = -1111
                    m_strMessage = "Keine Fahrzeuge gefunden."

                Case "ERR_LOEVM"
                    m_strMessage = "Verarbeitung abgebrochen: Auftrag wurde schon einmal gelöscht und ist somit nicht mehr anzulegen: OrderNR: " & strOrderNR
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select
            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
            End If
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        Finally
            If intID > -1 Then
                m_objLogApp.WriteStandardDataAccessSAP(intID)
            End If
            con.Close()
            m_blnGestartet = False
        End Try

         End Sub

    Private Sub modifyDatentabelleForPrint()

        Dim dtc As New DataColumn("PrintString", System.String.Empty.GetType)

        DatenTabelle.Columns.Add(dtc)
        dtc = New DataColumn("LinkURL", System.String.Empty.GetType)
        DatenTabelle.Columns.Add(dtc)
        DatenTabelle.AcceptChanges()
        setStatus(1)

    End Sub


    Public Sub fillVorschlagswerte(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "kruell_01.fillVorschlagswerte"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True
        End If


        If dtSAPVorschlagswerte Is Nothing Then
            createSAPTable(dtSAPVorschlagswerte)
        Else
            dtSAPVorschlagswerte.Clear()
        End If


        Dim con As New SAPConnection(m_BizTalkSapConnectionString)
        con.Open()
        Dim intId As Int32
        Try

            Dim cmd As New SAPCommand()
            cmd.Connection = con

            Dim strCom As String

            strCom = "EXEC Z_M_IMP_AUFTRDAT_006 @I_KUNNR=@pI_KUNNR,@GT_WEB=@pI_GT_WEB, "
            strCom = strCom & "@GT_WEB=@pO_GT_WEB OUTPUT OPTION 'disabledatavalidation'"

            cmd.CommandText = strCom

            Dim pI_KUNNR As New SAPParameter("@pI_KUNNR", ParameterDirection.Input)

            Dim pI_GT_WEB As New SAPParameter("@pI_GT_WEB", dtSAPVorschlagswerte)
            Dim pO_GT_WEB As New SAPParameter("@pO_GT_WEB", ParameterDirection.Output)

            'Importparameter hinzufügen
            cmd.Parameters.Add(pI_KUNNR)

            'tabelle Hinzufügen
            cmd.Parameters.Add(pI_GT_WEB)
            cmd.Parameters.Add(pO_GT_WEB)

            pI_KUNNR.Value = Right("0000000000" & m_objUser.KUNNR, 10)

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If

            m_intStatus = 0
            m_strMessage = ""
            intId = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_IMP_AUFTRDAT_006", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)
            cmd.ExecuteNonQuery()
            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, True)
            End If


            dtSAPVorschlagswerte = DirectCast(pO_GT_WEB.Value, DataTable)

            If dtSAPVorschlagswerte.Rows.Count = 0 Then
                m_intStatus = 0
                m_strMessage = "Keine Daten gefunden."
            End If

        Catch ex As Exception
            Select Case ex.Message
                Case "NOT_FOUND"
                    m_intStatus = -1111
                    m_strMessage = "Keine Fahrzeuge gefunden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select
            If m_intStatus = -1 Then
                m_objLogApp.WriteEndDataAccessSAP(m_intStatus, False, m_strMessage)
            End If
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        Finally
            If m_intStatus > -1 Then
                m_objLogApp.WriteStandardDataAccessSAP(m_intStatus)
            End If
            con.Close()
            'objSAP.Connection.Close()
            'objSAP.Dispose()
            m_blnGestartet = False
        End Try
    End Sub


    Private Sub createSAPTable(ByRef sapTable As DataTable)
        sapTable = New DataTable

        Dim dataColumns(10) As DataColumn

        dataColumns(0) = New DataColumn("MANDT", System.Type.GetType("System.String"))
        dataColumns(1) = New DataColumn("KUNNR", System.Type.GetType("System.String"))
        dataColumns(2) = New DataColumn("KENNUNG", System.Type.GetType("System.String"))
        dataColumns(3) = New DataColumn("POS_KURZTEXT", System.Type.GetType("System.String"))
        dataColumns(4) = New DataColumn("POS_TEXT", System.Type.GetType("System.String"))

        dataColumns(5) = New DataColumn("NAME1", System.Type.GetType("System.String"))
        dataColumns(6) = New DataColumn("NAME2", System.Type.GetType("System.String"))
        dataColumns(7) = New DataColumn("STRAS", System.Type.GetType("System.String"))
        dataColumns(8) = New DataColumn("PSTLZ", System.Type.GetType("System.String"))
        dataColumns(9) = New DataColumn("ORT01", System.Type.GetType("System.String"))

        sapTable.Columns.AddRange(dataColumns)
        sapTable.AcceptChanges()

    End Sub




    Private Sub loeschDatainSap(ByVal OrderNr As String)
        m_strClassAndMethod = "kruell_01.FILL"
        m_strAppID = m_strAppID
        m_strSessionID = m_strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True
        End If

        Dim objSAP As New SAPProxy_Kruell.SAPProxy_Kruell()

        objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
        objSAP.Connection.Open()

        Dim intID As Int32 = -1

        Try


            Dim strResultDelete As String = ""
            objSAP.Z_M_Imp_Auftrdat_003(Right("0000000000" & m_objUser.KUNNR, 10), OrderNr, m_objUser.UserName, strResultDelete)
            objSAP.CommitWork()

            If strResultDelete = "X" Then
                strSAPResultMeldung = strSAPResultMeldung & "<p align=""center""><b>OrderNr:&nbsp;" & OrderNr.ToString & "&nbsp;gelöscht </b></p>"
            End If
            intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Historie_Zb2", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)
            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, True)
            End If

        Catch ex As Exception
            Select Case ex.Message
                Case "NOT_FOUND"
                    m_intStatus = -1111
                    m_strMessage = "Keine Fahrzeuge gefunden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select
            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
            End If
        Finally
            If intID > -1 Then
                m_objLogApp.WriteStandardDataAccessSAP(intID)
            End If
            objSAP.Connection.Close()
            objSAP.Dispose()
            m_blnGestartet = False
        End Try

    End Sub


    Public Function createAenderungsFormular(ByVal sapRow As DataRow, ByVal strStatus As String, Optional ByVal earlyPrinting As Boolean = False) As String()

        Dim strHtmlCode As String
        Dim strHtmlCodeComplete As String
        Dim iLaenger As Int32
        Dim aStrSplitted1() As String
        Dim tmpstr1 As String
        Dim aStrSplitted2() As String
        Dim tmpstr2 As String
        Dim aStrSplitted3() As String


        Dim i As Int32

        Dim strBuild As New System.Text.StringBuilder(String.Empty)

        Dim strFA As String = "<font size=""1"" style=""Arial""><b>"
        Dim strFZ As String = "</font></b>"
        Dim strTrennLinie As String = "<tr><td colspan=""7""><P align=""center"">-----------------------------------------------------------------------------------------------</p></td></tr>"
        Dim strTrennLinie2 As String = "<tr><td colspan=""7""><P align=""Left"">*****************************************************************************</p></td></tr>"
        strHtmlCode = "<table cellpadding=""0"" cellpadding=""0"" height=""100%"" width=""630px"">"



        '7spaltige tabelle
        strHtmlCode = strHtmlCode & strTrennLinie2 'für Anfang des des Auftrages

        'Freigabedatum prüfen
        Try
            If IsDate(CStr(sapRow("Dat_Freig"))) Then 'normales Datumsformat
                tmpstr1 = CStr(sapRow("Dat_Freig"))
            Else 'sapDatumsformat
                tmpstr1 = CStr(MakeDateStandard(CStr(sapRow("Dat_Freig"))))
                If tmpstr1 = "01.01.1900" Then
                    tmpstr1 = ""
                End If
            End If

          
        Catch
            tmpstr1 = ""
        End Try

        strHtmlCode = strHtmlCode & generateNormalHTMLROW("Freigabe Benutzer", CStr(sapRow("Web_User_Freig")), "", "", "Freigabedatum", tmpstr1, "")
        strHtmlCode = strHtmlCode & strTrennLinie 'Für Trennung zur Freigabe
        'FakeDatum bei neuen Aufträgen, da das Erfassungsdatum von SAP gefüllt wird, und somit erst vorhanden ist wenn der Auftrag erneut abgerufen wird, sie wollen aber wenn Sie einen Ausdruck eines Auftrags machen,den Sie neu angelegt haben, ein Datum haben. 
        If CStr(sapRow("Dat_Erf_Auftr")) = "" Then
            strHtmlCode = strHtmlCode & generateNormalHTMLROW("KundenNummer", CStr(sapRow("Kunnr_Ln")), "", "", "Auftragserfassung", Today.ToShortDateString, "")
        Else
            If IsDate(CStr(sapRow("Dat_Erf_Auftr"))) Then 'im normalen datumsformat
                strHtmlCode = strHtmlCode & generateNormalHTMLROW("KundenNummer", CStr(sapRow("Kunnr_Ln")), "", "", "Auftragserfassung", CStr(sapRow("Dat_Erf_Auftr")), "")
            Else 'als sapDatumsFormat
                strHtmlCode = strHtmlCode & generateNormalHTMLROW("KundenNummer", CStr(sapRow("Kunnr_Ln")), "", "", "Auftragserfassung", CStr(MakeDateStandard(CStr(sapRow("Dat_Erf_Auftr")))), "")
            End If
        End If


        strHtmlCode = strHtmlCode & strTrennLinie
        strHtmlCode = strHtmlCode & generateUeberschriftHTMLROW("1) Kunde")
        strHtmlCode = strHtmlCode & strTrennLinie

        strHtmlCode = strHtmlCode & generateNormalHTMLROW("Kundenname", CStr(sapRow("Name_Ln")), "", "", "Nutzer/Fahrer", "Vorname", CStr(sapRow("Ap_Name1_Ln")))
        strHtmlCode = strHtmlCode & generateNormalHTMLROW("", "", "", "", "", "Nachname", CStr(sapRow("Ap_Name2_Ln")))
        strHtmlCode = strHtmlCode & generateNormalHTMLROW("Straße/Nr", CStr(sapRow("Str_Hnr_Ln")), "", "", "Handynummer", CStr(sapRow("Handy_Ln")), "")
        strHtmlCode = strHtmlCode & generateNormalHTMLROW("PLZ/ORT", CStr(sapRow("Plz_Ln")) & " " & CStr(sapRow("Stadt_Ln")), "", "", "sonstiges", CStr(sapRow("Sonst_Ln")), "")
        strHtmlCode = strHtmlCode & generateNormalHTMLROW("Telefon", CStr(sapRow("Tel_Ln")), "", "", "Versicherugnsges.", CStr(sapRow("Vers")), "")
        strHtmlCode = strHtmlCode & generateNormalHTMLROW("Fax", CStr(sapRow("Fax_Ln")), "", "", "", "", "")
        strHtmlCode = strHtmlCode & generateNormalHTMLROW("Fahrzeugtyp", CStr(sapRow("Car_Type")), "", "", "Order Nr.", CStr(sapRow("Order_Nr")), "")
        strHtmlCode = strHtmlCode & generateNormalHTMLROW("", "", "", "", "Nr.-Leasinggesl.", CStr(sapRow("Kunnr_Lg")), "")
        strHtmlCode = strHtmlCode & generateNormalHTMLROW("", "", "", "", "Leasinggeber", CStr(sapRow("Name_Lg")), "")
        strHtmlCode = strHtmlCode & generateNormalHTMLROW("Fahrgestellnummer", CStr(sapRow("Chassis_Num")), "", "", "Leasing-Nr", CStr(sapRow("Car_Bestnr")), "")

        strHtmlCode = strHtmlCode & strTrennLinie
        strHtmlCode = strHtmlCode & generateUeberschriftHTMLROW("2) Fahrzeugaufbereitung")
        strHtmlCode = strHtmlCode & strTrennLinie

        'firma für Fahrzeugaufgebreiteung
        aStrSplitted1 = CStr(sapRow("Fa_Aufber")).Split(CChar("|"))
        strHtmlCode = strHtmlCode & generateNormalHTMLROW("durch Firma", "Name1", aStrSplitted1(0), "", "", "", "")
        strHtmlCode = strHtmlCode & generateNormalHTMLROW("", "Name2", aStrSplitted1(1), "", "", "", "")
        strHtmlCode = strHtmlCode & generateNormalHTMLROW("", "Straße/Hausnummer", aStrSplitted1(2), "", "", "", "")
        strHtmlCode = strHtmlCode & generateNormalHTMLROW("", "Postleitzahl", aStrSplitted1(3), "", "", "", "")
        strHtmlCode = strHtmlCode & generateNormalHTMLROW("", "Ort", aStrSplitted1(4), "", "", "", "")
        strHtmlCode = strHtmlCode & generateNormalHTMLROW("Aufbereitungsart", "", "", "", "Ausstattung", "", "")

        'parallel laufende Werte, die je nach dem welcher länger so viele Rows generiert werden und es muss beachtet werden einen Leerstring bei dem anderen zu setzten wenn dieser geendet ist. JJ2007.11.27
        aStrSplitted1 = CStr(sapRow("Aufber_Art")).Split(CChar("|"))
        aStrSplitted2 = CStr(sapRow("Ausstatt")).Split(CChar("|"))

        If aStrSplitted1.Length >= aStrSplitted2.Length Then
            iLaenger = aStrSplitted1.Length
        Else
            iLaenger = aStrSplitted2.Length
        End If

        For i = 0 To iLaenger - 1
            If i > aStrSplitted1.Length - 1 Then
                tmpstr1 = ""
            Else
                tmpstr1 = aStrSplitted1(i)
            End If
            If i > aStrSplitted2.Length - 1 Then
                tmpstr2 = ""
            Else
                tmpstr2 = aStrSplitted2(i)
            End If
            strHtmlCode = strHtmlCode & generateNormalHTMLROW("", findVorschlagswertTextByShortText("AUFBEREITUNGSART", tmpstr1), "", "", "", findVorschlagswertTextByShortText("AUSSTATTUNG", tmpstr2), "")
        Next

        'betankungs Checkbox
        If CStr(sapRow("Car_Tank")) = "0" Then
            tmpstr1 = "nein"
        Else
            tmpstr1 = "ja"
        End If
        strHtmlCode = strHtmlCode & generateNormalHTMLROW("Betankungsorder", tmpstr1, "", "", "sonstige Optionen", CStr(sapRow("Sonst_Opt")), "")

        If CStr(sapRow("Vorhol")) = "X" Then
            tmpstr1 = "ja"
        Else
            tmpstr1 = "nein"
        End If
        strHtmlCode = strHtmlCode & generateNormalHTMLROW("Transportauftrag erforderlich", tmpstr1, "", "", "", "", "")


        strHtmlCode = strHtmlCode & strTrennLinie
        strHtmlCode = strHtmlCode & generateUeberschriftHTMLROW("3) zusätzliche Einbauten")
        strHtmlCode = strHtmlCode & strTrennLinie

        'Aufbereitung durch Firma, Eibauten mit Position und Positionsbeschreibung
        '--------------------------------------------------------------------------------
        aStrSplitted1 = CStr(sapRow("Sondeinb_Pos")).Split(CChar("|")) 'Diese beiden müssen konsistent in der länge sein
        aStrSplitted2 = CStr(sapRow("Aufber_Pos")).Split(CChar("|")) 'Diese beiden müssen konsistent in der länge sein
        aStrSplitted3 = CStr(sapRow("Fa_Zuseinbaut")).Split(CChar("|")) 'ist der AdressString für die Adresse der Firma 

        i = 0 'zähler zum vergleich zwischen der Anzahl der Adressfelder und den Einbaupositionen
        iLaenger = aStrSplitted1.Length - 1 'Anzahl der Einbaupositionen
        'In der Tabelle hat die Firma 5 Felder(Links), die Positionen der Aufbereitung Rechts sind beliebig lang, je nach dem Welche Seite Länger ist muss auf der Gegenseite ein LeerString in die Row geschrieben werden. JJ2007.11.27


        strHtmlCode = strHtmlCode & generateNormalHTMLROW("durch Firma:(z.B. AES/Wollnik.)", "Name1", aStrSplitted3(0), "", "Einbau", "", "")

        If i > iLaenger Then
            tmpstr1 = ""
            tmpstr2 = ""
        Else
            tmpstr1 = aStrSplitted1(i)
            tmpstr2 = aStrSplitted2(i)
        End If
        strHtmlCode = strHtmlCode & generateNormalHTMLROW("", "Name2", aStrSplitted3(1), "", "", findVorschlagswertTextByShortText("EINBAU", tmpstr1), tmpstr2)
        i = i + 1

        If i > iLaenger Then
            tmpstr1 = ""
            tmpstr2 = ""
        Else
            tmpstr1 = aStrSplitted1(i)
            tmpstr2 = aStrSplitted2(i)
        End If
        strHtmlCode = strHtmlCode & generateNormalHTMLROW("", "Straße/Hausnummer", aStrSplitted3(2), "", "", findVorschlagswertTextByShortText("EINBAU", tmpstr1), tmpstr2)
        i = i + 1

        If i > iLaenger Then
            tmpstr1 = ""
            tmpstr2 = ""
        Else
            tmpstr1 = aStrSplitted1(i)
            tmpstr2 = aStrSplitted2(i)
        End If
        strHtmlCode = strHtmlCode & generateNormalHTMLROW("", "Postleitzahl", aStrSplitted3(3), "", "", findVorschlagswertTextByShortText("EINBAU", tmpstr1), tmpstr2)
        i = i + 1

        If i > iLaenger Then
            tmpstr1 = ""
            tmpstr2 = ""
        Else
            tmpstr1 = aStrSplitted1(i)
            tmpstr2 = aStrSplitted2(i)
        End If
        strHtmlCode = strHtmlCode & generateNormalHTMLROW("", "Ort", aStrSplitted3(4), "", "", findVorschlagswertTextByShortText("EINBAU", tmpstr1), tmpstr2)


        'soltlen noch mehr Einbaupositionen vorhanden sein, diese alle schreiben 
        If iLaenger > i Then
            For i = i To iLaenger
                strHtmlCode = strHtmlCode & generateNormalHTMLROW("", "", "", "", "", findVorschlagswertTextByShortText("EINBAU", aStrSplitted1(i)), aStrSplitted2(i))
            Next
        End If



        'Winterradposition drucken JJU2008.09.22
        '--------------------------------------------------------------------------------
        aStrSplitted1 = CStr(sapRow("WINTERRAD_POS")).Split(CChar("|")) '
        aStrSplitted2 = CStr(sapRow("FA_WINTERRAD")).Split(CChar("|")) 'ist der AdressString für die Adresse der Firma 
        i = 0 'zähler zum vergleich zwischen der Anzahl der Adressfelder und den Winterradpositionen
        iLaenger = aStrSplitted1.Length - 1 'Anzahl der Einbaupositionen
        'In der Tabelle hat die Firma 5 Felder(Links), die Positionen der Aufbereitung Rechts sind beliebig lang, je nach dem Welche Seite Länger ist muss auf der Gegenseite ein LeerString in die Row geschrieben werden. JJ2007.11.27

      

        strHtmlCode = strHtmlCode & generateNormalHTMLROW("Firma Winterräder", "Name1", aStrSplitted2(0), "", "Winterrädertyp", "", "")

        If i > iLaenger Then
            tmpstr1 = ""
        Else
            tmpstr1 = aStrSplitted1(i)
        End If
        strHtmlCode = strHtmlCode & generateNormalHTMLROW("", "Name2", aStrSplitted2(1), "", "", findVorschlagswertTextByShortText("WINTERRAD_POS", tmpstr1), "")
        i = i + 1

        If i > iLaenger Then
            tmpstr1 = ""
        Else
            tmpstr1 = aStrSplitted1(i)
        End If
        strHtmlCode = strHtmlCode & generateNormalHTMLROW("", "Straße/Hausnummer", aStrSplitted2(2), "", "", findVorschlagswertTextByShortText("WINTERRAD_POS", tmpstr1), "")
        i = i + 1

        If i > iLaenger Then
            tmpstr1 = ""
        Else
            tmpstr1 = aStrSplitted1(i)
        End If
        strHtmlCode = strHtmlCode & generateNormalHTMLROW("", "Postleitzahl", aStrSplitted2(3), "", "", findVorschlagswertTextByShortText("WINTERRAD_POS", tmpstr1), "")
        i = i + 1

        If i > iLaenger Then
            tmpstr1 = ""

        Else
            tmpstr1 = aStrSplitted1(i)

        End If
        strHtmlCode = strHtmlCode & generateNormalHTMLROW("", "Ort", aStrSplitted2(4), "", "", findVorschlagswertTextByShortText("WINTERRAD_POS", tmpstr1), "")


        'soltlen noch mehr Einbaupositionen vorhanden sein, diese alle schreiben 
        If iLaenger > i Then
            For i = i To iLaenger
                strHtmlCode = strHtmlCode & generateNormalHTMLROW("", "", "", "", "", findVorschlagswertTextByShortText("WINTERRAD_POS", aStrSplitted1(i)), "")
            Next
        End If

        '--------------------------------------------------------------------------------


        strHtmlCode = strHtmlCode & strTrennLinie
        strHtmlCode = strHtmlCode & generateUeberschriftHTMLROW("4) Zulassung und Art")
        strHtmlCode = strHtmlCode & strTrennLinie

        strHtmlCode = strHtmlCode & generateNormalHTMLROW("Zulassung und Art", CStr(sapRow("Zul_Art")), "", "", "", "", "")
        strHtmlCode = strHtmlCode & generateNormalHTMLROW("Zulassungsdienst Name", findVorschlagswertTextByShortText("ZULASSUNGSDIENST", CStr(sapRow("Zul_Dienst"))), "", "", "", "", "")


        'abweichender Halter adresse
        aStrSplitted1 = CStr(sapRow("Abw_Halter")).Split(CChar("|"))
        strHtmlCode = strHtmlCode & generateNormalHTMLROW("abw. Halter/Zulassung", "Name1", aStrSplitted1(0), "", "", "", "")
        strHtmlCode = strHtmlCode & generateNormalHTMLROW("", "Name2", aStrSplitted1(1), "", "", "", "")
        strHtmlCode = strHtmlCode & generateNormalHTMLROW("", "Straße/Hausnummer", aStrSplitted1(2), "", "", "", "")
        strHtmlCode = strHtmlCode & generateNormalHTMLROW("", "Postleitzahl", aStrSplitted1(3), "", "", "", "")
        strHtmlCode = strHtmlCode & generateNormalHTMLROW("", "Ort", aStrSplitted1(4), "", "", "", "")

        'Wunschkennzeichen reserviert Checkbox
        If CStr(sapRow("W_Kennz_Res")) = "0" Then
            tmpstr1 = "nein"
        Else
            tmpstr1 = "ja"
        End If
        strHtmlCode = strHtmlCode & generateNormalHTMLROW("Wunschkennzeichen", CStr(sapRow("W_Kennz")), "", "", "WKZ reser.:", tmpstr1, "")


        strHtmlCode = strHtmlCode & strTrennLinie
        strHtmlCode = strHtmlCode & generateUeberschriftHTMLROW("5) Auslieferung/Übergabe")
        strHtmlCode = strHtmlCode & strTrennLinie

        'verbringungsdatum prüfen/ ist nur TEXT !! JJ2007.12.14
        'Try
        '    tmpstr1 = CStr(MakeDateStandard(sapRow.Gew_Verbr_Dat))
        '    If tmpstr1 = "01.01.1900" Then
        '        tmpstr1 = ""
        '    End If
        'Catch
        '    tmpstr1 = ""
        'End Try

        'strHtmlCode = strHtmlCode & generateNormalHTMLROW("Verbringungsdatum", tmpstr1, "", "", "", "", "")

        strHtmlCode = strHtmlCode & generateNormalHTMLROW("Verbringungsdatum", CStr(sapRow("Gew_Verbr_Dat")), "", "", "", "", "")
        strHtmlCode = strHtmlCode & generateNormalHTMLROW("Lieferung durch", findVorschlagswertTextByShortText("LIEFERUNGDURCH", CStr(sapRow("Fa_Ueberf"))), "", "", "", "", "")
        'abweichende Lieferanschrift
        aStrSplitted1 = CStr(sapRow("Abw_Lief_Adr")).Split(CChar("|"))
        strHtmlCode = strHtmlCode & generateNormalHTMLROW("abw. Lieferanschrift", "Name1", aStrSplitted1(0), "", "", "", "")
        strHtmlCode = strHtmlCode & generateNormalHTMLROW("", "Name2", aStrSplitted1(1), "", "", "", "")
        strHtmlCode = strHtmlCode & generateNormalHTMLROW("", "Straße/Hausnummer", aStrSplitted1(2), "", "", "", "")
        strHtmlCode = strHtmlCode & generateNormalHTMLROW("", "Postleitzahl", aStrSplitted1(3), "", "", "", "")
        strHtmlCode = strHtmlCode & generateNormalHTMLROW("", "Ort", aStrSplitted1(4), "", "", "", "")
        strHtmlCode = strHtmlCode & generateNormalHTMLROW("Bemerkung zur Verbringung", CStr(sapRow("Car_Bem")), "", "", "", "", "")

        strHtmlCode = strHtmlCode & strTrennLinie
        strHtmlCode = strHtmlCode & generateUeberschriftHTMLROW("6) Rückführung Fahrzeug")
        strHtmlCode = strHtmlCode & strTrennLinie

        'Rückfahrzeug Checkbox
        If CStr(sapRow("Car_Return")) = "0" Then
            tmpstr1 = "nein"
        Else
            tmpstr1 = "ja"
        End If
        'Rücklieferanschrift
        aStrSplitted1 = CStr(sapRow("Car_Ret_Adr")).Split(CChar("|"))


        strHtmlCode = strHtmlCode & generateNormalHTMLROW("Rückfahrzeug", tmpstr1, "", "", "Ablieferanschrift Rückl.", "Name1", aStrSplitted1(0))
        strHtmlCode = strHtmlCode & generateNormalHTMLROW("Fahrzeug", CStr(sapRow("Car_Ret_Besch")), "", "", "", "Name2", aStrSplitted1(1))
        strHtmlCode = strHtmlCode & generateNormalHTMLROW("amtl. Kennzeichen", CStr(sapRow("Lic_Num_Return")), "", "", "", "Straße/Hausnummer", aStrSplitted1(2))
        strHtmlCode = strHtmlCode & generateNormalHTMLROW("Leasinggeber", CStr(sapRow("Angaben_Lg")), "", "", "", "Postleitzahl", aStrSplitted1(3))
        strHtmlCode = strHtmlCode & generateNormalHTMLROW("", "", "", "", "", "Ort", aStrSplitted1(4))
        strHtmlCode = strHtmlCode & generateNormalHTMLROW("", "", "", "", "Bemerkung Rückfahrzeug", CStr(sapRow("Bem_Rueckfzg")), "")

        strHtmlCode = strHtmlCode & strTrennLinie
        strHtmlCode = strHtmlCode & generateUeberschriftHTMLROW("7) Bemerkungen")
        strHtmlCode = strHtmlCode & strTrennLinie


        Dim KompletteBemerkung As String = CStr(sapRow("Bem_Suz")) & CStr(sapRow("Bem_Suz2"))
        strHtmlCode = strHtmlCode & generateNormalHTMLROW("Ider ZUL-Unterlagen", KompletteBemerkung, "", "", "", "", "")

        strHtmlCode = strHtmlCode & strTrennLinie2 'für ende des des Auftrages



        replaceGerUmlauteForHTML(strHtmlCode)

        strHtmlCodeComplete = "<html><head><title>(Druckversion OrderNr: " & CStr(sapRow("Order_Nr")) & ")</title></head><body bgcolor=""#FFFFFF""><font size=""2"" style=""Arial""><b><a href=""javascript:window.print()"">Auftrag drucken...</a></b></font>" & strHtmlCode & "</body></html>"

        'Druckdaten zu Datentabelle hinzufügen
        Dim strDate As String

        strDate = Date.Now.Year.ToString & Date.Now.Month.ToString & Date.Now.Day.ToString & Date.Now.Hour.ToString & Date.Now.Minute.ToString & Date.Now.Second.ToString
        strDate = strDate & CStr(sapRow("Order_Nr")).Replace("/", "_")

        If earlyPrinting Then
            'Krüll wollte schon bei auftragsanzeige eine druckausgabe, daher muss darauf abgefragt werden, weil in diesem stadium die datentabelle noch nicht existiert JJU2008.06.12
            Dim backArray(1) As String
            backArray(0) = strHtmlCodeComplete
            backArray(1) = "/Portal/Temp/Excel/" & strDate & ".htm"
            Return backArray
        Else
            Dim tmprow() As DataRow
            tmprow = DatenTabelle.Select("ORDER_NR='" & CStr(sapRow("Order_Nr")) & "'")
            tmprow(0).Item("PrintString") = strHtmlCodeComplete
            tmprow(0).Item("LinkURL") = "/Portal/Temp/Excel/" & strDate & ".htm"
            tmprow(0).Item("Status") = strStatus
            Return Nothing
        End If


    End Function

    Private Function generateNormalHTMLROW(ByVal column1 As String, ByVal column2 As String, ByVal column3 As String, ByVal column4Leer As String, ByVal column5 As String, ByVal column6 As String, ByVal column7 As String) As String
        '7spaltige Row
        Dim HTMLROW As String
        HTMLROW = "<tr><td width=""85""> <FONT size=""1""><b>" & column1 & "</b></FONT></td><td width=""115""><FONT size=""1"">" & column2 & "</FONT></td><td width=""110""><FONT size=""1"">" & column3 & "</FONT></td><td width=""10""><FONT size=""1"">" & column4Leer & " </FONT></td><td width=""85""><FONT size=""1""><b> " & column5 & " </b></FONT></td><td width=""115""><FONT size=""1"">" & column6 & "</FONT></td> <td width=""110""><FONT size=""1"">" & column7 & "</FONT></td></tr>"
        Return HTMLROW
    End Function

    Private Function generateUeberschriftHTMLROW(ByVal ueberbschrift As String) As String
        Dim htmlROW As String

        Dim strUeberschriftBegin As String = "<tr><td colspan=""7""><FONT size=""2""><P align=""center""><U><B>"
        Dim strUeberschriftEnd As String = "</B></U></P></FONT></td></tr>"
        htmlROW = strUeberschriftBegin & ueberbschrift & strUeberschriftEnd
        Return htmlROW
    End Function


    Private Function findVorschlagswertTextByShortText(ByVal kennung As String, ByVal shortText As String) As String
        Dim rows() As DataRow
        rows = Vorschlagswerte.Select("POS_KURZTEXT='" & shortText & "' and Kennung='" & kennung & "'")
        If rows.Length > 1 Then
            Throw New Exception("Keine eindeutige Ermittlung des POS_TEXT zum POS_KURZTEXT: " & shortText)
        Else
            If rows.Length = 0 Then
                'text nicht vorhanden wert aus Datarow Nehmen JJU2008.07.04
                Return shortText
            Else
                'genau ein wert gefunden
                Return CStr(rows(0)("POS_TEXT"))
            End If
        End If
    End Function

    Private Sub replaceGerUmlauteForHTML(ByRef htmlCode As String)
        htmlCode = htmlCode.Replace("ß", "&szlig;")
        htmlCode = htmlCode.Replace("ä", "&auml;")
        htmlCode = htmlCode.Replace("Ä", "&Auml;")
        htmlCode = htmlCode.Replace("ö", "&ouml;")
        htmlCode = htmlCode.Replace("Ö", "&Ouml;")
        htmlCode = htmlCode.Replace("ü", "&uuml;")
        htmlCode = htmlCode.Replace("Ü", "&Uuml;")
        htmlCode = htmlCode.Replace("€", "&euro;")
    End Sub

#End Region



End Class
' ************************************************
' $History: kruell_01.vb $
' 
' *****************  Version 15  *****************
' User: Rudolpho     Date: 30.04.09   Time: 17:24
' Updated in $/CKAG/Applications/AppKruell/Lib
' 
' *****************  Version 14  *****************
' User: Jungj        Date: 15.12.08   Time: 9:06
' Updated in $/CKAG/Applications/AppKruell/Lib
' Fehleranalyse, "printstring" column. kleine Änderung zur sicherheit
' 
' *****************  Version 13  *****************
' User: Jungj        Date: 29.10.08   Time: 9:39
' Updated in $/CKAG/Applications/AppKruell/Lib
' BugFix bei neuanlage von aufträgen
' 
' *****************  Version 12  *****************
' User: Jungj        Date: 29.09.08   Time: 10:50
' Updated in $/CKAG/Applications/AppKruell/Lib
' SAP Logging vervollständigt
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 22.09.08   Time: 14:36
' Updated in $/CKAG/Applications/AppKruell/Lib
' ITA 2129
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 20.08.08   Time: 16:32
' Updated in $/CKAG/Applications/AppKruell/Lib
' ITA 2174
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 16.07.08   Time: 18:15
' Updated in $/CKAG/Applications/AppKruell/Lib
' ITA 2026 Bugfixes
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 15.07.08   Time: 13:07
' Updated in $/CKAG/Applications/AppKruell/Lib
' ITA 2071
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 7.07.08    Time: 8:24
' Updated in $/CKAG/Applications/AppKruell/Lib
' ITA 2026
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 25.06.08   Time: 8:26
' Updated in $/CKAG/Applications/AppKruell/Lib
' ITA 2025
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 20.06.08   Time: 16:03
' Updated in $/CKAG/Applications/AppKruell/Lib
' ITA 2022 fertig
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 12.06.08   Time: 14:47
' Updated in $/CKAG/Applications/AppKruell/Lib
' ITA 1999
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 29.05.08   Time: 10:54
' Updated in $/CKAG/Applications/AppKruell/Lib
' ITA 1955
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 21.05.08   Time: 14:53
' Updated in $/CKAG/Applications/AppKruell/Lib
' ITA 1923
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 14:37
' Created in $/CKAG/Applications/AppKruell/Lib
' 
' *****************  Version 49  *****************
' User: Jungj        Date: 7.03.08    Time: 10:22
' Updated in $/CKG/Applications/AppKruell/AppKruellWeb/Lib
' Ordernummer in Großbuchstaben umsetzen
' 
' *****************  Version 48  *****************
' User: Jungj        Date: 24.01.08   Time: 13:37
' Updated in $/CKG/Applications/AppKruell/AppKruellWeb/Lib
' BUGFIX Bei SONDEINB_POS/AUFBER_POS ungleiche Arraylängen
' 
' *****************  Version 47  *****************
' User: Jungj        Date: 9.01.08    Time: 17:09
' Updated in $/CKG/Applications/AppKruell/AppKruellWeb/Lib
' BugFix ITA 1400
' 
' *****************  Version 46  *****************
' User: Jungj        Date: 9.01.08    Time: 13:57
' Updated in $/CKG/Applications/AppKruell/AppKruellWeb/Lib
' ITA 1580 Report01 hinzugefügt, SS History Bodys hinzugefügt
' ************************************************


