Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports System.IO

Partial Public Class Report015
    Inherits System.Web.UI.Page

    Private m_User As Security.User
    Private m_App As Security.App
    Dim icDocs As InfoCenterData
    Dim fileSourcePath As String

    Protected WithEvents ucStyles As Portal.PageElements.Styles
    Protected WithEvents ucHeader As Portal.PageElements.Header

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)
        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text
        m_App = New Base.Kernel.Security.App(m_User)

        lblError.Text = ""

        fileSourcePath = CType(ConfigurationManager.AppSettings("DownloadPathSamba"), String) & m_User.KUNNR & "\"
        Literal1.Text = ""

        If Not IsPostBack Then
            FillGrouplist()
            FillDocumentTypes()
            FillDocuments()
        ElseIf Session("objInfoCenter") IsNot Nothing Then
            icDocs = CType(Session("objInfoCenter"), InfoCenterData)
        End If

    End Sub

    Private Sub FillGrouplist()
        Dim dtGroups As Admin.Kernel.GroupList

        lbxDocumentGroups.Items.Clear()

        Using cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            With cn
                .Open()
                dtGroups = New Admin.Kernel.GroupList(m_User.Customer.CustomerId, cn, m_User.Customer.AccountingArea)
                .Close()
            End With
        End Using

        If dtGroups Is Nothing OrElse dtGroups.Rows.Count < 1 Then
            Exit Sub
        End If

        For Each dr As DataRow In dtGroups.Rows
            lbxDocumentGroups.Items.Add(New ListItem(dr("GROUPNAME"), dr("GROUPID")))
        Next

    End Sub

    Private Sub FillDocumentTypes()

        Try
            If icDocs Is Nothing Then
                Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
                icDocs = New InfoCenterData(m_User, m_App, Session("AppID").ToString, Session.SessionID, strFileName)
            End If

            icDocs.GetDocumentTypes()
            Session("objInfoCenter") = icDocs

            UpdateDocumentTypeDropDowns()

        Catch ex As Exception
            Data.Visible = False
            lblError.Text = "Fehler beim Lesen der Dokumenttypen: " & ex.Message
        End Try

    End Sub

    Private Sub UpdateDocumentTypeDropDowns()

        Try
            If icDocs IsNot Nothing Then

                ddlDokumentart.Items.Clear()
                ddlDocTypeSelection.Items.Clear()
                For Each dr As DataRow In icDocs.DocumentTypes.Rows
                    ddlDokumentart.Items.Add(New ListItem(dr("doctypeName"), dr("documentTypeId")))
                    ddlDocTypeSelection.Items.Add(New ListItem(dr("doctypeName"), dr("documentTypeId")))
                Next

            End If

        Catch ex As Exception
            lblError.Text = "Fehler beim Füllen der Dokumenttyp-Auswahl: " & ex.Message
        End Try

    End Sub

    Private Sub FillDocuments()

        Try
            If icDocs Is Nothing Then
                Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
                icDocs = New InfoCenterData(m_User, m_App, Session("AppID").ToString, Session.SessionID, strFileName)
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
            Data.Visible = False
            lblError.Text = "Fehler beim Lesen der Dokumente: " & ex.Message
        End Try

    End Sub

    Protected Sub lbtnLoeschen_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnLoeschen.Click
        Dim chkSel As CheckBox
        Dim lButton As LinkButton
        Dim fName As String
        Dim fExtension As String
        Dim sPfad As String

        For Each gridRow As Telerik.Web.UI.GridDataItem In rgDokumente.Items
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
                    icDocs.DeleteDocument(CInt(gridRow("DocumentId").Text))
                End If
            End If
        Next

        Session("objInfoCenter") = icDocs
        rgDokumente.Rebind()

    End Sub


#Region " Grid "

    Protected Sub rgDokumente_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles rgDokumente.NeedDataSource
        If icDocs IsNot Nothing Then
            rgDokumente.DataSource = icDocs.Documents.DefaultView
        End If
    End Sub

    Protected Sub rgDokumente_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles rgDokumente.ItemDataBound
        If TypeOf e.Item Is Telerik.Web.UI.GridGroupHeaderItem Then

            If icDocs IsNot Nothing AndAlso icDocs.DocumentTypes IsNot Nothing Then
                Dim item As Telerik.Web.UI.GridGroupHeaderItem = CType(e.Item, Telerik.Web.UI.GridGroupHeaderItem)
                Dim strText As String = item.DataCell.Text.Split(":"c)(1)
                If IsNumeric(strText) Then
                    Dim docType As DataRow = icDocs.DocumentTypes.Select("documentTypeId=" & Integer.Parse(strText))(0)
                    item.DataCell.Text = docType("docTypeName")
                End If
            End If

        End If
    End Sub

    Private Sub rgDokumente_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles rgDokumente.ItemCommand
        Dim index As Integer
        Dim gridRow As Telerik.Web.UI.GridDataItem
        Dim lButton As LinkButton

        Select Case e.CommandName
            Case "showDocument"
                index = CType(e.Item.ItemIndex, Integer)
                gridRow = rgDokumente.Items(index)
                lButton = CType(e.Item.FindControl("lbtDateiOeffnen"), LinkButton)

                If lButton IsNot Nothing Then
                    Dim fName As String = lButton.Text
                    Dim fType As String = gridRow("FileType").Text
                    Dim fExtension As String = "." & fType
                    Dim sPfad As String = fileSourcePath & fName & "." & fType

                    If File.Exists(sPfad) Then

                        Session("App_Filepath") = sPfad

                        Select Case gridRow("FileType").Text
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

                        Literal1.Text = "						<script language=""Javascript"">" & vbCrLf
                        Literal1.Text &= "						  <!-- //" & vbCrLf
                        Literal1.Text &= "                          window.open(""Report014_1.aspx?AppID=" & Session("AppID").ToString & """, ""_blank"", ""left=0,top=0,resizable=YES,scrollbars=YES"");" & vbCrLf
                        Literal1.Text &= "						  //-->" & vbCrLf
                        Literal1.Text &= "						</script>" & vbCrLf

                    Else
                        lblError.Text = "Die angeforderte Datei wurde nicht auf dem Server gefunden"
                    End If
                End If

            Case "editDocument"
                index = CType(e.Item.ItemIndex, Integer)
                gridRow = rgDokumente.Items(index)
                lButton = CType(e.Item.FindControl("lbtDateiOeffnen"), LinkButton)

                If lButton IsNot Nothing Then
                    Dim fId As Integer = CInt(gridRow("DocumentId").Text)
                    Dim fName As String = lButton.Text
                    Dim fType As String = gridRow("FileType").Text
                    Dim fExtension As String = "." & fType

                    txtFileName.Text = fName & fExtension
                    ihSelectedDocumentId.Value = fId.ToString()

                    'Dokumentart in DropDown selektieren
                    ddlDokumentart.SelectedValue = gridRow("docTypeId").Text

                    lbxDocumentGroups.ClearSelection()

                    'Gruppenzuordnungen für das gewählte Dokument laden
                    icDocs.GetDocumentRights(fId)

                    'zugeordnete Gruppen in ListBox selektieren
                    For Each item As ListItem In lbxDocumentGroups.Items
                        If icDocs.DocumentRights.Select("DocumentId=" & fId & " AND GroupId=" & item.Value).Length > 0 Then
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

        For Each gridRow As Telerik.Web.UI.GridDataItem In rgDokumente.Items
            chkSel = CType(gridRow("colLoeschen").FindControl("rb_sel"), CheckBox)
            If chkSel IsNot Nothing Then
                chkSel.Checked = bChecked
            End If
        Next
    End Sub

#End Region

#Region " Upload "

    Protected Sub lbtnUpload_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnUpload.Click

        If Not String.IsNullOrEmpty(upFile.PostedFile.FileName) Then

            If Not Directory.Exists(fileSourcePath) Then
                lblError.Text = "Dokumentenverzeichnis nicht gefunden. Bitte wenden Sie sich an Ihren System-Administrator."
                Exit Sub
            End If

            Dim fileInfo As IO.FileInfo = New IO.FileInfo(upFile.PostedFile.FileName)
            Dim fExtension As String = fileInfo.Extension
            Dim fType As String = fExtension.ToLower().Trim("."c)
            Dim fName As String = fileInfo.Name.Replace(fExtension, "")
            fName = Right(fName, fName.Length - fName.LastIndexOf("\") - 1).ToString

            Select Case fType
                Case "jpg", "jpeg", "pdf", "doc", "docx", "xls", "xlsx", "gif"
                Case Else
                    lblError.Text = "Es können nur Dateien im Format jpg, jpeg, gif, doc, docx, xls, xlsx und pdf hochgeladen werden."
                    Exit Sub
            End Select

            'Datei in temp-Verzeichnis zwischenspeichern
            Dim tempVerzeichnis As String = ConfigurationManager.AppSettings("UploadPathInfocenterLocal")
            If String.IsNullOrEmpty(tempVerzeichnis) Then
                tempVerzeichnis = "c:\inetpub\wwwroot\portal\temp\infocenter\"
            End If
            If Not Directory.Exists(tempVerzeichnis) Then
                lblError.Text = "Upload-Verzeichnis nicht auf dem Server vorhanden. Bitte wenden Sie sich an Ihren Systemadministrator."
                Exit Sub
            End If
            If Not Directory.Exists(tempVerzeichnis & m_User.KUNNR) Then
                Directory.CreateDirectory(tempVerzeichnis & m_User.KUNNR)
            End If
            upFile.SaveAs(tempVerzeichnis & m_User.KUNNR & "\" & fName & fExtension)

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
                tempVerzeichnis = "c:\inetpub\wwwroot\portal\temp\infocenter\"
            End If

            If File.Exists(tempVerzeichnis & m_User.KUNNR & "\" & icDocs.UploadFile) Then
                File.Delete(tempVerzeichnis & m_User.KUNNR & "\" & icDocs.UploadFile)
            End If
            icDocs.UploadFile = ""
            Session("objInfoCenter") = icDocs
        End If
    End Sub

    Private Sub SaveUploadedFile(Optional ByVal blnDateiErsetzen As Boolean = False)

        If Not String.IsNullOrEmpty(icDocs.UploadFile) Then
            Dim tempVerzeichnis As String = ConfigurationManager.AppSettings("UploadPathInfocenterLocal")
            If String.IsNullOrEmpty(tempVerzeichnis) Then
                tempVerzeichnis = "c:\inetpub\wwwroot\portal\temp\infocenter\"
            End If

            Dim fileInfo As IO.FileInfo = New IO.FileInfo(tempVerzeichnis & m_User.KUNNR & "\" & icDocs.UploadFile)
            Dim fExtension As String = fileInfo.Extension
            Dim fType As String = fExtension.ToLower().Trim("."c)
            Dim fLastEdited As DateTime = fileInfo.LastWriteTime
            Dim fSize As Long = fileInfo.Length
            Dim fName As String = fileInfo.Name.Replace(fExtension, "")
            fName = Right(fName, fName.Length - fName.LastIndexOf("\") - 1).ToString

            'ggf. Datei umbenennen, um vorhandene Datei nicht zu überschreiben
            If File.Exists(fileSourcePath & fName & fExtension) AndAlso Not blnDateiErsetzen Then
                Dim blnVorhanden As Boolean = True
                Dim intSuffix As Integer = 0
                While (blnVorhanden)
                    intSuffix += 1
                    If Not File.Exists(fileSourcePath & fName & "_" & intSuffix.ToString() & fExtension) Then
                        blnVorhanden = False
                    End If
                End While
                fName = fName & "_" & intSuffix.ToString()
            End If

            File.Copy(tempVerzeichnis & m_User.KUNNR & "\" & icDocs.UploadFile, fileSourcePath & fName & fExtension, True)
            File.Delete(tempVerzeichnis & m_User.KUNNR & "\" & icDocs.UploadFile)

            icDocs.SaveDocument(-1, 1, fName, fType, fLastEdited, fSize)
            icDocs.UploadFile = ""
            Session("objInfoCenter") = icDocs

            rgDokumente.Rebind()
        End If

    End Sub

#End Region

#Region " Edit Document "

    Protected Sub lbtnSaveDocument_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnSaveDocument.Click

        Dim listeAdd As New Generic.List(Of Integer)
        Dim listeDelete As New Generic.List(Of Integer)
        Dim anzTreffer As Integer

        Dim documentId As Integer = CInt(ihSelectedDocumentId.Value)

        'Dokumententyp abgleichen und ggf. speichern
        Dim docTypeId As String = icDocs.Documents.Select("DocumentId=" & documentId)(0)("docTypeId")
        If Not ddlDokumentart.SelectedValue = docTypeId Then
            icDocs.SaveDocument(documentId, CInt(ddlDokumentart.SelectedValue))
        End If

        'Gruppenzuordnung abgleichen und ggf. speichern
        For Each item As ListItem In lbxDocumentGroups.Items
            anzTreffer = icDocs.DocumentRights.Select("DocumentId=" & documentId & " AND GroupId=" & item.Value).Length
            If item.Selected AndAlso anzTreffer = 0 Then
                listeAdd.Add(item.Value)
            ElseIf Not item.Selected AndAlso anzTreffer > 0 Then
                listeDelete.Add(item.Value)
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

#Region " Edit DocType "

    Protected Sub ddlDocTypeSelection_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDocTypeSelection.SelectedIndexChanged

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
        tdSaveNewDocType.Visible = False
        trEditDocType.Visible = True
        tdAddNewDocType.Visible = True

    End Sub

    Protected Sub lbtnSaveDocType_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnSaveDocType.Click

        If ddlDocTypeSelection.SelectedIndex >= 0 Then
            'Dokumententypbezeichnung abgleichen und ggf. speichern
            If Not ddlDocTypeSelection.SelectedItem.Text = txtDocTypeName.Text Then
                icDocs.SaveDocumentType(CInt(ddlDocTypeSelection.SelectedItem.Value), txtDocTypeName.Text)
            End If
        End If

        Session("objInfoCenter") = icDocs
        UpdateDocumentTypeDropDowns()
        rgDokumente.Rebind()

    End Sub

    Protected Sub lbtnDeleteDocType_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnDeleteDocType.Click
        Dim blnSuccess As Boolean = False

        If ddlDocTypeSelection.SelectedIndex >= 0 Then
            If ddlDocTypeSelection.SelectedItem.Value = "1" Then
                lblError.Text = "Die Default-Dokumentenart kann nicht gelöscht werden."
            Else
                blnSuccess = icDocs.DeleteDocumentType(CInt(ddlDocTypeSelection.SelectedItem.Value))

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

        trEditDocType.Visible = False
        tdAddNewDocType.Visible = False
        trNewDocType.Visible = True
        tdSaveNewDocType.Visible = True

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
        tdSaveNewDocType.Visible = False
        trEditDocType.Visible = True
        tdAddNewDocType.Visible = True

    End Sub

    Protected Sub lbtnCancelEditDocType_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnCancelEditDocType.Click

        divEditDocument.Visible = False
        divEditDocTypes.Visible = False
        Result.Visible = True

    End Sub

#End Region

End Class