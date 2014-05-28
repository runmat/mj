Option Explicit On
Option Strict On

Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common
Imports CKG.Services
Imports Telerik.Web.UI
Imports CKG.Base.Kernel.Security


Partial Public Class BapiOverView
    Inherits System.Web.UI.Page

    Private m_App As Security.App
    Private m_User As Security.User
    Private mObjBapiOverView As BapiOverViewClass

#Region "Properties"

    Private Property Refferer() As String
        Get
            If Not Session.Item(Me.Request.Url.LocalPath & "Refferer") Is Nothing Then
                Return Session.Item(Me.Request.Url.LocalPath & "Refferer").ToString()
            Else : Return Nothing
            End If
        End Get
        Set(ByVal value As String)
            Session.Item(Me.Request.Url.LocalPath & "Refferer") = value
        End Set
    End Property

#End Region

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)

        m_User = Common.GetUser(Me)
        Common.AdminAuth(Me, m_User, AdminLevel.Master)
        m_App = New App(m_User)
        Common.GetAppIDFromQueryString(Me)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

        lblError.Text = ""
        lblMessage.Text = ""

        If Not IsPostBack Then
            If String.IsNullOrEmpty(Refferer) AndAlso Not Me.Request.UrlReferrer Is Nothing Then
                Refferer = Me.Request.UrlReferrer.ToString
            End If

            'reloading page - remove old instance from session
            Session.Remove("mObjBapiOverViewSession")
        End If

        mObjBapiOverView = Common.GetOrCreateObject("mObjBapiOverViewSession", Function() New BapiOverViewClass(m_User.IsTestUser))

        If Not IsPostBack Then
            'dropDownListe füllen
            FillDBGrid()
        End If

        Common.TranslateTelerikColumns(DBDG)
    End Sub

    Protected Overrides Sub OnPreRender(e As EventArgs)
        MyBase.OnPreRender(e)

        Common.SetEndASPXAccess(Me)
    End Sub

    Protected Overrides Sub OnUnload(e As EventArgs)
        MyBase.OnUnload(e)

        Common.SetEndASPXAccess(Me)
    End Sub

    Private Sub FillDBGrid(Optional ByVal rebind As Boolean = True)
        If IsNothing(mObjBapiOverView.DBProxys) OrElse mObjBapiOverView.DBProxys.Rows.Count = 0 Then

            lblDBNoData.Text = "Keine Daten zur Anzeige gefunden."
            DBDG.Visible = False
            lblDBNoData.Visible = True
            lblDBInfo.Text = "Anzahl: 0"
        Else

            DBDG.Visible = True
            lblDBNoData.Visible = False


            Dim tmpDataView As New DataView(mObjBapiOverView.DBProxys)
            tmpDataView.RowStateFilter = DataViewRowState.ModifiedCurrent

            lblDBInfo.Text = "Anzahl: " & tmpDataView.Count

            DBDG.DataSource = tmpDataView
            If rebind Then DBDG.Rebind()
        End If
    End Sub

    Protected Sub responseBack(sender As Object, e As EventArgs)
        If Refferer = "" Then
            Dim strLinkPrefix As String = "/" & ConfigurationManager.AppSettings("WebAppPath") & "/"
            Response.Redirect(strLinkPrefix & "Start/Selection.aspx")
        Else
            Response.Redirect(Refferer)
        End If
    End Sub

    Protected Sub imgbDBVisible_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbDBVisible.Click
        PanelDB.Visible = Not PanelDB.Visible
        If PanelDB.Visible = True Then
            imgbDBVisible.ImageUrl = "../Images/minus.gif"
        Else
            imgbDBVisible.ImageUrl = "../Images/plus.gif"
        End If
    End Sub

    Protected Sub lbCreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbCreate.Click, btnEmpty.Click

        Dim filterValue = txtFilter.Text.Trim()
        If String.IsNullOrEmpty(filterValue) Then filterValue = "*"

        mObjBapiOverView.DBFilter = filterValue

        FillDBGrid()
    End Sub

    Protected Sub lbClearBapiStruktur_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbClearBapiStruktur.Click

        mObjBapiOverView.deleteAllBapis()
        mObjBapiOverView.fillDBProxys()
        FillDBGrid(False)

    End Sub

    Protected Sub DBDG_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs)
        FillDBGrid(False)
    End Sub

    Protected Sub DBDG_ItemCommand(sender As Object, e As GridCommandEventArgs)
        Select Case e.CommandName
            Case "Delete"
                mObjBapiOverView.deleteBapi(e.CommandArgument.ToString)
                mObjBapiOverView.fillDBProxys()
                FillDBGrid(False)
            Case "Details"
                Dim sbRows = m_User.Applications.Select("AppName='ShowBapis'")

                If sbRows.Count > 1 Then
                    sbRows = m_User.Applications.Select("AppName='ShowBapis' and AppFriendlyName like '%Services%'")
                End If

                If sbRows.Count = 0 Then Return

                Response.Redirect(String.Format("ShowBapis.aspx?AppID={0}&BapiName={1}", sbRows(0)("AppID"), e.CommandArgument.ToString))
        End Select
    End Sub

End Class
