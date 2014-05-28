Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Change01_2
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private objSuche As FFE_Search
    Private objHaendler As FFE_Haendler

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
    Protected WithEvents lblZeichen As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Zeichen23 As System.Web.UI.WebControls.Label
    Protected WithEvents ShowScript As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents cmdSave2 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents trcmdSave As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trcmdSave2 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lnkFahrzeugAuswahl As System.Web.UI.WebControls.HyperLink
    Protected WithEvents Kopfdaten1 As Kopfdatenhaendler

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
#Region "Methods"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        Kopfdaten1.Message = ""
        lnkKreditlimit.NavigateUrl = "Change01.aspx?AppID=" & Session("AppID").ToString
        lnkFahrzeugAuswahl.NavigateUrl = "Change01_2.aspx?AppID=" & Session("AppID").ToString

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If Session("objHaendler") Is Nothing Then
                Response.Redirect("Change01.aspx?AppID=" & Session("AppID").ToString)
            End If

            If Session("objSuche") Is Nothing Then
                Response.Redirect("Change01.aspx?AppID=" & Session("AppID").ToString)
            Else
                objSuche = CType(Session("objSuche"), FFE_Search)
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

            objHaendler = CType(Session("objHaendler"), FFE_Haendler)

            If Not IsPostBack Then
                trcmdSave.Visible = True
                trcmdSave2.Visible = False
                lnkFahrzeugAuswahl.Visible = False

                ddlPageSize.Items.Add("10")
                ddlPageSize.Items.Add("20")
                ddlPageSize.Items.Add("50")
                ddlPageSize.Items.Add("100")
                ddlPageSize.Items.Add("200")
                ddlPageSize.Items.Add("500")
                ddlPageSize.Items.Add("1000")
                ddlPageSize.SelectedIndex = 2
                lbl_Zeichen23.Visible = True
                FillGrid(0)
            End If
            If IsPostBack AndAlso trcmdSave2.Visible Then
                CheckGrid2()
                FillGrid(0)
                lbl_Zeichen23.Visible = False
                lblZeichen.Visible = False
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Function CheckGrid2() As Boolean
        Dim item As DataGridItem
        Dim cell As TableCell
        Dim ddl As DropDownList
        Dim txtBox As TextBox
        Dim control As Control
        Dim intZaehl As Int32 = 0
        Dim blnReturn As Boolean = False

        Dim tmpRows As DataRow()

        'Fahrzeugtabelle instanziert?
        CheckFahrzeuge()

        For Each item In DataGrid1.Items
            Dim strZZFAHRG As String = "ZZFAHRG = '" & CType(item.FindControl("lnkHistorie"), HyperLink).Text & "'"

            For Each cell In item.Cells
                For Each control In cell.Controls
                    If TypeOf control Is DropDownList Then
                        ddl = CType(control, DropDownList)
                        If ddl.ID = "cmbAbrufgrund" Then
                            If Not ddl.SelectedItem Is Nothing Then
                                If ddl.SelectedItem.Value = "000" Then
                                    intZaehl += 1
                                Else
                                    objHaendler.Fahrzeuge.AcceptChanges()
                                    tmpRows = objHaendler.Fahrzeuge.Select(strZZFAHRG)
                                    tmpRows(0).BeginEdit()
                                    tmpRows(0).Item("AUGRU") = ddl.SelectedItem.Value
                                    tmpRows(0).EndEdit()
                                    objHaendler.Fahrzeuge.AcceptChanges()
                                End If
                            End If
                        Else
                            intZaehl += 1
                        End If
                    End If
                    If TypeOf control Is TextBox Then
                        txtBox = CType(control, TextBox)
                        If txtBox.ID = "txtZusatztext" Then
                            If Not txtBox.Text.Trim Is String.Empty Then
                                tmpRows = objHaendler.Fahrzeuge.Select(strZZFAHRG)
                                tmpRows(0).BeginEdit()
                                tmpRows(0).Item("text50") = txtBox.Text
                                tmpRows(0).EndEdit()
                                objHaendler.Fahrzeuge.AcceptChanges()
                            Else
                                If txtBox.Visible = True Then
                                    'wenn nicht ausgefüllt fehler 
                                    intZaehl += 1
                                End If
                            End If
                        End If
                    End If
                Next
            Next
        Next
        If intZaehl = 0 Then
            blnReturn = True
        End If
        Session("AppHaendler") = objHaendler
        Return blnReturn
    End Function



    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        'If CheckGrid2() Then
        '    objHaendler.ErsetzeAbrufgruende()
        '    Session("AppHaendler") = objHaendler
        '    Response.Redirect("Change01_3.aspx?AppID=" & Session("AppID").ToString)
        'Else
        '    lblError.Text = "Bitte geben Sie die Informationen zu den Abrufgründen ein."
        '    FillGrid(DataGrid1.CurrentPageIndex)
        'End If
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
                lblError.Text = "Bitte wählen Sie erst Fahrzeuge zur Anforderung aus."
                FillGrid(0)
            Else
                trcmdSave.Visible = False
                trcmdSave2.Visible = True

                FillGrid(0)
                lnkFahrzeugAuswahl.Visible = True

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
                        strZZFAHRG = "ZZFAHRG = '" & CType(cell.FindControl("lnkHistorie"), HyperLink).Text & "'"
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
                                Case "chk0006"
                                    If chbox.Checked Then
                                        tmpRows(0).Item("MANDT") = "6"
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
                                        Throw New Exception("Anforderungsart Retail: Für das Feld ""Anfragenummer"" sind 13 numerische Zeichen erforderlich!")
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

        Dim strKKB As String = "0"
        Dim mitGrund As Boolean = False
        'Fahrzeugtabelle instanziert?
        CheckFahrzeuge()


        Dim tmpDataView As New DataView()

        tmpDataView = objHaendler.Fahrzeuge.DefaultView

        If trcmdSave2.Visible Then
            tmpDataView.RowFilter = "MANDT <> '0'"
        Else
            tmpDataView.RowFilter = ""
        End If

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

            lblNoData.Text = "Es wurden " & tmpDataView.Count.ToString & " nicht angeforderte Dokumente gefunden."
            lblNoData.Visible = True

            If DataGrid1.PageCount > 1 Then
                DataGrid1.PagerStyle.CssClass = "PagerStyle"
                DataGrid1.DataBind()
                DataGrid1.PagerStyle.Visible = True
            Else
                DataGrid1.PagerStyle.Visible = False
            End If

            If objHaendler.Kontingente.Rows.Count = 2 Then  'Nur Standard-Kontingent->Spalte ausblenden
                DataGrid1.Columns(DataGrid1.Columns.Count - 8).Visible = False
            Else
                If objHaendler.Kontingente.Rows.Count > 2 Then  'Delayed Payment-Kontingent vorhanden?
                    If objHaendler.Kontingente.Rows(3)("Kreditkontrollbereich").ToString = "0004" Then  'Kann nur in Zeile 3 sein...
                        If CInt(objHaendler.Kontingente.Rows(3)("Richtwert_Alt")) = 0 Then 'Richtwert=0, ausblenden!
                            DataGrid1.Columns(DataGrid1.Columns.Count - 8).Visible = False
                        End If
                    Else
                        DataGrid1.Columns(DataGrid1.Columns.Count - 8).Visible = False  'Nein, dann sowieso ausblenden
                    End If
                    If objHaendler.Kontingente.Rows(2)("Kreditkontrollbereich").ToString = "0003" Then  'Kontingent = Retail
                        If CInt(objHaendler.Kontingente.Rows(2)("Richtwert_Alt")) = 0 Then 'Richtwert=0, ausblenden!
                            DataGrid1.Columns(DataGrid1.Columns.Count - 5).Visible = False
                            DataGrid1.Columns(DataGrid1.Columns.Count - 7).Visible = False
                        ElseIf Not IsPostBack Then
                            lblZeichen.Visible = True
                        End If
                    Else
                        DataGrid1.Columns(DataGrid1.Columns.Count - 5).Visible = False
                        DataGrid1.Columns(DataGrid1.Columns.Count - 7).Visible = False
                    End If
                    If objHaendler.Kontingente.Rows(4)("Kreditkontrollbereich").ToString = "0006" Then  'Kontingent = Retail
                        If CInt(objHaendler.Kontingente.Rows(4)("Richtwert_Alt")) = 0 Then 'Richtwert=0, ausblenden!
                            DataGrid1.Columns(DataGrid1.Columns.Count - 6).Visible = False
                        ElseIf Not IsPostBack Then
                            lblZeichen.Visible = True
                        End If
                    Else
                        DataGrid1.Columns(DataGrid1.Columns.Count - 6).Visible = False
                    End If
                End If
            End If

            Dim item As DataGridItem
            Dim cell As TableCell
            Dim chkBox As CheckBox
            Dim control As Control
            Dim blnScriptFound As Boolean = False
            Dim intZaehl As Int32

            For Each item In DataGrid1.Items
                intZaehl = 1

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
                                            mitGrund = True
                                        End If
                                    End If
                                Case "chk0002"
                                    If strKKB = "2" Then
                                        chkBox.Checked = True
                                        mitGrund = True
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
                                Case "chk0006"
                                    If blnBezahlt Then
                                        chkBox.Enabled = False
                                    Else
                                        If strKKB = "6" Then
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

        For Each item As DataGridItem In DataGrid1.Items
            If Not item.FindControl("lnkHistorie") Is Nothing Then
                CType(item.FindControl("lnkHistorie"), HyperLink).NavigateUrl = "Report46.aspx?AppID=" & m_User.Applications.Select("AppName = 'Report46'")(0)("AppID").ToString & "&VIN=" & CType(item.FindControl("lnkHistorie"), HyperLink).Text
            End If
        Next

        If trcmdSave2.Visible Then
            AbrufDatagridView()
            If mitGrund = False Then
                DataGrid1.Columns(19).Visible = False
                DataGrid1.Columns(20).Visible = False
            End If
        Else
            StandardDatagridView()
        End If
    End Sub

    Private Sub StandardDatagridView()
        DataGrid1.Columns(0).Visible = True
        DataGrid1.Columns(1).Visible = False
        DataGrid1.Columns(2).Visible = True
        DataGrid1.Columns(3).Visible = True
        DataGrid1.Columns(4).Visible = True
        DataGrid1.Columns(5).Visible = True
        'DataGrid1.Columns(6).Visible = True
        DataGrid1.Columns(7).Visible = True
        DataGrid1.Columns(8).Visible = True
        'DataGrid1.Columns(9).Visible = True
        'DataGrid1.Columns(10).Visible = True
        'DataGrid1.Columns(11).Visible = True
        'DataGrid1.Columns(12).Visible = True
        'DataGrid1.Columns(13).Visible = True
        'DataGrid1.Columns(14).Visible = True
        'DataGrid1.Columns(15).Visible = True
        'DataGrid1.Columns(16).Visible = True
        DataGrid1.Columns(18).Visible = False
        DataGrid1.Columns(19).Visible = False
        DataGrid1.Columns(20).Visible = False
    End Sub

    Private Sub AbrufDatagridView()
        DataGrid1.Columns(0).Visible = True
        DataGrid1.Columns(1).Visible = False
        DataGrid1.Columns(2).Visible = True
        DataGrid1.Columns(3).Visible = True
        DataGrid1.Columns(4).Visible = True
        DataGrid1.Columns(5).Visible = True
        DataGrid1.Columns(6).Visible = True
        DataGrid1.Columns(7).Visible = True
        DataGrid1.Columns(8).Visible = True
        DataGrid1.Columns(9).Visible = True
        DataGrid1.Columns(10).Visible = False
        DataGrid1.Columns(11).Visible = False
        DataGrid1.Columns(12).Visible = False
        DataGrid1.Columns(13).Visible = False
        DataGrid1.Columns(14).Visible = False
        DataGrid1.Columns(15).Visible = False
        DataGrid1.Columns(16).Visible = False
        DataGrid1.Columns(17).Visible = False
        DataGrid1.Columns(18).Visible = True
        DataGrid1.Columns(19).Visible = True
        DataGrid1.Columns(20).Visible = False
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
            Response.Redirect("Change01.aspx?AppID=" & Session("AppID").ToString)
        End If
    End Sub

    Private Sub DataGrid1_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGrid1.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim tmpddl As DropDownList
            tmpddl = CType(e.Item.FindControl("cmbAbrufgrund"), DropDownList)
            If Not tmpddl Is Nothing AndAlso tmpddl.Items.Count > 0 Then
                Dim tmprows() As DataRow 'für fahrzeugtabelle
                If objHaendler Is Nothing Then
                    objHaendler = CType(Session("AppHaendler"), FFE_Haendler)
                End If
                tmprows = objHaendler.Fahrzeuge.Select("ZZFAHRG = '" & CType(e.Item.FindControl("lnkHistorie"), HyperLink).Text & "'")
                If Not tmprows.Length = 0 Then
                    Dim tmpListItem As ListItem
                    If Not tmprows(0).Item("AUGRU") Is DBNull.Value AndAlso Not CStr(tmprows(0).Item("AUGRU")) Is String.Empty Then
                        tmpListItem = tmpddl.Items.FindByValue(CStr(tmprows(0).Item("AUGRU")))
                        If Not tmpListItem Is Nothing Then
                            tmpddl.SelectedIndex = tmpddl.Items.IndexOf(tmpListItem)
                            'es muss festgestellt werden ob aktelle DDL auswahl einen Textbox anzeigen soll, db=MitZusatzText Boolean
                            'und das label lblZusatzinfo soll gefüllt werden 
                            Dim vwAbrufgruende As DataView = objHaendler.Abrufgruende.DefaultView
                            Select Case tmprows(0).Item("MANDT").ToString
                                Case "1"
                                    vwAbrufgruende.RowFilter = "AbrufTyp='temp'"
                                Case "2"
                                    vwAbrufgruende.RowFilter = "AbrufTyp='endg'"
                            End Select
                            Dim tmprows2() As DataRow 'für abrufgründe
                            tmprows2 = objHaendler.Abrufgruende.Select("SapWert='" & tmpddl.SelectedItem.Value & "'")
                            Dim tmpLabel As Label
                            tmpLabel = CType(e.Item.FindControl("lblZusatzinfo"), Label)
                            If Not tmprows2(0).Item("Zusatzbemerkung") Is DBNull.Value Then
                                tmpLabel.Text = tmprows2(0).Item("Zusatzbemerkung")
                            End If
                            If tmprows2(0).Item("MitZusatzText") Then
                                Dim tmptxt As TextBox
                                tmptxt = e.Item.FindControl("txtZusatztext")
                                If Not tmptxt Is Nothing Then
                                    tmptxt.Visible = True
                                    If Not tmprows(0).Item("text50") Is DBNull.Value Then
                                        tmptxt.Text = tmprows(0).Item("text50")
                                    End If
                                End If
                                DataGrid1.Columns(19).Visible = True
                            Else
                                Dim tmptxt As TextBox
                                tmptxt = e.Item.FindControl("txtZusatztext")
                                If Not tmptxt Is Nothing Then
                                    tmptxt.Visible = False
                                End If
                                DataGrid1.Columns(DataGrid1.Columns.Count - 1).Visible = False
                            End If
                        End If
                    End If
                End If
            Else
                tmpddl.Visible = False
            End If
        End If
    End Sub

    Public Function cmbAbrufgrund_ItemDataBound1(ByVal MANDT As String) As DataView

        If objHaendler Is Nothing Then
            objHaendler = CType(Session("AppHaendler"), FFE_Haendler)
        End If
        Dim vwAbrufgruende As DataView = objHaendler.Abrufgruende.DefaultView

        Select Case MANDT
            Case "1"
                vwAbrufgruende.RowFilter = "AbrufTyp='temp'"
            Case "2"
                vwAbrufgruende.RowFilter = "AbrufTyp='endg'"
            Case "3"
                vwAbrufgruende.RowFilter = "AbrufTyp='Ret'"
            Case "4"
                vwAbrufgruende.RowFilter = "AbrufTyp='DP'"
            Case "6"
                vwAbrufgruende.RowFilter = "AbrufTyp='kfkl'"
        End Select
        Return vwAbrufgruende
    End Function

    Private Sub cmdSave2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave2.Click
        If CheckGrid2() Then
            objHaendler.ErsetzeAbrufgruende()
            Session("AppHaendler") = objHaendler
            Response.Redirect("Change01_3.aspx?AppID=" & Session("AppID").ToString)
        Else
            lblError.Text = "Bitte geben Sie die Informationen zu den Abrufgründen ein."
            FillGrid(0)
        End If
    End Sub
#End Region

    Protected Sub DataGrid1_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DataGrid1.SelectedIndexChanged

    End Sub
End Class
        ' ************************************************
        ' $History: Change01_2.aspx.vb $
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 17.09.08   Time: 8:17
' Updated in $/CKAG/Applications/AppFFE/forms
' ITA 2222 fertig
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 18.06.08   Time: 14:33
' Updated in $/CKAG/Applications/AppFFE/forms
' Ausblenden Händler Kontingente
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 5.06.08    Time: 13:04
' Updated in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 4.06.08    Time: 18:43
' Updated in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 21.05.08   Time: 16:34
' Updated in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 15.05.08   Time: 16:59
' Updated in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 13.05.08   Time: 16:41
' Updated in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 5.05.08    Time: 17:09
' Updated in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 11.04.08   Time: 11:57
' Updated in $/CKAG/Applications/AppFFE/forms
' Migration OR
        ' 
        ' *****************  Version 1  *****************
        ' User: Fassbenders  Date: 9.04.08    Time: 13:32
        ' Created in $/CKAG/Applications/AppFFE/forms
        ' 
        ' *****************  Version 3  *****************
        ' User: Rudolpho     Date: 9.04.08    Time: 13:06
        ' Updated in $/CKG/Applications/AppFFE/AppFFEWeb/forms
        ' ITA: 1790
        ' 
        ' *****************  Version 2  *****************
        ' User: Rudolpho     Date: 7.04.08    Time: 14:04
        ' Updated in $/CKG/Applications/AppFFE/AppFFEWeb/forms
        ' 
        ' *****************  Version 1  *****************
        ' User: Rudolpho     Date: 3.04.08    Time: 11:19
        ' Created in $/CKG/Applications/AppFFE/AppFFEWeb/forms
        ' ITA 1790
        ' 
        ' ************************************************