Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Change05_2
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
    Private objHaendler As FDD_Haendler
    Private m_BooLaufzeitFehler As Boolean

    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lnkKreditlimit As System.Web.UI.WebControls.HyperLink
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ShowScript As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents Kopfdaten1 As Kopfdaten
    Protected WithEvents ucHeader As Header
    Protected WithEvents calZul As System.Web.UI.WebControls.Calendar
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents Linkbutton2 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents txtZulDatum As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        Kopfdaten1.Message = ""
        lnkKreditlimit.NavigateUrl = "Change05.aspx?AppID=" & Session("AppID").ToString

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If Session("objHaendler") Is Nothing Then
                Response.Redirect("Change05.aspx?AppID=" & Session("AppID").ToString)
            End If

            If Session("objSuche") Is Nothing Then
                Response.Redirect("Change05.aspx?AppID=" & Session("AppID").ToString)
            Else
                objSuche = CType(Session("objSuche"), Search)
            End If

            Kopfdaten1.UserReferenz = m_User.Reference
            Kopfdaten1.HaendlerNummer = objSuche.REFERENZ
            Dim strTemp As String = objSuche.NAME
            If objSuche.NAME_2.Length > 0 Then
                strTemp &= "<br>" & objSuche.NAME_2
            End If
            Kopfdaten1.HaendlerName = strTemp
            Kopfdaten1.Adresse = objSuche.COUNTRYISO & " - " & objSuche.POSTL_CODE & " " & objSuche.CITY & "<br>" & objSuche.STREET

            Session("objSuche") = objSuche

            objHaendler = CType(Session("objHaendler"), FDD_Haendler)

            If Not IsPostBack Then
                ddlPageSize.Items.Add("10")
                ddlPageSize.Items.Add("20")
                'ddlPageSize.Items.Add("50")
                'ddlPageSize.Items.Add("100")
                'ddlPageSize.Items.Add("200")
                'ddlPageSize.Items.Add("500")
                'ddlPageSize.Items.Add("1000")
                ddlPageSize.SelectedIndex = 1

                Kopfdaten1.Kontingente = objHaendler.Kontingente
                FillGrid(0)
            Else
                CheckGrid()
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim errMessage As String = String.Empty

        If objHaendler.isHEZ Then       'HEZ
            objHaendler.checkInputHEZ(txtZulDatum.Text, errMessage)
        End If
        If (errMessage <> String.Empty) Then
            Kopfdaten1.MessageError = errMessage
            txtZulDatum.Text = String.Empty
        Else
            DoSubmit()
        End If
    End Sub

    Private Sub DoSubmit()
        If m_BooLaufzeitFehler = True Then Exit Sub

        Dim tmpDataView As New DataView()
        tmpDataView = objHaendler.Fahrzeuge.DefaultView

        tmpDataView.RowFilter = "MANDT <> '0'"
        Dim intFahrzeugBriefe As Int32 = tmpDataView.Count
        tmpDataView.RowFilter = ""

        If intFahrzeugBriefe = 0 Then
            Kopfdaten1.MessageError = "Bitte wählen Sie erst Fahrzeuge zum Zulassen aus."
            FillGrid(DataGrid1.CurrentPageIndex)
        Else
            Session("objHaendler") = objHaendler
            objHaendler.zuldatum = CType(txtZulDatum.Text, Date) 'für HEZ Zulassungsdatum setzen!
            Response.Redirect("Change05_3.aspx?AppID=" & Session("AppID").ToString)
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
        Dim control As Control
        Dim blnScriptFound As Boolean = False
        Dim intZaehl As Int32
        Dim intReturn As Int32 = 0
        Dim txtBox As TextBox

        lblError.Text = ""
        m_BooLaufzeitFehler = False

        For Each item In DataGrid1.Items
            intZaehl = 1
            Dim strZZFAHRG As String = ""

            txtBox = CType(item.FindControl("txtHaltefrist"), TextBox)

            For Each cell In item.Cells
                If intZaehl = 1 Then
                    strZZFAHRG = "ZZFAHRG = '" & cell.Text & "'"
                End If
                For Each control In cell.Controls

                    objHaendler.Fahrzeuge.AcceptChanges()
                    Dim tmpRows As DataRow()
                    tmpRows = objHaendler.Fahrzeuge.Select(strZZFAHRG)
                    tmpRows(0).BeginEdit()

                    If TypeOf control Is CheckBox Then
                        chbox = CType(control, CheckBox)
                        'objHaendler.Fahrzeuge.AcceptChanges()
                        'Dim tmpRows As DataRow()
                        'Dim tmpRow As DataRow
                        'tmpRows = objHaendler.Fahrzeuge.Select(strZZFAHRG)
                        'tmpRows(0).BeginEdit()

                        Select Case chbox.ID
                            Case "chk0001"                              'Vermittler-Fzg. ausgewählt
                                If chbox.Checked Then
                                    tmpRows(0).Item("MANDT") = "2"      'Werte 2,3,4 nicht beliebig! SAP erwartet sie genau so.
                                    intReturn += 1
                                    txtBox.Text = ""
                                    txtBox.Enabled = False
                                    txtBox.Text = ""
                                    txtBox.CssClass = "LaufzeitInaktiv"
                                    tmpRows(0).Item("ZZLAUFZEIT") = ""
                                End If
                            Case "chk0002"
                                If chbox.Checked Then
                                    tmpRows(0).Item("MANDT") = "4"      'Vorführ-Fzg. ausgewählt
                                    intReturn += 1
                                    txtBox.Enabled = True
                                    txtBox.CssClass = String.Empty

                                    If Len(txtBox.Text) > 0 Then
                                        If CheckInputLaufzeitError(txtBox.Text) = True Then Exit Function
 
                                        tmpRows(0).Item("ZZLAUFZEIT") = txtBox.Text

                                    End If

                                End If
                            Case "chk0003"
                                If chbox.Checked Then
                                    tmpRows(0).Item("MANDT") = "3"      'Neu-Fzg. ausgewählt
                                    intReturn += 1
                                    txtBox.Enabled = True
                                    txtBox.CssClass = String.Empty

                                    If Len(txtBox.Text) > 0 Then
                                        If CheckInputLaufzeitError(txtBox.Text) = True Then Exit Function

                                        tmpRows(0).Item("ZZLAUFZEIT") = txtBox.Text

                                    End If
                                End If
                            Case "chkNichtAnfordern"
                                If chbox.Checked Then
                                    tmpRows(0).Item("MANDT") = "0"      'Neu-Fzg. ausgewählt
                                    intReturn += 1
                                    txtBox.Text = ""
                                    txtBox.Enabled = False
                                    txtBox.Text = ""
                                    txtBox.CssClass = "LaufzeitInaktiv"
                                    tmpRows(0).Item("ZZLAUFZEIT") = ""
                                End If
                        End Select
                        'tmpRows(0).EndEdit()
                        'objHaendler.Fahrzeuge.AcceptChanges()
                        'ElseIf TypeOf control Is TextBox Then

                        '    txtBox = CType(control, TextBox)
                        '    If txtBox.ID = "txtHaltefrist" Then

                        '        If tmpRows(0).Item("MANDT") = "3" Then
                        '            txtBox.Enabled = True
                        '            If Len(txtBox.Text) > 0 Then
                        '                tmpRows(0).Item("ZZLAUFZEIT") = txtBox.Text
                        '            End If
                        '        Else
                        '            tmpRows(0).Item("ZZLAUFZEIT") = ""
                        '        End If
                        '    End If

                    End If

                    tmpRows(0).EndEdit()
                    objHaendler.Fahrzeuge.AcceptChanges()


                Next
                intZaehl += 1
            Next
        Next
        Session("objHaendler") = objHaendler
        Return intReturn
    End Function


    Private Function CheckInputLaufzeitError(ByVal Laufzeit As String) As Boolean

        If IsNumeric(Laufzeit) = False Then
            lblError.Text = "Bitte geben Sie für die Mindesthaltefrist eine Zahl zwischen 1 und 365 ein."
            m_BooLaufzeitFehler = True
            Return m_BooLaufzeitFehler
        ElseIf ((CInt(Laufzeit) < 0) Or (CInt(Laufzeit) > 366)) Then
            lblError.Text = "Bitte geben Sie für die Mindesthaltefrist eine Zahl zwischen 1 und 365 ein."
            m_BooLaufzeitFehler = True
            Return m_BooLaufzeitFehler
        End If

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

            lblNoData.Text = "Es wurden " & tmpDataView.Count.ToString & " Zulassungsbereite Fahrzeuge gefunden."
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

            For Each item In DataGrid1.Items
                intZaehl = 1
                Dim mandt As String
                Dim strZZFAHRG As String = ""

                For Each cell In item.Cells
                    If intZaehl = 1 Then
                        strZZFAHRG = "ZZFAHRG = '" & cell.Text & "'"
                    End If
                    For Each control In cell.Controls
                        If TypeOf control Is CheckBox Then
                            chkBox = CType(control, CheckBox)
                            Dim tmpRows As DataRow()
                            tmpRows = objHaendler.Fahrzeuge.Select(strZZFAHRG)
                            mandt = tmpRows(0)("MANDT").ToString

                            Select Case chkBox.ID
                                Case "chk0001"
                                    If mandt = "2" Then
                                        chkBox.Checked = True
                                    End If
                                Case "chk0002"
                                    If mandt = "4" Then
                                        chkBox.Checked = True
                                    End If
                                Case "chk0003"
                                    If mandt = "3" Then
                                        chkBox.Checked = True
                                    End If
                                Case "chkNichtAnfordern"
                                    If mandt = "0" Then
                                        chkBox.Checked = True
                                    End If
                            End Select
                        End If
                    Next
                    intZaehl += 1
                Next
            Next
        End If
    End Sub

    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        DoSubmit()
    End Sub

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(0)
    End Sub


    Private Sub calZul_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calZul.SelectionChanged
        txtZulDatum.Text = calZul.SelectedDate.ToShortDateString
        calZul.Visible = False
    End Sub

    Private Sub Linkbutton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Linkbutton2.Click
        calZul.Visible = True
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Change05_2.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 30.04.09   Time: 14:43
' Updated in $/CKAG/Applications/appffd/Forms
' ITA: 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:23
' Created in $/CKAG/Applications/appffd/Forms
' 
' *****************  Version 9  *****************
' User: Uha          Date: 2.07.07    Time: 12:52
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 8  *****************
' User: Uha          Date: 7.03.07    Time: 12:51
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' 
' ************************************************
