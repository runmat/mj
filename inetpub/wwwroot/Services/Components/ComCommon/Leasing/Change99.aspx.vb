﻿Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary

Partial Public Class Change99
    Inherits Page
    Private _mApp As App
    Private _mUser As User
    Private _mVersand As Briefversand

    Protected WithEvents GridNavigation1 As Services.GridNavigation
    Protected WithEvents GridNavigation2 As Services.GridNavigation
    Protected WithEvents UpdatePanel1 As UpdatePanel

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        _mUser = GetUser(Me)
        FormAuth(Me, _mUser)
        GetAppIDFromQueryString(Me)
        _mApp = New App(_mUser)
        GridNavigation1.setGridElment(GridView1)
        GridNavigation1.setGridTitle("Versandfähige Dokumente")

        GridNavigation2.setGridTitle("Fehlerfälle / Dokumente ohne Versandmöglichkeit")
        GridNavigation2.setGridElment(GridView2)

        If Not Session("me") Is Nothing Then
            Reset()
            Exit Sub
        End If

        'Prüfen, ob die Adresspflege in der Gruppe des Users ist
        InApp()

        cpeDokuAusgabe.Collapsed = False
        cpeDokuAusgabe.ClientState = Nothing

        If Not IsPostBack Then
            fillBrieflieferanten()
            chkGruende.Attributes.Add("onclick", "return false;")
        ElseIf Not Session("App_Versand") Is Nothing Then
            _mVersand = CType(Session("App_Versand"), Briefversand)
        End If

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
        HelpProcedures.FixedGridViewCols(GridView1)
        HelpProcedures.FixedGridViewCols(GridView2)
        HelpProcedures.FixedGridViewCols(GridView3)
        If lbl_ExtendSearch.Visible = False Then
            ibtExtendSearch.Visible = False
        End If
        lbl_ExtendSearch.Text = ""
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Protected Sub ImageButton2Click(ByVal sender As Object, ByVal e As ImageClickEventArgs) Handles ImageButton2.Click

        'GridViews zurücksetzen
        Dim dummyTable As New DataTable
        GridView1.DataSource = dummyTable
        GridView1.DataBind()
        GridView2.DataSource = dummyTable
        GridView2.DataBind()


        Result.Visible = False

        If (txtFahrgestellnummer.Text.Trim.Length + txtKennz.Text.Trim.Length + _
            txtLeasingvertragsnummer.Text.Trim.Length + txtZBIINummer.Text.Trim.Length + _
            txtLeasingvertragsnummer.Text.Trim.Length + txtReferenznummer1.Text.Trim.Length + txtReferenznummer1.Text.Trim.Length + _
            txtAbmeldeauftragVon.Text.Trim.Length + _
            txtAbmeldeauftragBis.Text.Trim.Length + _
            txtAbmeldedatumVon.Text.Trim.Length + _
            txtAbmeldedatumBis.Text.Trim.Length + _
            txtRestlaufzeit.Text.Trim.Length + _
            txtZulassungsdatumVon.Text.Trim.Length + _
            txtZulassungsdatumBis.Text.Trim.Length > 0) OrElse ddlBrieflieferant.SelectedIndex > 0 Then

            If txtKennz.Text.Split(",").Length > 0 AndAlso txtKennz.Text.Trim.Length > 0 Then
                Dim sKennz() As String = txtKennz.Text.Split(",")
                CreateTableUploadKennz(sKennz)
                DoSubmit2()
            ElseIf txtLeasingvertragsnummer.Text.Split(",").Length > 0 AndAlso txtLeasingvertragsnummer.Text.Trim.Length > 0 Then
                Dim sVnr() As String = txtLeasingvertragsnummer.Text.Split(",")
                CreateTableUploadVNr(sVnr)
                DoSubmit2()
            ElseIf txtZBIINummer.Text.Split(",").Length > 0 AndAlso txtZBIINummer.Text.Trim.Length > 0 Then
                Dim sBriefNr() As String = txtZBIINummer.Text.Split(",")
                CreateTableUploadBriefNr(sBriefNr)
                DoSubmit2()
            Else
                DoSubmit()
            End If
            If _mVersand.Fahrzeuge.Rows.Count = 0 AndAlso _mVersand.FahrzeugeFehler.Rows.Count = 0 Then
                lblErrorDokumente.Text = "Es wurde keine Fahrzeuge gefunden."
            Else
                cpeAllData.ClientState = True
                cpeUpload.ClientState = True
            End If

        Else
            lblErrorDokumente.Text = "Bitte geben Sie ein Suchkriterium ein!"
        End If
    End Sub

    Private Sub DoSubmit()
        _mVersand = New Briefversand(_mUser, _mApp, Session("AppID").ToString, Session.SessionID.ToString, "")

        _mVersand.Fahrgestellnr = txtFahrgestellnummer.Text.Trim
        If txtKennz.Text.Trim.Length > 0 Then
            _mVersand.Kennzeichen = txtKennz.Text.Trim.ToUpper
        End If

        _mVersand.LVnr = txtLeasingvertragsnummer.Text.Trim
        _mVersand.Zb2Nr = txtZBIINummer.Text.Trim
        _mVersand.Ref1 = txtReferenznummer1.Text.Trim
        _mVersand.Ref2 = txtReferenznummer2.Text.Trim
        _mVersand.EQuiTyp = "B"

        'Erweiterte Selektion
        With _mVersand

            If ddlBrieflieferant.SelectedIndex > 0 Then
                .BrieflieferantNr = ddlBrieflieferant.SelectedValue
            End If

            If chkAbgemeldet.Checked = True Then
                .Abgemeldet = "X"
            End If

            .Restlaufzeit = txtRestlaufzeit.Text
            .AbmeldedatumVon = txtAbmeldedatumVon.Text
            .AbmeldedatumBis = txtAbmeldedatumBis.Text
            .AbmeldeauftragVon = txtAbmeldeauftragVon.Text
            .AbmeldeauftragBis = txtAbmeldeauftragBis.Text
            .ZulassungsdatumVon = txtZulassungsdatumVon.Text
            .ZulassungsdatumBis = txtZulassungsdatumBis.Text

        End With

        _mVersand.Fill(Session("AppID").ToString, Session.SessionID.ToString, Me)

        If _mVersand.Status > 0 Then

        Else

            FillGrid(0)
            FillGridFehler(0)
            Session("App_Versand") = _mVersand
        End If

    End Sub

    Private Sub DoSubmit2()
        _mVersand.EQuiTyp = "B"
        _mVersand.Fill(Session("AppID").ToString, Session.SessionID.ToString, Me, True)
        If _mVersand.Status = 0 Then
            FillGrid(0)
            FillGridFehler(0)
            Session("App_Versand") = _mVersand
        End If
    End Sub

    Private Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub GridNavigation1_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation1.PagerChanged
        GridView1.PageIndex = PageIndex
        FillGrid(PageIndex)
    End Sub

    Private Sub GridNavigation1PageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillGrid(0)
    End Sub

    Private Sub Gridview1Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs) Handles GridView1.Sorting
        FillGrid(GridView1.PageIndex, e.SortExpression)
    End Sub

    Private Sub GridView2_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles GridView2.PageIndexChanging
        FillGridFehler(e.NewPageIndex)
    End Sub

    Private Sub GridNavigation2_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation2.PagerChanged
        GridView2.PageIndex = PageIndex
        FillGridFehler(PageIndex)
    End Sub

    Private Sub GridNavigation2_PageSizeChanged() Handles GridNavigation2.PageSizeChanged
        FillGridFehler(0)
    End Sub

    Private Sub Gridview2_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs) Handles GridView2.Sorting
        FillGridFehler(GridView1.PageIndex, e.SortExpression)
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")

        CheckGridFahrzeuge()

        Dim tmpDataView As DataView = _mVersand.Fahrzeuge.DefaultView
        tmpDataView.RowFilter = ""

        If tmpDataView.Count = 0 Then
            GridView1.Visible = False
            data.Visible = False
            Result.Visible = False
            GridNavigation1.Visible = False

        Else
            Dim intTempPageIndex As Int32 = intPageIndex
            Dim strTempSort As String = ""
            Dim strDirection As String = ""
            GridView1.Visible = True
            data.Visible = True
            Result.Visible = True
            GridNavigation1.Visible = True

            If strSort.Trim(" "c).Length > 0 Then
                intTempPageIndex = 0
                strTempSort = strSort.Trim(" "c)
                If (ViewState("Sort") Is Nothing) OrElse (ViewState("Sort").ToString = strTempSort) Then
                    If ViewState("Direction") Is Nothing Then
                        strDirection = "desc"
                    Else
                        strDirection = ViewState("Direction").ToString
                    End If
                Else
                    strDirection = "desc"
                End If

                If strDirection = "asc" Then
                    strDirection = "desc"
                Else
                    strDirection = "asc"
                End If

                ViewState("Sort") = strTempSort
                ViewState("Direction") = strDirection
            Else
                If Not ViewState("Sort") Is Nothing Then
                    strTempSort = ViewState("Sort").ToString
                    If ViewState("Direction") Is Nothing Then
                        strDirection = "asc"
                        ViewState("Direction") = strDirection
                    Else
                        strDirection = ViewState("Direction").ToString
                    End If
                End If
            End If

            If Not strTempSort.Length = 0 Then
                tmpDataView.Sort = strTempSort & " " & strDirection
            End If

            GridView1.DataSource = tmpDataView
            GridView1.DataBind()
            GridView1.PageIndex = intTempPageIndex

            For Each gridrow As GridViewRow In GridView1.Rows

                Dim strHistoryLink As String
                Dim lnkFahrgestellnummer As HyperLink
                Dim lbl As Label
                If _mUser.Applications.[Select]("AppName = 'Report02'").Length > 0 Then
                    strHistoryLink = "../../../applications/AppF2/forms/Report02.aspx?AppID=" & _mUser.Applications.Select("AppName = 'Report02'")(0)("AppID").ToString() & "&VIN="
                    For Each grdRow As GridViewRow In GridView1.Rows
                        lnkFahrgestellnummer = DirectCast(grdRow.FindControl("lnkHistorie"), HyperLink)
                        lbl = DirectCast(grdRow.FindControl("lblAbmeldedatum"), Label)

                        If lbl.Text = "" Then
                            lbl.Text = "XX.XX.XXXX"
                            lbl.ForeColor = Drawing.Color.Red
                        End If

                        If lnkFahrgestellnummer IsNot Nothing Then
                            lnkFahrgestellnummer.NavigateUrl = strHistoryLink + lnkFahrgestellnummer.Text & "&cw=True"
                        End If

                    Next
                End If


                'Pruefen, ob schon in der Autorisierung.
                Dim strInitiator As String = ""
                Dim intAuthorizationID As Int32
                Dim sFin As String
                sFin = CType(gridrow.Cells(2).FindControl("lblFahrgestellnummer"), Label).Text
                _mApp.CheckForPendingAuthorization(CInt(Session("AppID")), _mUser.Organization.OrganizationId, _mUser.CustomerName, sFin, _mUser.IsTestUser, strInitiator,
                                                   intAuthorizationID)
                If Not strInitiator.Length = 0 Then
                    'Fahrzeug wurde schon mal freigegeben und liegt zur Autorisierung vor
                    CType(gridrow.Cells(9).FindControl("lblStatus"), Label).Text = "liegt zur Autorisierung vor"
                    CType(gridrow.Cells(1).FindControl("chkAnfordern"), CheckBox).Enabled = False
                End If
            Next
        End If
    End Sub

    Private Sub FillGridFehler(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        Dim tmpDataView As DataView = _mVersand.FahrzeugeFehler.DefaultView
        tmpDataView.RowFilter = ""

        If tmpDataView.Count = 0 Then

            GridView2.Visible = False
            data2.Visible = False
            GridNavigation2.Visible = False
        Else
            Dim intTempPageIndex As Int32 = intPageIndex
            Dim strTempSort As String = ""
            Dim strDirection As String = ""
            Result.Visible = True

            GridNavigation2.Visible = True
            GridView2.Visible = True
            data2.Visible = True
            If strSort.Trim(" "c).Length > 0 Then
                intTempPageIndex = 0
                strTempSort = strSort.Trim(" "c)
                If (ViewState("Sort") Is Nothing) OrElse (ViewState("Sort").ToString = strTempSort) Then
                    If ViewState("Direction") Is Nothing Then
                        strDirection = "desc"
                    Else
                        strDirection = ViewState("Direction").ToString
                    End If
                Else
                    strDirection = "desc"
                End If

                If strDirection = "asc" Then
                    strDirection = "desc"
                Else
                    strDirection = "asc"
                End If

                ViewState("Sort") = strTempSort
                ViewState("Direction") = strDirection
            Else
                If Not ViewState("Sort") Is Nothing Then
                    strTempSort = ViewState("Sort").ToString
                    If ViewState("Direction") Is Nothing Then
                        strDirection = "asc"
                        ViewState("Direction") = strDirection
                    Else
                        strDirection = ViewState("Direction").ToString
                    End If
                End If
            End If

            If Not strTempSort.Length = 0 Then
                tmpDataView.Sort = strTempSort & " " & strDirection
            End If

            GridView2.DataSource = tmpDataView
            GridView2.DataBind()
            GridView2.PageIndex = intTempPageIndex

        End If
    End Sub

    Private Sub FillOverView()
        lblAdrOverview.Text = lbl_SelAdresse.Text
        lblAdrOverviewShow.Text = lbl_SelAdresseShow.Text

        lblGrundOverviewShow.Text = ddlVersandgrund.SelectedItem.Text
        lblOptionsOverViewShow.Text = ""
        For Each litem As ListItem In chkGruende.Items
            If litem.Selected = True Then
                lblOptionsOverViewShow.Text &= litem.Text & "<br />"
            End If
        Next

        If rb_endg.Checked = True Then
            lblVersArtOverviewShow.Text = rb_endg.Text
        Else
            lblVersArtOverviewShow.Text = rb_temp.Text
        End If
    End Sub

    Private Sub FillGridOverView(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        Dim tmpDataView As DataView = _mVersand.Fahrzeuge.DefaultView
        tmpDataView.RowFilter = "Selected = '1'"

        If tmpDataView.Count = 0 Then
            GridView3.Visible = False
            ResultOverView.Visible = False

        Else
            Dim intTempPageIndex As Int32 = intPageIndex
            Dim strTempSort As String = ""
            Dim strDirection As String = ""
            GridView3.Visible = True
            ResultOverView.Visible = True
            If strSort.Trim(" "c).Length > 0 Then
                intTempPageIndex = 0
                strTempSort = strSort.Trim(" "c)
                If (ViewState("Sort") Is Nothing) OrElse (ViewState("Sort").ToString = strTempSort) Then
                    If ViewState("Direction") Is Nothing Then
                        strDirection = "desc"
                    Else
                        strDirection = ViewState("Direction").ToString
                    End If
                Else
                    strDirection = "desc"
                End If

                If strDirection = "asc" Then
                    strDirection = "desc"
                Else
                    strDirection = "asc"
                End If

                ViewState("Sort") = strTempSort
                ViewState("Direction") = strDirection
            Else
                If Not ViewState("Sort") Is Nothing Then
                    strTempSort = ViewState("Sort").ToString
                    If ViewState("Direction") Is Nothing Then
                        strDirection = "asc"
                        ViewState("Direction") = strDirection
                    Else
                        strDirection = ViewState("Direction").ToString
                    End If
                End If
            End If

            If Not strTempSort.Length = 0 Then
                tmpDataView.Sort = strTempSort & " " & strDirection
            End If

            GridView3.DataSource = tmpDataView
            GridView3.DataBind()
            GridView3.PageIndex = intTempPageIndex


        End If
    End Sub

    Private Sub fillBrieflieferanten()
        If _mVersand Is Nothing Then
            _mVersand = New Briefversand(_mUser, _mApp, Session("AppID").ToString, Session.SessionID.ToString, "")
        End If
        If _mVersand.Brieflieferanten Is Nothing Then
            hdnField.Value = "1"
            tr_Brieflieferant.Visible = False
            Exit Sub
        End If

        ddlBrieflieferant.DataSource = _mVersand.Brieflieferanten
        ddlBrieflieferant.DataTextField = "Adresse"
        ddlBrieflieferant.DataValueField = "KUNNR"
        ddlBrieflieferant.DataBind()

    End Sub

    Private Sub fillLaenderDLL()
        Dim sprache As String
        'Länder DLL füllen
        ddlLand.DataSource = _mVersand.Laender
        ddlLand.DataTextField = "FullDesc"
        ddlLand.DataValueField = "Land1"
        ddlLand.DataBind()
        'vorbelegung der Länderddl auf Grund der im Browser eingestellten erstsprache JJ2007.12.06
        Dim tmpstr() As String
        If Request("HTTP_ACCEPT_Language").IndexOf(",") = -1 Then
            'es ist nur eine sprache ausgewählt
            sprache = Request("HTTP_ACCEPT_Language")
        Else
            'es gibt eine erst und eine zweitsprache
            sprache = Request("HTTP_ACCEPT_Language").Substring(0, Request("HTTP_ACCEPT_Language").IndexOf(","))
        End If

        tmpstr = sprache.Split(CChar("-"))
        ' Länderkennzeichen setzen sich aus Region und Sprache zusammen. de-ch, de-at usw. leider werden bei Regionen in denen die Sprache das selbe Kürzel 
        ' hat nur einfache Kürzel geschrieben, z.b. bei "de"
        If tmpstr.Length > 1 Then
            sprache = tmpstr(1).ToUpper
        Else
            sprache = tmpstr(0).ToUpper
        End If
        ddlLand.Items.FindByValue(sprache).Selected = True

    End Sub

    Private Sub ibtNext_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles ibtNext.Click
        If CheckGridFahrzeuge() = True Then
            If _mVersand.Fahrzeuge.Select("Selected = '1'").Length > 1 Then
                lblMsgHeader.Text = "Ausgewählte Fahrzeuge"
                litMessage.Text = "Sie haben " & _mVersand.Fahrzeuge.Select("Selected = '1'").Length & " Briefe für den Versand ausgewählt."
                divMessage.Visible = True
                divBackDisabled.Style.Add("Height", "120%")
                divBackDisabled.Visible = True
                Exit Sub
            End If

        End If

        NextToChoice()
    End Sub

    Private Sub NextToChoice()
        lblErrorVersandOpt.Text = ""
        Dim bCheckgrid As Boolean = CheckGridFahrzeuge()
        If bCheckgrid = True Then
            VersandTabPanel1.Visible = False
            VersandTabPanel2.Visible = True
            lbtnStammdaten.CssClass = "VersandButtonStammReady"
            lbtnAdressdaten.CssClass = "VersandButtonAdresse"
            lblSteps.Text = "Schritt 2 von 4"
            Panel2.CssClass = "StepActive"
            _mVersand.EqTyp = "B"
            _mVersand.GetAdressenandZulStellen(Session("AppID").ToString, Session.SessionID.ToString, Me)
            If _mVersand.Status <> 0 Then
                lblErrorVersandOpt.Text = _mVersand.Message
            Else
                fillLaenderDLL()
                Session("App_Versand") = _mVersand
            End If
            If Not _mVersand.VersandArt Is Nothing Then
                If _mVersand.VersandArt = "1" Then
                    rb_temp.Checked = True
                ElseIf _mVersand.VersandArt = "2" Then
                    rb_endg.Checked = True
                End If

            End If
            trAdressuche.Visible = False
            trZulStelleSuche.Visible = False
            trFreieAdresse.Visible = False
            _mApp.GetAppAutLevel(_mUser.GroupID, Session("AppID").ToString)

            Dim Level() As String

            If String.IsNullOrEmpty(_mApp.AutorisierungsLevel) = False Then

                Level = Split(_mApp.AutorisierungsLevel, "|")
                Level = Split(Level(0), ",")

                rb_temp.Visible = False
                rb_endg.Visible = False

                For i As Integer = 0 To Level.Length - 1

                    Select Case Level(i)

                        Case 1, 2, 3
                            rb_temp.Visible = True
                        Case 4, 5, 6
                            rb_endg.Visible = True
                    End Select


                Next
            End If

            If rb_temp.Visible = True AndAlso rb_endg.Visible = False Then
                rb_temp.Checked = True
            ElseIf rb_temp.Visible = False AndAlso rb_endg.Visible = True Then
                rb_endg.Checked = True
            End If

            RadioButtonVersandChanged()

            If rb_endg.Visible = True AndAlso rb_temp.Visible = True Then
                If rb_endg.Checked = False AndAlso rb_temp.Checked = False Then
                    trAdressuche.Visible = False
                    trZulStelleSuche.Visible = False
                    trFreieAdresse.Visible = False
                End If
            End If

        Else
            lblErrorDokumente.Text = "Bitte wählen Sie ein Dokument zur Versendung aus!"
        End If
    End Sub

    Private Function CheckGridFahrzeuge() As Boolean

        For Each gvRow As GridViewRow In GridView1.Rows
            Dim chkAnfordern As CheckBox
            Dim lblEQUNR As Label
            lblEQUNR = CType(gvRow.Cells(0).FindControl("lblEQUNR"), Label)
            chkAnfordern = CType(gvRow.Cells(1).FindControl("chkAnfordern"), CheckBox)
            If chkAnfordern.Checked = True Then
                _mVersand.Fahrzeuge.Select("EQUNR = '" + lblEQUNR.Text + "'")(0)("Selected") = "1"
            End If
        Next
        Return _mVersand.Fahrzeuge.Select("Selected = '1'").Any()
    End Function

    Protected Sub ibtnSearchAdresse_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs) Handles ibtnSearchAdresse.Click
        Dim tmpItem As ListItem
        Dim strFirma As String
        Dim strStrasse As String
        Dim strPlz As String
        Dim strOrt As String
        Dim strReferenz As String

        strFirma = txtFirma.Text.Replace("*", "")
        strStrasse = txtStrasse.Text.Replace("*", "")
        strPlz = txtPlz.Text.Replace("*", "")
        strOrt = txtOrt.Text.Replace("*", "")
        strReferenz = txtReferenz.Text.Replace("*", "")


        If strFirma.Length + strStrasse.Length + strPlz.Length + strOrt.Length + strReferenz.Length > 0 Then

            _mVersand.SReferenz = strReferenz
            _mVersand.SName1 = strFirma
            _mVersand.SName2 = txtName2.Text
            _mVersand.SStrasse = strStrasse
            _mVersand.SPlz = strPlz
            _mVersand.SOrt = strOrt


            _mVersand.GetAdressen(Session("AppID").ToString, Session.SessionID.ToString, Page)


            Dim dv As DataView = _mVersand.Adressen.DefaultView
            dv.Sort = "NAME1 asc"

            If dv.Count > 0 Then
                Dim i As Int32 = 0
                ddlAdresse.Items.Clear()
                Do While i < dv.Count
                    tmpItem = New ListItem(dv.Item(i)("NAME1").ToString & " " & dv.Item(i)("NAME2").ToString & " - " & dv.Item(i)("STREET").ToString & ", " &
                                           dv.Item(i)("CITY1").ToString, dv.Item(i)("IDENT").ToString)
                    ddlAdresse.Items.Add(tmpItem)
                    i += 1
                Loop
                tmpItem = New ListItem("- bitte auswählen -", "0000")
                ddlAdresse.Items.Insert(0, tmpItem)

                trddlAdresse.Visible = True

                If dv.Count = 1 Then ddlAdresse.SelectedIndex = 1

                If ddlAdresse.Items.Count > 20 Then

                    ddlAdresse.Visible = False
                    lblSucheAdr.Visible = True
                    lbl_Versandan.Visible = False
                    lblSucheAdr.Text = "Bitte über die Suchkriterien genauer eingrenzen!"

                Else
                    ddlAdresse.Visible = True
                    lblSucheAdr.Visible = False
                    lbl_Versandan.Visible = True

                End If
            Else
                trddlAdresse.Visible = True
                dv.RowFilter = ""
                lblSucheAdr.ForeColor = Drawing.Color.Red
                lblSucheAdr.Text = "Kein Ergebnisse gefunden!"
                ddlAdresse.Visible = False
                lblSucheAdr.Visible = True
                lbl_Versandan.Visible = False
            End If


        Else
            txtFirma.BorderColor = Drawing.Color.Red
            txtStrasse.BorderColor = Drawing.Color.Red
            txtPlz.BorderColor = Drawing.Color.Red
            txtOrt.BorderColor = Drawing.Color.Red
            lblSucheAdr.ForeColor = Drawing.Color.Red
            txtReferenz.ForeColor = Drawing.Color.Red

            lblSucheAdr.Text = "Kein Suchkriterium gefüllt!"
        End If
    End Sub

    Protected Sub ibtn_SucheGesch_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs) Handles ibtn_SucheGesch.Click
        Dim tmpItem As ListItem
        Dim strKennzKreis As String
        Dim strPLZ As String
        Dim strOrt As String

        strOrt = txtOrtSucheGe.Text.Replace("*", "%")
        strKennzKreis = txtKennzKreis.Text.Replace("*", "%")
        strPLZ = txtPLZSucheGe.Text.Replace("*", "%")

        If strOrt.Length + strKennzKreis.Length + strPLZ.Length > 0 Then

            Dim sQuery As String = ""

            If strOrt.Length > 0 Then
                sQuery += "ORT01 LIKE '" & strOrt.Trim & "' AND "
            End If

            If strKennzKreis.Length > 0 Then
                sQuery += "ZKFZKZ LIKE '" & strKennzKreis.Trim & "' AND "
            End If

            If strPLZ.Length > 0 Then
                sQuery += "PSTLZ LIKE '" & strPLZ.Trim & "' AND "
            End If

            sQuery = Left(sQuery, sQuery.Length - 4)

            Dim dv As DataView = _mVersand.ZulStellen.DefaultView
            dv.RowFilter = sQuery
            dv.Sort = "PSTLZ asc"

            If dv.Count > 0 Then
                Dim i As Int32 = 0
                ddlZulStelle.Items.Clear()
                Do While i < dv.Count
                    tmpItem = New ListItem(dv.Item(i)("PSTLZ").ToString & " - " & dv.Item(i)("ORT01").ToString & " - " & dv.Item(i)("STRAS").ToString, dv.Item(i)("LIFNR").ToString)
                    ddlZulStelle.Items.Add(tmpItem)
                    i += 1
                Loop
                tmpItem = New ListItem("- bitte auswählen -", "0000")
                ddlZulStelle.Items.Insert(0, tmpItem)
                trZulStelle.Visible = True

                If dv.Count = 1 Then ddlZulStelle.SelectedIndex = 1

                If ddlZulStelle.Items.Count > 20 Then

                    ddlZulStelle.Visible = False
                    lblErrZulStelle.Visible = True
                    lbl_ZulStelle.Visible = False
                    lblErrZulStelle.Text = "Bitte über die Suchkriterien genauer eingrenzen!"

                Else
                    ddlZulStelle.Visible = True
                    lblErrZulStelle.Visible = False
                    lbl_ZulStelle.Visible = True
                End If
            Else
                trZulStelle.Visible = True
                dv.RowFilter = ""
                lblErrZulStelle.ForeColor = Drawing.Color.Red
                lblErrZulStelle.Text = "Kein Ergebnisse gefunden!"
                ddlZulStelle.Visible = False
                lblErrZulStelle.Visible = True
                lbl_ZulStelle.Visible = False
            End If


        Else
            txtOrtSucheGe.BorderColor = Drawing.Color.Red
            txtKennzKreis.BorderColor = Drawing.Color.Red
            txtPLZSucheGe.BorderColor = Drawing.Color.Red
            lblErrZulStelle.Text = "Kein Suchkriterium gefüllt!"
        End If
    End Sub

    Protected Sub ibtnNextToOptions_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ibtnNextToOptions.Click
        lblErrorAdressen.Visible = False
        lblErrorAdressen.Text = ""
        If _mVersand.VersandAdresseText <> String.Empty Then

            If rb_endg.Checked = False AndAlso rb_temp.Checked = False Then
                lblErrorAdressen.Visible = True
                lblErrorAdressen.Text = "Bitte wählen Sie eine Versandart aus!"
                Exit Sub
            End If
            VersandTabPanel1.Visible = False
            VersandTabPanel2.Visible = False
            VersandTabPanel3.Visible = True
            lbtnStammdaten.CssClass = "VersandButtonStammReady"
            lbtnAdressdaten.CssClass = "VersandButtonAdresseReady"
            lbtnVersanddaten.CssClass = "VersandButtonOptionen"
            lbtnAdressdaten.Enabled = True
            lblSteps.Text = "Schritt 3 von 4"
            Panel3.CssClass = "StepActive"
            _mVersand.OptionFlag = "3"

            Dim strVersandart As String = IIf(rb_temp.Checked, "1", "2")

            _mVersand.GetVersandOptions(Session("AppID").ToString, Session.SessionID.ToString, Me)


            'Versandoption "Versand ohne Abmeldung" ggf. rausfiltern, sonst als Inverses "Auf Abmeldung warten" weiter nutzen
            'NewLevel beinhaltet 2 Arrays: Level-Array und Autorisierungsarray(1 zu 1) getrennt durch |
            Dim blnLevel7 As Boolean = False
            Dim strLevelText = _mUser.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("NewLevel")
            If Not IsDBNull(strLevelText) AndAlso Not String.IsNullOrEmpty(strLevelText) Then
                Dim strLevel = Split(_mUser.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("NewLevel"), "|")(0)
                Dim levels() As String = strLevel.Split(",")
                blnLevel7 = levels.Contains("7")
            End If

            Dim updRows As DataRow() = _mVersand.VersandOptionen.Select("EXTGROUP='" & strVersandart & "' AND EAN11 = 'ZZABMELD'")


            If blnLevel7 = True AndAlso strVersandart = 2 Then
                cbxAufAbmeldungWarten.Visible = True
                _mVersand.AufAbmeldungWarten = _mVersand.VersandOptionen.Select("EXTGROUP='2' AND EAN11 = 'ZZABMELD' AND VW_AG = 'X'").Any()
                cbxAufAbmeldungWarten.Checked = _mVersand.AufAbmeldungWarten
                ConfirmNextToOverView.Enabled = _mVersand.ShowStilllegungsdatumPopup(Session("AppID").ToString)
            Else
                ConfirmNextToOverView.Enabled = False
            End If

            For Each updRow In updRows
                _mVersand.VersandOptionen.Rows.Remove(updRow)
            Next



            If _mVersand.Status <> 0 Then
                lblErrorAdressen.Visible = True
                lblErrorAdressen.Text = _mVersand.Message
            Else
                Session("App_Versand") = _mVersand

                _mVersand.VersandOptionen.DefaultView.RowFilter = "EXTGROUP='" & strVersandart & "' AND INTROW <> '0000000000'"

                chkListGruende.DataSource = _mVersand.VersandOptionen.DefaultView
                chkListGruende.DataValueField = "EAN11"
                chkListGruende.DataTextField = "ASKTX"
                chkListGruende.DataBind()

                grvDL.Columns(2).Visible = True
                grvDL.Columns(3).Visible = True

                grvDL.DataSource = _mVersand.VersandOptionen.DefaultView
                grvDL.DataBind()

                If _mVersand.VersandOptionen.Select("Selected = '1'").Length > 0 Then
                    _mVersand.VersandOptionen.DefaultView.RowFilter = "EXTGROUP='" & strVersandart & "' AND INTROW <> '0000000000' AND Selected = '1'"
                    chkGruende.DataSource = _mVersand.VersandOptionen.DefaultView
                    chkGruende.DataValueField = "EAN11"
                    chkGruende.DataTextField = "ASKTX"
                    chkGruende.DataBind()

                    Dim cbx As CheckBox
                    Dim lbl As Label
                    Dim ibt As ImageButton

                    Dim booInfo As Boolean = False
                    Dim booPreis As Boolean = False

                    For Each litem As ListItem In chkGruende.Items
                        litem.Selected = True

                        For Each dr As GridViewRow In grvDL.Rows

                            lbl = CType(dr.FindControl("lblDL"), Label)
                            cbx = CType(dr.FindControl("cbxDL"), CheckBox)

                            If lbl.Text = litem.Value Then
                                cbx.Checked = True
                                Exit For
                            End If

                        Next

                    Next

                    For Each dr As GridViewRow In grvDL.Rows

                        ibt = CType(dr.FindControl("ibtDL"), ImageButton)

                        If dr.Cells(2).Text <> "0,00 €" AndAlso dr.Cells(2).Text <> "" Then
                            booPreis = True
                        Else
                            dr.Cells(2).Text = ""
                        End If

                        If ibt.ToolTip.Length > 0 Then
                            booInfo = True
                        End If

                    Next

                    grvDL.Columns(2).Visible = booPreis
                    grvDL.Columns(3).Visible = booInfo

                End If
                _mVersand.VersandArt = strVersandart



                _mVersand.GetAbrufgrund(Session("AppID").ToString, Session.SessionID.ToString, Me)

                If _mVersand.Status <> 0 Then
                    lblErrorAdressen.Visible = True
                    lblErrorAdressen.Text = _mVersand.Message
                Else
                    ddlVersandgrund.Items.Clear()
                    Dim NewItem As New ListItem
                    NewItem.Text = "- Bitte wählen -"
                    NewItem.Value = "0"
                    NewItem.Selected = True
                    ddlVersandgrund.Items.Add(NewItem)
                    For Each dRow As DataRow In _mVersand.Versandgruende.Rows
                        NewItem = New ListItem
                        NewItem.Text = dRow("VGRUND_TEXT").ToString
                        NewItem.Value = dRow("ZZVGRUND").ToString
                        ddlVersandgrund.Items.Add(NewItem)
                    Next

                    If ddlVersandgrund.Items.Count = 2 Then
                        ddlVersandgrund.SelectedIndex = 1
                    End If

                    If Not _mVersand.VersandGrund Is Nothing Then
                        ddlVersandgrund.SelectedValue = _mVersand.VersandGrund
                    End If

                    txtBemerkung.Text = _mVersand.Bemerkung
                    txtHalter.Text = _mVersand.Halter
                End If
            End If
        Else
            lblErrorAdressen.Visible = True
            lblErrorAdressen.Text = "Bitte wählen Sie eine Versandadresse aus!"
        End If
    End Sub

    Private Sub ibtnNextToOverView_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles ibtnNextToOverView.Click
        lblErrorVersandOpt.Text = ""
        If ddlVersandgrund.SelectedIndex = 0 Then
            lblErrorVersandOpt.Text += "Bitte wählen Sie einen Versandgrund aus!<br />"
            lblErrorVersandOpt.Visible = True
        Else
            _mVersand.VersandGrund = ddlVersandgrund.SelectedItem.Value

            Select Case ddlVersandgrund.SelectedItem.Value
                Case "001", "005"
                    If rb_temp.Checked = True AndAlso Trim(txtHalter.Text).Length = 0 Then
                        lblErrorVersandOpt.Text += "Bitte geben Sie einen Halter ein!<br />"
                        lblErrorVersandOpt.Visible = True
                    End If
            End Select

        End If

 
        _mVersand.Halter = txtHalter.Text
        _mVersand.Bemerkung = txtBemerkung.Text

        Dim bAuswahlNormal As Boolean = False

        For Each litem As ListItem In chkGruende.Items
            If litem.Selected Then
                _mVersand.Materialnummer = litem.Value
                bAuswahlNormal = True

            End If
        Next

        'Nur wenn Versandoption "Auf Abmeldung warten" explizit gewählt ist, kein "Versand ohne Abmeldung"
        If cbxAufAbmeldungWarten.Checked = False Then
            _mVersand.VersandOhneAbmeldung = ""
        Else
            _mVersand.VersandOhneAbmeldung = "X"
        End If

        If bAuswahlNormal = False Then
            lblErrorVersandOpt.Text += "Bitte wählen Sie min. eine Versandoption aus, die nicht einer Zusatzoption entspricht!<br />"
            lblErrorVersandOpt.Visible = True
        End If
        If rb_temp.Checked = False AndAlso rb_endg.Checked = False Then
            lblErrorVersandOpt.Text += "Bitte wählen Sie eine Versandart aus!<br />"
            lblErrorVersandOpt.Visible = True
        End If

        If lblErrorVersandOpt.Text.Length = 0 Then
            VersandTabPanel1.Visible = False
            VersandTabPanel2.Visible = False
            VersandTabPanel3.Visible = False
            VersandTabPanel4.Visible = True
            lbtnStammdaten.CssClass = "VersandButtonStammReady"
            lbtnAdressdaten.CssClass = "VersandButtonAdresseReady"
            lbtnVersanddaten.CssClass = "VersandButtonOptionenReady"
            lbtnOverview.CssClass = "VersandButtonOverview"
            lbtnVersanddaten.Enabled = True
            lblSteps.Text = "Schritt 4 von 4"
            Panel4.CssClass = "StepActive"
            FillGridOverView(0)
            FillOverView()
            ConfirmNextToOverView.Enabled = False
        End If


    End Sub

    Protected Sub ibtnShowOptions_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs) Handles ibtnShowOptions.Click

        Dim ibt As ImageButton

        Dim booInfo As Boolean = False
        Dim booPreis As Boolean = False

        For Each dr As GridViewRow In grvDL.Rows

            ibt = CType(dr.FindControl("ibtDL"), ImageButton)

            If dr.Cells(2).Text <> "0,00 €" AndAlso dr.Cells(2).Text <> "" Then
                booPreis = True
            Else
                dr.Cells(2).Text = ""
            End If

            If ibt.ToolTip.Length > 0 Then
                booInfo = True
            End If

        Next

        grvDL.Columns(2).Visible = booPreis
        grvDL.Columns(3).Visible = booInfo


        divOptions.Visible = True


        divBackDisabled.Visible = True
    End Sub

    Private Sub lbtnCloseOption_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnCloseOption.Click
        divOptions.Visible = False
        divBackDisabled.Visible = False

        For Each litem As ListItem In chkGruende.Items
            litem.Selected = True
        Next
    End Sub

    Protected Sub lbtnSelectGruende_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnSelectGruende.Click
        Dim tempEndg As String = IIf(rb_temp.Checked, "1", "2")


        Dim lbl As Label
        Dim cbx As CheckBox

        For Each dr As GridViewRow In grvDL.Rows

            lbl = CType(dr.FindControl("lblDL"), Label)
            cbx = CType(dr.FindControl("cbxDL"), CheckBox)


            If cbx.Checked = True Then
                _mVersand.VersandOptionen.Select("EXTGROUP='" + tempEndg + "' AND EAN11 = '" + lbl.Text + "'")(0)("Selected") = "1"
            Else
                _mVersand.VersandOptionen.Select("EXTGROUP='" + tempEndg + "' AND EAN11 = '" + lbl.Text + "'")(0)("Selected") = "0"
            End If
            _mVersand.VersandOptionen.AcceptChanges()
        Next

        Dim bvalidate As Boolean = True
        lblErrPopUp.Visible = False

        Dim drows() As DataRow = _mVersand.VersandOptionen.Select("EXTGROUP='" + tempEndg + "'  AND Selected = '1'")
        If drows.Length > 0 Then
            For Each dRowSel As DataRow In drows
                If dRowSel("ALTERNAT").ToString = "X" Then
                    Dim drowsBasis() As DataRow
                    drowsBasis = _mVersand.VersandOptionen.Select("EXTGROUP='" + tempEndg + "'  AND Selected = '1' AND INTROW='" + dRowSel("ALT_INTROW").ToString + "'")
                    If drowsBasis.Length > 0 Then
                        bvalidate = False
                        lblErrPopUp.Visible = True
                        lblErrPopUp.Text = "Die ausgewählte Option """ + dRowSel("ASKTX").ToString + """ steht im Konflikt mit der Option """ + _
                        drowsBasis(0)("ASKTX").ToString + """. Bitte wählen Sie eine Option ab!"
                    Else
                        drowsBasis = _mVersand.VersandOptionen.Select("EXTGROUP='" + tempEndg + "'  AND Selected = '1' AND ALT_INTROW='" + _
                                                                    dRowSel("ALT_INTROW").ToString + _
                                                                    "' AND Not INTROW = '" + _
                                                                    dRowSel("INTROW").ToString + _
                                                                    "' AND  ALTERNAT = 'X'")
                        If drowsBasis.Length > 0 Then

                            bvalidate = False
                            lblErrPopUp.Visible = True
                            lblErrPopUp.Text = "Die ausgewählte Option """ + dRowSel("ASKTX").ToString + """ steht im Konflikt mit der Option """ + _
                            drowsBasis(0)("ASKTX").ToString + """. Bitte wählen Sie eine Option ab!"

                        End If
                    End If
                End If
            Next
        End If

        If bvalidate = True Then
            _mVersand.VersandOptionen.DefaultView.RowFilter = IIf(rb_temp.Checked, "EXTGROUP='1'", "EXTGROUP='2'")
            _mVersand.VersandOptionen.DefaultView.RowFilter += " AND Selected = '1'"
            chkGruende.DataSource = _mVersand.VersandOptionen.DefaultView
            chkGruende.DataValueField = "EAN11"
            chkGruende.DataTextField = "ASKTX"
            chkGruende.DataBind()

            For Each litem As ListItem In chkGruende.Items
                litem.Selected = True
            Next

            divOptions.Visible = False
            divBackDisabled.Visible = False
        End If

        'ConfirmNextToOverView.Enabled = _mVersand.ShowStilllegungsdatumPopup(Session("AppID").ToString)

    End Sub

    Protected Sub ibtnSucheSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ibtnSucheSave.Click
        AdressChoice()
    End Sub

    Private Sub AdressChoice()
        _mVersand.VersandAdresseText = ""
        If ddlAdresse.SelectedIndex > 0 Then
            Dim AdrRow As DataRow = _mVersand.Adressen.Select("IDENT = '" + ddlAdresse.SelectedValue + "'")(0)

            lblSucheAdr.Visible = False

            lbl_SelAdresseShow.Text = AdrRow("NAME1").ToString + " " & AdrRow("NAME2").ToString + _
                                    " <br /> " + AdrRow("STREET").ToString + " " + AdrRow("HOUSE_NUM1").ToString + " <br /> " + _
                                     AdrRow("POST_CODE1").ToString + " " + AdrRow("CITY1").ToString + " <br /> " & AdrRow("COUNTRY").ToString
            lbl_SelAdresse.Text = "Adresse:"
            trSelAdresse.Visible = True

            DivZulstelleSucheHead.Attributes.Add("style", "background-color:#A5A5A5")
            DivZulstelleHeadFlag.Attributes.Add("style", "background-color:#A5A5A5")
            PLZulstelle.Enabled = False

            DivFreeAdressLeftFlag.Attributes.Add("style", "background-color:#A5A5A5")
            DivFreeAdrSucheHead.Attributes.Add("style", "background-color:#A5A5A5")
            PLAdressmanuell.Enabled = False

            cpeZulstelle.ClientState = True
            cpeAdressmanuell.ClientState = True
            cpeAdressSuche.ClientState = True
            _mVersand.VersandAdresseText = lbl_SelAdresseShow.Text

            'jetzt immer die komplette Adresse mitgeben
            _mVersand.VersandAdresseZe = String.Empty
            'jetzt Debitornummer (SAPNR) weitergeben
            _mVersand.VersandAdresseZs = AdrRow("SAPNR").ToString

            'Manuelle Adresse
            _mVersand.Name1 = AdrRow("NAME1").ToString
            _mVersand.Name2 = AdrRow("NAME2").ToString
            _mVersand.Street = AdrRow("STREET").ToString
            _mVersand.HouseNum = AdrRow("HOUSE_NUM1").ToString
            _mVersand.PostCode = AdrRow("POST_CODE1").ToString
            _mVersand.City = AdrRow("CITY1").ToString
            _mVersand.LaenderKuerzel = AdrRow("COUNTRY").ToString

            If rb_temp.Checked = True Then
                _mVersand.Adressart = Briefversand.Adressarten.TempSuche
            Else
                _mVersand.Adressart = Briefversand.Adressarten.EndSuche
            End If
        Else
            lblSucheAdr.Visible = True
            lblSucheAdr.Text = "Bitte wählen Sie eine Adresse aus!"
        End If
    End Sub

    Protected Sub ibtnSucheGeschSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ibtnSucheGeschSave.Click

        If ddlZulStelle.SelectedIndex > 0 Then
            _mVersand.VersandAdresseText = ""
            Dim ZulRow As DataRow = _mVersand.ZulStellen.Select("LIFNR = '" + ddlZulStelle.SelectedValue + "'")(0)

            lbl_SelAdresseShow.Text = ZulRow("PSTLZ").ToString & " " & ZulRow("ORT01").ToString & " <br /> " & ZulRow("STRAS").ToString
            lbl_SelAdresse.Text = "Zulassungsstelle: "
            trSelAdresse.Visible = True

            DivAdressSucheHead.Attributes.Add("style", "background-color:#A5A5A5")
            DivAdressLeftFlag.Attributes.Add("style", "background-color:#A5A5A5")
            PLAdressSuche.Enabled = False

            DivFreeAdressLeftFlag.Attributes.Add("style", "background-color:#A5A5A5")
            DivFreeAdrSucheHead.Attributes.Add("style", "background-color:#A5A5A5")
            PLAdressmanuell.Enabled = False

            cpeZulstelle.ClientState = True
            cpeAdressmanuell.ClientState = True
            cpeAdressSuche.ClientState = True

            _mVersand.VersandAdresseZe = ddlZulStelle.SelectedItem.Value      'Versandadresse Nr. (60...)
            _mVersand.VersandAdresseText = lbl_SelAdresseShow.Text  'Versanddresse (Text...)

            'jetzt immer die komplette Adresse mitgeben
            _mVersand.VersandAdresseZs = String.Empty

            'Manuelle Adresse nullen
            _mVersand.Name1 = String.Empty
            _mVersand.Name2 = String.Empty
            _mVersand.Street = String.Empty
            _mVersand.HouseNum = String.Empty
            _mVersand.PostCode = String.Empty
            _mVersand.City = String.Empty
            _mVersand.LaenderKuerzel = String.Empty

            If rb_temp.Checked = True Then
                _mVersand.Adressart = Briefversand.Adressarten.TempZulassungsstelle
            Else
                _mVersand.Adressart = Briefversand.Adressarten.EndZulassungsstelle
            End If

        End If
    End Sub

    Protected Sub ibtnSucheManuellSave_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs) Handles ibtnSucheManuellSave.Click
        _mVersand.VersandAdresseText = ""
        lblErrorAdrManuell.Text = ""

        If txtFirmaManuell.Text.Trim(" "c).Length = 0 Then
            lblErrorAdrManuell.Text &= "Bitte ""Name"" eingeben!<br>&nbsp;"
        Else
            _mVersand.Name1 = txtFirmaManuell.Text.Trim(" "c)
        End If
        If txtPlzManuell.Text.Trim(" "c).Length = 0 Then
            lblErrorAdrManuell.Text &= "Bitte ""PLZ"" eingeben!<br>&nbsp;"
        Else
            If CInt(_mVersand.Laender.Select("Land1='" & ddlLand.SelectedItem.Value & "'")(0)("Lnplz")) > 0 Then
                If Not CInt(_mVersand.Laender.Select("Land1='" & ddlLand.SelectedItem.Value & "'")(0)("Lnplz")) = txtPlzManuell.Text.Trim(" "c).Length Then
                    lblError.Text = "Postleitzahl hat falsche Länge."
                Else
                    _mVersand.PostCode = txtPlzManuell.Text.Trim(" "c)
                End If
            End If

        End If
        If txtOrtManuell.Text.Trim(" "c).Length = 0 Then
            lblErrorAdrManuell.Text &= "Bitte ""Ort"" eingeben!<br>"
        Else
            _mVersand.City = txtOrtManuell.Text.Trim(" "c)
        End If
        If txtStrasseManuell.Text.Trim(" "c).Length = 0 Then
            lblErrorAdrManuell.Text &= "Bitte ""Strasse"" eingeben!<br>"
        Else
            _mVersand.Street = txtStrasseManuell.Text.Trim(" "c)
        End If
        If txtNrManuell.Text.Trim(" "c).Length = 0 Then
            lblErrorAdrManuell.Text &= "Bitte ""Nummer"" eingeben!<br>"
        Else
            _mVersand.HouseNum = txtNrManuell.Text.Trim(" "c)
        End If
        _mVersand.Name2 = txtName2.Text.Trim(" "c)
        If lblErrorAdrManuell.Text = "" Then
            _mVersand.LaenderKuerzel = ddlLand.SelectedItem.Value

            lbl_SelAdresseShow.Text = _mVersand.Name1 + ", " + _mVersand.Name2 + " <br /> " + _
                                      _mVersand.Street + " " + _mVersand.HouseNum + " <br /> " + _
                                      _mVersand.PostCode + " " + _mVersand.City
            lbl_SelAdresse.Text = "Freie Adresse:"
            'SAP-Adresse nullen
            _mVersand.VersandAdresseZs = String.Empty
            _mVersand.VersandAdresseText = lbl_SelAdresseShow.Text
            'Zulassungsstelle nullen
            _mVersand.VersandAdresseZe = String.Empty


            If rb_temp.Checked = True Then
                _mVersand.Adressart = Briefversand.Adressarten.TempManuell
            Else
                _mVersand.Adressart = Briefversand.Adressarten.EndManuell
            End If

            trSelAdresse.Visible = True
            DivAdressSucheHead.Attributes.Add("style", "background-color:#A5A5A5")
            DivAdressLeftFlag.Attributes.Add("style", "background-color:#A5A5A5")
            PLAdressSuche.Enabled = False

            DivZulstelleSucheHead.Attributes.Add("style", "background-color:#A5A5A5")
            DivZulstelleHeadFlag.Attributes.Add("style", "background-color:#A5A5A5")
            PLZulstelle.Enabled = False

            cpeZulstelle.ClientState = True
            cpeAdressmanuell.ClientState = True
            cpeAdressSuche.ClientState = True



        Else : lblErrorAdrManuell.Visible = True
        End If

    End Sub

    Protected Sub lbtnAdrUnload_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnAdrUnload.Click
        ResetAdress()

    End Sub

    Protected Sub lbtnStammdaten_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnStammdaten.Click
        VersandTabPanel1.Visible = True
        VersandTabPanel2.Visible = False
        VersandTabPanel3.Visible = False
        VersandTabPanel4.Visible = False

        VersandTabPanel3.Visible = False
        VersandTabPanel4.Visible = False
        lbtnStammdaten.CssClass = "VersandButtonStamm"
        lbtnAdressdaten.CssClass = "VersandButtonAdresseEnabled"
        lbtnAdressdaten.Enabled = False
        lbtnVersanddaten.CssClass = "VersandButtonOptionenEnabled"
        lbtnVersanddaten.Enabled = False
        lbtnOverview.CssClass = "VersandButtonOverviewEnabled"
        lbtnOverview.Enabled = False
        lblSteps.Text = "Schritt 1 von 4"
        Panel2.CssClass = "Steps"
        Panel3.CssClass = "Steps"
        Panel4.CssClass = "Steps"
        lblErrorAdrManuell.Text = ""
        lblErrorVersandOpt.Text = ""
        lblErrorAnfordern.Text = ""


        If Request.Url.ToString.Contains("mode") = True Then

            GridView1.Visible = False
            data.Visible = False
            Result.Visible = False
            GridNavigation1.Visible = False

            GridView2.Visible = False
            data2.Visible = False
            GridNavigation2.Visible = False

            If Not _mVersand Is Nothing Then

                If Not _mVersand.Fahrzeuge Is Nothing Then
                    If _mVersand.Fahrzeuge.DefaultView.Count > 0 Then
                        FillGrid(0)
                    End If
                    cpeAllData.Collapsed = True
                    cpeUpload.Collapsed = True
                End If

                If Not _mVersand.FahrzeugeFehler Is Nothing Then
                    If _mVersand.FahrzeugeFehler.DefaultView.Count > 0 Then
                        FillGridFehler(0)
                    End If
                End If
            End If
        End If


    End Sub

    Protected Sub lbtnAdressdaten_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnAdressdaten.Click
        VersandTabPanel2.Visible = True
        VersandTabPanel3.Visible = False
        VersandTabPanel4.Visible = False
        lbtnAdressdaten.CssClass = "VersandButtonAdresse"
        lbtnVersanddaten.CssClass = "VersandButtonOptionenEnabled"
        lbtnVersanddaten.Enabled = False
        lbtnOverview.CssClass = "VersandButtonOverviewEnabled"
        lbtnOverview.Enabled = False
        lblSteps.Text = "Schritt 2 von 4"
        Panel3.CssClass = "Steps"
        Panel4.CssClass = "Steps"
        lblErrorVersandOpt.Text = ""
        lblErrorAnfordern.Text = ""
    End Sub

    Protected Sub lbtnVersanddaten_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnVersanddaten.Click

        VersandTabPanel3.Visible = True
        VersandTabPanel4.Visible = False
        lbtnVersanddaten.CssClass = "VersandButtonOptionen"
        lbtnOverview.CssClass = "VersandButtonOverviewEnabled"
        lbtnOverview.Enabled = False
        lblSteps.Text = "Schritt 3 von 4"
        Panel4.CssClass = "Steps"
        For Each litem As ListItem In chkGruende.Items
            litem.Selected = True
        Next
    End Sub

    Protected Sub lbtnSend_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnSend.Click
        _mApp.GetAppAutLevel(_mUser.GroupID, Session("AppID").ToString)



        'Authorizationright wird von der Autorisierung auf Levelebene übersteuert
        Dim ZurAutorisierung As Boolean = False
        If String.IsNullOrEmpty(_mApp.AutorisierungsLevel) = False Then
            ZurAutorisierung = Autorisieren()
        Else
            If _mUser.Groups.ItemByID(_mUser.GroupID).Authorizationright > 0 Then ZurAutorisierung = True
        End If

        If ZurAutorisierung = True Then

            For Each tmpRow As DataRow In _mVersand.Fahrzeuge.Rows
                If tmpRow("Selected").ToString = "1" Then
                    Dim logApp As New Base.Kernel.Logging.Trace(_mApp.Connectionstring, _mApp.SaveLogAccessSAP, _mApp.LogLevel)
                    logApp.CollectDetails("Fahrgestellnr.", CType(tmpRow("Fahrgestellnummer").ToString, Object), True)
                    logApp.CollectDetails("Nummer ZBII", CType(tmpRow("NummerZBII").ToString, Object))
                    logApp.CollectDetails("Leasingnummer", CType(tmpRow("Leasingnummer").ToString, Object))
                    logApp.CollectDetails("Referenz1", CType(tmpRow("Referenz1").ToString, Object))
                    logApp.CollectDetails("Referenz2", CType(tmpRow("Referenz2").ToString, Object))
                    logApp.CollectDetails("Versandart", CType(lblVersArtOverviewShow.Text, Object))
                    logApp.CollectDetails("Versandgrund", CType(lblGrundOverviewShow.Text, Object))
                    logApp.CollectDetails("Versandoption", CType(lblOptionsOverViewShow.Text, Object))
                    logApp.CollectDetails("Sachbearbeiter", CType(_mUser.UserName, Object))
                    logApp.CollectDetails("Bemerkung", CType(txtBemerkung.Text, Object))
                    logApp.CollectDetails("Halter", CType(txtHalter.Text, Object))


                    _mVersand.Sachbearbeiter = _mUser.UserName
                    _mVersand.ReferenceforAut = tmpRow("EQUNR").ToString
                    _mVersand.VersgrundText = lblGrundOverviewShow.Text
                    _mVersand.Bemerkung = txtBemerkung.Text
                    _mVersand.Halter = txtHalter.Text
                    _mVersand.Beauftragungsdatum = Date.Today.ToShortDateString
                    _mVersand.VersartText = lblVersArtOverviewShow.Text
                    _mVersand.Briefversand = "1"
                    _mVersand.SchluesselVersand = ""
                    _mVersand.OptionFlag = "3"
                    Dim DetailArray(1, 2) As Object
                    Dim ms As MemoryStream
                    Dim formatter As BinaryFormatter
                    Dim b() As Byte

                    ms = New MemoryStream()
                    formatter = New BinaryFormatter()
                    formatter.Serialize(ms, _mVersand)
                    b = ms.ToArray
                    ms = New MemoryStream(b)
                    DetailArray(0, 0) = ms
                    DetailArray(0, 1) = "VersandObject"

                    'Pruefen, ob schon in der Autorisierung.
                    Dim strInitiator As String = ""
                    Dim intAuthorizationID As Int32


                    _mApp.CheckForPendingAuthorization(CInt(Session("AppID")), _mUser.Organization.OrganizationId, _mUser.CustomerName, tmpRow("Fahrgestellnummer").ToString,
                                                       _mUser.IsTestUser, strInitiator, intAuthorizationID)
                    If Not strInitiator.Length = 0 Then
                        tmpRow("Status") = "liegt zur Autorisierung vor"
                    Else
                        intAuthorizationID = WriteAuthorization(_mApp.Connectionstring, CInt(Session("AppID")), _mUser.UserName, _mUser.Organization.OrganizationId,
                                                                _mUser.CustomerName, tmpRow("Fahrgestellnummer").ToString, "", "", _mUser.IsTestUser, DetailArray)
                        logApp.WriteEntry("APP", _mUser.UserName, Session.SessionID, CInt(Session("AppID")),
                                          _mUser.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString,
                                          tmpRow("Fahrgestellnummer").ToString, "Briefversand für " & tmpRow("Fahrgestellnummer").ToString & " erfolgreich initiiert.",
                                          _mUser.CustomerName, _mUser.Customer.CustomerId, _mUser.IsTestUser, 0, logApp.InputDetails)
                    End If

                    Session("App_Versand") = _mVersand
                    lblErrorAnfordern.Visible = True
                    lbtnSend.Enabled = False
                    lblErrorAnfordern.Text = "Ihre Anforderung liegt zur Autorisierung vor."
                    lblErrorAnfordern.ForeColor = Drawing.ColorTranslator.FromHtml("#52C529")
                    lbtnOverview.CssClass = "VersandButtonOverviewReady"
                    lbtnStammdaten.Enabled = False
                    lbtnAdressdaten.Enabled = False
                    lbtnVersanddaten.Enabled = False
                    lbtnOverview.Enabled = False
                    lb_zurueck.Visible = True
                    FillGridOverView(0)
                    Session("App_Versand") = _mVersand
                End If

            Next
            _mVersand.AutorisierungText = "mit Autorisierung"

            Session("App_Versand") = _mVersand

            ibtnCreatePDF.Visible = True
            lblPDFPrint.Visible = True

        Else
            _mVersand.Briefversand = "1"
            _mVersand.SchluesselVersand = ""
            _mVersand.Anfordern(Session("AppID").ToString, Session.SessionID.ToString, Me)
            If _mVersand.Status <> 0 Then
                lblErrorAnfordern.Visible = True
                lbtnSend.Enabled = False
                lblErrorAnfordern.Text = _mVersand.Message
                FillGridOverView(0)
            Else

                _mVersand.AutorisierungText = "ohne Autorisierung"
                _mVersand.VersartText = lblVersArtOverviewShow.Text
                _mVersand.Sachbearbeiter = _mUser.UserName
                _mVersand.VersgrundText = lblGrundOverviewShow.Text

                Session("App_Versand") = _mVersand
                lblErrorAnfordern.Visible = True
                lbtnSend.Enabled = False
                lblErrorAnfordern.Text = "Ihre Anforderung wurde erfolgreich im System erstellt."
                lblErrorAnfordern.ForeColor = Drawing.ColorTranslator.FromHtml("#52C529")
                lbtnOverview.CssClass = "VersandButtonOverviewReady"
                lbtnStammdaten.Enabled = False
                lbtnAdressdaten.Enabled = False
                lbtnVersanddaten.Enabled = False
                lbtnOverview.Enabled = False
                lb_zurueck.Visible = True

                ibtnCreatePDF.Visible = True
                lblPDFPrint.Visible = True

                FillGridOverView(0)
                For i As Integer = 4 To 8
                    GridView3.Columns(i).Visible = False
                Next
            End If

        End If

    End Sub

    Private Sub ResetAdress()
        _mVersand.VersandAdresseText = String.Empty

        _mVersand.VersandAdresseZe = String.Empty
        _mVersand.VersandAdresseText = String.Empty

        'SAP-Adresse nullen
        _mVersand.VersandAdresseZs = String.Empty

        'Manuelle Adresse nullen
        _mVersand.Name1 = String.Empty
        _mVersand.Name2 = String.Empty
        _mVersand.Street = String.Empty
        _mVersand.HouseNum = String.Empty
        _mVersand.PostCode = String.Empty
        _mVersand.City = String.Empty
        _mVersand.LaenderKuerzel = String.Empty

        ' Partneradressen
        DivAdressSucheHead.Style.Remove("background-color")
        DivAdressLeftFlag.Style.Remove("background-color")
        PLAdressSuche.Enabled = True
        txtFirma.Text = ""
        txtHNr.Text = ""
        txtStrasse.Text = ""
        txtPlz.Text = ""
        txtOrt.Text = ""
        txtLand.Text = ""
        txtReferenz.Text = ""
        ddlAdresse.Items.Clear()
        trddlAdresse.Visible = False

        ' ZulStellen
        DivZulstelleSucheHead.Style.Remove("background-color")
        DivZulstelleHeadFlag.Style.Remove("background-color")
        PLZulstelle.Enabled = True
        ddlZulStelle.Items.Clear()
        trZulStelle.Visible = False
        txtOrtSucheGe.Text = ""
        txtKennzKreis.Text = ""
        txtPLZSucheGe.Text = ""


        DivFreeAdrSucheHead.Style.Remove("background-color")
        DivFreeAdrSucheHead.Style.Remove("background-color")
        PLAdressmanuell.Enabled = True

        txtFirmaManuell.Text = ""
        txtName2.Text = ""
        txtStrasseManuell.Text = ""
        txtPlzManuell.Text = ""
        txtStrasseManuell.Text = ""
        'ddlLand.SelectedIndex = 0
        'Wieder auf Standardland zurücksetzen
        fillLaenderDLL()


        cpeZulstelle.ClientState = True
        cpeAdressmanuell.ClientState = True
        cpeAdressSuche.ClientState = True

        lbl_SelAdresseShow.Text = ""
        lbl_SelAdresse.Text = ""
        trSelAdresse.Visible = False
    End Sub

    Protected Sub lb_zurueck_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_zurueck.Click
        _mVersand = Nothing
        Session("m_Versand") = Nothing
        Response.Redirect("Change99.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub

    Protected Sub ibtnUpload_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs) Handles ibtnUpload.Click

        Result.Visible = False

        'Prüfe Fehlerbedingung
        If (Not upFile1.PostedFile Is Nothing) AndAlso (Not (upFile1.PostedFile.FileName = String.Empty)) Then
            'lblExcelfile.Text = upFile1.PostedFile.FileName
            If Right(upFile1.PostedFile.FileName.ToUpper, 4) <> ".XLS" AndAlso Right(upFile1.PostedFile.FileName.ToUpper, 5) <> ".XLSX" Then
                lblErrorDokumente.Text = "Es können nur Dateien im .XLS -bzw. XLSX Format verarbeitet werden."
                Exit Sub
            End If
        Else
            lblErrorDokumente.Text = "Keine Datei ausgewählt"
            Exit Sub
        End If
        _mVersand = New Briefversand(_mUser, _mApp, Session("AppID").ToString, Session.SessionID.ToString, "")
        _mVersand.CreateUploadTable()
        'Lade Datei
        upload(upFile1.PostedFile)

    End Sub

    Private Sub upload(ByVal uFile As HttpPostedFile)

        Try
            If Not (uFile Is Nothing) Then
                'CHC ITA 5972
                lblError.Text = ""
                lblErrorDokumente.Text = ""
                Dim uploadV As Upload_Validator.Validator = New Upload_Validator.Validator()
                Dim XLS_CheckTable As DataTable = uploadV.UploadXLSohneModifikation(upFile1.PostedFile, ConfigurationManager.AppSettings("ExcelPath"), _mUser, lblError,
                                                                                    Session("AppID").ToString, Session.SessionID)
                If uploadV.CheckObZeilenMitMehrAlsEinemWertExistieren(XLS_CheckTable) > -1 Then
                    lblErrorDokumente.Text += "Es gibt Zeilen mit mehr als einem Wert. Bitte korrigieren Sie Ihre Datei!<br>"
                    Return
                End If

                CheckInputTable(XLS_CheckTable)
                lblErrorDokumente.Text += lblError.Text
                lblError.Text = ""

                If _mVersand.TblUpload.Rows.Count > 0 Then
                    _mVersand.EQuiTyp = "B"
                    _mVersand.Fill(Session("AppID").ToString, Session.SessionID.ToString, Me, True)
                    If _mVersand.Status > 0 Then
                        lblErrorDokumente.Visible = True
                        lblErrorDokumente.Text += "Fehler beim hochladen der Datei! " & _mVersand.Message
                    Else
                        FillGrid(0)
                        FillGridFehler(0)
                        Session("App_Versand") = _mVersand
                        cpeAllData.ClientState = True
                        cpeUpload.ClientState = True
                    End If
                Else
                    lblErrorDokumente.Text += "Fehler beim Hochladen der Datei!"
                End If

            End If
        Catch ex As Exception
            lblErrorDokumente.Text += "Fehler beim Hochladen der Datei! " & ex.Message
        End Try
    End Sub

    Private Sub CheckInputTable(ByVal tblInput As DataTable)

        Dim rowData As DataRow
        Dim Fahrgestellnummer As String = ""
        Dim Kennzeichen As String = ""
        Dim NummerZB2 As String = ""
        Dim LeaseNr As String = ""
        Dim Ref1 As String = ""
        Dim Ref2 As String = ""

        'CHC ITA 5972
        Dim uploadV As Upload_Validator.Validator = New Upload_Validator.Validator()

        For Each rowData In tblInput.Rows

            If Not TypeOf rowData(0) Is DBNull Then
                Fahrgestellnummer = CStr(rowData(0)).Trim
            End If

            If tblInput.Columns.Count > 1 Then
                If Not TypeOf rowData(1) Is DBNull Then
                    Kennzeichen = uploadV.FindeDeutschesKennzeichen(rowData)
                End If
            End If
            If tblInput.Columns.Count > 2 Then
                If Not TypeOf rowData(2) Is DBNull Then
                    NummerZB2 = CStr(rowData(2)).Trim
                End If
            End If
            If tblInput.Columns.Count > 3 Then
                If Not TypeOf rowData(3) Is DBNull Then
                    LeaseNr = CStr(rowData(3)).Trim
                End If
            End If

            If tblInput.Columns.Count > 4 Then
                If Not TypeOf rowData(4) Is DBNull Then
                    Ref1 = CStr(rowData(4)).Trim(" "c)
                End If
            End If

            If tblInput.Columns.Count > 5 Then
                If Not TypeOf rowData(5) Is DBNull Then
                    Ref2 = CStr(rowData(5)).Trim
                End If
            End If

            If Fahrgestellnummer.Length + Kennzeichen.Length + NummerZB2.Length _
               + LeaseNr.Length + Ref1.Length + Ref2.Length > 0 Then

                Dim UploadRow As DataRow

                UploadRow = _mVersand.TblUpload.NewRow
                UploadRow("CHASSIS_NUM") = Fahrgestellnummer
                UploadRow("LICENSE_NUM") = Kennzeichen
                UploadRow("TIDNR") = NummerZB2
                UploadRow("LIZNR") = LeaseNr
                UploadRow("ZZREFERENZ1") = Ref1
                UploadRow("ZZREFERENZ2") = Ref2
                _mVersand.TblUpload.Rows.Add(UploadRow)

                Fahrgestellnummer = ""
                Kennzeichen = ""
                LeaseNr = ""
                NummerZB2 = ""
                Ref1 = ""
                Ref2 = ""


            Else
                Exit For 'Ausstieg: Leerzeilen sollte es nicht geben

            End If
        Next
    End Sub

    Private Sub CreateTableUploadKennz(ByVal sKennz() As String)
        _mVersand = New Briefversand(_mUser, _mApp, Session("AppID").ToString, Session.SessionID.ToString, "")
        _mVersand.CreateUploadTable()

        Dim UploadRow As DataRow
        For Each Kennzeichen As String In sKennz
            UploadRow = _mVersand.TblUpload.NewRow
            UploadRow("CHASSIS_NUM") = ""
            UploadRow("LICENSE_NUM") = Kennzeichen
            UploadRow("TIDNR") = ""
            UploadRow("LIZNR") = ""
            UploadRow("ZZREFERENZ1") = ""
            UploadRow("ZZREFERENZ2") = ""
            _mVersand.TblUpload.Rows.Add(UploadRow)
        Next
    End Sub

    Private Sub CreateTableUploadBriefNr(ByVal sBriefNr() As String)
        _mVersand = New Briefversand(_mUser, _mApp, Session("AppID").ToString, Session.SessionID.ToString, "")
        _mVersand.CreateUploadTable()

        Dim UploadRow As DataRow
        For Each TIDNr As String In sBriefNr
            UploadRow = _mVersand.TblUpload.NewRow
            UploadRow("CHASSIS_NUM") = ""
            UploadRow("LICENSE_NUM") = ""
            UploadRow("TIDNR") = TIDNr
            UploadRow("LIZNR") = ""
            UploadRow("ZZREFERENZ1") = ""
            UploadRow("ZZREFERENZ2") = ""
            _mVersand.TblUpload.Rows.Add(UploadRow)
        Next


    End Sub

    Private Sub CreateTableUploadVNr(ByVal sVNr() As String)
        _mVersand = New Briefversand(_mUser, _mApp, Session("AppID").ToString, Session.SessionID.ToString, "")
        _mVersand.CreateUploadTable()

        Dim UploadRow As DataRow
        For Each Vertragsnr As String In sVNr
            UploadRow = _mVersand.TblUpload.NewRow
            UploadRow("CHASSIS_NUM") = ""
            UploadRow("LICENSE_NUM") = ""
            UploadRow("TIDNR") = ""
            UploadRow("LIZNR") = Vertragsnr
            UploadRow("ZZREFERENZ1") = ""
            UploadRow("ZZREFERENZ2") = ""
            _mVersand.TblUpload.Rows.Add(UploadRow)
        Next
    End Sub

    Protected Sub rb_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rb_temp.CheckedChanged, rb_endg.CheckedChanged

        _mVersand.VersandGrund = Nothing

        ResetAdress()

        RadioButtonVersandChanged()
    End Sub

    Private Sub RadioButtonVersandChanged()

        _mApp.GetAppAutLevel(_mUser.GroupID, Session("AppID").ToString)

        If String.IsNullOrEmpty(_mApp.AutorisierungsLevel) = False Then
            Dim Level() As String

            trAdressuche.Visible = False
            trZulStelleSuche.Visible = False
            trFreieAdresse.Visible = False

            Level = Split(_mApp.AutorisierungsLevel, "|")
            Level = Split(Level(0), ",")
            For i As Integer = 0 To Level.Length - 1

                If rb_temp.Checked = True Then

                    Select Case Level(i)

                        Case Briefversand.Adressarten.TempSuche
                            trAdressuche.Visible = True
                        Case Briefversand.Adressarten.TempZulassungsstelle
                            trZulStelleSuche.Visible = True
                        Case Briefversand.Adressarten.TempManuell
                            trFreieAdresse.Visible = True

                    End Select


                ElseIf rb_endg.Checked = True Then
                    Select Case Level(i)

                        Case Briefversand.Adressarten.EndSuche
                            trAdressuche.Visible = True
                        Case Briefversand.Adressarten.EndZulassungsstelle
                            trZulStelleSuche.Visible = True
                        Case Briefversand.Adressarten.EndManuell
                            trFreieAdresse.Visible = True

                    End Select
                End If
            Next

        Else
            trAdressuche.Visible = True
            trZulStelleSuche.Visible = True
            trFreieAdresse.Visible = True
            cpeAdressSuche.ClientState = True
        End If

    End Sub

    Protected Sub ibtAuswahl_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim chk As CheckBox

        For Each dr As GridViewRow In GridView1.Rows

            chk = CType(dr.FindControl("chkAnfordern"), CheckBox)
            chk.Checked = True
        Next

    End Sub

#Region "Autorisierungslevel"

    Private Function Autorisieren() As Boolean

        Dim ZurAutorisierung As Boolean = False
        'Welche Art von Versandadressen wurde ausgewählt?
        _mApp.GetAppAutLevel(_mUser.GroupID, Session("AppID").ToString)

        Dim Level() As String

        If String.IsNullOrEmpty(_mApp.AutorisierungsLevel) = False Then

            Level = Split(_mApp.AutorisierungsLevel, "|")

            'Beinhaltet das Level die Adressart?
            If Level(0).Contains(_mVersand.Adressart) Then

                'Zugehörige Autorisierungsart aus dem zweiten Array ermitteln
                Dim arrLevel() As String = Split(Level(0), ",")
                Dim arrAutorisierung() As String = Split(Level(1), ",")

                For i As Integer = 0 To arrLevel.Length - 1

                    If arrLevel(i) = _mVersand.Adressart Then
                        '1 = Autorisierung, 2 = Keine Autorisierung
                        If arrAutorisierung(i) = "1" Then ZurAutorisierung = True : Exit For

                    End If

                Next

            End If

        End If

        Return ZurAutorisierung

    End Function

#End Region

    Protected Sub ibtnCreatePDF_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs) Handles ibtnCreatePDF.Click

        Dim tmpDataView As DataView = _mVersand.Fahrzeuge.DefaultView
        tmpDataView.RowFilter = "Selected = '1'"

        Select Case _mVersand.Adressart
            Case 1, 4
                _mVersand.AdressartText = "Adressauswahl"
            Case 2, 5
                _mVersand.AdressartText = "Zulassungsstellen"
            Case 3, 6
                _mVersand.AdressartText = "manuelle Adresseingabe"
        End Select

        _mVersand.VersandoptionenText = lblOptionsOverViewShow.Text.Replace("<br />", vbCrLf)
        _mVersand.VersandAdresseText = _mVersand.VersandAdresseText.Replace(" <br /> ", vbCrLf)
        _mVersand.FahrzeugePrint = tmpDataView.ToTable

        Dim headTable As New DataTable("Kopf")

        headTable.Columns.Add("Beauftragungsdatum", GetType(System.String))
        headTable.Columns.Add("Sachbearbeiter", GetType(System.String))
        headTable.Columns.Add("VersartText", GetType(System.String))
        headTable.Columns.Add("AdressartText", GetType(System.String))
        headTable.Columns.Add("VersandadresseText", GetType(System.String))
        headTable.Columns.Add("VersgrundText", GetType(System.String))
        headTable.Columns.Add("Bemerkung", GetType(System.String))
        headTable.Columns.Add("Halter", GetType(System.String))
        headTable.Columns.Add("VersandoptionenText", GetType(System.String))
        headTable.Columns.Add("AutorisierungText", GetType(System.String))

        headTable.AcceptChanges()

        Dim dr As DataRow = headTable.NewRow

        dr("Beauftragungsdatum") = Date.Now().ToShortDateString
        dr("Sachbearbeiter") = _mVersand.Sachbearbeiter
        dr("VersartText") = _mVersand.VersartText
        dr("AdressartText") = _mVersand.AdressartText
        dr("VersandadresseText") = _mVersand.VersandAdresseText
        dr("VersgrundText") = _mVersand.VersgrundText
        dr("Bemerkung") = _mVersand.Bemerkung
        dr("Halter") = _mVersand.Halter
        dr("VersandoptionenText") = _mVersand.VersandoptionenText
        dr("AutorisierungText") = _mVersand.AutorisierungText

        headTable.Rows.Add(dr)
        headTable.AcceptChanges()

        _mVersand.FahrzeugePrint.TableName = "Fahrzeuge"

        Dim imageHt As New Hashtable()
        Try
            imageHt.Add("Logo", _mUser.Customer.LogoImage)
        Catch ex As Exception
            ' LogoPath am Customer nicht (korrekt) gepflegt - hier: ignorieren
        End Try

        Dim docFactory As New DocumentGeneration.WordDocumentFactory(_mVersand.FahrzeugePrint, imageHt)
        docFactory.CreateDocumentTable("Versandauftrag_" & _mUser.UserName, Page, "\Components\ComCommon\Documents\VersandZB2.doc", headTable)

    End Sub

    Protected Sub cmdOKWarnung_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdOKWarnung.Click
        divMessage.Visible = False
        divBackDisabled.Visible = False
        NextToChoice()
    End Sub

    Private Sub InApp()

        If _mUser.Applications.Select("AppURL = '../Components/ComCommon/Change05s.aspx'").Length > 0 Then
            ibtEdit.Visible = True
        End If

    End Sub

    Protected Sub ibtEdit_Click(sender As Object, e As ImageClickEventArgs) Handles ibtEdit.Click

        If ddlAdresse.SelectedIndex > 0 Then
            Dim Name As String
            Dim Ident As String

            Ident = ddlAdresse.SelectedValue
            Name = _mVersand.Adressen.Select("IDENT = '" & Ident & "'")(0)("NAME1").ToString

            Session.Add("me", Me)

            _mVersand.VersandArt = IIf(rb_temp.Checked, "1", "2")
            Session("App_Versand") = _mVersand

            Response.Redirect("../Change05s.aspx?AppID=" & Session("AppID").ToString & "&ident=" & Ident & "&Name=" & Name & "&eqtyp=B")
        Else
            lblSucheAdr.Visible = True
            lblSucheAdr.Text = "Bitte wählen Sie eine Adresse aus!"
        End If
    End Sub

    Private Sub Reset()

        Try
            Dim MeControls As Change99 = CType(Session("me"), Change99)

            _mVersand = Session("App_Versand")

            lblErrorVersandOpt.Text = ""

            VersandTabPanel1.Visible = False
            VersandTabPanel2.Visible = True
            lbtnStammdaten.CssClass = "VersandButtonStammReady"
            lbtnAdressdaten.CssClass = "VersandButtonAdresse"
            lblSteps.Text = "Schritt 2 von 4"
            Panel2.CssClass = "StepActive"

            If Not _mVersand.VersandArt Is Nothing Then
                If _mVersand.VersandArt = "1" Then
                    rb_temp.Checked = True
                ElseIf _mVersand.VersandArt = "2" Then
                    rb_endg.Checked = True
                End If

            End If

            trZulStelleSuche.Visible = False
            trFreieAdresse.Visible = False
            _mApp.GetAppAutLevel(_mUser.GroupID, Session("AppID").ToString)

            Dim Level() As String

            If String.IsNullOrEmpty(_mApp.AutorisierungsLevel) = False Then

                Level = Split(_mApp.AutorisierungsLevel, "|")
                Level = Split(Level(0), ",")

                rb_temp.Visible = False
                rb_endg.Visible = False

                For i As Integer = 0 To Level.Length - 1

                    Select Case Level(i)

                        Case 1, 2, 3
                            rb_temp.Visible = True
                        Case 4, 5, 6
                            rb_endg.Visible = True
                    End Select

                Next
            End If

            If rb_temp.Visible = True AndAlso rb_endg.Visible = False Then
                rb_temp.Checked = True
            ElseIf rb_temp.Visible = False AndAlso rb_endg.Visible = True Then
                rb_endg.Checked = True
            End If

            RadioButtonVersandChanged()

            cpeAdressSuche.Collapsed = False
            ibtEdit.Visible = True
            trddlAdresse.Visible = True
            trAdressuche.Visible = True

            Dim tmpItem As ListItem

            For Each Item As ListItem In MeControls.ddlAdresse.Items
                tmpItem = Item

                ddlAdresse.Items.Add(tmpItem)

            Next

            ddlAdresse.SelectedValue = MeControls.ddlAdresse.SelectedValue
            lblSucheAdr.Visible = False


            Dim Mode As String = Request.QueryString.Item("mode").ToString

            If Mode = "success" Then
                _mVersand.GetAdressenandZulStellen(Session("AppID").ToString, Session.SessionID.ToString, Me)
                Session("App_Versand") = _mVersand
                AdressChoice()
            End If

        Catch ex As Exception
            Session("me") = Nothing
        Finally
            Session("me") = Nothing
        End Try

    End Sub

    Protected Sub ibtExtendSearch_Click(sender As Object, e As ImageClickEventArgs) Handles ibtExtendSearch.Click

        If hdnField.Value = "1" Then
            tr_Brieflieferant.Visible = False
        End If

        ibtExtendSearch.Visible = False
        tblSearch.Visible = False
        ibtBack.Visible = True
        tblSearchExtend.Visible = True

        cpeDokuAusgabe.Collapsed = True
        cpeDokuAusgabe.ClientState = Nothing
    End Sub

    Protected Sub ibtBack_Click(sender As Object, e As ImageClickEventArgs) Handles ibtBack.Click
        ibtExtendSearch.Visible = True
        ibtBack.Visible = False
        tblSearch.Visible = True
        tblSearchExtend.Visible = False

        cpeDokuAusgabe.Collapsed = True
        cpeDokuAusgabe.ClientState = Nothing
    End Sub

 
End Class