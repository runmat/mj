Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business

Partial Public Class Report05_5
    Inherits System.Web.UI.Page

    Private m_context As HttpContext = HttpContext.Current
    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private objSuche As FFE_Search
    Private objReport05_objFDDBank As FFE_BankBase
    Private objReport05_objFDDBank2 As FFE_Bank_OffeneAnforderungen

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        cmdSave.Enabled = False
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text

        m_App = New Base.Kernel.Security.App(m_User)

        If (Session("objSuche") Is Nothing) Then
            objSuche = New FFE_Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
            If Not objSuche.LeseHaendlerSAP_Einzeln(Session("AppID").ToString, Session.SessionID.ToString, m_User.Reference) Then
                lblError.Text = "Fehler bei der Ermittlung der Händlerdaten.<br>(" & objSuche.ErrorMessage & ")"
            End If
        Else
            objSuche = CType(Session("objSuche"), FFE_Search)
        End If

        'If (Session("objSuche") Is Nothing) Then
        '    objSuche = New DealerSearch.Search(m_App, m_User)
        '    objSuche.LeseHaendlerSAP()
        'Else
        '    objSuche = CType(Session("objSuche"), DealerSearch.Search)
        'End If

        'If objSuche.GiveDealerDataByRef(m_User.Reference) Then
        Kopfdaten1.UserReferenz = m_User.Reference
        Kopfdaten1.HaendlerNummer = objSuche.REFERENZ
        Dim strTemp As String = objSuche.NAME
        If objSuche.NAME_2.Length > 0 Then
            strTemp &= "<br>" & objSuche.NAME_2
        End If
        Kopfdaten1.HaendlerName = strTemp
        Kopfdaten1.Adresse = objSuche.COUNTRYISO & " - " & objSuche.POSTL_CODE & " " & objSuche.CITY & "<br>" & objSuche.STREET
        'Else
        '    lblError.Text = "Fehler bei der Ermittlung der Händlerdaten.<br>(" & objSuche.ErrorMessage & ")"
        'End If

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

                objReport05_objFDDBank = New FFE_BankBase(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "", True)
                objReport05_objFDDBank.Customer = "60" & objSuche.REFERENZ
                objReport05_objFDDBank.CreditControlArea = "ZDAD"
                objReport05_objFDDBank.Show()

                m_context.Cache.Insert("objReport05_objFDDBank", objReport05_objFDDBank, New System.Web.Caching.CacheDependency(Server.MapPath("Report05.aspx")), DateTime.Now.AddMinutes(20), TimeSpan.Zero)
                CheckobjFDDBank2()
            Else
                If Not m_context.Cache("objReport05_objFDDBank") Is Nothing Then
                    objReport05_objFDDBank = CType(m_context.Cache("objReport05_objFDDBank"), FFE_BankBase)
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
            Kopfdaten1.Kontingente = objReport05_objFDDBank.Kontingente

            If Not IsPostBack Then
                Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
                objReport05_objFDDBank2 = New FFE_Bank_OffeneAnforderungen(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, strFileName, True)
                objReport05_objFDDBank2.AppID = Session("AppID").ToString
                objReport05_objFDDBank2.Customer = objSuche.CUSTOMER
                objReport05_objFDDBank2.CreditControlArea = "ZDAD"
                objReport05_objFDDBank2.Haendler = m_User.Reference
                objReport05_objFDDBank2.Show()
                m_context.Cache.Insert("objReport05_objFDDBank2", objReport05_objFDDBank2, New System.Web.Caching.CacheDependency(Server.MapPath("Report05.aspx")), DateTime.Now.AddMinutes(20), TimeSpan.Zero)

                If objReport05_objFDDBank2.Status = 0 Then
                    Dim objExcelExport As New Excel.ExcelExport()
                    Try
                        Excel.ExcelExport.WriteExcel(objReport05_objFDDBank2.Auftraege, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                        lnkExcel.NavigateUrl = "/Portal/Temp/Excel/" & strFileName
                        lnkExcel.Visible = True
                        lblDownloadTip.Visible = True
                    Catch
                    End Try
                End If

                FillGrid(0)
            Else
                If Not m_context.Cache("objReport05_objFDDBank2") Is Nothing Then
                    objReport05_objFDDBank2 = CType(m_context.Cache("objReport05_objFDDBank2"), FFE_Bank_OffeneAnforderungen)
                Else
                    Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
                    objReport05_objFDDBank2 = New FFE_Bank_OffeneAnforderungen(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, strFileName)
                    objReport05_objFDDBank2.AppID = Session("AppID").ToString
                    objReport05_objFDDBank2.Customer = objSuche.CUSTOMER
                    objReport05_objFDDBank2.CreditControlArea = "ZDAD"
                    objReport05_objFDDBank2.Haendler = m_User.Reference
                    objReport05_objFDDBank2.Show()
                    m_context.Cache.Insert("objReport05_objFDDBank2", objReport05_objFDDBank2, New System.Web.Caching.CacheDependency(Server.MapPath("Report05.aspx")), DateTime.Now.AddMinutes(20), TimeSpan.Zero)

                    If objReport05_objFDDBank2.Status = 0 Then
                        Dim objExcelExport As New Excel.ExcelExport()
                        Try
                            Excel.ExcelExport.WriteExcel(objReport05_objFDDBank2.Auftraege, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                            lnkExcel.NavigateUrl = "/Portal/Temp/Excel/" & strFileName
                            lnkExcel.Visible = True
                            lblDownloadTip.Visible = True
                        Catch
                        End Try
                    End If

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
            If objReport05_objFDDBank2.Auftraege.Rows.Count = 0 Then
                trExcel.Visible = False
                lblNoData.Text = "Keine Daten zur Anzeige gefunden."
                DataGrid1.Visible = False
                lblNoData.Visible = True
            Else
                trExcel.Visible = True
                DataGrid1.Visible = True
                lblNoData.Visible = False

                Dim tmpDataView As New DataView()
                tmpDataView = objReport05_objFDDBank2.Auftraege.DefaultView

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
                lblNoData.Text = "Es wurde(n) " & tmpDataView.Count.ToString & " offene Zulassung(en) gefunden, davon " & view.Count & " nicht freigegebene.<br>"
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
            End If
        Else
            lblError.Text = objReport05_objFDDBank2.Message
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
            objReport05_objFDDBank2.AuftragsNummer = e.Item.Cells(0).Text
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
            m_context.Cache.Insert("objReport05_objFDDBank2", objReport05_objFDDBank2, New System.Web.Caching.CacheDependency(Server.MapPath("Report05_5.aspx")), DateTime.Now.AddMinutes(20), TimeSpan.Zero)

            objReport05_objFDDBank.Show()

            m_context.Cache.Insert("objReport05_objFDDBank", objReport05_objFDDBank, New System.Web.Caching.CacheDependency(Server.MapPath("Report05_5.aspx")), DateTime.Now.AddMinutes(20), TimeSpan.Zero)

            If Not objReport05_objFDDBank2.Status = 0 Then
                lblError.Text = objReport05_objFDDBank2.Message
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
End Class
' ************************************************
' $History: Report05_5.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 11.09.08   Time: 11:03
' Updated in $/CKAG/Applications/AppFFE/forms
' ITA 2221
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 15.05.08   Time: 14:33
' Updated in $/CKAG/Applications/AppFFE/forms
' ITA:1865
' 
' ************************************************
