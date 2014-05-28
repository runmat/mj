Option Strict On
Option Explicit On

Imports CKG.Base.Kernel.Common
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Logging
Imports CKG.Base.Business
Imports Telerik.Web.UI
Imports Telerik.Web.UI.GridExcelBuilder

Public Class jve_ZugriffsHistorie
    Inherits System.Web.UI.Page

    Private m_User As User
    Private m_App As App
    Private isExcelExportConfigured As Boolean
    Private dsLogData As New DataSet

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load

        m_User = Common.GetUser(Me)
        Common.AdminAuth(Me, m_User, AdminLevel.Organization)

        m_App = New App(m_User)
        Common.GetAppIDFromQueryString(Me)

        lblHead.Text = CStr(m_User.Applications.Select("AppID = '" & Session("AppID").ToString() & "'")(0)("AppFriendlyName"))
        lblError.Text = ""

        If Session("dsLogData") IsNot Nothing Then
            dsLogData = CType(Session("dsLogData"), DataSet)
        End If

        If Not IsPostBack Then
            Try
                Common.TranslateTelerikColumns(rgGrid1)

                With ddbZeit.Items
                    .Add("1")
                    .Add("2")
                    .Add("3")
                    .Add("4")
                    .Add("5")
                    .Add("6")
                End With

                txtAbDatum.Text = Today.ToString("dd.MM.yyyy")
                txtBisDatum.Text = Today.ToString("dd.MM.yyyy")

                Using cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
                    cn.Open()

                    Dim cmdGetUser As New SqlClient.SqlCommand("SELECT DISTINCT Customername FROM dbo.Customer WHERE NOT (Customername like '') ORDER BY Customername", cn)
                    Dim dt As New DataTable()
                    Dim da As New SqlClient.SqlDataAdapter()
                    da.SelectCommand = cmdGetUser
                    da.Fill(dt)

                    dt.Columns.Add("ID", System.Type.GetType("System.Int32"))
                    Dim intTemp As Int32 = 1
                    Dim rowTemp As DataRow
                    For Each rowTemp In dt.Rows
                        rowTemp("ID") = intTemp
                        intTemp += 1
                    Next
                    rowTemp = dt.NewRow
                    rowTemp("ID") = 0
                    rowTemp("Customername") = "- alle -"
                    dt.Rows.Add(rowTemp)
                    Dim dv As DataView = dt.DefaultView
                    dv.Sort = "ID"

                    ddlCustomer.DataSource = dv
                    ddlCustomer.DataTextField = "Customername"
                    ddlCustomer.DataValueField = "Customername"
                    ddlCustomer.DataBind()
                    ddlCustomer.SelectedIndex = 0

                    cn.Close()
                End Using

            Catch ex As Exception
                m_App.WriteErrorText(1, m_User.UserName, "jve_ZugriffsHistorieAbfrage", "Page_Load", ex.ToString)
                lblError.Text = ex.ToString
            End Try
        End If
    End Sub

    Private Sub Page_PreRender(sender As Object, e As EventArgs)
        Common.SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(sender As Object, e As EventArgs)
        Common.SetEndASPXAccess(Me)
    End Sub

    Protected Sub lbCreate_Click(sender As Object, e As EventArgs)

        If Not IsDate(txtAbDatum.Text) AndAlso Not Common.IsStandardDate(txtAbDatum.Text) AndAlso Not Common.IsSAPDate(txtAbDatum.Text) Then
            lblError.Text = "Ab Datum enthält kein gültiges Datum. Geben Sie bitte ein gültiges Datum ein."
            Exit Sub
        End If
        If Not IsDate(txtBisDatum.Text) AndAlso Not Common.IsStandardDate(txtBisDatum.Text) AndAlso Not Common.IsSAPDate(txtBisDatum.Text) Then
            lblError.Text = "Bis Datum enthält kein gültiges Datum. Geben Sie bitte ein gültiges Datum ein."
            Exit Sub
        End If

        If System.DateTime.Compare(CDate(txtAbDatum.Text), CDate(txtBisDatum.Text)) > 0 Then
            lblError.Text = "Das Startdatum muss vor dem Enddatum liegen."
            Exit Sub
        End If

        FillDataGrid()

    End Sub

    Private Sub FillDataGrid()
        Dim dtUser As DataTable
        Dim dtAppDaten As DataTable
        Dim dtSAPDaten As DataTable
        Dim sql As String
        Dim sql1 As String
        Dim sql2 As String

        dsLogData.Relations.Clear()
        dsLogData.Tables.Clear()

        Try
            Dim strCustomerRestriction As String = ""
            If Not ddlCustomer.SelectedItem.Text = "- alle -" Then
                strCustomerRestriction = " AND Customername = '" & ddlCustomer.SelectedItem.Text & "'"
            End If

            Dim strUserRestriction As String = ""
            If (Not Replace(txtFilterUserName.Text, "*", "").Trim(" "c).Length = 0) Then
                strUserRestriction = " AND userName like '" & Replace(txtFilterUserName.Text, "*", "%") & "'"
            End If

            If rbOnline.Checked Then
                sql = "SELECT * FROM vwLogWebAccess WHERE endTime is Null" & strCustomerRestriction & strUserRestriction & " ORDER BY startTime DESC"
                sql1 = "SELECT * FROM vwLogStandardData ORDER BY Inserted DESC"
                sql2 = "SELECT * FROM vwLogStandardDataSAP WHERE endTime is Null ORDER BY startTime DESC"
            Else
                Dim strBisDatum As String = CStr(CDate(txtBisDatum.Text).AddDays(1).ToShortDateString)
                If rbError.Checked Then
                    sql = "SELECT * FROM vwLogError WHERE (startTime BETWEEN '" & txtAbDatum.Text & "' AND '" & strBisDatum & "')" & strCustomerRestriction & strUserRestriction & " ORDER BY startTime DESC"
                    sql1 = "SELECT * FROM vwLogStandardError WHERE (Inserted BETWEEN '" & txtAbDatum.Text & "' AND '" & strBisDatum & "') ORDER BY Inserted DESC"
                    sql2 = "SELECT * FROM vwLogStandardDataSAP WHERE (startTime BETWEEN '" & txtAbDatum.Text & "' AND '" & strBisDatum & "') AND (Sucess = 0) ORDER BY startTime DESC"
                Else
                    sql = "SELECT * FROM vwLogWebAccess WHERE (startTime BETWEEN '" & txtAbDatum.Text & "' AND '" & strBisDatum & "')" & strCustomerRestriction & strUserRestriction & " ORDER BY startTime DESC"
                    sql1 = "SELECT * FROM vwLogStandardData WHERE (Inserted BETWEEN '" & txtAbDatum.Text & "' AND '" & strBisDatum & "') ORDER BY Inserted DESC"
                    sql2 = "SELECT * FROM vwLogStandardDataSAP WHERE (startTime BETWEEN '" & txtAbDatum.Text & "' AND '" & strBisDatum & "') ORDER BY startTime DESC"
                End If
            End If

            Using cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
                cn.Open()

                Dim da As SqlClient.SqlDataAdapter

                da = New SqlClient.SqlDataAdapter(sql, cn)
                dtUser = New DataTable("Benutzer")
                da.Fill(dtUser)
                dtUser.Columns.Add("StartColor", System.Drawing.Color.Red.GetType)
                Dim rowTemp As DataRow
                dtUser.AcceptChanges()
                For Each rowTemp In dtUser.Rows
                    If (CDate(rowTemp("startTime")).AddHours(CType(ddbZeit.SelectedItem.Value, Integer)) < Now) And (TypeOf rowTemp("endTime") Is DBNull) Then
                        rowTemp("StartColor") = System.Drawing.Color.Red
                    Else
                        rowTemp("StartColor") = System.Drawing.Color.Black
                    End If
                Next
                dtUser.AcceptChanges()
                dsLogData.Tables.Add(dtUser)

                da = New SqlClient.SqlDataAdapter(sql1, cn)
                dtAppDaten = New DataTable("AppDaten")
                da.Fill(dtAppDaten)
                dsLogData.Tables.Add(dtAppDaten)

                da = New SqlClient.SqlDataAdapter(sql2, cn)
                dtSAPDaten = New DataTable("SAPDaten")
                da.Fill(dtSAPDaten)
                dsLogData.Tables.Add(dtSAPDaten)

                Dim dc1 As DataColumn
                Dim dc2 As DataColumn
                'Relation Benutzer => AppDaten
                dc1 = dsLogData.Tables("Benutzer").Columns("idSession")
                dc2 = dsLogData.Tables("AppDaten").Columns("idSession")
                Dim dr As DataRelation = New DataRelation("Benutzer_AppDaten", dc1, dc2, False)
                dsLogData.Relations.Add(dr)

                'Relation AppDaten => SAPDaten
                dc1 = dsLogData.Tables("AppDaten").Columns("StandardLogID")
                dc2 = dsLogData.Tables("SAPDaten").Columns("StandardLogID")
                dr = New DataRelation("AppDaten_SAPDaten", dc1, dc2, False)
                dsLogData.Relations.Add(dr)

                cn.Close()
            End Using

            Session("dsLogData") = dsLogData

        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "jve_LogMessage2", "checkData", ex.ToString)
            lblError.Text = "Fehler beim Lesen aus der Datenbank. (" & ex.Message & ")"
            Exit Sub
        End Try

        If dtUser.Rows.Count = 0 Then
            SearchMode()
            lblError.Text = "Keine Datensätze gefunden."
        Else
            SearchMode(False)

            rgGrid1.Rebind()
            'Setzen der DataSource geschieht durch das NeedDataSource-Event
        End If

    End Sub

    Private Sub SearchMode(Optional search As Boolean = True)
        NewSearch.Visible = Not search
        NewSearchUp.Visible = search
        Panel1.Visible = search
        lbCreate.Visible = search
        Result.Visible = Not search
    End Sub

    Protected Sub rgGrid1_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs)
        If dsLogData IsNot Nothing Then
            rgGrid1.MasterTableView.DataSource = dsLogData.Tables("Benutzer")
            rgGrid1.MasterTableView.DetailTables(0).DataSource = dsLogData.Tables("AppDaten")
            rgGrid1.MasterTableView.DetailTables(0).DetailTables(0).DataSource = dsLogData.Tables("SAPDaten")
        Else
            rgGrid1.DataSource = Nothing
        End If
    End Sub

    Protected Sub NewSearch_Click(sender As Object, e As ImageClickEventArgs)
        SearchMode()
    End Sub

    Protected Sub NewSearchUp_Click(sender As Object, e As ImageClickEventArgs)
        SearchMode(False)
    End Sub

    Protected Sub lbBack_Click(sender As Object, e As EventArgs)
        Session("dsLogData") = Nothing
        Response.Redirect("/Services/(S(" & Session.SessionID & "))/Start/Selection.aspx")
    End Sub

    Protected Sub rgGrid1_ItemCommand(sender As Object, e As GridCommandEventArgs)
        Select Case e.CommandName

            Case RadGrid.ExportToExcelCommandName
                Dim eSettings As GridExportSettings = rgGrid1.ExportSettings
                eSettings.ExportOnlyData = True
                eSettings.FileName = String.Format("Zugriffshistorie_{0:yyyyMMdd}", DateTime.Now)
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

    Protected Sub rgGrid1_ExcelMLExportRowCreated(sender As Object, e As GridExportExcelMLRowCreatedArgs)

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

    Protected Sub rgGrid1_ExcelMLExportStylesCreated(sender As Object, e As GridExportExcelMLStyleCreatedArgs)

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
