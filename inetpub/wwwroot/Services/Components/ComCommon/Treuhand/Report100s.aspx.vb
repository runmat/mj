Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Components.ComCommon.Treuhand

Namespace Treuhand
    Partial Public Class Report100s
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
            GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

            If IsPostBack = False Then

                SetRadioButtonCustomer()
            ElseIf Not (Session("TreuhandBestand") Is Nothing) Then
                TreuhandBestand = CType(Session("TreuhandBestand"), SperreFreigabe)
            End If

        End Sub

        Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSearch.Click
            If rdbCustomer.SelectedIndex = -1 Then
                lblError.Text = "Bitte wählen Sie einen Treunehmer aus!"
                lblError.Visible = True
            ElseIf Not txtERDatbis.Text.Trim.Length + txtERDatvon.Text.Trim.Length > 0 Then
                lblError.Text = "Bitte geben Sie einen Zeitraum an!"
                lblError.Visible = True
            Else
                TreuhandBestand.Treunehmer = rdbCustomer.SelectedValue.Split("|"c)(0).ToString
                TreuhandBestand.Erdatvon = txtERDatvon.Text
                TreuhandBestand.Erdatbis = txtERDatbis.Text
                TreuhandBestand.GetTreuhandBestand(Me.Page, Session("AppID").ToString, Session.SessionID.ToString)
                If TreuhandBestand.E_SUBRC = "0" Then
                    If TreuhandBestand.Result.Rows.Count > 0 Then
                        Session("TreuhandBestand") = TreuhandBestand
                        Response.Redirect("Report100s_2.aspx?AppID=" & Session("AppID").ToString)
                    Else
                        lblError.Text = "Es wurden keine Daten gefunden!"
                        lblError.Visible = True
                    End If
                Else
                    lblError.Visible = True
                    lblError.Text = TreuhandBestand.E_MESSAGE
                End If
            End If

        End Sub
#End Region

#Region "Methods"

        Private Sub SetRadioButtonCustomer()

            TreuhandBestand = New SperreFreigabe(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")

            TreuhandBestand.GetCustomer(Me.Page, Session("AppID").ToString, Session.SessionID.ToString)

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


        Protected Sub lbBack_Click(sender As Object, e As EventArgs) Handles lbBack.Click
            Session("TreuhandBestand") = Nothing
            Response.Redirect("/Services/(S(" & Session.SessionID & "))/Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
        End Sub
    End Class
End Namespace
