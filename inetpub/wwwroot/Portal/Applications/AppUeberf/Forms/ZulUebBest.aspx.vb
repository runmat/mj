Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Imports AppUeberf.Ueberf_01
Imports AppUeberf.Helper
Imports AppUeberf.Controls
Imports AppUeberf.Controls.ProgressControl

Public Class ZulUebBest
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private dv As DataView
    Private strError As String
    Private clsUeberf As Ueberf_01
    Private objSuche As Report_99

    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents Label6 As System.Web.UI.WebControls.Label
    Protected WithEvents lblAbName As System.Web.UI.WebControls.Label
    Protected WithEvents Label7 As System.Web.UI.WebControls.Label
    Protected WithEvents lblAbStrasse As System.Web.UI.WebControls.Label
    Protected WithEvents Label8 As System.Web.UI.WebControls.Label
    Protected WithEvents lblAbOrt As System.Web.UI.WebControls.Label
    Protected WithEvents Label9 As System.Web.UI.WebControls.Label
    Protected WithEvents lblAbAnsprechpartner As System.Web.UI.WebControls.Label
    Protected WithEvents Label16 As System.Web.UI.WebControls.Label
    Protected WithEvents lblAbTelefon As System.Web.UI.WebControls.Label
    Protected WithEvents lblFahrzeugdaten As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents lblHerst As System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents lblKennzeichen As System.Web.UI.WebControls.Label
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents lblVin As System.Web.UI.WebControls.Label
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents lblRef As System.Web.UI.WebControls.Label
    Protected WithEvents Label10 As System.Web.UI.WebControls.Label
    Protected WithEvents Label14 As System.Web.UI.WebControls.Label
    Protected WithEvents lblDatumUeberf As System.Web.UI.WebControls.Label
    Protected WithEvents Label21 As System.Web.UI.WebControls.Label
    Protected WithEvents lblTanken As System.Web.UI.WebControls.Label
    Protected WithEvents Label25 As System.Web.UI.WebControls.Label
    Protected WithEvents lblRotKenn As System.Web.UI.WebControls.Label
    Protected WithEvents lbl1Anschluss As System.Web.UI.WebControls.Label
    Protected WithEvents lbl1FzgDaten As System.Web.UI.WebControls.Label
    Protected WithEvents lbl1Name As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2ReName As System.Web.UI.WebControls.Label
    Protected WithEvents lbl1Herst As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2ReHerst As System.Web.UI.WebControls.Label
    Protected WithEvents lbl1StrNr As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2ReStrasse As System.Web.UI.WebControls.Label
    Protected WithEvents lbl1Kenn As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2ReKenn As System.Web.UI.WebControls.Label
    Protected WithEvents lbl1PLZOrt As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2RePlzOrt As System.Web.UI.WebControls.Label
    Protected WithEvents lbl1Ansprech As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2ReAnsprech As System.Web.UI.WebControls.Label
    Protected WithEvents lbl1Vin As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2ReVin As System.Web.UI.WebControls.Label
    Protected WithEvents lbl1Telefon As System.Web.UI.WebControls.Label
    Protected WithEvents lbl1Ref As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2ReRef As System.Web.UI.WebControls.Label
    Protected WithEvents lbl1FzgZugelassen As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2ReZugelassen As System.Web.UI.WebControls.Label
    Protected WithEvents lbl1Bereifung As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2ReBereif As System.Web.UI.WebControls.Label
    Protected WithEvents cmdBack As System.Web.UI.WebControls.ImageButton
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    Protected WithEvents cmdPrint As System.Web.UI.WebControls.Button
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblHaltername As System.Web.UI.WebControls.Label
    Protected WithEvents Label29 As System.Web.UI.WebControls.Label
    Protected WithEvents lblZulDatum As System.Web.UI.WebControls.Label
    Protected WithEvents Label33 As System.Web.UI.WebControls.Label
    Protected WithEvents Label34 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2ZulMail As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2ZulTel As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2ZulPlzOrt As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2ZulName2 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2ZulStrasse As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2ZulName1 As System.Web.UI.WebControls.Label
    Protected WithEvents lblZulassungsdienst As System.Web.UI.WebControls.Label
    Protected WithEvents Table7 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Label49 As System.Web.UI.WebControls.Label
    Protected WithEvents Label48 As System.Web.UI.WebControls.Label
    Protected WithEvents Label47 As System.Web.UI.WebControls.Label
    Protected WithEvents Label46 As System.Web.UI.WebControls.Label
    Protected WithEvents Label45 As System.Web.UI.WebControls.Label
    Protected WithEvents Label44 As System.Web.UI.WebControls.Label
    Protected WithEvents Label43 As System.Web.UI.WebControls.Label
    Protected WithEvents Label42 As System.Web.UI.WebControls.Label
    Protected WithEvents Label40 As System.Web.UI.WebControls.Label
    Protected WithEvents Label41 As System.Web.UI.WebControls.Label
    Protected WithEvents Label09 As System.Web.UI.WebControls.Label
    Protected WithEvents Label08 As System.Web.UI.WebControls.Label
    Protected WithEvents Label07 As System.Web.UI.WebControls.Label
    Protected WithEvents Label06 As System.Web.UI.WebControls.Label
    Protected WithEvents Label05 As System.Web.UI.WebControls.Label
    Protected WithEvents Label04 As System.Web.UI.WebControls.Label
    Protected WithEvents Label03 As System.Web.UI.WebControls.Label
    Protected WithEvents Label02 As System.Web.UI.WebControls.Label
    Protected WithEvents Label00 As System.Web.UI.WebControls.Label
    Protected WithEvents Label01 As System.Web.UI.WebControls.Label
    Protected WithEvents Table6 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Table8 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Table9 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Table5 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblStva As System.Web.UI.WebControls.Label
    Protected WithEvents lblUnterlagen As System.Web.UI.WebControls.Label
    Protected WithEvents lblLeasingnehmer As System.Web.UI.WebControls.Label
    Protected WithEvents Label31 As System.Web.UI.WebControls.Label
    Protected WithEvents lblVersicherungsnehmer As System.Web.UI.WebControls.Label
    Protected WithEvents Label32 As System.Web.UI.WebControls.Label
    Protected WithEvents lblKennzeichen1 As System.Web.UI.WebControls.Label
    Protected WithEvents Label27 As System.Web.UI.WebControls.Label
    Protected WithEvents lblKennzeichen2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label28 As System.Web.UI.WebControls.Label
    Protected WithEvents lblKennzeichen3 As System.Web.UI.WebControls.Label
    Protected WithEvents Label30 As System.Web.UI.WebControls.Label
    Protected WithEvents lblVersicherer As System.Web.UI.WebControls.Label
    Protected WithEvents Label35 As System.Web.UI.WebControls.Label
    Protected WithEvents lblKfzSteuer As System.Web.UI.WebControls.Label
    Protected WithEvents Label36 As System.Web.UI.WebControls.Label
    Protected WithEvents Label37 As System.Web.UI.WebControls.Label
    Protected WithEvents Label38 As System.Web.UI.WebControls.Label
    Protected WithEvents Label24 As System.Web.UI.WebControls.Label
    Protected WithEvents lblBemerkung As System.Web.UI.WebControls.Label
    Protected WithEvents lblAbTelefon2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label50 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2ReTelefon1 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2ReTelefon2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label52 As System.Web.UI.WebControls.Label
    Protected WithEvents Label23 As System.Web.UI.WebControls.Label
    Protected WithEvents lblEinw As System.Web.UI.WebControls.Label
    Protected WithEvents Label22 As System.Web.UI.WebControls.Label
    Protected WithEvents lblWW As System.Web.UI.WebControls.Label
    Protected WithEvents Label12 As System.Web.UI.WebControls.Label
    Protected WithEvents lblBereifung As System.Web.UI.WebControls.Label
    Protected WithEvents Label26 As System.Web.UI.WebControls.Label
    Protected WithEvents lblFahrzeugwert As System.Web.UI.WebControls.Label
    Protected WithEvents Label11 As System.Web.UI.WebControls.Label
    Protected WithEvents lblZugelassen As System.Web.UI.WebControls.Label
    Protected WithEvents Label51 As System.Web.UI.WebControls.Label
    Protected WithEvents lblAnTelefon2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label20 As System.Web.UI.WebControls.Label
    Protected WithEvents lblAnTelefon As System.Web.UI.WebControls.Label
    Protected WithEvents Label19 As System.Web.UI.WebControls.Label
    Protected WithEvents lblAnAnspechpartner As System.Web.UI.WebControls.Label
    Protected WithEvents Label18 As System.Web.UI.WebControls.Label
    Protected WithEvents lblAnOrt As System.Web.UI.WebControls.Label
    Protected WithEvents Label17 As System.Web.UI.WebControls.Label
    Protected WithEvents lblAnStrasse As System.Web.UI.WebControls.Label
    Protected WithEvents Label13 As System.Web.UI.WebControls.Label
    Protected WithEvents lblAnName As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblUeberfDatumFix As System.Web.UI.WebControls.Label
    Protected WithEvents lblWinterText As System.Web.UI.WebControls.Label
    Protected WithEvents Label39 As System.Web.UI.WebControls.Label
    Protected WithEvents pnlPlaceholder As System.Web.UI.WebControls.Panel
    Protected WithEvents lblBem As System.Web.UI.WebControls.Label
    Protected WithEvents lblReBemerkung As System.Web.UI.WebControls.Label
    Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    Protected WithEvents Label15 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Auftragsnr As System.Web.UI.WebControls.Label
    Protected WithEvents ProgressControl1 As AppUeberf.Controls.ProgressControl

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
        ' Hier Benutzercode zur Seiteninitialisierung einfügen
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

        If IsPostBack = False Then
            GetData()
        Else
            If clsUeberf Is Nothing Then
                clsUeberf = CType(Session(CONST_SESSION_UEBERFUEHRUNG), Ueberf_01)
            End If
        End If

        ProgressControl1.Fill(Source.ZulUebBest, clsUeberf)

    End Sub

    Private Sub GetData()

        If clsUeberf Is Nothing Then
            clsUeberf = CType(Session(CONST_SESSION_UEBERFUEHRUNG), Ueberf_01)
        End If

        With clsUeberf

            lblAbName.Text = .AbName
            lblAbStrasse.Text = .AbStrasse & " " & .AbNr
            lblAbOrt.Text = .AbPlz & " " & .AbOrt
            lblAbAnsprechpartner.Text = .AbAnsprechpartner
            lblAbTelefon.Text = .AbTelefon1
            lblAbTelefon2.Text = .AbTelefon2
            lblAnName.Text = .AnName
            lblAnStrasse.Text = .AnStrasse & " " & .AnNr
            lblAnOrt.Text = .AnPlz & " " & .AnOrt
            lblAnAnspechpartner.Text = .AnAnsprechpartner
            lblAnTelefon.Text = .AnTelefon1
            lblAnTelefon2.Text = .AnTelefon2

            lblHerst.Text = .Herst
            lblKennzeichen.Text = .Kenn1 & "-" & .Kenn2
            lblKennzeichen1.Text = .Wunschkennzeichen1
            lblKennzeichen2.Text = .Wunschkennzeichen2
            lblKennzeichen3.Text = .Wunschkennzeichen3


            lblVin.Text = .Vin
            lblRef.Text = .Ref
            lblBem.Text = .Bemerkung

            lblZugelassen.Text = .FzgZugelassen
            lblBereifung.Text = .SomWin

            lblDatumUeberf.Text = .DatumUeberf

            lblFahrzeugwert.Text = .FahrzeugwertTxt

            If .Tanken = True Then
                lblTanken.Text = "Ja"
            Else
                lblTanken.Text = "Nein"
            End If

            If .FzgEinweisung = True Then
                lblEinw.Text = "Ja"
            Else
                lblEinw.Text = "Nein"
            End If

            If .Waesche = True Then
                lblWW.Text = "Ja"
            Else
                lblWW.Text = "Nein"
            End If

            If .RotKenn = True Then
                lblRotKenn.Text = "Ja"
            Else
                lblRotKenn.Text = "Nein"
            End If

            lblHaltername.Text = .ZulHaltername

            lblZulDatum.Text = .Zulassungsdatum


            lblLeasingnehmer.Text = .Leasingnehmer
            lblVersicherungsnehmer.Text = .Versicherungsnehmer
            lblVersicherer.Text = .Versicherer
            lblKfzSteuer.Text = .KfzSteuer
            lblBemerkung.Text = .BemerkungLease

            If .FixDatumUeberfuehrung Then
                lblUeberfDatumFix.Text = "Fix"
            Else
                lblUeberfDatumFix.Text = ""
            End If
            If .WinterHandling Then
                lblWinterText.Text = .WinterText
            Else
                lblWinterText.Text = ""
            End If


            If Not .tblKreis Is Nothing Then

                If .tblKreis.Rows.Count > 0 Then

                    lblStva.Text = "Ermittelter Zulassungskreis: " & .tblKreis.Rows(0)("ZKFZKZ").ToString() & "/" & _
                                        .tblKreis.Rows(0)("ZKREIS").ToString()

                End If
            End If



            If .Anschluss = False Then
                Table5.Visible = False
            Else
                lbl2ReName.Text = .ReName
                lbl2ReStrasse.Text = .ReStrasse & " " & .ReNr
                lbl2RePlzOrt.Text = .RePlz & " " & .ReOrt
                lbl2ReAnsprech.Text = .ReAnsprechpartner
                lbl2ReTelefon1.Text = .ReTelefon1
                lbl2ReTelefon2.Text = .ReTelefon2
                lbl2ReHerst.Text = .ReHerst
                lbl2ReKenn.Text = .ReKenn1 & "-" & .ReKenn2
                lbl2ReVin.Text = .ReVin
                lbl2ReRef.Text = .ReRef
                lbl2ReZugelassen.Text = .ReFzgZugelassen
                lbl2ReBereif.Text = .ReSomWin
                lblReBemerkung.Text = .ReBemerkung
            End If

            Table7.Visible = False
            Table8.Visible = False
            Table9.Visible = False

        End With
    End Sub

    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdBack.Click

        If clsUeberf Is Nothing Then
            clsUeberf = CType(Session(CONST_SESSION_UEBERFUEHRUNG), Ueberf_01)
        End If

        If clsUeberf.Vbeln <> "" Then


            SetNothing()
            Response.Redirect("UeberfZulStart.aspx?AppID=" & Session("AppID").ToString)
        Else
            clsUeberf.Modus = 2

            Session("Ueberf") = clsUeberf
            If clsUeberf.Anschluss = False Then
                Response.Redirect("Ueberf02.aspx?AppID=" & Session("AppID").ToString)
            Else
                Response.Redirect("Ueberf03.aspx?AppID=" & Session("AppID").ToString)
            End If
        End If

    End Sub
    
    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim RetTable As DataTable
        Dim strErr As String = ""
        If clsUeberf Is Nothing Then
            clsUeberf = CType(Session(CONST_SESSION_UEBERFUEHRUNG), Ueberf_01)
        End If

        Dim Mail As System.Net.Mail.MailMessage
        RetTable = clsUeberf.Save()

        cmdSave.Visible = False

        If clsUeberf.Vbeln = "" Then

            strErr = "Die Überführung konnte wegen eines Fehlers nicht beauftragt werden! <br>"

            lblError.Text = strErr

        Else

            Dim strMessage As String
            DoSubmit()
            Beauftragt()

            strMessage = "Überführungsauftragsnummer: " & clsUeberf.Vbeln.TrimStart("0"c) & vbCrLf
            strMessage = strMessage & "Beauftragt von: " & m_User.UserName & vbCrLf
            strMessage = strMessage & "Kunde: " & m_User.KUNNR.TrimStart("0"c) & " " & m_User.CustomerName & vbCrLf
            strMessage = strMessage & "Leasingnehmer: " & clsUeberf.Leasingnehmer & vbCrLf
            strMessage = strMessage & "Referenz-Nr.: " & clsUeberf.Ref & vbCrLf
            strMessage = strMessage & "Haltername: " & clsUeberf.ZulHaltername & vbCrLf
            strMessage = strMessage & "Versicherungsnehmer: " & clsUeberf.Versicherungsnehmer & vbCrLf
            strMessage = strMessage & "Versicherer: " & clsUeberf.Versicherer & vbCrLf
            strMessage = strMessage & "KFZ-Steuer-Zahlung durch: " & clsUeberf.KfzSteuer & vbCrLf


            If Len(clsUeberf.Wunschkennzeichen1) > 0 Then
                strMessage = strMessage & "1. Wunschkennzeichen: " & clsUeberf.Wunschkennzeichen1 & vbCrLf
            End If

            If Len(clsUeberf.Wunschkennzeichen2) > 0 Then
                strMessage = strMessage & "2. Wunschkennzeichen: " & clsUeberf.Wunschkennzeichen2 & vbCrLf
            End If

            If Len(clsUeberf.Wunschkennzeichen3) > 0 Then
                strMessage = strMessage & "3. Wunschkennzeichen: " & clsUeberf.Wunschkennzeichen3 & vbCrLf
            End If


            strMessage = strMessage & "StvA: " & clsUeberf.Kenn1 & vbCrLf
            strMessage = strMessage & "Zulassungsdatum: " & clsUeberf.Zulassungsdatum & vbCrLf
            strMessage = strMessage & "Bemerkung: " & clsUeberf.BemerkungLease

            Try

                'Absenden
                Mail = New System.Net.Mail.MailMessage(ConfigurationManager.AppSettings("SmtpMailSender"), clsUeberf.VBMail, _
                                                        "Zulassung für Kunde: " & m_User.KUNNR.TrimStart("0"c) & " " & m_User.CustomerName, strMessage)
                Dim client As New System.Net.Mail.SmtpClient(ConfigurationManager.AppSettings("SmtpMailServer"))
                client.Send(Mail)

            Catch ex As Exception
                lblError.Text = "Fehler bei Versenden des Auftrags."
            End Try

            lblError.Text = "Ihr Auftrag wurde unter der Auftragsnummer " & clsUeberf.Vbeln.TrimStart("0"c) & " in unserem System erfasst!"
            lbl_Auftragsnr.Text = "Unsere Überführungsauftragsnr.: " & clsUeberf.Vbeln.TrimStart("0"c) & "  vom " & Today.ToShortDateString()
            lbl_Auftragsnr.Visible = True
            cmdPrint.Visible = True
        End If
    End Sub

    Private Sub DoSubmit()


        If clsUeberf Is Nothing Then
            clsUeberf = CType(Session(CONST_SESSION_UEBERFUEHRUNG), Ueberf_01)
        End If

        With clsUeberf

            'VKORG
            .getVKBuero(m_User.KUNNR)

            
            lbl2ZulName1.Text = .VBName1
            lbl2ZulName2.Text = .VBName2
            lbl2ZulStrasse.Text = .VBStrasse
            lbl2ZulPlzOrt.Text = .VBPlzOrt
            lbl2ZulTel.Text = .VBTel
            lbl2ZulMail.Text = .VBMail


        End With

        Try

            Dim objSuche As New Report_99(m_User, m_App, "")

            objSuche.PKennzeichen = clsUeberf.Kenn1
            objSuche.FILL(Session("AppID").ToString, Session.SessionID.ToString)

            Session("objSuche") = objSuche

            If Not objSuche.Status = 0 Then
                lblError.Text = "Fehler: " & objSuche.Message
            Else
                If objSuche.Result.Rows.Count = 0 Then
                    lblUnterlagen.Text = "Zu diesem STVA stehen uns zur Zeit leider noch keine Daten zur Verfügung."
                    lblUnterlagen.Visible = True
                    Table8.Visible = False
                    Table9.Visible = False
                Else
                    Table8.Visible = True
                    Table9.Visible = True
                    FillForm()
                End If
            End If

        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten."
        End Try
    End Sub

    Private Sub FillForm()
        objSuche = CType(Session("objSuche"), Report_99)

        Dim resultRow As DataRow
        Try
            resultRow = objSuche.Result.Rows(0)

            'Privat Zulassung
            Label00.Text = CType(resultRow("PZUL_BRIEF"), String)
            Label01.Text = CType(resultRow("PZUL_SCHEIN"), String)
            Label02.Text = CType(resultRow("PZUL_COC"), String)
            Label03.Text = CType(resultRow("PZUL_DECK"), String)
            Label04.Text = CType(resultRow("PZUL_VOLLM"), String)
            Label05.Text = CType(resultRow("PZUL_AUSW"), String)
            Label06.Text = CType(resultRow("PZUL_GEWERB"), String)
            Label07.Text = CType(resultRow("PZUL_HANDEL"), String)
            Label08.Text = CType(resultRow("PZUL_LAST"), String)
            Label09.Text = CType(resultRow("PZUL_BEM"), String)

            'Unternehmen Zulassung
            Label40.Text = CType(resultRow("UZUL_BRIEF"), String)
            Label41.Text = CType(resultRow("UZUL_SCHEIN"), String)
            Label42.Text = CType(resultRow("UZUL_COC"), String)
            Label43.Text = CType(resultRow("UZUL_DECK"), String)
            Label44.Text = CType(resultRow("UZUL_VOLLM"), String)
            Label45.Text = CType(resultRow("UZUL_AUSW"), String)
            Label46.Text = CType(resultRow("UZUL_GEWERB"), String)
            Label47.Text = CType(resultRow("UZUL_HANDEL"), String)
            Label48.Text = CType(resultRow("UZUL_LAST"), String)
            Label49.Text = CType(resultRow("UZUL_BEM"), String)

            If clsUeberf Is Nothing Then
                clsUeberf = CType(Session(CONST_SESSION_UEBERFUEHRUNG), Ueberf_01)
            End If

            If UCase(clsUeberf.VB3100) = "X" Then
                lblZulassungsdienst.Text = "Bitte senden Sie für die Zulassung folgende Unterlagen an:"
                lblUnterlagen.Text = "Erforderliche Unterlagen:"
            End If

        Catch ex As Exception
            lblError.Text = "Daten konnten nicht gelesen werden."
        End Try
    End Sub

    Private Sub Beauftragt()
        Table7.Visible = True
       
        lblZulassungsdienst.Visible = True
        lblUnterlagen.Visible = True
        lblStva.Visible = True

    End Sub
    
    Private Sub SetNothing()
        clsUeberf = Nothing
        Session("Ueberf") = Nothing
    End Sub

    Private Sub cmdPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPrint.Click
        Try
            Dim imageHt As New Hashtable()
            imageHt.Add("Logo", m_User.Customer.LogoImage)

            clsUeberf.getVKBueroFooter(m_User.KUNNR)

            Dim tblData As DataTable = Base.Kernel.Common.DataTableHelper.ObjectToDataTable(clsUeberf)
            tblData.Rows(0).BeginEdit()
            Dim dr As DataRow
            dr = tblData.Rows(0)
            dr("Vbeln") = clsUeberf.Vbeln.TrimStart("0"c)
            tblData.Rows(0).EndEdit()
            tblData.Columns.Add("Datum", GetType(String))
            tblData.Rows(0).BeginEdit()
            dr = tblData.Rows(0)
            dr("Datum") = Today.ToShortDateString
            tblData.Rows(0).EndEdit()

            Dim docFactory As New Base.Kernel.DocumentGeneration.WordDocumentFactory(tblData, imageHt)

            docFactory.CreateDocument(clsUeberf.Ref + "_ZulUeb", _
                                      Page, "\Applications\AppUeberf\Documents\ZulassungUndÜberführungLeasing.doc")

        Catch ex As Exception
            lblError.Text = "Fehler beim Erstellen des Ausdrucks: " + ex.Message
        End Try
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: ZulUebBest.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 4.06.08    Time: 16:42
' Updated in $/CKAG/Applications/AppUeberf/Forms
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 16.04.08   Time: 11:53
' Updated in $/CKAG/Applications/AppUeberf/Forms
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 16:06
' Created in $/CKAG/Applications/AppUeberf/Forms
' 
' *****************  Version 18  *****************
' User: Rudolpho     Date: 23.08.07   Time: 8:36
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 17  *****************
' User: Rudolpho     Date: 23.08.07   Time: 8:30
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 16  *****************
' User: Rudolpho     Date: 20.08.07   Time: 17:15
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' ITA: 1192, 1246
' 
' *****************  Version 15  *****************
' User: Rudolpho     Date: 27.07.07   Time: 10:13
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Dokumentvorlagen angepasst
' 
' *****************  Version 14  *****************
' User: Rudolpho     Date: 27.07.07   Time: 9:16
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 13  *****************
' User: Rudolpho     Date: 27.07.07   Time: 8:18
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Bugfixing drucken
' 
' *****************  Version 12  *****************
' User: Rudolpho     Date: 25.07.07   Time: 9:59
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
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