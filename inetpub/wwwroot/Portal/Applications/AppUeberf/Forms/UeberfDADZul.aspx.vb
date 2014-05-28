Option Explicit On
Option Strict On

Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common
Imports AppUeberf.Ueberf_01

Partial Public Class UeberfDADZul
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

    Private Const ERROR_MESSAGE_PFLICHTFELD As String = "Bitte füllen Sie alle Pflichfelder(*) aus."

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private mUeberfDAD As UeberfDAD
    Private mCalClicked As Int32
    Private mIbtCalZul As Boolean
    Private mIbtCalEVBVon As Boolean
    Private mIbtCalEVBBis As Boolean

    Private Enum AbweichenderVersicherungsnehmer
        Halter = 0
        Leasingnehmer = 1
        Leasinggesellschaft = 2
        Dritte = 3
    End Enum

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
                'FillControls()
            End If

            SetMenu()


        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Protected Sub calVon_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles calVon.SelectionChanged

        txtZulassungsdatum.Text = calVon.SelectedDate.ToShortDateString
        calVon.Visible = False

    End Sub

    Protected Sub calEVBVon_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles calEVBVon.SelectionChanged
        txtEVBVon.Text = calEVBVon.SelectedDate.ToShortDateString
        calEVBVon.Visible = False
    End Sub

    Protected Sub calEVBBis_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles calEVBBis.SelectionChanged
        txtEVBBis.Text = calEVBBis.SelectedDate.ToShortDateString
        calEVBBis.Visible = False
    End Sub

    Protected Sub lbtWeiter_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtWeiter.Click
        If PflichtfelderValidieren() = False Then
            Response.Redirect("UeberfDADVss.aspx?AppID=" & Session("AppID").ToString)
        End If
    End Sub

    Protected Sub ibtCalZul_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtCalZul.Click
        calVon.Visible = True
    End Sub

    Protected Sub ibtCalEVBVon_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtCalEVBVon.Click
        calEVBVon.Visible = True
    End Sub

    Protected Sub ibtCalEVBBis_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtCalEVBBis.Click
        calEVBBis.Visible = True
    End Sub

    Protected Sub chkHalterIstLN_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkHalterIstLN.CheckedChanged
        ChangeHalter(chkHalterIstLN.Checked)
    End Sub
    Protected Sub ibtSearchLN_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtSearchLN.Click
        Response.Redirect("UeberfDADAdresse.aspx?AppID=" & Session("AppID").ToString & "&Art=" & "LNEHMER")
    End Sub

    Protected Sub ibtSearchHalter_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtSearchHalter.Click
        Response.Redirect("UeberfDADAdresse.aspx?AppID=" & Session("AppID").ToString & "&Art=" & "HALTER")

        'Wenn abweichender Versicherungsnehmer = Halter
        If CLng(rdoVersicherung.SelectedItem.Value) = AbweichenderVersicherungsnehmer.Halter Then
            'Halterdaten übernehmen
            Me.txtVersNehmer.Text = txtShName.Text
            Me.txtVersStrasse.Text = txtShStrasse.Text
            Me.txtVersPLZ.Text = txtShPLZ.Text
            Me.txtVersOrt.Text = txtShOrt.Text
        End If

    End Sub

    Protected Sub ibtSearchVersicherer_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtSearchVersicherer.Click
        Response.Redirect("UeberfDADAdresse.aspx?AppID=" & Session("AppID").ToString & "&Art=" & "VERSICHERER")
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
    Protected Sub rdoVersicherung_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rdoVersicherung.SelectedIndexChanged

        Try
            Select Case CLng(rdoVersicherung.SelectedItem.Value)
                Case AbweichenderVersicherungsnehmer.Halter
                    'Halterdaten übernehmen
                    Me.txtVersNehmer.Text = txtShName.Text
                    Me.txtVersStrasse.Text = txtShStrasse.Text
                    Me.txtVersPLZ.Text = txtShPLZ.Text
                    Me.txtVersOrt.Text = txtShOrt.Text

                Case AbweichenderVersicherungsnehmer.Leasingnehmer
                    'Leasingnehmer übernehmen
                    Me.txtVersNehmer.Text = txtLnName.Text
                    Me.txtVersStrasse.Text = txtLnStrasse.Text
                    Me.txtVersPLZ.Text = txtLnPLZ.Text
                    Me.txtVersOrt.Text = txtLnOrt.Text

                Case AbweichenderVersicherungsnehmer.Leasinggesellschaft
                    'Versicherungsgesellschaft eintragen
                    mUeberfDAD.Adresssuche("LGEBER", "", "", "")

                    If IsNothing(mUeberfDAD.Adressen) = False Then
                        If mUeberfDAD.Adressen.Rows.Count > 0 Then

                            With mUeberfDAD.Adressen
                                Me.txtVersNehmer.Text = .Rows(0)("NAME1").ToString
                                Me.txtVersStrasse.Text = .Rows(0)("STRAS").ToString
                                Me.txtVersPLZ.Text = .Rows(0)("PSTLZ").ToString
                                Me.txtVersOrt.Text = .Rows(0)("ORT01").ToString
                            End With


                        End If
                    End If


                Case AbweichenderVersicherungsnehmer.Dritte
                    'Felder für manuelle Erfassung leeren
                    txtVersNehmer.Text = String.Empty
                    txtVersOrt.Text = String.Empty
                    txtVersPLZ.Text = String.Empty
                    txtVersStrasse.Text = String.Empty
            End Select
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try


    End Sub

    Protected Sub lbtBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtBack.Click
        Response.Redirect("UeberfDADStart.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Protected Sub btnZulkreis_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnZulkreis.Click
        Dim clsUeberf As Ueberf_01

        clsUeberf = New Ueberf_01(m_User, m_App, "")

        If Len(txtShPLZ.Text) <> 5 Then
            lblError.Text = "Bitte geben Sie eine 5-stellige PLZ ein."
        Else
            clsUeberf.getSTVA(txtShPLZ.Text)

            If Not clsUeberf.tblKreis Is Nothing Then

                mUeberfDAD.tblKreis = clsUeberf.tblKreis

                If clsUeberf.tblKreis.Rows.Count > 0 Then
                    txtZulkreis.Text = clsUeberf.tblKreis.Rows(0)("ZKFZKZ").ToString()
                   
                Else
                    lblError.Text = "Für die eingegebene PLZ konnte kein Zulassungskreis ermittelt werden."
                    txtZulkreis.Text = ""
                End If
            End If
        End If

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
    ' Erstellt am:  27.08.2008
    ' ITA:          2150
    '----------------------------------------------------------------------
    Private Sub FillControls()

        If IsNothing(Session("UeberfData")) = False Then
            mUeberfDAD = DirectCast(Session("UeberfData"), UeberfDAD)

            With mUeberfDAD
                '***Leasingnehmer***
                lblVertragsnummer.Text = .LeasingnehmerReferenz
                txtLnNummer.Text = .Leasingnehmernummer
                txtLnName.Text = .Leasingnehmer
                txtLnStrasse.Text = .LeasingnehmerStrasse
                txtLnPLZ.Text = .LeasingnehmerPLZ
                txtLnOrt.Text = .LeasingnehmerOrt


                '***Halter***
                txtShName.Text = .Halter
                txtShName2.Text = .Halter2
                txtShStrasse.Text = .HalterStrasse
                txtShPLZ.Text = .HalterPLZ
                txtShOrt.Text = .HalterOrt

                '***Versicherungsnehmer = Halter?***
                'txtVersAnsprech.Text = .VersAnsprechpartner
                'txtVersTelefon.Text = .VersTelefon
                'txtVersMail.Text = .VersMail

                If Len(.VersNehmerArt) > 0 Then
                    rdoVersicherung.Items.FindByText(.VersNehmerArt).Selected = True
                End If

                If Len(.VersNehmer) > 0 Then
                    txtVersNehmer.Text = .VersNehmer
                    txtVersStrasse.Text = .VersNehmerStrasse
                    txtVersPLZ.Text = .VersNehmerPLZ
                    txtVersOrt.Text = .VersNehmerOrt
                Else
                    txtVersNehmer.Text = .Halter
                    txtVersStrasse.Text = .HalterStrasse
                    txtVersPLZ.Text = .HalterPLZ
                    txtVersOrt.Text = .HalterOrt
                End If

                '***Zulassung***
                txtBriefnummer.Text = .Briefnummer
                txtFahrgestellnummer.Text = .LnFahrgestellnummer
                txtLnFahrzeugtyp.Text = .Fahrzeugtyp
                txtZulassungsdatum.Text = .Zulassungsdatum
                txtWKennzeichen1.Text = .Wunschkennzeichen1
                txtWKennzeichen2.Text = .Wunschkennzeichen2
                txtWKennzeichen3.Text = .Wunschkennzeichen3
                txtZulkreis.Text = .Zulassungskreis
                txtResName.Text = .ResName
                txtResNr.Text = .ResNummer

                If .Feinstaub = "X" Then
                    chkFeinstaub.Checked = True
                End If

                If Len(.ZulTerminart) > 0 Then
                    rdoZulassungsdatum.Items.FindByText(.ZulTerminart).Selected = True
                End If

                If Len(.KfzSteuer) > 0 Then
                    drpKfzSteuer.SelectedValue = .KfzSteuer
                End If


                '***Versicherungsdaten***
                txtVersGesellschaft.Text = .VersGesellschaft
                txtEVBNummer.Text = .EVBNummer
                txtEVBVon.Text = .EVBVon
                txtEVBBis.Text = .EVBBis
            End With


            '***Dropdown Leasingnehmer Ländercodes***
            drpLnLand = GetDropDownLand(drpLnLand)
            drpLnLand.SelectedValue = "DE"

            '***Dropdown Halter Ländercodes***
            drpShLand = GetDropDownLand(drpShLand)
            drpShLand.SelectedValue = "DE"

            '***Dropdown Versicherungsnehmer Ländercodes***
            drpVersLand = GetDropDownLand(drpVersLand)
            drpVersLand.SelectedValue = "DE"

        End If
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


    '----------------------------------------------------------------------
    ' Methode:      AddToSessionObject
    ' Autor:        SFa
    ' Beschreibung: Erfasste Daten im Sessionobjekt sichern
    ' Erstellt am:  28.08.2008
    ' ITA:          2150
    '----------------------------------------------------------------------
    Private Sub AddToSessionObject()

        With mUeberfDAD
            '***Leasingnehmer***
            .Leasingnehmernummer = txtLnNummer.Text
            .Leasingnehmer = txtLnName.Text
            .LeasingnehmerStrasse = txtLnStrasse.Text
            .LeasingnehmerPLZ = txtLnPLZ.Text
            .LeasingnehmerOrt = txtLnOrt.Text
            .LeasingnehmerLand = drpLnLand.SelectedValue
            '***Halter***
            .Halter = txtShName.Text
            .Halter2 = txtShName2.Text
            .HalterStrasse = txtShStrasse.Text
            .HalterPLZ = txtShPLZ.Text
            .HalterOrt = txtShOrt.Text
            .HalterLand = drpShLand.SelectedValue

            '***Zulassung***
            .LnFahrgestellnummer = txtFahrgestellnummer.Text
            .Fahrzeugtyp = txtLnFahrzeugtyp.Text
            .Briefnummer = txtBriefnummer.Text
            .Zulassungsdatum = txtZulassungsdatum.Text
            .ZulTerminart = rdoZulassungsdatum.SelectedItem.Text
            .Wunschkennzeichen1 = txtWKennzeichen1.Text
            .Wunschkennzeichen2 = txtWKennzeichen2.Text
            .Wunschkennzeichen3 = txtWKennzeichen3.Text
            .Zulassungskreis = txtZulkreis.Text
            .ResNummer = txtResNr.Text
            .ResName = txtResName.Text

            If chkFeinstaub.Checked = True Then
                .Feinstaub = "X"
            Else
                .Feinstaub = String.Empty
            End If

            .KfzSteuer = drpKfzSteuer.SelectedValue

            '***Versicherungsdaten***
            .VersGesellschaft = txtVersGesellschaft.Text
            .VersNehmerArt = rdoVersicherung.SelectedItem.Text
            .VersNehmer = txtVersNehmer.Text
            .VersNehmerStrasse = txtVersStrasse.Text
            .VersNehmerPLZ = txtVersPLZ.Text
            .VersNehmerOrt = txtVersOrt.Text
            .VersNehmerLand = drpVersLand.SelectedValue
            .EVBNummer = txtEVBNummer.Text
            .EVBVon = txtEVBVon.Text
            .EVBBis = txtEVBBis.Text
            '.VersAnsprechpartner = txtVersAnsprech.Text
            '.VersMail = txtVersMail.Text
            '.VersTelefon = txtVersTelefon.Text


        End With

        Session("UeberfData") = mUeberfDAD


    End Sub

    '----------------------------------------------------------------------
    ' Methode:      ChangeHalter
    ' Autor:        SFa
    ' Beschreibung: Übernimmt den Leasingnehmer als Halter oder leert die
    '               Halter-Controls
    ' Erstellt am:  28.08.2008
    ' ITA:          1234
    '----------------------------------------------------------------------
    Private Sub ChangeHalter(ByVal SetAsLeasingnehmer As Boolean)
        If SetAsLeasingnehmer = True Then
            txtShName.Text = txtLnName.Text
            txtShStrasse.Text = txtLnStrasse.Text
            txtShPLZ.Text = txtLnPLZ.Text
            txtShOrt.Text = txtLnOrt.Text

            'Wenn abweichender Versicherungsnehmer = Halter
            If CLng(rdoVersicherung.SelectedItem.Value) = AbweichenderVersicherungsnehmer.Halter Then
                'Halterdaten übernehmen
                Me.txtVersNehmer.Text = txtShName.Text
                Me.txtVersStrasse.Text = txtShStrasse.Text
                Me.txtVersPLZ.Text = txtShPLZ.Text
                Me.txtVersOrt.Text = txtShOrt.Text
            End If

        Else
            txtShName.Text = String.Empty
            txtShStrasse.Text = String.Empty
            txtShPLZ.Text = String.Empty
            txtShOrt.Text = String.Empty
        End If
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

        If Trim(txtBriefnummer.Text).Length = 0 Then
            PflichtfeldError = True
            txtBriefnummer.BorderColor = Color.Red
        Else
            If Len(txtBriefnummer.Text) <> 8 Then
                PflichtfeldError = True
                txtBriefnummer.BorderColor = Color.Red
                ErrorElse = "Briefnummer nicht 8-stellig."
            End If
        End If

        If Trim(txtShName.Text).Length = 0 Then
            PflichtfeldError = True
            txtShName.BorderColor = Color.Red
        End If

        If Trim(txtShStrasse.Text).Length = 0 Then
            PflichtfeldError = True
            txtShStrasse.BorderColor = Color.Red
        End If

        If Trim(txtShPLZ.Text).Length = 0 Then
            PflichtfeldError = True
            txtShPLZ.BorderColor = Color.Red
        End If

        If Trim(txtShOrt.Text).Length = 0 Then
            PflichtfeldError = True
            txtShOrt.BorderColor = Color.Red
        End If

        If Trim(txtZulassungsdatum.Text).Length = 0 Then
            PflichtfeldError = True
            txtZulassungsdatum.BorderColor = Color.Red
        ElseIf IsDate(txtZulassungsdatum.Text) = False Then
            PflichtfeldError = True
            txtZulassungsdatum.BorderColor = Color.Red
            ErrorElse = "Ungültiges Zulassungsdatum."
        ElseIf CDate(txtZulassungsdatum.Text) < Today.AddDays(2) Then
            PflichtfeldError = True
            txtZulassungsdatum.BorderColor = Color.Red
            ErrorElse = "Das Zulassungsdatum muss 2 Tage in der Zukunft liegen."
        End If

        If Not Trim(txtEVBNummer.Text).Length = 7 Then
            PflichtfeldError = True
            txtEVBNummer.BorderColor = Color.Red
            ErrorElse = "Die EVB-Nummer muss 7-stellig sein."
        End If

        If Trim(txtVersNehmer.Text).Length = 0 Then
            PflichtfeldError = True
            txtVersNehmer.BorderColor = Color.Red
        End If

        If Trim(txtVersStrasse.Text).Length = 0 Then
            PflichtfeldError = True
            txtVersStrasse.BorderColor = Color.Red
        End If

        If Trim(txtVersPLZ.Text).Length = 0 Then
            PflichtfeldError = True
            txtVersPLZ.BorderColor = Color.Red
        End If

        If Trim(txtVersOrt.Text).Length = 0 Then
            PflichtfeldError = True
            txtVersOrt.BorderColor = Color.Red
        End If

        If PflichtfeldError = True Then
            lblError.Text = ERROR_MESSAGE_PFLICHTFELD & " " & ErrorElse
            Return PflichtfeldError
        End If
    End Function

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
            mUeberfDAD = DirectCast(Session("UeberfData"), UeberfDAD)
        End If


        Select Case mUeberfDAD.Auftragsart
            Case Auftragsarten.Zulassung.ToString
                VariableAusgabe = "<td bgcolor=""#000099"" width=""170"" height=""31"" style=""text-align: center""><b>Zulassung<b></td>" & _
                "<td bgcolor=""#666666"" width=""600"" height=""31"" style=""text-align: center""><b>Versand Schein u. Schilder<b></td>"
            Case Auftragsarten.ZulassungAuslieferung.ToString
                VariableAusgabe = "<td bgcolor=""#000099"" width=""170"" height=""31"" style=""text-align: center""><b>Zulassung<b></td>" & _
                  "<td bgcolor=""#666666"" width=""600"" height=""31"" style=""text-align: center""><b>Versand Schein u. Schilder<b></td>" & _
                  "<td bgcolor=""#666666"" width=""170"" height=""31"" style=""text-align: center""><b>Auslieferung<b></td>"
            Case Auftragsarten.Alles.ToString
                VariableAusgabe = "<td bgcolor=""#000099"" width=""170"" height=""31"" style=""text-align: center""><b>Zulassung<b></td>" & _
                "<td bgcolor=""#666666"" width=""650"" height=""31"" style=""text-align: center""><b>Versand Schein u. Schilder<b></td>" & _
                                 "<td bgcolor=""#666666"" width=""170"" height=""31"" style=""text-align: center""><b>Auslieferung<b></td>" & _
                                 "<td bgcolor=""#666666"" width=""650"" height=""31"" style=""text-align: center""><b>Rückführung/Anschlußfahrt<b></td>"

        End Select

        Ausgabe = "<table style=""width: 620px; color: #FFFFFF;"">" & _
         "<tr>" & _
         "<td bgcolor=""#666666"" width=""96"" height=""31"" style=""text-align: center""><b>Start<b></td>"

        Ausgabe = Ausgabe & VariableAusgabe

        Ausgabe = Ausgabe & "<td bgcolor=""#666666"" width=""250"" height=""31"" style=""text-align: center""><b>Übersicht<b></td></tr></table>"

        ltAnzeige.Text = Ausgabe

    End Sub


#End Region




End Class
' ************************************************
' $History: UeberfDADZul.aspx.vb $
' 
' *****************  Version 18  *****************
' User: Jungj        Date: 16.12.08   Time: 11:08
' Updated in $/CKAG/Applications/AppUeberf/Forms
' evbpr�fung
' 
' *****************  Version 17  *****************
' User: Jungj        Date: 10.12.08   Time: 11:24
' Updated in $/CKAG/Applications/AppUeberf/Forms
' ITA 2463 testfertig
'
' ************************************************