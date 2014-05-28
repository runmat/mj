Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Partial Public Class Change07_2
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

            If IsPostBack = False Then


                LoadData()

            End If




        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten."
        End Try
    End Sub

    Private Sub LoadData()
        Dim Unfall As Unfallmeldung = CType(Session("Unfallmeldung"), Unfallmeldung)

        Dim SelRow As DataRow()

        SelRow = Unfall.ResultFahrzeuge.Select("EQUNR = '" & Unfall.EquiNr & "'")

        If SelRow.Length > 0 Then

            lblFahrgestellnummerShow.Text = SelRow(0)("CHASSIS_NUM").ToString
            lblKennzeichenShow.Text = SelRow(0)("LICENSE_NUM").ToString
            lblBriefnummerShow.Text = SelRow(0)("TIDNR").ToString
            lblUnitnrShow.Text = SelRow(0)("ZZREFERENZ1").ToString


        End If


    End Sub



    Protected Sub cmdCreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCreate.Click


        If cmdCreate.Text.Contains("zurücksetzen") = True Then
            cmdCreate.Text = Replace(cmdCreate.Text, "zurücksetzen", "Existenz prüfen")
            lblStation.Text = ""

            tr0.Visible = False
            tr1.Visible = False
            tr2.Visible = False
            'txtForderung.Text = ""
            txtStandort.Text = ""
            txtStationscode.Text = ""
            txtStationscode.Enabled = True
        Else
            If txtStationscode.Text.Length > 0 Then

                txtStationscode.Text = txtStationscode.Text.ToUpper

                Dim Unfall As Unfallmeldung = CType(Session("Unfallmeldung"), Unfallmeldung)

                Unfall.GetStation(Session("AppID").ToString, Session.SessionID.ToString, txtStationscode.Text, Me.Page)

                If Unfall.Stationen.Rows.Count > 0 Then


                    lblStation.Text = Unfall.Stationen.Rows(0)("NAME1").ToString & "<br />"
                    lblStation.Text &= Unfall.Stationen.Rows(0)("STREET").ToString & "<br />"
                    lblStation.Text &= Unfall.Stationen.Rows(0)("POST_CODE1").ToString & "&nbsp;" & Unfall.Stationen.Rows(0)("CITY1").ToString

                    txtStationscode.Enabled = False
                    cmdCreate.Text = Replace(cmdCreate.Text, "Existenz prüfen", "zurücksetzen")
                    tr0.Visible = True
                    tr1.Visible = True
                    tr2.Visible = True
                    'txtForderung.Text = ""
                    txtStandort.Text = ""
                Else
                    lblStation.Text = ""
                    lblError.Text = "Es wurde keine Station gefunden."
                    tr0.Visible = False
                    tr1.Visible = False
                    tr2.Visible = False
                    'txtForderung.Text = ""
                    txtStandort.Text = ""
                End If

            Else
                lblError.Text = "Bitte geben Sie einen Stationscode an."
            End If

        End If


    End Sub

  
    Protected Sub cmdSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSave.Click


        'If txtForderung.Text.Length > 0 AndAlso IsNumeric(txtForderung.Text) = False Then
        '    lblError.Text = "Bitte geben Sie einen korrekten Forderungsbetrag ein."
        '    Exit Sub
        'End If

        Dim Unfall As Unfallmeldung = CType(Session("Unfallmeldung"), Unfallmeldung)

        Unfall.SetMeldung(Session("AppID").ToString, Session.SessionID.ToString, txtStationscode.Text, txtStandort.Text, Me.Page)



        If Unfall.Unfallnummer.Length > 0 Then

            lblMessage.Text = "Ihre Meldung wurden unter der Nummer " & Unfall.Unfallnummer & " gesichert."

            cmdCreate.Enabled = False
            cmdSave.Enabled = False

            'txtForderung.Enabled = False
            txtStandort.Enabled = False
            txtStationscode.Enabled = False

            Dim RowFilter As String = "EQUNR <> '" & Unfall.EquiNr & "'"

            Unfall.ResultFahrzeuge.DefaultView.RowFilter = RowFilter

            Unfall.ResultFahrzeuge = Unfall.ResultFahrzeuge.DefaultView.ToTable


            Session("Unfallmeldung") = Unfall


        Else

            If Unfall.Status > 0 Then
                lblError.Text = Unfall.Message
            End If

        End If



    End Sub

    Protected Sub cmdBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdBack.Click
        Response.Redirect("Change07.aspx?AppID=" & Session("AppID").ToString)
    End Sub
End Class