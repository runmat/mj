Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Report_02
    Inherits System.Web.UI.Page
    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App

    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles
    Protected WithEvents tbAnwendungen As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents btnBack As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ucHeader As Header
#Region " Vom Web Form Designer generierter Code "

    'Dieser Aufruf ist für den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: Diese Methode ist für den Web Form-Designer erforderlich
        'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            DoSubmit()
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Session("ShowLink") = "True"
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        Dim sql As String
        Dim table As DataTable
        Dim command As New SqlClient.SqlCommand()
        Dim blnReturn As Boolean = True
        Dim intAppID As Int32



        sql = " SELECT "
        sql = sql & "   Application_1.AppID"
        sql = sql & " FROM "
        sql = sql & "   dbo.Application INNER JOIN"
        sql = sql & "   dbo.Application Application_1 ON dbo.Application.AppID = Application_1.AppParent"
        sql = sql & " WHERE "
        sql = sql & "   (dbo.Application.AppURL = '../Components/ComArchive/Report_02.aspx') AND"
        sql = sql & "   (Application_1.AppURL = '../Components/ComArchive/_Report00.aspx')"



        command.CommandText = sql

        Dim conn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Dim da As New SqlClient.SqlDataAdapter(command)
        command.Connection = conn
        conn.Open()
        table = New DataTable()
        da.Fill(table)

        If table.Rows.Count > 0 Then


            intAppID = table.Rows(0).Item("AppID")


            table = Nothing

        End If


        sql = " SELECT     EasyLagerortName"
        sql = sql & " FROM         dbo.Archiv"
        sql = sql & " GROUP BY EasyLagerortName"
        sql = sql & " HAVING      (EasyLagerortName <> N'FORD')"
        sql = sql & " Union"
        sql = sql & " SELECT     EasyLagerortName + ' ' + substring(EasyArchivname,1,3) as EasyLagerortName"
        sql = sql & " FROM         dbo.Archiv"
        sql = sql & " GROUP BY EasyLagerortName,EasyArchivname"
        sql = sql & " HAVING      (EasyLagerortName = 'FORD')"


        command.CommandText = sql
        table = New DataTable()
        da.Fill(table)


        Try
            If table.Rows.Count > 0 Then
                Dim tmpRow As DataRow
                For Each tmpRow In table.Rows
                    Dim trApp As HtmlTableRow
                    Dim tdApp1 As HtmlTableCell
                    Dim tdApp2 As HtmlTableCell
                    Dim litApp As Literal

                    trApp = New HtmlTableRow()
                    tdApp1 = New HtmlTableCell()
                    tdApp1.Attributes.Add("class", "TextLarge")
                    tdApp1.Align = "left"
                    tdApp1.Width = "100"
                    tdApp1.ColSpan = "2"
                    litApp = New Literal()
                    litApp.Text = "&nbsp;"
                    tdApp1.Controls.Add(litApp)
                    trApp.Cells.Add(tdApp1)
                    tdApp2 = New HtmlTableCell()
                    tdApp2.Attributes.Add("class", "TextLarge")
                    tdApp2.VAlign = "left"
                    tdApp2.Width = "100%"
                    Dim blnAlternate As Boolean = False
                    litApp = New Literal()
                    litApp.Text = "<table cellspacing=""0"" border=""0"" style=""width:100%;border-collapse:collapse;"">" & vbCrLf

                    Dim appUrl As String
                    Dim appSymbol As String

                    appUrl = "../../Components/ComArchive/_Report00.aspx?AppID=" & CStr(intAppID) & "&ELN=" & CStr(tmpRow("EasyLagerortName"))

                    '--------------------------------------
                    appSymbol = "../../Images/arrowgrey.gif"
                    litApp.Text &= "<td><img src=""" & appSymbol & """ border=""0"" /></td>" & vbCrLf
                    litApp.Text &= "<td class=""MainmenuItem"" nowrap=""nowrap"">&nbsp;<a href=""" & appUrl & """ target=""_self"">" & CStr(tmpRow("EasyLagerortName")) & "</a>&nbsp;</td>" & vbCrLf

                    '---------------------------------------------
                    litApp.Text &= "<td class=""MainmenuItemComment"" style=""width:100%;"">&nbsp;&nbsp;<span class=""MainmenuItemComment""></span></td>" & vbCrLf

                    litApp.Text &= "</tr>" & vbCrLf

                    litApp.Text &= "</table>" & vbCrLf
                    tdApp2.Controls.Add(litApp)
                    trApp.Cells.Add(tdApp2)
                    tbAnwendungen.Rows.Add(trApp)
                Next
            End If
            conn.Close()
            conn.Dispose()
            da.Dispose()

        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "Selection", "Page_Load", ex.ToString)

            lblError.Text = "Fehler bei der Ermittlung der Menüpunkte (" & ex.Message & ")"
            lblError.Visible = True
        End Try
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Response.Redirect("Selection.aspx?AppID=" & Session("AppID").ToString)
    End Sub
End Class

' ************************************************
' $History: Report_02.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 4.05.09    Time: 11:22
' Updated in $/CKAG/Components/ComArchive
' ITA:2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:29
' Created in $/CKAG/Components/ComArchive
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:03
' Created in $/CKAG/Components/ComArchive/inetpub/wwwroot/Portal/Components/ComArchive
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 9.10.07    Time: 18:12
' Created in $/CKG/Components/ComArchive/ComArchiveWeb
' 
