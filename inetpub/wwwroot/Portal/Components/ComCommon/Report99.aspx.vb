Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements


Public Class Report99
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

    Private m_User As Security.User
    Private m_App As Security.App

    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents txtKennzeichen As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdCreate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkEinzug As System.Web.UI.WebControls.HyperLink
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
    Protected WithEvents Label11 As System.Web.UI.WebControls.Label
    Protected WithEvents Label10 As System.Web.UI.WebControls.Label
    Protected WithEvents Label12 As System.Web.UI.WebControls.Label
    Protected WithEvents Label13 As System.Web.UI.WebControls.Label
    Protected WithEvents Label14 As System.Web.UI.WebControls.Label
    Protected WithEvents Label15 As System.Web.UI.WebControls.Label
    Protected WithEvents Label16 As System.Web.UI.WebControls.Label
    Protected WithEvents Label17 As System.Web.UI.WebControls.Label
    Protected WithEvents Label18 As System.Web.UI.WebControls.Label
    Protected WithEvents Label19 As System.Web.UI.WebControls.Label
    Protected WithEvents Label21 As System.Web.UI.WebControls.Label
    Protected WithEvents Label20 As System.Web.UI.WebControls.Label
    Protected WithEvents Label22 As System.Web.UI.WebControls.Label
    Protected WithEvents Label23 As System.Web.UI.WebControls.Label
    Protected WithEvents Label24 As System.Web.UI.WebControls.Label
    Protected WithEvents Label25 As System.Web.UI.WebControls.Label
    Protected WithEvents Label26 As System.Web.UI.WebControls.Label
    Protected WithEvents Label27 As System.Web.UI.WebControls.Label
    Protected WithEvents Label28 As System.Web.UI.WebControls.Label
    Protected WithEvents Label29 As System.Web.UI.WebControls.Label
    Protected WithEvents Label31 As System.Web.UI.WebControls.Label
    Protected WithEvents Label30 As System.Web.UI.WebControls.Label
    Protected WithEvents Label32 As System.Web.UI.WebControls.Label
    Protected WithEvents Label33 As System.Web.UI.WebControls.Label
    Protected WithEvents Label34 As System.Web.UI.WebControls.Label
    Protected WithEvents Label35 As System.Web.UI.WebControls.Label
    Protected WithEvents Label36 As System.Web.UI.WebControls.Label
    Protected WithEvents Label37 As System.Web.UI.WebControls.Label
    Protected WithEvents Label38 As System.Web.UI.WebControls.Label
    Protected WithEvents Label39 As System.Web.UI.WebControls.Label
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
    Protected WithEvents Label51 As System.Web.UI.WebControls.Label
    Protected WithEvents Label50 As System.Web.UI.WebControls.Label
    Protected WithEvents Label52 As System.Web.UI.WebControls.Label
    Protected WithEvents Label53 As System.Web.UI.WebControls.Label
    Protected WithEvents Label54 As System.Web.UI.WebControls.Label
    Protected WithEvents Label55 As System.Web.UI.WebControls.Label
    Protected WithEvents Label56 As System.Web.UI.WebControls.Label
    Protected WithEvents Label57 As System.Web.UI.WebControls.Label
    Protected WithEvents Label58 As System.Web.UI.WebControls.Label
    Protected WithEvents Label59 As System.Web.UI.WebControls.Label
    Protected WithEvents Label61 As System.Web.UI.WebControls.Label
    Protected WithEvents Label60 As System.Web.UI.WebControls.Label
    Protected WithEvents Label62 As System.Web.UI.WebControls.Label
    Protected WithEvents Label63 As System.Web.UI.WebControls.Label
    Protected WithEvents Label64 As System.Web.UI.WebControls.Label
    Protected WithEvents Label65 As System.Web.UI.WebControls.Label
    Protected WithEvents Label66 As System.Web.UI.WebControls.Label
    Protected WithEvents Label67 As System.Web.UI.WebControls.Label
    Protected WithEvents Label68 As System.Web.UI.WebControls.Label
    Protected WithEvents Label69 As System.Web.UI.WebControls.Label
    Protected WithEvents Label71 As System.Web.UI.WebControls.Label
    Protected WithEvents Label70 As System.Web.UI.WebControls.Label
    Protected WithEvents Label72 As System.Web.UI.WebControls.Label
    Protected WithEvents Label73 As System.Web.UI.WebControls.Label
    Protected WithEvents Label74 As System.Web.UI.WebControls.Label
    Protected WithEvents Label75 As System.Web.UI.WebControls.Label
    Protected WithEvents Label76 As System.Web.UI.WebControls.Label
    Protected WithEvents Label77 As System.Web.UI.WebControls.Label
    Protected WithEvents Label78 As System.Web.UI.WebControls.Label
    Protected WithEvents Label79 As System.Web.UI.WebControls.Label
    Protected WithEvents cmdWunsch As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdFormulare As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdGebuehr As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbl_Message As System.Web.UI.WebControls.Label
    Protected WithEvents cmdAmt As System.Web.UI.WebControls.LinkButton
    Private objSuche As Kernel.Report_99

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        lbl_Message.Text = ""
        If Request.QueryString("AppID").Length > 0 Then
            Session("AppID") = Request.QueryString("AppID").ToString
            lnkEinzug.NavigateUrl = "Report99_2.aspx?AppID=" & Session("AppID")
        Else
            lnkEinzug.NavigateUrl = String.Empty
        End If

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text
        m_App = New Security.App(m_User)

    End Sub

    Private Sub DoSubmit()
        Session("lnkExcel") = ""

        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
        Dim objSuche As New Kernel.Report_99(m_User, m_App, strFileName)

        objSuche.PKennzeichen = txtKennzeichen.Text
        objSuche.Fill(Session("AppID").ToString, Session.SessionID.ToString)

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
        objSuche = CType(Session("objSuche"), Kernel.Report_99)

        Dim SelectRow() As DataRow
        Dim resultRow As DataRow

        If objSuche.Result.Rows.Count = 1 Then
            resultRow = objSuche.Result.Rows(0)
        Else
            SelectRow = objSuche.Result.Select("Zkba2='00'")
            resultRow = SelectRow(0)
        End If

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
        ClickLink("STVALN")
    End Sub

    Private Sub cmdFormulare_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFormulare.Click
        ClickLink("STVALNFORM")
    End Sub

    Private Sub cmdGebuehr_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdGebuehr.Click
        ClickLink("STVALNGEB")
    End Sub

    Private Sub ClickLink(ByVal RowName As String)
        objSuche = CType(Session("objSuche"), Kernel.Report_99)
        Dim sUrl As String
        Dim resultRow As DataRow
        Dim sAmt As String

        resultRow = objSuche.Result.Rows(0)
        sUrl = CType(resultRow(RowName), String)
        sAmt = CType(resultRow("ZKFZKZ"), String)

        If Not sUrl.Length = 0 Then
            If Not sUrl.StartsWith("http") Then
                sUrl = "http://" & sUrl
            End If

            Dim guid As System.Guid = System.Guid.NewGuid

            Dim popupBuilder As New System.Text.StringBuilder()
            With popupBuilder
                .Append("<script languange=""Javascript""><!--")
                .Append(ControlChars.CrLf)
                .AppendFormat("window.open('" & sUrl & "', 'POPUP', 'dependent=yes,location=yes,menubar=no,resizable=yes,scrollbars=yes,status=no,toolbar=no');", guid.ToString)
                .Append(ControlChars.CrLf)
                .Append("--></script>")
                .Append(ControlChars.CrLf)
            End With

            ClientScript.RegisterClientScriptBlock(Me.GetType, "POPUP", popupBuilder.ToString)
            lbl_Message.Text = "Sollte sich kein neues Browserfenster öffnen, deaktivieren Sie bitte Ihren Popupblocker!"
        Else
            lbl_Message.Text = "Das Straßenverkehrsamt für das amtliche Kennzeichen " & sAmt & " bietet keinen Weblink hierfür an."
            If Not RowName = "STVALN" Then
                lbl_Message.Text &= "<br>Möchten Sie auf die Standardstartseite dieses Verkehrsamts wechseln, so klicken Sie bitte auf den Link Amt. "
            End If
        End If

    End Sub
End Class

' ************************************************
' $History: Report99.aspx.vb $
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 9.03.10    Time: 10:34
' Updated in $/CKAG/Components/ComCommon
' ITA: 2918
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 5.06.09    Time: 15:58
' Updated in $/CKAG/Components/ComCommon
' mögliche try catches entfernt
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 11.04.08   Time: 13:43
' Updated in $/CKAG/Components/ComCommon
' Migration
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 10.04.08   Time: 17:37
' Updated in $/CKAG/Components/ComCommon
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:32
' Created in $/CKAG/Components/ComCommon
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 10:47
' Created in $/CKAG/portal/Components/ComCommon
' 
' *****************  Version 17  *****************
' User: Rudolpho     Date: 30.01.08   Time: 10:36
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' ITA:1371
' 
' *****************  Version 16  *****************
' User: Rudolpho     Date: 25.01.08   Time: 12:41
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' 
' *****************  Version 15  *****************
' User: Rudolpho     Date: 23.01.08   Time: 13:08
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' ITA: 1371
' 
' *****************  Version 14  *****************
' User: Rudolpho     Date: 22.01.08   Time: 12:58
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' ITA: 1647
' 
' *****************  Version 13  *****************
' User: Rudolpho     Date: 21.01.08   Time: 11:35
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' 
' *****************  Version 12  *****************
' User: Rudolpho     Date: 15.01.08   Time: 12:47
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' ITA: 1371
' 
' *****************  Version 11  *****************
' User: Rudolpho     Date: 14.01.08   Time: 16:16
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 21.12.07   Time: 14:11
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' ITA:1371
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 21.12.07   Time: 13:38
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' ITA: 1371
' 
' *****************  Version 8  *****************
' User: Uha          Date: 12.07.07   Time: 9:28
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 7  *****************
' User: Uha          Date: 5.03.07    Time: 14:57
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' 
' ************************************************
