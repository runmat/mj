Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Partial Public Class Report23
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten."
        End Try
    End Sub

    Protected Sub btnAbmeldVon_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAbmeldVon.Click
        CalMeldungVon.Visible = True
        calMeldungBis.Visible = False
    End Sub

    Protected Sub btnAbmeldBis_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAbmeldBis.Click
        CalMeldungVon.Visible = False
        calMeldungBis.Visible = True
    End Sub

    Protected Sub lbStilllegungVon_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbStilllegungVon.Click
        CalStillVon.Visible = True
        CalStillBis.Visible = False
    End Sub

    Protected Sub lbStilllegungBis_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbStilllegungBis.Click
        CalStillVon.Visible = False
        CalStillBis.Visible = True
    End Sub

    Protected Sub CalStillVon_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles CalStillVon.SelectionChanged
        txtStilllegungVon.Text = CalStillVon.SelectedDate.ToShortDateString
        CalStillVon.Visible = False
    End Sub

    Protected Sub CalStillBis_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles CalStillBis.SelectionChanged
        txtStilllegungBis.Text = CalStillBis.SelectedDate.ToShortDateString
        CalStillBis.Visible = False
    End Sub

    Protected Sub CalMeldungVon_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles CalMeldungVon.SelectionChanged
        txtMeldungVon.Text = CalMeldungVon.SelectedDate.ToShortDateString
        CalMeldungVon.Visible = False
    End Sub

    Protected Sub calMeldungBis_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles calMeldungBis.SelectionChanged
        txtMeldungBis.Text = calMeldungBis.SelectedDate.ToShortDateString
        calMeldungBis.Visible = False
    End Sub

    Protected Sub cmdCreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCreate.Click
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        Dim checkDate As Boolean = True
        Session("lnkExcel") = ""
        Try
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            Dim m_Report As New Unfallmeldung(m_User, m_App, "")

            lblError.Text = ""

            Dim checkInput As Boolean = True
            Dim errorText As String = ""
            checkInput = HelpProcedures.checkDate(txtMeldungVon, txtMeldungBis, errorText, True)

            If checkInput = False Then
                lblError.Text = errorText
                Return
            End If

            checkInput = HelpProcedures.checkDate(txtStilllegungVon, txtStilllegungBis, errorText, True)

            If checkInput = False Then
                lblError.Text = errorText
                Return
            End If

            If checkInput Then

                m_Report.WebUser = ""
                m_Report.Station = ""
                m_Report.Mahnstufe = ""
                m_Report.OhneAbmeld = ""
                m_Report.MitAbmeld = "X"
                m_Report.AnlagedatVon = txtMeldungVon.Text
                m_Report.AnlagedatBis = txtMeldungBis.Text
                m_Report.AbmeldedatVon = txtStilllegungVon.Text
                m_Report.AbmeldedatBis = txtStilllegungBis.Text

                m_Report.FillUnfallMeldungen(Session("AppID").ToString, Session.SessionID.ToString, "", "", Me)

                Session("ResultTable") = m_Report.UFMeldungExcel

                If Not m_Report.Status = 0 Then
                    lblError.Text = "Fehler: " & m_Report.Message
                Else
                    If m_Report.Result.Rows.Count = 0 Then
                        lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                    Else
                        Try
                            Excel.ExcelExport.WriteExcel(m_Report.Result, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                        Catch
                        End Try
                       
                        Dim AbsoluterPfadZumVirituellenVerz As String = ConfigurationManager.AppSettings("ReplaceExcelPath")
                        Session("lnkExcel") = ConfigurationManager.AppSettings("ExcelPath").Replace(AbsoluterPfadZumVirituellenVerz, "") & strFileName & "".Replace("/", "\")
                        Response.Redirect("/PortalORM/(S(" & Session.SessionID & "))/Shared/Report01_2.aspx?AppID=" & Session("AppID").ToString)
                    End If
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub
End Class