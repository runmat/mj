Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements


Public Class Change01
    Inherits System.Web.UI.Page

#Region " Vom Web Form Designer generierter Code "

    'Dieser Aufruf ist für den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: Diese Methode ist für den Web Form-Designer erforderlich
        'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
        InitializeComponent()
    End Sub

#End Region

    Private m_context As HttpContext = HttpContext.Current
    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private m_change As VFS03

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents lbExcel As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Vorname As System.Web.UI.WebControls.Label
    Protected WithEvents txtVorname As System.Web.UI.WebControls.TextBox
    Protected WithEvents lbl_Name As System.Web.UI.WebControls.Label
    Protected WithEvents txtName As System.Web.UI.WebControls.TextBox
    Protected WithEvents lbl_Strasse As System.Web.UI.WebControls.Label
    Protected WithEvents txtStrasse As System.Web.UI.WebControls.TextBox
    Protected WithEvents lbl_Hausnummer As System.Web.UI.WebControls.Label
    Protected WithEvents txtHausnummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents lbl_Postleitzahl As System.Web.UI.WebControls.Label
    Protected WithEvents txtPostleitzahl As System.Web.UI.WebControls.TextBox
    Protected WithEvents lbl_Ort As System.Web.UI.WebControls.Label
    Protected WithEvents txtOrt As System.Web.UI.WebControls.TextBox
    Protected WithEvents lbl_AnzahlKennzeichen As System.Web.UI.WebControls.Label
    Protected WithEvents ddlAnzahlKennzeichen As System.Web.UI.WebControls.DropDownList
    Protected WithEvents rbNormal As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rbExpress As System.Web.UI.WebControls.RadioButton
    Protected WithEvents cmdContinue As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdConfirm As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbl_EmailAdresse As System.Web.UI.WebControls.Label
    Protected WithEvents txtEmailAdresse As System.Web.UI.WebControls.TextBox
    Protected WithEvents chkKeineEmailVorhanden As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkAdresse As System.Web.UI.WebControls.CheckBox
    Protected WithEvents lbl_Vermittlernummer As System.Web.UI.WebControls.Label
    Protected WithEvents txtVermittlernummer1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtVermittlernummer2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents lbl_Versandart As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Bezirk As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Tel As System.Web.UI.WebControls.Label
    Protected WithEvents txt_Tel As System.Web.UI.WebControls.TextBox
    Protected WithEvents lbl_KeineEmailVorhanden As System.Web.UI.WebControls.Label

    Protected WithEvents rblWE_Anrede As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents lbl_WEVorname As System.Web.UI.WebControls.Label
    Protected WithEvents txtWE_Vorname As System.Web.UI.WebControls.TextBox
    Protected WithEvents lbl_WEName As System.Web.UI.WebControls.Label
    Protected WithEvents txtWE_Name As System.Web.UI.WebControls.TextBox
    Protected WithEvents lbl_WEStrasse As System.Web.UI.WebControls.Label
    Protected WithEvents txtWE_Strasse As System.Web.UI.WebControls.TextBox
    Protected WithEvents lbl_WEHausnummer As System.Web.UI.WebControls.Label
    Protected WithEvents txtWE_Hausnummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents lbl_WEPostleitzahl As System.Web.UI.WebControls.Label
    Protected WithEvents txtWE_Postleitzahl As System.Web.UI.WebControls.TextBox
    Protected WithEvents lbl_WEOrt As System.Web.UI.WebControls.Label
    Protected WithEvents txtWE_Ort As System.Web.UI.WebControls.TextBox
    Protected WithEvents lbl_WETel As System.Web.UI.WebControls.Label
    Protected WithEvents txtWE_Tel As System.Web.UI.WebControls.TextBox

    Protected WithEvents ucStyles As Styles
    Protected WithEvents rblAnrede As RadioButtonList

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text
        m_App = New Base.Kernel.Security.App(m_User)

        lblError.Text = ""

        Try
            If Not IsPostBack Then
                If ddlAnzahlKennzeichen.Items.Count = 9 Then
                    Dim item As ListItem
                    Dim intLoop As Integer
                    For intLoop = 10 To 50 Step 5
                        item = New ListItem(intLoop.ToString, intLoop.ToString)
                        ddlAnzahlKennzeichen.Items.Add(item)
                    Next
                End If

                ddlAnzahlKennzeichen.SelectedIndex = 4
            End If

            If Session("AppChange") Is Nothing Then
                m_change = New VFS03(m_User, m_App, CStr(Request.QueryString("AppID")), Session.SessionID.ToString, "")
                Session("AppChange") = m_change
            Else
                m_change = CType(Session("AppChange"), VFS03)
            End If

            chkAdresse.Attributes.Add("onclick", "showhide();")
            insertScript()
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten:<br>" & ex.Message
        End Try
    End Sub
    Private Sub insertScript()
        Dim sScript As String
        If chkAdresse.Checked = True Then
            sScript = "						<script language=""Javascript"">" & vbCrLf
            sScript &= "						  <!-- //" & vbCrLf
            sScript &= "							document.getElementById('trAdresse').style.display='block';" & vbCrLf
            sScript &= "						  //-->" & vbCrLf
            sScript &= "						</script>" & vbCrLf
            Me.Controls.Add(New LiteralControl(sScript))
        End If
    End Sub
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub lbExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbExcel.Click
        Try
            Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
            excelFactory.CreateDocumentAndSendAsResponse(strFileName, m_change.ResultExcel, Me.Page)
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdContinue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdContinue.Click
        Try
            If m_change Is Nothing Then
                m_change = CType(Session("AppChange"), VFS03)
            End If

            If cmdContinue.Text = "Ändern" Then
                ConfirmMode(False)
            ElseIf cmdContinue.Text = "Abbrechen" Then
                Response.Redirect("/Portal/Start/Selection.aspx", False)
            Else
                Dim Bstyle As BorderStyle = BorderStyle.Solid
                Dim Bcolor As Color = Color.Red



                lblError.Text = ""
                'Pflichtfelder validieren
                If txtVorname.Text.Trim(" "c).Length = 0 Then
                    lblError.Text &= "Bitte ""Name o. Firma"" eingeben.<br>&nbsp;"

                    txtVorname.BorderColor = Bcolor
                    txtVorname.BorderStyle = Bstyle

                Else
                    txtVorname.BorderStyle = BorderStyle.NotSet
                    txtVorname.BorderColor = Nothing

                    m_change.Vorname = txtVorname.Text
                    m_change.Name = txtName.Text
                End If
                If txtStrasse.Text.Trim(" "c).Length = 0 Then
                    lblError.Text &= "Bitte ""Straße"" eingeben.<br>&nbsp;"

                    txtStrasse.BorderColor = Bcolor
                    txtStrasse.BorderStyle = Bstyle

                Else

                    txtStrasse.BorderColor = Nothing
                    txtStrasse.BorderStyle = BorderStyle.NotSet

                    m_change.Strasse = txtStrasse.Text
                End If
                If txtHausnummer.Text.Trim(" "c).Length = 0 Then
                    lblError.Text &= "Bitte ""Hausnummer"" eingeben.<br>&nbsp;"

                    txtHausnummer.BorderColor = Bcolor

                    txtHausnummer.BorderStyle = Bstyle

                Else
                    txtHausnummer.BorderStyle = BorderStyle.NotSet
                    txtHausnummer.BorderColor = Nothing

                    m_change.Hausnummer = txtHausnummer.Text
                End If
                If txtPostleitzahl.Text.Trim(" "c).Length = 0 Then
                    lblError.Text &= "Bitte ""Postleitzahl"" eingeben.<br>&nbsp;"

                    txtPostleitzahl.BorderColor = Bcolor

                    txtPostleitzahl.BorderStyle = Bstyle
                Else
                    txtPostleitzahl.BorderStyle = BorderStyle.NotSet
                    txtPostleitzahl.BorderColor = Nothing

                    m_change.Postleitzahl = txtPostleitzahl.Text
                End If
                If txtOrt.Text.Trim(" "c).Length = 0 Then
                    lblError.Text &= "Bitte ""Ort"" eingeben.<br>&nbsp;"


                    txtOrt.BorderColor = Bcolor

                    txtOrt.BorderStyle = Bstyle

                Else
                    txtOrt.BorderStyle = BorderStyle.NotSet
                    txtOrt.BorderColor = Nothing

                    m_change.Ort = txtOrt.Text
                End If
                If txt_Tel.Text.Trim(" "c).Length = 0 Then
                    lblError.Text &= "Bitte ""Telefon"" eingeben.<br>&nbsp;"

                    txt_Tel.BorderColor = Bcolor
                    txt_Tel.BorderStyle = Bstyle
                Else
                    txt_Tel.BorderStyle = BorderStyle.NotSet
                    txt_Tel.BorderColor = Nothing

                    m_change.Telefon = txt_Tel.Text
                End If
                If txtVermittlernummer1.Text.Trim(" "c).Length = 0 OrElse txtVermittlernummer2.Text.Trim(" "c).Length = 0 Then
                    lblError.Text &= "Bitte eine ""Vermittlernummer"" eingeben.<br>&nbsp;"

                    txtVermittlernummer1.BorderColor = Bcolor
                    txtVermittlernummer1.BorderStyle = Bstyle

                    txtVermittlernummer2.BorderColor = Bcolor
                    txtVermittlernummer2.BorderStyle = Bstyle

                Else
                    txtVermittlernummer1.BorderStyle = BorderStyle.NotSet
                    txtVermittlernummer2.BorderStyle = BorderStyle.NotSet

                    txtVermittlernummer1.BorderColor = Nothing
                    txtVermittlernummer2.BorderColor = Nothing


                    txtVermittlernummer1.Text = Right("000" & txtVermittlernummer1.Text, 3)
                    txtVermittlernummer2.Text = Right("00000" & txtVermittlernummer2.Text, 5)


                End If

                'txtVermittlernummer1.Text.Trim(" "c).Length()
                If Not IsNumeric(txtVermittlernummer1.Text) OrElse Not IsNumeric(txtVermittlernummer2.Text) Then
                    lblError.Text &= " ""Vermittlernummer"" ist nicht nummerisch.<br>&nbsp;"

                    txtVermittlernummer1.BorderColor = Bcolor
                    txtVermittlernummer1.BorderStyle = Bstyle

                    txtVermittlernummer2.BorderColor = Bcolor
                    txtVermittlernummer2.BorderStyle = Bstyle

                Else

                    txtVermittlernummer2.BorderStyle = BorderStyle.NotSet
                    txtVermittlernummer2.BorderColor = Nothing


                    If Not m_change.checkVertriebsdirektion(txtVermittlernummer1.Text) Then
                        lblError.Text &= " ""Vermittlernummer"" ist nicht gültig.<br>&nbsp;"

                        txtVermittlernummer1.BorderColor = Bcolor
                        txtVermittlernummer1.BorderStyle = Bstyle

                    Else
                        txtVermittlernummer1.BorderStyle = BorderStyle.NotSet
                        txtVermittlernummer1.BorderColor = Nothing
                        m_change.Agenturnummer = txtVermittlernummer1.Text.Trim(" "c) & txtVermittlernummer2.Text.Trim(" "c)
                    End If
                End If

                If rblAnrede.SelectedIndex = -1 Then
                    lblError.Text &= "Bitte Anrede auswählen.<br>&nbsp;"

                    rblAnrede.BorderColor = Bcolor
                    rblAnrede.BorderStyle = Bstyle
                    rblAnrede.BorderWidth = New Unit(2)
                Else
                    rblAnrede.BorderStyle = BorderStyle.None
                    rblAnrede.BorderColor = Nothing

                    m_change.Anrede = rblAnrede.SelectedValue
                End If


                If Not chkKeineEmailVorhanden.Checked And txtEmailAdresse.Text.Trim(" "c).Length = 0 Then

                    txtEmailAdresse.BorderColor = Bcolor
                    txtEmailAdresse.BorderStyle = Bstyle

                    lblError.Text &= "Bitte ""Email-Adresse"" eingeben bzw. bestätigen, wenn keine solche vorhanden."
                Else
                    txtEmailAdresse.BorderStyle = BorderStyle.NotSet
                    txtEmailAdresse.BorderColor = Nothing
                End If

                If chkAdresse.Checked = True Then
                    If rblWE_Anrede.SelectedIndex = -1 Then
                        lblError.Text &= "Bitte Anrede auswählen.<br>&nbsp;"

                        rblWE_Anrede.BorderColor = Bcolor
                        rblWE_Anrede.BorderStyle = Bstyle
                        rblWE_Anrede.BorderWidth = New Unit(2)
                    Else
                        rblWE_Anrede.BorderStyle = BorderStyle.None
                        rblWE_Anrede.BorderColor = Nothing

                        m_change.WEAnrede = rblWE_Anrede.SelectedValue
                    End If

                    If txtWE_Vorname.Text.Trim(" "c).Length = 0 Then
                        lblError.Text &= "Bitte ""Name o. Firma"" eingeben.<br>&nbsp;"

                        txtWE_Vorname.BorderColor = Bcolor
                        txtWE_Vorname.BorderStyle = Bstyle

                    Else
                        txtWE_Vorname.BorderStyle = BorderStyle.NotSet
                        txtWE_Vorname.BorderColor = Nothing

                        m_change.WEVorname = txtWE_Vorname.Text
                        m_change.WEName = txtWE_Name.Text
                    End If
                    If txtWE_Strasse.Text.Trim(" "c).Length = 0 Then
                        lblError.Text &= "Bitte ""Straße"" eingeben.<br>&nbsp;"

                        txtWE_Strasse.BorderColor = Bcolor
                        txtWE_Strasse.BorderStyle = Bstyle

                    Else

                        txtWE_Strasse.BorderColor = Nothing
                        txtWE_Strasse.BorderStyle = BorderStyle.NotSet

                        m_change.WEStrasse = txtWE_Strasse.Text
                    End If
                    If txtWE_Hausnummer.Text.Trim(" "c).Length = 0 Then
                        lblError.Text &= "Bitte ""Hausnummer"" eingeben.<br>&nbsp;"

                        txtWE_Hausnummer.BorderColor = Bcolor

                        txtWE_Hausnummer.BorderStyle = Bstyle

                    Else
                        txtWE_Hausnummer.BorderStyle = BorderStyle.NotSet
                        txtWE_Hausnummer.BorderColor = Nothing

                        m_change.WEHausnummer = txtWE_Hausnummer.Text
                    End If
                    If txtWE_Postleitzahl.Text.Trim(" "c).Length = 0 Then
                        lblError.Text &= "Bitte ""Postleitzahl"" eingeben.<br>&nbsp;"

                        txtWE_Postleitzahl.BorderColor = Bcolor

                        txtWE_Postleitzahl.BorderStyle = Bstyle
                    Else
                        txtWE_Postleitzahl.BorderStyle = BorderStyle.NotSet
                        txtWE_Postleitzahl.BorderColor = Nothing

                        m_change.WEPostleitzahl = txtWE_Postleitzahl.Text
                    End If
                    If txtWE_Ort.Text.Trim(" "c).Length = 0 Then
                        lblError.Text &= "Bitte ""Ort"" eingeben.<br>&nbsp;"


                        txtWE_Ort.BorderColor = Bcolor

                        txtWE_Ort.BorderStyle = Bstyle

                    Else
                        txtWE_Ort.BorderStyle = BorderStyle.NotSet
                        txtWE_Ort.BorderColor = Nothing

                        m_change.WEOrt = txtWE_Ort.Text
                    End If
                    If txtWE_Tel.Text.Trim(" "c).Length = 0 Then
                        lblError.Text &= "Bitte ""Telefon"" eingeben.<br>&nbsp;"

                        txtWE_Tel.BorderColor = Bcolor
                        txtWE_Tel.BorderStyle = Bstyle
                    Else
                        txtWE_Tel.BorderStyle = BorderStyle.NotSet
                        txtWE_Tel.BorderColor = Nothing

                        m_change.WETelefon = txtWE_Tel.Text
                    End If
                End If

                m_change.EmailAdresse = txtEmailAdresse.Text
                m_change.KeineEmailAdresse = chkKeineEmailVorhanden.Checked

                'Übrige Eingaben übernehmen
                m_change.AnzahlKennzeichen = ddlAnzahlKennzeichen.SelectedItem.Value
                m_change.Express = rbExpress.Checked

                If lblError.Text.Length = 0 Then
                    m_change.Confirm = True
                    ConfirmMode(True)
                    lblError.Text = "Bitte prüfen Sie Ihre Eingaben und bestätigen Sie Ihre Bestellung mit 'Absenden'."
                End If
                Session("AppChange") = m_change
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try

    End Sub

    Private Sub cmdConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdConfirm.Click
        If cmdConfirm.Text = "Absenden" Then
            anfordern()
        ElseIf cmdConfirm.Text = "Fortfahren" Then
            m_change.Mehrfachbestellung = True
            anfordern()
        End If
    End Sub

    Private Sub anfordern()
        Dim strMessage As String = ""
        Try

            m_change.Change()

            If m_change.Status = -2222 Then
                'heute schon eine bestellung getätigt
                cmdConfirm.Text = "Fortfahren"
                cmdContinue.Text = "Abbrechen"
                lblError.Text = "&nbsp;Sie haben heute bereits Bestellungen aufgegeben!<br>&nbsp;Möchten Sie fortfahren oder den Bestellvorgang abbrechen?<br>&nbsp;" & m_change.Message
                lblError.BorderColor = Color.Red
                lblError.BorderStyle = BorderStyle.Solid
                lbl_AnzahlKennzeichen.BorderWidth = New Unit(1)
                Exit Sub
            End If

            m_change.Complete = True


            cmdContinue.Enabled = False
            cmdConfirm.Enabled = False
            If Not m_change.Status = 0 Then
                lblError.Text = m_change.Message
                Exit Sub
            End If

            strMessage = strMessage & "Ihre Bestellung von Mopedkennzeichen." & vbCrLf & vbCrLf
            strMessage = strMessage & "Mit folgenden Werten wurde Ihre Bestellung in unser System übernommen:" & vbCrLf & vbCrLf & vbCrLf
            strMessage = strMessage & "Anrede: " & rblAnrede.SelectedValue & vbCrLf
            strMessage = strMessage & "Name o. Firma: " & txtVorname.Text.TrimStart("0"c) & " " & txtName.Text.TrimStart("0"c) & vbCrLf
            strMessage = strMessage & "Strasse/Nr.: " & txtStrasse.Text.TrimStart("0"c) & " " & txtHausnummer.Text.TrimStart("0"c) & vbCrLf
            strMessage = strMessage & "PLZ/Ort: " & txtPostleitzahl.Text.TrimStart("0"c) & " " & txtOrt.Text.TrimStart("0"c) & vbCrLf
            strMessage = strMessage & "Telefon: " & txt_Tel.Text.TrimStart(" "c) & vbCrLf
            If chkAdresse.Checked Then
                strMessage = strMessage & "#############################################" & vbCrLf & vbCrLf
                strMessage = strMessage & "Abweichende Versandadresse:" & vbCrLf & vbCrLf
                strMessage = strMessage & "Anrede: " & rblWE_Anrede.SelectedValue & vbCrLf
                strMessage = strMessage & "Name o. Firma: " & txtWE_Vorname.Text.TrimStart("0"c) & " " & txtWE_Name.Text.TrimStart("0"c) & vbCrLf
                strMessage = strMessage & "Strasse/Nr.: " & txtWE_Strasse.Text.TrimStart("0"c) & " " & txtWE_Hausnummer.Text.TrimStart("0"c) & vbCrLf
                strMessage = strMessage & "PLZ/Ort: " & txtWE_Postleitzahl.Text.TrimStart("0"c) & " " & txtWE_Ort.Text.TrimStart("0"c) & vbCrLf
                strMessage = strMessage & "Telefon: " & txtWE_Tel.Text.TrimStart(" "c) & vbCrLf
                strMessage = strMessage & "#############################################" & vbCrLf & vbCrLf
            End If
            strMessage = strMessage & "VD: " & txtVermittlernummer1.Text.Trim & txtVermittlernummer2.Text.Trim & vbCrLf
            strMessage = strMessage & "Anzahl Kennzeichen: " & ddlAnzahlKennzeichen.SelectedItem.Text & vbCrLf
            If rbNormal.Checked Then
                strMessage = strMessage & "Versandart: Normal" & vbCrLf
            Else
                strMessage = strMessage & "Versandart: Lieferung innerhalb von 48 Stunden (Versandkosten trägt die Volksfürsorge Versicherung AG)" & vbCrLf
            End If
            strMessage = strMessage & vbCrLf & "Achtung: Diese Nachricht wurde automatisch generiert! Bitte antworten Sie nicht darauf!"

            If Not chkKeineEmailVorhanden.Checked Then
                Try
                    Dim Mail As System.Net.Mail.MailMessage
                    Dim smtpMailSender As String = ""
                    Dim smtpMailServer As String = ""

                    smtpMailSender = ConfigurationManager.AppSettings("SmtpMailSender")
                    Mail = New System.Net.Mail.MailMessage(smtpMailSender, txtEmailAdresse.Text.Trim, "Bestellung von Mopedkennzeichen", strMessage)

                    smtpMailServer = ConfigurationManager.AppSettings("SmtpMailServer")
                    Dim client As New System.Net.Mail.SmtpClient(smtpMailServer)
                    client.Send(Mail)
                Catch ex As Exception
                    lblError.Text = "Fehler beim Versenden der E-Mail."
                End Try
            End If

            lblError.Text = lblError.Text & "<br>Bestellung erfolgreich durchgeführt!"

            Session("AppChange") = m_change
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Public Sub ConfirmMode(ByVal enabled As Boolean)
        If enabled Then
            cmdContinue.Text = "Ändern"
            cmdConfirm.Visible = True
            txtVermittlernummer1.Enabled = False
            txtVermittlernummer2.Enabled = False
            txtHausnummer.Enabled = False
            txtName.Enabled = False
            txtOrt.Enabled = False
            txtPostleitzahl.Enabled = False
            txtStrasse.Enabled = False
            txtVorname.Enabled = False
            ddlAnzahlKennzeichen.Enabled = False
            rbExpress.Enabled = False
            rbNormal.Enabled = False
            txtEmailAdresse.Enabled = False
            chkKeineEmailVorhanden.Enabled = False
            txt_Tel.Enabled = False
            chkAdresse.Enabled = False

            txtWE_Hausnummer.Enabled = False
            txtWE_Name.Enabled = False
            txtWE_Ort.Enabled = False
            txtWE_Postleitzahl.Enabled = False
            txtWE_Strasse.Enabled = False
            txtWE_Vorname.Enabled = False
            txtWE_Tel.Enabled = False
            rblWE_Anrede.Enabled = False
        Else
            cmdContinue.Text = "Weiter"
            cmdConfirm.Visible = False
            txtVermittlernummer1.Enabled = True
            txtVermittlernummer2.Enabled = True
            txtHausnummer.Enabled = True
            txtName.Enabled = True
            txtOrt.Enabled = True
            txtPostleitzahl.Enabled = True
            txtStrasse.Enabled = True
            txtVorname.Enabled = True
            ddlAnzahlKennzeichen.Enabled = True
            rbExpress.Enabled = True
            rbNormal.Enabled = True
            txtEmailAdresse.Enabled = True
            chkKeineEmailVorhanden.Enabled = True
            txt_Tel.Enabled = True
            txtWE_Hausnummer.Enabled = True
            txtWE_Name.Enabled = True
            txtWE_Ort.Enabled = True
            txtWE_Postleitzahl.Enabled = True
            txtWE_Strasse.Enabled = True
            txtWE_Vorname.Enabled = True
            txtWE_Tel.Enabled = True
            rblWE_Anrede.Enabled = True
        End If
    End Sub
End Class

' ************************************************
' $History: Change01.aspx.vb $
' 
' *****************  Version 15  *****************
' User: Jungj        Date: 20.01.09   Time: 15:48
' Updated in $/CKAG/Applications/appvfs/Forms
' michael änderungen
' 
' *****************  Version 14  *****************
' User: Rudolpho     Date: 19.01.09   Time: 14:19
' Updated in $/CKAG/Applications/appvfs/Forms
' ITA: 2498
' 
' *****************  Version 13  *****************
' User: Rudolpho     Date: 8.01.09    Time: 8:44
' Updated in $/CKAG/Applications/appvfs/Forms
' 
' *****************  Version 12  *****************
' User: Jungj        Date: 10.12.08   Time: 14:27
' Updated in $/CKAG/Applications/appvfs/Forms
' ita 2433 nachbesserungen
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 9.12.08    Time: 11:06
' Updated in $/CKAG/Applications/appvfs/Forms
' ITA 2433 testfertig
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 25.11.08   Time: 15:28
' Updated in $/CKAG/Applications/appvfs/Forms
' ITA 2086 nachbesserungen
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 25.11.08   Time: 9:35
' Updated in $/CKAG/Applications/appvfs/Forms
' ita 2086 nachbesserungen
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 24.11.08   Time: 12:56
' Updated in $/CKAG/Applications/appvfs/Forms
' ita 2086 testfertig
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 22.08.08   Time: 9:15
' Updated in $/CKAG/Applications/appvfs/Forms
' ITA 2086 fertig
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 21.07.08   Time: 8:41
' Updated in $/CKAG/Applications/appvfs/Forms
' ITA 2085
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 15.07.08   Time: 8:37
' Updated in $/CKAG/Applications/appvfs/Forms
' ITA 2075
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 14.07.08   Time: 9:07
' Updated in $/CKAG/Applications/appvfs/Forms
' ITA 2075 done
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 25.06.08   Time: 10:12
' Updated in $/CKAG/Applications/appvfs/Forms
' ITA 2028
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 29.04.08   Time: 12:27
' Updated in $/CKAG/Applications/appvfs/Forms
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 17:43
' Created in $/CKAG/Applications/appvfs/Forms
' 
' *****************  Version 16  *****************
' User: Rudolpho     Date: 28.02.08   Time: 9:21
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Forms
' 
' *****************  Version 15  *****************
' User: Rudolpho     Date: 21.02.08   Time: 12:43
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Forms
' 
' *****************  Version 14  *****************
' User: Rudolpho     Date: 21.02.08   Time: 10:29
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Forms
' ITA:1727
' 
' *****************  Version 13  *****************
' User: Rudolpho     Date: 5.02.08    Time: 16:20
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Forms
' ITA:1660
' 
' *****************  Version 12  *****************
' User: Rudolpho     Date: 5.02.08    Time: 9:59
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Forms
' ITA:1660
' 
' *****************  Version 11  *****************
' User: Rudolpho     Date: 4.02.08    Time: 10:52
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Forms
' ITA: 1660
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 1.02.08    Time: 9:31
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Forms
' ITA:1660
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 1.02.08    Time: 9:27
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Forms
' ITA:1660
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 1.02.08    Time: 9:07
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Forms
' ITA: 1660
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 31.01.08   Time: 8:52
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Forms
' ITA 1466
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 30.01.08   Time: 15:29
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Forms
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 30.01.08   Time: 15:12
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Forms
' Bugfixes ITA 1644
' 
' *****************  Version 4  *****************
' User: Uha          Date: 24.01.08   Time: 13:07
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Forms
' ITA 1644: Oberflächenkosmetik (Vertriebsdirektion entfernt) - BAPI nach
' wie vor funktionslos
' 
' *****************  Version 3  *****************
' User: Uha          Date: 23.01.08   Time: 12:36
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Forms
' ITA 1644: Formular mit Prüfung (BAPI immer noch funktionslos)
' 
' *****************  Version 2  *****************
' User: Uha          Date: 22.01.08   Time: 14:52
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Forms
' ITA 1644: Change01 und VFS03 - Vorversion, da BAPI derzeit nur Hülle
' 
' *****************  Version 1  *****************
' User: Uha          Date: 22.01.08   Time: 13:16
' Created in $/CKG/Applications/AppVFS/AppVFSWeb/Forms
' ITA 1644: Change01 und VFS03 (funktionslos) hinzugefügt
' 
' ************************************************
