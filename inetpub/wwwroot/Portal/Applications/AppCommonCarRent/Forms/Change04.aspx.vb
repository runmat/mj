Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Partial Public Class Change04
    Inherits System.Web.UI.Page

    Private m_User As Security.User
    Private m_App As Security.App
    Private objVersandfreigabe As Versandfreigabe
    Private blnGridFilled As Boolean
    Private blnInputChecked As Boolean
    Private blnFilterSelected As Boolean

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        blnGridFilled = False
        blnFilterSelected = False
        Try
            lblHead.Text = "Versandfreigabe"
            ucStyles.TitleText = lblHead.Text
            m_App = New Security.App(m_User)

            If Not IsPostBack Then
                ShowInput()
            End If

            If Session("App_Versandfreigabe") Is Nothing Then
                Initialload()
                blnInputChecked = True
            Else
                objVersandfreigabe = CType(Session("App_Versandfreigabe"), Versandfreigabe)
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
        Dim logApp As New Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)

        Try
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            objVersandfreigabe = New Versandfreigabe(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, strFileName)

            lblError.Text = ""

            If lblError.Text.Length = 0 Then
                objVersandfreigabe.Show(Session("AppID").ToString, Session.SessionID, Me)

                If Not objVersandfreigabe.Status = 0 Then
                    lblError.Text = "Fehler: " & objVersandfreigabe.Message
                Else
                    If objVersandfreigabe.Result.Rows.Count = 0 Then
                        lblError.Text = "Keine Daten vorhanden."
                    Else
                        imgbExcel.Visible = True
                        lnkCreateExcel.Visible = True
                        logApp.UpdateEntry("APP", Session("AppID").ToString, "Freigabe Versand bei Zahlungseingang")
                    End If
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Beim Lesen der Daten ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try

        Session.Add("App_Versandfreigabe", objVersandfreigabe)

        FillListbox()
        ShowInput()
    End Sub

    Private Sub FillListbox(Optional ByVal strSetIndex As String = "")
        Dim vwKunde As DataView

        vwKunde = objVersandfreigabe.Kunden.DefaultView

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

        If IsNothing(objVersandfreigabe.Result) = True Then
            imgbExcel.Visible = False
            lnkCreateExcel.Visible = False
            Me.lblError.Text = "Keine Daten zur Anzeige vorhanden."
            Exit Sub
        End If


        tmpDataView = objVersandfreigabe.Result.DefaultView
        If blnFilterSelected Then
            tmpDataView.RowFilter = "Selected = 1"
        Else
            objVersandfreigabe.Empfaenger = ListBox1.SelectedItem.Value
            tmpDataView.RowFilter = "Zzkunnr_Zs = '" & objVersandfreigabe.Empfaenger & "'"
            blnFilterSelected = True
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
            DataGrid1.BackColor = Drawing.Color.FromArgb(244, 164, 96)

            'erstmal ausblenden sind keine Daten enthlaten laut KGA 11.03.2009
            DataGrid1.Columns(3).Visible = False
            DataGrid1.Columns(4).Visible = False
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
                    tmpRows = objVersandfreigabe.Result.Select(strZZFAHRG)
                    tmpRows(0)("Selected") = chkBox.Checked
                End If
            Next
            cell = item.Cells(5)

            For Each control In cell.Controls
                If TypeOf control Is CheckBox Then
                    chkBox = CType(control, CheckBox)
                    tmpRows = objVersandfreigabe.Result.Select(strZZFAHRG)

                    tmpRows(0)("ZZBEZAHLT") = BooltoX(chkBox.Checked)
                End If
            Next
            cell = item.Cells(6)

            For Each control In cell.Controls
                If TypeOf control Is CheckBox Then
                    chkBox = CType(control, CheckBox)
                    tmpRows = objVersandfreigabe.Result.Select(strZZFAHRG)
                    tmpRows(0)("ZZAKTSPERRE") = BooltoX(chkBox.Checked)
                End If
            Next
        Next

        If IsNothing(objVersandfreigabe.Result) = True Then
            Me.lblError.Text = "Keine Daten zur Anzeige vorhanden."
            Exit Sub
        End If

        Session("App_Versandfreigabe") = objVersandfreigabe
    End Sub
    Private Function BooltoX(ByVal bInput As Boolean) As String
        If bInput = True Then
            Return "X"
        Else
            Return ""
        End If
    End Function


    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        Me.lblError.Text = ""
        If IsNothing(objVersandfreigabe.Result) = True Then
            Me.lblError.Text = "Keine Daten zur Anzeige vorhanden."
            Exit Sub
        End If

        CheckInput()

        Dim tmpRows() As DataRow = objVersandfreigabe.Result.Select("Selected = 1")
        Dim Fehler As String = ""

        If tmpRows.Length = 0 Then
            Me.lblError.Text = "Keine Fahrzeuge ausgewählt."
            Exit Sub
        Else

            For Each tmpRow As DataRow In tmpRows
                If tmpRow("ZZBEZAHLT").ToString = "X" AndAlso Not tmpRow("ZZAKTSPERRE").ToString = "X" Then
                Else
                    Fehler += tmpRow("CHASSIS_NUM").ToString & " <br />"
                End If
            Next

        End If
        If Fehler.Length > 0 Then
            lblError.Text = "Folgende Fahrzeuge können nicht freigegeben werden, da sie gesperrt oder nicht als bezahlt gekennzeichnet ist! <br />" & Fehler
            Exit Sub
        End If

        HideInput()
        imgbExcel.Visible = False
        lnkCreateExcel.Visible = False
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
        'If Not Session("lnkExcel") Is Nothing AndAlso CStr(Session("lnkExcel")).Length > 0 Then
        '    imgbExcel.Visible = True
        '    lnkCreateExcel.Visible = True
        'Else
        '    imgbExcel.Visible = False
        '    lnkCreateExcel.Visible = False
        'End If
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

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        FillGrid(e.NewPageIndex)
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
        Dim logApp As New Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)

        Try
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            objVersandfreigabe.ExcelFileName = strFileName

            lblError.Text = ""

            If lblError.Text.Length = 0 Then
                objVersandfreigabe.Change(Session("AppID").ToString, Session.SessionID, Me)

                If Not objVersandfreigabe.Status = 0 Then
                    lblError.Text = "Fehler: " & objVersandfreigabe.Message
                Else
                    lblError.Text = "Datenübernahme erfolgreich."
                    imgbExcel.Visible = True
                    lnkCreateExcel.Visible = True
                    logApp.UpdateEntry("APP", Session("AppID").ToString, "Freigabe Versand bei Zahlungseingang")
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Beim Lesen der Daten ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try

        Session("App_Versandfreigabe") = objVersandfreigabe

        blnInputChecked = True
        cmdConfirm.Enabled = False
        blnFilterSelected = True
    End Sub
    Private Sub CreateExcel()
        Dim ExcelTable As DataTable
        If Not (Session("App_Versandfreigabe") Is Nothing) Then

            ExcelTable = objVersandfreigabe.ResultExcel
            Try
                Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
                Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName

                excelFactory.CreateDocumentAndSendAsResponse(strFileName, ExcelTable, Me.Page)

            Catch ex As Exception
                lblError.Text = "Fehler beim Erstellen der Excel-Datei: " + ex.Message
            End Try
        End If
    End Sub
    Protected Sub imgbExcel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbExcel.Click
        CreateExcel()
    End Sub

    Protected Sub lnkCreateExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel.Click
        CreateExcel()
    End Sub

    Protected Sub lb_Auswahl_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_Auswahl.Click
        Session("App_Versandfreigabe") = Nothing
        Response.Redirect("Auswahl.aspx?AppID=" & Session("AppID").ToString)
    End Sub
End Class
' ************************************************
' $History: Change04.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 11.03.09   Time: 14:22
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA: 2671
' 
' ************************************************
