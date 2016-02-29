Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG

Public Class Report06
    Inherits Page

    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private m_objTable As DataTable
    Private m_strAppID As String

    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblHead As Label
    Protected WithEvents lblPageTitle As Label
    Protected WithEvents lnkKreditlimit As HyperLink
    Protected WithEvents cmdSave As LinkButton
    Protected WithEvents ddlPageSize1 As DropDownList
    Protected WithEvents ddlPageSize2 As DropDownList
    Protected WithEvents lblNoData1 As Label
    Protected WithEvents lblNoData2 As Label
    Protected WithEvents lblError As Label
    Protected WithEvents DataGrid1 As DataGrid
    Protected WithEvents DataGrid2 As DataGrid
    Protected WithEvents ShowScript As HtmlTableRow
    Protected WithEvents lnkCreateExcel As LinkButton

#Region " Vom Web Form Designer generierter Code "

    'Dieser Aufruf ist für den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: Diese Methode ist für den Web Form-Designer erforderlich
        'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
        InitializeComponent()
    End Sub

#End Region

#Region " Methods "

    Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        GetAppIDFromQueryString(Me)

        Try
            m_strAppID = CStr(Request.QueryString("AppID"))
            cmdSave.Enabled = False
            m_User = GetUser(Me)
            ucHeader.InitUser(m_User)
            FormAuth(Me, m_User)
            m_App = New Base.Kernel.Security.App(m_User)

            Try
                If Not IsPostBack Then
                    Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"

                    lblHead.Text = m_User.Applications.Select("AppID = " & m_strAppID)(0)("AppFriendlyName").ToString
                    ucStyles.TitleText = lblHead.Text

                    Dim m_Report As F1_Bank_TempMahn
                    If Not Session("Mahnungen") Is Nothing Then
                        m_objTable = CType(Session("Mahnungen"), DataTable)
                        ucHeader.LockLinks()
                        If IsNothing(Session("AusMahnSumme")) = False Then
                            lnkCreateExcel.Visible = True
                        End If
                    Else
                        m_Report = New F1_Bank_TempMahn(m_User, m_App, m_strAppID, Session.SessionID.ToString, strFileName)
                        m_Report.fill(Session("AppID").ToString, Session.SessionID)

                        If Not m_Report.Status = 0 Then
                            If m_Report.Status = -1 Then
                                lblError.Text = m_Report.Message
                                Exit Sub
                            Else
                                'wenn keine Mahnungen vorhanden sind, sofortige weiterleitung zur Startseite JJ2007.12.13
                                Response.Redirect("../../../Start/Selection.aspx")
                            End If

                        Else
                            If m_Report.NewResultTable.Rows.Count = 0 Then
                                'wenn keine Mahnungen vorhanden sind, sofortige weiterleitung zur Startseite JJ2007.12.13
                                Response.Redirect("../../../Start/Selection.aspx")
                            Else
                                m_objTable = m_Report.NewResultTable
                                Session.Add("Mahnungen", m_objTable)
                                ucHeader.LockLinks()
                                lnkCreateExcel.Visible = True
                            End If
                        End If
                    End If

                    ddlPageSize1.Items.Add("10")
                    ddlPageSize1.Items.Add("20")
                    ddlPageSize1.Items.Add("50")
                    ddlPageSize1.Items.Add("100")
                    ddlPageSize1.Items.Add("200")
                    ddlPageSize1.Items.Add("500")
                    ddlPageSize1.Items.Add("1000")
                    ddlPageSize1.SelectedIndex = 2

                    ddlPageSize2.Items.Add("10")
                    ddlPageSize2.Items.Add("20")
                    ddlPageSize2.Items.Add("50")
                    ddlPageSize2.Items.Add("100")
                    ddlPageSize2.Items.Add("200")
                    ddlPageSize2.Items.Add("500")
                    ddlPageSize2.Items.Add("1000")
                    ddlPageSize2.SelectedIndex = 2

                    If (Not Session("BackLink") Is Nothing) AndAlso CStr(Session("BackLink")) = "HistoryBack" Then
                        lnkKreditlimit.Text = "Zurück"
                        lnkKreditlimit.NavigateUrl = "javascript:history.back()"
                    End If

                    ViewState("Direction1") = "asc"
                    ViewState("Direction2") = "asc"
                    FillGrid1(0, "MAHNART")
                    FillGrid2(0, "MAHNART")

                    If Not m_objTable Is Nothing AndAlso m_objTable.Rows.Count > 0 Then
                        Try
                            Excel.ExcelExport.WriteExcel(m_objTable, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                        Catch
                        End Try
                    End If
                Else
                    If Not Session("Mahnungen") Is Nothing Then
                        m_objTable = CType(Session("Mahnungen"), DataTable)
                    End If
                End If

            Catch ex As Exception
                lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Try
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub FillGrid1(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        Dim tmpDataView As New DataView(m_objTable)
        tmpDataView.RowFilter = "ZZREFERENZ2 <> '1' AND ZZREFERENZ2 <> '2'"

        If tmpDataView.Count = 0 Then
            DataGrid1.Visible = False
            lblNoData1.Visible = True
            lblNoData1.Text = "Keine Daten zur Anzeige gefunden."
            ShowScript.Visible = False
        Else
            DataGrid1.Visible = True
            lblNoData1.Text = ""
            lblNoData1.Visible = False

            Dim intTempPageIndex As Int32 = intPageIndex
            Dim strTempSort As String = ""
            Dim strDirection As String = ""

            If strSort.Trim(" "c).Length > 0 Then
                intTempPageIndex = 0
                strTempSort = strSort.Trim(" "c)
                If (ViewState("Sort1") Is Nothing) OrElse (ViewState("Sort1").ToString = strTempSort) Then
                    If ViewState("Direction1") Is Nothing Then
                        strDirection = "desc"
                    Else
                        strDirection = ViewState("Direction1").ToString
                    End If
                Else
                    strDirection = "desc"
                End If

                If strDirection = "asc" Then
                    strDirection = "desc"
                Else
                    strDirection = "asc"
                End If

                ViewState("Sort1") = strTempSort
                ViewState("Direction1") = strDirection
            Else
                If Not ViewState("Sort1") Is Nothing Then
                    strTempSort = ViewState("Sort1").ToString
                    If ViewState("Direction1") Is Nothing Then
                        strDirection = "asc"
                        ViewState("Direction1") = strDirection
                    Else
                        strDirection = ViewState("Direction1").ToString
                    End If
                End If
            End If

            If Not strTempSort.Length = 0 Then
                tmpDataView.Sort = strTempSort & " " & strDirection
            End If

            DataGrid1.PageSize = CInt(ddlPageSize1.SelectedItem.Value)
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

            For Each item As DataGridItem In DataGrid1.Items

                If Not item.FindControl("lnkHistorie") Is Nothing Then

                    If Not m_User.Applications.Select("AppName = 'Report46'").Count = 0 Then

                        CType(item.FindControl("lnkHistorie"), HyperLink).NavigateUrl = "../../../Components/ComCommon/Finance/Report46.aspx?AppID=" & m_User.Applications.Select("AppName = 'Report46'")(0)("AppID").ToString & "&VIN=" & CType(item.FindControl("lnkHistorie"), HyperLink).Text

                    End If

                End If

            Next

        End If

        TryEnableSaveButton()
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        FillGrid1(e.NewPageIndex)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid1(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub ddlPageSize1_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlPageSize1.SelectedIndexChanged
        DataGrid1.PageSize = CInt(ddlPageSize1.SelectedItem.Value)
        FillGrid1(0)
    End Sub

    Private Sub FillGrid2(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        Dim tmpDataView As New DataView(m_objTable)
        tmpDataView.RowFilter = "ZZREFERENZ2 = '1' OR ZZREFERENZ2 = '2'"

        If tmpDataView.Count = 0 Then
            DataGrid2.Visible = False
            lblNoData2.Visible = True
            lblNoData2.Text = "Keine Daten zur Anzeige gefunden."
            ShowScript.Visible = False
        Else
            DataGrid2.Visible = True
            lblNoData2.Text = ""
            lblNoData2.Visible = False

            Dim intTempPageIndex As Int32 = intPageIndex
            Dim strTempSort As String = ""
            Dim strDirection As String = ""

            If strSort.Trim(" "c).Length > 0 Then
                intTempPageIndex = 0
                strTempSort = strSort.Trim(" "c)
                If (ViewState("Sort2") Is Nothing) OrElse (ViewState("Sort2").ToString = strTempSort) Then
                    If ViewState("Direction2") Is Nothing Then
                        strDirection = "desc"
                    Else
                        strDirection = ViewState("Direction2").ToString
                    End If
                Else
                    strDirection = "desc"
                End If

                If strDirection = "asc" Then
                    strDirection = "desc"
                Else
                    strDirection = "asc"
                End If

                ViewState("Sort2") = strTempSort
                ViewState("Direction2") = strDirection
            Else
                If Not ViewState("Sort2") Is Nothing Then
                    strTempSort = ViewState("Sort2").ToString
                    If ViewState("Direction2") Is Nothing Then
                        strDirection = "asc"
                        ViewState("Direction2") = strDirection
                    Else
                        strDirection = ViewState("Direction2").ToString
                    End If
                End If
            End If

            If Not strTempSort.Length = 0 Then
                tmpDataView.Sort = strTempSort & " " & strDirection
            End If

            DataGrid2.PageSize = CInt(ddlPageSize2.SelectedItem.Value)
            DataGrid2.CurrentPageIndex = intTempPageIndex

            DataGrid2.DataSource = tmpDataView
            DataGrid2.DataBind()

            If DataGrid2.PageCount > 1 Then
                DataGrid2.PagerStyle.CssClass = "PagerStyle"
                DataGrid2.DataBind()
                DataGrid2.PagerStyle.Visible = True
            Else
                DataGrid2.PagerStyle.Visible = False
            End If

            For Each item As DataGridItem In DataGrid2.Items

                If Not item.FindControl("lnkHistorie") Is Nothing Then

                    If Not m_User.Applications.Select("AppName = 'Report46'").Count = 0 Then

                        CType(item.FindControl("lnkHistorie"), HyperLink).NavigateUrl = "../../../Components/ComCommon/Finance/Report46.aspx?AppID=" & m_User.Applications.Select("AppName = 'Report46'")(0)("AppID").ToString & "&VIN=" & CType(item.FindControl("lnkHistorie"), HyperLink).Text

                    End If

                End If

            Next

        End If

        TryEnableSaveButton()
    End Sub

    Private Sub DataGrid2_PageIndexChanged(ByVal source As Object, ByVal e As DataGridPageChangedEventArgs) Handles DataGrid2.PageIndexChanged
        FillGrid2(e.NewPageIndex)
    End Sub

    Private Sub DataGrid2_SortCommand(ByVal source As Object, ByVal e As DataGridSortCommandEventArgs) Handles DataGrid2.SortCommand
        FillGrid2(DataGrid2.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub ddlPageSize2_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlPageSize2.SelectedIndexChanged
        DataGrid2.PageSize = CInt(ddlPageSize2.SelectedItem.Value)
        FillGrid2(0)
    End Sub

    Protected Sub lnkCreateExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel.Click
        Try
            Dim AppURL As String = Replace(Request.Url.LocalPath, "/Portal", "..")
            Dim tblTranslations As DataTable = CType(Session(AppURL), DataTable)
            Dim tblTemp As DataTable = CType(Session("Mahnungen"), DataTable).Copy

            'hilfsspalten
            tblTemp.Columns.Remove("MAHNART")
            tblTemp.Columns.Remove("AG")
            tblTemp.Columns.Remove("HAENDLER")
            tblTemp.Columns.Remove("HDGRP")
            tblTemp.Columns.Remove("AUGRU")
            tblTemp.Columns.Remove("HDGRP_EX")

            For Each col As DataGridColumn In DataGrid1.Columns
                For i As Integer = tblTemp.Columns.Count - 1 To 0 Step -1
                    Dim bVisibility As Integer = 0
                    Dim col2 As DataColumn = tblTemp.Columns(i)
                    If col2.ColumnName.ToUpper = col.SortExpression.ToUpper OrElse col2.ColumnName.ToUpper = col.HeaderText.ToUpper.Replace("COL_", "") Then
                        Dim sColName As String = TranslateColLbtn(DataGrid1, tblTranslations, col.HeaderText, bVisibility)
                        If col2.ColumnName = "ZZREFERENZ2" Then
                            col2.ColumnName = "SLIM CONFIRMED"
                        ElseIf bVisibility = 0 Then
                            tblTemp.Columns.Remove(col2)
                        ElseIf sColName.Length > 0 Then
                            col2.ColumnName = sColName
                        Else
                            'alle spalten die nicht in der spaltenübersetzung sind, entfernen
                            tblTemp.Columns.Remove(col2)
                        End If
                    End If
                Next
                tblTemp.AcceptChanges()
            Next
            Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
            excelFactory.CreateDocumentAndSendAsResponse(strFileName, tblTemp, Page)
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub TryEnableSaveButton()
        If DataGrid1.PageCount <= DataGrid1.CurrentPageIndex + 1 AndAlso DataGrid2.PageCount <= DataGrid2.CurrentPageIndex + 1 Then
            cmdSave.Enabled = True
        End If
    End Sub

    Private Sub cmdSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSave.Click
        If IsNothing(Session("AusMahnSumme")) = True Then
            Session("Mahnungen") = Nothing
            Response.Redirect("../../../Start/Selection.aspx")
        Else
            Session("Mahnungen") = Nothing
            Session("AusMahnSumme") = Nothing
            Response.Redirect("Report08.aspx?AppID=" & Session("AppID").ToString)
        End If

    End Sub

#End Region

End Class
