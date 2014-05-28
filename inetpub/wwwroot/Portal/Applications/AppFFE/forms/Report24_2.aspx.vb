Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements

Imports CKG.Base.Business

Public Class Report24_2
    Inherits System.Web.UI.Page

    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private m_intLineCount As Int32
    Private objSuche As FFE_Search
    Private objReport24_objFDDBank As FFE_BankBase
    Private objReport24_objFDDBank3 As FFE_Bank_3

    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lnkKreditlimit As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkVertragssuche As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkExcel As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblDownloadTip As System.Web.UI.WebControls.Label
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ShowScript As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents ucStyles As Styles
    Protected WithEvents ucHeader As Header
    Protected WithEvents Kopfdaten1 As Kopfdatenhaendler
    Protected WithEvents lnkCreateExcel As System.Web.UI.WebControls.LinkButton

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

            If Not Session("objReport24_objFDDBank") Is Nothing Then
                objReport24_objFDDBank = CType(Session("objReport24_objFDDBank"), FFE_BankBase)
                If Not IsPostBack Then
                    ddlPageSize.Items.Add("10")
                    ddlPageSize.Items.Add("20")
                    ddlPageSize.Items.Add("50")
                    ddlPageSize.Items.Add("100")
                    ddlPageSize.Items.Add("200")
                    ddlPageSize.Items.Add("500")
                    ddlPageSize.Items.Add("1000")
                    ddlPageSize.SelectedIndex = 2

                    
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
        If objReport24_objFDDBank.Status = 0 Then
            If Not Session("objReport24_objFDDBank3") Is Nothing Then
                objReport24_objFDDBank3 = CType(Session("objReport24_objFDDBank3"), FFE_Bank_3)
                FillGrid(0)
            Else
                Response.Redirect("../../../Start/Selection.aspx")
            End If
        Else
            lblError.Text = objReport24_objFDDBank.Message
        End If
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        m_intLineCount = 0
        If objReport24_objFDDBank3.Status = 0 Then
            If objReport24_objFDDBank3.Fahrzeuge.Rows.Count = 0 Then
                DataGrid1.Visible = False
                lblNoData.Visible = True
                lblNoData.Text = "Keine Daten zur Anzeige gefunden."
                ShowScript.Visible = False
            Else
                DataGrid1.Visible = True
                lblNoData.Visible = False

                Dim tmpDataView As New DataView()
                tmpDataView = objReport24_objFDDBank3.Fahrzeuge.DefaultView


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

                lnkCreateExcel.Visible = True
                Session("lnkExcel") = objReport24_objFDDBank3.FahrzeugeExcel

                Dim strHistoryLink As String = ""
                If m_User.Applications.Select("AppName = 'Report46'").Length > 0 Then
                    strHistoryLink = "Report46.aspx?AppID=" & m_User.Applications.Select("AppName = 'Report46'")(0)("AppID").ToString & "&VIN="
                End If
                Dim strStatusLink As String = ""
                If m_User.Applications.Select("AppName = 'Change06'").Length > 0 Then
                    strStatusLink = "Change06.aspx?AppID=" & m_User.Applications.Select("AppName = 'Change06'")(0)("AppID").ToString & "&VIN="
                End If

                Dim item As DataGridItem
                Dim cell As TableCell
                Dim button As LinkButton
                Dim Label As Label
                Dim control As Control
                Dim blnScriptFound As Boolean = False
                Dim intZaehl As Int32
                Dim hyperlink As HyperLink
                Dim sKont As String = ""

                For Each item In DataGrid1.Items
                    m_intLineCount += 1
                    intZaehl = 1
                    cell = item.Cells(7) ' Kontingentart
                    For Each control In cell.Controls
                        If TypeOf control Is Label Then
                            Label = CType(control, Label)
                            sKont = Label.Text
                        End If
                    Next
                    cell = item.Cells(12) ' Statuslink
                    For Each control In cell.Controls
                        If TypeOf control Is HyperLink Then
                            hyperlink = CType(control, HyperLink)
                            If sKont = "Standard temporär" Then hyperlink.Visible = True
                        End If
                    Next

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
                            If TypeOf control Is LinkButton Then
                                button = CType(control, LinkButton)
                                If button.CommandName = "Bezahlt" Then
                                    button.Attributes.Add("onClick", "if (!FreigebenConfirm(" & strParameter.Trim(","c) & ")) return false;")
                                    button.Attributes.Add("class", "StandardButtonTable")
                                    blnScriptFound = True
                                End If
                            End If

                            If TypeOf control Is HyperLink Then
                                hyperlink = CType(control, HyperLink)
                                Select Case hyperlink.ID
                                    Case "VIN"
                                        If strHistoryLink.Length > 0 Then
                                            hyperlink.NavigateUrl = strHistoryLink & hyperlink.NavigateUrl
                                        Else
                                            hyperlink.NavigateUrl = ""
                                        End If
                                    Case "Status"
                                        If strStatusLink.Length > 0 Then
                                            hyperlink.NavigateUrl = strStatusLink & hyperlink.NavigateUrl
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
            lblError.Text = objReport24_objFDDBank3.Message
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


    Protected Sub lnkCreateExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel.Click
        Try
            Dim control As New Control
            Dim tblTranslations As New DataTable()
            Dim AppURL As String
            Dim m_datatable As DataTable = Session("lnkExcel")
            Dim col As DataGridColumn
            Dim col2 As DataColumn
            Dim bVisibility As Integer
            Dim i As Integer
            Dim sColName As String = ""

            AppURL = Replace(Me.Request.Url.LocalPath, "/Portal", "..")
            tblTranslations = CType(Me.Session(AppURL), DataTable)
            For Each col In DataGrid1.Columns
                For i = m_datatable.Columns.Count - 1 To 0 Step -1
                    bVisibility = 0
                    col2 = m_datatable.Columns(i)
                    If col2.ColumnName = col.SortExpression Then
                        sColName = TranslateColLbtn(DataGrid1, tblTranslations, col.HeaderText, bVisibility)
                        If bVisibility = 0 Then
                            m_datatable.Columns.Remove(col2)
                        ElseIf sColName.Length > 0 Then
                            col2.ColumnName = sColName

                        End If
                    End If
                Next
                m_datatable.AcceptChanges()
            Next
            Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
            excelFactory.CreateDocumentAndSendAsResponse(strFileName, m_datatable, Me.Page)
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub
End Class
' ************************************************
' $History: Report24_2.aspx.vb $
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 18.06.08   Time: 14:33
' Updated in $/CKAG/Applications/AppFFE/forms
' Ausblenden Händler Kontingente
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 21.05.08   Time: 16:34
' Updated in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 13.05.08   Time: 16:41
' Updated in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 11.04.08   Time: 11:57
' Updated in $/CKAG/Applications/AppFFE/forms
' Migration OR
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 9.04.08    Time: 13:32
' Created in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 9.04.08    Time: 13:06
' Updated in $/CKG/Applications/AppFFE/AppFFEWeb/forms
' ITA: 1790
' 
' *****************  Version 1  *****************
' User: Rudolpho     Date: 3.04.08    Time: 11:19
' Created in $/CKG/Applications/AppFFE/AppFFEWeb/forms
' ITA 1790
' 
' ************************************************
