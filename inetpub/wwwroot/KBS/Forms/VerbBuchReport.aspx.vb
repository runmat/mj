Imports System.Net.Mime
Imports Aspose.Words
Imports KBS.KBS_BASE
Imports GeneralTools
Imports GeneralTools.Services
Imports KBS.DocumentGeneration

Partial Public Class VerbBuchReport
    Inherits Page

    Private mObjKasse As Kasse
    Private mObjVerbandbuch As ClsVerbandbuch

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        FormAuth(Me)
        lblError.Text = ""
        lblNoData.Text = ""

        If mObjKasse Is Nothing Then
            If Not Session("mKasse") Is Nothing Then
                mObjKasse = CType(Session("mKasse"), Kasse)
            Else
                Throw New Exception("benötigtes Session Objekt nicht vorhanden")
            End If
        End If

        lblVkbur.Text = "Verkaufsbüro: " & mObjKasse.Lagerort

        mObjVerbandbuch = New ClsVerbandbuch()
        mObjVerbandbuch.GetSAP(mObjKasse.Lagerort)

        If Not IsPostBack Then
            FillGrid(0)
        End If

    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")

        Dim tmpDataView As New DataView(mObjVerbandbuch.Entries)

        If tmpDataView.Count = 0 Then
            GridView1.Visible = False
            lblNoData.Visible = True
        Else
            GridView1.Visible = True
            lblNoData.Visible = False

            Dim intTempPageIndex As Int32 = intPageIndex
            Dim strTempSort As String = ""
            Dim strDirection As String = ""

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

            GridView1.PageIndex = intTempPageIndex
            GridView1.DataSource = tmpDataView

            GridView1.DataBind()

        End If
    End Sub


    Public Sub Export()

        Dim headTable As New DataTable("Kopf")
        headTable.Columns.Add("Kostenstelle", GetType(System.String))
        headTable.Columns.Add("Datum", GetType(System.String))
        Dim tmpSAPRow As DataRow
        tmpSAPRow = headTable.NewRow
        tmpSAPRow("Kostenstelle") = mObjKasse.Lagerort
        tmpSAPRow("Datum") = DateTime.Now().ToString()
        headTable.Rows.Add(tmpSAPRow)
        Dim imageHt As New Hashtable()
        Dim sFilePath As String = mObjKasse.Lagerort & "_" & Replace(Now.ToShortDateString, ".", "") & "_" & Replace(Now.ToShortTimeString, ":", "")
        Dim FilePath As String
        FilePath = ConfigurationManager.AppSettings("LocalDocumentsPath") & "Verbandbuch\" & sFilePath & ".pdf"
        Dim docFactory As New DocumentGeneration.WordDocumentFactory(mObjVerbandbuch.Entries, imageHt)

        docFactory.CreateDocumentTableAndSend("Verbandbuch__KST__" & mObjKasse.Lagerort & "__" & DateTime.Now().ToString(), Page, "\Vorlagen\Verbandbuch.doc", headTable)


    End Sub

    Private Sub GridView1_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs) Handles GridView1.Sorting
        FillGrid(GridView1.PageIndex, e.SortExpression)
    End Sub

    Private Sub responseBack()
        Response.Redirect("../Forms/Verbandbuch.aspx")
    End Sub

    Protected Sub lb_zurueck_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_zurueck.Click
        responseBack()
    End Sub

    Protected Sub OnClick(sender As Object, e As ImageClickEventArgs)
        Export()
    End Sub
End Class