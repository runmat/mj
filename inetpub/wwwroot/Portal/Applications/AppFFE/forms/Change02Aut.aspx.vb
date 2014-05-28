Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business

Imports System
Imports System.IO
Imports System.Collections
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization


Partial Public Class Change02Aut
    Inherits System.Web.UI.Page


    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private objSuche As FFE_Search
    Private objFDDBank As FFE_BankBase
    Private objFDDBank2 As FFE_Bank_2
    Private logApp As Base.Kernel.Logging.Trace

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblPageTitle.Text = "Werte ändern"

        lblInformation.Text = ""
        lblError.Text = ""

        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        Try
            m_App = New Base.Kernel.Security.App(m_User)

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text

            Dim OutPutStream As System.IO.MemoryStream
            Dim formatter As New BinaryFormatter()

            cmdDelete.Visible = False
            cmdBack.Visible = False
            cmdAuthorize.Visible = False
            If (Not Session("Authorization") Is Nothing) AndAlso CBool(Session("Authorization")) AndAlso _
                (Not Session("AuthorizationID") Is Nothing) AndAlso IsNumeric(Session("AuthorizationID")) Then
                OutPutStream = GiveAuthorizationDetails(m_App.Connectionstring, CInt(Session("AuthorizationID")), "objSuche")
                If OutPutStream Is Nothing Then
                    lblError.Text = "Keine Daten für den Vorgang vorhanden."
                Else
                    formatter = New BinaryFormatter()
                    objSuche = New FFE_Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
                    objSuche = DirectCast(formatter.Deserialize(OutPutStream), FFE_Search)
                    objSuche.ReNewSAPDestination(Session.SessionID.ToString, Session("AppID").ToString)

                    OutPutStream = GiveAuthorizationDetails(m_App.Connectionstring, CInt(Session("AuthorizationID")), "objFDDBank")
                    If OutPutStream Is Nothing Then
                        lblError.Text = "Keine Daten für den Vorgang vorhanden."
                    Else
                        formatter = New BinaryFormatter()
                        objFDDBank = New FFE_BankBase(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                        objFDDBank = DirectCast(formatter.Deserialize(OutPutStream), FFE_BankBase)

                        OutPutStream = GiveAuthorizationDetails(m_App.Connectionstring, CInt(Session("AuthorizationID")), "objFDDBank2")
                        If OutPutStream Is Nothing Then
                            lblError.Text = "Keine Daten für den Vorgang vorhanden."
                        Else
                            formatter = New BinaryFormatter()
                            objFDDBank2 = New FFE_Bank_2(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                            objFDDBank2 = DirectCast(formatter.Deserialize(OutPutStream), FFE_Bank_2)

                            If objSuche.LeseHaendlerSAP_Einzeln(Session("AppID").ToString, Session.SessionID.ToString, Session("SelectedDealer").ToString) Then
                                lblHaendlerNummer.Text = objSuche.REFERENZ
                                lblHaendlerName.Text = objSuche.NAME
                                If objSuche.NAME_2.Length > 0 Then
                                    lblHaendlerName.Text &= "<br>" & objSuche.NAME_2
                                End If
                                lblAdresse.Text = objSuche.COUNTRYISO & " - " & objSuche.POSTL_CODE & " " & objSuche.CITY & "<br>" & objSuche.STREET

                                Dim rowsTemp() As DataRow = objFDDBank2.Auftraege.Select("VBELN='" & objFDDBank2.AuftragsNummer.TrimStart("0"c) & "'") '.TrimStart("0"c)
                                lblVertragsnummer.Text = rowsTemp(0)("ZZREFNR").ToString
                                lblFahrgestellnummer.Text = rowsTemp(0)("ZZFAHRG").ToString
                                If Not rowsTemp(0)("ZZANFDT") Is System.DBNull.Value Then
                                    lblAngefordertAm.Text = rowsTemp(0)("ZZANFDT").ToString
                                Else
                                    lblAngefordertAm.Text = ""
                                End If
                                lblKontingentart.Text = rowsTemp(0)("BSTZD").ToString
                                lblBriefnummer.Text = rowsTemp(0)("ZZBRIEF").ToString

                                Dim strTemp As String = objFDDBank2.Kunde
                                lblKunde.Text = strTemp
                                'strTemp = Right(objFDDBank2.Faelligkeit, 2) & "." & Mid(objFDDBank2.Faelligkeit, 5, 2) & "." & Left(objFDDBank2.Faelligkeit, 4)
                                'If strTemp = ".." Then strTemp = ""
                                strTemp = objFDDBank2.Faelligkeit
                                lblFaelligkeit.Text = strTemp
                                If rowsTemp(0)("BSTZD").ToString = "Retail" Then
                                    TR_Betrag.Visible = True
                                    strTemp = rowsTemp(0)("ANSWT").ToString
                                    If IsNumeric(strTemp) Then
                                        lblBetrag.Text = Format(CDec(strTemp), "##,##0.00 €")
                                    Else
                                        lblBetrag.Text = "0,00 €"
                                    End If
                                End If
                                rdbStorno.Checked = objFDDBank2.Storno
                                rdbFreigabe.Checked = Not objFDDBank2.Storno

                                logApp = New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
                                logApp.CollectDetails("Kundennummer", CType(objFDDBank.Customer.TrimStart("0"c), Object), True)
                                logApp.CollectDetails("Finanzierungsnr.", CType(lblVertragsnummer.Text, Object))
                                logApp.CollectDetails("Angefordert", CType(lblAngefordertAm.Text, Object))
                                logApp.CollectDetails("Fahrgestellnummer", CType(lblFahrgestellnummer.Text, Object))
                                logApp.CollectDetails("ZBII-Nummer", CType(lblBriefnummer.Text, Object))
                                logApp.CollectDetails("Storno", CType(rdbStorno.Checked, Object))
                                logApp.CollectDetails("Freigabe", CType(rdbFreigabe.Checked, Object))
                                logApp.CollectDetails("Kunde", CType(lblKunde.Text, Object))
                                logApp.CollectDetails("Fälligkeit", CType(lblFaelligkeit.Text, Object))

                                cmdDelete.Visible = True
                                cmdBack.Visible = True
                                cmdAuthorize.Visible = True
                            Else
                                lblError.Text = "Fehler bei der Ermittlung der Händlerdaten.<br>(" & objSuche.ErrorMessage & ")"
                            End If
                        End If
                    End If
                End If
            Else
                lblError.Text = "Fehler bei der Datenübergabe."
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub DoSubmit3()
        Try
            objFDDBank2.SessionID = Session.SessionID.ToString
            objFDDBank2.Change()
            If objFDDBank2.Status = 0 Then
                'mein kein log weil es eigentlich zur autorisierung gehört!
                'WriteLog("Freigabe für Händler " & objSuche.REFERENZ & " erfolgreich durchgeführt.")
                logApp.WriteStandardDataAccessSAP(objFDDBank2.IDSAP)
                DeleteAuthorizationEntry(m_App.Connectionstring, CInt(Session("AuthorizationID")))
                Session("Authorization") = Nothing
                Session("AuthorizationID") = Nothing
                cmdAuthorize.Visible = False
                cmdDelete.Visible = False
                cmdBack.Visible = False
                lblInformation.Text = "<b>Ihre Daten wurden gespeichert.</b><br>&nbsp;"
                ConfirmMessage.Visible = True

                ' da für die benutzeraktivitäten ein logeintrag existieren muss, wird hier einer für die Autorisierung gefälscht! JJU20080626
                HelpProcedures.fakeLogEntryForAutorisierung(logApp, m_User, Session.SessionID, Session("AppID"), "Die Autorisierung der Freigabe für Händler " & objSuche.REFERENZ & " erfolgreich durchgeführt")


                'zurueck zur Liste oder Hauptmenue
                Dim strLastRecord As String = CStr(Request.QueryString("LastRecord"))
                If Not strLastRecord = "True" Then
                    Response.Redirect("Change14.aspx?AppID=" & CStr(m_User.Applications.Select("AppName='Change14'")(0).Item("AppID")) & "&Aut=@!", False)
                Else
                    Response.Redirect("../../../Start/Selection.aspx", False)
                End If
            Else
                'mein kein log weil es eigentlich zur autorisierung gehört!
                'WriteLog("Fehler bei der Brieffreigabe für Händler " & objSuche.REFERENZ & " , (Fehler: " & objFDDBank2.Message & ")", "ERR")
                HelpProcedures.fakeLogEntryForAutorisierung(logApp, m_User, Session.SessionID, Session("AppID"), "Fehler bei der Autorisierung der Brieffreigabe für Händler " & objSuche.REFERENZ & " , (Fehler: " & objFDDBank2.Message & ")", "ERR")

                logApp.WriteStandardDataAccessSAP(objFDDBank2.IDSAP)
                lblError.Text = "Beim Speichern Ihrer Daten ist ein Fehler aufgetreten.<br>(" & objFDDBank2.Message & ")"
                ConfirmMessage.Visible = False
            End If

        Catch ex As Exception
            'mein kein log weil es eigentlich zur autorisierung gehört!
            'WriteLog("Fehler bei der Freigabe für Händler " & objSuche.REFERENZ & " , (Fehler: " & ex.Message & ")", "ERR")
            HelpProcedures.fakeLogEntryForAutorisierung(logApp, m_User, Session.SessionID, Session("AppID"), "Fehler bei der Autorisierung der Brieffreigabe für Händler " & objSuche.REFERENZ & " , (Fehler: " & ex.Message & ")", "ERR")
            lblError.Text = "Beim Speichern Ihrer Daten ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            ConfirmMessage.Visible = False
        End Try
    End Sub

    Private Sub DoSubmit4()
        Try
            DeleteAuthorizationEntry(m_App.Connectionstring, CInt(Session("AuthorizationID")))
            'WriteLog("Freigabe für Händler " & objSuche.REFERENZ & " aus Autorisierung gelöscht.")
            HelpProcedures.fakeLogEntryForAutorisierung(logApp, m_User, Session.SessionID, Session("AppID"), "Freigabe für Händler " & objSuche.REFERENZ & " aus Autorisierung gelöscht.")
            lblInformation.Text = "<b>Daten wurden aus Autorisierung entfernt.</b><br>&nbsp;"
            Session("Authorization") = Nothing
            Session("AuthorizationID") = Nothing
            cmdAuthorize.Visible = False
            cmdDelete.Visible = False
            cmdBack.Visible = False
            ConfirmMessage.Visible = True
            'zurueck zur Liste oder Hauptmenue
            Dim strLastRecord As String = CStr(Request.QueryString("LastRecord"))
            If Not strLastRecord = "True" Then
                Response.Redirect("Change14.aspx?AppID=" & CStr(m_User.Applications.Select("AppName='Change14'")(0).Item("AppID")) & "&Aut=@!", False)
            Else
                Response.Redirect("../../../Start/Selection.aspx", False)
            End If
        Catch ex As Exception
            WriteLog("Fehler bei der Löschung einer Freigabe für Händler " & objSuche.REFERENZ & " , (Fehler: " & ex.Message & ")", "ERR")
            lblError.Text = "Beim Speichern Ihrer Daten ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            ConfirmMessage.Visible = False
        End Try
    End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        If lblError.Text.Length = 0 Then
            DoSubmit4()
        End If
    End Sub

    Private Sub cmdAuthorize_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAuthorize.Click
        If lblError.Text.Length = 0 Then
            DoSubmit3()
        End If
    End Sub

    Private Sub WriteLog(ByVal strMessage As String, Optional ByVal strType As String = "APP")
        logApp.WriteEntry(strType, m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, Right(Session("SelectedDealer").ToString, 5), strMessage, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 10, logApp.InputDetails)
    End Sub

    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBack.Click
        Response.Redirect("Change14.aspx?AppID=" & CStr(m_User.Applications.Select("AppName='Change14'")(0).Item("AppID")) & "&Aut=@!", False)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class
' ************************************************
' $History: Change02Aut.aspx.vb $
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 27.06.08   Time: 11:18
' Updated in $/CKAG/Applications/AppFFE/forms
' ITA 2043/2032 BugFix �bersicht Benutzeraktivit�ten bei Punkt
' Autorisierung 
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 10.06.08   Time: 17:24
' Updated in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 21.05.08   Time: 16:34
' Updated in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 15.05.08   Time: 14:33
' Updated in $/CKAG/Applications/AppFFE/forms
' ITA:1865
' 
' ************************************************
