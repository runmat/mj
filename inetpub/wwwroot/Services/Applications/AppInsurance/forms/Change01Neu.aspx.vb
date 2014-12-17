Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Business
Imports System.Drawing

Partial Public Class Change01Neu
    Inherits Page
    Private _mApp As Security.App
    Private _mUser As Security.User
    Private _mChange As VFS03
    Private _curDate As Date
    Private ReadOnly _errorTextBreak As String = "Beim Laden der Seite ist ein Fehler aufgetreten:<br>"
    Private ReadOnly _errorTextAgenturnummer As String = "Bitte geben Sie Ihre neunstellige Agenturnummer ein!"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        _curDate = Date.Today

        If IsPostBack = False Then
            Page.SetFocus(txtVermittlernummer)
            If Not Session("AppChange") Is Nothing Then Session("AppChange") = Nothing
            EnableControls(False)
            FillVersicherungsjahr()
        End If
    End Sub

    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreLoad
        _mUser = GetUser(Me)
        FormAuthNoReferrer(Me, _mUser)
        GetAppIDFromQueryString(Me)

        lblHead.Text = _mUser.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        _mApp = New Security.App(_mUser)
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
                _mChange = New VFS03(_mUser, _mApp, CStr(Request.QueryString("AppID")), Session.SessionID.ToString, "")
                Session("AppChange") = _mChange
            Else
                _mChange = CType(Session("AppChange"), VFS03)
            End If
            CheckWEAdresse(chkAdresse.Checked)
        Catch ex As Exception
            lblError.Text = _errorTextBreak & ex.Message
        End Try
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Protected Sub Button1Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
        ModalPopupExtender2.Show()
    End Sub

    Protected Sub BtnOkClick(ByVal sender As Object, ByVal e As EventArgs) Handles btnOK.Click

        If _mChange Is Nothing Then
            _mChange = CType(Session("AppChange"), VFS03)
        End If

        With _mChange

 
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

        lblError.Text = _mChange.ErrMessage

        Session("AppChange") = _mChange

        mb.Attributes.Add("style", "display:none")
        lblInfoVer.Visible = False
    End Sub

    Private Sub CmdContinueClick(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdContinue.Click

        Try
            If _mChange Is Nothing Then
                _mChange = CType(Session("AppChange"), VFS03)
            End If

            Dim vermittlernummer As String = ""
            lblInfoVer.Visible = False

            For i As Integer = 1 To txtVermittlernummer.Text.Length
                If IsNumeric(Mid(txtVermittlernummer.Text, i, 1)) = True Then
                    vermittlernummer &= Mid(txtVermittlernummer.Text, i, 1)
                End If
            Next

            If cmdContinue.Text.Contains("Weiter") AndAlso txtVermittlernummer.Enabled = True Then

                If Len(Trim(vermittlernummer)) <> 9 Then
                    lblError.Text = _errorTextAgenturnummer
                    Exit Sub
                End If

                _mChange.GetLastOrder(vermittlernummer, _
                                      Session("AppID").ToString, Session.SessionID.ToString, Page)

                If _mChange.Strasse <> "" Then
                    rblAnrede.Items.FindByText(_mChange.Anrede).Selected = True
                    txtVorname.Text = _mChange.Vorname
                    txtName.Text = _mChange.Name
                    txtStrasse.Text = _mChange.Strasse
                    txtHausnummer.Text = _mChange.Hausnummer
                    txtPostleitzahl.Text = _mChange.Postleitzahl
                    txtOrt.Text = _mChange.Ort
                    txt_Tel.Text = _mChange.Telefon
                    txtEmailAdresse.Text = _mChange.EmailAdresse
                    lblLastOrder.Text = _mChange.LastOrder
                    lblAnz_Ges.Text = _mChange.BestGesamt
                    lblAnz_Off.Text = _mChange.OffenBest
                End If

                EnableControls(True)

            ElseIf cmdContinue.Text.Contains("Ändern") Then
                ConfirmMode(False)
            ElseIf cmdContinue.Text = "Abbrechen" Then
                ' ReSharper disable Html.PathError
                Response.Redirect("/Services/Start/Selection.aspx", False)
                ' ReSharper restore Html.PathError
            Else

                'Dim Stichtag As String = ConfigurationManager.AppSettings("Stichtag")
                Dim stichtag As String = GetApplicationConfigValue("BestellfristJanuarAb", Session("AppID").ToString, _mUser.Customer.CustomerId, _mUser.GroupID)

                If String.IsNullOrEmpty(stichtag) Then Throw New Exception("Key 'BestellfristJanuarAb' nicht in der Tabelle 'ApplicationConfig' gepflegt.")


                litMessage.Text = "Versicherungskennzeichen für das Verkehrsjahr Firstdate/Seconddate können Sie ab dem X. Januar bestellen."

                If _curDate >= CDate("01.12." & _curDate.Year) Then
                    litMessage.Text = litMessage.Text.Replace("Firstdate", _curDate.Year + 1)
                    litMessage.Text = litMessage.Text.Replace("Seconddate", _curDate.Year + 2)
                Else
                    litMessage.Text = litMessage.Text.Replace("Firstdate", _curDate.Year)
                    litMessage.Text = litMessage.Text.Replace("Seconddate", _curDate.Year + 1)
                End If

                litMessage.Text = litMessage.Text.Replace("X", stichtag)

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
                If _curDate >= CDate("01.12." & _curDate.Year) AndAlso rblVersicherungsjahr.SelectedValue > _curDate.Year Then
                    divMessage.Visible = True
                    Exit Sub
                End If

                'User hat das nächste Versicherungsjahr vor dem Stichtag ausgewählt
                If _curDate < CDate(stichtag & ".01." & _curDate.Year) AndAlso rblVersicherungsjahr.SelectedValue = _curDate.Year Then
                    divMessage.Visible = True
                    Exit Sub
                End If

                Dim bcolor = ColorTranslator.FromHtml("#f44b12")
                Dim bError As Boolean

                lblError.Text = ""

                bError = CheckAdresse()
                If Len(vermittlernummer) = 0 Then
                    bError = True
                    txtVermittlernummer.BackColor = bcolor

                Else
                    txtVermittlernummer.BackColor = Nothing
                    _mChange.Agenturnummer = vermittlernummer
 
                End If

                If Not IsNumeric(vermittlernummer) Then
                    bError = True
                    txtVermittlernummer.BackColor = bcolor
                End If

                'Pflichtfelder validieren                
                If chkAdresse.Checked = True Then
                    If CheckWEAdresse() = True Then
                        bError = True
                    End If
                End If

                _mChange.EmailAdresse = txtEmailAdresse.Text
                _mChange.KeineEmailAdresse = chkKeineEmailVorhanden.Checked

                _mChange.Versicherungsjahr = rblVersicherungsjahr.SelectedValue

                'Übrige Eingaben übernehmen
                _mChange.AnzahlKennzeichen = ddlAnzahlKennzeichen.SelectedItem.Value
                Dim errorInfo As String = ""
                txtAnzalAAP.BackColor = Color.Empty
                If txtAnzalAAP.Text.Trim.Length > 0 Then
                    If IsNumeric(txtAnzalAAP.Text) Then
                        If CInt(_mChange.AnzahlKennzeichen) < CInt(txtAnzalAAP.Text) Then
                            bError = True
                            errorInfo &= "<br/>""Anzahl aap-Vordrucke"" darf max. ""Anzahl Kennzeichen"" betragen."
                            txtAnzalAAP.BackColor = bcolor
                        Else
                            _mChange.AnzahlAAP = CInt(txtAnzalAAP.Text)
                        End If
                    Else
                        bError = True
                        errorInfo &= "<br/>Bitte im Feld ""Anzahl aap-Vordrucke"" einen numerischen Wert eingeben."
                        txtAnzalAAP.BackColor = bcolor
                    End If
                Else
                    bError = True
                    errorInfo &= "<br/>Bitte im Feld ""Anzahl aap-Vordrucke"" einen numerischen Wert eingeben."
                    txtAnzalAAP.BackColor = bcolor
                End If

                If txtEmailAdresse.Text.Trim(" "c).Length > 0 AndAlso txtMailConfirm.Text.Trim(" "c).Length > 0 Then

                    If txtEmailAdresse.Text.Trim(" "c).ToUpper <> txtMailConfirm.Text.Trim(" "c).ToUpper Then
                        bError = True
                        errorInfo &= "<br/>Die E-Mailadressen müssen identisch sein."
                        txtEmailAdresse.BackColor = bcolor
                        txtMailConfirm.BackColor = bcolor
                    End If

                End If

                If bError = False Then

                    _mChange.Confirm = True
                    ConfirmMode(True)

                    _mChange.MinMax = False

                    Dim additionalInfo As String = "Ihre Bestellung wird zur Freigabe an die Württembergische Versicherung AG weitergeleitet.<br>Grund: "

                    If lblLastOrder.Text.Length > 0 Then
                        If DateDiff(DateInterval.Day, CDate(lblLastOrder.Text), Today) < 15 Then
                            additionalInfo &= "Innerhalb der letzten 14 Tage wurden bereits Versicherungskennzeichen bestellt.<br>"
                            _mChange.MinMax = True
                        End If
                    End If

                    If CInt(ddlAnzahlKennzeichen.SelectedItem.Text) > 50 Then
                        additionalInfo &= "Anzahl von 50 Versicherungskennzeichen überschritten.<br>"
                        _mChange.MinMax = True
                    End If


                    If _mChange.MinMax = False Then
                        additionalInfo = ""
                    End If

                    lblError.Text = additionalInfo & "Bitte prüfen Sie Ihre Eingaben. Durch klicken auf 'Ändern' können Sie <br /> Eingaben anpassen, oder bestätigen Sie Ihre Bestellung mit 'Absenden'."
                Else
                    lblError.Text = "Bitte prüfen Sie Ihre Eingaben. Fehlende oder fehlerhafte Daten sind markiert." & errorInfo
                End If
                _mChange.ErrMessage = lblError.Text
                Session("AppChange") = _mChange
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try

    End Sub

    Private Sub CmdConfirmClick(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdConfirm.Click

        Dim bError As Boolean
        If cmdConfirm.Text.Contains("Absenden") Then
            bError = CheckAdresse()
            If chkAdresse.Checked = True Then
                If CheckWEAdresse() = True Then
                    bError = True
                End If
            End If
            If bError = False Then
                Anfordern()
            End If

        ElseIf cmdConfirm.Text = "Fortfahren&nbsp;&#187;" Then
            bError = CheckAdresse()
            If chkAdresse.Checked = True Then
                If CheckWEAdresse() = True Then
                    bError = True
                End If
            End If
            If bError = False Then
                Anfordern()
            End If
        End If
    End Sub

    Private Function CheckAdresse() As Boolean
        Dim bcolor As Color = ColorTranslator.FromHtml("#f44b12")
        Dim bError As Boolean

        lblError.Text = ""
        bError = False
        'Pflichtfelder validieren
        If txtVorname.Text.Trim(" "c).Length = 0 Then
            bError = True
            txtVorname.BackColor = bcolor
        Else
            txtVorname.BackColor = Nothing
            _mChange.Vorname = txtVorname.Text
            _mChange.Name = txtName.Text
        End If
        If txtStrasse.Text.Trim(" "c).Length = 0 Then
            bError = True
            txtStrasse.BackColor = bcolor
        Else
            txtStrasse.BackColor = Nothing
            _mChange.Strasse = txtStrasse.Text
        End If
        If txtHausnummer.Text.Trim(" "c).Length = 0 Then
            bError = True
            txtHausnummer.BackColor = bcolor
        Else
            txtHausnummer.BackColor = Nothing
            _mChange.Hausnummer = txtHausnummer.Text
        End If
        If txtPostleitzahl.Text.Trim(" "c).Length = 0 Then
            bError = True
            txtPostleitzahl.BackColor = bcolor
        Else
            txtPostleitzahl.BackColor = Nothing
            _mChange.Postleitzahl = txtPostleitzahl.Text
        End If
        If txtOrt.Text.Trim(" "c).Length = 0 Then
            bError = True
            txtOrt.BackColor = bcolor
        Else
            txtOrt.BackColor = Nothing

            _mChange.Ort = txtOrt.Text
        End If
        If txt_Tel.Text.Trim(" "c).Length = 0 Then
            bError = True
            txt_Tel.BackColor = bcolor
        Else
            txt_Tel.BackColor = Nothing
            _mChange.Telefon = txt_Tel.Text
        End If

        If rblAnrede.SelectedIndex = -1 Then
            bError = True
            rblAnrede.BackColor = bcolor
        Else
            rblAnrede.BackColor = Nothing
            _mChange.Anrede = rblAnrede.SelectedValue
        End If

        If txtEmailAdresse.Text.Trim(" "c).Length = 0 Then
            txtEmailAdresse.BackColor = bcolor
            bError = True
        Else
            'EMailadresse validieren
            If HelpProcedures.EmailAddressCheck(txtEmailAdresse.Text) = False Then
                txtEmailAdresse.BackColor = bcolor
                bError = True
            Else
                txtEmailAdresse.BackColor = Nothing
            End If

        End If

        If txtMailConfirm.Text.Trim(" "c).Length = 0 Then
            txtMailConfirm.BackColor = bcolor
            bError = True
        Else
            'EMailadresse validieren
            If HelpProcedures.EmailAddressCheck(txtMailConfirm.Text) = False Then
                txtMailConfirm.BackColor = bcolor
                bError = True
            Else
                txtMailConfirm.BackColor = Nothing
            End If

        End If

        Return bError
    End Function

    Private Function CheckWEAdresse() As Boolean
        Dim bcolor As Color = ColorTranslator.FromHtml("#f44b12")
        Dim bError As Boolean = False
        If rblWE_Anrede.SelectedIndex = -1 Then
            lblError.Text &= "Bitte Anrede auswählen.<br>&nbsp;"
            rblWE_Anrede.BackColor = bcolor
            bError = True
        Else
            rblWE_Anrede.BackColor = Color.Empty
            _mChange.WEAnrede = rblWE_Anrede.SelectedValue
        End If

        If txtWE_Vorname.Text.Trim(" "c).Length = 0 Then
            lblError.Text &= "Bitte ""Name o. Firma"" eingeben.<br>&nbsp;"
            bError = True
            txtWE_Vorname.BackColor = bcolor
        Else
            txtWE_Vorname.BackColor = Color.Empty

            _mChange.WEVorname = txtWE_Vorname.Text
            _mChange.WEName = txtWE_Name.Text
        End If
        If txtWE_Strasse.Text.Trim(" "c).Length = 0 Then
            lblError.Text &= "Bitte ""Straße"" eingeben.<br>&nbsp;"
            bError = True
            txtWE_Strasse.BackColor = bcolor
        Else
            txtWE_Strasse.BackColor = Color.Empty
            _mChange.WEStrasse = txtWE_Strasse.Text
        End If
        If txtWE_Hausnummer.Text.Trim(" "c).Length = 0 Then
            lblError.Text &= "Bitte ""Hausnummer"" eingeben.<br>&nbsp;"
            bError = True
            txtWE_Hausnummer.BackColor = bcolor
        Else
            txtWE_Hausnummer.BackColor = Color.Empty

            _mChange.WEHausnummer = txtWE_Hausnummer.Text
        End If
        If txtWE_Postleitzahl.Text.Trim(" "c).Length = 0 Then
            lblError.Text &= "Bitte ""Postleitzahl"" eingeben.<br>&nbsp;"
            bError = True
            txtWE_Postleitzahl.BackColor = bcolor
        Else
            txtWE_Postleitzahl.BackColor = Color.Empty
            _mChange.WEPostleitzahl = txtWE_Postleitzahl.Text
        End If
        If txtWE_Ort.Text.Trim(" "c).Length = 0 Then
            lblError.Text &= "Bitte ""Ort"" eingeben.<br>&nbsp;"
            bError = True
            txtWE_Ort.BackColor = bcolor
        Else
            txtWE_Ort.BackColor = Color.Empty
            _mChange.WEOrt = txtWE_Ort.Text
        End If
        If txtWE_Tel.Text.Trim(" "c).Length = 0 Then
            lblError.Text &= "Bitte ""Telefon"" eingeben.<br>&nbsp;"
            bError = True
            txtWE_Tel.BackColor = bcolor
        Else
            txtWE_Tel.BackColor = Color.Empty
            _mChange.WETelefon = txtWE_Tel.Text
        End If
        Return bError
    End Function

    Private Sub Anfordern()
        Dim strMessage As String = ""
        Try

            _mChange.Bestellen(Page)
            _mChange.Complete = True
            cmdContinue.Enabled = False
            cmdConfirm.Enabled = False
            If Not _mChange.Status = 0 Then
                lblError.Text = _mChange.Message
                Exit Sub
            End If
            If _mChange.MinMax = True Then
                strMessage = strMessage & "Sehr geehrte Damen und Herren, "
                strMessage = strMessage & "<br/><br/>die nachfolgende Bestellung von Versicherungskennzeichen muss von Ihnen freigegeben "
                strMessage = strMessage & "<br/>werden, damit eine Lieferung an die Agentur erfolgen kann. "
                strMessage = strMessage & "<br/><br/>Grund hierfür ist entweder eine Bestellung > 50 Versicherungskennzeichen oder eine weitere "
                strMessage = strMessage & "<br/>Bestellung innerhalb von 14 Tagen. "
            Else
                strMessage = strMessage & "<br/>Ihre Bestellung von Versicherungskennzeichen. "
            End If

            If _mChange.MinMax = False Then
                strMessage = strMessage & "<br/><br/><span style=""font-size: 16pt;font-weight:bold; color: #DB0000"">Wichtig: Zur Bestätigung Ihrer Bestellung klicken Sie bitte auf folgenden Link: </span>" & vbCrLf & vbCrLf

                'Validation-Link
                Dim cryptKey As String = _mChange.GenerateValidationLink(_mChange.Agenturnummer & _mChange.Auftragsnummer)

                cryptKey = ConfigurationManager.AppSettings("InsuranceValidationUrl") & Server.UrlEncode(cryptKey)

                strMessage = strMessage & "<br/><a style=""font-size: 16pt;font-weight:bold; color: #DB0000"" href='" & cryptKey & "'>" & cryptKey & "</a>"
            End If

            strMessage = strMessage & "<br/><br/>Folgende Daten wurden in der Bestellung angegeben: "
            strMessage = strMessage & "<br/><br/>Anrede: " & rblAnrede.SelectedValue
            strMessage = strMessage & "<br/>Name o. Firma: " & txtVorname.Text.TrimStart("0"c) & " " & txtName.Text.TrimStart("0"c)
            strMessage = strMessage & "<br/>Strasse/Nr.: " & txtStrasse.Text.TrimStart("0"c) & " " & txtHausnummer.Text.TrimStart("0"c)
            strMessage = strMessage & "<br/>PLZ/Ort: " & txtPostleitzahl.Text.TrimStart("0"c) & " " & txtOrt.Text.TrimStart("0"c)
            strMessage = strMessage & "<br/>Telefon: " & txt_Tel.Text.TrimStart(" "c)
            strMessage = strMessage & "<br/><br/>Agenturnummer: " & txtVermittlernummer.Text
            strMessage = strMessage & "<br/>Auftragsnummer: " & _mChange.Auftragsnummer
            strMessage = strMessage & "<br/><br/>Anzahl Kennzeichen: " & ddlAnzahlKennzeichen.SelectedItem.Text
            If txtAnzalAAP.Text.Trim.Length > 0 Then
                strMessage = strMessage & "<br/><br/>Anzahl aapVerträge: " & txtAnzalAAP.Text & vbCrLf & vbCrLf
            End If

            If _mChange.MinMax = True Then
                strMessage = strMessage & "<br/><br/>Bitte prüfen Sie diesen Vorgang und geben die Bestellung im Freigabe-Center frei oder "
                strMessage = strMessage & "<br/>stornieren diese. "
                strMessage = strMessage & "<br/>Vergessen Sie bitte nicht, die Agentur im Falle eines Stornos oder bei Veränderung der "
                strMessage = strMessage & "<br/>Bestellmenge zu informieren!"
                strMessage = strMessage & "<br/><br/>Vielen Dank."
            End If

            strMessage = strMessage & vbCrLf & "<br/><br/>Achtung: Diese Nachricht wurde automatisch generiert! Bitte antworten Sie nicht darauf!"

            If Not chkKeineEmailVorhanden.Checked Then
                Try
                    Dim mail As Net.Mail.MailMessage
                    Dim smtpMailSender As String
                    Dim smtpMailServer As String
                    smtpMailSender = ConfigurationManager.AppSettings("SmtpMailSender")
                    If _mChange.MinMax = True Then
                        mail = New Net.Mail.MailMessage(smtpMailSender, "vkzbearbeitung@wuerttembergische.de", "Bestellung von Versicherungskennzeichen", strMessage)
                        lblError.Text = "Ihre Bestellung wird von der Württembergischen Versicherung AG geprüft."
                    Else
                        mail = New Net.Mail.MailMessage(smtpMailSender, txtEmailAdresse.Text.Trim, "Bestellung von Versicherungskennzeichen", strMessage)
                    End If
                    mail.IsBodyHtml = True
                    smtpMailServer = ConfigurationManager.AppSettings("SmtpMailServer")
                    Dim client As New Net.Mail.SmtpClient(smtpMailServer)
                    client.Send(mail)
                Catch ex As Exception
                    lblError.Text = "Fehler beim Versenden der E-Mail."
                End Try
            End If
            If _mChange.MinMax = False Then
                divControls.Visible = False
                divInfoText.Visible = True
                lblInfo.Text = ""
            End If

            Session("AppChange") = _mChange
        Catch ex As Exception
            lblError.Text = _errorTextBreak & ex.Message
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

    Private Sub ChkAdresseCheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkAdresse.CheckedChanged
        CheckWEAdresse(chkAdresse.Checked)
    End Sub

    Private Sub EnableControls(ByVal enabled As Boolean)

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
        Dim bestellungVorjahrBis As String = GetApplicationConfigValue("BestellfristVorjahrMoeglichBis", Session("AppID").ToString, _mUser.Customer.CustomerId, _mUser.GroupID)

        If String.IsNullOrEmpty(bestellungVorjahrBis) Then
            lblError.Text = _errorTextBreak & "(Key 'BestellfristVorjahrMoeglichBis' nicht in der Tabelle 'ApplicationConfig' gepflegt)"
            Exit Sub
        End If

        'Vorjahr
        If _curDate <= CDate(bestellungVorjahrBis & _curDate.Year) Then

            dl = New ListItem()
            dl.Value = _curDate.Year - 1
            dl.Text = _curDate.Year - 1 & "/" & _curDate.Year

            rblVersicherungsjahr.Items.Add(dl)
        End If

        'Aktuelles Jahr
        dl = New ListItem()
        dl.Value = _curDate.Year
        dl.Text = _curDate.Year & "/" & _curDate.Year + 1

        rblVersicherungsjahr.Items.Add(dl)

        'Nächstes Jahr
        If _curDate >= CDate("01.12." & _curDate.Year) Then
            dl = New ListItem()

            dl.Value = _curDate.Year + 1
            dl.Text = _curDate.Year + 1 & "/" & _curDate.Year + 2

            rblVersicherungsjahr.Items.Add(dl)
        End If

        If rblVersicherungsjahr.Items.Count < 2 Then
            rblVersicherungsjahr.Items(0).Selected = True
        End If

    End Sub

    Private Sub BtndefaultClick(ByVal sender As Object, ByVal e As EventArgs) Handles btndefault.Click
        CmdContinueClick(sender, e)
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click

    End Sub

    Protected Sub CmdOkWarnungClick(sender As Object, e As EventArgs) Handles cmdOKWarnung.Click
        divMessage.Visible = False
    End Sub

    Protected Sub LbBackClick(sender As Object, e As EventArgs) Handles lbBack.Click
        Response.Redirect("../../../Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub
End Class

