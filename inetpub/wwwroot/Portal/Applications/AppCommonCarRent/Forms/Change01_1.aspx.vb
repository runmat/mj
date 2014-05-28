Imports CKG
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Partial Public Class Change01_1
    Inherits System.Web.UI.Page

    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private mObjZulassung As Zulassung

#Region "Properties"

    Private Property Refferer() As String
        Get
            If Not Session.Item(Me.Request.Url.LocalPath & "Refferer") Is Nothing Then
                Return Session.Item(Me.Request.Url.LocalPath & "Refferer").ToString()
            Else : Return Nothing
            End If
        End Get
        Set(ByVal value As String)
            Session.Item(Me.Request.Url.LocalPath & "Refferer") = value
        End Set
    End Property

#End Region

    Private Sub Change01_1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            m_User = GetUser(Me)
            m_App = New Base.Kernel.Security.App(m_User) 'erzeugt ein App_objekt 
            ucHeader.InitUser(m_User)
            FormAuth(Me, m_User)
            lblError.Text = ""


            If mObjZulassung Is Nothing Then
                If Not Session("mObjZulassungSession") Is Nothing Then
                    mObjZulassung = CType(Session("mObjZulassungSession"), Zulassung)
                Else
                    Throw New Exception("benötigtes Session Objekt nicht vorhanden")
                End If
            End If


            If Not IsPostBack AndAlso Not scriptmanager1.IsInAsyncPostBack Then
                If Refferer Is Nothing Then
                    If Not Me.Request.UrlReferrer Is Nothing Then
                        Refferer = Me.Request.UrlReferrer.ToString
                    Else
                        Refferer = ""
                    End If
                End If

                GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")
                lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
                ucStyles.TitleText = lblHead.Text


                mObjZulassung.FILLLeasingnehmerUndBranding(Session("AppID").ToString, Me.Session.SessionID, Me)
                mObjZulassung.fillAbwScheinSchilderAdr(Session("AppID").ToString, Me.Session.SessionID, Me)
                If Not mObjZulassung.Status = 0 Then
                    lblError.Text = mObjZulassung.Message
                    Exit Sub
                End If

                fillControls()
                FillGrid(0)

            End If


        Catch ex As Exception
            lblError.Text = "Beim laden der Seite ist ein Fehler aufgetreten: " & ex.Message
        End Try


    End Sub

    Private Sub responseBack()
        If Refferer = "" Then
            Dim strLinkPrefix As String = "/" & ConfigurationManager.AppSettings("WebAppPath") & "/"
            Response.Redirect(strLinkPrefix & "Start/Selection.aspx")
        Else
            Response.Redirect(Refferer)
        End If
    End Sub

    Private Sub fillControls()


        'headbereich
        Dim tmpDV As New DataView(mObjZulassung.Leasingnehmer)
        tmpDV.Sort = "NameOrt"

        ddlLizensnehmer.DataSource = tmpDV
        ddlLizensnehmer.DataTextField = "NameOrt"
        ddlLizensnehmer.DataValueField = "ID"
        ddlLizensnehmer.DataBind()
        ddlLizensnehmer.Items.Insert(0, "-Keine Auswahl-")


        ddlBranding.DataSource = mObjZulassung.Brandings
        ddlBranding.DataTextField = "BEZEICHNUNG_1"
        ddlBranding.DataValueField = "ZZLABEL"
        ddlBranding.DataBind()
        ddlBranding.Items.Insert(0, "-Keine Auswahl-")

        ddlDeckungskarte.Items.Insert(0, "-Keine Auswahl-")

        'detailBereich
        ddlDataLinznehmer.DataSource = mObjZulassung.Leasingnehmer
        ddlDataLinznehmer.DataTextField = "NameOrt"
        ddlDataLinznehmer.DataValueField = "ID"
        ddlDataLinznehmer.DataBind()
        ddlDataLinznehmer.Items.Insert(0, "-Keine Auswahl-")


        ddlDataBranding.DataSource = mObjZulassung.Brandings
        ddlDataBranding.DataTextField = "BEZEICHNUNG_1"
        ddlDataBranding.DataValueField = "ZZLABEL"
        ddlDataBranding.DataBind()
        ddlDataBranding.Items.Insert(0, "-Keine Auswahl-")

        ddlDataDeckungskarte.Items.Insert(0, "-Keine Auswahl-")

    End Sub


    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")

        Dim tmpDataView As New DataView(mObjZulassung.Fahrzeuge)
        tmpDataView.RowFilter = "Uebernommen='X'"

        If tmpDataView.Count = 0 Then
            gvFahrzeuge.Visible = False
            lblGvFahrzeugeNoData.Visible = True
        Else
            gvFahrzeuge.Visible = True
            lblGvFahrzeugeNoData.Visible = False

            Dim intTempPageIndex As Int32 = intPageIndex
            Dim strTempSort As String = ""
            Dim strDirection As String = ""

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




            gvFahrzeuge.PageIndex = intTempPageIndex

            gvFahrzeuge.DataSource = tmpDataView

            gvFahrzeuge.DataBind()


            For Each item As GridViewRow In gvFahrzeuge.Rows
                If Not item.FindControl("lnkFahrgestellnummer") Is Nothing Then
                    If Not m_User.Applications.Select("AppName = 'Report46'").Length = 0 Then
                        CType(item.FindControl("lnkFahrgestellnummer"), HyperLink).NavigateUrl = "../../../Components/ComCommon/Finance/Report46.aspx?AppID=" & m_User.Applications.Select("AppName = 'Report46'")(0)("AppID").ToString & "&VIN=" & CType(item.FindControl("lnkFahrgestellnummer"), HyperLink).Text
                    End If
                End If

                Dim tmpLit As Literal = CType(item.FindControl("lvl2GridStart"), Literal)

                If item.RowState = DataControlRowState.Alternate Then
                    tmpLit.Text = "</td></tr><tr Class='GridTableAlternate'><td colspan='" & item.Cells.Count - 1 & "'>"
                Else
                    tmpLit.Text = "</td></tr><tr><td colspan='" & item.Cells.Count - 1 & "'>"
                End If

            Next
        End If

    End Sub

    Private Sub gvFahrzeuge_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvFahrzeuge.PageIndexChanging
        FillGrid(e.NewPageIndex)
    End Sub

    Public Sub chk_Auswahl_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If CType(sender, CheckBox).Checked Then
            mObjZulassung.Fahrzeuge.Select("EQUNR='" & CType(CType(CType(sender, CheckBox).Parent.Parent, GridViewRow).FindControl("lblEQUNR"), Label).Text & "'")(0)("Ausgewaehlt") = "X"
        Else
            mObjZulassung.Fahrzeuge.Select("EQUNR='" & CType(CType(CType(sender, CheckBox).Parent.Parent, GridViewRow).FindControl("lblEQUNR"), Label).Text & "'")(0)("Ausgewaehlt") = ""
        End If
    End Sub

    Public Sub gvFahrzeuge_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvFahrzeuge.RowCommand

        If Not e.CommandName = "Sort" Then
            If e.CommandName = "alleLoeschen" Then
                For Each tmprow As GridViewRow In gvFahrzeuge.Rows
                    mObjZulassung.Fahrzeuge.Select("EQUNR='" & CType(tmprow.FindControl("lblEQUNR"), Label).Text & "'")(0)("Ausgewaehlt") = ""
                Next
            ElseIf e.CommandName = "alleAuwaehlen" Then
                For Each tmprow As GridViewRow In gvFahrzeuge.Rows
                    If CType(tmprow.FindControl("chkAuswahl"), CheckBox).Enabled Then
                        mObjZulassung.Fahrzeuge.Select("EQUNR='" & CType(tmprow.FindControl("lblEQUNR"), Label).Text & "'")(0)("Ausgewaehlt") = "X"
                    End If
                Next

            ElseIf e.CommandName = "insertOne" Then
                mObjZulassung.Fahrzeuge.Select("EQUNR='" & e.CommandArgument.ToString & "'")(0)("Uebernommen") = "X"
                mObjZulassung.Fahrzeuge.Select("EQUNR='" & e.CommandArgument.ToString & "'")(0)("Ausgewaehlt") = ""

            ElseIf e.CommandName = "bearbeiten" Then

                lblEqunr.Text = e.CommandArgument.ToString
                fillDetailsAnsicht(mObjZulassung.Fahrzeuge.Select("EQUNR='" & e.CommandArgument.ToString & "'")(0))
                upEquiDetails.Update()
                showDetails(True)
                Exit Sub 'kein fillGrid bitte

            End If

            FillGrid(gvFahrzeuge.PageIndex)
        End If

    End Sub

    Private Sub fillDetailsAnsicht(ByRef tmpRow As DataRow)
        lblDataSelektionAbsender.Text = tmpRow("Absender").ToString
        lblDataSelektionBereifung.Text = tmpRow("Bereifung").ToString
        lblDataSelektionFarbe.Text = tmpRow("Farbe").ToString
        lblDataSelektionHersteller.Text = tmpRow("Hersteller").ToString
        lblDataSelektionGetriebe.Text = tmpRow("Getriebe").ToString
        lblDataSelektionKraftstoffart.Text = tmpRow("Kraftstoffart").ToString
        lblDataSelektionLiefermonat.Text = tmpRow("Liefermonat").ToString
        lblDataSelektionNavi.Text = tmpRow("Navi").ToString
        lblDataSelektionStandort.Text = tmpRow("Standort").ToString
        lblDataSelektionTyp.Text = tmpRow("Typ").ToString

        If Not tmpRow("BrandingNR").ToString = "-" AndAlso Not tmpRow("BrandingNR").ToString = "" Then
            ddlDataBranding.SelectedValue = tmpRow("BrandingNR").ToString
        Else
            ddlDataBranding.SelectedIndex = 0
        End If

        If Not tmpRow("LizensnehmerNR").ToString = "-" AndAlso Not tmpRow("LizensnehmerNR").ToString = "" Then
            ddlDataLinznehmer.SelectedValue = tmpRow("LizensnehmerNR").ToString
        Else
            ddlDataLinznehmer.SelectedIndex = 0
        End If
        ddlDataDeckungskarte.Items.Clear()
        fillDeckungsKartenZuLizensnehmer(tmpRow("LizensnehmerNR").ToString, ddlDataDeckungskarte)

        If Not tmpRow("EVBNR").ToString = "-" AndAlso Not tmpRow("EVBNR").ToString = "" Then
            If Not ddlDataDeckungskarte.Items.Count = 1 Then
                ddlDataDeckungskarte.SelectedValue = tmpRow("EVBNR").ToString
            End If
        Else
            ddlDataDeckungskarte.SelectedIndex = 0
        End If

        If Not tmpRow("Laufzeit").ToString = "-" AndAlso Not tmpRow("Laufzeit").ToString = "" Then
            txtDataMindesthaltedauer.Text = tmpRow("Laufzeit").ToString
        Else
            txtDataMindesthaltedauer.Text = "180"
        End If

        If Not tmpRow("Zuldat") Is DBNull.Value AndAlso Not tmpRow("Laufzeit").ToString = "" Then
            txtDataZulassungsdatum.Text = CDate(tmpRow("Zuldat").ToString).ToShortDateString
        Else
            txtDataZulassungsdatum.Text = ""
        End If

        If Not tmpRow("AbwScheinSchilderNR").ToString = "-" AndAlso Not tmpRow("AbwScheinSchilderNR").ToString = "" Then
            lblDataAbwVersandScheinSchilder.Text = tmpRow("AbwScheinSchilderTEXT").ToString
            lblAbwScheinSchilderNR.Text = tmpRow("AbwScheinSchilderNR").ToString
        Else
            lblAbwScheinSchilderNR.Text = ""
            lblDataAbwVersandScheinSchilder.Text = "-"
        End If

    End Sub

    Private Sub gvFahrzeuge_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvFahrzeuge.Sorting
        FillGrid(gvFahrzeuge.PageIndex, e.SortExpression)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)

        showAnzahlAuswahl()
    End Sub

    Private Sub showAnzahlAuswahl()
        lblAnzeigeAnzahlAusgewaehlt.Text = "Anzahl ausgewählter Fahrzeuge: " & mObjZulassung.Fahrzeuge.Select("Ausgewaehlt='X'").Count & " / " & mObjZulassung.Fahrzeuge.Select("Uebernommen='X'").Count
    End Sub

    Protected Sub ddlLizensnehmer_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlLizensnehmer.SelectedIndexChanged

        fillDeckungsKartenZuLizensnehmer(ddlLizensnehmer.SelectedValue, ddlDeckungskarte)
    End Sub

    Private Sub fillDeckungsKartenZuLizensnehmer(ByVal LizensnehmerNummer As String, ByRef deckungskartendropdown As DropDownList)
        deckungskartendropdown.Items.Clear()
        If Not IsNumeric(LizensnehmerNummer) Then
            deckungskartendropdown.Enabled = False
        Else
            Dim tmpDV As New DataView(mObjZulassung.Deckungskarten)
            tmpDV.RowFilter = "KUNNR_LZN='" & LizensnehmerNummer & "'"
            Dim e As Int32 = 0
            Do While e < tmpDV.Count

                Dim tmpItem As New ListItem(tmpDV(e)("DeckungsKarteAnzeige").ToString, tmpDV(e)("ZVSNR").ToString)

                If tmpDV(e)("GENERAL_ABD").ToString = "J" Then
                    tmpItem.Attributes.CssStyle("Color") = "Red"
                End If
                deckungskartendropdown.Items.Add(tmpItem)
                e += 1
            Loop
            deckungskartendropdown.Enabled = True
        End If
        deckungskartendropdown.Items.Insert(0, "-Keine Auswahl-")
    End Sub

    Protected Sub imgInsertAll_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgInsertAll.Click

        If Not txtZulassungsdatum.Text.Trim(" "c) = "" Then
            If Not checkdatum(CDate(txtZulassungsdatum.Text)) Then
                Exit Sub
            End If
        End If

        If Not ddlDeckungskarte.SelectedIndex = 0 Then
            If Not mObjZulassung.Deckungskarten.Select("ZVSNR='" & ddlDeckungskarte.SelectedValue & "'")(0)("GENERAL_ABD").ToString = "J" Then
                If mObjZulassung.Fahrzeuge.Select("Uebernommen='X' AND EVBNR='" & ddlDeckungskarte.SelectedValue & "'").Count = 0 Then
                    If Not mObjZulassung.Fahrzeuge.Select("Uebernommen='X' AND AUSGEWAEHLT='X'").Count > 1 Then
                        writeValuesIntoTable()
                    Else
                        lblError.Text = "Deckungskartennummer: " & ddlDeckungskarte.SelectedValue & " kann nicht mehr als einmal vergeben werden"
                    End If
                Else
                    lblError.Text = "Deckungskartennummer: " & ddlDeckungskarte.SelectedValue & " wurde bereits zugewiesen"
                End If
            Else
                writeValuesIntoTable()
            End If
        Else
            writeValuesIntoTable()
        End If
    End Sub

    Private Sub writeValuesIntoTable()
        For Each tmpRow In mObjZulassung.Fahrzeuge.Select("Uebernommen='X' AND AUSGEWAEHLT='X'")
            If Not ddlBranding.SelectedIndex = 0 Then
                tmpRow("BrandingTEXT") = ddlBranding.SelectedItem.Text
                tmpRow("BrandingNR") = ddlBranding.SelectedItem.Value
            Else
                tmpRow("BrandingTEXT") = "-"
                tmpRow("BrandingNR") = "-"
            End If
            If IsDate(txtZulassungsdatum.Text) Then
                tmpRow("Zuldat") = CDate(txtZulassungsdatum.Text)
            End If
            If IsNumeric(txtMindesthaltedauer.Text) Then
                tmpRow("Laufzeit") = txtMindesthaltedauer.Text
            End If
            If Not ddlLizensnehmer.SelectedIndex = 0 Then
                tmpRow("LizensnehmerTEXT") = ddlLizensnehmer.SelectedItem.Text
                tmpRow("LizensnehmerNR") = ddlLizensnehmer.SelectedItem.Value
            Else
                tmpRow("LizensnehmerTEXT") = "-"
                tmpRow("LizensnehmerNR") = "-"
            End If
            If Not ddlDeckungskarte.SelectedIndex = 0 Then
                tmpRow("EVBTEXT") = ddlDeckungskarte.SelectedItem.Text
                tmpRow("EVBNR") = ddlDeckungskarte.SelectedItem.Value
            Else
                tmpRow("EVBTEXT") = "-"
                tmpRow("EVBNR") = "-"
            End If
        Next
        FillGrid(gvFahrzeuge.PageIndex)
    End Sub

    Protected Sub lb_Uebernehmen_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_Uebernehmen.Click

        If Not txtDataZulassungsdatum.Text.Trim(" "c) = "" Then
            If Not checkdatum(CDate(txtDataZulassungsdatum.Text)) Then
                Exit Sub
            End If
        End If

        If Not ddlDataDeckungskarte.SelectedIndex = 0 Then
            If Not mObjZulassung.Deckungskarten.Select("ZVSNR='" & ddlDataDeckungskarte.SelectedValue & "'")(0)("GENERAL_ABD").ToString = "J" Then
                If mObjZulassung.Fahrzeuge.Select("Uebernommen='X' AND EVBNR='" & ddlDataDeckungskarte.SelectedValue & "' AND EQUNR <> '" & lblEqunr.Text & "'").Count = 0 Then
                    writeDetailValuesIntoTable()
                Else
                    lblError.Text = "Deckungskartennummer: " & ddlDataDeckungskarte.SelectedValue & " wurde bereits zugewiesen"
                End If
            Else
                writeDetailValuesIntoTable()
            End If
        Else
            writeDetailValuesIntoTable()
        End If
        showDetails(False)
    End Sub

    Private Sub writeDetailValuesIntoTable()
        Dim tmpRow = mObjZulassung.Fahrzeuge.Select("EQUNR='" & lblEqunr.Text & "'")(0)
        If Not ddlDataBranding.SelectedIndex = 0 Then
            tmpRow("BrandingTEXT") = ddlDataBranding.SelectedItem.Text
            tmpRow("BrandingNR") = ddlDataBranding.SelectedItem.Value
        Else
            tmpRow("BrandingTEXT") = "-"
            tmpRow("BrandingNR") = "-"

        End If
        If IsDate(txtDataZulassungsdatum.Text) Then
            tmpRow("Zuldat") = CDate(txtDataZulassungsdatum.Text)
        End If
        If IsNumeric(txtDataMindesthaltedauer.Text) Then
            tmpRow("Laufzeit") = txtDataMindesthaltedauer.Text
        End If
        If Not ddlDataLinznehmer.SelectedIndex = 0 Then
            tmpRow("LizensnehmerTEXT") = ddlDataLinznehmer.SelectedItem.Text
            tmpRow("LizensnehmerNR") = ddlDataLinznehmer.SelectedItem.Value
        Else
            tmpRow("LizensnehmerTEXT") = "-"
            tmpRow("LizensnehmerNR") = "-"

        End If
        If Not ddlDataDeckungskarte.SelectedIndex = 0 Then
            tmpRow("EVBTEXT") = ddlDataDeckungskarte.SelectedItem.Text
            tmpRow("EVBNR") = ddlDataDeckungskarte.SelectedItem.Value
        Else
            tmpRow("EVBTEXT") = "-"
            tmpRow("EVBNR") = "-"

        End If

        If Not lblAbwScheinSchilderNR.Text = "" Then
            tmpRow("AbwScheinSchilderNR") = lblAbwScheinSchilderNR.Text
            tmpRow("AbwScheinSchilderTEXT") = mObjZulassung.AbwScheinSchilderAdr.Select("DADPDI='" & lblAbwScheinSchilderNR.Text & "'")(0)("AdressAnzeige")
        End If

    End Sub

    Protected Sub lb_Abbrechen_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_Abbrechen.Click
        showDetails(False)
    End Sub

    Private Sub ddlDataLinznehmer_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDataLinznehmer.SelectedIndexChanged
        fillDeckungsKartenZuLizensnehmer(ddlDataLinznehmer.SelectedValue, ddlDataDeckungskarte)
    End Sub

    Private Sub showDetails(ByVal wert As Boolean)
        If wert Then
            tableKopfselekton.Visible = False
            lb_weiter.Visible = False
            gvFahrzeuge.Visible = False
            panelEquiDetails.Visible = True
            lblAnzeigeAnzahlAusgewaehlt.Visible = False
        Else
            tableKopfselekton.Visible = True
            lb_weiter.Visible = True
            lblAnzeigeAnzahlAusgewaehlt.Visible = True
            panelEquiDetails.Visible = False
            FillGrid(gvFahrzeuge.PageIndex)
        End If
    End Sub

    Protected Sub imgbSucheAdresse_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbSucheAdresse.Click
        lbHaendler.DataSource = mObjZulassung.AbwScheinSchilderAdr
        lbHaendler.DataTextField = "AdressAnzeige"
        lbHaendler.DataValueField = "DADPDI"
        lbHaendler.DataBind()

        lblHaendlerDetailsName1.Text = ""
        lblHaendlerDetailsName2.Text = ""
        lblHaendlerDetailsOrt.Text = ""
        lblHaendlerDetailsPLZ.Text = ""

        lblAdrErgebnissAnzahl.Text = lbHaendler.Items.Count.ToString
        upSucheAdresse.Update()
        showAdrSuche(True)
    End Sub

    Private Sub showAdrSuche(ByVal wert As Boolean)
        If wert Then
            panelEquiDetails.Visible = False
            panelSucheAdresse.Visible = True

        Else
            panelEquiDetails.Visible = True
            panelSucheAdresse.Visible = False
        End If
    End Sub

    Protected Sub lb_AdrUebernehmen_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_AdrUebernehmen.Click
        lblAbwScheinSchilderNR.Text = lbHaendler.SelectedValue
        lblDataAbwVersandScheinSchilder.Text = lbHaendler.SelectedItem.Text
        showAdrSuche(False)
    End Sub

    Protected Sub lb_AdrAbbrechen_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_AdrAbbrechen.Click
        showAdrSuche(False)
    End Sub

    Protected Sub lb_Suche_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_Suche.Click
        sucheAbweichendeVersandadressenScheinSchilder()
    End Sub

    Private Sub sucheAbweichendeVersandadressenScheinSchilder()
        Dim tmpFilterString As String = ""


        If Not txtName.Text.Trim(" "c) = "" Then
            tmpFilterString = tmpFilterString & " AND NAME LIKE '" & txtName.Text.Replace("*", "%") & "'"
        End If

        If Not txtOrt.Text.Trim(" "c) = "" Then
            tmpFilterString = tmpFilterString & " AND ORT01 LIKE '" & txtOrt.Text.Replace("*", "%") & "'"
        End If

        If Not txtPLZ.Text.Trim(" "c) = "" Then
            tmpFilterString = tmpFilterString & " AND PSTLZ LIKE '" & txtPLZ.Text.Replace("*", "%") & "'"
        End If

        mObjZulassung.Filterstring = tmpFilterString

        lbHaendler.DataSource = mObjZulassung.AbwScheinSchilderAdrDV
        lbHaendler.DataTextField = "AdressAnzeige"
        lbHaendler.DataValueField = "DADPDI"
        lbHaendler.DataBind()
        lblAdrErgebnissAnzahl.Text = lbHaendler.Items.Count.ToString


    End Sub

    Protected Sub btnLoesch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnLoesch.Click
        txtName.Text = "**"
        txtOrt.Text = "**"
        txtPLZ.Text = "**"
        sucheAbweichendeVersandadressenScheinSchilder()
    End Sub

    Protected Sub lbHaendler_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles lbHaendler.SelectedIndexChanged
        lblHaendlerDetailsName1.Text = mObjZulassung.AbwScheinSchilderAdr.Select("DADPDI='" & lbHaendler.SelectedValue & "'")(0)("NAME1").ToString
        lblHaendlerDetailsName2.Text = mObjZulassung.AbwScheinSchilderAdr.Select("DADPDI='" & lbHaendler.SelectedValue & "'")(0)("NAME2").ToString
        lblHaendlerDetailsOrt.Text = mObjZulassung.AbwScheinSchilderAdr.Select("DADPDI='" & lbHaendler.SelectedValue & "'")(0)("ORT01").ToString
        lblHaendlerDetailsPLZ.Text = mObjZulassung.AbwScheinSchilderAdr.Select("DADPDI='" & lbHaendler.SelectedValue & "'")(0)("PSTLZ").ToString
    End Sub

    Private Function checkdatum(ByVal datum As Date) As Boolean

        '...Datum > Tagesdatum + 14 Tage?
        If datum > Date.Today.AddDays(14) Then

            lblError.Text = "Zulassungsdatum darf nicht mehr als 14 Tage in der Zukunft liegen."
            Return False
        End If

        '...Wochenende ?
        If (datum.DayOfWeek = DayOfWeek.Saturday) Or (datum.DayOfWeek = DayOfWeek.Sunday) Then

            lblError.Text = "Zulassungsdatum ungültig (Wochenende)."
            Return False
        End If

        Return True
    End Function

    Private Function checkVollstaendigkeit() As Boolean

        Dim blnErrorFound = False
        For Each tmpRow As GridViewRow In gvFahrzeuge.Rows
            Dim blnErrorInRow = False
            Dim tmpFahrzeug As DataRow = mObjZulassung.Fahrzeuge.Select("EQUNR='" & CType(tmpRow.FindControl("lblEQUNR"), Label).Text & "'")(0)

            If tmpFahrzeug("ZULDAT") Is DBNull.Value Then
                blnErrorInRow = True
            End If

            If tmpFahrzeug("BrandingNR").ToString.Trim(" "c) = "" OrElse tmpFahrzeug("BrandingNR").ToString = "-" Then
                blnErrorInRow = True
            End If

            If tmpFahrzeug("EVBNR").ToString.Trim(" "c) = "" OrElse tmpFahrzeug("EVBNR").ToString = "-" Then
                blnErrorInRow = True
            End If

            If tmpFahrzeug("BrandingNR").ToString.Trim(" "c) = "" OrElse tmpFahrzeug("BrandingNR").ToString = "-" Then
                blnErrorInRow = True
            End If

            If tmpFahrzeug("LizensnehmerNR").ToString.Trim(" "c) = "" OrElse tmpFahrzeug("LizensnehmerNR").ToString = "-" Then
                blnErrorInRow = True
            End If

            If tmpFahrzeug("Laufzeit").ToString.Trim(" "c) = "" OrElse tmpFahrzeug("Laufzeit").ToString = "-" Then
                blnErrorInRow = True
            End If

            If blnErrorInRow Then
                tmpRow.BackColor = Drawing.Color.Red
                blnErrorFound = True
            End If

        Next

        If Not blnErrorFound Then
            Return True
        Else
            lblError.Text = "Bitte prüfen sie auf fehlende Eingaben"
            Return False
        End If

    End Function

    Protected Sub lb_weiter_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_weiter.Click
        If checkVollstaendigkeit() Then
            Response.Redirect("Change01_2.aspx?AppID=" & Session("AppID").ToString)
        End If
    End Sub

    Protected Sub imgbAbwScheinUndSchilderloeschen_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbAbwScheinUndSchilderloeschen.Click
        lblAbwScheinSchilderNR.Text = ""
        lblDataAbwVersandScheinSchilder.Text = "-"
    End Sub

    Protected Sub lb_zurueck_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_zurueck.Click
        responseBack()
    End Sub

End Class
' ************************************************
' $History: Change01_1.aspx.vb $
' 
' *****************  Version 15  *****************
' User: Fassbenders  Date: 30.03.10   Time: 13:39
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA: 3553
' 
' *****************  Version 14  *****************
' User: Fassbenders  Date: 30.09.09   Time: 11:41
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' KGA Pfung Zulassungsdatum Vergangenheit rausnehmen
' 
' *****************  Version 13  *****************
' User: Rudolpho     Date: 29.04.09   Time: 16:53
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' Warnungen
' 
' *****************  Version 12  *****************
' User: Jungj        Date: 13.03.09   Time: 10:52
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ITAn 2537 ergnzung
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 4.03.09    Time: 11:33
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA 2655
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 12.02.09   Time: 11:15
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA 2537 testfertig
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 12.02.09   Time: 10:56
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA 2537
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 12.02.09   Time: 10:51
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA 2537
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 11.02.09   Time: 17:31
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ita 2537
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 10.02.09   Time: 16:22
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA 2537
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 9.02.09    Time: 17:33
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ita 2537
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 5.02.09    Time: 17:53
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA 2537
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 4.02.09    Time: 19:04
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ita 2537
' 
' ************************************************