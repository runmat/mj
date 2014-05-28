Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common

Partial Public Class Report08
    Inherits System.Web.UI.Page

#Region "Declarations"
    Private m_User As Security.User
    Private m_App As Security.App
    Private mMahnSumme As F1_Bank_TempMahn
    Private mMahnDetail As F1_Bank_TempMahn
#End Region

#Region "Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User, True)
        GetAppIDFromQueryString(Me)
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Security.App(m_User)


            If IsPostBack = False Then

                ddlPageSize.Items.Add("10")
                ddlPageSize.Items.Add("20")
                ddlPageSize.Items.Add("50")
                ddlPageSize.Items.Add("100")
                ddlPageSize.Items.Add("200")
                ddlPageSize.Items.Add("500")
                ddlPageSize.Items.Add("1000")
                ddlPageSize.SelectedIndex = 2

                txtDatumVon.Text = Date.Today.ToShortDateString

                tdExcel.Visible = False

                If Request.UrlReferrer.AbsolutePath.Contains("Selection.aspx") Then
                    ClearSessionObjects()
                End If

                If IsNothing(Session("MahnTable")) = False Then
                    InitialNewSearch()
                    ddlPageSize.SelectedIndex = CType(Session("PageSize"), Integer)
                    grvAusgabe.PageSize = CInt(ddlPageSize.SelectedItem.Value)
                    FillGrid(CType(Session("PageIndex"), Integer), CType(Session("Sort"), String))
                End If

            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSearch.Click
        DoSubmit()
    End Sub

    Protected Sub cmdNewSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdNewSearch.Click
        ClearSessionObjects()
        cmdNewSearch.Visible = False
        cmdSearch.Visible = True
        txtDatumVon.Visible = True
        lblMeldungsdatum.Visible = True
        ibtCal.Visible = True
        grvAusgabe.Visible = False
        ddlPageSize.Visible = False
        tdExcel.Visible = False
        lblNoData.Text = ""
        txtDatumVon.Text = Date.Today.ToShortDateString
    End Sub

    Protected Sub cmdBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdBack.Click
        ClearSessionObjects()
        Response.Redirect("../../../Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub

    Protected Sub lnkCreateExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel.Click
        Try
            'Dim control As New Control
            Dim AppURL As String
            Dim TempTable As DataTable = CType(Session("MahnTable"), DataTable).Clone
            Dim AusgabeTable As DataTable = CType(Session("MahnTable"), DataTable).Copy
            Dim col2 As DataColumn
            Dim bVisibility As Integer
            Dim sColName As String = ""

            AppURL = Replace(Request.Url.LocalPath, "/Portal", "..")
            Dim tblTranslations As DataTable = CType(Session(AppURL), DataTable)

            For Each Row As DataRow In AusgabeTable.Rows

                Row("COUNT_WE") = CInt(Row("COUNT_WE")).ToString
                Row("COUNT_ZE") = CInt(Row("COUNT_ZE")).ToString

            Next

            AusgabeTable.AcceptChanges()


            Dim FieldControl As DataControlField

            For Each col2 In TempTable.Columns
                Dim found As Boolean = False

                For Each FieldControl In grvAusgabe.Columns
                    If col2.ColumnName.ToUpper = FieldControl.SortExpression Then
                        found = True

                        sColName = TranslateColLbtn(grvAusgabe, tblTranslations, FieldControl.HeaderText, bVisibility)


                        If FieldControl.Visible = False Then
                            found = False
                        Else
                            AusgabeTable.Columns(col2.ColumnName).ColumnName = sColName
                        End If

                        Exit For
                    End If
                Next

                If found = False Then
                    AusgabeTable.Columns.Remove(AusgabeTable.Columns(col2.ColumnName))

                End If

                AusgabeTable.AcceptChanges()
            Next
            
            If AusgabeTable.Columns.Count > 0 Then
                Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
                Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
                excelFactory.CreateDocumentAndSendAsResponse(strFileName, AusgabeTable, Me)
            Else
                Err.Raise(-1, , "Feldübersetzung prüfen.")
            End If
            
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub grvAusgabe_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grvAusgabe.PageIndexChanging
        FillGrid(e.NewPageIndex, String.Empty)
    End Sub

    Private Sub grvAusgabe_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grvAusgabe.RowCommand
        If e.CommandName = "weiter" Then

            Session("AusMahnSumme") = Nothing
            Session("PageSize") = Nothing


            Dim lbt As LinkButton

            If TypeOf e.CommandSource Is LinkButton Then

                lbt = CType(e.CommandSource, LinkButton)


                Session.Add("PageSize", ddlPageSize.SelectedIndex)


                mMahnDetail = New F1_Bank_TempMahn(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")

                mMahnDetail.HaendlerEx = lbt.Text

                mMahnDetail.fill(Session("AppID").ToString, Session.SessionID)

                If mMahnDetail.NewResultTable.Rows.Count = 0 Then
                    lblError.Text = "Es konnten keine Mahnungen ermittelt werden."
                Else

                    If Session("Mahnungen") Is Nothing Then
                        Session.Add("Mahnungen", mMahnDetail.NewResultTable)
                    Else
                        Session("Mahnungen") = mMahnDetail.NewResultTable
                    End If

                    Session.Add("AusMahnSumme", "AusMahnSumme")


                    Response.Redirect("Report06.aspx?AppID=" & Session("AppID").ToString)

                End If
            End If

        End If
    End Sub

    Private Sub grvAusgabe_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grvAusgabe.Sorting
        FillGrid(grvAusgabe.PageIndex, e.SortExpression)
    End Sub


    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        grvAusgabe.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(0, String.Empty)
    End Sub

    Protected Sub ibtCal_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtCal.Click
        calVon.Visible = True
    End Sub

    Protected Sub calVon_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles calVon.SelectionChanged
        txtDatumVon.Text = calVon.SelectedDate.ToShortDateString
        calVon.Visible = False
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
#End Region

#Region "Methods"


    '----------------------------------------------------------------------
    ' Methode:      DoSubmit
    ' Autor:        SFa
    ' Beschreibung: Ermittelt die Mahnungen in Summe
    ' Erstellt am:  25.09.2009
    ' ITA:          3125
    '----------------------------------------------------------------------
    Private Sub DoSubmit()

        lblError.Text = ""
        calVon.Visible = False

        If checkDate() Then
            mMahnSumme = New F1_Bank_TempMahn(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")

            mMahnSumme.fillMahnungenSumme(Session("AppID").ToString, Session.SessionID)

            If mMahnSumme.MahnSumme.Rows.Count = 0 Then
                lblError.Text = "Es konnten keine Mahnungen ermittelt werden."
            Else


                If Session("MahnTable") Is Nothing Then
                    Session.Add("MahnTable", mMahnSumme.MahnSumme)
                Else
                    Session("MahnTable") = mMahnSumme.MahnSumme
                End If

                InitialNewSearch()

                FillGrid(0)
            End If
        End If

    End Sub

    '----------------------------------------------------------------------
    ' Methode:      FillGrid
    ' Autor:        SFa
    ' Beschreibung: Füllt das gridview grvAusgabe aus der Bestandstabelle.
    '               Je nach Übergabeparameter findet eine Sortierung oder
    '               Seitenanzeige statt
    ' Erstellt am:  16.03.2009
    ' ITA:          2673 und 2667
    '----------------------------------------------------------------------
    Private Sub FillGrid(ByVal PageIndex As Int32, Optional ByVal Sort As String = "")

        Dim MahnTable As New DataTable
        Dim Direction As String = String.Empty

        If IsNothing(Session("PageIndex")) = True Then
            Session.Add("PageIndex", grvAusgabe.PageIndex)
        Else
            Session("PageIndex") = PageIndex
        End If


        If IsNothing(Session("Sort")) = True Then
            Session.Add("Sort", Sort)
        Else
            Session("Sort") = Sort
        End If


        If IsNothing(Session("MahnTable")) = False Then

            MahnTable = CType(Session("MahnTable"), DataTable)

            If MahnTable.Rows.Count > 0 Then

                grvAusgabe.Visible = True
                ddlPageSize.Visible = True

                tdExcel.Visible = True

                'Dim TempPageIndex As Int32 = PageIndex

                If Sort.Trim(" "c).Length > 0 Then
                    PageIndex = 0
                    Sort = Sort.Trim(" "c)
                    If (ViewState("Sort") Is Nothing) OrElse (ViewState("Sort").ToString = Sort) Then
                        If ViewState("Direction") Is Nothing Then
                            Direction = "desc"
                        Else
                            Direction = ViewState("Direction").ToString
                        End If
                    Else
                        Direction = "desc"
                    End If

                    If Direction = "asc" Then
                        Direction = "desc"
                    Else
                        Direction = "asc"
                    End If

                    ViewState("Sort") = Sort
                    ViewState("Direction") = Direction
                Else
                    If Not ViewState("Sort") Is Nothing Then
                        Sort = ViewState("Sort").ToString
                        If ViewState("Direction") Is Nothing Then
                            Direction = "asc"
                            ViewState("Direction") = Direction
                        Else
                            Direction = ViewState("Direction").ToString
                        End If
                    End If
                End If

                If Not Sort.Length = 0 Then
                    MahnTable.DefaultView.Sort = Sort & " " & Direction
                End If

                grvAusgabe.PageIndex = PageIndex

                grvAusgabe.DataSource = MahnTable.DefaultView
                grvAusgabe.DataBind()


                lnkCreateExcel.Visible = True

                lblNoData.Text = "Es wurden " & MahnTable.Rows.Count & " Mahnungen gefunden."
                lblNoData.Visible = True
            Else
                lblNoData.Visible = True
                lblNoData.Text = "Es wurden keine Mahnungen gefunden."
                cmdSearch.Visible = True

            End If
        Else
            lblNoData.Visible = True
        End If

    End Sub

    Private Sub ClearSessionObjects()
        Session("AusMahnSumme") = Nothing
        Session("PageSize") = Nothing
        Session("PageIndex") = Nothing
        Session("Sort") = Nothing
        Session("MahnTable") = Nothing
    End Sub

    Private Sub InitialNewSearch()
        cmdSearch.Visible = False
        cmdNewSearch.Visible = True
        lblMeldungsdatum.Visible = False
        ibtCal.Visible = False
        txtDatumVon.Visible = False
    End Sub

    Private Function checkDate() As Boolean
        Dim tmpbool As Boolean = True
        For Each ctrl As BaseValidator In Validators
            If ctrl.IsValid = False Then
                tmpbool = False
            End If
        Next
        Return tmpbool
    End Function

#End Region

End Class

' ************************************************
' $History: Report08.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 30.09.09   Time: 13:49
' Updated in $/CKAG/Applications/AppF1/forms
' 3125
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 29.09.09   Time: 13:14
' Created in $/CKAG/Applications/AppF1/forms
' ITA: 3125
' 