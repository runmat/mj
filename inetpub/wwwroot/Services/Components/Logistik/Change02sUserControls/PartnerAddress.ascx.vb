Option Explicit On
Option Strict On

Public Class PartnerAddress
    Inherits TranslatedUserControl
    Private Shared ReadOnly EventAddressChanged As New Object()

    Public Property AddressListSource As DataView
    Public Property DisplayAddress As DataRow

    Public Custom Event SelectedAddressChanged As EventHandler
        AddHandler(value As EventHandler)
            Me.Events.AddHandler(PartnerAddress.EventAddressChanged, value)
        End AddHandler
        RemoveHandler(value As EventHandler)
            Me.Events.RemoveHandler(PartnerAddress.EventAddressChanged, value)
        End RemoveHandler
        RaiseEvent(sender As Object, e As EventArgs)
            Dim eh As EventHandler = DirectCast(Me.Events(PartnerAddress.EventAddressChanged), EventHandler)

            If eh IsNot Nothing Then
                eh(sender, e)
            End If
        End RaiseEvent
    End Event

    Public Property SelectedAddress As String
        Get
            Return Me.ddlPartnerRG.SelectedValue
        End Get
        Set(value As String)
            Me.ddlPartnerRG.SelectedValue = value
        End Set
    End Property

    Public Property ValidationGroup As String
        Get
            Return rfvPartnerRG.ValidationGroup
        End Get
        Set(value As String)
            rfvPartnerRG.ValidationGroup = value
        End Set
    End Property

    Protected Overrides Sub OnDataBinding(e As EventArgs)
        ddlPartnerRG.DataSource = AddressListSource

        If DisplayAddress Is Nothing Then
            SelectedAddress = String.Empty
            txtRzFirma.Text = String.Empty
            txtRzStrasse.Text = String.Empty
            txtRzPLZ.Text = String.Empty
            txtRzOrt.Text = String.Empty
            txtRzAnsprechpartner.Text = String.Empty
            txtRzTelefon.Text = String.Empty
        Else
            SelectedAddress = DirectCast(DisplayAddress("KUNNR"), String)
            txtRzFirma.Text = DirectCast(DisplayAddress("NAME1"), String)
            txtRzStrasse.Text = String.Format("{0} {1}", DirectCast(DisplayAddress("STREET"), String), DirectCast(DisplayAddress("HOUSE_NUM1"), String))
            txtRzPLZ.Text = DirectCast(DisplayAddress("POST_CODE1"), String)
            txtRzOrt.Text = DirectCast(DisplayAddress("CITY1"), String)
            txtRzAnsprechpartner.Text = DirectCast(DisplayAddress("NAME2"), String)
            txtRzTelefon.Text = DirectCast(DisplayAddress("TEL_NUMBER"), String)
        End If
    End Sub

    Protected Sub OnSelectedIndexChanged(sender As Object, e As EventArgs)
        RaiseEvent SelectedAddressChanged(Me, EventArgs.Empty)
    End Sub
End Class