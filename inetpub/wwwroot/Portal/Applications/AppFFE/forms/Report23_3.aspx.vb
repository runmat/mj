Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Report23_3
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
    'Private m_intLineCount As Int32
    Private objSuche As FFE_Search
    Private objReport23_objFDDBank As FFE_BankBase
    Private objReport23_objFDDBank3 As FFE_Bank_3

    Protected WithEvents Hyperlink2 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents ShowScript As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lnkExcel As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblDownloadTip As System.Web.UI.WebControls.Label
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lnkKreditlimit As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkVertragssuche As System.Web.UI.WebControls.HyperLink
    Protected WithEvents ucStyles As Styles
    Protected WithEvents Kopfdaten1 As Kopfdaten

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            m_User = GetUser(Me)
            ucHeader.InitUser(m_User)
            FormAuth(Me, m_User)
            m_App = New Base.Kernel.Security.App(m_User)
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

            Dim strNamePart As String = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppName").ToString
            If (Left(strNamePart, 8) = "Report23") AndAlso (Session("SelectedDealer").ToString.Length = 0 OrElse (Session("objSuche") Is Nothing)) Then
                Response.Redirect("Report23.aspx?AppID=" & Session("AppID").ToString)
            Else
                If Left(strNamePart, 8) = "Report24" Then
                    If (Session("objSuche") Is Nothing) Then
                        objSuche = New FFE_Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
                        If Not objSuche.LeseHaendlerSAP_Einzeln(Session("AppID").ToString, Session.SessionID.ToString, m_User.Reference) Then
                            lblError.Text = "Fehler bei der Ermittlung der Händlerdaten.<br>(" & objSuche.ErrorMessage & ")"
                        End If
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
                Else
                    objSuche = CType(Session("objSuche"), FFE_Search)
                    If objSuche.LeseHaendlerSAP_Einzeln(Session("AppID").ToString, Session.SessionID.ToString, Session("SelectedDealer").ToString) Then
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
                End If
            End If

            If strNamePart = "Report23" Then
                lnkKreditlimit.NavigateUrl = "Report23.aspx?AppID=" & Session("AppID").ToString & "&Back=1"
                lnkVertragssuche.NavigateUrl = "Report23_2.aspx?AppID=" & Session("AppID").ToString
            Else
                lnkKreditlimit.NavigateUrl = "Report24.aspx?AppID=" & Session("AppID").ToString
                lnkKreditlimit.Text = "Vertragssuche"
                lnkVertragssuche.Visible = False
            End If
            ucStyles.TitleText = lblHead.Text

            If Not Session("objReport23_objFDDBank") Is Nothing Then
                objReport23_objFDDBank = CType(Session("objReport23_objFDDBank"), FFE_BankBase)
                If Not IsPostBack Then
                    ddlPageSize.Items.Add("10")
                    ddlPageSize.Items.Add("20")
                    ddlPageSize.Items.Add("50")
                    ddlPageSize.Items.Add("100")
                    ddlPageSize.Items.Add("200")
                    ddlPageSize.Items.Add("500")
                    ddlPageSize.Items.Add("1000")
                    ddlPageSize.SelectedIndex = 2

                    If Not Session("lnkExcel").ToString.Length = 0 Then
                        lblDownloadTip.Visible = True
                        lnkExcel.Visible = True
                        lnkExcel.NavigateUrl = Session("lnkExcel").ToString
                    End If
                    CheckobjFDDBank3()
                End If
            Else
                Response.Redirect("../../../Start/Selection.aspx")
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub CheckobjFDDBank3()
        If objReport23_objFDDBank.Status = 0 Then
            If Not Session("objReport23_objFDDBank3") Is Nothing Then
                objReport23_objFDDBank3 = CType(Session("objReport23_objFDDBank3"), FFE_Bank_3)
                FillGrid(0)
                Kopfdaten1.Kontingente = objReport23_objFDDBank.Kontingente
            Else
                Response.Redirect("../../../Start/Selection.aspx")
            End If
        Else
            lblError.Text = objReport23_objFDDBank.Message
        End If
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        'm_intLineCount = 0
        If objReport23_objFDDBank3.Status = 0 Then
            If objReport23_objFDDBank3.Fahrzeuge.Rows.Count = 0 Then
                DataGrid1.Visible = False
                lblNoData.Visible = True
                lblNoData.Text = "Keine Daten zur Anzeige gefunden."
                ShowScript.Visible = False
            Else
                DataGrid1.Visible = True
                lblNoData.Visible = False

                Dim tmpDataView As DataView = objReport23_objFDDBank3.Fahrzeuge.DefaultView

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

                lblNoData.Text = "Es wurden " & tmpDataView.Count.ToString & " Fahrzeugdokumente gefunden."
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
                Dim intZaehl As Int32

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
                        Next
                        intZaehl += 1
                    Next
                Next

                If blnScriptFound Then
                    ShowScript.Visible = True
                End If
            End If
        Else
            lblError.Text = objReport23_objFDDBank3.Message
            lblNoData.Visible = True
            ShowScript.Visible = False
        End If
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        CheckobjFDDBank3()
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        CheckobjFDDBank3()
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        CheckobjFDDBank3()
        DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(0)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Report23_3.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 4.06.08    Time: 18:43
' Updated in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 13.05.08   Time: 16:41
' Updated in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 1  *****************
' User: Rudolpho     Date: 5.05.08    Time: 17:09
' Created in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:23
' Created in $/CKAG/Applications/appffd/Forms
' 
' *****************  Version 11  *****************
' User: Uha          Date: 2.07.07    Time: 12:52
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 8.06.07    Time: 15:36
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' Abgleich Beyond Compare
' 
' *****************  Version 9  *****************
' User: Uha          Date: 3.05.07    Time: 18:05
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' Änderungen aus StartApplication vom 02.05.2007 Mittags übernommen
' 
' ************************************************
