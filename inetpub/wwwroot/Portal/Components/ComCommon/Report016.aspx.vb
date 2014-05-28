Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports System.IO

Partial Public Class Report016
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
            FillDocumentTypes()
            FillDocuments()
        ElseIf Session("objInfoCenter") IsNot Nothing Then
            icDocs = CType(Session("objInfoCenter"), InfoCenterData)
        End If

    End Sub

    Private Sub FillDocuments()

        Try
            If icDocs Is Nothing Then
                Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
                icDocs = New InfoCenterData(m_User, m_App, Session("AppID").ToString, Session.SessionID, strFileName)
            End If

            icDocs.GetDocuments(m_User.GroupID)

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

    Private Sub FillDocumentTypes()

        Try
            If icDocs Is Nothing Then
                Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
                icDocs = New InfoCenterData(m_User, m_App, Session("AppID").ToString, Session.SessionID, strFileName)
            End If

            icDocs.GetDocumentTypes()
            Session("objInfoCenter") = icDocs

        Catch ex As Exception
            Data.Visible = False
            lblError.Text = "Fehler beim Lesen der Dokumenttypen: " & ex.Message
        End Try

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

        End Select

    End Sub

#End Region

End Class