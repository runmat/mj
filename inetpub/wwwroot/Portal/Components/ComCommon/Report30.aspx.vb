Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business


Public Class Report30
    Inherits System.Web.UI.Page

#Region " Vom Web Form Designer generierter Code "

    'Dieser Aufruf ist für den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: Diese Methode ist für den Web Form-Designer erforderlich
        'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
        InitializeComponent()
    End Sub

#End Region

    Private m_User As Base.Kernel.Security.User
    Private m_App As Security.App
    Private m_objTrace As Base.Kernel.Logging.Trace

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents cmdCreate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ucHeader As Header
    Protected WithEvents calBisDatum As System.Web.UI.WebControls.Calendar
    Protected WithEvents txtAbDatum As System.Web.UI.WebControls.TextBox
    Protected WithEvents calAbDatum As System.Web.UI.WebControls.Calendar
    Protected WithEvents txtBisDatum As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnOpenSelectAb As System.Web.UI.WebControls.Button
    Protected WithEvents btnOpenSelectBis As System.Web.UI.WebControls.Button
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents txtFahrgestellnummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtKennzeichen As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAbVergDatum As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnOpenSelectAbVerg As System.Web.UI.WebControls.Button
    Protected WithEvents calAbVergDatum As System.Web.UI.WebControls.Calendar
    Protected WithEvents txtBisVergDatum As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnOpenSelectBisVerg As System.Web.UI.WebControls.Button
    Protected WithEvents calBisVergDatum As System.Web.UI.WebControls.Calendar
    Protected WithEvents cbPlakettenartGruen As System.Web.UI.WebControls.CheckBox
    Protected WithEvents cbPlakettenartGelb As System.Web.UI.WebControls.CheckBox
    Protected WithEvents cbPlakettenartRot As System.Web.UI.WebControls.CheckBox
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Session("ShowLink") = "False"
        Session("ShowOtherString") = ""
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)


        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text
        m_App = New Security.App(m_User)

        If Not IsPostBack Then
            Session("Result") = Nothing
            Session("ResultTable") = Nothing
        End If
       
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        Session("ShowLink") = "True"
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        Session("lnkExcel") = ""
        Session("ShowOtherString") = ""

        lblError.Text = ""
        Dim blnIsDateAb As Boolean
        Dim blnIsDateBis As Boolean

        blnIsDateAb = True
        If Len(txtAbDatum.Text) > 0 Then
            If Not IsDate(txtAbDatum.Text) Then
                If Not IsStandardDate(txtAbDatum.Text) Then
                    If Not IsSAPDate(txtAbDatum.Text) Then
                        lblError.Text = "Geben Sie bitte ein gültiges Datum ein!<br>"
                        blnIsDateAb = False
                    End If
                End If
            End If
        Else
            blnIsDateAb = False
        End If
        Dim datAb As Date
        If lblError.Text.Length = 0 Then
            If blnIsDateAb Then
                datAb = CDate(txtAbDatum.Text)
            End If
        End If

        blnIsDateBis = True
        If Len(txtBisDatum.Text) > 0 Then
            If Not IsDate(txtBisDatum.Text) Then
                If Not IsStandardDate(txtBisDatum.Text) Then
                    If Not IsSAPDate(txtBisDatum.Text) Then
                        lblError.Text = "Geben Sie bitte ein gültiges Datum ein!<br>"
                        blnIsDateBis = False
                    End If
                End If
            End If
        Else
            blnIsDateBis = False
        End If
        Dim datBis As Date
        If lblError.Text.Length = 0 Then
            If blnIsDateBis Then
                datBis = CDate(txtBisDatum.Text)
            End If
        End If

        If (blnIsDateAb And blnIsDateBis) AndAlso (datAb > datBis) Then
            lblError.Text = "Das Enddatum darf nicht vor dem Anfangsdatum liegen!<br>"
        End If

        blnIsDateAb = True
        If Len(txtAbVergDatum.Text) > 0 Then
            If Not IsDate(txtAbVergDatum.Text) Then
                If Not IsStandardDate(txtAbVergDatum.Text) Then
                    If Not IsSAPDate(txtAbVergDatum.Text) Then
                        lblError.Text = "Geben Sie bitte ein gültiges Datum ein!<br>"
                        blnIsDateAb = False
                    End If
                End If
            End If
        Else
            blnIsDateAb = False
        End If
        Dim datAbVerg As Date
        If lblError.Text.Length = 0 Then
            If blnIsDateAb Then
                datAbVerg = CDate(txtAbVergDatum.Text)
            End If
        End If

        blnIsDateBis = True
        If Len(txtBisVergDatum.Text) > 0 Then
            If Not IsDate(txtBisVergDatum.Text) Then
                If Not IsStandardDate(txtBisVergDatum.Text) Then
                    If Not IsSAPDate(txtBisVergDatum.Text) Then
                        lblError.Text = "Geben Sie bitte ein gültiges Datum ein!<br>"
                        blnIsDateBis = False
                    End If
                End If
            End If
        Else
            blnIsDateBis = False
        End If
        Dim datBisVerg As Date
        If lblError.Text.Length = 0 Then
            If blnIsDateBis Then
                datBisVerg = CDate(txtBisVergDatum.Text)
            End If
        End If

        If (blnIsDateAb And blnIsDateBis) AndAlso (datAbVerg > datBisVerg) Then
            lblError.Text = "Das Enddatum darf nicht vor dem Anfangsdatum liegen!<br>"
        End If

        If (Not cbPlakettenartGelb.Checked) And (Not cbPlakettenartGruen.Checked) And (Not cbPlakettenartRot.Checked) Then
            lblError.Text = "Wählen Sie mindestens eine Plakettenart aus!<br>"
        End If
        If lblError.Text.Length > 0 Then
            Exit Sub
        End If

        'Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
        'Dim objHandler As New Kernel.Report_30(m_User, m_App, strFileName)
        Dim objHandler As New Kernel.Report_30(m_User, m_App, "")

        objHandler.Kennzeichen = txtKennzeichen.Text
        objHandler.Fahrgestellnummer = txtFahrgestellnummer.Text
        objHandler.PlakettenartGelb = cbPlakettenartGelb.Checked
        objHandler.PlakettenartGruen = cbPlakettenartGruen.Checked
        objHandler.PlakettenartRot = cbPlakettenartRot.Checked
        objHandler.VergabedatumBis = datBisVerg
        objHandler.VergabedatumVon = datAbVerg
        objHandler.ZulassungsdatumBis = datBis
        objHandler.ZulassungsdatumVon = datAb

        objHandler.Fill(Session("AppID").ToString, Session.SessionID.ToString)

        Session("Result") = objHandler.Result
        Session("ResultTable") = objHandler.ResultTable

        If Not objHandler.Status = 0 Then
            lblError.Text = "Fehler: " & objHandler.Message
        Else
            If objHandler.Result.Rows.Count = 0 Then
                lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
            Else
                
                Session("lnkExcel") = "True"
                Response.Redirect("Report31.aspx?AppID=" & Session("AppID").ToString)
            End If
        End If
    End Sub

    Private Sub btnOpenSelectAb_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpenSelectAb.Click
        calAbDatum.Visible = True
    End Sub

    Private Sub btnOpenSelectBis_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpenSelectBis.Click
        calBisDatum.Visible = True
    End Sub

    Private Sub calAbDatum_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calAbDatum.SelectionChanged
        calAbDatum.Visible = False
        txtAbDatum.Text = calAbDatum.SelectedDate.ToShortDateString
    End Sub

    Private Sub calBisDatum_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calBisDatum.SelectionChanged
        calBisDatum.Visible = False
        txtBisDatum.Text = calBisDatum.SelectedDate.ToShortDateString
    End Sub

    Private Sub btnOpenSelectAbVerg_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpenSelectAbVerg.Click
        calAbVergDatum.Visible = True
    End Sub

    Private Sub btnOpenSelectBisVerg_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpenSelectBisVerg.Click
        calBisVergDatum.Visible = True
    End Sub

    Private Sub calAbVergDatum_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles calAbVergDatum.SelectionChanged
        calAbVergDatum.Visible = False
        txtAbVergDatum.Text = calAbVergDatum.SelectedDate.ToShortDateString
    End Sub

    Private Sub calBisVergDatum_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles calBisVergDatum.SelectionChanged
        calBisVergDatum.Visible = False
        txtBisVergDatum.Text = calBisVergDatum.SelectedDate.ToShortDateString
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Report30.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 5.06.09    Time: 15:58
' Updated in $/CKAG/Components/ComCommon
' mögliche try catches entfernt
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 4.05.09    Time: 11:50
' Updated in $/CKAG/Components/ComCommon
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 10.04.08   Time: 17:37
' Updated in $/CKAG/Components/ComCommon
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:32
' Created in $/CKAG/Components/ComCommon
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 10:47
' Created in $/CKAG/portal/Components/ComCommon
' 
' *****************  Version 3  *****************
' User: Uha          Date: 12.07.07   Time: 13:40
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' Report "Vergabe von Feinstaubplaketten" als Testversion erzeugt
' 
' *****************  Version 2  *****************
' User: Uha          Date: 12.07.07   Time: 9:28
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 1  *****************
' User: Uha          Date: 11.07.07   Time: 18:22
' Created in $/CKG/Components/ComCommon/ComCommonWeb
' Report "Vergabe von Feinstaubplaketten" im Rohbau erzeugt
' 
' ************************************************
