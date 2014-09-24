Imports CKG.Base.Kernel.Common.Common

Partial Public Class Change02
    Inherits Page
    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private m_report As Zul_Sperr_Entsperr

    Protected WithEvents ucStyles As Portal.PageElements.Styles
    Protected WithEvents ucHeader As Portal.PageElements.Header

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)
            Literal1.Text = ""
            If Not IsPostBack Then
                DoSubmit()
                If rbWI.Checked Then
                    Dim List As ListItem
                    For Each List In ddlVerwZweck.Items
                        If List.Text = "MF" Then
                            List.Selected = True
                        End If
                    Next
                End If

            ElseIf Not Session("App_Report") Is Nothing Then

                m_report = CType(Session("App_Report"), Zul_Sperr_Entsperr)
                Session("App_ResultTablePDIs") = m_report.ResultPDIs
                Session("App_ResultTable") = m_report.ResultTable
                Session("App_Result") = m_report.Result
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try


    End Sub

    Private Sub DoSubmit()
        Session("lnkExcel") = ""

        Try
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"

            m_report = New Zul_Sperr_Entsperr(m_User, m_App, strFileName)

            lblError.Text = ""


            m_Report.Fill(Session("AppID").ToString, Session.SessionID.ToString)
            ZulkreisSel.Visible = True

            If Not m_Report.Status = 0 Then
                lblError.Text = m_Report.Message
            Else
                If m_report.Result.Rows.Count = 0 Then
                    lblError.Text = "Keine Daten verfügbar!"
                    ZulkreisSel.Visible = False
                Else

                    Session("App_ResultTablePDIs") = m_report.ResultPDIs
                    Session("App_Report") = m_report
                    Session("App_ResultTable") = m_report.Result

                    FillControlls()
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try

    End Sub

    Private Sub FillControlls(Optional ByVal FlagFilter As String = "")

        If Session("App_Report") IsNot Nothing Then
            m_report = CType(Session("App_Report"), Zul_Sperr_Entsperr)
        End If

        Dim dvCarports As DataView = m_report.ResultPDIs.DefaultView
        Dim dvHersteller As DataView = m_report.Hersteller.DefaultView
        Dim dvLiefertermin As DataView = m_report.Liefermonat.DefaultView
        Dim dvBereifung As DataView = m_report.Bereifung.DefaultView
        Dim dvGetriebe As DataView = m_report.Getriebe.DefaultView
        Dim dvKraftstoff As DataView = m_report.Kraftstoff.DefaultView
        Dim dvNavi As DataView = m_report.Navi.DefaultView
        Dim dvFarbe As DataView = m_report.Farbe.DefaultView
        Dim dvVermiet As DataView = m_report.Vermiet.DefaultView
        Dim dvFzgArt As DataView = m_report.FzgArt.DefaultView
        Dim dvAufbauArt As DataView = m_report.AufbauArt.DefaultView
        Dim dvHaendlernr As DataView = m_report.Haendlernr.DefaultView
        Dim dvHaendlername As DataView = m_report.Handlername.DefaultView
        Dim dvEKIndikator As DataView = m_report.EKIndikator.DefaultView
        Dim dvVerwZweck As DataView = m_report.VerwZweck.DefaultView
        Dim dvOwnerCode As DataView = m_report.HOwnerCode.DefaultView
        Dim dvSperrdat As DataView = m_report.Sperrdat.DefaultView
        Dim dvZulKreis As DataView = m_report.ZulKreis.DefaultView

        If FlagFilter.Contains("Carportnr") = False Then
            If dvCarports.Count > 0 Then
                BoundControls(dvCarports, ddlCarports, "Carport", "Carportnr", "Carportnr")
            End If
        End If

        If FlagFilter.Contains("Hersteller_ID_Avis") = False Then
            If dvHersteller.Count > 0 Then
                BoundControls(dvHersteller, ddlHersteller, "Hersteller_ID_Avis", "Hersteller_ID_Avis", "Hersteller_ID_Avis")
            End If
        End If

        If FlagFilter.Contains("Liefermonat") = False Then
            If dvLiefertermin.Count > 0 Then
                BoundControls(dvLiefertermin, ddlLiefermonat, "Liefermonat", "ID", "Liefermonat")
            End If
        End If

        If FlagFilter.Contains("Reifenart") = False Then
            If dvBereifung.Count > 0 Then
                BoundControls(dvBereifung, ddlBereifung, "REIFENART", "ID", "REIFENART")
            End If
        End If

        If FlagFilter.Contains("Antriebsart") = False Then
            If dvGetriebe.Count > 0 Then
                BoundControls(dvGetriebe, ddlGetriebe, "ANTRIEBSART", "ID", "ANTRIEBSART")
            End If
        End If

        If FlagFilter.Contains("Kraftstoffart") = False Then
            If dvKraftstoff.Count > 0 Then
                BoundControls(dvKraftstoff, ddlKraftstoff, "Kraftstoffart", "ID", "Kraftstoffart")
            End If
        End If

        If FlagFilter.Contains("Navigation") = False Then
            If dvNavi.Count > 0 Then
                BoundControls(dvNavi, ddlNavi, "Navigation", "ID", "Navigation")
            End If
        End If

        If FlagFilter.Contains("Farbe") = False Then
            If dvFarbe.Count > 0 Then
                BoundControls(dvFarbe, ddlFarbe, "FARBE", "ID", "FARBE")
            End If
        End If

        If FlagFilter.Contains("Vermietgruppe") = False Then
            If dvVermiet.Count > 0 Then
                BoundControls(dvVermiet, ddlVermiet, "Vermietgruppe", "ID", "Vermietgruppe")
            End If
        End If

        If FlagFilter.Contains("Fahrzeugart") = False Then
            If dvFzgArt.Count > 0 Then
                BoundControls(dvFzgArt, ddlFzgArt, "Fahrzeugart", "ID", "Fahrzeugart")
            End If
        End If

        If FlagFilter.Contains("Aufbauart") = False Then
            If dvAufbauArt.Count > 0 Then
                BoundControls(dvAufbauArt, ddlAufbauArt, "AUFBAUART", "ID", "AUFBAUART")
            End If
        End If

        If FlagFilter.Contains("HaendlerId") = False Then
            If dvHaendlernr.Count > 0 Then
                BoundControls(dvHaendlernr, ddlHaendlernr, "HaendlerId", "ID", "HaendlerId")
            End If
        End If

        If FlagFilter.Contains("Haendler_Kurzname") = False Then
            If dvHaendlername.Count > 0 Then
                BoundControls(dvHaendlername, ddlHaendlername, "Haendler_Kurzname", "ID", "Haendler_Kurzname")
            End If
        End If

        If FlagFilter.Contains("Einkaufsindikator") = False Then
            If dvEKIndikator.Count > 0 Then
                BoundControls(dvEKIndikator, ddlEKIndikator, "Einkaufsindikator", "ID", "Einkaufsindikator")
            End If
        End If

        If FlagFilter.Contains("Verwendungszweck") = False Then
            If dvVerwZweck.Count > 0 Then
                BoundControls(dvVerwZweck, ddlVerwZweck, "VERWENDUNGSZWECK", "ID", "VERWENDUNGSZWECK")
            End If
        End If

        If FlagFilter.Contains("Owner_Code") = False Then
            If dvOwnerCode.Count > 0 Then
                BoundControls(dvOwnerCode, ddlOwnerCode, "OWNER_CODE", "ID", "OWNER_CODE")
            End If
        End If

        If FlagFilter.Contains("Datum_zur_Sperre") = False Then
            If dvSperrdat.Count > 0 Then
                BoundControls(dvSperrdat, ddlSperrdat, "Datum_zur_Sperre", "ID", "Datum_zur_Sperre")
            End If
        End If

        If FlagFilter.Contains("ZULASSUNGSORT") = False Then
            If dvZulKreis.Count > 0 Then
                dvZulKreis.RowFilter = "ZULASSUNGSORT<>'WI'"
                BoundControls(dvZulKreis, ddlZulKreis, "ZULASSUNGSORT", "ID", "ZULASSUNGSORT")
            End If
        End If
    End Sub

    Private Sub BoundControls(ByVal View As DataView, ByVal Dropdown As DropDownList, ByVal Text As String, _
                                                ByVal Value As String, ByVal Sort As String)
        If View.Count > 0 Then
            View.Sort = Sort
            With Dropdown
                .DataSource = View
                .DataTextField = Text
                .DataValueField = Value
                .DataBind()
            End With
        End If
    End Sub

    Protected Sub lbtn_Weiter_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtn_Weiter.Click
        Dim iStatus As Integer
        If rbWI.Checked Then
            m_report.DezZul = False
            m_report.FilterString = "ZULASSUNGSORT = 'WI' AND Verwendungszweck='MF'"
            iStatus = m_report.Filter("ZULASSUNGSORT = 'WI' AND Verwendungszweck='MF'")
            If iStatus <> 0 Then
                lbtn_backZul.Visible = True
                ZulkreisSel.Visible = False
                SelectionRow.Visible = True
                ButtonRow.Visible = True
                lbtn_backZul.Text = "Zulassungsort WI ändern"
                Session("AppZulKreis") = "WI"
                FillControlls("ZULASSUNGSORT = 'WI' AND Verwendungszweck='MF'")
                lblMessage.Text = "Anzahl gefundener Fahrzeuge: " & iStatus
                Dim ListIndex As Integer
                For i As Integer = 0 To ddlVerwZweck.Items.Count - 1
                    ddlVerwZweck.Items.Item(i).Selected = False
                    If ddlVerwZweck.Items.Item(i).Text = "MF" Then
                        ListIndex = i
                    End If
                Next
                ddlVerwZweck.Items.Item(ListIndex).Selected = True
            Else
                lblError.Text = "Keine Ergbnisse zu den gewählten Kriterien!"
            End If

        ElseIf rbDez.Checked AndAlso ddlZulKreis.SelectedIndex > 0 Then
            m_report.DezZul = True
            m_report.FilterString = "ZULASSUNGSORT = '" & ddlZulKreis.SelectedItem.Text & "'"
            iStatus = m_report.Filter("ZULASSUNGSORT = '" & ddlZulKreis.SelectedItem.Text & "'")
            If iStatus <> 0 Then
                lbtn_backZul.Text = "Zulassungsort " & ddlZulKreis.SelectedItem.Text & " ändern"
                ZulkreisSel.Visible = False
                SelectionRow.Visible = True
                lbtn_backZul.Visible = True
                ButtonRow.Visible = True
                ddlVerwZweck.Items.Item(0).Selected = True
                Session("AppZulKreis") = ddlZulKreis.SelectedItem.Text
                FillControlls("ZULASSUNGSORT = '" & ddlZulKreis.SelectedItem.Text & "'")
                lblMessage.Text = "Anzahl gefundener Fahrzeuge: " & iStatus
            Else
                lblError.Text = "Keine Ergbnisse zu den gewählten Kriterien!"
            End If

        End If
        Session("App_Report") = m_report
    End Sub

    Protected Sub cmdBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdBack.Click
        Session("App_Report") = Nothing
        Session("App_ResultTable") = Nothing
        Session("App_ResultTablePDIs") = Nothing
        Session("App_Result") = Nothing
        Response.Redirect("/Portal/(S(" & Session.SessionID & "))/Start/Selection.aspx")

    End Sub

    Protected Sub lbtn_backZul_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtn_backZul.Click
        lbtn_backZul.Visible = False
        ZulkreisSel.Visible = True
        SelectionRow.Visible = False
        ButtonRow.Visible = False
    End Sub

    Protected Sub cmdCreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCreate.Click
        m_report.FilterCarports()

        If lblMessage.Text.Length > 0 Then
            Dim NumString As String = ""
            For i As Integer = 1 To lblMessage.Text.Length

                If IsNumeric(Mid(lblMessage.Text, i, 1)) = True Then
                    NumString += Mid(lblMessage.Text, i, 1)

                End If

            Next

            If NumString.Length > 0 Then
                If CInt(NumString) > 150 Then
                    lblError.Text = "Zuviele Datensätze: Bitte wählen Sie max. 150 Fahrzeuge aus."
                    Exit Sub
                End If
            End If

        End If

        Response.Redirect("Change02_1.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub Setfilter()
        Dim iStatus As Integer
        Dim sFilter As String = "ZULASSUNGSORT = '" & Session("AppZulKreis").ToString & "'"
        Dim sFilterproof As String = proofDropdowns()

        If sFilterproof <> "" Then
            sFilter = sFilter & " And " & sFilterproof
            m_report.FilterString = sFilter
            iStatus = m_report.Filter(sFilter)
            If iStatus <> 0 Then
                FillControlls(sFilter)
                If SelOpen2.Value = "O" Then insertScript()
                lblMessage.Text = "Anzahl gefundener Fahrzeuge: " & iStatus
            Else
                lblError.Text = "Keine Treffer zu den gewählten Kriterien!"
            End If

        Else
            m_report.FilterString = sFilter
            iStatus = m_report.Filter(sFilter)
            If iStatus <> 0 Then
                FillControlls(sFilter)
                If SelOpen2.Value = "O" Then insertScript()
                lblMessage.Text = "Anzahl gefundener Fahrzeuge: " & iStatus
            Else
                lblError.Text = "Keine Treffer zu den gewählten Kriterien!"
            End If
        End If
        Session("App_Report") = m_report
    End Sub

    Protected Sub ddlCarports_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlCarports.SelectedIndexChanged
        Setfilter()
    End Sub

    Private Sub insertScript()

        Literal1.Text = "						<script language=""Javascript"">" & vbCrLf
        Literal1.Text &= "						  <!-- //" & vbCrLf
        Literal1.Text &= "							slidedown('Suche2');" & vbCrLf
        Literal1.Text &= "						  //-->" & vbCrLf
        Literal1.Text &= "						</script>" & vbCrLf

    End Sub

    Protected Sub ddlHersteller_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlHersteller.SelectedIndexChanged
        Setfilter()
    End Sub

    Protected Sub ddlLiefermonat_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlLiefermonat.SelectedIndexChanged
        Setfilter()
    End Sub

    Protected Sub ddlBereifung_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlBereifung.SelectedIndexChanged
        Setfilter()
    End Sub

    Protected Sub ddlGetriebe_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlGetriebe.SelectedIndexChanged
        Setfilter()
    End Sub

    Protected Sub ddlKraftstoff_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlKraftstoff.SelectedIndexChanged
        Setfilter()
    End Sub

    Private Function proofDropdowns() As String
        Dim sFilter As String = ""

        If ddlCarports.SelectedItem.Value <> "-1" Then
            sFilter = "Carportnr='" & ddlCarports.SelectedItem.Value & "'"
        End If
        If ddlHersteller.SelectedItem.Value <> "- keine Auswahl -" Then
            If sFilter <> "" Then
                sFilter = sFilter & " AND Hersteller_ID_Avis='" & ddlHersteller.SelectedItem.Text & "'"
            Else
                sFilter = "Hersteller_ID_Avis='" & ddlHersteller.SelectedItem.Text & "'"
            End If
        End If
        If ddlLiefermonat.SelectedItem.Value <> "-1" Then
            If sFilter <> "" Then
                sFilter = sFilter & " AND Liefermonat='" & ddlLiefermonat.SelectedItem.Text & "'"
            Else
                sFilter = "Liefermonat='" & ddlLiefermonat.SelectedItem.Text & "'"
            End If
        End If
        If ddlBereifung.SelectedItem.Value <> "-1" Then
            If sFilter <> "" Then
                sFilter = sFilter & " AND Reifenart='" & ddlBereifung.SelectedItem.Text & "'"
            Else
                sFilter = "Reifenart='" & ddlBereifung.SelectedItem.Text & "'"
            End If
        End If
        If ddlGetriebe.SelectedItem.Value <> "-1" Then
            If sFilter <> "" Then
                sFilter = sFilter & " AND Antriebsart='" & ddlGetriebe.SelectedItem.Text & "'"
            Else
                sFilter = "Antriebsart='" & ddlGetriebe.SelectedItem.Text & "'"
            End If
        End If
        If ddlKraftstoff.SelectedItem.Value <> "-1" Then
            If sFilter <> "" Then
                sFilter = sFilter & " AND Kraftstoffart='" & ddlKraftstoff.SelectedItem.Text & "'"
            Else
                sFilter = "Kraftstoffart='" & ddlKraftstoff.SelectedItem.Text & "'"
            End If
        End If
        If ddlNavi.SelectedItem.Value <> "-1" Then
            If sFilter <> "" Then
                sFilter = sFilter & " AND Navigation='" & Left(ddlNavi.SelectedItem.Text, 1) & "'"
            Else
                sFilter = "Navigation='" & Left(ddlNavi.SelectedItem.Text, 1) & "'"
            End If
        End If

        If ddlFarbe.SelectedItem.Value <> "-1" Then
            If sFilter <> "" Then
                sFilter = sFilter & " AND Farbe='" & ddlFarbe.SelectedItem.Text & "'"
            Else
                sFilter = "Farbe='" & ddlFarbe.SelectedItem.Text & "'"
            End If
        End If

        If ddlVermiet.SelectedItem.Value <> "-1" Then
            If sFilter <> "" Then
                sFilter = sFilter & " AND Vermietgruppe='" & ddlVermiet.SelectedItem.Text & "'"
            Else
                sFilter = "Vermietgruppe='" & ddlVermiet.SelectedItem.Text & "'"
            End If
        End If

        If ddlFzgArt.SelectedItem.Value <> "-1" Then
            If sFilter <> "" Then
                sFilter = sFilter & " AND Fahrzeugart='" & ddlFzgArt.SelectedItem.Text & "'"
            Else
                sFilter = "Fahrzeugart='" & ddlFzgArt.SelectedItem.Text & "'"
            End If
        End If

        If ddlAufbauArt.SelectedItem.Value <> "-1" Then
            If sFilter <> "" Then
                sFilter = sFilter & " AND Aufbauart='" & ddlAufbauArt.SelectedItem.Text & "'"
            Else
                sFilter = "Aufbauart='" & ddlAufbauArt.SelectedItem.Text & "'"
            End If
        End If

        If ddlHaendlernr.SelectedItem.Value <> "-1" Then
            If sFilter <> "" Then
                sFilter = sFilter & " AND HaendlerId='" & ddlHaendlernr.SelectedItem.Text & "'"
            Else
                sFilter = "HaendlerId='" & ddlHaendlernr.SelectedItem.Text & "'"
            End If
        End If

        If ddlHaendlername.SelectedItem.Value <> "-1" Then
            If sFilter <> "" Then
                sFilter = sFilter & " AND Haendler_Kurzname='" & ddlHaendlername.SelectedItem.Text & "'"
            Else
                sFilter = "Haendler_Kurzname='" & ddlHaendlername.SelectedItem.Text & "'"
            End If
        End If
        If ddlEKIndikator.SelectedItem.Value <> "-1" Then
            If sFilter <> "" Then
                sFilter = sFilter & " AND Einkaufsindikator='" & ddlEKIndikator.SelectedItem.Text & "'"
            Else
                sFilter = "Einkaufsindikator='" & ddlEKIndikator.SelectedItem.Text & "'"
            End If
        End If

        If ddlVerwZweck.SelectedItem.Value <> "-1" Then
            If sFilter <> "" Then
                sFilter = sFilter & " AND Verwendungszweck='" & ddlVerwZweck.SelectedItem.Text & "'"
            Else
                sFilter = "Verwendungszweck='" & ddlVerwZweck.SelectedItem.Text & "'"
            End If
        End If

        If ddlOwnerCode.SelectedItem.Value <> "-1" Then
            If sFilter <> "" Then
                sFilter = sFilter & " AND Owner_Code='" & ddlOwnerCode.SelectedItem.Text & "'"
            Else
                sFilter = "Owner_Code='" & ddlOwnerCode.SelectedItem.Text & "'"
            End If
        End If
        If ddlSperrdat.SelectedItem.Value <> "-1" Then
            If sFilter <> "" Then
                sFilter = sFilter & " AND Datum_zur_Sperre='" & ddlSperrdat.SelectedItem.Text & "'"
            Else
                sFilter = "Datum_zur_Sperre='" & ddlSperrdat.SelectedItem.Text & "'"
            End If
        End If
        Return sFilter
    End Function

    Protected Sub ddlNavi_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlNavi.SelectedIndexChanged
        Setfilter()
    End Sub

    Protected Sub ddlFarbe_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlFarbe.SelectedIndexChanged
        Setfilter()
    End Sub

    Protected Sub ddlVermiet_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlVermiet.SelectedIndexChanged
        Setfilter()
    End Sub

    Protected Sub ddlFzgArt_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlFzgArt.SelectedIndexChanged
        Setfilter()
    End Sub

    Protected Sub ddlAufbauArt_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlAufbauArt.SelectedIndexChanged
        Setfilter()
    End Sub

    Protected Sub ddlHaendlernr_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlHaendlernr.SelectedIndexChanged
        Setfilter()
    End Sub

    Protected Sub ddlHaendlername_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlHaendlername.SelectedIndexChanged
        Setfilter()
    End Sub

    Protected Sub ddlEKIndikator_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlEKIndikator.SelectedIndexChanged
        Setfilter()
    End Sub

    Protected Sub ddlVerwZweck_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlVerwZweck.SelectedIndexChanged
        Setfilter()
    End Sub

    Protected Sub ddlOwnerCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOwnerCode.SelectedIndexChanged
        Setfilter()
    End Sub

    Protected Sub ddlSperrdat_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlSperrdat.SelectedIndexChanged
        Setfilter()
    End Sub

End Class
' ************************************************
' $History: Change02.aspx.vb $
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 10.09.09   Time: 13:15
' Updated in $/CKAG/Applications/AppAvis/forms
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 30.12.08   Time: 15:50
' Updated in $/CKAG/Applications/AppAvis/forms
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 1.12.08    Time: 16:13
' Updated in $/CKAG/Applications/AppAvis/forms
' ITA: 2359 & kleinere Anpassungen
' 
' ************************************************
