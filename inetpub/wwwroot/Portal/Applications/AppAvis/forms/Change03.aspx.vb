Option Explicit On
Option Strict On


Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports System.Data.OleDb


Partial Public Class Change03
    Inherits System.Web.UI.Page

    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lb_weiter As LinkButton
    Protected WithEvents upFile As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents lblExcelfile As System.Web.UI.WebControls.Label

    Protected WithEvents txtZulassungsdatum As TextBox
    Protected WithEvents txtMVANummer As TextBox
    Protected WithEvents txtFahrgestellnummer As TextBox




    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App

    Private m_context As HttpContext = HttpContext.Current
    Private mObjUploadZulassung As UploadZulassung




    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' Hier Benutzercode zur Seiteninitialisierung einfügen
        Try
            m_User = GetUser(Me)
            m_App = New Base.Kernel.Security.App(m_User) 'erzeugt ein App_objekt 
            ucHeader.InitUser(m_User)
            FormAuth(Me, m_User)
            lblError.Text = ""

            If Not IsPostBack Then
                GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")
                lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
                ucStyles.TitleText = lblHead.Text
            End If

        Catch ex As Exception
            lblError.Text = "beim laden der Seite ist ein Fehler aufgetreten: " & ex.Message & " / " & mObjUploadZulassung.Message
        End Try
    End Sub

    Private Sub lb_Weiter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_weiter.Click
        doSubmit()
    End Sub

    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        Response.Redirect("/Portal/(S(" & Session.SessionID & "))/Start/Selection.aspx")
    End Sub

    Private Sub doSubmit()




        Dim tmpZulassungsTable = LoadZulassungsFile()

        If Not tmpZulassungsTable Is Nothing Then
            Dim strTemp As String = ""
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"

            mObjUploadZulassung = New UploadZulassung(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, strFileName)
            mObjUploadZulassung.generateZulassungsTable(tmpZulassungsTable)

            If mObjUploadZulassung.Status = 0 Then
                HelpProcedures.getAppParameters(Session("AppID").ToString, strTemp, System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
                Session.Add("mObjUploadZulassungSession", mObjUploadZulassung)
                If rbDez.Checked Then
                    mObjUploadZulassung.DezZul = True
                Else
                    mObjUploadZulassung.DezZul = False
                End If
                Response.Redirect("Change03_1.aspx?AppID=" & Session("AppID").ToString & strTemp)
            Else
                lblError.Text = mObjUploadZulassung.Message
                Exit Sub
            End If
        End If
    End Sub

    Private Function LoadZulassungsFile() As DataTable
        'Prüfe Fehlerbedingung
        If (Not upFile.PostedFile Is Nothing) AndAlso (Not (upFile.PostedFile.FileName = String.Empty)) Then
            lblExcelfile.Text = upFile.PostedFile.FileName
            If Right(upFile.PostedFile.FileName.ToUpper, 4) <> ".XLS" Then
                lblError.Text = "Es können nur Dateien im .XLS - Format verarbeitet werden."
                Return Nothing
                Exit Function
            End If
            If (upFile.PostedFile.ContentLength > CType(ConfigurationManager.AppSettings("MaxUploadSize"), Integer)) Then
                lblError.Text = "Datei '" & upFile.PostedFile.FileName & "' ist zu gross (>300 KB)."
                Return Nothing
                Exit Function
            End If
        Else
            If txtFahrgestellnummer.Text.Trim(" "c).Length = 0 AndAlso txtMVANummer.Text.Trim(" "c).Length = 0 Then
                lblError.Text = "Wählen Sie einen Vorgang"
                Return Nothing
                Exit Function
            Else
                Return checkAndGenerateEinzelzulassung()
            End If
        End If
        'Lade Datei
        Return getZulassungen(upFile.PostedFile)
    End Function


    Private Function checkAndGenerateEinzelzulassung() As DataTable
        Dim Fahrgestellnummer As String = ""
        Dim MVANummer As String = ""
        Dim ErrorMessage As String = ""



        If Not HelpProcedures.checkDate(txtZulassungsdatum, ErrorMessage, False) OrElse CDate(txtZulassungsdatum.Text) < Today Then
            If ErrorMessage.Length = 0 Then
                'wenn keine errormessage dann vergangenheit
                ErrorMessage = "Zulassungsdatum darf nicht in der Vergangenheit liegen"
            End If
        End If


        If Not txtFahrgestellnummer.Text.Trim(" "c).Length = 0 AndAlso Not txtFahrgestellnummer.Text.Trim(" "c).Length = 17 Then
            ErrorMessage = "Die Fahrgestellnummer ist nicht korrekt"
        Else
            Fahrgestellnummer = txtFahrgestellnummer.Text.Trim(" "c)
        End If



        If Not txtMVANummer.Text.Trim(" "c).Length = 0 Then
            If Not txtMVANummer.Text.Trim(" "c).Length < 7 Then
                txtMVANummer.Text = Right("00000000" & txtMVANummer.Text, 8)
                MVANummer = txtMVANummer.Text.Trim(" "c)
            Else
                ErrorMessage = "Die MVA-Nummer ist nicht korrekt"
            End If
        End If

        If Not ErrorMessage.Length = 0 Then
            lblError.Text = ErrorMessage
            Return Nothing
        Else
            Dim tmpDataTable As New DataTable
            tmpDataTable.Columns.Add("Fahrgestellnummer", String.Empty.GetType)
            tmpDataTable.Columns.Add("MVANummer", String.Empty.GetType)
            tmpDataTable.Columns.Add("Zulassungsdatum", String.Empty.GetType)

            Dim tmpRow As DataRow = tmpDataTable.NewRow()
            'überschriftsspalte wie in EXCEL faken
            tmpRow(0) = "Überschrift"
            tmpRow(1) = "Überschrift"
            tmpRow(2) = "Überschrift"
            tmpDataTable.Rows.Add(tmpRow)

            tmpRow = tmpDataTable.NewRow
            tmpRow(0) = Fahrgestellnummer
            tmpRow(1) = MVANummer
            tmpRow(2) = txtZulassungsdatum.Text
            tmpDataTable.Rows.Add(tmpRow)
            tmpDataTable.AcceptChanges()
            Return tmpDataTable
        End If
    End Function



    Private Function getDataTableFromExcel(ByVal filepath As String, ByVal filename As String) As DataTable
        '----------------------------------------------------------------------
        ' Methode: GetDataTable
        ' Autor: JJU 
        ' Beschreibung: extrahiert die Daten aus dem ersten Exceltabellen-Blatt in eine Datatable
        ' Erstellt am: 2008.09.22
        ' ITA: 1844
        '----------------------------------------------------------------------

        Dim objDataset1 As New DataSet()
        Dim sConnectionString As String = "Provider=Microsoft.Jet.OLEDB.4.0;" & _
                         "Data Source=" & filepath & filename & ";Extended Properties=""Excel 8.0;HDR=No"""
        Dim objConn As New OleDbConnection(sConnectionString)
        objConn.Open()

        Dim schemaTable As DataTable
        Dim tmpObj() As Object = {Nothing, Nothing, Nothing, "Table"}
        schemaTable = objConn.GetOleDbSchemaTable(OleDb.OleDbSchemaGuid.Tables, tmpObj)

        For Each sheet As DataRow In schemaTable.Rows
            Dim tableName As String = sheet("Table_Name").ToString
            Dim objCmdSelect As New OleDbCommand("SELECT * FROM [" & tableName & "]", objConn)
            Dim objAdapter1 As New OleDbDataAdapter(objCmdSelect)
            objAdapter1.Fill(objDataset1, tableName)
        Next
        Dim tblTemp As DataTable = objDataset1.Tables(0)
        objConn.Close()
        Return tblTemp
    End Function

    Private Function getZulassungen(ByVal uFile As System.Web.HttpPostedFile) As DataTable
        Dim tmpTable As New DataTable
        Try
            Dim filepath As String = ConfigurationManager.AppSettings("ExcelPath")
            Dim filename As String
            Dim info As System.IO.FileInfo

            'Dateiname: User_yyyyMMddhhmmss.xls
            filename = m_User.UserName & "_" & Format(Now, "yyyyMMddhhmmss") & ".xls"

            If Not (uFile Is Nothing) Then
                uFile.SaveAs(ConfigurationManager.AppSettings("ExcelPath") & filename)
                uFile = Nothing
                info = New System.IO.FileInfo(filepath & filename)
                If Not (info.Exists) Then
                    tmpTable = Nothing
                    Throw New Exception("Fehler beim Speichern")
                End If
                'Datei gespeichert -> Auswertung
                tmpTable = getDataTableFromExcel(filepath, filename)
            End If
        Catch ex As Exception
            lblError.Text = ex.Message
        Finally
            getZulassungen = tmpTable
        End Try

    End Function

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

End Class

' ************************************************
' $History: Change03.aspx.vb $
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 6.05.10    Time: 14:15
' Updated in $/CKAG/Applications/AppAvis/forms
' ITA: 3696
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 30.10.09   Time: 11:14
' Updated in $/CKAG/Applications/AppAvis/forms
' ita�s: 3216, 3155
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 28.04.09   Time: 17:09
' Updated in $/CKAG/Applications/AppAvis/forms
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 31.03.09   Time: 11:48
' Updated in $/CKAG/Applications/AppAvis/forms
' ITA 2758
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 15.01.09   Time: 11:21
' Updated in $/CKAG/Applications/AppAvis/forms
' ITA 2457
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 21.11.08   Time: 9:59
' Updated in $/CKAG/Applications/AppAvis/forms
' ITA 2412 testfertig
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 20.11.08   Time: 14:08
' Created in $/CKAG/Applications/AppAvis/forms
' ITa 2412 torso
' 
' ************************************************