Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common

Public Class Change02
    Inherits System.Web.UI.Page

#Region "Declarations"
    Private m_User As Security.User
    Private m_App As Security.App
    Private TableAuftrag As DataTable
#End Region

#Region "Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User, True)
        GetAppIDFromQueryString(Me)
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Security.App(m_User)

            ResetBorderColor()

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Protected Sub lbtBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtBack.Click
        If grvAusgabe.Visible = False Then
            TableForm.Visible = False
            lbtSave.Visible = False
            DoSubmit()
        Else
            Response.Redirect("../../../Start/selection.aspx")
        End If


    End Sub

    Protected Sub lbtSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtSave.Click
        If ValidateControls() = True Then
            lblError.Text = "Bitte füllen Sie die rot umrahmten Felder korrekt aus."
        Else
            Save()
        End If
    End Sub

    Protected Sub lbtGetData_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtGetData.Click
        DoSubmit()
    End Sub

    Private Sub grvAusgabe_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grvAusgabe.PageIndexChanging
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub grvAusgabe_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles grvAusgabe.RowEditing

    End Sub

    Private Sub grvAusgabe_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grvAusgabe.Sorting
        FillGrid(0, e.SortExpression)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

#End Region

#Region "Methods"

    Private Sub DoSubmit()

        Dim GetData As New RuecknahmeQuittung()

        TableAuftrag = GetData.Fill(m_User, m_App, Session("AppID").ToString, m_User.Reference, "1510")

        If TableAuftrag.Rows.Count > 0 Then
            FillGrid(0, "", True)
            Session("TableAuftrag") = TableAuftrag
            lbtGetData.Visible = False
            grvAusgabe.Visible = True
        Else
            lblInfo.Text = "Es wurden keine offenen Aufträge gefunden."
        End If

    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "", Optional ByVal NoSort As Boolean = False)
        Dim tmpDataView As DataView = TableAuftrag.DefaultView

        tmpDataView.RowFilter = ""

        If tmpDataView.Count = 0 Then

        Else
            
            Dim strTempSort As String = ""
            Dim strDirection As String = ""
            Dim intTempPageIndex As Int32 = intPageIndex

            If strSort.Trim(" "c).Length > 0 Then
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
                If NoSort = False Then
                    tmpDataView.Sort = strTempSort & " " & strDirection
                End If

            End If
            grvAusgabe.PageIndex = intTempPageIndex
            grvAusgabe.DataSource = tmpDataView
            grvAusgabe.DataBind()

        End If

    End Sub

    Protected Sub ibtVbeln_Click(ByVal sender As Object, ByVal e As EventArgs)

        Dim gvRow As GridViewRow = CType(CType(sender, Control).Parent.Parent, GridViewRow)

        Dim index As Integer = gvRow.RowIndex

        Dim row As GridViewRow = grvAusgabe.Rows(index)

        Dim TempTable As DataTable
        Dim Vbeln As String
        Dim lbl As Label

        TempTable = CType(Session("TableAuftrag"), DataTable)

        lbl = row.Cells(1).FindControl("lblAuftrag")

        Vbeln = lbl.Text


        Dim TempTableRow As DataRow() = TempTable.Select("VBELN = '" & Vbeln & "'")

        grvAusgabe.Visible = False
        TableForm.Visible = True
        lbtSave.Visible = True

        lblAuftrag.Text = TempTableRow(0)("VBELN")
        txtKennzeichen.Text = TempTableRow(0)("KENNZEICHEN")
        txtFahrgestellnummer.Text = TempTableRow(0)("FAHRG_NR")
        PrepareForm(CStr(TempTableRow(0)("VOLLERFASS")))
    End Sub

    Private Sub PrepareForm(ByVal Flag As String)
        If Flag.ToUpper() = "X" Then
            rblUnfall.SelectedValue = Nothing
            rblTechMaengel.SelectedValue = Nothing
            rblAustauschMotor.SelectedValue = Nothing
            rblAustauschGetriebe.SelectedValue = Nothing
            rblTacho.SelectedValue = Nothing
            rblOelstand.SelectedValue = Nothing
            rblMotorlauf.SelectedValue = Nothing
            rblGetriebe.SelectedValue = Nothing
            rblAnfahrtest.SelectedValue = Nothing
            rblKupplung.SelectedValue = Nothing
            rblProbefahrt.SelectedValue = Nothing
            rblFensterheber.SelectedValue = Nothing
            rblRadlager.SelectedValue = Nothing
            rblVerdeck.SelectedValue = Nothing
            rblGeblaese.SelectedValue = Nothing
            rblKlima.SelectedValue = Nothing
            rblRadioNavi.SelectedValue = Nothing
            rblSchluessel.SelectedValue = Nothing
        Else
            rblUnfall.SelectedValue = 0
            rblTechMaengel.SelectedValue = 0
            rblAustauschMotor.SelectedValue = 0
            rblAustauschGetriebe.SelectedValue = 0
            rblTacho.SelectedValue = 0
            rblOelstand.SelectedValue = 1
            rblMotorlauf.SelectedValue = 1
            rblGetriebe.SelectedValue = 1
            rblAnfahrtest.SelectedValue = 1
            rblKupplung.SelectedValue = 1
            rblProbefahrt.SelectedValue = 1
            rblFensterheber.SelectedValue = 1
            rblRadlager.SelectedValue = 1
            rblVerdeck.SelectedValue = 1
            rblGeblaese.SelectedValue = 1
            rblKlima.SelectedValue = 1
            rblRadioNavi.SelectedValue = 1
            rblSchluessel.SelectedValue = 1
        End If
    End Sub

    Private Function ValidateControls() As Boolean

        Dim isError As Boolean = False


        Dim RadioButtonList As New ArrayList()

        With RadioButtonList
            .Add(rblAnfahrtest)
            .Add(rblAustauschGetriebe)
            .Add(rblAustauschMotor)
            .Add(rblDispokontakt)
            .Add(rblFehlfahrt)
            .Add(rblFensterheber)
            .Add(rblFoto)
            .Add(rblGeblaese)
            '.Add(rblGenehmigungMail)
            .Add(rblGetriebe)
            .Add(rblKennzMontage)
            .Add(rblKlima)
            .Add(rblKupplung)
            .Add(rblMotorlauf)
            .Add(rblOelstand)
            .Add(rblProbefahrt)
            .Add(rblRadioNavi)
            .Add(rblRadlager)
            .Add(rblReifenhandling)
            .Add(rblSchaedBeiUeb)
            .Add(rblSchluessel)
            .Add(rblTacho)
            .Add(rblTechMaengel)
            .Add(rblUnfall)
            .Add(rblVerdeck)
            .Add(rblVorholung)
        End With


        For i As Integer = 0 To RadioButtonList.Count - 1

            If CType(RadioButtonList.Item(i), RadioButtonList).SelectedIndex = -1 Then
                isError = True
                CType(RadioButtonList.Item(i), RadioButtonList).BorderColor = Color.Red
            End If


        Next


        If IsNumeric(txtBetankung.Text) = False AndAlso txtBetankung.Text.Length > 0 Then
            isError = True
            txtBetankung.BorderColor = Color.Red
        End If

        If IsNumeric(txtFahrzeugwaesche.Text) = False AndAlso txtFahrzeugwaesche.Text.Length > 0 Then
            isError = True
            txtFahrzeugwaesche.BorderColor = Color.Red
        End If

        If IsNumeric(txtInnenreinigung.Text) = False AndAlso txtInnenreinigung.Text.Length > 0 Then
            isError = True
            txtInnenreinigung.BorderColor = Color.Red
        End If

        If IsNumeric(txtMaut.Text) = False AndAlso txtMaut.Text.Length > 0 Then
            isError = True
            txtMaut.BorderColor = Color.Red
        End If

        If IsNumeric(txtWartezeit.Text) = False AndAlso txtWartezeit.Text.Length > 0 Then
            isError = True
            txtWartezeit.BorderColor = Color.Red
        End If

        If cblUebernahmebedingungen.SelectedIndex = -1 Then
            isError = True
            cblUebernahmebedingungen.BorderColor = Color.Red
        End If

        If txtKennzeichen.Text.Length = 0 Then
            isError = True
            txtKennzeichen.BorderColor = Color.Red
        End If

        If txtFahrgestellnummer.Text.Length = 0 Then
            isError = True
            txtFahrgestellnummer.BorderColor = Color.Red
        End If

        If txtTachostand.Text.Length = 0 Then
            isError = True
            txtTachostand.BorderColor = Color.Red
        End If

        'Datum\Uhrzeit
        If txtUebergabeDatum.Text.Length <> 10 Then
            isError = True
            txtUebergabeDatum.BorderColor = Color.Red
        ElseIf IsDate(txtUebergabeDatum.Text) = False Then
            isError = True
            txtUebergabeDatum.BorderColor = Color.Red
        End If

        If txtUebergabeZeit1.Text.Length = 0 Then
            isError = True
            txtUebergabeZeit1.BorderColor = Color.Red
        End If

        If txtUebergabeZeit2.Text.Length = 0 Then
            isError = True
            txtUebergabeZeit2.BorderColor = Color.Red
        End If

        If txtUebernahmeUhrzeit1.Text.Length = 0 Then
            isError = True
            txtUebernahmeUhrzeit1.BorderColor = Color.Red
        End If

        If txtUebernahmeUhrzeit2.Text.Length = 0 Then
            isError = True
            txtUebernahmeUhrzeit2.BorderColor = Color.Red
        End If

        Dim dt As DateTime
        Dim UebTime As String


        If txtUebergabeZeit1.Text.Length > 0 AndAlso txtUebergabeZeit2.Text.Length > 0 Then

            UebTime = txtUebergabeZeit1.Text & ":" & txtUebergabeZeit2.Text

            If DateTime.TryParse(UebTime, dt) = False Then
                isError = True
                txtUebergabeZeit1.BorderColor = Color.Red
                txtUebergabeZeit2.BorderColor = Color.Red
            End If

        End If

        If txtUebernahmeUhrzeit1.Text.Length > 0 AndAlso txtUebernahmeUhrzeit2.Text.Length > 0 Then

            UebTime = txtUebernahmeUhrzeit1.Text & ":" & txtUebernahmeUhrzeit2.Text

            If DateTime.TryParse(UebTime, dt) = False Then
                isError = True
                txtUebernahmeUhrzeit1.BorderColor = Color.Red
                txtUebernahmeUhrzeit2.BorderColor = Color.Red
            End If

        End If




        'If rblGenehmigungMail.SelectedIndex > -1 Then
        '    If rblGenehmigungMail.SelectedItem.Value = 1 Then
        '        If CKG.Base.Business.HelpProcedures.EmailAddressCheck(txtMail.Text) = False Then
        '            isError = True
        '            txtMail.BorderColor = Color.Red
        '        End If
        '    End If
        'End If

        Return isError

    End Function

    Private Sub Save()

        Dim SaveData As New RuecknahmeQuittung()

        With SaveData

            .ANFAHRTEST = rblAnfahrtest.SelectedItem.Value
            .AUSTAUSCH_GETR = rblAustauschGetriebe.SelectedItem.Value
            .AUSTAUSCH_MOT = rblAustauschMotor.SelectedItem.Value
            .AUSTAUSCH_TACHO = rblTacho.SelectedItem.Value
            .BEMERKUNG = txtBemerkungen.Text
            .BETANKUNG = Replace(txtBetankung.Text, ",", ".")
            .DISPOKONT_NOTW = rblDispokontakt.SelectedItem.Value
            .DUNKELHEIT = IIf(cblUebernahmebedingungen.Items(3).Selected, 1, 0)
            '.EMAIL_ADR = txtMail.Text
            .ERSCHW_BED = IIf(cblUebernahmebedingungen.Items(0).Selected, 1, 0)
            .FAHRER = m_User.Reference
            .FAHRZEUGWAESCHE = txtFahrzeugwaesche.Text
            .FEHLFAHRT = rblFehlfahrt.SelectedItem.Value
            .FENSTERHEBER = rblFensterheber.SelectedItem.Value
            .FOTO_BEI_UEB = rblFoto.SelectedItem.Value
            .GEBLAESE = rblAnfahrtest.SelectedItem.Value
            '.GENEHMIG_MAEIL = rblGenehmigungMail.SelectedItem.Value
            .GETRIEBE = rblGetriebe.SelectedItem.Value
            .INNENREINIGUNG = Replace(txtInnenreinigung.Text, ",", ".")
            .KENNZ_MONTAGE = rblKennzMontage.SelectedItem.Value
            .KLIMAANLAGE = rblKlima.SelectedItem.Value
            .KUPPLUNG = rblKupplung.SelectedItem.Value
            .MAUT = Replace(txtMaut.Text, ",", ".")
            .MOTORLAUF = rblMotorlauf.SelectedItem.Value
            .OELSTAND = rblOelstand.SelectedItem.Value
            .PARKHAUS = IIf(cblUebernahmebedingungen.Items(4).Selected, 1, 0)
            .PROBEFAHRT = rblProbefahrt.SelectedItem.Value
            .RADIO_NAVI = rblRadioNavi.SelectedItem.Value
            .RADLAGER = rblRadlager.SelectedItem.Value
            .REGEN_NASS = IIf(cblUebernahmebedingungen.Items(2).Selected, 1, 0)
            .REIFENHANDLING = rblReifenhandling.SelectedItem.Value
            .SCHAED_BEI_UEB = rblSchaedBeiUeb.SelectedItem.Value
            .SCHLUESSEL = rblSchluessel.SelectedItem.Value
            .SCHNEE_EIS = IIf(cblUebernahmebedingungen.Items(2).Selected, 1, 0)
            .SONSTIGES = IIf(cblUebernahmebedingungen.Items(7).Selected, 1, 0)
            .TECHN_MAENGEL = rblTechMaengel.SelectedItem.Value
            .UNFALL = rblUnfall.SelectedItem.Value
            .VBELN = lblAuftrag.Text
            .VERDECK = rblVerdeck.SelectedItem.Value
            .VERSCHMUTZT = IIf(cblUebernahmebedingungen.Items(1).Selected, 1, 0)
            .VORHOL_NACHBR = rblVorholung.SelectedItem.Value
            .WARTEZEIT = txtWartezeit.Text
            .ZEITDRU_BEI_UEB = IIf(cblUebernahmebedingungen.Items(6).Selected, 1, 0)
            .ZZFAHRG = txtFahrgestellnummer.Text
            .ZZKENN = txtKennzeichen.Text
            .IUG_DAT = txtUebergabeDatum.Text
            .IUG_TIM = txtUebergabeZeit1.Text.PadLeft(2, "0"c) & txtUebergabeZeit2.Text.PadRight(2, "0"c) & "00"
            .IUN_TIM = txtUebernahmeUhrzeit1.Text.PadLeft(2, "0"c) & txtUebernahmeUhrzeit2.Text.PadRight(2, "0"c) & "00"
            .KM_BEI_UEBERG = txtTachostand.Text
            .KM_BEI_UEBERNAHM = txtTachostandUebernahme.Text
            .OEL = txtOel.Text
            .UEBERNACHTUNG = txtUebernachtung.Text
            .SONST_AUSLAGEN = txtSonstigeAuslagen.Text
        End With


        SaveData.Save(m_User, m_App, Session("AppID").ToString)

        If SaveData.ErrMessage.Length = 0 Then
            lblInfo.Text = "Ihre Daten wurden gespeichert."
            TableForm.Visible = False
            lbtSave.Visible = False
        Else
            lblError.Text = "Fehler: Ihre Daten konnten nicht gespeichert werden. Fehler: " & SaveData.ErrMessage
        End If

    End Sub

    Private Sub ResetBorderColor()

        lblError.Text = ""

        Dim RadioButtonList As New ArrayList()

        With RadioButtonList
            .Add(rblAnfahrtest)
            .Add(rblAustauschGetriebe)
            .Add(rblAustauschMotor)
            .Add(rblDispokontakt)
            .Add(rblFehlfahrt)
            .Add(rblFensterheber)
            .Add(rblFoto)
            .Add(rblGeblaese)
            '.Add(rblGenehmigungMail)
            .Add(rblGetriebe)
            .Add(rblKennzMontage)
            .Add(rblKlima)
            .Add(rblKupplung)
            .Add(rblMotorlauf)
            .Add(rblOelstand)
            .Add(rblProbefahrt)
            .Add(rblRadioNavi)
            .Add(rblRadlager)
            .Add(rblReifenhandling)
            .Add(rblSchaedBeiUeb)
            .Add(rblSchluessel)
            .Add(rblTacho)
            .Add(rblTechMaengel)
            .Add(rblUnfall)
            .Add(rblVerdeck)
            .Add(rblVorholung)
        End With


        For i As Integer = 0 To RadioButtonList.Count - 1

            CType(RadioButtonList.Item(i), RadioButtonList).BorderColor = Color.Empty

        Next

        txtBetankung.BorderColor = Color.Empty
        txtFahrzeugwaesche.BorderColor = Color.Empty
        txtInnenreinigung.BorderColor = Color.Empty
        txtMaut.BorderColor = Color.Empty
        txtWartezeit.BorderColor = Color.Empty
        cblUebernahmebedingungen.BorderColor = Color.Empty
        txtKennzeichen.BorderColor = Color.Empty
        txtFahrgestellnummer.BorderColor = Color.Empty
        txtTachostand.BorderColor = Color.Empty
        txtUebergabeDatum.BorderColor = Color.Empty
        txtUebergabeZeit1.BorderColor = Color.Empty
        txtUebergabeZeit2.BorderColor = Color.Empty
        txtUebernahmeUhrzeit1.BorderColor = Color.Empty
        txtUebernahmeUhrzeit2.BorderColor = Color.Empty
        'txtMail.BorderColor = Color.Empty

    End Sub



#End Region

End Class

' ************************************************
' $History: Change02.aspx.vb $
' 
' *****************  Version 8  *****************
' User: Fassbenders  Date: 26.05.11   Time: 9:17
' Updated in $/CKAG/Applications/AppUeberf/Forms
' 
' *****************  Version 7  *****************
' User: Dittbernerc  Date: 3.05.11    Time: 10:55
' Updated in $/CKAG/Applications/AppUeberf/Forms
' 
' *****************  Version 6  *****************
' User: Dittbernerc  Date: 26.04.11   Time: 12:29
' Updated in $/CKAG/Applications/AppUeberf/Forms
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 10.11.10   Time: 11:52
' Updated in $/CKAG/Applications/AppUeberf/Forms
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 29.10.10   Time: 13:26
' Updated in $/CKAG/Applications/AppUeberf/Forms
' ITA: 4240
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 5.05.10    Time: 10:17
' Updated in $/CKAG/Applications/AppUeberf/Forms
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 23.04.10   Time: 14:35
' Updated in $/CKAG/Applications/AppUeberf/Forms
' ITA: 3669
' 