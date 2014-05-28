Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Components.ComCommon

Public Class Change43_2
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
    Private objSuche As CKG.Components.ComCommon.Finance.Search
    Private objHaendler As fin_06

    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lnkKreditlimit As System.Web.UI.WebControls.HyperLink
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ShowScript As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents Kopfdaten1 As ComCommon.PageElements.Kopfdaten
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
        lnkKreditlimit.NavigateUrl = "Change43.aspx?AppID=" & Session("AppID").ToString


        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text
        m_App = New Base.Kernel.Security.App(m_User)

        If Session("AppHaendler") Is Nothing Then
            Response.Redirect("Change43.aspx?AppID=" & Session("AppID").ToString)
        End If

        If Session("objSuche") Is Nothing Then
            Response.Redirect("Change43.aspx?AppID=" & Session("AppID").ToString)
        Else
            objSuche = CType(Session("objSuche"), CKG.Components.ComCommon.Finance.Search)
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

        objHaendler = CType(Session("AppHaendler"), fin_06)

        If Not IsPostBack Then
            ddlPageSize.Items.Add("10")
            ddlPageSize.Items.Add("20")
            ddlPageSize.Items.Add("50")
            ddlPageSize.Items.Add("100")
            ddlPageSize.Items.Add("200")
            ddlPageSize.Items.Add("500")
            ddlPageSize.Items.Add("1000")
            ddlPageSize.SelectedIndex = 2

            Kopfdaten1.Kontingente = objHaendler.Kontingente
            FillGrid(0)
        Else
            CheckGrid()
        End If

    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim errMessage As String = String.Empty


        If objHaendler.isHEZ Then       'HEZ
            objHaendler.checkInputHEZ(txtZulDatum.Text, errMessage)
        End If


        'datumsprüfung
        If HelpProcedures.checkDate(txtZulDatum, errMessage, False) = True Then
            Dim tmpDate As Date
            Dim tmpDate2 As Date
            tmpDate = CDate(txtZulDatum.Text)

            If Not tmpDate <= Today Then
                If Not tmpDate.DayOfWeek = DayOfWeek.Saturday And Not tmpDate.DayOfWeek = DayOfWeek.Sunday Then
                    If TimeSerial(Now.Hour, Now.Minute, Now.Second) > TimeValue("10:00:00") Then
                        tmpdate2 = checkworkdays(2)
                        If tmpDate < tmpdate2 Then
                            errMessage = "nach 15:00 2 Arbeitstage für Zulassung, nächstmögliche Zulassung: " & tmpDate2.ToShortDateString
                            txtZulDatum.Text = tmpDate2.ToShortDateString
                        End If
                    ElseIf TimeSerial(Now.Hour, Now.Minute, Now.Second) <= TimeValue("15:00:00") Then
                        tmpDate2 = checkworkdays(1)
                        If tmpDate < tmpDate2 Then
                            errMessage = "vor 15:00 1 Arbeitstag für Zulassung, nächstmögliche Zulassung: " & tmpDate2.ToShortDateString
                        End If
                    End If
                Else
                    errMessage = "Zulassungsdatum darf nicht auf ein Wochenende fallen"
                End If
            Else
                errMessage = "Zulassungsdatum darf nicht in der Vergangenheit liegen oder 'heute' sein"
            End If
        End If

        If (errMessage <> String.Empty) Then
            Kopfdaten1.MessageError = errMessage
            txtZulDatum.Text = String.Empty
        Else
            DoSubmit()
        End If
    End Sub

    Private Function addDaysWithoutweekend(ByVal x As Int32) As Double
        'bapi in funktion checkworkdays übernimmt die funktion
        ''wie viel tage müssen hinzugefügt werden
        'Dim i As Double = 0
        'Dim z As Int32 = 1

        'While i < x
        '    If Not Now.AddDays(z).DayOfWeek = DayOfWeek.Saturday And Not Now.AddDays(z).DayOfWeek = DayOfWeek.Sunday Then
        '        i = i + 1
        '    End If
        '    z = z + 1
        'End While

        'Return z - 1 'z ist auf 1 initialisiert
    End Function

    Private Function checkworkdays(ByVal x As Int32) As Date
        If objHaendler Is Nothing Then
            objHaendler = CType(Session("AppHaendler"), fin_06)
        End If
        Dim i As Int32
        Dim tmpDatum As Date = Today
        'das bapi gibt den nächsten arbeitstag von dem übergebenen datum zurück
        For i = 0 To x - 1
            tmpDatum = objHaendler.getNextWorkDay(tmpDatum, Session("AppID").ToString, Session.SessionID)
        Next
        Return tmpDatum
    End Function


    Private Sub DoSubmit()
        Dim tmpDataView As DataView = objHaendler.Fahrzeuge.DefaultView

        tmpDataView.RowFilter = "MANDT <> '0'"
        Dim intFahrzeugBriefe As Int32 = tmpDataView.Count
        tmpDataView.RowFilter = ""

        If intFahrzeugBriefe = 0 Then
            Kopfdaten1.MessageError = "Bitte wählen Sie erst Fahrzeuge zum Zulassen aus."
            FillGrid(DataGrid1.CurrentPageIndex)
        Else
            Session("AppHaendler") = objHaendler
            objHaendler.zuldatum = CType(txtZulDatum.Text, Date) 'für HEZ Zulassungsdatum setzen!
            Response.Redirect("Change43_3.aspx?AppID=" & Session("AppID").ToString)
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
        Dim txtbox As TextBox
        Dim control As control
        'Dim blnScriptFound As Boolean = False
        Dim intZaehl As Int32
        Dim intReturn As Int32 = 0

        lblError.Text = ""

        For Each item In DataGrid1.Items
            intZaehl = 1
            Dim strZZFAHRG As String = ""

            For Each cell In item.Cells
                If intZaehl = 1 Then
                    strZZFAHRG = "ZZFAHRG = '" & cell.Text & "'"
                End If
                For Each control In cell.Controls

                    objHaendler.Fahrzeuge.AcceptChanges()
                    Dim tmpRows As DataRow()
                    tmpRows = objHaendler.Fahrzeuge.Select(strZZFAHRG)
                    tmpRows(0).BeginEdit()

                    Dim checkBox = TryCast(control, CheckBox)
                    If (checkBox IsNot Nothing) Then
                        chbox = checkBox
                        If chbox.ID = "chk0003" Then
                            If chbox.Checked Then
                                tmpRows(0).Item("MANDT") = "5"      'Neu-Fzg. ausgewählt
                                intReturn += 1
                            Else
                                tmpRows(0).Item("MANDT") = "0"      'Neu-Fzg. ausgewählt
                                intReturn += 1
                            End If
                        End If
                    End If

                    Dim textBox = TryCast(control, TextBox)
                    If (textBox IsNot Nothing) Then
                        txtbox = textBox
                        If txtbox.ID = "txt_wunschkennz" Then
                            If txtbox.Text.Length > 0 AndAlso tmpRows(0).Item("MANDT") = "5" Then
                                tmpRows(0).Item("LICENSE_NUM") = txtbox.Text     'Kennzeichen auslesen wenn gefüllt
                            End If
                        End If
                    End If
                    tmpRows(0).EndEdit()
                    objHaendler.Fahrzeuge.AcceptChanges()
                Next
                intZaehl += 1
            Next
        Next
        Session("AppHaendler") = objHaendler
        Return intReturn
    End Function

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        Dim tmpDataView As DataView = objHaendler.Fahrzeuge.DefaultView
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
            'Dim blnScriptFound As Boolean = False
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

                            If chkBox.ID = "chk0003" Then
                                chkBox.Checked = False
                                If mandt = "3" Then
                                    chkBox.Checked = True
                                End If
                            End If
                        End If
                    Next
                    intZaehl += 1
                Next
            Next
        End If
    End Sub

    'Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    '    DoSubmit()
    'End Sub

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
' $History: Change43_2.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 24.06.09   Time: 15:53
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2918 Z_S_Add_Date
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 5.06.09    Time: 15:58
' Updated in $/CKAG/Components/ComCommon/Finance
' mögliche try catches entfernt
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 10.04.08   Time: 17:37
' Updated in $/CKAG/Components/ComCommon/Finance
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:32
' Created in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 10:47
' Created in $/CKAG/portal/Components/ComCommon/Finance
' 
' *****************  Version 12  *****************
' User: Jungj        Date: 13.03.08   Time: 14:41
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' RTFS Kopfdaten änderung auf Finance Kopfdaten 
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 30.01.08   Time: 8:57
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' BugFixes 
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 29.01.08   Time: 15:59
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Zulassungsdatumcheck über bapi z_s_add_Date realisiert
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 29.01.08   Time: 15:09
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Z_S_ADD_DATE hinzugefügt
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 23.01.08   Time: 12:59
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Bugfixes Rothe 
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 21.01.08   Time: 9:45
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA: 1618
' 
' *****************  Version 6  *****************
' User: Uha          Date: 8.01.08    Time: 16:07
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 7.01.08    Time: 16:21
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' HEZ Bugfix + Dropdownliste zur Händlerauswahl 
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 4.01.08    Time: 16:01
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Bugfix RTFS
' 
' *****************  Version 3  *****************
' User: Uha          Date: 19.12.07   Time: 14:10
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1510,1512,1511 Anforderung / Zulassung
' 
' *****************  Version 2  *****************
' User: Uha          Date: 17.12.07   Time: 17:58
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Versand ZB II / Briefe - komplierfähige Zwischenversion
' 
' *****************  Version 1  *****************
' User: Uha          Date: 13.12.07   Time: 17:18
' Created in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Elemente für Temp./Endg. bzw. HEZ Anforderung hinzugefügt (Change42ff,
' fin_06, Change43ff und fin_08)
' 
' ************************************************
