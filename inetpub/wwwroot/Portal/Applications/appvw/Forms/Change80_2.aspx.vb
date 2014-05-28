Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Change80_2
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
    Private objHaendler As VW_06

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lnkKreditlimit As System.Web.UI.WebControls.HyperLink
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ucStyles As Styles
    Protected WithEvents cmdConfirm As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdBack As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkCreateExcel As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        lnkKreditlimit.NavigateUrl = "Change80.aspx?AppID=" & Session("AppID").ToString

        GetAppIDFromQueryString(Me)
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            If m_User.Organization.OrganizationAdmin Then
                lblHead.Text = Replace(lblHead.Text, " (Beauftragung)", "")
            End If
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If Session("objHaendler") Is Nothing Then
                Response.Redirect("Change80.aspx?AppID=" & Session("AppID").ToString)
            End If

            objHaendler = CType(Session("objHaendler"), VW_06)

            If Not IsPostBack Then
                lnkCreateExcel.Visible = False

                ddlPageSize.Items.Add("10")
                ddlPageSize.Items.Add("20")
                ddlPageSize.Items.Add("50")
                ddlPageSize.Items.Add("100")
                ddlPageSize.Items.Add("250")
                ddlPageSize.SelectedIndex = 2

                DataGrid1.BackColor = System.Drawing.Color.White

                FillGrid(0, "", True)
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Try
            If CheckGrid() = 0 Then
                lblError.Text = "Bitte wählen Sie erst Fahrzeuge zur Anforderung aus."
                FillGrid(DataGrid1.CurrentPageIndex, "")
            Else
                cmdConfirm.Visible = True
                cmdBack.Visible = True
                cmdSave.Visible = False
                FillGrid(DataGrid1.CurrentPageIndex, "AUSGEWAEHLT=1")
            End If
        Catch ex As Exception
            lblError.Text = "Beim Prüfen der Daten ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Function CheckGrid() As Int32
        Dim item As DataGridItem
        Dim cell As TableCell
        Dim textbox As TextBox
        Dim control As Control
        Dim blnScriptFound As Boolean = False
        Dim intReturn As Int32 = 0
        Dim tmpRows As DataRow()

        For Each item In DataGrid1.Items
            Dim strIKZ As String = ""
            strIKZ = "WAEHLBAR=1 AND AUSGEWAEHLT=0 AND REFERENZ1 = '" & item.Cells(3).Text & "'"
            tmpRows = objHaendler.FahrzeugeAenderung.Select(strIKZ)
            If (tmpRows.Length > 0) Then
                If CBool(tmpRows(0).Item("WAEHLBAR")) Then
                    tmpRows(0).BeginEdit()
                    tmpRows(0).Item("AUSGEWAEHLT") = False
                    cell = item.Cells(6)

                    For Each control In cell.Controls
                        If TypeOf control Is TextBox Then
                            textbox = CType(control, TextBox)
                            textbox.Text = textbox.Text.Trim(" "c)
                            If textbox.Text.Length = 6 Then
                                intReturn += 1

                                tmpRows(0).Item("CHASSIS_NUM2") = textbox.Text
                                tmpRows(0).Item("AUSGEWAEHLT") = True
                            End If
                        End If
                    Next
                    tmpRows(0).EndEdit()
                    objHaendler.FahrzeugeAenderung.AcceptChanges()
                End If
            End If
        Next
        Session("objHaendler") = objHaendler
        Return intReturn
    End Function

    Private Sub FillGrid(ByVal intPageIndex As Int32, ByVal strFilter As String, Optional ByVal fill As Boolean = False, Optional ByVal strSort As String = "")
        Dim tmpDataView As New DataView()
        tmpDataView = objHaendler.FahrzeugeAenderung.DefaultView
        tmpDataView.RowFilter = strFilter

        If tmpDataView.Count = 0 Then
            DataGrid1.Visible = False
            ddlPageSize.Visible = False
            lblNoData.Text = "Keine Daten zur Anzeige gefunden."
            lblNoData.Visible = True
            'ShowScript.Visible = False
        Else
            ddlPageSize.Visible = True
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
            If cmdSave.Visible Then
                lblNoData.Text = strTemp & " gefunden."
            ElseIf cmdConfirm.Enabled Then
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


            Dim item As DataGridItem
            Dim cell As TableCell
            Dim textBox As TextBox
            Dim control As Control
            Dim blnScriptFound As Boolean = False

            For Each item In DataGrid1.Items
                For Each cell In item.Cells
                    For Each control In cell.Controls
                        If TypeOf control Is TextBox Then
                            textBox = CType(control, TextBox)
                            If textBox.ID = "txtVIN2" Then
                                If cmdConfirm.Visible Then
                                    textBox.Enabled = False
                                End If
                                textBox.CssClass = ""
                                If textBox.Enabled = False Then
                                    textBox.CssClass = "InfoBoxFlat"
                                End If
                            End If
                        End If
                    Next
                Next
            Next
        End If
    End Sub

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        Try
            DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
            Dim strFilter As String = ""
            If cmdConfirm.Visible Then
                strFilter = "AUSGEWAEHLT=1"
            End If
            FillGrid(0, strFilter)
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        Try
            CheckGrid()
            Dim strFilter As String = ""
            If cmdConfirm.Visible Then
                strFilter = "AUSGEWAEHLT=1"
            End If
            FillGrid(e.NewPageIndex, strFilter)
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        Try
            CheckGrid()
            Dim strFilter As String = ""
            If cmdConfirm.Visible Then
                strFilter = "AUSGEWAEHLT=1"
            End If
            FillGrid(DataGrid1.CurrentPageIndex, strFilter, , e.SortExpression)
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBack.Click
        Try
            cmdSave.Visible = True
            cmdBack.Visible = False
            cmdConfirm.Visible = False

            FillGrid(DataGrid1.CurrentPageIndex, "")
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdConfirm.Click
        Try
            objHaendler.Change()
            Session("objHaendler") = objHaendler

            DataGrid1.Columns(4).Visible = True
            DataGrid1.Columns(5).Visible = False
            DataGrid1.Columns(6).Visible = False

            cmdConfirm.Enabled = False
            cmdBack.Enabled = False
            lnkCreateExcel.Visible = True

            FillGrid(0, "AUSGEWAEHLT=1")
        Catch ex As Exception
            lblError.Text = "Beim Speichern der Daten ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub lnkCreateExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkCreateExcel.Click
        Try
            Dim objExcel As DataTable = objHaendler.FahrzeugeAenderung.Copy

            objExcel.Columns(0).ColumnName = "Vorhaben"
            objExcel.Columns(1).ColumnName = "Vorhaben Teil"
            objExcel.Columns(2).ColumnName = "Fahrzeugtyp"
            objExcel.Columns(3).ColumnName = "IKZ"
            objExcel.Columns(4).ColumnName = "Fahrgestellnummer"
            objExcel.Columns(5).ColumnName = "Ergebnis"
            objExcel.Columns.Remove("CHASSIS_NUM1")
            objExcel.Columns.Remove("CHASSIS_NUM2")
            objExcel.Columns.Remove("WAEHLBAR")
            objExcel.Columns.Remove("AUSGEWAEHLT")
            Dim excelFactory As New Base.Kernel.DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName

            excelFactory.CreateDocumentAndSendAsResponse(strFileName, objExcel, Me.Page, , "Applications\AppVW\Docs\Werkstattzuordnungsliste_Template.xls")

        Catch ex As Exception
            lblError.Text = "Fehler beim Erstellen der Excel-Datei: " + ex.Message
        End Try
    End Sub
End Class

' ************************************************
' $History: Change80_2.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 4.05.09    Time: 11:14
' Updated in $/CKAG/Applications/appvw/Forms
' ITA: 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 17:48
' Created in $/CKAG/Applications/appvw/Forms
' 
' *****************  Version 3  *****************
' User: Uha          Date: 22.08.07   Time: 17:52
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Forms
' Bugfixing ITA 1120 und 1177
' 
' *****************  Version 2  *****************
' User: Uha          Date: 15.08.07   Time: 11:16
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Forms
' ITA 1177 "Werkstattzuordnungsliste II" - testfähige Version
' 
' *****************  Version 1  *****************
' User: Uha          Date: 14.08.07   Time: 13:42
' Created in $/CKG/Applications/AppVW/AppVWWeb/Forms
' ITA 1177 "Werkstattzuordnungsliste II" - kompilierfähige Rohversion
' hinzugefügt
' 
' ************************************************
