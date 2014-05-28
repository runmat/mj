Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common
Imports System.Data.OleDb
Imports System.Data

Public Class Change01
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
    Private objHaendler As LT_01c

    Protected WithEvents lnkLogout As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents Literal1 As System.Web.UI.WebControls.Literal
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdContinue As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblExcelfile As System.Web.UI.WebControls.Label
    Protected WithEvents trcmdUpload As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trcmdSearch As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tblSelection As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents upFile As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents txtOrdernummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents cbxDatei As System.Web.UI.HtmlControls.HtmlInputCheckBox
    Protected WithEvents txtKunnrZL As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents txtKunnrZF As System.Web.UI.WebControls.TextBox
    Private versandart As String

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        If Not (Request.QueryString.Item("art") Is Nothing) Then
            versandart = Request.QueryString.Item("art").ToString
        Else
            lblError.Text = "Versandart nicht angegeben!"
            Exit Sub
        End If
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            If m_User.Organization.OrganizationAdmin Then
                lblHead.Text = Replace(lblHead.Text, " (Beauftragung)", "")
            End If
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If Not IsPostBack Then
                objHaendler = New LT_01c(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
              
                Session("objHaendler") = objHaendler

                Literal1.Text = "		<script language=""JavaScript"">" & vbCrLf
                Literal1.Text &= "			<!-- //" & vbCrLf
                Literal1.Text &= "			window.document.Form1.txtOrdernummer.focus();" & vbCrLf
                Literal1.Text &= "			//-->" & vbCrLf
                Literal1.Text &= "		</script>" & vbCrLf
            Else
                objHaendler = CType(Session("objHaendler"), LT_01c)
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub DoSubmit()
        Dim b As Boolean
        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
        Session.Add("logObj", logApp)

        lblError.Text = String.Empty

        b = True

        If txtOrdernummer.Text.Length = 0 Then
            objHaendler.SucheLvNr = String.Empty
        Else
            objHaendler.SucheLvNr = txtOrdernummer.Text & "%"
        End If

        If txtKunnrZL.Text.Length = 0 Then
            objHaendler.SucheKunnrZL = String.Empty

        Else
            objHaendler.SucheKunnrZL = txtKunnrZL.Text
        End If


        If txtKunnrZF.Text.Length = 0 Then
            objHaendler.SucheKunnrZF = String.Empty

        Else
            objHaendler.SucheKunnrZF = txtKunnrZF.Text
        End If


        Dim blnGo As Boolean

        blnGo = False
        If b Then
            objHaendler.Haendlernummer = m_User.Reference
            objHaendler.KUNNR = String.Empty
            objHaendler.GiveCars(Me) '******Daten holen******

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

            If blnGo Then
                Dim tblTemp As DataTable

                'Initialisieren
                Dim rowNew As DataRow
                Dim row As DataRow

                tblTemp = objHaendler.Fahrzeuge.Copy
                tblTemp.Clear()

                For Each row In objHaendler.Fahrzeuge.Rows

                    rowNew = tblTemp.NewRow
                    rowNew("EQUNR") = row("EQUNR")
                   
                    rowNew("LIZNR") = row("LIZNR")
                    rowNew("CHASSIS_NUM") = row("CHASSIS_NUM")
                    rowNew("TIDNR") = row("TIDNR")
                    rowNew("LICENSE_NUM") = row("LICENSE_NUM")
                    rowNew("REPLA_DATE") = row("REPLA_DATE")
                    rowNew("Leasingnehmer") = row("Leasingnehmer")
                    rowNew("Lieferant") = row("Lieferant")
                    

                    tblTemp.Rows.Add(rowNew)
                Next
                objHaendler.Fahrzeuge = tblTemp
                Session("objHaendler") = objHaendler
                
            End If
        End If
        If Not blnGo Then
            Exit Sub
        End If
        Response.Redirect("Change01_2.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart & "&file=" & cbxDatei.Checked.ToString)
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        If (txtKunnrZL.Text = String.Empty And txtOrdernummer.Text = String.Empty And txtKunnrZF.Text = String.Empty) Then
            lblError.Text = "Bitte geben Sie mindestens ein Suchkriterium an."
            Exit Sub
        End If
        DoSubmit()
    End Sub

    Private Sub cmdUpload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If tblSelection.Visible Then
        
            tblSelection.Visible = False
            trcmdSearch.Visible = False
            Literal1.Text = ""

        Else
          
            tblSelection.Visible = True
            trcmdSearch.Visible = True
            Literal1.Text = "		<script language=""JavaScript"">" & vbCrLf
            Literal1.Text &= "			<!-- //" & vbCrLf
            Literal1.Text &= "			window.document.Form1.txtOrdernummer.focus();" & vbCrLf
            Literal1.Text &= "			//-->" & vbCrLf
            Literal1.Text &= "		</script>" & vbCrLf

        End If
    End Sub

    Private Sub cmdContinue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdContinue.Click
        If (cbxDatei.Checked) Then
            'Dateiupload!
            'Prüfe Fehlerbedingung
            If (Not upFile.PostedFile Is Nothing) AndAlso (Not (upFile.PostedFile.FileName = String.Empty)) Then
                If Right(upFile.PostedFile.FileName.ToUpper, 4) <> ".XLS" Then
                    lblError.Text = "Es können nur Dateien im .XLS - Format verarbeitet werden."
                Else
                    'Lade Datei
                    upload(upFile.PostedFile)
                End If
            Else
                lblError.Text = "Keine Datei ausgewählt."
            End If
        End If
    End Sub

    Private Sub upload(ByVal uFile As System.Web.HttpPostedFile)
        Try
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
                 "Extended Properties=""Excel 8.0;HDR=NO;"""

                Dim objConn As New OleDbConnection(sConnectionString)
                objConn.Open()

                Dim objCmdSelect As New OleDbCommand("SELECT * FROM [Tabelle1$]", objConn)

                Dim objAdapter1 As New OleDbDataAdapter()
                objAdapter1.SelectCommand = objCmdSelect

                Dim objDataset1 As New DataSet()
                objAdapter1.Fill(objDataset1, "XLData")

                Dim rowData As DataRow

                Dim i As Integer = 0
                Dim tblTemp As New DataTable()

                Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
                logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
                Session.Add("logObj", logApp)

               For Each rowData In objDataset1.Tables(0).Rows
                    'In dieser Schleife wird    
                    '   a) Eine Einzelsuche pro Zeile im SAP durchgeführt (über LV-Nr.,...

                    i += 1
                    If TypeOf rowData(0) Is System.DBNull And TypeOf rowData(1) Is System.DBNull Then Exit For

                    Dim strLeasingvertragsNr As String = ""             'Spalte 1 : Leasingvertragsnummer
                    If Not TypeOf rowData(0) Is System.DBNull Then
                        strLeasingvertragsNr = CStr(rowData(0)).Trim(" "c) & "%"
                    End If
                    Dim strKennzeichen As String = ""
                    If Not TypeOf rowData(1) Is System.DBNull Then      'Spalte 2 : Kennzeichen
                        strKennzeichen = CStr(rowData(1)).Trim(" "c)
                    End If

                    If Not (strLeasingvertragsNr.Length = 0 Or strKennzeichen.Length = 0) Then
                        'Nur suchen, wenn beide Felder gefüllt...
                        'Suchfelder füllen.....
                        If strKennzeichen.Length > 0 Then
                            objHaendler.SucheKennz = Left(strKennzeichen, 9)           'Kennzeichen
                            objHaendler.SucheLvNr = Left(strLeasingvertragsNr, 10)      'LV-Nr.
                        End If

                        objHaendler.Haendlernummer = ""
                        objHaendler.KUNNR = ""

                        '.....und suchen
                        objHaendler.GiveCars(Me)

                        Dim rowNew As DataRow
                        If (tblTemp.Columns.Count) = 0 Then
                            'Initialisieren
                            tblTemp = objHaendler.giveResultStructure(Me)
                        End If
                        rowNew = tblTemp.NewRow

                        If Not objHaendler.Status = 0 Then                          'Fehler...
                            rowNew("EQUNR") = objHaendler.Equimpent
                            rowNew("MANDT") = "11"
                            rowNew("LIZNR") = objHaendler.SucheLvNr
                            rowNew("CHASSIS_NUM") = ""
                            rowNew("TIDNR") = ""
                            rowNew("LICENSE_NUM") = objHaendler.SucheKennz
                        Else
                            If objHaendler.Fahrzeuge.Rows.Count = 0 Then            'Kein Eintrag in SAP gefunden
                                rowNew("EQUNR") = objHaendler.Equimpent
                                rowNew("MANDT") = "11"
                                rowNew("LIZNR") = objHaendler.SucheLvNr
                                rowNew("CHASSIS_NUM") = ""
                                rowNew("TIDNR") = ""
                                rowNew("LICENSE_NUM") = objHaendler.SucheKennz
                            Else
                                'Treffer!
                                With objHaendler.Fahrzeuge
                                    rowNew("EQUNR") = .Rows(0)("EQUNR")
                                    If .Rows(0)("STATUS").ToString.IndexOf("*") >= 0 Then
                                        rowNew("MANDT") = "77"              'In Autorisierung
                                    Else
                                        rowNew("MANDT") = "99"              'Normal...
                                    End If
                                    rowNew("LIZNR") = .Rows(0)("LIZNR")
                                    rowNew("CHASSIS_NUM") = .Rows(0)("CHASSIS_NUM")
                                    rowNew("TIDNR") = .Rows(0)("TIDNR")
                                    rowNew("LICENSE_NUM") = .Rows(0)("LICENSE_NUM")
                                End With
                            End If
                        End If
                      
                        tblTemp.Rows.Add(rowNew)
                        'Ignore:
                    End If
                Next
                lblExcelfile.Text = objDataset1.Tables(0).Rows(0)(0).ToString
                objConn.Close()

                If Not tblTemp.Rows Is Nothing AndAlso tblTemp.Rows.Count > 0 Then
                    objHaendler.Fahrzeuge = tblTemp
                    Session("objHaendler") = objHaendler
                    Response.Redirect("Change01_2.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart & "&file=" & cbxDatei.Checked.ToString)
                Else
                    lblError.Text = "Datei enthielt keine verwendbaren Daten."
                End If
            End If
        Catch ex As Exception
            lblError.Text = ex.Message
        Finally
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
' $History: Change01.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 30.06.09   Time: 16:47
' Updated in $/CKAG/Applications/AppLeaseTrend/Forms
' ITA 2918 Z_M_Fehlende_Coc_Leasetr
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
' *****************  Version 10  *****************
' User: Uha          Date: 2.07.07    Time: 13:19
' Updated in $/CKG/Applications/AppLeaseTrend/AppLeaseTrendWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 9  *****************
' User: Uha          Date: 8.03.07    Time: 10:46
' Updated in $/CKG/Applications/AppLeaseTrend/AppLeaseTrendWeb/Forms
' 
' ************************************************
