Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Change12_2
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
    Private objFahrzeuge As CSC_Sperrliste

    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ShowScript As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents lnkExcel As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents cmdConfirm As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdReset As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdBack As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblDownloadTip As System.Web.UI.WebControls.Label
    Protected WithEvents lnkKreditlimit As System.Web.UI.WebControls.HyperLink
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)
        lnkKreditlimit.NavigateUrl = "Change12.aspx?AppID=" & Session("AppID").ToString
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If Session("objFahrzeuge") Is Nothing Then
                Response.Redirect("Change12.aspx?AppID=" & Session("AppID").ToString)
            End If

            objFahrzeuge = CType(Session("objFahrzeuge"), CSC_Sperrliste)

            If Not IsPostBack Then
                If Not Session("lnkExcel").ToString.Length = 0 Then
                    lblDownloadTip.Visible = True
                    lnkExcel.Visible = True
                    lnkExcel.NavigateUrl = Session("lnkExcel").ToString
                End If
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
        CheckGrid()

        Dim tmpDataView As New DataView()
        tmpDataView = objFahrzeuge.Fahrzeuge.DefaultView

        tmpDataView.RowFilter = "ActionNOTHING = 0"
        Dim intFahrzeugBriefe As Int32 = tmpDataView.Count
        tmpDataView.RowFilter = ""

        If intFahrzeugBriefe = 0 Then
            lblError.Text = "Bitte wählen Sie erst Fahrzeuge zur Anforderung aus."
            FillGrid(DataGrid1.CurrentPageIndex)
        Else
            cmdSave.Visible = False
            cmdConfirm.Visible = True
            cmdReset.Visible = True

            FillGrid(0)
            Session("objFahrzeuge") = objFahrzeuge
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
        Dim chbox As RadioButton
        Dim textbox As TextBox
        Dim control As Control
        Dim blnScriptFound As Boolean = False
        Dim intZaehl As Int32
        Dim intReturn As Int32 = 0

        For Each item In DataGrid1.Items
            intZaehl = 0
            Dim strKontonummer As String = "Kontonummer = '" & item.Cells(0).Text & "'"

            For Each cell In item.Cells
                For Each control In cell.Controls
                    If TypeOf control Is RadioButton Then
                        chbox = CType(control, RadioButton)
                        objFahrzeuge.Fahrzeuge.AcceptChanges()
                        Dim tmpRows As DataRow()
                        tmpRows = objFahrzeuge.Fahrzeuge.Select(strKontonummer)
                        tmpRows(0).BeginEdit()
                        Select Case chbox.ID
                            Case "chkActionDELE"
                                If chbox.Checked Then
                                    tmpRows(0).Item("ActionDELE") = True
                                    tmpRows(0).Item("ActionNOTHING") = False
                                    intReturn += 1
                                End If
                            Case Else
                                If chbox.Checked Then
                                    tmpRows(0).Item("ActionNOTHING") = True
                                    tmpRows(0).Item("ActionDELE") = False
                                End If
                        End Select
                        tmpRows(0).EndEdit()
                        objFahrzeuge.Fahrzeuge.AcceptChanges()
                    End If
                    If TypeOf control Is TextBox Then
                        textbox = CType(control, TextBox)
                        If textbox.ID = "txtKontonummerNeu" Then
                            objFahrzeuge.Fahrzeuge.AcceptChanges()
                            Dim tmpRows As DataRow()
                            tmpRows = objFahrzeuge.Fahrzeuge.Select(strKontonummer)
                            tmpRows(0).BeginEdit()
                            tmpRows(0).Item("KontonummerNeu") = textbox.Text
                            tmpRows(0).EndEdit()
                            objFahrzeuge.Fahrzeuge.AcceptChanges()
                        End If
                    End If
                Next
                intZaehl += 1
            Next
        Next
        Session("objFahrzeuge") = objFahrzeuge
        Return intReturn
    End Function

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        Dim tmpDataView As New DataView()
        tmpDataView = objFahrzeuge.Fahrzeuge.DefaultView
        If cmdConfirm.Visible Then
            tmpDataView.RowFilter = "ActionNOTHING = 0"
            ShowScript.Visible = False
        Else
            tmpDataView.RowFilter = ""
        End If

        If tmpDataView.Count = 0 Then
            DataGrid1.Visible = False
            lblError.Text = "Keine Daten zur Anzeige gefunden."
            lblError.Visible = True
            ShowScript.Visible = False
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
                    lblNoData.Text = "Sie haben die folgenden Vorgänge beauftragt (Anzahl: " & tmpDataView.Count.ToString & ")."
                Else
                    lblNoData.Text = "Sie haben die folgenden Vorgänge ausgewählt (Anzahl: " & tmpDataView.Count.ToString & ")."
                End If
            Else
                lblNoData.Text = "Es wurden " & tmpDataView.Count.ToString & " Vorgänge gefunden."
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

            If cmdBack.Visible Then
                DataGrid1.Columns(DataGrid1.Columns.Count - 1).Visible = True
                DataGrid1.Columns(DataGrid1.Columns.Count - 2).Visible = True
                Dim k As Int32
                For k = 1 To 5
                    DataGrid1.Columns(DataGrid1.Columns.Count - 2 - k).Visible = False
                Next
            End If

            Dim item As DataGridItem
            Dim cell As TableCell
            Dim chkBox As RadioButton
            Dim CheckBox As CheckBox
            Dim textbox As TextBox
            Dim control As Control
            Dim blnScriptFound As Boolean = False

            For Each item In DataGrid1.Items

                Dim strKontonummer As String = "Kontonummer = '" & item.Cells(0).Text & "'"
                Dim strTIDNR As String = Replace(item.Cells(2).Text, "&nbsp;", "")
                Dim blnFehler As Boolean
                cell = item.Cells(7)
                For Each control In cell.Controls
                    If TypeOf control Is CheckBox Then
                        CheckBox = CType(control, CheckBox)
                        blnFehler = CheckBox.Checked
                    End If
                Next

                For Each cell In item.Cells
                    For Each control In cell.Controls
                        If TypeOf control Is RadioButton Then
                            chkBox = CType(control, RadioButton)
                            Select Case chkBox.ID
                                Case "chkActionVERS"
                                    If strTIDNR.Length = 0 Then
                                        chkBox.Enabled = False
                                    End If
                                Case "chkActionCHAN"
                                    If Not blnFehler Then
                                        chkBox.Enabled = False
                                    End If
                            End Select
                            If cmdConfirm.Visible Then
                                chkBox.Enabled = False
                            End If
                        End If
                        If TypeOf control Is TextBox Then
                            textbox = CType(control, TextBox)
                            If cmdConfirm.Visible Then
                                textbox.Enabled = False
                            End If
                        End If
                    Next
                Next
            Next
        End If
        If CStr(objFahrzeuge.Fahrzeuge.Rows(0)("ProblemCode")) = "082" Then
            cmdSave.Visible = False
            DataGrid1.Columns(DataGrid1.Columns.Count - 3).Visible = False
            DataGrid1.Columns(DataGrid1.Columns.Count - 4).Visible = False
        End If
    End Sub

    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        DoSubmit()
    End Sub

    Private Sub cmdConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdConfirm.Click
        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Request.QueryString("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)

        objFahrzeuge.IstNullliste = True
        objFahrzeuge.StandardLogID = logApp.LogStandardIdentity
        objFahrzeuge.Change(Session("AppID").ToString, Session.SessionID, Me)

        Dim tmpDataView As New DataView()
        tmpDataView = objFahrzeuge.Fahrzeuge.DefaultView
        tmpDataView.RowFilter = "ActionNOTHING = 0"
        Dim strTemp As String = "Es wurden " & tmpDataView.Count.ToString & " Vorgänge beauftragt."
        Dim tblTemp As DataTable
        tblTemp = New DataTable()
        tblTemp.Columns.Add("Kontonummer", System.Type.GetType("System.String"))
        tblTemp.Columns.Add("Fahrgestellnummer", System.Type.GetType("System.String"))
        tblTemp.Columns.Add("Briefnummer", System.Type.GetType("System.String"))
        tblTemp.Columns.Add("Kennzeichen", System.Type.GetType("System.String"))
        tblTemp.Columns.Add("Label", System.Type.GetType("System.String"))
        tblTemp.Columns.Add("Modellbezeichnung", System.Type.GetType("System.String"))
        tblTemp.Columns.Add("Datum Nullliste", System.Type.GetType("System.String"))
        tblTemp.Columns.Add("Datum Briefeingang", System.Type.GetType("System.String"))
        tblTemp.Columns.Add("Datum Versand", System.Type.GetType("System.String"))
        tblTemp.Columns.Add("Problem", System.Type.GetType("System.String"))
        tblTemp.Columns.Add("Bemerkung", System.Type.GetType("System.String"))
        tblTemp.Columns.Add("Aktion", System.Type.GetType("System.String"))
        Dim i As Int32
        Dim rowNew As DataRow
        For i = 0 To tmpDataView.Count - 1
            rowNew = tblTemp.NewRow
            rowNew("Kontonummer") = tmpDataView.Item(i)("Kontonummer")
            rowNew("Fahrgestellnummer") = tmpDataView.Item(i)("Fahrgestellnummer")
            rowNew("Briefnummer") = tmpDataView.Item(i)("Briefnummer")
            rowNew("Kennzeichen") = tmpDataView.Item(i)("Kennzeichen")
            rowNew("Label") = tmpDataView.Item(i)("Label")
            rowNew("Modellbezeichnung") = tmpDataView.Item(i)("Modellbezeichnung")
            If IsDate(tmpDataView.Item(i)("Datum_Nullliste")) Then
                rowNew("Datum Nullliste") = CType(tmpDataView.Item(i)("Datum_Nullliste"), DateTime).ToShortDateString
            End If
            If IsDate(tmpDataView.Item(i)("Datum_Briefeingang")) Then
                rowNew("Datum Briefeingang") = CType(tmpDataView.Item(i)("Datum_Briefeingang"), DateTime).ToShortDateString
            End If
            If IsDate(tmpDataView.Item(i)("Datum_Versand")) Then
                rowNew("Datum Versand") = CType(tmpDataView.Item(i)("Datum_Versand"), DateTime).ToShortDateString
            End If
            rowNew("Problem") = tmpDataView.Item(i)("Problem")
            rowNew("Bemerkung") = tmpDataView.Item(i)("Bemerkung")
            rowNew("Aktion") = tmpDataView.Item(i)("Action")
            tblTemp.Rows.Add(rowNew)
        Next

        If Not objFahrzeuge.Status = 0 Then
            lblError.Text = objFahrzeuge.Message
            strTemp &= " Es traten Fehler auf."
        End If

        logApp.UpdateEntry("APP", Session("AppID").ToString, strTemp, tblTemp)
        cmdBack.Visible = True
        FillGrid(0)
        cmdConfirm.Visible = False
        cmdReset.Visible = False
    End Sub

    Private Sub cmdReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReset.Click
        cmdSave.Visible = True
        cmdConfirm.Visible = False
        cmdReset.Visible = False
        FillGrid(DataGrid1.CurrentPageIndex)
    End Sub

    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBack.Click
        Session("objFahrzeuge") = Nothing
        Response.Redirect("Change12.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Change12_2.aspx.vb $
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
' *****************  Version 7  *****************
' User: Uha          Date: 2.07.07    Time: 10:42
' Updated in $/CKG/Applications/AppCSC/AppCSCWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 6  *****************
' User: Uha          Date: 12.03.07   Time: 15:54
' Updated in $/CKG/Applications/AppCSC/AppCSCWeb/Forms
' Image-Pfade von Customize auf Images umgesetzt
' 
' *****************  Version 5  *****************
' User: Uha          Date: 7.03.07    Time: 9:28
' Updated in $/CKG/Applications/AppCSC/AppCSCWeb/Forms
' 
' ************************************************
