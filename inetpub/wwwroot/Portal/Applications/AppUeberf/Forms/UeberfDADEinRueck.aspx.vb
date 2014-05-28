Option Explicit On
Option Strict On

Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Partial Public Class UeberfDADEinRueck
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

    Private Enum Anlieferung
        Haendler = 0
        Autoland = 1
        Dritte = 2
    End Enum


    Private Const ERROR_MESSAGE_PFLICHTFELD As String = "Bitte füllen Sie alle Pflichfelder(*) aus."

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private mUeberfDAD As UeberfDAD
    Private mCalClicked As Int32
    Private mIbtCalWunschtermin As Boolean
    Protected WithEvents ibtRefresh0 As ImageButton

#End Region

#Region "Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Hier Benutzercode zur Seiteninitialisierung einfügen
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
                FillControls()
            End If

            SetMenu()

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Protected Sub ibtCalWunschtermin_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtCalWunschtermin.Click
        calWunschtermin.Visible = True
    End Sub

    Protected Sub calWunschtermin_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles calWunschtermin.SelectionChanged
        txtWunschtermin.Text = calWunschtermin.SelectedDate.ToShortDateString
        calWunschtermin.Visible = False
    End Sub


    Protected Sub ibtRefresh0_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtRefresh0.Click
        If Trim(txtLnNummer.Text) <> String.Empty Then
            Try
                Dim LnData As New UeberfDADTables(m_User, m_App, "")
                Dim TempTable As DataTable

                TempTable = LnData.GetLnKunde(Me.txtLnNummer.Text)
                With mUeberfDAD
                    .RLeasingnehmernummer = Me.txtLnNummer.Text
                    If IsNothing(TempTable) = False Then
                        If TempTable.Rows.Count > 0 Then
                            .RAnName = TempTable.Rows(0)("NAME1_RL").ToString
                            .RAnStrasse = TempTable.Rows(0)("STRAS_RL").ToString
                            .RAnPLZ = TempTable.Rows(0)("PSTLZ_RL").ToString
                            .RAnOrt = TempTable.Rows(0)("ORT01_RL").ToString
                            .RAnAnsprechpartner = TempTable.Rows(0)("ANSPP_RL").ToString
                            .RAnTelefon = TempTable.Rows(0)("TELF1_RL").ToString
                            .RAnMail = TempTable.Rows(0)("EMAIL_RL").ToString
                            .RAnLand = TempTable.Rows(0)("LAND1_RL").ToString
                            If .Auftragsart = Auftragsarten.Rueckfuehrung.ToString Then
                                'nur bei einer Rückführung Abholadresse befüllen beim hochziehen der Leasingnehmerdaten
                                .RAbName = TempTable.Rows(0)("NAME1_ZL").ToString
                                .RAbOrt = TempTable.Rows(0)("ORT01_ZL").ToString
                                .RAbStrasse = TempTable.Rows(0)("STRAS_ZL").ToString
                                .RAbPLZ = TempTable.Rows(0)("PSTLZ_ZL").ToString
                            End If
                        End If
                    End If
                End With

                FillControls()
            Catch ex As Exception
                lblError.Text = ex.Message
            End Try

        End If
    End Sub

    Protected Sub ibtSearchAnlieferung_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtSearchAnlieferung.Click
        Select Case CStr(rdoAnlieferadresse.SelectedItem.Value)
            Case Anlieferung.Haendler.ToString
                Response.Redirect("UeberfDADAdresse.aspx?AppID=" & Session("AppID").ToString & "&Art=" & "HAENDLER" & "&Src=RUECK")
            Case Anlieferung.Autoland.ToString
                Response.Redirect("UeberfDADAdresse.aspx?AppID=" & Session("AppID").ToString & "&Art=" & "AUTOLAND" & "&Src=RUECK")
            Case Anlieferung.Dritte.ToString
                Response.Redirect("UeberfDADAdresse.aspx?AppID=" & Session("AppID").ToString & "&Art=" & "SONSTIGES" & "&Src=RUECK")
        End Select

    End Sub

    Protected Sub ibtSearchAbhol_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtSearchAbhol.Click
        Response.Redirect("UeberfDADAdresse.aspx?AppID=" & Session("AppID").ToString & "&Art=" & "LNEHMER" & "&Src=RUECK")
    End Sub

    Protected Sub lbtWeiter_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtWeiter.Click
        If PflichtfelderValidieren() = False Then
            Response.Redirect("UeberfDADSave.aspx?AppID=" & Session("AppID").ToString)
        End If

    End Sub

    Protected Sub lbtBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtBack.Click

        Select Case mUeberfDAD.Auftragsart
            Case Auftragsarten.Rueckfuehrung.ToString
                Response.Redirect("UeberfDADStart.aspx?AppID=" & Session("AppID").ToString)

            Case Auftragsarten.AuslieferungRueckfuehrung.ToString, Auftragsarten.Alles.ToString
                Response.Redirect("UeberfDADAus.aspx?AppID=" & Session("AppID").ToString)
        End Select
    End Sub

    Protected Sub rdoAnlieferadresse_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rdoAnlieferadresse.SelectedIndexChanged

        txtAnAnsprech.Text = String.Empty
        txtAnHandy.Text = String.Empty
        txtAnMail.Text = String.Empty
        txtAnName.Text = String.Empty
        txtAnOrt.Text = String.Empty
        txtAnPLZ.Text = String.Empty
        txtAnStrasse.Text = String.Empty
        txtAnTelefon.Text = String.Empty


        'mUeberfDAD.RAnlieferungArt = rdoAnlieferadresse.SelectedItem.Value

        Try
            If rdoAnlieferadresse.SelectedValue = Anlieferung.Autoland.ToString Then
                If Trim(txtAbStrasse.Text) = String.Empty OrElse _
                    Trim(txtAbPLZ.Text) = String.Empty OrElse _
                    Trim(txtAbOrt.Text) = String.Empty Then

                    rdoAnlieferadresse.SelectedValue = Anlieferung.Dritte.ToString

                    'rdoAnlieferadresse.SelectedValue = Anlieferung.Autoland.ToString
                    lblError.Text = "Bitte geben Sie eine vollständige Abholadresse ein."
                    Exit Sub

                Else
                    ibtSearchAbhol.Enabled = False
                    Dim AdressTable As DataTable

                    mUeberfDAD.CheckAdressen = Nothing


                    mUeberfDAD.CheckAdresse(txtAbStrasse.Text, txtAbPLZ.Text, txtAbOrt.Text)

                    AdressTable = mUeberfDAD.CheckAdressen

                    If AdressTable.Rows.Count > 1 Then
                        lblAnlieferungError.Visible = True
                        drpAutoland.Visible = True
                        ibtRefresh.Visible = True

                        drpAutoland.DataSource = AdressTable

                        drpAutoland.DataTextField = "Adresse"

                        drpAutoland.DataBind()


                    ElseIf AdressTable.Rows.Count = 1 Then
                        Dim GeoX As String
                        Dim GeoY As String
                        Dim TempAdresse As String


                        GeoX = AdressTable.Rows(0)("GEOX").ToString
                        GeoY = AdressTable.Rows(0)("GEOY").ToString
                        TempAdresse = AdressTable.Rows(0)("Adresse").ToString

                        AdressTable = mUeberfDAD.GetAutoland(GeoX, GeoY, TempAdresse)

                        If AdressTable.Rows.Count > 0 Then

                            txtAnName.Text = AdressTable.Rows(0)("NAME1").ToString
                            txtAnStrasse.Text = AdressTable.Rows(0)("STRASSE").ToString & " " & _
                            AdressTable.Rows(0)("HAUSNR").ToString
                            txtAnPLZ.Text = AdressTable.Rows(0)("POSTLTZ").ToString
                            txtAnOrt.Text = AdressTable.Rows(0)("ORT").ToString
                            txtAnAnsprech.Text = AdressTable.Rows(0)("NAME2").ToString
                            txtAnTelefon.Text = AdressTable.Rows(0)("TELNR").ToString
                            txtAnMail.Text = AdressTable.Rows(0)("SMTP_ADDR").ToString

                        End If


                    End If

                End If
            Else
                lblAnlieferungError.Visible = False
                drpAutoland.Visible = False
                ibtRefresh.Visible = False
            End If
        Catch ex As Exception
            rdoAnlieferadresse.SelectedValue = Anlieferung.Dritte.ToString
            lblError.Text = ex.Message & " Bitte korrigieren Sie die Abholadresse."
        End Try


    End Sub


    Protected Sub ibtRefresh_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtRefresh.Click
        Dim GeoX As String
        Dim GeoY As String
        Dim TempAdresse As String
        Dim AdressTable As DataTable
        Dim TempRow As DataRow


        TempRow = mUeberfDAD.CheckAdressen.Select("ADRESSE = '" & drpAutoland.SelectedItem.Text & "'")(0)


        GeoX = TempRow("GEOX").ToString
        GeoY = TempRow("GEOY").ToString
        TempAdresse = TempRow("Adresse").ToString

        AdressTable = mUeberfDAD.GetAutoland(GeoX, GeoY, TempAdresse)

        If AdressTable.Rows.Count > 0 Then

            txtAnName.Text = AdressTable.Rows(0)("NAME1").ToString
            txtAnStrasse.Text = AdressTable.Rows(0)("STRASSE").ToString & " " & _
            AdressTable.Rows(0)("HAUSNR").ToString
            txtAnPLZ.Text = AdressTable.Rows(0)("POSTLTZ").ToString
            txtAnOrt.Text = AdressTable.Rows(0)("ORT").ToString

            lblAnlieferungError.Visible = False
            drpAutoland.Visible = False
            ibtRefresh.Visible = False

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
    ' Erstellt am:  29.08.2008
    ' ITA:          2150
    '----------------------------------------------------------------------
    Private Sub FillControls()

        If IsNothing(Session("UeberfData")) = False Then
            mUeberfDAD = DirectCast(Session("UeberfData"), UeberfDAD)


            With mUeberfDAD
                '***Leasingnehmer***
                txtLnNummer.Text = .RLeasingnehmernummer
                txtLnVertragsnr.Text = .RLeasingnehmerReferenz
                txtLnAnsprech.Text = .RAnsprechLeasing


                '***Fahrzeugnutzer***
                txtFnName.Text = .RFnName
                txtFnTelefon.Text = .RFnTelefon
                txtFnMail.Text = .RFnMail


                '***Detaildaten***
                txtFahrgestellnummer.Text = .RFahrgestellnummer
                txtLnFahrzeugtyp.Text = .RFahrzeugtyp

                txtKennzeichen.Text = .RKennzeichen

                txtBemerkung.Text = .RBemerkung


                If Len(.RFahrzeugStatus) > 0 Then
                    If .RFahrzeugStatus = "Alt" Then
                        chkAbmeldung.Checked = True
                    End If

                End If

                If Len(.RSomWin) > 0 Then
                    rdbBereifung.Items.FindByValue(.RSomWin).Selected = True
                End If

                If Len(.RFahrzeugklasseValue) > 0 Then
                    rdbFahrzeugklasse.SelectedItem.Value = .RFahrzeugklasseValue
                    rdbFahrzeugklasse.SelectedItem.Text = .RFahrzeugklasseText
                End If


                '***Abholadresse***
                'Empfängeradresse aus Auslieferung übernehmen wenn Abholadresse noch nicht gefüllt
                If .Auftragsart <> Auftragsarten.Rueckfuehrung.ToString Then
                    If Len(.RAbName) < 1 Then
                        If Len(.EmpfName) > 0 Then

                            .RAbName = .EmpfName
                            .RAbStrasse = .EmpfStrasse
                            .RAbPLZ = .EmpfPLZ
                            .RAbOrt = .EmpfOrt
                            .RAbAnsprechpartner = .EmpfAnsprechpartner
                            .RAbTelefon = .EmpfTelefon
                            .RAbMail = .EmpfMail

                            txtAbName.Enabled = False
                            txtAbStrasse.Enabled = False
                            txtAbPLZ.Enabled = False
                            txtAbOrt.Enabled = False
                            txtAbAnsprech.Enabled = False
                            txtAbTelefon.Enabled = False

                            ibtSearchAbhol.Enabled = False

                        End If
                    End If
                Else
                    If .RAbName = String.Empty Then
                        .RAbName = .Leasingnehmer
                        .RAbStrasse = .LeasingnehmerStrasse
                        .RAbPLZ = .LeasingnehmerPLZ
                        .RAbOrt = .LeasingnehmerOrt
                    End If

                    ibtCalWunschtermin.Visible = True
                    txtWunschtermin.Visible = True
                    rdoAuslieferung.Visible = True
                    lblWunschtermin.Visible = True
                    lblTerminart.Visible = True

                    txtWunschtermin.Text = .RWunschtermin

                    If Len(.RWunschtermin) > 0 Then
                        rdoAuslieferung.Items.FindByText(.RWunschterminart).Selected = True
                    End If

                End If

                txtAbName.Text = .RAbName
                txtAbStrasse.Text = .RAbStrasse
                txtAbPLZ.Text = .RAbPLZ
                txtAbOrt.Text = .RAbOrt
                txtAbAnsprech.Text = .RAbAnsprechpartner
                txtAbTelefon.Text = .RAbTelefon
                txtAbHandy.Text = .RAbHandy
                txtAbMail.Text = .RAbMail

                '***Anlieferadresse***
                If Len(.RAnlieferungArt) > 0 Then
                    rdoAnlieferadresse.ClearSelection()
                    rdoAnlieferadresse.Items.FindByValue(.RAnlieferungArt).Selected = True




                    'If .RAnlieferungArt <> "Dritte" Then
                    '    rdoAnlieferadresse.Items.FindByValue(.RAnlieferungArt).Selected = False
                    'End If
                    'rdoAnlieferadresse.SelectedItem.Value = .RAnlieferungArt
                End If


                txtAnName.Text = .RAnName
                txtAnStrasse.Text = .RAnStrasse
                txtAnPLZ.Text = .RAnPLZ
                txtAnOrt.Text = .RAnOrt
                txtAnAnsprech.Text = .RAnAnsprechpartner
                txtAnTelefon.Text = .RAnTelefon
                txtAnHandy.Text = .RAnHandy
                txtAnMail.Text = .RAnMail



            End With

            '***Dropdown Abholadresse Ländercodes***
            drpAbLand = GetDropDownLand(drpAbLand)
            drpAbLand.SelectedValue = "DE"

            '***Dropdown Anlieferadresse Ländercodes***
            drpAnLand = GetDropDownLand(drpAnLand)
            drpAnLand.SelectedValue = "DE"



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
            .RLeasingnehmernummer = txtLnNummer.Text
            .RLeasingnehmerReferenz = txtLnVertragsnr.Text
            .RAnsprechLeasing = txtLnAnsprech.Text


            '***Fahrzeugnutzer***
            .RFnName = txtFnName.Text
            .RFnTelefon = txtFnTelefon.Text
            .RFnMail = txtFnMail.Text


            '***Detaildaten***
            .RFahrgestellnummer = txtFahrgestellnummer.Text
            .RFahrzeugtyp = txtLnFahrzeugtyp.Text

            .RKennzeichen = txtKennzeichen.Text
            .RBemerkung = txtBemerkung.Text

            If chkAbmeldung.Checked = True Then
                .RFahrzeugStatus = "Alt"
            Else
                .RFahrzeugStatus = "Neu"
            End If

            If .Auftragsart = Auftragsarten.Rueckfuehrung.ToString Then
                .RWunschtermin = txtWunschtermin.Text
                .RWunschterminart = rdoAuslieferung.SelectedItem.Text
            End If



            If rdbBereifung.SelectedIndex > -1 Then
                .RSomWin = rdbBereifung.SelectedItem.Value
            End If

            If rdbFahrzeugklasse.SelectedIndex > -1 Then
                .RFahrzeugklasseValue = rdbFahrzeugklasse.SelectedItem.Value
                .RFahrzeugklasseText = rdbFahrzeugklasse.SelectedItem.Text
            End If

            '***Abholadresse***
            .RAbName = txtAbName.Text
            .RAbStrasse = txtAbStrasse.Text
            .RAbPLZ = txtAbPLZ.Text
            .RAbOrt = txtAbOrt.Text
            .RAbAnsprechpartner = txtAbAnsprech.Text
            .RAbTelefon = txtAbTelefon.Text
            .RAbHandy = txtAbHandy.Text
            .RAbMail = txtAbMail.Text


            '***Anlieferadresse***

            .RAnlieferungArt = rdoAnlieferadresse.SelectedItem.Value

            .RAnName = txtAnName.Text
            .RAnStrasse = txtAnStrasse.Text
            .RAnPLZ = txtAnPLZ.Text
            .RAnOrt = txtAnOrt.Text
            .RAnAnsprechpartner = txtAnAnsprech.Text
            .RAnTelefon = txtAnTelefon.Text
            .RAnHandy = txtAnHandy.Text
            .RAnMail = txtAnMail.Text



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

        If mUeberfDAD.Auftragsart = Auftragsarten.Rueckfuehrung.ToString Then
            If Trim(txtWunschtermin.Text).Length = 0 Then
                PflichtfeldError = True
                txtWunschtermin.BorderColor = Color.Red
            End If
        End If


        If Trim(txtAbName.Text).Length = 0 Then
            PflichtfeldError = True
            txtAbName.BorderColor = Color.Red
        End If

        If Trim(txtAbStrasse.Text).Length = 0 Then
            PflichtfeldError = True
            txtAbStrasse.BorderColor = Color.Red
        End If

        If Trim(txtAbPLZ.Text).Length = 0 Then
            PflichtfeldError = True
            txtAbPLZ.BorderColor = Color.Red
        End If


        If Trim(txtAbOrt.Text).Length = 0 Then
            PflichtfeldError = True
            txtAbOrt.BorderColor = Color.Red
        End If

        If Trim(txtAbTelefon.Text).Length = 0 Then
            PflichtfeldError = True
            txtAbTelefon.BorderColor = Color.Red
        End If

        'If Trim(txtAbHandy.Text).Length = 0 Then
        '    PflichtfeldError = True
        '    txtAbHandy.BorderColor = Color.Red
        'End If

        'If Trim(txtAbMail.Text).Length = 0 Then
        '    PflichtfeldError = True
        '    txtAbMail.BorderColor = Color.Red
        'End If


        If Trim(txtAnName.Text).Length = 0 Then
            PflichtfeldError = True
            txtAnName.BorderColor = Color.Red
        End If

        If Trim(txtAnStrasse.Text).Length = 0 Then
            PflichtfeldError = True
            txtAnStrasse.BorderColor = Color.Red
        End If

        If Trim(txtAnPLZ.Text).Length = 0 Then
            PflichtfeldError = True
            txtAnPLZ.BorderColor = Color.Red
        End If


        If Trim(txtAnOrt.Text).Length = 0 Then
            PflichtfeldError = True
            txtAnOrt.BorderColor = Color.Red
        End If

        If Trim(txtAnTelefon.Text).Length = 0 Then
            PflichtfeldError = True
            txtAnTelefon.BorderColor = Color.Red
        End If

        'If Trim(txtAnHandy.Text).Length = 0 Then
        '    PflichtfeldError = True
        '    txtAnHandy.BorderColor = Color.Red
        'End If

        'If Trim(txtAnMail.Text).Length = 0 Then
        '    PflichtfeldError = True
        '    txtAnMail.BorderColor = Color.Red
        'End If

        If PflichtfeldError = True Then
            lblError.Text = ERROR_MESSAGE_PFLICHTFELD
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
            'mUeberfDAD = New UeberfDAD(mu
            mUeberfDAD = DirectCast(Session("UeberfData"), UeberfDAD)
        End If


        Select Case mUeberfDAD.Auftragsart
            Case Auftragsarten.Rueckfuehrung.ToString
                VariableAusgabe = "<td bgcolor=""#000099"" width=""650"" height=""31"" style=""text-align: center""><b>Rückführung/Anschlußfahrt<b></td>"

            Case Auftragsarten.AuslieferungRueckfuehrung.ToString
                VariableAusgabe = "<td bgcolor=""#666666"" width=""170"" height=""31"" style=""text-align: center""><b>Auslieferung<b></td>" & _
                      "<td bgcolor=""#000099"" width=""650"" height=""31"" style=""text-align: center""><b>Rückführung/Anschlußfahrt<b></td>"

            Case Auftragsarten.Alles.ToString
                VariableAusgabe = "<td bgcolor=""#666666"" width=""170"" height=""31"" style=""text-align: center""><b>Zulassung<b></td>" & _
                "<td bgcolor=""#666666"" width=""650"" height=""31"" style=""text-align: center""><b>Versand Schein u. Schilder<b></td>" & _
                                 "<td bgcolor=""#666666"" width=""170"" height=""31"" style=""text-align: center""><b>Auslieferung<b></td>" & _
                                 "<td bgcolor=""#000099"" width=""650"" height=""31"" style=""text-align: center""><b>Rückführung/Anschlußfahrt<b></td>"

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
' $History: UeberfDADEinRueck.aspx.vb $
' 
' *****************  Version 17  *****************
' User: Jungj        Date: 10.12.08   Time: 11:24
' Updated in $/CKAG/Applications/AppUeberf/Forms
' ITA 2463 testfertig
' 
' *****************  Version 16  *****************
' User: Fassbenders  Date: 2.12.08    Time: 13:42
' Updated in $/CKAG/Applications/AppUeberf/Forms
' 
' *****************  Version 15  *****************
' User: Jungj        Date: 7.11.08    Time: 11:25
' Updated in $/CKAG/Applications/AppUeberf/Forms
' ITA 2374 nachbesserung
' 
' *****************  Version 14  *****************
' User: Jungj        Date: 6.11.08    Time: 17:42
' Updated in $/CKAG/Applications/AppUeberf/Forms
' ITA 2374 testfertig
'
' 
' ************************************************