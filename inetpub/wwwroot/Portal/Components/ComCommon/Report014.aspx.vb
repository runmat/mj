Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common
Imports System.IO

Public Class Report014
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

    Private m_User As Security.User
    Private m_App As Security.App
    Dim table As DataTable

    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents cmdCreate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbtnUpload As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdDel As System.Web.UI.WebControls.LinkButton
    Protected WithEvents Button1 As System.Web.UI.WebControls.Button
    Protected WithEvents trSelection As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents ListBox1 As System.Web.UI.WebControls.ListBox
    Protected WithEvents Literal1 As System.Web.UI.WebControls.Literal
    Protected WithEvents ddlGruppe As System.Web.UI.WebControls.DropDownList
    Protected WithEvents trGruppe As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trResult As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trUpload As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents upFile As System.Web.UI.WebControls.FileUpload
    Dim fileSourcePath As String
    Dim fileGruppe As String
    Dim fileSavePath As String
    Dim FlagMaster As Boolean


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim sfolder As String()

        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text
        Literal1.Text = ""

        fileSourcePath = CType(ConfigurationManager.AppSettings("DownloadPathSamba"), String)
        If Request.QueryString("Flag") = "M" Then
            fileSourcePath = fileSourcePath & m_User.KUNNR & "\"
            InfoMasterVisible(True)
            FlagMaster = True
            If ddlGruppe.SelectedIndex <> -1 Then fileGruppe = ddlGruppe.SelectedItem.Text
        Else
            fileGruppe = m_User.Groups(0).GroupName
            fileSourcePath = fileSourcePath & m_User.KUNNR & "\" & fileGruppe & "\"
            InfoMasterVisible(False)
            FlagMaster = False
        End If

        If Not IsPostBack Then
            sfolder = System.IO.Directory.GetDirectories(fileSourcePath)
            Array.Sort(sfolder)
            If FlagMaster = True Then
                FillddlGruppe(sfolder)
            Else
                FillddlListbox(sfolder)
                DoSubmit()
            End If

        End If
       
    End Sub
    Private Sub FillddlGruppe(ByRef folderArray() As String)

        Dim sfoldName As String
        Dim objFolder As System.IO.FileInfo
        If folderArray.Length > 0 Then
            Dim Listitem As New ListItem
            Listitem.Text = "---"
            Listitem.Value = "999"
            ddlGruppe.Items.Add(Listitem)
            For Each sfoldName In folderArray
                objFolder = New System.IO.FileInfo(sfoldName)
                Dim tmpfo As String = Replace(objFolder.Name, "_", " ")
                Dim tmpname As String
                tmpname = Right(tmpfo, tmpfo.Length - 1)
                tmpname = Left(tmpfo, 1).ToUpper & tmpname

                Listitem = New ListItem
                Listitem.Text = tmpname
                Listitem.Value = objFolder.Name
                ddlGruppe.Items.Add(Listitem)
            Next

        End If

    End Sub

    Private Sub FillddlListbox(ByRef folderArray() As String)

        Dim sfoldName As String
        Dim objFolder As System.IO.FileInfo
        If folderArray.Length > 0 Then
            ListBox1.Items.Clear()
            For Each sfoldName In folderArray
                objFolder = New System.IO.FileInfo(sfoldName)
                Dim tmpfo As String = Replace(objFolder.Name, "_", " ")
                Dim tmpname As String
                tmpname = Right(tmpfo, tmpfo.Length - 1)
                tmpname = Left(tmpfo, 1).ToUpper & tmpname

                Dim Listitem As New ListItem
                Listitem.Text = tmpname
                Listitem.Value = objFolder.Name
                ListBox1.Items.Add(Listitem)
            Next
            ListBox1.SelectedIndex = 0
            ListBox1.Rows = folderArray.Length
        End If
    End Sub
    Private Sub InfoMasterVisible(ByVal Visible As Boolean)
        trResult.Visible = Not Visible
        ListBox1.Visible = Not Visible
        trGruppe.Visible = Visible
        trUpload.Visible = Not Visible
    End Sub

    Private Sub Fillgrid(ByVal intPageIndex As Int32, ByVal Table As DataTable, Optional ByVal strSort As String = "", Optional ByVal direction As String = "")

        If Table.Rows.Count = 0 Then
            DataGrid1.Visible = False
            lblNoData.Visible = True
            lblNoData.Text = "Keine Dokumente zur Anzeige gefunden."
            If FlagMaster = False Then
                DataGrid1.Columns(5).Visible = False
                cmdDel.Visible = False
                trUpload.Visible = False
            Else
                trUpload.Visible = True
            End If
        Else
            DataGrid1.Visible = True
            lblNoData.Visible = False
            Dim tmpDataView As New DataView()
            tmpDataView = Table.DefaultView

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
                strTempSort = "Filedate"
                strDirection = "asc"
                ViewState("Sort") = strTempSort
                ViewState("Direction") = strDirection
            End If
            If Not strTempSort.Length = 0 Then

                If direction.Length > 0 Then
                    strDirection = direction
                End If

                tmpDataView.Sort = strTempSort & " " & strDirection

            End If
            DataGrid1.DataSource = tmpDataView
            DataGrid1.DataBind()
            DataGrid1.CurrentPageIndex = intTempPageIndex

            If DataGrid1.PageCount > 1 Then
                DataGrid1.PagerStyle.CssClass = "PagerStyle"
                DataGrid1.DataBind()
                DataGrid1.PagerStyle.Visible = True
            Else
                DataGrid1.PagerStyle.Visible = False
            End If

            Dim item As DataGridItem
            Dim cell As TableCell
            Dim linBut As ImageButton
            Dim control As Control
            Dim sPattern As String


            For Each item In DataGrid1.Items
                sPattern = item.Cells(3).Text
                cell = item.Cells(0)
                If sPattern = "xls" Then
                    For Each control In cell.Controls
                        If TypeOf control Is ImageButton Then
                            linBut = CType(control, ImageButton)
                            If linBut.ID = "lbtExcel" Then
                                linBut.Visible = True
                            End If
                        End If
                    Next
                ElseIf sPattern = "doc" Then
                    For Each control In cell.Controls
                        If TypeOf control Is ImageButton Then
                            linBut = CType(control, ImageButton)
                            If linBut.ID = "lbtWord" Then
                                linBut.Visible = True
                            End If
                        End If
                    Next

                ElseIf sPattern.ToLower = "jpg" Or sPattern.ToLower = "jepg" Then
                    For Each control In cell.Controls
                        If TypeOf control Is ImageButton Then
                            linBut = CType(control, ImageButton)
                            If linBut.ID = "lbtJepg" Then
                                linBut.Visible = True
                            End If
                        End If
                    Next
                ElseIf sPattern.ToLower = "pdf" Then
                    For Each control In cell.Controls
                        If TypeOf control Is ImageButton Then
                            linBut = CType(control, ImageButton)
                            If linBut.ID = "lbtPDF" Then
                                linBut.Visible = True
                            End If
                        End If
                    Next
                ElseIf sPattern.ToLower = "gif" Then
                    For Each control In cell.Controls
                        If TypeOf control Is ImageButton Then
                            linBut = CType(control, ImageButton)
                            If linBut.ID = "lbtGif" Then
                                linBut.Visible = True
                            End If
                        End If
                    Next
                End If
            Next
            If FlagMaster = False Then
                DataGrid1.Columns(5).Visible = False
                cmdDel.Visible = False
                trUpload.Visible = False
            Else
                trUpload.Visible = True
                cmdDel.Visible = True
            End If
            lblNoData.Visible = True
            lblNoData.Text = "Folgende Dokumente stehen zum Download bereit."
        End If
    End Sub

    Private Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.ItemCommand
        If e.CommandName = "open" Then
            Dim sPfad As String
            Dim fname As String
            If e.Item.Cells(3).Text = "pdf" Then
                sPfad = e.Item.Cells(2).Text()
                Session("App_Filepath") = sPfad
                Session("App_ContentType") = "Application/pdf"

                Literal1.Text = "						<script language=""Javascript"">" & vbCrLf
                Literal1.Text &= "						  <!-- //" & vbCrLf
                Literal1.Text &= "                          window.open(""Report014_1.aspx?AppID=" & Session("AppID").ToString & """, ""_blank"", ""left=0,top=0,resizable=YES,scrollbars=YES"");" & vbCrLf
                Literal1.Text &= "						  //-->" & vbCrLf
                Literal1.Text &= "						</script>" & vbCrLf
            ElseIf e.Item.Cells(3).Text = "xls" Then
                Dim ExcelFactory As New Base.Kernel.DocumentGeneration.ExcelDocumentFactory()
                sPfad = e.Item.Cells(2).Text()
                fname = Right(sPfad, sPfad.Length - sPfad.LastIndexOf("\") - 1)
                ExcelFactory.ReturnExcelTab(sPfad, fname, Me.Page)
            ElseIf e.Item.Cells(3).Text = "doc" Then
                Dim dt As New System.Data.DataTable
                Dim imageHashTable As New Hashtable
                Dim WordFactory As New Base.Kernel.DocumentGeneration.WordDocumentFactory(dt, imageHashTable)
                sPfad = e.Item.Cells(2).Text()
                fname = Right(sPfad, sPfad.Length - sPfad.LastIndexOf("\") - 1)
                dt = Nothing
                imageHashTable = Nothing
                WordFactory.Returndoc(sPfad, fname, Me.Page)
            ElseIf e.Item.Cells(3).Text = "jpg" Or e.Item.Cells(3).Text = "gif" Then
                sPfad = e.Item.Cells(2).Text()
                Session("App_Filepath") = sPfad
                If e.Item.Cells(3).Text = "jpg" Then
                    Session("App_ContentType") = "image/JPEG"
                Else
                    Session("App_ContentType") = "image/GIF"
                End If
                Literal1.Text = "						<script language=""Javascript"">" & vbCrLf
                Literal1.Text &= "						  <!-- //" & vbCrLf
                Literal1.Text &= "                          window.open(""Report014_1.aspx?AppID=" & Session("AppID").ToString & """, ""_blank"", ""left=0,top=0,resizable=YES,scrollbars=YES"");" & vbCrLf
                Literal1.Text &= "						  //-->" & vbCrLf
                Literal1.Text &= "						</script>" & vbCrLf
            End If
            ListBox1.Visible = True
            trResult.Visible = True
            DoSubmit()
            
        End If
    End Sub

    Private Sub DoSubmit()
        Dim row As DataRow
        Dim column As DataColumn
        Dim files As String()
        Dim files2 As String()
        Dim files3 As String()
        Dim files4 As String()
        Dim files5 As String()
        Dim info As System.IO.FileInfo
        Dim i As Integer
        Dim fname As String
        Dim fname1 As String

        Try
            table = New DataTable()
            column = New DataColumn("Serverpfad", System.Type.GetType("System.String"))
            table.Columns.Add(column)
            column = New DataColumn("Filename", System.Type.GetType("System.String"))
            table.Columns.Add(column)
            column = New DataColumn("Filedate", System.Type.GetType("System.DateTime"))
            table.Columns.Add(column)
            column = New DataColumn("Pfad", System.Type.GetType("System.String"))
            table.Columns.Add(column)
            column = New DataColumn("Pattern", System.Type.GetType("System.String"))
            table.Columns.Add(column)

            If ListBox1.SelectedIndex <> -1 Then
                If FlagMaster = False Then
                    fileSourcePath = fileSourcePath & "\" & ListBox1.Items(ListBox1.SelectedIndex).Value
                Else
                    fileSourcePath = fileSourcePath & fileGruppe & "\" & ListBox1.Items(ListBox1.SelectedIndex).Value
                End If

                files = System.IO.Directory.GetFiles(fileSourcePath, "*.pdf")
                files2 = System.IO.Directory.GetFiles(fileSourcePath, "*.xls")
                files3 = System.IO.Directory.GetFiles(fileSourcePath, "*.doc")
                files4 = System.IO.Directory.GetFiles(fileSourcePath, "*.jpg")
                files5 = System.IO.Directory.GetFiles(fileSourcePath, "*.gif")
            Else
                files = System.IO.Directory.GetFiles(fileSourcePath & "\", "*.pdf")
                files2 = System.IO.Directory.GetFiles(fileSourcePath & "\", "*.xls")
                files3 = System.IO.Directory.GetFiles(fileSourcePath & "\", "*.doc")
                files4 = System.IO.Directory.GetFiles(fileSourcePath & "\", "*.jpg")
                files5 = System.IO.Directory.GetFiles(fileSourcePath & "\", "*.gif")
            End If


            For i = 0 To files.Length - 1
                info = New System.IO.FileInfo(files.GetValue(i).ToString)
                fname = files.GetValue(i).ToString
                fname1 = Right(fname, fname.Length - fname.LastIndexOf("\") - 1)
                row = table.NewRow()
                row("Serverpfad") = fileSourcePath & "\" & fname1
                fname1 = Left(fname1, fname1.LastIndexOf("."))
                row("Filename") = fname1
                row("Filedate") = info.CreationTime
                row("Pattern") = "pdf"
                table.Rows.Add(row)
            Next

            For i = 0 To files2.Length - 1
                info = New System.IO.FileInfo(files2.GetValue(i).ToString)
                fname = files2.GetValue(i).ToString
                fname1 = Right(fname, fname.Length - fname.LastIndexOf("\") - 1)
                row = table.NewRow()
                row("Serverpfad") = fileSourcePath & "\" & fname1
                fname1 = Left(fname1, fname1.LastIndexOf("."))
                row("Filename") = fname1
                row("Filedate") = info.CreationTime
                row("Pattern") = "xls"
                table.Rows.Add(row)
            Next
            For i = 0 To files3.Length - 1
                info = New System.IO.FileInfo(files3.GetValue(i).ToString)
                fname = files3.GetValue(i).ToString
                fname1 = Right(fname, fname.Length - fname.LastIndexOf("\") - 1)
                row = table.NewRow()
                row("Serverpfad") = fileSourcePath & "\" & fname1
                fname1 = Left(fname1, fname1.LastIndexOf("."))
                row("Filename") = fname1
                row("Filedate") = info.CreationTime
                row("Pattern") = "doc"
                table.Rows.Add(row)
            Next
            For i = 0 To files4.Length - 1
                info = New System.IO.FileInfo(files4.GetValue(i).ToString)
                fname = files4.GetValue(i).ToString
                fname1 = Right(fname, fname.Length - fname.LastIndexOf("\") - 1)
                row = table.NewRow()
                row("Serverpfad") = fileSourcePath & "\" & fname1
                fname1 = Left(fname1, fname1.LastIndexOf("."))
                row("Filename") = fname1
                row("Filedate") = info.CreationTime
                row("Pattern") = "jpg"
                table.Rows.Add(row)
            Next
            For i = 0 To files5.Length - 1
                info = New System.IO.FileInfo(files5.GetValue(i).ToString)
                fname = files5.GetValue(i).ToString
                fname1 = Right(fname, fname.Length - fname.LastIndexOf("\") - 1)
                row = table.NewRow()
                row("Serverpfad") = fileSourcePath & "\" & fname1
                fname1 = Left(fname1, fname1.LastIndexOf("."))
                row("Filename") = fname1
                row("Filedate") = info.CreationTime
                row("Pattern") = "gif"
                table.Rows.Add(row)
            Next

            Fillgrid(0, table, "filedate", "desc")
            Session("App_InfoTable") = table

        Catch ex As Exception
            lblNoData.Visible = True
            lblNoData.Text = "Keine Dokumente zur Anzeige gefunden."
        End Try
    End Sub

    Protected Sub ListBox1_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ListBox1.SelectedIndexChanged
        DoSubmit()
        ListBox1.Visible = True
        trResult.Visible = True
    End Sub

    Private Sub DataGrid1_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGrid1.ItemDataBound

        Dim cell As TableCell
        Dim ctrlLabel As Label
        cell = e.Item.Cells(1)
        ctrlLabel = cell.FindControl("lblFileDate")
        If Not ctrlLabel Is Nothing Then
            If IsDate(ctrlLabel.Text) Then
                Dim FileDate As Date = CType(ctrlLabel.Text, System.DateTime)
                If FileDate > DateAdd(DateInterval.Day, -25, Now) Then
                    Dim cell2 As TableCell
                    Dim ctrlLabel2 As Label
                    cell2 = e.Item.Cells(4)
                    ctrlLabel2 = cell.FindControl("lblStatus")
                    If Not ctrlLabel2 Is Nothing Then
                        ctrlLabel2.Visible = True
                    End If
                End If
            End If
        End If

    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        If Not Session("App_InfoTable") Is Nothing Then
            Dim table As DataTable = Session("App_InfoTable")
            Fillgrid(e.NewPageIndex, table)
            ListBox1.Visible = True
            trResult.Visible = True
        End If
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        If Not Session("App_InfoTable") Is Nothing Then
            Dim table As DataTable = Session("App_InfoTable")
            Fillgrid(DataGrid1.CurrentPageIndex, table, e.SortExpression)
            ListBox1.Visible = True
            trResult.Visible = True
        End If

    End Sub


    Protected Sub lbtnUpload_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnUpload.Click
        Dim sPath As String
        Dim File As HttpPostedFile
        Dim fName As String
        Dim fileInfo As IO.FileInfo



        If Not (upFile.PostedFile.FileName = String.Empty) Then
            fileInfo = New IO.FileInfo(upFile.PostedFile.FileName)

            Select Case fileInfo.Extension.ToUpper.ToString
                Case ".JPG"
                Case ".PDF"
                Case ".DOC"
                Case ".XLS"
                Case ".GIF"
                Case Else
                    lblError.Text = "Es können nur Dateien im Format jpg, gif, doc, xls und pdf hochgeldaden werden."
                    ListBox1.Visible = True
                    trResult.Visible = True
                    DoSubmit()
                    Exit Sub

            End Select

            sPath = fileSourcePath & "\" & fileGruppe & "\" & ListBox1.Items(ListBox1.SelectedIndex).Value
            File = CType(upFile.PostedFile, System.Web.HttpPostedFile)
            fName = upFile.PostedFile.FileName
            fName = Right(fName, fName.Length - fName.LastIndexOf("\") - 1).ToString
            File.SaveAs(sPath & "\" & fName)

        Else
            lblError.Text = "Keine Datei ausgewählt."
        End If
        ListBox1.Visible = True
        trResult.Visible = True
        DoSubmit()
    End Sub

    Protected Sub ddlGruppe_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlGruppe.SelectedIndexChanged
        Dim sfolder As String()
        Dim sPath As String
        If ddlGruppe.SelectedItem.Text <> "---" Then
            Session("App_fileGruppe") = ddlGruppe.SelectedItem.Text
            fileGruppe = ddlGruppe.SelectedItem.Text
            sPath = fileSourcePath & "\" & fileGruppe & "\"
            sfolder = System.IO.Directory.GetDirectories(sPath)
            Array.Sort(sfolder)
            FillddlListbox(sfolder)
            ListBox1.Visible = True
            trResult.Visible = True
            DoSubmit()
        Else
            ListBox1.Visible = False
            trResult.Visible = False
            trUpload.Visible = False
            cmdDel.Visible = False
            lblNoData.Text = ""
        End If



    End Sub

    Protected Sub cmdDel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdDel.Click
        Dim item As DataGridItem
        Dim cell As TableCell
        Dim chkSel As CheckBox
        Dim sPattern As String
        Dim sPath As String

        Session("App_fileGruppe") = ddlGruppe.SelectedItem.Text
        fileGruppe = ddlGruppe.SelectedItem.Text
        sPath = fileSourcePath & "\" & fileGruppe & "\" & ListBox1.Items(ListBox1.SelectedIndex).Value & "\"

        For Each item In DataGrid1.Items
            sPattern = item.Cells(2).Text
            cell = item.Cells(5)
            chkSel = CType(cell.FindControl("rb_sel"), CheckBox)
            If Not chkSel Is Nothing Then
                If chkSel.Checked Then
                    File.Delete(sPattern)
                End If
            End If
        Next
        ListBox1.Visible = True
        trResult.Visible = True
        DoSubmit()
    End Sub

End Class
' ************************************************
' $History: Report014.aspx.vb $
' 
' *****************  Version 12  *****************
' User: Fassbenders  Date: 25.05.11   Time: 15:44
' Updated in $/CKAG/Components/ComCommon
' 
' *****************  Version 11  *****************
' User: Rudolpho     Date: 30.03.10   Time: 17:23
' Updated in $/CKAG/Components/ComCommon
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 5.06.09    Time: 15:58
' Updated in $/CKAG/Components/ComCommon
' mögliche try catches entfernt
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 2.06.09    Time: 9:17
' Updated in $/CKAG/Components/ComCommon
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 26.05.09   Time: 13:48
' Updated in $/CKAG/Components/ComCommon
' ITA 2879
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 26.05.09   Time: 13:09
' Updated in $/CKAG/Components/ComCommon
' ITA 2879
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 11.05.09   Time: 13:59
' Updated in $/CKAG/Components/ComCommon
' ITA: 2851
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 3.09.08    Time: 12:44
' Updated in $/CKAG/Components/ComCommon
' ITA: 2176
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 14.08.08   Time: 17:33
' Updated in $/CKAG/Components/ComCommon
' ITA: 2176
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
' User: Rudolpho     Date: 25.01.08   Time: 12:41
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 23.01.08   Time: 10:19
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' ITA: 1647
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 22.01.08   Time: 12:58
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' ITA: 1647
' 
' ************************************************
