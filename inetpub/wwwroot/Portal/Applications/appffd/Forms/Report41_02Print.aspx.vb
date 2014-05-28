Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business
Imports System.Data.SqlClient.SqlDataReader
Imports System.Web.UI.WebControls.Label
Imports System
Imports System.IO

Public Class Report41_02Print
    Inherits System.Web.UI.Page

    Private objSuche As Search
    Private m_User As Base.Kernel.Security.User
    Private objApp As Base.Kernel.Security.App
    Private objFDDBank As BankBaseCredit
    Private objFDDBank2 As FDD_Bank_2
    Private objFDDBank3 As FDD_Zahlungsfrist
    Private objDistrikt As FFD_Bank_Distrikt
    Private objtblKontingente As DataTable
    Private m_strMessage As String
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles

    Protected WithEvents txtUser As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtK001 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtI001 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtF001 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Textbox1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtK002 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtI002 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtF002 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Textbox2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtA001 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtA002 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtNr As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAdresse As System.Web.UI.WebControls.TextBox
    Protected WithEvents datseit As System.Web.UI.WebControls.TextBox
    Protected WithEvents datLastChange As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDistrikt As System.Web.UI.WebControls.TextBox
    Protected WithEvents Textbox10 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Textbox14 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Textbox15 As System.Web.UI.WebControls.TextBox
    Protected WithEvents A003 As System.Web.UI.WebControls.TextBox
    Protected WithEvents R003 As System.Web.UI.WebControls.TextBox
    Protected WithEvents I003 As System.Web.UI.WebControls.TextBox
    Protected WithEvents A004 As System.Web.UI.WebControls.TextBox
    Protected WithEvents R004 As System.Web.UI.WebControls.TextBox
    Protected WithEvents I004 As System.Web.UI.WebControls.TextBox
    Protected WithEvents chkLastschrift As System.Web.UI.WebControls.CheckBox
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid

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
        m_User = GetUser(Me)
        FormAuth(Me, m_User)
        If Not Session("objSuche") Is Nothing Then
            objSuche = Session("objSuche")
            txtUser.Text = m_User.UserName & " " & Date.Today.ToShortDateString
            txtNr.Text = objSuche.REFERENZ
            txtName.Text = objSuche.NAME
            txtAdresse.Text = objSuche.STREET & ", " & objSuche.POSTL_CODE & " " & objSuche.CITY
        End If
        If Not Session("App_objDistrikt") Is Nothing Then
            objDistrikt = Session("App_objDistrikt")
            txtDistrikt.Text = objDistrikt.Distrikt
        End If
        If Not Session("objFDDBank3") Is Nothing Then
            objFDDBank3 = Session("objFDDBank3")
            DataGrid1.DataSource = objFDDBank3.Zahlungsfristen
            DataGrid1.DataBind()
        End If
        If Not Session("AppLastschrift") Is Nothing Then
            chkLastschrift.Checked = Session("AppLastschrift")
        End If

        If Not Session("App_GridGesamt") Is Nothing Then
            Dim tempDGrid As New DataGrid()
            Dim item As DataGridItem

            tempDGrid = Session("App_GridGesamt")
            For Each item In tempDGrid.Items
                Textbox14.Text = item.Cells(1).Text                            'Spalte Kontingent
                Textbox15.Text = item.Cells(2).Text                              'Spalte Kontingent
            Next
        End If
        If Not Session("App_GridKont1") Is Nothing Then
            Dim tempDGrid As New DataGrid()
            Dim item As DataGridItem
            Dim cell As TableCell
            Dim chkBox As CheckBox
            Dim label As Label
            Dim control As Control
            Dim icount As Integer = 0
            tempDGrid = Session("App_GridKont1")
            For Each item In tempDGrid.Items
                If icount = 1 Then
                    txtA002.Text = item.Cells(1).Text
                Else
                    txtA001.Text = item.Cells(1).Text
                End If


                cell = item.Cells(2)                            'Spalte Kontingent
                For Each control In cell.Controls
                    If TypeOf control Is Label Then
                        label = CType(control, Label)
                        If label.ID = "Label1" Then       'Label1 = nimmt Wert für Kontingent_Alt auf
                            If icount = 1 Then
                                txtK002.Text = label.Text
                            Else
                                txtK001.Text = label.Text
                            End If
                        End If
                    End If
                Next

                If icount = 1 Then                              'Spalte Inanspruchnahme
                    txtI002.Text = item.Cells(3).Text
                Else
                    txtI001.Text = item.Cells(3).Text
                End If

                cell = item.Cells(4)                            'Spalte Kontingent
                For Each control In cell.Controls
                    If TypeOf control Is Label Then
                        label = CType(control, Label)
                        If label.ID = "Label3" Then
                            If icount = 1 Then
                                txtF002.Text = label.Text
                            Else
                                txtF001.Text = label.Text
                            End If
                        End If
                    End If
                Next
                cell = item.Cells(5)
                For Each control In cell.Controls
                    If TypeOf control Is CheckBox Then
                        chkBox = CType(control, CheckBox)
                        If chkbox.Checked = True Then
                            If icount = 1 Then
                                Textbox1.Text = "X"
                            Else
                                Textbox2.Text = "X"
                            End If

                        End If
                    End If
                Next
                icount += 1
            Next
        End If
        If Not Session("App_GridKont3") Is Nothing Then
            Dim tempDGrid As New DataGrid()
            Dim item As DataGridItem
            Dim cell As TableCell
            Dim label As Label
            Dim control As Control
            Dim icount As Integer = 0
            tempDGrid = Session("App_GridKont3")
            For Each item In tempDGrid.Items
                If icount = 1 Then
                    A004.Text = item.Cells(1).Text
                Else
                    A003.Text = item.Cells(1).Text
                End If

                cell = item.Cells(2)
                For Each control In cell.Controls
                    If TypeOf control Is Label Then
                        label = CType(control, Label)
                        If label.ID = "Label2" Then
                            If icount = 1 Then
                                R004.Text = label.Text
                            Else
                                R003.Text = label.Text
                            End If
                        End If
                    End If
                Next
                If icount = 1 Then
                    I004.Text = item.Cells(3).Text
                Else
                    I003.Text = item.Cells(3).Text
                End If
                icount += 1
            Next
        End If

    End Sub

End Class
