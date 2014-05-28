Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common


Public Class Report201_2
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

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents Hyperlink2 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ShowScript As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents lblDownloadTip As System.Web.UI.WebControls.Label
    Protected WithEvents lnkExcel As System.Web.UI.WebControls.HyperLink
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblInfo As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lnkKreditlimit As System.Web.UI.WebControls.HyperLink
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents Literal1 As System.Web.UI.WebControls.Literal
    Private legende As String


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cmdSave.Enabled = False
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        '############### 
        legende = Request.QueryString.Item("legende")
        If (legende = "Ja") Then
            lblInfo.Text = "<br>*(V)ollmacht, (H)andelsregistereintrag, (P)ersonalausweis, (G)ewerbeanmeldung, (E)inzugsermächtigung"
        End If
        If (legende = "Report201") Then
            lblInfo.Text = "<br>'X' = vorhanden"
        End If
        If (legende = "Report203") Then
            lblInfo.Text = "<br>*(V)ollmacht, (H)andelsregistereintrag, (P)ersonalausweis, (G)ewerbeanmeldung, (E)inzugsermächtigung, 'X' = vorhanden"
        End If
        '################
        If (Session("ResultTable") Is Nothing) Then
            Response.Redirect(Request.UrlReferrer.ToString)
        Else
            m_objTable = CType(Session("ResultTable"), DataTable)
        End If
        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text
        Try
            m_App = New Base.Kernel.Security.App(m_User)

            'Dim u As Int32
            'For u = 0 To m_objTable.Columns.Count - 1
            '    Dim nC As New BoundColumn()
            '    nC.DataField = m_objTable.Columns(u).ColumnName
            '    nC.SortExpression = m_objTable.Columns(u).ColumnName
            '    nC.HeaderText = m_objTable.Columns(u).ExtendedProperties("HeadLine").ToString
            '    Select Case m_objTable.Columns(u).ExtendedProperties("Alignment")
            '        Case "Right"
            '            nC.ItemStyle.HorizontalAlign = HorizontalAlign.Right
            '        Case "Center"
            '            nC.ItemStyle.HorizontalAlign = HorizontalAlign.Center
            '        Case Else
            '            nC.ItemStyle.HorizontalAlign = HorizontalAlign.Left
            '    End Select
            '    DataGrid1.Columns.Add(nC)
            'Next

            If Not IsPostBack Then
                ddlPageSize.Items.Add("10")
                ddlPageSize.Items.Add("20")
                ddlPageSize.Items.Add("50")
                ddlPageSize.Items.Add("100")
                ddlPageSize.Items.Add("200")
                ddlPageSize.Items.Add("500")
                ddlPageSize.Items.Add("1000")
                ddlPageSize.SelectedIndex = 2

                'If (Not Session("ShowLink") Is Nothing) AndAlso Session("ShowLink").ToString = "True" Then
                '    lnkKreditlimit.Visible = True
                '    lnkKreditlimit.NavigateUrl = Request.UrlReferrer.ToString
                'End If
                If Not Session("lnkExcel").ToString.Length = 0 Then
                    lblDownloadTip.Visible = True
                    lnkExcel.Visible = True
                    lnkExcel.NavigateUrl = Session("lnkExcel").ToString
                End If
                FillGrid(0)
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        '##
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
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

            'Dim k As Int32
            'Dim l As Int32
            'For l = 0 To DataGrid1.Items.Count - 1
            '    For k = 0 To m_objTable.Columns.Count - 1
            '        Select Case m_objTable.Columns(k).ExtendedProperties("Alignment")
            '            Case "Right"
            '                DataGrid1.Items(l).Cells(k).HorizontalAlign = HorizontalAlign.Right
            '            Case "Center"
            '                DataGrid1.Items(l).Cells(k).HorizontalAlign = HorizontalAlign.Center
            '            Case Else
            '                DataGrid1.Items(l).Cells(k).HorizontalAlign = HorizontalAlign.Left
            '        End Select
            '    Next
            'Next

            If (Not Session("ShowOtherString") Is Nothing) AndAlso CStr(Session("ShowOtherString")).Length > 0 Then
                lblNoData.Text = CStr(Session("ShowOtherString"))
            Else
                lblNoData.Text = "Es wurden " & tmpDataView.Count.ToString & " Einträge zu """ & m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString & """ gefunden."
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
    End Sub

    'Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
    '    DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
    '    FillGrid(0)
    'End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(0)
    End Sub

    Private Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.ItemCommand
        If Not e.CommandArgument Is Nothing AndAlso CStr(e.CommandArgument).Trim.Length > 0 Then
            Dim strTarget As String = ""
            If e.CommandName = "Schilder" Then
                strTarget = "3"
            ElseIf e.CommandName = "Schein" Then
                strTarget = "4"
            End If
            If strTarget.Length > 0 Then
                Literal1.Text = "			<SCRIPT language=""JavaScript"">" & vbCrLf
                Literal1.Text &= "			<!-- //" & vbCrLf
                Literal1.Text &= "                          window.open(""Report201_" & strTarget & ".aspx?strKennzeichen=" & CStr(e.CommandArgument).Trim & """, ""_blank"", ""left=0,top=0,scrollbars=YES,menubar=YES,toolbar=YES,resizable=YES"");" & vbCrLf
                Literal1.Text &= "			//-->" & vbCrLf
                Literal1.Text &= "			</SCRIPT>" & vbCrLf
            End If
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
' $History: Report201_2.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 30.04.09   Time: 10:57
' Updated in $/CKAG/Applications/appecan/Forms
' ITA 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:07
' Created in $/CKAG/Applications/appecan/Forms
' 
' *****************  Version 4  *****************
' User: Uha          Date: 2.07.07    Time: 12:32
' Updated in $/CKG/Applications/AppECAN/AppECANWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 3  *****************
' User: Uha          Date: 7.03.07    Time: 11:16
' Updated in $/CKG/Applications/AppECAN/AppECANWeb/Forms
' 
' ************************************************
