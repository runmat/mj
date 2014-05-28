Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports System.IO

Partial Public Class Report014s
    Inherits System.Web.UI.Page

    Protected WithEvents GridNavigation1 As CKG.Services.GridNavigation
    Private m_User As Security.User
    Private m_App As Security.App
    Dim table As DataTable
    Dim fileSourcePath As String
    Dim fileGruppe As String
    Dim fileSavePath As String
    Dim FlagMaster As Boolean

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim sfolder As String()
        m_User = GetUser(Me)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)
        GridNavigation1.setGridElment(GridView1)
        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

        fileSourcePath = CType(ConfigurationManager.AppSettings("DownloadPathSamba"), String)
        Literal1.Text = ""
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
            Try
                sfolder = System.IO.Directory.GetDirectories(fileSourcePath)
                Array.Sort(sfolder)
                If FlagMaster = True Then
                    FillddlGruppe(sfolder)
                Else
                    FillddlListbox(sfolder)
                    DoSubmit()
                End If
            Catch ex As DirectoryNotFoundException
                lblError.Text = "Kein Verzeichnis gefunden! Bitte wenden Sie sich an Ihren System-Administrator."
                Result.Visible = False
                PanelUpload.Visible = False
            Catch ex As Exception
                lblError.Text = "Es ist ein Fehler aufgetreten! Bitte wenden Sie sich an Ihren System-Administrator."
                Result.Visible = False
                PanelUpload.Visible = False
            End Try
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
                If FlagMaster = True Then
                    fileSourcePath = fileSourcePath & fileGruppe
                End If

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
                row("Filename") = fname1.Replace("_"c, " "c)
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
                row("Filename") = fname1.Replace("_"c, " "c)
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
                row("Filename") = fname1.Replace("_"c, " "c)
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
                row("Filename") = fname1.Replace("_"c, " "c)
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
                row("Filename") = fname1.Replace("_"c, " "c)
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

    Private Sub Fillgrid(ByVal intPageIndex As Int32, ByVal Table As DataTable, Optional ByVal strSort As String = "", Optional ByVal direction As String = "")

        If Table.Rows.Count = 0 Then
            GridView1.Visible = False
            GridNavigation1.Visible = False
            lblNoData.Visible = True
            lblNoData.Text = "Keine Dokumente zur Anzeige gefunden."
            If FlagMaster = False Then
                GridView1.Columns(5).Visible = False
                cmdDel.Visible = False
                PanelUpload.Visible = False
                DivPlaceholder.Visible = Not PanelUpload.Visible
            Else
                PanelUpload.Visible = True
                DivPlaceholder.Visible = Not PanelUpload.Visible
            End If
        Else
            GridView1.Visible = True
            GridNavigation1.Visible = True
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

            GridView1.DataSource = tmpDataView
            GridView1.DataBind()
            GridView1.PageIndex = intTempPageIndex

            Dim item As GridViewRow
            Dim cell As TableCell
            Dim linBut As ImageButton
            Dim control As Control
            Dim sPattern As String = ""
            Dim lblPattern As Label 

            For Each item In GridView1.Rows
                lblPattern = CType(item.Cells(3).FindControl("lblPattern"), Label)

                If Not lblPattern Is Nothing Then sPattern = lblPattern.Text
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
                GridView1.Columns(5).Visible = False
                cmdDel.Visible = False
                PanelUpload.Visible = False
                DivPlaceholder.Visible = Not PanelUpload.Visible
            Else
                cmdDel.Visible = True
                PanelUpload.Visible = True
                DivPlaceholder.Visible = Not PanelUpload.Visible
            End If
            lblNoData.Visible = True

        End If
    End Sub

    Private Sub FillddlListbox(ByRef folderArray() As String)

        Dim sfoldName As String
        Dim objFolder As System.IO.FileInfo

        ListBox1.Items.Clear()

        If folderArray.Length > 0 Then
            For Each sfoldName In folderArray
                objFolder = New System.IO.FileInfo(sfoldName)
                'Dim tmpfo As String = objFolder.Name 'Replace(objFolder.Name, "_", " ")
                'Dim tmpname As String
                'tmpname = Right(tmpfo, tmpfo.Length - 1)
                '' Erstenbuchstaben groß schreiben
                'tmpname = Left(tmpfo, 1).ToUpper & tmpname

                Dim Listitem As New ListItem
                Listitem.Text = objFolder.Name 'tmpname
                Listitem.Value = objFolder.Name
                ListBox1.Items.Add(Listitem)
            Next

            ListBox1.SelectedIndex = 0
            ShowListBox(True)
            data.Attributes("style") = "float:right; width: 700px;"
        Else
            ShowListBox(False)
        End If
    End Sub

    Private Sub ShowListBox(ByVal Show As Boolean)
        If Show Then
            ListInfo.Visible = True
            data.Attributes("style") = "float:right; width: 700px;"
        Else
            ListInfo.Visible = False
            data.Attributes("style") = "width: 100%;"
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
                Dim tmpfo As String = objFolder.Name 'Replace(objFolder.Name, "_", " ")
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

    Private Sub InfoMasterVisible(ByVal Visible As Boolean)
        Result.Visible = Not Visible
        'ListBox1.Visible = Not Visible
        ShowListBox(Not Visible)
        trGruppe.Visible = Visible
        PanelUpload.Visible = Not Visible
        DivPlaceholder.Visible = Not PanelUpload.Visible
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
                    'ListBox1.Visible = True
                    ShowListBox(True)
                    Result.Visible = True
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

        'ListBox1.Visible = True
        ShowListBox(True)
        Result.Visible = True
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
            'ListBox1.Visible = True
            Result.Visible = True
            DoSubmit()
        Else
            'ListBox1.Visible = False
            ShowListBox(False)
            Result.Visible = False
            PanelUpload.Visible = False
            DivPlaceholder.Visible = Not PanelUpload.Visible
            cmdDel.Visible = False
            lblNoData.Text = ""
        End If

    End Sub

    Protected Sub cmdDel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdDel.Click
        Dim item As GridViewRow
        Dim cell As TableCell
        Dim chkSel As CheckBox
        Dim sPattern As String = ""
        Dim sPath As String
        Dim lblPattern As Label

        Session("App_fileGruppe") = ddlGruppe.SelectedItem.Text
        fileGruppe = ddlGruppe.SelectedItem.Text
        sPath = fileSourcePath & "\" & fileGruppe & "\" & ListBox1.Items(ListBox1.SelectedIndex).Value & "\"

        For Each item In GridView1.Rows
            lblPattern = CType(item.Cells(2).FindControl("lblServerPfad"), Label)
            If Not lblPattern Is Nothing Then sPattern = lblPattern.Text

            cell = item.Cells(5)
            chkSel = CType(cell.FindControl("rb_sel"), CheckBox)
            If Not chkSel Is Nothing Then
                If chkSel.Checked Then
                    File.Delete(sPattern)
                End If
            End If
        Next
        'ListBox1.Visible = True
        ShowListBox(True)
        Result.Visible = True
        DoSubmit()
    End Sub

    Protected Sub ListBox1_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ListBox1.SelectedIndexChanged
        DoSubmit()
        'ListBox1.Visible = True
        ShowListBox(True)
        Result.Visible = True
    End Sub

    Private Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        If e.CommandName = "open" Then
            Dim sPfad As String
            Dim fname As String
            Dim index As Integer
            Dim lblPattern As Label
            Dim lblServerpfad As Label
            index = CType(e.CommandArgument, Integer)

            Dim GridRow As GridViewRow = GridView1.Rows(index)
            lblPattern = CType(GridRow.Cells(3).FindControl("lblPattern"), Label)
            If lblPattern.Text = "pdf" Then
                lblServerpfad = CType(GridRow.Cells(2).FindControl("lblServerpfad"), Label)
                sPfad = lblServerpfad.Text()
                Session("App_Filepath") = sPfad
                Session("App_ContentType") = "Application/pdf"

                Literal1.Text = "						<script language=""Javascript"">" & vbCrLf
                Literal1.Text &= "						  <!-- //" & vbCrLf
                Literal1.Text &= "                          window.open(""Report014_1s.aspx?AppID=" & Session("AppID").ToString & """, ""_blank"", ""left=0,top=0,resizable=YES,scrollbars=YES"");" & vbCrLf
                Literal1.Text &= "						  //-->" & vbCrLf
                Literal1.Text &= "						</script>" & vbCrLf
            ElseIf lblPattern.Text = "xls" Then
                Dim ExcelFactory As New Base.Kernel.DocumentGeneration.ExcelDocumentFactory()
                lblServerpfad = CType(GridRow.Cells(2).FindControl("lblServerpfad"), Label)
                sPfad = lblServerpfad.Text()
                fname = Right(sPfad, sPfad.Length - sPfad.LastIndexOf("\") - 1)
                ExcelFactory.ReturnExcelTab(sPfad, fname, Me.Page)
            ElseIf lblPattern.Text = "doc" Then
                Dim dt As New System.Data.DataTable
                Dim imageHashTable As New Hashtable
                Dim WordFactory As New Base.Kernel.DocumentGeneration.WordDocumentFactory(dt, imageHashTable)
                lblServerpfad = CType(GridRow.Cells(2).FindControl("lblServerpfad"), Label)
                sPfad = lblServerpfad.Text()
                fname = Right(sPfad, sPfad.Length - sPfad.LastIndexOf("\") - 1)
                dt = Nothing
                imageHashTable = Nothing
                WordFactory.Returndoc(sPfad, fname, Me.Page)
            ElseIf lblPattern.Text = "jpg" Or lblPattern.Text = "gif" Then
                lblServerpfad = CType(GridRow.Cells(2).FindControl("lblServerpfad"), Label)
                sPfad = lblServerpfad.Text()
                Session("App_Filepath") = sPfad
                If GridRow.Cells(3).Text = "jpg" Then
                    Session("App_ContentType") = "image/JPEG"
                Else
                    Session("App_ContentType") = "image/GIF"
                End If
                Literal1.Text = "						<script language=""Javascript"">" & vbCrLf
                Literal1.Text &= "						  <!-- //" & vbCrLf
                Literal1.Text &= "                          window.open(""Report014_1s.aspx?AppID=" & Session("AppID").ToString & """, ""_blank"", ""left=0,top=0,resizable=YES,scrollbars=YES"");" & vbCrLf
                Literal1.Text &= "						  //-->" & vbCrLf
                Literal1.Text &= "						</script>" & vbCrLf
            End If
            'ListBox1.Visible = True
            ShowListBox(True)
            Result.Visible = True
            DoSubmit()

        End If
    End Sub

    Private Sub GridView1_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim addButton As LinkButton = CType(e.Row.Cells(0).Controls(0).FindControl("Linkbutton2"), LinkButton)
            addButton.CommandArgument = e.Row.RowIndex.ToString()
            Dim addImgButton As ImageButton = CType(e.Row.Cells(0).Controls(0).FindControl("lbtPDF"), ImageButton)
            addImgButton.CommandArgument = e.Row.RowIndex.ToString()
            addImgButton = CType(e.Row.Cells(0).Controls(0).FindControl("lbtExcel"), ImageButton)
            addImgButton.CommandArgument = e.Row.RowIndex.ToString()
            addImgButton = CType(e.Row.Cells(0).Controls(0).FindControl("lbtWord"), ImageButton)
            addImgButton.CommandArgument = e.Row.RowIndex.ToString()
            addImgButton = CType(e.Row.Cells(0).Controls(0).FindControl("lbtJepg"), ImageButton)
            addImgButton.CommandArgument = e.Row.RowIndex.ToString()
            addImgButton = CType(e.Row.Cells(0).Controls(0).FindControl("lbtGif"), ImageButton)
            addImgButton.CommandArgument = e.Row.RowIndex.ToString()

        End If
    End Sub

    Protected Sub ckb_SelectAll_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim item As GridViewRow
        Dim cell As TableCell
        Dim chkSel As CheckBox
        Dim bChecked As Boolean = CType(sender, CheckBox).Checked
        DoSubmit()
        For Each item In GridView1.Rows
            cell = item.Cells(5)
            chkSel = CType(cell.FindControl("rb_sel"), CheckBox)
            If Not chkSel Is Nothing Then
                chkSel.Checked = bChecked
            End If
        Next
        chkSel = CType(GridView1.HeaderRow.Cells(5).FindControl("ckb_SelectAll"), CheckBox)
        If Not chkSel Is Nothing Then
            chkSel.Checked = bChecked
        End If
        'ListBox1.Visible = True
        ShowListBox(True)
        Result.Visible = True
    End Sub

    Private Sub GridView1_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
        If Not Session("App_InfoTable") Is Nothing Then
            Dim table As DataTable = Session("App_InfoTable")
            Fillgrid(0, table, e.SortExpression)
        End If
    End Sub

    Protected Sub lbBack_Click(sender As Object, e As EventArgs) Handles lbBack.Click
        Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx")
    End Sub
End Class