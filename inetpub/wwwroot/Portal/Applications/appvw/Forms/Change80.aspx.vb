Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common
Imports System.Data.OleDb
Imports System.Data

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
    Private objHaendler As VW_06

    Protected WithEvents Hyperlink1 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkLogout As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents txtFahrgestellnummer As System.Web.UI.WebControls.TextBox
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
    Protected WithEvents txtIKZ As System.Web.UI.WebControls.TextBox
    Protected WithEvents trVIN1 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trVIN2 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents txtVorhaben As System.Web.UI.WebControls.TextBox

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cmdSearch.Visible = True
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If (Session("objHaendler") Is Nothing) OrElse (Not IsPostBack) Then
                objHaendler = New VW_06(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
            Else
                objHaendler = CType(Session("objHaendler"), VW_06)
            End If

            Session("objHaendler") = objHaendler
            If Not IsPostBack Then
                'Keine Eingeabe der VIN als Suchkriterium!
                trVIN1.Visible = False
                trVIN2.Visible = False

                tblSelection.Visible = True
                trcmdSearch.Visible = True
                tblUpload.Visible = False
                trcmdUpload.Visible = True
                trcmdContinue.Visible = False
                Literal1.Text = "		<script language=""JavaScript"">" & vbCrLf
                Literal1.Text &= "			<!-- //" & vbCrLf
                Literal1.Text &= "			window.document.Form1.txtVorhaben.focus();" & vbCrLf
                Literal1.Text &= "			//-->" & vbCrLf
                Literal1.Text &= "		</script>" & vbCrLf
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub DoSubmit()
        Try
            Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
            logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
            Session.Add("logObj", logApp)

            lblError.Text = ""

            'Keine Platzhaltersuche -> Werfe Platzhalter 'raus
            txtVorhaben.Text = RemoveBlankAndPlaceholder(txtVorhaben.Text)
            txtIKZ.Text = RemoveBlankAndPlaceholder(txtIKZ.Text)
            txtFahrgestellnummer.Text = RemoveBlankAndPlaceholder(txtFahrgestellnummer.Text)

            If txtVorhaben.Text.Length = 0 Then
                objHaendler.Vorhaben = ""
            Else
                objHaendler.Vorhaben = txtVorhaben.Text
            End If

            'IKZ auswerten
            objHaendler.FahrzeugeSuche = New DataTable()
            objHaendler.FahrzeugeSuche.Columns.Add("REFERENZ1", System.Type.GetType("System.String"))
            objHaendler.FahrzeugeSuche.Columns.Add("CHASSIS_NUM", System.Type.GetType("System.String"))
            objHaendler.FahrzeugeSuche.Columns.Add("CHASSIS_NUM_2", System.Type.GetType("System.String"))
            Dim tmpRow As DataRow
            If txtIKZ.Text.Length > 0 Then
                Dim arrayIKZ() As String = txtIKZ.Text.Split(","c)
                Dim i As Integer
                For i = 0 To arrayIKZ.Length - 1
                    tmpRow = objHaendler.FahrzeugeSuche.NewRow
                    tmpRow("REFERENZ1") = arrayIKZ(i)
                    tmpRow("CHASSIS_NUM") = ""
                    tmpRow("CHASSIS_NUM_2") = ""
                    objHaendler.FahrzeugeSuche.Rows.Add(tmpRow)
                Next
            ElseIf txtFahrgestellnummer.Text.Length > 0 Then
                tmpRow = objHaendler.FahrzeugeSuche.NewRow
                tmpRow("REFERENZ1") = ""
                tmpRow("CHASSIS_NUM") = txtFahrgestellnummer.Text
                tmpRow("CHASSIS_NUM_2") = ""
                objHaendler.FahrzeugeSuche.Rows.Add(tmpRow)
            End If

            objHaendler.Show()

            If Not objHaendler.Status = 0 Then
                lblError.Text = objHaendler.Message
                lblError.Visible = True
            Else
                If objHaendler.Result.Rows.Count = 0 Then
                    lblError.Text = "Zu den gewählten Kriterien wurden keine Ergebnisse gefunden."
                Else
                    Session("objHaendler") = objHaendler
                    Response.Redirect("Change80_2.aspx?AppID=" & Session("AppID").ToString)
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Beim Ermitteln der Daten ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
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

    Private Function RemoveBlankAndPlaceholder(ByVal strIn As String) As String
        Dim strReturn As String = strIn.Trim(" "c)
        strReturn = Replace(strReturn, "*", "")
        strReturn = Replace(strReturn, "%", "")
        Return strReturn
    End Function

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
                 "Extended Properties=""Excel 8.0;HDR=YES;"""

                Dim objConn As New OleDbConnection(sConnectionString)
                objConn.Open()

                Dim objCmdSelect As New OleDbCommand("SELECT * FROM [Tabelle1$]", objConn)

                Dim objAdapter1 As New OleDbDataAdapter()
                objAdapter1.SelectCommand = objCmdSelect

                Dim objDataset1 As New DataSet()
                objAdapter1.Fill(objDataset1, "XLData")

                Dim rowData As DataRow

                Dim i As Integer = 0

                Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
                logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
                Session.Add("logObj", logApp)

                objHaendler.FahrzeugeSuche = New DataTable()
                objHaendler.FahrzeugeSuche.Columns.Add("REFERENZ1", System.Type.GetType("System.String"))
                objHaendler.FahrzeugeSuche.Columns.Add("CHASSIS_NUM", System.Type.GetType("System.String"))
                objHaendler.FahrzeugeSuche.Columns.Add("CHASSIS_NUM_2", System.Type.GetType("System.String"))
                Dim tmpRow As DataRow

                For Each rowData In objDataset1.Tables(0).Rows
                    i += 1
                    If TypeOf rowData(0) Is System.DBNull And TypeOf rowData(1) Is System.DBNull Then Exit For

                    Dim strIKZ As String = ""
                    If Not TypeOf rowData(0) Is System.DBNull Then
                        strIKZ = CStr(rowData(0)).Trim(" "c)
                    End If
                    Dim strFahrgestellnummer As String = ""
                    If Not TypeOf rowData(1) Is System.DBNull Then
                        strFahrgestellnummer = CStr(rowData(1)).Trim(" "c)
                    End If
                    Dim strFahrgestellnummer2 As String = ""
                    If Not TypeOf rowData(2) Is System.DBNull Then
                        strFahrgestellnummer2 = CStr(rowData(2)).Trim(" "c)
                    End If

                    If strIKZ.Length + strFahrgestellnummer.Length = 0 Then GoTo Ignore

                    tmpRow = objHaendler.FahrzeugeSuche.NewRow
                    tmpRow("REFERENZ1") = strIKZ
                    tmpRow("CHASSIS_NUM") = strFahrgestellnummer
                    tmpRow("CHASSIS_NUM_2") = strFahrgestellnummer2
                    objHaendler.FahrzeugeSuche.Rows.Add(tmpRow)

Ignore:
                Next


                objConn.Close()

                If Not objHaendler.FahrzeugeSuche.Rows Is Nothing AndAlso objHaendler.FahrzeugeSuche.Rows.Count > 0 Then
                    objHaendler.Show()

                    If Not objHaendler.Status = 0 Then
                        lblError.Text = objHaendler.Message
                        lblError.Visible = True
                    Else
                        If objHaendler.Result.Rows.Count = 0 Then
                            lblError.Text = "Zu den gewählten Kriterien wurden keine Ergebnisse gefunden."
                        Else
                            Session("objHaendler") = objHaendler
                            Response.Redirect("Change80_2.aspx?AppID=" & Session("AppID").ToString)
                        End If
                    End If
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
' $History: Change80.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 4.05.09    Time: 11:14
' Updated in $/CKAG/Applications/appvw/Forms
' ITA: 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 17:48
' Created in $/CKAG/Applications/appvw/Forms
' 
' *****************  Version 2  *****************
' User: Uha          Date: 15.08.07   Time: 11:16
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Forms
' ITA 1177 "Werkstattzuordnungsliste II" - testfähige Version
' 
' *****************  Version 1  *****************
' User: Uha          Date: 14.08.07   Time: 13:42
' Created in $/CKG/Applications/AppVW/AppVWWeb/Forms
' ITA 1177 "Werkstattzuordnungsliste II" - kompilierfähige Rohversion
' hinzugefügt
' 
' ************************************************
