Option Explicit On
Option Strict On

Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Partial Public Class UeberfDADVss
    Inherits System.Web.UI.Page

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

    Protected WithEvents ucStyles As CKG.Portal.PageElements.Styles
    Protected WithEvents ucHeader As CKG.Portal.PageElements.Header

    Private Const ERROR_MESSAGE_PFLICHTFELD As String = "Bitte füllen Sie alle Pflichfelder(*) aus."

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private mUeberfDAD As UeberfDAD


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
                SetMenu()
            Else
                If mUeberfDAD Is Nothing Then
                    mUeberfDAD = DirectCast(Session("UeberfData"), UeberfDAD)
                End If
                AddToSessionObject()
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Protected Sub lbtWeiter_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtWeiter.Click
        'Pflichfelder prüfen
        If PflichtfelderValidieren() = False Then

            Select Case mUeberfDAD.Auftragsart
                Case Auftragsarten.Zulassung.ToString
                    Response.Redirect("UeberfDADSave.aspx?AppID=" & Session("AppID").ToString)

                Case Else
                    Response.Redirect("UeberfDADAus.aspx?AppID=" & Session("AppID").ToString)

            End Select

        End If
    End Sub
    Protected Sub ibtSearchLN_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtSearchLN.Click
        Response.Redirect("UeberfDADAdresse.aspx?AppID=" & Session("AppID").ToString & "&Art=" & "HAENDLER" & "&Src=VSS")
    End Sub

    Protected Sub lbtBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtBack.Click
        Response.Redirect("UeberfDADZul.aspx?AppID=" & Session("AppID").ToString)
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

                lblVertragsnummer.Text = .LeasingnehmerReferenz

                '***Händler***
                txtVssName1.Text = .HaendlerName1
                txtVssName2.Text = .HaendlerAnsprech
                txtVssStrasse.Text = .HaendlerStrasse
                txtVssPLZ.Text = .HaendlerPLZ
                txtVssOrt.Text = .HaendlerOrt

            End With

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
            '***Versandadresse***
            .HaendlerName1 = txtVssName1.Text
            .HaendlerAnsprech = txtVssName2.Text
            .HaendlerStrasse = txtVssStrasse.Text
            .HaendlerPLZ = txtVssPLZ.Text
            .HaendlerOrt = txtVssOrt.Text

            .VssName = txtVssName1.Text
            .VssName2 = txtVssName2.Text
            .VssStrasse = txtVssStrasse.Text
            .VssPLZ = txtVssPLZ.Text
            .VssOrt = txtVssOrt.Text

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
        If Trim(txtVssName1.Text).Length = 0 Then
            PflichtfeldError = True
            txtVssName1.BorderColor = Color.Red
        End If

        If Trim(txtVssStrasse.Text).Length = 0 Then
            PflichtfeldError = True
            txtVssStrasse.BorderColor = Color.Red
        End If

        If Trim(txtVssPLZ.Text).Length = 0 Then
            PflichtfeldError = True
            txtVssPLZ.BorderColor = Color.Red
        End If

        If Trim(txtVssPLZ.Text).Length = 0 Then
            PflichtfeldError = True
            txtVssPLZ.BorderColor = Color.Red
        End If

        If Trim(txtVssOrt.Text).Length = 0 Then
            PflichtfeldError = True
            txtVssOrt.BorderColor = Color.Red
        End If



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
            mUeberfDAD = DirectCast(Session("UeberfData"), UeberfDAD)
        End If


        Select Case mUeberfDAD.Auftragsart
            Case Auftragsarten.Zulassung.ToString
                VariableAusgabe = "<td bgcolor=""#666666"" width=""170"" height=""31"" style=""text-align: center""><b>Zulassung<b></td>" & _
                "<td bgcolor=""#000099"" width=""600"" height=""31"" style=""text-align: center""><b>Versand Schein u. Schilder<b></td>"
            Case Auftragsarten.ZulassungAuslieferung.ToString
                VariableAusgabe = "<td bgcolor=""#666666"" width=""170"" height=""31"" style=""text-align: center""><b>Zulassung<b></td>" & _
                  "<td bgcolor=""#000099"" width=""600"" height=""31"" style=""text-align: center""><b>Versand Schein u. Schilder<b></td>" & _
                  "<td bgcolor=""#666666"" width=""170"" height=""31"" style=""text-align: center""><b>Auslieferung<b></td>"
            Case Auftragsarten.Alles.ToString
                VariableAusgabe = "<td bgcolor=""#666666"" width=""170"" height=""31"" style=""text-align: center""><b>Zulassung<b></td>" & _
                                                 "<td bgcolor=""#000099"" width=""650"" height=""31"" style=""text-align: center""><b>Versand Schein u. Schilder<b></td>" & _
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
' $History: UeberfDADVss.aspx.vb $