Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Partial Public Class Change01
    Inherits System.Web.UI.Page

    Protected WithEvents ucStyles As CKG.Portal.PageElements.Styles
    Protected WithEvents ucHeader As CKG.Portal.PageElements.Header

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private m_objTable As DataTable
    Private m_objExcel As DataTable
    Private m_report As Carport

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            Literal1.Text = ""
            If Not IsPostBack Then
                DoSubmit()
                Dim List As ListItem
                For Each List In ddlVerwZweck.Items
                    If List.Text = "MF" Then
                        List.Selected = True
                    End If
                Next

                Setfilter()

            ElseIf Not Session("App_Report") Is Nothing Then
                m_report = CType(Session("App_Report"), Carport)
                Session("App_ResultTable") = m_report.Result
                Session("App_ResultTableCarports") = m_report.ResultCarports
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub calVon_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calVon.SelectionChanged
        Me.txtEingangsdatumVon.Text = calVon.SelectedDate.ToShortDateString
        calVon.Visible = False
        Setfilter()
    End Sub

    Private Sub DoSubmit()
        Dim checkInput As Boolean = True

        Session("lnkExcel") = ""

        Try

            lblError.Text = ""
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            m_report = New Carport(m_User, m_App, strFileName)
            m_report.Fill(Session("AppID").ToString, Session.SessionID.ToString)

            If Not m_report.Status = 0 Then
                lblError.Text = m_report.Message
            Else
                If Not m_report.Result.Rows.Count = 0 Then

                    lblResults.Text = "Es wurden " & m_report.Result.Rows.Count.ToString & " Fahrzeuge gefunden."
                    cmdDetails.Visible = True

                    Session("App_ResultTable") = m_report.Result
                    Session("App_ResultTableCarports") = m_report.ResultCarports

                    Session("App_Report") = m_report
                    lnkCreateExcel.Visible = True
                    lnkCreateExcel2.Visible = True
                End If

                m_objTable = m_report.ResultCarports

                FillGrid(0)
                FillControlls()
            End If
        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")

        If m_objTable Is Nothing Then
            m_objTable = CType(Session("App_ResultTableCarports"), DataTable)
        End If

        If m_objTable.Rows.Count = 0 Then
            rowResultate.Visible = False
            lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
        Else
            rowResultate.Visible = True

            Dim tmpDataView As New DataView()
            tmpDataView = m_objTable.DefaultView

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

            DataGrid1.CurrentPageIndex = intTempPageIndex

            DataGrid1.DataSource = tmpDataView
            DataGrid1.DataBind()

            If DataGrid1.PageCount > 1 Then
                DataGrid1.PagerStyle.Visible = True
                DataGrid1.PagerStyle.CssClass = "PagerStyle"
                If DataGrid1.CurrentPageIndex = DataGrid1.PageCount - 1 Then
                    DataGrid1.PagerStyle.NextPageText = "<img border=""0"" src=""/PortalORM/Images/empty.gif"" width=""12"" height=""11"">"
                Else
                    DataGrid1.PagerStyle.NextPageText = "<img border=""0"" src=""/PortalORM/Images/arrow_right.gif"" width=""12"" height=""11"">"
                End If

                If DataGrid1.CurrentPageIndex = 0 Then
                    DataGrid1.PagerStyle.PrevPageText = "<img border=""0"" src=""/PortalORM/Images/empty.gif"" width=""12"" height=""11"">"
                Else
                    DataGrid1.PagerStyle.PrevPageText = "<img border=""0"" src=""/PortalORM/Images/arrow_left.gif"" width=""12"" height=""11"">"
                End If
                DataGrid1.DataBind()
            Else
                DataGrid1.PagerStyle.Visible = False
            End If
        End If
    End Sub

    Private Sub DataGrid1_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGrid1.ItemDataBound
        Dim intItem As Int32

        For intItem = 0 To m_objTable.Columns.Count - 1
            If m_objTable.Columns(intItem).DataType Is System.Type.GetType("System.DateTime") Then
                If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                    e.Item.Cells(intItem).Text = DataBinder.Eval(e.Item.DataItem, m_objTable.Columns(intItem).ColumnName, "{0:dd.MM.yyyy}")
                End If
            End If
        Next
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub cmdDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDetails.Click
        Response.Redirect("Change01_2.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Protected Sub lnkCreateExcel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles lnkCreateExcel.Click
        CreateExcel()

    End Sub
    Private Sub Setfilter()
        Dim iStatus As Integer
        Dim sFilter As String
        Dim sFilterproof As String = ""
        sFilter = proofDropdowns()

        m_report.FilterString = sFilter
        iStatus = m_report.Filter(sFilter)
        If iStatus <> 0 Then
            FillControlls(sFilter)
            If SelOpen2.Value = "O" Then insertScript()
            Session("App_Report") = m_report

            lblResults.Text = "Es wurden " & iStatus & " Fahrzeuge gefunden."
            cmdDetails.Visible = True

            Session("App_ResultTable") = m_report.Result
            Session("App_ResultTableCarports") = m_report.ResultCarports

            Session("App_Report") = m_report
            lnkCreateExcel.Visible = True
            lnkCreateExcel2.Visible = True
            m_objTable = m_report.ResultCarports
            FillGrid(0)
        Else
            lblResults.Text = "Es wurden keine Fahrzeuge gefunden."
            rowResultate.Visible = False
            lnkCreateExcel.Visible = False
            lnkCreateExcel2.Visible = False
            If SelOpen2.Value = "O" Then insertScript()
        End If

    End Sub


    Protected Sub cmdBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdBack.Click
        Session("App_Report") = Nothing
        Session("App_ResultTable") = Nothing
        Session("App_ResultTableCarports") = Nothing
        Response.Redirect("/PortalORM/(S(" & Session.SessionID & "))/Start/Selection.aspx")
    End Sub

    Protected Sub lnkCreateExcel2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel2.Click
        CreateExcel()
    End Sub
    Private Sub CreateExcel()
        If Not (Session("App_ResultTableCarports") Is Nothing) Then
            m_objExcel = CType(Session("App_ResultTableCarports"), DataTable)

            Try
                Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
                Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName

                excelFactory.CreateDocumentAndSendAsResponse(strFileName, Me.m_objExcel, Me.Page)

            Catch ex As Exception
                lblError.Text = "Fehler beim Erstellen der Excel-Datei: " + ex.Message
            End Try
        End If
    End Sub
    Private Sub FillControlls(Optional ByVal FlagFilter As String = "")

        Dim dvCarports As DataView
        Dim dvHersteller As DataView
        Dim dvLiefertermin As DataView
        Dim dvBereifung As DataView
        Dim dvGetriebe As DataView
        Dim dvKraftstoff As DataView
        Dim dvNavi As DataView
        Dim dvFarbe As DataView
        Dim dvVermiet As DataView
        Dim dvFzgArt As DataView
        Dim dvAufbauArt As DataView
        Dim dvHaendlernr As DataView
        Dim dvHaendlername As DataView
        Dim dvEKIndikator As DataView
        Dim dvVerwZweck As DataView
        Dim dvOwnerCode As DataView
        Dim dvSperrdat As DataView
        Dim dvZulKreis As DataView
        If Session("App_Report") Is Nothing Then
        Else
            m_report = CType(Session("App_Report"), Carport)
        End If


        dvCarports = m_report.ResultPDIs.DefaultView
        dvHersteller = m_report.Hersteller.DefaultView
        dvLiefertermin = m_report.Liefermonat.DefaultView
        dvBereifung = m_report.Bereifung.DefaultView
        dvGetriebe = m_report.Getriebe.DefaultView
        dvKraftstoff = m_report.Kraftstoff.DefaultView
        dvNavi = m_report.Navi.DefaultView
        dvFarbe = m_report.Farbe.DefaultView
        dvVermiet = m_report.Vermiet.DefaultView
        dvFzgArt = m_report.FzgArt.DefaultView
        dvAufbauArt = m_report.AufbauArt.DefaultView
        dvHaendlernr = m_report.Haendlernr.DefaultView
        dvHaendlername = m_report.Handlername.DefaultView
        dvEKIndikator = m_report.EKIndikator.DefaultView
        dvVerwZweck = m_report.VerwZweck.DefaultView
        dvOwnerCode = m_report.HOwnerCode.DefaultView
        dvSperrdat = m_report.Sperrdat.DefaultView
        dvZulKreis = m_report.ZulKreis.DefaultView
        If FlagFilter.Contains("Carportnr") = False Then
            If dvCarports.Count > 0 Then
                BoundControls(dvCarports, ddlCarports, "Carport", "Carportnr", "Carportnr")
            End If
        End If

        If FlagFilter.Contains("Hersteller_ID_Avis") = False Then
            If dvHersteller.Count > 0 Then
                BoundControls(dvHersteller, ddlHersteller, "Hersteller_ID_Avis", "Hersteller_ID_Avis", "Hersteller_ID_Avis")
            End If
        End If
        If FlagFilter.Contains("Liefermonat") = False Then
            If dvLiefertermin.Count > 0 Then
                BoundControls(dvLiefertermin, ddlLiefermonat, "Liefermonat", "ID", "Liefermonat")
            End If
        End If
        If FlagFilter.Contains("Reifenart") = False Then
            If dvBereifung.Count > 0 Then
                BoundControls(dvBereifung, ddlBereifung, "REIFENART", "ID", "REIFENART")
            End If
        End If

        If FlagFilter.Contains("Antriebsart") = False Then
            If dvGetriebe.Count > 0 Then
                BoundControls(dvGetriebe, ddlGetriebe, "ANTRIEBSART", "ID", "ANTRIEBSART")
            End If
        End If

        If FlagFilter.Contains("Kraftstoffart") = False Then
            If dvKraftstoff.Count > 0 Then
                BoundControls(dvKraftstoff, ddlKraftstoff, "Kraftstoffart", "ID", "Kraftstoffart")
            End If
        End If

        If FlagFilter.Contains("Navigation") = False Then
            If dvNavi.Count > 0 Then
                BoundControls(dvNavi, ddlNavi, "Navigation", "ID", "Navigation")
            End If
        End If
        If FlagFilter.Contains("Farbe") = False Then
            If dvFarbe.Count > 0 Then
                BoundControls(dvFarbe, ddlFarbe, "FARBE", "ID", "FARBE")
                Dim List As ListItem
                For Each List In ddlFarbe.Items
                    If List.Text = "- keine Auswahl -" Then
                        List.Selected = True
                    End If
                Next

            End If
        End If

        If FlagFilter.Contains("Vermietgruppe") = False Then
            If dvVermiet.Count > 0 Then
                BoundControls(dvVermiet, ddlVermiet, "Vermietgruppe", "ID", "Vermietgruppe")
            End If
        End If

        If FlagFilter.Contains("Fahrzeugart") = False Then
            If dvFzgArt.Count > 0 Then
                BoundControls(dvFzgArt, ddlFzgArt, "Fahrzeugart", "ID", "Fahrzeugart")
            End If
        End If

        If FlagFilter.Contains("Aufbauart") = False Then
            If dvAufbauArt.Count > 0 Then
                BoundControls(dvAufbauArt, ddlAufbauArt, "AUFBAUART", "ID", "AUFBAUART")
            End If
        End If

        If FlagFilter.Contains("HaendlerId") = False Then
            If dvHaendlernr.Count > 0 Then
                BoundControls(dvHaendlernr, ddlHaendlernr, "HaendlerId", "ID", "HaendlerId")
            End If
        End If


        If FlagFilter.Contains("Haendler_Kurzname") = False Then
            If dvHaendlername.Count > 0 Then
                BoundControls(dvHaendlername, ddlHaendlername, "Haendler_Kurzname", "ID", "Haendler_Kurzname")
            End If
        End If

        If FlagFilter.Contains("Einkaufsindikator") = False Then
            If dvEKIndikator.Count > 0 Then
                BoundControls(dvEKIndikator, ddlEKIndikator, "Einkaufsindikator", "ID", "Einkaufsindikator")
            End If

        End If
        If FlagFilter.Contains("Verwendungszweck") = False Then
            If dvVerwZweck.Count > 0 Then
                BoundControls(dvVerwZweck, ddlVerwZweck, "VERWENDUNGSZWECK", "ID", "VERWENDUNGSZWECK")
            End If
        End If


        If FlagFilter.Contains("Owner_Code") = False Then
            If dvOwnerCode.Count > 0 Then
                BoundControls(dvOwnerCode, ddlOwnerCode, "OWNER_CODE", "ID", "OWNER_CODE")
            End If
        End If

        If FlagFilter.Contains("Datum_zur_Sperre") = False Then
            If dvSperrdat.Count > 0 Then
                BoundControls(dvSperrdat, ddlSperrdat, "Datum_zur_Sperre", "ID", "Datum_zur_Sperre")
            End If
        End If

    End Sub
    Private Sub BoundControls(ByVal View As DataView, ByVal Dropdown As DropDownList, ByVal Text As String, _
                                                ByVal Value As String, ByVal Sort As String)
        If View.Count > 0 Then
            View.Sort = Sort
            With Dropdown
                .DataSource = View
                .DataTextField = Text
                .DataValueField = Value
                .DataBind()
            End With
        End If
    End Sub
    Protected Sub ddlCarports_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlCarports.SelectedIndexChanged
        Setfilter()
    End Sub
    Private Sub insertScript()
        'If Not (highlightID Is Nothing) Then
        '        
        Literal1.Text = "						<script language=""Javascript"">" & vbCrLf
        Literal1.Text &= "						  <!-- //" & vbCrLf
        Literal1.Text &= "							document.getElementById('Suche2').style.display = 'block';" & vbCrLf
        Literal1.Text &= "							document.getElementById('UP').style.display = 'block';" & vbCrLf
        Literal1.Text &= "							document.getElementById('Down').style.display = 'none';" & vbCrLf
        Literal1.Text &= "						  //-->" & vbCrLf
        Literal1.Text &= "						</script>" & vbCrLf

    End Sub

    Protected Sub ddlHersteller_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlHersteller.SelectedIndexChanged
        Setfilter()
    End Sub


    Protected Sub ddlLiefermonat_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlLiefermonat.SelectedIndexChanged
        Setfilter()
    End Sub

    Protected Sub ddlBereifung_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlBereifung.SelectedIndexChanged
        Setfilter()
    End Sub

    Protected Sub ddlGetriebe_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlGetriebe.SelectedIndexChanged
        Setfilter()
    End Sub

    Protected Sub ddlKraftstoff_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlKraftstoff.SelectedIndexChanged
        Setfilter()
    End Sub


    Private Function proofDropdowns() As String
        Dim sFilter As String = ""
        If ddlCarports.SelectedItem.Value <> "-1" Then
            sFilter = "Carportnr='" & ddlCarports.SelectedItem.Value & "'"
        End If
        If ddlHersteller.SelectedItem.Value <> "- keine Auswahl -" Then
            If sFilter <> "" Then
                sFilter = sFilter & " AND Hersteller_ID_Avis='" & ddlHersteller.SelectedItem.Text & "'"
            Else
                sFilter = "Hersteller_ID_Avis='" & ddlHersteller.SelectedItem.Text & "'"
            End If
        End If
        If ddlLiefermonat.SelectedItem.Value <> "-1" Then
            If sFilter <> "" Then
                sFilter = sFilter & " AND Liefermonat='" & ddlLiefermonat.SelectedItem.Text & "'"
            Else
                sFilter = "Liefermonat='" & ddlLiefermonat.SelectedItem.Text & "'"
            End If
        End If
        If ddlBereifung.SelectedItem.Value <> "-1" Then
            If sFilter <> "" Then
                sFilter = sFilter & " AND Reifenart='" & ddlBereifung.SelectedItem.Text & "'"
            Else
                sFilter = "Reifenart='" & ddlBereifung.SelectedItem.Text & "'"
            End If
        End If
        If ddlGetriebe.SelectedItem.Value <> "-1" Then
            If sFilter <> "" Then
                sFilter = sFilter & " AND Antriebsart='" & ddlGetriebe.SelectedItem.Text & "'"
            Else
                sFilter = "Antriebsart='" & ddlGetriebe.SelectedItem.Text & "'"
            End If
        End If
        If ddlKraftstoff.SelectedItem.Value <> "-1" Then
            If sFilter <> "" Then
                sFilter = sFilter & " AND Kraftstoffart='" & ddlKraftstoff.SelectedItem.Text & "'"
            Else
                sFilter = "Kraftstoffart='" & ddlKraftstoff.SelectedItem.Text & "'"
            End If
        End If
        If ddlNavi.SelectedItem.Value <> "-1" Then
            If sFilter <> "" Then
                sFilter = sFilter & " AND Navigation='" & Left(ddlNavi.SelectedItem.Text, 1) & "'"
            Else
                sFilter = "Navigation='" & Left(ddlNavi.SelectedItem.Text, 1) & "'"
            End If
        End If

        If ddlFarbe.SelectedItem.Value <> "-1" Then
            If sFilter <> "" Then
                sFilter = sFilter & " AND Farbe='" & ddlFarbe.SelectedItem.Text & "'"
            Else
                sFilter = "Farbe='" & ddlFarbe.SelectedItem.Text & "'"
            End If
        End If

        If ddlVermiet.SelectedItem.Value <> "-1" Then
            If sFilter <> "" Then
                sFilter = sFilter & " AND Vermietgruppe='" & ddlVermiet.SelectedItem.Text & "'"
            Else
                sFilter = "Vermietgruppe='" & ddlVermiet.SelectedItem.Text & "'"
            End If
        End If

        If ddlFzgArt.SelectedItem.Value <> "-1" Then
            If sFilter <> "" Then
                sFilter = sFilter & " AND Fahrzeugart='" & ddlFzgArt.SelectedItem.Text & "'"
            Else
                sFilter = "Fahrzeugart='" & ddlFzgArt.SelectedItem.Text & "'"
            End If
        End If

        If ddlAufbauArt.SelectedItem.Value <> "-1" Then
            If sFilter <> "" Then
                sFilter = sFilter & " AND Aufbauart='" & ddlAufbauArt.SelectedItem.Text & "'"
            Else
                sFilter = "Aufbauart='" & ddlAufbauArt.SelectedItem.Text & "'"
            End If
        End If

        If ddlHaendlernr.SelectedItem.Value <> "-1" Then
            If sFilter <> "" Then
                sFilter = sFilter & " AND HaendlerId='" & ddlHaendlernr.SelectedItem.Text & "'"
            Else
                sFilter = "HaendlerId='" & ddlHaendlernr.SelectedItem.Text & "'"
            End If
        End If

        If ddlHaendlername.SelectedItem.Value <> "-1" Then
            If sFilter <> "" Then
                sFilter = sFilter & " AND Haendler_Kurzname='" & ddlHaendlername.SelectedItem.Text & "'"
            Else
                sFilter = "Haendler_Kurzname='" & ddlHaendlername.SelectedItem.Text & "'"
            End If
        End If
        If ddlEKIndikator.SelectedItem.Value <> "-1" Then
            If sFilter <> "" Then
                sFilter = sFilter & " AND Einkaufsindikator='" & ddlEKIndikator.SelectedItem.Text & "'"
            Else
                sFilter = "Einkaufsindikator='" & ddlEKIndikator.SelectedItem.Text & "'"
            End If
        End If

        If ddlVerwZweck.SelectedItem.Value <> "-1" Then
            If sFilter <> "" Then
                sFilter = sFilter & " AND Verwendungszweck='" & ddlVerwZweck.SelectedItem.Text & "'"
            Else
                sFilter = "Verwendungszweck='" & ddlVerwZweck.SelectedItem.Text & "'"
            End If
        End If

        If ddlOwnerCode.SelectedItem.Value <> "-1" Then
            If sFilter <> "" Then
                sFilter = sFilter & " AND Owner_Code='" & ddlOwnerCode.SelectedItem.Text & "'"
            Else
                sFilter = "Owner_Code='" & ddlOwnerCode.SelectedItem.Text & "'"
            End If
        End If
        If ddlSperrdat.SelectedItem.Value <> "-1" Then
            If sFilter <> "" Then
                sFilter = sFilter & " AND Datum_zur_Sperre='" & ddlSperrdat.SelectedItem.Text & "'"
            Else
                sFilter = "Datum_zur_Sperre='" & ddlSperrdat.SelectedItem.Text & "'"
            End If
        End If
        If txtEingangsdatumVon.Text <> "" Then
            If sFilter <> "" Then
                sFilter = sFilter & " AND Eingangsdatum>='" & txtEingangsdatumVon.Text & "'"
            Else
                sFilter = "Eingangsdatum>='" & txtEingangsdatumVon.Text & "'"
            End If
        End If
        If txtEingangsdatumBis.Text <> "" Then
            If sFilter <> "" Then
                sFilter = sFilter & " AND Eingangsdatum<='" & txtEingangsdatumBis.Text & "'"
            Else
                sFilter = "Eingangsdatum<='" & txtEingangsdatumBis.Text & "'"
            End If
        End If
        If txtMeldungsdatumVon.Text <> "" Then
            If sFilter <> "" Then
                sFilter = sFilter & " AND Datum_Bereit>='" & txtMeldungsdatumVon.Text & "'"
            Else
                sFilter = "Datum_Bereit>='" & txtMeldungsdatumVon.Text & "'"
            End If
        End If
        If txtMeldungsdatumBis.Text <> "" Then
            If sFilter <> "" Then
                sFilter = sFilter & " AND Datum_Bereit<='" & txtMeldungsdatumBis.Text & "'"
            Else
                sFilter = "Datum_Bereit<='" & txtMeldungsdatumBis.Text & "'"
            End If
        End If
        If rdo_Ja.Checked Then
            If sFilter <> "" Then
                sFilter = sFilter & " AND ZBII_vorhanden='J'"
            Else
                sFilter = "ZBII_vorhanden='J'"
            End If
        End If
        If rdo_Nein.Checked Then
            If sFilter <> "" Then
                sFilter = sFilter & " AND ZBII_vorhanden<>'J'"
            Else
                sFilter = "ZBII_vorhanden<>'J'"
            End If
        End If
        If rdo_JaSperr.Checked Then
            If sFilter <> "" Then
                sFilter = sFilter & " AND Sperre='X'"
            Else
                sFilter = "Sperre='X'"
            End If
        End If
        If rdo_NeinSperr.Checked Then
            If sFilter <> "" Then
                sFilter = sFilter & " AND Sperre<>'X'"
            Else
                sFilter = "Sperre<>'X'"
            End If
        End If
        If rdo_JaBereit.Checked Then
            If sFilter <> "" Then
                sFilter = sFilter & " AND Zulassungsbereit='X'"
            Else
                sFilter = "Zulassungsbereit='X'"
            End If
        End If
        If rdo_NeinBereit.Checked Then
            If sFilter <> "" Then
                sFilter = sFilter & " AND Zulassungsbereit <>'X'"
            Else
                sFilter = "Zulassungsbereit <>'X'"
            End If
        End If
        Return sFilter
    End Function


    Protected Sub ddlNavi_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlNavi.SelectedIndexChanged
        Setfilter()
    End Sub

    Protected Sub ddlFarbe_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlFarbe.SelectedIndexChanged
        Setfilter()
    End Sub

    Protected Sub ddlVermiet_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlVermiet.SelectedIndexChanged
        Setfilter()
    End Sub

    Protected Sub ddlFzgArt_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlFzgArt.SelectedIndexChanged
        Setfilter()
    End Sub

    Protected Sub ddlAufbauArt_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlAufbauArt.SelectedIndexChanged
        Setfilter()
    End Sub

    Protected Sub ddlHaendlernr_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlHaendlernr.SelectedIndexChanged
        Setfilter()
    End Sub

    Protected Sub ddlHaendlername_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlHaendlername.SelectedIndexChanged
        Setfilter()
    End Sub

    Protected Sub ddlEKIndikator_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlEKIndikator.SelectedIndexChanged
        Setfilter()
    End Sub

    Protected Sub ddlVerwZweck_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlVerwZweck.SelectedIndexChanged
        Setfilter()
    End Sub

    Protected Sub ddlOwnerCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOwnerCode.SelectedIndexChanged
        Setfilter()
    End Sub

    Protected Sub ddlSperrdat_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlSperrdat.SelectedIndexChanged
        Setfilter()
    End Sub

    Protected Sub btnCal1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnCal1.Click
        calVon.Visible = True
        If SelOpen2.Value = "O" Then insertScript()
    End Sub

    Protected Sub btnCal2_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnCal2.Click
        calBis.Visible = True
        If SelOpen2.Value = "O" Then insertScript()
    End Sub

    Protected Sub calBis_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles calBis.SelectionChanged
        Me.txtEingangsdatumBis.Text = calBis.SelectedDate.ToShortDateString
        calBis.Visible = False
        Setfilter()
    End Sub

    Protected Sub btnCalBereit1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnCalBereit1.Click
        calBisBereit.Visible = True
        If SelOpen2.Value = "O" Then insertScript()
    End Sub

    Protected Sub calBisBereit_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles calBisBereit.SelectionChanged
        Me.txtMeldungsdatumBis.Text = calBisBereit.SelectedDate.ToShortDateString
        calBisBereit.Visible = False
        Setfilter()
    End Sub

    Protected Sub btnCalBereit2_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnCalBereit2.Click
        calVonBereit.Visible = True
        If SelOpen2.Value = "O" Then insertScript()
    End Sub

    Protected Sub calVonBereit_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles calVonBereit.SelectionChanged
        Me.txtMeldungsdatumVon.Text = calVonBereit.SelectedDate.ToShortDateString
        calVonBereit.Visible = False
        Setfilter()
    End Sub

    Protected Sub ibtnDelEingVon_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnDelEingVon.Click
        txtEingangsdatumVon.Text = ""
        Setfilter()
    End Sub

    Protected Sub ibtnDelEingBis_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnDelEingBis.Click
        txtEingangsdatumBis.Text = ""
        Setfilter()
    End Sub

    Protected Sub ibtnDelBereitVon_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnDelBereitVon.Click
        txtMeldungsdatumVon.Text = ""
        Setfilter()
    End Sub

    Protected Sub ibtnDelBereitBis_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnDelBereitBis.Click
        txtMeldungsdatumBis.Text = ""
        Setfilter()
    End Sub

    Protected Sub rdo_Alle_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rdo_Alle.CheckedChanged
        Setfilter()
        If SelOpen2.Value = "O" Then insertScript()
    End Sub

    Protected Sub rdo_Ja_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rdo_Ja.CheckedChanged
        Setfilter()
        If SelOpen2.Value = "O" Then insertScript()
    End Sub

    Protected Sub rdo_Nein_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rdo_Nein.CheckedChanged
        Setfilter()
        If SelOpen2.Value = "O" Then insertScript()
    End Sub

    Protected Sub rdo_AlleSperr_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rdo_AlleSperr.CheckedChanged
        Setfilter()
        If SelOpen2.Value = "O" Then insertScript()
    End Sub

    Protected Sub rdo_JaSperr_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rdo_JaSperr.CheckedChanged
        Setfilter()
        If SelOpen2.Value = "O" Then insertScript()
    End Sub

    Protected Sub rdo_NeinSperr_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rdo_NeinSperr.CheckedChanged
        Setfilter()
        If SelOpen2.Value = "O" Then insertScript()
    End Sub

    Protected Sub rdo_AlleBereit_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rdo_AlleBereit.CheckedChanged
        Setfilter()
        If SelOpen2.Value = "O" Then insertScript()
    End Sub

    Protected Sub rdo_JaBereit_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rdo_JaBereit.CheckedChanged
        Setfilter()
        If SelOpen2.Value = "O" Then insertScript()
    End Sub

    Protected Sub rdo_NeinBereit_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rdo_NeinBereit.CheckedChanged
        Setfilter()
        If SelOpen2.Value = "O" Then insertScript()
    End Sub
End Class
' ************************************************
' $History: Change01.aspx.vb $
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 12.01.09   Time: 17:49
' Updated in $/CKAG/Applications/AppAvis/forms
' Bugfix
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 4.12.08    Time: 16:01
' Updated in $/CKAG/Applications/AppAvis/forms
' ITA: 2359, 2352
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 4.12.08    Time: 13:42
' Updated in $/CKAG/Applications/AppAvis/forms
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 1.12.08    Time: 16:13
' Updated in $/CKAG/Applications/AppAvis/forms
' ITA: 2359 & kleinere Anpassungen
' 
' ************************************************
