Option Explicit On
Option Strict On
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports System.Data.OleDb
Imports System.Drawing

Partial Public Class Change09
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Protected WithEvents ucStyles As Global.CKG.Portal.PageElements.Styles
    Protected WithEvents ucHeader As Global.CKG.Portal.PageElements.Header

    Private mObjUpload As UploadBereitstellung
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
        Response.Redirect("/PortalORM/(S(" & Session.SessionID & "))/Start/Selection.aspx")
    End Sub


    Private Sub doSubmit()
        Dim tmpFileTable = LoadFile()

        If Not tmpFileTable Is Nothing Then
            Dim strTemp As String = ""
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"

            mObjUpload = New UploadBereitstellung(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, strFileName)
            mObjUpload.generateUploadTable(tmpFileTable)
            If mObjUpload.Status = 0 Then

                mObjUpload.SetBereitstellung()
                Session.Add("App_mObjUploadBereit", mObjUpload)
                If Not mObjUpload.Erledigt Is Nothing Then
                    m_objTable = mObjUpload.Erledigt
                    Session.Add("App_mObjUploadBereit", mObjUpload)
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
                If GridView1.PageCount > 1 Then
                    GridView1.PagerStyle.CssClass = "PagerStyle"
                    GridView1.DataBind()
                End If
                For Each tmpItem As GridViewRow In GridView1.Rows
                    Dim tmpLabel As Label
                    tmpLabel = CType(tmpItem.FindControl("lblMessage"), Label)

                    Dim tmpStr As String = tmpLabel.Text

                    If Not tmpStr.Trim(" "c) = "" Then
                        If tmpStr = "Status " & Chr(34) & "Bereit" & Chr(34) & " gebucht." Then
                            tmpLabel.ForeColor = Color.Green
                        Else
                            tmpLabel.ForeColor = Color.Red
                        End If
                    End If
                Next
            End If
        Catch ex As Exception
            lblError.Text = "Daten konnten nicht geladen werden!"
        End Try
    End Sub

    Private Sub CreateExcel()
        Dim tblExcel As DataTable
        Dim Newrow As DataRow
        Dim Row As DataRow
        If Not (Session("App_mObjUploadBereit") Is Nothing) Then

            mObjUpload = CType(Session("App_mObjUploadBereit"), UploadBereitstellung)
            tblExcel = New DataTable
            With tblExcel
                .Columns.Add("Fahrgestellnummer", System.Type.GetType("System.String"))
                .Columns.Add("Status", System.Type.GetType("System.String"))
            End With
            For Each Row In mObjUpload.Erledigt.Rows
                Newrow = tblExcel.NewRow
                Newrow("Fahrgestellnummer") = Row("CHASSIS_NUM")
                Newrow("Status") = Row("RET_BEM")
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

    Protected Sub lnkCreateExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel.Click
        CreateExcel()
    End Sub
End Class