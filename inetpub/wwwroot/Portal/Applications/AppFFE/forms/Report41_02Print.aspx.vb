Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business
Imports System.Data.SqlClient.SqlDataReader
Imports System.Web.UI.WebControls.Label
Imports System
Imports System.IO

Partial Public Class Report41_02Print
    Inherits System.Web.UI.Page

    Private objSuche As FFE_Search
    Private m_User As Base.Kernel.Security.User
    Private objApp As Base.Kernel.Security.App
    Private objFDDBank As FFE_BankBase
    Private objFDDBank2 As FFE_Bank_2
    Private objFDDBank3 As FFE_Zahlungsfrist
    Private objDistrikt As FFE_Bank_Distrikt
    Private objtblKontingente As DataTable
    Private m_strMessage As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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

                If icount = 1 Then
                    txtI002.Text = item.Cells(3).Text
                Else
                    txtI001.Text = item.Cells(3).Text
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
                If icount = 1 Then ' DP
                    A004.Text = item.Cells(1).Text
                ElseIf icount = 2 Then 'KF/KL
                    A006.Text = item.Cells(1).Text
                Else
                    A003.Text = item.Cells(1).Text  'Retail
                End If

                cell = item.Cells(2)
                For Each control In cell.Controls
                    If TypeOf control Is Label Then
                        label = CType(control, Label)
                        If label.ID = "Label2" Then
                            If icount = 1 Then
                                R004.Text = label.Text
                            ElseIf icount = 2 Then 'KF/KL
                                R006.Text = label.Text
                            Else
                                R003.Text = label.Text
                            End If
                        End If
                    End If
                Next
                If icount = 1 Then
                    I004.Text = item.Cells(3).Text
                ElseIf icount = 2 Then 'KF/KL
                    I006.Text = item.Cells(3).Text
                Else
                    I003.Text = item.Cells(3).Text
                End If
                icount += 1
            Next
        End If

    End Sub

End Class
' ************************************************
' $History: Report41_02Print.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 4.06.08    Time: 18:43
' Updated in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 15.05.08   Time: 14:33
' Updated in $/CKAG/Applications/AppFFE/forms
' ITA:1865
' 
' ************************************************
