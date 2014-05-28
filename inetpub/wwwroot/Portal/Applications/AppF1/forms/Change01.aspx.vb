Imports CKG
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements


Public Class Change01
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
    Private m_blnDoSubmit As Boolean
    Private mstrHDL As String
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents txtHaendlerNr As System.Web.UI.WebControls.TextBox
    Protected WithEvents tr_HaendlerNr1 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents SucheHaendler1 As AppF1.SucheHaendler
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lbSelektionZurueckSetzen As System.Web.UI.WebControls.LinkButton

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
                If m_User.Reference Is Nothing OrElse m_User.Reference.Trim Is String.Empty OrElse m_User.Reference.Trim = "" Then 'standardfall wenn Bänker App betritt" 

                    tr_HaendlerNr1.Visible = True
                    SucheHaendler1.initialize()

                Else    'fall wenn user mit User-Referenz, normale Händlerzulassung 

                    tr_HaendlerNr1.Visible = False

                    objSuche = New Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
                    objSuche.fillHaendlerData(Session("AppID").ToString, Session.SessionID.ToString, m_User.Reference)
                    If Not objSuche.Status = 0 Then
                        lblError.Text = "Fehler bei der Ermittlung der Händlerdaten.<br>(" & objSuche.Message & ")"
                        Exit Sub
                    End If


                    Session("objSuche") = objSuche

                    'kontingenttabelle ausblenden wenn Parameter
                    If Request.QueryString("HDL") = "1" Then
                        Session("AppShowNot") = True
                    End If


                    DoSubmit()

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
                Response.Redirect("Change01_1.aspx?AppID=" & Session("AppID").ToString, False)
            Else

                'wenn nicht gerade in SAP gesucht wird und keine Händlernummer zurückkommt
                If SucheHaendler1.SAPSearchStep = False Then
                    lblError.Text = "wählen sie bitte einen Händler aus"
                    lblError.Visible = True
                End If

            End If
        Else 'händler fall
            Response.Redirect("Change01_1.aspx?AppID=" & Session("AppID").ToString, False)
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
' $History: Change01.aspx.vb $
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 15.07.09   Time: 17:39
' Updated in $/CKAG/Applications/AppF1/forms
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 15.07.09   Time: 17:31
' Updated in $/CKAG/Applications/AppF1/forms
' Anpassung Peis filter
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 20.04.09   Time: 10:39
' Updated in $/CKAG/Applications/AppF1/forms
' suche HändlerControl  Bugfixes
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 9.03.09    Time: 15:25
' Updated in $/CKAG/Applications/AppF1/forms
' ita 2664 nachbesserung
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 9.03.09    Time: 14:51
' Updated in $/CKAG/Applications/AppF1/forms
' 2664 testfertig
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 5.03.09    Time: 17:22
' Updated in $/CKAG/Applications/AppF1/forms
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 5.03.09    Time: 15:52
' Updated in $/CKAG/Applications/AppF1/forms
' ita 2664
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 4.03.09    Time: 17:30
' Updated in $/CKAG/Applications/AppF1/forms
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 4.03.09    Time: 11:12
' Created in $/CKAG/Applications/AppF1/forms
'
' ************************************************
