Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common

Partial Public Class Change10_2
    Inherits System.Web.UI.Page
    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private objECModelID As ecModelID


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If Not Session("App_ChangeModelID") Is Nothing Then
                objECModelID = Session("App_ChangeModelID")
            Else
                lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten."
                Return
            End If

            If Not IsPostBack Then

                FillForm()

            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten."
        End Try
    End Sub
    Private Sub FillForm()
        lblModellID.Text = objECModelID.ModelID
        lblModellBez.Text = objECModelID.ModelName
        lblSippCode.Text = objECModelID.SippCode
        lblLaufzeit.Text = objECModelID.Laufzeit.TrimStart("0")
        txtModellBez.Text = objECModelID.ModelName
        txtSippcode.Text = objECModelID.SippCode
        txtLaufzeit.Text = objECModelID.Laufzeit.TrimStart("0")
        If objECModelID.LZBindung = "X" Then
            cbxLaufz.Checked = True
            cbxLaufzBindNeu.Checked = True
        End If
    End Sub
    Private Sub DoSubmit()
        Dim bChange As Boolean = False


        If txtModellBez.Text.Trim.Length + txtSippcode.Text.Trim.Length + txtLaufzeit.Text.Trim.Length = 0 Then
            lblError.Text = "Es wurden keine Änderungen vorgenommen!"
            Return
        End If
        objECModelID.NeuModelName = ""
        objECModelID.NeuSippCode = ""
        objECModelID.NeuLaufzeit = ""

        If objECModelID.ModelName <> txtModellBez.Text.Trim Then
            objECModelID.NeuModelName = txtModellBez.Text.Trim
            bChange = True
        Else
            objECModelID.NeuModelName = objECModelID.ModelName
        End If
        If objECModelID.SippCode <> txtSippcode.Text.Trim Then
            objECModelID.NeuSippCode = txtSippcode.Text.Trim
            bChange = True
        Else
            objECModelID.NeuSippCode = objECModelID.SippCode
        End If
        If objECModelID.Laufzeit.TrimStart("0") <> txtLaufzeit.Text.Trim Then
            objECModelID.NeuLaufzeit = txtLaufzeit.Text.Trim
            bChange = True
        Else
            objECModelID.NeuLaufzeit = objECModelID.Laufzeit
        End If
        Dim sLZBindung As String = ""
        If cbxLaufzBindNeu.Checked = True Then
            sLZBindung = "X"
            If objECModelID.NeuLaufzeit.Length = 0 Then
                lblError.Text = "Bitte geben Sie die Laufzeit an!"
                Return
            End If
        End If
        If objECModelID.LZBindung <> sLZBindung Then
            objECModelID.NeuLZBindung = sLZBindung
            bChange = True
        Else
            objECModelID.NeuLZBindung = objECModelID.LZBindung
        End If

        If bChange = False Then
            lblError.Text = "Es wurden keine Änderungen vorgenommen!"
        Else
            If objECModelID.NeuModelName.Length = 0 Then
                objECModelID.NeuModelName = objECModelID.ModelName
            End If
            If objECModelID.NeuSippCode.Length = 0 Then
                objECModelID.NeuSippCode = objECModelID.SippCode
            End If
            If objECModelID.NeuLaufzeit.Length = 0 Then
                objECModelID.NeuLaufzeit = objECModelID.Laufzeit
            End If

            objECModelID.Change(Session("AppID").ToString, Session.SessionID.ToString, Me)

            If objECModelID.Status > 0 Then
                lblError.Text = objECModelID.Message
            Else
                Session("App_ChangeModelID") = Nothing
                cmdCreate.Enabled = False
                lblMessage.Text = "Daten erfolgreich gespeichert!"
            End If
        End If
    End Sub

    Protected Sub cmdCreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCreate.Click
        Dosubmit()
    End Sub

    Protected Sub cmdBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdBack.Click
        Response.Redirect("Change10.aspx?AppID=" & Session("AppID").ToString)
    End Sub
End Class