Option Explicit On

Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Imports AppUeberf.Ueberf_01
Imports AppUeberf.Helper

Public Class UeberfZulStart
    Inherits System.Web.UI.Page

    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents chkZul As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkUeberf As System.Web.UI.WebControls.CheckBox
    Protected WithEvents ucStyles As Styles
    Protected WithEvents btnConfirm As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdRight1 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents Label19 As System.Web.UI.WebControls.Label
    Protected WithEvents txtLeasingnehmerPLZ As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label15 As System.Web.UI.WebControls.Label
    Protected WithEvents Label13 As System.Web.UI.WebControls.Label
    Protected WithEvents txtLeasingnehmer As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtRef As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents txtLeasingnehmerOrt As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblFahrzeugdaten As System.Web.UI.WebControls.Label
    Protected WithEvents txtHerstTyp As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents txtVin As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents drpLeasingnehmer As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents Label6 As System.Web.UI.WebControls.Label
    Protected WithEvents txtLeasingnehmerStrasse As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtLeasingnehmerHausnr As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblLeasingnehmerKundennummer As System.Web.UI.WebControls.Label
    Protected WithEvents lblLeasingnehmerAnsprechpartner As System.Web.UI.WebControls.Label
    Protected WithEvents lblLeasingnehmerTelefon1 As System.Web.UI.WebControls.Label
    Protected WithEvents lblLeasingnehmerTelefon2 As System.Web.UI.WebControls.Label
    Protected WithEvents ctrlAddressSearchInput As Controls.AddressSearchInputControl

#Region " Vom Web Form Designer generierter Code "

    'Dieser Aufruf ist für den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: Diese Methode ist für den Web Form-Designer erforderlich
        'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
        InitializeComponent()

        ctrlAddressSearchInput.ResultDropdownList = drpLeasingnehmer
    End Sub

#End Region

    Private m_clsUeberfuehrungsdaten As Ueberf_01
    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App

#Region "Events"

    '-------------
    'Laden der Seite
    '-Initialisieren der Controls
    '-------------
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Session("ShowLink") = "False"
            m_User = GetUser(Me)
            ucHeader.InitUser(m_User)
            FormAuth(Me, m_User)

            GetAppIDFromQueryString(Me)

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)


            '------
            'Sicherstellen, dass ein Überführungsobjekt existiert und in der Session gespeichert wird
            '------
            If Session(CONST_SESSION_UEBERFUEHRUNG) Is Nothing Then
                m_clsUeberfuehrungsdaten = New Ueberf_01(m_User, m_App, "")
                Session(CONST_SESSION_UEBERFUEHRUNG) = m_clsUeberfuehrungsdaten
            Else
                If Request.UrlReferrer.ToString.IndexOf("Selection.aspx") > 0 Then
                    m_clsUeberfuehrungsdaten = Nothing
                    Session(CONST_SESSION_UEBERFUEHRUNG) = Nothing
                    m_clsUeberfuehrungsdaten = New Ueberf_01(m_User, m_App, "")
                    Session(CONST_SESSION_UEBERFUEHRUNG) = m_clsUeberfuehrungsdaten
                Else
                    m_clsUeberfuehrungsdaten = CType(Session(CONST_SESSION_UEBERFUEHRUNG), Ueberf_01)
                End If
            End If


            'Erstmaliges Laden der Seite
            If Not IsPostBack Then
                'Falls schon Daten im Überführungsobjekt vorhanden sind, dann diese wieder füllen
                FillControls(m_clsUeberfuehrungsdaten)
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try


    End Sub

    '-------------
    'Weiter-Knopf drücken
    '-Übernehmen der Daten aus Controls
    '-Weiterleiten auf entsprechende Folgeseite (abh. von Zulassung oder Überführung)
    '-------------
    Private Sub cmdRight1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdRight1.Click

        Try

            'Sicherstellen, dass Session-Objekt eingelesen wurde
            If m_clsUeberfuehrungsdaten Is Nothing Then
                m_clsUeberfuehrungsdaten = CType(Session(CONST_SESSION_UEBERFUEHRUNG), Ueberf_01)
            End If

            '------
            'Werte aus Controls übernehmen
            '------
            SetData(m_clsUeberfuehrungsdaten)


            'Überführungs-Objekt in Session speichern, um auf Folgeseiten wieder darauf zurückzugreifen
            Session(CONST_SESSION_UEBERFUEHRUNG) = m_clsUeberfuehrungsdaten

            'Weiterleiten auf entsprechende Seite
            If m_clsUeberfuehrungsdaten.Beauftragung = Beauftragungsart.UeberfuehrungKCL Then
                Response.Redirect("Ueberf01.aspx?AppID=" & Session("AppID").ToString)
            Else
                Response.Redirect("Zul01.aspx?AppID=" & Session("AppID").ToString)
            End If

        Catch mex As Exceptions.MandatoryDataMissingException
            lblError.Text = mex.Message
        Catch ex As Exception
            lblError.Text = "Fehler beim Speichern der Daten: " & ex.Message
        End Try

    End Sub

    '------
    'Leasingnehmer wird ausgewählt
    '------
    Private Sub drpLeasingnehmer_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles drpLeasingnehmer.SelectedIndexChanged
        If drpLeasingnehmer.Items.Count = 0 OrElse drpLeasingnehmer.SelectedItem.Value = "0" Then
            txtLeasingnehmer.Text = ""
            txtLeasingnehmer.Enabled = True
            txtLeasingnehmerPLZ.Text = ""
            txtLeasingnehmerPLZ.Enabled = True
            txtLeasingnehmerOrt.Text = ""
            txtLeasingnehmerOrt.Enabled = True
            txtLeasingnehmerStrasse.Text = ""
            txtLeasingnehmerStrasse.Enabled = True
            txtLeasingnehmerHausnr.Text = ""
            txtLeasingnehmerHausnr.Enabled = True
            lblLeasingnehmerKundennummer.Text = ""
            lblLeasingnehmerAnsprechpartner.Text = ""
            lblLeasingnehmerTelefon1.Text = ""
            lblLeasingnehmerTelefon2.Text = ""
        Else
            Dim tbl As DataSets.AddressDataSet.ADDRESSEDataTable = Session(drpLeasingnehmer.ID)
            Dim row As DataSets.AddressDataSet.ADDRESSERow = CType(tbl.Select("ID='" + drpLeasingnehmer.SelectedItem.Value + "'")(0), DataSets.AddressDataSet.ADDRESSERow)

            txtLeasingnehmer.Text = row.NAME
            txtLeasingnehmer.Enabled = False
            txtLeasingnehmerPLZ.Text = row.PLZ
            txtLeasingnehmerPLZ.Enabled = False
            txtLeasingnehmerOrt.Text = row.ORT
            txtLeasingnehmerOrt.Enabled = False
            txtLeasingnehmerStrasse.Text = row.STRASSE
            txtLeasingnehmerStrasse.Enabled = False
            txtLeasingnehmerHausnr.Text = row.HAUSNUMMER
            txtLeasingnehmerHausnr.Enabled = False
            lblLeasingnehmerKundennummer.Text = row.KUNDENNUMMER
            lblLeasingnehmerAnsprechpartner.Text = row.NAME2
            lblLeasingnehmerTelefon1.Text = row.TELEFON1
            lblLeasingnehmerTelefon2.Text = row.TELEFON2

        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
#End Region

#Region "Methoden"

    '-------
    'Schreibt die Daten aus dem Überführungsobjekt in die Controls
    '-------
    Private Sub FillControls(ByVal daten As Ueberf_01)

        With daten

            'Referenznummer
            txtRef.Text = .Ref


            txtLeasingnehmer.Text = .Leasingnehmer

            'Ggf Schreibschutz
            If Not .istLeasingnehmerGesperrt Then
                txtLeasingnehmer.Enabled = True
                txtLeasingnehmerPLZ.Enabled = True
                txtLeasingnehmerOrt.Enabled = True
                txtLeasingnehmerStrasse.Enabled = True
                txtLeasingnehmerHausnr.Enabled = True
            Else
                txtLeasingnehmer.Enabled = False
                txtLeasingnehmerPLZ.Enabled = False
                txtLeasingnehmerOrt.Enabled = False
                txtLeasingnehmerStrasse.Enabled = False
                txtLeasingnehmerHausnr.Enabled = False
            End If

            txtLeasingnehmerPLZ.Text = .LeasingnehmerPLZ
            txtLeasingnehmerOrt.Text = .LeasingnehmerOrt
            lblLeasingnehmerTelefon1.Text = .LeasingnehmerTelefon1
            lblLeasingnehmerTelefon2.Text = .LeasingnehmerTelefon2
            lblLeasingnehmerAnsprechpartner.Text = .LeasingnehmerAnsprechpartner
            txtLeasingnehmerStrasse.Text = .LeasingnehmerStrasse
            txtLeasingnehmerHausnr.Text = .LeasingnehmerHausnummer
            lblLeasingnehmerKundennummer.Text = .LeasingnehmerKundennummer

            'Fahrzeugdaten
            txtHerstTyp.Text = .Herst
            txtVin.Text = .Vin

            '----
            'Zulassung und Überführung verwenden
            '----
            If .Beauftragung = Beauftragungsart.ZulassungUndUeberfuehrungKCL Then
                'Zulassung und Überführung 
                chkZul.Checked = True
                chkUeberf.Checked = True

            ElseIf .Beauftragung = Beauftragungsart.ZulassungKCL Then
                'Zuassung
                chkZul.Checked = True
                chkUeberf.Checked = False

            ElseIf .Beauftragung = Beauftragungsart.UeberfuehrungKCL Then
                'Überführung

                chkZul.Checked = False
                chkUeberf.Checked = True

            End If


        End With

    End Sub

    '-------
    'Liest die Daten aus den Controls und schreibt sie in das Überführungsobjekt
    '-------
    Private Sub SetData(ByVal daten As Ueberf_01)

        Dim error_msg As New System.Text.StringBuilder()
        Dim strSonder As String
        With daten

            'Referenznummer
            If ControlHelper.CheckMandatoryField(txtRef, "Bitte geben Sie eine Referenznummer an.", error_msg) Then
                .Ref = txtRef.Text
            End If
            If txtRef.Text.Length > 0 Then
                strSonder = Proof_SpecialChar(txtRef.Text.Trim)
                If strSonder.Trim.Length > 0 Then
                    error_msg.Append("Bitte geben Sie die Referenz-Nr. ohne Sonderzeichen -> " & strSonder & " <- ein.  <br>")
                End If
            End If
            '---------
            'Leasingehmerdaten 
            '---------
            .istLeasingnehmerGesperrt = Not txtLeasingnehmer.Enabled OrElse txtLeasingnehmer.ReadOnly

            'Kundennummer
            .LeasingnehmerKundennummer = lblLeasingnehmerKundennummer.Text

            'Name
            If ControlHelper.CheckMandatoryField(txtLeasingnehmer, "Bitte geben Sie den Namen des Leasingnehmers an.", error_msg) Then
                .Leasingnehmer = txtLeasingnehmer.Text
            End If

            'PLZ
            If ControlHelper.CheckMandatoryField(txtLeasingnehmerPLZ, "Bitte geben Sie die Postleitzahl des Leasingnehmers an.", error_msg) _
            AndAlso ControlHelper.CheckPostcode(txtLeasingnehmerPLZ, error_msg) Then
                .LeasingnehmerPLZ = txtLeasingnehmerPLZ.Text
            End If

            'Ort
            If ControlHelper.CheckMandatoryField(txtLeasingnehmerOrt, "Bitte geben Sie den Ort des Leasingnehmers an.", error_msg) Then
                .LeasingnehmerOrt = txtLeasingnehmerOrt.Text
            End If

            'Strasse
            If ControlHelper.CheckMandatoryField(txtLeasingnehmerStrasse, "Bitte geben Sie die Straße des Leasingnehmers an.", error_msg) Then
                .LeasingnehmerStrasse = txtLeasingnehmerStrasse.Text
            End If

            If ControlHelper.CheckMandatoryField(txtLeasingnehmerHausnr, "Bitte geben Sie die Hausnummer des Leasingnehmers an.", error_msg) Then
                .LeasingnehmerHausnummer = txtLeasingnehmerHausnr.Text
            End If


            'Ansprechpartner
            .LeasingnehmerAnsprechpartner = lblLeasingnehmerAnsprechpartner.Text

            'Telefon 1 u. 2
            .LeasingnehmerTelefon1 = lblLeasingnehmerTelefon1.Text
            .LeasingnehmerTelefon2 = lblLeasingnehmerTelefon2.Text

            '----
            'Fahrzeugdaten
            '----
            'Fahrzeugtyp
            If ControlHelper.CheckMandatoryField(txtHerstTyp, "Bitte geben Sie einen Fahrzeugtyp an.", error_msg) Then
                .Herst = txtHerstTyp.Text
            End If

            'Fahrgestellnummer
            .Vin = txtVin.Text

            '----
            'Checkboxen Zulassung/Beauftragung
            '----
            If chkZul.Checked And chkUeberf.Checked Then
                'Zulassung und Überführung
                .Beauftragung = Beauftragungsart.ZulassungUndUeberfuehrungKCL

            ElseIf chkZul.Checked And Not chkUeberf.Checked Then
                'Zuassung beauftragen
                .Beauftragung = Beauftragungsart.ZulassungKCL

            ElseIf chkZul.Checked = False And chkUeberf.Checked = True Then
                'Überführung beauftragen
                .Beauftragung = Beauftragungsart.UeberfuehrungKCL

            Else
                error_msg.Append("Bitte wählen Sie eine Beauftragungsart aus.<br>")
            End If

        End With

        '----
        'Überprüfen, ob ein Fehler aufgetreten ist
        '----
        If error_msg.Length > 0 Then
            Throw New Exceptions.MandatoryDataMissingException(error_msg.ToString())
        End If

    End Sub

#End Region

End Class

' ************************************************
' $History: UeberfZulStart.aspx.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 16:06
' Created in $/CKAG/Applications/AppUeberf/Forms
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 9.01.08    Time: 16:42
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' ITA: 1604
' 
' *****************  Version 8  *****************
' User: Uha          Date: 2.07.07    Time: 10:05
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 7  *****************
' User: Uha          Date: 22.05.07   Time: 10:48
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 6  *****************
' User: Uha          Date: 21.05.07   Time: 11:47
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Änderungen im Vergleich zur Startapplikation zum Stand 11.05.2007
' 
' *****************  Version 5  *****************
' User: Uha          Date: 3.05.07    Time: 15:16
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Änderungen aus StartApplication vom 02.05.2007 Mittags übernommen
' 
' ************************************************