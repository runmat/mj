Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Change01
    Inherits System.Web.UI.Page
    '##### Floorcheck - Händleradministration via WEB"
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

#Region "Declarations"
    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private objHandler As Change_01
    'Private blnGridFilled As Boolean

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents ImageButton1 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblDownloadTip As System.Web.UI.WebControls.Label
    Protected WithEvents Datagrid2 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents Literal1 As System.Web.UI.WebControls.Literal
    Protected WithEvents PanelDatagrids As System.Web.UI.WebControls.Panel
    Protected WithEvents PanelAdressAenderung As System.Web.UI.WebControls.Panel
    Protected WithEvents lnkExcel As System.Web.UI.WebControls.HyperLink
    Protected WithEvents txtName1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtName2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtLand1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtPstlz As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtOrt01 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtStras As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtTelf1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtTelfx As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtSmtp_Addr As System.Web.UI.WebControls.TextBox
    Protected WithEvents ddlKatr9 As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lb_Aendern2 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lb_Back2 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents tr_Kunnr_I As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr_Name1 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr_Name2 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr_Land1 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr_Pstlz As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr_Ort01 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr_Stras As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr_Telf1 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr_Telfx As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr_Smtp_Addr As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr_Katr9 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lbl_Kunnr_I As System.Web.UI.WebControls.Label
    Protected WithEvents lblKunnr_IShow As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Name1 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Name2 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Land1 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Pstlz As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Ort01 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Stras As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Telf1 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Telfx As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Smtp_Addr As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Katr9 As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles
#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        'blnGridFilled = False

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text
        m_App = New Base.Kernel.Security.App(m_User)

        If Not Session("objHandler") Is Nothing Then
            objHandler = CType(Session("objHandler"), Change_01)
            Session("objHandler") = Nothing
        End If

        If Not IsPostBack Then
            HideInput()
            'blnGridFilled = True
            Session("lnkExcel") = ""
            PanelAdressAenderung.Visible = False
            PanelDatagrids.Visible = True

            DoSubmit()
        End If

    End Sub

    Private Sub AusfuehrenSchreibenSAP()
        objHandler.Kunnr_I = Me.lblKunnr_IShow.Text
        objHandler.Name1 = Me.txtName1.Text
        objHandler.Name2 = Me.txtName2.Text
        objHandler.Land1 = Me.txtLand1.Text
        objHandler.Pstlz = Me.txtPstlz.Text
        objHandler.Ort01 = Me.txtOrt01.Text
        objHandler.Stras = Me.txtStras.Text
        objHandler.Telf1 = Me.txtTelf1.Text
        objHandler.Telfx = Me.txtTelfx.Text
        objHandler.Smtp_Addr = Me.txtSmtp_Addr.Text
        objHandler.Katr9 = ddlKatr9.SelectedItem.Value

        If objHandler.Name1.Length = 0 Or objHandler.Stras.Length = 0 Or objHandler.Land1.Length = 0 Or objHandler.Pstlz.Length = 0 Or objHandler.Ort01.Length = 0 Then
            Throw New Exception("Adresse unvollständig eingegeben.")
        End If

        'Aufruf Change
        objHandler.Change()

        'Neuer Excellink
        If objHandler.Status = 0 Then
            'Neue Suche
            DoSubmit()
        Else
            lblError.Text = "Fehler: " & objHandler.Message
        End If

        'Darstellung wiederherstellen
        HideAdressAenderung()
    End Sub

    Private Sub AuswahlAdressAenderung(ByVal strKundennummer As String)
        ShowAdressAenderung()

        lblKunnr_IShow.Text = strKundennummer.Trim(" "c)

        Dim rows() As DataRow
        If lblKunnr_IShow.Text.Length = 0 Then
            lblError.Text = "Kein Schlüssel zur Adressänderung übergeben."
            lb_Aendern2.Enabled = False
            Exit Sub
        Else
            rows = objHandler.Result.Select("Kunnr = '" & lblKunnr_IShow.Text & "'")
            If CStr(rows(0)("Katr9")).Length > 0 Then
                ddlKatr9.Items.FindByValue(CStr(rows(0)("Katr9"))).Selected = True
            End If
            txtLand1.Text = CStr(rows(0)("Land1"))
            txtName1.Text = CStr(rows(0)("Name1"))
            txtName2.Text = CStr(rows(0)("Name2"))
            txtOrt01.Text = CStr(rows(0)("Ort01"))
            txtPstlz.Text = CStr(rows(0)("Pstlz"))
            txtSmtp_Addr.Text = CStr(rows(0)("Smtp_Addr"))
            txtStras.Text = CStr(rows(0)("Stras"))
            txtTelf1.Text = CStr(rows(0)("Telf1"))
            txtTelfx.Text = CStr(rows(0)("Telfx"))
            lb_Aendern2.Enabled = True
        End If

        'blnGridFilled = True
    End Sub

    Private Sub ShowAdressAenderung()
        PanelAdressAenderung.Visible = True
        PanelDatagrids.Visible = False
        lnkExcel.Visible = False
        lblDownloadTip.Visible = False
    End Sub

    Private Sub HideAdressAenderung()
        PanelAdressAenderung.Visible = False
        PanelDatagrids.Visible = True
        lnkExcel.Visible = True
        lblDownloadTip.Visible = True
    End Sub

    Private Sub ShowInput()
        Datagrid2.Visible = True
    End Sub

    Private Sub HideInput()
        Datagrid2.Visible = False
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        'blnGridFilled = True

        Dim tmpDataView = objHandler.Result.DefaultView

        If tmpDataView.Count = 0 Then
            Datagrid2.Visible = False
        Else
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

            Datagrid2.CurrentPageIndex = intTempPageIndex

            Datagrid2.DataSource = tmpDataView
            Datagrid2.DataBind()

            If Datagrid2.PageCount > 1 Then
                Datagrid2.PagerStyle.CssClass = "PagerStyle"
                Datagrid2.DataBind()
                Datagrid2.PagerStyle.Visible = True
            Else
                Datagrid2.PagerStyle.Visible = False
            End If

            Dim item As DataGridItem
            For Each item In Datagrid2.Items
                If objHandler.Kunnr_I = item.Cells(0).Text Then
                    item.CssClass = "GridTableHighlight"
                    SetLiteral1ForDatagrid2(item.Cells(0).Text)
                End If
            Next
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        If Not Session("lnkExcel") Is Nothing AndAlso CStr(Session("lnkExcel")).Length > 0 Then
            lnkExcel.NavigateUrl = CStr(Session("lnkExcel"))
            lnkExcel.Visible = True
            lblDownloadTip.Visible = True
        Else
            lnkExcel.Visible = False
            lblDownloadTip.Visible = False
        End If
        If Not objHandler Is Nothing Then
            Session.Add("objHandler", objHandler)
        End If

        SetEndASPXAccess(Me)

    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        DoSubmit()
       
    End Sub

    Private Sub DoSubmit()
        HideInput()
        Session("lnkExcel") = ""
        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)

        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
        objHandler = New Change_01(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, strFileName)

        lblError.Text = ""

        objHandler.Show()

        If Not objHandler.Status = 0 Then
            lblError.Text = "Fehler: " & objHandler.Message
            'blnGridFilled = True
        Else
            ShowInput()
            If objHandler.Result.Rows.Count = 0 Then
                lblError.Text = "Keine Daten vorhanden."
            Else

                Dim excelFactory As New Base.Kernel.DocumentGeneration.ExcelDocumentFactory()

                Try
                    objHandler.CreateExcelFromFieldTranslation(Session(Replace(Request.Url.LocalPath, "/Portal", "..")), objHandler.Result)
                    excelFactory.CreateDocumentAndWriteToFilesystem(ConfigurationManager.AppSettings("ExcelPath") & strFileName, objHandler.ResultExcel, Me)
                    Session("lnkExcel") = "/Portal/Temp/Excel/" & strFileName
                Catch ex2 As Exception
                    Dim strError As String = ex2.ToString
                End Try
                logApp.UpdateEntry("APP", Session("AppID").ToString, "Workflow Werkstattzuordnungsliste 1")

                FillGrid(0)
                FillDdl()
            End If
        End If

    End Sub

    Private Sub FillDdl()
        ddlKatr9.DataSource = objHandler.PruefIntervalle
        ddlKatr9.DataValueField = "Katr9"
        ddlKatr9.DataTextField = "Vtext"
        ddlKatr9.DataBind()
    End Sub

    Private Sub Datagrid2_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles Datagrid2.ItemCommand

        Select Case e.CommandName
            Case "Aendern"
                AuswahlAdressAenderung(e.Item.Cells(0).Text)
                'Case Else
                'blnGridFilled = True
        End Select
       
    End Sub

    Private Sub SetLiteral1ForDatagrid2(ByVal strTarget As String)
        Literal1.Text = "						<script language=""Javascript"">" & vbCrLf
        Literal1.Text &= "						  <!-- //" & vbCrLf
        Literal1.Text &= "						    window.document.location.href = ""#" & strTarget & """;" & vbCrLf
        Literal1.Text &= "						  //-->" & vbCrLf
        Literal1.Text &= "						</script>" & vbCrLf
    End Sub

    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        DoBack()
    End Sub

    Private Sub DoBack()

        HideAdressAenderung()
        'blnGridFilled = True
     
    End Sub

    Private Sub Datagrid2_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles Datagrid2.SortCommand

        FillGrid(0, e.SortExpression)

    End Sub

    Private Sub Datagrid2_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles Datagrid2.PageIndexChanged

        FillGrid(e.NewPageIndex)
       
    End Sub

    Private Sub lb_Aendern2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_Aendern2.Click

        AusfuehrenSchreibenSAP()
       
    End Sub

    Private Sub lb_Back2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_Back2.Click
        DoBack()
    End Sub
End Class

' ************************************************
' $History: Change01.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 5.06.09    Time: 13:51
' Updated in $/CKAG/Components/ComCommon
' Try Catch entfernt wenn möglich
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 11.04.08   Time: 13:43
' Updated in $/CKAG/Components/ComCommon
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:32
' Created in $/CKAG/Components/ComCommon
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 10:47
' Created in $/CKAG/portal/Components/ComCommon
' 
' *****************  Version 6  *****************
' User: Uha          Date: 27.09.07   Time: 10:27
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' Controlnamen für Feldübersetzungen geändert
' 
' *****************  Version 5  *****************
' User: Uha          Date: 26.09.07   Time: 16:49
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' In Change01, Change03 und Change80 neues Format "GridTableHighlight"
' verwendet.
' 
' *****************  Version 4  *****************
' User: Uha          Date: 20.09.07   Time: 16:34
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' ITA 1181: Testversion
' 
' *****************  Version 3  *****************
' User: Uha          Date: 19.09.07   Time: 13:19
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
'  ITA 1261: Testfähige Version
' 
' *****************  Version 2  *****************
' User: Uha          Date: 18.09.07   Time: 18:15
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' ITA 1261: Erste Testversion (Excelspalten müssen noch übersetzt werden;
' Rückschreiben wirft SAP-Fehler)
' 
' *****************  Version 1  *****************
' User: Uha          Date: 17.09.07   Time: 18:14
' Created in $/CKG/Components/ComCommon/ComCommonWeb
' ITA 1261: Under Construction - Keine Funktion
' 
' ************************************************
