Public Class Change04
    Inherits System.Web.UI.Page

    Private _user As Base.Kernel.Security.User
    Private _app As Base.Kernel.Security.App

    Protected Overrides Sub OnLoad(e As System.EventArgs)
        MyBase.OnLoad(e)

        Me._user = Base.Kernel.Common.Common.GetUser(Me)

        Base.Kernel.Common.Common.FormAuth(Me, Me._user)
        Base.Kernel.Common.Common.GetAppIDFromQueryString(Me)

        lblHead.Text = Me._user.Applications.Select("AppID = '" & Session("AppID").ToString() & "'")(0)("AppFriendlyName").ToString()
        Me._app = New Base.Kernel.Security.App(Me._user)

        Dim cb As New CarportBeauftragung(Me._user, Me._app, Me)
        'Me.msbDienstleistungen.Items.AddRange(cb.GetServices())
        'Me.msbDienstleistungen.DataTextField = "Text"
        'Me.msbDienstleistungen.DataValueField = "Value"
        'Me.msbDienstleistungen.DataSource = cb.GetServices()
        'Me.msbDienstleistungen.DataBind()

        If Not Me.IsPostBack Then
            Me.HideRows()

            Me.ddlCarport.Items.AddRange(cb.GetCarports().ToArray())
            Me.ddlLogistikPartner.Items.AddRange(cb.GetLogistikPartner().ToArray())

            Me.cvBereitstellungsdatum.ValueToCompare = DateTime.Today.ToShortDateString()
            Me.lbDienstleistungen.Items.AddRange(cb.GetServices().ToArray())
        End If
    End Sub

    Protected Overrides Sub OnPreRender(e As System.EventArgs)
        MyBase.OnPreRender(e)

        Base.Kernel.Common.Common.SetEndASPXAccess(Me)
    End Sub

    Protected Overrides Sub OnUnload(e As System.EventArgs)
        MyBase.OnUnload(e)

        Base.Kernel.Common.Common.SetEndASPXAccess(Me)
    End Sub

    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        Response.Redirect("../../../Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub

    Protected Sub OnStatusClicked(sender As Object, e As EventArgs)
        Change04.Säubern(Me.txtFahrgestellnummer)
        Change04.Säubern(Me.txtKennzeichen)

        Me.Validate("validation")

        If Me.IsValid Then
            Dim cb = New CarportBeauftragung(Me._user, Me._app, Me)
            Try
                Dim dr = cb.GetAuftrag(Me.txtFahrgestellnummer.Text)
                Dim anlzu As String = If(dr IsNot Nothing, dr.Field(Of String)("ANLZU"), Nothing)

                If Not String.IsNullOrEmpty(anlzu) AndAlso anlzu.Equals("B", StringComparison.OrdinalIgnoreCase) Then
                    Me.txtStatus.Text = "B=Beauftragt"

                    Me.txtFahrgestellnummer.Text = dr.Field(Of String)("CHASSIS_NUM")
                    Me.txtKennzeichen.Text = dr.Field(Of String)("ETEXT4")
                    Me.ddlCarport.SelectedValue = dr.Field(Of String)("ETEXT1")
                    Me.ddlLogistikPartner.SelectedValue = dr.Field(Of String)("ETEXT3")
                    Me.txtBereitstellungsdatum.Text = dr.Field(Of DateTime)("TERMIN_ANFANG").ToShortDateString()
                    Me.txtBemerkung.Text = dr.Field(Of String)("TEXTE")

                    Dim dl = From s In dr.Field(Of String)("DIENSTLEISTUNGEN").Split(", ")
                             Let t = s.Trim()
                             Join i As ListItem In lbDienstleistungen.Items On t Equals i.Text
                             Select i

                    For Each i In dl
                        i.Selected = True
                    Next

                    Me.Container.Enabled = False
                Else
                    Me.txtStatus.Text = "Neu"
                End If

                Me.ShowRows()
            Catch ex As Exception
                lblError.Text = ex.Message
                lblError.Visible = True
            End Try
        End If
    End Sub

    Protected Sub OnSaveClicked(sender As Object, e As EventArgs) Handles bSave.Click
        Change04.Säubern(Me.txtFahrgestellnummer)
        Change04.Säubern(Me.txtKennzeichen)

        Me.Validate("validation")

        If Me.IsValid Then
            Dim bereitstellungsdatum As DateTime
            Dim okay As Boolean = DateTime.TryParse(Me.txtBereitstellungsdatum.Text, bereitstellungsdatum)

            If okay = False Then
                lblError.Text = "Bitte geben Sie ein gültiges Bereitstellungsdatum ein."
                Exit Sub
            End If

            Dim dienstleistungen As IEnumerable(Of String) = From item As ListItem In Me.lbDienstleistungen.Items _
                                                             Where item.Selected _
                                                             Select item.Value

            Dim cb As New CarportBeauftragung(Me._user, Me._app, Me)
            cb.SaveRückmeldung(Me.txtFahrgestellnummer.Text, _
                               Me.txtKennzeichen.Text, _
                               Me.ddlCarport.SelectedValue, _
                               Me.ddlLogistikPartner.SelectedValue, _
                               If(okay, New DateTime?(bereitstellungsdatum), Nothing), _
                               Me.txtBemerkung.Text, _
                               dienstleistungen)

            Me.Container.Enabled = False
            Me.mpeSaveMessage.Show()
        End If
    End Sub

    Private Shared Sub Säubern(tb As TextBox)
        tb.Text = tb.Text.Trim().ToUpper(System.Globalization.CultureInfo.CurrentCulture)
    End Sub

    Private Sub HideRows()
        Me.row1.Style.Add(HtmlTextWriterStyle.Display, "none")
        Me.row2.Style.Add(HtmlTextWriterStyle.Display, "none")
        Me.row3.Style.Add(HtmlTextWriterStyle.Display, "none")
        Me.row4.Style.Add(HtmlTextWriterStyle.Display, "none")
        Me.row5.Style.Add(HtmlTextWriterStyle.Display, "none")
        Me.row6.Style.Add(HtmlTextWriterStyle.Display, "none")

        Me.bStatus.Style.Add(HtmlTextWriterStyle.MarginBottom, "12px")
    End Sub

    Private Sub ShowRows()
        Me.row1.Style(HtmlTextWriterStyle.Display) = Nothing
        Me.row2.Style(HtmlTextWriterStyle.Display) = Nothing
        Me.row3.Style(HtmlTextWriterStyle.Display) = Nothing
        Me.row4.Style(HtmlTextWriterStyle.Display) = Nothing
        Me.row5.Style(HtmlTextWriterStyle.Display) = Nothing
        Me.row6.Style(HtmlTextWriterStyle.Display) = Nothing

        Me.bStatus.Style(HtmlTextWriterStyle.MarginBottom) = Nothing
    End Sub

    Protected Sub ClearReport()
        Response.Redirect("change04.aspx?AppID=" & Session("AppID").ToString(), False)
    End Sub

    Protected Sub bOkay_Click(sender As Object, e As EventArgs) Handles bOkay.Click
        ClearReport()
    End Sub
End Class