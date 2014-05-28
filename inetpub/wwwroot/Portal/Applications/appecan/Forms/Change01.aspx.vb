Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Change01
    Inherits System.Web.UI.Page
    '##### ECAN Report "Freigabe Versand bei Zahlungseingang"
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
    Private objHandler As ECAN_01
    Private blnGridFilled As Boolean
    Private blnInputChecked As Boolean
    Private blnFilterSelected As Boolean

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents cmdCreate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ImageButton1 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ListBox1 As System.Web.UI.WebControls.ListBox
    Protected WithEvents cmdConfirm As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdBack As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblDownloadTip As System.Web.UI.WebControls.Label
    Protected WithEvents lnkExcel As System.Web.UI.WebControls.HyperLink
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        blnGridFilled = False
        blnFilterSelected = False
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If Not IsPostBack Then
                ShowInput()
            End If

            If Session("objHandler") Is Nothing Then
                Initialload()
                blnInputChecked = True
            Else
                objHandler = CType(Session("objHandler"), ECAN_01)
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub ShowInput()
        DataGrid1.Columns(0).Visible = True
        DataGrid1.Columns(1).Visible = False
        ListBox1.Visible = True
    End Sub

    Private Sub HideInput()
        DataGrid1.Columns(0).Visible = False
        DataGrid1.Columns(1).Visible = True
        ListBox1.Visible = False
    End Sub

    Private Sub Initialload()
        Session("lnkExcel") = ""
        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)

        Try
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            objHandler = New ECAN_01(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, strFileName)

            lblError.Text = ""

            If lblError.Text.Length = 0 Then
                objHandler.Show(Session("AppID").ToString, Session.SessionID.ToString, Me)

                If Not objHandler.Status = 0 Then
                    lblError.Text = "Fehler: " & objHandler.Message
                Else
                    If objHandler.Result.Rows.Count = 0 Then
                        lblError.Text = "Keine Daten vorhanden."
                    Else

                        Dim excelFactory As New Base.Kernel.DocumentGeneration.ExcelDocumentFactory()

                        Try
                            excelFactory.CreateDocumentAndWriteToFilesystem(ConfigurationManager.AppSettings("ExcelPath") & strFileName, objHandler.ResultExcel, Me)
                            Session("lnkExcel") = "/Portal/Temp/Excel/" & strFileName
                        Catch
                        End Try
                        logApp.UpdateEntry("APP", Session("AppID").ToString, "Freigabe Versand bei Zahlungseingang")
                    End If
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Beim Lesen der Daten ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try

        Session.Add("objHandler", objHandler)

        FillListbox()
        ShowInput()
    End Sub

    Private Sub FillListbox(Optional ByVal strSetIndex As String = "")
        Dim vwKunde As DataView

        vwKunde = objHandler.Kunden.DefaultView

        If vwKunde.Count > 0 Then
            vwKunde.Sort = "Kundennummer ASC"
            With ListBox1
                .DataSource = vwKunde
                .DataTextField = "Name"
                .DataValueField = "Kundennummer"
                .DataBind()
            End With
            If strSetIndex = String.Empty Then
                ListBox1.Items(0).Selected = True
            Else
                ListBox1.Items.FindByValue(strSetIndex).Selected = True
            End If
        End If
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        blnGridFilled = True

        Dim tmpDataView As New DataView()

        If IsNothing(objHandler.Result) = True Then
            Me.lblError.Text = "Keine Daten zur Anzeige vorhanden."
            Exit Sub
        End If


        tmpDataView = objHandler.Result.DefaultView
        If blnFilterSelected Then
            tmpDataView.RowFilter = "Selected = 1"
        Else
            objHandler.Empfaenger = ListBox1.SelectedItem.Value
            tmpDataView.RowFilter = "Zzkunnr_Zs = '" & objHandler.Empfaenger & "'"
        End If

        If tmpDataView.Count = 0 Then
            DataGrid1.Visible = False
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

            DataGrid1.DataSource = tmpDataView
            DataGrid1.DataBind()

            If DataGrid1.PageCount > 1 Then
                DataGrid1.PagerStyle.CssClass = "PagerStyle"
                DataGrid1.DataBind()
                DataGrid1.PagerStyle.Visible = True
            Else
                DataGrid1.PagerStyle.Visible = False
            End If

        End If
    End Sub

    Private Sub CheckInput()
        blnInputChecked = True

        Dim item As DataGridItem
        Dim cell As TableCell
        Dim chkBox As CheckBox
        Dim control As Control
        Dim tmpRows As DataRow()

        For Each item In DataGrid1.Items
            Dim strZZFAHRG As String = "Chassis_Num = '" & item.Cells(2).Text & "'"

            cell = item.Cells(0)

            For Each control In cell.Controls
                If TypeOf control Is CheckBox Then
                    chkBox = CType(control, CheckBox)
                    tmpRows = objHandler.Result.Select(strZZFAHRG)
                    tmpRows(0)("Selected") = chkBox.Checked
                End If
            Next
        Next

        If IsNothing(objHandler.Result) = True Then
            Me.lblError.Text = "Keine Daten zur Anzeige vorhanden."
            Exit Sub
        End If

        Session("objHandler") = objHandler
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        If IsNothing(objHandler.Result) = True Then
            Me.lblError.Text = "Keine Daten zur Anzeige vorhanden."
            Exit Sub
        End If

        CheckInput()
        Dim tmpDataView As DataView = objHandler.Result.DefaultView
        tmpDataView.RowFilter = "Selected = 1"

        If tmpDataView.Count = 0 Then
            Me.lblError.Text = "Keine Fahrzeuge ausgewählt."
            Exit Sub
        End If

        HideInput()
        cmdCreate.Visible = False
        cmdBack.Visible = True
        cmdConfirm.Visible = True
        blnInputChecked = True
        blnFilterSelected = True
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        If Not blnInputChecked Then
            CheckInput()
        End If
        If Not blnGridFilled Then
            FillGrid(0)
        End If
        If Not Session("lnkExcel") Is Nothing AndAlso CStr(Session("lnkExcel")).Length > 0 Then
            lnkExcel.NavigateUrl = CStr(Session("lnkExcel"))
            lnkExcel.Visible = True
            lblDownloadTip.Visible = True
        Else
            lnkExcel.Visible = False
            lblDownloadTip.Visible = False
        End If
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListBox1.SelectedIndexChanged

    End Sub

    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBack.Click
        If cmdConfirm.Visible And cmdConfirm.Enabled Then
            ShowInput()
            cmdCreate.Visible = True
            cmdBack.Visible = False
            cmdConfirm.Visible = False
            blnInputChecked = True
        Else
            Response.Redirect(Me.Request.Url.ToString)
        End If
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        If cmdBack.Visible Then
            blnInputChecked = True
            blnFilterSelected = True
        End If
        FillGrid(0, e.SortExpression)
    End Sub

    Private Sub cmdConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdConfirm.Click
        Session("lnkExcel") = ""
        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)

        Try
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            objHandler.ExcelFileName = strFileName

            lblError.Text = ""

            If lblError.Text.Length = 0 Then
                objHandler.Change(Session("AppID").ToString, Session.SessionID.ToString, Me)

                If Not objHandler.Status = 0 Then
                    lblError.Text = "Fehler: " & objHandler.Message
                Else
                    lblError.Text = "Datenübernahme erfolgreich."

                    Dim excelFactory As New Base.Kernel.DocumentGeneration.ExcelDocumentFactory()

                    Try
                        excelFactory.CreateDocumentAndWriteToFilesystem(ConfigurationManager.AppSettings("ExcelPath") & strFileName, objHandler.ResultExcel, Me)
                        Session("lnkExcel") = "/Portal/Temp/Excel/" & strFileName
                    Catch
                    End Try
                    logApp.UpdateEntry("APP", Session("AppID").ToString, "Freigabe Versand bei Zahlungseingang")
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Beim Lesen der Daten ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try

        Session("objHandler") = objHandler

        blnInputChecked = True
        cmdConfirm.Enabled = False
        blnFilterSelected = True
    End Sub
End Class

' ************************************************
' $History: Change01.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 4.02.10    Time: 16:49
' Updated in $/CKAG/Applications/appecan/Forms
' ITA: 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 30.04.09   Time: 10:57
' Updated in $/CKAG/Applications/appecan/Forms
' ITA 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:07
' Created in $/CKAG/Applications/appecan/Forms
' 
' *****************  Version 5  *****************
' User: Uha          Date: 20.08.07   Time: 17:11
' Updated in $/CKG/Applications/AppECAN/AppECANWeb/Forms
' Bugfix in Change01.aspx: Speicherbutton muss (ohne Autopostback im
' Datagrid) immer offen sein.
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 30.07.07   Time: 16:50
' Updated in $/CKG/Applications/AppECAN/AppECANWeb/Forms
' 
' *****************  Version 3  *****************
' User: Uha          Date: 9.07.07    Time: 13:26
' Updated in $/CKG/Applications/AppECAN/AppECANWeb/Forms
' Bugfixing - Change01 ("Freigabe Versand bei Zahlungseingang")
' 
' *****************  Version 2  *****************
' User: Uha          Date: 9.07.07    Time: 12:57
' Updated in $/CKG/Applications/AppECAN/AppECANWeb/Forms
' Change01 ("Freigabe Versand bei Zahlungseingang") zum Testen bereit
' 
' *****************  Version 1  *****************
' User: Uha          Date: 5.07.07    Time: 18:33
' Created in $/CKG/Applications/AppECAN/AppECANWeb/Forms
' Erster Zwischenstand für "Freigabe Versand bei Zahlungseingang" -
' Wegschreiben fehlt noch
' 
' ************************************************
