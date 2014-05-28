Imports CKG.Base.Kernel
Imports Telerik.Web.UI

Public Class Change04
    Inherits System.Web.UI.Page

    Dim m_User As Security.User
    Dim m_App As Security.App
    Dim m_KV As Klaerfaelle

    Protected Overrides Sub OnLoad(e As System.EventArgs)
        MyBase.OnLoad(e)

        m_User = CKG.Base.Kernel.Common.Common.GetUser(Me)
        CKG.Base.Kernel.Common.Common.FormAuth(Me, m_User)
        m_App = New Security.App(m_User)
        CKG.Base.Kernel.Common.Common.GetAppIDFromQueryString(Me)
        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

        m_KV = CKG.Base.Kernel.Common.Common.GetOrCreateObject("KV", Function() New Klaerfaelle(m_User, m_App, Session("AppID"), Session.SessionID))

        If Not IsPostBack Then
            Result.Visible = False
            m_KV.Result = Nothing
        End If
    End Sub

    Protected Overrides Sub OnPreRender(e As System.EventArgs)
        MyBase.OnPreRender(e)

        CKG.Base.Kernel.Common.Common.TranslateTelerikColumns(fzgGrid)
        CKG.Base.Kernel.Common.Common.SetEndASPXAccess(Me)
    End Sub

    Protected Overrides Sub OnUnload(e As System.EventArgs)
        MyBase.OnUnload(e)

        CKG.Base.Kernel.Common.Common.TranslateTelerikColumns(fzgGrid)
        CKG.Base.Kernel.Common.Common.SetEndASPXAccess(Me)
    End Sub

    Protected Sub runQuery(ByVal sender As Object, ByVal e As EventArgs)
        m_KV.LoadData(Me)
        LoadData()

        If Result.Visible Then
            lbQuery.Text = "» Neue Abfrage"
        End If
    End Sub

    Protected Sub GridNeedDataSource(ByVal sender As Object, ByVal e As GridNeedDataSourceEventArgs)
        LoadData(False)
    End Sub

    Private Sub LoadData(Optional ByVal rebind As Boolean = True)
        Dim view = New DataView(m_KV.Result, "", "KUNDE", DataViewRowState.CurrentRows)
        fzgGrid.DataSource = view
        If rebind Then fzgGrid.Rebind()

        If fzgGrid.Items.Count > 0 Then
            CKG.Base.Kernel.Common.Common.ResizeTelerikColumns(fzgGrid, view.ToTable())
            Result.Visible = True
        Else
            Result.Visible = False
        End If
    End Sub

    Protected Sub infoVersLoad(ByVal sender As Object, ByVal e As EventArgs)
        If sender Is Nothing OrElse Not TypeOf sender Is DropDownList Then Return

        Dim infoVers = CType(sender, DropDownList)

        infoVers.DataSource = m_KV.Versicherungsinfos.DefaultView
    End Sub

    Protected Sub infoVersChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim box As DropDownList = CType(sender, DropDownList)
        Dim item As GridDataItem = CType(box.NamingContainer, GridDataItem)

        Dim kunde = item("KUNDE").Text
        Dim sernr = item("SERNR").Text

        Dim row = m_KV.Result.Select(String.Format("KUNDE='{0}' and SERNR='{1}'", kunde, sernr)).FirstOrDefault
        If Not row Is Nothing Then row("INFO_VERS") = box.SelectedValue
        
        LoadData()

        lbSave.Enabled = Not m_KV.Result Is Nothing AndAlso m_KV.Result.Select("INFO_VERS <> ''").Length > 0
    End Sub

    Protected Sub save(ByVal sender As Object, ByVal e As EventArgs)
        Try
            m_KV.Save(Me)

            If (m_KV.Status <> 0) Then
                lblError.Text = m_KV.Message
                lblError.Visible = True
            End If

            m_KV.LoadData(Me)
            LoadData()
        Catch ex As Exception
            lblError.Text = ex.Message
            lblError.Visible = True

            m_KV.LoadData(Me)
            LoadData()
        End Try

        lbSave.Enabled = Not m_KV.Result Is Nothing AndAlso m_KV.Result.Select("INFO_VERS <> ''").Length > 0
    End Sub

    'Protected Sub navigateBack(ByVal sender As Object, ByVal e As EventArgs)
    '    Session.Remove("KV")
    '    Response.Redirect("../../../Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    'End Sub

    Protected Sub lbBack_Click(sender As Object, e As EventArgs) Handles lbBack.Click
        Session.Remove("KV")
        Response.Redirect("../../../Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub
End Class