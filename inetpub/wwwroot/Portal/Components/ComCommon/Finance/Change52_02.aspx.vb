Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Change52_02
    Inherits System.Web.UI.Page

    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private m_objTable As DataTable

    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents cmdsave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lbl_Info As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblHidden As System.Web.UI.WebControls.Label
    Protected WithEvents ShowScript As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Private legende As String
    'Private csv As String
    'Private schmal As String
    Protected WithEvents lb_Back As System.Web.UI.WebControls.LinkButton
    Dim m_report As fin_04
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



    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)
        m_App = New Base.Kernel.Security.App(m_User)
        
        If (Session("ResultTable") Is Nothing) Then
            Response.Redirect(Request.UrlReferrer.ToString)
        Else
            m_objTable = CType(Session("ResultTable"), DataTable)
        End If

        legende = Request.QueryString.Item("legende")
        If (legende = "Ja") Then
            lbl_Info.Text = "<br>*(V)ollmacht, (H)andelsregistereintrag, (P)ersonalausweis, (G)ewerbeanmeldung, (E)inzugsermächtigung"
        End If
        If (legende = "Report201") Then
            lbl_Info.Text = "<br>'X' = vorhanden"
        End If
        If (legende = "Report203") Then
            lbl_Info.Text = "<br>*(V)ollmacht, (H)andelsregistereintrag, (P)ersonalausweis, (G)ewerbeanmeldung, (E)inzugsermächtigung, 'X' = vorhanden"
        End If

        BuildTextForLabelHead()

        legende = Request.QueryString.Item("legende")
        If (legende = "Ja") Then
            lbl_Info.Text = "<br>*(V)ollmacht, (H)andelsregistereintrag, (P)ersonalausweis, (G)ewerbeanmeldung, (E)inzugsermächtigung"
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

            If Not Session("ApplblInfoText") Is Nothing Then
                If lbl_Info.Text.Length = 0 Then
                    lbl_Info.Text = CStr(Session("ApplblInfoText"))
                Else
                    lbl_Info.Text &= "<br>" & CStr(Session("ApplblInfoText"))
                End If
            End If
            m_objTable.Columns.Add("Status", System.Type.GetType("System.String"))
            FillGrid(0)
        End If

    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdsave.Click
        doSubmit()
    End Sub
   
    Private Sub DataGrid1_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        FillGrid(e.NewPageIndex)
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
            tmpDataView.RowFilter = "Zuordnen = True"
            Dim intTempPageIndex As Int32 = intPageIndex
            Dim strTempSort As String = ""
            Dim strDirection As String = ""

            'mit Commandargument "X" gekennzeichnete Spalten überspringen, da nicht datengebunden
            If Not String.IsNullOrEmpty(strSort) AndAlso Not strSort = "X" Then
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
            Dim label As Label
            Dim control As control

            For Each item In DataGrid1.Items                    'Zeilen des DataGrids durchgehen...
                Dim datEingang As Date
                cell = item.Cells(5)
                For Each control In cell.Controls
                    Dim control1 = TryCast(control, Label)
                    If (control1 IsNot Nothing) Then
                        label = control1
                        If label.ID = "Label6" Then
                            If IsDate(label.Text) Then
                                datEingang = label.Text
                                label.Text = datEingang.ToShortDateString
                            End If
                        End If
                    End If
                Next

            Next

        End If
    End Sub

    Private Sub DataGrid1_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGrid1.ItemDataBound
        Dim intItem As Int32

        For intItem = 0 To m_objTable.Columns.Count - 1
            If m_objTable.Columns(intItem).DataType Is System.Type.GetType("System.DateTime") Then
                If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                    e.Item.Cells(intItem).Text = DataBinder.Eval(e.Item.DataItem, m_objTable.Columns(intItem).ColumnName, "{0:dd.MM.yyyy}")
                End If
            End If
        Next
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(0)
    End Sub

    'Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Response.Redirect(GetUrlReferrerForCmdBack().ToString())
    'End Sub

    'Private Function StripQueryStringFromUrl(ByVal pUrl As String) As String
    '    Return Regex.Replace(pUrl, "\?.*$", String.Empty)
    'End Function

    'Private Function GetUrlReferrerForCmdBack() As System.Uri
    '    Dim aUri As System.Uri
    '    Dim aString As String

    '    aUri = Request.UrlReferrer
    '    aString = Request.QueryString("legende")
    '    If aString = "AppVFS-ADR" Or aString = "AppVFS-KZL" Then
    '        aUri = New Uri(lblHidden.Text)
    '    End If
    '    Return aUri
    'End Function
    
    Private Sub BuildTextForLabelHead()

        'Wenn die Anzeige aus AppVFS Kennzeichenbestand aufgerufen wurde,
        'soll nicht der Friendly Name angezeigt werden.
        ucStyles.TitleText = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        If (legende = "AppVFS-ADR") Then
            lblHead.Text = "Adressen"
            lblPageTitle.Text = String.Empty
            ucStyles.TitleText &= " - " & lblHead.Text
        ElseIf (legende = "AppVFS-KZL") Then
            lblHead.Text = "Kennzeichenliste"
            lblPageTitle.Text = String.Empty
            ucStyles.TitleText &= " - " & lblHead.Text
        Else
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        End If
    End Sub
    
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub doSubmit()

        Dim item As DataGridItem
        Dim cell As TableCell
        Dim label As label
        Dim txtBox As TextBox
        Dim control As control
        Dim tmpRows() As DataRow
        Dim sFahrgest As String = ""
        Dim sHnummer As String = ""
        Dim sLizNr As String = ""
        Dim sFinart As String = ""
        Dim sLabel As String = ""
        Dim sStatus As String = ""

        m_objTable.Columns.Add("Haendlernummer", System.Type.GetType("System.String"))
        m_objTable.Columns.Add("Lizenznummer", System.Type.GetType("System.String"))
        m_objTable.Columns.Add("Finanzierungsart", System.Type.GetType("System.String"))
        m_objTable.Columns.Add("Label", System.Type.GetType("System.String"))


        For Each item In DataGrid1.Items                    'Zeilen des DataGrids durchgehen...
            cell = item.Cells(1) 'Erste Spalte holen = CheckBox (nicht sichtbar)
            For Each control In cell.Controls
                Dim control1 = TryCast(control, Label)
                If (control1 IsNot Nothing) Then
                    label = control1
                    sFahrgest = label.Text ' Fahrgestellnummer holen
                End If
            Next
            cell = item.Cells(6)
            For Each control In cell.Controls
                Dim textBox = TryCast(control, TextBox)
                If (textBox IsNot Nothing) Then
                    txtBox = textBox
                    sHnummer = txtBox.Text.Trim ' Händlernummer holen
                    txtBox.Visible = False
                End If
                Dim control1 = TryCast(control, Label)
                If (control1 IsNot Nothing) Then
                    label = control1
                    label.Text = sHnummer
                    label.Visible = True
                End If
            Next
            cell = item.Cells(7)
            For Each control In cell.Controls
                Dim textBox = TryCast(control, TextBox)
                If (textBox IsNot Nothing) Then
                    txtBox = textBox
                    sLizNr = txtBox.Text.Trim ' Lizenznummer holen
                    txtBox.Visible = False
                End If
                Dim control1 = TryCast(control, Label)
                If (control1 IsNot Nothing) Then
                    label = control1
                    label.Text = sLizNr
                    label.Visible = True
                End If
            Next
            cell = item.Cells(8)
            For Each control In cell.Controls
                Dim textBox = TryCast(control, TextBox)
                If (textBox IsNot Nothing) Then
                    txtBox = textBox
                    sFinart = txtBox.Text.Trim ' Finanzierungsart holen
                    txtBox.Visible = False
                End If
                Dim control1 = TryCast(control, Label)
                If (control1 IsNot Nothing) Then
                    label = control1
                    label.Text = sFinart
                    label.Visible = True
                End If
            Next
            cell = item.Cells(9)
            For Each control In cell.Controls
                Dim textBox = TryCast(control, TextBox)
                If (textBox IsNot Nothing) Then
                    txtBox = textBox
                    sLabel = txtBox.Text.Trim ' Label holen
                    txtBox.Visible = False
                End If
                Dim control1 = TryCast(control, Label)
                If (control1 IsNot Nothing) Then
                    label = control1
                    label.Text = sLabel
                    label.Visible = True
                End If
            Next
            tmpRows = m_objTable.Select("Fahrgestellnummer='" & sFahrgest & "'")
            tmpRows(0).BeginEdit()
            tmpRows(0).Item("Haendlernummer") = sHnummer
            tmpRows(0).Item("Lizenznummer") = sLizNr
            tmpRows(0).Item("Finanzierungsart") = sFinart
            tmpRows(0).Item("Label") = sLabel
            tmpRows(0).EndEdit()
            m_objTable.AcceptChanges()
        Next
        Session("ResultTable") = m_objTable

        '*** SAP Update
        'Dim StatusTable As DataTable
        m_report = New fin_04(m_User, m_App, "")
        m_report.Change(m_objTable, sStatus, Session("AppID").ToString, Session.SessionID)
        '***
        If sStatus.Length > 0 Then
            lblError.Text = sStatus
        End If

        FillGrid(0)
        DataGrid1.Columns(6).Visible = False
        DataGrid1.Columns(7).Visible = False
        DataGrid1.Columns(8).Visible = False
        DataGrid1.Columns(9).Visible = False
        DataGrid1.Columns(10).Visible = True
        cmdsave.Visible = False
    End Sub

    Private Sub lb_Back_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_Back.Click
        Session("ResultTable") = Nothing
        Response.Redirect("Change52.aspx?AppID=" & Session("AppID").ToString)
    End Sub
End Class
' ************************************************
' $History: Change52_02.aspx.vb $
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 23.06.09   Time: 10:37
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2918 Z_M_Daten_Anlage
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
' *****************  Version 2  *****************
' User: Rudolpho     Date: 26.02.08   Time: 17:24
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA: 1733
' 
' ************************************************
