Imports CKG.Base.Business
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements

Public Class Druck1
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
    Protected WithEvents txtK002 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtI002 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtI003 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtF002 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtK004 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtI004 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtF004 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtK005 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtK003 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtI005 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtF005 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtK001 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtI001 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtF001 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtF003 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtA001 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtA002 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtA003 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtA004 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtA005 As System.Web.UI.WebControls.TextBox
    Protected WithEvents trRet As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trDP As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trHEZ As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents Textbox1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Textbox2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Textbox3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Textbox4 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Textbox5 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Textbox6 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Textbox7 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Textbox8 As System.Web.UI.WebControls.TextBox
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



        txtA001.Text = objFDDBank.Kontingente.Rows(0)("Kontingentart")
        txtA002.Text = objFDDBank.Kontingente.Rows(1)("Kontingentart")
        txtK001.Text = objFDDBank.Kontingente.Rows(0)("Kontingent_Neu")
        txtK002.Text = objFDDBank.Kontingente.Rows(1)("Kontingent_Neu")
        txtI001.Text = objFDDBank.Kontingente.Rows(0)("Ausschoepfung")
        txtI002.Text = objFDDBank.Kontingente.Rows(1)("Ausschoepfung")
        txtF001.Text = objFDDBank.Kontingente.Rows(0)("Frei")
        txtF002.Text = objFDDBank.Kontingente.Rows(1)("Frei")

        If objFDDBank.Kontingente.Rows(2)("Kreditkontrollbereich") = "0003" Then
            txtA003.Text = objFDDBank.Kontingente.Rows(2)("Kontingentart")
            txtK003.Text = objFDDBank.Kontingente.Rows(2)("Richtwert_Alt")
            'txtF003.Text = objFDDBank.Kontingente.Rows(2)("Frei")
            txtI003.Text = objFDDBank.Kontingente.Rows(2)("Ausschoepfung")

        End If

        If objFDDBank.Kontingente.Rows.Count = 4 Then
            If objFDDBank.Kontingente.Rows(3)("Kreditkontrollbereich") = "0004" Then
                'txtF004.Text = objFDDBank.Kontingente.Rows(3)("Frei")
                txtI004.Text = objFDDBank.Kontingente.Rows(3)("Ausschoepfung")
                txtK004.Text = objFDDBank.Kontingente.Rows(3)("Richtwert_Neu")
                txtA004.Text = objFDDBank.Kontingente.Rows(3)("Kontingentart")
            End If
            'HEZ
            If objFDDBank.Kontingente.Rows(3)("Kreditkontrollbereich") = "0005" Then
                'txtF005.Text = objFDDBank.Kontingente.Rows(3)("Frei")
                txtI005.Text = objFDDBank.Kontingente.Rows(3)("Ausschoepfung")
                txtK005.Text = objFDDBank.Kontingente.Rows(3)("Richtwert_Neu")
                txtA005.Text = objFDDBank.Kontingente.Rows(3)("Kontingentart")
            End If
        End If
        If objFDDBank.Kontingente.Rows.Count = 5 Then
            'DP
            txtF004.Text = objFDDBank.Kontingente.Rows(3)("Frei")
            txtI004.Text = objFDDBank.Kontingente.Rows(3)("Ausschoepfung")
            txtK004.Text = objFDDBank.Kontingente.Rows(3)("Richtwert_Neu")
            txtA004.Text = objFDDBank.Kontingente.Rows(3)("Kontingentart")
            'HEZ
            txtI005.Text = objFDDBank.Kontingente.Rows(4)("Ausschoepfung")
            txtF005.Text = objFDDBank.Kontingente.Rows(4)("Frei")
            txtK005.Text = objFDDBank.Kontingente.Rows(4)("Richtwert_Neu")
            txtA005.Text = objFDDBank.Kontingente.Rows(4)("Kontingentart")
        End If

        'If CType(objFDDBank.Kontingente.Rows(2)("ZeigeKontingentart"), Boolean) Then
        '    trDP.Visible = True
        'End If

        'If CType(objFDDBank.Kontingente.Rows(3)("ZeigeKontingentart"), Boolean) Then
        '    trHEZ.Visible = True
        'End If


        txtUser.Text = m_User.UserName.ToString
        txtNr.Text = objSuche.REFERENZ
        txtName.Text = objSuche.NAME
        txtAdresse.Text = objSuche.STREET & ", " & objSuche.POSTL_CODE & " " & objSuche.CITY

        Dim view As DataView
        view = objFDDBank2.Auftraege.DefaultView ' CType(Session("ResultTableRaw"), DataView)
        'view.RowFilter = "Initiator <> ''"

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
' $History: Druck1.aspx.vb $
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 1.06.10    Time: 13:43
' Updated in $/CKAG/Applications/appffd/Forms
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 9.07.09    Time: 11:18
' Updated in $/CKAG/Applications/appffd/Forms
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 26.06.09   Time: 13:41
' Updated in $/CKAG/Applications/appffd/Forms
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 26.06.09   Time: 13:33
' Updated in $/CKAG/Applications/appffd/Forms
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 26.06.09   Time: 10:33
' Updated in $/CKAG/Applications/appffd/Forms
' ITA: 2918
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
' User: Rudolpho     Date: 14.06.07   Time: 14:40
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' Abgleich Portal - Startapplication 14.06.2007
' 
' *****************  Version 4  *****************
' User: Uha          Date: 7.03.07    Time: 12:51
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' 
' ************************************************
