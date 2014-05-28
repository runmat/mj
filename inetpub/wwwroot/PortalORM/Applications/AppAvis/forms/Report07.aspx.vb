Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common


Partial Public Class Report07
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        If (Not IsPostBack) Then
            cbWhat.Visible = False
            cbWhat.Checked = True
        End If

        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            'DoSubmit()
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        Session("ShowLink") = "True"
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        Dim m_Report As New Fahrzeughistorie(m_User, m_App, "")
        Try
            Dim strBriefnummer As String
            Dim strFahrgestellnummer As String
            Dim strAmtlKennzeichen As String
            Dim strOrdernummer As String
            Dim mProduktionskennziffer As String

            If txtOrdernummer.Text.Length = 0 Then
                strOrdernummer = ""
            Else
                txtOrdernummer.Text = Trim(txtOrdernummer.Text)
                strOrdernummer = txtOrdernummer.Text
            End If
            If txtBriefnummer.Text.Length = 0 Then
                strBriefnummer = ""
            Else
                txtBriefnummer.Text = Trim(txtBriefnummer.Text)
                strBriefnummer = txtBriefnummer.Text
            End If
            If txtFahrgestellnummer.Text.Length = 0 Then
                strFahrgestellnummer = ""
            Else
                txtFahrgestellnummer.Text = Trim(txtFahrgestellnummer.Text)
                strFahrgestellnummer = txtFahrgestellnummer.Text
            End If
            If txtAmtlKennzeichen.Text.Length = 0 Then
                strAmtlKennzeichen = ""
            Else
                txtAmtlKennzeichen.Text = Trim(txtAmtlKennzeichen.Text)
                strAmtlKennzeichen = txtAmtlKennzeichen.Text
            End If
            If txtProduktionskennziffer.Text.Length = 0 Then
                mProduktionskennziffer = ""
            Else
                txtProduktionskennziffer.Text = Trim(txtProduktionskennziffer.Text)
                mProduktionskennziffer = txtProduktionskennziffer.Text
            End If

            If txtBriefnummer.Text.Length + txtFahrgestellnummer.Text.Length + txtAmtlKennzeichen.Text.Length + txtOrdernummer.Text.Length + txtProduktionskennziffer.Text.Length = 0 Then
                lblError.Text = "Bitte geben Sie ein Suchkriterium ein."
            Else

                m_Report.SapSourceORM = cbWhat.Checked
                m_Report.Fill(Session("AppID").ToString, Session.SessionID.ToString, strAmtlKennzeichen, strFahrgestellnummer, strBriefnummer, strOrdernummer, mProduktionskennziffer)

                Session("App_Report") = m_Report
            End If
        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try

        If Not m_Report.Status = 0 Then
            lblError.Text = m_Report.Message
        Else
            If (m_Report.Grunddaten Is Nothing) OrElse (m_Report.Grunddaten.Rows.Count = 0) Then
                lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
            Else
                Session("App_Historie") = m_Report.Historie
                Session("App_Adressen") = m_Report.Adressen
                Session("App_Grunddaten") = m_Report.Grunddaten
                Session("App_LastChange") = m_Report.LastChange
                Session("App_Equidaten") = m_Report.Equidaten
                Response.Redirect("Report07_2.aspx?AppID=" & Session("AppID").ToString)
            End If
        End If
    End Sub

    Protected Sub cmdBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdBack.Click
        Session("App_Historie") = Nothing
        Session("App_Adressen") = Nothing
        Session("App_LastChange") = Nothing
        Session("App_Equidaten") = Nothing
        Session("App_Report") = Nothing
        Response.Redirect("/PortalORM/(S(" & Session.SessionID & "))/Start/Selection.aspx")
    End Sub
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class
' ************************************************
' $History: Report07.aspx.vb $
' 
' *****************  Version 6  *****************
' User: Dittbernerc  Date: 9.06.09    Time: 17:17
' Updated in $/CKAG/Applications/AppAvis/forms
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 28.04.09   Time: 17:09
' Updated in $/CKAG/Applications/AppAvis/forms
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 13.01.09   Time: 8:36
' Updated in $/CKAG/Applications/AppAvis/forms
' Bugfix Fahrzeughistorie
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 6.01.09    Time: 14:15
' Updated in $/CKAG/Applications/AppAvis/forms
' Anpassungen
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 5.01.09    Time: 17:07
' Updated in $/CKAG/Applications/AppAvis/forms
' ITA: 2389
' 
' ************************************************
