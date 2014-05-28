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

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private objSuche As Search
    Private objHaendler As FDD_Bank_2
    Private objHaendler4 As FDD_Bank_4

    Protected WithEvents Hyperlink1 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkLogout As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents txtVertragsNr As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtFahrgNr As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtOrderNr As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtVersandDatum As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtFaelligDatum As System.Web.UI.WebControls.TextBox
    Protected WithEvents rbKKBER As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents cbxBezahlt As System.Web.UI.WebControls.CheckBox
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents lblStatus As System.Web.UI.WebControls.Label
    Protected WithEvents btnSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents txtHaendlernr As System.Web.UI.WebControls.TextBox
    Protected WithEvents Linkbutton1 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ucStyles As Styles
    Protected WithEvents btnBack As System.Web.UI.WebControls.LinkButton
    Protected WithEvents rbKKBEROLD As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents txtFaelligDatumOLD As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    Protected WithEvents lblFaelligkeit As System.Web.UI.WebControls.Label
    Protected WithEvents lblFaelligkeitOLD As System.Web.UI.WebControls.Label
    Private logApp As Base.Kernel.Logging.Trace

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
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
                    objHaendler = New FDD_Bank_2(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                End If
            Else
                objHaendler = CType(Session("objHaendler"), FDD_Bank_2)
            End If
            If txtVertragsNr.Enabled = True Then
                SetFocus(txtVertragsNr)
            End If
            Session("objHaendler") = objHaendler
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

        objHaendler = New FDD_Bank_2(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
        objHaendler = DirectCast(formatter.Deserialize(OutPutStream), FDD_Bank_2)

        Session("objHaendler") = objHaendler

        txtVertragsNr.Text = objHaendler.Auftraege.Rows(0)("LIZNR")
        txtFahrgNr.Text = objHaendler.Auftraege.Rows(0)("CHASSIS_NUM")
        txtOrderNr.Text = objHaendler.Auftraege.Rows(0)("ZZREFERENZ1")
        txtVersandDatum.Text = objHaendler.Auftraege.Rows(0)("ZZTMPDT")
        If Not IsDate(txtVersandDatum.Text) Then
            txtVersandDatum.Text = String.Empty
        Else
            txtVersandDatum.Text = CDate(txtVersandDatum.Text).ToShortDateString()
        End If

        If CType(objHaendler.Auftraege.Rows(0)("ZZBEZAHLT"), String) = "X" Then
            cbxBezahlt.Checked = True
        End If
        txtFaelligDatum.Text = objHaendler.Auftraege.Rows(0)("ZZFAEDT").ToString
        If Not IsDate(txtFaelligDatum.Text) Then
            txtFaelligDatum.Text = String.Empty
        Else
            txtFaelligDatum.Text = CDate(txtFaelligDatum.Text).ToShortDateString()
        End If

        kkber = objHaendler.Auftraege.Rows(0)("ZZKKBER")    'Kreditkontrollbereich

        If kkber = "0001" Then      'temp
            rbKKBER.SelectedIndex = 0
        End If
        If kkber = "0002" Then      'endg
            rbKKBER.SelectedIndex = 1
        End If
        If kkber = "0004" Then      'dp
            rbKKBER.SelectedIndex = 2
        End If

        txtVertragsNr.Enabled = False

    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        DoSubmit()
    End Sub

    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        lblError.Text = ""
     
        'objHaendler.KUNNR = m_User.KUNNR
        If txtVertragsNr.Text.Length = 0 Then
            lblError.Text = "Bitte geben Sie eine Vertragsnummer ein!"
        Else
            objHaendler.GiveCarSingle(Session("AppID").ToString, Session.SessionID, Me, txtVertragsNr.Text)

        If Not objHaendler.Status = 0 Then
            lblError.Text = objHaendler.Message
            lblError.Visible = True
        Else
            If objHaendler.Auftraege.Rows.Count = 0 Then
                lblError.Text = "Zu den gewählten Kriterien wurden keine Ergebnisse gefunden."
            Else
                Session("objHaendler") = objHaendler

                Dim kkber As String
                Dim org As String

                    txtFahrgNr.Text = objHaendler.Auftraege.Rows(0)("CHASSIS_NUM").ToString
                    txtOrderNr.Text = objHaendler.Auftraege.Rows(0)("ZZREFERENZ1").ToString
                    txtVersandDatum.Text = objHaendler.Auftraege.Rows(0)("ZZTMPDT").ToString

                    If Not IsDate(txtVersandDatum.Text) Then
                        txtVersandDatum.Text = String.Empty
                    Else
                        txtVersandDatum.Text = CDate(txtVersandDatum.Text).ToShortDateString()
                    End If

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
                    Exit Sub
                End If

                If Not IsDate(txtVersandDatum.Text) Then
                    'Kein Versanddatum!
                    lblError.Text = "Brief noch nicht versendet. Änderung nicht möglich!"
                    Exit Sub
                End If

                kkber = objHaendler.Auftraege.Rows(0)("ZZKKBER")    'Kreditkontrollbereich

                If (kkber <> "0001" And kkber <> "0002" And kkber <> "0004") Then
                    'Nur standard temp., endg. oder DP!
                    lblError.Text = "Falsche Kontingentart. Änderung nicht möglich!"
                    Exit Sub
                End If
                If cbxBezahlt.Checked Then
                    'Nur nicht bezahlte!
                    lblError.Text = "Fahrzeug ist bereits bezahlt. Änderung nicht möglich!"
                    Exit Sub
                End If

                btnSave.Visible = True

                If kkber = "0001" Then      'temp
                    rbKKBER.SelectedIndex = 0
                    rbKKBEROLD.SelectedIndex = 0
                End If
                If kkber = "0002" Then      'endg
                    rbKKBER.SelectedIndex = 1
                    rbKKBEROLD.SelectedIndex = 1
                End If

                Dim strDatum As String
                If kkber = "0004" Then      'dp
                    rbKKBER.SelectedIndex = 2
                    rbKKBEROLD.SelectedIndex = 2
                    txtFaelligDatum.Enabled = True
                    lblFaelligkeit.Enabled = True

                        If IsDate(objHaendler.Auftraege.Rows(0)("ZZFAEDT")) = True Then
                            strDatum = objHaendler.Auftraege.Rows(0)("ZZFAEDT")
                            txtFaelligDatum.Text = CDate(strDatum).ToShortDateString
                            txtFaelligDatumOLD.Text = CDate(strDatum).ToShortDateString
                        End If
                End If

                rbKKBER.Enabled = True
                lblStatus.Enabled = True
                End If
            End If
        End If
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        'Änderungen speichern
        Dim kkber As String = ""
        Dim fdat As String
        Dim hnr As String
        Dim rwert As Integer

        objHaendler = CType(Session("objHaendler"), FDD_Bank_2)

        'Wurde überhaupt etwas geändert?
        If (rbKKBER.SelectedIndex = rbKKBEROLD.SelectedIndex) And (txtFaelligDatum.Text = txtFaelligDatumOLD.Text) Then
            'Nein, alles gleich!!!
            lblError.Text = "Es wurden keine Änderungen vorgenommen. Vorgang nicht gespeichert!"
            Exit Sub
        End If

        If rbKKBER.SelectedIndex = 0 Then
            kkber = "0001"
        End If
        If rbKKBER.SelectedIndex = 1 Then
            kkber = "0002"
        End If
        If rbKKBER.SelectedIndex = 2 Then
            kkber = "0004"
        End If

        If Not (Request.QueryString("Aut") Is Nothing) AndAlso (Request.QueryString("Aut").Length > 0) Then

            logApp = New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
            logApp.CollectDetails("Kundennummer", CType(objHaendler.KUNNR, Object), True)
            logApp.CollectDetails("Kreditkontrollbereich", kkber, True)
            logApp.CollectDetails("Fälligkeit", txtFaelligDatum.Text, True)


            objHaendler.SetStatus(Session("AppID").ToString, Session.SessionID, Me, txtVertragsNr.Text, Right(CType(objHaendler.Auftraege.Rows(0)("PARNR"), String), 5), kkber, txtFaelligDatum.Text)
 
            If objHaendler.Status <> 0 Then 'Fehler
                lblError.Text = objHaendler.Message
                WriteLog("Fehler bei Änderung Fälligkeit + Status(Autorisierung) für Vertragsnummer " & CType(objHaendler.Auftraege.Rows(0)("LIZNR"), String) & " , (Fehler: " & objHaendler.Message & ")", Right(CType(objHaendler.Auftraege.Rows(0)("PARNR"), String), 5), "ERR")
            Else
                DeleteAuthorizationEntry(m_App.Connectionstring, CType(Request.QueryString("Aut"), Integer))
                lblError.Text = "Vorgang erfolgreich gespeichert."
                btnSave.Visible = False
                cmdSearch.Visible = False
                WriteLog("Änderung Fälligkeit + Status(Autorisierung) für Vertragsnummer " & txtVertragsNr.Text & " erfolgreich durchgeführt.", Right(CType(objHaendler.Auftraege.Rows(0)("PARNR"), String), 5))

            End If
        Else
            If kkber = "0004" AndAlso Not IsDate(txtFaelligDatum.Text) Then
                lblError.Text = "Ungültiges Fälligkeitsdatum."
                Exit Sub
            End If

            If IsDBNull(objHaendler.Auftraege.Rows(0)("ZZRWERT")) Then
                rwert = 0
            Else
                rwert = CType(objHaendler.Auftraege.Rows(0)("ZZRWERT"), Integer)
            End If

            If IsDBNull(objHaendler.Auftraege.Rows(0)("PARNR")) Then
                hnr = ""
            Else
                hnr = CType(objHaendler.Auftraege.Rows(0)("PARNR"), Integer).ToString()
            End If

            If kkber = "0004" AndAlso Not (rwert > 0) Then
                lblError.Text = "Händler ist nicht für Kontingentart 'Delayed Payment' autorisiert. Änderung nicht möglich!"
                Exit Sub
            End If

            fdat = String.Empty
            If kkber = "0004" Then              'Fälligkeitsdatum nur setzen, wenn DP!!!
                fdat = txtFaelligDatum.Text
                If Not IsDate(fdat) Then
                    txtFaelligDatum.Text = String.Empty
                End If
            End If

            'Pruefen, ob schon in der Autorisierung.
            Dim strInitiator As String = ""
            Dim intAuthorizationID As Int32

            m_App.CheckForPendingAuthorization(CInt(Session("AppID")), m_User.Organization.OrganizationId, Right(hnr, 5), txtFahrgNr.Text, m_User.IsTestUser, strInitiator, intAuthorizationID)
            If Not strInitiator.Length = 0 Then
                'Fahrzeug wurde schon mal freigegeben und liegt zur Autorisierung vor
                lblError.Text = "Dieser Vorgang wurde schon einmal geändert und liegt bereits zur Autorisierung vor!"
                Exit Sub
            End If

            'Serialisierung...

            'Kontingent + Faelligkeit setzen
            If Not fdat = String.Empty AndAlso IsDate(fdat) Then
                objHaendler.Auftraege.Rows(0)("ZZFAEDT") = fdat
            End If

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

            intAuthorizationID = WriteAuthorization(m_App.Connectionstring, CInt(Session("AppID")), m_User.UserName, m_User.Organization.OrganizationId, Right(hnr, 5), txtFahrgNr.Text, "", "", m_User.IsTestUser, DetailArray)

            If (intAuthorizationID > 0) Then
                rbKKBER.Enabled = False
                lblFaelligkeit.Enabled = False
                txtFaelligDatum.Enabled = False
                lblStatus.Enabled = False
                btnSave.Visible = False
                lblError.Text = "Vorgang zur Autorisierung gespeichert."
            Else
                lblError.Text = "Fehler: Vorgang konnte nicht zur Autorisierung gespeichert werden!"
            End If
        End If
    End Sub

    Private Sub WriteLog(ByVal strMessage As String, ByVal strHaendler As String, Optional ByVal strType As String = "APP")
        logApp.WriteEntry(strType, m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, Right(strHaendler, 5), strMessage, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 10)
    End Sub

    Private Sub Linkbutton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Linkbutton1.Click
        Try
            DeleteAuthorizationEntry(m_App.Connectionstring, CInt(Request.QueryString("Aut")))
            WriteLog("Änderung Fälligkeit + Status für Händler " & objHaendler.Auftraege.Rows(0)("PARNR") & " aus Autorisierung gelöscht.", Right(objHaendler.Auftraege.Rows(0)("PARNR").ToString, 5))
            lblError.Text = "<b>Daten wurden aus Autorisierung entfernt.</b><br>&nbsp;"
            Session("Authorization") = Nothing
            Session("AuthorizationID") = Nothing
            btnSave.Visible = False
            Linkbutton1.Visible = False
            btnBack.Visible = True
            cmdSearch.Visible = False
        Catch ex As Exception
            WriteLog("Fehler bei der Löschung einer Änderung Fälligkeit + Status für Händler " & objHaendler.Auftraege.Rows(0)("PARNR") & " , (Fehler: " & ex.Message & ")", "ERR")
            lblError.Text = "Beim Speichern Ihrer Daten ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Response.Redirect("Change14.aspx?AppID=" & CStr(m_User.Applications.Select("AppName='Change14'")(0).Item("AppID")), False)
    End Sub

    Private Sub rbKKBER_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbKKBER.SelectedIndexChanged

        If rbKKBER.SelectedIndex = 2 Then      'temp
            lblFaelligkeit.Enabled = True
            txtFaelligDatum.Enabled = True

        Else
            lblFaelligkeit.Enabled = False
            txtFaelligDatum.Enabled = False
            txtFaelligDatum.Text = txtFaelligDatumOLD.Text
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Change06.aspx.vb $
' 
' *****************  Version 7  *****************
' User: Fassbenders  Date: 23.03.10   Time: 16:34
' Updated in $/CKAG/Applications/appffd/Forms
' Rückgängig: Dynproxy-Zugriff
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 18.03.10   Time: 15:56
' Updated in $/CKAG/Applications/appffd/Forms
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 11.03.10   Time: 12:36
' Updated in $/CKAG/Applications/appffd/Forms
' ITA: 2918
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 13.07.09   Time: 17:44
' Updated in $/CKAG/Applications/appffd/Forms
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 15.06.09   Time: 17:08
' Updated in $/CKAG/Applications/appffd/Forms
' ITA 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 30.04.09   Time: 14:43
' Updated in $/CKAG/Applications/appffd/Forms
' ITA: 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:23
' Created in $/CKAG/Applications/appffd/Forms
' 
' *****************  Version 9  *****************
' User: Uha          Date: 2.07.07    Time: 12:52
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 8.06.07    Time: 15:36
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' Abgleich Beyond Compare
' 
' *****************  Version 7  *****************
' User: Uha          Date: 7.03.07    Time: 12:51
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' 
' ************************************************
