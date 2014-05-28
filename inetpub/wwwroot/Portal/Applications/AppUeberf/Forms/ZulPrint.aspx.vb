Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Imports AppUeberf.Ueberf_01
Imports AppUeberf.Helper

<Obsolete()> _
Public Class ZulPrint
    Inherits System.Web.UI.Page
    Protected WithEvents lblSchritt As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents Label10 As System.Web.UI.WebControls.Label
    Protected WithEvents lblDatumUeberf As System.Web.UI.WebControls.Label
    Protected WithEvents Label14 As System.Web.UI.WebControls.Label
    Protected WithEvents lblHaltername As System.Web.UI.WebControls.Label
    Protected WithEvents Label25 As System.Web.UI.WebControls.Label
    Protected WithEvents lblZulassungsdienst As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2ReName1 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2ReName2 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2ReStrasse As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2RePlzOrt As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2ReTelefon As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2ReE_Mail As System.Web.UI.WebControls.Label
    Protected WithEvents Table5 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents lblStva As System.Web.UI.WebControls.Label
    Protected WithEvents Label13 As System.Web.UI.WebControls.Label
    Protected WithEvents Label15 As System.Web.UI.WebControls.Label
    Protected WithEvents Label16 As System.Web.UI.WebControls.Label
    Protected WithEvents lblVersicherungsnehmer As System.Web.UI.WebControls.Label
    Protected WithEvents Label17 As System.Web.UI.WebControls.Label
    Protected WithEvents lblLeasingnehmer As System.Web.UI.WebControls.Label
    Protected WithEvents lblVersicherer As System.Web.UI.WebControls.Label
    Protected WithEvents lblKfzSteuer As System.Web.UI.WebControls.Label
    Protected WithEvents lblRef As System.Web.UI.WebControls.Label
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents lblKennzeichen1 As System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents lblKennzeichen2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label11 As System.Web.UI.WebControls.Label
    Protected WithEvents lblKennzeichen3 As System.Web.UI.WebControls.Label
    Protected WithEvents Label12 As System.Web.UI.WebControls.Label
    Protected WithEvents imgLogo As System.Web.UI.WebControls.Image
    Protected WithEvents lblUser As System.Web.UI.WebControls.Label
    Protected WithEvents lblOrtLabel As System.Web.UI.WebControls.Label
    Protected WithEvents lblLeasingnehmerOrt As System.Web.UI.WebControls.Label
    Protected WithEvents lblLeasingnehmerName As System.Web.UI.WebControls.Label
    Protected WithEvents lblNameLabel As System.Web.UI.WebControls.Label
    Protected WithEvents lblFahrzeugtyp As System.Web.UI.WebControls.Label
    Protected WithEvents lblTypLabel As System.Web.UI.WebControls.Label
    Protected WithEvents lblReferenz As System.Web.UI.WebControls.Label
    Protected WithEvents lblRefLabel As System.Web.UI.WebControls.Label
    Protected WithEvents lblSchildversandPLZOrt As System.Web.UI.WebControls.Label
    Protected WithEvents lblSchildversandStrasseHausnr As System.Web.UI.WebControls.Label
    Protected WithEvents lblSchildversandName As System.Web.UI.WebControls.Label
    Protected WithEvents Label19 As System.Web.UI.WebControls.Label
    Protected WithEvents lblBemerkung As System.Web.UI.WebControls.Label
    Protected WithEvents Label18 As System.Web.UI.WebControls.Label
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable

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

        'imgLogo.ImageUrl = "../../" & m_User.Customer.CustomerStyle.LogoPath

        imgLogo.ImageUrl = "../../" & m_User.Customer.LogoPath2


        If clsUeberf Is Nothing Then
            clsUeberf = CType(Session(CONST_SESSION_UEBERFUEHRUNG), Ueberf_01)
        End If

        With clsUeberf

            Me.lblLeasingnehmerName.Text = .Leasingnehmer
            Me.lblLeasingnehmerOrt.Text = .LeasingnehmerOrt
            Me.lblReferenz.Text = .Ref
            Me.lblFahrzeugtyp.Text = .Herst

            Me.lbl2ReName1.Text = .VBName1
            Me.lbl2ReName2.Text = .VBName2
            Me.lbl2ReStrasse.Text = .VBStrasse
            Me.lbl2RePlzOrt.Text = .VBPlzOrt
            Me.lbl2ReTelefon.Text = .VBTel
            Me.lbl2ReE_Mail.Text = .VBMail

            Me.lblKennzeichen1.Text = .Wunschkennzeichen1
            Me.lblKennzeichen2.Text = .Wunschkennzeichen2
            Me.lblKennzeichen3.Text = .Wunschkennzeichen3

            Me.lblSchildversandName.Text = .SchildversandName
            Me.lblSchildversandStrasseHausnr.Text = .SchildversandStrasseHausnr
            Me.lblSchildversandPLZOrt.Text = .SchildversandPLZOrt

            Me.lblRef.Text = .Ref
            Me.lblDatumUeberf.Text = .Zulassungsdatum
            Me.lblHaltername.Text = .ZulHaltername

            Me.lblLeasingnehmer.Text = .Leasingnehmer
            Me.lblVersicherungsnehmer.Text = .Versicherungsnehmer
            Me.lblVersicherer.Text = .Versicherer
            Me.lblKfzSteuer.Text = .KfzSteuer
            Me.lblBemerkung.Text = .BemerkungLease

            If Not .tblKreis Is Nothing Then

                If .tblKreis.Rows.Count > 0 Then

                    Me.lblStva.Text = "Ermittelter Zulassungskreis: " & .tblKreis.Rows(0)("ZKFZKZ").ToString() & "/" & _
                                        .tblKreis.Rows(0)("ZKREIS").ToString()

                End If
            End If


            lblUser.Text = "Erfasst durch: " & m_User.UserName


            'lblError.Text = "Ihr Auftrag wurde unter der Auftragsnummer " & .Vbeln.TrimStart("0"c) & " in unserem System erfasst!"

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
' $History: ZulPrint.aspx.vb $
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