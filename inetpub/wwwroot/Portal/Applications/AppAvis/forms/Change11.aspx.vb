Imports CKG.Base.Kernel.Common.Common

Public Class Change11
    Inherits Page

    Private _mUser As Base.Kernel.Security.User
    Private _mAdresspflege As Adresspflege

    Protected WithEvents ucStyles As Portal.PageElements.Styles
    Protected WithEvents ucHeader As Portal.PageElements.Header

    Private ReadOnly Property ModusStationen() As Boolean
        Get
            Return (_mAdresspflege.Modus = AdressPflegeModus.Stationen)
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        _mUser = GetUser(Me)
        ucHeader.InitUser(_mUser)
        FormAuth(Me, _mUser)

        GetAppIDFromQueryString(Me)

        lblError.Text = ""
        lblSuccess.Text = ""

        Try
            lblHead.Text = _mUser.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text

            If Session("_mAdresspflege") IsNot Nothing Then
                _mAdresspflege = CType(Session("_mAdresspflege"), Adresspflege)
            Else
                _mAdresspflege = New Adresspflege(_mUser.KUNNR, _mUser.UserName)
                Session("_mAdresspflege") = _mAdresspflege
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten."
        End Try
    End Sub

    Protected Sub lbtnStationen_Click(sender As Object, e As EventArgs) Handles lbtnStationen.Click
        _mAdresspflege.Modus = AdressPflegeModus.Stationen
        Session("_mAdresspflege") = _mAdresspflege

        SwitchToEditMode()
    End Sub

    Protected Sub lbtnSpediteure_Click(sender As Object, e As EventArgs) Handles lbtnSpediteure.Click
        _mAdresspflege.Modus = AdressPflegeModus.Spediteure
        Session("_mAdresspflege") = _mAdresspflege

        SwitchToEditMode()
    End Sub

    Protected Sub lbtnZurueck_Click(sender As Object, e As EventArgs) Handles lbtnZurueck.Click
        SwitchToSelectionMode()
    End Sub

    Private Sub SwitchToEditMode()
        trSelection.Visible = False
        trEdit.Visible = True

        txtStationscode.Text = ""
        ClearAdresseEdit()
        ClearAdresseSAP()

        lbtnInsert.Visible = ModusStationen
        lbtnInsert.Enabled = False
        lbtnChange.Enabled = False
        lblSearchFieldName.Text = IIf(ModusStationen, "Stationscode:", "Carport-ID:").ToString()
        txtName1.Enabled = ModusStationen
        txtName2.Enabled = ModusStationen
        txtStrasse.Enabled = ModusStationen
        txtNummer.Enabled = ModusStationen
        txtPLZ.Enabled = ModusStationen
        txtOrt.Enabled = ModusStationen
        txtLand.Enabled = ModusStationen
        txtTelefon.Enabled = ModusStationen
        txtFax.Enabled = ModusStationen
    End Sub

    Private Sub SwitchToSelectionMode()
        trSelection.Visible = True
        trEdit.Visible = False
    End Sub

    Protected Sub txtStationsnummer_TextChanged(sender As Object, e As EventArgs) Handles txtStationscode.TextChanged
        CheckStation()
    End Sub

    Protected Sub btnCheckStationExists_Click(sender As Object, e As EventArgs) Handles btnCheckStationExists.Click
        CheckStation()
    End Sub

    Private Sub CheckStation()
        txtStationscode.Text = txtStationscode.Text.ToUpper()

        ClearAdresseSAP()
        ClearAdresseEdit()

        If String.IsNullOrEmpty(txtStationscode.Text) Then
            lblError.Text = String.Format("Geben Sie eine {0} ein!", IIf(ModusStationen, "Stationsnummer", "Carport-ID"))
            Exit Sub
        End If

        If _mAdresspflege.CheckStationExists(txtStationscode.Text) Then
            lbtnChange.Enabled = True
            lbtnInsert.Enabled = False

            FillAdresseSAP()
            FillAdresseEdit()

            lblError.Text = _mAdresspflege.ErrorMessage
        Else
            lbtnChange.Enabled = False
            lbtnInsert.Enabled = True

            lblSuccess.Text = String.Format("{0} ist noch nicht vorhanden.", IIf(ModusStationen, "Die Station", "Das Carport"))
        End If

        Session("_mAdresspflege") = _mAdresspflege
    End Sub

    Private Sub FillAdresseSAP()
        Dim strSelectPropertyName As String = IIf(ModusStationen, "EX_KUNNR", "DADPDI").ToString
        Dim rows As DataRow() = _mAdresspflege.Adressen.Select(strSelectPropertyName & "='" & txtStationscode.Text & "'")
        If rows IsNot Nothing And rows.Count > 0 Then
            Dim row As DataRow = rows(0)

            If ModusStationen Then lblStationsnummer.Text = row("EX_KUNNR").ToString Else lblStationsnummer.Text = row("DADPDI").ToString
            lblName1.Text = row("NAME1").ToString
            lblName2.Text = row("NAME2").ToString
            If ModusStationen Then lblStrasse.Text = row("STREET").ToString Else lblStrasse.Text = row("STRAS1").ToString
            If ModusStationen Then lblHausnummer.Text = row("HOUSE_NUM1").ToString Else lblHausnummer.Text = ""
            If ModusStationen Then lblPlz.Text = row("POST_CODE1").ToString Else lblPlz.Text = row("PSTLZ").ToString
            If ModusStationen Then lblOrt.Text = row("CITY1").ToString Else lblOrt.Text = row("ORT01").ToString
            If ModusStationen Then lblLand.Text = row("COUNTRY").ToString Else lblLand.Text = row("LAND").ToString
            If ModusStationen Then lblTelefon.Text = row("TEL_NUMBER").ToString Else lblTelefon.Text = row("TELF1").ToString
            If ModusStationen Then lblFax.Text = row("FAX_NUMBER").ToString Else lblFax.Text = ""

            Dim mailAdr As String = row("SMTP_ADDR").ToString

            If mailAdr.Contains(";"c) Then
                Dim adrs() As String = mailAdr.Split(";"c)

                lblEmail1.Text = adrs(0)
                If adrs.Length > 1 Then lblEmail2.Text = adrs(1) Else lblEmail2.Text = ""
                If adrs.Length > 2 Then lblEmail3.Text = adrs(2) Else lblEmail3.Text = ""
                If adrs.Length > 3 Then lblEmail4.Text = adrs(3) Else lblEmail4.Text = ""
                If adrs.Length > 4 Then lblEmail5.Text = adrs(4) Else lblEmail5.Text = ""
            Else
                lblEmail1.Text = mailAdr
                lblEmail2.Text = ""
                lblEmail3.Text = ""
                lblEmail4.Text = ""
                lblEmail5.Text = ""
            End If

            If ModusStationen AndAlso Not String.IsNullOrEmpty(row("AEDAT").ToString) Then
                lblÄnderung.Text = "Zuletzt geändert am " & CDate(row("AEDAT")).ToShortDateString() & " von " & row("AENAM").ToString
            Else
                lblÄnderung.Text = ""
            End If
        End If
    End Sub

    Private Sub FillAdresseEdit()
        Dim strSelectPropertyName As String = IIf(ModusStationen, "EX_KUNNR", "DADPDI").ToString
        Dim rows As DataRow() = _mAdresspflege.Adressen.Select(strSelectPropertyName & "='" & txtStationscode.Text & "'")
        If rows IsNot Nothing And rows.Count > 0 Then
            Dim row As DataRow = rows(0)

            If ModusStationen Then txtStationscode.Text = row("EX_KUNNR").ToString Else txtStationscode.Text = row("DADPDI").ToString
            txtName1.Text = row("NAME1").ToString
            txtName2.Text = row("NAME2").ToString
            If ModusStationen Then txtStrasse.Text = row("STREET").ToString Else txtStrasse.Text = row("STRAS1").ToString
            If ModusStationen Then txtNummer.Text = row("HOUSE_NUM1").ToString Else txtNummer.Text = ""
            If ModusStationen Then txtPLZ.Text = row("POST_CODE1").ToString Else txtPLZ.Text = row("PSTLZ").ToString
            If ModusStationen Then txtOrt.Text = row("CITY1").ToString Else txtOrt.Text = row("ORT01").ToString
            If ModusStationen Then txtLand.Text = row("COUNTRY").ToString Else txtLand.Text = row("LAND").ToString
            If ModusStationen Then txtTelefon.Text = row("TEL_NUMBER").ToString Else txtTelefon.Text = row("TELF1").ToString
            If ModusStationen Then txtFax.Text = row("FAX_NUMBER").ToString Else txtFax.Text = ""

            Dim mailAdr As String = row("SMTP_ADDR").ToString

            If mailAdr.Contains(";"c) Then
                Dim adrs() As String = mailAdr.Split(";"c)

                txtEmail1.Text = adrs(0)
                If adrs.Length > 1 Then txtEmail2.Text = adrs(1) Else txtEmail2.Text = ""
                If adrs.Length > 2 Then txtEmail3.Text = adrs(2) Else txtEmail3.Text = ""
                If adrs.Length > 3 Then txtEmail4.Text = adrs(3) Else txtEmail4.Text = ""
                If adrs.Length > 4 Then txtEmail5.Text = adrs(4) Else txtEmail5.Text = ""
            Else
                txtEmail1.Text = mailAdr
                txtEmail2.Text = ""
                txtEmail3.Text = ""
                txtEmail4.Text = ""
                txtEmail5.Text = ""
            End If
        End If
    End Sub

    Private Sub ClearAdresseSAP()
        lblStationsnummer.Text = ""
        lblName1.Text = ""
        lblName2.Text = ""
        lblStrasse.Text = ""
        lblHausnummer.Text = ""
        lblPlz.Text = ""
        lblOrt.Text = ""
        lblLand.Text = ""
        lblTelefon.Text = ""
        lblFax.Text = ""
        lblEmail1.Text = ""
        lblEmail2.Text = ""
        lblEmail3.Text = ""
        lblEmail4.Text = ""
        lblEmail5.Text = ""
        lblÄnderung.Text = ""
    End Sub

    Private Sub ClearAdresseEdit()
        txtName1.Text = ""
        txtName2.Text = ""
        txtStrasse.Text = ""
        txtNummer.Text = ""
        txtPLZ.Text = ""
        txtOrt.Text = ""
        txtLand.Text = ""
        txtTelefon.Text = ""
        txtFax.Text = ""
        txtEmail1.Text = ""
        txtEmail2.Text = ""
        txtEmail3.Text = ""
        txtEmail4.Text = ""
        txtEmail5.Text = ""
    End Sub

    Private Function ValidateInput() As Boolean
        If String.IsNullOrEmpty(txtStationscode.Text) Then
            lblError.Text = String.Format("Geben Sie eine {0} ein!", IIf(ModusStationen, "Stationsnummer", "Carport-ID"))
            Return False
        End If

        If ModusStationen Then
            If String.IsNullOrEmpty(txtName1.Text) Then
                lblError.Text = "Geben Sie einen Namen ein!"
                Return False
            End If

            If String.IsNullOrEmpty(txtStrasse.Text) Then
                lblError.Text = "Geben Sie eine Straße ein!"
                Return False
            End If

            If String.IsNullOrEmpty(txtNummer.Text) Then
                lblError.Text = "Geben Sie eine Hausnummer ein!"
                Return False
            End If

            If String.IsNullOrEmpty(txtPLZ.Text) Then
                lblError.Text = "Geben Sie eine Postleitzahl ein!"
                Return False
            End If

            Dim tmpInt As Integer
            If String.IsNullOrEmpty(txtPLZ.Text) OrElse Not Integer.TryParse(txtPLZ.Text, tmpInt) Then
                lblError.Text = "Geben Sie eine gültige Postleitzahl ein!"
                Return False
            End If

            If String.IsNullOrEmpty(txtOrt.Text) Then
                lblError.Text = "Geben Sie einen Ort ein!"
                Return False
            End If
        End If
        
        If String.IsNullOrEmpty(txtEmail1.Text) AndAlso String.IsNullOrEmpty(txtEmail2.Text) AndAlso String.IsNullOrEmpty(txtEmail3.Text) AndAlso _
                String.IsNullOrEmpty(txtEmail4.Text) AndAlso String.IsNullOrEmpty(txtEmail5.Text) Then
            lblError.Text = "Geben Sie mindestens eine gültige E-Mail-Adresse ein!"
            Return False
        End If

        If Not String.IsNullOrEmpty(txtEmail1.Text) AndAlso (Not txtEmail1.Text.Contains("@") OrElse Not txtEmail1.Text.Contains(".")) Then
            lblError.Text = "Die angegebene E-Mail-Adresse 1 ist nicht gültig!"
            Return False
        End If

        If Not String.IsNullOrEmpty(txtEmail2.Text) AndAlso (Not txtEmail2.Text.Contains("@") OrElse Not txtEmail2.Text.Contains(".")) Then
            lblError.Text = "Die angegebene E-Mail-Adresse 2 ist nicht gültig!"
            Return False
        End If

        If Not String.IsNullOrEmpty(txtEmail3.Text) AndAlso (Not txtEmail3.Text.Contains("@") OrElse Not txtEmail3.Text.Contains(".")) Then
            lblError.Text = "Die angegebene E-Mail-Adresse 3 ist nicht gültig!"
            Return False
        End If

        If Not String.IsNullOrEmpty(txtEmail4.Text) AndAlso (Not txtEmail4.Text.Contains("@") OrElse Not txtEmail4.Text.Contains(".")) Then
            lblError.Text = "Die angegebene E-Mail-Adresse 4 ist nicht gültig!"
            Return False
        End If

        If Not String.IsNullOrEmpty(txtEmail5.Text) AndAlso (Not txtEmail5.Text.Contains("@") OrElse Not txtEmail5.Text.Contains(".")) Then
            lblError.Text = "Die angegebene E-Mail-Adresse 5 ist nicht gültig!"
            Return False
        End If

        Return True
    End Function

    Protected Sub lbtnInsert_Click(sender As Object, e As EventArgs) Handles lbtnInsert.Click

        If ValidateInput() Then
            Dim mailAdr As String = String.Format("{0};{1};{2};{3};{4}",
                                                  txtEmail1.Text,
                                                  txtEmail2.Text,
                                                  txtEmail3.Text,
                                                  txtEmail4.Text,
                                                  txtEmail5.Text)

            _mAdresspflege.InsertAdresse(txtStationscode.Text,
                                         txtName1.Text,
                                         txtName2.Text,
                                         txtStrasse.Text,
                                         txtNummer.Text,
                                         txtPLZ.Text,
                                         txtOrt.Text,
                                         txtLand.Text,
                                         txtTelefon.Text,
                                         txtFax.Text,
                                         mailAdr.Trim(";"c))

            If _mAdresspflege.ErrorOccured Then
                lblError.Text = _mAdresspflege.ErrorMessage
            Else
                lblSuccess.Text = "Adresse erfolgreich angelegt."

                lbtnInsert.Enabled = False
                lbtnChange.Enabled = True

                If _mAdresspflege.Adressen IsNot Nothing And _mAdresspflege.Adressen.Rows.Count > 0 Then
                    FillAdresseSAP()
                Else
                    ClearAdresseSAP()
                End If
            End If

            Session("_mAdresspflege") = _mAdresspflege
        End If

    End Sub

    Protected Sub lbtnChange_Click(sender As Object, e As EventArgs) Handles lbtnChange.Click

        If ValidateInput() Then
            Dim mailAdr As String = String.Format("{0};{1};{2};{3};{4}",
                                                  txtEmail1.Text,
                                                  txtEmail2.Text,
                                                  txtEmail3.Text,
                                                  txtEmail4.Text,
                                                  txtEmail5.Text)

            _mAdresspflege.ChangeAdresse(txtStationscode.Text,
                                         txtName1.Text,
                                         txtName2.Text,
                                         txtStrasse.Text,
                                         txtNummer.Text,
                                         txtPLZ.Text,
                                         txtOrt.Text,
                                         txtLand.Text,
                                         txtTelefon.Text,
                                         txtFax.Text,
                                         mailAdr.Trim(";"c))

            If _mAdresspflege.ErrorOccured Then
                lblError.Text = _mAdresspflege.ErrorMessage
            Else
                lblSuccess.Text = "Adresse erfolgreich geändert."

                If _mAdresspflege.Adressen IsNot Nothing And _mAdresspflege.Adressen.Rows.Count > 0 Then
                    FillAdresseSAP()
                Else
                    ClearAdresseSAP()
                End If
            End If

            Session("_mAdresspflege") = _mAdresspflege
        End If

    End Sub

End Class