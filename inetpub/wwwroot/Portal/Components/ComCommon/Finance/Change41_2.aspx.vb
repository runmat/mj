Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business

Public Class Change41_2
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

    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private objSuche As CKG.Components.ComCommon.Finance.Search
    Private objBank As CKG.Components.ComCommon.BankBaseCredit
    Private m_Change As fin_05

    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ucHeader As Header
    Protected WithEvents ShowScript As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents Literal1 As System.Web.UI.WebControls.Literal
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label

    Protected WithEvents lbExcel As LinkButton
    Protected WithEvents lblDownloadTip As System.Web.UI.WebControls.Label
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblVertragsNummer As System.Web.UI.WebControls.Label
    Protected WithEvents Memo As System.Web.UI.WebControls.TextBox
    Protected WithEvents SaveMemo As System.Web.UI.WebControls.LinkButton
    Protected WithEvents CancelMemo As System.Web.UI.WebControls.LinkButton
    Protected WithEvents trMemo As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lb_HaendlerSuche As LinkButton
    Protected WithEvents lb_HaendlerAuswahl As LinkButton
    Protected WithEvents ucStyles As Styles
    Protected WithEvents kopfdaten1 As CKG.Components.ComCommon.PageElements.Kopfdaten


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        m_App = New Base.Kernel.Security.App(m_User)
        GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")

        If Not IsPostBack Then
            trMemo.Visible = False
        End If

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text

        m_App = New Base.Kernel.Security.App(m_User)


        objSuche = New Finance.Search(m_App, m_User, Session.SessionID, CStr(Session("AppID")))

        If objSuche.LeseHaendlerSAP_Einzeln(Session("AppID").ToString, Session.SessionID.ToString, Session("SelectedDealer").ToString) Then
            kopfdaten1.HaendlerNummer = objSuche.REFERENZ
            Dim strTemp As String = objSuche.NAME
            If objSuche.NAME_2.Length > 0 Then
                strTemp &= "<br>" & objSuche.NAME_2
            End If
            kopfdaten1.HaendlerName = strTemp
            kopfdaten1.Adresse = objSuche.COUNTRYISO & " - " & objSuche.POSTL_CODE & " " & objSuche.CITY & "<br>" & objSuche.STREET
            Session.Add("objSuche", objSuche)
        Else
            lblError.Text = "Fehler bei der Ermittlung der Händlerdaten.<br>(" & objSuche.ErrorMessage & ")"
        End If




        objBank = CType(Session("objBank"), BankBaseCredit)
        m_Change = CType(Session("m_Change"), fin_05)
        If objBank.Status = 0 And m_Change.Status = 0 Then
            If Not IsPostBack Then
                ddlPageSize.Items.Add("10")
                ddlPageSize.Items.Add("20")
                ddlPageSize.Items.Add("50")
                ddlPageSize.Items.Add("100")
                ddlPageSize.Items.Add("200")
                ddlPageSize.Items.Add("500")
                ddlPageSize.Items.Add("1000")
                ddlPageSize.SelectedIndex = 2

                Dim objExcelExport As New Excel.ExcelExport()
                Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"


                lbExcel.Visible = True



                FillGrid(m_Change, 0)
                kopfdaten1.Kontingente = objBank.Kontingente
            End If

        Else
            lblError.Text = objBank.Message & "<br>" & m_Change.Message
        End If
       
    End Sub



    Private Sub FillGrid(ByVal objBank As fin_05, ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If objBank.Status = 0 Then
            If objBank.Auftraege.Rows.Count = 0 Then
                DataGrid1.Visible = False
                lblNoData.Visible = True
                lblNoData.Text = "Keine Daten zur Anzeige gefunden."
            Else
                DataGrid1.Visible = True
                lblNoData.Visible = False

                
                'wenn bei dem Händler besser gesagt bei dem Datensatz eine ZZREFERENZ1 angegeben ist, nur Datensätze anzeigen die dieser entsprechen JJ2008.04.10 ITA 1842
                If Not objBank.zzreferenz1 Is String.Empty Then
                    objBank.Auftraege.DefaultView.RowFilter = "ZZREFERENZ1='" & objBank.zzreferenz1 & "'"
                Else
                    objBank.Auftraege.DefaultView.RowFilter = ""
                End If

                Dim tmpDataView As DataView = objBank.Auftraege.DefaultView

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
                Dim textbox As textbox
                Dim control As control
                Dim blnScriptFound As Boolean = False
                Dim intZaehl As Int32

                For Each item In DataGrid1.Items
                    cell = item.Cells(0)
                    Dim strVertragsnummer As String
                    strVertragsnummer = cell.Text

                    For intZaehl = 1 To item.Cells.Count - 1
                        cell = item.Cells(intZaehl)
                        For Each control In cell.Controls
                            Dim linkButton = TryCast(control, LinkButton)
                            If (linkButton IsNot Nothing) Then
                                button = linkButton
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
                            Dim box = TryCast(control, TextBox)
                            If (box IsNot Nothing) Then
                                textbox = box
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
        FillGrid(m_Change, e.NewPageIndex)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(m_Change, DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(m_Change, 0)
    End Sub

    Private Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.ItemCommand
        Dim dr As DataRow
        For Each dr In m_Change.Auftraege.Rows
            dr("DatumAendern") = False
        Next
        If e.CommandName = "WriteMemo" Then
            'Memoeditierbereich oeffnen
            trMemo.Visible = True
            'Vertragsnummer in Ueberschrift
            lblVertragsNummer.Text = CType(e.Item.Cells(0).FindControl("lblVertragsNR"), Label).Text
            'evtl. vorhandenen Memotext in Textbox
            Dim ctl As Control
            Dim lnk As LinkButton
            For Each ctl In e.Item.Cells(5).Controls
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
                For Each dr In m_Change.Auftraege.Select(String.Format("Vertragsnummer={0}", Request.Form("txtVertragsnummer")))
                    dr("faellig am") = Request.Form("txtFaelligkeit")
                Next
                m_Change.Change(Session("AppId").ToString, Session.SessionID)
                Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)

                If m_Change.Status = 0 Then
                    'AENDERUNGEN LOGGEN!
                    logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, CStr(Request.Form("txtVertragsnummer")), "Fälligkeitsdatum für Vertrag-Nr. " & CStr(Request.Form("txtVertragsnummer")) & " auf den " & CStr(Request.Form("txtFaelligkeit")) & " festgesetzt.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
                    logApp.WriteStandardDataAccessSAP(m_Change.IDSAP)

                    For Each dr In m_Change.Auftraege.Select(String.Format("Vertragsnummer={0}", Request.Form("txtVertragsnummer")))
                        dr("DatumAendern") = False
                    Next
                    'DataGrid neu befuellen
                    FillGrid(m_Change, DataGrid1.CurrentPageIndex)
                Else
                    'AENDERUNGEN LOGGEN!
                    logApp.WriteEntry("ERR", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, CStr(Request.Form("txtVertragsnummer")), "Fehler beim Festlegen des Fälligkeitsdatum für Vertrag-Nr. " & CStr(Request.Form("txtVertragsnummer")) & " auf den " & CStr(Request.Form("txtFaelligkeit")) & ". (" & m_Change.Message & ")", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
                    logApp.WriteStandardDataAccessSAP(m_Change.IDSAP)

                    'Fehler ausgeben
                    lblError.Text = m_Change.Message
                End If
            Else
                lblError.Text = "Bitte geben Sie ein Datum im Format DD.MM.JJJJ ein."
            End If
        ElseIf e.CommandName = "Aendern" Then
            For Each dr In m_Change.Auftraege.Select(String.Format("Vertragsnummer={0}", Request.Form("txtVertragsnummer")))
                dr("DatumAendern") = True
            Next

            Literal1.Text = "						<script language=""Javascript"">" & vbCrLf
            Literal1.Text &= "						  <!-- //" & vbCrLf
            Literal1.Text &= "						    window.document.location.href = ""#" & CStr(Request.Form("txtVertragsnummer")) & """;" & vbCrLf
            Literal1.Text &= "						  //-->" & vbCrLf
            Literal1.Text &= "						</script>" & vbCrLf

            'DataGrid neu befuellen
            FillGrid(m_Change, DataGrid1.CurrentPageIndex)
        End If

        'direkte Verlinkung auf Statusänderungsreport möglich
        If e.CommandName = "Statusaenderung" Then

            If Session("AppFIN") Is Nothing Then
                Session.Add("AppFIN", "")
            End If

            If Session("AppHaendlerNr") Is Nothing Then
                Session.Add("AppHaendlerNr", "")
            End If

            If m_Change Is Nothing Then
                m_Change = CType(Session("m_Change"), fin_05)
            End If

            If objSuche Is Nothing Then
                objSuche = CType(Session("objSuche"), Finance.Search)
            End If

            Session.Item("AppFIN") = m_Change.Auftraege.Rows(e.Item.ItemIndex).Item("Fahrgestellnummer")
            Session.Item("AppHaendlerNr") = objSuche.REFERENZ

            If Not Session("AppFIN") Is Nothing AndAlso Not Session("AppHaendlerNr") Is Nothing Then
                Dim dvAppLinks As DataView = m_User.Applications.DefaultView
                dvAppLinks.RowFilter = "APPURL='../Components/ComCommon/Finance/Change47.aspx'"

                If dvAppLinks.Count = 1 Then
                    Dim strParameter As String = ""
                    HelpProcedures.getAppParameters(dvAppLinks.Item(0).Item("AppID"), strParameter, ConfigurationManager.AppSettings("Connectionstring"))
                    Response.Redirect("Change47.aspx?AppID=" & dvAppLinks.Item(0).Item("AppID") & strParameter & "&Linked=2")
                Else
                    lblError.Text = "Fehler bei der Weiterleitung zur Statusänderung"
                    lblError.Visible = True
                End If
            Else
                lblError.Text = "Fehler bei der Weiterleitung zur Statusänderung"
                lblError.Visible = True
            End If

        End If

    End Sub

    Private Sub SaveMemo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveMemo.Click
        'Memo speichern
        Dim dr As DataRow
        For Each dr In m_Change.Auftraege.Select(String.Format("Vertragsnummer='{0}'", lblVertragsNummer.Text))
            dr("Memo") = Memo.Text
        Next
        m_Change.Change(Session("AppId").ToString, Session.SessionID)
        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)

        If m_Change.Status = 0 Then
            'AENDERUNGEN LOGGEN!
            logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, lblVertragsNummer.Text, "Memo (" & Memo.Text & ") für Vertrag-Nr. " & lblVertragsNummer.Text & " gespeichert.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
            logApp.WriteStandardDataAccessSAP(m_Change.IDSAP)

            'DataGrid neu befuellen
            FillGrid(m_Change, DataGrid1.CurrentPageIndex)
        Else
            'AENDERUNGEN LOGGEN!
            logApp.WriteEntry("ERR", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, lblVertragsNummer.Text, "Fehler beim Speichern des Memos (" & Memo.Text & ") für Vertrag-Nr. " & lblVertragsNummer.Text & ". (" & m_Change.Message & ")", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
            logApp.WriteStandardDataAccessSAP(m_Change.IDSAP)

            'Fehler ausgeben
            lblError.Text = m_Change.Message
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

    Private Sub lb_HaendlerSuche_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_HaendlerSuche.Click
        Response.Redirect("Change41.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub lb_HaendlerAuswahl_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_HaendlerAuswahl.Click
        Response.Redirect("Change41_1.aspx?AppID=" & Session("AppID").ToString)
    End Sub


    Private Sub lbExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbExcel.Click

        Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
        excelFactory.CreateDocumentAndSendAsResponse(strFileName, m_Change.Auftraege, Page)
       
    End Sub
End Class

' ************************************************
' $History: Change41_2.aspx.vb $
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 22.06.09   Time: 16:02
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2918 Z_M_Faellige_Fahrzdok, Z_M_Faellige_Equipments_Update
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 19.06.09   Time: 10:10
' Updated in $/CKAG/Components/ComCommon/Finance
' ITa 2918 testfertig
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 5.06.09    Time: 15:58
' Updated in $/CKAG/Components/ComCommon/Finance
' mögliche try catches entfernt
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 4.05.09    Time: 11:50
' Updated in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 16.04.08   Time: 14:43
' Updated in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 31.01.08   Time: 18:05
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' AKF Änderungen
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 31.01.08   Time: 17:39
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' AKF Anpassungen
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 25.01.08   Time: 13:51
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Rothe Verbesserungen RTFS TEIL 2
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 18.01.08   Time: 10:57
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1624 fertig
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 20.12.07   Time: 12:41
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Excellinks berichtigt
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 13.12.07   Time: 13:14
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' lb Fieldtranslation Conirfm gemacht
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 13.12.07   Time: 11:25
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' TestFertig
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 13.12.07   Time: 11:18
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' fertig zum testen
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 13.12.07   Time: 10:32
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' 
' *****************  Version 1  *****************
' User: Uha          Date: 12.12.07   Time: 14:10
' Created in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1466/1499 (Fällige Vorgänge) Kompilierfähig = unfertig eingefügt
' 
' ************************************************
