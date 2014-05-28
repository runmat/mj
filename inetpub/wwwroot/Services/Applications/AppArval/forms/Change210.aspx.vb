Imports CKG
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Services.PageElements
Imports CKG.Base.Kernel.Common.Common


Partial Public Class Change210
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            m_User = GetUser(Me)

            FormAuth(Me, m_User)

            GetAppIDFromQueryString(Me)
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

            m_App = New Base.Kernel.Security.App(m_User)

            If Not IsPostBack Then
                lblAuftragsdatum.Text = Format(Now, "dd.MM.yyyy")
                trContinue.Visible = True
                trBack.Visible = False
                trConfirm.Visible = False
                trNew.Visible = False
                txtSachbearbeiter.Text = m_User.UserName
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdContinue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdContinue.Click
        If (txtLeasingvertragsNr.Text = String.Empty) Then
            lblError.Text = "Leasingvertragsnummer muß angegeben werden."
            Exit Sub
        End If
        If (txtLeasingvertragsNr.Text.Trim() = "") Then
            lblError.Text = "Leasingvertragsnummer muß angegeben werden."
            Exit Sub
        End If
        SwitchControls(False)
        trContinue.Visible = False
        trBack.Visible = True
        trConfirm.Visible = True
        trNew.Visible = False
        lblError.Text = "Sie haben folgenden Auftrag vorbereitet."
    End Sub

    Private Sub SwitchControls(ByVal blnStatus As Boolean)
        Dim control As Control
        Dim control2 As Control
        Dim checkbox As CheckBox
        Dim textbox As TextBox

        For Each control In Me.Controls
            If control.ID = "Form1" Then
                For Each control2 In control.Controls
                    If TypeOf control2 Is CheckBox Then
                        checkbox = CType(control2, CheckBox)
                        checkbox.Enabled = blnStatus
                        If trNew.Visible Then
                            checkbox.Checked = False
                        End If
                    End If
                    If TypeOf control2 Is TextBox Then
                        textbox = CType(control2, TextBox)
                        textbox.Enabled = blnStatus
                        If trNew.Visible Then
                            textbox.Text = ""
                        End If
                    End If
                Next
            End If
        Next
    End Sub

    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBack.Click
        DoOn()
        lblError.Text = ""
    End Sub

    Private Sub DoOn()
        SwitchControls(True)
        trContinue.Visible = True
        trBack.Visible = False
        trConfirm.Visible = False
        trNew.Visible = False
    End Sub

    Private Sub cmdConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdConfirm.Click
        Try
            Dim strHelp As String
            Dim strTemp As String = "Datum: " & lblAuftragsdatum.Text & vbCrLf
            strTemp &= "Leasingvertrags-Nr.: " & txtLeasingvertragsNr.Text & vbCrLf
            strTemp &= "Sachbearbeiter/Tel.: " & txtSachbearbeiter.Text & vbCrLf
            strTemp &= vbCrLf
            strTemp &= "Auftrag für:" & vbCrLf
            strHelp = "_"
            If cbxErsatzKfzSchein.Checked Then
                strHelp = "X"
            End If
            strTemp &= strHelp & " Ersatz Kfz-Schein" & vbCrLf
            strHelp = "_"
            If cbxNeuesSchild.Checked Then
                strHelp = "X"
            End If
            strTemp &= strHelp & " Neues Schild" & vbCrLf
            strHelp = "_"
            If cbxUmmeldung.Checked Then
                strHelp = "X"
            End If
            strTemp &= strHelp & " Ummeldung" & vbCrLf
            strHelp = "_"
            If cbxUmkennzeichnung.Checked Then
                strHelp = "X"
            End If
            strTemp &= strHelp & " Umkennzeichnung" & vbCrLf
            strHelp = "_"
            If cbxVersicherungswechsel.Checked Then
                strHelp = "X"
            End If
            strTemp &= strHelp & " Versicherungswechsel" & vbCrLf
            strHelp = "_"
            If cbxSonstiges.Checked Then
                strHelp = "X"
            End If
            strTemp &= strHelp & " Sonstiges" & vbCrLf
            strHelp = "_"
            If cbxBriefaufbietung.Checked Then
                strHelp = "X"
            End If
            strTemp &= strHelp & " Briefaufbietung" & vbCrLf
            strHelp = "_"
            If cbxTempversand.Checked Then
                strHelp = "X"
            End If
            strTemp &= strHelp & " Temp. Versand" & vbCrLf
            strTemp &= vbCrLf
            strTemp &= "Fahrzeugschein/-schilder an:" & vbCrLf
            strTemp &= "Firma: " & txtZielFirma.Text & vbCrLf
            strTemp &= "Firma2: " & txtZielFirma2.Text & vbCrLf
            strTemp &= "Strasse: " & txtZielStrasse.Text & " " & txtZielHNr.Text & vbCrLf
            strTemp &= "Ort: " & txtZielPLZ.Text & " " & txtZielOrt.Text & vbCrLf
            strTemp &= "Tel: " & txtZielTelefon.Text & vbCrLf
            strTemp &= vbCrLf
            strTemp &= "Alter Halter:" & vbCrLf
            strTemp &= "Firma: " & txtHalterAltFirma.Text & vbCrLf
            strTemp &= "Firma2: " & txtHalterAltFirma2.Text & vbCrLf
            strTemp &= "Strasse: " & txtHalterAltStrasse.Text & " " & txtHalterAltHNr.Text & vbCrLf
            strTemp &= "Ort: " & txtHalterAltPLZ.Text & " " & txtHalterAltOrt.Text & vbCrLf
            strTemp &= "Tel: " & txtHalterAltTelefon.Text & vbCrLf
            strTemp &= vbCrLf
            strTemp &= "Neuer Halter:" & vbCrLf
            strTemp &= "Firma: " & txtHalterNeuFirma.Text & vbCrLf
            strTemp &= "Firma2: " & txtHalterNeuFirma2.Text & vbCrLf
            strTemp &= "Strasse: " & txtHalterNeuStrasse.Text & " " & txtHalterNeuHNr.Text & vbCrLf
            strTemp &= "Ort: " & txtHalterNeuPLZ.Text & " " & txtHalterNeuOrt.Text & vbCrLf
            strTemp &= "Tel: " & txtHalterNeuTelefon.Text & vbCrLf
            strTemp &= vbCrLf
            strTemp &= "Fahrzeugdaten:" & vbCrLf
            strTemp &= "Amtliches Kennzeichen: " & txtAmtlichesKennzeichen.Text & vbCrLf
            strTemp &= vbCrLf
            strTemp &= "Wunschkennzeichen:" & vbCrLf
            strTemp &= "1: " & txtWKZ1.Text & vbCrLf
            strTemp &= "2: " & txtWKZ2.Text & vbCrLf
            strTemp &= "3: " & txtWKZ3.Text & vbCrLf
            strTemp &= vbCrLf
            strTemp &= "Reservierungsnummer/-name:" & vbCrLf
            strTemp &= txtReservierungsnummer.Text & vbCrLf
            strTemp &= vbCrLf
            strTemp &= "Gewünschter Zulassungstermin:" & vbCrLf
            strTemp &= txtZulassungstermin.Text & vbCrLf
            strTemp &= vbCrLf
            strTemp &= "Alter Versicherungsträger:" & vbCrLf
            strTemp &= txtVersicherungAlt.Text & vbCrLf
            strTemp &= vbCrLf
            strTemp &= "Neuer Versicherungsträger:" & vbCrLf
            strTemp &= txtVersicherungNeu.Text & vbCrLf
            strTemp &= vbCrLf
            strTemp &= "Bemerkung:" & vbCrLf
            strTemp &= txtBemerkung.Text & vbCrLf

            'Absenden
            'Dim Server As New System.Net.Mail.SmtpClient(ConfigurationManager.AppSettings("SmtpMailServer"))
            'Dim Mail As New System.Net.Mail.MailMessage(ConfigurationManager.AppSettings("SmtpMailSender"), ConfigurationManager.AppSettings("SmtpMailAddress"), "Sonstige Dienstleistung für Kunden " & m_User.Customer.KUNNR, strTemp)
            'Server.Send(Mail)

            System.Web.Mail.SmtpMail.SmtpServer = ConfigurationManager.AppSettings("SmtpMailServer")
            System.Web.Mail.SmtpMail.Send(ConfigurationManager.AppSettings("SmtpMailSender"), ConfigurationManager.AppSettings("SmtpMailAddress"), "Sonstige Dienstleistung für Kunden " & m_User.Customer.KUNNR, strTemp)

            'Message bringen
            lblError.Text = "Auftrag erfolgreich versendet."

            trContinue.Visible = False
            trBack.Visible = False
            trConfirm.Visible = False
            trNew.Visible = True
        Catch ex As Exception
            lblError.Text = "Fehler bei der Datenübermittlung.<br> (" & ex.Message & ")"
            trContinue.Visible = False
            trBack.Visible = True
            trConfirm.Visible = False
            trNew.Visible = False
        End Try
    End Sub

    Private Sub cmdNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdNew.Click
        DoOn()
        lblError.Text = ""
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Change210.aspx.vb $
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 21.04.09   Time: 11:52
' Created in $/CKAG2/Applications/AppArval/forms
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 1.07.08    Time: 16:01
' Updated in $/CKAG/Applications/apparval/Forms
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 16.04.08   Time: 11:53
' Updated in $/CKAG/Applications/apparval/Forms
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 16:18
' Created in $/CKAG/Applications/apparval/Forms
' 
' *****************  Version 9  *****************
' User: Fassbenders  Date: 8.11.07    Time: 10:01
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Forms
' 
' *****************  Version 8  *****************
' User: Uha          Date: 2.07.07    Time: 10:05
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 7  *****************
' User: Uha          Date: 6.03.07    Time: 15:30
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Forms
' GetAppIDFromQueryString(Me) hinzugefügt
' 
' *****************  Version 6  *****************
' User: Uha          Date: 6.03.07    Time: 14:48
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Forms
' 
' ************************************************
