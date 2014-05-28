Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business

Public Class Change18_2
    Inherits System.Web.UI.Page

    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private objSuche As Search
    Private objFDDBank As BankBaseCredit
    Private objFDDBank2 As FDD_Bank_2

    Protected WithEvents Kopfdaten1 As Kopfdaten
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkKreditlimit As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblAnzeige As System.Web.UI.WebControls.Label
    Protected WithEvents rbStandard As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rbFlottengeschaeft As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rbHEZ As System.Web.UI.WebControls.RadioButton
    Protected WithEvents ddlKontingentart As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents trKopfdaten As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trVorgangsArt As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trDataGrid1 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    Protected WithEvents Tr1 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents ShowScript As System.Web.UI.HtmlControls.HtmlTableRow

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

    'grpVorgaenge (Gruppierung der Radiobuttons für Kontingentart): Mehrfachauswahl nicht möglich!
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cmdSave.Enabled = False
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        Try
            lnkKreditlimit.NavigateUrl = "Change18.aspx?AppID=" & Session("AppID").ToString
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text

            m_App = New Base.Kernel.Security.App(m_User)
            Dim DistrictCount As Integer = Session("DistrictCount")
            If DistrictCount > 0 Then
                lnkKreditlimit.Text = "Distriktsuche"
                lnkKreditlimit.Visible = True
            Else
                If m_User.Organization.AllOrganizations Then
                    lnkKreditlimit.Visible = True
                Else
                    lnkKreditlimit.Visible = False
                End If
            End If


            If (Request.QueryString("ShowAll") = "True") And (Not IsPostBack) Then
                Session("SelectedDealer") = Nothing
            End If

            If (Session("objSuche") Is Nothing) OrElse _
                CType(Session("objSuche"), Search).HaendlerFiliale.Length = 0 Then
                'Keine Filialinformation vorhanden = Abbruch
                Response.Redirect("Change18.aspx?AppID=" & Session("AppID").ToString)
            Else
                'Filialinformation vorhanden
                objSuche = CType(Session("objSuche"), Search)

                If Session("SelectedDealer") Is Nothing Then
                    'Noch kein Händler ausgewählt
                    ' => Auswahltabelle
                    trKopfdaten.Visible = False
                    trVorgangsArt.Visible = False
                    'trPageSize.Visible = True
                    trDataGrid1.Visible = True
                    cmdSave.Visible = False

                    If (Not IsPostBack) Or (Session("objFDDBank2") Is Nothing) Then
                        'Daten aus SAP laden
                        objFDDBank2 = New FDD_Bank_2(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                        objFDDBank2.AppID = Session("AppID").ToString
                        objFDDBank2.CreditControlArea = "ZDAD"
                        objFDDBank2.Filiale = objSuche.HaendlerFiliale
                        objFDDBank2.Customer = m_User.KUNNR
                        objFDDBank2.Show_Retail(Session("AppID").ToString, Session.SessionID, Me)
                    Else
                        objFDDBank2 = CType(Session("objFDDBank2"), FDD_Bank_2)
                    End If

                    If Not IsPostBack Then
                        ddlPageSize.Items.Add("10")
                        ddlPageSize.Items.Add("20")
                        ddlPageSize.Items.Add("50")
                        ddlPageSize.Items.Add("100")
                        ddlPageSize.Items.Add("200")
                        ddlPageSize.Items.Add("500")
                        ddlPageSize.Items.Add("1000")
                        ddlPageSize.SelectedIndex = 2

                        Select Case objFDDBank2.Status
                            Case 0
                                FillGrid(objFDDBank2, 0)
                                Session("objFDDBank2") = objFDDBank2
                            Case -9999
                                'trPageSize.Visible = False
                                trDataGrid1.Visible = False
                                lblError.Text = "Fehler bei der Ermittlung der gesperrten Aufträge.<br>(" & objFDDBank2.Message & ")"
                            Case Else
                                'trPageSize.Visible = False
                                trDataGrid1.Visible = False
                                lblError.Text = objFDDBank2.Message
                        End Select
                    End If
                Else
                    trKopfdaten.Visible = True
                    trVorgangsArt.Visible = True        'Hier wird die Zeile der Vorgangsarten eingeblendet!
                    'trPageSize.Visible = False
                    trDataGrid1.Visible = False
                    cmdSave.Visible = True

                    objFDDBank = CType(Session("objFDDBank"), BankBaseCredit)
                    If objFDDBank.Status = 0 Then
                        Kopfdaten1.Kontingente = objFDDBank.Kontingente
                        cmdSave.Enabled = True
                        Session("objFDDBank") = objFDDBank
                    Else
                        ddlPageSize.Visible = False
                        lblError.Text = objFDDBank.Message
                    End If

                    If Not IsPostBack Then
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
                    End If

                    objFDDBank2 = CType(Session("objFDDBank2"), FDD_Bank_2)
                    Dim vwTemp As DataView = objFDDBank2.AuftragsUebersicht.DefaultView
                    vwTemp.RowFilter = "Händlernummer = '" & Session("SelectedDealer").ToString & "'"
                    Dim item As ListItem
                    Dim str As String
                    str = String.Empty

                    If (objFDDBank2.ZeigeAlle) Then                 '?????
                        item = New ListItem()
                        item.Value = "0"
                        item.Text = ""
                        ddlKontingentart.Items.Add(item)
                    End If
                    If (objFDDBank2.ZeigeStandard) Then
                        item = New ListItem()
                        item.Value = "1"
                        item.Text = CStr(CInt(vwTemp(0)(2)) + CInt(vwTemp(0)(3))) & " Vorgänge 'Standard'"
                        str &= "<u><b>" & CStr(CInt(vwTemp(0)(2)) + CInt(vwTemp(0)(3))) & "</b></u> Vorgänge 'Standard'<br>"
                        ddlKontingentart.Items.Add(item)
                    End If
                    If (objFDDBank2.ZeigeFlottengeschaeft) Then
                        item = New ListItem()
                        item.Value = "2"
                        item.Text = CStr(vwTemp(0)(4)) & " Vorgänge 'erweitertes Zahlungsziel' (Delayed Payment)"
                        str &= "<u><b>" & CStr(vwTemp(0)(4)) & "</b></u> Vorgänge 'Erweitertes Zahlungsziel' (Delayed Payment)<br>"
                        ddlKontingentart.Items.Add(item)
                    End If
                    If (objFDDBank2.ZeigeHEZ) Then
                        item = New ListItem()
                        item.Value = "3"
                        item.Text = CStr(vwTemp(0)(5)) & " Vorgänge 'HEZ'" 'HEZ
                        str &= "<u><b>" & CStr(vwTemp(0)(5)) & "</b></u> Vorgänge 'HEZ'<br><br>"
                        ddlKontingentart.Items.Add(item)
                    End If
                    lblAnzeige.Text = str
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim ddlVal As String

        objFDDBank2.ZeigeAlle = False
        objFDDBank2.ZeigeFlottengeschaeft = False
        objFDDBank2.ZeigeHEZ = False
        objFDDBank2.ZeigeStandard = False

        ddlVal = ddlKontingentart.SelectedItem.Value
        If (ddlVal = "0") Then 'Alle
            '??? Was soll hier passieren?
        End If
        If (ddlVal = "1") Then 'Standard
            objFDDBank2.ZeigeStandard = True 'rbStandard.Checked  'HEZ
        End If
        If (ddlVal = "2") Then 'Flottengeschäft
            objFDDBank2.ZeigeFlottengeschaeft = True
        End If
        If (ddlVal = "3") Then 'HEZ
            objFDDBank2.ZeigeHEZ = True
        End If

        objFDDBank2.Haendler = Session("SelectedDealer").ToString

        Session("objFDDBank2") = objFDDBank2
        Response.Redirect("Change02Edit.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub FillGrid(ByVal objBank As FDD_Bank_2, ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If objBank.Status = 0 Then
            If objBank.AuftragsUebersicht.Rows.Count = 0 Then
                trDataGrid1.Visible = False
                'trPageSize.Visible = False
                lblNoData.Visible = True
                lblNoData.Text = "Keine Daten zur Anzeige gefunden."
                ShowScript.Visible = False
            Else
                trDataGrid1.Visible = True
                'trPageSize.Visible = True
                lblNoData.Visible = False

                Dim tmpDataView As New DataView()
                tmpDataView = objBank.AuftragsUebersicht.DefaultView

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

                lblNoData.Text = "Es wurden " & objBank.AuftraegeAlle.Rows.Count.ToString & " gesperrte Aufträge gefunden."
                lblNoData.Visible = True

                If DataGrid1.PageCount > 1 Then
                    DataGrid1.PagerStyle.CssClass = "PagerStyle"
                    DataGrid1.DataBind()
                    DataGrid1.PagerStyle.Visible = True
                Else
                    DataGrid1.PagerStyle.Visible = False
                End If

            End If
        Else
            lblError.Text = objBank.Message
            lblNoData.Visible = True
            ShowScript.Visible = False
        End If
    End Sub

    Private Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.ItemCommand
        If Not e.Item.Cells(1).Text.Length = 0 Then
            Dim strRedirectURL As String = "Change18_2.aspx?AppID=" & Session("AppID").ToString
            Session("SelectedDealer") = e.Item.Cells(1).Text
            objFDDBank = New BankBaseCredit(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "", True)
            objFDDBank.CreditControlArea = "ZDAD"
            objFDDBank.Customer = Session("SelectedDealer").ToString
            objFDDBank.Show()
            Session("objFDDBank") = objFDDBank

            'Nur Retail enthält Daten. Direkt nach Change18Edit.aspx gehen!
            objFDDBank2.Haendler = Session("SelectedDealer").ToString
            Session("objFDDBank2") = objFDDBank2
            strRedirectURL = "Change18Edit.aspx?AppID=" & Session("AppID").ToString
            Response.Redirect(strRedirectURL)
        End If
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        FillGrid(objFDDBank2, e.NewPageIndex)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(objFDDBank2, DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
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
' $History: Change18_2.aspx.vb $
' 
' *****************  Version 6  *****************
' User: Fassbenders  Date: 23.03.10   Time: 16:34
' Updated in $/CKAG/Applications/appffd/Forms
' Rückgängig: Dynproxy-Zugriff
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 11.03.10   Time: 12:36
' Updated in $/CKAG/Applications/appffd/Forms
' ITA: 2918
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 10.03.10   Time: 15:22
' Updated in $/CKAG/Applications/appffd/Forms
' ITA: 2918
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
' *****************  Version 3  *****************
' User: Uha          Date: 2.07.07    Time: 12:52
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 19.06.07   Time: 14:39
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' 
' ************************************************
