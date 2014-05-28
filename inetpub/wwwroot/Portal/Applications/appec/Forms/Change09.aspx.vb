Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Partial Public Class Change09
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App

#Region "Events"


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

    Protected Sub btnVon_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnVon.Click
        calVon.Visible = True
        calBis.Visible = False
    End Sub

    Private Sub btnBis_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBis.Click
        calBis.Visible = True
        calVon.Visible = False
    End Sub

    Private Sub calVon_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calVon.SelectionChanged
        txtAnlagedatumVon.Text = calVon.SelectedDate.ToShortDateString
        calVon.Visible = False
    End Sub

    Private Sub calBis_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calBis.SelectionChanged
        txtAnlagedatumBis.Text = calBis.SelectedDate.ToShortDateString
        calBis.Visible = False
    End Sub

    Protected Sub btnAbmeldVon_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAbmeldVon.Click
        calAbmeldeDatVon.Visible = True
        calAbmeldeDatBis.Visible = False
    End Sub

    Protected Sub btnAbmeldBis_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAbmeldBis.Click
        calAbmeldeDatVon.Visible = False
        calAbmeldeDatBis.Visible = True
    End Sub

    Protected Sub calAbmeldeDatVon_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles calAbmeldeDatVon.SelectionChanged
        txtAbmeldungVon.Text = calAbmeldeDatVon.SelectedDate.ToShortDateString
        calAbmeldeDatVon.Visible = False
    End Sub

    Protected Sub calAbmeldeDatBis_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles calAbmeldeDatBis.SelectionChanged
        txtAbmeldungBis.Text = calAbmeldeDatBis.SelectedDate.ToShortDateString
        calAbmeldeDatBis.Visible = False
    End Sub

    Protected Sub chkMitAbmeld_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkMitAbmeld.CheckedChanged
        If chkMitAbmeld.Checked = True Then
            chkOhneAbmeld.Checked = False
            trAbmeldeDatVon.Visible = True
            trAbmeldeDatBis.Visible = True
        Else
            chkOhneAbmeld.Checked = False
            trAbmeldeDatVon.Visible = False
            trAbmeldeDatBis.Visible = False
        End If
    End Sub

    Protected Sub chkOhneAbmeld_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkOhneAbmeld.CheckedChanged
        If chkOhneAbmeld.Checked = True Then
            chkMitAbmeld.Checked = False
            trAbmeldeDatVon.Visible = False
            trAbmeldeDatBis.Visible = False
        Else
            chkMitAbmeld.Checked = False
            trAbmeldeDatVon.Visible = True
            trAbmeldeDatBis.Visible = True
        End If
    End Sub

    Protected Sub cmdCreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCreate.Click
        DoSubmit()
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

#End Region

#Region "Methods"

    Private Sub DoSubmit(Optional ByVal Fahrgestellnummer As String = "")
        Session("lnkExcel") = ""
        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"

        Dim m_Report As New Unfallmeldung(m_User, m_App, "")
        Try
            Dim strWebUser As String = ""
            Dim strFahrgestellnummer As String = ""
            Dim strAmtlKennzeichen As String = ""
            Dim strStation As String = ""
            Dim strMahnstufe As String = ""
            Dim strOhneAbmeld As String = ""
            Dim strMitAbmeld As String = ""
            Dim strStorniert As String = ""

            If txtKennzeichen.Text.Length > 0 Then
                If txtKennzeichen.Text.Contains("*") = True Then
                    If txtKennzeichen.Text.Length < 4 Then
                        lblError.Text = "Kennzeichen-Eingabe unterschreitet Minimallänge."
                        Return
                    End If
                End If
                strAmtlKennzeichen = txtKennzeichen.Text
            End If

            If txtFahrgestellnummer.Text.Length > 0 Then
                If txtFahrgestellnummer.Text.Contains("*") = True Then
                    If txtFahrgestellnummer.Text.Length < 9 Then
                        lblError.Text = "Fahrgestellnummer-Eingabe unterschreitet Minimallänge."
                        Return
                    End If
                End If
                strFahrgestellnummer = txtFahrgestellnummer.Text
            End If
            Dim checkInput As Boolean = True
            Dim errorText As String = ""
            checkInput = HelpProcedures.checkDate(txtAnlagedatumVon, txtAnlagedatumBis, errorText, True)

            If checkInput = False Then
                lblError.Text = errorText
                Return
            End If
            If txtWebuser.Text.Length > 0 Then
                strWebUser = txtWebuser.Text
            End If

            If txtStation.Text.Length > 0 Then
                strStation = txtStation.Text
            End If
            If ddlMahnstufen.SelectedIndex > 0 Then
                strMahnstufe = ddlMahnstufen.SelectedValue
            End If

            If chkOhneAbmeld.Checked Then
                strOhneAbmeld = "X"
            End If

            If chkMitAbmeld.Checked Then
                strMitAbmeld = "X"
                checkInput = HelpProcedures.checkDate(txtAbmeldungVon, txtAbmeldungBis, errorText, True)
                If checkInput = False Then
                    lblError.Text = errorText
                    Return
                End If
            End If
            If chkStorno.Checked Then
                strStorniert = "X"
            End If


            If strAmtlKennzeichen.Length + strFahrgestellnummer.Length + _
                strStation.Length + strWebUser.Length + strMahnstufe.Length + _
                strOhneAbmeld.Length + strMitAbmeld.Length + strWebUser.Length + _
                txtAnlagedatumVon.Text.Length + txtAnlagedatumBis.Text.Length + _
                txtAbmeldungVon.Text.Length + txtAbmeldungBis.Text.Length = 0 + _
                 strOhneAbmeld.Length > 0 + strOhneAbmeld.Length > 0 + strStorniert.Length > 0 Then
                lblError.Text = "Bitte geben sie mindestens ein Suchkriterium ein!"
                Return
            End If
            m_Report.WebUser = strWebUser
            m_Report.Station = strStation
            m_Report.Mahnstufe = strMahnstufe
            m_Report.OhneAbmeld = strOhneAbmeld
            m_Report.MitAbmeld = strMitAbmeld
            m_Report.Stornierte = strStorniert
            m_Report.AnlagedatVon = txtAnlagedatumVon.Text
            m_Report.AnlagedatBis = txtAnlagedatumBis.Text
            m_Report.AbmeldedatVon = txtAbmeldungVon.Text
            m_Report.AbmeldedatBis = txtAbmeldungBis.Text



            m_Report.FillUnfallMeldungen(Session("AppID").ToString, Session.SessionID.ToString, strAmtlKennzeichen, strFahrgestellnummer, Me)

            If m_Report.Status > 0 Then
                lblError.Text = m_Report.Message
            Else
                Try
                    Excel.ExcelExport.WriteExcel(m_Report.UFMeldungExcel, ConfigurationManager.AppSettings("ExcelPath") & strFileName)


                    Dim AbsoluterPfadZumVirituellenVerz As String = ConfigurationManager.AppSettings("ReplaceExcelPath")
                    Session("lnkExcel") = ConfigurationManager.AppSettings("ExcelPath").Replace(AbsoluterPfadZumVirituellenVerz, "") & strFileName & "".Replace("/", "\")

                Catch
                End Try

                Session("App_UnfallMeldungen") = m_Report
                Response.Redirect("Change09_2.aspx?AppID=" & Session("AppID").ToString)
            End If



        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten."
        End Try

        If Not m_Report.Status = 0 Then
            lblError.Text = m_Report.Message
        Else

            If (m_Report.ResultFahrzeuge Is Nothing) OrElse (m_Report.ResultFahrzeuge.Rows.Count = 0) Then

                lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
            Else

            End If


        End If
    End Sub
#End Region


End Class