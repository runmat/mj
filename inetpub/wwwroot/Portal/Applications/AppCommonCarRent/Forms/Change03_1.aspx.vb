Imports CKG
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Change03_1
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
    Private objFahrzeuge As MindesthaltefristIngnorieren

    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ShowScript As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents cmdConfirm As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdReset As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdBack As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ucStyles As Styles
    Protected WithEvents imgbExcel As ImageButton

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If Session("objFahrzeuge") Is Nothing Then
                Response.Redirect("Change08.aspx?AppID=" & Session("AppID").ToString)
            End If

            objFahrzeuge = CType(Session("objFahrzeuge"), MindesthaltefristIngnorieren)

            If Not IsPostBack Then
                ddlPageSize.Items.Add("10")
                ddlPageSize.Items.Add("20")
                ddlPageSize.Items.Add("50")
                ddlPageSize.Items.Add("100")
                ddlPageSize.Items.Add("200")
                ddlPageSize.Items.Add("500")
                ddlPageSize.Items.Add("1000")
                ddlPageSize.SelectedIndex = 2
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
            lblError.Text = "Bitte wählen Sie erst Fahrzeuge aus."
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

        Dim control As Control
        Dim blnScriptFound As Boolean = False
        Dim intZaehl As Int32
        Dim intReturn As Int32 = 0

        Try
            For Each item In DataGrid1.Items
                intZaehl = 0
                Dim strEquipmentnummer As String = "Equipmentnummer='" & item.Cells(0).Text & "'"

                For Each cell In item.Cells
                    For Each control In cell.Controls
                        If TypeOf control Is RadioButton Then
                            chbox = CType(control, RadioButton)
                            objFahrzeuge.Fahrzeuge.AcceptChanges()
                            Dim tmpRows As DataRow()
                            tmpRows = objFahrzeuge.Fahrzeuge.Select(strEquipmentnummer)
                            If tmpRows.Length > 0 Then
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
                            Else
                                Throw New Exception("Equipmentnummer nicht gefunden!")
                            End If
                            objFahrzeuge.Fahrzeuge.AcceptChanges()
                        End If
                    Next
                    intZaehl += 1
                Next
            Next
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
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

            If DataGrid1.PageCount > 1 Then
                DataGrid1.PagerStyle.CssClass = "PagerStyle"
                DataGrid1.DataBind()
                DataGrid1.PagerStyle.Visible = True
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
            Dim control As Control
            Dim blnScriptFound As Boolean = False

            For Each item In DataGrid1.Items

                Dim strEquipmentnummer As String = "Equipmentnummer = '" & item.Cells(0).Text & "'"
             

                For Each cell In item.Cells
                    For Each control In cell.Controls
                        If TypeOf control Is RadioButton Then
                            chkBox = CType(control, RadioButton)
                          
                            If cmdConfirm.Visible Then
                                chkBox.Enabled = False
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

        Dim i As Int32

        objFahrzeuge.StandardLogID = logApp.LogStandardIdentity
        objFahrzeuge.Change(Session("AppID").ToString, Session.SessionID, Me)

        Dim tblTemp As DataTable
        tblTemp = objFahrzeuge.Fahrzeuge.Copy

        For i = tblTemp.Rows.Count - 1 To 0 Step -1
            If CType(tblTemp.Rows(i)("ActionNOTHING"), Boolean) Then
                tblTemp.Rows(i).Delete()
            End If
        Next
        tblTemp.Columns.Remove("ActionDELE")
        tblTemp.Columns.Remove("ActionNOTHING")

        Dim strTemp As String = "Es wurden " & tblTemp.Rows.Count.ToString & " Vorgänge beauftragt."

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
        Response.Redirect("Change03.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(0)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Protected Sub imgbExcel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbExcel.Click
        Try
            Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
            excelFactory.CreateDocumentAndSendAsResponse(strFileName, CType(Session("ExcelResult"), DataTable), Me.Page)
        Catch ex As Exception
            lblError.Text = "Beim erstellen des Exceldatei ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

End Class

' ************************************************
' $History: Change03_1.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 29.04.09   Time: 16:53
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' Warnungen
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 19.02.09   Time: 10:18
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA 2586/ 2588
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 16.02.09   Time: 14:19
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ITa 2586/2588 unfertig
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 13.02.09   Time: 14:37
' Created in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA 2586 torso
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 15:49
' Created in $/CKAG/Applications/appec/Forms
' 
' *****************  Version 6  *****************
' User: Uha          Date: 2.07.07    Time: 12:29
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 5  *****************
' User: Uha          Date: 22.05.07   Time: 13:31
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 4  *****************
' User: Uha          Date: 7.03.07    Time: 11:02
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' 
' ************************************************
