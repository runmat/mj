Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Change03_2
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
    Private objZulassung As Arval_1
    'Private objSuche As Search

    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents txtHalterStrasse As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtLeasingnummerErfassung As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.LinkButton
    Protected WithEvents tblHalterEdit As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblLegende As System.Web.UI.WebControls.Label
    Protected WithEvents txtFahrgestellnr As System.Web.UI.WebControls.TextBox
    Protected WithEvents DropDownList1 As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents CheckBox1 As System.Web.UI.WebControls.CheckBox
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Server.ScriptTimeout = 180  '3 Minuten Timeout (nur für diese Seite!)
            m_User = GetUser(Me)
            ucHeader.InitUser(m_User)
            FormAuth(Me, m_User)

            GetAppIDFromQueryString(Me)

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If (Session("objSucheZulassung") Is Nothing) AndAlso (Not IsPostBack) Then
                objZulassung = New Arval_1(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
            Else
                objZulassung = CType(Session("objZulassung"), Arval_1)
            End If

            If Not IsPostBack Then
                ddlPageSize.Items.Add("10")
                ddlPageSize.Items.Add("20")
                ddlPageSize.Items.Add("50")
                ddlPageSize.Items.Add("100")
                ddlPageSize.Items.Add("250")
                ddlPageSize.SelectedIndex = 2

                Session("objZulassung") = objZulassung

            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")),
                         m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString,
                         m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)

        Session.Add("objLog", logApp)

        If CheckGrid() Then
            Dim tmpDataView As DataView = objZulassung.Result.DefaultView

            tmpDataView.RowFilter = "Ausgewaehlt = 1"
            Dim intFahrzeugBriefe As Int32 = tmpDataView.Count
            tmpDataView.RowFilter = ""

            If intFahrzeugBriefe = 0 Then
                FillGrid(DataGrid1.CurrentPageIndex)
            Else
                Session("objZulassung") = objZulassung
                Response.Redirect("Change03_4.aspx?AppID=" & Session("AppID").ToString)
            End If
            Response.Redirect("Change03_4.aspx?AppID=" & Session("AppID").ToString)
        End If
    End Sub

    'Private Sub cmdContinue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Response.Redirect("Change03_3.aspx?AppID=" & CStr(Session("AppID")))
    'End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        Dim row As DataRow
        Dim resultTable As New DataTable()
        Dim strUnterlagen As String = ""
        Dim leasingnr As String
        lblNoData.Text = ""
        leasingnr = txtLeasingnummerErfassung.Text

        If Not IsNumeric(leasingnr) Then
            lblError.Text = "Ungültige Leasingvertragsnummer."
            Exit Sub
        End If

        objZulassung.ArvalZulassung = CheckBox1.Checked
        objZulassung.SucheLvNr = txtLeasingnummerErfassung.Text
        objZulassung.SucheFgstNr = txtFahrgestellnr.Text
        objZulassung.Haendlernummer = m_User.Reference  'Nur die Eigenen Händler anzeigen!
        objZulassung.ClearResultTable()

        objZulassung.Show()

        If (objZulassung.Result.Rows.Count = 0) Then
            lblError.Text = objZulassung.Message
            Exit Sub
        End If

        Dim tmpHalterName1 As String = "initial"
        Dim tmpHalterPlz As String = "initial"

        For Each row In objZulassung.Result.Rows
            If Not ((row("HalterName1").ToString = tmpHalterName1) And (row("HalterPlz").ToString = tmpHalterPlz)) Then
                strUnterlagen = String.Empty
                resultTable.Clear()
                objZulassung.checkZulassungsdokumente(Session("AppID").ToString, Session.SessionID.ToString, resultTable,
                                                      row("HalterName1").ToString, row("HalterPlz").ToString)
                If resultTable.Rows.Count = 0 Then
                    'Kein Datensatz in der Zulassungsdokumententabelle gefunden = Alles fehlt!!
                    strUnterlagen = "VHPGE"
                    lblLegende.Visible = True
                End If

                If resultTable.Rows.Count > 1 Then
                    strUnterlagen = ""
                End If

                If resultTable.Rows.Count = 1 Then
                    If resultTable.Rows(0)("VOLLM").ToString = "" Then  'Vollmacht
                        strUnterlagen &= "V"
                        lblLegende.Visible = True
                    End If

                    If resultTable.Rows(0)("REGISTER").ToString = "" Then  'Handelsegistereintrag
                        strUnterlagen &= "H"
                        lblLegende.Visible = True
                    End If

                    If resultTable.Rows(0)("PERSO").ToString = "" Then  'Personalausweis
                        strUnterlagen &= "P"
                        lblLegende.Visible = True
                    End If

                    If resultTable.Rows(0)("GEWERBE").ToString = "" Then  'Gewerbeanmeldung
                        strUnterlagen &= "G"
                        lblLegende.Visible = True
                    End If

                    If resultTable.Rows(0)("EINZUG").ToString = "" Then  'Einzugsermächtigung
                        strUnterlagen &= "E"
                        lblLegende.Visible = True
                    End If

                    row("FUnterlagen") = strUnterlagen

                    If row("Evbnummer").ToString.Length = 0 Then
                        row("Evbnummer") = resultTable.Rows(0)("EVB_NUM").ToString
                    End If

                    row("EVB_VON") = resultTable.Rows(0)("EVB_VON").ToString
                    row("EVB_BIS") = resultTable.Rows(0)("EVB_BIS").ToString

                End If

                tmpHalterName1 = row("HalterName1").ToString
                tmpHalterPlz = row("HalterPlz").ToString
            End If

            row("FUnterlagen") = strUnterlagen
        Next

        If Not objZulassung.Message.Length = 0 Then
            If (objZulassung.Message <> "ERR_ZULDOKU") Then
                lblError.Text = objZulassung.Message
                lblNoData.Text = String.Empty
            Else
                Session("objZulassung") = objZulassung
                FillGrid(0)
            End If
        Else
            Session("objZulassung") = objZulassung
            FillGrid(0)
        End If
    End Sub

    'Private Sub cmdNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    txtLeasingnummerErfassung.Text = ""
    'End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        Dim tmpDataView As DataView
        tmpDataView = objZulassung.Result.DefaultView
        tmpDataView.RowFilter = ""

        If tmpDataView.Count = 0 Then
            DataGrid1.Visible = False
            ddlPageSize.Visible = False
            lblNoData.Text = "Keine Daten zur Anzeige gefunden."
            lblNoData.Visible = True
        Else
            ddlPageSize.Visible = True
            DataGrid1.Visible = True
            Dim intTempPageIndex As Int32 = intPageIndex
            Dim strTempSort As String = ""
            Dim strDirection As String = ""

            If strSort.Trim(" "c).Length > 0 Then
                intTempPageIndex = 0
                strTempSort = strSort.Trim(" "c)
                If (ViewState("Sort") Is Nothing) OrElse (ViewState("Sort").ToString = strTempSort) Then
                    If ViewState("Direction") Is Nothing Then
                        strDirection = "desc"
                    Else
                        strDirection = ViewState("Direction").ToString
                    End If
                Else
                    strDirection = "desc"
                End If

                If strDirection = "asc" Then
                    strDirection = "desc"
                Else
                    strDirection = "asc"
                End If

                ViewState("Sort") = strTempSort
                ViewState("Direction") = strDirection
            Else
                If Not ViewState("Sort") Is Nothing Then
                    strTempSort = ViewState("Sort").ToString
                    If ViewState("Direction") Is Nothing Then
                        strDirection = "asc"
                        ViewState("Direction") = strDirection
                    Else
                        strDirection = ViewState("Direction").ToString
                    End If
                End If
            End If

            If Not strTempSort.Length = 0 Then
                tmpDataView.Sort = strTempSort & " " & strDirection
            End If

            DataGrid1.CurrentPageIndex = intTempPageIndex

            DataGrid1.DataSource = tmpDataView
            DataGrid1.DataBind()

            If lblNoData.Text.Length = 0 Then
                lblNoData.Text = "Es wurde(n) " & tmpDataView.Count.ToString & " Fahrzeug(e) gefunden."
                lblNoData.Visible = True
            End If

            If DataGrid1.PageCount > 1 Then
                DataGrid1.PagerStyle.CssClass = "PagerStyle"
                DataGrid1.DataBind()
                DataGrid1.PagerStyle.Visible = True
            Else
                DataGrid1.PagerStyle.Visible = False
            End If
        End If
    End Sub

    Private Function CheckGrid(Optional ByVal toSort As Boolean = False) As Boolean
        Dim item As DataGridItem
        Dim cell As TableCell
        Dim checkBox As CheckBox
        Dim textbox As TextBox
        Dim label As Label
        Dim control As Control
        Dim intZaehl As Int32 = 0
        Dim intReturn As Boolean

        intReturn = True

        lblNoData.Text = ""

        For Each item In DataGrid1.Items
            Dim strWuk01_Buchstaben As String = ""
            Dim strWuk01_Ziffern As String = ""
            Dim strWuk02_Buchstaben As String = ""
            Dim strWuk02_Ziffern As String = ""
            Dim strWuk03_Buchstaben As String = ""
            Dim strWuk03_Ziffern As String = ""
            Dim strReservierungsdaten As String = ""
            Dim strZulDatum As String = ""
            Dim strEvbnr As String = ""
            Dim strEvbvon As String = ""
            Dim strEvbBis As String = ""
            Dim blnAuswahl As Boolean
            Dim blnVorreservierung As Boolean
            Dim blnKennzeichenserie As Boolean
            Dim strFhgstNr As String = ""
            Dim afnam As String
            Dim dat As Date

            For Each cell In item.Cells
                For Each control In cell.Controls
                    Dim box = TryCast(control, CheckBox)
                    If (box IsNot Nothing) Then
                        checkBox = box
                        If checkBox.ID = "Vorreserviert" Then
                            blnVorreservierung = checkBox.Checked
                        End If
                        If checkBox.ID = "Kennzeichenserie" Then
                            blnKennzeichenserie = checkBox.Checked
                        End If
                        If checkBox.ID = "Auswahl" Then
                            blnAuswahl = checkBox.Checked
                            If blnAuswahl = True Then
                                intZaehl += 1
                            End If
                        End If
                    End If
                Next

                For Each control In cell.Controls
                    Dim box = TryCast(control, TextBox)
                    If (box IsNot Nothing) Then
                        textbox = box
                        If textbox.ID = "Reservierungsdaten" Then
                            strReservierungsdaten = textbox.Text
                        End If
                        If textbox.ID = "Wuk01_Buchstaben" Then
                            strWuk01_Buchstaben = textbox.Text
                        End If
                        If textbox.ID = "Wuk01_Ziffern" Then
                            strWuk01_Ziffern = textbox.Text
                        End If
                        If textbox.ID = "Wuk02_Buchstaben" Then
                            strWuk02_Buchstaben = textbox.Text
                        End If
                        If textbox.ID = "Wuk02_Ziffern" Then
                            strWuk02_Ziffern = textbox.Text
                        End If
                        If textbox.ID = "Wuk03_Buchstaben" Then
                            strWuk03_Buchstaben = textbox.Text
                        End If
                        If textbox.ID = "Wuk03_Ziffern" Then
                            strWuk03_Ziffern = textbox.Text
                        End If
                        If textbox.ID = "DatumZulassung" Then
                            strZulDatum = textbox.Text
                        End If
                        If textbox.ID = "Evbnummer" Then
                            strEvbnr = textbox.Text
                        End If
                        If textbox.ID = "Evb_Von" Then
                            strEvbvon = textbox.Text
                        End If
                        If textbox.ID = "Evb_Bis" Then
                            strEvbBis = textbox.Text
                        End If
                    End If
                Next

                For Each control In cell.Controls
                    Dim control1 = TryCast(control, Label)
                    If (control1 IsNot Nothing) Then
                        label = control1
                        If label.ID = "lblLvNr" Then
                            strFhgstNr = label.Text
                        End If
                    End If
                Next
            Next

            Dim strZZFAHRG As String = "LeasingNummer = '" & strFhgstNr & "'"

            objZulassung.Result.AcceptChanges()
            Dim tmpRows As DataRow()
            tmpRows = objZulassung.Result.Select(strZZFAHRG)
            tmpRows(0).BeginEdit()

            tmpRows(0).Item("Wuk01_Buchstaben") = strWuk01_Buchstaben
            tmpRows(0).Item("Wuk01_Ziffern") = strWuk01_Ziffern
            tmpRows(0).Item("Wuk02_Buchstaben") = strWuk02_Buchstaben
            tmpRows(0).Item("Wuk02_Ziffern") = strWuk02_Ziffern
            tmpRows(0).Item("Wuk03_Buchstaben") = strWuk03_Buchstaben
            tmpRows(0).Item("Wuk03_Ziffern") = strWuk03_Ziffern
            tmpRows(0).Item("Vorreserviert") = blnVorreservierung
            tmpRows(0).Item("Reservierungsdaten") = strReservierungsdaten
            tmpRows(0).Item("Kennzeichenserie") = blnKennzeichenserie
            tmpRows(0).Item("Ausgewaehlt") = blnAuswahl

            tmpRows(0).Item("Evbnummer") = strEvbnr
            tmpRows(0).Item("Evb_von") = strEvbvon
            tmpRows(0).Item("Evb_bis") = strEvbBis

            tmpRows(0).Item("DatumZulassung") = strZulDatum

            tmpRows(0).EndEdit()
            objZulassung.Result.AcceptChanges()

            Dim cArray() As Char = {"1"c, "2"c, "3"c, "4"c, "5"c, "6"c, "7"c, "8"c, "9"c}

            If blnAuswahl Then
                If blnVorreservierung And strReservierungsdaten.Length = 0 Then
                    lblNoData.Text &= "<br>Bitte geben Sie die Daten für Fahrzeug """ & strFhgstNr & """ vollständig ein: Vorreservierung"
                    intReturn = False
                End If
                If blnVorreservierung And (strWuk01_Buchstaben.Length = 0 Or strWuk01_Ziffern.Length = 0) Then
                    lblNoData.Text &= "<br>Bitte geben Sie die Daten für Fahrzeug """ & strFhgstNr & """ vollständig ein: Wunschekennz. 1"
                    intReturn = False
                End If
                If (strWuk01_Ziffern <> String.Empty) And Not IsNumeric(strWuk01_Ziffern) Then
                    lblNoData.Text &= "<br>Die Wunschkennzeichen-Nummern für Fahrzeug """ & strFhgstNr & """ dürfen keine Buchstaben enthalten."
                    intReturn = False
                End If
                If (strWuk02_Ziffern <> String.Empty) And Not IsNumeric(strWuk02_Ziffern) Then
                    lblNoData.Text &= "<br>Die Wunschkennzeichen-Nummern für Fahrzeug """ & strFhgstNr & """ dürfen keine Buchstaben enthalten."
                    intReturn = False
                End If
                If (strWuk01_Buchstaben <> String.Empty And (strWuk01_Buchstaben.IndexOfAny(cArray) >= 0)) Then
                    lblNoData.Text &= "<br>Die Wunschkennzeichen-Ortskennzeichen für Fahrzeug """ & strFhgstNr & """ dürfen keine Zahlen enthalten."
                    intReturn = False
                End If
                If (strWuk02_Buchstaben <> String.Empty And (strWuk02_Buchstaben.IndexOfAny(cArray) >= 0)) Then
                    lblNoData.Text &= "<br>Die Wunschkennzeichen-Ortskennzeichen für Fahrzeug """ & strFhgstNr & """ dürfen keine Zahlen enthalten."
                    intReturn = False
                End If
                If (strWuk01_Ziffern <> String.Empty And strWuk01_Ziffern.Trim.IndexOf("0") = 0) Then
                    lblNoData.Text &= "<br>Die Wunschkennzeichen-Nummer für Fahrzeug """ & strFhgstNr & """ darf keine führenden Nullen enthalten."
                    intReturn = False
                End If
                If (strWuk02_Ziffern <> String.Empty And strWuk02_Ziffern.Trim.IndexOf("0") = 0) Then
                    lblNoData.Text &= "<br>Die Wunschkennzeichen-Nummer für Fahrzeug """ & strFhgstNr & """ darf keine führenden Nullen enthalten."
                    intReturn = False
                End If
            End If

            Try
                'Prüfen, ob eingegebenes Datum < Vorschlagsdatum! (obige Regel nun sinnlos, da sich die Funktion "SuggestionDay" geändert hat...)
                dat = CType(strZulDatum, Date)
                afnam = tmpRows(0)("Zulassungsstelle").ToString

                If (dat < objZulassung.SuggestionDay(strFhgstNr, afnam)) Then
                    lblNoData.Text &= "<br>Ungültiges Zulassungsdatum '" & strZulDatum & "'. Frühestens am " & objZulassung.SuggestionDay(strFhgstNr, afnam).ToShortDateString & "."
                    intReturn = False
                End If

                If (dat.DayOfWeek = DayOfWeek.Saturday Or dat.DayOfWeek = DayOfWeek.Sunday) Then
                    lblNoData.Text &= "<br>Ungültiges Zulassungsdatum (Wochenende)'" & strZulDatum & "'. "
                    intReturn = False
                End If
            Catch ex As Exception
                lblNoData.Text &= "<br>Ungültiges Zulassungsdatum '" & strZulDatum & "'"
                intReturn = False
            End Try
        Next

        If intReturn Then
            If (intZaehl = 0) And (toSort = False) Then
                lblError.Text = "Keine Fahrzeuge ausgewählt."
                intReturn = False
            End If
        End If

        If intReturn Then
            Session("objZulassung") = objZulassung
        End If

        Return intReturn
    End Function

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(0)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        CheckGrid(True)
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        CheckGrid(True)
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

End Class

' ************************************************
' $History: Change03_2.aspx.vb $
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 30.06.09   Time: 13:03
' Updated in $/CKAG/Applications/apparval/Forms
' ITA: 2918
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 25.08.08   Time: 17:11
' Updated in $/CKAG/Applications/apparval/Forms
' ITA: 1859
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 25.08.08   Time: 15:13
' Updated in $/CKAG/Applications/apparval/Forms
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 30.04.08   Time: 19:53
' Updated in $/CKAG/Applications/apparval/Forms
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 16.04.08   Time: 11:53
' Updated in $/CKAG/Applications/apparval/Forms
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 16:18
' Created in $/CKAG/Applications/apparval/Forms
' 
' *****************  Version 8  *****************
' User: Uha          Date: 2.07.07    Time: 10:05
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 7  *****************
' User: Uha          Date: 6.03.07    Time: 15:30
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Forms
' GetAppIDFromQueryString(Me) hinzugefügt
' 
' *****************  Version 6  *****************
' User: Uha          Date: 6.03.07    Time: 14:48
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Forms
' 
' ************************************************
