Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Change07_3
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private objSuche As Search
    Private objHaendler As FDD_Haendler

    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkKreditlimit As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtKopf As System.Web.UI.WebControls.TextBox
    Protected WithEvents LinkButton1 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents ShowScript As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents kopfdaten1 As Kopfdaten
    Protected WithEvents ucHeader As Header
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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        kopfdaten1.Message = ""
        lnkKreditlimit.NavigateUrl = "Change04.aspx?AppID=" & Session("AppID").ToString

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If Session("objHaendler") Is Nothing Then
                Response.Redirect("Change07.aspx?AppID=" & Session("AppID").ToString)
            End If

            If Session("objSuche") Is Nothing Then
                Response.Redirect("Change07.aspx?AppID=" & Session("AppID").ToString)
            Else
                objSuche = CType(Session("objSuche"), Search)
            End If

            kopfdaten1.UserReferenz = m_User.Reference
            kopfdaten1.HaendlerNummer = objSuche.REFERENZ
            Dim strTemp As String = objSuche.NAME
            If objSuche.NAME_2.Length > 0 Then
                strTemp &= "<br>" & objSuche.NAME_2
            End If
            kopfdaten1.HaendlerName = strTemp
            kopfdaten1.Adresse = objSuche.COUNTRYISO & " - " & objSuche.POSTL_CODE & " " & objSuche.CITY & "<br>" & objSuche.STREET

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

                kopfdaten1.Kontingente = objHaendler.Kontingente
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
        Dim i As Integer
        i = CheckGrid()
        If i <> -99 Then
            Dim tmpDataView As New DataView()
            tmpDataView = objHaendler.Fahrzeuge.DefaultView

            tmpDataView.RowFilter = "MANDT <> '0'"
            Dim intFahrzeugBriefe As Int32 = tmpDataView.Count
            tmpDataView.RowFilter = ""

            If intFahrzeugBriefe = 0 Then
                kopfdaten1.Message = "Bitte wählen Sie erst Fahrzeuge zur Anforderung aus."
                FillGrid(DataGrid1.CurrentPageIndex)
            Else
                Session("objHaendler") = objHaendler
                Response.Redirect("Change07_4.aspx?AppID=" & Session("AppID").ToString)
            End If
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


        'Fahrzeugtabelle instanziert?
        CheckFahrzeuge()


        'Kopftext setzen
        If txtKopf.Text <> String.Empty Then
            For Each tmpRow In objHaendler.Fahrzeuge.Rows
                'tmpRow("TEXT50") = txtKopf.Text
                tmpRow("KOPFTEXT") = txtKopf.Text
            Next
        End If
        Try
            For Each item In DataGrid1.Items
                intZaehl = 1
                Dim strZZFAHRG As String = ""
                For Each cell In item.Cells
                    If intZaehl = 1 Then
                        strZZFAHRG = "ZZFAHRG = '" & cell.Text & "'"
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
                                Case "chk0003"
                                    If chbox.Checked Then
                                        tmpRows(0).Item("MANDT") = "3"
                                        intReturn += 1
                                    End If
                                Case "chk0004"
                                    If chbox.Checked Then
                                        tmpRows(0).Item("MANDT") = "4"
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
                            If txtBox.ID = "txtAnfragenr" Then
                                ' Zusammensetzen der Anfragenummer
                                If tmpRows(0).Item("MANDT") = "3" Then
                                    If txtBox.Text.Length = 13 And IsNumeric(txtBox.Text) Then
                                        Dim sTemp As String
                                        Dim sKunnr As String
                                        Dim sTemp1 As String
                                        Dim sTemp2 As String
                                        Dim sTemp3 As String
                                        Dim sTemp4 As String
                                        Dim sTemp5 As String
                                        sTemp = Left(txtBox.Text, 10)
                                        sKunnr = Left(sTemp, 5)
                                        sTemp1 = Right(sTemp, 5)
                                        sTemp2 = Left(sTemp1, 2)
                                        sTemp3 = Right(sTemp1, 3)
                                        sTemp4 = Right(txtBox.Text, 3)
                                        'sTemp = kopfdaten1.HaendlerNummer
                                        sTemp5 = sKunnr & "." & sTemp2 & "." & sTemp3 & "." & sTemp4
                                        tmpRows(0).Item("TEXT300") = sTemp5
                                    ElseIf txtBox.Text.Length >= 0 AndAlso tmpRows(0).Item("MANDT") = "3" Then
                                        Throw New Exception("Für das Feld ""Anfragenummer"" sind 13 numerische Zeichen erforderlich!")
                                    End If
                                End If
                            End If
                            tmpRows(0).EndEdit()
                            objHaendler.Fahrzeuge.AcceptChanges()
                        End If
                    Next
                    intZaehl += 1
                Next
            Next
            Session("objHaendler") = objHaendler
            Return intReturn
        Catch ex As Exception
            lblError.Text = ex.Message
            CheckGrid = -99
        End Try
    End Function

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")


        'Fahrzeugtabelle instanziert?
        CheckFahrzeuge()


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

            lblNoData.Text = "Es wurden " & tmpDataView.Count.ToString & " nicht angeforderte Briefe gefunden."
            lblNoData.Visible = True

            If DataGrid1.PageCount > 1 Then
                DataGrid1.PagerStyle.CssClass = "PagerStyle"
                DataGrid1.DataBind()
                DataGrid1.PagerStyle.Visible = True
            Else
                DataGrid1.PagerStyle.Visible = False
            End If
            '§§§JVE 21.06.2005 <begin>

            'If objHaendler.Kontingente.Rows.Count = 2 OrElse CInt(objHaendler.Kontingente.Rows(2)("Richtwert_Alt")) = 0 Then
            '    DataGrid1.Columns(DataGrid1.Columns.Count - 1).Visible = False
            'End If

            If objHaendler.Kontingente.Rows.Count = 2 Then  'Nur Standard-Kontingent->Spalte ausblenden
                DataGrid1.Columns(DataGrid1.Columns.Count - 2).Visible = False
            Else
                If objHaendler.Kontingente.Rows.Count > 2 Then  'Delayed Payment-Kontingent vorhanden?
                    If objHaendler.Kontingente.Rows(3)("Kreditkontrollbereich").ToString = "0004" Then  'Kann nur in Zeile 3 sein...
                        If CInt(objHaendler.Kontingente.Rows(3)("Richtwert_Alt")) = 0 Then 'Richtwert=0, ausblenden!
                            DataGrid1.Columns(DataGrid1.Columns.Count - 4).Visible = False
                        End If
                    Else
                        DataGrid1.Columns(DataGrid1.Columns.Count - 4).Visible = False  'Nein, dann sowieso ausblenden
                    End If
                    If objHaendler.Kontingente.Rows(2)("Kreditkontrollbereich").ToString = "0003" Then  'Kontingent = Retail
                        If CInt(objHaendler.Kontingente.Rows(2)("Richtwert_Alt")) = 0 Then 'Richtwert=0, ausblenden!
                            DataGrid1.Columns(DataGrid1.Columns.Count - 2).Visible = False
                            DataGrid1.Columns(DataGrid1.Columns.Count - 3).Visible = False
                        End If
                    Else
                        DataGrid1.Columns(DataGrid1.Columns.Count - 2).Visible = False
                        DataGrid1.Columns(DataGrid1.Columns.Count - 3).Visible = False
                    End If
                End If
            End If
            '§§§<end>

            Dim item As DataGridItem
            Dim cell As TableCell
            Dim chkBox As CheckBox
            Dim control As Control
            Dim blnScriptFound As Boolean = False
            Dim intZaehl As Int32

            For Each item In DataGrid1.Items
                intZaehl = 1
                Dim strKKB As String = "0"
                Dim blnBezahlt As Boolean

                For Each cell In item.Cells
                    For Each control In cell.Controls
                        If TypeOf control Is CheckBox Then
                            chkBox = CType(control, CheckBox)
                            If chkBox.ID = "chkBezahlt" Then
                                blnBezahlt = chkBox.Checked
                            End If
                        End If
                    Next
                Next

                For Each cell In item.Cells
                    If intZaehl = 2 Then
                        strKKB = cell.Text
                    End If
                    For Each control In cell.Controls
                        If TypeOf control Is CheckBox Then
                            chkBox = CType(control, CheckBox)
                            Select Case chkBox.ID
                                Case "chkNichtAnfordern"
                                    If strKKB = "0" Then
                                        chkBox.Checked = True
                                    End If
                                Case "chk0001"
                                    If blnBezahlt Then
                                        chkBox.Enabled = False
                                    Else
                                        If strKKB = "1" Then
                                            chkBox.Checked = True
                                        End If
                                    End If
                                Case "chk0002"
                                    If strKKB = "2" Then
                                        chkBox.Checked = True
                                    End If
                                Case "chk0003"
                                    If blnBezahlt Then
                                        chkBox.Enabled = False
                                    Else
                                        If strKKB = "3" Then
                                            chkBox.Checked = True
                                        End If
                                    End If
                                Case "chk0004"
                                    If blnBezahlt Then
                                        chkBox.Enabled = False
                                    Else
                                        If strKKB = "4" Then
                                            chkBox.Checked = True
                                        End If
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

    Private Sub LinkButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        txtKopf.Visible = Not txtKopf.Visible
        If txtKopf.Visible = False Then
            txtKopf.Text = String.Empty
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub CheckFahrzeuge()
        'Wenn die Tabelle Fahrzeuge Nothing ist, hat der User mehrmals die Navigation des Browsers benutzt
        'Dann in die Fahrzeugsuche zurückleiten
        If IsNothing(objHaendler.Fahrzeuge) = True Then
            Response.Redirect("Change07.aspx?AppID=" & Session("AppID").ToString)
        End If
    End Sub


End Class