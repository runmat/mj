Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports System.Drawing

Namespace Zulassung
    Partial Public Class Change01_3
        Inherits System.Web.UI.Page
#Region "Declarations"
        Private m_User As Base.Kernel.Security.User
        Private m_App As Base.Kernel.Security.App
        Private objZulassung As Zulassung1
        Private versandart As String
        Private authentifizierung As String
        Private booError As Boolean
        Dim PflichtfeldWarning As Boolean
        Private Const ERROR_MESSAGE_PFLICHTFELD As String = "Bitte füllen Sie alle Pflichfelder(*) aus."
#End Region

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            m_User = GetUser(Me)
            FormAuth(Me, m_User, True)
            GetAppIDFromQueryString(Me)

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            m_App = New Security.App(m_User)

            If IsNothing(Session("objZulassung")) Then Response.Redirect("Change01.aspx?AppID=" & Session("AppID").ToString)

            lnkFahrzeugsuche.NavigateUrl = "Change01.aspx?AppID=" & Session("AppID").ToString
            lnkFahrzeugauswahl.NavigateUrl = "Change01_2.aspx?AppID=" & Session("AppID").ToString

            objZulassung = CType(Session("objZulassung"), Zulassung1)
            Try
                If Not IsPostBack Then
                    FillControls()
                Else
                    AddToSessionObject()
                End If
            Catch ex As Exception
                lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten."
            End Try
        End Sub


        Private Sub btnSuchen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSuchen.Click
            If Hidden1.Value.Length > 0 Then
                pnlHalter.Attributes.Add("style", "display:none")
                pnlZulDaten.Attributes.Add("style", "display:none")
                pnlVersicherer.Attributes.Add("style", "display:none")
                pnlVSS.Attributes.Add("style", "display:none")
                Panel1.Attributes.Add("style", "display:block")
                cmdSearch.Attributes.Add("style", "display:none")
                lblErrorSearch.Text = String.Empty


                'Laut ITA 4584(DMa) entfernen.
                'If Replace(txtName.Text, "*", "") = "" AndAlso _
                '    Replace(txtPLZ.Text, "*", "") = "" AndAlso _
                '    Replace(txtOrt.Text, "*", "") = "" Then
                '    trErrorSearch.Visible = True
                '    lblErrorSearch.Text = "Bitte geben Sie wenigstens ein Suchkriterium an."
                '    Exit Sub
                'End If


                If objZulassung Is Nothing Then
                    objZulassung = DirectCast(Session("objZulassung"), Zulassung1)
                End If
            End If

            Select Case Hidden1.Value
                Case "HALTER"
                    Try
                        objZulassung.Adressen = objZulassung.GetAdresse("HALTER", txtName.Text, _
                                                           txtPLZ.Text, _
                                                           txtOrt.Text)

                        drpAdresse.DataSource = objZulassung.Adressen

                        drpAdresse.DataTextField = "Adresse"
                        drpAdresse.DataValueField = "POS_KURZTEXT"

                        drpAdresse.DataBind()

                    Catch ex As Exception
                        trErrorSearch.Visible = True
                        lblErrorSearch.Text = ex.Message
                    End Try
                Case "VERSICHERER"
                    Try
                        objZulassung.Adressen = objZulassung.GetAdresse("VERSICHERER", txtName.Text, _
                                                           txtPLZ.Text, _
                                                           txtOrt.Text)

                        drpAdresse.DataSource = objZulassung.Adressen

                        drpAdresse.DataTextField = "Adresse"
                        drpAdresse.DataValueField = "POS_KURZTEXT"

                        drpAdresse.DataBind()

                    Catch ex As Exception
                        trErrorSearch.Visible = True
                        lblErrorSearch.Text = ex.Message
                    End Try
                Case "VSS"
                    Try
                        objZulassung.Adressen = objZulassung.GetAdresse("SSCHILDER", txtName.Text, _
                                                           txtPLZ.Text, _
                                                           txtOrt.Text)

                        drpAdresse.DataSource = objZulassung.Adressen

                        drpAdresse.DataTextField = "Adresse"
                        drpAdresse.DataValueField = "POS_KURZTEXT"

                        drpAdresse.DataBind()

                    Catch ex As Exception
                        trErrorSearch.Visible = True
                        lblErrorSearch.Text = ex.Message
                    End Try
                Case "EVB"
                    Try
                        objZulassung.Adressen = objZulassung.GetAdresse("EVB", txtName.Text, _
                                                           txtPLZ.Text, _
                                                           txtOrt.Text)

                        drpAdresse.DataSource = objZulassung.Adressen

                        drpAdresse.DataTextField = "Adresse"
                        drpAdresse.DataValueField = "POS_KURZTEXT"

                        drpAdresse.DataBind()

                    Catch ex As Exception
                        trErrorSearch.Visible = True
                        lblErrorSearch.Text = ex.Message
                    End Try
                Case Else


            End Select

        End Sub

        Private Sub lbCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCancel.Click
            pnlHalter.Attributes.Add("style", "display:block")
            pnlZulDaten.Attributes.Add("style", "display:block")
            pnlVersicherer.Attributes.Add("style", "display:block")
            pnlVSS.Attributes.Add("style", "display:block")
            cmdSearch.Attributes.Add("style", "display:block")
            Panel1.Attributes.Add("style", "display:none")
        End Sub

        Private Sub lbGet_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbGet.Click

            If drpAdresse.SelectedItem.Text = "Auswahl" Then
                pnlHalter.Attributes.Add("style", "display:none")
                pnlZulDaten.Attributes.Add("style", "display:none")
                pnlVersicherer.Attributes.Add("style", "display:none")
                pnlVSS.Attributes.Add("style", "display:none")
                cmdSearch.Attributes.Add("style", "display:none")
                Panel1.Attributes.Add("style", "display:block")

                lblErrorSearch.Text = "Bitte führen Sie zuerst eine Suche aus."
                Exit Sub
            End If
            If objZulassung Is Nothing Then
                objZulassung = DirectCast(Session("objZulassung"), Zulassung1)
            End If

            Dim Kennung As String
            Kennung = drpAdresse.SelectedItem.Value
            Dim AdressRows() As DataRow = objZulassung.Adressen.Select("POS_KURZTEXT = '" & Kennung & "'")

            Dim AdressRow As DataRow = AdressRows(0)

            With objZulassung
                Select Case Hidden1.Value
                    Case "HALTER"
                        .Halter = AdressRow("Name1").ToString
                        .HalterStrasse = AdressRow("STRAS").ToString
                        .HalterPLZ = AdressRow("PSTLZ").ToString
                        .HalterOrt = AdressRow("ORT01").ToString
                    Case "VERSICHERER"
                        .VersNehmer = AdressRow("Name1").ToString
                        .VersNehmerStrasse = AdressRow("STRAS").ToString
                        .VersNehmerPLZ = AdressRow("PSTLZ").ToString
                        .VersNehmerOrt = AdressRow("ORT01").ToString
                    Case "VSS"
                        .VssName = AdressRow("Name1").ToString
                        .VssName2 = AdressRow("Name2").ToString
                        .VssStrasse = AdressRow("STRAS").ToString
                        .VssPLZ = AdressRow("PSTLZ").ToString
                        .VssOrt = AdressRow("ORT01").ToString
                    Case "EVB"
                        .VersGesellschaft = AdressRow("Name1").ToString
                        .EVBNummer = AdressRow("POS_Text").ToString
                End Select
            End With
            Session("objZulassung") = objZulassung
            pnlHalter.Attributes.Add("style", "display:block")
            pnlZulDaten.Attributes.Add("style", "display:block")
            pnlVersicherer.Attributes.Add("style", "display:block")
            pnlVSS.Attributes.Add("style", "display:block")
            cmdSearch.Attributes.Add("style", "display:block")
            Panel1.Attributes.Add("style", "display:none")
            txtName.Text = ""
            txtPLZ.Text = ""
            txtOrt.Text = ""
            drpAdresse.Items.Clear()
            FillControls()
        End Sub

        Private Sub FillControls()

            If IsNothing(Session("objZulassung")) = False Then
                objZulassung = DirectCast(Session("objZulassung"), Zulassung1)

                With objZulassung
                    Dim tmpRows As DataRow()
                    tmpRows = objZulassung.Fahrzeuge.Select("AUSWAHL='99'")
                    .ZulCount = tmpRows.Length
                    If tmpRows.Length > 1 Then
                        trWunschkennz.Visible = False
                        trWunschInfo.Visible = False
                    End If

                    '***Halter***
                    txtShName.Text = .Halter
                    txtShName2.Text = .Halter2
                    txtShStrasse.Text = .HalterStrasse
                    txtShPLZ.Text = .HalterPLZ
                    txtShOrt.Text = .HalterOrt

                    '***Versicherungsnehmer = Halter?***

                    If Len(.VersNehmer) > 0 Then
                        txtVersNehmer.Text = .VersNehmer
                        txtVersStrasse.Text = .VersNehmerStrasse
                        txtVersPLZ.Text = .VersNehmerPLZ
                        txtVersOrt.Text = .VersNehmerOrt
                    End If

                    '***Zulassung***
                    txtZulassungsdatum.Text = .Zulassungsdatum

                    txtWKennzeichen2.Text = .Wunschkennzeichen2
                    txtWKennzeichen3.Text = .Wunschkennzeichen3
                    txtZulkreis.Text = .Zulassungskreis
                    txtResName.Text = .ResName
                    txtResNr.Text = .ResNummer

                    If .Feinstaub = "X" Then
                        chkFeinstaub.Checked = True
                    End If
                    If .Wunschkennzeichen1 = "Tageszulassung" Then
                        chkTagesul.Checked = True
                    Else
                        txtWKennzeichen1.Text = .Wunschkennzeichen1
                    End If

                    ddlZulassungsart.SelectedValue = .Zulassungsart


                    '***Versicherungsdaten***
                    txtVersGesellschaft.Text = .VersGesellschaft
                    txtEVBNummer.Text = .EVBNummer
                    txtEVBVon.Text = .EVBVon
                    txtEVBBis.Text = .EVBBis

                    '***Versand Schein/Schilder***
                    txtVssName1.Text = .VssName
                    txtVssName2.Text = .VssName2
                    txtVssStrasse.Text = .VssStrasse
                    txtVssPLZ.Text = .VssPLZ
                    txtVssOrt.Text = .VssOrt


                    '***Dropdown Halter Ländercodes***
                    drpShLand = GetDropDownLand(drpShLand)
                    drpShLand.SelectedValue = "DE"

                    '***Dropdown Versicherungsnehmer Ländercodes***
                    drpVersLand = GetDropDownLand(drpVersLand)
                    drpVersLand.SelectedValue = "DE"

                    '***Dropdown Schein/Schilder Ländercodes***
                    drpVSSLand = GetDropDownLand(drpVSSLand)
                    drpVSSLand.SelectedValue = "DE"

                    '***Dropdown Zulassungsart***
                    If objZulassung.tblZulassungsart Is Nothing Then
                        objZulassung.getZulassungsart(Me)
                    End If
                    ddlZulassungsart.DataSource = objZulassung.tblZulassungsart
                    ddlZulassungsart.DataTextField = "BEZEI"
                    ddlZulassungsart.DataValueField = "KVGR3"
                    ddlZulassungsart.DataBind()
                    ddlZulassungsart.SelectedValue = .Zulassungsart
                    ddlZulassungsart.Visible = Not chkTagesul.Checked

                End With

            End If
        End Sub
        Private Function GetDropDownLand(ByVal drpTemp As DropDownList) As DropDownList

            drpTemp.DataSource = objZulassung.Laender
            drpTemp.DataTextField = "FullDesc"
            drpTemp.DataValueField = "Land1"
            drpTemp.DataBind()

            Return drpTemp

        End Function

        Protected Sub btnZulkreis_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnZulkreis.Click
            If IsNothing(Session("objZulassung")) = False Then


                objZulassung = DirectCast(Session("objZulassung"), Zulassung1)

                If Len(txtShPLZ.Text) <> 5 Then
                    lblError.Text = "Bitte geben Sie eine 5-stellige PLZ ein."
                Else
                    objZulassung.getSTVA(Me.Page, txtShPLZ.Text)

                    If Not objZulassung.Kreis Is Nothing Then

                        If objZulassung.Kreis.Rows.Count > 0 Then
                            txtZulkreis.Text = objZulassung.Kreis.Rows(0)("ZKFZKZ").ToString()
                        Else
                            lblError.Text = "Für die eingegebene PLZ konnte kein Zulassungskreis ermittelt werden."
                            txtZulkreis.Text = ""
                        End If
                    End If
                End If

            End If
        End Sub

        Private Sub AddToSessionObject()

            With objZulassung
                '***Halter***
                .Halter = txtShName.Text
                .Halter2 = txtShName2.Text
                .HalterStrasse = txtShStrasse.Text
                .HalterPLZ = txtShPLZ.Text
                .HalterOrt = txtShOrt.Text
                .HalterLand = drpShLand.SelectedValue

                '***Zulassung***
                .Zulassungsdatum = txtZulassungsdatum.Text
                .Wunschkennzeichen1 = txtWKennzeichen1.Text
                .Wunschkennzeichen2 = txtWKennzeichen2.Text
                .Wunschkennzeichen3 = txtWKennzeichen3.Text
                .Zulassungskreis = txtZulkreis.Text
                .ResNummer = txtResNr.Text
                .ResName = txtResName.Text
                .Zulassungsart = ddlZulassungsart.SelectedValue
                .Zulassungsarttext = ddlZulassungsart.SelectedItem.Text

                If chkFeinstaub.Checked = True Then
                    .Feinstaub = "X"
                Else
                    .Feinstaub = String.Empty
                End If
                If chkTagesul.Checked = True Then
                    .Wunschkennzeichen1 = "Tageszulassung"
                End If

                '***Versicherungsdaten***
                .VersGesellschaft = txtVersGesellschaft.Text
                .VersNehmer = txtVersNehmer.Text
                .VersNehmerStrasse = txtVersStrasse.Text
                .VersNehmerPLZ = txtVersPLZ.Text
                .VersNehmerOrt = txtVersOrt.Text
                .VersNehmerLand = drpVersLand.SelectedValue
                .EVBNummer = txtEVBNummer.Text
                .EVBVon = txtEVBVon.Text
                .EVBBis = txtEVBBis.Text

                '***Versand Schein/Schilder***
                .VssName = txtVssName1.Text
                .VssName2 = txtVssName2.Text
                .VssStrasse = txtVssStrasse.Text
                .VssPLZ = txtVssPLZ.Text
                .VssOrt = txtVssOrt.Text
                .VssLand = drpVSSLand.SelectedValue

                Dim tmpRows As DataRow()
                tmpRows = objZulassung.Fahrzeuge.Select("AUSWAHL='99'")

                For Each tblrow As DataRow In tmpRows
                    If tmpRows.Length = 1 Then
                        tblrow("Wunschkennz1") = txtWKennzeichen1.Text
                        tblrow("Wunschkennz2") = txtWKennzeichen2.Text
                        tblrow("Wunschkennz3") = txtWKennzeichen3.Text
                        tblrow("ResName") = txtResName.Text
                        tblrow("ResNr") = txtResNr.Text
                    End If
                    If chkTagesul.Checked = True Then
                        tblrow("Wunschkennz1") = "Tageszulassung"
                    End If
                    tblrow("Zulkreis") = txtZulkreis.Text
                Next



            End With

            Session("objZulassung") = objZulassung

        End Sub

        Private Function PflichtfelderValidieren() As Boolean
            Dim PflichtfeldError As Boolean
            PflichtfeldWarning = False
            Dim ErrorElse As String = String.Empty

            'Wurden alle Pflichtfelder korrekt gefüllt?



            If Trim(txtShName.Text).Length = 0 Then
                PflichtfeldError = True
                txtShName.BorderColor = Color.Red
            End If

            If Trim(txtShStrasse.Text).Length = 0 Then
                PflichtfeldError = True
                txtShStrasse.BorderColor = Color.Red
            End If

            If Trim(txtShPLZ.Text).Length = 0 Then
                PflichtfeldError = True
                txtShPLZ.BorderColor = Color.Red
            End If

            If Trim(txtShOrt.Text).Length = 0 Then
                PflichtfeldError = True
                txtShOrt.BorderColor = Color.Red
            End If

            If Trim(txtZulassungsdatum.Text).Length = 0 Then
                PflichtfeldError = True
                txtZulassungsdatum.BorderColor = Color.Red
            ElseIf IsDate(txtZulassungsdatum.Text) = False Then
                PflichtfeldError = True
                txtZulassungsdatum.BorderColor = Color.Red
                ErrorElse = "Ungültiges Zulassungsdatum."
            ElseIf CDate(txtZulassungsdatum.Text) = Today.AddDays(1) Then
                PflichtfeldWarning = True
            ElseIf CDate(txtZulassungsdatum.Text) < Today.AddDays(2) Then
                PflichtfeldError = True
                txtZulassungsdatum.BorderColor = Color.Red
                ErrorElse = "Das Zulassungsdatum muss 2 Tage in der Zukunft liegen."
            End If

            If Not Trim(txtEVBNummer.Text).Length = 7 Then
                PflichtfeldError = True
                txtEVBNummer.BorderColor = Color.Red
                ErrorElse = "Die EVB-Nummer muss 7-stellig sein."
            End If

            If Trim(txtVersNehmer.Text).Length = 0 Then
                PflichtfeldError = True
                txtVersNehmer.BorderColor = Color.Red
            End If

            If Trim(txtVersStrasse.Text).Length = 0 Then
                PflichtfeldError = True
                txtVersStrasse.BorderColor = Color.Red
            End If

            If Trim(txtVersPLZ.Text).Length = 0 Then
                PflichtfeldError = True
                txtVersPLZ.BorderColor = Color.Red
            End If

            If Trim(txtVersOrt.Text).Length = 0 Then
                PflichtfeldError = True
                txtVersOrt.BorderColor = Color.Red
            End If
            If PflichtfeldError = True Then
                lblError.Text = ERROR_MESSAGE_PFLICHTFELD & " " & ErrorElse
                Return PflichtfeldError
            End If
            If PflichtfeldError = True Then
                lblError.Text = ERROR_MESSAGE_PFLICHTFELD & " " & ErrorElse
                Return PflichtfeldError
            End If
        End Function

        Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSearch.Click
            If PflichtfelderValidieren() = False Then
                If PflichtfeldWarning = True Then
                    MPEZulDate.Show()
                Else
                    If objZulassung.ZulCount > 1 And Not objZulassung.Wunschkennzeichen1 = "Tageszulassung" Then
                        Response.Redirect("Change01_4.aspx?AppID=" & Session("AppID").ToString)
                    Else
                        Response.Redirect("Change01_5.aspx?AppID=" & Session("AppID").ToString)
                    End If
                End If


            End If
        End Sub

        Protected Sub chkTagesul_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkTagesul.CheckedChanged
            'bei Tageszulassung immer "NormalFahrzeug" im Hintergrund übertragen. JJU20100811
            ddlZulassungsart.SelectedValue = "3"
            If chkTagesul.Checked Then
                ddlZulassungsart.Visible = False
            Else
                ddlZulassungsart.Visible = True
            End If
        End Sub

        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
            SetEndASPXAccess(Me)
        End Sub

        Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
            SetEndASPXAccess(Me)
        End Sub

        Protected Sub btnYes_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnYes.Click
            If objZulassung.ZulCount > 1 And Not objZulassung.Wunschkennzeichen1 = "Tageszulassung" Then
                Response.Redirect("Change01_4.aspx?AppID=" & Session("AppID").ToString)
            Else
                Response.Redirect("Change01_5.aspx?AppID=" & Session("AppID").ToString)
            End If
        End Sub

    End Class
End Namespace