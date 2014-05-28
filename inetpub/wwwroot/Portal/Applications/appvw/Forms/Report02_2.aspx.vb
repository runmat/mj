Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Report02_2
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

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App

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
        GetAppIDFromQueryString(Me)

        lnkBack.NavigateUrl = "Report02.aspx?AppID=" & Session("AppID")
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)
            filltable()

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub filltable()
        Dim dirInfo As System.IO.DirectoryInfo
        Dim fInfo As System.IO.FileInfo()
        Dim trString As String
        Dim path As String
        Dim i As Integer

        path = Request.PhysicalApplicationPath & "\Applications\AppVW\Docs\Lastschrift"
        dirInfo = New System.IO.DirectoryInfo(path)
        fInfo = dirInfo.GetFiles("*.*")
        trString = String.Empty

        If (fInfo.Length > 0) Then
            For i = 0 To fInfo.Length - 1
                trString &= "<tr nowrap=""nowrap"">"
                trString &= "<td class=""GridTableHead"" noWrap align=""center"" width=""200"">" & Left(fInfo(i).Name, fInfo(i).Name.IndexOf(".")) & "</td>"
                trString &= "<td noWrap align=""left"">&nbsp;<a href=""" & "\Portal\Applications\AppVW\Docs\Lastschrift\" & fInfo(i).Name & """ target=""_blank"">Download</a></TD><tr>"
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
' $History: Report02_2.aspx.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 17:48
' Created in $/CKAG/Applications/appvw/Forms
' 
' *****************  Version 8  *****************
' User: Uha          Date: 20.06.07   Time: 14:32
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 7  *****************
' User: Uha          Date: 8.03.07    Time: 15:06
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Forms
' 
' ************************************************
