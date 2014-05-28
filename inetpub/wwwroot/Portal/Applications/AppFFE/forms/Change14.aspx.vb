Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business

Imports System
Imports System.IO
Imports System.Collections
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization

Partial Public Class Change14
    Inherits System.Web.UI.Page

    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private m_intLineCount As Int32
    Private logApp As Base.Kernel.Logging.Trace
    Private objSuche As FFE_Search

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        cmdSave.Enabled = False
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        DataGrid1.Visible = False
        Datagrid2.Visible = False
        cmdBack.Visible = False

        Try

            GetAppIDFromQueryString(Me)

            Dim sAut As String = ""
            If Not Request.QueryString("Aut") = Nothing Then
                If Request.QueryString("Aut").ToString = "@!" Then
                    sAut = Request.QueryString("Aut").ToString()
                End If
            End If
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)
            lblPageTitle.Text = "Übersicht"

            logApp = New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
            Dim DistricCount As Integer
            Dim DistrictID As Integer
            If Not IsPostBack Then
                ''########### O.Rudolph ITA:946 Distriktstruktur 13.04.2007
                If sAut = "@!" Then ' zurück von der Autorisierung
                    DistrictID = Session("DistrictID")
                    ddlPageSize.Items.Add("10")
                    ddlPageSize.Items.Add("20")
                    ddlPageSize.Items.Add("50")
                    ddlPageSize.Items.Add("100")
                    ddlPageSize.Items.Add("200")
                    ddlPageSize.Items.Add("500")
                    ddlPageSize.Items.Add("1000")
                    ddlPageSize.SelectedIndex = 2
                    Session("Authorization") = Nothing
                    Session("AuthorizationID") = Nothing
                    Session("SelectedDealer") = Nothing
                    Session("PageIndex") = Nothing
                    If DistrictID > 0 Then
                        FillGrid(0, DistrictID.ToString)
                        Exit Sub
                    Else
                        FillGrid(0, m_User.Organization.OrganizationReference.ToString)
                        Exit Sub
                    End If

                Else
                    Session("Districtcount") = Nothing
                End If
                If Session("Districtcount") = Nothing Then

                    objSuche = New FFE_Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
                    If Not m_User.Organization.AllOrganizations Then
                        DistricCount = ReadDistricts()
                    Else
                        DistricCount = FilialenLesen()
                    End If
                    ddlPageSize.Items.Add("10")
                    ddlPageSize.Items.Add("20")
                    ddlPageSize.Items.Add("50")
                    ddlPageSize.Items.Add("100")
                    ddlPageSize.Items.Add("200")
                    ddlPageSize.Items.Add("500")
                    ddlPageSize.Items.Add("1000")
                    ddlPageSize.SelectedIndex = 2
                    Session("Authorization") = Nothing
                    Session("AuthorizationID") = Nothing
                    Session("SelectedDealer") = Nothing
                    Session("PageIndex") = Nothing
                End If
            Else
                DistricCount = Session("Districtcount")
                If DistricCount >= 1 Then
                    Session("DistrictID") = cmbDistrikte.SelectedItem.Value
                    If Session("SammelAutorisierungen") Is Nothing Then
                        FillGrid(0, cmbDistrikte.SelectedItem.Value.ToString)
                    Else
                        FillGrid1(0)
                    End If
                End If
            End If

            If DistricCount = 0 And Not IsPostBack Then
                trDistrikte.Visible = False
                trBackbutton.Visible = True
                trSaveButton.Visible = True
                'lnkKreditlimit.Visible = True
                'lnkVertragssuche.Visible = True
                If Session("SammelAutorisierungen") Is Nothing Then
                    FillGrid(0, m_User.Organization.OrganizationReference.ToString)
                Else
                    FillGrid1(0)
                End If
            ElseIf DistricCount = 1 And Not IsPostBack Then
                Session("DistrictID") = cmbDistrikte.SelectedItem.Value
                If Session("SammelAutorisierungen") Is Nothing Then
                    FillGrid(0, cmbDistrikte.SelectedItem.Value.ToString)
                Else
                    FillGrid1(0)
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim dt As New DataTable()
        Dim strTemp As String = "0"
        Dim DistrictID As Integer = 0
        Dim sDitriktOrganisation As String
        If m_User.IsTestUser Then
            strTemp = "1"
        End If
        If Not Session("DistrictID") = Nothing Then
            DistrictID = Session("DistrictID")
        End If
        If DistrictID > 0 Then
            sDitriktOrganisation = DistrictID.ToString
        Else
            sDitriktOrganisation = m_User.Organization.OrganizationReference.ToString
        End If
        Dim da As New SqlClient.SqlDataAdapter( _
          "SELECT * FROM vwAuthorization" & _
          " WHERE (NOT (InitializedBy='" & m_User.UserName & "'))" & _
          " AND (NOT (AuthorizationLevel<" & m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AuthorizationLevel").ToString & "))" & _
          " AND (OrganizationID=" & sDitriktOrganisation & ")" & _
          " AND (TestUser=" & strTemp & ")" & _
          " AND (BatchAuthorization=1)" & _
          " ORDER BY InitializedWhen", _
          m_App.Connectionstring)

        da.Fill(dt)
        dt.Columns.Add("Ergebnis", System.Type.GetType("System.String"))

        'Hier müssen die Vorgänge der verschiedenen Anwendungen autorisiert werden
        '1. Freigeben gesperrter Vorgänge
        Dim rowTemp As DataRow
        dt.AcceptChanges()
        For Each rowTemp In dt.Rows
            Select Case CStr(rowTemp("AppName"))
                Case "Change02"
                    Dim OutPutStream As System.IO.MemoryStream
                    Dim formatter As New BinaryFormatter()
                    Dim objSuche As FFE_Search
                    Dim objFDDBank As FFE_BankBase
                    Dim objFDDBank2 As FFE_Bank_2

                    OutPutStream = GiveAuthorizationDetails(m_App.Connectionstring, CInt(rowTemp("AuthorizationID")), "objSuche")
                    If OutPutStream Is Nothing Then
                        rowTemp("Ergebnis") = "Keine Daten für den Vorgang vorhanden."
                    Else
                        formatter = New BinaryFormatter()
                        objSuche = New FFE_Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
                        objSuche = DirectCast(formatter.Deserialize(OutPutStream), FFE_Search)
                        objSuche.ReNewSAPDestination(Session.SessionID.ToString, Session("AppID").ToString)

                        OutPutStream = GiveAuthorizationDetails(m_App.Connectionstring, CInt(rowTemp("AuthorizationID")), "objFDDBank")
                        If OutPutStream Is Nothing Then
                            rowTemp("Ergebnis") = "Keine Daten für den Vorgang vorhanden."
                        Else
                            formatter = New BinaryFormatter()
                            objFDDBank = New FFE_BankBase(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                            objFDDBank = DirectCast(formatter.Deserialize(OutPutStream), FFE_BankBase)
                            objFDDBank.SessionID = Session.SessionID.ToString

                            OutPutStream = GiveAuthorizationDetails(m_App.Connectionstring, CInt(rowTemp("AuthorizationID")), "objFDDBank2")
                            If OutPutStream Is Nothing Then
                                rowTemp("Ergebnis") = "Keine Daten für den Vorgang vorhanden."
                            Else
                                formatter = New BinaryFormatter()
                                objFDDBank2 = New FFE_Bank_2(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                                objFDDBank2 = DirectCast(formatter.Deserialize(OutPutStream), FFE_Bank_2)
                                objFDDBank2.SessionID = Session.SessionID.ToString

                                If objSuche.LeseHaendlerSAP_Einzeln(Session("AppID").ToString, Session.SessionID.ToString, rowTemp("CustomerReference").ToString) Then

                                    Dim rowsTemp() As DataRow = objFDDBank2.Auftraege.Select("VBELN='" & objFDDBank2.AuftragsNummer.TrimStart("0"c) & "'")

                                    logApp = New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
                                    logApp.CollectDetails("Kundennummer", CType(objFDDBank.Customer.TrimStart("0"c), Object), True)
                                    logApp.CollectDetails("Finanzierungsnr.", CType(rowsTemp(0)("ZZREFNR").ToString, Object))
                                    If Not rowsTemp(0)("ZZANFDT") Is System.DBNull.Value Then
                                        logApp.CollectDetails("Angefordert", CType(CDate(rowsTemp(0)("ZZANFDT")).ToShortDateString, Object))
                                    Else
                                        logApp.CollectDetails("Angefordert", CType("", Object))
                                    End If
                                    logApp.CollectDetails("Fahrgestellnummer", CType(rowsTemp(0)("ZZFAHRG").ToString, Object))
                                    logApp.CollectDetails("ZBII Nummer", CType(rowsTemp(0)("ZZBRIEF").ToString, Object))
                                    logApp.CollectDetails("Storno", CType(objFDDBank2.Storno, Object))
                                    logApp.CollectDetails("Freigabe", CType((Not objFDDBank2.Storno), Object))
                                    logApp.CollectDetails("Kunde", CType(objFDDBank2.Kunde, Object))
                                    logApp.CollectDetails("Fälligkeit", CType(objFDDBank2.Faelligkeit, Object))

                                    Try
                                        objFDDBank2.Change()
                                        If objFDDBank2.Status = 0 Then
                                            WriteLog("Freigabe für Händler " & objSuche.REFERENZ & " erfolgreich durchgeführt.", objSuche.REFERENZ)
                                            DeleteAuthorizationEntry(m_App.Connectionstring, CInt(rowTemp("AuthorizationID")))
                                            rowTemp("Ergebnis") = "OK"
                                        Else
                                            WriteLog("Fehler bei der Freigabe für Händler " & objSuche.REFERENZ & " , (Fehler: " & objFDDBank2.Message & ")", objSuche.REFERENZ, "ERR")
                                            lblError.Text = "Beim Speichern Ihrer Daten sind Fehler aufgetreten."
                                            rowTemp("Ergebnis") = objFDDBank2.Message
                                        End If

                                        logApp.WriteStandardDataAccessSAP(objFDDBank2.IDSAP)
                                    Catch ex As Exception
                                        WriteLog("Fehler bei der Freigabe für Händler " & objSuche.REFERENZ & " , (Fehler: " & ex.Message & ")", objSuche.REFERENZ, "ERR")
                                        lblError.Text = "Beim Speichern Ihrer Daten sind Fehler aufgetreten."
                                        rowTemp("Ergebnis") = ex.Message
                                    End Try
                                Else
                                    lblError.Text = "Fehler bei der Ermittlung der Händlerdaten.<br>(" & objSuche.ErrorMessage & ")"
                                    rowTemp("Ergebnis") = objSuche.ErrorMessage
                                End If
                            End If
                        End If
                    End If
                    '§§§JVE 29.09.2005 <begin>
                Case "Change06" 'Änderung fälligkeit und Status: Sammelautorisierung!
                    Dim OutPutStream As System.IO.MemoryStream
                    Dim formatter As New BinaryFormatter()
                    Dim kkber As String
                    Dim fdat As String
                    Dim objFDDBank2 As FFE_Bank_2

                    OutPutStream = GiveAuthorizationDetails(m_App.Connectionstring, CInt(rowTemp("AuthorizationID")), "objFDDBank2")

                    objFDDBank2 = New FFE_Bank_2(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                    objFDDBank2 = DirectCast(formatter.Deserialize(OutPutStream), FFE_Bank_2)

                    logApp = New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
                    logApp.CollectDetails("Kundennummer", CType(objFDDBank2.KUNNR, Object), True)
                    logApp.CollectDetails("Kreditkontrollbereich", CType(objFDDBank2.Auftraege.Rows(0)("ZZKKBER"), String), True)
                    logApp.CollectDetails("Fälligkeit", CType(objFDDBank2.Auftraege.Rows(0)("ZZFAEDT"), String), True)
                    kkber = CType(objFDDBank2.Auftraege.Rows(0)("ZZKKBER"), String)
                    fdat = CType(objFDDBank2.Auftraege.Rows(0)("ZZFAEDT"), String)
                    Try
                        objFDDBank2.SetStatus(CType(objFDDBank2.Auftraege.Rows(0)("LIZNR"), String), Right(CType(objFDDBank2.Auftraege.Rows(0)("PARNR"), String), 5), kkber, fdat)
                        If objFDDBank2.Status = 0 Then
                            WriteLog("Änderung Fälligkeit + Status für Finanzierungsnr. " & CType(objFDDBank2.Auftraege.Rows(0)("LIZNR"), String) & " erfolgreich durchgeführt.", CType(objFDDBank2.Auftraege.Rows(0)("PARNR"), String))
                            DeleteAuthorizationEntry(m_App.Connectionstring, CInt(rowTemp("AuthorizationID")))
                            rowTemp("Ergebnis") = "OK"
                        Else
                            WriteLog("Fehler bei Änderung Fälligkeit + Status für Finanzierungsnr. " & CType(objFDDBank2.Auftraege.Rows(0)("LIZNR"), String) & " , (Fehler: " & objFDDBank2.Message & ")", CType(objFDDBank2.Auftraege.Rows(0)("PARNR"), String), "ERR")
                            lblError.Text = "Beim Speichern Ihrer Daten sind Fehler aufgetreten."
                            rowTemp("Ergebnis") = objFDDBank2.Message
                        End If

                        logApp.WriteStandardDataAccessSAP(objFDDBank2.IDSAP)
                    Catch ex As Exception
                        WriteLog("Fehler bei Änderung Fälligkeit + Status für Finanzierungsnr. " & CType(objFDDBank2.Auftraege.Rows(0)("LIZNR"), String) & " , (Fehler: " & objFDDBank2.Message & ")", CType(objFDDBank2.Auftraege.Rows(0)("PARNR"), String), "ERR")
                        lblError.Text = "Beim Speichern Ihrer Daten sind Fehler aufgetreten."
                        rowTemp("Ergebnis") = ex.Message
                    End Try
                Case "Change08" 'Änderung fälligkeit und Status: Sammelautorisierung!
                    Dim OutPutStream As System.IO.MemoryStream
                    Dim formatter As New BinaryFormatter()
                    Dim kkber As String
                    Dim fdat As String
                    Dim objFDDBank2 As FFE_Bank_2

                    OutPutStream = GiveAuthorizationDetails(m_App.Connectionstring, CInt(rowTemp("AuthorizationID")), "objFDDBank2")

                    objFDDBank2 = New FFE_Bank_2(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                    objFDDBank2 = DirectCast(formatter.Deserialize(OutPutStream), FFE_Bank_2)

                    logApp = New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
                    logApp.CollectDetails("Kundennummer", CType(objFDDBank2.KUNNR, Object), True)
                    logApp.CollectDetails("Kreditkontrollbereich", CType(objFDDBank2.Auftraege.Rows(0)("ZZKKBER"), String), True)
                    logApp.CollectDetails("Fälligkeit", CType(objFDDBank2.Auftraege.Rows(0)("ZZFAEDT"), String), True)
                    kkber = CType(objFDDBank2.Auftraege.Rows(0)("ZZKKBER"), String)
                    fdat = CType(objFDDBank2.Auftraege.Rows(0)("ZZFAEDT"), String)
                    Try
                        objFDDBank2.SetStatus(CType(objFDDBank2.Auftraege.Rows(0)("LIZNR"), String), Right(CType(objFDDBank2.Auftraege.Rows(0)("PARNR"), String), 5), kkber, fdat)
                        If objFDDBank2.Status = 0 Then
                            WriteLog("Änderung Fälligkeit + Status für Finanzierungsnr. " & CType(objFDDBank2.Auftraege.Rows(0)("LIZNR"), String) & " erfolgreich durchgeführt.", CType(objFDDBank2.Auftraege.Rows(0)("PARNR"), String))
                            DeleteAuthorizationEntry(m_App.Connectionstring, CInt(rowTemp("AuthorizationID")))
                            rowTemp("Ergebnis") = "OK"
                        Else
                            WriteLog("Fehler bei Änderung Fälligkeit + Status für Finanzierungsnr. " & CType(objFDDBank2.Auftraege.Rows(0)("LIZNR"), String) & " , (Fehler: " & objFDDBank2.Message & ")", CType(objFDDBank2.Auftraege.Rows(0)("PARNR"), String), "ERR")
                            lblError.Text = "Beim Speichern Ihrer Daten sind Fehler aufgetreten."
                            rowTemp("Ergebnis") = objFDDBank2.Message
                        End If

                        logApp.WriteStandardDataAccessSAP(objFDDBank2.IDSAP)
                    Catch ex As Exception
                        WriteLog("Fehler bei Änderung Fälligkeit + Status für Finanzierungsnr. " & CType(objFDDBank2.Auftraege.Rows(0)("LIZNR"), String) & " , (Fehler: " & objFDDBank2.Message & ")", CType(objFDDBank2.Auftraege.Rows(0)("PARNR"), String), "ERR")
                        lblError.Text = "Beim Speichern Ihrer Daten sind Fehler aufgetreten."
                        rowTemp("Ergebnis") = ex.Message
                    End Try
                    '§§§JVE 29.09.2005 <end>
                Case "Change17"
                    Try
                        Dim OutPutStream As System.IO.MemoryStream
                        Dim formatter As New BinaryFormatter()
                        Dim objChange17_objFDDZahlungsfrist As FFE_Zahlungsfrist
                        Dim Zahlungsfristen As DataTable
                        Dim sneu As String = ""
                        Dim sKKber As String = rowTemp("ProcessReference3")
                        OutPutStream = GiveAuthorizationDetails(m_App.Connectionstring, CInt(rowTemp("AuthorizationID")), "objFDDZahlungsfrist")
                        objChange17_objFDDZahlungsfrist = New FFE_Zahlungsfrist(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                        objChange17_objFDDZahlungsfrist = DirectCast(formatter.Deserialize(OutPutStream), FFE_Zahlungsfrist)
                        Zahlungsfristen = objChange17_objFDDZahlungsfrist.Zahlungsfristen
                        Dim i As Integer
                        For i = 0 To Zahlungsfristen.Rows.Count - 1
                            If sKKber = Zahlungsfristen.Rows(i)(FFE_Zahlungsfrist.COLUMN_KONTINGENTID).ToString() Then
                                sneu = Zahlungsfristen.Rows(i)(FFE_Zahlungsfrist.COLUMN_ZAHLUNGSFRIST_NEU).ToString()

                                objChange17_objFDDZahlungsfrist.Zahlungsfristen.Rows(i)(FFE_Zahlungsfrist.COLUMN_ZAHLUNGSFRIST_NEU) = sneu
                                objChange17_objFDDZahlungsfrist.RowFaelligkeit = i
                                objChange17_objFDDZahlungsfrist.Change()

                                WriteLog("Änderung der Zahlungsfrist für Haendler " & objChange17_objFDDZahlungsfrist.Customer & " erfolgreich durchgeführt!", objChange17_objFDDZahlungsfrist.Customer)
                                DeleteAuthorizationEntry(m_App.Connectionstring, CInt(rowTemp("AuthorizationID")))
                                rowTemp("Ergebnis") = "OK"
                            End If
                        Next
                    Catch ex As Exception
                        'WriteLog("Fehler bei Änderung Zahlungsfristen für Vertragsnummer ", )
                        lblError.Text = "Beim Speichern Ihrer Daten sind Fehler aufgetreten."
                        rowTemp("Ergebnis") = ex.Message
                    End Try
                Case "Change18"
                    Dim OutPutStream As System.IO.MemoryStream
                    Dim formatter As New BinaryFormatter()
                    Dim objSuche As FFE_Search
                    Dim objFDDBank As FFE_BankBase
                    Dim objFDDBank2 As FFE_Bank_2

                    OutPutStream = GiveAuthorizationDetails(m_App.Connectionstring, CInt(rowTemp("AuthorizationID")), "objSuche")
                    If OutPutStream Is Nothing Then
                        rowTemp("Ergebnis") = "Keine Daten für den Vorgang vorhanden."
                    Else
                        formatter = New BinaryFormatter()
                        objSuche = New FFE_Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
                        objSuche = DirectCast(formatter.Deserialize(OutPutStream), FFE_Search)
                        objSuche.ReNewSAPDestination(Session.SessionID.ToString, Session("AppID").ToString)

                        OutPutStream = GiveAuthorizationDetails(m_App.Connectionstring, CInt(rowTemp("AuthorizationID")), "objFDDBank")
                        If OutPutStream Is Nothing Then
                            rowTemp("Ergebnis") = "Keine Daten für den Vorgang vorhanden."
                        Else
                            formatter = New BinaryFormatter()
                            objFDDBank = New FFE_BankBase(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                            objFDDBank = DirectCast(formatter.Deserialize(OutPutStream), FFE_BankBase)
                            objFDDBank.SessionID = Session.SessionID.ToString

                            OutPutStream = GiveAuthorizationDetails(m_App.Connectionstring, CInt(rowTemp("AuthorizationID")), "objFDDBank2")
                            If OutPutStream Is Nothing Then
                                rowTemp("Ergebnis") = "Keine Daten für den Vorgang vorhanden."
                            Else
                                formatter = New BinaryFormatter()
                                objFDDBank2 = New FFE_Bank_2(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                                objFDDBank2 = DirectCast(formatter.Deserialize(OutPutStream), FFE_Bank_2)
                                objFDDBank2.SessionID = Session.SessionID.ToString

                                If objSuche.LeseHaendlerSAP_Einzeln(Session("AppID").ToString, Session.SessionID.ToString, rowTemp("CustomerReference").ToString) Then

                                    Dim rowsTemp() As DataRow = objFDDBank2.Auftraege.Select("VBELN='" & objFDDBank2.AuftragsNummer.TrimStart("0"c) & "'")

                                    logApp = New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
                                    logApp.CollectDetails("Kundennummer", CType(objFDDBank.Customer.TrimStart("0"c), Object), True)
                                    logApp.CollectDetails("Finanzierungsnr.", CType(rowsTemp(0)("ZZREFNR").ToString, Object))
                                    If Not rowsTemp(0)("ZZANFDT") Is System.DBNull.Value Then
                                        logApp.CollectDetails("Angefordert", CType(CDate(rowsTemp(0)("ZZANFDT")).ToShortDateString, Object))
                                    Else
                                        logApp.CollectDetails("Angefordert", CType("", Object))
                                    End If
                                    logApp.CollectDetails("Fahrgestellnummer", CType(rowsTemp(0)("ZZFAHRG").ToString, Object))
                                    logApp.CollectDetails("ZBII-Nummer", CType(rowsTemp(0)("ZZBRIEF").ToString, Object))
                                    logApp.CollectDetails("Storno", CType(objFDDBank2.Storno, Object))
                                    logApp.CollectDetails("Freigabe", CType((Not objFDDBank2.Storno), Object))
                                    logApp.CollectDetails("Kunde", CType(objFDDBank2.Kunde, Object))
                                    logApp.CollectDetails("Fälligkeit", CType(objFDDBank2.Faelligkeit, Object))

                                    Try
                                        objFDDBank2.Change()
                                        If objFDDBank2.Status = 0 Then
                                            WriteLog("Freigabe für Händler " & objSuche.REFERENZ & " erfolgreich durchgeführt.", objSuche.REFERENZ)
                                            DeleteAuthorizationEntry(m_App.Connectionstring, CInt(rowTemp("AuthorizationID")))
                                            rowTemp("Ergebnis") = "OK"
                                        Else
                                            WriteLog("Fehler bei der Freigabe für Händler " & objSuche.REFERENZ & " , (Fehler: " & objFDDBank2.Message & ")", objSuche.REFERENZ, "ERR")
                                            lblError.Text = "Beim Speichern Ihrer Daten sind Fehler aufgetreten."
                                            rowTemp("Ergebnis") = objFDDBank2.Message
                                        End If

                                        logApp.WriteStandardDataAccessSAP(objFDDBank2.IDSAP)
                                    Catch ex As Exception
                                        WriteLog("Fehler bei der Freigabe für Händler " & objSuche.REFERENZ & " , (Fehler: " & ex.Message & ")", objSuche.REFERENZ, "ERR")
                                        lblError.Text = "Beim Speichern Ihrer Daten sind Fehler aufgetreten."
                                        rowTemp("Ergebnis") = ex.Message
                                    End Try
                                Else
                                    lblError.Text = "Fehler bei der Ermittlung der Händlerdaten.<br>(" & objSuche.ErrorMessage & ")"
                                    rowTemp("Ergebnis") = objSuche.ErrorMessage
                                End If
                            End If
                        End If
                    End If
                Case Else
                    'mehr gibts zur Zeit nicht.
                    'in weiteren Cases könnten weitere Anwendungen
                    'mit Sammelautorisierungen bedient werden
            End Select
        Next
        dt.AcceptChanges()

        Session("SammelAutorisierungen") = dt
        DataGrid1.Visible = False
        FillGrid1(0)
        cmdSave.Visible = False
        cmdBack.Visible = True
    End Sub
    Private Function ReadDistricts() As Integer
        'diesen ganzen block nur einmal, da die dropdown sich eigentlich die werte über einen postback hinaus merken sollte.
        Dim districtCount As Integer

        'Hier Zugriff auf neue BAPI....
        Dim appId As Integer = CInt(Session("AppID"))
        districtCount = objSuche.ReadDistrictSAP(Me.Page, appId, Session.SessionID)
        If districtCount > 0 Then
            With cmbDistrikte
                .Items.Clear()
                'dropdown füllen:
                .DataSource = objSuche.District
                .DataTextField = "NAME1"
                .DataValueField = "DISTRIKT"
                .DataBind()
                'vorbelegten distrikt suchen
                objSuche.District.RowFilter = "VORBELEGT='1'"
                Dim drv As DataRowView
                For Each drv In objSuche.District
                    Dim li As ListItem = .Items.FindByValue(drv("DISTRIKT").ToString)
                    If Not li Is Nothing Then
                        If Not .SelectedItem Is Nothing Then
                            .SelectedItem.Selected = False ' falls schon ein anderer selektiert, dann deselektieren.
                        End If
                        li.Selected = True
                    End If
                    Exit For ' nach dem ersten aussteigen, da nur einer selektiert sein darf!!!
                Next
            End With
        End If
        Session("DistrictCount") = districtCount
        Return districtCount
    End Function
    Private Function FilialenLesen(Optional ByVal blnUseComboInput As Boolean = False) As Integer
        If blnUseComboInput Then
            objSuche.HaendlerFiliale = cmbDistrikte.SelectedItem.Value
        Else
            If m_User.Organization.AllOrganizations Then
                objSuche.HaendlerFiliale = ""
            Else
                If m_User.Organization.OrganizationReference.Trim(" "c).Trim("0"c).Length = 0 Then
                    objSuche.HaendlerFiliale = "00"
                Else
                    objSuche.HaendlerFiliale = m_User.Organization.OrganizationReference
                End If
            End If
        End If

        If objSuche.LeseFilialenSAP() > 0 Then
            Session("objSuche") = objSuche
            cmbDistrikte.DataSource = objSuche.Filialen
            cmbDistrikte.DataValueField = "FILIALE"
            cmbDistrikte.DataTextField = "DISPLAY_FILIALE"
            cmbDistrikte.DataBind()
            Session("DistrictCount") = objSuche.Filialen.Count
            FilialenLesen = objSuche.Filialen.Count
            If objSuche.Filialen.Count = 1 Then
                cmbDistrikte.SelectedIndex = 0
                cmbDistrikte.Visible = False
            Else
                If Not m_User.Organization.AllOrganizations Then
                    Dim _li As ListItem
                    For Each _li In cmbDistrikte.Items
                        If _li.Value = m_User.Organization.OrganizationReference Then
                            _li.Selected = True
                            Exit For
                        End If
                    Next
                    cmbDistrikte.Visible = False
                Else
                    cmbDistrikte.SelectedIndex = 0
                    cmbDistrikte.Visible = True
                End If
            End If
        Else
            cmbDistrikte.Visible = False
            lblError.Text = "Fehler: " & objSuche.ErrorMessage
        End If
    End Function
    Private Sub FillGrid0(ByRef tblData As DataTable, ByRef dgShow As DataGrid, ByVal intPageIndex As Int32, ByVal strSort As String)
        m_intLineCount = 0
        Dim DistrictCount As Integer = Session("DistrictCount")
        If Not cmbDistrikte.SelectedItem Is Nothing Then
            'If DistrictCount > 0 Then
            lblDistrikt.Text = cmbDistrikte.SelectedItem.Text
            lblDistrikt.Visible = True
            cmbDistrikte.Visible = False
        Else : trDistrikte.Visible = False
        End If
        If tblData.Rows.Count = 0 Then
            trBackbutton.Visible = True
            cmdSelect.Visible = False
            If DistrictCount > 0 Then cmdReset.Visible = True
            trSaveButton.Visible = True
            dgShow.Visible = False
            ddlPageSize.Visible = False
            lblNoData.Visible = True
            lblNoData.Text = "Keine Daten zur Anzeige gefunden."
            ShowScript.Visible = False
            cmdSave.Enabled = False
            lblLegende.Visible = False
        Else
            dgShow.Visible = True
            lblNoData.Visible = False
            trBackbutton.Visible = True
            cmdSelect.Visible = False
            If DistrictCount > 0 Then cmdReset.Visible = True
            trSaveButton.Visible = True
            'lnkKreditlimit.Visible = True
            'lnkVertragssuche.Visible = True
            lblLegende.Visible = True

            'hier werden die Autorisationseinträge die schon SapSeitig freigegeben wurden, aus der Autorisierungstabelle entfernt
            Dim tmpobjFFEBank2 As FFE_Bank_2
            tmpobjFFEBank2 = New FFE_Bank_2(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
            tmpobjFFEBank2.checkAutorisierungsEintraegeForFreigabeGesperrterAuftraege(tblData)

            If Not tmpobjFFEBank2.Status = 0 Then
                lblError.Text = tmpobjFFEBank2.Message
                tmpobjFFEBank2 = Nothing
                Exit Sub
            End If
            tmpobjFFEBank2 = Nothing

            Dim tmpDataView As New DataView()
            tmpDataView = tblData.DefaultView
            Dim intTempPageIndex As Int32 = intPageIndex
            Dim strTempSort As String = ""
            Dim strDirection As String = ""

            Dim rowsTmp() As DataRow = tblData.Select("BatchAuthorization=1")
            If rowsTmp.GetLength(0) > 0 Then
                cmdSave.Enabled = True
            Else
                cmdSave.Enabled = False
            End If

            If strSort.Trim(" "c).Length > 0 Then
                intTempPageIndex = 0
                strTempSort = strSort.Trim(" "c)
                If (ViewState("Sort") Is Nothing) OrElse (ViewState("Sort").ToString = strTempSort) Then
                    If ViewState("Direction") Is Nothing Then
                        strDirection = "desc"
                    Else
                        strDirection = ViewState("Direction").ToString
                    End If
                Else
                    strDirection = "desc"
                End If

                If strDirection = "asc" Then
                    strDirection = "desc"
                Else
                    strDirection = "asc"
                End If

                ViewState("Sort") = strTempSort
                ViewState("Direction") = strDirection
            Else
                If Not ViewState("Sort") Is Nothing Then
                    strTempSort = ViewState("Sort").ToString
                    If ViewState("Direction") Is Nothing Then
                        strDirection = "asc"
                        ViewState("Direction") = strDirection
                    Else
                        strDirection = ViewState("Direction").ToString
                    End If
                End If
            End If

            If Not strTempSort.Length = 0 Then
                tmpDataView.Sort = strTempSort & " " & strDirection
            End If

            dgShow.CurrentPageIndex = intTempPageIndex

            dgShow.DataSource = tmpDataView
            dgShow.DataBind()

            lblNoData.Text = "Anzahl Vorgänge: " & tmpDataView.Count.ToString
            lblNoData.Visible = True

            If dgShow.PageCount > 1 Then
                dgShow.PagerStyle.CssClass = "PagerStyle"
                dgShow.DataBind()
                dgShow.PagerStyle.Visible = True
            Else
                dgShow.PagerStyle.Visible = False
            End If
        End If
    End Sub


    Private Sub FillGrid1(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        Dim dt As New DataTable()
        dt = CType(Session("SammelAutorisierungen"), DataTable)
        FillGrid0(dt, Datagrid2, intPageIndex, strSort)
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, ByVal sDitriktOrganisation As String, Optional ByVal strSort As String = "")
        Dim dt As New DataTable()
        Dim row As DataRow
        Dim strTemp As String = "0"
        If m_User.IsTestUser Then
            strTemp = "1"
        End If
        Dim da As New SqlClient.SqlDataAdapter( _
          "SELECT * FROM vwAuthorization" & _
          " WHERE (NOT (InitializedBy='" & m_User.UserName & "'))" & _
          " AND (NOT (AuthorizationLevel<" & m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AuthorizationLevel").ToString & "))" & _
          " AND (OrganizationID=" & sDitriktOrganisation & ")" & _
          " AND (TestUser=" & strTemp & ")" & _
          " ORDER BY InitializedWhen", _
          m_App.Connectionstring)


        da.Fill(dt)

        For Each row In dt.Rows
            If row("ProcessReference2").ToString = "2" Then
                row("ProcessReference2") = "S"
            End If
            If row("ProcessReference2").ToString = "3" Then
                row("ProcessReference2") = "N"
            End If
            If row("ProcessReference2").ToString = "4" Then
                row("ProcessReference2") = "V"
            End If
        Next
        dt.AcceptChanges()
        FillGrid0(dt, DataGrid1, intPageIndex, strSort)
    End Sub

    Private Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.ItemCommand
        If e.CommandName = "Autorisieren" Then
            Session("Authorization") = True
            Session("AuthorizationID") = e.Item.Cells(3).Text
            Dim strLastRecordParam As String = ""
            If DataGrid1.Items.Count < 2 Then
                strLastRecordParam = "&LastRecord=True"
            End If

            Select Case e.Item.Cells(1).Text

                Case "Change03" 'Kontingent-Aenderung
                    Session("objSuche") = Nothing
                    Session("SelectedDealer") = e.Item.Cells(7).Text
                    Response.Redirect(e.Item.Cells(1).Text & ".aspx?AppID=" & e.Item.Cells(0).Text & strLastRecordParam)
                Case "Change15" 'Sperrung Fahrzeugbrief-Kontingent
                    Session("objSuche") = Nothing
                    Session("SelectedDealer") = e.Item.Cells(7).Text
                    Response.Redirect(e.Item.Cells(1).Text & ".aspx?AppID=" & e.Item.Cells(0).Text & strLastRecordParam)
                Case "Change02" 'Freigabe gesperrter Auftraege
                    Session("objSuche") = Nothing
                    Session("SelectedDealer") = e.Item.Cells(7).Text
                    Response.Redirect(e.Item.Cells(1).Text & ".aspx?AppID=" & e.Item.Cells(0).Text & strLastRecordParam)
                Case "Change06" 'Änderung Fälligkeit + Händler  muss in Änderung Status Bank gehen
                    Session("objSuche") = Nothing
                    Session("SelectedDealer") = e.Item.Cells(7).Text
                    Response.Redirect("Change08.aspx?AppID=" & e.Item.Cells(0).Text & strLastRecordParam & "&Aut=" & e.Item.Cells(3).Text)
                Case "Change08" 'Änderung Status Bank
                    Session("objSuche") = Nothing
                    Session("SelectedDealer") = e.Item.Cells(7).Text
                    Response.Redirect(e.Item.Cells(1).Text & ".aspx?AppID=" & e.Item.Cells(0).Text & strLastRecordParam & "&Aut=" & e.Item.Cells(3).Text)
                Case "Change16" 'Regionalbürozuordnung
                    Session("objSuche") = Nothing
                    Session("SelectedDealer") = e.Item.Cells(7).Text
                    Dim sKKber As String = e.Item.Cells(12).Text
                    Session("ProcessReference") = sKKber
                    Response.Redirect(e.Item.Cells(1).Text & ".aspx?AppID=" & e.Item.Cells(0).Text & strLastRecordParam)
                Case "Change17" 'Zahlungsfrist
                    Session("objSuche") = Nothing
                    Session("SelectedDealer") = e.Item.Cells(7).Text
                    Dim sKKber As String = e.Item.Cells(12).Text
                    Session("ProcessReference") = sKKber
                    Response.Redirect(e.Item.Cells(1).Text & "Edit.aspx?AppID=" & e.Item.Cells(0).Text & strLastRecordParam)
                Case "Change18" 'Retail ITA:1067
                    Session("objSuche") = Nothing
                    Session("SelectedDealer") = e.Item.Cells(7).Text
                    Response.Redirect(e.Item.Cells(1).Text & "Edit.aspx?AppID=" & e.Item.Cells(0).Text & strLastRecordParam)
            End Select
        ElseIf e.CommandName = "Loeschen" Then
            Try
                DeleteAuthorizationEntry(m_App.Connectionstring, CInt(e.Item.Cells(3).Text))
                WriteLog("Autorisierung - Anwendung " & e.Item.Cells(4).Text & ", Initiator: " & e.Item.Cells(5).Text & ", Angelegt: " & e.Item.Cells(6).Text & ", Händler: " & e.Item.Cells(7).Text & ", Weiteres Merkmal: " & e.Item.Cells(8).Text & " - erfolgreich gelöscht.", e.Item.Cells(7).Text)
                lblNoData.Text = "<b>Daten wurden aus Autorisierung entfernt.</b><br>&nbsp;"
                Session("Authorization") = Nothing
                Session("AuthorizationID") = Nothing
                If Not Request.QueryString("Aut") = Nothing Then
                    Request.QueryString("Aut").Replace("@!", "")
                End If
                If Not cmbDistrikte.SelectedItem Is Nothing Then
                    FillGrid(0, cmbDistrikte.SelectedItem.Value.ToString)
                Else
                    FillGrid(0, m_User.Organization.OrganizationReference.ToString)
                End If

            Catch ex As Exception
                WriteLog("Autorisierung - Anwendung " & e.Item.Cells(4).Text & ", Initiator: " & e.Item.Cells(5).Text & ", Angelegt: " & e.Item.Cells(6).Text & ", Händler: " & e.Item.Cells(7).Text & ", Weiteres Merkmal: " & e.Item.Cells(8).Text & " - Fehler beim Löschen: " & ex.Message, e.Item.Cells(7).Text, "ERR")
                lblError.Text = "Beim Speichern Ihrer Daten ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Try
        End If
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        If Not cmbDistrikte.SelectedItem Is Nothing Then
            FillGrid(e.NewPageIndex, cmbDistrikte.SelectedItem.Value.ToString)
        Else
            FillGrid(e.NewPageIndex, m_User.Organization.OrganizationReference.ToString)
        End If
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand

        If Not cmbDistrikte.SelectedItem Is Nothing Then
            FillGrid(DataGrid1.CurrentPageIndex, cmbDistrikte.SelectedItem.Value.ToString, e.SortExpression)
        Else
            FillGrid(DataGrid1.CurrentPageIndex, m_User.Organization.OrganizationReference.ToString, e.SortExpression)
        End If
    End Sub

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        If Not cmbDistrikte.SelectedItem Is Nothing Then
            FillGrid(0, cmbDistrikte.SelectedItem.Value.ToString)
        Else
            FillGrid(0, m_User.Organization.OrganizationReference.ToString)
        End If
    End Sub

    Private Sub WriteLog(ByVal strMessage As String, ByVal strHaendler As String, Optional ByVal strType As String = "APP")
        logApp.WriteEntry(strType, m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, Right(strHaendler, 5), strMessage, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 10)
    End Sub

    Private Sub Datagrid2_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles Datagrid2.PageIndexChanged
        FillGrid1(e.NewPageIndex)
    End Sub

    Private Sub Datagrid2_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles Datagrid2.SortCommand
        FillGrid1(Datagrid2.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBack.Click
        cmdBack.Visible = False
        cmdSave.Visible = True
        If Not cmbDistrikte.SelectedItem Is Nothing Then
            FillGrid(0, cmbDistrikte.SelectedItem.Value.ToString)
        Else
            FillGrid(0, m_User.Organization.OrganizationReference.ToString)
        End If
        Datagrid2.Visible = False
    End Sub

    Private Sub cmdReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReset.Click
        Dim DistrictCount As Integer = Session("DistrictCount")
        trBackbutton.Visible = False
        cmdSelect.Text = "Auswählen"
        trSaveButton.Visible = False
        lnkKreditlimit.Visible = False
        lnkVertragssuche.Visible = False
        ddlPageSize.Visible = False
        lblNoData.Visible = False
        ShowScript.Visible = False
        Session("Authorization") = Nothing
        Session("AuthorizationID") = Nothing
        Session("SelectedDealer") = Nothing
        lblDistrikt.Visible = False
        cmbDistrikte.Visible = True
        cmdReset.Visible = False
        cmdSelect.Visible = True
    End Sub


 

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

End Class
' ************************************************
' $History: Change14.aspx.vb $
' 
' *****************  Version 11  *****************
' User: Fassbenders  Date: 10.03.10   Time: 14:25
' Updated in $/CKAG/Applications/AppFFE/forms
' ITA: 2918
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 28.07.08   Time: 9:55
' Updated in $/CKAG/Applications/AppFFE/forms
' ITA 2057 fertig
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 24.07.08   Time: 12:39
' Updated in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 16.07.08   Time: 11:34
' Updated in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 11.07.08   Time: 9:13
' Updated in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 10.06.08   Time: 17:24
' Updated in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 4.06.08    Time: 18:43
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
