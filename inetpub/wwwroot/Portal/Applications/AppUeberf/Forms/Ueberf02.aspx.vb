Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Imports AppUeberf.Controls.ProgressControl

Public Class Ueberf02
    Inherits System.Web.UI.Page
    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private dv As DataView
    Private strError As String
    Private clsUeberf As Ueberf_01

    Protected WithEvents lblFahrzeugdaten As System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    Protected WithEvents Label6 As System.Web.UI.WebControls.Label
    Protected WithEvents Label7 As System.Web.UI.WebControls.Label
    Protected WithEvents Label8 As System.Web.UI.WebControls.Label
    Protected WithEvents Label9 As System.Web.UI.WebControls.Label
    Protected WithEvents txtKennzeichen1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label10 As System.Web.UI.WebControls.Label
    Protected WithEvents txtKennzeichen2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAbgabetermin As System.Web.UI.WebControls.TextBox
    Protected WithEvents chkWagenVolltanken As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkWw As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkEinweisung As System.Web.UI.WebControls.CheckBox
    Protected WithEvents Label11 As System.Web.UI.WebControls.Label
    Protected WithEvents rdbZugelassen As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents ucHeader As Header
    Protected WithEvents Label14 As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents rdbBereifung As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents Label12 As System.Web.UI.WebControls.Label
    Protected WithEvents calVon As System.Web.UI.WebControls.Calendar
    Protected WithEvents btnVon As System.Web.UI.WebControls.LinkButton
    Protected WithEvents Label16 As System.Web.UI.WebControls.Label
    Protected WithEvents drpFahrzeugwert As System.Web.UI.WebControls.DropDownList
    Protected WithEvents Label15 As System.Web.UI.WebControls.Label
    Protected WithEvents chkRotKenn As System.Web.UI.WebControls.CheckBox
    Protected WithEvents Label13 As System.Web.UI.WebControls.Label
    Protected WithEvents chkFix As System.Web.UI.WebControls.CheckBox
    Protected WithEvents pnlWinterreifen As System.Web.UI.WebControls.Panel
    Protected WithEvents chkWinterreifenHandling As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkWinterreifenGroesser As System.Web.UI.WebControls.CheckBox
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents txtWinterreifenGroesse As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents rdoWinterreifenBesorgen As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rdoWinterreifenAbWerk As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rdlFelgen As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents rdlRadkappen As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents rdlZweiterRadsatz As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents cmdBack As System.Web.UI.WebControls.ImageButton
    Protected WithEvents cmdRight1 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents lnkAnschlussfahrt As System.Web.UI.WebControls.LinkButton
    Protected WithEvents txtBemerkung As CKG.Portal.PageElements.TextAreaControl
    Protected WithEvents ucStyles As Styles
    Protected WithEvents pnlPlaceHolder As System.Web.UI.WebControls.Panel
    Protected WithEvents pnlPlaceholder2 As System.Web.UI.WebControls.Panel
    Protected WithEvents pnlPlaceholder3 As System.Web.UI.WebControls.Panel
    Protected WithEvents Label18 As System.Web.UI.WebControls.Label
    Protected WithEvents ProgressControl1 As Controls.ProgressControl


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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try


        If IsPostBack = False Then
            GetData()
        Else
            If clsUeberf Is Nothing Then
                clsUeberf = Session("Ueberf")
            End If
        End If

        ProgressControl1.Fill(Source.Ueber02, clsUeberf)

        DisableControls()
    End Sub

    Private Sub cmRight1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdRight1.Click
        Weiter_Click(sender, e)
    End Sub

    Private Sub Weiter_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles lnkAnschlussfahrt.Click
        Try

            Dim booErr As Boolean
            Dim strErr As String


            'Daten speichern
            SetData()

            With clsUeberf

                .Modus = 2

                strErr = "Bitte füllen Sie alle Pflichtfelder aus: <br>"


                If .Herst = "" Then
                    booErr = True
                    strErr = strErr & "Hersteller / Typ <br>"
                End If

                '#996 Nicht mehr Pflicht
                'If .Kenn1 = "" Or .Kenn2 = "" Then
                '    booErr = True
                '    strErr = strErr & "Kennzeichen <br>"
                'End If

                If .FzgZugelassen = "" Then
                    booErr = True
                    strErr = strErr & "Fahrzeug zugelassen und fahrbereit? <br>"
                End If

                If .SomWin = "" Then
                    booErr = True
                    strErr = strErr & "Bereifung  <br>"
                End If


                If drpFahrzeugwert.SelectedItem.Value = "0" Then
                    booErr = True
                    strErr = strErr & "Fahrzeugwert  <br>"
                End If

                .SelFahrzeugwert = drpFahrzeugwert.SelectedItem.Value

            End With

            If txtAbgabetermin.Text <> "" Then
                If IsDate(txtAbgabetermin.Text) = False Then
                    If booErr = True Then
                        strErr = strErr & "Bitte geben Sie einen korrekten Überführungstermin im Format TT.MM.JJJJ ein.  <br>"
                    Else
                        booErr = True
                        strErr = "Bitte geben Sie einen korrekten Überführungstermin im Format TT.MM.JJJJ ein.  <br>"
                    End If
                End If
            End If


            Session("Ueberf") = clsUeberf

            If booErr = False Then
                If sender Is lnkAnschlussfahrt Then
                    clsUeberf.Anschluss = True
                    Response.Redirect("Ueberf03.aspx?AppID=" & Session("AppID").ToString)
                ElseIf clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.ZulassungUndUeberfuehrungKCL Then
                    clsUeberf.Anschluss = False
                    Response.Redirect("ZulUebBest.aspx?AppID=" & Session("AppID").ToString)
                Else
                    clsUeberf.Anschluss = False
                    Response.Redirect("Ueberf04.aspx?AppID=" & Session("AppID").ToString)
                End If
            Else
                lblError.Text = strErr
            End If

        Catch ex As Exception

            lblError.Text = ex.Message

        End Try

    End Sub

    Private Sub SetData()

        'Bei reiner Überführung
        DisableControls()

        With clsUeberf

            .Kenn1 = txtKennzeichen1.Text
            .Kenn2 = txtKennzeichen2.Text

            .FixDatumUeberfuehrung = Me.chkFix.Checked

            '------
            'Überführungsdatum prüfen
            '------
            If Me.txtAbgabetermin.Text.Trim <> String.Empty Then

                Dim ueberfuehrDatum As Date
                Try
                    ueberfuehrDatum = Date.Parse(txtAbgabetermin.Text)

                Catch ex As Exception
                    Throw New Exception("Bitte geben Sie ein gültiges Überführungsdatum ein.")
                End Try

                'Überführungsdatum >= Zulassungsdatum
                If ueberfuehrDatum < .Zulassungsdatum Then
                    Throw New Exception("Das Überführungsdatum darf nicht vor dem Zulassungsdatum sein.")
                End If

                If chkFix.Checked AndAlso ueberfuehrDatum < Date.Today.AddDays(4) AndAlso Not CBool(ViewState("WarnungTerminvorgabe")) Then
                    'Wiederreinnehmen, wenn dies doch nur Hinweis sein soll
                    'ViewState("WarnungTerminvorgabe") = True 
                    'Throw New Exception("Bitte prüfen Sie Ihre Terminvorgabe.")
                    Throw New Exception("Bitte beachten Sie die Standardvorlaufzeit von 4 Werktagen!")
                End If

                .DatumUeberf = ueberfuehrDatum

            End If

            .Bemerkung = txtBemerkung.Text

            '----
            'Winterreifen-Details scpeichern
            '----
            If chkWinterreifenHandling.Checked Then
                .WinterHandling = True
                Dim sb As New System.Text.StringBuilder()

                If rdoWinterreifenAbWerk.Checked Then
                    sb.Append(rdoWinterreifenAbWerk.Text + "; ")
                    .WinterReifenquelle = rdoWinterreifenAbWerk.ID
                ElseIf rdoWinterreifenBesorgen.Checked Then
                    sb.Append(rdoWinterreifenBesorgen.Text + "; ")
                    .WinterReifenquelle = rdoWinterreifenBesorgen.ID
                Else
                    'Sollte nicht vorkommen
                    Throw New Exception("Unbekannte Reifenquelle.")
                End If

                If chkWinterreifenGroesser.Checked Then sb.Append(chkWinterreifenGroesser.Text + " " + txtWinterreifenGroesse.Text + "; ")
                .WinterGroesser = chkWinterreifenGroesser.Checked
                .WinterGroesse = txtWinterreifenGroesse.Text

                If rdlFelgen.SelectedIndex > -1 Then
                    sb.Append("Felgen:" + rdlFelgen.SelectedItem.Text + "; ")
                    .WinterFelgen = rdlFelgen.SelectedItem.Value
                Else
                    .WinterFelgen = ""
                End If

                If rdlRadkappen.SelectedIndex > -1 Then
                    sb.Append("Radkappen:" + rdlRadkappen.SelectedItem.Text + "; ")
                    .WinterRadkappen = rdlRadkappen.SelectedItem.Value
                Else
                    .WinterRadkappen = ""
                End If

                If rdlZweiterRadsatz.SelectedIndex > -1 Then
                    sb.Append("Zweiter Radsatz:" + rdlZweiterRadsatz.SelectedItem.Text + "; ")
                    .WinterZweiterRadsatz = rdlZweiterRadsatz.SelectedItem.Value
                Else
                    .WinterZweiterRadsatz = ""
                End If

                .WinterText = sb.ToString()
            Else
                .WinterHandling = False
                .WinterText = ""
            End If

        End With


    End Sub

    Private Sub GetData()

        If clsUeberf Is Nothing Then
            clsUeberf = Session("Ueberf")
        End If

        'Bei reiner Überführung
        DisableControls()

        drpFahrzeugwert.AutoPostBack = True


        With drpFahrzeugwert
            .Items.Add(New ListItem("Bitte auswählen", "0"))
            .Items.Add(New ListItem("...bis  50  Tsd. €", "Z00"))
            .Items.Add(New ListItem("...bis 150  Tsd. €", "Z50"))
        End With

        With clsUeberf

            txtKennzeichen1.Text = .Kenn1
            txtKennzeichen2.Text = .Kenn2

            '#996 - Nicht mehr übernehmen
            'txtKennzeichen2.Text = .Kenn2
            If .DatumUeberf <> Date.MinValue Then
                txtAbgabetermin.Text = .DatumUeberf
            Else
                txtAbgabetermin.Text = ""
            End If

            Me.chkFix.Checked = .FixDatumUeberfuehrung

            txtBemerkung.Text = .Bemerkung
            chkEinweisung.Checked = .FzgEinweisung
            chkWagenVolltanken.Checked = .Tanken
            chkWw.Checked = .Waesche
            chkRotKenn.Checked = .RotKenn
            If .FzgZugelassen <> "" Then
                rdbZugelassen.Items.FindByValue(.FzgZugelassen).Selected = True
            End If
            If .SomWin <> "" Then
                rdbBereifung.Items.FindByValue(.SomWin).Selected = True
            End If

            If Not .SelFahrzeugwert Is Nothing Then
                drpFahrzeugwert.Items.FindByValue(.SelFahrzeugwert).Selected = True
            End If

            If .WinterHandling Then
                chkWinterreifenHandling.Checked = True

                If .WinterReifenquelle = rdoWinterreifenAbWerk.ID Then
                    rdoWinterreifenAbWerk.Checked = True
                ElseIf .WinterReifenquelle = rdoWinterreifenBesorgen.ID Then
                    rdoWinterreifenBesorgen.Checked = True
                End If

                chkWinterreifenGroesser.Checked = .WinterGroesser
                txtWinterreifenGroesse.Text = .WinterGroesse

                If .WinterFelgen <> "" Then rdlFelgen.Items.FindByValue(.WinterFelgen).Selected = True
                If .WinterRadkappen <> "" Then
                    rdlRadkappen.SelectedIndex = rdlRadkappen.Items.IndexOf(rdlRadkappen.Items.FindByValue(.WinterRadkappen))
                End If
                If .WinterZweiterRadsatz <> "" Then rdlZweiterRadsatz.Items.FindByValue(.WinterZweiterRadsatz).Selected = True

            End If
            CheckWinterreifenControls()

        End With

    End Sub


    Private Sub DisableControls()
        'Wenn es sich um eine reine Überführung handelt, und dort ein Fahrzeug im
        'Bestand gefunden wurde, sollen bestimmte Felder deaktiviert werden.
        If clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.ReineUeberfuehrung And clsUeberf.FahrzeugVorhanden = True Then
            With clsUeberf
                'If .Herst <> "" Then
                '    txtHerstTyp.Enabled = False
                'End If

                If .Kenn1 <> "" Then
                    txtKennzeichen1.Enabled = False
                End If

                If .Kenn2 <> "" Then
                    txtKennzeichen2.Enabled = False
                End If

                'If .Ref <> "" Then
                '    txtRef.Enabled = False
                'End If

                'If .Vin <> "" Then
                '    txtVin.Enabled = False
                'End If
            End With
        ElseIf clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.ZulassungUndUeberfuehrung Or _
                clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.OffeneUeberfuehrung Then

            'If clsUeberf.Vin <> "" Then
            '    txtVin.Enabled = False
            'End If

            'If clsUeberf.Ref <> "" Then
            '    txtRef.Enabled = False
            'End If

        ElseIf clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.ZulassungUndUeberfuehrungKCL Then
            If clsUeberf.Kenn1 <> "" Then
                txtKennzeichen1.Enabled = False
            End If

        End If
    End Sub


    Private Sub rdbZugelassen_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdbZugelassen.SelectedIndexChanged
        clsUeberf.FzgZugelassen = rdbZugelassen.SelectedItem.Value
    End Sub

    Private Sub chkWagenVolltanken_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkWagenVolltanken.CheckedChanged
        clsUeberf.Tanken = chkWagenVolltanken.Checked
    End Sub

    Private Sub chkEinweisung_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkEinweisung.CheckedChanged
        clsUeberf.FzgEinweisung = chkEinweisung.Checked
    End Sub

    Private Sub chkWw_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkWw.CheckedChanged
        clsUeberf.Waesche = chkWw.Checked
    End Sub

    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdBack.Click

        If clsUeberf Is Nothing Then
            clsUeberf = Session("Ueberf")
        End If

        clsUeberf.Modus = 1

        Session("Ueberf") = clsUeberf
        Response.Redirect("Ueberf01.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub rdbBereifung_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdbBereifung.SelectedIndexChanged
        clsUeberf.SomWin = rdbBereifung.SelectedItem.Value
    End Sub

    Private Sub btnVon_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVon.Click
        calVon.Visible = True
    End Sub
    Private Sub calVon_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calVon.SelectionChanged
        txtAbgabetermin.Text = calVon.SelectedDate.ToShortDateString
        calVon.Visible = False
    End Sub

    Private Sub chkRotKenn_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkRotKenn.CheckedChanged
        clsUeberf.RotKenn = chkRotKenn.Checked
    End Sub

    Private Sub drpFahrzeugwert_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles drpFahrzeugwert.SelectedIndexChanged
        clsUeberf.SelFahrzeugwert = drpFahrzeugwert.SelectedItem.Value
        clsUeberf.FahrzeugwertTxt = drpFahrzeugwert.SelectedItem.Text

    End Sub
    'Testkommentar


    '-----------
    'Winterreifen-Handling aktivieren
    '-----------
    Private Sub chkWinterreifenHandling_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkWinterreifenHandling.CheckedChanged
        Try
            CheckWinterreifenControls()

        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    '--------
    'Winterräderquelle hat sich geändert
    '--------
    Private Sub rdoWinterreifen_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoWinterreifenBesorgen.CheckedChanged, rdoWinterreifenAbWerk.CheckedChanged
        Try

            If rdoWinterreifenBesorgen.Checked Then
                Me.chkWinterreifenGroesser.Enabled = True
                Me.txtWinterreifenGroesse.Enabled = True
                Me.rdlFelgen.Enabled = True
                Me.rdlRadkappen.Enabled = True
                Me.rdlZweiterRadsatz.Enabled = True
            Else
                Me.chkWinterreifenGroesser.Enabled = False
                Me.txtWinterreifenGroesse.Enabled = False
                Me.rdlFelgen.Enabled = False
                Me.rdlRadkappen.Enabled = False
                Me.rdlZweiterRadsatz.Enabled = False
            End If

        Catch ex As Exception
            lblError.Text = ex.Message
        End Try

    End Sub

    '--------
    'Setzt die Verfügbarkeit der Winterreifen-Controls
    '--------
    Private Sub CheckWinterreifenControls()

        If chkWinterreifenHandling.Checked Then
            Me.pnlWinterreifen.Visible = True
            Me.rdoWinterreifenAbWerk.Enabled = True
            Me.rdoWinterreifenBesorgen.Enabled = True
            Me.chkWinterreifenGroesser.Enabled = rdoWinterreifenBesorgen.Checked
            Me.txtWinterreifenGroesse.Enabled = rdoWinterreifenBesorgen.Checked
            Me.rdlFelgen.Enabled = rdoWinterreifenBesorgen.Checked
            Me.rdlRadkappen.Enabled = rdoWinterreifenBesorgen.Checked
            Me.rdlZweiterRadsatz.Enabled = rdoWinterreifenBesorgen.Checked
        Else
            Me.pnlWinterreifen.Visible = False
            Me.rdoWinterreifenAbWerk.Enabled = False
            Me.rdoWinterreifenBesorgen.Enabled = False
            Me.chkWinterreifenGroesser.Enabled = False
            Me.txtWinterreifenGroesse.Enabled = False
            Me.rdlFelgen.Enabled = False
            Me.rdlRadkappen.Enabled = False
            Me.rdlZweiterRadsatz.Enabled = False
        End If
    End Sub

    Private Sub lnkAnschlussfahrt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkAnschlussfahrt.Click

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Ueberf02.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 16.04.08   Time: 11:53
' Updated in $/CKAG/Applications/AppUeberf/Forms
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 16:06
' Created in $/CKAG/Applications/AppUeberf/Forms
' 
' *****************  Version 16  *****************
' User: Rudolpho     Date: 10.07.07   Time: 15:15
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 15  *****************
' User: Uha          Date: 3.07.07    Time: 19:42
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' FormAuth in diverse Seiten wieder eingefügt
' 
' *****************  Version 14  *****************
' User: Uha          Date: 2.07.07    Time: 10:05
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 13  *****************
' User: Fassbenders  Date: 22.06.07   Time: 16:22
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 12  *****************
' User: Uha          Date: 22.05.07   Time: 10:48
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 11  *****************
' User: Uha          Date: 21.05.07   Time: 11:47
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Änderungen im Vergleich zur Startapplikation zum Stand 11.05.2007
' 
' *****************  Version 10  *****************
' User: Uha          Date: 3.05.07    Time: 15:16
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Änderungen aus StartApplication vom 02.05.2007 Mittags übernommen
' 
' ************************************************