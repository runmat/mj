Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common


Public Class Change01_2
    Inherits System.Web.UI.Page

    Protected WithEvents ucStyles As Styles
    Protected WithEvents ucHeader As Header
    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private ResultTable As DataTable
    Private highlightID As String
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents dataGrid As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblAnzahl As System.Web.UI.WebControls.Label
    Protected WithEvents btnSaveSAP As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblTableTitle As System.Web.UI.WebControls.Label
    Protected WithEvents txtOrtsKzOld As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents txtFree2 As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents trSumme As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents Linkbutton1 As System.Web.UI.WebControls.LinkButton
    
    Private objBatch As ec_01

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
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        Session.Add("AppID", Request.QueryString("AppID"))

        objBatch = CType(Session("objSuche"), ec_01)

        If Not IsPostBack Then
            Try
                
                lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
                ucStyles.TitleText = lblHead.Text

                FillGrid(0)
               
            Catch ex As Exception
            End Try

        End If
    End Sub

    Public Function noRecords() As Boolean

    End Function

    Private Sub btnFinish_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    
    End Sub

    Private Sub loadRecordsNach()
        'Nacherfassung()
      
        Dim message As String = String.Empty

        If Session("ReloadNacherfassung") = "1" Then
            Session("ReloadNacherfassung") = "0"
        End If

        loadRecords()

        If Not noRecords() Then
           
        Else
            lblError.Text = "Keine Daten gefunden."
        End If
    End Sub

    Private Sub loadRecords()
       
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        objBatch = CType(Session("objSuche"), ec_01)

        ResultTable = objBatch.ResultTable

        If (ResultTable Is Nothing) OrElse (ResultTable.Rows.Count = 0) Then
            dataGrid.Visible = False
           
        Else
            lblAnzahl.Text = ResultTable.Rows.Count

            dataGrid.Visible = True

            Dim tmpDataView As New DataView()
            tmpDataView = ResultTable.DefaultView

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
            Else
                tmpDataView.Sort = "modelid, batchid,unitnrvon,unitnrbis"
            End If

            dataGrid.CurrentPageIndex = intTempPageIndex
            dataGrid.DataSource = tmpDataView
            dataGrid.DataBind()

            If dataGrid.PageCount > 1 Then
                dataGrid.PagerStyle.CssClass = "PagerStyle"
                dataGrid.DataBind()
                dataGrid.PagerStyle.Visible = True
            Else
                dataGrid.PagerStyle.Visible = False
            End If
        End If
    End Sub

    Private Sub btnSaveSAP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveSAP.Click
        Dim row As DataRow
        Dim intCounter As Integer

        btnSaveSAP.Enabled = False

        For Each row In objBatch.ResultTable.Rows
            If (TypeOf (row("Status")) Is System.DBNull) OrElse (CType(row("Status"), String) <> "Gespeichert.") Then  'Nur die noch nicht abgesendeten Vorgänge...
                objBatch.saveData(Session("AppID").ToString, Session.SessionID, row, Me)
                If (objBatch.Status = 0) Then
                    row("Status") = "Gespeichert."
                Else
                    row("Status") = objBatch.Message
                End If

            End If
        Next
        FillGrid(0)

        For intCounter = objBatch.ResultTable.Rows.Count - 1 To 0 Step -1
            If (CType(objBatch.ResultTable.Rows(intCounter)("Status"), String) = "Gespeichert.") Then
                objBatch.ResultTable.Rows.Remove(objBatch.ResultTable.Rows(intCounter))
            End If
        Next
        Session("objSuche") = objBatch
    End Sub

    Private Sub dataGrid_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dataGrid.ItemCommand
        Dim status As String = ""
        Dim objBatch As ec_01

        objBatch = CType(Session("objSuche"), ec_01)

        If e.CommandName = "Select" Then
            objBatch.Selection = e.Item.ItemIndex           'Row-Nr
            objBatch.SelectionId = e.Item.Cells(0).Text     'Row-Id
            objBatch.getRowData(status)
            If (status <> String.Empty) Then
                lblError.Text = status
                Exit Sub
            End If
            Session("objSuche") = objBatch
            Response.Redirect("Change01.aspx?AppID=" & Session("AppID").ToString)
        End If
    End Sub


    Private Sub dataGrid_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dataGrid.SortCommand
        FillGrid(dataGrid.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub Linkbutton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Linkbutton1.Click
        Dim objBatch As ec_01

        objBatch = CType(Session("objSuche"), ec_01)
        objBatch.Selection = -1

        Response.Redirect("Change01.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Change01_2.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 25.06.09   Time: 16:04
' Updated in $/CKAG/Applications/appec/Forms
' ITA 2918 Z_M_Herstellergroup, Z_M_EC_AVM_BATCH_update,
' Z_M_EC_AVM_HERST_VWZWECK_MODID, Z_M_EC_AVM_BATCH_INSERT,
' Z_M_EC_AVM_BATCH_ANSICHT
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 17.06.09   Time: 8:47
' Updated in $/CKAG/Applications/appec/Forms
' Warnungen entfernt!
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 16.04.08   Time: 11:52
' Updated in $/CKAG/Applications/appec/Forms
' 
' *****************  Version 7  *****************
' User: Uha          Date: 2.07.07    Time: 12:29
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 6  *****************
' User: Uha          Date: 7.03.07    Time: 11:02
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' 
' ************************************************
