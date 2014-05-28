Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Business

Namespace Logistik
    Partial Public Class Change01
        Inherits System.Web.UI.Page
        Private m_App As App
        Private m_User As User
        Private m_change As Logistik.Logistik1

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            m_User = GetUser(Me)
            FormAuth(Me, m_User)
            GetAppIDFromQueryString(Me)

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            m_App = New App(m_User)

            lblError.Text = ""
            Try
                If Session("AppChange") Is Nothing Then
                    m_change = New Logistik.Logistik1(m_User, m_App, CStr(Request.QueryString("AppID")), Session.SessionID.ToString, "")
                    Session("AppChange") = m_change
                Else
                    m_change = CType(Session("AppChange"), Logistik.Logistik1)
                End If

            Catch ex As Exception
                lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten:<br>" & ex.Message
            End Try
        End Sub

        Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
            Response.Redirect("/Services/(S(" & Session.SessionID & "))/Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
        End Sub

        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
            SetEndASPXAccess(Me)
        End Sub

        Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
            SetEndASPXAccess(Me)
        End Sub

        Protected Sub lbCreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbCreate.Click
            DoSubmit()
        End Sub

        Private Sub DoSubmit()
            If Session("AppChange") Is Nothing Then
                m_change = New Logistik.Logistik1(m_User, m_App, CStr(Request.QueryString("AppID")), Session.SessionID.ToString, "")
                Session("AppChange") = m_change
            Else
                m_change = CType(Session("AppChange"), Logistik.Logistik1)
            End If
            Try


                If checkDate() Then

                    m_change.Fahrgestellnr = txtFahrgestellnummer.Text.Trim.ToUpper
                    m_change.Kennzeichen = txtAmtlKennzeichen.Text.Trim.ToUpper
                    m_change.MietendVon = txtMietendeVon.Text.Trim
                    m_change.Mietendbis = txtMietendeBis.Text.Trim

                    m_change.Show(Session("AppID").ToString, Session.SessionID, Me)
                    FillLieferAdresse()
                    If m_change.Status = 0 Then
                        Session.Add("AppChange", m_change)
                        Dim Parameterlist As String = ""
                        HelpProcedures.getAppParameters(Session("AppID").ToString, Parameterlist, ConfigurationManager.AppSettings.Get("Connectionstring"))
                        Response.Redirect("Change01_2.aspx?AppID=" & Session("AppID").ToString & Parameterlist)
                    Else
                        lblError.Text = m_change.Message
                        Exit Sub
                    End If
                End If

            Catch ex As Exception
                lblError.Text = "Es ist ein Fehler aufgetreten.(" & ex.Message & ")"
            Finally

            End Try
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

        Private Sub FillLieferAdresse()
            m_change.SORTL = ""
            m_change.FillLiefAdressen(Session("AppID").ToString, Session.SessionID, Me)
            If m_change.Status = 0 Then
                Session.Add("AppChange", m_change)
                If m_change.LiefAdressen.Rows.Count > 0 Then
                    Dim dRow() As DataRow = m_change.LiefAdressen.Select("EIKTO='" & m_User.Reference & "'")
                    If dRow.Length = 1 Then
                        m_change.SAPNr = dRow(0)("KUNNR").ToString
                    End If
                End If

            Else
                lblError.Text = m_change.Message
                Exit Sub
            End If
        End Sub


    End Class

End Namespace