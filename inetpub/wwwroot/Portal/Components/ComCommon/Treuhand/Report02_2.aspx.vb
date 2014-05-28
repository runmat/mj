
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Namespace Treuhand
    Partial Public Class Report02_2
        Inherits System.Web.UI.Page

#Region "Declarations"
        Private m_User As Base.Kernel.Security.User
        Private m_App As Base.Kernel.Security.App
        Private TreuhandBestand As SperreFreigabe
#End Region

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            m_User = GetUser(Me)
            ucHeader.InitUser(m_User)
            FormAuth(Me, m_User)
            GetAppIDFromQueryString(Me)

            lnkKreditlimit.NavigateUrl = "Report02.aspx?AppID=" & Session("AppID").ToString

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            TreuhandBestand = CType(Session("TreuhandBestand"), SperreFreigabe)
            If Not IsPostBack Then
                ddlPageSize.Items.Add("10")
                ddlPageSize.Items.Add("20")
                ddlPageSize.Items.Add("50")
                ddlPageSize.Items.Add("100")
                ddlPageSize.Items.Add("250")
                ddlPageSize.SelectedIndex = 2
                rbKeineZBII.Checked = True
                FillGrid(0)
            End If
        End Sub
        Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
            Dim tmpDataView As New DataView()
            TreuhandBestand.Bestand.DefaultView.RowFilter = ""
            tmpDataView = TreuhandBestand.Bestand.DefaultView
            If rbohneDokumente.Checked Then
                tmpDataView.RowFilter = "Fehlercode = '99'And  Erledigt IS Null"
            ElseIf rbKeineZBII.Checked Then
                tmpDataView.RowFilter = "Fehlercode = '05'"
            ElseIf rbAndererTG.Checked Then
                tmpDataView.RowFilter = "Fehlercode = '06'"
            End If


            If tmpDataView.Count = 0 Then
                GridView1.Visible = False
                ddlPageSize.Visible = False
                lblNoData.Text = "Keine Daten zur Anzeige gefunden."
                lblNoData.Visible = True
                lnkCreateExcel.Visible = False
                ExcelImg.Visible = False
            Else
                ddlPageSize.Visible = True
                Dim intTempPageIndex As Int32 = intPageIndex
                Dim strTempSort As String = ""
                Dim strDirection As String = ""
                lblNoData.Text = ""
                GridView1.Visible = True
                lnkCreateExcel.Visible = True
                ExcelImg.Visible = True
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

                GridView1.PageIndex = intTempPageIndex
                GridView1.PagerStyle.CssClass = "PagerStyle"
                GridView1.DataSource = tmpDataView
                GridView1.DataBind()

                If GridView1.PageCount > 1 Then
                    GridView1.PagerSettings.Visible = True
                Else
                    GridView1.PagerSettings.Visible = False
                End If
                If rbohneDokumente.Checked Then
                    GridView1.Columns(1).Visible = False
                    GridView1.Columns(2).Visible = False
                    GridView1.Columns(3).Visible = False
                End If
            End If
        End Sub

        Protected Sub rbKeineZBII_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rbKeineZBII.CheckedChanged
            FillGrid(0)
        End Sub

        Protected Sub rbohneDokumente_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rbohneDokumente.CheckedChanged
            FillGrid(0)
        End Sub

        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
            SetEndASPXAccess(Me)
        End Sub

        Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
            SetEndASPXAccess(Me)
        End Sub

        Private Function FilteredExcelTable() As DataTable
            Dim tmpDataView As New DataView()
            Dim tblTemp As DataTable

            TreuhandBestand.Bestand.DefaultView.RowFilter = ""
            tmpDataView = TreuhandBestand.Bestand.DefaultView
            If rbohneDokumente.Checked Then
                tmpDataView.RowFilter = "Fehlercode = '99'And  Erledigt IS Null"
            ElseIf rbKeineZBII.Checked Then
                tmpDataView.RowFilter = "Fehlercode = '05'"
            ElseIf rbAndererTG.Checked Then
                tmpDataView.RowFilter = "Fehlercode = '06'"
            End If

            tblTemp = TreuhandBestand.Bestand.Clone

            Dim tempNewRow As DataRow
            For i As Integer = 0 To tmpDataView.Count - 1
                tempNewRow = tblTemp.NewRow
                tempNewRow("Fahrgestellnummer") = tmpDataView.Item(i)("Fahrgestellnummer").ToString
                tempNewRow("NummerZBII") = tmpDataView.Item(i)("NummerZBII").ToString
                tempNewRow("Kennzeichen") = tmpDataView.Item(i)("Kennzeichen").ToString
                tempNewRow("Vertragsnummer") = tmpDataView.Item(i)("Vertragsnummer").ToString
                tempNewRow("Fehlercode") = tmpDataView.Item(i)("Fehlercode").ToString
                tempNewRow("Fehlertext") = tmpDataView.Item(i)("Fehlertext").ToString
                tempNewRow("bearbeitet") = tmpDataView.Item(i)("bearbeitet").ToString
                tempNewRow("bearbeitetam") = CDate(tmpDataView.Item(i)("bearbeitetam").ToString).ToShortDateString
                tblTemp.Rows.Add(tempNewRow)
            Next
            Return tblTemp
        End Function

        Protected Sub lnkCreateExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel.Click
            Dim control As New Control
            Dim tblTranslations As New DataTable()
            Dim tblTemp As DataTable
            Dim AppURL As String
            Dim col As DataControlField
            Dim col2 As DataColumn
            Dim bVisibility As Integer
            Dim i As Integer
            Dim sColName As String = ""
            tblTemp = FilteredExcelTable()
            AppURL = Replace(Me.Request.Url.LocalPath, "/Portal", "..")
            tblTranslations = CType(Me.Session(AppURL), DataTable)
            For Each col In GridView1.Columns
                For i = tblTemp.Columns.Count - 1 To 0 Step -1
                    bVisibility = 0
                    col2 = tblTemp.Columns(i)
                    If col2.ColumnName.ToUpper = col.SortExpression.ToUpper Then
                        sColName = TranslateColLbtn(GridView1, tblTranslations, col.HeaderText, bVisibility)
                        If bVisibility = 0 Then
                            tblTemp.Columns.Remove(col2)
                        ElseIf sColName.Length > 0 Then
                            col2.ColumnName = sColName
                        End If
                    End If
                Next
                tblTemp.AcceptChanges()
            Next
            Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
            excelFactory.CreateDocumentAndSendAsResponse(strFileName, tblTemp, Me.Page)

        End Sub

        Private Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
            FillGrid(e.NewPageIndex)
        End Sub

        Protected Sub ddlPageSize_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlPageSize.SelectedIndexChanged
            GridView1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
            FillGrid(0)
        End Sub

        Private Sub GridView1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
            FillGrid(0, e.SortExpression)
        End Sub

        Private Sub rbAndererTG_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbAndererTG.CheckedChanged
            FillGrid(0)
        End Sub
    End Class
End Namespace
