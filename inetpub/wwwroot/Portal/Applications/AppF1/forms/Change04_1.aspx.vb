Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Business
Imports CKG
Imports System
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary

Partial Public Class Change04_1
    Inherits System.Web.UI.Page

    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private objSuche As Search
    Private mObjKontingentVerwaltung As KontingentVerwaltung
    Private m_strInitiator As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        cmdSave.Enabled = False
        cmdConfirm.Enabled = False
        cmdReset.Enabled = False
        lblPageTitle.Text = "Werte ändern"

        lblInformation.Text = ""
        m_strInitiator = ""

        Try
            m_User = GetUser(Me)
            ucHeader.InitUser(m_User)
            FormAuth(Me, m_User)

            m_App = New Base.Kernel.Security.App(m_User)

            If (Session("objSuche") Is Nothing) Then
                If (Not Session("Authorization") Is Nothing) AndAlso CBool(Session("Authorization")) AndAlso _
                    (Not Session("AuthorizationID") Is Nothing) AndAlso IsNumeric(Session("AuthorizationID")) Then
                    Dim OutPutStream As System.IO.MemoryStream = GiveAuthorizationDetails(m_App.Connectionstring, CInt(Session("AuthorizationID")), "objSuche")
                    If OutPutStream Is Nothing Then
                        lblError.Text = "Keine Daten für den Vorgang vorhanden."
                    Else
                        Dim formatter As New BinaryFormatter()
                        objSuche = New Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
                        objSuche = DirectCast(formatter.Deserialize(OutPutStream), Search)
                        Session.Add("objSuche", objSuche)
                        ' objSuche.ReNewSAPDestination(Session.SessionID.ToString, Session("AppID").ToString)
                    End If
                Else
                    Try
                    Catch
                        Response.Redirect("Change04.aspx?AppID=" & Session("AppID").ToString)
                    End Try
                End If
            Else
                objSuche = CType(Session("objSuche"), Search)
            End If

            ' If objSuche.LeseHaendlerSAP_Einzeln(Session("AppID").ToString, Session.SessionID.ToString,objSuche.REFERENZ) Then
            lblHaendlerNummer.Text = objSuche.REFERENZ
            lblHaendlerName.Text = objSuche.NAME

            If objSuche.NAME_2.Length > 0 Then
                lblHaendlerName.Text &= "<br>" & objSuche.NAME_2
            End If

            lblAdresse.Text = objSuche.COUNTRYISO & " - " & objSuche.POSTL_CODE & " " & objSuche.CITY & "<br>" & objSuche.STREET


            lnkKreditlimit.NavigateUrl = "Change04.aspx?AppID=" & Session("AppID").ToString & "&ID=" & Request.QueryString("ID") & "&Back=1"

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text

            If Not IsPostBack Then
                ConfirmMessage.Visible = False

                '#################### Debug only ####################
                'Session("Authorization") = True
                'Session("AuthorizationID") = 6
                '#################### Debug only ####################
            End If

            If (Not Session("Authorization") Is Nothing) AndAlso (CBool(Session("Authorization"))) Then
                'Seite wurde mit dem Merkmal "Autorisieren" aufgerufen

                If (Session("AuthorizationID") Is Nothing) OrElse Session("AuthorizationID").ToString.Length = 0 Then
                    'AuthorizationID leer
                    lblError.Text = "Kein Vorgang zum Autorisieren übergeben."
                    DataGrid1.Visible = False
                Else
                    'AuthorizationID gefüllt -> Vorgang wird aus DB geladen
                    Dim OutPutStream As MemoryStream = GiveAuthorizationDetails(m_App.Connectionstring, CInt(Session("AuthorizationID")),"mObjKontingentVerwaltung")
                    If OutPutStream Is Nothing Then
                        lblError.Text = "Keine Daten für den Vorgang vorhanden."
                    Else
                        Dim formatter As New BinaryFormatter()
                        mObjKontingentVerwaltung = New KontingentVerwaltung(m_App, m_User, Session.SessionID.ToString, "")
                        mObjKontingentVerwaltung = DirectCast(formatter.Deserialize(OutPutStream), KontingentVerwaltung)
                        FillGrid()
                        DoSubmit1()

                        cmdSave.Visible = False
                        cmdConfirm.Visible = False
                        cmdReset.Visible = False
                        cmdBack.Visible = True
                        cmdAuthorize.Visible = True
                        cmdDelete.Visible = True
                    End If
                End If
            Else
                Dim intAuthorizationID As Int32

                m_App.CheckForPendingAuthorization(CInt(Session("AppID")), m_User.Organization.OrganizationId, objSuche.REFERENZ, "",
                                                   m_User.IsTestUser, m_strInitiator, intAuthorizationID)
                If Not m_strInitiator.Length = 0 Then
                    'Seite gesperrt aufgerufen, da Händlerdaten in Autorisierung

                    LoadAuthorizatioData(intAuthorizationID)
                    lblError.Text = "Die Angaben zum Händler " & objSuche.REFERENZ & " wurden vom Benutzer """ & m_strInitiator &
                        """ geändert.<br>&nbsp;&nbsp;Die Autorisierung steht noch aus!"
                Else
                    'Seite im normalen Änderungsmodus aufgerufen

                    If Not IsPostBack Then
                        mObjKontingentVerwaltung = New KontingentVerwaltung(m_App, m_User, Session.SessionID.ToString, "")
                        mObjKontingentVerwaltung.Customer = objSuche.REFERENZ
                        mObjKontingentVerwaltung.KUNNR = m_User.KUNNR
                        mObjKontingentVerwaltung.CreditControlArea = "ZDAD"
                        mObjKontingentVerwaltung.fillKontingent(Session("AppID").ToString, Session.SessionID, objSuche.REFERENZ)
                    Else
                        mObjKontingentVerwaltung = CType(Session("mObjKontingentVerwaltungSession"), KontingentVerwaltung)
                    End If

                    If mObjKontingentVerwaltung.Status = 0 Then
                        If Not IsPostBack Then
                            StartLoadData()
                        End If

                        cmdSave.Enabled = True
                        cmdConfirm.Enabled = True
                        cmdReset.Enabled = True
                        cmdAuthorize.Enabled = False
                        cmdBack.Enabled = False
                        cmdDelete.Enabled = False
                    Else
                        lblError.Text = mObjKontingentVerwaltung.Message

                    End If
                End If
            End If
            If Not IsPostBack Then
                Session.Add("mObjKontingentVerwaltungSession", mObjKontingentVerwaltung)
            End If

        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "Change04", "Page_Load", ex.ToString)

            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub FillGrid()
        DataGrid1.DataSource = mObjKontingentVerwaltung.Kontingente
        DataGrid1.DataBind()

        If mObjKontingentVerwaltung.GruppenHaendler.Rows.Count = 0 Then

            lblInfoKontingentart.Text = "<b>Händlerkontingent</b>"
            tblGruppenHaendler.Visible = False
        Else
            lblInfoKontingentart.Text = "<b>Gruppenkontingent</b>, der o.g. Händler gehört zu einer Gruppe, eine Kontingentändertung betrifft alle Händler in dieser Gruppe"

            DTGHaendler.DataSource = mObjKontingentVerwaltung.GruppenHaendler
            DTGHaendler.DataBind()
        End If

    End Sub

    Private Sub StartLoadData()

        cmdSave.Visible = True
        cmdConfirm.Visible = False
        cmdReset.Visible = False
        cmdAuthorize.Visible = False
        cmdBack.Visible = False
        cmdDelete.Visible = False

        If (mObjKontingentVerwaltung.Kontingente Is Nothing) OrElse (mObjKontingentVerwaltung.Kontingente.Rows.Count = 0) Then
            lblError.Text = "Fehler: Es konnten keine Kontingentdaten ermittelt werden."

            lblError.CssClass = "TextError"
        Else
            lblError.CssClass = "LabelExtraLarge"
            FillGrid()
        End If
    End Sub

    Private Sub DoSubmit1()
        Dim strChangeMessage As String = ""

        Dim hasChanged As Boolean = False
        If mObjKontingentVerwaltung Is Nothing Then
            mObjKontingentVerwaltung = CType(Session("mObjKontingentVerwaltungSession"), KontingentVerwaltung)
        End If
        Dim tmprow As DataRow
        For Each tmpItem As DataGridItem In DataGrid1.Items
            If Not mObjKontingentVerwaltung.Kontingente.Select("KKBER='" & tmpItem.Cells(0).Text & "'").Count = 1 Then
                Throw New Exception("es wurde kein eindeutiger Kreditkontrolbereich gefunden")
            Else
                tmprow = mObjKontingentVerwaltung.Kontingente.Select("KKBER='" & tmpItem.Cells(0).Text & "'")(0)
            End If


            Dim tmpString As String
            tmpString = CType(tmpItem.FindControl("txtKontingent_Neu"), TextBox).Text
            If IsNumeric(tmpString) AndAlso (tmpString.Length < 5) AndAlso (Not CInt(tmpString) < 0) Then
                tmprow("EingabeKontingent") = tmpString
            Else
                strChangeMessage &= "Bitte geben Sie numerische, positive und max. vierstellige Kontigentwerte ein.<br>"
            End If

            If CType(tmpItem.FindControl("chkGesperrt_Neu"), CheckBox).Checked Then
                tmprow("EingabeGesperrt") = "X"
            Else
                tmprow("EingabeGesperrt") = ""
            End If



            If Not tmprow("EingabeGesperrt").ToString = tmprow("CRBLB").ToString OrElse Not CInt(tmprow("EingabeKontingent")) = CInt(tmprow("KLIMK")) Then

                hasChanged = True

                If strChangeMessage.Length = 0 Then
                    CType(tmpItem.FindControl("chkGesperrt_Neu"), CheckBox).Enabled = False
                    CType(tmpItem.FindControl("txtKontingent_Neu"), TextBox).Enabled = False
                End If
            End If
        Next
        mObjKontingentVerwaltung.Kontingente.AcceptChanges()

        If Not hasChanged And strChangeMessage.Length = 0 Then
            strChangeMessage = "sie haben keine Änderung vorgenommen"
        End If

        If strChangeMessage.Length = 0 Then
            cmdSave.Visible = False
            cmdConfirm.Visible = True
            cmdReset.Visible = True
            cmdAuthorize.Visible = False
            cmdBack.Visible = False
            cmdDelete.Visible = False
        End If

        lblInformation.Text = strChangeMessage
        lblError.Text = strChangeMessage

    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        DoSubmit1()
    End Sub

    Private Sub DoSubmit2()

        If mObjKontingentVerwaltung Is Nothing Then
            mObjKontingentVerwaltung = CType(Session("mObjKontingentVerwaltungSession"), KontingentVerwaltung)
        End If

        If m_strInitiator.Length = 0 Then
            Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)

            Try

                Dim tblLogDetails As DataTable = GetChanges()

                If (CInt(m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AuthorizationLevel")) > 0) Then
                    'Anwendung erfordert Autorisierung (Level>0)

                    Dim DetailArray(2, 2) As Object
                    Dim ms As MemoryStream
                    Dim formatter As BinaryFormatter
                    Dim b() As Byte

                    ms = New MemoryStream()
                    formatter = New BinaryFormatter()
                    formatter.Serialize(ms, mObjKontingentVerwaltung)
                    b = ms.ToArray
                    ms = New MemoryStream(b)
                    DetailArray(0, 0) = ms
                    DetailArray(0, 1) = "mObjKontingentVerwaltung"


                    ms = New MemoryStream()
                    formatter = New BinaryFormatter()
                    formatter.Serialize(ms, objSuche)
                    b = ms.ToArray
                    ms = New MemoryStream(b)
                    DetailArray(1, 0) = ms
                    DetailArray(1, 1) = "objSuche"

                    Dim intAuthorizationID As Int32 = WriteAuthorization(m_App.Connectionstring, CInt(Session("AppID")), m_User.UserName,
                                                                         CInt(m_User.Organization.OrganizationId), objSuche.REFERENZ, "", "", "",
                                                                         m_User.IsTestUser, DetailArray)

                    LoadAuthorizatioData(intAuthorizationID)
                    ConfirmMessage.Visible = True
                    lblInformation.Text = "<b>Ihre Daten wurden zur Autorisierung gespeichert.</b><br>&nbsp;"

                Else
                    'Anwendung erfordert keine Autorisierung (Level=0)

                    mObjKontingentVerwaltung.writeKontingente(Session("AppID").ToString, Session.SessionID)
                    If mObjKontingentVerwaltung.Status = 0 Then

                        lblInformation.Text = "<b>Ihre Daten wurden gespeichert.</b><br>&nbsp;"
                        ConfirmMessage.Visible = True

                    Else
                        ' tblLogDetails zum Log hinzugefügt -- CDI 10.05.12
                        logApp.WriteEntry("ERR", m_User.UserName, Session.SessionID, CInt(Session("AppID")),
                                          m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString,
                                          objSuche.REFERENZ, "Fehler bei der Kontingentänderung von Händler " &
                                          objSuche.REFERENZ & " (DAD: " & objSuche.REFERENZ & ", Fehler: " & mObjKontingentVerwaltung.Message & ")",
                                          m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 10, tblLogDetails)

                        lblError.Text = "Beim Speichern Ihrer Daten ist ein Fehler aufgetreten.<br>(" & mObjKontingentVerwaltung.Message & ")"
                        lblError.CssClass = "TextError"
                        ConfirmMessage.Visible = False

                    End If

                    StartLoadData()
                End If

            Catch ex As Exception
                m_App.WriteErrorText(1, m_User.UserName, "Change04_1", "DoSubmit2", ex.ToString)

                ' tblLogDetails zum Log hinzugefügt -- CDI 10.05.12
                logApp.WriteEntry("ERR", m_User.UserName, Session.SessionID, CInt(Session("AppID")),
                                  m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString,
                                  objSuche.REFERENZ, "Fehler bei der Kontingentänderung von Händler " & objSuche.REFERENZ &
                                  " (DAD: " & objSuche.REFERENZ & ", Fehler: " & ex.Message & ")", m_User.CustomerName, m_User.Customer.CustomerId,
                                  m_User.IsTestUser, 10)

                lblError.Text = "Beim Speichern Ihrer Daten ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                lblError.CssClass = "TextError"
                ConfirmMessage.Visible = False
            End Try

            Session.Add("mObjKontingentVerwaltungSession", mObjKontingentVerwaltung)
        End If
    End Sub

    Private Sub DoSubmit3()
        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        Dim blnError As Boolean = False
        Try

            'Dim tblLogDetails As DataTable = GetChanges()

            mObjKontingentVerwaltung.SessionID = Session.SessionID.ToString
            mObjKontingentVerwaltung.writeKontingente(Session("AppID").ToString, Session.SessionID)
            If mObjKontingentVerwaltung.Status = 0 Then

                logApp.WriteStandardDataAccessSAP(mObjKontingentVerwaltung.IDSAP)
                lblInformation.Text = "<b>Ihre Daten wurden gespeichert.</b><br>&nbsp;"
                DeleteAuthorizationEntry(m_App.Connectionstring, CInt(Session("AuthorizationID")))

                HelpProcedures.fakeLogEntryForAutorisierung(logApp, m_User, Session.SessionID, Session("AppID").ToString,
                                                            "Autorisierung für  Änderung Kontingent von Händler " & objSuche.REFERENZ &
                                                            " (DAD: " & objSuche.REFERENZ & ") erfolgreich durchgeführt.")

                Session("Authorization") = Nothing
                Session("AuthorizationID") = Nothing
                Session("objSuche") = objSuche
                ConfirmMessage.Visible = True

                cmdSave.Enabled = True
                mObjKontingentVerwaltung.fillKontingent(Session("AppID").ToString, Session.SessionID, objSuche.REFERENZ)
                StartLoadData()
                Session.Add("mObjKontingentVerwaltungSession", mObjKontingentVerwaltung)
                If mObjKontingentVerwaltung.Status = 0 Then
                    'zurueck zur Liste oder Hauptmenue
                    Dim strLastRecord As String = CStr(Request.QueryString("LastRecord"))
                    Try
                        If Not strLastRecord = "True" Then
                            Response.Redirect("Change05.aspx?AppID=" & CStr(m_User.Applications.Select("AppName='Change05'")(0).Item("AppID")), False)
                        Else
                            Response.Redirect("../../../Start/Selection.aspx", False)
                        End If
                    Catch
                    End Try
                End If
            Else
                Session.Add("mObjKontingentVerwaltungSession", mObjKontingentVerwaltung)
                HelpProcedures.fakeLogEntryForAutorisierung(logApp, m_User, Session.SessionID, Session("AppID").ToString,
                                                            "Fehler bei Autorisierung der Kontingentänderung von Händler " & objSuche.REFERENZ &
                                                            " (DAD: " & objSuche.REFERENZ & ", Fehler: " & mObjKontingentVerwaltung.Message)

                lblError.Text = "Beim Speichern Ihrer Daten ist ein Fehler aufgetreten.<br>(" & mObjKontingentVerwaltung.Message & ")"
                lblError.CssClass = "TextError"
                ConfirmMessage.Visible = False

                blnError = True
            End If

        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "Change01Edit", "DoSubmit3", ex.ToString)

            Session.Add("mObjKontingentVerwaltungSession", mObjKontingentVerwaltung)
            HelpProcedures.fakeLogEntryForAutorisierung(logApp, m_User, Session.SessionID, Session("AppID").ToString,
                                                        "Fehler bei Autorisierung der Kontingentänderung von Händler " & objSuche.REFERENZ &
                                                        " (DAD: " & objSuche.REFERENZ & ", Fehler: " & ex.Message)

            lblError.Text = "Beim Speichern Ihrer Daten ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            lblError.CssClass = "TextError"
            ConfirmMessage.Visible = False

            blnError = True
        End Try
    End Sub

    Private Sub DoSubmit4()
        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)

        Try

            'Dim tblLogDetails As DataTable = GetChanges()

            DeleteAuthorizationEntry(m_App.Connectionstring, CInt(Session("AuthorizationID")))

            HelpProcedures.fakeLogEntryForAutorisierung(logApp, m_User, Session.SessionID, Session("AppID").ToString,
                                                        "Autorisierung für Löschung der Kontingentänderung für Händler " & objSuche.REFERENZ &
                                                        " (DAD: " & objSuche.REFERENZ & ") erfolgreich durchgeführt")
            lblInformation.Text = "<b>Daten wurden aus Autorisierung entfernt.</b><br>&nbsp;"
            Session("Authorization") = Nothing
            Session("AuthorizationID") = Nothing
            Session("objSuche") = objSuche
            ConfirmMessage.Visible = True

            cmdSave.Enabled = True

            'zurueck zur Liste oder Hauptmenue
            Dim strLastRecord As String = CStr(Request.QueryString("LastRecord"))
            Try
                If Not strLastRecord = "True" Then
                    Response.Redirect("Change05.aspx?AppID=" & CStr(m_User.Applications.Select("AppName='Change05'")(0).Item("AppID")), False)
                Else
                    Response.Redirect("../../../Start/Selection.aspx", False)
                End If
            Catch
            End Try
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "Change04_1", "DoSubmit4", ex.ToString)

            logApp.WriteEntry("ERR", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, objSuche.REFERENZ, "Fehler bei der Kontingentänderung von Händler " & objSuche.REFERENZ & " (DAD: " & objSuche.REFERENZ & ", Fehler: " & ex.Message & ")", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 10)
            lblError.Text = "Beim Speichern Ihrer Daten ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            ConfirmMessage.Visible = False

        End Try

        StartLoadData()
        Session.Add("mObjKontingentVerwaltungSession", mObjKontingentVerwaltung)
    End Sub

    Private Sub cmdConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdConfirm.Click
        DoSubmit2()
    End Sub

    Private Sub cmdReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReset.Click
        StartLoadData()
        Session.Add("mObjKontingentVerwaltungSession", mObjKontingentVerwaltung)
    End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        DoSubmit4()
    End Sub

    Private Sub cmdAuthorize_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAuthorize.Click
        DoSubmit3()
    End Sub

    Private Function GetChanges() As DataTable
        Dim m_tblKontingenteChanged As DataTable
        m_tblKontingenteChanged = New DataTable()

        m_tblKontingenteChanged.Columns.Add("Händler", System.Type.GetType("System.String"))
        m_tblKontingenteChanged.Columns.Add("Kontingentart", System.Type.GetType("System.String"))
        m_tblKontingenteChanged.Columns.Add("Altes Kontingent", System.Type.GetType("System.Int32"))
        m_tblKontingenteChanged.Columns.Add("Neues Kontingent", System.Type.GetType("System.Int32"))
        m_tblKontingenteChanged.Columns.Add("Gesperrt Neu", System.Type.GetType("System.String"))
        m_tblKontingenteChanged.Columns.Add("Gesperrt Alt", System.Type.GetType("System.String"))


        Dim rowTemp As DataRow
        For Each rowTemp In mObjKontingentVerwaltung.Kontingente.Rows
            Dim tmpRow2 As DataRow
            If Not rowTemp("EingabeGesperrt").ToString = rowTemp("CRBLB").ToString OrElse Not CInt(rowTemp("EingabeKontingent")) = CInt(rowTemp("KLIMK")) Then

                tmpRow2 = m_tblKontingenteChanged.NewRow
                tmpRow2("Händler") = objSuche.REFERENZ
                tmpRow2("Altes Kontingent") = rowTemp("KLIMK")
                tmpRow2("Kontingentart") = rowTemp("Kontingentart")
                tmpRow2("Neues Kontingent") = rowTemp("EingabeKontingent")
                tmpRow2("Gesperrt Neu") = rowTemp("EingabeGesperrt")
                tmpRow2("Gesperrt Alt") = rowTemp("CRBLB")

                m_tblKontingenteChanged.Rows.Add(tmpRow2)
            End If
        Next
        Return m_tblKontingenteChanged
    End Function

    Private Sub LoadAuthorizatioData(ByVal AuthorizationID As Int32)
        Dim OutPutStream As System.IO.MemoryStream = GiveAuthorizationDetails(m_App.Connectionstring, AuthorizationID, "mObjKontingentVerwaltung")
        Dim formatter As New BinaryFormatter()
        mObjKontingentVerwaltung = New KontingentVerwaltung(m_App, m_User, Session.SessionID.ToString, "")
        mObjKontingentVerwaltung = DirectCast(formatter.Deserialize(OutPutStream), KontingentVerwaltung)
        FillGrid()
        DoSubmit1()


        cmdSave.Visible = False
        cmdConfirm.Visible = False
        cmdReset.Visible = False
        cmdAuthorize.Visible = False
        cmdBack.Visible = False
        cmdDelete.Visible = False
    End Sub

    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBack.Click
        Try
            Response.Redirect("Change14.aspx?AppID=" & CStr(m_User.Applications.Select("AppName='Change14'")(0).Item("AppID")) & "&Aut=@!", False)
        Catch
        End Try
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

End Class
' ************************************************
' $History: Change04_1.aspx.vb $
' 
' *****************  Version 11  *****************
' User: Rudolpho     Date: 30.04.09   Time: 11:39
' Updated in $/CKAG/Applications/AppF1/forms
' ITA 2837
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 3.04.09    Time: 9:05
' Updated in $/CKAG/Applications/AppF1/forms
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 23.03.09   Time: 13:33
' Updated in $/CKAG/Applications/AppF1/forms
' Autorisierung
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 17.03.09   Time: 12:32
' Updated in $/CKAG/Applications/AppF1/forms
' 2687 autorisierungsanpassung
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 17.03.09   Time: 8:56
' Updated in $/CKAG/Applications/AppF1/forms
' kleine nderungen
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 12.03.09   Time: 9:10
' Updated in $/CKAG/Applications/AppF1/forms
' ITA 2678,2677 testfertig
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 11.03.09   Time: 17:01
' Updated in $/CKAG/Applications/AppF1/forms
' ITa 2677 unfertig
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 11.03.09   Time: 15:49
' Updated in $/CKAG/Applications/AppF1/forms
' ITa 2678 unfertig
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 11.03.09   Time: 11:34
' Updated in $/CKAG/Applications/AppF1/forms
' unfertig
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 11.03.09   Time: 11:26
' Updated in $/CKAG/Applications/AppF1/forms
' ITA 2677 unfertig
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 11.03.09   Time: 10:28
' Created in $/CKAG/Applications/AppF1/forms
' ITA 2677&2678
' ************************************************
