Option Explicit On
Option Strict On

Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class UeberfDADStart
    Inherits System.Web.UI.Page

#Region "Declarations"
    Private Const ERROR_MESSAGE_PFLICHTFELD As String = "Bitte füllen Sie alle Pflichfelder(*) aus."

    Protected WithEvents ucStyles As CKG.Portal.PageElements.Styles
    Protected WithEvents ucHeader As CKG.Portal.PageElements.Header

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private mUeberfDAD As UeberfDAD
    Private mError As Boolean

    '***Auftragsart****
    Private Enum Auftragsarten
        Zulassung = 1
        Auslieferung = 2
        ZulassungAuslieferung = 3
        AuslieferungRueckfuehrung = 4
        Rueckfuehrung = 5
        Alles = 6
    End Enum


#End Region


#Region "Events"
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If IsPostBack = False Then
                If IsNothing(Session("UeberfData")) = False Then
                    Session("UeberfData") = Nothing
                End If
            End If

            'Aufräumen
            ClearControls()

            SetMenu()

            mError = False

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try

    End Sub

    Protected Sub lbtWeiter_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtWeiter.Click



        'Beauftragungsart gewählt?
        If chkZul.Checked = False AndAlso chkUeberf.Checked = False AndAlso chkRueck.Checked = False Then
            lblError.Text = "Bitte wählen Sie eine Beauftragungsart aus."
            Exit Sub
        End If

        'Pflichfelder prüfen
        If PflichtfelderValidieren() = False Then
            'Leasingnehmerdaten vorbelegen
            LeasingnehmerUndVetragsdatenVorbelegen()

            If mError = True Then Exit Sub

            If IsNothing(Session("UeberfData")) = False Then
                Dim GetUeberf As UeberfDAD
                GetUeberf = DirectCast(Session("UeberfData"), UeberfDAD)

                'Je nach gewählter Auftragsart auf die entsprechende Seite weiterleiten
                Select Case GetUeberf.Auftragsart
                    'Zur Zulassung
                    Case Auftragsarten.Zulassung.ToString, _
                            Auftragsarten.ZulassungAuslieferung.ToString, _
                            Auftragsarten.Alles.ToString

                        Response.Redirect("UeberfDADZul.aspx?AppID=" & Session("AppID").ToString)
                        'Zur Auslieferung
                    Case Auftragsarten.Auslieferung.ToString, Auftragsarten.AuslieferungRueckfuehrung.ToString
                        Response.Redirect("UeberfDADAus.aspx?AppID=" & Session("AppID").ToString)
                        'Zur Rückführung/Anschlussfahrt
                    Case Auftragsarten.Rueckfuehrung.ToString
                        Response.Redirect("UeberfDADEinRueck.aspx?AppID=" & Session("AppID").ToString)
                End Select

            End If

        End If
    End Sub

    Protected Sub lbtBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtBack.Click
        Response.Redirect("../../../Start/Selection.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Protected Sub chkRueck_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkRueck.CheckedChanged
        'Voraussetzungen für die Auswahl Rücklieferung prüfen
        RuecklieferungValidieren()

    End Sub

    Protected Sub chkUeberf_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkUeberf.CheckedChanged
        'Voraussetzungen für die Auswahl Rücklieferung prüfen
        RuecklieferungValidieren()
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
    ' Methode: ClearControls
    ' Autor: SFa
    ' Beschreibung: Initialsetzung von Controls nach jedem PageLoad
    ' Erstellt am: 22.08.2008
    ' ITA: 2150
    '----------------------------------------------------------------------
    Private Sub ClearControls()
        lblError.Text = vbNullString
        lbtWeiter.Enabled = True
        txtReferenzNr.BorderColor = Color.Empty
        txtReferenzNrRuecktour.BorderColor = Color.Empty
        txtBuchungscode.BorderColor = Color.Empty

    End Sub

    '----------------------------------------------------------------------
    ' Methode: RuecklieferungValidieren
    ' Autor: SFa
    ' Beschreibung: Wird Rücklieferung ausgewählt, muss überprüft werden,
    '               ob auch Überführung angehakt wurde.
    ' Erstellt am: 22.08.2008
    ' ITA: 2150
    '----------------------------------------------------------------------
    Private Sub RuecklieferungValidieren()

        'Rückführung ausgewählt: Weitere Controls für die Rückführung einblenden.
        If chkRueck.Checked = True AndAlso chkUeberf.Checked = True Then
            lblReferenz.Visible = True
            txtReferenzNr.Visible = True

            lblReferenzNrRuecktour.Visible = True
            txtReferenzNrRuecktour.Visible = True
        ElseIf chkRueck.Checked = True And chkUeberf.Checked = False Then
            lblReferenzNrRuecktour.Visible = True
            txtReferenzNrRuecktour.Visible = True
            lblReferenz.Visible = False
            txtReferenzNr.Visible = False
            txtReferenzNr.Text = String.Empty
        Else
            lblReferenzNrRuecktour.Visible = False
            txtReferenzNrRuecktour.Visible = False

            txtReferenzNrRuecktour.Text = String.Empty

            lblReferenz.Visible = True
            txtReferenzNr.Visible = True

        End If

        'Bei einer Rückführung muss zunächst Überführung ausgewählt worden sein.
        If Me.chkRueck.Checked = True AndAlso Me.chkZul.Checked = True And Me.chkUeberf.Checked = False Then
            lblError.Text = "Die Auswahl Zulassung/Rückführung/Anschlussfahrt ist nur in Kombination mit Auslieferung möglich."
            lbtWeiter.Enabled = False
            Exit Sub
        End If

    End Sub

    '----------------------------------------------------------------------
    ' Methode:      PflichtfelderValidieren
    ' Autor:        SFa
    ' Beschreibung: Überprüfen, ob die Pflichtfelder korrekt gefüllt wurden.
    ' Erstellt am:  22.08.2008
    ' ITA:          2150
    '----------------------------------------------------------------------
    Private Function PflichtfelderValidieren() As Boolean
        Dim PflichtfeldError As Boolean

        'Wurden alle Pflichtfelder korrekt gefüllt?
        If chkRueck.Checked = True And chkUeberf.Checked = True Then

            If Trim(txtReferenzNrRuecktour.Text).Length = 0 Then
                PflichtfeldError = True
                txtReferenzNrRuecktour.BorderColor = Color.Red
            End If

            If Trim(txtReferenzNr.Text).Length = 0 Then
                PflichtfeldError = True
                txtReferenzNr.BorderColor = Color.Red
            End If

            If tr_Buchungscode.Visible = True Then
                If Trim(txtBuchungscode.Text).Length = 0 Then
                    PflichtfeldError = True
                    txtBuchungscode.BorderColor = Color.Red
                End If

            End If

           

        ElseIf chkRueck.Checked = True And chkUeberf.Checked = False Then
            If Trim(txtReferenzNrRuecktour.Text).Length = 0 Then
                PflichtfeldError = True
                txtReferenzNrRuecktour.BorderColor = Color.Red
            End If

        ElseIf chkUeberf.Checked = True And chkRueck.Checked = False Then
            If Trim(txtReferenzNr.Text).Length = 0 Then
                PflichtfeldError = True
                txtReferenzNr.BorderColor = Color.Red
            End If

            If tr_Buchungscode.Visible = True Then
                If Trim(txtBuchungscode.Text).Length = 0 Then
                    PflichtfeldError = True
                    txtBuchungscode.BorderColor = Color.Red
                End If

            End If


        Else
            If Trim(txtReferenzNr.Text).Length = 0 Then
                PflichtfeldError = True
                txtReferenzNr.BorderColor = Color.Red
            End If

            If tr_Buchungscode.Visible = True Then
                If Trim(txtBuchungscode.Text).Length = 0 Then
                    PflichtfeldError = True
                    txtBuchungscode.BorderColor = Color.Red
                End If

            End If

        End If

        If PflichtfeldError = True Then
            lblError.Text = ERROR_MESSAGE_PFLICHTFELD
            Return PflichtfeldError
        End If


    End Function

    '----------------------------------------------------------------------
    ' Methode:      LeasingnehmerUndVetragsdatenVorbelegen
    ' Autor:        SFa
    ' Beschreibung: Anhand der Referenz-Nr. die Adressfelder vorbelegen
    ' Erstellt am:  26.08.2008
    ' ITA:          2150
    '----------------------------------------------------------------------
    Private Sub LeasingnehmerUndVetragsdatenVorbelegen()


        mUeberfDAD = DirectCast(Session("UeberfData"), UeberfDAD)

        'Leasingnehmer- und Vertragsdaten holen
        mUeberfDAD.GetVertragsdaten(txtReferenzNr.Text, txtReferenzNrRuecktour.Text)

        'Equipmentdaten abfragen
        Dim TempTable As DataTable

        If txtReferenzNr.Text <> String.Empty Then
            TempTable = mUeberfDAD.GetEquiDaten(txtReferenzNr.Text)

            If IsNothing(TempTable) = False Then
                If TempTable.Rows.Count > 0 Then
                    Select Case mUeberfDAD.Auftragsart
                        Case Auftragsarten.Zulassung.ToString, Auftragsarten.ZulassungAuslieferung.ToString, _
                             Auftragsarten.Alles.ToString

                            If Len(TempTable.Rows(0)("LICENSE_NUM").ToString) > 0 AndAlso Len(TempTable.Rows(0)("REPLA_DATE").ToString) > 0 Then
                                lblError.Text = "Fahrzeug bereits zugelassen."
                                mError = True
                                Exit Sub
                            Else
                                mUeberfDAD.LnFahrgestellnummer = TempTable.Rows(0)("CHASSIS_NUM").ToString
                                mUeberfDAD.Briefnummer = TempTable.Rows(0)("TIDNR").ToString
                                mUeberfDAD.EquiNr = TempTable.Rows(0)("EQUNR").ToString
                            End If

                        Case Auftragsarten.Auslieferung.ToString, Auftragsarten.AuslieferungRueckfuehrung.ToString
                            mUeberfDAD.Kennzeichen = TempTable.Rows(0)("LICENSE_NUM").ToString
                            mUeberfDAD.LnFahrgestellnummer = TempTable.Rows(0)("CHASSIS_NUM").ToString
                            mUeberfDAD.Fahrzeugtyp = TempTable.Rows(0)("ZZHANDELSNAME").ToString
                    End Select
                End If

                TempTable.Dispose()
                TempTable = Nothing

            End If
        End If




        'Equidaten für die Rückführung
        Select Case mUeberfDAD.Auftragsart
            Case Auftragsarten.Alles.ToString, _
                    Auftragsarten.AuslieferungRueckfuehrung.ToString, _
                    Auftragsarten.Rueckfuehrung.ToString

                TempTable = mUeberfDAD.GetEquiDaten(txtReferenzNrRuecktour.Text)

                If IsNothing(TempTable) = False Then
                    If TempTable.Rows.Count > 0 Then
                        mUeberfDAD.RKennzeichen = TempTable.Rows(0)("LICENSE_NUM").ToString
                        mUeberfDAD.RFahrgestellnummer = TempTable.Rows(0)("CHASSIS_NUM").ToString
                        mUeberfDAD.RFahrzeugtyp = TempTable.Rows(0)("ZZHANDELSNAME").ToString
                    End If

                    TempTable.Dispose()
                    TempTable = Nothing

                End If

        End Select



        With mUeberfDAD
            .LeasingnehmerReferenz = txtReferenzNr.Text
            .RLeasingnehmerReferenz = txtReferenzNrRuecktour.Text
            If tr_Buchungscode.Visible = True Then

                .Buchungscode = txtBuchungscode.Text

            End If

        End With

        'Ländertabelle füllen
        mUeberfDAD.GetLaender()


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


        If IsNothing(Session("UeberfData")) = True Then
            mUeberfDAD = New UeberfDAD(m_User, m_App, "")
            Session.Add("UeberfData", mUeberfDAD)
            mUeberfDAD = DirectCast(Session("UeberfData"), UeberfDAD)
        Else
            If mUeberfDAD Is Nothing Then
                mUeberfDAD = DirectCast(Session("UeberfData"), UeberfDAD)
            End If
        End If


        'Nur Zulassung gewählt
        If chkZul.Checked = True And chkUeberf.Checked = False Then
            VariableAusgabe = "<td bgcolor=""#666666"" width=""170"" height=""31"" style=""text-align: center""><b>Zulassung<b></td>" & _
            "<td bgcolor=""#666666"" width=""600"" height=""31"" style=""text-align: center""><b>Versand Schein u. Schilder<b></td>"
            mUeberfDAD.Auftragsart = Auftragsarten.Zulassung.ToString
            'Nur Auslieferung gewählt
        ElseIf chkZul.Checked = False AndAlso chkRueck.Checked = False And chkUeberf.Checked = True Then
            VariableAusgabe = "<td bgcolor=""#666666"" width=""170"" height=""31"" style=""text-align: center""><b>Auslieferung<b></td>"
            mUeberfDAD.Auftragsart = Auftragsarten.Auslieferung.ToString
            'Zulassung und Auslieferung gewählt
        ElseIf chkZul.Checked = True AndAlso chkUeberf.Checked = True And chkRueck.Checked = False Then
            VariableAusgabe = "<td bgcolor=""#666666"" width=""170"" height=""31"" style=""text-align: center""><b>Zulassung<b></td>" & _
                              "<td bgcolor=""#666666"" width=""600"" height=""31"" style=""text-align: center""><b>Versand Schein u. Schilder<b></td>" & _
                              "<td bgcolor=""#666666"" width=""170"" height=""31"" style=""text-align: center""><b>Auslieferung<b></td>"
            mUeberfDAD.Auftragsart = Auftragsarten.ZulassungAuslieferung.ToString
            'Auslieferung und Rückführung gewählt
        ElseIf chkUeberf.Checked = True AndAlso chkRueck.Checked = True And chkZul.Checked = False Then
            VariableAusgabe = "<td bgcolor=""#666666"" width=""170"" height=""31"" style=""text-align: center""><b>Auslieferung<b></td>" & _
                             "<td bgcolor=""#666666"" width=""400"" height=""31"" style=""text-align: center""><b>Rückführung/Anschlußfahrt<b></td>"
            mUeberfDAD.Auftragsart = Auftragsarten.AuslieferungRueckfuehrung.ToString
            'Rückführung gewählt
        ElseIf chkUeberf.Checked = False AndAlso chkZul.Checked = False And chkRueck.Checked = True Then
            VariableAusgabe = "<td bgcolor=""#666666"" width=""650"" height=""31"" style=""text-align: center""><b>Rückführung/Anschlußfahrt<b></td>"
            mUeberfDAD.Auftragsart = Auftragsarten.Rueckfuehrung.ToString
        ElseIf chkUeberf.Checked = True AndAlso chkZul.Checked = True And chkRueck.Checked = True Then
            'Alles
            VariableAusgabe = "<td bgcolor=""#666666"" width=""170"" height=""31"" style=""text-align: center""><b>Zulassung<b></td>" & _
                             "<td bgcolor=""#666666"" width=""650"" height=""31"" style=""text-align: center""><b>Versand Schein u. Schilder<b></td>" & _
                             "<td bgcolor=""#666666"" width=""170"" height=""31"" style=""text-align: center""><b>Auslieferung<b></td>" & _
                             "<td bgcolor=""#666666"" width=""650"" height=""31"" style=""text-align: center""><b>Rückführung/Anschlußfahrt<b></td>"
            mUeberfDAD.Auftragsart = Auftragsarten.Alles.ToString
        End If

        Ausgabe = "<table style=""width: 620px; color: #FFFFFF;"">" & _
         "<tr>" & _
         "<td bgcolor=""#000099"" width=""96"" height=""31"" style=""text-align: center""><b>Start<b></td>"

        Ausgabe = Ausgabe & VariableAusgabe

        Ausgabe = Ausgabe & "<td bgcolor=""#666666"" width=""250"" height=""31"" style=""text-align: center""><b>Übersicht<b></td></tr></table>"

        ltAnzeige.Text = Ausgabe


    End Sub


#End Region

End Class

'$History: UeberfDADStart.aspx.vb $