Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Public Class Change03
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
    Private mObjVertragsdatenaenderung As Vertragsdatenaenderung
    'Private m_blnDoSubmit As Boolean
    'Private mstrHDL As String
    Protected WithEvents Hyperlink1 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkLogout As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents Literal1 As System.Web.UI.WebControls.Literal
    Protected WithEvents lbl_LIZNR As System.Web.UI.WebControls.Label
    Protected WithEvents txt_LIZNR As System.Web.UI.WebControls.TextBox
    Protected WithEvents tr_LIZNR As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cmdSearch.Visible = True
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User, True)
        GetAppIDFromQueryString(Me)
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If IsPostBack = False Then

                mObjVertragsdatenaenderung = New Vertragsdatenaenderung(m_User, m_App, "")
                Session.Add("mObjVertragsdatenaenderungSession", mObjVertragsdatenaenderung)
                tr_LIZNR.Visible = True
            Else 'wenn Postback
                If mObjVertragsdatenaenderung Is Nothing Then
                    mObjVertragsdatenaenderung = CType(Session("mObjVertragsdatenaenderungSession"), Vertragsdatenaenderung)
                End If
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        DoSubmit()
    End Sub

    Private Sub DoSubmit()

        lblError.Text = ""
        lblError.Visible = False

        mObjVertragsdatenaenderung.SucheLiznr = txt_LIZNR.Text.ToUpper

        If Not pruefung() Then
            lblError.Text = "geben Sie mindestens ein Suchkriterium ein"
            lblError.Visible = True
            Exit Sub
        End If


        mObjVertragsdatenaenderung.Fill(Session("AppId").ToString, Session.SessionID)

        If Not mObjVertragsdatenaenderung.Status = 0 Then
            lblError.Text = mObjVertragsdatenaenderung.Message
            lblError.Visible = True

        Else
            If mObjVertragsdatenaenderung.Result.Rows.Count = 0 Then
                lblError.Text = "Zu den gewählten Kriterien wurden keine Ergebnisse gefunden."
                lblError.Visible = True
            Else
                Session("ObjVertragsdatenaenderungSession") = mObjVertragsdatenaenderung
                Response.Redirect("Change03_1.aspx?AppID=" & Session("AppID").ToString)
            End If
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Function pruefung() As Boolean

        With mObjVertragsdatenaenderung
            If Not .sucheLIZNR Is Nothing AndAlso Not .sucheLIZNR.Trim = "" Then
                Return True
            End If
            Return False
        End With
    End Function
End Class

' ************************************************
' $History: Change03.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 11.08.08   Time: 17:54
' Updated in $/CKAG/Applications/AppBPLG/Forms
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 31.07.08   Time: 13:53
' Updated in $/CKAG/Applications/AppBPLG/Forms
' Doppeltes PageLoad entfernt
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 23.07.08   Time: 9:57
' Updated in $/CKAG/Applications/AppBPLG/Forms
' ITA 2101 testfertig
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 22.07.08   Time: 14:32
' Created in $/CKAG/Applications/AppBPLG/Forms
' ITA 2101 Rohversion
' ************************************************
