Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Report03
    Inherits System.Web.UI.Page
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents cmdCreate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents calVon As System.Web.UI.WebControls.Calendar
    Protected WithEvents calBis As System.Web.UI.WebControls.Calendar
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents txtErfassungsdatumVon As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtErfassungsdatumBis As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnBis As System.Web.UI.WebControls.LinkButton
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents lblInfo As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents ddlPDIs As System.Web.UI.WebControls.DropDownList
    Protected WithEvents btnVon As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ddlHersteller As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtLieferant As System.Web.UI.WebControls.TextBox
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
    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App

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

            If Not IsPostBack Then
                Initialload()
            Else

            End If
            'DoSubmit()
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten."
        End Try
    End Sub

    Private Sub Initialload()
        '§§§ JVE 18.07.2006: Hier DropDownlist mit PDIs füllen...
        Dim pdiListe As New PDIListe(m_User, m_App)
        'Dim strStatus As Integer
        Dim item As ListItem
        Dim row As DataRow

        pdiListe.getPDIs(Session("AppID").ToString, Session.SessionID)

        If (pdiListe.Status = 0) Then
            'Werte füllen...
            'item = New ListItem("-alle-", 0)
            'ddlPDIs.Items.Add(item)
            For Each row In pdiListe.PPDIs.Rows
                item = New ListItem(CStr(row("NAME1")), CStr(row("KUNPDI")))
                ddlPDIs.Items.Add(item)
            Next
        Else
            lblError.Text = pdiListe.Message
        End If

        'Herstellerliste****************
        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
        Dim objHersteller As Straub_05
        Dim tblHersteller As DataTable
        Dim tblHerstellerOutput As New DataTable()
        Dim rowNew As DataRow
        Dim rowExists As DataRow()
        Dim vwData As DataView
        Dim intLoop As Integer

        objHersteller = New Straub_05(m_User, m_App, strFileName)

        tblHerstellerOutput.Columns.Add("DOMVALUE_L", GetType(System.String))
        tblHerstellerOutput.Columns.Add("ZHERST", GetType(System.String))

        objHersteller.getData(Session("AppID"), Session.SessionID)

        If (objHersteller.Status = 0) Then
            tblHersteller = objHersteller.PHerstellerliste

            rowNew = tblHerstellerOutput.NewRow
            rowNew("DOMVALUE_L") = "0"
            rowNew("ZHERST") = "-alle-"
            tblHerstellerOutput.Rows.Add(rowNew)

            vwData = tblHersteller.DefaultView
            vwData.Sort = "ZHERST asc"

            For intLoop = 0 To vwData.Count - 1
                rowNew = tblHerstellerOutput.NewRow
                rowNew("DOMVALUE_L") = CStr(vwData.Item(intLoop)("DOMVALUE_L"))
                rowNew("ZHERST") = CStr(vwData.Item(intLoop)("DOMVALUE_L")) & " - " & CStr(vwData.Item(intLoop)("ZHERST"))
                tblHerstellerOutput.Rows.Add(rowNew)
            Next
            tblHerstellerOutput.AcceptChanges()

            With ddlHersteller
                .DataSource = tblHerstellerOutput
                .DataValueField = "DOMVALUE_L"
                .DataTextField = "ZHERST"
                .DataBind()
            End With
        Else
            lblError.Text = "Fehler beim Lesen der Halterdaten."
            cmdCreate.Enabled = False
        End If
        objHersteller.ResultTable = tblHerstellerOutput
        Session.Add("objSuche", objHersteller)

        Try
            txtErfassungsdatumBis.Text = Now.ToShortDateString
            txtErfassungsdatumVon.Text = CStr(ConfigurationSettings.AppSettings("DatumVon"))
        Catch ex As Exception
            lblError.Text = "Fehler beim Lesen der Selektionsparameter."
        End Try
    End Sub

    Private Sub DoSubmit()
        Dim checkInput As Boolean = True
        Session("lnkExcel") = ""
        Try
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            Dim m_Report As New Straub_03(m_User, m_App, strFileName)
            'Dim datErfassungsdatumVon As DateTime
            'Dim datErfassungsdatumBis As DateTime
            'Dim strPDI As String

            lblError.Text = ""
            'Datumsfelder prüfen
            If (txtErfassungsdatumVon.Text.Length = 0) Or (txtErfassungsdatumBis.Text.Length = 0) Then
                checkInput = False
            End If
            If (Not IsDate(txtErfassungsdatumVon.Text)) Or (Not IsDate(txtErfassungsdatumBis.Text)) Then
                checkInput = False
            End If
            If Not checkInput Then
                'Datumsfelder leer oder falsches Format
                lblError.Text = "Bitte geben Sie ein gültiges Eingangsdatum (von,bis) ein!"
            End If
            'Datumsfelder sind gefüllt und haben das richtige Format. Jetzt Werte prüfen.
            If checkInput Then
                m_Report.PDatumVon = CDate(txtErfassungsdatumVon.Text)
                m_Report.PDatumBis = CDate(txtErfassungsdatumBis.Text)
                m_Report.PHersteller = CStr(ddlHersteller.SelectedItem.Value)
                If m_Report.PHersteller = "0" Then
                    m_Report.PHersteller = String.Empty
                End If
                m_Report.PLieferant = txtLieferant.Text
                m_Report.PPDI = CStr(ddlPDIs.SelectedItem.Value)
                If (m_Report.PDatumVon > m_Report.PDatumBis) Then
                    checkInput = False
                    lblError.Text = "Eingangsdatum (von) muß kleiner oder gleich Eingangsdatum (bis) sein!<br>"
                End If
                '§§§ JVE 19.07.2006: Prüfung soll wieder raus!!
                'If checkInput AndAlso (m_Report.PDatumBis.Subtract(m_Report.PDatumVon).Days > 30) Then
                '    checkInput = False
                '    lblError.Text = "Der angegebene Zeitraum umfasst mehr als 30 Tage.<br>"
                'End If
            End If

            If checkInput Then
                m_Report.FILL(Session("AppID").ToString, Session.SessionID.ToString)

                Session("ResultTable") = m_Report.Result

                If Not m_Report.Status = 0 Then
                    lblError.Text = m_Report.Message
                Else
                    If m_Report.Result.Rows.Count = 0 Then
                        lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                    Else

                        Dim objExcelExport As New Excel.ExcelExport()
                        Try
                            objExcelExport.WriteExcel(m_Report.Result, ConfigurationSettings.AppSettings("ExcelPath") & strFileName)
                        Catch
                        End Try
                        Session("lnkExcel") = "/Portal/Temp/Excel/" & strFileName
                        Response.Redirect("/Portal/(S(" & Session.SessionID & "))/Shared/Report01_2.aspx?AppID=" & Session("AppID").ToString)
                    End If
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub btnVon_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        calVon.Visible = True
        calBis.Visible = False
    End Sub

    Private Sub btnBis_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBis.Click
        calVon.Visible = False
        calBis.Visible = True
    End Sub


    Private Sub calVon_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles calVon.SelectionChanged
        txtErfassungsdatumVon.Text = calVon.SelectedDate.ToShortDateString
        calVon.Visible = False
    End Sub

    Private Sub calBis_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles calBis.SelectionChanged
        txtErfassungsdatumBis.Text = calBis.SelectedDate.ToShortDateString
        calBis.Visible = False
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
' $History: Report03.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 23.03.10   Time: 17:05
' Updated in $/CKAG/Applications/AppSTRAUB/Forms
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 8.04.08    Time: 17:39
' Updated in $/CKAG/Applications/AppSTRAUB/Forms
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 17:38
' Created in $/CKAG/Applications/AppSTRAUB/Forms
' 
' *****************  Version 10  *****************
' User: Uha          Date: 2.07.07    Time: 14:28
' Updated in $/CKG/Applications/AppSTRAUB/AppSTRAUBWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 9  *****************
' User: Uha          Date: 23.05.07   Time: 8:53
' Updated in $/CKG/Applications/AppSTRAUB/AppSTRAUBWeb/Forms
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 8  *****************
' User: Uha          Date: 8.03.07    Time: 14:18
' Updated in $/CKG/Applications/AppSTRAUB/AppSTRAUBWeb/Forms
' 
' ************************************************
