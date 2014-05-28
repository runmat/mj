Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Change05_4
    Inherits System.Web.UI.Page

#Region " Vom Web Form Designer generierter Code "

    'Dieser Aufruf ist für den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: Diese Methode ist für den Web Form-Designer erforderlich
        'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
        InitializeComponent()
    End Sub

#End Region

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private objSuche As FFE_Search
    Private objAddressList As FFE_Search
    Private objHaendler As FFE_Haendler
    Private preise As DataTable
    Private angefordert As Integer  'Fahrzeuge, die laut Preisstaffelung berechnet werden (nicht gesperrt angelegt)
    Private preis_stueck As Decimal 'Preis pro Fahrzeug

    Protected WithEvents Kopfdaten1 As Kopfdaten
    Protected WithEvents lnkFahrzeugsuche As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkFahrzeugAuswahl As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkAdressAuswahl As System.Web.UI.WebControls.HyperLink
    Protected WithEvents ucHeader As Header
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblAddress As System.Web.UI.WebControls.Label
    Protected WithEvents lblVersandart As System.Web.UI.WebControls.Label
    Protected WithEvents lblMaterialNummer As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
    Protected WithEvents lnkAntrag As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblHalter As System.Web.UI.WebControls.Label
    Protected WithEvents lblEmpf As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        Kopfdaten1.Message = ""
        lnkFahrzeugAuswahl.NavigateUrl = "Change05_2.aspx?AppID=" & Session("AppID").ToString
        lnkFahrzeugsuche.NavigateUrl = "Change05.aspx?AppID=" & Session("AppID").ToString
        lnkAdressAuswahl.NavigateUrl = "Change05_3.aspx?AppID=" & Session("AppID").ToString

        If Request.QueryString("AppID").Length > 0 Then
            Session("AppID") = Request.QueryString("AppID").ToString
        End If
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text

            m_App = New Base.Kernel.Security.App(m_User)

            If Session("objHaendler") Is Nothing Then
                Response.Redirect("Change05.aspx?AppID=" & Session("AppID").ToString)
            End If

            If (Session("objSuche") Is Nothing) Then
                Response.Redirect("Change05.aspx?AppID=" & Session("AppID").ToString)
            Else
                objSuche = CType(Session("objSuche"), FFE_Search)
            End If

            Kopfdaten1.UserReferenz = m_User.Reference
            Kopfdaten1.HaendlerNummer = objSuche.REFERENZ
            Dim strTemp As String = objSuche.NAME
            If objSuche.NAME_2.Length > 0 Then
                strTemp &= "<br>" & objSuche.NAME_2
            End If
            Kopfdaten1.HaendlerName = strTemp
            Kopfdaten1.Adresse = objSuche.COUNTRYISO & " - " & objSuche.POSTL_CODE & " " & objSuche.CITY & "<br>" & objSuche.STREET

            Session("objSuche") = objSuche

            objHaendler = CType(Session("objHaendler"), FFE_Haendler)

            If Not IsPostBack Then
                InitialLoad()
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub InitialLoad()
        'Dim i As Integer
        Kopfdaten1.Kontingente = objHaendler.Kontingente
        '---------------Preistabelle anzeigen (HEZ)
        '§§§JVE 13.07.2005: wird nicht mehr benötigt!

        'Dim row As DataRow
        'Dim str As String = String.Empty

        'preise = objHaendler.getPrices()
        'If (preise.Rows.Count = 0) Then
        '    lblError.Text = "Es konnte keine Preisstaffelung ermittelt werden."
        '    Exit Sub
        'End If
        'For Each row In preise.Rows
        '    str += "ab " & Right("00" & CType(row("KSTBM"), Integer).ToString, 2) & " St. = " & CType(row("KBETR"), String) & " Euro" & "<br>"
        'Next
        'lblPreise.Text = str
        ''---------------------------
        lnkAdressAuswahl.Visible = True
        lnkFahrzeugAuswahl.Visible = True
        lblAddress.Text = Session("SelectedDeliveryText").ToString
        lblHalter.Text = Session("HalterAdresseText").ToString
        lblEmpf.Text = Session("EmpfAdresseText").ToString
        FillGrid()
    End Sub

    Private Sub DoSubmit()
        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)

        objHaendler.StandardLogID = logApp.LogStandardIdentity
        Try
            If Session("SelectedDeliveryValue").ToString.Length = 0 Then
                Response.Redirect(lnkFahrzeugAuswahl.NavigateUrl)
            Else
                Dim tmpDataView As New DataView()
                tmpDataView = objHaendler.Fahrzeuge.DefaultView

                tmpDataView.RowFilter = "MANDT <> '0'"           'Für HEZ

                objHaendler.Adresse = "60" & Session("SelectedDeliveryValue").ToString
                objHaendler.AdresseHalter = "60" & Session("HalterAdresseNummer").ToString    'HEZ
                objHaendler.AdresseEmpf = "60" & Session("EmpfAdresseNummer").ToString  'HEZ

                Dim intItemCounter As Int32
                Dim blnPerformedWithoutError As Boolean = True

                angefordert = 0
                'Briefe anfodern.......
                For intItemCounter = 0 To tmpDataView.Count - 1
                    If CStr(tmpDataView.Item(intItemCounter)("COMMENT")).Length = 0 Then
                        objHaendler.ZZFAHRG = tmpDataView.Item(intItemCounter)("ZZFAHRG").ToString
                        objHaendler.KreditkontrollBereich = tmpDataView.Item(intItemCounter)("MANDT").ToString      '§§§JVE 12.07.2005
                        objHaendler.MaterialNummer = lblMaterialNummer.Text
                        objHaendler.KUNNR = m_User.KUNNR
                        objHaendler.Anfordern(True)

                        If objHaendler.Status = -1111 Then
                            blnPerformedWithoutError = False
                            lblError.Text = objHaendler.Message
                            Exit For
                        End If

                        Dim sngStart As Single = CSng(Microsoft.VisualBasic.Timer)
                        Dim intStart As Int32 = 0
                        Do While CSng(Microsoft.VisualBasic.Timer) < sngStart + 1
                            intStart += 1
                        Loop

                        tmpDataView.Item(intItemCounter)("VBELN") = objHaendler.Auftragsnummer
                        If objHaendler.Auftragsnummer.Length = 0 Then
                            tmpDataView.Item(intItemCounter)("COMMENT") = "Fehler: " & objHaendler.Message
                        Else
                            tmpDataView.Item(intItemCounter)("COMMENT") = objHaendler.Auftragsstatus
                        End If
                        If objHaendler.Auftragsstatus <> "Vorgang OK" Then
                        Else
                            angefordert += 1                'Wenn Vorgang OK, Zähler hochsetzen HEZ
                        End If
                    End If
                Next

                If blnPerformedWithoutError Then
                    'HEZ---------------------------------------------------------------
                    '§§§JVE 13.07.2005: Preise werden nicht mehr benötigt (keine Anzeige).
                    'Dim i As Integer

                    'preise = objHaendler.getPrices()
                    ''get_stueckpreis(angefordert)
                    ''LICENSE_NUM als Preisspalte missbrauchen (bei HEZ keine Verwendung)
                    'For i = 0 To tmpDataView.Table.Rows.Count - 1
                    '    If i < angefordert Then
                    '        'tmpDataView.Table.Rows(i)("LICENSE_NUM") = CType(preis_stueck, String)
                    '    Else
                    '        'tmpDataView.Table.Rows(i)("LICENSE_NUM") = "0*"
                    '    End If
                    'Next
                    '-------------------------------------------------------------------
                    DataGrid1.DataSource = tmpDataView
                    DataGrid1.DataBind()
                    checkGrid()
                    DataGrid1.Columns(13).Visible = True

                    Dim i As Integer
                    Dim tblTemp As New DataTable()

                    With tblTemp.Columns
                        .Add("Kundennr.", System.Type.GetType("System.String"))
                        .Add("Händlernr.", System.Type.GetType("System.String"))
                        .Add("Fahrgestellnr.", System.Type.GetType("System.String"))
                        .Add("Finanzierungsnr.", System.Type.GetType("System.String"))
                        .Add("ZBII-Nummer", System.Type.GetType("System.String"))
                        .Add("Preis", System.Type.GetType("System.String"))
                        .Add("Ordernr.", System.Type.GetType("System.String"))
                        .Add("Equipmentnr.", System.Type.GetType("System.String"))
                        .Add("Bez.", System.Type.GetType("System.Boolean"))
                        .Add("COC Besch.vorh.", System.Type.GetType("System.Boolean"))
                        .Add("Auftragsnr.", System.Type.GetType("System.String"))
                        .Add("Kommentar", System.Type.GetType("System.String"))
                        .Add("Kontingentart", System.Type.GetType("System.String"))
                    End With
                    Dim tmpRow As DataRow
                    For i = 0 To tmpDataView.Count - 1
                        tmpRow = tblTemp.NewRow
                        tmpRow("Kundennr.") = objHaendler.KUNNR.TrimStart("0"c)
                        tmpRow("Händlernr.") = objHaendler.Customer.TrimStart("0"c)
                        tmpRow("Fahrgestellnr.") = tmpDataView(i)("ZZFAHRG")
                        tmpRow("Finanzierungsnr.") = tmpDataView(i)("LIZNR")
                        tmpRow("ZBII-Nummer") = tmpDataView(i)("TIDNR")
                        tmpRow("Preis") = tmpDataView(i)("LICENSE_NUM")
                        tmpRow("Ordernr.") = tmpDataView(i)("ZZREFERENZ1")
                        If Not TypeOf tmpDataView(i)("EQUNR") Is System.DBNull Then
                            tmpRow("Equipmentnr.") = CStr(tmpDataView(i)("EQUNR")).TrimStart("0"c)
                        End If
                        tmpRow("Bez.") = tmpDataView(i)("ZZBEZAHLT")
                        tmpRow("COC Besch.vorh.") = tmpDataView(i)("ZZCOCKZ")
                        tmpRow("Auftragsnr.") = tmpDataView(i)("VBELN")
                        tmpRow("Kommentar") = tmpDataView(i)("COMMENT")
                        Select Case tmpDataView(i)("MANDT").ToString
                            Case "5"
                                tmpRow("Kontingentart") = "HEZ"
                        End Select

                        tblTemp.Rows.Add(tmpRow)
                    Next
                    logApp.UpdateEntry("APP", Session("AppID").ToString, "HEZ-Anforderung zu Adresse-Nr. " & objHaendler.Adresse & " erfolgreich durchgeführt.", tblTemp)
                    Session("objHaendler") = objHaendler    'Merken für Auftragdruck...

                    'objHaendler = New FDD_Haendler(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "", "60" & objSuche.REFERENZ, , True)
                    objHaendler.Show()  'Kopfdaten aktualisieren...
                    Kopfdaten1.Kontingente = objHaendler.Kontingente

                    cmdSave.Visible = False
                    Session("SelectedDeliveryValue") = ""
                    lnkAdressAuswahl.Visible = False
                    lnkFahrzeugAuswahl.Visible = False

                    '§§§JVE 23.06.2005 <begin>
                    'lblMessage.Text = String.Empty
                    lblMessage.Text = "Die nachfolgend aufgeführten Vorgänge wurden zur Freigabe durch Ihre zuständige Ford Bank Filiale erfasst."
                    'lblMessage.CssClass = "LabelExtraLargeBlue"

                    '§§§ <end>                
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Fehler: " & ex.Message
            logApp.UpdateEntry("ERR", Session("AppID").ToString, "Fehler bei der Dokumentanforderung zu Adresse-Nr. " & objHaendler.Adresse & ", Fehler: " & ex.Message & ")")
        End Try
    End Sub

    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        DoSubmit()
    End Sub

    Private Sub FillGrid(Optional ByVal strSort As String = "")
        Dim tmpDataView As New DataView()
        tmpDataView = objHaendler.Fahrzeuge.DefaultView

        If Not strSort.Trim(" "c).Length = 0 Then
            Dim strDirection As String
            If ViewState("Direction") Is Nothing Then
                strDirection = "desc"
            Else
                strDirection = ViewState("Direction").ToString
            End If

            If strDirection = "asc" Then
                strDirection = "desc"
            Else
                strDirection = "asc"
            End If

            tmpDataView.Sort = strSort & " " & strDirection
            ViewState("Direction") = strDirection
        End If

        Dim blnScriptFound As Boolean = False
        Dim intZaehl0005 As Int32 = 0
        Dim blnGesperrteAnforderungen As Boolean = False

        tmpDataView.RowFilter = "MANDT <> '0'"      'Nur diejenigen anzeigen, die ausgewählt wurden
        DataGrid1.DataSource = tmpDataView

        lblMessage.Text = ""

        intZaehl0005 = 0                            'Anforderungen zählen
        Dim row As DataRow
        For Each row In tmpDataView.Table.Rows
            If (row("MANDT").ToString <> "0") Then
                intZaehl0005 += 1
            End If
        Next

        Dim rowTest As DataRow
        angefordert = 0
        For Each rowTest In objHaendler.Kontingente.Rows
            Dim strTemp As String = ""
            Dim intTemp As Int32 = 0
            Select Case rowTest("Kreditkontrollbereich").ToString
                Case "0005"
                    angefordert = intZaehl0005          'Zunächst alle anfordern

                    intTemp = CInt(rowTest("Ausschoepfung")) + intZaehl0005 - CInt(rowTest("Kontingent_Alt"))   'Evtl. gesperrt anzulegende Fahrzeuge
                    If intTemp > 0 And intZaehl0005 > 0 Then
                        '§§§JVE 21.06.2005 <begin>
                        'strTemp = "Überzählige Anforderungen werden gesperrt angelegt. (Ihre Anforderung von " & intZaehl0005.ToString & " Fahrzeug(en) übersteigt Ihr Kontingent um " & intTemp.ToString & " Fahrzeug(e).)<br>"
                        strTemp = "Zur Aktivierung der ausgewählten Vorgänge klicken Sie auf [Absenden].<br>"
                        '§§§<end>
                        blnGesperrteAnforderungen = True
                        angefordert = intZaehl0005 - intTemp
                    End If
                    If CType(rowTest("Gesperrt_Alt"), System.Boolean) And intZaehl0005 > 0 Then
                        strTemp = "<u>Kontingentart - HEZ</u>: Die Anforderungen werden gesperrt angelegt. (Ihr Kontingent ist zur Zeit nicht freigegeben.)<br>"
                        blnGesperrteAnforderungen = True
                        angefordert = 0
                    End If
            End Select
            lblMessage.Text &= strTemp
            'lblMessage.CssClass = "LabelExtraLargeBlue"
        Next

        'If Not lblMessage.Text.Length = 0 Then
        '    lblMessage.Text &= "<hr>"
        'End If

        DataGrid1.DataBind()
        DataGrid1.PagerStyle.Visible = False
        checkGrid()
        Session("ResultTableRaw") = tmpDataView
    End Sub

    Private Sub checkGrid()
        '§§§JVE 13.07.2005 <begin>
        'Radiobuttons für Versandart füllen
        Dim item As DataGridItem
        Dim cell As TableCell
        Dim chkBox As CheckBox
        Dim control As Control
        Dim intZaehl As Int32

        For Each item In DataGrid1.Items
            intZaehl = 1
            Dim mandt As String
            Dim strZZFAHRG As String = ""

            For Each cell In item.Cells
                If intZaehl = 1 Then
                    strZZFAHRG = "ZZFAHRG = '" & cell.Text & "'"
                End If
                For Each control In cell.Controls
                    If TypeOf control Is CheckBox Then
                        chkBox = CType(control, CheckBox)
                        Dim tmpRows As DataRow()
                        tmpRows = objHaendler.Fahrzeuge.Select(strZZFAHRG)
                        mandt = tmpRows(0)("MANDT").ToString

                        Select Case chkBox.ID
                            Case "chk0001"
                                If mandt = "2" Then
                                    chkBox.Checked = True
                                End If
                            Case "chk0002"
                                If mandt = "4" Then
                                    chkBox.Checked = True
                                End If
                            Case "chk0003"
                                If mandt = "3" Then
                                    chkBox.Checked = True
                                End If
                            Case "chkNichtAnfordern"
                                If mandt = "0" Then
                                    chkBox.Checked = True
                                End If
                        End Select
                    End If
                Next
                intZaehl += 1
            Next
        Next

        '§§§JVE 13.07.2005 <end>

    End Sub

    '§§§JVE 13.07.2005: wird nicht mehr benötigt! Preisfindung wird in SAP durchgeführt...
    'Private Sub get_stueckpreis(ByVal fahrzeuge As Int32)
    '    Dim found As Boolean = False
    '    Dim row As Integer = 0

    '    Try
    '        While Not found And row <= preise.Rows.Count - 1
    '            If CType(preise.Rows(row)("KSTBM"), Integer) <= fahrzeuge Then
    '                row += 1
    '            Else
    '                found = True
    '            End If
    '        End While

    '        If found = True Then
    '            If (row = 0) Then
    '                preis_stueck = CType(preise.Rows(row)("KBETR"), Decimal)
    '            Else
    '                preis_stueck = CType(preise.Rows(row - 1)("KBETR"), Decimal)
    '            End If
    '        Else
    '            If (row = preise.Rows.Count) Then
    '                preis_stueck = CType(preise.Rows(row - 1)("KBETR"), Decimal)
    '            End If
    '        End If
    '    Catch ex As Exception
    '        lblError.Text = "Fehler bei der Ermittlung der Gesamtsumme."
    '    End Try
    'End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        FillGrid(e.SortExpression)
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        DoSubmit()
        lnkAntrag.Visible = True
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Change05_4.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 10.06.08   Time: 17:24
' Updated in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 13.05.08   Time: 16:41
' Updated in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 1  *****************
' User: Rudolpho     Date: 5.05.08    Time: 17:09
' Created in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:23
' Created in $/CKAG/Applications/appffd/Forms
' 
' *****************  Version 13  *****************
' User: Uha          Date: 2.07.07    Time: 12:52
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 12  *****************
' User: Rudolpho     Date: 25.06.07   Time: 16:18
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' GetAppIDFromQueryString(Me) rausgenommen
' 
' *****************  Version 11  *****************
' User: Rudolpho     Date: 8.06.07    Time: 15:36
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' Abgleich Beyond Compare
' 
' *****************  Version 10  *****************
' User: Uha          Date: 2.05.07    Time: 17:06
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' AppID fehlte bei Sprung zur Seite Antrag.aspx
' 
' *****************  Version 9  *****************
' User: Uha          Date: 7.03.07    Time: 12:51
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' 
' ************************************************
