Imports CKG
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Services


Partial Public Class Report05
    Inherits System.Web.UI.Page

#Region "Declarations"
    Private m_App As Security.App
    Private m_User As Security.User
    Private objHistorie As Historie

    Protected WithEvents GridNavigation1 As Global.CKG.Services.GridNavigation

#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)
        m_App = New Security.App(m_User)
        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

        GridNavigation1.setGridElment(gvSelectOne)


        If Not IsPostBack Then
            txtReference.Focus()
        End If

        lblError.Visible = False

        If (Not Request.QueryString("VIN") Is Nothing AndAlso Not Request.QueryString("VIN") Is String.Empty) Then
            txtFahrgestellnummer.Text = Request.QueryString("VIN")
            DoSubmit()
        End If

    End Sub

    Private Sub gvSelectOne_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvSelectOne.RowCommand
        If e.CommandName = "weiter" Then
            DoSubmit(e.CommandArgument.ToString)
        End If
    End Sub

    Private Sub GridNavigation1_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation1.PagerChanged
        FillGrid(PageIndex)
    End Sub

    Private Sub GridNavigation1_PageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillGrid(0)
    End Sub

    Private Sub gvSelectOne_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvSelectOne.Sorting
        FillGrid(0, e.SortExpression)
    End Sub

    Protected Sub lbCreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbCreate.Click
        DoSubmit()
    End Sub

    Private Sub lbBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbBack.Click
        Response.Redirect("/Services/(S(" & Session.SessionID & "))/Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub

    Private Sub btndefault_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btndefault.Click
        DoSubmit()
    End Sub
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Protected Sub ibtNewSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtNewSearch.Click
        DoSubmit()
        'divSelection.Visible = Not divSelection.Visible
        'divTrenn.Visible = Not divTrenn.Visible

    End Sub
#End Region


#Region "Methods"

    Private Sub DoSubmit(Optional ByVal GridFin As String = "")
        Dim strFileName As String = ""

        objHistorie = New Historie(m_User, m_App, strFileName)

        If txtReference.Text.Length = 0 Then
            objHistorie.Ordernummer = ""
        Else
            objHistorie.Ordernummer = txtReference.Text
        End If
        If txtNummerZB2.Text.Length = 0 Then
            objHistorie.Briefnummer = ""
        Else
            objHistorie.Briefnummer = txtNummerZB2.Text
        End If
        If GridFin.Length = 0 Then
            If txtFahrgestellnummer.Text.Length = 0 Then
                objHistorie.FahrgestellNr = ""
            Else
                objHistorie.FahrgestellNr = txtFahrgestellnummer.Text
            End If
        Else
            objHistorie.FahrgestellNr = GridFin
            objHistorie.Ordernummer = ""
            objHistorie.Briefnummer = ""
            objHistorie.Kennzeichen = ""
        End If

        If txtAmtlKennzeichen.Text.Length = 0 Then
            objHistorie.Kennzeichen = ""
        Else
            objHistorie.Kennzeichen = txtAmtlKennzeichen.Text
        End If

        objHistorie.FillHistory(Session("AppID").ToString, Session.SessionID.ToString, Me)

        If Not objHistorie.Status = 0 Then
            lblError.Text = objHistorie.Message
            lblError.Visible = True
        Else
            'wenn es mehrere DS gibt, ist die tabelle GT_EQUIS, die eine EQUINUMMER zurücklifert wenn aus dieser ein DS ausgewählt wurde. 
            If objHistorie.diverseFahrzeuge.Rows.Count < 2 OrElse Not GridFin.Length = 0 Then
                If (objHistorie.History Is Nothing) OrElse (objHistorie.History.Rows.Count = 0) Then
                    lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                    lblError.Visible = True
                Else
                    Session("AppHistoryTable") = objHistorie.History
                    Session("AppQMEL_DATENTable") = objHistorie.QMEL_DATENTable
                    Session("AppQMMIDATENTable") = objHistorie.QMMIDATENTable

                    Session("AppDiverseFahrzeuge") = objHistorie.diverseFahrzeuge
                    Session("objReport") = objHistorie


                    If Request.QueryString("LinkedApp") Is Nothing Then 'wenn aus hauptmenü aufgerufen
                        Response.Redirect("Report05_2.aspx?AppID=" & Session("AppID").ToString)
                    Else 'wenn verlinkung
                        Dim sUrl As String
                        sUrl = Request.QueryString.Item("LinkedApp")
                        Response.Redirect("Report05_2.aspx?AppID=" & Session("AppID").ToString & "&LinkedApp=" & sUrl)
                    End If
                End If
            Else
                Result.Visible = True
                gvSelectOne.DataSource = objHistorie.diverseFahrzeuge
                gvSelectOne.DataBind()
                Session("DataTable") = objHistorie.diverseFahrzeuge
                divSelection.Visible = False
                divTrenn.Visible = True
                lblNewSearch.Visible = True
                ibtNewSearch.Visible = True
            End If
        End If
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")

        Dim FahrzeugTable As DataTable = CType(Session("DataTable"), DataTable)

        Dim tmpDataView As New DataView()
        tmpDataView = FahrzeugTable.DefaultView

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

        gvSelectOne.PageIndex = intTempPageIndex
        gvSelectOne.DataSource = tmpDataView
        gvSelectOne.DataBind()


    End Sub
#End Region


End Class