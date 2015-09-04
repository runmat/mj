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

    Protected Sub gebuehrChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim txtBox As TextBox = CType(sender, TextBox)
        Dim item As GridDataItem = CType(txtBox.NamingContainer, GridDataItem)

        Dim posRow As DataRow = mObjZulassungen.tblZulassungen.Select("ID=" & item("ID").Text.ToInt(0) & " AND POSNR=" & item("POSNR").Text.ToInt(0))(0)
        posRow("GEBUEHR") = txtBox.Text
    End Sub

    Protected Sub zulassungsdatumChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim txtBox As TextBox = CType(sender, TextBox)
        Dim item As GridDataItem = CType(txtBox.NamingContainer, GridDataItem)

        Dim posRows As DataRow() = mObjZulassungen.tblZulassungen.Select("ID=" & item("ID").Text.ToInt(0))
        For Each posRow As DataRow In posRows
            posRow("ZZZLDAT") = txtBox.Text.ToNullableDateTime("ddMMyy")
        Next
    End Sub

    Protected Sub kennzeichenChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim txtBox As TextBox = CType(sender, TextBox)
        Dim item As GridDataItem = CType(txtBox.NamingContainer, GridDataItem)

        Dim posRows As DataRow() = mObjZulassungen.tblZulassungen.Select("ID=" & item("ID").Text.ToInt(0))
        For Each posRow As DataRow In posRows
            posRow("ZZKENN") = txtBox.Text.ToUpper()
        Next
    End Sub

    Protected Sub rgGrid1_ItemCommand(ByVal sender As Object, ByVal e As GridCommandEventArgs) Handles rgGrid1.ItemCommand
        If TypeOf e.Item Is GridDataItem Then
            Dim gridRow As GridDataItem = CType(e.Item, GridDataItem)
            Dim posRows As DataRow() = mObjZulassungen.tblZulassungen.Select("ID=" & gridRow("ID").Text.ToInt(0))

            Select Case e.CommandName
                Case "Del"
                    For Each posRow As DataRow In posRows
                        If posRow("STATUS").ToString() = "L" Then
                            posRow("STATUS") = ""
                        Else
                            posRow("STATUS") = "L"
                        End If
                    Next

                Case "Ok"
                    For Each posRow As DataRow In posRows
                        If posRow("STATUS").ToString() = "E" Then
                            posRow("STATUS") = ""
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
End Class
