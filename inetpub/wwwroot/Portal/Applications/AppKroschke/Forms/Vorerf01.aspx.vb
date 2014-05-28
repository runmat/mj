Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common


Public Class Vorerf01
    Inherits System.Web.UI.Page

    Protected WithEvents ucStyles As Styles
    Protected WithEvents ucHeader As Header
    Protected WithEvents lnkKreditlimit As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblScript As System.Web.UI.WebControls.Label
    Protected WithEvents txtOrtsKzOld As System.Web.UI.HtmlControls.HtmlInputHidden
    Private ve As String = "Vorerfassung"
    Private zulDaten As clsVorerf01
    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private table As DataTable
    Private showCheckbox As Boolean
    Private appl As String
    Private vkorg As String
    Private vkbur As String
    Private kunnr As String
    Private qry_id As String
    Private stva As String
    Private zulDatumFilter As String
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents txtFree2 As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents btnListe As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblFilter As System.Web.UI.WebControls.Label
    Protected WithEvents lblDatensatz As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents lblStamm As System.Web.UI.WebControls.Label
    Protected WithEvents txtDienstleistungsnr As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtURL As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtKundenname As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtSTVA As System.Web.UI.WebControls.TextBox
    Protected WithEvents ddlUrl As System.Web.UI.WebControls.DropDownList
    Protected WithEvents id_Record As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtKundennummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents ddlKunnr As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtHalter As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtFahrgestellNr As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtTour As System.Web.UI.WebControls.TextBox
    Protected WithEvents cbxShowColumn As System.Web.UI.WebControls.CheckBox
    Protected WithEvents txtStVASel As System.Web.UI.WebControls.TextBox
    Protected WithEvents ddlSTVA As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LinkButton2 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents hypUrl As System.Web.UI.WebControls.HyperLink
    Protected WithEvents txtZulassungsdatum As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtOrtsKz As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtWunschkennzeichenAbc As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtWunschkennzeichenNr As System.Web.UI.WebControls.TextBox
    Protected WithEvents ddlKzTyp As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cbxWunschkennzFlag As System.Web.UI.WebControls.CheckBox
    Protected WithEvents cbxEinKennz As System.Web.UI.WebControls.CheckBox
    Protected WithEvents cbxReserviert As System.Web.UI.WebControls.CheckBox
    Protected WithEvents txtReservNr As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtInterneBemerkung As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdNew As System.Web.UI.WebControls.LinkButton
    Protected WithEvents btnAdresse As System.Web.UI.WebControls.LinkButton
    Protected WithEvents btnBearb As System.Web.UI.WebControls.LinkButton
    Protected WithEvents Table10 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Zeile0 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents Zeile1 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents Zeile2 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents Zeile3 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents Zeile4 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents Zeile5 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents Zeile8 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents Zeile6 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents Zeile7 As System.Web.UI.HtmlControls.HtmlTableRow
    Private ne As String = "Nacherfassung"
    Private status As String
    Protected WithEvents ddlDienst As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cbxSave As System.Web.UI.WebControls.CheckBox
    Protected WithEvents cbxListe As System.Web.UI.WebControls.CheckBox
    Protected WithEvents txtName1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtStrasse As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtPLZ As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtOrt As System.Web.UI.WebControls.TextBox
    Protected WithEvents tblAdresse As System.Web.UI.HtmlControls.HtmlTable

    Private id_Anzeige As String
    Dim back As String
    Protected WithEvents txtSAPID As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents txtDienstHidden As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents ddlKundeBakX As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtBarcode As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnBarcode As System.Web.UI.WebControls.LinkButton
    Protected WithEvents txtDummy As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents Table5 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents trAdresse As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trButtons As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents TRKundeInfo As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents txtPreisSTVA As System.Web.UI.WebControls.TextBox
    Protected WithEvents Textbox3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Table22 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblKundeInfo As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents txtKIReferenz As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtNotiz As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtVKkurz As System.Web.UI.WebControls.TextBox
    Protected WithEvents chk_krad As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chk_Feinstaub As System.Web.UI.WebControls.CheckBox
    Protected WithEvents txtWunschkennzeichen As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtVerVbeln As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAuart As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtSQLId As System.Web.UI.WebControls.Label

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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Authentifizierung
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        'Seitenparameter holen / initialisieren
        appl = ve
        id_Anzeige = Request.QueryString.Item("id")                 'Datensatz anzeigen?
        back = Request.QueryString.Item("B")                        'Rücksprung von Liste?
        zulDatumFilter = Request.QueryString("Zuldatum")            'Filter nach Zulassungsdatum?

        'Header initialisieren
        lblPageTitle.Text = appl.ToUpper & " der Zulassungsdaten"
        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text

        vkorg = Left(m_User.Reference, 4)
        vkbur = Right(m_User.Reference, 4)

        Try
            m_App = New Base.Kernel.Security.App(m_User)

            If Not IsPostBack Then

                If (back Is Nothing) Then
                    zulDaten = New clsVorerf01(m_User, True)
                    zulDaten.VKOrg = vkorg
                    zulDaten.VKBur = vkbur
                    Session.Add("ZulDaten", zulDaten)
                    loadForm()
                Else
                    zulDaten = CType(Session("ZulDaten"), clsVorerf01)
                    table = zulDaten.loadRecordset(id_Anzeige, status)
                    If (status <> String.Empty) Then
                        lblError.Text = "Fehler beim Laden des Datensatzes."
                        Exit Sub
                    End If
                    showData(table)
                End If
            Else
                fillDropdownlist()
                zulDaten = CType(Session("ZulDaten"), clsVorerf01)
                showCheckbox = cbxShowColumn.Checked
                ddlKzTyp.SelectedIndex = 0
            End If

            'Skripte setzen
            ddlSTVA.Attributes.Add("onChange", "SetOrtsKz();")
            ddlDienst.Attributes.Add("onChange", "SetDienst();")
            ddlKunnr.Attributes.Add("onChange", "SetKnr();")
            txtStVASel.Attributes.Add("onKeyup", "SetStVA();")
            txtKundennummer.Attributes.Add("onKeyup", "SetKunnr();")
            lblScript.Text = "<script language=""JavaScript"">window.document.Form1.ddlKunnr.focus();</script>"
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub loadForm()
        'Hier die bereits in SAP vorbelegten Daten laden: Dropdown-Listen Kunde und Dienstleistung
        Dim tblKundenBak As New DataTable()
        Dim message As String = ""
        Dim c() As Char = {"1"c, "2"c, "3"c, "4"c, "5"c, "6"c, "7"c, "8"c, "9"c}

        zulDaten = CType(Session("ZulDaten"), clsVorerf01)
        zulDaten.getSAPDatenBiz(vkorg, vkbur, message)

        If (message <> String.Empty) Then
            'Fehler aufgetreten
            lblError.Text = message
            Table10.Visible = False
            btnListe.Visible = False
            cmdNew.Visible = False
            tblAdresse.Visible = False
            Table5.Visible = False
            trButtons.Visible = False
        Else
            fillForm()
        End If
       
        Session("ZulDaten") = zulDaten
    End Sub

    Private Sub fillDropdownlist()

        Dim zulDaten As clsVorerf01
        Dim dv As DataView

        zulDaten = CType(Session("ZulDaten"), clsVorerf01)

        dv = zulDaten.Materialtabelle.DefaultView
        dv.Sort = "MAKTX"
        With ddlDienst
            .DataSource = dv
            .DataTextField = "MAKTX"
            .DataValueField = "MATNR"
            .DataBind()
        End With
        If (txtDienstHidden.Value = String.Empty) Then
            ddlDienst.SelectedIndex = 0
        Else
            ddlDienst.Items.FindByValue(txtDienstHidden.Value).Selected = True
        End If

        'STVA-Liste füllen           
        dv = zulDaten.Stvatabelle.DefaultView
        dv.Sort = "KREISBEZ"
        With ddlSTVA
            .DataSource = dv
            .DataTextField = "KREISBEZ"
            .DataValueField = "KREISKZ"
            .DataBind()
        End With

        'StVA URL füllen
        With ddlUrl
            .DataSource = dv
            .DataTextField = "URL"
            .DataValueField = "KREISKZ"
            .DataBind()
        End With
        If (txtStVASel.Text = String.Empty) Then
            ddlSTVA.SelectedIndex = 0
            ddlUrl.SelectedIndex = 0
        Else
            ddlSTVA.Items.FindByValue(txtStVASel.Text).Selected = True
            ddlUrl.Items.FindByValue(txtStVASel.Text).Selected = True
        End If
        'Kundendaten füllen
        dv = zulDaten.Kundentabelle.DefaultView
        dv.Sort = "NAME1"

        With ddlKunnr
            .DataSource = dv
            .DataTextField = "NAME1"
            .DataValueField = "KUNNR"
            .DataBind()
        End With
        If (txtKundennummer.Text = String.Empty) Then
            ddlKunnr.SelectedIndex = 0
        Else
            If txtDummy.Value.TrimStart("0"c) = String.Empty Then
                Dim ItemList As ListItem = ddlKunnr.Items.FindByValue(Right("0000000000" & txtKundennummer.Text, 10))
                If Not ItemList Is Nothing Then
                    ddlKunnr.Items.FindByValue(Right("0000000000" & txtKundennummer.Text, 10)).Selected = True
                Else
                    ddlKunnr.Items(0).Selected = True
                End If
            Else
                ddlKunnr.Items(txtDummy.Value).Selected = True
            End If
        End If
    End Sub

    Private Sub fillForm()
        Dim tblKundenBak As New DataTable()
        Dim item As ListItem
        Dim message As String = ""
        Dim dv As DataView
        Dim row As DataRow
        Dim rows As DataRow()
        Dim c() As Char = {"1"c, "2"c, "3"c, "4"c, "5"c, "6"c, "7"c, "8"c, "9"c}
        Dim zulDaten As clsVorerf01

        zulDaten = CType(Session("ZulDaten"), clsVorerf01)

        If (message <> String.Empty) Then
            'Fehler aufgetreten
            lblError.Text = message
        Else
            zulDaten.Kundentabelle.AcceptChanges()

            dv = zulDaten.Kundentabelle.DefaultView
            dv.Sort = "NAME1"

            With ddlKunnr
                .DataSource = dv
                .DataTextField = "NAME1"
                .DataValueField = "KUNNR"
                .DataBind()
            End With

            ddlKunnr.Items(0).Selected = True
            txtKundennummer.Text = CType(ddlKunnr.SelectedItem.Value, String).TrimStart("0"c)

            'Materialliste Füllen
            rows = zulDaten.Materialtabelle.Select("MAKTX = '- Keine Auswahl -'")

            If (rows.Length = 0) Then
                row = zulDaten.Materialtabelle.NewRow()
                row("MAKTX") = "- Keine Auswahl -"
                row("MATNR") = String.Empty
                zulDaten.Materialtabelle.Rows.Add(row)
                zulDaten.Materialtabelle.AcceptChanges()
            End If

            dv = zulDaten.Materialtabelle.DefaultView
            dv.Sort = "MAKTX"
            With ddlDienst
                .DataSource = dv
                .DataTextField = "MAKTX"
                .DataValueField = "MATNR"
                .DataBind()
            End With

            'STVA-Liste füllen           
            dv = zulDaten.Stvatabelle.DefaultView
            dv.Sort = "KREISBEZ"
            With ddlSTVA
                .DataSource = dv
                .DataTextField = "KREISBEZ"
                .DataValueField = "KREISKZ"
                .DataBind()
            End With

            'StVA URL füllen
            With ddlUrl
                .DataSource = dv
                .DataTextField = "URL"
                .DataValueField = "KREISKZ"
                .DataBind()
            End With
            txtOrtsKz.Text = ddlSTVA.SelectedItem.Value.ToString.ToUpper
            txtOrtsKzOld.Value = ddlSTVA.SelectedItem.Value.ToString.ToUpper

            'KennzeichenTyp füllen
            item = New ListItem()
            item.Text = "E - Euro"
            item.Value = "E"
            ddlKzTyp.Items.Add(item)

            item = New ListItem()
            item.Text = "F - Fun"
            item.Value = "F"
            ddlKzTyp.Items.Add(item)

            item = New ListItem()
            item.Text = "H - Historisch"
            item.Value = "H"
            ddlKzTyp.Items.Add(item)

            item = New ListItem()
            item.Text = "K - Kurzzeit"
            item.Value = "K"
            ddlKzTyp.Items.Add(item)

            item = New ListItem()
            item.Text = "S - Saison"
            item.Value = "S"
            ddlKzTyp.Items.Add(item)

            item = New ListItem()
            item.Text = "Z - Zoll"
            item.Value = "Z"
            ddlKzTyp.Items.Add(item)
        End If
    End Sub

    Private Sub save(ByVal update As Boolean)
        Dim status As String = ""
        Dim temp As String = ""
        Dim returnID As String = ""
        Dim str_Message As String = ""

        zulDaten = CType(Session("ZulDaten"), clsVorerf01)

        'Neue SAP-Id holen...
        If Not update Then
            txtSAPID.Text = CStr(zulDaten.GiveNewZulassungsID(str_Message))
            If str_Message.Trim.Length > 0 Then
                lblError.Text = str_Message
                Exit Sub
            End If
        End If

        'Daten sammeln und speichern...
        With zulDaten

            If Not (txtPreisSTVA.Text = String.Empty) Then
                .preis_stva = txtPreisSTVA.Text
            Else
                .preis_stva = 0
            End If

            .id_sap = CType(txtSAPID.Text, System.Int32)
            If Not (id_Record.Text = String.Empty) Then
                .id_recordset = CType(id_Record.Text, Int32)
            End If
            .id_user = m_User.UserID
            .id_session = Session.SessionID.ToString
            If Not (ddlKunnr.SelectedItem Is Nothing) Then
                .kundennr = ddlKunnr.SelectedItem.Value
                .kundenname = Left(ddlKunnr.SelectedItem.Text, ddlKunnr.SelectedItem.Text.IndexOf(" ~ "))
            End If
            .haltername = txtHalter.Text.ToUpper
            .internebemerkung = txtInterneBemerkung.Text.ToUpper
            .fahrgestellnr = txtFahrgestellNr.Text.ToUpper
            If Not (ddlSTVA.SelectedItem Is Nothing) Then
                .stvanr = ddlSTVA.SelectedItem.Value
                .stva = ddlSTVA.SelectedItem.Text
            End If
            .wunschennz = txtOrtsKz.Text.ToUpper
            .wunschennzABC = txtWunschkennzeichenAbc.Text.ToUpper
            .wunschennzNR = String.Empty
            .str_wunschkennz = .wunschennz & "-" & .wunschennzABC ' & " " & .wunschennzNR
            If Not (ddlDienst.SelectedItem Is Nothing) Then
                .dienstleistungnr = ddlDienst.SelectedItem.Value
                .dienstleistung = ddlDienst.SelectedItem.Text
            End If
            If Not (txtZulassungsdatum.Text = String.Empty) Then
                .zulassungsdatum = CType(clsVorerf01.toShortDateStr(txtZulassungsdatum.Text), Date)
            End If

            .toDelete = ""
            .saved = cbxSave.Checked
            .check2 = False
            .check3 = False
            .free1 = Now

            If Not (txtFree2.Value = String.Empty) Then
                .free2 = CType(txtFree2.Value, String)
            Else
                .free2 = String.Empty
            End If
            .free3 = String.Empty

            .kennztyp = ddlKzTyp.SelectedItem.Value
            .einkennz = cbxEinKennz.Checked
            .reserviertid = txtReservNr.Text
            .reserviert = cbxReserviert.Checked
            .wunschkennzflag = cbxWunschkennzFlag.Checked

            'Tour-Nr. extrahieren
            .tournr = ""
            temp = ddlKunnr.SelectedItem.Text
            If (temp.IndexOf("Tour") > 0) Then
                temp = temp.Substring(temp.IndexOf("Tour"), temp.Length - temp.IndexOf("Tour"))
                If temp.IndexOf("/") >= 0 Then
                    temp = temp.Substring(0, temp.IndexOf("/"))
                End If
                temp = temp.Replace("Tour", "").Trim
                .tournr = temp
            End If
            If (txtTour.Text <> String.Empty) Then
                .tournr = txtTour.Text
            End If

            'Alte Kunden-Nr. extrahieren
            temp = ddlKunnr.SelectedItem.Text
            temp = temp.Substring(temp.IndexOf(" ~ ") + 2, temp.Length - (temp.IndexOf(" ~ ") + 2)) 'Alte Kundennr = Neue Kundennr (Standard)
            If temp.IndexOf("~") > 0 Then   'Auch alte Kundennr vorhanden
                temp = temp.Substring(1, temp.IndexOf(" ~ ") - 1)
            End If
            .kundennralt = temp
            txtKundennummer.Text = Left(Trim(temp), 8)
            'Sonstige Dienstleistung
            .sonstDienst = String.Empty

            'Fremdbestand
            .fremdbestand = False
            'Barkunde
            .barkunde = False
            'Anlieferadresse    

            .name1 = txtName1.Text
            .strasse = txtStrasse.Text
            .plz = txtPLZ.Text
            .ort = txtOrt.Text
            .Notiz = txtNotiz.Text
            .Verkkurz = txtVKkurz.Text
            .KIReferenz = txtKIReferenz.Text

            .PAUART = txtAuart.Text
            .PZUEFUE = txtVerVbeln.Text
            .Feinstaub = chk_Feinstaub.Checked
            .Krad = chk_Krad.Checked
            'Speichern oder nicht?
            .tosave = False
        End With
        'Daten(speichern)
        If (update = False) Then
            zulDaten.saved = True
            zulDaten.recordset_Save(status, returnID)
            'INSERT
        Else
            zulDaten.recordset_Update(status, returnID)
            'UPDATE
        End If
        zulDaten.clear()    'Inhalte wieder löschen

        If status <> String.Empty Then 'Fehler, raus...
            lblError.Text = "Fehler beim Speichern der Daten!"
            Exit Sub
        Else
            txtSQLId.Text = "SQL-Id: " & returnID
        End If
    End Sub

    Private Function newVorerfassung() As Boolean
        Dim zDat As String
        Dim check As Boolean = True
        Dim c() As Char = {"1"c, "2"c, "3"c, "4"c, "5"c, "6"c, "7"c, "8"c, "9"c}

        'Wenn Listenanzeige aktiv, nicht prüfen und speichern 
        If (cbxListe.Checked = True) Then
            cbxListe.Checked = False
            Return False
            Exit Function
        End If

        'Plausibilitätsprüfung
        zDat = clsVorerf01.toShortDateStr(txtZulassungsdatum.Text)
        If ((zDat) <> String.Empty) Then
            If (IsDate(zDat) = False) Then
                lblError.Text = "Ungültiges Zulassungsdatum: Falsches Format."
                check = False
            Else
                Dim tagesdatum As Date = Date.Today
                tagesdatum = Date.Today
                tagesdatum = tagesdatum.AddYears(1)
                Dim tagesdatumBack As Date = Date.Today
                tagesdatumBack = Date.Today
                tagesdatumBack = tagesdatumBack.AddYears(-1)
                If zDat > tagesdatum Then
                    lblError.Text = "Das Datum darf max. 1 Jahr in der Zukunft liegen!"
                    check = False
                ElseIf zDat < tagesdatumBack Then
                    lblError.Text = "Das Datum darf max. 1 Jahr in der Vergangenheit liegen!"
                    check = False
                End If

            End If
        Else
            lblError.Text = "Ungültiges Zulassungsdatum: Falsches Format."
            check = False
        End If

        'und speichern...
        If (check = True) Then
            save(cbxSave.Checked)
            clearData()
        End If

        Return check
    End Function

    Private Function checkAdresse() As Boolean
        Dim wert As String
        Dim ok As Boolean = True
        'Prüfen, ob Adresse gefüllt
        Dim zulDienst As clsVorerf01
        Dim vw As DataView
        Dim rows As DataRow()

        zulDienst = CType(Session("ZulDaten"), clsVorerf01)
        vw = zulDienst.Kundentabelle.DefaultView
        vw.Sort = "NAME1"

        If txtKundennummer.Text.Trim = String.Empty Then
            wert = vw.Item(0)("KATR1").ToString
        Else
            If (txtDummy.Value = String.Empty) Then
                rows = zulDienst.Kundentabelle.Select("KUNNR=" & Right("0000000000" & txtKundennummer.Text, 10))
            Else
                rows = zulDienst.Kundentabelle.Select("KUNNR=" & ddlKunnr.Items(txtDummy.Value).Value.ToString)
            End If

            If (rows.Length = 0) Or (rows.Length > 1) Then
                lblError.Text = "Adressparamter konnte nicht gelesen werden."
                ok = False

                Return ok
                Exit Function
            Else
                wert = rows(0)("KATR1").ToString
            End If
        End If

        If (wert.Trim = "1Z") Then
            If txtName1.Text = String.Empty Then
                ok = False
            End If
            If txtStrasse.Text = String.Empty Then
                ok = False
            End If
            If txtPLZ.Text = String.Empty Then
                ok = False
            End If
            If txtOrt.Text = String.Empty Then
                ok = False
            End If
        End If

        If Not ok Then
            lblError.Text = "Bitte Anlieferadresse vollständig eingeben."
        End If

        Return ok
    End Function

    Private Function checkDienst() As Boolean
        Dim wert As String
        Dim ok As Boolean
        'Prüfen, ob Dienstleistung gefüllt
        ok = True
        wert = txtDienstHidden.Value
        If (wert = "- Keine Auswahl -") Or (wert = String.Empty) Then
            ok = False
        End If
        Return ok
    End Function

    Private Function checkStva() As Boolean
        Dim wert As String
        Dim ok As Boolean = True

        'Prüfen, ob Dienstleistung gefüllt
        wert = txtDienstHidden.Value
        If (wert = "- Keine Auswahl -") Or (wert = String.Empty) Then
            ok = False
        End If

        Return ok
    End Function

    Private Sub cmdNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdNew.Click
        Dim Check As Boolean
        If txtDummy.Value = "" Or txtDummy.Value = "0" Then
            If ddlKunnr.SelectedIndex < 1 Then
                lblError.Text = "Kein Kunde ausgewählt."
                Session("DummyValue") = Nothing
            ElseIf Not checkDienst() Then
                lblError.Text = "Keine Dienstleistung ausgewählt."
            ElseIf ddlSTVA.SelectedIndex < 1 Then
                lblError.Text = "Keine STVA ausgewählt."
            ElseIf txtOrtsKz.Text.Length = 0 Then
                lblError.Text = "1.Teil des Kennzeichen muss mit dem Amt gefüllt sein!"
            Else
                If checkAdresse() Then
                    txtDummy.Value = ddlKunnr.SelectedIndex.ToString
                    Session("DummyValue") = txtDummy.Value
                    Check = newVorerfassung()
                    If cbxSave.Checked Then
                        cbxSave.Checked = False
                    End If
                    If Check Then
                        ddlDienst.SelectedIndex = 0
                        txtDienstHidden.Value = String.Empty
                        txtDummy.Value = String.Empty
                    End If
                End If

            End If
        ElseIf Not checkDienst() Then
            lblError.Text = "Keine Dienstleistung ausgewählt."
        ElseIf ddlSTVA.SelectedIndex < 1 Then
            lblError.Text = "Keine STVA ausgewählt."
        ElseIf txtOrtsKz.Text.Length = 0 Then
            lblError.Text = "1.Teil des Kennzeichen muss mit dem Amt gefüllt sein!"
        Else
            If checkAdresse() Then
                Session("DummyValue") = txtDummy.Value
                Check = newVorerfassung()
                If cbxSave.Checked Then
                    cbxSave.Checked = False
                End If
                If Check Then
                    ddlDienst.SelectedIndex = 0
                    txtDienstHidden.Value = String.Empty
                    txtDummy.Value = String.Empty
                End If
            End If
        End If
    End Sub

    Public Function noRecords() As Boolean
        Return ((table Is Nothing) OrElse (table.Rows.Count = 0))
    End Function

    Private Sub btnListe_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnListe.Click
        Response.Redirect("Vorerf02.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub clearData()
        id_Record.Text = String.Empty
        txtInterneBemerkung.Text = String.Empty
        txtFree2.Value = String.Empty
        ddlDienst.SelectedIndex = 0
        cbxReserviert.Checked = False
        cbxWunschkennzFlag.Checked = False
        cbxEinKennz.Checked = False
        txtReservNr.Text = String.Empty
        txtVKkurz.Text = String.Empty
        txtKIReferenz.Text = String.Empty
        txtNotiz.Text = String.Empty
        txtOrtsKz.Text = ddlSTVA.SelectedItem.Value.ToString.ToUpper
        txtOrtsKzOld.Value = ddlSTVA.SelectedItem.Value.ToString.ToUpper
       
        txtTour.Text = String.Empty
        txtDummy.Value = String.Empty
        zulDaten.Kassengebuehr = 0
        chk_Feinstaub.Checked = 0
        chk_krad.Checked = 0
    End Sub

    Private Sub showData(ByVal table As DataTable, Optional ByVal rowIndex As Integer = 0)
        Dim litem As Integer
        Dim dat As String

        fillForm()

        cbxListe.Checked = False
        'Gespeichert
        cbxSave.Checked = CType(table.Rows(rowIndex)("saved"), Boolean)
        'ID
        id_Record.Text = table.Rows(rowIndex)("id").ToString
        'Kunde
        litem = ddlKunnr.Items.IndexOf(ddlKunnr.Items.FindByValue(CType(table.Rows(rowIndex).Item("kundennr"), String)))
        ddlKunnr.SelectedIndex = litem
        txtKundennummer.Text = CType(ddlKunnr.SelectedItem.Value, String).TrimStart("0"c)
        'Stva
        litem = ddlSTVA.Items.IndexOf(ddlSTVA.Items.FindByValue(CType(table.Rows(rowIndex).Item("stvanr"), String)))
        ddlSTVA.SelectedIndex = litem
        'Dienstleistung
        litem = ddlDienst.Items.IndexOf(ddlDienst.Items.FindByValue(CType(table.Rows(rowIndex).Item("dienstleistungnr"), String)))
        ddlDienst.SelectedIndex = litem
        txtDienstHidden.Value = ddlDienst.SelectedItem.Value
        'Halter
        txtHalter.Text = table.Rows(rowIndex)("haltername").ToString
        'Interne Bemerkung
        txtInterneBemerkung.Text = table.Rows(rowIndex)("internebemerkung").ToString
        'FahrgestellNr
        txtFahrgestellNr.Text = table.Rows(rowIndex)("fahrgestellnr").ToString
        txtWunschkennzeichen.Text = table.Rows(rowIndex)("wunschkennz").ToString
        txtWunschkennzeichenAbc.Text = table.Rows(rowIndex)("wunschkennzABC").ToString
        txtWunschkennzeichenNr.Text = String.Empty
        txtOrtsKz.Text = table.Rows(rowIndex)("wunschkennz").ToString
        txtStVASel.Text = table.Rows(rowIndex)("stvanr").ToString
        'ZulDatum
        dat = CType(table.Rows(rowIndex)("zulassungsdatum"), Date).ToShortDateString
        txtZulassungsdatum.Text = Left(dat, 2) & dat.Substring(3, 2) & Right(dat, 2)
        txtFree2.Value = table.Rows(rowIndex)("free2").ToString
        'ID
        txtSAPID.Text = table.Rows(rowIndex)("id_sap").ToString
        'Kennzeichentyp
        litem = ddlKzTyp.Items.IndexOf(ddlKzTyp.Items.FindByValue(CType(table.Rows(rowIndex).Item("kennztyp"), String)))
        ddlKzTyp.SelectedIndex = litem
        'Nur ein Kennzeichen
        cbxEinKennz.Checked = CType(table.Rows(rowIndex)("einkz"), Boolean)
        'ReservID
        txtReservNr.Text = table.Rows(rowIndex)("reservid").ToString
        'Reserviert
        cbxReserviert.Checked = CType(table.Rows(rowIndex)("reserv"), Boolean)
        'WunschkennzFlag
        cbxWunschkennzFlag.Checked = CType(table.Rows(rowIndex)("wunschkennzflag"), Boolean)
        'Tour
        txtTour.Text = table.Rows(rowIndex)("tour").ToString
        'Anlieferadresse
        txtName1.Text = table.Rows(rowIndex)("name1").ToString
        txtStrasse.Text = table.Rows(rowIndex)("strasse").ToString
        txtPLZ.Text = table.Rows(rowIndex)("plz").ToString
        txtOrt.Text = table.Rows(rowIndex)("ort").ToString
        txtVKkurz.Text = table.Rows(rowIndex)("VKkurz").ToString
        txtKIReferenz.Text = table.Rows(rowIndex)("KIReferenz").ToString
        txtNotiz.Text = table.Rows(rowIndex)("Notiz").ToString

        txtAuart.Text = table.Rows(rowIndex)("Auart").ToString.Trim
        txtVerVbeln.Text = table.Rows(rowIndex)("VerbVbeln").ToString.Trim

        chk_krad.Checked = CType(table.Rows(rowIndex)("krad"), Boolean)
        chk_Feinstaub.Checked = CType(table.Rows(rowIndex)("Feinstaub"), Boolean)
      
    End Sub

    Private Sub LinkButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LinkButton2.Click
        Dim url As String

        url = ddlUrl.Items(ddlSTVA.SelectedIndex).Text
        hypUrl.Target = "_blank"
        hypUrl.Text = "&#062;" & url
        hypUrl.NavigateUrl = "http://" & url
    End Sub

    Private Sub btnAdresse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdresse.Click

    End Sub

    Private Sub btnSaveListe_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        save(True)
        Response.Redirect("Input_004_02.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub cmdNewFore_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub btnBarcode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBarcode.Click
        Dim strStatus As String = ""
        Dim intCount As Integer
        Dim strSTVA As String = ""
        Dim strKennz As String = ""

        zulDaten = CType(Session("ZulDaten"), clsVorerf01)
        'Problem: bei FillDropdown wird bereits ein index gesetzt!
        If txtBarcode.Text.Trim <> String.Empty Then
            zulDaten.getSAPBarcodedaten(Right("0000000000" & txtBarcode.Text, 10), strStatus)
            If strStatus <> String.Empty Then
                lblError.Text = "Daten konnten nicht ermittelt werden."
            Else
                txtKundennummer.Text = zulDaten.PBarcodeKundennr.TrimStart("0"c)
                intCount = 0
                Try
                    While ((ddlKunnr.Items(intCount).Value) <> (zulDaten.PBarcodeKundennr))
                        intCount = intCount + 1
                    End While
                    ddlKunnr.SelectedIndex = intCount

                    intCount = 0
                    While ((ddlDienst.Items(intCount).Value) <> (zulDaten.PBarcodeDienst))
                        intCount = intCount + 1
                    End While
                    ddlDienst.SelectedIndex = intCount
                    txtDienstHidden.Value = ddlDienst.SelectedItem.Value

                    intCount = 0
                    strSTVA = zulDaten.PBarcodeStva.Substring(0, zulDaten.PBarcodeStva.IndexOf("-"))
                    strKennz = Right(zulDaten.PBarcodeStva, zulDaten.PBarcodeStva.Length - zulDaten.PBarcodeStva.IndexOf("-") - 1)
                    While ((ddlSTVA.Items(intCount).Value) <> (strSTVA))
                        intCount = intCount + 1
                    End While
                    ddlSTVA.SelectedIndex = intCount

                    txtDienstleistungsnr.Text = zulDaten.PBarcodeDienst.TrimStart("0"c)
                    txtStVASel.Text = strSTVA

                    txtOrtsKz.Text = strSTVA
                    txtWunschkennzeichenAbc.Text = strKennz

                    txtHalter.Text = zulDaten.PBarcodeRef1
                    txtFahrgestellNr.Text = zulDaten.PBarcodeRef2
                    If Not zulDaten.PAUART = String.Empty Then
                        txtInterneBemerkung.Text = "Mit Überführungsauftrag: " & zulDaten.PZUEFUE.TrimStart("0"c)
                        txtAuart.Text = zulDaten.PAUART
                        txtVerVbeln.Text = zulDaten.PZUEFUE.TrimStart("0"c)
                    End If
                Catch ex As Exception
                    lblError.Text = "Fehler beim lesen der Daten!"
                End Try
            End If
        End If

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

End Class

' ************************************************
' $History: Vorerf01.aspx.vb $
' 
' *****************  Version 11  *****************
' User: Rudolpho     Date: 29.04.10   Time: 16:59
' Updated in $/CKAG/Applications/AppKroschke/Forms
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 29.04.10   Time: 9:06
' Updated in $/CKAG/Applications/AppKroschke/Forms
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 18.03.10   Time: 10:31
' Updated in $/CKAG/Applications/AppKroschke/Forms
' 
' *****************  Version 8  *****************
' User: Fassbenders  Date: 11.03.10   Time: 14:32
' Updated in $/CKAG/Applications/AppKroschke/Forms
' ITA: 2918
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 7.10.09    Time: 13:47
' Updated in $/CKAG/Applications/AppKroschke/Forms
' ITA: 3110
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 29.04.09   Time: 13:46
' Updated in $/CKAG/Applications/AppKroschke/Forms
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 5.12.08    Time: 9:08
' Updated in $/CKAG/Applications/AppKroschke/Forms
' ITA:2149
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 14.10.08   Time: 13:07
' Updated in $/CKAG/Applications/AppKroschke/Forms
' ITA: 2301 & Warnungen bereinigt
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 25.09.08   Time: 17:14
' Updated in $/CKAG/Applications/AppKroschke/Forms
' ITA 2213
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 11.06.08   Time: 15:38
' Updated in $/CKAG/Applications/AppKroschke/Forms
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 14:30
' Created in $/CKAG/Applications/AppKroschke/Forms
' 
' *****************  Version 13  *****************
' User: Rudolpho     Date: 15.11.07   Time: 8:23
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb/Forms
' ITA:1433
' 
' *****************  Version 12  *****************
' User: Rudolpho     Date: 13.11.07   Time: 16:38
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb/Forms
' ITA: 1374, 1404, 1433
' 
' *****************  Version 11  *****************
' User: Rudolpho     Date: 21.08.07   Time: 17:40
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb/Forms
' ITA: 1242  Feinstaub und Krad beim nach Speichern/neuer Datensatz
' wieder rausgenommen
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 20.08.07   Time: 16:48
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb/Forms
' ITA: 1192 ,1242
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 6.07.07    Time: 14:29
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb/Forms
' ITA: 1130
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 4.07.07    Time: 17:06
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb/Forms
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 4.07.07    Time: 14:37
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb/Forms
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 4.07.07    Time: 12:39
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb/Forms
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 4.07.07    Time: 11:21
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb/Forms
' 
' *****************  Version 4  *****************
' User: Uha          Date: 2.07.07    Time: 13:07
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 3  *****************
' User: Uha          Date: 8.03.07    Time: 9:25
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb/Forms
' 
' ************************************************
