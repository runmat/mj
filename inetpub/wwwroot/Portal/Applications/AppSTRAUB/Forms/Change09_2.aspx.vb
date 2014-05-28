Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Change09_2
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

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    'Private objSuche As Search
    Private objHaendler As Straub_09c

    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ShowScript As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkKreditlimit As System.Web.UI.WebControls.HyperLink
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        lnkKreditlimit.NavigateUrl = "Change09.aspx?AppID=" & Session("AppID").ToString
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If Session("objHaendler") Is Nothing Then
                Response.Redirect("Change09.aspx?AppID=" & Session("AppID").ToString)
            End If

            objHaendler = CType(Session("objHaendler"), Straub_09c)

            If Not IsPostBack Then
                ddlPageSize.Items.Add("10")
                ddlPageSize.Items.Add("20")
                ddlPageSize.Items.Add("50")
                ddlPageSize.Items.Add("100")
                ddlPageSize.Items.Add("200")
                ddlPageSize.Items.Add("500")
                ddlPageSize.Items.Add("1000")
                ddlPageSize.SelectedIndex = 2

                FillGrid(0)
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        CheckGrid()

        Dim tmpDataView As New DataView()
        tmpDataView = objHaendler.Fahrzeuge.DefaultView

        tmpDataView.RowFilter = "MANDT <> '0'"
        Dim intFahrzeugBriefe As Int32 = tmpDataView.Count
        tmpDataView.RowFilter = ""

        If intFahrzeugBriefe = 0 Then
            lblError.Text = "Bitte wählen Sie erst Fahrzeuge zur Anforderung aus."
            'kopfdaten1.Message = "Bitte wählen Sie erst Fahrzeuge zur Anforderung aus."
            FillGrid(DataGrid1.CurrentPageIndex)
        Else
            Session("objHaendler") = objHaendler
            Response.Redirect("Change09_3.aspx?AppID=" & Session("AppID").ToString)
        End If
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        CheckGrid()
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        CheckGrid()
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Function CheckGrid() As Int32
        Dim item As DataGridItem
        Dim cell As TableCell
        Dim chbox As CheckBox
        Dim txtBox As TextBox
        Dim control As Control
        Dim blnScriptFound As Boolean = False
        Dim intZaehl As Int32
        Dim intReturn As Int32 = 0
        Dim tmpRows As DataRow()
        Dim tmpRow As DataRow

        For Each item In DataGrid1.Items
            intZaehl = 1
            Dim strZZFAHRG As String = ""
            For Each cell In item.Cells
                If intZaehl = 1 Then
                    strZZFAHRG = "EQUNR = '" & cell.Text & "'"
                End If
                For Each control In cell.Controls
                    If TypeOf control Is CheckBox Then
                        chbox = CType(control, CheckBox)
                        objHaendler.Fahrzeuge.AcceptChanges()
                        'Dim tmpRows As DataRow()
                        'Dim tmpRow As DataRow
                        tmpRows = objHaendler.Fahrzeuge.Select(strZZFAHRG)
                        tmpRows(0).BeginEdit()
                        Select Case chbox.ID
                            Case "chkNichtAnfordern"
                                If chbox.Checked Then
                                    tmpRows(0).Item("MANDT") = "0"
                                End If
                            Case "chk0001"
                                If chbox.Checked Then
                                    tmpRows(0).Item("MANDT") = "1"
                                    intReturn += 1
                                End If
                            Case "chk0002"
                                If chbox.Checked Then
                                    tmpRows(0).Item("MANDT") = "2"
                                    intReturn += 1
                                End If
                        End Select
                        tmpRows(0).EndEdit()
                        objHaendler.Fahrzeuge.AcceptChanges()
                    End If
                    'Positionstexte lesen
                    If TypeOf control Is TextBox Then
                        txtBox = CType(control, TextBox)
                        tmpRows = objHaendler.Fahrzeuge.Select(strZZFAHRG)
                        tmpRows(0).BeginEdit()
                        'tmpRows(0).Item("TEXT200") = txtBox.Text
                        tmpRows(0).Item("POSITIONSTEXT") = txtBox.Text
                    End If
                Next
                intZaehl += 1
            Next
        Next
        Session("objHaendler") = objHaendler
        Return intReturn
    End Function

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")

        Dim tmpDataView As New DataView()
        tmpDataView = objHaendler.Fahrzeuge.DefaultView
        tmpDataView.RowFilter = ""

        If tmpDataView.Count = 0 Then
            DataGrid1.Visible = False
            ddlPageSize.Visible = False
            lblNoData.Text = "Keine Daten zur Anzeige gefunden."
            lblNoData.Visible = True
            ShowScript.Visible = False
        Else
            ddlPageSize.Visible = True
            Dim intTempPageIndex As Int32 = intPageIndex
            Dim strTempSort As String = ""
            Dim strDirection As String

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

            If objHaendler.SucheEquiTyp = "B" Then
                lblNoData.Text = "Es wurde(n) " & tmpDataView.Count.ToString & " nicht angeforderte(r) Brief(e) gefunden."
            Else
                lblNoData.Text = "Es wurde(n) " & tmpDataView.Count.ToString & " nicht angeforderte Schlüsseltüte(n) gefunden."
            End If

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
            Dim chkBox As CheckBox
            Dim control As Control
            Dim blnScriptFound As Boolean = False
            Dim intZaehl As Int32
            Dim row As DataRow
            Dim strKKB As String = "0"

            For Each item In DataGrid1.Items
                'intZaehl = 1
                row = objHaendler.Fahrzeuge.Select("EQUNR='" & item.Cells(0).Text & "'")(0)
                strKKB = CStr(row("MANDT"))

                For Each cell In item.Cells
                    For Each control In cell.Controls
                        If TypeOf control Is CheckBox Then
                            chkBox = CType(control, CheckBox)
                            Select Case chkBox.ID
                                Case "chkNichtAnfordern"
                                    If strKKB = "0" Then
                                        chkBox.Checked = True
                                    End If
                                Case "chk0001"
                                    If strKKB = "1" Then
                                        chkBox.Checked = True
                                    End If
                                Case "chk0002"
                                    If strKKB = "2" Then
                                        chkBox.Checked = True
                                    End If
                            End Select
                        End If
                    Next
                    'intZaehl += 1
                Next
            Next
        End If
    End Sub

    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        DoSubmit()
    End Sub

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(0)
    End Sub

    Private Sub LinkButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'txtKopf.Visible = Not txtKopf.Visible
        'If txtKopf.Visible = False Then
        '    txtKopf.Text = String.Empty
        'End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Change09_2.aspx.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 17:38
' Created in $/CKAG/Applications/AppSTRAUB/Forms
' 
' *****************  Version 4  *****************
' User: Uha          Date: 2.07.07    Time: 14:28
' Updated in $/CKG/Applications/AppSTRAUB/AppSTRAUBWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 3  *****************
' User: Uha          Date: 8.03.07    Time: 14:18
' Updated in $/CKG/Applications/AppSTRAUB/AppSTRAUBWeb/Forms
' 
' ************************************************
