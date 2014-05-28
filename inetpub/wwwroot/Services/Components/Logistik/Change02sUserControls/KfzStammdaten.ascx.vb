Option Explicit On
Option Strict On

Public Class KfzStammdaten
    Inherits TranslatedUserControl
    Private Shared ReadOnly EventFillData As New Object

    Public Property ValidationGroup As String
        Get
            Return Me.rfvFahrgestellnummer.ValidationGroup
        End Get
        Set(value As String)
            Me.rfvFahrgestellnummer.ValidationGroup = value
            Me.rfvZugelassen.ValidationGroup = value
            Me.rfvBeauftragt.ValidationGroup = value
            Me.rfvTyp.ValidationGroup = value
            Me.rfvBereifung.ValidationGroup = value
            Me.rfvFahrzeugklasse.ValidationGroup = value
            Me.rfvFahrzeugwert.ValidationGroup = value
        End Set
    End Property

    Public Property Fahrzeug As Fahrzeug
        Get
            Return New Fahrzeug() With
                {
                    .Fahrgestellnummer = Me.txtFahrgestellnummer.Text.Trim(),
                    .Zugelassen = Me.rblZugelassen.SelectedValue,
                    .Kennzeichen = Me.txtKennzeichen1.Text.Trim(),
                    .Beauftragt = Me.rblBeauftragt.SelectedValue,
                    .Typ = Me.txtTyp.Text.Trim(),
                    .Bereifung = Me.rblBereifung.SelectedValue,
                    .Referenznummer = Me.txtReferenznummer.Text.Trim(),
                    .Klasse = Me.rblFahrzeugklasse.SelectedValue,
                    .Wert = Me.drpFahrzeugwert.SelectedValue
                }
        End Get
        Set(value As Fahrzeug)
            Me.txtFahrgestellnummer.Text = value.Fahrgestellnummer
            Me.rblZugelassen.SelectedValue = value.Zugelassen
            Me.txtKennzeichen1.Text = value.Kennzeichen
            Me.rblBeauftragt.SelectedValue = value.Beauftragt
            Me.txtTyp.Text = value.Typ
            Me.rblBereifung.SelectedValue = value.Bereifung
            Me.txtReferenznummer.Text = value.Referenznummer
            Me.rblFahrzeugklasse.SelectedValue = value.Klasse
            Me.drpFahrzeugwert.SelectedValue = value.Wert
        End Set
    End Property

    Public Custom Event FillData As EventHandler
        AddHandler(value As EventHandler)
            Me.Events.AddHandler(KfzStammdaten.EventFillData, value)
        End AddHandler
        RemoveHandler(value As EventHandler)
            Me.Events.RemoveHandler(KfzStammdaten.EventFillData, value)
        End RemoveHandler
        RaiseEvent(sender As Object, e As EventArgs)
            Dim eh As EventHandler = DirectCast(Me.Events(KfzStammdaten.EventFillData), EventHandler)

            If eh IsNot Nothing Then
                eh(sender, e)
            End If
        End RaiseEvent
    End Event

    Protected Sub OnErgänzenClick(sender As Object, e As EventArgs)
        RaiseEvent FillData(Me, EventArgs.Empty)

        txtFahrgestellnummer.Focus()
    End Sub
End Class