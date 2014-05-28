Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common


Public Class Report201_4
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

    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private m_objTable As DataTable

    Protected WithEvents Hyperlink2 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblFahrzeugkennzeichen As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    Protected WithEvents Label6 As System.Web.UI.WebControls.Label
    Protected WithEvents Label8 As System.Web.UI.WebControls.Label
    Protected WithEvents Label10 As System.Web.UI.WebControls.Label
    Protected WithEvents Label9 As System.Web.UI.WebControls.Label
    Protected WithEvents Label11 As System.Web.UI.WebControls.Label
    Private strKennzeichen As String


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)

        Try
            Dim AppName As String = Me.Request.Url.LocalPath
            AppName = Left(AppName, InStrRev(AppName, ".") - 1)
            AppName = Right(AppName, Len(AppName) - InStrRev(AppName, "/"))
            AppName = "AppName='" & AppName & "'"
            If Not AppName = "Selection" AndAlso _
               m_User.Applications.Select(AppName).Length = 0 Then
                Me.lblError.Visible = True
                'Me.Response.Redirect("../../../Start/Selection.aspx")
            Else
                If (Request.QueryString.Item("strKennzeichen") Is Nothing) Then
                    Me.lblError.Visible = True
                    'Me.Response.Redirect("../../../Start/Selection.aspx")
                Else
                    strKennzeichen = Request.QueryString.Item("strKennzeichen")
                    If (Session("ResultTable") Is Nothing) Then
                        Me.lblError.Visible = True
                        'Me.Response.Redirect("../../../Start/Selection.aspx")
                    Else
                        m_objTable = CType(Session("ResultTable"), DataTable)

                        m_App = New Base.Kernel.Security.App(m_User)

                        If Not IsPostBack Then
                            Dim rows As DataRow() = m_objTable.Select("[Kfz-Kennzeichen]='" & strKennzeichen & "'")
                            If rows.Length = 1 Then
                                If Not rows(0)("Kfz-Kennzeichen") Is Nothing AndAlso CStr(rows(0)("Kfz-Kennzeichen")).Trim.Length > 0 Then
                                    lblFahrzeugkennzeichen.Text = CStr(rows(0)("Kfz-Kennzeichen")) & ","
                                End If
                            Else
                                Me.lblError.Visible = True
                            End If
                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            Me.lblError.Visible = True
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
' $History: Report201_4.aspx.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:07
' Created in $/CKAG/Applications/appecan/Forms
' 
' *****************  Version 4  *****************
' User: Uha          Date: 2.07.07    Time: 12:32
' Updated in $/CKG/Applications/AppECAN/AppECANWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 3  *****************
' User: Uha          Date: 7.03.07    Time: 11:16
' Updated in $/CKG/Applications/AppECAN/AppECANWeb/Forms
' 
' ************************************************
