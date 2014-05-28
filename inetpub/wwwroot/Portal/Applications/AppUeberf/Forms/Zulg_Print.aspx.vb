Imports CKG.Base.Business

Public Class Zulg_Print
    Inherits System.Web.UI.Page
    Protected WithEvents lblSchritt As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblKundeName1 As System.Web.UI.WebControls.Label
    Protected WithEvents lblKundeStrasse As System.Web.UI.WebControls.Label
    Protected WithEvents lblKundeAnsprechpartner As System.Web.UI.WebControls.Label
    Protected WithEvents lblKundePlzOrt As System.Web.UI.WebControls.Label
    Protected WithEvents Label10 As System.Web.UI.WebControls.Label
    Protected WithEvents lblDatumUeberf As System.Web.UI.WebControls.Label
    Protected WithEvents Label14 As System.Web.UI.WebControls.Label
    Protected WithEvents lblHaltername As System.Web.UI.WebControls.Label
    Protected WithEvents Label25 As System.Web.UI.WebControls.Label
    Protected WithEvents lblRef As System.Web.UI.WebControls.Label
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents lblZulassungsdienst As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2ReName1 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2ReName2 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2ReStrasse As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2RePlzOrt As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2ReTelefon As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2ReE_Mail As System.Web.UI.WebControls.Label
    Protected WithEvents Table5 As System.Web.UI.HtmlControls.HtmlTable
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
    Protected WithEvents Table7 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents lblKennzeichen1 As System.Web.UI.WebControls.Label
    Protected WithEvents Label11 As System.Web.UI.WebControls.Label
    Protected WithEvents Label12 As System.Web.UI.WebControls.Label
    Protected WithEvents lblKennzeichen2 As System.Web.UI.WebControls.Label
    Protected WithEvents lblKennzeichen3 As System.Web.UI.WebControls.Label
    Protected WithEvents lblUnterlagen As System.Web.UI.WebControls.Label
    Protected WithEvents lblStva As System.Web.UI.WebControls.Label
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
    Private clsUeberf As UeberfgStandard_01
    Private objSuche As Report_99

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If clsUeberf Is Nothing Then
            clsUeberf = Session("Ueberf")
        End If

        With clsUeberf

            Me.lblKundeName1.Text = .KundeName
            Me.lblKundeStrasse.Text = .KundeStrasse
            Me.lblKundePlzOrt.Text = .KundeOrt
            Me.lblKundeAnsprechpartner.Text = .KundeAnsprechpartner

            Me.lbl2ReName1.Text = .VBName1
            Me.lbl2ReName2.Text = .VBName2
            Me.lbl2ReStrasse.Text = .VBStrasse
            Me.lbl2RePlzOrt.Text = .VBPlzOrt
            Me.lbl2ReTelefon.Text = .VBTel
            Me.lbl2ReE_Mail.Text = .VBMail

            Me.lblKennzeichen1.Text = .Wunschkennzeichen1
            Me.lblKennzeichen2.Text = .Wunschkennzeichen2
            Me.lblKennzeichen3.Text = .Wunschkennzeichen3


            Me.lblRef.Text = .Ref
            Me.lblDatumUeberf.Text = .Zulassungsdatum
            Me.lblHaltername.Text = .ZulHaltername

            If UCase(.VB3100) = "X" Then
                Me.lblZulassungsdienst.Text = "Bitte senden Sie für die Zulassung folgende Unterlagen an:"
                Me.lblUnterlagen.Text = "Erforderliche Unterlagen:"
            End If

            If Not .tblKreis Is Nothing Then

                If .tblKreis.Rows.Count > 0 Then

                    Me.lblStva.Text = "Ermittelter Zulassungskreis: " & .tblKreis.Rows(0)("ZKFZKZ") & "/" & _
                                        .tblKreis.Rows(0)("ZKREIS")

                End If
            End If


            FillForm()
            'lblError.Text = "Ihr Auftrag wurde unter der Auftragsnummer " & .Vbeln.TrimStart("0"c) & " in unserem System erfasst!"

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
            Me.Table6.Visible = False
            Me.Table7.Visible = False
            'lblError.Text = "Daten konnten nicht gelesen werden."
        End Try
    End Sub

End Class

' ************************************************
' $History: Zulg_Print.aspx.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 16:06
' Created in $/CKAG/Applications/AppUeberf/Forms
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 12.06.07   Time: 17:46
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 1  *****************
' User: Uha          Date: 7.03.07    Time: 16:56
' Created in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Überführungs-ASPX-Seiten aus Shared-Bereich übernommen
' 
' ******************************************
