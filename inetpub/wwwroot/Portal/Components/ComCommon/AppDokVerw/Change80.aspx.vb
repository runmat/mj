Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common
Imports System.Data.OleDb
Imports System.Data

Namespace AppDokVerw

    Public Class Change80
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
        'Private objSuche As DealerSearch.Search
        Private objHaendler As AppDokVerw_05

        Protected WithEvents Hyperlink1 As System.Web.UI.WebControls.HyperLink
        Protected WithEvents lnkLogout As System.Web.UI.WebControls.HyperLink
        Protected WithEvents lblError As System.Web.UI.WebControls.Label
        Protected WithEvents cmdSearch As System.Web.UI.WebControls.LinkButton
        Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
        Protected WithEvents ucHeader As Header
        Protected WithEvents ucStyles As Styles
        Protected WithEvents lblHead As System.Web.UI.WebControls.Label
        Protected WithEvents txtAmtlKennzeichen As System.Web.UI.WebControls.TextBox
        Protected WithEvents tblSelection As System.Web.UI.HtmlControls.HtmlTable
        Protected WithEvents tblUpload As System.Web.UI.HtmlControls.HtmlTable
        Protected WithEvents cmdUpload As System.Web.UI.WebControls.LinkButton
        Protected WithEvents trcmdUpload As System.Web.UI.HtmlControls.HtmlTableRow
        Protected WithEvents trcmdSearch As System.Web.UI.HtmlControls.HtmlTableRow
        Protected WithEvents upFile As System.Web.UI.HtmlControls.HtmlInputFile
        Protected WithEvents lblExcelfile As System.Web.UI.WebControls.Label
        Protected WithEvents cmdContinue As System.Web.UI.WebControls.LinkButton
        Protected WithEvents trcmdContinue As System.Web.UI.HtmlControls.HtmlTableRow
        Protected WithEvents Literal1 As System.Web.UI.WebControls.Literal
        Protected WithEvents cbxPlatzhaltersuche As System.Web.UI.WebControls.RadioButton
        Protected WithEvents cbxOhnePlatzhalter As System.Web.UI.WebControls.RadioButton
        Private versandart As String
        Protected WithEvents txtOrdernummer As System.Web.UI.WebControls.TextBox
        Protected WithEvents txtNummerZB2 As System.Web.UI.WebControls.TextBox
        Protected WithEvents txtFahrgestellnummer As System.Web.UI.WebControls.TextBox
        Protected WithEvents lbl_Leasingvertragsnummer As System.Web.UI.WebControls.Label
        Protected WithEvents lbl_Kennzeichen As System.Web.UI.WebControls.Label
        Protected WithEvents lbl_NummerZB2 As System.Web.UI.WebControls.Label
        Protected WithEvents lbl_Fahrgestellnummer As System.Web.UI.WebControls.Label
        Protected WithEvents lbl_Platzhaltersuche As System.Web.UI.WebControls.Label
        Protected WithEvents tr_Leasingvertragsnummer As System.Web.UI.HtmlControls.HtmlTableRow
        Protected WithEvents tr_Kennzeichen As System.Web.UI.HtmlControls.HtmlTableRow
        Protected WithEvents tr_KennzeichenZusatz As System.Web.UI.HtmlControls.HtmlTableRow
        Protected WithEvents tr_NummerZB2 As System.Web.UI.HtmlControls.HtmlTableRow
        Protected WithEvents tr_Fahrgestellnummer As System.Web.UI.HtmlControls.HtmlTableRow
        Protected WithEvents tr_FahrgestellnummerZusatz As System.Web.UI.HtmlControls.HtmlTableRow
        Protected WithEvents tr_Platzhaltersuche As System.Web.UI.HtmlControls.HtmlTableRow
        'Private authentifizierung As String

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            cmdSearch.Visible = True
            m_User = GetUser(Me)
            ucHeader.InitUser(m_User)
            FormAuth(Me, m_User)
            GetAppIDFromQueryString(Me)

            versandart = Request.QueryString.Item("art").ToString
            'authentifizierung = Request.QueryString.Item("art2").ToString

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            If m_User.Organization.OrganizationAdmin Then
                lblHead.Text = Replace(lblHead.Text, " (Beauftragung)", "")
            End If
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If (Session("objHaendler") Is Nothing) OrElse (Not IsPostBack) Then
                objHaendler = New AppDokVerw_05(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
            Else
                objHaendler = CType(Session("objHaendler"), AppDokVerw_05)
            End If

            Session("objHaendler") = objHaendler
            If Not IsPostBack Then
                tblSelection.Visible = True
                trcmdSearch.Visible = True
                tblUpload.Visible = False
                trcmdUpload.Visible = True
                trcmdContinue.Visible = False
                Literal1.Text = "		<script language=""JavaScript"">" & vbCrLf
                Literal1.Text &= "			<!-- //" & vbCrLf
                Literal1.Text &= "			window.document.Form1.txtOrdernummer.focus();" & vbCrLf
                Literal1.Text &= "			//-->" & vbCrLf
                Literal1.Text &= "		</script>" & vbCrLf
            End If

        End Sub

        Private Sub DoSubmit()
            Dim b As Boolean
            Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
            logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
            Session.Add("logObj", logApp)

            lblError.Text = ""

            b = True

            'Keine Platzhaltersuche -> Werfe Platzhalter 'raus
            If Not cbxPlatzhaltersuche.Checked Then
                txtOrdernummer.Text = Replace(txtOrdernummer.Text, "*", "")
                txtOrdernummer.Text = Replace(txtOrdernummer.Text, "%", "")

                txtAmtlKennzeichen.Text = Replace(txtAmtlKennzeichen.Text, "*", "")
                txtAmtlKennzeichen.Text = Replace(txtAmtlKennzeichen.Text, "%", "")

                txtFahrgestellnummer.Text = Replace(txtFahrgestellnummer.Text, "*", "")
                txtFahrgestellnummer.Text = Replace(txtFahrgestellnummer.Text, "%", "")
            End If

            'Briefnummer generell ohne Platzhalter
            txtNummerZB2.Text = Replace(txtNummerZB2.Text, "*", "")
            txtNummerZB2.Text = Replace(txtNummerZB2.Text, "%", "")


            If txtNummerZB2.Text.Length = 0 Then
                objHaendler.SucheNummerZB2 = ""
            Else
                objHaendler.SucheNummerZB2 = txtNummerZB2.Text.Replace(" ", "")
            End If


            If txtOrdernummer.Text.Length = 0 Then
                objHaendler.SucheLeasingvertragsNr = ""
            Else
                objHaendler.SucheLeasingvertragsNr = txtOrdernummer.Text.Replace(" ", "")
            End If

            txtFahrgestellnummer.Text = Replace(txtFahrgestellnummer.Text.Trim(" "c).Trim("*"c), " ", "")
            If txtFahrgestellnummer.Text.Length = 0 Then
                objHaendler.SucheFahrgestellNr = ""
            Else
                objHaendler.SucheFahrgestellNr = txtFahrgestellnummer.Text
                If objHaendler.SucheFahrgestellNr.Length < 17 Then
                    If objHaendler.SucheFahrgestellNr.Length > 7 Then
                        txtFahrgestellnummer.Text = "*" & objHaendler.SucheFahrgestellNr
                        objHaendler.SucheFahrgestellNr = "%" & objHaendler.SucheFahrgestellNr
                    Else
                        lblError.Text = "Bitte geben Sie die Fahrgestellnummer mindestens 8-stellig ein."
                        b = False
                    End If
                End If
            End If

            If txtAmtlKennzeichen.Text.Length = 0 Then
                objHaendler.SucheKennzeichen = ""
            Else
                txtAmtlKennzeichen.Text = Replace(txtAmtlKennzeichen.Text.Trim(" "c), " ", "")
                objHaendler.SucheKennzeichen = txtAmtlKennzeichen.Text
                'prüfe auf Eingabeformat Kreis und ein Buchstabe JJU2008.04.07
                Dim tmpaKennzeichen As String()
                tmpaKennzeichen = txtAmtlKennzeichen.Text.Split(",")
                Dim tmpStr As String
                Dim tmpStr2 As String
                For Each tmpStr2 In tmpaKennzeichen
                    tmpStr = tmpStr2.Replace("*", "")
                    If Not tmpStr.IndexOf("-") = -1 Then
                        If Not tmpStr.Length > tmpStr.IndexOf("-") + 1 OrElse tmpStr.IndexOf("-") = 0 OrElse tmpStr.Length < 3 Then
                            lblError.Text = "Bitte beachten Sie das Eingabeformat für Kennzeichen."
                            b = False
                            Exit For
                        End If
                    Else
                        lblError.Text = "Bitte beachten Sie das Eingabeformat für Kennzeichen."
                        b = False
                        Exit For
                    End If

                Next
            End If

            If b Then
                objHaendler.Haendlernummer = m_User.Reference
                'objHaendler.KUNNR = ""

                objHaendler.GiveCars()
                Dim blnGo As Boolean = False
                If Not objHaendler.Status = 0 Then
                    lblError.Text = objHaendler.Message
                    lblError.Visible = True
                Else
                    If objHaendler.Result.Rows.Count = 0 Then
                        lblError.Text = "Zu den gewählten Kriterien wurden keine Ergebnisse gefunden."
                    Else
                        blnGo = True
                    End If
                End If

                'SFA 10.11.2006 Wenn Kundengruppe gesetzt, dann nicht in die Historie abfragen
                If objHaendler.IsBooCustomerGroup = False Then


                    If Not blnGo Then
                        If Not cbxPlatzhaltersuche.Checked Then
                            Dim tblTemp As DataTable = objHaendler.GiveResultStructure
                            Dim rowNew As DataRow
                            rowNew = tblTemp.NewRow
                            rowNew("MANDT") = "11"
                            rowNew("LIZNR") = objHaendler.SucheLeasingvertragsNr
                            rowNew("CHASSIS_NUM") = ""
                            rowNew("TIDNR") = ""
                            rowNew("LICENSE_NUM") = objHaendler.SucheKennzeichen
                            rowNew("ZZREFERENZ1") = ""
                            rowNew("ZZCOCKZ") = ""
                            rowNew("STATUS") = "Keine Daten gefunden."
                            Dim m_Report As New Hist(m_User, m_App, "")
                            m_Report.FillHistory(Session("AppID").ToString, Session.SessionID.ToString, objHaendler.SucheKennzeichen, objHaendler.SucheFahrgestellNr, "", objHaendler.SucheLeasingvertragsNr)
                            If (Not m_Report.History Is Nothing) AndAlso (m_Report.History.Rows.Count = 1) AndAlso (Not m_Report.History.Rows(0)("ZZFAHRG").ToString = String.Empty) Then

                                'Fahrgestellnummer
                                rowNew("CHASSIS_NUM") = m_Report.History.Rows(0)("ZZFAHRG").ToString

                                'Auftragsnummer
                                rowNew("LIZNR") = m_Report.History.Rows(0)("ZZREF1").ToString

                                'Kfz-Briefnummer
                                rowNew("TIDNR") = m_Report.History.Rows(0)("ZZBRIEF").ToString

                                'Kfz-Kennzeichen
                                rowNew("LICENSE_NUM") = m_Report.History.Rows(0)("ZZKENN").ToString

                                'Equipmentnummer
                                rowNew("EQUNR") = m_Report.History.Rows(0)("EQUNR").ToString

                                'Switch für Änderung Versandart von temporär zu endgültig
                                rowNew("SWITCH") = False

                                If m_Report.History.Rows(0)("ZZSTATUS_ABG").ToString = "X" Then
                                    rowNew("STATUS") = "Abgemeldet"
                                ElseIf m_Report.History.Rows(0)("ZZSTATUS_BAG").ToString = "X" Then
                                    rowNew("STATUS") = "In Abmeldung"
                                ElseIf m_Report.History.Rows(0)("ABCKZ").ToString = "1" Then
                                    rowNew("STATUS") = "Temporär versendet"
                                ElseIf m_Report.History.Rows(0)("ABCKZ").ToString = "2" Then
                                    rowNew("STATUS") = "Endgültig versendet"
                                Else
                                    If TypeOf m_Report.History.Rows(0)("EQUNR") Is String Then
                                        If objHaendler.CheckAgainstAuthorizationTable(m_Report.History.Rows(0)("EQUNR").ToString) Then
                                            rowNew("STATUS") = "Zur Freigabe"
                                        Else
                                            rowNew("STATUS") = "Bitte in Historie prüfen!"
                                        End If
                                    Else
                                        rowNew("STATUS") = "Bitte in Historie prüfen!"
                                    End If
                                End If
                                blnGo = True
                            End If
                            tblTemp.Rows.Add(rowNew)
                            If blnGo Then
                                objHaendler.Fahrzeuge = tblTemp
                            End If
                        End If
                    End If
                End If
                If blnGo Then
                    Session("objHaendler") = objHaendler
                    'Response.Redirect("Change80_2.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart & "&art2=" & authentifizierung)
                    Response.Redirect("Change80_2.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart)
                End If
            End If
        End Sub

        Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
            If Not txtAmtlKennzeichen.Text = String.Empty Then
                txtAmtlKennzeichen.Text = Replace(txtAmtlKennzeichen.Text, " ", "").Trim(","c)
                If txtAmtlKennzeichen.Text.Length = 0 Then
                    txtAmtlKennzeichen.Text = String.Empty
                End If
            End If
            If (txtAmtlKennzeichen.Text = String.Empty And txtOrdernummer.Text = String.Empty And txtFahrgestellnummer.Text = String.Empty And txtNummerZB2.Text = String.Empty) Then
                lblError.Text = "Bitte geben Sie mindestens ein Suchkriterium an."
                Exit Sub
            End If
            DoSubmit()
        End Sub

        Private Sub cmdUpload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpload.Click
            If tblSelection.Visible Then
                tblUpload.Visible = True
                trcmdContinue.Visible = True
                tblSelection.Visible = False
                trcmdSearch.Visible = False
                Literal1.Text = ""
                cmdUpload.Text = " &#149;&nbsp;Einzelauswahl"
            Else
                tblUpload.Visible = False
                trcmdContinue.Visible = False
                tblSelection.Visible = True
                trcmdSearch.Visible = True
                Literal1.Text = "		<script language=""JavaScript"">" & vbCrLf
                Literal1.Text &= "			<!-- //" & vbCrLf
                Literal1.Text &= "			window.document.Form1.txtOrdernummer.focus();" & vbCrLf
                Literal1.Text &= "			//-->" & vbCrLf
                Literal1.Text &= "		</script>" & vbCrLf
                cmdUpload.Text = " &#149;&nbsp;Mehrfachauswahl"
            End If
        End Sub

        Private Sub cmdContinue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdContinue.Click
            'Prüfe Fehlerbedingung
            If (Not upFile.PostedFile Is Nothing) AndAlso (Not (upFile.PostedFile.FileName = String.Empty)) Then
                lblExcelfile.Text = upFile.PostedFile.FileName
                If Right(upFile.PostedFile.FileName.ToUpper, 4) <> ".XLS" Then
                    lblError.Text = "Es können nur Dateien im .XLS - Format verarbeitet werden."
                    Exit Sub
                End If
            Else
                lblError.Text = "Keine Datei ausgewählt"
                Exit Sub
            End If

            'Lade Datei
            upload(upFile.PostedFile)
        End Sub

        Private Sub upload(ByVal uFile As System.Web.HttpPostedFile)
            Dim filepath As String = ConfigurationManager.AppSettings("UploadpathLocal")
            Dim filename As String
            Dim info As System.IO.FileInfo

            'Dateiname: User_yyyyMMddhhmmss.xls
            filename = m_User.UserName & "_" & Format(Now, "yyyyMMddhhmmss") & ".xls"

            If Not (uFile Is Nothing) Then
                uFile.SaveAs(ConfigurationManager.AppSettings("UploadpathLocal") & filename)
                info = New System.IO.FileInfo(filepath & filename)
                If Not (info.Exists) Then
                    lblError.Text = "Fehler beim Speichern."
                    Exit Sub
                End If

                'Datei gespeichert -> Auswertung
                Dim sConnectionString As String = "Provider=Microsoft.Jet.OLEDB.4.0;" & _
                 "Data Source=" & filepath & filename & ";" & _
                 "Extended Properties=""Excel 8.0;HDR=YES;"""

                Dim objConn As New OleDbConnection(sConnectionString)
                objConn.Open()

                Dim objCmdSelect As New OleDbCommand("SELECT * FROM [Tabelle1$]", objConn)

                Dim objAdapter1 As New OleDbDataAdapter()
                objAdapter1.SelectCommand = objCmdSelect

                Dim objDataset1 As New DataSet()
                objAdapter1.Fill(objDataset1, "XLData")

                Dim tblTemp As DataTable = CheckInputTable(objDataset1.Tables(0))

                objConn.Close()

                If Not tblTemp.Rows Is Nothing AndAlso tblTemp.Rows.Count > 0 Then
                    objHaendler.Fahrzeuge = tblTemp
                    Session("objHaendler") = objHaendler
                    'Response.Redirect("Change80_2.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart & "&art2=" & authentifizierung)
                    Response.Redirect("Change80_2.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart)
                Else
                    lblError.Text = "Datei enthielt keine verwendbaren Daten."
                End If
            End If

        End Sub

        Private Function CheckInputTable(ByVal tblInput As DataTable) As DataTable
            Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
            logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
            Session.Add("logObj", logApp)

            Dim i As Integer = 0
            Dim rowData As DataRow
            Dim tblReturn As DataTable = Nothing

            For Each rowData In tblInput.Rows
                i += 1
                If TypeOf rowData(0) Is System.DBNull And TypeOf rowData(1) Is System.DBNull Then Exit For

                Dim strLeasingvertragsNr As String = ""
                If Not TypeOf rowData(0) Is System.DBNull Then
                    strLeasingvertragsNr = CStr(rowData(0)).Trim(" "c)
                End If
                Dim strKennzeichen As String = ""
                If Not TypeOf rowData(1) Is System.DBNull Then
                    strKennzeichen = CStr(rowData(1)).Trim(" "c)
                End If

                If strLeasingvertragsNr.Length = 0 And strKennzeichen.Length = 0 Then Exit For

                If tblReturn Is Nothing Then
                    tblReturn = objHaendler.GiveResultStructure
                End If

                If strKennzeichen.Length > 0 Then
                    objHaendler.SucheLeasingvertragsNr = ""
                    objHaendler.SucheKennzeichen = strKennzeichen
                Else
                    'If Not IsNumeric(strLeasingvertragsNr) Then GoTo Ignore
                    objHaendler.SucheLeasingvertragsNr = Left(strLeasingvertragsNr, 7)
                    objHaendler.SucheKennzeichen = ""
                End If

                objHaendler.SucheFahrgestellNr = ""
                objHaendler.Haendlernummer = ""
                'objHaendler.KUNNR = ""

                objHaendler.GiveCars()
                Dim ColumnCounter As Integer = tblReturn.Columns.Count - 1
                Dim j As Integer
                Dim rowNew As DataRow
                rowNew = tblReturn.NewRow
                If Not objHaendler.Status = 0 Then
                    rowNew("MANDT") = "11"
                    rowNew("LIZNR") = objHaendler.SucheLeasingvertragsNr
                    rowNew("CHASSIS_NUM") = ""
                    rowNew("TIDNR") = ""
                    rowNew("LICENSE_NUM") = objHaendler.SucheKennzeichen
                    rowNew("ZZREFERENZ1") = ""
                    rowNew("ZZCOCKZ") = ""
                    rowNew("SWITCH") = False        '§§§ JVE 16.10.2006
                    rowNew("STATUS") = objHaendler.Message
                    rowNew("EXPIRY_DATE") = String.Empty
                Else
                    If objHaendler.Fahrzeuge.Rows.Count = 0 Then
                        rowNew("MANDT") = "11"
                        rowNew("LIZNR") = objHaendler.SucheLeasingvertragsNr
                        rowNew("CHASSIS_NUM") = ""
                        rowNew("TIDNR") = ""
                        rowNew("LICENSE_NUM") = objHaendler.SucheKennzeichen
                        rowNew("ZZREFERENZ1") = ""
                        rowNew("ZZCOCKZ") = ""
                        rowNew("STATUS") = "Keine Daten gefunden."
                    Else
                        For j = 0 To ColumnCounter
                            rowNew(j) = objHaendler.Fahrzeuge.Rows(0)(j)
                        Next
                        rowNew("MANDT") = "99"
                    End If
                End If
                If Not CStr(rowNew("MANDT")) = "99" Then
                    Dim m_Report As New Hist(m_User, m_App, "")
                    m_Report.FillHistory(Session("AppID").ToString, Session.SessionID.ToString, objHaendler.SucheKennzeichen, "", "", objHaendler.SucheLeasingvertragsNr)
                    If (Not m_Report.History Is Nothing) AndAlso (m_Report.History.Rows.Count = 1) AndAlso (Not m_Report.History.Rows(0)("ZZFAHRG").ToString = String.Empty) Then

                        'Fahrgestellnummer
                        rowNew("CHASSIS_NUM") = m_Report.History.Rows(0)("ZZFAHRG").ToString

                        'Auftragsnummer
                        rowNew("LIZNR") = m_Report.History.Rows(0)("ZZREF1").ToString

                        'Kfz-Briefnummer
                        rowNew("TIDNR") = m_Report.History.Rows(0)("ZZBRIEF").ToString

                        'Kfz-Kennzeichen
                        rowNew("LICENSE_NUM") = m_Report.History.Rows(0)("ZZKENN").ToString

                        'Equipmentnummer
                        rowNew("EQUNR") = m_Report.History.Rows(0)("EQUNR").ToString

                        'Switch für Änderung Versandart von temporär zu endgültig
                        rowNew("SWITCH") = False

                        If m_Report.History.Rows(0)("ZZSTATUS_ABG").ToString = "X" Then
                            rowNew("STATUS") = "Abgemeldet"
                        ElseIf m_Report.History.Rows(0)("ZZSTATUS_BAG").ToString = "X" Then
                            rowNew("STATUS") = "In Abmeldung"
                        ElseIf m_Report.History.Rows(0)("ABCKZ").ToString = "1" Then
                            rowNew("STATUS") = "Temporär versendet"
                        ElseIf m_Report.History.Rows(0)("ABCKZ").ToString = "2" Then
                            rowNew("STATUS") = "Endgültig versendet"
                        Else
                            If TypeOf m_Report.History.Rows(0)("EQUNR") Is String Then
                                If objHaendler.CheckAgainstAuthorizationTable(m_Report.History.Rows(0)("EQUNR").ToString) Then
                                    rowNew("STATUS") = "Zur Freigabe"
                                Else
                                    rowNew("STATUS") = "Bitte in Historie prüfen!"
                                End If
                            Else
                                rowNew("STATUS") = "Bitte in Historie prüfen!"
                            End If
                        End If
                    End If
                End If
                tblReturn.Rows.Add(rowNew)
Ignore:
            Next
            'lblExcelfile.Text = objDataset1.Tables(0).Rows(0)(0).ToString

            Return tblReturn
        End Function

        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
            SetEndASPXAccess(Me)
        End Sub

        Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
            SetEndASPXAccess(Me)
        End Sub
    End Class
End Namespace

' ************************************************
' $History: Change80.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 5.06.09    Time: 13:51
' Updated in $/CKAG/Components/ComCommon/AppDokVerw
' Try Catch entfernt wenn möglich
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 4.05.09    Time: 11:50
' Updated in $/CKAG/Components/ComCommon/AppDokVerw
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 14.10.08   Time: 15:40
' Created in $/CKAG/Components/ComCommon/AppDokVerw
' 
