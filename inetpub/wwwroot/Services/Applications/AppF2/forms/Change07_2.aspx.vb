Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Business

Partial Public Class Change07_2
    Inherits System.Web.UI.Page

#Region "Declarations"
    Private m_App As App
    Private m_User As User
    Private objSuche As AppF2.Search
    Private objAppChange07 As TempZuEndg
    Private objHaendler As AppF2BankBaseCredit
    Dim Aut As Boolean = False
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)
        GridNavigation1.setGridElment(GridView1)
        m_App = New App(m_User)
        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

        lnkKreditlimit.NavigateUrl = "Change07.aspx?AppID=" & Session("AppID").ToString

        If objAppChange07 Is Nothing Then
            objAppChange07 = CType(Session("AppHaendler07"), TempZuEndg)
        End If


        If Request.QueryString("Linked") = 1 Then
            lnkKreditlimit.Text = "Briefversand"
            lnkKreditlimit.NavigateUrl = Session.Item("URLReferenz")
            lblHead.Text = "Änderung Versandstatus (nachträglich endgültig)"
        End If

        'zurücklinken zu Fällige Vorgänge
        If Request.QueryString("Linked") = 2 Then
            lnkKreditlimit.Text = "Fällige Vorgänge"
            'lnkKreditlimit.NavigateUrl = "Change41_1.aspx?AppID=" & Session("AppID").ToString & "&Linked=2"
            lnkKreditlimit.NavigateUrl = Session.Item("URLReferenz")
            lblHead.Text = "Änderung Versandstatus (nachträglich endgültig)"
        End If


        If Session("AppHaendler07") Is Nothing AndAlso Aut = False Then
            Response.Redirect("Change07.aspx?AppID=" & Session("AppID").ToString)
        End If

        If Session("objNewHaendlerSuche") Is Nothing AndAlso Aut = False Then
            Response.Redirect("Change07.aspx?AppID=" & Session("AppID").ToString)
        Else
            objSuche = CType(Session("objNewHaendlerSuche"), AppF2.Search)
        End If
        If objAppChange07.Customer Is Nothing OrElse objAppChange07.Customer.Trim Is String.Empty Then
            Kopfdaten1.Visible = False
        Else
            Kopfdaten1.Visible = True

            If Not objSuche.LeseHaendlerSAP_Einzeln(Session("AppID").ToString, Session.SessionID.ToString, objAppChange07.Customer, Me) Then
                lblError.Text = "Fehler bei der Ermittlung der Händlerdaten.<br>(" & objSuche.ErrorMessage & ")"
            Else
                Session("objSuche") = objSuche
            End If

            Kopfdaten1.UserReferenz = m_User.Reference
            Kopfdaten1.HaendlerNummer = objSuche.REFERENZ
            Dim strTemp As String = objSuche.NAME
            If objSuche.NAME_2.Length > 0 Then
                strTemp &= "<br>" & objSuche.NAME_2
            End If
            Kopfdaten1.HaendlerName = strTemp
            Kopfdaten1.Adresse = objSuche.COUNTRYISO & " - " & objSuche.POSTL_CODE & " " & objSuche.CITY & "<br>" & objSuche.STREET
            Session("objNewHaendlerSuche") = objSuche
        End If




        If Not IsPostBack Then

            objHaendler = New AppF2BankBaseCredit(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
            objHaendler.Customer = objSuche.REFERENZ
            objHaendler.KUNNR = m_User.KUNNR
            objHaendler.CreditControlArea = "ZDAD"
            objHaendler.Show(Session("AppID").ToString, Session.SessionID, Me)
            Kopfdaten1.Kontingente = objHaendler.Kontingente
            FillGrid(0)
            Session("App_objHaendler") = objHaendler
        Else
            objHaendler = CType(Session("App_objHaendler"), AppF2BankBaseCredit)
            Kopfdaten1.Kontingente = objHaendler.Kontingente
        End If

        'For Each row As GridViewRow In GridView1.Rows
        '    row.Cells(5).Attributes.Add("onchange", "javascript:linebreaker(this);")

        'Next

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
        HelpProcedures.FixedGridViewCols(GridView1)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
#End Region

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")

        If objAppChange07 Is Nothing Then
            objAppChange07 = CType(Session("AppHaendler07"), TempZuEndg)
        End If
        If Not objAppChange07.Fahrzeuge Is Nothing Then
            If objAppChange07.Status = 0 Then
                If objAppChange07.Fahrzeuge.Rows.Count = 0 Then
                    'trDataGrid1.Visible = False
                    'trPageSize.Visible = False
                    lblNoData.Visible = True
                    lblNoData.Text = "Keine Daten zur Anzeige gefunden."
                    Result.Visible = False
                Else
                    Result.Visible = True
                    lblNoData.Visible = False

                    Dim tmpDataView As New DataView(objAppChange07.Fahrzeuge)
                    If Aut Then
                        tmpDataView.RowFilter = "Fahrgestellnummer='" & Request.QueryString("FIN") & "'"
                    End If


                    Dim intTempPageIndex As Int32 = intPageIndex
                    Dim strTempSort As String = ""
                    Dim strDirection As String = ""

                    If strSort.Trim(" "c).Length > 0 Then
                        intTempPageIndex = 0
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


                    If objHaendler Is Nothing Then
                        objHaendler = CType(Session("App_objHaendler"), AppF2BankBaseCredit)
                    End If
                    objHaendler.Show(Session("AppID").ToString, Session.SessionID, Me)
                    Kopfdaten1.Kontingente = objHaendler.Kontingente

                End If
            Else
                lblError.Text = objAppChange07.Message
                lblNoData.Visible = True
                ShowScript.Visible = False
            End If
        Else
            lblNoData.Text = "Keine Daten zur Anzeige gefunden."
            Result.Visible = False
            lblNoData.Visible = True
        End If
    End Sub



    Private Sub GridView1_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridView1.RowEditing


        Dim txtFleet20 As TextBox = CType(GridView1.Rows(e.NewEditIndex).Cells(6).FindControl("txtFleet20"), TextBox)
        Dim txtFleet21 As TextBox = CType(GridView1.Rows(e.NewEditIndex).Cells(7).FindControl("txtFleet21"), TextBox)
        Dim lblEqunr As Label = CType(GridView1.Rows(e.NewEditIndex).Cells(0).FindControl("lblEqunr"), Label)
        Dim lblVertragsnummer As Label = CType(GridView1.Rows(e.NewEditIndex).Cells(3).FindControl("Label1"), Label)
        If objAppChange07 Is Nothing Then
            objAppChange07 = CType(Session("AppHaendler07"), TempZuEndg)
        End If

        objAppChange07.EQUNR = lblEqunr.Text
        objAppChange07.Fleet20 = txtFleet20.Text
        objAppChange07.Fleet21 = txtFleet21.Text


        If objAppChange07.AnBank = True Then
            objAppChange07.ChangeBank(Session("AppID").ToString, Session.SessionID, Me)
        Else
            objAppChange07.Change(Session("AppID").ToString, Session.SessionID, Me)
        End If


        If Not objAppChange07.Status = 0 Then
            lblError.Text = objAppChange07.Message
            lblError.Visible = True
            Exit Sub
        End If

        Dim sMsg As String = ""
        If objAppChange07.Message.Length > 0 Then
            sMsg = objAppChange07.Message
        Else
            sMsg = "Vorgang OK"
        End If

        'logging
        Dim logApp As New CKG.Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        logApp.CollectDetails("Vertragsnummer", CType(lblVertragsnummer.Text, Object), True)
        logApp.CollectDetails("Versanddatum", CType(GridView1.Rows(e.NewEditIndex).Cells(3).Text, Object))
        logApp.CollectDetails("Memo 1", CType(objAppChange07.Fleet20, Object))
        logApp.CollectDetails("Memo 2", CType(objAppChange07.Fleet21, Object))
        logApp.CollectDetails("Kommentar", CType(sMsg, Object))

        logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)


        If objAppChange07.Status = 0 Then
            Dim lblFleet20 As Label = CType(GridView1.Rows(e.NewEditIndex).Cells(6).FindControl("lblFleet20"), Label)
            Dim lblFleet21 As Label = CType(GridView1.Rows(e.NewEditIndex).Cells(7).FindControl("lblFleet21"), Label)
            Dim oLabel As Label = CType(GridView1.Rows(e.NewEditIndex).Cells(8).FindControl("lblStatus"), Label)
            Dim oLinkButton As ImageButton = CType(GridView1.Rows(e.NewEditIndex).Cells(8).FindControl("lbStatus"), ImageButton)
            txtFleet20.Visible = False
            txtFleet21.Visible = False
            oLinkButton.Visible = False
            lblFleet21.Visible = True
            lblFleet20.Visible = True
            oLabel.Visible = True

            objAppChange07.EQUNR = lblEqunr.Text
            lblFleet20.Text = txtFleet20.Text
            lblFleet21.Text = txtFleet21.Text
            oLabel.Text = sMsg
            logApp.UpdateEntry("APP", Session("AppID").ToString, "Änderung Versandstatus", logApp.InputDetails)

        Else
            logApp.UpdateEntry("ERR", Session("AppID").ToString, "Änderung Versandstatus: Speichern fehlgeschlagen", logApp.InputDetails)
        End If
    End Sub


    Private Sub GridView1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
        FillGrid(GridView1.PageIndex)
    End Sub

    Private Sub GridNavigation1_ddlPageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillGrid(0)
    End Sub
    Private Sub GridView1_PageIndexChanged(ByVal pageindex As Int32) Handles GridNavigation1.PagerChanged
        GridView1.EditIndex = -1
        FillGrid(pageindex)
    End Sub

End Class
