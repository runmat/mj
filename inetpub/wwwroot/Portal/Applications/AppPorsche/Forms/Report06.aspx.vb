Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business

Public Class Report06
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
    Private objSuche As Search
    Private objReport06_objPorsche As Base.Business.BankBaseCredit
    Private objReport06_objPorsche2 As Porsche_06

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents Kopfdaten1 As Kopfdaten
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ucHeader As Header
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lnkExcel As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblDownloadTip As System.Web.UI.WebControls.Label
    Protected WithEvents ShowScript As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents Literal1 As System.Web.UI.WebControls.Literal
    Protected WithEvents lblWait As System.Web.UI.WebControls.Panel
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Literal1.Text = ""
        DataGrid1.Visible = True
        lblWait.Visible = False

        cmdSave.Enabled = False
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)
        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text

        m_App = New Base.Kernel.Security.App(m_User)

        If Not IsPostBack Then
            objSuche = New Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
            If Not objSuche.LeseHaendlerSAP_Einzeln(Session("AppID").ToString, Session.SessionID.ToString, m_User.Reference) Then
                lblError.Text = "Fehler bei der Ermittlung der Händlerdaten.<br>(" & objSuche.ErrorMessage & ")"
            End If
            Session("objSuche") = objSuche
        Else
            objSuche = CType(Session("objSuche"), Search)
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

                objReport06_objPorsche = New Base.Business.BankBaseCredit(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                objReport06_objPorsche.Customer = "60" & objSuche.REFERENZ
                objReport06_objPorsche.CreditControlArea = "ZDAD"
                'objReport06_objPorsche.ShowStandard()

                '§§§ JVE 15.08.2006: Caching-Objekt vermischt Client-Daten! Daher in Session gepackt (in der gesamten Seite).
                Session("objHaendler") = objReport06_objPorsche
                'm_context.Cache.Insert("objReport06_objPorsche", objReport06_objPorsche, New System.Web.Caching.CacheDependency(Server.MapPath("Report05.aspx")), DateTime.Now.AddMinutes(20), TimeSpan.Zero)
                CheckobjFDDBank2()
            Else
                If Not Session("objHaendler") Is Nothing Then
                    objReport06_objPorsche = CType(Session("objHaendler"), Base.Business.BankBaseCredit)
                    'objReport06_objPorsche = CType(m_context.Cache("objReport06_objPorsche"), Base.Business.BankBaseCredit)
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
        If objReport06_objPorsche.Status = 0 Then
            Kopfdaten1.Kontingente = objReport06_objPorsche.Kontingente

            If Not IsPostBack Then
                Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
                objReport06_objPorsche2 = New Porsche_06(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, strFileName)
                objReport06_objPorsche2.AppID = Session("AppID").ToString
                objReport06_objPorsche2.Customer = objSuche.CUSTOMER
                objReport06_objPorsche2.CreditControlArea = "ZDAD"
                objReport06_objPorsche2.Haendler = m_User.Reference
                objReport06_objPorsche2.Show(Me)

                Session("objFDDBank2") = objReport06_objPorsche2
                'm_context.Cache.Insert("objReport06_objPorsche2", objReport06_objPorsche2, New System.Web.Caching.CacheDependency(Server.MapPath("Report05.aspx")), DateTime.Now.AddMinutes(20), TimeSpan.Zero)

                If (objReport06_objPorsche2.Status = 0) AndAlso (Not (objReport06_objPorsche2.Auftraege Is Nothing)) Then

                    Try
                        Excel.ExcelExport.WriteExcel(objReport06_objPorsche2.Auftraege, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                        lnkExcel.NavigateUrl = "/Portal/Temp/Excel/" & strFileName
                        lnkExcel.Visible = True
                        lblDownloadTip.Visible = True
                    Catch
                    End Try
                    FillGrid(0)
                Else
                    lblError.Text = "Keine Daten gefunden."
                    ddlPageSize.Visible = False
                    Kopfdaten1.Visible = False
                End If
            Else
                If Not Session("objFDDBank2") Is Nothing Then
                    objReport06_objPorsche2 = CType(Session("objFDDBank2"), Porsche_06)

                    If Not Request.Form.Item("txtAuftragsnummer") = "empty" Then
                        objReport06_objPorsche2.AuftragsNummer = CStr(Request.Form.Item("txtAuftragsnummer"))
                        objReport06_objPorsche2.Change(Me)

                        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
                        If Not objReport06_objPorsche2.Status = 0 Then
                            lblError.Text = objReport06_objPorsche2.Message
                            logApp.WriteEntry("ERR", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.Reference, "Fehler bei der Stornierung durch Händler " & objSuche.REFERENZ & " (Vorgang: " & objReport06_objPorsche2.AuftragsNummer & ", Fehler: " & objReport06_objPorsche2.Message & ")", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 10)
                        Else
                            lblError.Text = ""
                            logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.Reference, "Stornierung durch Händler " & objSuche.REFERENZ & " (Vorgang: " & objReport06_objPorsche2.AuftragsNummer & ")", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 10)
                        End If
                        logApp.WriteStandardDataAccessSAP(objReport06_objPorsche2.IDSAP)

                        objReport06_objPorsche2.Show(Me)
                        Session("objFDDBank2") = objReport06_objPorsche2

                        'objReport06_objPorsche.ShowStandard()
                        Kopfdaten1.Kontingente = objReport06_objPorsche.Kontingente
                        Session("objHaendler") = objReport06_objPorsche

                        If Not objReport06_objPorsche2.Status = 0 Then
                            lblError.Text = objReport06_objPorsche2.Message
                        End If
                        FillGrid(DataGrid1.CurrentPageIndex)
                        'Vorgang evtl. aus der Authorisierungstabelle löschen!
                        Dim fahrg As String

                        fahrg = CStr(Request.Form.Item("txtFahrgestellnummer"))
                        If (fahrg <> String.Empty) Then
                            DelAuthEntry(fahrg)
                        End If
                    End If
                Else
                    Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
                    objReport06_objPorsche2 = New Porsche_06(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, strFileName)
                    objReport06_objPorsche2.AppID = Session("AppID").ToString
                    objReport06_objPorsche2.Customer = objSuche.CUSTOMER
                    objReport06_objPorsche2.CreditControlArea = "ZDAD"
                    objReport06_objPorsche2.Haendler = m_User.Reference
                    objReport06_objPorsche2.Show(Me)

                    Session("objFDDBank2") = objReport06_objPorsche2
                    'm_context.Cache.Insert("objReport06_objPorsche2", objReport06_objPorsche2, New System.Web.Caching.CacheDependency(Server.MapPath("Report05.aspx")), DateTime.Now.AddMinutes(20), TimeSpan.Zero)

                    If (objReport06_objPorsche2.Status = 0) AndAlso (Not (objReport06_objPorsche2.Auftraege Is Nothing)) Then

                        Try
                            Excel.ExcelExport.WriteExcel(objReport06_objPorsche2.Auftraege, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                            lnkExcel.NavigateUrl = "/Portal/Temp/Excel/" & strFileName
                            lnkExcel.Visible = True
                            lblDownloadTip.Visible = True
                        Catch
                        End Try
                        FillGrid(0)
                    Else
                        lblError.Text = "Keine Daten gefunden."
                        ddlPageSize.Visible = False
                        Kopfdaten1.Visible = False
                    End If
                End If
            End If

            cmdSave.Enabled = False
        Else
            lblError.Text = objReport06_objPorsche.Message
        End If
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If objReport06_objPorsche2.Status = 0 Then
            If objReport06_objPorsche2.Auftraege.Rows.Count = 0 Then
                lnkExcel.Visible = False
                lblDownloadTip.Visible = False

                lblNoData.Text = "Keine Daten zur Anzeige gefunden."
                DataGrid1.Visible = False
                lblNoData.Visible = True
            Else
                lnkExcel.Visible = True
                lblDownloadTip.Visible = True
                DataGrid1.Visible = True
                lblNoData.Visible = False

                Dim tmpDataView As New DataView()
                tmpDataView = objReport06_objPorsche2.Auftraege.DefaultView

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
                lblNoData.Text = "Es wurde(n) " & tmpDataView.Count.ToString & " offene Anforderung(en) gefunden, davon " & view.Count & " gesperrte.<br>Zum Freigeben der gesperrten Aufträge setzen Sie sich bitte mit Porsche Financial Services GmbH in Verbindung."
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

                    cell = item.Cells(2)
                    Dim strAngefordertAm As String = cell.Text

                    cell = item.Cells(3)
                    Dim strFahrgestellnummer As String = cell.Text

                    cell = item.Cells(4)
                    Dim strBriefnummer As String = cell.Text

                    cell = item.Cells(5)
                    Dim strKontingentart As String = cell.Text

                    cell = item.Cells(7)
                    For Each control In cell.Controls
                        If TypeOf control Is LinkButton Then
                            button = CType(control, LinkButton)
                            If button.CommandName = "Storno" Then
                                button.Enabled = True
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
            End If
        Else
            lblError.Text = objReport06_objPorsche2.Message
            lblNoData.Visible = True
        End If
    End Sub

    Private Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.ItemCommand
        If Not e.Item.Cells(0).Text.Length = 0 Then
            lblWait.Visible = True
            DataGrid1.Visible = False

            Literal1.Text = "									<script language=""Javascript"">" & vbCrLf
            Literal1.Text &= "									<!-- //" & vbCrLf
            Literal1.Text &= "										window.document.Form1.txtAuftragsnummer.value = """ & e.Item.Cells(0).Text & """;" & vbCrLf
            Literal1.Text &= "										window.document.Form1.txtFahrgestellnummer.value = """ & e.Item.Cells(3).Text & """;" & vbCrLf
            Literal1.Text &= "										window.document.Form1.submit();" & vbCrLf
            Literal1.Text &= "									//-->" & vbCrLf
            Literal1.Text &= "									</script>" & vbCrLf
        End If
    End Sub

    Private Sub DelAuthEntry(ByVal fahrgestellnr As String)
        Dim cn As New SqlClient.SqlConnection
        Try
            cn.ConnectionString = m_User.App.Connectionstring
            cn.Open()

            Dim strDeleteSQL As String = "DELETE " & _
                                          "FROM [Authorization] " & _
                                          "WHERE CustomerReference=@cr AND ProcessReference=@pr"

            Dim cmd As New SqlClient.SqlCommand()
            cmd.Connection = cn
            cmd.Parameters.AddWithValue("@cr", m_User.Reference)
            cmd.Parameters.AddWithValue("@pr", fahrgestellnr)

            cmd.CommandText = strDeleteSQL
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "AutorisierungenLoeschen", "lbtnDelete_Click", ex.ToString)

            lblError.Text = ex.Message
        Finally
            cn.Close()
            cn.Dispose()
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
End Class

' ************************************************
' $History: Report06.aspx.vb $
' 
' *****************  Version 6  *****************
' User: Fassbenders  Date: 23.03.10   Time: 16:47
' Updated in $/CKAG/Applications/AppPorsche/Forms
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 11.03.10   Time: 14:42
' Updated in $/CKAG/Applications/AppPorsche/Forms
' ITA: 2918
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 10.03.10   Time: 15:53
' Updated in $/CKAG/Applications/AppPorsche/Forms
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 9.07.09    Time: 10:22
' Updated in $/CKAG/Applications/AppPorsche/Forms
' ITA 2918 Z_M_OFFENEANFOR_PORSCHE, Z_M_OFFENEANFOR_STORNO_PORSCHE,
' Z_M_UNANGEFORDERT_PORSCHE, Z_M_UNB_HAENDLER_PORSCHE,
' Z_M_EQUIS_ZU_STICHTAG
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 4.05.09    Time: 9:21
' Updated in $/CKAG/Applications/AppPorsche/Forms
' ITA: 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 11:28
' Created in $/CKAG/Applications/AppPorsche/Forms
' 
' *****************  Version 13  *****************
' User: Rudolpho     Date: 6.12.07    Time: 14:24
' Updated in $/CKG/Applications/AppPorsche/AppPorscheWeb/Forms
' ITA 1440
' 
' *****************  Version 12  *****************
' User: Uha          Date: 16.08.07   Time: 16:41
' Updated in $/CKG/Applications/AppPorsche/AppPorscheWeb/Forms
' ITA 1196 "Bugfix Report Offene Anforderungen" (Report06)
' 
' *****************  Version 11  *****************
' User: Uha          Date: 2.07.07    Time: 13:27
' Updated in $/CKG/Applications/AppPorsche/AppPorscheWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 10  *****************
' User: Uha          Date: 5.03.07    Time: 14:18
' Updated in $/CKG/Applications/AppPorsche/AppPorscheWeb/Forms
' 
' *****************  Version 9  *****************
' User: Uha          Date: 5.03.07    Time: 13:54
' Updated in $/CKG/Applications/AppPorsche/AppPorscheWeb/Forms
' 
' *****************  Version 8  *****************
' User: Uha          Date: 5.03.07    Time: 12:50
' Updated in $/CKG/Applications/AppPorsche/AppPorscheWeb/Forms
' 
' ************************************************
