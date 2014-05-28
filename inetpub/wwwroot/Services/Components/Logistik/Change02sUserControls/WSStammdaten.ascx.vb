Option Explicit On
Option Strict On

Imports CKG.Base.Kernel.Common

Public Class WSStammdaten
    Inherits WSBase

    Protected Overrides Sub OnLoad(e As System.EventArgs)
        MyBase.OnLoad(e)

        Dim translations = Common.GetTranslations(Page)

        Dim rows = translations.Select("FieldType='tr'")

        For Each row As DataRow In rows
            Dim visible = CBool(row("Visibility"))
            Dim name = CStr(row("FieldName"))
            Dim displayName = CStr(row("Content"))

            Dim cws = cwcSteps.Steps.FirstOrDefault(Function(s) s.Title = name)

            If cws Is Nothing Then
                name = CStr(row("Content"))
                cws = cwcSteps.Steps.FirstOrDefault(Function(s) s.Title = name)
            End If

            If cws IsNot Nothing Then
                If visible Then
                    cws.Title = displayName
                Else
                    cwcSteps.Steps.Remove(cws)
                End If
            End If

        Next

    End Sub
End Class
