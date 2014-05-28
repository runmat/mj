Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common
Imports System.Data.OleDb
Imports System.Data

Public Class Change81
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
    Private objHandler As MDR_06

    Protected WithEvents Hyperlink1 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkLogout As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents tblSelection As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblUpload As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents cmdUpload As System.Web.UI.WebControls.LinkButton
    Protected WithEvents upFile As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents lblExcelfile As System.Web.UI.WebControls.Label
    Protected WithEvents cmdContinue As System.Web.UI.WebControls.LinkButton
    Protected WithEvents Literal1 As System.Web.UI.WebControls.Literal
    Protected WithEvents txtHaendlernummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents lnkHaendlerSuchen As System.Web.UI.WebControls.LinkButton
    Protected WithEvents txtName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtStrasse As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtPLZ As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtOrt As System.Web.UI.WebControls.TextBox
    Protected WithEvents rblVersandart As RadioButtonList
    Protected WithEvents rbVK As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rbVKLN As System.Web.UI.WebControls.RadioButton
    Protected WithEvents calAbDatum As System.Web.UI.WebControls.Calendar
    Protected WithEvents btnOpenSelectAb As System.Web.UI.WebControls.Button
    Protected WithEvents txtDurchfuehrungsDatum As System.Web.UI.WebControls.TextBox
    Protected WithEvents trCalendar As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lnkAdresseManuell As System.Web.UI.WebControls.LinkButton

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If Not IsPostBack Then
                ShowUpload()
                trCalendar.Visible = False
            End If
            If (Session("objHandler") Is Nothing) Then
                ' OrElse (Not IsPostBack)
                DoNew()
            Else
                objHandler = CType(Session("objHandler"), MDR_06)
                If Not IsPostBack Then
                    If objHandler.SucheHaendlernummer.Length + objHandler.EmpfaengerName.Length + objHandler.EmpfaengerOrt.Length + objHandler.EmpfaengerPLZ.Length + objHandler.EmpfaengerStrasse.Length = 0 Then
                        DoNew()
                    Else
                        ShowInput()
                        txtHaendlernummer.Text = objHandler.SucheHaendlernummer
                        txtName.Text = objHandler.EmpfaengerName
                        txtOrt.Text = objHandler.EmpfaengerOrt
                        txtPLZ.Text = objHandler.EmpfaengerPLZ
                        txtStrasse.Text = objHandler.EmpfaengerStrasse
                        rblVersandart.SelectedValue = objHandler.Versandart
                        If objHandler.Versandcode = "VK" Then
                            rbVK.Checked = True
                            rbVKLN.Checked = False
                        Else
                            rbVK.Checked = False
                            rbVKLN.Checked = True
                        End If
                        If txtHaendlernummer.Text.Length > 0 Then
                            ShowManuell()
                        Else
                            ShowVorhanden()
                        End If
                    End If
                End If
            End If

            Session("objHandler") = objHandler
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub DoNew()
        objHandler = New MDR_06(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
        Session("logObj") = logApp
    End Sub

    Private Sub ShowUpload()
        tblUpload.Visible = True
        tblSelection.Visible = False
        Literal1.Text = ""
        cmdUpload.Text = "Einzelerfassung"
    End Sub

    Private Sub ShowInput()
        tblUpload.Visible = False
        tblSelection.Visible = True
        cmdUpload.Text = "Datei-Upload"

        If lnkHaendlerSuchen.Enabled Then
            If objHandler.Result Is Nothing Then
                objHandler.Show(Session("AppID").ToString, Session.SessionID.ToString, Me)
                If Not objHandler.Status = 0 Then
                    lblError.Text = "Es wurden keine hinterlegten Adressen gefunden."
                    ShowVorhanden()
                    lnkAdresseManuell.Enabled = False
                End If
            End If
        End If

        Session("objHandler") = objHandler
    End Sub

    Private Sub cmdUpload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpload.Click
        If tblSelection.Visible Then
            ShowUpload()
        Else
            ShowInput()
        End If
    End Sub

    Private Sub cmdContinue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdContinue.Click
        If tblUpload.Visible Then
            'Prüfe Fehlerbedingung
            If (Not upFile.PostedFile Is Nothing) AndAlso (Not (upFile.PostedFile.FileName = String.Empty)) Then
                lblExcelfile.Text = upFile.PostedFile.FileName
                If Right(upFile.PostedFile.FileName.ToUpper, 4) <> ".CSV" Then
                    lblError.Text = "Es können nur Dateien im .CSV - Format verarbeitet werden."
                    Exit Sub
                End If
            Else
                lblError.Text = "Keine Datei ausgewählt"
                Exit Sub
            End If

            'Lade Datei
            upload(upFile.PostedFile)
        Else
            If txtHaendlernummer.Text.Trim(" "c).Length > 0 Or (txtName.Text.Trim(" "c).Length > 0 And txtOrt.Text.Trim(" "c).Length > 0 And txtPLZ.Text.Trim(" "c).Length > 0 And txtStrasse.Text.Trim(" "c).Length > 0) Then
                If txtHaendlernummer.Text.Trim(" "c).Length = 0 Then
                    objHandler.EmpfaengerName = txtName.Text
                    objHandler.EmpfaengerOrt = txtOrt.Text
                    objHandler.EmpfaengerPLZ = txtPLZ.Text
                    objHandler.EmpfaengerStrasse = txtStrasse.Text
                Else
                    If Not PruefeHaendler() Then
                        Exit Sub
                    End If
                End If

                objHandler.Versandart = rblVersandart.SelectedValue
                If rbVK.Checked Then
                    objHandler.Versandcode = "VK"
                Else
                    objHandler.Versandcode = "VKLN"
                End If

                If objHandler.Fahrzeuge.Rows.Count > 0 Then
                    Dim tmpRow As DataRow
                    For Each tmpRow In objHandler.Fahrzeuge.Rows
                        tmpRow("Konzs") = objHandler.SucheHaendlernummer
                        tmpRow("Name1") = objHandler.EmpfaengerName
                        tmpRow("City1") = objHandler.EmpfaengerOrt
                        tmpRow("Post_Code1") = objHandler.EmpfaengerPLZ
                        tmpRow("Street") = objHandler.EmpfaengerStrasse
                        tmpRow("Vermarktungscode") = objHandler.Versandcode
                        tmpRow("Versandart") = objHandler.Versandart
                    Next
                End If

                Session("objHandler") = objHandler
                Response.Redirect("Change81_2.aspx?AppID=" & Session("AppID").ToString)
            Else
                lblError.Text = "Keine Adressdaten angegeben"
            End If
        End If
    End Sub

    Private Sub upload(ByVal uFile As System.Web.HttpPostedFile)
        Try
            Dim filepath As String = ConfigurationManager.AppSettings("UploadpathLocal")
            Dim filename As String
            Dim info As System.IO.FileInfo

            'Dateiname: User_yyyyMMddhhmmss.xls
            filename = m_User.UserName & "_" & Format(Now, "yyyyMMddhhmmss") & ".csv"

            If Not (uFile Is Nothing) Then
                uFile.SaveAs(ConfigurationManager.AppSettings("UploadpathLocal") & filename)
                uFile = Nothing
                info = New System.IO.FileInfo(filepath & filename)
                If Not (info.Exists) Then
                    lblError.Text = "Fehler beim Speichern."
                    Exit Sub
                End If


                'Datei gespeichert -> Auswertung

                Dim tblTemp As DataTable = GetDataTable(filepath & filename, ";")


                'ergab Probleme, da DataAdapter versuchte Datentypen für die Einzelnen Spalten zu definieren, JJ 2007.11.08
                '--------------------------------------------------------------------
                'Dim sConnectionString As String = "Provider=Microsoft.Jet.OLEDB.4.0;" & _
                '                 "Data Source=" & filepath & ";" & _
                '                 "Extended Properties=""Text;HDR=NO;FMT=Delimited\"""

                'Dim objConn As New OleDbConnection(sConnectionString)
                'objConn.Open()

                'Dim objCmdSelect As New OleDbCommand("SELECT * FROM " & filename, objConn)

                'Dim objAdapter1 As New OleDbDataAdapter()
                'objAdapter1.SelectCommand = objCmdSelect

                'Dim objDataset1 As New DataSet()
                'objAdapter1.Fill(objDataset1, "XLData")

                'Dim tblTemp As DataTable = objDataset1.Tables(0)

                'objConn.Close()
                '--------------------------------------------------------------------


                If Not tblTemp Is Nothing Then
                    If tblTemp.Rows.Count > 0 Then
                        Dim tmpRow As DataRow
                        Dim tmpNewRow As DataRow
                        Dim tmpRows() As DataRow
                        objHandler = CType(Session("objHandler"), MDR_06)
                        'wenn noch keine MDR Händlerdaten gefüllt sind, diese jetzt füllen
                        If objHandler.Result Is Nothing Then
                            objHandler.Show(Session("AppID").ToString, Session.SessionID.ToString, Me)
                        End If
                        For Each tmpRow In tblTemp.Rows
                            tmpNewRow = objHandler.Fahrzeuge.NewRow
                            tmpNewRow("KONZS") = Left(CStr(tmpRow(3)), 10)
                            'Um auch beim Upload von Dateien Händerdaten anzuzeigen JJ2007.11.14
                            tmpRows = objHandler.Result.Select("Konzs='" & Left(CStr(tmpRow(3)), 10) & "'")
                            If tmpRows.Length = 1 Then
                                tmpNewRow("Name1") = tmpRows(0).Item("Name1")
                                tmpNewRow("Street") = tmpRows(0).Item("Stras")
                                tmpNewRow("Post_Code1") = tmpRows(0).Item("Pstlz")
                                tmpNewRow("City1") = tmpRows(0).Item("Ort01")
                            End If

                            tmpNewRow("CHASSIS_NUM") = Left(CStr(tmpRow(0)), 30)


                            Dim tmpStr As String = CStr(tmpRow(5)).ToUpper

                            Select Case True
                                Case tmpStr.Contains("POST")
                                    tmpNewRow("VERSANDART") = objHandler.Umschluesselung("1391")
                                Case tmpStr.Contains("9.00 UHR")
                                    tmpNewRow("VERSANDART") = objHandler.Umschluesselung("1385")
                                Case tmpStr.Contains("12.00 UHR")
                                    tmpNewRow("VERSANDART") = objHandler.Umschluesselung("1390")
                                Case Else
                                    Throw New Exception("unbekannte Versandart: " & tmpStr)
                            End Select
                            tmpNewRow("VERMARKTUNGSCODE") = Left(CStr(tmpRow(4)), 10)
                            tmpNewRow("ERNAM") = Left(m_User.UserName, 12)
                            objHandler.Fahrzeuge.Rows.Add(tmpNewRow)
                        Next
                        objHandler.SucheHaendlernummer = ""
                        objHandler.EmpfaengerName = ""
                        objHandler.EmpfaengerOrt = ""
                        objHandler.EmpfaengerPLZ = ""
                        objHandler.EmpfaengerStrasse = ""
                        Session("objHandler") = objHandler
                        Response.Redirect("Change81_4.aspx?AppID=" & Session("AppID").ToString)
                    End If
                End If
            End If
        Catch ex As Exception
            lblError.Text = ex.Message & " / " & ex.StackTrace
        Finally
        End Try
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Function PruefeHaendler() As Boolean
        Dim blnReturn As Boolean = False
        Try
            objHandler.SucheHaendlernummer = ""
            txtHaendlernummer.Text = txtHaendlernummer.Text.Trim(" "c)
            If txtHaendlernummer.Text.Length = 0 Then
                lblError.Text = "Bitte geben Sie eine Händlernummer als Suchkriterium ein."
                Exit Function
            End If
            If objHandler.Result Is Nothing Then
                lblError.Text = "Es wurden keine hinterlegten Adressen gefunden."
                Exit Function
            End If
            Dim tmpRows() As DataRow = objHandler.Result.Select("Konzs='" & txtHaendlernummer.Text & "'")
            If tmpRows.Length = 1 Then
                objHandler.SucheHaendlernummer = txtHaendlernummer.Text
                txtName.Text = CStr(tmpRows(0)("Name1"))
                txtOrt.Text = CStr(tmpRows(0)("Ort01"))
                txtPLZ.Text = CStr(tmpRows(0)("Pstlz"))
                txtStrasse.Text = CStr(tmpRows(0)("Stras"))
                'Properties in OBJ.Händler Anlegen JJ2007.11.13
                'werden dann beim Seitenwechsel in die FarhzeugTabelle geschrieben. JJ2007.11.14
                objHandler.EmpfaengerName = CStr(tmpRows(0)("Name1"))
                objHandler.EmpfaengerOrt = CStr(tmpRows(0)("Ort01"))
                objHandler.EmpfaengerPLZ = CStr(tmpRows(0)("Pstlz"))
                objHandler.EmpfaengerStrasse = CStr(tmpRows(0)("Stras"))
                blnReturn = True
            Else
                lblError.Text = "Händlernummer nicht gefunden"
            End If
            Session("objHandler") = objHandler
        Catch ex As Exception
            lblError.Text = "Beim Suchen des Händlers ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
        Return blnReturn
    End Function

    Private Sub lnkHaendlerSuchen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkHaendlerSuchen.Click
        PruefeHaendler()
    End Sub

    Private Sub lnkAdresseManuell_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkAdresseManuell.Click
        txtHaendlernummer.Text = ""
        txtName.Text = ""
        txtOrt.Text = ""
        txtPLZ.Text = ""
        txtStrasse.Text = ""
        If txtHaendlernummer.Enabled Then
            ShowVorhanden()
        Else
            ShowManuell()
        End If
    End Sub

    Private Sub ShowVorhanden()
        lnkAdresseManuell.Text = "Vorhandene Adresse"
        lnkHaendlerSuchen.Enabled = False
        txtHaendlernummer.Enabled = False
        txtHaendlernummer.CssClass = "InfoBoxFlat"
        txtName.Enabled = True
        txtName.CssClass = ""
        txtOrt.Enabled = True
        txtOrt.CssClass = ""
        txtPLZ.Enabled = True
        txtPLZ.CssClass = ""
        txtStrasse.Enabled = True
        txtStrasse.CssClass = ""
        Literal1.Text = "		<script language=""JavaScript"">" & vbCrLf
        Literal1.Text &= "			<!-- //" & vbCrLf
        Literal1.Text &= "			window.document.Form1.txtName.focus();" & vbCrLf
        Literal1.Text &= "			//-->" & vbCrLf
        Literal1.Text &= "		</script>" & vbCrLf
    End Sub

    Private Sub ShowManuell()
        lnkAdresseManuell.Text = "Adresse manuell"
        lnkHaendlerSuchen.Enabled = True
        txtHaendlernummer.Enabled = True
        txtHaendlernummer.CssClass = ""
        txtName.Enabled = False
        txtName.CssClass = "InfoBoxFlat"
        txtOrt.Enabled = False
        txtOrt.CssClass = "InfoBoxFlat"
        txtPLZ.Enabled = False
        txtPLZ.CssClass = "InfoBoxFlat"
        txtStrasse.Enabled = False
        txtStrasse.CssClass = "InfoBoxFlat"
        Literal1.Text = "		<script language=""JavaScript"">" & vbCrLf
        Literal1.Text &= "			<!-- //" & vbCrLf
        Literal1.Text &= "			window.document.Form1.txtHaendlernummer.focus();" & vbCrLf
        Literal1.Text &= "			//-->" & vbCrLf
        Literal1.Text &= "		</script>" & vbCrLf
    End Sub

    Private Sub btnOpenSelectAb_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpenSelectAb.Click
        calAbDatum.Visible = True
    End Sub

    Private Sub calAbDatum_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles calAbDatum.SelectionChanged
        calAbDatum.Visible = False
        If calAbDatum.SelectedDate < objHandler.SuggestionDay() Then
            txtDurchfuehrungsDatum.Text = Format(objHandler.SuggestionDay(), "dd.MM.yyyy")
        Else
            txtDurchfuehrungsDatum.Text = calAbDatum.SelectedDate.ToShortDateString
        End If
    End Sub


    'JJ2007.11.09
    ''' <summary>
    ''' Gibt den Inhalt einer CSV Datei in einer DataTable zurück
    '''</summary>
    '''<param name="path">Pfad der CSV Datei</param>
    ''' <param name="seperator">Zeichen mit dem die Spalten getrennt werden. Meist ';' oder ','</param>
    ''' <returns></returns>
    Private Function GetDataTable(ByVal path As String, ByVal seperator As Char) As DataTable

        Dim dt As New DataTable()
        Dim aFile As New System.IO.FileStream(path, IO.FileMode.Open)
        System.Diagnostics.Debug.WriteLine(aFile.CanRead())
        Dim strLine As String
        Dim sr As New System.IO.StreamReader(aFile, System.Text.Encoding.Default)
        Dim aStrSplitted() As String
        Dim strElement As String

        Try

            strLine = sr.ReadLine()
            aStrSplitted = strLine.Split(seperator)
            For Each strElement In aStrSplitted
                dt.Columns.Add()
            Next

            Do
                aStrSplitted = strLine.Split(seperator)
                dt.Rows.Add(aStrSplitted)
                dt.AcceptChanges()
                strLine = sr.ReadLine()
                System.Diagnostics.Debug.WriteLine(strLine)
            Loop Until strLine Is Nothing
            Return dt
        Catch ex As Exception
            Return Nothing
            lblError.Text = ex.Message

        Finally
            sr.Close()
        End Try

    End Function




End Class

' ************************************************
' $History: Change81.aspx.vb $
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 6.04.10    Time: 14:30
' Updated in $/CKAG/Applications/appffd/Forms
' ITA: 3610
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 26.06.09   Time: 10:33
' Updated in $/CKAG/Applications/appffd/Forms
' ITA: 2918
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 30.04.09   Time: 14:43
' Updated in $/CKAG/Applications/appffd/Forms
' ITA: 2837
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 26.11.08   Time: 10:28
' Updated in $/CKAG/Applications/appffd/Forms
' ITA 2382 testfertig
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:23
' Created in $/CKAG/Applications/appffd/Forms
' 
' *****************  Version 12  *****************
' User: Jungj        Date: 13.02.08   Time: 13:16
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 14.11.07   Time: 8:52
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 14.11.07   Time: 8:51
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 13.11.07   Time: 17:19
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 13.11.07   Time: 14:57
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 8.11.07    Time: 15:03
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 8.11.07    Time: 14:56
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 8.11.07    Time: 13:52
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 8.11.07    Time: 11:09
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' 
' *****************  Version 3  *****************
' User: Uha          Date: 22.08.07   Time: 13:23
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' ITA 1208: Bugfixing 1
' 
' *****************  Version 2  *****************
' User: Uha          Date: 22.08.07   Time: 12:30
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' ITA 1208 Testversion
' 
' *****************  Version 1  *****************
' User: Uha          Date: 21.08.07   Time: 17:37
' Created in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' ITA 1208: Kompilierfähige Vorversion mit Teilfunktionalität
' 
' ************************************************
