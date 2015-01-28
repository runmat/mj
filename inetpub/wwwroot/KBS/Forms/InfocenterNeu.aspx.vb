Imports System.IO
Imports KBS.KBS_BASE
Imports Telerik.Web.UI

Partial Public Class InfocenterNeu
    Inherits Page

    Private mObjKasse As Kasse
    Dim fileSourcePath As String
    Dim icDocs As InfoCenterData

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

        fileSourcePath = ConfigurationManager.AppSettings("DownloadPathInfocenter")
        Literal1.Text = ""

        If Not IsPostBack Then
            FillDocumentTypes()
            FillDocuments()
        ElseIf Session("objInfoCenter") IsNot Nothing Then
            icDocs = CType(Session("objInfoCenter"), InfoCenterData)
        End If
    End Sub

    Private Sub FillDocumentTypes()
        Try
            If icDocs Is Nothing Then
                icDocs = New InfoCenterData()
            End If

            icDocs.GetDocumentTypes()
            Session("objInfoCenter") = icDocs

        Catch ex As Exception
            data.Visible = False
            lblError.Text = "Fehler beim Lesen der Dokumenttypen: " & ex.Message
        End Try
    End Sub

    Private Sub FillDocuments()
        Try
            If icDocs Is Nothing Then
                icDocs = New InfoCenterData()
            End If

            icDocs.GetDocuments(mObjKasse.CustomerID)
            Session("objInfoCenter") = icDocs
            'Befüllen des Grids läuft über das "NeedDataSource"-Event

            If icDocs.Documents IsNot Nothing AndAlso icDocs.Documents.Rows.Count > 0 Then
                rgDokumente.Rebind()
            Else
                lblError.Text = "Keine Dokumente zur Anzeige gefunden."
            End If

        Catch ex As Exception
            data.Visible = False
            lblError.Text = "Fehler beim Lesen der Dokumente: " & ex.Message
        End Try
    End Sub

    Protected Sub lb_zurueck_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_zurueck.Click
        If Session("objInfoCenter") IsNot Nothing Then
            Session.Remove("objInfoCenter")
        End If
        Response.Redirect("../Selection.aspx")
    End Sub

#Region "Grid"

    Protected Sub rgDokumente_NeedDataSource(ByVal sender As Object, ByVal e As GridNeedDataSourceEventArgs) Handles rgDokumente.NeedDataSource
        If icDocs IsNot Nothing Then
            rgDokumente.DataSource = icDocs.Documents.DefaultView
        End If
    End Sub

    Protected Sub rgDokumente_ItemDataBound(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles rgDokumente.ItemDataBound
        'Gruppenheader ohne Überschriften
        If TypeOf e.Item Is GridGroupHeaderItem Then
            Dim item As GridGroupHeaderItem = CType(e.Item, GridGroupHeaderItem)
            Dim groupDataRow As DataRowView = CType(e.Item.DataItem, DataRowView)
            item.DataCell.Text = groupDataRow("docTypeName").ToString()
        End If
    End Sub

    Protected Sub rgDokumente_ItemCommand(ByVal sender As Object, ByVal e As GridCommandEventArgs) Handles rgDokumente.ItemCommand
        Select Case e.CommandName
            Case "showDocument"
                Dim index As Integer = e.Item.ItemIndex
                Dim gridRow As GridDataItem = rgDokumente.Items(index)
                Dim lButton As LinkButton = CType(e.Item.FindControl("lbtDateiOeffnen"), LinkButton)

                If lButton IsNot Nothing Then
                    Dim fName As String = lButton.Text
                    Dim fType As String = gridRow("FileType").Text
                    Dim sPfad As String = fileSourcePath & fName & "." & fType

                    If File.Exists(sPfad) Then
                        Session("App_Filepath") = sPfad

                        Select Case fType
                            Case "pdf"
                                Session("App_ContentType") = "Application/pdf"
                                Session("App_ContentDisposition") = "inline"

                            Case "xls", "xlsx"
                                Session("App_ContentType") = "Application/vnd.ms-excel"
                                Session("App_ContentDisposition") = "attachment"

                            Case "doc", "docx"
                                Session("App_ContentType") = "Application/msword"
                                Session("App_ContentDisposition") = "attachment"

                            Case "jpg", "jpeg"
                                Session("App_ContentType") = "image/jpeg"
                                Session("App_ContentDisposition") = "inline"

                            Case "gif"
                                Session("App_ContentType") = "image/gif"
                                Session("App_ContentDisposition") = "inline"

                        End Select

                        Literal1.Text = "						<script language=""Javascript"">" & Environment.NewLine
                        Literal1.Text &= "						  <!-- //" & Environment.NewLine
                        Literal1.Text &= "                          window.open(""DownloadFile.aspx"", ""_blank"", ""left=0,top=0,resizable=YES,scrollbars=YES"");" & Environment.NewLine
                        Literal1.Text &= "						  //-->" & Environment.NewLine
                        Literal1.Text &= "						</script>" & Environment.NewLine
                    Else
                        lblError.Text = "Die angeforderte Datei wurde nicht auf dem Server gefunden"
                    End If
                End If
        End Select
    End Sub

#End Region

End Class