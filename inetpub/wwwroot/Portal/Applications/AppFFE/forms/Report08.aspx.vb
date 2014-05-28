Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Partial Public Class Report08
    Inherits System.Web.UI.Page


    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App

    Private resultTable As DataTable
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        m_App = New Base.Kernel.Security.App(m_User)

        GetAppIDFromQueryString(Me)

        If Not IsPostBack Then
            ddlPageSize.Items.Add("10")
            ddlPageSize.Items.Add("20")
            ddlPageSize.Items.Add("50")
            ddlPageSize.Items.Add("100")
            ddlPageSize.Items.Add("200")
            ddlPageSize.Items.Add("500")
            ddlPageSize.SelectedIndex = 2
            'tdLegende.Visible = False
            lnkExcel.Visible = False
            imgExcel.Visible = False
            lblDownloadTip.Visible = False
            imgExcel.Visible = False
            'tdPageTitle.Visible = False
            ddlKontingentart.Items.Add("ALLE")
            ddlKontingentart.Items.Add("Standard temporär")
            ddlKontingentart.Items.Add("Standard endgültig")
            Try
                lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
                ucStyles.TitleText = lblHead.Text

                txtJahr.Text = Now.Year.ToString

                'DoSubmit()
            Catch ex As Exception
                lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten."
            End Try
        End If
    End Sub

    'Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
    '    DoSubmit()
    'End Sub

    Private Sub DoSubmit()
        Session("lnkExcel") = ""
        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
        Dim m_Report As FFE_Bank_HaendlerPerformance
        Dim filterExpression As String = ""
        Dim intCounter As Integer

        Select Case ddlKontingentart.SelectedItem.Value
            Case "Standard temporär"
                filterExpression &= "TEMP"
            Case "Standard endgültig"
                filterExpression &= "ENDG"
        End Select

        Try
            'If Session("ResultTable") Is Nothing Then
            m_Report = New FFE_Bank_HaendlerPerformance(m_User, m_App, strFileName)
            m_Report.Geschaeftsjahr = txtJahr.Text
            m_Report.FILL(Session("AppID").ToString, Session.SessionID.ToString)
            Session("ResultTable") = m_Report.ResultTable
            'End If

            lblError.Text = ""
            resultTable = CType(Session("ResultTable"), DataTable)

            If lblError.Text.Length = 0 Then
                If resultTable Is Nothing OrElse resultTable.Rows.Count = 0 Then
                    lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                    FillGrid(0)
                Else
                    FillGrid(0, , filterExpression)
                    Dim objExcelExport As New Excel.ExcelExport()
                    Try
                        '§§§ JVE 20.09.2006: Unpassende Datensätze entfernen
                        If Not filterExpression Is Nothing Then
                            For intCounter = resultTable.Rows.Count - 1 To 0 Step -1
                                If CStr(resultTable.Rows(intCounter)("Versandart")) <> filterExpression Then
                                    resultTable.Rows.Remove(resultTable.Rows(intCounter))
                                End If
                            Next
                            resultTable.AcceptChanges()
                        End If
                        '----------------------------------------------------
                        Excel.ExcelExport.WriteExcel(resultTable, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                        Session("lnkExcel") = "/Portal/Temp/Excel/" & strFileName
                        lblDownloadTip.Visible = True
                        lnkExcel.Visible = True
                        imgExcel.Visible = True
                        lnkExcel.NavigateUrl = Session("lnkExcel").ToString
                    Catch ex As Exception
                    End Try
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten."
        End Try
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "", Optional ByVal filterExpression As String = "")
        resultTable = CType(Session("ResultTable"), DataTable)
        Dim rows As DataRow()

        If (resultTable Is Nothing) OrElse (resultTable.Rows.Count = 0) Then
            DataGrid1.Visible = False
            lblError.Text = "Keine Daten zur Anzeige gefunden."
            lnkExcel.Visible = False
            lblDownloadTip.Visible = False
            ddlPageSize.Visible = False
            imgExcel.Visible = False
        Else
            If (filterExpression Is Nothing) OrElse (filterExpression = String.Empty) Then
                rows = resultTable.Select("Versandart <> ''")
            Else
                rows = resultTable.Select("Versandart = '" & filterExpression & "'")
            End If

            If rows.Length = 0 Then
                DataGrid1.Visible = False
                lblError.Text = "Keine Daten zur Anzeige gefunden."
                lnkExcel.Visible = False
                imgExcel.Visible = False
                lblDownloadTip.Visible = False
                ddlPageSize.Visible = False
                'tdLegende.Visible = False
            Else
                lblMessage.Text = " Es wurden " & rows.Length & " Datensätze gefunden."
                DataGrid1.Visible = True
                imgExcel.Visible = True
                lnkExcel.Visible = True
                lblDownloadTip.Visible = True
                ddlPageSize.Visible = True
                'tdLegende.Visible = True

                Dim tmpDataView As New DataView()
                tmpDataView = resultTable.DefaultView

                If filterExpression <> String.Empty Then
                    tmpDataView.RowFilter = "Versandart = '" & filterExpression & "'"        'Hier Filterkriterien setzen
                End If

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

                If DataGrid1.PageCount > 1 Then
                    DataGrid1.PagerStyle.CssClass = "PagerStyle"
                    DataGrid1.DataBind()
                    DataGrid1.PagerStyle.Visible = True
                Else
                    DataGrid1.PagerStyle.Visible = False
                End If
            End If
        End If
    End Sub

    'Private Sub DataGrid1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    'End Sub

    'Private Sub DataGrid1_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
    '    
    'End Sub

    'Private Sub DataGrid1_SortCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)

    'End Sub

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(0)
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        DoSubmit()
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub DataGrid1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGrid1.SelectedIndexChanged
        DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(0)
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class
' ************************************************
' $History: Report08.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 4.06.08    Time: 18:43
' Updated in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 15.05.08   Time: 14:33
' Updated in $/CKAG/Applications/AppFFE/forms
' ITA:1865
' 
' ************************************************
