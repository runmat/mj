Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

<Obsolete()> _
Public Class UeberfPrint
    Inherits System.Web.UI.Page
    Protected WithEvents lblSchritt As System.Web.UI.WebControls.Label
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
    Protected WithEvents lbl2RePlzOrt As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents imgLogo As System.Web.UI.WebControls.Image
    Protected WithEvents Label27 As System.Web.UI.WebControls.Label
    Protected WithEvents lblReBemerkung As System.Web.UI.WebControls.Label
    Protected WithEvents Label24 As System.Web.UI.WebControls.Label
    Protected WithEvents lblUser As System.Web.UI.WebControls.Label
    Protected WithEvents lblAbTelefon2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label29 As System.Web.UI.WebControls.Label
    Protected WithEvents lblAnTelefon2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label31 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2ReTelefon1 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2ReTelefon2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label30 As System.Web.UI.WebControls.Label
    Protected WithEvents lblOrtLabel As System.Web.UI.WebControls.Label
    Protected WithEvents lblLeasingnehmerOrt As System.Web.UI.WebControls.Label
    Protected WithEvents lblLeasingnehmerName As System.Web.UI.WebControls.Label
    Protected WithEvents lblNameLabel As System.Web.UI.WebControls.Label
    Protected WithEvents lblFahrzeugtyp As System.Web.UI.WebControls.Label
    Protected WithEvents lblTypLabel As System.Web.UI.WebControls.Label
    Protected WithEvents lblReferenz As System.Web.UI.WebControls.Label
    Protected WithEvents lblRefLabel As System.Web.UI.WebControls.Label
    Protected WithEvents lblUeberfDatumFix As System.Web.UI.WebControls.Label
    Protected WithEvents Label39 As System.Web.UI.WebControls.Label
    Protected WithEvents lblWinterText As System.Web.UI.WebControls.Label
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
    Private m_User As Base.Kernel.Security.User

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        m_User = GetUser(Me)

        'imgLogo.ImageUrl = "../../" & m_User.Customer.CustomerStyle.LogoPath
        imgLogo.ImageUrl = "../../" & m_User.Customer.LogoPath2

        If clsUeberf Is Nothing Then
            clsUeberf = Session("Ueberf")
        End If

        With clsUeberf

            Me.lblLeasingnehmerName.Text = clsUeberf.Leasingnehmer
            Me.lblLeasingnehmerOrt.Text = clsUeberf.LeasingnehmerOrt
            Me.lblReferenz.Text = clsUeberf.Ref
            Me.lblFahrzeugtyp.Text = clsUeberf.Herst

            Me.lblAbName.Text = .AbName
            Me.lblAbStrasse.Text = .AbStrasse & " " & .AbNr
            Me.lblAbOrt.Text = .AbPlz & " " & .AbOrt
            Me.lblAbAnsprechpartner.Text = .AbAnsprechpartner
            Me.lblAbTelefon.Text = .AbTelefon1
            Me.lblAbTelefon2.Text = .AbTelefon2
            Me.lblAnName.Text = .AnName
            Me.lblAnStrasse.Text = .AnStrasse & " " & .AnNr
            Me.lblAnOrt.Text = .AnPlz & " " & .AnOrt
            Me.lblAnAnspechpartner.Text = .AnAnsprechpartner
            Me.lblAnTelefon.Text = .AnTelefon1
            Me.lblAnTelefon2.Text = .AnTelefon2

            Me.lblHerst.Text = .Herst
            Me.lblKennzeichen.Text = .Kenn1 & "-" & .Kenn2
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

            If .FixDatumUeberfuehrung Then
                Me.lblUeberfDatumFix.Text = "Fix"
            Else
                Me.lblUeberfDatumFix.Text = ""
            End If
            If .WinterHandling Then
                Me.lblWinterText.Text = .WinterText
            Else
                Me.lblWinterText.Text = ""
            End If


            If .Anschluss = False Then
                Me.Table5.Visible = False
            Else
                lbl2ReName.Text = .ReName
                lbl2ReStrasse.Text = .ReStrasse & " " & .ReNr
                Me.lbl2RePlzOrt.Text = .RePlz & " " & .ReOrt
                Me.lbl2ReAnsprech.Text = .ReAnsprechpartner
                Me.lbl2ReTelefon1.Text = .ReTelefon1
                Me.lbl2ReTelefon2.Text = .ReTelefon2
                Me.lbl2ReHerst.Text = .ReHerst
                Me.lbl2ReKenn.Text = .ReKenn1 & "-" & .ReKenn2
                Me.lbl2ReVin.Text = .ReVin
                Me.lbl2ReRef.Text = .ReRef
                Me.lbl2ReZugelassen.Text = .ReFzgZugelassen
                Me.lbl2ReBereif.Text = .ReSomWin
                Me.lblReBemerkung.Text = .ReBemerkung
            End If


            lblUser.Text = "Erfasst durch: " & m_User.UserName
            lblError.Text = "Ihr Auftrag wurde unter der Auftragsnummer " & .Vbeln.TrimStart("0"c) & " in unserem System erfasst!"

        End With
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: UeberfPrint.aspx.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 16:06
' Created in $/CKAG/Applications/AppUeberf/Forms
' 
' *****************  Version 10  *****************
' User: Uha          Date: 2.07.07    Time: 10:05
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 9  *****************
' User: Uha          Date: 21.05.07   Time: 11:47
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Änderungen im Vergleich zur Startapplikation zum Stand 11.05.2007
' 
' *****************  Version 8  *****************
' User: Uha          Date: 3.05.07    Time: 15:16
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Änderungen aus StartApplication vom 02.05.2007 Mittags übernommen
' 
' ************************************************