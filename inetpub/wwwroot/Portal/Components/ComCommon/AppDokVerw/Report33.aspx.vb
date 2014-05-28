Option Strict On
Option Explicit On
Option Compare Binary

Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Partial Public Class Report33
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private AppVerwaltung As AppDokVerw_03

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        cmdSave.Enabled = False
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        If Not IsPostBack Then
            trMemo.Visible = False
        End If

        GetAppIDFromQueryString(Me)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text
        m_App = New Base.Kernel.Security.App(m_User)
        AppVerwaltung = New AppDokVerw_03(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
        AppVerwaltung.AppID = Session("AppID").ToString
        AppVerwaltung.CreditControlArea = "ZDAD"
        AppVerwaltung.Show()

        If AppVerwaltung.Status = 0 Then

            If Not IsPostBack Then
                ddlPageSize.Items.Add("10")
                ddlPageSize.Items.Add("20")
                ddlPageSize.Items.Add("50")
                ddlPageSize.Items.Add("100")
                ddlPageSize.Items.Add("200")
                ddlPageSize.Items.Add("500")
                ddlPageSize.Items.Add("1000")
                ddlPageSize.SelectedIndex = 2


                FillGrid(AppVerwaltung, 0)


            End If
            cmdSave.Enabled = True
        Else
            lblError.Text = AppVerwaltung.Message
        End If

    End Sub

    'Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    'End Sub

    Private Sub FillGrid(ByVal objBank As AppDokVerw_03, ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If objBank.Status = 0 Then
            If objBank.Auftraege.Rows.Count = 0 Then
                DataGrid1.Visible = False
                lblNoData.Visible = True
                lblNoData.Text = "Keine Daten zur Anzeige gefunden."
            Else
                DataGrid1.Visible = True
                lblNoData.Visible = False
                ExcelImg.Visible = True
                lnkCreateExcel.Visible = True
                Dim tmpDataView As New DataView()
                tmpDataView = objBank.Auftraege.DefaultView
                Session("App_Auftraege") = objBank.Auftraege
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

                lblNoData.Text = "Es wurden " & tmpDataView.Count.ToString & " fällige Vorgänge gefunden."
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
                Dim button As LinkButton
                Dim textbox As TextBox
                Dim control As Control
                Dim blnScriptFound As Boolean = False
                Dim intZaehl As Int32
                Dim hyperlink As HyperLink

                Dim strHistoryLink As String = ""
                If m_User.Applications.Select("AppName = 'Report46'").Length > 0 Then

                    strHistoryLink = m_User.Applications.Select("AppName = 'Report46'")(0)("AppUrl").ToString
                    strHistoryLink = strHistoryLink & "?AppID=" & m_User.Applications.Select("AppName = 'Report46'")(0)("AppID").ToString & "&VIN="
                End If

                For Each item In DataGrid1.Items
                    cell = item.Cells(0)
                    Dim strVertragsnummer As String = ""
                    Dim LabelNummer As Label
                    For Each control In cell.Controls

                        If TypeOf control Is Label Then
                            LabelNummer = CType(control, Label)
                            If LabelNummer.ID = "lblVertrag" Then
                                strVertragsnummer = LabelNummer.Text
                                Exit For
                            End If
                        End If
                    Next
                    For intZaehl = 1 To item.Cells.Count - 1
                        cell = item.Cells(intZaehl)
                        For Each control In cell.Controls
                            If TypeOf control Is LinkButton Then
                                button = CType(control, LinkButton)
                                If button.CommandName = "Speichern" Then
                                    button.Attributes.Add("onClick", "if (!FaelligkeitConfirm('" & strVertragsnummer & "')) return false;")
                                    button.Attributes.Add("class", "StandardButtonTable")
                                    blnScriptFound = True
                                End If
                                If button.CommandName = "Aendern" Then
                                    button.Attributes.Add("onClick", "FaelligkeitEdit('" & strVertragsnummer & "');")
                                    button.Attributes.Add("class", "StandardButtonTable")
                                    blnScriptFound = True
                                End If
                            End If

                            If TypeOf control Is HyperLink Then
                                hyperlink = CType(control, HyperLink)
                                Select Case hyperlink.ID
                                    Case "VIN"
                                        If strHistoryLink.Length > 0 Then
                                            hyperlink.NavigateUrl = "../../" & strHistoryLink & hyperlink.NavigateUrl
                                        Else
                                            hyperlink.NavigateUrl = ""
                                        End If
                                End Select
                            End If

                        Next
                    Next

                    For intZaehl = 1 To item.Cells.Count - 1
                        cell = item.Cells(intZaehl)
                        For Each control In cell.Controls
                            If TypeOf control Is TextBox Then
                                textbox = CType(control, TextBox)
                                If textbox.ID = "txtFaelligkeit" Then
                                    textbox.ID = "txt" & strVertragsnummer
                                End If
                            End If
                        Next

                    Next


                Next
               
                If blnScriptFound Then
                    ShowScript.Visible = True
                End If
            End If
        Else
            lblError.Text = objBank.Message
            lblNoData.Visible = True
        End If
    End Sub



    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        FillGrid(AppVerwaltung, e.NewPageIndex)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(AppVerwaltung, DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    'Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
    '    FillGrid(AppVerwaltung, 0)
    'End Sub

    Private Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.ItemCommand
        Dim dr As DataRow
        For Each dr In AppVerwaltung.Auftraege.Rows
            dr("DatumAendern") = False
        Next
        If e.CommandName = "WriteMemo" Then
            'Memoeditierbereich oeffnen
            trMemo.Visible = True
            'Vertragsnummer in Ueberschrift
            Dim ctl As Control
            Dim lnk As LinkButton
            Dim LabelNummer As Label
            For Each ctl In e.Item.Cells(0).Controls
                Try
                    LabelNummer = CType(ctl, Label)
                    If LabelNummer.ID = "lblVertrag" Then
                        lblVertragsNummer.Text = LabelNummer.Text
                        Exit For
                    End If
                Catch
                End Try
            Next
            'evtl. vorhandenen Memotext in Textbox
            For Each ctl In e.Item.Cells(10).Controls
                Try
                    lnk = CType(ctl, LinkButton)
                    If Not lnk Is Nothing Then
                        Memo.Text = lnk.ToolTip
                        Exit For
                    End If
                Catch
                End Try
            Next
        ElseIf e.CommandName = "Speichern" Then
            'Datum pruefen
            If IsDate(Request.Form("txtFaelligkeit")) Then
                For Each dr In AppVerwaltung.Auftraege.Select(String.Format("Vertragsnummer={0}", Request.Form("txtVertragsnummer")))
                    dr("fällig am") = Request.Form("txtFaelligkeit")
                Next
                AppVerwaltung.Change()
                Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)

                If AppVerwaltung.Status = 0 Then
                    'AENDERUNGEN LOGGEN!
                    logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, CStr(Request.Form("txtVertragsnummer")), "Fälligkeitsdatum für Vertrag-Nr. " & CStr(Request.Form("txtVertragsnummer")) & " auf den " & CStr(Request.Form("txtFaelligkeit")) & " festgesetzt.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
                    logApp.WriteStandardDataAccessSAP(AppVerwaltung.IDSAP)

                    For Each dr In AppVerwaltung.Auftraege.Select(String.Format("Vertragsnummer={0}", Request.Form("txtVertragsnummer")))
                        dr("DatumAendern") = False
                    Next
                    'DataGrid neu befuellen
                    FillGrid(AppVerwaltung, DataGrid1.CurrentPageIndex)
                Else
                    'AENDERUNGEN LOGGEN!
                    logApp.WriteEntry("ERR", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, CStr(Request.Form("txtVertragsnummer")), "Fehler beim Festlegen des Fälligkeitsdatum für Vertrag-Nr. " & CStr(Request.Form("txtVertragsnummer")) & " auf den " & CStr(Request.Form("txtFaelligkeit")) & ". (" & AppVerwaltung.Message & ")", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
                    logApp.WriteStandardDataAccessSAP(AppVerwaltung.IDSAP)

                    'Fehler ausgeben
                    lblError.Text = AppVerwaltung.Message
                End If
            Else
                lblError.Text = "Bitte geben Sie ein Datum im Format DD.MM.JJJJ ein."
            End If
        ElseIf e.CommandName = "Aendern" Then
            For Each dr In AppVerwaltung.Auftraege.Select(String.Format("Vertragsnummer={0}", Request.Form("txtVertragsnummer")))
                dr("DatumAendern") = True
            Next

            Literal1.Text = "						<script language=""Javascript"">" & vbCrLf
            Literal1.Text &= "						  <!-- //" & vbCrLf
            Literal1.Text &= "						    window.document.location.href = ""#" & CStr(Request.Form("txtVertragsnummer")) & """;" & vbCrLf
            Literal1.Text &= "						  //-->" & vbCrLf
            Literal1.Text &= "						</script>" & vbCrLf

            'DataGrid neu befuellen
            FillGrid(AppVerwaltung, DataGrid1.CurrentPageIndex)
        End If
    End Sub

    Private Sub SaveMemo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveMemo.Click
        'Memo speichern
        Dim dr As DataRow
        For Each dr In AppVerwaltung.Auftraege.Select(String.Format("Vertragsnummer='{0}'", lblVertragsNummer.Text))
            dr("Memo") = Memo.Text
        Next
        AppVerwaltung.Change()
        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)

        If AppVerwaltung.Status = 0 Then
            'AENDERUNGEN LOGGEN!
            logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, lblVertragsNummer.Text, "Memo (" & Memo.Text & ") für Vertrag-Nr. " & lblVertragsNummer.Text & " gespeichert.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
            logApp.WriteStandardDataAccessSAP(AppVerwaltung.IDSAP)

            'DataGrid neu befuellen
            FillGrid(AppVerwaltung, DataGrid1.CurrentPageIndex)
        Else
            'AENDERUNGEN LOGGEN!
            logApp.WriteEntry("ERR", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, lblVertragsNummer.Text, "Fehler beim Speichern des Memos (" & Memo.Text & ") für Vertrag-Nr. " & lblVertragsNummer.Text & ". (" & AppVerwaltung.Message & ")", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
            logApp.WriteStandardDataAccessSAP(AppVerwaltung.IDSAP)

            'Fehler ausgeben
            lblError.Text = AppVerwaltung.Message
        End If

        'Editierbereich wieder schliessen
        CancelMemo_Click(Nothing, Nothing)
    End Sub

    Private Sub CancelMemo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CancelMemo.Click
        'Felder loeschen, Editierbereich schliessen
        lblVertragsNummer.Text = ""
        Memo.Text = ""
        trMemo.Visible = False
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Protected Sub lnkCreateExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel.Click
        'Dim control As New Control
        Dim tblTranslations As DataTable
        Dim tblTemp As DataTable = AppVerwaltung.ResultExcel.Copy
        Dim AppURL As String
        Dim col As DataGridColumn
        Dim col2 As DataColumn
        Dim bVisibility As Integer
        Dim i As Integer
        Dim sColName As String = ""

        AppURL = Replace(Request.Url.LocalPath, "/Portal", "..")
        tblTranslations = CType(Session(AppURL), DataTable)
        For Each col In DataGrid1.Columns
            For i = tblTemp.Columns.Count - 1 To 0 Step -1
                bVisibility = 0
                col2 = tblTemp.Columns(i)
                If col2.ColumnName.ToUpper = col.SortExpression.ToUpper Then
                    sColName = TranslateColLbtn(DataGrid1, tblTranslations, col.HeaderText, bVisibility)
                    If bVisibility = 0 Then
                        tblTemp.Columns.Remove(col2)
                    ElseIf sColName.Length > 0 Then
                        col2.ColumnName = sColName
                    End If
                End If
            Next
            tblTemp.AcceptChanges()
        Next
        Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
        excelFactory.CreateDocumentAndSendAsResponse(strFileName, tblTemp, Me)

    End Sub
End Class
' ************************************************
' $History: Report33.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 5.06.09    Time: 13:51
' Updated in $/CKAG/Components/ComCommon/AppDokVerw
' Try Catch entfernt wenn m�glich
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 23.07.08   Time: 17:04
' Updated in $/CKAG/Components/ComCommon/AppDokVerw
' 
' *****************  Version 1  *****************
' User: Rudolpho     Date: 15.07.08   Time: 14:20
' Created in $/CKAG/Components/ComCommon/AppDokVerw
' ITA: 2081
' 
' ************************************************
