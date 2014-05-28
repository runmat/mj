Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Zulg_UebPrint
    Inherits System.Web.UI.Page
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
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
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
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents lblZulassungsdienst As System.Web.UI.WebControls.Label
    Protected WithEvents Label01 As System.Web.UI.WebControls.Label
    Protected WithEvents Label00 As System.Web.UI.WebControls.Label
    Protected WithEvents Label02 As System.Web.UI.WebControls.Label
    Protected WithEvents Label03 As System.Web.UI.WebControls.Label
    Protected WithEvents Label04 As System.Web.UI.WebControls.Label
    Protected WithEvents Label05 As System.Web.UI.WebControls.Label
    Protected WithEvents Label06 As System.Web.UI.WebControls.Label
    Protected WithEvents Label07 As System.Web.UI.WebControls.Label
    Protected WithEvents Label08 As System.Web.UI.WebControls.Label
    Protected WithEvents Label09 As System.Web.UI.WebControls.Label
    Protected WithEvents Label41 As System.Web.UI.WebControls.Label
    Protected WithEvents Label40 As System.Web.UI.WebControls.Label
    Protected WithEvents Label42 As System.Web.UI.WebControls.Label
    Protected WithEvents Label43 As System.Web.UI.WebControls.Label
    Protected WithEvents Label44 As System.Web.UI.WebControls.Label
    Protected WithEvents Label45 As System.Web.UI.WebControls.Label
    Protected WithEvents Label46 As System.Web.UI.WebControls.Label
    Protected WithEvents Label47 As System.Web.UI.WebControls.Label
    Protected WithEvents Label48 As System.Web.UI.WebControls.Label
    Protected WithEvents Label49 As System.Web.UI.WebControls.Label
    Protected WithEvents Table6 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Table7 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Table8 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents lblZulName1 As System.Web.UI.WebControls.Label
    Protected WithEvents lblZulName2 As System.Web.UI.WebControls.Label
    Protected WithEvents lblZulStrasse As System.Web.UI.WebControls.Label
    Protected WithEvents lblZulPlzOrt As System.Web.UI.WebControls.Label
    Protected WithEvents lblZulTel As System.Web.UI.WebControls.Label
    Protected WithEvents lblZulMail As System.Web.UI.WebControls.Label
    Protected WithEvents lblUnterlagen As System.Web.UI.WebControls.Label
    Protected WithEvents lblStva As System.Web.UI.WebControls.Label
    Protected WithEvents Label34 As System.Web.UI.WebControls.Label
    Protected WithEvents lblZulDatum As System.Web.UI.WebControls.Label
    Protected WithEvents Label33 As System.Web.UI.WebControls.Label
    Protected WithEvents lblHaltername As System.Web.UI.WebControls.Label
    Protected WithEvents Label29 As System.Web.UI.WebControls.Label
    Protected WithEvents lblKennzeichen1 As System.Web.UI.WebControls.Label
    Protected WithEvents Label27 As System.Web.UI.WebControls.Label
    Protected WithEvents lblKennzeichen2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label28 As System.Web.UI.WebControls.Label
    Protected WithEvents lblKennzeichen3 As System.Web.UI.WebControls.Label
    Protected WithEvents Label30 As System.Web.UI.WebControls.Label
    Protected WithEvents imgLogo As System.Web.UI.WebControls.Image
    Protected WithEvents Table5 As System.Web.UI.HtmlControls.HtmlTable

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

    Private clsUeberf As Ueberf_01
    Private objSuche As Report_99
    Private m_User As Base.Kernel.Security.User

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        m_User = GetUser(Me)

        imgLogo.ImageUrl = m_User.Customer.CustomerStyle.LogoPath


        If clsUeberf Is Nothing Then
            clsUeberf = Session("Ueberf")
        End If

        With clsUeberf

            Me.lblKundeName1.Text = .KundeName
            Me.lblKundeStrasse.Text = .KundeStrasse
            Me.lblKundePlzOrt.Text = .KundeOrt
            Me.lblKundeAnsprechpartner.Text = .KundeAnsprechpartner
            Me.lblAbName.Text = .AbName
            Me.lblAbStrasse.Text = .AbStrasse & " " & .AbNr
            Me.lblAbOrt.Text = .AbPlz & " " & .AbOrt
            Me.lblAbAnsprechpartner.Text = .AbAnsprechpartner
            Me.lblAbTelefon.Text = .AbTelefon1
            Me.lblAnName.Text = .AnName
            Me.lblAnStrasse.Text = .AnStrasse & " " & .AnNr
            Me.lblAnOrt.Text = .AnPlz & " " & .AnOrt
            Me.lblAnAnspechpartner.Text = .AnAnsprechpartner
            Me.lblAnTelefon.Text = .AnTelefon1

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

            If Not .tblKreis Is Nothing Then

                If .tblKreis.Rows.Count > 0 Then

                    Me.lblStva.Text = "Ermittelter Zulassungskreis: " & .tblKreis.Rows(0)("ZKFZKZ") & "/" & _
                                        .tblKreis.Rows(0)("ZKREIS")

                End If
            End If


            If .Anschluss = False Then
                Me.Table5.Visible = False
            Else
                lbl2ReName.Text = .ReName
                lbl2ReStrasse.Text = .ReStrasse & " " & .ReNr
                Me.lbl2RePlzOrt.Text = .RePlz & " " & .ReOrt
                Me.lbl2ReAnsprech.Text = .ReAnsprechpartner
                Me.lbl2ReTelefon.Text = .ReTelefon1
                Me.lbl2ReHerst.Text = .ReHerst
                Me.lbl2ReKenn.Text = .ReKenn1 & "-" & .ReKenn2
                Me.lbl2ReVin.Text = .ReVin
                Me.lbl2ReRef.Text = .ReRef
                Me.lbl2ReZugelassen.Text = .ReFzgZugelassen
                Me.lbl2ReBereif.Text = .ReSomWin
            End If

            lblZulName1.Text = .VBName1
            lblZulName2.Text = .VBName2
            lblZulStrasse.Text = .VBStrasse
            lblZulPlzOrt.Text = .VBPlzOrt
            lblZulTel.Text = .VBTel
            lblZulMail.Text = .VBMail

            Me.lblDatumUeberf.Text = .DatumUeberf
            Me.lblHaltername.Text = .ZulHaltername
            Me.lblZulDatum.Text = .Zulassungsdatum

            If UCase(.VB3100) = "X" Then
                Me.lblZulassungsdienst.Text = "Bitte senden Sie für die Zulassung folgende Unterlagen an:"
                Me.lblUnterlagen.Text = "Erforderliche Unterlagen:"
            End If

            FillForm()

            lblError.Text = "Ihr Auftrag wurde unter der Auftragsnummer " & .Vbeln.TrimStart("0"c) & " in unserem System erfasst!"
        End With

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


        Catch ex As Exception
            Me.lblUnterlagen.Text = "Zu diesem STVA stehen uns zur Zeit leider noch keine Daten zur Verfügung."
            Me.Table7.Visible = False
            Me.Table8.Visible = False
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
' $History: Zulg_UebPrint.aspx.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 16:06
' Created in $/CKAG/Applications/AppUeberf/Forms
' 
' *****************  Version 3  *****************
' User: Uha          Date: 2.07.07    Time: 10:05
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 2  *****************
' User: Uha          Date: 3.05.07    Time: 15:16
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Änderungen aus StartApplication vom 02.05.2007 Mittags übernommen
' 
' *****************  Version 1  *****************
' User: Uha          Date: 7.03.07    Time: 16:56
' Created in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Überführungs-ASPX-Seiten aus Shared-Bereich übernommen
' 
' ******************************************
