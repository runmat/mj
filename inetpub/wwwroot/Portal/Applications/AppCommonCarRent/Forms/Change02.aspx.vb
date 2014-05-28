Option Explicit On
Option Strict On

Imports CKG
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Change02
    Inherits System.Web.UI.Page
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles

    Protected WithEvents lb_weiter As LinkButton
    Protected WithEvents lb_Back As LinkButton
    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App

    Protected WithEvents txtHaendlernr As TextBox
    Protected WithEvents txtKennzeichen As TextBox
    Protected WithEvents txtFahrgestellnummer As TextBox
    Protected WithEvents txt_RetourDateVon As TextBox
    Protected WithEvents txt_RetourDateBis As TextBox
    Protected WithEvents txt_AbmeldeDateVon As TextBox
    Protected WithEvents txt_AbmeldeDateBis As TextBox
    Protected WithEvents CompareValidator1 As CompareValidator
    Protected WithEvents rbl_Bezahlt As RadioButtonList
    Protected WithEvents rbl_Gesperrt As RadioButtonList
    Protected WithEvents btnEmpty As ImageButton

    Private mObjBriefanforderung As Briefanforderung


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

        ' Hier Benutzercode zur Seiteninitialisierung einfügen
        Try
            m_User = GetUser(Me)
            m_App = New Base.Kernel.Security.App(m_User) 'erzeugt ein App_objekt 
            ucHeader.InitUser(m_User)
            FormAuth(Me, m_User)
            lblError.Text = ""


            If Not IsPostBack Then
                GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")
                Select Case Me.Request.QueryString.Item("Art").ToUpper
                    Case "ENDG"
                        lblHead.Text = "Endgültiger Dokumentenversand"
                    Case "TEMP"
                        lblHead.Text = "Temporärer Dokumentenversand"
                    Case "BZKENNZ"
                        lblHead.Text = "Bezahltkennzeichen setzen und aufheben"
                    Case "SPERR"
                        lblHead.Text = "Versandsperre setzen und aufheben"
                    Case Else
                        Throw New Exception("Fehlender Querystringparameter 'Art' Vorgang wird abgebrochen.")
                End Select
                ucStyles.TitleText = lblHead.Text
            End If

        Catch ex As Exception
            lblError.Text = "beim laden der Seite ist ein Fehler aufgetreten: " & ex.Message
        End Try
    End Sub

    Private Sub lb_Weiter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_weiter.Click
        doSubmit()
    End Sub

    Private Function checkDate() As Boolean
        Dim tmpbool As Boolean = True
        For Each ctrl As BaseValidator In Me.Validators
            If ctrl.IsValid = False Then
                tmpbool = False
            End If
        Next
        Return tmpbool
    End Function

    Private Sub doSubmit()

        If (txtFahrgestellnummer.Text.Replace("*", "").Trim(" "c) = "" AndAlso _
            txtKennzeichen.Text.Replace("*", "").Trim(" "c) = "" AndAlso _
            txtHaendlernr.Text.Replace("*", "").Trim(" "c) = "" AndAlso _
            txt_RetourDateVon.Text.Trim(" "c) = "" AndAlso _
            txt_RetourDateBis.Text.Trim(" "c) = "" AndAlso _
            txt_AbmeldeDateBis.Text.Trim(" "c) = "" AndAlso _
            txt_AbmeldeDateVon.Text.Trim(" "c) = "") Then

            lblError.Text = "Geben Sie bitte ein Suchkriterium ein"
        Else
            If checkDate() Then



                mObjBriefanforderung = New Briefanforderung(m_User, m_App, Session("AppId").ToString, Me.Session.SessionID, "")
                mObjBriefanforderung.SucheFahrgestellnummer = txtFahrgestellnummer.Text.Trim(" "c)
                mObjBriefanforderung.SucheKennzeichen = txtKennzeichen.Text.Trim(" "c)
                mObjBriefanforderung.SucheHaendlernr = txtHaendlernr.Text.Trim(" "c)
                If txt_RetourDateVon.Text.Trim(" "c) <> "" Then
                    mObjBriefanforderung.RetourdatVon = txt_RetourDateVon.Text.Trim(" "c)
                End If
                If txt_RetourDateBis.Text.Trim(" "c) <> "" Then
                    mObjBriefanforderung.RetourdatBis = txt_RetourDateBis.Text.Trim(" "c)
                End If
                If txt_AbmeldeDateVon.Text.Trim(" "c) <> "" Then
                    mObjBriefanforderung.AbmeldeDatVon = txt_AbmeldeDateVon.Text.Trim(" "c)
                End If
                If txt_AbmeldeDateBis.Text.Trim(" "c) <> "" Then
                    mObjBriefanforderung.AbmeldeDatbis = txt_AbmeldeDateBis.Text.Trim(" "c)
                End If

                Select Case rbl_Bezahlt.SelectedValue
                    Case "0"
                        mObjBriefanforderung.Bezahlt = Nothing
                    Case "1"
                        mObjBriefanforderung.Bezahlt = "X"
                    Case "2"
                        mObjBriefanforderung.Bezahlt = " "
                End Select

                Select Case rbl_Gesperrt.SelectedValue
                    Case "0"
                        mObjBriefanforderung.Gesperrt = Nothing
                    Case "1"
                        mObjBriefanforderung.Gesperrt = "X"
                    Case "2"
                        mObjBriefanforderung.Gesperrt = " "
                End Select

                mObjBriefanforderung.Show(Session("AppID").ToString, Session.SessionID, Me)
                If mObjBriefanforderung.Status = 0 Then
                    Session.Add("mObjBriefanforderungSession", mObjBriefanforderung)
                    Dim Parameterlist As String = Me.Request.QueryString.Item("Art").ToUpper
                    
                    Select Case Parameterlist
                        Case "ENDG"
                            Parameterlist = "&Art=ENDG"
                        Case "TEMP"
                            Parameterlist = "&Art=TEMP"
                        Case "BZKENNZ"
                            Parameterlist = "&Art=BZKENNZ"
                        Case "SPERR"
                            Parameterlist = "&Art=SPERR"
                        Case Else
                            Throw New Exception("Fehlender Querystringparameter 'Art' Vorgang wird abgebrochen.")
                    End Select


                    Response.Redirect("Change02_1.aspx?AppID=" & Session("AppID").ToString & Parameterlist)
                End If
            Else
                lblError.Text = mObjBriefanforderung.Message
                Exit Sub
            End If
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Protected Sub lb_Back_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_Back.Click
        Response.Redirect("Auswahl.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Protected Sub btnEmpty_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnEmpty.Click
        doSubmit()
    End Sub

End Class
' ************************************************
' $History: Change02.aspx.vb $
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 10.06.09   Time: 14:36
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA: 2804
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 20.05.09   Time: 17:01
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 13.05.09   Time: 15:45
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 12.05.09   Time: 13:48
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 9.03.09    Time: 17:25
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA: 2671
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 13.02.09   Time: 13:51
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA 2596 / 2589
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 12.02.09   Time: 17:40
' Created in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA 2596 im test
' 
' ************************************************