Option Explicit On
Option Strict On

Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business
Imports System.Drawing

Partial Public Class Change04_2
    Inherits System.Web.UI.Page

    Protected WithEvents ucStyles As Global.CKG.Portal.PageElements.Styles
    Protected WithEvents ucHeader As Global.CKG.Portal.PageElements.Header

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App

    Private alControls As New ArrayList()

    Private tmpControl As New Control()
    Private tmpTxt As New TextBox()
    Private tmpCb As New CheckBox()
    Private tmpDTG As New DataGrid()
    Private tmpLBX As New ListBox()
    Private tmpDDL As New DropDownList()
    Private mError As Boolean

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        m_User = GetUser(Me)
        m_App = New Base.Kernel.Security.App(m_User) 'erzeugt ein App_objekt 

        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")
        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text

        lblError.Text = ""

        If Not IsPostBack Then

            FillControls()

            lb_Verwerfen.Attributes.Add("onclick", "return confirm('Ihre Änderungen sind noch nicht gespeichert, möchten Sie trotzdem fortfahren?');")

        End If

        checkVisibilyOfElements()

    End Sub

    Private Sub checkVisibilyOfElements()
        If Not ddl_VersDeckArt.SelectedIndex = 0 Then
            'wenn irgendwas ausgewählt wurde, dann Feld Dauer-EVB einblenden
            td_dauerEVB1.Visible = True
            td_dauerEVB2.Visible = True
        Else
            td_dauerEVB1.Visible = False
            td_dauerEVB2.Visible = False
            txt_DauerEVB.Text = ""
        End If

    End Sub

    Private Sub fillArrayListWithControls(ByVal mothercontrol As Control)
        'alle Contorls(txt,cb,DV, LBX) auf der seite werden in eine ArrayListe geschrieben, 
        'Dies muss jedes mal erneut passieren, weil komplette Objekte in der AL gesichert sind und keine Referenzen, 
        'so kann die AL auch nicht in einer Session abgelegt werden, weil sonst der Wert von den Objekt bezogen wird,
        ' wie Anfangs in der SessionArraylist hinzugefügt wurden JJ2007.11.15

        For Each tmpControl In mothercontrol.Controls
            'wenn das dataGrid eine zeile enthält würde es nicht in das ControlArray gesichert werden, dadurch könnte es auch nicht bei der Erfassung berücksichtigt werden. JJ2007.11.26
            If tmpControl.HasControls = True And tmpControl.GetType.Equals(tmpDTG.GetType) = False Then
                fillArrayListWithControls(tmpControl)
            Else
                If tmpControl.GetType.Equals(tmpCb.GetType()) = True OrElse tmpControl.GetType.Equals(tmpTxt.GetType()) = True OrElse tmpControl.GetType.Equals(tmpDTG.GetType) = True OrElse tmpControl.GetType.Equals(tmpLBX.GetType) = True OrElse tmpControl.GetType.Equals(tmpDDL.GetType) Then
                    alControls.Add(tmpControl)
                End If

            End If
        Next
    End Sub

    Private Sub FillControls()
        Dim tblStammdaten As New DataTable()
        Dim tblAnsprechpartner As New DataTable()
        Dim tblVersandbedingungen As New DataTable()

        Dim objChange04 As New Kernel.Change_04(m_User, m_App, "")
        Dim tblTeamResult As DataTable = objChange04.FillTeam(Session("AppID").ToString, Session.SessionID)

        ddl_Team.DataSource = tblTeamResult
        ddl_Team.DataValueField = "Team"
        ddl_Team.DataTextField = "Name"
        ddl_Team.DataBind()

        If Session("Stammdaten") IsNot Nothing Then

            tblStammdaten = DirectCast(Session("Stammdaten"), DataTable)
            tblAnsprechpartner = DirectCast(Session("Ansprechpartner"), DataTable)
            tblVersandbedingungen = DirectCast(Session("Versandbedingungen"), DataTable)

            If tblStammdaten.Rows.Count > 0 Then
                Dim stdaRow As DataRow = tblStammdaten.Rows(0)

                'Team
                ddl_Team.SelectedValue = stdaRow("TEAM").ToString()

                'Leasingnehmer
                txt_D_LN_KUNNR.Text = stdaRow("EXKUNNR_ZL").ToString()
                txt_D_LN_NAME1.Text = stdaRow("NAME1_ZL").ToString()
                txt_D_LN_STRAS.Text = stdaRow("STRAS_ZL").ToString()
                txt_D_LN_PSTLZ.Text = stdaRow("PSTLZ_ZL").ToString()
                txt_D_LN_ORT01.Text = stdaRow("ORT01_ZL").ToString()

                'Sonstiges
                rbl_SVCKART.SelectedValue = stdaRow("SVCKART").ToString()
                rbl_SV_NAVICD.SelectedValue = stdaRow("SV_NAVICD").ToString()
                rbl_SV_HANDY_ADAPTER.SelectedValue = stdaRow("SV_HANDY_ADAPTER").ToString()
                rbl_SV_Verbandskasten.SelectedValue = stdaRow("ZVERBANDSKASTEN").ToString()
                rbl_SV_Warndreieck.SelectedValue = stdaRow("ZWARNDREIECK").ToString()
                rbl_SV_Warnweste.SelectedValue = stdaRow("ZWARNWESTE").ToString()
                rbl_SV_Fussmatten.SelectedValue = stdaRow("ZFUSSMATTEN").ToString()
                rbl_SV_FScheinkontrolle.SelectedValue = stdaRow("ZKONTROLLEFS").ToString()

                'Vorgaben Zulassung
                ddl_SH_KUNDART.SelectedValue = stdaRow("ZUL_KUNDART").ToString()
                txt_SH_ZKFZKZ.Text = stdaRow("ZKFZKZ").ToString()
                txt_SH_Firma.Text = stdaRow("NAME1_SH").ToString()
                txt_SH_Strasse.Text = stdaRow("STRAS_SH").ToString()
                txt_SH_PLZ.Text = stdaRow("PSTLZ_SH").ToString()
                txt_SH_ORT.Text = stdaRow("ORT01_SH").ToString()
                txt_SH_DWK.Text = stdaRow("DW_KENNZ").ToString()
                ddl_ZUL_DURCH.SelectedValue = stdaRow("ZUL_DURCH").ToString()
                ddl_KFZ_Steuer.SelectedValue = stdaRow("KFZ_STEUERZAHLER").ToString()

                'Vericherungsdaten
                ddl_Versichert.SelectedValue = stdaRow("VERSICHERT").ToString()
                txt_Versicherung.Text = stdaRow("ZZVERSICHERUNG").ToString()
                ddl_VersDeckArt.SelectedValue = stdaRow("VERSDECKART").ToString()
                txt_DauerEVB.Text = stdaRow("DAUER_EVB").ToString()

                'Auslieferung
                txt_AL_Name1.Text = stdaRow("NAME1_AL").ToString()
                txt_AL_Ansprechpartner.Text = stdaRow("ANSPP_AL").ToString()
                txt_AL_Strasse.Text = stdaRow("STRAS_AL").ToString()
                txt_AL_PLZ.Text = stdaRow("PSTLZ_AL").ToString()
                txt_AL_Ort.Text = stdaRow("ORT01_AL").ToString()
                txt_AL_Telefon.Text = stdaRow("TELF1_AL").ToString()
                txt_AL_Fax.Text = stdaRow("FAX1_AL").ToString()
                txt_AL_EMAIL.Text = stdaRow("EMAIL_AL").ToString()
                txt_AL_AUSLINF.Text = stdaRow("AUSLINF_AL").ToString()

                'Fahrzeugrücknahme
                txt_RL_NAME1.Text = stdaRow("NAME1_RL").ToString()
                txt_RL_ANSPRECHP.Text = stdaRow("ANSPP_RL").ToString()
                txt_RL_Strasse.Text = stdaRow("STRAS_RL").ToString()
                txt_RL_PLZ.Text = stdaRow("PSTLZ_RL").ToString()
                txt_RL_Ort.Text = stdaRow("ORT01_RL").ToString()
                txt_RL_Telefon.Text = stdaRow("TELF1_RL").ToString()
                txt_RL_Fax.Text = stdaRow("FAX1_RL").ToString()
                txt_RL_EMAIL.Text = stdaRow("EMAIL_RL").ToString()

                'Sonstiges
                txt_SONDAG.Text = stdaRow("SONDAG").ToString()
                txt_Tankkarte.Text = stdaRow("TANKKARTE").ToString()
                txt_Tankkarte_BSH.Text = stdaRow("TANKKARTE_BSH").ToString()
            End If
        Else
            txt_D_LN_KUNNR.Text = Session("LNKUNNR").ToString
            Session("LNKUNNR") = Nothing
            Session.Add("Stammdaten", tblStammdaten)
        End If

        'Ansprechpartner mit 5 Zeilen anlegen
        If tblAnsprechpartner.Columns.Count = 0 Then
            InitTableAnsprechpartner(tblAnsprechpartner)
        End If
        If tblAnsprechpartner.Rows.Count < 5 Then
            Dim differenz As Int32 = 5 - tblAnsprechpartner.Rows.Count

            For i As Int32 = 1 To differenz
                Dim newAnsppRow As DataRow = tblAnsprechpartner.NewRow()
                newAnsppRow("MANDT") = "010"
                newAnsppRow("EXKUNNR_ZL") = txt_D_LN_KUNNR.Text
                newAnsppRow("AG") = Right("00000000000" & m_User.KUNNR, 10)
                tblAnsprechpartner.Rows.Add(newAnsppRow)
            Next
        End If

        grdAnsprechpartner.DataSource = tblAnsprechpartner
        grdAnsprechpartner.DataBind()

        'Versandbedingungen mit 5 Zeilen anlegen
        If tblVersandbedingungen.Columns.Count = 0 Then
            InitTableVersandbedingungen(tblVersandbedingungen)
        End If
        If tblVersandbedingungen.Rows.Count > 0 Then
            For e As Int32 = 0 To tblVersandbedingungen.Rows.Count - 1

                Dim versRow As DataRow = tblVersandbedingungen.Rows(e)

                If versRow("UEBNAHMBEST").ToString() = "X" Then
                    versRow("UEBNAHMBEST") = "true"
                Else
                    versRow("UEBNAHMBEST") = "false"
                End If

                If versRow("ZB2_VERSENDEN").ToString() = "X" Then
                    versRow("ZB2_VERSENDEN") = "true"
                Else
                    versRow("ZB2_VERSENDEN") = "false"
                End If

                If versRow("RUECKNAHMEPROTK").ToString() = "X" Then
                    versRow("RUECKNAHMEPROTK") = "true"
                Else
                    versRow("RUECKNAHMEPROTK") = "false"
                End If

                If versRow("ZB1_VERSENDEN").ToString() = "X" Then
                    versRow("ZB1_VERSENDEN") = "true"
                Else
                    versRow("ZB1_VERSENDEN") = "false"
                End If

            Next
        End If

        If tblVersandbedingungen.Rows.Count < 5 Then
            Dim differenz As Int32 = 5 - tblVersandbedingungen.Rows.Count

            For i As Int32 = 1 To differenz
                Dim newVersRow As DataRow = tblVersandbedingungen.NewRow()
                newVersRow("MANDT") = "010"
                newVersRow("EXKUNNR_ZL") = txt_D_LN_KUNNR.Text
                newVersRow("AG") = Right("00000000000" & m_User.KUNNR, 10)
                tblVersandbedingungen.Rows.Add(newVersRow)
            Next
        End If

        grdVersandbedingungen.DataSource = tblVersandbedingungen
        grdVersandbedingungen.DataBind()

    End Sub

    Private Sub Save()
        Try
            Dim objChange04 As New Kernel.Change_04(m_User, m_App, "")
            Dim tblAnsprechpartner As New DataTable()
            Dim tblVersandbedingungen As New DataTable()
            Dim tblStammdaten As DataTable = DirectCast(Session("Stammdaten"), DataTable)

            InitTableAnsprechpartner(tblAnsprechpartner)
            InitTableVersandbedingungen(tblVersandbedingungen)
            If tblStammdaten.Columns.Count = 0 Then
                InitTableStammdaten(tblStammdaten)
            End If
            If tblStammdaten.Rows.Count = 0 Then
                tblStammdaten.Rows.Add(tblStammdaten.NewRow())
            End If

            Dim stdaRow As DataRow = tblStammdaten.Rows(0)

            'Allgemein
            stdaRow("MANDT") = "010"
            stdaRow("AG") = Right("00000000000" & m_User.KUNNR, 10)

            'Team
            stdaRow("TEAM") = ddl_Team.SelectedItem.Value

            'Leasingnehmer		
            stdaRow("EXKUNNR_ZL") = txt_D_LN_KUNNR.Text
            stdaRow("NAME1_ZL") = txt_D_LN_NAME1.Text
            stdaRow("STRAS_ZL") = txt_D_LN_STRAS.Text
            stdaRow("PSTLZ_ZL") = txt_D_LN_PSTLZ.Text
            stdaRow("ORT01_ZL") = txt_D_LN_ORT01.Text

            'Sonstiges
            If rbl_SVCKART.SelectedItem.Value = "X" Then
                stdaRow("SVCKART") = "X"
            Else
                stdaRow("SVCKART") = String.Empty
            End If

            If rbl_SV_NAVICD.SelectedItem.Value = "X" Then
                stdaRow("SV_NAVICD") = "X"
            Else
                stdaRow("SV_NAVICD") = String.Empty
            End If

            If rbl_SV_HANDY_ADAPTER.SelectedItem.Value = "X" Then
                stdaRow("SV_HANDY_ADAPTER") = "X"
            Else
                stdaRow("SV_HANDY_ADAPTER") = String.Empty
            End If

            If rbl_SV_Verbandskasten.SelectedItem.Value = "X" Then
                stdaRow("ZVERBANDSKASTEN") = "X"
            Else
                stdaRow("ZVERBANDSKASTEN") = String.Empty
            End If

            If rbl_SV_Warndreieck.SelectedItem.Value = "X" Then
                stdaRow("ZWARNDREIECK") = "X"
            Else
                stdaRow("ZWARNDREIECK") = String.Empty
            End If

            If rbl_SV_Warnweste.SelectedItem.Value = "X" Then
                stdaRow("ZWARNWESTE") = "X"
            Else
                stdaRow("ZWARNWESTE") = String.Empty
            End If

            If rbl_SV_Fussmatten.SelectedItem.Value = "X" Then
                stdaRow("ZFUSSMATTEN") = "X"
            Else
                stdaRow("ZFUSSMATTEN") = String.Empty
            End If

            If rbl_SV_FScheinkontrolle.SelectedItem.Value = "X" Then
                stdaRow("ZKONTROLLEFS") = "X"
            Else
                stdaRow("ZKONTROLLEFS") = String.Empty
            End If

            'Vorgaben Zulassung	
            If ddl_SH_KUNDART.SelectedItem.Value <> "Auswahl" Then
                stdaRow("ZUL_KUNDART") = ddl_SH_KUNDART.SelectedItem.Value
            Else
                stdaRow("ZUL_KUNDART") = String.Empty
            End If

            stdaRow("ZKFZKZ") = txt_SH_ZKFZKZ.Text
            stdaRow("NAME1_SH") = txt_SH_Firma.Text
            stdaRow("STRAS_SH") = txt_SH_Strasse.Text
            stdaRow("PSTLZ_SH") = txt_SH_PLZ.Text
            stdaRow("ORT01_SH") = txt_SH_ORT.Text
            stdaRow("DW_KENNZ") = txt_SH_DWK.Text

            If ddl_ZUL_DURCH.SelectedItem.Value <> "Auswahl" Then
                stdaRow("ZUL_DURCH") = ddl_ZUL_DURCH.SelectedItem.Value
            Else
                stdaRow("ZUL_DURCH") = String.Empty
            End If

            If ddl_KFZ_Steuer.SelectedItem.Value <> "Auswahl" Then
                stdaRow("KFZ_STEUERZAHLER") = ddl_KFZ_Steuer.SelectedItem.Value
            Else
                stdaRow("KFZ_STEUERZAHLER") = String.Empty
            End If


            'Vericherungsdaten		
            If ddl_Versichert.SelectedItem.Value <> "Auswahl" Then
                stdaRow("VERSICHERT") = ddl_Versichert.SelectedItem.Value
            Else
                stdaRow("VERSICHERT") = String.Empty
            End If

            stdaRow("ZZVERSICHERUNG") = txt_Versicherung.Text

            If ddl_VersDeckArt.SelectedItem.Value <> "Auswahl" Then
                stdaRow("VERSDECKART") = ddl_VersDeckArt.SelectedItem.Value
            Else
                stdaRow("VERSDECKART") = String.Empty
            End If
            stdaRow("DAUER_EVB") = txt_DauerEVB.Text

            'Auslieferung		
            stdaRow("NAME1_AL") = txt_AL_Name1.Text
            stdaRow("ANSPP_AL") = txt_AL_Ansprechpartner.Text
            stdaRow("STRAS_AL") = txt_AL_Strasse.Text
            stdaRow("PSTLZ_AL") = txt_AL_PLZ.Text
            stdaRow("ORT01_AL") = txt_AL_Ort.Text
            stdaRow("TELF1_AL") = txt_AL_Telefon.Text
            stdaRow("FAX1_AL") = txt_AL_Fax.Text
            stdaRow("EMAIL_AL") = txt_AL_EMAIL.Text
            stdaRow("AUSLINF_AL") = txt_AL_AUSLINF.Text

            'Fahrzeugrücknahme		
            stdaRow("NAME1_RL") = txt_RL_NAME1.Text
            stdaRow("ANSPP_RL") = txt_RL_ANSPRECHP.Text
            stdaRow("STRAS_RL") = txt_RL_Strasse.Text
            stdaRow("PSTLZ_RL") = txt_RL_PLZ.Text
            stdaRow("ORT01_RL") = txt_RL_Ort.Text
            stdaRow("TELF1_RL") = txt_RL_Telefon.Text
            stdaRow("FAX1_RL") = txt_RL_Fax.Text
            stdaRow("EMAIL_RL") = txt_RL_EMAIL.Text

            'Sonstiges		
            stdaRow("SONDAG") = txt_SONDAG.Text
            stdaRow("TANKKARTE") = txt_Tankkarte.Text
            stdaRow("TANKKARTE_BSH") = txt_Tankkarte_BSH.Text

            Dim booErr As Boolean = False
            Dim grdRow As GridViewRow
            Dim cell As TableCell
            Dim ctrl As Control
            Dim chkBox As CheckBox
            Dim txtBox As TextBox
            Dim booFound As Boolean

            With grdAnsprechpartner

                For Each grdRow In .Rows
                    booFound = False
                    Dim newAnsppRow As DataRow = tblAnsprechpartner.NewRow()

                    For Each ctrl In grdRow.Controls

                        chkBox = CType(ctrl.FindControl("chkDelete"), CheckBox)

                        If Not chkBox.Checked Then
                            txtBox = CType(ctrl.FindControl("txtName"), TextBox)
                            If Not String.IsNullOrEmpty(txtBox.Text) Then

                                newAnsppRow("MANDT") = "010"
                                newAnsppRow("EXKUNNR_ZL") = txt_D_LN_KUNNR.Text
                                newAnsppRow("AG") = Right("00000000000" & m_User.KUNNR, 10)
                                newAnsppRow("NAME1") = txtBox.Text

                                txtBox = CType(ctrl.FindControl("txtTelefon"), TextBox)
                                newAnsppRow("TELF1") = txtBox.Text

                                txtBox = CType(ctrl.FindControl("txtEMAIL"), TextBox)
                                newAnsppRow("EMAIL") = txtBox.Text

                                booFound = True
                            End If
                        End If
                    Next

                    If booFound Then
                        tblAnsprechpartner.Rows.Add(newAnsppRow)
                    End If
                Next

            End With

            With grdVersandbedingungen

                For Each grdRow In .Rows
                    booFound = False
                    Dim newVersRow As DataRow = tblVersandbedingungen.NewRow()

                    For Each ctrl In grdRow.Controls

                        chkBox = CType(ctrl.FindControl("chkDelete"), CheckBox)

                        If Not chkBox.Checked Then
                            txtBox = CType(ctrl.FindControl("txtEMAIL"), TextBox)
                            If Not String.IsNullOrEmpty(txtBox.Text) Then

                                newVersRow("MANDT") = "010"
                                newVersRow("EXKUNNR_ZL") = txt_D_LN_KUNNR.Text
                                newVersRow("AG") = Right("00000000000" & m_User.KUNNR, 10)
                                newVersRow("EMAIL") = txtBox.Text

                                txtBox = CType(ctrl.FindControl("txtFAdressinhaber"), TextBox)
                                newVersRow("ZPOSITION") = txtBox.Text

                                chkBox = CType(ctrl.FindControl("chkUebernahmebest"), CheckBox)

                                If chkBox.Checked = True Then
                                    newVersRow("UEBNAHMBEST") = "X"
                                End If

                                chkBox = CType(ctrl.FindControl("chkZB2"), CheckBox)

                                If chkBox.Checked = True Then
                                    newVersRow("ZB2_VERSENDEN") = "X"
                                End If

                                chkBox = CType(ctrl.FindControl("chkRuecknahmeprot"), CheckBox)

                                If chkBox.Checked = True Then
                                    newVersRow("RUECKNAHMEPROTK") = "X"
                                End If

                                chkBox = CType(ctrl.FindControl("chkZB1"), CheckBox)

                                If chkBox.Checked = True Then
                                    newVersRow("ZB1_VERSENDEN") = "X"
                                End If

                                booFound = True
                            End If
                        End If
                    Next

                    If booFound Then
                        tblVersandbedingungen.Rows.Add(newVersRow)
                    End If
                Next

            End With

            If Trim(txt_D_LN_KUNNR.Text) = String.Empty Then
                mError = True
                txt_D_LN_KUNNR.BorderColor = Color.Red
            End If

            If Trim(txt_D_LN_NAME1.Text) = String.Empty Then
                mError = True
                txt_D_LN_NAME1.BorderColor = Color.Red
            End If

            If Trim(txt_D_LN_STRAS.Text) = String.Empty Then
                mError = True
                txt_D_LN_STRAS.BorderColor = Color.Red
            End If

            If Trim(txt_D_LN_PSTLZ.Text) = String.Empty Then
                mError = True
                txt_D_LN_PSTLZ.BorderColor = Color.Red
            End If

            If Trim(txt_D_LN_ORT01.Text) = String.Empty Then
                mError = True
                txt_D_LN_ORT01.BorderColor = Color.Red
            End If

            If mError = True Then
                lblError.Text = "Bitte füllen Sie alle Pflichtfelder(*) aus"
                Exit Sub
            Else

                'Daten speichern
                Try
                    objChange04.SaveLNKundendaten(Session("AppID").ToString, Session.SessionID, tblStammdaten, tblAnsprechpartner, tblVersandbedingungen)
                Catch saveEx As Exception
                    mError = True
                    If Base.Business.HelpProcedures.CastSapBizTalkErrorMessage(saveEx.Message) = "NO_DATA" Then
                        lblError.Text = "Kundennummer noch nicht vorhanden."
                    Else
                        mError = True
                        lblError.Text = "Fehler beim Speichern der Daten (" & saveEx.Message & ")."
                    End If
                End Try

                If Not objChange04.Status = 0 Then
                    mError = True
                    lblError.Text = "Fehler beim Speichern der Daten (" & objChange04.Message & ")."
                End If

                Session.Add("Stammdaten", tblStammdaten)
            End If
        Catch ex As Exception
            mError = True
            lblError.Text = "Fehler beim Speichern der Daten (" & ex.Message & ")."
        End Try
    End Sub

    Private Sub InitTableStammdaten(ByRef tbl As DataTable)
        tbl.Columns.Add("MANDT", GetType(String))
        tbl.Columns.Add("AG", GetType(String))
        tbl.Columns.Add("LFDNR", GetType(Int32))
        tbl.Columns.Add("EXKUNNR_ZL", GetType(String))
        tbl.Columns.Add("KUNNR_ZL", GetType(String))
        tbl.Columns.Add("NAME1_ZL", GetType(String))
        tbl.Columns.Add("STRAS_ZL", GetType(String))
        tbl.Columns.Add("PSTLZ_ZL", GetType(String))
        tbl.Columns.Add("ORT01_ZL", GetType(String))
        tbl.Columns.Add("LAND1_ZL", GetType(String))
        tbl.Columns.Add("SV_NAVICD", GetType(String))
        tbl.Columns.Add("SV_HANDY_ADAPTER", GetType(String))
        tbl.Columns.Add("ZUL_KUNDART", GetType(String))
        tbl.Columns.Add("ZKFZKZ", GetType(String))
        tbl.Columns.Add("NAME1_SH", GetType(String))
        tbl.Columns.Add("STRAS_SH", GetType(String))
        tbl.Columns.Add("PSTLZ_SH", GetType(String))
        tbl.Columns.Add("ORT01_SH", GetType(String))
        tbl.Columns.Add("LAND1_SH", GetType(String))
        tbl.Columns.Add("DW_KENNZ", GetType(String))
        tbl.Columns.Add("ZUL_DURCH", GetType(String))
        tbl.Columns.Add("KFZ_STEUERZAHLER", GetType(String))
        tbl.Columns.Add("VERSICHERT", GetType(String))
        tbl.Columns.Add("ZZVERSICHERUNG", GetType(String))
        tbl.Columns.Add("VERSDECKART", GetType(String))
        tbl.Columns.Add("DAUER_EVB", GetType(String))
        tbl.Columns.Add("NAME1_AL", GetType(String))
        tbl.Columns.Add("ANSPP_AL", GetType(String))
        tbl.Columns.Add("STRAS_AL", GetType(String))
        tbl.Columns.Add("PSTLZ_AL", GetType(String))
        tbl.Columns.Add("ORT01_AL", GetType(String))
        tbl.Columns.Add("LAND1_AL", GetType(String))
        tbl.Columns.Add("TELF1_AL", GetType(String))
        tbl.Columns.Add("FAX1_AL", GetType(String))
        tbl.Columns.Add("EMAIL_AL", GetType(String))
        tbl.Columns.Add("AUSLINF_AL", GetType(String))
        tbl.Columns.Add("NAME1_RL", GetType(String))
        tbl.Columns.Add("ANSPP_RL", GetType(String))
        tbl.Columns.Add("STRAS_RL", GetType(String))
        tbl.Columns.Add("PSTLZ_RL", GetType(String))
        tbl.Columns.Add("ORT01_RL", GetType(String))
        tbl.Columns.Add("LAND1_RL", GetType(String))
        tbl.Columns.Add("TELF1_RL", GetType(String))
        tbl.Columns.Add("FAX1_RL", GetType(String))
        tbl.Columns.Add("EMAIL_RL", GetType(String))
        tbl.Columns.Add("SONDAG", GetType(String))
        tbl.Columns.Add("SVCKART", GetType(String))
        tbl.Columns.Add("TANKKARTE", GetType(String))
        tbl.Columns.Add("TANKKARTE_BSH", GetType(String))
        tbl.Columns.Add("TEAM", GetType(String))
        tbl.Columns.Add("AENAM", GetType(String))
        tbl.Columns.Add("AEDAT", GetType(DateTime))
        tbl.Columns.Add("LOEVM", GetType(String))
        tbl.Columns.Add("ZHANDYADAPTER", GetType(String))
        tbl.Columns.Add("ZVERBANDSKASTEN", GetType(String))
        tbl.Columns.Add("ZNAVICD", GetType(String))
        tbl.Columns.Add("ZWARNDREIECK", GetType(String))
        tbl.Columns.Add("ZWARNWESTE", GetType(String))
        tbl.Columns.Add("ZFUSSMATTEN", GetType(String))
        tbl.Columns.Add("ZKONTROLLEFS", GetType(String))
    End Sub

    Private Sub InitTableAnsprechpartner(ByRef tbl As DataTable)
        tbl.Columns.Add("MANDT", GetType(String))
        tbl.Columns.Add("AG", GetType(String))
        tbl.Columns.Add("EXKUNNR_ZL", GetType(String))
        tbl.Columns.Add("NAME1", GetType(String))
        tbl.Columns.Add("ZPOSITION", GetType(String))
        tbl.Columns.Add("TELF1", GetType(String))
        tbl.Columns.Add("FAX1", GetType(String))
        tbl.Columns.Add("EMAIL", GetType(String))
        tbl.Columns.Add("BESCHREIBUNG", GetType(String))
    End Sub

    Private Sub InitTableVersandbedingungen(ByRef tbl As DataTable)
        tbl.Columns.Add("MANDT", GetType(String))
        tbl.Columns.Add("AG", GetType(String))
        tbl.Columns.Add("EXKUNNR_ZL", GetType(String))
        tbl.Columns.Add("EMAIL", GetType(String))
        tbl.Columns.Add("ZPOSITION", GetType(String))
        tbl.Columns.Add("UEBNAHMBEST", GetType(String))
        tbl.Columns.Add("ZB2_VERSENDEN", GetType(String))
        tbl.Columns.Add("RUECKNAHMEPROTK", GetType(String))
        tbl.Columns.Add("ZB1_VERSENDEN", GetType(String))
        tbl.Columns.Add("HALTER", GetType(String))
    End Sub

    Protected Sub lb_Speichern_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_Speichern.Click
        Save()
        If mError = False Then
            Response.Redirect("Change04.aspx?AppID=" & Session("AppID").ToString)
        Else
            mError = False
        End If
    End Sub

    Protected Sub lb_Verwerfen_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_Verwerfen.Click
        Response.Redirect("Change04.aspx?AppID=" & Session("AppID").ToString)
    End Sub
End Class