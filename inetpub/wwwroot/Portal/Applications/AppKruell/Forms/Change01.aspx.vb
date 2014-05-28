Option Explicit On 
Option Strict On


Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business

Public Class Change01
    Inherits System.Web.UI.Page
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lb_Suche As System.Web.UI.WebControls.LinkButton
    Protected WithEvents calVon As System.Web.UI.WebControls.Calendar
    Protected WithEvents calBis As System.Web.UI.WebControls.Calendar
    Protected WithEvents btnCal1 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents btnCal2 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents trCreate As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lb_Neu As System.Web.UI.WebControls.LinkButton
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Treffer As System.Web.UI.WebControls.Label
    Protected WithEvents lb_GeheZu As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblError As System.Web.UI.WebControls.Label


    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Protected WithEvents txt_Fahrgestellnummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_Ordernummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_DatumVon As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_DatumBis As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_Leasinggesellschaft As System.Web.UI.WebControls.TextBox
    Dim m_change As kruell_01
    Dim strErrorText As String

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

            If Not IsPostBack Then
                m_change = New kruell_01(m_User, m_App, "")
                m_change.SessionID = Me.Session.SessionID
                m_change.AppID = CStr(Session("AppID"))
                Session.Add("objChange", m_change)
            End If


        Catch ex As Exception
            lblError.Text = "Fehler beim laden der Seite: " & ex.Message.ToString
        End Try

    End Sub


   

    Private Sub lb_GeheZu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_GeheZu.Click
        Response.Redirect("Change01_1.aspx?AppID=" & Session("AppID").ToString)
    End Sub




    Private Sub lb_Neu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_Neu.Click
        Response.Redirect("Change01_2.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub lb_Suche_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_Suche.Click

        Dim valid As Boolean

        valid = HelpProcedures.checkDate(txt_DatumVon, txt_DatumBis, strErrorText, True)
        If m_change Is Nothing Then
            m_change = CType(Session("objChange"), kruell_01)
        End If


        If valid = True Then

            m_change.fahrgestellnummer = txt_Fahrgestellnummer.Text.ToUpper
            m_change.Ordernummer = txt_Ordernummer.Text
            m_change.Leasinggeber = txt_Leasinggesellschaft.Text
            m_change.DatumBis = txt_DatumBis.Text
            m_change.DatumVon = txt_DatumVon.Text


            'füllen der Datentabelle des Auftrags
            m_change.Fill(m_change.AppID, m_change.SessionID)
            If Not m_change.DatenTabelle Is Nothing Then
                lbl_Treffer.Text = m_change.DatenTabelle.Rows.Count.ToString
            End If
            lblError.Text = m_change.Message 'fehlermeldung ausgeben
        Else
            lblError.Text = strErrorText
        End If



    End Sub

    Private Sub btnCal1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCal1.Click
        calVon.Visible = True
        calBis.Visible = False
    End Sub

    Private Sub btnCal2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCal2.Click
        calVon.Visible = False
        calBis.Visible = True
    End Sub

    Private Sub calVon_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calVon.SelectionChanged
        txt_DatumVon.Text = calVon.SelectedDate.ToShortDateString
        calVon.Visible = False
    End Sub

    Private Sub calBis_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calBis.SelectionChanged
        txt_DatumBis.Text = calBis.SelectedDate.ToShortDateString
        calBis.Visible = False
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

End Class
' ************************************************
' $History: Change01.aspx.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 14:37
' Created in $/CKAG/Applications/AppKruell/Forms
' 
' *****************  Version 18  *****************
' User: Jungj        Date: 9.01.08    Time: 13:57
' Updated in $/CKG/Applications/AppKruell/AppKruellWeb/Forms
' ITA 1580 Report01 hinzugefügt, SS History Bodys hinzugefügt
' ************************************************

