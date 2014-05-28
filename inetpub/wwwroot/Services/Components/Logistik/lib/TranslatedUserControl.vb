Imports CKG.Base.Kernel.Common

Public MustInherit Class TranslatedUserControl
    Inherits UserControl

    Protected Overrides Sub OnPreRender(e As System.EventArgs)
        MyBase.OnPreRender(e)

        If (Not Page Is Nothing AndAlso Not Session Is Nothing) Then
            Dim translations As DataTable = Common.GetTranslations(Page)
            If Not translations Is Nothing AndAlso translations.Rows.Count > 0 Then
                For Each childControl As Control In Controls
                    Common.CheckAllControl(childControl, translations)
                Next
            End If
        End If
    End Sub
End Class
