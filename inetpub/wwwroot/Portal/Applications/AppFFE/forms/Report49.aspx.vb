Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Partial Public Class Report49
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Dim m_Report As FFE_Abgrufgruende

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session("ShowLink") = "False"
        Session("ShowOtherString") = ""
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        'NoDealer(Me, m_User) Soll jetzt auch ein Händlerreport sein
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
            txtAbDatum.Text = Today
            FillAbgruf()
        End If

    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        Session("ShowLink") = "True"
        DoSubmit()
    End Sub
    Private Sub FillAbgruf()
        m_Report = New FFE_Abgrufgruende(m_User, m_App, "")
        If rb_temp.checked = True Then
            m_Report.DBAbrufgrund("temp")
        ElseIf rb_Endg.Checked = True Then
            m_Report.DBAbrufgrund("endg")
        End If
        If m_Report.Status = 0 Then
            If Not m_Report.Abrufgruende Is Nothing Then
                If m_Report.Abrufgruende.Rows.Count > 0 Then
                    ddlAbrufgrund.Items.Clear()
                    ddlAbrufgrund.DataSource = m_Report.Abrufgruende.DefaultView
                    ddlAbrufgrund.DataTextField = "WebBezeichnung"
                    ddlAbrufgrund.DataValueField = "SapWert"
                    ddlAbrufgrund.DataBind()
                    If rb_Temp.Checked = True Then
                        ddlAbrufgrund.Items.Insert(0, New ListItem("alle", "TMP"))
                    ElseIf rb_Endg.Checked = True Then
                        'ITA 2146, neue Selektionsmöglichkeit JJU2008.10.1
                        ddlAbrufgrund.Items.Insert(0, New ListItem("Nachträglich endgültig", "NAE"))
                        ddlAbrufgrund.Items.Insert(0, New ListItem("alle", "END"))
                    End If
                Else
                    lblError.Text = "Keine Abrufgründe hinterlegt!"
                End If
            Else
                lblError.Text = "Keine Abrufgründe hinterlegt!"
            End If
        Else
            lblError.Text = "Fehler beim lesen der Abrufgründe!"
        End If

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
            m_Report = New FFE_Abgrufgruende(m_User, m_App, strFileName)
            m_Report.Kunnr = m_User.Reference
            If m_User.Reference.Length > 0 Then
                m_Report.Kunnr = Right("0000000000" & "60" + m_User.Reference, 10)
            End If

            m_Report.datVON = txtAbDatum.Text
            m_Report.datBIS = txtBisDatum.Text
            m_Report.SAPWert = ddlAbrufgrund.SelectedItem.Value
            m_Report.Fill(Session("AppID").ToString, Session.SessionID.ToString)

            Session("m_report") = m_Report
            Session("ResultTable") = m_Report.Result

            If Not m_Report.Status = 0 Then
                lblError.Text = m_Report.Message
            Else
                If m_Report.Result.Rows.Count = 0 Then
                    lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                Else
                    Response.Redirect("Report49_1.aspx?AppID=" & Session("AppID").ToString)
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

    Protected Sub rb_Temp_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rb_Temp.CheckedChanged
        FillAbgruf()
    End Sub

    Protected Sub rb_Endg_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rb_Endg.CheckedChanged
        FillAbgruf()
    End Sub
End Class
' ************************************************
' $History: Report49.aspx.vb $
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 1.10.08    Time: 14:39
' Updated in $/CKAG/Applications/AppFFE/forms
' ITA 2146 testfertig
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 17.09.08   Time: 8:17
' Updated in $/CKAG/Applications/AppFFE/forms
' ITA 2222 fertig
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 30.07.08   Time: 14:34
' Updated in $/CKAG/Applications/AppFFE/forms
' ITA:2091
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 25.06.08   Time: 11:02
' Updated in $/CKAG/Applications/AppFFE/forms
' ITA 2019, warte auf Testdaten
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 18.06.08   Time: 14:33
' Updated in $/CKAG/Applications/AppFFE/forms
' Ausblenden Hndler Kontingente
' 
' ************************************************
