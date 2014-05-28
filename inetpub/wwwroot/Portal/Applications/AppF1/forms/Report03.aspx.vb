Imports CKG
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements


Public Class Report03
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
    Private mObjInanspruchnahme As Inanspruchnahme



    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
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
                Session("mobjSucheSession") = Nothing


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


                    Session("mobjSucheSession") = objSuche

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
                Session("mobjSucheSession") = objSuche
                fillReportAndResponse(objSuche.REFERENZ)
            Else
                'wenn nicht gerade in SAP gesucht wird und keine Händlernummer zurückkommt
                If SucheHaendler1.SAPSearchStep = False Then
                    lblError.Text = "wählen sie bitte einen Händler aus"
                    lblError.Visible = True
                End If

            End If
        Else 'händler fall
            fillReportAndResponse(objSuche.REFERENZ)
        End If


    End Sub

    Private Sub fillReportAndResponse(ByVal haendlernummer As String)

        mObjInanspruchnahme = New Inanspruchnahme(m_User, m_App, Session("AppID").ToString, Session.SessionID, "")
        mObjInanspruchnahme.Haendler = haendlernummer
        mObjInanspruchnahme.Show(Session("AppID").ToString, Session.SessionID, Me)
        If mObjInanspruchnahme.Status = 0 Then
            Response.Redirect("Report03_1.aspx?AppID=" & Session("AppID").ToString, False)
            Session("mObjInanspruchnahmeSession") = mObjInanspruchnahme
        Else
            lblError.Text = mObjInanspruchnahme.Message
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
' $History: Report03.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 20.04.09   Time: 10:39
' Updated in $/CKAG/Applications/AppF1/forms
' suche H�ndlerControl  Bugfixes
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 23.03.09   Time: 17:09
' Updated in $/CKAG/Applications/AppF1/forms
' ita 2668 nachbesserungen
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 12.03.09   Time: 14:56
' Updated in $/CKAG/Applications/AppF1/forms
' ITA 2688/2668 unfertig
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 12.03.09   Time: 13:08
' Created in $/CKAG/Applications/AppF1/forms
' ITA 2668, 2688
' 
' ************************************************
