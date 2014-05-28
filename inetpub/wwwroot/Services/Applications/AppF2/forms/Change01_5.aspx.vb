Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Business

Partial Public Class Change01_5
    Inherits System.Web.UI.Page
#Region "Declarations"
    Private m_App As App
    Private m_User As User
    Private mstrHDL As String
    Private mstrREFEFERNZ1 As String
    Private objSuche As AppF2.Search
    Private objHaendler As Haendler
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)
        GridNavigation1.setGridElment(GridView1)

        m_App = New App(m_User)
        Kopfdaten1.Message = ""
        lnkFahrzeugAuswahl.NavigateUrl = "Change01_2.aspx?AppID=" & Session("AppID").ToString
        lnkFahrzeugsuche.NavigateUrl = "Change01.aspx?AppID=" & Session("AppID").ToString
        lnkAdressAuswahl.NavigateUrl = "Change01_3.aspx?AppID=" & Session("AppID").ToString
        lnkAbrufgrund.NavigateUrl = "Change01_4.aspx?AppID=" & Session("AppID").ToString
        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString


        m_App = New App(m_User)
        mstrHDL = ""
        If Request.QueryString("HDL") = 1 Then
            mstrHDL = "&HDL=1"
        End If

        If Session("AppHaendler") Is Nothing Then
            Response.Redirect("Change42.aspx?AppID=" & Session("AppID").ToString)
        End If

        If (Session("objSuche") Is Nothing) Then
            Response.Redirect("Change42.aspx?AppID=" & Session("AppID").ToString)
        Else
            objSuche = CType(Session("objSuche"), AppF2.Search)
        End If

        Kopfdaten1.UserReferenz = m_User.Reference
        Kopfdaten1.HaendlerNummer = objSuche.REFERENZ
        Dim strTemp As String = objSuche.NAME
        If objSuche.NAME_2.Length > 0 Then
            strTemp &= "<br>" & objSuche.NAME_2
        End If
        Kopfdaten1.HaendlerName = strTemp
        Kopfdaten1.Adresse = objSuche.STREET & "<br>" & objSuche.COUNTRYISO & " - " & objSuche.POSTL_CODE & " " & objSuche.CITY
        Session("objSuche") = objSuche

        objHaendler = CType(Session("AppHaendler"), Haendler)

        If Not IsPostBack Then
            InitialLoad()
        End If

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
        HelpProcedures.FixedGridViewCols(GridView1)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub GridNavigation1_ddlPageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillGrid(0)
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal pageindex As Int32) Handles GridNavigation1.PagerChanged
       FillGrid(pageindex)
    End Sub

    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        DoSubmit()
    End Sub

    Protected Sub cmdSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSave.Click
        DoSubmit()
    End Sub

    Private Sub GridView1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
        FillGrid(0, e.SortExpression)
    End Sub

#End Region


#Region "Methods"
    Private Sub InitialLoad()

        Kopfdaten1.Kontingente = objHaendler.Kontingente

        lnkAdressAuswahl.Visible = True
        lnkFahrzeugAuswahl.Visible = True
        FillGrid(0)
        lblAddress.Text = Session("SelectedDeliveryText").ToString
        Dim strNullen As String = "00000000000000"
        Select Case CType(Session("Materialnummer"), String)
            Case "1391"
                lblMaterialNummer.Text = strNullen & "1391"
                lblVersandart.Text = "Standard"
            Case "1385"
                lblMaterialNummer.Text = strNullen & "1385"
                lblVersandart.Text = objHaendler.VersandArtText
                'weitere Versandarten hinzufügen! JJU2008.03.05
            Case "1389"
                lblMaterialNummer.Text = strNullen & "1389"
                lblVersandart.Text = objHaendler.VersandArtText
            Case "1390"
                lblMaterialNummer.Text = strNullen & "1390"
                lblVersandart.Text = objHaendler.VersandArtText
            Case "5530"
                lblMaterialNummer.Text = strNullen & "5530"
                lblVersandart.Text = objHaendler.VersandArtText
            Case Else
                Throw New Exception("Versandart unbekannt: " & Session("Materialnummer").ToString)
        End Select
    End Sub
    Private Sub DoSubmit()
        Dim logApp As New CKG.Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
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

                objHaendler.Adresse = Session("SelectedDeliveryValue").ToString

                Dim intItemCounter As Int32
                Dim blnPerformedWithoutError As Boolean = True
                For intItemCounter = 0 To tmpDataView.Count - 1
                    If CStr(tmpDataView.Item(intItemCounter)("COMMENT")).Length = 0 Then
                        objHaendler.ZZFAHRG = tmpDataView.Item(intItemCounter)("ZZFAHRG").ToString
                        objHaendler.KreditkontrollBereich = tmpDataView.Item(intItemCounter)("MANDT").ToString
                        objHaendler.MaterialNummer = lblMaterialNummer.Text
                        objHaendler.KUNNR = m_User.KUNNR
                        '********************
                        '   ANFORDERN
                        '********************
                        objHaendler.Anfordern(Session("AppID").ToString, Session.SessionID, Me)

                        If objHaendler.Status = -1111 Then
                            blnPerformedWithoutError = False
                            lblError.Text = objHaendler.Message
                            Exit For
                        End If


                        tmpDataView.Item(intItemCounter)("VBELN") = objHaendler.Auftragsnummer
                        If objHaendler.Auftragsnummer.Length = 0 Then
                            tmpDataView.Item(intItemCounter)("COMMENT") = "Fehler: " & objHaendler.Message
                            lblError.Text = "Vorgang mit Fehlern abgeschlossen."
                        ElseIf CType(Session("AppShowNot"), Boolean) = True Then
                            tmpDataView.Item(intItemCounter)("COMMENT") = objHaendler.Auftragsstatus2
                        Else
                            tmpDataView.Item(intItemCounter)("COMMENT") = objHaendler.Auftragsstatus
                        End If
                    End If
                Next

                If blnPerformedWithoutError Then
                    'Dim tmpTable As DataTable
                    'Dim AppURL As String = Replace(Request.Url.LocalPath, "/Services", "..")
                    'tmpTable = Session(AppURL)
                    'Dim RowColname As DataRow

                    'For Each RowColname In tmpTable.Rows
                    '    If RowColname("ControlID") = "col_Kontonummer" Then
                    '        mstrREFEFERNZ1 = RowColname("Content").ToString
                    '    End If
                    'Next

                    Session("Fahrzeuge") = tmpDataView


                    GridView1.DataSource = tmpDataView
                    GridView1.DataBind()

                    Dim tblTemp As New DataTable()
                    Dim i As Int32

                    tblTemp.Columns.Add("Kundennr.", System.Type.GetType("System.String"))
                    tblTemp.Columns.Add("Händlernr.", System.Type.GetType("System.String"))
                    tblTemp.Columns.Add("Fahrgestellnr.", System.Type.GetType("System.String"))
                    tblTemp.Columns.Add("Vertragsnr.", System.Type.GetType("System.String"))
                    tblTemp.Columns.Add("Nummer ZBII", System.Type.GetType("System.String"))
                    tblTemp.Columns.Add("Kennzeichen", System.Type.GetType("System.String"))
                    'tblTemp.Columns.Add(mstrREFEFERNZ1, System.Type.GetType("System.String"))
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
                        tmpRow("Nummer ZBII") = tmpDataView(i)("TIDNR")
                        tmpRow("Kennzeichen") = tmpDataView(i)("LICENSE_NUM")
                        'tmpRow(mstrREFEFERNZ1) = tmpDataView(i)("ZZREFERENZ1")
                        tmpRow("Bez.") = tmpDataView(i)("ZZBEZAHLT")
                        tmpRow("COC Besch.vorh.") = tmpDataView(i)("ZZCOCKZ")
                        tmpRow("Auftragsnr.") = tmpDataView(i)("VBELN")
                        tmpRow("Kommentar") = tmpDataView(i)("COMMENT")
                        Select Case tmpDataView(i)("MANDT").ToString
                            Case "1"
                                tmpRow("Kontingentart") = "Standard temporär"
                            Case "2"
                                tmpRow("Kontingentart") = "Standard endgültig"
                        End Select

                        tblTemp.Rows.Add(tmpRow)
                    Next
                    logApp.UpdateEntry("APP", Session("AppID").ToString, "Briefanforderung zu Adresse-Nr. " & objHaendler.Adresse & " erfolgreich durchgeführt.", tblTemp)

                    objHaendler = New Haendler(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "", objSuche.REFERENZ, m_User.KUNNR)

                    objHaendler.Show(Session("AppID").ToString, Session.SessionID, Me)

                    Kopfdaten1.Kontingente = objHaendler.Kontingente

                    Session("AppHaendler") = objHaendler

                    cmdSave.Visible = False
                    Session("SelectedDeliveryValue") = ""
                    lnkAdressAuswahl.Visible = False
                    lnkFahrzeugAuswahl.Visible = False
                Else
                    InitialLoad()
                End If
                GridView1.Columns(GridView1.Columns.Count - 2).Visible = True
                GridView1.Columns(GridView1.Columns.Count - 1).Visible = True
            End If
        Catch ex As Exception
            lblError.Text = "Fehler: " & ex.Message
            logApp.UpdateEntry("ERR", Session("AppID").ToString, "Fehler bei der Briefanforderung zu Adresse-Nr. " & objHaendler.Adresse & ", Fehler: " & ex.Message & ")")

        End Try
    End Sub
    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        Dim tmpDataView As New DataView()

        If objHaendler.Fahrzeuge Is Nothing Then
            tmpDataView = CType(Session("Fahrzeuge"), DataView)
        Else
            tmpDataView = objHaendler.Fahrzeuge.DefaultView
        End If



        'If Not strSort.Trim(" "c).Length = 0 Then
        '    Dim strDirection As String
        '    If ViewState("Direction") Is Nothing Then
        '        strDirection = "desc"
        '    Else
        '        strDirection = ViewState("Direction").ToString
        '    End If

        '    If strDirection = "asc" Then
        '        strDirection = "desc"
        '    Else
        '        strDirection = "asc"
        '    End If

        '    tmpDataView.Sort = strSort & " " & strDirection
        '    ViewState("Direction") = strDirection
        'End If

        Dim intTempPageIndex As Int32 = intPageIndex
        Dim strTempSort As String = ""
        Dim strDirection As String = ""

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

        GridView1.PageIndex = intTempPageIndex

        tmpDataView.RowFilter = "MANDT <> '0'"
        GridView1.DataSource = tmpDataView
        GridView1.DataBind()

        Dim item As GridViewRow
        Dim cell As TableCell
        Dim chkBox As CheckBox
        'Dim lbl As Label
        Dim control As Control
        Dim blnScriptFound As Boolean = False
        Dim intZaehl0001 As Int32 = 0
        Dim intZaehl0002 As Int32 = 0
        Dim blnBezahlt As Boolean
        Dim strKKB As String
        Dim blnGesperrteAnforderungen As Boolean = False
        'col_Vertragsnummer
        For Each item In GridView1.Rows
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
    End Sub
#End Region

End Class