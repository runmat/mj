Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common


Public Class Change05_2
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

    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents Hyperlink2 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ShowScript As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblInfo As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lnkKreditlimit As System.Web.UI.WebControls.HyperLink
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents trSumme As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents btnConfirm As System.Web.UI.WebControls.LinkButton
    Protected WithEvents btnBack As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lstSumme As System.Web.UI.WebControls.ListBox
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lnkExcel As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblDownloadTip As System.Web.UI.WebControls.Label
    Protected WithEvents lblTask As System.Web.UI.WebControls.Label

    Private Const strTaskZulassen As String = "Zulassen"
    Private Const strTaskSperren As String = "Sperren"
    Private Const strTaskEntsperren As String = "Entsperren"
    Private Const strTaskVerschieben As String = "Verschieben"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'cmdSave.Enabled = False
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text
        'HyperLink1.NavigateUrl = "Change05_0.aspx?AppID=" & Session("AppID").ToString

        Try
            m_App = New Base.Kernel.Security.App(m_User)
            objSuche = CType(Session("objSuche"), Change_01)
            lblTask.Text = objSuche.Task.ToUpper

            If Not IsPostBack Then
                FillGrid()
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub FillGrid()
        Dim tmpDataView As New DataView()
        Dim status As String = ""
        Dim tblResult As DataTable

        tblResult = objSuche.getSummen(objSuche.Result, status)

        If status = String.Empty Then
            With lstSumme
                .DataSource = tblResult
                .DataTextField = "ModellAnzeige"
                .DataValueField = "Modell"
                .DataBind()
                .Enabled = False
            End With
            
            tmpDataView = objSuche.Result.DefaultView
            tmpDataView.RowFilter = "SelectedEinzel=True AND Art='CAR'"

            If tmpDataView.Count = 0 Then
                DataGrid1.Visible = False
            Else
                DataGrid1.DataSource = tmpDataView
                DataGrid1.DataBind()
            End If
        Else
            lblError.Text = status
        End If
    End Sub

    Private Sub btnConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConfirm.Click
        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
        Dim objExcelExport As New Excel.ExcelExport()
        Dim tblExcel As DataTable

        btnConfirm.Enabled = False
        objSuche = CType(Session("objSuche"), Change_01)

        Select Case objSuche.Task
            Case strTaskZulassen
                objSuche.setZulassung(Session("AppID").ToString, Session.SessionID, Me)
            Case strTaskSperren
                objSuche.setSperre(Session("AppID").ToString, Session.SessionID, Me)
            Case strTaskEntsperren
                objSuche.setSperre(Session("AppID").ToString, Session.SessionID, Me)
            Case strTaskVerschieben
                objSuche.setVerschieb(Session("AppID").ToString, Session.SessionID, Me)
        End Select

        FillGrid()
        tblExcel = objSuche.setResultRowClear()

        If Not tblExcel Is Nothing Then
            Try
                Excel.ExcelExport.WriteExcel(tblExcel, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                lnkExcel.Visible = True
                lblDownloadTip.Visible = True

                'änderung Excel Pfade JJ2007.12.14
                Dim AbsoluterPfadZumVirituellenVerz As String = ConfigurationManager.AppSettings("ReplaceExcelPath")
                lnkExcel.NavigateUrl = ConfigurationManager.AppSettings("ExcelPath").Replace(AbsoluterPfadZumVirituellenVerz, "") & strFileName & "".Replace("/", "\")


            Catch ex As Exception
            End Try
        End If

        Session("objSuche") = Nothing
    End Sub

    Private Sub DataGrid1_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGrid1.ItemDataBound
        If objSuche.Task = strTaskZulassen Then
            e.Item.Cells(9).Visible = False         'Bemerkungsfeld
            e.Item.Cells(10).Visible = False        'Ziel-PDI
        End If
        If objSuche.Task = strTaskSperren Then
            e.Item.Cells(7).Visible = False         'Zulassungsdatum
            e.Item.Cells(8).Visible = False         'Kennzeichenserie
            e.Item.Cells(9).Visible = False         'Bemerkungsfeld
            e.Item.Cells(10).Visible = False        'Ziel-PDI
        End If
        If objSuche.Task = strTaskEntsperren Then
            e.Item.Cells(7).Visible = False         'Zulassungsdatum
            e.Item.Cells(8).Visible = False         'Kennzeichenserie
            e.Item.Cells(9).Visible = False         'Bemerkungsfeld
            e.Item.Cells(10).Visible = False        'Ziel-PDI
        End If
        If objSuche.Task = strTaskVerschieben Then
            e.Item.Cells(7).Visible = False         'Zulassungsdatum
            e.Item.Cells(8).Visible = False         'Kennzeichenserie
            e.Item.Cells(9).Visible = False         'Bemerkungsfeld
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
' $History: Change05_2.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 25.06.09   Time: 14:20
' Updated in $/CKAG/Applications/appec/Forms
' ITA 2918 Z_M_WARENKORB_SPERRE, Z_MASSENZULASSUNG,
' Z_M_EC_AVM_KENNZ_SERIE, Z_M_EC_AVM_PDIWECHSEL,
' Z_M_EC_AVM_ZULASSUNGSSPERRE
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 17.06.09   Time: 8:47
' Updated in $/CKAG/Applications/appec/Forms
' Warnungen entfernt!
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 15:49
' Created in $/CKAG/Applications/appec/Forms
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 14.12.07   Time: 13:45
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' Anpassung Excel Links, wegen Webconfig Änderung, jetzt Variabel ab
' Virtuellem Verzeichnis
' 
' *****************  Version 10  *****************
' User: Uha          Date: 2.07.07    Time: 12:29
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 9  *****************
' User: Uha          Date: 7.03.07    Time: 11:02
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' 
' ************************************************
