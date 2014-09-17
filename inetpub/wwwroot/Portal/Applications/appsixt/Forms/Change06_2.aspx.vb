Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports System.Threading

Public Class Change06_2
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

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private objPDIs As SIXT_PDI
    Private objPDIsThread As SIXT_PDI
    Private m_blnDateFormatError As Boolean
    Private m_blnThisDateGT30 As Boolean
    Private m_blnThisTimeGU As Boolean
    Private m_blnThisDateLT2 As Boolean
    Private m_blnIsWeekEnd As Boolean
    Private m_ZulassungsZeit As Integer
    Private m_blnPDIError As Boolean
    Private m_blnBemerkungDat As Boolean

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lnkKreditlimit As System.Web.UI.WebControls.HyperLink
    Protected WithEvents ucHeader As Header
    <CLSCompliant(False)> Protected WithEvents HG1 As DBauer.Web.UI.WebControls.HierarGrid
    Protected WithEvents cmdWeiter As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdAbsenden As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdVerwerfen As System.Web.UI.WebControls.LinkButton
    Protected WithEvents litAddScript As System.Web.UI.WebControls.Literal
    Protected WithEvents cmdZurueck As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkExcel As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblDownloadTip As System.Web.UI.WebControls.Label
    Protected WithEvents rowExcelLink As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents cmdWeitereAuswahl As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ZZFAHRG_WDB2112611A592199 As System.Web.UI.WebControls.CheckBox
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
    Dim thread As SixtThread

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        lnkKreditlimit.NavigateUrl = "Change06.aspx?AppID=" & Session("AppID").ToString
        litAddScript.Text = ""
        Try
            If (Session("objPDIs") Is Nothing) Then
                Response.Redirect(Request.UrlReferrer.ToString)
            Else
                objPDIs = CType(Session("objPDIs"), SIXT_PDI)
            End If

            lblHead.Text = objPDIs.Task
            ucStyles.TitleText = lblHead.Text

            m_App = New Base.Kernel.Security.App(m_User)

            m_blnDateFormatError = False
            m_blnThisDateGT30 = False
            m_blnThisDateLT2 = False
            m_blnBemerkungDat = False
            m_blnPDIError = False
            m_blnIsWeekEnd = False

            Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
            logApp.WriteEntry("DBG", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, CStr(Session("AppID")), "Page_Load Change06_2 gestartet.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
            If Not IsPostBack Then

                'cmdAbsenden.Attributes.Add("onClick", "SetFlag()")

                rowExcelLink.Visible = False
                FillGrid(-1)
                Select Case objPDIs.Task
                    Case "Entsperren"
                        lblMessage.Text = "Es stehen " & objPDIs.FahrzeugeGesamtGesperrt.ToString & " Fahrzeuge zum " & objPDIs.Task & " zur Verfügung."
                    Case Else
                        lblMessage.Text = "Es stehen " & objPDIs.FahrzeugeGesamtZulassungsf.ToString & " Fahrzeuge zum " & objPDIs.Task & " zur Verfügung."
                End Select
            Else
                Dim blnInputPerformed As Boolean
                If Request.Form("__EVENTTARGET").ToString = "cmdAbsenden" Or Request.Form("__EVENTTARGET").ToString = "cmdWeitereAuswahl" Then
                    blnInputPerformed = False
                Else
                    blnInputPerformed = CheckInput()
                End If

                If Not Request.Form.Item("PDINummer").ToString = "empty" Then
                    objPDIs.PDINummer = Request.Form.Item("PDINummer").ToString
                    objPDIs.Show()
                    If objPDIs.Status = 0 Then
                        FillGrid(CInt(Request.Form.Item("NummerInGrid")))
                    Else
                        lblError.Text = "Beim Laden der PDI-Daten ist ein Fehler aufgetreten.<br>(" & objPDIs.Message & ")"
                    End If
                End If

                If m_blnDateFormatError Then
                    lblError.Text = "Bitte geben Sie ein gültiges Datum ein (Format: TT.MM.JJJJ oder TTMMJJJJ)."
                Else
                    If m_blnThisDateGT30 Then
                        lblError.Text = "Bitte geben Sie ein Datum ein, das max. 30 Tage in der Zukunft liegt."
                    ElseIf m_blnIsWeekEnd Then
                        lblError.Text = "Zulassungsdatum fällt auf einen Samstag oder Sonntag"
                    Else

                        If m_blnThisDateLT2 Then
                            lblError.Text = "Zulassungen für den Folgetag nach " & m_ZulassungsZeit & ":00 Uhr nicht möglich."
                        End If
                    End If
                End If

                If m_blnThisTimeGU Then
                    lblError.Text = "Zulassungs-Parameter konnten nicht gelesen werden!"
                End If

                If m_blnPDIError Then
                    lblError.Text = "Bitte geben Sie die Nummer eines vorhandenen PDIs ein."
                End If

                If (objPDIs.Task = "Verschieben") AndAlso (m_blnBemerkungDat) Then
                    lblError.Text = "Bitte geben Sie ein gültiges Datum ein (Format: TT.MM.JJJJ oder TTMMJJJJ)."
                End If

                If blnInputPerformed Then
                    FillGrid(-1)
                End If
            End If
            logApp.WriteEntry("DBG", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, CStr(Session("AppID")), "Page_Load Change06_2 beendet.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub HG1_TemplateSelection(ByVal sender As Object, ByVal e As DBauer.Web.UI.WebControls.HierarGridTemplateSelectionEventArgs) Handles HG1.TemplateSelection
        Select Case (e.Row.Table.TableName)
            Case "Modelle"
                e.TemplateFilename = "..\Templates\Modell.ascx"
            Case Else
                Throw New NotImplementedException("Unexpected child row in TemplateSelection event")
        End Select
    End Sub

    Private Sub FillGrid(ByVal intExpand As Int32, Optional ByVal blnSelectedOnly As Boolean = False)
        'bind the DataSet to the HierarGrid
        If blnSelectedOnly Then
            HG1.DataSource = objPDIs.PDI_Data_Selected
        Else
            HG1.DataSource = objPDIs.PDI_Data
        End If
        HG1.DataMember = "PDIs"
        HG1.DataBind()

        If objPDIs.Task = "Entsperren" Then
            HG1.Columns(5).Visible = False
        End If

        Dim item As DataGridItem
        Dim cell As TableCell
        Dim control As Control
        Dim checkbox As CheckBox
        Dim intItem As Int32 = 0

        For Each item In HG1.Items
            Dim strPDI_Nummer As String = item.Cells(1).Text
            Dim intValidCounter As Int32
            If objPDIs.Task = "Entsperren" Then
                intValidCounter = CInt(item.Cells(6).Text)
            Else
                intValidCounter = CInt(item.Cells(5).Text)
            End If
            Dim blnDetails As Boolean = False
            Dim blnLoaded As Boolean = False
            cell = item.Cells(2)
            For Each control In cell.Controls
                If TypeOf control Is CheckBox Then
                    checkbox = CType(control, CheckBox)
                    If checkbox.ID = "chkDetails" And checkbox.Checked Then
                        blnDetails = True
                    End If
                    If checkbox.ID = "chkLoaded" And checkbox.Checked Then
                        blnLoaded = True
                    End If
                End If
            Next

            cell = item.Cells(0)
            cell.Width = New Unit("100px")
            cell.HorizontalAlign = HorizontalAlign.Center

            If Not blnLoaded Then
                If intValidCounter > 0 Then
                    Dim button As New ImageButton()
                    cell.Controls.Add(button)
                    button.ImageUrl = "/Portal/Images/plus.gif"
                    button.Attributes.Add("onClick", "DetailsSuchen('" & strPDI_Nummer.Trim(","c) & "'," & intItem & ",'" & objPDIs.Task & "');")
                Else
                    Dim image As New System.Web.UI.WebControls.Image()
                    cell.Controls.Add(image)
                    image.ImageUrl = "/Portal/Images/empty.gif"
                    'Dim literal As New LiteralControl()
                    'cell.Controls.Add(literal)
                    'literal.Text = "Keine Fahrzeuge"
                    If intExpand = intItem Then
                        intExpand = -1
                    End If
                End If
            Else
                If Not blnDetails Then
                    Dim image As New System.Web.UI.WebControls.Image()
                    cell.Controls.Add(image)
                    image.ImageUrl = "/Portal/Images/empty.gif"
                    'Dim literal As New LiteralControl()
                    'cell.Controls.Add(literal)
                    'literal.Text = "Keine Fahrzeuge"
                    If intExpand = intItem Then
                        intExpand = -1
                    End If
                End If
            End If

            If blnSelectedOnly Then
                HG1.RowExpanded(intItem) = True
            End If

            intItem += 1
        Next

        If intExpand > -1 Then
            HG1.RowExpanded(intExpand) = True
        End If

        Session("objPDIs") = objPDIs
    End Sub

    Private Sub cmdWeiter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdWeiter.Click
        If (Not m_blnDateFormatError) And (Not m_blnThisDateGT30) And (Not m_blnThisDateLT2) And (Not m_blnPDIError) And (Not m_blnThisTimeGU) And (Not m_blnBemerkungDat) And (Not m_blnIsWeekEnd) Then
            If objPDIs.SelectCars > 0 Then
                FillGrid(-1, True)
                cmdWeiter.Visible = False
                cmdAbsenden.Visible = True
                cmdVerwerfen.Visible = True
                cmdWeitereAuswahl.Visible = True
                cmdZurueck.Visible = False
                DisableAllInput()
                lblMessage.Text = "Aufgabe: " & objPDIs.Task & ", Anzahl Fahrzeuge: " & objPDIs.SelectedCars.ToString
            Else
                lblError.Text = "Keine Fahrzeuge ausgewählt."
                FillGrid(-1)
            End If
        Else
            FillGrid(-1)
        End If
    End Sub

    Private Sub cmdAbsenden_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAbsenden.Click
        Dim withThreading As String
        withThreading = CType(ConfigurationManager.AppSettings("Threading"), String)

        If (objPDIs.Task = "Zulassen") AndAlso (withThreading = "ON") Then
            '*** Zulassung in separatem Prozess ausführen.
            If Session("SixtThread") Is Nothing Then
                thread = New SixtThread()
                thread.WorkObject = objPDIs
                Session.Add("SixtThread", thread)
            Else
                thread = CType(Session("SixtThread"), SixtThread)
            End If

            If (Not thread.IsRunning) Then
                litAddScript.Text = "						<script language=""Javascript"">" & vbCrLf
                litAddScript.Text &= "						  <!-- //" & vbCrLf
                litAddScript.Text &= "                          window.open(""Change06_6.aspx?AppID=" & Session("AppID").ToString & """, ""_blank"", ""width=800,height=600,left=0,top=0,directories=no,menubar=no,toolbar=no,status=no,scrollbars=yes,resizable=no,dependent=no"");" & vbCrLf
                litAddScript.Text &= "                          location.replace(""Change06.aspx?AppID=" & Session("AppID").ToString & """);" & vbCrLf
                litAddScript.Text &= "						  //-->" & vbCrLf
                litAddScript.Text &= "						</script>" & vbCrLf
            Else
                lblError.Text = "Zulassung bereits in Beauftragung!"
            End If
        Else
            '*** Zulassung normal ausführen.        
            lblPageTitle.Text = "Bestätigung der Datenübernahme"
            Select Case objPDIs.Task
                Case "Zulassen"
                    objPDIs.setZulassung(Session("AppID").ToString, Session.SessionID.ToString, Me)
                Case "Sperren"
                    objPDIs.setSperre(Session("AppID").ToString, Session.SessionID.ToString, Me, "X")
                Case "Entsperren"
                    objPDIs.setSperre(Session("AppID").ToString, Session.SessionID.ToString, Me, " ")
                Case "Verschieben"
                    objPDIs.Verschieben(Session("AppID").ToString, Session.SessionID.ToString, Me)
                    'objPDIs.Change()
            End Select



            Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
            If objPDIs.Status = 0 Then
                If Not objPDIs.ErrorCount = 0 Then
                    lblError.Text = "Aufgabe: " & objPDIs.Task & ", Anzahl Fahrzeuge: " & objPDIs.SelectedCars.ToString & " (davon " & CStr(objPDIs.ErrorCount) & " mit Fehler)"
                    lblMessage.Visible = False
                    logApp.WriteEntry("ERR", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, CStr(Session("AppID")), lblError.Text, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0, objPDIs.Erledigt)
                Else
                    lblMessage.Text = "Aufgabe: " & objPDIs.Task & ", Anzahl Fahrzeuge: " & objPDIs.SelectedCars.ToString
                    logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, CStr(Session("AppID")), lblMessage.Text, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0, objPDIs.Erledigt)
                End If
                Dim objExcelExport As New Excel.ExcelExport()
                Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
                Try
                    Excel.ExcelExport.WriteExcel(objPDIs.Erledigt, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                    lnkExcel.NavigateUrl = "/Portal/Temp/Excel/" & strFileName
                    rowExcelLink.Visible = True
                Catch
                End Try
            Else
                lblError.Text = objPDIs.Message
            End If
            logApp.WriteStandardDataAccessSAP(objPDIs.IDSAP)

            If objPDIs.SelectCars > 0 Then
                FillGrid(-1, True)
            End If
            cmdWeiter.Visible = False
            cmdAbsenden.Visible = False
            cmdVerwerfen.Visible = False
            cmdWeitereAuswahl.Visible = False
            cmdZurueck.Visible = True
            DisableAllInput()
        End If
    End Sub

    Private Sub cmdVerwerfen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdVerwerfen.Click
        Dim i As Int32
        For i = 0 To objPDIs.PDI_Data.Tables("Fahrzeuge").Rows.Count - 1
            objPDIs.PDI_Data.Tables("Fahrzeuge").Rows(i)("Ausgewaehlt") = False
            objPDIs.PDI_Data.Tables("Fahrzeuge").Rows(i)("Bemerkung") = ""
            objPDIs.PDI_Data.Tables("Fahrzeuge").Rows(i)("ZielPDI") = ""
            objPDIs.PDI_Data.Tables("Fahrzeuge").Rows(i)("DatumErstzulassung") = DBNull.Value
        Next
        For i = 0 To objPDIs.PDI_Data.Tables("Modelle").Rows.Count - 1
            objPDIs.PDI_Data.Tables("Modelle").Rows(i)("Bemerkung") = ""
            objPDIs.PDI_Data.Tables("Modelle").Rows(i)("ZielPDI") = ""
            objPDIs.PDI_Data.Tables("Modelle").Rows(i)("DatumErstzulassung") = DBNull.Value
        Next
        objPDIs.SelectCars()
        lblMessage.Text = ""
        FillGrid(-1)

        cmdWeiter.Visible = True
        cmdWeitereAuswahl.Visible = False
        cmdAbsenden.Visible = False
        cmdVerwerfen.Visible = False
        cmdZurueck.Visible = False
    End Sub

    Private Sub DisableAllInput()
        Dim strTemp As String = "			<script language=""JavaScript"">" & vbCrLf
        strTemp &= "		<!-- //" & vbCrLf
        strTemp &= "		  DisableControls();" & vbCrLf
        strTemp &= "		//-->" & vbCrLf
        strTemp &= "			</script>"
        litAddScript.Text = strTemp
        lblPageTitle.Text = "Kontrolle der Fahrzeugauswahl"
    End Sub

    Private Function CheckInput() As Boolean
        Dim blnReturn As Boolean = False

        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        logApp.WriteEntry("DBG", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, CStr(Session("AppID")), "CheckInput gestartet.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
        Try
            Dim i As Int32
            Dim j As Int32
            For i = 0 To Request.Form.Count - 1
                If InStr(Request.Form.Keys.Item(i), "ZZFAHRG_") > 0 Then
                    Dim strFahrgestellNummer As String = (Right(Request.Form.Keys.Item(i), 17))
                    Dim strAuswahl_alt As String = CStr(Request.Form(i))
                    Dim intPos As Int32 = InStr(strAuswahl_alt, ",")
                    If intPos > 0 Then
                        strAuswahl_alt = Left(strAuswahl_alt, intPos - 1)
                    End If
                    Dim blnAuswahl_alt As Boolean = CType(strAuswahl_alt, Boolean)
                    Dim blnAuswahl_neu As Boolean = False
                    Dim strBemerkung As String = ""

                    Dim strBemerkungDatum As String = ""        '§§§ JVE 03.05.2006: Bemerkung Datum
                    Dim datBemerkungDatum As Date

                    Dim strDatumErstzulassung As String = ""
                    Dim strZielPDI As String = ""
                    If UCase(Request.Form(i + 1).ToString) = "ON" And InStr(Request.Form.Keys.Item(i + 1), "chkAuswahl") > 0 Then
                        blnAuswahl_neu = True

                        Dim blnThisPDIOK As Boolean = True
                        Select Case objPDIs.Task
                            Case "Zulassen"
                                strDatumErstzulassung = Request.Form(i - 1).ToString
                                'strBemerkung = Request.Form(i - 2).ToString
                                If Not IsDate(strDatumErstzulassung) Then
                                    If Not IsStandardDate(strDatumErstzulassung) Then
                                        If Not IsSAPDate(strDatumErstzulassung) Then
                                            m_blnDateFormatError = True
                                        End If
                                    End If
                                End If

                                If Not m_blnDateFormatError Then
                                    Dim dateZulassung As Date
                                    dateZulassung = CType(strDatumErstzulassung, Date)

                                    'Prüfen, ob Datum mehr als 30 Tage in der Zukunft liegt.
                                    If (dateZulassung > Today.AddDays(30)) Then
                                        m_blnThisDateGT30 = True
                                        logApp.WriteEntry("ERR", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, CStr(Session("AppID")), "Fehlerhafte Datumseingabe für VIN " & strFahrgestellNummer & ". Datum mehr als 30 Tage in der Zukunft: " & Request.Form(i - 1).ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)

                                    ElseIf dateZulassung.DayOfWeek = DayOfWeek.Saturday OrElse dateZulassung.DayOfWeek = DayOfWeek.Sunday Then
                                        m_blnIsWeekEnd = True
                                        logApp.WriteEntry("ERR", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, CStr(Session("AppID")), "Zulassung fällt auf das Wochenende " & strFahrgestellNummer & ". Datum mehr als 30 Tage in der Zukunft: " & Request.Form(i - 1).ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
                                    Else

                                        'Prüfen, ob für den Benutzer Zulassungen nach 11 Uhr erlaubt sind!
                                        Dim group As Integer
                                        Dim flag As Integer

                                        Try
                                            m_blnThisTimeGU = False
                                            m_ZulassungsZeit = CType(ConfigurationManager.AppSettings("ZulassungNachUhrzeit"), Integer)

                                        Catch ex As Exception
                                            m_blnThisTimeGU = True
                                        End Try

                                        group = m_User.GroupID
                                        flag = SIXT_PDI.getFlag(group)


                                        If (flag = 0) And (Now.Hour >= m_ZulassungsZeit) And (dateZulassung = Today.AddDays(IIf(Today.DayOfWeek = DayOfWeek.Friday, 3, 1))) Then
                                            m_blnThisDateLT2 = True
                                            logApp.WriteEntry("ERR", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, CStr(Session("AppID")), "Fehlerhafte Datumseingabe für VIN " & strFahrgestellNummer & ". Datum weniger als 2 Tage in der Zukunft: " & Request.Form(i - 1).ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
                                        End If
                                    End If

                                    '§§§ JVE 06.09.2006: Prüfen, ob Datum <= Tagesdatum
                                    If dateZulassung <= Today.Date Then
                                        m_blnDateFormatError = True
                                    End If

                                Else
                                    logApp.WriteEntry("ERR", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, CStr(Session("AppID")), "Fehlerhafte Datumseingabe für VIN " & strFahrgestellNummer & ". Formatfehler: " & Request.Form(i - 1).ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
                                End If


                            Case "Verschieben"
                                strZielPDI = Request.Form(i - 1).ToString
                                strBemerkung = Request.Form(i - 3).ToString
                                strBemerkungDatum = Request.Form(i - 2).ToString    '§§§ JVE 03.05.2006: Bemerkung Datum

                                Try
                                    m_blnBemerkungDat = False
                                    datBemerkungDatum = CType(strBemerkungDatum, Date)
                                Catch ex As Exception
                                    m_blnBemerkungDat = True
                                End Try

                            Case "Sperren"
                                strBemerkung = Request.Form(i - 2).ToString
                                strBemerkungDatum = Request.Form(i - 1).ToString    '§§§ JVE 03.05.2006: Bemerkung Datum
                                Try
                                    m_blnBemerkungDat = False
                                    If (strBemerkungDatum.Trim <> String.Empty) Then    '#############
                                        datBemerkungDatum = CType(strBemerkungDatum, Date)
                                    End If
                                Catch ex As Exception
                                    m_blnBemerkungDat = True
                                End Try
                            Case "Entsperren"
                                strBemerkung = Request.Form(i - 2).ToString
                                strBemerkungDatum = Request.Form(i - 1).ToString    '§§§ JVE 03.05.2006: Bemerkung Datum
                                Try
                                    m_blnBemerkungDat = False
                                    If (strBemerkungDatum.Trim <> String.Empty) Then    '############
                                        datBemerkungDatum = CType(strBemerkungDatum, Date)
                                    End If
                                Catch ex As Exception
                                    m_blnBemerkungDat = True
                                End Try
                            Case Else
                                strBemerkung = Request.Form(i - 1).ToString
                        End Select
                        If Not blnThisPDIOK Then
                            m_blnPDIError = True
                        End If
                    End If
                    'If Not (blnAuswahl_alt = blnAuswahl_neu) Then
                    blnReturn = True
                    Dim intModellID As Int32
                    For j = 0 To objPDIs.PDI_Data.Tables("Fahrzeuge").Rows.Count - 1
                        If objPDIs.PDI_Data.Tables("Fahrzeuge").Rows(j)("Fahrgestellnummer").ToString = strFahrgestellNummer Then
                            objPDIs.PDI_Data.Tables("Fahrzeuge").Rows(j)("Ausgewaehlt") = blnAuswahl_neu
                            If Not (strBemerkung.Length = 0) Then
                                objPDIs.PDI_Data.Tables("Fahrzeuge").Rows(j)("Bemerkung") = strBemerkung
                            End If
                            objPDIs.PDI_Data.Tables("Fahrzeuge").Rows(j)("ZielPDI") = strZielPDI
                            If IsDate(strDatumErstzulassung) Then
                                objPDIs.PDI_Data.Tables("Fahrzeuge").Rows(j)("DatumErstzulassung") = CDate(strDatumErstzulassung)
                            End If
                            intModellID = CInt(objPDIs.PDI_Data.Tables("Fahrzeuge").Rows(j)("Modell_ID"))
                            '§§§ JVE 03.05.2006: Bemerkung Datum
                            If (Not m_blnBemerkungDat) AndAlso (Not (strBemerkungDatum.Length = 0)) Then
                                objPDIs.PDI_Data.Tables("Fahrzeuge").Rows(j)("BemerkungDatum") = strBemerkungDatum
                            End If

                        End If
                    Next
                    For j = 0 To objPDIs.PDI_Data.Tables("Modelle").Rows.Count - 1
                        If CInt(objPDIs.PDI_Data.Tables("Modelle").Rows(j)("ID")) = intModellID Then
                            If Not (strBemerkung.Length = 0) Then
                                objPDIs.PDI_Data.Tables("Modelle").Rows(j)("Bemerkung") = strBemerkung
                            End If
                            If strZielPDI.Length > 0 Then
                                objPDIs.PDI_Data.Tables("Modelle").Rows(j)("ZielPDI") = strZielPDI
                            End If
                            If IsDate(strDatumErstzulassung) Then
                                objPDIs.PDI_Data.Tables("Modelle").Rows(j)("DatumErstzulassung") = CDate(strDatumErstzulassung)
                            End If
                            '§§§ JVE 03.05.2006: Bemerkung Datum
                            If (Not m_blnBemerkungDat) AndAlso (Not (strBemerkungDatum.Length = 0)) Then
                                objPDIs.PDI_Data.Tables("Modelle").Rows(j)("BemerkungDatum") = strBemerkungDatum
                            End If

                        End If
                    Next
                    'End If
                End If
            Next
            For j = 0 To objPDIs.PDI_Data.Tables("Modelle").Rows.Count - 1
                Dim vwTemp As DataView = objPDIs.PDI_Data.Tables("Fahrzeuge").DefaultView
                vwTemp.RowFilter = "Ausgewaehlt = True AND Modell_ID = " & objPDIs.PDI_Data.Tables("Modelle").Rows(j)("ID").ToString
                objPDIs.PDI_Data.Tables("Modelle").Rows(j)("Anzahl_neu") = vwTemp.Count
                vwTemp.RowFilter = ""
            Next
            blnReturn = True
        Catch ex As Exception
            logApp.WriteEntry("ERR", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, CStr(Session("AppID")), "Fehler bei der Prüfung der Dateneingaben. (" & ex.Message & ")", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
            lblError.Text = "Fehler bei der Prüfung der Dateneingaben.<br>(" & ex.Message & ")"
            blnReturn = False
        End Try
        logApp.WriteEntry("DBG", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, CStr(Session("AppID")), "CheckInput beendet.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
        Return blnReturn
    End Function

    Private Sub cmdZurueck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdZurueck.Click
        'Session("ShowLink") = "True"
        Response.Redirect("Change06.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub cmdWeitereAuswahl_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdWeitereAuswahl.Click
        lblMessage.Text = ""
        FillGrid(-1)

        cmdWeiter.Visible = True
        cmdWeitereAuswahl.Visible = False
        cmdAbsenden.Visible = False
        cmdVerwerfen.Visible = False
        cmdZurueck.Visible = False
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Change06_2.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 16.02.10   Time: 11:40
' Updated in $/CKAG/Applications/appsixt/Forms
' ITA: 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 4.05.09    Time: 10:16
' Updated in $/CKAG/Applications/appsixt/Forms
' ITA:2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 13:37
' Created in $/CKAG/Applications/appsixt/Forms
' 
' *****************  Version 21  *****************
' User: Fassbenders  Date: 29.10.07   Time: 15:34
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Forms
' 
' *****************  Version 20  *****************
' User: Fassbenders  Date: 11.09.07   Time: 16:32
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Forms
' ITA: 1311
' 
' *****************  Version 19  *****************
' User: Uha          Date: 2.07.07    Time: 14:09
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 18  *****************
' User: Uha          Date: 12.03.07   Time: 17:21
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Forms
' Imagepfad geändert (Customize -> Images)
' 
' *****************  Version 17  *****************
' User: Uha          Date: 8.03.07    Time: 13:15
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Forms
' 
' ************************************************
