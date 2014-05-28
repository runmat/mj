Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements


Public Class Change00
    Inherits System.Web.UI.Page
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents ddlPDIs As System.Web.UI.WebControls.DropDownList
    Protected WithEvents btnConfirm As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cbxAlle As System.Web.UI.WebControls.CheckBox
    Protected WithEvents rblTask As System.Web.UI.WebControls.RadioButtonList
    Private strPDI As String


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
    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If Not IsPostBack Then
                '§§§ JVE 18.07.2006: Zunächst PDI-Liste laden...
                Initialload()
            Else
            End If
            'DoSubmit()
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub Initialload()
        '§§§ JVE 18.07.2006: Hier DropDownlist mit PDIs füllen...
        Dim pdiListe As New PDIListe(m_User, m_App)
        Dim item As ListItem
        Dim row As DataRow

        pdiListe.getPDIs(Session("AppID").ToString, Session.SessionID)

        If (pdiListe.Status = 0) Then
            'Werte füllen...
            For Each row In pdiListe.PPDIs.Rows
                item = New ListItem(CStr(row("NAME1")), CStr(row("KUNPDI")))
                ddlPDIs.Items.Add(item)
            Next
        Else
            lblError.Text = pdiListe.Message
        End If
    End Sub

    Private Sub btnConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConfirm.Click
        Dim objSuche As New Change_01(m_User, m_App, Session("AppID").ToString, Session.SessionID, "")

        If (objSuche.Status <> 0) Then
            lblError.Text = objSuche.Status
            Exit Sub
        End If

        If cbxAlle.Checked Then
            objSuche.PPDISuche = String.Empty
        Else
            objSuche.PPDISuche = CStr(ddlPDIs.SelectedItem.Value)
        End If

        objSuche.Task = rblTask.SelectedItem.Value

        Session("objSuche") = objSuche
        Response.Redirect("Change00_2.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Change00.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 23.03.10   Time: 17:05
' Updated in $/CKAG/Applications/AppSTRAUB/Forms
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
