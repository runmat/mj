Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary

Namespace Treuhand
    Partial Public Class Change03sAut
        Inherits System.Web.UI.Page

        Private m_User As Base.Kernel.Security.User
        Private m_App As Base.Kernel.Security.App
        Private logApp As Base.Kernel.Logging.Trace

        Dim m_report As Treuhandsperre

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            m_User = GetUser(Me)
            FormAuth(Me, m_User)
            m_App = New Base.Kernel.Security.App(m_User) 'erzeugt ein App_objekt 
            GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

            Dim sAut As String = ""
            If Not Request.QueryString("Aut") = Nothing Then
                If Request.QueryString("Aut").ToString = "@!" Then
                    sAut = Request.QueryString("Aut").ToString()
                End If
            End If
            If Not IsPostBack Then
                FillAutData()
            End If

        End Sub


        Private Sub FillAutData()

            Dim OutPutStream As System.IO.MemoryStream
            Dim formatter As New BinaryFormatter()

            'cmdDelete.Visible = False
            cmdSave.Visible = False
            If (Not Session("Authorization") Is Nothing) AndAlso CBool(Session("Authorization")) AndAlso _
                (Not Session("AuthorizationID") Is Nothing) AndAlso IsNumeric(Session("AuthorizationID")) Then
                OutPutStream = GiveAuthorizationDetails(m_App.Connectionstring, CInt(Session("AuthorizationID")), "objReport")
                If OutPutStream Is Nothing Then
                    lblError.Text = "Keine Daten für den Vorgang vorhanden."
                Else


                    OutPutStream = GiveAuthorizationDetails(m_App.Connectionstring, CInt(Session("AuthorizationID")), "objReport")
                    If OutPutStream Is Nothing Then
                        lblError.Text = "Keine Daten für den Vorgang vorhanden."
                    Else
                        cmdSave.Visible = True
                        formatter = New BinaryFormatter()
                        m_report = New Treuhandsperre(m_User, m_App, Session.SessionID.ToString, Session("AppID").ToString, "")
                        m_report = DirectCast(formatter.Deserialize(OutPutStream), Treuhandsperre)

                        lblFinShow.Text = m_report.FinforAut
                        Dim dRows() As DataRow
                        dRows = m_report.Result.Select("Fahrgestellnummer = '" & m_report.FinforAut & "'")
                        If dRows.Length > 0 Then
                            For Each ResultRow As DataRow In dRows
                                lblTreunehmShow.Text = ResultRow("Treunehmer").ToString
                                lblKennzShow.Text = ResultRow("Kennzeichen").ToString
                                lblAdresseShow.Text = ResultRow("Versandadresse").ToString
                                lblErstellerShow.Text = ResultRow("Ersteller").ToString
                                lblAblehnShow.Text = ResultRow("Ablehnender").ToString
                                lblAblehnGrundShow.Text = ResultRow("Ablehnungsgrund").ToString
                                If m_report.Aktion = "F" Then
                                    lblAktionShow.Text = "Freigabe"
                                Else
                                    lblAktionShow.Text = "Ablehnung"
                                End If
                                lblBELNR.Text = ResultRow("BELNR").ToString
                            Next
                        End If
                        Session("m_report") = m_report
                    End If
                End If
            Else
            lblError.Text = "Fehler bei der Datenübergabe."
            End If
        End Sub

        Private Sub WriteLog(ByVal strMessage As String, ByVal strHaendler As String, Optional ByVal strType As String = "APP")
            Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
            logApp.WriteEntry(strType, m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, strHaendler, strMessage, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 10)
        End Sub

        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
            SetEndASPXAccess(Me)
        End Sub

        Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
            SetEndASPXAccess(Me)
        End Sub

        Protected Sub cmdSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSave.Click
            If Not Session("m_report") Is Nothing Then
                m_report = CType(Session("m_report"), Treuhandsperre)
                m_report.Change(Session("AppID").ToString, Session.SessionID.ToString, Me)
                If m_report.Status < 0 Then
                    lblError.Text = m_report.Message
                Else
                    Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
                    logApp.CollectDetails("Treunehmer", CType(lblTreunehmShow.Text, Object), True)
                    logApp.CollectDetails("Angefordert am", CType(Now.ToShortDateString, Object))
                    logApp.CollectDetails("Fahrgestellnummer", CType(lblFinShow.Text, Object))
                    logApp.CollectDetails("Kennzeichen", CType(lblKennzShow.Text, Object))
                    If m_report.Aktion = "F" Then
                        logApp.CollectDetails("Freigabe", "X")
                        logApp.CollectDetails("Abgelehnt", "")
                    Else
                        logApp.CollectDetails("Freigabe", "")
                        logApp.CollectDetails("Abgelehnt", "X")
                    End If
                    logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, lblTreunehmShow.Text, "Freigabe für Treunehmer " & lblTreunehmShow.Text & " erfolgreich autorisiert.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0, logApp.InputDetails)
                    DeleteAuthorizationEntry(m_App.Connectionstring, CInt(Session("AuthorizationID")))
                    cmdSave.Enabled = False
                    lblNoData.Text = "Vorgang erfolgreich autorisiert."
                End If
            End If

        End Sub

       

        Protected Sub cmdDel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdDel.Click
            Try
                DeleteAuthorizationEntry(m_App.Connectionstring, CInt(Session("AuthorizationID")))
                lblNoData.Text = "<b>Daten wurden aus Autorisierung entfernt.</b><br>&nbsp;"
                Session("Authorization") = Nothing
                Session("AuthorizationID") = Nothing
                cmdSave.Visible = False
                cmdDel.Visible = False
                lbBack.Visible = False

                'zurueck zur Liste oder Hauptmenue
                Dim strLastRecord As String = CStr(Request.QueryString("LastRecord"))
                If Not strLastRecord = "True" Then
                    Response.Redirect("Change48s.aspx?AppID=" & CStr(m_User.Applications.Select("AppName='Change48s'")(0).Item("AppID")) & "&Aut=@!", False)
                Else
                    Response.Redirect("../../../Start/Selection.aspx", False)
                End If
            Catch ex As Exception
                WriteLog("Fehler bei der Löschung des Vorgangs für", lblTreunehmShow.Text & "/" & lblFinShow.Text, "ERR")
                lblError.Text = "Beim Speichern Ihrer Daten ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"

            End Try
        End Sub

        Protected Sub lbBack_Click(sender As Object, e As EventArgs) Handles lbBack.Click
            Response.Redirect("Change48s.aspx?AppID=" & Session("AppID").ToString)
        End Sub
    End Class
End Namespace
