Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common

Partial Public Class Change04s
    Inherits System.Web.UI.Page

#Region "Declarations"
    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
#End Region

#Region "Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        m_User = GetUser(Me)

        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)


        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

        If Not IsPostBack Then
            lblAuftragsdatum.Text = Format(Now, "dd.MM.yyyy")
            cmdContinue.Visible = True
            cmdCancel.Visible = False
            cmdConfirm.Visible = False
            cmdNew.Visible = False
            txtSachbearbeiter.Text = m_User.UserName
        End If


    End Sub


    Protected Sub cmdContinue_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdContinue.Click

        If lbl_Vertragsnummer.Visible = True Then
            If (txt_Vertragsnummer.Text = String.Empty) Then
                lblError.Text = "Leasingvertragsnummer muß angegeben werden."
                Exit Sub
            End If
        Else
            If (txtKennz.Text = String.Empty) Then
                lblError.Text = "Kennzeichen muß angegeben werden."
                Exit Sub
            End If
        End If

        
        
        SwitchControls(False)
        cmdContinue.Visible = False
        cmdCancel.Visible = True
        cmdConfirm.Visible = True
        cmdNew.Visible = False
        lblError.Text = "Sie haben folgenden Auftrag vorbereitet."
    End Sub


    Protected Sub cmdCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel.Click
        DoOn()
        lblError.Text = ""
    End Sub

    Protected Sub cmdConfirm_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdConfirm.Click
        Try
            Dim strHelp As String
            Dim strTemp As String = "Datum: " & lblAuftragsdatum.Text & vbCrLf

            strTemp &= lbl_Vertragsnummer.Text & txt_Vertragsnummer.Text & vbCrLf

            If txtKennz.Text.Length > 0 Then
                strTemp &= lbl_Kennzeichen.Text & " " & txtKennz.Text & vbCrLf
            End If

            strTemp &= "Sachbearbeiter/Tel.: " & txtSachbearbeiter.Text & vbCrLf
            strTemp &= vbCrLf
            strTemp &= lbl_Auftrag.Text & vbCrLf
            strHelp = "_"
            If rb_ErsatzKfzSchein.Checked Then
                strHelp = "X"
            End If
            strTemp &= strHelp & " " & rb_ErsatzKfzSchein.Text & vbCrLf
            strHelp = "_"
            If rb_NeuesSchild.Checked Then
                strHelp = "X"
            End If
            strTemp &= strHelp & " " & rb_NeuesSchild.Text & vbCrLf
            strHelp = "_"
            If rb_Ummeldung.Checked Then
                strHelp = "X"
            End If
            strTemp &= strHelp & " " & rb_Ummeldung.Text & vbCrLf
            strHelp = "_"
            If rb_Umkennzeichnung.Checked Then
                strHelp = "X"
            End If
            strTemp &= strHelp & " " & rb_Umkennzeichnung.Text & vbCrLf
            strHelp = "_"
            If rb_Versicherungswechsel.Checked Then
                strHelp = "X"
            End If
            strTemp &= strHelp & " " & rb_Versicherungswechsel.Text & vbCrLf
            strHelp = "_"
            If rb_Sonstiges.Checked Then
                strHelp = "X"
            End If
            strTemp &= strHelp & " " & rb_Sonstiges.Text & vbCrLf
            strHelp = "_"
            If rb_Briefaufbietung.Checked Then
                strHelp = "X"
            End If
            strTemp &= strHelp & " " & rb_Briefaufbietung.Text & vbCrLf
            strHelp = "_"
            If rb_Tempversand.Checked Then
                strHelp = "X"
            End If
            strTemp &= strHelp & " " & rb_Tempversand.Text & vbCrLf
            strTemp &= vbCrLf
            strTemp &= lbl_ScheinSchilderAn.Text & vbCrLf
            strTemp &= lbl_Firma.Text & " " & txtZielFirma.Text & vbCrLf
            strTemp &= lbl_Firma2.Text & " " & txtZielFirma2.Text & vbCrLf
            strTemp &= lbl_StrasseHsNr.Text & " " & txtZielStrasse.Text & " " & txtZielHNr.Text & vbCrLf
            strTemp &= lbl_PlzOrt.Text & " " & txtZielPLZ.Text & " " & txtZielOrt.Text & vbCrLf
            strTemp &= lbl_Telefon.Text & " " & txtZielTelefon.Text & vbCrLf
            strTemp &= vbCrLf
            strTemp &= lbl_HalterAlt.Text & vbCrLf
            strTemp &= lbl_FirmaAlt.Text & " " & txtHalterAltFirma.Text & vbCrLf
            strTemp &= lbl_Firma2Alt.Text & " " & txtHalterAltFirma2.Text & vbCrLf
            strTemp &= lbl_StrasseHsNrAlt.Text & " " & txtHalterAltStrasse.Text & " " & txtHalterAltHNr.Text & vbCrLf
            strTemp &= lbl_PlzOrtAlt.Text & " " & txtHalterAltPLZ.Text & " " & txtHalterAltOrt.Text & vbCrLf
            strTemp &= lbl_TelefonAlt.Text & " " & txtHalterAltTelefon.Text & vbCrLf
            strTemp &= vbCrLf
            strTemp &= lbl_HalterNeu.Text & vbCrLf
            strTemp &= lbl_FirmaNeu.Text & " " & txtHalterNeuFirma.Text & vbCrLf
            strTemp &= lbl_Firma2Neu.Text & " " & txtHalterNeuFirma2.Text & vbCrLf
            strTemp &= lbl_StrasseHsNrNeu.Text & " " & txtHalterNeuStrasse.Text & " " & txtHalterNeuHNr.Text & vbCrLf
            strTemp &= lbl_PlzOrtNeu.Text & " " & txtHalterNeuPLZ.Text & " " & txtHalterNeuOrt.Text & vbCrLf
            strTemp &= lbl_TelefonNeu.Text & " " & txtHalterNeuTelefon.Text & vbCrLf
            strTemp &= vbCrLf
            strTemp &= lbl_Fahrzeugdaten.Text & vbCrLf
            strTemp &= lbl_AmtlKennz.Text & " " & txtAmtlichesKennzeichen.Text & vbCrLf
            strTemp &= vbCrLf
            strTemp &= lbl_WunschKennz.Text & vbCrLf
            strTemp &= "1: " & " " & txtWKZ1.Text & vbCrLf
            strTemp &= "2: " & " " & txtWKZ2.Text & vbCrLf
            strTemp &= "3: " & " " & txtWKZ3.Text & vbCrLf
            strTemp &= vbCrLf
            strTemp &= lbl_Reservierung.Text & vbCrLf
            strTemp &= txtReservierungsnummer.Text & vbCrLf
            strTemp &= vbCrLf
            strTemp &= lbl_WunschZulassungstermin.Text & vbCrLf
            strTemp &= txtZulassungstermin.Text & vbCrLf
            strTemp &= vbCrLf
            strTemp &= lbl_VersicherungAlt.Text & vbCrLf
            strTemp &= txtVersicherungAlt.Text & vbCrLf
            strTemp &= vbCrLf
            strTemp &= lbl_VersicherungNeu.Text & vbCrLf
            strTemp &= txtVersicherungNeu.Text & vbCrLf
            strTemp &= vbCrLf
            strTemp &= lbl_Bemerkung.Text & vbCrLf
            strTemp &= txtBemerkung.Text & vbCrLf



            If Trim(m_User.Customer.CustomerContact.Kundenpostfach).Length > 0 Then
                'Absenden
                Dim Mail As System.Net.Mail.MailMessage
                Dim smtpMailSender As String = ""
                Dim smtpMailServer As String = ""

                smtpMailSender = ConfigurationManager.AppSettings("SmtpMailSender")
                Mail = New System.Net.Mail.MailMessage(smtpMailSender, m_User.Customer.CustomerContact.Kundenpostfach, "Sonstige Dienstleistung für Kunden " & m_User.Customer.KUNNR, strTemp)
                smtpMailServer = ConfigurationManager.AppSettings("SmtpMailServer")
                Dim client As New System.Net.Mail.SmtpClient(smtpMailServer)
                client.Send(Mail)

                'Message bringen
                lblError.Text = "Auftrag erfolgreich versendet."

                cmdContinue.Visible = False
                cmdCancel.Visible = False
                cmdConfirm.Visible = False
                cmdNew.Visible = True
            Else
                Err.Raise(-1, , "Kein Kundenpostfach eingerichtet.")
            End If



        Catch ex As Exception
            lblError.Text = "Fehler bei der Datenübermittlung.<br> (" & ex.Message & ")"
            cmdContinue.Visible = False
            cmdCancel.Visible = True
            cmdConfirm.Visible = False
            cmdNew.Visible = False
        End Try
    End Sub

    Protected Sub cmdNew_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdNew.Click
        DoOn()
        lblError.Text = ""
    End Sub

    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        Response.Redirect("/Services/(S(" & Session.SessionID & "))/Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

#End Region

#Region "Methods"

    Private Sub SwitchControls(ByVal blnStatus As Boolean)
        Dim control2 As Control
        Dim checkbox As CheckBox
        Dim textbox As TextBox

        For Each control2 In divControls.Controls

            If TypeOf control2 Is CheckBox Then
                checkbox = CType(control2, CheckBox)
                checkbox.Enabled = blnStatus
                If cmdNew.Visible Then
                    checkbox.Checked = False
                End If
            End If
            If TypeOf control2 Is TextBox Then
                textbox = CType(control2, TextBox)
                textbox.Enabled = blnStatus
                If cmdNew.Visible Then
                    textbox.Text = ""
                End If
            End If

        Next
    End Sub

    Private Sub DoOn()
        SwitchControls(True)
        cmdContinue.Visible = True
        cmdCancel.Visible = False
        cmdConfirm.Visible = False
        cmdNew.Visible = False
    End Sub

#End Region
   

   
End Class

' ************************************************
' $History: Change04s.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 11.11.10   Time: 9:18
' Updated in $/CKAG2/Services/Components/ComCommon
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 20.10.10   Time: 9:01
' Updated in $/CKAG2/Services/Components/ComCommon
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 19.10.10   Time: 13:00
' Created in $/CKAG2/Services/Components/ComCommon
' 