Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Report16
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

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private m_objTable As DataTable

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents cmdCreate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ucHeader As Header
    Protected WithEvents txtBriefnummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtFahrgestellnummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtHaendlerID As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtEingangsdatumBis As System.Web.UI.WebControls.TextBox
    Protected WithEvents rowHaendlerID As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblResults As System.Web.UI.WebControls.Label
    Protected WithEvents lnkExcel As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblDownloadTip As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents rowResultate As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents cmdDetails As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents txtEingangsdatumVon As System.Web.UI.WebControls.TextBox
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        rowHaendlerID.Visible = False
        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If Not IsPostBack Then
                Session("ShowLink") = "False"
                rowResultate.Visible = False
                cmdDetails.Visible = False
                Session("ResultTable") = Nothing
                Session("ResultTablePDIs") = Nothing
                InitialLoad()
            Else
                If (Session("ResultTableModelle") Is Nothing) Then
                    rowResultate.Visible = False
                    Session("ResultTable") = Nothing
                Else
                    m_objTable = CType(Session("ResultTableModelle"), DataTable)
                    'Fillgrid(0)
                End If
            End If

            'DoSubmit()
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub InitialLoad()
        '--------------------------------------------------------------------------------
        '§§§ JVE 02.05.2006: Datumsfelder vorbelegen (von = web.config, bis = Tagesdatum)
        Dim datEingangsdatumVon As DateTime

        Try
            datEingangsdatumVon = CType(ConfigurationManager.AppSettings("DatumVon02"), Date)
            txtEingangsdatumVon.Text = datEingangsdatumVon.ToShortDateString
            txtEingangsdatumBis.Text = Now.ToShortDateString
        Catch ex As Exception
            lblError.Text = "Fehler beim Seitenaufbau."
            cmdCreate.Enabled = False
        End Try
        '--------------------------------------------------------------------------------
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        'Session("ShowLink") = "True"
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        Session("lnkExcel") = ""
        Dim checkInput As Boolean = True

        Try
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            Dim m_Report As New Sixt_B02(m_User, m_App, strFileName)
            Dim strBriefnummer As String
            Dim datEingangsdatumVon As DateTime
            Dim datEingangsdatumBis As DateTime
            Dim strFahrgestellnummer As String
            Dim strHaendlerID As String

            If txtBriefnummer.Text.Length = 0 Then
                strBriefnummer = "|"
            Else
                strBriefnummer = txtBriefnummer.Text
            End If
            If txtFahrgestellnummer.Text.Length = 0 Then
                strFahrgestellnummer = "|"
            Else
                strFahrgestellnummer = txtFahrgestellnummer.Text
            End If
            If txtHaendlerID.Text.Length = 0 Then
                strHaendlerID = "|"
            Else
                strHaendlerID = txtHaendlerID.Text
            End If
            lblError.Text = ""

            'Datumsfelder prüfen
            checkInput = True
            'If ((txtEingangsdatumVon.Text.Length = 0) And (txtEingangsdatumBis.Text.Length < 0)) Then
            '    checkInput = False
            'End If
            'If (txtEingangsdatumVon.Text.Length > 0) And (txtEingangsdatumBis.Text.Length = 0) Then
            '    checkInput = False
            'End If
            'If checkInput Then
            '    If (txtEingangsdatumVon.Text.Length > 0) And (txtEingangsdatumBis.Text.Length > 0) Then
            '        If (Not IsDate(txtEingangsdatumVon.Text)) Or (Not IsDate(txtEingangsdatumBis.Text)) Then
            '            checkInput = False
            '        Else
            datEingangsdatumVon = CDate(txtEingangsdatumVon.Text)
            datEingangsdatumBis = CDate(txtEingangsdatumBis.Text)

            '            If (datEingangsdatumVon > datEingangsdatumBis) Then
            '                checkInput = False
            '                lblError.Text = "Eingangsdatum (von) muß kleiner oder gleich Eingangsdatum (bis) sein!<br>"
            '            End If
            '            If (datEingangsdatumBis.Subtract(datEingangsdatumVon).Days > 30) Then
            '                checkInput = False
            '                lblError.Text = "Der angegebene Zeitraum umfasst mehr als 30 Tage!<br>"
            '            End If
            '        End If
            '    End If
            'End If
            'Datumsfelder sind gefüllt und haben das richtige Format. Jetzt Werte prüfen.
            If checkInput Then
                m_Report.Fill(Session("AppID").ToString, Session.SessionID.ToString, Me, strBriefnummer, datEingangsdatumVon, datEingangsdatumBis, strFahrgestellnummer, strHaendlerID)

                If Not m_Report.Status = 0 Then
                    lblError.Text = m_Report.Message
                Else
                    If Not m_Report.Result.Rows.Count = 0 Then
                        lblResults.Text = "Es wurden " & m_Report.Result.Rows.Count.ToString & " nicht zulassungsfähige Fahrzeuge gefunden."
                        cmdDetails.Visible = True

                        Session("ResultTable") = m_Report.Result
                        Session("ResultTableModelle") = m_Report.ResultModelle

                        Try
                            Excel.ExcelExport.WriteExcel(m_Report.Result, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                        Catch
                        End Try
                        Session("lnkExcel") = "/Portal/Temp/Excel/" & strFileName
                        lnkExcel.NavigateUrl = Session("lnkExcel").ToString
                    End If

                    m_objTable = m_Report.ResultModelle
                    FillGrid(0)
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If m_objTable.Rows.Count = 0 Then
            rowResultate.Visible = False
            lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
        Else
            rowResultate.Visible = True

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

            If DataGrid1.PageCount > 1 Then
                DataGrid1.PagerStyle.Visible = True
                DataGrid1.PagerStyle.CssClass = "PagerStyle"
                If DataGrid1.CurrentPageIndex = DataGrid1.PageCount - 1 Then
                    DataGrid1.PagerStyle.NextPageText = "<img border=""0"" src=""/Portal/Images/empty.gif"" width=""12"" height=""11"">"
                Else
                    DataGrid1.PagerStyle.NextPageText = "<img border=""0"" src=""/Portal/Images/arrow_right.gif"" width=""12"" height=""11"">"
                End If

                If DataGrid1.CurrentPageIndex = 0 Then
                    DataGrid1.PagerStyle.PrevPageText = "<img border=""0"" src=""/Portal/Images/empty.gif"" width=""12"" height=""11"">"
                Else
                    DataGrid1.PagerStyle.PrevPageText = "<img border=""0"" src=""/Portal/Images/arrow_left.gif"" width=""12"" height=""11"">"
                End If
                DataGrid1.DataBind()
            Else
                DataGrid1.PagerStyle.Visible = False
            End If
        End If
    End Sub

    Private Sub cmdDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDetails.Click

        Session("ShowLink") = "True"
        Response.Redirect("/Portal/(S(" & Session.SessionID & "))/Shared/Report01_2.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub DataGrid1_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGrid1.ItemDataBound
        Dim intItem As Int32

        For intItem = 0 To m_objTable.Columns.Count - 1
            If m_objTable.Columns(intItem).DataType Is System.Type.GetType("System.DateTime") Then
                If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                    e.Item.Cells(intItem).Text = DataBinder.Eval(e.Item.DataItem, m_objTable.Columns(intItem).ColumnName, "{0:dd.MM.yyyy}")
                End If
            End If
        Next
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Report16.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 16.02.10   Time: 11:40
' Updated in $/CKAG/Applications/appsixt/Forms
' ITA: 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 4.05.09    Time: 10:16
' Updated in $/CKAG/Applications/appsixt/Forms
' ITA:2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 13:37
' Created in $/CKAG/Applications/appsixt/Forms
' 
' *****************  Version 12  *****************
' User: Uha          Date: 2.07.07    Time: 14:09
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 11  *****************
' User: Uha          Date: 23.05.07   Time: 9:11
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Forms
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 10  *****************
' User: Uha          Date: 22.05.07   Time: 11:27
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Forms
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 9  *****************
' User: Uha          Date: 12.03.07   Time: 17:21
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Forms
' Imagepfad geändert (Customize -> Images)
' 
' *****************  Version 8  *****************
' User: Uha          Date: 8.03.07    Time: 13:15
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Forms
' 
' ************************************************
