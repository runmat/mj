Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business
Imports System.Data.SqlClient.SqlDataReader
Imports System.Web.UI.WebControls.Label
Imports System
Imports System.IO

Public Class Report41_03
    Inherits System.Web.UI.Page
    Private objSuche As Search
    Private m_User As Base.Kernel.Security.User
    Private objApp As Base.Kernel.Security.App
    Private objFDDBank As BankBaseCredit
    Private objFDDBank2 As FDD_Bank_2
    Private objFDDBank3 As FDD_Zahlungsfrist

    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSelect As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdBack As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblHDNummer As System.Web.UI.WebControls.Label
    Protected WithEvents lblAdresse As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
    Protected WithEvents trHaendlernummer As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trName As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trOrt As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents Tr1 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblName As System.Web.UI.WebControls.Label
    Protected WithEvents TD1 As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents TD2 As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents AddressTable As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents HyperLink1 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents trHdAuswahl As System.Web.UI.HtmlControls.HtmlTableRow

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
                Dim tmpIntValue As Int32 = objSuche.LeseAdressenSAP(lblHDNummer.Text)
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
