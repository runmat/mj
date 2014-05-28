Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Change02
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
    ' Private objSuche As CKG.Components.ComCommon.Finance.Search
    Private mObjBriefanforderung As Briefanforderung
    'Private m_blnDoSubmit As Boolean
    'Private mstrHDL As String
    Protected WithEvents Hyperlink1 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkLogout As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ucHeader As Header
    Protected WithEvents Textbox1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtFahrgestellNr As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents Literal1 As System.Web.UI.WebControls.Literal
    Protected WithEvents lbl_Fahrgestellnummer As System.Web.UI.WebControls.Label
    Protected WithEvents tr_FahrgestellNr As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lb_Link As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbl_TIDNR As System.Web.UI.WebControls.Label
    Protected WithEvents txtTIDNR As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtZZREFERENZ1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents lbl_ZZREFERENZ1 As System.Web.UI.WebControls.Label
    Protected WithEvents tr_TIDNR As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr_ZZREFERENZ1 As System.Web.UI.HtmlControls.HtmlTableRow
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
                If mObjBriefanforderung Is Nothing Then
                    If Session.Item("mObjBriefanforderungSession") Is Nothing Then
                        mObjBriefanforderung = New Briefanforderung(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "", m_User.Reference, m_User.KUNNR)
                        Session.Add("mObjBriefanforderungSession", mObjBriefanforderung)
                    Else
                        mObjBriefanforderung = CType(Session("mObjBriefanforderungSession"), Briefanforderung)
                    End If
                End If


                tr_FahrgestellNr.Visible = True
                tr_TIDNR.Visible = True
                tr_ZZREFERENZ1.Visible = True
                tr_LIZNR.Visible = True

            Else 'wenn Postback
                If mObjBriefanforderung Is Nothing Then
                    mObjBriefanforderung = CType(Session("mObjBriefanforderungSession"), Briefanforderung)
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


        mObjBriefanforderung.SucheTIDNR = txtTIDNR.Text.ToUpper
        mObjBriefanforderung.SucheZZREFERENZ1 = txtZZREFERENZ1.Text ' Ordernummer
        mObjBriefanforderung.SucheFahrgestellNr = Replace(txtFahrgestellNr.Text, "%", "*")
        mObjBriefanforderung.sucheLIZNR = txt_LIZNR.Text.ToUpper

        If Not pruefung() Then
            lblError.Text = "geben Sie mindestens ein Suchkriterium ein"
            lblError.Visible = True
            Exit Sub
        End If


        mObjBriefanforderung.GiveCars()
        If Not mObjBriefanforderung.Status = 0 Then
            lblError.Text = mObjBriefanforderung.Message
            lblError.Visible = True
        Else
            If mObjBriefanforderung.Fahrzeuge.Rows.Count = 0 Then
                lblError.Text = "Zu den gewählten Kriterien wurden keine Ergebnisse gefunden."
                lblError.Visible = True
            Else
                Session("mObjBriefanforderungSession") = mObjBriefanforderung
                Response.Redirect("Change02_2.aspx?AppID=" & Session("AppID").ToString)
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

        With mObjBriefanforderung
            If Not .SucheFahrgestellNr Is Nothing AndAlso Not .SucheFahrgestellNr.Trim = "" Then
                Return True
            End If

            If Not .SucheTIDNR Is Nothing AndAlso Not .SucheTIDNR.Trim = "" Then
                Return True
            End If


            If Not .SucheZZREFERENZ1 Is Nothing AndAlso Not .SucheZZREFERENZ1.Trim = "" Then
                Return True
            End If

            If Not .sucheLIZNR Is Nothing AndAlso Not .sucheLIZNR.Trim = "" Then
                Return True
            End If
            Return False
        End With
    End Function

End Class

' ************************************************
' $History: Change02.aspx.vb $
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 11.08.08   Time: 17:54
' Updated in $/CKAG/Applications/AppBPLG/Forms
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 31.07.08   Time: 13:53
' Updated in $/CKAG/Applications/AppBPLG/Forms
' Doppeltes PageLoad entfernt
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 25.07.08   Time: 14:41
' Updated in $/CKAG/Applications/AppBPLG/Forms
' ita 2070 nachbesserungen
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 22.07.08   Time: 12:49
' Updated in $/CKAG/Applications/AppBPLG/Forms
' ITA 2070
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 18.07.08   Time: 14:19
' Updated in $/CKAG/Applications/AppBPLG/Forms
' ITA 2070 rohversion
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 18.07.08   Time: 12:51
' Created in $/CKAG/Applications/AppBPLG/Forms
' Klassen erstellt
' 
' ************************************************
