Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Report04
    Inherits System.Web.UI.Page
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents cmdCreate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdDetails As System.Web.UI.WebControls.LinkButton
    Protected WithEvents calVon As System.Web.UI.WebControls.Calendar
    Protected WithEvents calBis As System.Web.UI.WebControls.Calendar
    Protected WithEvents lnkExcel As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblDownloadTip As System.Web.UI.WebControls.Label
    Protected WithEvents txtPDI As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtEingangsdatumVon As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnVon As System.Web.UI.WebControls.LinkButton
    Protected WithEvents txtEingangsdatumBis As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnBis As System.Web.UI.WebControls.LinkButton
    Protected WithEvents txtFahrgestellnummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtModell As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblResults As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents rowResultate As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
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
    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private m_objTable As DataTable

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
                Session("ShowLink") = "False"
                rowResultate.Visible = False
                cmdDetails.Visible = False
                Session("ResultTable") = Nothing
                Session("ResultTablePDIs") = Nothing
            Else
                If (Session("ResultTablePDIs") Is Nothing) Then
                    rowResultate.Visible = False
                    Session("ResultTable") = Nothing
                Else
                    m_objTable = CType(Session("ResultTablePDIs"), DataTable)
                End If
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub btnVon_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVon.Click
        calVon.Visible = True
        calBis.Visible = False
    End Sub

    Private Sub btnBis_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBis.Click
        calVon.Visible = False
        calBis.Visible = True
    End Sub

    Private Sub calVon_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calVon.SelectionChanged
        Me.txtEingangsdatumVon.Text = calVon.SelectedDate.ToShortDateString
        calVon.Visible = False
    End Sub

    Private Sub calBis_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calBis.SelectionChanged
        Me.txtEingangsdatumBis.Text = calBis.SelectedDate.ToShortDateString
        calBis.Visible = False
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        Dim checkInput As Boolean = True

        Session("lnkExcel") = ""

        Try
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            Dim m_Report As New ec_06(m_User, m_App, strFileName)
            Dim strPDI As String
            Dim strFahrgestellnummer As String
            Dim strModell As String


            lblError.Text = ""

            checkInput = True


            'Datumsfelder prüfen
            If checkInput Then
                If (txtEingangsdatumVon.Text.Length > 0) OrElse (txtEingangsdatumBis.Text.Length > 0) Then

                    checkInput = CheckDate(txtEingangsdatumVon.Text, txtEingangsdatumBis.Text)

                    If (txtEingangsdatumVon.Text.Length > 0) AndAlso (txtEingangsdatumBis.Text.Length > 0) Then
                        If (txtEingangsdatumVon.Text > txtEingangsdatumBis.Text) Then
                            checkInput = False
                            lblError.Text = "Eingangsdatum (von) muß kleiner oder gleich Eingangsdatum (bis) sein!<br>"
                        End If
                    End If

                       

                End If
            End If


            strPDI = Me.txtPDI.Text
            strFahrgestellnummer = Me.txtFahrgestellnummer.Text
            strModell = Me.txtModell.Text

            If checkInput Then


                m_Report.Fill(Session("AppID").ToString, Session.SessionID.ToString, strPDI, txtEingangsdatumVon.Text, txtEingangsdatumBis.Text, strFahrgestellnummer, strModell, Me)

                If Not m_Report.Status = 0 Then
                    lblError.Text = m_Report.Message
                Else
                    If Not m_Report.Result.Rows.Count = 0 Then

                        lblResults.Text = "Es wurden " & m_Report.Result.Rows.Count.ToString & " Fahrzeuge gefunden."
                        cmdDetails.Visible = True

                        Session("ResultTable") = m_Report.Result
                        Session("ResultTablePDIs") = m_Report.ResultPDIs

                        Try
                            Excel.ExcelExport.WriteExcel(m_Report.Result, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                        Catch
                        End Try


                        If m_Report.Result.Rows.Count = 1 Then
                            Session("ShowLink") = "True"
                            Response.Redirect("/Portal/(S(" & Session.SessionID & "))/Shared/Report01_2.aspx?AppID=" & Session("AppID").ToString)
                        Else
                            Dim AbsoluterPfadZumVirituellenVerz As String = ConfigurationManager.AppSettings("ReplaceExcelPath")
                            Session("lnkExcel") = ConfigurationManager.AppSettings("ExcelPath").Replace(AbsoluterPfadZumVirituellenVerz, "") & strFileName & "".Replace("/", "\")
                            lnkExcel.NavigateUrl = Session("lnkExcel").ToString
                        End If

                    End If

                    m_objTable = m_Report.ResultPDIs
                    FillGrid(0)
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Function CheckDate(ByVal strDateVon As String, ByVal strdateBis As String) As Boolean

        Dim bReturn As Boolean = True

        If String.IsNullOrEmpty(strDateVon) Then
            If IsDate(strDateVon) = False Then
                lblError.Text = "Bitte geben Sie ein gültiges Datum ein."
                bReturn = False
            End If
        End If

        If String.IsNullOrEmpty(strdateBis) Then
            If IsDate(strdateBis) = False Then
                lblError.Text = "Bitte geben Sie ein gültiges Datum ein."
                bReturn = False
            End If
        End If
        
        Return bReturn

    End Function



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

    Private Sub cmdDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDetails.Click
        Session("ShowLink") = "True"
        Response.Redirect("/Portal/(S(" & Session.SessionID & "))/Shared/Report01_2.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Report04.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 26.06.09   Time: 11:29
' Updated in $/CKAG/Applications/appec/Forms
' ITA 2918
' Z_M_EC_AVM_BRIEFLEBENSLAUF,Z_M_Ec_Avm_Fzg_M_Dfs_O_Zul,Z_M_EC_AVM_FZG_OH
' NE_BRIEF,Z_M_Ec_Avm_Fzg_Ohne_Unitnr,Z_M_Ec_Avm_Nur_Brief_Vorh,
' Z_M_EC_AVM_OFFENE_ZAHLUNGEN,  Z_M_EC_AVM_PDI_BESTAND,
' Z_M_EC_AVM_STATUS_EINSTEUERUNG,  Z_M_EC_AVM_STATUS_GREENWAY,
' Z_M_Ec_Avm_Status_Zul, Z_M_EC_AVM_ZULASSUNGEN, Z_M_Ec_Avm_Zulassungen_2
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 17.06.09   Time: 8:47
' Updated in $/CKAG/Applications/appec/Forms
' Warnungen entfernt!
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 15:49
' Created in $/CKAG/Applications/appec/Forms
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 14.12.07   Time: 13:45
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' Anpassung Excel Links, wegen Webconfig Änderung, jetzt Variabel ab
' Virtuellem Verzeichnis
' 
' *****************  Version 8  *****************
' User: Uha          Date: 2.07.07    Time: 12:29
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 7  *****************
' User: Uha          Date: 23.05.07   Time: 9:47
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 6  *****************
' User: Uha          Date: 22.05.07   Time: 13:31
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 5  *****************
' User: Uha          Date: 12.03.07   Time: 16:15
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' 
' *****************  Version 4  *****************
' User: Uha          Date: 7.03.07    Time: 11:02
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' 
' ************************************************
