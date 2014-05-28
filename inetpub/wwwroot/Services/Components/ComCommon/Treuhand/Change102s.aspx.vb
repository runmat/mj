Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Components.ComCommon.Treuhand
Imports System.Data.OleDb
Imports System.IO

Namespace Treuhand
    Partial Public Class Change102s
        Inherits System.Web.UI.Page

#Region "Declarations"
        Private m_User As Base.Kernel.Security.User
        Private m_App As Base.Kernel.Security.App
        Private SperrObject As SperreFreigabe
        Private FreigabeObject As Treuhandsperre
        Dim fileSourcePath As String

#End Region

#Region "Events"

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            m_User = GetUser(Me) ' füllen Form.Session("objUser"), rückgabe eines UserObjekte
            FormAuth(Me, m_User)

            m_App = New Base.Kernel.Security.App(m_User) 'erzeugt ein App_objekt 
            GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            lblError.Text = ""

            fileSourcePath = CType(ConfigurationManager.AppSettings("DownloadPathExcelvorlagen"), String)
            Literal1.Text = ""

            If Not IsPostBack Then
                FillCustomerDropDownList()
                Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
                FreigabeObject = New Treuhandsperre(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, strFileName)
                Session("FreigabeObject") = FreigabeObject
            Else
                If Session("SperrObject") IsNot Nothing Then
                    SperrObject = CType(Session("SperrObject"), SperreFreigabe)
                End If
                If Session("FreigabeObject") IsNot Nothing Then
                    FreigabeObject = CType(Session("FreigabeObject"), Treuhandsperre)
                End If
            End If

        End Sub

        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
            SetEndASPXAccess(Me)
        End Sub

        Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
            SetEndASPXAccess(Me)
        End Sub

        Protected Sub rdbCustomer_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlCustomer.SelectedIndexChanged

            If String.IsNullOrEmpty(ddlCustomer.SelectedItem.Text) Then
                ShowOptions(False, False, False)
            Else
                Dim name As String
                For xAGS As Integer = 0 To SperrObject.Result.Rows.Count - 1

                    If SperrObject.Result.Rows(xAGS)("ZSELECT").ToString = "TG" Then
                        name = SperrObject.Result.Rows(xAGS)("NAME1_AG").ToString
                    Else
                        name = SperrObject.Result.Rows(xAGS)("NAME1_TG").ToString
                    End If

                    If name = ddlCustomer.SelectedItem.Text Then
                        If SperrObject.Result.Rows(xAGS)("TREUH_KEY").ToString = "F" Then
                            InfoText.Text = "Bitte nehmen Sie den Excel Upload mit der</br> Fahrgestellnummer vor"
                        Else
                            InfoText.Text = "Bitte nehmen Sie den Excel Upload mit der</br> Vertragsnummer vor"
                        End If

                        'Berechtigungen prüfen und Funktionen entspr. freischalten
                        SperrObject.FILLAuthorization(Me.Page, Session("AppID").ToString, Session.SessionID.ToString,
                                                      ddlCustomer.SelectedValue.Split("|"c)(1).ToString(), ddlCustomer.SelectedValue.Split("|"c)(0).ToString())
                        If SperrObject.tblAuthorization IsNot Nothing AndAlso SperrObject.tblAuthorization.Rows.Count > 0 Then
                            Dim tmpRow As DataRow = SperrObject.tblAuthorization.Rows(0)
                            ShowOptions((tmpRow("Freigeben").ToString() = "X"), (tmpRow("Sperren").ToString() = "X"), (tmpRow("Entsperren").ToString() = "X"))
                        End If

                        Exit For
                    End If

                Next
            End If

        End Sub

        Protected Sub rbOption_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rb_Freigeben.CheckedChanged, rb_Sperren.CheckedChanged, rb_Entsperren.CheckedChanged
            CheckedChanged()
        End Sub

        Private Sub CheckedChanged()
            Dim blnShowUploadcontrols As Boolean = ((rb_Sperren.Visible Or rb_Entsperren.Visible) AndAlso (rb_Sperren.Checked Or rb_Entsperren.Checked))
            trFreigabeAuswahl.Visible = rb_Freigeben.Visible AndAlso rb_Freigeben.Checked
            ShowUploadcontrols(blnShowUploadcontrols)
        End Sub

        Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSearch.Click
            If ddlCustomer.SelectedIndex = -1 Then
                lblError.Text = "Bitte wählen Sie einen Treunehmer aus!"
            Else
                If rb_Freigeben.Checked Then
                    StartSearchFreigabe()
                Else
                    'Prüfe Fehlerbedingung
                    If (Not upFile.PostedFile Is Nothing) AndAlso (Not (upFile.PostedFile.FileName = String.Empty)) Then
                        If Right(upFile.PostedFile.FileName.ToUpper, 4) <> ".XLS" Then
                            lblError.Text = "Es können nur Dateien im .XLS - Format verarbeitet werden."
                            Exit Sub
                        End If
                    Else
                        lblError.Text = "Keine Datei ausgewählt!"
                        Exit Sub
                    End If

                    'Lade Datei
                    upload(upFile.PostedFile)
                End If
            End If
        End Sub

        Protected Sub lbBack_Click(sender As Object, e As EventArgs) Handles lbBack.Click
            Session("SperrObject") = Nothing
            Response.Redirect("/Services/(S(" & Session.SessionID & "))/Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
        End Sub

        Protected Sub ibtnExcelVorlage_Click(sender As Object, e As EventArgs) Handles ibtnExcelVorlage.Click

            Dim fName As String = "Upload_Treuhand-Services_VWL"
            Dim sPfad As String = fileSourcePath & fName & ".xls"

            If File.Exists(sPfad) Then

                Session("App_Filepath") = sPfad
                Session("App_ContentType") = "Application/vnd.ms-excel"
                Session("App_ContentDisposition") = "attachment"

                Literal1.Text = "						<script language=""Javascript"">" & vbCrLf
                Literal1.Text &= "						  <!-- //" & vbCrLf
                Literal1.Text &= "                          window.open(""../Report014_1s.aspx?AppID=" & Session("AppID").ToString & """, ""_blank"", ""left=0,top=0,resizable=YES,scrollbars=YES"");" & vbCrLf
                Literal1.Text &= "						  //-->" & vbCrLf
                Literal1.Text &= "						</script>" & vbCrLf

            Else
                lblError.Text = "Die angeforderte Datei wurde nicht auf dem Server gefunden"
            End If

        End Sub

#End Region

#Region "Methods"

        Private Sub FillCustomerDropDownList()

            ddlCustomer.Items.Clear()
            ddlCustomer.Items.Add(New ListItem("", ""))

            SperrObject = New SperreFreigabe(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
            SperrObject.GetCustomer(Me.Page, Session("AppID").ToString, Session.SessionID.ToString)

            If SperrObject.Result.Rows.Count > 0 Then
                Dim itemCustomer As ListItem

                For xAGS As Integer = 0 To SperrObject.Result.Rows.Count - 1

                    If SperrObject.Result.Rows(xAGS)("ZSELECT").ToString = "TG" Then
                        itemCustomer = New ListItem(SperrObject.Result.Rows(xAGS)("NAME1_AG").ToString,
                            SperrObject.Result.Rows(xAGS)("AG").ToString & "|" & SperrObject.Result.Rows(xAGS)("TREU").ToString & "|" & SperrObject.Result.Rows(xAGS)("ZSELECT").ToString)
                    Else
                        itemCustomer = New ListItem(SperrObject.Result.Rows(xAGS)("NAME1_TG").ToString,
                            SperrObject.Result.Rows(xAGS)("AG").ToString & "|" & SperrObject.Result.Rows(xAGS)("TREU").ToString & "|" & SperrObject.Result.Rows(xAGS)("ZSELECT").ToString)
                    End If

                    ddlCustomer.Items.Add(itemCustomer)

                Next
            End If

            Session("SperrObject") = SperrObject
        End Sub

        Private Sub ShowOptions(ByVal freigeben As Boolean, ByVal sperren As Boolean, ByVal entsperren As Boolean)
            trAktion.Visible = (freigeben Or sperren Or entsperren)

            rb_Freigeben.Visible = freigeben
            rb_Sperren.Visible = sperren
            rb_Entsperren.Visible = entsperren

            cmdSearch.Enabled = (freigeben Or sperren Or entsperren)

            If rb_Freigeben.Visible Then
                rb_Freigeben.Checked = True
            ElseIf rb_Sperren.Visible Then
                rb_Sperren.Checked = True
            ElseIf rb_Entsperren.Visible Then
                rb_Entsperren.Checked = True
            End If
            CheckedChanged()
        End Sub

        Private Sub ShowUploadcontrols(ByVal show As Boolean)
            trUpload.Visible = show
            pnInfo.Visible = show
        End Sub

        Private Sub CheckInputTable(ByVal tblInput As DataTable)
            Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
            logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
            Session.Add("logObj", logApp)

            Dim i As Integer = 0
            Dim rowData As DataRow
            SperrObject.GiveResultStructure()
            SperrObject.tblUpload.Clear()
            For Each rowData In tblInput.Rows
                i += 1
                If IsDBNull(rowData(0)) And IsDBNull(rowData(1)) Then Exit For

                'Mit "0" gefüllte Zellen sind als leer zu betrachten

                'Vertragsnummer
                Dim strReferenz As String = ""
                If Not IsDBNull(rowData(0)) AndAlso rowData(0).ToString().Trim() <> "0" Then
                    strReferenz = CStr(rowData(0)).Trim()
                End If
                'Fahrgestellnummer
                Dim strEquiKey As String = ""
                If Not IsDBNull(rowData(1)) AndAlso rowData(1).ToString().Trim() <> "0" Then
                    strEquiKey = CStr(rowData(1)).Trim()
                End If
                'Ent-/Sperrdatum (soll pauschal auf aktuelles Tagesdatum gesetzt werden, unabhängig von der Benutzereingabe)
                Dim strSperrdatum As String = DateTime.Today.ToString("dd.MM.yyyy")
                'Vertragsbeginn
                Dim strVertragsbeginn As String = ""
                If Not IsDBNull(rowData(3)) AndAlso rowData(3).ToString().Trim() <> "0" Then
                    strVertragsbeginn = CStr(rowData(3)).Trim()
                End If
                'Vertragsende
                Dim strVertragsende As String = ""
                If Not IsDBNull(rowData(4)) AndAlso rowData(4).ToString().Trim() <> "0" Then
                    strVertragsende = CStr(rowData(4)).Trim()
                End If
                'Versanddatum
                Dim strVersanddatum As String = ""
                If Not IsDBNull(rowData(5)) AndAlso rowData(5).ToString().Trim() <> "0" Then
                    strVersanddatum = CStr(rowData(5)).Trim()
                End If
                'Händlernummer
                Dim strHaendlernummer As String = ""
                If Not IsDBNull(rowData(6)) AndAlso rowData(6).ToString().Trim() <> "0" Then
                    strHaendlernummer = CStr(rowData(6)).Trim()
                End If
                'Händlername
                Dim strHaendlername As String = ""
                If Not IsDBNull(rowData(7)) AndAlso rowData(7).ToString().Trim() <> "0" Then
                    strHaendlername = CStr(rowData(7)).Trim()
                End If
                'Adresszusatz/Ansprechpartner
                Dim strName2 As String = ""
                If Not IsDBNull(rowData(8)) AndAlso rowData(8).ToString().Trim() <> "0" Then
                    strName2 = CStr(rowData(8)).Trim()
                End If
                'Strasse/Hausnr
                Dim strStrasse As String = ""
                If Not IsDBNull(rowData(9)) AndAlso rowData(9).ToString().Trim() <> "0" Then
                    strStrasse = CStr(rowData(9)).Trim()
                End If
                'PLZ
                Dim strPLZ As String = ""
                If Not IsDBNull(rowData(10)) AndAlso rowData(10).ToString().Trim() <> "0" Then
                    strPLZ = CStr(rowData(10)).Trim()
                End If
                'Ort
                Dim strOrt As String = ""
                If Not IsDBNull(rowData(11)) AndAlso rowData(11).ToString().Trim() <> "0" Then
                    strOrt = CStr(rowData(11)).Trim()
                End If
                'Postfach
                Dim strPostfach As String = ""
                If Not IsDBNull(rowData(12)) AndAlso rowData(12).ToString().Trim() <> "0" Then
                    strPostfach = CStr(rowData(12)).Trim()
                End If

                Dim rowNew As DataRow = SperrObject.tblUpload.NewRow

                rowNew("ID") = SperrObject.tblUpload.Rows.Count + 1
                rowNew("AG") = ddlCustomer.SelectedValue.Split("|"c)(0).ToString

                rowNew("ZZREFERENZ2") = strReferenz
                rowNew("EQUI_KEY") = strEquiKey                
                If IsDate(strSperrdatum) Then
                    rowNew("SPERRDAT") = strSperrdatum
                Else
                    rowNew("SPERRDAT") = Now.ToShortDateString
                End If
                rowNew("VERTR_BEGINN") = strVertragsbeginn
                rowNew("VERTR_ENDE") = strVertragsende
                'Versanddaten
                rowNew("Versanddatum") = strVersanddatum
                rowNew("Haendlernummer") = strHaendlernummer
                rowNew("Haendlername") = strHaendlername
                rowNew("Name2") = strName2
                rowNew("Strasse") = strStrasse
                rowNew("PLZ") = strPLZ
                rowNew("Ort") = strOrt
                rowNew("Postfach") = strPostfach

                rowNew("ERNAM") = m_User.UserName
                rowNew("ERDAT") = Now.ToShortDateString
                If rb_Sperren.Checked = True Then
                    rowNew("TREUH_VGA") = "S"
                    SperrObject.SperrEnsperr = "gesperrt"
                Else
                    rowNew("TREUH_VGA") = "F"
                    SperrObject.SperrEnsperr = "entsperrt"
                End If
                rowNew("SUBRC") = ""
                rowNew("ERROR") = ""

                'Pruefen, ob schon in der Autorisierung.
                Dim strInitiator As String = ""
                Dim intAuthorizationID As Int32
                m_App.CheckForPendingAuthorization(CInt(Session("AppID")), m_User.Organization.OrganizationId, ddlCustomer.SelectedItem.Text, rowNew("EQUI_KEY").ToString, m_User.IsTestUser, strInitiator, intAuthorizationID)
                If Not strInitiator.Length = 0 Then
                    'Fahrzeug wurde schon mal freigegeben und liegt zur Autorisierung vor
                    rowNew("MESSAGE") = "Aut."
                    rowNew("SELECT") = ""
                Else
                    rowNew("MESSAGE") = ""
                    rowNew("SELECT") = "99"
                End If

                SperrObject.tblUpload.Rows.Add(rowNew)
            Next

            SperrObject.Treunehmer = ddlCustomer.SelectedItem.Text

        End Sub

        Private Sub upload(ByVal uFile As System.Web.HttpPostedFile)

            Dim objConn As OleDbConnection = Nothing

            Try
                Dim filepath As String = ConfigurationManager.AppSettings("ExcelPath")
                Dim filename As String
                Dim info As System.IO.FileInfo

                'Dateiname: User_yyyyMMddhhmmss.xls
                filename = m_User.UserName & "_" & Format(Now, "yyyyMMddhhmmss") & ".xls"

                If Not (uFile Is Nothing) Then
                    uFile.SaveAs(ConfigurationManager.AppSettings("ExcelPath") & filename)
                    info = New System.IO.FileInfo(filepath & filename)
                    If Not (info.Exists) Then
                        lblError.Text = "Fehler beim Speichern."
                        Exit Sub
                    End If

                    'Datei gespeichert -> Auswertung
                    Dim sConnectionString As String = ""
                    If Right(upFile.PostedFile.FileName.ToUpper, 4) = ".XLS" Then
                        sConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" & _
                         "Data Source=" & filepath & filename & ";" & _
                         "Extended Properties=""Excel 8.0;HDR=YES;"""
                    Else
                        sConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + filepath + filename + _
                        ";Extended Properties=""Excel 12.0 Xml;HDR=YES"""
                    End If

                    objConn = New OleDbConnection(sConnectionString)
                    objConn.Open()

                    Dim tab As String = "Tabelle1$"
                    Dim objAdapter1 As New OleDbDataAdapter()

                    Dim objCmdSelect As New OleDbCommand("SELECT * FROM [" + tab + "]", objConn)
                    objAdapter1.SelectCommand = objCmdSelect

                    Dim objDataset1 As New DataSet()
                    objAdapter1.Fill(objDataset1, "XLData")

                    CheckInputTable(objDataset1.Tables(0))

                    objConn.Close()

                    If Not SperrObject.tblUpload.Rows Is Nothing AndAlso SperrObject.tblUpload.Rows.Count > 0 Then

                        Dim tmp As String() = ddlCustomer.SelectedValue.Split("|"c)
                        SperrObject.Treugeber = tmp(1).ToString

                        SperrObject.AG = tmp(0).ToString
                        SperrObject.TREU = tmp(1).ToString
                        SperrObject.ZSelect = tmp(2).ToString

                        If rb_Sperren.Checked Then
                            SperrObject.IsSperren = True
                        Else
                            SperrObject.IsSperren = False
                        End If

                        Session("SperrObject") = SperrObject
                        Response.Redirect("Change102s_3.aspx?AppID=" & Session("AppID").ToString)
                    Else
                        lblError.Text = "Datei enthielt keine verwendbaren Daten."
                    End If
                End If
            Catch ex As Exception
                lblError.Text = "Bitte Format der Importdatei und den Tabellennamen (Tabelle1) prüfen."

                If objConn Is Nothing Then
                    objConn.Close()
                End If

            End Try
        End Sub

        Private Sub StartSearchFreigabe()
            If Not String.IsNullOrEmpty(ddlCustomer.SelectedValue) Then
                FreigabeObject.Treugeber = ddlCustomer.SelectedValue.Split("|"c)(1).ToString()
                FreigabeObject.Treunehmer = ddlCustomer.SelectedValue.Split("|"c)(0).ToString()
            End If
            If rb_Gesperrte.Checked Then
                FreigabeObject.Aktion = "G"
            Else
                FreigabeObject.Aktion = "A"
            End If
            Session("FreigabeObject") = FreigabeObject

            Response.Redirect("Change102s_2.aspx?AppID=" & Session("AppID").ToString)
        End Sub

#End Region

    End Class
End Namespace
