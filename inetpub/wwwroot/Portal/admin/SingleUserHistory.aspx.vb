
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements


Public Class SingleUserHistory
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
    Private m_User As User
    Private m_App As App

    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents txtAbDatum As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnOpenSelectAb As System.Web.UI.WebControls.Button
    Protected WithEvents calAbDatum As System.Web.UI.WebControls.Calendar
    Protected WithEvents txtBisDatum As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnOpenSelectBis As System.Web.UI.WebControls.Button
    Protected WithEvents calBisDatum As System.Web.UI.WebControls.Calendar
    Protected WithEvents cmdCreate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents TblSearch As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents TblLog As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents lblDownloadTip As System.Web.UI.WebControls.Label
    Protected WithEvents lnkExcel As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents txtUserName As System.Web.UI.WebControls.TextBox
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucStyles.TitleText = "Benutzerhistorie"
        AdminAuth(Me, m_User, AdminLevel.Organization)

        Try
            m_App = New App(m_User)

            If Not IsPostBack Then
                Dim tmpUser As New Base.Kernel.Security.User(CInt(Request.QueryString("UserID")), m_User.App.Connectionstring)
                txtUserName.Text = tmpUser.UserName

                If Not m_User.Customer.AccountingArea = -1 Then
                    If m_User.Customer.AccountingArea = tmpUser.Customer.AccountingArea Then
                        If (m_User.HighestAdminLevel < AdminLevel.Master) AndAlso (Not (m_User.Customer.CustomerId = tmpUser.Customer.CustomerId)) Then
                            Throw New Exception("Sie dürfen auf den Benutzer nicht zugreifen.")
                        End If
                    Else
                        Throw New Exception("Sie dürfen auf den Benutzer nicht zugreifen.")
                    End If
                End If

                calAbDatum.SelectedDate = Today.AddDays(-30)
                txtAbDatum.Text = calAbDatum.SelectedDate.ToShortDateString

                calBisDatum.SelectedDate = Today
                txtBisDatum.Text = calBisDatum.SelectedDate.ToShortDateString

                ucStyles.TitleText = "Benutzerhistorie von Benutzer """ & tmpUser.UserName & """"
            End If
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "SingleUserHistory", "Page_Load", ex.ToString)

            lblError.Text = ex.ToString
            lblError.Visible = True
        End Try
    End Sub

    Private Sub FillDataGrid1(ByVal blnForceNew As Boolean, ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        Dim dt As New DataTable()
        'If Not blnForceNew AndAlso (Not m_context.Cache("UserResult") Is Nothing) Then
        '    dt = CType(m_context.Cache("UserResult"), DataTable)
        If Not blnForceNew AndAlso (Not Session("UserResult") Is Nothing) Then
            dt = CType(Session("UserResult"), DataTable)
        Else
            If Not IsDate(Me.txtAbDatum.Text) Then
                Me.lblError.Text = "Bitte gültiges Startdatum übergeben."
                Exit Sub
            ElseIf Not IsDate(Me.txtBisDatum.Text) Then
                Me.lblError.Text = "Bitte gültiges Enddatum übergeben."
                Exit Sub
            ElseIf CDate(txtAbDatum.Text) > CDate(txtBisDatum.Text) Then
                lblError.Text = "Das Enddatum darf nicht vor dem Anfangsdatum liegen!"
                Exit Sub
            Else
                Dim strTemp As String
                strTemp = CStr(CDate(Me.txtBisDatum.Text).AddDays(1))

                Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
                cn.Open()
                Dim da As New SqlClient.SqlDataAdapter("SELECT ID, [Action] AS Aktion, LastChangedBy AS [Änderer], OldValue AS [Alter Wert], NewValue AS [Neuer Wert], InsertDate AS Zeitpunkt, SaveType AS Typ FROM AdminHistory_User WHERE (Username = @Username) AND (InsertDate BETWEEN CONVERT ( Datetime , '" & Me.txtAbDatum.Text & "' , 104 ) AND CONVERT ( Datetime , '" & strTemp & "' , 104 )) ", cn)
                With da.SelectCommand.Parameters
                    .AddWithValue("@Username", txtUserName.Text)
                End With
                da.Fill(dt)
                'm_context.Cache.Insert("UserResult", dt, New System.Web.Caching.CacheDependency(Server.MapPath("SingleUserHistory.aspx")), DateTime.Now.AddMinutes(20), TimeSpan.Zero)
                Session("UserResult") = dt
            End If
        End If

        If dt.Rows.Count = 0 Then
            Me.lblError.Text = "Keine Ergebnisse für den genannten Zeitraum."
            Exit Sub
        End If
        If strSort.Length > 0 Then
            If CStr(Me.ViewState("mySort")) = strSort Then
                strSort &= " DESC"
            End If
        Else
            If CStr(Me.ViewState("mySort")).Length = 0 Then
                strSort = "ID DESC"
            Else
                strSort = CStr(Me.ViewState("mySort"))
            End If
        End If
        Me.ViewState("mySort") = strSort

        dt.DefaultView.Sort = strSort
        With Me.DataGrid1
            .CurrentPageIndex = intPageIndex
            .DataSource = dt
            .DataBind()
            .Visible = True
        End With
        Me.TblLog.Visible = True
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillDataGrid1(False, DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        FillDataGrid1(False, e.NewPageIndex)
    End Sub

    Private Sub btnOpenSelectAb_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpenSelectAb.Click
        calAbDatum.Visible = True
    End Sub

    Private Sub btnOpenSelectBis_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpenSelectBis.Click
        calBisDatum.Visible = True
    End Sub

    Private Sub calAbDatum_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calAbDatum.SelectionChanged
        txtAbDatum.Text = calAbDatum.SelectedDate.ToShortDateString
    End Sub

    Private Sub calBisDatum_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calBisDatum.SelectionChanged
        txtBisDatum.Text = calBisDatum.SelectedDate.ToShortDateString
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        lblError.Text = ""
        DataGrid1.Visible = False

        Me.ViewState("mySort") = ""
        FillDataGrid1(True, 0)

        lblDownloadTip.Visible = False
        lnkExcel.Visible = False
        lnkExcel.NavigateUrl = ""
        DataGrid1.Visible = False

        If Not Session("UserResult") Is Nothing Then
            Dim tblResult As DataTable
            tblResult = CType(Session("UserResult"), DataTable)
            If Not tblResult.Rows.Count = 0 Then
                Dim objExcelExport As New Base.Kernel.Excel.ExcelExport()
                Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
                Try
                    Base.Kernel.Excel.ExcelExport.WriteExcel(tblResult, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                Catch
                End Try
                lblDownloadTip.Visible = True
                DataGrid1.Visible = True
                lnkExcel.Visible = True
                lnkExcel.NavigateUrl = "/Portal/Temp/Excel/" & strFileName
            End If
        End If

    End Sub

    Private Sub calAbDatum_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles calAbDatum.Load
        calAbDatum.Visible = False
    End Sub

    Private Sub calBisDatum_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles calBisDatum.Load
        calBisDatum.Visible = False
    End Sub

    Private Sub calAbDatum_VisibleMonthChanged(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MonthChangedEventArgs) Handles calAbDatum.VisibleMonthChanged
        calAbDatum.Visible = True
    End Sub

    Private Sub calBisDatum_VisibleMonthChanged(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MonthChangedEventArgs) Handles calBisDatum.VisibleMonthChanged
        calBisDatum.Visible = True
    End Sub

End Class

' ************************************************
' $History: SingleUserHistory.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 6.01.09    Time: 11:45
' Updated in $/CKAG/admin
' ITA 2503  Cache durch Session ersetzt
' 
' *****************  Version 2  *****************
' User: Hartmannu    Date: 11.09.08   Time: 11:34
' Updated in $/CKAG/admin
' Fixing Admin-Änderungen
' 
' *****************  Version 1  *****************
' User: Hartmannu    Date: 10.09.08   Time: 17:28
' Created in $/CKAG/admin
' ITA 2027 - Anzeige der erweiterten Benutzerhistorie
' 
' ************************************************
