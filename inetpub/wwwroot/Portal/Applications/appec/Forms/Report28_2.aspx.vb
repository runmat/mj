Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Report28_2
    Inherits System.Web.UI.Page

    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
   
    Private m_User As Base.Kernel.Security.User
    Private m_objTable As DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        If (Session("ResultTable") Is Nothing) Then
            Response.Redirect(Request.UrlReferrer.ToString)
        Else
            m_objTable = CType(Session("ResultTable"), DataTable)
        End If

        Try

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            
            If Not IsPostBack Then
                ddlPageSize.Items.Add("10")
                ddlPageSize.Items.Add("20")
                ddlPageSize.Items.Add("50")
                ddlPageSize.Items.Add("100")
                ddlPageSize.SelectedIndex = 2

                'If Not Session("lnkExcel").ToString.Length = 0 Then
                '    lblDownloadTip.Visible = True
                '    lnkExcel.Visible = True
                '    lnkExcel.NavigateUrl = Session("lnkExcel").ToString
                'End If

                If Not Session("lnkCSV").ToString.Length = 0 Then
                    lblDownloadTip.Visible = True
                    lnkCSV.Visible = True
                    lnkCSV.NavigateUrl = Session("lnkCSV").ToString
                End If

                FillGrid(0)
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten."
        End Try
        
    End Sub

    Private Sub GenerateExcel()
        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
       
        Dim tblExcel As DataTable = Session("ResultTable")

        Try
            Excel.ExcelExport.WriteExcel(tblExcel, ConfigurationManager.AppSettings("ExcelPath") & strFileName)

            ' ### CSV Export
            'strFileName = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".csv"
            'GeneralTools.Services.CsvService.CreateCSV(objBestand.Result, ConfigurationManager.AppSettings("ExcelPath") & strFileName)

            ' ### Aspose
            'Dim Excelobj As New DocumentGeneration.ExcelDocumentFactory
            'Excelobj.CreateDocumentAndWriteToFilesystem(ConfigurationManager.AppSettings("ExcelPath") & strFileName, objBestand.Result, Me)
        Catch
        End Try

        Dim AbsoluterPfadZumVirtuellenVerz As String = ConfigurationManager.AppSettings("ReplaceExcelPath")

        lnkExcel2.NavigateUrl = ConfigurationManager.AppSettings("ExcelPath").Replace(AbsoluterPfadZumVirtuellenVerz, "") & strFileName & "".Replace("/", "\")
        lnkExcel2.Visible = True
        lblDownloadTip.Visible = True

        btnUpdateExcel.Visible = False
    End Sub

    ''' <summary>
    ''' Grid mit Daten füllen
    ''' </summary>
    ''' <param name="intPageIndex"></param>
    ''' <param name="strSort"></param>
    ''' <remarks></remarks>
    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        Dim intTempPageIndex As Int32 = intPageIndex

        If m_objTable.Rows.Count = 0 Then
            DataGrid1.Visible = False
            lblNoData.Visible = True
            lblNoData.Text = "Keine Daten zur Anzeige gefunden."
        Else
            DataGrid1.Visible = True
            lblNoData.Visible = False

            Dim tmpDataView As DataView = m_objTable.DefaultView

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

            lblNoData.Text = "Es wurden " & tmpDataView.Count.ToString & " Einträge zu """ & m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString & """ gefunden."
            lblNoData.Visible = True

            If DataGrid1.PageCount > 1 Then
                DataGrid1.PagerStyle.CssClass = "PagerStyle"
                DataGrid1.DataBind()
                DataGrid1.PagerStyle.Visible = True
            Else
                DataGrid1.PagerStyle.Visible = False
            End If
        End If
    End Sub

    ''' <summary>
    ''' Bei Datenbindung der Datenzeilen den Link zur Fahrzeughistorie setzen
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub DataGrid1_ItemDataBound(ByVal source As Object, ByVal e As DataGridItemEventArgs) Handles DataGrid1.ItemDataBound

        'Die Fahrzeughistorie wurde dem User nicht zugeordnet
        If m_User.Applications.Select("AppName = 'Report15'").Length = 0 Then
            lblError.Text = "Hinweis: Die Fahrzeughistorie kann nicht über die Fahrgestellnummer aufgerufen werden, da sie nicht in der Gruppe enthalten ist."
            lblError.Visible = True
            Exit Sub
        End If

        'Dim datCell As TableCell
        'Dim chkBox As CheckBox
        Dim strHistoryLink As String = "Report15.aspx?AppID=" & m_User.Applications.Select("AppName = 'Report15'")(0)("AppID").ToString() & "&VIN="

        For Each cl As TableCell In e.Item.Cells
            Dim lnkHistorie As HyperLink = DirectCast(cl.FindControl("lnkHistorie"), HyperLink)
            If Not lnkHistorie Is Nothing Then
                lnkHistorie.NavigateUrl = strHistoryLink & lnkHistorie.Text
            End If
        Next
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(0)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Protected Sub cmdBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdBack.Click
        Response.Redirect("Report28.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Protected Sub btnUpdateExcel_Click(sender As Object, e As System.EventArgs) Handles btnUpdateExcel.Click

        If Not lnkExcel2.Visible And btnUpdateExcel.Enabled Then
            GenerateExcel()
            btnUpdateExcel.Enabled = True
        End If
    End Sub
    
End Class