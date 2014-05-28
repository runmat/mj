Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common
Imports System.Reflection
Imports Telerik.Web.UI
Imports Telerik.Web.UI.GridExcelBuilder

Public Class Report12
    Inherits System.Web.UI.Page

    Private m_User As User
    Private m_App As App
    Protected kopfdaten As CKG.Services.PageElements.Kopfdaten
    Private m_HB As Haendlerbestand
    Dim isExcelExportConfigured As Boolean

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = Common.GetUser(Me)
        Common.FormAuth(Me, m_User)
        m_App = New App(m_User)
        Common.GetAppIDFromQueryString(Me)
        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

        m_HB = Common.GetOrCreateObject("Haendlerbestand", Function() New Haendlerbestand(m_User, m_App, Session("AppID"), Session.SessionID))

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

        If Not IsPostBack AndAlso (bestandGrid.Visible) Then LoadData()

        kopfdaten.UserReferenz = m_User.Reference
        kopfdaten.HaendlerNummer = Session("HAENDLER_EX")
        kopfdaten.HaendlerName = Session("HAENDLER_NAME")
        kopfdaten.Adresse = Session("HAENDLER_ADDR")
    End Sub

    Protected Sub SearchClick(ByVal sender As Object, ByVal e As EventArgs)
        LoadData()
    End Sub

    Private Sub LoadData(Optional ByVal rebind As Boolean = True)
        m_HB.LoadData(Me, Session("HAENDLER_EX"))
        bestandGrid.DataSource = m_HB.Result
        If rebind Then bestandGrid.Rebind()

        bestandGrid.Visible = Not bestandGrid.DataSource Is Nothing
        cmdSearch.Visible = Not bestandGrid.Visible
    End Sub

    Protected Overrides Sub OnUnload(e As System.EventArgs)
        MyBase.OnUnload(e)

        Common.SetEndASPXAccess(Me)
    End Sub

    Protected Overrides Sub OnPreRender(e As System.EventArgs)
        MyBase.OnPreRender(e)

        Common.TranslateTelerikColumns(bestandGrid)
        Common.SetEndASPXAccess(Me)
    End Sub

    Protected Sub GridItemCommand(ByVal sender As Object, ByVal e As GridCommandEventArgs)

        Dim exportCommands = {RadGrid.ExportToCsvCommandName, RadGrid.ExportToExcelCommandName,
                                    RadGrid.ExportToPdfCommandName, RadGrid.ExportToWordCommandName}
        If exportCommands.Any(Function(c) c = e.CommandName) Then

            Dim eSettings = bestandGrid.ExportSettings

            eSettings.ExportOnlyData = True
            eSettings.FileName = String.Format("Händlerbestand_{0:yyyyMMdd}", DateTime.Now)
            eSettings.HideStructureColumns = True
            eSettings.IgnorePaging = True
            eSettings.OpenInNewWindow = True

            Select Case e.CommandName
                Case RadGrid.ExportToExcelCommandName
                    bestandGrid.MasterTableView.ExportToExcel()
                Case RadGrid.ExportToWordCommandName
                    bestandGrid.MasterTableView.ExportToWord()
                Case RadGrid.ExportToPdfCommandName
                    bestandGrid.MasterTableView.ExportToPdf()
            End Select
        End If
    End Sub

    Protected Sub GridNeedDataSource(ByVal sender As Object, ByVal e As GridNeedDataSourceEventArgs)
        LoadData(False)
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

    Protected Sub lbBack_Click(sender As Object, e As EventArgs) Handles lbBack.Click
        Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx")
    End Sub
End Class