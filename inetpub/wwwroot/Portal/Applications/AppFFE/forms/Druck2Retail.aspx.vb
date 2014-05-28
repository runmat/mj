Imports CKG.Base.Business
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements

Partial Public Class Druck2Retail
    Inherits System.Web.UI.Page

    Private objFDDBank2 As FFE_Bank_2
    Private objFDDBank As FFE_BankBase
    Private objSuche As FFE_Search
    Private m_User As Base.Kernel.Security.User

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Hier Benutzercode zur Seiteninitialisierung einfügen
        m_User = GetUser(Me)
        FormAuth(Me, m_User)


        objFDDBank2 = CType(Session("objFDDBank2"), FFE_Bank_2)
        objFDDBank = CType(Session("objFDDBank"), FFE_BankBase)
        objSuche = CType(Session("objSuche"), FFE_Search)


        If objFDDBank.Kontingente.Rows(2)("Kreditkontrollbereich") = "0003" Then
            txtA001.Text = objFDDBank.Kontingente.Rows(2)("Kontingentart")
            txtF001.Text = objFDDBank.Kontingente.Rows(2)("Frei")
            txtK001.Text = objFDDBank.Kontingente.Rows(2)("Kontingent_Neu")
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
' User: Rudolpho     Date: 15.05.08   Time: 14:33
' Updated in $/CKAG/Applications/AppFFE/forms
' ITA:1865
' 
' ************************************************
