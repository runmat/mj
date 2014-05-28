Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Report99_2
    Inherits System.Web.UI.Page
    '##### VW Report "Klärfallreport"
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

    Private m_User As Security.User
    Private m_App As Security.App

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents ImageButton1 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents tblContent As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents lblContent As System.Web.UI.WebControls.Label
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        Dim strAppID As String = String.Empty
        If Request.QueryString("AppID").Length > 0 Then
            strAppID = Request.QueryString("AppID").ToString
            Session("AppID") = strAppID
            lnkBack.NavigateUrl = "Report99.aspx?AppID=" & strAppID
        End If

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text
        m_App = New Security.App(m_User)
        filltable()

    End Sub

    Private Sub filltable()
        Dim dirInfo As System.IO.DirectoryInfo
        Dim fInfo As System.IO.FileInfo()
        Dim trString As String
        Dim path As String
        Dim i As Integer


        path = Request.PhysicalApplicationPath & "Docs\Lastschrift"
        dirInfo = New System.IO.DirectoryInfo(path)
        fInfo = dirInfo.GetFiles("*.*")
        trString = String.Empty

        If (fInfo.Length > 0) Then
            For i = 0 To fInfo.Length - 1
                trString &= "<tr nowrap=""nowrap"">"
                trString &= "<td class=""GridTableHead"" noWrap align=""center"" width=""200"">" & Left(fInfo(i).Name, fInfo(i).Name.IndexOf(".")) & "</td>"
                trString &= "<td noWrap align=""left"">&nbsp;<a href=""" & "\Portal\Docs\Lastschrift\" & fInfo(i).Name & """ target=""_blank"">Download</a></TD><tr>"
            Next
            lblContent.Text = trString
            lblError.Text = String.Empty
        Else
            lblError.Text = "Keine Daten vorhanden."
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
' $History: Report99_2.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 5.06.09    Time: 15:58
' Updated in $/CKAG/Components/ComCommon
' mögliche try catches entfernt
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:32
' Created in $/CKAG/Components/ComCommon
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 10:47
' Created in $/CKAG/portal/Components/ComCommon
' 
' *****************  Version 7  *****************
' User: Uha          Date: 12.07.07   Time: 9:28
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 6  *****************
' User: Uha          Date: 12.03.07   Time: 10:17
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' In Pfadangabe alte Start-Anwendung durch "Portal" ersetzt
' 
' *****************  Version 5  *****************
' User: Uha          Date: 5.03.07    Time: 14:57
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' 
' ************************************************
