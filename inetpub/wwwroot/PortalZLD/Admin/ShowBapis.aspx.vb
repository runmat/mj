Option Explicit On
Option Strict On

Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG

Public Class ShowBapis
    Inherits System.Web.UI.Page

    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private mObjShowBapis As ShowBapisClass

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

#Region "Methods"


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Hier Benutzercode zur Seiteninitialisierung einfügen
        Try
            m_User = GetUser(Me)
            m_App = New Base.Kernel.Security.App(m_User) 'erzeugt ein App_objekt 
            FormAuth(Me, m_User)

            If Not IsPostBack Then
                If Not Me.Request.UrlReferrer Is Nothing Then
                    Refferer = Me.Request.UrlReferrer.ToString
                Else
                    Refferer = ""
                End If

                GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")
                lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

                mObjShowBapis = New ShowBapisClass(m_User.IsTestUser)
                Session.Add("mObjShowBapisSession", mObjShowBapis)
            End If


            If mObjShowBapis Is Nothing Then
                If Session("mObjShowBapisSession") Is Nothing Then
                    Throw New Exception("Benötigtes Session Objekt nicht vorhanden")
                Else
                    mObjShowBapis = CType(Session("mObjShowBapisSession"), ShowBapisClass)
                End If
            End If



            If Not IsPostBack Then

                'seitenspeziefische Aktionen
                '-----------------------

                If Me.Request.QueryString.Item("BapiName") Is Nothing Then
                    FillWebBapisGrid(0)
                Else
                    'verlinkung
                    FillWebBapisGrid(0)
                    showSapBapi(Me.Request.QueryString.Item("BapiName"))
                    showWebBapi(Me.Request.QueryString.Item("BapiName"))
                End If




            End If
        Catch ex As Exception
            lblWebBapisError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub responseBack()
        If Refferer = "" Then
            Dim strLinkPrefix As String = "/" & ConfigurationManager.AppSettings("WebAppPath") & "/"
            Response.Redirect(strLinkPrefix & "Start/Selection.aspx")
        Else
            Response.Redirect(Refferer)
        End If
    End Sub

    Private Sub FillWebBapisGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If IsNothing(mObjShowBapis.DBProxys) OrElse mObjShowBapis.DBProxys.Rows.Count = 0 Then

            lblWebBapisNoData.Text = "Keine Daten zur Anzeige gefunden."
            WebBapisDG.Visible = False
            lblWebBapisNoData.Visible = True
            lblWebBapisInfo.Text = "Anzahl: 0"
        Else

            WebBapisDG.Visible = True
            lblWebBapisNoData.Visible = False

            Dim tmpDataView As New DataView(mObjShowBapis.DBProxys)
            tmpDataView.RowStateFilter = DataViewRowState.ModifiedCurrent

            Dim intTempPageIndex As Int32 = intPageIndex
            Dim strTempSort As String = ""
            Dim strDirection As String = ""

            If strSort.Trim(" "c).Length > 0 Then
                intTempPageIndex = 0
                strTempSort = strSort.Trim(" "c)
                If (ViewState(WebBapisDG.ID & "Sort") Is Nothing) OrElse (ViewState(WebBapisDG.ID & "Sort").ToString = strTempSort) Then
                    If ViewState(WebBapisDG.ID & "Direction") Is Nothing Then
                        strDirection = "desc"
                    Else
                        strDirection = ViewState(WebBapisDG.ID & "Direction").ToString
                    End If
                Else
                    strDirection = "desc"
                End If

                If strDirection = "asc" Then
                    strDirection = "desc"
                Else
                    strDirection = "asc"
                End If

                ViewState(WebBapisDG.ID & "Sort") = strTempSort
                ViewState(WebBapisDG.ID & "Direction") = strDirection
            Else
                If Not ViewState(WebBapisDG.ID & "Sort") Is Nothing Then
                    strTempSort = ViewState(WebBapisDG.ID & "Sort").ToString
                    If ViewState(WebBapisDG.ID & "Direction") Is Nothing Then
                        strDirection = "asc"
                        ViewState(WebBapisDG.ID & "Direction") = strDirection
                    Else
                        strDirection = ViewState(WebBapisDG.ID & "Direction").ToString
                    End If
                End If
            End If

            If Not strTempSort.Length = 0 Then
                tmpDataView.Sort = strTempSort & " " & strDirection
            End If

            lblWebBapisInfo.Text = "Anzahl: " & tmpDataView.Count

            WebBapisDG.CurrentPageIndex = intTempPageIndex

            WebBapisDG.DataSource = tmpDataView

            WebBapisDG.DataBind()

        End If
    End Sub

    Private Sub WebBapisDG_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles WebBapisDG.ItemCommand
        If e.CommandName = "ShowSAP" Then
            showSapBapi(e.CommandArgument.ToString)
        End If

        If e.CommandName = "ShowWEB" Then
            showWebBapi(e.CommandArgument.ToString)
        End If


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

    Private Sub WebBapisDG_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles WebBapisDG.SortCommand
        FillWebBapisGrid(0, e.SortExpression)
    End Sub

    Private Sub showSapBapi(ByVal bapiName As String)
        Try
            mObjShowBapis.getSAPStruktur(bapiName)
            FillSAPImportGrid(0)
            FillSAPExportGrid(0)
            FillSAPTabellenGrid(0)
            lblSAPBapiDatum.Text = mObjShowBapis.SapBapiDatum.ToShortDateString
            lblSAPBapiName.Text = mObjShowBapis.SapBapiName
        Catch ex As Exception
            lblSAPBapiError.Text = CKG.Base.Business.HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
        End Try
    End Sub

    Private Sub showWebBapi(ByVal bapiName As String)
        Try
            mObjShowBapis.getWebStruktur(bapiName, m_App, m_User, Me)
            FillWebImportGrid(0)
            FillWebExportGrid(0)
            FillWebTabellenGrid(0)
            lblWebBapiDatum.Text = mObjShowBapis.WebBapiDatum.ToShortDateString
            lblWebBapiName.Text = mObjShowBapis.WebBapiName
        Catch ex As Exception
            lblWebBapiError.Text = ex.Message
        End Try
    End Sub

    Protected Sub imgbSetFilter_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbSetFilter.Click
        mObjShowBapis.DBFilter = "*"
        mObjShowBapis.DBFilter = txtFilter.Text.Trim(" "c)
        FillWebBapisGrid(0)
    End Sub


    Protected Sub imgbLookSAP_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbLookSAP.Click
        showSapBapi(txtFilter.Text.Trim(" "c).Replace("*", ""))
    End Sub

    Protected Sub imgbWebBapiVisible_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbWebBapiVisible.Click
        panelWebBapi.Visible = Not panelWebBapi.Visible
        If panelWebBapi.Visible = True Then
            imgbWebBapiVisible.ImageUrl = "/PortalZLD/Images/minus.gif"
        Else
            imgbWebBapiVisible.ImageUrl = "/PortalZLD/Images/Plus1.gif"
        End If
    End Sub


#Region "WebImport"



    Private Sub FillWebImportGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If IsNothing(mObjShowBapis.WEBImportParameter) OrElse mObjShowBapis.WEBImportParameter.Rows.Count = 0 Then
            lblWebImportNoData.Text = "Keine Daten zur Anzeige gefunden."
            WebImportDG.Visible = False
            lblWebImportNoData.Visible = True
            lblWebImportInfo.Text = "Anzahl: 0"
        Else

            WebImportDG.Visible = True
            lblWebImportNoData.Visible = False

            Dim tmpDataView As New DataView(mObjShowBapis.WEBImportParameter)

            Dim intTempPageIndex As Int32 = intPageIndex
            Dim strTempSort As String = ""
            Dim strDirection As String = ""

            If strSort.Trim(" "c).Length > 0 Then
                intTempPageIndex = 0
                strTempSort = strSort.Trim(" "c)
                If (ViewState(WebImportDG.ID & "Sort") Is Nothing) OrElse (ViewState(WebImportDG.ID & "Sort").ToString = strTempSort) Then
                    If ViewState(WebImportDG.ID & "Direction") Is Nothing Then
                        strDirection = "desc"
                    Else
                        strDirection = ViewState(WebImportDG.ID & "Direction").ToString
                    End If
                Else
                    strDirection = "desc"
                End If

                If strDirection = "asc" Then
                    strDirection = "desc"
                Else
                    strDirection = "asc"
                End If

                ViewState(WebImportDG.ID & "Sort") = strTempSort
                ViewState(WebImportDG.ID & "Direction") = strDirection
            Else
                If Not ViewState(WebImportDG.ID & "Sort") Is Nothing Then
                    strTempSort = ViewState(WebImportDG.ID & "Sort").ToString
                    If ViewState(WebImportDG.ID & "Direction") Is Nothing Then
                        strDirection = "asc"
                        ViewState(WebImportDG.ID & "Direction") = strDirection
                    Else
                        strDirection = ViewState(WebImportDG.ID & "Direction").ToString
                    End If
                End If
            End If

            If Not strTempSort.Length = 0 Then
                tmpDataView.Sort = strTempSort & " " & strDirection
            End If

            lblWebImportInfo.Text = "Anzahl: " & tmpDataView.Count

            WebImportDG.CurrentPageIndex = intTempPageIndex

            WebImportDG.DataSource = tmpDataView

            WebImportDG.DataBind()

            For Each tmpItem As DataGridItem In WebImportDG.Items
                If tmpItem.Cells(2).Text = "Tabelle" Then
                    Dim tmpGrid As DataGrid = CType(tmpItem.FindControl("DataGridLvL2"), DataGrid)
                    tmpGrid.DataSource = mObjShowBapis.getWEBImportTabelle(tmpItem.Cells(1).Text)
                    tmpGrid.DataBind()
                End If
            Next

        End If
    End Sub

    Private Sub WebImportDG_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles WebImportDG.ItemCommand
        If e.CommandName = "Excel" Then
            Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & mObjShowBapis.WebBapiName & "_" & mObjShowBapis.getWEBImportTabelle(e.CommandArgument.ToString).TableName
            excelFactory.CreateDocumentAndSendAsResponse(strFileName, mObjShowBapis.getWEBImportTabelle(e.CommandArgument.ToString), Me.Page)
        End If

        If e.CommandName = "Visible" Then
            Dim tmpDG As DataGrid = CType(e.Item.FindControl("DataGridLvL2"), DataGrid)
            tmpDG.Visible = Not tmpDG.Visible
            If tmpDG.Visible Then
                CType(e.CommandSource, ImageButton).ImageUrl = "/PortalZLD/Images/minus.gif"
            Else
                CType(e.CommandSource, ImageButton).ImageUrl = "/PortalZLD/Images/Plus1.gif"
            End If
        End If
    End Sub

    Private Sub WebImportDG_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles WebImportDG.SortCommand
        FillWebImportGrid(WebImportDG.CurrentPageIndex, e.SortExpression)
    End Sub



    Protected Sub imgbWebImportParameter_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbWebImportParameter.Click
        Try
            Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & mObjShowBapis.WebBapiName & "_Web-Importparameter"
            excelFactory.CreateDocumentAndSendAsResponse(strFileName, mObjShowBapis.WEBImportParameter, Me.Page)
        Catch ex As Exception
            lblWebImportError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Protected Sub imgbWebImportVisible_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbWebImportVisible.Click
        panelWebImport.Visible = Not panelWebImport.Visible
        If panelWebImport.Visible = True Then
            imgbWebImportVisible.ImageUrl = "/PortalZLD/Images/minus.gif"
        Else
            imgbWebImportVisible.ImageUrl = "/PortalZLD/Images/Plus1.gif"
        End If
    End Sub
#End Region


#Region "WebExport"



    Private Sub FillWebExportGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If IsNothing(mObjShowBapis.WEBExportParameter) OrElse mObjShowBapis.WEBExportParameter.Rows.Count = 0 Then
            lblWebExportNoData.Text = "Keine Daten zur Anzeige gefunden."
            WebExportDG.Visible = False
            lblWebExportNoData.Visible = True
            lblWebExportInfo.Text = "Anzahl: 0"
        Else

            WebExportDG.Visible = True
            lblWebExportNoData.Visible = False

            Dim tmpDataView As New DataView(mObjShowBapis.WEBExportParameter)

            Dim intTempPageIndex As Int32 = intPageIndex
            Dim strTempSort As String = ""
            Dim strDirection As String = ""

            If strSort.Trim(" "c).Length > 0 Then
                intTempPageIndex = 0
                strTempSort = strSort.Trim(" "c)
                If (ViewState(WebExportDG.ID & "Sort") Is Nothing) OrElse (ViewState(WebExportDG.ID & "Sort").ToString = strTempSort) Then
                    If ViewState(WebExportDG.ID & "Direction") Is Nothing Then
                        strDirection = "desc"
                    Else
                        strDirection = ViewState(WebExportDG.ID & "Direction").ToString
                    End If
                Else
                    strDirection = "desc"
                End If

                If strDirection = "asc" Then
                    strDirection = "desc"
                Else
                    strDirection = "asc"
                End If

                ViewState(WebExportDG.ID & "Sort") = strTempSort
                ViewState(WebExportDG.ID & "Direction") = strDirection
            Else
                If Not ViewState(WebExportDG.ID & "Sort") Is Nothing Then
                    strTempSort = ViewState(WebExportDG.ID & "Sort").ToString
                    If ViewState(WebExportDG.ID & "Direction") Is Nothing Then
                        strDirection = "asc"
                        ViewState(WebExportDG.ID & "Direction") = strDirection
                    Else
                        strDirection = ViewState(WebExportDG.ID & "Direction").ToString
                    End If
                End If
            End If

            If Not strTempSort.Length = 0 Then
                tmpDataView.Sort = strTempSort & " " & strDirection
            End If

            lblWebExportInfo.Text = "Anzahl: " & tmpDataView.Count

            WebExportDG.CurrentPageIndex = intTempPageIndex

            WebExportDG.DataSource = tmpDataView

            WebExportDG.DataBind()

            For Each tmpItem As DataGridItem In WebExportDG.Items
                If tmpItem.Cells(2).Text = "Tabelle" Then
                    Dim tmpGrid As DataGrid = CType(tmpItem.FindControl("DataGridLvL2"), DataGrid)
                    tmpGrid.DataSource = mObjShowBapis.getWEBExportTabelle(tmpItem.Cells(1).Text)
                    tmpGrid.DataBind()
                End If
            Next

        End If
    End Sub

    Private Sub WebExportDG_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles WebExportDG.ItemCommand
        If e.CommandName = "Excel" Then
            Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & mObjShowBapis.WebBapiName & "_" & mObjShowBapis.getWEBExportTabelle(e.CommandArgument.ToString).TableName
            excelFactory.CreateDocumentAndSendAsResponse(strFileName, mObjShowBapis.getWEBExportTabelle(e.CommandArgument.ToString), Me.Page)
        End If

        If e.CommandName = "Visible" Then
            Dim tmpDG As DataGrid = CType(e.Item.FindControl("DataGridLvL2"), DataGrid)
            tmpDG.Visible = Not tmpDG.Visible
            If tmpDG.Visible Then
                CType(e.CommandSource, ImageButton).ImageUrl = "/PortalZLD/Images/minus.gif"
            Else
                CType(e.CommandSource, ImageButton).ImageUrl = "/PortalZLD/Images/Plus1.gif"
            End If
        End If
    End Sub

    Private Sub WebExportDG_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles WebExportDG.SortCommand
        FillWebExportGrid(WebExportDG.CurrentPageIndex, e.SortExpression)
    End Sub

    Protected Sub imgbWebExportParameter_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbWebExportParameter.Click
        Try
            Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & mObjShowBapis.WebBapiName & "_WebExportparameter"
            excelFactory.CreateDocumentAndSendAsResponse(strFileName, mObjShowBapis.WEBExportParameter, Me.Page)
        Catch ex As Exception
            lblWebExportError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Protected Sub imgbWebExportVisible_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbWebExportVisible.Click
        panelWebExport.Visible = Not panelWebExport.Visible
        If panelWebExport.Visible = True Then
            imgbWebExportVisible.ImageUrl = "/PortalZLD/Images/minus.gif"
        Else
            imgbWebExportVisible.ImageUrl = "/PortalZLD/Images/Plus9.gif"
        End If
    End Sub


#End Region


#Region "WebTabellen"



    Private Sub FillWebTabellenGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If IsNothing(mObjShowBapis.WEBTabellen) OrElse mObjShowBapis.WEBTabellen.Rows.Count = 0 Then
            lblWebTabellenNoData.Text = "Keine Daten zur Anzeige gefunden."
            WebTabellenDG.Visible = False
            lblWebTabellenNoData.Visible = True
            lblWebTabellenInfo.Text = "Anzahl: 0"
        Else

            WebTabellenDG.Visible = True
            lblWebTabellenNoData.Visible = False

            Dim tmpDataView As New DataView(mObjShowBapis.WEBTabellen)

            Dim intTempPageIndex As Int32 = intPageIndex
            Dim strTempSort As String = ""
            Dim strDirection As String = ""

            If strSort.Trim(" "c).Length > 0 Then
                intTempPageIndex = 0
                strTempSort = strSort.Trim(" "c)
                If (ViewState(WebTabellenDG.ID & "Sort") Is Nothing) OrElse (ViewState(WebTabellenDG.ID & "Sort").ToString = strTempSort) Then
                    If ViewState(WebTabellenDG.ID & "Direction") Is Nothing Then
                        strDirection = "desc"
                    Else
                        strDirection = ViewState(WebTabellenDG.ID & "Direction").ToString
                    End If
                Else
                    strDirection = "desc"
                End If

                If strDirection = "asc" Then
                    strDirection = "desc"
                Else
                    strDirection = "asc"
                End If

                ViewState(WebTabellenDG.ID & "Sort") = strTempSort
                ViewState(WebTabellenDG.ID & "Direction") = strDirection
            Else
                If Not ViewState(WebTabellenDG.ID & "Sort") Is Nothing Then
                    strTempSort = ViewState(WebTabellenDG.ID & "Sort").ToString
                    If ViewState(WebTabellenDG.ID & "Direction") Is Nothing Then
                        strDirection = "asc"
                        ViewState(WebTabellenDG.ID & "Direction") = strDirection
                    Else
                        strDirection = ViewState(WebTabellenDG.ID & "Direction").ToString
                    End If
                End If
            End If

            If Not strTempSort.Length = 0 Then
                tmpDataView.Sort = strTempSort & " " & strDirection
            End If

            lblWebTabellenInfo.Text = "Anzahl: " & tmpDataView.Count

            WebTabellenDG.CurrentPageIndex = intTempPageIndex

            WebTabellenDG.DataSource = tmpDataView

            WebTabellenDG.DataBind()

            For Each tmpItem As DataGridItem In WebTabellenDG.Items
                Dim tmpGrid As DataGrid = CType(tmpItem.FindControl("DataGridLvL2"), DataGrid)
                tmpGrid.DataSource = mObjShowBapis.getWEBTabellenTabelle(tmpItem.Cells(1).Text)
                tmpGrid.DataBind()
            Next

        End If
    End Sub

    Private Sub WebTabellenDG_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles WebTabellenDG.ItemCommand
        If e.CommandName = "Excel" Then
            Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & mObjShowBapis.WebBapiName & "_" & mObjShowBapis.getWEBTabellenTabelle(e.CommandArgument.ToString).TableName
            excelFactory.CreateDocumentAndSendAsResponse(strFileName, mObjShowBapis.getWEBTabellenTabelle(e.CommandArgument.ToString), Me.Page)
        End If

        If e.CommandName = "Visible" Then
            Dim tmpDG As DataGrid = CType(e.Item.FindControl("DataGridLvL2"), DataGrid)
            tmpDG.Visible = Not tmpDG.Visible
            If tmpDG.Visible Then
                CType(e.CommandSource, ImageButton).ImageUrl = "/PortalZLD/Images/minus.gif"
            Else
                CType(e.CommandSource, ImageButton).ImageUrl = "/PortalZLD/Images/Plus1.gif"
            End If
        End If
    End Sub

    Private Sub WebTabellenDG_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles WebTabellenDG.SortCommand
        FillWebTabellenGrid(WebTabellenDG.CurrentPageIndex, e.SortExpression)
    End Sub

    Protected Sub imgbWebTabellenVisible_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbWebTabellenVisible.Click
        panelWebTabellen.Visible = Not panelWebTabellen.Visible
        If panelWebTabellen.Visible Then
            imgbWebTabellenVisible.ImageUrl = "/PortalZLD/Images/minus.gif"
        Else
            imgbWebTabellenVisible.ImageUrl = "/PortalZLD/Images/Plus1.gif"
        End If
    End Sub


#End Region



    Protected Sub imgbSAPBapiVisible_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbSAPBapiVisible.Click
        panelSAPBapi.Visible = Not panelSAPBapi.Visible
        If panelSAPBapi.Visible = True Then
            imgbSAPBapiVisible.ImageUrl = "/PortalZLD/Images/minus.gif"
        Else
            imgbSAPBapiVisible.ImageUrl = "/PortalZLD/Images/Plus1.gif"
        End If
    End Sub


#Region "SAPImport"



    Private Sub FillSAPImportGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If IsNothing(mObjShowBapis.SAPImportParameter) OrElse mObjShowBapis.SAPImportParameter.Rows.Count = 0 Then
            lblSAPImportNoData.Text = "Keine Daten zur Anzeige gefunden."
            SAPImportDG.Visible = False
            lblSAPImportNoData.Visible = True
            lblSAPImportInfo.Text = "Anzahl: 0"
        Else

            SAPImportDG.Visible = True
            lblSAPImportNoData.Visible = False

            Dim tmpDataView As New DataView(mObjShowBapis.SAPImportParameter)

            Dim intTempPageIndex As Int32 = intPageIndex
            Dim strTempSort As String = ""
            Dim strDirection As String = ""

            If strSort.Trim(" "c).Length > 0 Then
                intTempPageIndex = 0
                strTempSort = strSort.Trim(" "c)
                If (ViewState(SAPImportDG.ID & "Sort") Is Nothing) OrElse (ViewState(SAPImportDG.ID & "Sort").ToString = strTempSort) Then
                    If ViewState(SAPImportDG.ID & "Direction") Is Nothing Then
                        strDirection = "desc"
                    Else
                        strDirection = ViewState(SAPImportDG.ID & "Direction").ToString
                    End If
                Else
                    strDirection = "desc"
                End If

                If strDirection = "asc" Then
                    strDirection = "desc"
                Else
                    strDirection = "asc"
                End If

                ViewState(SAPImportDG.ID & "Sort") = strTempSort
                ViewState(SAPImportDG.ID & "Direction") = strDirection
            Else
                If Not ViewState(SAPImportDG.ID & "Sort") Is Nothing Then
                    strTempSort = ViewState(SAPImportDG.ID & "Sort").ToString
                    If ViewState(SAPImportDG.ID & "Direction") Is Nothing Then
                        strDirection = "asc"
                        ViewState(SAPImportDG.ID & "Direction") = strDirection
                    Else
                        strDirection = ViewState(SAPImportDG.ID & "Direction").ToString
                    End If
                End If
            End If

            If Not strTempSort.Length = 0 Then
                tmpDataView.Sort = strTempSort & " " & strDirection
            End If

            lblSAPImportInfo.Text = "Anzahl: " & tmpDataView.Count

            SAPImportDG.CurrentPageIndex = intTempPageIndex

            SAPImportDG.DataSource = tmpDataView

            SAPImportDG.DataBind()

            For Each tmpItem As DataGridItem In SAPImportDG.Items
                If tmpItem.Cells(2).Text = "Tabelle" Then
                    Dim tmpGrid As DataGrid = CType(tmpItem.FindControl("DataGridLvL2"), DataGrid)
                    tmpGrid.DataSource = mObjShowBapis.getSAPImportTabelle(tmpItem.Cells(1).Text)
                    tmpGrid.DataBind()
                End If
            Next

        End If
    End Sub

    Private Sub SAPImportDG_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles SAPImportDG.ItemCommand
        If e.CommandName = "Excel" Then
            Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & mObjShowBapis.SapBapiName & "_" & mObjShowBapis.getSAPImportTabelle(e.CommandArgument.ToString).TableName
            excelFactory.CreateDocumentAndSendAsResponse(strFileName, mObjShowBapis.getSAPImportTabelle(e.CommandArgument.ToString), Me.Page)
        End If

        If e.CommandName = "Visible" Then
            Dim tmpDG As DataGrid = CType(e.Item.FindControl("DataGridLvL2"), DataGrid)
            tmpDG.Visible = Not tmpDG.Visible
            If tmpDG.Visible Then
                CType(e.CommandSource, ImageButton).ImageUrl = "/PortalZLD/Images/minus.gif"
            Else
                CType(e.CommandSource, ImageButton).ImageUrl = "/PortalZLD/Images/Plus1.gif"
            End If
        End If
    End Sub

    Private Sub SAPImportDG_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles SAPImportDG.SortCommand
        FillSAPImportGrid(SAPImportDG.CurrentPageIndex, e.SortExpression)
    End Sub



    Protected Sub imgbSAPImportParameter_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbSAPImportParameter.Click
        Try
            Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & mObjShowBapis.SapBapiName & "_SAP-Importparameter"
            excelFactory.CreateDocumentAndSendAsResponse(strFileName, mObjShowBapis.SAPImportParameter, Me.Page)
        Catch ex As Exception
            lblSAPImportError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Protected Sub imgbSAPImportVisible_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbSAPImportVisible.Click
        panelSAPImport.Visible = Not panelSAPImport.Visible
        If panelSAPImport.Visible = True Then
            imgbSAPImportVisible.ImageUrl = "/PortalZLD/Images/minus.gif"
        Else
            imgbSAPImportVisible.ImageUrl = "/PortalZLD/Images/Plus1.gif"
        End If
    End Sub
#End Region


#Region "SAPExport"



    Private Sub FillSAPExportGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If IsNothing(mObjShowBapis.SAPExportParameter) OrElse mObjShowBapis.SAPExportParameter.Rows.Count = 0 Then
            lblSAPExportNoData.Text = "Keine Daten zur Anzeige gefunden."
            SAPExportDG.Visible = False
            lblSAPExportNoData.Visible = True
            lblSAPExportInfo.Text = "Anzahl: 0"
        Else

            SAPExportDG.Visible = True
            lblSAPExportNoData.Visible = False

            Dim tmpDataView As New DataView(mObjShowBapis.SAPExportParameter)

            Dim intTempPageIndex As Int32 = intPageIndex
            Dim strTempSort As String = ""
            Dim strDirection As String = ""

            If strSort.Trim(" "c).Length > 0 Then
                intTempPageIndex = 0
                strTempSort = strSort.Trim(" "c)
                If (ViewState(SAPExportDG.ID & "Sort") Is Nothing) OrElse (ViewState(SAPExportDG.ID & "Sort").ToString = strTempSort) Then
                    If ViewState(SAPExportDG.ID & "Direction") Is Nothing Then
                        strDirection = "desc"
                    Else
                        strDirection = ViewState(SAPExportDG.ID & "Direction").ToString
                    End If
                Else
                    strDirection = "desc"
                End If

                If strDirection = "asc" Then
                    strDirection = "desc"
                Else
                    strDirection = "asc"
                End If

                ViewState(SAPExportDG.ID & "Sort") = strTempSort
                ViewState(SAPExportDG.ID & "Direction") = strDirection
            Else
                If Not ViewState(SAPExportDG.ID & "Sort") Is Nothing Then
                    strTempSort = ViewState(SAPExportDG.ID & "Sort").ToString
                    If ViewState(SAPExportDG.ID & "Direction") Is Nothing Then
                        strDirection = "asc"
                        ViewState(SAPExportDG.ID & "Direction") = strDirection
                    Else
                        strDirection = ViewState(SAPExportDG.ID & "Direction").ToString
                    End If
                End If
            End If

            If Not strTempSort.Length = 0 Then
                tmpDataView.Sort = strTempSort & " " & strDirection
            End If

            lblSAPExportInfo.Text = "Anzahl: " & tmpDataView.Count

            SAPExportDG.CurrentPageIndex = intTempPageIndex

            SAPExportDG.DataSource = tmpDataView

            SAPExportDG.DataBind()

            For Each tmpItem As DataGridItem In SAPExportDG.Items
                If tmpItem.Cells(2).Text = "Tabelle" Then
                    Dim tmpGrid As DataGrid = CType(tmpItem.FindControl("DataGridLvL2"), DataGrid)
                    tmpGrid.DataSource = mObjShowBapis.getSAPExportTabelle(tmpItem.Cells(1).Text)
                    tmpGrid.DataBind()
                End If
            Next

        End If
    End Sub

    Private Sub SAPExportDG_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles SAPExportDG.ItemCommand
        If e.CommandName = "Excel" Then
            Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & mObjShowBapis.SapBapiName & "_" & mObjShowBapis.getSAPExportTabelle(e.CommandArgument.ToString).TableName
            excelFactory.CreateDocumentAndSendAsResponse(strFileName, mObjShowBapis.getSAPExportTabelle(e.CommandArgument.ToString), Me.Page)
        End If

        If e.CommandName = "Visible" Then
            Dim tmpDG As DataGrid = CType(e.Item.FindControl("DataGridLvL2"), DataGrid)
            tmpDG.Visible = Not tmpDG.Visible
            If tmpDG.Visible Then
                CType(e.CommandSource, ImageButton).ImageUrl = "/PortalZLD/Images/minus.gif"
            Else
                CType(e.CommandSource, ImageButton).ImageUrl = "/PortalZLD/Images/Plus1.gif"
            End If
        End If
    End Sub

    Private Sub SAPExportDG_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles SAPExportDG.SortCommand
        FillSAPExportGrid(SAPExportDG.CurrentPageIndex, e.SortExpression)
    End Sub

    Protected Sub imgbSAPExportParameter_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbSAPExportParameter.Click
        Try
            Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & mObjShowBapis.SapBapiName & "_SAPExportparameter"
            excelFactory.CreateDocumentAndSendAsResponse(strFileName, mObjShowBapis.SAPExportParameter, Me.Page)
        Catch ex As Exception
            lblSAPExportError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Protected Sub imgbSAPExportVisible_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbSAPExportVisible.Click
        panelSAPExport.Visible = Not panelSAPExport.Visible
        If panelSAPExport.Visible = True Then
            imgbSAPExportVisible.ImageUrl = "/PortalZLD/Images/minus.gif"
        Else
            imgbSAPExportVisible.ImageUrl = "/PortalZLD/Images/Plus1.gif"
        End If
    End Sub


#End Region


#Region "SAPTabellen"



    Private Sub FillSAPTabellenGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If IsNothing(mObjShowBapis.SAPTabellen) OrElse mObjShowBapis.SAPTabellen.Rows.Count = 0 Then
            lblSAPTabellenNoData.Text = "Keine Daten zur Anzeige gefunden."
            SAPTabellenDG.Visible = False
            lblSAPTabellenNoData.Visible = True
            lblSAPTabellenInfo.Text = "Anzahl: 0"
        Else

            SAPTabellenDG.Visible = True
            lblSAPTabellenNoData.Visible = False

            Dim tmpDataView As New DataView(mObjShowBapis.SAPTabellen)

            Dim intTempPageIndex As Int32 = intPageIndex
            Dim strTempSort As String = ""
            Dim strDirection As String = ""

            If strSort.Trim(" "c).Length > 0 Then
                intTempPageIndex = 0
                strTempSort = strSort.Trim(" "c)
                If (ViewState(SAPTabellenDG.ID & "Sort") Is Nothing) OrElse (ViewState(SAPTabellenDG.ID & "Sort").ToString = strTempSort) Then
                    If ViewState(SAPTabellenDG.ID & "Direction") Is Nothing Then
                        strDirection = "desc"
                    Else
                        strDirection = ViewState(SAPTabellenDG.ID & "Direction").ToString
                    End If
                Else
                    strDirection = "desc"
                End If

                If strDirection = "asc" Then
                    strDirection = "desc"
                Else
                    strDirection = "asc"
                End If

                ViewState(SAPTabellenDG.ID & "Sort") = strTempSort
                ViewState(SAPTabellenDG.ID & "Direction") = strDirection
            Else
                If Not ViewState(SAPTabellenDG.ID & "Sort") Is Nothing Then
                    strTempSort = ViewState(SAPTabellenDG.ID & "Sort").ToString
                    If ViewState(SAPTabellenDG.ID & "Direction") Is Nothing Then
                        strDirection = "asc"
                        ViewState(SAPTabellenDG.ID & "Direction") = strDirection
                    Else
                        strDirection = ViewState(SAPTabellenDG.ID & "Direction").ToString
                    End If
                End If
            End If

            If Not strTempSort.Length = 0 Then
                tmpDataView.Sort = strTempSort & " " & strDirection
            End If

            lblSAPTabellenInfo.Text = "Anzahl: " & tmpDataView.Count

            SAPTabellenDG.CurrentPageIndex = intTempPageIndex

            SAPTabellenDG.DataSource = tmpDataView

            SAPTabellenDG.DataBind()

            For Each tmpItem As DataGridItem In SAPTabellenDG.Items
                Dim tmpGrid As DataGrid = CType(tmpItem.FindControl("DataGridLvL2"), DataGrid)
                tmpGrid.DataSource = mObjShowBapis.getSAPTabellenTabelle(tmpItem.Cells(1).Text)
                tmpGrid.DataBind()
            Next

        End If
    End Sub

    Private Sub SAPTabellenDG_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles SAPTabellenDG.ItemCommand
        If e.CommandName = "Excel" Then
            Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & mObjShowBapis.SapBapiName & "_" & mObjShowBapis.getSAPTabellenTabelle(e.CommandArgument.ToString).TableName
            excelFactory.CreateDocumentAndSendAsResponse(strFileName, mObjShowBapis.getSAPTabellenTabelle(e.CommandArgument.ToString), Me.Page)
        End If

        If e.CommandName = "Visible" Then
            Dim tmpDG As DataGrid = CType(e.Item.FindControl("DataGridLvL2"), DataGrid)
            tmpDG.Visible = Not tmpDG.Visible
            If tmpDG.Visible Then
                CType(e.CommandSource, ImageButton).ImageUrl = "/PortalZLD/Images/minus.gif"
            Else
                CType(e.CommandSource, ImageButton).ImageUrl = "/PortalZLD/Images/Plus1.gif"
            End If
        End If
    End Sub

    Private Sub SAPTabellenDG_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles SAPTabellenDG.SortCommand
        FillSAPTabellenGrid(SAPTabellenDG.CurrentPageIndex, e.SortExpression)
    End Sub

    Protected Sub imgbSAPTabellenVisible_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbSAPTabellenVisible.Click
        panelSAPTabellen.Visible = Not panelSAPTabellen.Visible
        If panelSAPTabellen.Visible Then
            imgbSAPTabellenVisible.ImageUrl = "/PortalZLD/Images/minus.gif"
        Else
            imgbSAPTabellenVisible.ImageUrl = "/PortalZLD/Images/Plus1.gif"
        End If
    End Sub


#End Region

#End Region

End Class