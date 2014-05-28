Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class change51_1
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

    'Private m_context As HttpContext = HttpContext.Current
    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private objSuche As CKG.Components.ComCommon.Finance.Search
    Private objBank As CKG.Components.ComCommon.BankBaseCredit
    Private m_change As fin_13

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents Kopfdaten1 As CKG.Components.ComCommon.PageElements.Kopfdaten
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lbExcel As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ShowScript As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lb_Haendlerauswahl As System.Web.UI.WebControls.LinkButton
    Protected WithEvents label1 As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles
    Protected WithEvents timerHidePopup As Global.System.Web.UI.Timer
    Protected WithEvents btnFake As Global.System.Web.UI.WebControls.Button
    Protected WithEvents ModalPopupExtender1 As Global.AjaxControlToolkit.ModalPopupExtender
    Protected WithEvents mb As Global.System.Web.UI.WebControls.Panel
    Protected WithEvents lbl_AuftragStornoErfolgreichMessage As Global.System.Web.UI.WebControls.Label
    Protected WithEvents lblStornoDetailsFahrgestellnummer As Global.System.Web.UI.WebControls.Label

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User, True)

        GetAppIDFromQueryString(Me)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text
        m_App = New Base.Kernel.Security.App(m_User)

        lblError.Text = ""

        If IsPostBack = False Then

            'objekt wiederherstellen
            m_change = CType(Session("m_change"), fin_13)
            objSuche = CType(Session("objSuche"), Finance.Search)


            If m_change.Haendler Is Nothing OrElse m_change.Haendler.Trim Is String.Empty Then
                'wenn kein konkreter Händler angegeben worden ist, alle offenen Anforderungen anzeigen
                'somit entfällt das füllen der Kopfdaten
                Kopfdaten1.Visible = False

            Else
                'wenn ein Konkreter Händler ausgwählt, dann kopfdaten und Kontingente füllen
                Kopfdaten1.Visible = True


                If Not objSuche.LeseHaendlerSAP_Einzeln(Session("AppID").ToString, Session.SessionID.ToString, m_change.Haendler) Then
                    lblError.Text = "Fehler bei der Ermittlung der Händlerdaten.<br>(" & objSuche.ErrorMessage & ")"
                Else
                    Session("objSuche") = objSuche
                End If



                'Kopfdatenfüllen
                Kopfdaten1.UserReferenz = m_User.Reference
                Kopfdaten1.HaendlerNummer = m_change.Haendler
                Dim strTemp As String = objSuche.NAME
                If objSuche.NAME_2.Length > 0 Then
                    strTemp &= "<br>" & objSuche.NAME_2
                End If
                Kopfdaten1.HaendlerName = strTemp
                Kopfdaten1.Adresse = objSuche.COUNTRYISO & " - " & objSuche.POSTL_CODE & " " & objSuche.CITY & "<br>" & objSuche.STREET

                'bankObjekt für Kontingente instanziieren 
                objBank = New BankBaseCredit(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                objBank.Customer = objSuche.REFERENZ
                objBank.KUNNR = m_User.KUNNR
                objBank.CreditControlArea = "ZDAD"
                objBank.Show(Session("AppID").ToString, Session.SessionID) 'kontingentetabelle füllen
                Session("objBank") = objBank
                Kopfdaten1.Kontingente = objBank.Kontingente 'kontingente anzeigen

            End If

            'Resultatstabelle füllen/objekt in Session schreiben und Excel erzeugen/Grid füllen
            m_change.show(Request.QueryString.Item("HEZ"), Session("AppID").ToString, Session.SessionID)
            If m_change.Status = 0 Then
                Session("m_change") = m_change
                FillGrid(0)
            Else
                lblError.Text = m_change.Message
            End If

            'dropDownListe füllen
            ddlPageSize.Items.Add("10")
            ddlPageSize.Items.Add("20")
            ddlPageSize.Items.Add("50")
            ddlPageSize.Items.Add("100")
            ddlPageSize.Items.Add("200")
            ddlPageSize.Items.Add("500")
            ddlPageSize.Items.Add("1000")
            ddlPageSize.SelectedIndex = 2
            If Request.QueryString("HDL") = 1 Then
                Session("AppShowNot") = True
            End If
        Else 'wenn postback
            If m_change Is Nothing Then
                m_change = CType(Session("m_change"), fin_13)
            End If

            If objBank Is Nothing Then
                objBank = CType(Session("objBank"), BankBaseCredit)
            End If

        End If

    End Sub
    
    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If m_change.Status = 0 Then

            If IsNothing(m_change.Auftraege) OrElse m_change.Auftraege.Rows.Count = 0 Then
                lbExcel.Visible = False

                lblNoData.Text = "Keine Daten zur Anzeige gefunden."
                DataGrid1.Visible = False
                lblNoData.Visible = True
            Else
                lbExcel.Visible = True

                DataGrid1.Visible = True
                lblNoData.Visible = False

                Dim tmpDataView As DataView = m_change.Auftraege.DefaultView

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
                'Anzahl der gesperrten Aufträge ermitteln
                Dim view As New DataView(tmpDataView.Table)
                view.RowFilter = "Gesperrt <> ''"
                '----------------------------------------
                lblNoData.Text = "Es wurde(n) " & tmpDataView.Count.ToString & " offene Anforderung(en) gefunden"
                lblNoData.Visible = True

                If DataGrid1.PageCount > 1 Then
                    DataGrid1.PagerStyle.CssClass = "PagerStyle"
                    DataGrid1.DataBind()
                    DataGrid1.PagerStyle.Visible = True
                Else
                    DataGrid1.PagerStyle.Visible = False
                End If

                Dim item As DataGridItem
                Dim cell As TableCell
                Dim button As LinkButton
                Dim control As Control
                Dim blnScriptFound As Boolean = False
                Dim tmpLabel As Label
                Dim strAngefordertAm As String = ""
                Dim strFahrgestellnummer As String = ""
                Dim strBriefnummer As String = ""
                Dim strKontingentart As String = ""

                For Each item In DataGrid1.Items


                    control = item.Cells(3).Controls(1)
                    Dim label = TryCast(control, Label)
                    If (label IsNot Nothing) Then
                        tmpLabel = label
                        strAngefordertAm = Replace(tmpLabel.Text, "&nbsp;", "")
                    End If


                    control = item.Cells(4).Controls(1)
                    Dim control1 = TryCast(control, Label)
                    If (control1 IsNot Nothing) Then
                        tmpLabel = control1
                        strFahrgestellnummer = Replace(tmpLabel.Text, "&nbsp;", "")
                    End If

                    control = item.Cells(5).Controls(1)
                    Dim control2 = TryCast(control, Label)
                    If (control2 IsNot Nothing) Then
                        tmpLabel = control2
                        strBriefnummer = Replace(tmpLabel.Text, "&nbsp;", "")
                    End If

                    control = item.Cells(6).Controls(1)
                    Dim label2 = TryCast(control, Label)
                    If (label2 IsNot Nothing) Then
                        tmpLabel = label2
                        strKontingentart = Replace(tmpLabel.Text, "&nbsp;", "")
                    End If


                    cell = item.Cells(8)
                    For Each control In cell.Controls
                        Dim linkButton = TryCast(control, LinkButton)
                        If (linkButton IsNot Nothing) Then
                            button = linkButton
                            If button.CommandName = "Storno" Then
                                button.Attributes.Add("onClick", "if (!StornoConfirm('" & strAngefordertAm & "','" & strFahrgestellnummer & "','" & strBriefnummer & "','" & strKontingentart & "')) return false;")
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
            lblError.Text = m_change.Message
            lblNoData.Visible = True
        End If
    End Sub

    Private Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.ItemCommand

        If e.CommandName = "Storno" Then
            If m_change Is Nothing Then
                m_change = CType(Session("m_change"), fin_13)
            End If


            m_change.EQUNR = m_change.Auftraege.Select("EQUNR='" & e.CommandArgument & "'")(0).Item("EQUNR")
            m_change.VBELN = m_change.Auftraege.Select("EQUNR='" & e.CommandArgument & "'")(0).Item("VBELN")
            m_change.StornoHaendler = m_change.Auftraege.Select("EQUNR='" & e.CommandArgument & "'")(0).Item("Haendlernummer")
            m_change.Fahrgestellnummer = m_change.Auftraege.Select("EQUNR='" & e.CommandArgument & "'")(0).Item("Fahrgestellnummer")
            m_change.Change(Session("AppID").ToString, Session.SessionID)

            If Not m_change.Status = 0 Then
                lblError.Text = m_change.Message
                Exit Sub
            Else
                fakeTheGrid(e.CommandArgument, "X", m_change.Fahrgestellnummer)
            End If


            'Kontingente aktuallisieren, kann immer geschehen, wurde ja zu  anfangs visible=false/true gesetzt wenn kein spezieller Händler
            If objBank Is Nothing Then
                If Session.Item("objBank") Is Nothing Then

                    objBank = New BankBaseCredit(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                    objBank.Customer = m_change.Haendler 'muss ja jetzt einen haben sonst ginge das storno nicht, 
                    objBank.KUNNR = m_User.KUNNR
                    objBank.CreditControlArea = "ZDAD"
                    objBank.Show(Session("AppID").ToString, Session.SessionID) 'kontingentetabelle füllen
                    Session("objBank") = objBank
                Else
                    objBank = CType(Session.Item("objBank"), BankBaseCredit)
                    objBank.Show(Session("AppID").ToString, Session.SessionID) 'kontingentetabelle füllen
                End If
            End If
            objBank.Show(Session("AppID").ToString, Session.SessionID) 'kontingentetabelle füllen
            Kopfdaten1.Kontingente = objBank.Kontingente 'kontingente anzeigen
        End If
    End Sub

    Private Sub fakeTheGrid(ByVal EQUNR As String, ByVal storno As String, ByVal fahrgestellnummer As String)

        m_change.Auftraege.Select("EQUNR='" & EQUNR & "'")(0).Delete()
        m_change.Auftraege.AcceptChanges()
        FillGrid(DataGrid1.CurrentPageIndex)

        If storno = "X" Then
            lblStornoDetailsFahrgestellnummer.Text = fahrgestellnummer
            ModalPopupExtender1.Show()
            timerHidePopup.Enabled = True
        End If
        label1.Visible = True
    End Sub
    
    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(0)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub lbExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbExcel.Click

        Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
        excelFactory.CreateDocumentAndSendAsResponse(strFileName, m_change.ResultExcel, Me.Page)
       
    End Sub

    Private Sub lb_Haendlerauswahl_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_Haendlerauswahl.Click
        Response.Redirect("Change51.aspx?AppID=" & Page.Session("AppID"))
    End Sub

    Protected Sub timerHidePopup_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles timerHidePopup.Tick
        timerHidePopup.Enabled = False
        ModalPopupExtender1.Hide()
    End Sub
End Class

' ************************************************
' $History: Change51_1.aspx.vb $
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 23.06.09   Time: 9:13
' Updated in $/CKAG/Components/ComCommon/Finance
' ita 2918 Z_M_OFFENEANFOR_STORNO_001, Z_M_OFFENEANFOR_001
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 22.06.09   Time: 13:18
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2918 Z_M_Creditlimit_Detail_001
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 19.06.09   Time: 10:10
' Updated in $/CKAG/Components/ComCommon/Finance
' ITa 2918 testfertig
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 5.06.09    Time: 15:58
' Updated in $/CKAG/Components/ComCommon/Finance
' mögliche try catches entfernt
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 4.05.09    Time: 11:50
' Updated in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 28.04.08   Time: 14:18
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 1811
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 24.04.08   Time: 8:39
' Updated in $/CKAG/Components/ComCommon/Finance
' Migration AKF-Entwicklungen
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 2.04.08    Time: 8:34
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 13.03.08   Time: 10:42
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' AKF Anpassungen Offene Anforderungen Bank/offene Anforderungen Händler
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 5.03.08    Time: 18:08
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' AKF Änderungen 1733
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 28.02.08   Time: 13:01
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ita 1733
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 27.02.08   Time: 15:33
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1733
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 26.02.08   Time: 16:59
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1733
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 26.02.08   Time: 8:50
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ita 1733
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 25.02.08   Time: 15:51
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ita 1733
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 25.02.08   Time: 15:05
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ita 1733
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 25.02.08   Time: 13:52
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ita 1733, offene Anforderungen Bank hinzugefügt
' ************************************************
