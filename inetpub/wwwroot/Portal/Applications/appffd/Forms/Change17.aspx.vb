Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business

' Anfroderungsnummer 968
' Erstellt am: 17.04.2007 - Tim Bardenhagen
Public Class Change17
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

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents suche1 As Suche
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim strLastRecordParam As String = ""
        Try
            m_User = GetUser(Me)
            FormAuth(Me, m_User)
            NoDealer(Me, m_User)
            m_App = New Base.Kernel.Security.App(m_User)

            ucHeader.InitUser(m_User)
            suche1.NurFilialSuche = False
            suche1.AllowEmptySearch = False
            If (Not Request.QueryString("ID") Is Nothing) AndAlso (Request.QueryString("ID").Length > 0) Then
                If CStr(Request.QueryString("LastRecord")) = "True" Then
                    strLastRecordParam = "&LastRecord=True"
                End If
                suche1.RedirectUrl = "Change17Edit.aspx?ID=" & Request.QueryString("ID").ToString & strLastRecordParam
            Else
                If CStr(Request.QueryString("LastRecord")) = "True" Then
                    strLastRecordParam = "?LastRecord=True"
                End If
                suche1.RedirectUrl = "Change17Edit.aspx" & strLastRecordParam
            End If
            suche1.User = m_User

            GetAppIDFromQueryString(Me)

            If Not IsPostBack Then
                If (Not Session("Authorization") Is Nothing) AndAlso (CBool(Session("Authorization"))) Then
                    If (Not Session("AuthorizationID") Is Nothing) AndAlso CInt(Session("AuthorizationID")) > 0 Then
                        Try
                            Response.Redirect(suche1.RedirectUrl)
                        Catch
                        End Try
                    End If
                End If
            End If
            suche1.Headline = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = suche1.Headline

        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "Change17", "Page_Load", ex.ToString)

            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
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
' $History: Change17.aspx.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:23
' Created in $/CKAG/Applications/appffd/Forms
' 
' *****************  Version 3  *****************
' User: Uha          Date: 2.07.07    Time: 12:52
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 15.06.07   Time: 8:11
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' 
' *****************  Version 1  *****************
' User: Uha          Date: 3.05.07    Time: 18:05
' Created in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' Änderungen aus StartApplication vom 02.05.2007 Mittags übernommen
' 
' ************************************************
