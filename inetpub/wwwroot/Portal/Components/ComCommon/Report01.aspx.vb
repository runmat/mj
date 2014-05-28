Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Report01
    Inherits System.Web.UI.Page
    '##### ALD Report: Monatsliste
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

    Private m_User As Security.User
    Private m_App As Security.App

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents cmdCreate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ddlRange As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddlJahr As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddlMonat As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblSelect As System.Web.UI.WebControls.Label
    Protected WithEvents tdLinks As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents trSelection As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)


        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text
        m_App = New Security.App(m_User)
        trSelection.Visible = False
        If IsPostBack Then
            'lblSelect.Visible = True
        Else
            showLinks()
        End If
     
    End Sub

    Private Sub showLinks()
        Dim hlink As HyperLink
        Dim list As New ArrayList()
        Dim lit As Literal
        Dim i As Integer

        Dim year_selected As Integer
        Dim month_selected As Integer
        Dim range_selected As Integer
        Dim ym_selected As Date
        Dim mon As String
        Dim yr As String

        year_selected = Now.Year
        month_selected = Now.Month
        range_selected = 5
        ym_selected = Now.Date
        '----------------------------------------------

        For i = 1 To range_selected + 1
            mon = ym_selected.AddMonths(-i).Month.ToString
            yr = ym_selected.AddMonths(-i).Year.ToString

            hlink = New HyperLink()
            hlink.Text = CType(i, String) & ".&nbsp;&nbsp;&nbsp;" & Right("00" & mon, 2) & " / " & yr
            hlink.NavigateUrl = "Report012.aspx?AppID=" & Session("AppID").ToString & "&M=" & mon & "&Y=" & yr
            tdLinks.Controls.Add(hlink)
            lit = New Literal()
            lit.Text = "<br><br>"

            tdLinks.Controls.Add(lit)
        Next
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Report01.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 5.06.09    Time: 15:58
' Updated in $/CKAG/Components/ComCommon
' mögliche try catches entfernt
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:32
' Created in $/CKAG/Components/ComCommon
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 10:47
' Created in $/CKAG/portal/Components/ComCommon
' 
' *****************  Version 7  *****************
' User: Uha          Date: 12.07.07   Time: 9:28
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 6  *****************
' User: Uha          Date: 22.05.07   Time: 9:45
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 5  *****************
' User: Uha          Date: 15.03.07   Time: 12:49
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' Image- und Excel-Pfade korrigiert
' 
' *****************  Version 4  *****************
' User: Uha          Date: 5.03.07    Time: 14:57
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' 
' ************************************************
