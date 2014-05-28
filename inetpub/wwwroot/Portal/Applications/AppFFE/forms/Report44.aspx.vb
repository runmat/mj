Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business

Partial Public Class Report44
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private objSuche As FFE_Search

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        'NoDealer(Me, m_User)
        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)
            ddlPageSize.Items.Add("10")
            ddlPageSize.Items.Add("20")
            ddlPageSize.Items.Add("50")
            ddlPageSize.Items.Add("100")
            ddlPageSize.Items.Add("200")
            ddlPageSize.Items.Add("500")
            ddlPageSize.Items.Add("1000")
            ddlPageSize.SelectedIndex = 2
            DoSubmit()
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Session("ShowLink") = "True"
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        Session("lnkExcel") = ""
        Try
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            Dim m_Report As New FFE_Bank_Imports(m_User, m_App, strFileName)

            m_Report.FILL(Session("AppID").ToString, Session.SessionID.ToString)

            Session("ResultTable") = m_Report.ResultTable

            If Not m_Report.Status = 0 Then
                lblError.Text = m_Report.Message
            Else
                If m_Report.ResultTable.Rows.Count = 0 Then
                    lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                Else

                    Dim objExcelExport As New Excel.ExcelExport()
                    Dim tblExcel As New DataTable()
                    Dim row As DataRow

                    For Each row In m_Report.ResultTable.Rows
                        If row("ZBEZAHLTE").ToString = "0000" Then row("ZBEZAHLTE") = ""
                        If row("ZUMFINANZ").ToString = "0000" Then row("ZUMFINANZ") = ""
                    Next
                    m_Report.ResultTable.AcceptChanges()
                    tblExcel = m_Report.ResultTable.Copy

                    Try
                        For Each row In tblExcel.Rows
                            '§§§ JVE 05.09.2006
                            Select Case row("Erfolg").ToString
                                Case "<img src=""/Portal/Images/erfolg.gif"">"
                                    row("Erfolg") = "Ja"
                                Case "<img src=""/Portal/Images/warnung.gif"">"
                                    row("Erfolg") = "Warnung"
                                Case "<img src=""/Portal/Images/fehler.gif"">"
                                    row("Erfolg") = "Nein"
                            End Select
                        Next

                        tblExcel.AcceptChanges()
                        Session("tblExcel") = tblExcel

                    Catch
                    End Try
                    FillGrid(0)
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Fehler: " & ex.Message
        End Try
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        Dim m_objTable As DataTable = Session("ResultTable")
        If m_objTable.Rows.Count = 0 Then
            DataGrid1.Visible = False
            lblNoData.Visible = True
            lblNoData.Text = "Keine Daten zur Anzeige gefunden."
        Else
            DataGrid1.Visible = True
            lblNoData.Visible = False

            Dim tmpDataView As New DataView()
            tmpDataView = m_objTable.DefaultView

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

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Protected Sub lnkCreateExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel.Click

        Try
            Dim control As New Control
            Dim tblTranslations As New DataTable()
            Dim AppURL As String
            Dim m_datatable As DataTable = Session("tblExcel")
            Dim col As DataGridColumn
            Dim col2 As DataColumn
            Dim bVisibility As Integer

            AppURL = Replace(Me.Request.Url.LocalPath, "/Portal", "..")
            tblTranslations = CType(Me.Session(AppURL), DataTable)
            For Each col In DataGrid1.Columns

                For Each col2 In m_datatable.Columns
                    If col2.ColumnName.ToUpper = col.SortExpression Then
                        col2.ColumnName = TranslateColLbtn(DataGrid1, tblTranslations, col.HeaderText, bVisibility)
                    End If
                Next
                m_datatable.AcceptChanges()
            Next
            Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
            excelFactory.CreateDocumentAndSendAsResponse(strFileName, m_datatable, Me.Page)
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Protected Sub ddlPageSize_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlPageSize.SelectedIndexChanged
        DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(0)
    End Sub
End Class
' ************************************************
' $History: Report44.aspx.vb $
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 4.06.08    Time: 18:43
' Updated in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 15.05.08   Time: 14:33
' Updated in $/CKAG/Applications/AppFFE/forms
' ITA:1865
' 
' ************************************************
