Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports Microsoft.VisualBasic

Public Class Change42_4
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
    Private objSuche As Finance.Search
    Private objHaendler As fin_06
    'Private mstrHDL As String
    Private mstrREFEFERNZ1 As String
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents Kopfdaten1 As PageElements.Kopfdaten
    Protected WithEvents lnkFahrzeugsuche As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkFahrzeugAuswahl As System.Web.UI.WebControls.HyperLink
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkAdressAuswahl As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblVersandart As System.Web.UI.WebControls.Label
    Protected WithEvents lblAddress As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblMaterialNummer As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents tblMessage As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents lblEnd As System.Web.UI.WebControls.Label
    Protected WithEvents lblTemp As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Hinweis As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Versandhinweis As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        Kopfdaten1.Message = ""
        lnkFahrzeugAuswahl.NavigateUrl = "Change42_2.aspx?AppID=" & Session("AppID").ToString
        lnkFahrzeugsuche.NavigateUrl = "Change42.aspx?AppID=" & Session("AppID").ToString
        lnkAdressAuswahl.NavigateUrl = "Change42_3.aspx?AppID=" & Session("AppID").ToString

        GetAppIDFromQueryString(Me)
        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text

        m_App = New Base.Kernel.Security.App(m_User)
        'mstrHDL = ""
        'If Request.QueryString("HDL") = 1 Then
        '    mstrHDL = "&HDL=1"
        'End If

        If Session("AppHaendler") Is Nothing Then
            Response.Redirect("Change42.aspx?AppID=" & Session("AppID").ToString)
        End If

        If (Session("objSuche") Is Nothing) Then
            Response.Redirect("Change42.aspx?AppID=" & Session("AppID").ToString)
        Else
            objSuche = CType(Session("objSuche"), Finance.Search)
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
        FillGrid()
        lblAddress.Text = Session("SelectedDeliveryText").ToString
        Dim strNullen As String = "00000000000000"
        Select Case CType(Session("Materialnummer"), String)
            Case "1391"
                lblMaterialNummer.Text = strNullen & "1391"
                lblVersandart.Text = "Standard"
            Case "1385"
                lblMaterialNummer.Text = strNullen & "1385"
                lblVersandart.Text = objHaendler.VersandArtText
                'weitere Versandarten hinzufügen! JJU2008.03.05
            Case "1389"
                lblMaterialNummer.Text = strNullen & "1389"
                lblVersandart.Text = objHaendler.VersandArtText
            Case "1390"
                lblMaterialNummer.Text = strNullen & "1390"
                lblVersandart.Text = objHaendler.VersandArtText
            Case "5530"
                lblMaterialNummer.Text = strNullen & "5530"
                lblVersandart.Text = objHaendler.VersandArtText
            Case Else
                Throw New Exception("Versandart unbekannt: " & Session("Materialnummer").ToString)
        End Select


        'Dim tmpRow As DataRow = objHaendler.Abrufgruende.Select("SapWert='" & objHaendler.Abrufgrund & "'")(0)

        'lblAbrufgrund.Text = CStr(tmpRow("WebBezeichnung"))
        'If objHaendler.AbrufgrundZusatztext.Length = 0 Then
        '    lblAbrufgrundZusatz.Text = ""
        'Else
        '    lblAbrufgrundZusatz.Text = objHaendler.AbrufgrundZusatztext
        '    If CStr(tmpRow("Zusatzbemerkung")).Length = 0 Then
        '        lblAbrufgrundZusatz.Text = "Bemerkung: " & lblAbrufgrundZusatz.Text
        '    Else
        '        lblAbrufgrundZusatz.Text = CStr(tmpRow("Zusatzbemerkung")) & ": " & lblAbrufgrundZusatz.Text
        '    End If
        'End If

        'If objHaendler.Zulassungstyp = "1" Then
        '    lblTyp.Text = "Temporär"
        'Else
        '    lblTyp.Text = "Endgültig"
        'End If
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)

        lblMessage.Text = String.Empty
        objHaendler.StandardLogID = logApp.LogStandardIdentity
        Try
            If Session("SelectedDeliveryValue").ToString.Length = 0 Then
                Response.Redirect(lnkFahrzeugAuswahl.NavigateUrl)
            Else
                Dim tmpDataView As DataView = objHaendler.Fahrzeuge.DefaultView

                tmpDataView.RowFilter = "MANDT <> '0'"

                objHaendler.Adresse = Session("SelectedDeliveryValue").ToString

                Dim intItemCounter As Int32
                Dim blnPerformedWithoutError As Boolean = True
                For intItemCounter = 0 To tmpDataView.Count - 1
                    If CStr(tmpDataView.Item(intItemCounter)("COMMENT")).Length = 0 Then
                        objHaendler.ZZFAHRG = tmpDataView.Item(intItemCounter)("ZZFAHRG").ToString
                        objHaendler.KreditkontrollBereich = tmpDataView.Item(intItemCounter)("MANDT").ToString
                        objHaendler.MaterialNummer = lblMaterialNummer.Text
                        objHaendler.KUNNR = m_User.KUNNR
                        objHaendler.Anfordern(Session("AppID").ToString, Session.SessionID)

                        If objHaendler.Status = -1111 Then
                            blnPerformedWithoutError = False
                            lblError.Text = objHaendler.Message
                            Exit For
                        End If


                        tmpDataView.Item(intItemCounter)("VBELN") = objHaendler.Auftragsnummer
                        If objHaendler.Auftragsnummer.Length = 0 Then
                            tmpDataView.Item(intItemCounter)("COMMENT") = "Fehler: " & objHaendler.Message
                            lblError.Text = "Vorgang mit Fehlern abgeschlossen."
                        ElseIf CType(Session("AppShowNot"), Boolean) = True Then
                            tmpDataView.Item(intItemCounter)("COMMENT") = objHaendler.Auftragsstatus2
                        Else
                            tmpDataView.Item(intItemCounter)("COMMENT") = objHaendler.Auftragsstatus
                        End If
                    End If
                Next

                If blnPerformedWithoutError Then
                    Dim tmpTable As DataTable
                    Dim AppURL As String = Replace(Request.Url.LocalPath, "/Portal", "..")
                    tmpTable = Session(AppURL)
                    Dim RowColname As DataRow

                    For Each RowColname In tmpTable.Rows
                        If RowColname("ControlID") = "col_Kontonummer" Then
                            mstrREFEFERNZ1 = RowColname("Content").ToString
                        End If
                    Next

                    DataGrid1.DataSource = tmpDataView
                    DataGrid1.DataBind()

                    Dim tblTemp As New DataTable()
                    Dim i As Int32

                    tblTemp.Columns.Add("Kundennr.", System.Type.GetType("System.String"))
                    tblTemp.Columns.Add("Händlernr.", System.Type.GetType("System.String"))
                    tblTemp.Columns.Add("Fahrgestellnr.", System.Type.GetType("System.String"))
                    tblTemp.Columns.Add("Vertragsnr.", System.Type.GetType("System.String"))
                    tblTemp.Columns.Add("Nummer ZB2", System.Type.GetType("System.String"))
                    tblTemp.Columns.Add("Kennzeichen", System.Type.GetType("System.String"))
                    tblTemp.Columns.Add(mstrREFEFERNZ1, System.Type.GetType("System.String"))
                    ' tblTemp.Columns.Add("Equipmentnr.", System.Type.GetType("System.String"))
                    tblTemp.Columns.Add("Bez.", System.Type.GetType("System.Boolean"))
                    tblTemp.Columns.Add("COC Besch.vorh.", System.Type.GetType("System.Boolean"))
                    tblTemp.Columns.Add("Auftragsnr.", System.Type.GetType("System.String"))
                    tblTemp.Columns.Add("Kommentar", System.Type.GetType("System.String"))
                    tblTemp.Columns.Add("Kontingentart", System.Type.GetType("System.String"))
                    Dim tmpRow As DataRow
                    For i = 0 To tmpDataView.Count - 1
                        tmpRow = tblTemp.NewRow
                        tmpRow("Kundennr.") = objHaendler.KUNNR.TrimStart("0"c)
                        tmpRow("Händlernr.") = objHaendler.Customer.TrimStart("0"c)
                        tmpRow("Fahrgestellnr.") = tmpDataView(i)("ZZFAHRG")
                        tmpRow("Vertragsnr.") = tmpDataView(i)("LIZNR")
                        tmpRow("Nummer ZB2") = tmpDataView(i)("TIDNR")
                        tmpRow("Kennzeichen") = tmpDataView(i)("LICENSE_NUM")
                        tmpRow(mstrREFEFERNZ1) = tmpDataView(i)("ZZREFERENZ1")
                        'If Not TypeOf tmpDataView(i)("EQUNR") Is System.DBNull Then
                        '    tmpRow("Equipmentnr.") = CStr(tmpDataView(i)("EQUNR")).TrimStart("0"c)
                        'End If
                        tmpRow("Bez.") = tmpDataView(i)("ZZBEZAHLT")
                        tmpRow("COC Besch.vorh.") = tmpDataView(i)("ZZCOCKZ")
                        tmpRow("Auftragsnr.") = tmpDataView(i)("VBELN")
                        tmpRow("Kommentar") = tmpDataView(i)("COMMENT")
                        Select Case tmpDataView(i)("MANDT").ToString
                            Case "1"
                                tmpRow("Kontingentart") = "Standard temporär"
                            Case "2"
                                tmpRow("Kontingentart") = "Standard endgültig"
                            Case "4"
                                tmpRow("Kontingentart") = "Erweitertes Zahlungsziel (Delayed Payment) endgültig"
                        End Select

                        tblTemp.Rows.Add(tmpRow)
                    Next
                    logApp.UpdateEntry("APP", Session("AppID").ToString, "Briefanforderung zu Adresse-Nr. " & objHaendler.Adresse & " erfolgreich durchgeführt.", tblTemp)

                    objHaendler = New fin_06(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "", objSuche.REFERENZ, m_User.KUNNR)

                    objHaendler.Show(Session("AppID").ToString, Session.SessionID)

                    Kopfdaten1.Kontingente = objHaendler.Kontingente

                    Session("AppHaendler") = objHaendler

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

    'Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    '    DoSubmit()
    'End Sub

    Private Sub FillGrid(Optional ByVal strSort As String = "")
        Dim tmpDataView As DataView = objHaendler.Fahrzeuge.DefaultView

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

        tmpDataView.RowFilter = "MANDT <> '0'"
        DataGrid1.DataSource = tmpDataView
        DataGrid1.DataBind()

        Dim item As DataGridItem
        Dim cell As TableCell
        Dim chkBox As CheckBox
        'Dim lbl As Label
        Dim control As control
        'Dim blnScriptFound As Boolean = False
        Dim intZaehl0001 As Int32 = 0
        Dim intZaehl0002 As Int32 = 0
        Dim blnBezahlt As Boolean
        Dim strKkb As String
        Dim blnGesperrteAnforderungen As Boolean = False
        'col_Vertragsnummer
        For Each item In DataGrid1.Items
            blnBezahlt = False
            strKkb = item.Cells(1).Text

            For Each cell In item.Cells
                For Each control In cell.Controls
                    Dim checkBox = TryCast(control, CheckBox)
                    If (checkBox IsNot Nothing) Then
                        chkBox = checkBox
                        If chkBox.ID = "Bezahlt" And chkBox.Checked Then
                            blnBezahlt = True
                        End If
                    End If
                Next
                For Each control In cell.Controls
                    Dim checkBox = TryCast(control, CheckBox)
                    If (checkBox IsNot Nothing) Then
                        chkBox = checkBox
                        If Not chkBox.ID = "Bezahlt" Then
                            Select Case chkBox.ID
                                Case "chk0001"
                                    If strKkb = "1" Then
                                        chkBox.Checked = True
                                        If Not blnBezahlt Then
                                            intZaehl0001 += 1
                                        End If
                                    Else
                                        chkBox.Checked = False
                                    End If
                                Case "chk0002"
                                    If strKkb = "2" Then
                                        chkBox.Checked = True
                                        If Not blnBezahlt Then
                                            intZaehl0002 += 1
                                        End If
                                    Else
                                        chkBox.Checked = False
                                    End If
                            End Select
                        End If
                    End If
                Next
            Next
        Next


        lblMessage.Text = ""
        Dim rowTest As DataRow
        Dim intTemp As Int32
        lblTemp.Text = 0
        lblEnd.Text = 0

        For Each rowTest In objHaendler.Kontingente.Rows

            Select Case rowTest("Kreditkontrollbereich").ToString
                Case "0001"
                    intTemp = CInt(rowTest("Ausschoepfung")) + intZaehl0001 - CInt(rowTest("Kontingent_Alt"))
                    If intTemp > 0 And intZaehl0001 > 0 Then
                        lblTemp.Text = intTemp.ToString
                        blnGesperrteAnforderungen = True
                    End If
                Case "0002"
                    intTemp = CInt(rowTest("Ausschoepfung")) + intZaehl0002 - CInt(rowTest("Kontingent_Alt"))
                    If intTemp > 0 And intZaehl0002 > 0 Then
                        lblEnd.Text = intTemp.ToString
                        blnGesperrteAnforderungen = True
                    End If
            End Select
            tblMessage.Visible = blnGesperrteAnforderungen
        Next
        lblMessage.Text = lblMessage.Text & "<br>Zur Aktivierung der ausgewählten Vorgänge klicken Sie auf [Absenden].<br>"
        If blnGesperrteAnforderungen Then
            lbl_Versandhinweis.Visible = True
        Else
            lbl_Versandhinweis.Visible = False
        End If
        DataGrid1.PagerStyle.Visible = False
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(e.SortExpression)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Change42_4.aspx.vb $
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 23.06.09   Time: 16:05
' Updated in $/CKAG/Components/ComCommon/Finance
' ITa 2918 Z_M_Briefanforderung_002
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 22.06.09   Time: 13:18
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2918 Z_M_Creditlimit_Detail_001
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 5.06.09    Time: 15:58
' Updated in $/CKAG/Components/ComCommon/Finance
' mögliche try catches entfernt
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 13.11.08   Time: 13:53
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2379 fertig
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 14.08.08   Time: 15:56
' Updated in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 25.07.08   Time: 8:18
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2125 done
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
' User: Fassbenders  Date: 4.04.08    Time: 10:47
' Created in $/CKAG/portal/Components/ComCommon/Finance
' 
' *****************  Version 15  *****************
' User: Jungj        Date: 5.03.08    Time: 12:02
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' akf änderungen 1733
' 
' *****************  Version 14  *****************
' User: Jungj        Date: 5.03.08    Time: 9:13
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' AKF Änderungen ITA 1733
' 
' *****************  Version 13  *****************
' User: Jungj        Date: 19.02.08   Time: 15:52
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' akfänderungen
' 
' *****************  Version 12  *****************
' User: Jungj        Date: 18.02.08   Time: 13:47
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' akf änderungen
' 
' *****************  Version 11  *****************
' User: Rudolpho     Date: 12.02.08   Time: 15:29
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA:1677
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 1.02.08    Time: 16:01
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 31.01.08   Time: 16:44
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Bugfix AKF
' 
' *****************  Version 8  *****************
' User: Uha          Date: 8.01.08    Time: 16:07
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' 
' *****************  Version 7  *****************
' User: Uha          Date: 7.01.08    Time: 18:47
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 4.01.08    Time: 16:01
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Bugfix RTFS
' 
' *****************  Version 5  *****************
' User: Uha          Date: 19.12.07   Time: 14:10
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1510,1512,1511 Anforderung / Zulassung
' 
' *****************  Version 4  *****************
' User: Uha          Date: 18.12.07   Time: 17:57
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Anforderung (temp./endg.) fast fertig
' 
' *****************  Version 3  *****************
' User: Uha          Date: 18.12.07   Time: 14:20
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
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
