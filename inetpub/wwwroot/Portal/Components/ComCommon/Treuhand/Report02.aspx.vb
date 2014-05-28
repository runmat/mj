Imports CKG.Base.Kernel.Common.Common

Namespace Treuhand
    Partial Public Class Report02
        Inherits System.Web.UI.Page

#Region "Declarations"
        Private m_User As Base.Kernel.Security.User
        Private m_App As Base.Kernel.Security.App
        Private TreuhandBestand As SperreFreigabe
#End Region

#Region "Events"

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            m_User = GetUser(Me) ' füllen Form.Session("objUser"), rückgabe eines UserObjekte
            FormAuth(Me, m_User)
            m_App = New Base.Kernel.Security.App(m_User) 'erzeugt ein App_objekt 
            ucHeader.InitUser(m_User)
            GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text

            If IsPostBack = False Then

                SetRadioButtonCustomer()
            ElseIf Not (Session("TreuhandBestand") Is Nothing) Then
                TreuhandBestand = CType(Session("TreuhandBestand"), SperreFreigabe)
            End If

        End Sub
        Protected Sub cmdWeiter_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdWeiter.Click
            If rdbCustomer.SelectedIndex = -1 Then
                lblError.Text = "Bitte wählen Sie einen Treunehmer aus!"

            ElseIf Not txtERDatbis.Text.Trim.Length + txtERDatvon.Text.Trim.Length > 0 Then
                lblError.Text = "Bitte geben Sie einen Zeitraum an!"
            Else
                TreuhandBestand.Erdatvon = txtERDatvon.Text
                TreuhandBestand.Erdatbis = txtERDatbis.Text
                TreuhandBestand.Treunehmer = rdbCustomer.SelectedValue.Split("|"c)(0).ToString
                TreuhandBestand.GetTreuhandBestand(Me.Page, Session("AppID").ToString, Session.SessionID.ToString)
                If TreuhandBestand.E_SUBRC = "0" Then
                    If TreuhandBestand.Result.Rows.Count > 0 Then
                        Session("TreuhandBestand") = TreuhandBestand
                        Response.Redirect("Report02_2.aspx?AppID=" & Session("AppID").ToString)
                    Else
                        lblError.Text = "Es wurden keine Daten gefunden!"
                    End If
                Else
                    lblError.Text = TreuhandBestand.E_MESSAGE
                End If
            End If
        End Sub

#End Region

#Region "Methods"

        Private Sub SetRadioButtonCustomer()

            TreuhandBestand = New SperreFreigabe(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")

            TreuhandBestand.GetCustomer(Session("AppID").ToString, Session.SessionID.ToString)

            If TreuhandBestand.Result.Rows.Count > 0 Then

                Dim rdbText As String
                Dim rdbValue As String


                For xAGS As Integer = 0 To TreuhandBestand.Result.Rows.Count - 1
                    Dim rdvitem As New ListItem

                    If TreuhandBestand.Result.Rows(xAGS)("ZSELECT").ToString = "TG" Then
                        rdbText = TreuhandBestand.Result.Rows(xAGS)("NAME1_AG").ToString
                    Else
                        rdbText = TreuhandBestand.Result.Rows(xAGS)("NAME1_TG").ToString
                    End If

                    rdbValue = TreuhandBestand.Result.Rows(xAGS)("AG").ToString & "|" & TreuhandBestand.Result.Rows(xAGS)("TREU").ToString

                    rdvitem.Text = rdbText
                    rdvitem.Value = rdbValue
                    rdbCustomer.Items.Add(rdvitem)
                Next
            End If

            Session("TreuhandBestand") = TreuhandBestand
        End Sub

#End Region


    End Class
End Namespace
