Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel
Imports CKG

Public Class jve_ZugriffsHistorie
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

    Private m_User As User
    Private m_App As App

    Private m_blnShowDetails() As Boolean

#Region "Events"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)

        AdminAuth(Me, m_User, AdminLevel.Organization)

        GetAppIDFromQueryString(Me)
        Dim cn As SqlClient.SqlConnection

        GridNavigation1.setGridElment(HGZ)
        Try
            m_App = New App(m_User)

            If Not IsPostBack Then
                lblError.Text = ""

                With ddbZeit.Items
                    .Add("1")
                    .Add("2")
                    .Add("3")
                    .Add("4")
                    .Add("5")
                    .Add("6")
                End With
                'calAbDatum.SelectedDate = Today
                'txtAbDatum.Text = cal.SelectedDate.ToShortDateString
                'calBisDatum.SelectedDate = Today
                'txtBisDatum.Text = calBisDatum.SelectedDate.ToShortDateString
                'TblLog.Visible = False

                cn = New SqlClient.SqlConnection(m_User.App.Connectionstring)
                cn.Open()

                Dim cmdGetUser As New SqlClient.SqlCommand("SELECT DISTINCT Customername FROM dbo.Customer WHERE NOT (Customername like '') ORDER BY Customername", cn)
                Dim dt As New DataTable()
                Dim da As New SqlClient.SqlDataAdapter()
                da.SelectCommand = cmdGetUser
                da.Fill(dt)

                dt.Columns.Add("ID", System.Type.GetType("System.Int32"))
                Dim intTemp As Int32 = 1
                Dim rowTemp As DataRow
                For Each rowTemp In dt.Rows
                    rowTemp("ID") = intTemp
                    intTemp += 1
                Next
                rowTemp = dt.NewRow
                rowTemp("ID") = 0
                rowTemp("Customername") = "- alle -"
                dt.Rows.Add(rowTemp)
                Dim dv As DataView = dt.DefaultView
                dv.Sort = "ID"

                ddlCustomer.DataSource = dv
                ddlCustomer.DataTextField = "Customername"
                ddlCustomer.DataValueField = "Customername"
                ddlCustomer.DataBind()
                ddlCustomer.SelectedIndex = 0

                cn.Close()

            End If
            ReDim m_blnShowDetails(HGZ.PageSize)
            'ReDim m_blnShowDetails(DataGrid1.PageSize)
            Dim i As Int32
            For i = 0 To HGZ.PageSize - 1
                m_blnShowDetails(i) = False
            Next
            'For i = 0 To DataGrid1.PageSize - 1
            '    m_blnShowDetails(i) = False
            'Next
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "jve_ZugriffsHistorieAbfrage", "Page_Load", ex.ToString)

            lblError.Text = ex.ToString
            lblError.Visible = True

        End Try
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        FillDataGrid(False, 0, e.SortExpression)
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        FillDataGrid(False, e.NewPageIndex)
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbCreate.Click
        'Dieser Code wird abgearbeitet, wenn der Link "Als Excel downloaden" ausgeführt wird
        lblError.Text = ""
        Session("dsLogData") = Nothing
        If Not IsDate(txtAbDatum.Text) Then
            If Not IsStandardDate(txtAbDatum.Text) Then
                If Not IsSAPDate(txtAbDatum.Text) Then
                    lblError.Text = "Ab Datum enthält kein gültiges Datum. Geben Sie bitte ein gültiges Datum ein."
                    lblError.Visible = True
                End If
            End If
        End If
        If Not IsDate(txtBisDatum.Text) Then
            If Not IsStandardDate(txtBisDatum.Text) Then
                If Not IsSAPDate(txtBisDatum.Text) Then
                    lblError.Text = "Bis Datum enthält kein gültiges Datum. Geben Sie bitte ein gültiges Datum ein."
                    lblError.Visible = True
                End If
            End If
        End If
        If lblError.Text.Length > 0 Then
            'Bei Fehler Prozedur beenden
            Exit Sub
        Else
            If System.DateTime.Compare(CDate(txtAbDatum.Text), CDate(txtBisDatum.Text)) > 0 Then
                lblError.Text = "Das Startdatum muss vor dem Enddatum liegen.<br>"
                Exit Sub
            End If
        End If

        FillDataGrid(True, 0, "startTime")

        If HGZ.Visible Then
            Dim dsResult As DataSet
            Dim tblResult As DataTable
            dsResult = CType(HGZ.DataSource, DataSet)
            tblResult = dsResult.Tables("Benutzer").Copy

            ' Daten für Excelexport zwischenspeichern
            Session("ExcelExport") = tblResult

            'Dim tblResult As DataTable
            'tblResult = CType(DataGrid1.DataSource, DataTable)

            With tblResult
                .Columns.Remove("id")
                .Columns.Remove("idSession")
                .Columns.Remove("StartColor")
                'Spalten umbenennen, damit sie in der Exceldatei vernünftige Namen haben
                .Columns(0).ColumnName = "Benutzer"
                .Columns(1).ColumnName = "Kunde"
                .Columns(2).ColumnName = "Abmeldestatus"
                .Columns(3).ColumnName = "Test"
                .Columns(4).ColumnName = "Anfrageart"
                .Columns(5).ColumnName = "Browser"
                .Columns(6).ColumnName = "Startzeit"
                .Columns(7).ColumnName = "Endzeit"
            End With

            If Not tblResult.Rows.Count = 0 Then
                Dim objExcelExport As New Base.Kernel.Excel.ExcelExport()
                Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
                Try
                    Base.Kernel.Excel.ExcelExport.WriteExcel(tblResult, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                Catch
                End Try
                'lblDownloadTip.Visible = True
                'lnkExcel.Visible = True
                'lnkExcel.NavigateUrl = "/Portal/Temp/Excel/" & strFileName
            End If
        End If
        Result.Visible = True
    End Sub

    Protected Sub lnkCreateExcel1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel1.Click
        Dim reportExcel As New DataTable()
        If TypeOf Session("ExcelExport") Is DataTable Then
            reportExcel = CType(Session("ExcelExport"), DataTable)
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

    Private Sub FillDataGrid(ByVal blnForceNew As Boolean, Optional ByVal intPageIndex As Int32 = 0, Optional ByVal strSort As String = "")
        Dim dsLogData As DataSet
        Dim dtUser As DataTable
        Dim dtAppDaten As DataTable
        Dim dtSAPDaten As DataTable
        Dim sql As String
        Dim sql1 As String
        Dim sql2 As String

        If Session("dsLogData") Is Nothing Then
            Try
                Dim strCustomerRestriction As String = ""
                If Not ddlCustomer.SelectedItem.Text = "- alle -" Then
                    strCustomerRestriction = " AND Customername = '" & ddlCustomer.SelectedItem.Text & "'"
                End If

                Dim strUserRestriction As String = ""
                If (Not Replace(txtFilterUserName.Text, "*", "").Trim(" "c).Length = 0) Then
                    strUserRestriction = " AND userName like '" & Replace(txtFilterUserName.Text, "*", "%") & "'"
                End If

                If rbOnline.Checked Then
                    sql = "SELECT * FROM vwLogWebAccess WHERE endTime is Null" & strCustomerRestriction & strUserRestriction & " ORDER BY startTime DESC"
                    sql1 = "SELECT * FROM vwLogStandardData ORDER BY Inserted DESC"
                    sql2 = "SELECT * FROM vwLogStandardDataSAP WHERE endTime is Null ORDER BY startTime DESC"
                Else
                    Dim strBisDatum As String = CStr(CDate(txtBisDatum.Text).AddDays(1).ToShortDateString)
                    If rbError.Checked Then
                        sql = "SELECT * FROM vwLogError WHERE (startTime BETWEEN '" & txtAbDatum.Text & "' AND '" & strBisDatum & "')" & strCustomerRestriction & strUserRestriction & " ORDER BY startTime DESC"
                        sql1 = "SELECT * FROM vwLogStandardError WHERE (Inserted BETWEEN '" & txtAbDatum.Text & "' AND '" & strBisDatum & "') ORDER BY Inserted DESC"
                        sql2 = "SELECT * FROM vwLogStandardDataSAP WHERE (startTime BETWEEN '" & txtAbDatum.Text & "' AND '" & strBisDatum & "') AND (Sucess = 0) ORDER BY startTime DESC"
                    Else
                        sql = "SELECT * FROM vwLogWebAccess WHERE (startTime BETWEEN '" & txtAbDatum.Text & "' AND '" & strBisDatum & "')" & strCustomerRestriction & strUserRestriction & " ORDER BY startTime DESC"
                        sql1 = "SELECT * FROM vwLogStandardData WHERE (Inserted BETWEEN '" & txtAbDatum.Text & "' AND '" & strBisDatum & "') ORDER BY Inserted DESC"
                        sql2 = "SELECT * FROM vwLogStandardDataSAP WHERE (startTime BETWEEN '" & txtAbDatum.Text & "' AND '" & strBisDatum & "') ORDER BY startTime DESC"
                    End If
                End If
                Dim conn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
                conn.Open()

                Dim da As SqlClient.SqlDataAdapter

                dsLogData = New DataSet()

                da = New SqlClient.SqlDataAdapter(sql, conn)
                dtUser = New DataTable("Benutzer")
                da.Fill(dtUser)
                dsLogData.Tables.Add(dtUser)

                da = New SqlClient.SqlDataAdapter(sql1, conn)
                dtAppDaten = New DataTable("AppDaten")
                da.Fill(dtAppDaten)
                dsLogData.Tables.Add(dtAppDaten)

                da = New SqlClient.SqlDataAdapter(sql2, conn)
                dtSAPDaten = New DataTable("SAPDaten")
                da.Fill(dtSAPDaten)
                dsLogData.Tables.Add(dtSAPDaten)

                Dim dc1 As DataColumn
                Dim dc2 As DataColumn
                'Relation Benutzer => AppDaten
                dc1 = dsLogData.Tables("Benutzer").Columns("idSession")
                dc2 = dsLogData.Tables("AppDaten").Columns("idSession")
                Dim dr As DataRelation = New DataRelation("Benutzer_AppDaten", dc1, dc2, False)
                dsLogData.Relations.Add(dr)

                'Relation AppDaten => SAPDaten
                dc1 = dsLogData.Tables("AppDaten").Columns("StandardLogID")
                dc2 = dsLogData.Tables("SAPDaten").Columns("StandardLogID")
                dr = New DataRelation("AppDaten_SAPDaten", dc1, dc2, False)
                dsLogData.Relations.Add(dr)

                conn.Close()
                conn.Dispose()
                da.Dispose()

                Session("dsLogData") = dsLogData

                '#ALT
                'dt = DBManager.Execute.Query(sql)
                dtUser.Columns.Add("StartColor", System.Drawing.Color.Red.GetType)

                Dim rowTemp As DataRow
                dtUser.AcceptChanges()
                For Each rowTemp In dtUser.Rows
                    If (CDate(rowTemp("startTime")).AddHours(CType(ddbZeit.SelectedItem.Value, Integer)) < Now) And (TypeOf rowTemp("endTime") Is DBNull) Then
                        rowTemp("StartColor") = System.Drawing.Color.Red
                    Else
                        rowTemp("StartColor") = System.Drawing.Color.Black
                    End If
                Next
                dtUser.AcceptChanges()
            Catch ex As Exception
                m_App.WriteErrorText(1, m_User.UserName, "jve_LogMessage2", "checkData", ex.ToString)
                lblError.Text = "Fehler beim Lesen aus der Datenbank.<br>(" & ex.Message & ")"
                Exit Sub
            End Try
        Else
            dsLogData = CType(Session("dsLogData"), DataSet)
            dtUser = dsLogData.Tables("Benutzer")
            dtAppDaten = dsLogData.Tables("AppDaten")
            dtSAPDaten = dsLogData.Tables("SAPDaten")
        End If

        lblError.Text = "Datenanzeige (keine Datensätze gefunden)"
        lblinfo.Text = ""
        HGZ.Visible = False
        'DataGrid1.Visible = False

        If (dtUser.Rows.Count > 0) Then
            lblError.Text = ""
            lblinfo.Text = "Datenanzeige: " & dtUser.Rows.Count & " Datensätze gefunden"
            HGZ.Visible = True
            'DataGrid1.Visible = True
            If strSort.Length > 0 Then
                If CStr(ViewState("mySort")) = strSort Then
                    strSort &= " DESC"
                End If
            Else
                strSort = CStr(ViewState("mySort"))
            End If

            ViewState("mySort") = strSort

            dtUser.DefaultView.Sort = strSort
            With HGZ
                .CurrentPageIndex = intPageIndex
                .DataSource = dsLogData
                .DataMember = "Benutzer"
                .DataBind()
                .Visible = True
                If .PageCount > 1 Then
                    .PagerStyle.Visible = True
                Else
                    .PagerStyle.Visible = False
                End If

                Dim intItemCount As Int32 = HGZ.Items.Count
                Dim intI As Int32
                For intI = 0 To intItemCount - 1
                    HGZ.RowExpanded(intI) = False
                Next
            End With
            'With DataGrid1
            '    .CurrentPageIndex = intPageIndex
            '    .DataSource = dt
            '    .DataBind()
            '    .Visible = True
            'End With
            'TblLog.Visible = True
        End If
    End Sub

#End Region

    Private Sub HGZ_TemplateSelection(ByVal sender As Object, ByVal e As DBauer.Web.UI.WebControls.HierarGridTemplateSelectionEventArgs) Handles HGZ.TemplateSelection
        Select Case (e.Row.Table.TableName)
            Case "AppDaten"
                e.TemplateFilename = "Templates\\AppData.ascx"
            Case Else
                Throw New NotImplementedException("Unexpected child row in TemplateSelection event")
        End Select
    End Sub

    Private Sub HGZ_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles HGZ.SortCommand
        FillDataGrid(False, 0, e.SortExpression)
    End Sub

    Private Sub HGZ_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles HGZ.PageIndexChanged
        FillDataGrid(False, e.NewPageIndex)
    End Sub

End Class
