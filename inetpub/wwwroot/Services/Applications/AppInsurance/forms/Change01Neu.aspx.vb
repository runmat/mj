Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Business
Imports System.Drawing
Imports System.IO

Partial Public Class Change01Neu
    Inherits System.Web.UI.Page
    Private m_App As Security.App
    Private m_User As Security.User
    Private m_change As VFS03
    Private curDate As Date

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        curDate = Date.Today

        If IsPostBack = False Then
            Page.SetFocus(txtVermittlernummer)
            If Not Session("AppChange") Is Nothing Then Session("AppChange") = Nothing
            EnableControls(False)
            FillVersicherungsjahr()
        End If
    End Sub

    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        m_User = GetUser(Me)
        FormAuthNoReferrer(Me, m_User)
        GetAppIDFromQueryString(Me)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        m_App = New Security.App(m_User)
        lblError.Text = ""
        Try
            If Not IsPostBack Then
                Dim item As ListItem
                Dim intLoop As Integer
                For intLoop = 5 To 75 Step 5
                    item = New ListItem(intLoop.ToString, intLoop.ToString)
                    ddlAnzahlKennzeichen.Items.Add(item)
                Next
                For intLoop = 100 To 150 Step 25
                    item = New ListItem(intLoop.ToString, intLoop.ToString)
                    ddlAnzahlKennzeichen.Items.Add(item)
                Next
                ddlAnzahlKennzeichen.SelectedIndex = 0
            End If

            If Session("AppChange") Is Nothing Then
                m_change = New VFS03(m_User, m_App, CStr(Request.QueryString("AppID")), Session.SessionID.ToString, "")
                Session("AppChange") = m_change
            Else
                m_change = CType(Session("AppChange"), VFS03)
            End If
            CheckWEAdresse(chkAdresse.Checked)
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten:<br>" & ex.Message
        End Try
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        ModalPopupExtender2.Show()
    End Sub

    Protected Sub btnOK_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnOK.Click

        If m_change Is Nothing Then
            m_change = CType(Session("AppChange"), VFS03)
        End If

        With m_change

            Dim Adresse As String = ddlAlternativAdressen.SelectedItem.Text

            Dim arrAdressen() As String = Split(Replace(ddlAlternativAdressen.SelectedItem.Text, " ", ""), ",").ToArray

            If chkAdresse.Checked = False Then
                .Postleitzahl = arrAdressen(1)
                txtPostleitzahl.Text = .Postleitzahl

                .Ort = arrAdressen(2)
                txtOrt.Text = .Ort

                .Strasse = arrAdressen(3)
                txtStrasse.Text = .Strasse

                If arrAdressen.Length > 4 Then
                    .Hausnummer = arrAdressen(4)
                    If .WEHausnummer.Length <> 0 Then
                        txtHausnummer.Text = .Hausnummer
                    End If
                ElseIf txtHausnummer.Text = "" Then
                    .Hausnummer = ""
                    txtHausnummer.Text = ""
                    txtHausnummer.Enabled = True
                End If

            Else
                .WEPostleitzahl = arrAdressen(1)
                txtWE_Postleitzahl.Text = .WEPostleitzahl

                .WEOrt = arrAdressen(2)
                txtWE_Ort.Text = .WEOrt

                .WEStrasse = arrAdressen(3)
                txtWE_Strasse.Text = .WEStrasse

                If arrAdressen.Length > 4 Then
                    .WEHausnummer = arrAdressen(4)
                    If .WEHausnummer.Length <> 0 Then
                        txtWE_Hausnummer.Text = .WEHausnummer
                    End If

                ElseIf txtWE_Hausnummer.Text = "" Then
                    .WEHausnummer = ""
                    txtWE_Hausnummer.Text = ""
                    txtWE_Hausnummer.Enabled = True
                End If
            End If


        End With

        lblError.Text = m_change.ErrMessage

        Session("AppChange") = m_change
        'ModalPopupExtender2.Hide()
        mb.Attributes.Add("style", "display:none")
        lblInfoVer.Visible = False
    End Sub

    Private Sub cmdContinue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdContinue.Click

        Try
            If m_change Is Nothing Then
                m_change = CType(Session("AppChange"), VFS03)
            End If

            Dim Vermittlernummer As String = ""
            Dim sNummerTrenn As String = ""
            lblInfoVer.Visible = False

            For i As Integer = 1 To txtVermittlernummer.Text.Length
                If IsNumeric(Mid(txtVermittlernummer.Text, i, 1)) = True Then
                    sNummerTrenn = txtVermittlernummer.Text
                    Vermittlernummer &= Mid(txtVermittlernummer.Text, i, 1)
                End If
            Next

            If cmdContinue.Text.Contains("Weiter") AndAlso txtVermittlernummer.Enabled = True Then

                If Len(Trim(Vermittlernummer)) <> 9 Then
                    lblError.Text = "Bitte geben Sie Ihre neunstellige Agenturnummer ein!"
                    Exit Sub
                End If

                m_change.GetLastOrder(Vermittlernummer, _
                                      Session("AppID").ToString, Session.SessionID.ToString, Page)

                If m_change.Strasse <> "" Then
                    rblAnrede.Items.FindByText(m_change.Anrede).Selected = True
                    txtVorname.Text = m_change.Vorname
                    txtName.Text = m_change.Name
                    txtStrasse.Text = m_change.Strasse
                    txtHausnummer.Text = m_change.Hausnummer
                    txtPostleitzahl.Text = m_change.Postleitzahl
                    txtOrt.Text = m_change.Ort
                    txt_Tel.Text = m_change.Telefon
                    txtEmailAdresse.Text = m_change.EmailAdresse
                    lblLastOrder.Text = m_change.LastOrder
                    lblAnz_Ges.Text = m_change.BestGesamt
                    lblAnz_Off.Text = m_change.OffenBest
                End If

                EnableControls(True)

            ElseIf cmdContinue.Text.Contains("Ändern") Then
                ConfirmMode(False)
            ElseIf cmdContinue.Text = "Abbrechen" Then
                Response.Redirect("/Services/Start/Selection.aspx", False)
            Else

                Dim Stichtag As String = ConfigurationManager.AppSettings("Stichtag")

                litMessage.Text = "Versicherungskennzeichen für das Verkehrsjahr Firstdate/Seconddate können Sie ab dem X. Januar bestellen."

                If curDate >= CDate("01.12." & curDate.Year) Then
                    litMessage.Text = litMessage.Text.Replace("Firstdate", curDate.Year + 1)
                    litMessage.Text = litMessage.Text.Replace("Seconddate", curDate.Year + 2)
                Else
                    litMessage.Text = litMessage.Text.Replace("Firstdate", curDate.Year)
                    litMessage.Text = litMessage.Text.Replace("Seconddate", curDate.Year + 1)
                End If

                litMessage.Text = litMessage.Text.Replace("X", Stichtag)

                Dim booFound As Boolean = False

                For Each item As ListItem In rblVersicherungsjahr.Items
                    If item.Selected = True Then
                        booFound = True
                        Exit For
                    End If
                Next

                If booFound = False Then
                    lblError.Text = "Bitte wählen Sie ein Verkehrsjahr!"
                    Exit Sub
                End If

                'User hat das nächste Versicherungsjahr ausgewählt
                If curDate >= CDate("01.12." & curDate.Year) AndAlso rblVersicherungsjahr.SelectedValue > curDate.Year Then
                    divMessage.Visible = True
                    Exit Sub
                End If

                'User hat das nächste Versicherungsjahr vor dem Stichtag ausgewählt
                If curDate < CDate(Stichtag & ".01." & curDate.Year) AndAlso rblVersicherungsjahr.SelectedValue = curDate.Year Then
                    divMessage.Visible = True
                    Exit Sub
                End If

                Dim Bcolor As Color = System.Drawing.ColorTranslator.FromHtml("#f44b12")
                Dim bError As Boolean

                lblError.Text = ""
                bError = False

                bError = CheckAdresse()
                If Len(Vermittlernummer) = 0 Then
                    bError = True
                    txtVermittlernummer.BackColor = Bcolor

                Else
                    txtVermittlernummer.BackColor = Nothing
                    m_change.Agenturnummer = Vermittlernummer
                    'txtVermittlernummer.Text = Right("000" & txtVermittlernummer.Text, 3)

                End If

                If Not IsNumeric(Vermittlernummer) Then
                    bError = True
                    txtVermittlernummer.BackColor = Bcolor
                End If
                'Pflichtfelder validieren                


                If chkAdresse.Checked = True Then
                    If CheckWEAdresse() = True Then
                        bError = True
                    End If
                End If

                m_change.EmailAdresse = txtEmailAdresse.Text
                m_change.KeineEmailAdresse = chkKeineEmailVorhanden.Checked

                m_change.Versicherungsjahr = rblVersicherungsjahr.SelectedValue

                'Übrige Eingaben übernehmen
                m_change.AnzahlKennzeichen = ddlAnzahlKennzeichen.SelectedItem.Value
                Dim ErrorInfo As String = ""
                txtAnzalAAP.BackColor = Color.Empty
                If txtAnzalAAP.Text.Trim.Length > 0 Then
                    If IsNumeric(txtAnzalAAP.Text) Then
                        If CInt(m_change.AnzahlKennzeichen) < CInt(txtAnzalAAP.Text) Then
                            bError = True
                            ErrorInfo &= "<br/>""Anzahl aap-Vordrucke"" darf max. ""Anzahl Kennzeichen"" betragen."
                            txtAnzalAAP.BackColor = Bcolor
                        Else
                            m_change.AnzahlAAP = CInt(txtAnzalAAP.Text)
                        End If
                    Else
                        bError = True
                        ErrorInfo &= "<br/>Bitte im Feld ""Anzahl aap-Vordrucke"" einen numerischen Wert eingeben."
                        txtAnzalAAP.BackColor = Bcolor
                    End If
                Else
                    bError = True
                    ErrorInfo &= "<br/>Bitte im Feld ""Anzahl aap-Vordrucke"" einen numerischen Wert eingeben."
                    txtAnzalAAP.BackColor = Bcolor
                End If

                If txtEmailAdresse.Text.Trim(" "c).Length > 0 AndAlso txtMailConfirm.Text.Trim(" "c).Length > 0 Then

                    If txtEmailAdresse.Text.Trim(" "c).ToUpper <> txtMailConfirm.Text.Trim(" "c).ToUpper Then
                        bError = True
                        ErrorInfo &= "<br/>Die E-Mailadressen müssen identisch sein."
                        txtEmailAdresse.BackColor = Bcolor
                        txtMailConfirm.BackColor = Bcolor
                    End If

                End If

                If bError = False Then

                    m_change.Confirm = True
                    ConfirmMode(True)

                    m_change.MinMax = False

                    Dim AdditionalInfo As String = "Ihre Bestellung wird zur Freigabe an die Württembergische Versicherung AG weitergeleitet.<br>Grund: "

                    If lblLastOrder.Text.Length > 0 Then
                        If DateDiff(DateInterval.Day, CDate(lblLastOrder.Text), Today) < 15 Then
                            AdditionalInfo &= "Innerhalb der letzten 14 Tage wurden bereits Versicherungskennzeichen bestellt.<br>"
                            m_change.MinMax = True
                        End If
                    End If

                    If CInt(ddlAnzahlKennzeichen.SelectedItem.Text) > 50 Then
                        AdditionalInfo &= "Anzahl von 50 Versicherungskennzeichen überschritten.<br>"
                        m_change.MinMax = True
                    End If


                    If m_change.MinMax = False Then
                        AdditionalInfo = ""
                    End If

                    lblError.Text = AdditionalInfo & "Bitte prüfen Sie Ihre Eingaben. Durch klicken auf 'Ändern' können Sie <br /> Eingaben anpassen, oder bestätigen Sie Ihre Bestellung mit 'Absenden'."
                Else
                    lblError.Text = "Bitte prüfen Sie Ihre Eingaben. Fehlende oder fehlerhafte Daten sind markiert." & ErrorInfo
                End If
                m_change.ErrMessage = lblError.Text
                Session("AppChange") = m_change
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try

    End Sub

    Private Sub cmdConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdConfirm.Click

        Dim bError As Boolean = False
        If cmdConfirm.Text.Contains("Absenden") Then
            bError = CheckAdresse()
            If chkAdresse.Checked = True Then
                If CheckWEAdresse() = True Then
                    bError = True
                End If
            End If
            If bError = False Then
                anfordern()
            End If

        ElseIf cmdConfirm.Text = "Fortfahren&nbsp;&#187;" Then
            bError = CheckAdresse()
            If chkAdresse.Checked = True Then
                If CheckWEAdresse() = True Then
                    bError = True
                End If
            End If
            If bError = False Then
                anfordern()
            End If
        End If
    End Sub

    Private Function CheckAdresse() As Boolean
        Dim Bcolor As Color = System.Drawing.ColorTranslator.FromHtml("#f44b12")
        Dim bError As Boolean

        lblError.Text = ""
        bError = False
        'Pflichtfelder validieren
        If txtVorname.Text.Trim(" "c).Length = 0 Then
            bError = True
            txtVorname.BackColor = Bcolor
        Else
            txtVorname.BackColor = Nothing
            m_change.Vorname = txtVorname.Text
            m_change.Name = txtName.Text
        End If
        If txtStrasse.Text.Trim(" "c).Length = 0 Then
            bError = True
            txtStrasse.BackColor = Bcolor
        Else
            txtStrasse.BackColor = Nothing
            m_change.Strasse = txtStrasse.Text
        End If
        If txtHausnummer.Text.Trim(" "c).Length = 0 Then
            bError = True
            txtHausnummer.BackColor = Bcolor
        Else
            txtHausnummer.BackColor = Nothing
            m_change.Hausnummer = txtHausnummer.Text
        End If
        If txtPostleitzahl.Text.Trim(" "c).Length = 0 Then
            bError = True
            txtPostleitzahl.BackColor = Bcolor
        Else
            txtPostleitzahl.BackColor = Nothing
            m_change.Postleitzahl = txtPostleitzahl.Text
        End If
        If txtOrt.Text.Trim(" "c).Length = 0 Then
            bError = True
            txtOrt.BackColor = Bcolor
        Else
            txtOrt.BackColor = Nothing

            m_change.Ort = txtOrt.Text
        End If
        If txt_Tel.Text.Trim(" "c).Length = 0 Then
            bError = True
            txt_Tel.BackColor = Bcolor
        Else
            txt_Tel.BackColor = Nothing
            m_change.Telefon = txt_Tel.Text
        End If

        If rblAnrede.SelectedIndex = -1 Then
            bError = True
            rblAnrede.BackColor = Bcolor
        Else
            rblAnrede.BackColor = Nothing
            m_change.Anrede = rblAnrede.SelectedValue
        End If

        If txtEmailAdresse.Text.Trim(" "c).Length = 0 Then
            txtEmailAdresse.BackColor = Bcolor
            bError = True
        Else
            'EMailadresse validieren
            If HelpProcedures.EmailAddressCheck(txtEmailAdresse.Text) = False Then
                txtEmailAdresse.BackColor = Bcolor
                bError = True
            Else
                txtEmailAdresse.BackColor = Nothing
            End If

        End If

        If txtMailConfirm.Text.Trim(" "c).Length = 0 Then
            txtMailConfirm.BackColor = Bcolor
            bError = True
        Else
            'EMailadresse validieren
            If HelpProcedures.EmailAddressCheck(txtMailConfirm.Text) = False Then
                txtMailConfirm.BackColor = Bcolor
                bError = True
            Else
                txtMailConfirm.BackColor = Nothing
            End If

        End If

        Return bError
    End Function

    Private Function CheckWEAdresse() As Boolean
        Dim Bcolor As Color = ColorTranslator.FromHtml("#f44b12")
        Dim bError As Boolean = False
        If rblWE_Anrede.SelectedIndex = -1 Then
            lblError.Text &= "Bitte Anrede auswählen.<br>&nbsp;"
            rblWE_Anrede.BackColor = Bcolor
            bError = True
        Else
            rblWE_Anrede.BackColor = Color.Empty
            m_change.WEAnrede = rblWE_Anrede.SelectedValue
        End If

        If txtWE_Vorname.Text.Trim(" "c).Length = 0 Then
            lblError.Text &= "Bitte ""Name o. Firma"" eingeben.<br>&nbsp;"
            bError = True
            txtWE_Vorname.BackColor = Bcolor
        Else
            txtWE_Vorname.BackColor = Color.Empty

            m_change.WEVorname = txtWE_Vorname.Text
            m_change.WEName = txtWE_Name.Text
        End If
        If txtWE_Strasse.Text.Trim(" "c).Length = 0 Then
            lblError.Text &= "Bitte ""Straße"" eingeben.<br>&nbsp;"
            bError = True
            txtWE_Strasse.BackColor = Bcolor
        Else
            txtWE_Strasse.BackColor = Color.Empty
            m_change.WEStrasse = txtWE_Strasse.Text
        End If
        If txtWE_Hausnummer.Text.Trim(" "c).Length = 0 Then
            lblError.Text &= "Bitte ""Hausnummer"" eingeben.<br>&nbsp;"
            bError = True
            txtWE_Hausnummer.BackColor = Bcolor
        Else
            txtWE_Hausnummer.BackColor = Color.Empty

            m_change.WEHausnummer = txtWE_Hausnummer.Text
        End If
        If txtWE_Postleitzahl.Text.Trim(" "c).Length = 0 Then
            lblError.Text &= "Bitte ""Postleitzahl"" eingeben.<br>&nbsp;"
            bError = True
            txtWE_Postleitzahl.BackColor = Bcolor
        Else
            txtWE_Postleitzahl.BackColor = Color.Empty
            m_change.WEPostleitzahl = txtWE_Postleitzahl.Text
        End If
        If txtWE_Ort.Text.Trim(" "c).Length = 0 Then
            lblError.Text &= "Bitte ""Ort"" eingeben.<br>&nbsp;"
            bError = True
            txtWE_Ort.BackColor = Bcolor
        Else
            txtWE_Ort.BackColor = Color.Empty
            m_change.WEOrt = txtWE_Ort.Text
        End If
        If txtWE_Tel.Text.Trim(" "c).Length = 0 Then
            lblError.Text &= "Bitte ""Telefon"" eingeben.<br>&nbsp;"
            bError = True
            txtWE_Tel.BackColor = Bcolor
        Else
            txtWE_Tel.BackColor = Color.Empty
            m_change.WETelefon = txtWE_Tel.Text
        End If
        Return bError
    End Function

    Private Sub anfordern()
        Dim strMessage As String = ""
        Try

            m_change.Bestellen(Me.Page)
            m_change.Complete = True
            cmdContinue.Enabled = False
            cmdConfirm.Enabled = False
            If Not m_change.Status = 0 Then
                lblError.Text = m_change.Message
                Exit Sub
            End If
            If m_change.MinMax = True Then
                strMessage = strMessage & "Sehr geehrte Damen und Herren, "
                strMessage = strMessage & "<br/><br/>die nachfolgende Bestellung von Versicherungskennzeichen muss von Ihnen freigegeben "
                strMessage = strMessage & "<br/>werden, damit eine Lieferung an die Agentur erfolgen kann. "
                strMessage = strMessage & "<br/><br/>Grund hierfür ist entweder eine Bestellung > 50 Versicherungskennzeichen oder eine weitere "
                strMessage = strMessage & "<br/>Bestellung innerhalb von 14 Tagen. "
            Else
                strMessage = strMessage & "<br/>Ihre Bestellung von Versicherungskennzeichen. "
            End If

            If m_change.MinMax = False Then
                strMessage = strMessage & "<br/><br/><span style=""font-size: 16pt;font-weight:bold; color: #DB0000"">Wichtig: Zur Bestätigung Ihrer Bestellung klicken Sie bitte auf folgenden Link: </span>" & vbCrLf & vbCrLf

                'Validation-Link
                Dim CryptKey As String = m_change.GenerateValidationLink(m_change.Agenturnummer & m_change.Auftragsnummer)

                CryptKey = ConfigurationManager.AppSettings("InsuranceValidationUrl") & Server.UrlEncode(CryptKey)

                strMessage = strMessage & "<br/><a style=""font-size: 16pt;font-weight:bold; color: #DB0000"" href='" & CryptKey & "'>" & CryptKey & "</a>"
            End If

            strMessage = strMessage & "<br/><br/>Folgende Daten wurden in der Bestellung angegeben: "
            strMessage = strMessage & "<br/><br/>Anrede: " & rblAnrede.SelectedValue
            strMessage = strMessage & "<br/>Name o. Firma: " & txtVorname.Text.TrimStart("0"c) & " " & txtName.Text.TrimStart("0"c)
            strMessage = strMessage & "<br/>Strasse/Nr.: " & txtStrasse.Text.TrimStart("0"c) & " " & txtHausnummer.Text.TrimStart("0"c)
            strMessage = strMessage & "<br/>PLZ/Ort: " & txtPostleitzahl.Text.TrimStart("0"c) & " " & txtOrt.Text.TrimStart("0"c)
            strMessage = strMessage & "<br/>Telefon: " & txt_Tel.Text.TrimStart(" "c)
            strMessage = strMessage & "<br/><br/>Agenturnummer: " & txtVermittlernummer.Text
            strMessage = strMessage & "<br/>Auftragsnummer: " & m_change.Auftragsnummer
            strMessage = strMessage & "<br/><br/>Anzahl Kennzeichen: " & ddlAnzahlKennzeichen.SelectedItem.Text
            If txtAnzalAAP.Text.Trim.Length > 0 Then
                strMessage = strMessage & "<br/><br/>Anzahl aapVerträge: " & txtAnzalAAP.Text & vbCrLf & vbCrLf
            End If

            If m_change.MinMax = True Then
                strMessage = strMessage & "<br/><br/>Bitte prüfen Sie diesen Vorgang und geben die Bestellung im Freigabe-Center frei oder "
                strMessage = strMessage & "<br/>stornieren diese. "
                strMessage = strMessage & "<br/>Vergessen Sie bitte nicht, die Agentur im Falle eines Stornos oder bei Veränderung der "
                strMessage = strMessage & "<br/>Bestellmenge zu informieren!"
                strMessage = strMessage & "<br/><br/>Vielen Dank."
            End If

            strMessage = strMessage & vbCrLf & "<br/><br/>Achtung: Diese Nachricht wurde automatisch generiert! Bitte antworten Sie nicht darauf!"

            If Not chkKeineEmailVorhanden.Checked Then
                Try
                    Dim Mail As System.Net.Mail.MailMessage
                    Dim smtpMailSender As String = ""
                    Dim smtpMailServer As String = ""
                    smtpMailSender = ConfigurationManager.AppSettings("SmtpMailSender")
                    If m_change.MinMax = True Then
                        Mail = New System.Net.Mail.MailMessage(smtpMailSender, "vkzbearbeitung@wuerttembergische.de", "Bestellung von Versicherungskennzeichen", strMessage)
                        lblError.Text = "Ihre Bestellung wird von der Württembergischen Versicherung AG geprüft."
                    Else
                        Mail = New System.Net.Mail.MailMessage(smtpMailSender, txtEmailAdresse.Text.Trim, "Bestellung von Versicherungskennzeichen", strMessage)
                    End If
                    Mail.IsBodyHtml = True
                    smtpMailServer = ConfigurationManager.AppSettings("SmtpMailServer")
                    Dim client As New System.Net.Mail.SmtpClient(smtpMailServer)
                    client.Send(Mail)
                Catch ex As Exception
                    lblError.Text = "Fehler beim Versenden der E-Mail."
                End Try
            End If
            If m_change.MinMax = False Then
                'lblError.Text = lblError.Text & "<br />Sie erhalten in Kürze eine e-Mail von noreply.sgw@kroschke.de. Hierbei handelt es sich <br />" & _
                '                " um eine Sicherheitsabfrage. Bitte bestätigen Sie darin Ihre Bestellung, damit die Ausführung erfolgen kann."
                divControls.Visible = False
                divInfoText.Visible = True
                lblInfo.Text = ""
            End If

            Session("AppChange") = m_change
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Public Sub ConfirmMode(ByVal enabled As Boolean)
        If enabled Then
            cmdContinue.Text = "Ändern&nbsp;&#187;"
            cmdConfirm.Visible = True
            If Trim(txtHausnummer.Text).Length > 0 Then
                txtHausnummer.Enabled = False
            End If

            txtName.Enabled = False
            txtOrt.Enabled = False
            txtPostleitzahl.Enabled = False
            txtStrasse.Enabled = False
            txtVorname.Enabled = False
            ddlAnzahlKennzeichen.Enabled = False
            txtAnzalAAP.Enabled = False
            txtEmailAdresse.Enabled = False
            txtMailConfirm.Enabled = False
            chkKeineEmailVorhanden.Enabled = False
            txt_Tel.Enabled = False
            rblAnrede.Enabled = False
            rblWE_Anrede.Enabled = False
            rblVersicherungsjahr.Enabled = False
        Else
            cmdContinue.Text = "Weiter&nbsp;&#187;"
            cmdConfirm.Visible = False
            txtHausnummer.Enabled = True
            txtName.Enabled = True
            txtOrt.Enabled = True
            txtPostleitzahl.Enabled = True
            txtStrasse.Enabled = True
            txtVorname.Enabled = True
            ddlAnzahlKennzeichen.Enabled = True
            txtAnzalAAP.Enabled = True
            txtEmailAdresse.Enabled = True
            txtMailConfirm.Enabled = True
            chkKeineEmailVorhanden.Enabled = True
            txt_Tel.Enabled = True
            rblAnrede.Enabled = True
            rblWE_Anrede.Enabled = True
            rblVersicherungsjahr.Enabled = True
        End If
    End Sub

    Protected Sub rbExpress_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)

    End Sub

    Private Sub CheckWEAdresse(ByVal bchecked As Boolean)
        lbl_WEAnrede.Visible = bchecked
        lbl_WEName.Visible = bchecked
        lbl_WEVorname.Visible = bchecked
        lbl_WEStrasse.Visible = bchecked
        lbl_WEPostleitzahl.Visible = bchecked
        lbl_WETel.Visible = bchecked

        rblWE_Anrede.Visible = bchecked
        rblWE_Anrede.SelectedIndex = 1
        txtWE_Name.Visible = bchecked
        txtWE_Vorname.Visible = bchecked
        txtWE_Strasse.Visible = bchecked
        txtWE_Hausnummer.Visible = bchecked
        txtWE_Postleitzahl.Visible = bchecked
        txtWE_Ort.Visible = bchecked
        txtWE_Tel.Visible = bchecked
    End Sub

    Private Sub chkAdresse_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAdresse.CheckedChanged
        CheckWEAdresse(chkAdresse.Checked)
    End Sub

    Private Sub EnableControls(ByVal Enabled As Boolean)

        For Each formControl As Control In divControls.Controls

            If TypeOf formControl Is TextBox Then
                CType(formControl, TextBox).Enabled = Enabled
            ElseIf TypeOf formControl Is RadioButtonList Then
                CType(formControl, RadioButtonList).Enabled = Enabled
            ElseIf TypeOf formControl Is CheckBox Then
                CType(formControl, CheckBox).Enabled = Enabled
            ElseIf TypeOf formControl Is DropDownList Then
                CType(formControl, DropDownList).Enabled = Enabled
            ElseIf TypeOf formControl Is Panel Then
                If formControl.ID = "pnlDefault" Then
                    'formControl.Controls.id

                    For Each PanelControl In formControl.Controls
                        If TypeOf PanelControl Is TextBox Then
                            If CType(PanelControl, TextBox).ID.Contains("Vermittlernummer") Then
                                CType(PanelControl, TextBox).Enabled = Not Enabled
                            End If
                        End If
                    Next

                End If
            End If

        Next

    End Sub

    Private Sub FillVersicherungsjahr()

        Dim dl As ListItem

        'Vorjahr
        If curDate < CDate("01.03." & curDate.Year) Then

            dl = New ListItem()
            dl.Value = curDate.Year - 1
            dl.Text = curDate.Year - 1 & "/" & curDate.Year

            rblVersicherungsjahr.Items.Add(dl)
        End If

        'Aktuelles Jahr
        dl = New ListItem()
        dl.Value = curDate.Year
        dl.Text = curDate.Year & "/" & curDate.Year + 1

        rblVersicherungsjahr.Items.Add(dl)

        'Nächstes Jahr
        If curDate >= CDate("01.12." & curDate.Year) Then
            dl = New ListItem()

            dl.Value = curDate.Year + 1
            dl.Text = curDate.Year + 1 & "/" & curDate.Year + 2

            rblVersicherungsjahr.Items.Add(dl)
        End If

        If rblVersicherungsjahr.Items.Count < 2 Then
            rblVersicherungsjahr.Items(0).Selected = True
        End If

    End Sub

    Private Sub btndefault_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btndefault.Click
        cmdContinue_Click(sender, e)
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click

    End Sub

    Protected Sub cmdOKWarnung_Click(sender As Object, e As EventArgs) Handles cmdOKWarnung.Click
        divMessage.Visible = False
    End Sub

    Protected Sub lbBack_Click(sender As Object, e As EventArgs) Handles lbBack.Click
        Response.Redirect("../../../Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub
End Class

' ************************************************
' $History: Change01Neu.aspx.vb $
' 
' *****************  Version 27  *****************
' User: Rudolpho     Date: 31.01.11   Time: 9:42
' Updated in $/CKAG2/Applications/AppInsurance/forms
' 
' *****************  Version 26  *****************
' User: Rudolpho     Date: 12.01.11   Time: 11:23
' Updated in $/CKAG2/Applications/AppInsurance/forms
' 
' *****************  Version 25  *****************
' User: Rudolpho     Date: 14.12.10   Time: 15:18
' Updated in $/CKAG2/Applications/AppInsurance/forms
' 
' *****************  Version 24  *****************
' User: Rudolpho     Date: 8.12.10    Time: 10:53
' Updated in $/CKAG2/Applications/AppInsurance/forms
' 
' *****************  Version 23  *****************
' User: Rudolpho     Date: 3.12.10    Time: 10:26
' Updated in $/CKAG2/Applications/AppInsurance/forms
' ITA: 4287 (CPC)
' 
' *****************  Version 22  *****************
' User: Rudolpho     Date: 2.12.10    Time: 15:00
' Updated in $/CKAG2/Applications/AppInsurance/forms
' 
' *****************  Version 21  *****************
' User: Fassbenders  Date: 19.11.10   Time: 15:53
' Updated in $/CKAG2/Applications/AppInsurance/forms
' 
' *****************  Version 20  *****************
' User: Rudolpho     Date: 3.02.10    Time: 16:44
' Updated in $/CKAG2/Applications/AppInsurance/forms
' 
' *****************  Version 19  *****************
' User: Rudolpho     Date: 2.02.10    Time: 16:15
' Updated in $/CKAG2/Applications/AppInsurance/forms
' 
' *****************  Version 18  *****************
' User: Rudolpho     Date: 19.01.10   Time: 15:29
' Updated in $/CKAG2/Applications/AppInsurance/forms
' 
' *****************  Version 17  *****************
' User: Rudolpho     Date: 18.01.10   Time: 16:11
' Updated in $/CKAG2/Applications/AppInsurance/forms
' 
' *****************  Version 16  *****************
' User: Rudolpho     Date: 15.01.10   Time: 8:53
' Updated in $/CKAG2/Applications/AppInsurance/forms
' 
' *****************  Version 15  *****************
' User: Rudolpho     Date: 14.01.10   Time: 15:40
' Updated in $/CKAG2/Applications/AppInsurance/forms
' 
' *****************  Version 14  *****************
' User: Rudolpho     Date: 6.01.10    Time: 14:50
' Updated in $/CKAG2/Applications/AppInsurance/forms
' 
' *****************  Version 13  *****************
' User: Rudolpho     Date: 11.12.09   Time: 8:57
' Updated in $/CKAG2/Applications/AppInsurance/forms
' 
' *****************  Version 12  *****************
' User: Rudolpho     Date: 26.11.09   Time: 17:03
' Updated in $/CKAG2/Applications/AppInsurance/forms
' 
' *****************  Version 11  *****************
' User: Rudolpho     Date: 23.11.09   Time: 17:42
' Updated in $/CKAG2/Applications/AppInsurance/forms
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 29.10.09   Time: 15:15
' Updated in $/CKAG2/Applications/AppInsurance/forms
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 28.10.09   Time: 15:27
' Updated in $/CKAG2/Applications/AppInsurance/forms
' 
' *****************  Version 8  *****************
' User: Fassbenders  Date: 26.10.09   Time: 14:30
' Updated in $/CKAG2/Applications/AppInsurance/forms
' ITA: 3249, 3206
' 