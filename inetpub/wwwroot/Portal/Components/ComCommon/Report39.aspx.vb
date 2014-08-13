Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Common
Imports System.Collections.Generic

Public Class Report39
    Inherits Page

    Private m_User As User
    Private m_App As App
    Private mExpress As Express

    Protected WithEvents ucHeader As CKG.Portal.PageElements.Header
    Protected WithEvents ucStyles As CKG.Portal.PageElements.Styles

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = Common.GetUser(Me) ' füllen Form.Session("objUser"), rückgabe eines UserObjekte
        Common.FormAuth(Me, m_User)
        m_App = New Base.Kernel.Security.App(m_User) 'erzeugt ein App_objekt 
        ucHeader.InitUser(m_User)
        Common.GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text

        If Page.IsPostBack = False Then

            ddlPageSize.Items.Add("10")
            ddlPageSize.Items.Add("20")
            ddlPageSize.Items.Add("50")
            ddlPageSize.Items.Add("100")
            ddlPageSize.Items.Add("200")
            ddlPageSize.Items.Add("500")
            ddlPageSize.Items.Add("1000")
            ddlPageSize.SelectedIndex = 1
        End If
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")

        Dim ResultTable As DataTable

        ResultTable = CType(Session("Express"), Express).TableVersendungen

        If Not ResultTable Is Nothing Then

            If ResultTable.Rows.Count = 0 Then
                lblError.Visible = True
                lblError.Text = "Keine Daten zur Anzeige gefunden."
                GridView1.Visible = False
                trExcel.Visible = False
            Else
                lblError.Visible = False
                GridView1.Visible = True
                lnkCreateExcel.Visible = True
                trExcel.Visible = True
                lblNoData.Visible = False
                cmdLoeschen.Visible = True

                Dim tmpDataView As New DataView(ResultTable)

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

            End If
        Else
            lblError.Text = "Keine Daten zur Anzeige gefunden."
        End If
    End Sub

    Private Sub lnkCreateExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel.Click

        Dim tblTemp As DataTable = CType(Session("Express"), Express).TableVersendungen.Copy
        Dim col As DataControlField
        Dim col2 As DataColumn
        Dim bVisibility As Integer
        Dim i As Integer
        Dim sColName As String = ""

        Dim AppURL As String = Replace(Me.Request.Url.LocalPath, "/Portal", "..")
        Dim tblTranslations As DataTable = CType(Me.Session(AppURL), DataTable)

        For Each col In GridView1.Columns
            For i = tblTemp.Columns.Count - 1 To 0 Step -1
                bVisibility = 0
                col2 = tblTemp.Columns(i)
                If col2.ColumnName.ToUpper = col.SortExpression.ToUpper Then
                    sColName = Common.TranslateColLbtn(GridView1, tblTranslations, col.HeaderText, bVisibility)
                    If bVisibility = 0 Then
                        tblTemp.Columns.Remove(col2)
                    ElseIf sColName.Length > 0 Then
                        col2.ColumnName = sColName
                    End If
                End If
            Next
            tblTemp.AcceptChanges()
        Next

        Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
        excelFactory.CreateDocumentAndSendAsResponse(strFileName, tblTemp, Me.Page)

        mExpress.Result = Session("ResultTable")
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.PreRender
        Common.SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Unload
        Common.SetEndASPXAccess(Me)
    End Sub

    Private Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        FillGrid(e.NewPageIndex)
    End Sub

    Protected Sub ddlPageSize_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlPageSize.SelectedIndexChanged
        GridView1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(GridView1.PageIndex)
    End Sub

    Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSearch.Click
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
            Session("Express") = mExpress

            If Not mExpress.TableVersendungen Is Nothing Then
                If mExpress.TableVersendungen.Rows.Count > 0 Then

                    mExpress.GetVersandtexte(Me.Page)

                    FillGrid(0)
                Else
                    lblError.Text = "Es wurden keine Versendungen gefunden."
                    GridView1.DataBind()
                    GridView1.Visible = False
                    trExcel.Visible = False
                End If
            Else
                lblError.Text = "Es wurden keine Versendungen gefunden."
                GridView1.DataBind()
                GridView1.Visible = False
                trExcel.Visible = False
            End If

        End If

    End Sub

    Private Function checkSearchParameters() As Boolean
        ' Felder auf UpperCase für saubere Suche
        txtKennzeichen.Text = txtKennzeichen.Text.ToUpper
        txtChassisNum.Text = txtChassisNum.Text.ToUpper

        If txtAbDatum.Text <> String.Empty Or txtBisDatum.Text <> String.Empty Then
            If txtAbDatum.Text <> String.Empty And txtBisDatum.Text <> String.Empty Then
                If Not IsDate(txtAbDatum.Text) Or Not IsDate(txtBisDatum.Text) Then
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

    Protected Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand
        mExpress = Session("Express")

        If e.CommandName = "Info" Then
            lblVersandstatus.Text = "Versandstatus"
            ResetExtender()

            S.AP.Init("Z_DPM_GET_SEND_STATUS", "I_POOLNR", e.CommandArgument.ToString())

            Dim StatusTable As DataTable = S.AP.GetExportTableWithExecute("GT_WEB")

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

            trSuche.Visible = False
            cmdLoeschen.Visible = False
            cmdSearch.Visible = False
            GridView1.Visible = False
            trVersand.Visible = True
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

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        trSuche.Visible = True
        trVersand.Visible = False
        cmdLoeschen.Visible = True
        cmdSearch.Visible = False
        GridView1.Visible = True
    End Sub

    Private Sub GridView1_Sorting(sender As Object, e As GridViewSortEventArgs) Handles GridView1.Sorting
        FillGrid(GridView1.PageIndex, e.SortExpression)
    End Sub
End Class