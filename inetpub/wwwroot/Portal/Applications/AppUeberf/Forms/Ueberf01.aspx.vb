Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Imports AppUeberf.Ueberf_01
Imports AppUeberf.Controls.ProgressControl

Public Class Ueberf01
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private dv As DataView
    Private strError As String
    Private clsUeberf As Ueberf_01

    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents drpAbholung As System.Web.UI.WebControls.DropDownList
    Protected WithEvents chkUbernahmeLeasingnehmerAbholung As System.Web.UI.WebControls.CheckBox
    Protected WithEvents txtAbName As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblAbKundennummer As System.Web.UI.WebControls.Label
    Protected WithEvents txtAbStrasse As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAbNr As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAbPLZ As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAbOrt As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAbAnsprechpartner As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAbTelefon1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAbTelefon2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label7 As System.Web.UI.WebControls.Label
    Protected WithEvents drpAnlieferung As System.Web.UI.WebControls.DropDownList
    Protected WithEvents chkUbernahmeLeasingnehmerAnlieferung As System.Web.UI.WebControls.CheckBox
    Protected WithEvents txtAnName As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblAnKundennummer As System.Web.UI.WebControls.Label
    Protected WithEvents txtAnStrasse As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAnNr As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAnPLZ As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAnOrt As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAnAnsprechpartner As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAnTelefon1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAnTelefon2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdBack As System.Web.UI.WebControls.ImageButton
    Protected WithEvents cmdRight1 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents ProgressControl1 As Controls.ProgressControl
    Protected WithEvents ctrlAddressSearchAbholung As Controls.AddressSearchInputControl
    Protected WithEvents ctrlAddressSearchAnlieferung As Controls.AddressSearchInputControl

#Region " Vom Web Form Designer generierter Code "

    'Dieser Aufruf ist für den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: Diese Methode ist für den Web Form-Designer erforderlich
        'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
        InitializeComponent()

        'Suchmasken mit Dropdownlisten verbinden, damit die Ergebnisse angezeigt werden
        Me.ctrlAddressSearchAbholung.ResultDropdownList = drpAbholung
        Me.ctrlAddressSearchAnlieferung.ResultDropdownList = drpAnlieferung

    End Sub

#End Region

#Region "Methoden"

    Private Sub FillControlls()
        Dim tblPartner As DataTable

        If Session("Ueberf") Is Nothing Then
            clsUeberf = New Ueberf_01(m_User, m_App, "")
        Else
            clsUeberf = Session("Ueberf")
        End If

        tblPartner = clsUeberf.getPartner(m_User.KUNNR)
        dv = tblPartner.DefaultView



        If (Session("DataView")) Is Nothing Then
            Session.Add("DataView", dv)
        Else
            Session("DataView") = dv
        End If

        If (Session("Ueberf")) Is Nothing Then
            Session.Add("Ueberf", clsUeberf)
        Else
            Session("Ueberf") = clsUeberf
        End If

    End Sub

    Private Sub Refill()
        With clsUeberf

            Me.txtAbName.Text = .AbName
            Me.txtAbStrasse.Text = .AbStrasse
            Me.txtAbNr.Text = .AbNr
            Me.txtAbPLZ.Text = .AbPlz
            Me.txtAbOrt.Text = .AbOrt
            Me.txtAbAnsprechpartner.Text = .AbAnsprechpartner
            Me.txtAbTelefon1.Text = .AbTelefon1
            Me.txtAbTelefon2.Text = .AbTelefon2
            Me.lblAbKundennummer.Text = .AbKundennummer

            If .AdressStatusAbholung <> AdressStatus.Frei Then
                Me.txtAbName.Enabled = False
                Me.txtAbStrasse.Enabled = False
                Me.txtAbNr.Enabled = False
                Me.txtAbPLZ.Enabled = False
                Me.txtAbOrt.Enabled = False
                'Me.txtAbAnsprechpartner.Enabled = False
                'Me.txtAbTelefon1.Enabled = False
                'Me.txtAbTelefon2.Enabled = False
            Else
                Me.txtAbName.Enabled = True
                Me.txtAbStrasse.Enabled = True
                Me.txtAbNr.Enabled = True
                Me.txtAbPLZ.Enabled = True
                Me.txtAbOrt.Enabled = True
                'Me.txtAbAnsprechpartner.Enabled = True
                'Me.txtAbTelefon1.Enabled = True
                'Me.txtAbTelefon2.Enabled = True
            End If
            chkUbernahmeLeasingnehmerAbholung.Checked = (.AdressStatusAbholung = AdressStatus.KopieVonLeasingnehmer)

            Me.txtAnName.Text = .AnName
            Me.txtAnStrasse.Text = .AnStrasse
            Me.txtAnNr.Text = .AnNr
            Me.txtAnPLZ.Text = .AnPlz
            Me.txtAnOrt.Text = .AnOrt
            Me.txtAnAnsprechpartner.Text = .AnAnsprechpartner
            Me.txtAnTelefon1.Text = .AnTelefon1
            Me.txtAnTelefon2.Text = .AnTelefon2
            Me.lblAnKundennummer.Text = .AnKundennummer


            If .AdressStatusAnlieferung <> AdressStatus.Frei Then
                Me.txtAnName.Enabled = False
                Me.txtAnStrasse.Enabled = False
                Me.txtAnNr.Enabled = False
                Me.txtAnPLZ.Enabled = False
                Me.txtAnOrt.Enabled = False
                'Me.txtAnAnsprechpartner.Enabled = False
                'Me.txtAnTelefon1.Enabled = False
                'Me.txtAnTelefon2.Enabled = False
            Else
                Me.txtAnName.Enabled = True
                Me.txtAnStrasse.Enabled = True
                Me.txtAnNr.Enabled = True
                Me.txtAnPLZ.Enabled = True
                Me.txtAnOrt.Enabled = True
                'Me.txtAnAnsprechpartner.Enabled = True
                'Me.txtAnTelefon1.Enabled = True
                'Me.txtAnTelefon2.Enabled = True
            End If
            chkUbernahmeLeasingnehmerAnlieferung.Checked = (.AdressStatusAnlieferung = AdressStatus.KopieVonLeasingnehmer)

            .Modus = 0
        End With



    End Sub

#End Region

#Region "Events"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            'DoSubmit()
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try

        If Request.UrlReferrer.ToString.IndexOf("Selection.aspx") = 0 Then
            Session("Ueberf") = Nothing
            clsUeberf = Nothing
        End If


        If IsPostBack = False Then
            FillControlls()
        End If


        If IsNothing(dv) Then
            dv = Session("DataView")
        End If

        If clsUeberf Is Nothing Then
            clsUeberf = Session("Ueberf")
            If clsUeberf.Modus = 1 Then
                Refill()
            End If
        Else
            If clsUeberf.Modus = 1 Then
                Refill()
            End If
        End If

        'Beaufragungsart "Reine Überführung"
        If clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.ReineUeberfuehrung Or _
                    clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.ZulassungUndUeberfuehrung Or _
                    clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.ZulassungUndUeberfuehrungKCL Or _
                    clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.UeberfuehrungKCL Then

            cmdBack.Visible = True
        End If

        ProgressControl1.Fill(Source.Ueber01, clsUeberf)
    End Sub


    '-----------
    'Übernahme von Leasingnehmerdaten für Abholungsadresse
    '-----------
    Private Sub chkUbernahmeLeasingnehmerAbholung_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUbernahmeLeasingnehmerAbholung.CheckedChanged
        Try
            Me.drpAbholung.SelectedIndex = -1
            If chkUbernahmeLeasingnehmerAbholung.Checked Then
                Me.txtAbName.Text = clsUeberf.Leasingnehmer
                Me.txtAbPLZ.Text = clsUeberf.LeasingnehmerPLZ
                Me.txtAbOrt.Text = clsUeberf.LeasingnehmerOrt
                Me.txtAbStrasse.Text = clsUeberf.LeasingnehmerStrasse
                Me.txtAbNr.Text = clsUeberf.LeasingnehmerHausnummer
                Me.lblAbKundennummer.Text = clsUeberf.LeasingnehmerKundennummer
                Me.txtAbAnsprechpartner.Text = clsUeberf.LeasingnehmerAnsprechpartner
                Me.txtAbTelefon1.Text = clsUeberf.LeasingnehmerTelefon1
                Me.txtAbTelefon2.Text = clsUeberf.LeasingnehmerTelefon2

                Me.txtAbName.Enabled = False
                Me.txtAbPLZ.Enabled = False
                Me.txtAbOrt.Enabled = False
                Me.txtAbStrasse.Enabled = False
                Me.txtAbNr.Enabled = False
            Else
                Me.txtAbName.Text = ""
                Me.txtAbPLZ.Text = ""
                Me.txtAbOrt.Text = ""
                Me.txtAbStrasse.Text = ""
                Me.txtAbNr.Text = ""
                Me.lblAbKundennummer.Text = ""
                Me.txtAbAnsprechpartner.Text = ""
                Me.txtAbTelefon1.Text = ""
                Me.txtAbTelefon2.Text = ""

                Me.txtAbName.Enabled = True
                Me.txtAbPLZ.Enabled = True
                Me.txtAbOrt.Enabled = True
                Me.txtAbStrasse.Enabled = True
                Me.txtAbNr.Enabled = True

            End If

        Catch ex As Exception
            lblError.Text = "Fehler bei der Übernahme der Leasingnehmer-Daten: " & ex.Message
        End Try
    End Sub

    '-----------
    'Übernahme von Leasingnehmerdaten für Anlieferungsadresse
    '-----------
    Private Sub chkUbernahmeLeasingnehmerAnlieferung_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUbernahmeLeasingnehmerAnlieferung.CheckedChanged
        Try
            Me.drpAnlieferung.SelectedIndex = -1
            If chkUbernahmeLeasingnehmerAnlieferung.Checked Then
                Me.txtAnName.Text = clsUeberf.Leasingnehmer
                Me.txtAnPLZ.Text = clsUeberf.LeasingnehmerPLZ
                Me.txtAnOrt.Text = clsUeberf.LeasingnehmerOrt
                Me.txtAnStrasse.Text = clsUeberf.LeasingnehmerStrasse
                Me.txtAnNr.Text = clsUeberf.LeasingnehmerHausnummer
                Me.lblAnKundennummer.Text = clsUeberf.LeasingnehmerKundennummer
                Me.txtAnAnsprechpartner.Text = clsUeberf.LeasingnehmerAnsprechpartner
                Me.txtAnTelefon1.Text = clsUeberf.LeasingnehmerTelefon1
                Me.txtAnTelefon2.Text = clsUeberf.LeasingnehmerTelefon2

                Me.txtAnName.Enabled = False
                Me.txtAnPLZ.Enabled = False
                Me.txtAnOrt.Enabled = False
                Me.txtAnStrasse.Enabled = False
                Me.txtAnNr.Enabled = False
            Else
                Me.txtAnName.Text = ""
                Me.txtAnPLZ.Text = ""
                Me.txtAnOrt.Text = ""
                Me.txtAnStrasse.Text = ""
                Me.txtAnNr.Text = ""
                Me.lblAnKundennummer.Text = ""
                Me.txtAnAnsprechpartner.Text = ""
                Me.txtAnTelefon1.Text = ""
                Me.txtAnTelefon2.Text = ""

                Me.txtAnName.Enabled = True
                Me.txtAnPLZ.Enabled = True
                Me.txtAnOrt.Enabled = True
                Me.txtAnStrasse.Enabled = True
                Me.txtAnNr.Enabled = True
            End If

        Catch ex As Exception
            lblError.Text = "Fehler bei der Übernahme der Leasingnehmer-Daten: " & ex.Message
        End Try
    End Sub

    Private Sub drpAbholung_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles drpAbholung.SelectedIndexChanged

        Me.chkUbernahmeLeasingnehmerAbholung.Checked = False
        If drpAbholung.SelectedItem.Value = "" OrElse drpAbholung.SelectedItem.Value = "0" Then
            Me.txtAbName.Text = ""
            Me.txtAbName.Enabled = True
            Me.txtAbStrasse.Text = ""
            Me.txtAbStrasse.Enabled = True
            Me.txtAbNr.Text = ""
            Me.txtAbNr.Enabled = True
            Me.txtAbPLZ.Text = ""
            Me.txtAbPLZ.Enabled = True
            Me.txtAbOrt.Text = ""
            Me.txtAbOrt.Enabled = True
            Me.txtAbAnsprechpartner.Text = ""
            'Me.txtAbAnsprechpartner.Enabled = True
            Me.txtAbTelefon1.Text = ""
            'Me.txtAbTelefon1.Enabled = True
            Me.txtAbTelefon2.Text = ""
            'Me.txtAbTelefon2.Enabled = True
            Me.lblAbKundennummer.Text = ""
        Else
            Dim row As DataSets.AddressDataSet.ADDRESSERow = CType(CType(Session(drpAbholung.ID), DataTable).Select("ID='" + drpAbholung.SelectedItem.Value + "'")(0), DataSets.AddressDataSet.ADDRESSERow)
            Me.txtAbName.Text = row.NAME
            Me.txtAbName.Enabled = False
            Me.txtAbStrasse.Text = row.STRASSE
            Me.txtAbStrasse.Enabled = False
            Me.txtAbNr.Text = row.HAUSNUMMER
            Me.txtAbNr.Enabled = False
            Me.txtAbPLZ.Text = row.PLZ
            Me.txtAbPLZ.Enabled = False
            Me.txtAbOrt.Text = row.ORT
            Me.txtAbOrt.Enabled = False
            Me.txtAbAnsprechpartner.Text = row.NAME2
            'Me.txtAbAnsprechpartner.Enabled = False
            Me.txtAbTelefon1.Text = row.TELEFON1
            'Me.txtAbTelefon1.Enabled = False
            Me.txtAbTelefon2.Text = row.TELEFON2
            'Me.txtAbTelefon2.Enabled = False
            Me.lblAbKundennummer.Text = row.KUNDENNUMMER
        End If

    End Sub

    Private Sub drpAnlieferung_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles drpAnlieferung.SelectedIndexChanged

        Me.chkUbernahmeLeasingnehmerAnlieferung.Checked = False
        If drpAnlieferung.SelectedItem.Value = "" OrElse drpAnlieferung.SelectedItem.Value = "0" Then
            Me.txtAnName.Text = ""
            Me.txtAnName.Enabled = True
            Me.txtAnStrasse.Text = ""
            Me.txtAnStrasse.Enabled = True
            Me.txtAnNr.Text = ""
            Me.txtAnNr.Enabled = True
            Me.txtAnPLZ.Text = ""
            Me.txtAnPLZ.Enabled = True
            Me.txtAnOrt.Text = ""
            Me.txtAnOrt.Enabled = True
            Me.txtAnAnsprechpartner.Text = ""
            'Me.txtAnAnsprechpartner.Enabled = True
            Me.txtAnTelefon1.Text = ""
            'Me.txtAnTelefon1.Enabled = True
            Me.txtAnTelefon2.Text = ""
            'Me.txtAnTelefon2.Enabled = True
            Me.lblAnKundennummer.Text = ""
        Else
            Dim row As DataSets.AddressDataSet.ADDRESSERow = CType(CType(Session(drpAnlieferung.ID), DataTable).Select("ID='" + drpAnlieferung.SelectedItem.Value + "'")(0), DataSets.AddressDataSet.ADDRESSERow)
            Me.txtAnName.Text = row.NAME
            Me.txtAnName.Enabled = False
            Me.txtAnStrasse.Text = row.STRASSE
            Me.txtAnStrasse.Enabled = False
            Me.txtAnNr.Text = row.HAUSNUMMER
            Me.txtAnNr.Enabled = False
            Me.txtAnPLZ.Text = row.PLZ
            Me.txtAnPLZ.Enabled = False
            Me.txtAnOrt.Text = row.ORT
            Me.txtAnOrt.Enabled = False
            Me.txtAnAnsprechpartner.Text = row.NAME2
            'Me.txtAnAnsprechpartner.Enabled = False
            Me.txtAnTelefon1.Text = row.TELEFON1
            'Me.txtAnTelefon1.Enabled = False
            Me.txtAnTelefon2.Text = row.TELEFON2
            'Me.txtAnTelefon2.Enabled = False
            Me.lblAnKundennummer.Text = row.KUNDENNUMMER
        End If
    End Sub

    Private Sub cmdRight1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdRight1.Click
        Dim strErrAbholung As String = ""
        Dim strErrAnlieferung As String = ""
        Dim booErrAbholung As Boolean
        Dim booErrAnlieferung As Boolean


        If txtAbName.Text = "" Then
            booErrAbholung = True
            strErrAbholung = "Firma / Name <br>"
        End If
        If Me.txtAbStrasse.Text = "" Then
            booErrAbholung = True
            strErrAbholung = strErrAbholung & "Straße <br>"
        End If
        If Me.txtAbNr.Text = "" Then
            booErrAbholung = True
            strErrAbholung = strErrAbholung & "Nr. <br>"
        End If
        If txtAbPLZ.Text = "" Then
            booErrAbholung = True
            strErrAbholung = strErrAbholung & "PLZ <br>"
        ElseIf Len(txtAbPLZ.Text) <> 5 Then
            booErrAbholung = True
            strErrAbholung = strErrAbholung & "Bitte geben Sie die Postleitzahl fünfstellig ein. <br>"
        ElseIf IsNumeric(Me.txtAbPLZ.Text) = False Then
            booErrAbholung = True
            strErrAbholung = strErrAbholung & "Bitte geben Sie numerische Werte für die Postleitzahl ein. <br>"
        End If
        If Me.txtAbOrt.Text = "" Then
            booErrAbholung = True
            strErrAbholung = strErrAbholung & "Ort <br>"
        End If
        If txtAbAnsprechpartner.Text = "" Then
            booErrAbholung = True
            strErrAbholung = strErrAbholung & "Ansprechpartner <br>"
        End If
        If txtAbTelefon1.Text = "" Then
            booErrAbholung = True
            strErrAbholung = strErrAbholung & "Tel. <br>"
        End If

        If txtAnName.Text = "" Then
            booErrAnlieferung = True
            strErrAnlieferung = "Firma / Name <br>"
        End If
        If Me.txtAnStrasse.Text = "" Then
            booErrAnlieferung = True
            strErrAnlieferung = strErrAnlieferung & "Straße <br>"
        End If
        If Me.txtAnNr.Text = "" Then
            booErrAnlieferung = True
            strErrAnlieferung = strErrAnlieferung & "Nr. <br>"
        End If
        If txtAnPLZ.Text = "" Then
            booErrAnlieferung = True
            strErrAnlieferung = strErrAbholung & "PLZ <br>"
        ElseIf Len(txtAnPLZ.Text) <> 5 Then
            booErrAnlieferung = True
            strErrAnlieferung = strErrAnlieferung & "Bitte geben Sie die Postleitzahl fünfstellig ein. <br>"
        ElseIf IsNumeric(Me.txtAnPLZ.Text) = False Then
            booErrAnlieferung = True
            strErrAnlieferung = strErrAnlieferung & "Bitte geben Sie numerische Werte für die Postleitzahl ein. <br>"
        End If
        If Me.txtAnOrt.Text = "" Then
            booErrAnlieferung = True
            strErrAnlieferung = strErrAnlieferung & "Ort <br>"
        End If

        If txtAnAnsprechpartner.Text = "" Then
            booErrAnlieferung = True
            strErrAnlieferung = strErrAnlieferung & "Ansprechpartner <br>"
        End If

        If txtAnTelefon1.Text = "" Then
            booErrAnlieferung = True
            strErrAnlieferung = strErrAnlieferung & "Tel. <br>"
        End If

        If booErrAbholung = True Or booErrAnlieferung = True Then

            strError = "Bitte füllen Sie alle Pflichtfelder korrekt aus. <br>"

            If booErrAbholung = True Then
                strError = strError & "Abholung:  <br>" & strErrAbholung & " <br>"
            End If

            If booErrAnlieferung = True Then
                strError = strError & "Anlieferung:  <br>" & strErrAnlieferung & " <br>"
            End If

            lblError.Text = strError

        Else
            'Daten aus der Seite in die Properties der Klasse eintragen
            With clsUeberf

                'Abholadresse
                .AbName = Me.txtAbName.Text
                .AbStrasse = Me.txtAbStrasse.Text
                .AbNr = Me.txtAbNr.Text
                .AbPlz = Me.txtAbPLZ.Text
                .AbOrt = Me.txtAbOrt.Text
                .AbAnsprechpartner = Me.txtAbAnsprechpartner.Text
                .AbTelefon1 = Me.txtAbTelefon1.Text
                .AbTelefon2 = Me.txtAbTelefon2.Text
                .AbKundennummer = Me.lblAbKundennummer.Text

                'Anlieferadresse
                .AnName = Me.txtAnName.Text
                .AnStrasse = Me.txtAnStrasse.Text
                .AnNr = Me.txtAnNr.Text
                .AnPlz = Me.txtAnPLZ.Text
                .AnOrt = Me.txtAnOrt.Text
                .AnAnsprechpartner = Me.txtAnAnsprechpartner.Text
                .AnTelefon1 = Me.txtAnTelefon1.Text
                .AnTelefon2 = Me.txtAnTelefon2.Text
                .AnKundennummer = Me.lblAnKundennummer.Text

                If chkUbernahmeLeasingnehmerAbholung.Checked Then
                    .AdressStatusAbholung = AdressStatus.KopieVonLeasingnehmer
                ElseIf Not txtAbName.Enabled OrElse txtAbName.ReadOnly Then
                    .AdressStatusAbholung = AdressStatus.Gesperrt
                Else
                    .AdressStatusAbholung = AdressStatus.Frei
                End If

                If chkUbernahmeLeasingnehmerAnlieferung.Checked Then
                    .AdressStatusAnlieferung = AdressStatus.KopieVonLeasingnehmer
                ElseIf Not txtAnName.Enabled OrElse txtAnName.ReadOnly Then
                    .AdressStatusAnlieferung = AdressStatus.Gesperrt
                Else
                    .AdressStatusAnlieferung = AdressStatus.Frei
                End If

            End With

            clsUeberf = Nothing

            Response.Redirect("Ueberf02.aspx?AppID=" & Session("AppID").ToString)
        End If

    End Sub

    '-----------
    'Zum vorherigen Schritt zurückgehen
    '-----------
    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdBack.Click

        'Beaufragungsart "Reine Überführung" oder "Zulassung und Überführung"
        If clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.ReineUeberfuehrung Then
            Response.Redirect("Ueberf05.aspx?AppID=" & Session("AppID").ToString)
        ElseIf clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.ZulassungUndUeberfuehrung Then
            Response.Redirect("../Applications/AppARVAL/Forms/ChangeZulUe02.aspx?AppID=" & Session("AppID").ToString)
        ElseIf clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.ZulassungUndUeberfuehrungKCL Then
            If clsUeberf.AnName <> "" Then
                clsUeberf.Modus = 1
            End If
            Response.Redirect("Zul01.aspx?AppID=" & Session("AppID").ToString)
        ElseIf clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.UeberfuehrungKCL Then
            Response.Redirect("UeberfZulStart.aspx?AppID=" & Session("AppID").ToString)
        End If

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
#End Region
End Class


' ************************************************
' $History: Ueberf01.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 16.04.08   Time: 11:53
' Updated in $/CKAG/Applications/AppUeberf/Forms
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 16:06
' Created in $/CKAG/Applications/AppUeberf/Forms
' 
' *****************  Version 7  *****************
' User: Uha          Date: 2.07.07    Time: 10:05
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 6  *****************
' User: Uha          Date: 22.05.07   Time: 10:48
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 5  *****************
' User: Uha          Date: 21.05.07   Time: 11:46
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Änderungen im Vergleich zur Startapplikation zum Stand 11.05.2007
' 
' *****************  Version 4  *****************
' User: Uha          Date: 3.05.07    Time: 15:16
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Änderungen aus StartApplication vom 02.05.2007 Mittags übernommen
' 
' ************************************************