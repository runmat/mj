Imports System
Imports System.Data
Imports KBS.KBS_BASE

Public Class Report99
    Inherits Page

    Private mObjKasse As Kasse
    Private objSuche As ComCommon

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        FormAuth(Me)
        lblError.Text = ""

        If mObjKasse Is Nothing Then
            If Not Session("mKasse") Is Nothing Then
                mObjKasse = CType(Session("mKasse"), Kasse)
            Else
                Throw New Exception("benötigtes Session Objekt nicht vorhanden")
            End If
        End If

        Title = lblHead.Text

        If Not IsPostBack Then
            lnkEinzug.NavigateUrl = "Report99_2.aspx"
        End If
    End Sub

    Private Sub DoSubmit()
        Session("lnkExcel") = ""

        objSuche = New ComCommon(mObjKasse)

        objSuche.PKennzeichen = txtKennzeichen.Text.ToUpper
        objSuche.FillDokumenteERP()

        Session("objSuche") = objSuche

        If objSuche.ErrorOccured Then
            lblError.Text = "Fehler: " & objSuche.ErrorMessage
        Else
            If objSuche.Dokumente.Rows.Count = 0 Then
                lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                txtKennzeichen.Text = ""
                Result.Visible = False
            Else
                FillForm()
            End If
        End If

    End Sub

    Private Sub FillForm()
        objSuche = CType(Session("objSuche"), ComCommon)

        Dim SelectRow() As DataRow
        Dim resultRow As DataRow

        If objSuche.Dokumente.Rows.Count = 1 Then
            resultRow = objSuche.Dokumente.Rows(0)
        Else
            SelectRow = objSuche.Dokumente.Select("Zkba2='00'")
            resultRow = SelectRow(0)
        End If

        'Privat Zulassung
        Label00.Text = resultRow("PZUL_BRIEF").ToString
        Label01.Text = resultRow("PZUL_SCHEIN").ToString
        Label02.Text = resultRow("PZUL_COC").ToString
        Label03.Text = resultRow("PZUL_DECK").ToString
        Label04.Text = resultRow("PZUL_VOLLM").ToString
        Label05.Text = resultRow("PZUL_AUSW").ToString
        Label06.Text = resultRow("PZUL_GEWERB").ToString
        Label07.Text = resultRow("PZUL_HANDEL").ToString
        Label08.Text = resultRow("PZUL_LAST").ToString
        Label09.Text = resultRow("PZUL_BEM").ToString

        'Privat Umschreibung
        Label10.Text = resultRow("PUMSCHR_BRIEF").ToString
        Label11.Text = resultRow("PUMSCHR_SCHEIN").ToString
        Label12.Text = resultRow("PUMSCHR_COC").ToString
        Label13.Text = resultRow("PUMSCHR_DECK").ToString
        Label14.Text = resultRow("PUMSCHR_VOLLM").ToString
        Label15.Text = resultRow("PUMSCHR_AUSW").ToString
        Label16.Text = resultRow("PUMSCHR_GEWERB").ToString
        Label17.Text = resultRow("PUMSCHR_HANDEL").ToString
        Label18.Text = resultRow("PUMSCHR_LAST").ToString
        Label19.Text = resultRow("PUMSCHR_BEM").ToString

        'Privat Umkennzeichnung
        Label20.Text = resultRow("PUMK_BRIEF").ToString
        Label21.Text = resultRow("PUMK_SCHEIN").ToString
        Label22.Text = resultRow("PUMK_COC").ToString
        Label23.Text = resultRow("PUMK_DECK").ToString
        Label24.Text = resultRow("PUMK_VOLLM").ToString
        Label25.Text = resultRow("PUMK_AUSW").ToString
        Label26.Text = resultRow("PUMK_GEWERB").ToString
        Label27.Text = resultRow("PUMK_HANDEL").ToString
        Label28.Text = resultRow("PUMK_LAST").ToString
        Label29.Text = resultRow("PUMK_BEM").ToString

        'Privat Ersatzfahrzeugschein
        Label30.Text = resultRow("PERS_BRIEF").ToString
        Label31.Text = resultRow("PERS_SCHEIN").ToString
        Label32.Text = resultRow("PERS_COC").ToString
        Label33.Text = resultRow("PERS_DECK").ToString
        Label34.Text = resultRow("PERS_VOLLM").ToString
        Label35.Text = resultRow("PERS_AUSW").ToString
        Label36.Text = resultRow("PERS_GEWERB").ToString
        Label37.Text = resultRow("PERS_HANDEL").ToString
        Label38.Text = resultRow("PERS_LAST").ToString
        Label39.Text = resultRow("PERS_BEM").ToString

        'Unternehmen Zulassung
        Label40.Text = resultRow("UZUL_BRIEF").ToString
        Label41.Text = resultRow("UZUL_SCHEIN").ToString
        Label42.Text = resultRow("UZUL_COC").ToString
        Label43.Text = resultRow("UZUL_DECK").ToString
        Label44.Text = resultRow("UZUL_VOLLM").ToString
        Label45.Text = resultRow("UZUL_AUSW").ToString
        Label46.Text = resultRow("UZUL_GEWERB").ToString
        Label47.Text = resultRow("UZUL_HANDEL").ToString
        Label48.Text = resultRow("UZUL_LAST").ToString
        Label49.Text = resultRow("UZUL_BEM").ToString

        'Unternehmen Umschreibung
        Label50.Text = resultRow("UUMSCHR_BRIEF").ToString
        Label51.Text = resultRow("UUMSCHR_SCHEIN").ToString
        Label52.Text = resultRow("UUMSCHR_COC").ToString
        Label53.Text = resultRow("UUMSCHR_DECK").ToString
        Label54.Text = resultRow("UUMSCHR_VOLLM").ToString
        Label55.Text = resultRow("UUMSCHR_AUSW").ToString
        Label56.Text = resultRow("UUMSCHR_GEWERB").ToString
        Label57.Text = resultRow("UUMSCHR_HANDEL").ToString
        Label58.Text = resultRow("UUMSCHR_LAST").ToString
        Label59.Text = resultRow("UUMSCHR_BEM").ToString

        'Unternehmen Umkennzeichnung
        Label60.Text = resultRow("UUMK_BRIEF").ToString
        Label61.Text = resultRow("UUMK_SCHEIN").ToString
        Label62.Text = resultRow("UUMK_COC").ToString
        Label63.Text = resultRow("UUMK_DECK").ToString
        Label64.Text = resultRow("UUMK_VOLLM").ToString
        Label65.Text = resultRow("UUMK_AUSW").ToString
        Label66.Text = resultRow("UUMK_GEWERB").ToString
        Label67.Text = resultRow("UUMK_HANDEL").ToString
        Label68.Text = resultRow("UUMK_LAST").ToString
        Label69.Text = resultRow("UUMK_BEM").ToString

        'Unternehmen Ersatzfahrzeugschein
        Label70.Text = resultRow("UERS_BRIEF").ToString
        Label71.Text = resultRow("UERS_SCHEIN").ToString
        Label72.Text = resultRow("UERS_COC").ToString
        Label73.Text = resultRow("UERS_DECK").ToString
        Label74.Text = resultRow("UERS_VOLLM").ToString
        Label75.Text = resultRow("UERS_AUSW").ToString
        Label76.Text = resultRow("UERS_GEWERB").ToString
        Label77.Text = resultRow("UERS_HANDEL").ToString
        Label78.Text = resultRow("UERS_LAST").ToString
        Label79.Text = resultRow("UERS_BEM").ToString

        cmdWunsch.Enabled = True
        cmdFormulare.Enabled = True
        cmdGebuehr.Enabled = True
        cmdAmt.Enabled = True
        Result.Visible = True
        If Label05.Text.Length = 0 Then
            lblError.Text = "Es konnten keine Dokumentenanforderungen gefunden werden, <br> benutzen Sie bitte die bereitgestellten Links des Zulassungskreises! "
        End If

    End Sub

    Private Sub ClearForm()

        Session("objSuche") = Nothing
        'Privat Zulassung
        Label00.Text = ""
        Label01.Text = ""
        Label02.Text = ""
        Label03.Text = ""
        Label04.Text = ""
        Label05.Text = ""
        Label06.Text = ""
        Label07.Text = ""
        Label08.Text = ""
        Label09.Text = ""

        'Privat Umschreibung
        Label10.Text = ""
        Label11.Text = ""
        Label12.Text = ""
        Label13.Text = ""
        Label14.Text = ""
        Label15.Text = ""
        Label16.Text = ""
        Label17.Text = ""
        Label18.Text = ""
        Label19.Text = ""

        'Privat Umkennzeichnung
        Label20.Text = ""
        Label21.Text = ""
        Label22.Text = ""
        Label23.Text = ""
        Label24.Text = ""
        Label25.Text = ""
        Label26.Text = ""
        Label27.Text = ""
        Label28.Text = ""
        Label29.Text = ""

        'Privat Ersatzfahrzeugschein
        Label30.Text = ""
        Label31.Text = ""
        Label32.Text = ""
        Label33.Text = ""
        Label34.Text = ""
        Label35.Text = ""
        Label36.Text = ""
        Label37.Text = ""
        Label38.Text = ""
        Label39.Text = ""

        'Unternehmen Zulassung
        Label40.Text = ""
        Label41.Text = ""
        Label42.Text = ""
        Label43.Text = ""
        Label44.Text = ""
        Label45.Text = ""
        Label46.Text = ""
        Label47.Text = ""
        Label48.Text = ""
        Label49.Text = ""

        'Unternehmen Umschreibung
        Label50.Text = ""
        Label51.Text = ""
        Label52.Text = ""
        Label53.Text = ""
        Label54.Text = ""
        Label55.Text = ""
        Label56.Text = ""
        Label57.Text = ""
        Label58.Text = ""
        Label59.Text = ""

        'Unternehmen Umkennzeichnung
        Label60.Text = ""
        Label61.Text = ""
        Label62.Text = ""
        Label63.Text = ""
        Label64.Text = ""
        Label65.Text = ""
        Label66.Text = ""
        Label67.Text = ""
        Label68.Text = ""
        Label69.Text = ""

        'Unternehmen Ersatzfahrzeugschein
        Label70.Text = ""
        Label71.Text = ""
        Label72.Text = ""
        Label73.Text = ""
        Label74.Text = ""
        Label75.Text = ""
        Label76.Text = ""
        Label77.Text = ""
        Label78.Text = ""
        Label79.Text = ""

        cmdWunsch.Enabled = False
        cmdFormulare.Enabled = False
        cmdGebuehr.Enabled = False
        cmdAmt.Enabled = False

    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdCreate.Click
        ClearForm()
        If (txtKennzeichen.Text.Trim <> String.Empty) Then
            DoSubmit()
        Else
            lblError.Text = "Bitte ein Ortskennzeichen eingeben."
        End If
    End Sub

    Private Sub cmdWunsch_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdWunsch.Click
        ClickLink("URL")
    End Sub

    Private Sub cmdAmt_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdAmt.Click

        objSuche = CType(Session("objSuche"), ComCommon)
        Dim sUrl As String
        Dim resultRow As DataRow
        Dim sAmt As String

        resultRow = objSuche.Dokumente.Rows(0)

        sUrl = "http://" & resultRow("STVALN").ToString
        sAmt = resultRow("ZKFZKZ").ToString
        Dim guid As Guid = guid.NewGuid
        If Not sUrl.Length = 0 Then
            Dim popupBuilder As New StringBuilder()
            With popupBuilder
                .Append("<script languange=""Javascript""><!--")
                .Append(ControlChars.CrLf)
                .AppendFormat("window.open('" & sUrl & "', 'POPUP', 'dependent=yes,location=yes,menubar=no,resizable=yes,scrollbars=yes,status=no,toolbar=no');", guid.ToString)
                .Append(ControlChars.CrLf)
                .Append("--></script>")
                .Append(ControlChars.CrLf)
            End With
            ClientScript.RegisterClientScriptBlock(Me.GetType, "POPUP", popupBuilder.ToString)

            lblMessage.Text = "Sollte sich kein neues Browserfenster öffnen, deaktivieren Sie bitte Ihren Popupblockler!"
        Else
            lblMessage.Text = "Das Straßenverkehrsamt für das amtliche Kennzeichen  " & sAmt & "  bietet keine Weblink hierfür an. "
        End If

    End Sub

    Private Sub cmdFormulare_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdFormulare.Click
        ClickLink("STVALNFORM")
    End Sub

    Private Sub cmdGebuehr_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdGebuehr.Click
        ClickLink("STVALNGEB")
    End Sub

    Private Sub ClickLink(ByVal RowName As String)
        objSuche = CType(Session("objSuche"), ComCommon)
        Dim sUrl As String
        Dim resultRow As DataRow
        Dim sTempUrl As String
        Dim sAmt As String

        resultRow = objSuche.Dokumente.Rows(0)
        sTempUrl = Left(resultRow(RowName).ToString, 7)
        sAmt = resultRow("ZKFZKZ").ToString
        If Not sTempUrl.Length = 0 Then
            If Not sTempUrl = "http://" Then
                sUrl = "http://" & CType(resultRow(RowName), String)
            Else
                sUrl = CType(resultRow(RowName), String)
            End If


            Dim guid As Guid = guid.NewGuid

            Dim popupBuilder As New StringBuilder()
            With popupBuilder
                .Append("<script languange=""Javascript""><!--")
                .Append(ControlChars.CrLf)
                .AppendFormat("window.open('" & sUrl & "', 'POPUP', 'dependent=yes,location=yes,menubar=no,resizable=yes,scrollbars=yes,status=no,toolbar=no');", guid.ToString)
                .Append(ControlChars.CrLf)
                .Append("--></script>")
                .Append(ControlChars.CrLf)
            End With

            Dim cstype As Type = Me.GetType()
            ClientScript.RegisterClientScriptBlock(cstype, "POPUP", popupBuilder.ToString)
        Else
            lblMessage.Text = "Das Straßenverkehrsamt für das amtliche Kennzeichen " & sAmt & " bietet keine Weblink hierfür an. <br>" & _
            "Möchten Sie auf die Standardstartseite dies Verkehrsamts wechseln, so klicken Sie bitte auf den Link Amt. "
        End If

    End Sub

    Protected Sub lb_zurueck_Click(sender As Object, e As EventArgs) Handles lb_zurueck.Click
        Response.Redirect("../Selection.aspx")
    End Sub

End Class