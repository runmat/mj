Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business

Public Class Report44
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

    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private m_objTable As DataTable
    Private m_strAppID As String
    'Private m_context As HttpContext = HttpContext.Current
    Dim m_Report As CKG.Components.ComCommon.Fin_07

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents Hyperlink2 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ShowScript As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents lnkKreditlimit As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblDownloadTip As System.Web.UI.WebControls.Label
    Protected WithEvents lnkExcel As System.Web.UI.WebControls.HyperLink
    Protected WithEvents ucHeader As Header
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cmdPrint As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetAppIDFromQueryString(Me)


        m_strAppID = CStr(Request.QueryString("AppID"))
        cmdSave.Enabled = False
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        m_App = New Base.Kernel.Security.App(m_User)

        Dim strFileName As String
        strFileName = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"

        If Not IsPostBack Then
            lblHead.Text = m_User.Applications.Select("AppID = " & m_strAppID)(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text


            If Not Session("Mahnungen") Is Nothing Then
                m_objTable = CType(Session("Mahnungen"), DataTable)
                ucHeader.LockLinks()
            Else
                m_Report = New Fin_07(m_User, m_App, strFileName)
                m_Report.Fill(m_strAppID, Session.SessionID.ToString, Request.QueryString.Item("ZZKKBER"))

                If Not m_Report.Status = 0 Then
                    If Not m_Report.Status = -1 Then
                        lblError.Text = m_Report.Message
                        Exit Sub
                    Else
                        'wenn keine Mahnungen vorhanden sind, sofortige weiterleitung zur Startseite JJ2007.12.13
                        Response.Redirect("../../../Start/Selection.aspx")
                    End If

                Else
                    If m_Report.Result.Rows.Count = 0 Then
                        lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                        Exit Sub
                    Else
                        m_objTable = m_Report.Result
                        Session.Add("Mahnungen", m_objTable)
                    End If
                End If
            End If

            ddlPageSize.Items.Add("10")
            ddlPageSize.Items.Add("20")
            ddlPageSize.Items.Add("50")
            ddlPageSize.Items.Add("100")
            ddlPageSize.Items.Add("200")
            ddlPageSize.Items.Add("500")
            ddlPageSize.Items.Add("1000")
            ddlPageSize.SelectedIndex = 2

            ViewState("Direction") = "asc"
            FillGrid(0, "Kontingentart")
        Else
            If Not Session("Mahnungen") Is Nothing Then
                m_objTable = CType(Session("Mahnungen"), DataTable)
            End If
        End If

        If Not m_objTable Is Nothing AndAlso m_objTable.Rows.Count > 0 Then
            'Dim objExcelExport As New Excel.ExcelExport()

            Excel.ExcelExport.WriteExcel(m_objTable, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
           
            lblDownloadTip.Visible = True
            lnkExcel.Visible = True
            lnkExcel.NavigateUrl = "/Portal/Temp/Excel/" & strFileName
        End If
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Log(m_User.Reference, "Erinnerung gesehen und mit ""OK"" bestätigt.")
        Response.Redirect("../../../Start/Selection.aspx")
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If m_objTable.Rows.Count = 0 Then
            DataGrid1.Visible = False
            lblNoData.Visible = True
            lblNoData.Text = "Keine Daten zur Anzeige gefunden."
            ShowScript.Visible = False
        Else
            DataGrid1.Visible = True
            lblNoData.Visible = False

            Dim tmpDataView As DataView = m_objTable.DefaultView

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

            DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
            DataGrid1.CurrentPageIndex = intTempPageIndex

            DataGrid1.DataSource = tmpDataView
            DataGrid1.DataBind()

            If (Not Session("ShowOtherString") Is Nothing) AndAlso CStr(Session("ShowOtherString")).Length > 0 Then
                lblNoData.Text = CStr(Session("ShowOtherString"))
            Else
                lblNoData.Text = "" '"Es wurden " & tmpDataView.Count.ToString & " Einträge zu """ & m_User.Applications.Select("AppID = " & m_strAppID)(0)("AppFriendlyName").ToString & """ gefunden."
            End If
            If (Not Session("BackLink") Is Nothing) AndAlso CStr(Session("BackLink")) = "HistoryBack" Then
                lnkKreditlimit.Text = "Zurück"
                lnkKreditlimit.NavigateUrl = "javascript:history.back()"
            End If
            lblNoData.Visible = True

            If DataGrid1.PageCount > 1 Then
                DataGrid1.PagerStyle.CssClass = "PagerStyle"
                DataGrid1.DataBind()
                DataGrid1.PagerStyle.Visible = True
            Else
                DataGrid1.PagerStyle.Visible = False
            End If
        End If
        With DataGrid1
            If .PageCount = .CurrentPageIndex + 1 Then
                cmdSave.Enabled = True
            End If
        End With
    End Sub

    Private Sub DataGrid1_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGrid1.ItemDataBound
        Dim intItem As Int32

        For intItem = 0 To m_objTable.Columns.Count - 1
            If m_objTable.Columns(intItem).DataType Is System.Type.GetType("System.DateTime") Then
                If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                    e.Item.Cells(intItem).Text = DataBinder.Eval(e.Item.DataItem, m_objTable.Columns(intItem).ColumnName, "{0:dd.MM.yyyy}")
                End If
            End If
        Next
    End Sub

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(0)
    End Sub

    'Private Sub cmdPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPrint.Click
    '    '##

    'End Sub

    Private Sub Log(ByVal strIdentification As String, ByVal strDescription As String, Optional ByVal strCategory As String = "APP")
        Dim logApp As New Base.Kernel.Logging.Trace(m_User.App.Connectionstring, m_User.App.SaveLogAccessSAP, m_User.App.LogLevel)

        ' strCategory
        Dim strUserName As String = m_User.UserName ' strUserName
        Dim strSessionID As String = Session.SessionID ' strSessionID
        Dim intSource As Integer = CInt(Request.QueryString("AppID")) ' intSource 
        Dim strTask As String = lblHead.Text ' strTask
        ' strIdentification
        ' strDescription
        Dim strCustomerName As String = m_User.CustomerName ' strCustomername
        Dim blnIsTestUser As Boolean = m_User.IsTestUser ' blnIsTestUser
        Dim intSeverity As Integer = 0 ' intSeverity 

        logApp.WriteEntry(strCategory, strUserName, strSessionID, intSource, strTask, strIdentification, strDescription, strCustomerName, m_User.Customer.CustomerId, blnIsTestUser, intSeverity)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.ItemCommand

        If e.CommandName = "link" Then

        End If
    End Sub

    Public Function createLink(ByVal Fahrgestellnummer As String) As String
        Dim dvAppLinks As DataView = m_User.Applications.DefaultView
        dvAppLinks.RowFilter = "APPURL='../Components/ComCommon/Finance/Report46.aspx'"

        If dvAppLinks.Count = 1 Then
            Dim strParameter As String = ""
            HelpProcedures.getAppParameters(dvAppLinks.Item(0).Item("AppID"), strParameter, ConfigurationManager.AppSettings("Connectionstring"))
            Return "Report46.aspx?AppID=" & dvAppLinks.Item(0).Item("AppID") & strParameter & "&VIN=" & Fahrgestellnummer
        Else
            lblError.Text = "Fehler bei der Weiterleitung zur Statusänderung"
            lblError.Visible = True
            Return ""
        End If
    End Function
End Class

' ************************************************
' $History: Report44.aspx.vb $
' 
' *****************  Version 5  *****************
' User: Dittbernerc  Date: 18.06.09   Time: 15:39
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2918 - abschaltung .net connector
' 
' BAPIS:
' 
' Z_M_Haendlerbestand
' Z_M_Faellige_Fahrzdok
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 5.06.09    Time: 15:58
' Updated in $/CKAG/Components/ComCommon/Finance
' mögliche try catches entfernt
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 11.04.08   Time: 13:43
' Updated in $/CKAG/Components/ComCommon/Finance
' Migration
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 10.04.08   Time: 17:37
' Updated in $/CKAG/Components/ComCommon/Finance
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:32
' Created in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 10:48
' Created in $/CKAG/portal/Components/ComCommon/Finance
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 18.02.08   Time: 15:10
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' akfÄnderungen
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 1.02.08    Time: 11:35
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' AKF Änderungen
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 18.12.07   Time: 15:55
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' BugFix Report 44/45
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 17.12.07   Time: 8:45
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 13.12.07   Time: 13:35
' Created in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Fällige Vorgänge Händler
' 
' ************************************************
