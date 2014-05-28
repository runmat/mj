Imports CKG.Base.Business
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Change42
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
    Private objSuche As Finance.Search
    Private objHaendler As fin_06
    Protected WithEvents Hyperlink1 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkLogout As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents Kopfdaten1 As PageElements.Kopfdaten
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ucHeader As Header
    Protected WithEvents Textbox1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtFahrgestellNr As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents Literal1 As System.Web.UI.WebControls.Literal
    Protected WithEvents lbl_Fahrgestellnummer As System.Web.UI.WebControls.Label
    Protected WithEvents txtHaendlerNr As System.Web.UI.WebControls.TextBox
    Protected WithEvents tr_FahrgestellNr As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lb_Link As System.Web.UI.WebControls.LinkButton
    Protected WithEvents tr_HaendlerNr1 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents txtHaendlernummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents lbl_TIDNR As System.Web.UI.WebControls.Label
    Protected WithEvents txtTIDNR As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtZZREFERENZ1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents lbl_ZZREFERENZ1 As System.Web.UI.WebControls.Label
    Protected WithEvents tr_TIDNR As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr_ZZREFERENZ1 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lbl_LIZNR As System.Web.UI.WebControls.Label
    Protected WithEvents txt_LIZNR As System.Web.UI.WebControls.TextBox
    Protected WithEvents SucheHaendler1 As SucheHaendler
    Protected WithEvents tr_LIZNR As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lbSelektionZurueckSetzen As System.Web.UI.WebControls.LinkButton

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cmdSearch.Visible = True
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User, True)
        GetAppIDFromQueryString(Me)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text
        m_App = New Base.Kernel.Security.App(m_User)

        If m_User.Reference.Trim(" "c).Length > 0 Then
            txtHaendlerNr.Text = m_User.Reference
        End If

        If (Not Request.QueryString("back") Is Nothing) AndAlso CStr(Request.QueryString("back")) = "1" Then
            If Not Session("objSuche") Is Nothing Then
                txtHaendlerNr.Text = CType(Session("objSuche"), Finance.Search).REFERENZ
            End If
        End If

        If (Not Request.QueryString("Linked") Is Nothing) AndAlso CStr(Request.QueryString("Linked")) = "1" Then
            If Not Session("objSuche") Is Nothing Then
                txtHaendlerNr.Text = Session("AppHaendlerNr")
            End If
        End If


        If IsPostBack = False Then
            If objHaendler Is Nothing Then
                If Session.Item("AppHaendler") Is Nothing Then
                    objHaendler = New fin_06(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "", m_User.Reference, m_User.KUNNR)
                    Session.Add("AppHaendler", objHaendler)
                Else
                    If TypeOf Session.Item("AppHaendler") Is fin_06 Then
                        objHaendler = CType(Session("AppHaendler"), fin_06)
                    Else
                        objHaendler = New fin_06(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "", m_User.Reference, m_User.KUNNR)
                        Session("AppHaendler") = objHaendler
                    End If
                End If
            End If

            If m_User.Reference Is Nothing OrElse m_User.Reference.Trim Is String.Empty OrElse m_User.Reference.Trim = "" Then 'standardfall wenn Bänker App betritt" 
                Kopfdaten1.Visible = False

                tr_HaendlerNr1.Visible = True
                tr_FahrgestellNr.Visible = True
                tr_TIDNR.Visible = True
                tr_ZZREFERENZ1.Visible = True
                tr_LIZNR.Visible = True



            Else    'fall wenn user mit User-Referenz, normale Händlerzulassung 

                Kopfdaten1.Visible = True

                tr_HaendlerNr1.Visible = False
                tr_FahrgestellNr.Visible = True
                tr_ZZREFERENZ1.Visible = True
                tr_TIDNR.Visible = True
                tr_LIZNR.Visible = True

                objSuche = New Finance.Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
                If Not objSuche.LeseHaendlerSAP_Einzeln(Session("AppID").ToString, Session.SessionID.ToString, txtHaendlerNr.Text) Then
                    lblError.Text = "Fehler bei der Ermittlung der Händlerdaten.<br>(" & objSuche.ErrorMessage & ")"
                End If


                Kopfdaten1.UserReferenz = m_User.Reference
                Kopfdaten1.HaendlerNummer = objSuche.REFERENZ
                Dim strTemp As String = objSuche.NAME
                If objSuche.NAME_2.Length > 0 Then
                    strTemp &= "<br>" & objSuche.NAME_2
                End If
                Kopfdaten1.HaendlerName = strTemp
                Kopfdaten1.Adresse = objSuche.COUNTRYISO & " - " & objSuche.POSTL_CODE & " " & objSuche.CITY & "<br>" & objSuche.STREET
                Session("objSuche") = objSuche

                'kontingenttabelle ausblenden wenn Parameter
                If Request.QueryString("HDL") = 1 Then
                    Session("AppShowNot") = True
                End If


                objHaendler.Customer = m_User.Reference 'benötigt für kontingente zu füllen
                objHaendler.Show(Session("AppID").ToString, Session.SessionID) 'kontingente Füllen

                If objHaendler.Status = 0 Then
                    Kopfdaten1.Kontingente = objHaendler.Kontingente
                Else
                    lblError.Text = "Fehler bei der Ermittlung der Kontingentdaten.<br>(" & objHaendler.Message & ")"
                End If

                
                Session("AppHaendler") = objHaendler

                Literal1.Text = "		<script language=""JavaScript"">" & vbCrLf
                Literal1.Text &= "			<!-- //" & vbCrLf
                Literal1.Text &= "			window.document.Form1.txtTIDNR.focus();" & vbCrLf
                Literal1.Text &= "			//-->" & vbCrLf
                Literal1.Text &= "		</script>" & vbCrLf

            End If

        Else 'wenn Postback
            If objHaendler Is Nothing Then
                objHaendler = CType(Session("AppHaendler"), fin_06)
            End If
        End If

    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        DoSubmit()
    End Sub

    Private Sub DoSubmit()

        lblError.Text = ""
        lblError.Visible = False
        Kopfdaten1.MessageError = ""
        Kopfdaten1.Message = ""

        If tr_HaendlerNr1.Visible = True Then 'BänkerFall
            objHaendler.Customer = SucheHaendler1.giveHaendlernummer
        Else 'HändlerFall
            objHaendler.Customer = m_User.Reference
        End If



        objHaendler.SucheTIDNR = txtTIDNR.Text
        objHaendler.SucheZZREFERENZ1 = txtZZREFERENZ1.Text ' Ordernummer
        objHaendler.SucheFahrgestellNr = Replace(txtFahrgestellNr.Text, "%", "*")
        objHaendler.KUNNR = m_User.KUNNR
        objHaendler.sucheLIZNR = txt_LIZNR.Text

        If Not pruefung() Then
            lblError.Text = "Bitte geben Sie ein Suchkriterium ein."
            lblError.Visible = True
            lbSelektionZurueckSetzen.Visible = True
            Exit Sub
        End If


        objHaendler.GiveCars(Session("AppID").ToString, Session.SessionID)
        If Not objHaendler.Status = 0 Then
            lblError.Text = objHaendler.Message
            lblError.Visible = True
            If objHaendler.Status = -2503 Then
                If m_User.Reference.Trim Is String.Empty Then
                    Session("AppFIN") = Replace(txtFahrgestellNr.Text, "%", "*")
                    Session("AppHaendlerNr") = objHaendler.getHaendlernummerByFin(Replace(txtFahrgestellNr.Text, "%", "*"))
                    If Not objHaendler.Status = 0 Then
                        lblError.Text = objHaendler.Message
                        lblError.Visible = True
                        Exit Sub
                    End If
                    lb_Link.Visible = True


                End If
            Else
                lb_Link.Visible = False
            End If
        Else
            If objHaendler.Fahrzeuge.Rows.Count = 0 Then
                lblError.Text = "Zu den gewählten Kriterien wurden keine Ergebnisse gefunden."
                lbSelektionZurueckSetzen.Visible = True
                lblError.Visible = True
            Else
                Session("AppHaendler") = objHaendler
                Response.Redirect("Change42_2.aspx?AppID=" & Session("AppID").ToString, False)
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

        With objHaendler
            If Not .SucheFahrgestellNr Is Nothing AndAlso Not .SucheFahrgestellNr.Trim = "" Then
                Return True
            End If

            If Not .SucheTIDNR Is Nothing AndAlso Not .SucheTIDNR.Trim = "" Then
                Return True
            End If


            If Not .SucheZZREFERENZ1 Is Nothing AndAlso Not .SucheZZREFERENZ1.Trim = "" Then
                Return True
            End If

            If Not .Customer Is Nothing AndAlso Not .Customer.Trim = "" AndAlso Not .Customer = "0000000000" Then
                Return True
            End If

            If Not .sucheLIZNR Is Nothing AndAlso Not .sucheLIZNR.Trim = "" Then
                Return True
            End If
            Return False
        End With
    End Function

    Private Sub lb_Link_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_Link.Click

        If Not Session("AppFIN") Is Nothing AndAlso Not Session("AppHaendlerNr") Is Nothing Then
            'Response.Redirect("Change47.aspx?AppID=" & Session("AppID").ToString & "&Linked=1")

            Dim dvAppLinks As DataView = m_User.Applications.DefaultView
            dvAppLinks.RowFilter = "APPURL='../Components/ComCommon/Finance/Change47.aspx'"
            If dvAppLinks.Count = 1 Then
                Dim strParameter As String = ""
                HelpProcedures.getAppParameters(dvAppLinks.Item(0).Item("AppID"), strParameter, ConfigurationManager.AppSettings("Connectionstring"))
                Response.Redirect("Change47.aspx?AppID=" & dvAppLinks.Item(0).Item("AppID") & strParameter & "&Linked=1")
            Else
                lblError.Text = "Fehler bei der Weiterleitung zur Statusänderung"
                lblError.Visible = True
            End If
        Else
            lblError.Text = "Fehler bei der Weiterleitung zur Statusänderung"
            lblError.Visible = True
        End If
    End Sub

    Private Sub lbSelektionZurueckSetzen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSelektionZurueckSetzen.Click
        SucheHaendler1.SelektionZuruecksetzen()
        txt_LIZNR.Text = ""
        txtFahrgestellNr.Text = ""
        txtTIDNR.Text = ""
        txtZZREFERENZ1.Text = ""
        lbSelektionZurueckSetzen.Visible = False
    End Sub
End Class

' ************************************************
' $History: Change42.aspx.vb $
' 
' *****************  Version 16  *****************
' User: Jungj        Date: 23.06.09   Time: 14:57
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2918 Z_M_Unangefordert_002
' 
' *****************  Version 15  *****************
' User: Jungj        Date: 22.06.09   Time: 13:18
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2918 Z_M_Creditlimit_Detail_001
' 
' *****************  Version 14  *****************
' User: Jungj        Date: 19.06.09   Time: 10:10
' Updated in $/CKAG/Components/ComCommon/Finance
' ITa 2918 testfertig
' 
' *****************  Version 13  *****************
' User: Jungj        Date: 5.06.09    Time: 15:58
' Updated in $/CKAG/Components/ComCommon/Finance
' mögliche try catches entfernt
' 
' *****************  Version 12  *****************
' User: Jungj        Date: 28.08.08   Time: 15:14
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2124 fertig
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 30.07.08   Time: 11:50
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2119
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 29.07.08   Time: 13:09
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2119 Prototyp fertig
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 25.07.08   Time: 16:05
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2119 nachbesserung
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 25.07.08   Time: 15:37
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2119 Prototyp
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 5.06.08    Time: 9:45
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 1988
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 4.06.08    Time: 19:37
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 1988
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 4.06.08    Time: 16:15
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 1988
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 23.05.08   Time: 15:24
' Updated in $/CKAG/Components/ComCommon/Finance
' Bugfixing RTFS Bank Portal
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 11.04.08   Time: 13:43
' Updated in $/CKAG/Components/ComCommon/Finance
' Migration
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
' User: Fassbenders  Date: 4.04.08    Time: 10:47
' Created in $/CKAG/portal/Components/ComCommon/Finance
' 
' *****************  Version 21  *****************
' User: Jungj        Date: 4.03.08    Time: 18:56
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' AKF ANPASSUNGEN ITA 1733
' 
' *****************  Version 20  *****************
' User: Jungj        Date: 4.03.08    Time: 10:03
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1733, 1667, 1738 
' 
' *****************  Version 19  *****************
' User: Jungj        Date: 1.03.08    Time: 14:50
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' AKF ÄNDERUNGEN X
' 
' *****************  Version 18  *****************
' User: Jungj        Date: 29.02.08   Time: 16:51
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1733
' 
' *****************  Version 17  *****************
' User: Jungj        Date: 29.02.08   Time: 12:29
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1733
' 
' *****************  Version 16  *****************
' User: Jungj        Date: 27.02.08   Time: 15:33
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1733
' 
' *****************  Version 15  *****************
' User: Jungj        Date: 27.02.08   Time: 8:00
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ita 1733
' 
' *****************  Version 14  *****************
' User: Jungj        Date: 26.02.08   Time: 16:59
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1733
' 
' *****************  Version 13  *****************
' User: Jungj        Date: 26.02.08   Time: 8:50
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ita 1733
' 
' *****************  Version 12  *****************
' User: Rudolpho     Date: 12.02.08   Time: 9:15
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' 
' *****************  Version 11  *****************
' User: Rudolpho     Date: 31.01.08   Time: 16:44
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Bugfix AKF
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 25.01.08   Time: 13:51
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Rothe Verbesserungen RTFS TEIL 2
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 18.01.08   Time: 10:57
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1624 fertig
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 17.01.08   Time: 12:38
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA: 1617
' 
' *****************  Version 7  *****************
' User: Uha          Date: 9.01.08    Time: 17:52
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1484: Abruf von ZBII ( Händler & Bank - AKF) - Bug-Fixing
' 
' *****************  Version 6  *****************
' User: Uha          Date: 7.01.08    Time: 18:47
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 4.01.08    Time: 16:01
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Bugfix RTFS
' 
' *****************  Version 4  *****************
' User: Uha          Date: 19.12.07   Time: 14:10
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1510,1512,1511 Anforderung / Zulassung
' 
' *****************  Version 3  *****************
' User: Uha          Date: 18.12.07   Time: 14:20
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' 
' *****************  Version 2  *****************
' User: Uha          Date: 17.12.07   Time: 17:58
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Versand ZB II / Briefe - komplierfähige Zwischenversion
' 
' *****************  Version 1  *****************
' User: Uha          Date: 13.12.07   Time: 17:18
' Created in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Elemente für Temp./Endg. bzw. HEZ Anforderung hinzugefügt (Change42ff,
' fin_06, Change43ff und fin_08)
' 
' ************************************************
