Option Strict On
Option Explicit On

Imports System.Xml.Serialization

Namespace SilverDAT
    <XmlRoot("MinimalEvaluate")> _
    Public NotInheritable Class MinimalEvaluate
        <XmlElement("keyKba")> _
        Public Property KeyKba As String
        <XmlElement("keyDatEuropa")> _
        Public Property KeyDatEuropa As String
        <XmlElement("kindOfTaxiation")> _
        Public Property KindOfTaxation As String
        <XmlElement("keyMa")> _
        Public Property KeyMa As String
        <XmlElement("dealerSalesPriceNet")> _
        Public Property DealerSalesPriceNet As Decimal
        <XmlElement("dealerPurchasePriceNet")> _
        Public Property DealerPurchasePriceNet As Decimal
        <XmlElement("dealerPurchasePriceGross")> _
        Public Property DealerPurchasePriceGross As Decimal
        <XmlElement("dealerSalesPriceGross")> _
        Public Property DealerSalesPriceGross As Decimal

        Public Shared Function FromString(minimalEvaluate As String) As MinimalEvaluate
            Dim ser As New XmlSerializer(GetType(MinimalEvaluate))
            Using sr As New System.IO.StringReader(minimalEvaluate)
                Return DirectCast(ser.Deserialize(sr), MinimalEvaluate)
            End Using
        End Function
    End Class
End Namespace