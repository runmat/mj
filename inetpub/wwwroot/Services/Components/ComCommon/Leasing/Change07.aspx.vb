Imports CKG.Base.Kernel.Common
Imports CKG.Base.Kernel.DocumentGeneration
Imports CKG.Base.Kernel.Security

Partial Public Class Change07
    Inherits System.Web.UI.Page

    Protected GridNavigation1 As CKG.Services.GridNavigation

    Private m_App As App
    Private m_User As User

    Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
        MyBase.OnLoad(e)

        Me.m_User = Common.GetUser(Me)
        Common.FormAuth(Me, Me.m_User)
        Common.GetAppIDFromQueryString(Me)
        Me.m_App = New App(Me.m_User)

        Me.GridNavigation1.setGridElment(gvBestand)

        Me.lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

        lblError.Text = ""

        If Not Me.IsPostBack Then
            Dim c As New CarportBeauftragung(Me.m_User, Me.m_App, Me)

            Me.ddlCarport.Items.AddRange(c.GetCarports().ToArray())
        End If
    End Sub

    Protected Overrides Sub OnPreRender(ByVal e As System.EventArgs)
        MyBase.OnPreRender(e)
        Common.SetEndASPXAccess(Me)
    End Sub

    Protected Overrides Sub OnUnload(ByVal e As System.EventArgs)
        MyBase.OnUnload(e)
        Common.SetEndASPXAccess(Me)
    End Sub

    Protected Sub lbCreate_Click(ByVal sender As Object, ByVal e As EventArgs)
        Me.DoSubmit()
    End Sub

    Protected Sub cbAuswahlCheckedChanged(sender As Object, e As EventArgs)
        Dim cb As CheckBox = DirectCast(sender, CheckBox)
        Dim hf As HiddenField = DirectCast(cb.Parent.FindControl("hfAuswahl"), HiddenField)
        Dim index As Integer = Convert.ToInt32(hf.Value)

        Dim table As DataTable = DirectCast(Session("mObjAuswertung"), DataTable)
        Dim row As DataRow = table.Rows(index)
        row.SetField("Auswahl", cb.Checked)
        table.AcceptChanges()
    End Sub

    Private Function GetSelected() As IEnumerable(Of Pair)
        Dim table As DataTable = DirectCast(Session("mObjAuswertung"), DataTable)
        Return From dr In table.AsEnumerable() _
               Where dr.Field(Of Boolean)("AUSWAHL") _
               Select New Pair(dr.Field(Of String)("AUFNR"), dr.Field(Of String)("CHASSIS_NUM"))
    End Function

    Protected Sub GridNavigation1_PagerChanged(ByVal PageIndex As Integer)
        Me.gvBestand.PageIndex = PageIndex
        Me.FillGrid(PageIndex)
    End Sub

    Protected Sub GridNavigation1_PageSizeChanged()
        Me.FillGrid(0)
    End Sub

    Protected Sub gvBestand_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvBestand.Sorting
        Me.FillGrid(gvBestand.PageIndex, e.SortExpression)
    End Sub

    Protected Sub NewSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles NewSearch.Click
        lbCreate.Visible = True
        tab1.Visible = True
        Queryfooter.Visible = True
        Result.Visible = False
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        Dim tmpDataView As New DataView()

        Dim tblTemp As DataTable = CType(Session("mObjAuswertung"), DataTable)

        tmpDataView = tblTemp.DefaultView

        If tmpDataView.Count = 0 Then
            Result.Visible = False
            lblError.Text = "zu Ihrer Selektion konnten keine Werte ermittelt werden"
        Else
            Result.Visible = True
            lbCreate.Visible = False
            tab1.Visible = False
            Queryfooter.Visible = False

            Dim strTempSort As String = ""
            Dim strDirection As String = ""
            Dim intTempPageIndex As Int32 = intPageIndex

            If strSort.Trim(" "c).Length > 0 Then
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

            gvBestand.PageIndex = intTempPageIndex
            gvBestand.DataSource = tmpDataView
            gvBestand.DataBind()

            Result.Visible = True
        End If
    End Sub

    Private Sub DoSubmit()

        Dim booFalse As Boolean = False

        If txtDatumVon.Text.Trim.Length = 0 Or txtDatumBis.Text.Trim.Length = 0 Then
            lblError.Text = "Die Felder ""Datum von"" und ""Datum bis"" müssen Werte enthalten!"
            lblError.Visible = True
            Exit Sub
        End If

        If Not cv_txtDatumVon.IsValid Then
            Exit Sub
        End If

        Dim datumVon As DateTime?
        Dim datumBis As DateTime?
        Dim temp As DateTime

        If DateTime.TryParse(Me.txtDatumVon.Text, temp) Then
            datumVon = temp
        End If

        If DateTime.TryParse(Me.txtDatumBis.Text, temp) Then
            datumBis = temp
        End If

        Dim c As New CarportBeauftragung(Me.m_User, Me.m_App, Me)

        Dim table As DataTable = c.GetAufträge(datumVon, datumBis, Me.ddlCarport.SelectedValue, Me.txtFahrgestellnummer.Text, Me.txtKennzeichen.Text, _
                                                          Me.txtStatus.Text, "")

        Dim col = table.Columns.Add("AUSWAHL", GetType(Boolean))
        col.DefaultValue = False

        For Each dr In table.AsEnumerable()
            dr.SetField(col.Ordinal, False)
        Next

        col.AllowDBNull = False
        table.AcceptChanges()

        Session("mObjAuswertung") = table
        FillGrid(0)
    End Sub

    Protected Sub lnkCreateExcel1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel1.Click
        Dim reportExcel As DataTable = CType(Session("mObjAuswertung"), DataTable).Copy

        Try
            Dim excelFactory As New ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
            excelFactory.CreateDocumentAndSendAsResponse(strFileName, reportExcel, Me)

        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

   
    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        Response.Redirect("../../../Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub
End Class

