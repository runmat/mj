Imports KBS.KBS_BASE
Imports GeneralTools.Models
Imports Telerik.Web.UI

Partial Public Class BestaetigungZulassungen
    Inherits Page

    Private mObjKasse As Kasse
    Private mObjZulassungen As Zulassungen

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        FormAuth(Me)
        lblError.Text = ""
        Title = lblHead.Text

        If mObjKasse Is Nothing Then
            If Not Session("mKasse") Is Nothing Then
                mObjKasse = CType(Session("mKasse"), Kasse)
            Else
                Throw New Exception("benötigtes Session Objekt nicht vorhanden")
            End If
        End If

        If mObjZulassungen Is Nothing Then
            If Session("objZulassungen") IsNot Nothing Then
                mObjZulassungen = CType(Session("objZulassungen"), Zulassungen)
            Else
                mObjZulassungen = New Zulassungen(mObjKasse.Lagerort)
                Session("objZulassungen") = mObjZulassungen
            End If
        End If

        If Not IsPostBack Then
            mObjZulassungen.LoadZulassungen()
            FillGrid()
        End If
    End Sub

    Private Sub FillGrid()
        If mObjZulassungen.tblZulassungen Is Nothing OrElse mObjZulassungen.tblZulassungen.Rows.Count = 0 Then
            lblError.Text = "Keine Daten gefunden."
            rgGrid1.Visible = False
        Else
            lblError.Text = ""
            rgGrid1.Visible = True
        End If

        rgGrid1.Rebind()
        'Setzen der DataSource geschieht durch das NeedDataSource-Event
    End Sub

    Protected Sub rgGrid1_NeedDataSource(ByVal sender As Object, ByVal e As GridNeedDataSourceEventArgs) Handles rgGrid1.NeedDataSource
        If mObjZulassungen.tblZulassungen IsNot Nothing Then
            rgGrid1.DataSource = mObjZulassungen.tblZulassungen.DefaultView
        Else
            rgGrid1.DataSource = Nothing
        End If
    End Sub

    Protected Sub lb_zurueck_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_zurueck.Click
        Response.Redirect("../Selection.aspx")
    End Sub

    Protected Sub rgGrid1_ItemCommand(ByVal sender As Object, ByVal e As GridCommandEventArgs) Handles rgGrid1.ItemCommand
        If TypeOf e.Item Is GridDataItem Then
            GetGridData()

            Dim gridRow As GridDataItem = CType(e.Item, GridDataItem)
            Dim posRows As DataRow() = mObjZulassungen.tblZulassungen.Select("ORDERID=" & gridRow("ORDERID").Text.ToInt(0))

            Select Case e.CommandName
                Case "Del"
                    For Each posRow As DataRow In posRows
                        If posRow("STATUS").ToString() = "L" Then
                            posRow("STATUS") = "B"
                        Else
                            posRow("STATUS") = "L"
                        End If
                    Next

                Case "Ok"
                    For Each posRow As DataRow In posRows
                        If posRow("STATUS").ToString() = "E" Then
                            posRow("STATUS") = "B"
                        Else
                            posRow("STATUS") = "E"
                        End If
                    Next

            End Select

            Session("objZulassungen") = mObjZulassungen
            FillGrid()
        End If
    End Sub

    Private Sub cmdSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSave.Click
        GetGridData()

        mObjZulassungen.SaveZulassungen()

        If mObjZulassungen.ErrorOccured Then
            lblError.Text = mObjZulassungen.ErrorMessage
        Else
            Dim doneRows As DataRow() = mObjZulassungen.tblZulassungen.Select("STATUS='E' OR STATUS='L'")
            For Each row As DataRow In doneRows
                mObjZulassungen.tblZulassungen.Rows.Remove(row)
            Next

            Session("objZulassungen") = mObjZulassungen
            FillGrid()

            lblError.Text = "Datensätze in SAP gespeichert. Keine Fehler aufgetreten."
        End If
    End Sub

    Private Sub GetGridData()
        For Each item As GridDataItem In rgGrid1.Items
            Dim tmpId As Integer = item("ORDERID").Text.ToInt(0)
            Dim tmpPosNr As Integer = item("POSNR").Text.ToInt(0)
            Dim tmpGebPos As Integer = item("GEB_POS").Text.ToInt(0)

            If tmpPosNr = 10 Then
                Dim idRows As DataRow() = mObjZulassungen.tblZulassungen.Select("ORDERID=" & tmpId)
                Dim datZulassungsdatum As DateTime? = CType(item.FindControl("txtZulassungsdatum"), TextBox).Text.ToNullableDateTime("ddMMyy")
                Dim strKennzeichen As String = CType(item.FindControl("txtKennzeichen"), TextBox).Text.ToUpper()
                For Each posRow As DataRow In idRows
                    If CType(posRow("ZZZLDAT"), DateTime?) <> datZulassungsdatum Then
                        posRow("ZZZLDAT") = datZulassungsdatum
                        If posRow("STATUS").ToString() = "O" Then posRow("STATUS") = "B"
                    End If
                    If posRow("ZZKENN").ToString() <> strKennzeichen Then
                        posRow("ZZKENN") = strKennzeichen
                        If posRow("STATUS").ToString() = "O" Then posRow("STATUS") = "B"
                    End If
                Next
            End If

            If tmpGebPos > 0 Then
                Dim posRow As DataRow = mObjZulassungen.tblZulassungen.Select("ORDERID=" & tmpId & " AND POSNR=" & tmpPosNr)(0)
                Dim decGebuehr As Decimal = CType(item.FindControl("txtGebuehr"), TextBox).Text.ToDecimal(0)
                If CDec(posRow("GEBUEHR")) <> decGebuehr Then
                    posRow("GEBUEHR") = decGebuehr
                    If posRow("STATUS").ToString() = "O" Then posRow("STATUS") = "B"
                End If
            End If
        Next

        Session("objZulassungen") = mObjZulassungen
    End Sub
End Class
