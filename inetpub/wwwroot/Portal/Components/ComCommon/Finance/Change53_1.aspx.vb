Imports System
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements


Public Class Change53_1
    Inherits System.Web.UI.Page

    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lb_back As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents trPageSize As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents trDataGrid1 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lbLoeschen As LinkButton
    Private m_User As Base.Kernel.Security.User
    'Private m_App As Base.Kernel.Security.App
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label


    Dim mObjBriefversand As Briefversand
    Dim mIDLiznr As String

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

        m_User = GetUser(Me) ' füllen Form.Session("objUser"), rückgabe eines UserObjekte 
        'm_App = New Base.Kernel.Security.App(m_User) 'erzeugt ein App_objekt 

        FormAuth(Me, m_User)
        ucHeader.InitUser(m_User)
        GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text
        lblError.Text = ""

        If mObjBriefversand Is Nothing Then
            mObjBriefversand = CType(Session("mObjBriefversandSession"), Briefversand)
        End If
        mIDLiznr = ""
        If Not Request.QueryString.Item("IDLIZNR") Is Nothing Then
            mIDLiznr = Request.QueryString.Item("IDLIZNR").ToString
        End If
        If IsPostBack = False Then
            FillGrid(0)
        End If

    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If mObjBriefversand.Status = 0 OrElse mObjBriefversand.Status = -1111 Then
            If mObjBriefversand.BriefversandFehlerTabelle.Rows.Count = 0 Then
                DataGrid1.Visible = False
                lblNoData.Visible = True
                Label1.Visible = False
                lblNoData.Text = "Keine Daten zur Anzeige gefunden."
                lbLoeschen.Enabled = False
            Else
                DataGrid1.Visible = True
                lblNoData.Visible = False

                Dim tmpDataView As DataView = mObjBriefversand.BriefversandFehlerTabelle.DefaultView

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


                DataGrid1.CurrentPageIndex = intTempPageIndex

                DataGrid1.DataSource = tmpDataView
                DataGrid1.DataBind()

                lblNoData.Text = "Es wurden " & tmpDataView.Count.ToString & " Fahrzeuge gefunden."
                lblNoData.Visible = True


                If DataGrid1.PageCount > 1 Then
                    DataGrid1.PagerStyle.CssClass = "PagerStyle"
                    DataGrid1.DataBind()
                    DataGrid1.PagerStyle.Visible = True
                Else
                    DataGrid1.PagerStyle.Visible = False
                End If
            End If
        Else
            DataGrid1.Visible = False
            Label1.Visible = False
            lblError.Text = mObjBriefversand.Message
            lblNoData.Visible = True
        End If
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Protected Sub lb_back_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_back.Click
        If mIDLiznr = "X" Then
            Response.Redirect("Change53.aspx?AppID=" & Session("AppID").ToString + "&IDLIZNR=" + mIDLiznr)
        Else
            Response.Redirect("Change53.aspx?AppID=" & Session("AppID").ToString)
        End If
    End Sub

    Protected Sub lbLoeschen_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbLoeschen.Click
        Dim anyChecked As Boolean = False
        lblError.Text = ""
        lblNoData.Text = ""

        If mObjBriefversand Is Nothing Then
            mObjBriefversand = CType(Session("mObjBriefversandSession"), Briefversand)
        End If

        For Each item As DataGridItem In DataGrid1.Items
            If CType(item.FindControl("chbLoeschen"), CheckBox).Checked = True Then

                mObjBriefversand.SapTabelle.Rows.Item(item.Cells(0).Text)("LOEVM") = "X"
                mObjBriefversand.SapTabelle.AcceptChanges()
                'grid anzeige faken!
                '---------------------------------
                mObjBriefversand.BriefversandFehlerTabelle.Select("Index='" & item.Cells(0).Text & "'")(0).Delete()
                mObjBriefversand.BriefversandFehlerTabelle.AcceptChanges()
                '---------------------------------
                anyChecked = True
            End If
        Next

        If Not anyChecked Then
            lblError.Text = "Bitte tätigen Sie eine Auswahl!"
            Exit Sub
        End If

        mObjBriefversand.change()
        If Not mObjBriefversand.Status = 0 Then
            lblError.Text = mObjBriefversand.Message
            DataGrid1.Visible = False
            lbLoeschen.Enabled = False
        Else
            FillGrid(0)
        End If
    End Sub

End Class
' ************************************************
' $History: Change53_1.aspx.vb $
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 22.12.10   Time: 15:03
' Updated in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 5.06.09    Time: 15:58
' Updated in $/CKAG/Components/ComCommon/Finance
' mögliche try catches entfernt
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 19.12.08   Time: 11:03
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2500 testfertig
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 11.09.08   Time: 10:42
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2079 fertig
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 28.07.08   Time: 15:12
' Created in $/CKAG/Components/ComCommon/Finance
' ITA 2079 fast fertig
'
' ************************************************