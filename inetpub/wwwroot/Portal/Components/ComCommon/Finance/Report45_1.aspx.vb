Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Business

Public Class Report45_1
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

    Private objSuche As CKG.Components.ComCommon.Finance.Search
    Private objAKFBank As BankBaseCredit
    Private m_report As fin_09

    Protected WithEvents Hyperlink2 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents ShowScript As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lnkCreateExcel As System.Web.UI.WebControls.LinkButton
    Protected WithEvents Kopfdaten1 As PageElements.Kopfdaten

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        m_App = New Base.Kernel.Security.App(m_User)
        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text

        If m_report Is Nothing Then
            m_report = CType(Session("m_report"), fin_09)
        End If

        If Not IsPostBack Then
            objSuche = New Finance.Search(m_App, m_User, Session.SessionID, CStr(Session("AppID")))
            If objSuche.LeseHaendlerSAP_Einzeln(Session("AppID").ToString, Session.SessionID.ToString, m_report.personenennummer) Then
                Kopfdaten1.UserReferenz = m_User.Reference
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


            objAKFBank = New BankBaseCredit(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, m_report.personenennummer)
            objAKFBank.Customer = m_report.personenennummer
            objAKFBank.KUNNR = m_User.KUNNR
            objAKFBank.CreditControlArea = "ZDAD"
            objAKFBank.Show(Session("AppID").ToString, Session.SessionID)


            If objAKFBank.Status = 0 Then
                Kopfdaten1.Kontingente = objAKFBank.Kontingente
                Session("objAKFBank") = objAKFBank
            End If

            'ausblenden der Kontingente in den Kopfdaten
            If Request.QueryString("HDL") = 1 Then
                Session("AppShowNot") = True
            End If

            ddlPageSize.Items.Add("10")
            ddlPageSize.Items.Add("20")
            ddlPageSize.Items.Add("50")
            ddlPageSize.Items.Add("100")
            ddlPageSize.Items.Add("200")
            ddlPageSize.Items.Add("500")
            ddlPageSize.Items.Add("1000")
            ddlPageSize.SelectedIndex = 2
            fillAll()
        End If
    End Sub

    Private Sub fillAll()
        If Not Session("objAKFBank") Is Nothing Then
            objAKFBank = CType(Session("objAKFBank"), BankBaseCredit)
            If objAKFBank.Status = 0 Then
                If Not Session("m_report") Is Nothing Then
                    m_report = CType(Session("m_report"), fin_09)
                    FillGrid(0)
                    Kopfdaten1.Kontingente = objAKFBank.Kontingente
                Else
                    Response.Redirect("../../../Start/Selection.aspx")
                End If
            Else
                lblError.Text = objAKFBank.Message
            End If
        End If
    End Sub


    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        'm_intLineCount = 0
        If m_report.Status = 0 Then
            If m_report.Fahrzeuge.Rows.Count = 0 Then
                DataGrid1.Visible = False
                lblNoData.Visible = True
                lblNoData.Text = "Keine Daten zur Anzeige gefunden."
                ShowScript.Visible = False
            Else
                lnkCreateExcel.Visible = True
                DataGrid1.Visible = True
                lblNoData.Visible = False

                Dim tmpDataView As DataView
                tmpDataView = m_report.Fahrzeuge.DefaultView

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

                lblNoData.Text = "Es wurden " & tmpDataView.Count.ToString & " Fahrzeuge gefunden."
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
                Dim control As control
                Dim blnScriptFound As Boolean = False
                Dim intZaehl As Int32
                Dim hyperlink As hyperlink

                Dim strHistoryLink As String = ""
                If m_User.Applications.Select("AppName = 'Report46'").Length > 0 Then
                    strHistoryLink = "Report46.aspx?AppID=" & m_User.Applications.Select("AppName = 'Report46'")(0)("AppID").ToString & "&VIN="
                End If

                For Each item In DataGrid1.Items
                    'm_intLineCount += 1
                    intZaehl = 1
                    Dim strParameter As String = ""
                    For Each cell In item.Cells
                        If cell.Text = "01.01.1900" Then
                            cell.Text = "&nbsp;"
                        End If
                        If intZaehl < 7 Then
                            If cell.Text = "&nbsp;" Then
                                strParameter &= "'',"
                            Else
                                strParameter &= "'" & cell.Text & "',"
                            End If
                        End If
                        For Each control In cell.Controls
                            Dim linkButton = TryCast(control, LinkButton)
                            If (linkButton IsNot Nothing) Then
                                button = linkButton
                                If button.CommandName = "Bezahlt" Then
                                    button.Attributes.Add("onClick", "if (!FreigebenConfirm(" & strParameter.Trim(","c) & ")) return false;")
                                    button.Attributes.Add("class", "StandardButtonTable")
                                    blnScriptFound = True
                                End If
                            End If

                            Dim link = TryCast(control, HyperLink)
                            If (link IsNot Nothing) Then
                                hyperlink = link
                                Select Case hyperlink.ID
                                    Case "VIN"
                                        If strHistoryLink.Length > 0 Then
                                            hyperlink.NavigateUrl = strHistoryLink & hyperlink.NavigateUrl
                                        Else
                                            hyperlink.NavigateUrl = ""
                                        End If
                                End Select
                            End If

                        Next
                        intZaehl += 1
                    Next
                Next
                If blnScriptFound Then
                    ShowScript.Visible = True
                End If
            End If
        Else
            lblError.Text = m_report.Message
            lblNoData.Visible = True
            ShowScript.Visible = False
        End If
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
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

    Private Sub lnkCreateExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkCreateExcel.Click

        m_report = CType(Session("m_report"), fin_09)
        Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
        excelFactory.CreateDocumentAndSendAsResponse(strFileName, m_report.FahrzeugeExcel, Me.Page)
    End Sub

   
    Private Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.ItemCommand
        If e.CommandName = "Briefkopie" Then


        Dim dvAppLinks As DataView = m_User.Applications.DefaultView
            dvAppLinks.RowFilter = "APPURL='../Components/ComCommon/Finance/Change49.aspx'"

        If dvAppLinks.Count = 1 Then
                Dim strParameter As String = ""
                HelpProcedures.getAppParameters(dvAppLinks.Item(0).Item("AppID"), strParameter, ConfigurationManager.AppSettings("Connectionstring"))
                Response.Redirect("Change49.aspx?AppID=" & dvAppLinks.Item(0).Item("AppID") & strParameter & "&FIN=" & e.CommandArgument)
        Else
                lblError.Text = "Fehler bei der Weiterleitung zur Anforderung Briefkopie"
            lblError.Visible = True
            End If
        End If

    End Sub
End Class

' ************************************************
' $History: Report45_1.aspx.vb $
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 22.06.09   Time: 13:18
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2918 Z_M_Creditlimit_Detail_001
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 19.06.09   Time: 10:10
' Updated in $/CKAG/Components/ComCommon/Finance
' ITa 2918 testfertig
' 
' *****************  Version 4  *****************
' User: Dittbernerc  Date: 18.06.09   Time: 15:39
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2918 - abschaltung .net connector
' 
' BAPIS:
' 
' Z_M_Haendlerbestand
' Z_M_Faellige_Fahrzdok
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 5.06.09    Time: 15:58
' Updated in $/CKAG/Components/ComCommon/Finance
' mögliche try catches entfernt
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 11.04.08   Time: 13:43
' Updated in $/CKAG/Components/ComCommon/Finance
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:32
' Created in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 10:48
' Created in $/CKAG/portal/Components/ComCommon/Finance
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 6.02.08    Time: 11:27
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' unfertig
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 1.02.08    Time: 14:33
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' AKF Änderungen
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 8.01.08    Time: 10:18
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 4.01.08    Time: 16:01
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Bugfix RTFS
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 19.12.07   Time: 10:35
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Wartend auf Testdaten ITA 1493/1514 kompilierfähig
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 18.12.07   Time: 10:27
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1464/1498 kompilierfähig
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 18.12.07   Time: 8:25
' Created in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1464/1498  hinzugefügt, Proxy neu erstellt + Bapi Händlerbestand
' 
' ************************************************

