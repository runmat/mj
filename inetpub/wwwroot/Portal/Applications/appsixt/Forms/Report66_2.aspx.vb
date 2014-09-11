Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common


Public Class Report66_2
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

    Protected WithEvents Hyperlink2 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents ShowScript As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents btnBack As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblInfo As System.Web.UI.WebControls.Label
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents lblDownloadTip As System.Web.UI.WebControls.Label
    Protected WithEvents lnkExcel As System.Web.UI.WebControls.HyperLink
    Protected WithEvents btnDelete As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Private legende As String


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'cmdSave.Enabled = False
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        ''############### 
        'legende = Request.QueryString.Item("legende")
        'If (legende = "Ja") Then
        '    lblInfo.Text = "<br>*(V)ollmacht, (H)andelsregistereintrag, (P)ersonalausweis, (G)ewerbeanmeldung, (E)inzugsermächtigung"
        'End If
        'If (legende = "Report201") Then
        '    lblInfo.Text = "<br>'X' = vorhanden"
        'End If
        'If (legende = "Report203") Then
        '    lblInfo.Text = "<br>*(V)ollmacht, (H)andelsregistereintrag, (P)ersonalausweis, (G)ewerbeanmeldung, (E)inzugsermächtigung, 'X' = vorhanden"
        'End If
        ''################
        If (Session("ResultTable") Is Nothing) Then
            Response.Redirect(Request.UrlReferrer.ToString)
        Else
            m_objTable = CType(Session("ResultTable"), DataTable)
        End If
        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text

        Try
            m_App = New Base.Kernel.Security.App(m_User)

            If Not IsPostBack Then
                ddlPageSize.Items.Add("10")
                ddlPageSize.Items.Add("20")
                ddlPageSize.Items.Add("50")
                ddlPageSize.Items.Add("100")
                ddlPageSize.Items.Add("200")
                ddlPageSize.Items.Add("500")
                ddlPageSize.Items.Add("1000")
                ddlPageSize.SelectedIndex = 2

                If (Not Session("lnkExcel") Is Nothing) AndAlso (Not Session("lnkExcel").ToString.Length = 0) Then
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

    Private Sub checkGrid()
        Dim row As DataRow
        Dim item As DataGridItem
        Dim cbxDelete As CheckBox

        For Each item In DataGrid1.Items
            cbxDelete = CType(item.FindControl("cbxDelete"), CheckBox)
            Try
                row = m_objTable.Select("Kennzeichen='" & item.Cells(1).Text & "'")(0)
                If cbxDelete.Checked Then
                    row("Delete") = True
                Else
                    row("Delete") = False
                End If
            Catch ex As Exception
                'Fahrgestellnummer nicht in Tabelle gefunden
            End Try
        Next
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If m_objTable.Rows.Count = 0 Then
            DataGrid1.Visible = False
            lblNoData.Visible = True
            lblNoData.Text = "Keine Daten zur Anzeige gefunden."
            ShowScript.Visible = False
        Else
            checkGrid()

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

            Dim row As DataRow
            Dim item As DataGridItem
            Dim cbxDelete As CheckBox

            For Each item In DataGrid1.Items
                Try
                    cbxDelete = CType(item.FindControl("cbxDelete"), CheckBox)
                    cbxDelete.Checked = False
                    row = m_objTable.Select("Kennzeichen='" & item.Cells(1).Text & "'")(0)
                    If CType(row("Delete"), Boolean) = True Then
                        cbxDelete.Checked = True
                    End If
                    If Not (TypeOf row("Status") Is System.DBNull) Then
                        If (CStr(row("Status")) = "Eintrag gelöscht.") Then
                            item.Enabled = False
                            cbxDelete.Enabled = False
                            item.Font.Bold = False
                        Else
                            item.Font.Bold = True
                        End If
                    End If
                Catch ex As Exception
                    'Fahrgestellnummer nicht in Tabelle gefunden
                End Try
            Next



            If (Not Session("ShowOtherString") Is Nothing) AndAlso CStr(Session("ShowOtherString")).Length > 0 Then
                lblNoData.Text = CStr(Session("ShowOtherString"))
            Else
                lblNoData.Text = "Es wurden " & tmpDataView.Count.ToString & " Einträge zu """ & m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString & """ gefunden."
            End If

            lblNoData.Visible = True


        End If
    End Sub

    'Private Sub DataGrid1_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)
    '    Dim intItem As Int32

    '    For intItem = 0 To m_objTable.Columns.Count - 1
    '        If m_objTable.Columns(intItem).DataType Is System.Type.GetType("System.DateTime") Then
    '            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
    '                e.Item.Cells(intItem).Text = DataBinder.Eval(e.Item.DataItem, m_objTable.Columns(intItem).ColumnName, "{0:dd.MM.yyyy}")
    '            End If
    '        End If
    '    Next

    '    Dim chkBox As CheckBox

    '    chkBox = New CheckBox()
    '    chkBox.ID = "chk" & e.Item.Cells(1).Text 'chk + Fahrgestellnr
    '    chkBox.Checked = False
    '    chkBox.Visible = True
    '    chkBox.Attributes.Add("onClick", "alert('" & chkBox.ID & "')")
    '    e.Item.Cells(10).Controls.Add(chkBox)
    'End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Response.Redirect("Report66.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(0)
    End Sub

    Private Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.ItemCommand
        'Dim kennzeichen As String
        'Dim fahrgestell As String
        'Dim brief As String
        'Dim schluess As String
        'Dim control As ImageButton
        'Dim control2 As Label


        'If (e.CommandName = "Delete") Then

        '    Dim m_Report As New Sixt_B66(m_User, m_App, "")

        '    kennzeichen = e.Item.Cells(1).Text
        '    fahrgestell = e.Item.Cells(2).Text

        '    brief = e.Item.Cells(7).Text
        '    schluess = e.Item.Cells(8).Text

        '    m_Report.Delete(kennzeichen, fahrgestell, brief, schluess)

        '    If m_Report.Status <> 0 Then
        '        lblError.Text = m_Report.Message
        '    Else
        '        'e.Item.BackColor = System.Drawing.Color.Gray
        '        e.Item.Enabled = False
        '        control = CType(e.Item.Cells(9).FindControl("btnDelete"), ImageButton)
        '        control.Visible = False
        '        control2 = CType(e.Item.Cells(9).FindControl("lblDelete"), Label)
        '        control2.Visible = True
        '    End If
        'End If
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Dim m_Report As New Sixt_B66(m_User, m_App, "")
        Dim row As DataRow
        Dim kennzeichen As String
        Dim fahrgestell As String
        Dim brief As String
        Dim schluess As String
        Dim strError As String
        Dim cbxDelete As CheckBox
        Dim item As DataGridItem
        Dim blnError As Boolean
        Dim intCounter As Integer

        checkGrid()
        DataGrid1.Columns(9).Visible = True

        blnError = False
        intCounter = 0
        For Each row In m_objTable.Rows
            Try
                kennzeichen = CStr(row("Kennzeichen"))
                fahrgestell = CStr(row("Fahrgestellnummer"))
                brief = CStr(row("Flag_Briefversand"))
                schluess = CStr(row("Flag_Schluesselversand"))

                If (Not (TypeOf row("Delete") Is System.DBNull) AndAlso (CBool(row("Delete")) = True)) Then
                    m_Report.Delete(Session("AppID").ToString, Session.SessionID.ToString, Me, kennzeichen, fahrgestell, brief, schluess)
                    intCounter = intCounter + 1
                    If (m_Report.Status <> 0) Then
                        strError = m_Report.Message
                        row("Status") = "Fehler! " & m_Report.Message
                        blnError = True
                    Else
                        strError = "Eintrag gelöscht."
                        row("Status") = strError
                        row("Delete") = False
                        For Each item In DataGrid1.Items
                            If item.Cells(1).Text = CStr(row("Kennzeichen")) Then
                                cbxDelete = CType(item.FindControl("cbxDelete"), CheckBox)
                                cbxDelete.Checked = False
                            End If
                        Next
                    End If
                End If
            Catch ex As Exception
                strError = "Fehler: Eintrag konnte nicht gelöscht werden!"
            End Try
        Next
        FillGrid(0)
        lblError.Text = String.Empty

        If (blnError = True) Then
            lblError.Text = "Hinwies: Es traten Fehler auf!"
        End If
        If intCounter = 0 Then
            lblError.Text = "Keine Datensätze zum löschen markiert."
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
' $History: Report66_2.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 6.01.11    Time: 15:26
' Updated in $/CKAG/Applications/appsixt/Forms
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 16.02.10   Time: 11:40
' Updated in $/CKAG/Applications/appsixt/Forms
' ITA: 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 4.05.09    Time: 10:16
' Updated in $/CKAG/Applications/appsixt/Forms
' ITA:2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 13:37
' Created in $/CKAG/Applications/appsixt/Forms
' 
' *****************  Version 7  *****************
' User: Uha          Date: 2.07.07    Time: 14:10
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 6  *****************
' User: Uha          Date: 8.03.07    Time: 13:15
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Forms
' 
' ************************************************
