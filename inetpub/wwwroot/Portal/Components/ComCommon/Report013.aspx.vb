Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Report013
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

    Private m_User As Security.User
    Private m_App As Security.App
    Private objHandler As Kernel.Monatslisten
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ucStyles As Styles
    Protected WithEvents ucHeader As Header

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)


        m_App = New Security.App(m_User)

        If Not IsPostBack Then
            FillGrid(0, , True)
        End If
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "", Optional ByVal fill As Boolean = False)
        Dim tmpDataView As New DataView()

        objHandler = CType(Session("ResultTable"), Kernel.Monatslisten)

        If (objHandler.Result Is Nothing) OrElse (objHandler.Result.Rows.Count = 0) Then
            DataGrid1.Visible = False
            'ddlPageSize.Visible = False
            'lblNoData.Text = "Keine Daten zur Anzeige gefunden."
            'lblNoData.Visible = True
            'ShowScript.Visible = False
        Else
            Dim intTempPageIndex As Int32 = intPageIndex
            Dim strTempSort As String = ""
            Dim strDirection As String = ""

            'ddlPageSize.Visible = True
            tmpDataView = objHandler.Result.DefaultView
            tmpDataView.RowFilter = ""

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

            'lblNoData.Text = "Es wurde(n) " & tmpDataView.Count.ToString & " KFZ-Brief(e) gefunden."
            'lblNoData.Visible = True

            If DataGrid1.PageCount > 1 Then
                DataGrid1.PagerStyle.CssClass = "PagerStyle"
                DataGrid1.DataBind()
                DataGrid1.PagerStyle.Visible = True
            Else
                DataGrid1.PagerStyle.Visible = False
            End If


            'Dim item As DataGridItem
            'Dim cell As TableCell
            'Dim chkBox As CheckBox
            'Dim control As Control
            'Dim blnScriptFound As Boolean = False
            'Dim intZaehl As Int32

            'For Each item In DataGrid1.Items
            '    intZaehl = 1
            '    Dim strKKB As String = "0"
            '    Dim blnBezahlt As Boolean

            '    For Each cell In item.Cells
            '        If intZaehl = 2 Then
            '            strKKB = cell.Text
            '        End If
            '        For Each control In cell.Controls
            '            If TypeOf control Is CheckBox Then
            '                chkBox = CType(control, CheckBox)
            '                Select Case chkBox.ID
            '                    Case "chk0000"
            '                        If strKKB = "99" Then
            '                            chkBox.Checked = True
            '                        End If
            '                End Select
            '            End If
            '        Next
            '        intZaehl += 1
            '    Next
            'Next
            'If objHandler.Fahrzeuge.Select("MANDT='11'").GetUpperBound(0) > -1 Then
            '    DataGrid1.Columns(DataGrid1.Columns.Count - 1).Visible = True
            'Else
            '    DataGrid1.Columns(DataGrid1.Columns.Count - 1).Visible = False
            'End If
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Report013.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 5.06.09    Time: 15:58
' Updated in $/CKAG/Components/ComCommon
' mögliche try catches entfernt
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
' User: Uha          Date: 20.09.07   Time: 16:34
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' ITA 1181: Testversion
' 
' *****************  Version 5  *****************
' User: Uha          Date: 12.07.07   Time: 9:28
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 4  *****************
' User: Uha          Date: 5.03.07    Time: 14:57
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' 
' ************************************************
