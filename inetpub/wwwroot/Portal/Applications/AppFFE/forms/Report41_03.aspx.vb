Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business
Imports System.Data.SqlClient.SqlDataReader
Imports System.Web.UI.WebControls.Label
Imports System
Imports System.IO

Partial Public Class Report41_03
    Inherits System.Web.UI.Page

    Private objSuche As FFE_Search
    Private m_User As Base.Kernel.Security.User
    Private objApp As Base.Kernel.Security.App
    Private objFDDBank As FFE_BankBase
    Private objFDDBank2 As FFE_Bank_2
    Private objFDDBank3 As FFE_Zahlungsfrist

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        Try
            If Not Session("objsuche") Is Nothing AndAlso Not Session("SelectedDealer") Is Nothing Then
                objSuche = Session("objSuche")
                lblHDNummer.Text = Session("SelectedDealer")
                Dim strTemp As String = objSuche.NAME
                If objSuche.NAME_2.Length > 0 Then
                    strTemp &= "<br>" & objSuche.NAME_2
                End If
                lblName.Text = strTemp
                lblAdresse.Text = objSuche.COUNTRYISO & " - " & objSuche.POSTL_CODE & " " & objSuche.CITY & "<br>" & objSuche.STREET
                Dim tmpIntValue As Int32 = objSuche.LeseAdressenSAP(Session("AppID").ToString, Session.SessionID.ToString, Me.Page, m_User.Reference, lblHDNummer.Text)
                If tmpIntValue > 0 Then
                    Dim tmpRow As DataRow
                    Dim iCount As Integer = 1
                    For Each tmpRow In objSuche.SearchResult.Rows
                        Dim lblDyn2 As New Label()
                        lblDyn2.Text = Replace(tmpRow("DISPLAY_ADDRESS").ToString, ",", "<br>")
                        Dim NewRow As New HtmlControls.HtmlTableRow()
                        Dim NewCell As New HtmlControls.HtmlTableCell()
                        Dim i As Integer = NewCell.Style.Count
                        NewCell.Style.Add("font-weight", "bold")
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
                    Session("objSuche") = objSuche
                End If

            End If
        Catch ex As Exception
            lblError.Text = "Fehler bei der Ermittlung der Versandadressen"
        End Try
    End Sub

    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBack.Click
        Response.Redirect("Report41_02.aspx?AppID=" & Session("AppID").ToString)
    End Sub
End Class
' ************************************************
' $History: Report41_03.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 3.03.10    Time: 15:08
' Updated in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 15.05.08   Time: 14:33
' Updated in $/CKAG/Applications/AppFFE/forms
' ITA:1865
' 
' ************************************************
