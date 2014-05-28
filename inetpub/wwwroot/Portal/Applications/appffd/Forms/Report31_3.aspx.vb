Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business

Public Class Report31_3
    Inherits System.Web.UI.Page

    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private objSuche As Search
    Private objFDDBank As BankBaseCredit
    Private objFDDBank4 As FDD_Bank_4

    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents Kopfdaten1 As Kopfdaten
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkKreditlimit As System.Web.UI.WebControls.HyperLink
    Protected WithEvents ucHeader As Header
    Protected WithEvents lnkVertragssuche As System.Web.UI.WebControls.HyperLink
    Protected WithEvents ShowScript As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents Literal1 As System.Web.UI.WebControls.Literal
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lnkExcel As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblDownloadTip As System.Web.UI.WebControls.Label
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblVertragsNummer As System.Web.UI.WebControls.Label
    Protected WithEvents Memo As System.Web.UI.WebControls.TextBox
    Protected WithEvents SaveMemo As System.Web.UI.WebControls.LinkButton
    Protected WithEvents CancelMemo As System.Web.UI.WebControls.LinkButton
    Protected WithEvents trMemo As System.Web.UI.HtmlControls.HtmlTableRow
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
        cmdSave.Enabled = False
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        If Not IsPostBack Then
            trMemo.Visible = False
        End If

        Try
            If (Session("objSuche") Is Nothing) Then
                Response.Redirect("Report31.aspx?AppID=" & Session("AppID").ToString)
            End If
            If (Session("SelectedDealer").ToString.Length = 0) Or (Session("objFDDBank") Is Nothing) Or (Session("objFDDBank4") Is Nothing) Then
                Response.Redirect("Report31_2.aspx?AppID=" & Session("AppID").ToString)
            End If

            objSuche = CType(Session("objSuche"), Search)
            If objSuche.LeseHaendlerSAP_Einzeln(Session("AppID").ToString, Session.SessionID.ToString, Session("SelectedDealer").ToString) Then
                Kopfdaten1.HaendlerNummer = objSuche.REFERENZ
                Dim strTemp As String = objSuche.NAME
                If objSuche.NAME_2.Length > 0 Then
                    strTemp &= "<br>" & objSuche.NAME_2
                End If
                Kopfdaten1.HaendlerName = strTemp
                Kopfdaten1.Adresse = objSuche.COUNTRYISO & " - " & objSuche.POSTL_CODE & " " & objSuche.CITY & "<br>" & objSuche.STREET
            Else
                lblError.Text = "Fehler bei der Ermittlung der Händlerdaten.<br>(" & objSuche.ErrorMessage & ")"
            End If

            lnkVertragssuche.NavigateUrl = "Report31_2.aspx?AppID=" & Session("AppID").ToString & "&ShowAll=True"
            lnkKreditlimit.NavigateUrl = "Report31.aspx?AppID=" & Session("AppID").ToString
            If m_User.Organization.AllOrganizations Then
                lnkKreditlimit.Visible = True
            Else
                lnkKreditlimit.Visible = False
            End If

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text

            m_App = New Base.Kernel.Security.App(m_User)

            objFDDBank = CType(Session("objFDDBank"), BankBaseCredit)
            objFDDBank4 = CType(Session("objFDDBank4"), FDD_Bank_4)
            If objFDDBank.Status = 0 And objFDDBank4.Status = 0 Then
                If Not IsPostBack Then
                    ddlPageSize.Items.Add("10")
                    ddlPageSize.Items.Add("20")
                    ddlPageSize.Items.Add("50")
                    ddlPageSize.Items.Add("100")
                    ddlPageSize.Items.Add("200")
                    ddlPageSize.Items.Add("500")
                    ddlPageSize.Items.Add("1000")
                    ddlPageSize.SelectedIndex = 2

                    Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
                    Try
                        Excel.ExcelExport.WriteExcel(objFDDBank4.ResultExcel, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                        lnkExcel.NavigateUrl = "../../../Temp/Excel/" & strFileName
                        lnkExcel.Visible = True
                        lblDownloadTip.Visible = True
                    Catch
                    End Try

                    FillGrid(objFDDBank4, 0)
                    Kopfdaten1.Kontingente = objFDDBank.Kontingente
                End If
                cmdSave.Enabled = True
            Else
                lblError.Text = objFDDBank.Message & "<br>" & objFDDBank4.Message
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub FillGrid(ByVal objBank As FDD_Bank_4, ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If objBank.Status = 0 Then


            If objBank.Auftraege.Rows.Count = 0 Then
                DataGrid1.Visible = False
                lblNoData.Visible = True
                lblNoData.Text = "Keine Daten zur Anzeige gefunden."
            Else
                DataGrid1.Visible = True
                lblNoData.Visible = False

                Dim tmpDataView As New DataView()
                tmpDataView = objBank.Auftraege.DefaultView

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

                For Each item In DataGrid1.Items
                    cell = item.Cells(0)
                    Dim strVertragsnummer As String
                    strVertragsnummer = cell.Text

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
                    cell = item.Cells(10)
                    If IsNumeric(cell.Text) Then
                        cell.Text = Format(CDec(cell.Text), "##,##0.00 €")
                    End If
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
        FillGrid(objFDDBank4, e.NewPageIndex)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(objFDDBank4, DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(objFDDBank4, 0)
    End Sub

    Private Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.ItemCommand
        Dim dr As DataRow
        For Each dr In objFDDBank4.Auftraege.Rows
            dr("DatumAendern") = False
        Next
        If e.CommandName = "WriteMemo" Then
            'Memoeditierbereich oeffnen
            trMemo.Visible = True
            'Vertragsnummer in Ueberschrift
            lblVertragsNummer.Text = e.Item.Cells(0).Text
            'evtl. vorhandenen Memotext in Textbox
            Dim ctl As Control
            Dim lnk As LinkButton
            For Each ctl In e.Item.Cells(6).Controls
                Try
                    lnk = DirectCast(ctl, LinkButton)
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
                For Each dr In objFDDBank4.Auftraege.Select(String.Format("Vertragsnummer={0}", Request.Form("txtVertragsnummer")))
                    dr("fällig am") = Request.Form("txtFaelligkeit")
                Next
                objFDDBank4.Change(Session("AppID").ToString, Session.SessionID, Me)
                Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)

                If objFDDBank4.Status = 0 Then
                    'AENDERUNGEN LOGGEN!
                    logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, CStr(Request.Form("txtVertragsnummer")), "Fälligkeitsdatum für Vertrag-Nr. " & CStr(Request.Form("txtVertragsnummer")) & " auf den " & CStr(Request.Form("txtFaelligkeit")) & " festgesetzt.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
                    logApp.WriteStandardDataAccessSAP(objFDDBank4.IDSAP)

                    For Each dr In objFDDBank4.Auftraege.Select(String.Format("Vertragsnummer={0}", Request.Form("txtVertragsnummer")))
                        dr("DatumAendern") = False
                    Next
                    'DataGrid neu befuellen
                    FillGrid(objFDDBank4, DataGrid1.CurrentPageIndex)
                Else
                    'AENDERUNGEN LOGGEN!
                    logApp.WriteEntry("ERR", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, CStr(Request.Form("txtVertragsnummer")), "Fehler beim Festlegen des Fälligkeitsdatum für Vertrag-Nr. " & CStr(Request.Form("txtVertragsnummer")) & " auf den " & CStr(Request.Form("txtFaelligkeit")) & ". (" & objFDDBank4.Message & ")", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
                    logApp.WriteStandardDataAccessSAP(objFDDBank4.IDSAP)

                    'Fehler ausgeben
                    lblError.Text = objFDDBank4.Message
                End If
            Else
                lblError.Text = "Bitte geben Sie ein Datum im Format DD.MM.JJJJ ein."
            End If
        ElseIf e.CommandName = "Aendern" Then
            For Each dr In objFDDBank4.Auftraege.Select(String.Format("Vertragsnummer={0}", Request.Form("txtVertragsnummer")))
                dr("DatumAendern") = True
            Next

            Literal1.Text = "						<script language=""Javascript"">" & vbCrLf
            Literal1.Text &= "						  <!-- //" & vbCrLf
            Literal1.Text &= "						    window.document.location.href = ""#" & CStr(Request.Form("txtVertragsnummer")) & """;" & vbCrLf
            Literal1.Text &= "						  //-->" & vbCrLf
            Literal1.Text &= "						</script>" & vbCrLf

            'DataGrid neu befuellen
            FillGrid(objFDDBank4, DataGrid1.CurrentPageIndex)
        End If
    End Sub

    Private Sub SaveMemo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveMemo.Click
        'Memo speichern
        Dim dr As DataRow
        For Each dr In objFDDBank4.Auftraege.Select(String.Format("Vertragsnummer={0}", lblVertragsNummer.Text))
            dr("Memo") = Memo.Text
        Next
        objFDDBank4.Change(Session("AppID").ToString, Session.SessionID, Me)
        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)

        If objFDDBank4.Status = 0 Then
            'AENDERUNGEN LOGGEN!
            logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, lblVertragsNummer.Text, "Memo (" & Memo.Text & ") für Vertrag-Nr. " & lblVertragsNummer.Text & " gespeichert.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
            logApp.WriteStandardDataAccessSAP(objFDDBank4.IDSAP)

            'DataGrid neu befuellen
            FillGrid(objFDDBank4, DataGrid1.CurrentPageIndex)
        Else
            'AENDERUNGEN LOGGEN!
            logApp.WriteEntry("ERR", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, lblVertragsNummer.Text, "Fehler beim Speichern des Memos (" & Memo.Text & ") für Vertrag-Nr. " & lblVertragsNummer.Text & ". (" & objFDDBank4.Message & ")", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
            logApp.WriteStandardDataAccessSAP(objFDDBank4.IDSAP)

            'Fehler ausgeben
            lblError.Text = objFDDBank4.Message
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
End Class
' ************************************************
' $History: Report31_3.aspx.vb $
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 23.03.10   Time: 16:34
' Updated in $/CKAG/Applications/appffd/Forms
' Rückgängig: Dynproxy-Zugriff
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 11.03.10   Time: 12:36
' Updated in $/CKAG/Applications/appffd/Forms
' ITA: 2918
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 19.06.09   Time: 16:48
' Updated in $/CKAG/Applications/appffd/Forms
' ITA: 2918
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
' *****************  Version 4  *****************
' User: Uha          Date: 2.07.07    Time: 12:52
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 19.06.07   Time: 14:41
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' History-Eintrag eingepflegt.
' 
' ************************************************
