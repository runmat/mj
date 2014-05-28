Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements


Public Class Report01
    Inherits System.Web.UI.Page

    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents btnConfirm As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles

    Private m_App As Base.Kernel.Security.App
    Protected WithEvents drpVJahr As System.Web.UI.WebControls.DropDownList
    Private m_User As Base.Kernel.Security.User
    Protected WithEvents txtOrgNr As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblPlatzhaltersuche As System.Web.UI.WebControls.Label

    Private objhandler As VFS01


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
                fill()

                If Not Session("objHandler") Is Nothing Then
                    objhandler = CType(Session("objHandler"), VFS01)

                    drpVJahr.Items.FindByText(objhandler.Versicherungsjahr).Selected = True
                    txtOrgNr.Text = objhandler.OrgNr

                    objhandler = Nothing
                    Session("objHandler") = Nothing
                End If
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try

    End Sub


    Private Sub fill()
        drpVJahr.Items.Add(New ListItem(Now.Year, Now.Year))
        drpVJahr.Items.Add(New ListItem(Now.Year - 1, Now.Year - 1))
        drpVJahr.Items.Add(New ListItem(Now.Year - 2, Now.Year - 2))
        'txtOrgNr.Text = "1*"
    End Sub


    Private Sub btnConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConfirm.Click

        'Kein Pflichtfeld mehr und keine Platzhaltersuche
        'If txtOrgNr.Text.Trim().Length < 2 Then
        '    lblError.Text = "Schränken Sie bitte die Suche durch Eingabe von 2 Zeichen  inkl * für die VD-Bezirk ein."
        '    Exit Sub
        'End If
        'txtOrgNr.Text = Replace(txtOrgNr.Text, "*", "")

        objhandler = New VFS01(m_User, m_App, "")

        objhandler.Versicherungsjahr = drpVJahr.SelectedItem.Text
        objhandler.OrgNr = txtOrgNr.Text

        objhandler.GiveData(Session("AppID").ToString, Session.SessionID)
        Session("objhandler") = objhandler

        objhandler = Nothing

        Response.Redirect("Report01_2.aspx?AppID=" & Session("AppID").ToString)
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
' *****************  Version 3  *****************
' User: Rudolpho     Date: 8.01.09    Time: 8:44
' Updated in $/CKAG/Applications/appvfs/Forms
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 26.11.08   Time: 17:11
' Updated in $/CKAG/Applications/appvfs/Forms
' ITA 2317 unfertig
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 17:43
' Created in $/CKAG/Applications/appvfs/Forms
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 25.06.07   Time: 14:26
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Forms
' bugfixing
' 
' *****************  Version 7  *****************
' User: Uha          Date: 21.06.07   Time: 18:45
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Forms
' Bugfixing VFS 2
' 
' *****************  Version 6  *****************
' User: Uha          Date: 20.06.07   Time: 18:58
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Forms
' Bug fixing 1
' 
' *****************  Version 5  *****************
' User: Uha          Date: 20.06.07   Time: 16:21
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 4  *****************
' User: Uha          Date: 3.05.07    Time: 19:07
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Forms
' Änderungen aus StartApplication vom 02.05.2007 Mittags übernommen
' 
' ************************************************
