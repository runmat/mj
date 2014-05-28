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
    Private objHaendler As ALD_2

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
    Protected WithEvents ddlSachbearbeiter As System.Web.UI.WebControls.DropDownList

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        lnkKreditlimit.NavigateUrl = "Change81.aspx?AppID=" & Session("AppID").ToString

        GetAppIDFromQueryString(Me)
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            If m_User.Organization.OrganizationAdmin Then
                lblHead.Text = Replace(lblHead.Text, " (Beauftragung)", "")
            End If
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If Session("objHaendler") Is Nothing Then
                Response.Redirect("Change81.aspx?AppID=" & Session("AppID").ToString)
            End If

            objHaendler = CType(Session("objHaendler"), ALD_2)

            If Not IsPostBack Then
                ddlPageSize.Items.Add("10")
                ddlPageSize.Items.Add("20")
                ddlPageSize.Items.Add("50")
                ddlPageSize.Items.Add("100")
                ddlPageSize.Items.Add("250")
                ddlPageSize.SelectedIndex = 2

                DataGrid1.BackColor = System.Drawing.Color.FromArgb(155, 255, 255)
                'DataGrid1.BackColor = System.Drawing.Color.LightSkyBlue

                'objHaendler.KUNNR = Right("0000000000" & m_User.KUNNR, 10)
                If objHaendler.Sachbearbeiter Is Nothing Then
                    lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & objHaendler.Message & ")"
                Else
                    ddlSachbearbeiter.DataSource = objHaendler.Sachbearbeiter
                    ddlSachbearbeiter.DataValueField = "SORTL"
                    ddlSachbearbeiter.DataTextField = "NAME1"
                    ddlSachbearbeiter.DataBind()
                    ddlSachbearbeiter.Items.Add("")
                    ddlSachbearbeiter.SelectedIndex = ddlSachbearbeiter.Items.Count - 1
                    FillGrid(0, , True)
                End If
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        CheckGrid()

        Dim tmpDataView As New DataView()
        tmpDataView = objHaendler.Fahrzeuge.DefaultView

        tmpDataView.RowFilter = "MANDT = '99'"
        Dim intFahrzeugBriefe As Int32 = tmpDataView.Count
        tmpDataView.RowFilter = ""

        If intFahrzeugBriefe = 0 Then
            lblError.Text = "Bitte wählen Sie erst Fahrzeuge zur Anforderung aus."
            FillGrid(DataGrid1.CurrentPageIndex)
        Else
            If PruefeSachbearbeiter() Then
                Session("objHaendler") = objHaendler
                Response.Redirect("Change81_4.aspx?AppID=" & Session("AppID").ToString)
            Else
                lblError.Text = "Bitte geben Sie für Ihre Auswahl jeweils eine gültige Sachbearbeiter-Nummer an."
                FillGrid(DataGrid1.CurrentPageIndex)
            End If
        End If
    End Sub

    Private Function CheckGrid() As Int32
        Dim item As DataGridItem
        Dim cell As TableCell
        Dim chbox As CheckBox
        Dim textbox As TextBox
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
                        If chbox.ID = "chk0000" Then
                            tmpRows = objHaendler.Fahrzeuge.Select(strZZFAHRG)
                            If (tmpRows.Length > 0) Then
                                tmpRows(0).BeginEdit()
                                If chbox.Checked Then           'anfordern
                                    tmpRows(0).Item("MANDT") = "99"
                                    intReturn += 1
                                Else
                                    tmpRows(0).Item("MANDT") = ""
                                End If
                                tmpRows(0).EndEdit()
                                objHaendler.Fahrzeuge.AcceptChanges()
                            End If
                        End If
                    End If

                    If TypeOf control Is TextBox Then
                        textbox = CType(control, TextBox)
                        If textbox.ID = "txtSachbearbeiter" Then
                            tmpRows = objHaendler.Fahrzeuge.Select(strZZFAHRG)
                            If (tmpRows.Length > 0) Then
                                tmpRows(0).BeginEdit()
                                tmpRows(0).Item("SACHBEARBEITER") = textbox.Text
                                tmpRows(0).EndEdit()
                                objHaendler.Fahrzeuge.AcceptChanges()
                            End If
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

            lblNoData.Text = "Es wurde(n) " & tmpDataView.Count.ToString & " KFZ-Brief(e) gefunden."
            lblNoData.Visible = True

            If DataGrid1.PageCount > 1 Then
                DataGrid1.PagerStyle.CssClass = "PagerStyle"
                DataGrid1.DataBind()
                DataGrid1.PagerStyle.Visible = True
            Else
                DataGrid1.PagerStyle.Visible = False
            End If


            If objHaendler.Fahrzeuge.Select("NOT STATUS=''").GetUpperBound(0) > -1 Then
                DataGrid1.Columns(DataGrid1.Columns.Count - 2).Visible = True
            Else
                DataGrid1.Columns(DataGrid1.Columns.Count - 2).Visible = False
            End If
        End If
    End Sub

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        CheckGrid()
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

    Private Function PruefeSachbearbeiter() As Boolean
        Dim blnReturn As Boolean = True
        Dim tmpRow As DataRow

        'Hier Prüfung gegen SAP-Ergebnisse
        objHaendler.Fahrzeuge.AcceptChanges()
        For Each tmpRow In objHaendler.Fahrzeuge.Rows
            Dim tmpRows() As DataRow = objHaendler.Sachbearbeiter.Select("SORTL='" & CStr(tmpRow("SACHBEARBEITER")) & "'")
            Dim strTemp As String = ""
            If TypeOf tmpRow("STATUS") Is System.String Then
                strTemp = CStr(tmpRow("STATUS"))
                strTemp = Replace(strTemp, " Sachbearb.-Nr. falsch", "")
            End If
            If tmpRows.Length = 0 Then
                If CStr(tmpRow("MANDT")) = "99" Then
                    tmpRow("STATUS") = strTemp & " Sachbearb.-Nr. falsch"
                    tmpRow("SB_CHECK") = False
                    blnReturn = False
                Else
                    tmpRow("SB_CHECK") = True
                End If
            Else
                tmpRow("SB_CHECK") = True
            End If
        Next
        objHaendler.Fahrzeuge.AcceptChanges()
        Session("objHaendler") = objHaendler

        Return blnReturn
    End Function

    Private Sub ddlSachbearbeiter_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSachbearbeiter.SelectedIndexChanged
        Dim tmpRow As DataRow

        CheckGrid()
        objHaendler.Fahrzeuge.AcceptChanges()
        For Each tmpRow In objHaendler.Fahrzeuge.Rows
            tmpRow("SACHBEARBEITER") = ddlSachbearbeiter.SelectedItem.Value
        Next
        objHaendler.Fahrzeuge.AcceptChanges()
        Session("objHaendler") = objHaendler

        FillGrid(0)
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
' *****************  Version 2  *****************
' User: Rudolpho     Date: 29.04.09   Time: 14:58
' Updated in $/CKAG/Applications/appALD/Forms
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 17.04.08   Time: 15:15
' Created in $/CKAG/Applications/appald/Forms
' 
' *****************  Version 8  *****************
' User: Uha          Date: 20.06.07   Time: 15:25
' Updated in $/CKG/Applications/AppALD/AppALDWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 7  *****************
' User: Uha          Date: 5.03.07    Time: 16:59
' Updated in $/CKG/Applications/AppALD/AppALDWeb/Forms
' 
' ************************************************
