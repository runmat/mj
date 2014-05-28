Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Partial Public Class Report01s1
    Inherits System.Web.UI.Page

#Region "Declarations"

    Protected WithEvents GridNavigation1 As CKG.Services.GridNavigation

    Private m_App As App
    Private m_User As User
#End Region

#Region "Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)
        m_App = New App(m_User)
        GridNavigation1.setGridElment(gvBestand)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

            m_App = New App(m_User)


            lblError.Text = ""

            Dim newSelection As Boolean = True

            If IsPostBack = False Then

                If Me.Request.UrlReferrer.AbsolutePath.Contains("selection.aspx") Then
                    Session("mObjBestand") = Nothing
                End If

                setSearchVisibility()
                If Not Session("mObjBestand") Is Nothing Then
                    If Not CType(Session("mObjBestand"), Bestand).Gesamt Is Nothing AndAlso Not CType(Session("mObjBestand"), Bestand).Gesamt.Rows.Count < 1 Then
                        newSelection = False
                    End If
                End If

                If newSelection Then
                    FillPLangruppenzähler(newSelection)
                    FillLeistungsart(ddlPlangruppenzähler.SelectedValue)
                    FillDropdowns()
                Else
                    FillPLangruppenzähler(newSelection)
                    FillDropdowns()

                    gvBestand.PageSize = CType(Session("mObjBestand"), Bestand).GridPageSize
                    FillGrid(CType(Session("mObjBestand"), Bestand).GridPageIndex, CType(Session("mObjBestand"), Bestand).GridSort)

                End If


            End If


        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try

    End Sub

    Protected Sub lbCreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbCreate.Click
        If CheckInputFields() = True Then

            DoSubmit()

        End If
    End Sub

    Protected Sub lnkCreatePDF1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreatePDF1.Click

        ScriptManager.RegisterClientScriptBlock( _
                                 Me, _
                                 GetType(Page), _
                                 "PageScript", _
                                 "window.open('Documents/" & m_User.KUNNR & "/Prozessablauf_VWLeasing.pdf');", _
                                 True)

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
    Private Sub GridNavigation1_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation1.PagerChanged
        gvBestand.PageIndex = PageIndex
        FillGrid(PageIndex)
    End Sub

    Private Sub GridNavigation1_PageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillGrid(0)
    End Sub

    Private Sub gvBestand_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvBestand.RowDataBound

    End Sub

    Private Sub gvBestand_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvBestand.Sorting
        FillGrid(gvBestand.PageIndex, e.SortExpression)
    End Sub

    Protected Sub lnkCreateExcel1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel1.Click
        'Dim reportExcel As DataTable = CType(Session("mObjBestand"), Bestand).Gesamt.DefaultView.ToTable
        Dim reportExcel As DataTable = CType(Session("mObjBestand"), Bestand).Gesamt.Copy.DefaultView.ToTable


        If m_User.Groups.ItemByID(m_User.GroupID).Authorizationright = 1 Then
            reportExcel.Columns("ANLZU").ColumnName = "Kundenstatus"
        Else
            reportExcel.Columns.Remove("ANLZU")

        End If

        reportExcel.AcceptChanges()


        For Each dr As DataRow In reportExcel.Rows

            If dr("Haltername").ToString.Contains("*X*") Then
                dr("Status") = "Erledigt"
                dr("Haltername") = Replace(dr("Haltername").ToString, "*X*", "")
            End If


        Next


        reportExcel.AcceptChanges()

        Try
            Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
            excelFactory.CreateDocumentAndSendAsResponse(strFileName, reportExcel, Me)

        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Protected Sub NewSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles NewSearch.Click
        lbCreate.Visible = True
        tab1.Visible = True
        Queryfooter.Visible = True
        Result.Visible = False
    End Sub


    Protected Sub btnEmpty_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEmpty.Click

    End Sub

    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        Session("mObjBestand") = Nothing
        Response.Redirect("/Services/(S(" & Session.SessionID & "))/Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub
#End Region

#Region "Methods"
    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        Dim tblTemp = CType(Session("mObjBestand"), Bestand).Gesamt.Copy

        Dim arbeitsplanCount As Integer = CType(Session("mObjBestand"), Bestand).Arbeitsplan.Rows.Count

        CType(Session("mObjBestand"), Bestand).GridPageIndex = intPageIndex
        CType(Session("mObjBestand"), Bestand).GridSort = strSort

        tblTemp.Columns.Remove("ANLZU")
        tblTemp.Columns.Remove("Kennzeichen")
        tblTemp.Columns.Remove("Mitarbeiternummer")
        tblTemp.Columns.Remove("Strasse")
        tblTemp.Columns.Remove("PLZ")
        tblTemp.Columns.Remove("Ort")
        tblTemp.Columns.Remove("Telefon")
        tblTemp.Columns.Remove("Email")

        tblTemp.Columns.Remove("KM_Hin_Abfahrt")
        tblTemp.Columns.Remove("KM_Hin_Ankunft")
        tblTemp.Columns.Remove("KM_Hin_Differenz")
        tblTemp.Columns.Remove("KM_Rueck_Abfahrt")
        tblTemp.Columns.Remove("KM_Rueck_Ankunft")
        tblTemp.Columns.Remove("KM_Rueck_Differenz")

        tblTemp.AcceptChanges()

        If CType(Session("mObjBestand"), Bestand).OffTeil = True Then

            Dim booRemove As Boolean

            For Each dr As DataRow In tblTemp.Rows

                booRemove = True

                For i As Integer = 2 To tblTemp.Columns.Count - 1
                    If dr(i).ToString = "" OrElse dr(i).ToString.Contains("*TR*") Then
                        booRemove = False
                        Exit For
                    End If
                    If dr(i).ToString = "" OrElse dr(i).ToString.Contains("*PD*") Then
                        booRemove = False
                        Exit For
                    End If
                    If dr(i).ToString.Contains("*X*") AndAlso CType(Session("mObjBestand"), Bestand).OffTeil = True Then
                        booRemove = True
                        Exit For
                    End If
                Next

                If booRemove = True Then
                    dr.Delete()
                End If

            Next

            tblTemp.AcceptChanges()

        End If


        tblTemp.DefaultView.RowFilter = ""

        Dim tmpDataView = tblTemp.DefaultView

        If tmpDataView.Count = 0 Then
            Result.Visible = False
            lblError.Text = "zu Ihrer Selektion konnten keine Werte ermittelt werden"
        Else
            Result.Visible = True
            lbCreate.Visible = False
            tab1.Visible = False
            Queryfooter.Visible = False
            Dim strTempSort As String = ""
            Dim strDirection As String = ""
            Dim intTempPageIndex As Int32 = intPageIndex

            If strSort.Trim(" "c).Length > 0 Then
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
            gvBestand.PageIndex = intTempPageIndex
            gvBestand.DataSource = tmpDataView
            gvBestand.DataBind()

            CType(Session("mObjBestand"), Bestand).GridPageSize = gvBestand.PageSize

            Const colAdressButton As Integer = 0
            Const colStatusDD As Integer = 2
            Const colAuftragsnummer As Integer = 3
            Const colStatus As Integer = 7

            For Each item As GridViewRow In gvBestand.Rows

                Dim arrResult() As String = item.Cells(colStatus).Text.Split("/"c)

                If arrResult.Length = 2 Then
                    item.Cells(colStatus).HorizontalAlign = HorizontalAlign.Right

                    Dim countErledigt As Integer = CInt(arrResult(0))
                    Dim percentResult As Integer = CInt(((countErledigt * 100) / arbeitsplanCount))

                    If percentResult < 60 Then
                        item.Cells(colStatus).ForeColor = Drawing.Color.Red
                    ElseIf percentResult >= 60 AndAlso percentResult < 100 Then
                        item.Cells(colStatus).ForeColor = Drawing.Color.Orange
                    Else
                        item.Cells(colStatus).ForeColor = Drawing.Color.Green
                    End If
                End If

                Dim booShowRueckButton = False

                Dim i As Integer = 3
                While i < item.Cells.Count
                    'Adress-Button ausblenden, wenn kein Halter vorhanden ist
                    'Adress-Button nicht mehr ausblenden DMa 07.03.2011
                    'If i = 4 Then
                    '    If item.Cells(i).Text = "&nbsp;" Then
                    '        If TypeOf (item.Cells(1).FindControl("ibtAdresse")) Is ImageButton Then
                    '            ibt = CType(item.Cells(1).FindControl("ibtAdresse"), ImageButton)
                    '            ibt.Visible = False
                    '        End If

                    '    End If
                    'End If

                    If i > 5 Then
                        If item.Cells(i).Text = "&nbsp;" OrElse item.Cells(i).Text.Contains("*TR*") = True OrElse item.Cells(i).Text.Contains("*PD*") = True Then
                            booShowRueckButton = True
                        End If
                    End If

                    item.Cells(i).Text = Server.HtmlDecode(item.Cells(i).Text)
                    If item.Cells(i).Text.Contains("*TR*") Then
                        item.Cells(i).Text = item.Cells(i).Text.Replace("*TR*", "")
                        item.Cells(i).ForeColor = Drawing.Color.Orange
                    ElseIf item.Cells(i).Text.Contains("*PD*") Then
                        item.Cells(i).Text = item.Cells(i).Text.Replace("*PD*", "")
                        item.Cells(i).ForeColor = Drawing.Color.FromArgb(51, 102, 204)
                    ElseIf item.Cells(i).Text.Contains("*X*") Then
                        If chkOffene.Checked = False Then
                            item.Cells(colStatus).Text = "<img alt='Haken' src='/Services/Images/haken_gruen.gif' style='width: 14px; height: 14px;' />"
                            item.Cells(colStatus).Style.Add("text-align", "center")
                        End If

                        item.Cells(i).Text = item.Cells(i).Text.Replace("*X*", "")
                    End If

                    i += 1
                End While

                If booShowRueckButton = False Then
                    If TypeOf (item.Cells(colAdressButton).FindControl("imgbRueckmeldungErfassen")) Is ImageButton Then
                        Dim ibt As ImageButton = CType(item.Cells(colAdressButton).FindControl("imgbRueckmeldungErfassen"), ImageButton)
                        ibt.Visible = False
                    End If
                End If

                item.Cells(colAuftragsnummer).Visible = False
            Next

            gvBestand.HeaderRow.Cells(colAuftragsnummer).Visible = False

            If m_User.Groups.ItemByID(m_User.GroupID).Authorizationright = 1 Then
                gvBestand.Columns(colAdressButton).Visible = False
                gvBestand.Columns(colStatusDD).Visible = True

                For Each item In gvBestand.Rows
                    Dim lbl = DirectCast(item.FindControl("lblFin"), Label)
                    Dim lbl2 = DirectCast(item.FindControl("lblAuf"), Label)

                    If Not lbl Is Nothing AndAlso Not lbl2 Is Nothing Then
                        Dim ddl = CType(item.FindControl("ddlStatus"), DropDownList)
                        ddl.SelectedValue = CType(Session("mObjBestand"), Bestand).Gesamt.Select(String.Format("Fahrgestellnummer = '{0}' and Auftragsnummer='{1}'", lbl.Text, lbl2.Text))(0)("ANLZU")
                    Else
                        Continue For
                    End If
                Next
            Else
                gvBestand.Columns(colStatusDD).Visible = False
            End If

            If CType(Session("mObjBestand"), Bestand).ArbeitsplatzUser = "showAll" Then
                gvBestand.Columns(colAdressButton).Visible = False
            End If

            Result.Visible = True
        End If

    End Sub

    Private Sub FillPLangruppenzähler(ByVal refill As Boolean)

        Dim getData As Bestand

        If refill = False Then
            getData = CType(Session("mObjBestand"), Bestand)
        Else
            getData = New Bestand
            getData.GetPlangruppenzähler(m_User, m_App, Me)
            Session("mObjBestand") = getData
        End If

        Dim tblTemp = getData.Plangruppenzähler.DefaultView.ToTable

        For Each dr As DataRow In tblTemp.Rows
            ddlPlangruppenzähler.Items.Add(New ListItem(dr.Field(Of String)("POS_TEXT"), dr.Field(Of String)("POS_KURZTEXT")))
        Next
    End Sub

    Private Sub FillLeistungsart(plangruppenzähler As String)
        Dim getData = CType(Session("mObjBestand"), Bestand)
        getData.GewählterPlangruppenzähler = plangruppenzähler
        getData.GetLeistungsart(m_User, m_App, Me)
        Session("mObjBestand") = getData
    End Sub

    Private Sub FillDropdowns()
        Dim tblTemp = CType(Session("mObjBestand"), Bestand).Leistungsart.DefaultView.ToTable

        ddlBedingungPos.Items.Clear()
        ddlBedingungNeg.Items.Clear()

        If tblTemp.Rows.Count = 0 Then
            ddlBedingungPos.Items.Add(New ListItem("Keine Leistungsarten vorhanden", "0"))
            ddlBedingungNeg.Items.Add(New ListItem("Keine Leistungsarten vorhanden", "0"))
            ddlBedingungPos.Enabled = False
            ddlBedingungNeg.Enabled = False
        Else
            ddlBedingungPos.Items.Add(New ListItem("Wählen Sie eine Leistungsart", "0"))
            ddlBedingungNeg.Items.Add(New ListItem("Wählen Sie eine Leistungsart", "0"))

            For Each dr As DataRow In tblTemp.Rows
                ddlBedingungPos.Items.Add(New ListItem(dr("LTXA1").ToString, dr("LARNT").ToString))
                ddlBedingungNeg.Items.Add(New ListItem(dr("LTXA1").ToString, dr("LARNT").ToString))
            Next

            ddlBedingungPos.Enabled = True
            ddlBedingungNeg.Enabled = True
        End If

        ddlOperator.Items.Clear()
        ddlOperator.Items.Add(New ListItem("Auswahl", "0"))
        ddlOperator.Items.Add(New ListItem("am", "EQ"))
        ddlOperator.Items.Add(New ListItem("vor", "LT"))
        ddlOperator.Items.Add(New ListItem("nach", "GT"))
    End Sub

    Private Sub DoSubmit()

        Dim getData = CType(Session("mObjBestand"), Bestand)
        Dim artMit As String = ""
        Dim artOhne As String = ""
        Dim artOperator As String = ""
        Dim mitDatum As String = ""
        Dim mitarbeiternr As String = ""
        Dim fahrgestellnr As String = ""
        Dim halter As String = ""
        Dim vertragsnr As String = ""
        Dim boolEinzel As Boolean = False


        getData.OffTeil = chkOffene.Checked

        If rbMehrfachsuche.Checked Then

            If ddlBedingungPos.SelectedItem.Value <> "0" Then
                artMit = ddlBedingungPos.SelectedItem.Value
            End If

            If ddlBedingungNeg.SelectedItem.Value <> "0" Then
                artOhne = ddlBedingungNeg.SelectedItem.Value
            End If

            If ddlOperator.SelectedItem.Value <> "0" Then
                artOperator = ddlOperator.SelectedItem.Value
            End If

            If txtDatum.Text.Length > 0 Then
                mitDatum = txtDatum.Text
            End If
        Else
            If txtMitarbeiter.Text.Length > 0 Then
                mitarbeiternr = txtMitarbeiter.Text.Trim
                boolEinzel = True
            End If
            If txtFahrgestellnummer.Text.Length > 0 Then
                If boolEinzel = True Then
                    lblError.Text = "Bitte geben Sie nur ein Suchkriterium ein!"
                    Exit Sub
                End If
                boolEinzel = True
                fahrgestellnr = txtFahrgestellnummer.Text.Trim
            End If
            If txtVertragsnummer.Text.Length > 0 Then
                If boolEinzel = True Then
                    lblError.Text = "Bitte geben Sie nur ein Suchkriterium ein!"
                    Exit Sub
                End If
                boolEinzel = True
                vertragsnr = txtVertragsnummer.Text.Trim
            End If
            If txtHalter.Text.Length > 0 Then
                If boolEinzel = True Then
                    lblError.Text = "Bitte geben Sie nur ein Suchkriterium ein!"
                    Exit Sub
                End If
                halter = txtHalter.Text.Trim
            End If
        End If

        getData.Mitarbeiternr = mitarbeiternr
        getData.Fin = fahrgestellnr
        getData.LvNummer = vertragsnr
        getData.Haltername = halter
        getData.MitDatum = mitDatum
        getData.Op = artOperator
        getData.ArtOhne = artOhne
        getData.ArtMit = artMit
        getData.Kundenstatus = txtKundenStatus.Text

        getData.GetAuswertung(m_User, m_App, Me)

        If getData.Gesamt.Rows.Count > 0 Then
            Session("mObjBestand") = getData
            FillGrid(0)
        Else
            lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
        End If
    End Sub

    Private Function CheckInputFields() As Boolean

        Dim booError As Boolean = True

        If Len(txtFahrgestellnummer.Text & txtVertragsnummer.Text & txtHalter.Text) > 0 Then
            If txtFahrgestellnummer.Text.Length > 0 Then
                If txtFahrgestellnummer.Text.Length <> 17 Then
                    lblError.Text = "Bitte geben Sie eine 17-stellige Fahrgestellnummer ein."
                    Return False
                End If
            End If
        End If

        If ddlBedingungPos.SelectedItem.Value <> "0" AndAlso ddlBedingungNeg.SelectedItem.Value <> "0" Then
            If ddlBedingungPos.SelectedItem.Value = ddlBedingungNeg.SelectedItem.Value Then
                lblError.Text = "'Dienstleistung offen' und 'Dienstleistung erledigt' können nicht denselben Eintrag haben."
                Return False
            End If
        End If

        If txtDatum.Text.Length > 0 Then
            If IsDate(txtDatum.Text) = False Then
                lblError.Text = "Ungültiges Datum."
                Return False
            End If
        End If

        If ddlBedingungPos.SelectedItem.Value <> "0" Then
            If ddlOperator.SelectedItem.Value = "0" And txtDatum.Text.Length > 0 Then
                lblError.Text = "Bitte wählen Sie einen Operator."
                Return False
            End If

            If ddlOperator.SelectedItem.Value <> "0" And txtDatum.Text.Length = 0 Then
                lblError.Text = "Bitte tragen Sie ein Datum ein."
                Return False
            End If
        End If

        Return True
    End Function

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        ModalPopupExtender2.Show()
    End Sub

    Private Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click
        'ModalPopupExtender2.Hide()
        mb.Attributes.Add("style", "display:none")
        mb.Visible = False
    End Sub

    Private Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvBestand.RowCommand
        If e.CommandName = "RueckmeldungErfassen" Then
            Dim parameterlist As String = ""
            HelpProcedures.getAppParameters(Session("AppID").ToString, parameterlist, ConfigurationManager.AppSettings.Get("Connectionstring"))
            Response.Redirect("Report01s_02.aspx?AppID=" & Session("AppID").ToString & parameterlist & "&AUFNR=" & e.CommandArgument.ToString)
        End If

        If e.CommandName = "Adresse" Then

            Response.Redirect("Report01s_03.aspx?AppID=" & Session("AppID").ToString & "&AUFNR=" & e.CommandArgument.ToString)

        End If

    End Sub

    Protected Sub rbMehrfachsuche_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rbMehrfachsuche.CheckedChanged, rbEinzelsuche.CheckedChanged
        setSearchVisibility()
    End Sub

    Private Sub SetSearchVisibility()
        'keine elegante lösung zum ausblenden, leider aus zeitgründen keine umstellung Anordnung möglich. JJU190510 ITA 3738
        Dim boolEinzel = rbEinzelsuche.Checked
        Dim boolMehrfach = rbMehrfachsuche.Checked

        Einzelsuche1.Visible = boolEinzel
        Einzelsuche2.Visible = boolEinzel
        Einzelsuche3.Visible = boolEinzel
        Einzelsuche4.Visible = boolEinzel
        Einzelsuche5.Visible = boolEinzel

        Mehrfachsuche1.Visible = boolMehrfach
        Mehrfachsuche2.Visible = boolMehrfach
        Mehrfachsuche3.Visible = boolMehrfach
        chkOffene.Visible = boolMehrfach
        If boolEinzel Then
            chkOffene.Checked = False
            txtKundenStatus.Text = ""
        End If


        trKundenstatus.Visible = boolMehrfach

        m_App.GetAppAutLevel(m_User.GroupID, Session("AppID").ToString)

        If m_User.Groups.ItemByID(m_User.GroupID).Authorizationright = 1 Then

            trKundenstatus.Visible = boolMehrfach
        Else
            trKundenstatus.Visible = False

        End If

    End Sub


#End Region

    Protected Sub ddlPlangruppenzähler_SelectedIndexChanged(sender As Object, e As EventArgs)
        FillLeistungsart(ddlPlangruppenzähler.SelectedValue)
        FillDropdowns()
    End Sub


    Protected Sub ddlStatus_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddl1 As DropDownList = CType(sender, DropDownList)
        Dim row As GridViewRow = CType(ddl1.NamingContainer, GridViewRow)

        Dim index As Integer = row.RowIndex

        Dim lbl As Label = gvBestand.Rows(index).FindControl("lblFin")
        Dim lbl2 As Label = gvBestand.Rows(index).FindControl("lblAuf")

        Dim bs As Bestand = CType(Session("mObjBestand"), Bestand)

        bs.Gesamt.Select("Fahrgestellnummer = '" & lbl.Text & "' and Auftragsnummer='" & lbl2.Text & "'")(0)("ANLZU") = ddl1.SelectedValue

        'Dim Auftragsnummer As String = bs.Kopfdaten.Select("KTEXT = '" & lbl.Text & "'")(0)("AUFNR")

        bs.SaveStatus(m_User, m_App, Me, ddl1.SelectedValue, lbl2.Text)

    End Sub
End Class
' ************************************************
' $History: Report01s.aspx.vb $
' 
' *****************  Version 30  *****************
' User: Fassbenders  Date: 25.05.11   Time: 10:28
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing
' 
' *****************  Version 29  *****************
' User: Fassbenders  Date: 7.03.11    Time: 16:44
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing
' 
' *****************  Version 28  *****************
' User: Jungj        Date: 12.08.10   Time: 17:51
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing
' ITA 4047 nachbesserung
' 
' *****************  Version 27  *****************
' User: Jungj        Date: 12.08.10   Time: 15:45
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing
' ITA 4047 testfertig
' 
' *****************  Version 26  *****************
' User: Rudolpho     Date: 28.07.10   Time: 15:05
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing
' 
' *****************  Version 25  *****************
' User: Rudolpho     Date: 28.07.10   Time: 9:23
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing
' 
' *****************  Version 24  *****************
' User: Rudolpho     Date: 27.07.10   Time: 9:41
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing
' 
' *****************  Version 23  *****************
' User: Rudolpho     Date: 23.07.10   Time: 16:20
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing
' 
' *****************  Version 22  *****************
' User: Rudolpho     Date: 19.07.10   Time: 11:15
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing
' ITA: 3912
' 
' *****************  Version 21  *****************
' User: Fassbenders  Date: 15.07.10   Time: 14:48
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing
' 
' *****************  Version 20  *****************
' User: Rudolpho     Date: 15.07.10   Time: 14:42
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing
' 
' *****************  Version 19  *****************
' User: Fassbenders  Date: 14.07.10   Time: 16:24
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing
' 
' *****************  Version 18  *****************
' User: Fassbenders  Date: 12.07.10   Time: 8:47
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing
' 
' *****************  Version 17  *****************
' User: Fassbenders  Date: 9.07.10    Time: 11:23
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing
' 
' *****************  Version 16  *****************
' User: Fassbenders  Date: 8.07.10    Time: 14:29
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing
' 
' *****************  Version 15  *****************
' User: Fassbenders  Date: 8.07.10    Time: 13:21
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing
' 
' *****************  Version 14  *****************
' User: Fassbenders  Date: 8.07.10    Time: 10:13
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing
' ITA: 3912
' 
' *****************  Version 13  *****************
' User: Fassbenders  Date: 16.06.10   Time: 14:03
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing
' 
' *****************  Version 12  *****************
' User: Fassbenders  Date: 15.06.10   Time: 12:59
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing
' ITA: 3829
' 
' *****************  Version 11  *****************
' User: Fassbenders  Date: 11.06.10   Time: 15:13
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing
' ITA: 3829
' 
' *****************  Version 10  *****************
' User: Fassbenders  Date: 2.06.10    Time: 10:36
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing
' ITA: 3754
' 
' *****************  Version 9  *****************
' User: Fassbenders  Date: 1.06.10    Time: 14:35
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing
' 
' *****************  Version 8  *****************
' User: Fassbenders  Date: 31.05.10   Time: 17:34
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 20.05.10   Time: 13:35
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing
' ITA 3738 testfertig
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 19.05.10   Time: 23:11
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing
' ITA 3738
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 18.05.10   Time: 20:37
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing
' ITA 3754 unfertig
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 17.05.10   Time: 21:08
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 14.05.10   Time: 16:59
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 11.05.10   Time: 13:51
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.05.10    Time: 16:56
' Created in $/CKAG2/Services/Components/ComCommon/Leasing
' ITA: 3738
' 