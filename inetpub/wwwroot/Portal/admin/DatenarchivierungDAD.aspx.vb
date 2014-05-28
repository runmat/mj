Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Partial Public Class DatenarchivierungDAD
    Inherits System.Web.UI.Page

    Private m_User As User
    Private m_App As App
    Private dtSearch As New DataTable
    Private dtTest As New DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        ucStyles.TitleText = "DAD Datenarchivierung"
        AdminAuth(Me, m_User, AdminLevel.Organization)
    End Sub

    Protected Sub cmdCreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCreate.Click
        lblError.Visible = False
        Suchen()
    End Sub

    Protected Sub Suchen()

        Dim strBrief As String = txtBrief.Text.Trim
        Dim strFahrgestellnummer As String = txtFahrgestellnummer.Text.Trim
        Dim strKennzeichen As String = txtKennzeichen.Text.Trim

        If strBrief = "" And strFahrgestellnummer = "" And strKennzeichen = "" Then
            lblError.Text = "Es wurde kein Suchkriterium gewählt."
            lblError.Visible = True
        ElseIf Not strBrief = "" And strFahrgestellnummer = "" And strKennzeichen = "" Then

            dtSearch = GetAuftragsdaten(strBrief)

            If dtSearch.Rows.Count = 1 Then
                ShowSearch(dtSearch.Rows(0))
                ShowKunde(dtSearch.Rows(0))
                ShowDaten(dtSearch.Rows(0))

                cmdBack.Visible = True
                cmdCreate.Visible = False
            ElseIf dtSearch.Rows.Count > 1 Then
                SwitchSearch(False)
                SelectKunden(dtSearch)

                cmdBack.Visible = True
                cmdCreate.Visible = False
            Else
                lblError.Text = "Es wurde kein Datensatz gefunden"
                lblError.Visible = True
            End If

        ElseIf strBrief = "" And Not strFahrgestellnummer = "" And strKennzeichen = "" Then

            dtSearch = GetAuftragsdaten(, strFahrgestellnummer)

            If dtSearch.Rows.Count = 1 Then
                ShowSearch(dtSearch.Rows(0))
                ShowKunde(dtSearch.Rows(0))
                ShowDaten(dtSearch.Rows(0))

                cmdBack.Visible = True
                cmdCreate.Visible = False
            ElseIf dtSearch.Rows.Count > 1 Then
                SwitchSearch(False)
                SelectKunden(dtSearch)

                cmdBack.Visible = True
                cmdCreate.Visible = False
            Else
                lblError.Text = "Es wurde kein Datensatz gefunden"
                lblError.Visible = True
            End If

        ElseIf strBrief = "" And strFahrgestellnummer = "" And Not strKennzeichen = "" Then

            dtSearch = GetAuftragsdaten(, , strKennzeichen)

            If dtSearch.Rows.Count = 1 Then
                ShowSearch(dtSearch.Rows(0))
                ShowKunde(dtSearch.Rows(0))
                ShowDaten(dtSearch.Rows(0))

                cmdBack.Visible = True
                cmdCreate.Visible = False
            ElseIf dtSearch.Rows.Count > 1 Then
                SwitchSearch(False)
                SelectKunden(dtSearch)

                cmdBack.Visible = True
                cmdCreate.Visible = False
            Else
                lblError.Text = "Es wurde kein Datensatz gefunden"
                lblError.Visible = True
            End If
        ElseIf Not strBrief = "" And Not strFahrgestellnummer = "" And strKennzeichen = "" Then
            lblError.Text = "Es kann nur ein Suchkriterium eingegeben werden."
            lblError.Visible = True
        ElseIf Not strBrief = "" And strFahrgestellnummer = "" And Not strKennzeichen = "" Then
            lblError.Text = "Es kann nur ein Suchkriterium eingegeben werden."
            lblError.Visible = True
        ElseIf strBrief = "" And Not strFahrgestellnummer = "" And Not strKennzeichen = "" Then
            lblError.Text = "Es kann nur ein Suchkriterium eingegeben werden."
            lblError.Visible = True
        ElseIf Not strBrief = "" And Not strFahrgestellnummer = "" And Not strKennzeichen = "" Then
            lblError.Text = "Es kann nur ein Suchkriterium eingegeben werden."
            lblError.Visible = True
        End If
    End Sub

    Private Sub ShowDaten(ByVal Row As DataRow)
        TblFahrzeugdaten.Visible = True

        lblKundenname.Text = Row("BEZEICHNUNG").ToString

        lblAuftragsnummer.Text = Row("AUFTRAGSNUMMER").ToString
        lblFarbziffer.Text = Row("FARBZIFFER").ToString

        If TypeOf Row("DATUMERSTZULASSUNG") Is DBNull Then
            lblDatumZulassung.Text = ""
        Else
            lblDatumZulassung.Text = CDate(Row("DATUMERSTZULASSUNG")).Date
        End If

        If TypeOf Row("CARPORTLISTEERFASST") Is DBNull Then
            lblDatumCarportliste.Text = ""
        Else
            lblDatumCarportliste.Text = CDate(Row("CARPORTLISTEERFASST")).Date
        End If


        If TypeOf Row("ABMELDUNGZURUECK") Is DBNull Then
            lblAbmeldungLBV.Text = ""
        Else
            lblAbmeldungLBV.Text = CDate(Row("ABMELDUNGZURUECK")).Date
        End If

        If TypeOf Row("BRIEFNACHABMELDUNGVERSENDET") Is DBNull Then
            lblDatumBriefVersendet.Text = ""
        Else
            lblDatumBriefVersendet.Text = CDate(Row("BRIEFNACHABMELDUNGVERSENDET")).Date
        End If


        lblUnitnummer.Text = Row("UNITNR").ToString
        lblHaltername1.Text = Row("Haltername1").ToString
        lblHaltername2.Text = Row("Haltername2").ToString

        If TypeOf Row("DATUMABMELDUNG") Is DBNull Then
            lblDatumAbmeldung.Text = ""
        Else
            lblDatumAbmeldung.Text = CDate(Row("DATUMABMELDUNG")).Date
        End If


        If TypeOf Row("SCHLUESSELVERSENDET") Is DBNull Then
            lblDatumSchluesselVersendet.Text = ""
        Else
            lblDatumSchluesselVersendet.Text = CDate(Row("SCHLUESSELVERSENDET")).Date
        End If

        If TypeOf Row("HAENDLERVERSANDGEDRUCKT") Is DBNull Then
            lblDatumHaendlerversandGedruckt.Text = ""
        Else
            lblDatumHaendlerversandGedruckt.Text = CDate(Row("HAENDLERVERSANDGEDRUCKT")).Date
        End If
        lblHaendlerName1.Text = Row("BriefempfaengerName1").ToString
        lblHaendlerName2.Text = Row("BriefempfaengerName2").ToString
        lblHaendlerOrt.Text = Row("BriefempfaengerOrt").ToString
        lblHaendlerPLZ.Text = Row("BriefempfaengerPLZ").ToString
        lblHaendlerStraße.Text = Row("BriefempfaengerStrasse").ToString

    End Sub

    Private Sub ShowSearch(ByVal Row As DataRow)
        SwitchSearch(False)

        lblBrief.Text = Row("FAHRZEUGBRIEFNUMMER")
        lblFahrgestellnummer.Text = Row("FAHRGESTELLNUMMER")
        lblKennzeichen.Text = Row("KENNZEICHEN")
    End Sub

    Private Sub ShowKunde(ByVal Row As DataRow)
        TblKundenauswahl.Visible = True
        ddlKunde.Visible = False
        cmdBack.Visible = False
        lblKundenname.Visible = True
        cmdKundenWaehlen.Visible = False

        lblKundenname.Text = Row("BEZEICHNUNG")
    End Sub

    Private Sub SwitchSearch(ByVal BoxVisible As Boolean)
        If BoxVisible = True Then
            txtBrief.Visible = True
            txtFahrgestellnummer.Visible = True
            txtKennzeichen.Visible = True

            lblBrief.Visible = False
            lblFahrgestellnummer.Visible = False
            lblKennzeichen.Visible = False
        Else
            txtBrief.Visible = False
            txtFahrgestellnummer.Visible = False
            txtKennzeichen.Visible = False

            lblBrief.Visible = True
            lblFahrgestellnummer.Visible = True
            lblKennzeichen.Visible = True
        End If
    End Sub

    Private Sub SelectKunden(ByVal Kundendaten As DataTable)
        TblKundenauswahl.Visible = True
        ddlKunde.Visible = True
        lblKundenname.Visible = False
        cmdKundenWaehlen.Visible = True
        ddlKunde.DataSource = Kundendaten
        ddlKunde.DataTextField = "BEZEICHNUNG"
        ddlKunde.DataValueField = "ID"
        ddlKunde.DataBind()
        ' Daten for Refresh sichern
        Session("SearchTable") = dtSearch
    End Sub

    Protected Sub cmdBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdBack.Click
        SwitchSearch(True)
        TblKundenauswahl.Visible = False
        TblFahrzeugdaten.Visible = False
        cmdBack.Visible = False
        cmdCreate.Visible = True
    End Sub

    Protected Sub cmdKundenWaehlen_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdKundenWaehlen.Click
        dtSearch = Session("SearchTable")
        For Each Row As DataRow In dtSearch.Rows()
            If Row("ID") = ddlKunde.SelectedValue Then
                ShowSearch(Row)
                ShowDaten(Row)
                ShowKunde(Row)
                Exit For
            End If
        Next
        cmdKundenWaehlen.Visible = False
        cmdBack.Visible = True
    End Sub

    Protected Function GetAuftragsdaten(Optional ByVal Briefnummer As String = "", Optional ByVal Fahrgestellnummer As String = "", Optional ByVal Kennzeichen As String = "") As DataTable
        Dim dt As New DataTable

        Dim cn As New SqlClient.SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionstringDADArchiv"))
        cn.Open()

        If Briefnummer <> "" Then

            Dim da As New SqlClient.SqlDataAdapter("SELECT * FROM vwGrunddaten INNER JOIN Kennzeichen ON vwGrunddaten.ID = Kennzeichen.AUFTRAGSDATENID " & _
                                                   "WHERE FAHRZEUGBRIEFNUMMER like '" & Briefnummer & "'", cn)
            da.Fill(dt)

        ElseIf Fahrgestellnummer <> "" Then

            Dim da As New SqlClient.SqlDataAdapter("SELECT * FROM vwGrunddaten INNER JOIN Kennzeichen ON vwGrunddaten.ID = Kennzeichen.AUFTRAGSDATENID " & _
                                                   "WHERE FAHRGESTELLNUMMER like '" & Fahrgestellnummer & "'", cn)
            da.Fill(dt)

        ElseIf Kennzeichen <> "" Then

            Dim da As New SqlClient.SqlDataAdapter("SELECT * FROM vwGrunddaten INNER JOIN Kennzeichen ON vwGrunddaten.ID = Kennzeichen.AUFTRAGSDATENID " & _
                                                   "WHERE KENNZEICHEN like '" & Kennzeichen & "'", cn)
            da.Fill(dt)

        End If
        cn.Close()

        Return dt
    End Function
End Class