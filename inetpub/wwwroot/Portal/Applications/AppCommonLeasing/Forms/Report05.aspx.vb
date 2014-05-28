Option Explicit On
Option Strict On

Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Report05
    Inherits System.Web.UI.Page
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles

    Protected WithEvents lb_weiter As LinkButton

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App

    Private mObjVersendeteZBIITemp As VersendeteZBIITemp

    Protected WithEvents txtBisDatum As TextBox
    Protected WithEvents txtAbDatum As TextBox
    Protected WithEvents txtLeasingVertragsnummer As TextBox

    Protected WithEvents imgbDatumAb As ImageButton
    Protected WithEvents imgbDatumBis As ImageButton

    Protected WithEvents calAbDatum As Calendar
    Protected WithEvents calBisDatum As Calendar


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

        ' Hier Benutzercode zur Seiteninitialisierung einfügen
        Try
            m_User = GetUser(Me)
            m_App = New Base.Kernel.Security.App(m_User) 'erzeugt ein App_objekt 
            ucHeader.InitUser(m_User)
            FormAuth(Me, m_User)
            lblError.Text = ""

            If Not IsPostBack Then
                GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")
                lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
                ucStyles.TitleText = lblHead.Text
            End If

        Catch ex As Exception
            lblError.Text = "beim laden der Seite ist ein Fehler aufgetreten: " & ex.Message
        End Try
    End Sub

    Private Sub lb_Weiter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_weiter.Click
        doSubmit()
    End Sub

    Private Sub doSubmit()
        Dim errorText As String = ""
        If HelpProcedures.checkDate(txtAbDatum, txtBisDatum, errorText, True, 1) Then
            If Not txtAbDatum.Text.Trim(" "c) = "" OrElse Not txtBisDatum.Text.Trim(" "c) = "" OrElse Not txtLeasingVertragsnummer.Text.Trim(" "c) = "" Then
                mObjVersendeteZBIITemp = New VersendeteZBIITemp(m_User, m_App, "")

                mObjVersendeteZBIITemp.DatumAb = txtAbDatum.Text
                mObjVersendeteZBIITemp.DatumBis = txtBisDatum.Text
                mObjVersendeteZBIITemp.Leasingvertragsnummer = txtLeasingVertragsnummer.Text

                mObjVersendeteZBIITemp.Fill(Session("AppId").ToString)
                If mObjVersendeteZBIITemp.Status = 0 Then
                    Session.Add("mObjVersendeteZBIITempSession", mObjVersendeteZBIITemp)
                    Response.Redirect("Report05_1.aspx?AppID=" & Session("AppID").ToString)
                Else
                    lblError.Text = mObjVersendeteZBIITemp.Message
                    Exit Sub
                End If
            Else
                lblError.Text = "Geben Sie mindestens ein Selektionskriterium ein"
            End If
        Else
            lblError.Text = errorText
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Protected Sub imgbDatumAb_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbDatumAb.Click
        calAbDatum.Visible = Not calAbDatum.Visible
    End Sub

    Protected Sub imgbDatumBis_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbDatumBis.Click
        calBisDatum.Visible = Not calBisDatum.Visible
    End Sub

    Protected Sub calAbDatum_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles calAbDatum.SelectionChanged
        txtAbDatum.Text = calAbDatum.SelectedDate.ToShortDateString
        calAbDatum.Visible = False
    End Sub

    Protected Sub calBisDatum_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles calBisDatum.SelectionChanged
        txtBisDatum.Text = calBisDatum.SelectedDate.ToShortDateString
        calBisDatum.Visible = False
    End Sub
End Class
' ************************************************
' $History: Report05.aspx.vb $
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 3.11.08    Time: 15:53
' Created in $/CKAG/Applications/AppCommonLeasing/Forms
' ITA 2354 warte auf testdaten
' 
' ************************************************