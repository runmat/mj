Option Explicit On
Option Strict On
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports System.Data.OleDb

Partial Public Class Change07
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Protected WithEvents ucStyles As Global.CKG.Portal.PageElements.Styles
    Protected WithEvents ucHeader As Global.CKG.Portal.PageElements.Header

    Private m_context As HttpContext = HttpContext.Current
    Private mObjUpload As UploadSperr_Entsperr
    Private m_objTable As DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            m_User = GetUser(Me)
            m_App = New Base.Kernel.Security.App(m_User) 'erzeugt ein App_objekt 
            ucHeader.InitUser(m_User)

            FormAuth(Me, m_User)
            lblError.Text = ""

            If Not IsPostBack Then
                GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")
                lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
                ucStyles.TitleText = lblHead.Text
            End If

        Catch ex As Exception
            lblError.Text = "beim laden der Seite ist ein Fehler aufgetreten: " & ex.Message
        End Try
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Protected Sub cmdCreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCreate.Click
        doSubmit()
    End Sub

    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        Response.Redirect("/Portal/(S(" & Session.SessionID & "))/Start/Selection.aspx")
    End Sub

    Private Sub doSubmit()
        Dim tmpFileTable = LoadFile()

        If Not tmpFileTable Is Nothing Then
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"

            mObjUpload = New UploadSperr_Entsperr(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, strFileName)
            If rdo_list.Items(0).Selected = True Then
                mObjUpload.generateSperrTable(tmpFileTable)
                mObjUpload.Task = "Sperren"
            Else
                mObjUpload.generateEntsperrTable(tmpFileTable)
                mObjUpload.Task = "EntSperren"
            End If

            If mObjUpload.Status = 0 Then

                mObjUpload.save()
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
        Dim sConnectionString As String = "Provider=Microsoft.Jet.OLEDB.4.0;" & _
                         "Data Source=" & filepath & filename & ";Extended Properties=""Excel 8.0;HDR=No"""
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
        If (Not upFile.PostedFile Is Nothing) AndAlso (Not (upFile.PostedFile.FileName = String.Empty)) Then
            lblExcelfile.Text = upFile.PostedFile.FileName
            If Right(upFile.PostedFile.FileName.ToUpper, 4) <> ".XLS" Then
                lblError.Text = "Es können nur Dateien im .XLS - Format verarbeitet werden."
                Return Nothing
                Exit Function
            End If
            If (upFile.PostedFile.ContentLength > CType(ConfigurationManager.AppSettings("MaxUploadSize"), Integer)) Then
                lblError.Text = "Datei '" & upFile.PostedFile.FileName & "' ist zu gross (>300 KB)."
                Return Nothing
                Exit Function
            End If
        Else
            lblError.Text = "Keine Datei ausgewählt"
            Return Nothing
            Exit Function
        End If
        'Lade Datei
        Return getFileData(upFile.PostedFile)
    End Function

    Private Function getFileData(ByVal uFile As System.Web.HttpPostedFile) As DataTable
        Dim tmpTable As New DataTable
        Try
            Dim filepath As String = ConfigurationManager.AppSettings("ExcelPath")
            Dim filename As String
            Dim info As System.IO.FileInfo

            'Dateiname: User_yyyyMMddhhmmss.xls
            filename = m_User.UserName & "_" & Format(Now, "yyyyMMddhhmmss") & ".xls"

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
                'lblNoData.Visible = True
                'lblNoData.Text = "Keine Daten zur Anzeige gefunden."
            Else
                GridView1.Visible = True
                ExcelCell.Visible = True
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

                If mObjUpload.Task = "EntSperren" Then

                End If
                If GridView1.PageCount > 1 Then
                    GridView1.PagerStyle.CssClass = "PagerStyle"
                    GridView1.PageIndex = intTempPageIndex
                    GridView1.DataBind()
                End If
                If mObjUpload.Task = "EntSperren" Then
                    GridView1.Columns(1).Visible = False
                    GridView1.Columns(2).Visible = False
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
            If mObjUpload.Task = "Sperren" Then
                With tblExcel
                    .Columns.Add("Fahrgestellnummer", System.Type.GetType("System.String"))
                    .Columns.Add("Datum Sperre bis", System.Type.GetType("System.String"))
                    .Columns.Add("Sperrvermerk", System.Type.GetType("System.String"))
                    .Columns.Add("Status", System.Type.GetType("System.String"))

                End With
                For Each row In mObjUpload.Erledigt.Rows
                    Newrow = tblExcel.NewRow
                    Newrow("Fahrgestellnummer") = Row("CHASSIS_NUM")
                    Newrow("Datum Sperre bis") = Row("DAT_SPERRE")
                    Newrow("Sperrvermerk") = Row("SPERRVERMERK")
                    Newrow("Status") = Row("RETUR_BEM")
                    tblExcel.Rows.Add(Newrow)
                Next
            Else
                With tblExcel
                    .Columns.Add("Fahrgestellnummer", System.Type.GetType("System.String"))
                    .Columns.Add("Status", System.Type.GetType("System.String"))
                End With
                For Each Row In mObjUpload.Erledigt.Rows
                    Newrow = tblExcel.NewRow
                    Newrow("Fahrgestellnummer") = Row("CHASSIS_NUM")
                    Newrow("Status") = Row("RETUR_BEM")
                    tblExcel.Rows.Add(Newrow)
                Next
            End If
            Try
                Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
                Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName

                excelFactory.CreateDocumentAndSendAsResponse(strFileName, tblExcel, Me.Page)

            Catch ex As Exception
                lblError.Text = "Fehler beim Erstellen der Excel-Datei: " + ex.Message
            End Try
        End If
    End Sub

    Protected Sub lnkCreateExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel.Click
        CreateExcel()
    End Sub

    Private Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging

        If Not Session("App_mObjUpload") Is Nothing Then
            mObjUpload = CType(Session("App_mObjUpload"), UploadSperr_Entsperr)
            If rdo_list.Items(0).Selected = True Then
                m_objTable = mObjUpload.Sperren
            ElseIf rdo_list.Items(1).Selected = True Then
                m_objTable = mObjUpload.Entsperren
            End If
            FillGrid(e.NewPageIndex)
        Else
            lblError.Text = "Benötigtes Session Objekt nicht vorhanden!"
        End If



    End Sub
End Class