Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common

Partial Public Class Change04_4
    Inherits System.Web.UI.Page
    Private m_User As Security.User
    Private m_App As Security.App
    Private objSuche As Search
    Private objAddressList As Search
    Private objHaendler As Porsche_05

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        FormAuth(Me, m_User)

        Kopfdaten1.Message = ""
        lnkFahrzeugAuswahl.NavigateUrl = "Change04_2.aspx?AppID=" & Session("AppID").ToString
        lnkFahrzeugsuche.NavigateUrl = "Change04.aspx?AppID=" & Session("AppID").ToString
        lnkAdressAuswahl.NavigateUrl = "Change04_3.aspx?AppID=" & Session("AppID").ToString
        GridNavigation1.setGridElment(DataGrid1)
        GetAppIDFromQueryString(Me)
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

            m_App = New Security.App(m_User)

            If Session("objHaendler") Is Nothing Then
                Response.Redirect("Change04.aspx?AppID=" & Session("AppID").ToString)
            End If

            If (Session("objSuche") Is Nothing) Then
                Response.Redirect("Change04.aspx?AppID=" & Session("AppID").ToString)
            Else
                objSuche = CType(Session("objSuche"), Search)
            End If

            Kopfdaten1.UserReferenz = m_User.Reference
            Kopfdaten1.HaendlerNummer = objSuche.REFERENZ
            Dim strTemp As String = objSuche.NAME
            If objSuche.NAME_2.Length > 0 Then
                strTemp &= "<br />" & objSuche.NAME_2
            End If
            Kopfdaten1.HaendlerName = strTemp
            Kopfdaten1.Adresse = objSuche.COUNTRYISO & " - " & objSuche.POSTL_CODE & " " & objSuche.CITY & "<br />" & objSuche.STREET

            Session("objSuche") = objSuche

            objHaendler = CType(Session("objHaendler"), Porsche_05)

            If Not IsPostBack Then
                InitialLoad()
            End If

        Catch ex As Exception
            lblError.Visible = True
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
                lblVersandart.Text = "Express (vor 9:00 Uhr) (28,20 Euro Netto)"
                'Case "1389"
                '    lblMaterialNummer.Text = strNullen & "1389"
                '    lblVersandart.Text = "vor 10:00 Uhr (23,00 Euro Netto)"
                'Case "1390"
                '    lblMaterialNummer.Text = strNullen & "1390"
                '    lblVersandart.Text = "vor 12:00 Uhr (17,80 Euro Netto)"
        End Select
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        Dim logApp As New Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)

        lblMessage.Text = String.Empty
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
                        objHaendler.Anfordern()

                        If objHaendler.Status = -1111 Then
                            blnPerformedWithoutError = False
                            lblError.Visible = True
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
                    DataGrid1.Columns(10).Visible = False
                    DataGrid1.Columns(11).Visible = False
                    DataGrid1.Columns(12).Visible = False
                    DataGrid1.Columns(13).Visible = True

                    Dim item As DataGridItem
                    Dim cell As TableCell
                    Dim chkBox As CheckBox
                    Dim control As Control
                    Dim strKKB As String

                    For Each item In DataGrid1.Items
                        strKKB = item.Cells(1).Text
                        Select Case strKKB
                            Case "1"
                                item.Cells(13).Text = "Standard temporär"
                            Case "2"
                                item.Cells(13).Text = "Standard endgültig"
                            Case "3"
                                item.Cells(13).Text = "Delayed Payments endgültig"
                            Case "4"
                                item.Cells(13).Text = "Erweitertes Zahlungsziel (Delayed Payment) endgültig"
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
                                            'Case "chk0003"
                                            '    If strKKB = "3" Then
                                            '        chkBox.Checked = True
                                            '    Else
                                            '        chkBox.Checked = False
                                            '    End If
                                        Case "chk0004"
                                            If strKKB = "4" Then
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
                    tblTemp.Columns.Add("Vertragsnr.", System.Type.GetType("System.String"))
                    tblTemp.Columns.Add("Briefnr.", System.Type.GetType("System.String"))
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
                        tmpRow("Händlernr.") = objHaendler.Customer.TrimStart("0"c)
                        tmpRow("Fahrgestellnr.") = tmpDataView(i)("ZZFAHRG")
                        tmpRow("Vertragsnr.") = tmpDataView(i)("LIZNR")
                        tmpRow("Briefnr.") = tmpDataView(i)("TIDNR")
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
                                'Case "3"
                                '    tmpRow(tblTemp.Columns.Count - 1) = "Delayed Payment endgültig"
                            Case "4"
                                tmpRow("Kontingentart") = "Erweitertes Zahlungsziel (Delayed Payment) endgültig"
                        End Select

                        tblTemp.Rows.Add(tmpRow)
                    Next
                    logApp.UpdateEntry("APP", Session("AppID").ToString, "Briefanforderung zu Adresse-Nr. " & objHaendler.Adresse & " erfolgreich durchgeführt.", tblTemp)
                    'logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, Right(objHaendler.Customer, 5), "Briefanforderung zu Adresse-Nr. " & objHaendler.Adresse & " erfolgreich durchgeführt.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0, tblTemp)

                    objHaendler = New Porsche_05(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "", "60" & objSuche.REFERENZ)

                    objHaendler.ShowStandard()

                    Kopfdaten1.Kontingente = objHaendler.Kontingente
                    Session("objHaendler") = objHaendler

                    cmdSave.Visible = False
                    'cmdSave.Text = "Zurück"
                    Session("SelectedDeliveryValue") = ""
                    lnkAdressAuswahl.Visible = False
                    lnkFahrzeugAuswahl.Visible = False
                Else
                    InitialLoad()
                End If
            End If
        Catch ex As Exception
            lblError.Visible = True
            lblError.Text = "Fehler: " & ex.Message
            logApp.UpdateEntry("ERR", Session("AppID").ToString, "Fehler bei der Briefanforderung zu Adresse-Nr. " & objHaendler.Adresse & ", Fehler: " & ex.Message & ")")
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
        'Dim intZaehl0003 As Int32 = 0
        'Dim intZaehl0004 As Int32 = 0
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
                            End Select
                        End If
                    End If
                Next
            Next
        Next

        lblMessage.Text = ""
        Dim rowTest As DataRow
        Dim strTemp As String = ""
        Dim intTemp As Int32 = 0
        lblTemp.Text = 0
        lblEnd.Text = 0

        For Each rowTest In objHaendler.Kontingente.Rows

            Select Case rowTest("Kreditkontrollbereich").ToString
                Case "0001"
                    intTemp = CInt(rowTest("Ausschoepfung")) + intZaehl0001 - CInt(rowTest("Kontingent_Alt"))
                    If intTemp > 0 And intZaehl0001 > 0 Then
                        lblTemp.Text = intTemp.ToString
                        blnGesperrteAnforderungen = True
                    End If
                Case "0002"
                    intTemp = CInt(rowTest("Ausschoepfung")) + intZaehl0002 - CInt(rowTest("Kontingent_Alt"))
                    If intTemp > 0 And intZaehl0002 > 0 Then
                        lblEnd.Text = intTemp.ToString
                        blnGesperrteAnforderungen = True
                    End If

            End Select
            tblMessage.Visible = blnGesperrteAnforderungen
        Next
        lblMessage.Text = lblMessage.Text & "<br>Zur Aktivierung der ausgewählten Vorgänge klicken Sie auf [Absenden].<br>"
 
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
    Private Sub GridNavigation1_ddlPageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillGrid(0)
    End Sub
    Private Sub DataGrid1_PageIndexChanged(ByVal pageindex As Int32) Handles GridNavigation1.PagerChanged
        DataGrid1.EditItemIndex = -1
        FillGrid(pageindex)
    End Sub
End Class