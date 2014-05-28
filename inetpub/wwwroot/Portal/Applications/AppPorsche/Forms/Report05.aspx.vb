Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports System.IO


Public Class Report05
    Inherits System.Web.UI.Page
    Implements IComparer

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

    Protected WithEvents ucHeader As Header
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents tblFiles As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents lblInfo As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            DoSubmit()
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
        ' make it return this in descending order (newest first) 
        Return -DateTime.Compare(DirectCast(x, FileInfo).CreationTime, DirectCast(y, FileInfo).CreationTime)
    End Function

    Private Function getFiles() As ArrayList
        'Dateien holen
        Dim dirInfo As DirectoryInfo
        Dim fileInfo As FileInfo()
        Dim fileInfoCopy As FileInfo

        Dim strPath As String
        Dim strPathDownload As String
        Dim arrFileList As New ArrayList()
        Dim intCounter As Integer

        strPath = System.Configuration.ConfigurationManager.AppSettings("DownloadPath")
        strPathDownload = System.Configuration.ConfigurationManager.AppSettings("DownloadPathWeb")

        Try
            dirInfo = New DirectoryInfo(strPath)
            fileInfo = dirInfo.GetFiles("*.TXT")    'Nur Textdateien...


            If fileInfo.Length = 0 Then
                lblError.Text = "Keine Dateien gefunden."
            Else
                For intCounter = 0 To fileInfo.Length - 1
                    'Dateien auf Webserver kopieren...
                    fileInfoCopy = fileInfo(intCounter).CopyTo(strPathDownload & fileInfo(intCounter).Name, True)
                    arrFileList.Add(fileInfoCopy)
                Next
                lblInfo.Text = "Es wurde(n) " & fileInfo.Length & " Datei(en) gefunden."
            End If

            arrFileList.Sort(Me)

            Return arrFileList

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten."
            Return Nothing
        End Try
    End Function
    Public Function MakeDateTimeStandard(ByVal strInput As String) As Date
        Dim strTemp As String = Mid(strInput, 7, 2) & "." & Mid(strInput, 5, 2) & "." & Left(strInput, 4) & " " & _
        Mid(strInput, 9, 2) & ":" & Mid(strInput, 11, 2) & ":" & Right(strInput, 2)


        If IsDate(strTemp) Then
            Return CDate(strTemp)
        Else
            Return CDate("01.01.1900")
        End If
    End Function
    Private Sub setFiles(ByVal arrFiles As ArrayList)
        'Ausgabe auf Webform
        Dim intCount As Integer
        Dim tblOutputTable As System.Web.UI.HtmlControls.HtmlTable
        Dim tblOutputTableCell As System.Web.UI.HtmlControls.HtmlTableCell
        Dim tblOutputTableRow As System.Web.UI.HtmlControls.HtmlTableRow
        Dim lnkText As HyperLink
        Dim strFilename As String
        Dim fileInfo As FileInfo


        Dim strFilePath As String

        strFilePath = System.Configuration.ConfigurationManager.AppSettings("DownloadPathWebView")

        tblOutputTable = tblFiles
        For intCount = 0 To arrFiles.Count - 1

            fileInfo = CType(arrFiles(intCount), FileInfo)
            strFilename = fileInfo.DirectoryName & "\" & fileInfo.Name & ";" & fileInfo.CreationTime.ToString

            lnkText = New HyperLink()
            'lnkText.NavigateUrl = Left(strFilename, strFilename.IndexOf(";"))
            'lnkText.NavigateUrl = strFilePath & Right(lnkText.NavigateUrl, lnkText.NavigateUrl.Length - lnkText.NavigateUrl.LastIndexOf("\") - 1)
            'SFA 27.02.2007
            lnkText.NavigateUrl = Replace(strFilePath, "\", "/") & fileInfo.Name



            lnkText.Target = "_blank"
            'lnkText.Text = CStr(intCount + 1) & ".)&nbsp;&nbsp;&nbsp;" & Right(strFilename, strFilename.Length - strFilename.IndexOf(";") - 1) & " - " & Right(lnkText.NavigateUrl, lnkText.NavigateUrl.Length - lnkText.NavigateUrl.LastIndexOf("\") - 1).ToUpper
            'lnkText.Text = CStr(intCount + 1) & ".)&nbsp;&nbsp;&nbsp;" & Format(Now(), "dd.MM.yyyy hh:mm:ss") & " - " & fileInfo.Name.ToUpper

            strFilename = Left(Right(strFilename, 38), 14)
            strFilename = MakeDateTimeStandard(strFilename)

            lnkText.Text = CStr(intCount + 1) & ".)&nbsp;&nbsp;&nbsp;" & _
                           strFilename & " - Lastschrift"

            'lnkText.NavigateUrl = "file://" & lnkText.NavigateUrl.Replace("\"c, "/"c)
            lnkText.CssClass = "DownloadLink"

            'Spalte
            tblOutputTableCell = New System.Web.UI.HtmlControls.HtmlTableCell()
            tblOutputTableCell.Controls.Add(lnkText)
            'Zeile
            tblOutputTableRow = New System.Web.UI.HtmlControls.HtmlTableRow()
            tblOutputTableRow.Cells.Add(tblOutputTableCell)

            tblOutputTable.Rows.Add(tblOutputTableRow)

            'Spalte
            tblOutputTableCell = New System.Web.UI.HtmlControls.HtmlTableCell()
            tblOutputTableCell.InnerText = " "
            'Zeile
            tblOutputTableRow = New System.Web.UI.HtmlControls.HtmlTableRow()
            tblOutputTableRow.Cells.Add(tblOutputTableCell)

            tblOutputTable.Rows.Add(tblOutputTableRow)
            tblOutputTable.Rows.Add(tblOutputTableRow)
        Next
    End Sub

    Private Sub DoSubmit()
        Dim arrFiles As ArrayList
        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)

        logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, "", "Download Lastschriftdatei (Benutzer: " & m_User.UserName & ")", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser)
        Try
            arrFiles = getFiles()
            If arrFiles.Count > 0 Then
                setFiles(arrFiles)
            End If
        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
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
' $History: Report05.aspx.vb $
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 4.05.09    Time: 9:21
' Updated in $/CKAG/Applications/AppPorsche/Forms
' ITA: 2837
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 9.04.09    Time: 13:58
' Updated in $/CKAG/Applications/AppPorsche/Forms
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 5.06.08    Time: 14:23
' Updated in $/CKAG/Applications/AppPorsche/Forms
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 24.04.08   Time: 16:29
' Updated in $/CKAG/Applications/AppPorsche/Forms
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 11:28
' Created in $/CKAG/Applications/AppPorsche/Forms
' 
' *****************  Version 14  *****************
' User: Fassbenders  Date: 6.12.07    Time: 17:16
' Updated in $/CKG/Applications/AppPorsche/AppPorscheWeb/Forms
' 
' *****************  Version 13  *****************
' User: Fassbenders  Date: 5.12.07    Time: 15:03
' Updated in $/CKG/Applications/AppPorsche/AppPorscheWeb/Forms
' 
' *****************  Version 12  *****************
' User: Uha          Date: 2.07.07    Time: 13:27
' Updated in $/CKG/Applications/AppPorsche/AppPorscheWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 11  *****************
' User: Uha          Date: 5.03.07    Time: 13:54
' Updated in $/CKG/Applications/AppPorsche/AppPorscheWeb/Forms
' 
' *****************  Version 10  *****************
' User: Uha          Date: 5.03.07    Time: 12:50
' Updated in $/CKG/Applications/AppPorsche/AppPorscheWeb/Forms
' 
' ************************************************
