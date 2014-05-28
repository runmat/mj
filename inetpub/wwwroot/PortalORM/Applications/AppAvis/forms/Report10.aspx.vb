Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Partial Public Class Report10
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles

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
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        Session("ShowLink") = "True"
        If (checkInput()) Then
            DoSubmit()
        End If
    End Sub

    Private Sub DoSubmit()
        Session("lnkExcel") = ""
        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)


        Try
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            Dim m_Report As New Rechnungsdatenreport(m_User, m_App, "")

            lblError.Text = ""

            If checkDate() Then
                Dim strRechnungsnummer As String
                Dim strFahrgestellnummer As String
                Dim strAusgabeart As String
                Dim strRechnungsdatumVon As String
                Dim strRechnungsdatumBis As String
                Dim strLeistungsdatumVon As String
                Dim strLeistungsdatumBis As String
                Dim strLeistungsart As String
                Dim strSpediteur As String

                If txtRechnNr.Text.Length = 0 Then
                    strRechnungsnummer = ""
                Else
                    txtRechnNr.Text = Trim(txtRechnNr.Text)
                    strRechnungsnummer = txtRechnNr.Text
                End If
                If txtFahrgestNr.Text.Length = 0 Then
                    strFahrgestellnummer = ""
                    strAusgabeart = ""
                Else
                    txtFahrgestNr.Text = Trim(txtFahrgestNr.Text)
                    strFahrgestellnummer = txtFahrgestNr.Text
                    'Ausgabeart nur relevant, wenn Fahrgestellnr. angegeben
                    If cbRechnungsausgabe.Checked Then
                        strAusgabeart = "R"
                    Else
                        strAusgabeart = "F"
                    End If
                End If
                If txtRechnDatVon.Text.Length = 0 Then
                    strRechnungsdatumVon = ""
                Else
                    strRechnungsdatumVon = txtRechnDatVon.Text
                End If
                If txtRechnDatBis.Text.Length = 0 Then
                    strRechnungsdatumBis = ""
                Else
                    strRechnungsdatumBis = txtRechnDatBis.Text
                End If
                If txtLeistDatVon.Text.Length = 0 Then
                    strLeistungsdatumVon = ""
                Else
                    strLeistungsdatumVon = txtLeistDatVon.Text
                End If
                If txtLeistDatBis.Text.Length = 0 Then
                    strLeistungsdatumBis = ""
                Else
                    strLeistungsdatumBis = txtLeistDatBis.Text
                End If
                If txtLeistArt.Text.Length = 0 Then
                    strLeistungsart = ""
                Else
                    txtLeistArt.Text = Trim(txtLeistArt.Text)
                    strLeistungsart = txtLeistArt.Text
                End If
                If txtSpediteur.Text.Length = 0 Then
                    strSpediteur = ""
                Else
                    txtSpediteur.Text = Trim(txtSpediteur.Text)
                    strSpediteur = txtSpediteur.Text
                End If

                m_Report.Fill(Session("AppID").ToString, Session.SessionID.ToString, Me, strRechnungsnummer, strFahrgestellnummer, strAusgabeart, strRechnungsdatumVon, strRechnungsdatumBis, strLeistungsdatumVon, strLeistungsdatumBis, strLeistungsart, strSpediteur)

                Session("ResultTable") = m_Report.Result
                Session("ExcelTable") = m_Report.ExcelResult
                If Not m_Report.Status = 0 Then
                    lblError.Text = m_Report.Message
                Else
                    Session("lnkExcel") = "/PortalORM/Temp/Excel/" & strFileName
                    logApp.UpdateEntry("APP", Session("AppID").ToString, "Report: Rechnungsdatenreport")
                    Response.Redirect("/PortalORM/(S(" & Session.SessionID & "))/Shared/Report01_2.aspx?AppID=" & Session("AppID").ToString)
                End If
            End If
            
        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Function checkDate() As Boolean
        Dim tmpbool As Boolean = True
        For Each ctrl As BaseValidator In Me.Validators
            If ctrl.IsValid = False Then
                tmpbool = False
            End If
        Next
        Return tmpbool
    End Function

    Private Function checkInput() As Boolean
        Dim DateVon As Date
        Dim DateBis As Date

        If txtRechnNr.Text.Length + txtFahrgestNr.Text.Length + txtRechnDatVon.Text.Length + txtRechnDatBis.Text.Length + txtLeistDatVon.Text.Length + txtLeistDatBis.Text.Length + txtLeistArt.Text.Length + txtSpediteur.Text.Length = 0 Then
            lblError.Text = "Bitte geben Sie ein Suchkriterium ein."
            Return False
        End If

        'wenn nicht Filter nach Rechn-/Fahrgest-Nr, mind. eine Datumseingrenzung erforderlich
        If txtRechnNr.Text.Length = 0 AndAlso txtFahrgestNr.Text.Length = 0 Then
            If txtRechnDatVon.Text.Length + txtRechnDatBis.Text.Length + txtLeistDatVon.Text.Length + txtLeistDatBis.Text.Length = 0 Then
                lblError.Text = "Es muss mindestens eine der beiden möglichen Datumseingrenzungen(von-bis)erfolgen."
                Return False
            End If
        End If

        If IsDate(txtRechnDatVon.Text) AndAlso IsDate(txtRechnDatBis.Text) Then
            DateVon = CType(txtRechnDatVon.Text, Date)
            DateBis = CType(txtRechnDatBis.Text, Date)
            If DateDiff(DateInterval.Day, DateVon, DateBis) > 90 Then
                lblError.Text = "Ausgewählter Zeitraum des Rechnungsdatums zu groß. Maximal 3 Monate."
                Return False
            End If
        ElseIf IsDate(txtRechnDatVon.Text) AndAlso Not IsDate(txtRechnDatBis.Text) Then
            lblError.Text = "Rechnungsdatum bis muss gefüllt sein!"
            Return False
        ElseIf Not IsDate(txtRechnDatVon.Text) AndAlso IsDate(txtRechnDatBis.Text) Then
            lblError.Text = "Rechnungsdatum von muss gefüllt sein!"
            Return False
        End If
        If IsDate(txtLeistDatVon.Text) AndAlso IsDate(txtLeistDatBis.Text) Then
            DateVon = CType(txtLeistDatVon.Text, Date)
            DateBis = CType(txtLeistDatBis.Text, Date)
            If DateDiff(DateInterval.Day, DateVon, DateBis) > 90 Then
                lblError.Text = "Ausgewählter Zeitraum des Leistungsdatums zu groß. Maximal 3 Monate."
                Return False
            End If
        ElseIf IsDate(txtLeistDatVon.Text) AndAlso Not IsDate(txtLeistDatBis.Text) Then
            lblError.Text = "Leistungsdatum bis muss gefüllt sein!"
            Return False
        ElseIf Not IsDate(txtLeistDatVon.Text) AndAlso IsDate(txtLeistDatBis.Text) Then
            lblError.Text = "Leistungsdatum von muss gefüllt sein!"
            Return False
        End If
        Return True
    End Function

    Protected Sub cmdBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdBack.Click
        Session("ResultTable") = Nothing
        Session("ExcelTable") = Nothing
        Response.Redirect("/PortalORM/(S(" & Session.SessionID & "))/Start/Selection.aspx")
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

End Class