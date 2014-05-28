Imports System.Text.RegularExpressions

Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Imports AppUeberf.Ueberf_01
Imports AppUeberf.Helper
Imports AppUeberf.Controls.ProgressControl

Public Class Zul01
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private dv As DataView
    Private strError As String
    Private clsUeberf As Ueberf_01

    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents calVon As System.Web.UI.WebControls.Calendar
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents txtKennzeichen1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label10 As System.Web.UI.WebControls.Label
    Protected WithEvents Label6 As System.Web.UI.WebControls.Label
    Protected WithEvents txtAbgabetermin As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnVon As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdBack As System.Web.UI.WebControls.ImageButton
    Protected WithEvents cmdRight1 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents Label14 As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents txtHalter As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents txtHalterPLZ As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtKennZusatz1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label7 As System.Web.UI.WebControls.Label
    Protected WithEvents Label8 As System.Web.UI.WebControls.Label
    Protected WithEvents txtKennZusatz2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label9 As System.Web.UI.WebControls.Label
    Protected WithEvents txtKennzeichen2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtKennZusatz3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label11 As System.Web.UI.WebControls.Label
    Protected WithEvents txtKennzeichen3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnZulkreis As System.Web.UI.WebControls.LinkButton
    Protected WithEvents Label12 As System.Web.UI.WebControls.Label
    Protected WithEvents txtZulkreis As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBemerkung As CKG.Portal.PageElements.TextAreaControl
    Protected WithEvents Label16 As System.Web.UI.WebControls.Label
    Protected WithEvents Label17 As System.Web.UI.WebControls.Label
    Protected WithEvents txtVersicherer As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label18 As System.Web.UI.WebControls.Label
    Protected WithEvents rdSteuerzahlung As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents Label20 As System.Web.UI.WebControls.Label
    Protected WithEvents drpHalter As System.Web.UI.WebControls.DropDownList
    Protected WithEvents Label21 As System.Web.UI.WebControls.Label
    Protected WithEvents drpVersicherer As System.Web.UI.WebControls.DropDownList
    Protected WithEvents rdbVersicherungsnehmer As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents ucStyles As Styles
    Protected WithEvents txtHalterOrt As System.Web.UI.WebControls.TextBox
    Protected WithEvents chkUbernahmeLeasingnehmer As System.Web.UI.WebControls.CheckBox
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents pnlSchilderversand As System.Web.UI.WebControls.Panel
    Protected WithEvents txtSchilderversandPLZOrt As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtSchilderversandStraßeHausnr As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtSchilderversandName As System.Web.UI.WebControls.TextBox
    Protected WithEvents ctrlAddressSearchHalter As Controls.AddressSearchInputControl
    Protected WithEvents ctrlAddressSearchSchilder As Controls.AddressSearchInputControl
    Protected WithEvents chkUebernahmeLeasingnehmerSchilder As System.Web.UI.WebControls.CheckBox
    Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    Protected WithEvents Label13 As System.Web.UI.WebControls.Label
    Protected WithEvents Label15 As System.Web.UI.WebControls.Label
    Protected WithEvents Label19 As System.Web.UI.WebControls.Label
    Protected WithEvents pnlSpace As System.Web.UI.WebControls.Panel
    Protected WithEvents Label22 As System.Web.UI.WebControls.Label
    Protected WithEvents drpSchilderversand As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ProgressControl1 As Controls.ProgressControl


#Region " Vom Web Form Designer generierter Code "

    'Dieser Aufruf ist für den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: Diese Methode ist für den Web Form-Designer erforderlich
        'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
        InitializeComponent()

        'Suchmasken mit Dropdownlisten verbinden, damit die Ergebnisse angezeigt werden
        ctrlAddressSearchHalter.ResultDropdownList = drpHalter
        ctrlAddressSearchSchilder.ResultDropdownList = drpSchilderversand
    End Sub

#End Region

#Region "Events"

    '-------
    'Laden der Seite
    '-------
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        lblError.Text = ""

        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try

        If Request.UrlReferrer.ToString.IndexOf("UeberfZulStart.aspx") = 0 Then
            Session(CONST_SESSION_UEBERFUEHRUNG) = Nothing
            clsUeberf = Nothing
        End If


        If Session(CONST_SESSION_UEBERFUEHRUNG) Is Nothing Then
            clsUeberf = New Ueberf_01(m_User, m_App, "")
            Session(CONST_SESSION_UEBERFUEHRUNG) = clsUeberf
        Else
            clsUeberf = Session(CONST_SESSION_UEBERFUEHRUNG)
        End If


        If Not IsPostBack Then
            'Controls initial befüllen
            InitialFill()
            FillControls(clsUeberf)
        End If

        'Partner-Daten wieder aus der Session ladens
        If IsNothing(dv) Then
            dv = Session("DataView")
        End If

        ProgressControl1.Fill(Source.Zul01, clsUeberf)

    End Sub

    '-------
    'Speichern der Daten und weiter 
    '-------
    Private Sub cmdRight1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdRight1.Click
        Try

            'Daten in das Überführungs-Objekt schreiben
            clsUeberf = CType(Session(CONST_SESSION_UEBERFUEHRUNG), Ueberf_01)
            SetData(clsUeberf)


            If clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.ZulassungKCL Then
                Response.Redirect("ZulBest01.aspx?AppID=" & Session("AppID").ToString)
            Else
                Response.Redirect("Ueberf01.aspx?AppID=" & Session("AppID").ToString)
            End If

        Catch mex As Exceptions.MandatoryDataMissingException
            lblError.Text = mex.Message
        Catch ex As Exception
            lblError.Text = "Fehler beim Speichern der Daten: " & ex.Message
        End Try

    End Sub

    '--------
    'Zurück gehen
    '--------
    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdBack.Click

        If clsUeberf Is Nothing Then
            clsUeberf = Session(CONST_SESSION_UEBERFUEHRUNG)
        End If


        Session(CONST_SESSION_UEBERFUEHRUNG) = clsUeberf
        Response.Redirect("UeberfZulStart.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    '--------
    'Kalender anzeigen
    '--------
    Private Sub btnVon_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVon.Click
        calVon.Visible = True
    End Sub

    '--------
    'Datum auf Kalender ausgewählt
    '--------
    Private Sub calVon_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calVon.SelectionChanged
        txtAbgabetermin.Text = calVon.SelectedDate.ToShortDateString
        calVon.Visible = False
    End Sub

    '--------
    'Zulassungskreis-Knopf gedrückt
    '--------
    Private Sub btnZulkreis_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnZulkreis.Click


        If IsNothing(clsUeberf) Then
            clsUeberf = Session(CONST_SESSION_UEBERFUEHRUNG)
        End If

        If Len(txtHalterPLZ.Text) <> 5 Then
            lblError.Text = "Bitte geben Sie eine 5-stellige PLZ ein."
        Else
            clsUeberf.getSTVA(txtHalterPLZ.Text)

            If Not clsUeberf.tblKreis Is Nothing Then

                If clsUeberf.tblKreis.Rows.Count > 0 Then
                    txtZulkreis.Text = clsUeberf.tblKreis.Rows(0)("ZKFZKZ").ToString()
                    txtKennzeichen1.Text = clsUeberf.tblKreis.Rows(0)("ZKFZKZ").ToString()
                    txtKennzeichen2.Text = clsUeberf.tblKreis.Rows(0)("ZKFZKZ").ToString()
                    txtKennzeichen3.Text = clsUeberf.tblKreis.Rows(0)("ZKFZKZ").ToString()
                Else
                    lblError.Text = "Für die eingegebene PLZ konnte kein Zulassungskreis ermittelt werden."
                    txtZulkreis.Text = ""
                    txtKennzeichen1.Text = ""
                    txtKennzeichen2.Text = ""
                    txtKennzeichen3.Text = ""
                End If
            End If
        End If
    End Sub


    '---------
    'Es wurde ein neues Halter ausgewählt
    '---------
    Private Sub drpHalter_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpHalter.SelectedIndexChanged
        chkUbernahmeLeasingnehmer.Checked = False
        If drpHalter.Items.Count = 0 OrElse drpHalter.SelectedItem.Value = "" OrElse drpHalter.SelectedItem.Value = "0" Then
            txtHalter.Text = ""
            txtHalter.Enabled = True
            txtHalterPLZ.Text = ""
            txtHalterPLZ.Enabled = True
            txtHalterOrt.Text = ""
            txtHalterOrt.Enabled = True
        Else
            Dim row As DataSets.AddressDataSet.ADDRESSERow = CType(CType(Session(drpHalter.ID), DataTable).Select("ID='" + drpHalter.SelectedItem.Value + "'")(0), DataSets.AddressDataSet.ADDRESSERow)
            txtHalter.Text = row.NAME
            txtHalter.Enabled = False
            txtHalterPLZ.Text = row.PLZ
            txtHalterPLZ.Enabled = False
            txtHalterOrt.Text = row.ORT
            txtHalterOrt.Enabled = False

        End If
    End Sub

    '---------
    'Schilderversand ausgewählt
    '---------
    Private Sub drpSchilderversand_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpSchilderversand.SelectedIndexChanged
        chkUbernahmeLeasingnehmer.Checked = False
        If drpSchilderversand.Items.Count = 0 OrElse drpSchilderversand.SelectedItem.Value = "" OrElse drpSchilderversand.SelectedItem.Value = "0" Then
            txtSchilderversandName.Text = ""
            txtSchilderversandName.Enabled = True
            txtSchilderversandPLZOrt.Text = ""
            txtSchilderversandPLZOrt.Enabled = True
            txtSchilderversandStraßeHausnr.Text = ""
            txtSchilderversandStraßeHausnr.Enabled = True
        Else
            Dim row As DataSets.AddressDataSet.ADDRESSERow = CType(CType(Session(drpSchilderversand.ID), DataTable).Select("ID='" + drpSchilderversand.SelectedItem.Value + "'")(0), DataSets.AddressDataSet.ADDRESSERow)
            txtSchilderversandName.Text = row.NAME
            'txtSchilderversandName.Enabled = False
            txtSchilderversandPLZOrt.Text = row.PLZ + " " + row.ORT
            'txtSchilderversandPLZOrt.Enabled = False
            txtSchilderversandStraßeHausnr.Text = row.STRASSE + " " + row.HAUSNUMMER
            'txtSchilderversandStraßeHausnr.Enabled = False
        End If
    End Sub

    '----------
    'Halter - Übernahme Leasingnehmer
    '----------
    Private Sub chkUbernahmeLeasingnehmer_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUbernahmeLeasingnehmer.CheckedChanged
        Try
            drpHalter.SelectedIndex = -1
            If chkUbernahmeLeasingnehmer.Checked Then
                txtHalter.Text = clsUeberf.Leasingnehmer
                txtHalterPLZ.Text = clsUeberf.LeasingnehmerPLZ
                txtHalterOrt.Text = clsUeberf.LeasingnehmerOrt
                txtHalter.Enabled = False
                txtHalterPLZ.Enabled = False
                txtHalterOrt.Enabled = False
            Else
                txtHalter.Text = ""
                txtHalterPLZ.Text = ""
                txtHalterOrt.Text = ""
                txtHalter.Enabled = True
                txtHalterPLZ.Enabled = True
                txtHalterOrt.Enabled = True
            End If

        Catch ex As Exception
            lblError.Text = "Fehler bei der Übernahme der Leasingnehmer-Daten: " & ex.Message
        End Try
    End Sub

    '----------
    'Schilderversand - Übernahme Leasingnehmer
    '----------
    Private Sub chkUebernahmeLeasingnehmerSchilder_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUebernahmeLeasingnehmerSchilder.CheckedChanged
        Try
            drpSchilderversand.SelectedIndex = -1
            If chkUbernahmeLeasingnehmer.Checked Then
                txtSchilderversandName.Text = clsUeberf.Leasingnehmer
                txtSchilderversandPLZOrt.Text = clsUeberf.LeasingnehmerPLZ + " " + clsUeberf.LeasingnehmerOrt
                txtSchilderversandStraßeHausnr.Text = clsUeberf.LeasingnehmerStrasse + " " + clsUeberf.LeasingnehmerHausnummer
                'txtHalter.Enabled = False
                'txtHalterPLZ.Enabled = False
                'txtHalterOrt.Enabled = False
            Else
                txtHalter.Text = ""
                txtHalterPLZ.Text = ""
                txtHalterOrt.Text = ""
                txtHalter.Enabled = True
                txtHalterPLZ.Enabled = True
                txtHalterOrt.Enabled = True
            End If

        Catch ex As Exception
            lblError.Text = "Fehler bei der Übernahme der Leasingnehmer-Daten: " & ex.Message
        End Try
    End Sub

    '----------
    'Versicherer wurde ausgewählt
    '----------
    Private Sub drpVersicherer_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpVersicherer.SelectedIndexChanged
        If drpVersicherer.SelectedIndex = -1 OrElse drpVersicherer.SelectedItem.Value = "" OrElse drpVersicherer.SelectedItem.Value = "0" Then
            txtVersicherer.Text = ""
            txtVersicherer.Enabled = True
        Else
            Dim row As DataSets.AddressDataSet.ADDRESSERow = CType(CType(Session(drpVersicherer.ID), DataTable).Select("ID='" + drpVersicherer.SelectedItem.Value + "'")(0), DataSets.AddressDataSet.ADDRESSERow)
            txtVersicherer.Text = row.NAME + ", " + row.ORT
            txtVersicherer.Enabled = False
            clsUeberf.selVersicherer = drpVersicherer.SelectedItem.Value()
        End If
    End Sub

    '---------
    'RadioButton Versicherungsnehmer hat sich geändert
    '---------
    Private Sub rdbVersicherungsnehmer_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdbVersicherungsnehmer.SelectedIndexChanged
        clsUeberf.Versicherungsnehmer = rdbVersicherungsnehmer.SelectedItem.Text
    End Sub

    '---------
    'RadioButton Steuerzahl hat sich geändert
    '---------
    Private Sub rdSteuerzahlung_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdSteuerzahlung.SelectedIndexChanged
        clsUeberf.KfzSteuer = rdSteuerzahlung.SelectedItem.Text
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
#End Region

#Region "Methoden"

    '----------
    'Liefert der Kennzeichenzusatz ab dem "-"
    '----------
    Private Function GetKennzeichenZusatz(ByVal kennzeichen As String) As String
        If kennzeichen Is Nothing Then
            Return ""
        Else
            Return Regex.Replace(kennzeichen, "^[^\-]*\-(.*)$", "$1", RegexOptions.Singleline)
        End If
    End Function

    Private Sub FillControls(ByVal daten As Ueberf_01)


        With daten

            txtZulkreis.Text = .Kenn1
            txtKennzeichen1.Text = .Kenn1
            txtKennzeichen2.Text = .Kenn1
            txtKennzeichen3.Text = .Kenn1

            txtKennZusatz1.Text = GetKennzeichenZusatz(.Wunschkennzeichen1) 'Mid(.Wunschkennzeichen1, (Len(.Wunschkennzeichen1) + 1) - InStr(.Wunschkennzeichen1, "-"))
            txtKennZusatz2.Text = GetKennzeichenZusatz(.Wunschkennzeichen2) 'Mid(.Wunschkennzeichen2, (Len(.Wunschkennzeichen2) + 1) - InStr(.Wunschkennzeichen2, "-"))
            txtKennZusatz3.Text = GetKennzeichenZusatz(.Wunschkennzeichen3) 'Mid(.Wunschkennzeichen3, (Len(.Wunschkennzeichen3) + 1) - InStr(.Wunschkennzeichen3, "-"))


            If .Zulassungsdatum <> Date.MinValue Then
                txtAbgabetermin.Text = .Zulassungsdatum
            Else
                txtAbgabetermin.Text = ""
            End If

            txtHalter.Text = .ZulHaltername
            txtHalterPLZ.Text = .ZulPLZ
            txtHalterOrt.Text = .ZulOrt
            txtVersicherer.Text = .Versicherer

            If .Versicherungsnehmer <> "" Then
                rdbVersicherungsnehmer.Items.FindByText(.Versicherungsnehmer).Selected = True
            End If

            If .KfzSteuer <> "" Then
                rdSteuerzahlung.Items.FindByText(.KfzSteuer).Selected = True
            End If


            If .HalterAdressStatus = AdressStatus.Gesperrt OrElse .HalterAdressStatus = AdressStatus.KopieVonLeasingnehmer Then
                txtHalter.Enabled = False
                txtHalterPLZ.Enabled = False
                txtHalterOrt.Enabled = False
            Else
                txtHalter.Enabled = True
                txtHalterPLZ.Enabled = True
                txtHalterOrt.Enabled = True
            End If

            chkUbernahmeLeasingnehmer.Checked = (.HalterAdressStatus = AdressStatus.KopieVonLeasingnehmer)


            'Versicherer-Daten
            Dim liVers As ListItem = drpVersicherer.Items.FindByValue(.selVersicherer)
            If liVers Is Nothing Then
                liVers = New ListItem(.selVersicherer)
                drpVersicherer.Items.Add(liVers)
            End If
            liVers.Selected = True

            txtVersicherer.Text = .Versicherer

            If .selVersicherer <> "0" Then
                txtVersicherer.Enabled = False
            Else
                txtVersicherer.Enabled = True
            End If

            txtBemerkung.Text = .BemerkungLease

            If .Beauftragung <> Beauftragungsart.ZulassungKCL Then
                'Keine Überführung ausgewählt
                pnlSchilderversand.Visible = False
            Else
                pnlSchilderversand.Visible = True
                txtSchilderversandName.Text = .SchildversandName
                txtSchilderversandPLZOrt.Text = .SchildversandPLZOrt
                txtSchilderversandStraßeHausnr.Text = .SchildversandStrasseHausnr
            End If

            .zulModus = 0
        End With


    End Sub

    Private Sub SetData(ByVal daten As Ueberf_01)

        Dim error_msg As New System.Text.StringBuilder()

        With daten

            If ControlHelper.CheckMandatoryField(txtHalter, "Bitte geben Sie einen Zulassungskreis an.", error_msg) Then
                .Kenn1 = txtZulkreis.Text
            End If


            If Not Trim(txtKennZusatz1.Text) = String.Empty Then
                .Wunschkennzeichen1 = txtKennzeichen1.Text & "-" & txtKennZusatz1.Text
            End If

            If Not Trim(txtKennZusatz2.Text) = String.Empty Then
                .Wunschkennzeichen2 = txtKennzeichen2.Text & "-" & txtKennZusatz2.Text
            End If


            If Not Trim(txtKennZusatz3.Text) = String.Empty Then
                .Wunschkennzeichen3 = txtKennzeichen3.Text & "-" & txtKennZusatz3.Text
            End If


            Try
                Dim dteZulassung As Date = Date.Parse(txtAbgabetermin.Text)

                If dteZulassung <= Date.Today Then
                    error_msg.Append("Bitte geben Sie ein Zulassungsdatum in der Zukunft ein.")
                End If

                .Zulassungsdatum = dteZulassung
            Catch ex As Exception
                error_msg.Append("Bitte geben Sie ein gültiges Zulassungsdatum ein.")
            End Try


            'Vorschlagswert für die Überführung
            'Soll laut IT-Anforderung 715 nicht mehr übernommen werden
            '.DatumUeberf = txtAbgabetermin.Text

            '------
            'Halter-Daten
            '------
            If ControlHelper.CheckMandatoryField(txtHalter, "Bitte geben Sie einen Halter an.", error_msg) Then
                .ZulHaltername = txtHalter.Text
            End If

            If ControlHelper.CheckMandatoryField(txtHalterPLZ, "Bitte geben Sie eine PLZ für den Halter an.", error_msg) Then
                .ZulPLZ = txtHalterPLZ.Text
            End If

            If ControlHelper.CheckMandatoryField(txtHalterOrt, "Bitte geben Sie einen Ort für den Halter an.", error_msg) Then
                .ZulOrt = txtHalterOrt.Text
            End If


            If ControlHelper.CheckMandatoryField(txtVersicherer, "Bitte geben Sie einen Versicherer an.", error_msg) Then
                .Versicherer = txtVersicherer.Text
            End If

            .BemerkungLease = txtBemerkung.Text


            'Schildversand speichern
            .SchildversandName = txtSchilderversandName.Text
            .SchildversandPLZOrt = txtSchilderversandPLZOrt.Text
            .SchildversandStrasseHausnr = txtSchilderversandStraßeHausnr.Text

            'Einträge in den Dropdownlists
            .selVersicherer = drpVersicherer.SelectedItem.Value

            If chkUbernahmeLeasingnehmer.Checked Then
                .HalterAdressStatus = AdressStatus.KopieVonLeasingnehmer
            ElseIf Not txtHalter.Enabled OrElse txtHalter.ReadOnly Then
                .HalterAdressStatus = AdressStatus.Gesperrt
            Else
                .HalterAdressStatus = AdressStatus.Frei
            End If

        End With

        '----
        'Überprüfen, ob ein Fehler aufgetreten ist
        '----
        If error_msg.Length > 0 Then
            Throw New Exceptions.MandatoryDataMissingException(error_msg.ToString())
        End If

    End Sub

    Private Sub InitialFill()

        Dim tblPartner As DataSets.AddressDataSet.ADDRESSEDataTable
        tblPartner = clsUeberf.getPartner(m_User.KUNNR)
        dv = tblPartner.DefaultView

        Dim e As Long = 0
        dv.RowFilter = "TYP = 'ZV'"
        Session(drpVersicherer.ID) = dv.Table 'Damit man beim Selektieren wieder auf die Daten zugreifen kann
        drpVersicherer.AutoPostBack = True
        With drpVersicherer
            .Items.Add(New ListItem("Manuelle Eingabe", "0"))
            Do While e < dv.Count

                Dim addrRow As DataSets.AddressDataSet.ADDRESSERow = CType(dv.Item(e).Row, DataSets.AddressDataSet.ADDRESSERow)
                .Items.Add(New ListItem(addrRow.NAME + ", " + addrRow.ORT, addrRow.ID))
                e = e + 1
            Loop
        End With

        'DataView für die Partner
        If (Session("DataView")) Is Nothing Then
            Session.Add("DataView", dv)
        Else
            Session("DataView") = dv
        End If


    End Sub

#End Region
End Class

' ************************************************
' $History: Zul01.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 16.04.08   Time: 11:53
' Updated in $/CKAG/Applications/AppUeberf/Forms
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 16:06
' Created in $/CKAG/Applications/AppUeberf/Forms
' 
' *****************  Version 12  *****************
' User: Rudolpho     Date: 30.11.07   Time: 10:54
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' ITA: 1255
' 
' *****************  Version 11  *****************
' User: Uha          Date: 3.07.07    Time: 19:42
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' FormAuth in diverse Seiten wieder eingefügt
' 
' *****************  Version 10  *****************
' User: Uha          Date: 2.07.07    Time: 10:05
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 9  *****************
' User: Uha          Date: 22.05.07   Time: 10:49
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 8  *****************
' User: Uha          Date: 21.05.07   Time: 11:47
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Änderungen im Vergleich zur Startapplikation zum Stand 11.05.2007
' 
' *****************  Version 7  *****************
' User: Uha          Date: 3.05.07    Time: 15:16
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Änderungen aus StartApplication vom 02.05.2007 Mittags übernommen
' 
' ************************************************