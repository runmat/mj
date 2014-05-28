Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security

Partial Public Class MailPflegeUebersicht
    Inherits System.Web.UI.Page

    Private m_User As User
    Private m_App As App
    Private mSeite As String
    Private mDatum As Date
    Private mObjMails As New Mails
    Private cn As New SqlClient.SqlConnection
    Private Enum Mailstatus
        Auswahl = 0
        NeuerText = 1
        NeueMail = 2
    End Enum

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        cn.ConnectionString = m_User.App.Connectionstring
        AdminAuth(Me, m_User, AdminLevel.Organization)
        m_App = New App(m_User)
        GetAppIDFromQueryString(Me)
        If Not IsPostBack Then
            FillCustomer()
        End If
    End Sub

    Sub ddlFilterCustomer_SelectedIndexChanged(ByVal Sender As Object, ByVal e As System.EventArgs) Handles ddlFilterCustomer.SelectedIndexChanged
        Dim kundenID As Integer = ddlFilterCustomer.SelectedValue
        If kundenID <> 0 Then
            ltlMailtext.Text = ""
            lstMailpool.Items.Clear()
            lstEmpfaenger.Items.Clear()
            lstCC.Items.Clear()
            UpdateListeTexte(kundenID)
        End If

    End Sub

    Sub lstBetreff_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstBetreff.SelectedIndexChanged
        Dim KundeID As Integer = ddlFilterCustomer.SelectedValue
        Dim TextID As Integer = lstBetreff.SelectedValue

        ltlMailtext.Text = mObjMails.TextByTextID(TextID, cn)
        lblVorgangsnummer.Text = mObjMails.VorgangsnummerByTextID(TextID, cn)
        chkAktiv.Checked = mObjMails.AktivByTextID(TextID, cn)

        UpdateListeMailpool(KundeID, TextID)
        UpdateListeEmpfaenger(KundeID, TextID)
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
        lblError.Visible = False
        If ddlFilterCustomer.SelectedValue > 0 Then
            If Session("Aktiv") = 1 Then
                If Session("Update") = True Then
                    mObjMails.UpdateText(Session("TextIdBearbeiten"), Editor1.Content, txtNewBetreff.Text, ddlFilterCustomer.SelectedValue, txtVorgangsnummer.Text, False, cn)
                Else
                    mObjMails.NewText(Editor1.Content, txtNewBetreff.Text, ddlFilterCustomer.SelectedValue, txtVorgangsnummer.Text, False, cn)
                End If

            ElseIf Session("Aktiv") = 2 Then
                mObjMails.NewMail(txtNewMail.Text, ddlFilterCustomer.SelectedValue, cn)
            End If
            if mObjMails.ClassError <> "" then
                lblError.Text = mObjMails.ClassError
                lblError.Visible = True
            else
                Formularwechsel(Mailstatus.Auswahl)
            End if
        Else
            lblError.Text = "Es ist kein Kunde ausgewählt."
            lblError.Visible = True
        End If

        Session("Update") = False
    End Sub

    Protected Sub lbnNewText_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbnNewText.Click
        lstVorgangsnummer.DataSource = mObjMails.GetVorgangsnummernList(cn, ddlFilterCustomer.SelectedValue)
        lstVorgangsnummer.DataTextField = "Vorgangsnummer"
        lstVorgangsnummer.DataBind()

        Formularwechsel(Mailstatus.NeuerText)
    End Sub

    Protected Sub lbnNewMail_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbnNewMail.Click
        Formularwechsel(Mailstatus.NeueMail)
    End Sub

    Protected Sub btnAbbrechen_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAbbrechen.Click
        Formularwechsel(Mailstatus.Auswahl)
    End Sub

    Protected Sub lbnMailtoEmpf_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbnMailtoEmpf.Click
        If lstMailpool.SelectedIndex >= 0
            mObjMails.Empfaenger_Aktualisieren(lstMailpool.SelectedValue, lstBetreff.SelectedValue, False, cn)
            UpdateListeEmpfaenger(ddlFilterCustomer.SelectedValue, lstBetreff.SelectedValue)
        End if
    End Sub

    Protected Sub lbnMailtoCC_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbnMailtoCC.Click
        If lstMailpool.SelectedIndex >= 0
            mObjMails.Empfaenger_Aktualisieren(lstMailpool.SelectedValue, lstBetreff.SelectedValue, True, cn)
            UpdateListeEmpfaenger(ddlFilterCustomer.SelectedValue, lstBetreff.SelectedValue)
        End if
    End Sub

    Protected Sub lbnCCtoMail_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbnCCtoMail.Click
        If lstCC.SelectedIndex >= 0        
            mObjMails.Empfaenger_Loeschen(lstCC.SelectedValue, cn)
            UpdateListeEmpfaenger(ddlFilterCustomer.SelectedValue, lstBetreff.SelectedValue)
        End if
    End Sub

    Protected Sub lbnEmpftoMail_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbnEmpftoMail.Click
        If lstEmpfaenger.SelectedIndex >= 0
            mObjMails.Empfaenger_Loeschen(lstEmpfaenger.SelectedValue, cn)
            UpdateListeEmpfaenger(ddlFilterCustomer.SelectedValue, lstBetreff.SelectedValue)
        End if
    End Sub

    Protected Sub lbnTextLoeschen_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibnTextLoeschen.Click
        If lstBetreff.SelectedIndex > -1 Then
            mObjMails.Text_Loeschen(lstBetreff.SelectedValue, cn)
            UpdateListeTexte(ddlFilterCustomer.SelectedValue)
            lstMailpool.Items.Clear()
            lstEmpfaenger.Items.Clear()
            lstCC.Items.Clear()
        End If

    End Sub

    Protected Sub btnVorgangsnrListe_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnVorgangsnrListe.Click
        If grid.Visible = False Then
            grid.Visible = True
        Else
            grid.Visible = False
        End If
    End Sub

    Private Sub chkAktiv_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAktiv.CheckedChanged
        If lstBetreff.SelectedIndex > -1 Then
            mObjMails.AktivChange(lstBetreff.SelectedValue, chkAktiv.Checked, cn)
        End If

    End Sub

    Protected Sub ibnTextBearbeiten_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibnTextBearbeiten.Click
        If lstBetreff.SelectedIndex > -1 Then

            Dim TextID As Integer = lstBetreff.SelectedValue
            Dim Textdaten As DataTable = mObjMails.TextAllByTextID(TextID, cn)

            If mObjMails.ClassError = "" Then

                lstVorgangsnummer.DataSource = mObjMails.GetVorgangsnummernList(cn, ddlFilterCustomer.SelectedValue)
                lstVorgangsnummer.DataTextField = "Vorgangsnummer"
                lstVorgangsnummer.DataBind()

                Session("Update") = True
                Session("TextIdBearbeiten") = lstBetreff.SelectedValue
                Formularwechsel(Mailstatus.NeuerText)

                txtVorgangsnummer.Text = Textdaten.Rows(0)("Vorgangsnummer").ToString
                Editor1.Content = Textdaten.Rows(0)("Text").ToString
                txtNewBetreff.Text = Textdaten.Rows(0)("Betreff").ToString
            Else
                lblError.Text = "Fehler beim aufrufen des Textes."
                lblError.Visible = True
            End If

        End If
    End Sub
#End Region

#Region "Methods"
    Sub Formularwechsel(ByVal Ansicht As Integer)

        Select Case Ansicht

            Case Mailstatus.NeuerText
                trNewMail.Visible = False

                trNewText1.Visible = True
                trNewText2.Visible = True

                trSelectMail1.Visible = False
                trSelectMail2.Visible = False
                trSelectMail3.Visible = False
                trSelectMail4.Visible = False
                lblVorgangsnummer.Visible = False
                lblVorgangsnr.Visible = False

                btnSave.Visible = True
                btnAbbrechen.Visible = True

                lbnNewMail.Visible = False
                lbnNewText.Visible = False

                lblError.Visible = False

                Session("Aktiv") = 1

            Case Mailstatus.NeueMail

                trNewMail.Visible = True

                trNewText1.Visible = False
                trNewText2.Visible = False

                trSelectMail1.Visible = False
                trSelectMail2.Visible = False
                trSelectMail3.Visible = False
                trSelectMail4.Visible = False
                lblVorgangsnummer.Visible = False
                lblVorgangsnr.Visible = False

                btnSave.Visible = True
                btnAbbrechen.Visible = True

                lbnNewMail.Visible = False
                lbnNewText.Visible = False

                lblError.Visible = False

                Session("Aktiv") = 2

            Case Else
                trNewMail.Visible = False

                trNewText1.Visible = False
                trNewText2.Visible = False

                trSelectMail1.Visible = True
                trSelectMail2.Visible = True
                trSelectMail3.Visible = True
                trSelectMail4.Visible = True
                lblVorgangsnummer.Visible = True
                lblVorgangsnr.Visible = True

                btnSave.Visible = False
                btnAbbrechen.Visible = False

                lbnNewMail.Visible = True
                lbnNewText.Visible = True

                lblError.Visible = False
                UpdateListeTexte(ddlFilterCustomer.SelectedValue)
                lstMailpool.Items.Clear()
                lstEmpfaenger.Items.Clear()
                lstCC.Items.Clear()

                lblVorgangsnummer.Text = ""
                ltlMailtext.Text = ""

                Session("Aktiv") = 0

        End Select

    End Sub

    Private Sub FillCustomer()
        cn.Open()
        Try
            Dim dtCustomers As Kernel.CustomerList
            dtCustomers = New Kernel.CustomerList(m_User.Customer.AccountingArea, cn, True, False)

            Dim dv As DataView = dtCustomers.DefaultView
            dv.Sort = "Customername"
            Session.Add("AppCustomerListView", dv)

            With ddlFilterCustomer
                .DataSource = dv
                .DataTextField = "Customername"
                .DataValueField = "CustomerID"
                .DataBind()
            End With

            ddlFilterCustomer.Items(0).Text = "Auswahl"

        Catch ex As Exception
            lblError.Text = "Fehler beim Laden der Kunden"
        Finally
            cn.Close()
        End Try
    End Sub

    Sub UpdateListeMailpool(ByVal KundeID As Integer, ByVal TextID As Integer)
        lstMailpool.DataSource = mObjMails.GetMailList(KundeID, cn)
        lstMailpool.DataTextField = "EmailAdresse"
        lstMailpool.DataValueField = "MailID"
        lstMailpool.DataBind()
    End Sub

    Sub UpdateListeEmpfaenger(ByVal KundeID As Integer, ByVal TextID As Integer)
        lstEmpfaenger.DataSource = mObjMails.GetEmpfaengerList(TextID, KundeID, cn)
        lstEmpfaenger.DataTextField = "EmailAdresse"
        lstEmpfaenger.DataValueField = "EmailVersandID"
        lstEmpfaenger.DataBind()

        lstCC.DataSource = mObjMails.GetEmpfaengerListCC(TextID, KundeID, cn)
        lstCC.DataTextField = "EmailAdresse"
        lstCC.DataValueField = "EmailVersandID"
        lstCC.DataBind()
    End Sub

    Sub UpdateListeTexte(ByVal kundenID As Integer)
        lstBetreff.DataSource = mObjMails.GetTextList(kundenID, cn)
        lstBetreff.DataTextField = "Betreff"
        lstBetreff.DataValueField = "TextID"
        lstBetreff.DataBind()
    End Sub
#End Region

End Class