Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Business
Imports System.Data.OleDb

Partial Public Class Change04
    Inherits System.Web.UI.Page
    Private m_User As Security.User
    Private m_App As Security.App
    Private mObjUpload As UploadSperr_Entsperr
    Private m_objTable As DataTable

    Protected WithEvents GridNavigation1 As CKG.Services.GridNavigation

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)
        GridNavigation1.setGridElment(GridView1)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        If m_User.Organization.OrganizationAdmin Then
            lblHead.Text = Replace(lblHead.Text, " (Beauftragung)", "")
        End If

        m_App = New Security.App(m_User)

        If (Session("App_mObjUpload") Is Nothing) OrElse (Not IsPostBack) Then
            mObjUpload = New UploadSperr_Entsperr(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
        Else
            mObjUpload = CType(Session("App_mObjUpload"), UploadSperr_Entsperr)
        End If

        'Für den Upload
        Me.Form.Enctype = "multipart/form-data"

        ' Session("AppChange") = m_change
    End Sub
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Protected Sub lbCreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbCreate.Click
        doSubmit()
    End Sub
    Private Sub doSubmit()
        Dim tmpFileTable = LoadFile()

        If Not tmpFileTable Is Nothing Then
            Dim strTemp As String = ""
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"

            mObjUpload = New UploadSperr_Entsperr(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, strFileName)
            If rdo_list.Items(0).Selected = True Then
                mObjUpload.Task = "S"
            Else
                mObjUpload.Task = "E"
            End If
            mObjUpload.generateUploadTable(tmpFileTable)

            If mObjUpload.Status = 0 Then

                mObjUpload.save(Session("AppID").ToString, Session.SessionID, Me)
                Session.Add("App_mObjUpload", mObjUpload)
                If Not mObjUpload.Erledigt Is Nothing Then
                    m_objTable = mObjUpload.Erledigt
                    Session.Add("App_mObjUpload", mObjUpload)
                    FillGrid(0)
                End If
            Else
                lblError.Text = mObjUpload.Message
                Exit Sub
            End If
        End If
    End Sub
    Private Function getDataTableFromExcel(ByVal filepath As String, ByVal filename As String) As DataTable

        Dim objDataset1 As New DataSet()
        Dim sConnectionString As String = ""
        If Right(upFile1.PostedFile.FileName.ToUpper, 4) = ".XLS" Then
            sConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" & _
                             "Data Source=" & filepath & filename & ";Extended Properties=""Excel 8.0;HDR=Yes"""

        ElseIf Right(upFile1.PostedFile.FileName.ToUpper, 5) = ".XLSX" Then
            sConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" & "Data Source=" & filepath & filename & ";Extended Properties=""Excel 12.0 Xml;HDR=Yes"""
        End If


        Dim objConn As New OleDbConnection(sConnectionString)
        objConn.Open()

        Dim schemaTable As DataTable
        Dim tmpObj() As Object = {Nothing, Nothing, Nothing, "Table"}
        schemaTable = objConn.GetOleDbSchemaTable(OleDb.OleDbSchemaGuid.Tables, tmpObj)

        For Each sheet As DataRow In schemaTable.Rows
            Dim tableName As String = sheet("Table_Name").ToString
            Dim objCmdSelect As New OleDbCommand("SELECT * FROM [" & tableName & "]", objConn)
            Dim objAdapter1 As New OleDbDataAdapter(objCmdSelect)
            objAdapter1.Fill(objDataset1, tableName)
        Next
        Dim tblTemp As DataTable = objDataset1.Tables(0)
        objConn.Close()
        Return tblTemp
    End Function
    Private Function LoadFile() As DataTable
        'Prüfe Fehlerbedingung
        If (Not upFile1.PostedFile Is Nothing) AndAlso (Not (upFile1.PostedFile.FileName = String.Empty)) Then
            lblExcelfile.Text = upFile1.PostedFile.FileName
            If Right(upFile1.PostedFile.FileName.ToUpper, 4) <> ".XLS" AndAlso Right(upFile1.PostedFile.FileName.ToUpper, 5) <> ".XLSX" Then
                lblError.Text = "Es können nur Dateien im .XLS -bzw. .XLSX Format verarbeitet werden."
                Return Nothing
                Exit Function
            End If
            If (upFile1.PostedFile.ContentLength > CType(ConfigurationManager.AppSettings("MaxUploadSize"), Integer)) Then
                lblError.Text = "Datei '" & upFile1.PostedFile.FileName & "' ist zu gross (>300 KB)."
                Return Nothing
                Exit Function
            End If
        Else
            lblError.Text = "Keine Datei ausgewählt"
            Return Nothing
            Exit Function
        End If
        'Lade Datei
        Return getFileData(upFile1.PostedFile)
    End Function
    Private Function getFileData(ByVal uFile As System.Web.HttpPostedFile) As DataTable
        Dim tmpTable As New DataTable
        Try
            Dim filepath As String = ConfigurationManager.AppSettings("ExcelPath")
            Dim filename As String = ""
            Dim info As System.IO.FileInfo

            If Right(upFile1.PostedFile.FileName.ToUpper, 4) = ".XLS" Then
                filename = m_User.UserName & "_" & Format(Now, "yyyyMMddhhmmss") & ".xls"

            ElseIf Right(upFile1.PostedFile.FileName.ToUpper, 5) = ".XLSX" Then
                filename = m_User.UserName & "_" & Format(Now, "yyyyMMddhhmmss") & ".xlsx"

            End If

            If Not (uFile Is Nothing) Then
                uFile.SaveAs(ConfigurationManager.AppSettings("ExcelPath") & filename)
                uFile = Nothing
                info = New System.IO.FileInfo(filepath & filename)
                If Not (info.Exists) Then
                    tmpTable = Nothing
                    Throw New Exception("Fehler beim Speichern.")
                End If
                'Datei gespeichert -> Auswertung
                tmpTable = getDataTableFromExcel(filepath, filename)
            End If
        Catch ex As Exception
            lblError.Text = ex.Message
        Finally
            getFileData = tmpTable
        End Try
    End Function
    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        Try


            If m_objTable.Rows.Count = 0 Then
                GridView1.Visible = False
                Result.Visible = False
                'lblNoData.Visible = True
                'lblNoData.Text = "Keine Daten zur Anzeige gefunden."
            Else
                Result.Visible = True
                GridView1.Visible = True
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

                GridView1.DataSource = tmpDataView
                GridView1.DataBind()

                If GridView1.PageCount > 1 Then
                    GridView1.PagerStyle.CssClass = "PagerStyle"
                    GridView1.PageIndex = intTempPageIndex
                    GridView1.DataBind()
                End If
                If mObjUpload.Task = "E" Then
                    GridView1.Columns(1).Visible = False
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Daten konnten nicht geladen werden!"
        End Try
    End Sub
    Private Sub CreateExcel()
        Dim tblExcel As DataTable
        Dim Newrow As DataRow
        Dim Row As DataRow
        If Not (Session("App_mObjUpload") Is Nothing) Then

            mObjUpload = CType(Session("App_mObjUpload"), UploadSperr_Entsperr)
            tblExcel = New DataTable

            With tblExcel
                .Columns.Add("Fahrgestellnummer", System.Type.GetType("System.String"))
                .Columns.Add("Status", System.Type.GetType("System.String"))
            End With
            For Each Row In mObjUpload.Erledigt.Rows
                Newrow = tblExcel.NewRow
                Newrow("Fahrgestellnummer") = Row("CHASSIS_NUM")
                Newrow("Status") = Row("BEM_RETURN")
                tblExcel.Rows.Add(Newrow)
            Next

            Try
                Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
                Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName

                excelFactory.CreateDocumentAndSendAsResponse(strFileName, tblExcel, Me.Page)

            Catch ex As Exception
                lblError.Text = "Fehler beim Erstellen der Excel-Datei: " + ex.Message
            End Try
        End If
    End Sub

    Protected Sub lnkCreateExcel1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel1.Click
        CreateExcel()
    End Sub

    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        Response.Redirect("/Services/(S(" & Session.SessionID & "))/Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub

    Private Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging

        If Not Session("App_mObjUpload") Is Nothing Then
            mObjUpload = CType(Session("App_mObjUpload"), UploadSperr_Entsperr)
            m_objTable = mObjUpload.tbUpload
            FillGrid(e.NewPageIndex)
        Else
            lblError.Text = "Benötigtes Session Objekt nicht vorhanden!"
        End If



    End Sub

    Private Sub GridView1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
        FillGrid(gridview1.PageIndex,e.SortExpression)
    End Sub
End Class