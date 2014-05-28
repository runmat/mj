Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Business
Imports System.IO

Partial Public Class Report07
    Inherits System.Web.UI.Page

#Region "Declarations"
    Private m_App As Security.App
    Private m_User As Security.User
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User, True)
        GetAppIDFromQueryString(Me)

        Literal1.Text = ""

        If IsPostBack = False Then
            GetFiles()
        End If

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text

        m_App = New Security.App(m_User)
    End Sub

    Protected Sub cmdBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdBack.Click
        Response.Redirect("../../../Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub

    Private Sub grvFiles_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grvFiles.RowCommand

        If e.CommandName = "open" Then


            Dim index As Integer = Convert.ToInt32(e.CommandArgument)

            Dim row As GridViewRow = grvFiles.Rows(index)

            Dim btn As LinkButton

            btn = CType(row.FindControl("lbtFilename"), LinkButton)

            Literal1.Text = "						<script language=""Javascript"">" & vbCrLf
            Literal1.Text &= "						  <!-- //" & vbCrLf
            Literal1.Text &= "                          window.open(""Report07_1.aspx?AppID=" & Session("AppID").ToString & "&File=" & btn.Text & """, ""_blank"", ""left=0,top=0,resizable=YES,scrollbars=YES"");" & vbCrLf
            Literal1.Text &= "						  //-->" & vbCrLf
            Literal1.Text &= "						</script>" & vbCrLf

        End If

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

#End Region

#Region "Methods"
    Private Sub GetFiles()

        Dim row As DataRow
        Dim column As DataColumn
        Dim TempTable As DataTable
        Dim fileSourcePath As String

        Try

            fileSourcePath = CType(ConfigurationManager.AppSettings("DownloadPathGMAC"), String)

            TempTable = New DataTable()
            column = New DataColumn("FileName", System.Type.GetType("System.String"))
            TempTable.Columns.Add(column)
            column = New DataColumn("ChangeDate", System.Type.GetType("System.DateTime"))
            TempTable.Columns.Add(column)
            column = New DataColumn("Link", System.Type.GetType("System.String"))
            TempTable.Columns.Add(column)


            Dim DirInfo As New DirectoryInfo(fileSourcePath)
            Dim files() As FileInfo = DirInfo.GetFiles("*.xls")

            If files.Length < 1 Then Err.Raise(-1, , "Es wurden keine Dateien gefunden.")

            For Each file In files

                row = TempTable.NewRow()

                row("FileName") = file.Name
                row("Link") = file.Name
                row("ChangeDate") = file.LastWriteTime

                TempTable.Rows.Add(row)

            Next


            grvFiles.DataSource = TempTable.DefaultView
            grvFiles.DataBind()


        Catch ex As Exception
            grvFiles.Visible = False
            lblError.Visible = True
            lblError.Text = ex.Message
        End Try
    End Sub


#End Region

End Class

' ************************************************
' $History: Report07.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 9.09.09    Time: 12:47
' Updated in $/CKAG/Applications/AppF1/forms
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 28.05.09   Time: 14:46
' Updated in $/CKAG/Applications/AppF1/forms
' ITA: 2681
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 27.05.09   Time: 16:23
' Updated in $/CKAG/Applications/AppF1/forms
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 27.05.09   Time: 10:17
' Created in $/CKAG/Applications/AppF1/forms
' ITA: 2681
' 