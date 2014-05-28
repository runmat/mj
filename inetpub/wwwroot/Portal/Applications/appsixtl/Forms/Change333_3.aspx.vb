Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Change333_3
    Inherits System.Web.UI.Page

#Region " Vom Web Form Designer generierter Code "

    'Dieser Aufruf ist f�r den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: Diese Methode ist f�r den Web Form-Designer erforderlich
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
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Private versandart As String
    Protected WithEvents lnkKreditlimit As System.Web.UI.WebControls.HyperLink
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Private upload As Boolean

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        lnkKreditlimit.NavigateUrl = "Change333_2.aspx?AppID=" & Session("AppID").ToString
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

    Private Function CheckGrid() As Boolean
        Dim item As DataGridItem
        Dim cell As TableCell
        Dim cbx As CheckBox
        Dim control As Control
        Dim intReturn As Boolean
        Dim tmpRows As DataRow()
        Dim intRows As Integer

        tblResult = CType(Session("ResultTable"), DataTable)
        intRows = 0
        intReturn = True

        For Each item In DataGrid1.Items

            Dim strZZFAHRG As String = ""
            strZZFAHRG = "Fahrgestellnummer = '" & item.Cells(4).Text & "'"

            For Each cell In item.Cells
                For Each control In cell.Controls
                    If TypeOf control Is CheckBox Then
                        cbx = CType(control, CheckBox)

                        tmpRows = tblResult.Select(strZZFAHRG)
                        If (tmpRows.Length > 0) Then
                            tmpRows(0).Item("Auswahl") = String.Empty
                            If cbx.ID = "cbxFreigabe" Then
                                tmpRows(0).BeginEdit()
                                'Eingabe pr�fen..
                                If (cbx.Checked) Then
                                    tmpRows(0).Item("Auswahl") = "X"
                                    intRows += 1
                                End If
                                'If (cbx.Text.Trim <> String.Empty) AndAlso (Not IsNumeric(txtBox.Text)) Then
                                '    tmpRows(0).Item("Status") = "Eingabe nicht numerisch."
                                '    intReturn = False
                                'Else
                                '    If (txtBox.Text.Trim <> String.Empty) Then
                                '        tmpRows(0).Item("Auswahl") = "X"
                                '        tmpRows(0).Item("Status") = String.Empty
                                '        intRows += 1
                                '    End If
                                'End If
                                'tmpRows(0).Item("Leasingvertragsnummer") = txtBox.Text.Trim
                                tmpRows(0).EndEdit()
                                tblResult.AcceptChanges()
                            End If
                        End If
                    End If
                Next
            Next
        Next
        Session("ResultTable") = tblResult
        If (intRows = 0) Then
            intReturn = False
        End If
        Return intReturn
    End Function

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "", Optional ByVal fill As Boolean = False)
        Dim tmpDataView As New DataView()

        'CheckGrid()
        tblResult = CType(Session("ResultTable"), DataTable)

        tmpDataView = tblResult.DefaultView
        tmpDataView.RowFilter = "Auswahl='X'"

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

            lblNoData.Text = "Sie haben nachfolgende(n) " & tmpDataView.Count.ToString & " KFZ-Brief(e) zur Abmeldung ausgew�hlt:"
            lblNoData.Visible = True

            If DataGrid1.PageCount > 1 Then
                DataGrid1.PagerStyle.CssClass = "PagerStyle"
                DataGrid1.DataBind()
                DataGrid1.PagerStyle.Visible = True
            Else
                DataGrid1.PagerStyle.Visible = False
            End If

            'Eingabefelder wieder f�llen...

            Dim item As DataGridItem
            Dim cell As TableCell
            Dim cbx As CheckBox
            Dim control As Control
            Dim strZZFAHRG As String
            Dim tmpRows As DataRow()

            For Each item In DataGrid1.Items
                For Each cell In item.Cells
                    item.Enabled = False
                    strZZFAHRG = "Fahrgestellnummer = '" & item.Cells(3).Text & "'"

                    For Each control In cell.Controls
                        If TypeOf control Is CheckBox Then
                            cbx = CType(control, CheckBox)
                            If cbx.ID = "cbxFreigabe" Then
                                tmpRows = tblResult.Select(strZZFAHRG)
                                If tmpRows.Length > 0 Then
                                    cbx.Enabled = True
                                    If Not (TypeOf tmpRows(0)("Auswahl") Is System.DBNull) AndAlso (tmpRows(0)("Auswahl") <> String.Empty) Then
                                        cbx.Checked = True
                                    Else
                                        cbx.Checked = False
                                    End If
                                Else
                                    cbx.Enabled = False
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

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim objSixtLease As SixtLease_10

        lnkKreditlimit.NavigateUrl = String.Empty
        lnkKreditlimit.Enabled = False

        If Not CheckGrid() Then
            lblError.Text = "Bitte Eingaben pr�fen."
            Exit Sub
        End If

        cmdSave.Enabled = False

        objSixtLease = CType(Session("objHaendler"), SixtLease_10)
        tblResult = CType(Session("ResultTable"), DataTable)

        objSixtLease.Absenden(tblResult, Page)

        Session("ResultTable") = tblResult
        FillGrid(0)

    End Sub

    Private Sub DataGrid1_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGrid1.ItemDataBound
        e.Item.Cells(0).Visible = False
        'e.Item.Cells(e.Item.Cells.Count - 2).Visible = False
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Change333_3.aspx.vb $
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
' *****************  Version 8  *****************
' User: Uha          Date: 2.07.07    Time: 14:17
' Updated in $/CKG/Applications/AppSIXTL/AppSIXTLWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingef�hrt
' 
' *****************  Version 7  *****************
' User: Uha          Date: 4.06.07    Time: 16:57
' Updated in $/CKG/Applications/AppSIXTL/AppSIXTLWeb/Forms
' Change333 aktualisiert (Layout auf SGW war neuer!!!)
' 
' *****************  Version 6  *****************
' User: Uha          Date: 8.03.07    Time: 13:42
' Updated in $/CKG/Applications/AppSIXTL/AppSIXTLWeb/Forms
' 
' ************************************************
