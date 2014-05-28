Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Change81_2
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
    Private objSuche As Search
    Private objHaendler As SonstDL

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lnkKreditlimit As System.Web.UI.WebControls.HyperLink
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        lnkKreditlimit.NavigateUrl = "Change81.aspx?AppID=" & Session("AppID").ToString

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        If m_User.Organization.OrganizationAdmin Then
            lblHead.Text = Replace(lblHead.Text, " (Beauftragung)", "")
        End If
        ucStyles.TitleText = lblHead.Text
        m_App = New Base.Kernel.Security.App(m_User)

        If Session("objHaendler") Is Nothing Then
            Response.Redirect("Change81.aspx?AppID=" & Session("AppID").ToString)
        End If

        objHaendler = CType(Session("objHaendler"), SonstDL)

        If Not IsPostBack Then
            ddlPageSize.Items.Add("10")
            ddlPageSize.Items.Add("20")
            ddlPageSize.Items.Add("50")
            ddlPageSize.Items.Add("100")
            ddlPageSize.Items.Add("250")
            ddlPageSize.SelectedIndex = 2

            DataGrid1.BackColor = System.Drawing.Color.DarkSeaGreen
            FillGrid(0, , True)
        End If

    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        CheckGrid()

        Dim tmpDataView As New DataView()
        tmpDataView = objHaendler.Fahrzeuge.DefaultView

        tmpDataView.RowFilter = "MANDT = '99'"
        Dim intFahrzeugBriefe As Int32 = tmpDataView.Count
        tmpDataView.RowFilter = ""

        'IT 1241: Mehrere Fahrzeuge möglich
        If intFahrzeugBriefe = 0 Then
            lblError.Text = "Bitte wählen Sie mindestens ein Fahrzeug zur Beauftragung aus."
            FillGrid(DataGrid1.CurrentPageIndex)
        Else
            Session("objHaendler") = objHaendler
            Response.Redirect("Change81_3.aspx?AppID=" & Session("AppID").ToString)
        End If
    End Sub

    Private Function CheckGrid() As Int32
        Dim item As DataGridItem
        Dim cell As TableCell
        Dim chbox As CheckBox
        Dim control As Control
        Dim blnScriptFound As Boolean = False
        Dim intReturn As Int32 = 0
        Dim tmpRows As DataRow()

        For Each item In DataGrid1.Items
            Dim strZZFAHRG As String = ""
            strZZFAHRG = "EQUNR = '" & item.Cells(0).Text & "'"

            For Each cell In item.Cells
                For Each control In cell.Controls
                    If TypeOf control Is CheckBox Then
                        chbox = CType(control, CheckBox)

                        tmpRows = objHaendler.Fahrzeuge.Select(strZZFAHRG)
                        If (tmpRows.Length > 0) Then
                            tmpRows(0).BeginEdit()
                            If chbox.ID = "chk0000" Then
                                If chbox.Checked Then           'anfordern
                                    tmpRows(0).Item("MANDT") = "99"
                                    intReturn += 1
                                Else
                                    If Not CStr(tmpRows(0).Item("MANDT")) = "11" Then
                                        tmpRows(0).Item("MANDT") = ""
                                    End If
                                End If
                            End If
                            tmpRows(0).EndEdit()
                            objHaendler.Fahrzeuge.AcceptChanges()
                        End If
                    End If
                Next
            Next
        Next
        Session("objHaendler") = objHaendler
        Return intReturn
    End Function

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "", Optional ByVal fill As Boolean = False)
        Dim tmpDataView As New DataView()
        tmpDataView = objHaendler.Fahrzeuge.DefaultView
        tmpDataView.RowFilter = ""

        If tmpDataView.Count = 0 Then
            DataGrid1.Visible = False
            ddlPageSize.Visible = False
            lblNoData.Text = "Keine Daten zur Anzeige gefunden."
            lblNoData.Visible = True
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

            lblNoData.Text = "Es wurde(n) " & tmpDataView.Count.ToString & " KFZ-Brief(e) gefunden."
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
            Dim hyperlink As HyperLink
            Dim control As Control


            Dim strHistoryLink As String = ""
            If m_User.Applications.Select("AppName = 'Report46'").Length > 0 Then
                strHistoryLink = "Report46.aspx?AppID=" & m_User.Applications.Select("AppName = 'Report46'")(0)("AppID").ToString & "&VIN="
            End If

            For Each item In DataGrid1.Items
                For Each cell In item.Cells
                    For Each control In cell.Controls
                        If TypeOf control Is HyperLink Then
                            hyperlink = CType(control, HyperLink)
                            Select Case hyperlink.ID
                                Case "VIN"
                                    If strHistoryLink.Length > 0 Then
                                        hyperlink.NavigateUrl = "../Finance/" & strHistoryLink & hyperlink.NavigateUrl
                                    Else
                                        hyperlink.NavigateUrl = ""
                                    End If
                            End Select
                        End If
                    Next
                Next
            Next

            If objHaendler.Fahrzeuge.Select("MANDT='11'").GetUpperBound(0) > -1 Then
                DataGrid1.Columns(DataGrid1.Columns.Count - 1).Visible = True
            Else
                DataGrid1.Columns(DataGrid1.Columns.Count - 1).Visible = False
            End If
        End If
    End Sub

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(0)
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        CheckGrid()
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        CheckGrid()
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Change81_2.aspx.vb $
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 5.06.09    Time: 15:58
' Updated in $/CKAG/Components/ComCommon/SonstigeDL
' mögliche try catches entfernt
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 4.05.09    Time: 11:50
' Updated in $/CKAG/Components/ComCommon/SonstigeDL
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 30.03.09   Time: 14:59
' Updated in $/CKAG/Components/ComCommon/SonstigeDL
' ITA 2760
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 10.10.08   Time: 15:35
' Updated in $/CKAG/Components/ComCommon/SonstigeDL
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 9.10.08    Time: 15:06
' Created in $/CKAG/Components/ComCommon/SonstigeDL
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 9.10.08    Time: 14:52
' Created in $/CKAG/ComCommon/SonstigeDL
' ************************************************
