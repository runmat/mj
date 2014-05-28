Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common
Imports System.Data.OleDb
Imports System.Data
Imports System.Configuration

Public Class Change03
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
    Private objHandler As Change_03

    Protected WithEvents Hyperlink1 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkLogout As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents tblUpload As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents upFile As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents lblExcelfile As System.Web.UI.WebControls.Label
    Protected WithEvents lb_Upload As System.Web.UI.WebControls.LinkButton
    Protected WithEvents Datagrid2 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents tr_Auftrag As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lbl_Auftrag As System.Web.UI.WebControls.Label
    Protected WithEvents litAuftragShow As System.Web.UI.WebControls.Literal
    Protected WithEvents lb_Back As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbl_Haendler As System.Web.UI.WebControls.Label
    Protected WithEvents litHaendlerShow As System.Web.UI.WebControls.Literal
    Protected WithEvents tr_Haendler As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents Literal1 As System.Web.UI.WebControls.Literal

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text
        m_App = New Base.Kernel.Security.App(m_User)

        If (Session("objHandler") Is Nothing) OrElse (Not IsPostBack) Then
            DoSubmit()
            Session("objHandler") = objHandler
        Else
            objHandler = CType(Session("objHandler"), Change_03)
        End If

        If Not IsPostBack Then
            tblUpload.Visible = False
            lb_Upload.Visible = False
            lb_Back.Visible = False

            'Literal1.Text = "		<script language=""JavaScript"">" & vbCrLf
            'Literal1.Text &= "			<!-- //" & vbCrLf
            'Literal1.Text &= "			window.document.Form1.txtVorhaben.focus();" & vbCrLf
            'Literal1.Text &= "			//-->" & vbCrLf
            'Literal1.Text &= "		</script>" & vbCrLf
            Literal1.Text = ""
        End If
       
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        Dim tmpDataView As New DataView()
        tmpDataView = objHandler.Result.DefaultView

        If tmpDataView.Count = 0 Then
            Datagrid2.Visible = False
        Else
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

            Datagrid2.CurrentPageIndex = intTempPageIndex

            Datagrid2.DataSource = tmpDataView
            Datagrid2.DataBind()

            If Datagrid2.PageCount > 1 Then
                Datagrid2.PagerStyle.CssClass = "PagerStyle"
                Datagrid2.DataBind()
                Datagrid2.PagerStyle.Visible = True
            Else
                Datagrid2.PagerStyle.Visible = False
            End If

            Dim item As DataGridItem
            For Each item In Datagrid2.Items
                If objHandler.Vbeln = item.Cells(0).Text Then
                    item.CssClass = "GridTableHighlight"
                    SetLiteral1ForDatagrid2(item.Cells(0).Text)
                End If
            Next
        End If
    End Sub

    Private Sub SetLiteral1ForDatagrid2(ByVal strTarget As String)
        Literal1.Text = "						<script language=""Javascript"">" & vbCrLf
        Literal1.Text &= "						  <!-- //" & vbCrLf
        Literal1.Text &= "						    window.document.location.href = ""#" & strTarget & """;" & vbCrLf
        Literal1.Text &= "						  //-->" & vbCrLf
        Literal1.Text &= "						</script>" & vbCrLf
    End Sub

    Private Sub DoSubmit()
        objHandler = New Change_03(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
        objHandler.Status_I = "G"
        objHandler.Show()
        FillGrid(0)
    End Sub

    Private Sub upload(ByVal uFile As System.Web.HttpPostedFile)

        Try
            Dim filepath As String = ConfigurationManager.AppSettings("UploadpathLocal")

            Dim filename As String
            Dim info As System.IO.FileInfo

            'Dateiname: User_yyyyMMddhhmmss.xls
            filename = m_User.UserName & "_" & Format(Now, "yyyyMMddhhmmss") & ".xls"

            If Not (uFile Is Nothing) Then
                uFile.SaveAs(ConfigurationManager.AppSettings("UploadpathLocal") & filename)
                info = New System.IO.FileInfo(filepath & filename)
                If Not (info.Exists) Then
                    lblError.Text = "Fehler beim Speichern."
                    Exit Sub
                End If

                'Datei gespeichert -> Auswertung
                Dim sConnectionString As String = "Provider=Microsoft.Jet.OLEDB.4.0;" & _
                 "Data Source=" & filepath & filename & ";" & _
                 "Extended Properties=""Excel 8.0;HDR=YES;"""
                Dim objConn As New OleDbConnection(sConnectionString)


                objConn.Open()

                Dim objCmdSelect As New OleDbCommand("SELECT * FROM [Tabelle1$]", objConn)

                Dim objAdapter1 As New OleDbDataAdapter()
                objAdapter1.SelectCommand = objCmdSelect

                Dim objDataset1 As New DataSet()
                objAdapter1.Fill(objDataset1, "XLData")

                Dim rowData As DataRow

                Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
                logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
                Session.Add("logObj", logApp)

                Dim tmpRow As DataRow

                objHandler.ReNewPositions()

                For Each rowData In objDataset1.Tables(0).Rows
                    If TypeOf rowData(0) Is System.DBNull Or TypeOf rowData(1) Is System.DBNull Then GoTo Ignore

                    tmpRow = objHandler.Positionen.NewRow

                    If Not TypeOf rowData(0) Is System.DBNull Then
                        tmpRow("Pruefungsart") = CStr(rowData(0)).Trim(" "c)
                    End If
                    If Not TypeOf rowData(1) Is System.DBNull Then
                        tmpRow("Fin") = CStr(rowData(1)).Trim(" "c)
                    End If
                    If Not TypeOf rowData(2) Is System.DBNull Then
                        tmpRow("Referenz") = CStr(rowData(2)).Trim(" "c)
                    End If
                    If Not TypeOf rowData(3) Is System.DBNull Then
                        tmpRow("Kennzeichen") = CStr(rowData(3)).Trim(" "c)
                    End If
                    If Not TypeOf rowData(4) Is System.DBNull Then
                        tmpRow("Modell") = CStr(rowData(4)).Trim(" "c)
                    End If
                    If Not TypeOf rowData(5) Is System.DBNull Then
                        tmpRow("Farbe") = CStr(rowData(5)).Trim(" "c)
                    End If
                    If Not TypeOf rowData(6) Is System.DBNull Then
                        tmpRow("Km_stand") = CStr(rowData(6)).Trim(" "c)
                    End If
                    If Not TypeOf rowData(7) Is System.DBNull Then
                        tmpRow("Finanzierung") = CStr(rowData(7)).Trim(" "c)
                    End If
                    If Not TypeOf rowData(8) Is System.DBNull Then
                        tmpRow("Bemerkung") = CStr(rowData(8)).Trim(" "c)
                    End If

                    objHandler.Positionen.Rows.Add(tmpRow)

Ignore:
                Next


                objConn.Close()
                If Not objHandler.Positionen.Rows Is Nothing AndAlso objHandler.Positionen.Rows.Count > 0 Then
                    objHandler.FillAdditionalColumn()
                    Session("objHandler") = objHandler
                    Response.Redirect("Change03_2.aspx?AppID=" & Session("AppID").ToString)
                Else
                    lblError.Text = "Datei enthielt keine verwendbaren Daten."
                End If
            End If
        Catch ex As Exception
            lblError.Text = ex.Message
            Throw ex
        Finally

        End Try
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Datagrid2_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles Datagrid2.SortCommand

        FillGrid(0, e.SortExpression)

    End Sub

    Private Sub Datagrid2_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles Datagrid2.PageIndexChanged

        FillGrid(e.NewPageIndex)
       
    End Sub

    Private Sub Datagrid2_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles Datagrid2.ItemCommand

        If e.CommandName = "Select" Then
            objHandler.Vbeln = e.Item.Cells(0).Text
            litAuftragShow.Text = e.Item.Cells(0).Text & " zum " & e.Item.Cells(1).Text
            litHaendlerShow.Text = e.Item.Cells(2).Text & ",<br>" & e.Item.Cells(3).Text & " " & e.Item.Cells(4).Text & ",<br>" & e.Item.Cells(5).Text & " " & e.Item.Cells(6).Text
            Session("objHandler") = objHandler
            Datagrid2.Visible = False
            lb_Upload.Visible = True
            lb_Back.Visible = True
            tblUpload.Visible = True
        End If
       
    End Sub

    Private Sub lb_Back_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_Back.Click

        FillGrid(0)

        Datagrid2.Visible = True
        lb_Upload.Visible = False
        lb_Back.Visible = False
        tblUpload.Visible = False
       
    End Sub

    Private Sub lb_Upload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_Upload.Click
        'Prüfe Fehlerbedingung
        If (Not upFile.PostedFile Is Nothing) AndAlso (Not (upFile.PostedFile.FileName = String.Empty)) Then
            lblExcelfile.Text = upFile.PostedFile.FileName
            If Right(upFile.PostedFile.FileName.ToUpper, 4) <> ".XLS" Then
                lblError.Text = "Es können nur Dateien im .XLS - Format verarbeitet werden."
                Exit Sub
            End If
        Else
            lblError.Text = "Keine Datei ausgewählt"
            Exit Sub
        End If

        'Lade Datei
        upload(upFile.PostedFile)
    End Sub
End Class

' ************************************************
' $History: Change03.aspx.vb $
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 5.06.09    Time: 13:54
' Updated in $/CKAG/Components/ComCommon
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 5.06.09    Time: 13:51
' Updated in $/CKAG/Components/ComCommon
' Try Catch entfernt wenn möglich
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 11.04.08   Time: 13:43
' Updated in $/CKAG/Components/ComCommon
' Migration
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 10.04.08   Time: 17:37
' Updated in $/CKAG/Components/ComCommon
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:32
' Created in $/CKAG/Components/ComCommon
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 10:47
' Created in $/CKAG/portal/Components/ComCommon
' 
' *****************  Version 5  *****************
' User: Uha          Date: 27.09.07   Time: 10:27
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' Controlnamen für Feldübersetzungen geändert
' 
' *****************  Version 4  *****************
' User: Uha          Date: 26.09.07   Time: 16:49
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' In Change01, Change03 und Change80 neues Format "GridTableHighlight"
' verwendet.
' 
' *****************  Version 3  *****************
' User: Uha          Date: 26.09.07   Time: 13:22
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' Bugfixing in ITA 1237, 1181 und 1238 (Alle Floorcheck)
' 
' *****************  Version 2  *****************
' User: Uha          Date: 25.09.07   Time: 17:03
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' ITA 1124 hinzugefügt (Change03/Change03_2) und allgemeines Bugfix
' Floorcheck
' 
' *****************  Version 1  *****************
' User: Uha          Date: 24.09.07   Time: 18:07
' Created in $/CKG/Components/ComCommon/ComCommonWeb
' ITA 1124: Upload Prüflisten via WEB - Nicht lauffähige Vorversion
' 
' ************************************************
