Imports CKG.Base.Business
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements

Public Class Druck2Retail
    Inherits System.Web.UI.Page

    Private objFDDBank2 As FDD_Bank_2
    Private objFDDBank As BankBaseCredit
    Private objSuche As Search
    Private m_User As Base.Kernel.Security.User
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents txtUser As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtNr As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAdresse As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtK001 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtI001 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtF001 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtA001 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Textbox1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Textbox5 As System.Web.UI.WebControls.TextBox
    Private m_App As Base.Kernel.Security.App

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
        ' Hier Benutzercode zur Seiteninitialisierung einfügen
        m_User = GetUser(Me)
        FormAuth(Me, m_User)

        objFDDBank2 = CType(Session("objFDDBank2"), FDD_Bank_2)
        objFDDBank = CType(Session("objFDDBank"), BankBaseCredit)
        objSuche = CType(Session("objSuche"), Search)


        If objFDDBank.Kontingente.Rows(2)("Kreditkontrollbereich") = "0003" Then
            txtA001.Text = objFDDBank.Kontingente.Rows(2)("Kontingentart")
            'txtK001.Text = objFDDBank.Kontingente.Rows(2)("Kontingent_Neu")
            'txtF001.Text = objFDDBank.Kontingente.Rows(2)("Frei")
            txtI001.Text = objFDDBank.Kontingente.Rows(2)("Ausschoepfung")
            txtK001.Text = objFDDBank.Kontingente.Rows(2)("Richtwert_Neu")
        End If

        txtUser.Text = m_User.UserName.ToString
        txtNr.Text = objSuche.REFERENZ
        txtName.Text = objSuche.NAME
        txtAdresse.Text = objSuche.STREET & ", " & objSuche.POSTL_CODE & " " & objSuche.CITY

        Dim view As DataView
        view = objFDDBank2.Auftraege.DefaultView ' CType(Session("ResultTableRaw"), DataView)


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
' $History: Druck2Retail.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 1.06.10    Time: 13:43
' Updated in $/CKAG/Applications/appffd/Forms
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
' *****************  Version 3  *****************
' User: Uha          Date: 2.07.07    Time: 12:52
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 19.06.07   Time: 14:36
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' 
' ************************************************
