Imports System.IO
Imports KBS.KBS_BASE
Imports Telerik.Web.UI

Partial Public Class InfocenterNeuAdmin
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
            FillCustomerlist()
            FillDocumentTypes()
            FillDocuments()
        ElseIf Session("objInfoCenter") IsNot Nothing Then
            icDocs = CType(Session("objInfoCenter"), InfoCenterData)
        End If
    End Sub

    Private Sub FillCustomerlist()
        Try
            If icDocs Is Nothing Then
                icDocs = New InfoCenterData()
            End If

            icDocs.GetCustomerList()
            Session("objInfoCenter") = icDocs

            If icDocs.Customer Is Nothing Then
                Exit Sub
            End If

            For Each cust As DataRow In icDocs.Customer.Rows
                lbxDocumentCustomer.Items.Add(New ListItem(cust("Customername").ToString(), cust("CustomerID").ToString()))
            Next

        Catch ex As Exception
            data.Visible = False
            lblError.Text = "Fehler beim Lesen der Kunden: " & ex.Message
        End Try
    End Sub

    Private Sub FillDocumentTypes()
        Try
            If icDocs Is Nothing Then
                icDocs = New InfoCenterData()
            End If

            icDocs.GetDocumentTypes()
            Session("objInfoCenter") = icDocs

            UpdateDocumentTypeDropDowns()

        Catch ex As Exception
            data.Visible = False
            lblError.Text = "Fehler beim Lesen der Dokumenttypen: " & ex.Message
        End Try
    End Sub

    Private Sub UpdateDocumentTypeDropDowns()
        Try
            If icDocs IsNot Nothing Then
                ddlDokumentart.Items.Clear()
                ddlDocTypeSelection.Items.Clear()
                For Each dr As DataRow In icDocs.DocumentTypes.Rows
                    ddlDokumentart.Items.Add(New ListItem(dr("doctypeName").ToString(), dr("documentTypeId").ToString()))
                    ddlDocTypeSelection.Items.Add(New ListItem(dr("doctypeName").ToString(), dr("documentTypeId").ToString()))
                Next
            End If

        Catch ex As Exception
            lblError.Text = "Fehler beim Füllen der Dokumenttyp-Auswahl: " & ex.Message
        End Try
    End Sub

    Private Sub FillDocuments()
        Try
            If icDocs Is Nothing Then
                icDocs = New InfoCenterData()
            End If

            icDocs.GetDocuments()
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

    Protected Sub lbtnLoeschen_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnLoeschen.Click
        Dim chkSel As CheckBox
        Dim lButton As LinkButton
        Dim fName As String
        Dim fExtension As String
        Dim sPfad As String

        For Each gridRow As GridDataItem In rgDokumente.Items
            chkSel = CType(gridRow.FindControl("rb_sel"), CheckBox)
            If chkSel IsNot Nothing AndAlso chkSel.Checked Then
                lButton = CType(gridRow.FindControl("lbtDateiOeffnen"), LinkButton)
                If lButton IsNot Nothing Then
                    fName = lButton.Text
                    fExtension = "." & gridRow("FileType").Text
                    sPfad = fileSourcePath & fName & fExtension

                    If File.Exists(sPfad) Then
                        File.Delete(sPfad)
                    End If
                    icDocs.DeleteDocument(Int32.Parse(gridRow("DocumentId").Text))
                End If
            End If
        Next

        Session("objInfoCenter") = icDocs
        rgDokumente.Rebind()
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
        Dim index As Integer
        Dim gridRow As GridDataItem
        Dim lButton As LinkButton

        Select Case e.CommandName
            Case "showDocument"
                index = e.Item.ItemIndex
                gridRow = rgDokumente.Items(index)
                lButton = CType(e.Item.FindControl("lbtDateiOeffnen"), LinkButton)

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

            Case "editDocument"
                index = e.Item.ItemIndex
                gridRow = rgDokumente.Items(index)
                lButton = CType(e.Item.FindControl("lbtDateiOeffnen"), LinkButton)

                If lButton IsNot Nothing Then
                    Dim fId As Integer = Int32.Parse(gridRow("DocumentId").Text)
                    Dim fName As String = lButton.Text
                    Dim fType As String = gridRow("FileType").Text
                    Dim fExtension As String = "." & fType

                    txtFileName.Text = fName & fExtension
                    ihSelectedDocumentId.Value = fId.ToString()

                    'Dokumentart in DropDown selektieren
                    ddlDokumentart.SelectedValue = gridRow("docTypeId").Text

                    lbxDocumentCustomer.ClearSelection()

                    'Kundenzuordnungen für das gewählte Dokument laden
                    icDocs.GetDocumentRights(fId)

                    'zugeordnete Kunden in ListBox selektieren
                    For Each item As ListItem In lbxDocumentCustomer.Items
                        If icDocs.DocumentRights.Select("DocumentId=" & fId & " AND CustomerId=" & item.Value).Length > 0 Then
                            item.Selected = True
                        End If
                    Next

                    Result.Visible = False
                    divEditDocTypes.Visible = False
                    divEditDocument.Visible = True
                End If

        End Select
    End Sub

    Protected Sub ckb_SelectAll_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim chkSel As CheckBox
        Dim bChecked As Boolean = CType(sender, CheckBox).Checked

        For Each gridRow As GridDataItem In rgDokumente.Items
            chkSel = CType(gridRow("colLoeschen").FindControl("rb_sel"), CheckBox)
            If chkSel IsNot Nothing Then
                chkSel.Checked = bChecked
            End If
        Next
    End Sub

#End Region

#Region "Upload"

    Protected Sub lbtnUpload_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnUpload.Click
        If Not String.IsNullOrEmpty(upFile.PostedFile.FileName) Then
            If Not Directory.Exists(fileSourcePath) Then
                Try
                    Directory.CreateDirectory(fileSourcePath)
                Catch ex As Exception
                    lblError.Text = "Dokumentenverzeichnis konnte nicht angelegt werden. Bitte wenden Sie sich an Ihren System-Administrator."
                    Exit Sub
                End Try
            End If

            Dim fileInfo As FileInfo = New FileInfo(upFile.PostedFile.FileName)
            Dim fExtension As String = fileInfo.Extension
            Dim fType As String = fExtension.ToLower().Trim("."c)
            Dim fName As String = fileInfo.Name.Replace(fExtension, "")
            fName = fName.Substring(0, fName.Length - fName.LastIndexOf("\") - 1)

            Select Case fType
                Case "jpg", "jpeg", "pdf", "doc", "docx", "xls", "xlsx", "gif"

                Case Else
                    lblError.Text = "Es können nur Dateien im Format jpg, jpeg, gif, doc, docx, xls, xlsx und pdf hochgeladen werden."
                    Exit Sub

            End Select

            'Datei in temp-Verzeichnis zwischenspeichern
            Dim tempVerzeichnis As String = ConfigurationManager.AppSettings("UploadPathInfocenterLocal")
            If String.IsNullOrEmpty(tempVerzeichnis) Then
                tempVerzeichnis = "c:\inetpub\wwwroot\kbs\temp\infocenter\"
            End If
            If Not Directory.Exists(tempVerzeichnis) Then
                lblError.Text = "Upload-Verzeichnis nicht auf dem Server vorhanden. Bitte wenden Sie sich an Ihren Systemadministrator."
                Exit Sub
            End If
            upFile.SaveAs(tempVerzeichnis & fName & fExtension)

            'Dateiname in Session merken
            icDocs.UploadFile = fName & fExtension
            Session("objInfoCenter") = icDocs

            If File.Exists(fileSourcePath & fName & fExtension) Then
                rblPopupOptions.SelectedValue = "Beibehalten"
                ModalPopupExtender1.Show()
            Else
                SaveUploadedFile()
            End If
        Else
            lblError.Text = "Keine Datei ausgewählt."
        End If
    End Sub

    Protected Sub lbtnPopupOK_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnPopupOK.Click
        Select Case rblPopupOptions.SelectedValue
            Case "Beibehalten"
                SaveUploadedFile()

            Case "Ersetzen"
                SaveUploadedFile(True)

        End Select
    End Sub

    Protected Sub lbtnPopupCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnPopupCancel.Click
        If Not String.IsNullOrEmpty(icDocs.UploadFile) Then
            Dim tempVerzeichnis As String = ConfigurationManager.AppSettings("UploadPathInfocenterLocal")
            If String.IsNullOrEmpty(tempVerzeichnis) Then
                tempVerzeichnis = "c:\inetpub\wwwroot\kbs\temp\infocenter\"
            End If
            If File.Exists(tempVerzeichnis & icDocs.UploadFile) Then
                File.Delete(tempVerzeichnis & icDocs.UploadFile)
            End If
            icDocs.UploadFile = ""
            Session("objInfoCenter") = icDocs
        End If
    End Sub

    Private Sub SaveUploadedFile(Optional blnDateiErsetzen As Boolean = False)
        If Not String.IsNullOrEmpty(icDocs.UploadFile) Then
            Dim tempVerzeichnis As String = ConfigurationManager.AppSettings("UploadPathInfocenterLocal")
            If String.IsNullOrEmpty(tempVerzeichnis) Then
                tempVerzeichnis = "c:\inetpub\wwwroot\kbs\temp\infocenter\"
            End If

            Dim fileInfo As FileInfo = New FileInfo(tempVerzeichnis & icDocs.UploadFile)
            Dim fExtension As String = fileInfo.Extension
            Dim fType As String = fExtension.ToLower().Trim("."c)
            Dim fLastEdited As DateTime = fileInfo.LastWriteTime
            Dim fSize As Long = fileInfo.Length
            Dim fName As String = fileInfo.Name.Replace(fExtension, "")
            fName = fName.Substring(0, fName.Length - fName.LastIndexOf("\") - 1)

            'ggf. Datei umbenennen, um vorhandene Datei nicht zu überschreiben
            If File.Exists(fileSourcePath & fName & fExtension) AndAlso Not blnDateiErsetzen Then
                Dim blnVorhanden As Boolean = True
                Dim intSuffix As Integer = 0
                While blnVorhanden
                    intSuffix += 1
                    If Not File.Exists(fileSourcePath & fName & "_" & intSuffix.ToString() & fExtension) Then
                        blnVorhanden = False
                    End If
                End While
                fName = fName & "_" & intSuffix.ToString()
            End If

            File.Copy(tempVerzeichnis & icDocs.UploadFile, fileSourcePath & fName & fExtension, True)
            File.Delete(tempVerzeichnis & icDocs.UploadFile)

            icDocs.SaveDocument(-1, 1, fName, fType, fLastEdited, fSize)
            icDocs.UploadFile = ""
            Session("objInfoCenter") = icDocs

            rgDokumente.Rebind()
        End If
    End Sub

#End Region

#Region "Edit Document"

    Protected Sub lbtnSaveDocument_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnSaveDocument.Click
        Dim listeAdd As New List(Of Integer)
        Dim listeDelete As New List(Of Integer)
        Dim anzTreffer As Integer

        Dim documentId As Integer = Int32.Parse(ihSelectedDocumentId.Value)

        'Dokumententyp abgleichen und ggf. speichern
        Dim docTypeId As String = icDocs.Documents.Select("DocumentId=" & documentId)(0)("docTypeId").ToString()
        If Not ddlDokumentart.SelectedValue = docTypeId Then
            icDocs.SaveDocument(documentId, Int32.Parse(ddlDokumentart.SelectedValue))
        End If

        'Kundenzuordnung abgleichen und ggf. speichern
        For Each item As ListItem In lbxDocumentCustomer.Items
            anzTreffer = icDocs.DocumentRights.Select("DocumentId=" & documentId & " AND CustomerId=" & item.Value).Length
            If item.Selected AndAlso anzTreffer = 0 Then
                listeAdd.Add(Int32.Parse(item.Value))
            ElseIf Not item.Selected AndAlso anzTreffer > 0 Then
                listeDelete.Add(Int32.Parse(item.Value))
            End If
        Next

        If listeAdd.Count > 0 Then
            icDocs.SaveDocumentRights(documentId, listeAdd)
        End If
        If listeDelete.Count > 0 Then
            icDocs.DeleteDocumentRights(documentId, listeDelete)
        End If

        divEditDocument.Visible = False
        divEditDocTypes.Visible = False
        Result.Visible = True

        Session("objInfoCenter") = icDocs
        rgDokumente.Rebind()
    End Sub

    Protected Sub lbtnCancelEditDocument_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnCancelEditDocument.Click
        divEditDocument.Visible = False
        divEditDocTypes.Visible = False
        Result.Visible = True
    End Sub

#End Region

#Region "Edit DocType"

    Protected Sub ddlDocTypeSelection_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlDocTypeSelection.SelectedIndexChanged
        If ddlDocTypeSelection.SelectedIndex >= 0 Then
            txtDocTypeName.Text = ddlDocTypeSelection.SelectedItem.Text
        Else
            txtDocTypeName.Text = ""
        End If
    End Sub

    Protected Sub lbtnEditDocTypes_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnEditDocTypes.Click
        If ddlDocTypeSelection.SelectedIndex >= 0 Then
            txtDocTypeName.Text = ddlDocTypeSelection.SelectedItem.Text
        Else
            txtDocTypeName.Text = ""
        End If

        Result.Visible = False
        divEditDocument.Visible = False
        divEditDocTypes.Visible = True
        trNewDocType.Visible = False
        lbtnSaveDocType.Visible = True
        lbtnSaveNewDocType.Visible = False
        lbtnDeleteDocType.Visible = True
        trEditDocType.Visible = True
        lbtnAddNewDocType.Visible = True
    End Sub

    Protected Sub lbtnSaveDocType_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnSaveDocType.Click
        If ddlDocTypeSelection.SelectedIndex >= 0 Then
            'Dokumententypbezeichnung abgleichen und ggf. speichern
            If Not ddlDocTypeSelection.SelectedItem.Text = txtDocTypeName.Text Then
                icDocs.SaveDocumentType(Int32.Parse(ddlDocTypeSelection.SelectedItem.Value), txtDocTypeName.Text)
            End If
        End If

        Session("objInfoCenter") = icDocs
        UpdateDocumentTypeDropDowns()
        rgDokumente.Rebind()
    End Sub

    Protected Sub lbtnDeleteDocType_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnDeleteDocType.Click
        If ddlDocTypeSelection.SelectedIndex >= 0 Then
            If ddlDocTypeSelection.SelectedItem.Value = "1" Then
                lblError.Text = "Die Default-Dokumentenart kann nicht gelöscht werden."
            Else
                Dim blnSuccess As Boolean = icDocs.DeleteDocumentType(Int32.Parse(ddlDocTypeSelection.SelectedItem.Value))

                If blnSuccess Then
                    Session("objInfoCenter") = icDocs
                    UpdateDocumentTypeDropDowns()
                    rgDokumente.Rebind()
                Else
                    lblError.Text = "Die Dokumentenart konnte nicht gelöscht werden, da sie noch von Dokumenten genutzt wird."
                End If
            End If
        End If
    End Sub

    Protected Sub lbtnAddNewDocType_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnAddNewDocType.Click
        txtNewDocType.Text = ""
        trEditDocType.Visible = False
        lbtnAddNewDocType.Visible = False
        trNewDocType.Visible = True
        lbtnSaveDocType.Visible = False
        lbtnSaveNewDocType.Visible = True
        lbtnDeleteDocType.Visible = False
    End Sub

    Protected Sub lbtnSaveNewDocType_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnSaveNewDocType.Click
        If Not String.IsNullOrEmpty(txtNewDocType.Text) Then
            icDocs.SaveDocumentType(-1, txtNewDocType.Text.Trim())
        Else
            lblError.Text = "Es wurde keine Bezeichnung für die neue Dokumentenart angegeben"
        End If

        Session("objInfoCenter") = icDocs
        UpdateDocumentTypeDropDowns()

        trNewDocType.Visible = False
        lbtnSaveDocType.Visible = True
        lbtnSaveNewDocType.Visible = False
        trEditDocType.Visible = True
        lbtnAddNewDocType.Visible = True
        lbtnDeleteDocType.Visible = True
    End Sub

    Protected Sub lbtnCancelEditDocType_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnCancelEditDocType.Click
        divEditDocument.Visible = False
        divEditDocTypes.Visible = False
        Result.Visible = True
    End Sub

#End Region

End Class