Option Explicit On
Option Strict On
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common

Public Class Change03
    Inherits Page

#Region "Declarations"

    Private m_App As Security.App
    Private m_User As Security.User
    Protected WithEvents GridNavigation1 As CKG.Services.GridNavigation
    Private m_change As VFS03

#End Region

#Region "Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        GridNavigation1.setGridElment(GridView1)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        m_App = New Security.App(m_User)
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

    Private Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles GridView1.RowCommand

        If e.CommandName = "Update" Then

            Dim index As Integer = CType(e.CommandArgument, Integer)

            lblIndex.Text = index.ToString

            lblAgentur.Text = GridView1.Rows(index).Cells(1).Text

            Dim lbl As Label

            lbl = CType(GridView1.Rows(index).FindControl("lblSPERRE_WEB_ID"), Label)

            lblAuftragsnummer.Text = lbl.Text

            lbl = CType(GridView1.Rows(index).FindControl("lblANZ_APVTG"), Label)

            txtAnzalAAP.Text = lbl.Text

            lbl = CType(GridView1.Rows(index).FindControl("lblERDAT"), Label)

            lblBeauftragtAm.Text = CDate(lbl.Text).ToShortDateString.ToString & " "

            lbl = CType(GridView1.Rows(index).FindControl("lblERZET"), Label)

            lblBeauftragtAm.Text &= Left(lbl.Text, 2) & ":" & Mid(lbl.Text, 3, 2) & ":" & Right(lbl.Text, 2)


            lbl = CType(GridView1.Rows(index).FindControl("lblANZAHL"), Label)
            ddlAnzahlKennzeichen.Items.Clear()
            Dim item As ListItem
            Dim intLoop As Integer
            For intLoop = 1 To 19
                item = New ListItem(intLoop.ToString, intLoop.ToString)
                ddlAnzahlKennzeichen.Items.Add(item)
            Next
            For intLoop = 20 To 75 Step 5
                item = New ListItem(intLoop.ToString, intLoop.ToString)
                ddlAnzahlKennzeichen.Items.Add(item)
            Next
            For intLoop = 100 To 150 Step 25
                item = New ListItem(intLoop.ToString, intLoop.ToString)
                ddlAnzahlKennzeichen.Items.Add(item)
            Next
            ddlAnzahlKennzeichen.SelectedValue = lbl.Text

            Dim Adresse As String

            lbl = CType(GridView1.Rows(index).FindControl("lblANRED"), Label)

            Adresse = lbl.Text & "<br>"


            Adresse &= GridView1.Rows(index).Cells(3).Text & " "


            Adresse &= GridView1.Rows(index).Cells(4).Text & "<br />"

            Adresse &= GridView1.Rows(index).Cells(5).Text & "<br />"

            lbl = CType(GridView1.Rows(index).FindControl("lblSTREET"), Label)

            Adresse &= lbl.Text & " "

            lbl = CType(GridView1.Rows(index).FindControl("lblHOUSE_NUM1"), Label)

            Adresse &= lbl.Text & "<br/><br/>"

            lbl = CType(GridView1.Rows(index).FindControl("lblPOST_CODE1"), Label)

            Adresse &= lbl.Text & " "

            lbl = CType(GridView1.Rows(index).FindControl("lblCITY1"), Label)

            Adresse &= lbl.Text & "<br/><br/>"


            Adresse &= "Telefon: "

            lbl = CType(GridView1.Rows(index).FindControl("lblTELF1"), Label)

            Adresse &= lbl.Text & "<br/><br/>"

            lblAdresse.Text = Adresse

            lbl = CType(GridView1.Rows(index).FindControl("lblSMTP_ADDR"), Label)

            txtEmailAdresse.Text = lbl.Text


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

            lblAdresseWe.Text = AdresseWE

            lblLastOrder.Text = ""
            lbl = CType(GridView1.Rows(index).FindControl("lblDAT_LETZT_BEST"), Label)
            If IsDate(lbl.Text) Then lblLastOrder.Text = CDate(lbl.Text).ToShortDateString
            If lblLastOrder.Text.Trim.Length <> 0 Then
                lbl = CType(GridView1.Rows(index).FindControl("lblUZEIT_LETZT_BEST"), Label)
                lblLastOrder.Text = lblLastOrder.Text & " " & lbl.Text
            End If

            Dim Sperre As String

            lbl = CType(GridView1.Rows(index).FindControl("lblSTATUS_TEXT"), Label)
            Sperre = lbl.Text
            lblLastOrder2.Text = Sperre

            disableControls(False)
            mb.Visible = True

        End If


    End Sub

    Private Sub GridView1_RowUpdating(ByVal sender As Object, ByVal e As GridViewUpdateEventArgs) Handles GridView1.RowUpdating

    End Sub

    Private Sub GridView1_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs) Handles GridView1.Sorting
        FillGrid(GridView1.PageIndex)
    End Sub

#End Region

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")

        Dim AdressTable As DataTable

        'If Not Session("LastOrderAdress") Is Nothing Then

        'End If

        AdressTable = CType(Session("LastOrderAdress"), DataTable)

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
        m_change = New VFS03(m_User, m_App, CStr(Request.QueryString("AppID")), Session.SessionID.ToString, "")

        m_change.SperreGrund = RadioButtonList1.SelectedValue

        m_change.GetGesperrte(CStr(Request.QueryString("AppID")), Session.SessionID.ToString, Page)

        If m_change.LastOrderAdress Is Nothing OrElse m_change.LastOrderAdress.Rows.Count = 0 Then
            lblError.Text = "Es wurden keine Daten zur Anzeige gefunden."
            lblError.Visible = True
            Result.Visible = False
        Else
            Session.Add("LastOrderAdress", m_change.LastOrderAdress)
            Session.Add("ResultExcel", m_change.ResultExcel)
            FillGrid(0)

        End If
    End Sub

    Private Sub disableControls(ByVal Enable As Boolean)
        GridView1.Enabled = Enable
        NewSearch.Enabled = Enable
        RadioButtonList1.Enabled = Enable
        btnConfirm.Enabled = Enable
        lbBack.Enabled = Enable
        Dim ddl As DropDownList
        ddl = CType(GridNavigation1.FindControl("ddlPageSize"), DropDownList)
        If Not ddl Is Nothing Then ddl.Enabled = Enable
        lnkCreateExcel.Enabled = Enable

    End Sub

    Protected Sub btnOK_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnOK.Click


        m_change = New VFS03(m_User, m_App, CStr(Request.QueryString("AppID")), Session.SessionID.ToString, "")


        Dim sAgentur As String = ""
        Dim sNummerTrenn As String = ""

        For i As Integer = 1 To lblAgentur.Text.Length
            If IsNumeric(Mid(lblAgentur.Text, i, 1)) = True Then
                sNummerTrenn = lblAgentur.Text
                sAgentur &= Mid(lblAgentur.Text, i, 1)
            End If

        Next

        m_change.EmailAdresse = txtEmailAdresse.Text
        m_change.AnzahlKennzeichen = CInt(ddlAnzahlKennzeichen.SelectedItem.Value)
        Dim ErrorInfo As String = ""
        lblPoUpErr.Text = ""
        If txtAnzalAAP.Text.Trim.Length > 0 Then
            If IsNumeric(txtAnzalAAP.Text) Then
                If CInt(m_change.AnzahlKennzeichen) < CInt(txtAnzalAAP.Text) Then
                    ErrorInfo &= """Anzahl aap-Vordrucke"" darf max. ""Anzahl Kennzeichen"" betragen."
                Else
                    m_change.AnzahlAAP = CInt(txtAnzalAAP.Text)
                End If
            Else
                ErrorInfo &= "Bitte im Feld ""Anzahl aap-Vordrucke"" einen numerischen Wert eingeben."
            End If
        Else
            ErrorInfo &= "Bitte im Feld ""Anzahl aap-Vordrucke"" einen numerischen Wert eingeben."
        End If
        If ErrorInfo.Length = 0 Then
            m_change.Freigabe(sAgentur, lblAuftragsnummer.Text, Page)

            UpdateValues("Freigegeben")

            disableControls(True)
            mb.Visible = False
        Else
            lblPoUpErr.Text = ErrorInfo
            disableControls(False)
            mb.Visible = True
        End If

    End Sub

    Protected Sub btnStorno_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnStorno.Click

        m_change = New VFS03(m_User, m_App, CStr(Request.QueryString("AppID")), Session.SessionID.ToString, "")

        Dim sAgentur As String = ""
        Dim sNummerTrenn As String = ""

        For i As Integer = 1 To lblAgentur.Text.Length
            If IsNumeric(Mid(lblAgentur.Text, i, 1)) = True Then
                sNummerTrenn = lblAgentur.Text
                sAgentur &= Mid(lblAgentur.Text, i, 1)
            End If

        Next

        m_change.Storno(sAgentur, lblAuftragsnummer.Text, Page)

        UpdateValues("Storniert")

        disableControls(True)
        mb.Visible = False
    End Sub

    Private Sub UpdateValues(ByVal Status As String)

        GridView1.Rows(CInt(lblIndex.Text)).Cells(GridView1.Columns.Count - 1).Text = Status

        Dim btn As ImageButton

        btn = CType(GridView1.Rows(CInt(lblIndex.Text)).FindControl("lbSave"), ImageButton)

        btn.Visible = False

        Dim TempTable As DataTable = CType(Session("LastOrderAdress"), DataTable)

        Dim RowSet As DataRow()

        RowSet = TempTable.Select("SPERRE_WEB_ID = '" & lblAuftragsnummer.Text & "'")

        RowSet(0).Item("STATUS_TEXT") = Status

        TempTable.AcceptChanges()

        Session("LastOrderAdress") = TempTable

        GridView1.Rows(CInt(lblIndex.Text)).Cells(GridView1.Columns.Count - 1).Text = Status

    End Sub

    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        Response.Redirect("../../../Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub

    Private Sub lnkCreateExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel.Click

        Dim reportExcel As DataTable
        Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
        reportExcel = CType(Session("ResultExcel"), DataTable)

        reportExcel.AcceptChanges()
        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
        ' excelFactory.CreateDocumentAndWriteToFilesystemWithPath(sPath + strFileName, reportExcel)
        excelFactory.CreateDocumentAndSendAsResponse(strFileName, reportExcel, Me)
    End Sub

    Private Sub NewSearch_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs) Handles NewSearch.Click
        tab1.Visible = Not tab1.Visible
        btnConfirm.Visible = Not btnConfirm.Visible
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
        disableControls(True)
        mb.Visible = False
    End Sub

End Class
