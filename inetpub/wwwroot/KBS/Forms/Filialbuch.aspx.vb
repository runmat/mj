Imports System
Imports KBS.KBS_BASE
Imports KBS.DigitalesFilialbuch

Public Class Filialbuch
    Inherits Page

#Region "Enumeratoren"

    Private Enum ViewStatus
        Bedienerkarte
        Gebietsleiter
        FilialeProtokoll
        FilialeAufgaben
    End Enum

#End Region

    Private mObjKasse As Kasse
    Private mObjFilialbuch As FilialbuchClass
    Private mObjLongStringToSap As LongStringToSap
    Private strBedienernummer As String
    Private curView As ViewStatus

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        FormAuth(Me)

        lblError.Text = ""
        Title = lblHead.Text

        If Not Session("mKasse") Is Nothing Then
            mObjKasse = Session("mKasse")
        End If
        If Not Session("mObjFilialbuch") Is Nothing Then
            mObjFilialbuch = Session("mObjFilialbuch")
        End If
        If Not Session("curView") Is Nothing Then
            curView = Session("curView")
        End If
        If Not Session("mObjLongStringToSap") Is Nothing Then
            mObjLongStringToSap = Session("mObjLongStringToSap")
        End If
        If Not Session("strBedienernummer") Is Nothing Then
            strBedienernummer = Session("strBedienernummer")
        End If

        If Not IsPostBack Then
            mObjFilialbuch = New FilialbuchClass()
            Session("mObjFilialbuch") = mObjFilialbuch

            curView = ViewStatus.Bedienerkarte
            Session("curView") = curView

            mObjLongStringToSap = New LongStringToSap()
            Session("mObjLongStringToSap") = mObjLongStringToSap

            Title = lblHead.Text
            lblKostenstelle.Text = mObjKasse.Lagerort
        Else
            Dim eventArg As String = Request("__EVENTARGUMENT")
            If eventArg = "MyCustomArgument" Then
                txtBedienerkarte_TextChanged()
            End If
        End If

        ViewControl(curView)

        txtBedienerkarte.Attributes.Add("onkeyup", "javascript:ControlField(this);")
        Session("LastPage") = Me
    End Sub

    ''' <summary>
    ''' Methode die aufgerufen, wird wenn sich der Text im Bedienerkartenfeld ändert.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub txtBedienerkarte_TextChanged()
        If CheckBedienerKarte() Then
            Try
                Dim FilBuUser = mObjFilialbuch.LoginUser(mObjKasse.Lagerort, strBedienernummer)
                If mObjFilialbuch.ErrorOccured Then
                    Throw New Exception(mObjFilialbuch.ErrorCode & ": " & mObjFilialbuch.ErrorMessage)
                Else
                    lblBedienernummer.Text = strBedienernummer
                    lblUser.Text = FilBuUser.Bedienername
                    Select Case FilBuUser.Rolle
                        Case FilialbuchClass.Rolle.Filiale
                            curView = ViewStatus.FilialeAufgaben
                            FillListAufgaben()
                        Case FilialbuchClass.Rolle.Gebietsleiter
                            curView = ViewStatus.Gebietsleiter
                            FillListProtokoll()
                        Case Else
                            curView = ViewStatus.Bedienerkarte
                            lblError.Text = "Der Benutzer ist keiner bekannten Rolle zugeordnet!"
                    End Select
                End If

            Catch ex As Exception
                lblError.Text = ex.Message
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Die Methode prüft, ob der Inhalt des Bedienerkartenfeldes eine gültige Bedienernummer enthält und extrahiert diese in die Variabel strBedienernummer
    ''' </summary>
    ''' <returns>True falls eine gültige Bedienernummer gefunden werden konnte sonst False</returns>
    ''' <remarks></remarks>
    Private Function CheckBedienerKarte() As Boolean
        If txtBedienerkarte.Text = String.Empty Then
            lblBedienError.Text = "Bitte lesen Sie die Bedienerkarte ein!"
            Return False
        ElseIf txtBedienerkarte.Text.Length <> 15 Then
            lblBedienError.Text = "Fehler beim Einlesen der Bedienerkarte. Barcode hat die falsche Länge!"
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
                Session("strBedienernummer") = strBedienernummer
                Return True
            Catch ex As Exception
                lblBedienError.Text = "Fehler beim Einlesen der Bedienerkarte. Versuchen Sie es nochmal!"
                Return False

            End Try

        End If

    End Function

    Private Sub FillListAufgaben()
        mObjFilialbuch.GetEinträge(mObjFilialbuch.UserLoggedIn, FilialbuchClass.StatusFilter.Neu)
        If mObjFilialbuch.ErrorOccured Then
            lblError.Text = mObjFilialbuch.ErrorCode & ": " & mObjFilialbuch.ErrorMessage
        Else
            gvAufgabenFiliale.Visible = True
            gvAllFiliale.Visible = False
            gvAllGL.Visible = False

            gvAufgabenFiliale.DataSource = mObjFilialbuch.Protokoll.CreateTable(IFilialbuchEntry.EntryStatus.Ausblenden, IFilialbuchEntry.EntryStatus.Ausblenden,
                                                                                IFilialbuchEntry.EmpfängerStatus.Neu, IFilialbuchEntry.EmpfängerStatus.Ausblenden)
            gvAufgabenFiliale.DataBind()
        End If
    End Sub

    Private Sub FillListProtokoll()
        If txtDatumVon.Text.Trim = String.Empty Then
            txtDatumVon.Text = DateAdd(DateInterval.Month, -3, Today).ToShortDateString()
        End If
        If txtDatumBis.Text.Trim = String.Empty Then
            txtDatumBis.Text = Today.ToShortDateString()
        End If

        Select curView
            Case ViewStatus.Gebietsleiter
                mObjFilialbuch.GetEinträge(mObjFilialbuch.UserLoggedIn, FilialbuchClass.StatusFilter.Alle, mObjKasse.Lagerort, CDate(txtDatumVon.Text), CDate(txtDatumBis.Text))
            Case Else
                mObjFilialbuch.GetEinträge(mObjFilialbuch.UserLoggedIn, FilialbuchClass.StatusFilter.Alle, Nothing, CDate(txtDatumVon.Text), CDate(txtDatumBis.Text))
        End Select

        If mObjFilialbuch.ErrorOccured Then
            lblError.Text = mObjFilialbuch.ErrorCode & ": " & mObjFilialbuch.ErrorMessage
        Else
            Select Case curView

                Case ViewStatus.FilialeProtokoll
                    gvAufgabenFiliale.Visible = False
                    gvAllFiliale.Visible = True
                    gvAllGL.Visible = False

                    Select Case ddlFilterFiliale.SelectedValue

                        Case "all"
                            gvAllFiliale.DataSource = mObjFilialbuch.Protokoll.CreateTable()

                        Case "E0", "E1", "E3", "E4"
                            ' "Gelesen", "Beantwortet", "Erledigt", "Neu"
                            Dim sFilter = ddlFilterFiliale.SelectedValue.ToString().Trim("E"c)
                            Dim dt As DataTable = mObjFilialbuch.Protokoll.CreateTable(IFilialbuchEntry.EntryStatus.Ausblenden, IFilialbuchEntry.EntryStatus.Ausblenden, CType(sFilter, IFilialbuchEntry.EmpfängerStatus), CType(sFilter, IFilialbuchEntry.EmpfängerStatus))

                            'Bei Auswahl "Erledigt" geschlossene nicht rausfiltern
                            If ddlFilterFiliale.SelectedValue = "E4" Then
                                gvAllFiliale.DataSource = dt
                            Else
                                gvAllFiliale.DataSource = FilterClosed(dt)
                            End If

                        Case Else
                            Dim dt As DataTable = mObjFilialbuch.Protokoll.CreateTable(CType(ddlFilter.SelectedValue, IFilialbuchEntry.EntryStatus), CType(ddlFilter.SelectedValue, IFilialbuchEntry.EntryStatus))
                            gvAllFiliale.DataSource = dt

                    End Select

                    gvAllFiliale.DataBind()

                Case ViewStatus.Gebietsleiter
                    gvAufgabenFiliale.Visible = False
                    gvAllFiliale.Visible = False
                    gvAllGL.Visible = True

                    Select Case ddlFilter.SelectedValue

                        Case "all"
                            gvAllGL.DataSource = mObjFilialbuch.Protokoll.CreateTable()

                        Case "E0", "E1", "E3", "E4"
                            ' "Gelesen", "Beantwortet", "Erledigt", "Neu"
                            Dim sFilter = ddlFilter.SelectedValue.ToString().Trim("E"c)
                            Dim dt As DataTable = mObjFilialbuch.Protokoll.CreateTable(IFilialbuchEntry.EntryStatus.Ausblenden, IFilialbuchEntry.EntryStatus.Ausblenden, CType(sFilter, IFilialbuchEntry.EmpfängerStatus), CType(sFilter, IFilialbuchEntry.EmpfängerStatus))

                            'Bei Auswahl "Erledigt" geschlossene nicht rausfiltern
                            If ddlFilter.SelectedValue = "E4" Then
                                gvAllGL.DataSource = dt
                            Else
                                gvAllGL.DataSource = FilterClosed(dt)
                            End If

                        Case Else
                            Dim dt As DataTable = mObjFilialbuch.Protokoll.CreateTable(IFilialbuchEntry.EntryStatus.Ausblenden, CType(ddlFilter.SelectedValue, IFilialbuchEntry.EntryStatus), IFilialbuchEntry.EmpfängerStatus.Ausblenden, IFilialbuchEntry.EmpfängerStatus.Ausblenden)
                            gvAllGL.DataSource = dt

                    End Select

                    gvAllGL.DataBind()

            End Select
        End If
    End Sub

    ''' <summary>
    ''' Löscht alle geschlossenen Datensätze aus der mitgegebenen Tabelle
    ''' </summary>
    ''' <param name="dt">Protokolltabelle</param>
    ''' <returns>Protokolltabelle ohne geschlossene Sätze</returns>
    ''' <remarks></remarks>
    Private Function FilterClosed(ByRef dt As DataTable) As DataTable
        'geschlossene Sätze filtern
        For Each row As DataRow In dt.Rows
            Dim bDelete = False
            If Not TypeOf row("I_STATUS") Is DBNull Then
                If row("I_STATUS") = "Geschlossen" Then
                    bDelete = True
                End If
            End If

            If bDelete = False And Not TypeOf row("O_STATUS") Is DBNull Then
                If row("O_STATUS") = "Geschlossen" Then
                    row.Delete()
                End If
            End If

            If bDelete Then
                row.Delete()
            End If
        Next
        dt.AcceptChanges()

        Return dt
    End Function
   
    ''' <summary>
    ''' Ereignis das beim Entladen der Seite aufgerufen wird
    ''' </summary>
    ''' <param name="sender">Absender des Ereignisses</param>
    ''' <param name="e">EventArgumente</param>
    ''' <remarks></remarks>
    Private Sub Filialbuch_Unload(sender As Object, e As EventArgs) Handles Me.Unload
        ' aktuellen Objekt-Status sichern
        Session("mObjFilialbuch") = mObjFilialbuch
        Session("curView") = curView
    End Sub

#Region "ButtonEvents"

    Protected Sub lb_zurueck_Click(sender As Object, e As EventArgs) Handles lb_zurueck.Click
        If curView = ViewStatus.Bedienerkarte Then
            Response.Redirect("../Selection.aspx")
        Else
            curView = ViewStatus.Bedienerkarte
            ViewControl(curView)
        End If
    End Sub

    Protected Sub lbtAdd_Click(sender As Object, e As EventArgs) Handles lbtAdd.Click
        ShowPopUpNewEntry()
    End Sub

    Protected Sub lbtFilialbesuch_Click(sender As Object, e As EventArgs) Handles lbtFilialbesuch.Click
        mObjFilialbuch.NeuerEintrag("Filialbesuch durch Leiter Filialbetrieb", "Filialbesuch durch Leiter Filialbetrieb am " & Today.ToShortDateString, mObjKasse.Lagerort,
                                    strBedienernummer, "FILB", "")

        If mObjFilialbuch.ErrorOccured Then
            lblError.Text = mObjFilialbuch.ErrorCode & ": " & mObjFilialbuch.ErrorMessage
        End If

        FillListProtokoll()
    End Sub

    Protected Sub lbProtkoll_Click(sender As Object, e As EventArgs) Handles lbProtkoll.Click
        If curView = ViewStatus.FilialeAufgaben Or curView = ViewStatus.FilialeProtokoll Then
            curView = ViewStatus.FilialeProtokoll
        Else
            curView = ViewStatus.Gebietsleiter
        End If
        ViewControl(curView)
        FillListProtokoll()
    End Sub

    Private Sub lbAufgaben_Click(sender As Object, e As EventArgs) Handles lbAufgaben.Click
        curView = ViewStatus.FilialeAufgaben
        ViewControl(curView)
        FillListAufgaben()
    End Sub

    Protected Sub lbtnRefresh_Click(sender As Object, e As EventArgs) Handles lbtnRefresh.Click
        ' # CurView bereits auf GL oder Filiale gesetzt, daher keine Änderung nötig
        FillListProtokoll()
    End Sub

#End Region

    ''' <summary>
    ''' Ansichtssteuerung für alle Elemente der Seite
    ''' </summary>
    ''' <param name="VS">Das anzuzeigende Seiten-Layout</param>
    ''' <remarks></remarks>
    Private Sub ViewControl(ByVal VS As ViewStatus)
        Select Case VS
            Case ViewStatus.Bedienerkarte
                tblBedienerkarte.Visible = True
                tblHeaderTabs.Visible = False

                gvAufgabenFiliale.Visible = False
                gvAllFiliale.Visible = False
                gvAllGL.Visible = False

                EditAufgabe2.Visible = False

                Timespan.Visible = False

                lblBedienernummer.Text = String.Empty
                lblUser.Text = String.Empty

                txtBedienerkarte.Focus()

            Case ViewStatus.FilialeProtokoll
                tblBedienerkarte.Visible = False
                tblHeaderTabs.Visible = True

                lbAufgaben.Visible = True
                lbAufgaben.CssClass = "TabButtonBig Active"
                lbProtkoll.CssClass = "TabButtonBig"

                gvAufgabenFiliale.Visible = False
                gvAllFiliale.Visible = True
                gvAllGL.Visible = False

                Timespan.Visible = True
                ddlFilter.Visible = False
                ddlFilterFiliale.Visible = True

                EditAufgabe2.Visible = False

            Case ViewStatus.FilialeAufgaben
                tblBedienerkarte.Visible = False
                tblHeaderTabs.Visible = True

                lbAufgaben.Visible = True
                lbAufgaben.CssClass = "TabButtonBig"
                lbProtkoll.CssClass = "TabButtonBig Active"

                gvAufgabenFiliale.Visible = True
                gvAllFiliale.Visible = False
                gvAllGL.Visible = False

                Timespan.Visible = False
                
                EditAufgabe2.Visible = True
                lbtAdd.Visible = True
                lbtFilialbesuch.Visible = False
                lbtAdd.Text = "Anfrage"

            Case ViewStatus.Gebietsleiter
                tblBedienerkarte.Visible = False
                tblHeaderTabs.Visible = True

                lbAufgaben.Visible = False
                lbAufgaben.CssClass = "TabButtonBig Active"
                lbProtkoll.CssClass = "TabButtonBig"

                gvAufgabenFiliale.Visible = False
                gvAllFiliale.Visible = False
                gvAllGL.Visible = True

                Timespan.Visible = True
                ddlFilter.Visible = True
                ddlFilterFiliale.Visible = False

                EditAufgabe2.Visible = True
                lbtAdd.Visible = True
                lbtFilialbesuch.Visible = True
                lbtAdd.Text = "hinzufügen"
        End Select

    End Sub

#Region "GridViewEvents"

    Private Sub gvAufgabenFiliale_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvAufgabenFiliale.RowCommand
        Dim tmpRows = mObjFilialbuch.Protokoll.ProtokollTabelle.Select("Rowindex='" & e.CommandArgument & "'")
        Select Case e.CommandName
            Case "ReadAufgabeText"
                Dim Text = mObjLongStringToSap.ReadStringERP(CStr(tmpRows(0)("I_LTXNR")))
                ShowPopUp(False, False, CStr(tmpRows(0)("I_BETREFF")), Text, e.CommandArgument)
            Case "AnswerAufgabe"
                ShowPopUp(True, False, "AW:" & CStr(tmpRows(0)("I_BETREFF")), "", e.CommandArgument)
            Case "ErlAufgabe"
                mObjFilialbuch.Protokoll.EintragBeantworten(e.CommandArgument, strBedienernummer, IFilialbuchEntry.EmpfängerStatus.Erledigt)
                If mObjFilialbuch.Protokoll.Fehler Then
                    lblError.Text = "Es ist ein Fehler aufgetreten: " & mObjFilialbuch.Protokoll.Message
                End If
                FillListAufgaben()
            Case "ReadAufgabe"
                mObjFilialbuch.Protokoll.EintragBeantworten(e.CommandArgument, strBedienernummer, IFilialbuchEntry.EmpfängerStatus.Gelesen)
                If mObjFilialbuch.Protokoll.Fehler Then
                    lblError.Text = "Es ist ein Fehler aufgetreten: " & mObjFilialbuch.Protokoll.Message
                End If
                FillListAufgaben()
            Case "DatumEingangSort"
                Dim NewDir As SortDirection
                If ViewState("SortDirection") = SortDirection.Ascending Then
                    NewDir = SortDirection.Descending
                Else
                    NewDir = SortDirection.Ascending
                End If
                ViewState("SortDirection") = NewDir
                gvAufgabenFiliale.Sort("I_ERDAT, I_ERZEIT", NewDir)
            Case "SortVon"
                Dim NewDir As SortDirection
                If ViewState("SortDirection") = SortDirection.Ascending Then
                    NewDir = SortDirection.Descending
                Else
                    NewDir = SortDirection.Ascending
                End If
                ViewState("SortDirection") = NewDir
                gvAufgabenFiliale.Sort("I_VON", NewDir)
            Case "RückfrageAufgabe"
                ShowPopUp(True, True, "RF:" & CStr(tmpRows(0)("I_BETREFF")), "", e.CommandArgument)
        End Select
    End Sub

    Private Sub gvAufgabenFiliale_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvAufgabenFiliale.Sorting
        Dim View As DataView = mObjFilialbuch.Protokoll.ProtokollTabelle.DefaultView
        Dim sortparts As String() = e.SortExpression.Split(","c)
        Dim sortString As String = ""

        For i = 0 To sortparts.GetLength(0) - 1
            sortString += sortparts(i)

            If e.SortDirection = SortDirection.Ascending Then
                sortString += " ASC"
            Else
                sortString += " DESC"
            End If
            If i < sortparts.GetLength(0) - 1 Then
                sortString += ","
            End If
        Next
        View.Sort = sortString
        gvAufgabenFiliale.DataSource = View
        gvAufgabenFiliale.DataBind()
    End Sub

    Private Sub gvAllFiliale_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvAllFiliale.RowCommand
        Dim tmpRows = mObjFilialbuch.Protokoll.ProtokollTabelle.Select("Rowindex='" & e.CommandArgument & "'")
        Select Case e.CommandName
            Case "ReadAufgabeText"
                Dim Text = mObjLongStringToSap.ReadStringERP(CStr(tmpRows(0)("I_LTXNR")))
                ShowPopUp(False, False, CStr(tmpRows(0)("I_BETREFF")), Text, e.CommandArgument)
            Case "ReadAnswerText"
                Dim Text = mObjLongStringToSap.ReadStringERP(CStr(tmpRows(0)("O_LTXNR")))
                ShowPopUp(False, False, CStr(tmpRows(0)("O_BETREFF")), Text, e.CommandArgument)
            Case "AnswerAufgabe"
                ShowPopUp(True, False, "AW:" & CStr(tmpRows(0)("I_BETREFF")), "", e.CommandArgument)
            Case "ErlAufgabe"
                mObjFilialbuch.Protokoll.EintragBeantworten(e.CommandArgument, strBedienernummer, IFilialbuchEntry.EmpfängerStatus.Erledigt)
                If mObjFilialbuch.Protokoll.Fehler Then
                    lblError.Text = "Es ist ein Fehler aufgetreten: " & mObjFilialbuch.Protokoll.Message
                End If
                FillListProtokoll()
            Case "LoeAufgabe"
                mObjFilialbuch.Protokoll.EintragAbschliessen(e.CommandArgument, strBedienernummer, IFilialbuchEntry.EntryStatus.Gelöscht)
                If mObjFilialbuch.Protokoll.Fehler Then
                    lblError.Text = "Es ist ein Fehler aufgetreten: " & mObjFilialbuch.Protokoll.Message
                End If
                FillListProtokoll()
            Case "CloseAufgabe"
                mObjFilialbuch.Protokoll.EintragAbschliessen(e.CommandArgument, strBedienernummer, IFilialbuchEntry.EntryStatus.Geschlossen)
                If mObjFilialbuch.Protokoll.Fehler Then
                    lblError.Text = "Es ist ein Fehler aufgetreten: " & mObjFilialbuch.Protokoll.Message
                End If
                FillListProtokoll()
            Case "ReadAufgabe"
                mObjFilialbuch.Protokoll.EintragBeantworten(e.CommandArgument, strBedienernummer, IFilialbuchEntry.EmpfängerStatus.Gelesen)
                If mObjFilialbuch.Protokoll.Fehler Then
                    lblError.Text = "Es ist ein Fehler aufgetreten: " & mObjFilialbuch.Protokoll.Message
                End If
                FillListProtokoll()
            Case "DatumEingangSort"
                Dim NewDir As SortDirection
                If ViewState("SortDirection") = SortDirection.Ascending Then
                    NewDir = SortDirection.Descending
                Else
                    NewDir = SortDirection.Ascending
                End If
                ViewState("SortDirection") = NewDir
                gvAllFiliale.Sort("I_ERDAT, I_ERZEIT", NewDir)
            Case "DatumAusgangSort"
                Dim NewDir As SortDirection
                If ViewState("SortDirection") = SortDirection.Ascending Then
                    NewDir = SortDirection.Descending
                Else
                    NewDir = SortDirection.Ascending
                End If
                ViewState("SortDirection") = NewDir
                gvAllFiliale.Sort("O_ERDAT, O_ERZEIT", NewDir)
            Case "SortVon"
                Dim NewDir As SortDirection
                If ViewState("SortDirection") = SortDirection.Ascending Then
                    NewDir = SortDirection.Descending
                Else
                    NewDir = SortDirection.Ascending
                End If
                ViewState("SortDirection") = NewDir
                gvAllFiliale.Sort("I_VON", NewDir)
            Case "SortAn"
                Dim NewDir As SortDirection
                If ViewState("SortDirection") = SortDirection.Ascending Then
                    NewDir = SortDirection.Descending
                Else
                    NewDir = SortDirection.Ascending
                End If
                ViewState("SortDirection") = NewDir
                gvAllFiliale.Sort("O_AN", NewDir)
        End Select
    End Sub

    Private Sub gvAllFiliale_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvAllFiliale.Sorting
        Dim View As DataView = mObjFilialbuch.Protokoll.ProtokollTabelle.DefaultView
        Dim sortparts As String() = e.SortExpression.Split(","c)
        Dim sortString As String = ""

        For i = 0 To sortparts.GetLength(0) - 1
            sortString += sortparts(i)

            If e.SortDirection = SortDirection.Ascending Then
                sortString += " ASC"
            Else
                sortString += " DESC"
            End If
            If i < sortparts.GetLength(0) - 1 Then
                sortString += ","
            End If
        Next
        View.Sort = sortString
        gvAllFiliale.DataSource = View
        gvAllFiliale.DataBind()
    End Sub

    Private Sub gvAllGL_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvAllGL.RowCommand
        Dim tmpRows = mObjFilialbuch.Protokoll.ProtokollTabelle.Select("Rowindex='" & e.CommandArgument & "'")
        Select Case e.CommandName
            Case "ReadAufgabeText"
                Dim Text = mObjLongStringToSap.ReadStringERP(CStr(tmpRows(0)("I_LTXNR")))
                ShowPopUp(False, False, CStr(tmpRows(0)("I_BETREFF")), Text, e.CommandArgument)
            Case "ReadAnswerText"
                Dim Text = mObjLongStringToSap.ReadStringERP(CStr(tmpRows(0)("O_LTXNR")))
                ShowPopUp(False, False, CStr(tmpRows(0)("O_BETREFF")), Text, e.CommandArgument)
            Case "AnswerAufgabe"
                ShowPopUp(True, False, "AW:" & CStr(tmpRows(0)("I_BETREFF")), "", e.CommandArgument)
            Case "ErlAufgabe"
                mObjFilialbuch.Protokoll.EintragBeantworten(e.CommandArgument, strBedienernummer, IFilialbuchEntry.EmpfängerStatus.Erledigt)
                If mObjFilialbuch.Protokoll.Fehler Then
                    lblError.Text = "Es ist ein Fehler aufgetreten: " & mObjFilialbuch.Protokoll.Message
                End If
                FillListProtokoll()
            Case "LoeAufgabe"
                mObjFilialbuch.Protokoll.EintragAbschliessen(e.CommandArgument, strBedienernummer, IFilialbuchEntry.EntryStatus.Gelöscht)
                If mObjFilialbuch.Protokoll.Fehler Then
                    lblError.Text = "Es ist ein Fehler aufgetreten: " & mObjFilialbuch.Protokoll.Message
                End If
                FillListProtokoll()
            Case "CloseAufgabe"
                mObjFilialbuch.Protokoll.EintragAbschliessen(e.CommandArgument, strBedienernummer, IFilialbuchEntry.EntryStatus.Geschlossen)
                If mObjFilialbuch.Protokoll.Fehler Then
                    lblError.Text = "Es ist ein Fehler aufgetreten: " & mObjFilialbuch.Protokoll.Message
                End If
                FillListProtokoll()
            Case "ReadAufgabe"
                mObjFilialbuch.Protokoll.EintragBeantworten(e.CommandArgument, strBedienernummer, IFilialbuchEntry.EmpfängerStatus.Gelesen)
                If mObjFilialbuch.Protokoll.Fehler Then
                    lblError.Text = "Es ist ein Fehler aufgetreten: " & mObjFilialbuch.Protokoll.Message
                End If
                FillListProtokoll()
            Case "DatumEingangSort"
                Dim NewDir As SortDirection
                If ViewState("SortDirection") = SortDirection.Ascending Then
                    NewDir = SortDirection.Descending
                Else
                    NewDir = SortDirection.Ascending
                End If
                ViewState("SortDirection") = NewDir
                gvAllGL.Sort("I_DATETIME", NewDir)
            Case "DatumAusgangSort"
                Dim NewDir As SortDirection
                If ViewState("SortDirection") = SortDirection.Ascending Then
                    NewDir = SortDirection.Descending
                Else
                    NewDir = SortDirection.Ascending
                End If
                ViewState("SortDirection") = NewDir
                gvAllGL.Sort("O_DATETIME", NewDir)
            Case "SortVon"
                Dim NewDir As SortDirection
                If ViewState("SortDirection") = SortDirection.Ascending Then
                    NewDir = SortDirection.Descending
                Else
                    NewDir = SortDirection.Ascending
                End If
                ViewState("SortDirection") = NewDir
                gvAllGL.Sort("I_VON", NewDir)
            Case "SortAn"
                Dim NewDir As SortDirection
                If ViewState("SortDirection") = SortDirection.Ascending Then
                    NewDir = SortDirection.Descending
                Else
                    NewDir = SortDirection.Ascending
                End If
                ViewState("SortDirection") = NewDir
                gvAllGL.Sort("O_AN", NewDir)
        End Select
    End Sub

    Private Sub gvAllGL_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvAllGL.Sorting
        Dim View As DataView = mObjFilialbuch.Protokoll.ProtokollTabelle.DefaultView
        Dim sortparts As String() = e.SortExpression.Split(","c)
        Dim sortString As String = ""

        For i = 0 To sortparts.GetLength(0) - 1
            sortString += sortparts(i)

            If e.SortDirection = SortDirection.Ascending Then
                sortString += " ASC"
            Else
                sortString += " DESC"
            End If
            If i < sortparts.GetLength(0) - 1 Then
                sortString += ","
            End If
        Next
        View.Sort = sortString
        gvAllGL.DataSource = View
        gvAllGL.DataBind()
    End Sub

#End Region

#Region "PopUpActions"

    Private Sub ClosePopup()
        txtBetreff.Text = ""
        txtText.Text = ""
        lblBetreff.Text = ""
        lblText.Text = ""
        chkEdit.Checked = False
        chkIsRückfrage.Checked = False

        mpeLangtext.Hide()
    End Sub

    Private Sub ClosePopupNewEntry()
        txtNewBetreff.Text = ""
        txtNewText.Text = ""
        txtZuerledigenBis.Text = ""

        mpeNeuerText.Hide()
    End Sub

    Private Sub ShowPopUp(ByVal editmode As Boolean, ByVal isrückfrage As Boolean, ByVal strBetreff As String, ByVal strText As String, ByVal rowindex As String, Optional ByVal vorgid As String = "", _
                            Optional ByVal lfdnr As String = "", Optional ByVal an As String = "")
        txtBetreff.Visible = editmode
        txtText.Visible = editmode

        lblBetreff.Text = Not editmode
        lblText.Text = Not editmode

        lblErrorLangtext.Text = ""
        lblVorgangsID.Text = vorgid
        lblLFDNR.Text = lfdnr
        lblAn.Text = an
        lblRowIndex.Text = rowindex

        If editmode Then
            txtBetreff.Text = strBetreff
            txtText.Text = strText
            divText.Attributes("Style") = "margin: 0px 3px 10px 3px; font-size: 10px; overflow: visible; height: 230px; border: none;"
            lblBetreff.Text = ""
            lblText.Text = ""
            chkEdit.Checked = True
            chkIsRückfrage.Checked = isrückfrage
        Else
            txtBetreff.Text = ""
            txtText.Text = ""
            divText.Attributes("Style") = "margin: 0px 3px 10px 3px; font-size: 10px; overflow: visible; height: 230px; border: solid 1px #dddddd;"
            lblBetreff.Text = strBetreff
            lblText.Text = strText
            chkEdit.Checked = False
            chkIsRückfrage.Checked = False
        End If

        mpeLangtext.Show()
    End Sub

    Private Sub ShowPopUpNewEntry()
        Dim lstVorgangsarten As New List(Of VorgangsartDetails)(mObjFilialbuch.Vorgangsarten)

        For i As Integer = (lstVorgangsarten.Count - 1) To 0 Step -1
            Dim item As VorgangsartDetails = lstVorgangsarten(i)
            Dim gefunden As Boolean = False

            ' Nur die für die Rolle zulässigen Vorgangsarten zur Auswahl stellen
            For Each itemRolle As VorgangsartRolleDetails In mObjFilialbuch.VorgangsartenRolle
                If item.Vorgangsart = itemRolle.Vorgangsart Then
                    gefunden = True
                    Exit For
                End If
            Next

            If Not gefunden OrElse item.Vorgangsart = "ANTW" Then
                lstVorgangsarten.Remove(item)
            End If
        Next

        ddlVorgangsarten.DataSource = lstVorgangsarten
        ddlVorgangsarten.DataValueField = "Vorgangsart"
        ddlVorgangsarten.DataTextField = "Bezeichnung"
        ddlVorgangsarten.DataBind()

        mpeNeuerText.Show()
    End Sub

    Protected Sub ibtnOkNew_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnOkNew.Click
        If txtNewBetreff.Text.TrimStart(",") = "" Then
            lblErrorNewText.Text = "Geben Sie einen Betreff ein!"
            mpeNeuerText.Show()
            Exit Sub
        End If

        Dim empfaenger As String = mObjKasse.Lagerort

        ' Wenn Absender Filialmitarbeiter -> Empfänger = Vorgesetzter
        If ddlVorgangsarten.SelectedValue = "FILZ" Then
            empfaenger = mObjFilialbuch.UserLoggedIn.NamePa
        End If

        mObjFilialbuch.NeuerEintrag(txtNewBetreff.Text, txtNewText.Text, empfaenger, strBedienernummer, ddlVorgangsarten.SelectedValue, txtZuerledigenBis.Text)

        If mObjFilialbuch.ErrorOccured Then
            lblError.Text = mObjFilialbuch.ErrorCode & ": " & mObjFilialbuch.ErrorMessage
        End If

        FillListProtokoll()
        ClosePopupNewEntry()
    End Sub

    Protected Sub ibtnOK_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnOK.Click
        If chkEdit.Checked Then

            If txtBetreff.Text.TrimStart(",") = "" Then
                lblErrorLangtext.Text = "Geben Sie einen Betreff ein!"
                mpeLangtext.Show()
                Exit Sub
            ElseIf txtText.Text.Trim() = "" Then
                lblErrorLangtext.Text = "Geben Sie einen Antworttext ein!"
                mpeLangtext.Show()
                Exit Sub
            Else
                If curView = ViewStatus.FilialeAufgaben Then
                    If chkIsRückfrage.Checked Then
                        mObjFilialbuch.Protokoll.Rückfrage(lblRowIndex.Text, txtBetreff.Text, txtText.Text, strBedienernummer, mObjFilialbuch.UserLoggedIn.Kostenstelle)
                    Else
                        mObjFilialbuch.Protokoll.EintragBeantworten(lblRowIndex.Text, txtBetreff.Text, txtText.Text, strBedienernummer)
                    End If
                    If mObjFilialbuch.Protokoll.Fehler Then
                        lblError.Text = "Es ist ein Fehler aufgetreten: " & mObjFilialbuch.Protokoll.Message
                    End If
                    FillListAufgaben()
                ElseIf curView = ViewStatus.FilialeProtokoll Then
                    mObjFilialbuch.Protokoll.EintragBeantworten(lblRowIndex.Text, txtBetreff.Text, txtText.Text, strBedienernummer)
                    If mObjFilialbuch.Protokoll.Fehler Then
                        lblError.Text = "Es ist ein Fehler aufgetreten: " & mObjFilialbuch.Protokoll.Message
                    End If
                    FillListProtokoll()
                ElseIf curView = ViewStatus.Gebietsleiter Then
                    mObjFilialbuch.Protokoll.EintragBeantworten(lblRowIndex.Text, txtBetreff.Text, txtText.Text, strBedienernummer)
                    If mObjFilialbuch.Protokoll.Fehler Then
                        lblError.Text = "Es ist ein Fehler aufgetreten: " & mObjFilialbuch.Protokoll.Message
                    End If
                    FillListProtokoll()
                End If
            End If
        End If

        ClosePopup()
    End Sub

#End Region

#Region "HelpProcedures"

    Protected Sub img_prerender(ByVal sender As Object, ByVal e As EventArgs)
        Dim img As HtmlImage = sender
        Select Case img.Attributes("value")
            Case "Neu"
                img.Visible = True
                img.Src = "../images/new.png"
                img.Alt = "Neu"
                img.Attributes.Add("Title", "Neu")
            Case "Geantwortet"
                img.Visible = True
                img.Src = "../images/email.png"
                img.Alt = "Geantwortet"
                img.Attributes.Add("Title", "Geantwortet")
            Case "AutomatischBeantwortet"
                img.Visible = True
                img.Src = "../images/email.png"
                img.Alt = "Automatisch beantwortet"
                img.Attributes.Add("Title", "Automatisch beantwortet")
            Case "Geschlossen"
                img.Visible = True
                img.Src = "../images/Lock.png"
                img.Alt = "Geschlossen"
                img.Attributes.Add("Title", "Geschlossen")
            Case "Gelöscht"
                img.Visible = True
                img.Src = "../images/bin_closed.png"
                img.Alt = "Gelöscht"
                img.Attributes.Add("Title", "Gelöscht")
            Case "Gesendet"
                img.Visible = True
                img.Src = "../images/Email_go.png"
                img.Alt = "Gesendet"
                img.Attributes.Add("Title", "Gesendet")
            Case Else
                img.Visible = False
        End Select
    End Sub

    Protected Sub img2_prerender(ByVal sender As Object, ByVal e As EventArgs)
        Dim img As HtmlImage = sender
        Select Case img.Attributes("value")
            Case "Neu"
                img.Visible = True
                img.Src = "../images/new.png"
                img.Alt = "Neu"
                img.Attributes.Add("Title", "Neu")
            Case ("Gelesen")
                img.Visible = True
                img.Src = "../images/eye.png"
                img.Alt = "Gelesen"
                img.Attributes.Add("Title", "Gelesen")
            Case "AutomatischBeantwortet"
                img.Visible = True
                img.Src = "../images/email.png"
                img.Alt = "Automatisch beantwortet"
                img.Attributes.Add("Title", "Automatisch beantwortet")
            Case ("Geantwortet")
                img.Visible = True
                img.Src = "../images/email.png"
                img.Alt = "Geantwortet"
                img.Attributes.Add("Title", "Geantwortet")
            Case ("Gelöscht")
                img.Visible = True
                img.Src = "../images/bin_closed.png"
                img.Alt = "Gelöscht"
                img.Attributes.Add("Title", "Gelöscht")
            Case ("Erledigt")
                img.Visible = True
                img.Src = "../images/haken_gruen.gif"
                img.Alt = "Erledigt"
                img.Attributes.Add("Title", "Erledigt")
            Case Else
                img.Visible = False
        End Select
    End Sub

    Protected Sub DivRender(sender As Object, e As EventArgs)
        Dim div As HtmlControl = sender
        Select Case div.Attributes("value")
            Case "True"
                div.Attributes("Style") = "height:22px; margin-bottom:3px; white-space:nowrap; border-top:solid 1px #595959;"
            Case Else
                div.Attributes("Style") = "height:22px; margin-bottom:3px; white-space:nowrap;"
        End Select
    End Sub

#End Region

End Class