Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security

Partial Public Class Report99_2s
    Inherits System.Web.UI.Page
    Private m_User As User
    Private m_App As App

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        FormAuth(Me, m_User)
        m_App = New App(m_User)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        Dim strAppID As String = String.Empty
        If Request.QueryString("AppID").Length > 0 Then
            strAppID = Request.QueryString("AppID").ToString
            Session("AppID") = strAppID
        End If

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        m_App = New Security.App(m_User)
        filltable()

    End Sub

    Private Sub filltable()
        Dim dirInfo As System.IO.DirectoryInfo
        Dim fInfo As System.IO.FileInfo()
        Dim trString As String
        Dim path As String
        Dim i As Integer


        path = Request.PhysicalApplicationPath & "Docs\Lastschrift"
        dirInfo = New System.IO.DirectoryInfo(path)
        fInfo = dirInfo.GetFiles("*.*")
        trString = String.Empty

        Dim LinkTable As New DataTable
        LinkTable.Columns.Add("Bundesland", GetType(System.String))
        LinkTable.Columns.Add("Pfad", GetType(System.String))

        If (fInfo.Length > 0) Then
            For i = 0 To fInfo.Length - 1
                Dim dRow As DataRow = LinkTable.NewRow
                dRow("Bundesland") = Left(fInfo(i).Name, fInfo(i).Name.IndexOf("."))
                dRow("Pfad") = "\Services\Docs\Lastschrift\" & fInfo(i).Name
                LinkTable.Rows.Add(dRow)
            Next
            Repeater1.DataSource = LinkTable
            Repeater1.DataBind()
            lblError.Text = String.Empty
        Else
            lblError.Text = "Keine Daten vorhanden."
        End If

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        Response.Redirect("Report99.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub
End Class