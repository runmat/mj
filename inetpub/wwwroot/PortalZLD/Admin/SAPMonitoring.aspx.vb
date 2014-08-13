
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common
Imports CKG.PortalZLD.PageElements
Imports CKG.Base.Kernel
Imports CKG


Public Class SAPMonitoring
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

    Private m_blnShowDetails() As Boolean
    Private m_objTrace As Base.Kernel.Logging.Trace
    Protected WithEvents GridNavigation1 As Global.CKG.PortalZLD.GridNavigation

#Region "Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        AdminAuth(Me, m_User, AdminLevel.Organization)

        GetAppIDFromQueryString(Me)
        GridNavigation1.setGridElment(DataGrid1)

        Try
            m_App = New App(m_User)

            If Not IsPostBack Then
                FillForm()

                txtAbDatum_CalendarExtender.SelectedDate = Today

                txtBisDatum_CalendarExtender.SelectedDate = Today
            Else
                If Not m_context.Cache("m_objTrace") Is Nothing Then
                    m_objTrace = CType(m_context.Cache("m_objTrace"), Base.Kernel.Logging.Trace)
                End If
            End If

            ReDim m_blnShowDetails(DataGrid1.PageSize)
            Dim i As Int32
            For i = 0 To DataGrid1.PageSize - 1
                m_blnShowDetails(i) = False
            Next
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "SAPMonitoring", "Page_Load", ex.ToString)

            lblError.Text = ex.ToString
            lblError.Visible = True
        End Try
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbCreate.Click
        lblError.Text = ""
        trdata1.Visible = False
        trdata2.Visible = False
        Resultshow.Visible = True

        If Not IsDate(txtAbDatum.Text) Then
            If Not IsStandardDate(txtAbDatum.Text) Then
                If Not IsSAPDate(txtAbDatum.Text) Then
                    lblError.Text = "Geben Sie bitte ein gültiges Datum ein!<br>"
                End If
            End If
        End If
        If Not IsDate(txtBisDatum.Text) Then
            If Not IsStandardDate(txtBisDatum.Text) Then
                If Not IsSAPDate(txtBisDatum.Text) Then
                    lblError.Text = "Geben Sie bitte ein gültiges Datum ein!<br>"
                End If
            End If
        End If

        'Wenn der Datentyp nicht stimmt, sollte hier ausgestiegen werden
        If lblError.Text <> String.Empty Then Exit Sub


        Dim datAb As Date = CDate(txtAbDatum.Text)
        Dim datBis As Date = CDate(txtBisDatum.Text)
        If datAb > datBis Then
            lblError.Text = "Das Enddatum darf nicht vor dem Anfangsdatum liegen!<br>"
        End If
        If lblError.Text.Length > 0 Then
            Exit Sub
        End If

        If rbBAPI.Checked Then
            Me.ViewState("mySort") = ""
            FillDataGrid1(True, 0)
            trdata1.Visible = True
        Else
            Me.ViewState("mySort") = "[ASPX_ID]"
            FillDataGrid2(True, 0)
            trdata2.Visible = True
        End If

        'If Not Session("SAPMonitorResultExcel") Is Nothing Then
        '    Dim tblResult As DataTable
        '    tblResult = CType(Session("SAPMonitorResultExcel"), DataTable)
        '    If Not tblResult.Rows.Count = 0 Then
        '        Dim objExcelExport As New Base.Kernel.Excel.ExcelExport()
        '        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
        '        Try
        '            Base.Kernel.Excel.ExcelExport.WriteExcel(tblResult, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
        '        Catch
        '        End Try
        '        'lblDownloadTip.Visible = True
        '        'lnkExcel.Visible = True
        '        'lnkExcel.NavigateUrl = "/Services/Temp/Excel/" & strFileName
        '    End If
        'ElseIf Not Session("SAPMonitorResult") Is Nothing Then
        '    Dim tblResult As DataTable
        '    tblResult = CType(Session("SAPMonitorResult"), DataTable)
        '    If Not tblResult.Rows.Count = 0 Then
        '        Dim objExcelExport As New Base.Kernel.Excel.ExcelExport()
        '        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
        '        Try
        '            Base.Kernel.Excel.ExcelExport.WriteExcel(tblResult, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
        '        Catch
        '        End Try
        '        'lblDownloadTip.Visible = True
        '        'lnkExcel.Visible = True
        '        'lnkExcel.NavigateUrl = "/Services/Temp/Excel/" & strFileName
        '    End If
        'Else
        '    'lblDownloadTip.Visible = False
        '    'lnkExcel.Visible = False
        '    'lnkExcel.NavigateUrl = ""
        'End If
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillDataGrid1(False, DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        FillDataGrid1(False, e.NewPageIndex)
    End Sub

    Private Sub rbBAPI_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbBAPI.CheckedChanged
        If rbBAPI.Checked Then
            FillForm()
        End If
    End Sub

    Private Sub rbASPX_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbASPX.CheckedChanged
        If rbASPX.Checked Then
            FillForm()
        End If
    End Sub

    Private Sub HGZ_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles HGZ.SortCommand
        FillDataGrid2(False, HGZ.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub HGZ_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles HGZ.PageIndexChanged
        FillDataGrid2(False, e.NewPageIndex)
    End Sub

    Private Sub HGZ_TemplateSelection(ByVal sender As Object, ByVal e As DBauer.Web.UI.WebControls.HierarGridTemplateSelectionEventArgs) Handles HGZ.TemplateSelection
        Select Case (e.Row.Table.TableName)
            Case "BAPI"
                e.TemplateFilename = "Templates\\BAPIData.ascx"
            Case Else
                Throw New NotImplementedException("Unexpected child row in TemplateSelection event")
        End Select
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        If Me.trdata2.Visible Then
            Dim item As DataGridItem
            Dim cell As TableCell
            Dim control As Control
            Dim image As System.Web.UI.WebControls.Image
            For Each item In HGZ.Items
                For Each cell In item.Cells
                    For Each control In cell.Controls
                        If TypeOf control Is System.Web.UI.WebControls.Image Then
                            image = CType(control, System.Web.UI.WebControls.Image)
                            image.Width = 16
                            image.Height = 16
                            If InStr(image.ImageUrl, "plus.gif") > 0 Then
                                image.ImageUrl = "/PortalZLD/Images/plus.gif"
                            End If
                            If InStr(image.ImageUrl, "minus.gif") > 0 Then
                                image.ImageUrl = "/PortalZLD/Images/minus.gif"
                            End If
                        End If
                    Next
                Next
            Next
        End If
    End Sub

    Protected Sub lnkCreateExcel1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel1.Click
        Dim reportExcel As New DataTable()
        If TypeOf Session("SAPMonitorResult") Is DataTable Then
            reportExcel = CType(Session("SAPMonitorResult"), DataTable)
            If reportExcel.Rows.Count > 0 Then

                Try
                    Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
                    Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
                    excelFactory.CreateDocumentAndSendAsResponse(strFileName, reportExcel, Me)
                Catch ex As Exception
                    lblError.Text = ex.Message
                End Try

            End If
        End If
    End Sub
#End Region

#Region "Methods"
    Private Sub FillDataGrid2(ByVal blnForceNew As Boolean, ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        Dim dsLogData As New DataSet()
        Dim dt As New DataTable()
        Dim blnSortNew As Boolean = False

        If strSort.Length > 0 Then
            strSort = "[" & strSort & "]"
            blnSortNew = True
            If CStr(Me.ViewState("mySort")) = strSort Then
                strSort &= " DESC"
            End If
        Else
            strSort = CStr(Me.ViewState("mySort"))
        End If
        Me.ViewState("mySort") = strSort

        If Not blnForceNew AndAlso Not blnSortNew AndAlso (Not Session("SAPMonitorResult") Is Nothing) Then
            dsLogData = CType(Session("SAPMonitorResult"), DataSet)
        Else
            If Not IsDate(Me.txtAbDatum.Text) Then
                Me.lblError.Text = "Bitte Datum/Startdatum übergeben."
                Exit Sub
            ElseIf Not IsDate(Me.txtBisDatum.Text) Then
                Me.lblError.Text = "Bitte Datum/Enddatum übergeben."
                Exit Sub
            Else
                Dim datTemp As Date
                datTemp = DateAdd(DateInterval.Day, 1, CDate(Me.txtBisDatum.Text))

                Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
                cn.Open()

                'Ermittele Daten für die Excel-Datei
                Dim da As New SqlClient.SqlDataAdapter("SELECT TOP 100 PERCENT " & _
                                                        "     LogAccessASPXID AS ASPX_ID, " & _
                                                        "     Seite, " & _
                                                        "     AppFriendlyName AS Anwendung, " & _
                                                        "     StartZeit AS [Start ASPX], " & _
                                                        "     EndZeit AS [Ende ASPX], " & _
                                                        "     Dauer AS [Dauer ASPX], " & _
                                                        "     Username AS Benutzer, " & _
                                                        "     Kunnr AS [Kundennr.], " & _
                                                        "     Customername As Kundenname, " & _
                                                        "     AccountingArea, " & _
                                                        "     IsTestUser AS Testbenutzer, " & _
                                                        "     LogAccessSAPID AS SAP_ID, " & _
                                                        "     BAPI, " & _
                                                        "     Description AS Beschreibung, " & _
                                                        "     StartTime AS [Start SAP], " & _
                                                        "     EndTime AS [Ende SAP], " & _
                                                        "     Duration AS [Dauer SAP], " & _
                                                        "     Sucess AS Erfolg, " & _
                                                        "     ErrorMessage AS Fehlermeldung " & _
                                                        "FROM dbo.vwASPXBAPI " & _
                                                        "WHERE " & _
                                                        "     (Seite LIKE @Seite) " & _
                                                        "     AND (StartZeit BETWEEN @von AND @bis) " & _
                                                        "ORDER BY LogAccessASPXID,LogAccessSAPID", _
                                                        cn)
                Dim strASPX As String = Me.ddlAuswahl.SelectedItem.Value
                If strASPX = String.Empty Then strASPX = "%"
                With da.SelectCommand.Parameters
                    .AddWithValue("@Seite", strASPX)
                    .AddWithValue("@von", CDate(Me.txtAbDatum.Text))
                    .AddWithValue("@bis", datTemp)
                End With
                da.Fill(dt)
                
                Dim tmpRow As DataRow
                Dim tmpASPX_ID As String = "XXXX"
                For Each tmpRow In dt.Rows
                    If TypeOf tmpRow("Dauer ASPX") Is System.DBNull Then
                        tmpRow("Dauer ASPX") = 0
                    End If
                    If TypeOf tmpRow("Dauer SAP") Is System.DBNull Then
                        tmpRow("Dauer SAP") = 0
                    End If
                    If tmpASPX_ID = CStr(tmpRow("ASPX_ID")) Then
                        tmpRow("Dauer ASPX") = System.DBNull.Value
                    End If
                    tmpASPX_ID = CStr(tmpRow("ASPX_ID"))
                Next
                'm_context.Cache.Insert("SAPMonitorResultExcel", dt, New System.Web.Caching.CacheDependency(Server.MapPath("SAPMonitoring.aspx")), DateTime.Now.AddMinutes(20), TimeSpan.Zero)
                Session("SAPMonitorResultExcel") = dt
                'Ermittele Daten für die Darstellung
                dt = New DataTable("ASPX")
                da = New SqlClient.SqlDataAdapter("SELECT TOP 100 PERCENT " & _
                                                        "     ASPX_ID, " & _
                                                        "     Seite, " & _
                                                        "     Anwendung, " & _
                                                        "     [Start ASPX], " & _
                                                        "     [Ende ASPX], " & _
                                                        "     [Dauer ASPX], " & _
                                                        "     Benutzer, " & _
                                                        "     Kunnr, " & _
                                                        "     Customername, " & _
                                                        "     AccountingArea, " & _
                                                        "     Testbenutzer, " & _
                                                        "     [Dauer SAP], " & _
                                                        "     [Zugriffe SAP] " & _
                                                        "FROM dbo.vwASPXBAPI_SUM " & _
                                                        "WHERE " & _
                                                        "     (Seite LIKE @Seite) " & _
                                                        "     AND ([Start ASPX] BETWEEN @von AND @bis) " & _
                                                        "ORDER BY " & strSort, _
                                                        cn)
                With da.SelectCommand.Parameters
                    .AddWithValue("@Seite", strASPX)
                    .AddWithValue("@von", CDate(Me.txtAbDatum.Text))
                    .AddWithValue("@bis", datTemp)
                End With
                da.Fill(dt)

                For Each tmpRow In dt.Rows
                    If TypeOf tmpRow("Dauer ASPX") Is System.DBNull Then
                        tmpRow("Dauer ASPX") = 0
                    End If
                    If TypeOf tmpRow("Dauer SAP") Is System.DBNull Then
                        tmpRow("Dauer SAP") = 0
                    End If
                Next
                dsLogData.Tables.Add(dt)

                Dim dt2 As New DataTable("BAPI")
                da = New SqlClient.SqlDataAdapter("SELECT TOP 100 PERCENT " & _
                                                        "     SAP_ID, " & _
                                                        "     ASPX_ID, " & _
                                                        "     BAPI, " & _
                                                        "     Beschreibung, " & _
                                                        "     [Start SAP], " & _
                                                        "     [Ende SAP], " & _
                                                        "     [Dauer SAP], " & _
                                                        "     Erfolg, " & _
                                                        "     Fehlermeldung " & _
                                                        "FROM dbo.vwASPXBAPI_DETAIL " & _
                                                        "WHERE " & _
                                                        "      ([Start SAP] BETWEEN @von AND @bis) ", _
                                                        cn)
                With da.SelectCommand.Parameters
                    .AddWithValue("@von", CDate(Me.txtAbDatum.Text))
                    .AddWithValue("@bis", datTemp)
                End With

                'Abfrage braucht sehr lange
                da.SelectCommand.CommandTimeout = 300

                da.Fill(dt2)
                dsLogData.Tables.Add(dt2)

                Dim dc1 As DataColumn
                Dim dc2 As DataColumn
                'Relation ASPX => BAPI
                dc1 = dsLogData.Tables("ASPX").Columns("ASPX_ID")
                dc2 = dsLogData.Tables("BAPI").Columns("ASPX_ID")
                Dim dr As DataRelation = New DataRelation("ASPX_BAPI", dc1, dc2, False)
                dsLogData.Relations.Add(dr)

                'm_context.Cache.Insert("SAPMonitorResult", dsLogData, New System.Web.Caching.CacheDependency(Server.MapPath("SAPMonitoring.aspx")), DateTime.Now.AddMinutes(20), TimeSpan.Zero)
                Session("SAPMonitorResult") = dsLogData
            End If
        End If

        With Me.HGZ
            .CurrentPageIndex = intPageIndex
            .DataSource = dsLogData
            .DataMember = "ASPX"

            '.DataSource = dsLogData.Tables("ASPX")
            .DataBind()
            '.Visible = True
        End With
        Me.trdata2.Visible = True
    End Sub

    Private Sub FillDataGrid1(ByVal blnForceNew As Boolean, ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        Dim dt As New DataTable()
        'If Not blnForceNew AndAlso (Not m_context.Cache("SAPMonitorResult") Is Nothing) Then
        '    dt = CType(m_context.Cache("SAPMonitorResult"), DataTable)
        If Not blnForceNew AndAlso (Not Session("SAPMonitorResult") Is Nothing) Then
            dt = CType(Session("SAPMonitorResult"), DataTable)
        Else
            If Not IsDate(Me.txtAbDatum.Text) Then
                Me.lblError.Text = "Bitte Datum/Startdatum übergeben."
                Exit Sub
            ElseIf Not IsDate(Me.txtBisDatum.Text) Then
                Me.lblError.Text = "Bitte Datum/Enddatum übergeben."
                Exit Sub
            Else
                Dim strTemp As String
                strTemp = CStr(DateAdd(DateInterval.Day, 1, CDate(Me.txtBisDatum.Text)))

                Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)

                cn.Open()
                Dim da As New SqlClient.SqlDataAdapter("SELECT LogStandard.UserName AS Benutzer, IsTestUser AS Testbenutzer, BAPI,Description, StartTime AS Start, EndTime AS Ende, DATEDIFF(second, StartTime, EndTime) As Dauer, Sucess AS Erfolg, ErrorMessage AS Fehlermeldung FROM LogAccessSAP inner join LogStandard ON LogAccessSAP.StandardLogID = LogStandard.ID WHERE BAPI LIKE @BAPI AND StartTime BETWEEN CONVERT ( Datetime , '" & Me.txtAbDatum.Text & "' , 104 ) AND CONVERT ( Datetime , '" & strTemp & "' , 104 ) ORDER BY BAPI", cn)
                Dim strBAPI As String = Me.ddlAuswahl.SelectedItem.Value
                If strBAPI = String.Empty Then strBAPI = "%"
                da.SelectCommand.CommandTimeout = 120

                With da.SelectCommand.Parameters
                    .AddWithValue("@BAPI", strBAPI)
                End With
                da.Fill(dt)
                Dim tmpRow As DataRow
                For Each tmpRow In dt.Rows
                    If TypeOf tmpRow("Dauer") Is System.DBNull Then
                        tmpRow("Dauer") = 0
                    End If
                Next
                'm_context.Cache.Insert("SAPMonitorResult", dt, New System.Web.Caching.CacheDependency(Server.MapPath("SAPMonitoring.aspx")), DateTime.Now.AddMinutes(20), TimeSpan.Zero)
                Session("SAPMonitorResult") = dt
            End If
        End If

        If strSort.Length > 0 Then
            If CStr(Me.ViewState("mySort")) = strSort Then
                strSort &= " DESC"
            End If
        Else
            strSort = CStr(Me.ViewState("mySort"))
        End If
        Me.ViewState("mySort") = strSort

        dt.DefaultView.Sort = strSort
        With Me.DataGrid1
            .CurrentPageIndex = intPageIndex
            .DataSource = dt
            .DataBind()
            '.Visible = True
        End With

        GridNavigation1.setGridElment(DataGrid1)
        Me.trdata1.Visible = True
    End Sub

    Private Sub FillForm()
        lblError.Text = ""
        trdata1.Visible = False
        trdata2.Visible = False
        Resultshow.Visible = False

        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()
        If rbBAPI.Checked Then
            FillBAPI(cn)
            ExcelDiv.Visible = True
        Else
            FillASPX(cn)
            ExcelDiv.Visible = False
        End If
    End Sub

    Private Sub FillASPX(ByVal cn As SqlClient.SqlConnection)
        Dim dt As New DataTable()
        Dim da As New SqlClient.SqlDataAdapter("SELECT DISTINCT Seite, Seite + ' - ' + AppFriendlyName AS Anzeige FROM vwASPXBAPI ORDER BY Seite", cn)
        da.Fill(dt)
        Dim dr As DataRow = dt.NewRow
        dr("Seite") = ""
        dr("Anzeige") = "- alle -"
        dt.Rows.Add(dr)

        dt.DefaultView.Sort = "Seite"
        With ddlAuswahl
            .DataTextField = "Anzeige"
            .DataValueField = "Seite"
            .DataSource = dt.DefaultView
            .DataBind()
        End With
    End Sub

    Private Sub FillBAPI(ByVal cn As SqlClient.SqlConnection)
        Dim dt As New DataTable()
        Dim da As New SqlClient.SqlDataAdapter("SELECT DISTINCT BAPI, BAPI AS Anzeige FROM LogAccessSAP ORDER BY BAPI", cn)
        da.Fill(dt)
        Dim dr As DataRow = dt.NewRow
        dr("BAPI") = ""
        dr("Anzeige") = "- alle -"
        dt.Rows.Add(dr)

        dt.DefaultView.Sort = "BAPI"
        With ddlAuswahl
            .DataTextField = "Anzeige"
            .DataValueField = "BAPI"
            .DataSource = dt.DefaultView
            .DataBind()
        End With
    End Sub

#End Region
End Class
