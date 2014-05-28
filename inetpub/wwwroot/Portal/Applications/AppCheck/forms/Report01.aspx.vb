Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports System.IO

Partial Public Class Report01
    Inherits System.Web.UI.Page

    Protected WithEvents ucHeader As CKG.Portal.PageElements.Header
    Protected WithEvents ucStyles As CKG.Portal.PageElements.Styles

    Private m_User As Security.User
    
    Dim table As DataTable
    Dim fileSourcePath As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text
        Literal1.Text = ""
        Try
            If CType(Request.QueryString("check"), String) = "1" Then ' Kunde
                fileSourcePath = ConfigurationManager.AppSettings("UpDownKundeXL")
                fileSourcePath += m_User.Organization.OrganizationReference & "\export\" & m_User.Reference & "\"
            Else
                fileSourcePath = ConfigurationManager.AppSettings("DownloadPathZulXL")
            End If


            If Not IsPostBack Then
                DoSubmit()
            End If
        Catch ex As Exception
            lblNoData.Visible = True
            lblNoData.Text = "Keine Dokumente zur Anzeige gefunden."
        End Try
    End Sub

    Private Sub Fillgrid(ByVal intPageIndex As Int32, ByVal Table As DataTable, Optional ByVal strSort As String = "", Optional ByVal direction As String = "")

        If Table.Rows.Count = 0 Then
            DataGrid1.Visible = False
            lblNoData.Visible = True
            lblNoData.Text = "Keine Dokumente zur Anzeige gefunden."

        Else
            DataGrid1.Visible = True
            lblNoData.Visible = False

            Dim tmpDataView As DataView = Table.DefaultView

            Dim intTempPageIndex As Int32 = intPageIndex
            Dim strTempSort As String = ""
            Dim strDirection As String

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
            End If

            DataGrid1.DataSource = tmpDataView
            DataGrid1.DataBind()

            Dim item As DataGridItem
            Dim cell As TableCell
            Dim linBut As ImageButton
            Dim control As Control
            Dim sPattern As String

            For Each item In DataGrid1.Items
                sPattern = item.Cells(3).Text
                cell = item.Cells(0)
                If sPattern = "xls" Or sPattern = "csv" Then
                    For Each control In cell.Controls
                        Dim imageButton = TryCast(control, ImageButton)
                        If (imageButton IsNot Nothing) Then
                            linBut = imageButton
                            If linBut.ID = "lbtExcel" Then
                                linBut.Visible = True
                            End If
                        End If
                    Next
                ElseIf sPattern = "doc" Then
                    For Each control In cell.Controls
                        Dim imageButton = TryCast(control, ImageButton)
                        If (imageButton IsNot Nothing) Then
                            linBut = imageButton
                            If linBut.ID = "lbtWord" Then
                                linBut.Visible = True
                            End If
                        End If
                    Next

                ElseIf sPattern.ToLower = "jpg" Or sPattern.ToLower = "jepg" Then
                    For Each control In cell.Controls
                        Dim imageButton = TryCast(control, ImageButton)
                        If (imageButton IsNot Nothing) Then
                            linBut = imageButton
                            If linBut.ID = "lbtJepg" Then
                                linBut.Visible = True
                            End If
                        End If
                    Next
                ElseIf sPattern.ToLower = "pdf" Then
                    For Each control In cell.Controls
                        Dim imageButton = TryCast(control, ImageButton)
                        If (imageButton IsNot Nothing) Then
                            linBut = imageButton
                            If linBut.ID = "lbtPDF" Then
                                linBut.Visible = True
                            End If
                        End If
                    Next
                ElseIf sPattern.ToLower = "gif" Then
                    For Each control In cell.Controls
                        Dim imageButton = TryCast(control, ImageButton)
                        If (imageButton IsNot Nothing) Then
                            linBut = imageButton
                            If linBut.ID = "lbtGif" Then
                                linBut.Visible = True
                            End If
                        End If
                    Next
                ElseIf sPattern.ToLower = "zip" Then
                    For Each control In cell.Controls
                        Dim imageButton = TryCast(control, ImageButton)
                        If (imageButton IsNot Nothing) Then
                            linBut = imageButton
                            If linBut.ID = "lbtZip" Then
                                linBut.Visible = True
                            End If
                        End If
                    Next
                End If
            Next

            lblNoData.Visible = True
            lblNoData.Text = "Folgende Dokumente stehen zum Download bereit."
        End If
    End Sub

    Private Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.ItemCommand
        If e.CommandName = "open" Then
            Dim sPfad As String
            Dim fname As String
            Try
                If e.Item.Cells(3).Text = "pdf" Then
                    sPfad = e.Item.Cells(2).Text()
                    Session("App_Filepath") = sPfad
                    Session("App_ContentType") = "Application/pdf"

                    Literal1.Text = "						<script language=""Javascript"">" & vbCrLf
                    Literal1.Text &= "						  <!-- //" & vbCrLf
                    Literal1.Text &= "                          window.open(""Report01_2.aspx?AppID=" & Session("AppID").ToString & """, ""_blank"", ""left=0,top=0,resizable=YES,scrollbars=YES"");" & vbCrLf
                    Literal1.Text &= "						  //-->" & vbCrLf
                    Literal1.Text &= "						</script>" & vbCrLf

                ElseIf e.Item.Cells(3).Text = "xls" Then
                    Dim ExcelFactory As New DocumentGeneration.ExcelDocumentFactory()
                    sPfad = e.Item.Cells(2).Text()
                    fname = Right(sPfad, sPfad.Length - sPfad.LastIndexOf("\") - 1)

                    ExcelFactory.ReturnExcelTab(sPfad, fname, Me)
                ElseIf e.Item.Cells(3).Text = "csv" Then
                    Dim ExcelFactory As New DocumentGeneration.ExcelDocumentFactory()
                    sPfad = e.Item.Cells(2).Text()
                    fname = Right(sPfad, sPfad.Length - sPfad.LastIndexOf("\") - 1)

                    ExcelFactory.CSVGetExcelTab(sPfad, fname, Me)
                ElseIf e.Item.Cells(3).Text = "doc" Then
                    Dim dt As New DataTable
                    Dim imageHashTable As New Hashtable
                    Dim WordFactory As New DocumentGeneration.WordDocumentFactory(dt, imageHashTable)
                    sPfad = e.Item.Cells(2).Text()
                    fname = Right(sPfad, sPfad.Length - sPfad.LastIndexOf("\") - 1)
                    WordFactory.Returndoc(sPfad, fname, Me)
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
                    Literal1.Text &= "                          window.open(""Report01_2.aspx?AppID=" & Session("AppID").ToString & """, ""_blank"", ""left=0,top=0,resizable=YES,scrollbars=YES"");" & vbCrLf
                    Literal1.Text &= "						  //-->" & vbCrLf
                    Literal1.Text &= "						</script>" & vbCrLf
                ElseIf e.Item.Cells(3).Text = "zip" Then
                    sPfad = e.Item.Cells(2).Text()
                    Session("App_Filepath") = sPfad
                    Session("App_ContentType") = "application/x-zip-compressed"

                    Literal1.Text = "						<script language=""Javascript"">" & vbCrLf
                    Literal1.Text &= "						  <!-- //" & vbCrLf
                    Literal1.Text &= "                          window.open(""Report01_2.aspx?AppID=" & Session("AppID").ToString & """, ""_blank"", ""left=0,top=0,resizable=YES,scrollbars=YES"");" & vbCrLf
                    Literal1.Text &= "						  //-->" & vbCrLf
                    Literal1.Text &= "						</script>" & vbCrLf
                End If
            Catch ex As Exception
                lblError.Text = "Fehler beim Erstellen der Excel-Datei: " + ex.Message
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
        Dim files6 As String()
        Dim files7 As String()
        Dim info As FileInfo
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


            files = Directory.GetFiles(fileSourcePath, "*.pdf")
            files2 = Directory.GetFiles(fileSourcePath, "*.xls")
            files3 = Directory.GetFiles(fileSourcePath, "*.doc")
            files4 = Directory.GetFiles(fileSourcePath, "*.jpg")
            files5 = Directory.GetFiles(fileSourcePath, "*.gif")
            files6 = Directory.GetFiles(fileSourcePath, "*.csv")
            files7 = Directory.GetFiles(fileSourcePath, "*.zip")


            For i = 0 To files.Length - 1
                info = New FileInfo(files.GetValue(i).ToString)
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
                info = New FileInfo(files2.GetValue(i).ToString)
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
                info = New FileInfo(files3.GetValue(i).ToString)
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
                info = New FileInfo(files4.GetValue(i).ToString)
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
                info = New FileInfo(files5.GetValue(i).ToString)
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

            For i = 0 To files6.Length - 1
                info = New FileInfo(files6.GetValue(i).ToString)
                fname = files6.GetValue(i).ToString
                fname1 = Right(fname, fname.Length - fname.LastIndexOf("\") - 1)
                row = table.NewRow()
                row("Serverpfad") = fileSourcePath & fname1
                fname1 = Left(fname1, fname1.LastIndexOf("."))
                row("Filename") = fname1
                row("Filedate") = info.CreationTime
                row("Pattern") = "csv"
                table.Rows.Add(row)
            Next

            For i = 0 To files7.Length - 1
                info = New FileInfo(files7.GetValue(i).ToString)
                fname = files7.GetValue(i).ToString
                fname1 = Right(fname, fname.Length - fname.LastIndexOf("\") - 1)
                row = table.NewRow()
                row("Serverpfad") = fileSourcePath & fname1
                fname1 = Left(fname1, fname1.LastIndexOf("."))
                row("Filename") = fname1
                row("Filedate") = info.CreationTime
                row("Pattern") = "zip"
                table.Rows.Add(row)
            Next

            Fillgrid(0, table)
        Catch ex As Exception
            lblNoData.Visible = True
            lblNoData.Text = "Keine Dokumente zur Anzeige gefunden."
        End Try
    End Sub
End Class