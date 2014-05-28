Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business

Imports System
Imports System.IO
Imports System.Collections
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization
Imports System.Text.RegularExpressions

Partial Public Class Change17Edit
    Inherits System.Web.UI.Page

    Private m_context As HttpContext = HttpContext.Current
    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private objSuche As FFE_Search
    Private objChange17_objFDDZahlungsfrist As FFE_Zahlungsfrist
    Private objFDDBank As FFE_BankBase
    Private m_strInitiator As String
    Dim m_tblKontingenteChanged As DataTable

#Region "Events"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cmdSave.Enabled = False
        lblPageTitle.Text = "Werte ändern"

        lblInformation.Text = ""
        m_strInitiator = ""

        Try
            m_User = GetUser(Me)
            ucHeader.InitUser(m_User)
            FormAuth(Me, m_User)

            m_App = New Base.Kernel.Security.App(m_User)

            If Session("SelectedDealer") = Nothing OrElse (Session("objSuche") Is Nothing) Then
                If (Not Session("Authorization") Is Nothing) AndAlso CBool(Session("Authorization")) AndAlso _
                    (Not Session("AuthorizationID") Is Nothing) AndAlso IsNumeric(Session("AuthorizationID")) Then
                    Dim OutPutStream As System.IO.MemoryStream = GiveAuthorizationDetails(m_App.Connectionstring, CInt(Session("AuthorizationID")), "objSuche")
                    If OutPutStream Is Nothing Then
                        lblError.Text = "Keine Daten für den Vorgang vorhanden."
                    Else
                        Dim formatter As New BinaryFormatter()
                        objSuche = New FFE_Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
                        objSuche = DirectCast(formatter.Deserialize(OutPutStream), FFE_Search)
                        objSuche.ReNewSAPDestination(Session.SessionID.ToString, Session("AppID").ToString)
                    End If
                    trPflege.Visible = False
                    trPflegelabel.Visible = False
                Else
                    Try
                    Catch
                        Response.Redirect("Change17.aspx?AppID=" & Session("AppID").ToString)
                    End Try
                    objSuche = CType(Session("objSuche"), FFE_Search)
                End If
            Else
                objSuche = CType(Session("objSuche"), FFE_Search)
            End If

            If objSuche.LeseHaendlerSAP_Einzeln(Session("AppID").ToString, Session.SessionID.ToString, Session("SelectedDealer").ToString) Then
                lblHaendlerNummer.Text = objSuche.REFERENZ
                lblHaendlerName.Text = objSuche.NAME
                If objSuche.NAME_2.Length > 0 Then
                    lblHaendlerName.Text &= "<br>" & objSuche.NAME_2
                End If
                lblAdresse.Text = objSuche.COUNTRYISO & " - " & objSuche.POSTL_CODE & " " & objSuche.CITY & "<br>" & objSuche.STREET
            Else
                lblError.Text = "Fehler bei der Ermittlung der Händlerdaten.<br>(" & objSuche.ErrorMessage & ")"
            End If
            lnkHaendlerSuche.NavigateUrl = "Change17.aspx?AppID=" & Session("AppID").ToString & "&Back=1" & "&ID=" & Request.QueryString("ID")

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
                    Dim OutPutStream As System.IO.MemoryStream = GiveAuthorizationDetails(m_App.Connectionstring, CInt(Session("AuthorizationID")), "objFDDZahlungsfrist")
                    If OutPutStream Is Nothing Then
                        lblError.Text = "Keine Daten für den Vorgang vorhanden."
                    Else
                        'Dim formatter As New BinaryFormatter()
                        'objChange17_objFDDZahlungsfrist = New FFE_Zahlungsfrist(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                        'objChange17_objFDDZahlungsfrist = DirectCast(formatter.Deserialize(OutPutStream), FFE_Zahlungsfrist)
                        'FillGrid()

                        FocusScript.Visible = False

                        'cmdSave.Visible = False
                        Dim intAuthorizationID As Int32 = Session("AuthorizationID")
                        cmdAuthorize.Visible = True
                        cmdDelete.Visible = True
                        cmdBack.Visible = True
                        LoadAuthorizatioData(intAuthorizationID)

                    End If
                End If
            Else
                Dim intAuthorizationID As Int32
                Dim iDistrictID As Int16
                If Not Session("SelectedDistrict") = Nothing Then
                    iDistrictID = Session("SelectedDistrict")
                Else
                    iDistrictID = m_User.Organization.OrganizationReference
                End If
                m_App.CheckForPendingAuthorization(CInt(Session("AppID")), iDistrictID, Session("SelectedDealer").ToString, "", m_User.IsTestUser, m_strInitiator, intAuthorizationID)
                If Not m_strInitiator.Length = 0 Then
                    'Seite gesperrt aufgerufen, da Händlerdaten in Autorisierung

                    If Not IsPostBack Then
                        LoadAuthorizatioData(intAuthorizationID)
                        lblInformation.Text = "Die Angaben zum Händler " & Session("SelectedDealer").ToString & " wurden vom Benutzer """ & m_strInitiator & """ geändert.<br>&nbsp;&nbsp;Die Autorisierung steht noch aus!"
                    End If
                Else
                    'Seite im normalen Änderungsmodus aufgerufen

                    If Not IsPostBack Then
                        objChange17_objFDDZahlungsfrist = New FFE_Zahlungsfrist(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                        objChange17_objFDDZahlungsfrist.Customer = "60" & Session("SelectedDealer").ToString
                        objChange17_objFDDZahlungsfrist.KUNNR = m_User.KUNNR
                        objChange17_objFDDZahlungsfrist.CreditControlArea = "ZDAD"
                        If (Not Request.QueryString("ID") Is Nothing) AndAlso (Request.QueryString("ID").Length > 0) Then
                            objChange17_objFDDZahlungsfrist.ZeigeGesperrt = True
                        Else
                            objChange17_objFDDZahlungsfrist.ZeigeGesperrt = False
                        End If
                    Else
                        objChange17_objFDDZahlungsfrist = CType(m_context.Cache("objChange17_objFDDZahlungsfrist"), FFE_Zahlungsfrist)
                        objFDDBank = Session("objFDDBank")
                    End If
                    objChange17_objFDDZahlungsfrist.Show()

                    If objChange17_objFDDZahlungsfrist.Status = 0 Then

                        objChange17_objFDDZahlungsfrist.ShowKontingentDetails()

                        If objChange17_objFDDZahlungsfrist.Status = 0 Then

                            If Not IsPostBack Then
                                StartLoadData()
                            End If
                            Session("objChange17_objFDDZahlungsfrist") = objChange17_objFDDZahlungsfrist
                            checkkontingente()
                            cmdSave.Enabled = True
                        Else
                            lblError.Text = objChange17_objFDDZahlungsfrist.Message
                            trPflegelabel.Visible = False
                            trPflege.Visible = False
                            FocusScript.Visible = False
                        End If

                    Else
                        lblError.Text = objChange17_objFDDZahlungsfrist.Message
                        trPflegelabel.Visible = False
                        trPflege.Visible = False
                        FocusScript.Visible = False
                    End If





                End If
            End If

            If Not IsPostBack Then
                m_context.Cache.Insert("objChange17_objFDDZahlungsfrist", objChange17_objFDDZahlungsfrist, New System.Web.Caching.CacheDependency(Server.MapPath("Change17Edit.aspx")), DateTime.Now.AddMinutes(20), TimeSpan.Zero)
            End If
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "Change17Edit", "Page_Load", ex.ToString)

            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Try
            Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)

            If objChange17_objFDDZahlungsfrist Is Nothing Then
                objChange17_objFDDZahlungsfrist = Session("objChange17_objFDDZahlungsfrist")
            End If


            'werte von details fristen sichern
            For Each item2 As DataGridItem In DataGrid1.Items
                If Not item2.FindControl("DataGridLvL2") Is Nothing Then
                    Dim tmpDataGrid As DataGrid
                    tmpDataGrid = CType(item2.FindControl("DataGridLvL2"), DataGrid)
                    For Each item3 As DataGridItem In tmpDataGrid.Items
                        Dim tmpRows() As DataRow
                        tmpRows = objChange17_objFDDZahlungsfrist.FaelligkeitenAnzeigeTable.Select("KKBER='" & item2.Cells(0).Text & "' AND ZZVGRUND='" & item3.Cells(0).Text & "'")
                        If Not tmpRows.Length = 1 Then
                            Throw New Exception("AbrufGrundDetails konnte nicht eindeutig ermittelt und gespeichert werden!")
                            Exit Sub
                        Else

                            If Not CType(item3.FindControl("txtNeueZahlungsfristProAbrufgrund"), TextBox).Text.Trim(" "c) = "" Then 'wenn keine Eintragung dann gar nichts machen bitte
                                tmpRows(0)("NeueZFProAG") = CType(item3.FindControl("txtNeueZahlungsfristProAbrufgrund"), TextBox).Text.Trim(" "c)

                                CType(item3.FindControl("txtNeueZahlungsfristProAbrufgrund"), TextBox).Enabled = False

                                'allen Kontingentarten in denen ein Detail Wert geändert wurde, muss im TextBoxField "Neue Fälligkeit in Tagen" etwas eingetragen sein!
                                'dies muss wegen dem aufbau des ur-reports so gemacht werden, JJU2008.09.16
                                If CType(item2.FindControl("txtZahlungsfristNeu"), TextBox).Text.Trim(" "c) = "" Then
                                    CType(item2.FindControl("txtZahlungsfristNeu"), TextBox).Text = Right(item2.Cells(2).Text, 2) 'cell2 = alte Zahlungsfrist, muss fortmatiert weil ur-textbox nur eine Eintragung von 2 Stellen zulässt. JJU2008.09.16
                                End If
                                objChange17_objFDDZahlungsfrist.FaelligkeitenAnzeigeTable.AcceptChanges()
                            End If
                        End If
                    Next
                End If
            Next


            Dim item As DataGridItem
            For Each item In DataGrid1.Items


                'Werte ermitteln

                Dim cell As TableCell
                cell = item.Cells(0)
                Dim strAuftragsnummer As String
                strAuftragsnummer = cell.Text

                For Each cell In item.Cells

                    Dim c As System.Web.UI.Control
                    For Each c In cell.Controls

                        If TypeOf c Is TextBox Then

                            Dim t As TextBox = CType(c, TextBox)

                            'Neue Zahlungsfrist auslesen und in DataRow schreiben
                            If t.ID = "txtZahlungsfristNeu" AndAlso t.Enabled = True Then
                                If Regex.IsMatch(t.Text.Trim, "^\d{0,2}$", RegexOptions.Singleline) AndAlso t.Text.Length <> 0 Then
                                    objChange17_objFDDZahlungsfrist = New FFE_Zahlungsfrist(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                                    objChange17_objFDDZahlungsfrist = Session("objChange17_objFDDZahlungsfrist")
                                    Dim sKontingentart As String
                                    Dim cell2 As TableCell
                                    cell2 = item.Cells(1)
                                    sKontingentart = cell2.Text

                                    Dim sKKber As String

                                    cell2 = item.Cells(0)
                                    sKKber = cell2.Text
                                    If sKKber = "0001" Then
                                        objChange17_objFDDZahlungsfrist.Zahlungsfristen.Rows(0)(FFE_Zahlungsfrist.COLUMN_KONTINGENTART) = sKontingentart
                                        objChange17_objFDDZahlungsfrist.Zahlungsfristen.Rows(0)(FFE_Zahlungsfrist.COLUMN_KONTINGENTID) = sKKber
                                    End If
                                    If sKKber = "0002" Then
                                        objChange17_objFDDZahlungsfrist.Zahlungsfristen.Rows(1)(FFE_Zahlungsfrist.COLUMN_KONTINGENTART) = sKontingentart
                                        objChange17_objFDDZahlungsfrist.Zahlungsfristen.Rows(1)(FFE_Zahlungsfrist.COLUMN_KONTINGENTID) = sKKber
                                    End If
                                    If sKKber = "0003" Then
                                        objChange17_objFDDZahlungsfrist.Zahlungsfristen.Rows(2)(FFE_Zahlungsfrist.COLUMN_KONTINGENTART) = sKontingentart
                                        objChange17_objFDDZahlungsfrist.Zahlungsfristen.Rows(2)(FFE_Zahlungsfrist.COLUMN_KONTINGENTID) = sKKber
                                    End If
                                    If sKKber = "0004" Then
                                        objChange17_objFDDZahlungsfrist.Zahlungsfristen.Rows(3)(FFE_Zahlungsfrist.COLUMN_KONTINGENTART) = sKontingentart
                                        objChange17_objFDDZahlungsfrist.Zahlungsfristen.Rows(3)(FFE_Zahlungsfrist.COLUMN_KONTINGENTID) = sKKber
                                    End If
                                    If sKKber = "0006" Then
                                        objChange17_objFDDZahlungsfrist.Zahlungsfristen.Rows(4)(FFE_Zahlungsfrist.COLUMN_KONTINGENTART) = sKontingentart
                                        objChange17_objFDDZahlungsfrist.Zahlungsfristen.Rows(4)(FFE_Zahlungsfrist.COLUMN_KONTINGENTID) = sKKber
                                    End If
                                    If (CInt(m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AuthorizationLevel")) > 0) Then
                                        'Anwendung erfordert Autorisierung (Level>0)
                                        Dim DetailArray(2, 2) As Object
                                        Dim ms As MemoryStream
                                        Dim formatter As BinaryFormatter
                                        Dim b() As Byte

                                        ms = New MemoryStream()
                                        formatter = New BinaryFormatter()
                                        formatter.Serialize(ms, objSuche)
                                        b = ms.ToArray
                                        ms = New MemoryStream(b)
                                        DetailArray(0, 0) = ms
                                        DetailArray(0, 1) = "objSuche"
                                        If sKKber = "0001" Then
                                            objChange17_objFDDZahlungsfrist.Zahlungsfristen.Rows(0)(FFE_Zahlungsfrist.COLUMN_ZAHLUNGSFRIST_NEU) = t.Text.Trim
                                        End If
                                        If sKKber = "0002" Then
                                            objChange17_objFDDZahlungsfrist.Zahlungsfristen.Rows(1)(FFE_Zahlungsfrist.COLUMN_ZAHLUNGSFRIST_NEU) = t.Text.Trim
                                        End If
                                        If sKKber = "0003" Then
                                            objChange17_objFDDZahlungsfrist.Zahlungsfristen.Rows(2)(FFE_Zahlungsfrist.COLUMN_ZAHLUNGSFRIST_NEU) = t.Text.Trim
                                        End If
                                        If sKKber = "0004" Then
                                            objChange17_objFDDZahlungsfrist.Zahlungsfristen.Rows(3)(FFE_Zahlungsfrist.COLUMN_ZAHLUNGSFRIST_NEU) = t.Text.Trim
                                        End If
                                        If sKKber = "0006" Then
                                            objChange17_objFDDZahlungsfrist.Zahlungsfristen.Rows(4)(FFE_Zahlungsfrist.COLUMN_ZAHLUNGSFRIST_NEU) = t.Text.Trim
                                        End If
                                        ms = New MemoryStream()
                                        formatter = New BinaryFormatter()
                                        formatter.Serialize(ms, objChange17_objFDDZahlungsfrist)
                                        b = ms.ToArray
                                        ms = New MemoryStream(b)
                                        DetailArray(1, 0) = ms
                                        DetailArray(1, 1) = "objFDDZahlungsfrist"

                                        'Pruefen, ob schon in der Autorisierung.
                                        Dim strInitiator As String = ""
                                        Dim intAuthorizationID As Int32
                                        Dim iDistrictID As Integer
                                        If Not Session("SelectedDistrict") = Nothing Then
                                            iDistrictID = Session("SelectedDistrict")
                                        Else
                                            iDistrictID = m_User.Organization.OrganizationReference
                                        End If
                                        Dim sString As String = Session("AppID")
                                        If t.Enabled = True Then
                                            m_App.CheckForPendingAuthorization(CInt(Session("AppID")), iDistrictID, objSuche.REFERENZ, "", m_User.IsTestUser, strInitiator, intAuthorizationID)
                                            intAuthorizationID = WriteAuthorization(m_App.Connectionstring, CInt(Session("AppID")), m_User.UserName, iDistrictID, objSuche.REFERENZ, "", "", sKKber, m_User.IsTestUser, DetailArray)
                                            logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, Right(Session("SelectedDealer").ToString, 5), "Zahlungsfrist für Händler " & objSuche.REFERENZ & " erfolgreich initiiert.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
                                            lblInformation.Text = "Die Änderung der Zahlungsfrist wurde initiiert und liegt zur Autorisierung vor!"
                                            FocusScript.Visible = False
                                            cmdSave.Visible = False
                                            t.Enabled = False
                                        End If
                                    Else
                                        'objChange17_objFDDZahlungsfrist.Zahlungsfristen.Rows(0)(FFE_Zahlungsfrist.COLUMN_ZAHLUNGSFRIST_NEU) = t.Text.Trim
                                    End If

                                Else
                                    'Throw New Exception("Bitte geben Sie eine Zahl von 0 bis 99 ein oder lassen Sie das Feld leer.")
                                End If
                            End If
                        End If
                    Next

                Next

            Next

        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "Change17Edit", "Page_Load", ex.ToString)

            lblError.Text = ex.Message

        End Try
    End Sub

    Private Sub cmdAuthorize_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAuthorize.Click
        Try
            Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
            Dim item As DataGridItem
            For Each item In DataGrid1.Items
                'Werte ermitteln

                Dim cell As TableCell
                cell = item.Cells(0)
                Dim strAuftragsnummer As String
                strAuftragsnummer = cell.Text

                For Each cell In item.Cells

                    Dim c As System.Web.UI.Control
                    For Each c In cell.Controls

                        If TypeOf c Is TextBox Then

                            Dim t As TextBox = CType(c, TextBox)

                            'Neue Zahlungsfrist auslesen und in DataRow schreiben
                            If t.ID = "txtZahlungsfristNeu" AndAlso t.Visible = True Then

                                If Regex.IsMatch(t.Text.Trim, "^\d{0,2}$", RegexOptions.Singleline) Then
                                    If strAuftragsnummer = "0001" Then
                                        objChange17_objFDDZahlungsfrist.RowFaelligkeit = 0
                                        objChange17_objFDDZahlungsfrist.Zahlungsfristen.Rows(0)(FFE_Zahlungsfrist.COLUMN_ZAHLUNGSFRIST_NEU) = t.Text.Trim
                                    End If
                                    If strAuftragsnummer = "0002" Then
                                        objChange17_objFDDZahlungsfrist.RowFaelligkeit = 1
                                        objChange17_objFDDZahlungsfrist.Zahlungsfristen.Rows(1)(FFE_Zahlungsfrist.COLUMN_ZAHLUNGSFRIST_NEU) = t.Text.Trim
                                    End If
                                    If strAuftragsnummer = "0003" Then
                                        objChange17_objFDDZahlungsfrist.RowFaelligkeit = 2
                                        objChange17_objFDDZahlungsfrist.Zahlungsfristen.Rows(2)(FFE_Zahlungsfrist.COLUMN_ZAHLUNGSFRIST_NEU) = t.Text.Trim
                                    End If
                                    If strAuftragsnummer = "0004" Then
                                        objChange17_objFDDZahlungsfrist.RowFaelligkeit = 3
                                        objChange17_objFDDZahlungsfrist.Zahlungsfristen.Rows(3)(FFE_Zahlungsfrist.COLUMN_ZAHLUNGSFRIST_NEU) = t.Text.Trim
                                    End If
                                    If strAuftragsnummer = "0006" Then
                                        objChange17_objFDDZahlungsfrist.RowFaelligkeit = 4
                                        objChange17_objFDDZahlungsfrist.Zahlungsfristen.Rows(4)(FFE_Zahlungsfrist.COLUMN_ZAHLUNGSFRIST_NEU) = t.Text.Trim
                                    End If
                                Else
                                    Throw New Exception("Bitte geben Sie eine Zahl von 0 bis 99 ein oder lassen Sie das Feld leer.")
                                End If
                            End If

                        End If
                    Next

                Next

            Next

            objChange17_objFDDZahlungsfrist.Change()

            'detail zahllungsfristen speichern.
            'entgegen aller logik kann man die detailszahlungsfristen nur als tabelle ins sap schieben.
            'also praktisch nur die sätze einer kontingentart in der tabelle ändern und ins sap schießen, je nach dem welche Kontingentart gerade betroffen ist. 
            'JJU2008.09.12

            For Each item In DataGrid1.Items

                'If item.Visible = True Then
                If item.Cells(1).Visible = True Then 'der urheber stand darauf nicht das item sondern alle cells in einem item auf visible=false zu setzen wenns nicht angezeigt werden soll.... JJU20080915

                    'bearbeitete Tabelle werte abrufen 
                    Dim tmpRows() As DataRow
                    tmpRows = objChange17_objFDDZahlungsfrist.FaelligkeitenAnzeigeTable.Select("KKBER='" & item.Cells(0).Text & "'")

                    If Not tmpRows.Length = 0 Then 'wenn min 1 detailseintrag zur kontingent art
                        Dim i As Int32 = 0
                        Dim tmpRow As DataRow
                        While i < tmpRows.Length
                            tmpRow = objChange17_objFDDZahlungsfrist.SapTabelle.NewRow
                            tmpRow("MANDT") = tmpRows(i)("MANDT")
                            tmpRow("KUNNR") = tmpRows(i)("KUNNR")
                            tmpRow("KUNNR_ZF") = tmpRows(i)("KUNNR_ZF")
                            tmpRow("KKBER") = tmpRows(i)("KKBER")
                            tmpRow("ZZVGRUND") = tmpRows(i)("ZZVGRUND")
                            'wenn keine eingabe getätigt wurde, den alten wert wieder ins sap schreiben, da sonst sap die werte mit 0 überschreibt weil es das nicht checkt JJU20081105
                            If tmpRows(i)("NeueZFProAG").ToString = "" Then
                                tmpRow("ZZFRIST") = tmpRows(i)("ZZFRIST")
                            Else 'neuer wert vorhanden also setzen

                                tmpRow("ZZFRIST") = tmpRows(i)("NeueZFProAG")
                            End If

                            objChange17_objFDDZahlungsfrist.SapTabelle.Rows.Add(tmpRow)
                            objChange17_objFDDZahlungsfrist.SapTabelle.AcceptChanges()
                            i += 1
                        End While

                        objChange17_objFDDZahlungsfrist.changeFristDetails()
                        If Not objChange17_objFDDZahlungsfrist.Status = 0 Then
                            Throw New Exception("Beim übertragen der Detail Zahlungsfristen in das System ist ein Fehler aufgetreten: " & objChange17_objFDDZahlungsfrist.Message)
                        End If
                    End If
                End If


            Next
            DeleteAuthorizationEntry(m_App.Connectionstring, Session("AuthorizationID"))
            HelpProcedures.fakeLogEntryForAutorisierung(logApp, m_User, Session.SessionID, Session("AppID"), "Die Autorisierung für die Änderung der Zahlungsfrist für Händler " & objSuche.REFERENZ & " (DAD: " & Session("SelectedDealer").ToString & ") war erfolgreich!")
            lblInformation.Text = "Die Zahlungsfrist für Händler " & lblHaendlerNummer.Text & " wurde erfolgreich geändert."
            DataGrid1.Visible = False
            cmdAuthorize.Visible = False
            cmdDelete.Visible = False
            cmdBack.Visible = True

        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "Change17Edit", "Page_Load", ex.ToString)

            lblError.Text = ex.Message

        End Try
    End Sub

    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBack.Click
        Try
            Response.Redirect("Change14.aspx?AppID=" & CStr(m_User.Applications.Select("AppName='Change14'")(0).Item("AppID")) & "&Aut=@!", False)
        Catch
        End Try
    End Sub
    Private Sub btn_Ok_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Ok.Click


        If IsNumeric(txt_NF.Text) = True Then
            Dim sNF As String
            Dim cell As TableCell
            Dim cell2 As TableCell
            Dim c As System.Web.UI.Control
            Dim item As DataGridItem
            sNF = txt_NF.Text
            If chk_temp.Checked = True Then

                For Each item In DataGrid1.Items
                    cell = item.Cells(0)
                    For Each cell In item.Cells
                        If cell.Text = "0001" Then
                            cell2 = item.Cells(3)
                            For Each c In cell2.Controls
                                If TypeOf c Is TextBox Then
                                    Dim t As TextBox = CType(c, TextBox)
                                    If t.ID = "txtZahlungsfristNeu" Then
                                        t.Text = sNF
                                        cmdSave.Enabled = True
                                    End If
                                End If
                            Next
                        End If
                    Next
                Next
            End If
            If chk_endgueltig.Checked = True Then
                For Each item In DataGrid1.Items
                    cell = item.Cells(0)
                    For Each cell In item.Cells
                        If cell.Text = "0002" Then
                            cell2 = item.Cells(3)
                            For Each c In cell2.Controls
                                If TypeOf c Is TextBox Then
                                    Dim t As TextBox = CType(c, TextBox)
                                    If t.ID = "txtZahlungsfristNeu" Then
                                        t.Text = sNF
                                        cmdSave.Enabled = True
                                    End If

                                End If
                            Next
                        End If
                    Next
                Next
            End If
            If chk_Retail.Checked = True Then
                For Each item In DataGrid1.Items
                    cell = item.Cells(0)
                    For Each cell In item.Cells
                        If cell.Text = "0003" Then
                            cell2 = item.Cells(3)
                            For Each c In cell2.Controls
                                If TypeOf c Is TextBox Then
                                    Dim t As TextBox = CType(c, TextBox)
                                    If t.ID = "txtZahlungsfristNeu" Then
                                        t.Text = sNF
                                        cmdSave.Enabled = True
                                    End If

                                End If
                            Next
                        End If
                    Next
                Next
            End If
            If chk_dp.Checked = True Then
                For Each item In DataGrid1.Items
                    cell = item.Cells(0)
                    For Each cell In item.Cells
                        If cell.Text = "0004" Then
                            cell2 = item.Cells(3)
                            For Each c In cell2.Controls
                                If TypeOf c Is TextBox Then
                                    Dim t As TextBox = CType(c, TextBox)
                                    If t.ID = "txtZahlungsfristNeu" Then
                                        t.Text = sNF
                                        cmdSave.Enabled = True
                                    End If

                                End If
                            Next
                        End If
                    Next
                Next
            End If
            If chk_kf.Checked = True Then
                For Each item In DataGrid1.Items
                    cell = item.Cells(0)
                    For Each cell In item.Cells
                        If cell.Text = "0006" Then
                            cell2 = item.Cells(3)
                            For Each c In cell2.Controls
                                If TypeOf c Is TextBox Then
                                    Dim t As TextBox = CType(c, TextBox)
                                    If t.ID = "txtZahlungsfristNeu" Then
                                        t.Text = sNF
                                        cmdSave.Enabled = True
                                    End If

                                End If
                            Next
                        End If
                    Next
                Next
            End If
        Else : lblError.Text = "Bitte geben Sie einen numerischen Wert für die neue Fälligkeit ein!"
            cmdSave.Enabled = False
        End If
        txt_NF.Text = ""
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
#End Region

#Region "Methoden"

    Private Sub LoadAuthorizatioData(ByVal AuthorizationID As Int32)
        Dim sneu As String
        Dim sKkber As String
        Dim OutPutStream As System.IO.MemoryStream = GiveAuthorizationDetails(m_App.Connectionstring, AuthorizationID, "objFDDZahlungsfrist")
        Dim formatter As New BinaryFormatter()
        Dim Zahlungsfristen As DataTable
        objChange17_objFDDZahlungsfrist = New FFE_Zahlungsfrist(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
        objChange17_objFDDZahlungsfrist = DirectCast(formatter.Deserialize(OutPutStream), FFE_Zahlungsfrist)
        Session("objChange17_objFDDZahlungsfrist") = objChange17_objFDDZahlungsfrist
        Zahlungsfristen = objChange17_objFDDZahlungsfrist.Zahlungsfristen
        Dim i As Integer

        FillGrid()


        For i = 0 To Zahlungsfristen.Rows.Count - 1
            sneu = Zahlungsfristen.Rows(i)(FFE_Zahlungsfrist.COLUMN_ZAHLUNGSFRIST_NEU).ToString()
            sKkber = Zahlungsfristen.Rows(i)(FFE_Zahlungsfrist.COLUMN_KONTINGENTID).ToString()
            If sneu.Length > 0 Then

                Dim item As DataGridItem
                Dim c As System.Web.UI.Control
                For Each item In DataGrid1.Items
                    'Werte ermitteln

                    Dim cell As TableCell
                    cell = item.Cells(3)
                    Dim cell2 As TableCell = item.Cells(0)
                    For Each c In cell.Controls

                        If TypeOf c Is TextBox Then

                            Dim t As TextBox = CType(c, TextBox)

                            'Neue Zahlungsfrist auslesen und in DataRow schreiben
                            If t.ID = "txtZahlungsfristNeu" And sKkber = cell2.Text Then
                                t.Text = sneu
                                t.Enabled = False
                                If sKkber = "0001" Then
                                    chk_temp.Enabled = False
                                    chk_temp.Checked = False
                                End If
                                If sKkber = "0002" Then
                                    chk_endgueltig.Enabled = False
                                    chk_endgueltig.Checked = False
                                End If
                                If sKkber = "0003" Then
                                    chk_Retail.Enabled = False
                                    chk_Retail.Checked = False
                                    chk_Retail.Visible = True
                                End If
                                If sKkber = "0004" Then
                                    chk_dp.Enabled = False
                                    chk_dp.Checked = False
                                End If
                                FocusScript.Visible = False
                                cmdSave.Visible = True
                                cmdSave.Enabled = True
                            End If
                        End If
                    Next
                Next
                If (Not Session("Authorization") Is Nothing) AndAlso (Not Session("ProcessReference") Is Nothing) Then
                    For Each item In DataGrid1.Items
                        'Werte ermitteln
                        Dim cell As TableCell
                        Dim cell2 As TableCell
                        Dim sreference As String = Session("ProcessReference")

                        cell = item.Cells(0)
                        If cell.Text = sreference Then
                            For Each cell2 In item.Cells
                                cell2.Visible = True
                            Next
                        Else
                            For Each cell2 In item.Cells
                                cell2.Visible = False
                            Next
                        End If
                    Next
                    cmdSave.Visible = False
                End If
            End If

        Next
        disableDetailInputFields()

        checkkontingente()
    End Sub


    Private Sub disableDetailInputFields()

        '----------------------------------------------------------------------
        ' Methode: disableDetailInputFields
        ' Autor: JJU
        ' Beschreibung: Diese methode disabelt alle eingabefelder der des detail Grids wenn die Übergeordnete Kontingentart einen wert Enthält
        ' Erstellt am: 2008.09.16
        ' ITA: 2216
        '----------------------------------------------------------------------

        For Each item As DataGridItem In DataGrid1.Items
            If Not item.FindControl("DataGridLvL2") Is Nothing Then
                If Not CType(item.FindControl("txtZahlungsfristNeu"), TextBox).Text.Trim(" "c) = "" Then
                    For Each item2 As DataGridItem In CType(item.FindControl("DataGridLvL2"), DataGrid).Items
                        CType(item2.FindControl("txtNeueZahlungsfristProAbrufgrund"), WebControl).Enabled = False
                    Next
                End If
            End If
        Next


    End Sub

    Private Sub StartLoadData()
        FocusScript.Visible = True

        cmdSave.Visible = True

        If objChange17_objFDDZahlungsfrist.Zahlungsfristen Is Nothing OrElse objChange17_objFDDZahlungsfrist.Zahlungsfristen.Rows.Count = 0 Then
            lblError.Text = "Fehler: Es konnten keine Zahlungsfristendaten ermittelt werden."
            FocusScript.Visible = False
            lblError.CssClass = "TextError"
        Else
            lblError.CssClass = "LabelExtraLarge"
            FillGrid()
        End If
    End Sub

    Private Sub FillGrid()
        DataGrid1.DataSource = objChange17_objFDDZahlungsfrist.Zahlungsfristen

        DataGrid1.DataBind()
        Dim tmpDataGrid As DataGrid
        Dim tmpDV As DataView

        For Each item As DataGridItem In DataGrid1.Items
            If Not item.FindControl("DataGridLvL2") Is Nothing Then
                tmpDataGrid = CType(item.FindControl("DataGridLvL2"), DataGrid)
                tmpDV = objChange17_objFDDZahlungsfrist.FaelligkeitenAnzeigeTable.DefaultView
                tmpDV.RowFilter = "KKBER='" & item.Cells(0).Text & "'"
                tmpDataGrid.DataSource = tmpDV
                If Not tmpDV.Count = 0 Then
                    tmpDataGrid.DataBind()
                End If
            End If
        Next

    End Sub

    Private Sub checkkontingente()
        Dim iRow As DataRow

        objFDDBank = New FFE_BankBase(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
        objFDDBank.CreditControlArea = "ZDAD"
        objFDDBank.Customer = Session("SelectedDealer").ToString
        objFDDBank.Show()
        Session("objFDDBank") = objFDDBank


        For Each iRow In objFDDBank.Kontingente.Rows
            Dim sTemp As String = iRow("Kreditkontrollbereich")
            Dim bTemp As Boolean = iRow("ZeigeKontingentart")
            Dim m_tblKontingente As New DataTable()

            If sTemp = "0001" Then
                chk_temp.Visible = True
            End If
            If sTemp = "0002" Then
                chk_endgueltig.Visible = True
            End If
            If sTemp = "0003" Then
                chk_Retail.Visible = True
            End If
            If sTemp = "0004" Then
                chk_dp.Visible = True
            End If
            If sTemp = "0006" Then
                chk_kf.Visible = True
            End If
        Next
    End Sub


    Protected Sub cmdDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdDelete.Click
        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)

        Try
            DeleteAuthorizationEntry(m_App.Connectionstring, Session("AuthorizationID"))
            HelpProcedures.fakeLogEntryForAutorisierung(logApp, m_User, Session.SessionID, Session("AppID"), "Die Autorisierung für die Löschung der Änderung bei der Zahlungsfrist für Händler " & objSuche.REFERENZ & " (DAD: " & Session("SelectedDealer").ToString & ") war erfolgreich!")
            lblInformation.Text = "Die Änderung der Zahlungsfrist des Händlers " & lblHaendlerNummer.Text & " wurde gelöscht."
            DataGrid1.Visible = False
            cmdAuthorize.Visible = False
            cmdDelete.Visible = False
            cmdBack.Visible = True
        Catch ex As Exception
            lblError.Text = "Fehler beim löschen des Vorgangs!"
        End Try

    End Sub
#End Region
End Class

' ************************************************
' $History: Change17Edit.aspx.vb $
' 
' *****************  Version 12  *****************
' User: Jungj        Date: 6.11.08    Time: 14:45
' Updated in $/CKAG/Applications/AppFFE/forms
' ITA 2216 nachbesserungen nach mazdatest
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 5.11.08    Time: 13:32
' Updated in $/CKAG/Applications/AppFFE/forms
' ITA 2216 Anpassungen anch MazdaTest
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 16.09.08   Time: 16:44
' Updated in $/CKAG/Applications/AppFFE/forms
' ITA 2216 testfertig
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 15.09.08   Time: 10:36
' Updated in $/CKAG/Applications/AppFFE/forms
' Historie hinzugefgt
' 
' ************************************************