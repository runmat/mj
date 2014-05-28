Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Change04
    Inherits System.Web.UI.Page

    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lb_Suche As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbl_Haendlernummer As System.Web.UI.WebControls.Label
    Protected WithEvents txtPersonennummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents trCreate As System.Web.UI.HtmlControls.HtmlTableRow
    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Protected WithEvents lbl_FIN As System.Web.UI.WebControls.Label
    Protected WithEvents txtEndkundennummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtHaendlernummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtLIZNR As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtFIN As System.Web.UI.WebControls.TextBox

    Protected WithEvents lbl_Hersteller As System.Web.UI.WebControls.Label
    Protected WithEvents ddl_Haendler As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lbl_Haendler As System.Web.UI.WebControls.Label
    Dim mObjFreigabe As Freigabe

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
        Try
            m_User = GetUser(Me) ' füllen Form.Session("objUser"), rückgabe eines UserObjekte 
            m_App = New Base.Kernel.Security.App(m_User) 'erzeugt ein App_objekt 

            FormAuth(Me, m_User)
            ucHeader.InitUser(m_User)
            GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            lblError.Text = ""

            If Not IsPostBack Then
                mObjFreigabe = New Freigabe(m_User, m_App, CStr(Session("AppID")), Session.SessionID, "")
                mObjFreigabe.SessionID = Session.SessionID
                mObjFreigabe.AppID = CStr(Session("AppID"))
                Session.Add("mObjFreigabeSession", mObjFreigabe)
            Else
                If mObjFreigabe Is Nothing Then
                    mObjFreigabe = CType(Session("mObjFreigabeSession"), Freigabe)
                End If
            End If

        Catch ex As Exception
            lblError.Text = "Fehler beim laden der Seite: " & ex.Message.ToString
        End Try

    End Sub

    Private Sub lb_Suche_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_Suche.Click

        mObjFreigabe.SucheFahrgestellnr = txtFIN.Text.Trim(" "c)
        mObjFreigabe.SucheEndkundennummer = txtEndkundennummer.Text.Trim(" "c)
        mObjFreigabe.SucheLIZNR = txtLIZNR.Text.ToUpper.Trim(" "c)
        mObjFreigabe.SucheHaendlernummer = txtHaendlernummer.Text.Trim(" "c)

        mObjFreigabe.Show()

        If mObjFreigabe.Status = 0 AndAlso Not mObjFreigabe.Result Is Nothing Then
            Response.Redirect("Change04_1.aspx?AppID=" & Session("AppID").ToString)
        ElseIf mObjFreigabe.Status < 0 Then
            lblError.Text = mObjFreigabe.Message
        ElseIf mObjFreigabe.Result.Rows.Count <= 0 Then
            lblError.Text = "keine Daten vorhanden"
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
' $History: Change04.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 11.08.08   Time: 17:54
' Updated in $/CKAG/Applications/AppBPLG/Forms
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 23.07.08   Time: 13:56
' Created in $/CKAG/Applications/AppBPLG/Forms
' ITA 2100 erstellung Rohversion
'
' ************************************************
