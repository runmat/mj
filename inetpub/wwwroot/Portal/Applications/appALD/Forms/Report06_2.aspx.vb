Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Report06_2
    Inherits System.Web.UI.Page
    Protected WithEvents grdResult As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private m_User As Base.Kernel.Security.User

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)

        GetAppIDFromQueryString(Me)
        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text
        Try
            grdResult.DataSource = Session("ResultTable")
            grdResult.DataBind()
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try

        'Alter Inhalt der Sub ohne GetUser
        'UH: 20.06.2007
        'If Not Session("objUser") Is Nothing Then
        '    Dim m_User As Base.Kernel.Security.User
        '    m_User = CType(Session("objUser"), Base.Kernel.Security.User)
        '    lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        '    ucStyles.TitleText = lblHead.Text
        '    grdResult.DataSource = Session("ResultTable")
        '    grdResult.DataBind()
        'End If
    End Sub


    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Report06_2.aspx.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 17.04.08   Time: 15:15
' Created in $/CKAG/Applications/appald/Forms
' 
' *****************  Version 4  *****************
' User: Uha          Date: 20.06.07   Time: 15:25
' Updated in $/CKG/Applications/AppALD/AppALDWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 3  *****************
' User: Uha          Date: 5.03.07    Time: 16:59
' Updated in $/CKG/Applications/AppALD/AppALDWeb/Forms
' 
' ************************************************
