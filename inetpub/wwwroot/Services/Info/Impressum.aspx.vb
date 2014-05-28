Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Partial Public Class Impressum
    Inherits System.Web.UI.Page

    Private m_User As Security.User

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("objUser") Is Nothing Then
            m_User = CType(Session("objUser"), Security.User)
            If m_User.Customer.AccountingArea = "1510" Then
                ImpressumAG.Visible = False
                ImpressumDAD.Visible = True

            Else
                ImpressumAG.Visible = True
                ImpressumDAD.Visible = False
            End If
        End If
        Me.Title = "Impressum"
    End Sub

End Class