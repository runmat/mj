Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports Telerik.Web.UI

Imports System.Linq
Imports System.Web.UI.WebControls

Imports CKG.Base.Kernel.Security
Imports System.Data
Imports System.Drawing

Imports Telerik.Web.UI.GridExcelBuilder


Public Class Report06
    Inherits System.Web.UI.Page

#Region "Declarations"
    Private m_App As Security.App
    Private m_User As Security.User
    Private m_Track As Sendungsverfolgung
    Private sapSource As MopedKennzeichen
    Private changedRows As Dictionary(Of Integer, String)
    Private isExcelExportConfigured As Boolean = False

#End Region

#Region "PageEvents"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim lastVisibleCol As Integer = -1
        Dim version As String = Request.Browser.Version.Replace(".", ",")
        Dim ver As Double = 0.0

        m_User = GetUser(Me)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        m_App = New Security.App(m_User)

        Dim appURL As String = Me.Request.Url.LocalPath.Replace("/Services", "..")
        Dim table As DataTable = CType(Context.Session(appURL), DataTable)

        sapSource = New MopedKennzeichen(m_User, m_App, Session("AppID"), Session.SessionID.ToString, "")

        If Not IsPostBack Then

            'prüfen ob ein Internetexplorer < Version 7 verwendet wird
            If Double.TryParse(version, ver) And Request.Browser.Browser.ToUpper().Equals("IE") Then

                If (ver < 8.0) Then

                    lblError.Text = "Achtung!</br>Für eine optimale Darstellung wird ein Internet Explorer ab der Version 8.0 empfohlen."
                    lblError.Text += "</br>Der Kompatibilitätsmodus sollte ebenfalls ausgeschaltet sein."
                    lblError.Visible = True

                End If

            End If

        End If



        If fzgGrid.Visible Then
            'LoadData()
        End If

        TranslateTelerikColumns(fzgGrid)

    End Sub

    Private Sub Page_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender

        SetEndASPXAccess(Me)
        CKG.Base.Kernel.Common.Common.SetEndASPXAccess(Me)

    End Sub

    Private Sub Page_Unload(sender As Object, e As System.EventArgs) Handles Me.Unload

        SetEndASPXAccess(Me)

    End Sub

#End Region

#Region "Methods"

    ''' <summary>
    ''' Daten an das Gridview binden
    ''' </summary>
    ''' <param name="rebind"></param>
    ''' 
    Private Sub LoadData(Optional ByVal rebind As Boolean = True)

        Try

            Dim tmpInt = 0
            Dim sVerJahr = txt_VersJahr.Text.Trim

            If String.IsNullOrEmpty(sVerJahr) Then

                lblError.Text = "Versicherungsjahr darf nich leer sein"
                lblError.Visible = True
                Return
            End If

            If Not Integer.TryParse(sVerJahr, tmpInt) Or Not sVerJahr.Length = 4 Then

                lblError.Text = "Versicherungsjahr - Falsches Format"
                lblError.Visible = True
                Return


            End If

            Dim Orgnummer As String = ""
            Dim sNummerTrenn As String = ""

            For i As Integer = 1 To txt_Vermittler.Text.Length
                If Not (Mid(txt_Vermittler.Text, i, 1)) = "-" AndAlso Not (Mid(txt_Vermittler.Text, i, 1)) = "_" Then
                    sNummerTrenn = txt_Vermittler.Text
                    Orgnummer &= Mid(txt_Vermittler.Text, i, 1)
                End If
            Next

            sapSource.Vermittler = Orgnummer

            sapSource.VersJahr = sVerJahr
            sapSource.SerialNumber = txt_SerialNr.Text.Trim
            'sapSource.Vermittler = txt_Vermittler.Text.Trim
            sapSource.Vermittler = Orgnummer
            sapSource.IsRueckLaeufer = "X"
            sapSource.IsAgentur = ""
            lblError.Visible = False

            If rebind Then
                sapSource.GetData(Session("AppID"), Session.SessionID.ToString, Me.Page)

                If sapSource.Result Is Nothing Then

                    lblError.Visible = True
                    pnlSelection.Visible = True
                    lblError.Text = sapSource.Message
                    fzgGrid.Visible = False
                    Return

                End If

                Session("BINDTABLE") = sapSource.Result

            End If

            Select Case sapSource.Status

                Case -1111 ' NO DATA
                    lblNoData.Visible = True
                    pnlSelection.Visible = True
                    lblNoData.Text = sapSource.Message
                    fzgGrid.Visible = False
                Case -9999 ' other error
                    lblError.Visible = True
                    lblError.Text = sapSource.Message
                    fzgGrid.Visible = False
                Case Else
                    fzgGrid.DataSource = CType(Session("BINDTABLE"), DataTable)
                    If (rebind) Then fzgGrid.Rebind()
                    'Spaltenbreiten anhand der Tabelleninhale ermitteln und setzen
                    'ResizeTelerikColumns(fzgGrid, sapSource.ReturnTable, 300)

            End Select

            NewSearch.ImageUrl = "../../../Images/queryArrowUp.gif"
            pnlSelection.Visible = False
            fzgGrid.Visible = True
            lb_Weiter.Visible = False

            Result.Visible = True


        Catch ex As Exception

            lb_Weiter.Visible = True
            fzgGrid.Visible = False
            Result.Visible = False
            lblError.Text = ex.Message
            lblError.Visible = True
            lblNoData.Visible = False
        End Try
    End Sub


    Private Sub DoSapInsert()

        Dim index As Integer

        changedRows = Session("CHGROWS")

        If changedRows Is Nothing Then
            Return
        End If
        Dim valuesToSap(changedRows.Count - 1) As String
        Dim dt As DataTable = Session("BINDTABLE")
        Dim tmpDic(changedRows.Count) As String
        Dim str As String = String.Empty
        For i = 0 To changedRows.Count - 1


            index = changedRows.Keys(i)


            str = dt.Rows(index).Item("AG") + "|"
            str += dt.Rows(index).Item("KUNDE") + "|"
            str += dt.Rows(index).Item("VERS_JAHR") + "|"
            str += dt.Rows(index).Item("MATNR") + "|"
            str += dt.Rows(index).Item("SERNR") + "|"
            str += dt.Rows(index).Item("EQUNR") + "|"
            str += dt.Rows(index).Item("BEMERKUNG") + "|"
            str += ""

            valuesToSap(i) = str

            'sapSource.ImportRow = valuesToSap
        Next

        sapSource.SetData(Session("AppID"), Session.SessionID.ToString, Me.Page, valuesToSap)
        Session("CHGROWS") = Nothing



    End Sub

#End Region

#Region "ControlEvents"

    Protected Sub NewSearch_Click(sender As Object, e As ImageClickEventArgs) Handles NewSearch.Click

        If pnlSelection.Visible Then

            pnlSelection.Visible = False
            fzgGrid.Visible = True
            Result.Visible = True
            lb_Weiter.Visible = False
            NewSearch.ImageUrl = "../../../Images/queryArrowUp.gif"

        Else

            pnlSelection.Visible = True
            lb_Weiter.Visible = True
            fzgGrid.Visible = False
            Result.Visible = False
            NewSearch.ImageUrl = "../../../Images/queryArrow.gif"
        End If

    End Sub

    Protected Sub lbBack_Click(sender As Object, e As EventArgs) Handles lbBack.Click
        Response.Redirect("../../../Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub

    Protected Sub lb_Weiter_Click(sender As Object, e As EventArgs) Handles lb_Weiter.Click

        LoadData()

    End Sub

#End Region

#Region "GridEvents"

    Protected Sub FzgGridItemCommand(sender As Object, e As GridCommandEventArgs) Handles fzgGrid.ItemCommand

        Dim exportCommands = New String() {RadGrid.ExportToCsvCommandName, RadGrid.ExportToExcelCommandName,
                                        RadGrid.ExportToPdfCommandName, RadGrid.ExportToWordCommandName}

        'if (!exportCommands.Any(c => c == e.CommandName))
        '            Return

        Dim eSettings As GridExportSettings = fzgGrid.ExportSettings

        eSettings.ExportOnlyData = True
        eSettings.FileName = String.Format("Lizenzfahrzeuge_{0:yyyyMMdd}", DateTime.Now)
        eSettings.HideStructureColumns = False
        eSettings.IgnorePaging = True
        eSettings.OpenInNewWindow = True

        Select Case (e.CommandName)

            Case RadGrid.ExportToExcelCommandName
                fzgGrid.MasterTableView.ExportToExcel()

            Case RadGrid.ExportToWordCommandName
                fzgGrid.MasterTableView.ExportToWord()

            Case RadGrid.ExportToPdfCommandName
                fzgGrid.MasterTableView.ExportToPdf()

        End Select
    End Sub

    Protected Sub FzgGridNeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs)

        'var mi =  MethodInfo.GetCurrentMethod();
        'System.Diagnostics.Trace.WriteLine(mi.DeclaringType.Name + "." + mi.Name);
        LoadData(False)
    End Sub

    Protected Sub FzgGridPageChanged(sender As Object, e As GridPageChangedEventArgs)

        'var mi = MethodInfo.GetCurrentMethod();
        'System.Diagnostics.Trace.WriteLine(mi.DeclaringType.Name + "." + mi.Name); 
        LoadData(False)
    End Sub

    Protected Sub FzgGridPageSizeChanged(sender As Object, e As GridPageSizeChangedEventArgs)

        'var mi = MethodInfo.GetCurrentMethod();
        'System.Diagnostics.Trace.WriteLine(mi.DeclaringType.Name + "." + mi.Name);
        LoadData(False)
    End Sub

    Protected Sub FzgGridSortCommand(sender As Object, e As GridSortCommandEventArgs)

        'var mi = MethodInfo.GetCurrentMethod();
        'System.Diagnostics.Trace.WriteLine(mi.DeclaringType.Name + "." + mi.Name);
        LoadData(False)
    End Sub

    Protected Sub fzgGrid_ExcelMLExportRowCreated(sender As Object, e As Telerik.Web.UI.GridExcelBuilder.GridExportExcelMLRowCreatedArgs)

        If e.RowType = GridExportExcelMLRowType.DataRow Then
            'Add custom styles to the desired cells
            'CellElement cell = e.Row.Cells.GetCellByName("UnitPrice");
            'cell.StyleValue = cell.StyleValue == "itemStyle" ? "priceItemStyle" : "alternatingPriceItemStyle";

            'cell = e.Row.Cells.GetCellByName("ExtendedPrice");
            'cell.StyleValue = cell.StyleValue == "itemStyle" ? "priceItemStyle" : "alternatingPriceItemStyle";

            'cell = e.Row.Cells.GetCellByName("Discount");
            'cell.StyleValue = cell.StyleValue == "itemStyle" ? "percentItemStyle" : "alternatingPercentItemStyle";

            If Not isExcelExportConfigured Then

                'Set Worksheet name
                e.Worksheet.Name = "Zurückgesendete Kennzeichen"

                'Set Column widths
                For Each column As ColumnElement In e.Worksheet.Table.Columns

                    If (e.Worksheet.Table.Columns.IndexOf(column) = 2) Then
                        column.Width = Unit.Point(180) 'set width 180 to ProductName column
                    Else
                        column.Width = Unit.Point(80) 'set width 80 to the rest of the columns
                    End If
                Next

                'Set Page options
                Dim pageSetup As PageSetupElement = e.Worksheet.WorksheetOptions.PageSetup
                pageSetup.PageLayoutElement.IsCenteredVertical = True
                pageSetup.PageLayoutElement.IsCenteredHorizontal = True
                pageSetup.PageMarginsElement.Left = 0.5
                pageSetup.PageMarginsElement.Top = 0.5
                pageSetup.PageMarginsElement.Right = 0.5
                pageSetup.PageMarginsElement.Bottom = 0.5
                pageSetup.PageLayoutElement.PageOrientation = PageOrientationType.Landscape

                'Freeze panes
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

    Protected Sub fzgGrid_ExcelMLExportStylesCreated(sender As Object, e As Telerik.Web.UI.GridExcelBuilder.GridExportExcelMLStyleCreatedArgs)

        'Add currency and percent styles

        Dim priceStyle As StyleElement = New StyleElement("priceItemStyle")
        priceStyle.NumberFormat.FormatType = NumberFormatType.Currency
        priceStyle.FontStyle.Color = System.Drawing.Color.Red
        e.Styles.Add(priceStyle)

        Dim alternatingPriceStyle As StyleElement = New StyleElement("alternatingPriceItemStyle")
        alternatingPriceStyle.NumberFormat.FormatType = NumberFormatType.Currency
        alternatingPriceStyle.FontStyle.Color = System.Drawing.Color.Red
        e.Styles.Add(alternatingPriceStyle)

        Dim percentStyle As StyleElement = New StyleElement("percentItemStyle")
        percentStyle.NumberFormat.FormatType = NumberFormatType.Percent
        percentStyle.FontStyle.Italic = True
        e.Styles.Add(percentStyle)

        Dim alternatingPercentStyle As StyleElement = New StyleElement("alternatingPercentItemStyle")
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

    Protected Sub fzgGrid_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles fzgGrid.ItemCreated
        If TypeOf e.Item Is GridDataItem AndAlso e.Item.IsInEditMode Then
            'TryCast(TryCast(e.Item, GridDataItem)("UnitPrice").Controls(0), RadNumericTextBox).Width = Unit.Pixel(50)
        End If
    End Sub

    Protected Sub fzgGrid_ItemUpdated(ByVal source As Object, ByVal e As GridUpdatedEventArgs) Handles fzgGrid.ItemUpdated

        Dim item As GridEditableItem = DirectCast(e.Item, GridEditableItem)
        Dim id As String = item.GetDataKeyValue("ProductID").ToString()

        If Not e.Exception Is Nothing Then
            e.KeepInEditMode = True
            e.ExceptionHandled = True

        Else

        End If

    End Sub

    Protected Sub fzgGrid_UpdateCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles fzgGrid.UpdateCommand

        Dim dataItem As GridDataItem = e.Item
        Dim crtl As TextBox = dataItem.Item("BEMERKUNG").Controls(0)
        Dim dt As DataTable = Session("BINDTABLE")

        Dim ps = fzgGrid.PageSize
        Dim currentPage = fzgGrid.CurrentPageIndex
        Dim arg As Integer '= e.Item.RowIndex - 2

        'Paging beachten
        arg = (ps * currentPage) + e.Item.RowIndex - 2
        dt.Rows(arg).Item("BEMERKUNG") = crtl.Text
        Session("BINDTABLE") = dt

        'direct in SAP schreiben
        Dim valuesToSap(0) As String
        Dim index As Integer = 0
        Dim str As String = String.Empty
        str = dt.Rows(index).Item("AG") + "|"
        str += dt.Rows(index).Item("KUNDE") + "|"
        str += dt.Rows(index).Item("VERS_JAHR") + "|"
        str += dt.Rows(index).Item("MATNR") + "|"
        str += dt.Rows(index).Item("SERNR") + "|"
        str += dt.Rows(index).Item("EQUNR") + "|"
        str += dt.Rows(index).Item("BEMERKUNG") + "|"
        str += ""

        valuesToSap(0) = str

        sapSource.SetData(Session("AppID"), Session.SessionID.ToString, Me.Page, valuesToSap)

        e.Item.Edit = False

        e.Item.Style.Item("Background-Image") = "none"
        e.Item.Selected = False

        ' Session("CHGROWS") = Nothing

        'Return


        'changedRows = Session("CHGROWS")


        'If changedRows Is Nothing Then
        '    changedRows = New Dictionary(Of Integer, String)
        'End If

        'If changedRows.ContainsKey(arg) Then
        '    changedRows(arg) = crtl.Text
        'Else
        '    changedRows.Add(arg, crtl.Text)
        'End If

        'Session("CHGROWS") = changedRows
        '



        'DoSapInsert()


    End Sub

    Protected Sub fzgGrid_SelectedIndexChanged(sender As Object, e As EventArgs) Handles fzgGrid.SelectedIndexChanged

    End Sub

    Protected Sub fzgGrid_EditCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles fzgGrid.EditCommand

        If e.CommandName.ToLower = "edit" Then
            Return
        End If

        If e.Canceled = True Then
            Return
        Else
            LoadData(False)
        End If


    End Sub

    Protected Sub fzgGrid_InsertCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles fzgGrid.InsertCommand
    End Sub

    Protected Sub fzgGrid_ItemEvent(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles fzgGrid.ItemEvent
    End Sub

#End Region

End Class