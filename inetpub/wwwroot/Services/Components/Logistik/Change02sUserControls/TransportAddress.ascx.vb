Option Strict On
Option Explicit On

Public Class ValidatePostcodeEventArgs
    Inherits EventArgs
    Private ReadOnly _postcode As String
    Private ReadOnly _country As String


    Public Sub New(postcode As String, country As String)
        Me._postcode = postcode
        Me._country = country
        Me.IsValid = True
    End Sub

    Public ReadOnly Property Postcode As String
        Get
            Return Me._postcode
        End Get
    End Property

    Public ReadOnly Property Country As String
        Get
            Return Me._country
        End Get
    End Property

    Public Property IsValid As Boolean
End Class

Public Class TransportAddress
    Inherits UserControl
    Private Shared ReadOnly EventValidatePostcode As New Object()

    Protected WithEvents UpdatePanel1 As UpdatePanel

    Protected ReadOnly Property TransferPage As ITransferPage
        Get
            Return DirectCast(Page, ITransferPage)
        End Get
    End Property

    Public Property HalterAdressSelector As Boolean
        Get
            Return Not Me.trAnsprechpartner.Visible
        End Get
        Set(value As Boolean)
            Me.trAnsprechpartner.Visible = Not value
            Me.trDatum.Visible = Not value
            Me.trEmail.Visible = Not value
            'Me.trPosText.Visible = Not value
            Me.trTelefon.Visible = Not value
            Me.trUhrzeit.Visible = Not value
            Me.ipUeberfuehrung.Visible = Not value
            Me.fahrzeugdaten.Visible = Not value
        End Set
    End Property

    Public Property ShowKmButton As Boolean
        Get
            Return ibKM.Visible
        End Get
        Set(value As Boolean)
            ibKM.Visible = value
            ModalOverlay2.Visible = value
        End Set
    End Property

    Public Property ShowSearch As Boolean
        Get
            Return Me.lbSearch.Visible
        End Get
        Set(value As Boolean)
            Me.lbSearch.Visible = value
            Me.ModalOverlay1.Visible = value
            Me.dummySearchButton.Enabled = value
        End Set
    End Property

    Public Property ShowTransportTypes As Boolean
        Get
            Return Me.cvTransporttyp.Enabled
        End Get
        Set(value As Boolean)
            Me.trTransporttyp.Visible = value
            Me.cvTransporttyp.Enabled = value
            Me.ipTransporttyp.Visible = value
            Me.filler.Visible = Not value
        End Set
    End Property

    Public Property ShowZusatzfahrtenCheckbox As Boolean
        Get
            Return Me.divZusatzfahrtCheckbox.Visible
        End Get
        Set(value As Boolean)
            Me.divZusatzfahrtCheckbox.Visible = value
        End Set
    End Property

    Public Property SearchType As String

    Public Property ValidationGroup As String
        Get
            Return Me.rfvAbFirma.ValidationGroup
        End Get
        Set(value As String)
            Me.cvTransporttyp.ValidationGroup = value
            Me.rfvAbFirma.ValidationGroup = value
            Me.rfvAbStrasse.ValidationGroup = value
            Me.rfvAbPLZ.ValidationGroup = value
            Me.cvAbPLZ.ValidationGroup = value
            Me.rfvAbOrt.ValidationGroup = value
            Me.rfvAbAnsprechpartner.ValidationGroup = value
            Me.rfvAbtelefon.ValidationGroup = value
        End Set
    End Property

    Public ReadOnly Property Adresse As Adresse
        Get
            Return New Adresse() With
                   {
                       .Ansprechpartner = Me.txtAbAnsprechpartner.Text,
                       .Name = Me.txtAbFirma.Text,
                       .Ort = Me.txtAbOrt.Text,
                       .Postleitzahl = Me.txtAbPLZ.Text,
                       .Straße = Me.txtAbStrasse.Text,
                       .Telefon = Me.txtAbTelefon.Text,
                       .EMail = Me.txtAbEMail.Text,
                       .Land = Me.ddlAbLand.SelectedValue
                   }
        End Get
    End Property

    Public Property Fahrt As Fahrt
        Get
            Dim txts = {txtAbAnsprechpartner, txtAbFirma, txtAbOrt, txtAbPLZ, txtAbStrasse, txtAbTelefon}
            Dim empty = txts.All(Function(txt) String.IsNullOrEmpty(txt.Text))

            If empty Then
                Return Nothing
            Else
                Dim parts As String() = Me.ddlAbUhrzeit.SelectedValue.Split("-"c)

                Return New Fahrt() With
                    {
                        .Datum = If(String.IsNullOrEmpty(Me.txtAbDatum.Text), DirectCast(Nothing, DateTime?), Convert.ToDateTime(Me.txtAbDatum.Text)),
                        .ZeitVon = If(parts(0) = "0", Nothing, parts(0)),
                        .ZeitBis = If(parts(1) = "0", Nothing, parts(1)),
                        .Transporttyp = If(Me.ShowTransportTypes, Me.ddlTransporttyp.SelectedItem.Text, String.Empty),
                        .TransporttypCode = If(Me.ShowTransportTypes, Me.ddlTransporttyp.SelectedItem.Value, String.Empty),
                        .Adresse = Adresse
                    }
            End If
        End Get

        Set(value As Fahrt)
            Dim a As Adresse = Nothing

            If value Is Nothing Then
                Me.txtAbDatum.Text = String.Empty
                Me.ddlAbUhrzeit.SelectedValue = "0-0"
                Me.ddlTransporttyp.SelectedValue = "00"
            Else
                Me.txtAbDatum.Text = If(value.Datum.HasValue, value.Datum.Value.ToShortDateString(), String.Empty)
                Me.ddlAbUhrzeit.SelectedValue = If(value.ZeitVon Is Nothing, "0-0", value.ZeitVon & "-" & value.ZeitBis)
                Me.ddlTransporttyp.SelectedValue = If(Me.ShowTransportTypes, value.TransporttypCode, "00")
                a = value.Adresse
            End If

            If a Is Nothing Then
                Me.txtAbAnsprechpartner.Text = String.Empty
                Me.txtAbFirma.Text = String.Empty
                Me.txtAbOrt.Text = String.Empty
                Me.txtAbPLZ.Text = String.Empty
                Me.txtAbStrasse.Text = String.Empty
                Me.txtAbTelefon.Text = String.Empty
                Me.txtAbEMail.Text = String.Empty
                Me.ddlAbLand.SelectedValue = "DE"
            Else
                Me.txtAbAnsprechpartner.Text = a.Ansprechpartner
                Me.txtAbFirma.Text = a.Name
                Me.txtAbOrt.Text = a.Ort
                Me.txtAbPLZ.Text = a.Postleitzahl
                Me.txtAbStrasse.Text = a.Straße
                Me.txtAbTelefon.Text = a.Telefon
                Me.txtAbEMail.Text = a.EMail
                Me.ddlAbLand.SelectedValue = a.Land
            End If
        End Set
    End Property

    Public ReadOnly Property MustInitDropdowns As Boolean
        Get
            Return Me.ddlAbLand.Items.Count = 0
        End Get
    End Property

    Public Custom Event ValidatePostcode As EventHandler(Of ValidatePostcodeEventArgs)
        AddHandler(value As EventHandler(Of ValidatePostcodeEventArgs))
            Me.Events.AddHandler(TransportAddress.EventValidatePostcode, value)
        End AddHandler

        RemoveHandler(value As EventHandler(Of ValidatePostcodeEventArgs))
            Me.Events.RemoveHandler(TransportAddress.EventValidatePostcode, value)
        End RemoveHandler

        RaiseEvent(sender As Object, e As ValidatePostcodeEventArgs)
            Dim eh As EventHandler(Of ValidatePostcodeEventArgs) = DirectCast(Me.Events(TransportAddress.EventValidatePostcode), EventHandler(Of ValidatePostcodeEventArgs))

            If eh IsNot Nothing Then
                eh(sender, e)
            End If
        End RaiseEvent
    End Event

    Protected Overrides Sub OnLoad(e As System.EventArgs)
        MyBase.OnLoad(e)
        Me.SetCountryWatermark()
    End Sub

    Private Sub SetCountryWatermark()
        If Me.ddlAbLand.Items.Count > 0 Then
            Me.ddlAbUhrzeit.Items(0).Attributes.Add("class", "Watermarked")
        End If
    End Sub

    Public Sub InitDropdowns(countries As IEnumerable, times As IEnumerable, Optional transportTypes As IEnumerable = Nothing)
        Me.ddlAbLand.DataSource = countries
        Me.ddlAbLand.DataBind()
        Me.ddlAbLand.SelectedValue = "DE"

        Me.ddlAbUhrzeit.DataSource = times
        Me.ddlAbUhrzeit.DataBind()

        If Me.ShowTransportTypes Then
            Me.ddlTransporttyp.DataSource = transportTypes
            Me.ddlTransporttyp.DataBind()
        End If
    End Sub

    Public Sub ShowVehicleDetails(kfz As Fahrzeug)
        If kfz IsNot Nothing Then
            Me.lblAbDetailKennzeichen.Text = kfz.Kennzeichen
            Me.lblAbDetailTyp.Text = kfz.Typ
            Me.lblAbDetailFin.Text = kfz.Fahrgestellnummer
        End If
    End Sub

    Private Sub FillAddress()
        Me.txtAbAnsprechpartner.Text = Me.AddressSearch1.Address.Ansprechpartner
        Me.txtAbFirma.Text = Me.AddressSearch1.Address.Name
        Me.txtAbOrt.Text = Me.AddressSearch1.Address.Ort
        Me.txtAbPLZ.Text = Me.AddressSearch1.Address.Postleitzahl
        Me.txtAbStrasse.Text = Me.AddressSearch1.Address.Straße
        Me.txtAbTelefon.Text = Me.AddressSearch1.Address.Telefon
        Me.txtAbEMail.Text = Me.AddressSearch1.Address.EMail
        Me.fAbDebitorNr.Value = Me.AddressSearch1.Address.DebitorNr

        Dim land As ListItem = Me.ddlAbLand.Items.FindByValue(Me.AddressSearch1.Address.Land)

        If land IsNot Nothing Then
            Me.ddlAbLand.SelectedValue = land.Value
        End If
        Dim CarportName As String = Me.AddressSearch1.Address.Carport

        Dim Transfer1 As Transfer = TransferPage.Transfer
        Dim returnMessage As String = ""
        Dim returnMessageErfolg As String = ""

        If Me.SearchType = "Abholadresse" Then
            Transfer1.CheckFahrzeugStandort(Page, CarportName, Me.lblAbDetailFin.Text, returnMessage, returnMessageErfolg)
        End If

        If (returnMessage = "") Then
            CarportmessageError.Visible = False
            CarportmessageError.Text = ""
        Else
            CarportmessageError.Visible = True
            CarportmessageError.Text = returnMessage
        End If

        If (returnMessageErfolg = "") Then
            CarportmessageErfolg.Visible = False
            CarportmessageErfolg.Text = ""
        Else
            CarportmessageErfolg.Visible = True
            CarportmessageErfolg.Text = returnMessageErfolg
        End If

        UpdatePanel1.Update()
    End Sub

    Protected Sub OnSearchClick(sender As Object, e As EventArgs)
        If Not Me.AddressSearch1.Search(Me.SearchType, Me.txtAbFirma.Text, Me.txtAbPLZ.Text, Me.txtAbOrt.Text) Then
            ' show overlay
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "SearchOverlay", ModalOverlay1.ShowOverlayClientScript, True)

            ModalOverlay1.Focus()
        Else
            FillAddress()
            ColorAnimationExtender1.StartAnimation()
        End If
    End Sub

    Protected Sub OnKmClick(sender As Object, e As EventArgs)
        If Not Page.IsValid Then Return

        Try
            ' get distances & bind gridview
            Dim transferPage = DirectCast(Me.Page, ITransferPage)
            Dim dal = transferPage.Dal
            dal.RefillSubmitData()

            Dim transfer = transferPage.Transfer

            Dim entfernungen = transfer.Adressen.Copy

            entfernungen.Columns.Add("KM", GetType(System.String))
            entfernungen.AcceptChanges()

            entfernungen.Rows(0)("KM") = " - "
            'entfernungen.DefaultView.RowFilter = "FAHRT="
            transfer.Entfernungen = entfernungen
            transfer.FillEntfernungen(transferPage.CSKUser, Me.Page)

            gvKilometer.DataSource = transfer.Entfernungen
            gvKilometer.DataBind()

            ' show overlay
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "SearchOverlay", ModalOverlay2.ShowOverlayClientScript, True)
        Catch ex As Exception
            System.Diagnostics.Trace.WriteLine(ex)
        End Try
    End Sub

    Protected Sub OnAddressSelected(sender As Object, e As EventArgs)
        ' hide overlay
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "HideSearchOverlay", ModalOverlay1.HideOverlayClientScript, True)
        Me.FillAddress()
        Me.txtAbFirma.Focus()
    End Sub

    Protected Sub OnValidatePostcode(sender As Object, e As ServerValidateEventArgs)
        Dim vpe As New ValidatePostcodeEventArgs(e.Value.Trim(), Me.ddlAbLand.SelectedValue)

        RaiseEvent ValidatePostcode(Me, vpe)

        e.IsValid = vpe.IsValid
    End Sub

End Class