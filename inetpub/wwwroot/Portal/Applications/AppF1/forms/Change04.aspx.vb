Imports CKG
Imports CKG.Base.Kernel.Common.Common


Public Class Change04
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
    Private objSuche As Search
   

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
                Session("objSuche") = Nothing


                If m_User.Reference Is Nothing OrElse m_User.Reference.Trim Is String.Empty OrElse m_User.Reference.Trim = "" Then 'standardfall wenn Bänker App betritt" 

                    tr_HaendlerNr1.Visible = True
                    SucheHaendler1.initialize()
                Else
                    tr_HaendlerNr1.Visible = False

                End If


            End If

            If SucheHaendler1.isInUse Then
                lbSelektionZurueckSetzen.Visible = True
            Else
                lbSelektionZurueckSetzen.Visible = False
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

        If m_User.Reference Is Nothing OrElse m_User.Reference.Trim Is String.Empty OrElse m_User.Reference.Trim = "" Then
            'bänker fall
            If Not SucheHaendler1.giveHaendlernummer = "" Then

                objSuche = New Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
                objSuche.fillHaendlerData(Session("AppID").ToString, Session.SessionID.ToString, SucheHaendler1.giveHaendlernummer)
                If Not objSuche.Status = 0 Then
                    lblError.Text = "Fehler bei der Ermittlung der Händlerdaten.<br>(" & objSuche.ErrorMessage & ")"
                    Exit Sub
                End If
                Session("objSuche") = objSuche
                Response.Redirect("Change04_1.aspx?AppID=" & Session("AppID").ToString, False)
            Else
                lblError.Text = "wählen Sie bitte einen Händler aus"
            End If
        Else 'händler fall
            lblError.Text = "Sie haben keine Berechtigung zur Kontingentpflege"
        End If


    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub lbSelektionZurueckSetzen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSelektionZurueckSetzen.Click
        SucheHaendler1.SelektionZuruecksetzen()
    End Sub
End Class

' ************************************************
' $History: Change04.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 12.03.09   Time: 9:10
' Updated in $/CKAG/Applications/AppF1/forms
' ITA 2678,2677 testfertig
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 11.03.09   Time: 11:26
' Updated in $/CKAG/Applications/AppF1/forms
' ITA 2677 unfertig
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 11.03.09   Time: 10:28
' Created in $/CKAG/Applications/AppF1/forms
' ITA 2677&2678
' ************************************************
