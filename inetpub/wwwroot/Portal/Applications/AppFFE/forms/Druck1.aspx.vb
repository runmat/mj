Imports CKG.Base.Business
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements

Partial Public Class Druck1
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

        txtA001.Text = objFDDBank.Kontingente.Rows(0)("Kontingentart")
        txtA002.Text = objFDDBank.Kontingente.Rows(1)("Kontingentart")
        txtK001.Text = objFDDBank.Kontingente.Rows(0)("Kontingent_Neu")
        txtK002.Text = objFDDBank.Kontingente.Rows(1)("Kontingent_Neu")
        txtI001.Text = objFDDBank.Kontingente.Rows(0)("Ausschoepfung")
        txtI002.Text = objFDDBank.Kontingente.Rows(1)("Ausschoepfung")
        txtF001.Text = objFDDBank.Kontingente.Rows(0)("Frei")
        txtF002.Text = objFDDBank.Kontingente.Rows(1)("Frei")

        If objFDDBank.Kontingente.Rows.Count = 4 Then
            If objFDDBank.Kontingente.Rows(3)("Kreditkontrollbereich") = "0004" Then
                txtF004.Text = objFDDBank.Kontingente.Rows(3)("Frei")
                txtI004.Text = objFDDBank.Kontingente.Rows(3)("Ausschoepfung")
                txtK004.Text = objFDDBank.Kontingente.Rows(3)("Richtwert_Neu")
                txtA004.Text = objFDDBank.Kontingente.Rows(3)("Kontingentart")
            End If
            'HEZ
            If objFDDBank.Kontingente.Rows(3)("Kreditkontrollbereich") = "0005" Then
                txtF005.Text = objFDDBank.Kontingente.Rows(3)("Frei")
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
' *****************  Version 3  *****************
' User: Rudolpho     Date: 15.05.08   Time: 14:33
' Updated in $/CKAG/Applications/AppFFE/forms
' ITA:1865
' 
' ************************************************
