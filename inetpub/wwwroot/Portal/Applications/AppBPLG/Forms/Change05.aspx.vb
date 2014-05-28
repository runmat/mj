Imports CKG.Base.Kernel.Common
Imports CKG.Base.Kernel.DocumentGeneration
Imports CKG.Base.Kernel.Security
Imports CKG.Portal.PageElements

Public Class Change05
    Inherits Page

    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles

    Private m_User As User
    Private m_App As App
    Private m_Fahrzeugbestand As Fahrzeugbestand
    Private m_AppID As Integer

    Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        Try
            m_User = Common.GetUser(Me) ' füllen Form.Session("objUser"), rückgabe eines UserObjekte 
            m_App = New Base.Kernel.Security.App(m_User) 'erzeugt ein App_objekt 

            Common.FormAuth(Me, m_User)
            ucHeader.InitUser(m_User)
            Common.GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")

            Dim appID = CStr(Session("AppID"))
            m_AppID = Integer.Parse(appID)

            lblHead.Text = m_User.Applications.Select("AppID = '" & appID & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            lblError.Text = ""

            m_Fahrzeugbestand = Common.GetOrCreateObject("Fahrzeugbestand", Function() New Fahrzeugbestand(m_User, m_App, appID))

            If IsPostBack Then
                BindGrid()
            Else
                txtFilterFIN.Text = m_Fahrzeugbestand.FilterFIN
                txtFilterKUNNR.Text = m_Fahrzeugbestand.FilterKundennummer
                txtFilterHAENDLER.Text = m_Fahrzeugbestand.FilterHaendlernummer
                txtFilterBRANDING.Text = m_Fahrzeugbestand.FilterBranding
            End If
        Catch ex As Exception
            lblError.Text = "Fehler beim Laden der Seite: " & ex.Message.ToString
        End Try
    End Sub

    Private Sub lb_Suche_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_Suche.Click

        m_User.App.GetAppAutLevel(m_User.GroupID, m_AppID)

        Dim FilterKundennummer = txtFilterKUNNR.Text.Trim(" "c)
        Dim FilterHaendlernummer = txtFilterHAENDLER.Text.Trim(" "c)
        Dim FilterBranding = txtFilterBRANDING.Text.ToUpper.Trim(" "c)
        Dim FilterFIN = txtFilterFIN.Text.Trim(" "c)


        m_Fahrzeugbestand.FilterFIN = FilterFIN
        m_Fahrzeugbestand.FilterKundennummer = FilterKundennummer
        m_Fahrzeugbestand.FilterBranding = FilterBranding
        m_Fahrzeugbestand.FilterHaendlernummer = FilterHaendlernummer


        'level wird geprüft

        Dim levelAut() As String
        Dim dt As New DataTable

        dt.Columns.Add("Name", GetType(System.String))
        dt.Columns.Add("Level", GetType(System.String))
        dt.Columns.Add("Autorisierung", GetType(System.String))

        If String.IsNullOrEmpty(m_User.App.AutorisierungsLevel) = False Then

            'Beinhaltet 2 Arrays: Level-Array und Autorisierungsarray(1 zu 1) getrennt durch |
            levelAut = Split(m_User.App.AutorisierungsLevel, "|")

            'Level-Array
            Dim level() As String = Split(levelAut(0), ",")

            'Autorisieungsarray
            Dim aut() As String = Split(levelAut(1), ",")

            Dim dr As DataRow

            'Level aus Level-Array
            For i = 0 To level.Length - 1

                dr = dt.NewRow

                dr("Level") = level(i) 'Level hinzufügen
                dr("Autorisierung") = aut(i) 'Zum Level gehörende Autorisierung hinzufügen

                dt.Rows.Add(dr)

            Next

        End If

        'get highest level
        Dim highestlevel As Integer = 0
        For Each tmpRow As DataRow In dt.Rows
            ' --- MJE, 19.12.2012, added  >> Or tmpRow("Autorisierung").ToString = "2" <<
            If (tmpRow("Autorisierung").ToString = "1" Or tmpRow("Autorisierung").ToString = "2") Then
                If highestlevel < Integer.Parse(tmpRow("Level").ToString()) Then
                    highestlevel = Integer.Parse(tmpRow("Level").ToString())
                End If
            End If
        Next


        If (highestlevel = 0 Or highestlevel = 1 Or highestlevel = 2) And (FilterKundennummer = Nothing And FilterHaendlernummer = Nothing And FilterBranding = Nothing And FilterFIN = Nothing) Then
            lblError.Text = "Bitte definieren Sie mindestens einen Filter!"
            Exit Sub

        End If

        m_Fahrzeugbestand.Show()

       
        If (highestlevel = 0 Or highestlevel = 1) Then
            resultGrid.Columns(0).Visible = False
        End If

        If m_Fahrzeugbestand.Status = 0 Then
            If m_Fahrzeugbestand.Result Is Nothing OrElse m_Fahrzeugbestand.Result.Rows.Count <= 0 Then
                lblError.Text = "keine Daten vorhanden"
            Else
                BindGrid()
            End If
        Else
            lblError.Text = m_Fahrzeugbestand.Message
        End If
    End Sub

    Private Sub lb_NewSearch_Click(sender As Object, e As EventArgs) Handles lb_NewSearch.Click
        txtFilterBRANDING.Text = ""
        txtFilterFIN.Text = ""
        txtFilterHAENDLER.Text = ""
        txtFilterKUNNR.Text = ""

        searchRow.Visible = True
        lb_NewSearch.Visible = False
    End Sub

    Private Sub BindGrid()
        If m_Fahrzeugbestand.Result Is Nothing OrElse m_Fahrzeugbestand.Result.Rows.Count = 0 Then
            resultRow.Visible = False
            searchRow.Visible = True
            Return
        End If

        Session("ResultTable") = m_Fahrzeugbestand.Result

        Dim view = New DataView(m_Fahrzeugbestand.Result)
        Dim direction = CType(If(ViewState("SortDirection"), SortDirection.Ascending), SortDirection)
        Dim sort = CStr(If(ViewState("SortColumn"), String.Empty))

        If Not String.IsNullOrEmpty(sort) Then
            view.Sort = String.Concat(sort, " ", If(direction = SortDirection.Ascending, "asc", "desc"))
        End If

        resultRow.Visible = True
        lb_NewSearch.Visible = True
        searchRow.Visible = False

        If view.Count < 11 Then
            resultGrid.AllowPaging = False
        Else
            resultGrid.AllowPaging = True
        End If

        resultGrid.DataSource = view
        resultGrid.DataBind()
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.PreRender
        Common.SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Unload
        Common.SetEndASPXAccess(Me)
    End Sub

    Private Sub resultGrid_EditCommand(source As Object, e As DataGridCommandEventArgs) Handles resultGrid.EditCommand
        If e.Item Is Nothing Then Return

        Select Case e.CommandName
            Case "Edit"
                Dim chassis_num = CStr(resultGrid.DataKeys(e.Item.ItemIndex))
                Dim rows = m_Fahrzeugbestand.Result.Select("CHASSIS_NUM='" & chassis_num & "'")

                If rows.Length <> 1 Then Return

                Dim row = rows(0)
                m_Fahrzeugbestand.SelectedFIN = row.Field(Of String)("CHASSIS_NUM")

                lbl_DisplayFIN.Text = m_Fahrzeugbestand.SelectedFIN
                txtEditBRANDING.Text = row.Field(Of String)("BRANDING")
                txtEditHAENDLER.Text = row.Field(Of String)("HAENDLER")
                txtEditKUNNR.Text = row.Field(Of String)("KUNNR")
                txtEditLIZNR.Text = row.Field(Of String)("LIZNR")

                mpeEditFzg.Show()
        End Select
    End Sub

    Private Sub resultGrid_PageIndexChanged(source As Object, e As DataGridPageChangedEventArgs) Handles resultGrid.PageIndexChanged
        resultGrid.CurrentPageIndex = e.NewPageIndex
        BindGrid()
    End Sub

    Private Sub resultGrid_SortCommand(source As Object, e As DataGridSortCommandEventArgs) Handles resultGrid.SortCommand
        If e.SortExpression.Equals(ViewState("SortColumn")) Then
            ViewState("SortDirection") = If(SortDirection.Ascending.Equals(ViewState("SortDirection")), SortDirection.Descending, SortDirection.Ascending)
        Else
            ViewState("SortColumn") = e.SortExpression
            ViewState("SortDirection") = SortDirection.Ascending
        End If
        BindGrid()
    End Sub

    Private Sub btnOK_Click(sender As Object, e As System.EventArgs) Handles btnOK.Click
        Try
            m_Fahrzeugbestand.ChangedBranding = txtEditBRANDING.Text
            m_Fahrzeugbestand.ChangedHaendlernummer = txtEditHAENDLER.Text
            m_Fahrzeugbestand.ChangedKundennummer = txtEditKUNNR.Text
            m_Fahrzeugbestand.ChangedVertragsnummer = txtEditLIZNR.Text

            m_Fahrzeugbestand.Change()

            If m_Fahrzeugbestand.Status <> 0 Then
                lblEditError.Text = m_Fahrzeugbestand.Message
                mpeEditFzg.Show()
                Return
            End If

            mpeEditFzg.Hide()

            m_Fahrzeugbestand.Show()
            BindGrid()
        Catch ex As Exception
        End Try
    End Sub

    Protected Sub lnkCreateExcel_Click(sender As Object, e As EventArgs) Handles lnkCreateExcel.Click
        Dim data As DataTable = DirectCast(Session("ResultTable"), DataTable).Copy()

        Dim appUrl = Request.Url.LocalPath.Replace("/Portal", "..")
        Dim tblTranslations = DirectCast(Session(appUrl), DataTable)
        For Each col As DataGridColumn In resultGrid.Columns
            Dim i = data.Columns.Count - 1
            While i >= 0
                Dim bVisibility As Integer = 0
                Dim col2 = data.Columns(i)
                If String.Compare(col2.ColumnName, col.SortExpression, True) = 0 Then
                    Dim sColName = Common.TranslateColLbtn(resultGrid, tblTranslations, col.HeaderText, bVisibility)
                    If bVisibility = 0 Then
                        data.Columns.Remove(col2)
                    ElseIf sColName.Length > 0 Then
                        col2.ColumnName = sColName
                    End If
                End If
                i -= 1
            End While
            data.AcceptChanges()
        Next

        Dim excelFactory As New ExcelDocumentFactory()
        Dim reportName = String.Format("{0:yyyyMMdd_HHmmss_}{1}", DateTime.Now, m_User.UserName)
        excelFactory.CreateDocumentAndSendAsResponse(reportName, data, Me, False, Nothing, 0, 0)
    End Sub
End Class