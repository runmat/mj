Option Infer On

Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common
Imports CKG.Base.Kernel.Security
Imports System.Data.OleDb
Imports System.Data

Namespace Zulassung
    Partial Public Class Change01
        Inherits Page
#Region "Declarations"
        Private m_User As User
        Private m_App As App
        Private objZulassung As Zulassung1
        Private versandart As String
        Private authentifizierung As String
        Private booError As Boolean
#End Region

#Region "Overrides"
        Protected Overrides Sub OnLoad(e As System.EventArgs)
            MyBase.OnLoad(e)

            m_User = Common.GetUser(Me)
            Common.FormAuth(Me, m_User)
            Common.GetAppIDFromQueryString(Me)
            m_App = New Base.Kernel.Security.App(m_User)

            Dim appID = Session("AppID").ToString

            lblHead.Text = m_User.Applications.Select("AppID = '" & appID & "'")(0)("AppFriendlyName").ToString

            If Not IsPostBack Then Session.Remove("objZulassung")
            objZulassung = Common.GetOrCreateObject("objZulassung", Function() New Zulassung1(m_User, m_App, ""))

            'Für den Upload
            Me.Form.Enctype = "multipart/form-data"
            CheckAuswahl()
            objZulassung.GetLaender()
        End Sub

        Protected Overrides Sub OnPreRender(e As System.EventArgs)
            MyBase.OnPreRender(e)

            Common.SetEndASPXAccess(Me)
        End Sub

        Protected Overrides Sub OnUnload(e As System.EventArgs)
            MyBase.OnUnload(e)

            Common.SetEndASPXAccess(Me)
        End Sub
#End Region

#Region "Events"
        Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
            lblerror.Text = ""
            Try

                If rb_Upload.Checked = True Then

                    'Prüfe Fehlerbedingung
                    If (Not upFile1.PostedFile Is Nothing) AndAlso (Not (upFile1.PostedFile.FileName = String.Empty)) Then
                        'lblExcelfile.Text = upFile1.PostedFile.FileName
                        If Right(upFile1.PostedFile.FileName.ToUpper, 4) <> ".XLS" Then
                            lblerror.Text = "Es können nur Dateien im .XLS - Format verarbeitet werden."
                            Exit Sub
                        End If
                    Else
                        lblerror.Text = "Keine Datei ausgewählt"
                        Exit Sub
                    End If

                    booError = False

                    'Lade Datei
                    'upload(upFile1.PostedFile)
                    Dim uploadV As Upload_Validator.Validator = New Upload_Validator.Validator()

                    'CHC ITA 5972
                    lblerror.Text = ""
                    Check_Data(uploadV.UploadFahrgestellnummern(upFile1.PostedFile, ConfigurationManager.AppSettings("ExcelPath"), m_User, lblerror, Session("AppID").ToString, Session.SessionID))



                    If booError = True Then Exit Sub



                Else

                    If (txtFahrgestellnummer.Text = String.Empty And txtNummerZB2.Text = String.Empty _
                        And txtVertragsnummer.Text = String.Empty) Then
                        txtFahrgestellnummer.Text = "*"
                        'lblerror.Text = "Bitte geben Sie mindestens ein Suchkriterium an."
                        'Exit Sub
                    Else

                        If txtFahrgestellnummer.Text.Contains("*") Then
                            If txtFahrgestellnummer.Text.IndexOf("*"c) <> txtFahrgestellnummer.Text.LastIndexOf("*") Then
                                lblerror.Text = "Für die Suche über die Fahrgestellnummer ist nur ein vorangestellter Platzhalter(*) vorgesehen."
                                Exit Sub
                            End If
                        End If



                    End If

                End If
                If lblerror.Text = "" Then
                    DoSubmit()
                End If
            Catch ex As Exception
                'lblerror.Text = Err.Description
            End Try

        End Sub

        Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
            Response.Redirect("/Services/(S(" & Session.SessionID & "))/Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
        End Sub

        Protected Sub rb_Upload_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rb_Upload.CheckedChanged
            CheckAuswahl()
        End Sub

        Protected Sub rb_Einzelauswahl_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rb_Einzelauswahl.CheckedChanged
            CheckAuswahl()
        End Sub

#End Region

#Region "Methods"
        Private Sub DoSubmit()
            Dim b As Boolean
            Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
            logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
            Session.Add("logObj", logApp)

            'lblerror.Text = ""

            b = True

            txtFahrgestellnummer.Text = Replace(txtFahrgestellnummer.Text, "*", "")
            txtFahrgestellnummer.Text = Replace(txtFahrgestellnummer.Text, "%", "")

            If txtVertragsnummer.Text.Length = 0 Then
                objZulassung.Vertragsnummer = ""
            Else
                objZulassung.Vertragsnummer = txtVertragsnummer.Text.Replace(" ", "")
            End If

            If txtNummerZB2.Text.Length = 0 Then
                objZulassung.NummerZBII = ""
            Else
                objZulassung.NummerZBII = txtNummerZB2.Text.Replace(" ", "")
            End If

            txtFahrgestellnummer.Text = Replace(txtFahrgestellnummer.Text.Trim(" "c).Trim("*"c), " ", "")
            If txtFahrgestellnummer.Text.Length = 0 Then
                objZulassung.Fahrgestellnr = ""
            Else
                objZulassung.Fahrgestellnr = txtFahrgestellnummer.Text
                If objZulassung.Fahrgestellnr.Length < 17 Then
                    If objZulassung.Fahrgestellnr.Length > 4 Then
                        txtFahrgestellnummer.Text = "*" & objZulassung.Fahrgestellnr
                        objZulassung.Fahrgestellnr = "*" & objZulassung.Fahrgestellnr
                    Else
                        lblerror.Text = "Bitte geben Sie die Fahrgestellnummer mindestens 5-stellig ein."
                        b = False
                    End If
                End If
            End If

            If b Then
                objZulassung.Show(Session("AppID").ToString, Session.SessionID, Me)
                Dim blnGo As Boolean = False
                If Not objZulassung.Status = 0 Then
                    lblerror.Text = objZulassung.Message
                    lblerror.Visible = True
                Else
                    If objZulassung.Fahrzeuge.Rows.Count = 0 Then
                        lblerror.Text = "Zu den gewählten Kriterien wurden keine Ergebnisse gefunden."
                    Else
                        blnGo = True
                    End If
                End If
                If blnGo Then
                    Session("objZulassung") = objZulassung
                    Response.Redirect("Change01_2.aspx?AppID=" & Session("AppID").ToString)
                End If
            End If
        End Sub

        'Private Sub upload(ByVal uFile As System.Web.HttpPostedFile)
        '    'Try
        '    Dim filepath As String = ConfigurationManager.AppSettings("ExcelPath")
        '    Dim filename As String
        '    Dim info As System.IO.FileInfo

        '    'Dateiname: User_yyyyMMddhhmmss.xls
        '    filename = m_User.UserName & "_" & Format(Now, "yyyyMMddhhmmss") & ".xls"

        '    If Not (uFile Is Nothing) Then
        '        uFile.SaveAs(ConfigurationManager.AppSettings("ExcelPath") & filename)
        '        info = New System.IO.FileInfo(filepath & filename)
        '        If Not (info.Exists) Then
        '            lblerror.Text = "Fehler beim Speichern."
        '            Exit Sub
        '        End If

        '        'Datei gespeichert -> Auswertung
        '        Dim sConnectionString As String = "Provider=Microsoft.Jet.OLEDB.4.0;" & _
        '         "Data Source=" & filepath & filename & ";" & _
        '         "Extended Properties=""Excel 8.0;HDR=YES;"""

        '        Dim objConn As New OleDbConnection(sConnectionString)
        '        objConn.Open()

        '        Dim Tabellenname() As String
        '        Dim i As Integer = 0
        '        Do While i < objConn.GetSchema("Tables").Rows.Count
        '            ReDim Preserve Tabellenname(i)
        '            Tabellenname(i) = objConn.GetSchema("Tables").Rows(i)("TABLE_NAME")
        '            i = i + 1
        '        Loop


        '        Dim objAdapter1 As New OleDbDataAdapter()
        '        Dim objDataset1 As New DataSet()
        '        Dim tblTemp As DataTable



        '        'herausfinden in welchem Sheet Daten sind, bei mehr als einem gefülltem Sheet erfolgt eine Fehlermeldung
        '        Dim SheetMitDatenCounter As Integer = 0
        '        i = 0
        '        Do While i < objConn.GetSchema("Tables").Rows.Count
        '            Dim objCmdSelect As New OleDbCommand(String.Format("SELECT * FROM [{0}]", Tabellenname(i)), objConn)
        '            objAdapter1 = New OleDbDataAdapter()
        '            objAdapter1.SelectCommand = objCmdSelect

        '            objDataset1 = New DataSet()
        '            objAdapter1.Fill(objDataset1, "XLData")

        '            If objDataset1.Tables(0).Rows.Count > 1 Then
        '                tblTemp = objDataset1.Tables(0)
        '                SheetMitDatenCounter += 1
        '            End If

        '            i += 1
        '        Loop

        '        objConn.Close()

        '        If IsNothing(tblTemp) Then
        '            Exit Sub
        '        End If

        '        For Each xrow As DataRow In tblTemp.Rows
        '            xrow(0) = VIN_bereinigen(xrow(0))
        '        Next

        '        If (Not tblTemp.Rows Is Nothing AndAlso tblTemp.Rows.Count > 0) And SheetMitDatenCounter = 1 Then
        '            objZulassung.FINs = tblTemp
        '            objZulassung.Show(Session("AppID").ToString, Session.SessionID, Me)
        '            Dim blnGo As Boolean = False
        '            If Not objZulassung.Status = 0 AndAlso Not objZulassung.Status = 102 Then
        '                lblerror.Text = objZulassung.Message
        '                lblerror.Visible = True
        '            Else
        '                If objZulassung.Fahrzeuge.Rows.Count = 0 Then
        '                    lblerror.Text = "Zu den gewählten Kriterien wurden keine Ergebnisse gefunden."
        '                ElseIf objZulassung.Status = 102 AndAlso objZulassung.Fahrzeuge.Rows.Count = 1 Then
        '                    lblerror.Text = objZulassung.Message
        '                    lblerror.Visible = True
        '                Else
        '                    blnGo = True
        '                End If
        '            End If
        '            If blnGo Then
        '                Session("objZulassung") = objZulassung
        '                Response.Redirect("Change01_2.aspx?AppID=" & Session("AppID").ToString)
        '            End If
        '        ElseIf SheetMitDatenCounter > 1 Then
        '            lblerror.Text = "Datei enthielt mehrere gefüllte Tabellen."
        '        Else
        '            lblerror.Text = "Datei enthielt keine verwendbaren Daten."
        '        End If
        '    End If
        'End Sub

        Private Sub Check_Data(ByVal tblTemp As DataTable)
            If lblerror.Text = "" Then
                If Not (tblTemp Is Nothing) Then
                    If (Not tblTemp.Rows Is Nothing AndAlso tblTemp.Rows.Count > 0) Then
                        objZulassung.FINs = tblTemp
                        objZulassung.Show(Session("AppID").ToString, Session.SessionID, Me)
                        Dim blnGo As Boolean = False
                        If Not objZulassung.Status = 0 AndAlso Not objZulassung.Status = 102 Then
                            lblerror.Text = objZulassung.Message
                            lblerror.Visible = True
                        Else
                            If objZulassung.Fahrzeuge.Rows.Count = 0 Then
                                lblerror.Text = "Zu den gewählten Kriterien wurden keine Ergebnisse gefunden."
                            ElseIf objZulassung.Status = 102 AndAlso objZulassung.Fahrzeuge.Rows.Count = 1 Then
                                lblerror.Text = objZulassung.Message
                                lblerror.Visible = True
                            Else
                                blnGo = True
                            End If
                        End If
                        If blnGo Then
                            Session("objZulassung") = objZulassung
                            Response.Redirect("Change01_2.aspx?AppID=" & Session("AppID").ToString)
                        End If
                    Else
                        lblerror.Text += "Datei enthielt keine verwendbaren Daten."
                    End If
                End If
            End If
        End Sub

        Public Function VIN_bereinigen(ByRef strIn As String) As String
            Dim Zahlenreihe As String = ""
            Dim myChars() As Char = strIn.ToCharArray()
            For Each ch As Char In myChars
                If Char.IsLetterOrDigit(ch) Then
                    Zahlenreihe += ch
                End If
            Next
            Return Zahlenreihe
        End Function

        Private Sub CheckAuswahl()

            If rb_Einzelauswahl.Checked = True Then
                tr_Vertragsnummer.Visible = True
                tr_Fahrgestellnummer.Visible = True
                tr_FahrgestellnummerZusatz.Visible = True
                tr_NummerZB2.Visible = True
                tr_upload.Visible = False
            Else
                tr_Vertragsnummer.Visible = False
                tr_Fahrgestellnummer.Visible = False
                tr_FahrgestellnummerZusatz.Visible = False
                tr_NummerZB2.Visible = False
                tr_upload.Visible = True
            End If

        End Sub
#End Region
    End Class
End Namespace