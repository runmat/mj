Option Explicit On
Option Strict On

Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Services.PageElements
Imports CKG.Base.Business
Imports CKG

Partial Public Class PeisAdministration
    Inherits System.Web.UI.Page

    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private mObjPeisAdministration As PeisAdministrationClass

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
            AdminAuth(Me, m_User, Security.AdminLevel.Master)

            lblError.Text = ""

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

                mObjPeisAdministration = New PeisAdministrationClass()
                Session.Add("mObjPeisAdministrationSession", mObjPeisAdministration)
            End If


            If mObjPeisAdministration Is Nothing Then
                If Session("mObjPeisAdministrationSession") Is Nothing Then
                    Throw New Exception("Benötigtes Session Objekt nicht vorhanden")
                Else
                    mObjPeisAdministration = CType(Session("mObjPeisAdministrationSession"), PeisAdministrationClass)
                End If
            End If



            If Not IsPostBack Then

                'seitenspeziefische Aktionen

                '################
                '-----------------------
                'dropDownListe(füllen)
                'ddlPageSize.Items.Add("10")
                'ddlPageSize.Items.Add("20")
                'ddlPageSize.Items.Add("50")
                'ddlPageSize.Items.Add("100")
                'ddlPageSize.SelectedIndex = 0

                'FillGrid(0)
                ''-----------------------

                'If CBool(ConfigurationManager.AppSettings("enablePEIS")) Then
                '    lblPeisStatus.Text = "on"
                '    lblPeisStatus.ForeColor = Color.Green
                'Else
                '    lblPeisStatus.Text = "off"
                '    lblPeisStatus.ForeColor = Color.Red
                'End If

                'If CBool(ConfigurationManager.AppSettings("enablePeisErrorFilter")) Then
                '    lblPeisInterneFehlerFilter.Text = "on"
                '    lblPeisInterneFehlerFilter.ForeColor = Color.Green
                'Else
                '    lblPeisInterneFehlerFilter.Text = "off"
                '    lblPeisInterneFehlerFilter.ForeColor = Color.Red
                'End If

                'If CBool(ConfigurationManager.AppSettings("enableSessionTimeOutFilter")) Then
                '    lblSessionTimeOutFilter.Text = "on"
                '    lblSessionTimeOutFilter.ForeColor = Color.Green
                'Else
                '    lblSessionTimeOutFilter.Text = "off"
                '    lblSessionTimeOutFilter.ForeColor = Color.Red
                'End If

                'lblPeisTargetMail.Text = ConfigurationManager.AppSettings("PEISTargetEmail")


            End If


        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
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

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        'If IsNothing(mObjPeisAdministration.Filter) OrElse mObjPeisAdministration.Filter.Rows.Count = 0 Then

        '    lblNoData.Text = "Keine Daten zur Anzeige gefunden."
        '    DG.Visible = False
        '    lblNoData.Visible = True
        '    lblInfo.Text = "Anzahl: 0"
        'Else

        '    DG.Visible = True
        '    lblNoData.Visible = False

        '    Dim tmpDataView As New DataView(mObjPeisAdministration.Filter)

        '    Dim intTempPageIndex As Int32 = intPageIndex
        '    Dim strTempSort As String = ""
        '    Dim strDirection As String = String.Empty

        '    If strSort.Trim(" "c).Length > 0 Then
        '        intTempPageIndex = 0
        '        strTempSort = strSort.Trim(" "c)
        '        If (ViewState(DG.ID & "Sort") Is Nothing) OrElse (ViewState(DG.ID & "Sort").ToString = strTempSort) Then
        '            If ViewState(DG.ID & "Direction") Is Nothing Then
        '                strDirection = "desc"
        '            Else
        '                strDirection = ViewState(DG.ID & "Direction").ToString
        '            End If
        '        Else
        '            strDirection = "desc"
        '        End If

        '        If strDirection = "asc" Then
        '            strDirection = "desc"
        '        Else
        '            strDirection = "asc"
        '        End If

        '        ViewState(DG.ID & "Sort") = strTempSort
        '        ViewState(DG.ID & "Direction") = strDirection
        '    Else
        '        If Not ViewState(DG.ID & "Sort") Is Nothing Then
        '            strTempSort = ViewState(DG.ID & "Sort").ToString
        '            If ViewState(DG.ID & "Direction") Is Nothing Then
        '                strDirection = "asc"
        '                ViewState(DG.ID & "Direction") = strDirection
        '            Else
        '                strDirection = ViewState(DG.ID & "Direction").ToString
        '            End If
        '        End If
        '    End If

        '    If Not strTempSort.Length = 0 Then
        '        tmpDataView.Sort = strTempSort & " " & strDirection
        '    End If

        '    lblInfo.Text = "Anzahl: " & tmpDataView.Count

        '    DG.CurrentPageIndex = intTempPageIndex

        '    DG.DataSource = tmpDataView

        '    DG.DataBind()

        'End If
    End Sub

    'Private Sub DG_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DG.ItemCommand
    '    If e.CommandName = "Delete" Then
    '        mObjPeisAdministration.deleteFilter(e.CommandArgument.ToString)
    '        mObjPeisAdministration.fillFilter()
    '        FillGrid(0)
    '    End If
    'End Sub


    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Public Sub chkFilterEnabled_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim tmpGriditem As DataGridItem = CType(CType(sender, CheckBox).Parent.Parent, DataGridItem)
        If CType(sender, CheckBox).Checked Then
            mObjPeisAdministration.changeEnabled(tmpGriditem.Cells(0).Text, "X")
        Else
            mObjPeisAdministration.changeEnabled(tmpGriditem.Cells(0).Text, "")
        End If
        'mObjPeisAdministration.fillFilter()
    End Sub


    'Protected Sub ddlPageSize_SelectedIndexChanged1(ByVal sender As Object, ByVal e As EventArgs) Handles ddlPageSize.SelectedIndexChanged
    '    DG.PageSize = CInt(ddlPageSize.SelectedItem.Value)
    '    DG.EditItemIndex = -1
    '    FillGrid(0)
    'End Sub

    'Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DG.PageIndexChanged
    '    DG.EditItemIndex = -1
    '    FillGrid(e.NewPageIndex)
    'End Sub

    'Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DG.SortCommand
    '    DG.EditItemIndex = -1
    '    FillGrid(DG.CurrentPageIndex, e.SortExpression)
    'End Sub

    'Private Sub lbInsertFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbInsertFilter.Click

    '    If checkNewFilter() Then
    '        mObjPeisAdministration.insertFilter(txtFehlerName.Text, Left(txtFehlerBeschreibung.Text, 500), Left(txtFehlerBeispiel.Text, 500))
    '        mObjPeisAdministration.fillFilter()
    '        mObjPeisAdministration.KeyWords.Rows.Clear()
    '        txtFehlerBeispiel.Text = ""
    '        txtFehlerBeschreibung.Text = ""
    '        txtFehlerName.Text = ""
    '        dgKeyWords.DataSource = mObjPeisAdministration.KeyWords
    '        dgKeyWords.DataBind()
    '        mObjPeisAdministration.fillFilter()
    '        FillGrid(0)
    '    End If

    'End Sub

    Private Function checkNewFilter() As Boolean
        'lblError.Text = ""
        'mObjPeisAdministration.KeyWords.AcceptChanges()
        'If mObjPeisAdministration.KeyWords.Rows.Count < 3 Then
        '    lblError.Text = "Es müssen mindestens 3 KeyWords / Filter eingegeben werden"
        'End If

        'If txtFehlerBeispiel.Text.Replace(" ", "").Length = 0 Then
        '    lblError.Text = "FehlerBeispiel muss gefüllt sein"
        'End If

        'If txtFehlerName.Text.Replace(" ", "").Length = 0 Then
        '    lblError.Text = "FehlerName muss gefüllt sein"
        'End If


        'If txtFehlerBeschreibung.Text.Replace(" ", "").Length = 0 Then
        '    lblError.Text = "FehlerBeschreibung muss gefüllt sein"
        'End If

        'If lblError.Text = "" Then
        '    Return True
        'Else
        '    Return False
        'End If

    End Function

    'Protected Sub imgbInsertUpdateDG_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbInsertUpdateDG.Click
    'If Not txtKeyWordInsert.Text.Contains(" ") AndAlso HelpProcedures.isAlphaNumeric(txtKeyWordInsert.Text) AndAlso Not txtKeyWordInsert.Text.Length < 3 Then
    '    Dim tmpDataRow As DataRow = mObjPeisAdministration.KeyWords.NewRow
    '    tmpDataRow.Item(0) = txtKeyWordInsert.Text
    '    mObjPeisAdministration.KeyWords.Rows.Add(tmpDataRow)
    '    mObjPeisAdministration.KeyWords.AcceptChanges()
    '    dgKeyWords.DataSource = mObjPeisAdministration.KeyWords
    '    dgKeyWords.DataBind()
    '    txtKeyWordInsert.Text = ""
    'Else
    '    lblError.Text = "KeyWord muss gefüllt sein, muss min. 3 Zeichen haben und darf nur Buchstaben oder Zahlen entahlten "
    'End If
    'End Sub

    'Protected Sub imgbExcel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbExcel.Click
    '    Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
    '    Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
    '    excelFactory.CreateDocumentAndSendAsResponse(strFileName, mObjPeisAdministration.Filter, Me.Page)
    'End Sub

    'Private Sub dgKeyWords_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgKeyWords.ItemCommand
    'If e.CommandName = "Delete" Then
    '    mObjPeisAdministration.KeyWords.Rows(e.Item.ItemIndex).Delete()
    '    mObjPeisAdministration.KeyWords.AcceptChanges()
    '    dgKeyWords.DataSource = mObjPeisAdministration.KeyWords
    '    dgKeyWords.DataBind()
    'End If
    'End Sub

    'Private Sub lb_zurueck_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lb_zurueck.Click
    '    responseBack()
    'End Sub

#End Region



End Class
