Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Report04_2
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
    Private m_objTable As DataTable
    'Private m_blnUnvollstaendigeTuete As Boolean

    Protected WithEvents Hyperlink2 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents ShowScript As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents ucStyles As Global.CKG.Portal.PageElements.Styles
    Protected WithEvents ucHeader As Global.CKG.Portal.PageElements.Header

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cmdSave.Enabled = False
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        If (Session("ResultTable") Is Nothing) Then
            Response.Redirect(Request.UrlReferrer.ToString)
        Else
            m_objTable = CType(Session("ResultTable"), DataTable)
            'm_blnUnvollstaendigeTuete = CBool(Session("UnvollstaendigeTuete"))
        End If
        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text
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

                'If (Not Session("ShowLink") Is Nothing) AndAlso Session("ShowLink").ToString = "True" Then
                '    lnkKreditlimit.Visible = True
                '    lnkKreditlimit.NavigateUrl = "Report04.aspx?AppID=" & Session("AppID").ToString
                'End If
                If Not Session("lnkExcel").ToString.Length = 0 Then
                    lblDownloadTip.Visible = True
                    lnkExcel.Visible = True
                    lnkExcel.NavigateUrl = Session("lnkExcel").ToString
                End If
                FillGrid(0)
            Else
                CheckGrid()
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        '##
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub CheckGrid()
        Dim item As DataGridItem
        Dim control As CheckBox
        Dim row As DataRow

        For Each item In DataGrid1.Items
            row = m_objTable.Select("Equipmentnummer=" & item.Cells(0).Text)(0)
            control = CType(item.FindControl("cbxDelete"), CheckBox)
            If control.Checked = True Then
                row("Delete") = True
            Else
                row("Delete") = False
            End If
        Next
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        Dim item As DataGridItem
        Dim control As CheckBox
        Dim row As DataRow

        If m_objTable.Rows.Count = 0 Then
            DataGrid1.Visible = False
            lblNoData.Visible = True
            lblNoData.Text = "Keine Daten zur Anzeige gefunden."
            'ShowScript.Visible = False
        Else
            DataGrid1.Visible = True
            lblNoData.Visible = False

            Dim tmpDataView As New DataView()
            tmpDataView = m_objTable.DefaultView

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

            For Each item In DataGrid1.Items
                row = m_objTable.Select("Equipmentnummer=" & item.Cells(0).Text)(0)
                control = CType(item.FindControl("cbxDelete"), CheckBox)
                If CType(row("Delete"), Boolean) = True Then
                    control.Checked = True
                Else
                    control.Checked = False
                End If
            Next

            If (Not Session("ShowOtherString") Is Nothing) AndAlso CStr(Session("ShowOtherString")).Length > 0 Then
                lblNoData.Text = CStr(Session("ShowOtherString"))
            Else
                lblNoData.Text = "Es wurden " & tmpDataView.Count.ToString & " Einträge zu """ & m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString & """ gefunden."
            End If
            'If (Not Session("BackLink") Is Nothing) AndAlso CStr(Session("BackLink")) = "HistoryBack" Then
            '    lnkKreditlimit.Text = "Zurück"
            '    lnkKreditlimit.NavigateUrl = "javascript:history.back()"
            'End If
            lblNoData.Visible = True

            If DataGrid1.PageCount > 1 Then
                DataGrid1.PagerStyle.CssClass = "PagerStyle"
                DataGrid1.DataBind()
                DataGrid1.PagerStyle.Visible = True
            Else
                DataGrid1.PagerStyle.Visible = False
            End If
        End If
    End Sub

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(0)
    End Sub

    'Private Sub DataGrid1_ItemCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs)
    '    If e.CommandName = "Delete" Then
    '        Try
    '            lblError.Text = ""

    '            Dim m_Report As New ec_10(m_User, m_App, "")
    '            m_Report.CHASSIS_NUM = e.Item.Cells(0).Text
    '            m_Report.EQUNR = e.Item.Cells(1).Text
    '            m_Report.Clear(Session("AppID").ToString, Session.SessionID.ToString)

    '            Dim strLogMsg As String = "Tüte zu Fahrgestell-Nr. " & e.Item.Cells(0).Text & " als vermisst markiert."
    '            Log(e.Item.Cells(0).Text, strLogMsg, e)

    '            Try
    '                'If m_blnUnvollstaendigeTuete Then
    '                '    Response.Redirect("Report10.aspx?AppID=" & Session("AppID").ToString)
    '                'Else
    '                Response.Redirect("Report10.aspx?AppID=" & Session("AppID").ToString & "&Ges=1")
    '                'End If
    '            Catch
    '            End Try
    '        Catch ex As Exception
    '            Me.lblError.Text = ex.Message
    '            If Not ex.InnerException Is Nothing Then
    '                Me.lblError.Text &= ": " & ex.InnerException.Message
    '            End If
    '            Log(e.Item.Cells(0).Text, Me.lblError.Text, e, "ERR")
    '        End Try
    '    End If
    'End Sub

    Private Sub Log(ByVal strIdentification As String, ByVal strDescription As String, ByVal strEQUNR As String, Optional ByVal strCategory As String = "APP")
        Dim logApp As New Base.Kernel.Logging.Trace(m_User.App.Connectionstring, m_User.App.SaveLogAccessSAP, m_User.App.LogLevel)

        Dim strUserName As String = m_User.UserName ' strUserName
        Dim strSessionID As String = Session.SessionID ' strSessionID
        Dim intSource As Integer = CInt(Request.QueryString("AppID")) ' intSource 
        Dim strTask As String = m_User.Applications.Select("AppID = '" & CStr(Request.QueryString("AppID")) & "'")(0)("AppFriendlyName").ToString ' strTask
        Dim strCustomerName As String = m_User.CustomerName ' strCustomername
        Dim blnIsTestUser As Boolean = m_User.IsTestUser ' blnIsTestUser
        Dim intSeverity As Integer = 0 ' intSeverity 
        Dim tblParameters As DataTable = GetLogParameters(strEQUNR) ' tblParameters

        logApp.WriteEntry(strCategory, strUserName, strSessionID, intSource, strTask, strIdentification, strDescription, strCustomerName, m_User.Customer.CustomerId, blnIsTestUser, intSeverity, tblParameters)
    End Sub

    Private Function GetLogParameters(ByVal strEQUNR As String) As DataTable
        Try
            Dim tblPar As New DataTable()
            With tblPar
                .Columns.Add("Fahrg-Nr", System.Type.GetType("System.String"))
                .Rows.Add(.NewRow)
                .Rows(0)("Fahrg-Nr") = strEQUNR
            End With
            Return tblPar
        Catch ex As Exception
            Dim dt As New DataTable()
            dt.Columns.Add("Fehler beim erstellen der Log-Parameter", System.Type.GetType("System.String"))
            dt.Rows.Add(dt.NewRow)
            Dim str As String = ex.Message
            If Not ex.InnerException Is Nothing Then
                str &= ": " & ex.InnerException.Message
            End If
            dt.Rows(0)("Fehler beim erstellen der Log-Parameter") = str
            Return dt
        End Try
    End Function

    Private Sub btnSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSubmit.Click

        Dim m_Report As New Avis04(m_User, m_App, "") 'neues objekt? dann müssen wir halt die tabelle übergeben. JJU20081203


        m_Report.Clear(Session("AppID").ToString, Session.SessionID.ToString, m_objTable)

        If Not m_Report.Status = 0 Then
            lblError.Text = m_Report.Message
            Exit Sub
        Else
            lblNoData.Text = "Vogang erfolgreich abgeschlossen."
        End If

        'Erfolgreich gelöschte Zeilen entfernen

        Dim intCounter As Integer

        For intCounter = m_objTable.Rows.Count - 1 To 0 Step -1
            If (m_objTable.Rows(intCounter)("Status").ToString = "OK") Then
                m_objTable.Rows.Remove(m_objTable.Rows(intCounter))
            End If
        Next

        m_objTable.AcceptChanges()
        Session("ResultTable") = m_objTable
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
' $History: Report04_2.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 28.04.09   Time: 17:09
' Updated in $/CKAG/Applications/AppAvis/forms
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 3.12.08    Time: 10:46
' Updated in $/CKAG/Applications/AppAvis/forms
' ITA 2419 testfertig
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 20.11.08   Time: 17:06
' Created in $/CKAG/Applications/AppAvis/forms
' ************************************************