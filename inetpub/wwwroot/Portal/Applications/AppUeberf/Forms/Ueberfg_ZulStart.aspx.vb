Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Ueberfg_ZulStart
    Inherits System.Web.UI.Page

    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents chkZul As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkUeberf As System.Web.UI.WebControls.CheckBox
    Protected WithEvents ucStyles As Styles
    Protected WithEvents btnConfirm As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdRight1 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents ucHeader As Header

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

    Private clsUeberf As UeberfgStandard_01
    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            'DoSubmit()
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try



        If Session("Ueberf") Is Nothing Then
            clsUeberf = New UeberfgStandard_01(m_User, m_App, "")
            Session("Ueberf") = clsUeberf
        Else

            If Request.UrlReferrer.ToString.IndexOf("Selection.aspx") > 0 Then
                clsUeberf = Nothing
                Session("Ueberf") = Nothing
                clsUeberf = New UeberfgStandard_01(m_User, m_App, "")
                Session("Ueberf") = clsUeberf
            Else
                clsUeberf = Session("Ueberf")
            End If
        End If

        If IsPostBack = False Then
            'Zulassung und Überführung
            If clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.ZulassungUndUeberfuehrungKCL Then
                chkZul.Checked = True
                chkUeberf.Checked = True
                'Zuassung
            ElseIf clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.ZulassungKCL Then
                chkZul.Checked = True
                chkUeberf.Checked = False
                'Überführung
            ElseIf clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.UeberfuehrungKCL Then
                chkZul.Checked = False
                chkUeberf.Checked = True
            End If
        End If

    End Sub


    Private Sub cmdRight1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdRight1.Click
        If clsUeberf Is Nothing Then
            clsUeberf = Session("Ueberf")
        End If

        'Zulassung und Überführung beauftragen
        If chkZul.Checked = True And chkUeberf.Checked = True Then
            clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.ZulassungUndUeberfuehrungKCL
            'Zuassung beauftragen
        ElseIf chkZul.Checked = True And chkUeberf.Checked = False Then
            clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.ZulassungKCL
            'Überführung beauftragen
        ElseIf chkZul.Checked = False And chkUeberf.Checked = True Then
            clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.UeberfuehrungKCL
        Else
            lblError.Text = "Bitte wählen Sie eine Beauftragungsart aus."
            Exit Sub
        End If

        Session("Ueberf") = clsUeberf
        If clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.UeberfuehrungKCL Then
            Response.Redirect("Ueberfg_00.aspx?AppID=" & Session("AppID").ToString)
        Else
            Response.Redirect("Zulg_01.aspx?AppID=" & Session("AppID").ToString)
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
' $History: Ueberfg_ZulStart.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 16.04.08   Time: 11:53
' Updated in $/CKAG/Applications/AppUeberf/Forms
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 16:06
' Created in $/CKAG/Applications/AppUeberf/Forms
' 
' *****************  Version 5  *****************
' User: Uha          Date: 2.07.07    Time: 10:05
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 21.06.07   Time: 16:27
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 12.06.07   Time: 17:46
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 2  *****************
' User: Uha          Date: 5.04.07    Time: 11:14
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Verlinkung der Formulare untereinander korrigiert.
' 
' *****************  Version 1  *****************
' User: Uha          Date: 7.03.07    Time: 16:56
' Created in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Überführungs-ASPX-Seiten aus Shared-Bereich übernommen
' 
' ******************************************
