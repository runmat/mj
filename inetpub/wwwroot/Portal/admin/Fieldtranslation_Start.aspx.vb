
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Admin


Public Class Fieldtranslation_Start
    Inherits System.Web.UI.Page
    Private m_User As User
    Private m_App As App

    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles

#Region " Declarations"

    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label

    Protected WithEvents btnSuche As Button

    Protected WithEvents cmdback As System.Web.UI.WebControls.LinkButton

    Protected WithEvents txtFilterAppName As TextBox
    Protected WithEvents txtFilterAppFriendlyName As TextBox

    Private dvApplication As DataView

    Private mcontext As HttpContext = HttpContext.Current

#End Region



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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Hier Benutzercode zur Seiteninitialisierung einfügen
        Try
            m_User = GetUser(Me)
            m_App = New Base.Kernel.Security.App(m_User) 'erzeugt ein App_objekt 
            ucHeader.InitUser(m_User)
            AdminAuth(Me, m_User, AdminLevel.Master)

            If Not IsPostBack Then
                If Not Me.Request.UrlReferrer Is Nothing Then
                    If Me.Request.UrlReferrer.ToString.Contains("FieldTranslation") Then
                        'ist von einer feldübersetzung zurückgekommen, grid laden
                        FillDataGrid()
                    End If
                End If
                GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")
                lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
                ucStyles.TitleText = lblHead.Text
            End If


        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub btnSuche_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSuche.Click
        search()
        FillDataGrid()
    End Sub


    Private Sub FillDataGrid()
        Dim strSort As String = "AppID"
        If Not ViewState("ResultSort") Is Nothing Then
            strSort = ViewState("ResultSort").ToString
        End If
        FillDataGrid(strSort)
    End Sub
    Private Sub FillDataGrid(ByVal strSort As String)

        'If Not mcontext.Cache("myAppListView") Is Nothing Then
        '    dvApplication = CType(mcontext.Cache("myAppListView"), DataView)
        If Not Session("myAppListView") Is Nothing Then
            dvApplication = CType(Session("myAppListView"), DataView)
        Else
            search()
        End If
        dvApplication.Sort = strSort
        If dvApplication.Count > DataGrid1.PageSize Then
            DataGrid1.PagerStyle.Visible = True
        Else
            DataGrid1.PagerStyle.Visible = False
        End If

        DataGrid1.DataSource = dvApplication
        DataGrid1.DataBind()
    End Sub

    Private Sub search()
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try

            Dim dtApplication As ApplicationList

            cn.Open()

            dtApplication = New ApplicationList(txtFilterAppName.Text, _
                                                cn, _
                                                txtFilterAppFriendlyName.Text)

            dvApplication = dtApplication.DefaultView
            'mcontext.Cache.Insert("myAppListView", dvApplication, Nothing, DateTime.Now.AddMinutes(20), TimeSpan.Zero)
            Session("myAppListView") = dvApplication
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        Dim strSort As String = e.SortExpression
        If Not ViewState("ResultSort") Is Nothing AndAlso ViewState("ResultSort").ToString = strSort Then
            strSort &= " DESC"
        End If
        ViewState("ResultSort") = strSort
        FillDataGrid(strSort)
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        DataGrid1.CurrentPageIndex = e.NewPageIndex
        FillDataGrid()
    End Sub


    Private Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.ItemCommand
        If e.CommandName = "goToFieldTranslation" Then
            Response.Redirect("FieldTranslation.aspx?AppURL=" & e.CommandArgument)
        End If
    End Sub

    Protected Sub cmdback_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdback.Click
        Response.Redirect("../Start/Selection.aspx")
    End Sub
End Class

' ************************************************
' $History: Fieldtranslation_Start.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 6.01.09    Time: 11:45
' Updated in $/CKAG/admin
' ITA 2503  Cache durch Session ersetzt
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 8.10.08    Time: 13:35
' Updated in $/CKAG/admin
' ITA 2295 testfertig
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 6.10.08    Time: 9:19
' Updated in $/CKAG/admin
' ITA 2295 fertig
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 2.10.08    Time: 14:42
' Created in $/CKAG/admin
' ITA 2295 kompilierfähig
' ************************************************
