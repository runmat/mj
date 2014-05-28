Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Report_03
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

    'Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    'Private m_objTable As DataTable
    'Private resultTable As DataTable

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblHead As Label
    Protected WithEvents lblLVNr As System.Web.UI.WebControls.Label
    Protected WithEvents btnSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents txtDatum As System.Web.UI.WebControls.TextBox
    Protected WithEvents Image1 As System.Web.UI.WebControls.Image
    Protected WithEvents cbxSB As System.Web.UI.WebControls.CheckBox
    Protected WithEvents cbxEnt As System.Web.UI.WebControls.CheckBox
    Protected WithEvents cbxVers As System.Web.UI.WebControls.CheckBox
    Protected WithEvents cbxFahrz As System.Web.UI.WebControls.CheckBox
    Protected WithEvents txtBemerkung As System.Web.UI.WebControls.TextBox
    Protected WithEvents Image2 As System.Web.UI.WebControls.Image
    Protected WithEvents lnkFahrzeugsuche As System.Web.UI.WebControls.HyperLink
    Protected WithEvents btnAbsenden As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ShowScript As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        If (Session("ResultTableNative") Is Nothing) Then
            Response.Redirect(Request.UrlReferrer.ToString)
            'Else
            '    m_objTable = CType(Session("ResultTableNative"), DataTable)
        End If
        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text
        Try
            'm_App = New Base.Kernel.Security.App(m_User)
            If Not IsPostBack Then
                showdata()
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub showdata()
        Dim equi As String
        Dim tblData As DataTable
        Dim selectedRow As DataRow

        If (Session("ResultTableNative") Is Nothing) Then
            Response.Redirect(Request.UrlReferrer.ToString & "?AppID=" & Session("AppID").ToString)
            'Else
            '    resultTable = CType(Session("ResultTableNative"), DataTable)
        End If

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text

        'Daten filtern
        equi = Request.Item("e")    'Equipment-Nr. holen

        tblData = CType(Session("ResultTableNative"), DataTable)            'LV-Nr. anzeigen
        selectedRow = tblData.Select("Equipment='" & equi & "'")(0)
        lblLVNr.Text = CStr(selectedRow("LVNr"))
    End Sub

    Private Function checkData() As Boolean
        Dim blnResult As Boolean
        Dim datDate As Date

        blnResult = False

        If (txtBemerkung.Text.Trim <> String.Empty) Then
            blnResult = True
        End If

        If (txtDatum.Text.Trim <> String.Empty) Then
            Try
                datDate = CType(txtDatum.Text, Date)
                blnResult = True
            Catch ex As Exception
                blnResult = False
            End Try
        End If

        If cbxEnt.Checked Then
            blnResult = True
        End If

        If cbxFahrz.Checked Then
            blnResult = True
        End If

        If cbxSB.Checked Then
            blnResult = True
        End If

        If cbxVers.Checked Then
            blnResult = True
        End If

        Return blnResult
    End Function
    
    Private Sub btnAbsenden_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAbsenden.Click
        Dim status As String = ""

        If checkData() Then
            sendMail(status)
            If status <> String.Empty Then
                lblError.Text = status
            Else
                lblError.Text = "Vorgang erfolgreich."
                txtBemerkung.Enabled = False
                txtDatum.Enabled = False
                cbxEnt.Enabled = False
                cbxFahrz.Enabled = False
                cbxSB.Enabled = False
                cbxVers.Enabled = False
                btnAbsenden.Enabled = False
            End If
        Else
            lblError.Text = "Keine gültigen Eingabedaten."
        End If
    End Sub

    Private Sub sendMail(ByRef status As String)
        'Mailversand...
        Dim mail As Net.Mail.MailMessage
        Dim strMailAdresse As String
        Dim strMailBody As String

        status = String.Empty

        Try

            strMailAdresse = ConfigurationManager.AppSettings("SmtpMailAddress")
            strMailBody = "Benutzername: " & m_User.UserName & vbCrLf

            strMailBody &= "LV-Nr.: " & lblLVNr.Text & vbCrLf
            strMailBody &= "LV beendet zum: " & txtDatum.Text & vbCrLf
            strMailBody &= "SB ist in Ordnung: "
            If cbxSB.Checked Then
                strMailBody &= "(X)"
            Else
                strMailBody &= "()"
            End If

            strMailBody &= vbCrLf & "Höhe der Entschädigung im Schadensfall ist in Ordnung: "
            If cbxEnt.Checked Then
                strMailBody &= "(X)"
            Else
                strMailBody &= "()"
            End If

            strMailBody &= vbCrLf & "Versichererwechsel: "
            If cbxVers.Checked Then
                strMailBody &= "(X)"
            Else
                strMailBody &= "()"
            End If

            strMailBody &= vbCrLf & "Fahrzeugwechsel: "
            If cbxFahrz.Checked Then
                strMailBody &= "(X)"
            Else
                strMailBody &= "()"
            End If
            strMailBody &= vbCrLf & "Sonstiges: " & txtBemerkung.Text & vbCrLf

            mail = New Net.Mail.MailMessage(ConfigurationManager.AppSettings("SmtpMailSender"), strMailAdresse, _
                                                   "Klärfallmail Arval", strMailBody)
            Dim client As New Net.Mail.SmtpClient(ConfigurationManager.AppSettings("SmtpMailServer"))
            client.Send(mail)

        Catch ex As Exception
            status = "Fehler beim Versenden der Mail."
        End Try
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Report_03.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 16.04.08   Time: 11:53
' Updated in $/CKAG/Applications/apparval/Forms
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 16:18
' Created in $/CKAG/Applications/apparval/Forms
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 8.11.07    Time: 10:01
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Forms
' 
' *****************  Version 4  *****************
' User: Uha          Date: 2.07.07    Time: 10:05
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 3  *****************
' User: Uha          Date: 6.03.07    Time: 14:48
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Forms
' 
' ************************************************
