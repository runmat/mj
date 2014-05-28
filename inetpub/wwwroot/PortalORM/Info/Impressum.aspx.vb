Imports CKG.Base.Kernel

Namespace Info
    Public Class Impressum
        Inherits System.Web.UI.Page
        Protected WithEvents ucStyles As PageElements.Styles

#Region " Vom Web Form Designer generierter Code "
        Protected WithEvents tblEditUser As System.Web.UI.HtmlControls.HtmlTable
        Protected WithEvents tblSearchResult As System.Web.UI.HtmlControls.HtmlTable
        Protected WithEvents lblCMail As System.Web.UI.WebControls.Label
        Protected WithEvents lblCWeb As System.Web.UI.WebControls.Label
        Protected WithEvents Form1 As System.Web.UI.HtmlControls.HtmlForm
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
            If Not Session("objUser") Is Nothing Then
                m_User = CType(Session("objUser"), Security.User)
            End If
            ucHeader.InitUser(m_User)
            ucStyles.TitleText = "Impressum"
        End Sub
    End Class
End Namespace

' ************************************************
' $History: Impressum.aspx.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 3.04.08    Time: 17:17
' Created in $/CKAG/PortalORM/Info
' 
' *****************  Version 4  *****************
' User: Uha          Date: 1.03.07    Time: 16:38
' Updated in $/CKG/PortalORM/Info
' 
' ************************************************