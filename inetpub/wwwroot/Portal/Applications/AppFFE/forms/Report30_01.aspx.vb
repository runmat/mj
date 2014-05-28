Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business

Partial Public Class Report30_01
    Inherits System.Web.UI.Page
    Private Const m_SessionVarID As String = "FDD_Haendlerstatus"
    Private Const m_CellIdxTmpKontingent As Int32 = 1
    Private Const m_CellIdxTmpInanspruchnahme As Int32 = 2
    Private Const m_CellIdxTmpFreiesKontingent As Int32 = 3
    Private Const m_CellIdxTmpFrist As Int32 = 4
    Private Const m_CellIdxTmpSperre As Int32 = 5
    Private Const m_CellIdxEndgKontingent As Int32 = 6
    Private Const m_CellIdxEndgInanspruchnahme As Int32 = 7
    Private Const m_CellIdxEndgFreiesKontingent As Int32 = 8
    Private Const m_CellIdxEndgFrist As Int32 = 9
    Private Const m_CellIdxEndgSperre As Int32 = 10
    Private Const m_CellIdxRetailRichtwert As Int32 = 11
    Private Const m_CellIdxRetailAusschoepfung As Int32 = 12
    Private Const m_CellIdxRetailFrist As Int32 = 13
    Private Const m_CellIdxRetailSperre As Int32 = 14
    Private Const m_CellIdxDelayedRichtwert As Int32 = 15
    Private Const m_CellIdxDelayedAusschoepfung As Int32 = 16
    Private Const m_CellIdxDelayedFrist As Int32 = 17
    Private Const m_CellIdxDelayedSperre As Int32 = 18
    Private Const m_CellIdxHEZRichtwert As Int32 = 19
    Private Const m_CellIdxHEZAusschoepfung As Int32 = 20
    Private Const m_CellIdxHEZFrist As Int32 = 21
    Private Const m_CellIdxHEZSperre As Int32 = 22
    Private Const m_CellIdxKFKLRichtwert As Int32 = 23
    Private Const m_CellIdxKFKLAusschoepfung As Int32 = 24
    Private Const m_CellIdxKFKLFrist As Int32 = 25
    Private Const m_CellIdxKFKLSperre As Int32 = 26

    Private chkTemporaer As CheckBox
    Private chkEndgueltig As CheckBox
    Private chkRetail As CheckBox
    Private chkDelayed As CheckBox
    Private chkHEZ As CheckBox
    Private chkKFKL As CheckBox

    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private objSuche As FFE_Search
    Private m_DecoratedDataGrid As ASPNetDataGridDecorator = New ASPNetDataGridDecorator()
    Private m_Haendlerstatus As FFE_Haendlerstatus

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

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
                objSuche = CType(Session("objSuche"), FFE_Search)

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

                        m_Haendlerstatus = New FFE_Haendlerstatus(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")

                        m_Haendlerstatus.ReadHandlerstatusAll()

                        Session(m_SessionVarID) = m_Haendlerstatus

                    End If



                    If IsPostBack = True Then
                        fillCHKVariablen()
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

                        fillGridView()
                        fillCHKVariablen()
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


    Private Sub fillGridView()
        'dient nur dazu die gv anzeigen zu lassen mit einer Zeile
        Dim tmpAL As New ArrayList
        tmpAL.Add(New Object())
        gvKontigentarten.DataSource = tmpAL
        gvKontigentarten.DataBind()
    End Sub

    Private Sub FillGrid(ByVal objHaendlerstatus As FFE_Haendlerstatus, ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
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
        Dim objFDDBank As FFE_BankBase
        If Not e.Item.Cells(0).Text.Length = 0 Then
            Dim strRedirectURL As String = "Report29_23.aspx?AppID=" & Session("AppID").ToString
            Session("SelectedDealer") = e.Item.Cells(0).Text
            objFDDBank = New FFE_BankBase(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
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

    Private Sub fillCHKVariablen()
        chkTemporaer = CType(gvKontigentarten.Rows(0).FindControl("chkTemporaer"), CheckBox)
        chkEndgueltig = CType(gvKontigentarten.Rows(0).FindControl("chkEndgueltig"), CheckBox)
        chkRetail = CType(gvKontigentarten.Rows(0).FindControl("chkRetail"), CheckBox)
        chkDelayed = CType(gvKontigentarten.Rows(0).FindControl("chkDelayed"), CheckBox)
        chkKFKL = CType(gvKontigentarten.Rows(0).FindControl("chkKFKL"), CheckBox)
        chkHEZ = CType(gvKontigentarten.Rows(0).FindControl("chkHEZ"), CheckBox)
    End Sub

    Private Sub Datagrid1_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGrid1.ItemDataBound
        Dim countVisibleKontingentarten As Int32 = 0


        If chkTemporaer.Checked Then
            e.Item.Cells(m_CellIdxTmpKontingent).Visible = True
            e.Item.Cells(m_CellIdxTmpInanspruchnahme).Visible = True
            e.Item.Cells(m_CellIdxTmpFreiesKontingent).Visible = True
            e.Item.Cells(m_CellIdxTmpFrist).Visible = True
            e.Item.Cells(m_CellIdxTmpSperre).Visible = True
            countVisibleKontingentarten += 1
        Else
            e.Item.Cells(m_CellIdxTmpKontingent).Visible = False
            e.Item.Cells(m_CellIdxTmpInanspruchnahme).Visible = False
            e.Item.Cells(m_CellIdxTmpFreiesKontingent).Visible = False
            e.Item.Cells(m_CellIdxTmpFrist).Visible = False
            e.Item.Cells(m_CellIdxTmpSperre).Visible = False
        End If

        If chkEndgueltig.Checked Then
            e.Item.Cells(m_CellIdxEndgKontingent).Visible = True
            e.Item.Cells(m_CellIdxEndgInanspruchnahme).Visible = True
            e.Item.Cells(m_CellIdxEndgFreiesKontingent).Visible = True
            e.Item.Cells(m_CellIdxEndgFrist).Visible = True
            e.Item.Cells(m_CellIdxEndgSperre).Visible = True
            countVisibleKontingentarten += 1
        Else
            e.Item.Cells(m_CellIdxEndgKontingent).Visible = False
            e.Item.Cells(m_CellIdxEndgInanspruchnahme).Visible = False
            e.Item.Cells(m_CellIdxEndgFreiesKontingent).Visible = False
            e.Item.Cells(m_CellIdxEndgFrist).Visible = False
            e.Item.Cells(m_CellIdxEndgSperre).Visible = False
        End If

        If chkRetail.Checked Then
            e.Item.Cells(m_CellIdxRetailRichtwert).Visible = True
            e.Item.Cells(m_CellIdxRetailAusschoepfung).Visible = True
            e.Item.Cells(m_CellIdxRetailFrist).Visible = True
            e.Item.Cells(m_CellIdxRetailSperre).Visible = True
            countVisibleKontingentarten += 1
        Else
            e.Item.Cells(m_CellIdxRetailRichtwert).Visible = False
            e.Item.Cells(m_CellIdxRetailAusschoepfung).Visible = False
            e.Item.Cells(m_CellIdxRetailFrist).Visible = False
            e.Item.Cells(m_CellIdxRetailSperre).Visible = False
        End If

        If chkDelayed.Checked Then
            e.Item.Cells(m_CellIdxDelayedRichtwert).Visible = True
            e.Item.Cells(m_CellIdxDelayedAusschoepfung).Visible = True
            e.Item.Cells(m_CellIdxDelayedFrist).Visible = True
            e.Item.Cells(m_CellIdxDelayedSperre).Visible = True
            countVisibleKontingentarten += 1
        Else
            e.Item.Cells(m_CellIdxDelayedRichtwert).Visible = False
            e.Item.Cells(m_CellIdxDelayedAusschoepfung).Visible = False
            e.Item.Cells(m_CellIdxDelayedFrist).Visible = False
            e.Item.Cells(m_CellIdxDelayedSperre).Visible = False
        End If

        'Hier werden die Spalten für Händlereigene Zulassung auf jeden Fall sichtbar gemacht,
        'wenn vorher alle Spalten auf unsichtbar gesetzt wurden. Eine Spalte soll auf 
        'jeden Fall sichtbar bleiben
        If chkHEZ.Checked Then
            e.Item.Cells(m_CellIdxHEZRichtwert).Visible = True
            e.Item.Cells(m_CellIdxHEZAusschoepfung).Visible = True
            e.Item.Cells(m_CellIdxHEZFrist).Visible = True
            e.Item.Cells(m_CellIdxHEZSperre).Visible = True
            countVisibleKontingentarten += 1
        Else
            e.Item.Cells(m_CellIdxHEZRichtwert).Visible = False
            e.Item.Cells(m_CellIdxHEZAusschoepfung).Visible = False
            e.Item.Cells(m_CellIdxHEZFrist).Visible = False
            e.Item.Cells(m_CellIdxHEZSperre).Visible = False
        End If

        If chkKFKL.Checked Then
            e.Item.Cells(m_CellIdxKFKLRichtwert).Visible = True
            e.Item.Cells(m_CellIdxKFKLAusschoepfung).Visible = True
            e.Item.Cells(m_CellIdxKFKLFrist).Visible = True
            e.Item.Cells(m_CellIdxKFKLSperre).Visible = True
            countVisibleKontingentarten += 1
        Else
            e.Item.Cells(m_CellIdxKFKLRichtwert).Visible = False
            e.Item.Cells(m_CellIdxKFKLAusschoepfung).Visible = False
            e.Item.Cells(m_CellIdxKFKLFrist).Visible = False
            e.Item.Cells(m_CellIdxKFKLSperre).Visible = False
        End If

        'Eine Kontingentart muß immer sichtbar sein
        If countVisibleKontingentarten = 0 Then
            e.Item.Cells(m_CellIdxTmpKontingent).Visible = True
            e.Item.Cells(m_CellIdxTmpInanspruchnahme).Visible = True
            e.Item.Cells(m_CellIdxTmpFreiesKontingent).Visible = True
            e.Item.Cells(m_CellIdxTmpFrist).Visible = True
            e.Item.Cells(m_CellIdxTmpSperre).Visible = True
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
            cell.ColumnSpan = 5
            cell.HorizontalAlign = HorizontalAlign.Center
            header.Add(cell)
            countVisibleKontingentarten += 1
        End If

        If chkEndgueltig.Checked Then
            cell = New TableCell()
            cell.Text = "Endgültig"
            cell.ColumnSpan = 5
            cell.HorizontalAlign = HorizontalAlign.Center
            header.Add(cell)
            countVisibleKontingentarten += 1
        End If

        If chkRetail.Checked Then
            cell = New TableCell()
            cell.Text = "Retail"
            cell.ColumnSpan = 4
            cell.HorizontalAlign = HorizontalAlign.Center
            header.Add(cell)
            countVisibleKontingentarten += 1
        End If

        If chkDelayed.Checked Then
            cell = New TableCell()
            cell.Text = "Delayed payment"
            cell.ColumnSpan = 4
            cell.HorizontalAlign = HorizontalAlign.Center
            header.Add(cell)
            countVisibleKontingentarten += 1
        End If

        If chkHEZ.Checked Then
            cell = New TableCell()
            cell.Text = "Händlereigene<br>Zulassung"
            cell.ColumnSpan = 4
            cell.HorizontalAlign = HorizontalAlign.Center
            header.Add(cell)
            countVisibleKontingentarten += 1
        End If

        If chkKFKL.Checked Then
            cell = New TableCell()
            cell.Text = "KF/KL"
            cell.ColumnSpan = 4
            cell.HorizontalAlign = HorizontalAlign.Center
            header.Add(cell)
            countVisibleKontingentarten += 1
        End If


        'Eine Kontingentart soll immer sichtbar sein
        If countVisibleKontingentarten = 0 Then
            cell = New TableCell()
            cell.Text = "Temporär"
            cell.ColumnSpan = 5
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
                reportExcel.Columns.Remove("Delayed payment Richtwert")
                reportExcel.Columns.Remove("Delayed payment Ausschöpfung")
            End If
            If Not chkHEZ.Checked Then
                reportExcel.Columns.Remove("HEZ Richtwert")
                reportExcel.Columns.Remove("HEZ Ausschöpfung")
            End If

            reportExcel.AcceptChanges()
            Dim objExcelExport As New Excel.ExcelExport()
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
' User: Jungj        Date: 24.06.08   Time: 15:00
' Updated in $/CKAG/Applications/AppFFE/forms
' ITA 2033 Warten auf Feld�bersetzungs IT Anforderung
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 4.06.08    Time: 18:43
' Updated in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 15.05.08   Time: 14:33
' Updated in $/CKAG/Applications/AppFFE/forms
' ITA:1865
' 
' ************************************************
