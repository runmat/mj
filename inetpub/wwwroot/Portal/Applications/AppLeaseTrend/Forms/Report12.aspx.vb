Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Report12
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App


    Protected WithEvents ucHeader As Header
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblInfo As System.Web.UI.WebControls.Label
    Protected WithEvents txtLV As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtKennzeichen As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtFahrgestellnr As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents cmdCreate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents txtErfassungsDatumVon As eWorld.UI.CalendarPopup
    Protected WithEvents txtErfassungsDatumBis As eWorld.UI.CalendarPopup
    Protected WithEvents ucStyles As Styles


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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub


    'Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
    '    Session("ShowLink") = "True"
    '    DoSubmit()
    'End Sub

    Private Function CheckInput() As Boolean
        Dim blnEmptyDatVon As Boolean = True
        Dim blnEmptyDatbis As Boolean = True
        Dim blnEmptyLVNr As Boolean = True
        Dim blnEmptyKennzeichen As Boolean = True
        Dim blnEmptyFahrgestellnr As Boolean = True
        Dim blnResult As Boolean = True

        If (Not txtErfassungsDatumVon.ValidDateEntered) Then
            blnEmptyDatVon = False
        End If

        If (Not txtErfassungsDatumBis.ValidDateEntered) Then
            blnEmptyDatbis = False
        End If

        If (txtLV.Text.Trim.Length = 0) Then
            blnEmptyLVNr = False
        End If

        If (txtKennzeichen.Text.Trim.Length = 0) Then
            blnEmptyKennzeichen = False
        End If

        If (txtFahrgestellnr.Text.Trim.Length = 0) Then
            blnEmptyFahrgestellnr = False
        End If

        If Not (blnEmptyDatVon Or blnEmptyDatbis Or blnEmptyLVNr Or blnEmptyKennzeichen Or blnEmptyFahrgestellnr) Then
            blnResult = False
            lblError.Text = "Bitte geben Sie mindestens ein Suchkriterium ein!"
        End If

        Return blnResult
    End Function

    Private Sub DoSubmit()
        Session("lnkExcel") = ""
        Try
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            Dim m_Report As New LT_12(m_User, m_App, strFileName)

            lblError.Text = ""

            If CheckInput() Then
                With m_Report
                    .PErfassungsDatumVon = String.Empty
                    .PErfassungsDatumBis = String.Empty

                    If txtErfassungsDatumVon.ValidDateEntered Then
                        .PErfassungsDatumVon = txtErfassungsDatumVon.SelectedDate
                    End If
                    If txtErfassungsDatumBis.ValidDateEntered Then
                        .PErfassungsDatumBis = txtErfassungsDatumBis.SelectedDate
                    End If
                    .PLVnr = txtLV.Text
                    .PKennzeichen = txtKennzeichen.Text
                    .PFahrgestellnr = txtFahrgestellnr.Text
                    .Fill(Session("AppID").ToString, Session.SessionID.ToString, Me)
                End With

                Session("ResultTable") = m_Report.Result

                If Not m_Report.Status = 0 Then
                    lblError.Text = "Fehler: " & m_Report.Message
                Else
                    If m_Report.Result.Rows.Count = 0 Then
                        lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                    Else

                        Try
                            Excel.ExcelExport.WriteExcel(m_Report.Result, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                        Catch
                        End Try
                        Session("lnkExcel") = "/Portal/Temp/Excel/" & strFileName
                        Response.Redirect("/Portal/(S(" & Session.SessionID & "))/Shared/Report01_2.aspx?AppID=" & Session("AppID").ToString)
                    End If
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten."
        End Try
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        Session("ShowLink") = "True"
        DoSubmit()
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Report12.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 30.06.09   Time: 14:58
' Updated in $/CKAG/Applications/AppLeaseTrend/Forms
' ITA 2918  Z_M_FEHLENDE_VERTRAGSNUMMERN, Z_M_BRIEFLEBENSLAUF_LT,
' Z_M_BRIEFERSTEINGANG_LEASETR, Z_M_BRIEFANFORDERUNG_ALLG,
' Z_M_UNANGEFORDERT_L, Z_M_KUNDENDATEN_LT, Z_M_VERSENDETE_ZB2_ENDG_LT,
' Z_M_VERSENDETE_ZB2_TEMP_LT, Z_M_BRIEF_TEMP_VERS_MAHN_002,
' Z_M_DATEN_OHNE_ZB2_LT
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 4.05.09    Time: 9:12
' Updated in $/CKAG/Applications/AppLeaseTrend/Forms
' ITA: 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 15:07
' Created in $/CKAG/Applications/AppLeaseTrend/Forms
' 
' *****************  Version 7  *****************
' User: Uha          Date: 2.07.07    Time: 13:19
' Updated in $/CKG/Applications/AppLeaseTrend/AppLeaseTrendWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 6  *****************
' User: Uha          Date: 23.05.07   Time: 9:21
' Updated in $/CKG/Applications/AppLeaseTrend/AppLeaseTrendWeb/Forms
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 5  *****************
' User: Uha          Date: 8.03.07    Time: 10:46
' Updated in $/CKG/Applications/AppLeaseTrend/AppLeaseTrendWeb/Forms
' 
' ************************************************
