Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary

Partial Public Class Change99
    Inherits System.Web.UI.Page
    Private m_App As App
    Private m_User As User
    Private m_Versand As Briefversand

    Protected WithEvents GridNavigation1 As CKG.Services.GridNavigation
    Protected WithEvents GridNavigation2 As CKG.Services.GridNavigation

    Private Enum Adressarten
        TempSuche = 1
        TempZulassungsstelle = 2
        TempManuell = 3
        EndSuche = 4
        EndZulassungsstelle = 5
        EndManuell = 6
    End Enum

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)
        m_App = New App(m_User)
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
            m_Versand = CType(Session("App_Versand"), Briefversand)
        End If

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
        HelpProcedures.FixedGridViewCols(GridView1)
        HelpProcedures.FixedGridViewCols(GridView2)
        HelpProcedures.FixedGridViewCols(GridView3)
        If lbl_ExtendSearch.Visible = False Then
            ibtExtendSearch.Visible = False
        End If
        lbl_ExtendSearch.Text = ""
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Protected Sub ImageButton2_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton2.Click

        'GridViews zurücksetzen
        Dim DummyTable As New DataTable
        GridView1.DataSource = DummyTable
        GridView1.DataBind()
        GridView2.DataSource = DummyTable
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
            If m_Versand.Fahrzeuge.Rows.Count = 0 AndAlso m_Versand.FahrzeugeFehler.Rows.Count = 0 Then
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
        m_Versand = New Briefversand(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString,  "")

        m_Versand.Fahrgestellnr = txtFahrgestellnummer.Text.Trim
        If txtKennz.Text.Trim.Length > 0 Then
            m_Versand.Kennzeichen = txtKennz.Text.Trim.ToUpper
        End If

        m_Versand.LVnr = txtLeasingvertragsnummer.Text.Trim
        m_Versand.ZBIINr = txtZBIINummer.Text.Trim
        m_Versand.Ref1 = txtReferenznummer1.Text.Trim
        m_Versand.Ref2 = txtReferenznummer2.Text.Trim
        m_Versand.EQuiTyp = "B"

        'Erweiterte Selektion
        With m_Versand

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

        m_Versand.FILL(Session("AppID").ToString, Session.SessionID.ToString, Me)

        If m_Versand.Status > 0 Then

        Else

            FillGrid(0)
            FillGridFehler(0)
            Session("App_Versand") = m_Versand
        End If

    End Sub

    Private Sub DoSubmit2()
        m_Versand.EQuiTyp = "B"
        m_Versand.FILL(Session("AppID").ToString, Session.SessionID.ToString, Me, True)
        If m_Versand.Status = 0 Then
            FillGrid(0)
            FillGridFehler(0)
            Session("App_Versand") = m_Versand
        End If
    End Sub

    Private Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub GridNavigation1_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation1.PagerChanged
        GridView1.PageIndex = PageIndex
        FillGrid(PageIndex)
    End Sub

    Private Sub GridNavigation1_PageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillGrid(0)
    End Sub

    Private Sub Gridview1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
        FillGrid(GridView1.PageIndex, e.SortExpression)
    End Sub

    Private Sub GridView2_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView2.PageIndexChanging
        FillGridFehler(e.NewPageIndex)
    End Sub

    Private Sub GridNavigation2_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation2.PagerChanged
        GridView2.PageIndex = PageIndex
        FillGridFehler(PageIndex)
    End Sub

    Private Sub GridNavigation2_PageSizeChanged() Handles GridNavigation2.PageSizeChanged
        FillGridFehler(0)
    End Sub

    Private Sub Gridview2_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView2.Sorting
        FillGridFehler(GridView1.PageIndex, e.SortExpression)
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")

        CheckGridFahrzeuge()

        Dim tmpDataView As New DataView()
        tmpDataView = m_Versand.Fahrzeuge.DefaultView
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

                Dim strHistoryLink As [String] = ""
                Dim lnkFahrgestellnummer As HyperLink
                Dim lbl As Label
                If m_User.Applications.[Select]("AppName = 'Report02'").Length > 0 Then
                    strHistoryLink = "../../../applications/AppF2/forms/Report02.aspx?AppID=" & m_User.Applications.Select("AppName = 'Report02'")(0)("AppID").ToString() & "&VIN="
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
                m_App.CheckForPendingAuthorization(CInt(Session("AppID")), m_User.Organization.OrganizationId, m_User.CustomerName, sFin, m_User.IsTestUser, strInitiator,
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
        Dim tmpDataView As New DataView()
        tmpDataView = m_Versand.FahrzeugeFehler.DefaultView
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
        Dim tmpDataView As New DataView()
        tmpDataView = m_Versand.Fahrzeuge.DefaultView
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
        If m_Versand Is Nothing Then
            m_Versand = New Briefversand(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
        End If
        If m_Versand.Brieflieferanten Is Nothing Then
            hdnField.Value = "1"
            tr_Brieflieferant.Visible = False
            Exit Sub
        End If

        ddlBrieflieferant.DataSource = m_Versand.Brieflieferanten
        ddlBrieflieferant.DataTextField = "Adresse"
        ddlBrieflieferant.DataValueField = "KUNNR"
        ddlBrieflieferant.DataBind()

    End Sub

    Private Sub fillLaenderDLL()
        Dim sprache As String
        'Länder DLL füllen
        ddlLand.DataSource = m_Versand.Laender
        'ddlLand.DataTextField = "Beschreibung"
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

    Private Sub ibtNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ibtNext.Click
        If CheckGridFahrzeuge() = True Then
            If m_Versand.Fahrzeuge.Select("Selected = '1'").Length > 1 Then
                lblMsgHeader.Text = "Ausgewählte Fahrzeuge"
                litMessage.Text = "Sie haben " & m_Versand.Fahrzeuge.Select("Selected = '1'").Length & " Briefe für den Versand ausgewählt."
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
            m_Versand.EqTyp = "B"
            m_Versand.GetAdressenandZulStellen(Session("AppID").ToString, Session.SessionID.ToString, Me)
            If m_Versand.Status <> 0 Then
                lblErrorVersandOpt.Text = m_Versand.Message
            Else
                fillLaenderDLL()
                Session("App_Versand") = m_Versand
            End If
            If Not m_Versand.VersandArt Is Nothing Then
                If m_Versand.VersandArt = "1" Then
                    rb_temp.Checked = True
                ElseIf m_Versand.VersandArt = "2" Then
                    rb_endg.Checked = True
                End If

            End If
            trAdressuche.Visible = False
            trZulStelleSuche.Visible = False
            trFreieAdresse.Visible = False
            m_App.GetAppAutLevel(m_User.GroupID, Session("AppID").ToString)

            Dim Level() As String

            If String.IsNullOrEmpty(m_App.AutorisierungsLevel) = False Then

                Level = Split(m_App.AutorisierungsLevel, "|")
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


            'If m_App.AppAutLevel >= 4 Then
            '    rb_endg.Visible = True
            'Else
            '    rb_endg.Visible = False
            'End If

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
                m_Versand.Fahrzeuge.Select("EQUNR = '" + lblEQUNR.Text + "'")(0)("Selected") = "1"
                'Else
                '    m_Versand.Fahrzeuge.Select("EQUNR = '" + lblEQUNR.Text + "'")(0)("Selected") = ""
            End If
        Next
        Return m_Versand.Fahrzeuge.Select("Selected = '1'").Any()
    End Function

    Protected Sub ibtnSearchAdresse_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSearchAdresse.Click
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

            'Dim sQuery As String = ""

            'If strFirma.Length > 0 Then
            '    sQuery += "NAME1 LIKE '" & strFirma.Trim & "' AND "
            'End If

            'If strStrasse.Length > 0 Then
            '    sQuery += "STREET LIKE '" & strStrasse.Trim & "' AND "
            'End If

            'If strOrt.Length > 0 Then
            '    sQuery += "CITY1 LIKE '" & strOrt.Trim & "' AND "
            'End If

            'If strPlz.Length > 0 Then
            '    sQuery += "POST_CODE1 LIKE '" & strPlz.Trim & "' AND "
            'End If

            'If strLand.Length > 0 Then
            '    sQuery += "COUNTRY LIKE '" & strLand.Trim & "' AND "
            'End If
            'sQuery = Left(sQuery, sQuery.Length - 4)

            m_Versand.sReferenz = strReferenz
            m_Versand.sName1 = strFirma
            m_Versand.sName2 = txtName2.Text
            m_Versand.sStrasse = strStrasse
            m_Versand.sPLZ = strPlz
            m_Versand.sOrt = strOrt


            m_Versand.GetAdressen(Session("AppID").ToString, Session.SessionID.ToString, Me.Page)


            Dim dv As New DataView
            dv = m_Versand.Adressen.DefaultView
            'dv.RowFilter = sQuery
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

    Protected Sub ibtn_SucheGesch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtn_SucheGesch.Click
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


            Dim dv As New DataView
            dv = m_Versand.ZulStellen.DefaultView
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
        If m_Versand.VersandAdresseText <> String.Empty Then

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
            m_Versand.OptionFlag = "3"

            Dim strVersandart As String = IIf(rb_temp.Checked, "1", "2")

            m_Versand.GetVersandOptions(Session("AppID").ToString, Session.SessionID.ToString, Me)

            'Versandoption "Versand ohne Abmeldung" ggf. rausfiltern, sonst als Inverses "Auf Abmeldung warten" weiter nutzen
            'NewLevel beinhaltet 2 Arrays: Level-Array und Autorisierungsarray(1 zu 1) getrennt durch |
            Dim blnLevel7 As Boolean = False
            Dim strLevelText = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("NewLevel")
            If Not IsDBNull(strLevelText) AndAlso Not String.IsNullOrEmpty(strLevelText) Then
                Dim strLevel = Split(m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("NewLevel"), "|")(0)
                Dim levels() As String = strLevel.Split(",")
                blnLevel7 = levels.Contains("7")
            End If

            Dim updRows As DataRow() = m_Versand.VersandOptionen.Select("EXTGROUP='" & strVersandart & "' AND EAN11 = 'ZZABMELD'")
            If strVersandart = "1" Or Not blnLevel7 Then
                For Each updRow In updRows
                    m_Versand.VersandOptionen.Rows.Remove(updRow)
                Next
            Else
                For Each updRow In updRows
                    updRow("ASKTX") = "Auf Abmeldung warten"
                    updRow("EAN11") = "ZZABMELD_INVERTED"
                Next
            End If

            If m_Versand.Status <> 0 Then
                lblErrorAdressen.Visible = True
                lblErrorAdressen.Text = m_Versand.Message
            Else
                Session("App_Versand") = m_Versand

                m_Versand.VersandOptionen.DefaultView.RowFilter = "EXTGROUP='" & strVersandart & "' AND INTROW <> '0000000000'"

                chkListGruende.DataSource = m_Versand.VersandOptionen.DefaultView
                chkListGruende.DataValueField = "EAN11"
                chkListGruende.DataTextField = "ASKTX"
                chkListGruende.DataBind()

                grvDL.Columns(2).Visible = True
                grvDL.Columns(3).Visible = True

                grvDL.DataSource = m_Versand.VersandOptionen.DefaultView
                grvDL.DataBind()

                If m_Versand.VersandOptionen.Select("Selected = '1'").Length > 0 Then
                    m_Versand.VersandOptionen.DefaultView.RowFilter = "EXTGROUP='" & strVersandart & "' AND INTROW <> '0000000000' AND Selected = '1'"
                    chkGruende.DataSource = m_Versand.VersandOptionen.DefaultView
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
                m_Versand.VersandArt = strVersandart
                m_Versand.GetAbrufgrund(Session("AppID").ToString, Session.SessionID.ToString, Me)

                If m_Versand.Status <> 0 Then
                    lblErrorAdressen.Visible = True
                    lblErrorAdressen.Text = m_Versand.Message
                Else
                    ddlVersandgrund.Items.Clear()
                    Dim NewItem As New ListItem
                    NewItem.Text = "- Bitte wählen -"
                    NewItem.Value = "0"
                    NewItem.Selected = True
                    ddlVersandgrund.Items.Add(NewItem)
                    For Each dRow As DataRow In m_Versand.Versandgruende.Rows
                        NewItem = New ListItem
                        NewItem.Text = dRow("VGRUND_TEXT").ToString
                        NewItem.Value = dRow("ZZVGRUND").ToString
                        ddlVersandgrund.Items.Add(NewItem)
                    Next

                    If ddlVersandgrund.Items.Count = 2 Then
                        ddlVersandgrund.SelectedIndex = 1
                    End If

                    If Not m_Versand.VersandGrund Is Nothing Then
                        ddlVersandgrund.SelectedValue = m_Versand.VersandGrund
                    End If

                    txtBemerkung.Text = m_Versand.Bemerkung
                    txtHalter.Text = m_Versand.Halter
                    ConfirmNextToOptions.Enabled = False
                End If
            End If
        Else
            lblErrorAdressen.Visible = True
            lblErrorAdressen.Text = "Bitte wählen Sie eine Versandadresse aus!"
        End If
    End Sub

    Private Sub ibtnNextToOverView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ibtnNextToOverView.Click
        lblErrorVersandOpt.Text = ""
        If ddlVersandgrund.SelectedIndex = 0 Then
            lblErrorVersandOpt.Text += "Bitte wählen Sie einen Versandgrund aus!<br />"
            lblErrorVersandOpt.Visible = True
        Else
            m_Versand.VersandGrund = ddlVersandgrund.SelectedItem.Value

            Select Case ddlVersandgrund.SelectedItem.Value
                Case "001", "005"
                    If rb_temp.Checked = True AndAlso Trim(txtHalter.Text).Length = 0 Then
                        lblErrorVersandOpt.Text += "Bitte geben Sie einen Halter ein!<br />"
                        lblErrorVersandOpt.Visible = True
                    End If
            End Select

        End If

        m_Versand.Halter = txtHalter.Text
        m_Versand.Bemerkung = txtBemerkung.Text

        Dim bAuswahlNormal As Boolean = False
        Dim bAufAbmeldungWarten = False
        For Each litem As ListItem In chkGruende.Items
            If litem.Selected Then
                If litem.Value = "ZZABMELD_INVERTED" Then
                    bAufAbmeldungWarten = True
                Else
                    m_Versand.Materialnummer = litem.Value
                    bAuswahlNormal = True
                End If
            End If
        Next

        'Nur wenn Versandoption "Auf Abmeldung warten" explizit gewählt ist, kein "Versand ohne Abmeldung"
        If bAufAbmeldungWarten Then
            m_Versand.VersohneAbeld = ""
        Else
            m_Versand.VersohneAbeld = "X"
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
            ConfirmNextToOverview.Enabled = False
            FillGridOverView(0)
            FillOverView()
        End If


    End Sub

    Protected Sub ibtnShowOptions_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnShowOptions.Click

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

    Private Sub lbtnCloseOption_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnCloseOption.Click
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
                m_Versand.VersandOptionen.Select("EXTGROUP='" + tempEndg + "' AND EAN11 = '" + lbl.Text + "'")(0)("Selected") = "1"
            Else
                m_Versand.VersandOptionen.Select("EXTGROUP='" + tempEndg + "' AND EAN11 = '" + lbl.Text + "'")(0)("Selected") = "0"
            End If
            m_Versand.VersandOptionen.AcceptChanges()
        Next

        Dim bvalidate As Boolean = True
        lblErrPopUp.Visible = False

        Dim bAufAbmeldungWarten As Boolean = False

        Dim drows() As DataRow = m_Versand.VersandOptionen.Select("EXTGROUP='" + tempEndg + "'  AND Selected = '1'")
        If drows.Length > 0 Then
            For Each dRowSel As DataRow In drows
                If dRowSel("ALTERNAT").ToString = "X" Then
                    Dim drowsBasis() As DataRow
                    drowsBasis = m_Versand.VersandOptionen.Select("EXTGROUP='" + tempEndg + "'  AND Selected = '1' AND INTROW='" + dRowSel("ALT_INTROW").ToString + "'")
                    If drowsBasis.Length > 0 Then
                        bvalidate = False
                        lblErrPopUp.Visible = True
                        lblErrPopUp.Text = "Die ausgewählte Option """ + dRowSel("ASKTX").ToString + """ steht im Konflikt mit der Option """ + _
                        drowsBasis(0)("ASKTX").ToString + """. Bitte wählen Sie eine Option ab!"
                    Else
                        drowsBasis = m_Versand.VersandOptionen.Select("EXTGROUP='" + tempEndg + "'  AND Selected = '1' AND ALT_INTROW='" + _
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

                If dRowSel("EAN11").ToString() = "ZZABMELD_INVERTED" Then
                    bAufAbmeldungWarten = True
                End If
            Next
        End If

        'wenn "Aus Abmeldung warten" bewusst gewählt wurde -> Warnhinweis
        ConfirmNextToOverview.Enabled = bAufAbmeldungWarten

        If bvalidate = True Then
            m_Versand.VersandOptionen.DefaultView.RowFilter = IIf(rb_temp.Checked, "EXTGROUP='1'", "EXTGROUP='2'")
            m_Versand.VersandOptionen.DefaultView.RowFilter += " AND Selected = '1'"
            chkGruende.DataSource = m_Versand.VersandOptionen.DefaultView
            chkGruende.DataValueField = "EAN11"
            chkGruende.DataTextField = "ASKTX"
            chkGruende.DataBind()

            For Each litem As ListItem In chkGruende.Items
                litem.Selected = True
            Next

            divOptions.Visible = False
            divBackDisabled.Visible = False
        End If

    End Sub

    Protected Sub ibtnSucheSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ibtnSucheSave.Click
        AdressChoice()
    End Sub

    Private Sub AdressChoice()
        m_Versand.VersandAdresseText = ""
        If ddlAdresse.SelectedIndex > 0 Then
            Dim AdrRow As DataRow = m_Versand.Adressen.Select("IDENT = '" + ddlAdresse.SelectedValue + "'")(0)

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
            m_Versand.VersandAdresseText = lbl_SelAdresseShow.Text

            'jetzt immer die komplette Adresse mitgeben
            m_Versand.VersandAdresse_ZE = String.Empty
            'jetzt Debitornummer (SAPNR) weitergeben
            m_Versand.VersandAdresse_ZS = AdrRow("SAPNR").ToString

            'Manuelle Adresse
            m_Versand.Name1 = AdrRow("NAME1").ToString
            m_Versand.Name2 = AdrRow("NAME2").ToString
            m_Versand.Street = AdrRow("STREET").ToString
            m_Versand.HouseNum = AdrRow("HOUSE_NUM1").ToString
            m_Versand.PostCode = AdrRow("POST_CODE1").ToString
            m_Versand.City = AdrRow("CITY1").ToString
            m_Versand.laenderKuerzel = AdrRow("COUNTRY").ToString

            If rb_temp.Checked = True Then
                m_Versand.Adressart = Adressarten.TempSuche
            Else
                m_Versand.Adressart = Adressarten.EndSuche
                ConfirmNextToOptions.Enabled = m_Versand.Fahrzeuge.Select("Selected = '1' AND Abmeldedatum IS NULL").Any()
            End If
        Else
            lblSucheAdr.Visible = True
            lblSucheAdr.Text = "Bitte wählen Sie eine Adresse aus!"
        End If
    End Sub

    Protected Sub ibtnSucheGeschSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ibtnSucheGeschSave.Click

        If ddlZulStelle.SelectedIndex > 0 Then
            m_Versand.VersandAdresseText = ""
            Dim ZulRow As DataRow = m_Versand.ZulStellen.Select("LIFNR = '" + ddlZulStelle.SelectedValue + "'")(0)

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

            m_Versand.VersandAdresse_ZE = ddlZulStelle.SelectedItem.Value      'Versandadresse Nr. (60...)
            m_Versand.VersandAdresseText = lbl_SelAdresseShow.Text  'Versanddresse (Text...)

            'jetzt immer die komplette Adresse mitgeben
            m_Versand.VersandAdresse_ZS = String.Empty

            'Manuelle Adresse nullen
            m_Versand.Name1 = String.Empty
            m_Versand.Name2 = String.Empty
            m_Versand.Street = String.Empty
            m_Versand.HouseNum = String.Empty
            m_Versand.PostCode = String.Empty
            m_Versand.City = String.Empty
            m_Versand.laenderKuerzel = String.Empty

            If rb_temp.Checked = True Then
                m_Versand.Adressart = Adressarten.TempZulassungsstelle
            Else
                m_Versand.Adressart = Adressarten.EndZulassungsstelle
                ConfirmNextToOptions.Enabled = m_Versand.Fahrzeuge.Select("Selected = '1' AND Abmeldedatum IS NULL").Any()
            End If

        End If
    End Sub

    Protected Sub ibtnSucheManuellSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSucheManuellSave.Click
        m_Versand.VersandAdresseText = ""
        lblErrorAdrManuell.Text = ""
        Dim strAdresse As String = ""


        If txtFirmaManuell.Text.Trim(" "c).Length = 0 Then
            lblErrorAdrManuell.Text &= "Bitte ""Name"" eingeben!<br>&nbsp;"
        Else
            m_Versand.Name1 = txtFirmaManuell.Text.Trim(" "c)
            strAdresse = txtFirmaManuell.Text.Trim(" "c) & ", "
        End If
        If txtPlzManuell.Text.Trim(" "c).Length = 0 Then
            lblErrorAdrManuell.Text &= "Bitte ""PLZ"" eingeben!<br>&nbsp;"
        Else
            If CInt(m_Versand.Laender.Select("Land1='" & ddlLand.SelectedItem.Value & "'")(0)("Lnplz")) > 0 Then
                If Not CInt(m_Versand.Laender.Select("Land1='" & ddlLand.SelectedItem.Value & "'")(0)("Lnplz")) = txtPlzManuell.Text.Trim(" "c).Length Then
                    lblError.Text = "Postleitzahl hat falsche Länge."
                Else
                    m_Versand.PostCode = txtPlzManuell.Text.Trim(" "c)
                    strAdresse = strAdresse & ddlLand.SelectedItem.Value & "-" & txtPlzManuell.Text.Trim(" "c) & " "
                End If
            End If

        End If
        If txtOrtManuell.Text.Trim(" "c).Length = 0 Then
            lblErrorAdrManuell.Text &= "Bitte ""Ort"" eingeben!<br>"
        Else
            m_Versand.City = txtOrtManuell.Text.Trim(" "c)
            strAdresse = strAdresse & txtOrtManuell.Text.Trim(" "c) & ", "
        End If
        If txtStrasseManuell.Text.Trim(" "c).Length = 0 Then
            lblErrorAdrManuell.Text &= "Bitte ""Strasse"" eingeben!<br>"
        Else
            m_Versand.Street = txtStrasseManuell.Text.Trim(" "c)
            strAdresse = strAdresse & txtStrasseManuell.Text.Trim(" "c) & " "
        End If
        If txtNrManuell.Text.Trim(" "c).Length = 0 Then
            lblErrorAdrManuell.Text &= "Bitte ""Nummer"" eingeben!<br>"
        Else
            m_Versand.HouseNum = txtNrManuell.Text.Trim(" "c)
            strAdresse = strAdresse & txtNrManuell.Text.Trim(" "c)
        End If
        m_Versand.Name2 = txtName2.Text.Trim(" "c)
        If lblErrorAdrManuell.Text = "" Then
            m_Versand.laenderKuerzel = ddlLand.SelectedItem.Value

            lbl_SelAdresseShow.Text = m_Versand.Name1 + ", " + m_Versand.Name2 + " <br /> " + _
                                      m_Versand.Street + " " + m_Versand.HouseNum + " <br /> " + _
                                      m_Versand.PostCode + " " + m_Versand.City
            lbl_SelAdresse.Text = "Freie Adresse:"
            'SAP-Adresse nullen
            m_Versand.VersandAdresse_ZS = String.Empty
            m_Versand.VersandAdresseText = lbl_SelAdresseShow.Text
            'Zulassungsstelle nullen
            m_Versand.VersandAdresse_ZE = String.Empty


            If rb_temp.Checked = True Then
                m_Versand.Adressart = Adressarten.TempManuell
            Else
                m_Versand.Adressart = Adressarten.EndManuell
                ConfirmNextToOptions.Enabled = m_Versand.Fahrzeuge.Select("Selected = '1' AND Abmeldedatum IS NULL").Any()
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


        If Me.Request.Url.ToString.Contains("mode") = True Then

            GridView1.Visible = False
            data.Visible = False
            Result.Visible = False
            GridNavigation1.Visible = False

            GridView2.Visible = False
            data2.Visible = False
            GridNavigation2.Visible = False

            If Not m_Versand Is Nothing Then

                If Not m_Versand.Fahrzeuge Is Nothing Then
                    If m_Versand.Fahrzeuge.DefaultView.Count > 0 Then
                        FillGrid(0)
                    End If
                    cpeAllData.Collapsed = True
                    cpeUpload.Collapsed = True
                End If

                If Not m_Versand.FahrzeugeFehler Is Nothing Then
                    If m_Versand.FahrzeugeFehler.DefaultView.Count > 0 Then
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
        m_App.GetAppAutLevel(m_User.GroupID, Session("AppID").ToString)



        'Authorizationright wird von der Autorisierung auf Levelebene übersteuert
        Dim ZurAutorisierung As Boolean = False
        If String.IsNullOrEmpty(m_App.AutorisierungsLevel) = False Then
            ZurAutorisierung = Autorisieren()
        Else
            If m_User.Groups.ItemByID(m_User.GroupID).Authorizationright > 0 Then ZurAutorisierung = True
        End If

        If ZurAutorisierung = True Then

            For Each tmpRow As DataRow In m_Versand.Fahrzeuge.Rows
                If tmpRow("Selected").ToString = "1" Then
                    Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
                    logApp.CollectDetails("Fahrgestellnr.", CType(tmpRow("Fahrgestellnummer").ToString, Object), True)
                    logApp.CollectDetails("Nummer ZBII", CType(tmpRow("NummerZBII").ToString, Object))
                    logApp.CollectDetails("Leasingnummer", CType(tmpRow("Leasingnummer").ToString, Object))
                    logApp.CollectDetails("Referenz1", CType(tmpRow("Referenz1").ToString, Object))
                    logApp.CollectDetails("Referenz2", CType(tmpRow("Referenz2").ToString, Object))
                    logApp.CollectDetails("Versandart", CType(lblVersArtOverviewShow.Text, Object))
                    logApp.CollectDetails("Versandgrund", CType(lblGrundOverviewShow.Text, Object))
                    logApp.CollectDetails("Versandoption", CType(lblOptionsOverViewShow.Text, Object))
                    logApp.CollectDetails("Sachbearbeiter", CType(m_User.UserName, Object))
                    logApp.CollectDetails("Bemerkung", CType(txtBemerkung.Text, Object))
                    logApp.CollectDetails("Halter", CType(txtHalter.Text, Object))


                    m_Versand.Sachbearbeiter = m_User.UserName
                    m_Versand.ReferenceforAut = tmpRow("EQUNR").ToString
                    m_Versand.VersgrundText = lblGrundOverviewShow.Text
                    m_Versand.Bemerkung = txtBemerkung.Text
                    m_Versand.Halter = txtHalter.Text
                    m_Versand.Beauftragungsdatum = Date.Today.ToShortDateString
                    m_Versand.VersartText = lblVersArtOverviewShow.Text
                    m_Versand.Briefversand = "1"
                    m_Versand.SchluesselVersand = ""
                    m_Versand.OptionFlag = "3"
                    Dim DetailArray(1, 2) As Object
                    Dim ms As MemoryStream
                    Dim formatter As BinaryFormatter
                    Dim b() As Byte

                    ms = New MemoryStream()
                    formatter = New BinaryFormatter()
                    formatter.Serialize(ms, m_Versand)
                    b = ms.ToArray
                    ms = New IO.MemoryStream(b)
                    DetailArray(0, 0) = ms
                    DetailArray(0, 1) = "VersandObject"

                    'Pruefen, ob schon in der Autorisierung.
                    Dim strInitiator As String = ""
                    Dim intAuthorizationID As Int32


                    m_App.CheckForPendingAuthorization(CInt(Session("AppID")), m_User.Organization.OrganizationId, m_User.CustomerName, tmpRow("Fahrgestellnummer").ToString,
                                                       m_User.IsTestUser, strInitiator, intAuthorizationID)
                    If Not strInitiator.Length = 0 Then
                        tmpRow("Status") = "liegt zur Autorisierung vor"
                    Else
                        intAuthorizationID = WriteAuthorization(m_App.Connectionstring, CInt(Session("AppID")), m_User.UserName, m_User.Organization.OrganizationId,
                                                                m_User.CustomerName, tmpRow("Fahrgestellnummer").ToString, "", "", m_User.IsTestUser, DetailArray)
                        logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")),
                                          m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString,
                                          tmpRow("Fahrgestellnummer").ToString, "Briefversand für " & tmpRow("Fahrgestellnummer").ToString & " erfolgreich initiiert.",
                                          m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0, logApp.InputDetails)
                    End If

                    Session("App_Versand") = m_Versand
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
                    Session("App_Versand") = m_Versand
                End If

            Next
            m_Versand.AutorisierungText = "mit Autorisierung"

            Session("App_Versand") = m_Versand

            ibtnCreatePDF.Visible = True
            lblPDFPrint.Visible = True

        Else
            m_Versand.Briefversand = "1"
            m_Versand.SchluesselVersand = ""
            m_Versand.Anfordern(Session("AppID").ToString, Session.SessionID.ToString, Me)
            If m_Versand.Status <> 0 Then
                lblErrorAnfordern.Visible = True
                lbtnSend.Enabled = False
                lblErrorAnfordern.Text = m_Versand.Message
                FillGridOverView(0)
            Else

                m_Versand.AutorisierungText = "ohne Autorisierung"
                m_Versand.VersartText = lblVersArtOverviewShow.Text
                m_Versand.Sachbearbeiter = m_User.UserName
                m_Versand.VersgrundText = lblGrundOverviewShow.Text

                Session("App_Versand") = m_Versand
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
        m_Versand.VersandAdresseText = String.Empty

        m_Versand.VersandAdresse_ZE = String.Empty
        m_Versand.VersandAdresseText = String.Empty

        'SAP-Adresse nullen
        m_Versand.VersandAdresse_ZS = String.Empty

        'Manuelle Adresse nullen
        m_Versand.Name1 = String.Empty
        m_Versand.Name2 = String.Empty
        m_Versand.Street = String.Empty
        m_Versand.HouseNum = String.Empty
        m_Versand.PostCode = String.Empty
        m_Versand.City = String.Empty
        m_Versand.laenderKuerzel = String.Empty

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
        m_Versand = Nothing
        Session("m_Versand") = Nothing
        Response.Redirect("Change99.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub

    Protected Sub ibtnUpload_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnUpload.Click

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
        m_Versand = New Briefversand(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
        m_Versand.CreateUploadTable()
        'Lade Datei
        upload(upFile1.PostedFile)

    End Sub

    Private Sub upload(ByVal uFile As System.Web.HttpPostedFile)

        'Dim filepath As String = ConfigurationManager.AppSettings("ExcelPath")
        'Dim filename As String = ""
        'Dim info As System.IO.FileInfo

        ''Dateiname: User_yyyyMMddhhmmss.xls
        'If Right(upFile1.PostedFile.FileName.ToUpper, 4) = ".XLS" Then
        '    filename = m_User.UserName & "_" & Format(Now, "yyyyMMddhhmmss") & ".xls"
        'End If
        'If Right(upFile1.PostedFile.FileName.ToUpper, 5) = ".XLSX" Then
        '    filename = m_User.UserName & "_" & Format(Now, "yyyyMMddhhmmss") & ".xlsx"
        'End If
        Try
            If Not (uFile Is Nothing) Then
                'uFile.SaveAs(ConfigurationManager.AppSettings("ExcelPath") & filename)
                'info = New System.IO.FileInfo(filepath & filename)
                'If Not (info.Exists) Then
                '    lblError.Text = "Fehler beim Speichern."
                '    Exit Sub
                'End If

                'Dim sConnectionString As String = ""
                'If Right(upFile1.PostedFile.FileName.ToUpper, 4) = ".XLS" Then
                '    sConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" & _
                '     "Data Source=" & filepath & filename & ";" & _
                '     "Extended Properties=""Excel 8.0;HDR=YES;"""
                'Else
                '    sConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + filepath + filename + _
                '    ";Extended Properties=""Excel 12.0 Xml;HDR=YES"""
                'End If

                'Dim objConn As New OleDbConnection(sConnectionString)
                'objConn.Open()

                'Dim objCmdSelect As New OleDbCommand("SELECT * FROM [Tabelle1$]", objConn)

                'Dim objAdapter1 As New OleDbDataAdapter()
                'objAdapter1.SelectCommand = objCmdSelect

                'Dim objDataset1 As New DataSet()
                'objAdapter1.Fill(objDataset1, "XLData")

                'CHC ITA 5972
                lblError.Text = ""
                lblErrorDokumente.Text = ""
                Dim uploadV As Upload_Validator.Validator = New Upload_Validator.Validator()
                Dim XLS_CheckTable As DataTable = uploadV.UploadXLSohneModifikation(upFile1.PostedFile, ConfigurationManager.AppSettings("ExcelPath"), m_User, lblError,
                                                                                    Session("AppID").ToString, Session.SessionID)
                If uploadV.CheckObZeilenMitMehrAlsEinemWertExistieren(XLS_CheckTable) > -1 Then
                    lblErrorDokumente.Text += "Es gibt Zeilen mit mehr als einem Wert. Bitte korrigieren Sie Ihre Datei!<br>"
                    Return
                End If

                CheckInputTable(XLS_CheckTable)
                lblErrorDokumente.Text += lblError.Text
                lblError.Text = ""
                'CheckInputTable(objDataset1.Tables(0))

                'objConn.Close()

                If m_Versand.tblUpload.Rows.Count > 0 Then
                    m_Versand.EQuiTyp = "B"
                    m_Versand.FILL(Session("AppID").ToString, Session.SessionID.ToString, Me, True)
                    If m_Versand.Status > 0 Then
                        lblErrorDokumente.Visible = True
                        lblErrorDokumente.Text += "Fehler beim hochladen der Datei! " & m_Versand.Message
                    Else
                        FillGrid(0)
                        FillGridFehler(0)
                        Session("App_Versand") = m_Versand
                        cpeAllData.ClientState = True
                        cpeUpload.ClientState = True
                        'cpeDokuAusgabe.ClientState = True
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
        Dim tblReturn As DataTable = Nothing
        Dim Fahrgestellnummer As String = ""
        Dim Kennzeichen As String = ""
        Dim NummerZB2 As String = ""
        Dim LeaseNr As String = ""
        Dim Ref1 As String = ""
        Dim Ref2 As String = ""

        'CHC ITA 5972
        Dim uploadV As Upload_Validator.Validator = New Upload_Validator.Validator()

        For Each rowData In tblInput.Rows

            If Not TypeOf rowData(0) Is System.DBNull Then
                Fahrgestellnummer = CStr(rowData(0)).Trim
            End If

            If tblInput.Columns.Count > 1 Then
                If Not TypeOf rowData(1) Is System.DBNull Then
                    'Kennzeichen = CStr(rowData(1)).Trim
                    Kennzeichen = uploadV.FindeDeutschesKennzeichen(rowData)
                End If
            End If
            If tblInput.Columns.Count > 2 Then
                If Not TypeOf rowData(2) Is System.DBNull Then
                    NummerZB2 = CStr(rowData(2)).Trim
                End If
            End If
            If tblInput.Columns.Count > 3 Then
                If Not TypeOf rowData(3) Is System.DBNull Then
                    LeaseNr = CStr(rowData(3)).Trim
                End If
            End If

            If tblInput.Columns.Count > 4 Then
                If Not TypeOf rowData(4) Is System.DBNull Then
                    Ref1 = CStr(rowData(4)).Trim(" "c)
                End If
            End If

            If tblInput.Columns.Count > 5 Then
                If Not TypeOf rowData(5) Is System.DBNull Then
                    Ref2 = CStr(rowData(5)).Trim
                End If
            End If

            If Fahrgestellnummer.Length + Kennzeichen.Length + NummerZB2.Length _
               + LeaseNr.Length + Ref1.Length + Ref2.Length > 0 Then

                Dim UploadRow As DataRow

                UploadRow = m_Versand.tblUpload.NewRow
                UploadRow("CHASSIS_NUM") = Fahrgestellnummer
                UploadRow("LICENSE_NUM") = Kennzeichen
                UploadRow("TIDNR") = NummerZB2
                UploadRow("LIZNR") = LeaseNr
                UploadRow("ZZREFERENZ1") = Ref1
                UploadRow("ZZREFERENZ2") = Ref2
                m_Versand.tblUpload.Rows.Add(UploadRow)

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
        m_Versand = New Briefversand(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
        m_Versand.CreateUploadTable()

        Dim UploadRow As DataRow
        For Each Kennzeichen As String In sKennz
            UploadRow = m_Versand.tblUpload.NewRow
            UploadRow("CHASSIS_NUM") = ""
            UploadRow("LICENSE_NUM") = Kennzeichen
            UploadRow("TIDNR") = ""
            UploadRow("LIZNR") = ""
            UploadRow("ZZREFERENZ1") = ""
            UploadRow("ZZREFERENZ2") = ""
            m_Versand.tblUpload.Rows.Add(UploadRow)
        Next
    End Sub

    Private Sub CreateTableUploadBriefNr(ByVal sBriefNr() As String)
        m_Versand = New Briefversand(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
        m_Versand.CreateUploadTable()

        Dim UploadRow As DataRow
        For Each TIDNr As String In sBriefNr
            UploadRow = m_Versand.tblUpload.NewRow
            UploadRow("CHASSIS_NUM") = ""
            UploadRow("LICENSE_NUM") = ""
            UploadRow("TIDNR") = TIDNr
            UploadRow("LIZNR") = ""
            UploadRow("ZZREFERENZ1") = ""
            UploadRow("ZZREFERENZ2") = ""
            m_Versand.tblUpload.Rows.Add(UploadRow)
        Next


    End Sub

    Private Sub CreateTableUploadVNr(ByVal sVNr() As String)
        m_Versand = New Briefversand(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
        m_Versand.CreateUploadTable()

        Dim UploadRow As DataRow
        For Each Vertragsnr As String In sVNr
            UploadRow = m_Versand.tblUpload.NewRow
            UploadRow("CHASSIS_NUM") = ""
            UploadRow("LICENSE_NUM") = ""
            UploadRow("TIDNR") = ""
            UploadRow("LIZNR") = Vertragsnr
            UploadRow("ZZREFERENZ1") = ""
            UploadRow("ZZREFERENZ2") = ""
            m_Versand.tblUpload.Rows.Add(UploadRow)
        Next
    End Sub

    Protected Sub rb_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rb_temp.CheckedChanged, rb_endg.CheckedChanged

        m_Versand.VersandGrund = Nothing

        ResetAdress()

        RadioButtonVersandChanged()
    End Sub

    Private Sub RadioButtonVersandChanged()

        m_App.GetAppAutLevel(m_User.GroupID, Session("AppID").ToString)

        If String.IsNullOrEmpty(m_App.AutorisierungsLevel) = False Then
            Dim Level() As String

            trAdressuche.Visible = False
            trZulStelleSuche.Visible = False
            trFreieAdresse.Visible = False

            Level = Split(m_App.AutorisierungsLevel, "|")
            Level = Split(Level(0), ",")
            For i As Integer = 0 To Level.Length - 1

                If rb_temp.Checked = True Then

                    Select Case Level(i)

                        Case Adressarten.TempSuche
                            trAdressuche.Visible = True
                        Case Adressarten.TempZulassungsstelle
                            trZulStelleSuche.Visible = True
                        Case Adressarten.TempManuell
                            trFreieAdresse.Visible = True

                    End Select


                ElseIf rb_endg.Checked = True Then
                    Select Case Level(i)

                        Case Adressarten.EndSuche
                            trAdressuche.Visible = True
                        Case Adressarten.EndZulassungsstelle
                            trZulStelleSuche.Visible = True
                        Case Adressarten.EndManuell
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


        'If rb_temp.Checked = True Then
        '    m_App.GetAppAutLevel(m_User.GroupID, Session("AppID").ToString)



        '    Select Case m_App.AppAutLevel
        '        Case 1
        '            trAdressuche.Visible = False
        '            trZulStelleSuche.Visible = True
        '            cpeZulstelle.ClientState = True
        '            trFreieAdresse.Visible = False
        '        Case 2
        '            rb_endg.Visible = False
        '            trAdressuche.Visible = True
        '            cpeAdressSuche.ClientState = True
        '            trZulStelleSuche.Visible = True
        '            trFreieAdresse.Visible = False
        '        Case 3
        '            cpeAdressSuche.ClientState = True
        '            trAdressuche.Visible = True
        '            trZulStelleSuche.Visible = True
        '            trFreieAdresse.Visible = True
        '        Case Else
        '            cpeAdressSuche.ClientState = True
        '            trAdressuche.Visible = True
        '            trZulStelleSuche.Visible = True
        '            trFreieAdresse.Visible = True
        '    End Select

        'ElseIf rb_endg.Checked = True Then
        '    m_App.GetAppAutLevel(m_User.GroupID, Session("AppID").ToString)
        '    Select Case m_App.AppAutLevel
        '        Case 4
        '            cpeAdressSuche.ClientState = True
        '            trAdressuche.Visible = True
        '            trZulStelleSuche.Visible = False
        '            trFreieAdresse.Visible = False
        '        Case 5
        '            cpeAdressSuche.ClientState = True
        '            trAdressuche.Visible = True
        '            trZulStelleSuche.Visible = True
        '            trFreieAdresse.Visible = False
        '        Case 6
        '            cpeAdressSuche.ClientState = True
        '            trAdressuche.Visible = True
        '            trZulStelleSuche.Visible = True
        '            trFreieAdresse.Visible = True
        '    End Select
        'End If
    End Sub

    Protected Sub ibtAuswahl_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
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
        m_App.GetAppAutLevel(m_User.GroupID, Session("AppID").ToString)

        Dim Level() As String

        If String.IsNullOrEmpty(m_App.AutorisierungsLevel) = False Then

            Level = Split(m_App.AutorisierungsLevel, "|")

            'Beinhaltet das Level die Adressart?
            If Level(0).Contains(m_Versand.Adressart) Then

                'Zugehörige Autorisierungsart aus dem zweiten Array ermitteln
                Dim arrLevel() As String = Split(Level(0), ",")
                Dim arrAutorisierung() As String = Split(Level(1), ",")

                For i As Integer = 0 To arrLevel.Length - 1

                    If arrLevel(i) = m_Versand.Adressart Then
                        '1 = Autorisierung, 2 = Keine Autorisierung
                        If arrAutorisierung(i) = "1" Then ZurAutorisierung = True : Exit For

                    End If


                Next


            End If


        End If


        Return ZurAutorisierung

    End Function

#End Region
   
    Protected Sub ibtnCreatePDF_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCreatePDF.Click

        Dim tmpDataView As New DataView()
        tmpDataView = m_Versand.Fahrzeuge.DefaultView
        tmpDataView.RowFilter = "Selected = '1'"

        Select Case m_Versand.Adressart
            Case 1, 4
                m_Versand.AdressartText = "Adressauswahl"
            Case 2, 5
                m_Versand.AdressartText = "Zulassungsstellen"
            Case 3, 6
                m_Versand.AdressartText = "manuelle Adresseingabe"
        End Select

        m_Versand.VersandoptionenText = lblOptionsOverViewShow.Text.Replace("<br />", vbCrLf)
        m_Versand.VersandAdresseText = m_Versand.VersandAdresseText.Replace(" <br /> ", vbCrLf)
        m_Versand.FahrzeugePrint = tmpDataView.ToTable
        'm_Versand.Beauftragungsdatum = Date.Now().ToShortDateString


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
        dr("Sachbearbeiter") = m_Versand.Sachbearbeiter
        dr("VersartText") = m_Versand.VersartText
        dr("AdressartText") = m_Versand.AdressartText
        dr("VersandadresseText") = m_Versand.VersandAdresseText
        dr("VersgrundText") = m_Versand.VersgrundText
        dr("Bemerkung") = m_Versand.Bemerkung
        dr("Halter") = m_Versand.Halter
        dr("VersandoptionenText") = m_Versand.VersandoptionenText
        dr("AutorisierungText") = m_Versand.AutorisierungText

        headTable.Rows.Add(dr)
        headTable.AcceptChanges()


        m_Versand.FahrzeugePrint.TableName = "Fahrzeuge"


        Dim imageHt As New Hashtable()
        Try
            imageHt.Add("Logo", m_User.Customer.LogoImage)
        Catch ex As Exception
            ' LogoPath am Customer nicht (korrekt) gepflegt - hier: ignorieren
        End Try

        Dim docFactory As New DocumentGeneration.WordDocumentFactory(m_Versand.FahrzeugePrint, imageHt)
        docFactory.CreateDocumentTable("Versandauftrag_" & m_User.UserName, Me.Page, "\Components\ComCommon\Documents\VersandZB2.doc", headTable)

    End Sub

    Protected Sub cmdOKWarnung_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdOKWarnung.Click
        divMessage.Visible = False
        divBackDisabled.Visible = False
        NextToChoice()
    End Sub

    Private Sub InApp()

        If m_User.Applications.Select("AppURL = '../Components/ComCommon/Change05s.aspx'").Length > 0 Then
            ibtEdit.Visible = True
        End If

    End Sub

    Protected Sub ibtEdit_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ibtEdit.Click

        If ddlAdresse.SelectedIndex > 0 Then
            Dim Name As String
            Dim Ident As String

            Ident = ddlAdresse.SelectedValue
            Name = m_Versand.Adressen.Select("IDENT = '" & Ident & "'")(0)("NAME1").ToString

            Session.Add("me", Me)

            m_Versand.VersandArt = IIf(rb_temp.Checked, "1", "2")
            Session("App_Versand") = m_Versand

            Response.Redirect("../Change05s.aspx?AppID=" & Session("AppID").ToString & "&ident=" & Ident & "&Name=" & Name & "&eqtyp=B")
        Else
            lblSucheAdr.Visible = True
            lblSucheAdr.Text = "Bitte wählen Sie eine Adresse aus!"
        End If
    End Sub

    Private Sub Reset()


        Try
            Dim MeControls As Change99 = CType(Session("me"), Change99)

            m_Versand = Session("App_Versand")

            lblErrorVersandOpt.Text = ""

            VersandTabPanel1.Visible = False
            VersandTabPanel2.Visible = True
            lbtnStammdaten.CssClass = "VersandButtonStammReady"
            lbtnAdressdaten.CssClass = "VersandButtonAdresse"
            lblSteps.Text = "Schritt 2 von 4"
            Panel2.CssClass = "StepActive"

            If Not m_Versand.VersandArt Is Nothing Then
                If m_Versand.VersandArt = "1" Then
                    rb_temp.Checked = True
                ElseIf m_Versand.VersandArt = "2" Then
                    rb_endg.Checked = True
                End If

            End If

            trZulStelleSuche.Visible = False
            trFreieAdresse.Visible = False
            m_App.GetAppAutLevel(m_User.GroupID, Session("AppID").ToString)

            Dim Level() As String

            If String.IsNullOrEmpty(m_App.AutorisierungsLevel) = False Then

                Level = Split(m_App.AutorisierungsLevel, "|")
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
                tmpItem = New ListItem

                tmpItem = Item

                ddlAdresse.Items.Add(tmpItem)

            Next

            ddlAdresse.SelectedValue = MeControls.ddlAdresse.SelectedValue
            lblSucheAdr.Visible = False


            Dim Mode As String = Request.QueryString.Item("mode").ToString




            If Mode = "success" Then
                m_Versand.GetAdressenandZulStellen(Session("AppID").ToString, Session.SessionID.ToString, Me)
                Session("App_Versand") = m_Versand
                AdressChoice()
            End If



        Catch ex As Exception
            Session("me") = Nothing
        Finally
            Session("me") = Nothing
        End Try

    End Sub

    Protected Sub ibtExtendSearch_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ibtExtendSearch.Click

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

    Protected Sub ibtBack_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ibtBack.Click
        ibtExtendSearch.Visible = True
        ibtBack.Visible = False
        tblSearch.Visible = True
        tblSearchExtend.Visible = False

        cpeDokuAusgabe.Collapsed = True
        cpeDokuAusgabe.ClientState = Nothing
    End Sub

End Class