Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common


Public Class Change00_3
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

    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private objSuche As Change_01

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents Hyperlink2 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ShowScript As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents lblDownloadTip As System.Web.UI.WebControls.Label
    Protected WithEvents lnkExcel As System.Web.UI.WebControls.HyperLink
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblInfo As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lnkKreditlimit As System.Web.UI.WebControls.HyperLink
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents trSumme As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents btnConfirm As System.Web.UI.WebControls.LinkButton

    '-----------------------------------------------------------
    'Private columnID As Integer = 0
    'Private columnArt As Integer = 1
    'Private columnDropDown_Verwendung As Integer = 8
    'Private columnDropDown_Versicherer As Integer = 9
    'Private columnTextZuldatum As Integer = 7
    'Private columnTextZulAnzahl As Integer = 10
    '-----------------------------------------------------------

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'cmdSave.Enabled = False
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text
        Try
            m_App = New Base.Kernel.Security.App(m_User)

            If Not IsPostBack Then
                FillGrid()
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub FillGrid()
        Dim tmpDataView As New DataView()

        objSuche = CType(Session("objSuche"), Change_01)

        tmpDataView = objSuche.Result.DefaultView
        tmpDataView.RowFilter = "SelectedEinzel=True AND Art='CAR'"

        If tmpDataView.Count = 0 Then
            DataGrid1.Visible = False
        Else
            DataGrid1.DataSource = tmpDataView
            DataGrid1.DataBind()
        End If
    End Sub

    Private Sub btnConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConfirm.Click
        btnConfirm.Enabled = False

        objSuche = CType(Session("objSuche"), Change_01)

        If objSuche.Task = "Zulassung" Then
            objSuche.setZulassung(Session("AppID").ToString, Session.SessionID)
        Else
            objSuche.setDisposition(Session("AppID").ToString, Session.SessionID)
        End If

        FillGrid()
    End Sub

    Private Sub DataGrid1_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGrid1.ItemDataBound
        objSuche = CType(Session("objSuche"), Change_01)

        If objSuche.Task <> "Zulassung" Then
            e.Item.Cells(8).Visible = False
            e.Item.Cells(9).Visible = False
        End If

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Change00_3.aspx.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 17:38
' Created in $/CKAG/Applications/AppSTRAUB/Forms
' 
' *****************  Version 7  *****************
' User: Uha          Date: 2.07.07    Time: 14:28
' Updated in $/CKG/Applications/AppSTRAUB/AppSTRAUBWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 6  *****************
' User: Uha          Date: 8.03.07    Time: 14:18
' Updated in $/CKG/Applications/AppSTRAUB/AppSTRAUBWeb/Forms
' 
' ************************************************
