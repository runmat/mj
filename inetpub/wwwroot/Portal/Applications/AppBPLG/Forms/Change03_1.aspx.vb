Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Change03_1
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
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    'Private legende As String
    'Private csv As String
    'Private schmal As String
    Protected WithEvents lb_Back As System.Web.UI.WebControls.LinkButton
    Dim mObjVertragsdatenaenderung As Vertragsdatenaenderung

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


        If (Session("ObjVertragsdatenaenderungSession") Is Nothing) Then
            Response.Redirect(Request.UrlReferrer.ToString)
        Else
            If mObjVertragsdatenaenderung Is Nothing Then
                mObjVertragsdatenaenderung = CType(Session("ObjVertragsdatenaenderungSession"), Vertragsdatenaenderung)
            End If
        End If

        Try
            If Not IsPostBack Then
                mObjVertragsdatenaenderung.ResultTable = mObjVertragsdatenaenderung.Result.Copy
                mObjVertragsdatenaenderung.ResultTable.Columns.Add("Status", System.Type.GetType("System.String"))
                mObjVertragsdatenaenderung.ResultTable.AcceptChanges()
                If m_objTable Is Nothing Then
                    m_objTable = mObjVertragsdatenaenderung.ResultTable
                End If
                FillGrid(0)
            Else
                If m_objTable Is Nothing Then
                    m_objTable = mObjVertragsdatenaenderung.ResultTable
                End If
            End If


        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdsave.Click
        doSubmit()
    End Sub

    'Private Sub DataGrid1_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
    '    FillGrid(e.NewPageIndex)
    'End Sub

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
            
        End If
    End Sub

    Private Sub DataGrid1_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGrid1.DataBinding

    End Sub

    Private Sub DataGrid1_ItemDataBound1(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGrid1.ItemDataBound
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

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub doSubmit()

        If mObjVertragsdatenaenderung Is Nothing Then
            mObjVertragsdatenaenderung = CType(Session("ObjVertragsdatenaenderungSession"), Vertragsdatenaenderung)
        End If

        If m_objTable Is Nothing Then
            m_objTable = mObjVertragsdatenaenderung.ResultTable
        End If
        Dim item As DataGridItem
        Dim txtBox As TextBox
        Dim TmpCmb As DropDownList
        Dim tmpRows() As DataRow
        Dim sEQUNR As String
        Dim sHnummer As String
        Dim sLizNr As String
        Dim sEndKundenNR As String
        Dim sLabel As String

        For Each item In DataGrid1.Items

            sEQUNR = item.Cells(0).Text

            txtBox = CType(item.FindControl("txt_HaendlerNR"), TextBox)

            sHnummer = txtBox.Text.Trim

            txtBox = CType(item.FindControl("txt_Lizenznr"), TextBox)
            sLizNr = txtBox.Text.Trim

            txtBox = CType(item.FindControl("txt_EndkundenNummer"), TextBox)
            sEndKundenNR = txtBox.Text.Trim
            
            TmpCmb = CType(item.FindControl("cmbBrandings"), DropDownList)
            sLabel = TmpCmb.SelectedValue


            If sLabel = "0" OrElse sLizNr.Trim(" "c) = "" OrElse sEndKundenNR.Trim(" "c) = "" OrElse sHnummer.Trim(" "c) = "" Then
                lblError.Text = "Füllen Sie bitte alle Pflichtfelder"
                lblError.Visible = True
                Exit Sub
            End If

            tmpRows = m_objTable.Select("EQUNR='" & sEQUNR & "'")
            tmpRows(0).BeginEdit()
            tmpRows(0).Item("HaendlerNR") = sHnummer
            tmpRows(0).Item("Lizenznr") = sLizNr
            tmpRows(0).Item("EndkundenNummer") = sEndKundenNR
            If Not sLabel Is String.Empty AndAlso Not sLabel = "" Then
                tmpRows(0).Item("Branding") = sLabel
            End If
            tmpRows(0).EndEdit()
            m_objTable.AcceptChanges()
        Next
        Session("ResultTable") = m_objTable

        mObjVertragsdatenaenderung.Change(m_objTable)

        If Not mObjVertragsdatenaenderung.Status = 0 Then
            lblError.Text = mObjVertragsdatenaenderung.Message
        End If

        FillGrid(0)
       
        DataGrid1.Columns(DataGrid1.Columns.Count - 1).Visible = True
        cmdsave.Visible = False
    End Sub

    Private Sub lb_Back_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_Back.Click
        Session("ResultTable") = Nothing
        Response.Redirect("Change03.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub DataGrid1_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGrid1.ItemDataBound

        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim TmpCmb As DropDownList
            TmpCmb = CType(e.Item.FindControl("cmbBrandings"), DropDownList)
            If Not TmpCmb Is Nothing Then
                If mObjVertragsdatenaenderung Is Nothing Then
                    mObjVertragsdatenaenderung = CType(Session("ObjVertragsdatenaenderungSession"), Vertragsdatenaenderung)
                End If
                TmpCmb.DataSource = mObjVertragsdatenaenderung.Brandings
                TmpCmb.DataTextField = "CMBText"
                TmpCmb.DataValueField = "ZZLABEL"
                TmpCmb.DataBind()
                Dim tmpItem As New ListItem("-Keine Auswahl-", "0")
                TmpCmb.Items.Insert(0, tmpItem)
                TmpCmb.SelectedValue = mObjVertragsdatenaenderung.Result.Select("EQUNR='" & e.Item.Cells(0).Text & "'")(0)("Branding").ToString

            End If
        End If
    End Sub

End Class
' ************************************************
' $History: Change03_1.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 29.04.09   Time: 16:09
' Updated in $/CKAG/Applications/AppBPLG/Forms
' Warnungen
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 29.07.08   Time: 14:29
' Updated in $/CKAG/Applications/AppBPLG/Forms
' BPLG Test Nachbesserungen
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 23.07.08   Time: 9:57
' Updated in $/CKAG/Applications/AppBPLG/Forms
' ITA 2101 testfertig
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 22.07.08   Time: 14:32
' Created in $/CKAG/Applications/AppBPLG/Forms
' ITA 2101 Rohversion
' ************************************************
