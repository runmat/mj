Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Change300_3
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
    'Private objHaendler As SixtLease_02

    Private tblResult As DataTable

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Private versandart As String
    Protected WithEvents lnkKreditlimit As System.Web.UI.WebControls.HyperLink
    Protected WithEvents Linkbutton1 As System.Web.UI.WebControls.LinkButton
    Private upload As Boolean

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        lnkKreditlimit.NavigateUrl = "Change300_2.aspx?AppID=" & Session("AppID").ToString
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            'If Session("objHaendler") Is Nothing Then
            '    Response.Redirect("Change205.aspx?AppID=" & Session("AppID").ToString)
            'End If

            'tblResult = CType(Session("ResultTable"), DataTable)

            If Not IsPostBack Then
                ddlPageSize.Items.Add("10")
                ddlPageSize.Items.Add("20")
                ddlPageSize.Items.Add("50")
                ddlPageSize.Items.Add("100")
                ddlPageSize.Items.Add("250")
                ddlPageSize.SelectedIndex = 2

                FillGrid(0, , True)
            Else
                CheckGrid()
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim objSixtLease As SixtLease_08
        Dim row As DataRow
        Dim status As String = ""

        If Not CheckGrid() Then
            lblError.Text = "Bitte Eingaben prüfen."
            Exit Sub
        End If

        cmdSave.Enabled = False

        objSixtLease = CType(Session("objHaendler"), SixtLease_08)
        tblResult = CType(Session("ResultTable"), DataTable)

        For Each row In tblResult.Rows
            If (row("Auswahl").ToString = "X") Then
                objSixtLease.Absenden(status, row("Equipment").ToString, row("Leasingvertragsnummer").ToString, Page)
                row("Status") = status
            End If
        Next
        Session("ResultTable") = tblResult
        FillGrid(0)

        objSixtLease.sendMail(tblResult, status)
        If (status <> String.Empty) Then
            lblError.Text = status
        End If
        lnkKreditlimit.Enabled = False
    End Sub

    Private Function CheckGrid() As Boolean
        Dim item As DataGridItem
        Dim cell As TableCell
        Dim txtBox As TextBox
        Dim control As Control
        Dim intReturn As Boolean
        Dim tmpRows As DataRow()

        tblResult = CType(Session("ResultTable"), DataTable)
        intReturn = True
        For Each item In DataGrid1.Items

            Dim strZZFAHRG As String = ""
            For Each cell In item.Cells

                strZZFAHRG = "Equipment = '" & item.Cells(0).Text & "'"

                For Each control In cell.Controls
                    If TypeOf control Is TextBox Then
                        txtBox = CType(control, TextBox)

                        tmpRows = tblResult.Select(strZZFAHRG)
                        If (tmpRows.Length > 0) Then
                            tmpRows(0).Item("Auswahl") = String.Empty
                            If txtBox.ID = "txtLVNR" Then
                                tmpRows(0).BeginEdit()
                                'Eingabe prüfen..
                                If (txtBox.Text.Trim <> String.Empty) AndAlso (Not IsNumeric(txtBox.Text)) Then
                                    tmpRows(0).Item("Status") = "Eingabe nicht numerisch."
                                    intReturn = False
                                Else
                                    tmpRows(0).Item("Auswahl") = "X"
                                    tmpRows(0).Item("Status") = String.Empty
                                End If
                                tmpRows(0).Item("Leasingvertragsnummer") = txtBox.Text.Trim
                                tmpRows(0).EndEdit()
                                tblResult.AcceptChanges()
                            End If
                        End If
                    End If
                Next
            Next
        Next
        Session("ResultTable") = tblResult
        Return intReturn
    End Function

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "", Optional ByVal fill As Boolean = False)
        Dim tmpDataView As New DataView()

        lblNoData.Text = "Bitte bestätigen Sie Ihre Eingaben durch Klick auf 'Absenden'!"
        lblNoData.Visible = True

        tblResult = CType(Session("ResultTable"), DataTable)
        tmpDataView = tblResult.DefaultView
        tmpDataView.RowFilter = "Auswahl='X'"

        If tmpDataView.Count = 0 Then
            DataGrid1.Visible = False
            ddlPageSize.Visible = False
            'lblNoData.Text = "Keine Daten zur Anzeige gefunden."
            'lblNoData.Visible = True
            'ShowScript.Visible = False
        Else
            ddlPageSize.Visible = True
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

            'lblNoData.Text = "Es wurden " & tmpDataView.Count.ToString & " KFZ-Briefe gefunden."
            'lblNoData.Visible = True

            If DataGrid1.PageCount > 1 Then
                DataGrid1.PagerStyle.CssClass = "PagerStyle"
                DataGrid1.DataBind()
                DataGrid1.PagerStyle.Visible = True
            Else
                DataGrid1.PagerStyle.Visible = False
            End If

            'Eingabefelder wieder füllen...

            Dim item As DataGridItem
            Dim cell As TableCell
            Dim txtBox As TextBox
            Dim control As Control
            Dim strZZFAHRG As String
            Dim tmpRows As DataRow()

            For Each item In DataGrid1.Items

                item.Cells(0).Enabled = False
                item.Cells(1).Enabled = False
                item.Cells(2).Enabled = False
                item.Cells(3).Enabled = False
                item.Cells(4).Enabled = False

                For Each cell In item.Cells

                    strZZFAHRG = "Equipment = '" & item.Cells(0).Text & "'"

                    For Each control In cell.Controls
                        If TypeOf control Is TextBox Then
                            txtBox = CType(control, TextBox)
                            If txtBox.ID = "txtLVNR" Then
                                tmpRows = tblResult.Select(strZZFAHRG)
                                If tmpRows.Length > 0 Then
                                    txtBox.Text = tmpRows(0)("Leasingvertragsnummer").ToString
                                    If Not (TypeOf tmpRows(0)("Status") Is System.DBNull) AndAlso (tmpRows(0)("Status") <> String.Empty) Then
                                        txtBox.CssClass = "InputBoxFlatError"
                                    Else
                                        txtBox.CssClass = "InputBoxFlat"
                                    End If
                                End If
                            End If
                        End If
                    Next
                Next
            Next
        End If
    End Sub

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(0)
    End Sub

    Private Sub Linkbutton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'calZul.Visible = True
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        'CheckGrid()
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        'CheckGrid()
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub Linkbutton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Linkbutton1.Click
        Response.Redirect("Change300_2.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Change300_3.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 4.05.09    Time: 10:47
' Updated in $/CKAG/Applications/appsixtl/Forms
' ITA:2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 17:27
' Created in $/CKAG/Applications/appsixtl/Forms
' 
' *****************  Version 6  *****************
' User: Uha          Date: 2.07.07    Time: 14:17
' Updated in $/CKG/Applications/AppSIXTL/AppSIXTLWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 5  *****************
' User: Uha          Date: 8.03.07    Time: 13:42
' Updated in $/CKG/Applications/AppSIXTL/AppSIXTLWeb/Forms
' 
' ************************************************
