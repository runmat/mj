Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common

Namespace Zulassung
    Partial Public Class Change01_5
        Inherits System.Web.UI.Page

        Private m_User As Base.Kernel.Security.User
        Private m_App As Base.Kernel.Security.App
        Private objZulassung As Zulassung1

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            m_User = GetUser(Me)
            FormAuth(Me, m_User, True)
            GetAppIDFromQueryString(Me)

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            m_App = New Security.App(m_User)
            GridNavigation1.setGridElment(GridView1)

            If IsNothing(Session("objZulassung")) Then Response.Redirect("Change01.aspx?AppID=" & Session("AppID").ToString)

            lnkFahrzeugsuche.NavigateUrl = "Change01.aspx?AppID=" & Session("AppID").ToString
            lnkFahrzeugauswahl.NavigateUrl = "Change01_2.aspx?AppID=" & Session("AppID").ToString
            lnkAdressen.NavigateUrl = "Change01_3.aspx?AppID=" & Session("AppID").ToString
            lnkWunschkennz.NavigateUrl = "Change01_4.aspx?AppID=" & Session("AppID").ToString

            objZulassung = CType(Session("objZulassung"), Zulassung1)
            Try
                If Not IsPostBack Then
                    FillControls()
                    FillGrid(0)
                End If
            Catch ex As Exception
                lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten."
            End Try
        End Sub
        Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")

            If objZulassung.Fahrzeuge.Rows.Count = 0 Then
                Result.Visible = False
            Else
                Result.Visible = True

                Dim tmpDataView As New DataView()
                tmpDataView = objZulassung.Fahrzeuge.DefaultView
                tmpDataView.RowFilter = "AUSWAHL = '99'"
                If objZulassung.Fahrzeuge.Select("AUSWAHL = '99'").Length > 1 Then
                    lnkWunschkennz.Visible = True
                Else
                    lnkWunschkennz.Visible = False
                End If
                Dim intTempPageIndex As Int32 = intPageIndex
                Dim strTempSort As String = ""
                Dim strDirection As String = ""

                If strSort.Trim(" "c).Length > 0 Then
                    intTempPageIndex = 0
                    strTempSort = strSort.Trim(" "c)
                    If (ViewState("Sort") Is Nothing) OrElse (ViewState("Sort").ToString = strTempSort) Then
                        If ViewState("Direction") Is Nothing Then
                            strDirection = "desc"
                        Else
                            strDirection = ViewState("Direction").ToString
                        End If
                    Else
                        strDirection = "desc"
                    End If

                    If strDirection = "asc" Then
                        strDirection = "desc"
                    Else
                        strDirection = "asc"
                    End If

                    ViewState("Sort") = strTempSort
                    ViewState("Direction") = strDirection
                Else
                    If Not ViewState("Sort") Is Nothing Then
                        strTempSort = ViewState("Sort").ToString
                        If ViewState("Direction") Is Nothing Then
                            strDirection = "asc"
                            ViewState("Direction") = strDirection
                        Else
                            strDirection = ViewState("Direction").ToString
                        End If
                    End If

                End If
                If Not strTempSort.Length = 0 Then
                    tmpDataView.Sort = strTempSort & " " & strDirection
                End If

                GridView1.PageIndex = intTempPageIndex

                GridView1.DataSource = tmpDataView
                GridView1.DataBind()
            End If
        End Sub
        Private Sub FillControls()

            If IsNothing(Session("objZulassung")) = False Then

                With objZulassung

                    '***Halter***
                    lblShName.Text = .Halter & " " & .Halter2
                    lblShStrasse.Text = .HalterStrasse
                    lblShOrt.Text = .HalterPLZ & " " & .HalterOrt

                    '***Versicherungsnehmer***
                    lblVersName.Text = .VersNehmer
                    lblVersStrasse.Text = .VersNehmerStrasse
                    lblVersOrt.Text = .VersNehmerPLZ & " " & .VersNehmerOrt
                    'lblVersAnsprech.Text = .VersAnsprechpartner
                    'lblVersTelefon.Text = .VersTelefon
                    'lblVersMail.Text = .VersMail


                    ''***Zulassung***
                    lblZulDat.Text = .Zulassungsdatum
                    lblZulassungsart.Text = .Zulassungsarttext
                    If .Feinstaub = "X" Then
                        chkFeinstaub.Checked = True
                    End If

                    If .Wunschkennzeichen1 = "Tageszulassung" Then
                        chkTagesZul.Checked = True
                        tr_Zulassungsart.Visible = False
                    End If

                    '***Versicherungsdaten***
                    lblVersGesellschaft.Text = .VersGesellschaft
                    lblEvbNr.Text = .EVBNummer
                    lblEvbVon.Text = .EVBVon
                    lblEvbBis.Text = .EVBBis

                    lblAnzahl.Text = "Sie haben " & .Fahrzeuge.Select("AUSWAHL='99'").Length & " Fahrzeuge zur Zulassung ausgewählt!"
                End With

            End If
        End Sub
        Private Sub DoSubmit()
            If IsNothing(Session("objZulassung")) = False Then

                With objZulassung

                    '### Wunschkennzeichen/Reservierung aus Tabelle ###
                    Dim tmpRows() As DataRow
                    tmpRows = .Fahrzeuge.Select("AUSWAHL='99'")
                    If tmpRows.Length > 0 Then
                        For Each dRow As DataRow In tmpRows
                            .EquiNr = dRow("EQUNR").ToString
                            .Fahrgestellnr = dRow("CHASSIS_NUM").ToString
                            .NummerZBII = dRow("TIDNR").ToString
                            .Wunschkennzeichen1 = dRow("Wunschkennz1").ToString
                            .Wunschkennzeichen2 = dRow("Wunschkennz2").ToString
                            .Wunschkennzeichen3 = dRow("Wunschkennz3").ToString
                            .ResNummer = dRow("ResNr").ToString
                            .ResName = dRow("ResName").ToString
                            .Change(Session("AppID").ToString, Session.SessionID.ToString, Me)
                            If .Status = 0 Then
                                If .AuftragsNr <> "" Then
                                    dRow("bem") = "Vorgang OK! Auftrag angelegt!"
                                    dRow("Auftragsnr") = .AuftragsNr.TrimStart("0"c)
                                Else
                                    dRow("bem") = "Fehler! Auftrag nicht angelegt!"
                                    dRow("Auftragsnr") = ""
                                End If
                            Else
                                lblError.Text = "Beim Senden der Daten ist ein Fehler aufgetreten. Detail:" & .Message
                                Exit Sub
                            End If
                        Next
                        FillGrid(0)
                        pnlHalter.Visible = False
                        pnlVersicherer.Visible = False
                        pnlZulDaten.Visible = False
                        For i As Integer = 4 To 8
                            GridView1.Columns(i).Visible = False
                        Next
                        GridView1.Columns(9).Visible = True
                        GridView1.Columns(10).Visible = True
                        cmdContinue.Visible = False

                        Session.Remove("objZulassung")
                    End If
                End With

            End If
        End Sub
        Protected Sub cmdContinue_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdContinue.Click
            DoSubmit()
        End Sub
        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
            SetEndASPXAccess(Me)
        End Sub

        Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
            SetEndASPXAccess(Me)
        End Sub

    End Class
End Namespace