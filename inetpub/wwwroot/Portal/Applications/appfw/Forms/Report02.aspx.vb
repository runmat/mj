Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Report02
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
    Protected WithEvents rbEin As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rbAus As System.Web.UI.WebControls.RadioButton
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles
    Protected WithEvents tr_Datumab As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr_Datumbis As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lbl_Datumab As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Datumbis As System.Web.UI.WebControls.Label

    Protected WithEvents ucHeader As Header

    Dim m_Report As fw_02

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

            calBisDatum.SelectedDate = Today
            calAbDatum.SelectedDate = Today
            txtBisDatum.Text = calBisDatum.SelectedDate.ToShortDateString
            txtAbDatum.Text = calBisDatum.SelectedDate.ToShortDateString
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



            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            m_Report = New fw_02(m_User, m_App, datAb, datBis, strFileName)


            m_Report.Fill(Session("AppID").ToString, Session.SessionID.ToString)
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
' $History: Report02.aspx.vb $
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
' *****************  Version 1  *****************
' User: Jungj        Date: 14.04.08   Time: 11:53
' Created in $/CKG/Applications/AppFW/AppFWWeb/Forms
' ITA 1845
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
' ITA 1677: Report02_1.aspx zum Testen fertig
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

