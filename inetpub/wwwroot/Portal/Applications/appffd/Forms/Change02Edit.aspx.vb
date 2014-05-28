Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business

Imports System
Imports System.IO
Imports System.Collections
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization


Public Class Change02Edit
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

    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents Kopfdaten1 As Kopfdaten
    Protected WithEvents ShowScript As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkKreditlimit As System.Web.UI.WebControls.HyperLink
    Protected WithEvents ucHeader As Header
    Protected WithEvents lnkVertragssuche As System.Web.UI.WebControls.HyperLink
    Protected WithEvents Literal1 As System.Web.UI.WebControls.Literal
    Protected WithEvents lblLegende As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lnkExcel As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblDownloadTip As System.Web.UI.WebControls.Label
    Protected WithEvents txtFaelligkeit As System.Web.UI.WebControls.TextBox
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents HyperLink1 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkDistrikt As System.Web.UI.WebControls.HyperLink
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cmdSave.Enabled = False
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        Try
            If (Session("objSuche") Is Nothing) Then
                Response.Redirect("Change02.aspx?AppID=" & Session("AppID").ToString)
            End If
            If (Session("SelectedDealer").ToString.Length = 0) Or (Session("objFDDBank") Is Nothing) Or (Session("objFDDBank2") Is Nothing) Then
                Response.Redirect("Change02_2.aspx?AppID=" & Session("AppID").ToString)
            End If

            objSuche = CType(Session("objSuche"), Search)
            If objSuche.LeseHaendlerSAP_Einzeln(Session("AppID").ToString, Session.SessionID.ToString, Session("SelectedDealer").ToString) Then
                Kopfdaten1.HaendlerNummer = objSuche.REFERENZ
                Dim strTemp As String = objSuche.NAME
                If objSuche.NAME_2.Length > 0 Then
                    strTemp &= "<br>" & objSuche.NAME_2
                End If
                Kopfdaten1.HaendlerName = strTemp
                Kopfdaten1.Adresse = objSuche.COUNTRYISO & " - " & objSuche.POSTL_CODE & " " & objSuche.CITY & "<br>" & objSuche.STREET
            Else
                lblError.Text = "Fehler bei der Ermittlung der Händlerdaten.<br>(" & objSuche.ErrorMessage & ")"
            End If

            lnkVertragssuche.NavigateUrl = "Change02_2.aspx?AppID=" & Session("AppID").ToString & "&ShowAll=True"
            lnkKreditlimit.NavigateUrl = "Change02.aspx?AppID=" & Session("AppID").ToString
            lnkDistrikt.NavigateUrl = "Change02.aspx?AppID=" & Session("AppID").ToString
            If m_User.Organization.AllOrganizations Then
                lnkKreditlimit.Visible = True
            Else
                lnkKreditlimit.Visible = False
            End If

            Dim Districtcount As Integer = Session("DistrictCount")
            If Districtcount > 0 Then
                lnkDistrikt.Visible = True
            End If
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text

            m_App = New Base.Kernel.Security.App(m_User)

            objFDDBank = CType(Session("objFDDBank"), BankBaseCredit)
            objFDDBank2 = CType(Session("objFDDBank2"), FDD_Bank_2)
            If objFDDBank.Status = 0 And objFDDBank2.Status = 0 Then
                If Not IsPostBack Then
                    ddlPageSize.Items.Add("10")
                    ddlPageSize.Items.Add("20")
                    ddlPageSize.Items.Add("50")
                    ddlPageSize.Items.Add("100")
                    ddlPageSize.Items.Add("200")
                    ddlPageSize.Items.Add("500")
                    ddlPageSize.Items.Add("1000")
                    ddlPageSize.SelectedIndex = 2

                    Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
                    Dim tblTemp As DataTable = objFDDBank2.Auftraege.Copy
                    Dim tmpRows As DataRow()

                    tmpRows = objFDDBank2.Auftraege.Select("KontingentCode = '0003'")
                    If tmpRows.Length > 0 Then
                        Dim i2 As Integer

                        For i2 = 0 To tmpRows.Length - 1
                            Dim iLoeschFlag As Integer = 0
                            tmpRows(i2).BeginEdit()
                            If tmpRows(i2).Item("KontingentCode").ToString = "0003" Then
                                iLoeschFlag = 1
                            End If
                            If iLoeschFlag = 1 Then
                                tmpRows(i2).Delete()
                            End If
                            tmpRows(i2).EndEdit()
                            tblTemp.AcceptChanges()
                        Next i2
                    End If
                    tblTemp.Columns.Remove("InAutorisierung")
                    tblTemp.Columns.Remove("Initiator")
                    tblTemp.Columns.Remove("KontingentCode")
                    Try
                        Excel.ExcelExport.WriteExcel(tblTemp, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                        lnkExcel.NavigateUrl = "/Portal/Temp/Excel/" & strFileName
                        lnkExcel.Visible = True
                        lblDownloadTip.Visible = True
                    Catch
                    End Try

                    FillGrid(objFDDBank2, 0)
                    If objFDDBank2.ZeigeFlottengeschaeft Then
                        'tdFaelligkeitAlle.Visible = True
                        DataGrid1.Columns(DataGrid1.Columns.Count - 2).Visible = True
                        DataGrid1.Columns(DataGrid1.Columns.Count - 3).Visible = True
                    Else
                        'tdFaelligkeitAlle.Visible = False
                        DataGrid1.Columns(DataGrid1.Columns.Count - 2).Visible = False
                        DataGrid1.Columns(DataGrid1.Columns.Count - 3).Visible = False
                    End If
                    '§§§ JVE 14.07.2005 <begin>
                    If objFDDBank2.ZeigeHEZ Then 'Bei HEZ Spalte für Zulassungsart und Legende einblenden!
                        lblLegende.Visible = True
                        DataGrid1.Columns(DataGrid1.Columns.Count - 4).Visible = True
                    End If
                    '§§§ JVE 14.07.2005 <end>
                    Kopfdaten1.Kontingente = objFDDBank.Kontingente
                Else
                    If Request.Form("txtStorno").ToString = "X" Or Request.Form("txtFreigeben").ToString = "X" Then
                        Literal1.Text = "						<script language=""Javascript"">" & vbCrLf
                        Literal1.Text &= "						  <!-- //" & vbCrLf
                        Literal1.Text &= "						    window.document.location.href = ""#" & CStr(Request.Form("txtAuftragsNummer")) & """;" & vbCrLf
                        Literal1.Text &= "						  //-->" & vbCrLf
                        Literal1.Text &= "						</script>" & vbCrLf

                        objFDDBank2 = CType(Session("objFDDBank2"), FDD_Bank_2)
                        objFDDBank2.AuftragsNummer = Request.Form("txtAuftragsNummer").ToString
                        If Request.Form("txtStorno").ToString = "X" Then
                            objFDDBank2.Storno = True
                        Else
                            objFDDBank2.Storno = False
                        End If

                        Dim strTemp As String = ""
                        Dim strTemp2 As String = ""

                        Dim j As Int32
                        Dim strAuftragsnummer As String = Request.Form("txtAuftragsNummer").ToString
                        For j = 0 To Request.Form.Keys.Count - 1
                            If InStr(Request.Form.Keys(j).ToString, "txt" & Request.Form("txtAuftragsNummer").ToString) > 0 Then
                                strTemp = Request.Form(j).ToString
                                If (Not objFDDBank2.Storno) AndAlso (objFDDBank2.Auftraege.Select("Auftragsnummer=" & strAuftragsnummer)(0)("KontingentCode").ToString = "0004") AndAlso (strTemp.Length = 0) Then
                                    FillGrid(objFDDBank2, DataGrid1.CurrentPageIndex)
                                    Throw New System.Exception("Das Feld ""Kunde"" muss für diese Kontingentart gefüllt werden.")
                                End If
                                objFDDBank2.Kunde = strTemp
                                objFDDBank2.Auftraege.Select("Auftragsnummer=" & strAuftragsnummer)(0)("Kunde") = strTemp
                            End If

                            If InStr(Request.Form.Keys(j).ToString, "dat" & Request.Form("txtAuftragsNummer").ToString) > 0 Then
                                strTemp2 = Request.Form(j).ToString
                                If (strTemp2.Trim(" "c).Length > 0 Or txtFaelligkeit.Text.Trim(" "c).Length > 0) Then
                                    If (Not objFDDBank2.Storno) AndAlso (objFDDBank2.Auftraege.Select("Auftragsnummer=" & strAuftragsnummer)(0)("KontingentCode").ToString = "0004") AndAlso (Not (IsDate(strTemp2) Or IsDate(txtFaelligkeit.Text))) Then
                                        FillGrid(objFDDBank2, DataGrid1.CurrentPageIndex)
                                        Throw New System.Exception("Das Feld ""Fälligkeit"" muss mit einem Datum im Format ""TT.MM.JJJJ"" gefüllt werden.")
                                    End If
                                    If IsDate(strTemp2) Then
                                        objFDDBank2.Faelligkeit = strTemp2
                                    Else
                                        If IsDate(txtFaelligkeit.Text) Then
                                            objFDDBank2.Faelligkeit = txtFaelligkeit.Text
                                            strTemp2 = txtFaelligkeit.Text
                                        End If
                                    End If
                                    objFDDBank2.Auftraege.Select("Auftragsnummer=" & strAuftragsnummer)(0)("Faelligkeit") = CDate(strTemp2)
                                End If
                            End If
                            '§§§JVE 14.07.2005 <begin>: Zulassungsart setzen
                            'If (Request.Form.Keys(j).ToString = "txtZulassungsart") Then
                            '    objFDDBank2.Zulassungsart = Request.Form(j).ToString
                            'End If
                            '§§§JVE 14.07.2005 <end>
                        Next



                        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
                        logApp.CollectDetails("Kundennummer", CType(objFDDBank.Customer.TrimStart("0"c), Object), True)
                        logApp.CollectDetails("Vertragsnummer", CType(Request.Form("txtVertragsnummer").ToString, Object))
                        logApp.CollectDetails("Angefordert", CType(Request.Form("txtAngefordert").ToString, Object))
                        logApp.CollectDetails("Fahrgestellnummer", CType(Request.Form("txtFahrgestellnummer").ToString, Object))
                        logApp.CollectDetails("Briefnummer", CType(Request.Form("txtBriefnummer").ToString, Object))
                        logApp.CollectDetails("Storno", CType(objFDDBank2.Storno, Object))
                        logApp.CollectDetails("Freigabe", CType((Not objFDDBank2.Storno), Object))
                        logApp.CollectDetails("Kunde", CType(strTemp, Object))
                        logApp.CollectDetails("Fälligkeit", CType(strTemp2, Object))

                        If (CInt(m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AuthorizationLevel")) > 0) Then
                            'Anwendung erfordert Autorisierung (Level>0)
                            Dim DetailArray(3, 2) As Object
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

                            ms = New MemoryStream()
                            formatter = New BinaryFormatter()
                            formatter.Serialize(ms, objFDDBank)
                            b = ms.ToArray
                            ms = New MemoryStream(b)
                            DetailArray(1, 0) = ms
                            DetailArray(1, 1) = "objFDDBank"

                            ms = New MemoryStream()
                            formatter = New BinaryFormatter()
                            formatter.Serialize(ms, objFDDBank2)
                            b = ms.ToArray
                            ms = New MemoryStream(b)
                            DetailArray(2, 0) = ms
                            DetailArray(2, 1) = "objFDDBank2"

                            'Pruefen, ob schon in der Autorisierung.
                            Dim strInitiator As String = ""
                            Dim intAuthorizationID As Int32
                            Dim iDistrictID As Integer
                            If Not Session("SelectedDistrict") = Nothing Then
                                iDistrictID = Session("SelectedDistrict")
                            Else
                                iDistrictID = m_User.Organization.OrganizationId
                            End If
                            m_App.CheckForPendingAuthorization(CInt(Session("AppID")), iDistrictID, objSuche.REFERENZ, Request.Form("txtFahrgestellnummer").ToString, m_User.IsTestUser, strInitiator, intAuthorizationID)
                            If Not strInitiator.Length = 0 Then
                                'Fahrzeug wurde schon mal freigegeben und liegt zur Autorisierung vor
                                lblError.Text = "Diese Briefanforderung wurde schon einmal freigegeben und liegt bereits zur Autorisierung vor!"
                                Exit Sub
                            End If

                            intAuthorizationID = WriteAuthorization(m_App.Connectionstring, CInt(Session("AppID")), m_User.UserName, iDistrictID, objSuche.REFERENZ, Request.Form("txtFahrgestellnummer").ToString, Request.Form("txtZulassungsart").ToString.Trim, "", m_User.IsTestUser, DetailArray)
                            objFDDBank2.Auftraege.Select("Auftragsnummer=" & strAuftragsnummer)(0)("InAutorisierung") = True
                            objFDDBank2.Auftraege.Select("Auftragsnummer=" & strAuftragsnummer)(0)("Initiator") = m_User.UserName

                            logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, Right(Session("SelectedDealer").ToString, 5), "Brieffreigabe für Händler " & objSuche.REFERENZ & " erfolgreich initiiert.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0, logApp.InputDetails)
                        Else
                            'Anwendung erfordert keine Autorisierung (Level=0)

                            objFDDBank2.Change(Session("AppID").ToString, Session.SessionID, Me)
                            If Not objFDDBank2.Status = 0 Then
                                lblError.Text = objFDDBank2.Message
                                logApp.WriteEntry("ERR", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, Right(Session("SelectedDealer").ToString, 5), "Fehler bei der Brieffreigabe für Händler " & objSuche.REFERENZ & " , (Fehler: " & objFDDBank2.Message & ")", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 10, logApp.InputDetails)
                            Else
                                logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, Right(Session("SelectedDealer").ToString, 5), "Brieffreigabe für Händler " & objSuche.REFERENZ & " erfolgreich durchgeführt.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0, logApp.InputDetails)
                            End If

                            logApp.WriteStandardDataAccessSAP(objFDDBank2.IDSAP)
                            objFDDBank2.Show(Session("AppID").ToString, Session.SessionID, Me)
                        End If

                        Session("objFDDBank2") = objFDDBank2

                        FillGrid(objFDDBank2, DataGrid1.CurrentPageIndex)

                        objFDDBank.Show()
                        If objFDDBank.Status = 0 Then
                            Kopfdaten1.Kontingente = objFDDBank.Kontingente

                            cmdSave.Enabled = True
                        Else
                            lblError.Text = objFDDBank.Message
                        End If

                    End If
                End If
                cmdSave.Enabled = True
            Else
                lblError.Text = objFDDBank.Message & "<br>" & objFDDBank2.Message
            End If
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Private Sub FillGrid(ByVal objBank As FDD_Bank_2, ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If objBank.Status = 0 Then
            If objBank.Auftraege.Rows.Count = 0 Then
                DataGrid1.Visible = False
                lblNoData.Visible = True
                lblNoData.Text = "Keine Daten zur Anzeige gefunden."
                ShowScript.Visible = False
            Else
                DataGrid1.Visible = True
                lblNoData.Visible = False
                ddlPageSize.Visible = True

                Dim tmpDataView As New DataView()
                tmpDataView = objBank.Auftraege.DefaultView

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

                DataGrid1.CurrentPageIndex = intTempPageIndex

                DataGrid1.DataSource = tmpDataView
                DataGrid1.DataBind()

                lblNoData.Text = "Es wurden " & tmpDataView.Count.ToString & " gesperrte Aufträge gefunden."
                lblNoData.Visible = True

                If DataGrid1.PageCount > 1 Then
                    DataGrid1.PagerStyle.CssClass = "PagerStyle"
                    DataGrid1.DataBind()
                    DataGrid1.PagerStyle.Visible = True
                Else
                    DataGrid1.PagerStyle.Visible = False
                End If

                'Code zum Generieren des Button-Scriptes
                Dim item As DataGridItem
                Dim cell As TableCell
                Dim button As LinkButton
                Dim textbox As TextBox
                Dim control As Control
                Dim blnScriptFound As Boolean = False
                Dim intZaehl As Int32

                For Each item In DataGrid1.Items
                    cell = item.Cells(0)
                    Dim strAuftragsnummer As String
                    strAuftragsnummer = cell.Text

                    Dim strParameter As String = ""
                    strParameter = "'" & strAuftragsnummer & "',"

                    For intZaehl = 1 To 4
                        cell = item.Cells(intZaehl)
                        If cell.Text = "&nbsp;" Then
                            strParameter &= "'',"
                        Else
                            strParameter &= "'" & cell.Text & "',"
                        End If
                    Next

                    cell = item.Cells(8)
                    For Each control In cell.Controls
                        If TypeOf control Is TextBox Then
                            textbox = CType(control, TextBox)
                            textbox.ID = "dat" & strAuftragsnummer
                        End If
                    Next

                    cell = item.Cells(9)
                    For Each control In cell.Controls
                        If TypeOf control Is TextBox Then
                            textbox = CType(control, TextBox)
                            textbox.ID = "txt" & strAuftragsnummer
                        End If
                    Next
                    '§§§JVE 14.07.2005 <begin>: Zulassungsart als zusätztlicher Parameter für das Skript...
                    cell = item.Cells(7)
                    If cell.Text = "N" Then
                        strParameter &= "'3'"
                    End If
                    If cell.Text = "S" Then
                        strParameter &= "'2'"
                    End If
                    If cell.Text = "V" Then
                        strParameter &= "'4'"
                    End If
                    '§§§JVE 14.07.2005 <begin>
                    cell = item.Cells(10)
                    For Each control In cell.Controls
                        If TypeOf control Is LinkButton Then
                            button = CType(control, LinkButton)
                            If button.CommandName = "Storno" Then
                                ' =>    Auf Kundenwunsch abgeschaltet
                                'button.Attributes.Add("onClick", "if (!StornoConfirm(" & strParameter.Trim(","c) & ")) return false;")
                                button.Attributes.Add("onClick", "DoStorno(" & strParameter.Trim(","c) & ");")
                                button.Attributes.Add("class", "StandardButtonTable")
                                blnScriptFound = True
                            End If
                            If button.CommandName = "Freigabe" Then
                                ' =>    Auf Kundenwunsch abgeschaltet
                                'button.Attributes.Add("onClick", "if (!FreigebenConfirm(" & strParameter.Trim(","c) & ")) return false;")
                                button.Attributes.Add("onClick", "DoFreigeben(" & strParameter.Trim(","c) & ");")
                                button.Attributes.Add("class", "StandardButtonTable")
                                blnScriptFound = True
                            End If
                        End If
                    Next
                Next
                If blnScriptFound Then
                    ShowScript.Visible = True
                End If
            End If
        Else
            lblError.Text = objBank.Message
            lblNoData.Visible = True
            ShowScript.Visible = False
        End If
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        FillGrid(objFDDBank2, e.NewPageIndex)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(objFDDBank2, DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(objFDDBank2, 0)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Change02Edit.aspx.vb $
' 
' *****************  Version 7  *****************
' User: Fassbenders  Date: 23.03.10   Time: 16:34
' Updated in $/CKAG/Applications/appffd/Forms
' Rückgängig: Dynproxy-Zugriff
' 
' *****************  Version 6  *****************
' User: Fassbenders  Date: 11.03.10   Time: 12:36
' Updated in $/CKAG/Applications/appffd/Forms
' ITA: 2918
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 10.03.10   Time: 15:22
' Updated in $/CKAG/Applications/appffd/Forms
' ITA: 2918
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 15.06.09   Time: 17:08
' Updated in $/CKAG/Applications/appffd/Forms
' ITA 2918
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 12.06.09   Time: 15:23
' Updated in $/CKAG/Applications/appffd/Forms
' ITA 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 30.04.09   Time: 14:43
' Updated in $/CKAG/Applications/appffd/Forms
' ITA: 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:23
' Created in $/CKAG/Applications/appffd/Forms
' 
' *****************  Version 13  *****************
' User: Uha          Date: 2.07.07    Time: 12:52
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 12  *****************
' User: Rudolpho     Date: 14.06.07   Time: 14:40
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' Abgleich Portal - Startapplication 14.06.2007
' 
' *****************  Version 11  *****************
' User: Rudolpho     Date: 8.06.07    Time: 15:36
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' Abgleich Beyond Compare
' 
' *****************  Version 10  *****************
' User: Fassbenders  Date: 8.06.07    Time: 11:26
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' 
' *****************  Version 9  *****************
' User: Uha          Date: 3.05.07    Time: 18:05
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' Änderungen aus StartApplication vom 02.05.2007 Mittags übernommen
' 
' ************************************************
