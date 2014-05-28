Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common

Partial Public Class Report04
    Inherits System.Web.UI.Page

#Region "Declarations"
    Private m_App As Security.App
    Private m_User As Security.User
    Protected WithEvents GridNavigation1 As Global.CKG.Services.GridNavigation
    Private m_change As VFS03

#End Region


#Region "Events"
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

        If e.CommandName = "Update" Then

            Dim index As Integer = CType(e.CommandArgument, Integer)

            lblIndex.Text = index.ToString

            lblAgentur.Text = GridView1.Rows(index).Cells(1).Text

            Dim lbl As Label

            lbl = CType(GridView1.Rows(index).FindControl("lblSPERRE_WEB_ID"), Label)

            lblAuftragsnummer.Text = lbl.Text

            lbl = CType(GridView1.Rows(index).FindControl("lblANZ_APVTG"), Label)

            lblAnzahlAAP.Text = lbl.Text

            lbl = CType(GridView1.Rows(index).FindControl("lblERDAT"), Label)

            lblBeauftragtAm.Text = CDate(lbl.Text).ToShortDateString.ToString & " "

            lbl = CType(GridView1.Rows(index).FindControl("lblERZET"), Label)

            lblBeauftragtAm.Text &= Left(lbl.Text, 2) & ":" & Mid(lbl.Text, 3, 2) & ":" & Right(lbl.Text, 2)


            lbl = CType(GridView1.Rows(index).FindControl("lblANZAHL"), Label)

            lblMenge.Text = lbl.Text

            Dim Adresse As String

            lbl = CType(GridView1.Rows(index).FindControl("lblANRED"), Label)

            Adresse = lbl.Text & "<br>"


            Adresse &= GridView1.Rows(index).Cells(3).Text & " "


            Adresse &= GridView1.Rows(index).Cells(4).Text & "<br>"

            'lbl = CType(GridView1.Rows(index).FindControl("lblSTREET"), Label)

            'Adresse &= lbl.Text & " "


            lbl = CType(GridView1.Rows(index).FindControl("lblSTREET"), Label)

            Adresse &= lbl.Text & " "

            lbl = CType(GridView1.Rows(index).FindControl("lblHOUSE_NUM1"), Label)

            Adresse &= lbl.Text & "<br><br>"

            lbl = CType(GridView1.Rows(index).FindControl("lblPOST_CODE1"), Label)

            Adresse &= lbl.Text & " "

            lbl = CType(GridView1.Rows(index).FindControl("lblCITY1"), Label)

            Adresse &= lbl.Text & "<br><br>"


            Adresse &= "Telefon: "

            lbl = CType(GridView1.Rows(index).FindControl("lblTELF1"), Label)

            'Adresse &= lbl.Text & "<br>"

            'Adresse &= "E-Mail: "

            lblAdresse.Text = Adresse

            lbl = CType(GridView1.Rows(index).FindControl("lblSMTP_ADDR"), Label)

            lblEmail.Text = lbl.Text

            'Abweichende Adresse
            Dim AdresseWE As String

            lbl = CType(GridView1.Rows(index).FindControl("lblWE_ANRED"), Label)

            AdresseWE = lbl.Text & "<br>"

            lbl = CType(GridView1.Rows(index).FindControl("lblWE_NAME1"), Label)

            AdresseWE &= lbl.Text & " "

            lbl = CType(GridView1.Rows(index).FindControl("lblWE_NAME2"), Label)

            AdresseWE &= lbl.Text & "<br>"

            lbl = CType(GridView1.Rows(index).FindControl("lblWE_STREET"), Label)

            AdresseWE &= lbl.Text & " "

            lbl = CType(GridView1.Rows(index).FindControl("lblWE_HOUSE_NUM1"), Label)

            AdresseWE &= lbl.Text & "<br>"

            lbl = CType(GridView1.Rows(index).FindControl("lblWE_POST_CODE1"), Label)

            AdresseWE &= lbl.Text & " "

            lbl = CType(GridView1.Rows(index).FindControl("lblWE_CITY1"), Label)

            AdresseWE &= lbl.Text & "<br><br>"

            lbl = CType(GridView1.Rows(index).FindControl("lblWE_TELF1"), Label)


            If lbl.Text.Length > 0 Then
                AdresseWE &= "Telefon: "
                AdresseWE &= lbl.Text
            End If

            lblAdresse.Text = Adresse
            lblAdresseWe.Text = AdresseWE

            ModalPopupExtender2.Show()


        End If


    End Sub

    Private Sub GridView1_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles GridView1.RowUpdating

    End Sub

    Private Sub GridView1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
        FillGrid(GridView1.PageIndex)
    End Sub


#End Region
    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")

        Dim AdressTable As DataTable

        AdressTable = CType(Session("Frei_Loesch_Liste"), DataTable)

        If Not AdressTable Is Nothing Then

            If AdressTable.Rows.Count = 0 Then
                lblError.Visible = True
                lblError.Text = "Keine Daten zur Anzeige gefunden."
                Result.Visible = False
            Else
                Result.Visible = True
                DivPlaceholder.Visible = False
                lblError.Visible = False

                Dim tmpDataView As New DataView(AdressTable)

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
            DivPlaceholder.Visible = True
            lblError.Visible = True
            tab1.Visible = True
            btnConfirm.Visible = True
        End If
    End Sub

    Protected Sub btnConfirm_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConfirm.Click

        Session("Frei_Loesch_Liste") = Nothing

        m_change = New VFS03(m_User, m_App, CStr(Request.QueryString("AppID")), Session.SessionID.ToString, "")

        m_change.DatumVon = txtDateVon.Text
        If IsDate(txtDateBis.Text) Then
            m_change.DatumBis = txtDateBis.Text
        Else
            m_change.DatumBis = Now.ToShortDateString
        End If
        m_change.StatusFilter = ""
        If Not ddlStatus.SelectedValue = "0" Then
            m_change.StatusFilter = ddlStatus.SelectedValue
        End If

        m_change.GetFreig_Loesch_Liste(CStr(Request.QueryString("AppID")), Session.SessionID.ToString, Me.Page)

        If m_change.Frei_Loesch_Liste Is Nothing OrElse m_change.Frei_Loesch_Liste.Rows.Count = 0 Then
            lblError.Visible = True
            Result.Visible = False
            lblError.Text = "Es wurden keine Daten zur Anzeige gefunden."
        Else

            If Session("Frei_Loesch_Liste") Is Nothing Then
                Session.Add("Frei_Loesch_Liste", m_change.Frei_Loesch_Liste)
            End If
            If Session("ResultExcel") Is Nothing Then
                Session.Add("ResultExcel", m_change.ResultExcel)
            End If

            FillGrid(0)

        End If
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        ModalPopupExtender2.Show()
    End Sub


    Private Sub UpdateValues(ByVal Status As String)

        GridView1.Rows(CInt(lblIndex.Text)).Cells(23).Text = Status

        Dim btn As ImageButton

        btn = CType(GridView1.Rows(CInt(lblIndex.Text)).FindControl("lbSave"), ImageButton)

        btn.Visible = False

        Dim TempTable As DataTable = CType(Session("Frei_Loesch_Liste"), DataTable)

        Dim RowSet As DataRow()

        RowSet = TempTable.Select("SPERRE_WEB_ID = '" & lblAuftragsnummer.Text & "'")

        RowSet(0).Item("STATUS_TEXT") = Status

        TempTable.AcceptChanges()

        Session("Frei_Loesch_Liste") = TempTable

        GridView1.Rows(CInt(lblIndex.Text)).Cells(GridView1.Columns.Count - 1).Text = Status

    End Sub

    Protected Sub lbBack_Click(sender As Object, e As EventArgs) Handles lbBack.Click
        Response.Redirect("../../../Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub

    Private Sub lnkCreateExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkCreateExcel.Click

        Dim reportExcel As DataTable
        Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
        Dim sPath As String = ConfigurationManager.AppSettings("ExcelPath")
        reportExcel = CType(Session("ResultExcel"), DataTable)

        reportExcel.AcceptChanges()
        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
        ' excelFactory.CreateDocumentAndWriteToFilesystemWithPath(sPath + strFileName, reportExcel)
        excelFactory.CreateDocumentAndSendAsResponse(strFileName, reportExcel, Me)
    End Sub

    Private Sub NewSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles NewSearch.Click
        tab1.Visible = Not tab1.Visible
        btnConfirm.Visible = Not btnConfirm.Visible
    End Sub

End Class