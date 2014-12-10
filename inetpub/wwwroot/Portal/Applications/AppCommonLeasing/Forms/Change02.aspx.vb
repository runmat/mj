Option Explicit On
Option Strict On

Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports System.Data.OleDb

Public Class Change02
    Inherits System.Web.UI.Page
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblErrorDetails As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles

    Protected WithEvents lb_weiter As LinkButton
    Protected WithEvents lnkFahrzeughistorie As HyperLink

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App

    Protected WithEvents txtLeasingvertragsnummer As TextBox
    Protected WithEvents txtKennzeichen As TextBox
    Protected WithEvents txtSuchname As TextBox
    Protected WithEvents txtFahrgestellnummer As TextBox
    Protected WithEvents rb_Auswahl As RadioButtonList
    Protected WithEvents tblUpload As HtmlTable
    Protected WithEvents tblEinzelauswahl As HtmlTable
    Protected WithEvents info1 As HtmlGenericControl
    Protected WithEvents info2 As HtmlGenericControl
    Protected WithEvents lblExcelfile As System.Web.UI.WebControls.Label
    Protected WithEvents upFile As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents tr_Upload As HtmlTableRow


    Private mObjBriefanforderung As Briefanforderung



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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' Hier Benutzercode zur Seiteninitialisierung einfügen
        Try
            m_User = GetUser(Me)
            m_App = New Base.Kernel.Security.App(m_User) 'erzeugt ein App_objekt 
            ucHeader.InitUser(m_User)
            FormAuth(Me, m_User)
            lblError.Text = ""
            lblErrorDetails.Text = ""
            lnkFahrzeughistorie.Visible = False

            If Not IsPostBack Then
                GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")
                lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
                ucStyles.TitleText = lblHead.Text
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten: " & ex.Message
        End Try
    End Sub

    Private Sub lb_Weiter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_weiter.Click

        If tblEinzelauswahl.Visible = True Then
            doSubmit()
        Else
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
        End If


    End Sub

    Private Sub doSubmit()

        'Einzelauswahl gewählt
        If tblEinzelauswahl.Visible = True Then
            If (txtFahrgestellnummer.Text.Replace("*", "").Trim(" "c) = "" AndAlso txtKennzeichen.Text.Replace("*", "").Trim(" "c) = "" AndAlso txtLeasingvertragsnummer.Text.Replace("*", "").Trim(" "c) = "" AndAlso txtSuchname.Text.Replace("*", "").Trim(" "c) = "") Then
                lblError.Text = "Geben Sie bitte ein Suchkriterium ein"
            Else
                'Prüfen, ob Eingaben durch Komma getrennt wurden
                If (txtKennzeichen.Text & txtLeasingvertragsnummer.Text).Contains(",") = False Then
                    mObjBriefanforderung = New Briefanforderung(m_User, m_App, Session("AppId").ToString, Me.Session.SessionID, "")
                    mObjBriefanforderung.SucheFahrgestellnummer = txtFahrgestellnummer.Text.Trim(" "c)
                    mObjBriefanforderung.SucheKennzeichen = txtKennzeichen.Text.Trim(" "c)
                    mObjBriefanforderung.SucheLeasingvertragsnummer = txtLeasingvertragsnummer.Text.Trim(" "c)
                    mObjBriefanforderung.SucheSuchname = txtSuchname.Text.Trim(" "c)

                    FillBriefanforderung(mObjBriefanforderung)
                Else 'Kennzeichen oder Leasingvertragsnummer durch Komma getrennt: Tabelle erzeugen
                    'Zunächst prüfen, ob nur eine Textbox Kommata enthält, sonst Fehler
                    If txtKennzeichen.Text.Contains(",") AndAlso txtLeasingvertragsnummer.Text.Contains(",") Then
                        lblError.Text = "Mehrfacheingaben nur in jeweils einem Feld möglich."
                        Exit Sub
                    Else
                        'Tabelle erzeugen
                        Dim TempTable As New DataTable

                        mObjBriefanforderung = New Briefanforderung(m_User, m_App, Session("AppId").ToString, Me.Session.SessionID, "")



                        Dim tmpa As String()
                        If txtKennzeichen.Text.Contains(",") Then
                            txtKennzeichen.Text = Replace(txtKennzeichen.Text, " ", "")
                            tmpa = txtKennzeichen.Text.Split(",".ToCharArray)
                            TempTable.Columns.Add("LICENSE_NUM", GetType(System.String))

                        Else
                            txtLeasingvertragsnummer.Text = Replace(txtLeasingvertragsnummer.Text, " ", "")
                            tmpa = txtLeasingvertragsnummer.Text.Split(",".ToCharArray)
                            TempTable.Columns.Add("LIZNR", GetType(System.String))
                        End If


                        Dim tmpStr As String
                        Dim tmpStr2 As String
                        For Each tmpStr2 In tmpa
                            tmpStr = tmpStr2.Replace("*", "")

                            Dim TmpRow As DataRow = TempTable.NewRow

                            TmpRow(0) = tmpStr


                            TempTable.Rows.Add(TmpRow)

                        Next

                        TempTable.AcceptChanges()

                        If txtKennzeichen.Text.Contains(",") Then
                            mObjBriefanforderung.KennzeichenTable = TempTable
                        Else
                            mObjBriefanforderung.VertragsnummernTable = TempTable
                        End If

                        FillBriefanforderung(mObjBriefanforderung)

                    End If

                End If

            End If

        End If


    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
        Auswahl()
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Auswahl()

        If tr_Upload.Visible = True Then
            info1.Visible = False
            info2.Visible = False

            Select Case rb_Auswahl.SelectedItem.Value
                Case "1"
                    tblUpload.Visible = False
                    tblEinzelauswahl.Visible = True
                Case "2"
                    tblEinzelauswahl.Visible = False
                    tblUpload.Visible = True
                    info1.Visible = True
                    ClearEinzelauswahl()

                Case "3"
                    tblEinzelauswahl.Visible = False
                    tblUpload.Visible = True
                    info2.Visible = True
                    ClearEinzelauswahl()
            End Select
        End If


    End Sub

    Private Sub ClearEinzelauswahl()

        txtFahrgestellnummer.Text = ""
        txtKennzeichen.Text = ""
        txtLeasingvertragsnummer.Text = ""
        txtSuchname.Text = ""

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

                Dim tblTemp As DataTable = objDataset1.Tables(0)

                objConn.Close()

                If Not tblTemp.Rows Is Nothing AndAlso tblTemp.Rows.Count > 0 Then
                    mObjBriefanforderung = New Briefanforderung(m_User, m_App, Session("AppId").ToString, Me.Session.SessionID, "")

                    If rb_Auswahl.SelectedItem.Value = "2" Then

                        'Kennzeichen
                        Dim tblTemp2 As New DataTable
                        tblTemp2.Columns.Add("LICENSE_NUM", GetType(System.String))

                        For i As Integer = 0 To (tblTemp.Rows.Count - 1)
                            Dim strWert As String = tblTemp.Rows(i)(0).ToString()
                            If Not String.IsNullOrEmpty(strWert) Then
                                Dim newRow As DataRow = tblTemp2.NewRow()
                                newRow("LICENSE_NUM") = strWert
                                tblTemp2.Rows.Add(newRow)
                            End If
                        Next

                        mObjBriefanforderung.KennzeichenTable = tblTemp2

                    Else

                        'Kennzeichen
                        Dim tblTemp2 As New DataTable
                        tblTemp2.Columns.Add("LIZNR", GetType(System.String))

                        For i As Integer = 0 To (tblTemp.Rows.Count - 1)
                            Dim strWert As String = tblTemp.Rows(i)(0).ToString()
                            If Not String.IsNullOrEmpty(strWert) Then
                                Dim newRow As DataRow = tblTemp2.NewRow()
                                newRow("LIZNR") = strWert
                                tblTemp2.Rows.Add(newRow)
                            End If
                        Next

                        mObjBriefanforderung.VertragsnummernTable = tblTemp2

                    End If
                        FillBriefanforderung(mObjBriefanforderung)

                Else
                    lblError.Text = "Datei enthielt keine verwendbaren Daten."
                End If
            End If
        Catch ex As Exception
            lblError.Text = ex.Message
        Finally
        End Try
    End Sub

    Private Sub FillBriefanforderung(ByVal mObjBriefanforderung As Briefanforderung)

        lblError.Text = ""
        mObjBriefanforderung.Show()
        If mObjBriefanforderung.Status = 0 Then
            Session.Add("mObjBriefanforderungSession", mObjBriefanforderung)
            If mObjBriefanforderung.Fahrzeuge.Rows.Count = 1 Then
                If mObjBriefanforderung.Fahrzeuge.Rows(0)("FEHLER").ToString.Trim <> "" Then
                    lblError.Text = "Dokument kann nicht versendet werden! Ursache: " & mObjBriefanforderung.Fahrzeuge.Rows(0)("FEHLER").ToString.Trim
                    Exit Sub
                End If
            End If
            Dim Parameterlist As String = ""
            HelpProcedures.getAppParameters(Session("AppID").ToString, Parameterlist, ConfigurationManager.AppSettings.Get("Connectionstring"))
            Response.Redirect("Change02_1.aspx?AppID=" & Session("AppID").ToString & Parameterlist)
        Else
            lblError.Text = mObjBriefanforderung.Message
            Select Case mObjBriefanforderung.Status
                Case -7777
                    lblErrorDetails.Text = "<b>Mögliche Ursachen können sein:</b><br><br>" & _
                                        " - Eingabefehler<br>" & _
                                         " - Dokument nicht ausgelagert<br>" & _
                                         " - Änderung der Vertragsdaten<br><br>" & _
                                        " <b>Bitte prüfen Sie alternative Suchkriterien.</b>"

                Case -8888
                    lblErrorDetails.Text = "<b>Mögliche Ursachen können sein:</b><br><br>" & _
                                            " - Dokument ist bereits versendet<br>" & _
                                            " - Dokument ist bereits angefordert<br>" & _
                                            " - Änderung der Vertragsdaten<br><br>" & _
                                            " <b>Bitte prüfen Sie alternative Suchkriterien.</b><br>" & _
                                            " <b>Details zum Status finden Sie in der </b>"

                    If txtFahrgestellnummer.Text.Length = 17 Then
                        lnkFahrzeughistorie.NavigateUrl = "../../../Components/ComCommon/Finance/Report46.aspx?AppID=" & m_User.Applications.Select("AppName = 'Report46'")(0)("AppID").ToString & "&VIN=" & txtFahrgestellnummer.Text
                    Else
                        lnkFahrzeughistorie.NavigateUrl = "../../../Components/ComCommon/Finance/Report46.aspx?AppID=" & m_User.Applications.Select("AppName = 'Report46'")(0)("AppID").ToString
                    End If
                    lnkFahrzeughistorie.Visible = True


            End Select

            Exit Sub
        End If


    End Sub

End Class
' ************************************************
' $History: Change02.aspx.vb $
' 
' *****************  Version 13  *****************
' User: Rudolpho     Date: 22.12.09   Time: 14:42
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' ITA: 3408
' 
' *****************  Version 12  *****************
' User: Fassbenders  Date: 17.09.09   Time: 12:51
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' 
' *****************  Version 11  *****************
' User: Fassbenders  Date: 14.09.09   Time: 11:48
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' 
' *****************  Version 10  *****************
' User: Fassbenders  Date: 10.09.09   Time: 15:16
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' ITA: 3050, 3056
' 
' *****************  Version 9  *****************
' User: Fassbenders  Date: 8.09.09    Time: 15:47
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' 
' *****************  Version 8  *****************
' User: Fassbenders  Date: 8.09.09    Time: 13:37
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' ITA: 3056
' 
' *****************  Version 7  *****************
' User: Fassbenders  Date: 31.08.09   Time: 16:27
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 18.03.09   Time: 8:51
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' ita 2656 testfertig 
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 17.03.09   Time: 17:26
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' ITA 2656 unfertig
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 6.11.08    Time: 13:34
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' ITA 2365,2367,2362
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 22.10.08   Time: 11:15
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' ITA 2284 Weiterentwicklung
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 21.10.08   Time: 17:11
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' ITA 2284 weiterentwicklung
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 17.10.08   Time: 10:44
' Created in $/CKAG/Applications/AppCommonLeasing/Forms
' ITA 2284 torso
' 
' ************************************************