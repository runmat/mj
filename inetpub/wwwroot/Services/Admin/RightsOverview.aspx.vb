Option Explicit On
Option Strict On

Imports CKG
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Excel
Imports CKG.Base.Kernel.Common.Common


Partial Public Class RightsOverview
    Inherits System.Web.UI.Page

    Private m_App As Security.App
    Private m_User As Security.User
    Private mObjBapiOverView As BapiOverViewClass


    '''<summary>
    '''GridNavigation1-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents GridNavigation1 As Global.CKG.Services.GridNavigation


#Region "Properties"

    Private Property Refferer() As String
        Get
            If Not Session.Item(Me.Request.Url.LocalPath & "Refferer") Is Nothing Then
                Return Session.Item(Me.Request.Url.LocalPath & "Refferer").ToString()
            Else : Return Nothing
            End If
        End Get
        Set(ByVal value As String)
            Session.Item(Me.Request.Url.LocalPath & "Refferer") = value
        End Set
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GridNavigation1.setGridElment(RightsOverview)
        m_User = GetUser(Me)
        m_App = New Security.App(m_User) 'erzeugt ein App_objekt 
        AdminAuth(Me, m_User, Security.AdminLevel.Master)

        If Not IsPostBack Then
            If Refferer Is Nothing Then
                If Not Me.Request.UrlReferrer Is Nothing Then
                    Refferer = Me.Request.UrlReferrer.ToString
                Else
                    Refferer = ""
                End If
            End If

            GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString


        End If


        
        If Not IsPostBack Then

            fill_RightsTable(0)


        End If

    End Sub

    Private Function fill_RightsTable(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "") As DataTable

        Dim intTempPageIndex As Int32 = intPageIndex

        Dim cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Dim tblReturn As New DataTable()
        Dim sqlString As String = "SELECT application.AppName, application.AppURL , Customer.CName , WebGroup.GroupName FROM Application inner join Rights on application.AppID = rights.AppID inner join WebGroup on rights.GroupID = WebGroup.GroupID inner join Customer on WebGroup.CustomerID = Customer.CustomerID ORDER BY AppURL, CName , GroupName "
        If strSort <> "" Then
            sqlString = sqlString.Replace("ORDER BY AppURL, CName , GroupName", "ORDER BY " & strSort)
        End If
        Dim cmd As SqlClient.SqlCommand = New SqlClient.SqlCommand(sqlString, cn)


        Dim adContact As New SqlClient.SqlDataAdapter(cmd)

        Try
            cn.Open()
            adContact.Fill(tblReturn)

            'If intPageIndex <> "" Then
            RightsOverview.PageIndex = intTempPageIndex
            'Else
            'RightsOverview.PageIndex = 0
            'End If

            RightsOverview.DataSource = tblReturn
            RightsOverview.DataBind()


        Catch ex As Exception

        Finally
            cn.Close()
        End Try


        Return tblReturn
    End Function



    Private Sub responseBack()
        If Refferer = "" Then
            Dim strLinkPrefix As String = "/" & ConfigurationManager.AppSettings("WebAppPath") & "/"
            Response.Redirect(strLinkPrefix & "Start/Selection.aspx")
        Else
            Response.Redirect(Refferer)
        End If
    End Sub

    Private Sub ExportExcel(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkCreateExcel1.Click
        'Dieser Code wird abgearbeitet, wenn der Link "Als Excel downloaden" ausgeführt wird

        Dim tempTable As DataTable = fill_RightsTable(0, "")

        If Not RightsOverview.Rows.Count = 0 Then
            Dim objExcelExport As New Base.Kernel.Excel.ExcelExport()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            Try
                Base.Kernel.Excel.ExcelExport.WriteExcel(tempTable, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
            Catch
            End Try
            Response.Redirect("/Services/Temp/Excel/" & strFileName)
        End If
    End Sub


    Private Sub RightsOverview_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles RightsOverview.Sorting
        fill_RightsTable(RightsOverview.PageSize, e.SortExpression)
    End Sub

    Private Sub GridNavigation1_ddlPageSizeChanged() Handles GridNavigation1.PageSizeChanged
        fill_RightsTable(RightsOverview.PageSize)
    End Sub
    Private Sub RightsOverview_PageIndexChanged(ByVal pageindex As Int32) Handles GridNavigation1.PagerChanged
        fill_RightsTable(pageindex)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub lb_zurueck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_zurueck.Click
        responseBack()
    End Sub
End Class