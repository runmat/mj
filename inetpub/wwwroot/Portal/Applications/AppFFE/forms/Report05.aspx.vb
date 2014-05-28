Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business

Public Class Report05
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

    Private m_context As HttpContext = HttpContext.Current
    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private objSuche As FFE_Search
    Private objReport05_objFDDBank As FFE_BankBase
    Private objReport05_objFDDBank2 As FFE_Bank_OffeneAnforderungen

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents Kopfdaten1 As Kopfdatenhaendler
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ucHeader As Header
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lnkExcel As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblDownloadTip As System.Web.UI.WebControls.Label
    Protected WithEvents ShowScript As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lnkCreateExcel As System.Web.UI.WebControls.LinkButton

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        cmdSave.Enabled = False
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text

        m_App = New Base.Kernel.Security.App(m_User)

        If Request.QueryString("HDL") = "1" Then
            Session("AppShowNot") = True
        End If

        If (Session("objSuche") Is Nothing) Then
            objSuche = New FFE_Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
            If Not objSuche.LeseHaendlerSAP_Einzeln(Session("AppID").ToString, Session.SessionID.ToString, m_User.Reference) Then
                lblError.Text = "Fehler bei der Ermittlung der Händlerdaten.<br>(" & objSuche.ErrorMessage & ")"
            End If
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

        Try
            m_App = New Base.Kernel.Security.App(m_User)

            If Not IsPostBack Then
                ddlPageSize.Items.Add("10")
                ddlPageSize.Items.Add("20")
                ddlPageSize.Items.Add("50")
                ddlPageSize.Items.Add("100")
                ddlPageSize.Items.Add("200")
                ddlPageSize.Items.Add("500")
                ddlPageSize.Items.Add("1000")
                ddlPageSize.SelectedIndex = 2

                objReport05_objFDDBank = New FFE_BankBase(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                objReport05_objFDDBank.Customer = "60" & objSuche.REFERENZ
                objReport05_objFDDBank.CreditControlArea = "ZDAD"
                objReport05_objFDDBank.Show()

                Session("objHaendler") = objReport05_objFDDBank

                CheckobjFDDBank2()
            Else
                If Not Session("objHaendler") Is Nothing Then
                    objReport05_objFDDBank = CType(Session("objHaendler"), FFE_BankBase)
                    CheckobjFDDBank2()
                Else
                    Response.Redirect("../../../Start/Selection.aspx")
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Bei der Ermittlung der Daten ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub CheckobjFDDBank2()
        If objReport05_objFDDBank.Status = 0 Then
            If Not IsPostBack Then
                Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
                objReport05_objFDDBank2 = New FFE_Bank_OffeneAnforderungen(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, strFileName)
                objReport05_objFDDBank2.AppID = Session("AppID").ToString
                objReport05_objFDDBank2.Customer = objSuche.CUSTOMER
                objReport05_objFDDBank2.CreditControlArea = "ZDAD"
                objReport05_objFDDBank2.Haendler = m_User.Reference
                objReport05_objFDDBank2.Show()

                Session("objFDDBank2") = objReport05_objFDDBank2
                FillGrid(0)
            Else
                If Not Session("objFDDBank2") Is Nothing Then
                    objReport05_objFDDBank2 = CType(Session("objFDDBank2"), FFE_Bank_OffeneAnforderungen)
                Else
                    Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
                    objReport05_objFDDBank2 = New FFE_Bank_OffeneAnforderungen(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, strFileName)
                    objReport05_objFDDBank2.AppID = Session("AppID").ToString
                    objReport05_objFDDBank2.Customer = objSuche.CUSTOMER
                    objReport05_objFDDBank2.CreditControlArea = "ZDAD"
                    objReport05_objFDDBank2.Haendler = m_User.Reference
                    objReport05_objFDDBank2.Show()

                    Session("objFDDBank2") = objReport05_objFDDBank2

                    FillGrid(0)
                End If
            End If

            cmdSave.Enabled = False
        Else
            lblError.Text = objReport05_objFDDBank.Message
        End If
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If objReport05_objFDDBank2.Status = 0 Then

            If IsNothing(objReport05_objFDDBank2.Auftraege) OrElse objReport05_objFDDBank2.Auftraege.Rows.Count = 0 Then
                lnkExcel.Visible = False
                lblDownloadTip.Visible = False
                lblNoData.Text = "Keine Daten zur Anzeige gefunden."
                DataGrid1.Visible = False
                lblNoData.Visible = True
            Else
                lnkCreateExcel.Visible = True
                DataGrid1.Visible = True
                lblNoData.Visible = False

                Dim tmpDataView As New DataView()
                tmpDataView = objReport05_objFDDBank2.Auftraege.DefaultView
                Session("lnkExcel") = objReport05_objFDDBank2.Auftraege

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
                'Anzahl der gesperrten Aufträge ermitteln
                Dim view As New DataView(tmpDataView.Table)
                view.RowFilter = "Gesperrt <> ''"
                '----------------------------------------
                lblNoData.Text = "Anzahl offene Anforderungen: " & tmpDataView.Count.ToString & "<br>Davon in Bearbeitung Bank: " & view.Count & "<br>"
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
                Dim hyperlink As HyperLink
                Dim Label As Label
                Dim control As Control
                Dim blnScriptFound As Boolean = False
                Dim strAuftragsnummer As String = ""
                Dim strVertragsnummer As String = ""
                Dim strAngefordertAm As String = ""
                Dim strFahrgestellnummer As String = ""
                Dim strKontingentart As String = ""
                Dim strBriefnummer As String = ""

                For Each item In DataGrid1.Items
                    cell = item.Cells(0)
                    For Each control In cell.Controls
                        If TypeOf control Is Label Then
                            Label = CType(control, Label)
                            strAuftragsnummer = Label.Text
                        End If
                    Next

                    cell = item.Cells(1)
                    For Each control In cell.Controls
                        If TypeOf control Is Label Then
                            Label = CType(control, Label)
                            strVertragsnummer = Label.Text.Trim
                        End If
                    Next

                    cell = item.Cells(2)
                    For Each control In cell.Controls
                        If TypeOf control Is Label Then
                            Label = CType(control, Label)
                            strAngefordertAm = Label.Text
                        End If
                    Next

                    cell = item.Cells(3)
                    For Each control In cell.Controls
                        If TypeOf control Is HyperLink Then
                            hyperlink = CType(control, HyperLink)
                            strFahrgestellnummer = hyperlink.Text
                        End If
                    Next

                    cell = item.Cells(4)
                    For Each control In cell.Controls
                        If TypeOf control Is Label Then
                            Label = CType(control, Label)
                            strBriefnummer = Label.Text
                        End If
                    Next

                    cell = item.Cells(5)
                    For Each control In cell.Controls
                        If TypeOf control Is Label Then
                            Label = CType(control, Label)
                            strKontingentart = Label.Text
                        End If
                    Next
                    cell = item.Cells(7)
                    For Each control In cell.Controls
                        If TypeOf control Is LinkButton Then
                            button = CType(control, LinkButton)
                            If button.CommandName = "Storno" Then
                                button.Attributes.Add("onClick", "if (!StornoConfirm('" & strAuftragsnummer & "','" & strVertragsnummer & "','" & strAngefordertAm & "','" & strFahrgestellnummer & "','" & strBriefnummer & "','" & strKontingentart & "')) return false;")
                                button.Attributes.Add("class", "StandardButtonTable")
                                blnScriptFound = True
                            End If
                        End If
                    Next
                Next
                If blnScriptFound Then
                    ShowScript.Visible = True
                End If

                For Each itemX As DataGridItem In DataGrid1.Items
                    If Not itemX.FindControl("lnkHistorie") Is Nothing Then
                        CType(itemX.FindControl("lnkHistorie"), HyperLink).NavigateUrl = "Report46.aspx?AppID=" & m_User.Applications.Select("AppName = 'Report46'")(0)("AppID").ToString & "&VIN=" & CType(itemX.FindControl("lnkHistorie"), HyperLink).Text
                    End If
                Next


            End If
        Else
            lblError.Text = objReport05_objFDDBank2.Message
            lblNoData.Visible = True
        End If
    End Sub

    Private Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.ItemCommand
        Dim ctrl As Control
        Dim Label1 As Label
        Dim sAuftrag As String = ""

        For Each ctrl In e.Item.Cells(0).Controls
            Try
                Label1 = DirectCast(ctrl, Label)
                If Label1.ID = "lblAuftragsnummer" Then
                    sAuftrag = Label1.Text
                    Exit For
                End If
            Catch
            End Try
        Next

        If Not sAuftrag.Length = 0 Then
            objReport05_objFDDBank2.AuftragsNummer = sAuftrag
            objReport05_objFDDBank2.Change()

            Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
            If Not objReport05_objFDDBank2.Status = 0 Then
                lblError.Text = objReport05_objFDDBank2.Message
                logApp.WriteEntry("ERR", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.Reference, "Fehler bei der Stornierung durch Händler " & objSuche.REFERENZ & " (Vorgang: " & objReport05_objFDDBank2.AuftragsNummer & ", Fehler: " & objReport05_objFDDBank2.Message & ")", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 10)
            Else
                lblError.Text = ""
                logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.Reference, "Stornierung durch Händler " & objSuche.REFERENZ & " (Vorgang: " & objReport05_objFDDBank2.AuftragsNummer & ")", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 10)
            End If
            logApp.WriteStandardDataAccessSAP(objReport05_objFDDBank2.IDSAP)

            objReport05_objFDDBank2.Show()
            Session("objFDDBank2") = objReport05_objFDDBank2
            'm_context.Cache.Insert("objReport05_objFDDBank2", objReport05_objFDDBank2, New System.Web.Caching.CacheDependency(Server.MapPath("Report05.aspx")), DateTime.Now.AddMinutes(20), TimeSpan.Zero)

            objReport05_objFDDBank.Show()
            Session("objHaendler") = objReport05_objFDDBank
            'm_context.Cache.Insert("objReport05_objFDDBank", objReport05_objFDDBank, New System.Web.Caching.CacheDependency(Server.MapPath("Report05.aspx")), DateTime.Now.AddMinutes(20), TimeSpan.Zero)

            If Not objReport05_objFDDBank2.Status = 0 Then
                lblError.Text = objReport05_objFDDBank2.Message
            End If
            FillGrid(DataGrid1.CurrentPageIndex)
            '§§§JVE 06.09.2005 <begin>
            'Vorgang evtl. aus der Authorisierungstabelle löschen!
            Dim fahrg As String = ""
            For Each ctrl In e.Item.Cells(3).Controls
                Try
                    Label1 = DirectCast(ctrl, Label)
                    If Label1.ID = "Label2" Then
                        fahrg = Label1.Text
                        Exit For
                    End If
                Catch
                End Try
            Next
            If (fahrg <> String.Empty) Then
                DelAuthEntry(fahrg)
            End If
            '§§§JVE 06.09.2005 <end>
        End If
    End Sub

    Private Sub DelAuthEntry(ByVal fahrgestellnr As String)
        'Dim tblLogParameter As DataTable

        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try
            cn.Open()
            'tblLogParameter = SetOldLogParameters(CInt(txtAuthorizationID.Text), tblLogParameter)

            Dim strDeleteSQL As String = "DELETE " & _
                                          "FROM [Authorization] " & _
                                          "WHERE CustomerReference=@cr AND ProcessReference=@pr"

            Dim cmd As New SqlClient.SqlCommand()
            cmd.Connection = cn
            cmd.Parameters.AddWithValue("@cr", m_User.Reference)
            cmd.Parameters.AddWithValue("@pr", fahrgestellnr)

            cmd.CommandText = strDeleteSQL
            cmd.ExecuteNonQuery()

            'Log(txtAuthorizationID.Text, "Autorisierung löschen", tblLogParameter)
            'Search(True, True, True, True)
            'lblMessage.Text = "Die Autorisierung wurde gelöscht."
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "AutorisierungenLoeschen", "lbtnDelete_Click", ex.ToString)

            lblError.Text = ex.Message
            'Log(txtAuthorizationID.Text, lblError.Text, tblLogParameter, "ERR")
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
                cn.Dispose()
            End If
        End Try
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(0)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Protected Sub lnkCreateExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel.Click
        Try
            Dim control As New Control
            Dim tblTranslations As New DataTable()
            Dim AppURL As String
            Dim m_datatable As DataTable = Session("lnkExcel")
            Dim col As DataGridColumn
            Dim col2 As DataColumn
            Dim bVisibility As Integer
            Dim i As Integer
            Dim sColName As String = ""

            AppURL = Replace(Me.Request.Url.LocalPath, "/Portal", "..")
            tblTranslations = CType(Me.Session(AppURL), DataTable)
            For Each col In DataGrid1.Columns
                For i = m_datatable.Columns.Count - 1 To 0 Step -1
                    bVisibility = 0
                    col2 = m_datatable.Columns(i)
                    If col2.ColumnName = col.SortExpression Then
                        sColName = TranslateColLbtn(DataGrid1, tblTranslations, col.HeaderText, bVisibility)
                        If bVisibility = 0 Then
                            m_datatable.Columns.Remove(col2)
                        ElseIf sColName.Length > 0 Then
                            col2.ColumnName = sColName
                        End If
                    End If
                Next
                m_datatable.AcceptChanges()
            Next
            Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
            excelFactory.CreateDocumentAndSendAsResponse(strFileName, m_datatable, Me.Page)
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub
End Class

' ************************************************
' $History: Report05.aspx.vb $
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 17.09.08   Time: 8:17
' Updated in $/CKAG/Applications/AppFFE/forms
' ITA 2222 fertig
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 11.09.08   Time: 11:03
' Updated in $/CKAG/Applications/AppFFE/forms
' ITA 2221
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 18.06.08   Time: 14:33
' Updated in $/CKAG/Applications/AppFFE/forms
' Ausblenden Händler Kontingente
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 21.05.08   Time: 16:34
' Updated in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 13.05.08   Time: 16:41
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
' *****************  Version 1  *****************
' User: Rudolpho     Date: 3.04.08    Time: 11:19
' Created in $/CKG/Applications/AppFFE/AppFFEWeb/forms
' ITA 1790
' 
' ************************************************

