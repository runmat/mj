Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements


Public Class Change05
    Inherits System.Web.UI.Page
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents ddlPDIs As System.Web.UI.WebControls.DropDownList
    Protected WithEvents btnConfirm As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cbxAlle As System.Web.UI.WebControls.CheckBox
    Protected WithEvents rbAktion As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents cbxPDI As System.Web.UI.WebControls.CheckBox
    Private strPDI As String

    Private Const strTaskZulassen As String = "Zulassen"
    Private Const strTaskSperren As String = "Sperren"
    Private Const strTaskEntsperren As String = "Entsperren"
    Private Const strTaskVerschieben As String = "Verschieben"
    Private objSuche As Change_01

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

            If Not IsPostBack Then
                '§§§ JVE 18.07.2006: Zunächst PDI-Liste laden...
                Initialload()
            End If
            'DoSubmit()
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub Initialload()
        '§§§ JVE 18.07.2006: Hier DropDownlist mit PDIs füllen...
        Dim pdiListe As New PDIListe(m_User, m_App)

        pdiListe.getPDIs(Session("AppID").ToString, Session.SessionID)

        Dim e As Long
        Dim strListValue As String
        Dim dv As DataView

        e = 0

        dv = pdiListe.PPDIs.DefaultView

        dv.Sort = "KUNPDI asc"

        If (pdiListe.Status = 0) Then

            With ddlPDIs

                Do While e < dv.Count

                    strListValue = Left(dv.Item(e)("KUNPDI") & ".......", 7) & dv.Item(e)("NAME1")

                    strListValue = Replace(strListValue, "_", "&nbsc;")

                    .Items.Add(New ListItem(strListValue, dv.Item(e)("KUNPDI")))

                    e = e + 1
                Loop

            End With

        Else
            lblError.Text = pdiListe.Message
        End If


        objSuche = New Change_01(m_User, m_App, Session("AppID").ToString, Session.SessionID, "")
        objSuche.Customer = m_User.KUNNR

        If (objSuche.Status <> 0) Then
            lblError.Text = objSuche.Status
        Else
            objSuche.PDIListe = pdiListe.PPDIs
            Session("objSuche") = objSuche
        End If
    End Sub

    Private Sub btnConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConfirm.Click

        objSuche = CType(Session("objSuche"), Change_01)

        If cbxAlle.Checked Then
            objSuche.PPDISuche = String.Empty
        Else
            objSuche.PPDISuche = CStr(ddlPDIs.SelectedItem.Value)
        End If

        'Flag für PDI-Bereitmeldung
        objSuche.Task = rbAktion.SelectedItem.Value
        objSuche.PPhase = "B"
        If Not cbxPDI.Checked Then
            objSuche.PPhase = " "c
        End If

        'Sperrkennzeichen setzen
        If objSuche.Task = strTaskEntsperren Then
            objSuche.PSperre = " "c
        End If
        If objSuche.Task = strTaskSperren Then
            objSuche.PSperre = "X"c
        End If

        Session("objSuche") = objSuche
        'Response.Redirect("Change05_1.aspx?AppID=" & Session("AppID").ToString)
        Response.Redirect("Change05_0.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Change05.aspx.vb $
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 1.04.10    Time: 14:05
' Updated in $/CKAG/Applications/appec/Forms
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 1.04.10    Time: 13:24
' Updated in $/CKAG/Applications/appec/Forms
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 8.03.10    Time: 9:23
' Updated in $/CKAG/Applications/appec/Forms
' ITA: 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 17.06.09   Time: 8:47
' Updated in $/CKAG/Applications/appec/Forms
' Warnungen entfernt!
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 15:49
' Created in $/CKAG/Applications/appec/Forms
' 
' *****************  Version 11  *****************
' User: Uha          Date: 2.07.07    Time: 12:29
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 10  *****************
' User: Uha          Date: 22.05.07   Time: 13:31
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 9  *****************
' User: Uha          Date: 7.03.07    Time: 11:02
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' 
' ************************************************
