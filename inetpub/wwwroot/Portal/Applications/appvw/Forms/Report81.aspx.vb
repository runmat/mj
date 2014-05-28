Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Report81
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

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents cmdCreate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ucHeader As Header
    Protected WithEvents txtFahrgestellnummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents trCreate As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trSelectDropdown As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents calBis As System.Web.UI.WebControls.Calendar
    Protected WithEvents calVon As System.Web.UI.WebControls.Calendar
    Protected WithEvents txtReferenznummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBriefeingangVon As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBriefeingangBis As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnCal1 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents btnCal2 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
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
                trSelectDropdown.Visible = False
                Session("MultiResult") = Nothing
                DataGrid1.PageSize = 50
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        Session("ShowLink") = "True"
        If (txtBriefeingangBis.Text = String.Empty And txtBriefeingangVon.Text = String.Empty And txtFahrgestellnummer.Text = String.Empty And txtReferenznummer.Text = String.Empty) Then
            lblError.Text = "Bitte geben Sie mindestens ein Suchkriterium an."
            Exit Sub
        End If
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        Dim m_Report As New VW_02(m_User, m_App, "")
        Dim b As Boolean

        Try
            Dim datBriefeingangVon As DateTime
            Dim datBriefeingangBis As DateTime
            Dim strFahrgestellnummer As String
            Dim strReferenznummer As String

            b = True
            If txtReferenznummer.Text.Length = 0 Then
                strReferenznummer = ""
            Else
                'If (Not IsNumeric(Right(txtReferenznummer.Text, 6))) Or (Not txtReferenznummer.Text.Length = 8) Then
                '    lblError.Text = "Bitte geben Sie die Referenznummer 8-stellig ein (2 Buchstaben und 6 Ziffern)."
                '    b = False
                'Else
                strReferenznummer = txtReferenznummer.Text
                'End If
            End If

            txtFahrgestellnummer.Text = Replace(txtFahrgestellnummer.Text.Trim(" "c).Trim("*"c), " ", "")
            If txtFahrgestellnummer.Text.Length = 0 Then
                strFahrgestellnummer = ""
            Else
                strFahrgestellnummer = txtFahrgestellnummer.Text
                If strFahrgestellnummer.Length < 17 Then
                    If strFahrgestellnummer.Length > 7 Then
                        txtFahrgestellnummer.Text = "*" & strFahrgestellnummer
                        strFahrgestellnummer = "%" & strFahrgestellnummer
                    Else
                        lblError.Text = "Bitte geben Sie die Fahrgestellnummer mindestens 8-stellig ein."
                        b = False
                    End If
                End If
            End If
            If Len(txtBriefeingangVon.Text) > 0 Then
                If IsDate(txtBriefeingangVon.Text) Then
                    datBriefeingangVon = CDate(txtBriefeingangVon.Text)
                Else
                    lblError.Text = "Briefeingang Von - Muss einen Datumswert beinhalten."
                    b = False
                End If
            End If
            If Len(txtBriefeingangBis.Text) > 0 Then
                If IsDate(txtBriefeingangBis.Text) Then
                    datBriefeingangBis = CDate(txtBriefeingangBis.Text)
                Else
                    lblError.Text = "Briefeingang Bis - Muss einen Datumswert beinhalten."
                    b = False
                End If
            End If

            If txtBriefeingangVon.Text.Length + txtBriefeingangBis.Text.Length + txtFahrgestellnummer.Text.Length + txtReferenznummer.Text.Length = 0 Then
                lblError.Text = "Bitte geben Sie ein Suchkriterium ein."
                b = False
            Else
                If b Then
                    m_Report.FillHistory(Session("AppID").ToString, Session.SessionID.ToString, datBriefeingangVon, datBriefeingangBis, strFahrgestellnummer, strReferenznummer)
                    If m_Report.ResultCount > 1 Then
                        b = False
                        cmdCreate.Enabled = False
                        trSelectDropdown.Visible = True
                        txtBriefeingangVon.Enabled = False
                        txtBriefeingangBis.Enabled = False
                        txtReferenznummer.Enabled = False
                        txtFahrgestellnummer.Enabled = False
                        Session("MultiResult") = m_Report.Result
                        FillGrid(0, "Briefeingang")
                        lblError.Text = "Ihre Suche ergab mehrere Treffer. Bitte wählen Sie aus."
                    Else
                        Session("ResultTable") = m_Report.History
                    End If
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            b = False
        End Try

        If b Then
            If Not m_Report.Status = 0 Then
                lblError.Text = m_Report.Message
            Else
                If (m_Report.History Is Nothing) OrElse (m_Report.History.Rows.Count = 0) OrElse (m_Report.History.Rows(0)("ZZFAHRG").ToString = String.Empty) Then
                    lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                Else
                    Response.Redirect("Report81_2.aspx?AppID=" & Session("AppID").ToString)
                End If
            End If
        End If
        Session("ShowLink") = "False"
    End Sub

    'Private Sub cmdSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Session("ShowLink") = "True"
    '    txtAmtlKennzeichen.Text = ""
    '    txtBriefnummer.Text = ""
    '    txtFahrgestellnummer.Text = ddlSelect.SelectedItem.Value.ToString
    '    DoSubmit()
    'End Sub

    Private Sub btnCal1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCal1.Click
        calVon.Visible = True
        calBis.Visible = False
    End Sub

    Private Sub btnCal2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCal2.Click
        calVon.Visible = False
        calBis.Visible = True
    End Sub

    Private Sub calVon_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles calVon.SelectionChanged
        txtBriefeingangVon.Text = calVon.SelectedDate.ToShortDateString
        calVon.Visible = False
    End Sub

    Private Sub calBis_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles calBis.SelectionChanged
        txtBriefeingangBis.Text = calBis.SelectedDate.ToShortDateString
        calBis.Visible = False
    End Sub

    Private Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.ItemCommand
        Session("ShowLink") = "True"
        txtBriefeingangVon.Text = ""
        txtBriefeingangBis.Text = ""
        txtReferenznummer.Text = ""
        txtFahrgestellnummer.Text = e.Item.Cells(0).Text
        DoSubmit()
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If Session("MultiResult") Is Nothing Then
            trSelectDropdown.Visible = False
            lblError.Text = "Technisches Problem: Datenverlust - Bitte Erneuern Sie Ihre Anfrage."
        Else
            trSelectDropdown.Visible = True
            lblError.Text = ""

            Dim tmpDataView As New DataView()
            Dim tmpDataTable As New DataTable()
            tmpDataTable = CType(Session("MultiResult"), DataTable)
            tmpDataView = tmpDataTable.DefaultView

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
        End If
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
' $History: Report81.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 4.05.09    Time: 11:14
' Updated in $/CKAG/Applications/appvw/Forms
' ITA: 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 17:48
' Created in $/CKAG/Applications/appvw/Forms
' 
' *****************  Version 6  *****************
' User: Uha          Date: 20.06.07   Time: 14:32
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 5  *****************
' User: Uha          Date: 8.03.07    Time: 15:06
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Forms
' 
' ************************************************
