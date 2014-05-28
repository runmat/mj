Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Zulg_Best01
    Inherits System.Web.UI.Page
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblSchritt As System.Web.UI.WebControls.Label
    Protected WithEvents lblKundeName1 As System.Web.UI.WebControls.Label
    Protected WithEvents lblKundeStrasse As System.Web.UI.WebControls.Label
    Protected WithEvents lblKundeAnsprechpartner As System.Web.UI.WebControls.Label
    Protected WithEvents lblKundePlzOrt As System.Web.UI.WebControls.Label
    Protected WithEvents lblHaltername As System.Web.UI.WebControls.Label
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents lblRef As System.Web.UI.WebControls.Label
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
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
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
    Protected WithEvents lblKennzeichen1 As System.Web.UI.WebControls.Label
    Protected WithEvents Label11 As System.Web.UI.WebControls.Label
    Protected WithEvents Label12 As System.Web.UI.WebControls.Label
    Protected WithEvents lblKennzeichen2 As System.Web.UI.WebControls.Label
    Protected WithEvents lblKennzeichen3 As System.Web.UI.WebControls.Label
    Protected WithEvents lblUnterlagen As System.Web.UI.WebControls.Label
    Protected WithEvents lblStva As System.Web.UI.WebControls.Label
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

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private clsUeberf As UeberfgStandard_01
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
            Me.Beauftragen()
        Else
            If clsUeberf Is Nothing Then
                clsUeberf = Session("Ueberf")
            End If

        End If

        GetData()

    End Sub

    Private Sub DoSubmit()


        If clsUeberf Is Nothing Then
            clsUeberf = Session("Ueberf")
        End If

        GetData()

        With clsUeberf

            'VKORG
            .getVKBuero(m_User.KUNNR)

            Me.lblKennzeichen1.Text = .Wunschkennzeichen1
            Me.lblKennzeichen2.Text = .Wunschkennzeichen2
            Me.lblKennzeichen3.Text = .Wunschkennzeichen3

            Me.lblRef.Text = .Ref

            Me.lblDatumUeberf.Text = .Zulassungsdatum


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
                    Me.lblUnterlagen.Text = "Zu diesem STVA stehen uns zur Zeit leider noch keine Daten zur Verfügung."
                    clsUeberf.UnterlagenTxt = Me.lblUnterlagen.Text
                    Me.Table6.Visible = False
                    Me.Table7.Visible = False
                Else
                    Me.Table6.Visible = True
                    Me.Table7.Visible = True
                    FillForm()
                    clsUeberf.ShowTables = True
                End If
            End If

        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten."
        End Try
    End Sub

    Private Sub GetData()
        If clsUeberf Is Nothing Then
            clsUeberf = Session("Ueberf")
        End If

        With clsUeberf


            Me.lblKundeName1.Text = .KundeName
            Me.lblKundeStrasse.Text = .KundeStrasse
            Me.lblKundePlzOrt.Text = .KundeOrt
            Me.lblKundeAnsprechpartner.Text = .KundeAnsprechpartner
            Me.lblHaltername.Text = .ZulHaltername

            Me.lblKennzeichen1.Text = .Wunschkennzeichen1
            Me.lblKennzeichen2.Text = .Wunschkennzeichen2
            Me.lblKennzeichen3.Text = .Wunschkennzeichen3


            Me.lblRef.Text = .Ref

            Me.lblDatumUeberf.Text = .Zulassungsdatum


            If Not .tblKreis Is Nothing Then

                If .tblKreis.Rows.Count > 0 Then

                    Me.lblStva.Text = "Ermittelter Zulassungskreis: " & .tblKreis.Rows(0)("ZKFZKZ") & "/" & _
                                        .tblKreis.Rows(0)("ZKREIS")
                    clsUeberf.StVa = Me.lblStva.Text
                End If
            End If


        End With
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

                'Unternehmen Zulassung
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
                clsUeberf.UnterlagenTxt = Me.lblUnterlagen.Text
            End If


        Catch ex As Exception
            lblError.Text = "Daten konnten nicht gelesen werden."
        End Try
    End Sub


    Private Sub Beauftragen()
        Me.Table5.Visible = False
        Me.Table6.Visible = False
        Me.Table7.Visible = False
        Me.lblZulassungsdienst.Visible = False
        Me.lblUnterlagen.Visible = False
        Me.cmdSave.Visible = True
    End Sub

    Private Sub Beauftragt()
        Me.Table5.Visible = True
        Me.lblZulassungsdienst.Visible = True
        Me.lblUnterlagen.Visible = True
        Me.lblStva.Visible = True
    End Sub


    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdBack.Click
        If clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.ZulassungKCL And cmdPrint.Visible = True Or _
            clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.ZulassungUndUeberfuehrungKCL And cmdPrint.Visible = True Then
            clsUeberf = Nothing
            Session("Ueberf") = Nothing
            Response.Redirect("Ueberfg_ZulStart.aspx?AppID=" & Session("AppID").ToString)
        Else
            Response.Redirect("Zulg_01.aspx?AppID=" & Session("AppID").ToString)
        End If
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim Mail As System.Net.Mail.MailMessage
        If clsUeberf Is Nothing Then
            clsUeberf = Session("Ueberf")
        End If

        Me.cmdSave.Visible = False

        Dim strMessage As String = ""
        DoSubmit()
        Beauftragt()

        strMessage = "Beauftragt von: " & m_User.UserName & vbCrLf
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
        strMessage = strMessage & "Zulassungsdatum: " & clsUeberf.DatumUeberf

        Try
            'Absenden
            Mail = New System.Net.Mail.MailMessage(ConfigurationManager.AppSettings("SmtpMailSender"), clsUeberf.VBMail, _
                                                    "Zulassung für Kunde: " & m_User.KUNNR.TrimStart("0"c) & " " & m_User.CustomerName, strMessage)
            Dim client As New System.Net.Mail.SmtpClient(ConfigurationManager.AppSettings("SmtpMailServer"))
            client.Send(Mail)
        Catch ex As Exception
            lblError.Text = "Fehler bei Versenden des Auftrags."
        End Try

        Me.cmdPrint.Visible = True

    End Sub


    Private Sub cmdPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPrint.Click
        'Response.Write("<script>window.open('Zulg_Print.aspx?AppID=" & Session("AppID").ToString & "')</script>")

        Try
            Dim imageHt As New Hashtable()
            imageHt.Add("Logo", m_User.Customer.LogoImage)

            'clsUeberf.getVKBueroFooter(m_User.KUNNR)

            Dim docFactory As New Base.Kernel.DocumentGeneration.WordDocumentFactory(Base.Kernel.Common.DataTableHelper.ObjectToDataTable(clsUeberf), imageHt)

            docFactory.CreateDocument(clsUeberf.Ref + "_Ueb", Me.Page, "\Applications\AppUeberf\Documents\ZulassungStandard.doc")


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
' $History: Zulg_Best01.aspx.vb $
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
' *****************  Version 6  *****************
' User: Uha          Date: 3.07.07    Time: 19:42
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' FormAuth in diverse Seiten wieder eingefügt
' 
' *****************  Version 5  *****************
' User: Uha          Date: 2.07.07    Time: 10:05
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 19.06.07   Time: 15:11
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 12.06.07   Time: 17:46
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
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
