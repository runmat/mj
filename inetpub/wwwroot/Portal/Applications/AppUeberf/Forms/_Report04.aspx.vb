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
    Protected WithEvents txtAuftragdatumBis As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnCal3 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents btnCal4 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cal1 As System.Web.UI.WebControls.Calendar
    Protected WithEvents cal2 As System.Web.UI.WebControls.Calendar
    Protected WithEvents cal3 As System.Web.UI.WebControls.Calendar
    Protected WithEvents cal4 As System.Web.UI.WebControls.Calendar
    Protected WithEvents rbAuftragart As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents lnkExcel As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblDownloadTip As System.Web.UI.WebControls.Label
    Protected WithEvents lnkKreditlimit As System.Web.UI.WebControls.HyperLink
    Protected WithEvents trKUNNR As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lbl_KundenNr As System.Web.UI.WebControls.Label
    Protected WithEvents cmb_KundenNr As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblScript As System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Leasingkunde As System.Web.UI.WebControls.Label
    Protected WithEvents txt_Leasingkunde As System.Web.UI.WebControls.TextBox
    Protected WithEvents lbl_Leasinggesellschaft As System.Web.UI.WebControls.Label
    Protected WithEvents txt_Leasinggesellschaft As System.Web.UI.WebControls.TextBox
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
                lblDownloadTip.Visible = False
                lnkExcel.Visible = False

                Dim uebf As New Ueberfuehrung(m_User, m_App, Session("AppID").ToString, Session.SessionID, String.Empty)
                Dim dv As DataView = uebf.readGroupData(Page)
                If Not dv Is Nothing Then
                    If dv.Table.Rows.Count > 0 Then
                        With cmb_KundenNr
                            .DataSource = dv
                            .DataValueField = "KUNNR"
                            .DataTextField = "Anzeige"
                            .DataBind()
                        End With
                    Else
                        trKUNNR.Visible = False
                    End If
                End If
            End If
            If trKUNNR.Visible And cmb_KundenNr.Items.Count = 0 Then
                cmb_KundenNr.Visible = False
            End If

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

        If rbAuftragart.SelectedItem.Value = "N" Then
            If (txtAuftragdatum.Text = String.Empty) And (txtAuftragdatumBis.Text = String.Empty) Then
                txtAuftragdatum.Text = DateAdd(DateInterval.Day, -90, System.DateTime.Today)
                txtAuftragdatumBis.Text = System.DateTime.Today
            End If
        Else
            'Alles leer? Fehler!    
            If ((txtAuftrag.Text = String.Empty) And (txtReferenz.Text = String.Empty) And (txtAuftragdatum.Text = String.Empty) And (txtAuftragdatumBis.Text = String.Empty) And (txtKennzeichen.Text = String.Empty) And (txtUeberfuehrungdatumVon.Text = String.Empty) And (txtUeberfuehrungdatumBis.Text = String.Empty)) Then

                If txt_Leasinggesellschaft.Visible = False AndAlso txt_Leasingkunde.Visible = False Then
                    'ist wenn nicht Krüll Report JJ2007.11.13
                    status = "Keine Abfragekriterien angegeben."
                    Return False
                Else
                    'wenn KrüllReport und diese Felder auch nicht ausgefüllt sind JJ2007.11.13
                    If txt_Leasinggesellschaft.Text = String.Empty AndAlso txt_Leasingkunde.Text = String.Empty Then
                        status = "Keine Abfragekriterien angegeben."
                        Return False
                    End If
                End If

            End If
        End If


        Return True
    End Function

    Private Sub DoSubmit()
        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)

        Try

            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"

            Dim uebf As New Ueberfuehrung(m_User, m_App, Session("AppID").ToString, Session.SessionID, strFileName)
            Dim table As DataTable

            Dim kunnr As String = String.Empty
            If Not cmb_KundenNr.SelectedItem Is Nothing Then
                kunnr = cmb_KundenNr.SelectedItem.Value
                Session("kunnr") = kunnr
            Else
                Session("kunnr") = m_User.KUNNR
            End If

            uebf.readSAPAuftraege2(txtAuftrag.Text, txtAuftragdatum.Text, txtAuftragdatumBis.Text, txtReferenz.Text, txtKennzeichen.Text, _
                                   txtUeberfuehrungdatumVon.Text, txtUeberfuehrungdatumBis.Text, rbAuftragart.SelectedItem.Value, kunnr, _
                                   txt_Leasinggesellschaft.Text, txt_Leasingkunde.Text)
            table = uebf.Result

            If (uebf.Status = 0 AndAlso table.Rows.Count > 0) Then
                Session("ResultTable") = table

                'Dim objExcelExport As New Excel.ExcelExport()
                Try
                    Dim tblExcel As New DataTable()
                    tblExcel.Columns.Add("Referenz", table.Columns("Zzrefnr").DataType)
                    tblExcel.Columns.Add("Auftrag", table.Columns("Aufnr").DataType)
                    tblExcel.Columns.Add("Auftragsdatum", table.Columns("Wadat").DataType)
                    tblExcel.Columns.Add("Fahrt", table.Columns("Fahrtnr").DataType)
                    tblExcel.Columns.Add("Kennzeichen", table.Columns("Zzkenn").DataType)
                    tblExcel.Columns.Add("Typ", table.Columns("Zzbezei").DataType)
                    tblExcel.Columns.Add("Leistungsdatum", table.Columns("VDATU").DataType)
                    tblExcel.Columns.Add("Abgabedatum", table.Columns("wadat_ist").DataType)
                    tblExcel.Columns.Add("Von", table.Columns("Fahrtvon").DataType)
                    tblExcel.Columns.Add("Nach", table.Columns("Fahrtnach").DataType)
                    tblExcel.Columns.Add("Km", table.Columns("Gef_Km").DataType)
                    tblExcel.Columns.Add("Klärfall", table.Columns("KFTEXT").DataType)

                    Dim tmpRow As DataRow
                    Dim tmpNew As DataRow
                    For Each tmpRow In table.Rows
                        tmpNew = tblExcel.NewRow

                        tmpNew("Referenz") = tmpRow("Zzrefnr")

                        tmpNew("Auftrag") = tmpRow("Aufnr")
                        tmpNew("Auftragsdatum") = tmpRow("Wadat")
                        tmpNew("Fahrt") = tmpRow("Fahrtnr")
                        tmpNew("Kennzeichen") = tmpRow("Zzkenn")
                        tmpNew("Typ") = tmpRow("Zzbezei")
                        tmpNew("Leistungsdatum") = tmpRow("VDATU")
                        tmpNew("Abgabedatum") = tmpRow("wadat_ist")
                        tmpNew("Von") = tmpRow("Fahrtvon")
                        tmpNew("Nach") = tmpRow("Fahrtnach")
                        tmpNew("Km") = tmpRow("Gef_Km")
                        If CStr(tmpRow("KFTEXT")).Trim(" "c) <> String.Empty Then
                            tmpNew("Klärfall") = "X"
                        Else
                            tmpNew("Klärfall") = ""
                        End If

                        tblExcel.Rows.Add(tmpNew)
                    Next
                    Excel.ExcelExport.WriteExcel(tblExcel, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                    lblDownloadTip.Visible = True
                    lnkExcel.Visible = True
                    lnkExcel.NavigateUrl = "/Portal/Temp/Excel/" & strFileName
                Catch
                End Try

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

            Dim tmpDataView As DataView = table.DefaultView

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

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        Session("ShowLink") = "True"
        Dim status As String = ""

        lblDownloadTip.Visible = False
        lnkExcel.Visible = False

        If checkInput(status) Then
            DoSubmit()
        Else
            lblError.Text = status
        End If
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

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub
    
End Class

' ************************************************
' $History: _Report04.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 2.02.09    Time: 11:38
' Updated in $/CKAG/Applications/AppUeberf/Forms
' ITA: 2578
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 16.04.08   Time: 11:53
' Updated in $/CKAG/Applications/AppUeberf/Forms
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 16:06
' Created in $/CKAG/Applications/AppUeberf/Forms
' 
' *****************  Version 14  *****************
' User: Jungj        Date: 13.11.07   Time: 10:15
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 13  *****************
' User: Jungj        Date: 12.11.07   Time: 17:27
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 12  *****************
' User: Jungj        Date: 9.11.07    Time: 14:58
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 8.11.07    Time: 18:10
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 8.11.07    Time: 10:45
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 8.11.07    Time: 10:17
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 8  *****************
' User: Uha          Date: 2.07.07    Time: 10:05
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 7  *****************
' User: Uha          Date: 26.06.07   Time: 8:19
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Fahrzeugueberfuehrungen: Es fehlte das Schreiben der Kunnr in die
' Session (_Report04.aspx.vb).
' 
' *****************  Version 6  *****************
' User: Fassbenders  Date: 21.06.07   Time: 16:27
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 20.06.07   Time: 11:03
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 19.06.07   Time: 15:54
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 3  *****************
' User: Uha          Date: 22.05.07   Time: 11:35
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 2  *****************
' User: Uha          Date: 22.05.07   Time: 10:48
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 1  *****************
' User: Uha          Date: 13.03.07   Time: 13:19
' Created in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' _Report04 aus Share hinzugefügt
' 
' ************************************************
