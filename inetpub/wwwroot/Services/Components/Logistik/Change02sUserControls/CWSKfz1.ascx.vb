Option Explicit On
Option Strict On

Public Class CWSKfz1
    Inherits CWSKfzBase

    Protected Overrides ReadOnly Property FahrzeugNummer As String
        Get
            Return "1"
        End Get
    End Property

    Protected Overrides ReadOnly Property ValidationGroup As String
        Get
            Return "CWSKfz1"
        End Get
    End Property

    Protected Overrides ReadOnly Property KfzControl As KfzStammdaten
        Get
            Return Me.ksHinfahrt
        End Get
    End Property
End Class