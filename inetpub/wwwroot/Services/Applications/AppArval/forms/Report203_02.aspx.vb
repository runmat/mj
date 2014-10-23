Imports CKG
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports System.Text.RegularExpressions

Public Class Report203_02
    Inherits System.Web.UI.Page

    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private m_objTable As DataTable

    Protected WithEvents lblPageTitle As Label
    Protected WithEvents lnkFahrzeugsuche As HyperLink
    Protected WithEvents Label10 As Label
    Protected WithEvents Label37 As Label
    Protected WithEvents Label23 As Label
    Protected WithEvents Label1 As Label
    Protected WithEvents Label2 As Label
    Protected WithEvents lblHalter As Label
    Protected WithEvents lblHOrt As Label
    Protected WithEvents lblPerso As Label
    Protected WithEvents lblGewerbe As Label
    Protected WithEvents lblEinzug As Label
    Protected WithEvents Label4 As Label
    Protected WithEvents lblVollst As Label
    Protected WithEvents Label12 As Label
    Protected WithEvents Label18 As Label
    Protected WithEvents Label27 As Label
    Protected WithEvents Label7 As Label
    Protected WithEvents Label8 As Label
    Protected WithEvents Label24 As Label
    Protected WithEvents Label26 As Label
    Protected WithEvents btnSave As LinkButton
    Protected WithEvents lblDateVollm As Label
    Protected WithEvents lbl_DateGew As Label
    Protected WithEvents lblVollmRegDate As Label
    Protected WithEvents txt_NummerEVB As TextBox
    Protected WithEvents txtDatumvon As TextBox
    Protected WithEvents txtDatumbis As TextBox
    Protected WithEvents lblBemerk As Label
    Protected WithEvents Label38 As Label
    Protected WithEvents lblKunnr As Label
    Protected WithEvents Label22 As Label
    Protected WithEvents lblVollmacht As Label
    Protected WithEvents Label21 As Label
    Protected WithEvents lblRegister As Label
    Protected WithEvents Tr5 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents Label3 As Label
    Protected WithEvents calBis As Calendar
    Protected WithEvents calVon As Calendar
    Protected WithEvents btnCal1 As LinkButton
    Protected WithEvents btnCal2 As LinkButton
    Protected WithEvents lblKUNNR_SAP As Label
    Protected WithEvents lblEVB As Label
    Protected WithEvents lblvon As Label
    Protected WithEvents lblBis As Label
    Protected WithEvents lblDateForm1 As Label
    Protected WithEvents lblDateForm2 As Label

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

        m_User = GetUser(Me)
        FormAuth(Me, m_User)

        Dim sKunnr As String = ""

        If (Session("ResultTable") Is Nothing) Then
            Response.Redirect(Request.UrlReferrer.ToString)
        Else
            m_objTable = CType(Session("ResultTable"), DataTable)
        End If

        m_App = New Base.Kernel.Security.App(m_User)

        If Not IsPostBack Then
            If Not Request.QueryString("Kunnr") Is Nothing Then

                sKunnr = Request.QueryString("Kunnr")
                Initialload(sKunnr)
            Else

            End If
        End If
    End Sub
    Private Sub Initialload(ByVal Kunnr As String)

        Dim dRows() As DataRow
        dRows = m_objTable.Select("Kundennr='" & Kunnr & "'")

        If dRows.Length > 0 Then
            lblHalter.Text = dRows(0)("Halter").ToString
            lblHOrt.Text = dRows(0)("HOrt").ToString
            lblVollmacht.Text = dRows(0)("Vollmacht").ToString
            lblKunnr.Text = dRows(0)("Kundennr").ToString
            lblRegister.Text = dRows(0)("Register").ToString
            lblKUNNR_SAP.Text = dRows(0)("KUNNR").ToString

            lblPerso.Text = dRows(0)("Personalausweis").ToString
            lblGewerbe.Text = dRows(0)("Gewerbeanmeld").ToString
            lblEinzug.Text = dRows(0)("Einzugserm").ToString
            lblVollst.Text = dRows(0)("Vollst").ToString


            lblDateVollm.Text = dRows(0)("Datum Vollmacht").ToString
            lbl_DateGew.Text = dRows(0)("RegDat").ToString
            lblVollmRegDate.Text = dRows(0)("Neue Vollmacht").ToString
            lblBemerk.Text = dRows(0)("Bemerkung").ToString
            txt_NummerEVB.Text = dRows(0)("EVB Nummer").ToString

            If IsDate(dRows(0)("gültig bis").ToString) Then txtDatumbis.Text = dRows(0)("gültig bis").ToString
            If IsDate(dRows(0)("gültig ab").ToString) Then txtDatumvon.Text = dRows(0)("gültig ab").ToString

        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Dim m_Report As New Arval_04(m_User, m_App, "")

        If Not txt_NummerEVB.Text.Length > 0 Then
            lblError.Visible = True
            lblError.Text = "Bitte geben Sie ""Nummer der Dauer-EVB"" ein!"
        Else
            lblError.Visible = False
            m_Report.EVBNr = (txt_NummerEVB.Text)
        End If

        If Not txtDatumbis.Text.Length > 0 Then
            lblError.Visible = True
            lblError.Text = "Bitte geben Sie ""Datum gültig bis"" ein!"
        ElseIf IsDate(txtDatumbis.Text) Then
            lblError.Visible = False
            m_Report.EVBbis = txtDatumbis.Text
        End If

        If Not txtDatumvon.Text.Length > 0 Then
            lblError.Visible = True
            lblError.Text = "Bitte geben Sie ""Datum gültig ab"" ein!"
        ElseIf IsDate(txtDatumvon.Text) Then
            lblError.Visible = False
            m_Report.EVBvon = txtDatumvon.Text
        End If
        m_Report.Update_EVB(Session("AppID").ToString, Session.SessionID, Me, lblKUNNR_SAP.Text)

        If m_Report.Message.Length > 0 Then
            lblError.Text = m_Report.Message
        Else
            Dim dRows() As DataRow
            Dim sKunnr As String = Request.QueryString("Kunnr")
            dRows = m_objTable.Select("Kundennr='" & sKunnr & "'")

            If dRows.Length > 0 Then
                dRows(0)("EVB Nummer") = txt_NummerEVB.Text
                dRows(0)("gültig bis") = txtDatumbis.Text
                dRows(0)("gültig ab") = txtDatumvon.Text
                m_objTable.AcceptChanges()
                Session("ResultTable") = m_objTable
            End If
            btnSave.Visible = False
            btnCal1.Visible = False
            btnCal2.Visible = False
            txt_NummerEVB.Visible = False
            txtDatumbis.Visible = False
            txtDatumvon.Visible = False
            lblEVB.Visible = True
            lblDateForm1.Visible = True
            lblDateForm2.Visible = True


            lblDateForm1.Text = txtDatumvon.Text
            lblDateForm2.Text = txtDatumbis.Text
            lblEVB.Text = txt_NummerEVB.Text
        End If

    End Sub

    Private Sub btnCal1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCal1.Click
        calVon.Visible = True
        calBis.Visible = False
    End Sub

    Private Sub btnCal2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCal2.Click
        calVon.Visible = False
        calBis.Visible = True
    End Sub

    Private Sub calVon_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calVon.SelectionChanged
        txtDatumvon.Text = calVon.SelectedDate.ToShortDateString
        calVon.Visible = False
    End Sub

    Private Sub calBis_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calBis.SelectionChanged
        txtDatumbis.Text = calBis.SelectedDate.ToShortDateString
        calBis.Visible = False
    End Sub
End Class