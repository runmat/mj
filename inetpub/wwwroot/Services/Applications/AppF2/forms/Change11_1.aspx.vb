Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common
Imports Telerik.Web.UI
Imports System.Reflection

Public Class Change11_1
    Inherits System.Web.UI.Page

    Private m_User As User
    Private m_App As App
    Private m_UE As UnangeforderteEquipments
    'Private m_AG As Abrufgruende
    Protected kopfdaten As CKG.Services.PageElements.Kopfdaten

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        m_User = Common.GetUser(Me)
        Common.FormAuth(Me, m_User)
        m_App = New App(m_User)
        Common.GetAppIDFromQueryString(Me)
        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

        If Not IsPostBack Then
            Session.Remove("UE")
        End If

        m_UE = Common.GetOrCreateObject("UE", Function() New UnangeforderteEquipments(m_User, m_App, Session("AppID"), Session.SessionID))

        augruSource.ConnectionString = ConfigurationManager.AppSettings("Connectionstring")
        augruSource.SelectParameters.Clear()
        augruSource.SelectParameters.Add("cID", m_User.Customer.CustomerId)
        augruSource.SelectParameters.Add("gID", m_User.GroupID)

        kopfdaten.UserReferenz = m_User.Reference
        kopfdaten.HaendlerNummer = Session("HAENDLER_EX")
        kopfdaten.HaendlerName = Session("HAENDLER_NAME")
        kopfdaten.Adresse = Session("HAENDLER_ADDR")
    End Sub



    Protected Sub augruSelectedAll(sender As Object, e As EventArgs)
        Dim box As DropDownList = CType(sender, DropDownList)

        SetAllAUGRUs(box.SelectedValue)

        LoadData()
    End Sub

    Protected Sub augruSelectedItem(sender As Object, e As EventArgs)
        Dim box As DropDownList = CType(sender, DropDownList)
        Dim item As GridDataItem = CType(box.NamingContainer, GridDataItem)

        Dim equnr = item("EQUNR").Text
        Dim chass = item("CHASSIS_NUM").Text

        If box.SelectedValue = "168" OrElse box.SelectedValue = "169" Then
            SetAllAUGRUs(box.SelectedValue)
        Else
            Dim row = m_UE.Result.Select(String.Format("EQUNR='{0}' and CHASSIS_NUM='{1}'", equnr, chass)).FirstOrDefault
            If Not row Is Nothing Then row("AUGRU") = box.SelectedValue
        End If

        LoadData()
    End Sub

    Private Sub SetAllAUGRUs(ByVal value As String)
        Dim rows = m_UE.Result.Select("Selected")
        For Each row In rows
            row("AUGRU") = value
        Next
    End Sub

    Protected Sub SearchClick(sender As Object, e As EventArgs)
        LoadData()

        If fzgGrid.Items.Count > 0 Then ShowSuche(False)
    End Sub

    Protected Sub NewSearch_Click(sender As Object, e As ImageClickEventArgs)
        ShowSuche(True)
    End Sub

    Private Sub ShowSuche(Optional ByVal open As Boolean = True)
        If open Then
            NewSearch.Visible = False
            NewSearchUp.Visible = True
            lblNewSearch.Visible = False
            cmdSearch.Visible = True
            tab1.Visible = True
            Queryfooter.Visible = True
        Else
            NewSearch.Visible = True
            NewSearchUp.Visible = False
            lblNewSearch.Visible = True
            cmdSearch.Visible = False
            tab1.Visible = False
            Queryfooter.Visible = False
        End If
    End Sub

    Protected Sub NewSearchUp_Click(sender As Object, e As ImageClickEventArgs)
        ShowSuche(False)
    End Sub

    Protected Sub GridNeedDataSource(ByVal sender As Object, e As GridNeedDataSourceEventArgs)
        LoadData(False)
    End Sub

    Protected Sub selectedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim box As CheckBox = CType(sender, CheckBox)
        Dim item As GridDataItem = CType(box.NamingContainer, GridDataItem)

        Dim equnr = item("EQUNR").Text
        Dim chass = item("CHASSIS_NUM").Text

        Dim row = m_UE.Result.Select(String.Format("EQUNR='{0}' and CHASSIS_NUM='{1}'", equnr, chass)).FirstOrDefault
        If Not row Is Nothing Then row("Selected") = box.Checked
        LoadData()
    End Sub

    Private Sub LoadData(Optional ByVal rebind As Boolean = True)
        lblError.Visible = False
        lblNoData.Visible = False

        Select Case views.ActiveViewIndex
            Case 0
                Try
                    fzgGrid.Visible = True
                    fzgGrid.DataSource = Nothing

                    Dim fin = txt_Fahrgestellnummer.Text.Trim
                    Dim zb2 = txt_ZBII.Text.Trim
                    m_UE.LoadData(Session("HAENDLER_EX"), fin, zb2, Me)

                    If (m_UE.Status <> 0) Then
                        Throw New Exception(m_UE.Message)
                    End If

                    fzgGrid.DataSource = m_UE.Result
                    If rebind Then fzgGrid.Rebind()

                    cmdNext.Enabled = Not m_UE.Result Is Nothing AndAlso m_UE.Result.Select("Selected").Length > 0

                Catch ex As Exception
                    fzgGrid.Visible = False
                    lblError.Text = ex.Message
                    lblError.Visible = True
                    lblNoData.Visible = False
                    cmdNext.Enabled = False
                End Try
            Case 1
                Try
                    selFzgGrid.Visible = True
                    selFzgGrid.DataSource = Nothing

                    Dim view = New DataView(m_UE.Result, "Selected", "CHASSIS_NUM", DataViewRowState.CurrentRows)
                    selFzgGrid.DataSource = view
                    If rebind Then selFzgGrid.Rebind()

                    cmdDone.Enabled = SelectedRowsHaveAUGRU()
                Catch ex As Exception
                    selFzgGrid.Visible = False
                    lblError.Text = ex.Message
                    lblError.Visible = True
                End Try
        End Select
    End Sub

    Private Function SelectedRowsHaveAUGRU() As Boolean
        Return Not m_UE.Result.Select("Selected").Any(Function(r) String.IsNullOrEmpty(r("AUGRU").ToString))
    End Function

    Protected Sub NextClick(ByVal sender As Object, ByVal e As EventArgs)
        views.ActiveViewIndex = 1
        LoadData()
    End Sub

    Protected Sub DoneClick(ByVal sender As Object, ByVal e As EventArgs)
        If SelectedRowsHaveAUGRU() Then
            For Each row In m_UE.Result.Select("Selected")
                row("Subject") = txt_Subject.Text
            Next

            Response.Redirect("Change11_2.aspx" + Request.Url.Query)
        End If
    End Sub

    Protected Overrides Sub OnUnload(e As System.EventArgs)
        MyBase.OnUnload(e)

        Common.SetEndASPXAccess(Me)
    End Sub

    Protected Overrides Sub OnPreRender(e As System.EventArgs)
        MyBase.OnPreRender(e)

        Common.TranslateTelerikColumns(fzgGrid, selFzgGrid)
        Common.SetEndASPXAccess(Me)
    End Sub

    'Protected Sub NavigateBack(ByVal sender As Object, ByVal e As EventArgs)
    '    If views.ActiveViewIndex = 1 Then
    '        views.ActiveViewIndex = 0
    '        LoadData()
    '    Else
    '        Session.Remove("UE")
    '        Session.Remove("HAENDLER_EX")
    '        Session.Remove("HAENDLER_NAME")
    '        Session.Remove("HAENDLER_ADDR")
    '        Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx")
    '    End If
    'End Sub

    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        If views.ActiveViewIndex = 1 Then
            views.ActiveViewIndex = 0
            LoadData()
        Else
            Session.Remove("UE")
            Session.Remove("HAENDLER_EX")
            Session.Remove("HAENDLER_NAME")
            Session.Remove("HAENDLER_ADDR")
            Response.Redirect("Change11.aspx?AppID=" & Session("AppID").ToString)
        End If
    End Sub

End Class