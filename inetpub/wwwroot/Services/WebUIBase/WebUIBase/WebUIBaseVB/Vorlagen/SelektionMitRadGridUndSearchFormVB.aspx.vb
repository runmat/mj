Option Strict On
Option Explicit On

Imports CKG.Base.Kernel.Common
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Business
Imports Telerik.Web.UI
Imports Telerik.Web.UI.GridExcelBuilder

Namespace Vorlagen

    ''' <summary>
    ''' Vorlage für Reports mit Telerik RadGrid
    ''' </summary>
    ''' <remarks>TODO-Kommentare markieren Stellen, an denen Report-individuelle Anpassungen vorgenommen werden müssen</remarks>
    Partial Public Class SelektionMitRadGridUndSearchFormVB
        Inherits System.Web.UI.Page

        Private m_User As CKG.Base.Kernel.Security.User
        Private m_App As CKG.Base.Kernel.Security.App
        Private isExcelExportConfigured As Boolean
        'TODO: ReportBase durch gewünschten Datentyp ersetzen
        Private m_Report As ReportBase

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            m_User = Common.GetUser(Me)
            Common.FormAuth(Me, m_User)

            m_App = New App(m_User)
            Common.GetAppIDFromQueryString(Me)

            lblHead.Text = CStr(m_User.Applications.Select("AppID = '" & Session("AppID").ToString() & "'")(0)("AppFriendlyName"))
            lblError.Text = ""

            Try

                'TODO: "Datenobjektname" durch geeigneten Objektnamen ersetzen
                If Session("Datenobjektname") Is Nothing Then
                    'TODO: m_Report mit neuer Objektinstanz belegen
                Else
                    m_Report = CType(Session("Datenobjektname"), ReportBase)
                    m_Report.SessionID = Session.SessionID
                    m_Report.AppID = CStr(Session("AppID"))
                    Session("Datenobjektname") = m_Report
                End If

                If Not IsPostBack Then
                    Common.TranslateTelerikColumns(rgGrid1)

                    'TODO: Seiteninitialisierung, z.B. Füllen von Dropdowns

                    Dim persister As New GridSettingsPersister(rgGrid1, GridSettingsType.All)
                    Session("rgGrid1_original") = persister.LoadForUser(m_User, Session("AppID").ToString(), GridSettingsType.All.ToString())
                End If

            Catch ex As Exception
                lblError.Text = "Keine Dokumente zur Anzeige gefunden."
            End Try
        End Sub

        Private Sub Page_PreRender(sender As Object, e As EventArgs)
            Common.SetEndASPXAccess(Me)
        End Sub

        Private Sub Page_Unload(sender As Object, e As EventArgs)
            Common.SetEndASPXAccess(Me)
        End Sub

        Protected Sub lbCreate_Click(sender As Object, e As EventArgs)
            DoSubmit()
        End Sub

        Private Sub DoSubmit()

            'TODO: Eingabefelder prüfen

            'TODO: eingegebene Parameter in m_Report-Objekt übernehmen

            'TODO: (SAP-)Abruffunktion von m_Report ausführen

            If m_Report.Status = 0 Then
                Session("Datenobjektname") = m_Report
                Fillgrid()
            Else
                lblError.Text = m_Report.Message
            End If

        End Sub

        Private Sub Fillgrid()

            If m_Report.Result.Rows.Count = 0 Then
                SearchMode()
                lblError.Text = "Keine Dokumente zur Anzeige gefunden."
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
            If m_Report IsNot Nothing Then
                rgGrid1.DataSource = m_Report.Result.DefaultView
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
            Session("Datenobjektname") = Nothing
            Response.Redirect("/Services/(S(" & Session.SessionID & "))/Start/Selection.aspx")
        End Sub

        Private Sub StoreGridSettings(grid As RadGrid, settingsType As GridSettingsType)
            Dim persister As New GridSettingsPersister(grid, settingsType)
            persister.SaveForUser(m_User, Session("AppID").ToString(), settingsType.ToString())
        End Sub

        Protected Sub rgGrid1_ItemCreated(sender As Object, e As GridItemEventArgs)
            If e.Item.ItemType = GridItemType.CommandItem Then
                Dim gcItem As GridCommandItem = CType(e.Item, GridCommandItem)

                Dim rButton As Control = gcItem.FindControl("RefreshButton")
                If rButton Is Nothing Then
                    rButton = gcItem.FindControl("RebindGridButton")
                End If
                If rButton Is Nothing Then Exit Sub

                Dim rButton_parent As Control = rButton.Parent

                Dim saveLayoutButton As New Button()
                With saveLayoutButton
                    .ToolTip = "Layout speichern"
                    .CommandName = "SaveGridLayout"
                    .CssClass = "rgSaveLayout"
                End With
                rButton_parent.Controls.AddAt(0, saveLayoutButton)

                Dim resetLayoutButton As New Button()
                With resetLayoutButton
                    .ToolTip = "Layout zurücksetzen"
                    .CommandName = "ResetGridLayout"
                    .CssClass = "rgResetLayout"
                End With
                rButton_parent.Controls.AddAt(1, resetLayoutButton)

            End If
        End Sub

        Protected Sub rgGrid1_ItemCommand(sender As Object, e As GridCommandEventArgs)
            Select Case e.CommandName

                Case RadGrid.ExportToExcelCommandName
                    Dim eSettings As GridExportSettings = rgGrid1.ExportSettings
                    eSettings.ExportOnlyData = True
                    'TODO: Dateiname anpassen
                    eSettings.FileName = String.Format("Fahrzeugbestand_{0:yyyyMMdd}", DateTime.Now)
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

                Case "SaveGridLayout"
                    StoreGridSettings(rgGrid1, GridSettingsType.All)

                Case "ResetGridLayout"
                    Dim settings As String = CStr(Session("rgGrid1_original"))
                    Dim persister As New GridSettingsPersister(rgGrid1, GridSettingsType.All)
                    persister.LoadSettings(settings)
                    Fillgrid()

            End Select
        End Sub

        Protected Sub rgGrid1_ExcelMLExportRowCreated(sender As Object, e As GridExportExcelMLRowCreatedArgs)

            If e.RowType = GridExportExcelMLRowType.DataRow Then

                If Not isExcelExportConfigured Then
                    'TODO: Arbeitsblattname anpassen
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

End Namespace

