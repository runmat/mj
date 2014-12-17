Imports CKG.Base.Kernel.Common.Common

Partial Public Class Change10
    Inherits Page
    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
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

    Protected Sub btnNewModelId_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNewModelId.Click
        DoSubmit(True)
    End Sub

#Region "Methods"

    Private Sub DoSubmit(Optional ByVal newModelId As Boolean = False)
        Session("lnkExcel") = ""

        Dim objECModelID As New ecModelID(m_User, m_App, "")
        Try

            If Not newModelId AndAlso String.IsNullOrEmpty(txtModelID.Text) Then
                lblError.Text = "Bitte geben Sie eine Model-ID ein!"
                Return
            End If

            If newModelId Then

                objECModelID.ModelID = ""
                objECModelID.VerarbeitungsKz = "N"
                objECModelID.Gesamt = False
                objECModelID.LoadHerstellerIds()

            Else

                objECModelID.ModelID = txtModelID.Text
                objECModelID.VerarbeitungsKz = "U"
                objECModelID.Gesamt = (rblGesamt.SelectedValue = "Ja")
                objECModelID.Show(Me)

            End If

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