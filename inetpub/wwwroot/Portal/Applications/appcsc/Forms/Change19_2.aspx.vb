Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Change19_2
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
    Private objFahrzeuge As CSC_Briefruecklaeufer '  CSC_Briefruecklaeufer

    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents lnkExcel As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents cmdConfirm As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdReset As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdBack As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblDownloadTip As System.Web.UI.WebControls.Label
    Protected WithEvents tblResult As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblWait As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Image2 As System.Web.UI.WebControls.Image
    Protected WithEvents Image3 As System.Web.UI.WebControls.Image
    Protected WithEvents Image4 As System.Web.UI.WebControls.Image
    Protected WithEvents Image5 As System.Web.UI.WebControls.Image
    Protected WithEvents Image6 As System.Web.UI.WebControls.Image
    Protected WithEvents Image7 As System.Web.UI.WebControls.Image
    Protected WithEvents Image8 As System.Web.UI.WebControls.Image
    Protected WithEvents Image9 As System.Web.UI.WebControls.Image
    Protected WithEvents Image10 As System.Web.UI.WebControls.Image
    Protected WithEvents Image11 As System.Web.UI.WebControls.Image
    Protected WithEvents chkDataLoaded As System.Web.UI.WebControls.CheckBox
    Protected WithEvents lnkKreditlimit As System.Web.UI.WebControls.HyperLink
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)
        lnkKreditlimit.NavigateUrl = "Report19.aspx?AppID=" & Session("AppID").ToString
        Try
            If Request.QueryString("TYPE") = "False" Then
                lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString & ":Briefe ohne Daten"
            Else
                lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString & ":Daten ohne Briefe"
                DataGrid1.Columns(DataGrid1.Columns.Count - 4).Visible = False   'Spalte ÄN ausblenden
            End If

            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If Session("objFahrzeuge") Is Nothing Then
                Response.Redirect("Report19.aspx?AppID=" & Session("AppID").ToString)
            End If

            objFahrzeuge = CType(Session("objFahrzeuge"), CSC_Briefruecklaeufer) ' CSC_Briefruecklaeufer) ' CSC_Briefruecklaeufer) ' CSC_Briefruecklaeufer)

            If objFahrzeuge.Result.Rows.Count = 0 Then
                lblError.Text = "Zu den gewählten Kriterien wurden keine Ergebnisse gefunden."
            Else
                If Not IsPostBack Then
                    ddlPageSize.Items.Add("10")
                    ddlPageSize.Items.Add("20")
                    ddlPageSize.Items.Add("50")
                    ddlPageSize.Items.Add("100")
                    ddlPageSize.Items.Add("200")
                    ddlPageSize.Items.Add("500")
                    ddlPageSize.Items.Add("1000")
                    ddlPageSize.SelectedIndex = 0

                    FillGrid(0)
                    Session("lnkExcel") = "/Portal/Temp/Excel/" & objFahrzeuge.ExcelFileName
                    Session("objFahrzeuge") = objFahrzeuge
                    If Not Session("lnkExcel").ToString.Length = 0 Then
                        lblDownloadTip.Visible = True
                        lnkExcel.Visible = True
                        lnkExcel.NavigateUrl = Session("lnkExcel").ToString
                    End If
                    cmdSave.Visible = True
                Else
                    CheckGrid()
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        Dim status As String = String.Empty
        Dim tmpDataView As New DataView()

        CheckGrid(status)

        If status <> String.Empty Then
            lblError.Text = status
            Exit Sub
        End If

        tmpDataView = objFahrzeuge.Result.DefaultView

        tmpDataView.RowFilter = "ActionNOTHING = 0"
        Dim intFahrzeugBriefe As Int32 = tmpDataView.Count
        'tmpDataView.RowFilter = ""

        If intFahrzeugBriefe = 0 Then
            lblError.Text = "Bitte zunächst Vorgang bearbeiten."

            'FillGrid(DataGrid1.CurrentPageIndex)
        Else
            cmdSave.Visible = False
            cmdConfirm.Visible = True
            cmdReset.Visible = True

            FillGrid(0)
            Session("objFahrzeuge") = objFahrzeuge
            'Response.Redirect("Change07_3.aspx?AppID=" & Session("AppID").ToString)
        End If
    End Sub

    Private Function CheckGrid(Optional ByRef status As String = "") As Int32
        Dim item As DataGridItem
        Dim cell As TableCell
        Dim chbox As RadioButton
        Dim textbox As TextBox
        Dim control As Control
        Dim blnScriptFound As Boolean = False
        Dim intZaehl As Int32
        Dim intReturn As Int32 = 0

        For Each item In DataGrid1.Items
            intZaehl = 0
            Dim strContentCell0 As String = item.Cells(1).Text      'Vertragsnummer = Schlüssel
            If strContentCell0 = String.Empty OrElse strContentCell0 = "&nbsp;" Then
                lblError.Text = "Datensätze ohne Kontonummer können nicht bearbeitet werden und werden übersprungen!"
            Else
                Dim strKontonummer As String = "Vertragsnummer = '" & strContentCell0 & "'"

                For Each cell In item.Cells
                    For Each control In cell.Controls
                        If TypeOf control Is RadioButton Then
                            chbox = CType(control, RadioButton)
                            objFahrzeuge.Result.AcceptChanges()
                            Dim tmpRows As DataRow()
                            tmpRows = objFahrzeuge.Result.Select(strKontonummer)
                            If tmprows.Length > 0 Then
                                tmpRows(0).BeginEdit()
                                Select Case chbox.ID
                                    Case "chkActionDELE"
                                        If chbox.Checked Then
                                            tmpRows(0).Item("ActionDELE") = True

                                            tmpRows(0).Item("ActionNOTHING") = False
                                            tmpRows(0).Item("ActionVERS") = False
                                            tmpRows(0).Item("ActionCHAN") = False
                                            'tmpRows(0).Item("VertragsnummerNeu") = String.Empty
                                            intReturn += 1
                                        End If
                                    Case "chkActionVERS"
                                        If chbox.Checked Then
                                            tmpRows(0).Item("ActionVERS") = True

                                            tmpRows(0).Item("ActionNOTHING") = False
                                            tmpRows(0).Item("ActionDELE") = False
                                            tmpRows(0).Item("ActionCHAN") = False
                                            intReturn += 1
                                        End If
                                    Case "chkActionCHAN"
                                        If chbox.Checked Then
                                            tmpRows(0).Item("ActionCHAN") = True

                                            tmpRows(0).Item("ActionNOTHING") = False
                                            tmpRows(0).Item("ActionDELE") = False
                                            tmpRows(0).Item("ActionVERS") = False
                                            intReturn += 1
                                            'If tmpRows(0).Item("VertragsnummerNeu") = String.Empty Then
                                            '    status = "Fehler Zeile " & intReturn & ": Bitte neue Kontonummer angeben!"
                                            'End If
                                        End If
                                    Case "chkActionNOTHING"
                                        If chbox.Checked Then
                                            tmpRows(0).Item("ActionNOTHING") = True
                                            tmpRows(0).Item("ActionDELE") = False
                                            tmpRows(0).Item("ActionVERS") = False
                                            tmpRows(0).Item("ActionCHAN") = False
                                            'tmpRows(0).Item("VertragsnummerNeu") = String.Empty
                                        End If
                                End Select
                                tmpRows(0).EndEdit()
                                objFahrzeuge.Result.AcceptChanges()
                            End If
                        End If
                        If TypeOf control Is TextBox Then
                            textbox = CType(control, TextBox)
                            If textbox.ID = "txtKontonummerNeu" Then
                                objFahrzeuge.Result.AcceptChanges()
                                Dim tmpRows As DataRow()
                                tmpRows = objFahrzeuge.Result.Select(strKontonummer)
                                textbox.Visible = False
                                If tmpRows.Length > 0 Then
                                    If (tmpRows(0).Item("ActionCHAN") = True) Then
                                        tmpRows(0).BeginEdit()
                                        tmpRows(0).Item("VertragsnummerNeu") = textbox.Text
                                        tmpRows(0).EndEdit()
                                        objFahrzeuge.Result.AcceptChanges()
                                        textbox.Visible = True
                                    End If
                                End If
                            End If
                        End If
                    Next
                    intZaehl += 1
                Next
            End If
        Next
        Session("objFahrzeuge") = objFahrzeuge
        Return intReturn
    End Function

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        Dim tmpDataView As New DataView()

        tmpDataView = objFahrzeuge.Result.DefaultView
        'If cmdConfirm.Visible Then
        '    tmpDataView.RowFilter = "ActionNOTHING = 0 AND Kontonummer <> ''" 'Seidel, 26.10.2004: Datensaetze ohne Kontonummer ausblenden.
        'Else
        '    tmpDataView.RowFilter = "Kontonummer <> ''" 'Seidel, 26.10.2004: Datensaetze ohne Kontonummer ausblenden.
        'End If

        If tmpDataView.Count = 0 Then
            DataGrid1.Visible = False
            lblError.Text = "Keine Daten zur Anzeige gefunden."
            lblError.Visible = True
        Else
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
            If cmdConfirm.Visible Then
                DataGrid1.AllowPaging = False
                DataGrid1.AllowSorting = False
            Else
                DataGrid1.AllowPaging = True
                DataGrid1.AllowSorting = True
            End If
            DataGrid1.DataSource = tmpDataView
            DataGrid1.DataBind()

            If cmdConfirm.Visible Then
                If cmdBack.Visible Then
                    lblNoData.Text = " Sie haben die folgenden Vorgänge beauftragt (Anzahl: " & tmpDataView.Count.ToString & ")."
                Else
                    lblNoData.Text = " Sie haben die folgenden Vorgänge ausgewählt (Anzahl: " & tmpDataView.Count.ToString & ")."
                End If
            Else
                lblNoData.Text = " Es wurde(n) " & tmpDataView.Count.ToString & " Vorgang/Vorgänge gefunden."
            End If
            lblNoData.Visible = True

            If DataGrid1.AllowPaging AndAlso DataGrid1.PageCount > 1 Then
                DataGrid1.PagerStyle.Visible = True
                DataGrid1.PagerStyle.CssClass = "PagerStyle"
                If DataGrid1.CurrentPageIndex = DataGrid1.PageCount - 1 Then
                    DataGrid1.PagerStyle.NextPageText = "<img border=""0"" src=""/Portal/Images/empty.gif"" width=""12"" height=""11"">"
                Else
                    DataGrid1.PagerStyle.NextPageText = "<img border=""0"" src=""/Portal/Images/arrow_right.gif"" width=""12"" height=""11"">"
                End If

                If DataGrid1.CurrentPageIndex = 0 Then
                    DataGrid1.PagerStyle.PrevPageText = "<img border=""0"" src=""/Portal/Images/empty.gif"" width=""12"" height=""11"">"
                Else
                    DataGrid1.PagerStyle.PrevPageText = "<img border=""0"" src=""/Portal/Images/arrow_left.gif"" width=""12"" height=""11"">"
                End If
                DataGrid1.DataBind()
            Else
                DataGrid1.PagerStyle.Visible = False
            End If

            Dim item As DataGridItem
            Dim cell As TableCell
            Dim textbox As TextBox
            Dim control As Control

            For Each item In DataGrid1.Items
                For Each cell In item.Cells
                    Dim strContentCell0 As String = item.Cells(1).Text
                    For Each control In cell.Controls
                        If TypeOf control Is TextBox Then
                            textbox = CType(control, TextBox)
                            If textbox.ID = "txtKontonummerNeu" Then
                                Dim tmpRows As DataRow()

                                Dim strKontonummer As String = "Vertragsnummer = '" & strContentCell0 & "'"

                                tmpRows = objFahrzeuge.Result.Select(strKontonummer)
                                If tmpRows.Length > 0 Then
                                    textbox.Text = tmpRows(0).Item("VertragsnummerNeu")
                                    If tmpRows(0).Item("ActionCHAN") = True Then
                                        textbox.Visible = True
                                    Else
                                        textbox.Visible = False
                                    End If
                                    'If textbox.Text <> String.Empty Then

                                    'End If
                                End If
                            End If
                        End If
                    Next
                Next
            Next
        End If
    End Sub

    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        DoSubmit()
    End Sub

    Private Sub cmdConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdConfirm.Click
        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Request.QueryString("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)

        objFahrzeuge.StandardLogID = logApp.LogStandardIdentity
        objFahrzeuge.Change(Session("AppID").ToString, Session.SessionID.ToString, Me)

        Dim tmpDataView As New DataView()
        tmpDataView = objFahrzeuge.Result.DefaultView
        tmpDataView.RowFilter = "ActionNOTHING = 0"

        Dim strTemp As String = "Es wurde(n) " & tmpDataView.Count.ToString & " Vorgang/Vorgänge beauftragt."
        Dim tblTemp As DataTable
        tblTemp = New DataTable()

        With tblTemp.Columns
            '.Add("Equipment", System.Type.GetType("System.String"))
            .Add("Kontonummer", System.Type.GetType("System.String"))
            .Add("Fahrgestellnummer", System.Type.GetType("System.String"))
            .Add("Briefnummer", System.Type.GetType("System.String"))
            .Add("Kennzeichen", System.Type.GetType("System.String"))
            .Add("Datum WV-Liste", System.Type.GetType("System.String"))
            .Add("Vorgang", System.Type.GetType("System.String"))
            .Add("KontonummerNeu", System.Type.GetType("System.String"))
            .Add("Aktion", System.Type.GetType("System.String"))
            .Add("Bemerkung", System.Type.GetType("System.String"))
        End With

        Dim i As Int32
        Dim rowNew As DataRow
        For i = 0 To tmpDataView.Count - 1
            rowNew = tblTemp.NewRow
            'rowNew("Equipment") = tmpDataView.Item(i)("Equipment")
            rowNew("Kontonummer") = tmpDataView.Item(i)("Vertragsnummer")
            rowNew("Fahrgestellnummer") = tmpDataView.Item(i)("Fahrgestellnummer")
            rowNew("Briefnummer") = tmpDataView.Item(i)("Briefnummer")
            rowNew("Kennzeichen") = tmpDataView.Item(i)("Kennzeichen")
            rowNew("Datum WV-Liste") = tmpDataView.Item(i)("DatumAenderung")
            If Request.QueryString("TYPE") = "False" Then
                rowNew("Vorgang") = "Brief ohne Daten"
            Else
                rowNew("Vorgang") = "Daten ohne Brief"
            End If

            rowNew("KontonummerNeu") = tmpDataView.Item(i)("VertragsnummerNeu")
            rowNew("Aktion") = tmpDataView.Item(i)("Action")
            rowNew("Bemerkung") = tmpDataView.Item(i)("Bemerkung")
            'rowNew("Bemerkung") = tmpDataView.Item(i)("Bemerkung")
            'rowNew("Aktion") = tmpDataView.Item(i)("Action")
            tblTemp.Rows.Add(rowNew)
        Next

        If Not objFahrzeuge.Status = 0 Then

            strTemp &= " Es traten Fehler auf."
            lblError.Text = strTemp
        End If

        logApp.UpdateEntry("APP", Session("AppID").ToString, strTemp, tblTemp)
        cmdBack.Visible = True
        FillGrid(0)
        cmdConfirm.Visible = False
        cmdReset.Visible = False
    End Sub

    Private Sub cmdReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReset.Click
        'cmdSave.Visible = True
        'cmdConfirm.Visible = False
        'cmdReset.Visible = False
        'FillGrid(DataGrid1.CurrentPageIndex)
        Response.Redirect("Report19.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    'Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBack.Click
    '    'Session("objFahrzeuge") = Nothing    
    '    Dim tmpDataView As New DataView()
    '    Dim type As String

    '    tmpDataView = objFahrzeuge.Result.DefaultView
    '    tmpDataView.RowFilter = ""

    '    FillGrid(0)
    '    Session("objFahrzeuge") = objFahrzeuge

    '    type = Request.QueryString("TYPE")
    '    Response.Redirect("Change19_2.aspx?AppID=" & Session("AppID").ToString & "&BACK=1&TYPE=" & type)
    'End Sub

    Private Sub AddClientScript(ByRef Page As System.Web.UI.Page, ByVal Script As String)
        Dim strScript As String
        Dim lit As LiteralControl

        Try
            strScript = "<SCRIPT language=""JavaScript"" type=""text/javascript"">"
            strScript &= String.Format("{0}", Script)
            strScript &= "</SCRIPT>"

            lit = New LiteralControl(strScript)
            Page.Controls.Add(lit)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        CheckGrid()
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        CheckGrid()
        FillGrid(e.NewPageIndex)
    End Sub

    'Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged

    '    DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
    '    FillGrid(0)

    'End Sub

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(0)
    End Sub

    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBack.Click
        Dim tmpDataView As New DataView()
        Dim type As String

        If cmdReset.Visible = True Then
            tmpDataView = objFahrzeuge.Result.DefaultView
            tmpDataView.RowFilter = ""

            FillGrid(0)
            Session("objFahrzeuge") = objFahrzeuge

            type = Request.QueryString("TYPE")
            Response.Redirect("Change19_2.aspx?AppID=" & Session("AppID").ToString & "&BACK=1&TYPE=" & type)
        Else
            Response.Redirect("Report19.aspx?AppID=" & Session("AppID").ToString)
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Change19_2.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 11.03.10   Time: 13:34
' Updated in $/CKAG/Applications/appcsc/Forms
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 2.05.08    Time: 10:37
' Updated in $/CKAG/Applications/appcsc/Forms
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 11:37
' Created in $/CKAG/Applications/appcsc/Forms
' 
' *****************  Version 6  *****************
' User: Uha          Date: 2.07.07    Time: 10:43
' Updated in $/CKG/Applications/AppCSC/AppCSCWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 5  *****************
' User: Uha          Date: 12.03.07   Time: 15:54
' Updated in $/CKG/Applications/AppCSC/AppCSCWeb/Forms
' Image-Pfade von Customize auf Images umgesetzt
' 
' *****************  Version 4  *****************
' User: Uha          Date: 7.03.07    Time: 9:28
' Updated in $/CKG/Applications/AppCSC/AppCSCWeb/Forms
' 
' ************************************************
