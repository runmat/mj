Option Strict On
Option Explicit On

Partial Public Class KBSMenue
    Inherits System.Web.UI.UserControl



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Me.IsPostBack = False Then

            Dim objKasse As Kasse = CType(Page.Session("mKasse"), Kasse)

            Dim Applications As DataView = New DataView(objKasse.Applications)

            'Nur "Superadmin"-User dürfen Anwendungen mit Autorisierungslevel >0 sehen/nutzen
            If Not objKasse.Master Then
                Applications.RowFilter = "ISNULL(AuthorizationLevel, 0) = 0"
            End If

            GridChange.DataSource = Applications
            GridChange.DataBind()
        End If
    End Sub

    Private Sub GridChange_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridChange.RowCommand
        If e.CommandName = "goTo" Then
            Response.Redirect("/KBS/Forms/" & e.CommandArgument.ToString)
        End If
    End Sub
End Class

' ************************************************
' $History: KBSMenue.ascx.vb $
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 12.02.10   Time: 16:29
' Updated in $/CKAG2/KBS/controls
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 10.02.10   Time: 17:53
' Updated in $/CKAG2/KBS/controls
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 30.04.09   Time: 11:44
' Updated in $/CKAG2/KBS/controls
' ITA 2838 unfertig
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 24.04.09   Time: 15:48
' Updated in $/CKAG2/KBS/controls
' ITA 2808
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 24.04.09   Time: 10:35
' Updated in $/CKAG2/KBS/controls
' from server.transfer back wegen js
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 23.04.09   Time: 17:50
' Updated in $/CKAG2/KBS/controls
' ITA 2808
' 
'
' ************************************************