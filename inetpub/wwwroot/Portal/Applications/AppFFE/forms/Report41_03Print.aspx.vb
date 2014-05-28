Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business
Imports System.Data.SqlClient.SqlDataReader
Imports System.Web.UI.WebControls.Label
Imports System
Imports System.IO


Partial Public Class Report41_03Print
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
            Dim tmpRow As DataRow
            Dim iCount As Integer = 1
            For Each tmpRow In objSuche.SearchResult.Rows
                Dim lblDyn2 As New Label()
                lblDyn2.Text = Replace(tmpRow("DISPLAY_ADDRESS").ToString, ",", "<br>")
                Dim NewRow As New HtmlControls.HtmlTableRow()
                Dim NewCell As New HtmlControls.HtmlTableCell()
                Dim i As Integer = NewCell.Style.Count

                NewCell.Style.Add("font-size", "12")
                NewCell.Style.Add("font-family", "Arial")
                i = NewCell.Style.Count
                Dim iRow As Integer = AddressTable.Rows.Count
                If iRow = 0 Then
                    NewRow.ID = "TR" & iCount
                    NewCell.ID = "TD" & iCount
                    NewCell.Controls.Add(lblDyn2)
                    NewRow.Cells.Add(NewCell)
                    AddressTable.Rows.Add(NewRow)
                    iCount += 1
                ElseIf iRow > 0 AndAlso AddressTable.Rows(iRow - 1).Cells.Count = 0 Then
                    NewCell.ID = "TD" & iCount
                    NewCell.Controls.Add(lblDyn2)
                    AddressTable.Rows(iRow - 1).Cells.Add(NewCell)
                    iCount += 1
                ElseIf iRow > 0 AndAlso AddressTable.Rows(iRow - 1).Cells.Count = 1 Then
                    NewCell.ID = "TD" & iCount
                    NewCell.Controls.Add(lblDyn2)
                    AddressTable.Rows(iRow - 1).Cells.Add(NewCell)
                    iCount += 1
                ElseIf iRow > 0 AndAlso AddressTable.Rows(iRow - 1).Cells.Count = 2 Then
                    NewRow.ID = "TR" & iCount
                    NewCell.ID = "TD" & iCount
                    NewCell.Controls.Add(lblDyn2)
                    NewRow.Cells.Add(NewCell)
                    AddressTable.Rows.Add(NewRow)
                    iCount += 1
                End If
            Next
        End If
    End Sub

End Class
' ************************************************
' $History: Report41_03Print.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 15.05.08   Time: 14:33
' Updated in $/CKAG/Applications/AppFFE/forms
' ITA:1865
' 
' ************************************************
