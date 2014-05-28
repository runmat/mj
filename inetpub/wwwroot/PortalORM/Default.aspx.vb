
<CLSCompliant(False)> Public Class _Default
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Response.Redirect("/PortalORM/(S(" & Session.SessionID & "))/Start/Selection.aspx")
    End Sub

End Class

' ************************************************
' $History: Default.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 11.04.08   Time: 15:47
' Updated in $/CKAG/portal
' Migration
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 11.04.08   Time: 11:57
' Updated in $/CKAG/portal
' Migration OR
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 3.04.08    Time: 17:16
' Created in $/CKAG/portal
' 
' *****************  Version 4  *****************
' User: Uha          Date: 31.05.07   Time: 11:41
' Updated in $/CKG/Portal
' ITA 1077 - Login bei bereits aktiver Anmeldung ermöglichen
' 
' *****************  Version 3  *****************
' User: Uha          Date: 1.03.07    Time: 16:38
' Updated in $/CKG/Portal
' 
' ************************************************