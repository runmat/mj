Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Partial Public Class Report80
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text
        m_App = New Base.Kernel.Security.App(m_User)

        'DoSubmit()
       
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        Session("ShowLink") = "True"
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        Session("lnkExcel") = ""
        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)


        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
        Dim m_Report As New AppDokVerw_04(m_User, m_App, strFileName)
        Dim datAbmeldedatumVon As DateTime
        Dim datAbmeldedatumBis As DateTime

        lblError.Text = ""

        If Not (txtAbmeldedatumVon.Text = String.Empty) Then
            If Not IsDate(txtAbmeldedatumVon.Text) Then
                If Not IsStandardDate(txtAbmeldedatumVon.Text) Then
                    If Not IsSAPDate(txtAbmeldedatumVon.Text) Then
                        lblError.Text = "Geben Sie bitte ein gültiges ""Abmeldedatum von"" ein!<br>"
                    Else
                        datAbmeldedatumVon = CDate(txtAbmeldedatumVon.Text)
                    End If
                Else
                    datAbmeldedatumVon = CDate(txtAbmeldedatumVon.Text)
                End If
            Else
                datAbmeldedatumVon = CDate(txtAbmeldedatumVon.Text)
            End If
        Else
            lblError.Text = "Geben Sie bitte ein gültiges ""Abmeldedatum von"" ein!<br>"
        End If

        If Not (txtAbmeldedatumBis.Text = String.Empty) Then
            If Not IsDate(txtAbmeldedatumBis.Text) Then
                If Not IsStandardDate(txtAbmeldedatumBis.Text) Then
                    If Not IsSAPDate(txtAbmeldedatumBis.Text) Then
                        lblError.Text &= "Geben Sie bitte ein gültiges ""Abmeldedatum bis"" ein!<br>"
                    Else
                        datAbmeldedatumBis = CDate(txtAbmeldedatumBis.Text)
                    End If
                Else
                    datAbmeldedatumBis = CDate(txtAbmeldedatumBis.Text)
                End If
            Else
                datAbmeldedatumBis = CDate(txtAbmeldedatumBis.Text)
            End If
        Else
            lblError.Text &= "Geben Sie bitte ein gültiges ""Abmeldedatum bis"" ein!<br>"
        End If

        If lblError.Text.Length = 0 Then
            If datAbmeldedatumVon > datAbmeldedatumBis Then
                lblError.Text &= """Abmeldedatum bis"" muss größer als ""Abmeldedatum von"" sein!<br>"
            Else
                If datAbmeldedatumVon > datAbmeldedatumBis Or DateAdd(DateInterval.Month, 3, datAbmeldedatumVon) < datAbmeldedatumBis Then
                    lblError.Text &= "Der maximale Zeitraum (""Abmeldedatum von"" - ""Abmeldedatum bis"") beträgt drei Monate!<br>"
                End If
            End If
        End If

        If lblError.Text.Length = 0 Then
            m_Report.AbmeldedatumVon = datAbmeldedatumVon
            m_Report.AbmeldedatumBis = datAbmeldedatumBis
            If rb_Alle.Checked Then
                m_Report.ABC_KZ = "0"
            End If
            If rb_tempVersand.Checked Then
                m_Report.ABC_KZ = "1"
            End If
            If rb_endgVersand.Checked Then
                m_Report.ABC_KZ = "2"
            End If
            m_Report.Fill(Session("AppID").ToString, Session.SessionID.ToString)

            Session("ResultTable") = m_Report.ResultTable

            If Not m_Report.Status = 0 Then
                lblError.Text = "Fehler: " & m_Report.Message
            Else
                If m_Report.ResultTable.Rows.Count = 0 Then
                    lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                Else

                    'Dim objExcelExport As New Excel.ExcelExport()
                    'Try
                    '    Excel.ExcelExport.WriteExcel(m_Report.Result, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                    'Catch
                    'End Try
                    'Session("lnkExcel") = "/Portal/Temp/Excel/" & strFileName
                    logApp.UpdateEntry("APP", Session("AppID").ToString, "Report: Fehlende Abmeldeunterlagen")
                    Response.Redirect("Report80_2.aspx?AppID=" & Session("AppID").ToString)
                End If
            End If
        End If

    End Sub

    Private Sub btnCal1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCal1.Click
        calVon.Visible = True
        calBis.Visible = False
    End Sub

    Private Sub btnCal2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCal2.Click
        calVon.Visible = False
        calBis.Visible = True
    End Sub

    Private Sub calVon_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles calVon.SelectionChanged
        txtAbmeldedatumVon.Text = calVon.SelectedDate.ToShortDateString
        calVon.Visible = False
    End Sub

    Private Sub calBis_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles calBis.SelectionChanged
        txtAbmeldedatumBis.Text = calBis.SelectedDate.ToShortDateString
        calBis.Visible = False
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

End Class
' ************************************************
' $History: Report80.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 5.06.09    Time: 13:51
' Updated in $/CKAG/Components/ComCommon/AppDokVerw
' Try Catch entfernt wenn m�glich
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 4.05.09    Time: 11:50
' Updated in $/CKAG/Components/ComCommon/AppDokVerw
' 
' *****************  Version 1  *****************
' User: Rudolpho     Date: 15.07.08   Time: 14:20
' Created in $/CKAG/Components/ComCommon/AppDokVerw
' ITA: 2081
' 
' ************************************************
