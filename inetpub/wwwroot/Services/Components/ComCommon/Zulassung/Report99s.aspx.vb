Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security

Partial Public Class Report99s
    Inherits System.Web.UI.Page
    Private m_User As User
    Private m_App As App
    Private objSuche As Report_99s

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        FormAuth(Me, m_User)
        m_App = New App(m_User)
        GetAppIDFromQueryString(Me)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

        If Request.QueryString("AppID").Length > 0 Then
            Session("AppID") = Request.QueryString("AppID").ToString
            lnkEinzug.NavigateUrl = "Report99_2s.aspx?AppID=" & Session("AppID")
        Else
            lnkEinzug.NavigateUrl = String.Empty
        End If

    End Sub

    Private Sub DoSubmit()
        Session("lnkExcel") = ""

        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
        Dim objSuche As New Report_99s(m_User, m_App, strFileName)

        objSuche.PKennzeichen = txtKennzeichen.Text
        objSuche.Fill(Me.Page, Session("AppID").ToString, Session.SessionID.ToString)

        Session("objSuche") = objSuche

        If Not objSuche.Status = 0 Then
            lblError.Text = "Fehler: " & objSuche.Message
        Else
            If objSuche.Result.Rows.Count = 0 Then
                lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                txtKennzeichen.Text = ""
            Else
                FillForm()
            End If
        End If


    End Sub

    Private Sub FillForm()
        objSuche = CType(Session("objSuche"), Report_99s)

        Dim SelectRow() As DataRow
        Dim resultRow As DataRow

        If objSuche.Result.Rows.Count = 1 Then
            resultRow = objSuche.Result.Rows(0)
        Else
            SelectRow = objSuche.Result.Select("Zkba2='00'")
            resultRow = SelectRow(0)
        End If

        Result.Visible = True

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

        'Privat Umschreibung
        Label10.Text = CType(resultRow("PUMSCHR_BRIEF"), String)
        Label11.Text = CType(resultRow("PUMSCHR_SCHEIN"), String)
        Label12.Text = CType(resultRow("PUMSCHR_COC"), String)
        Label13.Text = CType(resultRow("PUMSCHR_DECK"), String)
        Label14.Text = CType(resultRow("PUMSCHR_VOLLM"), String)
        Label15.Text = CType(resultRow("PUMSCHR_AUSW"), String)
        Label16.Text = CType(resultRow("PUMSCHR_GEWERB"), String)
        Label17.Text = CType(resultRow("PUMSCHR_HANDEL"), String)
        Label18.Text = CType(resultRow("PUMSCHR_LAST"), String)
        Label19.Text = CType(resultRow("PUMSCHR_BEM"), String)

        'Privat Umkennzeichnung
        Label20.Text = CType(resultRow("PUMK_BRIEF"), String)
        Label21.Text = CType(resultRow("PUMK_SCHEIN"), String)
        Label22.Text = CType(resultRow("PUMK_COC"), String)
        Label23.Text = CType(resultRow("PUMK_DECK"), String)
        Label24.Text = CType(resultRow("PUMK_VOLLM"), String)
        Label25.Text = CType(resultRow("PUMK_AUSW"), String)
        Label26.Text = CType(resultRow("PUMK_GEWERB"), String)
        Label27.Text = CType(resultRow("PUMK_HANDEL"), String)
        Label28.Text = CType(resultRow("PUMK_LAST"), String)
        Label29.Text = CType(resultRow("PUMK_BEM"), String)

        'Privat Ersatzfahrzeugschein
        Label30.Text = CType(resultRow("PERS_BRIEF"), String)
        Label31.Text = CType(resultRow("PERS_SCHEIN"), String)
        Label32.Text = CType(resultRow("PERS_COC"), String)
        Label33.Text = CType(resultRow("PERS_DECK"), String)
        Label34.Text = CType(resultRow("PERS_VOLLM"), String)
        Label35.Text = CType(resultRow("PERS_AUSW"), String)
        Label36.Text = CType(resultRow("PERS_GEWERB"), String)
        Label37.Text = CType(resultRow("PERS_HANDEL"), String)
        Label38.Text = CType(resultRow("PERS_LAST"), String)
        Label39.Text = CType(resultRow("PERS_BEM"), String)

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

        'Unternehmen Umschreibung
        Label50.Text = CType(resultRow("UUMSCHR_BRIEF"), String)
        Label51.Text = CType(resultRow("UUMSCHR_SCHEIN"), String)
        Label52.Text = CType(resultRow("UUMSCHR_COC"), String)
        Label53.Text = CType(resultRow("UUMSCHR_DECK"), String)
        Label54.Text = CType(resultRow("UUMSCHR_VOLLM"), String)
        Label55.Text = CType(resultRow("UUMSCHR_AUSW"), String)
        Label56.Text = CType(resultRow("UUMSCHR_GEWERB"), String)
        Label57.Text = CType(resultRow("UUMSCHR_HANDEL"), String)
        Label58.Text = CType(resultRow("UUMSCHR_LAST"), String)
        Label59.Text = CType(resultRow("UUMSCHR_BEM"), String)

        'Unternehmen Umkennzeichnung
        Label60.Text = CType(resultRow("UUMK_BRIEF"), String)
        Label61.Text = CType(resultRow("UUMK_SCHEIN"), String)
        Label62.Text = CType(resultRow("UUMK_COC"), String)
        Label63.Text = CType(resultRow("UUMK_DECK"), String)
        Label64.Text = CType(resultRow("UUMK_VOLLM"), String)
        Label65.Text = CType(resultRow("UUMK_AUSW"), String)
        Label66.Text = CType(resultRow("UUMK_GEWERB"), String)
        Label67.Text = CType(resultRow("UUMK_HANDEL"), String)
        Label68.Text = CType(resultRow("UUMK_LAST"), String)
        Label69.Text = CType(resultRow("UUMK_BEM"), String)

        'Unternehmen Ersatzfahrzeugschein
        Label70.Text = CType(resultRow("UERS_BRIEF"), String)
        Label71.Text = CType(resultRow("UERS_SCHEIN"), String)
        Label72.Text = CType(resultRow("UERS_COC"), String)
        Label73.Text = CType(resultRow("UERS_DECK"), String)
        Label74.Text = CType(resultRow("UERS_VOLLM"), String)
        Label75.Text = CType(resultRow("UERS_AUSW"), String)
        Label76.Text = CType(resultRow("UERS_GEWERB"), String)
        Label77.Text = CType(resultRow("UERS_HANDEL"), String)
        Label78.Text = CType(resultRow("UERS_LAST"), String)
        Label79.Text = CType(resultRow("UERS_BEM"), String)

        cmdWunsch.Enabled = True
        cmdFormulare.Enabled = True
        cmdGebuehr.Enabled = True
        cmdAmt.Enabled = True
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
    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        ClearForm()
        If (txtKennzeichen.Text.Trim <> String.Empty) Then
            DoSubmit()
        Else
            lblError.Text = "Bitte ein Ortskennzeichen eingeben."
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub cmdWunsch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdWunsch.Click
        ClickLink("URL")
    End Sub

    Private Sub cmdAmt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAmt.Click

        objSuche = CType(Session("objSuche"), Report_99s)
        Dim sUrl As String
        Dim resultRow As DataRow
        Dim sAmt As String
        lblInfo.Text = ""
        resultRow = objSuche.Result.Rows(0)

        sUrl = "http://" & CType(resultRow("STVALN"), String)
        sAmt = CType(resultRow("ZKFZKZ"), String)

        If Not sUrl.Length = 0 Then
            Dim popupBuilder As String

            popupBuilder = "<script languange=""Javascript"">"
            popupBuilder += ControlChars.CrLf
            popupBuilder += "window.open('" & sUrl & "', 'POPUP', 'dependent=yes,location=yes,menubar=no,resizable=yes,scrollbars=yes,status=no,toolbar=no');"
            popupBuilder += ControlChars.CrLf
            popupBuilder += "</script>"
            popupBuilder += ControlChars.CrLf
            ClientScript.RegisterClientScriptBlock(Me.GetType(), "POPUP", popupBuilder)

            lblInfo.Text = "Sollte sich kein neues Browserfenster öffnen, deaktivieren Sie bitte Ihren Popupblockler!"
        Else
            lblInfo.Text = "Das Straßenverkehrsamt für das amtliche Kennzeichen  " & sAmt & "  bietet keine Weblink hierfür an. "
        End If

    End Sub

    Private Sub cmdFormulare_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFormulare.Click
        ClickLink("STVALNFORM")
    End Sub

    Private Sub cmdGebuehr_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdGebuehr.Click
        ClickLink("STVALNGEB")
    End Sub

    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        Response.Redirect("/Services/(S(" & Session.SessionID & "))/Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub

    Private Sub ClickLink(ByVal RowName As String)
        objSuche = CType(Session("objSuche"), Report_99s)
        Dim sUrl As String
        Dim resultRow As DataRow
        Dim sTempUrl As String
        Dim sAmt As String
        lblInfo.Text = ""
        resultRow = objSuche.Result.Rows(0)
        sTempUrl = Left(CType(resultRow(RowName), String), 7)
        sAmt = CType(resultRow("ZKFZKZ"), String)
        If Not sTempUrl.Length = 0 Then
            If Not sTempUrl = "http://" Then
                sUrl = "http://" & CType(resultRow(RowName), String)
            Else
                sUrl = CType(resultRow(RowName), String)
            End If


            Dim guid As System.Guid = System.Guid.NewGuid

            Dim popupBuilder As String

            popupBuilder = "<script languange=""Javascript"">"
            popupBuilder += ControlChars.CrLf
            popupBuilder += "window.open('" & sUrl & "', 'POPUP', 'dependent=yes,location=yes,menubar=no,resizable=yes,scrollbars=yes,status=no,toolbar=no');"
            popupBuilder += ControlChars.CrLf
            popupBuilder += "</script>"
            popupBuilder += ControlChars.CrLf
            ClientScript.RegisterClientScriptBlock(Me.GetType, "POPUP", popupBuilder)
            ClientScript.RegisterClientScriptBlock(Me.GetType(), "POPUP", popupBuilder.ToString)
        Else
            lblInfo.Text = "Das Straßenverkehrsamt für das amtliche Kennzeichen " & sAmt & " bietet keine Weblink hierfür an. <br>" & _
            "Möchten Sie auf die Standardstartseite dies Verkehrsamts wechseln, so klicken Sie bitte auf den Link Amt. "
        End If

    End Sub
End Class