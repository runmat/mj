Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports System.IO
Imports System.Collections
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization

Public Class Change06
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private objHaendler As FFE_Bank_2
    Private bDefault As Boolean
    Protected WithEvents ucStyles As Styles
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.LinkButton
    Protected WithEvents btnSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents Linkbutton1 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents btnBack As System.Web.UI.WebControls.LinkButton
    Protected WithEvents txtVertragsNr As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtHaendlernr As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtFahrgNr As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtOrderNr As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtVersandDatum As System.Web.UI.WebControls.TextBox
    Protected WithEvents cbxBezahlt As System.Web.UI.WebControls.CheckBox
    Protected WithEvents rbKKBER As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents lblFaelligkeit As System.Web.UI.WebControls.Label
    Protected WithEvents rbKKBEROLD As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents lblFaelligkeitOLD As System.Web.UI.WebControls.Label
    Protected WithEvents txtFaelligDatumOLD As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Vertragsnr As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_FahrgestellNr As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_OrderNr As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_VersandDatum As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Bezahlt As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Status As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Statusold As System.Web.UI.WebControls.Label
    Protected WithEvents tr_Versand As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr_Vertragsnummer As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr_FahrgestellNr As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr_Bezahlt As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr_Status As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr_Statusold As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr_BriefNr As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lbl_BriefNr As System.Web.UI.WebControls.Label
    Protected WithEvents txtBriefNr As System.Web.UI.WebControls.TextBox
    Protected WithEvents tr_OrderNr As System.Web.UI.HtmlControls.HtmlTableRow
    Private logApp As Base.Kernel.Logging.Trace
    Protected WithEvents rb_Temp As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rb_Endg As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rb_DP As System.Web.UI.WebControls.RadioButton
    Protected WithEvents lbl_Faelligkeit As System.Web.UI.WebControls.Label
    Protected WithEvents txt_Faelligkeit As System.Web.UI.WebControls.TextBox


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
#Region " Methods "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        cmdSearch.Visible = True
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        m_App = New Base.Kernel.Security.App(m_User)
        logApp = New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)



        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text

            If (Session("objHaendler") Is Nothing) OrElse (Not IsPostBack) Then
                If Not (Request.QueryString("Aut") Is Nothing) AndAlso (Request.QueryString("Aut").Length > 0) Then
                    btnSave.Visible = True
                    btnSave.Text = "&#149;&nbsp;Autorisieren"
                    cmdSearch.Visible = False
                    Linkbutton1.Visible = True
                    btnBack.Visible = True
                    showData()
                Else
                    btnSave.Text = "&#149;&nbsp;Speichern"
                    objHaendler = New FFE_Bank_2(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                    If (Not Request.QueryString("VIN") Is Nothing AndAlso Not Request.QueryString("VIN") Is String.Empty) Then
                        txtFahrgNr.Text = Request.QueryString("VIN")
                        DoSubmit()
                    Else
                        rb_Temp.Enabled = False
                        rb_Endg.Enabled = False
                        rb_DP.Enabled = False
                    End If
                End If
            Else
                objHaendler = CType(Session("objHaendler"), FFE_Bank_2)

            End If

            Session("objHaendler") = objHaendler
            If Request.Params("art") = "DEFAULT" Then
                bDefault = True
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub showData()
        Dim OutPutStream As System.IO.MemoryStream
        Dim formatter As New BinaryFormatter()
        Dim kkber As String

        formatter = New BinaryFormatter()
        OutPutStream = GiveAuthorizationDetails(m_App.Connectionstring, CType(Request.QueryString("Aut"), Integer), "objFDDBank2")

        objHaendler = New FFE_Bank_2(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
        objHaendler = DirectCast(formatter.Deserialize(OutPutStream), FFE_Bank_2)

        Session("objHaendler") = objHaendler

        txtVertragsNr.Text = objHaendler.Auftraege.Rows(0)("LIZNR")
        txtFahrgNr.Text = objHaendler.Auftraege.Rows(0)("CHASSIS_NUM")
        txtBriefNr.Text = objHaendler.Auftraege.Rows(0)("TIDNR")
        txtOrderNr.Text = objHaendler.Auftraege.Rows(0)("ZZREFERENZ1")
        txtVersandDatum.Text = objHaendler.Auftraege.Rows(0)("ZZTMPDT")
        txtBriefNr.Text = objHaendler.Auftraege.Rows(0)("TIDNR")
        If CType(objHaendler.Auftraege.Rows(0)("ZZBEZAHLT"), String) = "X" Then
            cbxBezahlt.Checked = True
        End If
        'txtFaelligDatum.Text = objHaendler.Auftraege.Rows(0)("ZZFAEDT")
        'If Not IsDate(txtFaelligDatum.Text) Then
        '    txtFaelligDatum.Text = String.Empty
        'End If

        txt_Faelligkeit.Text = objHaendler.Auftraege.Rows(0)("ZZFAEDT")
        If Not IsDate(txt_Faelligkeit.Text) Then
            txt_Faelligkeit.Text = String.Empty
        End If

        kkber = objHaendler.Auftraege.Rows(0)("ZZKKBER")    'Kreditkontrollbereich

        If kkber = "0001" Then      'temp
            rb_Temp.Checked = True
        End If
        If kkber = "0002" Then      'endg
            rb_Endg.Checked = True
        End If
        If kkber = "0004" Then      'dp
            rb_DP.Checked = True
        End If

        txtVertragsNr.Enabled = False

        'logApp.CollectDetails("Kundennummer", CType(objHaendler.KUNNR, Object), True)
        'logApp.CollectDetails("Vertragsnummer", CType(txtVertragsNr.Text, Object))
        'logApp.CollectDetails("Fahrgestellnummer", CType(txtFahrgNr.Text, Object))
        'logApp.CollectDetails("Fälligkeit", CType(txtFaelligDatum.Text, Object))
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        If cmdSearch.Text = "&#149;&nbsp;Zurücksetzen" Then
            Session("objHaendler") = Nothing
            txt_Faelligkeit.Text = ""
            txtBriefNr.Text = ""
            txtFaelligDatumOLD.Text = ""
            txtFahrgNr.Text = ""
            txtHaendlernr.Text = ""
            txtOrderNr.Text = ""
            txtVersandDatum.Text = ""
            txtVersandDatum.Enabled = False
            lbl_Status.Enabled = False
            txtVertragsNr.Text = ""
            rb_DP.Checked = False
            rb_Temp.Checked = False
            rb_Endg.Checked = False
            rb_DP.Enabled = False
            rb_Temp.Enabled = False
            rb_Endg.Enabled = False
            cbxBezahlt.Enabled = False
            rbKKBEROLD.SelectedIndex = -1
            txt_Faelligkeit.Enabled = False
            lbl_Faelligkeit.Enabled = False
            cmdSearch.Text = "&#149;&nbsp;Suchen"
        ElseIf cmdSearch.Text = "&#149;&nbsp;Suchen" Then
            DoSubmit()
        End If

    End Sub

    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        lblError.Text = ""

        objHaendler.KUNNR = m_User.KUNNR
        If txtVertragsNr.Text.Length + txtBriefNr.Text.Length + txtFahrgNr.Text.Length = 0 Then
            lblError.Text = "Bitte geben Sie eine mindestens ein Suchkriterium ein!"
        Else
            objHaendler.Briefnr = txtBriefNr.Text.Trim
            objHaendler.Fahrgestellnr = txtFahrgNr.Text.Trim
            objHaendler.UserRef = m_User.Reference
            objHaendler.GiveCarSingle(txtVertragsNr.Text)

            If Not objHaendler.Status = 0 Then
                lblError.Text = objHaendler.Message
                lblError.Visible = True
            Else
                If objHaendler.Auftraege.Rows.Count = 0 Then
                    lblError.Text = "Zu den gewählten Kriterien wurden keine Ergebnisse gefunden."
                    rb_Temp.Enabled = False
                    rb_Endg.Enabled = False
                    rb_DP.Enabled = False
                Else
                    Session("objHaendler") = objHaendler

                    Dim kkber As String
                    Dim org As String

                    txtFahrgNr.Text = objHaendler.Auftraege.Rows(0)("CHASSIS_NUM")
                    txtVertragsNr.Text = objHaendler.Auftraege.Rows(0)("LIZNR")
                    txtBriefNr.Text = objHaendler.Auftraege.Rows(0)("TIDNR")
                    If objHaendler.Auftraege.Rows(0)("ZZREFERENZ1").ToString <> "0000" Then
                        txtOrderNr.Text = objHaendler.Auftraege.Rows(0)("ZZREFERENZ1")
                    End If
                    txtVersandDatum.Text = objHaendler.Auftraege.Rows(0)("ZZTMPDT")
                    'txtBriefNr.Text = objHaendler.Auftraege.Rows(0)("TIDNR")
                    If CType(objHaendler.Auftraege.Rows(0)("ZZBEZAHLT"), String) = "X" Then
                        cbxBezahlt.Checked = True
                    End If

                    org = Right(CType(objHaendler.Auftraege.Rows(0)("PARNR"), String), 5) 'Organisation holen

                    Dim suche As New Base.Business.Search(m_User.App, m_User, CType(Session.SessionID, String), CType(Session("AppID"), String))
                    suche.LeseHaendlerSAP(CType(Session("AppID"), String), CType(Session.SessionID, String), , m_User.Organization.OrganizationReference)
                    suche.Haendler.RowFilter = "REFERENZ = '" & org & "'"
                    'Nur Briefe der eigenen Organisation dürfen geändert werden!
                    If suche.Haendler.Count = 0 Then
                        lblError.Text = "Abweichende Organisation. Änderung nicht möglich!"
                        rb_Temp.Enabled = False
                        rb_Endg.Enabled = False
                        rb_DP.Enabled = False
                        Exit Sub
                    End If

                    If Not IsDate(txtVersandDatum.Text) Then
                        'Kein Versanddatum!
                        lblError.Text = "Brief noch nicht versendet. Änderung nicht möglich!"
                        rb_Temp.Enabled = False
                        rb_Endg.Enabled = False
                        rb_DP.Enabled = False
                        Exit Sub
                    End If

                    kkber = objHaendler.Auftraege.Rows(0)("ZZKKBER")    'Kreditkontrollbereich

                    If (kkber <> "0001" And kkber <> "0002" And kkber <> "0004") Then
                        'Nur standard temp., endg. oder DP!
                        lblError.Text = "Falsche Kontingentart. Änderung nicht möglich!"
                        rb_Temp.Enabled = False
                        rb_Endg.Enabled = False
                        rb_DP.Enabled = False
                        Exit Sub
                    End If
                    If cbxBezahlt.Checked Then
                        'Nur nicht bezahlte!
                        lblError.Text = "Fahrzeug ist bereits bezahlt. Änderung nicht möglich!"
                        rb_Temp.Enabled = False
                        rb_Endg.Enabled = False
                        rb_DP.Enabled = False
                        Exit Sub
                    End If

                    btnSave.Visible = True
                    cmdSearch.Text = "&#149;&nbsp;Zurücksetzen"
                    If kkber = "0001" Then      'temp
                        rb_Temp.Checked = True
                        rbKKBEROLD.SelectedIndex = 0
                    End If
                    If kkber = "0002" Then      'endg
                        rb_Endg.Checked = True
                        rbKKBEROLD.SelectedIndex = 1
                    End If

                    Dim strDatum As String
                    If kkber = "0004" Then      'dp
                        rb_DP.Checked = True
                        rbKKBEROLD.SelectedIndex = 2
                        'txtFaelligDatum.Enabled = True
                        'lblFaelligkeit.Enabled = True
                        txt_Faelligkeit.Enabled = True
                        lbl_Faelligkeit.Enabled = True
                        If IsSAPDate(objHaendler.Auftraege.Rows(0)("ZZFAEDT")) = True Then
                            strDatum = objHaendler.Auftraege.Rows(0)("ZZFAEDT")
                            'txtFaelligDatum.Text = strDatum
                            txt_Faelligkeit.Text = strDatum
                            txtFaelligDatumOLD.Text = strDatum
                        End If
                    End If
                    rb_Temp.Enabled = True
                    rb_Endg.Enabled = True
                    rb_DP.Enabled = True
                    'rbKKBER.Enabled = True
                    lbl_Status.Enabled = True
                End If
            End If
        End If
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        'Änderungen speichern
        Dim kkber As String = ""
        Dim fdat As String = ""
        Dim hnr As String = ""
        Dim rwert As String = ""
        Dim kkberIndex As Integer

        objHaendler = CType(Session("objHaendler"), FFE_Bank_2)

        If rb_Temp.Checked Then
            kkberIndex = 0
        End If
        If rb_Endg.Checked Then
            kkberIndex = 1
        End If
        If rb_DP.Checked Then
            kkberIndex = 2
        End If

        'Wurde überhaupt etwas geändert?
        If (kkberIndex = rbKKBEROLD.SelectedIndex) And (txt_Faelligkeit.Text = txtFaelligDatumOLD.Text) Then
            'Nein, alles gleich!!!
            lblError.Text = "Es wurden keine Änderungen vorgenommen. Vorgang nicht gespeichert!"
            Exit Sub
        End If

        If kkberIndex = 0 Then
            kkber = "0001"
        End If
        If kkberIndex = 1 Then
            kkber = "0002"
        End If
        If kkberIndex = 2 Then
            kkber = "0004"
        End If
        If kkber = "0004" AndAlso Not IsDate(txt_Faelligkeit.Text) Then
            lblError.Text = "Ungültiges Fälligkeitsdatum."
            Exit Sub
        End If

        rwert = CType(objHaendler.Auftraege.Rows(0)("ZZRWERT"), Integer)
        hnr = CType(objHaendler.Auftraege.Rows(0)("PARNR"), Integer)

        If kkber = "0004" AndAlso Not (rwert > 0) Then
            lblError.Text = "Händler ist nicht für Kontingentart 'Delayed Payment' autorisiert. Änderung nicht möglich!"
            Exit Sub
        End If

        fdat = String.Empty
        If kkber = "0004" Then              'Fälligkeitsdatum nur setzen, wenn DP!!!
            fdat = txt_Faelligkeit.Text
            If Not IsDate(fdat) Then
                txt_Faelligkeit.Text = String.Empty
            End If
        End If
        If Not (Request.QueryString("Aut") Is Nothing) AndAlso (Request.QueryString("Aut").Length > 0) Then
            objHaendler.SetStatus(txtVertragsNr.Text, Right(CType(objHaendler.Auftraege.Rows(0)("PARNR"), String), 5), kkber, txt_Faelligkeit.Text)
            If objHaendler.Status <> 0 Then 'Fehler
                lblError.Text = objHaendler.Message
            Else
                DeleteAuthorizationEntry(m_App.Connectionstring, CType(Request.QueryString("Aut"), Integer))

                HelpProcedures.fakeLogEntryForAutorisierung(logApp, m_User, Session.SessionID, Session("AppID"), "Autorisierung Änderung Status bei " & Right(objHaendler.Customer, 5) & " erfolgreich durchgeführt")
                lblMessage.Text = "Vorgang erfolgreich gespeichert."
                btnSave.Visible = False
                cmdSearch.Visible = False
            End If
        Else
            If kkber = "0004" AndAlso Not IsDate(txt_Faelligkeit.Text) Then
                lblError.Text = "Ungültiges Fälligkeitsdatum."
                Exit Sub
            End If

            rwert = CType(objHaendler.Auftraege.Rows(0)("ZZRWERT"), Integer)
            hnr = CType(objHaendler.Auftraege.Rows(0)("PARNR"), Integer)

            If kkber = "0004" AndAlso Not (rwert > 0) Then
                lblError.Text = "Händler ist nicht für Kontingentart 'Delayed Payment' autorisiert. Änderung nicht möglich!"
                Exit Sub
            End If

            fdat = String.Empty
            If kkber = "0004" Then              'Fälligkeitsdatum nur setzen, wenn DP!!!
                fdat = txt_Faelligkeit.Text
                If Not IsDate(fdat) Then
                    txt_Faelligkeit.Text = String.Empty
                End If
            End If

            'Pruefen, ob schon in der Autorisierung.
            Dim strInitiator As String = ""
            Dim intAuthorizationID As Int32

            m_App.CheckForPendingAuthorization(CInt(Session("AppID")), CInt(m_User.Organization.OrganizationReference), Right(hnr, 5), txtFahrgNr.Text, m_User.IsTestUser, strInitiator, intAuthorizationID)
            If Not strInitiator.Length = 0 Then
                'Fahrzeug wurde schon mal freigegeben und liegt zur Autorisierung vor
                lblError.Text = "Dieser Vorgang wurde schon einmal geändert und liegt bereits zur Autorisierung vor!"
                Exit Sub
            End If

            'Serialisierung...

            'Kontingent + Faelligkeit setzen
            objHaendler.Auftraege.Rows(0)("ZZFAEDT") = fdat
            objHaendler.Auftraege.Rows(0)("ZZKKBER") = kkber

            Dim formatter As System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
            Dim ms As MemoryStream
            Dim b() As Byte
            Dim DetailArray(1, 1) As Object

            ms = New MemoryStream()
            formatter = New BinaryFormatter()
            formatter.Serialize(ms, objHaendler)
            b = ms.ToArray
            ms = New MemoryStream(b)
            DetailArray(0, 0) = ms
            DetailArray(0, 1) = "objFDDBank2"

            intAuthorizationID = WriteAuthorization(m_App.Connectionstring, CInt(Session("AppID")), m_User.UserName, CInt(m_User.Organization.OrganizationReference), Right(hnr, 5), txtFahrgNr.Text, "", "", m_User.IsTestUser, DetailArray)

            If (intAuthorizationID > 0) Then
                lbl_Faelligkeit.Enabled = False
                txt_Faelligkeit.Enabled = False
                lbl_Status.Enabled = False
                btnSave.Visible = False
                lblMessage.Text = "Vorgang liegt jetzt der Bank zur Autorisierung vor."
                WriteLog("Änderung Status für " & Right(hnr, 5) & " bzw. Fzg.: " & txtFahrgNr.Text & " liegt zur Autorisierung vor.")
            Else
                lblError.Text = "Fehler: Vorgang konnte nicht zur Autorisierung gespeichert werden!"
            End If
        End If
    End Sub

    Private Sub WriteLog(ByVal strMessage As String, Optional ByVal strType As String = "APP")
        logApp.WriteEntry(strType, m_User.UserName, Session.SessionID, CInt(Session("AppID")), "Änderung Status", "Change", strMessage, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 10)
    End Sub

    Private Sub Linkbutton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Linkbutton1.Click
        Try
            DeleteAuthorizationEntry(m_App.Connectionstring, CInt(Request.QueryString("Aut")))
            'WriteLog("Änderung Fälligkeit + Status für Händler " & objHaendler.Auftraege.Rows(0)("PARNR") & " aus Autorisierung gelöscht.")
            HelpProcedures.fakeLogEntryForAutorisierung(logApp, m_User, Session.SessionID, Session("AppID"), "Änderung Fälligkeit + Status für Händler " & objHaendler.Auftraege.Rows(0)("PARNR") & " aus Autorisierung gelöscht.")
            lblMessage.Text = "<b>Daten wurden aus Autorisierung entfernt.</b><br>&nbsp;"
            Session("Authorization") = Nothing
            Session("AuthorizationID") = Nothing
            btnSave.Visible = False
            Linkbutton1.Visible = False
            btnBack.Visible = True
            cmdSearch.Visible = False
        Catch ex As Exception
            'WriteLog("Fehler bei der Löschung einer Änderung Fälligkeit + Status für Händler " & objHaendler.Auftraege.Rows(0)("PARNR") & " , (Fehler: " & ex.Message & ")", "ERR")
            HelpProcedures.fakeLogEntryForAutorisierung(logApp, m_User, Session.SessionID, Session("AppID"), "Fehler bei der Löschung einer Änderung Fälligkeit + Status für Händler " & objHaendler.Auftraege.Rows(0)("PARNR") & " , (Fehler: " & ex.Message & ")", "ERR")
            lblError.Text = "Beim Speichern Ihrer Daten ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Response.Redirect("Change14.aspx?AppID=" & CStr(m_User.Applications.Select("AppName='Change14'")(0).Item("AppID")), False)
    End Sub

    Private Sub rbKKBER_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbKKBER.SelectedIndexChanged

        If rbKKBER.SelectedIndex = 2 Then      'temp
            'lblFaelligkeit.Enabled = True
            'txtFaelligDatum.Enabled = True
            lbl_Faelligkeit.Enabled = True
            txt_Faelligkeit.Enabled = True
        Else
            'lblFaelligkeit.Enabled = False
            'txtFaelligDatum.Enabled = False
            lbl_Faelligkeit.Enabled = False
            txt_Faelligkeit.Enabled = False
            'txtFaelligDatum.Text = txtFaelligDatumOLD.Text
            txt_Faelligkeit.Text = txtFaelligDatumOLD.Text
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
#End Region
End Class
' ************************************************
' $History: Change06.aspx.vb $
' 
' *****************  Version 13  *****************
' User: Rudolpho     Date: 24.07.08   Time: 12:39
' Updated in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 12  *****************
' User: Rudolpho     Date: 16.07.08   Time: 11:34
' Updated in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 27.06.08   Time: 11:18
' Updated in $/CKAG/Applications/AppFFE/forms
' ITA 2043/2032 BugFix Übersicht Benutzeraktivitäten bei Punkt
' Autorisierung 
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 10.06.08   Time: 17:24
' Updated in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 9.06.08    Time: 15:44
' Updated in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 6.06.08    Time: 9:21
' Updated in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 4.06.08    Time: 18:43
' Updated in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 23.05.08   Time: 16:21
' Updated in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 15.05.08   Time: 14:33
' Updated in $/CKAG/Applications/AppFFE/forms
' ITA:1865
' 
' ************************************************
