Option Explicit On
Option Strict On

Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Business

Public Class Change04
    Inherits System.Web.UI.Page
    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        m_User = GetUser(Me) ' füllen Form.Session("objUser"), Rückgabe eines UserObjekte 
        m_App = New Base.Kernel.Security.App(m_User) 'erzeugt ein App_objekt 

        FormAuth(Me, m_User)
        ucHeader.InitUser(m_User)
        GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text

        Me.txtSucheKundennr.Focus()

        If Not IsPostBack Then
            Session("Stammdaten") = Nothing
        End If

    End Sub

    Protected Sub lb_Suche_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_Suche.Click

        If Trim(txtSucheKundennr.Text) = String.Empty Then
            lblError.Text = "Bitte geben Sie eine Kundennummer ein."
            Exit Sub
        End If
        Dim objChange04 As New Kernel.Change_04(m_User, m_App, "")

        Dim tblStammdaten As New DataTable()
        Dim tblAnsprechpartner As New DataTable()
        Dim tblVersandbedingungen As New DataTable()
        Dim tblTeam As New DataTable()
        Dim booErr As Boolean = False

        Try
            'Daten abrufen
            objChange04.FillLNKundendaten(Session("AppID").ToString, Session.SessionID, txtSucheKundennr.Text, tblStammdaten,
                                          tblAnsprechpartner, tblTeam, tblVersandbedingungen)

            If Not objChange04.Status = 0 Then
                booErr = True
                lblError.Text = "Fehler bei der Ermittlung der Kundendaten(" & objChange04.Message & ")."
            End If

            Session.Add("Stammdaten", tblStammdaten)
            Session.Add("Ansprechpartner", tblAnsprechpartner)
            Session.Add("Versandbedingungen", tblVersandbedingungen)

            If tblStammdaten Is Nothing OrElse tblStammdaten.Rows.Count = 0 Then
                booErr = True
                lblError.Text = "Kundennummer noch nicht vorhanden."
                Me.lb_Neu.Visible = True
            End If

        Catch ex As Exception
            booErr = True
            If Base.Business.HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) = "NO_DATA" Then
                lblError.Text = "Kundennummer noch nicht vorhanden."
                Me.lb_Neu.Visible = True
            Else
                lblError.Text = "Fehler bei der Ermittlung der Kundendaten(" & ex.Message & ")."
            End If
        End Try

        If booErr = False Then
            Response.Redirect("Change04_2.aspx?AppID=" & Session("AppID").ToString)
        End If

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Protected Sub lb_Neu_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_Neu.Click
        Dim strLNKUNNR As String
        strLNKUNNR = Me.txtSucheKundennr.Text
        Session.Add("LNKUNNR", strLNKUNNR)
        Response.Redirect("Change04_2.aspx?AppID=" & Session("AppID").ToString)
    End Sub
End Class