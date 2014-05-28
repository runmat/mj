Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business

Imports System
Imports System.IO
Imports System.Collections
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization


Public Class Change02Aut
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

    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private objSuche As Search
    Private objFDDBank As BankBaseCredit
    Private objFDDBank2 As FDD_Bank_2
    Private logApp As Base.Kernel.Logging.Trace

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents Hyperlink2 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblInformation As System.Web.UI.WebControls.Label
    Protected WithEvents ConfirmMessage As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblScript As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents cmdAuthorisize As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdDelete As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdAuthorize As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblVertragsnummer As System.Web.UI.WebControls.Label
    Protected WithEvents lblAngefordertAm As System.Web.UI.WebControls.Label
    Protected WithEvents lblFahrgestellnummer As System.Web.UI.WebControls.Label
    Protected WithEvents lblBriefnummer As System.Web.UI.WebControls.Label
    Protected WithEvents lblKontingentart As System.Web.UI.WebControls.Label
    Protected WithEvents rdbStorno As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rdbFreigabe As System.Web.UI.WebControls.RadioButton
    Protected WithEvents lblKunde As System.Web.UI.WebControls.Label
    Protected WithEvents cmdBack As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblFaelligkeit As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHaendlerNummer As System.Web.UI.WebControls.Label
    Protected WithEvents lblHaendlerName As System.Web.UI.WebControls.Label
    Protected WithEvents lblAdresse As System.Web.UI.WebControls.Label
    Protected WithEvents lblBetrag As System.Web.UI.WebControls.Label
    Protected WithEvents TR_Betrag As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
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
                    objSuche = New Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
                    objSuche = DirectCast(formatter.Deserialize(OutPutStream), Search)
                    objSuche.ReNewSAPDestination(Session.SessionID.ToString, Session("AppID").ToString)

                    OutPutStream = GiveAuthorizationDetails(m_App.Connectionstring, CInt(Session("AuthorizationID")), "objFDDBank")
                    If OutPutStream Is Nothing Then
                        lblError.Text = "Keine Daten für den Vorgang vorhanden."
                    Else
                        formatter = New BinaryFormatter()
                        objFDDBank = New BankBaseCredit(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                        objFDDBank = DirectCast(formatter.Deserialize(OutPutStream), BankBaseCredit)

                        OutPutStream = GiveAuthorizationDetails(m_App.Connectionstring, CInt(Session("AuthorizationID")), "objFDDBank2")
                        If OutPutStream Is Nothing Then
                            lblError.Text = "Keine Daten für den Vorgang vorhanden."
                        Else
                            formatter = New BinaryFormatter()
                            objFDDBank2 = New FDD_Bank_2(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                            objFDDBank2 = DirectCast(formatter.Deserialize(OutPutStream), FDD_Bank_2)

                            If objSuche.LeseHaendlerSAP_Einzeln(Session("AppID").ToString, Session.SessionID.ToString, Session("SelectedDealer").ToString) Then
                                lblHaendlerNummer.Text = objSuche.REFERENZ
                                lblHaendlerName.Text = objSuche.NAME
                                If objSuche.NAME_2.Length > 0 Then
                                    lblHaendlerName.Text &= "<br>" & objSuche.NAME_2
                                End If
                                lblAdresse.Text = objSuche.COUNTRYISO & " - " & objSuche.POSTL_CODE & " " & objSuche.CITY & "<br>" & objSuche.STREET

                                Dim rowsTemp() As DataRow = objFDDBank2.Auftraege.Select("Auftragsnummer='" & objFDDBank2.AuftragsNummer.TrimStart("0"c) & "'")
                                lblVertragsnummer.Text = rowsTemp(0)("Vertragsnummer").ToString
                                lblFahrgestellnummer.Text = rowsTemp(0)("Fahrgestellnummer").ToString
                                If Not rowsTemp(0)("Angefordert am") Is System.DBNull.Value Then
                                    lblAngefordertAm.Text = CDate(rowsTemp(0)("Angefordert am")).ToShortDateString
                                Else
                                    lblAngefordertAm.Text = ""
                                End If
                                lblKontingentart.Text = rowsTemp(0)("Kontingentart").ToString
                                lblBriefnummer.Text = rowsTemp(0)("Briefnummer").ToString

                                Dim strTemp As String = objFDDBank2.Kunde
                                lblKunde.Text = strTemp
                                'strTemp = Right(objFDDBank2.Faelligkeit, 2) & "." & Mid(objFDDBank2.Faelligkeit, 5, 2) & "." & Left(objFDDBank2.Faelligkeit, 4)
                                'If strTemp = ".." Then strTemp = ""
                                strTemp = objFDDBank2.Faelligkeit
                                lblFaelligkeit.Text = strTemp
                                If rowsTemp(0)("Kontingentart").ToString = "Retail" Then
                                    TR_Betrag.Visible = True
                                    strTemp = rowsTemp(0)("Betrag").ToString
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
                                logApp.CollectDetails("Vertragsnummer", CType(lblVertragsnummer.Text, Object))
                                logApp.CollectDetails("Angefordert", CType(lblAngefordertAm.Text, Object))
                                logApp.CollectDetails("Fahrgestellnummer", CType(lblFahrgestellnummer.Text, Object))
                                logApp.CollectDetails("Briefnummer", CType(lblBriefnummer.Text, Object))
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
            If ex.Message = "Keine Applikationsdaten für den Logeintrag vorhanden." Then
                lblError.Text = "Vorgang konnte nicht autorisiert werden, da bereits storniert oder fertig bearbeitet!"
            Else : lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End If

        End Try
    End Sub

    Private Sub DoSubmit3()
        Try
            objFDDBank2.SessionID = Session.SessionID.ToString
            objFDDBank2.Change(Session("AppID").ToString, Session.SessionID, Me)
            If objFDDBank2.Status = 0 Then
                WriteLog("Brieffreigabe für Händler " & objSuche.REFERENZ & " erfolgreich durchgeführt.")
                logApp.WriteStandardDataAccessSAP(objFDDBank2.IDSAP)
                DeleteAuthorizationEntry(m_App.Connectionstring, CInt(Session("AuthorizationID")))
                Session("Authorization") = Nothing
                Session("AuthorizationID") = Nothing
                cmdAuthorize.Visible = False
                cmdDelete.Visible = False
                cmdBack.Visible = False
                lblInformation.Text = "<b>Ihre Daten wurden gespeichert.</b><br>&nbsp;"
                ConfirmMessage.Visible = True
                'zurueck zur Liste oder Hauptmenue
                Dim strLastRecord As String = CStr(Request.QueryString("LastRecord"))
                If Not strLastRecord = "True" Then
                    Response.Redirect("Change14.aspx?AppID=" & CStr(m_User.Applications.Select("AppName='Change14'")(0).Item("AppID")) & "&Aut=@!", False)
                Else
                    Response.Redirect("../../../Start/Selection.aspx", False)
                End If
            Else
                WriteLog("Fehler bei der Brieffreigabe für Händler " & objSuche.REFERENZ & " , (Fehler: " & objFDDBank2.Message & ")", "ERR")
                logApp.WriteStandardDataAccessSAP(objFDDBank2.IDSAP)
                lblError.Text = "Beim Speichern Ihrer Daten ist ein Fehler aufgetreten.<br>(" & objFDDBank2.Message & ")"
                ConfirmMessage.Visible = False
            End If

        Catch ex As Exception
            WriteLog("Fehler bei der Brieffreigabe für Händler " & objSuche.REFERENZ & " , (Fehler: " & ex.Message & ")", "ERR")
            lblError.Text = "Beim Speichern Ihrer Daten ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            ConfirmMessage.Visible = False
        End Try
    End Sub

    Private Sub DoSubmit4()
        Try
            DeleteAuthorizationEntry(m_App.Connectionstring, CInt(Session("AuthorizationID")))
            WriteLog("Brieffreigabe für Händler " & objSuche.REFERENZ & " aus Autorisierung gelöscht.")
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
            WriteLog("Fehler bei der Löschung einer Brieffreigabe für Händler " & objSuche.REFERENZ & " , (Fehler: " & ex.Message & ")", "ERR")
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
' *****************  Version 4  *****************
' User: Fassbenders  Date: 23.03.10   Time: 16:34
' Updated in $/CKAG/Applications/appffd/Forms
' Rückgängig: Dynproxy-Zugriff
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 11.03.10   Time: 12:36
' Updated in $/CKAG/Applications/appffd/Forms
' ITA: 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 15.06.09   Time: 17:08
' Updated in $/CKAG/Applications/appffd/Forms
' ITA 2918
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:23
' Created in $/CKAG/Applications/appffd/Forms
' 
' *****************  Version 11  *****************
' User: Uha          Date: 2.07.07    Time: 12:52
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 26.06.07   Time: 15:17
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' Betrag hinzugefügt!
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 26.06.07   Time: 14:32
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' 
' *****************  Version 8  *****************
' User: Fassbenders  Date: 8.06.07    Time: 11:26
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' 
' *****************  Version 7  *****************
' User: Uha          Date: 3.05.07    Time: 18:05
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' Änderungen aus StartApplication vom 02.05.2007 Mittags übernommen
' 
' ************************************************
