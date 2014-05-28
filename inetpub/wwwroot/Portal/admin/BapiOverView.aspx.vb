Option Explicit On
Option Strict On

Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Partial Public Class BapiOverView
    Inherits System.Web.UI.Page

    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private mObjBapiOverView As BapiOverViewClass

    Protected WithEvents ucHeader As Portal.PageElements.Header
    Protected WithEvents ucStyles As Portal.PageElements.Styles

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
            ucHeader.InitUser(m_User)
            AdminAuth(Me, m_User, Security.AdminLevel.Master)
            lblRamError.Text = ""

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
                ucStyles.TitleText = lblHead.Text

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

                'seitenspeziefische Aktionen

                '#######Ram#########
                '-----------------------
                'dropDownListe füllen
                ddlRamPageSize.Items.Add("10")
                ddlRamPageSize.Items.Add("20")
                ddlRamPageSize.Items.Add("50")
                ddlRamPageSize.Items.Add("100")
                ddlRamPageSize.SelectedIndex = 0

                lblRamTimeInfo.Text = Now.ToLongTimeString

                lbSpeicherbedarf.Attributes.Add("onClick", "return confirm('zur Berechnung des Speicherbedarfs der Proxy-Objekte im RAM werden alle Objekte entfernt.');")

                FillRamGrid(0)
                '-----------------------

                '#######DB#########
                '-----------------------
                'dropDownListe füllen
                ddlDBPageSize.Items.Add("10")
                ddlDBPageSize.Items.Add("20")
                ddlDBPageSize.Items.Add("50")
                ddlDBPageSize.Items.Add("100")
                ddlDBPageSize.SelectedIndex = 0
                FillDBGrid(0)
                '-----------------------

                '#######UpdateDB#########
                '-----------------------
                'dropDownListe füllen
                ddlUpdateDBPageSize.Items.Add("10")
                ddlUpdateDBPageSize.Items.Add("20")
                ddlUpdateDBPageSize.Items.Add("50")
                ddlUpdateDBPageSize.Items.Add("100")
                ddlUpdateDBPageSize.SelectedIndex = 0
                FillUpdateDBGrid(0)
                '-----------------------


            End If


           

        Catch ex As Exception
            lblRamError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
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

    Private Sub FillRamGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If IsNothing(mObjBapiOverView.RamProxys) OrElse mObjBapiOverView.RamProxys.Rows.Count = 0 Then

            lblRamNoData.Text = "Keine Daten zur Anzeige gefunden."
            RamDG.Visible = False
            lblRamNoData.Visible = True
            lblRamInfo.Text = "Anzahl: 0"
        Else

            RamDG.Visible = True
            lblRamNoData.Visible = False

            Dim tmpDataView As New DataView(mObjBapiOverView.RamProxys)
            tmpDataView.RowStateFilter = DataViewRowState.ModifiedCurrent

            Dim intTempPageIndex As Int32 = intPageIndex
            Dim strTempSort As String = ""
            Dim strDirection As String = String.Empty

            If strSort.Trim(" "c).Length > 0 Then
                intTempPageIndex = 0
                strTempSort = strSort.Trim(" "c)
                If (ViewState(RamDG.ID & "Sort") Is Nothing) OrElse (ViewState(RamDG.ID & "Sort").ToString = strTempSort) Then
                    If ViewState(RamDG.ID & "Direction") Is Nothing Then
                        strDirection = "desc"
                    Else
                        strDirection = ViewState(RamDG.ID & "Direction").ToString
                    End If
                Else
                    strDirection = "desc"
                End If

                If strDirection = "asc" Then
                    strDirection = "desc"
                Else
                    strDirection = "asc"
                End If

                ViewState(RamDG.ID & "Sort") = strTempSort
                ViewState(RamDG.ID & "Direction") = strDirection
            Else
                If Not ViewState(RamDG.ID & "Sort") Is Nothing Then
                    strTempSort = ViewState(RamDG.ID & "Sort").ToString
                    If ViewState(RamDG.ID & "Direction") Is Nothing Then
                        strDirection = "asc"
                        ViewState(RamDG.ID & "Direction") = strDirection
                    Else
                        strDirection = ViewState(RamDG.ID & "Direction").ToString
                    End If
                End If
            End If

            If Not strTempSort.Length = 0 Then
                tmpDataView.Sort = strTempSort & " " & strDirection
            End If

            lblRamInfo.Text = "Anzahl: " & tmpDataView.Count

            RamDG.CurrentPageIndex = intTempPageIndex

            RamDG.DataSource = tmpDataView

            RamDG.DataBind()

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

            DBDG.CurrentPageIndex = intTempPageIndex

            DBDG.DataSource = tmpDataView

            DBDG.DataBind()

        End If
    End Sub

    Private Sub FillUpdateDBGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If IsNothing(mObjBapiOverView.UpdateDBProxys) OrElse mObjBapiOverView.UpdateDBProxys.Rows.Count = 0 Then

            lblUpdateDBNoData.Text = "Keine Daten zur Anzeige gefunden."
            updateDBDG.Visible = False
            lblUpdateDBNoData.Visible = True
            lblUpdateDBInfo.Text = "Anzahl: 0"
        Else

            updateDBDG.Visible = True
            lblUpdateDBNoData.Visible = False

            Dim tmpDataView As New DataView(mObjBapiOverView.UpdateDBProxys)
            tmpDataView.RowStateFilter = DataViewRowState.ModifiedCurrent

            Dim intTempPageIndex As Int32 = intPageIndex
            Dim strTempSort As String = ""
            Dim strDirection As String = ""

            If strSort.Trim(" "c).Length > 0 Then
                intTempPageIndex = 0
                strTempSort = strSort.Trim(" "c)
                If (ViewState(updateDBDG.ID & "Sort") Is Nothing) OrElse (ViewState(updateDBDG.ID & "Sort").ToString = strTempSort) Then
                    If ViewState(updateDBDG.ID & "Direction") Is Nothing Then
                        strDirection = "desc"
                    Else
                        strDirection = ViewState(updateDBDG.ID & "Direction").ToString
                    End If
                Else
                    strDirection = "desc"
                End If

                If strDirection = "asc" Then
                    strDirection = "desc"
                Else
                    strDirection = "asc"
                End If

                ViewState(updateDBDG.ID & "Sort") = strTempSort
                ViewState(updateDBDG.ID & "Direction") = strDirection
            Else
                If Not ViewState(updateDBDG.ID & "Sort") Is Nothing Then
                    strTempSort = ViewState(updateDBDG.ID & "Sort").ToString
                    If ViewState(updateDBDG.ID & "Direction") Is Nothing Then
                        strDirection = "asc"
                        ViewState(updateDBDG.ID & "Direction") = strDirection
                    Else
                        strDirection = ViewState(updateDBDG.ID & "Direction").ToString
                    End If
                End If
            End If

            If Not strTempSort.Length = 0 Then
                tmpDataView.Sort = strTempSort & " " & strDirection
            End If

            lblUpdateDBInfo.Text = "Anzahl: " & tmpDataView.Count

            updateDBDG.CurrentPageIndex = intTempPageIndex

            updateDBDG.DataSource = tmpDataView

            updateDBDG.DataBind()

        End If
    End Sub

    Private Sub RamDG_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles RamDG.ItemCommand
        If e.CommandName = "Delete" Then
            mObjBapiOverView.deleteBapiFrom(e.CommandArgument.ToString, "RAM")
            mObjBapiOverView.fillRamProxys()
            FillRamGrid(0)
        End If
    End Sub


    Protected Sub ddlRamPageSize_SelectedIndexChanged1(ByVal sender As Object, ByVal e As EventArgs) Handles ddlRamPageSize.SelectedIndexChanged
        RamDG.PageSize = CInt(ddlRamPageSize.SelectedItem.Value)
        RamDG.EditItemIndex = -1
        FillRamGrid(0)
    End Sub


    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles RamDG.PageIndexChanged
        RamDG.EditItemIndex = -1
        FillRamGrid(e.NewPageIndex)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles RamDG.SortCommand
        RamDG.EditItemIndex = -1
        FillRamGrid(RamDG.CurrentPageIndex, e.SortExpression)
    End Sub


    Private Sub ddlRamPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlDBPageSize.SelectedIndexChanged
        DBDG.PageSize = CInt(ddlDBPageSize.SelectedItem.Value)
        DBDG.EditItemIndex = -1
        FillDBGrid(0)
    End Sub

    Private Sub DBDG_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DBDG.ItemCommand

        If e.CommandName = "Delete" Then
            mObjBapiOverView.deleteBapiFrom(e.CommandArgument.ToString, "DB")
            mObjBapiOverView.fillDBProxys()
            FillDBGrid(0)
            mObjBapiOverView.fillRamProxys()
            FillRamGrid(0)
        End If

        If e.CommandName = "Details" Then
            Dim Parameterlist As String = ""
            CKG.Base.Business.HelpProcedures.getAppParameters(Session("AppID").ToString, Parameterlist, ConfigurationManager.AppSettings.Get("Connectionstring"))
            Response.Redirect("ShowBapis.aspx?AppID=" & Session("AppID").ToString & Parameterlist & "&BapiName=" & e.CommandArgument.ToString)
        End If
    End Sub


    Private Sub DBDB_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DBDG.PageIndexChanged
        DBDG.EditItemIndex = -1
        FillDBGrid(e.NewPageIndex)
    End Sub


    Private Sub DBDB_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DBDG.SortCommand
        DBDG.EditItemIndex = -1
        FillDBGrid(DBDG.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub ddlDBPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlDBPageSize.SelectedIndexChanged
        DBDG.PageSize = CInt(ddlDBPageSize.SelectedItem.Value)
        DBDG.EditItemIndex = -1
        FillDBGrid(0)
    End Sub

    Private Sub UpdateDBDG_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles updateDBDG.ItemCommand
        If e.CommandName = "Delete" Then
            mObjBapiOverView.deleteBapiFrom(e.CommandArgument.ToString, "updateDB")
            mObjBapiOverView.fillUpdateDBProxys()
            FillUpdateDBGrid(0)
        End If
    End Sub

    Private Sub UpdateDB_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles updateDBDG.PageIndexChanged
        updateDBDG.EditItemIndex = -1
        FillUpdateDBGrid(e.NewPageIndex)
    End Sub

    Private Sub UpdateDB_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles updateDBDG.SortCommand
        updateDBDG.EditItemIndex = -1
        FillUpdateDBGrid(updateDBDG.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub ddlUpdateDBPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlUpdateDBPageSize.SelectedIndexChanged
        updateDBDG.PageSize = CInt(ddlUpdateDBPageSize.SelectedItem.Value)
        updateDBDG.EditItemIndex = -1
        FillUpdateDBGrid(0)
    End Sub

    Protected Sub imgbRamExcel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbRamExcel.Click
        Try
            Dim control As New Control
            Dim tblTranslations As New DataTable()
            Dim tblTemp As New DataTable()
            Dim AppURL As String
            Dim col As DataGridColumn
            Dim col2 As DataColumn
            Dim bVisibility As Integer
            Dim i As Integer
            Dim sColName As String = ""

            AppURL = Replace(Me.Request.Url.LocalPath, "/Portal", "..")
            tblTranslations = CType(Me.Session(AppURL), DataTable)
            tblTemp = mObjBapiOverView.RamProxys.Copy
            For Each col In RamDG.Columns
                For i = tblTemp.Columns.Count - 1 To 0 Step -1
                    bVisibility = 0
                    col2 = tblTemp.Columns(i)
                    If col2.ColumnName.ToUpper = col.SortExpression.ToUpper OrElse col2.ColumnName.ToUpper = col.HeaderText.ToUpper.Replace("COL_", "") Then
                        sColName = TranslateColLbtn(RamDG, tblTranslations, col.HeaderText, bVisibility)
                        If bVisibility = 0 Then
                            tblTemp.Columns.Remove(col2)
                        ElseIf sColName.Length > 0 Then
                            col2.ColumnName = sColName
                        End If
                    End If
                    'EQUNR nicht mit in Excel ausgeben, wird meist nur als boundcolumn versteckt als schlüssel verwendet JJU2008.10.23
                    If col2.ColumnName.ToUpper = "EQUNR" Then
                        tblTemp.Columns.Remove(col2)
                    End If
                Next
                tblTemp.AcceptChanges()
            Next
            Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
            excelFactory.CreateDocumentAndSendAsResponse(strFileName, tblTemp, Me.Page)
        Catch ex As Exception
            lblRamError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Protected Sub imgbRamVisible_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbRamVisible.Click

        panelRam.Visible = Not panelRam.Visible
        If panelRam.Visible = True Then
            imgbRamVisible.ImageUrl = "../Images/minus.gif"
        Else
            imgbRamVisible.ImageUrl = "../Images/plus.gif"
        End If
    End Sub

    Protected Sub imgbDBExcel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbDBExcel.Click
        Try
            Dim control As New Control
            Dim tblTranslations As New DataTable()
            Dim tblTemp As New DataTable()
            Dim AppURL As String
            Dim col As DataGridColumn
            Dim col2 As DataColumn
            Dim bVisibility As Integer
            Dim i As Integer
            Dim sColName As String = ""

            AppURL = Replace(Me.Request.Url.LocalPath, "/Portal", "..")
            tblTranslations = CType(Me.Session(AppURL), DataTable)
            tblTemp = mObjBapiOverView.DBProxys.Copy
            For Each col In DBDG.Columns
                For i = tblTemp.Columns.Count - 1 To 0 Step -1
                    bVisibility = 0
                    col2 = tblTemp.Columns(i)
                    If col2.ColumnName.ToUpper = col.SortExpression.ToUpper OrElse col2.ColumnName.ToUpper = col.HeaderText.ToUpper.Replace("COL_", "") Then
                        sColName = TranslateColLbtn(DBDG, tblTranslations, col.HeaderText, bVisibility)
                        If bVisibility = 0 Then
                            tblTemp.Columns.Remove(col2)
                        ElseIf sColName.Length > 0 Then
                            col2.ColumnName = sColName
                        End If
                    End If
                    'EQUNR nicht mit in Excel ausgeben, wird meist nur als boundcolumn versteckt als schlüssel verwendet JJU2008.10.23
                    If col2.ColumnName.ToUpper = "EQUNR" Then
                        tblTemp.Columns.Remove(col2)
                    End If
                Next
                tblTemp.AcceptChanges()
            Next
            Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
            excelFactory.CreateDocumentAndSendAsResponse(strFileName, tblTemp, Me.Page)
        Catch ex As Exception
            lblDBError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Protected Sub imgbDBVisible_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbDBVisible.Click

        panelDB.Visible = Not panelDB.Visible
        If panelDB.Visible = True Then
            imgbDBVisible.ImageUrl = "../Images/minus.gif"
        Else
            imgbDBVisible.ImageUrl = "../Images/plus.gif"
        End If
    End Sub

    Protected Sub imgbUpdateDBExcel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbUpdateDBExcel.Click
        Try
            Dim control As New Control
            Dim tblTranslations As New DataTable()
            Dim tblTemp As New DataTable()
            Dim AppURL As String
            Dim col As DataGridColumn
            Dim col2 As DataColumn
            Dim bVisibility As Integer
            Dim i As Integer
            Dim sColName As String = ""

            AppURL = Replace(Me.Request.Url.LocalPath, "/Portal", "..")
            tblTranslations = CType(Me.Session(AppURL), DataTable)
            tblTemp = mObjBapiOverView.UpdateDBProxys.Copy
            For Each col In updateDBDG.Columns
                For i = tblTemp.Columns.Count - 1 To 0 Step -1
                    bVisibility = 0
                    col2 = tblTemp.Columns(i)
                    If col2.ColumnName.ToUpper = col.SortExpression.ToUpper OrElse col2.ColumnName.ToUpper = col.HeaderText.ToUpper.Replace("COL_", "") Then
                        sColName = TranslateColLbtn(updateDBDG, tblTranslations, col.HeaderText, bVisibility)
                        If bVisibility = 0 Then
                            tblTemp.Columns.Remove(col2)
                        ElseIf sColName.Length > 0 Then
                            col2.ColumnName = sColName
                        End If
                    End If
                    'EQUNR nicht mit in Excel ausgeben, wird meist nur als boundcolumn versteckt als schlüssel verwendet JJU2008.10.23
                    If col2.ColumnName.ToUpper = "EQUNR" Then
                        tblTemp.Columns.Remove(col2)
                    End If
                Next
                tblTemp.AcceptChanges()
            Next
            Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
            excelFactory.CreateDocumentAndSendAsResponse(strFileName, tblTemp, Me.Page)
        Catch ex As Exception
            lblUpdateDBError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Protected Sub imgbUpdateDBVisible_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbUpdateDBVisible.Click

        PanelUpdateDB.Visible = Not PanelUpdateDB.Visible
        If PanelUpdateDB.Visible = True Then
            imgbUpdateDBVisible.ImageUrl = "../Images/minus.gif"
        Else
            imgbUpdateDBVisible.ImageUrl = "../Images/plus.gif"
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

    Protected Sub lbSpeicherbedarf_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbSpeicherbedarf.Click
        lblSpeicherbedarf.Text = "Speicherbedarf: ~ " & Math.Round(mObjBapiOverView.Speicherbedarf, 2) & " Megabyte."
        mObjBapiOverView.fillRamProxys()
        lblRamTimeInfo.Text = Now.ToLongTimeString
        FillRamGrid(0)
    End Sub

    Protected Sub imgbRamAktulisieren_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbRamAktulisieren.Click
        mObjBapiOverView.fillRamProxys()
        lblRamTimeInfo.Text = Now.ToLongTimeString
        FillRamGrid(RamDG.CurrentPageIndex)
    End Sub

    Protected Sub imgbSetFilter_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbSetFilter.Click
        mObjBapiOverView.UpdateDBFilter = "*"
        mObjBapiOverView.DBFilter = "*"
        mObjBapiOverView.RamFilter = "*"
        Select Case rblFilterFor.SelectedValue
            Case "all"
                mObjBapiOverView.UpdateDBFilter = txtFilter.Text.Trim(" "c)
                mObjBapiOverView.DBFilter = txtFilter.Text.Trim(" "c)
                mObjBapiOverView.RamFilter = txtFilter.Text.Trim(" "c)
            Case "UpdateDB"
                mObjBapiOverView.UpdateDBFilter = txtFilter.Text.Trim(" "c)
            Case "DB"
                mObjBapiOverView.DBFilter = txtFilter.Text.Trim(" "c)
            Case "RAM"
                mObjBapiOverView.RamFilter = txtFilter.Text.Trim(" "c)
        End Select
        FillRamGrid(0)
        FillDBGrid(0)
        FillUpdateDBGrid(0)
    End Sub

    Protected Sub imgbInsertUpdateDG_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbInsertUpdateDG.Click
        Try
            mObjBapiOverView.insertBapiIntoUpdateDB(txtInsertBapiName.Text.Trim(" "c), m_User.UserName)
            mObjBapiOverView.fillUpdateDBProxys()
            FillUpdateDBGrid(updateDBDG.CurrentPageIndex)
        Catch ex As Exception
            lblUpdateDBError.Text = ex.Message
        End Try
    End Sub

#End Region

   
End Class

' ************************************************
' $History: BapiOverView.aspx.vb $
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 2.07.09    Time: 11:38
' Updated in $/CKAG/admin
' bugfix ddlPagesize
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 30.06.09   Time: 10:20
' Updated in $/CKAG/admin
' ddl Fix
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 28.04.09   Time: 16:42
' Updated in $/CKAG/admin
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 6.03.09    Time: 9:10
' Updated in $/CKAG/admin
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 15.12.08   Time: 17:37
' Created in $/CKAG/admin
' Dyn Proxy integriert
' 
' ************************************************
