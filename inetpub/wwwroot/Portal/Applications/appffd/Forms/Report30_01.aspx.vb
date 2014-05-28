Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business

Public Class Report30_01
    Inherits System.Web.UI.Page

#Region " Vom Web Form Designer generierter Code "

    'Dieser Aufruf ist für den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: Diese Methode ist für den Web Form-Designer erforderlich
        'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
        InitializeComponent()
    End Sub

#End Region

    Private Const m_SessionVarID As String = "FDD_Haendlerstatus"
    Private Const m_CellIdxTmpKontingent As Int32 = 1
    Private Const m_CellIdxTmpInanspruchnahme As Int32 = 2
    Private Const m_CellIdxTmpFreiesKontingent As Int32 = 3
    Private Const m_CellIdxEndgKontingent As Int32 = 4
    Private Const m_CellIdxEndgInanspruchnahme As Int32 = 5
    Private Const m_CellIdxEndgFreiesKontingent As Int32 = 6
    Private Const m_CellIdxRetailRichtwert As Int32 = 7
    Private Const m_CellIdxRetailAusschoepfung As Int32 = 8
    Private Const m_CellIdxDelayedRichtwert As Int32 = 9
    Private Const m_CellIdxDelayedAusschoepfung As Int32 = 10
    Private Const m_CellIdxHEZRichtwert As Int32 = 11
    Private Const m_CellIdxHEZAusschoepfung As Int32 = 12

    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private objSuche As Search
    Private m_DecoratedDataGrid As ASPNetDataGridDecorator = New ASPNetDataGridDecorator()
    Private m_Haendlerstatus As FDD_Haendlerstatus


    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ShowScript As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents trVorgangsArt As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trPageSize As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trDataGrid1 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents Linkbutton1 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents chkTemporaer As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkEndgueltig As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkRetail As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkDelayed As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkHEZ As System.Web.UI.WebControls.CheckBox
    Protected WithEvents lblDownloadTip As System.Web.UI.WebControls.Label
    Protected WithEvents lnkExcel As System.Web.UI.WebControls.HyperLink
    Protected WithEvents ucStyles As Styles


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'cmdSave.Enabled = False
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        m_DecoratedDataGrid.DataGridToDecorate = DataGrid1

        Dim showall As Boolean

        showall = False
        If Not (Request.QueryString("SHOWALL") Is Nothing) Then
            showall = True
        End If

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text

            m_App = New Base.Kernel.Security.App(m_User)
            'MD: Wird derzeit nicht benötigt
            'Dim DistrictCount As Integer = Session("DistrictCount")
            'If DistrictCount > 0 Then
            '    lnkKreditlimit.Text = "Distriktsuche"
            '    lnkKreditlimit.Visible = True

            'ElseIf m_User.Organization.AllOrganizations Then
            '    lnkKreditlimit.Visible = True
            'Else
            '    lnkKreditlimit.Visible = False
            'End If

            'lnkKreditlimit.Visible = False

            If (Session("objSuche") Is Nothing) Then
                Response.Redirect("Report30.aspx?AppID=" & Session("AppID").ToString)
            Else
                'Filialinformation vorhanden
                objSuche = CType(Session("objSuche"), Search)

                m_Haendlerstatus = Session(m_SessionVarID)

                If Session("SelectedDealer") Is Nothing Then
                    'Noch kein Händler ausgewählt
                    ' => Auswahltabelle
                    trVorgangsArt.Visible = False
                    trPageSize.Visible = True
                    trDataGrid1.Visible = True
                    'cmdSave.Visible = False

                    If (Not IsPostBack) Or (m_Haendlerstatus Is Nothing) Then
                        'Daten aus SAP laden

                        m_Haendlerstatus = New FDD_Haendlerstatus(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")

                        m_Haendlerstatus.ReadHandlerstatusAll(Session("AppID").ToString, Session.SessionID, Me)

                        Session(m_SessionVarID) = m_Haendlerstatus

                    End If

                    If Not IsPostBack Then
                        ddlPageSize.Items.Add("10")
                        ddlPageSize.Items.Add("20")
                        ddlPageSize.Items.Add("50")
                        ddlPageSize.Items.Add("100")
                        ddlPageSize.Items.Add("200")
                        ddlPageSize.Items.Add("500")
                        ddlPageSize.Items.Add("1000")
                        ddlPageSize.SelectedIndex = 2

                        Select Case m_Haendlerstatus.Status
                            Case 0
                                FillGrid(m_Haendlerstatus, 0)
                                Session(m_SessionVarID) = m_Haendlerstatus
                            Case -9999
                                trPageSize.Visible = False
                                trDataGrid1.Visible = False
                                lblError.Text = "Fehler bei der Ermittlung der Kontingente.<br>(" & m_Haendlerstatus.Message & ")"
                            Case Else
                                trPageSize.Visible = False
                                trDataGrid1.Visible = False
                                lblError.Text = m_Haendlerstatus.Message
                        End Select
                    End If
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub


    Private Sub FillGrid(ByVal objHaendlerstatus As FDD_Haendlerstatus, ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If objHaendlerstatus.Status = 0 Then
            If objHaendlerstatus.Kontingente.Rows.Count = 0 Then
                trDataGrid1.Visible = False
                trPageSize.Visible = False
                lblNoData.Visible = True
                lblNoData.Text = "Keine Daten zur Anzeige gefunden."
                lblDownloadTip.Visible = False
                lnkExcel.Visible = False
                ShowScript.Visible = False
            Else
                SetExcelLink()
                'checkGrid(objBank)
                trDataGrid1.Visible = True
                trPageSize.Visible = True
                lblNoData.Visible = False

                CreateMergedDataGridHeader()

                Dim tmpDataView As New DataView()
                tmpDataView = objHaendlerstatus.Kontingente.DefaultView

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

                lblNoData.Text = "Es wurden " & objHaendlerstatus.Kontingente.Rows.Count.ToString & " Einträge gefunden."
                lblNoData.Visible = True

                If DataGrid1.PageCount > 1 Then
                    DataGrid1.PagerStyle.CssClass = "PagerStyle"
                    DataGrid1.DataBind()
                    DataGrid1.PagerStyle.Visible = True
                Else
                    DataGrid1.PagerStyle.Visible = False
                End If

                Dim item As DataGridItem
                Dim cell As TableCell

                For Each item In DataGrid1.Items
                    'cell = item.Cells(4)
                    For Each cell In item.Cells
                        If IsNumeric(cell.Text) Then
                            If CInt(cell.Text) < 0 Then
                                cell.ForeColor = System.Drawing.Color.Red
                                cell.Font.Bold = True
                                cell.Font.Size = FontUnit.Point(10)
                            End If
                        End If
                    Next
                Next
            End If
        Else
            lblError.Text = objHaendlerstatus.Message
            lblNoData.Visible = True
            ShowScript.Visible = False
        End If
    End Sub

    Private Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.ItemCommand
        Dim objFDDBank As BankBaseCredit
        If Not e.Item.Cells(0).Text.Length = 0 Then
            Dim strRedirectURL As String = "Report29_23.aspx?AppID=" & Session("AppID").ToString
            Session("SelectedDealer") = e.Item.Cells(0).Text
            objFDDBank = New BankBaseCredit(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
            objFDDBank.CreditControlArea = "ZDAD"
            objFDDBank.Customer = Session("SelectedDealer").ToString
            objFDDBank.Show()
            Session("objFDDBank") = objFDDBank

            Response.Redirect(strRedirectURL)
        End If
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        FillGrid(m_Haendlerstatus, e.NewPageIndex)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(m_Haendlerstatus, DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub Datagrid1_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGrid1.ItemDataBound
        Dim countVisibleKontingentarten As Int32 = 0

        If chkTemporaer.Checked Then
            e.Item.Cells(m_CellIdxTmpKontingent).Visible = True
            e.Item.Cells(m_CellIdxTmpInanspruchnahme).Visible = True
            e.Item.Cells(m_CellIdxTmpFreiesKontingent).Visible = True
            countVisibleKontingentarten += 1
        Else
            e.Item.Cells(m_CellIdxTmpKontingent).Visible = False
            e.Item.Cells(m_CellIdxTmpInanspruchnahme).Visible = False
            e.Item.Cells(m_CellIdxTmpFreiesKontingent).Visible = False
        End If

        If chkEndgueltig.Checked Then
            e.Item.Cells(m_CellIdxEndgKontingent).Visible = True
            e.Item.Cells(m_CellIdxEndgInanspruchnahme).Visible = True
            e.Item.Cells(m_CellIdxEndgFreiesKontingent).Visible = True
            countVisibleKontingentarten += 1
        Else
            e.Item.Cells(m_CellIdxEndgKontingent).Visible = False
            e.Item.Cells(m_CellIdxEndgInanspruchnahme).Visible = False
            e.Item.Cells(m_CellIdxEndgFreiesKontingent).Visible = False
        End If

        If chkRetail.Checked Then
            e.Item.Cells(m_CellIdxRetailRichtwert).Visible = True
            e.Item.Cells(m_CellIdxRetailAusschoepfung).Visible = True
            countVisibleKontingentarten += 1
        Else
            e.Item.Cells(m_CellIdxRetailRichtwert).Visible = False
            e.Item.Cells(m_CellIdxRetailAusschoepfung).Visible = False
        End If

        If chkDelayed.Checked Then
            e.Item.Cells(m_CellIdxDelayedRichtwert).Visible = True
            e.Item.Cells(m_CellIdxDelayedAusschoepfung).Visible = True
            countVisibleKontingentarten += 1
        Else
            e.Item.Cells(m_CellIdxDelayedRichtwert).Visible = False
            e.Item.Cells(m_CellIdxDelayedAusschoepfung).Visible = False
        End If

        'Hier werden die Spalten für Händlereigene Zulassung auf jeden Fall sichtbar gemacht,
        'wenn vorher alle Spalten auf unsichtbar gesetzt wurden. Eine Spalte soll auf 
        'jeden Fall sichtbar bleiben
        If chkHEZ.Checked Then
            e.Item.Cells(m_CellIdxHEZRichtwert).Visible = True
            e.Item.Cells(m_CellIdxHEZAusschoepfung).Visible = True
        Else
            e.Item.Cells(m_CellIdxHEZRichtwert).Visible = False
            e.Item.Cells(m_CellIdxHEZAusschoepfung).Visible = False
        End If

        'Eine Kontingentart muß immer sichtbar sein
        If countVisibleKontingentarten = 0 Then
            e.Item.Cells(m_CellIdxTmpKontingent).Visible = True
            e.Item.Cells(m_CellIdxTmpInanspruchnahme).Visible = True
            e.Item.Cells(m_CellIdxTmpFreiesKontingent).Visible = True
        End If
    End Sub

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(m_Haendlerstatus, 0)
    End Sub

    Private Sub CreateMergedDataGridHeader()
        Dim cell As TableCell = Nothing
        Dim header As ArrayList = New ArrayList()
        Dim countVisibleKontingentarten As Int32 = 0

        cell = New TableCell()
        cell.Text = "Kontingentart"
        cell.RowSpan = 1
        cell.HorizontalAlign = HorizontalAlign.Center
        header.Add(cell)

        If chkTemporaer.Checked Then
            cell = New TableCell()
            cell.Text = "Temporär"
            cell.ColumnSpan = 3
            cell.HorizontalAlign = HorizontalAlign.Center
            header.Add(cell)
            countVisibleKontingentarten += 1
        End If

        If chkEndgueltig.Checked Then
            cell = New TableCell()
            cell.Text = "Endgültig"
            cell.ColumnSpan = 3
            cell.HorizontalAlign = HorizontalAlign.Center
            header.Add(cell)
            countVisibleKontingentarten += 1
        End If

        If chkRetail.Checked Then
            cell = New TableCell()
            cell.Text = "Retail"
            cell.ColumnSpan = 2
            cell.HorizontalAlign = HorizontalAlign.Center
            header.Add(cell)
            countVisibleKontingentarten += 1
        End If

        If chkDelayed.Checked Then
            cell = New TableCell()
            cell.Text = "Delayed payment"
            cell.ColumnSpan = 2
            cell.HorizontalAlign = HorizontalAlign.Center
            header.Add(cell)
            countVisibleKontingentarten += 1
        End If

        If chkHEZ.Checked Then
            cell = New TableCell()
            cell.Text = "Händlereigene<br>Zulassung"
            cell.ColumnSpan = 2
            cell.HorizontalAlign = HorizontalAlign.Center
            header.Add(cell)
            countVisibleKontingentarten += 1
        End If

        'Eine Kontingentart soll immer sichtbar sein
        If countVisibleKontingentarten = 0 Then
            cell = New TableCell()
            cell.Text = "Temporär"
            cell.ColumnSpan = 3
            cell.HorizontalAlign = HorizontalAlign.Center
            header.Add(cell)
            chkTemporaer.Checked = True
        End If

        m_DecoratedDataGrid.AddMergeHeader(header)
    End Sub

    Private Sub Linkbutton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Linkbutton1.Click
        FillGrid(m_Haendlerstatus, DataGrid1.CurrentPageIndex)
    End Sub

    Private Sub SetExcelLink()
        Dim strfilename As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
        Dim reportExcel As DataTable

        Try
            reportExcel = m_Haendlerstatus.Kontingente.Copy()

            reportExcel.Columns(1).ColumnName = "Temp. Kontingent"
            reportExcel.Columns(2).ColumnName = "Temp. Inanspruchnahme"
            reportExcel.Columns(3).ColumnName = "Temp. Freies Kontingent"

            reportExcel.Columns(4).ColumnName = "Endg. Kontingent"
            reportExcel.Columns(5).ColumnName = "Endg. Inanspruchnahme"
            reportExcel.Columns(6).ColumnName = "Endg. Freies Kontingent"

            reportExcel.Columns(7).ColumnName = "Retail Richtwert"
            reportExcel.Columns(8).ColumnName = "Retail Ausschöpfung"

            reportExcel.Columns(9).ColumnName = "Delayed payment Richtwert"
            reportExcel.Columns(10).ColumnName = "Delayed payment Ausschöpfung"

            reportExcel.Columns(11).ColumnName = "HEZ Richtwert"
            reportExcel.Columns(12).ColumnName = "HEZ Ausschöpfung"

            'Es wurde der Einfachheit halber die komplette Tabelle kopiert.
            'Anschließend erfolgt die Aufbereitung der Tabelle gemäß der aktivierten checkBoxen.
            'Wenn eine chkBox deaktiviert ist, werden die entsprechenden Spalten aus der kopierten
            'Tabelle entfernt

            If Not chkTemporaer.Checked Then
                'Diese Kontingentart auf jeden Fall aktivieren , wenn keine andere aktiviert ist
                If chkEndgueltig.Checked Or chkRetail.Checked Or chkDelayed.Checked Or chkHEZ.Checked Then
                    reportExcel.Columns.Remove("Temp. Kontingent")
                    reportExcel.Columns.Remove("Temp. Inanspruchnahme")
                    reportExcel.Columns.Remove("Temp. Freies Kontingent")
                End If
            End If
            If Not chkEndgueltig.Checked Then
                reportExcel.Columns.Remove("Endg. Kontingent")
                reportExcel.Columns.Remove("Endg. Inanspruchnahme")
                reportExcel.Columns.Remove("Endg. Freies Kontingent")
            End If
            If Not chkRetail.Checked Then
                reportExcel.Columns.Remove("Retail Richtwert")
                reportExcel.Columns.Remove("Retail Ausschöpfung")
            End If
            If Not chkDelayed.Checked Then
                reportExcel.Columns.Remove("Delayed Richtwert")
                reportExcel.Columns.Remove("Delayed Ausschöpfung")
            End If
            If Not chkHEZ.Checked Then
                reportExcel.Columns.Remove("HEZ Richtwert")
                reportExcel.Columns.Remove("HEZ Ausschöpfung")
            End If

            reportExcel.AcceptChanges()

            Excel.ExcelExport.WriteExcel(reportExcel, ConfigurationManager.AppSettings("ExcelPath") & strfilename)

            Session("lnkExcel") = "/Portal/Temp/Excel/" & strfilename

        Catch
        End Try

        If Not Session("lnkExcel").ToString.Length = 0 Then
            lblDownloadTip.Visible = True
            lnkExcel.Visible = True
            lnkExcel.NavigateUrl = Session("lnkExcel").ToString
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class
' ************************************************
' $History: Report30_01.aspx.vb $
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 23.03.10   Time: 16:34
' Updated in $/CKAG/Applications/appffd/Forms
' Rückgängig: Dynproxy-Zugriff
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 10.03.10   Time: 15:22
' Updated in $/CKAG/Applications/appffd/Forms
' ITA: 2918
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 15.06.09   Time: 17:08
' Updated in $/CKAG/Applications/appffd/Forms
' ITA 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 30.04.09   Time: 14:43
' Updated in $/CKAG/Applications/appffd/Forms
' ITA: 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:23
' Created in $/CKAG/Applications/appffd/Forms
' 
' *****************  Version 5  *****************
' User: Uha          Date: 2.07.07    Time: 12:52
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 19.06.07   Time: 16:10
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' History-Eintrag eingepflegt.
' 
' ************************************************


