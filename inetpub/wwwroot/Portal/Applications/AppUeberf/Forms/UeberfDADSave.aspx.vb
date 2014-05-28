Option Explicit On
Option Strict On

Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Partial Public Class UeberfDADSave
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

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private mUeberfDAD As UeberfDAD

#End Region

#Region "Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Hier Benutzercode zur Seiteninitialisierung einfügen
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)
        If IsPostBack = False Then
            'Vorgaben laden
            FillControls()
            SetMenu()

        End If



        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

        Catch ex As Exception
            lblMessage.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Protected Sub lbtWeiter_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtWeiter.Click
        If IsNothing(Session("UeberfData")) = False Then
            mUeberfDAD = DirectCast(Session("UeberfData"), UeberfDAD)
            Try
                mUeberfDAD.Save()

                Select Case mUeberfDAD.Auftragsart
                    Case Auftragsarten.Auslieferung.ToString, Auftragsarten.Rueckfuehrung.ToString, Auftragsarten.AuslieferungRueckfuehrung.ToString
                        lblMessage.Text = "Auftrag gespeichert. Auftragsnummer: " & mUeberfDAD.AuftragsnummerUeberf
                    Case Auftragsarten.Zulassung.ToString
                        lblMessage.Text = "Auftrag gespeichert. Auftragsnummer: " & mUeberfDAD.AuftragsnummerZul
                    Case Auftragsarten.ZulassungAuslieferung.ToString, _
                            Auftragsarten.Alles.ToString
                        lblMessage.Text = "Aufträge gespeichert. Auftragsnummer Zulassung: " & mUeberfDAD.AuftragsnummerZul & _
                                          "<br>Auftragsnummer Überführung: " & mUeberfDAD.AuftragsnummerUeberf
                End Select


                lbtWeiter.Visible = False
                lbtPrint.Visible = True

            Catch ex As Exception
                lblMessage.Text = ex.Message
            End Try

        End If
    End Sub

    Protected Sub lbtBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtBack.Click
        If IsNothing(Session("UeberfData")) = False Then
            mUeberfDAD = DirectCast(Session("UeberfData"), UeberfDAD)
        End If

        If mUeberfDAD.AuftragsnummerUeberf <> String.Empty OrElse mUeberfDAD.AuftragsnummerZul <> String.Empty Then
            Response.Redirect("UeberfDADStart.aspx?AppID=" & Session("AppID").ToString)
            mUeberfDAD = Nothing
            Session("UeberfData") = Nothing
            Exit Sub
        End If

        Select Case mUeberfDAD.Auftragsart
            Case Auftragsarten.Auslieferung.ToString, _
                    Auftragsarten.ZulassungAuslieferung.ToString
                Response.Redirect("UeberfDADAus.aspx?AppID=" & Session("AppID").ToString)

            Case Auftragsarten.Zulassung.ToString
                Response.Redirect("UeberfDADVss.aspx?AppID=" & Session("AppID").ToString)
            Case Auftragsarten.AuslieferungRueckfuehrung.ToString, _
                    Auftragsarten.Rueckfuehrung.ToString, _
                    Auftragsarten.Alles.ToString
                Response.Redirect("UeberfDADEinRueck.aspx?AppID=" & Session("AppID").ToString)

        End Select
    End Sub

    Protected Sub lbtPrint_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtPrint.Click
        Try

            If IsNothing(Session("UeberfData")) = False Then
                mUeberfDAD = DirectCast(Session("UeberfData"), UeberfDAD)

                Dim imageHt As New Hashtable()
                imageHt.Add("Logo", m_User.Customer.LogoImage)

                Dim docFactory As New Base.Kernel.DocumentGeneration.WordDocumentFactory(Base.Kernel.Common.DataTableHelper.ObjectToDataTable(mUeberfDAD), imageHt)

                Select Case mUeberfDAD.Auftragsart
                    Case Auftragsarten.Zulassung.ToString
                        docFactory.CreateDocument(mUeberfDAD.LeasingnehmerReferenz + "_Zul", Me.Page, "\Applications\AppUeberf\Documents\DADZulassung.doc")
                    Case Auftragsarten.Auslieferung.ToString
                        docFactory.CreateDocument(mUeberfDAD.LeasingnehmerReferenz + "_Aus", Me.Page, "\Applications\AppUeberf\Documents\DADAuslieferung.doc")
                    Case Auftragsarten.Rueckfuehrung.ToString
                        docFactory.CreateDocument(mUeberfDAD.LeasingnehmerReferenz + "_Anschluss", Me.Page, "\Applications\AppUeberf\Documents\DADAnschluss.doc")
                    Case Auftragsarten.ZulassungAuslieferung.ToString
                        docFactory.CreateDocument(mUeberfDAD.LeasingnehmerReferenz + "_ZulAus", Me.Page, "\Applications\AppUeberf\Documents\DADZulassungAuslieferung.doc")
                    Case Auftragsarten.AuslieferungRueckfuehrung.ToString
                        docFactory.CreateDocument(mUeberfDAD.LeasingnehmerReferenz + "_AusAnschluss", Me.Page, "\Applications\AppUeberf\Documents\DADAuslieferungAnschluss.doc")
                    Case Auftragsarten.Alles.ToString
                        docFactory.CreateDocument(mUeberfDAD.LeasingnehmerReferenz + "_Gesamt", Me.Page, "\Applications\AppUeberf\Documents\DADGesamtauftrag.doc")
                End Select

            Else
                lblMessage.Text = "Drucken fehlgeschlagen."
                lbtPrint.Enabled = False
            End If

        Catch ex As Exception
            lblMessage.Text = "Fehler beim Erstellen des Ausdrucks: " + ex.Message
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
    ' Erstellt am:  27.08.2008
    ' ITA:          2150
    '----------------------------------------------------------------------
    Private Sub FillControls()

        If IsNothing(Session("UeberfData")) = False Then
            mUeberfDAD = DirectCast(Session("UeberfData"), UeberfDAD)

            With mUeberfDAD

                '***Zulassung***

                '***Leasingnehmer***
                lblLnNummer.Text = .Leasingnehmernummer
                lblLnName.Text = .Leasingnehmer
                lblLnStrasse.Text = .LeasingnehmerStrasse
                lblLnOrt.Text = .LeasingnehmerPLZ & " " & .LeasingnehmerOrt
                lblRef.Text = .LeasingnehmerReferenz
                lblBuchungscode.Text = .Buchungscode

                '***Halter***
                lblShName.Text = .Halter & " " & .Halter2
                lblShStrasse.Text = .HalterStrasse
                lblShOrt.Text = .HalterPLZ & " " & .HalterOrt

                '***Versicherungsnehmer***
                lblVersName.Text = .VersNehmer
                lblVersStrasse.Text = .VersNehmerStrasse
                lblVersOrt.Text = .VersNehmerPLZ & " " & .VersNehmerOrt
                'lblVersAnsprech.Text = .VersAnsprechpartner
                'lblVersTelefon.Text = .VersTelefon
                'lblVersMail.Text = .VersMail

                ''***Zulassung***
                lblVin.Text = .LnFahrgestellnummer
                lblHerst.Text = .Fahrzeugtyp
                lblZulDat.Text = .Zulassungsdatum
                lblBriefnr.Text = .Briefnummer
                lblTerminart.Text = .ZulTerminart
                lblResNr.Text = .ResNummer
                lblResName.Text = .ResName


                If .Wunschkennzeichen1 <> String.Empty Then
                    lblWunschkenn.Text = .Wunschkennzeichen1
                    If .Wunschkennzeichen2 <> String.Empty Then
                        lblWunschkenn.Text = lblWunschkenn.Text & ", " & .Wunschkennzeichen2
                        If .Wunschkennzeichen3 <> String.Empty Then
                            lblWunschkenn.Text = lblWunschkenn.Text & ", " & .Wunschkennzeichen3
                        End If
                    End If

                End If


                lblFeinstaub.Text = IIf(.Feinstaub = "X", "Ja", "").ToString
                If .KfzSteuer = "H" Then
                    lblSteuer.Text = "Halter"
                ElseIf .KfzSteuer = "L" Then
                    lblSteuer.Text = "Leasingnehmer"
                End If

                '***Versicherungsdaten***
                lblVersGesellschaft.Text = .VersGesellschaft
                lblEvbNr.Text = .EVBNummer
                lblEvbVon.Text = .EVBVon
                lblEvbBis.Text = .EVBBis

                '***Versand Schein u. Schilder****
                lblVssName.Text = .HaendlerName1 & " " & .HaendlerAnsprech
                lblVssStrasse.Text = .HaendlerStrasse
                lblVssOrt.Text = .HaendlerPLZ & " " & .HaendlerOrt

                '***Auslieferung***
                '***Leasingnehmer***
                lblAusLnName.Text = .Leasingnehmer
                lblAusLnNummer.Text = .Leasingnehmernummer
                lblAusLnStrasse.Text = .LeasingnehmerStrasse
                lblAusLnOrt.Text = .LeasingnehmerPLZ & " " & .LeasingnehmerOrt

                '***Fahrzeugnutzer***
                lblFnName.Text = .FnName
                lblFnTelefon.Text = .FnTelefon
                lblFnMail.Text = .FnMail

                '***Fahrzeugdaten***
                lblAusFahrzeugtyp.Text = .Fahrzeugtyp
                lblAusFin.Text = .LnFahrgestellnummer
                'lblStatus.Text = .FahrzeugStatus
                lblAusBriefnummer.Text = .Briefnummer
                lblAusKennzeichen.Text = .Kennzeichen
                lblAusBereifung.Text = .SomWin
                lblAusFzgKlasse.Text = .FahrzeugklasseText

                '***Händler***
                lblHdName.Text = .HaendlerName1
                lblHdStrasse.Text = .HaendlerStrasse
                lblHdOrt.Text = .HaendlerPLZ & " " & .HaendlerOrt
                lblHdAnsprech.Text = .HaendlerAnsprech
                lblHdTelefon.Text = .HaendlerTelefon
                lblHdTelefon2.Text = .HaendlerTelefon2
                lblHdMail.Text = .HaendlerMail

                '***Fahrzeugempfänger***
                lblFeName.Text = .EmpfName
                lblFeStrasse.Text = .EmpfStrasse
                lblFeOrt.Text = .EmpfPLZ & " " & .EmpfOrt
                lblFeAnsprech.Text = .EmpfAnsprechpartner
                lblFeTelefon.Text = .EmpfTelefon
                lblFeTelefon2.Text = .EmpfTelefon2
                lblFeMail.Text = .EmpfMail


                lblAusTerminhinweis.Text = .TerminhinweisAuslieferung

                lblWaesche.Text = .Waesche
                lblTanken.Text = .Tanken
                lblEinweisung.Text = .FzgEinweisung

                lblAusDatum.Text = .AuslieferungDatum

                If Not .WinterText = "" Then
                    lblWinterText.Text = .WinterText
                    lblWinterText.Visible = True
                    lblWinterAnzeige.Visible = True
                End If



                lblAusBemerkung.Text = .BemerkungAus

                lblAusTankkarten.Text = .Tankkarten

                lblAusServicekarte.Text = IIf(.Servicekarte = "X", "Ja", "").ToString

                '***Rückführung***
                'Leasingnehmer
                lblRLnNr.Text = .RLeasingnehmernummer
                lblRReferenz.Text = .RLeasingnehmerReferenz
                lblRAnAnsprech.Text = .RAnsprechLeasing

                'Fahrzeugnutzer
                lblRFnName.Text = .RFnName
                lblRFnTelefon.Text = .RFnTelefon
                lblRFnMail.Text = .RFnMail

                'Fahrzeugdaten
                lblRFahrzeugtyp.Text = .RFahrzeugtyp
                lblRFahrgestellnummer.Text = .RFahrgestellnummer
                lblRStatus.Text = .RFahrzeugStatus

                lblRKennzeichen.Text = .RKennzeichen
                lblRBereifung.Text = .RSomWin
                lblRFahrzeugKlasse.Text = .RFahrzeugklasseText

                lblRWunschtermin.Text = .RWunschterminart & " " & .RWunschtermin

                'Bemerkung5 füllen
                .Bemerkung5 = IIf(.HandyAdapter.Length > 0, .HandyAdapter & ", ", String.Empty).ToString
                .Bemerkung5 = .Bemerkung5 & IIf(.Verbandskasten.Length > 0, .Verbandskasten & ", ", String.Empty).ToString
                .Bemerkung5 = .Bemerkung5 & IIf(.NaviCD.Length > 0, .NaviCD & ", ", String.Empty).ToString
                .Bemerkung5 = .Bemerkung5 & IIf(.Warndreieck.Length > 0, .Warndreieck & ", ", String.Empty).ToString
                .Bemerkung5 = .Bemerkung5 & IIf(.Warnweste.Length > 0, .Warnweste & ", ", String.Empty).ToString
                .Bemerkung5 = .Bemerkung5 & IIf(.Fussmatten.Length > 0, .Fussmatten & ", ", String.Empty).ToString
                .Bemerkung5 = .Bemerkung5 & IIf(.FKontrolle.Length > 0, .FKontrolle & ", ", String.Empty).ToString

                lblWeiteres.Text = .Bemerkung5

                'Abholadresse
                lblRAbName.Text = .RAbName
                lblRAbStrasse.Text = .RAbStrasse
                lblRAbOrt.Text = .RAbPLZ & " " & .RAbOrt
                lblRAbAnsprech.Text = .RAbAnsprechpartner
                lblRAbTelefon.Text = .RAbTelefon
                lblRAbHandy.Text = .RAbHandy
                lblRAbMail.Text = .RAbMail

                'Anlieferadresse
                lblRAnName.Text = .RAnName
                lblRAnStrasse.Text = .RAnStrasse
                lblRAnOrt.Text = .RAnPLZ & " " & .RAnOrt
                lblRAnAnsprech.Text = .RAnAnsprechpartner
                lblRAnTelefon.Text = .RAnTelefon
                lblRAnHandy.Text = .RAnHandy
                lblRAnMail.Text = .RAnMail

                lblRBemerkung.Text = .RBemerkung

            End With

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
            mUeberfDAD = DirectCast(Session("UeberfData"), UeberfDAD)
        End If


        Select Case mUeberfDAD.Auftragsart
            Case Auftragsarten.Zulassung.ToString
                VariableAusgabe = "<td bgcolor=""#666666"" width=""170"" height=""31"" style=""text-align: center""><b>Zulassung<b></td>" & _
                "<td bgcolor=""#666666"" width=""600"" height=""31"" style=""text-align: center""><b>Versand Schein u. Schilder<b></td>"
                pnlZulassung.Visible = True
            Case Auftragsarten.ZulassungAuslieferung.ToString
                VariableAusgabe = "<td bgcolor=""#666666"" width=""170"" height=""31"" style=""text-align: center""><b>Zulassung<b></td>" & _
                  "<td bgcolor=""#666666"" width=""600"" height=""31"" style=""text-align: center""><b>Versand Schein u. Schilder<b></td>" & _
                  "<td bgcolor=""#666666"" width=""170"" height=""31"" style=""text-align: center""><b>Auslieferung<b></td>"
                pnlAuslieferung.Visible = True
                pnlZulassung.Visible = True
            Case Auftragsarten.Auslieferung.ToString
                VariableAusgabe = "<td bgcolor=""#666666"" width=""170"" height=""31"" style=""text-align: center""><b>Auslieferung<b></td>"
                pnlAuslieferung.Visible = True
            Case Auftragsarten.AuslieferungRueckfuehrung.ToString
                VariableAusgabe = "<td bgcolor=""#666666"" width=""170"" height=""31"" style=""text-align: center""><b>Auslieferung<b></td>" & _
                 "<td bgcolor=""#666666"" width=""400"" height=""31"" style=""text-align: center""><b>Rückführung/Anschlußfahrt<b></td>"
                pnlAuslieferung.Visible = True
                pnlRueck.Visible = True
            Case Auftragsarten.Rueckfuehrung.ToString
                VariableAusgabe = "<td bgcolor=""#666666"" width=""650"" height=""31"" style=""text-align: center""><b>Rückführung/Anschlußfahrt<b></td>"
                pnlRueck.Visible = True
            Case Auftragsarten.Alles.ToString
                VariableAusgabe = "<td bgcolor=""#666666"" width=""170"" height=""31"" style=""text-align: center""><b>Zulassung<b></td>" & _
                "<td bgcolor=""#666666"" width=""650"" height=""31"" style=""text-align: center""><b>Versand Schein u. Schilder<b></td>" & _
                                 "<td bgcolor=""#666666"" width=""170"" height=""31"" style=""text-align: center""><b>Auslieferung<b></td>" & _
                                 "<td bgcolor=""#666666"" width=""650"" height=""31"" style=""text-align: center""><b>Rückführung/Anschlußfahrt<b></td>"
                pnlAuslieferung.Visible = True
                pnlZulassung.Visible = True
                pnlRueck.Visible = True
        End Select

        Ausgabe = "<table style=""width: 620px; color: #FFFFFF;"">" & _
         "<tr>" & _
         "<td bgcolor=""#666666"" width=""96"" height=""31"" style=""text-align: center""><b>Start<b></td>"

        Ausgabe = Ausgabe & VariableAusgabe

        Ausgabe = Ausgabe & "<td bgcolor=""#000099"" width=""250"" height=""31"" style=""text-align: center""><b>Übersicht<b></td></tr></table>"

        ltAnzeige.Text = Ausgabe

    End Sub


#End Region



 

End Class
' ************************************************
' $History: UeberfDADSave.aspx.vb $
' 
' *****************  Version 15  *****************
' User: Fassbenders  Date: 7.12.10    Time: 11:32
' Updated in $/CKAG/Applications/AppUeberf/Forms
' 
' *****************  Version 14  *****************
' User: Fassbenders  Date: 11.12.08   Time: 14:39
' Updated in $/CKAG/Applications/AppUeberf/Forms
' 
' *****************  Version 13  *****************
' User: Jungj        Date: 4.11.08    Time: 9:01
' Updated in $/CKAG/Applications/AppUeberf/Forms
' ITA 2343 fertig
' 
' *****************  Version 12  *****************
' User: Jungj        Date: 3.11.08    Time: 11:15
' Updated in $/CKAG/Applications/AppUeberf/Forms
' ITA 2343 fertigstellung 
'
' ************************************************