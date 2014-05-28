Imports CKG.Base.Kernel
Imports CKG.Base.Business.HelpProcedures
Namespace Info
    Public Class ContactPage
        Inherits System.Web.UI.Page
        Protected WithEvents ucStyles As PageElements.Styles

#Region " Vom Web Form Designer generierter Code "
        Protected WithEvents lblError As System.Web.UI.WebControls.Label
        Protected WithEvents tblEditUser As System.Web.UI.HtmlControls.HtmlTable
        Protected WithEvents tblSearchResult As System.Web.UI.HtmlControls.HtmlTable
        Protected WithEvents lblCName As System.Web.UI.WebControls.Label
        Protected WithEvents lblCAddress As System.Web.UI.WebControls.Label
        Protected WithEvents lblCMail As System.Web.UI.WebControls.Label
        Protected WithEvents lblCWeb As System.Web.UI.WebControls.Label
        Protected WithEvents lnkMail As System.Web.UI.WebControls.HyperLink
        Protected WithEvents lnkWeb As System.Web.UI.WebControls.HyperLink
        Protected WithEvents pnlLinks As System.Web.UI.WebControls.Panel
        Protected WithEvents Form1 As System.Web.UI.HtmlControls.HtmlForm
        Protected WithEvents lblCName2 As System.Web.UI.WebControls.Label
        Protected WithEvents lblCAddress2 As System.Web.UI.WebControls.Label
        Protected WithEvents lnkMail2 As System.Web.UI.WebControls.HyperLink
        Protected WithEvents lnkWeb2 As System.Web.UI.WebControls.HyperLink
        Protected WithEvents pnlLinks2 As System.Web.UI.WebControls.Panel
        Protected WithEvents pnl2ndAddress As System.Web.UI.WebControls.Panel
        Protected WithEvents ucHeader As PageElements.Header
        'Dieser Aufruf ist für den Web Form-Designer erforderlich.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: Diese Methode ist für den Web Form-Designer erforderlich
            'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
            InitializeComponent()
        End Sub

#End Region

#Region " Membervariables "
        Private m_User As Security.User
#End Region

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            ' Hier Benutzercode zur Seiteninitialisierung einfügen
            Try
                If Not Session("objUser") Is Nothing Then
                    m_User = CType(Session("objUser"), Security.User)
                End If
                'InitHeader.InitUser(m_User)

                ucHeader.InitUser(m_User)
                ucStyles.TitleText = "Kontaktseite"
                If Not m_User Is Nothing AndAlso Not m_User.Customer Is Nothing AndAlso Not m_User.Groups.Count = 0 Then
                    Dim _contact As Security.Contact = m_User.Customer.CustomerContact.CombineWith(m_User.Organization.OrganizationContact)
                    pnlLinks.Controls.Clear()

                    With _contact
                        lblCName.Text = .Name
                        lblCAddress.Text = TranslateHTML(.Address, TranslationDirection.ReadHTML)
                    End With

                    With pnlLinks.Controls
                        .Add(_contact.GetMailHyperLink)
                        .Add(New LiteralControl("<br>"))
                        .Add(_contact.GetWebHyperLink)
                    End With

                    pnl2ndAddress.Controls.Clear()
                Else
                    pnl2ndAddress.Visible = True
                End If

            Catch ex As Exception
                lblError.Text = ex.Message
            End Try
        End Sub
    End Class
End Namespace

' ************************************************
' $History: ContactPage.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Dittbernerc  Date: 11.05.11   Time: 11:31
' Updated in $/CKAG/portal/Info
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 3.04.08    Time: 17:17
' Created in $/CKAG/portal/Info
' 
' *****************  Version 5  *****************
' User: Uha          Date: 1.03.07    Time: 16:38
' Updated in $/CKG/Portal/Info
' 
' ************************************************