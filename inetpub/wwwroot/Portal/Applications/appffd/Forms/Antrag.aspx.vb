Imports CKG.Base.Business
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements


Public Class Antrag
    Inherits System.Web.UI.Page
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Protected WithEvents CheckBox6 As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CheckBox5 As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CheckBox4 As System.Web.UI.WebControls.CheckBox
    Protected WithEvents Table2 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents TextBox6 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBox7 As System.Web.UI.WebControls.TextBox
    Protected WithEvents RadioButton1 As System.Web.UI.WebControls.RadioButton
    Protected WithEvents RadioButton2 As System.Web.UI.WebControls.RadioButton
    Protected WithEvents RadioButton3 As System.Web.UI.WebControls.RadioButton
    Protected WithEvents RadioButton4 As System.Web.UI.WebControls.RadioButton
    Protected WithEvents RadioButton5 As System.Web.UI.WebControls.RadioButton
    Protected WithEvents RadioButton0 As System.Web.UI.WebControls.RadioButton
    Protected WithEvents CheckBox1 As System.Web.UI.WebControls.CheckBox
    Protected WithEvents cbx3 As System.Web.UI.WebControls.CheckBox
    Protected WithEvents RadioButton6 As System.Web.UI.WebControls.RadioButton
    Protected WithEvents RadioButton7 As System.Web.UI.WebControls.RadioButton
    Protected WithEvents RadioButton8 As System.Web.UI.WebControls.RadioButton
    Protected WithEvents ucHeader As Header
    Protected WithEvents txtOrtDatum As System.Web.UI.WebControls.TextBox
    Private objHaendler As FDD_Haendler

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

        'GetAppIDFromQueryString(Me)
        objHaendler = CType(Session("objHaendler"), FDD_Haendler)

        Table2.Visible = False
        m_User = GetUser(Me)
        FormAuth(Me, m_User)



        Dim view As DataView
        view = objHaendler.Fahrzeuge.DefaultView ' CType(Session("ResultTableRaw"), DataView)

        'For Each row In view.Table.Rows
        '    row("ZZFAHRG") = "-" & Right(row("ZZFAHRG").ToString.Trim, 5).ToString
        'Next

        DataGrid1.DataSource = view
        DataGrid1.DataBind()
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Antrag.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 30.04.09   Time: 14:43
' Updated in $/CKAG/Applications/appffd/Forms
' ITA: 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:23
' Created in $/CKAG/Applications/appffd/Forms
' 
' *****************  Version 6  *****************
' User: Uha          Date: 2.07.07    Time: 12:52
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 25.06.07   Time: 16:18
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' GetAppIDFromQueryString(Me) rausgenommen
' 
' *****************  Version 4  *****************
' User: Uha          Date: 7.03.07    Time: 12:51
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' 
' ************************************************
