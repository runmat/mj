Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Change01_2
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
    Private objFahrer As CK_01

    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lnkKreditlimit As System.Web.UI.WebControls.HyperLink
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ucHeader As Header
    Protected WithEvents lnkExcel As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents cmdConfirm As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdReset As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdBack As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblDownloadTip As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents ddlPageSizeNotInUse As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lbtnSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        lnkKreditlimit.NavigateUrl = "Change01.aspx?AppID=" & Session("AppID").ToString
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If Session("objFahrer") Is Nothing Then
                Response.Redirect("Change01.aspx?AppID=" & Session("AppID").ToString)
            End If

            objFahrer = CType(Session("objFahrer"), CK_01)

            If Not IsPostBack Then
                If Not Session("lnkExcel").ToString.Length = 0 Then
                    lblDownloadTip.Visible = True
                    lnkExcel.Visible = True
                    lnkExcel.NavigateUrl = Session("lnkExcel").ToString
                End If

                FillGrid(0)
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    'Private Sub DataGrid1_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
    '    FillGrid(e.NewPageIndex)
    'End Sub

    'Private Sub DataGrid1_SortCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
    '    FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    'End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        Dim tmpDataView As New DataView()
        tmpDataView = objFahrer.FahrerTabelle.DefaultView

        If tmpDataView.Count = 0 Then
            DataGrid1.Visible = False
            lblError.Text = "Keine Daten zur Anzeige gefunden."
            lblError.Visible = True

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
            If cmdConfirm.Visible Then
                DataGrid1.AllowPaging = False
                DataGrid1.AllowSorting = False
            Else
                DataGrid1.AllowPaging = True
                DataGrid1.AllowSorting = True
            End If
            DataGrid1.DataSource = tmpDataView
            DataGrid1.DataBind()

            If DataGrid1.PageCount > 1 Then
                DataGrid1.PagerStyle.CssClass = "PagerStyle"
                DataGrid1.DataBind()
                DataGrid1.PagerStyle.Visible = True
            Else
                DataGrid1.PagerStyle.Visible = False
            End If

            Dim item As DataGridItem
            Dim datVerfdat As Date

            For Each item In DataGrid1.Items

                datVerfdat = CDate(item.Cells(0).Text)
                If datVerfdat.DayOfWeek = DayOfWeek.Saturday Or datVerfdat.DayOfWeek = DayOfWeek.Sunday Then
                    item.Cells(0).ForeColor = System.Drawing.Color.Red
                End If
            Next
        End If
    End Sub

    Private Sub lbtnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnSave.Click


        Dim item As DataGridItem
        Dim txt As TextBox
        Dim booError As Boolean = False


        lblError.Visible = False
        lblError.Text = ""
        lblNoData.Visible = False
        lblNoData.Text = ""

        objFahrer.ExportTable = New DataTable
        objFahrer.ExportTable.Columns.Add("VERFDAT", GetType(System.DateTime))
        objFahrer.ExportTable.Columns.Add("ANZ_FAHRER", GetType(System.String))


        objFahrer.ExportTable.AcceptChanges()

        Dim NewRow As DataRow

        For Each item In DataGrid1.Items

            NewRow = objFahrer.ExportTable.NewRow

            txt = CType(item.FindControl("txtAnzahl"), TextBox)

            txt.BorderColor = Color.Empty

            If txt.Enabled = True Then

                If Trim(txt.Text) <> "" Then

                    'If IsNumeric(txt.Text) = True Then

                    If txt.Text.Contains(",") OrElse (txt.Text.Length = 2 AndAlso Left(txt.Text, 1) = 0) Then
                        booError = True
                        txt.BorderColor = Color.Red
                    End If

                    If IsNumeric(txt.Text) = False Then
                        txt.Text = txt.Text.ToUpper
                    End If


                    If IsNumeric(txt.Text) = False AndAlso (txt.Text <> "U" AndAlso txt.Text <> "K" AndAlso txt.Text <> "I") Then
                        booError = True
                        txt.BorderColor = Color.Red
                    End If


                    NewRow("ANZ_FAHRER") = txt.Text





                    'Else
                    '    booError = True
                    '    txt.BorderColor = Color.Red
                    'End If


                End If
            Else
                '2 Leerzeichen einfügen(SAP-Initial)
                NewRow("ANZ_FAHRER") = "  "
            End If

            NewRow("VERFDAT") = CDate(item.Cells(0).Text)

            objFahrer.ExportTable.Rows.Add(NewRow)
            objFahrer.ExportTable.AcceptChanges()

        Next


        If booError = True Then
            lblError.Visible = True
            lblError.Text = "Es sind numerische Werte(Ganzzahlen) erlaubt sowie die Buchstaben U, K oder I. Die Daten werden nicht gespeichert."
            objFahrer.ExportTable = Nothing
        Else
            objFahrer.ChangeNew(Session("AppID").ToString, Session.SessionID.ToString)

            If objFahrer.Status <> -9999 Then
                lblNoData.Visible = True
                lblNoData.Text = "Die Daten wurden gespeichert."
                DataGrid1.Enabled = False
                lbtnSave.Visible = False
            Else
                lblError.Visible = True
                lblError.Text = objFahrer.Message

            End If
            
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
' $History: Change01_2.aspx.vb $
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 4.10.10    Time: 17:49
' Updated in $/CKAG/Applications/AppKroschke/Forms
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 13.01.10   Time: 10:30
' Updated in $/CKAG/Applications/AppKroschke/Forms
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 12.01.10   Time: 17:18
' Updated in $/CKAG/Applications/AppKroschke/Forms
' ITA: 3332
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 14.10.08   Time: 13:07
' Updated in $/CKAG/Applications/AppKroschke/Forms
' ITA: 2301 & Warnungen bereinigt
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 14:30
' Created in $/CKAG/Applications/AppKroschke/Forms
' 
' *****************  Version 6  *****************
' User: Uha          Date: 2.07.07    Time: 13:07
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 5  *****************
' User: Uha          Date: 8.03.07    Time: 9:25
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb/Forms
' 
' ************************************************
