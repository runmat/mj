Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports System.Data.OleDb
Imports System.Text

Public Class Change06
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
    Private objPDIs As SIXT_PDI

    Protected WithEvents lnkLogout As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents ImageButton1 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents trPDI As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents ucStyles As Styles
    Protected strTaskSelected As String = ""
    Protected WithEvents txtEinzel As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtMehrfach As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.LinkButton
    Protected WithEvents txtPDINummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents tdPDI As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents tdFahrg As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents Table7 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents rbEinzel As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rbMehrfach As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rbSuchePDI As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rbSucheFahrg As System.Web.UI.WebControls.RadioButton
    Protected WithEvents Table5 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents idWeiss As System.Web.UI.HtmlControls.HtmlInputRadioButton
    Protected WithEvents idGelb As System.Web.UI.HtmlControls.HtmlInputRadioButton
    Protected WithEvents idOrange As System.Web.UI.HtmlControls.HtmlInputRadioButton
    Protected WithEvents idRot As System.Web.UI.HtmlControls.HtmlInputRadioButton
    Protected WithEvents idViolett As System.Web.UI.HtmlControls.HtmlInputRadioButton
    Protected WithEvents idBlau As System.Web.UI.HtmlControls.HtmlInputRadioButton
    Protected WithEvents idGruen As System.Web.UI.HtmlControls.HtmlInputRadioButton
    Protected WithEvents idGrau As System.Web.UI.HtmlControls.HtmlInputRadioButton
    Protected WithEvents idBraun As System.Web.UI.HtmlControls.HtmlInputRadioButton
    Protected WithEvents idSchwarz As System.Web.UI.HtmlControls.HtmlInputRadioButton
    Protected WithEvents idAlle As System.Web.UI.HtmlControls.HtmlInputRadioButton
    Protected WithEvents chkZulassen As System.Web.UI.WebControls.RadioButton
    Protected WithEvents chkSperren As System.Web.UI.WebControls.RadioButton
    Protected WithEvents chkEntsperren As System.Web.UI.WebControls.RadioButton
    Protected WithEvents chkVerschieben As System.Web.UI.WebControls.RadioButton
    Protected submit As Boolean
    Private intSucheFarbe As Integer
    Protected WithEvents lblNoMatch As System.Web.UI.WebControls.Label
    Protected WithEvents Linkbutton1 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents Table4 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblHeader As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents trNoMatch As System.Web.UI.HtmlControls.HtmlTableRow
    Private strSucheFahrgestellnr As String

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cmdSearch.Visible = True
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        '######################################################################################
        '§§§JVE 07.04.2006
        'Wenn ein Redirect über Javascript erfolgt (window.location oder window.navigate) wird 
        'Request.UrlReferrer nicht gefüllt... Anscheinend ein IE-Problem, bei FireFox gehts.
        'vgl. http://www.mcse.ms/archive109-2005-7-1718415.html
        'Daher: Nur User + App. prüfen.
        If Not (Request.UrlReferrer Is Nothing) Then
            FormAuth(Me, m_User)
            'submit = False
        Else
            FormAuthNoReferrer(Me, m_User)
            'submit = True
        End If
        '#####################################################################################

        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            cmdSearch.Enabled = True
            ImageButton1.Enabled = True

            If Not IsPostBack Then
                rbSuchePDI.Checked = True
                rbEinzel.Checked = True
                rbSuchePDI.CssClass = "ButtonHighlight"
                chkZulassen.CssClass = "ButtonHighlight"
                rbEinzel.CssClass = "ButtonHighlight"
                chkZulassen.Checked = True
                idAlle.Checked = True
                intSucheFarbe = -1
                strSucheFahrgestellnr = String.Empty
                trNoMatch.Visible = False
            Else
                If (rbSuchePDI.Checked) Then
                    rbSuchePDI.CssClass = "ButtonHighlight"
                    rbSucheFahrg.CssClass = ""
                    strSucheFahrgestellnr = txtEinzel.Text
                    setColors()
                Else
                    rbSucheFahrg.CssClass = "ButtonHighlight"
                    rbSuchePDI.CssClass = ""
                    strSucheFahrgestellnr = txtEinzel.Text
                    setColors()
                End If
            End If

            rbSuchePDI.Attributes.Add("onClick", "switchInput(0)")
            rbSucheFahrg.Attributes.Add("onClick", "switchInput(1)")

            chkZulassen.Attributes.Add("onClick", "switchTask(0)")
            chkSperren.Attributes.Add("onClick", "switchTask(1)")
            chkEntsperren.Attributes.Add("onClick", "switchTask(2)")
            chkVerschieben.Attributes.Add("onClick", "switchTask(3)")

            rbEinzel.Attributes.Add("onClick", "setInputFocus(0)")
            rbMehrfach.Attributes.Add("onClick", "setInputFocus(1)")

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub setColors()
        idWeiss.Attributes.Remove("style")
        idGelb.Attributes.Remove("style")
        idOrange.Attributes.Remove("style")
        idRot.Attributes.Remove("style")
        idViolett.Attributes.Remove("style")
        idBlau.Attributes.Remove("style")
        idGruen.Attributes.Remove("style")
        idGrau.Attributes.Remove("style")
        idBraun.Attributes.Remove("style")
        idSchwarz.Attributes.Remove("style")

        If idWeiss.Checked Then
            idWeiss.Attributes.Add("style", "BACKGROUND:#FFFFFF")
            intSucheFarbe = 0
        End If
        If idGelb.Checked Then
            idGelb.Attributes.Add("style", "BACKGROUND:#FFFF00")
            intSucheFarbe = 1
        End If
        If idOrange.Checked Then
            idOrange.Attributes.Add("style", "BACKGROUND:#FF8800")
            intSucheFarbe = 2
        End If
        If idRot.Checked Then
            idRot.Attributes.Add("style", "BACKGROUND:#FF0000")
            intSucheFarbe = 3
        End If
        If idViolett.Checked Then
            idViolett.Attributes.Add("style", "BACKGROUND:#FF00FF")
            intSucheFarbe = 4
        End If
        If idBlau.Checked Then
            idBlau.Attributes.Add("style", "BACKGROUND:#0088FF")
            intSucheFarbe = 5
        End If
        If idGruen.Checked Then
            idGruen.Attributes.Add("style", "BACKGROUND:#008800")
            intSucheFarbe = 6
        End If
        If idGrau.Checked Then
            idGrau.Attributes.Add("style", "BACKGROUND:#888888")
            intSucheFarbe = 7
        End If
        If idBraun.Checked Then
            idBraun.Attributes.Add("style", "BACKGROUND:#804000")
            intSucheFarbe = 8
        End If
        If idSchwarz.Checked Then
            idSchwarz.Attributes.Add("style", "BACKGROUND:#000000")
            intSucheFarbe = 9
        End If
        If idAlle.Checked Then
            intSucheFarbe = -1
        End If

        chkZulassen.CssClass = ""
        chkSperren.CssClass = ""
        chkEntsperren.CssClass = ""
        chkVerschieben.CssClass = ""

        If chkZulassen.Checked Then
            chkZulassen.CssClass = "ButtonHighlight"
        End If
        If chkSperren.Checked Then
            chkSperren.CssClass = "ButtonHighlight"
        End If
        If chkEntsperren.Checked Then
            chkEntsperren.CssClass = "ButtonHighlight"
        End If
        If chkVerschieben.Checked Then
            chkVerschieben.CssClass = "ButtonHighlight"
        End If

        rbEinzel.CssClass = ""
        rbMehrfach.CssClass = ""

        If rbEinzel.Checked Then
            rbEinzel.CssClass = "ButtonHighlight"
        End If

        If rbMehrfach.Checked Then
            rbMehrfach.CssClass = "ButtonHighlight"
        End If
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        'Selektionskriterien
        DoSubmit()
    End Sub

    Private Sub upload(ByVal uFile As System.Web.HttpPostedFile)

        Dim filepath As String = ConfigurationManager.AppSettings("UploadpathLocal")
        Dim filename As String
        Dim info As System.IO.FileInfo
        Dim objCmdSelect As OleDbCommand
        Dim objAdapter1 As OleDbDataAdapter
        Dim objDataset1 As DataSet
        Dim sConnectionString As String
        Dim objConn As New OleDbConnection()
        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        Dim row As DataRow
        Dim rowFind As DataRow()
        Dim objNoMatch As New StringBuilder()

        logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
        Session.Add("logObj", logApp)

        Try
            'Dateiname: User_yyyyMMddhhmmss.xls
            filename = m_User.UserName & "_" & Format(Now, "yyyyMMddhhmmss") & ".xls"

            If Not (uFile Is Nothing) Then
                uFile.SaveAs(ConfigurationManager.AppSettings("UploadpathLocal") & filename)
                info = New System.IO.FileInfo(filepath & filename)
                If Not (info.Exists) Then
                    lblError.Text = "Fehler beim Speichern."
                    Exit Sub
                End If

                sConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & filepath & filename & ";Extended Properties=""Excel 8.0;HDR=NO;"""
                objConn.ConnectionString = sConnectionString
                'Datei gespeichert -> Auswertung
                objConn.Open()
                objCmdSelect = New OleDbCommand("SELECT * FROM [Tabelle1$]", objConn)  'Muß immer Tabelle 1 heißen!
                objAdapter1 = New OleDbDataAdapter()
                objAdapter1.SelectCommand = objCmdSelect
                objDataset1 = New DataSet()
                objAdapter1.Fill(objDataset1, "XLData")
                objPDIs.FahrgestellNr = New DataTable
                objPDIs.FahrgestellNr.Columns.Add("ZZFAHRG", GetType(System.String))
                For Each row In objDataset1.Tables(0).Rows
                    Dim NewRow As DataRow = objPDIs.FahrgestellNr.NewRow
                    NewRow("ZZFAHRG") = row(0).ToString
                    objPDIs.FahrgestellNr.Rows.Add(NewRow)
                Next
                objPDIs.PSucheFarbe = intSucheFarbe
                objPDIs.ShowPDIs(Session("AppID").ToString, Session.SessionID.ToString, Me)

                For Each row In objDataset1.Tables(0).Rows
                    rowFind = objPDIs.FahrzeugeGesamt.Select("ZZFAHRG='" & row(0) & "'")
                    If (rowFind.Length = 0) Then
                        objNoMatch.Append(row(0))
                        objNoMatch.Append("<br>")
                    End If
                Next
                If objNoMatch.Length > 0 Then
                    lblNoMatch.Text = objNoMatch.ToString
                End If
            End If
        Catch ex As Exception
            lblError.Text = ex.Message
        Finally
            objConn.Close()
        End Try
    End Sub

    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)

        logApp.WriteEntry("DBG", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, CStr(Session("AppID")), "PDI-Suche gestartet.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)

        If chkZulassen.Checked Then
            strTaskSelected = "Zulassen"
        End If
        If chkEntsperren.Checked Then
            strTaskSelected = "Entsperren"
        End If
        If chkSperren.Checked Then
            strTaskSelected = "Sperren"
        End If
        If chkVerschieben.Checked Then
            strTaskSelected = "Verschieben"
        End If

        If strTaskSelected.Length = 0 Then
            lblError.Text = "Bitte wählen Sie eine Aufgabe aus."
        Else
            objPDIs = New SIXT_PDI(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "", m_User.KUNNR, "ZDAD", txtPDINummer.Text.Trim(" "c), strTaskSelected)
            If (rbSuchePDI.Checked) Then    '### PDI-Suche
                objPDIs.PSucheFarbe = intSucheFarbe
                objPDIs.ShowPDIs(Session("AppID").ToString, Session.SessionID.ToString, Me)
            Else
                If (rbSucheFahrg.Checked) Then   '### Fahrgestellnummer-Suche
                    If rbEinzel.Checked = True Then
                        'Einzelsuche
                        If txtEinzel.Text.Trim <> String.Empty Then
                            objPDIs.PSucheFarbe = intSucheFarbe
                            objPDIs.FahrgestellNr = New DataTable
                            objPDIs.FahrgestellNr.Columns.Add("ZZFAHRG", GetType(System.String))

                            Dim NewRow As DataRow = objPDIs.FahrgestellNr.NewRow
                            NewRow("ZZFAHRG") = txtEinzel.Text
                            objPDIs.FahrgestellNr.Rows.Add(NewRow)

                            objPDIs.ShowPDIs(Session("AppID").ToString, Session.SessionID.ToString, Me)
                        Else
                            lblError.Text = "Keine Fahrgestellnummer angegeben."
                        End If
                    Else
                        'Mehrfachupload
                        If (Not txtMehrfach.PostedFile Is Nothing) AndAlso (Not (txtMehrfach.PostedFile.FileName = String.Empty)) Then
                            If Right(txtMehrfach.PostedFile.FileName.ToUpper, 4) <> ".XLS" Then
                                lblError.Text = "Es können nur Dateien im .XLS - Format verarbeitet werden."
                            Else
                                'Lade Datei
                                upload(txtMehrfach.PostedFile)
                            End If
                        Else
                            lblError.Text = "Keine Datei ausgewählt."
                        End If
                    End If
                End If
            End If

            Session("objPDIs") = objPDIs
            'Weiter zur Auswahlseite....
            If rbSuchePDI.Checked = True Then
                If objPDIs.Status = 0 Then
                    If objPDIs.PDI_Data.Tables("PDIs").Rows.Count = 0 Then
                        lblError.Text = "Keine Daten zur Anzeige gefunden."
                    Else
                        logApp.WriteEntry("DBG", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, CStr(Session("AppID")), "PDI-Suche beendet.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
                        Response.Redirect("Change06_2.aspx?AppID=" & Session("AppID").ToString)
                    End If
                Else
                    logApp.WriteEntry("ERR", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, CStr(Session("AppID")), "Es konnten keine Daten ermittelt werden. (Fehler: " & objPDIs.Message & " )", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
                    lblError.Text = "Es konnten keine Daten ermittelt werden."
                End If
            Else
                If (lblNoMatch.Text <> String.Empty) Then
                    logApp.WriteEntry("DBG", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, CStr(Session("AppID")), "PDI-Suche beendet.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
                    trNoMatch.Visible = True
                Else
                    logApp.WriteEntry("DBG", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, CStr(Session("AppID")), "PDI-Suche beendet.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
                    Response.Redirect("Change06_2.aspx?AppID=" & Session("AppID").ToString)
                End If
            End If
        End If
    End Sub

    Private Sub Linkbutton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Linkbutton1.Click
        Response.Redirect("Change06_2.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Change06.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 16.02.10   Time: 11:40
' Updated in $/CKAG/Applications/appsixt/Forms
' ITA: 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 4.05.09    Time: 10:16
' Updated in $/CKAG/Applications/appsixt/Forms
' ITA:2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 13:37
' Created in $/CKAG/Applications/appsixt/Forms
' 
' *****************  Version 13  *****************
' User: Uha          Date: 2.07.07    Time: 14:09
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 12  *****************
' User: Uha          Date: 22.05.07   Time: 11:27
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Forms
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 11  *****************
' User: Uha          Date: 8.03.07    Time: 13:15
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Forms
' 
' ************************************************
