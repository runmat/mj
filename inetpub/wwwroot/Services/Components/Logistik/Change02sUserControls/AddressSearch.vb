Imports System.ComponentModel

Public Class AddressSearch
    Inherits Controls.AddressSearch

    Public Const Abholadresse As String = "Abholadresse"
    Public Const Auslieferung As String = "Auslieferung"
    Public Const Halter As String = "Halter"

    Private transferPage As ITransferPage

    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Property Address As Adresse

    Protected Overrides Sub OnInit(e As System.EventArgs)
        MyBase.OnInit(e)

        Me.transferPage = DirectCast(Me.Page, ITransferPage)
    End Sub

    Protected Overrides Function DoSearch(addressType As String, name As String, postcode As String, town As String) As System.Data.DataTable
        Me.transferPage.Transfer.FillAdressen(Me.transferPage.CSKUser, Me.Page, addressType, name, postcode, town, Me.transferPage.GetKundennummer())
        Select Case addressType
            Case AddressSearch.Abholadresse
                Return Me.transferPage.Transfer.Abholadresse
            Case AddressSearch.Auslieferung
                Return Me.transferPage.Transfer.Zieladresse
            Case AddressSearch.Halter
                Return Me.transferPage.Transfer.Halter
            Case Else
                Return Nothing
        End Select
    End Function

    Protected Overrides Sub ExtractData(addressRow As System.Data.DataRow)
        Me.Address = New Adresse() With
                     {
                        .DebitorNr = addressRow.Field(Of String)("KUNNR"),
                        .Ansprechpartner = addressRow.Field(Of String)("Name2"),
                        .Name = addressRow.Field(Of String)("NAME1"),
                        .Ort = addressRow.Field(Of String)("ORT01"),
                        .Postleitzahl = addressRow.Field(Of String)("PSTLZ"),
                        .Straße = addressRow.Field(Of String)("STRAS"),
                        .Telefon = addressRow.Field(Of String)("TELNR"),
                        .Land = addressRow.Field(Of String)("LAND1"),
                        .Carport = addressRow.Field(Of String)("POS_TEXT"),
                        .EMail = addressRow.Field(Of String)("Email")
                    }
    End Sub

End Class
