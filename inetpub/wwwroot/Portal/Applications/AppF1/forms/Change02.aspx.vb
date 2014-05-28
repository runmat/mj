﻿Imports CKG
Imports CKG.Base.Kernel.Common.Common


Public Class Change02
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
    'Private m_blnDoSubmit As Boolean
    'Private mstrHDL As String
   


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cmdSearch.Visible = True
        m_User = GetUser(Me)
        FormAuth(Me, m_User, True)
        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            m_App = New Base.Kernel.Security.App(m_User)


            If IsPostBack = False Then
                Session("objSuche") = Nothing
                Session("objFDDBank") = Nothing
                Session("objFDDBank2") = Nothing

                If (Not Session("Authorization") Is Nothing) AndAlso (CBool(Session("Authorization"))) Then
                    If (Not Session("AuthorizationID") Is Nothing) AndAlso CInt(Session("AuthorizationID")) > 0 Then
                        Dim strLastRecordParam As String = ""
                        If CStr(Request.QueryString("LastRecord")) = "True" Then
                            strLastRecordParam = "?LastRecord=True"
                        End If
                        Response.Redirect("Change02Aut.aspx" & strLastRecordParam)
                    End If
                End If


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
                Response.Redirect("Change02_2.aspx?AppID=" & Session("AppID").ToString, False)
            Else
                Response.Redirect("Change02_1.aspx?AppID=" & Session("AppID").ToString, False)
            End If
        Else 'händler fall
            Response.Redirect("Change02_2.aspx?AppID=" & Session("AppID").ToString, False)
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
' $History: Change02.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 16.03.09   Time: 10:26
' Updated in $/CKAG/Applications/AppF1/forms
' ita 2685
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 10.03.09   Time: 10:48
' Updated in $/CKAG/Applications/AppF1/forms
' ITA 2675 unfertig
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 9.03.09    Time: 17:49
' Created in $/CKAG/Applications/AppF1/forms
' ITA 2675 unfertig
' 
'
' ************************************************
