Option Explicit On
Option Strict On

Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports CKG.Services.GridNavigation

Partial Public Class Change08
    Inherits System.Web.UI.Page


#Region "Declarations"
    Private m_App As App
    Private m_User As User
    Protected WithEvents GridNavi As CKG.Services.GridNavigation
    Private mVertragsdaten As Vertragsdaten
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)
        m_App = New App(m_User)
        GridNavi.setGridElment(gvAusgabe)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

            m_App = New App(m_User)

            lblError.Text = ""

            If IsPostBack = True Then
                SaveGridToTable()
            Else
                Session("OutputTable") = Nothing
                Session("Distrikte") = Nothing
                Session("SaveTable") = Nothing
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try



    End Sub

    Protected Sub lbCreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbCreate.Click
        DoSubmit()
    End Sub

    Protected Sub ddlDistrikte_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim ddl As DropDownList = CType(sender, DropDownList)
        Dim FilterExpression As String = ""

        If ddl.SelectedItem.Value <> "0" Then
            FilterExpression = "Distrikt = '" & ddl.SelectedItem.Value & "'"
        End If

        Session("DistriktID") = ddl.SelectedItem.Value

        FillGrid(0, strFilter:=FilterExpression)


    End Sub

    Protected Sub lbCreateExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbCreateExcel.Click
        Dim TempTable As DataTable = CType(Session("OutputTable"), DataTable)
        Dim excelFactory As New CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory()
        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName

        If Not Session("DistriktID") Is Nothing Then
            If Session("DistriktID").ToString <> "0" Then

                TempTable.DefaultView.RowFilter = "Distrikt = '" & Session("DistriktID").ToString & "'"


                TempTable = TempTable.DefaultView.ToTable

            End If
        End If

        excelFactory.CreateDocumentAndSendAsResponse(strFileName, TempTable, Me.Page, , , , )
    End Sub


    Protected Sub lbSpeichern_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbSpeichern.Click
        Dim TempTable As New DataTable()
        TempTable = CType(Session("OutputTable"), DataTable)

        TempTable.DefaultView.RowFilter = "Erledigt = true"

        If TempTable.DefaultView.Count > 0 Then


            mVertragsdaten = New Vertragsdaten(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")

            TempTable = TempTable.DefaultView.ToTable

            mVertragsdaten.SaveTable = CType(Session("SaveTable"), DataTable)

            Dim rSet() As DataRow


            For Each dr As DataRow In TempTable.Rows

                rSet = mVertragsdaten.SaveTable.Select("LIZNR = '" & dr("Vertragsnummer").ToString & "'")

                rSet(0).BeginEdit()

                rSet(0)("ERLKZ") = "X"

                rSet(0).EndEdit()

                mVertragsdaten.SaveTable.AcceptChanges()
            Next

            mVertragsdaten.Save(Session("AppID").ToString, Session.SessionID.ToString, Me)

            lblError.Text = "Ihre Daten wurden gespeichert."
            Result.Visible = False
            Session("OutputTable") = Nothing
            Session("Distrikte") = Nothing
            Session("SaveTable") = Nothing
            lbCreate.Visible = Not lbCreate.Visible
            tab1.Visible = Not tab1.Visible
            Queryfooter.Visible = Not Queryfooter.Visible
        Else
            lblError.Text = "Es wurden keine Vorgänge auf erledigt gesetzt."
        End If


    End Sub


    Private Sub GridNavi_PagerChanged(ByVal PageIndex As Integer) Handles GridNavi.PagerChanged
        gvAusgabe.PageIndex = PageIndex
        FillGrid(PageIndex)
    End Sub

    Private Sub GridNavi_PageSizeChanged() Handles GridNavi.PageSizeChanged
        FillGrid(0)
    End Sub

    Private Sub gvAusgabe_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAusgabe.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header) Then

            Dim TempTable As New DataTable()

            TempTable = CType(Session("Distrikte"), DataTable)

            Dim ddl As DropDownList = CType(e.Row.FindControl("ddlDistrikte"), DropDownList)

            TempTable.DefaultView.Sort = "DistriktID"
            ddl.DataSource = TempTable.DefaultView
            ddl.DataValueField = "DistriktID"
            ddl.DataTextField = "Distrikt"
            ddl.DataBind()

            If Not Session("DistriktID") Is Nothing Then
                ddl.SelectedValue = CType(Session("DistriktID"), String)
            End If


        End If

    End Sub
    Private Sub gvAusgabe_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvAusgabe.Sorting
        FillGrid(gvAusgabe.PageIndex, e.SortExpression)
    End Sub


    'Protected Sub lb_zurueck_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_zurueck.Click
    '    Session("OutputTable") = Nothing
    '    Session("Distrikte") = Nothing
    '    Session("SaveTable") = Nothing

    '    Response.Redirect("../../../Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    'End Sub

    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        Session("OutputTable") = Nothing
        Session("Distrikte") = Nothing
        Session("SaveTable") = Nothing
        Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx")
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
        HelpProcedures.FixedGridViewCols(gvAusgabe)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
 
#End Region

#Region "Methods"
    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "", Optional ByVal strFilter As String = "")

        Dim tmpDataView As New DataView()
        Dim TempTable As New DataTable()



        TempTable = CType(Session("OutputTable"), DataTable)


        tmpDataView = TempTable.DefaultView

        tmpDataView.RowFilter = strFilter

        If tmpDataView.Count = 0 Then
            Result.Visible = False

        Else
            Result.Visible = True
            lbCreate.Visible = False
            tab1.Visible = False
            Queryfooter.Visible = False
            'Queryfooter.Visible = False
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
            gvAusgabe.PageIndex = intTempPageIndex
            gvAusgabe.DataSource = tmpDataView
            gvAusgabe.DataBind()

        End If

    End Sub


    Private Sub DoSubmit()

        Try

            mVertragsdaten = New Vertragsdaten(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")

            If rb_Halter.Checked = True Then
                mVertragsdaten.SuchKennzeichen = "H"
            ElseIf rb_Kennz.Checked = True Then
                mVertragsdaten.SuchKennzeichen = "K"
            ElseIf rb_ZBI.Checked = True Then
                mVertragsdaten.SuchKennzeichen = "Z"
            End If

            mVertragsdaten.Fill(Session("AppID").ToString, Session.SessionID.ToString, Me)

            Session("OutputTable") = mVertragsdaten.OutputTable
            Session("Distrikte") = mVertragsdaten.Distrikte
            Session("SaveTable") = mVertragsdaten.SaveTable


            If Not mVertragsdaten.Status = 0 Then
                lblError.Text = mVertragsdaten.Message
            ElseIf mVertragsdaten.OutputTable.Rows.Count = 0 Then
                lblError.Text = "Keine Ergebnisse für die gewaehlten Kriterien."
            Else
                lbSpeichern.Visible = True
                FillGrid(0)
            End If

        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.(" & ex.Message & ")"
        End Try
    End Sub


    Private Sub SaveGridToTable()


        If Session("OutputTable") Is Nothing Then Exit Sub

        If gvAusgabe.Rows.Count = 0 Then Exit Sub

        Dim TempTable As DataTable = CType(Session("OutputTable"), DataTable)
        Dim TempRowSet() As DataRow
        Dim lb As Label
        Dim cbo As CheckBox

        For Each GridRow As GridViewRow In gvAusgabe.Rows

            lb = CType(GridRow.FindControl("lblVertragsnummer"), Label)
            TempRowSet = TempTable.Select("Vertragsnummer = '" & lb.Text & "'")

            If TempRowSet.Length > 0 Then
                TempRowSet(0).BeginEdit()

                cbo = CType(GridRow.FindControl("cboErledigt"), CheckBox)

                TempRowSet(0)("Erledigt") = cbo.Checked
                TempRowSet(0).EndEdit()

                TempTable.AcceptChanges()

            End If


        Next

        Session("OutputTable") = TempTable


    End Sub


#End Region


    Protected Sub NewSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles NewSearch.Click
        lbCreate.Visible = Not lbCreate.Visible
        tab1.Visible = Not tab1.Visible
        Queryfooter.Visible = Not Queryfooter.Visible
    End Sub
End Class
' ************************************************
' $History: Change08.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Dittbernerc  Date: 5.04.11    Time: 16:30
' Updated in $/CKAG2/Applications/AppF2/forms
' FixGridViewCols:
' 
' Elemente wie Textboxen, Dropdownlisten, und Checkboxen werden in der
' Berechnung mit ber�cksichtigt.
' Die Headrow wird nur noch einmalig abgefragt nicht pro abgefragter Row
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 6.12.10    Time: 16:56
' Updated in $/CKAG2/Applications/AppF2/forms
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 8.12.09    Time: 17:12
' Updated in $/CKAG2/Applications/AppF2/forms
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.12.09    Time: 16:52
' Created in $/CKAG2/Applications/AppF2/forms
' ITA: 3264
' 