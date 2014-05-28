Imports KBS.KBS_BASE

Partial Public Class Infocenter
    Inherits Page

    Private mObjKasse As Kasse
    Dim table As DataTable
    Dim fileSourcePath As String
    Dim fileGruppe As String
    Dim FlagMaster As Boolean

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        FormAuth(Me)
        lblError.Text = ""

        If mObjKasse Is Nothing Then
            If Not Session("mKasse") Is Nothing Then
                mObjKasse = CType(Session("mKasse"), Kasse)
            Else
                Throw New Exception("benötigtes Session Objekt nicht vorhanden")
            End If
        End If

        Title = lblHead.Text
        GridNavigation1.setGridElment(GridView1)
        Dim sfolder As String()
        fileSourcePath = ConfigurationManager.AppSettings("DownloadPathSamba")
        Literal1.Text = ""

        fileGruppe = "Filiale"
        fileSourcePath = fileSourcePath & CType(Session("mKasse"), Kasse).KUNNR & "\" & fileGruppe & "\"

        FlagMaster = False
        If Not IsPostBack Then
            sfolder = IO.Directory.GetDirectories(fileSourcePath)
            Array.Sort(sfolder)
            FillddlListbox(sfolder)
            DoSubmit()
        End If

    End Sub

    Private Sub FillddlListbox(ByRef folderArray() As String)

        Dim sfoldName As String
        Dim objFolder As IO.FileInfo
        If folderArray.Length > 0 Then
            ListBox1.Items.Clear()
            For Each sfoldName In folderArray
                objFolder = New IO.FileInfo(sfoldName)
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
        End If
    End Sub

    Protected Sub ListBox1_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ListBox1.SelectedIndexChanged
        lblNoData.Visible = False
        lblNoData.Text = ""
        DoSubmit()
        ListBox1.Visible = True
        Result.Visible = True
    End Sub

    Private Sub DoSubmit()
        Dim row As DataRow
        Dim column As DataColumn
        Dim files As String()
        Dim files2 As String()
        Dim files3 As String()
        Dim files4 As String()
        Dim files5 As String()
        Dim info As IO.FileInfo
        Dim i As Integer
        Dim fname As String
        Dim fname1 As String

        Try
            table = New DataTable()
            column = New DataColumn("Serverpfad", Type.GetType("System.String"))
            table.Columns.Add(column)
            column = New DataColumn("Filename", Type.GetType("System.String"))
            table.Columns.Add(column)
            column = New DataColumn("Filedate", Type.GetType("System.DateTime"))
            table.Columns.Add(column)
            column = New DataColumn("Pfad", Type.GetType("System.String"))
            table.Columns.Add(column)
            column = New DataColumn("Pattern", Type.GetType("System.String"))
            table.Columns.Add(column)

            If ListBox1.SelectedIndex <> -1 Then
                If FlagMaster = False Then
                    fileSourcePath = fileSourcePath & "\" & ListBox1.Items(ListBox1.SelectedIndex).Value
                Else
                    fileSourcePath = fileSourcePath & fileGruppe & "\" & ListBox1.Items(ListBox1.SelectedIndex).Value
                End If

                files = IO.Directory.GetFiles(fileSourcePath, "*.pdf")
                files2 = IO.Directory.GetFiles(fileSourcePath, "*.xls")
                files3 = IO.Directory.GetFiles(fileSourcePath, "*.doc")
                files4 = IO.Directory.GetFiles(fileSourcePath, "*.jpg")
                files5 = IO.Directory.GetFiles(fileSourcePath, "*.gif")
            Else
                files = IO.Directory.GetFiles(fileSourcePath & "\", "*.pdf")
                files2 = IO.Directory.GetFiles(fileSourcePath & "\", "*.xls")
                files3 = IO.Directory.GetFiles(fileSourcePath & "\", "*.doc")
                files4 = IO.Directory.GetFiles(fileSourcePath & "\", "*.jpg")
                files5 = IO.Directory.GetFiles(fileSourcePath & "\", "*.gif")
            End If


            For i = 0 To files.Length - 1
                info = New IO.FileInfo(files.GetValue(i).ToString)
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
                info = New IO.FileInfo(files2.GetValue(i).ToString)
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
                info = New IO.FileInfo(files3.GetValue(i).ToString)
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
                info = New IO.FileInfo(files4.GetValue(i).ToString)
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
                info = New IO.FileInfo(files5.GetValue(i).ToString)
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
            Fillgrid(0, table)
            Session("App_InfoTable") = table

        Catch ex As Exception
            lblNoData.Visible = True
            lblNoData.Text = "Keine Dokumente zur Anzeige gefunden."
        End Try
    End Sub

    Private Sub Fillgrid(ByVal intPageIndex As Int32, ByVal tbl As DataTable, Optional ByVal strSort As String = "")

        If tbl.Rows.Count = 0 Then
            GridView1.Visible = False
            lblNoData.Visible = True
            lblNoData.Text = "Keine Dokumente zur Anzeige gefunden."
        Else
            GridView1.Visible = True
            lblNoData.Visible = False
            Dim tmpDataView As DataView = tbl.DefaultView

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
                strTempSort = "Filename"
                strDirection = "asc"
                ViewState("Sort") = strTempSort
                ViewState("Direction") = strDirection
            End If
            If Not strTempSort.Length = 0 Then
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

            lblNoData.Visible = True

        End If
    End Sub

    Private Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles GridView1.RowCommand
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
                Literal1.Text &= "                          window.open(""Infocenter_print.aspx"" , ""_blank"", ""left=0,top=0,resizable=YES,scrollbars=YES"");" & vbCrLf
                Literal1.Text &= "						  //-->" & vbCrLf
                Literal1.Text &= "						</script>" & vbCrLf
            ElseIf lblPattern.Text = "xls" Then
                Dim ExcelFactory As New DocumentGeneration.ExcelDocumentFactory()
                lblServerpfad = CType(GridRow.Cells(2).FindControl("lblServerpfad"), Label)
                sPfad = lblServerpfad.Text()
                fname = Right(sPfad, sPfad.Length - sPfad.LastIndexOf("\") - 1)
                ExcelFactory.ReturnExcelTab(sPfad, fname, Page)
            ElseIf lblPattern.Text = "doc" Then
                Dim dt As New DataTable
                Dim imageHashTable As New Hashtable
                Dim WordFactory As New DocumentGeneration.WordDocumentFactory(dt, imageHashTable)
                lblServerpfad = CType(GridRow.Cells(2).FindControl("lblServerpfad"), Label)
                sPfad = lblServerpfad.Text()
                fname = Right(sPfad, sPfad.Length - sPfad.LastIndexOf("\") - 1)
                dt = Nothing
                imageHashTable = Nothing
                WordFactory.Returndoc(sPfad, fname, Page)
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
                Literal1.Text &= "                          window.open(""Infocenter_print.aspx"", ""_blank"", ""left=0,top=0,resizable=YES,scrollbars=YES"");" & vbCrLf
                Literal1.Text &= "						  //-->" & vbCrLf
                Literal1.Text &= "						</script>" & vbCrLf
            End If
            ListBox1.Visible = True
            Result.Visible = True
            DoSubmit()

        End If
    End Sub

    Private Sub GridView1_RowCreated(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles GridView1.RowCreated
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

    Protected Sub lb_zurueck_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_zurueck.Click
        responseBack()
    End Sub

    Private Sub responseBack()

        Response.Redirect("../Selection.aspx")
    End Sub

    Private Sub GridNavigation1_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation1.PagerChanged
        GridView1.PageIndex = PageIndex
        table = CType(Session("App_InfoTable"), DataTable)
        Fillgrid(PageIndex, table)
    End Sub

    Private Sub GridNavigation1_PageSizeChanged() Handles GridNavigation1.PageSizeChanged
        table = CType(Session("App_InfoTable"), DataTable)
        Fillgrid(0, table)
    End Sub

End Class