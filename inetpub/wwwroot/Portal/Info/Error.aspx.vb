Imports CKG.Base.Kernel
Imports CKG.Base.Business.HelpProcedures

Namespace Info
    Public Class ErrorPage
        Inherits System.Web.UI.Page
        Protected WithEvents ucStyles As PageElements.Styles

#Region " Vom Web Form Designer generierter Code "
        Protected WithEvents lblError As System.Web.UI.WebControls.Label
        Protected WithEvents tblEditUser As System.Web.UI.HtmlControls.HtmlTable
        Protected WithEvents tblSearchResult As System.Web.UI.HtmlControls.HtmlTable
        Protected WithEvents lblCMail As System.Web.UI.WebControls.Label
        Protected WithEvents lblCWeb As System.Web.UI.WebControls.Label
        Protected WithEvents lnkMail As System.Web.UI.WebControls.HyperLink
        Protected WithEvents pnlLinks As System.Web.UI.WebControls.Panel
        Protected WithEvents Form1 As System.Web.UI.HtmlControls.HtmlForm
        Protected WithEvents lblErrorMessage As System.Web.UI.WebControls.Label
        Protected WithEvents lblCAddress As System.Web.UI.WebControls.Label
        Protected WithEvents lnkWeb As System.Web.UI.WebControls.HyperLink
        Protected WithEvents lblCName As System.Web.UI.WebControls.Label
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
                ucHeader.InitUser(m_User)
                ucStyles.TitleText = "Fehler"

                Dim strID As String = CStr(Request.QueryString("ID"))
                Select Case strID
                    Case "404"
                        lblErrorMessage.Text = "Die angeforderte Seite wurde nicht gefunden! (HTTP-Error 404)"
                    Case "403"
                        lblErrorMessage.Text = "Sie haben keinen Zugriff auf die angeforderte Seite oder das Verzeichnis! (HTTP-Error 403)"
                    Case "500"
                        lblErrorMessage.Text = "Ein interner Server-Fehler ist aufgetreten! (HTTP-Error 500)"
                    Case Else
                        lblErrorMessage.Text = "Ein Fehler ist aufgetreten!" & "<br><br>"
                        Dim ex As Exception = Server.GetLastError
                        Dim exIn As Exception = ex
                        While Not exIn Is Nothing
                            lblErrorMessage.Text = exIn.Message & "<br><br>"
                            exIn = exIn.InnerException
                        End While
                        Server.ClearError()
                End Select

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
                End If

            Catch ex As Exception
                lblError.Text = ex.Message
            End Try
        End Sub
    End Class
End Namespace

' ************************************************
' $History: Error.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Dittbernerc  Date: 9.05.11    Time: 16:52
' Updated in $/CKAG/portal/Info
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 3.04.08    Time: 17:17
' Created in $/CKAG/portal/Info
' 
' *****************  Version 4  *****************
' User: Uha          Date: 1.03.07    Time: 16:38
' Updated in $/CKG/Portal/Info
' 
' ************************************************