Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Change04
    Inherits System.Web.UI.Page
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ucHeader As Header
    Protected WithEvents cmdBack As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ucStyles As Styles


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

    Private tblResult As DataTable

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            Dim m_Report As New Change_04(m_User, m_App)

            If Not IsPostBack Then
                ddlPageSize.Items.Add("10")
                ddlPageSize.Items.Add("20")
                ddlPageSize.Items.Add("50")
                ddlPageSize.Items.Add("100")
                ddlPageSize.Items.Add("250")
                ddlPageSize.SelectedIndex = 2

                m_Report.FILL(Session("AppID").ToString, Session.SessionID.ToString)

                If Not (m_Report.ModellTable Is Nothing) AndAlso (m_Report.ModellTable.Rows.Count > 0) Then
                    Session("objSuche") = m_Report
                    FillGrid(0, , True)
                Else
                    cmdSave.Visible = False
                    ddlPageSize.Visible = False
                    lblError.Text = "Keine Daten gefunden."
                End If
            Else
                CheckGrid()
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub


    Private Function CheckGrid() As Boolean
        Dim item As DataGridItem
        Dim m_Report As Change_04
        Dim cell As TableCell
        Dim txtBox As TextBox
        Dim control As Control
        Dim intReturn As Boolean
        Dim tmpRows As DataRow()
        Dim intRows As Integer
        Dim strError As String

        m_Report = CType(Session("objSuche"), Change_04)
        tblResult = m_Report.ModellTable

        'tblResult = CType(Session("ResultTable"), DataTable)
        intRows = 0
        intReturn = True

        For Each item In DataGrid1.Items

            Dim strEqui As String = ""
            strError = String.Empty
            For Each cell In item.Cells

                strEqui = "EQUNR = '" & item.Cells(0).Text & "'"

                For Each control In cell.Controls
                    If TypeOf control Is TextBox Then
                        txtBox = CType(control, TextBox)

                        tmpRows = tblResult.Select(strEqui)
                        If (tmpRows.Length > 0) Then
                            If txtBox.ID = "txtModell" Then
                                If cmdSave.Visible = False Then
                                    txtBox.Text = tmpRows(0).Item("ZZMODELL")
                                    txtBox.Enabled = False
                                Else
                                    If (txtBox.Text.Trim <> String.Empty) Then
                                        tmpRows(0).BeginEdit()
                                        tmpRows(0).Item("ZZMODELL") = txtBox.Text.Trim
                                        tmpRows(0).EndEdit()
                                        tblResult.AcceptChanges()
                                    End If
                                End If
                            End If

                            '§§§ JVE 23.08.2006: Model-Code wird auch erfasst...

                            If txtBox.ID = "txtModelcode" Then
                                If cmdSave.Visible = False Then
                                    txtBox.Text = tmpRows(0).Item("Modellcode")
                                    txtBox.Enabled = False
                                Else
                                    If (txtBox.Text.Trim <> String.Empty) Then
                                        tmpRows(0).BeginEdit()
                                        tmpRows(0).Item("Modellcode") = txtBox.Text.Trim
                                        tmpRows(0).EndEdit()
                                        tblResult.AcceptChanges()
                                    End If
                                End If
                            End If
                        End If
                    End If
                Next
            Next
        Next
        Session("objSuche") = m_Report
        If (intRows = 0) Then
            intReturn = False
        End If
        Return intReturn
    End Function

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "", Optional ByVal fill As Boolean = False)
        Dim tmpDataView As New DataView()
        Dim m_Report As Change_04
        Dim item As DataGridItem
        Dim cell As TableCell
        Dim txtBox As TextBox
        Dim control As Control
        Dim intReturn As Boolean
        Dim tmpRows As DataRow()
        Dim intRows As Integer
        Dim strError As String

        m_Report = CType(Session("objSuche"), Change_04)

        tblResult = m_Report.ModellTable

        tmpDataView = tblResult.DefaultView
        tmpDataView.RowFilter = ""

        If tmpDataView.Count = 0 Then
            DataGrid1.Visible = False
            ddlPageSize.Visible = False
            lblNoData.Text = "Keine Daten zur Anzeige gefunden."
            lblNoData.Visible = True
            'ShowScript.Visible = False
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

            lblNoData.Text = "Es wurden " & tmpDataView.Count.ToString & " Datensätze gefunden."
            lblNoData.Visible = True

            If DataGrid1.PageCount > 1 Then
                DataGrid1.PagerStyle.CssClass = "PagerStyle"
                DataGrid1.DataBind()
                DataGrid1.PagerStyle.Visible = True
            Else
                DataGrid1.PagerStyle.Visible = False
            End If

            For Each item In DataGrid1.Items

                Dim strEqui As String = ""
                strError = String.Empty
                For Each cell In item.Cells

                    strEqui = "EQUNR = '" & item.Cells(0).Text & "'"

                    For Each control In cell.Controls
                        If TypeOf control Is TextBox Then
                            txtBox = CType(control, TextBox)

                            tmpRows = tblResult.Select(strEqui)
                            If (tmpRows.Length > 0) Then
                                If txtBox.ID = "txtModell" Then
                                    txtBox.Text = tmpRows(0).Item("ZZMODELL")
                                    'txtBox.Enabled = False
                                End If

                                If txtBox.ID = "txtModelcode" Then
                                    If Not (TypeOf tmpRows(0).Item("Modellcode") Is System.DBNull) Then
                                        txtBox.Text = tmpRows(0).Item("Modellcode")
                                        'txtBox.Enabled = False
                                    End If
                                End If
                            End If
                        End If
                    Next
                Next
            Next

        End If
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim m_Report As Change_04

        cmdSave.Enabled = False

        m_Report = CType(Session("objSuche"), Change_04)
        m_Report.SetModell(Session("AppID").ToString, Session.SessionID.ToString, tblResult)

        FillGrid(0)
    End Sub

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(0)
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBack.Click
        Response.Redirect("/Portal/Start/Selection.aspx")
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Change04.aspx.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 17:38
' Created in $/CKAG/Applications/AppSTRAUB/Forms
' 
' *****************  Version 10  *****************
' User: Uha          Date: 2.07.07    Time: 14:28
' Updated in $/CKG/Applications/AppSTRAUB/AppSTRAUBWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 9  *****************
' User: Uha          Date: 8.03.07    Time: 14:18
' Updated in $/CKG/Applications/AppSTRAUB/AppSTRAUBWeb/Forms
' 
' ************************************************
