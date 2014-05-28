Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common
Imports System.Text.RegularExpressions

Public Class Change01_1
    Inherits System.Web.UI.Page

    'Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private m_objTable As DataTable

    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents cmdsave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lbl_Info As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblHidden As System.Web.UI.WebControls.Label
    Protected WithEvents ShowScript As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Private legende As String
    'Private csv As String
    'Private schmal As String
    Protected WithEvents lb_Back As System.Web.UI.WebControls.LinkButton
    Dim mChange As Nacherfassung

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
        'm_App = New Base.Kernel.Security.App(m_User)


        If (Session("ResultTable") Is Nothing) Then
            Response.Redirect(Request.UrlReferrer.ToString)
        Else
            m_objTable = CType(Session("ResultTable"), DataTable)
        End If

        Try

            If Not IsPostBack Then

                If mChange Is Nothing Then
                    mChange = CType(Session("ObjNacherfassungSession"), Nacherfassung)
                End If
                Session("ObjNacherfassungSession") = mChange
                ddlPageSize.Items.Add("10")
                ddlPageSize.Items.Add("20")
                ddlPageSize.Items.Add("50")
                ddlPageSize.Items.Add("100")
                ddlPageSize.Items.Add("200")
                ddlPageSize.Items.Add("500")
                ddlPageSize.Items.Add("1000")
                ddlPageSize.SelectedIndex = 2

                m_objTable.Columns.Add("Status", System.Type.GetType("System.String"))
                FillGrid(0)
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdsave.Click
        doSubmit()
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
                    If TypeOf control Is label Then
                        label = CType(control, label)
                        If label.ID = "Label6" Then
                            If IsDate(label.Text) Then
                                datEingang = CDate(label.Text)
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


    'Private Sub BuildTextForLabelHead()

    '    'Wenn die Anzeige aus AppVFS Kennzeichenbestand aufgerufen wurde,
    '    'soll nicht der Friendly Name angezeigt werden.
    '    ucStyles.TitleText = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
    '    If (legende = "AppVFS-ADR") Then
    '        lblHead.Text = "Adressen"
    '        lblPageTitle.Text = String.Empty
    '        ucStyles.TitleText &= " - " & lblHead.Text
    '    ElseIf (legende = "AppVFS-KZL") Then
    '        lblHead.Text = "Kennzeichenliste"
    '        lblPageTitle.Text = String.Empty
    '        ucStyles.TitleText &= " - " & lblHead.Text
    '    Else
    '        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
    '    End If
    'End Sub


    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub doSubmit()

        If mChange Is Nothing Then
            mChange = CType(Session("ObjNacherfassungSession"), Nacherfassung)
        End If

        If m_objTable Is Nothing Then
            m_objTable = mChange.ResultTable
        End If
        Dim item As DataGridItem
        Dim txtBox As TextBox
        Dim TmpCmb As DropDownList
        Dim tmpRows() As DataRow
        Dim sEQUNR As String = ""
        Dim sHnummer As String = ""
        Dim sLizNr As String = ""
        Dim sEndKundenNR As String = ""
        Dim sLabel As String = ""
        For Each item In DataGrid1.Items

            sEQUNR = item.Cells(0).Text

          
            txtBox = CType(item.FindControl("txt_HaendlerNR"), TextBox)

            sHnummer = txtBox.Text.Trim

            txtBox = CType(item.FindControl("txt_Lizenznr"), TextBox)
            sLizNr = txtBox.Text.Trim

          
            txtBox = CType(item.FindControl("txt_EndkundenNummer"), TextBox)
            sEndKundenNR = txtBox.Text.Trim

           
            TmpCmb = CType(item.FindControl("cmbBrandings"), DropDownList)
            If TmpCmb.Visible = True Then
                sLabel = TmpCmb.SelectedValue
            End If

            If sLabel = "0" OrElse sLizNr Is String.Empty OrElse sEndKundenNR = "" OrElse sHnummer = "" Then
                lblError.Text = "Füllen Sie bitte alle Pflichtfelder"
                lblError.Visible = True
                Exit Sub
            End If

            tmpRows = m_objTable.Select("EQUNR='" & sEQUNR & "'")
            tmpRows(0).BeginEdit()
            tmpRows(0).Item("HaendlerNR") = sHnummer
            tmpRows(0).Item("Lizenznr") = sLizNr
            tmpRows(0).Item("EndkundenNummer") = sEndKundenNR
            If Not sLabel Is String.Empty AndAlso Not sLabel = "" Then 'eine leeres Label darf niemals eingetragen werden, da es sonst das alte überschreibt im sap JJU2008.07.17
                tmpRows(0).Item("Branding") = sLabel
            End If
            tmpRows(0).EndEdit()
            m_objTable.AcceptChanges()
        Next

        Session("ResultTable") = m_objTable

        mChange.Change(m_objTable)

        If Not mChange.Status = 0 Then
            lblError.Text = mChange.Message
        End If

        FillGrid(0)
        
        DataGrid1.Columns(DataGrid1.Columns.Count - 1).Visible = True
        cmdsave.Visible = False
    End Sub

    Private Sub lb_Back_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_Back.Click
        Session("ResultTable") = Nothing
        Response.Redirect("Change01.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub DataGrid1_ItemDataBound1(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGrid1.ItemDataBound
        Dim TmpCmb As DropDownList = CType(e.Item.FindControl("cmbBrandings"), DropDownList)

        If Not TmpCmb Is Nothing Then
            If mChange Is Nothing Then
                mChange = CType(Session("ObjNacherfassungSession"), Nacherfassung)
            End If
            TmpCmb.DataSource = mChange.Brandings
            TmpCmb.DataTextField = "CMBText"
            TmpCmb.DataValueField = "ZZLABEL"
            TmpCmb.DataBind()
            Dim tmpItem As New ListItem("-Keine Auswahl-", "0")
            TmpCmb.Items.Insert(0, tmpItem)
        End If
    End Sub
    
    Public Function getBranding(ByVal kuerzel As String) As String

        If Not kuerzel Is String.Empty AndAlso Not kuerzel = "" Then
            If mChange Is Nothing Then
                mChange = CType(Session("ObjNacherfassungSession"), Nacherfassung)
            End If
            Return mChange.Brandings.Select("ZZLABEL='" & kuerzel & "'")(0)("BRANDING").ToString
        End If
        Return ""
    End Function

    Private Sub DataGrid1_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Protected Sub ddlPageSize_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlPageSize.SelectedIndexChanged
        DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(0)
    End Sub

End Class
' ************************************************
' $History: Change01_1.aspx.vb $
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 29.04.09   Time: 16:09
' Updated in $/CKAG/Applications/AppBPLG/Forms
' Warnungen
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 21.08.08   Time: 8:22
' Updated in $/CKAG/Applications/AppBPLG/Forms
' ITA 2178 fertig
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 11.08.08   Time: 17:54
' Updated in $/CKAG/Applications/AppBPLG/Forms
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 18.07.08   Time: 10:36
' Updated in $/CKAG/Applications/AppBPLG/Forms
' ITA 2069 fertig
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 17.07.08   Time: 15:52
' Updated in $/CKAG/Applications/AppBPLG/Forms
' killAllDBNullValuesInDataTable methode hinzugefügt
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 11.07.08   Time: 12:33
' Created in $/CKAG/Applications/AppBPLG/Forms
' Erstellung ITA 2069
' 
' ************************************************
