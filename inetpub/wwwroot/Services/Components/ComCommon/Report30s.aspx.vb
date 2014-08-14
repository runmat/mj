Imports CKG.Base.Business
Imports CKG.Base.Kernel.Common
Imports CKG.Base.Kernel.Security

Public Class Report30s1
    Inherits Page

    Private m_App As App
    Private m_User As User
    Private mExpress As Express

    Protected WithEvents GridNavigation1 As CKG.Services.GridNavigation

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = Common.GetUser(Me)
        Common.FormAuth(Me, m_User)
        Common.GetAppIDFromQueryString(Me)
        m_App = New App(m_User)
        GridNavigation1.setGridElment(GridView1)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

            m_App = New App(m_User)

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Common.SetEndASPXAccess(Me)
        HelpProcedures.FixedGridViewCols(GridView1)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        Common.SetEndASPXAccess(Me)
    End Sub

    Private Sub GridNavigation1_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation1.PagerChanged
        GridView1.PageIndex = PageIndex
        FillGrid(PageIndex)
    End Sub

    Private Sub GridNavigation1_PageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillGrid(0)
    End Sub

    Protected Sub lbCreate_Click(sender As Object, e As EventArgs) Handles lbCreate.Click

        If checkSearchParameters() Then

            mExpress = New Express(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")

            With mExpress
                .DatumAb = txtAbDatum.Text
                .DatumBis = txtBisDatum.Text
                .Kennzeichen = txtKennzeichen.Text
                .Fahrgestellnummer = txtChassisNum.Text
                .Name1 = txtName1.Text
                .Name2 = txtName2.Text
                .Strasse = txtStrasse.Text
                .PLZ = txtPLZ.Text
                .Ort = txtOrt.Text
            End With

            mExpress.GetVersendungen(Me.Page)

            If Not mExpress.TableVersendungen Is Nothing Then
                mExpress.GetVersandtexte(Me.Page)
                Session("Express") = mExpress
                FillGrid(0)
            Else
                lblError.Text = "Zu Ihrer Suche wurden keine Datensätze gefunden."

                HideGrid()
            End If
        Else
            HideGrid()
        End If

    End Sub

    Private Sub HideGrid()
        GridView1.Visible = False
        Result.Visible = False
        GridNavigation1.Visible = False
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        Dim tmpDataView As DataView = CType(Session("Express"), Express).TableVersendungen.DefaultView
        tmpDataView.RowFilter = ""

        If tmpDataView.Count = 0 Then
            HideGrid()
        Else
            Dim intTempPageIndex As Int32 = intPageIndex
            Dim strTempSort As String = ""
            Dim strDirection As String = ""
            GridView1.Visible = True
            Result.Visible = True

            GridNavigation1.Visible = True
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

            GridView1.DataSource = tmpDataView
            GridView1.DataBind()
            GridView1.PageIndex = intTempPageIndex

        End If
    End Sub

    Private Function checkSearchParameters() As Boolean
        ' Felder auf UpperCase für saubere Suche
        txtKennzeichen.Text = txtKennzeichen.Text.ToUpper
        txtChassisNum.Text = txtChassisNum.Text.ToUpper

        If txtAbDatum.Text <> String.Empty Or txtBisDatum.Text <> String.Empty Then
            If txtAbDatum.Text <> String.Empty And txtBisDatum.Text <> String.Empty Then
                If Not cv_DatumVon.IsValid Or Not cv_DatumBis.IsValid Then
                    Return False
                End If
            Else
                lblError.Text = "Geben Sie einen gültigen Datumsbereich (von-bis) ein."
                Return False
            End If
        ElseIf txtChassisNum.Text <> String.Empty Or txtKennzeichen.Text <> String.Empty Or txtName1.Text <> String.Empty Or txtName2.Text <> String.Empty Then
            Return True
        Else
            lblError.Text = "Geben Sie einen gültigen Datumsbereich (von-bis) oder eine Fahrgestellnummer, ein Kennzeichen oder einen Namen als Suchkriterium ein!"
            Return False
        End If

        Return True
    End Function

    Protected Sub ibtNewSearch_Click(sender As Object, e As ImageClickEventArgs) Handles ibtNewSearch.Click
        divSelection.Visible = Not divSelection.Visible
        lbCreate.Visible = Not lbCreate.Visible
    End Sub

    Private Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand
        mExpress = Session("Express")

        If e.CommandName = "Info" Then
            lblVersandstatus.Text = "Versandstatus"
            ResetExtender()

            Dim StatusTable As DataTable = mExpress.GetVersendungsStatus(e.CommandArgument.ToString(), Me)

            ModalPopupExtender2.Show()

            Dim intMaxStatus As Integer = -1
            Dim enthalteneStati As New List(Of Integer)

            For Each dr As DataRow In StatusTable.Rows

                Dim intStatusHier As Integer = 0
                Integer.TryParse(dr("STATUS_HIER").ToString(), intStatusHier)
                Dim strStatusCode As String = dr("STATUS_CODE").ToString()
                Dim strTrackingNr As String = dr("ZZTRACK").ToString()
                Dim strDatum As String = Left(dr("STATUS_DATE").ToString(), 10)
                Dim strZeit As String = Left(dr("STATUS_TIME").ToString(), 2) & ":" & Mid(dr("STATUS_TIME").ToString(), 3, 2)

                Select Case intStatusHier

                    Case 0
                        lblDat0.Text = strDatum
                        lblTime0.Text = strZeit

                    Case 1
                        lblDat1.Text = strDatum
                        lblTime1.Text = strZeit
                        lblMeld1.Text = strStatusCode
                        Image6.Visible = True
                        Image2.Visible = False

                    Case 2
                        lblDat2.Text = strDatum
                        lblTime2.Text = strZeit
                        lblMeld2.Text = strStatusCode
                        Image7.Visible = True
                        Image3.Visible = False

                    Case 3
                        lblDat3.Text = strDatum
                        lblTime3.Text = strZeit
                        lblMeld3.Text = strStatusCode
                        Image8.Visible = True
                        Image4.Visible = False

                    Case 4
                        lblDat4.Text = strDatum
                        lblTime4.Text = strZeit
                        lblMeld4.Text = strStatusCode
                        Image9.Visible = True
                        Image5.Visible = False

                    Case 5
                        trFehler.Visible = True
                        lblDat5.Text = strDatum
                        lblTime5.Text = strZeit
                        lblMeld5.Text = strStatusCode

                End Select

                If intStatusHier > intMaxStatus Then
                    ' Letzten gültigen Status schreiben
                    lblVersandstatus.Text = "Versandstatus (" & strStatusCode & ", Tracking-Nr: " & strTrackingNr & ")"
                    intMaxStatus = intStatusHier
                End If

                enthalteneStati.Add(intStatusHier)
            Next

            ' Erste Zeile füllen, falls nicht durch Statuszeile mit Status 0 geschehen
            If String.IsNullOrEmpty(lblDat0.Text) Then
                Dim row As DataRow = mExpress.TableVersendungen.Select("POOLNR='" & e.CommandArgument.ToString() & "'")(0)
                lblDat0.Text = Left(row("ZZLSDAT").ToString(), 10)
                lblTime0.Text = row("ZZLSTIM").ToString()
            End If

            ' ggf. übersprungene Stati ausblenden
            Select Case intMaxStatus

                Case 2
                    If Not enthalteneStati.Contains(1) Then
                        trStatus1.Visible = False
                    End If

                Case 3
                    If Not enthalteneStati.Contains(1) Then
                        trStatus1.Visible = False
                    End If
                    If Not enthalteneStati.Contains(2) Then
                        trStatus2.Visible = False
                    End If

                Case 4
                    If Not enthalteneStati.Contains(1) Then
                        trStatus1.Visible = False
                    End If
                    If Not enthalteneStati.Contains(2) Then
                        trStatus2.Visible = False
                    End If
                    If Not enthalteneStati.Contains(3) Then
                        trStatus3.Visible = False
                    End If

            End Select

        End If

    End Sub

    Private Sub ResetExtender()

        ' Zeile 0
        trStatus0.Visible = True
        lblDat0.Text = ""
        lblTime0.Text = ""

        ' Zeile 1
        trStatus1.Visible = True
        lblDat1.Text = ""
        lblTime1.Text = ""
        lblMeld1.Text = ""
        Image2.Visible = True
        Image6.Visible = False

        ' Zeile 2
        trStatus2.Visible = True
        lblDat2.Text = ""
        lblTime2.Text = ""
        lblMeld2.Text = ""
        Image3.Visible = True
        Image7.Visible = False

        ' Zeile 3
        trStatus3.Visible = True
        lblDat3.Text = ""
        lblTime3.Text = ""
        lblMeld3.Text = ""
        Image4.Visible = True
        Image8.Visible = False

        ' Zeile 4
        trStatus4.Visible = True
        lblDat4.Text = ""
        lblTime4.Text = ""
        lblMeld4.Text = ""
        Image5.Visible = True
        Image9.Visible = False

        ' Zeile 5 - Fehler
        trFehler.Visible = False
        lblDat5.Text = ""
        lblTime5.Text = ""
        lblMeld5.Text = ""

    End Sub

    Private Sub GridView1_Sorting(sender As Object, e As GridViewSortEventArgs) Handles GridView1.Sorting
        FillGrid(GridView1.PageIndex, e.SortExpression)
    End Sub

    Protected Sub lnkCreateExcel1_Click(sender As Object, e As EventArgs) Handles lnkCreateExcel1.Click
        Dim tblTemp As DataTable = CType(Session("Express"), Express).TableVersendungen.Copy

        Dim col2 As DataColumn = Nothing
        Dim bVisibility As Integer = 0
        Dim i As Integer = 0
        Dim sColName As String = ""
        Dim AppURL As String = Me.Request.Url.LocalPath.Replace("/Services", "..")
        Dim tblTranslations As DataTable = DirectCast(Me.Session(AppURL), DataTable)
        Dim found As Boolean

        i = tblTemp.Columns.Count - 1
        While i >= 0
            found = False
            For Each col As DataControlField In GridView1.Columns

                bVisibility = 0
                col2 = tblTemp.Columns(i)
                If col2.ColumnName.ToUpper() = col.SortExpression.ToUpper() Then
                    found = True

                    sColName = CKG.Base.Kernel.Common.Common.TranslateColLbtn(GridView1, tblTranslations, col.HeaderText, bVisibility)

                    If bVisibility = 0 Then
                        tblTemp.Columns.Remove(col2)
                    ElseIf sColName.Length > 0 Then
                        col2.ColumnName = sColName
                    End If

                End If
            Next
            'wenn nicht gefunden dann entfernen, außer
            If Not found Then
                tblTemp.Columns.Remove(col2)
            End If

            tblTemp.AcceptChanges()
            i += -1
        End While
        Dim excelFactory As New CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory()
        Dim filename As String = [String].Format("{0:yyyyMMdd_HHmmss_}", System.DateTime.Now) + m_User.UserName
        excelFactory.CreateDocumentAndSendAsResponse(filename, tblTemp, Me.Page, False, Nothing, 0, 0)

    End Sub

    Protected Sub lbBack_Click(sender As Object, e As EventArgs) Handles lbBack.Click
        Response.Redirect("../../Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub
End Class