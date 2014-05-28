Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Change03_2
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

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private objSuche As Search
    Private objHandler As Change_03

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lnkCreateExcel As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lb_Confirm As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lb_Back As System.Web.UI.WebControls.LinkButton
    Protected WithEvents litHaendlerShow As System.Web.UI.WebControls.Literal
    Protected WithEvents lbl_Haendler As System.Web.UI.WebControls.Label
    Protected WithEvents litAuftragShow As System.Web.UI.WebControls.Literal
    Protected WithEvents lbl_Auftrag As System.Web.UI.WebControls.Label
    Protected WithEvents tr_Auftrag As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr_Haendler As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        If m_User.Organization.OrganizationAdmin Then
            lblHead.Text = Replace(lblHead.Text, " (Beauftragung)", "")
        End If
        ucStyles.TitleText = lblHead.Text
        m_App = New Base.Kernel.Security.App(m_User)

        If Session("objHandler") Is Nothing Then
            Response.Redirect("Change03.aspx?AppID=" & Session("AppID").ToString)
        End If

        objHandler = CType(Session("objHandler"), Change_03)

        If Not IsPostBack Then
            lnkCreateExcel.Visible = False

            DataGrid1.BackColor = System.Drawing.Color.White

            FillGrid(0)

            Dim tmpRows() As DataRow = objHandler.Result.Select("Vbeln='" & objHandler.Vbeln & "'")
            litAuftragShow.Text = objHandler.Vbeln & " zum " & CDate(tmpRows(0)("Wldat")).ToShortDateString
            litHaendlerShow.Text = CStr(tmpRows(0)("Kunnr")) & ",<br>" & CStr(tmpRows(0)("Name1")) & " " & CStr(tmpRows(0)("Name2")) & ",<br>" & CStr(tmpRows(0)("Post_code1")) & " " & CStr(tmpRows(0)("City1"))
        End If
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        Dim tmpDataView As New DataView()
        tmpDataView = objHandler.Positionen.DefaultView

        If tmpDataView.Count = 0 Then
            DataGrid1.Visible = False
            lblNoData.Text = "Keine Daten zur Anzeige gefunden."
            lblNoData.Visible = True
            lb_Confirm.Visible = False
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

            DataGrid1.CurrentPageIndex = intTempPageIndex

            DataGrid1.DataSource = tmpDataView
            DataGrid1.DataBind()

            Dim strTemp As String
            If tmpDataView.Count = 1 Then
                strTemp = "Es wurde ein Fahrzeug"
            Else
                strTemp = "Es wurden " & tmpDataView.Count.ToString & " Fahrzeuge"
            End If
            If lb_Confirm.Visible Then
                lblNoData.Text = strTemp & " ausgewählt."
            Else
                lblNoData.Text = strTemp & " übertragen."
            End If
            lblNoData.Visible = True

            If DataGrid1.PageCount > 1 Then
                DataGrid1.PagerStyle.CssClass = "PagerStyle"
                DataGrid1.DataBind()
                DataGrid1.PagerStyle.Visible = True
            Else
                DataGrid1.PagerStyle.Visible = False
            End If

            'Dim item As DataGridItem
            'Dim cell As TableCell
            'Dim textBox As textBox
            'Dim control As control
            'Dim blnScriptFound As Boolean = False

            'For Each item In DataGrid1.Items
            '    For Each cell In item.Cells
            '        For Each control In cell.Controls
            '            If TypeOf control Is textBox Then
            '                textBox = CType(control, textBox)
            '                If textBox.ID = "txtVIN2" Then
            '                    If cmdConfirm.Visible Then
            '                        textBox.Enabled = False
            '                    End If
            '                    textBox.CssClass = ""
            '                    If textBox.Enabled = False Then
            '                        textBox.CssClass = "InfoBoxFlat"
            '                    End If
            '                End If
            '            End If
            '        Next
            '    Next
            'Next
        End If
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged

        FillGrid(e.NewPageIndex)
       
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand

        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
       
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub lnkCreateExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkCreateExcel.Click

        Dim excelFactory As New Base.Kernel.DocumentGeneration.ExcelDocumentFactory()
        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName

        objHandler.CreateExcelFromFieldTranslation(Session(Replace(Request.Url.LocalPath, "/Portal", "..")), objHandler.Positionen)
        excelFactory.CreateDocumentAndSendAsResponse(strFileName, objHandler.ResultExcel, Me.Page)

    End Sub

    Private Sub lb_Back_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_Back.Click
        Response.Redirect("Change03.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub lb_Confirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_Confirm.Click

        objHandler.Change()
        Session("objHandler") = objHandler

        lb_Confirm.Visible = False
        lnkCreateExcel.Visible = True

        FillGrid(0)

    End Sub
End Class

' ************************************************
' $History: Change03_2.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 5.06.09    Time: 13:51
' Updated in $/CKAG/Components/ComCommon
' Try Catch entfernt wenn möglich
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 10.04.08   Time: 17:37
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
' *****************  Version 4  *****************
' User: Uha          Date: 27.09.07   Time: 10:27
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' Controlnamen für Feldübersetzungen geändert
' 
' *****************  Version 3  *****************
' User: Uha          Date: 26.09.07   Time: 13:22
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' Bugfixing in ITA 1237, 1181 und 1238 (Alle Floorcheck)
' 
' *****************  Version 2  *****************
' User: Uha          Date: 25.09.07   Time: 17:03
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' ITA 1124 hinzugefügt (Change03/Change03_2) und allgemeines Bugfix
' Floorcheck
' 
' *****************  Version 1  *****************
' User: Uha          Date: 24.09.07   Time: 18:07
' Created in $/CKG/Components/ComCommon/ComCommonWeb
' ITA 1124: Upload Prüflisten via WEB - Nicht lauffähige Vorversion
' 
' ************************************************
