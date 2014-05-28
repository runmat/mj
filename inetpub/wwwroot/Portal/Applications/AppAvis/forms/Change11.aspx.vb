Imports CKG.Base.Kernel.Common.Common

Public Class Change11
    Inherits System.Web.UI.Page

    Private _mUser As Base.Kernel.Security.User
    'Private _mApp As Base.Kernel.Security.App
    Private _mAdresspflege As Adresspflege
    Private dtAdresse As DataTable = Nothing

    Protected WithEvents ucStyles As Portal.PageElements.Styles
    Protected WithEvents ucHeader As Portal.PageElements.Header

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        _mUser = GetUser(Me)
        ucHeader.InitUser(_mUser)
        FormAuth(Me, _mUser)

        GetAppIDFromQueryString(Me)

        lblError.Text = ""
        lblSuccess.Text = ""

        Try
            lblHead.Text = _mUser.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            '_mApp = New Base.Kernel.Security.App(_mUser)

            If Session("_mAdresspflege") Is Nothing Then
                Session("_mAdresspflege") = New Adresspflege(_mUser.KUNNR, _mUser.UserName)
            End If

            _mAdresspflege = CType(Session("_mAdresspflege"), Adresspflege)

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten."
        End Try
    End Sub

    Protected Sub lbtnInsert_Click(sender As Object, e As EventArgs) Handles lbtnInsert.Click

        If ValidateInput() Then
            dtAdresse = _mAdresspflege.InsertAdresse(rtbStationscode.Text,
                                                                rtbName1.Text,
                                                                rtbName2.Text,
                                                                rtbStrasse.Text,
                                                                rtbNummer.Text,
                                                                rtbPLZ.Text,
                                                                rtbOrt.Text,
                                                                rtbLand.Text,
                                                                rtbTelefon.Text,
                                                                rtbFax.Text,
                                                                rtbEmail.Text.Trim())

            If _mAdresspflege.ErrorOccured Then
                lblError.Text = _mAdresspflege.ErrorMessage

                If dtAdresse IsNot Nothing And dtAdresse.Rows.Count > 0 Then
                    FillAdresse(dtAdresse)
                    lbtnInsert.Visible = False
                    lbtnChange.Visible = True
                Else
                    ClearAdresseSAP()
                End If
            Else
                lblSuccess.Text = "Adresse erfolgreich angelegt."
            End If
        End If

    End Sub

    Protected Sub lbtnChange_Click(sender As Object, e As EventArgs) Handles lbtnChange.Click

        If ValidateInput() Then
            dtAdresse = _mAdresspflege.ChangeAdresse(rtbStationscode.Text,
                                                      rtbName1.Text,
                                                      rtbName2.Text,
                                                      rtbStrasse.Text,
                                                      rtbNummer.Text,
                                                      rtbPLZ.Text,
                                                      rtbOrt.Text,
                                                      rtbLand.Text,
                                                      rtbTelefon.Text,
                                                      rtbFax.Text,
                                                      rtbEmail.Text.Trim())
            If _mAdresspflege.ErrorOccured Then
                lblError.Text = _mAdresspflege.ErrorMessage

                If dtAdresse IsNot Nothing And dtAdresse.Rows.Count > 0 Then
                    FillAdresse(dtAdresse)
                Else
                    ClearAdresseSAP()
                End If
            Else
                lblSuccess.Text = "Adresse erfolgreich geändert."
            End If
        End If

    End Sub

    Protected Sub rtbStationsnummer_TextChanged(sender As Object, e As EventArgs) Handles rtbStationscode.TextChanged
        CheckStation()
    End Sub

    ''' <summary>
    ''' Führt die Validierung der Eingabefelder druch
    ''' </summary>
    ''' <returns>True wenn Validierung erfolgreich</returns>
    Private Function ValidateInput() As Boolean

        If rtbStationscode.Text.Trim() = "" Then
            lblError.Text = "Geben Sie eine Stationsnummer ein!"
            Return False
        End If

        If rtbName1.Text.Trim() = "" Then
            lblError.Text = "Geben Sie einen Namen ein!"
            Return False
        End If

        If rtbStrasse.Text.Trim() = "" Then
            lblError.Text = "Geben Sie eine Straße ein!"
            Return False
        End If

        If rtbNummer.Text.Trim() = "" Then
            lblError.Text = "Geben Sie eine Hausnummer ein!"
            Return False
        End If

        If rtbPLZ.Text.Trim() <> "" AndAlso Not Integer.TryParse(rtbPLZ.Text, New Integer) Then
            lblError.Text = "Geben Sie eine gültige Postleitzahl ein!"
            Return False
        End If

        If rtbOrt.Text.Trim() = "" Then
            lblError.Text = "Geben Sie einen Ort ein!"
            Return False
        End If

        If rtbEmail.Text.Trim() = "" Then
            lblError.Text = "Geben Sie mindestens eine gültige E-Mail-Adresse ein!"
            Return False
        ElseIf Not rtbEmail.Text.Contains("@") Or Not rtbEmail.Text.Contains(".") Then
            lblError.Text = "Die angegebene E-Mail-Adresse ist nicht gültig!"
            Return False
        End If

        Return True
    End Function

    ''' <summary>
    '''  Füllt die Hilfstabelle mit den in SAP gespeicherten Adressdaten
    ''' </summary>
    ''' <param name="dt">SAP-Adresstabelle</param>
    Private Sub FillAdresse(ByRef dt As DataTable)
        Dim rows As DataRow() = dt.Select("EX_KUNNR='" & rtbStationscode.Text & "'")
        If rows IsNot Nothing And rows.Count > 0 Then
            Dim row As DataRow = rows(0)

            lblStationsnummer.Text = row("EX_KUNNR").ToString
            lblName1.Text = row("NAME1").ToString
            lblName2.Text = row("NAME2").ToString
            lblStrasse.Text = row("STREET").ToString
            lblHausnummer.Text = row("HOUSE_NUM1").ToString
            lblPlz.Text = row("POST_CODE1").ToString
            lblOrt.Text = row("CITY1").ToString
            lblLand.Text = row("COUNTRY").ToString
            lblTelefon.Text = row("TEL_NUMBER").ToString
            lblFax.Text = row("FAX_NUMBER").ToString
            lblEmail.Text = row("SMTP_ADDR").ToString

            If row("AEDAT").ToString <> "" Then
                lblÄnderung.Text = "Zuletzt geändert am " & CDate(row("AEDAT")).ToShortDateString & " von " & row("AENAM").ToString
            End If
        End If
    End Sub

    ''' <summary>
    '''  Füllt die Eingabefelder mit den in SAP gespeicherten Adressdaten
    ''' </summary>
    ''' <param name="dt">SAP-Adresstabelle</param>
    Private Sub FillAdressInput(ByRef dt As DataTable)
        Dim rows As DataRow() = dt.Select("EX_KUNNR='" & rtbStationscode.Text & "'")
        If rows IsNot Nothing And rows.Count > 0 Then
            Dim row As DataRow = rows(0)

            rtbStationscode.Text = row("EX_KUNNR").ToString
            rtbName1.Text = row("NAME1").ToString
            rtbName2.Text = row("NAME2").ToString
            rtbStrasse.Text = row("STREET").ToString
            rtbNummer.Text = row("HOUSE_NUM1").ToString
            rtbPLZ.Text = row("POST_CODE1").ToString
            rtbOrt.Text = row("CITY1").ToString
            rtbLand.Text = row("COUNTRY").ToString
            rtbTelefon.Text = row("TEL_NUMBER").ToString
            rtbFax.Text = row("FAX_NUMBER").ToString
            rtbEmail.Text = row("SMTP_ADDR").ToString
        End If
    End Sub

    ''' <summary>
    ''' Löscht die Adressfelder mit den in SAP gespeicherten Adressdaten
    ''' </summary>
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
        lblEmail.Text = ""
        lblÄnderung.Text = ""
    End Sub

    ''' <summary>
    ''' Löscht die Adresseingabefelder außer Stationscode
    ''' </summary>
    Private Sub ClearAdressenInput()
        rtbName1.Text = ""
        rtbName2.Text = ""
        rtbStrasse.Text = ""
        rtbNummer.Text = ""
        rtbPLZ.Text = ""
        rtbOrt.Text = ""
        rtbLand.Text = ""
        rtbTelefon.Text = ""
        rtbFax.Text = ""
        rtbEmail.Text = ""
    End Sub

    Protected Sub btnCheckStationExists_Click(sender As Object, e As EventArgs) Handles btnCheckStationExists.Click
        CheckStation()
    End Sub

    ''' <summary>
    ''' Prüft den Stationscode
    ''' </summary>
    Private Sub CheckStation()
        rtbStationscode.Text = rtbStationscode.Text.ToUpper()

        ClearAdresseSAP()
        ClearAdressenInput()

        If rtbStationscode.Text = String.Empty Then
            lblError.Text = "Geben Sie einen Stationscode ein!"
        ElseIf _mAdresspflege.CheckStationExists(rtbStationscode.Text) Then
            lbtnChange.Enabled = True
            lbtnInsert.Enabled = False

            FillAdresse(_mAdresspflege.Adressen)
            FillAdressInput(_mAdresspflege.Adressen)

            lblError.Text = _mAdresspflege.ErrorMessage
        Else
            lbtnChange.Enabled = False
            lbtnInsert.Enabled = True

            lblSuccess.Text = "Die Station ist noch nicht vorhanden."
        End If
    End Sub

End Class