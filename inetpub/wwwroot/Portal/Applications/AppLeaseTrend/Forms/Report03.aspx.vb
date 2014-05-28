Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Report03
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
    Private m_App As Base.Kernel.Security.App

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents cmdCreate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ucHeader As Header
    Protected WithEvents txtBriefnummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtFahrgestellnummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAmtlKennzeichen As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtOrdernummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles

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

            'DoSubmit()
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        Session("ShowLink") = "True"
        If (txtAmtlKennzeichen.Text = String.Empty And txtOrdernummer.Text = String.Empty) Then
            lblError.Text = "Bitte geben Sie mindestens ein Suchkriterium an."
            Exit Sub
        End If
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        Dim m_Report As New LT_03(m_User, m_App, "")
        Dim b As Boolean

        Try
            Dim strBriefnummer As String
            Dim strFahrgestellnummer As String
            Dim strAmtlKennzeichen As String
            Dim strOrdernummer As String

            b = True
            If txtOrdernummer.Text.Length = 0 Then
                strOrdernummer = ""
            Else
                strOrdernummer = txtOrdernummer.Text & "%"
            End If
            If txtBriefnummer.Text.Length = 0 Then
                strBriefnummer = ""
            Else
                strBriefnummer = txtBriefnummer.Text
            End If
            If txtFahrgestellnummer.Text.Length = 0 Then
                strFahrgestellnummer = ""
            Else
                strFahrgestellnummer = txtFahrgestellnummer.Text
            End If
            If txtAmtlKennzeichen.Text.Length = 0 Then
                strAmtlKennzeichen = ""
            Else
                strAmtlKennzeichen = txtAmtlKennzeichen.Text
            End If

            If txtBriefnummer.Text.Length + txtFahrgestellnummer.Text.Length + txtAmtlKennzeichen.Text.Length + txtOrdernummer.Text.Length = 0 Then
                lblError.Text = "Bitte geben Sie ein Suchkriterium ein."
                b = False
            Else
                m_Report.FillHistory(Session("AppID").ToString, Session.SessionID.ToString, strAmtlKennzeichen, strFahrgestellnummer, strBriefnummer, strOrdernummer, Me)
                Session("ResultTable") = m_Report.History
            End If
        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            b = False
        End Try

        If b Then
            If Not m_Report.Status = 0 Then
                lblError.Text = m_Report.Message
            Else
                If (m_Report.History Is Nothing) OrElse (m_Report.History.Rows.Count = 0) OrElse (m_Report.History.Rows(0)("ZZFAHRG").ToString = String.Empty) Then
                    lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                Else
                    Response.Redirect("Report03_2.aspx?AppID=" & Session("AppID").ToString)
                End If
            End If
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Report03.aspx.vb $
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
' *****************  Version 5  *****************
' User: Uha          Date: 2.07.07    Time: 13:19
' Updated in $/CKG/Applications/AppLeaseTrend/AppLeaseTrendWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 4  *****************
' User: Uha          Date: 8.03.07    Time: 10:46
' Updated in $/CKG/Applications/AppLeaseTrend/AppLeaseTrendWeb/Forms
' 
' ************************************************
