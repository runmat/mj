Option Explicit On
Option Strict On

Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common


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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GridNavigation2.setGridElment(DBDG)
        m_User = GetUser(Me)
        m_App = New Security.App(m_User) 'erzeugt ein App_objekt 
        AdminAuth(Me, m_User, Security.AdminLevel.Master)

        If Not IsPostBack Then
            If Refferer Is Nothing Then
                If Not Me.Request.UrlReferrer Is Nothing Then
                    Refferer = Me.Request.UrlReferrer.ToString
                Else
                    Refferer = ""
                End If
            End If

            GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

            mObjBapiOverView = New BapiOverViewClass(m_User.IsTestUser)
            Session.Add("mObjBapiOverViewSession", mObjBapiOverView)
        End If


        If mObjBapiOverView Is Nothing Then
            If Session("mObjBapiOverViewSession") Is Nothing Then
                Throw New Exception("Benötigtes Session Objekt nicht vorhanden")
            Else
                mObjBapiOverView = CType(Session("mObjBapiOverViewSession"), BapiOverViewClass)
            End If
        End If
        If Not IsPostBack Then

            FillDBGrid(0)

        End If

    End Sub
    

    Private Sub FillDBGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
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


            Dim intTempPageIndex As Int32 = intPageIndex
            Dim strTempSort As String = ""
            Dim strDirection As String = ""

            If strSort.Trim(" "c).Length > 0 Then
                intTempPageIndex = 0
                strTempSort = strSort.Trim(" "c)
                If (ViewState(DBDG.ID & "Sort") Is Nothing) OrElse (ViewState(DBDG.ID & "Sort").ToString = strTempSort) Then
                    If ViewState(DBDG.ID & "Direction") Is Nothing Then
                        strDirection = "desc"
                    Else
                        strDirection = ViewState(DBDG.ID & "Direction").ToString
                    End If
                Else
                    strDirection = "desc"
                End If

                If strDirection = "asc" Then
                    strDirection = "desc"
                Else
                    strDirection = "asc"
                End If

                ViewState(DBDG.ID & "Sort") = strTempSort
                ViewState(DBDG.ID & "Direction") = strDirection
            Else
                If Not ViewState(DBDG.ID & "Sort") Is Nothing Then
                    strTempSort = ViewState(DBDG.ID & "Sort").ToString
                    If ViewState(DBDG.ID & "Direction") Is Nothing Then
                        strDirection = "asc"
                        ViewState(DBDG.ID & "Direction") = strDirection
                    Else
                        strDirection = ViewState(DBDG.ID & "Direction").ToString
                    End If
                End If
            End If

            If Not strTempSort.Length = 0 Then
                tmpDataView.Sort = strTempSort & " " & strDirection
            End If

            lblDBInfo.Text = "Anzahl: " & tmpDataView.Count

            DBDG.PageIndex = intTempPageIndex

            DBDG.DataSource = tmpDataView

            DBDG.DataBind()

        End If
    End Sub


    Private Sub responseBack()
        If Refferer = "" Then
            Dim strLinkPrefix As String = "/" & ConfigurationManager.AppSettings("WebAppPath") & "/"
            Response.Redirect(strLinkPrefix & "Start/Selection.aspx")
        Else
            Response.Redirect(Refferer)
        End If
    End Sub

    Private Sub DBDG_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DBDG.RowCommand
        If e.CommandName = "Delete" Then
            mObjBapiOverView.deleteBapi(e.CommandArgument.ToString)
            mObjBapiOverView.fillDBProxys()
            FillDBGrid(0)
        End If

        If e.CommandName = "Details" Then
            Dim Parameterlist As String = ""
            CKG.Base.Business.HelpProcedures.getAppParameters(Session("AppID").ToString, Parameterlist, ConfigurationManager.AppSettings.Get("Connectionstring"))
            Response.Redirect("ShowBapis.aspx?AppID=" & Session("AppID").ToString & Parameterlist & "&BapiName=" & e.CommandArgument.ToString)
        End If
    End Sub

    Private Sub DBDG_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles DBDG.Sorting
        FillDBGrid(DBDG.PageIndex, e.SortExpression)
    End Sub

    Private Sub GridNavigation2_ddlPageSizeChanged() Handles GridNavigation2.PageSizeChanged
        FillDBGrid(0)
    End Sub
    Private Sub DBDG_PageIndexChanged(ByVal pageindex As Int32) Handles GridNavigation2.PagerChanged
        FillDBGrid(pageindex)
    End Sub

    Protected Sub imgbDBVisible_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbDBVisible.Click
        PanelDB.Visible = Not PanelDB.Visible
        If PanelDB.Visible = True Then
            imgbDBVisible.ImageUrl = "/PortalZLD/Images/minus.gif"
        Else
            imgbDBVisible.ImageUrl = "/PortalZLD/Images/Plus1.gif"
        End If
    End Sub

    Protected Sub lbCreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbCreate.Click
        mObjBapiOverView.DBFilter = "*"

        mObjBapiOverView.DBFilter = txtFilter.Text.Trim(" "c)

        FillDBGrid(0)
    End Sub
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub lb_zurueck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_zurueck.Click
        responseBack()
    End Sub

    Protected Sub DBDG_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DBDG.RowDeleting

    End Sub
End Class