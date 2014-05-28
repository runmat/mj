Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Namespace [Shared]
    Public Class Change06_3NEU
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
        Private objPDIs As Base.Business.ABEDaten

        Protected WithEvents lblError As System.Web.UI.WebControls.Label
        Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
        Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
        Protected WithEvents ucHeader As Header
        Protected WithEvents lblHead As System.Web.UI.WebControls.Label
        Protected WithEvents lnkFahrzeugsuche As System.Web.UI.WebControls.HyperLink
        Protected WithEvents lbl_0 As System.Web.UI.WebControls.Label
        Protected WithEvents lbl_1 As System.Web.UI.WebControls.Label
        Protected WithEvents lbl_2 As System.Web.UI.WebControls.Label
        Protected WithEvents lbl_3 As System.Web.UI.WebControls.Label
        Protected WithEvents lbl_4 As System.Web.UI.WebControls.Label
        Protected WithEvents lbl_5 As System.Web.UI.WebControls.Label
        Protected WithEvents lbl_6 As System.Web.UI.WebControls.Label
        Protected WithEvents lbl_7 As System.Web.UI.WebControls.Label
        Protected WithEvents lbl_8 As System.Web.UI.WebControls.Label
        Protected WithEvents lbl_9 As System.Web.UI.WebControls.Label
        Protected WithEvents lbl_10 As System.Web.UI.WebControls.Label
        Protected WithEvents lbl_11 As System.Web.UI.WebControls.Label
        Protected WithEvents lbl_13 As System.Web.UI.WebControls.Label
        Protected WithEvents lbl_14 As System.Web.UI.WebControls.Label
        Protected WithEvents lbl_12 As System.Web.UI.WebControls.Label
        Protected WithEvents lbl_19 As System.Web.UI.WebControls.Label
        Protected WithEvents lbl_20 As System.Web.UI.WebControls.Label
        Protected WithEvents lbl_15 As System.Web.UI.WebControls.Label
        Protected WithEvents lbl_16 As System.Web.UI.WebControls.Label
        Protected WithEvents lbl_21 As System.Web.UI.WebControls.Label
        Protected WithEvents lbl_17 As System.Web.UI.WebControls.Label
        Protected WithEvents lbl_18 As System.Web.UI.WebControls.Label
        Protected WithEvents lbl_22 As System.Web.UI.WebControls.Label
        Protected WithEvents lbl_23 As System.Web.UI.WebControls.Label
        Protected WithEvents lbl_24 As System.Web.UI.WebControls.Label
        Protected WithEvents lbl_26 As System.Web.UI.WebControls.Label
        Protected WithEvents lbl_25 As System.Web.UI.WebControls.Label
        Protected WithEvents lbl_27 As System.Web.UI.WebControls.Label
        Protected WithEvents lbl_28 As System.Web.UI.WebControls.Label
        Protected WithEvents lbl_29 As System.Web.UI.WebControls.Label
        Protected WithEvents lbl_30 As System.Web.UI.WebControls.Label
        Protected WithEvents lbl_31 As System.Web.UI.WebControls.Label
        Protected WithEvents lbl_32 As System.Web.UI.WebControls.Label
        Protected WithEvents lbl_33 As System.Web.UI.WebControls.Label
        Protected WithEvents lbl_55 As System.Web.UI.WebControls.Label
        Protected WithEvents lbl_91 As System.Web.UI.WebControls.Label
        Protected WithEvents lbl_92 As System.Web.UI.WebControls.Label
        Protected WithEvents lbl_93 As System.Web.UI.WebControls.Label
        Protected WithEvents lbl_94 As System.Web.UI.WebControls.Label
        Protected WithEvents lbl_95 As System.Web.UI.WebControls.Label
        Protected WithEvents lbl_96 As System.Web.UI.WebControls.Label
        Protected WithEvents lbl_97 As System.Web.UI.WebControls.Label
        Protected WithEvents lbl_98 As System.Web.UI.WebControls.Label
        Protected WithEvents lbl_99 As System.Web.UI.WebControls.Label
        Protected WithEvents lbl_00 As System.Web.UI.WebControls.Label
        Protected WithEvents lbl_KennEmpty As System.Web.UI.WebControls.Label
        Protected WithEvents lbl_Kennzeichen As System.Web.UI.WebControls.Label
        Protected WithEvents Label3 As System.Web.UI.WebControls.Label
        Protected WithEvents lbl_FIN As System.Web.UI.WebControls.Label

        Protected WithEvents ucStyles As Styles

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            m_User = GetUser(Me)
            ucHeader.InitUser(m_User)
            FormAuth(Me, m_User)
            ucHeader.Visible = False

            'Farben erstmal ausblenden

            lbl_55.Visible = False
            lbl_91.Visible = False
            lbl_92.Visible = False
            lbl_93.Visible = False
            lbl_94.Visible = False
            lbl_95.Visible = False
            lbl_96.Visible = False
            lbl_97.Visible = False
            lbl_98.Visible = False
            lbl_99.Visible = False
            Try
                m_App = New Security.App(m_User)

                'If (Session("objPDIs") Is Nothing) Then
                '    If (Session("ResultTable") Is Nothing) Then
                '        lblError.Text = "Fehler: Die Seite wurde ohne Kontext aufgerufen."
                '    Else
                objPDIs = New Base.Business.ABEDaten(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "", m_User.KUNNR, "ZDAD", "", "")
                '    End If
                'Else
                '    objPDIs = CType(Session("objPDIs"), ABEDatenNeu)
                'End If
                If Not objPDIs Is Nothing Then
                    lblHead.Text = objPDIs.Task
                    ucStyles.TitleText = lblHead.Text

                    If Request.QueryString("EqNr") Is Nothing Then
                        lblError.Text = "Fehler: Die Seite wurde ohne Fahrzeugnummer aufgerufen."
                    Else
                        'TrimStart("0"c)
                        objPDIs.FillDatenABE(Session("AppID").ToString, Session.SessionID.ToString, Me.Page, Request.QueryString("EqNr").ToString)
                        If objPDIs.Status = 0 Then
                            With objPDIs.ABE_Daten
                                'lbl_00.Text = .Farbziffer
                                lbl_FIN.Text = .Fahrgestellnummer

                                If Not Session("ABEKennzeichen") Is Nothing Then
                                    lbl_Kennzeichen.Text = Session("ABEKennzeichen")
                                End If
                                lbl_0.Text = .ZZKLARTEXT_TYP
                                lbl_1.Text = .ZZHERST_TEXT
                                lbl_2.Text = .ZZHERSTELLER_SCH
                                lbl_3.Text = .ZZHANDELSNAME
                                lbl_4.Text = .ZZGENEHMIGNR
                                lbl_5.Text = .ZZGENEHMIGDAT
                                lbl_6.Text = .ZZFHRZKLASSE_TXT
                                lbl_7.Text = .ZZTEXT_AUFBAU
                                lbl_8.Text = .ZZFABRIKNAME
                                lbl_9.Text = .ZZVARIANTE
                                lbl_10.Text = .ZZVERSION
                                lbl_11.Text = .ZZHUBRAUM.TrimStart("0"c)
                                lbl_13.Text = .ZZNENNLEISTUNG.TrimStart("0"c)
                                lbl_14.Text = .ZZBEIUMDREH.TrimStart("0"c)
                                lbl_12.Text = .ZZHOECHSTGESCHW
                                lbl_19.Text = .ZZSTANDGERAEUSCH.TrimStart("0"c)
                                lbl_20.Text = .ZZFAHRGERAEUSCH.TrimStart("0"c)
                                lbl_15.Text = .ZZKRAFTSTOFF_TXT
                                lbl_16.Text = .ZZCODE_KRAFTSTOF
                                lbl_21.Text = .ZZFASSVERMOEGEN
                                lbl_17.Text = .ZZCO2KOMBI
                                lbl_18.Text = .ZZSLD & " / " & .ZZNATIONALE_EMIK
                                lbl_22.Text = .ZZABGASRICHTL_TG
                                lbl_23.Text = .ZZANZACHS.TrimStart("0"c)
                                lbl_24.Text = .ZZANTRIEBSACHS.TrimStart("0"c)
                                lbl_26.Text = .ZZANZSITZE.TrimStart("0"c)
                                lbl_25.Text = .ZZACHSL_A1_STA.TrimStart("0"c) & ", " & .ZZACHSL_A2_STA.TrimStart("0"c) & ", " & .ZZACHSL_A3_STA.TrimStart("0"c)
                                If Right(lbl_25.Text, 2) = ", " Then
                                    lbl_25.Text = Left(lbl_25.Text, lbl_25.Text.Length - 2)
                                End If
                                lbl_27.Text = .ZZBEREIFACHSE1 & ", " & .ZZBEREIFACHSE2 & ", " & .ZZBEREIFACHSE3
                                If Right(lbl_27.Text, 2) = ", " Then
                                    lbl_27.Text = Left(lbl_27.Text, lbl_27.Text.Length - 2)
                                End If

                                lbl_28.Text = .ZZZULGESGEW.TrimStart("0"c)
                                lbl_29.Text = .ZZTYP_SCHL
                                lbl_30.Text = .ZZBEMER1 & "<br>" & .ZZBEMER2 & "<br>" & .ZZBEMER3 & "<br>" & .ZZBEMER4 & "<br>" & .ZZBEMER5 & "<br>" & .ZZBEMER6 & "<br>" & .ZZBEMER7 & "<br>" & .ZZBEMER8 & "<br>" & .ZZBEMER9 & "<br>" & .ZZBEMER10 & "<br>" & .ZZBEMER11 & "<br>" & .ZZBEMER12 & "<br>" & .ZZBEMER13 & "<br>" & .ZZBEMER14
                                lbl_31.Text = .ZZLAENGEMIN.TrimStart("0"c)
                                lbl_32.Text = .ZZBREITEMIN.TrimStart("0"c)
                                lbl_33.Text = .ZZHOEHEMIN

                                lbl_00.Text = .ZZFARBE & " (" & .Farbziffer & ")"
                                lbl_55.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                                lbl_91.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                                lbl_92.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                                lbl_93.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                                lbl_94.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                                lbl_95.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                                lbl_96.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                                lbl_97.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                                lbl_98.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                                lbl_99.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"

                                Select Case .Farbziffer
                                    Case 0 : lbl_99.Visible = True
                                    Case 1 : lbl_98.Visible = True
                                    Case 2 : lbl_97.Visible = True
                                    Case 3 : lbl_96.Visible = True
                                    Case 4 : lbl_95.Visible = True
                                    Case 5 : lbl_94.Visible = True
                                    Case 6 : lbl_93.Visible = True
                                    Case 7 : lbl_92.Visible = True
                                    Case 8 : lbl_91.Visible = True
                                    Case 9 : lbl_55.Visible = True
                                    Case Else

                                End Select

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
' $History: Change06_3NEU.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 17.03.11   Time: 9:02
' Updated in $/CKAG/portal/Shared
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 10.03.11   Time: 11:37
' Updated in $/CKAG/portal/Shared
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 10.03.10   Time: 16:43
' Updated in $/CKAG/portal/Shared
' ITA: 2918
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 3.04.08    Time: 17:21
' Created in $/CKAG/portal/shared
' 
' *****************  Version 5  *****************
' User: Uha          Date: 20.06.07   Time: 14:32
' Updated in $/CKG/Portal/Shared
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 4  *****************
' User: Uha          Date: 1.03.07    Time: 16:38
' Updated in $/CKG/Portal/Shared
' 
' ************************************************