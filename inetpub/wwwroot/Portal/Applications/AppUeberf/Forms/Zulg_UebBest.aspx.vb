Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Zulg_UebBest
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private dv As DataView
    Private strError As String
    Private clsUeberf As UeberfgStandard_01
    Private objSuche As Report_99

    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblSchritt As System.Web.UI.WebControls.Label
    Protected WithEvents lblKundeName1 As System.Web.UI.WebControls.Label
    Protected WithEvents lblKundeStrasse As System.Web.UI.WebControls.Label
    Protected WithEvents lblKundeAnsprechpartner As System.Web.UI.WebControls.Label
    Protected WithEvents lblKundePlzOrt As System.Web.UI.WebControls.Label
    Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    Protected WithEvents Label15 As System.Web.UI.WebControls.Label
    Protected WithEvents Label6 As System.Web.UI.WebControls.Label
    Protected WithEvents lblAbName As System.Web.UI.WebControls.Label
    Protected WithEvents Label13 As System.Web.UI.WebControls.Label
    Protected WithEvents lblAnName As System.Web.UI.WebControls.Label
    Protected WithEvents Label7 As System.Web.UI.WebControls.Label
    Protected WithEvents lblAbStrasse As System.Web.UI.WebControls.Label
    Protected WithEvents Label17 As System.Web.UI.WebControls.Label
    Protected WithEvents lblAnStrasse As System.Web.UI.WebControls.Label
    Protected WithEvents Label8 As System.Web.UI.WebControls.Label
    Protected WithEvents lblAbOrt As System.Web.UI.WebControls.Label
    Protected WithEvents Label18 As System.Web.UI.WebControls.Label
    Protected WithEvents lblAnOrt As System.Web.UI.WebControls.Label
    Protected WithEvents Label9 As System.Web.UI.WebControls.Label
    Protected WithEvents lblAbAnsprechpartner As System.Web.UI.WebControls.Label
    Protected WithEvents Label19 As System.Web.UI.WebControls.Label
    Protected WithEvents lblAnAnspechpartner As System.Web.UI.WebControls.Label
    Protected WithEvents Label16 As System.Web.UI.WebControls.Label
    Protected WithEvents lblAbTelefon As System.Web.UI.WebControls.Label
    Protected WithEvents Label20 As System.Web.UI.WebControls.Label
    Protected WithEvents lblAnTelefon As System.Web.UI.WebControls.Label
    Protected WithEvents lblFahrzeugdaten As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents lblHerst As System.Web.UI.WebControls.Label
    Protected WithEvents Label11 As System.Web.UI.WebControls.Label
    Protected WithEvents lblZugelassen As System.Web.UI.WebControls.Label
    Protected WithEvents lblKennzeichen As System.Web.UI.WebControls.Label
    Protected WithEvents Label26 As System.Web.UI.WebControls.Label
    Protected WithEvents lblFahrzeugwert As System.Web.UI.WebControls.Label
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents lblVin As System.Web.UI.WebControls.Label
    Protected WithEvents Label12 As System.Web.UI.WebControls.Label
    Protected WithEvents lblBereifung As System.Web.UI.WebControls.Label
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents lblRef As System.Web.UI.WebControls.Label
    Protected WithEvents Label10 As System.Web.UI.WebControls.Label
    Protected WithEvents Label14 As System.Web.UI.WebControls.Label
    Protected WithEvents lblDatumUeberf As System.Web.UI.WebControls.Label
    Protected WithEvents Label22 As System.Web.UI.WebControls.Label
    Protected WithEvents lblWW As System.Web.UI.WebControls.Label
    Protected WithEvents Label21 As System.Web.UI.WebControls.Label
    Protected WithEvents lblTanken As System.Web.UI.WebControls.Label
    Protected WithEvents Label23 As System.Web.UI.WebControls.Label
    Protected WithEvents lblEinw As System.Web.UI.WebControls.Label
    Protected WithEvents Label25 As System.Web.UI.WebControls.Label
    Protected WithEvents lblRotKenn As System.Web.UI.WebControls.Label
    Protected WithEvents Label24 As System.Web.UI.WebControls.Label
    Protected WithEvents lblBem As System.Web.UI.WebControls.Label
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
    Protected WithEvents lbl2ReTelefon As System.Web.UI.WebControls.Label
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
    Protected WithEvents lblKennzeichen1 As System.Web.UI.WebControls.Label
    Protected WithEvents Label27 As System.Web.UI.WebControls.Label
    Protected WithEvents lblKennzeichen2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label28 As System.Web.UI.WebControls.Label
    Protected WithEvents lblKennzeichen3 As System.Web.UI.WebControls.Label
    Protected WithEvents Label30 As System.Web.UI.WebControls.Label
    Protected WithEvents lblStva As System.Web.UI.WebControls.Label
    Protected WithEvents lblUnterlagen As System.Web.UI.WebControls.Label
    Protected WithEvents Label31 As System.Web.UI.WebControls.Label
    Protected WithEvents Label32 As System.Web.UI.WebControls.Label
    Protected WithEvents Label36 As System.Web.UI.WebControls.Label
    Protected WithEvents lblRegName As System.Web.UI.WebControls.Label
    Protected WithEvents Label39 As System.Web.UI.WebControls.Label
    Protected WithEvents lblRechName As System.Web.UI.WebControls.Label
    Protected WithEvents Label37 As System.Web.UI.WebControls.Label
    Protected WithEvents lblRegStrasse As System.Web.UI.WebControls.Label
    Protected WithEvents Label35 As System.Web.UI.WebControls.Label
    Protected WithEvents lblRechStrasse As System.Web.UI.WebControls.Label
    Protected WithEvents Label38 As System.Web.UI.WebControls.Label
    Protected WithEvents lblRegOrt As System.Web.UI.WebControls.Label
    Protected WithEvents Label50 As System.Web.UI.WebControls.Label
    Protected WithEvents lblRechOrt As System.Web.UI.WebControls.Label
    Protected WithEvents cmdNewOrder As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdNewOrderHoldData As System.Web.UI.WebControls.LinkButton
    Protected WithEvents Label51 As System.Web.UI.WebControls.Label
    Protected WithEvents Label52 As System.Web.UI.WebControls.Label
    Protected WithEvents lblAbFax As System.Web.UI.WebControls.Label
    Protected WithEvents lblAnFax As System.Web.UI.WebControls.Label
    Protected WithEvents lblExpress As System.Web.UI.WebControls.Label
    Protected WithEvents Label53 As System.Web.UI.WebControls.Label
    Protected WithEvents lblAbTelefon2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label54 As System.Web.UI.WebControls.Label
    Protected WithEvents lblAnTelefon2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label55 As System.Web.UI.WebControls.Label
    Protected WithEvents lblFahrzeugklasse As System.Web.UI.WebControls.Label
    Protected WithEvents Label56 As System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label57 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2ReFahrzeugklasse As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2ReFax As System.Web.UI.WebControls.Label
    Protected WithEvents Label58 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Auftragsnr As System.Web.UI.WebControls.Label
    Protected WithEvents lblHinZulKCL As System.Web.UI.WebControls.Label
    Protected WithEvents Label59 As System.Web.UI.WebControls.Label
    Protected WithEvents lblAnZulKCL As System.Web.UI.WebControls.Label
    Protected WithEvents Label60 As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles

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
                clsUeberf = Session("Ueberf")
            End If
        End If

        If clsUeberf.Anschluss = False Then
            Me.lblSchritt.Text = "Schritt 3 von 3"
        Else
            Me.lblSchritt.Text = "Schritt 4 von 4"
        End If

    End Sub

    Private Sub GetData()

        If clsUeberf Is Nothing Then
            clsUeberf = Session("Ueberf")
        End If

        With clsUeberf
            dv = Session("DataViewRG")
            dv.RowFilter = "PARVW = 'RG' AND KUNNR = '" & .SelRegulierer & "'"
            .RegName = dv.Item(0)("NAME1") & " " & dv.Item(0)("NAME2")
            .RegStrasse = dv.Item(0)("STREET") & " " & dv.Item(0)("HOUSE_NUM1")
            .RegOrt = dv.Item(0)("POST_CODE1") & " " & dv.Item(0)("CITY1")
            Me.lblRegName.Text = .RegName
            Me.lblRegStrasse.Text = .RegStrasse
            Me.lblRegOrt.Text = .RegOrt
            dv.RowFilter = "PARVW = 'RE' AND KUNNR = '" & .SelRechnungsempf & "'"
            .RechName = dv.Item(0)("NAME1") & " " & dv.Item(0)("NAME2")
            .RechStrasse = dv.Item(0)("STREET") & " " & dv.Item(0)("HOUSE_NUM1")
            .RechOrt = dv.Item(0)("POST_CODE1") & " " & dv.Item(0)("CITY1")

            Me.lblRechName.Text = .RechName
            Me.lblRechStrasse.Text = .RechStrasse
            Me.lblRechOrt.Text = .RechOrt

            Me.lblKundeName1.Text = .KundeName
            Me.lblKundeStrasse.Text = .KundeStrasse
            Me.lblKundePlzOrt.Text = .KundeOrt
            Me.lblKundeAnsprechpartner.Text = .KundeAnsprechpartner
            Me.lblAbName.Text = .AbName
            Me.lblAbStrasse.Text = .AbStrasse & " " & .AbNr
            Me.lblAbOrt.Text = .AbPlz & " " & .AbOrt
            Me.lblAbAnsprechpartner.Text = .AbAnsprechpartner
            Me.lblAbTelefon.Text = .AbTelefon
            Me.lblAbTelefon2.Text = .AbTelefon2
            Me.lblAbFax.Text = .AbFax
            Me.lblAnName.Text = .AnName
            Me.lblAnStrasse.Text = .AnStrasse & " " & .AnNr
            Me.lblAnOrt.Text = .AnPlz & " " & .AnOrt
            Me.lblAnAnspechpartner.Text = .AnAnsprechpartner
            Me.lblAnTelefon.Text = .AnTelefon
            Me.lblAnTelefon2.Text = .AnTelefon2
            Me.lblAnFax.Text = .AnFax

            Me.lblHerst.Text = .Herst
            Me.lblKennzeichen.Text = .Kenn1 & "-" & .Kenn2
            Me.lblKennzeichen1.Text = .Wunschkennzeichen1
            Me.lblKennzeichen2.Text = .Wunschkennzeichen2
            Me.lblKennzeichen3.Text = .Wunschkennzeichen3


            Me.lblVin.Text = .Vin
            Me.lblRef.Text = .Ref
            Me.lblBem.Text = .Bemerkung

            Me.lblZugelassen.Text = .FzgZugelassen
            Me.lblBereifung.Text = .SomWin

            Me.lblDatumUeberf.Text = .DatumUeberf

            Me.lblFahrzeugwert.Text = .FahrzeugwertTxt

            Me.lblFahrzeugklasse.Text = .FahrzeugklasseTxt

            If .Tanken = True Then
                Me.lblTanken.Text = "Ja"
            Else
                Me.lblTanken.Text = "Nein"
            End If

            If .FzgEinweisung = True Then
                Me.lblEinw.Text = "Ja"
            Else
                Me.lblEinw.Text = "Nein"
            End If

            If .Waesche = True Then
                Me.lblWW.Text = "Ja"
            Else
                Me.lblWW.Text = "Nein"
            End If

            If .RotKenn = True Then
                Me.lblRotKenn.Text = "Ja"
            Else
                Me.lblRotKenn.Text = "Nein"
            End If

            Me.lblHinZulKCL.Text = .Hin_KCL_Zulassen



            Me.lblExpress.Text = .Express

            Me.lblHaltername.Text = .ZulHaltername

            Me.lblZulDatum.Text = .Zulassungsdatum

            If Not .tblKreis Is Nothing Then

                If .tblKreis.Rows.Count > 0 Then
                    Me.lblStva.Text = "Ermittelter Zulassungskreis: " & .tblKreis.Rows(0)("ZKFZKZ") & "/" & _
                                        .tblKreis.Rows(0)("ZKREIS")
                    clsUeberf.StVa = Me.lblStva.Text
                End If
            End If

            If .Anschluss = False Then
                Me.Table5.Visible = False
            Else
                lbl2ReName.Text = .ReName
                lbl2ReStrasse.Text = .ReStrasse & " " & .ReNr
                Me.lbl2RePlzOrt.Text = .RePlz & " " & .ReOrt
                Me.lbl2ReAnsprech.Text = .ReAnsprechpartner
                Me.lbl2ReTelefon.Text = .ReTelefon
                Me.lbl2ReFax.Text = .ReFax
                Me.lbl2ReHerst.Text = .ReHerst
                Me.lbl2ReFahrzeugklasse.Text = .ReFahrzeugklasseTxt
                Me.lbl2ReKenn.Text = .ReKenn1 & "-" & .ReKenn2
                Me.lbl2ReVin.Text = .ReVin
                Me.lbl2ReRef.Text = .ReRef
                Me.lbl2ReZugelassen.Text = .ReFzgZugelassen
                Me.lbl2ReBereif.Text = .ReSomWin
                Me.lblAnZulKCL.Text = .An_KCL_Zulassen
            End If

            'Me.Table7.Visible = False
            Me.Table8.Visible = False
            Me.Table9.Visible = False
        End With
    End Sub


    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdBack.Click

        If clsUeberf Is Nothing Then
            clsUeberf = Session("Ueberf")
        End If

        If clsUeberf.Vbeln <> "" Then
            SetNothing()
            Response.Redirect("UeberfZulg_Start.aspx?AppID=" & Session("AppID").ToString)
        Else
            clsUeberf.Modus = 2

            Session("Ueberf") = clsUeberf
            If clsUeberf.Anschluss = False Then
                Response.Redirect("Ueberfg_02.aspx?AppID=" & Session("AppID").ToString)
            Else
                Response.Redirect("Ueberfg_03.aspx?AppID=" & Session("AppID").ToString)
            End If
        End If

    End Sub


    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim RetTable As DataTable
        Dim strErr As String = ""
        If clsUeberf Is Nothing Then
            clsUeberf = Session("Ueberf")
        End If


        RetTable = clsUeberf.Save()

        Me.cmdSave.Visible = False


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
            strMessage = strMessage & "Haltername: " & clsUeberf.ZulHaltername & vbCrLf
            'strMessage = strMessage & "Kennzeichen: " & clsUeberf.Kenn1 & "-" & clsUeberf.Kenn2 & vbCrLf

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
            strMessage = strMessage & "Zulassungsdatum: " & clsUeberf.Zulassungsdatum

            Try
                'Absenden
                Dim Mail As System.Net.Mail.MailMessage
                Dim smtpMailSender As String = ""
                Dim smtpMailServer As String = ""
                smtpMailSender = ConfigurationManager.AppSettings("SmtpMailSender")
                smtpMailServer = ConfigurationManager.AppSettings("SmtpMailServer")
                Mail = New System.Net.Mail.MailMessage(smtpMailSender, clsUeberf.VBMail, _
                       "Zulassung für Kunde: " & m_User.KUNNR.TrimStart("0"c) & " " & _
                       m_User.CustomerName, strMessage)

                Dim client As New System.Net.Mail.SmtpClient(smtpMailServer)
                client.Send(Mail)

            Catch ex As Exception
                lblError.Text = "Fehler bei Versenden des Auftrags."
            End Try

            'Me.Table7.Visible = True
            'Me.Table8.Visible = True
            'Me.Table9.Visible = True

            lblError.Text = "Ihr Auftrag wurde unter der Auftragsnummer " & clsUeberf.Vbeln.TrimStart("0"c) & " in unserem System erfasst!"
            lbl_Auftragsnr.Text = "Unsere Überführungsauftragsnr.: " & clsUeberf.Vbeln.TrimStart("0"c) & "  vom " & Today.ToShortDateString()
            lbl_Auftragsnr.Visible = True
            Me.cmdBack.Visible = False
            Me.cmdNewOrder.Visible = True
            Me.cmdNewOrderHoldData.Visible = True
            Me.cmdPrint.Visible = True
        End If
    End Sub

    Private Sub DoSubmit()


        If clsUeberf Is Nothing Then
            clsUeberf = Session("Ueberf")
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

                    Me.lblUnterlagen.Text = "Zu diesem STVA stehen uns zur Zeit leider noch keine Daten zur Verfügung."

                    'Wird für ein Mergefield in Word gebraucht 
                    clsUeberf.UnterlagenTxt = "Zu diesem STVA stehen uns zur Zeit leider noch keine Daten zur Verfügung."

                    Me.lblUnterlagen.Visible = True
                    Me.Table8.Visible = False
                    Me.Table9.Visible = False
                Else
                    Me.Table8.Visible = True
                    Me.Table9.Visible = True
                    clsUeberf.ShowTables = True
                    FillForm()
                    'Wird für ein Mergefield in Word gebraucht 
                    clsUeberf.UnterlagenTxt = "Bitte halten Sie folgende Unterlagen für die Zulassung bereit:"

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
            With clsUeberf
                .ZP0 = CType(resultRow("PZUL_SCHEIN"), String)
                .ZP1 = CType(resultRow("PZUL_BRIEF"), String)
                .ZP2 = CType(resultRow("PZUL_COC"), String)
                .ZP3 = CType(resultRow("PZUL_DECK"), String)
                .ZP4 = CType(resultRow("PZUL_VOLLM"), String)
                .ZP5 = CType(resultRow("PZUL_AUSW"), String)
                .ZP6 = CType(resultRow("PZUL_GEWERB"), String)
                .ZP7 = CType(resultRow("PZUL_HANDEL"), String)
                .ZP8 = CType(resultRow("PZUL_LAST"), String)
                .ZP9 = CType(resultRow("PZUL_BEM"), String)


                'Privat Zulassung
                Label00.Text = .ZP0
                Label01.Text = .ZP1
                Label02.Text = .ZP2
                Label03.Text = .ZP3
                Label04.Text = .ZP4
                Label05.Text = .ZP5
                Label06.Text = .ZP6
                Label07.Text = .ZP7
                Label08.Text = .ZP8
                Label09.Text = .ZP9


                .Z0 = CType(resultRow("UZUL_BRIEF"), String)
                .Z1 = CType(resultRow("UZUL_SCHEIN"), String)
                .Z2 = CType(resultRow("UZUL_COC"), String)
                .Z3 = CType(resultRow("UZUL_DECK"), String)
                .Z4 = CType(resultRow("UZUL_VOLLM"), String)
                .Z5 = CType(resultRow("UZUL_AUSW"), String)
                .Z6 = CType(resultRow("UZUL_GEWERB"), String)
                .Z7 = CType(resultRow("UZUL_HANDEL"), String)
                .Z8 = CType(resultRow("UZUL_LAST"), String)
                .Z9 = CType(resultRow("UZUL_BEM"), String)

                Label40.Text = .Z0
                Label41.Text = .Z1
                Label42.Text = .Z2
                Label43.Text = .Z3
                Label44.Text = .Z4
                Label45.Text = .Z5
                Label46.Text = .Z6
                Label47.Text = .Z7
                Label48.Text = .Z8
                Label49.Text = .Z9

            End With

            If clsUeberf Is Nothing Then
                clsUeberf = Session("Ueberf")
            End If

            clsUeberf.ZulassungsdienstTxt = "In Kürze wird sich Ihr Zulassungsdienst mit Ihnen in Verbindung setzen:"

            If UCase(clsUeberf.VB3100) = "X" Then
                Me.lblZulassungsdienst.Text = "Bitte senden Sie für die Zulassung folgende Unterlagen an:"
                clsUeberf.ZulassungsdienstTxt = Me.lblZulassungsdienst.Text
                Me.lblUnterlagen.Text = "Erforderliche Unterlagen:"
                'Für das Mergefield in Word
                clsUeberf.UnterlagenTxt = Me.lblUnterlagen.Text
            End If

        Catch ex As Exception
            lblError.Text = "Daten konnten nicht gelesen werden."
        End Try
    End Sub

    Private Sub Beauftragt()
        Me.Table7.Visible = True
        Me.lblZulassungsdienst.Visible = True
        Me.lblUnterlagen.Visible = True
        Me.lblStva.Visible = True
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

            If clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.ZulassungUndUeberfuehrungKCL Then
                docFactory.CreateDocument(clsUeberf.Ref + "_Ueb", Me, "\Applications\AppUeberf\Documents\ZulassungUndÜberführungStandard.doc")

            Else
                docFactory.CreateDocument(clsUeberf.Ref + "_Ueb", Me, "\Applications\AppUeberf\Documents\ÜberführungStandard.doc")
            End If


        Catch ex As Exception
            lblError.Text = "Fehler beim Erstellen des Ausdrucks: " + ex.Message
        End Try

    End Sub

    Private Sub cmdNewOrder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdNewOrder.Click
        If clsUeberf Is Nothing Then
            clsUeberf = Session("Ueberf")
        End If

        If clsUeberf.Vbeln <> "" Then
            SetNothing()
            Response.Redirect("Ueberfg_ZulStart.aspx?AppID=" & Session("AppID").ToString)
        End If
    End Sub

    Private Sub cmdNewOrderHoldData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdNewOrderHoldData.Click
        If clsUeberf Is Nothing Then
            clsUeberf = Session("Ueberf")
        End If

        If clsUeberf.Vbeln <> "" Then

            clsUeberf.CleanClass()
            clsUeberf.HoldData = True
            Session("Ueberf") = clsUeberf

            Response.Redirect("Ueberfg_ZulStart.aspx?AppID=" & Session("AppID").ToString)

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
' $History: Zulg_UebBest.aspx.vb $
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
' *****************  Version 17  *****************
' User: Rudolpho     Date: 23.08.07   Time: 13:14
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 16  *****************
' User: Rudolpho     Date: 23.08.07   Time: 12:57
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' ITA: 1246 
' 
' *****************  Version 15  *****************
' User: Rudolpho     Date: 23.08.07   Time: 8:36
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 14  *****************
' User: Rudolpho     Date: 20.08.07   Time: 17:15
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' ITA: 1192, 1246
' 
' *****************  Version 13  *****************
' User: Rudolpho     Date: 27.07.07   Time: 11:18
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Dokumentvorlagen angepasst
' 
' *****************  Version 12  *****************
' User: Rudolpho     Date: 27.07.07   Time: 8:18
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Bugfixing drucken
' 
' *****************  Version 11  *****************
' User: Rudolpho     Date: 23.07.07   Time: 11:20
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 9.07.07    Time: 11:56
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 9  *****************
' User: Uha          Date: 3.07.07    Time: 19:42
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' FormAuth in diverse Seiten wieder eingefügt
' 
' *****************  Version 8  *****************
' User: Uha          Date: 2.07.07    Time: 10:05
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 7  *****************
' User: Fassbenders  Date: 22.06.07   Time: 16:39
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 6  *****************
' User: Fassbenders  Date: 20.06.07   Time: 15:47
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 19.06.07   Time: 15:11
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 12.06.07   Time: 17:46
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 3  *****************
' User: Uha          Date: 3.05.07    Time: 15:16
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Änderungen aus StartApplication vom 02.05.2007 Mittags übernommen
' 
' *****************  Version 2  *****************
' User: Uha          Date: 5.04.07    Time: 11:14
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Verlinkung der Formulare untereinander korrigiert.
' 
' *****************  Version 1  *****************
' User: Uha          Date: 7.03.07    Time: 16:56
' Created in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Überführungs-ASPX-Seiten aus Shared-Bereich übernommen
' 
' ******************************************
