
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common
Imports System.IO

<CLSCompliant(False)> Public Class _Report04
    Inherits System.Web.UI.Page

    Protected WithEvents ucStyles As Styles
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    'Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Protected WithEvents cmdBack As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ShowScript As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents rdbSplit As System.Web.UI.WebControls.RadioButtonList
    'Private booStartpunkt As Boolean

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
        m_User = GetUser(Me)

        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text
        
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim dirInfo As DirectoryInfo
        Dim filInfo As FileInfo()
        Dim filSingle As FileInfo
        Dim strPath As String

        strPath = ConfigurationManager.AppSettings("SaveSplitPDF")
        
        Try

            dirInfo = New DirectoryInfo(strPath)
            
            filInfo = dirInfo.GetFiles

            If filInfo.Length > 0 Then
                If rdbSplit.Items.FindByValue(1).Selected = True Then
                    lblError.Text = "Startpunkt wurde bereits gesetzt."

                Else
                    For Each filSingle In filInfo


                        filSingle.Delete()

                    Next

                    lblError.Text = "Startpunkt zurückgesetzt."

                End If
            Else
                If rdbSplit.Items.FindByValue(1).Selected = True Then

                    filSingle = New FileInfo(strPath & "startsplit")

                    filSingle.Create.Close()

                    lblError.Text = "Startpunkt gesetzt."
                    
                Else
                    lblError.Text = "Keine Datei zum Zurücksetzen vorhanden."

                End If

            End If
            
        Catch ex As Exception
            lblError.Text = "Beim Speichern ist ein Fehler aufgetreten."
        End Try
    End Sub

    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBack.Click
        Response.Redirect("/Portal/(S(" & Session.SessionID & "))/Start/Selection.aspx")
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: _Report04.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 30.04.09   Time: 9:25
' Updated in $/CKAG/Applications/appdcl/Forms
' ITA: 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:00
' Created in $/CKAG/Applications/appdcl/Forms
' 
' *****************  Version 2  *****************
' User: Uha          Date: 21.06.07   Time: 12:36
' Updated in $/CKG/Applications/AppDCL/AppDCLWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 1  *****************
' User: Uha          Date: 3.05.07    Time: 10:24
' Created in $/CKG/Applications/AppDCL/AppDCLWeb/Forms
' Zusammenführung auf Stand 02.05.2007 Mittags
' 
' ************************************************