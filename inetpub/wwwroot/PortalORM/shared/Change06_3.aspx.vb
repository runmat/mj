Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Namespace [Shared]
    Public Class Change06_3
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

        Private m_User As Security.User
        Private m_App As Security.App
        Private objPDIs As Base.Business.ABEDaten ' SIXT_PDI

        Protected WithEvents lblError As System.Web.UI.WebControls.Label
        Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
        Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
        Protected WithEvents ucHeader As Header
        Protected WithEvents Label1_1 As System.Web.UI.WebControls.Label
        Protected WithEvents Label1_2 As System.Web.UI.WebControls.Label
        Protected WithEvents Label1_3 As System.Web.UI.WebControls.Label
        Protected WithEvents Label1_4 As System.Web.UI.WebControls.Label
        Protected WithEvents Label2_1 As System.Web.UI.WebControls.Label
        Protected WithEvents Label2_2 As System.Web.UI.WebControls.Label
        Protected WithEvents Label3_1 As System.Web.UI.WebControls.Label
        Protected WithEvents Label3_2 As System.Web.UI.WebControls.Label
        Protected WithEvents Label4_1 As System.Web.UI.WebControls.Label
        Protected WithEvents Label4_2 As System.Web.UI.WebControls.Label
        Protected WithEvents Label5 As System.Web.UI.WebControls.Label
        Protected WithEvents Label6 As System.Web.UI.WebControls.Label
        Protected WithEvents Label7 As System.Web.UI.WebControls.Label
        Protected WithEvents Label8 As System.Web.UI.WebControls.Label
        Protected WithEvents Label9 As System.Web.UI.WebControls.Label
        Protected WithEvents Label10 As System.Web.UI.WebControls.Label
        Protected WithEvents Label11 As System.Web.UI.WebControls.Label
        Protected WithEvents Label12 As System.Web.UI.WebControls.Label
        Protected WithEvents Label13_1 As System.Web.UI.WebControls.Label
        Protected WithEvents Label13_2 As System.Web.UI.WebControls.Label
        Protected WithEvents Label13_3 As System.Web.UI.WebControls.Label
        Protected WithEvents Label14 As System.Web.UI.WebControls.Label
        Protected WithEvents Label15 As System.Web.UI.WebControls.Label
        Protected WithEvents Label16_1 As System.Web.UI.WebControls.Label
        Protected WithEvents Label16_2 As System.Web.UI.WebControls.Label
        Protected WithEvents Label16_3 As System.Web.UI.WebControls.Label
        Protected WithEvents Label17 As System.Web.UI.WebControls.Label
        Protected WithEvents Label18 As System.Web.UI.WebControls.Label
        Protected WithEvents Label19 As System.Web.UI.WebControls.Label
        Protected WithEvents Label20 As System.Web.UI.WebControls.Label
        Protected WithEvents Label21 As System.Web.UI.WebControls.Label
        Protected WithEvents Label22 As System.Web.UI.WebControls.Label
        Protected WithEvents Label23 As System.Web.UI.WebControls.Label
        Protected WithEvents Label24 As System.Web.UI.WebControls.Label
        Protected WithEvents Label25 As System.Web.UI.WebControls.Label
        Protected WithEvents Label26 As System.Web.UI.WebControls.Label
        Protected WithEvents Label27 As System.Web.UI.WebControls.Label
        Protected WithEvents Label28 As System.Web.UI.WebControls.Label
        Protected WithEvents Label29 As System.Web.UI.WebControls.Label
        Protected WithEvents Label30 As System.Web.UI.WebControls.Label
        Protected WithEvents Label31 As System.Web.UI.WebControls.Label
        Protected WithEvents Label33 As System.Web.UI.WebControls.Label
        Protected WithEvents Label32_2 As System.Web.UI.WebControls.Label
        Protected WithEvents Label32 As System.Web.UI.WebControls.Label
        Protected WithEvents lblHead As System.Web.UI.WebControls.Label
        Protected WithEvents lnkFahrzeugsuche As System.Web.UI.WebControls.HyperLink
        Protected WithEvents ucStyles As Styles

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            m_User = GetUser(Me)
            ucHeader.InitUser(m_User)
            FormAuth(Me, m_User)
            ucHeader.Visible = False
            Try
                m_App = New Security.App(m_User)

                'If (Session("objPDIs") Is Nothing) Then
                'If (Session("ResultTable") Is Nothing) Then
                '    lblError.Text = "Fehler: Die Seite wurde ohne Kontext aufgerufen."
                'Else
                objPDIs = New Base.Business.ABEDaten(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "", m_User.KUNNR, "ZDAD", "", "")
                'End If
                'Else
                'objPDIs = CType(Session("objPDIs"), ABEDaten)
                'End If
                If Not objPDIs Is Nothing Then
                    lblHead.Text = objPDIs.Task
                    ucStyles.TitleText = lblHead.Text

                    If Request.QueryString("EqNr") Is Nothing Then
                        lblError.Text = "Feher: Die Seite wurde ohne Fahrzeugnummer aufgerufen."
                    Else
                        objPDIs.FillDatenABE(Session("AppID").ToString, Session.SessionID.ToString, Me.Page, Request.QueryString("EqNr").ToString)
                        If objPDIs.Status = 0 Then
                            With objPDIs.ABE_Daten
                                Label1_1.Text = .FahrzeugartText
                                Label1_2.Text = .Fahrzeugart
                                Label1_3.Text = .AufbauartText
                                Label1_4.Text = .AufbauartSchluessel
                                Label2_1.Text = .HerstellerKlartext
                                Label2_2.Text = .IdentifikationsnummerFuerFahrzeug
                                Label3_1.Text = .TypUndAusfuehrungsart
                                Label3_2.Text = .TypSchluessel & .Ausfuehrung & " " & .TypUndAusfuehrungsPruefziffer
                                Label4_1.Text = .Fahrgestellnummer
                                Label4_2.Text = .FahrgestellnummerPruefziffer
                                Label5.Text = .AntriebsartKurztext & " " & .Antriebsart
                                Label6.Text = .Hoechstgeschwindigkeit
                                Label7.Text = .Leistung
                                Label8.Text = .Hubraum
                                Label9.Text = .NutzAufliegelast
                                Label10.Text = .Tankinhalt
                                Label11.Text = .Stehplaetze
                                Label12.Text = .MaximalAnzahlPersonen
                                Label13_1.Text = .LaengeDesFahrzeugs
                                Label13_2.Text = .BreiteDesFahrzeugs
                                Label13_3.Text = .HoeheDesFahrzeugs
                                Label14.Text = .Leergewicht
                                Label15.Text = .ZulaessigesGesamtgewicht
                                Label16_1.Text = .AchslastVorn
                                Label16_2.Text = .AchslastMitte
                                Label16_3.Text = .AchslastHinten
                                Label17.Text = "1"
                                Label18.Text = .AnzahlDerAchsenDesFahrzeugs
                                Label19.Text = .DavonAngetriebeneAchsen
                                Label20.Text = .BereifunsgroesseVorn
                                Label21.Text = .BereifunsgroesseMitte_Hinten
                                Label22.Text = .OderBereifunsgroesseVorn
                                Label23.Text = .OderBereifunsgroesseMitte_Hinten
                                Label24.Text = .BremsdruckEinleitung
                                Label25.Text = .BremsdruckZweileitung
                                Label26.Text = .Anhaengerkupplung
                                Label27.Text = .AnhaengerKupplungPruefzeichen
                                Label28.Text = .AnhaengerlastGebremst
                                Label29.Text = .AnhaengerlastUngebremst
                                Label30.Text = .StandGeraeusch
                                Label31.Text = .Fahrgeraeusch
                                Label32_2.Text = .TagDerErstenZulassung
                                Label32.Text = .Farbziffer
                                Label33.Text = .Bemerkung
                            End With
                        Else
                            lblError.Text = objPDIs.Message
                        End If
                    End If
                End If
            Catch ex As Exception
                lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Try
        End Sub

        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
            SetEndASPXAccess(Me)
        End Sub

        Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
            SetEndASPXAccess(Me)
        End Sub
    End Class
End Namespace

' ************************************************
' $History: Change06_3.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 10.03.10   Time: 16:43
' Updated in $/CKAG/PortalORM/Shared
' ITA: 2918
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 3.04.08    Time: 17:21
' Created in $/CKAG/PortalORM/shared
' 
' *****************  Version 6  *****************
' User: Uha          Date: 20.06.07   Time: 14:32
' Updated in $/CKG/PortalORM/Shared
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 5  *****************
' User: Uha          Date: 1.03.07    Time: 16:38
' Updated in $/CKG/PortalORM/Shared
' 
' ************************************************