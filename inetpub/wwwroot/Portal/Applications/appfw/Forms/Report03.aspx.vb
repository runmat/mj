Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Report03
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents cmdCreate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents txtAbDatum As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnOpenSelectAb As System.Web.UI.WebControls.Button
    Protected WithEvents calAbDatum As System.Web.UI.WebControls.Calendar
    Protected WithEvents txtBisDatum As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnOpenSelectBis As System.Web.UI.WebControls.Button
    Protected WithEvents calBisDatum As System.Web.UI.WebControls.Calendar
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles
    Protected WithEvents tr_Datumab As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr_Datumbis As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lbl_Datumab As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Datumbis As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents rb_unvollstaendig As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rb_vollstaendig As System.Web.UI.WebControls.RadioButton

    Dim m_Report As fw_03

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
        Session("ShowLink") = "False"
        Session("ShowOtherString") = ""
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        NoDealer(Me, m_User)
        GetAppIDFromQueryString(Me)
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)


       
        If Not IsPostBack Then

            calBisDatum.SelectedDate = Today
            txtBisDatum.Text = calBisDatum.SelectedDate.ToShortDateString
            txtAbDatum.Text = calBisDatum.SelectedDate.ToShortDateString

            'Skriptefunktionen einfügen
            rb_unvollstaendig.Checked = True
                rb_unvollstaendig.Attributes.Add("onclick ", "Set_Datums('False');")
                rb_vollstaendig.Attributes.Add("onclick ", "Set_Datums('True');")

            End If
            lblError.Text = ""

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        Session("ShowLink") = "True"
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        Session("lnkExcel") = ""
        Try
            lblError.Text = ""

            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            m_Report = New fw_03(m_User, m_App, strFileName)


            If rb_vollstaendig.Checked = True Then
                If Not IsDate(txtAbDatum.Text) Then
                    If Not IsStandardDate(txtAbDatum.Text) Then
                        If Not IsSAPDate(txtAbDatum.Text) Then
                            lblError.Text = "Geben Sie bitte ein gültiges Datum ein!<br>"
                            Exit Sub
                        End If
                    End If
                End If
                If Not IsDate(txtBisDatum.Text) Then
                    If Not IsStandardDate(txtBisDatum.Text) Then
                        If Not IsSAPDate(txtBisDatum.Text) Then
                            lblError.Text = "Geben Sie bitte ein gültiges Datum ein!<br>"
                            Exit Sub
                        End If
                    End If
                End If
                Dim datAb As Date = CDate(txtAbDatum.Text)
                Dim datBis As Date = CDate(txtBisDatum.Text)
                If datAb > datBis Then
                    lblError.Text = "Das Enddatum darf nicht vor dem Anfangsdatum liegen!<br>"
                    Exit Sub
                Else
                    m_Report.datBIS = datBis
                    m_Report.datVON = datAb
                End If
                m_Report.Fill1(Session("AppID").ToString, Session.SessionID.ToString)
            Else

                m_Report.Fill2(Session("AppID").ToString, Session.SessionID.ToString)
            End If


            Session("m_report") = m_Report
            Session("ResultTable") = m_Report.Result

            If Not m_Report.Status = 0 Then
                lblError.Text = m_Report.Message
            Else
                If m_Report.Result.Rows.Count = 0 Then
                    lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                Else
                    'wiso bekomme ich den filename den ich beim erzeugen des Reportobjektes übergeben musste nicht wieder zurück? Property Eingebaut in fw_02 JJU2008.04.14

                    Dim objExcelExport As New Excel.ExcelExport()

                    objExcelExport.WriteExcel(m_Report.Result, ConfigurationSettings.AppSettings("ExcelPath") & m_Report.FileName)


                    Session("lnkExcel") = "/Portal/Temp/Excel/" & m_Report.FileName
                    Response.Redirect("/Portal/(S(" & Session.SessionID & "))/Shared/Report01_2.aspx?AppID=" & Session("AppID").ToString)

                End If
            End If
        Catch ex As Exception
            lblError.Text = "Fehler: " & ex.Message
        End Try
    End Sub

    Private Sub btnOpenSelectAb_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpenSelectAb.Click
        calAbDatum.Visible = True
    End Sub

    Private Sub btnOpenSelectBis_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpenSelectBis.Click
        calBisDatum.Visible = True
    End Sub

    Private Sub calAbDatum_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calAbDatum.SelectionChanged
        calAbDatum.Visible = False
        txtAbDatum.Text = calAbDatum.SelectedDate.ToShortDateString
    End Sub

    Private Sub calBisDatum_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calBisDatum.SelectionChanged
        calBisDatum.Visible = False
        txtBisDatum.Text = calBisDatum.SelectedDate.ToShortDateString
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Report03.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 21.08.08   Time: 9:50
' Updated in $/CKAG/Applications/appfw/Forms
' ITA 2137 ungetestet
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 17.06.08   Time: 12:59
' Updated in $/CKAG/Applications/appfw/Forms
' ITA 1845
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 6.05.08    Time: 13:39
' Created in $/CKAG/Applications/appfw/Forms
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 6.05.08    Time: 12:49
' Created in $/CKAG/Applications/appfw/appfw/Forms
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 15.04.08   Time: 9:03
' Updated in $/CKG/Applications/AppFW/AppFWWeb/Forms
' ITA 1783
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 14.04.08   Time: 16:16
' Created in $/CKG/Applications/AppFW/AppFWWeb/Forms
' ITA 1783/1841
' ************************************************

