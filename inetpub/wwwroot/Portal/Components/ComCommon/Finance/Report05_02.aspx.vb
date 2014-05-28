Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Report05_02
    Inherits System.Web.UI.Page

    Private m_context As HttpContext = HttpContext.Current
    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private objSuche As Finance.Search
    Private objReport05_objRTFS As BankBaseCredit
    Private objReport05_objRTFS2 As fin_13
    Private m_report As fin_09
    Private strInputref As String
    Protected WithEvents Kopfdaten1 As ComCommon.PageElements.Kopfdaten
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents trExcel As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lnkCreateExcel As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ShowScript As System.Web.UI.HtmlControls.HtmlTableRow

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

        GetAppIDFromQueryString(Me)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text

        m_App = New Base.Kernel.Security.App(m_User)
        strInputref = m_User.Reference

        If Not Session("m_report") Is Nothing Then
            m_report = CType(Session("m_report"), fin_09)
            strInputref = m_report.personenennummer
        End If

        If (Session("objSuche") Is Nothing) Then
            objSuche = New Finance.Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
            If Not objSuche.LeseHaendlerSAP_Einzeln(Session("AppID").ToString, Session.SessionID.ToString, strInputref) Then
                lblError.Text = "Fehler bei der Ermittlung der Händlerdaten.<br>(" & objSuche.ErrorMessage & ")"
            End If
        Else
            objSuche = CType(Session("objSuche"), Finance.Search)
        End If

        Kopfdaten1.UserReferenz = strInputref
        Kopfdaten1.HaendlerNummer = objSuche.REFERENZ
        Dim strTemp As String = objSuche.NAME
        If objSuche.NAME_2.Length > 0 Then
            strTemp &= "<br>" & objSuche.NAME_2
        End If
        Kopfdaten1.HaendlerName = strTemp
        Kopfdaten1.Adresse = objSuche.COUNTRYISO & " - " & objSuche.POSTL_CODE & " " & objSuche.CITY & "<br>" & objSuche.STREET

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

            'kontingenttabelle ausblenden wenn Parameter
            If Request.QueryString("HDL") = 1 Then
                Session("AppShowNot") = True
            End If

            objReport05_objRTFS = New BankBaseCredit(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
            objReport05_objRTFS.Customer = objSuche.REFERENZ
            objReport05_objRTFS.KUNNR = m_User.KUNNR
            objReport05_objRTFS.CreditControlArea = "ZDAD"
            objReport05_objRTFS.Show(Session("AppID").ToString, Session.SessionID)


            m_context.Cache.Insert("objReport05_objRTFS", objReport05_objRTFS, New System.Web.Caching.CacheDependency(Server.MapPath("Report05.aspx")), DateTime.Now.AddMinutes(20), TimeSpan.Zero)
            CheckobjRTFS2()

        Else
            If Not m_context.Cache("objReport05_objRTFS") Is Nothing Then
                objReport05_objRTFS = CType(m_context.Cache("objReport05_objRTFS"), BankBaseCredit)
                CheckobjRTFS2()
            Else
                Response.Redirect("../../../Start/Selection.aspx")
            End If
        End If
       
    End Sub

    Private Sub CheckobjRTFS2()
        If objReport05_objRTFS.Status = 0 Then
            Kopfdaten1.Kontingente = objReport05_objRTFS.Kontingente

            If Not IsPostBack Then
                Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
                objReport05_objRTFS2 = New fin_13(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, strFileName, True)
                objReport05_objRTFS2.AppID = Session("AppID").ToString
                objReport05_objRTFS2.Customer = objSuche.CUSTOMER
                objReport05_objRTFS2.CreditControlArea = "ZDAD"
                objReport05_objRTFS2.Haendler = strInputref
                objReport05_objRTFS2.show("X", Session("AppID").ToString, Session.SessionID)
                m_context.Cache.Insert("objReport05_objRTFS2", objReport05_objRTFS2, New Caching.CacheDependency(Server.MapPath("Report05.aspx")), DateTime.Now.AddMinutes(20), TimeSpan.Zero)

                FillGrid(0)
            Else
                If Not m_context.Cache("objReport05_objRTFS2") Is Nothing Then
                    objReport05_objRTFS2 = CType(m_context.Cache("objReport05_objRTFS2"), fin_13)
                Else
                    Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
                    objReport05_objRTFS2 = New fin_13(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, strFileName)
                    objReport05_objRTFS2.AppID = Session("AppID").ToString
                    objReport05_objRTFS2.Customer = objSuche.CUSTOMER
                    objReport05_objRTFS2.CreditControlArea = "ZDAD"
                    objReport05_objRTFS2.Haendler = strInputref
                    objReport05_objRTFS2.show("X", Session("AppID").ToString, Session.SessionID) ' HEZ Kennzeichen
                    m_context.Cache.Insert("objReport05_objRTFS2", objReport05_objRTFS2, New Caching.CacheDependency(Server.MapPath("Report05.aspx")), DateTime.Now.AddMinutes(20), TimeSpan.Zero)

                    FillGrid(0)
                End If
            End If

            cmdSave.Enabled = False
        Else
            lblError.Text = objReport05_objRTFS.Message
        End If
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If Not objReport05_objRTFS2.Auftraege Is Nothing Then
            If objReport05_objRTFS2.Status = 0 Then
                If objReport05_objRTFS2.Auftraege.Rows.Count = 0 Then
                    trExcel.Visible = False
                    lblNoData.Text = "Keine Daten zur Anzeige gefunden."
                    DataGrid1.Visible = False
                    lblNoData.Visible = True

                Else
                    trExcel.Visible = True
                    DataGrid1.Visible = True
                    lblNoData.Visible = False

                    Dim tmpDataView As DataView= objReport05_objRTFS2.Auftraege.DefaultView

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
                    lblNoData.Text = "Es wurde(n) " & tmpDataView.Count.ToString & " offene Zulassung(en) gefunden, davon " & view.Count & " nicht freigegebene.<br>Zum Freigeben der Aufträge setzen Sie sich bitte mit Ihrem zuständigen Ansprechpartner in Verbindung."
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
                    Dim control As Control
                    Dim blnScriptFound As Boolean = False


                    For Each item In DataGrid1.Items
                        cell = item.Cells(0)
                        Dim strAuftragsnummer As String = cell.Text

                        cell = item.Cells(1)
                        Dim strVertragsnummer As String = cell.Text

                        cell = item.Cells(3)
                        Dim strAngefordertAm As String = cell.Text

                        cell = item.Cells(2)
                        Dim strFahrgestellnummer As String = cell.Text

                        cell = item.Cells(5)
                        Dim strBriefnummer As String = cell.Text

                        cell = item.Cells(6)
                        Dim strKontingentart As String = cell.Text

                        cell = item.Cells(8)
                        For Each control In cell.Controls
                            Dim linkButton = TryCast(control, LinkButton)
                            If (linkButton IsNot Nothing) Then
                                button = linkButton
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
                    Session("App_objReport05_objRTFS2") = objReport05_objRTFS2
                End If
            Else
                lblError.Text = objReport05_objRTFS2.Message
                lblNoData.Visible = True
            End If
        Else
            trExcel.Visible = False
            lblNoData.Text = "Keine Daten zur Anzeige gefunden."
            DataGrid1.Visible = False
            lblNoData.Visible = True
        End If
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(0)
    End Sub

    Private Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.ItemCommand
        If Not e.Item.Cells(0).Text.Length = 0 Then
            objReport05_objRTFS2.VBELN = e.Item.Cells(0).Text
            objReport05_objRTFS2.EQUNR = e.Item.Cells(1).Text
            objReport05_objRTFS2.Change(Session("AppID").ToString, Session.SessionID)

            Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
            If Not objReport05_objRTFS2.Status = 0 Then
                lblError.Text = objReport05_objRTFS2.Message
                logApp.WriteEntry("ERR", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, strInputref, "Fehler bei der Stornierung durch Händler " & objSuche.REFERENZ & m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 10)
            Else
                lblError.Text = ""
                logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, strInputref, "Stornierung durch Händler " & objSuche.REFERENZ, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 10)
            End If
            logApp.WriteStandardDataAccessSAP(objReport05_objRTFS2.IDSAP)

            objReport05_objRTFS2.show("X", Session("AppID").ToString, Session.SessionID)
            m_context.Cache.Insert("objReport05_objRTFS2", objReport05_objRTFS2, New Caching.CacheDependency(Server.MapPath("Report05_5.aspx")), DateTime.Now.AddMinutes(20), TimeSpan.Zero)

            objReport05_objRTFS.Show(Session("AppID").ToString, Session.SessionID)
            m_context.Cache.Insert("objReport05_objRTFS", objReport05_objRTFS, New Caching.CacheDependency(Server.MapPath("Report05_5.aspx")), DateTime.Now.AddMinutes(20), TimeSpan.Zero)

            If Not objReport05_objRTFS2.Status = 0 Then
                lblError.Text = objReport05_objRTFS2.Message
            End If
            FillGrid(DataGrid1.CurrentPageIndex)
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub lnkCreateExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkCreateExcel.Click

        objReport05_objRTFS2 = Session("App_objReport05_objRTFS2")
        Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
        excelFactory.CreateDocumentAndSendAsResponse(strFileName, objReport05_objRTFS2.ResultExcel, Page)
       
    End Sub

  
End Class
' ************************************************
' $History: Report05_02.aspx.vb $
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 23.06.09   Time: 9:13
' Updated in $/CKAG/Components/ComCommon/Finance
' ita 2918 Z_M_OFFENEANFOR_STORNO_001, Z_M_OFFENEANFOR_001
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 22.06.09   Time: 13:18
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2918 Z_M_Creditlimit_Detail_001
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 19.06.09   Time: 10:10
' Updated in $/CKAG/Components/ComCommon/Finance
' ITa 2918 testfertig
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 5.06.09    Time: 15:58
' Updated in $/CKAG/Components/ComCommon/Finance
' mögliche try catches entfernt
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 11.04.08   Time: 13:43
' Updated in $/CKAG/Components/ComCommon/Finance
' Migration
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
' User: Fassbenders  Date: 4.04.08    Time: 10:48
' Created in $/CKAG/portal/Components/ComCommon/Finance
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 13.03.08   Time: 14:41
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' RTFS Kopfdaten änderung auf Finance Kopfdaten 
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 13.03.08   Time: 11:33
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' RTFS Anpassungen 
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 24.01.08   Time: 9:52
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Grid Item Styles  cell-Padding entfernt
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 22.01.08   Time: 13:41
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' doku
' 
' ************************************************
