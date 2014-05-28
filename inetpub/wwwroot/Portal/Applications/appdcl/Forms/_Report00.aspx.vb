Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

<CLSCompliant(False)> Public Class _Report00
    Inherits System.Web.UI.Page

    Protected WithEvents ucStyles As Styles
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents lblScript As System.Web.UI.WebControls.Label
    Protected WithEvents btnBearb As System.Web.UI.WebControls.LinkButton
    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Protected WithEvents CheckBox1 As System.Web.UI.WebControls.CheckBox
    Protected WithEvents lnkKreditlimit As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents td2 As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents lblFahrernrtext As System.Web.UI.WebControls.Label
    Protected WithEvents btnFahrernr As System.Web.UI.WebControls.LinkButton
    Protected WithEvents txtFahrernr As System.Web.UI.WebControls.TextBox

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

        Dim uebf As Ueberfuehrung
        m_User = GetUser(Me)

        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text

        Try
            m_App = New Base.Kernel.Security.App(m_User)

            If IsPostBack Then
                If (txtFahrernr.Text.Trim <> String.Empty) Then
                    uebf = New Ueberfuehrung(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                    uebf.getAuftraege(Session("AppID").ToString, Session.SessionID.ToString, txtFahrernr.Text.Trim)
                    If uebf.Status <> 0 AndAlso uebf.Status = -2201 Then
                        lblError.Text = "Keine Aufträge gefunden bzw. Fahrer-Nummer falsch."
                        Exit Sub
                    ElseIf uebf.Status <> 0 Then

                    End If

                    Response.Redirect("_Report01.aspx?AppID=" & Session("AppID").ToString & "&FID=" & txtFahrernr.Text)
                Else
                    lblError.Text = "Keine Fahrernummer eingetragen."
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Keine Aufträge gefunden bzw. Fahrer-Nummer falsch."
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
' $History: _Report00.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 4.02.10    Time: 14:38
' Updated in $/CKAG/Applications/appdcl/Forms
' ITA: 2918
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
' *****************  Version 5  *****************
' User: Rudolpho     Date: 27.07.07   Time: 8:20
' Updated in $/CKG/Applications/AppDCL/AppDCLWeb/Forms
' 
' *****************  Version 4  *****************
' User: Uha          Date: 21.06.07   Time: 12:36
' Updated in $/CKG/Applications/AppDCL/AppDCLWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 3  *****************
' User: Uha          Date: 7.03.07    Time: 10:26
' Updated in $/CKG/Applications/AppDCL/AppDCLWeb/Forms
' 
' ************************************************
