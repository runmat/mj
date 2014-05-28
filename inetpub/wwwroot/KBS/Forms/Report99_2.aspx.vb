Imports System
Imports System.Data
Imports KBS.KBS_BASE

Partial Public Class Report99_2
    Inherits Page

    Private mObjKasse As Kasse

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        FormAuth(Me)
        lblError.Text = ""

        If mObjKasse Is Nothing Then
            If Not Session("mKasse") Is Nothing Then
                mObjKasse = CType(Session("mKasse"), Kasse)
            Else
                Throw New Exception("benötigtes Session Objekt nicht vorhanden")
            End If
        End If

        Title = lblHead.Text

        filltable()

    End Sub

    Private Sub filltable()
        Dim dirInfo As IO.DirectoryInfo
        Dim fInfo As IO.FileInfo()
        Dim path As String
        Dim i As Integer

        path = Request.PhysicalApplicationPath & "Docs\Lastschrift"
        dirInfo = New IO.DirectoryInfo(path)
        fInfo = dirInfo.GetFiles("*.*")

        Dim LinkTable As New DataTable
        LinkTable.Columns.Add("Bundesland", GetType(System.String))
        LinkTable.Columns.Add("Pfad", GetType(System.String))

        If (fInfo.Length > 0) Then
            For i = 0 To fInfo.Length - 1
                Dim dRow As DataRow = LinkTable.NewRow
                dRow("Bundesland") = Left(fInfo(i).Name, fInfo(i).Name.IndexOf("."))
                dRow("Pfad") = "\KBS\Docs\Lastschrift\" & fInfo(i).Name
                LinkTable.Rows.Add(dRow)
            Next
            Repeater1.DataSource = LinkTable
            Repeater1.DataBind()
            lblError.Text = String.Empty
        Else
            lblError.Text = "Keine Daten vorhanden."
        End If

    End Sub

    Protected Sub lb_zurueck_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_zurueck.Click
        Response.Redirect("Report99.aspx", False)
    End Sub

End Class