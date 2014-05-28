Imports CKG.Base.Kernel.Common
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Logging
Imports Telerik.Web.UI

Public Class Report14
    Inherits System.Web.UI.Page

    Dim m_User As CKG.Base.Kernel.Security.User
    Dim m_App As App
    Dim m_M As Mahnungen

    Protected Overrides Sub OnLoad(e As System.EventArgs)
        MyBase.OnLoad(e)

        m_User = Common.GetUser(Me)
        Common.FormAuth(Me, m_User)
        m_App = New App(m_User)
        Common.GetAppIDFromQueryString(Me)
        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

        If Not IsPostBack Then
            Dim search = New DealerSearch(m_User, m_App, Session("AppID"), Session.SessionID)
            search.LoadData(Me, m_User.Reference)

            If search.Result Is Nothing OrElse search.Result.Rows.Count <> 1 Then
                lblError.Text = "Händler konnte nicht gefunden werden."
                lblError.Visible = True
                Exit Sub
            End If

            Dim r = search.Result.Rows.Cast(Of DataRow).First

            Dim haendler = CStr(r("HAENDLER"))
            If String.IsNullOrEmpty(haendler) Then Throw New Exception("Haendler nicht gesetzt.")

            Session("HAENDLER_EX") = CType(r("HAENDLER_EX"), String)
            Dim name1 = CType(r("NAME1"), String)
            Dim name2 = CType(r("NAME2"), String)
            Session("HAENDLER_NAME") = IIf(String.IsNullOrEmpty(name2), name1, name1 & "<br />" & name2)
            Session("HAENDLER_ADDR") = String.Format("{0} - {1} {2}<br />{3}", r("LAND1"), r("PSTLZ"), r("ORT01"), r("STRAS"))
            Session("HAENDLER") = haendler
        End If

        m_M = Common.GetOrCreateObject("M&M", Function() New Mahnungen(m_User, m_App, Session("AppID"), Session.SessionID))

        Common.TranslateTelerikColumns(mahnGrid)
    End Sub

    Protected Overrides Sub OnPreRender(e As System.EventArgs)
        MyBase.OnPreRender(e)

        LockControls(Not cmdNext.Enabled)
        lbBack.Enabled = True
        Common.SetEndASPXAccess(Me)
    End Sub

    Private Sub LoadData(Optional ByVal rebind As Boolean = True)
        If m_M.Result Is Nothing Then
            m_M.LoadData(Me, Session("HAENDLER_EX"))
        End If

        mahnGrid.DataSource = m_M.Result

        If Not m_M.Result Is Nothing Then
            Dim c = m_M.Result.Rows.Count
            If c = 0 Then
                lblError.Text = "Es liegen keine Mahnungen vor."
                lbBack.Visible = True
                lbBack.Enabled = True
                ' Done()
            ElseIf Math.Ceiling(c / mahnGrid.PageSize) = 1 Then
                EnableNextButton()
            End If
        End If

        If rebind Then mahnGrid.Rebind()
    End Sub

    Protected Sub GridNeedDataSource(ByVal sender As Object, ByVal e As GridNeedDataSourceEventArgs)
        If Not m_M Is Nothing Then
            LoadData(False)
        End If
    End Sub

    Protected Sub GridPageIndexChanged(ByVal sender As Object, ByVal e As GridPageChangedEventArgs)
        If e.NewPageIndex = mahnGrid.PageCount - 1 Then
            EnableNextButton()
        End If
    End Sub

    Private Sub EnableNextButton()
        cmdNext.Enabled = True
    End Sub

    Private Sub LockControls(Optional ByVal lock As Boolean = True, Optional ByVal control As Control = Nothing)
        If control Is Nothing Then control = Me

        Dim ignoreControls As Control() = {cmdNext, mahnGrid}
        Dim ignoreID = "lnkLogout"

        If Array.IndexOf(ignoreControls, control) <> -1 Then Return
        If ignoreID = control.ID Then Return

        If TypeOf control Is WebControl AndAlso Not TypeOf control Is Label Then
            CType(control, WebControl).Enabled = Not lock
        End If

        For Each subControl In control.Controls
            LockControls(lock, subControl)
        Next
    End Sub

    Protected Function MahnArtText(ByVal kuerzel As String) As String
        Select Case kuerzel
            Case "WE"
                Return "Wiedereingang"
            Case "ZE"
                Return "Zahlungseingang"
            Case Else
                Return "unbekannt"
        End Select
    End Function

    Protected Sub NextClick(ByVal sender As Object, ByVal e As EventArgs)
        Done()
    End Sub

    Private Sub Done()
        Session.Remove("M&M")
        Log("Mahnungen gesehen und mit ""OK"" bestätigt.")

        Response.Redirect("../../../Start/Selection.aspx")
    End Sub

    Private Sub Log(ByVal strDescription As String, Optional ByVal strCategory As String = "APP")
        Dim logApp = New Trace(m_User.App.Connectionstring, m_User.App.SaveLogAccessSAP, m_User.App.LogLevel)

        Dim strUserName As String = m_User.UserName ' strUserName
        Dim strSessionID As String = Session.SessionID ' strSessionID
        Dim intSource As Integer = CInt(Session("AppID")) ' intSource 
        Dim strTask As String = lblHead.Text ' strTask
        Dim strIdentification As String = m_User.Reference
        ' strDescription
        Dim strCustomerName As String = m_User.CustomerName ' strCustomername
        Dim blnIsTestUser As Boolean = m_User.IsTestUser ' blnIsTestUser
        Dim intSeverity As Integer = 0 ' intSeverity 

        logApp.WriteEntry(strCategory, strUserName, strSessionID, intSource, strTask, strIdentification, strDescription, strCustomerName, m_User.Customer.CustomerId, blnIsTestUser, intSeverity)
    End Sub

    Protected Sub lbBack_Click(sender As Object, e As EventArgs) Handles lbBack.Click
        Response.Redirect("../../../Start/Selection.aspx", False)
    End Sub
End Class