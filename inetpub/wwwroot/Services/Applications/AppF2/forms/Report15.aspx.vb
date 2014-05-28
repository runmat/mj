﻿Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common
Imports System.Reflection
Imports Telerik.Web.UI
Imports Telerik.Web.UI.GridExcelBuilder

Public Class Report15
    Inherits System.Web.UI.Page

    Dim m_User As User
    Dim m_App As App
    Protected kopfdaten As CKG.Services.PageElements.Kopfdaten
    Dim m_TW As TraceWrapper
    Dim isExcelExportConfigured As Boolean

    Protected Overrides Sub OnLoad(e As System.EventArgs)
        MyBase.OnLoad(e)

        m_User = Common.GetUser(Me)
        Common.FormAuth(Me, m_User)
        m_App = New App(m_User)
        Common.GetAppIDFromQueryString(Me)
        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

        If Not IsPostBack Then
            Dim search = New DealerSearch(m_User, m_App, Session("AppID"), Session.SessionID)
            search.LoadData(Me, m_User.Reference)

            If search.Result Is Nothing OrElse search.Result.Rows.Count <> 1 Then
                lblError.Text = "Händler konnte nicht gefunden werden."
                lblError.Visible = True
                Exit Sub
            End If

            Dim r = search.Result.Rows.Cast(Of DataRow).First

            Dim haendler = CStr(r("HAENDLER"))
            If String.IsNullOrEmpty(haendler) Then Throw New Exception("Haendler nicht gesetzt.")

            Session("HAENDLER_EX") = CType(r("HAENDLER_EX"), String)
            Dim name1 = CType(r("NAME1"), String)
            Dim name2 = CType(r("NAME2"), String)
            Session("HAENDLER_NAME") = IIf(String.IsNullOrEmpty(name2), name1, name1 & "<br />" & name2)
            Session("HAENDLER_ADDR") = String.Format("{0} - {1} {2}<br />{3}", r("LAND1"), r("PSTLZ"), r("ORT01"), r("STRAS"))
            Session("HAENDLER") = haendler
        End If

        kopfdaten.UserReferenz = m_User.Reference
        kopfdaten.HaendlerNummer = Session("HAENDLER_EX")
        kopfdaten.HaendlerName = Session("HAENDLER_NAME")
        kopfdaten.Adresse = Session("HAENDLER_ADDR")

        m_TW = Common.GetOrCreateObject("TW", Function() New TraceWrapper(m_App, m_User, Session.SessionID))
    End Sub

    Protected Overrides Sub OnUnload(e As System.EventArgs)
        MyBase.OnUnload(e)

        Common.SetEndASPXAccess(Me)
    End Sub

    Protected Overrides Sub OnPreRender(e As System.EventArgs)
        MyBase.OnPreRender(e)

        Common.TranslateTelerikColumns(logGrid)
        Common.SetEndASPXAccess(Me)
    End Sub

    'Protected Sub NavigateBack(ByVal sender As Object, ByVal e As EventArgs)
    '    Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx")
    'End Sub

    Protected Sub lbBack_Click(sender As Object, e As EventArgs) Handles lbBack.Click
        Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx")
    End Sub

    Protected Sub NewSearch_Click(sender As Object, e As ImageClickEventArgs)
        ShowSuche(True)
    End Sub

    Protected Sub NewSearchUp_Click(sender As Object, e As ImageClickEventArgs)
        ShowSuche(False)
    End Sub


    Private Sub ShowSuche(Optional ByVal open As Boolean = True)
        If open Then
            NewSearch.Visible = False
            NewSearchUp.Visible = True
            lblNewSearch.Visible = False
            tab1.Visible = True
            Queryfooter.Visible = True
        Else
            NewSearch.Visible = True
            NewSearchUp.Visible = False
            lblNewSearch.Visible = True
            tab1.Visible = False
            Queryfooter.Visible = False
        End If
    End Sub

    Protected Sub SearchClick(ByVal sender As Object, ByVal e As EventArgs)
        LoadData()

        If logGrid.Visible AndAlso logGrid.Items.Count > 0 Then ShowSuche(False)
    End Sub

    Protected Sub GridItemCommand(ByVal sender As Object, ByVal e As GridCommandEventArgs)
        Dim exportCommands = {RadGrid.ExportToCsvCommandName, RadGrid.ExportToExcelCommandName,
                                   RadGrid.ExportToPdfCommandName, RadGrid.ExportToWordCommandName}
        If exportCommands.Any(Function(c) c = e.CommandName) Then

            Dim eSettings = logGrid.ExportSettings

            eSettings.ExportOnlyData = True
            eSettings.FileName = String.Format("Ausgeführte_Tätigkeiten_{0:yyyyMMdd}", DateTime.Now)
            eSettings.HideStructureColumns = True
            eSettings.IgnorePaging = True
            eSettings.OpenInNewWindow = True

            Select Case e.CommandName
                Case RadGrid.ExportToExcelCommandName
                    logGrid.MasterTableView.ExportToExcel()
                Case RadGrid.ExportToWordCommandName
                    logGrid.MasterTableView.ExportToWord()
                Case RadGrid.ExportToPdfCommandName
                    logGrid.MasterTableView.ExportToPdf()
            End Select
        End If
    End Sub

    Protected Sub GridExportRowCreated(ByVal sender As Object, ByVal e As GridExportExcelMLRowCreatedArgs)
        If e.RowType = GridExportExcelMLRowType.DataRow Then
            If Not isExcelExportConfigured Then
                ' Set Page options
                Dim pageSetup = e.Worksheet.WorksheetOptions.PageSetup
                pageSetup.PageLayoutElement.IsCenteredVertical = True
                pageSetup.PageLayoutElement.IsCenteredHorizontal = True
                pageSetup.PageMarginsElement.Left = 0.5
                pageSetup.PageMarginsElement.Top = 0.5
                pageSetup.PageMarginsElement.Right = 0.5
                pageSetup.PageMarginsElement.Bottom = 0.5
                pageSetup.PageLayoutElement.PageOrientation = PageOrientationType.Landscape

                ' Freeze panes
                e.Worksheet.WorksheetOptions.AllowFreezePanes = True
                e.Worksheet.WorksheetOptions.LeftColumnRightPaneNumber = 1
                e.Worksheet.WorksheetOptions.TopRowBottomPaneNumber = 1
                e.Worksheet.WorksheetOptions.SplitHorizontalOffset = 1
                e.Worksheet.WorksheetOptions.SplitVerticalOffest = 1
                e.Worksheet.WorksheetOptions.ActivePane = 2
                isExcelExportConfigured = True
            End If
        End If
    End Sub

    Protected Sub GridExportStylesCreated(ByVal sender As Object, ByVal e As GridExportExcelMLStyleCreatedArgs)

        ' Add currency and percent styles
        Dim priceStyle = New StyleElement("priceItemStyle")
        priceStyle.NumberFormat.FormatType = NumberFormatType.Currency
        priceStyle.FontStyle.Color = System.Drawing.Color.Red
        e.Styles.Add(priceStyle)

        Dim alternatingPriceStyle = New StyleElement("alternatingPriceItemStyle")
        alternatingPriceStyle.NumberFormat.FormatType = NumberFormatType.Currency
        alternatingPriceStyle.FontStyle.Color = System.Drawing.Color.Red
        e.Styles.Add(alternatingPriceStyle)

        Dim percentStyle = New StyleElement("percentItemStyle")
        percentStyle.NumberFormat.FormatType = NumberFormatType.Percent
        percentStyle.FontStyle.Italic = True
        e.Styles.Add(percentStyle)

        Dim alternatingPercentStyle = New StyleElement("alternatingPercentItemStyle")
        alternatingPercentStyle.NumberFormat.FormatType = NumberFormatType.Percent
        alternatingPercentStyle.FontStyle.Italic = True
        e.Styles.Add(alternatingPercentStyle)

        ' Apply background colors 
        For Each style In e.Styles
            If style.Id = "headerStyle" Then
                style.InteriorStyle.Pattern = InteriorPatternType.Solid
                style.InteriorStyle.Color = System.Drawing.Color.Gray
            End If
            If style.Id = "alternatingItemStyle" OrElse style.Id = "alternatingPriceItemStyle" OrElse style.Id = "alternatingPercentItemStyle" OrElse style.Id = "alternatingDateItemStyle" Then
                style.InteriorStyle.Pattern = InteriorPatternType.Solid
                style.InteriorStyle.Color = System.Drawing.Color.LightGray
            End If
            If style.Id.Contains("itemStyle") OrElse style.Id = "priceItemStyle" OrElse style.Id = "percentItemStyle" OrElse style.Id = "dateItemStyle" Then
                style.InteriorStyle.Pattern = InteriorPatternType.Solid
                style.InteriorStyle.Color = System.Drawing.Color.White
            End If
        Next
    End Sub

    Protected Sub GridNeedDataSource(ByVal sender As Object, ByVal e As GridNeedDataSourceEventArgs)
        LoadData(False)
    End Sub

    Protected Sub ModeChanged(ByVal sender As Object, ByVal e As EventArgs)
        rfvDate.Enabled = rbtnDate.Checked
        txtDate.Enabled = rbtnDate.Checked
    End Sub

    Private Sub LoadData(Optional ByVal rebind As Boolean = True)
        If rbtnCurrSession.Checked Then
            m_TW.Load()
        Else
            m_TW.Load(False, DateTime.Parse(txtDate.Text))
        End If

        logGrid.DataSource = m_TW.Result
        If rebind Then logGrid.Rebind()

        If Not String.IsNullOrEmpty(m_TW.ErrorMessage) Then
            lblError.Text = m_TW.ErrorMessage
            lblError.Visible = True
        End If

        logGrid.Visible = logGrid.Items.Count > 0
        lblNoData.Visible = Not logGrid.Visible
    End Sub
End Class