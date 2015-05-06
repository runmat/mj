Imports System
Imports KBS.KBS_BASE
Imports TimeRegistration

Public Class Zeiterfassung
    Inherits Page

#Region "Enumeratoren"

    Private Enum ViewStatus
        Bedienerkarte
        Zeiterfassung
    End Enum

    Private Enum PopUpType
        OK_Abbrechen
        Ja_Nein
        Weiter_Zurück
    End Enum

    Private Enum PopUpDialog
        Ruestzeit_Kommen
        Ruestzeit_Gehen
    End Enum

#End Region

    Private mObjKasse As Kasse
    Private TimeReg As TimeRegistrator
    Private strBedienernummer As String
    
    Private mpKBS As KBS

    Protected WithEvents btnOKmaster As LinkButton
    Protected WithEvents btnCancelmaster As LinkButton

    Protected Sub Page_Init() Handles Me.Init
        'PopUp aus Masterpage laden
        btnOKmaster = Master.FindControl("btnPanelErrorOk")
        btnCancelmaster = Master.FindControl("btnPanelErrorCancel")
        mpKBS = Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        FormAuth(Me)
        If Not Session("mKasse") Is Nothing Then
            mObjKasse = Session("mKasse")
        End If

        lblError.Text = ""
        Title = lblHead.Text

        ' SAP Zeit holen
        lblServerzeit.Text = TimeRegistrator.getServerzeit()

        If Not IsPostBack Then
            If TypeOf (Session("LastPage")) Is ÜbersichtZeiten Then
                If Not Session("TimeReg") Is Nothing Then
                    TimeReg = Session("TimeReg")
                    ViewControl(ViewStatus.Zeiterfassung)
                End If
            Else
                ViewControl(ViewStatus.Bedienerkarte)
            End If
        Else
            Dim eventArg As String = Request("__EVENTARGUMENT")
            If eventArg = "MyCustomArgument" Then
                txtBedienerkarte_TextChanged()
            End If

            If Not Session("TimeReg") Is Nothing Then
                TimeReg = Session("TimeReg")
                ViewControl(ViewStatus.Zeiterfassung)
            Else
                ViewControl(ViewStatus.Bedienerkarte)
            End If

        End If

        txtBedienerkarte.Attributes.Add("onkeyup", "javascript:ControlField(this);")
        Session("LastPage") = Me
    End Sub

    Private Sub ShowPopUp(ByVal HeaderText As String, ByVal Text As String, Optional ByVal TextType As MessageType = MessageType.FlatText)
        mpKBS.ShowErrorPopUp(HeaderText, Text, TextType)
    End Sub

    Protected Sub btnOK_Click() Handles btnOKmaster.Click
        Session("bPanelCancel") = False
        Session("bPanelOK") = True
    End Sub

    Protected Sub btnCancel_Click() Handles btnCancelmaster.Click
        Session("bPanelCancel") = True
        Session("bPanelOK") = False
    End Sub

    Private Function CheckBedienerKarte() As Boolean

        If txtBedienerkarte.Text = String.Empty Then
            lblBedienError.Text = "Bitte lesen Sie die Bedienerkarte ein!"
            Return False
        ElseIf txtBedienerkarte.Text.Length <> 15 Then
            lblBedienError.Text = "Fehler beim einlesen der Bedienerkarte. Barcode hat die falsche Länge!"
            Return False
        Else
            Try
                Dim strCode As String
                Dim strBediener As String
                strCode = Left(txtBedienerkarte.Text, 14)
                strCode = Right(strCode, 13)
                strBediener = strCode.Substring(3, 1)
                strBediener &= strCode.Substring(6, 1)
                strBediener &= strCode.Substring(8, 1)
                strBediener &= strCode.Substring(11, 1)
                strBedienernummer = strBediener
                Return True
            Catch ex As Exception
                lblBedienError.Text = "Fehler beim einlesen der Bedienerkarte. Versuchen Sie es nochmal!"
                Return False

            End Try

        End If

    End Function

    Public Sub txtBedienerkarte_TextChanged()
        If CheckBedienerKarte() Then
            Dim tru As TimeRegUser
            Try
                tru = New TimeRegUser(strBedienernummer)
            Catch ex As Exception
                ShowPopUp("Fehler!", "TimRegUser-Objekt konnte nicht erzeugt werden.", MessageType.ErrorText)
                Exit Sub
            End Try

            If Not tru.ErrorOccured Then
                Session("TimeRegUser") = tru

                Try
                    TimeReg = New TimeRegistrator(CType(Session("TimeRegUser"), TimeRegUser), mObjKasse.Lagerort)
                Catch ex As Exception
                    ShowPopUp("Fehler!", "TimeRegistrator-Objekt konnte nicht erzeugt werden.", MessageType.ErrorText)
                    Exit Sub
                End Try

                If Not TimeReg.ErrorOccured Then
                    Session("TimeReg") = TimeReg
                    lblUsername.Text = tru.Username
                Else
                    ShowPopUp("Fehler!", TimeReg.ErrorMessage, MessageType.ErrorText)
                End If
            Else
                ShowPopUp("Fehler!", tru.ErrorMessage, MessageType.ErrorText)
            End If
        End If
    End Sub

    Protected Sub btnKommen_Click(sender As Object, e As EventArgs) Handles btnKommen.Click
        ' Session Wert muss for AskForRuestZeit gesetzt werden, damit DoSubmit() in der Sub ShowRequestPopUp() korrekt aufgerufen werden kann
        Session("ViewMode") = PopUpDialog.Ruestzeit_Kommen
        AskForRuestZeit(TimeRegistrator.TimeAction.Kommen)
    End Sub

    Protected Sub btnGehen_Click(sender As Object, e As EventArgs) Handles btnGehen.Click
        ' Session Wert muss for AskForRuestZeit gesetzt werden, damit DoSubmit() in der Sub ShowRequestPopUp() korrekt aufgerufen werden kann
        Session("ViewMode") = PopUpDialog.Ruestzeit_Gehen
        AskForRuestZeit(TimeRegistrator.TimeAction.Gehen)
    End Sub

    ''' <summary>
    ''' Ansichtssteuerung für alle Elemente der Seite
    ''' </summary>
    ''' <param name="VS">Das anzuzeigende Seiten-Layout</param>
    ''' <remarks></remarks>
    Private Sub ViewControl(ByVal VS As ViewStatus)
        Select Case VS
            Case ViewStatus.Bedienerkarte
                tblBedienerkarte.Visible = True
                Zeiterfassung.Visible = False
                Options.Visible = False
                txtBedienerkarte.Focus()
            Case ViewStatus.Zeiterfassung
                tblBedienerkarte.Visible = False
                Zeiterfassung.Visible = True
                Options.Visible = True

                lblUsername.Text = TimeReg.User.Username

        End Select

    End Sub

    Private Function AskForRuestZeit(timeAction As TimeRegistrator.TimeAction) As Boolean
        Dim bRuest As Boolean = False

        If TimeReg Is Nothing Then
            If Not Session("TimeReg") Is Nothing Then
                TimeReg = CType(Session("TimeReg"), TimeRegistrator)
            Else
                If Not Session("TimeRegUser") Is Nothing Then
                    TimeReg = New TimeRegistrator(CType(Session("TimeRegUser"), TimeRegUser), mObjKasse.Lagerort)
                Else
                    If strBedienernummer IsNot Nothing Or strBedienernummer <> "" Then
                        Dim TiReUs As New TimeRegUser(strBedienernummer)
                        TimeReg = New TimeRegistrator(TiReUs, mObjKasse.Lagerort)
                        Session("TimeReg") = TimeReg
                        Session("TimeRegUser") = TiReUs
                    Else
                        Session("TimeReg") = Nothing
                        Session("TimeRegUser") = Nothing
                        ViewControl(ViewStatus.Bedienerkarte)
                        ShowPopUp("Fehler!", "Ihre Zeit konnte nicht erfolgreich übermittelt werden. Bitte versuchen Sie es erneut!", MessageType.ErrorText)
                        Return False
                    End If
                End If
            End If
        End If

        If TimeReg.GetLastAction = timeAction Then
            Dim row As DataRow = TimeReg.GetLastActionRow()

            If CInt(row("BUZEIT")) + 1000 > CInt(TimeRegistrator.getServerzeit(True)) Then
                If timeAction = TimeRegistrator.TimeAction.Kommen Then
                    ShowPopUp("Kommen-Zeit kann nicht gebucht werden!", "Für den heutigen Tag wurde bereits eine Kommen-Zeit erfasst,<br/>" &
                                "diese finden Sie in der Übersicht Stempelzeiten.<br/><br/>" &
                                "Dieser Vorgang wird abgebrochen.", MessageType.NotificationText)
                ElseIf timeAction = TimeRegistrator.TimeAction.Gehen Then
                    ShowPopUp("Gehen-Zeit kann nicht gebucht werden!", "Für den heutigen Tag wurde bereits eine Gehen-Zeit erfasst,<br/>" &
                                "diese finden Sie in der Übersicht Stempelzeiten.<br/><br/>" &
                                "Dieser Vorgang wird abgebrochen.", MessageType.NotificationText)
                End If
            Else
                TimeReg.getRuestzeiten(mObjKasse.Lagerort)
                Select Case timeAction
                    Case TimeRegistrator.TimeAction.Kommen
                        ShowRequestPopUp("Rüstzeit?", "Soll für diese Buchung eine Rüstzeit vergeben werden?", MessageType.FlatText, PopUpType.Weiter_Zurück, PopUpDialog.Ruestzeit_Kommen)
                    Case TimeRegistrator.TimeAction.Gehen
                        ShowRequestPopUp("Rüstzeit?", "Soll für diese Buchung eine Rüstzeit vergeben werden?", MessageType.FlatText, PopUpType.Weiter_Zurück, PopUpDialog.Ruestzeit_Gehen)
                End Select
            End If
        Else
            TimeReg.getRuestzeiten(mObjKasse.Lagerort)
            Select Case timeAction
                Case TimeRegistrator.TimeAction.Kommen
                    ShowRequestPopUp("Rüstzeit?", "Soll für diese Buchung eine Rüstzeit vergeben werden?", MessageType.FlatText, PopUpType.Weiter_Zurück, PopUpDialog.Ruestzeit_Kommen)
                Case TimeRegistrator.TimeAction.Gehen
                    ShowRequestPopUp("Rüstzeit?", "Soll für diese Buchung eine Rüstzeit vergeben werden?", MessageType.FlatText, PopUpType.Weiter_Zurück, PopUpDialog.Ruestzeit_Gehen)
            End Select
        End If

        Return bRuest
    End Function

    Private Sub ShowRequestPopUp(ByVal Headline As String, ByVal Text As String, ByVal TextType As MessageType, ByVal PUT As PopUpType, ByVal PUD As PopUpDialog)
        lblPanelQuestionHeadline.Text = Headline
        lblPanelQuestion.Text = Text

        Select Case TextType
            Case MessageType.FlatText
                'ShowElements
                btnPanelQuestionCancel.Visible = True

                'ElementsLook
                lblPanelQuestionHeadline.ForeColor = Drawing.Color.Black
            Case MessageType.ErrorText
                'ShowElements
                btnPanelQuestionCancel.Visible = True

                'ElementsLook
                lblPanelQuestionHeadline.ForeColor = Drawing.Color.Red
            Case MessageType.NotificationText
                'ShowElements
                btnPanelQuestionCancel.Visible = False

                'ElementsLook
                lblPanelQuestionHeadline.ForeColor = Drawing.Color.DarkGoldenrod
        End Select


        Select Case PUT
            Case PopUpType.Ja_Nein
                btnPanelQuestionOK.Text = "Ja"
                btnPanelQuestionCancel.Text = "Nein"
            Case PopUpType.OK_Abbrechen
                btnPanelQuestionOK.Text = "OK"
                btnPanelQuestionCancel.Text = "Abbruch"
            Case PopUpType.Weiter_Zurück
                btnPanelQuestionOK.Text = "Weiter"
                btnPanelQuestionCancel.Text = "Zurück"
        End Select

        Select Case PUD
            Case PopUpDialog.Ruestzeit_Kommen
                ' Wenn keine Rüstzeiten möglich sind Popup nicht anzeigen und Daten direkt senden
                If TimeReg.CanDoÖffnung = False Then
                    DoSubmit()
                    Exit Sub
                End If

                ' Panel für Auswahl einblenden
                trPopSelectionPanel1.Visible = True
                ' Abstandshalter einblenden
                trPopSelectionPanel2.Visible = True


                trOeffnung.Visible = True
                trAbrechnung.Visible = False
                trEinzahlung.Visible = False
                chkOeffnung.Enabled = TimeReg.CanDoÖffnung
                chkAbrechnung.Enabled = False
                chkEinzahlung.Enabled = False

                chkOeffnung.Checked = False
                chkAbrechnung.Checked = False
                chkEinzahlung.Checked = False
            Case PopUpDialog.Ruestzeit_Gehen
                ' Wenn keine Rüstzeiten möglich sind Popup nicht anzeigen und Daten direkt senden
                If TimeReg.CanDoAbrechnung = False And TimeReg.CanDoEinzahlung = False Then
                    DoSubmit()
                    Exit Sub
                End If

                ' Panel für Auswahl einblenden
                trPopSelectionPanel1.Visible = True
                ' Abstandshalter einblenden
                trPopSelectionPanel2.Visible = True

                trOeffnung.Visible = False
                trAbrechnung.Visible = True
                trEinzahlung.Visible = True
                chkOeffnung.Enabled = False
                chkAbrechnung.Enabled = TimeReg.CanDoAbrechnung
                chkEinzahlung.Enabled = TimeReg.CanDoEinzahlung

                chkOeffnung.Checked = False
                chkAbrechnung.Checked = False
                chkEinzahlung.Checked = False
            Case Else
                ' Panel für Auswahl ausblenden
                trPopSelectionPanel1.Visible = False
                ' Abstandshalter ausblenden
                trPopSelectionPanel2.Visible = False

                trOeffnung.Visible = False
                trAbrechnung.Visible = False
                trEinzahlung.Visible = False
                chkOeffnung.Enabled = False
                chkAbrechnung.Enabled = False
                chkEinzahlung.Enabled = False

                chkOeffnung.Checked = False
                chkAbrechnung.Checked = False
                chkEinzahlung.Checked = False
        End Select
        mpeQuestion.Show()
    End Sub

    ''' <summary>
    ''' Prüft die im Dialog selektierten Felder für die Rüstzeitvergabe und übersetzt sie in den entsprechenden Rüstschlüssel
    ''' </summary>
    ''' <returns>Rüstschlüssel als <c>TimeRegistrator.TimeRuest</c>-Enum</returns>
    ''' <remarks></remarks>
    Private Function CheckRuestzeitSelected() As TimeRegistrator.TimeRuest
        If Session("ViewMode") Is Nothing Then
            Return TimeRegistrator.TimeRuest.KeinSchlüssel
        Else
            If Session("ViewMode") = PopUpDialog.Ruestzeit_Kommen Then
                If chkOeffnung.Checked Then
                    Return TimeRegistrator.TimeRuest.Kommen
                Else
                    Return TimeRegistrator.TimeRuest.KeinSchlüssel
                End If
            ElseIf Session("ViewMode") = PopUpDialog.Ruestzeit_Gehen Then
                If chkAbrechnung.Checked Then
                    If chkEinzahlung.Checked Then
                        Return TimeRegistrator.TimeRuest.Abrechnung_Einzahlung
                    Else
                        Return TimeRegistrator.TimeRuest.Abrechnung
                    End If
                ElseIf chkEinzahlung.Checked Then
                    Return TimeRegistrator.TimeRuest.Einzahlung
                Else
                    Return TimeRegistrator.TimeRuest.KeinSchlüssel
                End If
            Else
                Return TimeRegistrator.TimeRuest.KeinSchlüssel
            End If
        End If
    End Function

    ''' <summary>
    ''' Sendet die aktuell erfasste Zeit an SAP
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DoSubmit()
        Dim LastCheckpoint As String = "DoSubmit"
        Try
            If Not Session("ViewMode") Is Nothing Then
                If Session("ViewMode") = PopUpDialog.Ruestzeit_Kommen Then
                    LastCheckpoint = "TimeReg.doStampTime Kommen"
                    Dim strStampTime As String = TimeReg.doStampTime(TimeRegistrator.TimeAction.Kommen, CheckRuestzeitSelected())
                    If TimeReg.ErrorOccured Then
                        LastCheckpoint = "TimeReg.ErrorOccured Kommen"
                        Dim ErrorText As String

                        If Not TimeReg.ErrorTable Is Nothing Then
                            Dim highestErrorState As MessageType = MessageType.FlatText
                            Dim SB As StringBuilder = New StringBuilder()

                            For Each row As DataRow In TimeReg.ErrorTable.Rows
                                'höchsten Fehlerlevel ermitteln
                                If highestErrorState <> MessageType.ErrorText Then
                                    Select Case row("MSGTY").ToString.ToUpper
                                        Case "E" 'Error
                                            highestErrorState = MessageType.ErrorText
                                        Case "I" 'Information
                                            highestErrorState = MessageType.NotificationText
                                    End Select
                                End If

                                SB.AppendLine(row(0).ToString() & ": " & row(1).ToString() & "| " & row(2).ToString() & "<br />")
                            Next

                            ErrorText = SB.ToString()

                            Select Case highestErrorState
                                Case MessageType.ErrorText
                                    ShowPopUp("Fehler:", ErrorText, MessageType.ErrorText)
                                Case MessageType.NotificationText
                                    ShowPopUp("Hinweis:", ErrorText, MessageType.NotificationText)
                                Case Else
                                    ShowPopUp("", ErrorText, MessageType.FlatText)
                            End Select
                        Else
                            ErrorText = TimeReg.ErrorMessage
                            ShowPopUp("Fehler:", ErrorText, MessageType.ErrorText)
                        End If
                    Else
                        Dim strMeldung As String = "Es wurde für " & strStampTime & " eine Kommen-Zeit erfasst."

                        'Prüfen, ob zu bewertende KVPs vorliegen
                        Dim objKVP As New KVP()
                        objKVP.KVPLogin(mObjKasse.Lagerort, TimeReg.User.Kartennummer)
                        If Not objKVP.ErrorOccured AndAlso objKVP.ZuBewertendeKVPs > 0 Then
                            strMeldung = strMeldung & "<br/><br/>ACHTUNG: Ihre Meinung ist gefragt! Im KVP-Modul warten noch Verbesserungsvorschläge auf Ihre Bewertung!"
                        End If

                        ShowPopUp("Zeiterfassung", strMeldung, MessageType.NotificationText)
                    End If
                ElseIf Session("ViewMode") = PopUpDialog.Ruestzeit_Gehen Then
                    LastCheckpoint = "TimeReg.doStampTime Gehen"
                    Dim strStampTime As String = TimeReg.doStampTime(TimeRegistrator.TimeAction.Gehen, CheckRuestzeitSelected())
                    If TimeReg.ErrorOccured Then
                        LastCheckpoint = "TimeReg.ErrorOccured Gehen"
                        Dim ErrorText As String
                        If Not TimeReg.ErrorTable Is Nothing Then
                            Dim SB As StringBuilder = New StringBuilder()
                            For Each row As DataRow In TimeReg.ErrorTable.Rows
                                SB.AppendLine(row(0).ToString() & ": " & row(1).ToString() & "| " & row(2).ToString() & "<br />")
                            Next
                            ErrorText = SB.ToString()
                        Else
                            ErrorText = TimeReg.ErrorMessage
                        End If

                        ShowPopUp("Fehler:", ErrorText, MessageType.ErrorText)
                    Else
                        ShowPopUp("Zeiterfassung", "Es wurde für " & strStampTime & " eine Gehen-Zeit erfasst.", MessageType.NotificationText)
                    End If
                End If
            End If
        Catch ex As Exception
            ShowPopUp("Fehler:", "Fehler an der Stelle " & LastCheckpoint & ": " & ex.Message, MessageType.ErrorText)
        End Try
    End Sub

    Protected Sub btnPanelQuestionOK_Clicked(ByVal sender As Object, ByVal e As EventArgs) Handles btnPanelQuestionOK.Click
        DoSubmit()
    End Sub

    Protected Sub btnPanelQuestionCancel_Clicked(ByVal sender As Object, ByVal e As EventArgs) Handles btnPanelQuestionCancel.Click
        Session("ViewMode") = Nothing
    End Sub

    Protected Sub timServerzeit_Tick(sender As Object, e As EventArgs) Handles timServerzeit.Tick
        lblServerzeit.Text = TimeRegistrator.getServerzeit()
    End Sub

    Protected Sub btnÜbersicht_Click(sender As Object, e As EventArgs) Handles btnÜbersicht.Click
        Response.Redirect("ÜbersichtZeiten.aspx")
    End Sub

    Private Sub Zeiterfassung_Unload(sender As Object, e As EventArgs) Handles Me.Unload

    End Sub

    Protected Sub lb_zurueck_Click(sender As Object, e As EventArgs) Handles lb_zurueck.Click
        Session("TimeReg") = Nothing
        Session("TimeRegUser") = Nothing
        Response.Redirect("Zeiterfassung.aspx")
    End Sub

    Private Sub lvZeitnachweis_ItemCommand(sender As Object, e As ListViewCommandEventArgs) Handles lvZeitnachweis.ItemCommand

        Select Case e.CommandName
            Case "GetPDF"
                Dim ComArg As String = e.CommandArgument.ToString()
                Dim split As String() = ComArg.Split(" "c)
                Dim Vdate As String = split(0) + split(1).PadLeft(2, "0"c) + "01"
                Dim Bdate As String = split(0) + split(1).PadLeft(2, "0"c) + "30"

                Try
                    Session("PDFPrintObj") = New PDFPrintObj(TimeReg.User.Kartennummer, Vdate, Bdate)
                    Response.Redirect("PDFZeitnachweise.aspx", False)
                Catch ex As Exception
                    Session("PDFPrintObj") = Nothing
                End Try
        End Select
    End Sub

End Class