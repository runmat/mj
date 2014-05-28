Option Explicit On
Option Strict On

Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Report18_1
    Inherits System.Web.UI.Page

#Region " Vom Web Form Designer generierter Code "

    'Dieser Aufruf ist für den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: Diese Methode ist für den Web Form-Designer erforderlich
        'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
        InitializeComponent()
    End Sub

#End Region

    'Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User

    Private mObjfin_10 As fin_10

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lb_zurueck As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ucStyles As Styles
    Protected WithEvents imgbExcel As ImageButton

    Protected WithEvents lblInfo As System.Web.UI.WebControls.Label



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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Hier Benutzercode zur Seiteninitialisierung einfügen

        m_User = GetUser(Me)
        'm_App = New Base.Kernel.Security.App(m_User) 'erzeugt ein App_objekt 
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        lblError.Text = ""

        If Not IsPostBack Then
            If Refferer Is Nothing Then
                If Not Request.UrlReferrer Is Nothing Then
                    Refferer = Request.UrlReferrer.ToString
                Else
                    Refferer = ""
                End If
            End If

            GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
        End If

        If mObjfin_10 Is Nothing Then
            If Session("mObjfin_10Session") Is Nothing Then
                Throw New Exception("benötigtiges SessionObjekt nicht vorhanden")
            Else
                mObjfin_10 = CType(Session("mObjfin_10Session"), fin_10)
            End If
        End If

        If Not IsPostBack Then

            'seitenspeziefische Aktionen

            'dropDownListe füllen
            ddlPageSize.Items.Add("10")
            ddlPageSize.Items.Add("20")
            ddlPageSize.Items.Add("50")
            ddlPageSize.Items.Add("100")
            ddlPageSize.Items.Add("200")
            ddlPageSize.Items.Add("500")
            ddlPageSize.Items.Add("1000")
            ddlPageSize.SelectedIndex = 2

            FillGrid(0)
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

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If mObjfin_10.Status = 0 Then

            If IsNothing(mObjfin_10.Result) OrElse mObjfin_10.Result.Rows.Count = 0 Then

                lblNoData.Text = "Keine Daten zur Anzeige gefunden."
                DataGrid1.Visible = False
                lblNoData.Visible = True
                lblInfo.Text = "Anzahl: 0"
            Else

                DataGrid1.Visible = True
                lblNoData.Visible = False

                Dim tmpDataView As New DataView(mObjfin_10.Result)


                Dim intTempPageIndex As Int32 = intPageIndex
                Dim strTempSort As String = ""
                Dim strDirection As String = ""

                If strSort.Trim(" "c).Length > 0 Then
                    intTempPageIndex = 0
                    strTempSort = strSort.Trim(" "c)
                    If (ViewState("Sort") Is Nothing) OrElse (ViewState("Sort").ToString = strTempSort) Then
                        If ViewState("Direction") Is Nothing Then
                            strDirection = "desc"
                        Else
                            strDirection = ViewState("Direction").ToString
                        End If
                    Else
                        strDirection = "desc"
                    End If

                    If strDirection = "asc" Then
                        strDirection = "desc"
                    Else
                        strDirection = "asc"
                    End If

                    ViewState("Sort") = strTempSort
                    ViewState("Direction") = strDirection
                Else
                    If Not ViewState("Sort") Is Nothing Then
                        strTempSort = ViewState("Sort").ToString
                        If ViewState("Direction") Is Nothing Then
                            strDirection = "asc"
                            ViewState("Direction") = strDirection
                        Else
                            strDirection = ViewState("Direction").ToString
                        End If
                    End If
                End If

                If Not strTempSort.Length = 0 Then
                    tmpDataView.Sort = strTempSort & " " & strDirection
                End If

                lblInfo.Text = "Anzahl: " & tmpDataView.Count

                DataGrid1.CurrentPageIndex = intTempPageIndex

                DataGrid1.DataSource = tmpDataView

                DataGrid1.DataBind()

                For Each item As DataGridItem In DataGrid1.Items
                    If Not item.FindControl("lnkFahrgestellnummer") Is Nothing Then
                        CType(item.FindControl("lnkFahrgestellnummer"), HyperLink).NavigateUrl = "Report46.aspx?AppID=" & m_User.Applications.Select("AppName = 'Report46'")(0)("AppID").ToString & "&VIN=" & CType(item.FindControl("lnkFahrgestellnummer"), HyperLink).Text
                    End If
                Next
            End If
        Else
            lblError.Text = mObjfin_10.Message
            Exit Sub
        End If

    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        DataGrid1.EditItemIndex = -1
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        DataGrid1.EditItemIndex = -1
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        DataGrid1.EditItemIndex = -1
        FillGrid(0)
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

    Protected Sub imgbExcel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbExcel.Click
        'Dim control As New Control
        Dim tblTranslations As DataTable
        Dim tblTemp As DataTable
        Dim AppURL As String
        Dim col As DataGridColumn
        Dim col2 As DataColumn
        Dim bVisibility As Integer
        Dim i As Integer
        Dim sColName As String

        AppURL = Replace(Request.Url.LocalPath, "/Portal", "..")
        tblTranslations = CType(Session(AppURL), DataTable)
        tblTemp = mObjfin_10.Result.Copy

        For Each col In DataGrid1.Columns
            For i = tblTemp.Columns.Count - 1 To 0 Step -1
                bVisibility = 0
                col2 = tblTemp.Columns(i)
                If col2.ColumnName.ToUpper = col.SortExpression.ToUpper OrElse col2.ColumnName.ToUpper = col.HeaderText.ToUpper.Replace("COL_", "") Then
                    sColName = TranslateColLbtn(DataGrid1, tblTranslations, col.HeaderText, bVisibility)
                    If bVisibility = 0 Then
                        tblTemp.Columns.Remove(col2)
                    ElseIf sColName.Length > 0 Then
                        col2.ColumnName = sColName
                    Else
                        'alle spalten die nicht in der spaltenübersetzung sind, entfernen
                        tblTemp.Columns.Remove(col2)
                    End If

                End If
            Next
            tblTemp.AcceptChanges()
        Next

        Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
        excelFactory.CreateDocumentAndSendAsResponse(strFileName, tblTemp, Page)

    End Sub
End Class

' ************************************************
' $History: Report18_1.aspx.vb $
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 5.06.09    Time: 15:58
' Updated in $/CKAG/Components/ComCommon/Finance
' mögliche try catches entfernt
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 4.05.09    Time: 11:50
' Updated in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 3.12.08    Time: 16:01
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2342 testfertig
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 24.10.08   Time: 10:03
' Updated in $/CKAG/Components/ComCommon/Finance
' bug bei Exceldownload beseitigt(translation laut Fieldtranslation des
' grids)
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 20.10.08   Time: 14:59
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2288 nachbesserung
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 20.10.08   Time: 14:21
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2288
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 20.10.08   Time: 13:17
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2288
' 
' ************************************************
