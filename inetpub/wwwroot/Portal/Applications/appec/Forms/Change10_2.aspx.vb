Imports CKG.Base.Kernel.Common.Common

Partial Public Class Change10_2
    Inherits Page
    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private objECModelID As ecModelID

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
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

    Protected Sub cmdCreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCreate.Click
        Dosubmit()
    End Sub

    Protected Sub cmdBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdBack.Click
        Response.Redirect("Change10.aspx?AppID=" & Session("AppID").ToString)
    End Sub

#Region "Methods"

    Private Sub FillForm()
        With objECModelID

            Dim blnNeueModelId As Boolean = (.VerarbeitungsKz = "N")

            trAltNeu.Visible = Not blnNeueModelId
            trHersteller.Visible = blnNeueModelId
            txtModellId.Visible = blnNeueModelId
            requiredModelId.Visible = blnNeueModelId
            requiredBezeichnung.Visible = blnNeueModelId
            requiredSippCode.Visible = blnNeueModelId
            lblRequiredFieldsHint.Visible = blnNeueModelId
            lblModellID.Visible = Not blnNeueModelId
            lblModellBez.Visible = Not blnNeueModelId
            lblSippCode.Visible = Not blnNeueModelId
            lblLaufzeit.Visible = Not blnNeueModelId
            cbxLaufzBind.Visible = Not blnNeueModelId
            rblFzggruppeLkw.Visible = Not blnNeueModelId
            rblWinterreifen.Visible = Not blnNeueModelId
            rblAhk.Visible = Not blnNeueModelId
            rblNavi.Visible = Not blnNeueModelId
            rblSecuritiFleet.Visible = Not blnNeueModelId
            rblLeasing.Visible = Not blnNeueModelId

            If blnNeueModelId Then

                If .TblHerstellerIds IsNot Nothing Then
                    Dim tmpDv As New DataView(.TblHerstellerIds)
                    tmpDv.Sort = "HERST_T"
                    ddlHersteller.DataSource = tmpDv
                    ddlHersteller.DataTextField = "HERST_T"
                    ddlHersteller.DataValueField = "HERST_GROUP"
                    ddlHersteller.DataBind()
                End If

                txtModellId.Text = .NeuModelID

            Else

                lblModellID.Text = .ModelID
                lblModellBez.Text = .ModelName
                lblSippCode.Text = .SippCode
                If Not String.IsNullOrEmpty(.Laufzeit) Then
                    lblLaufzeit.Text = .Laufzeit.TrimStart("0")
                Else
                    lblLaufzeit.Text = ""
                End If
                cbxLaufzBind.Checked = .LZBindung
                rblFzggruppeLkw.SelectedValue = IIf(.FzggruppeLkw, "Ja", "Nein")
                rblWinterreifen.SelectedValue = IIf(.Winterreifen, "Ja", "Nein")
                rblAhk.SelectedValue = IIf(.Ahk, "Ja", "Nein")
                rblNavi.SelectedValue = IIf(.Navi, "Ja", "Nein")
                rblSecuritiFleet.SelectedValue = IIf(.SecuFleet, "Ja", "Nein")
                rblLeasing.SelectedValue = IIf(.Leasing, "Ja", "Nein")

            End If

            txtModellBez.Text = .ModelName
            txtSippcode.Text = .SippCode
            If Not String.IsNullOrEmpty(.Laufzeit) Then
                txtLaufzeit.Text = .Laufzeit.TrimStart("0")
            Else
                txtLaufzeit.Text = ""
            End If
            cbxLaufzBindNeu.Checked = .LZBindung
            rblFzggruppeLkwNeu.SelectedValue = IIf(.FzggruppeLkw, "Ja", "Nein")
            rblWinterreifenNeu.SelectedValue = IIf(.Winterreifen, "Ja", "Nein")
            rblAhkNeu.SelectedValue = IIf(.Ahk, "Ja", "Nein")
            rblNaviNeu.SelectedValue = IIf(.Navi, "Ja", "Nein")
            rblSecuritiFleetNeu.SelectedValue = IIf(.SecuFleet, "Ja", "Nein")
            rblLeasingNeu.SelectedValue = IIf(.Leasing, "Ja", "Nein")

        End With
    End Sub

    Private Sub DoSubmit()
        With objECModelID

            Dim blnNeueModelId As Boolean = (.VerarbeitungsKz = "N")

            .NeuModelName = txtModellBez.Text.Trim()
            .NeuSippCode = txtSippcode.Text.Trim()
            .NeuLaufzeit = txtLaufzeit.Text.Trim()
            .NeuLZBindung = cbxLaufzBindNeu.Checked
            .NeuFzggruppeLkw = (rblFzggruppeLkwNeu.SelectedValue = "Ja")
            .NeuWinterreifen = (rblWinterreifenNeu.SelectedValue = "Ja")
            .NeuAhk = (rblAhkNeu.SelectedValue = "Ja")
            .NeuNavi = (rblNaviNeu.SelectedValue = "Ja")
            .NeuSecuFleet = (rblSecuritiFleetNeu.SelectedValue = "Ja")
            .NeuLeasing = (rblLeasingNeu.SelectedValue = "Ja")

            If .NeuLZBindung AndAlso String.IsNullOrEmpty(.NeuLaufzeit) Then
                lblError.Text = "Bitte geben Sie die Laufzeit an!"
                Return
            End If

            If blnNeueModelId Then

                .HerstellerID = ddlHersteller.SelectedValue
                .NeuModelID = txtModellId.Text.Trim()

                If String.IsNullOrEmpty(.HerstellerID) OrElse String.IsNullOrEmpty(.NeuModelID) OrElse String.IsNullOrEmpty(.NeuModelName) OrElse String.IsNullOrEmpty(.NeuSippCode) Then
                    lblError.Text = "Bitte füllen Sie alle Pflichtfelder aus!"
                    Return
                End If

            Else

                .HerstellerID = ""
                .NeuModelID = .ModelID

                If Not .IsChanged Then
                    lblError.Text = "Es wurden keine Änderungen vorgenommen!"
                    Return
                End If

            End If

            .Change(Session("AppID").ToString, Session.SessionID.ToString, Me)

            If .Status <> 0 Then
                lblError.Text = objECModelID.Message
            Else
                Session("App_ChangeModelID") = Nothing
                cmdCreate.Enabled = False
                lblMessage.Text = "Daten erfolgreich gespeichert!"
            End If

        End With
    End Sub

#End Region

End Class