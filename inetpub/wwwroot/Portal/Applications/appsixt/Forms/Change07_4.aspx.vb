Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Change07_4
    Inherits System.Web.UI.Page
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lnkKreditlimit As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private objBlocken As Sixt_B15
    Protected WithEvents cmdAbsenden As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdVerwerfen As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdWeitereAuswahl As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdZurueck As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdWeiter As System.Web.UI.WebControls.LinkButton
    Private logApp As Base.Kernel.Logging.Trace



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

        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If (Session("objBlocken") Is Nothing) OrElse (Not IsPostBack) Then
                objBlocken = New Sixt_B15(m_User, m_App, "")
            Else
                objBlocken = Session("objBlocken")
            End If

            If Not IsPostBack Then

                logApp = New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
                logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)

                ddlPageSize.Items.Add("10")
                ddlPageSize.Items.Add("20")
                ddlPageSize.Items.Add("50")
                ddlPageSize.Items.Add("100")
                ddlPageSize.Items.Add("200")
                ddlPageSize.Items.Add("500")
                ddlPageSize.Items.Add("1000")
                ddlPageSize.SelectedIndex = 2

                loadData()
                FillGrid(0, , True)
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try

    End Sub

    Private Sub loadData()
        Dim view As DataView

        objBlocken.GiveZumLoeschen(CInt(Session("AppID")), Session.SessionID, Me, "")


        view = objBlocken.Master.DefaultView

        DataGrid1.DataSource = view
        DataGrid1.DataBind()
        Session("objBlocken") = objBlocken    'Daten merken
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "", _
                        Optional ByVal fill As Boolean = False, _
                        Optional ByVal Filter As Boolean = False)

        Dim tmpDataView As New DataView()
        tmpDataView = objBlocken.Master.DefaultView


        tmpDataView.RowFilter = ""


        If Filter = True Then
            tmpDataView.RowFilter = "AnzahlLoeschen <> ''"
        End If


        If objBlocken.Master.Select("Status <> ''").Length > 0 Then
            DataGrid1.Columns(8).Visible = True
        End If


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

            lblNoData.Text = "Es wurde(n) " & tmpDataView.Count.ToString & " Regeln gefunden."
            lblNoData.Visible = True

            If DataGrid1.PageCount > 1 Then
                DataGrid1.PagerStyle.CssClass = "PagerStyle"
                DataGrid1.DataBind()
                DataGrid1.PagerStyle.Visible = True
            Else
                DataGrid1.PagerStyle.Visible = False
            End If

        End If
    End Sub



    Private Function CheckGrid() As Boolean
        Dim item As DataGridItem
        Dim cell As TableCell
        Dim txtBox As TextBox
        Dim control As Control
        Dim blnScriptFound As Boolean = False
        Dim strRegelID As String

        Dim tmpRows As DataRow()
        Dim strErr As String
        Dim booErr As Boolean
        Dim booChoose As Boolean


        For Each item In DataGrid1.Items



            strErr = String.Empty

            For Each cell In item.Cells


                strRegelID = "Regel_ID = '" & item.Cells(0).Text & "'"


                For Each control In cell.Controls
                    If TypeOf control Is TextBox Then
                        txtBox = CType(control, TextBox)

                        objBlocken.Master.AcceptChanges()
                        'Dim tmpRows As DataRow()
                        'Dim tmpRow As DataRow
                        tmpRows = objBlocken.Master.Select(strRegelID)
                        tmpRows(0).BeginEdit()

                        item.Cells(8).Text = String.Empty

                        If txtBox.Text.Trim.Length > 0 Then
                            booChoose = True

                            If IsNumeric(txtBox.Text) = False Then
                                strErr = "Kein nummerischer Wert."
                                item.Cells(8).Text = strErr
                                'DataGrid1.Columns(8).Visible = True
                                tmpRows(0)("status") = strErr
                                booErr = True
                            ElseIf CInt(txtBox.Text) > item.Cells(6).Text Then
                                strErr = "Max. Anzahl überschritten."
                                item.Cells(8).Text = strErr
                                'DataGrid1.Columns(8).Visible = True
                                tmpRows(0)("status") = strErr
                                booErr = True
                            Else
                                tmpRows(0)("status") = String.Empty
                            End If
                        End If

                        tmpRows(0)("AnzahlLoeschen") = txtBox.Text


                        tmpRows(0).EndEdit()
                        objBlocken.Master.AcceptChanges()
                    End If
                Next

            Next
        Next
        Session("objBlocken") = objBlocken

        If booChoose = False Then
            lblError.Text = "Bitte geben Sie die Anzahl der zu löschenden Regeln an."
            booErr = True
        End If

        Return booErr
    End Function

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(0)
    End Sub


    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub cmdWeiter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdWeiter.Click

        If CheckGrid() = False Then
            Me.cmdAbsenden.Visible = True
            Me.cmdWeiter.Visible = False
            Me.cmdVerwerfen.Visible = True
            Me.cmdWeitereAuswahl.Visible = True
            Me.cmdZurueck.Visible = False
            Me.DataGrid1.Enabled = False

            FillGrid(0, , , True)
        Else
            FillGrid(0, , , False)
        End If

    End Sub

    Private Sub cmdZurueck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdZurueck.Click
        Response.Redirect("Change07.aspx?AppID=" & Session("AppID").ToString)

    End Sub

    Private Sub cmdVerwerfen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdVerwerfen.Click
        loadData()
        Me.cmdAbsenden.Visible = False
        Me.cmdVerwerfen.Visible = False
        Me.cmdWeitereAuswahl.Visible = False
        Me.cmdWeiter.Visible = True
        Me.cmdZurueck.Visible = True
        Me.DataGrid1.Enabled = True

    End Sub

    Private Sub cmdWeitereAuswahl_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdWeitereAuswahl.Click
        FillGrid(0, , , False)
        Me.cmdAbsenden.Visible = False
        Me.cmdVerwerfen.Visible = False
        Me.cmdWeitereAuswahl.Visible = False
        Me.cmdWeiter.Visible = True
        Me.cmdZurueck.Visible = True
        Me.DataGrid1.Enabled = True

    End Sub

    Private Sub cmdAbsenden_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAbsenden.Click

        objBlocken.GeblockteLoeschen(CInt(Session("AppID")), Session.SessionID, Me)

        lblNoData.Text = "Die ausgewählten Regeln wurden gelöscht."

        Me.cmdAbsenden.Visible = False
        Me.cmdVerwerfen.Visible = False
        Me.cmdWeitereAuswahl.Visible = False
        Me.cmdWeiter.Visible = False
        Me.cmdZurueck.Visible = True


    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Change07_4.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 16.02.10   Time: 11:40
' Updated in $/CKAG/Applications/appsixt/Forms
' ITA: 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 4.05.09    Time: 10:16
' Updated in $/CKAG/Applications/appsixt/Forms
' ITA:2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 13:37
' Created in $/CKAG/Applications/appsixt/Forms
' 
' *****************  Version 4  *****************
' User: Uha          Date: 2.07.07    Time: 14:09
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 3  *****************
' User: Uha          Date: 22.05.07   Time: 11:27
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Forms
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 2  *****************
' User: Uha          Date: 21.05.07   Time: 15:45
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Forms
' History-Eintrag bei vb-Klassen hinzugefügt
' 
' ************************************************
