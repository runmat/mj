Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Business

Partial Public Class Report02
    Inherits System.Web.UI.Page

#Region "Declarations"

    Protected WithEvents GridNavigation1 As CKG.Services.GridNavigation
    Private m_App As App
    Private m_User As User

#End Region

#Region "Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)
        GridNavigation1.setGridElment(gvSelectOne)
        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        m_App = New App(m_User)

        divSpace.Visible = False

        If (Not Request.QueryString("VIN") Is Nothing AndAlso Not Request.QueryString("VIN") Is String.Empty) Then
            txtFahrgestellnummer.Text = Request.QueryString("VIN")
            DoSubmit()
        End If

    End Sub

    Private Sub lb_Create_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbCreate.Click
        Session("ShowLink") = "True"
        DoSubmit()
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

    Protected Sub ibtNewSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtNewSearch.Click
        divSelection.Visible = True
    End Sub

    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click

        If Not Request.QueryString.Item("LinkedApp") Is Nothing Then
            Dim sUrl As String
            sUrl = Request.QueryString.Item("LinkedApp")
            Response.Redirect("../../" & sUrl, False)

        ElseIf Request.UrlReferrer.OriginalString.Contains("Report02.aspx") Then
            Response.Redirect("../../../Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)

        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
        HelpProcedures.FixedGridViewCols(gvSelectOne)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

#End Region

#Region "Methods"

    Private Sub DoSubmit(Optional ByVal equnr As String = "")

        Session("DataTable") = Nothing

        Dim m_Report As New Historie(m_User, m_App, "")

        Dim strBriefnummer As String
        Dim strFahrgestellnummer As String
        Dim strAmtlKennzeichen As String
        Dim strOrdernummer As String

        If txtOrdernummer.Text.Length = 0 Then
            strOrdernummer = ""
        Else
            strOrdernummer = txtOrdernummer.Text
        End If
        If txtBriefnummer.Text.Length = 0 Then
            strBriefnummer = ""
        Else
            strBriefnummer = txtBriefnummer.Text
        End If
        If txtFahrgestellnummer.Text.Length = 0 Then
            strFahrgestellnummer = ""
        Else
            strFahrgestellnummer = txtFahrgestellnummer.Text
        End If
        If txtAmtlKennzeichen.Text.Length = 0 Then
            strAmtlKennzeichen = ""
        Else
            strAmtlKennzeichen = txtAmtlKennzeichen.Text
        End If

        If txtBriefnummer.Text.Length + txtFahrgestellnummer.Text.Length + txtAmtlKennzeichen.Text.Length + txtOrdernummer.Text.Length + equnr.Length = 0 Then
            lblError.Text = "Bitte geben Sie ein Suchkriterium ein."
        Else
            m_Report.Fill(Session("AppID").ToString, Session.SessionID.ToString, strAmtlKennzeichen, strFahrgestellnummer, strBriefnummer, strOrdernummer, equnr)
            If Not m_Report.Status = 0 Then
                lblError.Text = m_Report.Message
            Else
                'wenn es mehrere DS gibt, ist die tabelle GT_EQUIS, die eine EQUINUMMER zurücklifert wenn aus dieser ein DS ausgewählt wurde. 
                If m_Report.diverseFahrzeuge.Rows.Count < 2 OrElse Not equnr.Length = 0 Then
                    If (m_Report.BRIEFLEBENSLAUF_LPTable Is Nothing) OrElse (m_Report.BRIEFLEBENSLAUF_LPTable.Rows.Count = 0) Then
                        lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                    Else
                        Session("BRIEFLEBENSLAUF_LPTable") = m_Report.BRIEFLEBENSLAUF_LPTable
                        Session("QMEL_DATENTable") = m_Report.QMEL_DATENTable
                        Session("QMMIDATENTable") = m_Report.QMMIDATENTable
                        Session("LLSCHDATENTable") = m_Report.LLSCHDATENTable
                        Session("GT_ADDR") = m_Report.GT_ADDR

                        If Not m_Report.AnzBemerkungen Is Nothing Then
                            Session("AnzBemerkungen") = m_Report.AnzBemerkungen
                        End If

                        Session("objReport") = m_Report

                        Dim strShowZweitschluessel As String = GetApplicationConfigValue("FzgHistorieServicesZweitschluesselAnzeigen", Session("AppID").ToString, m_User.Customer.CustomerId)
                        Session("History_ShowZweitschluessel") = (strShowZweitschluessel.ToLower() = "true")

                        If Request.QueryString("Linked") = "false" Then 'wenn aus hauptmenü aufgerufen
                            Response.Redirect("Report02_2.aspx?AppID=" & Session("AppID").ToString)
                        Else 'wenn verlinkung
                            Response.Redirect("Report02_2.aspx?AppID=" & Session("AppID").ToString & "&Linked=true" & IIf(Request.QueryString.Item("cw") Is Nothing, "", "&cw=true"))
                        End If

                    End If
                Else
                    Result.Visible = True
                    gvSelectOne.DataSource = m_Report.diverseFahrzeuge
                    gvSelectOne.DataBind()
                    Session("DataTable") = m_Report.diverseFahrzeuge
                    divSelection.Visible = False
                    divSpace.Visible = True
                    lblNewSearch.Visible = True
                    ibtNewSearch.Visible = True
                End If
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