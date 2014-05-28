Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business

Partial Public Class Change07_5
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private objSuche As FFE_Search
    Private objAddressList As FFE_Search
    Private objHaendler As FFE_Haendler

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        Kopfdaten1.Message = ""
        lnkFahrzeugAuswahl.NavigateUrl = "Change07_3.aspx?AppID=" & Session("AppID").ToString
        lnkFahrzeugsuche.NavigateUrl = "Change07.aspx?AppID=" & Session("AppID").ToString
        lnkAdressAuswahl.NavigateUrl = "Change07_4.aspx?AppID=" & Session("AppID").ToString

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text

            m_App = New Base.Kernel.Security.App(m_User)

            If Session("objHaendler") Is Nothing Then
                Response.Redirect("Change04.aspx?AppID=" & Session("AppID").ToString)
            End If

            If (Session("objSuche") Is Nothing) Then
                Response.Redirect("Change04.aspx?AppID=" & Session("AppID").ToString)
            Else
                objSuche = CType(Session("objSuche"), FFE_Search)
            End If

            Kopfdaten1.UserReferenz = m_User.Reference
            Kopfdaten1.HaendlerNummer = objSuche.REFERENZ
            Dim strTemp As String = objSuche.NAME
            If objSuche.NAME_2.Length > 0 Then
                strTemp &= "<br>" & objSuche.NAME_2
            End If
            Kopfdaten1.HaendlerName = strTemp
            Kopfdaten1.Adresse = objSuche.COUNTRYISO & " - " & objSuche.POSTL_CODE & " " & objSuche.CITY & "<br>" & objSuche.STREET

            Session("objSuche") = objSuche

            objHaendler = CType(Session("objHaendler"), FFE_Haendler)

            If Not IsPostBack Then
                InitialLoad()
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub InitialLoad()
        Kopfdaten1.Kontingente = objHaendler.Kontingente

        lnkAdressAuswahl.Visible = True
        lnkFahrzeugAuswahl.Visible = True
        FillGrid()
        DataGrid1.Columns(8).Visible = False
        DataGrid1.Columns(9).Visible = False
        lblAddress.Text = Session("SelectedDeliveryText").ToString
        Dim strNullen As String = "00000000000000"
        Select Case CType(Session("Materialnummer"), String)
            Case "1391"
                lblMaterialNummer.Text = strNullen & "1391"
                lblVersandart.Text = "Standard"
            Case "1385"
                lblMaterialNummer.Text = strNullen & "1385"
                lblVersandart.Text = "Express vor 9:00 Uhr (28,20 Euro Netto)"
            Case "1389"
                lblMaterialNummer.Text = strNullen & "1389"
                lblVersandart.Text = "Express vor 10:00 Uhr (23,00 Euro Netto)"
            Case "1390"
                lblMaterialNummer.Text = strNullen & "1390"
                lblVersandart.Text = "Express vor 12:00 Uhr (17,80 Euro Netto)"
        End Select
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)

        objHaendler.StandardLogID = logApp.LogStandardIdentity
        Try
            If Session("SelectedDeliveryValue").ToString.Length = 0 Then
                Response.Redirect(lnkFahrzeugAuswahl.NavigateUrl)
            Else
                Dim tmpDataView As New DataView()
                tmpDataView = objHaendler.Fahrzeuge.DefaultView

                tmpDataView.RowFilter = "MANDT <> '0'"
                objHaendler.Adresse = "60" & Session("SelectedDeliveryValue").ToString

                Dim intItemCounter As Int32
                Dim blnPerformedWithoutError As Boolean = True
                For intItemCounter = 0 To tmpDataView.Count - 1
                    If CStr(tmpDataView.Item(intItemCounter)("COMMENT")).Length = 0 Then
                        objHaendler.ZZFAHRG = tmpDataView.Item(intItemCounter)("ZZFAHRG").ToString
                        objHaendler.KreditkontrollBereich = tmpDataView.Item(intItemCounter)("MANDT").ToString
                        objHaendler.MaterialNummer = lblMaterialNummer.Text
                        objHaendler.KUNNR = m_User.KUNNR
                        objHaendler.Anfordern_Bank()

                        If objHaendler.Status = -1111 Then
                            blnPerformedWithoutError = False
                            lblError.Text = objHaendler.Message
                            Exit For
                        End If

                        Dim sngStart As Single = CSng(Microsoft.VisualBasic.Timer)
                        Dim intStart As Int32 = 0
                        Do While CSng(Microsoft.VisualBasic.Timer) < sngStart + 1
                            intStart += 1
                        Loop

                        tmpDataView.Item(intItemCounter)("VBELN") = objHaendler.Auftragsnummer
                        If objHaendler.Auftragsnummer.Length = 0 Then
                            tmpDataView.Item(intItemCounter)("COMMENT") = "Fehler: " & objHaendler.Message
                        Else
                            tmpDataView.Item(intItemCounter)("COMMENT") = objHaendler.Auftragsstatus
                        End If
                    End If
                Next

                If blnPerformedWithoutError Then
                    DataGrid1.DataSource = tmpDataView
                    DataGrid1.DataBind()
                    DataGrid1.Columns(8).Visible = True
                    DataGrid1.Columns(9).Visible = True
                    Dim iCount As Integer
                    For iCount = 10 To 15
                        DataGrid1.Columns(iCount).Visible = False
                    Next iCount
                    DataGrid1.Columns(16).Visible = True

                    Dim item As DataGridItem
                    Dim cell As TableCell
                    Dim chkBox As CheckBox
                    Dim control As Control
                    Dim strKKB As String

                    For Each item In DataGrid1.Items
                        strKKB = item.Cells(1).Text
                        Select Case strKKB
                            Case "1"
                                item.Cells(16).Text = "Standard temporär"
                            Case "2"
                                item.Cells(16).Text = "Standard endgültig"
                            Case "3"
                                item.Cells(16).Text = "Retail"
                            Case "4"
                                item.Cells(16).Text = "Erweitertes Zahlungsziel (Delayed Payment) endgültig"
                            Case "6"
                                item.Cells(16).Text = "KF/KL"
                        End Select
                        For Each cell In item.Cells
                            For Each control In cell.Controls
                                If TypeOf control Is CheckBox Then
                                    chkBox = CType(control, CheckBox)
                                    Select Case chkBox.ID
                                        Case "chk0001"
                                            If strKKB = "1" Then
                                                chkBox.Checked = True
                                            Else
                                                chkBox.Checked = False
                                            End If
                                        Case "chk0002"
                                            If strKKB = "2" Then
                                                chkBox.Checked = True
                                            Else
                                                chkBox.Checked = False
                                            End If
                                        Case "chk0003"
                                            If strKKB = "3" Then
                                                chkBox.Checked = True
                                            Else
                                                chkBox.Checked = False
                                            End If
                                        Case "chk0004"
                                            If strKKB = "4" Then
                                                chkBox.Checked = True
                                            Else
                                                chkBox.Checked = False
                                            End If
                                        Case "chk0006"
                                            If strKKB = "6" Then
                                                chkBox.Checked = True
                                            Else
                                                chkBox.Checked = False
                                            End If
                                    End Select
                                End If
                            Next
                        Next
                    Next

                    Dim tblTemp As New DataTable()
                    Dim i As Int32

                    tblTemp.Columns.Add("Kundennr.", System.Type.GetType("System.String"))
                    tblTemp.Columns.Add("Händlernr.", System.Type.GetType("System.String"))
                    tblTemp.Columns.Add("Fahrgestellnr.", System.Type.GetType("System.String"))
                    tblTemp.Columns.Add("Finanzierungsnr.", System.Type.GetType("System.String"))

                    For i = 0 To tmpDataView.Count - 1
                        If tmpDataView(i)("MANDT").ToString = "3" Then
                            tblTemp.Columns.Add("Anfragenr.", System.Type.GetType("System.String"))
                            Exit For
                        End If
                    Next i

                    tblTemp.Columns.Add("ZBII-Nummer", System.Type.GetType("System.String"))
                    tblTemp.Columns.Add("Kennzeichen", System.Type.GetType("System.String"))
                    tblTemp.Columns.Add("Ordernr.", System.Type.GetType("System.String"))
                    tblTemp.Columns.Add("Equipmentnr.", System.Type.GetType("System.String"))
                    tblTemp.Columns.Add("Bez.", System.Type.GetType("System.Boolean"))
                    tblTemp.Columns.Add("COC Besch.vorh.", System.Type.GetType("System.Boolean"))
                    tblTemp.Columns.Add("Auftragsnr.", System.Type.GetType("System.String"))
                    tblTemp.Columns.Add("Kommentar", System.Type.GetType("System.String"))
                    tblTemp.Columns.Add("Kontingentart", System.Type.GetType("System.String"))
                    Dim tmpRow As DataRow
                    For i = 0 To tmpDataView.Count - 1
                        tmpRow = tblTemp.NewRow
                        tmpRow("Kundennr.") = objHaendler.KUNNR.TrimStart("0"c)

                        'in dem LogFile soll die Händlernummer ohne vorstehende 60 sein. JJU2008.06.26
                        If objHaendler.Customer.TrimStart("0"c).StartsWith("60") Then
                            tmpRow("Händlernr.") = objHaendler.Customer.TrimStart("0"c).Substring(2, objHaendler.Customer.TrimStart("0"c).Length - 2)
                        Else
                            tmpRow("Händlernr.") = objHaendler.Customer.TrimStart("0"c)
                        End If
                        tmpRow("Fahrgestellnr.") = tmpDataView(i)("ZZFAHRG")
                        tmpRow("Finanzierungsnr.") = tmpDataView(i)("LIZNR")
                        If tmpDataView(i)("MANDT").ToString = "3" Then tmpRow("Anfragenr.") = tmpDataView(i)("TEXT300")
                        tmpRow("ZBII-Nummer") = tmpDataView(i)("TIDNR")
                        tmpRow("Kennzeichen") = tmpDataView(i)("LICENSE_NUM")
                        tmpRow("Ordernr.") = tmpDataView(i)("ZZREFERENZ1")
                        If Not TypeOf tmpDataView(i)("EQUNR") Is System.DBNull Then
                            tmpRow("Equipmentnr.") = CStr(tmpDataView(i)("EQUNR")).TrimStart("0"c)
                        End If
                        tmpRow("Bez.") = tmpDataView(i)("ZZBEZAHLT")
                        tmpRow("COC Besch.vorh.") = tmpDataView(i)("ZZCOCKZ")
                        tmpRow("Auftragsnr.") = tmpDataView(i)("VBELN")
                        tmpRow("Kommentar") = tmpDataView(i)("COMMENT")
                        Select Case tmpDataView(i)("MANDT").ToString
                            Case "1"
                                tmpRow("Kontingentart") = "Standard temporär"
                            Case "2"
                                tmpRow("Kontingentart") = "Standard endgültig"
                            Case "3"
                                tmpRow("Kontingentart") = "Retail"
                            Case "4"
                                tmpRow("Kontingentart") = "Erweitertes Zahlungsziel (Delayed Payment) endgültig"
                            Case "6"
                                tmpRow("Kontingentart") = "KF/KL"
                        End Select

                        tblTemp.Rows.Add(tmpRow)
                    Next
                    logApp.UpdateEntry("APP", Session("AppID").ToString, "Dokumentenanforderung zu Adresse-Nr. " & objHaendler.Adresse & " erfolgreich durchgeführt.", tblTemp)
                    'logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, Right(objHaendler.Customer, 5), "Briefanforderung zu Adresse-Nr. " & objHaendler.Adresse & " erfolgreich durchgeführt.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0, tblTemp)

                    objHaendler = New FFE_Haendler(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "", "60" & objSuche.REFERENZ)

                    objHaendler.Show()

                    Kopfdaten1.Kontingente = objHaendler.Kontingente
                    Session("objHaendler") = objHaendler

                    cmdSave.Visible = False
                    'cmdSave.Text = "Zurück"
                    Session("SelectedDeliveryValue") = ""
                    lnkAdressAuswahl.Visible = False
                    lnkFahrzeugAuswahl.Visible = False
                    lblMessage.Text = "Sie haben folgenden Versandauftrag erstellt:"
                Else
                    InitialLoad()
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Fehler: " & ex.Message
            logApp.UpdateEntry("ERR", Session("AppID").ToString, "Fehler bei der Dokumentenanforderung zu Adresse-Nr. " & objHaendler.Adresse & ", Fehler: " & ex.Message & ")")
            'logApp.WriteEntry("ERR", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, Right(objHaendler.Customer, 5), "Fehler bei der Briefanforderung zu Adresse-Nr. " & objHaendler.Adresse & ", Fehler: " & ex.Message & ")", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 10)
        End Try
    End Sub

    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        DoSubmit()
    End Sub

    Private Sub FillGrid(Optional ByVal strSort As String = "")
        Dim tmpDataView As New DataView()
        tmpDataView = objHaendler.Fahrzeuge.DefaultView

        If Not strSort.Trim(" "c).Length = 0 Then
            Dim strDirection As String
            If ViewState("Direction") Is Nothing Then
                strDirection = "desc"
            Else
                strDirection = ViewState("Direction").ToString
            End If

            If strDirection = "asc" Then
                strDirection = "desc"
            Else
                strDirection = "asc"
            End If

            tmpDataView.Sort = strSort & " " & strDirection
            ViewState("Direction") = strDirection
        End If

        tmpDataView.RowFilter = "MANDT <> '0'"
        DataGrid1.DataSource = tmpDataView
        DataGrid1.DataBind()

        Dim item As DataGridItem
        Dim cell As TableCell
        Dim chkBox As CheckBox
        Dim control As Control
        Dim blnScriptFound As Boolean = False
        Dim intZaehl0001 As Int32 = 0
        Dim intZaehl0002 As Int32 = 0
        Dim intZaehl0003 As Int32 = 0
        Dim intZaehl0004 As Int32 = 0
        Dim intZaehl0006 As Int32 = 0
        Dim blnBezahlt As Boolean
        Dim strKKB As String
        Dim blnGesperrteAnforderungen As Boolean = False

        For Each item In DataGrid1.Items
            blnBezahlt = False
            strKKB = item.Cells(1).Text
            For Each cell In item.Cells
                For Each control In cell.Controls
                    If TypeOf control Is CheckBox Then
                        chkBox = CType(control, CheckBox)
                        If chkBox.ID = "Bezahlt" And chkBox.Checked Then
                            blnBezahlt = True
                        End If
                    End If
                Next
                For Each control In cell.Controls
                    If TypeOf control Is CheckBox Then
                        chkBox = CType(control, CheckBox)
                        If Not chkBox.ID = "Bezahlt" Then
                            Select Case chkBox.ID
                                Case "chk0001"
                                    If strKKB = "1" Then
                                        chkBox.Checked = True
                                        If Not blnBezahlt Then
                                            intZaehl0001 += 1
                                        End If
                                    Else
                                        chkBox.Checked = False
                                    End If
                                Case "chk0002"
                                    If strKKB = "2" Then
                                        chkBox.Checked = True
                                        If Not blnBezahlt Then
                                            intZaehl0002 += 1
                                        End If
                                    Else
                                        chkBox.Checked = False
                                    End If
                                Case "chk0003"
                                    If strKKB = "3" Then
                                        chkBox.Checked = True
                                        'If Not blnBezahlt Then
                                        intZaehl0003 += 1
                                        'End If
                                    Else
                                        chkBox.Checked = False
                                    End If
                                Case "chk0004"
                                    If strKKB = "4" Then
                                        chkBox.Checked = True
                                        'If Not blnBezahlt Then
                                        intZaehl0004 += 1
                                        'End If
                                    Else
                                        chkBox.Checked = False
                                    End If
                                Case "chk0006"
                                    If strKKB = "6" Then
                                        chkBox.Checked = True
                                        intZaehl0006 += 1
                                    Else
                                        chkBox.Checked = False
                                    End If
                            End Select
                        End If
                    End If
                Next
            Next
        Next

        'SFA 12.12.2008 Es werden teilweise nur 2 Zeilen geliefert!
        If objHaendler.Kontingente.Rows.Count > 2 Then
            If CInt(objHaendler.Kontingente.Rows(2)("Richtwert_Alt")) = 0 Then ' Retailspalte ausblenden 
                DataGrid1.Columns(DataGrid1.Columns.Count - 4).Visible = False
            End If
        End If
        If objHaendler.Kontingente.Rows.Count > 3 Then
            If CInt(objHaendler.Kontingente.Rows(3)("Richtwert_Alt")) = 0 Then ' DPSpalte ausblenden 
                DataGrid1.Columns(DataGrid1.Columns.Count - 5).Visible = False
            End If
        End If

        If objHaendler.Kontingente.Rows.Count > 4 Then
            If CInt(objHaendler.Kontingente.Rows(4)("Richtwert_Alt")) = 0 Then ' KFKLSpalte ausblenden 
                DataGrid1.Columns(DataGrid1.Columns.Count - 3).Visible = False
            End If
        End If


        lblMessage.Text = ""
        Dim rowTest As DataRow
        For Each rowTest In objHaendler.Kontingente.Rows
            Dim strTemp As String = ""
            Dim intTemp As Int32 = 0
            Select Case rowTest("Kreditkontrollbereich").ToString
                Case "0001"
                    intTemp = CInt(rowTest("Ausschoepfung")) + intZaehl0001 - CInt(rowTest("Kontingent_Alt"))
                    If intTemp > 0 And intZaehl0001 > 0 Then
                        strTemp = "<u>Kontingentart - Standard temporär</u>: Überzählige Dokumentenanforderungen werden separat freigegeben. (Ihre Anforderung von " & intZaehl0001.ToString & " Fahrzeug(en) übersteigt Ihr Kontingent um " & intTemp.ToString & " Fahrzeug(e).)<br>"
                        blnGesperrteAnforderungen = True
                    End If
                    If CType(rowTest("Gesperrt_Alt"), System.Boolean) And intZaehl0001 > 0 Then
                        strTemp = "<u>Kontingentart - Standard temporär</u>: Die Dokumentenanforderungen werden separat freigegeben. (Ihr Kontingent ist zur Zeit nicht freigegeben.)<br>Für die Freigabe der Aufträge setzen Sie sich bitte mit Ihrem zuständigen Ansprechpartner in Verbindung."
                        blnGesperrteAnforderungen = True
                    End If
                Case "0002"
                    intTemp = CInt(rowTest("Ausschoepfung")) + intZaehl0002 - CInt(rowTest("Kontingent_Alt"))
                    If intTemp > 0 And intZaehl0002 > 0 Then
                        strTemp = "<u>Kontingentart - Standard endgültig</u>: Überzählige Dokumentenanforderungen werden separat freigegeben (Ihre Anforderung von " & intZaehl0002.ToString & " unbezahlten Fahrzeug(en) übersteigt Ihr Kontingent um " & intTemp.ToString & " Fahrzeug(e).)<br>"
                        blnGesperrteAnforderungen = True
                    End If
                    If CType(rowTest("Gesperrt_Alt"), System.Boolean) And intZaehl0002 > 0 Then
                        strTemp = "<u>Kontingentart - Standard endgültig</u>: Die Dokumentenanforderungen werden separat freigegeben. (Ihr Kontingent ist zur Zeit nicht freigegeben.)<br>Für die Freigabe der Aufträge setzen Sie sich bitte mit Ihrem zuständigen Ansprechpartner in Verbindung."
                        blnGesperrteAnforderungen = True
                    End If
            End Select
            'lblMessage.Text &= strTemp --- sollte erstmal ausgeblendet sein
        Next
        lblMessage.Text = lblMessage.Text & "<br>Zur Aktivierung der ausgewählten Vorgänge klicken Sie auf [Absenden].<br>"
        'lblMessage.CssClass = "LabelExtraLargeBlue"
        If intZaehl0003 > 0 Then
            lblMessage.Text &= "<u>Kontingentart - Retail</u>: Die Dokumentenanforderungen werden separat freigegeben.<br>Für die Freigabe der Aufträge setzen Sie sich bitte mit Ihrem zuständigen Ansprechpartner in Verbindung."
            blnGesperrteAnforderungen = True
        End If
        If intZaehl0004 > 0 Then
            lblMessage.Text &= "<br><u>Kontingentart - Erweitertes Zahlungsziel(Delayed Payment) endgültig</u>: Die Dokumentenanforderungen werden separat freigegeben.<br>Für die Freigabe der Aufträge setzen Sie sich bitte mit Ihrem zuständigen Ansprechpartner in Verbindung."
            blnGesperrteAnforderungen = True
        End If
        If intZaehl0006 > 0 Then
            lblMessage.Text &= "<br><u>Kontingentart - KF/KL </u>: Die Dokumentenanforderungen werden separat freigegeben.<br>Für die Freigabe der Aufträge setzen Sie sich bitte mit Ihrem zuständigen Ansprechpartner in Verbindung."
            blnGesperrteAnforderungen = True
        End If
        'If Not lblMessage.Text.Length = 0 Then
        '    lblMessage.Text &= "<hr>"
        'End If
        If blnGesperrteAnforderungen Then
            lblVersandhinweis.Visible = True
        Else
            lblVersandhinweis.Visible = False
        End If
        DataGrid1.PagerStyle.Visible = False
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        FillGrid(e.SortExpression)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class
' ************************************************
' $History: Change07_5.aspx.vb $
' 
' *****************  Version 12  *****************
' User: Fassbenders  Date: 12.12.08   Time: 13:29
' Updated in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 8.09.08    Time: 9:12
' Updated in $/CKAG/Applications/AppFFE/forms
' doppeltes hinzufgen der spalte Anfragenr bei briefanforderung
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 9.07.08    Time: 15:07
' Updated in $/CKAG/Applications/AppFFE/forms
' ITA 2036
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 10.06.08   Time: 17:24
' Updated in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 6.06.08    Time: 9:21
' Updated in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 4.06.08    Time: 18:43
' Updated in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 23.05.08   Time: 16:21
' Updated in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 21.05.08   Time: 16:34
' Updated in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 16.05.08   Time: 15:54
' Updated in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 15.05.08   Time: 14:33
' Updated in $/CKAG/Applications/AppFFE/forms
' ITA:1865
' 
' ************************************************
