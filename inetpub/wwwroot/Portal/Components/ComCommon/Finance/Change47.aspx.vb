Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements


Public Class Change47
    Inherits System.Web.UI.Page

    Private m_User As Security.User
    Private m_App As Security.App
    Private objSuche As Finance.Search
    Private objHaendler As fin_09
    Private m_blnDoSubmit As Boolean
    Private m_iLinked As Int32 = 0


    Protected WithEvents SucheHaendler1 As SucheHaendler
    Protected WithEvents lblHead As Label
    Protected WithEvents lblPageTitle As Label
    Protected WithEvents lbSuche As LinkButton
    Protected WithEvents lbl_HaendlerNr As Label
    Protected WithEvents txtHaendlerNummerLocal As TextBox
    Protected WithEvents txtVertragsNr As TextBox
    Protected WithEvents lbl_Fahrgestellnummer As Label
    Protected WithEvents txtFahrgestellNr As TextBox
    Protected WithEvents lblError As Label
    Protected WithEvents Literal1 As Literal
    Protected WithEvents tr_Vertragsnummer As HtmlTableRow
    Protected WithEvents ucStyles As Styles
    Protected WithEvents ddl_Haendler As DropDownList
    Protected WithEvents lbl_Vertragsnummer As Label
    Protected WithEvents lbl_haendlernamenAnzeige As Label
    Protected WithEvents tr_HaendlerDropDown As HtmlTableRow
    Protected WithEvents tr_HaendlerNr As HtmlTableRow
    Protected WithEvents tr_Objektnummer As HtmlTableRow
    Protected WithEvents lbl_Objektnummer As Label
    Protected WithEvents txtObjektnummer As TextBox
    Protected WithEvents tr_Fahrgestellnummer As HtmlTableRow
    Protected WithEvents tr_HaendlernamenAnzeige As HtmlTableRow
    Protected WithEvents tr_AnzeigeHaendlerSuche As HtmlTableRow
    Protected WithEvents lbSelektionZurueckSetzen As LinkButton


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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        lbSuche.Visible = True
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User, True)
        GetAppIDFromQueryString(Me)

        If Not Session("doSubmit") Is Nothing Then
            m_blnDoSubmit = CType(Session("doSubmit"), Boolean)
        End If


        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text
        m_App = New Security.App(m_User)

        If m_User.Reference.Trim(" "c).Length > 0 Then
            txtHaendlerNummerLocal.Text = m_User.Reference
        End If


        If Not IsPostBack Then
            If (Not Request.QueryString("back") Is Nothing) AndAlso CStr(Request.QueryString("back")) = "1" Then
                Session("AppHaendler09") = Nothing
                Session("objSuche") = Nothing
            End If
        End If


        Select Case Request.QueryString("Linked")
            Case 1
                Session.Add("URLReferenz", Page.Request.UrlReferrer.ToString)
                txtFahrgestellNr.Text = Session("AppFIN")
                txtHaendlerNummerLocal.Text = Session("AppHaendlerNr")
                If Not Session("objSuche") Is Nothing AndAlso TypeOf Session("objSuche") Is Finance.Search Then
                    objSuche = Session("objSuche")
                Else
                    objSuche = New Finance.Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
                End If
                Session("objSuche") = objSuche
                m_iLinked = 1
                lblHead.Text = "Änderung Versandstatus (nachträglich endgültig)"
                ucStyles.TitleText = lblHead.Text
                m_blnDoSubmit = True
                DoSubmit()
                Exit Sub
            Case 2
                'verlinkung von Fällige Vorgänge (change41_2)
                Session.Add("URLReferenz", Page.Request.UrlReferrer.ToString)
                txtFahrgestellNr.Text = Session("AppFIN")
                txtHaendlerNummerLocal.Text = Session("AppHaendlerNr")
                If Not Session("objSuche") Is Nothing AndAlso TypeOf Session("objSuche") Is Finance.Search Then
                    objSuche = Session("objSuche")
                Else
                    objSuche = New Finance.Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
                End If
                Session("objSuche") = objSuche
                m_iLinked = 2
                lblHead.Text = "Änderung Versandstatus (nachträglich endgültig)"
                ucStyles.TitleText = lblHead.Text
                m_blnDoSubmit = True
                DoSubmit()
        End Select
        

        If Not IsPostBack Then
            objSuche = New Finance.Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
            Session("objSuche") = objSuche

            If m_User.Reference Is Nothing OrElse m_User.Reference.Trim(" "c) = "" Then 'Bänkerfall
                tr_HaendlerNr.Visible = True
                tr_AnzeigeHaendlerSuche.Visible = True
                tr_Fahrgestellnummer.Visible = False
                tr_Vertragsnummer.Visible = False
                tr_Objektnummer.Visible = False
                tr_HaendlernamenAnzeige.Visible = False
                m_blnDoSubmit = False
            Else 'Händlerfall, auswahl Händler entfällt
                tr_AnzeigeHaendlerSuche.Visible = False
                tr_HaendlerNr.Visible = False
                tr_Fahrgestellnummer.Visible = True
                tr_Vertragsnummer.Visible = True
                tr_Objektnummer.Visible = True
                tr_HaendlernamenAnzeige.Visible = True

                If Not Session("objSuche") Is Nothing Then
                    objSuche = Session("objSuche")
                Else
                    objSuche = New Finance.Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
                End If

                Dim strHaendler As String
                strHaendler = txtHaendlerNummerLocal.Text

                If Not objSuche.LeseHaendlerSAP_Einzeln(Session("AppID").ToString, Session.SessionID.ToString, strHaendler) Then
                    lblError.Text = "Der Händler konnte nicht ermittelt werden."
                    lbSuche.Visible = False
                    Exit Sub
                End If

                Dim strTemp As String = objSuche.NAME
                If objSuche.NAME_2.Length > 0 Then
                    strTemp &= "<br>" & objSuche.NAME_2
                End If
                lbl_haendlernamenAnzeige.Text = strTemp
                m_blnDoSubmit = True
                Session("objSuche") = objSuche
            End If

        End If

    End Sub

    Private Sub lbSuche_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbSuche.Click
        DoSubmit()
    End Sub
    
    Private Sub DoSubmit()
        lblError.Text = ""
        lblError.Visible = False
        If objSuche Is Nothing Then
            objSuche = Session("objSuche")
        End If

        If m_blnDoSubmit = True Then 'fall wenn bänker bereits in der 2. suchmaske war oder händler fall (Bekommt gleich die 2. suchmaske )

            If (Session("AppHaendler09") Is Nothing) OrElse (Not IsPostBack) Then
                objHaendler = New fin_09(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
            Else
                objHaendler = CType(Session("AppHaendler09"), fin_09)
            End If


            objHaendler.Vertragsnummer = txtVertragsNr.Text
            objHaendler.Fahrgestellnummer = Replace(txtFahrgestellNr.Text, "%", "*")
            objHaendler.Objektnummer = txtObjektnummer.Text.Trim
            objHaendler.KUNNR = m_User.KUNNR
            objHaendler.Customer = txtHaendlerNummerLocal.Text
            objHaendler.FILL(Session("AppID").ToString, Session.SessionID.ToString)

            If Not objHaendler.Status = 0 Then
                lblError.Text = objHaendler.Message
                lblError.Visible = True
            Else
                If objHaendler.Fahrzeuge.Rows.Count = 0 Then
                    lblError.Visible = True
                    lblError.Text = "Zu den gewählten Kriterien wurden keine Ergebnisse gefunden."
                Else
                    Session("AppHaendler09") = objHaendler
                    If m_iLinked = 1 Then
                        Response.Redirect("Change47_02.aspx?AppID=" & Session("AppID").ToString & "&Linked=1")
                    ElseIf m_iLinked = 2 Then
                        Response.Redirect("Change47_02.aspx?AppID=" & Session("AppID").ToString & "&Linked=2")
                    Else
                        Response.Redirect("Change47_02.aspx?AppID=" & Session("AppID").ToString)
                    End If
                End If
            End If

        Else 'fall Bänker wenn er eine Händlerauswahl getroffen hat. 
            Dim tmpHaendlernummer As String
            tmpHaendlernummer = SucheHaendler1.giveHaendlernummer()
            If tmpHaendlernummer = Nothing Then
                lblError.Text = "Wählen Sie einen Händler aus"
                lblError.Visible = True
                lbSelektionZurueckSetzen.Visible = True
                Exit Sub
            Else 'wenn er eine Händlerauswahl getroffen hat 
                txtHaendlerNummerLocal.Text = tmpHaendlernummer

                'händlerNamen ermitteln und in label anzeigen sowie Händlersuche aus und andere Suchkriterien einblenden
                Dim strHaendler As String
                strHaendler = txtHaendlerNummerLocal.Text

                If Not objSuche.LeseHaendlerSAP_Einzeln(Session("AppID").ToString, Session.SessionID.ToString, strHaendler) Then
                    lblError.Text = "Der Händler konnte nicht ermittelt werden."
                    lbSuche.Visible = False
                    Exit Sub
                End If

                Dim strTemp As String = objSuche.NAME
                If objSuche.NAME_2.Length > 0 Then
                    strTemp &= "<br>" & objSuche.NAME_2
                End If
                lbl_haendlernamenAnzeige.Text = strTemp
                Session("objSuche") = objSuche


                'veränderung der anzeige 
                tr_HaendlerNr.Visible = False
                tr_AnzeigeHaendlerSuche.Visible = False
                tr_Fahrgestellnummer.Visible = True
                tr_HaendlernamenAnzeige.Visible = True
                tr_Objektnummer.Visible = True
                tr_Vertragsnummer.Visible = True
                m_blnDoSubmit = True
            End If
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        Session("doSubmit") = m_blnDoSubmit
        SetEndASPXAccess(Me)
    End Sub

    Private Sub lbSelektionZurueckSetzen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSelektionZurueckSetzen.Click
        SucheHaendler1.SelektionZuruecksetzen()
        lbSelektionZurueckSetzen.Visible = False
    End Sub
End Class

' ************************************************
' $History: Change47.aspx.vb $
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 24.06.09   Time: 13:11
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2918 Z_Dad_Daten_Einaus_Report_003
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 19.06.09   Time: 10:10
' Updated in $/CKAG/Components/ComCommon/Finance
' ITa 2918 testfertig
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 5.06.09    Time: 15:58
' Updated in $/CKAG/Components/ComCommon/Finance
' mögliche try catches entfernt
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 28.08.08   Time: 15:14
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2124 fertig
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 30.07.08   Time: 11:50
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2119
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 6.06.08    Time: 8:24
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 1988
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 5.06.08    Time: 16:50
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 1988
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 3.06.08    Time: 16:51
' Updated in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 29.05.08   Time: 15:55
' Updated in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 10.04.08   Time: 17:37
' Updated in $/CKAG/Components/ComCommon/Finance
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:32
' Created in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 10:48
' Created in $/CKAG/portal/Components/ComCommon/Finance
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 4.03.08    Time: 18:56
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' AKF ANPASSUNGEN ITA 1733
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 4.03.08    Time: 10:03
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1733, 1667, 1738 
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 12.02.08   Time: 9:15
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 8.02.08    Time: 12:59
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Leere Eingabefelder abfangen
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 25.01.08   Time: 13:51
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Rothe Verbesserungen RTFS TEIL 2
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 18.01.08   Time: 10:57
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1624 fertig
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 17.01.08   Time: 12:38
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA: 1617
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 8.01.08    Time: 16:13
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Bugfix RTFS
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 8.01.08    Time: 12:49
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA:1515
' 
' ************************************************

