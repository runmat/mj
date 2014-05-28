Option Explicit On
Option Strict On

Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Partial Public Class UeberfDADAus
    Inherits System.Web.UI.Page

    Protected WithEvents ucStyles As CKG.Portal.PageElements.Styles
    Protected WithEvents ucHeader As CKG.Portal.PageElements.Header


#Region "Declarations"
    '***Auftragsart****
    Private Enum Auftragsarten
        Zulassung = 1
        Auslieferung = 2
        ZulassungAuslieferung = 3
        AuslieferungRueckfuehrung = 4
        Rueckfuehrung = 5
        Alles = 6
    End Enum

    Private Enum Fahrzeugempfaenger
        Leasingnehmer = 0
        Halter = 1
        Dritte = 2
    End Enum


    Private Const ERROR_MESSAGE_PFLICHTFELD As String = "Bitte füllen Sie alle Pflichfelder(*) aus."

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private mUeberfDAD As UeberfDAD
    Private mCalClicked As Int32
    Private mIbtCalAuslieferung As Boolean

#End Region

#Region "Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If IsPostBack = False Then
                'Vorgaben laden
                FillControls()

            Else
                If mUeberfDAD Is Nothing Then
                    mUeberfDAD = DirectCast(Session("UeberfData"), UeberfDAD)
                End If
                AddToSessionObject()
            End If

            SetMenu()

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Protected Sub lbtWeiter_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtWeiter.Click
        If PflichtfelderValidieren() = False Then

            '***Winterreifen-Details speichern***
            With mUeberfDAD
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
                        'Throw New Exception("Unbekannte Reifenquelle.")
                        lblError.Text = "Winterreifenhandling: Unbekannte Reifenquelle."
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


            Select Case mUeberfDAD.Auftragsart
                Case Auftragsarten.Auslieferung.ToString, Auftragsarten.ZulassungAuslieferung.ToString
                    Response.Redirect("UeberfDADSave.aspx?AppID=" & Session("AppID").ToString)
                Case Auftragsarten.AuslieferungRueckfuehrung.ToString, Auftragsarten.Alles.ToString
                    Response.Redirect("UeberfDADEinRueck.aspx?AppID=" & Session("AppID").ToString)

            End Select

        End If
    End Sub

    Protected Sub rdoFahrzeugempfaenger_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rdoFahrzeugempfaenger.SelectedIndexChanged

        If IsNothing(Session("UeberfData")) = False Then
            mUeberfDAD = DirectCast(Session("UeberfData"), UeberfDAD)
        End If


        Select Case rdoFahrzeugempfaenger.SelectedItem.Text
            Case Fahrzeugempfaenger.Leasingnehmer.ToString
                'Leasingnehmer übernehmen
                txtEmpfName.Text = txtLnName.Text
                txtEmpfStrasse.Text = txtLnStrasse.Text
                txtEmpfPLZ.Text = txtLnPLZ.Text
                txtEmpfOrt.Text = txtLnOrt.Text

            Case Fahrzeugempfaenger.Halter.ToString
                'Halter übernehmen, wenn es eine Zulassung gibt
                With mUeberfDAD
                    If Len(.Halter) > 0 Then

                        txtEmpfName.Text = .Halter
                        txtEmpfStrasse.Text = .HalterStrasse
                        txtEmpfPLZ.Text = .HalterPLZ
                        txtEmpfOrt.Text = .HalterOrt
                        txtEmpfAPName.Text = .Halter2

                    End If
                End With

            Case Fahrzeugempfaenger.Dritte.ToString
                'Felder für manuelle Erfassung leeren
                txtEmpfName.Text = String.Empty
                txtEmpfStrasse.Text = String.Empty
                txtEmpfPLZ.Text = String.Empty
                txtEmpfOrt.Text = String.Empty
        End Select
    End Sub

    Protected Sub chkWinterreifenHandling_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkWinterreifenHandling.CheckedChanged
        Try
            CheckWinterreifenControls()

        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Protected Sub ibtCalAuslieferung_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtCalAuslieferung.Click
        calAuslieferung.Visible = True
    End Sub

    Protected Sub calAuslieferung_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles calAuslieferung.SelectionChanged
        txtAuslieferungDatum.Text = calAuslieferung.SelectedDate.ToShortDateString
        calAuslieferung.Visible = False
    End Sub

    Protected Sub ibtRefresh_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtRefresh.Click

        If Trim(txtLnNummer.Text) <> String.Empty Then

            Try
                mUeberfDAD.FillLeasingnehmer(Me.txtLnNummer.Text, False)

                FillControls()
            Catch ex As Exception
                lblError.Text = ex.Message
            End Try

        End If

    End Sub

    Protected Sub ibtSearchLN_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtSearchLN.Click
        Response.Redirect("UeberfDADAdresse.aspx?AppID=" & Session("AppID").ToString & "&Art=" & "LNEHMER" & "&Src=AUS")
    End Sub

    Protected Sub ibtSearchAuslieferung_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtSearchAuslieferung.Click
        Response.Redirect("UeberfDADAdresse.aspx?AppID=" & Session("AppID").ToString & "&Art=" & "AUSLIEFERUNG")
    End Sub

    Protected Sub ibtSearchHaendler_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtSearchHaendler.Click
        Response.Redirect("UeberfDADAdresse.aspx?AppID=" & Session("AppID").ToString & "&Art=" & "HAENDLER" & "&Src=AUS")
    End Sub

    Protected Sub lbtBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtBack.Click

        Select Case mUeberfDAD.Auftragsart
            Case Auftragsarten.Auslieferung.ToString
                Response.Redirect("UeberfDADStart.aspx?AppID=" & Session("AppID").ToString)

            Case Auftragsarten.ZulassungAuslieferung.ToString, Auftragsarten.Alles.ToString
                Response.Redirect("UeberfDADVss.aspx?AppID=" & Session("AppID").ToString)
        End Select
    End Sub

    '--------
    'Winterräderquelle hat sich geändert
    '--------
    Private Sub rdoWinterreifen_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoWinterreifenBesorgen.CheckedChanged, rdoWinterreifenAbWerk.CheckedChanged
        Try

            Me.rdlZweiterRadsatz.Enabled = True

            If rdoWinterreifenBesorgen.Checked Then
                Me.chkWinterreifenGroesser.Enabled = True
                Me.txtWinterreifenGroesse.Enabled = True
                Me.rdlFelgen.Enabled = True
                Me.rdlRadkappen.Enabled = True
            Else
                Me.chkWinterreifenGroesser.Enabled = False
                Me.txtWinterreifenGroesse.Enabled = False
                Me.rdlFelgen.Enabled = False
                Me.rdlRadkappen.Enabled = False
                'Me.rdlZweiterRadsatz.Enabled = False
            End If

        Catch ex As Exception
            lblError.Text = ex.Message
        End Try

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
#End Region

#Region "Methods"
    '----------------------------------------------------------------------
    ' Methode:      FillControls
    ' Autor:        SFa
    ' Beschreibung: Daten aus dem Session-Objekt holen und in die Controls
    '               schreiben
    ' Erstellt am:  29.08.2008
    ' ITA:          2150
    '----------------------------------------------------------------------
    Private Sub FillControls()

        If IsNothing(Session("UeberfData")) = False Then
            mUeberfDAD = DirectCast(Session("UeberfData"), UeberfDAD)

            With mUeberfDAD
                '***Leasingnehmer***
                txtReferenz.Text = .LeasingnehmerReferenz
                txtLnNummer.Text = .Leasingnehmernummer
                txtLnName.Text = .Leasingnehmer
                txtLnStrasse.Text = .LeasingnehmerStrasse
                txtLnPLZ.Text = .LeasingnehmerPLZ
                txtLnOrt.Text = .LeasingnehmerOrt

                Select Case .Auftragsart
                    Case Auftragsarten.ZulassungAuslieferung.ToString, Auftragsarten.Alles.ToString
                        txtReferenz.Enabled = False
                        txtLnNummer.Enabled = False
                        txtLnName.Enabled = False
                        txtLnStrasse.Enabled = False
                        txtLnPLZ.Enabled = False
                        txtLnOrt.Enabled = False
                        ibtRefresh.Enabled = False
                        ibtSearchLN.Enabled = False
                        drpLnLand.Enabled = False
                End Select


                '***Fahrzeugnutzer***
                txtFnName.Text = .FnName
                txtFnTelefon.Text = .FnTelefon
                txtFnMail.Text = .FnMail

                '***Aus Zulassung***
                txtFahrgestellnummer.Text = .LnFahrgestellnummer
                txtLnFahrzeugtyp.Text = .Fahrzeugtyp

                '***Auslieferung***
                txtKennzeichen.Text = .Kennzeichen

                'If Len(.FahrzeugStatus) > 0 Then
                '    drpFahrzeugstatus.SelectedItem.Value = .FahrzeugStatus
                'End If

                If Len(.SomWin) > 0 Then
                    rdbBereifung.Items.FindByValue(.SomWin).Selected = True
                End If


                'füllen der Controls anhand der objWerte
                '----------------------------------------------------

                If .FzgEinweisung.ToString = "X" Then chkEinweisung.Checked = True
                If .Servicekarte.ToString = "X" Then chkServicekarte.Checked = True
                If .Tanken.ToString = "X" Then chkWagenVolltanken.Checked = True
                If .Waesche.ToString = "X" Then chkWw.Checked = True
                txtTankkarten.Text = .Tankkarten


                If .HandyAdapter.Length > 0 Then
                    chkHandyadapter.Checked = True
                End If

                If .Verbandskasten.Length > 0 Then
                    chkVerbandskasten.Checked = True
                End If

                If .NaviCD.Length > 0 Then
                    chkNaviCD.Checked = True
                End If

                If .Warndreieck.Length > 0 Then
                    chkWarndreieck.Checked = True
                End If

                If .Warnweste.Length > 0 Then
                    chkWarnweste.Checked = True
                End If

                If .Fussmatten.Length > 0 Then
                    chkFussmatten.Checked = True
                End If

                If .FKontrolle.Length > 0 Then
                    chkFKontrolle.Checked = True
                End If
                '----------------------------------------------------


                txtAuslieferungDatum.Text = .AuslieferungDatum

                If Len(.TerminhinweisAuslieferungValue) > 0 Then
                    rdoAuslieferung.Items.FindByValue(.TerminhinweisAuslieferungValue).Selected = True
                End If

                If Len(.FahrzeugklasseValue) > 0 Then
                    rdbFahrzeugklasse.SelectedItem.Value = .FahrzeugklasseValue
                    rdbFahrzeugklasse.SelectedItem.Text = .FahrzeugklasseText
                End If






                txtBemerkung.Text = .BemerkungAus

                '***Händler***
                txtHaendlerName.Text = .HaendlerName1
                txtHaendlerStrasse.Text = .HaendlerStrasse
                txtHaendlerPLZ.Text = .HaendlerPLZ
                txtHaendlerOrt.Text = .HaendlerOrt
                txtHaendlerApName.Text = .HaendlerAnsprech
                txtHaendlerTelefon.Text = .HaendlerTelefon
                txtHaendlerTelefon2.Text = .HaendlerTelefon2
                txtHaendlerMail.Text = .HaendlerMail

                '***Fahrzeugempfänger***
                If Len(Trim(txtEmpfName.Text)) = 0 Then
                    .EmpfName = .FnName
                End If

                txtEmpfName.Text = .EmpfName
                txtEmpfStrasse.Text = .EmpfStrasse
                txtEmpfPLZ.Text = .EmpfPLZ
                txtEmpfOrt.Text = .EmpfOrt
                txtEmpfAPName.Text = .EmpfAnsprechpartner
                txtEmpfAPTelefon.Text = .EmpfTelefon
                txtEmpfAPTelefon2.Text = .EmpfTelefon2
                txtEmpfAPMail.Text = .EmpfMail


            End With


            '***Dropdown Leasingnehmer Ländercodes***
            drpLnLand = GetDropDownLand(drpLnLand)
            drpLnLand.SelectedValue = "DE"

            '***Dropdown Händler Ländercodes***
            drpHaendlerLand = GetDropDownLand(drpHaendlerLand)
            drpHaendlerLand.SelectedValue = "DE"

            '***Dropdown Fahrzeugempfänger Ländercodes***
            drpEmpfLand = GetDropDownLand(drpEmpfLand)
            drpEmpfLand.SelectedValue = "DE"


        End If
    End Sub

    '----------------------------------------------------------------------
    ' Methode:      AddToSessionObject
    ' Autor:        SFa
    ' Beschreibung: Erfasste Daten im Sessionobjekt sichern
    ' Erstellt am:  03.09.2008
    ' ITA:          2150
    '----------------------------------------------------------------------
    Private Sub AddToSessionObject()

        With mUeberfDAD
            '***Leasingnehmer***
            .LeasingnehmerReferenz = txtReferenz.Text
            .Leasingnehmernummer = txtLnNummer.Text
            .Leasingnehmer = txtLnName.Text
            .LeasingnehmerStrasse = txtLnStrasse.Text
            .LeasingnehmerPLZ = txtLnPLZ.Text
            .LeasingnehmerOrt = txtLnOrt.Text

            '***Fahrzeugnutzer***
            .FnName = txtFnName.Text
            .FnTelefon = txtFnTelefon.Text
            .FnMail = txtFnMail.Text

            '***Aus Zulassung***
            .LnFahrgestellnummer = txtFahrgestellnummer.Text
            .Fahrzeugtyp = txtLnFahrzeugtyp.Text

            '***Auslieferung***
            .Kennzeichen = txtKennzeichen.Text

            '.FahrzeugStatus = drpFahrzeugstatus.SelectedItem.Value

            If rdbBereifung.SelectedIndex > -1 Then
                .SomWin = rdbBereifung.SelectedItem.Value
            End If


            'schreiben  Dienstleistungsdetails in das Obj
            '-----------------------------------------------------------
            .FzgEinweisung = IIf(chkEinweisung.Checked = True, "X", "").ToString

            .Tanken = IIf(chkWagenVolltanken.Checked = True, "X", "").ToString

            .Waesche = IIf(chkWw.Checked = True, "X", "").ToString
            .Servicekarte = IIf(chkServicekarte.Checked = True, "X", "").ToString

            .Tankkarten = txtTankkarten.Text

            If chkHandyadapter.Checked = True Then
                .HandyAdapter = lblHandyadapter.Text
            Else
                .HandyAdapter = String.Empty
            End If

            If chkVerbandskasten.Checked = True Then
                .Verbandskasten = lblVerbandskasten.Text
            Else
                .Verbandskasten = String.Empty
            End If

            If chkNaviCD.Checked = True Then
                .NaviCD = lblNaviCD.Text
            Else
                .NaviCD = String.Empty
            End If

            If chkWarndreieck.Checked = True Then
                .Warndreieck = lblWarndreieck.Text
            Else
                .Warndreieck = String.Empty
            End If

            If chkWarnweste.Checked = True Then
                .Warnweste = lblWarnweste.Text
            Else
                .Warnweste = String.Empty
            End If

            If chkFussmatten.Checked = True Then
                .Fussmatten = lblFussmatten.Text
            Else
                .Fussmatten = String.Empty
            End If

            If chkFKontrolle.Checked = True Then
                .FKontrolle = lblFKontrolle.Text
            Else
                .FKontrolle = String.Empty
            End If
            '-----------------------------------------------------------



            .AuslieferungDatum = txtAuslieferungDatum.Text

            .TerminhinweisAuslieferungValue = rdoAuslieferung.SelectedItem.Value
            .TerminhinweisAuslieferung = rdoAuslieferung.SelectedItem.Text


            If rdbFahrzeugklasse.SelectedIndex > -1 Then
                .FahrzeugklasseValue = rdbFahrzeugklasse.SelectedItem.Value
                .FahrzeugklasseText = rdbFahrzeugklasse.SelectedItem.Text
            End If


            .BemerkungAus = txtBemerkung.Text

            '***Händler***
            .HaendlerName1 = txtHaendlerName.Text
            .HaendlerStrasse = txtHaendlerStrasse.Text
            .HaendlerPLZ = txtHaendlerPLZ.Text
            .HaendlerOrt = txtHaendlerOrt.Text
            .HaendlerAnsprech = txtHaendlerApName.Text
            .HaendlerTelefon = txtHaendlerTelefon.Text
            .HaendlerMail = txtHaendlerMail.Text

            '***Fahrzeugempfänger***
            .EmpfName = txtEmpfName.Text
            .EmpfStrasse = txtEmpfStrasse.Text
            .EmpfPLZ = txtEmpfPLZ.Text
            .EmpfOrt = txtEmpfOrt.Text
            .EmpfAnsprechpartner = txtEmpfAPName.Text
            .EmpfTelefon = txtEmpfAPTelefon.Text
            .EmpfMail = txtEmpfAPMail.Text


        End With

        Session("UeberfData") = mUeberfDAD

    End Sub

    '----------------------------------------------------------------------
    ' Methode:      PflichtfelderValidieren
    ' Autor:        SFa
    ' Beschreibung: Überprüfen, ob die Pflichtfelder korrekt gefüllt wurden.
    ' Erstellt am:  03.09.2008
    ' ITA:          2150
    '----------------------------------------------------------------------
    Private Function PflichtfelderValidieren() As Boolean
        Dim PflichtfeldError As Boolean
        Dim ErrorElse As String = String.Empty

        'Wurden alle Pflichtfelder korrekt gefüllt?
        'Fahrzeugnutzer
        If Trim(txtFnName.Text).Length = 0 Then
            PflichtfeldError = True
            txtFnName.BorderColor = Color.Red
        End If
        'Leasingnehmer
        If Trim(txtLnNummer.Text).Length = 0 Then
            PflichtfeldError = True
            txtLnNummer.BorderColor = Color.Red
        End If

        If Trim(txtLnName.Text).Length = 0 Then
            PflichtfeldError = True
            txtLnName.BorderColor = Color.Red
        End If

        If Trim(txtLnStrasse.Text).Length = 0 Then
            PflichtfeldError = True
            txtLnStrasse.BorderColor = Color.Red
        End If

        If Trim(txtLnPLZ.Text).Length = 0 Then
            PflichtfeldError = True
            txtLnPLZ.BorderColor = Color.Red
        End If

        If Trim(txtLnOrt.Text).Length = 0 Then
            PflichtfeldError = True
            txtLnOrt.BorderColor = Color.Red
        End If

        If Trim(txtLnFahrzeugtyp.Text).Length = 0 Then
            PflichtfeldError = True
            txtLnFahrzeugtyp.BorderColor = Color.Red
        End If

        If Trim(txtFahrgestellnummer.Text).Length = 0 Then
            PflichtfeldError = True
            txtFahrgestellnummer.BorderColor = Color.Red
        End If

        If rdbBereifung.SelectedIndex < 0 Then
            PflichtfeldError = True
            rdbBereifung.BorderWidth = 1
            rdbBereifung.BorderColor = Color.Red
        End If

        If rdbFahrzeugklasse.SelectedIndex < 0 Then
            PflichtfeldError = True
            rdbFahrzeugklasse.BorderWidth = 1
            rdbFahrzeugklasse.BorderColor = Color.Red
        End If


        If Trim(txtAuslieferungDatum.Text).Length = 0 Then
            PflichtfeldError = True
            txtAuslieferungDatum.BorderColor = Color.Red
        ElseIf IsDate(txtAuslieferungDatum.Text) = False Then
            PflichtfeldError = True
            txtAuslieferungDatum.BorderColor = Color.Red
            ErrorElse = "Ungültiges Auslieferungsdatum."
        End If


        If Trim(txtHaendlerName.Text).Length = 0 Then
            PflichtfeldError = True
            txtHaendlerName.BorderColor = Color.Red
        End If

        If Trim(txtHaendlerStrasse.Text).Length = 0 Then
            PflichtfeldError = True
            txtHaendlerStrasse.BorderColor = Color.Red
        End If

        If Trim(txtHaendlerPLZ.Text).Length = 0 Then
            PflichtfeldError = True
            txtHaendlerPLZ.BorderColor = Color.Red
        End If


        If Trim(txtHaendlerOrt.Text).Length = 0 Then
            PflichtfeldError = True
            txtHaendlerOrt.BorderColor = Color.Red
        End If

        If Trim(txtHaendlerApName.Text).Length = 0 Then
            PflichtfeldError = True
            txtHaendlerApName.BorderColor = Color.Red
        End If

        If Trim(txtHaendlerTelefon.Text).Length = 0 Then
            PflichtfeldError = True
            txtHaendlerTelefon.BorderColor = Color.Red
        End If

        If Trim(txtEmpfName.Text).Length = 0 Then
            PflichtfeldError = True
            txtEmpfName.BorderColor = Color.Red
        End If

        If Trim(txtEmpfStrasse.Text).Length = 0 Then
            PflichtfeldError = True
            txtEmpfStrasse.BorderColor = Color.Red
        End If

        If Trim(txtEmpfPLZ.Text).Length = 0 Then
            PflichtfeldError = True
            txtEmpfPLZ.BorderColor = Color.Red
        End If


        If Trim(txtEmpfOrt.Text).Length = 0 Then
            PflichtfeldError = True
            txtEmpfOrt.BorderColor = Color.Red
        End If

        If Trim(txtEmpfAPName.Text).Length = 0 Then
            PflichtfeldError = True
            txtEmpfAPName.BorderColor = Color.Red
        End If

        If Trim(txtEmpfAPTelefon.Text).Length = 0 Then
            PflichtfeldError = True
            txtEmpfAPTelefon.BorderColor = Color.Red
        End If

        If PflichtfeldError = True Then
            lblError.Text = ERROR_MESSAGE_PFLICHTFELD & " " & ErrorElse
            Return PflichtfeldError
        End If
    End Function

    '----------------------------------------------------------------------
    ' Methode:      CheckWinterreifenControls
    ' Autor:        SFa
    ' Beschreibung: Initialisiert die Controls für das Winterreifenhandling
    ' Erstellt am:  03.09.2008
    ' ITA:          2150
    '----------------------------------------------------------------------
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

    '----------------------------------------------------------------------
    ' Methode:      SetMenu
    ' Autor:        SFa
    ' Beschreibung: Setzt das Übersichtsmenü nach Beauftragungsart zusammen
    ' Erstellt am:  18.09.2008
    ' ITA:          2150
    '----------------------------------------------------------------------
    Private Sub SetMenu()

        Dim Auftragsart As String = String.Empty
        Dim Ausgabe As String
        Dim VariableAusgabe As String = String.Empty
        Dim TableLength As Long = 600

        If IsNothing(Session("UeberfData")) = False Then
            'mUeberfDAD = New UeberfDAD(mu
            mUeberfDAD = DirectCast(Session("UeberfData"), UeberfDAD)
        End If


        Select Case mUeberfDAD.Auftragsart
            Case Auftragsarten.Auslieferung.ToString
                VariableAusgabe = "<td bgcolor=""#000099"" width=""170"" height=""31"" style=""text-align: center""><b>Auslieferung<b></td>"

            Case Auftragsarten.ZulassungAuslieferung.ToString
                VariableAusgabe = "<td bgcolor=""#666666"" width=""170"" height=""31"" style=""text-align: center""><b>Zulassung<b></td>" & _
                  "<td bgcolor=""#666666"" width=""600"" height=""31"" style=""text-align: center""><b>Versand Schein u. Schilder<b></td>" & _
                  "<td bgcolor=""#000099"" width=""170"" height=""31"" style=""text-align: center""><b>Auslieferung<b></td>"
            Case Auftragsarten.AuslieferungRueckfuehrung.ToString
                VariableAusgabe = "<td bgcolor=""#000099"" width=""170"" height=""31"" style=""text-align: center""><b>Auslieferung<b></td>" & _
                                 "<td bgcolor=""#666666"" width=""650"" height=""31"" style=""text-align: center""><b>Rückführung/Anschlußfahrt<b></td>"
            Case Auftragsarten.Alles.ToString
                VariableAusgabe = "<td bgcolor=""#666666"" width=""170"" height=""31"" style=""text-align: center""><b>Zulassung<b></td>" & _
                "<td bgcolor=""#666666"" width=""650"" height=""31"" style=""text-align: center""><b>Versand Schein u. Schilder<b></td>" & _
                                 "<td bgcolor=""#000099"" width=""170"" height=""31"" style=""text-align: center""><b>Auslieferung<b></td>" & _
                                 "<td bgcolor=""#666666"" width=""650"" height=""31"" style=""text-align: center""><b>Rückführung/Anschlußfahrt<b></td>"

        End Select

        Ausgabe = "<table style=""width: 620px; color: #FFFFFF;"">" & _
         "<tr>" & _
         "<td bgcolor=""#666666"" width=""96"" height=""31"" style=""text-align: center""><b>Start<b></td>"

        Ausgabe = Ausgabe & VariableAusgabe

        Ausgabe = Ausgabe & "<td bgcolor=""#666666"" width=""250"" height=""31"" style=""text-align: center""><b>Übersicht<b></td></tr></table>"

        ltAnzeige.Text = Ausgabe

    End Sub

    '----------------------------------------------------------------------
    ' Methode:      GetDropDownLand
    ' Autor:        SFa
    ' Beschreibung: Füllt ein DropDownList Control mit Ländercodes
    ' Erstellt am:  22.09.2008
    ' ITA:          2150
    '----------------------------------------------------------------------
    Private Function GetDropDownLand(ByVal drpTemp As DropDownList) As DropDownList

        drpTemp.DataSource = mUeberfDAD.Laender
        drpTemp.DataTextField = "FullDesc"
        drpTemp.DataValueField = "Land1"
        drpTemp.DataBind()

        Return drpTemp

    End Function

#End Region


End Class
' ************************************************
' $History: UeberfDADAus.aspx.vb $
' 
' *****************  Version 12  *****************
' User: Jungj        Date: 3.11.08    Time: 11:15
' Updated in $/CKAG/Applications/AppUeberf/Forms
' ITA 2343 fertigstellung 
'
' ************************************************