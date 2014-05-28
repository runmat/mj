Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Imports AppUeberf.Ueberf_01
Imports AppUeberf.Helper
Imports AppUeberf.Controls.ProgressControl

Public Class ZulBest01
    Inherits System.Web.UI.Page
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHaltername As System.Web.UI.WebControls.Label
    Protected WithEvents Label10 As System.Web.UI.WebControls.Label
    Protected WithEvents Label14 As System.Web.UI.WebControls.Label
    Protected WithEvents lblDatumUeberf As System.Web.UI.WebControls.Label
    Protected WithEvents Label25 As System.Web.UI.WebControls.Label
    Protected WithEvents lblZulassungsdienst As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2ReStrasse As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2RePlzOrt As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2ReTelefon As System.Web.UI.WebControls.Label
    Protected WithEvents cmdBack As System.Web.UI.WebControls.ImageButton
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    Protected WithEvents cmdPrint As System.Web.UI.WebControls.Button
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents Table5 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents lbl2ReE_Mail As System.Web.UI.WebControls.Label
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
    Protected WithEvents ucHeader As Header
    Protected WithEvents lbl2ReName1 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2ReName2 As System.Web.UI.WebControls.Label
    Protected WithEvents Table6 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Table7 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents lblUnterlagen As System.Web.UI.WebControls.Label
    Protected WithEvents lblStva As System.Web.UI.WebControls.Label
    Protected WithEvents Label13 As System.Web.UI.WebControls.Label
    Protected WithEvents lblLeasingnehmer As System.Web.UI.WebControls.Label
    Protected WithEvents Label15 As System.Web.UI.WebControls.Label
    Protected WithEvents lblVersicherungsnehmer As System.Web.UI.WebControls.Label
    Protected WithEvents Label16 As System.Web.UI.WebControls.Label
    Protected WithEvents lblVersicherer As System.Web.UI.WebControls.Label
    Protected WithEvents Label17 As System.Web.UI.WebControls.Label
    Protected WithEvents lblKfzSteuer As System.Web.UI.WebControls.Label
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents lblRef As System.Web.UI.WebControls.Label
    Protected WithEvents lblKennzeichen1 As System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents lblKennzeichen2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label11 As System.Web.UI.WebControls.Label
    Protected WithEvents lblKennzeichen3 As System.Web.UI.WebControls.Label
    Protected WithEvents Label12 As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles
    Protected WithEvents pnlPlaceholder As System.Web.UI.WebControls.Panel
    Protected WithEvents pnlPlaceHolder2 As System.Web.UI.WebControls.Panel
    Protected WithEvents pnlPlaceholder3 As System.Web.UI.WebControls.Panel
    Protected WithEvents lblBemerkung As System.Web.UI.WebControls.Label
    Protected WithEvents Label18 As System.Web.UI.WebControls.Label
    Protected WithEvents Label19 As System.Web.UI.WebControls.Label
    Protected WithEvents lblSchildversandName As System.Web.UI.WebControls.Label
    Protected WithEvents lblSchildversandStrasseHausnr As System.Web.UI.WebControls.Label
    Protected WithEvents lblSchildversandPLZOrt As System.Web.UI.WebControls.Label
    Protected WithEvents ProgressControl1 As Controls.ProgressControl


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

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private clsUeberf As Ueberf_01
    Private objSuche As Report_99


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
            Beauftragen()
        End If

        If clsUeberf Is Nothing Then
            clsUeberf = CType(Session(CONST_SESSION_UEBERFUEHRUNG), Ueberf_01)
        End If

        ProgressControl1.Fill(Source.ZulBest, clsUeberf)

        GetData()

    End Sub

    Private Sub DoSubmit()


        If clsUeberf Is Nothing Then
            clsUeberf = CType(Session(CONST_SESSION_UEBERFUEHRUNG), Ueberf_01)
        End If

        GetData()

        With clsUeberf

            'VKORG
            .getVKBuero(m_User.KUNNR)

            lblKennzeichen1.Text = .Wunschkennzeichen1
            lblKennzeichen2.Text = .Wunschkennzeichen2
            lblKennzeichen3.Text = .Wunschkennzeichen3

            lblRef.Text = .Ref

            lblDatumUeberf.Text = .Zulassungsdatum


            lbl2ReName1.Text = .VBName1
            lbl2ReName2.Text = .VBName2
            lbl2ReStrasse.Text = .VBStrasse
            lbl2RePlzOrt.Text = .VBPlzOrt
            lbl2ReTelefon.Text = .VBTel
            lbl2ReE_Mail.Text = .VBMail


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
                    Table6.Visible = False
                    Table7.Visible = False
                Else
                    Table6.Visible = True
                    Table7.Visible = True
                    FillForm()
                End If
            End If

        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten."
        End Try
    End Sub

    Private Sub GetData()
        If clsUeberf Is Nothing Then
            clsUeberf = CType(Session(CONST_SESSION_UEBERFUEHRUNG), Ueberf_01)
        End If

        With clsUeberf

            lblHaltername.Text = .ZulHaltername

            lblKennzeichen1.Text = .Wunschkennzeichen1
            lblKennzeichen2.Text = .Wunschkennzeichen2
            lblKennzeichen3.Text = .Wunschkennzeichen3


            lblRef.Text = .Ref

            lblDatumUeberf.Text = .Zulassungsdatum


            lblLeasingnehmer.Text = .Leasingnehmer
            lblVersicherungsnehmer.Text = .Versicherungsnehmer
            lblVersicherer.Text = .Versicherer
            lblKfzSteuer.Text = .KfzSteuer
            lblBemerkung.Text = .BemerkungLease

            lblSchildversandName.Text = .SchildversandName
            lblSchildversandStrasseHausnr.Text = .SchildversandStrasseHausnr
            lblSchildversandPLZOrt.Text = .SchildversandPLZOrt

            If Not .tblKreis Is Nothing Then

                If .tblKreis.Rows.Count > 0 Then

                    lblStva.Text = "Ermittelter Zulassungskreis: " & .tblKreis.Rows(0)("ZKFZKZ").ToString() & "/" & _
                                        .tblKreis.Rows(0)("ZKREIS").ToString()

                End If
            End If


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


    Private Sub Beauftragen()
        Table5.Visible = False
        Table6.Visible = False
        Table7.Visible = False
        lblZulassungsdienst.Visible = False
        lblUnterlagen.Visible = False
        cmdSave.Visible = True
    End Sub

    Private Sub Beauftragt()
        Table5.Visible = True
        lblZulassungsdienst.Visible = True
        lblUnterlagen.Visible = True
        lblStva.Visible = True
    End Sub


    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdBack.Click

        clsUeberf.zulModus = 1


        If clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.ZulassungKCL And cmdPrint.Visible = True Or _
            clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.ZulassungUndUeberfuehrungKCL And cmdPrint.Visible = True Then
            clsUeberf = Nothing
            Session("Ueberf") = Nothing
            Response.Redirect("UeberfZulStart.aspx?AppID=" & Session("AppID").ToString)
        Else
            Response.Redirect("Zul01.aspx?AppID=" & Session("AppID").ToString)
        End If
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        If clsUeberf Is Nothing Then
            clsUeberf = CType(Session(CONST_SESSION_UEBERFUEHRUNG), Ueberf_01)
        End If

        cmdSave.Visible = False

        Dim strMessage As String = ""
        DoSubmit()
        Beauftragt()

        strMessage = "Beauftragt von: " & m_User.UserName & vbCrLf
        strMessage = strMessage & "Kunde: " & m_User.KUNNR.TrimStart("0"c) & " " & m_User.CustomerName & vbCrLf
        strMessage = strMessage & "Leasingnehmer: " & clsUeberf.Leasingnehmer & vbCrLf
        strMessage = strMessage & "Referenz-Nr.: " & clsUeberf.Leasingnehmer & vbCrLf
        strMessage = strMessage & "Haltername: " & clsUeberf.ZulHaltername & vbCrLf
        strMessage = strMessage & "Versicherungsnehmer: " & clsUeberf.Versicherungsnehmer & vbCrLf
        strMessage = strMessage & "Versicherer: " & clsUeberf.Versicherungsnehmer & vbCrLf
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
            Dim client As New System.Net.Mail.SmtpClient(ConfigurationManager.AppSettings("SmtpMailServer"))
            client.Send(ConfigurationManager.AppSettings("SmtpMailSender"), clsUeberf.VBMail, "Zulassung für Kunde: " & m_User.KUNNR.TrimStart("0"c) & " " & m_User.CustomerName, strMessage)

        Catch ex As Exception
            lblError.Text = "Fehler bei Versenden des Auftrags."
        End Try

        cmdPrint.Visible = True

    End Sub


    Private Sub cmdPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPrint.Click
        'Response.Write("<script>window.open('ZulPrint.aspx?AppID=" & Session("AppID").ToString & "')</script>")

        Try
            Dim imageHt As New Hashtable()
            imageHt.Add("Logo", m_User.Customer.LogoImage)

            clsUeberf.getVKBueroFooter(m_User.KUNNR)

            Dim docFactory As New Base.Kernel.DocumentGeneration.WordDocumentFactory(Base.Kernel.Common.DataTableHelper.ObjectToDataTable(clsUeberf), imageHt)

            docFactory.CreateDocument(clsUeberf.Ref + "_Zul", Page, "\Applications\AppUeberf\Documents\Zulassung.doc")


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
' $History: ZulBest01.aspx.vb $
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
' User: Uha          Date: 22.05.07   Time: 10:49
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