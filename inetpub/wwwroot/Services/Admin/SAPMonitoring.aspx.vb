Option Strict On
Option Explicit On

Imports CKG.Base.Kernel.Common
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Logging
Imports CKG.Base.Business
Imports Telerik.Web.UI
Imports Telerik.Web.UI.GridExcelBuilder

Public Class SAPMonitoring
    Inherits Page

    Private m_User As User
    Private m_App As App
    Private isExcelExportConfigured As Boolean
    Private dtBAPI As New DataTable
    Private dsASPX As New DataSet

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load

        m_User = Common.GetUser(Me)
        Common.AdminAuth(Me, m_User, AdminLevel.Organization)

        m_App = New App(m_User)
        Common.GetAppIDFromQueryString(Me)

        lblHead.Text = CStr(m_User.Applications.Select("AppID = '" & Session("AppID").ToString() & "'")(0)("AppFriendlyName"))
        lblError.Text = ""

        Try
            If Session("objBAPI") IsNot Nothing Then
                dtBAPI = CType(Session("objBAPI"), DataTable)
            End If
            If Session("objASPX") IsNot Nothing Then
                dsASPX = CType(Session("objASPX"), DataSet)
            End If

            If Not IsPostBack Then
                Common.TranslateTelerikColumns(rgGrid1)
                Common.TranslateTelerikColumns(rgGrid2)

                FillForm()

                txtAbDatum.Text = Today.ToString("dd.MM.yyyy")
                txtBisDatum.Text = Today.ToString("dd.MM.yyyy")
            End If
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "SAPMonitoring", "Page_Load", ex.ToString)
            lblError.Text = ex.ToString
        End Try
    End Sub

    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        Common.SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(sender As Object, e As EventArgs) Handles Me.Unload
        Common.SetEndASPXAccess(Me)
    End Sub

    Private Sub rbBAPI_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbBAPI.CheckedChanged
        If rbBAPI.Checked Then
            FillForm()
        End If
    End Sub

    Private Sub rbASPX_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbASPX.CheckedChanged
        If rbASPX.Checked Then
            FillForm()
        End If
    End Sub

    Private Sub FillForm()
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()
        If rbBAPI.Checked Then
            FillBAPI(cn)
        Else
            FillASPX(cn)
        End If
    End Sub

    Private Sub FillASPX(ByVal cn As SqlClient.SqlConnection)
        Dim dt As New DataTable()
        Dim da As New SqlClient.SqlDataAdapter("SELECT DISTINCT Seite, Seite + ' - ' + AppFriendlyName AS Anzeige FROM vwASPXBAPI ORDER BY Seite", cn)
        da.Fill(dt)
        Dim dr As DataRow = dt.NewRow
        dr("Seite") = ""
        dr("Anzeige") = "- alle -"
        dt.Rows.Add(dr)

        dt.DefaultView.Sort = "Seite"
        With ddlAuswahl
            .DataTextField = "Anzeige"
            .DataValueField = "Seite"
            .DataSource = dt.DefaultView
            .DataBind()
        End With
    End Sub

    Private Sub FillBAPI(ByVal cn As SqlClient.SqlConnection)
        Dim dt As New DataTable()
        Dim da As New SqlClient.SqlDataAdapter("SELECT DISTINCT BAPI, BAPI AS Anzeige FROM LogAccessSAP ORDER BY BAPI", cn)
        da.Fill(dt)
        Dim dr As DataRow = dt.NewRow
        dr("BAPI") = ""
        dr("Anzeige") = "- alle -"
        dt.Rows.Add(dr)

        dt.DefaultView.Sort = "BAPI"
        With ddlAuswahl
            .DataTextField = "Anzeige"
            .DataValueField = "BAPI"
            .DataSource = dt.DefaultView
            .DataBind()
        End With
    End Sub

    Protected Sub lbCreate_Click(sender As Object, e As EventArgs)

        If Not IsDate(txtAbDatum.Text) AndAlso Not Common.IsStandardDate(txtAbDatum.Text) AndAlso Not Common.IsSAPDate(txtAbDatum.Text) Then
            lblError.Text = "Geben Sie bitte ein gültiges Datum ein!"
            Exit Sub
        End If
        If Not IsDate(txtBisDatum.Text) AndAlso Not Common.IsStandardDate(txtBisDatum.Text) AndAlso Not Common.IsSAPDate(txtBisDatum.Text) Then
            lblError.Text = "Geben Sie bitte ein gültiges Datum ein!"
            Exit Sub
        End If

        Dim datAb As Date = CDate(txtAbDatum.Text)
        Dim datBis As Date = CDate(txtBisDatum.Text)
        If datAb > datBis Then
            lblError.Text = "Das Enddatum darf nicht vor dem Anfangsdatum liegen!"
            Exit Sub
        End If

        SearchMode(False)

        If rbBAPI.Checked Then
            ShowGrid(1)
            FillDataGrid1()
        Else
            ShowGrid(2)
            FillDataGrid2()
        End If

    End Sub

    Private Sub ShowGrid(gridnr As Integer)
        Select Case gridnr
            Case 1
                rgGrid2.Visible = False
                rgGrid1.Visible = True
            Case Else
                rgGrid1.Visible = False
                rgGrid2.Visible = True
        End Select
    End Sub

    Private Sub FillDataGrid1()

        dtBAPI.Clear()

        Dim strTemp As String
        strTemp = CStr(DateAdd(DateInterval.Day, 1, CDate(Me.txtBisDatum.Text)))

        Using cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            cn.Open()
            Using da As New SqlClient.SqlDataAdapter("SELECT LogStandard.UserName AS Benutzer, IsTestUser AS Testbenutzer, BAPI,Description, StartTime AS Start, EndTime AS Ende, DATEDIFF(second, StartTime, EndTime) As Dauer, Sucess AS Erfolg, ErrorMessage AS Fehlermeldung FROM LogAccessSAP inner join LogStandard ON LogAccessSAP.StandardLogID = LogStandard.ID WHERE BAPI LIKE @BAPI AND StartTime BETWEEN CONVERT ( Datetime , '" & Me.txtAbDatum.Text & "' , 104 ) AND CONVERT ( Datetime , '" & strTemp & "' , 104 ) ORDER BY BAPI", cn)

                Dim strBAPI As String = Me.ddlAuswahl.SelectedItem.Value
                If String.IsNullOrEmpty(strBAPI) Then strBAPI = "%"
                da.SelectCommand.CommandTimeout = 120
                da.SelectCommand.Parameters.AddWithValue("@BAPI", strBAPI)
                da.Fill(dtBAPI)

            End Using
            cn.Close()
        End Using

        Dim tmpRow As DataRow
        For Each tmpRow In dtBAPI.Rows
            If TypeOf tmpRow("Dauer") Is System.DBNull Then
                tmpRow("Dauer") = 0
            End If
        Next
        dtBAPI.AcceptChanges()

        Session("objBAPI") = dtBAPI

        rgGrid1.Rebind()
        'Setzen der DataSource geschieht durch das NeedDataSource-Event
    End Sub

    Private Sub SearchMode(Optional search As Boolean = True)
        NewSearch.Visible = Not search
        NewSearchUp.Visible = search
        Panel1.Visible = search
        lbCreate.Visible = search
        Result.Visible = Not search
    End Sub

    Protected Sub rgGrid1_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs)
        If dtBAPI IsNot Nothing Then
            rgGrid1.DataSource = dtBAPI.DefaultView
        Else
            rgGrid1.DataSource = Nothing
        End If
    End Sub

    Private Sub FillDataGrid2()

        dsASPX.Relations.Clear()
        dsASPX.Tables.Clear()

        If Not IsDate(Me.txtAbDatum.Text) Then
            Me.lblError.Text = "Bitte Datum/Startdatum übergeben."
            Exit Sub
        ElseIf Not IsDate(Me.txtBisDatum.Text) Then
            Me.lblError.Text = "Bitte Datum/Enddatum übergeben."
            Exit Sub
        Else
            Dim datTemp As Date
            datTemp = DateAdd(DateInterval.Day, 1, CDate(Me.txtBisDatum.Text))

            Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            cn.Open()

            Dim strASPX As String = Me.ddlAuswahl.SelectedItem.Value
            If strASPX = String.Empty Then strASPX = "%"
            
            Dim dta As New DataTable("ASPX")
            Dim da As New SqlClient.SqlDataAdapter("SELECT TOP 100 PERCENT " & _
                                                    "     ASPX_ID, " & _
                                                    "     Seite, " & _
                                                    "     Anwendung, " & _
                                                    "     [Start ASPX], " & _
                                                    "     [Ende ASPX], " & _
                                                    "     [Dauer ASPX], " & _
                                                    "     Benutzer, " & _
                                                    "     Kunnr, " & _
                                                    "     Customername, " & _
                                                    "     AccountingArea, " & _
                                                    "     Testbenutzer, " & _
                                                    "     [Dauer SAP], " & _
                                                    "     [Zugriffe SAP] " & _
                                                    "FROM dbo.vwASPXBAPI_SUM " & _
                                                    "WHERE " & _
                                                    "     (Seite LIKE @Seite) " & _
                                                    "     AND ([Start ASPX] BETWEEN @von AND @bis) " & _
                                                    "ORDER BY Seite ", cn)
            With da.SelectCommand.Parameters
                .AddWithValue("@Seite", strASPX)
                .AddWithValue("@von", CDate(Me.txtAbDatum.Text))
                .AddWithValue("@bis", datTemp)
            End With
            da.Fill(dta)

            For Each tmpRow As DataRow In dta.Rows
                If TypeOf tmpRow("Dauer ASPX") Is System.DBNull Then
                    tmpRow("Dauer ASPX") = 0
                End If
                If TypeOf tmpRow("Dauer SAP") Is System.DBNull Then
                    tmpRow("Dauer SAP") = 0
                End If
            Next
            dsASPX.Tables.Add(dta)

            Dim dtb As New DataTable("BAPI")
            da = New SqlClient.SqlDataAdapter("SELECT TOP 100 PERCENT " & _
                                                    "     SAP_ID, " & _
                                                    "     ASPX_ID, " & _
                                                    "     BAPI, " & _
                                                    "     Beschreibung, " & _
                                                    "     [Start SAP], " & _
                                                    "     [Ende SAP], " & _
                                                    "     [Dauer SAP], " & _
                                                    "     Erfolg, " & _
                                                    "     Fehlermeldung " & _
                                                    "FROM dbo.vwASPXBAPI_DETAIL " & _
                                                    "WHERE " & _
                                                    "      ([Start SAP] BETWEEN @von AND @bis) ", _
                                                    cn)
            With da.SelectCommand.Parameters
                .AddWithValue("@von", CDate(Me.txtAbDatum.Text))
                .AddWithValue("@bis", datTemp)
            End With

            'Abfrage braucht sehr lange
            da.SelectCommand.CommandTimeout = 300

            da.Fill(dtb)
            dsASPX.Tables.Add(dtb)

            Dim dc1 As DataColumn
            Dim dc2 As DataColumn
            'Relation ASPX => BAPI
            dc1 = dsASPX.Tables("ASPX").Columns("ASPX_ID")
            dc2 = dsASPX.Tables("BAPI").Columns("ASPX_ID")
            Dim dr As DataRelation = New DataRelation("ASPX_BAPI", dc1, dc2, False)
            dsASPX.Relations.Add(dr)
            dsASPX.AcceptChanges()

            Session("objASPX") = dsASPX

        End If

        rgGrid2.Rebind()
        'Setzen der DataSource geschieht durch das NeedDataSource-Event

    End Sub

    Protected Sub rgGrid2_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs)
        If dsASPX IsNot Nothing Then
            rgGrid2.MasterTableView.DataSource = dsASPX.Tables("ASPX")
            rgGrid2.MasterTableView.DetailTables(0).DataSource = dsASPX.Tables("BAPI")
        Else
            rgGrid2.DataSource = Nothing
        End If
    End Sub

    Protected Sub NewSearch_Click(sender As Object, e As ImageClickEventArgs)
        SearchMode()
    End Sub

    Protected Sub NewSearchUp_Click(sender As Object, e As ImageClickEventArgs)
        SearchMode(False)
    End Sub

    Protected Sub lbBack_Click(sender As Object, e As EventArgs)
        Session("objBAPI") = Nothing
        Session("objASPX") = Nothing
        Response.Redirect("/Services/(S(" & Session.SessionID & "))/Start/Selection.aspx")
    End Sub

    Protected Sub rgGrid1_ItemCommand(sender As Object, e As GridCommandEventArgs)
        Select Case e.CommandName

            Case RadGrid.ExportToExcelCommandName
                Dim eSettings As GridExportSettings = rgGrid1.ExportSettings
                eSettings.ExportOnlyData = True
                eSettings.FileName = String.Format("BAPI_{0:yyyyMMdd}", DateTime.Now)
                eSettings.HideStructureColumns = True
                eSettings.IgnorePaging = True
                eSettings.OpenInNewWindow = True
                ' hide non display columns from excel export
                For Each col As GridColumn In rgGrid1.MasterTableView.Columns
                    If TypeOf col Is GridEditableColumn AndAlso Not col.Display Then
                        col.Visible = False
                    End If
                Next
                rgGrid1.Rebind()
                rgGrid1.MasterTableView.ExportToExcel()

        End Select
    End Sub

    Protected Sub rgGrid2_ItemCommand(sender As Object, e As GridCommandEventArgs)
        Select Case e.CommandName

            Case RadGrid.ExportToExcelCommandName
                Dim eSettings As GridExportSettings = rgGrid2.ExportSettings
                eSettings.ExportOnlyData = True
                eSettings.FileName = String.Format("ASPX_{0:yyyyMMdd}", DateTime.Now)
                eSettings.HideStructureColumns = True
                eSettings.IgnorePaging = True
                eSettings.OpenInNewWindow = True
                ' hide non display columns from excel export
                For Each col As GridColumn In rgGrid2.MasterTableView.Columns
                    If TypeOf col Is GridEditableColumn AndAlso Not col.Display Then
                        col.Visible = False
                    End If
                Next
                rgGrid2.Rebind()
                rgGrid2.MasterTableView.ExportToExcel()

        End Select
    End Sub

    Protected Sub rgGrid_ExcelMLExportRowCreated(sender As Object, e As GridExportExcelMLRowCreatedArgs)

        If e.RowType = GridExportExcelMLRowType.DataRow Then

            If Not isExcelExportConfigured Then
                e.Worksheet.Name = "Seite 1"

                'Set Page options
                Dim layout As PageLayoutElement = e.Worksheet.WorksheetOptions.PageSetup.PageLayoutElement
                layout.IsCenteredVertical = True
                layout.IsCenteredHorizontal = True
                layout.PageOrientation = PageOrientationType.Landscape
                Dim margins As PageMarginsElement = e.Worksheet.WorksheetOptions.PageSetup.PageMarginsElement
                margins.Left = 0.5
                margins.Top = 0.5
                margins.Right = 0.5
                margins.Bottom = 0.5

                'Freeze panes
                Dim wso As WorksheetOptionsElement = e.Worksheet.WorksheetOptions
                wso.AllowFreezePanes = True
                wso.LeftColumnRightPaneNumber = 1
                wso.TopRowBottomPaneNumber = 1
                wso.SplitHorizontalOffset = 1
                wso.SplitVerticalOffest = 1
                wso.ActivePane = 2

                isExcelExportConfigured = True
            End If

        End If

    End Sub

    Protected Sub rgGrid_ExcelMLExportStylesCreated(sender As Object, e As GridExportExcelMLStyleCreatedArgs)

        'Add currency and percent styles
        Dim priceStyle As New StyleElement("priceItemStyle")
        priceStyle.NumberFormat.FormatType = NumberFormatType.Currency
        priceStyle.FontStyle.Color = System.Drawing.Color.Red
        e.Styles.Add(priceStyle)

        Dim alternatingPriceStyle As New StyleElement("alternatingPriceItemStyle")
        alternatingPriceStyle.NumberFormat.FormatType = NumberFormatType.Currency
        alternatingPriceStyle.FontStyle.Color = System.Drawing.Color.Red
        e.Styles.Add(alternatingPriceStyle)

        Dim percentStyle As New StyleElement("percentItemStyle")
        percentStyle.NumberFormat.FormatType = NumberFormatType.Percent
        percentStyle.FontStyle.Italic = True
        e.Styles.Add(percentStyle)

        Dim alternatingPercentStyle As New StyleElement("alternatingPercentItemStyle")
        alternatingPercentStyle.NumberFormat.FormatType = NumberFormatType.Percent
        alternatingPercentStyle.FontStyle.Italic = True
        e.Styles.Add(alternatingPercentStyle)

        'Apply background colors 
        For Each style As StyleElement In e.Styles
            If style.Id = "headerStyle" Then
                style.InteriorStyle.Pattern = InteriorPatternType.Solid
                style.InteriorStyle.Color = System.Drawing.Color.Gray
            End If
            If style.Id = "alternatingItemStyle" Or style.Id = "alternatingPriceItemStyle" Or style.Id = "alternatingPercentItemStyle" Or style.Id = "alternatingDateItemStyle" Then
                style.InteriorStyle.Pattern = InteriorPatternType.Solid
                style.InteriorStyle.Color = System.Drawing.Color.LightGray
            End If
            If style.Id.Contains("itemStyle") Or style.Id = "priceItemStyle" Or style.Id = "percentItemStyle" Or style.Id = "dateItemStyle" Then
                style.InteriorStyle.Pattern = InteriorPatternType.Solid
                style.InteriorStyle.Color = System.Drawing.Color.White
            End If
        Next

    End Sub

End Class
