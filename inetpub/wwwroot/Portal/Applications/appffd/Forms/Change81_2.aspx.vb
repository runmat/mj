Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Change81_2
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
    Private objHandler As MDR_06

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lnkKreditlimit As System.Web.UI.WebControls.HyperLink
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ucStyles As Styles
    Protected WithEvents cmdNeueVIN As System.Web.UI.WebControls.LinkButton
    Protected WithEvents txtVIN As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        lnkKreditlimit.NavigateUrl = "Change81.aspx?AppID=" & Session("AppID").ToString
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If Session("objHandler") Is Nothing Then
                Response.Redirect("Change81.aspx?AppID=" & Session("AppID").ToString)
            End If

            objHandler = CType(Session("objHandler"), MDR_06)

            If Not IsPostBack Then
                ddlPageSize.Items.Add("10")
                ddlPageSize.Items.Add("20")
                ddlPageSize.Items.Add("50")
                ddlPageSize.Items.Add("100")
                ddlPageSize.Items.Add("250")
                ddlPageSize.SelectedIndex = 2

                'DataGrid1.BackColor = System.Drawing.Color.DarkSeaGreen
                FillGrid(0, , True)
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Try
            Dim tmpDataView As New DataView()
            tmpDataView = objHandler.Fahrzeuge.DefaultView

            tmpDataView.RowFilter = ""
            Dim intFahrzeugBriefe As Int32 = tmpDataView.Count

            If intFahrzeugBriefe = 0 Then
                lblError.Text = "Bitte erfassen Sie zunächst Fahrgestellnummern zur Beauftragung."
                FillGrid(DataGrid1.CurrentPageIndex)
            Else
                Session("objHandler") = objHandler
                Response.Redirect("Change81_4.aspx?AppID=" & Session("AppID").ToString)
            End If
        Catch ex As Exception
            lblError.Text = "Beim Aufbau der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "", Optional ByVal fill As Boolean = False)
        Dim tmpDataView As New DataView()
        tmpDataView = objHandler.Fahrzeuge.DefaultView
        tmpDataView.RowFilter = ""

        If tmpDataView.Count = 0 Then
            DataGrid1.Visible = False
            cmdSave.Enabled = False
            ddlPageSize.Visible = False
            lblNoData.Text = "Keine Daten zur Anzeige gefunden."
            lblNoData.Visible = True
        Else
            DataGrid1.Visible = True
            cmdSave.Enabled = True
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

            lblNoData.Text = "Es wurde(n) " & tmpDataView.Count.ToString & " Fahrgestellnummer(n) erfasst."
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
            'Dim hyperlink As HyperLink
            'Dim control As Control

            'Dim strHistoryLink As String = ""
            'If m_User.Applications.Select("AppName = 'Report02'").Length > 0 Then
            '    strHistoryLink = "../../AppLeasePlan/Forms/Report02.aspx?AppID=" & m_User.Applications.Select("AppName = 'Report02'")(0)("AppID").ToString & "&VIN="
            'End If

            'For Each item In DataGrid1.Items
            '    cell = item.Cells(0)
            '    For Each control In cell.Controls
            '        If TypeOf control Is HyperLink Then
            '            hyperlink = CType(control, HyperLink)
            '            Select Case hyperlink.ID
            '                Case "VIN"
            '                    If strHistoryLink.Length > 0 Then
            '                        hyperlink.NavigateUrl = strHistoryLink & hyperlink.NavigateUrl
            '                    Else
            '                        hyperlink.NavigateUrl = ""
            '                    End If
            '            End Select
            '        End If
            '    Next
            'Next
        End If
    End Sub

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        Try
            DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
            FillGrid(0)
        Catch ex As Exception
            lblError.Text = "Beim Aufbau der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        Try
            FillGrid(e.NewPageIndex)
        Catch ex As Exception
            lblError.Text = "Beim Aufbau der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        Try
            FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
        Catch ex As Exception
            lblError.Text = "Beim Aufbau der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub cmdNeueVIN_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdNeueVIN.Click
        Try
            txtVIN.Text = generateFullChassisNum(txtVIN.Text)
            If Not txtVIN.Text.Trim(" "c).Length = 17 Then
                lblError.Text = "Bitte geben Sie eine 17-stellige Fahrgestellnummer ein."
            Else
                Dim tmpRows() As DataRow = objHandler.Fahrzeuge.Select("Chassis_Num='" & Left(txtVIN.Text.Trim(" "c), 17) & "'")
                If tmpRows.Length = 1 Then
                    lblError.Text = "Fahrgestellnummer wurde bereits erfasst."
                Else
                    Dim rowNew As DataRow = objHandler.Fahrzeuge.NewRow
                    rowNew("Chassis_Num") = Left(txtVIN.Text.Trim(" "c), 17)
                    rowNew("Ernam") = Left(m_User.UserName, 12)
                    rowNew("Konzs") = objHandler.SucheHaendlernummer
                    rowNew("Name1") = objHandler.EmpfaengerName
                    rowNew("City1") = objHandler.EmpfaengerOrt
                    rowNew("Post_Code1") = objHandler.EmpfaengerPLZ
                    rowNew("Street") = objHandler.EmpfaengerStrasse
                    rowNew("Vermarktungscode") = objHandler.Versandcode

                    rowNew("Versandart") = objHandler.Versandart

                    objHandler.Fahrzeuge.Rows.Add(rowNew)

                    FillGrid(0)
                    Session("objHandler") = objHandler
                    txtVIN.Text = "WF0_XX"
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Beim Hinzufügen der Fahrgestellnummer ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Function generateFullChassisNum(ByVal FGSNR As String) As String
        'Durch der Platzhalter _ soll durch die 10. Stelle einer kompletten Fahrgestelltnumemr ersetzt werden. JJ2007.11.28

        If FGSNR.Trim(" "c).Length = 17 AndAlso Not FGSNR.IndexOf("_") = -1 AndAlso FGSNR.IndexOf("_") = 3 Then
            FGSNR = FGSNR.Replace("_", FGSNR.Chars(9))
        End If

        Return FGSNR

    End Function

    Private Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.ItemCommand
        Try
            Select Case e.CommandName
                Case "Delete"
                    Dim cell As TableCell = e.Item.Cells(0)
                    Dim control As Control

                    For Each control In cell.Controls
                        If TypeOf control Is HyperLink Then
                            Dim strVIN As String = "Chassis_Num='" & CType(control, HyperLink).Text & "'"

                            Dim tmpRows() As DataRow = objHandler.Fahrzeuge.Select(strVIN)
                            If tmpRows.Length = 1 Then
                                objHandler.Fahrzeuge.Rows.Remove(tmpRows(0))

                                FillGrid(0)
                                Session("objHandler") = objHandler
                                txtVIN.Text = ""
                            End If
                        End If
                    Next
                Case Else
                    'Tu nix
            End Select
        Catch ex As Exception
            lblError.Text = "Beim Löschen der Fahrgestellnummer ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub
End Class

' ************************************************
' $History: Change81_2.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 30.04.09   Time: 14:43
' Updated in $/CKAG/Applications/appffd/Forms
' ITA: 2837
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 26.11.08   Time: 10:28
' Updated in $/CKAG/Applications/appffd/Forms
' ITA 2382 testfertig
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:23
' Created in $/CKAG/Applications/appffd/Forms
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 28.11.07   Time: 17:26
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 28.11.07   Time: 16:43
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' 
' *****************  Version 4  *****************
' User: Uha          Date: 22.08.07   Time: 13:43
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' Link auf Fahrzeughistorie aus Datagrid1 in Change81_2 entfernt
' 
' *****************  Version 3  *****************
' User: Uha          Date: 22.08.07   Time: 13:23
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' ITA 1208: Bugfixing 1
' 
' *****************  Version 2  *****************
' User: Uha          Date: 22.08.07   Time: 12:30
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' ITA 1208 Testversion
' 
' *****************  Version 1  *****************
' User: Uha          Date: 21.08.07   Time: 17:37
' Created in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' ITA 1208: Kompilierfähige Vorversion mit Teilfunktionalität
' 
' ************************************************
