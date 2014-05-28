Imports CKG.Base.Business
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common
Imports Microsoft.VisualBasic

Public Class Change81_3
    Inherits System.Web.UI.Page

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

    Private objHaendler As SonstDL

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lnkFahrzeugsuche As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkFahrzeugAuswahl As System.Web.UI.WebControls.HyperLink
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ImageButton1 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents litScript As System.Web.UI.WebControls.Literal
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents ddHalterwechselEmpfaenger As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddHalterwechselNeuerStandort As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddHalterwechselNeuerHalter As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtHalterName1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtHalterName2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtHalterStrasse As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtHalterPLZ As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtHalterOrt As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtStandortName1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtStandortName2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtStandortStrasse As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtStandortPLZ As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtStandortOrt As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtKreis As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblKreis As System.Web.UI.WebControls.Label
    Protected WithEvents txtWunschkennzeichen As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtReserviertAuf As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtVersicherungstraeger As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtEmpfaengerName1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtEmpfaengerName2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtEmpfaengerStrasse As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtEmpfaengerPLZ As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtEmpfaengerOrt As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDurchfuehrungsDatum As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnOpenSelectAb As System.Web.UI.WebControls.Button
    Protected WithEvents lblHinweis As System.Web.UI.WebControls.Label
    Protected WithEvents txtBemerkung As System.Web.UI.WebControls.TextBox
    Protected WithEvents trWunschkennzeichen As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trReserviertAuf As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trVersicherungstraeger As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trDurchfuehrungsdatum As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trHinweis As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trBemerkung As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trEmpfaenger2 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trNeuerHalter1 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trNeuerHalter2 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trNeuerStandort1 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trNeuerStandort2 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trEmpfaenger1 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lnkStandortAnzeige As System.Web.UI.WebControls.LinkButton
    Protected WithEvents txtHalterHausnr As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtEmpfaengerHausnr As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtStandortHausnr As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblEVB As System.Web.UI.WebControls.Label
    Protected WithEvents txtEVB As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDatumVon As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents txtDatumbis As System.Web.UI.WebControls.TextBox
    Protected WithEvents trGueltigkeit As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents trHinweisGueltigkeit As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents ddlDienstleistung As System.Web.UI.WebControls.DropDownList

    Private tmpbankBaseobj As BankBase
    Private mError As Boolean = False

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        lnkFahrzeugAuswahl.NavigateUrl = "Change81_2.aspx?AppID=" & Session("AppID").ToString
        lnkFahrzeugsuche.NavigateUrl = "Change81.aspx?AppID=" & Session("AppID").ToString

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        If m_User.Organization.OrganizationAdmin Then
            lblHead.Text = Replace(lblHead.Text, " (Beauftragung)", "")
        End If
        ucStyles.TitleText = lblHead.Text
        m_App = New Base.Kernel.Security.App(m_User)

        If Session("objHaendler") Is Nothing Then
            Response.Redirect("Change81.aspx?AppID=" & Session("AppID").ToString)
        End If
        objHaendler = CType(Session("objHaendler"), SonstDL)

        'dient nur für die makeSAPDate-Funktion
        If tmpbankBaseobj Is Nothing Then
            tmpbankBaseobj = New BankBaseCredit(m_User, m_App, Session("AppID").ToString, Page.Session.SessionID, "")
        End If

        If Not IsPostBack Then


            'Init
            txtHalterName1.Text = objHaendler.HalterName1
            txtHalterName2.Text = objHaendler.HalterName2
            txtHalterOrt.Text = objHaendler.HalterOrt
            txtHalterPLZ.Text = objHaendler.HalterPLZ
            txtHalterStrasse.Text = objHaendler.HalterStrasse
            txtHalterHausnr.Text = objHaendler.HalterHausnr
            txtStandortName1.Text = objHaendler.StandortName1
            txtStandortName2.Text = objHaendler.StandortName2
            txtStandortOrt.Text = objHaendler.StandortOrt
            txtStandortPLZ.Text = objHaendler.StandortPLZ
            txtStandortStrasse.Text = objHaendler.StandortStrasse
            txtStandortHausnr.Text = objHaendler.StandortHausnr
            txtKreis.Text = objHaendler.Kreis
            lblKreis.Text = objHaendler.Kreis
            txtWunschkennzeichen.Text = objHaendler.Wunschkennzeichen
            txtWunschkennzeichen.Text = objHaendler.Wunschkennzeichen
            txtReserviertAuf.Text = objHaendler.ReserviertAuf
            txtReserviertAuf.Text = objHaendler.ReserviertAuf
            txtVersicherungstraeger.Text = objHaendler.Versicherungstraeger


            'evbfeld aufsplitten
            Dim split() As String
            If Not objHaendler.evbNummer Is Nothing Then
                split = objHaendler.evbNummer.Split(" ")
                txtEVB.Text = split(0)
                'es kann nicht sein das nur ein Datum vorhanden ist
                If split.Length > 1 Then
                    txtDatumVon.Text = HelpProcedures.MakeDateStandard(split(1))
                    txtDatumbis.Text = HelpProcedures.MakeDateStandard(split(2))
                End If

            Else
                txtEVB.Text = String.Empty
                txtDatumVon.Text = String.Empty
                txtDatumbis.Text = String.Empty
            End If


            txtEmpfaengerName1.Text = objHaendler.EmpfaengerName1
            txtEmpfaengerName2.Text = objHaendler.EmpfaengerName2
            txtEmpfaengerOrt.Text = objHaendler.EmpfaengerOrt
            txtEmpfaengerPLZ.Text = objHaendler.EmpfaengerPLZ
            txtEmpfaengerStrasse.Text = objHaendler.EmpfaengerStrasse
            txtEmpfaengerHausnr.Text = objHaendler.EmpfaengerHausnr


            If IsDate(objHaendler.DurchfuehrungsDatum) AndAlso Not objHaendler.DurchfuehrungsDatum < Today Then
                txtDurchfuehrungsDatum.Text = objHaendler.DurchfuehrungsDatum.ToShortDateString
            Else
                txtDurchfuehrungsDatum.Text = Today.ToShortDateString
            End If

            txtBemerkung.Text = objHaendler.Bemerkung

            HideAll()
            FillDates()

            Dim item As New ListItem()
            item.Value = "kein"
            item.Text = "-- keine Auswahl --"
            ddlDienstleistung.Items.Add(item)

            item = New ListItem()
            item.Value = "2052"
            item.Text = "Ummeldung innerorts"
            ddlDienstleistung.Items.Add(item)

            item = New ListItem()
            item.Value = "572"
            item.Text = "Ummeldung ausserorts"
            ddlDienstleistung.Items.Add(item)

            item = New ListItem()
            item.Value = "1294"
            item.Text = "Umkennzeichnung"
            ddlDienstleistung.Items.Add(item)

            item = New ListItem()
            item.Value = "2037"
            item.Text = "Ersatzfahrzeugschein"
            ddlDienstleistung.Items.Add(item)

            item = New ListItem()
            item.Value = "1380-1"
            item.Text = "Technischer Eintrag"
            ddlDienstleistung.Items.Add(item)

            item = New ListItem()
            item.Value = "1380-2"
            item.Text = "Korrektur wegen Fehleintrag"
            ddlDienstleistung.Items.Add(item)

            item = New ListItem()
            item.Value = "1380-3"
            item.Text = "Abmeldung"
            ddlDienstleistung.Items.Add(item)

            item = New ListItem()
            item.Value = "1462"
            item.Text = "Wiederzulassung"
            ddlDienstleistung.Items.Add(item)
        End If

        If ddlDienstleistung.SelectedItem.Value.ToString = "kein" Then
            cmdSave.Enabled = False
        Else
            cmdSave.Enabled = True
        End If
    End Sub

    Private Sub HideAll()
        trNeuerHalter1.Visible = False
        trNeuerHalter2.Visible = False
        trNeuerStandort1.Visible = False
        trNeuerStandort2.Visible = False
        lnkStandortAnzeige.Text = "Anzeigen"
        trWunschkennzeichen.Visible = False
        trReserviertAuf.Visible = False
        trVersicherungstraeger.Visible = False
        trGueltigkeit.Visible = False
        trEmpfaenger1.Visible = False
        trEmpfaenger2.Visible = False
        trDurchfuehrungsdatum.Visible = False
        trHinweis.Visible = False
        trBemerkung.Visible = False
        
    End Sub

    Private Sub FillDates()
        Dim tmpDataView As DataView = objHaendler.Fahrzeuge.DefaultView

        tmpDataView.RowFilter = "MANDT = '99'"
        Dim intFahrzeugBriefe As Int32 = tmpDataView.Count

        'Alt: Nur ein Fahrzeug erlaubt
        'If Not intFahrzeugBriefe = 1 Then
        '    'Schrott! Weg hier!
        '    Response.Redirect("Change81.aspx?AppID=" & Session("AppID").ToString)
        'Else
        '    lblKreis.Text = Split(CStr(tmpDataView(0)("LICENSE_NUM")), "-")(0)
        'End If

        'IT 1241: Mehrere Fahrzeuge möglich
        If intFahrzeugBriefe = 0 Then
            'Schrott! Weg hier!
            Response.Redirect("Change81.aspx?AppID=" & Session("AppID").ToString)
        Else
            lblKreis.Text = Split(CStr(tmpDataView(0)("LICENSE_NUM")), "-")(0)
        End If

        tmpDataView.RowFilter = ""
        
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        
        objHaendler.Auftragsgrund = ddlDienstleistung.SelectedItem.Value.ToString
        objHaendler.BeauftragungKlartext = ddlDienstleistung.SelectedItem.Text.ToString

        If trNeuerHalter2.Visible Then

            checkEingaben(txtHalterName1)
            checkEingaben(txtHalterOrt)
            checkEingaben(txtHalterPLZ)
            checkEingaben(txtHalterStrasse)
            checkEingaben(txtHalterHausnr)

            If Not txtHalterPLZ.Text.Length = 0 Then
                If Not IsNumeric(txtHalterPLZ.Text) OrElse Not txtHalterPLZ.Text.Length = 5 Then
                    'Throw New Exception("PLZ ist ungültig")
                    lblError.Text = "PLZ ist ungültig"
                    Exit Sub
                End If
            End If

            If Not txtHalterHausnr.Text.Length = 0 Then
                If Not IsNumeric(txtHalterHausnr.Text) Then
                    'Throw New Exception("Hausnummerist ungültig")
                    lblError.Text = "Hausnummerist ungültig"
                    Exit Sub
                End If
            End If

            objHaendler.HalterName1 = txtHalterName1.Text
            objHaendler.HalterName2 = txtHalterName2.Text
            objHaendler.HalterOrt = txtHalterOrt.Text
            objHaendler.HalterPLZ = txtHalterPLZ.Text
            objHaendler.HalterStrasse = txtHalterStrasse.Text
            objHaendler.HalterHausnr = txtHalterHausnr.Text
        Else
            txtHalterName1.Text = ""
            txtHalterName2.Text = ""
            txtHalterOrt.Text = ""
            txtHalterPLZ.Text = ""
            txtHalterStrasse.Text = ""
            txtHalterHausnr.Text = ""
        End If

        If trNeuerStandort2.Visible Then
            objHaendler.StandortName1 = txtStandortName1.Text
            objHaendler.StandortName2 = txtStandortName2.Text
            objHaendler.StandortOrt = txtStandortOrt.Text
            objHaendler.StandortPLZ = txtStandortPLZ.Text
            objHaendler.StandortStrasse = txtStandortStrasse.Text
            objHaendler.StandortHausnr = txtStandortHausnr.Text
        Else
            txtStandortName1.Text = ""
            txtStandortName2.Text = ""
            txtStandortOrt.Text = ""
            txtStandortPLZ.Text = ""
            txtStandortStrasse.Text = ""
            txtStandortHausnr.Text = ""
        End If

        If trWunschkennzeichen.Visible Then
            objHaendler.Kreis = txtKreis.Text
            objHaendler.Wunschkennzeichen = txtWunschkennzeichen.Text
        Else
            txtKreis.Text = ""
            txtWunschkennzeichen.Text = ""
        End If

        If trReserviertAuf.Visible Then
            objHaendler.ReserviertAuf = txtReserviertAuf.Text
        Else
            txtReserviertAuf.Text = ""
        End If

        If trVersicherungstraeger.Visible Then
            'prüfung
            Dim strFehlermeldung As String = ""
            checkEingaben(txtEVB)

            If Not txtEVB.Text.Length = 0 Then

                If Not txtEVB.Text.Length < 7 AndAlso HelpProcedures.isAlphaNumeric(txtEVB.Text) AndAlso HelpProcedures.checkDate(txtDatumVon, txtDatumbis, strFehlermeldung, True) Then
                    objHaendler.Versicherungstraeger = txtVersicherungstraeger.Text
                    'ein Datumswert kann niemals alleine gefüllt sein
                    If Not txtDatumVon.Text Is String.Empty Then
                        objHaendler.evbNummer = txtEVB.Text & " " & HelpProcedures.MakeDateSAP(txtDatumVon.Text) & " " & HelpProcedures.MakeDateSAP(txtDatumbis.Text)
                    Else
                        objHaendler.evbNummer = txtEVB.Text
                    End If
                Else
                    If txtEVB.Text.Length < 7 OrElse Not HelpProcedures.isAlphaNumeric(txtEVB.Text) Then
                        lblError.Text = "EVB-Nummer ungültig"
                        Exit Sub
                    Else
                        lblError.Text = strFehlermeldung
                    End If
                    Exit Sub
                End If

            End If

        End If


        If trEmpfaenger2.Visible Then
            
            checkEingaben(txtEmpfaengerName1)
            checkEingaben(txtEmpfaengerOrt)
            checkEingaben(txtEmpfaengerPLZ)
            checkEingaben(txtEmpfaengerStrasse)
            checkEingaben(txtEmpfaengerHausnr)

            If Not txtEmpfaengerPLZ.Text.Length = 0 Then
                If Not IsNumeric(txtEmpfaengerPLZ.Text) OrElse Not txtEmpfaengerPLZ.Text.Length = 5 Then
                    'Throw New Exception("PLZ ist ungültig")
                    lblError.Text = "PLZ ist ungültig"
                    Exit Sub
                End If
            End If

            If Not txtEmpfaengerHausnr.Text.Length = 0 Then
                If Not IsNumeric(txtEmpfaengerHausnr.Text) Then
                    'Throw New Exception("Hausnummerist ungültig")
                    lblError.Text = "Hausnummerist ungültig"
                    Exit Sub
                End If
            End If
            
            objHaendler.EmpfaengerName1 = txtEmpfaengerName1.Text
            objHaendler.EmpfaengerName2 = txtEmpfaengerName2.Text
            objHaendler.EmpfaengerOrt = txtEmpfaengerOrt.Text
            objHaendler.EmpfaengerPLZ = txtEmpfaengerPLZ.Text
            objHaendler.EmpfaengerStrasse = txtEmpfaengerStrasse.Text
            objHaendler.EmpfaengerHausnr = txtEmpfaengerHausnr.Text
        Else
            txtEmpfaengerName1.Text = ""
            txtEmpfaengerName2.Text = ""
            txtEmpfaengerOrt.Text = ""
            txtEmpfaengerPLZ.Text = ""
            txtEmpfaengerStrasse.Text = ""
            txtEmpfaengerHausnr.Text = ""
        End If

        If trDurchfuehrungsdatum.Visible Then
            If checkdatum(txtDurchfuehrungsDatum.Text) Then
                objHaendler.DurchfuehrungsDatum = CDate(txtDurchfuehrungsDatum.Text)
            Else
                lblError.Text = "Ungültiges Durchführungsdatum!" 'Throw New Exception()
                Exit Sub
            End If
        Else
            txtDurchfuehrungsDatum.Text = Format(objHaendler.SuggestionDay, "dd.MM.yyyy")
        End If

        If trBemerkung.Visible Then
            objHaendler.Bemerkung = txtBemerkung.Text
        Else
            txtBemerkung.Text = ""
        End If

        If mError Then
            lblError.Text = "Bitte prüfen sie fehlende Eingaben." 'Throw New Exception("Bitte prüfen sie fehlende Eingaben")
            Exit Sub
        Else
            Response.Redirect("Change81_4.aspx?AppID=" & Session("AppID").ToString)
        End If

        Session("objHaendler") = objHaendler

    End Sub

    Private Function checkdatum(ByVal datum As Date) As Boolean

        '§§§ JVE 28.08.2006: Diese Prüfung muß natürlich rein!

        '...Datum < Tagesdatum?
        If datum < Date.Today Then

            lblError.Text = "Durchführungsdatum darf nicht in der Vergangenheit liegen."
            Return False
        End If


        '...Wochenende ?
        If (datum.DayOfWeek = DayOfWeek.Saturday) Or (datum.DayOfWeek = DayOfWeek.Sunday) Then

            lblError.Text = "Durchführungsdatum ungültig (Wochenende)."
            Return False
        End If


        Return True
    End Function

    'Private Sub rbAbmeldung_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    'End Sub

    Private Sub checkEingaben(ByRef tmpTXT As TextBox)

        If tmpTXT.Text.Trim(" "c).Length = 0 Then
            tmpTXT.BorderWidth = 1
            tmpTXT.BorderColor = Color.Red
            tmpTXT.BorderStyle = BorderStyle.Solid
            mError = True
        Else
            tmpTXT.BorderStyle = BorderStyle.NotSet
            tmpTXT.BorderColor = Nothing
            tmpTXT.BorderWidth = Nothing
        End If
    End Sub
    
    Private Sub ddlDienstleistung_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDienstleistung.SelectedIndexChanged
        Select Case ddlDienstleistung.SelectedItem.Text.ToString
            Case "-- keine Auswahl --"
                HideAll()
            Case "Ummeldung innerorts"
                trNeuerHalter1.Visible = True
                trNeuerHalter2.Visible = True

                'Keine Anzeige mehr!!!
                'trNeuerStandort1.Visible = True

                trWunschkennzeichen.Visible = True
                trReserviertAuf.Visible = True
                trVersicherungstraeger.Visible = True
                trGueltigkeit.Visible = True
                trEmpfaenger1.Visible = True
                trEmpfaenger2.Visible = True
                trDurchfuehrungsdatum.Visible = True
                trBemerkung.Visible = True

                'Kreiskennzeichen vorbelegt und nicht änderbar
                txtKreis.Text = lblKreis.Text
                txtKreis.Visible = False
                lblKreis.Visible = True
            Case "Ummeldung ausserorts"
                HideAll()
                trNeuerHalter1.Visible = True
                trNeuerHalter2.Visible = True

                'Keine Anzeige mehr
                'trNeuerStandort1.Visible = True

                trWunschkennzeichen.Visible = True
                trReserviertAuf.Visible = True
                trVersicherungstraeger.Visible = True
                trGueltigkeit.Visible = True
                trEmpfaenger1.Visible = True
                trEmpfaenger2.Visible = True
                trDurchfuehrungsdatum.Visible = True
                trBemerkung.Visible = True

                'Kreiskennzeichen vorbelegt und änderbar
                txtKreis.Text = lblKreis.Text
                txtKreis.Visible = True
                lblKreis.Visible = False
            Case "Umkennzeichnung"
                HideAll()
                trWunschkennzeichen.Visible = True
                trReserviertAuf.Visible = True
                trEmpfaenger1.Visible = True
                trEmpfaenger2.Visible = True
                trDurchfuehrungsdatum.Visible = True
                trBemerkung.Visible = True

                'Kreiskennzeichen vorbelegt und nicht änderbar
                txtKreis.Text = lblKreis.Text
                txtKreis.Visible = False
                lblKreis.Visible = True
            Case "Ersatzfahrzeugschein"
                HideAll()
                trEmpfaenger1.Visible = True
                trEmpfaenger2.Visible = True
                trDurchfuehrungsdatum.Visible = True
                trHinweis.Visible = True
                lblHinweis.Text = "Bitte Verlusterklärung / eidestattliche Versicherung im Original an DAD senden."
                trBemerkung.Visible = True

                'Ohne Kreiskennzeichen
                txtKreis.Text = ""
            Case "Technischer Eintrag"
                HideAll()
                trDurchfuehrungsdatum.Visible = True
                trHinweis.Visible = True
                lblHinweis.Text = "Bitte Gutachten im Original an DAD senden."
                trBemerkung.Visible = True

                'Ohne Kreiskennzeichen
                txtKreis.Text = ""
            Case "Korrektur wegen Fehleintrag"
                HideAll()
                trDurchfuehrungsdatum.Visible = True
                trBemerkung.Visible = True

                'Ohne Kreiskennzeichen
                txtKreis.Text = ""
            Case "Abmeldung"
                HideAll()
                trDurchfuehrungsdatum.Visible = True
                trHinweis.Visible = True
                lblHinweis.Text = "Bitte ZB1 im Original und Kennzeichen an DAD senden."
                trBemerkung.Visible = True

                'Ohne Kreiskennzeichen
                txtKreis.Text = ""
            Case "Wiederzulassung"
                HideAll()
                trVersicherungstraeger.Visible = True
                trGueltigkeit.Visible = True
                trEmpfaenger1.Visible = True
                trEmpfaenger2.Visible = True
                trDurchfuehrungsdatum.Visible = True
                trBemerkung.Visible = True

                'Ohne Kreiskennzeichen
                txtKreis.Text = ""
        End Select
        'hinweis tr eiblenden wenn tr Gültigkeit sichtbar
        trHinweisGueltigkeit.Visible = trGueltigkeit.Visible
    End Sub

    Private Sub lnkStandortAnzeige_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkStandortAnzeige.Click
        If lnkStandortAnzeige.Text = "Anzeigen" Then
            lnkStandortAnzeige.Text = "Verbergen"
            trNeuerStandort2.Visible = True
        Else
            lnkStandortAnzeige.Text = "Anzeigen"
            trNeuerStandort2.Visible = False
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
' $History: Change81_3.aspx.vb $
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 5.06.09    Time: 15:58
' Updated in $/CKAG/Components/ComCommon/SonstigeDL
' mögliche try catches entfernt
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 4.05.09    Time: 11:50
' Updated in $/CKAG/Components/ComCommon/SonstigeDL
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 30.03.09   Time: 15:39
' Updated in $/CKAG/Components/ComCommon/SonstigeDL
' ITA 2760 testfertig
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 30.03.09   Time: 11:45
' Updated in $/CKAG/Components/ComCommon/SonstigeDL
' ITA 2760
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 27.03.09   Time: 14:28
' Updated in $/CKAG/Components/ComCommon/SonstigeDL
' ITa 2760 unfertig
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 9.10.08    Time: 15:06
' Created in $/CKAG/Components/ComCommon/SonstigeDL
' 
' *****************  Version 1  *****************
