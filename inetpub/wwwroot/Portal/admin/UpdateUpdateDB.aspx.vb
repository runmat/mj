Option Explicit On
Option Strict On

Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Partial Public Class UpdateUpdateDB
    Inherits System.Web.UI.Page

    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User

    Private mObjUpdateUpdateDB As UpdateUpdateDBClass

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
            FormAuth(Me, m_User)
            lblUpdatedBapisError.Text = ""

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

                mObjUpdateUpdateDB = New UpdateUpdateDBClass(m_User.IsTestUser)
                Session.Add("mObjUpdateUpdateDBSession", mObjUpdateUpdateDB)
            End If


            If mObjUpdateUpdateDB Is Nothing Then
                If Session("mObjUpdateUpdateDBSession") Is Nothing Then
                    Throw New Exception("Benötigtes Session Objekt nicht vorhanden")
                Else
                    mObjUpdateUpdateDB = CType(Session("mObjUpdateUpdateDBSession"), UpdateUpdateDBClass)
                End If
            End If



            If Not IsPostBack Then
                'seitenspeziefische Aktionen
                mObjUpdateUpdateDB.getLastUpdateRun()
                lblLastUpdateDate.Text = mObjUpdateUpdateDB.LastUpdate.ToString
                lblLastUpdateWebUser.Text = mObjUpdateUpdateDB.LastUpdateUser
            End If




        Catch ex As Exception
            lblUpdatedBapisError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
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

    Private Sub FillUpdatedBapisGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If IsNothing(mObjUpdateUpdateDB.updatedBapis) OrElse mObjUpdateUpdateDB.updatedBapis.Rows.Count = 0 Then

            lblUpdatedBapisNoData.Text = "Keine Daten zur Anzeige gefunden."
            UpdatedBapisDG.Visible = False
            lblUpdatedBapisNoData.Visible = True
            lblUpdatedBapisInfo.Text = "Anzahl: 0"
            imgbUpdatedBapisExcel.Visible = False
        Else

            UpdatedBapisDG.Visible = True
            lblUpdatedBapisNoData.Visible = False
            imgbUpdatedBapisExcel.Visible = True
            Dim tmpDataView As New DataView(mObjUpdateUpdateDB.updatedBapis)


            Dim intTempPageIndex As Int32 = intPageIndex
            Dim strTempSort As String = ""
            Dim strDirection As String = ""

            If strSort.Trim(" "c).Length > 0 Then
                intTempPageIndex = 0
                strTempSort = strSort.Trim(" "c)
                If (ViewState(UpdatedBapisDG.ID & "Sort") Is Nothing) OrElse (ViewState(UpdatedBapisDG.ID & "Sort").ToString = strTempSort) Then
                    If ViewState(UpdatedBapisDG.ID & "Direction") Is Nothing Then
                        strDirection = "desc"
                    Else
                        strDirection = ViewState(UpdatedBapisDG.ID & "Direction").ToString
                    End If
                Else
                    strDirection = "desc"
                End If

                If strDirection = "asc" Then
                    strDirection = "desc"
                Else
                    strDirection = "asc"
                End If

                ViewState(UpdatedBapisDG.ID & "Sort") = strTempSort
                ViewState(UpdatedBapisDG.ID & "Direction") = strDirection
            Else
                If Not ViewState(UpdatedBapisDG.ID & "Sort") Is Nothing Then
                    strTempSort = ViewState(UpdatedBapisDG.ID & "Sort").ToString
                    If ViewState(UpdatedBapisDG.ID & "Direction") Is Nothing Then
                        strDirection = "asc"
                        ViewState(UpdatedBapisDG.ID & "Direction") = strDirection
                    Else
                        strDirection = ViewState(UpdatedBapisDG.ID & "Direction").ToString
                    End If
                End If
            End If

            If Not strTempSort.Length = 0 Then
                tmpDataView.Sort = strTempSort & " " & strDirection
            End If

            lblUpdatedBapisInfo.Text = "Anzahl: " & tmpDataView.Count

            UpdatedBapisDG.CurrentPageIndex = intTempPageIndex

            UpdatedBapisDG.DataSource = tmpDataView

            UpdatedBapisDG.DataBind()

        End If
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles UpdatedBapisDG.PageIndexChanged
        UpdatedBapisDG.EditItemIndex = -1
        FillUpdatedBapisGrid(e.NewPageIndex)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles UpdatedBapisDG.SortCommand
        UpdatedBapisDG.EditItemIndex = -1
        FillUpdatedBapisGrid(UpdatedBapisDG.CurrentPageIndex, e.SortExpression)
    End Sub

    Protected Sub imgbUpdatedBapisExcel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbUpdatedBapisExcel.Click
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
            tblTemp = mObjUpdateUpdateDB.updatedBapis.Copy
            For Each col In UpdatedBapisDG.Columns
                For i = tblTemp.Columns.Count - 1 To 0 Step -1
                    bVisibility = 1
                    col2 = tblTemp.Columns(i)
                    If col2.ColumnName.ToUpper = col.SortExpression.ToUpper OrElse col2.ColumnName.ToUpper = col.HeaderText.ToUpper.Replace("COL_", "") Then
                        sColName = TranslateColLbtn(UpdatedBapisDG, tblTranslations, col.HeaderText, bVisibility)
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
            lblUpdatedBapisError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
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

    Protected Sub imgbUpdate_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbUpdate.Click
        mObjUpdateUpdateDB.fillUpdatedBapisProxys(m_User, Session("AppID").ToString)
        imgbUpdate.Enabled = False
        FillUpdatedBapisGrid(0)
    End Sub

#End Region
End Class

' ************************************************
' $History: UpdateUpdateDB.aspx.vb $
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
