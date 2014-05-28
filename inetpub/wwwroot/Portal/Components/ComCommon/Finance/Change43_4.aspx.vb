Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Components.ComCommon
Imports Microsoft.VisualBasic
Public Class Change43_4
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
    Private objSuche As CKG.Components.ComCommon.Finance.Search
    'Private objAddressList As CKG.Components.ComCommon.Finance.Search
    Private objHaendler As fin_06
    Private preise As DataTable
    Private angefordert As Integer  'Fahrzeuge, die laut Preisstaffelung berechnet werden (nicht gesperrt angelegt)
    Private preis_stueck As Decimal 'Preis pro Fahrzeug

    Protected WithEvents Kopfdaten1 As ComCommon.PageElements.Kopfdaten
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
        lnkFahrzeugAuswahl.NavigateUrl = "Change43_2.aspx?AppID=" & Session("AppID").ToString
        lnkFahrzeugsuche.NavigateUrl = "Change43.aspx?AppID=" & Session("AppID").ToString
        lnkAdressAuswahl.NavigateUrl = "Change43_3.aspx?AppID=" & Session("AppID").ToString

        If Request.QueryString("AppID").Length > 0 Then
            Session("AppID") = Request.QueryString("AppID").ToString
        End If
        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text

        m_App = New Base.Kernel.Security.App(m_User)

        If Session("AppHaendler") Is Nothing Then
            Response.Redirect("Change04.aspx?AppID=" & Session("AppID").ToString)
        End If

        If (Session("objSuche") Is Nothing) Then
            Response.Redirect("Change43.aspx?AppID=" & Session("AppID").ToString)
        Else
            objSuche = CType(Session("objSuche"), CKG.Components.ComCommon.Finance.Search)
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

        objHaendler = CType(Session("AppHaendler"), fin_06)

        If Not IsPostBack Then
            InitialLoad()
        End If

    End Sub

    Private Sub InitialLoad()
        Kopfdaten1.Kontingente = objHaendler.Kontingente
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

                objHaendler.Adresse = Session("SelectedDeliveryValue").ToString
                objHaendler.AdresseHalter = Session("HalterAdresseNummer").ToString    'HEZ
                objHaendler.AdresseEmpf = Session("EmpfAdresseNummer").ToString  'HEZ

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
                        objHaendler.Anfordern(Session("AppID").ToString, Session.SessionID, True)

                        If objHaendler.Status = -1111 Then
                            blnPerformedWithoutError = False
                            lblError.Text = objHaendler.Message
                            Exit For
                        End If

                        Dim sngStart As Single = CSng(Timer)
                        Dim intStart As Int32 = 0
                        Do While CSng(Timer) < sngStart + 1
                            intStart += 1
                        Loop

                        tmpDataView.Item(intItemCounter)("VBELN") = objHaendler.Auftragsnummer
                        If objHaendler.Auftragsnummer.Length = 0 Then
                            tmpDataView.Item(intItemCounter)("COMMENT") = "Fehler: " & objHaendler.Message
                            lblError.Text = "Vorgang mit Fehlern abgeschlossen."
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
                    DataGrid1.DataSource = tmpDataView
                    DataGrid1.DataBind()
                    checkGrid()

                    Dim tblTemp As New DataTable()
                    Dim i As Integer

                    With tblTemp.Columns
                        .Add("Kundennr.", System.Type.GetType("System.String"))
                        .Add("Händlernr.", System.Type.GetType("System.String"))
                        .Add("Fahrgestellnr.", System.Type.GetType("System.String"))
                        .Add("Vertragsnr.", System.Type.GetType("System.String"))
                        .Add("Briefnr.", System.Type.GetType("System.String"))
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
                        tmpRow("Vertragsnr.") = tmpDataView(i)("LIZNR")
                        tmpRow("Briefnr.") = tmpDataView(i)("TIDNR")
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
                    Session("AppHaendler") = objHaendler    'Merken für Auftragdruck...

                    'objHaendler = New FDD_Haendler(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "", "60" & objSuche.REFERENZ, , True)
                    objHaendler.Show(Session("AppID").ToString, Session.SessionID)  'Kopfdaten aktualisieren...
                    Kopfdaten1.Kontingente = objHaendler.Kontingente

                    cmdSave.Visible = False
                    Session("SelectedDeliveryValue") = ""
                    lnkAdressAuswahl.Visible = False
                    lnkFahrzeugAuswahl.Visible = False
                Else
                    InitialLoad()
                End If
                DataGrid1.Columns(DataGrid1.Columns.Count - 2).Visible = True
                DataGrid1.Columns(DataGrid1.Columns.Count - 1).Visible = True
            End If
        Catch ex As Exception
            lblError.Text = "Fehler: " & ex.Message
            logApp.UpdateEntry("ERR", Session("AppID").ToString, "Fehler bei der Briefanforderung zu Adresse-Nr. " & objHaendler.Adresse & ", Fehler: " & ex.Message & ")")
            Throw ex
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
        Dim control As control
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


    Private Sub DataGrid1_SortCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        FillGrid(e.SortExpression)
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        DoSubmit()
        'lnkAntrag.Visible = True
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Change43_4.aspx.vb $
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 23.06.09   Time: 16:05
' Updated in $/CKAG/Components/ComCommon/Finance
' ITa 2918 Z_M_Briefanforderung_002
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 22.06.09   Time: 13:18
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2918 Z_M_Creditlimit_Detail_001
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 5.06.09    Time: 15:58
' Updated in $/CKAG/Components/ComCommon/Finance
' mögliche try catches entfernt
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 14.08.08   Time: 15:56
' Updated in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 10.04.08   Time: 17:37
' Updated in $/CKAG/Components/ComCommon/Finance
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:32
' Created in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 10:48
' Created in $/CKAG/portal/Components/ComCommon/Finance
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 13.03.08   Time: 14:41
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' RTFS Kopfdaten änderung auf Finance Kopfdaten 
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 4.01.08    Time: 16:01
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Bugfix RTFS
' 
' *****************  Version 4  *****************
' User: Uha          Date: 19.12.07   Time: 17:39
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1511 Testversion
' 
' *****************  Version 3  *****************
' User: Uha          Date: 19.12.07   Time: 14:10
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1510,1512,1511 Anforderung / Zulassung
' 
' *****************  Version 2  *****************
' User: Uha          Date: 17.12.07   Time: 17:58
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Versand ZB II / Briefe - komplierfähige Zwischenversion
' 
' *****************  Version 1  *****************
' User: Uha          Date: 13.12.07   Time: 17:18
' Created in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Elemente für Temp./Endg. bzw. HEZ Anforderung hinzugefügt (Change42ff,
' fin_06, Change43ff und fin_08)
' 
' ************************************************
