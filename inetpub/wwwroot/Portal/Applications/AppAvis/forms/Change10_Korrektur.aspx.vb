Imports CKG.Base.Kernel.Common.Common

Partial Class Change10_Korrektur
    Inherits System.Web.UI.Page

    Private _mUser As Base.Kernel.Security.User
    Private _mTransportbeauftragung As Transportbeauftragung

    Protected WithEvents ucStyles As Portal.PageElements.Styles
    Protected WithEvents ucHeader As Portal.PageElements.Header

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        _mUser = GetUser(Me)
        ucHeader.InitUser(_mUser)
        FormAuth(Me, _mUser)

        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = _mUser.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            
            If Session("_mTransportbeauftragung") IsNot Nothing Then
                _mTransportbeauftragung = CType(Session("_mTransportbeauftragung"), Transportbeauftragung)
            End If

            If Not IsPostBack Then
                'txtDatFreiVon.SelectedDate = Today
                'txtDatFreiBis.SelectedDate = Today

                Session("StepKorrektur") = 1
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten."
        End Try
    End Sub

    Private Sub cmdBack_Click(sender As Object, e As System.EventArgs) Handles cmdBack.Click
        If CInt(Session("StepKorrektur")) = 1 Then
            Response.Redirect("Change10.aspx?AppID=" & _mUser.Applications.Select("AppName='Change10'")(0)("AppID").ToString)
        End If
    End Sub

    Private Sub cmdCreate_Click(sender As Object, e As System.EventArgs) Handles cmdCreate.Click
        If CInt(Session("StepKorrektur")) = 1 Then
            '_mTransportbeauftragung.DatumZulassungVon = txtDatZulVon.SelectedDate
            '_mTransportbeauftragung.DatumZulassungBis = txtDatZulBis.SelectedDate
            '_mTransportbeauftragung.DatumFreisetzungVon = txtDatFreiVon.SelectedDate
            '_mTransportbeauftragung.DatumFreisetzungBis = txtDatFreiBis.SelectedDate
            _mTransportbeauftragung.strAuftragsnummer = rtbAuftragsnummer.Text

            Session("_mTransportbeauftragung") = _mTransportbeauftragung
            Session("StepKorrektur") = 2

            Response.Redirect("Change10_Korrektur_2.aspx?AppID=" & _mUser.Applications.Select("AppName='Change10_Korrektur_2'")(0)("AppID").ToString)
        End If

    End Sub

End Class