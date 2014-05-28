Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Change40
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
    Protected WithEvents SucheHaendler1 As SucheHaendler
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lbSuche As LinkButton
    Protected WithEvents lblPageTitle As Label
    Protected WithEvents lblHead As Label
    Protected WithEvents lbSelektionZurueckSetzen As LinkButton



    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        Try
            m_User = GetUser(Me)
            ucHeader.InitUser(m_User)
            FormAuth(Me, m_User, True)
            m_App = New Base.Kernel.Security.App(m_User)
            GetAppIDFromQueryString(Me)
            lblPageTitle.Text = " (Händlersuche)"
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

            lblError.Text = ""
            lblError.Visible = False

        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "Change40", "Page_Load", ex.ToString)

            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            Throw ex
        End Try
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Protected Sub lbSuche_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbSuche.Click
        Dim tmpHaendlernummer As String
        tmpHaendlernummer = SucheHaendler1.giveHaendlernummer

        If tmpHaendlernummer Is Nothing Then
            lblError.Text = "Wählen Sie einen Händler aus"
            lblError.Visible = True
            lbSelektionZurueckSetzen.Visible = True
        Else
            'für Change40Edit
            Session("SelectedDealer") = tmpHaendlernummer
            Session("objSuche") = New Finance.Search(m_App, m_User, Session.SessionID, CStr(Session("AppID")))
            Response.Redirect("Change40Edit.aspx")
        End If
    End Sub
    Private Sub lbSelektionZurueckSetzen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSelektionZurueckSetzen.Click
        SucheHaendler1.SelektionZuruecksetzen()
        lbSelektionZurueckSetzen.Visible = False
    End Sub

End Class

' ************************************************
' $History: Change40.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 5.06.09    Time: 13:51
' Updated in $/CKAG/Components/ComCommon/Finance
' Try Catch entfernt wenn möglich
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 30.07.08   Time: 13:09
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2119
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 5.06.08    Time: 16:50
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 1988
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:32
' Created in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 10:47
' Created in $/CKAG/portal/Components/ComCommon/Finance
' 
' *****************  Version 4  *****************
' User: Uha          Date: 8.01.08    Time: 16:53
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' 
' *****************  Version 3  *****************
' User: Uha          Date: 13.12.07   Time: 13:58
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1481/1509 (Änderung / Sperrung Händlerkontingent) Testversion
' 
' *****************  Version 2  *****************
' User: Uha          Date: 12.12.07   Time: 15:56
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1473/1497 (Mahnstufe 3) als Testversion; ITA 1481/1509
' (Änderung/Sperrung Händlerkontingent) komplierfähig
' 
' *****************  Version 1  *****************
' User: Uha          Date: 12.12.07   Time: 13:23
' Created in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Übernahme der Suchfunktion aus FFD (ohne Anpassung)
' 
' ************************************************
