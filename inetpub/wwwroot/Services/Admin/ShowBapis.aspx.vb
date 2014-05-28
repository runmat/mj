Imports CKG
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common
Imports Telerik.Web.UI
Imports CKG.Base.Kernel.Security

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

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)
        Try
            m_User = Common.GetUser(Me)
            Common.AdminAuth(Me, m_User, AdminLevel.Master)
            m_App = New App(m_User)
            Common.GetAppIDFromQueryString(Me)

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

            If Not IsPostBack Then
                If String.IsNullOrEmpty(Refferer) AndAlso Not Me.Request.UrlReferrer Is Nothing Then
                    Refferer = Me.Request.UrlReferrer.ToString
                End If

                'reloading page - remove old instance from session
                Session.Remove("mObjShowBapisSession")
            End If

            mObjShowBapis = Common.GetOrCreateObject("mObjShowBapisSession", Function() New ShowBapisClass(m_User.IsTestUser))

            If Not IsPostBack Then

                'seitenspezifische Aktionen
                '-----------------------
                Dim bapi = Request("BapiName")
                If bapi Is Nothing Then bapi = String.Empty

                FillWebBapisGrid()

                If Not String.IsNullOrEmpty(bapi) Then
                    'verlinkung
                    showSapBapi(bapi)
                    showWebBapi(bapi)
                End If
            End If

            Common.TranslateTelerikColumns(WebBapisDG) ' no translations available (yet?), WebImportDG, WebExportDG, WebTabellenDG)
        Catch ex As Exception
            lblWebBapisError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Protected Overrides Sub OnPreRender(e As EventArgs)
        MyBase.OnPreRender(e)

        Common.SetEndASPXAccess(Me)
    End Sub

    Protected Overrides Sub OnUnload(e As EventArgs)
        MyBase.OnUnload(e)

        Common.SetEndASPXAccess(Me)
    End Sub

    Protected Sub responseBack(sender As Object, e As EventArgs)
        If Refferer = "" Then
            Dim strLinkPrefix As String = "/" & ConfigurationManager.AppSettings("WebAppPath") & "/"
            Response.Redirect(strLinkPrefix & "Start/Selection.aspx")
        Else
            Response.Redirect(Refferer)
        End If
    End Sub

    Private Sub FillWebBapisGrid(Optional ByVal rebind As Boolean = True)
        If IsNothing(mObjShowBapis.DBProxys) OrElse mObjShowBapis.DBProxys.Rows.Count = 0 Then
            lblWebBapisNoData.Text = "Keine Daten zur Anzeige gefunden."
            lblWebBapisNoData.Visible = True
            WebBapisDG.Visible = False
            lblWebBapisInfo.Text = "Anzahl: 0"
        Else
            lblWebBapisNoData.Visible = False
            WebBapisDG.Visible = True

            Dim tmpDataView As New DataView(mObjShowBapis.DBProxys)
            tmpDataView.RowStateFilter = DataViewRowState.ModifiedCurrent

            lblWebBapisInfo.Text = "Anzahl: " & tmpDataView.Count

            WebBapisDG.DataSource = tmpDataView

            If rebind Then WebBapisDG.Rebind()
        End If
    End Sub

    Protected Sub WebBapisDG_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs)
        FillWebBapisGrid(False)
    End Sub

    Protected Sub WebBapisDG_ItemCommand(sender As Object, e As GridCommandEventArgs)
        If e.CommandName = "LookAt" Then
            Dim bapi = e.CommandArgument
            showSapBapi(bapi)
            showWebBapi(bapi)
        End If

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
            FillWebImportGrid()
            FillWebExportGrid()
            FillWebTabellenGrid()
            lblWebBapiDatum.Text = mObjShowBapis.WebBapiDatum.ToShortDateString
            lblWebBapiName.Text = mObjShowBapis.WebBapiName
        Catch ex As Exception
            lblWebBapiError.Text = ex.Message
        End Try
    End Sub

    Protected Sub imgbSetFilter_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbSetFilter.Click
        Dim filterValue = txtFilter.Text.Trim()
        If String.IsNullOrEmpty(filterValue) Then filterValue = "*"

        mObjShowBapis.DBFilter = filterValue
        FillWebBapisGrid()
    End Sub

    Protected Sub imgbLookSAP_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbLookSAP.Click
        showSapBapi(txtFilter.Text.Trim(" "c).Replace("*", ""))
    End Sub

    Protected Sub imgbWebBapiVisible_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbWebBapiVisible.Click
        panelWebBapi.Visible = Not panelWebBapi.Visible
        If panelWebBapi.Visible = True Then
            imgbWebBapiVisible.ImageUrl = "../Images/minus.gif"
        Else
            imgbWebBapiVisible.ImageUrl = "../Images/plus.gif"
        End If
    End Sub


#Region "WebImport"

    Private Sub FillWebImportGrid(Optional ByVal rebind As Boolean = True)
        If IsNothing(mObjShowBapis.WEBImportParameter) OrElse mObjShowBapis.WEBImportParameter.Rows.Count = 0 Then
            lblWebImportNoData.Text = "Keine Daten zur Anzeige gefunden."
            WebImportDG.Visible = False
            lblWebImportNoData.Visible = True
            lblWebImportInfo.Text = "Anzahl: 0"
        Else

            WebImportDG.Visible = True
            lblWebImportNoData.Visible = False

            Dim tmpDataView As New DataView(mObjShowBapis.WEBImportParameter)

            lblWebImportInfo.Text = "Anzahl: " & tmpDataView.Count

            WebImportDG.DataSource = tmpDataView

            If rebind Then WebImportDG.Rebind()

        End If
    End Sub

    Protected Sub WebImportDG_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs)
        If (Not e.IsFromDetailTable) Then FillWebImportGrid(False)
    End Sub

    Protected Sub WebImportDG_DetailTableDataBind(sender As Object, e As GridDetailTableDataBindEventArgs)
        If Not e.DetailTableView.Name = "Spalten" Then Return

        Dim dataItem = CType(e.DetailTableView.ParentItem, GridDataItem)
        Dim tableName = dataItem.GetDataKeyValue("PARAMETER")

        e.DetailTableView.DataSource = mObjShowBapis.getWEBImportTabelle(tableName)
    End Sub

    Protected Sub imgbWebImportVisible_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbWebImportVisible.Click
        panelWebImport.Visible = Not panelWebImport.Visible
        If panelWebImport.Visible = True Then
            imgbWebImportVisible.ImageUrl = "../Images/minus.gif"
        Else
            imgbWebImportVisible.ImageUrl = "../Images/plus.gif"
        End If
    End Sub

#End Region


#Region "WebExport"

    Private Sub FillWebExportGrid(Optional ByVal rebind As Boolean = True)
        If IsNothing(mObjShowBapis.WEBExportParameter) OrElse mObjShowBapis.WEBExportParameter.Rows.Count = 0 Then
            lblWebExportNoData.Text = "Keine Daten zur Anzeige gefunden."
            WebExportDG.Visible = False
            lblWebExportNoData.Visible = True
            lblWebExportInfo.Text = "Anzahl: 0"
        Else

            WebExportDG.Visible = True
            lblWebExportNoData.Visible = False

            Dim tmpDataView As New DataView(mObjShowBapis.WEBExportParameter)


            lblWebExportInfo.Text = "Anzahl: " & tmpDataView.Count

            WebExportDG.DataSource = tmpDataView

            If rebind Then WebExportDG.Rebind()

        End If
    End Sub

    Protected Sub WebExportDG_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs)
        If (Not e.IsFromDetailTable) Then FillWebExportGrid(False)
    End Sub

    Protected Sub WebExportDG_DetailTableDataBind(sender As Object, e As GridDetailTableDataBindEventArgs)
        If Not e.DetailTableView.Name = "Spalten" Then Return

        Dim dataItem = CType(e.DetailTableView.ParentItem, GridDataItem)
        Dim tableName = dataItem.GetDataKeyValue("PARAMETER")

        e.DetailTableView.DataSource = mObjShowBapis.getWEBExportTabelle(tableName)
    End Sub

    Protected Sub imgbWebExportVisible_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbWebExportVisible.Click
        panelWebExport.Visible = Not panelWebExport.Visible
        If panelWebExport.Visible = True Then
            imgbWebExportVisible.ImageUrl = "../Images/minus.gif"
        Else
            imgbWebExportVisible.ImageUrl = "../Images/plus.gif"
        End If
    End Sub

#End Region


#Region "WebTabellen"

    Private Sub FillWebTabellenGrid(Optional ByVal rebind As Boolean = True)
        If IsNothing(mObjShowBapis.WEBTabellen) OrElse mObjShowBapis.WEBTabellen.Rows.Count = 0 Then
            lblWebTabellenNoData.Text = "Keine Daten zur Anzeige gefunden."
            WebTabellenDG.Visible = False
            lblWebTabellenNoData.Visible = True
            lblWebTabellenInfo.Text = "Anzahl: 0"
        Else

            WebTabellenDG.Visible = True
            lblWebTabellenNoData.Visible = False

            Dim tmpDataView As New DataView(mObjShowBapis.WEBTabellen)

            lblWebTabellenInfo.Text = "Anzahl: " & tmpDataView.Count

            WebTabellenDG.DataSource = tmpDataView

            If rebind Then WebTabellenDG.Rebind()
        End If
    End Sub

    Protected Sub WebTabellenDG_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs)
        If (Not e.IsFromDetailTable) Then FillWebTabellenGrid(False)
    End Sub

    Protected Sub WebTabellenDG_DetailTableDataBind(sender As Object, e As GridDetailTableDataBindEventArgs)
        If Not e.DetailTableView.Name = "Spalten" Then Return

        Dim dataItem = CType(e.DetailTableView.ParentItem, GridDataItem)
        Dim tableName = dataItem.GetDataKeyValue("TabellenName")

        e.DetailTableView.DataSource = mObjShowBapis.getWEBTabellenTabelle(tableName)
    End Sub

    Protected Sub imgbWebTabellenVisible_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbWebTabellenVisible.Click
        panelWebTabellen.Visible = Not panelWebTabellen.Visible
        If panelWebTabellen.Visible Then
            imgbWebTabellenVisible.ImageUrl = "../Images/minus.gif"
        Else
            imgbWebTabellenVisible.ImageUrl = "../Images/plus.gif"
        End If
    End Sub

#End Region


    Protected Sub imgbSAPBapiVisible_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbSAPBapiVisible.Click
        panelSAPBapi.Visible = Not panelSAPBapi.Visible
        If panelSAPBapi.Visible = True Then
            imgbSAPBapiVisible.ImageUrl = "../Images/minus.gif"
        Else
            imgbSAPBapiVisible.ImageUrl = "../Images/plus.gif"
        End If
    End Sub


#Region "SAPImport"

    Private Sub FillSAPImportGrid(Optional ByVal rebind As Boolean = True)
        If IsNothing(mObjShowBapis.SAPImportParameter) OrElse mObjShowBapis.SAPImportParameter.Rows.Count = 0 Then
            lblSAPImportNoData.Text = "Keine Daten zur Anzeige gefunden."
            SAPImportDG.Visible = False
            lblSAPImportNoData.Visible = True
            lblSAPImportInfo.Text = "Anzahl: 0"
        Else

            SAPImportDG.Visible = True
            lblSAPImportNoData.Visible = False

            Dim tmpDataView As New DataView(mObjShowBapis.SAPImportParameter)

            lblSAPImportInfo.Text = "Anzahl: " & tmpDataView.Count

            SAPImportDG.DataSource = tmpDataView

            If rebind Then SAPImportDG.Rebind()
        End If
    End Sub

    Protected Sub SAPImportDG_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs)
        If (Not e.IsFromDetailTable) Then FillSAPImportGrid(False)
    End Sub

    Protected Sub SAPImportDG_DetailTableDataBind(sender As Object, e As GridDetailTableDataBindEventArgs)
        If Not e.DetailTableView.Name = "Spalten" Then Return

        Dim dataItem = CType(e.DetailTableView.ParentItem, GridDataItem)
        Dim tableName = dataItem.GetDataKeyValue("PARAMETER")

        e.DetailTableView.DataSource = mObjShowBapis.getSAPImportTabelle(tableName)
    End Sub

    Protected Sub imgbSAPImportVisible_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbSAPImportVisible.Click
        panelSAPImport.Visible = Not panelSAPImport.Visible
        If panelSAPImport.Visible Then
            FillSAPImportGrid()
            imgbSAPImportVisible.ImageUrl = "../Images/minus.gif"
        Else
            imgbSAPImportVisible.ImageUrl = "../Images/plus.gif"
        End If
    End Sub

#End Region


#Region "SAPExport"

    Private Sub FillSAPExportGrid(Optional ByVal rebind As Boolean = True)
        If IsNothing(mObjShowBapis.SAPExportParameter) OrElse mObjShowBapis.SAPExportParameter.Rows.Count = 0 Then
            lblSAPExportNoData.Text = "Keine Daten zur Anzeige gefunden."
            SAPExportDG.Visible = False
            lblSAPExportNoData.Visible = True
            lblSAPExportInfo.Text = "Anzahl: 0"
        Else

            SAPExportDG.Visible = True
            lblSAPExportNoData.Visible = False

            Dim tmpDataView As New DataView(mObjShowBapis.SAPExportParameter)

            lblSAPExportInfo.Text = "Anzahl: " & tmpDataView.Count

            SAPExportDG.DataSource = tmpDataView

            If rebind Then SAPExportDG.Rebind()
        End If
    End Sub

    Protected Sub SAPExportDG_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs)
        If (Not e.IsFromDetailTable) Then FillSAPExportGrid(False)
    End Sub

    Protected Sub SAPExportDG_DetailTableDataBind(sender As Object, e As GridDetailTableDataBindEventArgs)
        If Not e.DetailTableView.Name = "Spalten" Then Return

        Dim dataItem = CType(e.DetailTableView.ParentItem, GridDataItem)
        Dim tableName = dataItem.GetDataKeyValue("PARAMETER")

        e.DetailTableView.DataSource = mObjShowBapis.getSAPExportTabelle(tableName)
    End Sub

    Protected Sub imgbSAPExportVisible_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbSAPExportVisible.Click
        panelSAPExport.Visible = Not panelSAPExport.Visible
        If panelSAPExport.Visible Then
            FillSAPExportGrid()
            imgbSAPExportVisible.ImageUrl = "../Images/minus.gif"
        Else
            imgbSAPExportVisible.ImageUrl = "../Images/plus.gif"
        End If
    End Sub

#End Region


#Region "SAPTabellen"

    Protected Sub SAPTabellenDG_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs)
        If (Not e.IsFromDetailTable) Then FillSAPTabellenGrid(False)
    End Sub

    Protected Sub SAPTabellenDG_DetailTableDataBind(sender As Object, e As GridDetailTableDataBindEventArgs)
        If Not e.DetailTableView.Name = "Spalten" Then Return

        Dim dataItem = CType(e.DetailTableView.ParentItem, GridDataItem)
        Dim tableName = dataItem.GetDataKeyValue("TabellenName")

        e.DetailTableView.DataSource = mObjShowBapis.getSAPTabellenTabelle(tableName)
    End Sub

    Private Sub FillSAPTabellenGrid(Optional ByVal rebind As Boolean = True)
        If IsNothing(mObjShowBapis.SAPTabellen) OrElse mObjShowBapis.SAPTabellen.Rows.Count = 0 Then
            lblSAPTabellenNoData.Text = "Keine Daten zur Anzeige gefunden."
            SAPTabellenDG.Visible = False
            lblSAPTabellenNoData.Visible = True
            lblSAPTabellenInfo.Text = "Anzahl: 0"
        Else
            SAPTabellenDG.Visible = True
            lblSAPTabellenNoData.Visible = False

            Dim tmpDataView As New DataView(mObjShowBapis.SAPTabellen)

            lblSAPTabellenInfo.Text = "Anzahl: " & tmpDataView.Count

            SAPTabellenDG.DataSource = tmpDataView

            If rebind Then SAPTabellenDG.Rebind()
        End If
    End Sub

    Protected Sub imgbSAPTabellenVisible_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbSAPTabellenVisible.Click
        panelSAPTabellen.Visible = Not panelSAPTabellen.Visible
        If panelSAPTabellen.Visible Then
            FillSAPTabellenGrid()
            imgbSAPTabellenVisible.ImageUrl = "../Images/minus.gif"
        Else
            imgbSAPTabellenVisible.ImageUrl = "../Images/plus.gif"
        End If
    End Sub


#End Region


End Class