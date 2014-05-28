Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common

Partial Public Class Change10
    Inherits System.Web.UI.Page
    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten."
        End Try
    End Sub

    Protected Sub btnConfirm_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConfirm.Click
        DoSubmit()
    End Sub

#Region "Methods"

    Private Sub DoSubmit(Optional ByVal Fahrgestellnummer As String = "")
        Session("lnkExcel") = ""
        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"

        Dim objECModelID As New ecModelID(m_User, m_App, "")
        Try


            If txtModelID.Text.Length > 0 Then
                objECModelID.ModelID = txtModelID.Text
            Else
                lblError.Text = "Bitte geben Sie eine Model-ID ein!"
                Return
            End If


            objECModelID.Gesamt = rbAktion.SelectedValue


            objECModelID.Show(Me)

            If objECModelID.Message.Length > 0 Then
                lblError.Text = objECModelID.Message
            Else
                Session("App_ChangeModelID") = objECModelID
                Response.Redirect("Change10_2.aspx?AppID=" & Session("AppID").ToString)
            End If



        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten."
        End Try
    End Sub
#End Region
End Class