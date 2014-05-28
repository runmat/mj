Option Strict On
Option Explicit On

Public Class WSDienstleistungen
    Inherits WSBase

    Private _fzg1 As Fahrzeug
    Private _fzg2 As Fahrzeug

    Protected ReadOnly Property TransferPage As ITransferPage
        Get
            Return DirectCast(Me.Page, ITransferPage)
        End Get
    End Property

    Protected Overrides Sub OnInit(e As System.EventArgs)
        MyBase.OnInit(e)
        ' Greife auf cwSteps.Controls zu um das Erzeugen der einzelnen Schritten
        ' zu erzwingen. Sie können nun selber auf PropertChanged reagieren
        Dim i As Integer = Me.cwcSteps.Controls.Count
        AddHandler Me.TransferPage.Dal.PropertyChanged, AddressOf Me.OnDalPropertyChanged
        Me.AddFzgPropertyChanged(Me._fzg1, Me.TransferPage.Dal.Fzg1)
        Me.AddFzgPropertyChanged(Me._fzg2, Me.TransferPage.Dal.Fzg2)
    End Sub

    Private Sub OnDalPropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs)
        If e.PropertyName.Equals("Fzg1", StringComparison.Ordinal) Then
            Me.RemoveFzgPropertyChanged(Me._fzg1)
            Me.AddFzgPropertyChanged(Me._fzg1, Me.TransferPage.Dal.Fzg1)
        ElseIf e.PropertyName.Equals("Fzg2", StringComparison.Ordinal) Then
            Me.cwcSteps.Steps(3).Enabled = Me.TransferPage.Dal.Fzg2 IsNot Nothing
            Me.RemoveFzgPropertyChanged(Me._fzg2)
            Me.AddFzgPropertyChanged(Me._fzg2, Me.TransferPage.Dal.Fzg2)
        End If
    End Sub

    Private Sub AddFzgPropertyChanged(ByRef fzg As Fahrzeug, newFzg As Fahrzeug)
        fzg = newFzg

        If fzg IsNot Nothing Then
            AddHandler fzg.PropertyChanged, AddressOf Me.OnFzgPropertyChanged
        End If
    End Sub

    Private Sub RemoveFzgPropertyChanged(ByRef fzg As Fahrzeug)
        If fzg IsNot Nothing Then
            RemoveHandler fzg.PropertyChanged, AddressOf Me.OnFzgPropertyChanged
            fzg = Nothing
        End If
    End Sub

    Private Sub OnFzgPropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs)
        Dim fzg As Fahrzeug = DirectCast(sender, Fahrzeug)
        If e.PropertyName.Equals("Zusatzfahrten", StringComparison.Ordinal) Then
            Dim any As Boolean = fzg.Zusatzfahrten.Any()
            If Me._fzg1.Equals(fzg) Then
                Me.cwcSteps.Steps(0).Enabled = any
            Else
                Me.cwcSteps.Steps(2).Enabled = any
            End If
        End If
    End Sub

    Protected Overrides Sub OnUnload(e As System.EventArgs)
        Me.RemoveFzgPropertyChanged(Me._fzg2)
        Me.RemoveFzgPropertyChanged(Me._fzg1)
        RemoveHandler Me.TransferPage.Dal.PropertyChanged, AddressOf Me.OnDalPropertyChanged

        MyBase.OnUnload(e)
    End Sub
End Class