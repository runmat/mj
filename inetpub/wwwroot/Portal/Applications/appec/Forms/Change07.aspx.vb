Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Partial Public Class Change07
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

            If IsPostBack = False Then

                If Me.Page.Request.UrlReferrer.AbsolutePath.Contains("Change07_2") = True Then
                    Reload()
                End If

            End If



        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten."
        End Try
    End Sub

    Private Sub DoSubmit(Optional ByVal Fahrgestellnummer As String = "")
        Dim m_Report As New Unfallmeldung(m_User, m_App, "")
        Try
            Dim strBriefnummer As String
            Dim strFahrgestellnummer As String
            Dim strAmtlKennzeichen As String
            Dim strOrdernummer As String


            If txtAmtlKennzeichen.Text.Length = 0 Then
                strAmtlKennzeichen = ""
            Else

                If txtAmtlKennzeichen.Text.Contains("*") = True Then
                    If txtAmtlKennzeichen.Text.Length < 8 Then
                        lblError.Text = "Kennzeichen-Eingabe unterschreitet Minimallänge."
                        Return
                    End If
                End If


                strAmtlKennzeichen = txtAmtlKennzeichen.Text


            End If

            If txtFahrgestellnummer.Text.Length = 0 Then
                strFahrgestellnummer = ""
            Else

                If txtFahrgestellnummer.Text.Contains("*") = True Then
                    If txtAmtlKennzeichen.Text.Length < 9 Then
                        lblError.Text = "Fahrgestellnummer-Eingabe unterschreitet Minimallänge."
                        Return
                    End If
                End If


                strFahrgestellnummer = txtFahrgestellnummer.Text
            End If

            If txtBriefnummer.Text.Length = 0 Then
                strBriefnummer = ""
            Else

                If txtBriefnummer.Text.Contains("*") = True Then
                    If txtBriefnummer.Text.Length < 5 Then
                        lblError.Text = "Briefnummer-Eingabe unterschreitet Minimallänge."
                        Return
                    End If
                End If

                strBriefnummer = txtBriefnummer.Text
            End If

            If txtOrdernummer.Text.Length = 0 Then
                strOrdernummer = ""
            Else

                If txtOrdernummer.Text.Contains("*") = True Then
                    If txtOrdernummer.Text.Length < 7 Then
                        lblError.Text = "Unitnummer-Eingabe unterschreitet Minimallänge."
                        Return
                    End If
                End If

                strOrdernummer = txtOrdernummer.Text
            End If




            If txtBriefnummer.Text.Length + txtFahrgestellnummer.Text.Length + txtAmtlKennzeichen.Text.Length + txtOrdernummer.Text.Length + Fahrgestellnummer.Length = 0 Then
                lblError.Text = "Bitte geben Sie ein Suchkriterium ein."
            Else

                m_Report.MeldeTyp = rblAuswahl.SelectedValue

                m_Report.FillUnfall(Session("AppID").ToString, Session.SessionID.ToString, strAmtlKennzeichen, strFahrgestellnummer, strBriefnummer, strOrdernummer, Me)


                Session("Unfallmeldung") = m_Report
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

                If m_Report.ResultFahrzeuge.Rows.Count = 1 Then

                    m_Report.EquiNr = m_Report.ResultFahrzeuge.Rows(0)("EQUNR").ToString

                    Session("Unfallmeldung") = m_Report

                    Response.Redirect("Change07_2.aspx?AppID=" & Session("AppID").ToString)
                Else
                    gvSelectOne.DataSource = m_Report.ResultFahrzeuge
                    gvSelectOne.DataBind()
                End If


            End If


        End If
    End Sub


    Protected Sub cmdCreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCreate.Click
        'Session("ShowLink") = "True"
        DoSubmit()
    End Sub


    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub gvSelectOne_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvSelectOne.RowCommand
        If e.CommandName = "weiter" Then

            Dim m_Report As Unfallmeldung = CType(Session("Unfallmeldung"), Unfallmeldung)

            m_Report.EquiNr = e.CommandArgument.ToString

            Session("Unfallmeldung") = m_Report

            Response.Redirect("Change07_2.aspx?AppID=" & Session("AppID").ToString)

            'DoSubmit(e.CommandArgument.ToString)
        End If
    End Sub


    Private Sub Reload()

        If IsNothing(Session("Unfallmeldung")) = False Then

            If CType(Session("Unfallmeldung"), Unfallmeldung).ResultFahrzeuge.Rows.Count > 1 Then
                gvSelectOne.DataSource = CType(Session("Unfallmeldung"), Unfallmeldung).ResultFahrzeuge
                gvSelectOne.DataBind()
            End If

           
        End If


    End Sub

End Class