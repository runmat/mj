﻿Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security
Imports System.Runtime.Serialization.Formatters.Binary

Partial Public Class Change101Aut
    Inherits Page
    Private m_App As App
    Private m_User As User
    Private m_Versand As Briefversand

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
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

        Dim OutPutStream As IO.MemoryStream
        Dim formatter As BinaryFormatter

        cmdSave.Visible = False
        If (Not Session("Authorization") Is Nothing) AndAlso CBool(Session("Authorization")) AndAlso _
            (Not Session("AuthorizationID") Is Nothing) AndAlso IsNumeric(Session("AuthorizationID")) Then
            OutPutStream = GiveAuthorizationDetails(m_App.Connectionstring, CInt(Session("AuthorizationID")), "VersandObject")
            If OutPutStream Is Nothing Then
                lblError.Text = "Keine Daten für den Vorgang vorhanden."
            Else


                OutPutStream = GiveAuthorizationDetails(m_App.Connectionstring, CInt(Session("AuthorizationID")), "VersandObject")
                If OutPutStream Is Nothing Then
                    lblError.Text = "Keine Daten für den Vorgang vorhanden."
                Else
                    cmdSave.Visible = True
                    formatter = New BinaryFormatter()
                    m_Versand = New Briefversand(m_User, m_App, Session.SessionID.ToString, Session("AppID").ToString, "")
                    m_Versand = DirectCast(formatter.Deserialize(OutPutStream), Briefversand)

                    Dim dRows() As DataRow
                    dRows = m_Versand.Fahrzeuge.Select("EQUNR = '" & m_Versand.ReferenceforAut & "'")
                    If dRows.Length > 0 Then
                        For Each ResultRow As DataRow In dRows
                            lblFahrgestellnummerShow.Text = ResultRow("Fahrgestellnummer").ToString
                            lblLeasingnummerShow.Text = ResultRow("Leasingnummer").ToString
                            lblNummerZBIIShow.Text = ResultRow("NummerZBII").ToString
                            lblReferenz1Show.Text = ResultRow("Referenz1").ToString
                            lblReferenz2Show.Text = ResultRow("Referenz2").ToString
                            lblSachbearbeiterShow.Text = m_Versand.Sachbearbeiter
                            lblVersArtShow.Text = m_Versand.VersartText
                            lblGrundShow.Text = m_Versand.VersgrundText
                            lblOptionsShow.Text = m_Versand.VersandOptionen.Select("EAN11 = '" & m_Versand.Materialnummer & "'")(0)("ASKTX").ToString
                            lblAdrShow.Text = m_Versand.VersandAdresseText
                        Next
                    End If

                    Session("m_Versand") = m_Versand
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

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Protected Sub cmdSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSave.Click
        If Not Session("m_Versand") Is Nothing Then
            m_Versand = CType(Session("m_Versand"), Briefversand)
            m_Versand.AnfordernAusAutorisierung2(Session("AppID").ToString, Session.SessionID.ToString, Page)
            If m_Versand.Status <> 0 Then
                lblError.Text = m_Versand.Message
            Else
                Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)

                logApp.CollectDetails("Fahrgestellnr.", CType(lblFahrgestellnummerShow.Text, Object), True)
                logApp.CollectDetails("Nummer ZBII", CType(lblNummerZBIIShow.Text, Object))
                logApp.CollectDetails("Leasingnummer", CType(lblLeasingnummerShow.Text, Object))
                logApp.CollectDetails("Referenz1", CType(lblReferenz1Show.Text, Object))
                logApp.CollectDetails("Referenz2", CType(lblReferenz2Show.Text, Object))
                logApp.CollectDetails("Versandart", CType(lblVersArtShow.Text, Object))
                logApp.CollectDetails("Versandgrund", CType(lblGrundShow.Text, Object))
                logApp.CollectDetails("Versandoption", CType(lblOptionsShow.Text, Object))
                logApp.CollectDetails("Sachbearbeiter", CType(m_Versand.Sachbearbeiter, Object))
                logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, lblFahrgestellnummerShow.Text, "Briefversand für " & lblFahrgestellnummerShow.Text & " erfolgreich autorisiert.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0, logApp.InputDetails)

            End If

            cmdSave.Visible = False
            cmdDel.Visible = False
            Session("m_Versand") = m_Versand 'Auftragsnummer
            DeleteAuthorizationEntry(m_App.Connectionstring, CInt(Session("AuthorizationID")))

            lblNoData.Visible = True

            If m_Versand.Status = 0 Then
                lblNoData.ForeColor = Drawing.ColorTranslator.FromHtml("#52C529")
                lblNoData.Text = "Der Vorgang wurde erfolgreich autorisiert!"
                Dim strLastRecord As String = CStr(Request.QueryString("LastRecord"))
                If Not strLastRecord = "True" Then
                    cmdNext.Visible = True
                Else
                    cmdBack.Visible = True
                End If
            Else
                lblError.Text = "Fehler bei der Autorisierung des Vorganges!"

            End If
        End If

    End Sub

    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        Response.Redirect("../Treuhand/Change48s.aspx?AppID=" & CStr(m_User.Applications.Select("AppName='Change48s'")(0).Item("AppID")) & "&Aut=@!", False)
    End Sub

    Protected Sub cmdDel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdDel.Click
        Try
            DeleteAuthorizationEntry(m_App.Connectionstring, CInt(Session("AuthorizationID")))
            lblNoData.Text = "<b>Daten wurden aus Autorisierung entfernt.</b><br>&nbsp;"
            Session("Authorization") = Nothing
            Session("AuthorizationID") = Nothing
            cmdSave.Visible = False
            cmdDel.Visible = False

            'zurueck zur Liste oder Hauptmenue
            Dim strLastRecord As String = CStr(Request.QueryString("LastRecord"))
            If Not strLastRecord = "True" Then
                Response.Redirect("../Treuhand/Change48s.aspx?AppID=" & CStr(m_User.Applications.Select("AppName='Change48s'")(0).Item("AppID")) & "&Aut=@!", False)
            Else
                Response.Redirect("../../../Start/Selection.aspx", False)
            End If
        Catch ex As Exception
            WriteLog("Fehler bei der Löschung des Vorgangs für", lblFahrgestellnummerShow.Text, "ERR")
            lblError.Text = "Beim Speichern Ihrer Daten ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"

        End Try

    End Sub

    Protected Sub cmdNext_Click(sender As Object, e As EventArgs) Handles cmdNext.Click
        Response.Redirect("../Treuhand/Change48s.aspx?AppID=" & CStr(m_User.Applications.Select("AppName='Change48s'")(0).Item("AppID")) & "&Aut=@!", False)
    End Sub

    Protected Sub cmdBack_Click(sender As Object, e As EventArgs) Handles cmdBack.Click
        Response.Redirect("../../../Start/Selection.aspx", False)
    End Sub

End Class