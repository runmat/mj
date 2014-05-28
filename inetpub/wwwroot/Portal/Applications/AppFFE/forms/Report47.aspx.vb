Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Report47
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
    Protected WithEvents chbInAufbietung As System.Web.UI.WebControls.CheckBox

    Protected WithEvents ucHeader As Header

    Dim m_Report As FFE_Aufbietung

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


        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
        If Not IsPostBack Then
            txtBisDatum.Text = Today
            txtAbDatum.Text = DateAdd(DateInterval.Day, -30, Today)
        End If
       


    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        Session("ShowLink") = "True"
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        Session("lnkExcel") = ""
        Try
            lblError.Text = ""
            If chbInAufbietung.Checked = False Then
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
                End If
            End If
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            m_Report = New FFE_Aufbietung(m_User, m_App, strFileName)

            m_Report.datVON = txtAbDatum.Text
            m_Report.datBIS = txtBisDatum.Text
            If chbInAufbietung.Checked Then
                m_Report.abckz = "9"
            Else
                m_Report.abckz = ""
            End If

            m_Report.Fill(Session("AppID").ToString, Session.SessionID.ToString)

            Session("m_report") = m_Report
            Session("ResultTable") = m_Report.Result

            If Not m_Report.Status = 0 Then
                lblError.Text = m_Report.Message
            Else
                If m_Report.Result.Rows.Count = 0 Then
                    lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                Else
                    Dim DataR As DataRow
                    For Each DataR In m_Report.Result.Rows
                        If DataR("Laufend").ToString = "9" Then
                            DataR("Laufend") = "In Aufbietung"
                        End If
                    Next

                    Session("ShowOtherString") = "Es wurden " & m_Report.Result.Rows.Count.ToString & " Einträge gefunden."

                    Try
                        Excel.ExcelExport.WriteExcel(m_Report.Result, System.Configuration.ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                    Catch
                    End Try
                    Session("lnkExcel") = "/Portal/Temp/Excel/" & strFileName
                    Response.Redirect("Report47_1.aspx?AppID=" & Session("AppID").ToString)
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

    Protected Sub chbInAufbietung_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chbInAufbietung.CheckedChanged
        If chbInAufbietung.Checked Then
            txtAbDatum.Text = ""
            txtBisDatum.Text = ""
            txtAbDatum.Enabled = False
            txtBisDatum.Enabled = False
            btnOpenSelectAb.Enabled = False
            btnOpenSelectBis.Enabled = False
        Else
            txtAbDatum.Enabled = True
            txtBisDatum.Enabled = True
            btnOpenSelectAb.Enabled = True
            btnOpenSelectBis.Enabled = True
        End If

    End Sub
End Class

' ************************************************
' $History: Report47.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 4.06.08    Time: 18:43
' Updated in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 13.05.08   Time: 16:46
' Updated in $/CKAG/Applications/AppFFE/forms
' ITA 1877
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 6.05.08    Time: 15:49
' Updated in $/CKAG/Applications/AppFFE/forms
' ITA 1877
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 6.05.08    Time: 14:27
' Created in $/CKAG/Applications/AppFFE/forms
' ITA 1877
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
' ITA 1677: Report47_1.aspx zum Testen fertig
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

