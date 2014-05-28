Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Partial Public Class Ueberfg_00_NL
    Inherits System.Web.UI.Page

#Region "Declarations"
    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            Session("NLKunnr") = Nothing

            If Not IsPostBack Then

                Dim uebf As New Ueberfuehrung(m_User, m_App, Session("AppID").ToString, Session.SessionID, String.Empty)
                Dim dv As DataView = uebf.readGroupData(Me)
                If Not dv Is Nothing Then
                    If dv.Table.Rows.Count > 1 Then
                        With ddlKunde
                            .DataSource = dv
                            .DataValueField = "KUNNR"
                            .DataTextField = "Anzeige"
                            .DataBind()
                        End With
                    Else
                        If dv.Table.Rows.Count = 1 Then
                            Session.Add("NLKunnr", dv.Table.Rows(0)("KUNNR"))
                            Response.Redirect("Ueberfg_00.aspx?AppID=" & Session("AppID").ToString)
                        Else
                            lblError.Text = "Keine Kundendaten zur Auswahl gepflegt. Bitte wenden Sie sich an Ihren Ansprechpartner beim DAD."
                            cmdCreate.Visible = False
                            trKUNNR.Visible = False
                        End If

                    End If
                End If
            End If
            If trKUNNR.Visible And ddlKunde.Items.Count = 0 Then
                ddlKunde.Visible = False
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        Session("Ueberf") = Nothing
        Session.Add("NLKunnr", ddlKunde.SelectedItem.Value)
        Response.Redirect("Ueberfg_00.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Protected Sub cmdBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdBack.Click
        Response.Redirect("../../../Start/Selection.aspx?AppID=" & Session("AppID").ToString)
    End Sub

#End Region





End Class