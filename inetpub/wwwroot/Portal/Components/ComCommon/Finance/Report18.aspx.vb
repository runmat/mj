Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Report18
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lb_Weiter As System.Web.UI.WebControls.LinkButton
    Protected WithEvents txtAbDatum As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBisDatum As System.Web.UI.WebControls.TextBox
    Protected WithEvents rbEin As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rbAus As System.Web.UI.WebControls.RadioButton
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles
    Protected WithEvents rb_alle As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rb_Temp As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rb_end As System.Web.UI.WebControls.RadioButton
    Protected WithEvents tr_EinAus As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr_Versand As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lbl_Versand As System.Web.UI.WebControls.Label
    Protected WithEvents tr_Datumab As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr_Datumbis As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lbl_Datumab As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Datumbis As System.Web.UI.WebControls.Label
    Protected WithEvents rb_alleTemp As System.Web.UI.WebControls.RadioButton
    Protected WithEvents ucHeader As Header
    Protected WithEvents CEbisDatum As AjaxControlToolkit.CalendarExtender
    Protected WithEvents CEAbDatum As AjaxControlToolkit.CalendarExtender
    Protected WithEvents txtReferenznummer As TextBox

    Dim m_Report As fin_10

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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        m_App = New Base.Kernel.Security.App(m_User)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)


        If Not IsPostBack Then

            txtAbDatum.Text = Today.ToShortDateString
            txtBisDatum.Text = Today.ToShortDateString

            GetAppIDFromQueryString(Me)

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text

            Session("ShowLink") = "False"
            Session("ShowOtherString") = ""
        End If


        If Not IsPostBack Then

            If Request.QueryString("Flag") = 1 Then
                rb_Temp.Checked = True
                rbAus.Checked = True 'wenn rb_temp checked ist muss aus ausgänge checked sein, sonst inkonsistenz der anzeige JJU2008.03.04
            Else
                tr_Versand.Attributes.Add("style", "Display: none;")
            End If
        Else
            'wenn Postback
            If rbAus.Checked = True Then
                'wenn ausgänge gecheckt dann ExtraZeile anzeigen
                tr_Versand.Attributes.Add("style", "Display:'';")
            End If
        End If
        'Skripte einfügen
        rbEin.Attributes.Add("onclick ", "Set_VersandFalse();")
        rbAus.Attributes.Add("onclick ", "Set_VersandTrue();")
        rb_alleTemp.Attributes.Add("onclick ", "Set_Datums('False')")
        
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_Weiter.Click
        Session("ShowLink") = "True"
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        Session("lnkExcel") = ""

        lblError.Text = ""
        If Not IsDate(txtAbDatum.Text) Then
            If Not IsStandardDate(txtAbDatum.Text) Then
                'If Not IsSAPDate(txtAbDatum.Text) Then
                lblError.Text = "Geben Sie bitte ein gültiges Datum ein!<br>"
                Exit Sub
                'End If
            End If
        End If

        If Not IsDate(txtBisDatum.Text) Then
            If Not IsStandardDate(txtBisDatum.Text) Then
                'If Not IsSAPDate(txtBisDatum.Text) Then
                lblError.Text = "Geben Sie bitte ein gültiges Datum ein!<br>"
                Exit Sub
                'End If
            End If
        End If

        Dim datAb As Date = CDate(txtAbDatum.Text)
        Dim datBis As Date = CDate(txtBisDatum.Text)
        If datAb > datBis Then
            lblError.Text = "Das Enddatum darf nicht vor dem Anfangsdatum liegen!<br>"
            Exit Sub
        End If


        Dim strAction As String = "AUS"
        If rbEin.Checked Then
            strAction = "NEU"
        End If

        Dim strVersand As String = "A"
        If rb_end.Checked = True Then
            strVersand = "2"
        End If
        If rb_Temp.Checked = True Then
            strVersand = "1"
        End If

        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
        m_Report = New fin_10(m_User, m_App, datAb, datBis, strAction, strVersand, strFileName)

        If rb_alleTemp.Checked = True And rb_alleTemp.Visible = True Then
            m_Report.action = "AUS"
            m_Report.versand = "1"
            'SFa: 5 Jahre zurück; sonst Langläufer
            m_Report.datVON = New Date(Date.Today.Year - 5, 1, 1)
            m_Report.datBIS = Today
            m_Report.tmpSelection = True
        Else
            m_Report.tmpSelection = False
        End If

        m_Report.Referenznummer = txtReferenznummer.Text


        m_Report.Fill(Session("AppID").ToString, Session.SessionID.ToString)
        Session("mObjfin_10Session") = m_Report

        If Not m_Report.Status = 0 Then
            lblError.Text = m_Report.Message
        Else
            If m_Report.Result.Rows.Count = 0 Then
                lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
            Else
                'hier muss auf grund der aktion eine andere aspx aufgerufen werden, wegen feldübersetzung. JJU2008.10.20
                If m_Report.action = "AUS" Then
                    'ausgänge 
                    Response.Redirect("Report18_2.aspx?AppID=" & Session("AppID").ToString)
                Else
                    'eingänge
                    Response.Redirect("Report18_1.aspx?AppID=" & Session("AppID").ToString)
                End If
            End If
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
' $History: Report18.aspx.vb $
' 
' *****************  Version 11  *****************
' User: Fassbenders  Date: 4.09.09    Time: 9:55
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA: 3050
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 5.06.09    Time: 15:58
' Updated in $/CKAG/Components/ComCommon/Finance
' mögliche try catches entfernt
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 2.02.09    Time: 11:27
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2550 unfertig
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 6.11.08    Time: 15:19
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2364 Bugfixing
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 22.10.08   Time: 14:01
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2312 für alphabet nodealer funktion entfernt
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 20.10.08   Time: 14:21
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2288
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 20.10.08   Time: 13:17
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2288
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 16.04.08   Time: 14:43
' Updated in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 9.04.08    Time: 11:37
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' datumsvorbelegung 
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 4.03.08    Time: 10:03
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1733, 1667, 1738 
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 29.02.08   Time: 16:51
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1733
' 
' *****************  Version 7  *****************
' User: Uha          Date: 4.02.08    Time: 11:56
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1677: Report18_1.aspx zum Testen fertig
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 24.01.08   Time: 12:43
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 8.01.08    Time: 16:13
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Bugfix RTFS
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 20.12.07   Time: 8:29
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 19.12.07   Time: 10:50
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA: 1532
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 18.12.07   Time: 16:54
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' 
' *****************  Version 1  *****************
' User: Rudolpho     Date: 18.12.07   Time: 13:58
' Created in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA: 1479, 1503
' 
' ************************************************

