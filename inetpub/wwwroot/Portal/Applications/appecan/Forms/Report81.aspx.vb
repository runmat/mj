Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Report81
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
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSelect As System.Web.UI.WebControls.LinkButton
    Protected WithEvents trCreate As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trSelect As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trSelectDropdown As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents ddlSelect As System.Web.UI.WebControls.DropDownList
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

            If Not IsPostBack Then
                trSelect.Visible = False
                trSelectDropdown.Visible = False
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        Session("ShowLink") = "True"
        If (txtAmtlKennzeichen.Text = String.Empty And txtBriefnummer.Text = String.Empty And txtFahrgestellnummer.Text = String.Empty) Then
            lblError.Text = "Bitte geben Sie mindestens ein Suchkriterium an."
            Exit Sub
        End If
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        Dim m_Report As New ECAN_07(m_User, m_App, "")
        Dim b As Boolean

        Try
            Dim strFahrgestellnummer As String
            Dim strAmtlKennzeichen As String
            Dim strBriefnummer As String = ""

            b = True
            If txtBriefnummer.Text.Length = 0 Then
                strBriefnummer = ""
            Else
                If (Not IsNumeric(Right(txtBriefnummer.Text, 6))) Or (Not txtBriefnummer.Text.Length = 8) Then
                    lblError.Text = "Bitte geben Sie die Fahrzeugbriefnummer 8-stellig ein (2 Buchstaben und 6 Ziffern)."
                    b = False
                Else
                    strBriefnummer = txtBriefnummer.Text
                End If
            End If

            txtFahrgestellnummer.Text = Replace(txtFahrgestellnummer.Text.Trim(" "c).Trim("*"c), " ", "")
            If txtFahrgestellnummer.Text.Length = 0 Then
                strFahrgestellnummer = ""
            Else
                strFahrgestellnummer = txtFahrgestellnummer.Text
                If strFahrgestellnummer.Length < 17 Then
                    If strFahrgestellnummer.Length > 7 Then
                        txtFahrgestellnummer.Text = "*" & strFahrgestellnummer
                        strFahrgestellnummer = "%" & strFahrgestellnummer
                    Else
                        lblError.Text = "Bitte geben Sie die Fahrgestellnummer mindestens 8-stellig ein."
                        b = False
                    End If
                End If
            End If
            If txtAmtlKennzeichen.Text.Length = 0 Then
                strAmtlKennzeichen = ""
            Else
                txtAmtlKennzeichen.Text = Replace(txtAmtlKennzeichen.Text.Trim(" "c), " ", "")
                strAmtlKennzeichen = txtAmtlKennzeichen.Text.Trim("*"c)
                Dim intTemp As Integer = InStr(strAmtlKennzeichen, "-")
                Select Case intTemp
                    Case 2
                        If strAmtlKennzeichen.Length < 3 Then
                            lblError.Text = "Bitte beachten Sie das Eingabeformat für Kennzeichen."
                            b = False
                        Else
                            If Right(txtAmtlKennzeichen.Text, 1) = "*" Then
                                txtAmtlKennzeichen.Text = strAmtlKennzeichen & "*"
                                strAmtlKennzeichen = strAmtlKennzeichen & "%"
                            End If
                        End If
                    Case 3
                        If strAmtlKennzeichen.Length < 4 Then
                            lblError.Text = "Bitte beachten Sie das Eingabeformat für Kennzeichen."
                            b = False
                        Else
                            If Right(txtAmtlKennzeichen.Text, 1) = "*" Then
                                txtAmtlKennzeichen.Text = strAmtlKennzeichen & "*"
                                strAmtlKennzeichen = strAmtlKennzeichen & "%"
                            End If
                        End If
                    Case 4
                        If strAmtlKennzeichen.Length < 5 Then
                            lblError.Text = "Bitte beachten Sie das Eingabeformat für Kennzeichen."
                            b = False
                        Else
                            If Right(txtAmtlKennzeichen.Text, 1) = "*" Then
                                txtAmtlKennzeichen.Text = strAmtlKennzeichen & "*"
                                strAmtlKennzeichen = strAmtlKennzeichen & "%"
                            End If
                        End If
                    Case Else
                        lblError.Text = "Bitte beachten Sie das Eingabeformat für Kennzeichen."
                        b = False
                End Select
            End If

            If txtBriefnummer.Text.Length + txtFahrgestellnummer.Text.Length + txtAmtlKennzeichen.Text.Length = 0 Then
                lblError.Text = "Bitte geben Sie ein Suchkriterium ein."
                b = False
            Else
                If b Then
                    m_Report.FillHistory(Session("AppID").ToString, Session.SessionID.ToString, Me, strAmtlKennzeichen, strFahrgestellnummer, strBriefnummer, "")
                    If m_Report.ResultCount > 1 Then
                        b = False
                        cmdCreate.Enabled = False
                        trSelect.Visible = True
                        trSelectDropdown.Visible = True
                        txtAmtlKennzeichen.Enabled = False
                        txtBriefnummer.Enabled = False
                        txtFahrgestellnummer.Enabled = False
                        Dim tmpView As DataView
                        tmpView = m_Report.Result.DefaultView
                        tmpView.Sort = "TIDNR"
                        ddlSelect.DataSource = tmpView
                        ddlSelect.DataValueField = "ZZFAHRG"
                        ddlSelect.DataTextField = "DISPLAY"
                        ddlSelect.DataBind()
                        lblError.Text = "Ihre Suche ergab mehrere Treffer. Bitte wählen Sie aus."
                    Else
                        Session("ResultTable") = m_Report.History
                    End If
                End If
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
                    Response.Redirect("Report81_2.aspx?AppID=" & Session("AppID").ToString)
                End If
            End If
        End If
        Session("ShowLink") = "False"
    End Sub

    Private Sub cmdSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSelect.Click
        Session("ShowLink") = "True"
        txtAmtlKennzeichen.Text = ""
        txtBriefnummer.Text = ""
        txtFahrgestellnummer.Text = ddlSelect.SelectedItem.Value.ToString
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
' $History: Report81.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 4.02.10    Time: 16:49
' Updated in $/CKAG/Applications/appecan/Forms
' ITA: 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 30.04.09   Time: 10:57
' Updated in $/CKAG/Applications/appecan/Forms
' ITA 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:07
' Created in $/CKAG/Applications/appecan/Forms
' 
' *****************  Version 4  *****************
' User: Uha          Date: 2.07.07    Time: 12:32
' Updated in $/CKG/Applications/AppECAN/AppECANWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 3  *****************
' User: Uha          Date: 7.03.07    Time: 11:16
' Updated in $/CKG/Applications/AppECAN/AppECANWeb/Forms
' 
' ************************************************
