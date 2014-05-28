Option Explicit On
Option Strict On

Public Class CWSKfz2
    Inherits CWSKfzBase

    Protected Overrides ReadOnly Property FahrzeugNummer As String
        Get
            Return "2"
        End Get
    End Property

    Protected Overrides ReadOnly Property ValidationGroup As String
        Get
            Return "CWSKfz2"
        End Get
    End Property

    Protected Overrides ReadOnly Property KfzControl As KfzStammdaten
        Get
            Return Me.ksRückholung
        End Get
    End Property

    Public Overrides Sub Validate()
        Dim kfz As Fahrzeug = Me.ksRückholung.Fahrzeug
        If kfz.Fahrgestellnummer.Length > 0 OrElse kfz.Kennzeichen.Length > 0 OrElse kfz.Typ.Length > 0 Then
            MyBase.Validate()
        End If
    End Sub
End Class