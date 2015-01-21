Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common

Partial Public Class Report05
    Inherits Page

#Region "Declarations"

    Private m_App As Security.App
    Private m_User As Security.User
    Protected WithEvents GridNavigation1 As CKG.Services.GridNavigation
    Private m_Track As Sendungsverfolgung

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        GridNavigation1.setGridElment(GridView1)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        m_App = New Security.App(m_User)

        If Not IsPostBack Then
            txtDateBis.Text = Now.ToShortDateString
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub GridNavigation1_ddlPageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillGrid(0)
    End Sub

    Private Sub GridView1_PageIndexChanged(ByVal pageindex As Int32) Handles GridNavigation1.PagerChanged
        GridView1.EditIndex = -1
        FillGrid(pageindex)
    End Sub

    Private Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles GridView1.RowDataBound
        m_Track = CType(Session("m_Track"), Sendungsverfolgung)

        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim dRow = CType(e.Row.DataItem, DataRowView)
            Dim sendNr As String = dRow("ZZTRACK").ToString()

            Dim ibutton As ImageButton = CType(e.Row.FindControl("lbSendung"), ImageButton)

            If Not String.IsNullOrEmpty(sendNr) Then
                Dim erfDat As Date = CDate(dRow("ERDAT"))

                'Ab 16.1.2015 UPS statt DHL
                Dim servicePartnerUrl As String = IIf(erfDat >= New Date(2015, 1, 16).Date, m_Track.Url_Ups, m_Track.Url_Dhl)
                ibutton.Attributes.Add("onclick", "javascript:openinfo('" & String.Format(servicePartnerUrl, sendNr) & "')")
            Else
                ibutton.Visible = False
            End If

        End If

    End Sub

    Private Sub GridView1_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs) Handles GridView1.Sorting
        FillGrid(GridView1.PageIndex, e.SortExpression)
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        m_Track = CType(Session("m_Track"), Sendungsverfolgung)
        Dim TrackTable As DataTable = m_Track.SendungsDaten

        If TrackTable IsNot Nothing Then

            If TrackTable.Rows.Count = 0 Then
                lblError.Visible = True
                lblError.Text = "Keine Daten zur Anzeige gefunden."
                Result.Visible = False
            Else
                Result.Visible = True
                lblError.Visible = False

                Dim tmpDataView As New DataView(TrackTable)

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

                GridView1.PageIndex = intTempPageIndex

                GridView1.DataSource = tmpDataView
                GridView1.DataBind()
                tab1.Visible = False
                btnConfirm.Visible = False
            End If

        Else
            lblError.Text = "Keine Daten zur Anzeige gefunden."
            Result.Visible = False
            lblError.Visible = True
            tab1.Visible = True
            btnConfirm.Visible = True
        End If
    End Sub

    Protected Sub btnConfirm_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConfirm.Click

        m_Track = New Sendungsverfolgung(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")

        m_Track.DatumVon = txtDateVon.Text
        If IsDate(txtDateBis.Text) Then
            m_Track.DatumBis = txtDateBis.Text
        Else
            m_Track.DatumBis = Now.ToShortDateString
        End If
        m_Track.Agentur = ""
        For i As Integer = 1 To txtOrgNr.Text.Length
            If IsNumeric(Mid(txtOrgNr.Text, i, 1)) = True Then
                m_Track.Agentur &= Mid(txtOrgNr.Text, i, 1)
            End If
        Next
        m_Track.GetData(CStr(Request.QueryString("AppID")), Session.SessionID.ToString, Page)

        Session("m_Track") = m_Track

        If m_Track.SendungsDaten Is Nothing OrElse m_Track.SendungsDaten.Rows.Count = 0 Then
            lblError.Visible = True
            Result.Visible = False
            lblError.Text = "Es wurden keine Daten zur Anzeige gefunden."
        Else
            FillGrid(0)
        End If

    End Sub

    Protected Sub NewSearch_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs) Handles NewSearch.Click
        tab1.Visible = Not tab1.Visible
        btnConfirm.Visible = Not btnConfirm.Visible
    End Sub

    Protected Sub lnkCreateExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel.Click
        m_Track = CType(Session("m_Track"), Sendungsverfolgung)

        If m_Track IsNot Nothing Then
            Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
            Dim reportExcel As DataTable = m_Track.ResultExcel
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
            excelFactory.CreateDocumentAndSendAsResponse(strFileName, reportExcel, Me)
        End If
    End Sub

    Protected Sub lbBack_Click(sender As Object, e As EventArgs) Handles lbBack.Click
        Response.Redirect("../../../Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub

End Class