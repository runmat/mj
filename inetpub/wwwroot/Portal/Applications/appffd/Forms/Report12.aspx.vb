Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common
Imports System.IO

Public Class Report12
    Inherits System.Web.UI.Page
    '##### MDR Report "Versendete Zulassungsdaten"
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
    Protected WithEvents ImageButton1 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents btnCal1 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents calVon As System.Web.UI.WebControls.Calendar
    Protected WithEvents btnCal2 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents calBis As System.Web.UI.WebControls.Calendar
    Protected WithEvents txtDatVon As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDatBis As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDateiname As System.Web.UI.WebControls.TextBox
    Protected WithEvents ListBox1 As System.Web.UI.WebControls.ListBox
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents tblResult As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents lnkCreateCSV As System.Web.UI.WebControls.LinkButton
    Protected WithEvents trCSV As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblDownloadTip As System.Web.UI.WebControls.Label
    Protected WithEvents lnkShowExcel As System.Web.UI.WebControls.HyperLink
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
                tblResult.Visible = False
                trCSV.Visible = False
                Session("ReportMDR") = Nothing
                txtDatVon.Text = Now.ToShortDateString
                txtDatBis.Text = Now.ToShortDateString
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        Session("ShowLink") = "True"
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        Session("lnkExcel") = ""
        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)

        Try
            trCSV.Visible = False
            tblResult.Visible = False
            Session("ReportMDR") = Nothing

            'Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".csv"
            'Dim m_Report As New MDR_02(m_User, m_App, strFileName)
            Dim m_Report As New MDR_02(m_User, m_App, "")

            'Plausi-Prüfungen...
            Dim datAb As Date
            Dim datBis As Date

            If (txtDatVon.Text.Trim = String.Empty) And (txtDatBis.Text.Trim = String.Empty) And (txtDateiname.Text = String.Empty) Then
                lblError.Text = "Bitte füllen Sie ein Suchkriterium aus."
                Exit Sub
            End If

            If Not txtDateiname.Text = String.Empty Then
                m_Report.Dateiname = txtDateiname.Text

                lblError.Text = ""
            Else
                If (txtDatVon.Text.Trim = String.Empty) Or (txtDatBis.Text.Trim = String.Empty) Then
                    lblError.Text = "Datumseingaben unvollständig."
                    Exit Sub
                End If

                Try
                    datAb = CType(txtDatVon.Text, Date)
                    datBis = CType(txtDatBis.Text, Date)
                Catch ex As Exception
                    lblError.Text = "Bitte Datumsformat (TT.MM.YYYY) beachten."
                    Exit Sub
                End Try

                If (datBis < datAb) Then
                    lblError.Text = "Endatum darf nicht vor Startdatum liegen."
                    Exit Sub
                End If

                m_Report.datAb = txtDatVon.Text
                m_Report.datBis = txtDatBis.Text

                lblError.Text = ""
            End If

            If lblError.Text.Length = 0 Then
                m_Report.Fill(Session("AppID").ToString, Session.SessionID.ToString, Me)

                If Not m_Report.Status = 0 Then
                    lblError.Text = "Fehler: " & m_Report.Message
                Else
                    If m_Report.Result.Rows.Count = 0 Then
                        lblError.Text = "Keine Daten vorhanden."
                    Else
                        tblResult.Visible = True

                        ListBox1.DataSource = m_Report.ResultTable
                        ListBox1.DataValueField = "Dateiname"
                        ListBox1.DataTextField = "Anzeige"
                        ListBox1.DataBind()

                        Session("ReportMDR") = m_Report
                    End If
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            'logApp.UpdateEntry("ERR", Session("AppID").ToString, "Fehler im Report <Fehlende Abmeldeunterlagen>. Fehler: " & ex.Message)
        End Try
    End Sub

    Private Sub btnCal1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCal1.Click
        calVon.Visible = True
        calBis.Visible = False
    End Sub

    Private Sub btnCal2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCal2.Click
        calVon.Visible = False
        calBis.Visible = True
    End Sub

    Private Sub calVon_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calVon.SelectionChanged
        txtDatVon.Text = calVon.SelectedDate.ToShortDateString
        calVon.Visible = False
    End Sub

    Private Sub calBis_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calBis.SelectionChanged
        txtDatBis.Text = calBis.SelectedDate.ToShortDateString
        calBis.Visible = False
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        Try
            Dim m_Report As MDR_02
            m_Report = CType(Session("ReportMDR"), MDR_02)

            Dim tmpDataView As New DataView()

            tmpDataView = m_Report.Result.DefaultView
            tmpDataView.RowFilter = "Dateiname = '" & ListBox1.SelectedItem.Value & "'"

            If tmpDataView.Count = 0 Then
                DataGrid1.Visible = False
                lnkCreateCSV.Enabled = False
                trCSV.Visible = False
            Else
                lnkCreateCSV.Enabled = True

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
        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListBox1.SelectedIndexChanged
        FillGrid(0)
    End Sub

    Private Sub lnkCreateCSV_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkCreateCSV.Click
        Dim sString As String
        Dim sChar As String = Chr(34)
        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & ListBox1.SelectedItem.Value
        Try
            trCSV.Visible = False

            Dim m_Report As MDR_02
            m_Report = CType(Session("ReportMDR"), MDR_02)

            Dim tmpDataView As New DataView()

            tmpDataView = m_Report.Result.DefaultView
            tmpDataView.RowFilter = "Dateiname = '" & ListBox1.SelectedItem.Value & "'"

            If tmpDataView.Count > 0 Then
                Dim tmpTable As New DataTable()
                'tmpTable.Columns.Add("KdKz", System.Type.GetType("System.String"))
                'tmpTable.Columns.Add("VIN", System.Type.GetType("System.String"))
                'tmpTable.Columns.Add("Prüfziffer", System.Type.GetType("System.String"))
                'tmpTable.Columns.Add("Kennzeichen", System.Type.GetType("System.String"))
                'tmpTable.Columns.Add("Zulassung", System.Type.GetType("System.DateTime"))

                Dim tmpLoop As Integer
                'Dim tmpRow As DataRow
                For tmpLoop = 0 To tmpDataView.Count - 1
                    'tmpRow = tmpTable.NewRow
                    'tmpRow("KdKz") = CStr(tmpDataView(0)("KdKz"))
                    'tmpRow("VIN") = CStr(tmpDataView(0)("VIN"))
                    'tmpRow("Prüfziffer") = CStr(tmpDataView(0)("Prüfziffer"))
                    'tmpRow("Kennzeichen") = CStr(tmpDataView(0)("Kennzeichen"))
                    'If Not tmpDataView(0)("Zulassung") Is Nothing AndAlso IsDate(tmpDataView(0)("Zulassung")) Then
                    '    tmpRow("Zulassung") = CDate(tmpDataView(0)("Zulassung"))
                    'End If
                    'tmpTable.Rows.Add(tmpRow)

                    sString = sChar & CStr(tmpDataView(tmpLoop)("KdKz")) & sChar & ";"
                    sString = sString & sChar & CStr(tmpDataView(tmpLoop)("VIN")) & sChar & ";"
                    sString = sString & sChar & CStr(tmpDataView(tmpLoop)("Prüfziffer")) & sChar & ";"
                    sString = sString & sChar & CStr(tmpDataView(tmpLoop)("Kennzeichen")) & sChar & ";"
                    If Not tmpDataView(0)("Zulassung") Is Nothing AndAlso IsDate(tmpDataView(tmpLoop)("Zulassung")) Then
                        sString = sString & sChar & CDate(tmpDataView(tmpLoop)("Zulassung")) & sChar
                    End If
                    'tmpTable.Rows.Add(tmpRow)


                    Dim path As String = MapPath("/Portal/Temp/Excel/") & Replace(strFileName, ".XLS", ".csv")
                    If File.Exists(path) = False Then
                        Dim sw As StreamWriter = File.CreateText(path)
                        ' Create csv
                        sw.WriteLine(sString)
                        sw.Flush()
                        sw.Close()
                    Else
                        Dim sw As StreamWriter = File.AppendText(path)
                        sw.WriteLine(sString)
                        sw.Flush()
                        sw.Close()
                    End If

                Next

                'If File.Exists("/Portal/Temp/Excel/" & Replace(strFileName, ".xls", ".csv")) Then

                '    sFile = File.OpenWrite()
                '    sFile.Writeln()
                'Else
                '    File.Create("/Portal/Temp/Excel/" & Replace(strFileName, ".xls", ".csv"))
                'End If
                'Dim excelFactory As New Base.Kernel.DocumentGeneration.ExcelDocumentFactory()
                'excelFactory.CreateDocumentAndWriteToFilesystem(ConfigurationManager.AppSettings("ExcelPath") & strFileName, tmpTable, Me.Page, , , , , False)
                lnkShowExcel.NavigateUrl = "/Portal/Temp/Excel/" & Replace(strFileName, ".XLS", ".csv")
                trCSV.Visible = True
            End If
        Catch ex As Exception
            lblError.Text = "Beim Erstellen der CSV-Datei ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub
End Class

' ************************************************
' $History: Report12.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 26.06.09   Time: 10:33
' Updated in $/CKAG/Applications/appffd/Forms
' ITA: 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 30.04.09   Time: 14:43
' Updated in $/CKAG/Applications/appffd/Forms
' ITA: 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:23
' Created in $/CKAG/Applications/appffd/Forms
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 23.10.07   Time: 17:20
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' Bugfix Report12 csv Datei erstellen
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 23.10.07   Time: 14:34
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' Bugfix Report12 csv Datei erstellen
' 
' *****************  Version 4  *****************
' User: Uha          Date: 20.08.07   Time: 16:17
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' 
' *****************  Version 3  *****************
' User: Uha          Date: 13.08.07   Time: 10:39
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' CSV-Ausgabe in MDR Report "Versendete Zulassungsdaten" inegriert
' 
' *****************  Version 2  *****************
' User: Uha          Date: 9.08.07    Time: 15:51
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' Report "Versendete Zulassungsdaten" - 1. Version ohne Excel Download
' 
' *****************  Version 1  *****************
' User: Uha          Date: 9.08.07    Time: 11:12
' Created in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' Report "Versendete Zulassungsdaten" vorbereitet
' 
' ************************************************
