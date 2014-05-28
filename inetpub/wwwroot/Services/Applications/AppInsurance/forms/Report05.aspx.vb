Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common

Partial Public Class Report05
    Inherits System.Web.UI.Page

#Region "Declarations"
    Private m_App As Security.App
    Private m_User As Security.User
    Protected WithEvents GridNavigation1 As Global.CKG.Services.GridNavigation
    Private m_Track As Sendungsverfolgung
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub GridNavigation1_ddlPageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillGrid(0)
    End Sub

    Private Sub GridView1_PageIndexChanged(ByVal pageindex As Int32) Handles GridNavigation1.PagerChanged
        GridView1.EditIndex = -1
        FillGrid(pageindex)
    End Sub

    Private Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand

        If e.CommandName = "Show" Then

            Dim strSendnumber As String = CType(e.CommandArgument, String)



        End If


    End Sub

    Private Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        'OnClick="javascript:openinfo('http://nolp.dhl.de/nextt-online-public/set_identcodes.do?lang=de&idc=' & '<%# Bind("ZZTRACK") %>' & );"
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim ibutton As ImageButton
            Dim lblSendnr As Label
            lblSendnr = CType(e.Row.FindControl("lblSendnr"), Label)
            ibutton = CType(e.Row.FindControl("lbSendung"), ImageButton)
            If lblSendnr.Text.Trim.Length > 0 Then
                ibutton.Attributes.Add("onclick", "javascript:openinfo('http://nolp.dhl.de/nextt-online-public/set_identcodes.do?lang=de&idc=" & lblSendnr.Text & "')")
            Else
                ibutton.Visible = False
            End If


        End If

    End Sub

    Private Sub GridView1_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles GridView1.RowUpdating

    End Sub

    Private Sub GridView1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
        FillGrid(GridView1.PageIndex, e.SortExpression)
    End Sub
    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")



        Dim TrackTable As DataTable

        m_Track = CType(Session("m_Track"), Sendungsverfolgung)
        TrackTable = m_Track.SendungsDaten
        If Not TrackTable Is Nothing Then

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
        Session("m_Track") = Nothing

        m_Track = New Sendungsverfolgung(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")

        m_Track.DatumVon = txtDateVon.Text
        If IsDate(txtDateBis.Text) Then
            m_Track.DatumBis = txtDateBis.Text
        Else
            m_Track.DatumBis = Now.ToShortDateString
        End If
        m_Track.Agentur = ""
        Dim sNummerTrenn As String = ""
        For i As Integer = 1 To txtOrgNr.Text.Length
            If IsNumeric(Mid(txtOrgNr.Text, i, 1)) = True Then
                sNummerTrenn = txtOrgNr.Text
                m_Track.Agentur &= Mid(txtOrgNr.Text, i, 1)
            End If
        Next
        m_Track.GetData(CStr(Request.QueryString("AppID")), Session.SessionID.ToString, Me.Page)

        If m_Track.SendungsDaten Is Nothing OrElse m_Track.SendungsDaten.Rows.Count = 0 Then
            lblError.Visible = True
            Result.Visible = False
            lblError.Text = "Es wurden keine Daten zur Anzeige gefunden."
        Else

            If Session("m_Track") Is Nothing Then
                Session.Add("m_Track", m_Track)
            End If
            If Session("ResultExcel") Is Nothing Then
                Session.Add("ResultExcel", m_Track.ResultExcel)
            End If
            FillGrid(0)

        End If
    End Sub

    Protected Sub NewSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles NewSearch.Click
        tab1.Visible = Not tab1.Visible
        btnConfirm.Visible = Not btnConfirm.Visible
    End Sub

    Protected Sub lnkCreateExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel.Click
        Dim reportExcel As DataTable
        Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
        Dim sPath As String = ConfigurationManager.AppSettings("ExcelPath")
        reportExcel = CType(Session("ResultExcel"), DataTable)

        reportExcel.AcceptChanges()
        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
        ' excelFactory.CreateDocumentAndWriteToFilesystemWithPath(sPath + strFileName, reportExcel)
        excelFactory.CreateDocumentAndSendAsResponse(strFileName, reportExcel, Me)
    End Sub

    Protected Sub lbBack_Click(sender As Object, e As EventArgs) Handles lbBack.Click
        Response.Redirect("../../../Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub
End Class