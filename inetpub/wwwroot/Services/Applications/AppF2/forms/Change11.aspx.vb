Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common
Imports CKG.Base.Business


Public Class Change11
    Inherits System.Web.UI.Page

    Private m_User As User
    Private m_App As App
    Private m_DS As DealerSearch

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        m_User = Common.GetUser(Me)
        Common.FormAuth(Me, m_User)
        m_App = New App(m_User)
        Common.GetAppIDFromQueryString(Me)
        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

        m_DS = Common.GetOrCreateObject("DealerSearch", Function() New DealerSearch(m_User, m_App, Session("AppID"), Session.SessionID))

        If Not IsPostBack Then
            txtNummer.Text = m_User.Reference.Trim
            UpdateSearch()
        End If

        If Not String.IsNullOrEmpty(m_User.Reference) Then
            NextStep(m_User.Reference)
        End If


    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Common.SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        Common.SetEndASPXAccess(Me)
    End Sub

    Protected Sub searchChanged(ByVal sender As Object, ByVal e As EventArgs)
        UpdateSearch()
    End Sub

    Private Sub UpdateSearch()
        Dim result = m_DS.FilterResult(txtNummer.Text, txtName1.Text, txtName2.Text, txtPLZ.Text, txtOrt.Text, Me)

        If Not m_DS.Result Is Nothing Then
            lblErgebnissAnzahl.Text = result.Count
            lbHaendler.DataSource = result
            tr_HaendlerAuswahl.Visible = True
        Else
            lblErgebnissAnzahl.Text = String.Empty
            lbHaendler.DataSource = Nothing
            tr_HaendlerAuswahl.Visible = False
        End If

        lbHaendler.DataTextField = "DISPLAY"
        lbHaendler.DataValueField = "HAENDLER_EX"
        lbHaendler.DataBind()

        SyncSelection()
    End Sub

    Private Sub SyncSelection()
        Dim haendlerNr = lbHaendler.SelectedValue
        Dim haendlerRow = m_DS.Result.Select("HAENDLER_EX = '" & haendlerNr & "'").FirstOrDefault

        If Not haendlerRow Is Nothing Then
            lblHaendlerDetailsNR.Text = haendlerNr
            lblHaendlerDetailsName1.Text = haendlerRow("NAME1")
            lblHaendlerDetailsName2.Text = haendlerRow("NAME2")
            lblHaendlerDetailsStrasse.Text = haendlerRow("STRAS")
            lblHaendlerDetailsPLZ.Text = haendlerRow("PSTLZ")
            lblHaendlerDetailsOrt.Text = haendlerRow("ORT01")
            lbNext.Enabled = True
        Else
            lblHaendlerDetailsNR.Text = "no selection"
            lblHaendlerDetailsName1.Text = ""
            lblHaendlerDetailsName2.Text = ""
            lblHaendlerDetailsPLZ.Text = ""
            lblHaendlerDetailsOrt.Text = ""
            lblHaendlerDetailsStrasse.Text = ""
            lbNext.Enabled = False
        End If
    End Sub

    Protected Sub selected(ByVal sender As Object, ByVal e As EventArgs)
        SyncSelection()
    End Sub

    Protected Sub nextClick(ByVal sender As Object, ByVal e As EventArgs)
        NextStep(lbHaendler.SelectedValue)
    End Sub

    Private Sub NextStep(haendlerNr As String)
        haendlerNr = haendlerNr.Trim

        If (String.IsNullOrEmpty(haendlerNr)) Then Return

        Dim haendlerRows = m_DS.Result.Select("HAENDLER_EX = '" & haendlerNr & "'").ToList

        If haendlerRows.Count = 1 Then
            Dim r = haendlerRows(0)

            Dim haendler = CStr(r("HAENDLER"))
            If String.IsNullOrEmpty(haendler) Then Throw New Exception("Haendler nicht gesetzt.")

            Session("HAENDLER_EX") = CType(r("HAENDLER_EX"), String)
            Dim name1 = CType(r("NAME1"), String)
            Dim name2 = CType(r("NAME2"), String)
            Session("HAENDLER_NAME") = IIf(String.IsNullOrEmpty(name2), name1, name1 & "<br />" & name2)
            Session("HAENDLER_ADDR") = String.Format("{0} - {1} {2}<br />{3}", r("LAND1"), r("PSTLZ"), r("ORT01"), r("STRAS"))
            Session("HAENDLER") = haendler
            Session.Remove("DealerSearch")
            Response.Redirect("Change11_1.aspx" + Request.Url.Query)
        End If
    End Sub

    Protected Sub resetClick(ByVal sender As Object, ByVal e As EventArgs)
        txtNummer.Text = String.Empty
        txtName1.Text = String.Empty
        txtName2.Text = String.Empty
        txtPLZ.Text = String.Empty
        txtOrt.Text = String.Empty

        m_DS.LoadData(Me)
        UpdateSearch()
    End Sub

    'Protected Sub NavigateBack(ByVal sender As Object, ByVal e As EventArgs)
    '    Session.Remove("DealerSearch")
    '    Session.Remove("HAENDLER_EX")
    '    Session.Remove("HAENDLER_NAME")
    '    Session.Remove("HAENDLER_ADDR")
    '    Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx")
    'End Sub

    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        Session.Remove("DealerSearch")
        Session.Remove("HAENDLER_EX")
        Session.Remove("HAENDLER_NAME")
        Session.Remove("HAENDLER_ADDR")
        Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx")
    End Sub
End Class