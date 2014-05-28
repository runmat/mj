Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business


Public Class Change41_1
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
    Private objBank As BankBaseCredit
    Private m_Change As fin_05

    Protected WithEvents DataGrid1 As DataGrid
    Protected WithEvents ShowScript As HtmlTableRow
    Protected WithEvents trVorgangsArt As HtmlTableRow
    Protected WithEvents trPageSize As HtmlTableRow
    Protected WithEvents trDataGrid1 As HtmlTableRow
    Protected WithEvents lblHead As Label
    Protected WithEvents lblPageTitle As Label
    Protected WithEvents lb_Haendlersuche As LinkButton
    Protected WithEvents lblNoData As Label
    Protected WithEvents ddlPageSize As DropDownList
    Protected WithEvents lblError As Label
    Protected WithEvents ucStyles As Styles
    Protected WithEvents ucHeader As Header
   


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text
        m_App = New Base.Kernel.Security.App(m_User)
        

        If Not IsPostBack Then
            ddlPageSize.Items.Add("10")
            ddlPageSize.Items.Add("20")
            ddlPageSize.Items.Add("50")
            ddlPageSize.Items.Add("100")
            ddlPageSize.Items.Add("200")
            ddlPageSize.Items.Add("500")
            ddlPageSize.Items.Add("1000")
            ddlPageSize.SelectedIndex = 2

            If m_Change Is Nothing Then
                m_Change = CType(Session("m_change"), fin_05)
            End If

            If Request.QueryString("Linked") = 2 Then
                If m_Change Is Nothing Then
                    m_Change = New fin_05(m_User, m_App, CStr(Session("AppID")), Session.SessionID, "")
                End If
                automaticFoward()
            End If


            m_Change.Show(Session("AppID").ToString, Session.SessionID)
            Session.Add("m_Change", m_Change)
            
            FillGrid(m_Change, 0)
        Else
            If m_Change Is Nothing Then
                m_Change = CType(Session("m_change"), fin_05)
            End If
        End If

    End Sub

    Private Sub FillGrid(ByVal objBank As fin_05, ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If Not objBank.AuftragsUebersicht Is Nothing Then
            If objBank.Status = 0 Then
                If objBank.AuftragsUebersicht.Rows.Count = 0 Then
                    trDataGrid1.Visible = False
                    trPageSize.Visible = False
                    lblNoData.Visible = True
                    lblNoData.Text = "Keine Daten zur Anzeige gefunden."
                    ShowScript.Visible = False
                Else
                    trDataGrid1.Visible = True
                    trPageSize.Visible = True
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
        Else
            lblNoData.Text = "Keine Daten zur Anzeige gefunden."
            DataGrid1.Visible = False
            lblNoData.Visible = True
        End If
    End Sub

    Private Sub LoadSapData()
        'Daten aus SAP laden
        m_Change = New fin_05(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
        m_Change.AppID = Session("AppID").ToString
        m_Change.CreditControlArea = "ZDAD"
        m_Change.Filiale = objSuche.HaendlerFiliale
        m_Change.Customer = m_User.KUNNR
        m_Change.Show(Session("AppID").ToString, Session.SessionID)
    End Sub
    
    Private Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.ItemCommand

        If e.CommandName = "Page" Then Exit Sub

        If e.CommandName = "SelectZZREFERENZ1" OrElse e.CommandName = "Select" Then
            If Not e.Item.Cells(0).Text.Length = 0 Then
                Dim strRedirectURL As String = "Change41_2.aspx?AppID=" & Session("AppID").ToString
                Session("SelectedDealer") = e.Item.Cells(0).Text
                objBank = New BankBaseCredit(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                objBank.CreditControlArea = "ZDAD"
                objBank.KUNNR = m_User.KUNNR
                objBank.Customer = Session("SelectedDealer").ToString
                objBank.Show(Session("AppID").ToString, Session.SessionID)
                Session("objBank") = objBank

                If IsNothing(m_Change) = True Then
                    LoadSapData()
                End If

                m_Change.Haendler = Session("SelectedDealer").ToString
                m_Change.zzreferenz1 = e.CommandArgument
                Session("m_Change") = m_Change
                Response.Redirect(strRedirectURL)
            End If
        End If
        
    End Sub

    Private Sub automaticFoward()

        If Session.Item("AppHaendlerNr") Is Nothing Then
            lblError.Text = "Session für Händlernummer nicht gefunden, Weiterleitung kann nicht erfolgen."
            Exit Sub
        Else
            Dim strRedirectURL As String = "Change41_2.aspx?AppID=" & Session("AppID").ToString
            Session("SelectedDealer") = Session("AppHaendlerNr")
            objBank = New BankBaseCredit(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
            objBank.CreditControlArea = "ZDAD"
            objBank.KUNNR = m_User.KUNNR
            objBank.Customer = Session("SelectedDealer").ToString
            objBank.Show(Session("AppID").ToString, Session.SessionID)
            Session("objBank") = objBank

            m_Change.Haendler = Session("SelectedDealer").ToString
            m_Change.Show(Session("AppID").ToString, Session.SessionID)
            Session("m_Change") = m_Change
            Response.Redirect(strRedirectURL)

        End If
    End Sub


    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged

        FillGrid(m_Change, e.NewPageIndex)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(m_Change, DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

        DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(m_Change, 0)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub lb_Haendlersuche_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_Haendlersuche.Click
        Response.Redirect("Change41.aspx?AppID=" & Session("AppID").ToString)
    End Sub
End Class

' ************************************************
' $History: Change41_1.aspx.vb $
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 22.06.09   Time: 16:02
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2918 Z_M_Faellige_Fahrzdok, Z_M_Faellige_Equipments_Update
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 22.06.09   Time: 13:18
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2918 Z_M_Creditlimit_Detail_001
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 5.06.09    Time: 15:58
' Updated in $/CKAG/Components/ComCommon/Finance
' mögliche try catches entfernt
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 4.05.09    Time: 11:50
' Updated in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 16.04.08   Time: 14:43
' Updated in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 14.04.08   Time: 14:50
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1842
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 18.01.08   Time: 10:57
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1624 fertig
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 7.01.08    Time: 14:32
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1499 Verbesserungen Change41_X
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 4.01.08    Time: 16:01
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Bugfix RTFS
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 20.12.07   Time: 12:41
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Excellinks berichtigt
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 13.12.07   Time: 13:14
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' lb Fieldtranslation Conirfm gemacht
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 13.12.07   Time: 11:25
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 13.12.07   Time: 11:18
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' fertig zum testen
' 
' *****************  Version 1  *****************
' User: Uha          Date: 12.12.07   Time: 14:10
' Created in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1466/1499 (Fällige Vorgänge) Kompilierfähig = unfertig eingefügt
' 
' ************************************************
