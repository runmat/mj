Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG
Imports System.Data.OleDb

Public Class Change01_1
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private objSuche As Search
    Private objHaendler As F1_Haendler

    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.LinkButton
    Protected WithEvents txtFahrgestellNr As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lbl_Fahrgestellnummer As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_ZBII As System.Web.UI.WebControls.Label
    Protected WithEvents txt_ZBII As System.Web.UI.WebControls.TextBox
    Protected WithEvents tr_Fahrgestellnummer As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents Kopfdaten1 As AppF1.Kopfdaten
    Protected WithEvents upFile As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents lblExcelfile As System.Web.UI.WebControls.Label


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
        cmdSearch.Visible = True
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If Request.QueryString("HDL") = "1" Then ' Kontingente ausblenden?
                Session("AppShowNot") = True
            End If

            If (Session("objSuche") Is Nothing) Then
                Throw New Exception("Fehlendes Session Objekt")
            Else
                objSuche = CType(Session("objSuche"), Search)
            End If

            Kopfdaten1.UserReferenz = m_User.Reference
            Kopfdaten1.HaendlerNummer = objSuche.REFERENZ
            Kopfdaten1.SapInterneHaendlerNummer = objSuche.SapInterneHaendlerReferenzNummer
            Dim strTemp As String = objSuche.NAME
            If objSuche.NAME_2.Length > 0 Then
                strTemp &= "<br>" & objSuche.NAME_2
            End If
            Kopfdaten1.HaendlerName = strTemp
            Kopfdaten1.Adresse = objSuche.COUNTRYISO & " - " & objSuche.POSTL_CODE & " " & objSuche.CITY & "<br>" & objSuche.STREET
            Kopfdaten1.Kontingente = objSuche.Kontingente


            If (Session("objHaendler") Is Nothing) OrElse (Not IsPostBack) Then
                objHaendler = New F1_Haendler(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "", "60" & objSuche.REFERENZ)
                objHaendler.GetDienstleister(Session("AppID").ToString, Session.SessionID)
            Else
                objHaendler = CType(Session("objHaendler"), F1_Haendler)
            End If
            'händlernummer
            objHaendler.Customer = objSuche.REFERENZ

            Session("objHaendler") = objHaendler
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        DoSubmit()
    End Sub

    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        DoSubmit()
    End Sub


    Private Function LoadExcelFile() As DataTable
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
        End If
        'Lade Datei
        Return getTableFromExcel(upFile.PostedFile)
    End Function

    Private Function getTableFromExcel(ByVal uFile As System.Web.HttpPostedFile) As DataTable
        Dim tmpTable As New DataTable

        Try
            Dim filepath As String = ConfigurationManager.AppSettings("UploadpathLocal")
            Dim filename As String
            Dim info As System.IO.FileInfo

            'Dateiname: User_yyyyMMddhhmmss.xls
            filename = m_User.UserName & "_" & Format(Now, "yyyyMMddhhmmss") & ".xls"

            If Not (uFile Is Nothing) Then
                uFile.SaveAs(ConfigurationManager.AppSettings("UploadpathLocal") & filename)
                uFile = Nothing
                info = New System.IO.FileInfo(filepath & filename)
                If Not (info.Exists) Then
                    lblError.Text = "Fehler beim Speichern."
                    Return Nothing
                    Exit Function
                End If
                'Datei gespeichert -> Auswertung
                tmpTable = getDataTableFromExcel(filepath, filename)
            End If
        Catch ex As Exception
            tmpTable = Nothing
            lblError.Text = ex.Message & " / " & ex.StackTrace
        End Try

        Return tmpTable
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

    Private Sub DoSubmit()
        lblError.Text = ""
        lblError.Visible = False
        Kopfdaten1.Message = ""

        Dim tmpTable = LoadExcelFile()

        If Not tmpTable Is Nothing Then
            'user hat eine excel hochgeladen
            'suche über den kompletten bestand
            txtFahrgestellNr.Text = ""
            txt_ZBII.Text = ""
            objHaendler.MassenAnforderung = tmpTable
        End If

        objHaendler.SucheFahrgestellNr = Replace(txtFahrgestellNr.Text, "%", "*")
        objHaendler.SucheBriefNr = Replace(txt_ZBII.Text, "%", "*")

        objHaendler.KUNNR = m_User.KUNNR
        objHaendler.SucheHaendlernummer = objSuche.REFERENZ
        objHaendler.GiveCars(Session("AppID").ToString, Session.SessionID)

        If Not objHaendler.Status = 0 Then
            lblError.Text = objHaendler.Message
            lblError.Visible = True
        Else
            If objHaendler.Fahrzeuge.Rows.Count = 0 Then
                Kopfdaten1.Message = "Zu den gewählten Kriterien wurden keine Ergebnisse gefunden."
            Else
                Session("objHaendler") = objHaendler
                Response.Redirect("Change01_2.aspx?AppID=" & Session("AppID").ToString)
            End If
        End If

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

End Class
' ************************************************
' $History: Change01_1.aspx.vb $
' 
' *****************  Version 9  *****************
' User: Fassbenders  Date: 4.01.11    Time: 14:33
' Updated in $/CKAG/Applications/AppF1/forms
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 30.04.09   Time: 11:39
' Updated in $/CKAG/Applications/AppF1/forms
' ITA 2837
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 28.04.09   Time: 13:18
' Updated in $/CKAG/Applications/AppF1/forms
' ITA 2823 testfertig
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 9.04.09    Time: 14:45
' Updated in $/CKAG/Applications/AppF1/forms
' Nachbesserungen dokumentenversand
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 30.03.09   Time: 9:10
' Updated in $/CKAG/Applications/AppF1/forms
' ITA 2664 nachbesserungen
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 9.03.09    Time: 14:51
' Updated in $/CKAG/Applications/AppF1/forms
' 2664 testfertig
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 5.03.09    Time: 15:52
' Updated in $/CKAG/Applications/AppF1/forms
' ita 2664
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 4.03.09    Time: 17:30
' Updated in $/CKAG/Applications/AppF1/forms
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 4.03.09    Time: 11:12
' Created in $/CKAG/Applications/AppF1/forms
' 
' ************************************************