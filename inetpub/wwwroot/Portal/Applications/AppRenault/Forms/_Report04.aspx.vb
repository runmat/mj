Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

<CLSCompliant(False)> Public Class _Report04
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

    Protected WithEvents ImageButton1 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents ucHeader As Header
    Protected WithEvents calZul As System.Web.UI.WebControls.Calendar
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents btnCal2 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents txtUeberfuehrungdatumBis As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnCal1 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents txtUeberfuehrungdatumVon As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    Protected WithEvents txtAuftragdatum As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblAuftragdatum As System.Web.UI.WebControls.Label
    Protected WithEvents txtKennzeichen As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblKennzeichen As System.Web.UI.WebControls.Label
    Protected WithEvents txtReferenz As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblReferenz As System.Web.UI.WebControls.Label
    Protected WithEvents cmdCreate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents txtAuftrag As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblAuftrag As System.Web.UI.WebControls.Label
    Protected WithEvents lblScript As System.Web.UI.WebControls.Label
    Protected WithEvents txtAuftragdatumBis As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnCal3 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents btnCal4 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cal1 As System.Web.UI.WebControls.Calendar
    Protected WithEvents cal2 As System.Web.UI.WebControls.Calendar
    Protected WithEvents cal3 As System.Web.UI.WebControls.Calendar
    Protected WithEvents cal4 As System.Web.UI.WebControls.Calendar
    Protected WithEvents rbAuftragart As System.Web.UI.WebControls.RadioButtonList
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

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Function checkInput(ByRef status As String) As Boolean
        Dim datevon As Date
        Dim datebis As Date
        Dim span As New TimeSpan(90, 0, 0, 0)

        status = String.Empty

        Try
            If txtUeberfuehrungdatumVon.Text <> String.Empty Then
                datevon = CType(txtUeberfuehrungdatumVon.Text, Date)
                If txtUeberfuehrungdatumBis.Text = String.Empty Then
                    datebis = datevon
                    txtUeberfuehrungdatumBis.Text = txtUeberfuehrungdatumVon.Text
                End If
            End If
            If txtUeberfuehrungdatumBis.Text <> String.Empty Then
                datebis = CType(txtUeberfuehrungdatumBis.Text, Date)
                If txtUeberfuehrungdatumVon.Text = String.Empty Then
                    datevon = datebis
                    txtUeberfuehrungdatumVon.Text = txtUeberfuehrungdatumBis.Text
                End If
            End If
        Catch ex As Exception
            status = "Überführungsdatum ungültig."
            Return False
        End Try
        If (datevon > datevon) Then
            status = "Überführungsdatum (von) muss vor Überführungsdatum (bis) liegen."
            Return False
        Else
            If DateAdd(DateInterval.Month, 3, datevon) < datebis Then
                status = "Der maximale Zeitraum (""Überführungsdatum von"" - ""Überführungsdatum bis"") beträgt drei Monate!<br>"
                Return False
            End If
        End If

        'Auftragsdatum
        Try
            If txtAuftragdatum.Text <> String.Empty Then
                datevon = CType(txtAuftragdatum.Text, Date)
                If txtAuftragdatumBis.Text = String.Empty Then
                    datebis = datevon
                    txtAuftragdatumBis.Text = txtAuftragdatum.Text
                End If
            End If
            If txtAuftragdatumBis.Text <> String.Empty Then
                datebis = CType(txtAuftragdatumBis.Text, Date)
                If txtAuftragdatum.Text = String.Empty Then
                    datevon = datebis
                    txtAuftragdatum.Text = txtAuftragdatumBis.Text
                End If
            End If
        Catch ex As Exception
            status = "Auftragsdatum ungültig."
            Return False
        End Try
        If (datevon > datebis) Then
            status = "Auftragsdatum (von) muss vor Auftragsdatum (bis) liegen."
            Return False
        Else
            If DateAdd(DateInterval.Month, 3, datevon) < datebis Then
                status = "Der maximale Zeitraum (""Auftragsdatum von"" - ""Auftragsdatum bis"") beträgt drei Monate!<br>"
                Return False
            End If
        End If

        'Alles leer? Fehler!    
        If ((txtAuftrag.Text = String.Empty) And (txtReferenz.Text = String.Empty) And (txtAuftragdatum.Text = String.Empty) And (txtAuftragdatumBis.Text = String.Empty) And (txtKennzeichen.Text = String.Empty) And (txtUeberfuehrungdatumVon.Text = String.Empty) And (txtUeberfuehrungdatumBis.Text = String.Empty)) Then
            status = "Keine Abfragekriterien angegeben."
            Return False
        End If
        Return True
    End Function

    Private Sub DoSubmit()
        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)

        Try

            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            '§§§ JVE 22.02.2006 Methode "readSAPAuftraege" in Base.Business.Ueberfuehrung eingebaut (neue Klasse), weil von ALD und ALDA verwendet.
            '                   Gleichzeitig aus der AppDCL.Ueberfuehrung-Klasse entfernt!
            'Dim uebf As New AppDCL.Ueberfuehrung(m_User, m_App, Session("AppID").ToString, Session.SessionID, strFileName)
            Dim uebf As New AppUeberf.Ueberfuehrung(m_User, m_App, Session("AppID").ToString, Session.SessionID, strFileName)
            Dim table As DataTable

            uebf.readSAPAuftraege(txtAuftrag.Text, txtAuftragdatum.Text, txtAuftragdatumBis.Text, txtReferenz.Text, txtKennzeichen.Text, txtUeberfuehrungdatumVon.Text, txtUeberfuehrungdatumBis.Text, rbAuftragart.SelectedItem.Value)
            table = uebf.Result

            If (uebf.Status = 0 AndAlso table.Rows.Count > 0) Then
                Session("ResultTable") = table
                FillGrid(0)
            Else
                Session("ResultTable") = Nothing
                FillGrid(0)
                lblError.Text = uebf.Message
            End If


        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            logApp.UpdateEntry("ERR", Session("AppID").ToString, "Fehler im Report <Optisches Archiv (Recherche Überführungen)>. Fehler: " & ex.Message)
        End Try
    End Sub


    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        Dim table As DataTable

        table = CType(Session("ResultTable"), DataTable)

        If (table Is Nothing) OrElse (table.Rows.Count = 0) Then
            DataGrid1.Visible = False
            lblNoData.Visible = True
            lblNoData.Text = "Keine Daten zur Anzeige gefunden."
            'ShowScript.Visible = False
        Else
            DataGrid1.Visible = True
            lblNoData.Visible = False

            Dim tmpDataView As New DataView()
            tmpDataView = table.DefaultView

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

            If (Not Session("ShowOtherString") Is Nothing) AndAlso CStr(Session("ShowOtherString")).Length > 0 Then
                lblNoData.Text = CStr(Session("ShowOtherString"))
            Else
                lblNoData.Text = "Es wurden " & tmpDataView.Count.ToString & " Einträge zu """ & m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString & """ gefunden."
            End If
            If (Not Session("BackLink") Is Nothing) AndAlso CStr(Session("BackLink")) = "HistoryBack" Then
                'lnkKreditlimit.Text = "Zurück"
                'lnkKreditlimit.NavigateUrl = "javascript:history.back()"
            End If
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
            Dim control As Control
            Dim hyperlink As HyperLink

            For Each item In DataGrid1.Items
                For Each cell In item.Cells
                    For Each control In cell.Controls
                        If TypeOf control Is HyperLink Then
                            hyperlink = CType(control, HyperLink)
                            hyperlink.NavigateUrl &= "&AppID=" & CStr(Request.QueryString("AppID"))
                        End If
                    Next
                Next
            Next
        End If
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        Session("ShowLink") = "True"
        Dim status As String = ""

        If checkInput(status) Then
            DoSubmit()
        Else
            lblError.Text = status
        End If
    End Sub

    Private Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.ItemCommand
        Dim table As DataTable
        Dim rowNr As Integer
        table = CType(Session("ResultTable"), DataTable)

        If (e.CommandName = "showDetails") Then
            rowNr = e.Item.Cells(0).Text
        End If
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub btnCal1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCal1.Click
        cal1.Visible = True
        cal2.Visible = False
        cal3.Visible = False
        cal4.Visible = False
    End Sub

    Private Sub btnCal2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCal2.Click
        cal1.Visible = False
        cal2.Visible = True
        cal3.Visible = False
        cal4.Visible = False
    End Sub

    Private Sub btnCal3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCal3.Click
        cal1.Visible = False
        cal2.Visible = False
        cal3.Visible = True
        cal4.Visible = False
    End Sub

    Private Sub btnCal4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCal4.Click
        cal1.Visible = False
        cal2.Visible = False
        cal3.Visible = False
        cal4.Visible = True
    End Sub

    Private Sub cal1_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cal1.SelectionChanged
        txtAuftragdatum.Text = cal1.SelectedDate.ToShortDateString
        cal1.Visible = False
    End Sub

    Private Sub cal2_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cal2.SelectionChanged
        txtAuftragdatumBis.Text = cal2.SelectedDate.ToShortDateString
        cal2.Visible = False
    End Sub

    Private Sub cal3_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cal3.SelectionChanged
        txtUeberfuehrungdatumVon.Text = cal3.SelectedDate.ToShortDateString
        cal3.Visible = False
    End Sub

    Private Sub cal4_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cal4.SelectionChanged
        txtUeberfuehrungdatumBis.Text = cal4.SelectedDate.ToShortDateString
        cal4.Visible = False
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
' $History: _Report04.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 4.05.09    Time: 9:45
' Updated in $/CKAG/Applications/AppRenault/Forms
' ITA:2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 12:55
' Created in $/CKAG/Applications/AppRenault/Forms
' 
' *****************  Version 5  *****************
' User: Uha          Date: 9.08.07    Time: 17:33
' Updated in $/CKG/Applications/AppRenault/AppRenaultWeb/Forms
' Beim Wechsel von _Report04 zu _Report041 fehlte die AppID -> Gefixt
' 
' *****************  Version 4  *****************
' User: Uha          Date: 2.07.07    Time: 13:36
' Updated in $/CKG/Applications/AppRenault/AppRenaultWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 3  *****************
' User: Uha          Date: 8.03.07    Time: 11:34
' Updated in $/CKG/Applications/AppRenault/AppRenaultWeb/Forms
' 
' ************************************************
