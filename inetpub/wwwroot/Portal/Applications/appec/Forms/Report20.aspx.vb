Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common
Imports System.Data

Public Class Report20
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
    Private objHaendler As ec_17

    Protected WithEvents lnkLogout As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents Literal1 As System.Web.UI.WebControls.Literal
    Protected WithEvents lblExcelfile As System.Web.UI.WebControls.Label
    Protected WithEvents cmdContinue As System.Web.UI.WebControls.LinkButton
    Protected WithEvents tblSelection As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents upFile As System.Web.UI.HtmlControls.HtmlInputFile
    Private versandart As String

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
                objHaendler = New ec_17(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                If Session("objHaendler") Is Nothing Then
                    Session.Add("objHaendler", objHaendler)
                Else
                    Session("objHaendler") = objHaendler
                End If

            Else
                objHaendler = CType(Session("objHaendler"), ec_17)
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten."
        End Try
    End Sub


    Private Sub upload(ByVal uFile As System.Web.HttpPostedFile, ByRef strFilename As String)
        Try
            Dim filepath As String = ConfigurationManager.AppSettings("ExcelPath")
            Dim filename As String
            Dim info As System.IO.FileInfo

            'Dateiname: User_yyyyMMddhhmmss.xls
            filename = m_User.UserName & "_" & Format(Now, "yyyyMMddhhmmss") & ".xls"

            If Not (uFile Is Nothing) Then
                uFile.SaveAs(filepath & filename)
                info = New System.IO.FileInfo(filepath & filename)
                If Not (info.Exists) Then
                    lblError.Text = "Fehler beim Speichern."
                    strFilename = String.Empty
                Else
                    strFilename = filepath & filename
                End If

            End If
        Catch ex As Exception
            lblError.Text = ex.Message
        Finally
        End Try
    End Sub

    Private Sub cmdContinue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdContinue.Click
        'Prüfe Fehlerbedingung
        Dim strFilename As String = ""
        Dim strExcelFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)

        If (Not upFile.PostedFile Is Nothing) AndAlso (Not (upFile.PostedFile.FileName = String.Empty)) Then
            If Right(upFile.PostedFile.FileName.ToUpper, 4) <> ".XLS" Then
                lblError.Text = "Es können nur Dateien im .XLS - Format verarbeitet werden."
            Else
                'Lade Datei
                upload(upFile.PostedFile, strFilename)
                If strFilename <> String.Empty Then
                    objHaendler.PUploadfile = strFilename
                    Dim objExcel As New Base.Kernel.DocumentGeneration.ExcelDocumentFactory()
                    Dim tblName As String = ""
                    objExcel.ReturnTableName(strFilename, tblName)

                    objHaendler.setData(tblName)
                    If objHaendler.Status <> 0 Then
                        lblError.Text = objHaendler.Message
                    Else
                        Session("objHaendler") = objHaendler
                        Session("ResultTable") = objHaendler.Result

                        Try
                            Excel.ExcelExport.WriteExcel(objHaendler.Result, ConfigurationManager.AppSettings("ExcelPath") & strExcelFileName)
                            Session("lnkExcel") = "/Portal/Temp/Excel/" & strExcelFileName
                            logApp.UpdateEntry("APP", Session("AppID").ToString, "Report: Fahrzeugstatus Zulassungen")
                        Catch
                        End Try
                    End If
                End If
            End If
            If objHaendler.Status <> 0 Then
                Exit Sub
            End If
            Response.Redirect("/Portal/(S(" & Session.SessionID & "))/Shared/Report01_2.aspx?AppID=" & Session("AppID").ToString)
        Else
            lblError.Text = "Keine Datei ausgewählt."
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
' $History: Report20.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 17.06.09   Time: 8:47
' Updated in $/CKAG/Applications/appec/Forms
' Warnungen entfernt!
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 15:49
' Created in $/CKAG/Applications/appec/Forms
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 10.08.07   Time: 11:26
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' Bugfix: Zugriff auf ExcelTabelle geändert AppEC- ec_17
' 
' *****************  Version 4  *****************
' User: Uha          Date: 2.07.07    Time: 12:29
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 21.06.07   Time: 8:50
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' ITA: 1050
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 20.06.07   Time: 15:44
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' ITA:1050 Bapi Z_V_FAHRZEUG_STATUS_001 eingefügt
' 
' *****************  Version 1  *****************
' User: Rudolpho     Date: 20.06.07   Time: 11:56
' Created in $/CKG/Applications/AppEC/AppECWeb/Forms
' ITA: 1050
' 
' ************************************************
